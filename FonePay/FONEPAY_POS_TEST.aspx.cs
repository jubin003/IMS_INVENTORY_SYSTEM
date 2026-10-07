using System;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using PhyeGanCore;

public partial class FONEPAY_POS_TEST : Page
{
    PhyeGan PG = new PhyeGan();
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected async void btnGenerateQR_Click(object sender, EventArgs e)
    {
        decimal amount;
        if (!decimal.TryParse(txtAmount.Text.Trim(), out amount) || amount <= 0)
        {
            lblStatus.Text = "Enter a valid amount greater than 0.";
            return;
        }

        string remarks1 = string.IsNullOrEmpty(txtRemarks1.Text.Trim()) ? "Test POS" : txtRemarks1.Text.Trim();
        string remarks2 = string.IsNullOrEmpty(txtRemarks2.Text.Trim()) ? "Test" : txtRemarks2.Text.Trim();

        try
        {
            var result = await FonepayQrPosService.GenerateDynamicQrAsync(amount, remarks1, remarks2);
            if (!result.Success)
            {
                lblStatus.Text = "QR generation failed: " + result.ErrorMessage;
                hfQrMessage.Value = "";
                return;
            }

            FonepayQrPosService.RegisterTransaction(result.Prn, amount.ToString("0.00"), result.WsUrl, result.QrMessage);
            Session["FPQR_TEST_Prn"] = result.Prn;

            lblPrn.Text = result.Prn;
            lblAmountShown.Text = amount.ToString("#,##0.00");
            lblStatus.Text = "PENDING - waiting for payment...";
            hfQrMessage.Value = result.QrMessage;
            string scanText = PG.CompanyName();
            string script =
                "try { if (typeof renderFonepayQr === 'function') { renderFonepayQr('" + HttpUtility.JavaScriptStringEncode(result.QrMessage) + "'); } } " +
                "catch (e) { console.error('renderFonepayQr failed:', e); } " +
                "try { if (typeof startQRPolling === 'function') { startQRPolling({ statusElementId: '" + lblStatus.ClientID + "', amountElementId: '" + lblAmountShown.ClientID + "' }); } } " +
                "catch (e) { console.error('startQRPolling failed:', e); } " +
                "if (typeof showQrOnPos === 'function') { " +
                "showQrOnPos(" + amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + ", " +
                "'" + HttpUtility.JavaScriptStringEncode(scanText) + "', " +
                "'" + HttpUtility.JavaScriptStringEncode(result.QrMessage) + "')" +
                ".catch(function(err){ console.error('POS display error:', err); }); " +
                "} else { console.error('showQrOnPos is not defined - PosLocalClient.js did not load correctly.'); }";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showFonepayQR", script, true);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Error: " + ex.Message;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Session["FPQR_TEST_Prn"] = null;
        lblPrn.Text = "";
        lblAmountShown.Text = "";
        lblStatus.Text = "Idle";
        hfQrMessage.Value = "";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "idleFonepayPos",
            "if (typeof stopQRPolling === 'function') { stopQRPolling(); } " +
            "if (typeof idlePos === 'function') { idlePos().catch(function(err){ console.error('POS idle error:', err); }); }", true);
    }

    [WebMethod(EnableSession = true)]
    public static FonepayQrPosService.PaymentStatusResult CheckQRPaymentStatus()
    {
        var prn = System.Web.HttpContext.Current.Session["FPQR_TEST_Prn"] as string;
        return FonepayQrPosService.CheckPaymentStatus(prn);
    }
}
