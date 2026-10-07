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

    COUNTRY CONEnt = new COUNTRY();
    COUNTRYService CONSer = new COUNTRYService();

    CURRENCY_RATE CRTEnt = new CURRENCY_RATE();
    CURRENCY_RATEService CRTSer = new CURRENCY_RATEService();

    NAME_COMPANY NEnt = new NAME_COMPANY();
    NAME_COMPANYService NSer = new NAME_COMPANYService();

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    PRODUCT_CATEGORY PCEnt = new PRODUCT_CATEGORY();
    PRODUCT_CATEGORYService PCSer = new PRODUCT_CATEGORYService();

    SALES_INVOICE_DETAIL SIDEnt = new SALES_INVOICE_DETAIL();
    SALES_INVOICE_DETAILService SIDSer = new SALES_INVOICE_DETAILService();

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

        imgn.ImageUrl = "~/images/BarCode/Invoice/" + "18693" + ".jpg";

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

        // Build the three footer amounts the same way the original ProductExport
        // proforma print does: Sub Total from TOTAL_AMOUNT, Freight Charge from
        // FREIGHT_CHARGE, Grand Total from FINAL_AMOUNT.
        double proTotal = 0;
        double.TryParse(EIMEnt.TOTAL_AMOUNT, out proTotal);

        double proFlight = 0;
        double.TryParse(EIMEnt.FREIGHT_CHARGE, out proFlight);

        double proGrand = 0;
        double.TryParse(EIMEnt.FINAL_AMOUNT, out proGrand);

        string proSubTotalText = paymentCurrencyCode + " " + proTotal.ToString("##,##0.00");
        string proFlightChargeText = paymentCurrencyCode + " " + proFlight.ToString("##,##0.00");
        string proGrandTotalText = paymentCurrencyCode + " " + proGrand.ToString("##,##0.00");

        LoadSalesGridProforma(EIMEnt.INVOICE_MASTER_ID, proSubTotalText, proFlightChargeText, proGrandTotalText, paymentFactor, paymentCurrencyCode);

        printdetailProforma.Visible = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "printProforma", "printProforma();", true);
    }

    protected void PopulateProformaLabels(EXPORT_INVOICE_MASTER EIMEnt, string invoiceNumberForPrint)
    {
        string DecimalC = "";
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

            CONEnt = new COUNTRY();
            CONEnt.PK_ID = CEnt.COUNTRY;
            CONEnt = (COUNTRY)CONSer.GetSingle(CONEnt);
            if (CONEnt != null)
                lblProCustomerCountry.Text = CONEnt.COUNTRY_NAME;
            else
                lblProCustomerCountry.Text = "";
        }
        else
        {
            lblProCustomerName.Text = "";
            lblProCustomerAddress.Text = "";
            lblProCustomerCountry.Text = "";
        }

        string ProCurrencyName = EIMEnt.CURRENCY_TYPE_ID;
        FCEnt = new FOREIGN_CURRENCY();
        FCEnt.PK_ID = EIMEnt.CURRENCY_TYPE_ID;
        FCEnt = (FOREIGN_CURRENCY)FCSer.GetSingle(FCEnt);
        if (FCEnt != null)
        {
            ProCurrencyName = FCEnt.CURRENCY_NAME;
            DecimalC = FCEnt.DECIMAL_CURRENCY;
        }
        lblProPaymentCurrency.Text = ProCurrencyName;
        lblProPaymentMode.Text = hf.getPaymentType(EIMEnt.MODE_OF_PAYMENT);
        lblProShipmentType.Text = EIMEnt.SHIPMENT_TYPE;
        lblProShipmentNo.Text = EIMEnt.SHIPMENT_NUMBER;

        // Local (Sales Invoice) remarks go in the top remark line, matching the
        // original page where lblLocalRemarks = SIMEnt.REMARKS.
        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.PK_ID = EIMEnt.INVOICE_MASTER_ID;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
        if (SIMEnt != null)
            lblLocalRemarks.Text = SIMEnt.REMARKS;
        else
            lblLocalRemarks.Text = "";

        lblProRemarks.Text = EIMEnt.REMARKS;
        lblProUserName.Text = PG.CompanyName();

        // Same "amount in words" split as the original proforma print.
        double amount = Convert.ToDouble(EIMEnt.FINAL_AMOUNT);
        double integerAmount = Math.Truncate(amount);

        string[] parts = amount.ToString("F2", CultureInfo.InvariantCulture).Split('.');
        string decimalDigits = parts.Length > 1 ? parts[1] : "00";

        string decimalPart = "";
        if (decimalDigits != "00")
        {
            double decimalValue = double.Parse(decimalDigits, CultureInfo.InvariantCulture);
            string decimalWords = hf.NumWordsWrapper(decimalValue);
            decimalPart = " And " + decimalWords + " " + DecimalC;
        }

        lblProAmountInWord.Text = ProCurrencyName + " "
            + hf.NumWordsWrapper(integerAmount)
            + decimalPart + " only";
    }

    protected void LoadSalesGridProforma(string PK_id, string subTotalText,
     string flightChargeText, string grandTotalText,
     double paymentFactor = 1, string currencyCode = "")
    {
        DataTable DummyTable = hf.LoadSalesInvoice(PK_id);
        ConvertMoneyColumns(DummyTable, paymentFactor);

        if (!DummyTable.Columns.Contains("CATEGORY_CODE"))
            DummyTable.Columns.Add("CATEGORY_CODE", typeof(string));

        // Preload the SALES_INVOICE_DETAIL rows for this invoice so we have
        // PRODUCT_ID available for each grid row (LoadSalesInvoice does not
        // expose PRODUCT_ID / PRODUCT_CODE / SNO).
        SIDEnt = new SALES_INVOICE_DETAIL();
        SIDEnt.SALES_INVOICE_ID = PK_id;

        var productIdBySno = new Dictionary<string, string>();
        foreach (SALES_INVOICE_DETAIL d in SIDSer.GetAll(SIDEnt))
        {
            if (d != null && !string.IsNullOrEmpty(d.SNO))
                productIdBySno[d.SNO] = d.PRODUCT_ID;
        }

        var categoryCache = new Dictionary<string, string>();

        int rowIdx = 0;   // 0-based; converted to SNO (1-based) below
        foreach (DataRow row in DummyTable.Rows)
        {
            rowIdx++;

            string productId = "";

            // 1) If the DataTable happens to have PRODUCT_ID, use it.
            if (DummyTable.Columns.Contains("PRODUCT_ID") && row["PRODUCT_ID"] != DBNull.Value)
                productId = row["PRODUCT_ID"].ToString().Trim();

            // 2) Otherwise, look it up in the preloaded detail map.
            //    SALES_INVOICE_DETAIL.SNO is written as 1..N in insert order
            //    (from Container.DataItemIndex + 1), which is exactly the order
            //    of rows returned by hf.LoadSalesInvoice. So use row index + 1.
            if (string.IsNullOrEmpty(productId))
            {
                string sno = rowIdx.ToString();
                if (productIdBySno.ContainsKey(sno))
                    productId = productIdBySno[sno];
            }

            if (string.IsNullOrEmpty(productId))
            {
                row["CATEGORY_CODE"] = "";
                continue;
            }

            if (categoryCache.ContainsKey(productId))
            {
                row["CATEGORY_CODE"] = categoryCache[productId];
                continue;
            }

            // 3) PRODUCT via GetSingle by PK_ID
            PRODUCT productEnt = new PRODUCT();
            productEnt.PK_ID = productId;
            productEnt = (PRODUCT)PSer.GetSingle(productEnt);

            // 4) PRODUCT_CATEGORY via GetSingle by PK_ID = PRODUCT.CATEGORY_ID
            string categoryCode = "";
            if (productEnt != null && !string.IsNullOrEmpty(productEnt.CATEGORY_ID))
            {
                PRODUCT_CATEGORY catEnt = new PRODUCT_CATEGORY();
                catEnt.PK_ID = productEnt.CATEGORY_ID;
                catEnt = (PRODUCT_CATEGORY)PCSer.GetSingle(catEnt);

                if (catEnt != null && !string.IsNullOrEmpty(catEnt.CATEGORY_CODE))
                    categoryCode = catEnt.CATEGORY_CODE;
            }

            categoryCache[productId] = categoryCode;
            row["CATEGORY_CODE"] = categoryCode;
        }

        string[] PrefixColumns = new string[] { "RATE", "TOTAL" };
        PrefixCurrencyCode(DummyTable, currencyCode, PrefixColumns);

        gridProforma.DataSource = DummyTable;
        gridProforma.DataBind();

        hdnProSubTotal.Value = subTotalText;
        hdnProFreightCharge.Value = flightChargeText;
        hdnProGrandTotal.Value = grandTotalText;

        ScriptManager.RegisterStartupScript(this, this.GetType(),
            "formatProformaFooter", "formatProformaFooter();", true);
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