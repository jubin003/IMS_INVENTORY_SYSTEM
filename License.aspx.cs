using Entity.Components;
using Service.Components;
using System;
using System.Net;
using PhyeGanCore;

public partial class License : System.Web.UI.Page
{
    HelperFunction hf = new HelperFunction();
    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string date = PG.SoftwareExpiryDate();
            DateTime myDate = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            TimeSpan ts1 = myDate - DateTime.Today;
            if (myDate > DateTime.Today)
            {
                lblPreMsgRemainingDays.Text = "Software will expire on " + PGD.GetNepaliDateFromEnglish(PG.SoftwareExpiryDate(),"dd/mm/yyyy") + " [" +
                PG.SoftwareExpiryDate() + "]<br>" + ts1.Days.ToString() + " days remaining.";
            }
            else
            {
                lblPreMsgExpired.Text = "Software have been expire on " + PG.SoftwareExpiryDate();
            }
        }
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        if (PG.CompanyName() != "")
        {
            string licence_key = txtkey1.Text + "-" + txtkey2.Text + "-" + txtkey3.Text + "-" + txtkey4.Text;
            string msg = "";
            if (licence_key.Length == 19)
            {
                try
                {
                    int key1 = Convert.ToInt32(txtkey1.Text);
                    int key2 = Convert.ToInt32(txtkey2.Text);
                    int key3 = Convert.ToInt32(txtkey3.Text);
                    int key4 = Convert.ToInt32(txtkey4.Text);
                }
                catch { msg = "Invalid License Key"; }

            }
            else
            {
                txtkey1.Text = "";
                txtkey2.Text = "";
                txtkey3.Text = "";
                txtkey4.Text = "";
                msg = "Invalid License Key";
            }
            if (msg == "")
            {
                try
                {
                    if (PG.RenewedExpiryDate(licence_key) != "")
                    {
                        DateTime myDate = DateTime.ParseExact(PG.RenewedExpiryDate(licence_key), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (myDate > DateTime.Today)
                        {
                            PG.UpdateExpireDate(licence_key);
                            TimeSpan ts1 = myDate - DateTime.Today;
                            lblRenewMsg.Text = "Congratulation !!! <br>Software License have been renewed.<br> Next Expiration date is " + PG.RenewedExpiryDate(licence_key) + ". <br>Valid for " +
                                ts1.Days.ToString() + " days.";
                            lblPreMsgExpired.Visible = false;
                            lblPreMsgRemainingDays.Visible = false;
                        }
                        else
                        {
                            lblRenewMsg.Text = "Sorry !!! <br>Your License Key have been expired.<br> Please Contact Service provide for valid License Key.";
                        }
                        key_entry_form.Visible = false;
                        renew.Visible = true;
                    }
                    else
                    {
                        txtkey1.Text = "";
                        txtkey2.Text = "";
                        txtkey3.Text = "";
                        txtkey4.Text = "";
                        lblErrorMessage.Text = "Invalid License Key";
                    }
                }
                catch
                {
                    lblErrorMessage.Text = "No Internet Connection.";
                }
            }
            else
            {
                lblErrorMessage.Text = msg;
                lblErrorMessage.Visible = true;
                key_entry_form.Visible = true;
                renew.Visible = false;
            }
        }
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Login.aspx");
    }
}