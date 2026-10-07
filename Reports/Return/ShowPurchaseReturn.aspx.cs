using Entity.Components;
using PhyeGanCore;
using Service.Components;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Reports_Return_ShowPurchaseReturn : System.Web.UI.Page
{
    PURCHASE_INVOICE_MASTER PIMEnt = new PURCHASE_INVOICE_MASTER();
    PURCHASE_INVOICE_MASTERService PIMSer = new PURCHASE_INVOICE_MASTERService();

    PURCHASE_INVOICE_DETAIL PIDEnt = new PURCHASE_INVOICE_DETAIL();
    PURCHASE_INVOICE_DETAILService PIDSer = new PURCHASE_INVOICE_DETAILService();

    PURCHASE_RETURN_MASTER PRMEnt = new PURCHASE_RETURN_MASTER();
    PURCHASE_RETURN_MASTERService PRMSer = new PURCHASE_RETURN_MASTERService();

    PURCHASE_RETURN_DETAIL PRDEnt = new PURCHASE_RETURN_DETAIL();
    PURCHASE_RETURN_DETAILService PRDSer = new PURCHASE_RETURN_DETAILService();

    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();
    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfile = new UserProfileEntity();
    Boolean IsPageRefresh = false;
    string invno = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            invno = Request.QueryString["invno"].ToString();

            if (invno != "")
            {

                LoadToPrint(invno);
                SetVisibility();
            }
        }
    }

    #region to print bill

    
    protected void cleardata()
    {
        lblDebitNoteNo.Text = "";
        lblSupplierName.Text = "";
       
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
               PRMEnt = new PURCHASE_RETURN_MASTER();
        PRMEnt.PK_ID = pk_id;
        PRMEnt = (PURCHASE_RETURN_MASTER)PRMSer.GetSingle(PRMEnt);
        if (PRMEnt != null)
        {



            gridReturnInvoice.DataSource = hf.LoadPurchaseReturnInvoice(pk_id);
            gridReturnInvoice.DataBind();
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
            lblAmountInWord.Text = hf.NumWordsWrapper(Convert.ToDouble(PRMEnt.RETURN_AMOUNT)) + " only";//.ToUpper()

        }

        printdetail.Visible = true;
    }

    #endregion
    protected void SetVisibility()
    {
        trInvRoundOff.Visible = PGPS.RoundOff();// whether or not to show round off on inv
        trInvRoTot.Visible = PGPS.RoundOff();
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        string script = "window.close();";
        ScriptManager.RegisterStartupScript(this, GetType(), "CloseTab", script, true);
    }
}