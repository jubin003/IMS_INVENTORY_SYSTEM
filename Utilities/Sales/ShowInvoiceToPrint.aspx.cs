using Entity.Components;
using PhyeGanCore;
using Service.Components;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Utilities_Sales_ShowInvoiceToPrint : System.Web.UI.Page
{
    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

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

    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfile = new UserProfileEntity();
    Boolean IsPageRefresh = false;

    string invno = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                invno = Request.QueryString["invno"].ToString();

                if (invno != "")
                {
                    SIMEnt = new SALES_INVOICE_MASTER();
                    SIMEnt.PK_ID = invno;
                    SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
                    if (SIMEnt != null)
                    {
                        LoadToPrint(SIMEnt.PK_ID);
                        ShowImage(SIMEnt.PK_ID);
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
        //blInvoiceHeading1.Text = "Copy of Original: " + SIMEnt.COPY_PRINTNO;
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
            lblVATPercent.Text = SIMEnt.TAX_VAT_AMOUNT == "0" ? "0" : (Convert.ToDouble(PG.CompanyTAXPercent()).ToString("00.00"));
            lblVATAmount.Text = Convert.ToDouble(SIMEnt.TAX_VAT_AMOUNT).ToString("##,##0.00");
            lblGTotal.Text = Convert.ToDouble(SIMEnt.GRAND_TOTAL).ToString("##,##0.00");
            lblRoundoff.Text = Convert.ToDouble(SIMEnt.ROUND_OFF).ToString("##,##0.00");
            lblBillAmount.Text = Convert.ToDouble(SIMEnt.INVOICE_AMOUNT).ToString("##,##0.00");
            lblAmountInWord.Text = hf.NumWordsWrapper(Convert.ToDouble(SIMEnt.INVOICE_AMOUNT)) + " only";//.ToUpper()
            lblForCompanyName.Text = PG.CompanyName();
            lblPONumber.Text = SIMEnt.PO_NUMBER;
            if (SIMEnt.REMARKS != "")
                lblRemarks.Text = "* " + SIMEnt.REMARKS;
            SetGridColumnVisibility();

            gridSalesInvoice.DataSource = hf.LoadSalesInvoice(SIMEnt.PK_ID);
            gridSalesInvoice.DataBind();
            printdetail.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
        }
    }
    private void SetGridColumnVisibility()
    {
        gridSalesInvoice.Columns[3].Visible = PGPS.ProductBatch();
        gridSalesInvoice.Columns[4].Visible = PGPS.ProductExpDate(); ;
        gridSalesInvoice.Columns[5].Visible = PGPS.ShowDualQuantity();//Upper Qty 
        //6- Qty
        //7- Rate
        //8- Amount
        gridSalesInvoice.Columns[9].Visible = PGPS.ItemWiseDiscount(); // Item wise discount
        gridSalesInvoice.Columns[10].Visible = PGPS.ItemWiseDiscount(); // Amount After Item wise discount
        if (PG.CompanyTAXType() != "VAT")//whether or not to show vat on bill
        {
            trInvVat.Visible = false;
        }
        trInvRoundOff.Visible = PGPS.RoundOff();// whether or not to show round off on inv
        if (PG.CompanyTAXType() == "None")
        {
            trTaxableAmt.Visible = false;
        }
        if (!PGPS.RoundOff() && PG.CompanyTAXType() == "None")
        {
            trTaxableAmt.Visible = false;
            trNetAmount.Visible = false;
        }
        divPO.Visible = PGPS.ShowPO();
    }
    protected void ShowImage(string pk_id)
    {

        string folderVirtualPath = "~/images/Purchae_Bill_Img/";
        string folderPhysicalPath = Server.MapPath(folderVirtualPath);
        string[] supportedExtensions = { ".png", ".jpg", ".jpeg" };
        string imgVirtualPath;
        foreach (var extension in supportedExtensions)
        {
            string fileName = "Purchase_Bill" + pk_id + extension;
            string filePhysicalPath = System.IO.Path.Combine(folderPhysicalPath, fileName);

            if (File.Exists(filePhysicalPath))
            {
                imgVirtualPath = folderVirtualPath + fileName;
                ImgPurchaseBill.ImageUrl = imgVirtualPath;
                divImg.Visible = true;
            }
        }



    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        string script = "window.close();";
        ScriptManager.RegisterStartupScript(this, GetType(), "CloseTab", script, true);
    }
}