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
using System.Collections;

public partial class Expenditure_expenditure_dakhila : System.Web.UI.Page
{
    EXPENDITURE_DAKHILA EDMEnt = new EXPENDITURE_DAKHILA();
    EXPENDITURE_DAKHILAService EDMSer = new EXPENDITURE_DAKHILAService();

    EXPENDITURE_DAKHILA_DETAILS ADDEnt = new EXPENDITURE_DAKHILA_DETAILS();
    EXPENDITURE_DAKHILA_DETAILSService ADDSer = new EXPENDITURE_DAKHILA_DETAILSService();



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

    VOUCHER_ALTER_REQUEST VAREnt = new VOUCHER_ALTER_REQUEST();
    VOUCHER_ALTER_REQUESTService VARSer = new VOUCHER_ALTER_REQUESTService();

    GL_ACCOUNT GLAEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLASer = new GL_ACCOUNTService();

    GL_SUB_ACCOUNT GLSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GLSASer = new GL_SUB_ACCOUNTService();



    UserProfileEntity userProfile = new UserProfileEntity();
    HelperFunction hf = new HelperFunction();

    AccountFunction af = new AccountFunction();

    Boolean flag = false;
    Boolean IsPageRefresh = false;
    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGan PG = new PhyeGan();
    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();
    string old_pk_id = "";
    string random_number = "";
    string responseurl = "~/Login.aspx";

    string voucher_pk_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                if (!IsPostBack)
                {
                    ViewState["postids"] = System.Guid.NewGuid().ToString();
                    Session["postid"] = ViewState["postids"].ToString();
                    LoadSuppliers();
                    Loadexp();


                    LoadPaymentType("P");
                    try
                    {
                        old_pk_id = Request.QueryString["opi"].ToString();
                        random_number = Request.QueryString["r"].ToString();
                    }
                    catch { }
                    if (old_pk_id == "")
                    {
                        lblOldPK_ID.Text = "";
                        txtDakhilaDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
                        txtInvoiceDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
                        CreateGridFirst();
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
                        EDMEnt = new EXPENDITURE_DAKHILA();
                        EDMEnt.PK_ID = old_pk_id;
                        EDMEnt = (EXPENDITURE_DAKHILA)EDMSer.GetSingle(EDMEnt);
                        if (EDMEnt != null)
                        {
                            lblOldPK_ID.Text = EDMEnt.PK_ID;
                            //LoadPurchaseDetail(EXPENDITURE_DAKHILA);
                        }

                        else
                        {
                            responseurl = "~/forbidden.aspx";
                            Response.Redirect(responseurl);
                        }
                    }
                    txtSupplierCode.Focus();

                }
                else
                {
                    if (ViewState["postids"].ToString() != Session["postid"].ToString())
                    {
                        IsPageRefresh = true;
                    }
                    Session["postid"] = System.Guid.NewGuid().ToString();
                    ViewState["postids"] = Session["postid"].ToString();

                }
            }
            catch (Exception ee)
            {
                Response.Redirect(responseurl);
            }
        }
    }
    protected void LoadPurchaseDetail(PURCHASE_INVOICE_MASTER EDMEnt)
    {
        if (EDMEnt.SUPPLIER_ID == "") //walk in customer
        {
            divSupplier.Visible = false;
            divWalkinSupplier.Visible = true;
            chkSuppliers.Checked = true;
            chkWalkIn.Checked = true;
            ddlPaymentType.Enabled = false;
            txtSupplierName.Text = EDMEnt.SUPPLIER_NAME;
        }
        else
        {
            divSupplier.Visible = true;
            divWalkinSupplier.Visible = false;
            chkSuppliers.Checked = false;
            chkWalkIn.Checked = false;
            ddlPaymentType.Enabled = true;
            SEnt = new SUPPLIERS();
            SEnt.PK_ID = EDMEnt.SUPPLIER_ID;
            SEnt = (SUPPLIERS)SSer.GetSingle(SEnt);
            if (SEnt != null)
            {
                txtSupplierCode.Text = SEnt.SUPPLIER_CODE;
                ddlSupplier.SelectedValue = SEnt.PK_ID;
            }
        }
        txtSupplierPANVAT.Text = EDMEnt.SUPPLIER_PAN_VAT;
        txtSupplierAddress.Text = EDMEnt.SUPPLIER_ADDRESS;
        txtContactNo.Text = EDMEnt.SUPPLIER_CONTACT;
        txtInvoiceNumber.Text = EDMEnt.SUPPLIER_INVOICE_NO;
        txtInvoiceDate.Text = EDMEnt.INVOICE_DAY + "/" + EDMEnt.INVOICE_MONTH + "/" + EDMEnt.INVOICE_YEAR;
        txtDakhilaDate.Text = EDMEnt.DAKHILA_DAY + "/" + EDMEnt.DAKHILA_MONTH + "/" + EDMEnt.DAKHILA_YEAR;
        ddlPaymentType.SelectedValue = EDMEnt.PURCHASE_TYPE_ID;
        lblSubTotalAmount.Text = EDMEnt.SUB_TOTAL_AMOUNT;
        txtDiscount.Text = EDMEnt.DISCOUNT_PERCENT;
        txtDiscountAmount.Text = EDMEnt.DISCOUNT_AMOUNT;
        lblTotalAmount.Text = EDMEnt.TOTAL_AMOUNT;
        lblVAT.Text = EDMEnt.TAX_VAT_AMOUNT;
        lblGrandTotal.Text = EDMEnt.GRAND_TOTAL;
        txtRound.Text = EDMEnt.ROUND_OFF;
        lblInvoiceAmount.Text = EDMEnt.INVOICE_AMOUNT;
        LoadGrid(EDMEnt.PK_ID);
    }


    protected void LoadPaymentType(string type)
    {
        PTEnt = new PAYMENT_TYPE();
        PTEnt.SALES_PURCHASE = type;
        PTEnt.STATUS = "1";
        ddlPaymentType.DataSource = PTSEr.GetAll(PTEnt);
        ddlPaymentType.DataTextField = "PAYMENT_NAME";
        ddlPaymentType.DataValueField = "PAYMENT_CODE";
        ddlPaymentType.DataBind();
    }
    protected void LoadSuppliers()
    {
        SEnt = new SUPPLIERS();
        SEnt.SUPPLIER_TYPE = "1";//local supplier
        SEnt.STATUS = "1";
        ddlSupplier.DataSource = SSer.GetAll(SEnt);
        ddlSupplier.DataTextField = "SUPPLIER_NAME";
        ddlSupplier.DataValueField = "PK_ID";
        ddlSupplier.DataBind();
        ddlSupplier.Items.Insert(0, "Select");
    }
    protected void Loadexp()
    {
        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_MASTER_CODE = "0302";
        GLAEnt.STATUS = "1";
        ddlexp.DataSource = GLASer.GetAll(GLAEnt);
        ddlexp.DataTextField = "GL_NAME";
        ddlexp.DataValueField = "GL_CODE";
        ddlexp.DataBind();
        ddlexp.Items.Insert(0, "Select");
    }
    protected void LoadExpiryYear()
    {
        double startyear = DateTime.Now.Year;

        ArrayList year = new ArrayList();
        for (int i = 0; i < 10; i++)
        {
            year.Add(startyear.ToString());
            startyear++;
        }

        ddlExpYear.DataSource = year;
        ddlExpYear.DataBind();

    }

    protected DataTable CreateGridFirst()
    {
        int row = grdPurchaseDetail.Rows.Count;
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("exp_CODE");
        dummyTable.Columns.Add("exp");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("TOTAL");

        DataView dv = new DataView(dummyTable);

        grdPurchaseDetail.DataSource = dv;
        grdPurchaseDetail.DataBind();

        if (!PGPS.ProductBatch())
        {
            grdPurchaseDetail.Columns[3].Visible = false;
        }
        if (!PGPS.ProductExpDate())
        {
            grdPurchaseDetail.Columns[4].Visible = false;
        }


        return dummyTable;
    }

    protected DataTable LoadGrid(string PIM_pk_id)
    {
        EntityList theList = new EntityList();
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("exp_CODE");
        dummyTable.Columns.Add("exp");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("TOTAL");

        DataView dv = new DataView(dummyTable);

        ADDEnt = new EXPENDITURE_DAKHILA_DETAILS();
        ADDEnt.EXP_DAKHILA_ID = PIM_pk_id;
        theList = ADDSer.GetAll(ADDEnt);
        if (theList.Count > 0)
        {
            foreach (EXPENDITURE_DAKHILA_DETAILS PID in theList)
            {
                DataRow dummyRow = dummyTable.NewRow();

                GLAEnt = new GL_ACCOUNT();
                GLAEnt.GL_MASTER_CODE = "0302";
                GLAEnt.GL_CODE = PID.EXP_GL_CODE;
                GLAEnt = (GL_ACCOUNT)GLASer.GetSingle(GLAEnt);
                if (GLAEnt != null)
                {
                    dummyRow["PK_ID"] = GLAEnt.PK_ID;
                    dummyRow["exp_CODE"] = GLAEnt.GL_CODE;
                    dummyRow["exp"] = GLAEnt.GL_NAME;
                    dummyRow["QUANTITY"] = PID.QUANTITY;
                    dummyRow["RATE"] = Convert.ToDouble(PID.RATE).ToString("0.00");
                    dummyRow["TOTAL"] = Convert.ToDouble(PID.TOTAL).ToString("0.00");
                    dummyTable.Rows.Add(dummyRow);
                }
            }
        }
        grdPurchaseDetail.DataSource = dv;
        grdPurchaseDetail.DataBind();

        return dummyTable;
    }
    protected DataTable CreateGrid(int RowIndex, bool addRemoveFlag)
    {
        int row = grdPurchaseDetail.Rows.Count;
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("exp_CODE");
        dummyTable.Columns.Add("exp");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("TOTAL");

        if (row > 0)
        {
            foreach (GridViewRow r in grdPurchaseDetail.Rows)
            {
                Label lblPK_ID = r.FindControl("lblPK_ID") as Label;
                Label lblexpCode = r.FindControl("lblexpCode") as Label;
                Label lblexpName = r.FindControl("lblexpName") as Label;
                Label lblQty = r.FindControl("lblQty") as Label;
                Label lblUnit = r.FindControl("lblUnit") as Label;
                Label lblRate = r.FindControl("lblRate") as Label;
                Label lblItemTotal = r.FindControl("lblItemTotal") as Label;

                DataRow dummyRw = dummyTable.NewRow();

                if (lblPK_ID.Text == ddlexp.SelectedValue && lblRate.Text == txtRate.Text) // the selected exp is already in the grid update the qty and rate
                {
                    dummyRw["PK_ID"] = lblPK_ID.Text;
                    dummyRw["exp_CODE"] = lblexpCode.Text;
                    dummyRw["exp"] = lblexpName.Text;
                    dummyRw["QUANTITY"] = txtQty.Text;
                    dummyRw["UNIT"] = lblUnit.Text;
                    dummyRw["RATE"] = Convert.ToDouble(txtRate.Text).ToString("0.00");
                    dummyRw["TOTAL"] = Convert.ToDouble(txtAmount.Text).ToString("0.00");

                    flag = true;
                }

                else // the selected exp is not in the grid add the exp detail in the grid
                {
                    dummyRw["PK_ID"] = lblPK_ID.Text;
                    dummyRw["exp_CODE"] = lblexpCode.Text;
                    dummyRw["exp"] = lblexpName.Text;
                    dummyRw["QUANTITY"] = lblQty.Text;
                    dummyRw["UNIT"] = lblUnit.Text;
                    dummyRw["RATE"] = Convert.ToDouble(lblRate.Text).ToString("0.00");
                    dummyRw["TOTAL"] = Convert.ToDouble(lblItemTotal.Text).ToString("0.00");
                }

                dummyTable.Rows.Add(dummyRw);

                //  flag = false;

                if (r.RowIndex == RowIndex && addRemoveFlag == false)
                {
                    dummyTable.Rows.Remove(dummyRw);
                    flag = true;
                }
            }
        }


        if (flag == false)
        {
            DataRow dummyRow = dummyTable.NewRow();
            GLAEnt = new GL_ACCOUNT();

            GLAEnt.GL_CODE = ddlexp.SelectedValue;
            GLAEnt = (GL_ACCOUNT)GLASer.GetSingle(GLAEnt);
            if (GLAEnt != null)
            {

                dummyRow["PK_ID"] = GLAEnt.PK_ID;
                dummyRow["exp_CODE"] = GLAEnt.GL_CODE;
                dummyRow["exp"] = GLAEnt.GL_NAME;
                dummyRow["QUANTITY"] = txtQty.Text;
                dummyRow["RATE"] = Convert.ToDouble(txtRate.Text).ToString("0.00");
                dummyRow["TOTAL"] = Convert.ToDouble(txtAmount.Text).ToString("0.00");
                dummyTable.Rows.Add(dummyRow);

            }
        }
        DataView dv = new DataView(dummyTable);

        grdPurchaseDetail.DataSource = dummyTable;
        grdPurchaseDetail.DataBind();
        getTotal();
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
            Label lblItemTotal = gr.FindControl("lblItemTotal") as Label;
            total = total + (Convert.ToDouble(lblItemTotal.Text));
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
    protected void grdPurchaseDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            CreateGrid(gr.RowIndex, false);
        }
    }
    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        SEnt = new SUPPLIERS();
        if (ddlSupplier.SelectedValue != "Select")
        {
            SEnt.PK_ID = ddlSupplier.SelectedValue;
            SEnt = (SUPPLIERS)SSer.GetSingle(SEnt);
            if (SEnt != null)
            {
                txtSupplierCode.Text = SEnt.SUPPLIER_CODE;
                txtSupplierAddress.Text = SEnt.ADDRESS;
                txtSupplierPANVAT.Text = SEnt.VAT_PAN_NUMBER;
                txtContactNo.Text = SEnt.MOBILE;
                txtInvoiceNumber.Focus();
            }
        }
        else
        {
            txtSupplierCode.Text = "";
        }
    }
    protected void txtSupplierCode_TextChanged(object sender, EventArgs e)
    {
        txtSupplierCode.Text = txtSupplierCode.Text.ToUpper();
        SEnt = new SUPPLIERS();
        SEnt.SUPPLIER_CODE = txtSupplierCode.Text;
        SEnt = (SUPPLIERS)SSer.GetSingle(SEnt);
        if (SEnt != null)
        {
            ddlSupplier.SelectedValue = SEnt.PK_ID;
            txtSupplierAddress.Text = SEnt.ADDRESS;
            txtSupplierPANVAT.Text = SEnt.VAT_PAN_NUMBER;
            txtContactNo.Text = SEnt.MOBILE;
            txtInvoiceNumber.Focus();
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Not a valid Supplier Code.");
            txtSupplierCode.Text = "";
            txtSupplierAddress.Text = "";
            txtSupplierPANVAT.Text = "";
            txtContactNo.Text = "";
            txtSupplierCode.Focus();
        }
    }
    protected void ddlexp_SelectedIndexChanged(object sender, EventArgs e)
    {
        GLAEnt = new GL_ACCOUNT();
        if (ddlexp.SelectedValue != "Select")
        {
            GLAEnt.GL_CODE = ddlexp.SelectedValue;
            GLAEnt = (GL_ACCOUNT)GLASer.GetSingle(GLAEnt);
            if (GLAEnt != null)
            {
                txtexpCode.Text = GLAEnt.GL_CODE;
                txtQty.Focus();
                txtBatch.Focus();
                txtQty.Focus();
            }
        }
        else
        {
            txtexpCode.Text = "";
            txtexpCode.Focus();
        }
    }

    protected void txtexpCode_TextChanged(object sender, EventArgs e)
    {
        txtexpCode.Text = txtexpCode.Text.ToUpper();
        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_CODE = txtexpCode.Text;
        GLAEnt = (GL_ACCOUNT)GLASer.GetSingle(GLAEnt);
        if (GLAEnt != null && txtexpCode.Text != "")
        {
            ddlexp.SelectedValue = GLAEnt.GL_CODE;
            txtQty.Focus();
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Not a valid exp Code.");
            txtexpCode.Text = "";
            txtexpCode.Focus();
            ddlexp.SelectedValue = "Select";
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        double Qty = 0;
        double rate = 0;
        string msg = "";
        try
        {
            Qty = Convert.ToDouble(txtQty.Text);
        }
        catch
        {
            msg = "Enter Number Only. ";
            txtQty.Focus();
            txtQty.Text = "";
        }
        try
        {
            rate = Convert.ToDouble(txtRate.Text);
        }
        catch
        {
            msg = "Enter Number Only. ";
            txtRate.Focus();
            txtRate.Text = "";
        }
        if (ddlexp.SelectedItem.ToString() == "Select")
        {
            msg = msg + "Select exp";
        }
        if (msg == "")
        {
            CreateGrid(1, true);
            Clearexp();
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), msg);
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
    protected void Clearexp()
    {
        txtexpCode.Text = "";
        ddlexp.SelectedValue = "Select";
        txtQty.Text = "";
        txtRate.Text = "";
        txtAmount.Text = "";
        txtBatch.Text = "";
        txtexpCode.Focus();
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
        //ddlPaymentType.SelectedValue = "CP";
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



    protected void btnSavePurchase_Click(object sender, EventArgs e)
    {
        if (!IsPageRefresh)
        {
            string msg = "Please Enter ";
            try
            {
                if (chkWalkIn.Checked == false)
                {
                    if (ddlSupplier.SelectedValue == "Select")
                        msg = msg + "Supplier Name";
                }
                else
                {
                    if (txtSupplierName.Text == "")
                        txtSupplierName.Text = "CASH";
                }
                if (txtInvoiceNumber.Text == "")
                    msg = msg + " Invoice No";
                string[] chalandate = txtInvoiceDate.Text.Split('/');
                string date = PGD.ConvertNepaliTOEnglish(chalandate[0], chalandate[1], chalandate[2]);
                if (date == "")
                    msg = msg + " Date";
            }
            catch
            {
                msg = msg + " Valid Date";
            }
            if (msg == "Please Enter ")
            {
                if (grdPurchaseDetail.Rows.Count != 0)
                {
                    DistributedTransaction DT = new DistributedTransaction();
                    userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                    string pk_id = "";
                    //if lblOldPK_ID is not null then it is purchase edit
                    // in purchase edit, update purchase master, delete all purchase child and re insert purchase child

                    if (lblOldPK_ID.Text == "")//new entry
                    {
                        #region for new purchase entry
                        #region to insert in purchase master
                        EDMEnt = new EXPENDITURE_DAKHILA();
                        EDMEnt.BILL_NO = txtInvoiceNumber.Text;
                        string[] invoicedate = txtInvoiceDate.Text.Split('/');
                        EDMEnt.BILL_DATE = PGD.ConvertNepaliTOEnglish(invoicedate[0], invoicedate[1], invoicedate[2]);
                        EDMEnt.BILL_DAY = invoicedate[0];
                        EDMEnt.BILL_MONTH = invoicedate[1];
                        EDMEnt.BILL_YEAR = invoicedate[2];
                        EDMEnt.DAKHILA_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                        EDMEnt.DAKHILA_DAY = PGD.NepaliDay();
                        EDMEnt.DAKHILA_MONTH = PGD.NepaliMonth();
                        EDMEnt.DAKHILA_YEAR = PGD.NepaliYear();
                        EDMEnt.DAKHILA_FISCALYEAR = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                        if (chkWalkIn.Checked == false)
                        {
                            EDMEnt.SUPPLIER_ID = ddlSupplier.SelectedValue;
                        }


                        EDMEnt.APPROVE_BY = userProfile.EmployeeID;
                        EDMEnt.SUB_TOTAL = lblSubTotalAmount.Text; // item wise total
                        EDMEnt.TOTAL = lblTotalAmount.Text; // amount after discount
                        EDMEnt.VAT = lblVAT.Text;
                        EDMEnt.OFFICE_CODE = userProfile.LocationID;
                        EDMEnt.GRAND_TOTAL = lblGrandTotal.Text; //amount after addition of VAT
                        pk_id = EDMSer.Insert(EDMEnt, DT).ToString();
                        #endregion
                        #region to insert in to purchase  detail
                        foreach (GridViewRow gr in grdPurchaseDetail.Rows)
                        {
                            Label lblSno = (Label)gr.FindControl("lblSno");
                            Label lblPK_ID = (Label)gr.FindControl("lblPK_ID");
                            Label lblQty = (Label)gr.FindControl("lblQty");
                            Label lblRate = (Label)gr.FindControl("lblRate");
                            Label lblItemTotal = (Label)gr.FindControl("lblItemTotal");

                            ADDEnt = new EXPENDITURE_DAKHILA_DETAILS();
                            ADDEnt.PK_ID = pk_id;
                            ADDEnt.SNO = lblSno.Text;
                            ADDEnt.EXP_DAKHILA_ID = lblPK_ID.Text;
                            ADDEnt.QUANTITY = lblQty.Text;
                            ADDEnt.RATE = lblRate.Text;
                            ADDEnt.TOTAL = lblItemTotal.Text;
                            ADDEnt.OFFICE_CODE = userProfile.LocationID;
                            ADDSer.Insert(ADDEnt, DT);

                        }
                        #endregion

                        #endregion
                    }
                    else
                    {
                        #region to update old purchase entry
                        #region to update purchase master
                        EDMEnt = new EXPENDITURE_DAKHILA();
                        EDMEnt.PK_ID = lblOldPK_ID.Text;
                        EDMEnt = (EXPENDITURE_DAKHILA)EDMSer.GetSingle(EDMEnt);
                        if (EDMEnt != null)
                        {
                            EDMEnt.BILL_NO = txtInvoiceNumber.Text;
                            string[] invoicedate = txtInvoiceDate.Text.Split('/');
                            EDMEnt.BILL_DATE = PGD.ConvertNepaliTOEnglish(invoicedate[0], invoicedate[1], invoicedate[2]);
                            EDMEnt.BILL_DAY = invoicedate[0];
                            EDMEnt.BILL_MONTH = invoicedate[1];
                            EDMEnt.BILL_YEAR = invoicedate[2];
                            EDMEnt.DAKHILA_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                            EDMEnt.DAKHILA_DAY = PGD.NepaliDay();
                            EDMEnt.DAKHILA_MONTH = PGD.NepaliMonth();
                            EDMEnt.DAKHILA_YEAR = PGD.NepaliYear();
                            EDMEnt.DAKHILA_FISCALYEAR = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                            EDMEnt.SUPPLIER_ID = ddlSupplier.SelectedValue;
                            EDMEnt.APPROVE_BY = userProfile.EmployeeID;
                            EDMEnt.SUB_TOTAL = lblSubTotalAmount.Text; // item wise total
                            EDMEnt.DISCOUNT = txtDiscountAmount.Text;
                            EDMEnt.SUB_TOTAL = lblTotalAmount.Text; // amount after discount
                            EDMEnt.VAT = lblVAT.Text;
                            EDMEnt.OFFICE_CODE = userProfile.LocationID;
                            EDMEnt.GRAND_TOTAL = lblGrandTotal.Text; //amount after addition of VAT
                            EDMSer.Update(EDMEnt, DT);
                            pk_id = lblOldPK_ID.Text;

                            #region to cancel previous vouchers
                            VMEnt = new VOUCHER_MASTER();
                            VMEnt.REF_TABLE = "Purchase Invoice";
                            VMEnt.REF_ID = EDMEnt.PK_ID;
                            VMEnt.STATUS = "2";
                            EntityList theVouchers = VMSer.GetAll(VMEnt, DT);
                            foreach (VOUCHER_MASTER VM in theVouchers)
                            {
                                VM.STATUS = "-1";
                                VMSer.Update(VM, DT);

                                VAREnt = new VOUCHER_ALTER_REQUEST();
                                VAREnt.VOUCHER_PK_ID = VM.PK_ID;
                                VAREnt.ALTER_DETAIL = "Purchase Edit";
                                VAREnt.ALTERED_BY = userProfile.EmployeeID;
                                VAREnt.ALTERED_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                                VAREnt.STATUS = "2";
                                VARSer.Insert(VAREnt, DT);
                            }

                            #endregion
                        }

                        #endregion

                        ADDEnt = new EXPENDITURE_DAKHILA_DETAILS();
                        ADDEnt.EXP_DAKHILA_ID = lblOldPK_ID.Text;
                        ADDSer.Delete(ADDEnt, DT);

                        // pahile bhaye ko invoice ko sabai detail lai delete garcha aani pheri insert garcha
                        #region to insert in to purchase  detail
                        foreach (GridViewRow gr in grdPurchaseDetail.Rows)
                        {
                            Label lblSno = (Label)gr.FindControl("lblSno");
                            Label lblPK_ID = (Label)gr.FindControl("lblPK_ID");
                            Label lblQty = (Label)gr.FindControl("lblQty");
                            Label lblRate = (Label)gr.FindControl("lblRate");
                            Label lblItemTotal = (Label)gr.FindControl("lblItemTotal");

                            ADDEnt = new EXPENDITURE_DAKHILA_DETAILS();
                            ADDEnt.EXP_DAKHILA_ID = pk_id;
                            ADDEnt.SNO = lblSno.Text;
                            ADDEnt.EXP_GL_CODE = lblPK_ID.Text;

                            ADDEnt.QUANTITY = lblQty.Text;
                            ADDEnt.RATE = lblRate.Text;
                            ADDEnt.TOTAL = lblItemTotal.Text;
                            ADDEnt.OFFICE_CODE = userProfile.LocationID;
                            ADDSer.Insert(ADDEnt, DT);

                        }
                        #endregion

                        #endregion
                    }
                    if (fileAttachment.HasFile)
                    {
                        string lblMsg;
                        try
                        {
                            // Specify the folder path where the image will be saved
                            string folderPath = Server.MapPath("~/images/Purchae_Bill_Img/");

                            // Ensure the folder exists; create it if it does not exist
                            if (!System.IO.Directory.Exists(folderPath))
                            {
                                System.IO.Directory.CreateDirectory(folderPath);
                            }

                            // Validate the uploaded file type (allow only image files)
                            string fileExtension = System.IO.Path.GetExtension(fileAttachment.FileName).ToLower();
                            if (fileExtension != ".jpg" || fileExtension != ".jpeg" || fileExtension != ".png" || fileExtension != ".pdf")
                            {
                                lblMsg = "Only Images are allowed.";
                            }

                            // Generate a new file name (e.g., based on some unique identifier)
                            string fileName = "Purchase_Bill" + pk_id + fileExtension;

                            // Combine the folder path with the new file name
                            string filePath = System.IO.Path.Combine(folderPath, fileName);

                            // Save the uploaded file to the specified path
                            fileAttachment.SaveAs(filePath);
                        }
                        catch (Exception ex)
                        {
                            // Handle and display errors
                            lblMsg = "An error occurred while uploading the file: " + ex.Message;
                        }
                    }
                    #region account portion                       
                    double round = 0;
                    double round_value = 0;

                    #region to insert in voucher master and child 
                    // credit ma purchase gare ko cha bhaye JV banaunu parcha to effect supplier
                    // walking supplier sanga credit ma purchase huna so JV bandaina
                    // walking supplier ko lagi sidao payment voucher (debit Voucher Bancha)
                    #region to insert in Voucher Master

                    VMEnt = new VOUCHER_MASTER();
                    if (chkWalkIn.Checked == false) // party bhaye matra journal bancha nabhaye sidai Debit voucher bancha
                    {
                        VMEnt.VOUCHER_TYPE = "JV";
                        VMEnt.VOUCHER_NUMBER = af.getNext_VM_ID("JV", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfile.LocationID);
                    }
                    else
                    {
                        VMEnt.VOUCHER_TYPE = "DV";
                        VMEnt.VOUCHER_NUMBER = af.getNext_VM_ID("DV", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfile.LocationID);
                    }
                    VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.VOUCHER_DAY = PGD.NepaliDay();
                    VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
                    VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
                    VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                    VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.TRN_AMOUNT = lblInvoiceAmount.Text;
                    if (chkWalkIn.Checked == false) // party bhaye matra journal bancha nabhaye sidai Debit voucher bancha
                    {
                        VMEnt.NARRATION = "exp Purchase from " + ddlSupplier.SelectedItem.ToString() + ". Invoice no." + txtInvoiceNumber.Text;
                    }
                    else
                    {
                        VMEnt.NARRATION = "Cash Purchase from Invoice no." + txtInvoiceNumber.Text + ". " + txtSupplierName.Text;
                    }
                    VMEnt.REF_TABLE = "exp Dakhila";
                    VMEnt.REF_ID = pk_id;
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
                    voucher_pk_id = VMSer.Insert(VMEnt, DT).ToString();
                    #endregion

                    EDMEnt = new EXPENDITURE_DAKHILA();
                    EDMEnt.PK_ID = pk_id;
                    EDMEnt = (EXPENDITURE_DAKHILA)EDMSer.GetSingle(EDMEnt, DT);
                    if (EDMEnt != null)
                    {
                        EDMEnt.VM_ID = voucher_pk_id;
                        EDMSer.Update(EDMEnt, DT);
                    }
                    #region to insert in to voucher child
                    int sno = 1;
                    #region for Dr Part
                    #region for Purchase
                    foreach (GridViewRow gvr in grdPurchaseDetail.Rows)
                    {
                        Label lblexpCode = gvr.FindControl("lblexpCode") as Label;
                        Label lblItemTotal = gvr.FindControl("lblItemTotal") as Label;

                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = voucher_pk_id;
                        VCEnt.SNO = sno.ToString();
                        VCEnt.GL_CODE = lblexpCode.Text;
                        VCEnt.SGL_CODE = "";
                        VCEnt.DR_AMOUNT = lblItemTotal.Text;
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
                    VCEnt.DR_AMOUNT = lblVAT.Text;
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCEnt.CR_AMOUNT = "0";
                    VCEnt.REMARKS = "By";
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
                    VCEnt.DR_AMOUNT = round.ToString();
                    VCEnt.CR_AMOUNT = "0";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCEnt.REMARKS = "By";
                    if (round_value > 0) /// discount cha bhaye matra insert garni
                    {
                        VCSer.Insert(VCEnt, DT);
                        sno++;
                    }

                    #endregion
                    #endregion
                    #region for Cr Part


                    #region for Cash or credit(Supplier)
                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = voucher_pk_id;
                    VCEnt.SNO = sno.ToString();
                    if (chkWalkIn.Checked == false) // it is supplier
                    {
                        VCEnt.GL_CODE = "040301"; //GL_CODE of Sundry Creditor Local Supplier
                        VCEnt.SGL_CODE = txtSupplierCode.Text;
                    }
                    else // is is not supplier -> walkin supplier -> walkin supplier bhaye sidai cash garni
                    {
                        VCEnt.GL_CODE = "010102"; //GL_CODE of Cash
                    }
                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = lblInvoiceAmount.Text;
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCEnt.REMARKS = "To";
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
                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = txtDiscountAmount.Text;
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCEnt.REMARKS = "To";
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
                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = round.ToString();
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCEnt.REMARKS = "To";
                    if (round_value < 0) /// discount cha bhaye matra insert garni
                    {
                        VCSer.Insert(VCEnt, DT);
                        sno++;
                    }

                    #endregion
                    #endregion
                    #endregion

                    #endregion

                    #region to insert in voucher master and child if purchase is made from cash  with supplier                    
                    // supplier sanga credit ma purchase gare ko bhaye JV bani sake pachi Payment voucher(DV) bancha 
                    //bank (cheque or QR) bata bahye manually payment voucher banaunu parcha, mode of payment ma credit gare ra 
                    if (ddlPaymentType.SelectedValue != "CR" && chkWalkIn.Checked == false)
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
                        VMEnt.TRN_AMOUNT = lblInvoiceAmount.Text;
                        if (chkSuppliers.Checked == false)
                            VMEnt.NARRATION = "Payment made to " + ddlSupplier.SelectedItem.ToString() + " for Invoice no." + txtInvoiceNumber.Text;
                        else
                            VMEnt.NARRATION = "Payment made to " + txtSupplierName.Text + " for Invoice no." + txtInvoiceNumber.Text;
                        VMEnt.REF_TABLE = "Expenditure Dakhila";
                        VMEnt.REF_ID = pk_id;
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
                        voucher_pk_id = VMSer.Insert(VMEnt, DT).ToString();
                        #endregion
                        #region to insert in to voucher child
                        #region for Dr part
                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = voucher_pk_id;
                        VCEnt.SNO = "1";
                        VCEnt.GL_CODE = "040301"; //GL_CODE of Sundry Creditor Local Supplier
                        VCEnt.SGL_CODE = txtSupplierCode.Text;
                        VCEnt.DR_AMOUNT = lblInvoiceAmount.Text;
                        VCEnt.CR_AMOUNT = "0";
                        VCEnt.REMARKS = "By";
                        VCEnt.OFFICE_CODE = userProfile.LocationID;
                        VCSer.Insert(VCEnt, DT);
                        #endregion

                        #region for Cr Part
                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = voucher_pk_id;
                        VCEnt.SNO = "2";
                        VCEnt.GL_CODE = "010102"; //GL_CODE of Cash
                        VCEnt.DR_AMOUNT = "0";
                        VCEnt.CR_AMOUNT = lblInvoiceAmount.Text;
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
                        EDMEnt = new EXPENDITURE_DAKHILA();
                        EDMEnt.PK_ID = pk_id;
                        EDMEnt = (EXPENDITURE_DAKHILA)EDMSer.GetSingle(EDMEnt);
                        if (EDMEnt != null)
                        {
                            HelperFunction.MsgBox(this, this.GetType(), "Your Dakhila Number is " + EDMEnt.DAKHILA_NO);
                        }
                        CreateGridFirst();
                        Clear();

                    }
                    else
                    {
                        DT.Abort();
                        HelperFunction.MsgBox(this, this.GetType(), "Sorry Something Goes Wrong.");
                    }
                    DT.Dispose();
                }
            }
            else
                HelperFunction.MsgBox(this, this.GetType(), msg);
        }
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
    protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void chkSuppliers_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSuppliers.Checked == true)
        {
            divSupplier.Visible = false;
            divWalkinSupplier.Visible = true;
            chkWalkIn.Checked = true;
            ddlPaymentType.SelectedValue = "CP";
            ddlPaymentType.Enabled = false;
        }

    }
    protected void chkWalkIn_CheckedChanged(object sender, EventArgs e)
    {
        if (chkWalkIn.Checked == false)
        {
            divSupplier.Visible = true;
            divWalkinSupplier.Visible = false;
            chkSuppliers.Checked = false;
            ddlPaymentType.Enabled = true;
        }
    }
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        double Qty = 0;
        double rate = 0;
        string msg = "";
        try
        {
            Qty = Convert.ToDouble(txtQty.Text);
        }
        catch
        {
            msg = "Enter Number Only. ";
            txtQty.Focus();
            txtQty.Text = "";
            HelperFunction.MsgBox(this, this.GetType(), msg);
        }
        try
        {
            rate = Convert.ToDouble(txtRate.Text);
            txtAmount.Text = (Qty * rate).ToString("#0.00");
        }
        catch
        {
            txtRate.Focus();
            txtRate.Text = "";
        }

    }
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        double Qty = 0;
        double rate = 0;
        string msg = "";
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
            rate = Convert.ToDouble(txtRate.Text);
            txtRate.Text = Convert.ToDouble(txtRate.Text).ToString("#0.00");
        }
        catch
        {
            msg = "Enter Number Only. ";
            txtRate.Focus();
            txtRate.Text = "";
            HelperFunction.MsgBox(this, this.GetType(), msg);
        }
        txtAmount.Text = (Qty * rate).ToString("#0.00");
    }
    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        double Qty = 0;
        double amount = 0;
        string msg = "";
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
            amount = Convert.ToDouble(txtAmount.Text);
            txtAmount.Text = Convert.ToDouble(txtAmount.Text).ToString("#0.00");
        }
        catch
        {
            msg = "Enter Number Only. ";
            txtAmount.Focus();
            txtAmount.Text = "";
            HelperFunction.MsgBox(this, this.GetType(), msg);
        }
        txtRate.Text = (amount / Qty).ToString("#0.00");
    }




}