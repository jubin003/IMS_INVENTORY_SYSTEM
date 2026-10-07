using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

public partial class DYNAMICQR_TEST : System.Web.UI.Page
{
    private const string BASE_URL_API = "https://merchantapi.fonepay.com";
    private const string MERCHANT_CODE = "2222080002341816";
    private const string SECRET_KEY = "f3a0aa4095ba4928b14afb6aa09056b0";
    private const string USERNAME = "mail4tuladhar@gmail.com";
    private const string PASSWORD = "F0neP@y2026_01";

    private const string QR_REQUEST_PATH = "/api/merchant/merchantDetailsForThirdParty/thirdPartyDynamicQrDownload";
    private const string QR_STATUS_PATH = "/api/merchant/merchantDetailsForThirdParty/thirdPartyDynamicQrGetStatus";

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected async void btnGenerateQR_Click(object sender, EventArgs e)
    {
        string amount = txtAmount.Text.Trim();
        string remarks1 = txtRemarks1.Text.Trim();
        string remarks2 = txtRemarks2.Text.Trim();

        if (string.IsNullOrEmpty(amount))
        {
            lblStatus.Text = "Please enter an amount.";
            return;
        }

        string prn = Guid.NewGuid().ToString("N").Substring(0, 20);
        
        string message = string.Join(",", amount, prn, MERCHANT_CODE, remarks1, remarks2);
        string dataValidation = ComputeHmacSha512(SECRET_KEY, message);

        var serializer = new JavaScriptSerializer();
        var requestObj = new Dictionary<string, string>
        {
            { "amount", amount },
            { "remarks1", remarks1 },
            { "remarks2", remarks2 },
            { "prn", prn },
            { "merchantCode", MERCHANT_CODE },
            { "dataValidation", dataValidation },
            { "username", USERNAME },
            { "password", PASSWORD }
        };
        string requestJson = serializer.Serialize(requestObj);

        try
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            using (var client = new HttpClient())
            {
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(BASE_URL_API + QR_REQUEST_PATH, content);
                string body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    lblStatus.Text = "HTTP " + (int)response.StatusCode + " " + response.StatusCode + " - " + body;
                    hfQrMessage.Value = "";
                    hfWsUrl.Value = "";
                    return;
                }

                var result = serializer.Deserialize<Dictionary<string, object>>(body);

                bool success = result.ContainsKey("success") && (bool)result["success"];
                if (success)
                {
                    lblPrn.Text = prn;
                    lblStatus.Text = "QR generated. Scan to pay.";
                    hfQrMessage.Value = result["qrMessage"].ToString();
                    hfWsUrl.Value = result["thirdpartyQrWebSocketUrl"].ToString();
                    Session["FONEPAY_PRN"] = prn;
                }
                else
                {
                    string msg = result.ContainsKey("message") ? result["message"].ToString() : body;
                    lblStatus.Text = "QR generation failed: " + msg;
                    hfQrMessage.Value = "";
                    hfWsUrl.Value = "";
                }
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Error: " + GetFullErrorMessage(ex);
        }
    }

    protected async void btnCheckStatus_Click(object sender, EventArgs e)
    {
        string prn = Session["FONEPAY_PRN"] as string;
        if (string.IsNullOrEmpty(prn))
        {
            lblStatus.Text = "No active QR request. Generate a QR first.";
            return;
        }

        // Message for HMAC_SHA512 => {PRN},{MERCHANT-CODE}
        string message = string.Join(",", prn, MERCHANT_CODE);
        string dataValidation = ComputeHmacSha512(SECRET_KEY, message);

        var serializer = new JavaScriptSerializer();
        var requestObj = new Dictionary<string, string>
        {
            { "prn", prn },
            { "merchantCode", MERCHANT_CODE },
            { "dataValidation", dataValidation },
            { "username", USERNAME },
            { "password", PASSWORD }
        };
        string requestJson = serializer.Serialize(requestObj);

        try
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            using (var client = new HttpClient())
            {
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(BASE_URL_API + QR_STATUS_PATH, content);
                string body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    lblStatus.Text = "HTTP " + (int)response.StatusCode + " " + response.StatusCode + " - " + body;
                    return;
                }

                var result = serializer.Deserialize<Dictionary<string, object>>(body);
                string status = result.ContainsKey("paymentStatus") ? result["paymentStatus"].ToString() : "unknown";
                lblStatus.Text = "Payment status for PRN " + prn + ": " + status;
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Error: " + GetFullErrorMessage(ex);
        }
    }

    private static string ComputeHmacSha512(string key, string message)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        using (var hmac = new HMACSHA512(keyBytes))
        {
            byte[] hash = hmac.ComputeHash(messageBytes);
            var sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }

    private static string GetFullErrorMessage(Exception ex)
    {
        string details = ex.Message;
        var inner = ex.InnerException;
        while (inner != null)
        {
            details += " -> " + inner.Message;
            inner = inner.InnerException;
        }
        return details;
    }
}