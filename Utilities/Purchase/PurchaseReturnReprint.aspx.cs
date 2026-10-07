using Entity.Components;
using PhyeGanCore;
using Service.Components;
using System;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Utilities_Purchase_PurchaseReturnReprint : System.Web.UI.Page
{
    PURCHASE_INVOICE_MASTER PIMEnt = new PURCHASE_INVOICE_MASTER();
    PURCHASE_INVOICE_MASTERService PIMSer = new PURCHASE_INVOICE_MASTERService();

    PURCHASE_INVOICE_DETAIL PIDEnt = new PURCHASE_INVOICE_DETAIL();
    PURCHASE_INVOICE_DETAILService PIDSer = new PURCHASE_INVOICE_DETAILService();

    PURCHASE_RETURN_MASTER PRMEnt = new PURCHASE_RETURN_MASTER();
    PURCHASE_RETURN_MASTERService PRMSer = new PURCHASE_RETURN_MASTERService();

    PURCHASE_RETURN_DETAIL PRDEnt = new PURCHASE_RETURN_DETAIL();
    PURCHASE_RETURN_DETAILService PRDSer = new PURCHASE_RETURN_DETAILService();
    UserProfileEntity userProfileEnt = new UserProfileEntity();
    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();
    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfile = new UserProfileEntity();
    Boolean IsPageRefresh = false;
    static string path = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["postids"] = System.Guid.NewGuid().ToString();
            Session["postid"] = ViewState["postids"].ToString();
            try
            {
                txtFromDate.Text = PGD.GetTodayNepaliDate();
                txtToDate.Text = PGD.GetTodayNepaliDate();
                userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
                if (hf.checkPageAccess(path, userProfile.UserGroupID.ToString()))
                {
                    LoadFiscalYear();
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

    protected void LoadFiscalYear()
    {
        FYEnt = new FISCALYEAR();
        ddlFiscalYear.DataSource = FYSer.GetAll(FYEnt);
        ddlFiscalYear.DataTextField = "FISCAL_YEAR";
        ddlFiscalYear.DataValueField = "FISCAL_YEAR";
        ddlFiscalYear.DataBind();
        ddlFiscalYear.SelectedValue = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (chkDateWise.Checked == true)
        {
            grdPurchaseReturn.DataSource = hf.getPurchaseReturnList("", "", PGD.GetEnglishDateFromNepali(txtFromDate.Text, "dd/mm/yyyy"), PGD.GetEnglishDateFromNepali(txtToDate.Text, "dd/mm/yyyy"), userProfile.LocationID);
        }
        else
        {
            grdPurchaseReturn.DataSource = hf.getPurchaseReturnList(ddlFiscalYear.SelectedValue, txtDebitNoteNo.Text, "", "", userProfile.LocationID);
        }
        grdPurchaseReturn.DataBind();
    }

    protected void chkDateWise_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateWise.Checked == true)
        {
            NoDate1.Visible = false;
            NoDate2.Visible = false;
            WithDate1.Visible = true;
            WithDate2.Visible = true;
        }
        else
        {
            NoDate1.Visible = true;
            NoDate2.Visible = true;
            WithDate1.Visible = false;
            WithDate2.Visible = false;
        }
    }

    protected void grdPurchaseReturn_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Alter"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
            LoadToPrint(lblPK_ID.Text);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
        }
    }
    #region to print bill

    protected void LoadCompanyDetail()
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        lblCompanyName.Text = PG.CompanyName();
        lblCompanyAddress.Text = PG.BranchAddress(userProfileEnt.LocationID);
        lblEmail.Text = PG.BranchEmail(userProfileEnt.LocationID);
        lblWebsite.Text = PG.CompanyWebsite();
        if (PG.CompanyEmail() == "" && PG.CompanyWebsite() == "")
        {
            divEmail.Visible = false;
        }
        lblPanNo.Text = PG.CompanyVATPan();
        lblPhone.Text = PG.CompanyContact();

    }
    protected void cleardata()
    {
        lblDebitNoteNo.Text = "";
        lblSupplierName.Text = "";
        lblInvoiceNo.Text = "";
        lblSupplierName.Text = "";
        lblAddress.Text = "";
        lblSupplierPanNo.Text = "";
        lblDNEnglishDate.Text = "";
        lblDNNepaliDate.Text = "";

        lblAddress.Text = "";
        lblBillSubTotal.Text = "";
        lblBillDiscountPercent.Text = "";
        lblDiscount.Text = "";
        lblTaxableAmount.Text = "";
        lblVATAmount.Text = "";
        lblGTotal.Text = "";
        lblRoundoff.Text = "";
        lblBillAmount.Text = "";
        lblAmountInWord.Text = "";



    }
    protected void LoadToPrint(string pk_id)
    {
        cleardata();
        LoadCompanyDetail();
        PRMEnt = new PURCHASE_RETURN_MASTER();
        PRMEnt.PK_ID = pk_id;
        PRMEnt = (PURCHASE_RETURN_MASTER)PRMSer.GetSingle(PRMEnt);
        if (PRMEnt != null)
        {
            PIMEnt = new PURCHASE_INVOICE_MASTER();
            PIMEnt.PK_ID = PRMEnt.PURCHASE_ID;
            PIMEnt = (PURCHASE_INVOICE_MASTER)PIMSer.GetSingle(PIMEnt);
            if (PIMEnt != null)
            {
                lblInvoiceNo.Text = PIMEnt.SUPPLIER_INVOICE_NO;
                lblInvoiceNepaliDate.Text = PIMEnt.INVOICE_DAY + "/" + PIMEnt.INVOICE_MONTH + "/" + PIMEnt.INVOICE_YEAR;
                lblInvoiceEnglishDate.Text = PIMEnt.INVOICE_DATE;
                lblDakhilaNo.Text = PIMEnt.DAKHILA_NUMBER;
                lblDakhilaNepaliDate.Text = PIMEnt.DAKHILA_DAY + "/" + PIMEnt.DAKHILA_MONTH + "/" + PIMEnt.DAKHILA_YEAR;
                lblDakhilaEnglishiDate.Text = PIMEnt.DAKHILA_DATE;
                
            }
            LoadPurchaseRetGrid(PRMEnt.PK_ID);
            lblDebitNoteNo.Text = PRMEnt.DEBIT_NOTE_NUMBER;
            lblDNNepaliDate.Text = PRMEnt.NOTE_DAY + "/" + PRMEnt.NOTE_MONTH + "/" + PRMEnt.NOTE_YEAR;
            lblDNEnglishDate.Text = PRMEnt.NOTE_DATE;
            lblSupplierName.Text = PRMEnt.SUPPLIER_NAME;
            lblAddress.Text = PRMEnt.SUPPLIER_ADDRESS;
            lblSupplierPanNo.Text = PRMEnt.SUPPLIER_PAN_VAT;
            lblModeofPayment.Text = hf.getPaymentType(PRMEnt.RETURN_TYPE_ID);
            lblNoteRemarks.Text = PRMEnt.RETURN_REMARKS;

            lblInvCreatedBy.Text = hf.getEmployeeName(PRMEnt.USER_ID);
            lblBillSubTotal.Text = Convert.ToDouble(PRMEnt.SUB_TOTAL_AMOUNT).ToString("#0.00");
            lblBillDiscountPercent.Text = Convert.ToDouble(PRMEnt.DISCOUNT_PERCENT).ToString("#0.00");
            lblDiscount.Text = Convert.ToDouble(PRMEnt.DISCOUNT_AMOUNT).ToString("#0.00");
            lblTaxableAmount.Text = (Convert.ToDouble(PRMEnt.TOTAL_AMOUNT).ToString("#0.00"));
            lblVATPercent.Text = (Convert.ToDouble("13").ToString("00.00"));
            lblVATAmount.Text = Convert.ToDouble(PRMEnt.TAX_VAT_AMOUNT).ToString("#0.00");
            lblGTotal.Text = Convert.ToDouble(PRMEnt.GRAND_TOTAL_AMOUNT).ToString("#0.00");
            lblRoundoff.Text = Convert.ToDouble(PRMEnt.ROUND_OFF).ToString("#0.00");
            lblBillAmount.Text = Convert.ToDouble(PRMEnt.RETURN_AMOUNT).ToString("#0.00");
            lblAmountInWord.Text = hf.NumWordsWrapper(Convert.ToDouble(PRMEnt.RETURN_AMOUNT)) + " Only";//.ToUpper()

        }

        printdetail.Visible = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void LoadPurchaseRetGrid(string PK_id)
    {
        DataTable dt = hf.LoadPurchaseReturnInvoice(PK_id); // your existing data
        int desiredRowCount = 30;

        while (dt.Rows.Count < desiredRowCount)
        {
            dt.Rows.Add(dt.NewRow()); // add empty rows to reach 20
        }
        
        gridReturnInvoice.DataSource = dt;
        gridReturnInvoice.DataBind();
    }
    #endregion


}