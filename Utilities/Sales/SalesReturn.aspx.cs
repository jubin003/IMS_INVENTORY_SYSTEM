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
using Entity.Framework;
using System.Drawing;
using System.Net;
using System.Net.Security;

public partial class Utilities_Sales_SalesReturn : System.Web.UI.Page
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

    VOUCHER_MASTER VMEnt = new VOUCHER_MASTER();
    VOUCHER_MASTERService VMSer = new VOUCHER_MASTERService();

    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();

    VOUCHER_CHILD VCEnt = new VOUCHER_CHILD();
    VOUCHER_CHILDService VCSer = new VOUCHER_CHILDService();

    GL_SUB_ACCOUNT GLSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GLSASer = new GL_SUB_ACCOUNTService();

    AGENT AEnt = new AGENT();
    AGENTService ASer = new AGENTService();

    AREA AREAEnt = new AREA();
    AREAService AREASer = new AREAService();
    BANK_ACCOUNT BAEnt = new BANK_ACCOUNT();
    BANK_ACCOUNTService BASer = new BANK_ACCOUNTService();


    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfile = new UserProfileEntity();
    Boolean IsPageRefresh = false;
    AccountFunction af = new AccountFunction();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["postids"] = System.Guid.NewGuid().ToString();
            Session["postid"] = ViewState["postids"].ToString();
            try
            {
                userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                string path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
                if (hf.checkPageAccess(path, userProfile.UserGroupID.ToString()))
                {
                    LoadFiscalYear();
                    lblunique_token.Text = userProfile.UserName + hf.getmaxCNid(); // this code is for not allowing double entry of CN bill
                    txtVATPercent.Text = PG.CompanyTAXPercent();
                    LoadCustomers();
                    SetVisibility();
                    if (PG.CompanyTAXType() != "VAT")
                    {
                        divVAT1.Visible = false;
                    }
                    else
                    {
                        divVAT1.Visible = true;
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
    protected void LoadCustomers()
    {
        CEnt = new CUSTOMER();
        CEnt.STATUS = "1";
        ddlCustomer.DataSource = CSer.GetAll(CEnt);
        ddlCustomer.DataTextField = "CUSTOMER_FULLNAME";
        ddlCustomer.DataValueField = "PK_ID";
        ddlCustomer.DataBind();
        ddlCustomer.Items.Insert(0, "Select");
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
            grdSales.DataSource = hf.getSalesList("", "", PGD.GetEnglishDateFromNepali(txtFromDate.Text, "dd/mm/yyyy"), PGD.GetEnglishDateFromNepali(txtToDate.Text, "dd/mm/yyyy"), userProfile.LocationID);
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
            LoadPaymentType();
            divInvoiceDetail.Visible = true;
            divInitial.Visible = false;
            txtCreditNoteRemarks.Focus();
            CheckCreditReturn();
        }
    }
    protected void LoadPaymentType()
    {
        PTEnt = new PAYMENT_TYPE();
        PTEnt.SALES_PURCHASE = "S";
        PTEnt.STATUS = "1";
        ddlPaymentType.DataSource = PTSEr.GetAll(PTEnt);
        ddlPaymentType.DataTextField = "PAYMENT_NAME";
        ddlPaymentType.DataValueField = "PAYMENT_CODE";
        ddlPaymentType.DataBind();
    }



    protected void cleardata()
    {
        txtCustomerCode.Text = "";
        txtCustomerAddress.Text = "";
        txtCustomerPANVAT.Text = "";
        txtCustomerContact.Text = "";
        ddlCustomer.SelectedValue = "Select";
        txtInvoiceNumber.Text = "";
        txtCreditNoteDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
        ddlPaymentType.SelectedValue = "CS";
        lblSubTotalAmount.Text = "0.00";
        lblTotalAmount.Text = "0.00";
        txtDiscountAmount.Text = "0.00";
        txtDiscount.Text = "0";
        lblVAT.Text = "0.00";
        lblGrandTotal.Text = "0.00";
        txtRound.Text = "0.00";
        lblInvoiceAmount.Text = "0.00";
        divWalkInCustomer.Visible = false;
        txtCreditNoteRemarks.Text = "";
    }
    protected void LoadInvoiceDetail(string PK_ID)
    {
        cleardata();
        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.PK_ID = PK_ID;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
        if (SIMEnt != null)
        {
            if (SIMEnt.CUSTOMER_ID == "") //walk in customer
            {
                divExistingCustomer.Visible = false;
                divWalkInCustomer.Visible = true;
                txtCustomerName.Text = SIMEnt.CUSTOMER_NAME;
            }
            else
            {
                divExistingCustomer.Visible = true;
                divWalkInCustomer.Visible = false;
                CEnt = new CUSTOMER();
                CEnt.PK_ID = SIMEnt.CUSTOMER_ID;
                CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
                if (CEnt != null)
                {
                    txtCustomerCode.Text = CEnt.CUSTOMER_CODE;
                    ddlCustomer.SelectedValue = CEnt.PK_ID;
                    lblAgentCode.Text = CEnt.AGENT_ID;
                    lblAreaCode.Text = CEnt.AREA_ID;
                }
            }

            txtInvoiceNumber.Text = SIMEnt.INVOICE_NUMBER;
            txtCustomerAddress.Text = SIMEnt.CUSTOMER_ADDRESS;
            txtCustomerPANVAT.Text = SIMEnt.COSTOMER_PAN_VAT;
            txtInvoiceDate.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
            ddlPaymentType.SelectedValue = SIMEnt.SALES_TYPE_ID;
            //   txtDiscountAmount.Text = SIMEnt.TAXABLE_DISC_AMOUNT;

            if (SIMEnt.EXEMPTED == "1")
            {
                txtVATPercent.Text = "0";
                ddlExempted.SelectedValue = "0";
            }
            else
            {
                txtVATPercent.Text = PG.CompanyTAXPercent();
                ddlExempted.SelectedValue = "1";
            }
        }

        LoadGrid(PK_ID);
    }
    protected void grdSales_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblPK_ID = e.Row.FindControl("lblPK_ID") as Label;
            EntityList newList = new EntityList();
            SRMEnt = new SALES_RETURN_MASTER();
            SRMEnt.SALES_INVOICE_ID = lblPK_ID.Text;
            newList = SRMSer.GetAll(SRMEnt);
            if (newList.Count > 0)
            {
                e.Row.BackColor = ColorTranslator.FromHtml("#F85F3E");//81F79F
            }
        }
    }
    //===========================================================
    protected DataTable LoadGrid(string PIM_pk_id)
    {
        EntityList theList = new EntityList();
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("PRODUCT_CODE");
        dummyTable.Columns.Add("PRODUCT");
        dummyTable.Columns.Add("BATCH_NO");
        dummyTable.Columns.Add("EXPIRY_DATE");
        dummyTable.Columns.Add("UPPER_QUANTITY");
        dummyTable.Columns.Add("U_UNIT");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("R_QUANTITY");
        dummyTable.Columns.Add("R_UNIT");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("DISCOUNT");
        dummyTable.Columns.Add("ACTUAL_DISCOUNT");
        dummyTable.Columns.Add("TOTAL");
        dummyTable.Columns.Add("R_TOTAL");

        DataView dv = new DataView(dummyTable);

        SIDEnt = new SALES_INVOICE_DETAIL();
        SIDEnt.SALES_INVOICE_ID = PIM_pk_id;
        theList = SIDSer.GetAll(SIDEnt);
        if (theList.Count > 0)
        {
            foreach (SALES_INVOICE_DETAIL SID in theList)
            {
                DataRow dummyRow = dummyTable.NewRow();

                PEnt = new PRODUCT();
                PEnt.PK_ID = SID.PRODUCT_ID;
                PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                if (PEnt != null)
                {
                    dummyRow["PK_ID"] = PEnt.PK_ID;
                    dummyRow["PRODUCT_CODE"] = PEnt.PRODUCT_CODE;
                    dummyRow["PRODUCT"] = PEnt.PRODUCT_NAME;
                    dummyRow["BATCH_NO"] = SID.BATCH_NO;
                    dummyRow["EXPIRY_DATE"] = SID.EXPIRY_DATE;
                    dummyRow["UPPER_QUANTITY"] = SID.UPPER_QUANTITY;
                    if (SID.UPPER_QUANTITY != "" && SID.UPPER_QUANTITY != "0")
                        dummyRow["U_UNIT"] = getUnitName(PEnt.UPPER_UNIT_ID);

                    dummyRow["QUANTITY"] = SID.QUANTITY;
                    dummyRow["UNIT"] = getUnitName(PEnt.UNIT_ID);
                    dummyRow["R_QUANTITY"] = "";
                    dummyRow["R_UNIT"] = getUnitName(PEnt.UNIT_ID);
                    dummyRow["RATE"] = Convert.ToDouble(SID.RATE).ToString("0.00");
                    dummyRow["TOTAL"] = Convert.ToDouble(SID.TOTAL).ToString("0.00");
                    dummyRow["DISCOUNT"] = "";
                    dummyRow["ACTUAL_DISCOUNT"] = (Convert.ToDouble(SID.SCHEME_DISCOUNT) / Convert.ToDouble(SID.QUANTITY)).ToString("0.0000");
                    dummyRow["R_TOTAL"] = "";
                    dummyTable.Rows.Add(dummyRow);
                }
            }
        }
        grdSalesDetail.DataSource = dv;
        grdSalesDetail.DataBind();
        return dummyTable;
    }
    protected void grdSalesDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblProductPK_ID = e.Row.FindControl("lblProductPK_ID") as Label;
            Label lblQty = e.Row.FindControl("lblQty") as Label;
            Label lblRemQty = e.Row.FindControl("lblRemQty") as Label;

            //   
            double rem_stock = 0;
            SIDEnt = new SALES_INVOICE_DETAIL();
            SIDEnt.SALES_INVOICE_ID = lblPK_ID_Hiden.Text;
            SIDEnt.PRODUCT_ID = lblProductPK_ID.Text;
            SIDEnt = (SALES_INVOICE_DETAIL)SIDSer.GetSingle(SIDEnt);
            if (SIDEnt != null)
            {
                rem_stock = Convert.ToDouble(SIDEnt.QUANTITY);
            }

            EntityList newList = new EntityList();
            SRMEnt = new SALES_RETURN_MASTER();
            SRMEnt.SALES_INVOICE_ID = lblPK_ID_Hiden.Text;
            newList = SRMSer.GetAll(SRMEnt);
            if (newList.Count > 0)
            {

                foreach (SALES_RETURN_MASTER srm in newList)
                {
                    SRDEnt = new SALES_RETURN_DETAIL();
                    SRDEnt.SALES_RETURN_ID = srm.PK_ID;
                    SRDEnt.PRODUCT_ID = lblProductPK_ID.Text;
                    SRDEnt = (SALES_RETURN_DETAIL)SRDSer.GetSingle(SRDEnt);
                    if (SRDEnt != null)
                    {
                        rem_stock = rem_stock - Convert.ToDouble(SRDEnt.QUANTITY);

                    }
                }
            }
            lblRemQty.Text = rem_stock.ToString();
        }

    }
    protected void txtRQty_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gr = ((TextBox)sender).Parent.Parent as GridViewRow;
        TextBox lblItemRTotal = gr.FindControl("lblItemRTotal") as TextBox;
        TextBox txtRQty = gr.FindControl("txtRQty") as TextBox;
        Label lblRate = gr.FindControl("lblRate") as Label;
        Label lblRemQty = gr.FindControl("lblRemQty") as Label;
        Label lblSchDiscount = gr.FindControl("lblSchDiscount") as Label;
        Label lblActualSchDiscount = gr.FindControl("lblActualSchDiscount") as Label;
        Label lblAfterScheDisc = gr.FindControl("lblAfterScheDisc") as Label;
        try
        {
            if (Convert.ToDouble(txtRQty.Text) <= Convert.ToDouble(lblRemQty.Text))
            {
                lblItemRTotal.Text = (Convert.ToDouble(lblRate.Text) * Convert.ToDouble(txtRQty.Text)).ToString("#0.00");
                lblSchDiscount.Text = (Convert.ToDouble(lblActualSchDiscount.Text) * Convert.ToDouble(txtRQty.Text)).ToString("#0.00");
                lblAfterScheDisc.Text = (Convert.ToDouble(lblItemRTotal.Text) - Convert.ToDouble(lblSchDiscount.Text)).ToString("#0.00");
                lblItemRTotal.Enabled = true;
            }
            else
            {
                HelperFunction.MsgBox(this, this.GetType(), "Return Qty cannot be greater than Remaining Qty to return.");
                txtRQty.Text = "";
                txtRQty.Focus();
            }
        }
        catch { }
        getTotal();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divInvoiceDetail.Visible = false;
        divInitial.Visible = true;
    }

    //=-==============================================
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


                    SRMEnt.SUB_TOTAL = lblSubTotalAmount.Text;
                    SRMEnt.DISCOUNT_PERCENT = txtDiscount.Text;
                    SRMEnt.DISCOUNT_AMOUNT = txtDiscountAmount.Text;
                    SRMEnt.TAXABLE_AMOUNT = lblTotalAmount.Text;
                    SRMEnt.TAX_VAT_AMOUNT = lblVAT.Text;
                    SRMEnt.GRAND_TOTAL = lblGrandTotal.Text;
                    SRMEnt.ROUND_OFF = txtRound.Text;
                    SRMEnt.SALES_RETURN_AMOUNT = lblInvoiceAmount.Text;
                    SRMEnt.USER_ID = userProfile.EmployeeID;
                    SRMEnt.RETURN_REMARKS = txtCreditNoteRemarks.Text;
                    SRMEnt.NEXT_CN_ID = lblunique_token.Text;
                    SRMEnt.SALES_RETURN_TYPE = ddlPaymentType.SelectedValue;
                    SRMEnt.OFFICE_CODE = userProfile.LocationID;
                    string SR_PK_ID = SRMSer.Insert(SRMEnt, DT).ToString();
                    #endregion
                    double billSno = 1;
                    #region to insert in sales return detail
                    foreach (GridViewRow gr in grdSalesDetail.Rows)
                    {
                        Label lblProductPK_ID = gr.FindControl("lblProductPK_ID") as Label;
                        Label lblBatch = gr.FindControl("lblBatch") as Label;
                        Label lblExpiryDate = gr.FindControl("lblExpiryDate") as Label;
                        Label lblQty = gr.FindControl("lblQty") as Label;
                        TextBox txtRQty = gr.FindControl("txtRQty") as TextBox;
                        Label lblRate = gr.FindControl("lblRate") as Label;
                        TextBox lblItemRTotal = gr.FindControl("lblItemRTotal") as TextBox;

                        Label lblSchDiscount = gr.FindControl("lblSchDiscount") as Label;
                        Label lblAfterScheDisc = gr.FindControl("lblAfterScheDisc") as Label;
                        if (!string.IsNullOrEmpty(lblItemRTotal.Text))
                        {
                            SRDEnt = new SALES_RETURN_DETAIL();
                            SRDEnt.SALES_RETURN_ID = SR_PK_ID;
                            SRDEnt.SNO = billSno.ToString();
                            SRDEnt.PRODUCT_ID = lblProductPK_ID.Text;
                            SRDEnt.BATCH_NUMBER = lblBatch.Text;
                            SRDEnt.EXPIRY_DATE = lblExpiryDate.Text;

                            SRDEnt.QUANTITY = txtRQty.Text;
                            SRDEnt.RATE = lblRate.Text;
                            SRDEnt.TOTAL = lblItemRTotal.Text;
                            SRDEnt.SCHEME_DISCOUNT = lblSchDiscount.Text;
                            SRDEnt.OFFICE_CODE = userProfile.LocationID;
                            if (ddlExempted.SelectedValue == "1")
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


                            double rqty = 0; // to check if there is return qty or not
                            try
                            {
                                rqty = Convert.ToDouble(txtRQty.Text);
                            }
                            catch { }
                            if (rqty > 0)
                            {
                                SRDSer.Insert(SRDEnt, DT);
                                billSno++;
                            }
                        }

                    }
                    #endregion

                    #region account portion 
                    string invice_number = SIMEnt.INVOICE_NUMBER;


                    #region to insert in Voucher Master CN
                    //If Sales return is in credit to customer just create credit note
                    //If Sales return is in cash to walk in customer just create credit note no need to create JV
                    //If sales return is in cash create credit note and then create Journal Voucher to make cash effect
                    VMEnt = new VOUCHER_MASTER();
                    VMEnt.VOUCHER_TYPE = "CN";
                    VMEnt.VOUCHER_NUMBER = af.getNext_VM_ID("CN", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfile.LocationID);
                    VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.VOUCHER_DAY = PGD.NepaliDay();
                    VMEnt.VOUCHER_MONTH = PGD.NepaliMonth();
                    VMEnt.VOUCHER_YEAR = PGD.NepaliYear();
                    VMEnt.VOUCHER_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                    VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VMEnt.TRN_AMOUNT = lblInvoiceAmount.Text;
                    VMEnt.NARRATION = "Sales Return from Invoice no." + invice_number + ". " + txtCreditNoteRemarks.Text;
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
                    foreach (GridViewRow gvr in grdSalesDetail.Rows)
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
                    VCEnt.DR_AMOUNT = lblVAT.Text;
                    VCEnt.CR_AMOUNT = "0";
                    VCEnt.REMARKS = "By";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    if (lblVAT.Text != "0.00")
                    {
                        VCSer.Insert(VCEnt, DT);
                        sno++;
                    }
                    #endregion

                    #region for Round Off; if round off is +ve 
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

                    if (txtCustomerCode.Text == "") // walking in customer ko lagi sadai cash nai huncha
                    {
                        VCEnt.GL_CODE = "010102"; //GL_CODE of Cash
                    }
                    else// customer ko lagi cash return baye pani customer ko ma effect dekhauna parni bhaye ko le 
                    // customer ko code nai patahu ni , pachi JV banaye ra cash effect dine
                    {
                        VCEnt.GL_CODE = "010301"; //GL_CODE of Sundry Debitors Customer                       
                        VCEnt.SGL_CODE = txtCustomerCode.Text;
                    }
                    VCEnt.DR_AMOUNT = "0";
                    VCEnt.CR_AMOUNT = lblInvoiceAmount.Text;
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
                    VCEnt.CR_AMOUNT = txtDiscountAmount.Text;
                    VCEnt.REMARKS = "To";
                    VCEnt.OFFICE_CODE = userProfile.LocationID;
                    if (txtDiscountAmount.Text != "0.00") /// discount cha bhaye matra insert garni
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

                    #region to insert in Voucher Master JV
                    //If sales return is in cash create credit note and then create Payment Voucher(DV) to make cash effect
                    if (txtCustomerCode.Text != "" && ddlCNType.SelectedValue == "CS")
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
                        VMEnt.NARRATION = "Cash Return for sales return, Invoice no." + invice_number + ". " + txtCreditNoteRemarks.Text;
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
                        VCEnt.SGL_CODE = txtCustomerCode.Text;
                        VCEnt.DR_AMOUNT = lblInvoiceAmount.Text;
                        VCEnt.CR_AMOUNT = "0";
                        VCEnt.REMARKS = "By";
                        VCEnt.OFFICE_CODE = userProfile.LocationID;
                        VCSer.Insert(VCEnt, DT);
                        #endregion

                        #region for Cr Part
                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = new_voucher_pk_id;
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
                                        ref_invoice_number = txtInvoiceNumber.Text,
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
                                        // yo line le APL ko SSL certificate expire bhaye pani kam garni huncha 
                                        // yo first line 1 ta bhaye na bhane SSL certificate expire bhayo bhana API le kam gardaina
                                        //  ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

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

    #region to print credit note
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
            lblCNVATPercent.Text = Convert.ToDouble(PG.CompanyTAXPercent()).ToString("0.00");
            lblCNVATAmount.Text = Convert.ToDouble(SRMEnt.TAX_VAT_AMOUNT).ToString("0.00");
            lblCNGrandTotal.Text = Convert.ToDouble(SRMEnt.GRAND_TOTAL).ToString("0.00");
            lblCNRoundoff.Text = SRMEnt.ROUND_OFF;
            lblCNInvoiceAmount.Text = Convert.ToDouble(SRMEnt.SALES_RETURN_AMOUNT).ToString("0.00");
            lblCNAmounInWords.Text = hf.NumWordsWrapper(Convert.ToDouble(SRMEnt.SALES_RETURN_AMOUNT)) + " Only";
            lblCNRemarks.Text = SRMEnt.RETURN_REMARKS;
            lblInvCreatedBy.Text = SRMEnt.USER_ID;
            printdetail.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
        }

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
        cleardata();
    }
    // btnClear and btn Cancel is similar only diference is unique token
    // btn cance dont need to reset unique token
    protected void btnCancel_Click1(object sender, EventArgs e)
    {
        divInitial.Visible = true;
        divInvoiceDetail.Visible = false;
        divButton.Visible = false;
        printdetail.Visible = false;
        LoadInvoiceGrid();
        cleardata();
    }

    protected void getTotal()
    {
        double total = 0;
        double discount_percent = 0;
        double discount = 0;
        double beforedecimal = 0;
        double afterdecimal = 0;
        foreach (GridViewRow gr in grdSalesDetail.Rows)
        {
            TextBox lblItemRTotal = gr.FindControl("lblItemRTotal") as TextBox;
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
        lblTotalAmount.Text = (total - discount).ToString("#0.00");
        double vat = Convert.ToDouble(txtVATPercent.Text) / 100;
        lblVAT.Text = ((total - discount) * vat).ToString("#0.00");
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
    protected string getUnitName(string PK_ID)
    {
        string unit_name = "";
        PUEnt = new PRODUCT_UNIT();
        PUEnt.PK_ID = PK_ID;
        PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
        if (PUEnt != null)
            unit_name = PUEnt.UNIT_NAME;
        return unit_name;
    }
    protected void ddlCNType_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckCreditReturn();
    }
    protected void CheckCreditReturn()
    {
        if (ddlCNType.SelectedValue == "CR")
        {
            if (txtCustomerCode.Text == "")
            {
                HelperFunction.MsgBox(this, this.GetType(), "Credit return cannot be perform for walk in customer.");
                ddlCNType.SelectedValue = "CS";
            }
        }
    }
    protected void SetVisibility()
    {
        trInvRoundOff.Visible = PGPS.RoundOff();// whether or not to show round off on inv
        trInvRoTot.Visible = PGPS.RoundOff();
        gridSalesRetInvoice.Columns[2].Visible = PGPS.ShowDualQuantity();//Upper Qty 
        if (PG.CompanyTAXType() != "VAT")//whether or not to show vat on bill
        {
            trInvVats.Visible = false;

        }


        grdSalesDetail.Columns[11].Visible = PGPS.ItemWiseDiscount(); // Item wise discount
        grdSalesDetail.Columns[12].Visible = PGPS.ItemWiseDiscount(); // Am
    }

    protected void lblItemRTotal_TextChanged(object sender, EventArgs e)
    {
        getTotal();
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
}


