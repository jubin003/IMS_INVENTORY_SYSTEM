<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DYNAMICQR_POS.aspx.cs" Inherits="DYNAMICQR_DYNAMICQR_POS" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="container" style="max-width: 520px; margin-top: 30px;">
        <h3>Dynamic QR Payment</h3>

        <div class="form-group">
            <label>Amount (NPR)</label>
            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" placeholder="e.g. 100.00" />
        </div>

        <div class="form-group" runat="server" visible="false">
            <label>Scan text (optional)</label>
            <asp:TextBox ID="txtScan" runat="server" CssClass="form-control" placeholder="SCAN TO PAY" />
        </div>

        <asp:Button ID="btnGenerateQR" runat="server" Text="Generate QR" CssClass="btn btn-primary"
            OnClick="btnGenerateQR_Click" />

        <div style="margin-top: 20px;">
            <asp:Image ID="imgQR" runat="server" Style="max-width: 260px; display: block;" />
        </div>

        <p id="pStatus" style="margin-top: 15px; font-weight: bold;">
            <asp:Label ID="lblMessage" runat="server" />
        </p>

        <hr />
        <h5>Manual POS controls</h5>
        <asp:Button ID="btnPass" runat="server" Text="PASS" CssClass="btn btn-success" OnClick="btnPass_Click" />
        <asp:Button ID="btnFail" runat="server" Text="FAIL" CssClass="btn btn-danger" OnClick="btnFail_Click" />
        <asp:Button ID="btnWait" runat="server" Text="WAIT" CssClass="btn btn-warning" OnClick="btnWait_Click" />
        <asp:Button ID="btnIdle" runat="server" Text="IDLE" CssClass="btn btn-secondary" OnClick="btnIdle_Click" />

        <button type="button" id="btnCheckStatus" class="btn btn-info" onclick="manualCheckStatus(); return false;">
            Check Status
        </button>
    </div>

    <script type="text/javascript">
        var pollTimer = null;
        var pollAttempts = 0;
        var MAX_POLL_ATTEMPTS = 75; // ~5 minutes at 4s intervals — adjust as needed

        function setStatusText(text, cssClass) {
            var el = document.getElementById('<%= lblMessage.ClientID %>');
            if (!el) return;
            el.innerText = text;
            el.className = cssClass || '';
        }

        function startStatusPolling() {
            stopStatusPolling();
            pollAttempts = 0;
            setStatusText('Waiting for payment...', 'text-success');
            pollTimer = setInterval(pollOnce, 4000);
        }

        function stopStatusPolling() {
            if (pollTimer) {
                clearInterval(pollTimer);
                pollTimer = null;
            }
        }

        function pollOnce() {
            pollAttempts++;
            if (pollAttempts > MAX_POLL_ATTEMPTS) {
                stopStatusPolling();
                setStatusText('Payment timed out. Please try again.', 'text-danger');
                PageMethods.ResetToIdle();
                return;
            }

            PageMethods.CheckPaymentStatus(onStatusSuccess, onStatusError);
        }

        function onStatusSuccess(result) {
            switch (result.Status) {
                case 'PENDING':
                    setStatusText('Processing... ' + (result.Message || ''), 'text-success');
                    break;
                case 'SUCCESS':
                    stopStatusPolling();
                    setStatusText('Payment Successful! ' + (result.Message || ''), 'text-success');
                    setTimeout(function () { PageMethods.ResetToIdle(); }, 5000);
                    break;
                case 'FAILED':
                    stopStatusPolling();
                    setStatusText('Payment Failed. ' + (result.Message || ''), 'text-danger');
                    setTimeout(function () { PageMethods.ResetToIdle(); }, 5000);
                    break;
                case 'NONE':
                    stopStatusPolling();
                    break;
                default:
                    // Unexpected/ERROR status — show it instead of freezing silently.
                    setStatusText('Status: ' + result.Status + ' ' + (result.Message || ''), 'text-danger');
                    break;
            }
        }

        function onStatusError(err) {
            // Surface the failure instead of freezing on old text — this is
            // what silently swallowing errors looked like before.
            setStatusText('Error checking payment status: ' + err.get_message(), 'text-danger');
        }

        function manualCheckStatus() {
            var btn = document.getElementById('btnCheckStatus');
            if (btn) btn.disabled = true;

            PageMethods.CheckPaymentStatus(
                function (result) {
                    if (btn) btn.disabled = false;
                    onStatusSuccess(result);
                },
                function (err) {
                    if (btn) btn.disabled = false;
                    onStatusError(err);
                }
            );
        }
    </script>

</asp:Content>
