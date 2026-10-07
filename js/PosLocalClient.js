// PosLocalClient.js
//
// Runs in the CASHIER'S browser — on the same PC as the NiziPOS background
// service and the connected UART display. Because it targets 127.0.0.1, it
// always reaches the device attached to whichever machine the user is
// physically sitting at. The server can never do this reliably (NiziPOS
// returns 403 for any non-localhost caller), so this logic must live here,
// not in server-side C#.

// Fetched from the server (which reads from the DB) instead of being
// hardcoded here: apiKey and baseUrl. Cached in memory only — not
// localStorage/sessionStorage — so it lasts for this page load only.
let _posConfigPromise = null;

async function getPosConfig() {
    if (!_posConfigPromise) {
        _posConfigPromise = fetch((window.POS_APP_ROOT || "/") + "PosConfig.ashx", { credentials: "same-origin" }).then((res) => {
            if (!res.ok) throw new Error("Could not load POS config from server (are you logged in?).");
            return res.json();
        });
    }
    return _posConfigPromise;
}

async function posGetStatus() {
    const { apiKey, baseUrl } = await getPosConfig();
    const res = await fetch(`${baseUrl}/api/status`, {
        headers: { "X-API-Key": apiKey }
    });
    return res.json();
}

async function posConnect(port = null) {
    const { apiKey, baseUrl } = await getPosConfig();
    const res = await fetch(`${baseUrl}/api/connect`, {
        method: "POST",
        headers: { "X-API-Key": apiKey, "Content-Type": "application/json" },
        body: JSON.stringify({ port })
    });
    return res.json();
}

    async function posSendCommand(command) {
        const { apiKey, baseUrl } = await getPosConfig();
        const res = await fetch(`${baseUrl}/api/command`, {
            method: "POST",
            headers: { "X-API-Key": apiKey, "Content-Type": "application/json" },
            body: JSON.stringify({ command })
        });
        return res.json();
    }

    async function posEnsureConnected() {
        const status = await posGetStatus();
        if (status.connected) return true;
        // "port": null tells the LOCAL service (on this machine) to auto-detect
        // its own COM port — exactly what the docs recommend.
        const result = await posConnect(null);
        return !!result.success;
    }

    // Sends the raw QR payload straight to NiziPOS's on-device QR** command --
    // NiziPOS renders/encodes the QR itself, so nothing is rasterized here or
    // server-side. This is the path FONEPAY_POS_TEST.aspx now uses for Fonepay's
    // qrMessage as well (see that page's window.onload).
    //
    // NOTE: an earlier round of testing found this command truncates/fails to
    // render payloads as long as Fonepay's full EMV qrMessage string -- that's
    // why showQrImageOnPos()/pushFonepayQrToDevice() below were added as a
    // workaround. If you're seeing a garbled/blank QR on the physical device
    // again, that limit is almost certainly why -- confirm against your unit's
    // current firmware before assuming it's fixed.
    async function showQrOnPos(amount, scanLabel, qrPayload) {
        const ok = await posEnsureConnected();
        if (!ok) throw new Error("Could not connect to local POS device.");
        const command = `QR**Rs. ${amount.toFixed(2)}**${scanLabel}**${qrPayload}`;
        return posSendCommand(command);
    }

    // DEPRECATED for the Fonepay flow (FONEPAY_POS_TEST.aspx now calls
    // showQrOnPos() directly with the raw qrMessage instead). Left in place in
    // case the QR** length limit noted above turns out to still be real on your
    // hardware and you need to fall back to image upload.
    //
    // Pushes a pre-rendered JPEG to the device via NiziPOS's
    // POST /api/upload-image endpoint, instead of asking the device to encode
    // text into a QR itself.
    //
    // jpegBlob: a Blob/File of JPEG image data.
    // size: optional target size string per NiziPOS docs (e.g. "320x480").
    //       Leave undefined to let NiziPOS use its own default/auto-fit —
    //       test both against your actual screen (docs example is 320x480,
    //       but the B30/B31 reports 240x320, so confirm which works for you).
    async function showQrImageOnPos(jpegBlob, size) {
        const ok = await posEnsureConnected();
        if (!ok) throw new Error("Could not connect to local POS device.");

        const { apiKey, baseUrl } = await getPosConfig();
        const formData = new FormData();
        formData.append("image", jpegBlob, "qr.jpg");
        if (size) formData.append("size", size);

        const res = await fetch(`${baseUrl}/api/upload-image`, {
            method: "POST",
            // Do NOT set Content-Type manually here — the browser needs to set
            // its own multipart boundary for FormData to be parsed correctly.
            headers: { "X-API-Key": apiKey },
            body: formData
        });
        return res.json();
    }
    window.showQrImageOnPos = showQrImageOnPos;

    // DEPRECATED for the Fonepay flow -- see showQrOnPos()/showQrImageOnPos()
    // notes above. Kept only as a fallback if the QR** command's length limit
    // turns out to still apply on your hardware; fetches the server-rendered
    // JPEG for a given prn (from QrDeviceImage.ashx) and pushes it to the device.
    async function pushFonepayQrToDevice(prn, size) {
        const resp = await fetch((window.POS_APP_ROOT || "/") + `QrDeviceImage.ashx?prn=${encodeURIComponent(prn)}`, {
            credentials: "same-origin"
        });
        if (!resp.ok) {
            const text = await resp.text().catch(() => "");
            throw new Error(`Could not fetch QR image for prn ${prn}: HTTP ${resp.status} ${text}`);
        }
        const blob = await resp.blob();
        return showQrImageOnPos(blob, size);
    }
    window.pushFonepayQrToDevice = pushFonepayQrToDevice;

    async function showWaitOnPos(amount, message = "Please wait...") {
        return posSendCommand(`WAIT**${amount.toFixed(2)}**${message}`);
    }

        async function showPassOnPos(title = "SUCCESS!", message = "Payment successful") {
            return posSendCommand(`PASS**${title}**${message}`);
        }

            async function showFailOnPos(amount, message = "Payment Failed") {
                return posSendCommand(`FAIL**${amount.toFixed(2)}**${message}`);
            }

                async function idlePos() {
                    return posSendCommand("IDLE");
                }

                // ---- ASP.NET AJAX Page Method helper ----
                //
                // Assumes the host page has <asp:ScriptManager EnablePageMethods="true" />
                // somewhere on it — that's what makes POSTing to "<page>.aspx/MethodName"
                // with a JSON body work and return { d: <result> }. If the page doesn't
                // have a ScriptManager with EnablePageMethods enabled, this will 404 and
                // this helper needs to be pointed at a .ashx handler instead.
                async function callPageMethod(methodName, params) {
                    const res = await fetch(window.location.pathname + "/" + methodName, {
                        method: "POST",
                        headers: { "Content-Type": "application/json; charset=utf-8" },
                        body: JSON.stringify(params || {}),
                        credentials: "same-origin"
                    });
                    if (!res.ok) {
                        throw new Error("Page method " + methodName + " failed: HTTP " + res.status);
                    }
                    const data = await res.json();
                    return data.d;
                }

                // --- Page wiring: status display + auto-connect + send pending command on load ---
                //
                // Expects the host page to contain:
                //   <div id="posStatus"></div>
                //   <div id="lblPosStatus"></div>
                // The pending QR command and its amount are pushed from the server via
                // Page.ClientScript.RegisterHiddenField("hfQrCommand", ...) /
                // RegisterHiddenField("hfAmount", ...) in btnGenerateQR_Click — no markup
                // changes needed, ASP.NET renders these as <input type="hidden"> for us.
                (function () {
                    function setStatus(el, text, color) {
                        if (!el) return;
                        el.textContent = text;
                        el.style.color = color || '';
                    }

                    // Sends whatever's sitting in hfQrCommand to the local POS device, then
                    // clears it so it isn't re-sent on a later postback. Safe to call more
                    // than once — a no-op if the hidden field is empty.
                    async function sendPendingPosCommandIfAny(connected) {
                        var lblPosStatus = document.getElementById('lblPosStatus');
                        var hidden = document.getElementById('hfQrCommand');
                        var command = hidden ? hidden.value : '';
                        if (!command) return;

                        if (connected) {
                            try {
                                await posSendCommand(command);
                                if (lblPosStatus) lblPosStatus.textContent = 'Sent to POS device.';
                            } catch (err) {
                                if (lblPosStatus) lblPosStatus.textContent = 'POS send error: ' + err.message;
                            }
                        } else {
                            if (lblPosStatus) lblPosStatus.textContent = 'POS device not connected - command not sent.';
                        }
                        hidden.value = '';
                    }
                    window.sendPendingPosCommandIfAny = sendPendingPosCommandIfAny;

                    async function ensureConnectedWithStatus() {
                        var posStatusEl = document.getElementById('posStatus');
                        setStatus(posStatusEl, 'Checking for POS device on this terminal...', '');

                        var connected = false;
                        try {
                            var data = await posGetStatus();
                            if (data.connected) {
                                setStatus(posStatusEl, 'POS device connected (' + data.port + ').', 'green');
                                connected = true;
                            } else {
                                setStatus(posStatusEl, 'No POS device connected. Attempting to connect...', '#b00');
                            }
                        } catch (err) {
                            setStatus(posStatusEl, 'Could not reach POS service on this terminal: ' + err.message, '#b00');
                        }

                        if (!connected) {
                            try {
                                var connectData = await posConnect(null);
                                if (connectData.success) {
                                    setStatus(posStatusEl, 'POS device connected (' + connectData.port + ').', 'green');
                                    connected = true;
                                } else {
                                    setStatus(posStatusEl, 'Connect failed: ' + (connectData.error || 'unknown error'), '#b00');
                                }
                            } catch (err) {
                                setStatus(posStatusEl, 'Connect error: ' + err.message, '#b00');
                            }
                        }
                        return connected;
                    }

                    async function initPosOnPage() {
                        var connected = await ensureConnectedWithStatus();
                        await sendPendingPosCommandIfAny(connected);
                    }

                    window.addEventListener('load', function () {
                        initPosOnPage();
                    });

                    // --- Payment status polling, wired to the actual PASS/FAIL push ---
                    //
                    // Started by the server via
                    //   ClientScript.RegisterStartupScript(GetType(), "startPolling", "startStatusPolling();", true);
                    // after a QR is generated. Polls CheckPaymentStatus (server reads the
                    // active transaction from Session — no billNumber needs to be passed
                    // from the client) every 2s until SUCCESS/FAILED/timeout, then pushes
                    // the matching command to the local POS device and idles it.

                    var _pollTimer = null;
                    var POLL_INTERVAL_MS = 2000;
                    var POLL_MAX_MS = 5 * 60 * 1000; // give up after 5 minutes
                    var IDLE_DELAY_MS = 5000; // let the PASS/FAIL screen sit for a bit before idling

                    async function startStatusPolling() {
                        if (_pollTimer) {
                            clearInterval(_pollTimer);
                            _pollTimer = null;
                        }

                        var startedAt = Date.now();
                        var amountEl = document.getElementById('hfAmount');
                        var amount = amountEl ? (parseFloat(amountEl.value) || 0) : 0;

                        var lblPosStatus = document.getElementById('lblPosStatus');

                        _pollTimer = setInterval(async function () {
                            if (Date.now() - startedAt > POLL_MAX_MS) {
                                clearInterval(_pollTimer);
                                _pollTimer = null;
                                try { await showFailOnPos(amount, "Payment timed out"); }
                                catch (e) { console.error("Failed to push timeout FAIL to POS:", e); }
                                setTimeout(function () { idlePos().catch(function () {}); }, IDLE_DELAY_MS);
                                if (lblPosStatus) lblPosStatus.textContent = 'Payment timed out.';
                                return;
                            }

                            var status;
                            try {
                                status = await callPageMethod('CheckPaymentStatus', {});
                            } catch (err) {
                                // Transient poll failure (network blip, etc.) — try again next tick
                                // rather than aborting the whole flow.
                                console.error("CheckPaymentStatus poll failed:", err);
                                return;
                            }

                            if (!status || status.Status === 'NONE' || status.Status === 'PENDING') {
                                return; // keep polling
                            }

                            clearInterval(_pollTimer);
                            _pollTimer = null;

                            if (status.Status === 'SUCCESS') {
                                if (lblPosStatus) lblPosStatus.textContent = status.Message || 'Payment successful.';
                                try { await showPassOnPos('SUCCESS!', status.Message || 'Payment successful'); }
                                catch (e) { console.error('Failed to push PASS to POS:', e); }
                            } else {
                                if (lblPosStatus) lblPosStatus.textContent = status.Message || 'Payment failed.';
                                try { await showFailOnPos(amount, status.Message || 'Payment failed'); }
                                catch (e) { console.error('Failed to push FAIL to POS:', e); }
                            }

                            setTimeout(async function () {
                                try { await idlePos(); } catch (e) { /* best effort */ }
                                try { await callPageMethod('ResetToIdle', {}); } catch (e) { /* best effort */ }
                            }, IDLE_DELAY_MS);
                        }, POLL_INTERVAL_MS);
                    }
                    window.startStatusPolling = startStatusPolling;

                    // --- QR popup polling for ProductSales.aspx (Utilities_Sales_ProductSales) ---
                    // and reused as-is by FONEPAY_POS_TEST.aspx for the Fonepay flow --
                    // both pages call this instead of hand-rolling their own poll loop.
                    //
                    // Started via: showQRPopup(); startQRPolling(); showQrOnPos(...)...
                    // Polls CheckQRPaymentStatus (the calling page's own WebMethod, reads the
                    // active transaction from Session server-side — nothing to pass from the
                    // client) every 2s until SUCCESS/FAILED/timeout, pushes the matching
                    // PASS/FAIL command to the local POS device, then hides the popup (if
                    // hideQRPopup exists on the page) and idles the device.
                    //
                    // options (all optional, default to ProductSales.aspx's original element
                    // ids so existing callers with no args are unaffected):
                    //   statusElementId — id of the element whose textContent shows the result
                    //   amountElementId — id of the element read for the fallback FAIL/timeout amount

                    var _qrPollTimer = null;
                    var QR_POLL_INTERVAL_MS = 2000;
                    var QR_POLL_MAX_MS = 5 * 60 * 1000; // give up after 5 minutes
                    var QR_IDLE_DELAY_MS = 4000; // let the PASS/FAIL screen sit for a bit before idling

                    function readQrAmount(amountElementId) {
                        var el = document.getElementById(amountElementId || 'lblQRAmount');
                        if (!el) return 0;
                        var cleaned = (el.textContent || el.value || '').replace(/[^0-9.]/g, '');
                        return parseFloat(cleaned) || 0;
                    }

                    function finishQrFlow() {
                        setTimeout(function () {
                            if (typeof hideQRPopup === 'function') {
                                try { hideQRPopup(); } catch (e) { /* best effort */ }
                            }
                            idlePos().catch(function (e) { console.error('POS idle error:', e); });
                        }, QR_IDLE_DELAY_MS);
                    }

                    async function startQRPolling(options) {
                        options = options || {};
                        var statusElementId = options.statusElementId || 'lblQRStatus';
                        var amountElementId = options.amountElementId || 'lblQRAmount';

                        if (_qrPollTimer) {
                            clearInterval(_qrPollTimer);
                            _qrPollTimer = null;
                        }

                        var startedAt = Date.now();
                        var amount = readQrAmount(amountElementId);
                        var lblQRStatus = document.getElementById(statusElementId);

                        _qrPollTimer = setInterval(async function () {
                            if (Date.now() - startedAt > QR_POLL_MAX_MS) {
                                clearInterval(_qrPollTimer);
                                _qrPollTimer = null;
                                if (lblQRStatus) lblQRStatus.textContent = 'Payment timed out.';
                                try { await showFailOnPos(amount, "Payment timed out"); }
                                catch (e) { console.error("Failed to push timeout FAIL to POS:", e); }
                                finishQrFlow();
                                return;
                            }

                            var status;
                            try {
                                status = await callPageMethod('CheckQRPaymentStatus', {});
                            } catch (err) {
                                // Transient poll failure — try again next tick rather than aborting.
                                console.error("CheckQRPaymentStatus poll failed:", err);
                                return;
                            }

                            if (!status || status.Status === 'NONE' || status.Status === 'PENDING') {
                                return; // keep polling
                            }

                            clearInterval(_qrPollTimer);
                            _qrPollTimer = null;

                            if (status.Status === 'SUCCESS') {
                                if (lblQRStatus) lblQRStatus.textContent = status.Message || 'Payment successful.';
                                try { await showPassOnPos('SUCCESS!', status.Message || 'Payment successful'); }
                                catch (e) { console.error('Failed to push PASS to POS:', e); }
                            } else {
                                if (lblQRStatus) lblQRStatus.textContent = status.Message || 'Payment failed.';
                                try { await showFailOnPos(amount, status.Message || 'Payment failed'); }
                                catch (e) { console.error('Failed to push FAIL to POS:', e); }
                            }

                            finishQrFlow();
                        }, QR_POLL_INTERVAL_MS);
                    }
                    window.startQRPolling = startQRPolling;
                    window.stopQRPolling = function () {
                        if (_qrPollTimer) { clearInterval(_qrPollTimer); _qrPollTimer = null; }
                    };
                })();
