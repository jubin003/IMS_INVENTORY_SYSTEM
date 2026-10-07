using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;
using DataHelper.Framework;
using System.Data.SqlClient;
using System.Configuration;
using Entity.Framework;

public partial class Utilities_Purchase_ProductPurchaseReturn : System.Web.UI.Page
{
    PURCHASE_INVOICE_MASTER PIMEnt = new PURCHASE_INVOICE_MASTER();
    PURCHASE_INVOICE_MASTERService PIMSer = new PURCHASE_INVOICE_MASTERService();

    PURCHASE_INVOICE_DETAIL PIDEnt = new PURCHASE_INVOICE_DETAIL();
    PURCHASE_INVOICE_DETAILService PIDSer = new PURCHASE_INVOICE_DETAILService();


    PURCHASE_RETURN_MASTER PRMEnt = new PURCHASE_RETURN_MASTER();
    PURCHASE_RETURN_MASTERService PRMSer = new PURCHASE_RETURN_MASTERService();

    PURCHASE_RETURN_DETAIL PRDEnt = new PURCHASE_RETURN_DETAIL();
    PURCHASE_RETURN_DETAILService PRDSer = new PURCHASE_RETURN_DETAILService();

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();
    PAYMENT_TYPE PTEnt = new PAYMENT_TYPE();
    PAYMENT_TYPEService PTSEr = new PAYMENT_TYPEService();

    SUPPLIERS SEnt = new SUPPLIERS();
    SUPPLIERSService SSer = new SUPPLIERSService();
    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();

    VOUCHER_MASTER VMEnt = new VOUCHER_MASTER();
    VOUCHER_MASTERService VMSer = new VOUCHER_MASTERService();

    VOUCHER_CHILD VCEnt = new VOUCHER_CHILD();
    VOUCHER_CHILDService VCSer = new VOUCHER_CHILDService();

    GL_SUB_ACCOUNT GLSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GLSASer = new GL_SUB_ACCOUNTService();

    UserProfileEntity userProfileEnt = new UserProfileEntity();
    HelperFunction hf = new HelperFunction();
    Boolean flag = false;
    Boolean IsPageRefresh = false;
    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGan PG = new PhyeGan();
    AccountFunction af = new AccountFunction();

    static string path = "";
    string old_pk_id = "";
    string random_number = "";
    string responseurl = "~/Login.aspx";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDebitNoteDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
            LoadSuppliers();
            LoadPaymentType();
            LoadDNType();
            try
            {
                old_pk_id = Request.QueryString["opi"].ToString();
                random_number = Request.QueryString["r"].ToString();
            }
            catch { }
            if (old_pk_id == "")
            {
                lblOldPK_ID.Text = "";
            }
            else
            {
                try
                {
                    old_pk_id = (Convert.ToDouble(old_pk_id) / Convert.ToDouble(random_number)).ToString(); // pkid is multiplied by random number in request url
                }
                catch
                {
                    responseurl = "~/forbidden.aspx";
                    Response.Redirect(responseurl);
                }
                PIMEnt = new PURCHASE_INVOICE_MASTER();
                PIMEnt.PK_ID = old_pk_id;
                PIMEnt = (PURCHASE_INVOICE_MASTER)PIMSer.GetSingle(PIMEnt);
                if (PIMEnt != null)
                {
                    lblOldPK_ID.Text = PIMEnt.PK_ID;
                    LoadPurchaseDetail(PIMEnt);
                }
                else
                {
                    responseurl = "~/forbidden.aspx";
                    Response.Redirect(responseurl);
                }
            }
        }
    }
    protected void LoadPurchaseDetail(PURCHASE_INVOICE_MASTER PIMEnt)
    {
        if (PIMEnt.SUPPLIER_ID == "") //walk in customer
        {
            divSupplier.Visible = false;
            divWalkinSupplier.Visible = true;
            chkSuppliers.Checked = true;
            chkWalkIn.Checked = true;
            txtSupplierName.Text = PIMEnt.SUPPLIER_NAME;
        }
        else
        {
            divSupplier.Visible = true;
            divWalkinSupplier.Visible = false;
            chkSuppliers.Checked = false;
            chkSuppliers.Checked = false;
            SEnt = new SUPPLIERS();
            SEnt.PK_ID = PIMEnt.SUPPLIER_ID;
            SEnt = (SUPPLIERS)SSer.GetSingle(SEnt);
            if (SEnt != null)
            {
                txtSupplierCode.Text = SEnt.SUPPLIER_CODE;
                ddlSupplier.SelectedValue = SEnt.PK_ID;
            }
        }
        txtDakhilaNo.Text = PIMEnt.DAKHILA_NUMBER;
        txtSupplierPANVAT.Text = PIMEnt.SUPPLIER_PAN_VAT;
        txtSupplierAddress.Text = PIMEnt.SUPPLIER_ADDRESS;
        txtContactNo.Text = PIMEnt.SUPPLIER_CONTACT;
        txtInvoiceNumber.Text = PIMEnt.SUPPLIER_INVOICE_NO;
        txtInvoiceDate.Text = PIMEnt.INVOICE_DAY + "/" + PIMEnt.INVOICE_MONTH + "/" + PIMEnt.INVOICE_YEAR;
        txtDakhilaDate.Text = PIMEnt.DAKHILA_DAY + "/" + PIMEnt.DAKHILA_MONTH + "/" + PIMEnt.DAKHILA_YEAR;
        ddlPaymentType.SelectedValue = PIMEnt.PURCHASE_TYPE_ID;

        txtDiscount.Text = PIMEnt.DISCOUNT_PERCENT;

        LoadGrid(PIMEnt.PK_ID);
        getTotal();
    }

    protected void LoadPaymentType()
    {
        PTEnt = new PAYMENT_TYPE();
        PTEnt.SALES_PURCHASE = "P";
        PTEnt.STATUS = "1";
        ddlPaymentType.DataSource = PTSEr.GetAll(PTEnt);
        ddlPaymentType.DataTextField = "PAYMENT_NAME";
        ddlPaymentType.DataValueField = "PAYMENT_CODE";
        ddlPaymentType.DataBind();
    }

    protected void LoadDNType()
    {
        PTEnt = new PAYMENT_TYPE();
        PTEnt.SALES_PURCHASE = "P";
        PTEnt.STATUS = "1";
        ddlDNType.DataSource = PTSEr.GetAll(PTEnt);
        ddlDNType.DataTextField = "PAYMENT_NAME";
        ddlDNType.DataValueField = "PAYMENT_CODE";
        ddlDNType.DataBind();
    }
    protected void LoadSuppliers()
    {
        SEnt = new SUPPLIERS();
        SEnt.STATUS = "1";
        ddlSupplier.DataSource = SSer.GetAll(SEnt);
        ddlSupplier.DataTextField = "SUPPLIER_NAME";
        ddlSupplier.DataValueField = "PK_ID";
        ddlSupplier.DataBind();
        ddlSupplier.Items.Insert(0, "Select");
    }

    protected DataTable LoadGrid(string PIM_pk_id)
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        EntityList theList = new EntityList();
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("PRODUCT_CODE");
        dummyTable.Columns.Add("PRODUCT");
        dummyTable.Columns.Add("BATCH_NO");
        dummyTable.Columns.Add("EXPIRY_DATE");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("R_QUANTITY");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("TOTAL");
        dummyTable.Columns.Add("R_TOTAL");

        DataView dv = new DataView(dummyTable);

        PIDEnt = new PURCHASE_INVOICE_DETAIL();
        PIDEnt.PURCHASE_INVOICE_ID = PIM_pk_id;
        PIDEnt.OFFICE_CODE = userProfileEnt.LocationID;
        theList = PIDSer.GetAll(PIDEnt);
        if (theList.Count > 0)
        {
            foreach (PURCHASE_INVOICE_DETAIL PID in theList)
            {
                DataRow dummyRow = dummyTable.NewRow();

                PEnt = new PRODUCT();
                PEnt.PK_ID = PID.PRODUCT_ID;
                PEnt.OFFICE_CODE = userProfileEnt.LocationID;
                PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                if (PEnt != null)
                {
                    dummyRow["PK_ID"] = PEnt.PK_ID;
                    dummyRow["PRODUCT_CODE"] = PEnt.PRODUCT_CODE;
                    dummyRow["PRODUCT"] = PEnt.PRODUCT_NAME;
                    dummyRow["BATCH_NO"] = PID.BATCH_NUMBER;
                    dummyRow["EXPIRY_DATE"] = PID.EXPIRY_DATE;
                    dummyRow["QUANTITY"] = PID.QUANTITY;
                    dummyRow["UNIT"] = hf.getProductUnit(PEnt.PK_ID);
                    dummyRow["R_QUANTITY"] = "";
                    dummyRow["RATE"] = PID.RATE;
                    dummyRow["TOTAL"] = PID.SUB_TOTAL;
                    dummyRow["R_TOTAL"] = "";
                    dummyTable.Rows.Add(dummyRow);
                }
            }
        }
        grdPurchaseDetail.DataSource = dv;
        grdPurchaseDetail.DataBind();
        return dummyTable;
    }
    protected void getTotal()
    {
        double total = 0;
        double discount_percent = 0;
        double discount = 0;
        double beforedecimal = 0;
        double afterdecimal = 0;
        foreach (GridViewRow gr in grdPurchaseDetail.Rows)
        {
            Label lblItemRTotal = gr.FindControl("lblItemRTotal") as Label;
            try
            {
                total = total + (Convert.ToDouble(lblItemRTotal.Text));
            }
            catch
            { }
        }
        try
        {
            discount_percent = Convert.ToDouble(txtDiscount.Text);
        }
        catch
        {
            txtDiscount.Text = "0";
        }
        if (discount_percent > 0)
        {
            txtDiscountAmount.Text = ((total * Convert.ToDouble(txtDiscount.Text)) / 100).ToString("#0.00");
            discount = Convert.ToDouble(txtDiscountAmount.Text);
        }
        else
        {
            try
            {
                discount = Convert.ToDouble(txtDiscountAmount.Text);
            }
            catch { txtDiscountAmount.Text = "0"; }
        }
        lblSubTotalAmount.Text = total.ToString("#0.00");
        lblTotalAmount.Text = (total - discount).ToString("#0.00");
        lblVAT.Text = ((total - discount) * Convert.ToDouble(txtVATPercent.Text) / 100).ToString("#0.00");
        lblGrandTotal.Text = (Convert.ToDouble(lblVAT.Text) + Convert.ToDouble(lblTotalAmount.Text)).ToString("#0.00");
        beforedecimal = Math.Truncate(Convert.ToDouble(lblGrandTotal.Text));
        afterdecimal = Convert.ToDouble(lblGrandTotal.Text) - beforedecimal;
        if (afterdecimal == 0)
        {
            txtRound.Text = (afterdecimal).ToString("0.00");
            lblInvoiceAmount.Text = beforedecimal.ToString("0.00");
        }
        else if (afterdecimal >= 0.5)
        {
            beforedecimal = beforedecimal + 1;
            txtRound.Text = (1 - afterdecimal).ToString("0.00");
            lblInvoiceAmount.Text = beforedecimal.ToString("0.00");
        }
        else
        {
            txtRound.Text = "-" + (afterdecimal).ToString("0.00");
            lblInvoiceAmount.Text = beforedecimal.ToString("0.00");
        }

    }

    protected void Clear()
    {
        txtSupplierCode.Text = "";
        txtSupplierAddress.Text = "";
        txtSupplierPANVAT.Text = "";
        txtContactNo.Text = "";
        ddlSupplier.SelectedValue = "Select";
        txtInvoiceNumber.Text = "";
        txtInvoiceDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
        ddlPaymentType.SelectedValue = "CP";
        lblSubTotalAmount.Text = "0.00";
        lblTotalAmount.Text = "0.00";
        txtDiscountAmount.Text = "0.00";
        txtDiscount.Text = "0";
        lblVAT.Text = "0.00";
        lblGrandTotal.Text = "0.00";
        txtRound.Text = "0.00";
        lblInvoiceAmount.Text = "0.00";
        divSupplier.Visible = true;
        divWalkinSupplier.Visible = false;
        chkSuppliers.Checked = false;
        chkWalkIn.Checked = false;
        ddlPaymentType.Enabled = true;
        txtSupplierCode.Focus();
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
    protected void txtRound_TextChanged(object sender, EventArgs e)
    {
        string round = "";
        try
        {
            round = Convert.ToDouble(txtRound.Text).ToString();
        }
        catch
        { txtRound.Text = "0.00"; }
        lblInvoiceAmount.Text = (Convert.ToDouble(lblGrandTotal.Text) + Convert.ToDouble(txtRound.Text)).ToString("###.00");
    }
    protected void txtRQty_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gr = ((TextBox)sender).Parent.Parent as GridViewRow;
        Label lblItemRTotal = gr.FindControl("lblItemRTotal") as Label;
        TextBox txtRQty = gr.FindControl("txtRQty") as TextBox;
        Label lblRate = gr.FindControl("lblRate") as Label;
        Label lblQty = gr.FindControl("lblQty") as Label;

        if (Convert.ToDouble(txtRQty.Text) <= Convert.ToDouble(lblQty.Text))
            lblItemRTotal.Text = (Convert.ToDouble(lblRate.Text) * Convert.ToDouble(txtRQty.Text)).ToString("#0.00");
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Return Qty cannot be greater than Invoice Qty.");
            txtRQty.Text = "";
            txtRQty.Focus();
        }
        getTotal();
    }
    protected void btnSavePurchase_Click(object sender, EventArgs e)
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (!IsPageRefresh)
        {
            string msg = "Please Enter ";
            try
            {
                double amt = Convert.ToDouble(lblInvoiceAmount.Text);
            }
            catch
            {
                msg = msg + " Return Qty or Return Amount is 0";
            }
            if (txtReturnNote.Text == "")
                msg = msg + " Return Note.";
            if (msg == "Please Enter ")
            {
                if (grdPurchaseDetail.Rows.Count != 0)
                {
                    DistributedTransaction DT = new DistributedTransaction();

                    string pk_id = "";
                    #region to insert in purchase return master
                    PRMEnt = new PURCHASE_RETURN_MASTER();

                    PRMEnt.NOTE_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    PRMEnt.NOTE_DAY = PGD.NepaliDay();
                    PRMEnt.NOTE_MONTH = PGD.NepaliMonth();
                    PRMEnt.NOTE_YEAR = PGD.NepaliYear();
                    PRMEnt.NOTE_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                    PRMEnt.PURCHASE_ID = lblOldPK_ID.Text;
                    if (chkWalkIn.Checked == false)
                    {
                        PRMEnt.SUPPLIER_ID = ddlSupplier.SelectedValue;
                        PRMEnt.SUPPLIER_NAME = ddlSupplier.SelectedItem.ToString();
                    }
                    else
                    {
                        PRMEnt.SUPPLIER_NAME = txtSupplierName.Text;
                    }
                    PRMEnt.SUPPLIER_PAN_VAT = txtSupplierPANVAT.Text;
                    PRMEnt.SUPPLIER_ADDRESS = txtSupplierAddress.Text;
                    PRMEnt.SUPPLIER_CONTACT = txtContactNo.Text;
                    PRMEnt.SUB_TOTAL_AMOUNT = lblTotalAmount.Text;
                    PRMEnt.DISCOUNT_AMOUNT = txtDiscountAmount.Text;
                    PRMEnt.DISCOUNT_PERCENT = txtDiscount.Text;
                    PRMEnt.TOTAL_AMOUNT = lblTotalAmount.Text;
                    PRMEnt.TAX_VAT_AMOUNT = lblVAT.Text;
                    PRMEnt.GRAND_TOTAL_AMOUNT = lblGrandTotal.Text;
                    PRMEnt.ROUND_OFF = txtRound.Text;
                    PRMEnt.RETURN_AMOUNT = lblInvoiceAmount.Text;
                    PRMEnt.USER_ID = userProfileEnt.EmployeeID;
                    PRMEnt.RETURN_TYPE_ID = ddlPaymentType.SelectedValue;
                    PRMEnt.RETURN_REMARKS = txtReturnNote.Text;
                    PRMEnt.OFFICE_CODE = userProfileEnt.LocationID;
                    pk_id = PRMSer.Insert(PRMEnt, DT).ToString();
                    #endregion

                    double billSno = 1;
                    #region to insert in to purchase return detail
                    foreach (GridViewRow gr in grdPurchaseDetail.Rows)
                    {
                        Label lblSno = (Label)gr.FindControl("lblSno");
                        Label lblPK_ID = (Label)gr.FindControl("lblPK_ID");
                        Label lblBatch = (Label)gr.FindControl("lblBatch");
                        Label lblExpDate = (Label)gr.FindControl("lblExpDate");
                        TextBox txtRQty = (TextBox)gr.FindControl("txtRQty");
                        Label lblRate = (Label)gr.FindControl("lblRate");
                        Label lblItemRTotal = (Label)gr.FindControl("lblItemRTotal");

                        PRDEnt = new PURCHASE_RETURN_DETAIL();
                        PRDEnt.PURCHASE_RETURN_ID = pk_id;
                        PRDEnt.SNO = billSno.ToString(); // return bill ma random 
                        PRDEnt.PRODUCT_ID = lblPK_ID.Text;
                        PRDEnt.BATCH_NUMBER = lblBatch.Text;
                        PRDEnt.EXPIRY_DATE = lblExpDate.Text;
                        PRDEnt.QUANTITY = txtRQty.Text;
                        PRDEnt.RATE = lblRate.Text;
                        PRDEnt.AMOUNT = lblItemRTotal.Text;
                        PRDEnt.OFFICE_CODE = userProfileEnt.LocationID; ;
                        double rqty = 0; // to check if there is return qty or not
                        try
                        {
                            rqty = Convert.ToDouble(txtRQty.Text);
                        }
                        catch { }
                        if (rqty > 0)
                        {
                            PRDSer.Insert(PRDEnt, DT);
                            billSno++;
                        }

                    }
                    #endregion



                    #region account portion
                    if (PG.AccountEnabled() == "ENABLE")
                    {
                        double round = 0;
                        double round_value = 0;
                        #region to insert in Voucher Master

                        VMEnt = new VOUCHER_MASTER();
                        VMEnt.VOUCHER_TYPE = "DN";
                        VMEnt.VOUCHER_NUMBER = af.getNext_VM_ID("DN", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfileEnt.LocationID);
                        VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                        VMEnt.VOUCHER_DAY = PGD.NepaliDay();
                        VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
                        VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
                        VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                        VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                        VMEnt.TRN_AMOUNT = lblInvoiceAmount.Text;
                        VMEnt.NARRATION = "Purchase return from Invoice no." + txtInvoiceNumber.Text + ". " + txtReturnNote.Text;
                        VMEnt.REF_TABLE = "Purchase Invoice";
                        VMEnt.REF_ID = pk_id;
                        VMEnt.PREPARE_BY = userProfileEnt.EmployeeID;
                        VMEnt.CHECKED_BY = userProfileEnt.EmployeeID;
                        VMEnt.APPROVED_BY = userProfileEnt.EmployeeID;
                        VMEnt.STATUS = "2";//status 2 is voucher approved
                        VMEnt.APPROVED_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                        VMEnt.APPROVED_DAY = PGD.NepaliDay();
                        VMEnt.APPROVED_MONTH = PGD.NepaliMonth();
                        VMEnt.APPROVED_YEAR = PGD.NepaliYear();
                        VMEnt.APPROVED_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                        VMEnt.OFFICE_CODE = userProfileEnt.LocationID;
                        string voucher_pk_id = VMSer.Insert(VMEnt, DT).ToString();
                        #endregion

                        PIMEnt = new PURCHASE_INVOICE_MASTER();
                        PIMEnt.PK_ID = pk_id;
                        PIMEnt = (PURCHASE_INVOICE_MASTER)PIMSer.GetSingle(PIMEnt, DT);
                        if (PIMEnt != null)
                        {
                            PIMEnt.PB_VM_ID = voucher_pk_id;
                            PIMSer.Update(PIMEnt, DT);
                        }

                        #region to insert in to voucher child
                        int sno = 1;
                        #region for Dr Part
                        #region for Cash Or Bank
                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = voucher_pk_id;
                        VCEnt.SNO = sno.ToString();
                        VCEnt.GL_CODE = "040301"; //GL_CODE of Sundry Debitors Supplier
                        VCEnt.SGL_CODE = txtSupplierCode.Text;
                        VCEnt.DR_AMOUNT = lblInvoiceAmount.Text;
                        VCEnt.CR_AMOUNT = "0";
                        VCEnt.REMARKS = "By";
                        VCEnt.OFFICE_CODE = userProfileEnt.LocationID;
                        VCSer.Insert(VCEnt, DT);
                        sno++;
                        #region
                        #endregion

                        #endregion
                        #region for Discount                 

                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = voucher_pk_id;
                        VCEnt.SNO = sno.ToString();
                        VCEnt.GL_CODE = "020202"; //GL_CODE of Discount Received 
                        VCEnt.DR_AMOUNT = txtDiscountAmount.Text;
                        VCEnt.CR_AMOUNT = "0";
                        VCEnt.REMARKS = "By";
                        VCEnt.OFFICE_CODE = userProfileEnt.LocationID;
                        if (txtDiscountAmount.Text != "0") /// discount cha bhaye matra insert garni
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
                            round_value = Convert.ToDouble(txtRound.Text);
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
                        VCEnt.OFFICE_CODE = userProfileEnt.LocationID;
                        if (round_value < 0) /// discount cha bhaye matra insert garni
                        {
                            VCSer.Insert(VCEnt, DT);
                            sno++;
                        }

                        #endregion

                        #endregion
                        #region for Cr Part
                        #region for Purchase Return
                        foreach (GridViewRow gvr in grdPurchaseDetail.Rows)
                        {
                            Label lblProductCode = gvr.FindControl("lblProductCode") as Label;
                            Label lblItemRTotal = gvr.FindControl("lblItemRTotal") as Label;

                            VCEnt = new VOUCHER_CHILD();
                            VCEnt.VOUCHER_ID = voucher_pk_id;
                            VCEnt.SNO = sno.ToString();
                            VCEnt.GL_CODE = "010402";  //Purchase Account                      
                            VCEnt.SGL_CODE = lblProductCode.Text;
                            VCEnt.DR_AMOUNT = "0";
                            VCEnt.CR_AMOUNT = lblItemRTotal.Text;
                            VCEnt.REMARKS = "To";
                            VCEnt.OFFICE_CODE = userProfileEnt.LocationID; ;
                            if (lblItemRTotal.Text != "")
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
                        VCEnt.DR_AMOUNT = "0";
                        VCEnt.CR_AMOUNT = lblVAT.Text;
                        VCEnt.REMARKS = "To";
                        VCEnt.OFFICE_CODE = userProfileEnt.LocationID;
                        VCSer.Insert(VCEnt, DT);
                        sno++;

                        #endregion
                        #region for Round Off; if round off is +ve 
                        round = 0;
                        round_value = 0;
                        try
                        {
                            round_value = Convert.ToDouble(txtRound.Text);
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
                        VCEnt.DR_AMOUNT = "0";
                        VCEnt.CR_AMOUNT = round.ToString();
                        VCEnt.REMARKS = "To";
                        VCEnt.OFFICE_CODE = userProfileEnt.LocationID;
                        if (round_value > 0) /// discount cha bhaye matra insert garni
                        {
                            VCSer.Insert(VCEnt, DT);
                            sno++;
                        }

                        #endregion
                        #endregion
                        #endregion


                    }
                    #endregion

                    if (DT.HAPPY == true)
                    {
                        DT.Commit();
                        PRMEnt = new PURCHASE_RETURN_MASTER();
                        PRMEnt.PK_ID = pk_id;
                        PRMEnt = (PURCHASE_RETURN_MASTER)PRMSer.GetSingle(PRMEnt);
                        if (PRMEnt != null)
                        {
                            LoadToPrint(pk_id); // Customer Copy
                        }
                        Clear();
                    }
                    else
                    {
                        DT.Abort();
                        HelperFunction.MsgBox(this, this.GetType(), "Sorry Something Goes Wrong.");
                    }
                    DT.Dispose();

                    divHide.Visible = false;
                    divShow.Visible = true;
                }
            }
            else
                HelperFunction.MsgBox(this, this.GetType(), msg);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/utilities/purchase/purchasereturn.aspx");
    }
    #region to print bill

    protected void LoadCompanyDetail()
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        lblCompanyName.Text = PG.CompanyName();
        lblCompanyAddress.Text = PG.BranchAddress(userProfileEnt.LocationID);
        lblEmail.Text = PG.BranchEmail(userProfileEnt.LocationID);
        lblWebsite.Text = PG.CompanyWebsite();
        if (PG.CompanyEmail() == "" && PG.CompanyWebsite() == "")
        {
            divEmail.Visible = false;
        }
        lblPanNo.Text = PG.CompanyVATPan();
        lblPhone.Text = PG.CompanyContact();

    }
    protected void cleardata()
    {
        lblDebitNoteNo.Text = "";
        lblSupplierName.Text = "";
        lblInvoiceNo.Text = "";
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
        LoadCompanyDetail();
        PRMEnt = new PURCHASE_RETURN_MASTER();
        PRMEnt.PK_ID = pk_id;
        PRMEnt = (PURCHASE_RETURN_MASTER)PRMSer.GetSingle(PRMEnt);
        if (PRMEnt != null)
        {
            PIMEnt = new PURCHASE_INVOICE_MASTER();
            PIMEnt.PK_ID = PRMEnt.PURCHASE_ID;
            PIMEnt = (PURCHASE_INVOICE_MASTER)PIMSer.GetSingle(PIMEnt);
            if (PIMEnt != null)
            {
                lblInvoiceNo.Text = PIMEnt.SUPPLIER_INVOICE_NO;
                lblInvoiceNepaliDate.Text = PIMEnt.INVOICE_DAY + "/" + PIMEnt.INVOICE_MONTH + "/" + PIMEnt.INVOICE_YEAR;
                lblInvoiceEnglishDate.Text = PIMEnt.INVOICE_DATE;
                lblDakhilaNo.Text = PIMEnt.DAKHILA_NUMBER;
                lblDakhilaNepaliDate.Text = PIMEnt.DAKHILA_DAY + "/" + PIMEnt.DAKHILA_MONTH + "/" + PIMEnt.DAKHILA_YEAR;
                lblDakhilaEnglishiDate.Text = PIMEnt.DAKHILA_DATE;
                
            }
            LoadPurchaseRetGrid(PRMEnt.PK_ID);
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
            lblAmountInWord.Text = hf.NumWordsWrapper(Convert.ToDouble(PRMEnt.RETURN_AMOUNT)) + " Only";//.ToUpper()

        }
     
        printdetail.Visible = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void LoadPurchaseRetGrid(string PK_id)
    {
        DataTable dt = hf.LoadPurchaseReturnInvoice(PK_id); // your existing data
        int desiredRowCount = 30;

        while (dt.Rows.Count < desiredRowCount)
        {
            dt.Rows.Add(dt.NewRow()); // add empty rows to reach 20
        }

        gridReturnInvoice.DataSource = dt;
        gridReturnInvoice.DataBind();
    }
    #endregion

    protected void grdPurchaseDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
        }
    }
}
