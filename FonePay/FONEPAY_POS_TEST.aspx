<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FONEPAY_POS_TEST.aspx.cs" Inherits="FONEPAY_POS_TEST" Async="true" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Fonepay POS Service - Test Page</title>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />

        <div style="max-width: 480px; margin: 30px auto; font-family: Arial, sans-serif;">

            <h3>FonepayQrPosService - Test Page</h3>
            <p style="color:#666; font-size: 13px;">
                Exercises GenerateDynamicQrAsync -&gt; RegisterTransaction -&gt; the per-transaction
                WS listener -&gt; CheckPaymentStatus, the same path the real POS page uses.
            </p>

            <div>
                <label>Amount (NPR)</label><br />
                <asp:TextBox ID="txtAmount" runat="server" placeholder="e.g. 10" />
            </div>

            <div style="margin-top: 10px;">
                <label>Remarks 1</label><br />
                <asp:TextBox ID="txtRemarks1" runat="server" Text="Test POS" />
            </div>

            <div style="margin-top: 10px;">
                <label>Remarks 2</label><br />
                <asp:TextBox ID="txtRemarks2" runat="server" Text="Test" />
            </div>

            <div style="margin-top: 15px;">
                <asp:Button ID="btnGenerateQR" runat="server" Text="Generate QR" OnClick="btnGenerateQR_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="false" />
            </div>

            <div id="fonepayQrHolder" style="margin-top: 20px;"></div>

            <p><b>PRN:</b> <asp:Label ID="lblPrn" runat="server" /></p>
            <p><b>Amount:</b> <asp:Label ID="lblAmountShown" runat="server" /></p>
            <p><b>Status:</b> <asp:Label ID="lblStatus" runat="server" Text="Idle" /></p>

            <asp:HiddenField ID="hfQrMessage" runat="server" />
        </div>
        <script>
        window.POS_APP_ROOT = '<%= ResolveUrl("~/") %>';
    </script>
         <script src="<%= ResolveUrl("~/js/PosLocalClient.js") %>"></script>
        <script type="text/javascript">
            function renderFonepayQr(text) {
                var holder = document.getElementById('fonepayQrHolder');
                holder.innerHTML = '';
                if (!text) return;
                try {
                    new QRCode(holder, {
                        text: text,
                        width: 260,
                        height: 260,
                        typeNumber: 0,
                        correctLevel: QRCode.CorrectLevel.L
                    });
                } catch (err) {
                    holder.innerHTML = '<span style="color:red;">QR render failed: ' + err.message + '</span>';
                    console.error('QRCode render error:', err, 'text length:', text.length);
                }
            }
        </script>

    </form>
</body>
</html>
