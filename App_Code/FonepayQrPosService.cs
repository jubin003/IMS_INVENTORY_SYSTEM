using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using QRCoder;
using Entity.Components;
using Service.Components;
using Entity.Framework;

// Mirrors DynamicQrPosService.cs, but for Fonepay's thirdPartyDynamicQr* API instead of NPI.
//
// Key difference from the NPI flow: Fonepay does not hand you one persistent WS channel to
// subscribe to by trace id. Each QR generation call returns its OWN one-shot WebSocket URL
// (thirdpartyQrWebSocketUrl). So instead of a single long-lived listener multiplexing many
// transactions, we spin up one short-lived listener per PRN, and use its push as a "wake up
// and check" signal rather than a source of truth -- the authoritative status always comes
// from thirdPartyDynamicQrGetStatus.
//
// Physical POS display: the raw Fonepay qrMessage string is now sent straight to NiziPOS's
// built-in QR** command from the client (PosLocalClient.js's showQrOnPos(), called from
// FONEPAY_POS_TEST.aspx's window.onload) -- NiziPOS renders/encodes the QR on-device, nothing
// is rasterized here or server-side.
//
// HISTORY / KNOWN RISK: an earlier round of testing found the QR** command truncates
// payloads as long as Fonepay's full EMV qrMessage (short strings rendered fine, the real
// payload didn't), which is why this was previously routed through GenerateQrJpegForDevice()
// below + QrDeviceImage.ashx + NiziPOS's POST /api/upload-image instead. That JPEG path is
// still here, unused, as a fallback -- if the physical device shows a blank/garbled code
// again, that length limit is almost certainly why, and switching PosLocalClient.js's Fonepay
// call back to pushFonepayQrToDevice()/showQrImageOnPos() is the known-working workaround.
public static class FonepayQrPosService
{
    // Config that used to be hardcoded here now lives in the DYNAMIC_QR settings row,
    // read fresh via GetSettings() -- mirrors the pattern in DynamicQrPosService.cs.
    // New columns added to DYNAMIC_QR for this: FONEPAY_BASEURL, FONEPAY_MERCHANTCODE,
    // FONEPAY_SECRETKEY, FONEPAY_USERNAME, FONEPAY_PASSWORD, NIZIPOS_APIKEY, NIZIPOS_BASEURL.
    private static DYNAMIC_QR GetSettings()
    {
        DYNAMIC_QRService DQRSer = new DYNAMIC_QRService();
        EntityList settingsList = DQRSer.GetAll(new DYNAMIC_QR());
        return (DYNAMIC_QR)settingsList[0];
    }

    // These two are fixed API route literals, not secrets or per-deployment config, so
    // they stay as plain consts -- no reason to round-trip the DB for a URL path that
    // never changes.
    private const string QR_REQUEST_PATH = "/api/merchant/merchantDetailsForThirdParty/thirdPartyDynamicQrDownload";
    private const string QR_STATUS_PATH = "/api/merchant/merchantDetailsForThirdParty/thirdPartyDynamicQrGetStatus";
    private const string NIZIPOS_COMMAND_PATH = "/api/command";
    private const string FONEPAY_BASEURL = "https://merchantapi.fonepay.com";


    public class QrResult
    {
        public bool Success;
        public string QrMessage;   // raw EMV string -- render client-side with qrcode.min.js (browser) or GenerateQrJpegForDevice (physical POS)
        public string Prn;
        public string WsUrl;
        public string ErrorMessage;
    }

    public class PaymentStatusResult
    {
        public string Status;   // NONE | PENDING | SUCCESS | FAILED
        public string Message;
    }

    private class TransactionRecord
    {
        public string Prn;
        public string Amount;
        public string QrMessage;
        public string Status = "PENDING";
        public string Message = "Waiting for payment...";
        public DateTime CreatedUtc = DateTime.UtcNow;
        public CancellationTokenSource ListenerCts;
    }

    private static readonly ConcurrentDictionary<string, TransactionRecord> _byPrn =
        new ConcurrentDictionary<string, TransactionRecord>();

    // ==================== QR generation ====================

    public static async Task<QrResult> GenerateDynamicQrAsync(decimal amount, string remarks1, string remarks2)
    {
        DYNAMIC_QR DQEnt = GetSettings();

        string amt = amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        string prn = Guid.NewGuid().ToString("N").Substring(0, 20);

        string message = string.Join(",", amt, prn, DQEnt.FONEPAY_MERCHANTCODE, remarks1 ?? "", remarks2 ?? "");
        string dataValidation = ComputeHmacSha512(DQEnt.FONEPAY_SECRETKEY, message);

        var serializer = new JavaScriptSerializer();
        var requestObj = new Dictionary<string, string>
        {
            { "amount", amt },
            { "remarks1", remarks1 ?? "" },
            { "remarks2", remarks2 ?? "" },
            { "prn", prn },
            { "merchantCode", DQEnt.FONEPAY_MERCHANTCODE },
            { "dataValidation", dataValidation },
            { "username", DQEnt.FONEPAY_USERNAME },
            { "password", DQEnt.FONEPAY_PASSWORD }
        };
        string requestJson = serializer.Serialize(requestObj);

        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        using (var client = new HttpClient())
        {
            HttpResponseMessage response;
            string body;
            try
            {
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                response = await client.PostAsync(FONEPAY_BASEURL + QR_REQUEST_PATH, content);
                body = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return new QrResult { Success = false, ErrorMessage = GetFullErrorMessage(ex) };
            }

            if (!response.IsSuccessStatusCode)
                return new QrResult { Success = false, ErrorMessage = "HTTP " + (int)response.StatusCode + " - " + body };

            Dictionary<string, object> result;
            try { result = serializer.Deserialize<Dictionary<string, object>>(body); }
            catch (Exception ex) { return new QrResult { Success = false, ErrorMessage = "Bad response from Fonepay: " + ex.Message }; }

            bool success = result.ContainsKey("success") && (bool)result["success"];
            if (!success)
            {
                string msg = result.ContainsKey("message") ? result["message"].ToString() : body;
                return new QrResult { Success = false, ErrorMessage = msg };
            }

            return new QrResult
            {
                Success = true,
                Prn = prn,
                QrMessage = result["qrMessage"].ToString(),
                WsUrl = result["thirdpartyQrWebSocketUrl"].ToString()
            };
        }
    }

    // ==================== transaction tracking ====================

    public static void RegisterTransaction(string prn, string amount, string wsUrl, string qrMessage = null)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-30);
        foreach (var kvp in _byPrn)
        {
            if (kvp.Value.CreatedUtc < cutoff)
            {
                TransactionRecord removed;
                if (_byPrn.TryRemove(kvp.Key, out removed))
                {
                    try { if (removed.ListenerCts != null) removed.ListenerCts.Cancel(); } catch { }
                }
            }
        }

        var record = new TransactionRecord { Prn = prn, Amount = amount, QrMessage = qrMessage };
        _byPrn[prn] = record;

        LogWebhook("FP RegisterTransaction called for PRN " + prn + " -- starting WS listener + REST poller.");

        // Fire-and-forget: listen on this transaction's dedicated socket as a "wake up
        // sooner" signal, AND independently poll the authoritative status endpoint on a
        // fixed schedule regardless of whether the socket ever pushes anything. The
        // WebSocket from Fonepay is not guaranteed to arrive (connection can fail, drop
        // silently, or simply never fire for a given transaction) -- if we only checked
        // status reactively from that push, a missed/failed WS message means the
        // transaction sits at PENDING forever even after a real successful payment.
        // The periodic poll below is what makes status updates reliable regardless of
        // the WS's behavior.
        //
        // IMPORTANT: both are wrapped in Task.Run() and this class registers itself with
        // HostingEnvironment (see EnsureBackgroundWorkerRegistered below), exactly like
        // DynamicQrPosService's WsBackgroundWorker. Without this, these fire-and-forget
        // async calls run tied to the ASP.NET request's SynchronizationContext -- once
        // btnGenerateQR_Click finishes and the HTTP response for THIS request is sent,
        // continuations after their first await can be silently orphaned (no exception,
        // no log, they just never resume), which is very likely why status updates were
        // going missing with no trace in the log beyond "RegisterTransaction called".
        // Task.Run moves them onto the thread pool, free of that context.
        EnsureBackgroundWorkerRegistered();
        record.ListenerCts = new CancellationTokenSource();
        Task.Run(() => ListenForPaymentAsync(prn, wsUrl, record.ListenerCts.Token));
        Task.Run(() => PollStatusPeriodicallyAsync(prn, record.ListenerCts.Token));
    }

    // ==================== background-work lifetime (survive request lifecycle) ====================

    // Registering with HostingEnvironment tells IIS/ASP.NET "don't tear this down
    // immediately on app pool shutdown/recycle -- give it a chance to call Stop() first".
    // Combined with Task.Run() above (which frees the work from any one request's
    // SynchronizationContext), this is what makes the listener/poller reliably survive
    // past the lifetime of the HTTP request that started them -- same pattern
    // DynamicQrPosService uses for its persistent WS connection.
    private class FonepayBackgroundWorker : IRegisteredObject
    {
        public void Stop(bool immediate)
        {
            foreach (var kvp in _byPrn)
            {
                try { if (kvp.Value.ListenerCts != null) kvp.Value.ListenerCts.Cancel(); } catch { }
            }
            HostingEnvironment.UnregisterObject(this);
        }
    }

    private static readonly FonepayBackgroundWorker _backgroundWorker = new FonepayBackgroundWorker();
    private static bool _backgroundWorkerRegistered = false;
    private static readonly object _backgroundWorkerLock = new object();

    private static void EnsureBackgroundWorkerRegistered()
    {
        if (_backgroundWorkerRegistered) return;
        lock (_backgroundWorkerLock)
        {
            if (_backgroundWorkerRegistered) return;
            HostingEnvironment.RegisterObject(_backgroundWorker);
            _backgroundWorkerRegistered = true;
        }
    }

    // Independent safety-net poller: calls the real Fonepay status endpoint every few
    // seconds regardless of the WebSocket, until the transaction resolves, is cancelled
    // (RegisterTransaction cleanup), or this poller's own timeout elapses. This is what
    // guarantees CheckPaymentStatus/CheckQRPaymentStatus eventually reflects reality even
    // if the WS listener never receives a push for this PRN.
    //
    // A genuine FAILED is only ever set in ONE place: right here, when the deadline below
    // is reached with the transaction still PENDING. ConfirmStatusFromApiAsync itself never
    // maps anything to FAILED -- Fonepay's sandbox reports "expired"/"failed"/"cancelled"
    // for a QR that's merely been scanned but not yet paid, well before any real timeout,
    // so a single such reading is not trustworthy evidence of an actual failure. Treating
    // that as PENDING until this deadline (rather than reacting to it immediately) is what
    // makes "scanned but not paid yet" correctly show as pending instead of failed.
    private static async Task PollStatusPeriodicallyAsync(string prn, CancellationToken ct)
    {
        var pollInterval = TimeSpan.FromSeconds(4);
        var deadline = DateTime.UtcNow.AddMinutes(5); // matches/exceeds the WS listener's own window

        while (!ct.IsCancellationRequested && DateTime.UtcNow < deadline)
        {
            try
            {
                await Task.Delay(pollInterval, ct);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            TransactionRecord rec;
            if (!_byPrn.TryGetValue(prn, out rec) || rec.Status != "PENDING")
                return; // resolved (by WS path or a previous poll tick) or purged -- nothing left to do

            LogWebhook("FP REST poller tick for PRN " + prn + " -- calling status endpoint.");
            await ConfirmStatusFromApiAsync(prn);
        }

        if (ct.IsCancellationRequested) return;

        TransactionRecord finalRec;
        if (_byPrn.TryGetValue(prn, out finalRec) && finalRec.Status == "PENDING")
        {
            LogWebhook("FP poller for PRN " + prn + " reached its 5-minute deadline still PENDING -- marking FAILED (timeout).");
            finalRec.Status = "FAILED";
            finalRec.Message = "Payment timed out.";

            LogWebhook("FP PRN " + prn + " timed out -- updating NiziPOS display to FAIL.");
            await SendNiziPosCommandAsync("FAIL**" + finalRec.Amount + "**Payment Failed");
            Task.Run(() => ResetDeviceToIdleAfterDelayAsync(TimeSpan.FromSeconds(5)));
        }
    }

    public static PaymentStatusResult CheckPaymentStatus(string prn)
    {
        if (string.IsNullOrEmpty(prn))
            return new PaymentStatusResult { Status = "NONE", Message = "No active transaction." };

        TransactionRecord record;
        if (!_byPrn.TryGetValue(prn, out record))
            return new PaymentStatusResult { Status = "NONE", Message = "Transaction expired or not found." };

        return new PaymentStatusResult { Status = record.Status, Message = record.Message };
    }

    // Used by QrDeviceImage.ashx to look up the qrMessage for a given prn so it can
    // render the JPEG without the client having to pass the raw EMV string around.
    public static string GetQrMessageForPrn(string prn)
    {
        TransactionRecord record;
        if (!string.IsNullOrEmpty(prn) && _byPrn.TryGetValue(prn, out record))
            return record.QrMessage;
        return null;
    }

    public class DeviceQrInfo
    {
        public string QrMessage;
        public string Amount; // already formatted "0.00" from RegisterTransaction
    }

    // Same lookup as GetQrMessageForPrn, but also returns the amount so QrDeviceImage.ashx
    // can pass it straight into GenerateQrJpegForDevice without a second round trip or the
    // caller having to carry the amount around separately.
    public static DeviceQrInfo GetDeviceQrInfoForPrn(string prn)
    {
        TransactionRecord record;
        if (!string.IsNullOrEmpty(prn) && _byPrn.TryGetValue(prn, out record))
            return new DeviceQrInfo { QrMessage = record.QrMessage, Amount = record.Amount };
        return null;
    }

    // ==================== NiziPOS device image rendering (fallback, currently unused) ====================

    // DEPRECATED for the live flow -- the QR now goes to NiziPOS via its own QR** command
    // (see PosLocalClient.js's showQrOnPos(), fed the raw qrMessage directly) instead of
    // being rasterized here. Left in place, along with QrDeviceImage.ashx and
    // PosLocalClient.js's showQrImageOnPos()/pushFonepayQrToDevice(), as a fallback in case
    // the QR** length limit noted at the top of this file turns out to still apply.
    //
    // Renders the EMV QR string to a JPEG sized for the NiziPOS display. This bypasses
    // the device's on-board text-to-QR encoder entirely (and its length cap), since the
    // image is fully rendered here and just uploaded as a picture via
    // POST /api/upload-image on the local NiziPOS service.
    //
    // IMPORTANT: QRCoder always produces a SQUARE bitmap, but the B30/B31 screen is
    // 240x320 (a 3:4 portrait rectangle). If we hand NiziPOS a square image and let IT
    // scale/fit the mismatched aspect ratio, it crops the overflow -- which is exactly
    // the "cut off at the edges" symptom. So instead we letterbox the QR onto a canvas
    // that already matches the device's exact screen size (white background, QR
    // centered, scaled to fit with margin), so there is no aspect-ratio mismatch left
    // for NiziPOS to "fix" by cropping.
    //
    // targetWidth/targetHeight should match your device's actual screen resolution
    // (240x320 for the B30/B31 per NiziPOS docs -- confirm against your unit).
    // pixelsPerModule controls the QR's own render density before it's scaled to fit;
    // leave the default unless the QR looks blurry after scaling.
    //
    // amountText/scanText are optional so existing callers (e.g. GetQrMessageForPrn-only
    // code, or QrDeviceImage.ashx before it's updated) keep compiling unchanged -- pass
    // null/empty to fall back to the old bare-QR rendering. When supplied, they're drawn
    // as a reserved text band above and below the QR (amount above, scan text below) --
    // the same "amount above / text below" layout the Dynamic QR popup uses for
    // lblQRAmount/imgQRPopup/lblQRStatus, just composited into one JPEG since the
    // physical device can only be handed a single image, not separate label elements.
    public static byte[] GenerateQrJpegForDevice(string qrMessage, string amountText = null, string scanText = null,
        int targetWidth = 240, int targetHeight = 320, int pixelsPerModule = 10)
    {
        if (string.IsNullOrEmpty(qrMessage))
            throw new ArgumentException("qrMessage is empty -- nothing to render.");

        using (var generator = new QRCodeGenerator())
        using (var qrData = generator.CreateQrCode(qrMessage, QRCodeGenerator.ECCLevel.L))
        using (var qrCode = new QRCode(qrData))
        using (Bitmap qrBmp = qrCode.GetGraphic(pixelsPerModule))
        using (var canvas = new Bitmap(targetWidth, targetHeight))
        using (var ms = new MemoryStream())
        {
            // Leave a small margin so the QR isn't touching the screen edges --
            // most scanners want a "quiet zone" around the code anyway.
            const int marginPx = 12;

            // Reserve a band at the top for the amount and at the bottom for the scan
            // text. Only reserve space for a band whose text was actually supplied, so
            // passing amountText/scanText as null keeps producing the old fully-centered
            // bare QR (no dead white space where a label would otherwise sit unused).
            int topBand = string.IsNullOrEmpty(amountText) ? 0 : 40;
            int bottomBand = string.IsNullOrEmpty(scanText) ? 0 : 34;

            int availableHeight = targetHeight - topBand - bottomBand - (marginPx * 2);
            int availableWidth = targetWidth - (marginPx * 2);
            int maxSide = Math.Min(availableWidth, availableHeight);
            if (maxSide < 10) maxSide = Math.Min(targetWidth, targetHeight); // safety net for tiny targets

            // Scale the square QR down (never up) to fit within maxSide x maxSide,
            // preserving its 1:1 aspect ratio -- this is the "contain" behavior
            // NiziPOS's own resize apparently doesn't do for us.
            int qrSide = Math.Min(maxSide, Math.Min(qrBmp.Width, qrBmp.Height));
            int destX = (targetWidth - qrSide) / 2;
            int destY = topBand + marginPx + Math.Max(0, (availableHeight - qrSide) / 2);

            using (var g = Graphics.FromImage(canvas))
            {
                g.Clear(Color.White);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                using (var centered = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    if (topBand > 0)
                    {
                        var amountRect = new RectangleF(4, 2, targetWidth - 8, topBand - 4);
                        using (var amountFont = new Font("Arial", 16, FontStyle.Bold))
                        using (var amountBrush = new SolidBrush(Color.Black))
                        {
                            g.DrawString("Rs. " + amountText, amountFont, amountBrush, amountRect, centered);
                        }
                    }

                    g.DrawImage(qrBmp, new Rectangle(destX, destY, qrSide, qrSide));

                    if (bottomBand > 0)
                    {
                        var scanRect = new RectangleF(4, targetHeight - bottomBand, targetWidth - 8, bottomBand - 2);
                        using (var scanFont = new Font("Arial", 11, FontStyle.Regular))
                        using (var scanBrush = new SolidBrush(Color.Black))
                        {
                            g.DrawString(scanText, scanFont, scanBrush, scanRect, centered);
                        }
                    }
                }
            }

            canvas.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }

    // ==================== per-transaction WebSocket listener ====================

    private static async Task ListenForPaymentAsync(string prn, string wsUrl, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(wsUrl)) return;

        try
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            using (var ws = new ClientWebSocket())
            {
                using (var connectCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
                {
                    connectCts.CancelAfter(TimeSpan.FromSeconds(15));
                    await ws.ConnectAsync(new Uri(wsUrl), connectCts.Token);
                }
                LogWebhook("FP WS connected for PRN " + prn);

                var buffer = new byte[8192];
                // Give the customer a few minutes to scan and pay before we give up on the socket
                // and fall back entirely on the client-side poll to CheckQRPaymentStatus.
                using (var overallCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
                {
                    overallCts.CancelAfter(TimeSpan.FromMinutes(3));

                    while (ws.State == WebSocketState.Open && !overallCts.IsCancellationRequested)
                    {
                        WebSocketReceiveResult result;
                        var ms = new MemoryStream();
                        do
                        {
                            result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), overallCts.Token);
                            if (result.MessageType == WebSocketMessageType.Close) break;
                            ms.Write(buffer, 0, result.Count);
                        } while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Close) break;

                        string msg = Encoding.UTF8.GetString(ms.ToArray());
                        LogWebhook("FP WS message for PRN " + prn + ": " + msg);

                        // IMPORTANT: do NOT trust paymentSuccess/success straight out of the
                        // push -- confirmed by testing, Fonepay's per-transaction socket also
                        // fires a message with those fields set at SCAN/verification time
                        // (customer's app successfully read the QR), not only at final
                        // settlement. Treating that as a completed payment is what was
                        // producing an instant "Success" the moment the QR was scanned,
                        // before any money had actually moved. So per this class's own
                        // documented design (see header comment), every push -- recognized
                        // shape or not -- is just a "something happened, go check" wake-up
                        // signal; LogRecognizedPushFields below only logs what the push
                        // claims for diagnostics/comparison, it never sets record.Status.
                        // The REST call is the sole source of truth for SUCCESS/FAILED.
                        LogRecognizedPushFields(prn, msg);

                        // Per current requirements, the device display is left alone here --
                        // it keeps showing the QR straight through the scan/verification push
                        // and only switches once ConfirmStatusFromApiAsync resolves the
                        // transaction to a real SUCCESS (PASS**) or the poller's timeout marks
                        // it FAILED (FAIL**). No intermediate "Please wait..." screen.
                        await ConfirmStatusFromApiAsync(prn);

                        TransactionRecord rec;
                        if (_byPrn.TryGetValue(prn, out rec) && rec.Status != "PENDING")
                            break;
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
            // normal on timeout / RegisterTransaction cleanup
        }
        catch (Exception ex)
        {
            LogWebhook("FP WS listener error for PRN " + prn + ": " + GetFullErrorMessage(ex));
        }
    }

    // Diagnostic only: parses the WS push's own claimed status purely so it's visible in
    // the log alongside the REST result that actually decides record.Status -- useful for
    // spotting cases like Fonepay's scan/verification push claiming paymentSuccess=true
    // well before the REST endpoint agrees a real payment has settled. This NEVER writes
    // to record.Status; ConfirmStatusFromApiAsync (REST) is the sole source of truth.
    //
    // CONFIRMED real shape (from an actual captured push):
    //   {
    //     "merchantId": "...", "deviceId": "...",
    //     "transactionStatus": "{\"traceId\":...,\"productNumber\":\"<prn>\",\"amount\":1.0,
    //                             \"message\":\"RES000\",\"success\":true,
    //                             \"paymentSuccess\":true, ...}",
    //     "socketUrl": "..."
    //   }
    // Note "transactionStatus" is a STRING containing escaped/nested JSON, not a nested
    // object directly -- it has to be deserialized a second time.
    private static void LogRecognizedPushFields(string prn, string message)
    {
        var serializer = new JavaScriptSerializer();
        Dictionary<string, object> outer;
        try
        {
            outer = serializer.Deserialize<Dictionary<string, object>>(message);
        }
        catch
        {
            LogWebhook("FP WS push for PRN " + prn + " was not JSON (e.g. a bare ping) -- nothing to log, querying REST anyway.");
            return;
        }
        if (outer == null) return;

        object txnStatusRaw;
        if (!outer.TryGetValue("transactionStatus", out txnStatusRaw) || txnStatusRaw == null)
        {
            string flatStatus = FirstNonEmptyValue(outer, "paymentStatus", "payment_status", "status");
            if (!string.IsNullOrEmpty(flatStatus))
                LogWebhook("FP WS push for PRN " + prn + " claims flat status '" + flatStatus + "' (not acted on directly -- confirming via REST).");
            else
                LogWebhook("FP WS push for PRN " + prn + " had no recognized status field (nested or flat) -- likely a connection handshake/ack. Confirming via REST.");
            return;
        }

        Dictionary<string, object> inner;
        try
        {
            inner = serializer.Deserialize<Dictionary<string, object>>(txnStatusRaw.ToString());
        }
        catch (Exception ex)
        {
            LogWebhook("FP WS push for PRN " + prn + " had a transactionStatus field that didn't parse as JSON: " + ex.Message);
            return;
        }
        if (inner == null) return;

        object val;
        bool? paidOk = null;
        if (inner.TryGetValue("paymentSuccess", out val) && val != null)
            paidOk = Convert.ToBoolean(val);
        else if (inner.TryGetValue("success", out val) && val != null)
            paidOk = Convert.ToBoolean(val);

        string code = inner.ContainsKey("message") && inner["message"] != null ? inner["message"].ToString() : null;

        LogWebhook("FP WS push for PRN " + prn + " claims (via nested transactionStatus) paymentSuccess/success=" + paidOk +
            " code='" + code + "' -- NOT acted on directly (this field is set at scan/verification time too, not just final settlement); confirming via REST instead.");
    }

    private static string FirstNonEmptyValue(Dictionary<string, object> json, params string[] keys)
    {
        foreach (var key in keys)
        {
            foreach (var kvp in json)
            {
                if (string.Equals(kvp.Key, key, StringComparison.OrdinalIgnoreCase) && kvp.Value != null)
                {
                    string val = kvp.Value.ToString();
                    if (!string.IsNullOrEmpty(val)) return val;
                }
            }
        }
        return null;
    }

    private static async Task ConfirmStatusFromApiAsync(string prn)
    {
        TransactionRecord record;
        if (!_byPrn.TryGetValue(prn, out record)) return;

        DYNAMIC_QR DQEnt = GetSettings();

        // Message for HMAC_SHA512 => {PRN},{MERCHANT-CODE}
        string message = string.Join(",", prn, DQEnt.FONEPAY_MERCHANTCODE);
        string dataValidation = ComputeHmacSha512(DQEnt.FONEPAY_SECRETKEY, message);

        var serializer = new JavaScriptSerializer();
        var requestObj = new Dictionary<string, string>
        {
            { "prn", prn },
            { "merchantCode", DQEnt.FONEPAY_MERCHANTCODE },
            { "dataValidation", dataValidation },
            { "username", DQEnt.FONEPAY_USERNAME },
            { "password", DQEnt.FONEPAY_PASSWORD }
        };
        string requestJson = serializer.Serialize(requestObj);

        try
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            using (var client = new HttpClient())
            {
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(FONEPAY_BASEURL + QR_STATUS_PATH, content);
                string body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    LogWebhook("FP status check HTTP error for PRN " + prn + ": " + body);
                    return;
                }

                var result = serializer.Deserialize<Dictionary<string, object>>(body);
                string status = result.ContainsKey("paymentStatus") ? result["paymentStatus"].ToString() : "unknown";

                // Only "success" is ever trusted here. Fonepay's sandbox reports
                // "expired"/"failed"/"cancelled" for a QR that's simply been scanned but
                // not yet paid -- these are NOT reliable evidence of an actual failure, so
                // every non-success reading is treated as still PENDING. A genuine FAILED
                // is only ever set by PollStatusPeriodicallyAsync's own deadline (a real
                // timeout with no successful payment), never from a single status read.
                string mapped = string.Equals((status ?? "").Trim(), "success", StringComparison.OrdinalIgnoreCase)
                    ? "SUCCESS"
                    : "PENDING";

                LogWebhook("FP status check (REST) for PRN " + prn + ": raw response = " + body + " | parsed paymentStatus = '" + status + "' | mapped = " + mapped);

                // Never downgrade away from a status another path already set (e.g. don't
                // stomp a SUCCESS/FAILED that was already resolved) -- only advance PENDING.
                if (record.Status != "PENDING") return;

                record.Status = mapped;
                record.Message = mapped == "SUCCESS" ? "Payment received." : "Waiting for payment...";

                if (mapped == "SUCCESS")
                {
                    LogWebhook("FP PRN " + prn + " confirmed SUCCESS via REST -- updating NiziPOS display to PASS.");
                    await SendNiziPosCommandAsync("PASS**Payment Received!**Rs. " + record.Amount + " received");
                    Task.Run(() => ResetDeviceToIdleAfterDelayAsync(TimeSpan.FromSeconds(5)));
                }
            }
        }
        catch (Exception ex)
        {
            LogWebhook("FP status check error for PRN " + prn + ": " + GetFullErrorMessage(ex));
        }
    }

    // ==================== NiziPOS display commands ====================

    // Fire-and-forget style helper (caller decides whether to await inline or Task.Run
    // it) that pushes a raw command string to the local NiziPOS background service.
    // Never throws -- a display hiccup should never take down the payment flow itself.
    private static async Task SendNiziPosCommandAsync(string command)
    {
        DYNAMIC_QR DQEnt = GetSettings();

        if (string.IsNullOrEmpty(DQEnt.POS_API_KEY) || DQEnt.POS_API_KEY == "nizipos-fixed-secret-token")
        {
            LogWebhook("*** NiziPOS command '" + command + "' SKIPPED -- NIZIPOS_APIKEY in the DYNAMIC_QR settings row is empty or still the placeholder value. " +
                "Update it with your real token from the NiziPOS distribution email. ***");
            return;
        }

        try
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", DQEnt.POS_API_KEY);
                var serializer = new JavaScriptSerializer();
                string requestJson = serializer.Serialize(new Dictionary<string, string> { { "command", command } });
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(DQEnt.POS_BASE_URL + NIZIPOS_COMMAND_PATH, content);
                if (!response.IsSuccessStatusCode)
                {
                    string body = await response.Content.ReadAsStringAsync();
                    LogWebhook("NiziPOS command '" + command + "' failed: HTTP " + (int)response.StatusCode + " - " + body);
                }
            }
        }
        catch (Exception ex)
        {
            LogWebhook("NiziPOS command '" + command + "' threw: " + GetFullErrorMessage(ex));
        }
    }

    // Per the NiziPOS recommended flow diagram: after PASS/FAIL is shown, wait a few
    // seconds so the customer/cashier can actually read the result, then return the
    // device to its idle screen. Meant to be Task.Run()'d, not awaited inline.
    private static async Task ResetDeviceToIdleAfterDelayAsync(TimeSpan delay)
    {
        try { await Task.Delay(delay); } catch { }
        await SendNiziPosCommandAsync("IDLE");
    }

    // ==================== helpers ====================

    private static string ComputeHmacSha512(string key, string message)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            byte[] hash = hmac.ComputeHash(messageBytes);
            var sb = new StringBuilder();
            foreach (byte b in hash) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    private static string GetFullErrorMessage(Exception ex)
    {
        string details = ex.Message;
        var inner = ex.InnerException;
        while (inner != null) { details += " -> " + inner.Message; inner = inner.InnerException; }
        return details;
    }

    public static void LogWebhook(string message)
    {
        try
        {
            string logPath = HostingEnvironment.MapPath("~/App_Data/fonepay_webhook_log.txt");
            string line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " | " + message + Environment.NewLine;
            File.AppendAllText(logPath, line);
        }
        catch (Exception ex)
        {
            try { System.Diagnostics.Trace.WriteLine("LogWebhook failed: " + ex.Message + " | original: " + message); } catch { }
        }
    }
}