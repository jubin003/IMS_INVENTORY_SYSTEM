using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


using Entity.Components;
using Service.Components;
using DataHelper.Framework;
using System.IO;
using Oracle.DataAccess.Client;
using Entity.Framework;

using PhyeGanCore;
using System.Collections.Generic;

public partial class entryforms_Company : System.Web.UI.Page
{
    NAME_COMPANY NCEnt = new NAME_COMPANY();
    NAME_COMPANYService NCSer = new NAME_COMPANYService();

    PRODUCT_SETTING PSEnt = new PRODUCT_SETTING();
    PRODUCT_SETTINGService PSSer = new PRODUCT_SETTINGService();

    PRODUCT_RATE_TYPE PRTEnt = new PRODUCT_RATE_TYPE();
    PRODUCT_RATE_TYPEService PRTSer = new PRODUCT_RATE_TYPEService();

    PRODUCT_TYPE PTEnt = new PRODUCT_TYPE();
    PRODUCT_TYPEService PTSer = new PRODUCT_TYPEService();

    OFFICE OEnt = new OFFICE();
    OFFICEService OSer = new OFFICEService();

    OFFICE_TYPE OTEnt = new OFFICE_TYPE();
    OFFICE_TYPEService OTSer = new OFFICE_TYPEService();

    UserProfileEntity userProfileEnt = new UserProfileEntity();
    PhyeGan PG = new PhyeGan();
    HelperFunction hf = new HelperFunction();
    static string path = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
                if (hf.checkPageAccess(path, userProfileEnt.UserGroupID.ToString()))
                {
                    if (userProfileEnt.UserName == "phyegan")
                    {                        
                        LoadProductSetting();
                        NCEnt = new NAME_COMPANY();
                        NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
                        if (NCEnt != null)
                        {
                            LoadRType();
                            try
                            {
                                txtCode.Text = ED.Decrypt(NCEnt.ORG_CODE);
                                txtName.Text = ED.Decrypt(NCEnt.ORG_NAME);
                                txtAddress.Text = ED.Decrypt(NCEnt.ORG_ADDRESS);
                                txtContactDetail.Text = NCEnt.CONTACT_DETAIL;
                                txtFax.Text = NCEnt.FAX;
                                txtEmail.Text = NCEnt.EMAIL_ID;
                                txtWebsite.Text = NCEnt.WEBSITE;
                                txtPanNo.Text = ED.Decrypt(NCEnt.PAN_NO);
                                txtEximCode.Text = ED.Decrypt(NCEnt.EXIM_CODE);
                                txtRegno.Text = ED.Decrypt(NCEnt.REG_NO);
                                ddlTaxType.SelectedValue = NCEnt.TAX_TYPE;
                                if (NCEnt.TAX_PERCENT == "")
                                    txtTAXPercent.Text = "13";
                                else
                                    txtTAXPercent.Text = NCEnt.TAX_PERCENT;
                                ddlInvoiceType.SelectedValue = NCEnt.INVOICE_TYPE;
                                txtResetIP.Text = NCEnt.RESET_IP;
                                txtInvoicePrefix.Text = NCEnt.INVOICE_PREFIX;
                                rbtnCBMS.SelectedValue = NCEnt.CBMS_PUSH;
                                txtCBMSURL.Text = NCEnt.CBMS_URL;
                                txtCBMSPassword.Text = NCEnt.CBMS_PASSWORD;
                                txtCBMSUsername.Text = NCEnt.CBMS_USERNAME;
                                txtCBMS_ACtivation_Date.Text = ED.Decrypt(NCEnt.CBMS_A_DATE);
                                txtSystem_installed_date.Text = ED.Decrypt(NCEnt.I_DATE);
                                txtExpiryDate.Text = ED.Decrypt(NCEnt.E_DATE);
                                rbtnStatus.SelectedValue = ED.Decrypt(NCEnt.VE_DATE);
                                ddlInvoiceHeader.SelectedValue = ED.Decrypt(NCEnt.INVOICE);
                                txtBaseURL.Text = NCEnt.BASE_URL;
                                RadioBranchStatus.SelectedValue = NCEnt.BRANCH_STATUS;
                                ddlSMSStatus.SelectedValue = ED.Decrypt(NCEnt.SMS_STATUS);
                                txtSMSSend.Text = ED.Decrypt(NCEnt.SMS_SEND_URL);
                                txtSMSGet.Text = ED.Decrypt(NCEnt.SMS_GET_URL);
                                txtSMSUsername.Text = ED.Decrypt(NCEnt.SMS_USERID);
                                txtSMSPassword.Text = ED.Decrypt(NCEnt.SMS_PASSWORD);
                                rdbtnManu.SelectedValue = NCEnt.IS_MANUFACTURER;
                                txtVersion.Text = NCEnt.SYS_VERSION;
                                txtOpeningFiscalYear.Text = NCEnt.OPENING_FY;
                                checkForBranch();
                            }
                            catch { }
                        }
                    }
                    else
                        Response.Redirect("~/forbidden.aspx");
                }
                else
                {
                    Response.Redirect("~/forbidden.aspx");
                }



            }
            catch (Exception ww)
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }
    protected void LoadProductSetting()
    {
        PSEnt = new PRODUCT_SETTING();
        PSEnt.PK_ID = "1";
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.DUAL_QUANTITY == "1")
            {
                chkDual.Checked = true;
            }
            else
            {
                chkDual.Checked = false;
            }
            if (PSEnt.SHOW_DUAL_QUANTITY == "1")
            {
                chkShowDual.Checked = true;
            }
            else
            {
                chkShowDual.Checked = false;
            }
            if (PSEnt.ITEM_WISE_DISCOUNT == "1")
            {
                chkItemDis.Checked = true;
            }
            else
            {
                chkItemDis.Checked = false;
            }
            if (PSEnt.MULTIPLE_RATE == "1")
            {
                chkRateM.Checked = true;
            }
            else
            {
                chkRateM.Checked = false;
            }
            if (PSEnt.SHOW_AVAILABILITY == "1")
            {
                chkAVailQty.Checked = true;
            }
            else
            {
                chkAVailQty.Checked = false;
            }
            if (PSEnt.PRODUCT_MANUFACTURER == "1")
            {
                chkManU.Checked = true;
            }
            else
            {
                chkManU.Checked = false;
            }
            if (PSEnt.EXPIRY_DATE == "1")
            {
                chkExpDate.Checked = true;
            }
            else
            {
                chkExpDate.Checked = false;
            }
            if (PSEnt.BATCH_NUMBER == "1")
            {
                chkBatNum.Checked = true;
            }
            else
            {
                chkBatNum.Checked = false;
            }
            if (PSEnt.PRODUCT_COLOUR == "1")
            {
                chkProCol.Checked = true;
            }
            else
            {
                chkProCol.Checked = false;
            }
            if (PSEnt.DUAL_QUANTITY == "1")
            {
                chkDual.Checked = true;
            }
            else
            {
                chkDual.Checked = false;
            }
            if (PSEnt.PRODUCT_SIZE == "1")
            {
                chkProSize.Checked = true;
            }
            else
            {
                chkProSize.Checked = false;
            }
            if (PSEnt.SHOW_PO_NUMBER == "1")
            {
                chkShowPO.Checked = true;
            }
            else
            {
                chkShowPO.Checked = false;
            }
            if (PSEnt.ROUND_OFF == "1")
            {
                chkRoundOff.Checked = true;
            }
            else
            {
                chkRoundOff.Checked = false;
            }
            if (PSEnt.ONLY_STOCK_SALES == "1")
            {
                chkOnlyStock.Checked = true;
            }
            else
            {
                chkOnlyStock.Checked = false;
            }
            txtVersionControll.Text = PSEnt.VERSION_CONTROL;
            
        }
        PRTEnt = new PRODUCT_RATE_TYPE();
        PRTEnt.PK_ID = "1";
        PRTEnt = (PRODUCT_RATE_TYPE)PRTSer.GetSingle(PRTEnt);
        if (PRTEnt != null && PRTEnt.RATE_EDITABLE=="1")
        {
            chkEditable.Checked = true;

        }
        else
        {
            chkEditable.Checked = false;
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        NCEnt = new NAME_COMPANY();
        NCSer.Delete(NCEnt);

        NCEnt = new NAME_COMPANY();
        NCEnt.ORG_CODE = ED.Encrypt(txtCode.Text);
        NCEnt.ORG_NAME = ED.Encrypt(txtName.Text);
        NCEnt.ORG_ADDRESS = ED.Encrypt(txtAddress.Text);
        NCEnt.CONTACT_DETAIL = txtContactDetail.Text;
        NCEnt.FAX = txtFax.Text;
        NCEnt.EMAIL_ID = txtEmail.Text;
        NCEnt.WEBSITE = txtWebsite.Text;
        NCEnt.PAN_NO = ED.Encrypt(txtPanNo.Text);
        NCEnt.REG_NO = ED.Encrypt(txtRegno.Text);
        NCEnt.TAX_TYPE = ddlTaxType.SelectedValue;
        NCEnt.TAX_PERCENT = txtTAXPercent.Text;
        NCEnt.EXIM_CODE = ED.Encrypt(txtEximCode.Text);
        NCEnt.INVOICE_TYPE = ddlInvoiceType.SelectedValue;
        NCEnt.RESET_IP = txtResetIP.Text;
        NCEnt.IS_MANUFACTURER = rdbtnManu.SelectedValue;


        NCEnt.CBMS_PUSH = rbtnCBMS.SelectedValue;
        NCEnt.CBMS_URL = txtCBMSURL.Text;
        NCEnt.CBMS_PASSWORD = txtCBMSPassword.Text;
        NCEnt.CBMS_USERNAME = txtCBMSUsername.Text;
        NCEnt.CBMS_A_DATE = ED.Encrypt(txtCBMS_ACtivation_Date.Text);
        NCEnt.I_DATE = ED.Encrypt(txtSystem_installed_date.Text);

        NCEnt.E_DATE = ED.Encrypt(txtExpiryDate.Text);
        NCEnt.VE_DATE = ED.Encrypt(rbtnStatus.SelectedValue);
        NCEnt.INVOICE = ED.Encrypt(ddlInvoiceHeader.SelectedValue);
        NCEnt.INVOICE_PREFIX = txtInvoicePrefix.Text;
        NCEnt.BASE_URL = txtBaseURL.Text;

        NCEnt.SMS_STATUS = ED.Encrypt(ddlSMSStatus.SelectedValue);
        NCEnt.SMS_SEND_URL = ED.Encrypt(txtSMSSend.Text);
        NCEnt.SMS_GET_URL = ED.Encrypt(txtSMSGet.Text);
        NCEnt.SMS_USERID = ED.Encrypt(txtSMSUsername.Text);
        NCEnt.SMS_PASSWORD = ED.Encrypt(txtSMSPassword.Text);

        NCEnt.SYS_VERSION = txtVersion.Text;
        NCEnt.OPENING_FY = txtOpeningFiscalYear.Text;
        NCEnt.BRANCH_STATUS = RadioBranchStatus.SelectedValue;
        NCSer.Insert(NCEnt);


        OEnt = new OFFICE();
        OEnt.OFFICECODE = "HO";
        OEnt = (OFFICE)OSer.GetSingle(OEnt);
        if (OEnt != null)
        {
            OEnt.OFFICENAME = ED.Encrypt(txtName.Text);
            OEnt.OPENING_FISCAL_YEAR = txtOpeningFiscalYear.Text;
            OEnt.STREET = txtAddress.Text;
            OEnt.INVOICE_PREFIX = txtInvoicePrefix.Text;
            OEnt.PHONE_NO = txtContactDetail.Text;
            OEnt.EMAIL = txtEmail.Text;
            OEnt.COMPANY_CODE = ED.Encrypt(txtCode.Text);
            OSer.Update(OEnt);
        }
        
       
        PSEnt = new PRODUCT_SETTING();
        PSEnt.PK_ID = "1";
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (chkDual.Checked == true)
            {
                PSEnt.DUAL_QUANTITY = "1";
            }
            else
            {
                PSEnt.DUAL_QUANTITY = "0";
            }
            if (chkShowDual.Checked == true)
            {
                PSEnt.SHOW_DUAL_QUANTITY = "1";
            }
            else
            {
                PSEnt.SHOW_DUAL_QUANTITY = "0";
            }
            if (chkItemDis.Checked == true)
            {
                PSEnt.ITEM_WISE_DISCOUNT = "1";
            }
            else
            {
                PSEnt.ITEM_WISE_DISCOUNT = "0";
            }
            if (chkRateM.Checked == true)
            {
                PSEnt.MULTIPLE_RATE = "1";
            }
            else
            {
                PSEnt.MULTIPLE_RATE = "0";
                

            }
            if (chkAVailQty.Checked == true)
            {
                PSEnt.SHOW_AVAILABILITY = "1";
            }
            else
            {
                PSEnt.SHOW_AVAILABILITY = "0";
            }
            if (chkManU.Checked == true)
            {
                PSEnt.PRODUCT_MANUFACTURER = "1";
            }
            else
            {
                PSEnt.PRODUCT_MANUFACTURER = "0";
            }
            if (chkExpDate.Checked == true)
            {
                PSEnt.EXPIRY_DATE = "1";
            }
            else
            {
                PSEnt.EXPIRY_DATE = "0";
            }
            if (chkBatNum.Checked == true)
            {
                PSEnt.BATCH_NUMBER = "1";
            }
            else
            {
                PSEnt.BATCH_NUMBER = "0";
            }
            if (chkProCol.Checked == true)
            {
                PSEnt.PRODUCT_COLOUR = "1";
            }
            else
            {
                PSEnt.PRODUCT_COLOUR = "0";
            }
            if (chkProSize.Checked == true)
            {
                PSEnt.PRODUCT_SIZE = "1";
            }
            else
            {
                PSEnt.PRODUCT_SIZE = "0";
            }
            if (chkShowPO.Checked == true)
            {
                PSEnt.SHOW_PO_NUMBER = "1";
            }
            else
            {
                PSEnt.SHOW_PO_NUMBER = "0";
            }
            if (chkRoundOff.Checked ==true )
            {
                PSEnt.ROUND_OFF = "1";
            }
            else
            {
                PSEnt.ROUND_OFF = "0";
            }
            if (chkOnlyStock.Checked == true)
            {
                PSEnt.ONLY_STOCK_SALES = "1";
            }
            else
            {
                PSEnt.ONLY_STOCK_SALES = "0";
            }
            PSSer.Update(PSEnt);






           

            string date = ED.Decrypt(ED.Encrypt(txtExpiryDate.Text));
            DateTime myDate = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            //if (myDate > DateTime.Today)
            //{
            //    HelperFunction.MsgBox(this, this.GetType(), "greater.");
            //}
            //else
            //{
            //    HelperFunction.MsgBox(this, this.GetType(), "smaller.");
            //}

            TimeSpan ts = myDate - DateTime.Today;

            string days = ts.Days.ToString();


            if (RadioBranchStatus.SelectedValue == "1")
            {
                foreach (GridViewRow gr in grdBranchDetail.Rows)
                {
                    Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
                    TextBox txtBrCODE = gr.FindControl("txtBrCODE") as TextBox;
                    TextBox txtBrName = gr.FindControl("txtBrName") as TextBox;
                    TextBox txtBrAddress = gr.FindControl("txtBrAddress") as TextBox;
                    DropDownList ddlBrOfficeType = gr.FindControl("ddlBrOfficeType") as DropDownList;
                    TextBox txtBrInvPrefix = gr.FindControl("txtBrInvPrefix") as TextBox;
                    TextBox txtBrPhone = gr.FindControl("txtBrPhone") as TextBox;
                    TextBox txtBrEmail = gr.FindControl("txtBrEmail") as TextBox;
                    TextBox txtBrFiscalYear = gr.FindControl("txtBrFiscalYear") as TextBox;
                    TextBox txtBrPURPrefix = gr.FindControl("txtBrPURPrefix") as TextBox;
                    TextBox txtBrPRPrefix = gr.FindControl("txtBrPRPrefix") as TextBox;
                    TextBox txtBrSRPrefix = gr.FindControl("txtBrSRPrefix") as TextBox;

                    OEnt = new OFFICE();
                    OEnt.PK_ID = lblPK_ID.Text;
                    OEnt.OFFICECODE = txtBrCODE.Text;
                    OEnt = (OFFICE)OSer.GetSingle(OEnt);
                    if (OEnt != null)
                    {
                        OEnt.OFFICENAME = ED.Encrypt(txtBrName.Text);
                        OEnt.OPENING_FISCAL_YEAR = txtBrFiscalYear.Text;
                        OEnt.STREET = txtBrAddress.Text;
                        OEnt.OFFICETYPEID = "2";
                        OEnt.INVOICE_PREFIX = txtBrInvPrefix.Text;
                        OEnt.PURCHASE_PREFIX = txtBrPURPrefix.Text;
                        OEnt.PR_PREFIX = txtBrPRPrefix.Text;
                        OEnt.SR_PREFIX = txtBrSRPrefix.Text;
                        OEnt.EMAIL = txtBrEmail.Text;
                        OEnt.COMPANY_CODE = ED.Encrypt(txtCode.Text);
                        OSer.Update(OEnt);
                    }
                    else
                    {
                        OEnt = new OFFICE();
                        OEnt.OFFICECODE = txtBrCODE.Text;
                        OEnt.OFFICENAME = ED.Encrypt(txtBrName.Text);
                        OEnt.OPENING_FISCAL_YEAR = txtBrFiscalYear.Text;
                        OEnt.STREET = txtBrAddress.Text;
                        OEnt.OFFICETYPEID = "2";
                        OEnt.INVOICE_PREFIX = txtBrInvPrefix.Text;
                        OEnt.PURCHASE_PREFIX = txtBrPURPrefix.Text;
                        OEnt.PR_PREFIX = txtBrPRPrefix.Text;
                        OEnt.SR_PREFIX = txtBrSRPrefix.Text;
                        OEnt.PHONE_NO = txtBrPhone.Text;
                        OEnt.EMAIL = txtBrEmail.Text;
                        OEnt.COMPANY_CODE = ED.Encrypt(txtCode.Text);
                        OSer.Insert(OEnt);
                    }
                }
            }

            HelperFunction.MsgBox(this, this.GetType(), "Licence will expire in " + days + "days.");


            setRateAvailability();
            SetProductType();

        }
    }

    protected void chkDual_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDual.Checked != true)
        {
            chkShowDual.Checked = false;
            chkShowDual.Enabled = false;
        }
        else
        {
            chkShowDual.Checked = true;
            chkShowDual.Enabled = true;
        }
    }
    protected void LoadRType()
    {
        PRTEnt = new PRODUCT_RATE_TYPE();
        gridCusType.DataSource = PRTSer.GetAll(PRTEnt);
        gridCusType.DataBind();
    }
    protected void setRateAvailability()
    {
        if (chkRateM.Checked == true)
        {
            // Set the first row's status to 0 (off)
            PRTEnt = new PRODUCT_RATE_TYPE();
            PRTEnt.PK_ID = "1";
            PRTEnt = (PRODUCT_RATE_TYPE)PRTSer.GetSingle(PRTEnt); // Fetch a single record with PK_ID = "1"
            if (PRTEnt != null)
            {
                PRTEnt.STATUS = "0"; // Set status to "off"
                PRTSer.Update(PRTEnt); // Save the change
            }

            // Iterate through the rest of the rows, set their status to 1 (on)
            for (int i = 2; i <= gridCusType.Rows.Count; i++)  // Start from the second row (i = 1)
            {


                PRTEnt = new PRODUCT_RATE_TYPE();
                PRTEnt.PK_ID = i.ToString();  // Get PK_ID from the grid
                PRTEnt = (PRODUCT_RATE_TYPE)PRTSer.GetSingle(PRTEnt); // Fetch a single record with PK_ID
                if (PRTEnt != null)
                {
                    PRTEnt.STATUS = "1"; // Set status to "on"
                    PRTSer.Update(PRTEnt); // Save the change
                }
            }
        }
        else
        {
            // Set the first row's status to 1 (on)
            PRTEnt = new PRODUCT_RATE_TYPE();
            PRTEnt.PK_ID = "1";
            PRTEnt = (PRODUCT_RATE_TYPE)PRTSer.GetSingle(PRTEnt); // Fetch a single record with PK_ID = "1"
            if (PRTEnt != null)
            {
                PRTEnt.STATUS = "1";
                if (chkEditable.Checked == true)
                {
                    PRTEnt.RATE_EDITABLE = "1";
                }
                else
                {
                    PRTEnt.RATE_EDITABLE = "0";
                }
                PRTSer.Update(PRTEnt); // Save the change
            }

            // Iterate through the rest of the rows, set their status to 0 (off)
            for (int i = 2; i <= gridCusType.Rows.Count; i++)  // Start from the second row (i = 1)
            {
                PRTEnt = new PRODUCT_RATE_TYPE();
                PRTEnt.PK_ID = i.ToString();  // Get PK_ID from the grid
                PRTEnt = (PRODUCT_RATE_TYPE)PRTSer.GetSingle(PRTEnt); // Fetch a single record with PK_ID
                if (PRTEnt != null)
                {
                    PRTEnt.STATUS = "0"; // Set status to "on"
                    if (chkEditable.Checked == true)
                    {
                        PRTEnt.RATE_EDITABLE = "0";
                    }
                    else
                    {
                        PRTEnt.RATE_EDITABLE = "1";
                    }
                    PRTSer.Update(PRTEnt); // Save the change
                }
            }
        }
        LoadRType();
    }

    protected void chkRateM_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRateM.Checked != true)
        {
            tdEditable.Visible = true;
            chkEditable.Checked = true;

        }
        else
        {
            tdEditable.Visible = false;
            chkEditable.Checked = false;
        }
    }

    protected void SetProductType()
    {
        if (rdbtnManu.SelectedValue!="1")
        {
            // Set the first row's status to 0 (off)
            PTEnt = new PRODUCT_TYPE();
            PTEnt.PK_ID = "1";
            PTEnt = (PRODUCT_TYPE)PTSer.GetSingle(PTEnt); // Fetch a single record with PK_ID = "1"
            if (PTEnt != null)
            {
                PTEnt.STATUS = "0";
                PTEnt.SHOW_IN_SALES = "0";
                PTEnt.SHOW_IN_PURCHASE = "0";// Set status to "off"
                PTSer.Update(PTEnt); // Save the change
            }

            // Iterate through the rest of the rows, set their status to 1 (on)
            for (int i = 2; i <= gridProType.Rows.Count; i++)  // Start from the second row (i = 1)
            {


                PTEnt = new PRODUCT_TYPE();
                PTEnt.PK_ID = i.ToString();  // Get PK_ID from the grid
                PTEnt = (PRODUCT_TYPE)PTSer.GetSingle(PTEnt); // Fetch a single record with PK_ID
                if (PTEnt != null)
                {
                    PTEnt.STATUS = "1"; // Set status to "on"
                    PTSer.Update(PTEnt); // Save the change
                }
            }
        }
        else
        {
            // Set the first row's status to 1 (on)
            PTEnt = new PRODUCT_TYPE();
            PTEnt.PK_ID = "1";
            PTEnt = (PRODUCT_TYPE)PTSer.GetSingle(PTEnt); // Fetch a single record with PK_ID = "1"
            if (PTEnt != null)
            {
                PTEnt.STATUS = "1";
                PTEnt.SHOW_IN_SALES = "1";
                PTEnt.SHOW_IN_PURCHASE = "1";
                PTSer.Update(PTEnt); // Save the change
            }

            // Iterate through the rest of the rows, set their status to 0 (off)
            for (int i = 2; i <= gridProType.Rows.Count; i++)  // Start from the second row (i = 1)
            {
                PTEnt = new PRODUCT_TYPE();
                PTEnt.PK_ID = i.ToString();  // Get PK_ID from the grid
                PTEnt = (PRODUCT_TYPE)PTSer.GetSingle(PTEnt); // Fetch a single record with PK_ID
                if (PTEnt != null)
                {
                    PTEnt.STATUS = "0"; // Set status to "on"
                   
                    PTSer.Update(PTEnt); // Save the change
                }
            }
        }
        LoadPType();
    }
    protected void LoadPType()
    {
        PTEnt = new PRODUCT_TYPE();
        gridProType.DataSource = PTSer.GetAll(PTEnt);
        gridProType.DataBind();
    }

    protected void ddlTaxType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTaxType.Text == "None")
        {

            txtTAXPercent.Text = "0";
            txtTAXPercent.Enabled = false;
        }
        else
        {
            txtTAXPercent.Enabled = true;

        }
    }

    private DataTable CreateGridFirst()
    {
        DataTable dummyTable = new DataTable();

        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("OFFICENAME");
        dummyTable.Columns.Add("OFFICECODE");
        dummyTable.Columns.Add("OPENING_FISCAL_YEAR");
        dummyTable.Columns.Add("STREET");
        dummyTable.Columns.Add("INVOICE_PREFIX");
        dummyTable.Columns.Add("PURCHASE_PREFIX");
        dummyTable.Columns.Add("PR_PREFIX");
        dummyTable.Columns.Add("SR_PREFIX");
        dummyTable.Columns.Add("PHONE_NO");
        dummyTable.Columns.Add("EMAIL");

        DataRow dummyRw = dummyTable.NewRow();

        dummyTable.Rows.Add(dummyRw);

        DataView dv = new DataView(dummyTable);
        grdBranchDetail.DataSource = dv;
        grdBranchDetail.DataBind();
        return dummyTable;
    }


    protected DataTable CreateGrid(int RowIndex, bool addRemoveFlag)
    {
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("OFFICENAME");
        dummyTable.Columns.Add("OFFICECODE");
        dummyTable.Columns.Add("OPENING_FISCAL_YEAR");
        dummyTable.Columns.Add("STREET");
        dummyTable.Columns.Add("INVOICE_PREFIX");
        dummyTable.Columns.Add("PURCHASE_PREFIX");
        dummyTable.Columns.Add("PR_PREFIX");
        dummyTable.Columns.Add("SR_PREFIX");
        dummyTable.Columns.Add("PHONE_NO");
        dummyTable.Columns.Add("EMAIL");

        foreach (GridViewRow r in grdBranchDetail.Rows)
        {
            Label lblPK_ID = grdBranchDetail.Rows[r.RowIndex].FindControl("lblPK_ID") as Label;
            TextBox txtBrName = grdBranchDetail.Rows[r.RowIndex].FindControl("txtBrName") as TextBox;
            TextBox txtBrCODE = grdBranchDetail.Rows[r.RowIndex].FindControl("txtBrCODE") as TextBox;
            TextBox txtBrAddress = grdBranchDetail.Rows[r.RowIndex].FindControl("txtBrAddress") as TextBox;
            DropDownList ddlBrOfficeType = grdBranchDetail.Rows[r.RowIndex].FindControl("ddlBrOfficeType") as DropDownList;
            TextBox txtBrInvPrefix = grdBranchDetail.Rows[r.RowIndex].FindControl("txtBrInvPrefix") as TextBox;
            TextBox txtBrPhone = grdBranchDetail.Rows[r.RowIndex].FindControl("txtBrPhone") as TextBox;
            TextBox txtBrEmail = grdBranchDetail.Rows[r.RowIndex].FindControl("txtBrEmail") as TextBox;
            TextBox txtBrFiscalYear = grdBranchDetail.Rows[r.RowIndex].FindControl("txtBrFiscalYear") as TextBox;
            TextBox txtBrPURPrefix = grdBranchDetail.Rows[r.RowIndex].FindControl("txtBrPURPrefix") as TextBox;
            TextBox txtBrPRPrefix = grdBranchDetail.Rows[r.RowIndex].FindControl("txtBrPRPrefix") as TextBox;
            TextBox txtBrSRPrefix = grdBranchDetail.Rows[r.RowIndex].FindControl("txtBrSRPrefix") as TextBox;


            DataRow dummyRow = dummyTable.NewRow();
            dummyRow["PK_ID"] = lblPK_ID.Text;
            dummyRow["OFFICENAME"] = txtBrName.Text;
            dummyRow["OFFICECODE"] = txtBrCODE.Text;
            dummyRow["OPENING_FISCAL_YEAR"] = txtBrFiscalYear.Text;
            dummyRow["STREET"] = txtBrAddress.Text;
            dummyRow["INVOICE_PREFIX"] = txtBrInvPrefix.Text;
            dummyRow["PURCHASE_PREFIX"] = txtBrPURPrefix.Text;
            dummyRow["PR_PREFIX"] = txtBrPRPrefix.Text;
            dummyRow["SR_PREFIX"] = txtBrSRPrefix.Text;
            dummyRow["PHONE_NO"] = txtBrPhone.Text;
            dummyRow["EMAIL"] = txtBrEmail.Text;

            dummyTable.Rows.Add(dummyRow);

            if (r.RowIndex == RowIndex && addRemoveFlag == true) //addRemoveFlag = true (row add garne) 
            {
                DataRow dummyRw = dummyTable.NewRow();
                //dummyRow["Lobe"] = "Select";
                //dummyRow["Location"] = "Select";
                //dummyRow["Size"] = "";
                //dummyRow["Composition"] = "Select";
                //dummyRow["Echogenicity"] = "Select";
                //dummyRow["TallerThanWide"] = "Select";
                //dummyRow["Margins"] = "Select";
                //dummyRow["EchogenicFoci"] = "Select";
                //dummyRow["TotalPoints"] = "";
                //dummyRow["TI_RADS"] = "";
                dummyTable.Rows.Add(dummyRw);
            }
            else if (r.RowIndex == RowIndex && addRemoveFlag == false)//addRemoveFlag = false (row delete garne) 
                dummyTable.Rows.Remove(dummyRow);
        }

        DataView dv = new DataView(dummyTable);
        grdBranchDetail.DataSource = dv;
        grdBranchDetail.DataBind();
        return dummyTable;
    }

    protected void RadioBranchStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkForBranch();
    }
    protected void checkForBranch()
    {
        if (RadioBranchStatus.SelectedValue == "1")
        {
            CreateGridFirst();
            grdBranchDetail.Visible = true;
            loadGrid();
        }
        else
        {
            grdBranchDetail.Visible = false;
        }
    }

    protected void grdBranchDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Add"))
        {
            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            CreateGrid(gr.RowIndex, true);

        }

        if (e.CommandName.Equals("Remove"))
        {
            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            CreateGrid(gr.RowIndex, false);
        }
    }

    protected void grdBranchDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            DropDownList ddlBrOfficeType = e.Row.FindControl("ddlBrOfficeType") as DropDownList;
            TextBox txtBrName = e.Row.FindControl("txtBrName") as TextBox;
            Label lblPK_ID = e.Row.FindControl("lblPK_ID") as Label;

            OEnt = new OFFICE();
            OEnt.PK_ID = lblPK_ID.Text;
            OEnt = (OFFICE)OSer.GetSingle(OEnt);
            if (OEnt != null)
            {
                txtBrName.Text = ED.Decrypt(OEnt.OFFICENAME);
                //txtBrName.Text = OEnt.OFFICENAME;
            }

        }
    }

    protected void loadGrid()
    {
        OEnt = new OFFICE();
        OEnt.OFFICETYPEID = "2";
        grdBranchDetail.DataSource = OSer.GetAll(OEnt);
        grdBranchDetail.DataBind();
    }
}