<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FonepayTest.aspx.cs" Inherits="FonepayTest" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8" />
    <title>Fonepay Dynamic QR - Test Page</title>
    <script src="<%= ResolveUrl("~/js/qrcode.min.js") %>"></script>
    <%--<script src="<%= ResolveUrl("~/js/PosLocalClient.js") %>"></script>--%>
    <style>
        * {
            box-sizing: border-box;
        }

        body {
            font-family: -apple-system, Segoe UI, Roboto, Helvetica, Arial, sans-serif;
            background: #f4f5f7;
            color: #1d1f22;
            margin: 0;
            padding: 32px 16px;
        }

        .card {
            max-width: 480px;
            margin: 0 auto;
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.08), 0 4px 12px rgba(0,0,0,0.06);
            padding: 28px;
        }

        h1 {
            font-size: 20px;
            margin: 0 0 4px;
            display: flex;
            align-items: center;
            gap: 8px;
        }

            h1 .dot {
                width: 10px;
                height: 10px;
                border-radius: 50%;
                background: #e5252c;
                display: inline-block;
            }

        .sub {
            color: #6b7280;
            font-size: 13px;
            margin: 0 0 24px;
        }

        label {
            display: block;
            font-size: 13px;
            font-weight: 600;
            margin: 14px 0 6px;
        }

        input {
            width: 100%;
            padding: 10px 12px;
            border: 1px solid #d1d5db;
            border-radius: 8px;
            font-size: 14px;
        }

            input:focus {
                outline: none;
                border-color: #e5252c;
            }

        button {
            width: 100%;
            margin-top: 20px;
            padding: 12px;
            border: none;
            border-radius: 8px;
            background: #e5252c;
            color: #fff;
            font-size: 15px;
            font-weight: 600;
            cursor: pointer;
        }

            button:disabled {
                background: #f3a5a8;
                cursor: not-allowed;
            }

            button.secondary {
                background: #fff;
                color: #e5252c;
                border: 1px solid #e5252c;
                margin-top: 8px;
            }

        #qrCanvas {
            display: flex;
            justify-content: center;
            align-items: center;
            margin: 20px auto;
        }

            #qrCanvas canvas,
            #qrCanvas img {
                width: 256px;
                height: 256px;
                padding: 8px;
                background: white;
                box-shadow: 0 0 0 1px #eee;
                border-radius: 8px;
            }

        .status-badge {
            display: inline-block;
            margin-top: 14px;
            padding: 6px 14px;
            border-radius: 999px;
            font-size: 13px;
            font-weight: 600;
        }

        .status-PENDING {
            background: #fff4e5;
            color: #a15c00;
        }

        .status-SUCCESS {
            background: #e6f7ec;
            color: #1a7a3f;
        }

        .status-FAILED {
            background: #fdeceb;
            color: #b3261e;
        }

        .status-NONE {
            background: #f0f1f3;
            color: #555;
        }

        .meta {
            margin-top: 10px;
            font-size: 12px;
            color: #777;
            word-break: break-all;
        }

        .error {
            color: #b3261e;
            font-size: 13px;
            margin-top: 10px;
            display: none;
        }

        .spin {
            display: inline-block;
            width: 14px;
            height: 14px;
            margin-right: 6px;
            border: 2px solid rgba(255,255,255,0.5);
            border-top-color: #fff;
            border-radius: 50%;
            animation: spin 0.7s linear infinite;
            vertical-align: -2px;
        }

        @keyframes spin {
            to {
                transform: rotate(360deg);
            }
        }
    </style>
</head>
<body>

    <div class="card">
        <h1><span class="dot"></span>Fonepay Dynamic QR - Test</h1>
        <p class="sub">Generates a live QR against your configured Fonepay merchant settings.</p>

        <label for="amount">Amount (NPR)</label>
        <input id="amount" type="number" min="1" step="0.01" value="10" />

        <label for="remarks1">Remarks 1</label>
        <input id="remarks1" type="text" value="Test payment" maxlength="25" />

        <label for="remarks2">Remarks 2</label>
        <input id="remarks2" type="text" value="Test page" maxlength="25" />

        <button type="button" id="generateBtn">Generate QR</button>
        <div class="error" id="errorBox"></div>

        <div class="qr-wrap" id="qrWrap">
            <div id="qrCanvas"></div>
            <div>
                <span class="status-badge status-PENDING" id="statusBadge">PENDING</span>
            </div>
            <div class="meta" id="statusMessage">Waiting for payment...</div>
            <div class="meta" id="prnMeta"></div>
            <button type="button" class="secondary" id="checkNowBtn">Check status now</button>
            <button type="button" class="secondary" id="resetBtn">Start over</button>
        </div>
    </div>

    <script>
        (function () {
            var generateBtn = document.getElementById('generateBtn');
            var checkNowBtn = document.getElementById('checkNowBtn');
            var resetBtn = document.getElementById('resetBtn');
            var errorBox = document.getElementById('errorBox');
            var qrWrap = document.getElementById('qrWrap');
            //var qrCanvas = document.getElementById('qrCanvas');
            var statusBadge = document.getElementById('statusBadge');
            var statusMessage = document.getElementById('statusMessage');
            var prnMeta = document.getElementById('prnMeta');

            var currentPrn = null;
            var pollTimer = null;

            function showError(msg) {
                errorBox.textContent = msg;
                errorBox.style.display = msg ? 'block' : 'none';
            }

            function setStatus(status, message) {
                statusBadge.className = 'status-badge status-' + status;
                statusBadge.textContent = status;
                statusMessage.textContent = message || '';
            }

            function stopPolling() {
                if (pollTimer) { clearInterval(pollTimer); pollTimer = null; }
            }

            function resetUi() {
                stopPolling();
                currentPrn = null;
                qrWrap.style.display = 'none';
                showError('');
                generateBtn.disabled = false;
                generateBtn.textContent = 'Generate QR';
            }

            async function pollStatus() {
                if (!currentPrn) return;
                try {
                    var res = await fetch('FonepayApi.aspx?action=status&prn=' + encodeURIComponent(currentPrn));
                    var data = await res.json();
                    setStatus(data.status, data.message);
                    if (data.status === 'SUCCESS' || data.status === 'FAILED') {
                        stopPolling();
                    }
                } catch (e) {
                    // transient network hiccup while polling -- keep trying
                }
            }

            async function checkNow() {
                if (!currentPrn) return;
                checkNowBtn.disabled = true;
                try {
                    var res = await fetch('FonepayApi.aspx?action=statusServer&prn=' + encodeURIComponent(currentPrn));
                    var data = await res.json();
                    setStatus(data.status, data.message);
                    if (data.status === 'SUCCESS' || data.status === 'FAILED') {
                        stopPolling();
                    }
                } catch (e) {
                    showError('Could not reach the server to check status.');
                } finally {
                    checkNowBtn.disabled = false;
                }
            }

            async function generate() {
                showError('');

                var amount = document.getElementById('amount').value;
                var remarks1 = document.getElementById('remarks1').value;
                var remarks2 = document.getElementById('remarks2').value;

                if (!amount || Number(amount) <= 0) {
                    showError('Enter a valid amount.');
                    return;
                }

                generateBtn.disabled = true;
                generateBtn.innerHTML =
                    '<span class="spin"></span>Generating...';

                try {
                    var url = 'FonepayApi.aspx?action=generate'
                        + '&amount=' + encodeURIComponent(amount)
                        + '&remarks1=' + encodeURIComponent(remarks1)
                        + '&remarks2=' + encodeURIComponent(remarks2);

                    var res = await fetch(url);
                    var data = await res.json();

                    if (!data.success) {
                        showError(data.error || 'Failed to generate QR.');
                        generateBtn.disabled = false;
                        generateBtn.textContent = 'Generate QR';
                        return;
                    }

                    if (!data.qrMessage) {
                        throw new Error(
                            'Fonepay did not return a QR message.'
                        );
                    }

                    currentPrn = data.prn;

                    prnMeta.textContent =
                        'PRN: ' + data.prn;

                    var qrCanvas =
                        document.getElementById('qrCanvas');

                    qrCanvas.innerHTML = '';

                    qrWrap.style.display = 'block';

                    setStatus(
                        'PENDING',
                        'Waiting for payment...'
                    );

                    if (typeof QRCode === 'undefined') {
                        throw new Error(
                            'QRCode library was not loaded.'
                        );
                    }

                    new QRCode(qrCanvas, {
                        text: data.qrMessage,
                        width: 256,
                        height: 256,
                        correctLevel: QRCode.CorrectLevel.M
                    });

                    console.log(
                        'QR generated successfully.'
                    );

                    generateBtn.textContent =
                        'QR generated';

                    stopPolling();

                    pollTimer =
                        setInterval(
                            pollStatus,
                            2000
                        );

                } catch (e) {

                    showError(
                        'Unexpected error: ' +
                        e.message
                    );

                    generateBtn.disabled = false;

                    generateBtn.textContent =
                        'Generate QR';
                }
            }

            generateBtn.addEventListener('click', generate);
            checkNowBtn.addEventListener('click', checkNow);
            resetBtn.addEventListener('click', resetUi);
        })();
</script>

</body>
</html>
