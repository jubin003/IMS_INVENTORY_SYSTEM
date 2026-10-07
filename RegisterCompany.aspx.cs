using Entity.Components;
using Service.Components;
using System;
using System.Net;
using System.Linq;
using PhyeGanCore;
using System.Data;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public partial class RegisterCompany : System.Web.UI.Page
{
    HelperFunction hf = new HelperFunction();
    PhyeGan PG = new PhyeGan();
    NAME_COMPANY NCEnt = new NAME_COMPANY();
    NAME_COMPANYService NCSer = new NAME_COMPANYService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (PG.CompanyName() != "")
            {
                lblPreMsgExpired.Visible = false;
                lblPreMsgRemainingDays.Visible = false;
                lblErrorMessage.Visible = true;
                btnContinue.Visible = false;
                btnNext.Visible = true;
                key_entry_form.Visible = false;
            }
        }
    }

    protected async void btnContinue_Click(object sender, EventArgs e)
    {
        if (PG.CompanyName() == "")
        {
            try
            {
                DataTable dtable = await Consume_WebAPI_GetMethod(txtPAN.Text);
                if (dtable.Rows.Count > 0)
                {
                    List<string> ids_list = new List<string>();

                    NCEnt = new NAME_COMPANY();
                    NCEnt.ORG_CODE = ED.Encrypt(dtable.Rows[0][0].ToString());
                    NCEnt.ORG_NAME = ED.Encrypt(dtable.Rows[0][1].ToString());
                    NCEnt.ORG_ADDRESS = ED.Encrypt(dtable.Rows[0][2].ToString());
                    NCEnt.PAN_NO = ED.Encrypt(dtable.Rows[0][3].ToString());
                    NCEnt.REG_NO = ED.Encrypt(dtable.Rows[0][4].ToString());
                    NCEnt.E_DATE = ED.Encrypt(dtable.Rows[0][5].ToString());
                    NCEnt.VE_DATE = ED.Encrypt(dtable.Rows[0][6].ToString());
                    NCEnt.BASE_URL = dtable.Rows[0][7].ToString();
                    NCEnt.SYS_VERSION= dtable.Rows[0][8].ToString();
                    NCEnt.CBMS_URL = dtable.Rows[0][9].ToString();
                    NCSer.Insert(NCEnt);

                    string date = PG.SoftwareExpiryDate();
                    DateTime myDate = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    TimeSpan ts1 = myDate - DateTime.Today;
                    lblErrorMessage.Text = "Company have been registered.<br> Next Expiration date is " + PG.SoftwareExpiryDate() + ". <br>Valid for " +
                        ts1.Days.ToString() + " days.";
                    lblPreMsgExpired.Visible = false;
                    lblPreMsgRemainingDays.Visible = false;
                    lblErrorMessage.Visible = true;
                    btnContinue.Visible = false;
                    renew.Visible = true;
                    key_entry_form.Visible = false;
                }
                else
                {
                    txtPAN.Text = "";
                    lblErrorMessage.Text = "Invalid License Key";

                }
            }
            catch (Exception kbhayio)
            {
                lblErrorMessage.Text = "No Internet Connection.";
            }
        }
    }
    static async Task<DataTable> Consume_WebAPI_GetMethod(string pannumber)
    {
        DataTable dt = new DataTable();
        try
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://phyegan.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var api_url = "companydetail.php?pannumber=" + pannumber + "&productcode=IMS";
            var response = await client.GetAsync(api_url).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                dynamic result = await response.Content.ReadAsStringAsync();
                var jsonParsed = JObject.Parse(result);
                dt = jsonParsed["records"].ToObject<DataTable>();//"records is the object name send from json form website
            }
        }
        catch //(Exception kbhayio)
        {
        }
        return dt;
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Login.aspx");
    }
}