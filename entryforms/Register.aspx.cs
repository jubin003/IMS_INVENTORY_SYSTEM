using Entity.Components;
using PhyeGanCore;
using Service.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class entryforms_Register : System.Web.UI.Page
{
    NAME_COMPANY NCEnt = new NAME_COMPANY();
    NAME_COMPANYService NCSer = new NAME_COMPANYService();
    UserProfileEntity userProfileEnt = new UserProfileEntity();
    PhyeGan PG = new PhyeGan();
    HelperFunction hf = new HelperFunction();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["postids"] = System.Guid.NewGuid().ToString();
            Session["postid"] = ViewState["postids"].ToString();
            try
            {
                userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                string path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
                if (hf.checkPageAccess(path, userProfileEnt.UserGroupID.ToString()))
                {
                    NCEnt = new NAME_COMPANY();
                    NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
                    if (NCEnt != null)
                    {
                        txtContactDetail.Text = NCEnt.CONTACT_DETAIL;
                        txtFax.Text = NCEnt.FAX;
                        txtEmail.Text = NCEnt.EMAIL_ID;
                        txtWebsite.Text = NCEnt.WEBSITE;
                        ddlTaxType.SelectedValue = NCEnt.TAX_TYPE;
                        if (NCEnt.TAX_PERCENT == "")
                            txtTAXPercent.Text = "13";
                        else
                            txtTAXPercent.Text = NCEnt.TAX_PERCENT;
                        ddlInvoiceType.SelectedValue = NCEnt.INVOICE_TYPE;
                        txtResetIP.Text = NCEnt.RESET_IP;
                        rbtnCBMS.SelectedValue = NCEnt.CBMS_PUSH;
                        txtCBMSApprovedDate.Text = ED.Decrypt(NCEnt.CBMS_A_DATE);
                        txtCBMSURL.Text = NCEnt.CBMS_URL;
                        txtCBMSPassword.Text = NCEnt.CBMS_PASSWORD;
                        txtCBMSUsername.Text = NCEnt.CBMS_USERNAME;                        
                        txtBaseURL.Text = NCEnt.BASE_URL;
                        try
                        {
                            ddlInvoiceHeader.SelectedValue = ED.Decrypt(NCEnt.INVOICE);
                            ddlSMSStatus.SelectedValue = ED.Decrypt(NCEnt.SMS_STATUS);
                            txtSMSSend.Text = ED.Decrypt(NCEnt.SMS_SEND_URL);
                            txtSMSGet.Text = ED.Decrypt(NCEnt.SMS_GET_URL);
                            txtSMSUsername.Text = ED.Decrypt(NCEnt.SMS_USERID);
                            txtSMSPassword.Text = ED.Decrypt(NCEnt.SMS_PASSWORD);
                        }
                        catch { }
                        txtOpeningFiscalYear.Text = NCEnt.OPENING_FY;
                    }
                }
                else
                {
                    Response.Redirect("~/forbidden.aspx");
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                Response.Redirect("~/forbidden.aspx");
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string err = "";
        try
        {
            double taxper = Convert.ToDouble(txtTAXPercent.Text);
        }
        catch
        {
            txtTAXPercent.Text = "";
            err = "Enter TAX Percent";
        }
        if(err=="")
        { 
        NCEnt = new NAME_COMPANY();
        NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                NCEnt.CONTACT_DETAIL = txtContactDetail.Text;
                NCEnt.FAX = txtFax.Text;
                NCEnt.EMAIL_ID = txtEmail.Text;
                NCEnt.WEBSITE = txtWebsite.Text;
                NCEnt.TAX_TYPE = ddlTaxType.SelectedValue;
                NCEnt.TAX_PERCENT = txtTAXPercent.Text;
                NCEnt.INVOICE_TYPE = ddlInvoiceType.SelectedValue;
                NCEnt.RESET_IP = txtResetIP.Text;

                NCEnt.CBMS_PUSH = rbtnCBMS.SelectedValue;
                NCEnt.CBMS_A_DATE= ED.Encrypt(txtCBMSApprovedDate.Text);
                NCEnt.CBMS_URL = txtCBMSURL.Text;
                NCEnt.CBMS_PASSWORD = txtCBMSPassword.Text;
                NCEnt.CBMS_USERNAME = txtCBMSUsername.Text;

                NCEnt.INVOICE = ED.Encrypt(ddlInvoiceHeader.SelectedValue);
                NCEnt.BASE_URL = txtBaseURL.Text;

                NCEnt.SMS_STATUS = ED.Encrypt(ddlSMSStatus.SelectedValue);
                NCEnt.SMS_SEND_URL = ED.Encrypt(txtSMSSend.Text);
                NCEnt.SMS_GET_URL = ED.Encrypt(txtSMSGet.Text);
                NCEnt.SMS_USERID = ED.Encrypt(txtSMSUsername.Text);
                NCEnt.SMS_PASSWORD = ED.Encrypt(txtSMSPassword.Text);

                NCEnt.OPENING_FY = txtOpeningFiscalYear.Text;
                NCSer.Update(NCEnt);

                string date = ED.Decrypt(NCEnt.E_DATE);
                DateTime myDate = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);


                TimeSpan ts = myDate - DateTime.Today;

                string days = ts.Days.ToString();
                HelperFunction.MsgBox(this, this.GetType(), "Licence will expire in " + days + "days.");
            }
            else
                HelperFunction.MsgBox(this, this.GetType(), err);
        }
    }

    protected void ddlTaxType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTaxType.SelectedValue == "VAT")
        {
            txtTAXPercent.Text = "13";
        }
        else
        {
            txtTAXPercent.Text = "0";
        }
    }
}