using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using PhyeGanCore;
using Service.Components;
public partial class Reports_Return_ShowSalesReturn : System.Web.UI.Page
{
    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();

    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();


    SALES_INVOICE_MASTER SIMEnt = new SALES_INVOICE_MASTER();
    SALES_INVOICE_MASTERService SIMSer = new SALES_INVOICE_MASTERService();

    SALES_INVOICE_DETAIL SIDEnt = new SALES_INVOICE_DETAIL();
    SALES_INVOICE_DETAILService SIDSer = new SALES_INVOICE_DETAILService();

    SALES_RETURN_MASTER SRMEnt = new SALES_RETURN_MASTER();
    SALES_RETURN_MASTERService SRMSer = new SALES_RETURN_MASTERService();

    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfile = new UserProfileEntity();
    Boolean IsPageRefresh = false;

    string invno = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        invno = Request.QueryString["invno"].ToString();

        if (invno != "")
        {
            SRMEnt = new SALES_RETURN_MASTER();
            SRMEnt.SALES_INVOICE_ID = invno;
            SRMEnt = (SALES_RETURN_MASTER)SRMSer.GetSingle(SRMEnt);
            if (SRMEnt != null)
            {
                PrintCreditNote(SRMEnt.PK_ID);
                printdetail.Visible = true;
                SetVisibility();
            }
        }
    }
    protected void SetVisibility()
    {
        trInvRoundOff.Visible = PGPS.RoundOff();// whether or not to show round off on inv
        trInvRoTot.Visible = PGPS.RoundOff();
        gridSalesRetInvoice.Columns[2].Visible = PGPS.ShowDualQuantity();//Upper Qty 

        gridSalesRetInvoice.Columns[6].Visible = PGPS.ItemWiseDiscount(); // Item wise discount
        gridSalesRetInvoice.Columns[7].Visible = PGPS.ItemWiseDiscount(); // Amount After Item wise discount
    }
    protected void LoadCompanyDetail()
    {
        lblCompanyName.Text = PG.CompanyName();
        lblCompanyAddress.Text = PG.CompanyAddress();
        lblEmail.Text = PG.CompanyEmail();
        lblWebsite.Text = PG.CompanyWebsite();
        if (PG.CompanyEmail() == "" && PG.CompanyWebsite() == "")
        {
            divEmail.Visible = false;
        }
        lblPanNo.Text = PG.CompanyVATPan();
        lblPhone.Text = PG.CompanyContact();

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



    }
    protected void PrintCreditNote(string PK_ID)
    {
        LoadCompanyDetail();
        SRMEnt = new SALES_RETURN_MASTER();
        SRMEnt.PK_ID = PK_ID;
        SRMEnt = (SALES_RETURN_MASTER)SRMSer.GetSingle(SRMEnt);
        if (SRMEnt != null)
        {
            lblCNInvoiceNumber.Text = SRMEnt.CREDIT_NOTE_NUMBER;
            lblInvoiceNepaliDate.Text = SRMEnt.NOTE_DAY + "/" + SRMEnt.NOTE_MONTH + "/" + SRMEnt.NOTE_YEAR;
            lblInvoiceEnglishDate.Text = SRMEnt.NOTE_DATE;

            gridSalesRetInvoice.DataSource = hf.LoadSalesReturnInvoice(PK_ID);
            gridSalesRetInvoice.DataBind();

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
            lblInvCreatedBy.Text = SRMEnt.USER_ID;


        }

    }


    protected void btn_back_Click(object sender, EventArgs e)
    {
        string script = "window.close();";
        ScriptManager.RegisterStartupScript(this, GetType(), "CloseTab", script, true);
    }

}