using Entity.Components;
using PhyeGanCore;
using Service.Components;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
public partial class Utilities_Sales_SalesReturnReprint : System.Web.UI.Page
{
    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();
    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();


    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();

    PRODUCT_SETTING PSEnt = new PRODUCT_SETTING();
    PRODUCT_SETTINGService PSSer = new PRODUCT_SETTINGService();

    SALES_INVOICE_MASTER SIMEnt = new SALES_INVOICE_MASTER();
    SALES_INVOICE_MASTERService SIMSer = new SALES_INVOICE_MASTERService();

    SALES_INVOICE_DETAIL SIDEnt = new SALES_INVOICE_DETAIL();
    SALES_INVOICE_DETAILService SIDSer = new SALES_INVOICE_DETAILService();

    SALES_RETURN_MASTER SRMEnt = new SALES_RETURN_MASTER();
    SALES_RETURN_MASTERService SRMSer = new SALES_RETURN_MASTERService();

    SALES_RETURN_DETAIL SRDEnt = new SALES_RETURN_DETAIL();
    SALES_RETURN_DETAILService SRDSer = new SALES_RETURN_DETAILService();

    PAYMENT_TYPE PTEnt = new PAYMENT_TYPE();
    PAYMENT_TYPEService PTSEr = new PAYMENT_TYPEService();

    AGENT AEnt = new AGENT();
    AGENTService ASer = new AGENTService();

    AREA AREAEnt = new AREA();
    AREAService AREASer = new AREAService();

    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfile = new UserProfileEntity();
    Boolean IsPageRefresh = false;
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
                string path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
                if (hf.checkPageAccess(path, userProfile.UserGroupID.ToString()))
                {
                    LoadFiscalYear();
                }
                else
                {
                    Response.Redirect("~/forbidden.aspx");
                }
                if (PG.CompanyTAXType() != "VAT")//whether or not to show vat on bill
                {
                    trInvVats.Visible = false;


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
            grdSalesReturn.DataSource = hf.getSalesReturnList("", "", PGD.GetEnglishDateFromNepali(txtFromDate.Text, "dd/mm/yyyy"), PGD.GetEnglishDateFromNepali(txtToDate.Text, "dd/mm/yyyy"), userProfile.LocationID);
        }
        else
        {
            grdSalesReturn.DataSource = hf.getSalesReturnList(ddlFiscalYear.SelectedValue, txtDebitNoteNo.Text, "", "", userProfile.LocationID);
        }
        grdSalesReturn.DataBind();
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
    protected void grdSalesReturn_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Alter"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
            PrintCreditNote(lblPK_ID.Text);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
        }
    }

    #region to print bill

    protected void LoadCompanyDetail()
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        lblCompanyName.Text = PG.CompanyName();
        lblCompanyAddress.Text = PG.BranchAddress(userProfile.LocationID);
        lblEmail.Text = PG.BranchEmail(userProfile.LocationID);
        lblWebsite.Text = PG.CompanyWebsite();
        if (PG.CompanyEmail() == "" && PG.CompanyWebsite() == "")
        {
            divEmail.Visible = false;
        }
        lblPanNo.Text = PG.CompanyVATPan();
        lblPhone.Text = PG.BranchContact(userProfile.LocationID);
    }
    protected void clearCN()
    {
        lblCreditNoteNo.Text = "";
        lblCNCustomerName.Text = "";
        lblCNInvoiceNumber.Text = "";
        lblCNCustomerAddress.Text = "";
        lblCNModeofPayment.Text = "";
        lblCNCustomerPanNo.Text = "";
        lblCNNepaliDate.Text = "";
        lblCNEnglishDate.Text = "";
        lblInvoiceNepaliDate.Text = "";
        lblInvoiceEnglishDate.Text = "";


        lblCNSubTotal.Text = "";
        lblCNDiscountPercent.Text = "";
        lblCNDiscountAmount.Text = "";
        lblCNTaxableAmount.Text = "";
        lblCNVATPercent.Text = "";
        lblCNVATAmount.Text = "";
        lblCNGrandTotal.Text = "";
        lblCNRoundoff.Text = "";
        lblCNInvoiceAmount.Text = "";
        lblCNAmounInWords.Text = "";
        lblCNRemarks.Text = "";
    }
    protected void PrintCreditNote(string PK_ID)
    {
        LoadCompanyDetail();
        clearCN();

        SRMEnt = new SALES_RETURN_MASTER();
        SRMEnt.PK_ID = PK_ID;
        SRMEnt = (SALES_RETURN_MASTER)SRMSer.GetSingle(SRMEnt);
        if (SRMEnt != null)
        {
            SIMEnt = new SALES_INVOICE_MASTER();
            SIMEnt.PK_ID = SRMEnt.SALES_INVOICE_ID;
            SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
            if (SIMEnt != null)
            {
                lblCNInvoiceNumber.Text = SIMEnt.INVOICE_NUMBER;
                lblInvoiceNepaliDate.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
                lblInvoiceEnglishDate.Text = SIMEnt.INVOICE_DATE;

            }
            LoadSalesRetGrid(SRMEnt.PK_ID);
            lblCreditNoteNo.Text = SRMEnt.CREDIT_NOTE_NUMBER;
            lblCNNepaliDate.Text = SRMEnt.NOTE_DAY + "/" + SRMEnt.NOTE_MONTH + "/" + SRMEnt.NOTE_YEAR;
            lblCNEnglishDate.Text = SRMEnt.NOTE_DATE;
            lblCNCustomerName.Text = SRMEnt.CUSTOMER_NAME;
            lblCNCustomerAddress.Text = SRMEnt.CUSTOMER_ADDRESS;
            lblCNCustomerPanNo.Text = SRMEnt.COSTOMER_PAN_VAT;
            lblCNModeofPayment.Text = hf.getPaymentType(SRMEnt.SALES_RETURN_TYPE);
            lblCNSubTotal.Text = Convert.ToDouble(SRMEnt.SUB_TOTAL).ToString("0.00");
            lblCNDiscountPercent.Text = Convert.ToDouble(SRMEnt.DISCOUNT_PERCENT).ToString("0.00");
            lblCNDiscountAmount.Text = Convert.ToDouble(SRMEnt.DISCOUNT_AMOUNT).ToString("0.00");
            lblCNTaxableAmount.Text = Convert.ToDouble(SRMEnt.TAXABLE_AMOUNT).ToString("0.00");
            lblCNVATPercent.Text = SRMEnt.TAX_VAT_AMOUNT == "0" ? "0" : (Convert.ToDouble(PG.CompanyTAXPercent()).ToString("00.00"));
            lblCNVATAmount.Text = Convert.ToDouble(SRMEnt.TAX_VAT_AMOUNT).ToString("0.00");
            lblCNGrandTotal.Text = Convert.ToDouble(SRMEnt.GRAND_TOTAL).ToString("0.00");
            lblCNRoundoff.Text = SRMEnt.ROUND_OFF;
            lblCNInvoiceAmount.Text = Convert.ToDouble(SRMEnt.SALES_RETURN_AMOUNT).ToString("0.00");
            lblCNAmounInWords.Text = hf.NumWordsWrapper(Convert.ToDouble(SRMEnt.SALES_RETURN_AMOUNT)) + " Only";
            lblCNRemarks.Text = SRMEnt.RETURN_REMARKS;
            lblInvCreatedBy.Text = hf.getEmployeeName(SRMEnt.USER_ID);


            printdetail.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
        }

    }
    #endregion

    protected void LoadSalesRetGrid(string PK_id)
    {
        DataTable dt = hf.LoadSalesReturnInvoice(PK_id); // your existing data
        int desiredRowCount = 30;

        while (dt.Rows.Count < desiredRowCount)
        {
            dt.Rows.Add(dt.NewRow()); // add empty rows to reach 20
        }

        gridSalesRetInvoice.DataSource = dt;
        gridSalesRetInvoice.DataBind();

    }


    protected void gridSalesRetInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblPK_ID = e.Row.FindControl("lblPK_ID") as Label;
            Label lblProdID = e.Row.FindControl("lblProdID") as Label;
            Label lblProdName = e.Row.FindControl("lblProdName") as Label;
            Label lblBatch = e.Row.FindControl("lblBatch") as Label;
            Label lblExpDate = e.Row.FindControl("lblExpDate") as Label;



            PEnt = new PRODUCT();
            PEnt.PK_ID = lblProdID.Text;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                lblProdName.Text = PEnt.PRODUCT_NAME;
            }


        }
    }

    protected void grdSalesReturn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            HtmlTableRow trInvVat = e.Row.FindControl("trInvVat") as HtmlTableRow;
            HtmlTableRow trRound = e.Row.FindControl("trRound") as HtmlTableRow;
            trRound.Visible = PGPS.RoundOff();
            trInvVat.Visible = PG.CompanyTAXType() == "VAT";
        }
    }
    protected void lblInvoiceNo_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((LinkButton)sender).Parent.Parent as GridViewRow;

        LinkButton lblInvoiceNo = gr.FindControl("lblInvoiceNo") as LinkButton;
        Label lblInvoiceDay = gr.FindControl("lblInvoiceDay") as Label;
        Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
        string url = "~/utilities/Sales/ShowInvoiceToPrint.aspx?invno=" + lblPK_ID.Text;
        string fullUrl = ResolveUrl(url);

        // Register JavaScript to open the URL in a new tab
        string script = "window.open('" + fullUrl + "', '_blank');";
        ScriptManager.RegisterStartupScript(this, GetType(), "OpenInvoice", script, true);

    }
}