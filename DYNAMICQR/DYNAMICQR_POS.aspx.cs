using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
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
using System.Web.Hosting;
using System.Web.Services;
using Newtonsoft.Json.Linq;
using Entity.Components;
using Service.Components;
using Entity.Framework;
using PhyeGanCore;

public partial class DYNAMICQR_DYNAMICQR_POS : System.Web.UI.Page
{
    PhyeGan PG = new PhyeGan();
    DYNAMIC_QR DQREnt = new DYNAMIC_QR();
    DYNAMIC_QRService DQRSer = new DYNAMIC_QRService();
   
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected async void btnGenerateQR_Click(object sender, EventArgs e)
    {
        decimal amount;
        if (!decimal.TryParse(txtAmount.Text, out amount))
        {
            lblMessage.CssClass = "text-danger";
            lblMessage.Text = "Please enter a valid amount.";
            return;
        }

        try
        {
            bool connected = await PosClient_EnsureConnectedAsync();
            if (!connected)
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "POS device is not connected. Please check the device and try again.";
                return;
            }

            string billNumber = GenerateBillNumber();
            var result = await GenerateDynamicQrAsync(amount, billNumber);

            if (!result.Success)
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "QR generation failed: " + result.ErrorMessage;
                await PosClient_SendCommandAsync("FAIL**" + amount.ToString("0.00") + "**Payment Failed");
                await Task.Delay(5000);
                await PosClient_SendCommandAsync("IDLE");
                return;
            }

            if (!string.IsNullOrEmpty(result.QrImageBase64))
            {
                imgQR.ImageUrl = "data:image/png;base64," + result.QrImageBase64;
            }

            RegisterTransaction(billNumber, result.ValidationTraceId, amount.ToString("0.00"));

            Session["NPI_BillNumber"] = billNumber;
            Session["NPI_ValidationTraceId"] = result.ValidationTraceId;

            try
            {
                await WsSendTransactionDetailRequestAsync(result.ValidationTraceId);
            }
            catch (Exception wsEx)
            {
                LogWebhook("WS subscribe failed for " + result.ValidationTraceId + ": " + wsEx.Message);
            }

            string scanText = string.IsNullOrEmpty(txtScan.Text) ? PG.CompanyName() : txtScan.Text;
            string qrCommand = "QR**Rs. " + amount.ToString("0.00") + "**" + scanText + "**" + result.QrString;
            await PosClient_SendCommandAsync(qrCommand);

            lblMessage.CssClass = "text-success";
            lblMessage.Text = "QR generated. Trace Id: " + result.ValidationTraceId + " — waiting for payment...";

            ClientScript.RegisterStartupScript(this.GetType(), "startPolling", "startStatusPolling();", true);
        }
        catch (Exception ex)
        {
            string details = ex.Message;
            var inner = ex.InnerException;
            while (inner != null)
            {
                details += " → " + inner.Message;
                inner = inner.InnerException;
            }
            lblMessage.CssClass = "text-danger";
            lblMessage.Text = "Error: " + details;
        }
    }

    private string GenerateBillNumber()
    {
        string ticks = DateTime.UtcNow.Ticks.ToString();
        return ticks.Substring(ticks.Length - 12);
    }

    protected async void btnPass_Click(object sender, EventArgs e)
    {
        string result = await PosClient_SendCommandAsync("PASS**SUCCESS!**Payment Successful");
        lblMessage.Text = "POS: " + result;
    }

    protected async void btnFail_Click(object sender, EventArgs e)
    {
        string command = "FAIL**" + txtAmount.Text + "**Payment Failed";
        string result = await PosClient_SendCommandAsync(command);
        lblMessage.Text = "POS: " + result;
    }

    protected async void btnWait_Click(object sender, EventArgs e)
    {
        string command = "WAIT**" + txtAmount.Text + "**Please Wait...";
        string result = await PosClient_SendCommandAsync(command);
        lblMessage.Text = "POS: " + result;
    }

    protected async void btnIdle_Click(object sender, EventArgs e)
    {
        string result = await PosClient_SendCommandAsync("IDLE");
        lblMessage.Text = "POS: " + result;
    }


    [WebMethod(EnableSession = true)]
    public static PaymentStatusResult CheckPaymentStatus()
    {
        var session = HttpContext.Current.Session;
        var billNumber = session["NPI_BillNumber"] as string;

        if (string.IsNullOrEmpty(billNumber))
            return new PaymentStatusResult { Status = "NONE", Message = "No active transaction." };

        TransactionRecord record;
        if (!_byBillNumber.TryGetValue(billNumber, out record))
            return new PaymentStatusResult { Status = "NONE", Message = "Transaction expired or not found." };

        return new PaymentStatusResult { Status = record.Status, Message = record.Message };
    }

    [WebMethod(EnableSession = true)]
    public static async Task<string> ResetToIdle()
    {
        await PosClient_SendCommandAsync("IDLE");
        var session = HttpContext.Current.Session;
        session["NPI_BillNumber"] = null;
        session["NPI_ValidationTraceId"] = null;
        return "OK";
    }

    public class PaymentStatusResult
    {
        // NONE | PENDING | SUCCESS | FAILED
        public string Status;
        public string Message;
    }

    private class TransactionRecord
    {
        public string BillNumber;
        public string ValidationTraceId;
        public string Status = "PENDING";  // PENDING | SUCCESS | FAILED
        public string Message = "Waiting for payment...";
        public string Amount;
        public DateTime CreatedUtc = DateTime.UtcNow;
    }

    private static readonly ConcurrentDictionary<string, TransactionRecord> _byBillNumber =
        new ConcurrentDictionary<string, TransactionRecord>();
    private static readonly ConcurrentDictionary<string, TransactionRecord> _byTraceId =
        new ConcurrentDictionary<string, TransactionRecord>();

    private static void RegisterTransaction(string billNumber, string validationTraceId, string amount)
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

    private static ClientWebSocket _wsClient;
    private static CancellationTokenSource _wsCts;
    private static readonly SemaphoreSlim _wsConnectLock = new SemaphoreSlim(1, 1);
    private static readonly SemaphoreSlim _wsSendLock = new SemaphoreSlim(1, 1);

    private static async Task EnsureWebSocketConnectedAsync()
    {
        if (_wsClient != null && _wsClient.State == WebSocketState.Open)
            return;

        // Bounded wait on the lock itself - belt-and-suspenders alongside the connect/send
        // timeouts above, so no request can ever be stuck waiting on this forever.
        bool acquired = await _wsConnectLock.WaitAsync(TimeSpan.FromSeconds(20));
        if (!acquired)
            throw new TimeoutException("Timed out waiting for the WebSocket connect lock - a previous connect attempt may be stuck.");
        try
        {
            if (_wsClient != null && _wsClient.State == WebSocketState.Open)
                return;

            // ---- was: ConfigurationManager.AppSettings["NPI_WsUrl"] ----
            DYNAMIC_QRService DQRSer = new DYNAMIC_QRService();
            EntityList settingsList = DQRSer.GetAll(new DYNAMIC_QR());
            DYNAMIC_QR DQEnt = (DYNAMIC_QR)settingsList[0];

            string wsUrl = DQEnt.NPI_WSURL; // e.g. wss://host/nqrws
            if (string.IsNullOrEmpty(wsUrl))
                throw new InvalidOperationException("NPI_WSURL is not configured in the database.");

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            // Dispose any previous socket before replacing it - it may be sitting in a
            // Closed/Aborted/CloseReceived state from a prior disconnect and shouldn't
            // just be dropped/garbage-collected implicitly.
            if (_wsClient != null)
            {
                try { _wsClient.Dispose(); } catch { /* best effort */ }
            }

            _wsCts = new CancellationTokenSource();
            _wsClient = new ClientWebSocket();

            LogWebhook("WS connecting to " + wsUrl);
            try
            {
                // A stalled ConnectAsync (no timeout of its own) would otherwise hold
                // _wsConnectLock forever, hanging every future request that touches the
                // socket - including page loads after a transaction completes. Cap it.
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
                while (innerEx != null)
                {
                    chain += " → " + innerEx.Message;
                    innerEx = innerEx.InnerException;
                }
                LogWebhook("WS ConnectAsync FAILED for " + wsUrl + " : " + chain);
                throw;
            }
            LogWebhook("WS connected (transport-level). Starting STOMP handshake...");

            // NOTE: matches vendor sample exactly now - fire CONNECT then immediately
            // SUBSCRIBE without blocking on a CONNECTED reply. The vendor's own sample
            // client never waits for CONNECTED before subscribing/sending, so we don't
            // either. The receive loop still logs/handles CONNECTED if one arrives.
            await StompConnectAsync();

            await StompSendFrameAsync("SUBSCRIBE", new[]
            {
                new KeyValuePair<string, string>("id", "sub-0"),
                new KeyValuePair<string, string>("destination", "/user/nqrws/check-txn-status"),
            }, null);
            LogWebhook("WS subscribed to /user/nqrws/check-txn-status");


            Task.Run(() => WsReceiveLoopAsync(_wsCts.Token));

            await ResendPendingTransactionRequestsAsync();
        }
        finally
        {
            _wsConnectLock.Release();
        }
    }

    private static async Task ResendPendingTransactionRequestsAsync()
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-30);
        foreach (var kvp in _byTraceId)
        {
            var record = kvp.Value;
            if (record.Status == "PENDING" && record.CreatedUtc >= cutoff)
            {
                try
                {
                    await SendTransactionDetailRequestOnOpenSocketAsync(kvp.Key);
                    LogWebhook("WS resent check-txn-status request for request_id=" + kvp.Key + " after (re)connect");
                }
                catch (Exception ex)
                {
                    LogWebhook("WS resend failed for " + kvp.Key + ": " + ex.Message);
                }
            }
        }
    }

    private static async Task WsSendTransactionDetailRequestAsync(string validationTraceId)
    {
        await EnsureWebSocketConnectedAsync();
        await SendTransactionDetailRequestOnOpenSocketAsync(validationTraceId);
    }

    private static async Task SendTransactionDetailRequestOnOpenSocketAsync(string validationTraceId)
    {
        // ---- was: ConfigurationManager.AppSettings["NPI_TerminalLabel"/"NPI_WsUsername"/"NPI_WsApiToken"/"NPI_NCHLPublicKeyBase64"] ----
        DYNAMIC_QRService DQRSer = new DYNAMIC_QRService();
        EntityList settingsList = DQRSer.GetAll(new DYNAMIC_QR());
        DYNAMIC_QR DQEnt = (DYNAMIC_QR)settingsList[0];

        string merchantId = DQEnt.NPI_TERMINALLABEL;
        string username = DQEnt.NPI_WSUSERNAME;
        string rawApiToken = DQEnt.NPI_WSAPITOKEN;
        string publicKeyBase64OrPath = DQEnt.NPI_NCHLPUBLICKEY;

        string encryptedApiToken = EncryptApiToken(rawApiToken, publicKeyBase64OrPath);

        var payload = new JObject();
        payload.Add("merchant_id", merchantId);
        payload.Add("request_id", validationTraceId);
        payload.Add("username", username);
        payload.Add("api_token", encryptedApiToken);

        string body = payload.ToString(Newtonsoft.Json.Formatting.None);

        // matches vendor sample: SEND frame now carries an explicit content-length header
        await StompSendFrameAsync("SEND", new[]
        {
            new KeyValuePair<string, string>("destination", "/nqrws/check-txn-status"),
            new KeyValuePair<string, string>("content-length", Encoding.UTF8.GetByteCount(body).ToString()),
        }, body);

        LogWebhook("WS sent check-txn-status request for request_id=" + validationTraceId);
    }


    private static async Task StompConnectAsync()
    {
        // matches vendor sample's CONNECT frame headers exactly:
        // accept-version:1.1,1.0 / heart-beat:10000,10000 - no "host" header,
        // and we no longer block waiting for a CONNECTED frame in response.
        await StompSendFrameAsync("CONNECT", new[]
        {
            new KeyValuePair<string, string>("accept-version", "1.1,1.0"),
            new KeyValuePair<string, string>("heart-beat", "10000,10000"),
        }, null);

        LogWebhook("STOMP CONNECT frame sent (not waiting for CONNECTED, matching vendor sample).");
    }

    private static async Task StompSendFrameAsync(string command, IEnumerable<KeyValuePair<string, string>> headers, string body)
    {
        var sb = new StringBuilder();
        sb.Append(command).Append('\n');
        foreach (var h in headers)
            sb.Append(h.Key).Append(':').Append(h.Value).Append('\n');
        sb.Append('\n');
        if (!string.IsNullOrEmpty(body))
            sb.Append(body);
        sb.Append('\0');

        await _wsSendLock.WaitAsync();
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            // Same reasoning as the connect timeout - an unbounded SendAsync could hang
            // and hold _wsSendLock forever, blocking every other frame send indefinitely.
            using (var sendTimeoutCts = CancellationTokenSource.CreateLinkedTokenSource(_wsCts.Token))
            {
                sendTimeoutCts.CancelAfter(TimeSpan.FromSeconds(15));
                await _wsClient.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, sendTimeoutCts.Token);
            }
        }
        finally
        {
            _wsSendLock.Release();
        }
    }

    private static IEnumerable<Tuple<string, Dictionary<string, string>, string>> ParseStompFrames(string raw)
    {
        var frames = new List<Tuple<string, Dictionary<string, string>, string>>();
        foreach (var chunk in raw.Split('\0'))
        {
            string frameText = chunk.Trim('\n', '\r');
            if (string.IsNullOrEmpty(frameText))
                continue; // heart-beat or trailing empty split

            string[] lines = frameText.Replace("\r\n", "\n").Split('\n');
            string cmd = lines[0];
            var headers = new Dictionary<string, string>();
            int i = 1;
            for (; i < lines.Length; i++)
            {
                if (lines[i].Length == 0) { i++; break; } // blank line ends headers
                int idx = lines[i].IndexOf(':');
                if (idx > 0)
                    headers[lines[i].Substring(0, idx)] = lines[i].Substring(idx + 1);
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
                            // Complete the close handshake properly (matches vendor sample's
                            // ReceiveMessages, which calls CloseAsync here instead of just
                            // breaking out - leaving it unfinished left the socket stuck in
                            // CloseReceived instead of a clean Closed state).
                            try
                            {
                                await _wsClient.CloseAsync(
                                    WebSocketCloseStatus.NormalClosure,
                                    "",
                                    CancellationToken.None);
                            }
                            catch (Exception closeEx)
                            {
                                LogWebhook("WS CloseAsync during server-close handshake failed: " + closeEx.Message);
                            }
                            closedByServer = true;
                            break;
                        }
                        ms.Write(buffer, 0, result.Count);
                    } while (!result.EndOfMessage);

                    if (closedByServer)
                        break; // fall out of the while loop, not the whole method

                    message = Encoding.UTF8.GetString(ms.ToArray());
                }

                LogWebhook("WS RECV: " + message);

                try
                {
                    foreach (var frame in ParseStompFrames(message))
                        await HandleStompFrameAsync(frame.Item1, frame.Item2, frame.Item3);
                }
                catch (Exception handleEx)
                {
                    LogWebhook("WS message handling EXCEPTION: " + handleEx);
                }
            }
        }
        catch (Exception ex)
        {
            LogWebhook("WS receive loop EXCEPTION: " + ex);
        }

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
            case "CONNECTED":
                // no longer gates the connect flow - just logged for visibility now
                LogWebhook("STOMP CONNECTED received.");
                return;

            case "ERROR":
                LogWebhook("STOMP ERROR frame: " + (body ?? "(no body)"));
                return;

            case "MESSAGE":
                if (!string.IsNullOrEmpty(body))
                    await HandleWsPayloadAsync(body);
                return;

            case "RECEIPT":
            default:
                return; // nothing to do for these
        }
    }

    private static async Task HandleWsPayloadAsync(string message)
    {
        JObject json;
        try { json = JObject.Parse(message); }
        catch { return; } // not JSON (e.g. a bare connection ack) — ignore

        string txnId = (string)json["txn_id"] ?? (string)json["request_id"];
        string status = (string)json["status"];           // ENTR | PARSED | COMPLETED | FAILED
        string statusText = (string)json["message"];
        string debitStatus = (string)json["debit_status"];
        string creditStatus = (string)json["credit_status"];

        if (string.IsNullOrEmpty(txnId) || string.IsNullOrEmpty(status))
            return;

        TransactionRecord record;
        if (!_byTraceId.TryGetValue(txnId, out record))
        {
            LogWebhook("WS message for unknown txn_id=" + txnId + " — ignored.");
            return;
        }

        switch (status.ToUpperInvariant())
        {
            case "COMPLETED":
                {

                    bool debitOk = debitStatus == "000";
                    bool creditOk = creditStatus == "000" ||
                                    string.Equals(creditStatus, "DEFER", StringComparison.OrdinalIgnoreCase);
                    bool success = debitOk && creditOk;

                    record.Status = success ? "SUCCESS" : "FAILED";
                    record.Message = statusText ?? (success ? "Payment successful" : "Payment failed");

                    if (success)
                        await PosClient_SendCommandAsync("PASS**SUCCESS!**Payment Successful");
                    else
                        await PosClient_SendCommandAsync("FAIL**" + (record.Amount ?? "0.00") + "**Payment Failed");
                    break;
                }
            case "FAILED":
                record.Status = "FAILED";
                record.Message = statusText ?? "Payment failed";
                await PosClient_SendCommandAsync("FAIL**" + (record.Amount ?? "0.00") + "**Payment Failed");
                break;

            case "ENTR":
            case "PARSED":
            default:
                record.Message = statusText ?? record.Message;
                break;
        }
    }

    private static string LoadPublicKeyBase64(string base64OrPath)
    {
        if (string.IsNullOrEmpty(base64OrPath))
            throw new InvalidOperationException("NPI_NCHLPUBLICKEY is not configured in the database.");

        string trimmed = base64OrPath.Trim();

        // NOTE: previously checked trimmed.Contains("/") to detect a file path, but
        // base64-encoded data legitimately contains "/" as part of its alphabet, so a
        // raw base64 key value was being misidentified as a path (causing "path too
        // long" errors from Server.MapPath). Now only backslash or a recognized file
        // extension counts as "looks like a path".
        bool looksLikePath = trimmed.Contains("\\") ||
                              trimmed.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ||
                              trimmed.EndsWith(".pem", StringComparison.OrdinalIgnoreCase) ||
                              trimmed.EndsWith(".key", StringComparison.OrdinalIgnoreCase);
        if (looksLikePath)
        {
            string fullPath = HttpContext.Current.Server.MapPath(trimmed);
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
            byte[] encrypted = rsa.Encrypt(dataBytes, false); // false => PKCS#1 v1.5 padding
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
            else throw new FormatException("Unexpected SPKI header — is this really an X.509 public key blob?");

            byte[] seq = reader.ReadBytes(15);
            if (!BytesEqual(seq, seqOid))
                throw new FormatException("Not an RSA public key (algorithm OID mismatch).");

            twoBytes = reader.ReadUInt16();
            if (twoBytes == 0x8103) reader.ReadByte();
            else if (twoBytes == 0x8203) reader.ReadInt16();
            else throw new FormatException("Unexpected BIT STRING header.");

            byte paddingByte = reader.ReadByte();
            if (paddingByte != 0x00)
                throw new FormatException("Unexpected BIT STRING padding byte.");

            twoBytes = reader.ReadUInt16();
            if (twoBytes == 0x8130) reader.ReadByte();
            else if (twoBytes == 0x8230) reader.ReadInt16();
            else throw new FormatException("Unexpected inner SEQUENCE header.");

            twoBytes = reader.ReadUInt16();
            byte lowByte = 0x00, highByte = 0x00;
            if (twoBytes == 0x8102)
            {
                lowByte = reader.ReadByte();
            }
            else if (twoBytes == 0x8202)
            {
                highByte = reader.ReadByte();
                lowByte = reader.ReadByte();
            }
            else
            {
                throw new FormatException("Unexpected modulus length header.");
            }

            byte[] modSizeBytes = { lowByte, highByte, 0x00, 0x00 };
            int modSize = BitConverter.ToInt32(modSizeBytes, 0);

            int firstByte = reader.PeekChar();
            if (firstByte == 0x00)
            {
                reader.ReadByte();
                modSize -= 1;
            }

            byte[] modulus = reader.ReadBytes(modSize);

            if (reader.ReadByte() != 0x02)
                throw new FormatException("Unexpected exponent tag.");
            int expBytesLen = reader.ReadByte();
            byte[] exponent = reader.ReadBytes(expBytesLen);

            return new RSAParameters { Modulus = modulus, Exponent = exponent };
        }
    }

    private static bool BytesEqual(byte[] a, byte[] b)
    {
        if (a.Length != b.Length) return false;
        for (int i = 0; i < a.Length; i++)
            if (a[i] != b[i]) return false;
        return true;
    }
    private class QrResult
    {
        public bool Success;
        public string QrImageBase64;
        public string QrString;
        public string ValidationTraceId;
        public string ErrorMessage;
    }

    private static async Task<QrResult> GenerateDynamicQrAsync(decimal amount, string billNumber)
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        // ---- was: ConfigurationManager.AppSettings["NPI_..."] for all of the below ----
        DYNAMIC_QRService DQRSer = new DYNAMIC_QRService();
        EntityList settingsList = DQRSer.GetAll(new DYNAMIC_QR());
        DYNAMIC_QR DQEnt = (DYNAMIC_QR)settingsList[0];

        string baseUrl = DQEnt.NPI_BASEURL;
        string username = DQEnt.NPI_USERNAME;
        string password = DQEnt.NPI_PASSWORD;
        string pfxPath =DQEnt.NPI_PFXPATH;
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

        const string transactionCurrency = "524"; // NPR
        string transactionAmount = amount.ToString("0.00");

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
        var cert = new X509Certificate2(
            pfxPath, pfxPassword,
            X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

        using (RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(tokenString);
            byte[] signature = rsa.SignData(dataBytes, "SHA256");
            return Convert.ToBase64String(signature);
        }
    }

    //private const string POS_API_KEY = "Z8uVovI2eftp65dO9JoxEstKcWggSlTAza4erAQhELmSC761rVtp5IIzaXOxWNw0ycPCICYCnJBCVPCzvdT8fbJvWIWm69fhHveZesIiDEIeI0BkdSspMPimWYNWs25D";
    //private const string POS_BASE_URL = "http://127.0.0.1:9121";

    private static async Task<bool> PosClient_GetConnectedAsync()
    {
        DYNAMIC_QRService DQRSer = new DYNAMIC_QRService();
        EntityList settingsList = DQRSer.GetAll(new DYNAMIC_QR());
        DYNAMIC_QR DQEnt = (DYNAMIC_QR)settingsList[0];
        string POS_API_KEY = DQEnt.POS_API_KEY;
        string POS_BASE_URL = DQEnt.POS_BASE_URL;

        try
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", POS_API_KEY);
                var response = await client.GetAsync(POS_BASE_URL + "/api/status");
                string body = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(body);
                return (bool?)json["connected"] ?? false;
            }
        }
        catch
        {
            return false;
        }
    }

    private static async Task<bool> PosClient_ConnectAsync(string port)
    {
        DYNAMIC_QRService DQRSer = new DYNAMIC_QRService();
        EntityList settingsList = DQRSer.GetAll(new DYNAMIC_QR());
        DYNAMIC_QR DQEnt = (DYNAMIC_QR)settingsList[0];
        string POS_API_KEY = DQEnt.POS_API_KEY;
        string POS_BASE_URL = DQEnt.POS_BASE_URL;
        try
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", POS_API_KEY);
                var body = new JObject();
                body.Add("port", port);
                var content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(POS_BASE_URL + "/api/connect", content);
                string respBody = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(respBody);
                return (bool?)json["success"] ?? false;
            }
        }
        catch
        {
            return false;
        }
    }

    private static async Task<bool> PosClient_EnsureConnectedAsync()
    {
        if (await PosClient_GetConnectedAsync()) return true;
        return await PosClient_ConnectAsync(null);
    }

    private static async Task<string> PosClient_SendCommandAsync(string command)
    {
        DYNAMIC_QRService DQRSer = new DYNAMIC_QRService();
        EntityList settingsList = DQRSer.GetAll(new DYNAMIC_QR());
        DYNAMIC_QR DQEnt = (DYNAMIC_QR)settingsList[0];
        string POS_API_KEY = DQEnt.POS_API_KEY;
        string POS_BASE_URL = DQEnt.POS_BASE_URL;
        try
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", POS_API_KEY);
                string json = "{\"command\":\"" + command.Replace("\"", "\\\"") + "\"}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(POS_BASE_URL + "/api/command", content);
                string body = await response.Content.ReadAsStringAsync();
                return response.StatusCode + " - " + body;
            }
        }
        catch (Exception ex)
        {
            return "POS send failed: " + ex.Message;
        }
    }

    private static void LogWebhook(string message)
    {
        try
        {
            string logPath = HttpContext.Current.Server.MapPath("~/App_Data/webhook_log.txt");
            string line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " | " + message + Environment.NewLine;
            File.AppendAllText(logPath, line);
        }
        catch { /* never let logging break the actual flow */ }
    }
}
