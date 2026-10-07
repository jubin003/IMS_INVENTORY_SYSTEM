using Entity.Components;
using PhyeGanCore;
using Service.Components;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Reports_Purchase_ShowPurchaseInvoice : System.Web.UI.Page
{
    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();

    PRODUCT_SETTING PSEnt = new PRODUCT_SETTING();
    PRODUCT_SETTINGService PSSer = new PRODUCT_SETTINGService();

    PURCHASE_INVOICE_MASTER PIMEnt = new PURCHASE_INVOICE_MASTER();
    PURCHASE_INVOICE_MASTERService PIMSer = new PURCHASE_INVOICE_MASTERService();

    PURCHASE_INVOICE_DETAIL PIDEnt = new PURCHASE_INVOICE_DETAIL();
    PURCHASE_INVOICE_DETAILService PIDSer = new PURCHASE_INVOICE_DETAILService();

    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfile = new UserProfileEntity();
    Boolean IsPageRefresh = false;

    string dakhno = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                dakhno = Request.QueryString["dakhno"].ToString();

                if (dakhno != "")
                {
                    PIMEnt = new PURCHASE_INVOICE_MASTER();
                    PIMEnt.PK_ID = dakhno;
                    PIMEnt = (PURCHASE_INVOICE_MASTER)PIMSer.GetSingle(PIMEnt);
                    if (PIMEnt != null)
                    {
                        LoadToPrint(PIMEnt.PK_ID);

                    }
                }
            }
            catch (Exception ee)
            {
                HelperFunction.MsgBox(this, this.GetType(), ee.ToString());
            }
        }
    }
    protected void cleardata()
    {
        lblInvoiceNo.Text = "";
        lblCustomerName.Text = "";
        lblTranDate.Text = "";
        lblTranNepaliDate.Text = "";
        lblBillEnglishDate.Text = "";
        lblBillNepaliDate.Text = "";

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
        lblInvoiceHeading.Text = "INVOICE Detail";
        //blInvoiceHeading1.Text = "Copy of Original: " + PIMEnt.COPY_PRINTNO;
        PIMEnt = new PURCHASE_INVOICE_MASTER();
        PIMEnt.PK_ID = pk_id;
        PIMEnt = (PURCHASE_INVOICE_MASTER)PIMSer.GetSingle(PIMEnt);
        if (PIMEnt != null)
        {
            lblInvoiceNo.Text = PIMEnt.SUPPLIER_INVOICE_NO;
            lblDakhilaNo.Text = PIMEnt.DAKHILA_NUMBER;
            lblCustomerName.Text = PIMEnt.SUPPLIER_NAME;
            lblAddress.Text = PIMEnt.SUPPLIER_ADDRESS;
            lblCustomerPanNo.Text = PIMEnt.SUPPLIER_PAN_VAT;
            lblModeofPayment.Text = hf.getPaymentType(PIMEnt.PURCHASE_TYPE_ID);
            lblBillEnglishDate.Text = PIMEnt.INVOICE_DATE;
            lblBillNepaliDate.Text = PIMEnt.INVOICE_DAY + "/" + PIMEnt.INVOICE_MONTH + "/" + PIMEnt.INVOICE_YEAR;
            lblTranDate.Text = PIMEnt.DAKHILA_DATE;
            lblTranNepaliDate.Text = PIMEnt.DAKHILA_DAY + "/" + PIMEnt.DAKHILA_MONTH + "/" + PIMEnt.DAKHILA_YEAR;
            lblPrintedBy.Text = hf.getEmployeeName(PIMEnt.USER_ID);
            //lblTime.Text = PIMEnt.PRINT_TIME;
            lblInvCreatedBy.Text = hf.getEmployeeName(PIMEnt.USER_ID);
            lblBillSubTotal.Text = Convert.ToDouble(PIMEnt.SUB_TOTAL_AMOUNT).ToString("##,##0.00");
            lblBillDiscountPercent.Text = Convert.ToDouble(PIMEnt.DISCOUNT_PERCENT).ToString("#0.00");
            lblDiscount.Text = Convert.ToDouble(PIMEnt.DISCOUNT_AMOUNT).ToString("##,##0.00");
            lblTaxableAmount.Text = (Convert.ToDouble(PIMEnt.SUB_TOTAL_AMOUNT).ToString("##,##0.00"));
            lblVATPercent.Text = PIMEnt.TAX_VAT_AMOUNT == "0" ? "0" : (Convert.ToDouble(PG.CompanyTAXPercent()).ToString("00.00"));
            lblVATAmount.Text = Convert.ToDouble(PIMEnt.TAX_VAT_AMOUNT).ToString("##,##0.00");
            lblGTotal.Text = Convert.ToDouble(PIMEnt.GRAND_TOTAL).ToString("##,##0.00");
            lblRoundoff.Text = Convert.ToDouble(PIMEnt.ROUND_OFF).ToString("##,##0.00");
            lblBillAmount.Text = Convert.ToDouble(PIMEnt.INVOICE_AMOUNT).ToString("##,##0.00");
            lblAmountInWord.Text = hf.NumWordsWrapper(Convert.ToDouble(PIMEnt.INVOICE_AMOUNT)) + " only";//.ToUpper()
            lblForCompanyName.Text = PG.CompanyName()+" <br/> "+ PG.BranchAddress(PIMEnt.OFFICE_CODE);
            //lblPONumber.Text = PIMEnt.PO_NUMBER;
            //if (PIMEnt.REMARKS != "")
            //    lblRemarks.Text = "* " + PIMEnt.REMARKS;
            SetGridColumnVisibility();

            gridSalesInvoice.DataSource = hf.LoadPurchaseInvoice(PIMEnt.PK_ID);
            gridSalesInvoice.DataBind();
            printdetail.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
        }
    }
    private void SetGridColumnVisibility()
    {
        gridSalesInvoice.Columns[3].Visible = PGPS.ProductBatch();
        gridSalesInvoice.Columns[4].Visible = PGPS.ProductExpDate(); ;
        gridSalesInvoice.Columns[5].Visible = PGPS.ShowDualQuantity();
        trInvRoTot.Visible = PGPS.RoundOff();
        trInvRoundOff.Visible = PGPS.RoundOff();
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        string script = "window.close();";
        ScriptManager.RegisterStartupScript(this, GetType(), "CloseTab", script, true);
    }
}