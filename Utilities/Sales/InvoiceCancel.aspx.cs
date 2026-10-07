using System;
using System.Web;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DataHelper.Framework;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.UI;


public partial class Utilities_Sales_InvoiceCancel : System.Web.UI.Page
{
    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();
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

    VOUCHER_MASTER VMEnt = new VOUCHER_MASTER();
    VOUCHER_MASTERService VMSer = new VOUCHER_MASTERService();

    VOUCHER_CHILD VCEnt = new VOUCHER_CHILD();
    VOUCHER_CHILDService VCSer = new VOUCHER_CHILDService();
    BANK_ACCOUNT BAEnt = new BANK_ACCOUNT();
    BANK_ACCOUNTService BASer = new BANK_ACCOUNTService();

    GL_SUB_ACCOUNT GLSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GLSASer = new GL_SUB_ACCOUNTService();

    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();
    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfile = new UserProfileEntity();
    Boolean IsPageRefresh = false;
    AccountFunction af = new AccountFunction();
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
                    lblunique_token.Text = userProfile.UserName + hf.getmaxCNid(); // this code is for not allowing double entry of CN bill
                    SetVisibility();
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
        LoadInvoiceGrid();
    }
    protected void LoadInvoiceGrid()
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (chkDateWise.Checked == true)
        {
            grdSales.DataSource = hf.getSalesList("", "", PGD.GetEnglishDateFromNepali(txtFromDate.Text, "dd/mm/yyyy"), PGD.GetEnglishDateFromNepali(txtToDate.Text, "dd/mm/yyyy"),userProfile.LocationID);
        }
        else
        {
            grdSales.DataSource = hf.getSalesList(ddlFiscalYear.SelectedValue, txtInvoiceNo.Text, "", "", userProfile.LocationID);
        }
        grdSales.DataBind();
        if (grdSales.Rows.Count == 0 && txtInvoiceNo.Text != null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Invalid Invoice Number or this Invoice have been already Cancel.");
        }
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
    protected void grdSales_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Alter"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
            lblPK_ID_Hiden.Text = lblPK_ID.Text;

            LoadInvoiceDetail(lblPK_ID.Text);
            txtCreditNoteDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
            divInvoiceDetail.Visible = true;
            divInitial.Visible = false;


            if (lblModeofPayment.Text == "QR")
            {
                LoadPaymentType();
                divSalesReturnType.Visible = true;
            }

        }
    }
    protected void LoadPaymentType()
    {
        PTEnt = new PAYMENT_TYPE();
        PTEnt.SALES_PURCHASE = "CN";
        PTEnt.STATUS = "1";
        ddlCNType.DataSource = PTSEr.GetAll(PTEnt);
        ddlCNType.DataTextField = "PAYMENT_NAME";
        ddlCNType.DataValueField = "PAYMENT_CODE";
        ddlCNType.DataBind();
    }

    protected void LoadBank()
    {
        BAEnt = new BANK_ACCOUNT();
        BAEnt.SHOW_IN_RECEIPT = "1";
        ddlBank.DataSource = BASer.GetAll(BAEnt);
        ddlBank.DataTextField = "LEDGER_NAME";
        ddlBank.DataValueField = "BANK_CODE";
        ddlBank.DataBind();
    }
    protected void LoadInvoiceDetail(string PK_ID)
    {
        cleardata();
        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.PK_ID = PK_ID;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
        if (SIMEnt != null)
        {
            lblInvoiceNo.Text = SIMEnt.INVOICE_NUMBER;
            lblCustomerID.Text = SIMEnt.CUSTOMER_ID;
            lblCustomerName.Text = SIMEnt.CUSTOMER_NAME;
            lblAddress.Text = SIMEnt.CUSTOMER_ADDRESS;
            lblCustomerPanNo.Text = SIMEnt.COSTOMER_PAN_VAT;
            lblModeofPayment.Text = hf.getPaymentType(SIMEnt.SALES_TYPE_ID);
            lblBillEnglishDate.Text = SIMEnt.INVOICE_DATE;
            lblBillNepaliDate.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
            lblTranDate.Text = SIMEnt.TRANSACTION_DATE;
            lblTranNepaliDate.Text = SIMEnt.TRANSACTION_DAY + "/" + SIMEnt.TRANSACTION_MONTH + "/" + SIMEnt.TRANSACTION_YEAR;
            lblBillSubTotal.Text = Convert.ToDouble(SIMEnt.TAXABLE_SUB_TOTAL).ToString("#0.00");
            lblBillDiscountPercent.Text = Convert.ToDouble(SIMEnt.DISCOUNT_PERCENT).ToString("#0.00");
            lblDiscount.Text = Convert.ToDouble(SIMEnt.TAXABLE_DISC_AMOUNT).ToString("#0.00");
            lblTaxableAmount.Text = (Convert.ToDouble(SIMEnt.TAXABLE_TOTAL).ToString("#0.00"));
            lblVATPercent.Text = SIMEnt.TAX_VAT_AMOUNT == "0" ? "0" : (Convert.ToDouble(PG.CompanyTAXPercent()).ToString("00.00"));
            lblVATAmount.Text = Convert.ToDouble(SIMEnt.TAX_VAT_AMOUNT).ToString("#0.00");
            lblGTotal.Text = Convert.ToDouble(SIMEnt.GRAND_TOTAL).ToString("#0.00");
            lblRoundoff.Text = Convert.ToDouble(SIMEnt.ROUND_OFF).ToString("#0.00");
            lblBillAmount.Text = Convert.ToDouble(SIMEnt.INVOICE_AMOUNT).ToString("#0.00");
            lblAmountInWord.Text = hf.NumWordsWrapper(Convert.ToDouble(SIMEnt.INVOICE_AMOUNT)) + " only";//.ToUpper()
        }
        SIDEnt = new SALES_INVOICE_DETAIL();
        SIDEnt.SALES_INVOICE_ID = PK_ID;
        grdInvoiceDetail.DataSource = SIDSer.GetAll(SIDEnt);
        grdInvoiceDetail.DataBind();


        PSEnt = new PRODUCT_SETTING();
        PSEnt = (PRODUCT_SETTING)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            if (PSEnt.BATCH_NUMBER == "0")
                grdInvoiceDetail.Columns[2].Visible = false;
            if (PSEnt.EXPIRY_DATE == "0")
                grdInvoiceDetail.Columns[3].Visible = false;

        }

    }
    protected void grdInvoiceDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblProductId = e.Row.FindControl("lblProductId") as Label;
            Label lblProductName = e.Row.FindControl("lblProductName") as Label;
            Label lblProductCode = e.Row.FindControl("lblProductCode") as Label;
            Label lblUUnit = e.Row.FindControl("lblUUnit") as Label;
            Label lblUnit = e.Row.FindControl("lblUnit") as Label;
            PEnt = new PRODUCT();
            PEnt.PK_ID = lblProductId.Text;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                lblProductName.Text = PEnt.PRODUCT_NAME;
                lblProductCode.Text = PEnt.PRODUCT_CODE;
                lblUnit.Text = hf.getProductUnit(PEnt.PK_ID);
                lblUUnit.Text = hf.getProductUnitUpper(PEnt.PK_ID);
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divInvoiceDetail.Visible = false;
        divInitial.Visible = true;

    }
    protected void cleardata()
    {
        lblInvoiceNo.Text = "";
        lblCustomerID.Text = "";
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
        txtCreditNoteRemarks.Text = "";
        divBankDetail.Visible = false;

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string unique_token = "";
        SRMEnt = new SALES_RETURN_MASTER();
        SRMEnt.NEXT_CN_ID = lblunique_token.Text;
        SRMEnt = (SALES_RETURN_MASTER)SRMSer.GetSingle(SRMEnt);
        if (SRMEnt != null)
            unique_token = SRMEnt.NEXT_CN_ID;

        if (!IsPageRefresh && unique_token == "") // to check if it is post back 
        {
            string msg = "";
            if (txtCreditNoteRemarks.Text == "")
                msg = "Enter Return Remarks";
            if (msg == "")
            {
                DistributedTransaction DT = new DistributedTransaction();
                userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                SIMEnt = new SALES_INVOICE_MASTER();
                SIMEnt.PK_ID = lblPK_ID_Hiden.Text;
                SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
                if (SIMEnt != null)
                {
                    SIMEnt.CANCEL_STATUS = "1";
                    SIMSer.Update(SIMEnt, DT);

                    #region to insert in sales return master
                    SRMEnt = new SALES_RETURN_MASTER();
                    SRMEnt.NOTE_DAY = PGD.NepaliDay();
                    SRMEnt.NOTE_MONTH = PGD.NepaliMonth();
                    SRMEnt.NOTE_YEAR = PGD.NepaliYear();
                    SRMEnt.NOTE_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    SRMEnt.NOTE_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                    SRMEnt.NOTE_TIME = DateTime.Now.ToString("h:mm:ss tt");
                    SRMEnt.SALES_INVOICE_ID = SIMEnt.PK_ID;
                    SRMEnt.CUSTOMER_ID = SIMEnt.CUSTOMER_ID;
                    SRMEnt.CUSTOMER_NAME = SIMEnt.CUSTOMER_NAME;
                    SRMEnt.COSTOMER_PAN_VAT = SIMEnt.COSTOMER_PAN_VAT;
                    SRMEnt.CUSTOMER_ADDRESS = SIMEnt.CUSTOMER_ADDRESS;
                    SRMEnt.CUSTOMER_CONTACT_NUMBER = SIMEnt.CUSTOMER_CONTACT_NUMBER;
                    SRMEnt.SUB_TOTAL = SIMEnt.TAXABLE_SUB_TOTAL;
                    SRMEnt.DISCOUNT_PERCENT = SIMEnt.DISCOUNT_PERCENT;
                    SRMEnt.DISCOUNT_AMOUNT = SIMEnt.TAXABLE_DISC_AMOUNT;
                    SRMEnt.TAXABLE_AMOUNT = SIMEnt.TAXABLE_TOTAL;
                    SRMEnt.TAX_VAT_AMOUNT = SIMEnt.TAX_VAT_AMOUNT;
                    SRMEnt.GRAND_TOTAL = SIMEnt.GRAND_TOTAL;
                    SRMEnt.ROUND_OFF = SIMEnt.ROUND_OFF;
                    SRMEnt.SALES_RETURN_AMOUNT = SIMEnt.INVOICE_AMOUNT;
                    SRMEnt.USER_ID = userProfile.UserName;
                    SRMEnt.RETURN_REMARKS = txtCreditNoteRemarks.Text;
                    SRMEnt.NEXT_CN_ID = lblunique_token.Text;
                    SRMEnt.SALES_RETURN_TYPE = ddlCNType.SelectedValue;
                    SRMEnt.OFFICE_CODE = userProfile.LocationID;
                    string SR_PK_ID = SRMSer.Insert(SRMEnt, DT).ToString();
                    #endregion
                    #region to insert in sales return detail
                    foreach (GridViewRow gr in grdInvoiceDetail.Rows)
                    {
                        Label lblSno = gr.FindControl("lblSno") as Label;
                        Label lblProductId = gr.FindControl("lblProductId") as Label;
                        Label lblBatch = gr.FindControl("lblBatch") as Label;
                        Label lblExpiryDate = gr.FindControl("lblExpiryDate") as Label;
                        Label lblUQty = gr.FindControl("lblUQty") as Label;
                        Label lblQty = gr.FindControl("lblQty") as Label;
                        Label lblRate = gr.FindControl("lblRate") as Label;
                        Label lblAmount = gr.FindControl("lblAmount") as Label;
                        Label lblScheDisc = gr.FindControl("lblScheDisc") as Label;
                        Label lblAfterScheDisc = gr.FindControl("lblAfterScheDisc") as Label;

                        SRDEnt = new SALES_RETURN_DETAIL();
                        SRDEnt.SALES_RETURN_ID = SR_PK_ID;
                        SRDEnt.SNO = lblSno.Text;
                        SRDEnt.PRODUCT_ID = lblProductId.Text;
                        SRDEnt.BATCH_NUMBER = lblBatch.Text;
                        SRDEnt.EXPIRY_DATE = lblExpiryDate.Text;
                        SRDEnt.QUANTITY = lblQty.Text;
                        SRDEnt.UPPER_QUANTITY = lblUQty.Text;
                        SRDEnt.RATE = lblRate.Text;
                        SRDEnt.TOTAL = lblAmount.Text;
                        SRDEnt.SCHEME_DISCOUNT = lblScheDisc.Text;
                        SRDEnt.OFFICE_CODE = userProfile.LocationID;
                        if (SIMEnt.EXEMPTED == "0")//Taxable
                        {
                            SRDEnt.TAXABLE_TOTAL = (Convert.ToDouble(lblAfterScheDisc.Text)).ToString();
                            SRDEnt.NON_TAXABLE_TOTAL = "0";
                            SRDEnt.TAX_AMOUNT = ((Convert.ToDouble(lblAfterScheDisc.Text)) * Convert.ToDouble(PG.CompanyTAXPercent()) / 100).ToString();
                            SRDEnt.AMOUNT_AFTER_TAX = (Convert.ToDouble(lblAfterScheDisc.Text) + (Convert.ToDouble(lblAfterScheDisc.Text)) * Convert.ToDouble(PG.CompanyTAXPercent()) / 100).ToString();
                        }
                        else
                        {
                            SRDEnt.TAXABLE_TOTAL = "0";
                            SRDEnt.NON_TAXABLE_TOTAL = (Convert.ToDouble(lblAfterScheDisc.Text)).ToString();
                            SRDEnt.TAX_AMOUNT = "0";
                            SRDEnt.AMOUNT_AFTER_TAX = (Convert.ToDouble(lblAfterScheDisc.Text)).ToString();
                        }
                        SRDSer.Insert(SRDEnt, DT);
                    }
                    #endregion



                    #region account portion  
                    #region Credit note part for invoice cancel     
                    string invice_number = SIMEnt.INVOICE_NUMBER;

                    #region to insert in Voucher Master

                    VMEnt = new VOUCHER_MASTER();
                    VMEnt.VOUCHER_TYPE = "CN";
                    VMEnt.VOUCHER_NUMBER = af.getNext_VM_ID("CN", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfile.LocationID);
                    VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.VOUCHER_DAY = PGD.NepaliDay();
                    VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
                    VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
                    VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                    VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.TRN_AMOUNT = lblBillAmount.Text;
                    VMEnt.NARRATION = "Sales Cancel Invoice no." + invice_number + ". " + txtCreditNoteRemarks.Text;
                    VMEnt.REF_TABLE = "Invoice Master";
                    VMEnt.REF_ID = SIMEnt.PK_ID;
                    VMEnt.PREPARE_BY = userProfile.EmployeeID;
                    VMEnt.CHECKED_BY = userProfile.EmployeeID;
                    VMEnt.APPROVED_BY = userProfile.EmployeeID;
                    VMEnt.STATUS = "2";//status 2 is voucher approved
                    VMEnt.APPROVED_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.APPROVED_DAY = PGD.NepaliDay();
                    VMEnt.APPROVED_MONTH = PGD.NepaliMonth();
                    VMEnt.APPROVED_YEAR = PGD.NepaliYear();
                    VMEnt.APPROVED_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                    VMEnt.OFFICE_CODE = userProfile.LocationID; ;
                    string voucher_pk_id = VMSer.Insert(VMEnt, DT).ToString();
                    #endregion
                    SRMEnt = new SALES_RETURN_MASTER();
                    SRMEnt.PK_ID = SR_PK_ID;
                    SRMEnt = (SALES_RETURN_MASTER)SRMSer.GetSingle(SRMEnt, DT);
                    if (SRMEnt != null)
                    {
                        SRMEnt.SR_VM_ID = voucher_pk_id;
                        SRMSer.Update(SRMEnt);
                    }
                    #region to insert in to voucher child
                    int sno = 1;

                    #region for Dr Part
                    #region for Sales
                    foreach (GridViewRow gvr in grdInvoiceDetail.Rows)
                    {
                        Label lblProductCode = gvr.FindControl("lblProductCode") as Label;
                        Label lblAfterScheDisc = gvr.FindControl("lblAfterScheDisc") as Label;

                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = voucher_pk_id;
                        VCEnt.SNO = sno.ToString();
                        VCEnt.GL_CODE = "020101";  //Sales Account                      
                        VCEnt.SGL_CODE = lblProductCode.Text;
                        VCEnt.DR_AMOUNT = lblAfterScheDisc.Text;
                        VCEnt.CR_AMOUNT = "0";
                        VCEnt.REMARKS = "By";
                        VCEnt.OFFICE_CODE = userProfile.LocationID;
                        VCSer.Insert(VCEnt, DT);
                        sno++;
                    }
                    #endregion
                    #region for VAT Part

                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = voucher_pk_id;
                    VCEnt.SNO = sno.ToString();
                    VCEnt.GL_CODE = "040108";  //VAT Account                      
                    VCEnt.SGL_CODE = "";
                    VCEnt.DR_AMOUNT = lblVATAmount.Text;
                    VCEnt.CR_AMOUNT = "0";
                    VCEnt.REMARKS = "By";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    try
                    {
                        if (Convert.ToDouble(lblVATAmount.Text) != 0)
                        {
                            VCSer.Insert(VCEnt, DT);
                            sno++;
                        }
                    }
                    catch { }
                    #endregion

                    #region for Round Off; if round off is +ve 
                    double round = 0;
                    double round_value = 0;
                    try
                    {
                        round_value = Convert.ToDouble(lblRoundoff.Text);
                        if (round_value < 0)
                            round = round_value * -1;
                        else
                            round = round_value;
                    }
                    catch { }
                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = voucher_pk_id;
                    VCEnt.SNO = sno.ToString();
                    VCEnt.GL_CODE = "020201"; //GL_CODE of Round Off ; Indirect Income  
                    VCEnt.DR_AMOUNT = round.ToString();
                    VCEnt.CR_AMOUNT = "0";
                    VCEnt.REMARKS = "By";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    if (round_value > 0) /// discount cha bhaye matra insert garni
                    {
                        VCSer.Insert(VCEnt, DT);
                        sno++;
                    }

                    #endregion
                    #endregion
                    #region for Cr Part

                    #region for Customer
                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = voucher_pk_id;
                    VCEnt.SNO = sno.ToString();
                    if (lblCustomerID.Text != "") // Party
                    {
                        // 
                        VCEnt.GL_CODE = "010301"; //GL_CODE of Sundry Debitors Customer
                        CEnt = new CUSTOMER();
                        CEnt.PK_ID = lblCustomerID.Text;
                        CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
                        if (CEnt != null)
                        {
                            VCEnt.SGL_CODE = CEnt.CUSTOMER_CODE;
                        }
                    }
                    else // walk in customer -- customer id chaina bhaye teyo walkin customer huncha
                    {
                        if (SIMEnt.SALES_TYPE_ID == "CS")
                        {
                            VCEnt.GL_CODE = "010102"; //GL_CODE of Cash
                        }
                        else if (SIMEnt.SALES_TYPE_ID == "QR")
                        {
                            VCEnt.GL_CODE = "010101"; //GL_CODE of Bank
                            VCEnt.SGL_CODE = ddlBank.SelectedValue;
                        }
                    }

                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = lblBillAmount.Text;
                    VCEnt.REMARKS = "To";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCSer.Insert(VCEnt, DT);
                    sno++;

                    #endregion
                    #region for Discount                 

                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = voucher_pk_id;
                    VCEnt.SNO = sno.ToString();
                    VCEnt.GL_CODE = "030201"; //GL_CODE of Discount  
                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = lblDiscount.Text;
                    VCEnt.REMARKS = "To";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    if (lblDiscount.Text != "0.00") /// discount cha bhaye matra insert garni
                    {
                        VCSer.Insert(VCEnt, DT);
                        sno++;
                    }

                    #endregion

                    #region for Round Off; if round off is -ve 
                    round = 0;
                    round_value = 0;
                    try
                    {
                        round_value = Convert.ToDouble(lblRoundoff.Text);
                        if (round_value < 0)
                            round = round_value * -1;
                        else
                            round = round_value;
                    }
                    catch { }
                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = voucher_pk_id;
                    VCEnt.SNO = sno.ToString();
                    VCEnt.GL_CODE = "030202"; //GL_CODE of Round Off ; Indirect Expenses  
                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = round.ToString();
                    VCEnt.REMARKS = "To";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    if (round_value < 0) /// discount cha bhaye matra insert garni
                    {
                        VCSer.Insert(VCEnt, DT);
                        sno++;
                    }

                    #endregion
                    #endregion
                    #endregion

                    #endregion

                    #region for cash or QR sales for party; 
                    //yedi party lai cash or QR sales bahye, Credit voucher pani bancha teslai nil garna 1 ta payment voucher banaunu parcha
                    //Walk in customer ko DV nabani sidai CN bancha so CN matra banaye pugcha sales return ko DV banauna parena 
                    if (lblModeofPayment.Text != "Credit" && lblCustomerID.Text != "")
                    {
                        #region to insert in Voucher Master

                        VMEnt = new VOUCHER_MASTER();
                        VMEnt.VOUCHER_TYPE = "DV";
                        VMEnt.VOUCHER_NUMBER = af.getNext_VM_ID("DV", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfile.LocationID);
                        VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                        VMEnt.VOUCHER_DAY = PGD.NepaliDay();
                        VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
                        VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
                        VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                        VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                        VMEnt.TRN_AMOUNT = lblBillAmount.Text;
                        VMEnt.NARRATION = "Payment Inversion of Invoice no." + invice_number + ". " + txtCreditNoteRemarks.Text;
                        VMEnt.REF_TABLE = "Invoice Master";
                        VMEnt.REF_ID = SIMEnt.PK_ID;
                        VMEnt.PREPARE_BY = userProfile.EmployeeID;
                        VMEnt.CHECKED_BY = userProfile.EmployeeID;
                        VMEnt.APPROVED_BY = userProfile.EmployeeID;
                        VMEnt.STATUS = "2";//status 2 is voucher approved
                        VMEnt.APPROVED_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                        VMEnt.APPROVED_DAY = PGD.NepaliDay();
                        VMEnt.APPROVED_MONTH = PGD.NepaliMonth();
                        VMEnt.APPROVED_YEAR = PGD.NepaliYear();
                        VMEnt.APPROVED_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                        VMEnt.OFFICE_CODE = userProfile.LocationID;
                        string new_voucher_pk_id = VMSer.Insert(VMEnt, DT).ToString();
                        #endregion

                        #region to insert in to voucher child
                        #region for Dr part

                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = new_voucher_pk_id;
                        VCEnt.SNO = "1";
                        VCEnt.GL_CODE = "010301"; //GL_CODE of Sundry Debitors Customer
                        VCEnt.SGL_CODE = hf.getCustomerCodeFromId(lblCustomerID.Text);
                        VCEnt.DR_AMOUNT = lblBillAmount.Text;
                        VCEnt.CR_AMOUNT = "0";
                        VCEnt.REMARKS = "By";
                        VCEnt.OFFICE_CODE = userProfile.LocationID;
                        VCSer.Insert(VCEnt, DT);
                        #endregion

                        #region for Cr Part
                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = new_voucher_pk_id;
                        VCEnt.SNO = "2";

                        if (lblModeofPayment.Text == "QR")
                        {
                            if (ddlCNType.SelectedValue == "CS")
                            {
                                VCEnt.GL_CODE = "010102"; //GL_CODE of Cash
                            }
                            else if (ddlCNType.SelectedValue == "QR")
                            {
                                VCEnt.GL_CODE = "010101"; //GL_CODE of Bank
                                VCEnt.SGL_CODE = ddlBank.SelectedValue;
                            }
                        }
                        else // QR hoe na bhane teyo cash ho
                        {
                            VCEnt.GL_CODE = "010102"; //GL_CODE of Cash
                        }

                        VCEnt.DR_AMOUNT = "0";
                        VCEnt.CR_AMOUNT = lblBillAmount.Text;
                        VCEnt.REMARKS = "To";
                        VCEnt.OFFICE_CODE = userProfile.LocationID;
                        VCSer.Insert(VCEnt, DT);
                        #endregion
                        #endregion
                    }
                    #endregion

                    #endregion

                    if (DT.HAPPY == true)
                    {
                        DT.Commit();

                        SRMEnt = new SALES_RETURN_MASTER();
                        SRMEnt.PK_ID = SR_PK_ID;
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
                                        ref_invoice_number = lblInvoiceNo.Text,
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
                                SRMEnt.CBMS_PUSH = "Offline";
                                SRMEnt.CBMS_PUSH_RT = "NO";
                                SRMSer.Update(SRMEnt);
                            }
                            PrintCreditNote(SR_PK_ID);
                            divButton.Visible = true;
                            divInvoiceDetail.Visible = false;
                        }
                    }
                    else
                    {
                        DT.Abort();
                        HelperFunction.MsgBox(this, this.GetType(), "Something goes wrong. Please try again.");
                    }
                }

                DT.Dispose();
            }
            else
            {
                HelperFunction.MsgBox(this, this.GetType(), msg);
            }
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "This Credit Note have been already saved.");
        }
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

    #region to Credit Note

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
            lblInvCreatedBy.Text = hf.getEmployeeName(SIMEnt.USER_ID);

            printdetail.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
        }
    }
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
    #endregion

    protected void btnClear_Click(object sender, EventArgs e)
    {
        divInitial.Visible = true;
        divInvoiceDetail.Visible = false;
        divButton.Visible = false;
        printdetail.Visible = false;
        LoadInvoiceGrid();
        lblunique_token.Text = userProfile.UserName + hf.getmaxCNid(); // this code is for not allowing double entry of CN bill
    }



    protected void ddlCNType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCNType.SelectedValue == "QR")
        {
            LoadBank();
            divBankDetail.Visible = true;
        }
        else
        {
            divBankDetail.Visible = false;
        }
    }
    protected void SetVisibility()
    {
        trInvRoundOff.Visible = PGPS.RoundOff();// whether or not to show round off on inv
        trInvRoTot.Visible = PGPS.RoundOff();
        trInvRoundOffS.Visible = PGPS.RoundOff();// whether or not to show round off on inv
        if (PG.CompanyTAXType() != "VAT")//whether or not to show vat on bill
        {
            trInvVat.Visible = false;
            trInvVats.Visible = false;
            
        }
        gridSalesRetInvoice.Columns[2].Visible = PGPS.ShowDualQuantity();//Upper Qty 

        gridSalesRetInvoice.Columns[6].Visible = PGPS.ItemWiseDiscount(); // Item wise discount
        gridSalesRetInvoice.Columns[7].Visible = PGPS.ItemWiseDiscount(); // Amount After Item wise discount
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