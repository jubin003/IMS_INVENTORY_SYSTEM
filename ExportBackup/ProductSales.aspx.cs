using System;
using System.Web;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;
using System.Data;
using DataHelper.Framework;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.UI;
using System.Globalization;
using System.Collections;
using Entity.Framework;

public partial class Export_ProductSales : System.Web.UI.Page
{

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();

    PAYMENT_TYPE PTEnt = new PAYMENT_TYPE();
    PAYMENT_TYPEService PTSer = new PAYMENT_TYPEService();

    EXPORT_INVOICE_MASTER EIMEnt = new EXPORT_INVOICE_MASTER();
    EXPORT_INVOICE_MASTERService EIMSer = new EXPORT_INVOICE_MASTERService();

    EXPORT_INVOICE_DETAIL EIDEnt = new EXPORT_INVOICE_DETAIL();
    EXPORT_INVOICE_DETAILService EIDSer = new EXPORT_INVOICE_DETAILService();

    FOREIGN_CURRENCY FCEnt = new FOREIGN_CURRENCY();
    FOREIGN_CURRENCYService FCSer = new FOREIGN_CURRENCYService();

    CURRENCY_RATE CRTEnt = new CURRENCY_RATE();
    CURRENCY_RATEService CRTSer = new CURRENCY_RATEService();

    BANK_ACCOUNT BAEnt = new BANK_ACCOUNT();
    BANK_ACCOUNTService BASer = new BANK_ACCOUNTService();

    VOUCHER_MASTER VMEnt = new VOUCHER_MASTER();
    VOUCHER_MASTERService VMSer = new VOUCHER_MASTERService();

    VOUCHER_CHILD VCEnt = new VOUCHER_CHILD();
    VOUCHER_CHILDService VCSer = new VOUCHER_CHILDService();

    PRODUCT_RATE_TYPE CTEnt = new PRODUCT_RATE_TYPE();
    PRODUCT_RATE_TYPEService CTSer = new PRODUCT_RATE_TYPEService();

    PURCHASE_INVOICE_DETAIL PIDEnt = new PURCHASE_INVOICE_DETAIL();
    PURCHASE_INVOICE_DETAILService PIDSer = new PURCHASE_INVOICE_DETAILService();

    PRODUCT_RATES PREnt = new PRODUCT_RATES();
    PRODUCT_RATEService PRSer = new PRODUCT_RATEService();

    NAME_COMPANY NEnt = new NAME_COMPANY();
    NAME_COMPANYService NSer = new NAME_COMPANYService();

    ORDER_NUMBER ONEnt = new ORDER_NUMBER();
    ORDER_NUMBERService ONSer = new ORDER_NUMBERService();

    SALES_INVOICE_MASTER SIMEnt = new SALES_INVOICE_MASTER();
    SALES_INVOICE_MASTERService SIMSer = new SALES_INVOICE_MASTERService();

    SALES_INVOICE_DETAIL SIDEnt = new SALES_INVOICE_DETAIL();
    SALES_INVOICE_DETAILService SIDSer = new SALES_INVOICE_DETAILService();

    PACKING_LIST_TEMP EPackTemp = new PACKING_LIST_TEMP();
    PACKING_LIST_TEMPService SPackTemp = new PACKING_LIST_TEMPService();

    PACKING_LIST_TEMP PLTEnt = new PACKING_LIST_TEMP();
    EntityList PLTList = new EntityList();

    PACKING_LIST PLPEnt = new PACKING_LIST();
    PACKING_LISTService PLPSer = new PACKING_LISTService();

    UserProfileEntity userProfile = new UserProfileEntity();
    HelperFunction hf = new HelperFunction();
    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGan PG = new PhyeGan();
    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();
    AccountFunction af = new AccountFunction();

    Boolean flag = false;
    bool OfficeCopy;
    string path = "";

    DataTable DT = new DataTable();
    DataTable DummyTable = new DataTable();
    DataView DV = new DataView();
    DataRow DR = null;
    DataRowView DRV = null;
    EntityList TheList = new EntityList();

    Label LblSno = null;
    Label LblTaxable = null;
    Label LblProductPK_ID = null;
    TextBox TxtGridQty = null;
    Label LblRate = null;
    Label LblItemTotal = null;
    Label LblScheDisc = null;
    Label LblAfterScheDisc = null;
    Label LblBatch = null;
    Label LblExpDate = null;
    Label LblProductCode = null;
    Label LblProductName = null;
    Label LblUQty = null;
    Label LblUUnit = null;
    Label LblUnit = null;
    Label LblProductRate = null;
    Label LblFCRate = null;
    Label LblFCTotal = null;
    Label LblNumericRate = null;
    Label LblProName80 = null;
    Label LblHSCode = null;
    Label LblPackQty = null;
    Label LblPackNo = null;
    TextBox TxtPackQty = null;
    TextBox TxtPackNo = null;
    ImageButton ImgPackEdit = null;
    ImageButton ImgPackDelete = null;
    ImageButton ImgPackUpdate = null;
    ImageButton ImgPackCancel = null;

    GridViewRow GRow = null;
    GridViewRow FooterRow = null;

    string Msg = "";
    string UniqueToken = "";
    string PkId = "";
    string NewQty = "";
    string NewPackNo = "";
    string InvoiceNumberForPrint = "";
    string PaymentCurrencyCode = "";
    string BillingSymbol = "Rs";
    string BillingAsOfDateEnglish = "";
    string PaymentAsOfDateEnglish = "";
    string InvoiceDateEnglish = "";
    string TodayEng = "";
    string SalesInvoiceNumber = "";
    string ExportInvoiceNumber = "";
    string InvoiceNumber = "";
    string GeneratedSalesInvoiceNumber = "";
    string SalesPkId = "";
    string ExportPkId = "";
    string VoucherPkId = "";
    string BillType = "A5L";
    string[] DateParts = new string[0];
    string Month = "";
    string Day = "";
    string Year = "";
    string EngDate = "";
    string ProCurrencyName = "";
    string ProSubTotalText = "";
    string[] MoneyColumns = new string[0];
    string[] PrefixColumns = new string[0];
    string[] Columns = new string[0];
    string Col = "";
    string HsCode = "";
    string ProductCode = "";
    string CurrencyId = "";
    string DisplayCurrencyCode = "";
    string FcRateText = "";
    string FcTotalText = "";
    string Text80 = "";
    string BillingCurrencyId = "";
    string BillingDisplayCurrencyCode = "";

    int Sno = 0;
    int NextOrder = 0;

    bool Ready = false;
    bool IsEditing = false;
    bool IsSameRow = false;
    bool HasProduct = false;

    double Qty = 0;
    double Rate = 0;
    double Rate2 = 0;
    double Amount = 0;
    double SubTotal = 0;
    double DiscountAmount = 0;
    double DiscountPercent = 0;
    double TaxableTotal = 0;
    double InvoiceAmount = 0;
    double Total = 0;
    double ScheDisc = 0;
    double Discount = 0;
    double DiscountPercentAmt = 0;
    double BeforeDecimal = 0;
    double AfterDecimal = 0;
    double Vat = 0;
    double PreReturnTotal = 0;
    double VatReturn = 0;
    double GrandTotalForRound = 0;
    double NprRate = 0;
    double NprTotal = 0;
    double DisplayRate = 0;
    double NumericRate = 0;
    double DisplayTotal = 0;
    double OldNprRate = 0;
    double BillingExchangeRate = 1.0;
    double DisplayExchangeRate = 1.0;
    double BillingRate = 1.0;
    double BRate = 1.0;
    double PaymentRate = 0;
    double PaymentFactor = 1;
    double BillingFactor = 1;
    double SubTotalConverted = 0;
    double DiscountConverted = 0;
    double TaxableTotalConverted = 0;
    double InvoiceAmountConverted = 0;
    double TaxableSubTotal80 = 0;
    double DiscAmount80 = 0;
    double TaxableTotal80 = 0;
    double VatAmount80 = 0;
    double InvoiceAmount80 = 0;
    double ResolvedExchangeRate = 0;
    double VatReturnAmount = 0;
    double LocalSubTotal = 0;
    double LocalDiscountAmount = 0;
    double LocalAdvanceAmount = 0;
    double ExportAmount = 0;
    double ExportDiscountAmount = 0;
    double ExportTotalAmount = 0;
    double ExportAdvanceAmount = 0;
    double ExportFinalAmount = 0;
    double LineExchangeRate = 0;
    double LocalRate = 0;
    double LocalTotal = 0;
    double ConvertedRate = 0;
    double ConvertedTotal = 0;
    double Sd = 0;
    double ExchangeRate = 0;
    double ForeignRate = 0;
    double FcRate = 0;
    double AfterScheDisc = 0;
    double BillingRateForGrid = 1.0;
    double CurrentQty = 0;
    double AlreadyPacked = 0;
    double Remaining = 0;
    double Q = 0;
    double RoundOff = 0;
    double RoundOffValue = 0;
    double Asd = 0;

    DateTime ResolvedRateDate = DateTime.MinValue;
    DateTime BRateDate = DateTime.MinValue;
    DateTime BillingRateDate = DateTime.MinValue;
    DateTime PaymentRateDate = DateTime.MinValue;
    DateTime Today = DateTime.Now.Date;
    DateTime DateOut = DateTime.MinValue;

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
                    CreateGridFirst();
                    LoadCustomers();
                    LoadProduct();
                    LoadBank();
                    LoadPaymentCurrency();
                    LoadBillingCurrency();
                    UpdateBillingExchangeRateInfo();
                    SetVisibilty();
                    loadCustomerType();
                    ddLoadMOP();
                    //ddlPaymentType.SelectedValue = "CR";
                    txtQty.Text = "1";
                    txtVATPercent.Text = PG.CompanyTAXPercent();
                    txtTransactionDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
                    txtInvoiceDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
                    lblunique_token.Text = userProfile.UserName + hf.getmaxinvid();

                    if (PG.CompanyTAXType() != "VAT")
                    {
                        divVAT1.Visible = false;
                        divVAT2.Visible = false;
                    }

                    if (PGPS.ProductBatch())
                    {
                        divBatch.Visible = true;
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
            catch (Exception)
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }

    protected void LoadCompanyDetail()
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        lblCompanyName.Text = PG.CompanyName();
        lblCompanyAddress.Text = PG.BranchAddress(userProfile.LocationID);
        lblEmail.Text = PG.BranchEmail(userProfile.LocationID);
        lblWebsite.Text = PG.CompanyWebsite();
        if (PG.CompanyEmail() == "" && PG.CompanyWebsite() == "")
            divEmail.Visible = false;
        lblPanNo.Text = PG.CompanyVATPan();
        lblPhone1.Text = PG.BranchContact(userProfile.LocationID);

        lblCompanyName80.Text = PG.CompanyName();
        lblCompanyAddress80.Text = PG.BranchAddress(userProfile.LocationID);
        lblPhone80.Text = PG.BranchContact(userProfile.LocationID);
        lblEximCode80.Text = PG.EXIMCODE();
    }

    protected void loadCustomerType()
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        CTEnt = new PRODUCT_RATE_TYPE();
        CTEnt.STATUS = "1";
        CTEnt.OFFICE_CODE = userProfile.LocationID;
        ddlProductRateType.DataSource = CTSer.GetAll(CTEnt);
        ddlProductRateType.DataTextField = "RATE_TYPE_NAME";
        ddlProductRateType.DataValueField = "PK_ID";
        ddlProductRateType.DataBind();
    }

    protected void ddLoadMOP()
    {
        PTEnt = new PAYMENT_TYPE();
        PTEnt.STATUS = "1";
        PTEnt.SALES_PURCHASE = "S";
        ddlPaymentType.DataSource = PTSer.GetAll(PTEnt);
        ddlPaymentType.DataTextField = "PAYMENT_NAME";
        ddlPaymentType.DataValueField = "PAYMENT_CODE";
        ddlPaymentType.DataBind();
        ddlPaymentType.Items.Insert(0, "Select");

    }

    protected void LoadCustomers()
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        CEnt = new CUSTOMER();
        CEnt.STATUS = "1";
        CEnt.OFFICE_CODE = userProfile.LocationID;
        ddlCustomer.DataSource = CSer.GetAll(CEnt);
        ddlCustomer.DataTextField = "CUSTOMER_FULLNAME";
        ddlCustomer.DataValueField = "PK_ID";
        ddlCustomer.DataBind();
        ddlCustomer.Items.Insert(0, "Select");
    }

    protected void LoadProduct()
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        PEnt = new PRODUCT();
        PEnt.STATUS = "1";
        TheList = new EntityList();
        TheList = PSer.GetAll(PEnt);
        ddlProduct.DataSource = TheList;
        ddlProduct.DataTextField = "PRODUCT_FULLNAME";
        ddlProduct.DataValueField = "PK_ID";
        ddlProduct.DataBind();
        ddlProduct.Items.Insert(0, "Select");
    }

    protected void LoadBank()
    {
        BAEnt = new BANK_ACCOUNT();
        BAEnt.SHOW_IN_RECEIPT = "1";
        ddlBank.DataSource = BASer.GetAll(BAEnt);
        ddlBank.DataTextField = "BANK_NAME";
        ddlBank.DataValueField = "BANK_CODE";
        ddlBank.DataBind();
    }

    protected void LoadPaymentCurrency()
    {
        FCEnt = new FOREIGN_CURRENCY();
        FCEnt.STATUS = "1";
        ddlPaymentCurrency.DataSource = FCSer.GetAll(FCEnt);
        ddlPaymentCurrency.DataTextField = "CURRENCY_NAME";
        ddlPaymentCurrency.DataValueField = "PK_ID";
        ddlPaymentCurrency.DataBind();
        ddlPaymentCurrency.Items.Insert(0, "Select");
    }

    protected void LoadBillingCurrency()
    {
        FCEnt = new FOREIGN_CURRENCY();
        FCEnt.STATUS = "1";
        ddlBillingIn.DataSource = FCSer.GetAll(FCEnt);
        ddlBillingIn.DataTextField = "CURRENCY_NAME";
        ddlBillingIn.DataValueField = "PK_ID";
        ddlBillingIn.DataBind();
        ddlBillingIn.Items.Insert(0, "Select");
        UpdateBillingExchangeRateInfo();
    }

    protected void LoadBatch(string product)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        ddlBatch.Items.Clear();
        ArrayList batch_array = new ArrayList();

        DT = hf.getOpeningClosingBalance(PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()),
            null, null, product, null, null, null, null, userProfile.LocationID);

        if (DT != null && DT.Rows.Count > 0)
        {
            if (DT.Columns.Contains("batch_number"))
            {
                foreach (DataRow DRow in DT.Rows)
                {
                    string batchVal = "";
                    if (DRow["batch_number"] != DBNull.Value)
                    {
                        batchVal = DRow["batch_number"].ToString();
                    }
                    if (batchVal != "")
                    {
                        batch_array.Add(batchVal);
                    }
                }
            }
        }

        ddlBatch.DataSource = batch_array;
        ddlBatch.DataBind();
    }

    protected void LoadAvailablity(string product, string batch)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

        if (PGPS.SHOW_AVAILABILITY() || PGPS.OnlyStockSales())
        {
            DT = hf.getOpeningClosingBalance(PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()),
                null, null, product, batch, null, null, null, userProfile.LocationID);

            if (DT != null && DT.Rows.Count > 0)
            {
                if (DT.Columns.Contains("CLOSING"))
                {
                    foreach (DataRow DRow in DT.Rows)
                    {
                        if (DRow["CLOSING"] != DBNull.Value)
                        {
                            txtAvilableQty.Text = DRow["CLOSING"].ToString();
                        }
                    }
                }

                if (DT.Columns.Contains("expiry_date"))
                {
                    foreach (DataRow DRow in DT.Rows)
                    {
                        if (DRow["expiry_date"] != DBNull.Value)
                        {
                            txtExpDate.Text = DRow["expiry_date"].ToString();
                        }
                    }
                }
            }
            else
            {
                txtAvilableQty.Text = "0";
            }

            if (PGPS.OnlyStockSales())
            {
                int availQty = 0;
                int.TryParse(txtAvilableQty.Text, out availQty);
                if (availQty > 0)
                {
                    btnAdd.Visible = true;
                }
                else
                {
                    btnAdd.Visible = false;
                }
            }
        }
    }

    protected void LoadRateType()
    {
        CTEnt = new PRODUCT_RATE_TYPE();
        CTEnt.PK_ID = ddlProductRateType.SelectedValue;
        CTEnt = (PRODUCT_RATE_TYPE)CTSer.GetSingle(CTEnt);

        if (CTEnt != null)
        {
            if (CTEnt.RATE_EDITABLE == "0")
            {
                txtRate.Enabled = false;
                txtAmount.Enabled = false;
            }
            else
            {
                txtRate.Enabled = true;
                txtAmount.Enabled = true;
            }
        }
    }

    protected void LoadQty()
    {
        Qty = 0;
        Rate = 0;
        try
        {
            Qty = Convert.ToDouble(txtQty.Text);
            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlProduct.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                if (PEnt.DUAL_UNIT == "1")
                    txtUQty.Text = (Convert.ToDouble(txtQty.Text) / Convert.ToDouble(PEnt.PACK_QTY)).ToString("0.##");

                txtRate.Focus();

                if (PGPS.OnlyStockSales() || PGPS.SHOW_AVAILABILITY())
                {
                    if (Convert.ToDouble(txtAvilableQty.Text) - Convert.ToDouble(txtQty.Text) < 0)
                    {
                        HelperFunction.MsgBox(this, this.GetType(), "Sorry Quantity Exceeds Stock!!!");
                        if (PGPS.OnlyStockSales())
                            btnAdd.Visible = false;
                        else
                            btnAdd.Visible = true;
                    }
                    else
                    {
                        btnAdd.Visible = true;
                    }
                }
            }
        }
        catch
        {
            txtQty.Focus();
            txtQty.Text = "";
            HelperFunction.MsgBox(this, this.GetType(), "Enter Number Only. ");
            return;
        }

        try
        {
            Rate = Convert.ToDouble(txtRate.Text);
            txtAmount.Text = (Qty * Rate).ToString("#0.00");
        }
        catch
        {
            txtRate.Focus();
            txtRate.Text = "";
        }
    }

    protected void loadRate()
    {
        PREnt = new PRODUCT_RATES();
        PREnt.CUSTOMER_TYPE_ID = ddlProductRateType.SelectedValue;
        PREnt.PRODUCT_ID = ddlProduct.SelectedValue;
        PREnt = (PRODUCT_RATES)PRSer.GetSingle(PREnt);

        if (PREnt != null)
        {
            if (txtQty.Text == "")
            {
                txtQty.Text = "1";
            }
            txtRate.Text = PREnt.RATE;
            LoadQty();
        }
        else
        {
            txtRate.Text = "0";
            LoadQty();
        }
    }

    protected bool TryGetExchangeRate(string currencyId, string asOfDateEnglish, out double rate, out DateTime rateDate)
    {
        rate = 0;
        rateDate = DateTime.MinValue;
        if (string.IsNullOrEmpty(currencyId))
            return false;

        DateTime asOf;
        if (!DateTime.TryParseExact(asOfDateEnglish, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out asOf))
            return false;

        string[] datesToTry = new string[2];
        datesToTry[0] = asOfDateEnglish;
        string[] parts = asOfDateEnglish.Split('/');
        datesToTry[1] = parts[1] + "/" + parts[0] + "/" + parts[2];

        CRTEnt = new CURRENCY_RATE();
        CRTEnt.CURRENCY_TYPE_ID = currencyId;
        EntityList rateList = CRTSer.GetAll(CRTEnt);

        foreach (string dateStr in datesToTry)
        {
            DateTime dt;
            if (!DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                continue;

            foreach (CURRENCY_RATE CRRow in rateList)
            {
                DateTime crDate;
                if (DateTime.TryParseExact(CRRow.CONVERSION_DATE, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out crDate) &&
                    crDate.Date == dt.Date)
                {
                    rateDate = crDate;
                    rate = Convert.ToDouble(CRRow.EXCHANGE_RATE);
                    return true;
                }
            }
        }

        return false;
    }

    protected void CheckTodaysExchangeRate()
    {
        if (string.IsNullOrEmpty(ddlPaymentCurrency.SelectedValue))
            return;

        string todayEnglish = PGD.GetTodayDate("dd/mm/yyyy");
        if (!DateTime.TryParseExact(todayEnglish, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out Today))
            Today = DateTime.Now.Date;

        CRTEnt = new CURRENCY_RATE();
        CRTEnt.CURRENCY_TYPE_ID = ddlPaymentCurrency.SelectedValue;
        EntityList rateList = CRTSer.GetAll(CRTEnt);

        bool foundToday = false;
        foreach (CURRENCY_RATE CRRow in rateList)
        {
            DateTime crDate;
            if (!DateTime.TryParseExact(CRRow.CONVERSION_DATE, "MM/dd/yyyy",
        CultureInfo.InvariantCulture, DateTimeStyles.None, out crDate))
                continue;

            if (crDate.Date == Today.Date)
            {
                foundToday = true;
                break;
            }
        }

        if (!foundToday)
        {
            HelperFunction.MsgBox(this, this.GetType(),
                "Today's exchange rate not found for " + ddlPaymentCurrency.SelectedItem.Text + ".");
        }
    }

    protected void UpdateBillingExchangeRateInfo()
    {
        // ---------- Product Price In (Billing Currency) ----------
        if (string.IsNullOrEmpty(ddlBillingIn.SelectedValue) || ddlBillingIn.SelectedValue == "Select")
        {
            lblBillingExchangeRateInfo.Text = "";
            lblBillingExchangeRateInfo.Visible = false;
        }
        else
        {
            double todayBillingRate;
            DateTime todayBillingDate;
            if (TryGetTodaysExchangeRate(ddlBillingIn.SelectedValue, out todayBillingRate, out todayBillingDate))
            {
                lblBillingExchangeRateInfo.Text = "Today's Rate = "
                    + todayBillingRate.ToString("##0.00")
                    + " (as of " + todayBillingDate.ToString("dd/MM/yyyy") + ")";
            }
            else
            {
                lblBillingExchangeRateInfo.Text = "Today's Rate = N/A";
            }
            lblBillingExchangeRateInfo.Visible = true;
        }

        // ---------- Payment Currency ----------
        if (tdPaymentCurrency.Visible
            && !string.IsNullOrEmpty(ddlPaymentCurrency.SelectedValue)
            && ddlPaymentCurrency.SelectedValue != "Select")
        {
            double todayPaymentRate;
            DateTime todayPaymentDate;
            if (TryGetTodaysExchangeRate(ddlPaymentCurrency.SelectedValue, out todayPaymentRate, out todayPaymentDate))
            {
                lblPaymentExchangeRateInfo.Text = "Today's Rate = "
                    + todayPaymentRate.ToString("##0.00")
                    + " (as of " + todayPaymentDate.ToString("dd/MM/yyyy") + ")";
            }
            else
            {
                lblPaymentExchangeRateInfo.Text = "Today's Rate = N/A";
            }
            lblPaymentExchangeRateInfo.Visible = true;
        }
        else
        {
            lblPaymentExchangeRateInfo.Text = "";
            lblPaymentExchangeRateInfo.Visible = false;
        }
    }

    protected double GetDisplayCurrencyRate()
    {
        InvoiceDateEnglish = PGD.GetEnglishDateFromNepali(txtInvoiceDate.Text, "dd/mm/yyyy");

        if (!string.IsNullOrEmpty(ddlBillingIn.SelectedValue) &&
            ddlBillingIn.SelectedValue != "Select" &&
            ddlBillingIn.SelectedValue != "NPR")
        {
            if (TryGetExchangeRate(ddlBillingIn.SelectedValue, InvoiceDateEnglish, out Rate, out DateOut) && Rate > 0)
                return Rate;
        }

        if (tdPaymentCurrency.Visible &&
            !string.IsNullOrEmpty(ddlPaymentCurrency.SelectedValue) &&
            ddlPaymentCurrency.SelectedValue != "Select")
        {
            if (TryGetExchangeRate(ddlPaymentCurrency.SelectedValue, InvoiceDateEnglish, out Rate, out DateOut) && Rate > 0)
                return Rate;
        }

        return 1.0;
    }

    protected void SetVisibilty()
    {
        trRtotal.Visible = PGPS.RoundOff();
        trRateType.Visible = PGPS.MultipleRate();

        if (PGPS.MultipleRate() == false)
        {
            CTEnt = new PRODUCT_RATE_TYPE();
            CTEnt.PK_ID = "1";
            CTEnt = (PRODUCT_RATE_TYPE)CTSer.GetSingle(CTEnt);
            if (CTEnt != null)
            {
                if (CTEnt.RATE_EDITABLE == "0")
                    txtAmount.Enabled = false;
                else
                    txtAmount.Enabled = true;
            }
        }

        tdAvaiQty.Visible = PGPS.SHOW_AVAILABILITY();
        trRoundOff.Visible = PGPS.RoundOff();
        tdDualUnit.Visible = PGPS.ShowDualQuantity();
        lblUUnit.Visible = PGPS.ShowDualQuantity();
        tdSchDisc.Visible = PGPS.ItemWiseDiscount();

        grdSalesDetail.Columns[3].Visible = PGPS.ProductBatch();
        grdSalesDetail.Columns[4].Visible = PGPS.ProductExpDate();
        grdSalesDetail.Columns[5].Visible = PGPS.DualQuantity();
        grdSalesDetail.Columns[9].Visible = PGPS.ItemWiseDiscount();
        grdSalesDetail.Columns[10].Visible = PGPS.ItemWiseDiscount();

        bool showFCRate = false;

        if (!string.IsNullOrEmpty(ddlBillingIn.SelectedValue) &&
            ddlBillingIn.SelectedValue != "Select" &&
            ddlBillingIn.SelectedValue != "NPR")
        {
            showFCRate = true;
        }
        else if (!string.IsNullOrEmpty(ddlPaymentCurrency.SelectedValue) &&
                 ddlPaymentCurrency.SelectedValue != "Select")
        {
            showFCRate = true;
        }

        grdSalesDetail.Columns[11].Visible = showFCRate;
        grdSalesDetail.Columns[12].Visible = showFCRate;
        grdSalesDetail.Columns[13].Visible = false;
        grdSalesDetail.Columns[14].Visible = true;

        divPO.Visible = PGPS.ShowPO();

        if (ddlBillingIn.SelectedItem != null &&
            ddlBillingIn.SelectedItem.Text.Trim() == "NPR")
        {
            tdPaymentCurrency.Visible = true;
        }
        else
        {
            tdPaymentCurrency.Visible = false;
        }
    }

    protected DataTable CreateGridFirst()
    {
        DummyTable = new DataTable();
        DummyTable.Columns.Add("PK_ID");
        DummyTable.Columns.Add("TAXABLE");
        DummyTable.Columns.Add("PRODUCT_CODE");
        DummyTable.Columns.Add("PRODUCT");
        DummyTable.Columns.Add("BATCH_NO");
        DummyTable.Columns.Add("EXPIRY_DATE");
        DummyTable.Columns.Add("U_QUANTITY");
        DummyTable.Columns.Add("U_UNIT");
        DummyTable.Columns.Add("QUANTITY");
        DummyTable.Columns.Add("UNIT");
        DummyTable.Columns.Add("RATE");
        DummyTable.Columns.Add("TOTAL");
        DummyTable.Columns.Add("SCHE_DISC");
        DummyTable.Columns.Add("AFTER_SCHE_DISC");
        DummyTable.Columns.Add("PRODUCT_RATE");
        DummyTable.Columns.Add("EXCHANGE_RATE");
        DummyTable.Columns.Add("FC_RATE");
        DummyTable.Columns.Add("NUMERIC_RATE");

        DR = DummyTable.NewRow();
        DR["PK_ID"] = "";
        DR["TAXABLE"] = "";
        DR["PRODUCT_CODE"] = "";
        DR["PRODUCT"] = "";
        DR["BATCH_NO"] = "";
        DR["EXPIRY_DATE"] = "";
        DR["U_QUANTITY"] = "";
        DR["U_UNIT"] = "";
        DR["QUANTITY"] = "";
        DR["UNIT"] = "";
        DR["RATE"] = "";
        DR["TOTAL"] = "";
        DR["SCHE_DISC"] = "";
        DR["AFTER_SCHE_DISC"] = "";
        DR["PRODUCT_RATE"] = "";
        DR["EXCHANGE_RATE"] = "";
        DR["FC_RATE"] = "";
        DR["NUMERIC_RATE"] = "";

        DV = new DataView(DummyTable);
        grdSalesDetail.DataSource = DV;
        grdSalesDetail.DataBind();
        return DummyTable;
    }

    protected DataTable CreateGrid(int RowIndex, bool addRemoveFlag)
    {
        int row = grdSalesDetail.Rows.Count;
        DummyTable = new DataTable();
        DummyTable.Columns.Add("PK_ID");
        DummyTable.Columns.Add("TAXABLE");
        DummyTable.Columns.Add("PRODUCT_CODE");
        DummyTable.Columns.Add("PRODUCT");
        DummyTable.Columns.Add("BATCH_NO");
        DummyTable.Columns.Add("EXPIRY_DATE");
        DummyTable.Columns.Add("U_QUANTITY");
        DummyTable.Columns.Add("U_UNIT");
        DummyTable.Columns.Add("QUANTITY");
        DummyTable.Columns.Add("UNIT");
        DummyTable.Columns.Add("RATE");
        DummyTable.Columns.Add("TOTAL");
        DummyTable.Columns.Add("SCHE_DISC");
        DummyTable.Columns.Add("AFTER_SCHE_DISC");
        DummyTable.Columns.Add("PRODUCT_RATE");
        DummyTable.Columns.Add("EXCHANGE_RATE");
        DummyTable.Columns.Add("FC_RATE");
        DummyTable.Columns.Add("NUMERIC_RATE");

        if (row > 0)
        {
            foreach (GridViewRow r in grdSalesDetail.Rows)
            {
                LblProductPK_ID = r.FindControl("lblProductPK_ID") as Label;
                LblTaxable = r.FindControl("lblTaxable") as Label;
                LblProductCode = r.FindControl("lblProductCode") as Label;
                LblProductName = r.FindControl("lblProductName") as Label;
                LblBatch = r.FindControl("lblBatch") as Label;
                LblExpDate = r.FindControl("lblExpDate") as Label;
                LblUQty = r.FindControl("lblUQty") as Label;
                LblUUnit = r.FindControl("lblUUnit") as Label;
                TxtGridQty = r.FindControl("txtGridQty") as TextBox;
                LblUnit = r.FindControl("lblUnit") as Label;
                LblRate = r.FindControl("lblRate") as Label;
                LblItemTotal = r.FindControl("lblItemTotal") as Label;
                LblScheDisc = r.FindControl("lblScheDisc") as Label;
                LblAfterScheDisc = r.FindControl("lblAfterScheDisc") as Label;
                LblProductRate = r.FindControl("lblProductRate") as Label;
                LblFCRate = r.FindControl("lblFCRate") as Label;

                DR = DummyTable.NewRow();

                IsSameRow = false;
                if (LblProductPK_ID.Text == ddlProduct.SelectedValue &&
                    LblRate.Text == txtRate.Text &&
                    LblUQty.Text == txtUQty.Text)
                {
                    IsSameRow = true;
                }

                if (IsSameRow)
                {
                    DR["PK_ID"] = LblProductPK_ID.Text;
                    DR["TAXABLE"] = LblTaxable.Text;
                    DR["PRODUCT_CODE"] = LblProductCode.Text;
                    DR["PRODUCT"] = LblProductName.Text;
                    DR["BATCH_NO"] = LblBatch.Text;
                    DR["EXPIRY_DATE"] = LblExpDate.Text;
                    DR["U_QUANTITY"] = txtUQty.Text;
                    DR["U_UNIT"] = LblUUnit.Text;
                    DR["QUANTITY"] = txtQty.Text;
                    DR["UNIT"] = LblUnit.Text;
                    DR["RATE"] = txtRate.Text;
                    DR["TOTAL"] = txtAmount.Text;

                    Sd = 0;
                    try { Sd = Convert.ToDouble(txtScheDisc.Text); }
                    catch { }
                    DR["SCHE_DISC"] = Sd.ToString();
                    DR["AFTER_SCHE_DISC"] = txtAmount.Text;
                    DR["PRODUCT_RATE"] = txtRate.Text;

                    ExchangeRate = GetDisplayCurrencyRate();
                    ForeignRate = Convert.ToDouble(txtRate.Text);
                    if (ExchangeRate > 0)
                        FcRate = ForeignRate / ExchangeRate;
                    else
                        FcRate = ForeignRate;

                    DR["EXCHANGE_RATE"] = ExchangeRate.ToString("##0.0000");
                    DR["FC_RATE"] = FcRate.ToString("##0.00");
                    DR["NUMERIC_RATE"] = txtRate.Text;

                    flag = true;
                }
                else
                {
                    DR["PK_ID"] = LblProductPK_ID.Text;
                    DR["TAXABLE"] = LblTaxable.Text;
                    DR["PRODUCT_CODE"] = LblProductCode.Text;
                    DR["PRODUCT"] = LblProductName.Text;
                    DR["BATCH_NO"] = LblBatch.Text;
                    DR["EXPIRY_DATE"] = LblExpDate.Text;
                    DR["U_QUANTITY"] = LblUQty.Text;
                    DR["U_UNIT"] = LblUUnit.Text;
                    DR["QUANTITY"] = TxtGridQty.Text;
                    DR["UNIT"] = LblUnit.Text;
                    DR["RATE"] = LblRate.Text;
                    DR["TOTAL"] = LblItemTotal.Text;
                    DR["SCHE_DISC"] = LblScheDisc.Text;
                    DR["AFTER_SCHE_DISC"] = LblAfterScheDisc.Text;

                    DR["PRODUCT_RATE"] = LblRate.Text;

                    NumericRate = 0;
                    double.TryParse(LblRate.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out NumericRate);

                    DisplayExchangeRate = GetDisplayCurrencyRate();
                    if (DisplayExchangeRate > 0)
                        DisplayRate = NumericRate / DisplayExchangeRate;
                    else
                        DisplayRate = NumericRate;

                    DR["EXCHANGE_RATE"] = DisplayExchangeRate.ToString("##0.0000");
                    DR["FC_RATE"] = DisplayRate.ToString("##0.00");
                    DR["NUMERIC_RATE"] = NumericRate.ToString("##0.00");
                }

                DummyTable.Rows.Add(DR);

                if (r.RowIndex == RowIndex && addRemoveFlag == false)
                {
                    DummyTable.Rows.Remove(DR);
                    flag = true;
                }
            }
        }

        if (flag == false)
        {
            DR = DummyTable.NewRow();

            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlProduct.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);

            if (PEnt != null)
            {
                DR["PK_ID"] = PEnt.PK_ID;
                DR["TAXABLE"] = PEnt.TAX_STATUS;
                DR["PRODUCT_CODE"] = PEnt.PRODUCT_CODE;
                DR["PRODUCT"] = PEnt.PRODUCT_NAME;

                PIDEnt = new PURCHASE_INVOICE_DETAIL();
                PIDEnt.PRODUCT_ID = PEnt.PK_ID;
                PIDEnt.BATCH_NUMBER = ddlBatch.SelectedValue;
                PIDEnt = (PURCHASE_INVOICE_DETAIL)PIDSer.GetSingle(PIDEnt);
                if (PIDEnt != null)
                {
                    DR["BATCH_NO"] = PIDEnt.BATCH_NUMBER;
                    DR["EXPIRY_DATE"] = PIDEnt.EXPIRY_DATE;
                }

                DR["U_QUANTITY"] = txtUQty.Text;
                if (txtUQty.Text != "" && txtUQty.Text != "0")
                    DR["U_UNIT"] = getUnitName(PEnt.UPPER_UNIT_ID);
                DR["QUANTITY"] = txtQty.Text;
                DR["UNIT"] = getUnitName(PEnt.UNIT_ID);

                NumericRate = 0;
                Qty = 0;
                double.TryParse(txtRate.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out NumericRate);
                double.TryParse(txtQty.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out Qty);

                BillingRate = 1.0;

                if (!string.IsNullOrEmpty(ddlBillingIn.SelectedValue) &&
                    ddlBillingIn.SelectedValue != "Select" &&
                    ddlBillingIn.SelectedValue != "NPR")
                {
                    InvoiceDateEnglish = PGD.GetEnglishDateFromNepali(txtInvoiceDate.Text, "dd/mm/yyyy");

                    if (TryGetExchangeRate(ddlBillingIn.SelectedValue, InvoiceDateEnglish, out BRate, out BRateDate) && BRate > 0)
                        BillingRate = BRate;
                }

                NprRate = NumericRate * BillingRate;
                NprTotal = Qty * NprRate;

                DR["RATE"] = NprRate.ToString("##0.00");
                DR["TOTAL"] = NprTotal.ToString("##0.00");

                Sd = 0;
                try { Sd = Convert.ToDouble(txtScheDisc.Text); }
                catch { }
                DR["SCHE_DISC"] = Sd.ToString();
                AfterScheDisc = NprTotal - Sd;
                DR["AFTER_SCHE_DISC"] = AfterScheDisc.ToString("##0.00");

                DR["PRODUCT_RATE"] = NprRate.ToString("##0.00");

                DisplayExchangeRate = GetDisplayCurrencyRate();
                if (DisplayExchangeRate > 0)
                    DisplayRate = NprRate / DisplayExchangeRate;
                else
                    DisplayRate = NumericRate;

                DR["EXCHANGE_RATE"] = DisplayExchangeRate.ToString("##0.0000");
                DR["FC_RATE"] = DisplayRate.ToString("##0.00");
                DR["NUMERIC_RATE"] = NumericRate.ToString("##0.00");

                DummyTable.Rows.Add(DR);
            }
        }

        DV = new DataView(DummyTable);
        grdSalesDetail.DataSource = DummyTable;
        grdSalesDetail.DataBind();

        getTotal();
        return DummyTable;
    }

    protected void RefreshGridRates()
    {
        if (grdSalesDetail.Rows.Count == 0)
            return;

        DummyTable = new DataTable();
        DummyTable.Columns.Add("PK_ID");
        DummyTable.Columns.Add("TAXABLE");
        DummyTable.Columns.Add("PRODUCT_CODE");
        DummyTable.Columns.Add("PRODUCT");
        DummyTable.Columns.Add("BATCH_NO");
        DummyTable.Columns.Add("EXPIRY_DATE");
        DummyTable.Columns.Add("U_QUANTITY");
        DummyTable.Columns.Add("U_UNIT");
        DummyTable.Columns.Add("QUANTITY");
        DummyTable.Columns.Add("UNIT");
        DummyTable.Columns.Add("RATE");
        DummyTable.Columns.Add("TOTAL");
        DummyTable.Columns.Add("SCHE_DISC");
        DummyTable.Columns.Add("AFTER_SCHE_DISC");
        DummyTable.Columns.Add("PRODUCT_RATE");
        DummyTable.Columns.Add("EXCHANGE_RATE");
        DummyTable.Columns.Add("FC_RATE");
        DummyTable.Columns.Add("NUMERIC_RATE");

        BillingExchangeRate = 1.0;

        if (!string.IsNullOrEmpty(ddlBillingIn.SelectedValue) &&
            ddlBillingIn.SelectedValue != "Select" &&
            ddlBillingIn.SelectedValue != "NPR")
        {
            InvoiceDateEnglish = PGD.GetEnglishDateFromNepali(txtInvoiceDate.Text, "dd/mm/yyyy");

            if (TryGetExchangeRate(ddlBillingIn.SelectedValue, InvoiceDateEnglish, out BRate, out BRateDate) && BRate > 0)
                BillingExchangeRate = BRate;
        }

        DisplayExchangeRate = GetDisplayCurrencyRate();

        foreach (GridViewRow r in grdSalesDetail.Rows)
        {
            LblProductPK_ID = r.FindControl("lblProductPK_ID") as Label;
            LblTaxable = r.FindControl("lblTaxable") as Label;
            LblProductCode = r.FindControl("lblProductCode") as Label;
            LblProductName = r.FindControl("lblProductName") as Label;
            LblBatch = r.FindControl("lblBatch") as Label;
            LblExpDate = r.FindControl("lblExpDate") as Label;
            LblUQty = r.FindControl("lblUQty") as Label;
            LblUUnit = r.FindControl("lblUUnit") as Label;
            TxtGridQty = r.FindControl("txtGridQty") as TextBox;
            LblUnit = r.FindControl("lblUnit") as Label;
            LblRate = r.FindControl("lblRate") as Label;
            LblScheDisc = r.FindControl("lblScheDisc") as Label;
            LblAfterScheDisc = r.FindControl("lblAfterScheDisc") as Label;
            LblNumericRate = r.FindControl("lblNumericRate") as Label;

            NumericRate = 0;
            Qty = 0;
            ScheDisc = 0;

            if (TxtGridQty != null)
                double.TryParse(TxtGridQty.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out Qty);

            if (LblScheDisc != null)
                double.TryParse(LblScheDisc.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out ScheDisc);

            if (LblNumericRate != null)
                double.TryParse(LblNumericRate.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out NumericRate);

            if (NumericRate == 0 && LblRate != null)
            {
                double.TryParse(LblRate.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out OldNprRate);

                if (OldNprRate > 0)
                    NumericRate = OldNprRate;
            }

            NprRate = NumericRate * BillingExchangeRate;
            NprTotal = Qty * NprRate;
            AfterScheDisc = NprTotal - ScheDisc;

            if (DisplayExchangeRate > 0)
                DisplayRate = NprRate / DisplayExchangeRate;
            else
                DisplayRate = NumericRate;

            DR = DummyTable.NewRow();
            DR["PK_ID"] = LblProductPK_ID.Text;
            DR["TAXABLE"] = LblTaxable.Text;
            DR["PRODUCT_CODE"] = LblProductCode.Text;
            DR["PRODUCT"] = LblProductName.Text;
            DR["BATCH_NO"] = LblBatch.Text;
            DR["EXPIRY_DATE"] = LblExpDate.Text;
            DR["U_QUANTITY"] = LblUQty.Text;
            DR["U_UNIT"] = LblUUnit.Text;
            DR["QUANTITY"] = Qty.ToString();
            DR["UNIT"] = LblUnit.Text;
            DR["RATE"] = NprRate.ToString("##0.00");
            DR["TOTAL"] = NprTotal.ToString("##0.00");
            DR["SCHE_DISC"] = ScheDisc.ToString("##0.00");
            DR["AFTER_SCHE_DISC"] = AfterScheDisc.ToString("##0.00");
            DR["PRODUCT_RATE"] = NprRate.ToString("##0.00");
            DR["EXCHANGE_RATE"] = DisplayExchangeRate.ToString("##0.0000");
            DR["FC_RATE"] = DisplayRate.ToString("##0.00");
            DR["NUMERIC_RATE"] = NumericRate.ToString("##0.00");

            DummyTable.Rows.Add(DR);
        }

        grdSalesDetail.DataSource = DummyTable;
        grdSalesDetail.DataBind();
        getTotal();
    }

    protected void grdSalesDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            GRow = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            CreateGrid(GRow.RowIndex, false);
        }
        else if (e.CommandName.Equals("EditGridPack"))
        {
            GRow = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;

            LblProductCode = GRow.FindControl("lblProductCode") as Label;
            TxtGridQty = GRow.FindControl("txtGridQty") as TextBox;

            if (LblProductCode != null)
                hdnEditPackProductCode.Value = LblProductCode.Text;

            if (TxtGridQty != null)
                hdnEditPackQty.Value = TxtGridQty.Text;

            hdnProductCode.Value = hdnEditPackProductCode.Value;
            hdnPackMode.Value = "EDIT";
            hdnEditPackPK.Value = "";

            BindPackGrid();
            OpenPackPopup(editMode: true);
        }
    }

    private void OpenPackPopup(bool editMode)
    {
        btnPopup_ModalPopupExtender.Show();

        Remaining = RecalculatePackAvailability(hdnEditPackPK.Value);

        if (!editMode)
        {
            txtPckQTY.Text = "";
            txtPack.Text = "";
        }

        txtPckQTY.ReadOnly = false;
        txtPckQTY.Enabled = true;
        txtPack.ReadOnly = false;
        txtPack.Enabled = true;

        BindPackGrid();
    }

    protected void grdSalesDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DRV = e.Row.DataItem as DataRowView;
            if (DRV == null) return;

            TxtGridQty = e.Row.FindControl("txtGridQty") as TextBox;
            LblRate = e.Row.FindControl("lblRate") as Label;
            LblItemTotal = e.Row.FindControl("lblItemTotal") as Label;
            LblFCRate = e.Row.FindControl("lblFCRate") as Label;
            LblFCTotal = e.Row.FindControl("lblFCTotal") as Label;

            NprRate = 0;
            NprTotal = 0;
            DisplayRate = 0;
            NumericRate = 0;

            double.TryParse(DRV["RATE"].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out NprRate);
            double.TryParse(DRV["TOTAL"].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out NprTotal);
            double.TryParse(DRV["FC_RATE"].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out DisplayRate);
            double.TryParse(DRV["NUMERIC_RATE"].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out NumericRate);

            Qty = 0;
            double.TryParse(TxtGridQty.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out Qty);

            if (Qty == 0)
                double.TryParse(DRV["QUANTITY"].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out Qty);

            DisplayTotal = Qty * DisplayRate;
            CurrencyId = "";
            
            if (!string.IsNullOrEmpty(ddlPaymentCurrency.SelectedValue) &&
                ddlPaymentCurrency.SelectedValue != "Select")
            {
                CurrencyId = ddlPaymentCurrency.SelectedValue;
            }
            else if (!string.IsNullOrEmpty(ddlBillingIn.SelectedValue) &&
                     ddlBillingIn.SelectedValue != "Select" &&
                     ddlBillingIn.SelectedValue != "NPR")
            {
                CurrencyId = ddlBillingIn.SelectedValue;
            }

            DisplayCurrencyCode = "";

            if (!string.IsNullOrEmpty(CurrencyId))
            {
                FCEnt = new FOREIGN_CURRENCY();
                FCEnt.PK_ID = CurrencyId;
                FCEnt = (FOREIGN_CURRENCY)FCSer.GetSingle(FCEnt);

                if (FCEnt != null && !string.IsNullOrEmpty(FCEnt.CURRENCY_CODE))
                    DisplayCurrencyCode = FCEnt.CURRENCY_CODE;
            }

            FcRateText = DisplayRate.ToString("##0.00");
            FcTotalText = DisplayTotal.ToString("##0.00");

            if (!string.IsNullOrEmpty(DisplayCurrencyCode))
            {
                FcRateText = DisplayCurrencyCode + " " + FcRateText;
                FcTotalText = DisplayCurrencyCode + " " + FcTotalText;
            }

            LblRate.Text = NprRate.ToString("##0.00");
            LblItemTotal.Text = NprTotal.ToString("##0.00");
            LblFCRate.Text = FcRateText;
            LblFCTotal.Text = FcTotalText;
        }
    }

    protected void txtGridQty_TextChanged(object sender, EventArgs e)
    {
        RefreshGridRates();
    }

    protected void getTotal()
    {
        Total = 0;
        ScheDisc = 0;
        DiscountPercent = 0;
        Discount = 0;
        BeforeDecimal = 0;
        AfterDecimal = 0;

        foreach (GridViewRow r in grdSalesDetail.Rows)
        {
            LblItemTotal = r.FindControl("lblItemTotal") as Label;
            LblScheDisc = r.FindControl("lblScheDisc") as Label;
            Total = Total + (Convert.ToDouble(LblItemTotal.Text));
            try
            {
                ScheDisc = ScheDisc + (Convert.ToDouble(LblScheDisc.Text));
            }
            catch { }
        }

        try
        {
            DiscountPercent = Convert.ToDouble(txtDiscount.Text);
        }
        catch
        {
            txtDiscount.Text = "0";
        }

        if (DiscountPercent > 0)
        {
            txtDiscountAmount.Text = ((Total * Convert.ToDouble(txtDiscount.Text)) / 100).ToString("#0.00");
            Discount = Convert.ToDouble(txtDiscountAmount.Text);
        }
        else
        {
            try
            {
                Discount = Convert.ToDouble(txtDiscountAmount.Text);
            }
            catch
            {
                txtDiscountAmount.Text = "0";
            }
        }

        lblSubTotalAmount.Text = Total.ToString("#0.00");
        lblTotalScheDisc.Text = ScheDisc.ToString("#0.00");
        lblAfterSchDiscount.Text = (Total - ScheDisc).ToString("#0.00");
        lblTotalAmount.Text = (Total - ScheDisc - Discount).ToString("#0.00");

        Vat = Convert.ToDouble(txtVATPercent.Text) / 100;
        lblVAT.Text = ((Total - ScheDisc - Discount) * Vat).ToString("#0.00");

        PreReturnTotal = Convert.ToDouble(lblVAT.Text) + Convert.ToDouble(lblTotalAmount.Text);

        VatReturn = 0;
        if (ddlPaymentType.SelectedValue == "QR")
        {
            VatReturn = Convert.ToDouble(lblVAT.Text) * 0.10;
            lblVATReturn.Text = VatReturn.ToString("#0.00");
            trVATReturn.Visible = true;
        }
        else
        {
            lblVATReturn.Text = "0.00";
            trVATReturn.Visible = false;
        }

        lblGrandTotal.Text = (PreReturnTotal - VatReturn).ToString("#0.00");
        GrandTotalForRound = Convert.ToDouble(lblGrandTotal.Text);

        if (PGPS.RoundOff() == true)
        {
            BeforeDecimal = Math.Truncate(GrandTotalForRound);
            AfterDecimal = GrandTotalForRound - BeforeDecimal;

            if (AfterDecimal == 0)
            {
                txtRound.Text = (AfterDecimal).ToString("0.00");
                lblInvoiceAmount.Text = BeforeDecimal.ToString("0.00");
            }
            else if (AfterDecimal >= 0.5)
            {
                BeforeDecimal = BeforeDecimal + 1;
                txtRound.Text = (1 - AfterDecimal).ToString("0.00");
                lblInvoiceAmount.Text = BeforeDecimal.ToString("0.00");
            }
            else
            {
                txtRound.Text = "-" + (AfterDecimal).ToString("0.00");
                lblInvoiceAmount.Text = BeforeDecimal.ToString("0.00");
            }
        }
        else
        {
            txtRound.Text = "0.00";
            lblInvoiceAmount.Text = GrandTotalForRound.ToString("#0.00");
        }
    }

    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnAdd.Visible = true;
        PEnt = new PRODUCT();

        if (ddlProduct.SelectedValue != "Select")
        {
            PEnt.PK_ID = ddlProduct.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                txtProductCode.Text = PEnt.PRODUCT_CODE;
                LoadBatch(txtProductCode.Text);
                txtQty.Text = "1";

                string batch = null;
                if (ddlBatch.SelectedValue != "")
                {
                    batch = ddlBatch.SelectedValue;
                }

                LoadAvailablity(txtProductCode.Text, batch);
                loadRate();

                if (PEnt.DUAL_UNIT == "1")
                {
                    txtUQty.Enabled = true;
                    txtUQty.Focus();
                    lblUUnit.Text = getUnitName(PEnt.UPPER_UNIT_ID);
                }
                else
                {
                    txtUQty.Enabled = false;
                    txtQty.Focus();
                    txtUQty.Text = "";
                    lblUUnit.Text = "";
                }

                lblBUnit.Text = getUnitName(PEnt.UNIT_ID);
                LoadRateType();
            }
        }
        else
        {
            txtProductCode.Text = "";
            txtProductCode.Focus();
            lblUUnit.Text = "";
            lblBUnit.Text = "";
        }
    }

    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        txtProductCode.Text = txtProductCode.Text.ToUpper();

        if (txtProductCode.Text != "")
        {
            PEnt = new PRODUCT();
            PEnt.PRODUCT_CODE = txtProductCode.Text;
            PEnt.OFFICE_CODE = userProfile.LocationID;
            PEnt.TAX_STATUS = ddlExempted.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);

            if (PEnt != null)
            {
                ddlProduct.SelectedValue = PEnt.PK_ID;
                LoadBatch(txtProductCode.Text);

                string batch = null;
                if (ddlBatch.SelectedValue != "")
                {
                    batch = ddlBatch.SelectedValue;
                }

                LoadAvailablity(txtProductCode.Text, batch);

                if (PEnt.DUAL_UNIT == "1")
                {
                    txtUQty.Enabled = true;
                    txtUQty.Focus();
                    lblUUnit.Text = getUnitName(PEnt.UPPER_UNIT_ID);
                }
                else
                {
                    txtUQty.Enabled = false;
                    txtUQty.Text = "";
                    txtQty.Focus();
                    lblUUnit.Text = "";
                }

                lblBUnit.Text = getUnitName(PEnt.UNIT_ID);
            }
            else
            {
                txtProductCode.Text = "";
                txtProductCode.Focus();
                ddlProduct.SelectedValue = "Select";
                lblUUnit.Text = "";
                lblBUnit.Text = "";
                HelperFunction.MsgBox(this, this.GetType(), "Invalid Product Code.");
            }
        }
        else
        {
            txtProductCode.Text = "";
            txtProductCode.Focus();
            ddlProduct.SelectedValue = "Select";
            lblUUnit.Text = "";
            lblBUnit.Text = "";
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        string batch = null;
        if (ddlBatch.SelectedValue != "")
            batch = ddlBatch.SelectedValue;

        LoadAvailablity(txtProductCode.Text, batch);
    }

    protected void ddlProductRateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadRate();
        LoadRateType();
    }

    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        LoadQty();
    }

    protected void txtUQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlProduct.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);

            if (PEnt != null)
            {
                txtQty.Text = (Convert.ToDouble(txtUQty.Text) * Convert.ToDouble(PEnt.PACK_QTY)).ToString();
                txtRate.Focus();

                if (string.IsNullOrEmpty(txtAvilableQty.Text))
                {
                    txtAvilableQty.Text = "0";
                }

                if (Convert.ToDouble(txtAvilableQty.Text) - Convert.ToDouble(txtQty.Text) < 0)
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Avilable qty is less then sales qty. Are you sure to make invoice?");
                }
            }
        }
        catch
        {
            txtUQty.Text = "0";
            txtUQty.Focus();
            HelperFunction.MsgBox(this, this.GetType(), "Enter Number Only. ");
        }
    }

    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        Qty = 0;
        Rate = 0;

        try
        {
            Qty = Convert.ToDouble(txtQty.Text);
        }
        catch
        {
            txtQty.Focus();
            txtQty.Text = "";
        }

        try
        {
            Rate = Convert.ToDouble(txtRate.Text);
            txtRate.Text = Convert.ToDouble(txtRate.Text).ToString("#0.00");
            txtScheDisc.Focus();
        }
        catch
        {
            txtRate.Focus();
            txtRate.Text = "";
            HelperFunction.MsgBox(this, this.GetType(), "Enter Number Only. ");
            return;
        }

        txtAmount.Text = (Qty * Rate).ToString("#0.00");
    }

    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        Qty = 0;
        Amount = 0;

        try
        {
            Qty = Convert.ToDouble(txtQty.Text);
        }
        catch
        {
            txtQty.Focus();
            txtQty.Text = "";
        }

        try
        {
            Amount = Convert.ToDouble(txtAmount.Text);
            txtAmount.Text = Convert.ToDouble(txtAmount.Text).ToString("#0.00");
        }
        catch
        {
            txtAmount.Focus();
            txtAmount.Text = "";
            HelperFunction.MsgBox(this, this.GetType(), "Enter Number Only. ");
            return;
        }

        txtRate.Text = (Amount / Qty).ToString("#0.00");
    }

    protected void txtDiscount_TextChanged(object sender, EventArgs e)
    {
        getTotal();
    }

    protected void txtDiscountAmount_TextChanged(object sender, EventArgs e)
    {
        txtDiscount.Text = "0";
        getTotal();
    }

    protected void txtVATPercent_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double vatper = Convert.ToDouble(txtVATPercent.Text) / 100;
            lblVAT.Text = (vatper * Convert.ToDouble(lblTotalAmount.Text)).ToString("0.00");
        }
        catch
        {
            txtVATPercent.Focus();
            txtVATPercent.Text = PG.CompanyTAXPercent();
            HelperFunction.MsgBox(this, this.GetType(), "Enter Number Only. ");
        }
    }

    protected void txtRound_TextChanged(object sender, EventArgs e)
    {
        RoundOff = 0;
        try
        {
            RoundOff = Convert.ToDouble(txtRound.Text);
        }
        catch
        {
            txtRound.Text = "0.00";
        }
        lblInvoiceAmount.Text = (Convert.ToDouble(lblGrandTotal.Text) + RoundOff).ToString("#0.00");
    }

    protected void ddlExempted_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlExempted.SelectedValue == "1")
        {
            txtVATPercent.Text = PG.CompanyTAXPercent();
        }
        else
        {
            txtVATPercent.Text = "0";
        }

        LoadProduct();
        CreateGridFirst();
        getTotal();
        txtProductCode.Text = "";
    }

    protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPaymentType.SelectedValue == "CR")
        {
            divBankDetail.Visible = false;
            divCredit.Visible = true;
        }
        else if (ddlPaymentType.SelectedValue == "QR")
        {
            divBankDetail.Visible = true;
            divCredit.Visible = false;
        }
        else
        {
            divBankDetail.Visible = false;
            divCredit.Visible = false;
        }

        if (ddlPaymentType.SelectedValue == "AP")
        {
            tdAdvAmount.Visible = true;
        }
        else
        {
            tdAdvAmount.Visible = false;
        }

        getTotal();
    }

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCustomer.SelectedValue != "Select")
        {
            CEnt = new CUSTOMER();
            CEnt.PK_ID = ddlCustomer.SelectedValue;
            CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
            if (CEnt != null)
            {
                txtCustomerCode.Text = CEnt.CUSTOMER_CODE;
                txtCustomerAddress.Text = CEnt.ADDRESS;
                txtCustomerPANVAT.Text = CEnt.VAT_PAN_NUMBER;
                txtCustomerContact.Text = CEnt.MOBILE;
            }
        }
        else
        {
            Clear();
        }
    }

    protected void txtCustomerCode_TextChanged(object sender, EventArgs e)
    {
        txtCustomerCode.Text = txtCustomerCode.Text.ToUpper();
        CEnt = new CUSTOMER();
        CEnt.CUSTOMER_CODE = txtCustomerCode.Text;
        CEnt = (CUSTOMER)CSer.GetSingle(CEnt);

        if (CEnt != null && !string.IsNullOrEmpty(txtCustomerCode.Text))
        {
            ddlCustomer.SelectedValue = CEnt.PK_ID;
            txtCustomerAddress.Text = CEnt.ADDRESS;
            txtCustomerPANVAT.Text = CEnt.VAT_PAN_NUMBER;
            txtCustomerContact.Text = CEnt.MOBILE;
        }
        else
        {
            ddlCustomer.SelectedValue = "Select";
            txtCustomerCode.Text = "";
            txtCustomerCode.Focus();
            HelperFunction.MsgBox(this, this.GetType(), "Not a Valid Customer Code.");
        }
    }

    protected void txtCustomerPANVAT_TextChanged(object sender, EventArgs e)
    {
        Boolean PAN_Valid = true;
        if (!string.IsNullOrEmpty(txtCustomerPANVAT.Text))
        {
            try
            {
                Double num = Convert.ToDouble(txtCustomerPANVAT.Text);
                if (txtCustomerPANVAT.Text.Length != 9)
                {
                    PAN_Valid = false;
                }
            }
            catch
            {
                PAN_Valid = false;
            }

            if (!PAN_Valid)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Not a valid PAN/VAT Number.");
                txtCustomerPANVAT.Text = "0";
                txtCustomerPANVAT.Focus();
            }
        }
    }

    protected void ddlPaymentCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetVisibilty();
        UpdateBillingExchangeRateInfo();
        CheckTodaysExchangeRate();
        RefreshGridRates();
    }

    protected void ddlBillingIn_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetVisibilty();

        if (ddlBillingIn.SelectedItem.Text.Trim() == "NPR")
            tdPaymentCurrency.Visible = true;
        else
            tdPaymentCurrency.Visible = false;

        UpdateBillingExchangeRateInfo();
        RefreshGridRates();
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Qty = 0;
        Rate = 0;

        if (!double.TryParse(txtQty.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out Qty) || Qty <= 0)
        {
            txtQty.Focus();
            txtQty.Text = "";
            HelperFunction.MsgBox(this, this.GetType(), "Enter a valid quantity.");
            return;
        }

        if (!double.TryParse(txtRate.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out Rate) || Rate <= 0)
        {
            txtRate.Focus();
            txtRate.Text = "";
            HelperFunction.MsgBox(this, this.GetType(), "Enter a valid rate.");
            return;
        }

        if (ddlProduct.SelectedItem.ToString() == "Select")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Select a product.");
            return;
        }

        CreateGrid(1, true);
        ClearProduct();
    }

    protected void ClearProduct()
    {
        txtProductCode.Text = "";
        ddlProduct.SelectedIndex = 0;
        txtUQty.Text = "";
        txtQty.Text = "";
        lblUUnit.Text = "";
        lblBUnit.Text = "";
        txtRate.Text = "";
        txtAmount.Text = "";
        txtScheDisc.Text = "";
        txtProductCode.Focus();
    }

    protected void Clear()
    {
        ddlCustomer.SelectedIndex = 0;
        txtCustomerCode.Text = "";
        txtCustomerAddress.Text = "";
        txtCustomerPANVAT.Text = "0";
        txtCustomerContact.Text = "";
        txtDiscountAmount.Text = "";
        txtDiscount.Text = "";
        ddlPaymentType.SelectedValue = "CR";
        divBankDetail.Visible = false;
        lblSubTotalAmount.Text = "0.00";
        lblTotalAmount.Text = "0.00";
        lblVAT.Text = "0.00";
        lblGrandTotal.Text = "0.00";
        txtRound.Text = "0.00";
        lblInvoiceAmount.Text = "0.00";
        txtTransactionDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();

        divExistingCustomer.Visible = true;
        txtCustomerCode.Focus();
        ddlPaymentType.Enabled = true;
        divCredit.Visible = false;
        CreateGridFirst();
        //txtRemarks.Text = "";
        ClearProduct();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        UniqueToken = "";
        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.NEXT_INVOIICE_ID = lblunique_token.Text;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);

        if (SIMEnt != null)
            UniqueToken = SIMEnt.NEXT_INVOIICE_ID;

        if (UniqueToken != "")
        {
            HelperFunction.MsgBox(this, this.GetType(), "This bill have been already saved.");
            return;
        }

        if (txtCustomerCode.Text == "")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Select Customer.");
            return;
        }

        if (!(grdSalesDetail.Rows.Count > 0))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Atleast 1 product must be added to save the sales.");
            return;
        }
        if (ddlBillingIn.SelectedValue == "Select" || string.IsNullOrEmpty(ddlBillingIn.SelectedValue))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Select the Product Price in");
            return;
        }

        bool billingIsNPR = (ddlBillingIn.SelectedItem != null &&
                             ddlBillingIn.SelectedItem.Text.Trim() == "NPR");

        if (billingIsNPR &&
            (ddlPaymentCurrency.SelectedValue == "Select" || string.IsNullOrEmpty(ddlPaymentCurrency.SelectedValue)))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Select the Billing in Currency");
            return;
        }

        if(ddlPaymentType.SelectedValue =="Select")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Select the Mode of Payment");
            return;
        }
        string resolvedCurrencyId;
        string resolvedCurrencyName;

        if (!billingIsNPR)
        {
            resolvedCurrencyId = ddlBillingIn.SelectedValue;
            resolvedCurrencyName = ddlBillingIn.SelectedItem.Text;
        }
        else
        {
            resolvedCurrencyId = ddlPaymentCurrency.SelectedValue;
            resolvedCurrencyName = ddlPaymentCurrency.SelectedItem.Text;
        }

        ResolvedExchangeRate = 0;
        ResolvedRateDate = DateTime.MinValue;

        InvoiceDateEnglish = PGD.GetEnglishDateFromNepali(txtInvoiceDate.Text, "dd/mm/yyyy");
        if (!TryGetExchangeRate(resolvedCurrencyId, InvoiceDateEnglish, out ResolvedExchangeRate, out ResolvedRateDate))
        {
            HelperFunction.MsgBox(this, this.GetType(),
                "No exchange rate found for " + resolvedCurrencyName +
                " on the invoice date. Please add one in CURRENCY_RATE for that exact date before saving.");
            return;
        }

        DistributedTransaction DT = new DistributedTransaction();
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

        VoucherPkId = "";
        SalesPkId = "";
        ExportPkId = "";

        #region Insert into Sales Invoice Master

        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.INVOICE_DATE = PGD.GetEnglishDateFromNepali(txtInvoiceDate.Text, "dd/mm/yyyy");
        SIMEnt.INVOICE_DAY = PGD.NepaliDay();
        SIMEnt.INVOICE_MONTH = PGD.NepaliMonth();
        SIMEnt.INVOICE_YEAR = PGD.NepaliYear();
        SIMEnt.INVOICE_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());

        string[] tDate = txtTransactionDate.Text.Split('/');
        SIMEnt.TRANSACTION_DATE = PGD.GetEnglishDateFromNepali(txtTransactionDate.Text, "dd/mm/yyyy");
        SIMEnt.TRANSACTION_DAY = tDate[0];
        SIMEnt.TRANSACTION_MONTH = tDate[1];
        SIMEnt.TRANSACTION_YEAR = tDate[2];
        SIMEnt.TRANSACTION_FY = PGD.checkFiscalYear(tDate[1], tDate[2]);

        SIMEnt.CUSTOMER_ID = ddlCustomer.SelectedValue;
        SIMEnt.CUSTOMER_NAME = getCustomerName(ddlCustomer.SelectedValue);
        SIMEnt.CUSTOMER_ADDRESS = txtCustomerAddress.Text;
        SIMEnt.COSTOMER_PAN_VAT = txtCustomerPANVAT.Text;
        SIMEnt.CUSTOMER_CONTACT_NUMBER = txtCustomerContact.Text;
        SIMEnt.INVOICE_TIME = DateTime.Now.ToString("HH:mm:ss");
        SIMEnt.USER_ID = userProfile.EmployeeID;
        SIMEnt.TAXABLE_SUB_TOTAL = Convert.ToDouble(lblAfterSchDiscount.Text).ToString("##0.00");
        SIMEnt.DISCOUNT_PERCENT = txtDiscount.Text;

        if (ddlExempted.SelectedValue == "1")
            SIMEnt.EXEMPTED = "0";
        else
            SIMEnt.EXEMPTED = "1";

        SIMEnt.TAXABLE_DISC_AMOUNT = Convert.ToDouble(txtDiscountAmount.Text).ToString("##0.00");
        SIMEnt.TAXABLE_TOTAL = Convert.ToDouble(lblTotalAmount.Text).ToString("##0.00");
        SIMEnt.TAX_VAT_AMOUNT = Convert.ToDouble(lblVAT.Text).ToString("##0.00");
        SIMEnt.GRAND_TOTAL = Convert.ToDouble(lblGrandTotal.Text).ToString("##0.00");
        SIMEnt.ROUND_OFF = txtRound.Text;
        SIMEnt.INVOICE_AMOUNT = Convert.ToDouble(lblInvoiceAmount.Text).ToString("##0.00");

        VatReturnAmount = 0;
        try { VatReturnAmount = Convert.ToDouble(lblVATReturn.Text); }
        catch { }

        if (VatReturnAmount > 0)
        {
            SIMEnt.VAT_RETURN = VatReturnAmount.ToString("##0.00");
        }

        SIMEnt.IS_PRINTED = "1";
        SIMEnt.COPY_PRINTNO = "0";
        SIMEnt.CANCEL_STATUS = "0";
        SIMEnt.SALES_TYPE_ID = ddlPaymentType.SelectedValue;

        if (ddlPaymentType.SelectedValue == "QR")
        {
            SIMEnt.TRANSACTION_ID = Session["PS_QR_TransactionId"] as string;
            SIMEnt.QR_TYPE = Session["PS_QR_Type"] as string;
            if (SIMEnt.QR_TYPE == null)
                SIMEnt.QR_TYPE = "NepalPay";
        }

        SIMEnt.NEXT_INVOIICE_ID = lblunique_token.Text;
        SIMEnt.PO_NUMBER = txtContractNo.Text;
        SIMEnt.REMARKS = txtRemarks.Text;
        SIMEnt.OFFICE_CODE = userProfile.LocationID;
        SIMEnt.PRINT_BY = hf.getEmployeeName(userProfile.EmployeeID);
        SIMEnt.PRINT_TIME = System.DateTime.Now.ToString();

        try
        {
            SalesPkId = SIMSer.Insert(SIMEnt, DT).ToString();
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, this.GetType(), "DEBUG INSERT ERROR (Sales Invoice): " + ex.Message);
            return;
        }

        #endregion

        #region Get generated sales invoice number

        GeneratedSalesInvoiceNumber = "";
        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.PK_ID = SalesPkId;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt, DT);
        if (SIMEnt != null)
        {
            GeneratedSalesInvoiceNumber = SIMEnt.INVOICE_NUMBER;
        }

        #endregion

        #region Update ORDER_NUMBER

        TodayEng = PGD.GetTodayDate("dd/mm/yyyy");

        ONEnt = new ORDER_NUMBER();
        ONEnt.OFFICE_CODE = userProfile.LocationID;
        ONEnt.ORD_DATE = TodayEng;
        ONEnt = (ORDER_NUMBER)ONSer.GetSingle(ONEnt, DT);

        if (ONEnt != null)
        {
            NextOrder = Convert.ToInt32(ONEnt.ORDER_NO) + 1;
            ONEnt.ORDER_NO = NextOrder.ToString();
            ONSer.Update(ONEnt, DT);
        }
        else
        {
            ONEnt = new ORDER_NUMBER();
            ONEnt.OFFICE_CODE = userProfile.LocationID;
            ONEnt.ORD_DATE = TodayEng;
            ONSer.Insert(ONEnt, DT);
        }

        #endregion

        #region Insert into Sales Invoice Detail

        foreach (GridViewRow r in grdSalesDetail.Rows)
        {
            LblSno = (Label)r.FindControl("lblSno");
            LblTaxable = (Label)r.FindControl("lblTaxable");
            LblProductPK_ID = (Label)r.FindControl("lblProductPK_ID");
            LblUQty = (Label)r.FindControl("lblUQty");
            TxtGridQty = (TextBox)r.FindControl("txtGridQty");
            LblRate = (Label)r.FindControl("lblRate");
            LblItemTotal = (Label)r.FindControl("lblItemTotal");
            LblScheDisc = (Label)r.FindControl("lblScheDisc");
            LblAfterScheDisc = (Label)r.FindControl("lblAfterScheDisc");
            LblBatch = (Label)r.FindControl("lblBatch");
            LblExpDate = (Label)r.FindControl("lblExpDate");

            SIDEnt = new SALES_INVOICE_DETAIL();
            SIDEnt.SALES_INVOICE_ID = SalesPkId;
            SIDEnt.SNO = LblSno.Text;
            SIDEnt.PRODUCT_ID = LblProductPK_ID.Text;
            SIDEnt.UPPER_QUANTITY = LblUQty.Text;
            SIDEnt.QUANTITY = TxtGridQty.Text;
            SIDEnt.RATE = LblRate.Text;
            SIDEnt.TOTAL = LblItemTotal.Text;
            SIDEnt.SCHEME_DISCOUNT = LblScheDisc.Text;

            if (LblTaxable.Text == "1")
            {
                SIDEnt.TAXABLE_TOTAL = (Convert.ToDouble(LblAfterScheDisc.Text)).ToString();
                SIDEnt.NON_TAXABLE_TOTAL = "0";
                SIDEnt.TAX_AMOUNT = ((Convert.ToDouble(LblAfterScheDisc.Text)) * Convert.ToDouble(PG.CompanyTAXPercent()) / 100).ToString();
                SIDEnt.AMOUNT_AFTER_TAX = (Convert.ToDouble(LblAfterScheDisc.Text) + (Convert.ToDouble(LblAfterScheDisc.Text)) * Convert.ToDouble(PG.CompanyTAXPercent()) / 100).ToString();
            }
            else
            {
                SIDEnt.TAXABLE_TOTAL = "0";
                SIDEnt.NON_TAXABLE_TOTAL = (Convert.ToDouble(LblAfterScheDisc.Text)).ToString();
                SIDEnt.TAX_AMOUNT = "0";
                SIDEnt.AMOUNT_AFTER_TAX = (Convert.ToDouble(LblAfterScheDisc.Text)).ToString();
            }

            SIDEnt.EXPIRY_DATE = LblExpDate.Text;
            SIDEnt.BATCH_NO = LblBatch.Text;
            SIDEnt.OFFICE_CODE = userProfile.LocationID;
            SIDSer.Insert(SIDEnt, DT);
        }

        #endregion

        #region Insert into Export Invoice Master

        LocalSubTotal = Convert.ToDouble(lblSubTotalAmount.Text);
        LocalDiscountAmount = 0;
        try { LocalDiscountAmount = Convert.ToDouble(txtDiscountAmount.Text); }
        catch { }

        LocalAdvanceAmount = 0;
        try { LocalAdvanceAmount = Convert.ToDouble(txtAdvanceAmt.Text); }
        catch { }

        ExportAmount = 0;
        if (ResolvedExchangeRate > 0)
            ExportAmount = LocalSubTotal / ResolvedExchangeRate;

        ExportDiscountAmount = 0;
        if (ResolvedExchangeRate > 0)
            ExportDiscountAmount = LocalDiscountAmount / ResolvedExchangeRate;

        ExportTotalAmount = ExportAmount - ExportDiscountAmount;

        ExportAdvanceAmount = 0;
        if (ResolvedExchangeRate > 0)
            ExportAdvanceAmount = LocalAdvanceAmount / ResolvedExchangeRate;

        ExportFinalAmount = ExportTotalAmount - ExportAdvanceAmount;

        EIMEnt = new EXPORT_INVOICE_MASTER();
        EIMEnt.INVOICE_DATE = PGD.GetEnglishDateFromNepali(txtInvoiceDate.Text, "dd/mm/yyyy");
        EIMEnt.INVOICE_DAY = PGD.NepaliDay();
        EIMEnt.INVOICE_MONTH = PGD.NepaliMonth();
        EIMEnt.INVOICE_YEAR = PGD.NepaliYear();
        EIMEnt.INVOICE_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());

        EIMEnt.INVOICE_MASTER_ID = SalesPkId;
        EIMEnt.INVOICE_NUMBER = GeneratedSalesInvoiceNumber;

        EIMEnt.CONTRACT_NUMBER = txtContractNo.Text;
        EIMEnt.CONTRACT_DATE = PGD.GetEnglishDateFromNepali(txtContractDate.Text, "dd/mm/yyyy");
        EIMEnt.CUSTOMER_ID = ddlCustomer.SelectedValue;
        EIMEnt.AMOUNT = ExportAmount.ToString("##0.00");
        EIMEnt.DISCOUNT_AMOUNT = ExportDiscountAmount.ToString("##0.00");
        EIMEnt.TOTAL_AMOUNT = ExportTotalAmount.ToString("##0.00");
        EIMEnt.ADVANCE_AMOUNT = ExportAdvanceAmount.ToString("##0.00");
        EIMEnt.FINAL_AMOUNT = ExportFinalAmount.ToString("##0.00");
        EIMEnt.CURRENCY_TYPE_ID = resolvedCurrencyId;
        EIMEnt.MODE_OF_PAYMENT = ddlPaymentType.SelectedValue;
        EIMEnt.REMARKS = txtRemarksE.Text;
        EIMEnt.OFFICE_CODE = userProfile.LocationID;
        EIMEnt.STATUS = "1";
        EIMEnt.SHIPMENT_TYPE = txtShipmentType.Text;
        EIMEnt.SHIPMENT_NUMBER = txtShipmentNo.Text;
        EIMEnt.USER_ID = userProfile.EmployeeID;

        try
        {
            ExportPkId = EIMSer.Insert(EIMEnt, DT).ToString();
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, this.GetType(), "DEBUG INSERT ERROR (Export Invoice): " + ex.Message);
            return;
        }

        #endregion

        #region Insert into Export Invoice Detail

        LineExchangeRate = ResolvedExchangeRate;

        foreach (GridViewRow r in grdSalesDetail.Rows)
        {
            LblSno = (Label)r.FindControl("lblSno");
            LblProductPK_ID = (Label)r.FindControl("lblProductPK_ID");
            TxtGridQty = (TextBox)r.FindControl("txtGridQty");
            LblRate = (Label)r.FindControl("lblRate");
            LblItemTotal = (Label)r.FindControl("lblItemTotal");
            LblBatch = (Label)r.FindControl("lblBatch");
            LblExpDate = (Label)r.FindControl("lblExpDate");

            LocalRate = 0;
            try { LocalRate = Convert.ToDouble(LblRate.Text); }
            catch { }

            LocalTotal = 0;
            try { LocalTotal = Convert.ToDouble(LblItemTotal.Text); }
            catch { }

            ConvertedRate = 0;
            if (LineExchangeRate > 0)
                ConvertedRate = LocalRate / LineExchangeRate;

            ConvertedTotal = 0;
            if (LineExchangeRate > 0)
                ConvertedTotal = LocalTotal / LineExchangeRate;

            EIDEnt = new EXPORT_INVOICE_DETAIL();
            EIDEnt.EXPORT_MASTER_ID = ExportPkId;
            EIDEnt.SNO = LblSno.Text;
            EIDEnt.PRODUCT_ID = LblProductPK_ID.Text;
            EIDEnt.QTY = TxtGridQty.Text;
            EIDEnt.RATE = ConvertedRate.ToString("##0.00");
            EIDEnt.TOTAL = ConvertedTotal.ToString("##0.00");
            EIDEnt.EXPIRY_DATE = LblExpDate.Text;
            EIDEnt.BATCH_NUMBER = LblBatch.Text;
            EIDEnt.OFFICE_CODE = userProfile.LocationID;
            EIDSer.Insert(EIDEnt, DT);
        }

        #endregion

        #region Account portion

        SalesInvoiceNumber = "";
        ExportInvoiceNumber = "";

        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.PK_ID = SalesPkId;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt, DT);
        if (SIMEnt != null)
        {
            SalesInvoiceNumber = SIMEnt.INVOICE_NUMBER;
        }

        EIMEnt = new EXPORT_INVOICE_MASTER();
        EIMEnt.PK_ID = ExportPkId;
        EIMEnt = (EXPORT_INVOICE_MASTER)EIMSer.GetSingle(EIMEnt, DT);
        if (EIMEnt != null)
        {
            ExportInvoiceNumber = EIMEnt.INVOICE_NUMBER;
        }

        InvoiceNumber = SalesInvoiceNumber;

        #region Voucher Master as JV for party

        VMEnt = new VOUCHER_MASTER();
        VMEnt.VOUCHER_TYPE = "JV";
        VMEnt.VOUCHER_NUMBER = af.getNext_VM_ID("JV", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfile.LocationID);
        VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
        VMEnt.VOUCHER_DAY = PGD.NepaliDay();
        VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
        VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
        VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
        VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
        VMEnt.TRN_AMOUNT = lblInvoiceAmount.Text;
        VMEnt.NARRATION = "Sales Invoice no." + SalesInvoiceNumber + " (Export Invoice no." + ExportInvoiceNumber + ")";
        VMEnt.REF_TABLE = "Invoice Master";
        VMEnt.REF_ID = SalesPkId;
        VMEnt.PREPARE_BY = userProfile.EmployeeID;
        VMEnt.CHECKED_BY = userProfile.EmployeeID;
        VMEnt.APPROVED_BY = userProfile.EmployeeID;
        VMEnt.STATUS = "2";
        VMEnt.APPROVED_DATE = PGD.GetTodayDate("dd/mm/yyyy");
        VMEnt.APPROVED_DAY = PGD.NepaliDay();
        VMEnt.APPROVED_MONTH = PGD.NepaliMonth();
        VMEnt.APPROVED_YEAR = PGD.NepaliYear();
        VMEnt.APPROVED_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
        VMEnt.OFFICE_CODE = userProfile.LocationID;
        VoucherPkId = VMSer.Insert(VMEnt, DT).ToString();

        #endregion

        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.PK_ID = SalesPkId;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt, DT);
        if (SIMEnt != null)
        {
            SIMEnt.SB_VM_ID = VoucherPkId;
            SIMSer.Update(SIMEnt, DT);
        }

        #region Voucher Child

        Sno = 1;

        #region Dr Part

        VCEnt = new VOUCHER_CHILD();
        VCEnt.VOUCHER_ID = VoucherPkId;
        VCEnt.SNO = Sno.ToString();
        VCEnt.GL_CODE = "010301";
        VCEnt.SGL_CODE = txtCustomerCode.Text;
        VCEnt.DR_AMOUNT = lblInvoiceAmount.Text;
        VCEnt.CR_AMOUNT = "0";
        VCEnt.REMARKS = "By";
        VCEnt.OFFICE_CODE = userProfile.LocationID;
        VCSer.Insert(VCEnt, DT);
        Sno++;

        VCEnt = new VOUCHER_CHILD();
        VCEnt.VOUCHER_ID = VoucherPkId;
        VCEnt.SNO = Sno.ToString();
        VCEnt.GL_CODE = "030201";
        VCEnt.DR_AMOUNT = txtDiscountAmount.Text;
        VCEnt.CR_AMOUNT = "0";
        VCEnt.REMARKS = "By";
        VCEnt.OFFICE_CODE = userProfile.LocationID;
        if (txtDiscountAmount.Text != "0")
        {
            VCSer.Insert(VCEnt, DT);
            Sno++;
        }

        RoundOff = 0;
        RoundOffValue = 0;
        try
        {
            RoundOffValue = Convert.ToDouble(txtRound.Text);
            if (RoundOffValue < 0)
                RoundOff = RoundOffValue * -1;
            else
                RoundOff = RoundOffValue;
        }
        catch { }

        VCEnt = new VOUCHER_CHILD();
        VCEnt.VOUCHER_ID = VoucherPkId;
        VCEnt.SNO = Sno.ToString();
        VCEnt.GL_CODE = "030202";
        VCEnt.DR_AMOUNT = RoundOff.ToString();
        VCEnt.CR_AMOUNT = "0";
        VCEnt.REMARKS = "By";
        VCEnt.OFFICE_CODE = userProfile.LocationID;
        if (RoundOffValue < 0)
        {
            VCSer.Insert(VCEnt, DT);
            Sno++;
        }

        #endregion

        #region Cr Part

        foreach (GridViewRow r in grdSalesDetail.Rows)
        {
            LblProductCode = r.FindControl("lblProductCode") as Label;
            LblAfterScheDisc = r.FindControl("lblAfterScheDisc") as Label;

            VCEnt = new VOUCHER_CHILD();
            VCEnt.VOUCHER_ID = VoucherPkId;
            VCEnt.SNO = Sno.ToString();
            VCEnt.GL_CODE = "020101";
            VCEnt.SGL_CODE = LblProductCode.Text;
            VCEnt.DR_AMOUNT = "0";
            VCEnt.CR_AMOUNT = LblAfterScheDisc.Text;
            VCEnt.REMARKS = "To";
            VCEnt.OFFICE_CODE = userProfile.LocationID;
            VCSer.Insert(VCEnt, DT);
            Sno++;
        }

        if (ddlExempted.SelectedValue == "1")
        {
            VCEnt = new VOUCHER_CHILD();
            VCEnt.VOUCHER_ID = VoucherPkId;
            VCEnt.SNO = Sno.ToString();
            VCEnt.GL_CODE = "040108";
            VCEnt.SGL_CODE = "";
            VCEnt.DR_AMOUNT = "0";
            VCEnt.CR_AMOUNT = lblVAT.Text;
            VCEnt.REMARKS = "To";
            VCEnt.OFFICE_CODE = userProfile.LocationID;
            VCSer.Insert(VCEnt, DT);
            Sno++;
        }

        RoundOff = 0;
        RoundOffValue = 0;
        try
        {
            RoundOffValue = Convert.ToDouble(txtRound.Text);
            if (RoundOffValue < 0)
                RoundOff = RoundOffValue * -1;
            else
                RoundOff = RoundOffValue;
        }
        catch { }

        VCEnt = new VOUCHER_CHILD();
        VCEnt.VOUCHER_ID = VoucherPkId;
        VCEnt.SNO = Sno.ToString();
        VCEnt.GL_CODE = "020201";
        VCEnt.DR_AMOUNT = "0";
        VCEnt.CR_AMOUNT = RoundOff.ToString();
        VCEnt.REMARKS = "To";
        VCEnt.OFFICE_CODE = userProfile.LocationID;
        if (RoundOffValue > 0)
        {
            VCSer.Insert(VCEnt, DT);
            Sno++;
        }

        #endregion

        #endregion

        #endregion

        #region Voucher Master + Child if sales is in cash or bank (CV)

        if (ddlPaymentType.SelectedValue != "CR")
        {
            VMEnt = new VOUCHER_MASTER();
            VMEnt.VOUCHER_TYPE = "CV";
            VMEnt.VOUCHER_NUMBER = af.getNext_VM_ID("CV", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfile.LocationID);
            VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
            VMEnt.VOUCHER_DAY = PGD.NepaliDay();
            VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
            VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
            VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
            VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
            VMEnt.TRN_AMOUNT = lblInvoiceAmount.Text;
            VMEnt.NARRATION = "Payment received from " + ddlCustomer.SelectedItem.ToString() + " for Invoice no." + InvoiceNumber;
            VMEnt.REF_TABLE = "Invoice Master";
            VMEnt.REF_ID = SalesPkId;
            VMEnt.PREPARE_BY = userProfile.EmployeeID;
            VMEnt.CHECKED_BY = userProfile.EmployeeID;
            VMEnt.APPROVED_BY = userProfile.EmployeeID;
            VMEnt.STATUS = "2";
            VMEnt.APPROVED_DATE = PGD.GetTodayDate("dd/mm/yyyy");
            VMEnt.APPROVED_DAY = PGD.NepaliDay();
            VMEnt.APPROVED_MONTH = PGD.NepaliMonth();
            VMEnt.APPROVED_YEAR = PGD.NepaliYear();
            VMEnt.APPROVED_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
            VMEnt.OFFICE_CODE = userProfile.LocationID;
            VoucherPkId = VMSer.Insert(VMEnt, DT).ToString();

            VCEnt = new VOUCHER_CHILD();
            VCEnt.VOUCHER_ID = VoucherPkId;
            VCEnt.SNO = "1";

            if (ddlPaymentType.SelectedValue == "CS")
            {
                VCEnt.GL_CODE = "010102";
            }
            else if (ddlPaymentType.SelectedValue == "QR")
            {
                VCEnt.GL_CODE = "010101";
                VCEnt.SGL_CODE = ddlBank.SelectedValue;
            }

            VCEnt.DR_AMOUNT = lblInvoiceAmount.Text;
            VCEnt.CR_AMOUNT = "0";
            VCEnt.REMARKS = "By";
            VCEnt.OFFICE_CODE = userProfile.LocationID;
            VCSer.Insert(VCEnt, DT);

            VCEnt = new VOUCHER_CHILD();
            VCEnt.VOUCHER_ID = VoucherPkId;
            VCEnt.SNO = "2";
            VCEnt.GL_CODE = "010301";
            VCEnt.SGL_CODE = txtCustomerCode.Text;
            VCEnt.DR_AMOUNT = "0";
            VCEnt.CR_AMOUNT = lblInvoiceAmount.Text;
            VCEnt.REMARKS = "To";
            VCEnt.OFFICE_CODE = userProfile.LocationID;
            VCSer.Insert(VCEnt, DT);
        }

        #endregion

        if (DT.HAPPY == true)
        {
            MovePackTempToPackingList(SalesPkId, DT);
            DT.Commit();

            lblPK_ID.Text = ExportPkId;

            #region CBMS push

            SIMEnt = new SALES_INVOICE_MASTER();
            SIMEnt.PK_ID = SalesPkId;
            SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);

            if (SIMEnt != null)
            {
                if (PG.CBMSPush() == "ON")
                {
                    using (HttpClient Client = new HttpClient())
                    {
                        Client.DefaultRequestHeaders.Accept.Clear();
                        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        BillViewModel BillVM = new BillViewModel
                        {
                            username = PG.CBMSUsername(),
                            password = PG.CBMSPassword(),
                            seller_pan = PG.CompanyVATPan(),
                            buyer_pan = SIMEnt.COSTOMER_PAN_VAT,
                            buyer_name = SIMEnt.CUSTOMER_NAME,
                            fiscal_year = PGD.CBMSFY(SIMEnt.INVOICE_FY),
                            invoice_number = SIMEnt.INVOICE_NUMBER,
                            invoice_date = SIMEnt.INVOICE_YEAR + "." + SIMEnt.INVOICE_MONTH + "." + SIMEnt.INVOICE_DAY,
                            total_sales = Convert.ToDouble(SIMEnt.TAXABLE_SUB_TOTAL),
                            taxable_sales_vat = 0,
                            vat = 0,
                            excisable_amount = 0,
                            excise = 0,
                            taxable_sales_hst = 0,
                            hst = 0,
                            amount_for_esf = 0,
                            esf = 0,
                            export_sales = Convert.ToDouble(SIMEnt.TAXABLE_SUB_TOTAL),
                            tax_exempted_sales = 0,
                            isrealtime = true,
                            datetimeClient = DateTime.Now
                        };

                        Client.BaseAddress = new Uri(PG.CBMSURL());

                        try
                        {
                            System.Net.Http.HttpResponseMessage Response = Client.PostAsJsonAsync("api/bill", BillVM).Result;

                            if (Response.IsSuccessStatusCode)
                            {
                                System.Net.Http.HttpContent ResultContent = Response.Content;
                                string ResultString = ResultContent.ReadAsStringAsync().Result;
                                SIMEnt.CBMS_PUSH = ResultString;
                                if (ResultString == "200")
                                    SIMEnt.CBMS_PUSH_RT = "YES";
                                else
                                    SIMEnt.CBMS_PUSH_RT = "NO";
                            }
                            else
                            {
                                SIMEnt.CBMS_PUSH = "Error";
                                SIMEnt.CBMS_PUSH_RT = "NO";
                            }
                        }
                        catch
                        {
                            SIMEnt.CBMS_PUSH = "Offline";
                            SIMEnt.CBMS_PUSH_RT = "NO";
                        }
                    }

                    SIMSer.Update(SIMEnt);
                }
                else
                {
                    SIMEnt.CBMS_PUSH = "CBMS OFF";
                    SIMEnt.CBMS_PUSH_RT = "";
                    SIMSer.Update(SIMEnt);
                }
            }

            #endregion

            EIMEnt = new EXPORT_INVOICE_MASTER();
            EIMEnt.PK_ID = ExportPkId;
            EIMEnt = (EXPORT_INVOICE_MASTER)EIMSer.GetSingle(EIMEnt);

            if (EIMEnt != null)
            {
                lblInvoiceHeading.Text = PG.InvoiceHeading();
                lblInvoiceHeading1.Text = "";

                lblPK_ID.Text = SalesPkId;
                LoadToPrint(SalesPkId);
                btnOfficeCopy.Visible = true;
                btnProforma.Visible = true;

                CreateGridFirst();
                Clear();
                lblunique_token.Text = userProfile.UserName + hf.getmaxinvid();
            }
            tdAdvAmount.Visible = false;
        }
        else
        {
            DT.Abort();
            HelperFunction.MsgBox(this, this.GetType(), "Sorry Something Goes Wrong.");
        }

        DT.Dispose();
       

    }

    protected void btnOfficeCopy_Click(object sender, EventArgs e)
    {
        if (PG.InvoiceHeading() == "TAX INVOICE")
            lblInvoiceHeading.Text = "INVOICE";
        else
            lblInvoiceHeading.Text = PG.InvoiceHeading();

        OfficeCopy = true;
        btnOfficeCopy.Visible = false;
        LoadToPrint(lblPK_ID.Text, "OFFICE");
    }

    protected void btnProforma_Click(object sender, EventArgs e)
    {
        LoadToPrint(lblPK_ID.Text, "PROFORMA");
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        CreateGridFirst();
        Clear();
        printdetail.Visible = false;
        printdetail80.Visible = false;
        printdetailTOK.Visible = false;
        printdetailProforma.Visible = false;
        btnOfficeCopy.Visible = false;
        lblunique_token.Text = userProfile.UserName + hf.getmaxinvid();
        UpdateBillingExchangeRateInfo();
    }

    protected void cleardata()
    {
        lblInvoiceNo.Text = "";
        lblCustomerName.Text = "";
        txtCustomerAddress.Text = "";
        txtCustomerPANVAT.Text = "0";
        lblTranDate.Text = "";
        lblTranNepaliDate.Text = "";
        lblBillEnglishDate.Text = "";
        lblBillNepaliDate.Text = "";
        lblAddress.Text = "";
        lblBillSubTotal.Text = "";
        lblBillDiscountPercent.Text = "";
        lblDiscount.Text = "";
        lblGTotal.Text = "";
        lblBillAmount.Text = "";
        lblAmountInWord.Text = "";
        lblVATReturn80.Text = "";
        trVATReturn80.Visible = false;
        lblProAmountInWord.Text = "";
    }

    protected void LoadToPrint(string pk_id, string printMode = "SAVE")
    {
        cleardata();
        LoadCompanyDetail();
        if (OfficeCopy != true)
        {
            if (PG.InvoiceHeading() == "TAX INVOICE")
                lblInvoiceHeading.Text = "TAX INVOICE";
            else
                lblInvoiceHeading.Text = PG.InvoiceHeading();
        }

        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.PK_ID = pk_id;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);

        if (SIMEnt != null)
        {
            lblInvoiceNo.Text = SIMEnt.INVOICE_NUMBER;
            lblCustomerName.Text = SIMEnt.CUSTOMER_NAME;
            lblAddress.Text = SIMEnt.CUSTOMER_ADDRESS;
            lblCustomerPanNo.Text = SIMEnt.COSTOMER_PAN_VAT;
            lblModeofPayment.Text = hf.getPaymentType(SIMEnt.SALES_TYPE_ID);
            lblBillEnglishDate.Text = SIMEnt.INVOICE_DATE;
            lblBillNepaliDate.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
            lblTranDate.Text = SIMEnt.TRANSACTION_DATE;
            lblTranNepaliDate.Text = SIMEnt.TRANSACTION_DAY + "/" + SIMEnt.TRANSACTION_MONTH + "/" + SIMEnt.TRANSACTION_YEAR;
            lblPrintedBy.Text = hf.getEmployeeName(SIMEnt.PRINT_BY);
            lblTime.Text = SIMEnt.PRINT_TIME;
            lblInvCreatedBy.Text = hf.getEmployeeName(SIMEnt.USER_ID);
            lblBillSubTotal.Text = Convert.ToDouble(SIMEnt.TAXABLE_SUB_TOTAL).ToString("##,##0.00");
            lblBillDiscountPercent.Text = Convert.ToDouble(SIMEnt.DISCOUNT_PERCENT).ToString("#0.00");
            lblDiscount.Text = Convert.ToDouble(SIMEnt.TAXABLE_DISC_AMOUNT).ToString("##,##0.00");
            lblTaxableAmount.Text = (Convert.ToDouble(SIMEnt.TAXABLE_TOTAL).ToString("##,##0.00"));

            if (SIMEnt.TAX_VAT_AMOUNT == "0")
                lblVATPercent.Text = "0";
            else
                lblVATPercent.Text = Convert.ToDouble(PG.CompanyTAXPercent()).ToString("00.00");

            lblVATAmount.Text = Convert.ToDouble(SIMEnt.TAX_VAT_AMOUNT).ToString("##,##0.00");

            double vatReturnAmt = 0;
            try { vatReturnAmt = Convert.ToDouble(SIMEnt.VAT_RETURN); } catch { }

            //if (SIMEnt.SALES_TYPE_ID == "QR" && vatReturnAmt > 0)
            //{
            //    trInvVATReturn.Visible = true;
            //    lblInvVATReturn.Text = vatReturnAmt.ToString("##,##0.00");
            //}
            //else
            //{
            //    trInvVATReturn.Visible = false;
            //}

            double preReturnTotalPrint = Convert.ToDouble(SIMEnt.TAXABLE_TOTAL) + Convert.ToDouble(SIMEnt.TAX_VAT_AMOUNT);
            lblGTotal.Text = preReturnTotalPrint.ToString("##,##0.00");
            lblRoundoff.Text = Convert.ToDouble(SIMEnt.ROUND_OFF).ToString("##,##0.00");
            lblBillAmount.Text = Convert.ToDouble(SIMEnt.INVOICE_AMOUNT).ToString("##,##0.00");
            lblAmountInWord.Text = hf.NumWordsWrapper(Convert.ToDouble(SIMEnt.INVOICE_AMOUNT)) + " only";
            lblForCompanyName.Text = PG.CompanyName();
            lblPONumber.Text = SIMEnt.PO_NUMBER;

            if (SIMEnt.REMARKS != "")
            {
                lblRemarks.Text = "* " + SIMEnt.REMARKS;
            }

            LoadSalesGrid(SIMEnt.PK_ID);
            LoadSalesGrid80(SIMEnt.PK_ID);
            LoadSalesGridTOK(SIMEnt.PK_ID);
            SetGridColumnVisibility();

            //if (!string.IsNullOrEmpty(currentOrderNumber))
            //{
            //    lblOrderNoHidden.Text = currentOrderNumber;
            //}

            if (OfficeCopy == true)
            {
                if (PG.InvoiceHeading() == "TAX INVOICE")
                    lblInvoiceHeading80.Text = "INVOICE";
                else
                    lblInvoiceHeading80.Text = PG.InvoiceHeading();
            }
            else
            {
                lblInvoiceHeading80.Text = PG.InvoiceHeading();
            }

            lblInvoiceNo80.Text = SIMEnt.INVOICE_NUMBER;
            lblBillNepaliDate80.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
            lblBillEnglishDate80.Text = SIMEnt.INVOICE_DATE;
            lblTranNepaliDate80.Text = SIMEnt.TRANSACTION_DAY + "/" + SIMEnt.TRANSACTION_MONTH + "/" + SIMEnt.TRANSACTION_YEAR;
            lblTranDate80.Text = SIMEnt.TRANSACTION_DATE;
            lblCustomerName80.Text = SIMEnt.CUSTOMER_NAME;
            lblCustomerPanNo80.Text = SIMEnt.COSTOMER_PAN_VAT;
            lblModeofPayment80.Text = hf.getPaymentType(SIMEnt.SALES_TYPE_ID);
            lblBillSubTotal80.Text = Convert.ToDouble(SIMEnt.TAXABLE_SUB_TOTAL).ToString("##,##0.00");
            lblDiscPercent80.Text = Convert.ToDouble(SIMEnt.DISCOUNT_PERCENT).ToString("#0.00");
            lblDiscount80.Text = Convert.ToDouble(SIMEnt.TAXABLE_DISC_AMOUNT).ToString("##,##0.00");
            lblTaxable80.Text = Convert.ToDouble(SIMEnt.TAXABLE_TOTAL).ToString("##,##0.00");

            if (SIMEnt.TAX_VAT_AMOUNT == "0")
                lblVATPercent80.Text = "0";
            else
                lblVATPercent80.Text = Convert.ToDouble(PG.CompanyTAXPercent()).ToString("00.00");

            lblVAT80.Text = Convert.ToDouble(SIMEnt.TAX_VAT_AMOUNT).ToString("##,##0.00");

            if (SIMEnt.SALES_TYPE_ID == "QR" && vatReturnAmt > 0)
            {
                trVATReturn80.Visible = true;
                lblVATReturn80.Text = vatReturnAmt.ToString("##,##0.00");
            }
            else
            {
                trVATReturn80.Visible = false;
            }

            lblGrandTotal80.Text = preReturnTotalPrint.ToString("##,##0.00");
            lblGTotal80.Text = Convert.ToDouble(SIMEnt.INVOICE_AMOUNT).ToString("##,##0.00");
            lblAmountInWord80.Text = hf.NumWordsWrapper(Convert.ToDouble(SIMEnt.INVOICE_AMOUNT)) + " Only.";
            lblPrintedBy80.Text = hf.getEmployeeName(SIMEnt.PRINT_BY);
            lblTime80.Text = SIMEnt.PRINT_TIME;
            lblCreatedBy80.Text = hf.getEmployeeName(SIMEnt.USER_ID);

            if (SIMEnt.REMARKS != "")
                lblRemarks80.Text = "* " + SIMEnt.REMARKS;

            lblBillNoTOK.Text = SIMEnt.INVOICE_NUMBER;

            //if (!string.IsNullOrEmpty(currentOrderNumber))
            //    lblOrderNoTOK.Text = currentOrderNumber;
            //else
            //    lblOrderNoTOK.Text = lblOrderNoHidden.Text;

            lblBillDateTOK.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
            lblCompanyNameTOK.Text = PG.CompanyName();
            lblCompanyAddressTOK.Text = PG.BranchAddress(userProfile.LocationID);
            lblPhoneTOK.Text = PG.BranchContact(userProfile.LocationID);
            lblPanNoTOK.Text = PG.CompanyVATPan();
            lblPrintedByTOK.Text = hf.getEmployeeName(SIMEnt.PRINT_BY);
            lblTimeTOK.Text = SIMEnt.PRINT_TIME;

            //lblOrderNoSimpleTOK.Text = currentOrderNumber;
            lblDateSimpleTOK.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
            lblNameSimpleTOK.Text = SIMEnt.CUSTOMER_NAME;

            NEnt = new NAME_COMPANY();
            NEnt = (NAME_COMPANY)NSer.GetSingle(NEnt);

            string billType = "A5L";
            if (NEnt != null && !string.IsNullOrEmpty(NEnt.INVOICE_TYPE))
                billType = NEnt.INVOICE_TYPE;

            if (printMode == "PROFORMA")
            {
                EIMEnt = new EXPORT_INVOICE_MASTER();
                EIMEnt.INVOICE_MASTER_ID = SIMEnt.PK_ID;
                EntityList exportList = EIMSer.GetAll(EIMEnt);

                if (exportList != null && exportList.Count > 0)
                    EIMEnt = (EXPORT_INVOICE_MASTER)exportList[0];
                else
                    EIMEnt = null;

                if (EIMEnt != null)
                {
                    double paymentRate = 0;
                    DateTime paymentRateDate = DateTime.MinValue;
                    double paymentFactor = 1;
                    string paymentCurrencyCode = "";

                    FCEnt = new FOREIGN_CURRENCY();
                    FCEnt.PK_ID = EIMEnt.CURRENCY_TYPE_ID;
                    FCEnt = (FOREIGN_CURRENCY)FCSer.GetSingle(FCEnt);
                    if (FCEnt != null)
                        paymentCurrencyCode = FCEnt.CURRENCY_CODE;

                    if (!string.IsNullOrEmpty(EIMEnt.CURRENCY_TYPE_ID))
                    {
                        string invDateNep = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
                        string invDateEng = PGD.GetEnglishDateFromNepali(invDateNep, "dd/mm/yyyy");

                        if (TryGetExchangeRate(EIMEnt.CURRENCY_TYPE_ID, invDateEng, out paymentRate, out paymentRateDate) && paymentRate > 0)
                            paymentFactor = 1 / paymentRate;
                    }

                    string proSubTotalText = paymentCurrencyCode + " " + Convert.ToDouble(EIMEnt.FINAL_AMOUNT).ToString("##,##0.00");

                    PopulateProformaLabels(EIMEnt, SIMEnt.INVOICE_NUMBER);
                    LoadSalesGridProforma(EIMEnt.INVOICE_MASTER_ID, proSubTotalText, paymentFactor, paymentCurrencyCode);

                    printdetailProforma.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printProforma();", true);
                    return;
                }
            }

            if (printMode == "OFFICE")
            {
                if (billType == "Continuous")
                {
                    printdetail.Visible = false;
                    printdetail80.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printBill80mmOnly();", true);
                }
                else
                {
                    printdetail.Visible = true;
                    printdetail80.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
                }
            }
            else
            {
                if (billType == "Continuous")
                {
                    printdetail80.Visible = true;
                    printdetailTOK.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printTOKand80mm();", true);
                }
                else
                {
                    printdetail.Visible = true;
                    printdetail80.Visible = false;
                    printdetailTOK.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
                }
            }
        }
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

        lblProContractDate.Text = PGD.ConvertToOrdinalDateFormat(EIMEnt.CONTRACT_DATE);

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

        ProCurrencyName = EIMEnt.CURRENCY_TYPE_ID;
        FCEnt = new FOREIGN_CURRENCY();
        FCEnt.PK_ID = EIMEnt.CURRENCY_TYPE_ID;
        FCEnt = (FOREIGN_CURRENCY)FCSer.GetSingle(FCEnt);
        if (FCEnt != null)
            ProCurrencyName = FCEnt.CURRENCY_NAME;

        lblProPaymentCurrency.Text = ProCurrencyName;
        lblProPaymentMode.Text = hf.getPaymentType(EIMEnt.MODE_OF_PAYMENT);
        lblProShipmentType.Text = EIMEnt.SHIPMENT_TYPE;
        lblProShipmentNo.Text = EIMEnt.SHIPMENT_NUMBER;
        lblProRemarks.Text = EIMEnt.REMARKS;
        lblProUserName.Text = PG.CompanyName();
        lblProAmountInWord.Text = ProCurrencyName + " " + hf.NumWordsWrapper(Convert.ToDouble(EIMEnt.FINAL_AMOUNT)) + " only";
    }

    protected void LoadSalesGridProforma(string PK_id, string subTotalText, double paymentFactor = 1, string currencyCode = "")
    {
        DummyTable = hf.LoadSalesInvoice(PK_id);
        ConvertMoneyColumns(DummyTable, paymentFactor);

        PrefixColumns = new string[] { "RATE", "TOTAL" };
        PrefixCurrencyCode(DummyTable, currencyCode, PrefixColumns);

        gridProforma.DataSource = DummyTable;
        gridProforma.DataBind();

        FooterRow = gridProforma.FooterRow;
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

    protected void LoadSalesGrid(string PK_id, double billingFactor = 1)
    {
        DummyTable = hf.LoadSalesInvoice(PK_id);
        ConvertMoneyColumns(DummyTable, billingFactor);

        gridSalesInvoice.DataSource = DummyTable;
        gridSalesInvoice.DataBind();
    }

    protected void LoadSalesGrid80(string PK_id, double billingFactor = 1)
    {
        DummyTable = hf.LoadSalesInvoice(PK_id);
        ConvertMoneyColumns(DummyTable, billingFactor);

        gridSalesInvoice80.DataSource = DummyTable;
        gridSalesInvoice80.DataBind();
    }

    protected void LoadSalesGridTOK(string PK_id)
    {
        DummyTable = hf.LoadSalesInvoice(PK_id);
        gridTOK.DataSource = DummyTable;
        gridTOK.DataBind();
    }

    private void ConvertMoneyColumns(DataTable dt, double billingFactor)
    {
        if (billingFactor == 1 || dt == null) return;
        MoneyColumns = new string[] { "RATE", "TOTAL", "SCHEME_DISCOUNT", "TAXABLE_TOTAL" };
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

    private void SetGridColumnVisibility()
    {
        gridSalesInvoice.Columns[3].Visible = PGPS.ProductBatch();
        gridSalesInvoice.Columns[4].Visible = PGPS.ProductExpDate();
        gridSalesInvoice.Columns[5].Visible = PGPS.ShowDualQuantity();
        gridSalesInvoice.Columns[9].Visible = PGPS.ItemWiseDiscount();
        gridSalesInvoice.Columns[10].Visible = PGPS.ItemWiseDiscount();
    }

    protected void gridSalesInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DRV = e.Row.DataItem as DataRowView;
            if (DRV == null) return;

            LblHSCode = e.Row.FindControl("lblHSCode") as Label;

            if (LblHSCode != null)
            {
                HsCode = "";

                if (DRV.DataView.Table.Columns.Contains("HS_CODE"))
                {
                    object hsObj = DRV["HS_CODE"];
                    if (hsObj != null && hsObj != DBNull.Value)
                    {
                        HsCode = hsObj.ToString();
                    }
                }

                if (string.IsNullOrEmpty(HsCode) && DRV.DataView.Table.Columns.Contains("PRODUCT_CODE"))
                {
                    object pcObj = DRV["PRODUCT_CODE"];
                    if (pcObj != null && pcObj != DBNull.Value)
                    {
                        ProductCode = pcObj.ToString();

                        PEnt = new PRODUCT();
                        PEnt.PRODUCT_CODE = ProductCode;
                        PEnt = (PRODUCT)PSer.GetSingle(PEnt);

                        if (PEnt != null && PEnt.HS_CODE != null)
                        {
                            HsCode = PEnt.HS_CODE;
                        }
                    }
                }

                LblHSCode.Text = HsCode;
            }
        }
    }

    protected void gridSalesInvoice80_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LblProName80 = e.Row.FindControl("lblProName80") as Label;
            if (LblProName80 != null)
            {
                Text80 = LblProName80.Text;
                if (!string.IsNullOrEmpty(Text80) && Text80.Length > 20)
                {
                    Text80 = Text80.Substring(0, 20).TrimEnd() + "...";
                }
                LblProName80.Text = Text80;
            }
        }
    }

    protected string getUnitName(string PK_ID)
    {
        string unit_name = "";
        PUEnt = new PRODUCT_UNIT();
        PUEnt.PK_ID = PK_ID;
        PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
        if (PUEnt != null && PK_ID != "")
            unit_name = PUEnt.UNIT_NAME;
        return unit_name;
    }

    protected string getCustomerName(string pk_id)
    {
        string customer_name = "";
        CEnt = new CUSTOMER();
        CEnt.PK_ID = pk_id;
        CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
        if (CEnt != null)
        {
            customer_name = CEnt.CUSTOMER_NAME;
        }
        return customer_name;
    }

    public class BillViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string seller_pan { get; set; }
        public string buyer_pan { get; set; }
        public string fiscal_year { get; set; }
        public string buyer_name { get; set; }
        public string invoice_number { get; set; }
        public string invoice_date { get; set; }
        public double total_sales { get; set; }
        public Nullable<double> taxable_sales_vat { get; set; }
        public Nullable<double> vat { get; set; }
        public Nullable<double> excisable_amount { get; set; }
        public Nullable<double> excise { get; set; }
        public Nullable<double> taxable_sales_hst { get; set; }
        public Nullable<double> hst { get; set; }
        public Nullable<double> amount_for_esf { get; set; }
        public Nullable<double> esf { get; set; }
        public Nullable<double> export_sales { get; set; }
        public Nullable<double> tax_exempted_sales { get; set; }
        public bool isrealtime { get; set; }
        public DateTime datetimeClient { get; set; }
    }

    protected void btnPack_Click(object sender, EventArgs e)
    {
        hdnProductCode.Value = txtProductCode.Text;
        hdnPackMode.Value = "NORMAL";
        hdnEditPackPK.Value = "";
        hdnEditPackQty.Value = "";
        hdnEditPackProductCode.Value = "";

        OpenPackPopup(editMode: false);
    }

    protected void btnAddPack_Click(object sender, EventArgs e)
    {
        string productCode = hdnProductCode.Value;

        double enteredQty = 0;
        double.TryParse(txtPckQTY.Text, out enteredQty);

        string pack = txtPack.Text;
        Remaining = RecalculatePackAvailability(hdnEditPackPK.Value);

        string errorMsg = "";

        if (string.IsNullOrEmpty(productCode))
            errorMsg = "Product is not selected.";
        else if (enteredQty <= 0)
            errorMsg = "Enter a valid quantity.";
        else if (enteredQty > Remaining)
            errorMsg = "Quantity cannot exceed the available quantity (" + Remaining.ToString("0.##") + ").";

        if (errorMsg != "")
        {
            HelperFunction.MsgBox(this, this.GetType(), errorMsg);
            BindPackGrid();
            btnPopup_ModalPopupExtender.Show();
            return;
        }

        EPackTemp = new PACKING_LIST_TEMP();
        EPackTemp.PRODUCT_CODE = productCode;
        EPackTemp.QTY = enteredQty.ToString();
        EPackTemp.PACK_NO = pack;
        EPackTemp.USER_ID = userProfile.EmployeeID;

        SPackTemp.Insert(EPackTemp);

        RecalculatePackAvailability(hdnEditPackPK.Value);

        txtPckQTY.Text = "";
        txtPack.Text = "";

        BindPackGrid();
        btnPopup_ModalPopupExtender.Show();
    }

    protected void btnSavePack_Click(object sender, EventArgs e)
    {

    }
    private double RecalculatePackAvailability(string excludePkId = null)
    {
        CurrentQty = 0;

        if (hdnPackMode.Value == "EDIT" && !string.IsNullOrEmpty(hdnEditPackQty.Value))
        {
            double.TryParse(hdnEditPackQty.Value, out CurrentQty);
        }
        else
        {
            double.TryParse(txtQty.Text, out CurrentQty);
        }

        AlreadyPacked = 0;

        if (!string.IsNullOrEmpty(hdnProductCode.Value))
        {
            PLTEnt = new PACKING_LIST_TEMP();
            PLTEnt.PRODUCT_CODE = hdnProductCode.Value;
            PLTList = SPackTemp.GetAll(PLTEnt);

            foreach (PACKING_LIST_TEMP PLTRow in PLTList)
            {
                if (!string.IsNullOrEmpty(excludePkId) && PLTRow.PK_ID == excludePkId)
                    continue;

                Q = 0;
                double.TryParse(PLTRow.QTY, out Q);
                AlreadyPacked = AlreadyPacked + Q;
            }
        }

        Remaining = CurrentQty - AlreadyPacked;
        if (Remaining < 0)
            Remaining = 0;

        txtAvlQty.Text = Remaining.ToString();

        return Remaining;
    }

    private void BindPackGrid()
    {
        PackDT = new DataTable();
        PackDT.Columns.Add("PK_ID");
        PackDT.Columns.Add("PRODUCT_CODE");
        PackDT.Columns.Add("QTY");
        PackDT.Columns.Add("PACK_NO");
        PackDT.Columns.Add("USER_ID");

        PLTEnt = new PACKING_LIST_TEMP();
        PLTEnt.PRODUCT_CODE = hdnProductCode.Value;
        PLTList = SPackTemp.GetAll(PLTEnt);

        foreach (PACKING_LIST_TEMP PLTRow in PLTList)
        {
            DR = PackDT.NewRow();
            DR["PK_ID"] = PLTRow.PK_ID;
            DR["PRODUCT_CODE"] = PLTRow.PRODUCT_CODE;
            DR["QTY"] = PLTRow.QTY;
            DR["PACK_NO"] = PLTRow.PACK_NO;
            DR["USER_ID"] = PLTRow.USER_ID;
            PackDT.Rows.Add(DR);
        }

        grdPack.DataSource = PackDT;
        grdPack.DataBind();
    }

    protected void grdPack_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        DRV = e.Row.DataItem as DataRowView;
        if (DRV == null)
            return;

        PkId = "";
        if (DRV["PK_ID"] != null)
            PkId = DRV["PK_ID"].ToString();

        LblPackQty = e.Row.FindControl("lblPackQty") as Label;
        TxtPackQty = e.Row.FindControl("txtPackQty") as TextBox;
        LblPackNo = e.Row.FindControl("lblPackNo") as Label;
        TxtPackNo = e.Row.FindControl("txtPackNo") as TextBox;
        ImgPackEdit = e.Row.FindControl("imgPackEdit") as ImageButton;
        ImgPackDelete = e.Row.FindControl("imgPackDelete") as ImageButton;
        ImgPackUpdate = e.Row.FindControl("imgPackUpdate") as ImageButton;
        ImgPackCancel = e.Row.FindControl("imgPackCancel") as ImageButton;

        IsEditing = false;
        if (PkId != "" && hdnEditPackPK.Value == PkId)
            IsEditing = true;

        if (IsEditing)
        {
            LblPackQty.Visible = false;
            TxtPackQty.Visible = true;

            LblPackNo.Visible = false;
            TxtPackNo.Visible = true;

            ImgPackEdit.Visible = false;
            ImgPackDelete.Visible = false;
            ImgPackUpdate.Visible = true;
            ImgPackCancel.Visible = true;
        }
        else
        {
            LblPackQty.Visible = true;
            TxtPackQty.Visible = false;

            LblPackNo.Visible = true;
            TxtPackNo.Visible = false;

            ImgPackEdit.Visible = true;
            ImgPackDelete.Visible = true;
            ImgPackUpdate.Visible = false;
            ImgPackCancel.Visible = false;
        }
    }

    protected void grdPack_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditPack")
        {
            hdnEditPackPK.Value = e.CommandArgument.ToString();

            BindPackGrid();
            ReopenPackPopup();
        }
        else if (e.CommandName == "UpdatePack")
        {
            GRow = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;

            if (GRow != null)
            {
                TxtPackQty = GRow.FindControl("txtPackQty") as TextBox;
                TxtPackNo = GRow.FindControl("txtPackNo") as TextBox;

                NewQty = "";
                if (TxtPackQty != null)
                    NewQty = TxtPackQty.Text;

                NewPackNo = "";
                if (TxtPackNo != null)
                    NewPackNo = TxtPackNo.Text;

                PkId = e.CommandArgument.ToString();

                double newQtyVal = 0;
                double.TryParse(NewQty, out newQtyVal);

                Remaining = RecalculatePackAvailability(PkId);

                if (newQtyVal <= 0 || newQtyVal > Remaining)
                {
                    HelperFunction.MsgBox(this, this.GetType(),
                        "Quantity cannot exceed the available quantity (" + Remaining.ToString("0.##") + ").");

                    hdnEditPackPK.Value = "";
                    RecalculatePackAvailability();

                    BindPackGrid();
                    ReopenPackPopup();
                    return;
                }

                PLTEnt = new PACKING_LIST_TEMP();
                PLTEnt.PK_ID = PkId;
                PLTEnt = (PACKING_LIST_TEMP)SPackTemp.GetSingle(PLTEnt);

                if (PLTEnt != null)
                {
                    PLTEnt.QTY = NewQty;
                    PLTEnt.PACK_NO = NewPackNo;
                    SPackTemp.Update(PLTEnt);
                }
            }

            hdnEditPackPK.Value = "";

            RecalculatePackAvailability();

            BindPackGrid();
            ReopenPackPopup();
        }
        else if (e.CommandName == "CancelPack")
        {
            hdnEditPackPK.Value = "";

            BindPackGrid();
            ReopenPackPopup();
        }
        else if (e.CommandName == "RemovePack")
        {
            PkId = e.CommandArgument.ToString();

            PLTEnt = new PACKING_LIST_TEMP();
            PLTEnt.PK_ID = PkId;
            SPackTemp.Delete(PLTEnt);

            if (hdnEditPackPK.Value == PkId)
                hdnEditPackPK.Value = "";

            RecalculatePackAvailability();

            BindPackGrid();
            ReopenPackPopup();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hdnPackMode.Value = "NORMAL";
        hdnEditPackPK.Value = "";
        txtPckQTY.ReadOnly = false;
        txtPckQTY.Enabled = true;
        txtPack.ReadOnly = false;
        txtPack.Enabled = true;
    }

    private void ReopenPackPopup()
    {
        txtPckQTY.ReadOnly = false;
        txtPckQTY.Enabled = true;
        txtPack.ReadOnly = false;
        txtPack.Enabled = true;
        btnPopup_ModalPopupExtender.Show();
    }

    private DataTable PackDT = new DataTable();


    private void MovePackTempToPackingList(string invoiceId, DistributedTransaction DT)
    {
        if (string.IsNullOrEmpty(invoiceId))
            return;

        PLTEnt = new PACKING_LIST_TEMP();
        PLTList = SPackTemp.GetAll(PLTEnt);

        if (PLTList == null || PLTList.Count == 0)
            return;

        foreach (PACKING_LIST_TEMP PLTRow in PLTList)
        {
            PEnt = new PRODUCT();
            PEnt.PRODUCT_CODE = PLTRow.PRODUCT_CODE;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);

            if (PEnt == null)
                continue;

            PLPEnt = new PACKING_LIST();
            PLPEnt.INVOICE_ID = invoiceId;
            PLPEnt.PRODUCT_ID = PEnt.PK_ID;
            PLPEnt.PACKING_NO = PLTRow.PACK_NO;
            PLPEnt.QTY = PLTRow.QTY;

            PLPSer.Insert(PLPEnt, DT);

            PLTEnt = new PACKING_LIST_TEMP();
            PLTEnt.PK_ID = PLTRow.PK_ID;
            SPackTemp.Delete(PLTEnt, DT);
        }
    }

    protected void txtPckQTY_TextChanged(object sender, EventArgs e)
    {
        Remaining = RecalculatePackAvailability(hdnEditPackPK.Value);

        double enteredQty = 0;
        double.TryParse(txtPckQTY.Text, out enteredQty);

        if (enteredQty > Remaining)
        {
            HelperFunction.MsgBox(this, this.GetType(),
                "Quantity cannot exceed the available quantity (" + Remaining.ToString("0.##") + ").");
        }

        BindPackGrid();
        btnPopup_ModalPopupExtender.Show();
    }

    protected bool TryGetTodaysExchangeRate(string currencyId, out double rate, out DateTime rateDate)
    {
        rate = 0;
        rateDate = DateTime.MinValue;

        if (string.IsNullOrEmpty(currencyId) || currencyId == "Select")
            return false;

        string todayEnglish = PGD.GetTodayDate("dd/mm/yyyy");
        return TryGetExchangeRate(currencyId, todayEnglish, out rate, out rateDate);
    }
}