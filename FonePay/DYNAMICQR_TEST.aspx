<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DYNAMICQR_TEST.aspx.cs" Inherits="DYNAMICQR_TEST" Async="true" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Fonepay Dynamic QR - Test Page</title>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="max-width: 480px; margin: 30px auto; font-family: Arial, sans-serif;">

            <h3>Fonepay Dynamic QR - Test Page</h3>

            <div>
                <label>Amount (NPR)</label><br />
                <asp:TextBox ID="txtAmount" runat="server" placeholder="e.g. 10" />
            </div>

            <div style="margin-top: 10px;">
                <label>Remarks 1</label><br />
                <asp:TextBox ID="txtRemarks1" runat="server" Text="test1" />
            </div>

            <div style="margin-top: 10px;">
                <label>Remarks 2</label><br />
                <asp:TextBox ID="txtRemarks2" runat="server" Text="test2" />
            </div>

            <div style="margin-top: 15px;">
                <asp:Button ID="btnGenerateQR" runat="server" Text="Generate QR" OnClick="btnGenerateQR_Click" />
                <asp:Button ID="btnCheckStatus" runat="server" Text="Check Status" OnClick="btnCheckStatus_Click" />
            </div>

            <div id="qrcode" style="margin-top: 20px;"></div>

            <p><b>PRN:</b> <asp:Label ID="lblPrn" runat="server" /></p>
            <p><b>Status:</b> <asp:Label ID="lblStatus" runat="server" Text="Idle" /></p>

            <asp:HiddenField ID="hfQrMessage" runat="server" />
            <asp:HiddenField ID="hfWsUrl" runat="server" />
        </div>

        <script type="text/javascript">
            var ws = null;

            function setStatus(text) {
                var el = document.getElementById('<%= lblStatus.ClientID %>');
                if (el) el.innerText = text;
            }

            function renderQr(text) {
                var holder = document.getElementById('qrcode');
                holder.innerHTML = '';
                if (text) {
                    try {
                        new QRCode(holder, {
                            text: text,
                            width: 260,
                            height: 260,
                            correctLevel: QRCode.CorrectLevel.L  // long EMV strings need max capacity
                        });
                    } catch (err) {
                        holder.innerHTML = '<span style="color:red;">QR render failed: ' + err.message + '</span>';
                        console.error('QRCode render error:', err, 'text length:', text.length);
                    }
                }
            }

            function connectWs(url) {
                if (!url) return;
                if (ws) { try { ws.close(); } catch (e) { } }
                ws = new WebSocket(url);
                ws.onopen = function () { setStatus('Connected. Waiting for scan / payment...'); };
                ws.onmessage = function (evt) {
                    // Fonepay pushes QR-verification and payment JSON messages here.
                    // For this simple test page we just show the raw message;
                    // click "Check Status" afterwards to confirm the final result.
                    setStatus('WebSocket message: ' + evt.data);
                };
                ws.onerror = function () { setStatus('WebSocket error.'); };
                ws.onclose = function () { };
            }

            window.onload = function () {
                var qrMsg = document.getElementById('<%= hfQrMessage.ClientID %>').value;
                var wsUrl = document.getElementById('<%= hfWsUrl.ClientID %>').value;
                if (qrMsg) renderQr(qrMsg);
                if (wsUrl) connectWs(wsUrl);
            };
        </script>

    </form>
</body>
</html>
