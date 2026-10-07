using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

public partial class DYNAMICQR_NiziTest : System.Web.UI.Page
{
    // Replace with your actual API key
    private const string API_KEY = "Z8uVovI2eftp65dO9JoxEstKcWggSlTAza4erAQhELmSC761rVtp5IIzaXOxWNw0ycPCICYCnJBCVPCzvdT8fbJvWIWm69fhHveZesIiDEIeI0BkdSspMPimWYNWs25D";

    // NiziPOS API URL
    private const string BASE_URL = "http://127.0.0.1:9121";

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected async void btnQR_Click(object sender, EventArgs e)
    {
        string command = "QR**Rs. "
                        + txtAmount.Text
                        + "**"
                        + txtScan.Text
                        + "**"
                        + txtPayload.Text;

        await SendCommand(command);
    }

    protected async void btnPass_Click(object sender, EventArgs e)
    {
        await SendCommand("PASS**SUCCESS!**Payment Successful");
    }

    protected async void btnFail_Click(object sender, EventArgs e)
    {
        string command = "FAIL**"
                        + txtAmount.Text
                        + "**Payment Failed";

        await SendCommand(command);
    }

    protected async void btnWait_Click(object sender, EventArgs e)
    {
        string command = "WAIT**"
                        + txtAmount.Text
                        + "**Please Wait...";

        await SendCommand(command);
    }

    protected async void btnIdle_Click(object sender, EventArgs e)
    {
        await SendCommand("IDLE");
    }

    private async Task SendCommand(string command)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", API_KEY);

                string json = "{\"command\":\"" +
                              command.Replace("\"", "\\\"") +
                              "\"}";

                StringContent content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage response =
                    await client.PostAsync(BASE_URL + "/api/command", content);

                string result = await response.Content.ReadAsStringAsync();

                lblResult.Text =
                    "<b>HTTP Status:</b> " + response.StatusCode +
                    "<br/><br/><b>Response:</b><br/>" + result;
            }
        }
        catch (Exception ex)
        {
            lblResult.Text = "<span style='color:red'>" + ex.Message + "</span>";
        }
    }
}