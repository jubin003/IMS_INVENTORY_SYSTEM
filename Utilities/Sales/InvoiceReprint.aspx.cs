using Entity.Components;
using PhyeGanCore;
using Service.Components;
using System;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Utilities_Sales_InvoiceReprint : System.Web.UI.Page
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

    AGENT AEnt = new AGENT();
    AGENTService ASer = new AGENTService();

    AREA AREAEnt = new AREA();
    AREAService AREASer = new AREAService();

    NAME_COMPANY NEnt = new NAME_COMPANY();
    NAME_COMPANYService NSer = new NAME_COMPANYService();

    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfile = new UserProfileEntity();
    Boolean IsPageRefresh = false;
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
                    txtFromDate.Text = PGD.GetTodayNepaliDate();
                    txtToDate.Text = PGD.GetTodayNepaliDate();
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
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        string invoice_no = "";
        try
        {
            invoice_no = Convert.ToDouble(txtInvoiceNo.Text).ToString("00000000");
        }
        catch { }
        if (chkDateWise.Checked == true)
        {

            grdSales.DataSource = hf.getSalesList("", invoice_no, PGD.GetEnglishDateFromNepali(txtFromDate.Text, "dd/mm/yyyy"), PGD.GetEnglishDateFromNepali(txtToDate.Text, "dd/mm/yyyy"), userProfile.LocationID);
        }
        else
        {
            grdSales.DataSource = hf.getSalesList(ddlFiscalYear.SelectedValue, txtInvoiceNo.Text, "", "", userProfile.LocationID);
        }

        grdSales.DataBind();
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
            userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            SIMEnt = new SALES_INVOICE_MASTER();
            SIMEnt.PK_ID = lblPK_ID.Text;
            SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
            if (SIMEnt != null)
            {
                SIMEnt.PRINT_TIME = System.DateTime.Now.ToString();
                SIMEnt.PRINT_BY = userProfile.EmployeeID;
                SIMEnt.COPY_PRINTNO = (Convert.ToDouble(SIMEnt.COPY_PRINTNO) + 1).ToString();
                SIMSer.Update(SIMEnt);
                LoadToPrint(lblPK_ID.Text);
            }
        }
        else if (e.CommandName.Equals("TOK"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
            userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

            SIMEnt = new SALES_INVOICE_MASTER();
            SIMEnt.PK_ID = lblPK_ID.Text;
            SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);

            if (SIMEnt != null)
            {
                LoadToTOKPrint(lblPK_ID.Text);
            }
        }
    }

    #region to print bill

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
        lblPhone1.Text = PG.BranchContact(userProfile.LocationID);

        //new 80mm
        lblCompanyName80.Text = PG.CompanyName();
        lblCompanyAddress80.Text = PG.BranchAddress(userProfile.LocationID);
        lblPhone80.Text = PG.BranchContact(userProfile.LocationID);
        lblPanNo80.Text = PG.CompanyVATPan();

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

        lblInvVATReturn.Text = "";
        trInvVATReturn.Visible = false;
        lblVATReturn80.Text = "";
        trVATReturn80.Visible = false;
    }

    protected void LoadToTOKPrint(string pk_id)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.PK_ID = pk_id;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);

        if (SIMEnt != null)
        {
            lblBillNoTOK.Text = SIMEnt.INVOICE_NUMBER;
            lblBillDateTOK.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
            lblCompanyNameTOK.Text = PG.CompanyName();
            lblCompanyAddressTOK.Text = PG.BranchAddress(userProfile.LocationID);
            lblPhoneTOK.Text = PG.BranchContact(userProfile.LocationID);
            lblPanNoTOK.Text = PG.CompanyVATPan();
            lblPrintedByTOK.Text = hf.getEmployeeName(userProfile.EmployeeID);
            lblTimeTOK.Text = System.DateTime.Now.ToString();
            lblNameTOK.Text = SIMEnt.CUSTOMER_NAME;

            DataTable dt = hf.LoadSalesInvoice(pk_id);
            gridTOK.DataSource = dt;
            gridTOK.DataBind();

            printdetailTOK.Visible = true;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "tokprint", "printTOK();", true);

        }
    }
    protected void LoadToPrint(string pk_id)
    {
        cleardata();
        LoadCompanyDetail();
        if (PG.InvoiceHeading() == "TAX INVOICE")
            lblInvoiceHeading.Text = "INVOICE";
        else
            lblInvoiceHeading.Text = PG.InvoiceHeading();
        lblInvoiceHeading1.Text = "Copy of Original: " + SIMEnt.COPY_PRINTNO;
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
                lblRemarks.Text = "* " + SIMEnt.REMARKS;


            // ---- NEW 80mm labels ----
            //lblOrderNo80.Text = GetOrderNumberByInvoice(SIMEnt.PK_ID);
            if (PG.InvoiceHeading() == "TAX INVOICE")
                lblInvoiceHeading80.Text = "INVOICE";
            else
                lblInvoiceHeading80.Text = PG.InvoiceHeading();
            lblInvoiceHeading180.Text = "Copy of Original: " + SIMEnt.COPY_PRINTNO;

            lblInvoiceNo80.Text = SIMEnt.INVOICE_NUMBER;
            lblBillNepaliDate80.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
            lblBillEnglishDate80.Text = SIMEnt.INVOICE_DATE;
            lblTranNepaliDate80.Text = SIMEnt.TRANSACTION_DAY + "/" + SIMEnt.TRANSACTION_MONTH + "/" + SIMEnt.TRANSACTION_YEAR;
            lblTranDate80.Text = SIMEnt.TRANSACTION_DATE;

            // Customer details
            lblCustomerName80.Text = SIMEnt.CUSTOMER_NAME;
            //lblAddress80.Text = SIMEnt.CUSTOMER_ADDRESS;
            lblCustomerPanNo80.Text = SIMEnt.COSTOMER_PAN_VAT;
            lblModeofPayment80.Text = hf.getPaymentType(SIMEnt.SALES_TYPE_ID);

            // Totals
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







            LoadSalesGrid(SIMEnt.PK_ID);
            //new 80mm
            LoadSalesGrid80(SIMEnt.PK_ID);

            SetGridColumnVisibility();


            // Decide layout based on company's configured bill type
            NEnt = new NAME_COMPANY();
            NEnt = (NAME_COMPANY)NSer.GetSingle(NEnt);
            string billType = (NEnt != null && !string.IsNullOrEmpty(NEnt.INVOICE_TYPE)) ? NEnt.INVOICE_TYPE : "A5L";

            if (billType == "Continuous")
            {
                printdetail.Visible = false;
                printdetail80.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
            }
            else
            {
                printdetail.Visible = true;
                printdetail80.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printA4Only();", true);
            }
        }
    }

    //new80mm
    protected void LoadSalesGrid80(string PK_id)
    {
        DataTable dt = hf.LoadSalesInvoice(PK_id);
        gridSalesInvoice80.DataSource = dt;
        gridSalesInvoice80.DataBind();
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
    //protected void gridSalesInvoice80_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        GridView gv = (GridView)sender;
    //        GridViewRow separatorRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
    //        TableCell cell = new TableCell();
    //        cell.ColumnSpan = gv.Columns.Count;
    //        cell.Text = "______________________________________";
    //        cell.Style["padding"] = "0";
    //        cell.Style["font-family"] = "Courier New, monospace";
    //        cell.Style["font-size"] = "11px";
    //        separatorRow.Cells.Add(cell);
    //        e.Row.Cells[0].ColumnSpan = 0;

    //        // Use PreRender instead
    //        gv.PreRender += (s, args) =>
    //        {
    //            Table tbl = (Table)gv.Controls[0];
    //            tbl.Rows.AddAt(1, separatorRow);
    //        };
    //    }
    //}
    #endregion
    protected void LoadSalesGrid(string PK_id)
    {
        DataTable dt = hf.LoadSalesInvoice(PK_id);
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
        gridSalesInvoice.Columns[2].Visible = PGPS.ProductBatch();       
        gridSalesInvoice.Columns[3].Visible = PGPS.ProductExpDate();     
        gridSalesInvoice.Columns[4].Visible = PGPS.ShowDualQuantity();   
        gridSalesInvoice.Columns[8].Visible = PGPS.ItemWiseDiscount();   
        gridSalesInvoice.Columns[9].Visible = PGPS.ItemWiseDiscount();   

        if (PG.CompanyTAXType() != "VAT")
        {
            trInvVat.Visible = false;
            trVat80.Visible = false;
            trInvVATReturn.Visible = false;
            trVATReturn80.Visible = false;
        }

        trInvRoundOff.Visible = PGPS.RoundOff();

        divPO.Visible = PGPS.ShowPO();
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