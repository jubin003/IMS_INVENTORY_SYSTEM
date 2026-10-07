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
using System.IO;

public partial class Utilities_Purchase_ProductImport : System.Web.UI.Page
{
    PURCHASE_INVOICE_MASTER PIMEnt = new PURCHASE_INVOICE_MASTER();
    PURCHASE_INVOICE_MASTERService PIMSer = new PURCHASE_INVOICE_MASTERService();

    PURCHASE_INVOICE_DETAIL PIDEnt = new PURCHASE_INVOICE_DETAIL();
    PURCHASE_INVOICE_DETAILService PIDSer = new PURCHASE_INVOICE_DETAILService();

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

    GL_ACCOUNT GLAEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLASer = new GL_ACCOUNTService();

    GL_SUB_ACCOUNT GLSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GLSASer = new GL_SUB_ACCOUNTService();

    FOREIGN_CURRENCY FCEnt = new FOREIGN_CURRENCY();
    FOREIGN_CURRENCYService FCSer = new FOREIGN_CURRENCYService();

    IMPORT_INVOICE_DETAIL IIDEnt = new IMPORT_INVOICE_DETAIL();
    IMPORT_INVOICE_DETAILService IIDSer = new IMPORT_INVOICE_DETAILService();

    AGENT AEnt = new AGENT();
    AGENTService ASer = new AGENTService();

    BANK_ACCOUNT BAEnt = new BANK_ACCOUNT();
    BANK_ACCOUNTService BASer = new BANK_ACCOUNTService();

    UserProfileEntity userProfile = new UserProfileEntity();
    HelperFunction hf = new HelperFunction();

    AccountFunction af = new AccountFunction();

    Boolean flag = false;
    Boolean IsPageRefresh = false;
    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGan PG = new PhyeGan();
    string old_pk_id = "";
    string random_number = "";
    string responseurl = "~/Login.aspx";
    string errmsg = "";
    static string path = "";
    string voucher_pk_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                ViewState["postids"] = System.Guid.NewGuid().ToString();
                Session["postid"] = ViewState["postids"].ToString();
                userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
                if (hf.checkPageAccess(path, userProfile.UserGroupID.ToString()))
                {
                    LoadCurrency();
                    LoadSuppliers();
                    LoadProduct();
                    LoadProductUnit();
                    LoadPaymentType("I"); // payment type of import
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
                    txtSupplierCode.Focus();
                }
                else
                {
                    Response.Redirect("~/forbidden.aspx");
                }

            }
            catch (Exception ww)
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }

    #region step 1 : Product Import

    protected void LoadPurchaseDetail(PURCHASE_INVOICE_MASTER PIMEnt)
    {
        SEnt = new SUPPLIERS();
        SEnt.PK_ID = PIMEnt.SUPPLIER_ID;
        SEnt = (SUPPLIERS)SSer.GetSingle(SEnt);
        if (SEnt != null)
        {
            txtSupplierCode.Text = SEnt.SUPPLIER_CODE;
            ddlSupplier.SelectedValue = SEnt.PK_ID;
        }

        txtSupplierPANVAT.Text = PIMEnt.SUPPLIER_PAN_VAT;
        txtSupplierAddress.Text = PIMEnt.SUPPLIER_ADDRESS;
        txtContactNo.Text = PIMEnt.SUPPLIER_CONTACT;
        txtInvoiceNumber.Text = PIMEnt.SUPPLIER_INVOICE_NO;
        txtInvoiceDate.Text = PIMEnt.INVOICE_DAY + "/" + PIMEnt.INVOICE_MONTH + "/" + PIMEnt.INVOICE_YEAR;
        txtDakhilaDate.Text = PIMEnt.DAKHILA_DAY + "/" + PIMEnt.DAKHILA_MONTH + "/" + PIMEnt.DAKHILA_YEAR;
        ddlPaymentType.SelectedValue = PIMEnt.PURCHASE_TYPE_ID;
        LoadGrid(PIMEnt.PK_ID);
    }
    protected void LoadCurrency()
    {
        FCEnt = new FOREIGN_CURRENCY();
        FCEnt.STATUS = "1";
        ddlCurrency.DataSource = FCSer.GetAll(FCEnt);
        ddlCurrency.DataValueField = "PK_ID";
        ddlCurrency.DataTextField = "CURRENCY";
        ddlCurrency.DataBind();
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
        SEnt.STATUS = "1";
        SEnt.SUPPLIER_TYPE = "2"; // Foregin Supplier
        ddlSupplier.DataSource = SSer.GetAll(SEnt);
        ddlSupplier.DataTextField = "SUPPLIER_NAME";
        ddlSupplier.DataValueField = "PK_ID";
        ddlSupplier.DataBind();
        ddlSupplier.Items.Insert(0, "Select");
    }
    protected void LoadProduct()
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        PEnt = new PRODUCT();
        PEnt.STATUS = "1";
        PEnt.OFFICE_CODE = userProfile.LocationID;
        EntityList theList = new EntityList();
        theList = PSer.GetAll(PEnt);
        ddlProduct.DataSource = theList;

        ddlProduct.DataTextField = "PRODUCT_FULLNAME";
        ddlProduct.DataValueField = "PK_ID";
        ddlProduct.DataBind();
        ddlProduct.Items.Insert(0, "Select");
    }
    protected void LoadProductUnit()
    {
        PUEnt = new PRODUCT_UNIT();
        ddlUnit.DataSource = PUSer.GetAll(PUEnt);
        ddlUnit.DataTextField = "UNIT_NAME";
        ddlUnit.DataValueField = "PK_ID";
        ddlUnit.DataBind();
        ddlUnit.Items.Insert(0, "");
    }
    protected DataTable CreateGridFirst()
    {
        int row = grdPurchaseDetail.Rows.Count;
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("PRODUCT_CODE");
        dummyTable.Columns.Add("PRODUCT");
        dummyTable.Columns.Add("BATCH_NO");
        dummyTable.Columns.Add("EXPIRY_DATE");
        dummyTable.Columns.Add("WEIGHT");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("FC_RATE");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("EXCHANGE_RATE");
        dummyTable.Columns.Add("FC_TOTAL");
        dummyTable.Columns.Add("TOTAL");
        dummyTable.Columns.Add("PP_TOTAL");

        DataView dv = new DataView(dummyTable);

        grdPurchaseDetail.DataSource = dv;
        grdPurchaseDetail.DataBind();

        return dummyTable;
    }
    protected DataTable LoadGrid(string PIM_pk_id)
    {
        EntityList theList = new EntityList();
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("PRODUCT_CODE");
        dummyTable.Columns.Add("PRODUCT");
        dummyTable.Columns.Add("BATCH_NO");
        dummyTable.Columns.Add("EXPIRY_DATE");
        dummyTable.Columns.Add("WEIGHT");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("FC_RATE");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("EXCHANGE_RATE");
        dummyTable.Columns.Add("FC_TOTAL");
        dummyTable.Columns.Add("TOTAL");
        dummyTable.Columns.Add("PP_TOTAL");

        DataView dv = new DataView(dummyTable);

        PIDEnt = new PURCHASE_INVOICE_DETAIL();
        PIDEnt.PURCHASE_INVOICE_ID = PIM_pk_id;
        theList = PIDSer.GetAll(PIDEnt);
        if (theList.Count > 0)
        {
            foreach (PURCHASE_INVOICE_DETAIL PID in theList)
            {
                DataRow dummyRow = dummyTable.NewRow();

                PEnt = new PRODUCT();
                PEnt.PK_ID = PID.PRODUCT_ID;
                PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                if (PEnt != null)
                {
                    dummyRow["PK_ID"] = PEnt.PK_ID;
                    dummyRow["PRODUCT_CODE"] = PEnt.PRODUCT_CODE;
                    dummyRow["PRODUCT"] = PEnt.PRODUCT_NAME;
                    dummyRow["BATCH_NO"] = PID.BATCH_NUMBER;
                    dummyRow["EXPIRY_DATE"] = PID.EXPIRY_DATE;
                    dummyRow["WEIGHT"] = PID.WEIGHT;
                    dummyRow["QUANTITY"] = PID.QUANTITY;
                    dummyRow["UNIT"] = hf.getProductUnit(PEnt.PK_ID);
                    dummyRow["FC_RATE"] = Convert.ToDouble(PID.FC_RATE).ToString("0.00");
                    dummyRow["RATE"] = Convert.ToDouble(PID.RATE).ToString("0.00");
                    dummyRow["EXCHANGE_RATE"] = Convert.ToDouble(PID.CONVERSION_RATE).ToString("0.00");
                    dummyRow["FC_TOTAL"] = Convert.ToDouble(PID.FC_SUB_TOTAL).ToString("0.00");
                    dummyRow["TOTAL"] = Convert.ToDouble(PID.SUB_TOTAL).ToString("0.00");
                    dummyRow["PP_TOTAL"] = Convert.ToDouble(PID.IMPORT_AMOUNT).ToString("0.00");
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
        dummyTable.Columns.Add("PRODUCT_CODE");
        dummyTable.Columns.Add("PRODUCT");
        dummyTable.Columns.Add("BATCH_NO");
        dummyTable.Columns.Add("EXPIRY_DATE");
        dummyTable.Columns.Add("WEIGHT");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("FC_RATE");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("EXCHANGE_RATE");
        dummyTable.Columns.Add("FC_TOTAL");
        dummyTable.Columns.Add("TOTAL");
        dummyTable.Columns.Add("PP_TOTAL");

        if (row > 0)
        {
            foreach (GridViewRow r in grdPurchaseDetail.Rows)
            {
                Label lblPK_ID = r.FindControl("lblPK_ID") as Label;
                Label lblProductCode = r.FindControl("lblProductCode") as Label;
                Label lblProductName = r.FindControl("lblProductName") as Label;
                Label lblBatch = r.FindControl("lblBatch") as Label;
                Label lblExpDate = r.FindControl("lblExpDate") as Label;
                Label lblQty = r.FindControl("lblQty") as Label;
                Label lblWeight = r.FindControl("lblWeight") as Label;
                Label lblUnit = r.FindControl("lblUnit") as Label;
                Label lblFCRate = r.FindControl("lblFCRate") as Label;
                Label lblRate = r.FindControl("lblRate") as Label;
                Label lblExchangeRate = r.FindControl("lblExchangeRate") as Label;
                Label lblFC_ItemTotal = r.FindControl("lblFC_ItemTotal") as Label;
                Label lblItemTotal = r.FindControl("lblItemTotal") as Label;
                Label lblPPItemTotal = r.FindControl("lblPPItemTotal") as Label;

                DataRow dummyRw = dummyTable.NewRow();

                if (lblPK_ID.Text == ddlProduct.SelectedValue && lblRate.Text == txtRate.Text) // the selected product is already in the grid update the qty and rate
                {
                    dummyRw["PK_ID"] = lblPK_ID.Text;
                    dummyRw["PRODUCT_CODE"] = lblProductCode.Text;
                    dummyRw["PRODUCT"] = lblProductName.Text;
                    dummyRw["BATCH_NO"] = lblBatch.Text;
                    dummyRw["EXPIRY_DATE"] = lblExpDate.Text;
                    dummyRw["WEIGHT"] = lblWeight.Text;
                    dummyRw["QUANTITY"] = txtQty.Text;
                    dummyRw["UNIT"] = hf.getProductUnit(lblPK_ID.Text);
                    dummyRw["FC_RATE"] = Convert.ToDouble(txtRate.Text).ToString("0.00");
                    dummyRw["RATE"] = (Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString("0.00");
                    dummyRw["EXCHANGE_RATE"] = txtExchangeRate.Text;
                    dummyRw["FC_TOTAL"] = Convert.ToDouble(txtAmount.Text).ToString("0.00");
                    dummyRw["TOTAL"] = (Convert.ToDouble(txtAmount.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString("0.00");
                    dummyRw["PP_TOTAL"] = (Convert.ToDouble(txtPPAmount.Text)).ToString("0.00");

                    flag = true;
                }

                else // the selected product is not in the grid add the product detail in the grid
                {
                    dummyRw["PK_ID"] = lblPK_ID.Text;
                    dummyRw["PRODUCT_CODE"] = lblProductCode.Text;
                    dummyRw["PRODUCT"] = lblProductName.Text;
                    dummyRw["BATCH_NO"] = lblBatch.Text;
                    dummyRw["EXPIRY_DATE"] = lblExpDate.Text;
                    dummyRw["WEIGHT"] = lblWeight.Text;
                    dummyRw["QUANTITY"] = lblQty.Text;
                    dummyRw["UNIT"] = hf.getProductUnit(lblPK_ID.Text);
                    dummyRw["FC_RATE"] = Convert.ToDouble(lblFCRate.Text).ToString("0.00");
                    dummyRw["RATE"] = Convert.ToDouble(lblRate.Text).ToString("0.00");
                    dummyRw["EXCHANGE_RATE"] = lblExchangeRate.Text;
                    dummyRw["FC_TOTAL"] = Convert.ToDouble(lblFC_ItemTotal.Text).ToString("0.00");
                    dummyRw["TOTAL"] = Convert.ToDouble(lblItemTotal.Text).ToString("0.00");
                    dummyRw["PP_TOTAL"] = Convert.ToDouble(lblPPItemTotal.Text).ToString("0.00");
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

            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlProduct.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                dummyRow["PK_ID"] = PEnt.PK_ID;
                dummyRow["PRODUCT_CODE"] = PEnt.PRODUCT_CODE;
                dummyRow["PRODUCT"] = PEnt.PRODUCT_NAME;
                dummyRow["BATCH_NO"] = "";
                dummyRow["EXPIRY_DATE"] = "";
                dummyRow["WEIGHT"] = txtWeight.Text;
                dummyRow["QUANTITY"] = txtQty.Text;
                dummyRow["UNIT"] = hf.getProductUnit(PEnt.PK_ID);
                dummyRow["FC_RATE"] = Convert.ToDouble(txtRate.Text).ToString("0.00");
                dummyRow["RATE"] = (Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString("0.00");
                dummyRow["EXCHANGE_RATE"] = txtExchangeRate.Text;
                dummyRow["FC_TOTAL"] = Convert.ToDouble(txtAmount.Text).ToString("0.00");
                dummyRow["TOTAL"] = (Convert.ToDouble(txtAmount.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString("0.00");
                dummyRow["PP_TOTAL"] = (Convert.ToDouble(txtPPAmount.Text)).ToString("0.00");
                dummyTable.Rows.Add(dummyRow);
            }
        }

        DataView dv = new DataView(dummyTable);

        grdPurchaseDetail.DataSource = dummyTable;
        grdPurchaseDetail.DataBind();
        grdPurchaseDetail.Columns[6].Visible = true;
        grdPurchaseDetail.Columns[9].Visible = true;

        getTotal();
        return dummyTable;
    }
    protected void getTotal()
    {
        double total = 0;
        double PPtotal = 0;
        foreach (GridViewRow gr in grdPurchaseDetail.Rows)
        {
            Label lblItemTotal = gr.FindControl("lblItemTotal") as Label;
            Label lblPPItemTotal = gr.FindControl("lblPPItemTotal") as Label;
            total = total + (Convert.ToDouble(lblItemTotal.Text));
            PPtotal = PPtotal + (Convert.ToDouble(lblPPItemTotal.Text));
        }
        lblInvoiceTotal.Text = total.ToString("0.00");
        lblKharidKhataAmount.Text = PPtotal.ToString("0.00");
        lblKharidKhataVAT.Text = (Convert.ToDouble(lblKharidKhataAmount.Text) * Convert.ToDouble(txtKharidKhataVAT.Text) / 100).ToString("0");
        lblKharidKhataTotal.Text = (Convert.ToDouble(lblKharidKhataAmount.Text) + Convert.ToDouble(lblKharidKhataVAT.Text)).ToString("0");
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
    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        PEnt = new PRODUCT();
        if (ddlProduct.SelectedValue != "Select")
        {
            PEnt.PK_ID = ddlProduct.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                txtProductCode.Text = PEnt.PRODUCT_CODE;
                txtQty.Focus();
                ddlUnit.SelectedValue = PEnt.UNIT_ID;
            }
        }
        else
        {
            txtProductCode.Text = "";
            txtProductCode.Focus();
            ddlUnit.SelectedValue = "";
        }
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        txtProductCode.Text = txtProductCode.Text.ToUpper();
        PEnt = new PRODUCT();
        PEnt.PRODUCT_CODE = txtProductCode.Text;
        PEnt = (PRODUCT)PSer.GetSingle(PEnt);
        if (PEnt != null && txtProductCode.Text != "")
        {
            ddlProduct.SelectedValue = PEnt.PK_ID;
            txtQty.Focus();
            ddlUnit.SelectedValue = PEnt.UNIT_ID;
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Not a valid Product Code.");
            txtProductCode.Text = "";
            txtProductCode.Focus();
            ddlProduct.SelectedValue = "Select";
            ddlUnit.SelectedValue = "";
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
        if (ddlProduct.SelectedItem.ToString() == "Select")
        {
            msg = msg + "Select Product";
        }
        if (msg == "")
        {
            CreateGrid(1, true);
            ClearProduct();
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), msg);
        }
    }
    protected void ClearProduct()
    {
        txtProductCode.Text = "";
        ddlProduct.SelectedValue = "Select";
        txtQty.Text = "";
        ddlUnit.SelectedValue = "";
        txtRate.Text = "";
        txtAmount.Text = "";
        txtPPAmount.Text = "";
        txtProductCode.Focus();
        txtWeight.Text = "";
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
        divSupplier.Visible = true;
        ddlPaymentType.Enabled = true;
        txtSupplierCode.Focus();
    }
    protected void txtDiscount_TextChanged(object sender, EventArgs e)
    {
        getTotal();
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
            txtPPAmount.Focus();
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
            txtPPAmount.Focus();
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

    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCurrency.SelectedValue != "1")
        {
            FCEnt = new FOREIGN_CURRENCY();
            FCEnt.PK_ID = ddlCurrency.SelectedValue;
            FCEnt = (FOREIGN_CURRENCY)FCSer.GetSingle(FCEnt);
            if (FCEnt != null)
            {
                //txtExchangeRate.Text = FCEnt.EXCHANGE_RATE;
            }
        }
        else
        {
            txtExchangeRate.Text = "1";
        }
    }
    #endregion
    protected void btnMoveToCustom_Click(object sender, EventArgs e)
    {
        if (txtSupplierCode.Text == "")
        {
            errmsg = "Select Suppliers. ";
        }
        if (txtPPNumber.Text == "")
        {
            errmsg += "Enter Pragapayan Number. ";
        }
        if (grdPurchaseDetail.Rows.Count == 0)
        {
            errmsg += "Add Product to Purchase";
        }
        if (errmsg == "")
        {
            divStep1.Visible = false;
            divStep2.Visible = true;
            divStep3.Visible = false;
            divStep4.Visible = false;
            divStep5.Visible = false;
            divStep6.Visible = false;
        }

        lblerror.Text = errmsg;
        if (lblTotalImportExpenses.Text == "") // Previous gare ra pheri next garyo bahne grid ko value khali na garuana lai 
        {
            CustomExpensesAgent();
            LoadImportGrid();
            LoadCustom();
        }
    }
    protected void btnBackToInvoice_Click(object sender, EventArgs e)
    {
        divStep1.Visible = true;
        divStep2.Visible = false;
        divStep3.Visible = false;
        divStep4.Visible = false;
        divStep5.Visible = false;
        divStep6.Visible = false;
    }

    #region step 2 : Custom Agent
    protected void LoadCustom()
    {
        GLSAEnt = new GL_SUB_ACCOUNT();
        GLSAEnt.GL_CODE = "040305"; //Custom Office Code
        ddlCustom.DataSource = GLSASer.GetAll(GLSAEnt);
        ddlCustom.DataTextField = "SUB_GL_NAME";
        ddlCustom.DataValueField = "SUB_GL_CODE";
        ddlCustom.DataBind();
    }
    protected void LoadImportGrid()
    {
        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_MASTER_CODE = "0303"; //	Custom Expenses
        grdImportCost.DataSource = GLASer.GetAll(GLAEnt);
        grdImportCost.DataBind();
    }
    protected void CustomExpensesAgent()
    {
        SEnt = new SUPPLIERS();
        SEnt.STATUS = "1";
        SEnt.SUPPLIER_TYPE = "3"; //Agent
        ddlCustomExpensesAgent.DataSource = SSer.GetAll(SEnt);
        ddlCustomExpensesAgent.DataTextField = "SUPPLIER_NAME";
        ddlCustomExpensesAgent.DataValueField = "PK_ID";
        ddlCustomExpensesAgent.DataBind();
    }

    protected void txtAmount_Step2_TextChanged(object sender, EventArgs e)
    {
        double imptotal = 0;
        foreach (GridViewRow gr in grdImportCost.Rows)
        {
            try
            {
                TextBox txtAmount = gr.FindControl("txtAmount") as TextBox;
                imptotal = imptotal + Convert.ToDouble(txtAmount.Text);
            }
            catch { }
        }
        lblTotalImportExpenses.Text = imptotal.ToString("0.00");
    }
    #endregion

    protected void btnBackToCustom_Click(object sender, EventArgs e)
    {
        divStep1.Visible = false;
        divStep2.Visible = true; //Custom
        divStep3.Visible = false;
        divStep4.Visible = false;
        divStep5.Visible = false;
        divStep6.Visible = false;
    }
    protected void btnMoveToForeginFreight_Click(object sender, EventArgs e)
    {
        if (lblTotalImportExpenses.Text == "")
        {
            errmsg = "Add Custome Expenses ";
        }

        if (errmsg == "")
        {
            divStep1.Visible = false;
            divStep2.Visible = false;
            divStep3.Visible = true;
            divStep4.Visible = false;
            divStep5.Visible = false;
            divStep6.Visible = false;
        }
        if (lblForeginFreightTotal.Text == "")// Previous gare ra pheri nest garyo bahne grid ko value khali na garuana lai 
        {
            lblerror.Text = errmsg;
            ForeignFreightAgent();
            LoadForeignFreightGrid();
        }
    }

    #region step 3 : Foregin Freight

    protected void LoadForeignFreightGrid()
    {
        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_MASTER_CODE = "0304";//	Import Expenses (Foregin)
        grdForeignFreight.DataSource = GLASer.GetAll(GLAEnt);
        grdForeignFreight.DataBind();
    }
    protected void ForeignFreightAgent()
    {
        SEnt = new SUPPLIERS();
        SEnt.STATUS = "1";
        SEnt.SUPPLIER_TYPE = "3"; //Agent
        ddlForeignFreightAgent.DataSource = SSer.GetAll(SEnt);
        ddlForeignFreightAgent.DataTextField = "SUPPLIER_NAME";
        ddlForeignFreightAgent.DataValueField = "PK_ID";
        ddlForeignFreightAgent.DataBind();
    }

    protected void txtAmount_Step3_TextChanged(object sender, EventArgs e)
    {
        double imptotal = 0;
        foreach (GridViewRow gr in grdForeignFreight.Rows)
        {
            try
            {
                TextBox txtAmount = gr.FindControl("txtAmount") as TextBox;
                imptotal = imptotal + Convert.ToDouble(txtAmount.Text);
            }
            catch { }
        }
        lblForeginFreightTotal.Text = imptotal.ToString("0.00");

    }
    #endregion

    protected void bntBackToForeginFreight_Click(object sender, EventArgs e)
    {
        divStep1.Visible = false;
        divStep2.Visible = false;
        divStep3.Visible = true;//Foregin freight
        divStep4.Visible = false;
        divStep5.Visible = false;
        divStep6.Visible = false;
    }
    protected void btnMoveToNepalFreight_Click(object sender, EventArgs e)
    {
        if (lblForeginFreightTotal.Text == "")
        {
            errmsg = "Enter Foregin Freight";
        }
        if (errmsg == "")
        {
            divStep1.Visible = false;
            divStep2.Visible = false;
            divStep3.Visible = false;
            divStep4.Visible = true; //Nepal Freight
            divStep5.Visible = false;
            divStep6.Visible = false;
        }

        lblerror.Text = errmsg;
        if (lblNepalFreightTotal.Text == "")
        {
            LoadNepalFreightGrid();
            NepalFreightAgent();
        }
    }

    #region step 4 : Nepal Freight
    protected void NepalFreightAgent()
    {
        SEnt = new SUPPLIERS();
        SEnt.STATUS = "1";
        SEnt.SUPPLIER_TYPE = "3"; //Agent
        ddlNepalFreightAgent.DataSource = SSer.GetAll(SEnt);
        ddlNepalFreightAgent.DataTextField = "SUPPLIER_NAME";
        ddlNepalFreightAgent.DataValueField = "PK_ID";
        ddlNepalFreightAgent.DataBind();
        ddlNepalFreightAgent.Items.Insert(0, "Select");
    }

    protected void ddlNepalFreightAgent_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlNepalFreightAgent.SelectedValue != "Select")
        {
            SEnt = new SUPPLIERS();
            SEnt.PK_ID = ddlNepalFreightAgent.SelectedValue;
            SEnt = (SUPPLIERS)SSer.GetSingle(SEnt);
            if (SEnt != null)
            {
                txtNepalFreightAgentPANVAT.Text = SEnt.VAT_PAN_NUMBER;
                txtNepalFreightAgentAddress.Text = SEnt.ADDRESS;
            }
        }
    }
    protected void LoadNepalFreightGrid()
    {
        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_MASTER_CODE = "0305"; // Import Expenses (Nepal)
        grdNepalFreight.DataSource = GLASer.GetAll(GLAEnt);
        grdNepalFreight.DataBind();
    }

    protected void txtAmount_Step4_TextChanged(object sender, EventArgs e)
    {
        double imptotal = 0;
        foreach (GridViewRow gr in grdNepalFreight.Rows)
        {
            try
            {
                TextBox txtAmount = gr.FindControl("txtAmount") as TextBox;
                imptotal = imptotal + Convert.ToDouble(txtAmount.Text);
            }
            catch { }
        }
        lblNepalFreightSubTotal.Text = imptotal.ToString("0.00");
        lblNepalFreightVAT.Text = (Convert.ToDouble(txtNepalFreightVAT.Text) * imptotal / 100).ToString("0");
        lblNepalFreightTotal.Text = (Convert.ToDouble(lblNepalFreightSubTotal.Text) + Convert.ToDouble(lblNepalFreightVAT.Text)).ToString("0");
    }
    #endregion

    protected void btnPreviousStep4_Click(object sender, EventArgs e)
    {
        divStep1.Visible = false;
        divStep2.Visible = false;
        divStep3.Visible = false;
        divStep4.Visible = true;
        divStep5.Visible = false;
        divStep6.Visible = false;
    }
    protected void bntMoveToOtherExpenses_Click(object sender, EventArgs e)
    {
        if (lblNepalFreightTotal.Text == "")
        {
            errmsg = "Enter Nepal Freight";
        }
        if (errmsg == "")
        {
            divStep1.Visible = false;
            divStep2.Visible = false;
            divStep3.Visible = false;
            divStep4.Visible = false;
            divStep5.Visible = true; //OtherExpenses
            divStep6.Visible = false;
        }

        lblerror.Text = errmsg;
        LaodExpenseHeading();
    }

    #region step 5 : Other Charges
    protected void LaodExpenseHeading()
    {
        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_MASTER_CODE = "0306"; // GL master code of Import Expenses (Other)
        GLAEnt.STATUS = "1";
        ddlExpensesHeading.DataSource = GLASer.GetAll(GLAEnt);
        ddlExpensesHeading.DataTextField = "GL_NAME";
        ddlExpensesHeading.DataValueField = "GL_CODE";
        ddlExpensesHeading.DataBind();
        ddlExpensesHeading.Items.Insert(0, "Select");
    }

    protected void LoadBank()
    {
        GLSAEnt = new GL_SUB_ACCOUNT();
        GLSAEnt.GL_CODE = "010101"; // GL  code of Ba
        GLSAEnt.STATUS = "1";
        ddlBank.DataSource = GLSASer.GetAll(GLSAEnt);
        ddlBank.DataTextField = "SUB_GL_NAME";
        ddlBank.DataValueField = "SUB_GL_CODE";
        ddlBank.DataBind();
    }
    protected void LoadParty()
    {
        EntityList theList1 = new EntityList();
        EntityList theList2 = new EntityList();
        EntityList newList = new EntityList();
        GLSAEnt = new GL_SUB_ACCOUNT();
        GLSAEnt.GL_CODE = "040303"; // GL  code of Agent
        GLSAEnt.STATUS = "1";
        theList1 = GLSASer.GetAll(GLSAEnt);

        GLSAEnt = new GL_SUB_ACCOUNT();
        GLSAEnt.GL_CODE = "040304"; // GL  code of Party
        GLSAEnt.STATUS = "1";
        theList2 = GLSASer.GetAll(GLSAEnt);

        foreach (GL_SUB_ACCOUNT GSA in theList1)
        {
            newList.Add(GSA);
        }
        foreach (GL_SUB_ACCOUNT GSA in theList2)
        {
            newList.Add(GSA);
        }

        ddlParty.DataSource = newList;
        ddlParty.DataTextField = "SUB_GL_NAME";
        ddlParty.DataValueField = "SUB_GL_CODE";
        ddlParty.DataBind();
    }
    protected void btnAddOtherCharges_Click(object sender, EventArgs e)
    {
        if (ddlExpensesHeading.SelectedValue == "030601") // Bank Charge
        {
            divOtherChargeBank.Visible = true;
            divOtherCharge.Visible = false;
            LoadBank();
        }
        else
        {
            divOtherChargeBank.Visible = false;
            divOtherCharge.Visible = true;
            LoadParty();
        }
        divOtherChargeAddition.Visible = true;
    }

    protected DataTable CreateGridExpenses(int RowIndex, bool addRemoveFlag)
    {
        int row = grdOtherExpenses.Rows.Count;
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("ExpensesCode");
        dummyTable.Columns.Add("ExpensesHeading");
        dummyTable.Columns.Add("PartyCode");
        dummyTable.Columns.Add("PartyName");
        dummyTable.Columns.Add("ExpenseAmount");
        dummyTable.Columns.Add("ExpenseRemarks");

        if (row > 0)
        {
            foreach (GridViewRow r in grdOtherExpenses.Rows)
            {
                Label lblExpenseCode = r.FindControl("lblExpenseCode") as Label;
                Label lblExpenseHeading = r.FindControl("lblExpenseHeading") as Label;
                Label lblPartyCode = r.FindControl("lblPartyCode") as Label;
                Label lblPartyName = r.FindControl("lblPartyName") as Label;
                Label lblExpenseAmount = r.FindControl("lblExpenseAmount") as Label;
                Label lblExpenseAmountRemarks = r.FindControl("lblExpenseAmountRemarks") as Label;


                DataRow dummyRw = dummyTable.NewRow();

                if (lblExpenseCode.Text == ddlExpensesHeading.SelectedValue && lblExpenseAmount.Text == txtOtherExpenseAmount.Text) // the selected product is already in the grid update the qty and rate
                {
                    dummyRw["ExpensesCode"] = ddlExpensesHeading.SelectedValue;
                    dummyRw["ExpensesHeading"] = ddlExpensesHeading.SelectedItem.ToString();

                    if (ddlExpensesHeading.SelectedValue == "030601")//Bank Charges
                    {
                        dummyRw["PartyCode"] = ddlBank.SelectedValue;
                        dummyRw["PartyName"] = ddlBank.SelectedItem.ToString();
                    }
                    else
                    {
                        dummyRw["PartyCode"] = ddlParty.SelectedValue;
                        dummyRw["PartyName"] = ddlParty.SelectedItem.ToString();
                    }
                    dummyRw["ExpenseAmount"] = txtOtherExpenseAmount.Text;
                    dummyRw["ExpenseRemarks"] = txtOtherExpenseRemarks.Text;


                    flag = true;
                }

                else // the selected product is not in the grid add the product detail in the grid
                {
                    dummyRw["ExpensesCode"] = lblExpenseCode.Text;
                    dummyRw["ExpensesHeading"] = lblExpenseHeading.Text;
                    dummyRw["PartyCode"] = lblPartyCode.Text;
                    dummyRw["PartyName"] = lblPartyName.Text;
                    dummyRw["ExpenseAmount"] = lblExpenseAmount.Text;
                    dummyRw["ExpenseRemarks"] = lblExpenseAmountRemarks.Text;

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

            dummyRow["ExpensesCode"] = ddlExpensesHeading.SelectedValue;
            dummyRow["ExpensesHeading"] = ddlExpensesHeading.SelectedItem.ToString();

            if (ddlExpensesHeading.SelectedValue == "030601")//Bank Charges
            {
                dummyRow["PartyCode"] = ddlBank.SelectedValue;
                dummyRow["PartyName"] = ddlBank.SelectedItem.ToString();
            }
            else
            {
                dummyRow["PartyCode"] = ddlParty.SelectedValue;
                dummyRow["PartyName"] = ddlParty.SelectedItem.ToString();
            }
            dummyRow["ExpenseAmount"] = txtOtherExpenseAmount.Text;
            dummyRow["ExpenseRemarks"] = txtOtherExpenseRemarks.Text;

            dummyTable.Rows.Add(dummyRow);

        }

        DataView dv = new DataView(dummyTable);

        grdOtherExpenses.DataSource = dummyTable;
        grdOtherExpenses.DataBind();

        getTotal();
        return dummyTable;
    }

    protected void grdOtherExpenses_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            CreateGridExpenses(gr.RowIndex, false);
        }
    }
    protected void btnAddOtherExpenses_Click(object sender, EventArgs e)
    {
        CreateGridExpenses(1, true);
        divOtherChargeAddition.Visible = false;
        txtOtherExpenseAmount.Text = "";
        txtOtherExpenseRemarks.Text = "";
    }

    #endregion

    protected void btnBackToNepalFreight_Click(object sender, EventArgs e)
    {
        divStep1.Visible = false;
        divStep2.Visible = false;
        divStep3.Visible = false;
        divStep4.Visible = true; // nepal freight
        divStep5.Visible = false;
        divStep6.Visible = false;
    }
    protected void btnMoveToCostCalculation_Click(object sender, EventArgs e)
    {
        if (lblNepalFreightTotal.Text == "")
        {
            errmsg = "Enter Nepal Freight";
        }
        if (errmsg == "")
        {
            divStep1.Visible = false;
            divStep2.Visible = false;
            divStep3.Visible = false;
            divStep4.Visible = false;
            divStep5.Visible = false;
            divStep6.Visible = true; // CostCalculation
        }

        lblerror.Text = errmsg;

    }

    #region Step 6  Cost Calculation
    protected void btnBackToOtherExpenses_Click(object sender, EventArgs e)
    {
        divStep1.Visible = false;
        divStep2.Visible = false;
        divStep3.Visible = false;
        divStep4.Visible = false;
        divStep5.Visible = true; // nepal freight
        divStep6.Visible = false;
    }
    #endregion

    protected void btnSaveImport_Click(object sender, EventArgs e)
    {
        if (!IsPageRefresh)
        {
            DistributedTransaction DT = new DistributedTransaction();
            string msg = "Please Enter ";
            try
            {
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
                    userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                    string pk_id = "";
                    //if lblOldPK_ID is not null then it is purchase edit
                    // in purchase edit, update purchase master, delete all purchase child and re insert purchase child
                    string v_number = af.getNext_VM_ID("JV", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfile.LocationID);
                    double v_no = Convert.ToDouble(v_number);
                    #region for supplier
                    if (lblOldPK_ID.Text == "")//new entry
                    {
                        #region to insert in purchase master
                        PIMEnt = new PURCHASE_INVOICE_MASTER();
                        PIMEnt.SUPPLIER_INVOICE_NO = txtInvoiceNumber.Text;
                        string[] invoicedate = txtInvoiceDate.Text.Split('/');
                        PIMEnt.INVOICE_DATE = PGD.ConvertNepaliTOEnglish(invoicedate[0], invoicedate[1], invoicedate[2]);
                        PIMEnt.INVOICE_DAY = invoicedate[0];
                        PIMEnt.INVOICE_MONTH = invoicedate[1];
                        PIMEnt.INVOICE_YEAR = invoicedate[2];
                        PIMEnt.DAKHILA_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                        PIMEnt.DAKHILA_DAY = PGD.NepaliDay();
                        PIMEnt.DAKHILA_MONTH = PGD.NepaliMonth();
                        PIMEnt.DAKHILA_YEAR = PGD.NepaliYear();
                        PIMEnt.DAKHILA_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                        PIMEnt.SUPPLIER_ID = ddlSupplier.SelectedValue;
                        PIMEnt.SUPPLIER_NAME = ddlSupplier.SelectedItem.ToString();
                        PIMEnt.SUPPLIER_PAN_VAT = txtSupplierPANVAT.Text;
                        PIMEnt.SUPPLIER_ADDRESS = txtSupplierAddress.Text;
                        PIMEnt.SUPPLIER_CONTACT = txtContactNo.Text;
                        PIMEnt.USER_ID = userProfile.EmployeeID;
                        PIMEnt.SUB_TOTAL_AMOUNT = lblInvoiceTotal.Text;
                        PIMEnt.DISCOUNT_PERCENT = "0";
                        PIMEnt.DISCOUNT_AMOUNT = "0";
                        PIMEnt.TOTAL_AMOUNT = lblInvoiceTotal.Text;
                        PIMEnt.TAX_VAT_AMOUNT = "0";
                        PIMEnt.GRAND_TOTAL = lblInvoiceTotal.Text; //amount after addition of VAT
                        PIMEnt.ROUND_OFF = "0";
                        PIMEnt.INVOICE_AMOUNT = lblInvoiceTotal.Text; //amount after addition of VAT
                        PIMEnt.PURCHASE_TYPE_ID = ddlPaymentType.SelectedValue;
                        PIMEnt.CURRENCY = ddlCurrency.SelectedValue;
                        PIMEnt.IMPORT_LOCAL_PURCHASE = "I";//Import
                        PIMEnt.PP_NUMBER = txtPPNumber.Text;
                        PIMEnt.IMPORT_AMOUNT = lblKharidKhataAmount.Text;
                        PIMEnt.IMPORT_VAT = lblKharidKhataVAT.Text;
                        PIMEnt.IMPORT_TOTAL = lblKharidKhataTotal.Text;
                        PIMEnt.OFFICE_CODE = userProfile.LocationID;
                        pk_id = PIMSer.Insert(PIMEnt, DT).ToString();
                        #endregion

                        #region to insert in to purchase  detail
                        foreach (GridViewRow gr in grdPurchaseDetail.Rows)
                        {
                            Label lblSno = (Label)gr.FindControl("lblSno");
                            Label lblPK_ID = (Label)gr.FindControl("lblPK_ID");
                            Label lblBatch = (Label)gr.FindControl("lblBatch");
                            Label lblExpDate = (Label)gr.FindControl("lblExpDate");
                            Label lblQty = (Label)gr.FindControl("lblQty");
                            Label lblFCRate = (Label)gr.FindControl("lblFCRate");
                            Label lblRate = (Label)gr.FindControl("lblRate");
                            Label lblExchangeRate = (Label)gr.FindControl("lblExchangeRate");
                            Label lblFC_ItemTotal = (Label)gr.FindControl("lblFC_ItemTotal");
                            Label lblItemTotal = (Label)gr.FindControl("lblItemTotal");
                            Label lblPPItemTotal = (Label)gr.FindControl("lblPPItemTotal");

                            PIDEnt = new PURCHASE_INVOICE_DETAIL();
                            PIDEnt.PURCHASE_INVOICE_ID = pk_id;
                            PIDEnt.SNO = lblSno.Text;
                            PIDEnt.PRODUCT_ID = lblPK_ID.Text;
                            PIDEnt.BATCH_NUMBER = lblBatch.Text;
                            PIDEnt.EXPIRY_DATE = lblExpDate.Text;
                            PIDEnt.QUANTITY = lblQty.Text;
                            PIDEnt.FC_RATE = lblFCRate.Text;
                            PIDEnt.RATE = lblRate.Text;
                            PIDEnt.CONVERSION_RATE = lblExchangeRate.Text;
                            PIDEnt.FC_SUB_TOTAL = lblFC_ItemTotal.Text;
                            PIDEnt.SUB_TOTAL = lblItemTotal.Text;
                            PIDEnt.IMPORT_AMOUNT = lblPPItemTotal.Text;
                            PIDEnt.OFFICE_CODE = userProfile.LocationID;
                            string PID_ID = PIDSer.Insert(PIDEnt, DT).ToString();
                        }
                        #endregion

                        #region account portion                      
                        #region to insert in Voucher Master

                        VMEnt = new VOUCHER_MASTER();
                        VMEnt.VOUCHER_TYPE = "JV";
                        VMEnt.VOUCHER_NUMBER = v_no.ToString();
                        VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                        VMEnt.VOUCHER_DAY = PGD.NepaliDay();
                        VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
                        VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
                        VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                        VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                        VMEnt.TRN_AMOUNT = lblInvoiceTotal.Text;
                        VMEnt.NARRATION = "Import of goods, PP no." + txtPPNumber.Text;// + getInvoiceNumber(invoicemasterid, DT);
                        VMEnt.REF_TABLE = "Purchase Invoice";
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
                        v_no++;
                        #endregion
                        #region to update invoice master with vouchermaster id
                        PIMEnt = new PURCHASE_INVOICE_MASTER();
                        PIMEnt.PK_ID = pk_id;
                        PIMEnt = (PURCHASE_INVOICE_MASTER)PIMSer.GetSingle(PIMEnt, DT);
                        if (PIMEnt != null)
                        {
                            PIMEnt.PB_VM_ID = voucher_pk_id;
                            PIMSer.Update(PIMEnt, DT);
                        }
                        #endregion
                        #region to insert in to voucher child
                        int S_sno = 1;
                        #region for Dr Part
                        #region for Purchase
                        foreach (GridViewRow gvr in grdPurchaseDetail.Rows)
                        {
                            Label lblProductCode = gvr.FindControl("lblProductCode") as Label;
                            Label lblItemTotal = gvr.FindControl("lblItemTotal") as Label;

                            VCEnt = new VOUCHER_CHILD();
                            VCEnt.VOUCHER_ID = voucher_pk_id;
                            VCEnt.SNO = S_sno.ToString();
                            VCEnt.GL_CODE = "010401";  //Purchase Account                      
                            VCEnt.SGL_CODE = lblProductCode.Text;
                            VCEnt.DR_AMOUNT = lblItemTotal.Text;
                            VCEnt.CR_AMOUNT = "0";
                            VCEnt.REMARKS = "By";
                            VCEnt.OFFICE_CODE = userProfile.LocationID;
                            VCSer.Insert(VCEnt, DT);
                            S_sno++;
                        }
                        #endregion

                        #endregion
                        #region for Cr Part
                        #region for suppliers
                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = voucher_pk_id;
                        VCEnt.SNO = S_sno.ToString();
                        VCEnt.GL_CODE = "040302"; //GL_CODE of Sundry Debitors Foreign Supplier
                        VCEnt.SGL_CODE = txtSupplierCode.Text;
                        VCEnt.DR_AMOUNT = "0";
                        VCEnt.CR_AMOUNT = lblInvoiceTotal.Text;
                        VCEnt.REMARKS = "To";
                        VCEnt.OFFICE_CODE = userProfile.LocationID; 
                        VCSer.Insert(VCEnt, DT);
                        S_sno++;
                        #endregion
                        #endregion
                        #endregion

                        #endregion
                    }
                    #endregion

                    #region for custom
                    #region account portion Custom

                    #region to insert in Voucher Master

                    VMEnt = new VOUCHER_MASTER();
                    VMEnt.VOUCHER_TYPE = "JV";
                    VMEnt.VOUCHER_NUMBER = v_no.ToString();
                    VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.VOUCHER_DAY = PGD.NepaliDay();
                    VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
                    VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
                    VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                    VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.TRN_AMOUNT = lblTotalImportExpenses.Text;
                    VMEnt.NARRATION = "Custom Expenses on Import of goods, PP no." + txtPPNumber.Text;// + getInvoiceNumber(invoicemasterid, DT);
                    VMEnt.REF_TABLE = "";
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
                    VMEnt.OFFICE_CODE = userProfile.LocationID; ;
                    string custom_voucher_pk_id = VMSer.Insert(VMEnt, DT).ToString();
                    v_no++;
                    #endregion

                    #region to insert in to voucher child
                    int custom_sno = 1;
                    #region for Dr Part
                    #region for Custom Expenses
                    foreach (GridViewRow gvr in grdImportCost.Rows)
                    {
                        Label lblGL_CODE = gvr.FindControl("lblGL_CODE") as Label;
                        TextBox txtAmount = gvr.FindControl("txtAmount") as TextBox;

                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = custom_voucher_pk_id;
                        VCEnt.SNO = custom_sno.ToString();
                        VCEnt.GL_CODE = lblGL_CODE.Text;
                        VCEnt.SGL_CODE = "";
                        VCEnt.DR_AMOUNT = txtAmount.Text;
                        VCEnt.CR_AMOUNT = "0";
                        VCEnt.REMARKS = "By";
                        VCEnt.OFFICE_CODE= userProfile.LocationID; ;
                        if (txtAmount.Text != "0")
                        {
                            VCSer.Insert(VCEnt, DT);
                            custom_sno++;
                        }
                    }
                    #endregion                  
                    #endregion
                    #region for Cr Part
                    #region for Custom
                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = custom_voucher_pk_id;
                    VCEnt.SNO = custom_sno.ToString();
                    VCEnt.GL_CODE = "040304"; //GL_CODE of Custom Office
                    VCEnt.SGL_CODE = ddlCustom.SelectedValue;
                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = lblTotalImportExpenses.Text;
                    VCEnt.REMARKS = "To";
                    VCEnt.OFFICE_CODE = userProfile.LocationID; 
                    VCSer.Insert(VCEnt, DT);

                    #endregion
                    #endregion
                    #endregion
                    #endregion

                    #region account portion Agent

                    #region to insert in Voucher Master

                    VMEnt = new VOUCHER_MASTER();
                    VMEnt.VOUCHER_TYPE = "JV";
                    VMEnt.VOUCHER_NUMBER = v_no.ToString();
                    VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.VOUCHER_DAY = PGD.NepaliDay();
                    VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
                    VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
                    VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                    VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.TRN_AMOUNT = lblTotalImportExpenses.Text;
                    VMEnt.NARRATION = "Expenses on Import of goods, PP no." + txtPPNumber.Text + " Custom."; // + getInvoiceNumber(invoicemasterid, DT);
                    VMEnt.REF_TABLE = "";
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
                    string custom_agent_voucher_pk_id = VMSer.Insert(VMEnt, DT).ToString();
                    v_no++;
                    #endregion

                    #region to insert in to voucher child

                    #region for Dr Part
                    #region for Custom Expenses
                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = custom_agent_voucher_pk_id;
                    VCEnt.SNO = custom_sno.ToString();
                    VCEnt.GL_CODE = "040304"; //GL_CODE of Custom Office
                    VCEnt.SGL_CODE = ddlCustom.SelectedValue;
                    VCEnt.DR_AMOUNT = lblTotalImportExpenses.Text;
                    VCEnt.CR_AMOUNT = "0";
                    VCEnt.REMARKS = "By";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCSer.Insert(VCEnt, DT);
                    #endregion                  
                    #endregion
                    #region for Cr Part
                    #region for Custom
                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = custom_agent_voucher_pk_id;
                    VCEnt.SNO = custom_sno.ToString();
                    VCEnt.GL_CODE = "040303"; //GL_CODE of Agent
                    VCEnt.SGL_CODE = getSGL_Code(ddlCustomExpensesAgent.SelectedValue);
                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = lblTotalImportExpenses.Text;
                    VCEnt.REMARKS = "To";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCSer.Insert(VCEnt, DT);

                    #endregion
                    #endregion
                    #endregion
                    #endregion
                    #region to insert in to IMPORT_INVOICE_DETAIL
                    foreach (GridViewRow gvr in grdImportCost.Rows)
                    {
                        Label lblGL_CODE = gvr.FindControl("lblGL_CODE") as Label;
                        TextBox txtAmount = gvr.FindControl("txtAmount") as TextBox;
                        CheckBox chkCostEffect = gvr.FindControl("chkCostEffect") as CheckBox;

                        IIDEnt = new IMPORT_INVOICE_DETAIL();
                        IIDEnt.PP_NUMBER = txtPPNumber.Text;
                        IIDEnt.ACC_HEADING = lblGL_CODE.Text;
                        IIDEnt.AMOUNT = txtAmount.Text;
                        if (chkCostEffect.Checked == true)
                            IIDEnt.COST_EFFECT = "1";
                        else
                            IIDEnt.COST_EFFECT = "0";
                        IIDEnt.VM_ID = custom_voucher_pk_id;
                        IIDEnt.FISCAL_YEAR = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                        IIDEnt.OFFICE_CODE = userProfile.LocationID;
                        if (txtAmount.Text != "0")
                            IIDSer.Insert(IIDEnt, DT);
                    }
                    #endregion
                    #endregion

                    #region for foreign frieght
                    #region account portion

                    #region to insert in Voucher Master

                    VMEnt = new VOUCHER_MASTER();
                    VMEnt.VOUCHER_TYPE = "JV";
                    VMEnt.VOUCHER_NUMBER = v_no.ToString();
                    VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.VOUCHER_DAY = PGD.NepaliDay();
                    VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
                    VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
                    VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                    VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.TRN_AMOUNT = lblForeginFreightTotal.Text;
                    VMEnt.NARRATION = "Expenses on Import of goods, PP no." + txtPPNumber.Text + " Foreign Frieght.";// + getInvoiceNumber(invoicemasterid, DT);
                    VMEnt.REF_TABLE = "";
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
                    string foreign_voucher_pk_id = VMSer.Insert(VMEnt, DT).ToString();
                    v_no++;
                    #endregion

                    #region to insert in to voucher child
                    int foreign_sno = 1;
                    #region for Dr Part
                    #region for Foreign Expenses
                    foreach (GridViewRow gvr in grdForeignFreight.Rows)
                    {
                        Label lblGL_CODE = gvr.FindControl("lblGL_CODE") as Label;
                        TextBox txtAmount = gvr.FindControl("txtAmount") as TextBox;

                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = foreign_voucher_pk_id;
                        VCEnt.SNO = foreign_sno.ToString();
                        VCEnt.GL_CODE = lblGL_CODE.Text;
                        VCEnt.SGL_CODE = "";
                        VCEnt.DR_AMOUNT = txtAmount.Text;
                        VCEnt.CR_AMOUNT = "0";
                        VCEnt.REMARKS = "By";
                        VCEnt.OFFICE_CODE = userProfile.LocationID;
                        if (txtAmount.Text != "0")
                        {
                            VCSer.Insert(VCEnt, DT);
                            foreign_sno++;
                        }
                    }
                    #endregion                  
                    #endregion
                    #region for Cr Part
                    #region for Custom
                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = foreign_voucher_pk_id;
                    VCEnt.SNO = foreign_sno.ToString();
                    VCEnt.GL_CODE = "040303"; //GL_CODE of Agent
                    VCEnt.SGL_CODE = getSGL_Code(ddlForeignFreightAgent.SelectedValue);
                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = lblForeginFreightTotal.Text;
                    VCEnt.REMARKS = "To";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCSer.Insert(VCEnt, DT);
                    #endregion
                    #endregion
                    #endregion
                    #endregion

                    #region to insert in to IMPORT_INVOICE_DETAIL
                    foreach (GridViewRow gvr in grdForeignFreight.Rows)
                    {
                        Label lblGL_CODE = gvr.FindControl("lblGL_CODE") as Label;
                        TextBox txtAmount = gvr.FindControl("txtAmount") as TextBox;
                        CheckBox chkCostEffect = gvr.FindControl("chkCostEffect") as CheckBox;

                        IIDEnt = new IMPORT_INVOICE_DETAIL();
                        IIDEnt.PP_NUMBER = txtPPNumber.Text;
                        IIDEnt.ACC_HEADING = lblGL_CODE.Text;
                        IIDEnt.AMOUNT = txtAmount.Text;
                        if (chkCostEffect.Checked == true)
                            IIDEnt.COST_EFFECT = "1";
                        else
                            IIDEnt.COST_EFFECT = "0";
                        IIDEnt.VM_ID = foreign_voucher_pk_id;
                        IIDEnt.FISCAL_YEAR = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                        IIDEnt.OFFICE_CODE = userProfile.LocationID;
                        if (txtAmount.Text != "0")
                            IIDSer.Insert(IIDEnt, DT);
                    }
                    #endregion
                    #endregion

                    #region for Nepal Frieght
                    #region to insert in purchase master
                    PIMEnt = new PURCHASE_INVOICE_MASTER();
                    PIMEnt.SUPPLIER_INVOICE_NO = txtInvoiceNumber.Text;
                    string[] NF_invoicedate = txtNepalFreightInvoiceDate.Text.Split('/');
                    PIMEnt.INVOICE_DATE = PGD.ConvertNepaliTOEnglish(NF_invoicedate[0], NF_invoicedate[1], NF_invoicedate[2]);
                    PIMEnt.INVOICE_DAY = NF_invoicedate[0];
                    PIMEnt.INVOICE_MONTH = NF_invoicedate[1];
                    PIMEnt.INVOICE_YEAR = NF_invoicedate[2];
                    PIMEnt.DAKHILA_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    PIMEnt.DAKHILA_DAY = PGD.NepaliDay();
                    PIMEnt.DAKHILA_MONTH = PGD.NepaliMonth();
                    PIMEnt.DAKHILA_YEAR = PGD.NepaliYear();
                    PIMEnt.DAKHILA_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                    PIMEnt.SUPPLIER_ID = ddlNepalFreightAgent.SelectedValue;
                    PIMEnt.SUPPLIER_NAME = ddlNepalFreightAgent.SelectedItem.ToString();
                    PIMEnt.SUPPLIER_PAN_VAT = txtNepalFreightAgentPANVAT.Text;
                    PIMEnt.SUPPLIER_ADDRESS = txtNepalFreightAgentAddress.Text;
                    PIMEnt.SUPPLIER_CONTACT = "";
                    PIMEnt.USER_ID = userProfile.EmployeeID;
                    PIMEnt.SUB_TOTAL_AMOUNT = lblNepalFreightSubTotal.Text;
                    PIMEnt.DISCOUNT_PERCENT = "0";
                    PIMEnt.DISCOUNT_AMOUNT = "0";
                    PIMEnt.TOTAL_AMOUNT = lblNepalFreightSubTotal.Text;
                    PIMEnt.TAX_VAT_AMOUNT = lblNepalFreightVAT.Text;
                    PIMEnt.GRAND_TOTAL = lblNepalFreightTotal.Text; //amount after addition of VAT
                    PIMEnt.ROUND_OFF = "0";
                    PIMEnt.INVOICE_AMOUNT = lblInvoiceTotal.Text; //amount after addition of VAT
                    PIMEnt.PURCHASE_TYPE_ID = ddlNepalFreight.SelectedValue;
                    PIMEnt.IMPORT_LOCAL_PURCHASE = "LP";//Local
                    PIMEnt.OFFICE_CODE = userProfile.LocationID;
                    string N_pk_id = PIMSer.Insert(PIMEnt, DT).ToString();
                    #endregion

                    #region to insert in to purchase  detail
                    foreach (GridViewRow gr in grdNepalFreight.Rows)
                    {
                        Label lblSno = (Label)gr.FindControl("lblSno");
                        Label lblGL_CODE = (Label)gr.FindControl("lblGL_CODE");
                        TextBox txtAmount = (TextBox)gr.FindControl("txtAmount");

                        PIDEnt = new PURCHASE_INVOICE_DETAIL();
                        PIDEnt.PURCHASE_INVOICE_ID = N_pk_id;
                        PIDEnt.SNO = lblSno.Text;
                        PIDEnt.PRODUCT_ID = lblGL_CODE.Text;
                        PIDEnt.QUANTITY = "1";
                        PIDEnt.RATE = txtAmount.Text;
                        PIDEnt.SUB_TOTAL = txtAmount.Text;
                        PIDEnt.OFFICE_CODE = userProfile.LocationID; ;
                        string PID_ID = PIDSer.Insert(PIDEnt, DT).ToString();
                    }
                    #endregion

                    #region account portion                   
                    #region to insert in Voucher Master

                    VMEnt = new VOUCHER_MASTER();
                    VMEnt.VOUCHER_TYPE = "JV";
                    VMEnt.VOUCHER_NUMBER = v_no.ToString();
                    VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.VOUCHER_DAY = PGD.NepaliDay();
                    VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
                    VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
                    VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                    VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.TRN_AMOUNT = lblNepalFreightTotal.Text;
                    VMEnt.NARRATION = "Expenses on Import of goods,  PP no." + txtPPNumber.Text + " Nepal Frieght";// + getInvoiceNumber(invoicemasterid, DT);
                    VMEnt.REF_TABLE = "Purchase Invoice";
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
                    string Nepal_voucher_pk_id = VMSer.Insert(VMEnt, DT).ToString();
                    v_no++;
                    #endregion
                    #region to update invoice master with vouchermaster id
                    PIMEnt = new PURCHASE_INVOICE_MASTER();
                    PIMEnt.PK_ID = N_pk_id;
                    PIMEnt = (PURCHASE_INVOICE_MASTER)PIMSer.GetSingle(PIMEnt, DT);
                    if (PIMEnt != null)
                    {
                        PIMEnt.PB_VM_ID = Nepal_voucher_pk_id;
                        PIMSer.Update(PIMEnt, DT);
                    }
                    #endregion
                    #region to insert in to voucher child
                    int N_sno = 1;
                    #region for Dr Part
                    #region for Purchase
                    foreach (GridViewRow gvr in grdNepalFreight.Rows)
                    {
                        Label lblGL_CODE = gvr.FindControl("lblGL_CODE") as Label;
                        TextBox txtAmount = gvr.FindControl("txtAmount") as TextBox;

                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = Nepal_voucher_pk_id;
                        VCEnt.SNO = N_sno.ToString();
                        VCEnt.GL_CODE = lblGL_CODE.Text;
                        VCEnt.SGL_CODE = "";
                        VCEnt.DR_AMOUNT = txtAmount.Text;
                        VCEnt.CR_AMOUNT = "0";
                        VCEnt.REMARKS = "By";
                        VCEnt.OFFICE_CODE = userProfile.LocationID;
                        if (txtAmount.Text != "0")
                        {
                            VCSer.Insert(VCEnt, DT);
                            N_sno++;
                        }
                    }
                    #endregion
                    #region for VAT Part

                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = Nepal_voucher_pk_id;
                    VCEnt.SNO = N_sno.ToString();
                    VCEnt.GL_CODE = "040108";  //VAT Payable Account                      
                    VCEnt.SGL_CODE = "";
                    VCEnt.DR_AMOUNT = lblNepalFreightVAT.Text;
                    VCEnt.CR_AMOUNT = "0";
                    VCEnt.REMARKS = "By";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCSer.Insert(VCEnt, DT);
                    N_sno++;
                    #endregion

                    #endregion
                    #region for Cr Part
                    #region for suppliers
                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = Nepal_voucher_pk_id;
                    VCEnt.SNO = N_sno.ToString();
                    GLSAEnt = new GL_SUB_ACCOUNT();
                    GLSAEnt.SUB_GL_CODE = getSGL_Code(ddlNepalFreightAgent.SelectedValue);
                    GLSAEnt = (GL_SUB_ACCOUNT)GLSASer.GetSingle(GLSAEnt);
                    if (GLSAEnt != null)
                        VCEnt.GL_CODE = GLSAEnt.GL_CODE; //GL_CODE of Sundry Creditors                   
                    VCEnt.SGL_CODE = getSGL_Code(ddlNepalFreightAgent.SelectedValue);
                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = lblNepalFreightTotal.Text;
                    VCEnt.REMARKS = "To";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCSer.Insert(VCEnt, DT);
                    N_sno++;
                    #endregion
                    #endregion
                    #endregion
                    #endregion
                    #region to insert in to IMPORT_INVOICE_DETAIL
                    foreach (GridViewRow gvr in grdNepalFreight.Rows)
                    {
                        Label lblGL_CODE = gvr.FindControl("lblGL_CODE") as Label;
                        TextBox txtAmount = gvr.FindControl("txtAmount") as TextBox;
                        CheckBox chkCostEffect = gvr.FindControl("chkCostEffect") as CheckBox;

                        IIDEnt = new IMPORT_INVOICE_DETAIL();
                        IIDEnt.PP_NUMBER = txtPPNumber.Text;
                        IIDEnt.ACC_HEADING = lblGL_CODE.Text;
                        IIDEnt.AMOUNT = txtAmount.Text;
                        if (chkCostEffect.Checked == true)
                            IIDEnt.COST_EFFECT = "1";
                        else
                            IIDEnt.COST_EFFECT = "0";
                        IIDEnt.VM_ID = N_pk_id;
                        IIDEnt.FISCAL_YEAR = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                        IIDEnt.OFFICE_CODE = userProfile.LocationID; 
                        if (txtAmount.Text != "0")
                            IIDSer.Insert(IIDEnt, DT);
                    }
                    #endregion
                    #endregion

                    #region for Other Expenses
                    string Other_voucher_pk_id = "";

                    foreach (GridViewRow gvr in grdOtherExpenses.Rows)
                    {
                        Label lblExpenseCode = gvr.FindControl("lblExpenseCode") as Label;
                        Label lblPartyCode = gvr.FindControl("lblPartyCode") as Label;
                        Label lblExpenseAmount = gvr.FindControl("lblExpenseAmount") as Label;
                        Label lblExpenseAmountRemarks = gvr.FindControl("lblExpenseAmountRemarks") as Label;
                        CheckBox chkCostEffect = gvr.FindControl("chkCostEffect") as CheckBox;


                        #region account portion                   
                        #region to insert in Voucher Master
                        VMEnt = new VOUCHER_MASTER();
                        VMEnt.VOUCHER_TYPE = "JV";
                        VMEnt.VOUCHER_NUMBER = v_no.ToString();
                        VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                        VMEnt.VOUCHER_DAY = PGD.NepaliDay();
                        VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
                        VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
                        VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                        VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                        VMEnt.TRN_AMOUNT = lblExpenseAmount.Text;
                        VMEnt.NARRATION = "Expenses on Import of goods, PP no." + txtPPNumber.Text + " " + lblExpenseAmountRemarks.Text;// + getInvoiceNumber(invoicemasterid, DT);
                        VMEnt.REF_TABLE = "Purchase Invoice";
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
                        Other_voucher_pk_id = VMSer.Insert(VMEnt, DT).ToString();
                        v_no++;
                        #endregion
                        #region to insert in to voucher child
                        #region for Dr Part
                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = Other_voucher_pk_id;
                        VCEnt.SNO = "1";
                        VCEnt.GL_CODE = lblExpenseCode.Text;
                        VCEnt.SGL_CODE = "";
                        VCEnt.DR_AMOUNT = lblExpenseAmount.Text;
                        VCEnt.CR_AMOUNT = "0";
                        VCEnt.REMARKS = "By";
                        VCEnt.OFFICE_CODE = userProfile.LocationID;
                        if (txtAmount.Text != "0")
                            VCSer.Insert(VCEnt, DT);
                        N_sno++;
                        #endregion
                        #region for Cr Part                    
                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = Other_voucher_pk_id;
                        VCEnt.SNO = "2";

                        GLSAEnt = new GL_SUB_ACCOUNT();
                        GLSAEnt.SUB_GL_CODE = lblPartyCode.Text;
                        GLSAEnt = (GL_SUB_ACCOUNT)GLSASer.GetSingle(GLSAEnt);
                        if (GLSAEnt != null)
                            VCEnt.GL_CODE = GLSAEnt.GL_CODE; //GL_CODE of Sundry Creditors 
                        VCEnt.SGL_CODE = lblPartyCode.Text;
                        VCEnt.DR_AMOUNT = "0";
                        VCEnt.CR_AMOUNT = lblExpenseAmount.Text;
                        VCEnt.REMARKS = "To";
                        VCEnt.OFFICE_CODE = userProfile.LocationID;
                        VCSer.Insert(VCEnt, DT);
                        #endregion
                        #endregion
                        #endregion

                        #region to insert in to IMPORT_INVOICE_DETAIL
                        IIDEnt = new IMPORT_INVOICE_DETAIL();
                        IIDEnt.PP_NUMBER = txtPPNumber.Text;
                        IIDEnt.ACC_HEADING = lblExpenseCode.Text;
                        IIDEnt.AMOUNT = lblExpenseAmount.Text;
                        if (chkCostEffect.Checked == true)
                            IIDEnt.COST_EFFECT = "1";
                        else
                            IIDEnt.COST_EFFECT = "0";
                        IIDEnt.VM_ID = Other_voucher_pk_id;
                        IIDEnt.FISCAL_YEAR = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                        IIDEnt.OFFICE_CODE = userProfile.LocationID; 
                        if (txtAmount.Text != "0")
                            IIDSer.Insert(IIDEnt, DT);
                        #endregion
                    }
                    #endregion

                    if (DT.HAPPY == true)
                    {
                        DT.Commit();
                        PIMEnt = new PURCHASE_INVOICE_MASTER();
                        PIMEnt.PK_ID = pk_id;
                        PIMEnt = (PURCHASE_INVOICE_MASTER)PIMSer.GetSingle(PIMEnt);
                        if (PIMEnt != null)
                        {
                            HelperFunction.MsgBox(this, this.GetType(), "Your Dakhila Number is " + PIMEnt.DAKHILA_NUMBER);
                        }
                        UploadPragyaPanFile(pk_id);
                        UploadForeignFrieght(pk_id);
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
    protected void UploadPragyaPanFile(string pk_id)
    {
        if (ppFile.HasFile)
        {
            string lblMsg;

            try
            {
                // Specify the folder path where the image will be saved
                string folderPath = Server.MapPath("~/images/Import_Attachments/");

                // Ensure the folder exists; create it if it does not exist
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }

                // Validate the uploaded file type (allow only image files)
                string fileExtension = System.IO.Path.GetExtension(ppFile.FileName).ToLower();
                if (fileExtension != ".jpg" || fileExtension != ".jpeg" || fileExtension != ".png" || fileExtension != ".pdf")
                {
                    lblMsg = "Only Images are allowed.";
                }

                // Generate a new file name (e.g., based on some unique identifier)
                string fileName = "PragyaPanPatra" + pk_id + fileExtension;
                string filePath = System.IO.Path.Combine(folderPath, fileName);
                if (File.Exists(filePath))
                {
                    // Delete the existing file
                    File.Delete(filePath);
                }
                // Combine the folder path with the new file name


                // Save the uploaded file to the specified path
                ppFile.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                // Handle and display errors
                lblMsg = "An error occurred while uploading the file: " + ex.Message;
            }
        }
    }
    protected void UploadForeignFrieght(string pk_id)
    {
        if (ffFile.HasFile)
        {
            string lblMsg;

            try
            {
                // Specify the folder path where the image will be saved
                string folderPath = Server.MapPath("~/images/Import_Attachments/");

                // Ensure the folder exists; create it if it does not exist
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }

                // Validate the uploaded file type (allow only image files)
                string fileExtension = System.IO.Path.GetExtension(ffFile.FileName).ToLower();
                if (fileExtension != ".jpg" || fileExtension != ".jpeg" || fileExtension != ".png" || fileExtension != ".pdf")
                {
                    lblMsg = "Only Images are allowed.";
                }

                // Generate a new file name (e.g., based on some unique identifier)
                string fileName = "Foreign_Frieght" + pk_id + fileExtension;
                string filePath = System.IO.Path.Combine(folderPath, fileName);
                if (File.Exists(filePath))
                {
                    // Delete the existing file
                    File.Delete(filePath);
                }
                // Combine the folder path with the new file name


                // Save the uploaded file to the specified path
                ffFile.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                // Handle and display errors
                lblMsg = "An error occurred while uploading the file: " + ex.Message;
            }
        }
    }

    protected string getSGL_Code(string supplier_id)
    {
        string sgl_code = "";
        SEnt = new SUPPLIERS();
        SEnt.PK_ID = supplier_id;
        SEnt = (SUPPLIERS)SSer.GetSingle(SEnt);
        if (SEnt != null)
        {
            sgl_code = SEnt.SUPPLIER_CODE;
        }
        return sgl_code;
    }
}