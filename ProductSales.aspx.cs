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
using System.Net;
using System.Net.Security;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity.Framework;

public partial class ProductSales : System.Web.UI.Page
{
    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    OPENING_BALANCE OBEnt = new OPENING_BALANCE();
    OPENING_BALANCEService OBSer = new OPENING_BALANCEService();

    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();

    SALES_INVOICE_MASTER SIMEnt = new SALES_INVOICE_MASTER();
    SALES_INVOICE_MASTERService SIMSer = new SALES_INVOICE_MASTERService();

    SALES_INVOICE_DETAIL SIDEnt = new SALES_INVOICE_DETAIL();
    SALES_INVOICE_DETAILService SIDSer = new SALES_INVOICE_DETAILService();

    AGENT AEnt = new AGENT();
    AGENTService ASer = new AGENTService();

    AREA AREAEnt = new AREA();
    AREAService AREASer = new AREAService();

    PAYMENT_TYPE PTEnt = new PAYMENT_TYPE();
    PAYMENT_TYPEService PTSEr = new PAYMENT_TYPEService();

    BANK_ACCOUNT BAEnt = new BANK_ACCOUNT();
    BANK_ACCOUNTService BASer = new BANK_ACCOUNTService();

    VOUCHER_MASTER VMEnt = new VOUCHER_MASTER();
    VOUCHER_MASTERService VMSer = new VOUCHER_MASTERService();

    VOUCHER_CHILD VCEnt = new VOUCHER_CHILD();
    VOUCHER_CHILDService VCSer = new VOUCHER_CHILDService();

    GL_SUB_ACCOUNT GLSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GLSASer = new GL_SUB_ACCOUNTService();

    PRODUCT_RATE_TYPE CTEnt = new PRODUCT_RATE_TYPE();
    PRODUCT_RATE_TYPEService CTSer = new PRODUCT_RATE_TYPEService();

    PURCHASE_INVOICE_DETAIL PIDEnt = new PURCHASE_INVOICE_DETAIL();
    PURCHASE_INVOICE_DETAILService PIDSer = new PURCHASE_INVOICE_DETAILService();

    PRODUCT_RATES PREnt = new PRODUCT_RATES();
    PRODUCT_RATEService PRSer = new PRODUCT_RATEService();

    PRODUCT_TYPE PrTEnt = new PRODUCT_TYPE();
    PRODUCT_TYPEService PrTSer = new PRODUCT_TYPEService();

    NAME_COMPANY NEnt = new NAME_COMPANY();
    NAME_COMPANYService NSer = new NAME_COMPANYService();

    UserProfileEntity userProfile = new UserProfileEntity();
    HelperFunction hf = new HelperFunction();
    Boolean flag = false;
    Boolean IsPageRefresh = false;
    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGan PG = new PhyeGan();
    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();
    AccountFunction af = new AccountFunction();

    ORDER_NUMBER ONEnt = new ORDER_NUMBER();
    ORDER_NUMBERService ONSer = new ORDER_NUMBERService();

    string currentOrderNumber = ""; // holds the generated order no for this save, used when printing
    bool OfficeCopy;
    string path = "";
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
                    LoadAgent();
                    LoadArea();
                    //LoadPaymentType();
                    LoadBank();
                    SetVisibilty();
                    loadCustomerType();

                    txtQty.Text = "1";
                    txtVATPercent.Text = PG.CompanyTAXPercent();
                    txtTransactionDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
                    txtInvoiceDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
                    lblunique_token.Text = userProfile.UserName + hf.getmaxinvid();
                    loadCustomerType();// this code is for not allowing double entry of same bill
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
            catch (Exception ee)
            {
                Response.Redirect("~/Login.aspx");
            }
        }
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
        //ddlCustomerType.Items.Insert(0, "Select");

    }
    //protected void LoadPaymentType()
    //{
    //    PTEnt = new PAYMENT_TYPE();
    //    PTEnt.SALES_PURCHASE = "S";
    //    PTEnt.STATUS = "1";
    //    ddlPaymentType.DataSource = PTSEr.GetAll(PTEnt);
    //    ddlPaymentType.DataTextField = "PAYMENT_NAME";
    //    ddlPaymentType.DataValueField = "PAYMENT_CODE";
    //    ddlPaymentType.DataBind();
    //}
    protected void LoadBank()
    {
        BAEnt = new BANK_ACCOUNT();
        BAEnt.SHOW_IN_RECEIPT = "1";
        ddlBank.DataSource = BASer.GetAll(BAEnt);
        ddlBank.DataTextField = "BANK_NAME";
        ddlBank.DataValueField = "BANK_CODE";
        ddlBank.DataBind();
    }
    protected void chkWalkIn_CheckedChanged(object sender, EventArgs e)
    {
        //if (chkWalkIn.Checked == true)
        //{
        //    divExistingCustomer.Visible = false;
        //    divWalkInCustomer.Visible = true;
        //    divCredit.Visible = false;
        //    ddlPaymentType.Enabled = true;
        //    ddlPaymentType.SelectedValue = "CS";
        //    divBankDetail.Visible = false;
        //    ddlAgentName.SelectedValue = "Select";
        //    ddlArea.SelectedValue = "Select";
        //    txtCustomerAddress.Text = "";
        //    txtCustomerContact.Text = "";
        //    txtCustomerPANVAT.Text = "0";
        //    txtCustomerCode.Text = "";
        //}
        //else
        //{
            ddlCustomer.SelectedValue = "Select";
            divExistingCustomer.Visible = true;
            divWalkInCustomer.Visible = false;
            divCredit.Visible = true;
            ddlPaymentType.Enabled = true;
        //}
    }
    protected void ddlPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPaymentType.SelectedValue == "CR")
        {
            //if (chkWalkIn.Checked == false)
            //{
                divBankDetail.Visible = false;
                divCredit.Visible = true;
                CustomerBalance(txtCustomerCode.Text);
            //}
            //else
            //{
                HelperFunction.MsgBox(this, this.GetType(), "Credit Sales cannot be made for Walkin Customer. Please create customer for Credit Sales.");
                //LoadPaymentType();
            //}
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
        btnShowQR.Visible = (ddlPaymentType.SelectedValue == "QR");
        getTotal(); // recalculate so VAT Return row reflects the new sales type
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
        PEnt.OFFICE_CODE = userProfile.LocationID;
        EntityList theList = new EntityList();
        theList = PSer.GetAll(PEnt);
        ddlProduct.DataSource = theList;
        ddlProduct.DataTextField = "PRODUCT_FULLNAME";
        ddlProduct.DataValueField = "PK_ID";
        ddlProduct.DataBind();
        ddlProduct.Items.Insert(0, "Select");
    }
    protected void LoadAgent()
    {
        AEnt = new AGENT();
        AEnt.STATUS = "1";
        AEnt.AGENT_TYPE = "S";
        ddlAgentName.DataSource = ASer.GetAll(AEnt);
        ddlAgentName.DataTextField = "AGENT_NAME";
        ddlAgentName.DataValueField = "AGENT_CODE";
        ddlAgentName.DataBind();
        ddlAgentName.Items.Insert(0, "Select");
    }
    protected void LoadArea()
    {
        AREAEnt = new AREA();
        ddlArea.DataSource = AREASer.GetAll(AREAEnt);
        ddlArea.DataTextField = "AREA_NAME";
        ddlArea.DataValueField = "AREA_CODE";
        ddlArea.DataBind();
        ddlArea.Items.Insert(0, "Select");
    }
    protected DataTable CreateGridFirst()
    {
        int row = grdSalesDetail.Rows.Count;
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("TAXABLE");
        dummyTable.Columns.Add("PRODUCT_CODE");
        dummyTable.Columns.Add("PRODUCT");
        dummyTable.Columns.Add("BATCH_NO");
        dummyTable.Columns.Add("EXPIRY_DATE");
        dummyTable.Columns.Add("U_QUANTITY");
        dummyTable.Columns.Add("U_UNIT");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("TOTAL");
        dummyTable.Columns.Add("SCHE_DISC");
        dummyTable.Columns.Add("AFTER_SCHE_DISC");


        DataRow dummyRw = dummyTable.NewRow();
        dummyRw["PK_ID"] = "";
        dummyRw["TAXABLE"] = "";
        dummyRw["PRODUCT_CODE"] = "";
        dummyRw["PRODUCT"] = "";
        dummyRw["BATCH_NO"] = "";
        dummyRw["EXPIRY_DATE"] = "";
        dummyRw["U_QUANTITY"] = "";
        dummyRw["U_UNIT"] = "";
        dummyRw["QUANTITY"] = "";
        dummyRw["UNIT"] = "";
        dummyRw["RATE"] = "";
        dummyRw["TOTAL"] = "";
        dummyRw["SCHE_DISC"] = "";
        dummyRw["AFTER_SCHE_DISC"] = "";

        DataView dv = new DataView(dummyTable);
        grdSalesDetail.DataSource = dv;
        grdSalesDetail.DataBind();
        return dummyTable;
    }
    protected DataTable CreateGrid(int RowIndex, bool addRemoveFlag)
    {
        int row = grdSalesDetail.Rows.Count;
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("TAXABLE");
        dummyTable.Columns.Add("PRODUCT_CODE");
        dummyTable.Columns.Add("PRODUCT");
        dummyTable.Columns.Add("BATCH_NO");
        dummyTable.Columns.Add("EXPIRY_DATE");
        dummyTable.Columns.Add("U_QUANTITY");
        dummyTable.Columns.Add("U_UNIT");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("TOTAL");
        dummyTable.Columns.Add("SCHE_DISC");
        dummyTable.Columns.Add("AFTER_SCHE_DISC");

        if (row > 0)
        {
            foreach (GridViewRow r in grdSalesDetail.Rows)
            {
                Label lblProductPK_ID = r.FindControl("lblProductPK_ID") as Label;
                Label lblTaxable = r.FindControl("lblTaxable") as Label;
                Label lblProductCode = r.FindControl("lblProductCode") as Label;
                Label lblProductName = r.FindControl("lblProductName") as Label;
                Label lblBatch = r.FindControl("lblBatch") as Label;
                Label lblExpDate = r.FindControl("lblExpDate") as Label;
                Label lblUQty = r.FindControl("lblUQty") as Label;
                Label lblUUnit = r.FindControl("lblUUnit") as Label;
                TextBox lblQty = r.FindControl("txtGridQty") as TextBox;
                Label lblUnit = r.FindControl("lblUnit") as Label;
                Label lblRate = r.FindControl("lblRate") as Label;
                Label lblItemTotal = r.FindControl("lblItemTotal") as Label;
                Label lblScheDisc = r.FindControl("lblScheDisc") as Label;
                Label lblAfterScheDisc = r.FindControl("lblAfterScheDisc") as Label;
                DataRow dummyRw = dummyTable.NewRow();

                if (lblProductPK_ID.Text == ddlProduct.SelectedValue && lblRate.Text == txtRate.Text && lblUQty.Text == txtUQty.Text) // the selected product is already in the grid update the qty and rate
                {
                    dummyRw["PK_ID"] = lblProductPK_ID.Text;
                    dummyRw["TAXABLE"] = lblTaxable.Text;
                    dummyRw["PRODUCT_CODE"] = lblProductCode.Text;
                    dummyRw["PRODUCT"] = lblProductName.Text;
                    dummyRw["BATCH_NO"] = lblBatch.Text;
                    dummyRw["EXPIRY_DATE"] = lblExpDate.Text;
                    dummyRw["U_QUANTITY"] = txtUQty.Text;
                    dummyRw["U_UNIT"] = lblUUnit.Text;
                    dummyRw["QUANTITY"] = txtQty.Text;
                    dummyRw["UNIT"] = lblUnit.Text;
                    dummyRw["RATE"] = txtRate.Text;
                    dummyRw["TOTAL"] = txtAmount.Text;
                    double sd = 0;
                    try
                    {
                        sd = Convert.ToDouble(txtScheDisc.Text);
                    }
                    catch { }
                    dummyRw["SCHE_DISC"] = sd.ToString();
                    dummyRw["AFTER_SCHE_DISC"] = txtAmount.Text;

                    flag = true;
                }
                else // the selected product is not in the grid add the product detail in the grid
                {
                    dummyRw["PK_ID"] = lblProductPK_ID.Text;
                    dummyRw["TAXABLE"] = lblTaxable.Text;
                    dummyRw["PRODUCT_CODE"] = lblProductCode.Text;
                    dummyRw["PRODUCT"] = lblProductName.Text;
                    dummyRw["BATCH_NO"] = lblBatch.Text;
                    dummyRw["EXPIRY_DATE"] = lblExpDate.Text;
                    dummyRw["U_QUANTITY"] = lblUQty.Text;
                    dummyRw["U_UNIT"] = lblUUnit.Text;
                    dummyRw["QUANTITY"] = lblQty.Text;
                    dummyRw["UNIT"] = lblUnit.Text;
                    dummyRw["RATE"] = lblRate.Text;
                    dummyRw["TOTAL"] = lblItemTotal.Text;
                    dummyRw["SCHE_DISC"] = lblScheDisc.Text;
                    dummyRw["AFTER_SCHE_DISC"] = lblAfterScheDisc.Text;
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
                dummyRow["TAXABLE"] = PEnt.TAX_STATUS;
                dummyRow["PRODUCT_CODE"] = PEnt.PRODUCT_CODE;
                dummyRow["PRODUCT"] = PEnt.PRODUCT_NAME;

                PIDEnt = new PURCHASE_INVOICE_DETAIL();
                PIDEnt.PRODUCT_ID = PEnt.PK_ID;
                PIDEnt.BATCH_NUMBER = ddlBatch.SelectedValue;
                PIDEnt = (PURCHASE_INVOICE_DETAIL)PIDSer.GetSingle(PIDEnt);
                if (PIDEnt != null)
                {
                    dummyRow["BATCH_NO"] = PIDEnt.BATCH_NUMBER;
                    dummyRow["EXPIRY_DATE"] = PIDEnt.EXPIRY_DATE;
                }

                dummyRow["U_QUANTITY"] = txtUQty.Text;
                if (txtUQty.Text != "" && txtUQty.Text != "0")
                    dummyRow["U_UNIT"] = getUnitName(PEnt.UPPER_UNIT_ID);
                dummyRow["QUANTITY"] = txtQty.Text;
                dummyRow["UNIT"] = getUnitName(PEnt.UNIT_ID);
                dummyRow["RATE"] = txtRate.Text;
                dummyRow["TOTAL"] = txtAmount.Text;
                double sd = 0;
                try
                {
                    sd = Convert.ToDouble(txtScheDisc.Text);
                }
                catch { }
                dummyRow["SCHE_DISC"] = sd.ToString();
                dummyRow["AFTER_SCHE_DISC"] = txtScheDisc.Text;
                double asd = 0;
                try
                {
                    asd = Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtScheDisc.Text);
                    dummyRow["AFTER_SCHE_DISC"] = asd.ToString();
                }
                catch
                {
                    dummyRow["AFTER_SCHE_DISC"] = txtAmount.Text;
                }
                dummyTable.Rows.Add(dummyRow);
            }
        }

        DataView dv = new DataView(dummyTable);

        grdSalesDetail.DataSource = dummyTable;
        grdSalesDetail.DataBind();

        getTotal();
        return dummyTable;
    }
    protected void getTotal()
    {
        double total = 0;
        double ScheDisc = 0;
        double discount_percent = 0;
        double discount = 0;
        double beforedecimal = 0;
        double afterdecimal = 0;
        foreach (GridViewRow gr in grdSalesDetail.Rows)
        {
            Label lblItemTotal = gr.FindControl("lblItemTotal") as Label;
            Label lblScheDisc = gr.FindControl("lblScheDisc") as Label;
            total = total + (Convert.ToDouble(lblItemTotal.Text));
            try
            {
                ScheDisc = ScheDisc + (Convert.ToDouble(lblScheDisc.Text));
            }
            catch { }
        }
        try
        {
            discount_percent = Convert.ToDouble(txtDiscount.Text);
        }
        catch
        { txtDiscount.Text = "0"; }
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
        lblTotalScheDisc.Text = ScheDisc.ToString("#0.00");
        lblAfterSchDiscount.Text = (total - ScheDisc).ToString("#0.00");
        lblTotalAmount.Text = (total - ScheDisc - discount).ToString("#0.00");
        double vat = Convert.ToDouble(txtVATPercent.Text) / 100;
        lblVAT.Text = ((total - ScheDisc - discount) * vat).ToString("#0.00");

        // Total before VAT Return is deducted
        double preReturnTotal = Convert.ToDouble(lblVAT.Text) + Convert.ToDouble(lblTotalAmount.Text);

        // VAT Return: only applies when Sales Type is QR. 10% of VAT, deducted before Grand Total is shown.
        double vatReturn = 0;
        if (ddlPaymentType.SelectedValue == "QR")
        {
            vatReturn = Convert.ToDouble(lblVAT.Text) * 0.10;
            lblVATReturn.Text = vatReturn.ToString("#0.00");
            trVATReturn.Visible = true;
        }
        else
        {
            lblVATReturn.Text = "0.00";
            trVATReturn.Visible = false;
        }

        // Grand Total now already reflects the VAT Return deduction
        lblGrandTotal.Text = (preReturnTotal - vatReturn).ToString("#0.00");
        double grandTotalForRound = Convert.ToDouble(lblGrandTotal.Text);

        if (PGPS.RoundOff() == true)
        {
            beforedecimal = Math.Truncate(grandTotalForRound);
            afterdecimal = grandTotalForRound - beforedecimal;
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
        else
        {
            txtRound.Text = "0.00";
            lblInvoiceAmount.Text = grandTotalForRound.ToString("#0.00");
        }
    }
    protected void grdSalesDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            CreateGrid(gr.RowIndex, false);
        }
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
            //if (grdSalesDetail.Rows.Count <= 20)
            //    CreateGrid(1, true);
            //else
            //    HelperFunction.MsgBox(this, this.GetType(), "Only 20 itesm can be sold for this bill.");
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
        //chkWalkIn.Checked = false;
        divWalkInCustomer.Visible = false;
        divExistingCustomer.Visible = true;
        txtCustomerCode.Focus();
        ddlAgentName.SelectedValue = "Select";
        ddlArea.SelectedValue = "Select";
        ddlPaymentType.Enabled = true;
        divCredit.Visible = false;
        CreateGridFirst();
        txtPONumber.Text = "";
        txtRemarks.Text = "";
        ClearProduct();
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

    protected string TruncateText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;
        if (text.Length <= maxLength) return text;
        return text.Substring(0, maxLength).TrimEnd() + "...";
    }

    protected void gridSalesInvoice80_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblProName80 = e.Row.FindControl("lblProName80") as Label;
            if (lblProName80 != null)
            {
                lblProName80.Text = TruncateText(lblProName80.Text, 20);
            }
        }
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
                CustomerBalance(CEnt.CUSTOMER_CODE);
                try
                {
                    ddlArea.SelectedValue = CEnt.AREA_ID;
                }
                catch { }
                try
                {
                    ddlAgentName.SelectedValue = CEnt.AGENT_ID;
                }
                catch { }
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
            CustomerBalance(txtCustomerCode.Text);
            try
            {
                ddlArea.SelectedValue = CEnt.AREA_ID;
            }
            catch { }
            try
            {
                ddlAgentName.SelectedValue = CEnt.AGENT_ID;
            }
            catch { }
        }
        else
        {
            ddlCustomer.SelectedValue = "Select";
            txtCustomerCode.Text = "";
            txtCustomerCode.Focus();
            HelperFunction.MsgBox(this, this.GetType(), "Not a Valid Customer Code.");
        }
    }

    protected void LoadBatch(string product)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        ddlBatch.Items.Clear();
        ArrayList batch_array = new ArrayList();
        DataTable DT;
        string batch = null;
        if (ddlBatch.SelectedValue != "")
            batch = ddlBatch.SelectedValue;
        DT = hf.getOpeningClosingBalance(PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), null, null, product, null, null, null, null, userProfile.LocationID);

        if (DT != null && DT.Rows.Count > 0)
        {
            int columnIndex = DT.Columns.IndexOf("batch_number");
            // Check if the column exists
            if (columnIndex != -1)
            {
                // Iterate through the rows of the DataTable and retrieve the values in the specified column
                foreach (DataRow row in DT.Rows)
                {
                    // Access the value in the specified column using the column index
                    object columnValue = row[columnIndex];
                    batch_array.Add(columnValue.ToString());
                }
            }
        }
        ddlBatch.DataSource = batch_array;
        ddlBatch.DataBind();
    }

    protected void CustomerBalance(string Customer_Code)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        CEnt = new CUSTOMER();
        CEnt.CUSTOMER_CODE = Customer_Code;
        CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
        if (CEnt != null)
        {
            txtCustomerCreditLimit.Text = CEnt.CREDIT_LIMIT;
            DataTable newDT = af.getBalance("01", null, CEnt.CUSTOMER_CODE, PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfile.LocationID);
            if (newDT.Rows.Count > 0)
                txtCustomerBalance.Text = newDT.Rows[0][2].ToString();
            else
                txtCustomerBalance.Text = "0";
        }
        double credit_limit = 0;
        double balance = 0;
        try
        {
            credit_limit = Convert.ToDouble(CEnt.CREDIT_LIMIT);
        }
        catch { txtCustomerCreditLimit.Text = "0"; }
        try
        {
            balance = Convert.ToDouble(txtCustomerBalance.Text);
        }
        catch { }
        if ((credit_limit - balance) < 0 && ddlPaymentType.SelectedValue == "CR")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Credit Limit Exceeded.");
        }
    }

    protected void LoadAvailablity(string product, string batch)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (PGPS.SHOW_AVAILABILITY() || PGPS.OnlyStockSales())
        {
            DataTable DT;
            DT = hf.getOpeningClosingBalance(PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), null, null, product, batch, null, null, null, userProfile.LocationID);

            if (DT != null && DT.Rows.Count > 0)
            {
                int closing_columnIndex = DT.Columns.IndexOf("CLOSING");
                // Check if the column exists
                if (closing_columnIndex != -1)
                {
                    // Iterate through the rows of the DataTable and retrieve the values in the specified column
                    foreach (DataRow row in DT.Rows)
                    {
                        // Access the value in the specified column using the column index
                        object columnValue = row[closing_columnIndex];
                        txtAvilableQty.Text = columnValue.ToString();
                    }
                }
                int expiry_columnIndex = DT.Columns.IndexOf("expiry_date");
                // Check if the column exists
                if (expiry_columnIndex != -1)
                {
                    // Iterate through the rows of the DataTable and retrieve the values in the specified column
                    foreach (DataRow row in DT.Rows)
                    {
                        // Access the value in the specified column using the column index
                        object columnValue = row[expiry_columnIndex];
                        txtExpDate.Text = columnValue.ToString();
                    }
                }

            }
            else
            {
                txtAvilableQty.Text = "0";
            }
            if (PGPS.OnlyStockSales())
            {
                if (Convert.ToInt32(txtAvilableQty.Text) > 0)
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string unique_token = "";
        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.NEXT_INVOIICE_ID = lblunique_token.Text;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
        if (SIMEnt != null)
            unique_token = SIMEnt.NEXT_INVOIICE_ID;

        if (!IsPageRefresh && unique_token == "") // to check if it is post back 
        {
            Boolean ready = true;
            string msg = "";
            //if (chkWalkIn.Checked == true) // if walk in customer is true
            //{
            //    if (txtCustomerName.Text == "") // if name of walk in customer is not provided
            //        txtCustomerName.Text = "Cash Sales";
            //}
            //else
            //{
                ready = txtCustomerCode.Text == "" ? false : true; // if it is from existing customer customer code should not be empty
                if (!ready)
                    msg = msg + "Select Customer.";
            //}

            if (!(grdSalesDetail.Rows.Count > 0))
            {
                msg = msg + "Atleast 1 product must be added to save the sales.";
            }

            if (msg == "")
            {
                DistributedTransaction DT = new DistributedTransaction();
                userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

                string voucher_pk_id = "";
                #region to insert into sales master
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
                //if (chkWalkIn.Checked == true)
                //{
                //    SIMEnt.CUSTOMER_NAME = txtCustomerName.Text;
                //}
                //else
                //{
                    SIMEnt.CUSTOMER_ID = ddlCustomer.SelectedValue;
                    SIMEnt.CUSTOMER_NAME = getSustomerName(ddlCustomer.SelectedValue);
                //}
                SIMEnt.CUSTOMER_ADDRESS = txtCustomerAddress.Text;
                SIMEnt.COSTOMER_PAN_VAT = txtCustomerPANVAT.Text;
                SIMEnt.CUSTOMER_CONTACT_NUMBER = txtCustomerContact.Text;
                SIMEnt.INVOICE_TIME = DateTime.Now.ToString("HH:mm:ss");
                SIMEnt.USER_ID = userProfile.EmployeeID;
                SIMEnt.TAXABLE_SUB_TOTAL = Convert.ToDouble(lblAfterSchDiscount.Text).ToString("##0.00");
                SIMEnt.DISCOUNT_PERCENT = txtDiscount.Text;
                if (ddlExempted.SelectedValue == "1")//Taxable
                {
                    SIMEnt.EXEMPTED = "0";
                }
                else
                {
                    SIMEnt.EXEMPTED = "1";
                }

                SIMEnt.TAXABLE_DISC_AMOUNT = Convert.ToDouble(txtDiscountAmount.Text).ToString("##0.00");
                SIMEnt.TAXABLE_TOTAL = Convert.ToDouble(lblTotalAmount.Text).ToString("##0.00");
                SIMEnt.TAX_VAT_AMOUNT = Convert.ToDouble(lblVAT.Text).ToString("##0.00");
                SIMEnt.GRAND_TOTAL = Convert.ToDouble(lblGrandTotal.Text).ToString("##0.00");
                SIMEnt.ROUND_OFF = txtRound.Text;
                SIMEnt.INVOICE_AMOUNT = Convert.ToDouble(lblInvoiceAmount.Text).ToString("##0.00");
                double vatReturnAmount = 0;
                try { vatReturnAmount = Convert.ToDouble(lblVATReturn.Text); } catch { }
                if (vatReturnAmount > 0)
                {
                    SIMEnt.VAT_RETURN = vatReturnAmount.ToString("##0.00");
                }
                SIMEnt.IS_PRINTED = "1";
                SIMEnt.COPY_PRINTNO = "0";
                SIMEnt.CANCEL_STATUS = "0";
                SIMEnt.SALES_TYPE_ID = ddlPaymentType.SelectedValue;
                if (ddlPaymentType.SelectedValue == "QR")
                {
                    SIMEnt.TRANSACTION_ID = Session["PS_QR_TransactionId"] as string;
                    SIMEnt.QR_TYPE = Session["PS_QR_Type"] as string ?? "NepalPay";
                }
                SIMEnt.NEXT_INVOIICE_ID = lblunique_token.Text;
                SIMEnt.PO_NUMBER = txtPONumber.Text;
                SIMEnt.REMARKS = txtRemarks.Text;
                SIMEnt.OFFICE_CODE = userProfile.LocationID;
                string pk_id = "";
                try
                {
                    pk_id = SIMSer.Insert(SIMEnt, DT).ToString();
                }
                catch (Exception ex)
                {
                    HelperFunction.MsgBox(this, this.GetType(), "DEBUG INSERT ERROR: " + ex.Message);
                    return;
                }



                // ===== UPDATE (same office+date) / INSERT (new office or date) ORDER NUMBER =====
                string todayEng = PGD.GetTodayDate("dd/mm/yyyy"); // matches VOUCHER_DATE format used elsewhere

                ONEnt = new ORDER_NUMBER();
                ONEnt.OFFICE_CODE = userProfile.LocationID;
                ONEnt.ORD_DATE = todayEng;
                ONEnt = (ORDER_NUMBER)ONSer.GetSingle(ONEnt, DT); // look for existing row for this office+date

                if (ONEnt != null)
                {
                    // same office, same date -> increment existing row
                    int nextOrder = Convert.ToInt32(ONEnt.ORDER_NO) + 1;
                    ONEnt.ORDER_NO = nextOrder.ToString();
                    ONSer.Update(ONEnt, DT);
                    currentOrderNumber = nextOrder.ToString();
                }
                else
                {
                    // new office or new date -> SP inserts a fresh row starting at 1
                    ONEnt = new ORDER_NUMBER();
                    ONEnt.OFFICE_CODE = userProfile.LocationID;
                    ONEnt.ORD_DATE = todayEng;
                    string newOrderPkId = ONSer.Insert(ONEnt, DT).ToString();

                    ONEnt = new ORDER_NUMBER();
                    ONEnt.PK_ID = newOrderPkId;
                    ONEnt = (ORDER_NUMBER)ONSer.GetSingle(ONEnt, DT);
                    currentOrderNumber = ONEnt != null ? ONEnt.ORDER_NO : "1";
                }
                // ====================================================



                #endregion
                #region to insert in to sales detail
                foreach (GridViewRow gr in grdSalesDetail.Rows)
                {
                    Label lblSno = (Label)gr.FindControl("lblSno");
                    Label lblTaxable = (Label)gr.FindControl("lblTaxable");
                    Label lblProductPK_ID = (Label)gr.FindControl("lblProductPK_ID");
                    Label lblUQty = (Label)gr.FindControl("lblUQty");
                    TextBox lblQty = (TextBox)gr.FindControl("txtGridQty");
                    Label lblRate = (Label)gr.FindControl("lblRate");
                    Label lblItemTotal = (Label)gr.FindControl("lblItemTotal");
                    Label lblScheDisc = (Label)gr.FindControl("lblScheDisc");
                    Label lblAfterScheDisc = (Label)gr.FindControl("lblAfterScheDisc");
                    Label lblBatch = (Label)gr.FindControl("lblBatch");
                    Label lblExpDate = (Label)gr.FindControl("lblExpDate");


                    SIDEnt = new SALES_INVOICE_DETAIL();
                    SIDEnt.SALES_INVOICE_ID = pk_id;
                    SIDEnt.SNO = lblSno.Text;
                    SIDEnt.PRODUCT_ID = lblProductPK_ID.Text;
                    SIDEnt.UPPER_QUANTITY = lblUQty.Text;
                    SIDEnt.QUANTITY = lblQty.Text;
                    SIDEnt.RATE = lblRate.Text;
                    SIDEnt.TOTAL = lblItemTotal.Text;
                    SIDEnt.SCHEME_DISCOUNT = lblScheDisc.Text;
                    if (lblTaxable.Text == "1")
                    {
                        SIDEnt.TAXABLE_TOTAL = (Convert.ToDouble(lblAfterScheDisc.Text)).ToString();
                        SIDEnt.NON_TAXABLE_TOTAL = "0";
                        SIDEnt.TAX_AMOUNT = ((Convert.ToDouble(lblAfterScheDisc.Text)) * Convert.ToDouble(PG.CompanyTAXPercent()) / 100).ToString();
                        SIDEnt.AMOUNT_AFTER_TAX = (Convert.ToDouble(lblAfterScheDisc.Text) + (Convert.ToDouble(lblAfterScheDisc.Text)) * Convert.ToDouble(PG.CompanyTAXPercent()) / 100).ToString();
                    }
                    else
                    {
                        SIDEnt.TAXABLE_TOTAL = "0";
                        SIDEnt.NON_TAXABLE_TOTAL = (Convert.ToDouble(lblAfterScheDisc.Text)).ToString();
                        SIDEnt.TAX_AMOUNT = "0";
                        SIDEnt.AMOUNT_AFTER_TAX = (Convert.ToDouble(lblAfterScheDisc.Text)).ToString();
                    }
                    SIDEnt.EXPIRY_DATE = lblExpDate.Text;
                    SIDEnt.BATCH_NO = lblBatch.Text;
                    SIDEnt.OFFICE_CODE = userProfile.LocationID;
                    SIDSer.Insert(SIDEnt, DT);
                }
                #endregion


                #region account portion             

                string invice_number = "";
                SIMEnt = new SALES_INVOICE_MASTER();
                SIMEnt.PK_ID = pk_id;
                SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt, DT);
                if (SIMEnt != null)
                {
                    invice_number = SIMEnt.INVOICE_NUMBER;
                }

                #region in vouceher master and child  

                #region to insert in Voucher Master as JV for party and CN for Walkin Customer

                VMEnt = new VOUCHER_MASTER();
                //if (chkWalkIn.Checked == false) // party bhaye matra journal bancha nabhaye sidai Credit voucher bancha
                //{
                    VMEnt.VOUCHER_TYPE = "JV";
                    VMEnt.VOUCHER_NUMBER = af.getNext_VM_ID("JV", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfile.LocationID);
                //}
                //else
                //{
                //    VMEnt.VOUCHER_TYPE = "CV";
                //    VMEnt.VOUCHER_NUMBER = af.getNext_VM_ID("CV", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfile.LocationID);
                //}
                VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                VMEnt.VOUCHER_DAY = PGD.NepaliDay();
                VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
                VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
                VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                VMEnt.TRN_AMOUNT = lblInvoiceAmount.Text;
                VMEnt.NARRATION = "Sales Invoice no." + invice_number;
                VMEnt.REF_TABLE = "Invoice Master";
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

                SIMEnt = new SALES_INVOICE_MASTER();
                SIMEnt.PK_ID = pk_id;
                SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt, DT);
                if (SIMEnt != null)
                {
                    SIMEnt.SB_VM_ID = voucher_pk_id;
                    SIMSer.Update(SIMEnt, DT);
                }

                #region to insert in to voucher child
                int sno = 1;
                #region for Dr Part
                //if (chkWalkIn.Checked == false) // party bhaye matra party ko detail jancha  
                //{
                    #region for Party
                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = voucher_pk_id;
                    VCEnt.SNO = sno.ToString();
                    VCEnt.GL_CODE = "010301"; //GL_CODE of Sundry Debitors Customer
                    VCEnt.SGL_CODE = txtCustomerCode.Text;
                    VCEnt.DR_AMOUNT = lblInvoiceAmount.Text;
                    VCEnt.CR_AMOUNT = "0";
                    VCEnt.REMARKS = "By";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCSer.Insert(VCEnt, DT);
                    sno++;
                    #endregion
                //}
                //else // party hoena bhane cash jancha
                //{
                //    #region for Walk in Customer
                //    VCEnt = new VOUCHER_CHILD();
                //    VCEnt.VOUCHER_ID = voucher_pk_id;
                //    VCEnt.SNO = sno.ToString();
                //    VCEnt.GL_CODE = "010102"; //GL_CODE of Cash
                //    VCEnt.SGL_CODE = "";
                //    VCEnt.DR_AMOUNT = lblInvoiceAmount.Text;
                //    VCEnt.CR_AMOUNT = "0";
                //    VCEnt.REMARKS = "By";
                //    VCEnt.OFFICE_CODE = userProfile.LocationID;
                //    VCSer.Insert(VCEnt, DT);
                //    sno++;
                //    #endregion
                //}

                #region for Discount                 

                VCEnt = new VOUCHER_CHILD();
                VCEnt.VOUCHER_ID = voucher_pk_id;
                VCEnt.SNO = sno.ToString();
                VCEnt.GL_CODE = "030201"; //GL_CODE of Discount  
                VCEnt.DR_AMOUNT = txtDiscountAmount.Text;
                VCEnt.CR_AMOUNT = "0";
                VCEnt.REMARKS = "By";
                VCEnt.OFFICE_CODE = userProfile.LocationID;
                if (txtDiscountAmount.Text != "0") /// discount cha bhaye matra insert garni
                {
                    VCSer.Insert(VCEnt, DT);
                    sno++;
                }

                #endregion

                #region for Round Off; if round off is -ve 
                double round = 0;
                double round_value = 0;
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
                VCEnt.GL_CODE = "030202"; //GL_CODE of Round Off ; Indirect Expenses  
                VCEnt.DR_AMOUNT = round.ToString();
                VCEnt.CR_AMOUNT = "0";
                VCEnt.REMARKS = "By";
                VCEnt.OFFICE_CODE = userProfile.LocationID;
                if (round_value < 0) /// discount cha bhaye matra insert garni
                {
                    VCSer.Insert(VCEnt, DT);
                    sno++;
                }

                #endregion
                #endregion
                #region for Cr Part
                #region for Sales
                foreach (GridViewRow gvr in grdSalesDetail.Rows)
                {
                    Label lblProductCode = gvr.FindControl("lblProductCode") as Label;
                    Label lblAfterScheDisc = gvr.FindControl("lblAfterScheDisc") as Label;

                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = voucher_pk_id;
                    VCEnt.SNO = sno.ToString();
                    VCEnt.GL_CODE = "020101";  //Sales Account                      
                    VCEnt.SGL_CODE = lblProductCode.Text;
                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = lblAfterScheDisc.Text;
                    VCEnt.REMARKS = "To";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCSer.Insert(VCEnt, DT);
                    sno++;
                }
                #endregion
                #region for VAT Part
                if (ddlExempted.SelectedValue == "1") // 1 is VAT
                {
                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = voucher_pk_id;
                    VCEnt.SNO = sno.ToString();
                    VCEnt.GL_CODE = "040108";  //VAT Account                      
                    VCEnt.SGL_CODE = "";
                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = lblVAT.Text;
                    VCEnt.REMARKS = "To";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    VCSer.Insert(VCEnt, DT);
                    sno++;
                }
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
                VCEnt.OFFICE_CODE = userProfile.LocationID;
                if (round_value > 0) /// discount cha bhaye matra insert garni
                {
                    VCSer.Insert(VCEnt, DT);
                    sno++;
                }

                #endregion
                #endregion
                #endregion

                #endregion

                #region to insert in voucher master and child if sales is in cash or bank for party only as Credit Voucher
                if (ddlPaymentType.SelectedValue != "CR" )
                {
                    #region to insert in Voucher Master                   
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
                    //if (chkWalkIn.Checked == false)
                        VMEnt.NARRATION = "Payment received from " + ddlCustomer.SelectedItem.ToString() + " for Invoice no." + invice_number;
                    //else
                    //    VMEnt.NARRATION = "Payment received from " + txtCustomerName.Text + " for Invoice no." + invice_number;
                    VMEnt.REF_TABLE = "Invoice Master";
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
                    if (ddlPaymentType.SelectedValue == "CS")
                    {
                        VCEnt.GL_CODE = "010102"; //GL_CODE of Cash
                    }
                    else if (ddlPaymentType.SelectedValue == "QR")
                    {
                        VCEnt.GL_CODE = "010101"; //GL_CODE of Bank
                        VCEnt.SGL_CODE = ddlBank.SelectedValue;
                    }
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
                    VCEnt.GL_CODE = "010301"; //GL_CODE of Sundry Debitors Customer
                    VCEnt.SGL_CODE = txtCustomerCode.Text;
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
                    #region to send in CBMS
                    lblPK_ID.Text = pk_id;
                    SIMEnt = new SALES_INVOICE_MASTER();
                    SIMEnt.PK_ID = pk_id;
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
                                    // yo line le APL ko SSL certificate expire bhaye pani kam garni huncha 
                                    // yo first line 1 ta bhaye na bhane SSL certificate expire bhayo bhana API le kam gardaina
                                    //  ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

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
                        }
                        else
                        {
                            SIMEnt.CBMS_PUSH = "CBMS OFF";
                            SIMEnt.CBMS_PUSH_RT = "";
                            SIMSer.Update(SIMEnt);
                        }
                    }
                    #endregion
                    SIMEnt = new SALES_INVOICE_MASTER();
                    SIMEnt.PK_ID = pk_id;
                    SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
                    if (SIMEnt != null)
                    {
                        lblInvoiceHeading.Text = PG.InvoiceHeading();
                        lblInvoiceHeading1.Text = "";

                        SIMEnt.PRINT_TIME = System.DateTime.Now.ToString();
                        SIMEnt.PRINT_BY = userProfile.EmployeeID;
                        SIMSer.Update(SIMEnt);
                        LoadToPrint(pk_id); // Customer Copy
                        btnOfficeCopy.Visible = true;
                        btnPrintTOK.Visible = true;

                        CreateGridFirst();
                        Clear();
                        lblunique_token.Text = userProfile.UserName + hf.getmaxinvid(); // this code is for not allowing double entry of same bill

                    }
                }
                else
                {
                    DT.Abort();

                    HelperFunction.MsgBox(this, this.GetType(), "Sorry Something Goes Wrong.");
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
            HelperFunction.MsgBox(this, this.GetType(), "This bill have been already saved.");
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
    protected void btnOfficeCopy_Click(object sender, EventArgs e)
    {
        if (PG.InvoiceHeading() == "TAX INVOICE")
        {
            lblInvoiceHeading.Text = "INVOICE";
        }
        else
        {
            lblInvoiceHeading.Text = PG.InvoiceHeading();
        }

        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.PK_ID = lblPK_ID.Text;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
        if (SIMEnt != null)
        {
            SIMEnt.PRINT_TIME = System.DateTime.Now.ToString();
            SIMEnt.PRINT_BY = userProfile.EmployeeID;
            //SIMEnt.COPY_PRINTNO = (Convert.ToDouble(SIMEnt.COPY_PRINTNO) + 1).ToString();
            btnOfficeCopy.Visible = false;
            //  SIMSer.Update(SIMEnt); 
            OfficeCopy = true;
            LoadToPrint(lblPK_ID.Text, "OFFICE"); // Office Copy

        }


    }
    #region to print bill
    protected void btnPrintTOK_Click(object sender, EventArgs e)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.PK_ID = lblPK_ID.Text;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
        if (SIMEnt != null)
        {
            LoadSalesGridTOK(SIMEnt.PK_ID);

            lblBillNoTOK.Text = SIMEnt.INVOICE_NUMBER;
            lblOrderNoTOK.Text = lblOrderNoHidden.Text;
            lblBillDateTOK.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
            lblCompanyNameTOK.Text = PG.CompanyName();
            lblCompanyAddressTOK.Text = PG.BranchAddress(userProfile.LocationID);
            lblPhoneTOK.Text = PG.BranchContact(userProfile.LocationID);
            lblPanNoTOK.Text = PG.CompanyVATPan();
            lblPrintedByTOK.Text = hf.getEmployeeName(SIMEnt.PRINT_BY);
            lblTimeTOK.Text = SIMEnt.PRINT_TIME;
            //lblNameTOK.Text = SIMEnt.CUSTOMER_NAME;

            printdetailTOK.Visible = true;
            btnPrintTOK.Visible = false;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printTOK();", true);
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

        // 80mm labels
        lblCompanyName80.Text = PG.CompanyName();
        lblCompanyAddress80.Text = PG.BranchAddress(userProfile.LocationID);
        lblPhone80.Text = PG.BranchContact(userProfile.LocationID);
        lblPanNo80.Text = PG.CompanyVATPan();
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
        lblTaxableAmount.Text = "";
        lblVATAmount.Text = "";
        lblGTotal.Text = "";
        lblRoundoff.Text = "";
        lblBillAmount.Text = "";
        lblAmountInWord.Text = "";

        lblInvVATReturn.Text = "";
        trInvVATReturn.Visible = false;
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
            lblVATPercent.Text = SIMEnt.TAX_VAT_AMOUNT == "0" ? "0" : (Convert.ToDouble(PG.CompanyTAXPercent()).ToString("00.00"));
            lblVATAmount.Text = Convert.ToDouble(SIMEnt.TAX_VAT_AMOUNT).ToString("##,##0.00");
            double vatReturnAmt = 0;
            try { vatReturnAmt = Convert.ToDouble(SIMEnt.VAT_RETURN); } catch { }

            if (SIMEnt.SALES_TYPE_ID == "QR" && vatReturnAmt > 0)
            {
                trInvVATReturn.Visible = true;
                lblInvVATReturn.Text = vatReturnAmt.ToString("##,##0.00");
            }
            else
            {
                trInvVATReturn.Visible = false;
            }
            double preReturnTotalPrint = Convert.ToDouble(SIMEnt.TAXABLE_TOTAL) + Convert.ToDouble(SIMEnt.TAX_VAT_AMOUNT);
            lblGTotal.Text = preReturnTotalPrint.ToString("##,##0.00");
            lblRoundoff.Text = Convert.ToDouble(SIMEnt.ROUND_OFF).ToString("##,##0.00");
            lblBillAmount.Text = Convert.ToDouble(SIMEnt.INVOICE_AMOUNT).ToString("##,##0.00");
            lblAmountInWord.Text = hf.NumWordsWrapper(Convert.ToDouble(SIMEnt.INVOICE_AMOUNT)) + " only";//.ToUpper()
            lblForCompanyName.Text = PG.CompanyName();
            lblPONumber.Text = SIMEnt.PO_NUMBER;
            if (SIMEnt.REMARKS != "")
            {
                lblRemarks.Text = "* " + SIMEnt.REMARKS;
            }

            LoadSalesGrid(SIMEnt.PK_ID);
            LoadSalesGrid80(SIMEnt.PK_ID);     // new
            LoadSalesGridTOK(SIMEnt.PK_ID);
            SetGridColumnVisibility();




            // ---- 80mm labels ----
            //lblOrderNo80.Text = currentOrderNumber;
            // ---- 80mm labels ----
            //lblOrderNo80.Text = currentOrderNumber;
            if (!string.IsNullOrEmpty(currentOrderNumber))
            {
                lblOrderNoHidden.Text = currentOrderNumber;
            }
            if (OfficeCopy == true)
                lblInvoiceHeading80.Text = (PG.InvoiceHeading() == "TAX INVOICE") ? "INVOICE" : PG.InvoiceHeading();
            else
                lblInvoiceHeading80.Text = PG.InvoiceHeading();
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
            lblVATPercent80.Text = SIMEnt.TAX_VAT_AMOUNT == "0" ? "0" : Convert.ToDouble(PG.CompanyTAXPercent()).ToString("00.00");
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

            // ---- TOK labels ----
            lblBillNoTOK.Text = SIMEnt.INVOICE_NUMBER;
            lblOrderNoTOK.Text = !string.IsNullOrEmpty(currentOrderNumber) ? currentOrderNumber : lblOrderNoHidden.Text;
            lblBillDateTOK.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
            lblCompanyNameTOK.Text = PG.CompanyName();
            lblCompanyAddressTOK.Text = PG.BranchAddress(userProfile.LocationID);
            lblPhoneTOK.Text = PG.BranchContact(userProfile.LocationID);
            lblPanNoTOK.Text = PG.CompanyVATPan();
            lblPrintedByTOK.Text = hf.getEmployeeName(SIMEnt.PRINT_BY);
            lblTimeTOK.Text = SIMEnt.PRINT_TIME;
            //lblNameTOK.Text = SIMEnt.CUSTOMER_NAME;


            // ---- Simple TOK labels (only used for the auto-print immediately after Save) ----
            lblOrderNoSimpleTOK.Text = currentOrderNumber;
            lblDateSimpleTOK.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
            lblNameSimpleTOK.Text = SIMEnt.CUSTOMER_NAME;

            // ---- Proforma Invoice labels ----
            PopulateProformaLabels(SIMEnt);
            LoadSalesGridProforma(SIMEnt.PK_ID);

            // Decide layout based on company's configured bill type


            // Decide layout based on company's configured bill type
            NEnt = new NAME_COMPANY();
            NEnt = (NAME_COMPANY)NSer.GetSingle(NEnt);
            string billType = (NEnt != null && !string.IsNullOrEmpty(NEnt.INVOICE_TYPE)) ? NEnt.INVOICE_TYPE : "A5L";

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
                // On Save, always print the Proforma Invoice first, then the normal invoice format right after.
                printdetailProforma.Visible = true;

                if (billType == "Continuous")
                {
                    printdetail80.Visible = true;
                    printdetailTOK.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printProformaThen80mmTOK();", true);
                }
                else
                {
                    printdetail.Visible = true;
                    printdetail80.Visible = false;
                    printdetailTOK.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printProformaThenA4();", true);
                }
            }
        }
    }

    protected void PopulateProformaLabels(SALES_INVOICE_MASTER SIMEnt)
    {
        lblProCompanyName.Text = PG.CompanyName();
        lblProCompanyAddress.Text = PG.BranchAddress(userProfile.LocationID);
        //lblProCompanyRegNo.Text = txtCompanyRegNo != null ? txtCompanyRegNo.Text : "";
        lblProCompanyPan.Text = PG.CompanyVATPan();

        // Reuses the existing tax-invoice number as-is (no separate Proforma numbering series)
        lblProInvoiceNo.Text = SIMEnt.INVOICE_NUMBER;
        lblProIssueDate.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR
            + " [" + SIMEnt.INVOICE_DATE + "]";
        lblProContractNo.Text = txtContractNo.Text;
        lblProContractDate.Text = txtContractDate.Text;

        lblProCustomerName.Text = SIMEnt.CUSTOMER_NAME;
        lblProCustomerAddress.Text = SIMEnt.CUSTOMER_ADDRESS;

        lblProPaymentCurrency.Text = txtPaymentCurrency.Text;
        lblProPaymentMode.Text = hf.getPaymentType(SIMEnt.SALES_TYPE_ID);
        lblProShipmentType.Text = txtShipmentType.Text;
        lblProShipmentNo.Text = txtShipmentNo.Text;

        lblProSubTotal.Text = Convert.ToDouble(SIMEnt.TAXABLE_SUB_TOTAL).ToString("##,##0.00");
        lblProAmountInWord.Text = txtPaymentCurrency.Text + hf.NumWordsWrapper(Convert.ToDouble(SIMEnt.INVOICE_AMOUNT)) + " only";

        //lblProForCompanyName.Text = PG.CompanyName();
        //lblProPrintedBy.Text = hf.getEmployeeName(SIMEnt.PRINT_BY);
        //lblProTime.Text = SIMEnt.PRINT_TIME;
    }

    protected void LoadSalesGridProforma(string PK_id)
    {
        DataTable dt = hf.LoadSalesInvoice(PK_id);
        gridProforma.DataSource = dt;
        gridProforma.DataBind();
    }

    protected void LoadSalesGrid(string PK_id)
    {
        DataTable dt = hf.LoadSalesInvoice(PK_id); // your existing data
        int desiredRowCount = 30;

        while (dt.Rows.Count < desiredRowCount)
        {
            dt.Rows.Add(dt.NewRow()); // add empty rows to reach 20
        }


        gridSalesInvoice.DataSource = dt;
        gridSalesInvoice.DataBind();
    }
    private void SetGridColumnVisibility()
    {
        gridSalesInvoice.Columns[1].Visible = PGPS.ProductBatch();       // was 2
        gridSalesInvoice.Columns[2].Visible = PGPS.ProductExpDate();     // was 3
        gridSalesInvoice.Columns[3].Visible = PGPS.ShowDualQuantity();   // was 4 — Upper Qty
                                                                         // 4 = Qty, 5 = Rate, 6 = Amount
        gridSalesInvoice.Columns[7].Visible = PGPS.ItemWiseDiscount();   // was 8 — Sch.Disc
        gridSalesInvoice.Columns[8].Visible = PGPS.ItemWiseDiscount();   // was 9 — Taxable Amount

        if (PG.CompanyTAXType() != "VAT")
            trInvVat.Visible = false;

        trInvRoundOff.Visible = PGPS.RoundOff();
    }

    #endregion

    protected string getSustomerName(string pk_id)
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
    protected void txtRound_TextChanged(object sender, EventArgs e)
    {
        double round = 0;
        try
        {
            round = Convert.ToDouble(txtRound.Text);
        }
        catch
        {
            txtRound.Text = "0.00";
        }
        lblInvoiceAmount.Text = (Convert.ToDouble(lblGrandTotal.Text) + round).ToString("#0.00");
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
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        LoadQty();

    }
    protected void LoadQty()
    {
        double Qty = 0;
        double rate = 0;
        string msg = "";
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
                        {
                            btnAdd.Visible = false;

                        }
                        else
                        {
                            btnAdd.Visible = true;
                        }

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
            txtScheDisc.Focus();
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
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        string batch = null;
        if (ddlBatch.SelectedValue != "")
            batch = ddlBatch.SelectedValue;
        LoadAvailablity(txtProductCode.Text, batch);
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
        tdAvaiQty.Visible = PGPS.SHOW_AVAILABILITY();
        tdPO.Visible = PGPS.ShowPO();
        trRoundOff.Visible = PGPS.RoundOff();
        tdDualUnit.Visible = PGPS.ShowDualQuantity();
        lblUUnit.Visible = PGPS.ShowDualQuantity();
        tdSchDisc.Visible = PGPS.ItemWiseDiscount();
        if (PG.CompanyTAXType() == "None")
        {
            trTaxableAmt.Visible = false;
        }
        if (!PGPS.RoundOff() && PG.CompanyTAXType() == "None")
        {
            trTaxableAmt.Visible = false;
            trNetAmount.Visible = false;
        }

        grdSalesDetail.Columns[3].Visible = PGPS.ProductBatch();
        grdSalesDetail.Columns[4].Visible = PGPS.ProductExpDate();
        grdSalesDetail.Columns[5].Visible = PGPS.DualQuantity();
        grdSalesDetail.Columns[9].Visible = PGPS.ItemWiseDiscount();
        grdSalesDetail.Columns[10].Visible = PGPS.ItemWiseDiscount();
        divPO.Visible = PGPS.ShowPO();
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
    protected void ddlProductRateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadRate();
        LoadRateType();
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

    protected void LoadSalesGrid80(string PK_id)
    {
        DataTable dt = hf.LoadSalesInvoice(PK_id);
        gridSalesInvoice80.DataSource = dt;
        gridSalesInvoice80.DataBind();
    }

    protected void LoadSalesGridTOK(string PK_id)
    {
        DataTable dt = hf.LoadSalesInvoice(PK_id);
        gridTOK.DataSource = dt;
        gridTOK.DataBind();
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
    }
    protected void txtGridQty_TextChanged(object sender, EventArgs e)
    {
        TextBox txtGridQty = sender as TextBox;
        GridViewRow row = (GridViewRow)txtGridQty.NamingContainer;

        Label lblRate = row.FindControl("lblRate") as Label;
        Label lblItemTotal = row.FindControl("lblItemTotal") as Label;
        Label lblScheDisc = row.FindControl("lblScheDisc") as Label;
        Label lblAfterScheDisc = row.FindControl("lblAfterScheDisc") as Label;

        double qty = 0;
        double rate = 0;

        try
        {
            qty = Convert.ToDouble(txtGridQty.Text);
        }
        catch
        {
            HelperFunction.MsgBox(this, this.GetType(), "Enter Number Only.");
            txtGridQty.Text = "0";
            qty = 0;
        }

        try
        {
            rate = Convert.ToDouble(lblRate.Text);
        }
        catch { rate = 0; }

        double total = qty * rate;
        lblItemTotal.Text = total.ToString("#0.00");

        double sd = 0;
        try { sd = Convert.ToDouble(lblScheDisc.Text); } catch { }
        lblAfterScheDisc.Text = (total - sd).ToString("#0.00");

        getTotal(); // refresh Sub Total / VAT / Grand Total etc.
    }




    protected async void btnShowQR_Click(object sender, EventArgs e)
    {
        decimal amount;
        if (!decimal.TryParse(lblInvoiceAmount.Text, out amount) || amount <= 0)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please add at least one item before generating the QR.");
            return;
        }

        try
        {
            string billNumber = DynamicQrPosService.GenerateBillNumber();
            var result = await DynamicQrPosService.GenerateDynamicQrAsync(amount, billNumber);

            if (!result.Success)
            {
                HelperFunction.MsgBox(this, this.GetType(), "QR generation failed: " + result.ErrorMessage);
                return;
            }

            imgQRPopup.ImageUrl = "data:image/png;base64," + result.QrImageBase64;
            lblQRAmount.Text = amount.ToString("#,##0.00");
            lblQRStatus.Text = "Waiting for payment...";

            DynamicQrPosService.RegisterTransaction(billNumber, result.ValidationTraceId, amount.ToString("0.00"));
            Session["PS_QR_BillNumber"] = billNumber;

            try { await DynamicQrPosService.WsSendTransactionDetailRequestAsync(result.ValidationTraceId); }
            catch (Exception wsEx) { DynamicQrPosService.LogWebhook("PS: WS subscribe failed: " + wsEx.Message); }

            // POS device I/O now happens client-side (PosLocalClient.js), since the server can no
            // longer reliably reach the USB device attached to this specific cashier's terminal
            // (see the removed-methods note in DynamicQrPosService.cs). We hand the QR payload to
            // the browser via a startup script, which calls showQrOnPos() already defined there.
            string scanText = string.IsNullOrEmpty(txtScan.Text) ? PG.CompanyName() : txtScan.Text;
            string script =
                "showQRPopup(); startQRPolling(); " +
                "showQrOnPos(" + amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + ", " +
                "'" + HttpUtility.JavaScriptStringEncode(scanText) + "', " +
                "'" + HttpUtility.JavaScriptStringEncode(result.QrString) + "')" +
                ".catch(function(err){ console.error('POS display error:', err); });";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "showQR", script, true);
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Error generating QR: " + ex.Message);
        }
    }

    protected void btnCloseQR_Click(object sender, EventArgs e)
    {
        Session["PS_QR_BillNumber"] = null;
        // Same reasoning as above: tell the LOCAL device (via the browser) to go idle,
        // instead of the server pushing to it directly.
        ScriptManager.RegisterStartupScript(this, this.GetType(), "hideQR",
            "hideQRPopup(); idlePos().catch(function(err){ console.error('POS idle error:', err); });", true);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static DynamicQrPosService.PaymentStatusResult CheckQRPaymentStatus()
    {
        var billNumber = HttpContext.Current.Session["PS_QR_BillNumber"] as string;
        var result = DynamicQrPosService.CheckPaymentStatus(billNumber);

        if (result.Status == "SUCCESS")
        {
            HttpContext.Current.Session["PS_QR_TransactionId"] = result.TransactionId;
            HttpContext.Current.Session["PS_QR_Type"] = result.QrType;
        }

        return result;
    }
}