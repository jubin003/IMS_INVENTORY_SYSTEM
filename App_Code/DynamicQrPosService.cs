using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using Entity.Components;
using Service.Components;
using Entity.Framework;
using System.Web.Hosting;


public static class DynamicQrPosService
{
    

    public class QrResult
    {
        public bool Success;
        public string QrImageBase64;
        public string QrString;
        public string ValidationTraceId;
        public string ErrorMessage;
    }

    public class PaymentStatusResult
    {
        public string Status;  // NONE | PENDING | SUCCESS | FAILED
        public string Message;
        public string TransactionId;
        public string QrType;
    }

    private class TransactionRecord
    {
        public string BillNumber;
        public string ValidationTraceId;
        public string Status = "PENDING";
        public string Message = "Waiting for payment...";
        public string Amount;
        public string TransactionId;
        public string QrType = "NepalPay";
        public DateTime CreatedUtc = DateTime.UtcNow;
        public int ResendAttempts = 0;
        public DateTime LastResendUtc = DateTime.MinValue;
    }

    private static readonly ConcurrentDictionary<string, TransactionRecord> _byBillNumber =
        new ConcurrentDictionary<string, TransactionRecord>();
    private static readonly ConcurrentDictionary<string, TransactionRecord> _byTraceId =
        new ConcurrentDictionary<string, TransactionRecord>();

    private static DYNAMIC_QR GetSettings()
    {
        DYNAMIC_QRService DQRSer = new DYNAMIC_QRService();
        EntityList settingsList = DQRSer.GetAll(new DYNAMIC_QR());
        return (DYNAMIC_QR)settingsList[0];
    }

    public static string GenerateBillNumber()
    {
        string ticks = DateTime.UtcNow.Ticks.ToString();
        return ticks.Substring(ticks.Length - 12);
    }

    public static void RegisterTransaction(string billNumber, string validationTraceId, string amount)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-30);
        foreach (var kvp in _byBillNumber)
        {
            if (kvp.Value.CreatedUtc < cutoff)
            {
                TransactionRecord removed;
                _byBillNumber.TryRemove(kvp.Key, out removed);
                if (removed != null && !string.IsNullOrEmpty(removed.ValidationTraceId))
                {
                    TransactionRecord removed2;
                    _byTraceId.TryRemove(removed.ValidationTraceId, out removed2);
                }
            }
        }

        var record = new TransactionRecord
        {
            BillNumber = billNumber,
            ValidationTraceId = validationTraceId,
            Amount = amount
        };
        _byBillNumber[billNumber] = record;
        if (!string.IsNullOrEmpty(validationTraceId))
            _byTraceId[validationTraceId] = record;
    }

    public static PaymentStatusResult CheckPaymentStatus(string billNumber)
    {
        if (string.IsNullOrEmpty(billNumber))
            return new PaymentStatusResult { Status = "NONE", Message = "No active transaction." };

        TransactionRecord record;
        if (!_byBillNumber.TryGetValue(billNumber, out record))
            return new PaymentStatusResult { Status = "NONE", Message = "Transaction expired or not found." };

        return new PaymentStatusResult
        {
            Status = record.Status,
            Message = record.Message,
            TransactionId = record.TransactionId,
            QrType = record.QrType
        };
    }

    // ==================== WebSocket / STOMP ====================

    private static ClientWebSocket _wsClient;
    private static CancellationTokenSource _wsCts;
    private static readonly SemaphoreSlim _wsConnectLock = new SemaphoreSlim(1, 1);
    private static readonly SemaphoreSlim _wsSendLock = new SemaphoreSlim(1, 1);

    public static async Task EnsureWebSocketConnectedAsync()
    {
        if (_wsClient != null && _wsClient.State == WebSocketState.Open)
            return;

        bool acquired = await _wsConnectLock.WaitAsync(TimeSpan.FromSeconds(20));
        if (!acquired)
            throw new TimeoutException("Timed out waiting for the WebSocket connect lock.");
        try
        {
            if (_wsClient != null && _wsClient.State == WebSocketState.Open)
                return;

            DYNAMIC_QR DQEnt = GetSettings();

            string wsUrl = DQEnt.NPI_WSURL;
            if (string.IsNullOrEmpty(wsUrl))
                throw new InvalidOperationException("NPI_WSURL is not configured in the database.");

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            if (_wsCts != null)
            {
                try { _wsCts.Cancel(); } catch { }
                try { _wsCts.Dispose(); } catch { }
            }
            _wsCts = new CancellationTokenSource();

            if (_wsClient != null)
            {
                try { _wsClient.Dispose(); } catch { }
            }
            _wsClient = new ClientWebSocket();

            LogWebhook("WS connecting to " + wsUrl);
            try
            {
                using (var connectTimeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_wsCts.Token))
                {
                    connectTimeoutCts.CancelAfter(TimeSpan.FromSeconds(15));
                    await _wsClient.ConnectAsync(new Uri(wsUrl), connectTimeoutCts.Token);
                }
            }
            catch (Exception connEx)
            {
                string chain = connEx.Message;
                var innerEx = connEx.InnerException;
                while (innerEx != null) { chain += " -> " + innerEx.Message; innerEx = innerEx.InnerException; }
                LogWebhook("WS ConnectAsync FAILED for " + wsUrl + " : " + chain);
                throw;
            }
            LogWebhook("WS connected. Starting STOMP handshake...");

            await StompConnectAsync();

            await StompSendFrameAsync("SUBSCRIBE", new[]
            {
                new KeyValuePair<string, string>("id", "sub-0"),
                new KeyValuePair<string, string>("destination", "/user/nqrws/check-txn-status"),
            }, null);
            LogWebhook("WS subscribed to /user/nqrws/check-txn-status");

            if (!_wsWorkerRegistered)
            {
                HostingEnvironment.RegisterObject(_wsWorker);
                _wsWorkerRegistered = true;
            }
            Task.Run(() => WsReceiveLoopAsync(_wsCts.Token));
            Task.Run(() => WsHeartbeatLoopAsync(_wsCts.Token));

            await ResendPendingTransactionRequestsAsync();
        }
        finally
        {
            _wsConnectLock.Release();
        }
    }


    private class WsBackgroundWorker : IRegisteredObject
    {
        public void Stop(bool immediate)
        {
            try { if (_wsCts != null) _wsCts.Cancel(); } catch { }
            HostingEnvironment.UnregisterObject(this);
        }
    }

    private static readonly WsBackgroundWorker _wsWorker = new WsBackgroundWorker();
    private static bool _wsWorkerRegistered = false;

    private const int MaxResendAttempts = 5;
    private static readonly TimeSpan MinResendSpacing = TimeSpan.FromSeconds(10);

    private static async Task ResendPendingTransactionRequestsAsync()
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-30);
        foreach (var kvp in _byTraceId)
        {
            var record = kvp.Value;
            if (record.Status != "PENDING" || record.CreatedUtc < cutoff)
                continue;

            if (record.ResendAttempts >= MaxResendAttempts)
            {
                if (record.ResendAttempts == MaxResendAttempts)
                {
                    // Log once, then bump past the threshold so we don't re-log every reconnect.
                    record.ResendAttempts++;
                    LogWebhook("WS giving up on resending check-txn-status for request_id=" + kvp.Key +
                               " after " + MaxResendAttempts + " attempts -- leaving as PENDING for manual/timeout handling.");
                }
                continue;
            }

            if (DateTime.UtcNow - record.LastResendUtc < MinResendSpacing)
                continue; // don't hammer the server faster than MinResendSpacing per pending transaction

            try
            {
                record.ResendAttempts++;
                record.LastResendUtc = DateTime.UtcNow;
                await SendTransactionDetailRequestOnOpenSocketAsync(kvp.Key);
                LogWebhook("WS resent check-txn-status for request_id=" + kvp.Key + " after (re)connect (attempt " + record.ResendAttempts + ")");
            }
            catch (Exception ex) { LogWebhook("WS resend failed for " + kvp.Key + ": " + ex.Message); }
        }
    }

    public static async Task WsSendTransactionDetailRequestAsync(string validationTraceId)
    {
        await EnsureWebSocketConnectedAsync();
        await SendTransactionDetailRequestOnOpenSocketAsync(validationTraceId);
    }

    private static async Task SendTransactionDetailRequestOnOpenSocketAsync(string validationTraceId)
    {
      

        DYNAMIC_QR DQEnt = GetSettings();

        string merchantId = DQEnt.NPI_TERMINALLABEL;
        string username = DQEnt.NPI_WSUSERNAME;
        string rawApiToken = DQEnt.NPI_WSAPITOKEN;
        string publicKeyBase64OrPath = DQEnt.NPI_NCHLPUBLICKEY;

        string encryptedApiToken = EncryptApiToken(rawApiToken, publicKeyBase64OrPath);

        int rawApiTokenLen = rawApiToken != null ? rawApiToken.Length : -1;
        int encryptedApiTokenLen = encryptedApiToken != null ? encryptedApiToken.Length : -1;
        string encryptedApiTokenPreview = (encryptedApiToken != null && encryptedApiToken.Length > 24)
            ? encryptedApiToken.Substring(0, 12) + "..." + encryptedApiToken.Substring(encryptedApiToken.Length - 12)
            : encryptedApiToken;

        LogWebhook("WS check-txn-status payload diagnostics: merchant_id='" + merchantId +
                   "' username='" + username +
                   "' request_id='" + validationTraceId +
                   "' rawApiToken.Length=" + rawApiTokenLen +
                   " encryptedApiToken.Length=" + encryptedApiTokenLen +
                   " encryptedApiToken.Preview='" + encryptedApiTokenPreview + "'");

        var payload = new JObject();
        payload.Add("merchant_id", merchantId);
        payload.Add("request_id", validationTraceId);
        payload.Add("username", username);
        payload.Add("api_token", encryptedApiToken);

        string body = payload.ToString(Newtonsoft.Json.Formatting.None);

        await StompSendFrameAsync("SEND", new[]
        {
            new KeyValuePair<string, string>("destination", "/nqrws/check-txn-status"),
            new KeyValuePair<string, string>("content-length", Encoding.UTF8.GetByteCount(body).ToString()),
        }, body);

        LogWebhook("WS sent check-txn-status for request_id=" + validationTraceId);
    }

    private static async Task StompConnectAsync()
    {
        await StompSendFrameAsync("CONNECT", new[]
        {
            new KeyValuePair<string, string>("accept-version", "1.1,1.0"),
            new KeyValuePair<string, string>("heart-beat", "10000,10000"),
        }, null);

        LogWebhook("STOMP CONNECT frame sent.");
    }

    // The CONNECT frame promises "heart-beat:10000,10000" -- i.e. this client
    // guarantees it will send a heartbeat at least every 10s. Without actually
    // doing that, brokers (or proxies in front of them) that enforce that
    // contract will silently tear down the connection once a QR sits open for
    // more than ~10-20s waiting on the customer to scan and pay. The client's
    // ClientWebSocket.State can still report "Open" for a while after that
    // (a stale socket often accepts one more SendAsync without erroring), so
    // SendTransactionDetailRequestOnOpenSocketAsync can appear to succeed
    // (its "WS sent check-txn-status..." log line fires) even though the
    // broker never actually receives/answers it -- explaining a SEND with no
    // matching WS RECV ever showing up in the log.
    private const int HeartbeatIntervalMs = 8000; // a little under the promised 10000ms for safety margin

    private static async Task WsHeartbeatLoopAsync(CancellationToken token)
    {
        try
        {
            while (_wsClient != null && _wsClient.State == WebSocketState.Open && !token.IsCancellationRequested)
            {
                await Task.Delay(HeartbeatIntervalMs, token);

                if (_wsClient == null || _wsClient.State != WebSocketState.Open || token.IsCancellationRequested)
                    break;

                await _wsSendLock.WaitAsync(token);
                try
                {
                    // STOMP heartbeat = a single newline byte, sent outside of any frame.
                    byte[] heartbeatBytes = Encoding.UTF8.GetBytes("\n");
                    using (var sendTimeoutCts = CancellationTokenSource.CreateLinkedTokenSource(token))
                    {
                        sendTimeoutCts.CancelAfter(TimeSpan.FromSeconds(10));
                        await _wsClient.SendAsync(new ArraySegment<byte>(heartbeatBytes), WebSocketMessageType.Text, true, sendTimeoutCts.Token);
                    }
                }
                finally { _wsSendLock.Release(); }
            }
        }
        catch (OperationCanceledException) { /* expected on shutdown/reconnect */ }
        catch (Exception ex) { LogWebhook("WS heartbeat loop EXCEPTION: " + ex.Message); }
    }

    private static async Task StompSendFrameAsync(string command, IEnumerable<KeyValuePair<string, string>> headers, string body)
    {
        var sb = new StringBuilder();
        sb.Append(command).Append('\n');
        foreach (var h in headers)
            sb.Append(h.Key).Append(':').Append(h.Value).Append('\n');
        sb.Append('\n');
        if (!string.IsNullOrEmpty(body)) sb.Append(body);
        sb.Append('\0');

        await _wsSendLock.WaitAsync();
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            using (var sendTimeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_wsCts.Token))
            {
                sendTimeoutCts.CancelAfter(TimeSpan.FromSeconds(15));
                await _wsClient.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, sendTimeoutCts.Token);
            }
        }
        finally { _wsSendLock.Release(); }
    }

    private static IEnumerable<Tuple<string, Dictionary<string, string>, string>> ParseStompFrames(string raw)
    {
        var frames = new List<Tuple<string, Dictionary<string, string>, string>>();
        foreach (var chunk in raw.Split('\0'))
        {
            string frameText = chunk.Trim('\n', '\r');
            if (string.IsNullOrEmpty(frameText)) continue;

            string[] lines = frameText.Replace("\r\n", "\n").Split('\n');
            string cmd = lines[0];
            var headers = new Dictionary<string, string>();
            int i = 1;
            for (; i < lines.Length; i++)
            {
                if (lines[i].Length == 0) { i++; break; }
                int idx = lines[i].IndexOf(':');
                if (idx > 0) headers[lines[i].Substring(0, idx)] = lines[i].Substring(idx + 1);
            }
            string body = i < lines.Length ? string.Join("\n", lines, i, lines.Length - i) : null;
            frames.Add(Tuple.Create(cmd, headers, body));
        }
        return frames;
    }

    private static async Task WsReceiveLoopAsync(CancellationToken token)
    {
        var buffer = new byte[8192];
        try
        {
            while (_wsClient.State == WebSocketState.Open && !token.IsCancellationRequested)
            {
                string message = null;
                bool closedByServer = false;

                using (var ms = new MemoryStream())
                {
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _wsClient.ReceiveAsync(new ArraySegment<byte>(buffer), token);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            LogWebhook("WS closed by server: " + result.CloseStatusDescription);
                            try { await _wsClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None); }
                            catch (Exception closeEx) { LogWebhook("WS CloseAsync failed: " + closeEx.Message); }
                            closedByServer = true;
                            break;
                        }
                        ms.Write(buffer, 0, result.Count);
                    } while (!result.EndOfMessage);

                    if (closedByServer) break;
                    message = Encoding.UTF8.GetString(ms.ToArray());
                }

                LogWebhook("WS RECV: " + message);
                try
                {
                    foreach (var frame in ParseStompFrames(message))
                        await HandleStompFrameAsync(frame.Item1, frame.Item2, frame.Item3);
                }
                catch (Exception handleEx) { LogWebhook("WS message handling EXCEPTION: " + handleEx); }
            }
        }
        catch (Exception ex) { LogWebhook("WS receive loop EXCEPTION: " + ex); }

        if (!token.IsCancellationRequested)
        {
            try { await Task.Delay(3000, CancellationToken.None); } catch { }
            try { await EnsureWebSocketConnectedAsync(); }
            catch (Exception reconnectEx) { LogWebhook("WS reconnect failed: " + reconnectEx.Message); }
        }
    }

    private static async Task HandleStompFrameAsync(string command, Dictionary<string, string> headers, string body)
    {
        switch (command)
        {
            case "CONNECTED": LogWebhook("STOMP CONNECTED received."); return;
            case "ERROR": LogWebhook("STOMP ERROR frame: " + (body ?? "(no body)")); return;
            case "MESSAGE": if (!string.IsNullOrEmpty(body)) await HandleWsPayloadAsync(body); return;
            default: return;
        }
    }

    private static Task HandleWsPayloadAsync(string message)
    {
        JObject json;
        try { json = JObject.Parse(message); } catch { return Task.FromResult(false); }

        string requestId = (string)json["request_id"];
        string txnId = (string)json["txn_id"];
        string status = (string)json["status"];
        string statusText = (string)json["message"];
        string debitStatus = (string)json["debit_status"];
        string creditStatus = (string)json["credit_status"];

        // Lookup MUST use request_id -- that's what _byTraceId is keyed with when the
        // request goes out (see SendTransactionDetailRequestOnOpenSocketAsync). txn_id
        // is NPI's own id and is not something we ever registered a record under.
        string lookupKey = !string.IsNullOrEmpty(requestId) ? requestId : txnId;

        if (string.IsNullOrEmpty(lookupKey) || string.IsNullOrEmpty(status)) return Task.FromResult(false);

        TransactionRecord record;
        if (!_byTraceId.TryGetValue(lookupKey, out record))
        {
            LogWebhook("WS message for unknown request_id=" + lookupKey + " - ignored.");
            return Task.FromResult(false);
        }

        record.TransactionId = txnId ?? record.TransactionId ?? lookupKey;

        switch (status.ToUpperInvariant())
        {
            case "COMPLETED":
                {
                    bool debitOk = debitStatus == "000";
                    bool creditOk = creditStatus == "000" || creditStatus == "999" ||
                                    string.Equals(creditStatus, "DEFER", StringComparison.OrdinalIgnoreCase);
                    bool success = debitOk && creditOk;
                    record.Status = success ? "SUCCESS" : "FAILED";
                    record.Message = statusText ?? (success ? "Payment successful" : "Payment failed");
                    // Status update only -- no server-side POS push. The browser owning the
                    // USB terminal polls CheckPaymentStatus and calls showPassOnPos()/
                    // showFailOnPos() itself via PosLocalClient.js.
                    break;
                }
            case "FAILED":
                record.Status = "FAILED";
                record.Message = statusText ?? "Payment failed";
                break;
            default:
                record.Message = statusText ?? record.Message;
                break;
        }

        return Task.FromResult(true);
    }

    // ==================== RSA token encryption ====================

    private static string LoadPublicKeyBase64(string base64OrPath)
    {
        if (string.IsNullOrEmpty(base64OrPath))
            throw new InvalidOperationException("NPI_NCHLPUBLICKEY is not configured in the database.");

        string trimmed = base64OrPath.Trim();
        bool looksLikePath = trimmed.Contains("\\") ||
                              trimmed.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ||
                              trimmed.EndsWith(".pem", StringComparison.OrdinalIgnoreCase) ||
                              trimmed.EndsWith(".key", StringComparison.OrdinalIgnoreCase);
        if (looksLikePath)
        {
            string fullPath = HostingEnvironment.MapPath(trimmed);
            trimmed = File.ReadAllText(fullPath).Trim();
        }

        trimmed = trimmed.Replace("-----BEGIN PUBLIC KEY-----", "")
                          .Replace("-----END PUBLIC KEY-----", "")
                          .Replace("\r", "").Replace("\n", "").Replace(" ", "");
        return trimmed;
    }

    private static string EncryptApiToken(string apiToken, string publicKeyBase64OrPath)
    {
        string base64 = LoadPublicKeyBase64(publicKeyBase64OrPath);
        byte[] spkiBytes = Convert.FromBase64String(base64);
        RSAParameters rsaParams = DecodeX509PublicKey(spkiBytes);

        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(rsaParams);
            byte[] dataBytes = Encoding.UTF8.GetBytes(apiToken);
            byte[] encrypted = rsa.Encrypt(dataBytes, false);
            return Convert.ToBase64String(encrypted);
        }
    }

    private static RSAParameters DecodeX509PublicKey(byte[] x509Key)
    {
        byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };

        using (var mem = new MemoryStream(x509Key))
        using (var reader = new BinaryReader(mem))
        {
            ushort twoBytes = reader.ReadUInt16();
            if (twoBytes == 0x8130) reader.ReadByte();
            else if (twoBytes == 0x8230) reader.ReadInt16();
            else throw new FormatException("Unexpected SPKI header.");

            byte[] seq = reader.ReadBytes(15);
            if (!BytesEqual(seq, seqOid)) throw new FormatException("Not an RSA public key.");

            twoBytes = reader.ReadUInt16();
            if (twoBytes == 0x8103) reader.ReadByte();
            else if (twoBytes == 0x8203) reader.ReadInt16();
            else throw new FormatException("Unexpected BIT STRING header.");

            byte paddingByte = reader.ReadByte();
            if (paddingByte != 0x00) throw new FormatException("Unexpected BIT STRING padding byte.");

            twoBytes = reader.ReadUInt16();
            if (twoBytes == 0x8130) reader.ReadByte();
            else if (twoBytes == 0x8230) reader.ReadInt16();
            else throw new FormatException("Unexpected inner SEQUENCE header.");

            twoBytes = reader.ReadUInt16();
            byte lowByte = 0x00, highByte = 0x00;
            if (twoBytes == 0x8102) lowByte = reader.ReadByte();
            else if (twoBytes == 0x8202) { highByte = reader.ReadByte(); lowByte = reader.ReadByte(); }
            else throw new FormatException("Unexpected modulus length header.");

            byte[] modSizeBytes = { lowByte, highByte, 0x00, 0x00 };
            int modSize = BitConverter.ToInt32(modSizeBytes, 0);

            int firstByte = reader.PeekChar();
            if (firstByte == 0x00) { reader.ReadByte(); modSize -= 1; }

            byte[] modulus = reader.ReadBytes(modSize);

            if (reader.ReadByte() != 0x02) throw new FormatException("Unexpected exponent tag.");
            int expBytesLen = reader.ReadByte();
            byte[] exponent = reader.ReadBytes(expBytesLen);

            return new RSAParameters { Modulus = modulus, Exponent = exponent };
        }
    }

    private static bool BytesEqual(byte[] a, byte[] b)
    {
        if (a.Length != b.Length) return false;
        for (int i = 0; i < a.Length; i++) if (a[i] != b[i]) return false;
        return true;
    }

    // ==================== QR generation ====================

    public static async Task<QrResult> GenerateDynamicQrAsync(decimal amount, string billNumber)
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        DYNAMIC_QR DQEnt = GetSettings();

        string baseUrl = DQEnt.NPI_BASEURL;
        string username = DQEnt.NPI_USERNAME;
        string password = DQEnt.NPI_PASSWORD;
        string pfxPath = HostingEnvironment.MapPath("~/certs/" + DQEnt.NPI_PFXPATH);
        string pfxPassword = DQEnt.NPI_PFXPASSWORD.Trim();
        string acquirerId = DQEnt.NPI_ACQUIRERID;
        string merchantId = DQEnt.NPI_MERCHANTID;
        string merchantName = DQEnt.NPI_MERCHANTNAME;
        string merchantCategoryCode = DQEnt.NPI_MERCHANTCATEGORYCODE;
        string merchantCity = DQEnt.NPI_MERCHANTCITY;
        string merchantCountry = DQEnt.NPI_MERCHANTCOUNTRY;
        string merchantPostalCode = DQEnt.NPI_MERCHANTPOSTALCODE;
        string userId = DQEnt.NPI_USERID;
        string tl = DQEnt.NPI_TERMINALLABEL ?? "Terminal1";
        string storeLabel = DQEnt.NPI_STORELABEL;

        const string transactionCurrency = "524";
        string transactionAmount = amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        string tokenString = string.Join(",", acquirerId, merchantId, merchantCategoryCode, transactionCurrency, transactionAmount, billNumber, userId);
        string token = SignToken(tokenString, pfxPath, pfxPassword);

        var requestBody = new JObject();
        requestBody.Add("pointOfInitialization", 12);
        requestBody.Add("acquirerId", acquirerId);
        requestBody.Add("merchantId", merchantId);
        requestBody.Add("merchantName", merchantName);
        requestBody.Add("merchantCategoryCode", int.Parse(merchantCategoryCode));
        requestBody.Add("merchantCountry", merchantCountry);
        requestBody.Add("merchantCity", merchantCity);
        requestBody.Add("merchantPostalCode", merchantPostalCode);
        requestBody.Add("merchantLanguage", "en");
        requestBody.Add("transactionCurrency", int.Parse(transactionCurrency));
        requestBody.Add("transactionAmount", transactionAmount);
        requestBody.Add("valueOfConvenienceFeeFixed", "0.00");
        requestBody.Add("billNumber", billNumber);
        requestBody.Add("referenceLabel", null);
        requestBody.Add("mobileNo", null);
        requestBody.Add("storeLabel", storeLabel);
        requestBody.Add("terminalLabel", tl);
        requestBody.Add("purposeOfTransaction", "Bill payment");
        requestBody.Add("additionalConsumerDataRequest", null);
        requestBody.Add("loyaltyNumber", null);
        requestBody.Add("qrImage", true);
        requestBody.Add("token", token);

        using (var client = new HttpClient { BaseAddress = new Uri(baseUrl) })
        {
            var authBytes = Encoding.UTF8.GetBytes(username + ":" + password);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

            var content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/qr/generateQR", content);
            string responseBody = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(responseBody);
            string responseCode = (string)json["responseCode"];

            if (responseCode == "000")
            {
                var data = json["data"];
                return new QrResult
                {
                    Success = true,
                    QrImageBase64 = (string)data["qrImage"],
                    QrString = (string)data["qrString"],
                    ValidationTraceId = (string)data["validationTraceId"]
                };
            }

            string errMsg = (string)json["responseMessage"] ?? responseBody;
            return new QrResult { Success = false, ErrorMessage = errMsg };
        }
    }

    private static string SignToken(string tokenString, string pfxPath, string pfxPassword)
    {
        var cert = new X509Certificate2(pfxPath, pfxPassword,
            X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

        using (RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(tokenString);
            byte[] signature = rsa.SignData(dataBytes, "SHA256");
            return Convert.ToBase64String(signature);
        }
    }

    // NOTE: PosClient_* (device connect/status/command) intentionally does not live here.
    // POS device I/O now lives entirely client-side in PosLocalClient.js, which runs in the
    // browser of the terminal that owns the USB port and gets its apiKey/baseUrl from
    // PosConfig.ashx. This service's job is limited to: talking to NPI (GenerateDynamicQrAsync),
    // tracking transaction status (RegisterTransaction/CheckPaymentStatus), and the WebSocket
    // listener that updates that status.

    public static void LogWebhook(string message)
    {
        try
        {
            string logPath = HostingEnvironment.MapPath("~/App_Data/webhook_log.txt");
            string line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " | " + message + Environment.NewLine;
            File.AppendAllText(logPath, line);
        }
        catch (Exception ex)
        {
            // Don't let a logging failure be completely invisible -- surface it via
            // Trace (visible in DebugView / IIS trace) so a future path/permissions
            // problem doesn't silently disappear the way the missing WS RECV lines did.
            try { System.Diagnostics.Trace.WriteLine("LogWebhook failed: " + ex.Message + " | original message: " + message); } catch { }
        }
    }
}