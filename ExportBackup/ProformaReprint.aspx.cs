using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using PhyeGanCore;
using Service.Components;
using System.Data;
using System.Globalization;


public partial class Export_ProformaReprint : System.Web.UI.Page
{
    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

    EXPORT_INVOICE_MASTER EIMEnt = new EXPORT_INVOICE_MASTER();
    EXPORT_INVOICE_MASTERService EIMSer = new EXPORT_INVOICE_MASTERService();

    SALES_INVOICE_MASTER SIMEnt = new SALES_INVOICE_MASTER();
    SALES_INVOICE_MASTERService SIMSer = new SALES_INVOICE_MASTERService();

    FOREIGN_CURRENCY FCEnt = new FOREIGN_CURRENCY();
    FOREIGN_CURRENCYService FCSer = new FOREIGN_CURRENCYService();

    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    CURRENCY_RATE CRTEnt = new CURRENCY_RATE();
    CURRENCY_RATEService CRTSer = new CURRENCY_RATEService();

    NAME_COMPANY NEnt = new NAME_COMPANY();
    NAME_COMPANYService NSer = new NAME_COMPANYService();

    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfile = new UserProfileEntity();
    static string path = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["postids"] = System.Guid.NewGuid().ToString();
            Session["postid"] = ViewState["postids"].ToString();
            try
            {
                userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
                if (hf.checkPageAccess(path, userProfile.UserGroupID.ToString()))
                {
                    LoadFiscalYear();
                    txtFromDate.Text = PGD.GetTodayNepaliDate();
                    txtToDate.Text = PGD.GetTodayNepaliDate();
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

    protected void chkDateWise_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateWise.Checked)
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

    protected void btnShow_Click(object sender, EventArgs e)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        DataTable dt = new DataTable();

        if (chkDateWise.Checked)
        {
            string fromEng = PGD.GetEnglishDateFromNepali(txtFromDate.Text, "dd/mm/yyyy");
            string toEng = PGD.GetEnglishDateFromNepali(txtToDate.Text, "dd/mm/yyyy");
            dt = hf.getExportSalesList("", "", fromEng, toEng, userProfile.LocationID);
        }
        else
        {
            string invNo = txtInvoiceNo.Text.Trim();
            dt = hf.getExportSalesList(ddlFiscalYear.SelectedValue, invNo, "", "", userProfile.LocationID);
        }

        grdProforma.DataSource = dt;
        grdProforma.DataBind();
    }

    protected void grdProforma_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Print")
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
            if (lblPK_ID != null)
            {
                LoadToPrint(lblPK_ID.Text);
                imgn.ImageUrl = "~/images/BarCode/Invoice/" + "18693" + ".jpg";
            }

            
        }
    }

    protected void lblInvoiceNo_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        GridViewRow gr = (GridViewRow)lnk.NamingContainer;
        Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
        if (lblPK_ID != null)
            LoadToPrint(lblPK_ID.Text);
    }

    protected void LoadToPrint(string pk_id)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

        EIMEnt = new EXPORT_INVOICE_MASTER();
        EIMEnt.PK_ID = pk_id;
        EIMEnt = (EXPORT_INVOICE_MASTER)EIMSer.GetSingle(EIMEnt);
        if (EIMEnt == null) return;

        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.PK_ID = EIMEnt.INVOICE_MASTER_ID;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
        if (SIMEnt == null) return;

        string paymentCurrencyCode = "";
        FCEnt = new FOREIGN_CURRENCY();
        FCEnt.PK_ID = EIMEnt.CURRENCY_TYPE_ID;
        FCEnt = (FOREIGN_CURRENCY)FCSer.GetSingle(FCEnt);
        if (FCEnt != null) paymentCurrencyCode = FCEnt.CURRENCY_CODE;

        double paymentFactor = 1;
        double nprSubTotal = 0;
        double fcySubTotal = 0;
        double.TryParse(SIMEnt.TAXABLE_SUB_TOTAL, out nprSubTotal);
        double.TryParse(EIMEnt.AMOUNT, out fcySubTotal);

        if (nprSubTotal > 0 && fcySubTotal > 0)
            paymentFactor = fcySubTotal / nprSubTotal;

        PopulateProformaLabels(EIMEnt, SIMEnt.INVOICE_NUMBER);

        string proSubTotalText = paymentCurrencyCode + " " + Convert.ToDouble(EIMEnt.FINAL_AMOUNT).ToString("##,##0.00");
        LoadSalesGridProforma(EIMEnt.INVOICE_MASTER_ID, proSubTotalText, paymentFactor, paymentCurrencyCode);

        printdetailProforma.Visible = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "printProforma", "printProforma();", true);
    }

    protected void PopulateProformaLabels(EXPORT_INVOICE_MASTER EIMEnt, string invoiceNumberForPrint)
    {
        lblProCompanyName.Text = PG.CompanyName();
        lblProCompanyAddress.Text = PG.BranchAddress(userProfile.LocationID);
        lblProCompanyRegNo.Text = PG.CompanyRegistration();
        lblProEximCode.Text = PG.EXIMCODE();
        lblProCompanyEmail.Text = PG.CompanyEmail();

        lblProInvoiceNo.Text = invoiceNumberForPrint;

        DateTime proIssue;
        if (DateTime.TryParse(EIMEnt.INVOICE_DATE, out proIssue))
            lblProIssueDate.Text = PGD.ConvertToOrdinalDateFormat(proIssue.ToString("dd/MM/yyyy"));
        else
            lblProIssueDate.Text = EIMEnt.INVOICE_DATE;

        lblProContractNo.Text = EIMEnt.CONTRACT_NUMBER;

        DateTime proContract;
        if (DateTime.TryParse(EIMEnt.CONTRACT_DATE, out proContract))
            lblProContractDate.Text = PGD.ConvertToOrdinalDateFormat(proContract.ToString("dd/MM/yyyy"));
        else
            lblProContractDate.Text = EIMEnt.CONTRACT_DATE;

        CEnt = new CUSTOMER();
        CEnt.PK_ID = EIMEnt.CUSTOMER_ID;
        CEnt = (CUSTOMER)CSer.GetSingle(CEnt);

        if (CEnt != null)
        {
            lblProCustomerName.Text = CEnt.CUSTOMER_NAME;
            lblProCustomerAddress.Text = CEnt.ADDRESS;
        }
        else
        {
            lblProCustomerName.Text = "";
            lblProCustomerAddress.Text = "";
        }

        string currencyName = EIMEnt.CURRENCY_TYPE_ID;
        FCEnt = new FOREIGN_CURRENCY();
        FCEnt.PK_ID = EIMEnt.CURRENCY_TYPE_ID;
        FCEnt = (FOREIGN_CURRENCY)FCSer.GetSingle(FCEnt);
        if (FCEnt != null)
            currencyName = FCEnt.CURRENCY_NAME;

        lblProPaymentCurrency.Text = currencyName;
        lblProPaymentMode.Text = hf.getPaymentType(EIMEnt.MODE_OF_PAYMENT);
        lblProShipmentType.Text = EIMEnt.SHIPMENT_TYPE;
        lblProShipmentNo.Text = EIMEnt.SHIPMENT_NUMBER;
        lblProRemarks.Text = EIMEnt.REMARKS;
        lblProUserName.Text = PG.CompanyName();
        lblProAmountInWord.Text = currencyName + " " + hf.NumWordsWrapper(Convert.ToDouble(EIMEnt.FINAL_AMOUNT)) + " only";
    }

    protected void LoadSalesGridProforma(string PK_id, string subTotalText, double paymentFactor = 1, string currencyCode = "")
    {
        DataTable DummyTable = hf.LoadSalesInvoice(PK_id);
        ConvertMoneyColumns(DummyTable, paymentFactor);

        string[] PrefixColumns = new string[] { "RATE", "TOTAL" };
        PrefixCurrencyCode(DummyTable, currencyCode, PrefixColumns);

        gridProforma.DataSource = DummyTable;
        gridProforma.DataBind();

        GridViewRow FooterRow = gridProforma.FooterRow;
        if (FooterRow != null && FooterRow.Cells.Count >= 6)
        {
            FooterRow.Cells[0].Text = "";
            FooterRow.Cells[0].Style.Add("border", "1px solid #000");

            FooterRow.Cells[1].Text = "";
            FooterRow.Cells[1].Style.Add("border", "1px solid #000");

            FooterRow.Cells[2].ColumnSpan = 3;
            FooterRow.Cells[2].Text = "Sub Total";
            FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Left;
            FooterRow.Cells[2].Style.Add("border", "1px solid #000");

            FooterRow.Cells[3].Visible = false;
            FooterRow.Cells[4].Visible = false;

            FooterRow.Cells[5].Text = subTotalText;
            FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            FooterRow.Cells[5].Style.Add("border", "1px solid #000");
        }

        hdnProSubTotal.Value = subTotalText;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "formatProformaFooter", "formatProformaFooter();", true);
    }

    private void ConvertMoneyColumns(DataTable dt, double billingFactor)
    {
        if (billingFactor == 1 || dt == null) return;
        string[] MoneyColumns = new string[] { "RATE", "TOTAL", "SCHEME_DISCOUNT", "TAXABLE_TOTAL" };
        foreach (DataRow DRRow in dt.Rows)
        {
            foreach (string ColName in MoneyColumns)
            {
                if (dt.Columns.Contains(ColName) && DRRow[ColName] != DBNull.Value && DRRow[ColName] != null)
                {
                    double val;
                    if (double.TryParse(DRRow[ColName].ToString(), out val))
                    {
                        DRRow[ColName] = (val * billingFactor).ToString("##0.00");
                    }
                }
            }
        }
    }

    private void PrefixCurrencyCode(DataTable dt, string currencyCode, string[] columns)
    {
        if (string.IsNullOrEmpty(currencyCode) || dt == null) return;

        foreach (DataRow DRRow in dt.Rows)
        {
            foreach (string ColName in columns)
            {
                if (dt.Columns.Contains(ColName) && DRRow[ColName] != DBNull.Value && DRRow[ColName] != null)
                {
                    string val = DRRow[ColName].ToString();
                    if (!string.IsNullOrEmpty(val))
                        DRRow[ColName] = currencyCode + " " + val;
                }
            }
        }
    }
}