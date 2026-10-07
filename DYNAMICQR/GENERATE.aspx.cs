using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public partial class DYNAMICQR_GENERATE : System.Web.UI.Page
{
   

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
            var result = await GenerateDynamicQrAsync(amount);

            if (result.Success)
            {
                imgQR.ImageUrl = "data:image/png;base64," + result.QrImageBase64;
                lblMessage.CssClass = "text-success";
                lblMessage.Text = "QR generated. Trace Id: " + result.ValidationTraceId;
            }
            else
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "QR generation failed: " + result.ErrorMessage;
            }
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

    private class QrResult
    {
        public bool Success;
        public string QrImageBase64;
        public string QrString;
        public string ValidationTraceId;
        public string ErrorMessage;
    }

    private async Task<QrResult> GenerateDynamicQrAsync(decimal amount)
    {
        // Force TLS 1.2 — required by NCHL's servers
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        // ---- Config from Web.config ----
        string baseUrl = ConfigurationManager.AppSettings["NPI_BaseUrl"];
        string username = ConfigurationManager.AppSettings["NPI_Username"];
        string password = ConfigurationManager.AppSettings["NPI_Password"];
        string pfxPath = ConfigurationManager.AppSettings["NPI_PfxPath"];
        string pfxPassword = ConfigurationManager.AppSettings["NPI_PfxPassword"];
        string acquirerId = ConfigurationManager.AppSettings["NPI_AcquirerId"];
        string merchantId = ConfigurationManager.AppSettings["NPI_MerchantId"];
        string merchantName = ConfigurationManager.AppSettings["NPI_MerchantName"];
        string merchantCategoryCode = ConfigurationManager.AppSettings["NPI_MerchantCategoryCode"];
        string merchantCity = ConfigurationManager.AppSettings["NPI_MerchantCity"];
        string merchantCountry = ConfigurationManager.AppSettings["NPI_MerchantCountry"];
        string merchantPostalCode = ConfigurationManager.AppSettings["NPI_MerchantPostalCode"];
       
        string userId = ConfigurationManager.AppSettings["NPI_UserId"];
        string tl = "0"/*ConfigurationManager.AppSettings["NPI_TerminalLabel"]*/;
        string storeLabel = ConfigurationManager.AppSettings["NPI_StoreLabel"];


        const string transactionCurrency = "524"; // NPR
        string billNumber = "0"; // must be unique per QR
        string transactionAmount = amount.ToString("0.00");

        // ---- Build & sign token ----
        // TokenString for dynamic QR:
        // acquirerId, merchantId, merchantCategoryCode, transactionCurrency, transactionAmount, billNumber, userId
        string tokenString = string.Join(",",acquirerId,merchantId,merchantCategoryCode,transactionCurrency,transactionAmount,billNumber,userId);
        //System.Diagnostics.Debug.WriteLine("TOKEN STRING: " + tokenString);
        string token = SignToken(tokenString, pfxPath, pfxPassword);
        
        var requestBody = new JObject();
        requestBody.Add("pointOfInitialization", 12); // 12 = dynamic QR
        requestBody.Add("acquirerId", "00001901");
        requestBody.Add("merchantId", "19012AB4VKK");
        requestBody.Add("merchantName", "Clinic Manager");
        requestBody.Add("merchantCategoryCode", int.Parse("3533"));
        requestBody.Add("merchantCountry", "NP");
        requestBody.Add("merchantCity", "Kathmandu");
        requestBody.Add("merchantPostalCode", "44600");
        requestBody.Add("merchantLanguage", "en");
        requestBody.Add("transactionCurrency", int.Parse("524"));
        requestBody.Add("transactionAmount", transactionAmount);
        requestBody.Add("valueOfConvenienceFeeFixed", "0.00");
        requestBody.Add("billNumber", "0");
        requestBody.Add("referenceLabel", null);
        requestBody.Add("mobileNo", null);
        requestBody.Add("storeLabel", "Store1");
        requestBody.Add("terminalLabel", "Terminal1");
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

    // Token = base64( SHA256withRSA sign( tokenString ) ) using NPI.pfx private key
    private string SignToken(string tokenString, string pfxPath, string pfxPassword)
    {
        var cert = new X509Certificate2(
            pfxPath,
            pfxPassword,
            X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

        using (RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(tokenString);
            byte[] signature = rsa.SignData(dataBytes, "SHA256");
            return Convert.ToBase64String(signature);
        }
    }
}
