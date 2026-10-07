using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Entity.Framework;
using Service.Components;
using PhyeGanCore;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data;
using Newtonsoft.Json;
using System.Web.UI;
using System.Collections.Generic;
using System.Web;

public partial class _Default : System.Web.UI.Page
{
    SALES_INVOICE_MASTER SIMEnt = new SALES_INVOICE_MASTER();
    SALES_INVOICE_MASTERService SIMSer = new SALES_INVOICE_MASTERService();

    OFFICE OEnt = new OFFICE();
    OFFICEService OSer = new OFFICEService();

    SALES_RETURN_MASTER SRMEnt = new SALES_RETURN_MASTER();
    SALES_RETURN_MASTERService SRMSer = new SALES_RETURN_MASTERService();

    EntityList theList = new EntityList();
    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGan PG = new PhyeGan();

    GraphFunction GF = new GraphFunction();
    UserProfileEntity userProfile = new UserProfileEntity();

    HelperFunction hf = new HelperFunction();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack )
        {

            
            ViewState["postids"] = System.Guid.NewGuid().ToString();
            Session["postid"] = ViewState["postids"].ToString();
            try
            {
                userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

                OEnt = new OFFICE();
                OEnt.PK_ID = userProfile.LocationID;
                OEnt = (OFFICE)OSer.GetSingle(OEnt);
                if (OEnt != null)
                {
                    if (OEnt.OFFICECODE != "HO")
                    {
                        divView.Visible = false;
                    }
                    else
                    {
                        divView.Visible = true;
                    }
                }
                
                bool isViewAll = ToggleSwitch.Checked ;
                divView.Visible = PG.CompanyBranch_Status();
                LoadMonthlyTopSalesChart(isViewAll);
                LoadDailySales(isViewAll);
                LoadDailyCashSales(isViewAll);
                LoadDailyCreditSales(isViewAll);
                LoadDailyPurchases(isViewAll);
                LoadCashPurchases(isViewAll);
                LoadDailyCashPurchases(isViewAll);
                LoadDailyCreditPurchases(isViewAll);
                LoadOperatingCostLastSix(isViewAll);
                LoadCashFlowData(isViewAll);
                LoadTopCreditors(isViewAll);
                LoadTopDebtors(isViewAll);
                //loadgrid();
                //loadreturngrid();
                LoadAdmissionDischargeLineGraph();
                #region sales bill
                foreach (GridViewRow gr in gridCBMS.Rows)
                {
                    Label lblPkID = gr.FindControl("lblPkID") as Label;
                    // Perform necessary actions...

                    SIMEnt = new SALES_INVOICE_MASTER();
                    SIMEnt.PK_ID = lblPkID.Text;
                    SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
                    if (SIMEnt != null)
                    {
                        //Sample Code CU for Posting Bill 
                        double taxablesalesvat = 0;
                        double taxexemptedsales = 0;
                        if (SIMEnt.EXEMPTED == "1")
                        {
                            taxexemptedsales = Convert.ToDouble(SIMEnt.TAXABLE_TOTAL);
                        }
                        else
                        {
                            taxablesalesvat = Convert.ToDouble(SIMEnt.TAXABLE_TOTAL);
                        }
                        if (PG.CBMSPush() == "ON")
                        {
                            #region to send in CBMS
                            double taxable_sub_total = 0;
                            double nontaxable_sub_total = 0;
                            if (SIMEnt.EXEMPTED != "1")
                            {
                                taxable_sub_total = Convert.ToDouble(SIMEnt.TAXABLE_SUB_TOTAL);
                            }
                            else
                            {
                                nontaxable_sub_total = Convert.ToDouble(SIMEnt.TAXABLE_SUB_TOTAL);
                            }
                            using (var client = new HttpClient())
                            {
                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                BillViewModel p = new BillViewModel
                                {
                                    username = PG.CBMSUsername(),
                                    password = PG.CBMSPassword(),
                                    seller_pan = PG.CompanyVATPan(),
                                    buyer_pan = SIMEnt.COSTOMER_PAN_VAT,
                                    buyer_name = SIMEnt.CUSTOMER_NAME,
                                    fiscal_year = PGD.CBMSFY(SIMEnt.INVOICE_FY), // "2076.077",                            
                                    invoice_number = SIMEnt.INVOICE_NUMBER,
                                    invoice_date = SIMEnt.INVOICE_YEAR + "." + SIMEnt.INVOICE_MONTH + "." + SIMEnt.INVOICE_DAY, // "2077.07.06",
                                    total_sales = Convert.ToDouble(SIMEnt.TAXABLE_SUB_TOTAL),
                                    taxable_sales_vat = taxable_sub_total,
                                    vat = Convert.ToDouble(SIMEnt.TAX_VAT_AMOUNT),
                                    excisable_amount = 0,
                                    excise = 0,
                                    taxable_sales_hst = 0,
                                    hst = 0,
                                    amount_for_esf = 0,
                                    esf = 0,
                                    export_sales = 0,
                                    tax_exempted_sales = nontaxable_sub_total,
                                    isrealtime = true,
                                    datetimeClient = DateTime.Now
                                };

                                client.BaseAddress = new Uri(PG.CBMSURL());
                                try
                                {
                                    var response = client.PostAsJsonAsync("api/bill", p).Result;

                                    if (response.IsSuccessStatusCode)
                                    {
                                        var result = response.Content.ReadAsStringAsync();
                                        Console.Write(result.Result);
                                        Console.ReadLine();
                                        SIMEnt.CBMS_PUSH = result.Result;
                                        if (result.Result == "200")
                                            SIMEnt.CBMS_PUSH_RT = "YES";
                                        else
                                            SIMEnt.CBMS_PUSH_RT = "NO";

                                        //104: model invalid
                                        //200: success
                                        //102: exception while saving credit note details
                                        //101: bill does not exists
                                        //100: API credentials do not match
                                        //103: Unknown exceptions
                                        //105: Bill does not exists (for Sales Return)
                                    }
                                    else
                                    {
                                        Console.Write("Error");
                                        Console.ReadLine();
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
                            #endregion
                        }
                        else
                        {
                            SIMEnt.CBMS_PUSH = "CBMS OFF";
                            SIMEnt.CBMS_PUSH_RT = "";
                            SIMSer.Update(SIMEnt);
                        }
                    }
                }
                #endregion
                #region sales return
                foreach (GridViewRow gr1 in gridCBMSSalesReturn.Rows)
                {
                    string Credit_note_number = "";
                    Label lblPkID = gr1.FindControl("lblPkID") as Label;
                    Label lblCreditNoteNumber = gr1.FindControl("lblCreditNoteNumber") as Label;
                    // Perform necessary actions...
                    SRMEnt = new SALES_RETURN_MASTER();
                    SRMEnt.PK_ID = lblPkID.Text;
                    lblCreditNoteNumber.Text = Credit_note_number;
                    SRMEnt = (SALES_RETURN_MASTER)SRMSer.GetSingle(SRMEnt);
                    if (SRMEnt != null)
                    {
                        if (PG.CBMSPush() == "ON")
                        {
                            #region to send in CBMS
                            //Sample Code CU for Posting Bill 
                            using (var client = new HttpClient())
                            {
                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                BillReturnViewModel p = new BillReturnViewModel
                                {
                                    username = PG.CBMSUsername(),
                                    password = PG.CBMSPassword(),
                                    seller_pan = PG.CompanyVATPan(),
                                    buyer_pan = SRMEnt.COSTOMER_PAN_VAT,
                                    buyer_name = SRMEnt.CUSTOMER_NAME,
                                    fiscal_year = PGD.CBMSFY(SRMEnt.NOTE_FY), // "2076.077",
                                    ref_invoice_number = SIMEnt.INVOICE_NUMBER,
                                    credit_note_date = SRMEnt.NOTE_YEAR + "." + SRMEnt.NOTE_MONTH + "." + SRMEnt.NOTE_DAY,//   "2074.07.06",
                                    credit_note_number = SRMEnt.CREDIT_NOTE_NUMBER,
                                    reason_for_return = SRMEnt.RETURN_REMARKS,
                                    total_sales = Convert.ToDouble(SRMEnt.SALES_RETURN_AMOUNT),
                                    taxable_sales_vat = Convert.ToDouble(SRMEnt.TAXABLE_AMOUNT),
                                    vat = Convert.ToDouble(SRMEnt.TAX_VAT_AMOUNT),
                                    excisable_amount = 0,
                                    excise = 0,
                                    taxable_sales_hst = 0,
                                    hst = 0,
                                    amount_for_esf = 0,
                                    esf = 0,
                                    export_sales = 0,
                                    tax_exempted_sales = 0,
                                    isrealtime = true,
                                    datetimeClient = DateTime.Now
                                };

                                client.BaseAddress = new Uri(PG.CBMSURL());
                                try
                                {
                                    var response = client.PostAsJsonAsync("api/billreturn", p).Result;
                                    if (response.IsSuccessStatusCode)
                                    {
                                        var result = response.Content.ReadAsStringAsync();
                                        SRMEnt.CBMS_PUSH = result.Result;
                                        if (result.Result == "200")
                                            SRMEnt.CBMS_PUSH_RT = "YES";
                                        else
                                            SRMEnt.CBMS_PUSH_RT = "NO";
                                    }
                                    else
                                    {
                                        Console.Write("Error");
                                        Console.ReadLine();
                                        SRMEnt.CBMS_PUSH = "Error";
                                        SRMEnt.CBMS_PUSH_RT = "NO";
                                    }
                                }
                                catch
                                {
                                    SRMEnt.CBMS_PUSH = "Offline";
                                    SRMEnt.CBMS_PUSH_RT = "NO";
                                }

                                SRMSer.Update(SRMEnt);
                            }

                            #endregion
                        }
                        else
                        {
                            SRMEnt.CBMS_PUSH = "CBMS OFF";
                            SRMEnt.CBMS_PUSH_RT = "";
                            SRMSer.Update(SRMEnt);
                        }
                    }
                }
                #endregion
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    
    }
    

    protected void LoadMonthlyTopSalesChart(bool isViewAll)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string locationId = isViewAll ? null : userProfile.LocationID;

        DataTable monthlyTopSalesData = GF.monthlyTopSales(locationId);
        string Cjson = JsonConvert.SerializeObject(monthlyTopSalesData);
        string script = "renderTopSalesChart(" + Cjson + ");";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadMonthlyTopSalesChart", script, true);
    }

    private void LoadDailySales(bool isViewAll)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string locationId = isViewAll ? null : userProfile.LocationID;

        DataTable dailySalesData = GF.DailySales("", locationId);
        string json = JsonConvert.SerializeObject(dailySalesData);
        string script = "renderSalesChart(" + json + ");";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadDailySalesChart", script, true);
    }

    private void LoadDailyCashSales(bool isViewAll)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string locationId = isViewAll ? null : userProfile.LocationID;

        DataTable dailyCashSalesData = GF.DailySales("CS", locationId);
        string json = JsonConvert.SerializeObject(dailyCashSalesData);
        string script = "renderCashSalesChart(" + json + ");";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadDailyCashSalesChart", script, true);
    }


    private void LoadDailyCreditSales(bool isViewAll)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string locationId = isViewAll ? null : userProfile.LocationID;

        DataTable dailyCreditSalesData = GF.DailySales("CR", locationId);
        string json = JsonConvert.SerializeObject(dailyCreditSalesData);
        string script = "renderCreditSalesChart(" + json + ");";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadDailyCreditSalesChart", script, true);
    }


    private void LoadDailyPurchases(bool isViewAll)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string locationId = isViewAll ? null : userProfile.LocationID;

        DataTable dailyPurchasesData = GF.DailyPurchase("", locationId);
        string json = JsonConvert.SerializeObject(dailyPurchasesData);
        string script = "renderPurchasesChart(" + json + ");";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadDailyPurchasesChart", script, true);
    }


    private void LoadCashPurchases(bool isViewAll)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string locationId = isViewAll ? null : userProfile.LocationID;

        DataTable dailyPurchasesData = GF.DailyPurchase("", locationId);
        string json = JsonConvert.SerializeObject(dailyPurchasesData);
        string script = "renderPurchasesChart(" + json + ");";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadDailyPurchasesChart", script, true);
    }

    private void LoadDailyCashPurchases(bool isViewAll)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string locationId = isViewAll ? null : userProfile.LocationID;

        DataTable dailyCashPurchasesData = GF.DailyPurchase("CP", locationId);
        string json = JsonConvert.SerializeObject(dailyCashPurchasesData);
        string script = "renderCashPurchasesChart(" + json + ");";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadDailyCashPurchasesChart", script, true);
    }

    private void LoadDailyCreditPurchases(bool isViewAll)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string locationId = isViewAll ? null : userProfile.LocationID;

        DataTable dailyCreditPurchasesData = GF.DailyPurchase("CR", locationId);
        string json = JsonConvert.SerializeObject(dailyCreditPurchasesData);
        string script = "renderCreditPurchasesChart(" + json + ");";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadDailyCreditPurchasesChart", script, true);
    }


    private void LoadOperatingCostLastSix(bool isViewAll)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string locationId = isViewAll ? null : userProfile.LocationID;

        DataTable costData = GF.OperatingCostLastSix(locationId);
        string json = JsonConvert.SerializeObject(costData);
        string script = "renderOperatingCostLastSix(" + json + ");";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadOperatingCostLastSix", script, true);
    }


    private void LoadCashFlowData(bool isViewAll)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string locationId = isViewAll ? null : userProfile.LocationID;

        DataTable cashFlowData = GF.CashFlowThisLastMth(locationId);
        string json = JsonConvert.SerializeObject(cashFlowData);
        string script = "renderCashFlowChart(" + json + ");";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadCashFlowChart", script, true);
    }

    private void LoadTopCreditors(bool isViewAll)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string locationId = isViewAll ? null : userProfile.LocationID;

        DataTable topCreditorsData = GF.GetTopDebtorsCreditors("01", "010301", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), locationId);
        string json = JsonConvert.SerializeObject(topCreditorsData);
        string script = "renderTopCreditorsChart(" + json + ");";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadTopCreditorsChart", script, true);
    }


    private void LoadTopDebtors(bool isViewAll)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string locationId = isViewAll ? null : userProfile.LocationID;

        DataTable topDebtorsData = GF.GetTopDebtorsCreditors("04", "040301", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), locationId);
        string json = JsonConvert.SerializeObject(topDebtorsData);
        string script = "renderTopDebtorsChart(" + json + ");";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "LoadTopDebtorsChart", script, true);
    }


    public class BillReturnViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string seller_pan { get; set; }
        public string buyer_pan { get; set; }
        public string fiscal_year { get; set; }
        public string buyer_name { get; set; }
        public string ref_invoice_number { get; set; }
        public string credit_note_number { get; set; }
        public string credit_note_date { get; set; }
        public string reason_for_return { get; set; }
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

    protected void gridCBMS_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblInvoiceNo = (Label)e.Row.FindControl("lblInvoiceNo");
            Label lblPkID = (Label)e.Row.FindControl("lblPkID");
            Label lblInvoiceDate = (Label)e.Row.FindControl("lblInvoiceDate");
            Label lblCustomerName = (Label)e.Row.FindControl("lblCustomerName");
            Label lblInvoiceAmount = (Label)e.Row.FindControl("lblInvoiceAmount");

            SIMEnt = new SALES_INVOICE_MASTER();
            SIMEnt.PK_ID = lblPkID.Text;
            SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
            if (SIMEnt != null)
            {
                if (SIMEnt.CBMS_PUSH != "200" || SIMEnt.CBMS_PUSH != "CBMS OFF")
                {
                    lblInvoiceNo.Text = SIMEnt.INVOICE_NUMBER;
                    lblInvoiceAmount.Text = SIMEnt.INVOICE_AMOUNT;
                    lblCustomerName.Text = SIMEnt.CUSTOMER_NAME;
                    lblInvoiceDate.Text = SIMEnt.INVOICE_DATE;
                    lblInvoiceNo.Text = SIMEnt.INVOICE_NUMBER;

                }
            }

        }

    }
    protected void gridCBMSSalesReturn_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblSalesInvoiceID = (Label)e.Row.FindControl("lblSalesInvoiceID");
            Label lblPkID = (Label)e.Row.FindControl("lblPkID");
            Label lblNoteDate = (Label)e.Row.FindControl("lblNoteDate");
            Label lblCustomerName = (Label)e.Row.FindControl("lblCustomerName");
            Label lblSalesReturnAmount = (Label)e.Row.FindControl("lblSalesReturnAmount");
            Label lblCreditNoteNumber = (Label)e.Row.FindControl("lblCreditNoteNumber");

            SRMEnt = new SALES_RETURN_MASTER();
            SRMEnt.PK_ID = lblPkID.Text;
            SRMEnt = (SALES_RETURN_MASTER)SRMSer.GetSingle(SRMEnt);
            if (SRMEnt != null)
            {
                if (SRMEnt.CBMS_PUSH != "200" || SRMEnt.CBMS_PUSH != "CBMS OFF")
                {
                    lblSalesInvoiceID.Text = SRMEnt.SALES_INVOICE_ID;
                    lblSalesReturnAmount.Text = SRMEnt.SALES_RETURN_AMOUNT;
                    lblCustomerName.Text = SRMEnt.CUSTOMER_NAME;
                    lblNoteDate.Text = SRMEnt.NOTE_DATE;
                    lblCreditNoteNumber.Text = SRMEnt.CREDIT_NOTE_NUMBER;
                }
            }

        }

    }
    protected void LoadGrid()
    {
        if (PG.IS_CBMS_Approved())
        {
            gridCBMS.DataSource = hf.getUNSINK_INVOICE(PG.CBMS_Approved_Date());
            gridCBMS.DataBind();
        }
    }
    protected void LoadReturnGrid()
    {
        if (PG.IS_CBMS_Approved())
        {
            gridCBMSSalesReturn.DataSource = hf.getUNSINK_CREDITNOTE(PG.CBMS_Approved_Date());
            gridCBMSSalesReturn.DataBind();
        }
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

    protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect("utilities/purchase/productpurchase.aspx");
    }

    protected void ImageButton2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect("utilities/sales/productsales.aspx");
    }

    protected void ImageButton5_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect("administration/customer.aspx");
    }

    protected void ImageButton6_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect("administration/supplier.aspx");
    }

    protected void ImageButton7_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect("administration/product.aspx");
    }

    protected void ImageButton3_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect("reports/purchase/purchasereport.aspx");
    }

    protected void ImageButton4_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect("reports/sales/dailysaleschallan.aspx");
    }

    protected void ImageButton8_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect("account/reports/sundrydebitorsbalance.aspx");
    }

    protected void ImageButton9_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect("account/reports/sundrycreditorsbalance.aspx");
    }

    protected void ImageButton10_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect("account/reports/account_ledger.aspx");
    }

    private void LoadAdmissionDischargeLineGraph()
    {



        // Initialize an empty row for each day (1 to 32)
        Dictionary<int, DataRow> dayRows = new Dictionary<int, DataRow>();
        DataTable DT = new DataTable();
        DT.Columns.Add("Day");
        DT.Columns.Add("SalesDate");
        DT.Columns.Add("PurchaseDate");


        for (int i = 1; i <= 32; i++)
        {
            DataRow newRow = DT.NewRow();
            newRow["Day"] = i.ToString();
            newRow["SalesDate"] = i.ToString() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
            newRow["PurchaseDate"] = i.ToString() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
            DT.Rows.Add(newRow);
            dayRows[i] = newRow;
        }



        GridView1.DataSource = DT;
        GridView1.DataBind();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblSalesDate = (Label)e.Row.FindControl("lblSalesDate");
            Label lblPurchaseDate = (Label)e.Row.FindControl("lblPurchaseDate");
            Label lblCashPurchase = (Label)e.Row.FindControl("lblCashPurchase");
            Label lblCreditPurchase = (Label)e.Row.FindControl("lblCreditPurchase");
            Label lblCashSales = (Label)e.Row.FindControl("lblCashSales");
            Label lblCreditSales = (Label)e.Row.FindControl("lblCreditSales");

            string[] Adate = lblSalesDate.Text.Split('/');
            string[] Ddate = lblPurchaseDate.Text.Split('/');
            int[] AdateInt = Array.ConvertAll(Adate, int.Parse);
            int[] DdateInt = Array.ConvertAll(Ddate, int.Parse);
            string cashSalesDay, cashSalesMonth, cashSalesYear;
            string cashPurchaseDay, cashPurchaseMonth, cashPurchaseYear;

            cashSalesDay = AdateInt[0].ToString("#00");
            cashSalesMonth = AdateInt[1].ToString("#00");
            cashSalesYear = AdateInt[2].ToString();

            cashPurchaseDay = DdateInt[0].ToString("#00");
            cashPurchaseMonth = DdateInt[1].ToString("#00");
            cashPurchaseYear = DdateInt[2].ToString();

            if (ToggleSwitch.Checked)
            {
            userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                lblCashSales.Text = GF.getmonthlySalesSummary(cashSalesDay, cashSalesMonth, cashSalesYear, null);
                lblCashPurchase.Text = GF.getmonthlyPurchaseSummary(cashPurchaseDay, cashPurchaseMonth, cashPurchaseYear, null);
            }
            else
            {
                lblCashSales.Text = GF.getmonthlySalesSummary(cashSalesDay, cashSalesMonth, cashSalesYear, userProfile.LocationID);
                lblCashPurchase.Text = GF.getmonthlyPurchaseSummary(cashPurchaseDay, cashPurchaseMonth, cashPurchaseYear, userProfile.LocationID);
            }
           
        }
    }

    protected void ToggleSwitch_CheckedChanged(object sender, EventArgs e)
    {
        if (ToggleSwitch.Checked)  // English Selected
        {
            bool isViewAll = ToggleSwitch.Checked;

            LoadMonthlyTopSalesChart(isViewAll);
            LoadDailySales(isViewAll);
            LoadDailyCashSales(isViewAll);
            LoadDailyCreditSales(isViewAll);
            LoadDailyPurchases(isViewAll);
            LoadCashPurchases(isViewAll);
            LoadDailyCashPurchases(isViewAll);
            LoadDailyCreditPurchases(isViewAll);
            LoadOperatingCostLastSix(isViewAll);
            LoadCashFlowData(isViewAll);
            LoadTopCreditors(isViewAll);
            LoadTopDebtors(isViewAll);
            LoadGrid(); 
            LoadReturnGrid();
            LoadAdmissionDischargeLineGraph();
        }
        else  // Nepali Selected
        {
            bool isViewAll = ToggleSwitch.Checked;

            LoadMonthlyTopSalesChart(isViewAll);
            LoadDailySales(isViewAll);
            LoadDailyCashSales(isViewAll);
            LoadDailyCreditSales(isViewAll);
            LoadDailyPurchases(isViewAll);
            LoadCashPurchases(isViewAll);
            LoadDailyCashPurchases(isViewAll);
            LoadDailyCreditPurchases(isViewAll);
            LoadOperatingCostLastSix(isViewAll);
            LoadCashFlowData(isViewAll);
            LoadTopCreditors(isViewAll);
            LoadTopDebtors(isViewAll);
            LoadGrid();
            LoadReturnGrid();
            LoadAdmissionDischargeLineGraph();
        }
    }
}