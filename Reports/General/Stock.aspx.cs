using System;
using Entity.Components;
using Service.Components;
using System.Web.UI;
using PhyeGanCore;
using System.Web.UI.WebControls;
using System.Web;

public partial class Reports_General_Stock : System.Web.UI.Page
{
    PRODUCT_CATEGORY PCEnt = new PRODUCT_CATEGORY();
    PRODUCT_CATEGORYService PCSer = new PRODUCT_CATEGORYService();

    PRODUCT_SUB_CATEGORY PSCEnt = new PRODUCT_SUB_CATEGORY();
    PRODUCT_SUB_CATEGORYService PSCSer = new PRODUCT_SUB_CATEGORYService();

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();

    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

    PhyeGan PG = new PhyeGan();

    HelperFunction hf = new HelperFunction();
    PhyeGanDate PGD = new PhyeGanDate();

    Boolean IsPageRefresh = false;

    UserProfileEntity userProfileEnt = new UserProfileEntity();

    static string path = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["postids"] = System.Guid.NewGuid().ToString();
                Session["postid"] = ViewState["postids"].ToString();

                userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");

                if (hf.checkPageAccess(path, userProfileEnt.UserGroupID.ToString()))
                {
                    loadBranch();
                    LoadProductCategory();
                    LoadProductSubCategory();
                    LoadProduct();
                    LoadFiscalYear();
                }
                else
                {
                    Response.Redirect("~/forbidden.aspx");
                }
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
        catch (System.Threading.ThreadAbortException)
        {
            Response.Redirect("~/forbidden.aspx");
        }
        catch
        {
            Response.Redirect("~/Login.aspx");
        }
    }
    protected void loadBranch()
    {
        ddlBranch.DataSource = PG.getBranchList();
        ddlBranch.DataTextField = "OFFICENAME";
        ddlBranch.DataValueField = "PK_ID";
        ddlBranch.DataBind();

        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (userProfileEnt.LocationTypeID == 1 && PG.checkBranchAccess(path, userProfileEnt.UserGroupID.ToString()) && PG.CompanyBranch_Status())
        {
            divBranch.Visible = true;
            ddlBranch.Items.Insert(0, "-");
        }
        else
        {
            divBranch.Visible = false;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
        }
    }

    protected void LoadCompany()
    {
        lblCompanyName.Text = PG.CompanyName();
        if (ddlBranch.SelectedValue != "")
        {
            lblAddress.Text = PG.BranchAddress(ddlBranch.SelectedValue);
            lblContact.Text = PG.BranchContact(ddlBranch.SelectedValue);
        }
        else
        {
            lblAddress.Text = "";
            lblContact.Text = "";
        }

    }
    protected void LoadProductCategory()
    {
        PCEnt = new PRODUCT_CATEGORY();
        PCEnt.STATUS = "1";
        ddlCategoryFilter.DataSource = PCSer.GetAll(PCEnt);
        ddlCategoryFilter.DataTextField = "CATEGORY_NAME";
        ddlCategoryFilter.DataValueField = "PK_ID";
        ddlCategoryFilter.DataBind();
        ddlCategoryFilter.Items.Insert(0, "Select");

    }
    protected void LoadProductSubCategory()
    {
        PSCEnt = new PRODUCT_SUB_CATEGORY();
        PSCEnt.CATEGORY_ID = ddlCategoryFilter.SelectedValue;
        PSCEnt.STATUS = "1";
        ddlSubCategoryFilter.DataSource = PSCSer.GetAll(PSCEnt);
        ddlSubCategoryFilter.DataTextField = "SUB_CATEGORY_NAME";
        ddlSubCategoryFilter.DataValueField = "PK_ID";
        ddlSubCategoryFilter.DataBind();
        ddlSubCategoryFilter.Items.Insert(0, "Select");
    }
    protected void LoadProduct()
    {
        PEnt = new PRODUCT();
        PEnt.CATEGORY_ID = ddlCategoryFilter.SelectedValue;
        PEnt.OFFICE_CODE = ddlBranch.SelectedValue;
        if (ddlSubCategoryFilter.SelectedValue != "Select")
            PEnt.SUB_CATEGORY_ID = ddlSubCategoryFilter.SelectedValue;
        else
            PEnt.SUB_CATEGORY_ID = "0";
        ddlProductFilter.DataSource = PSer.GetAll(PEnt);
        ddlProductFilter.DataValueField = "PRODUCT_CODE";
        ddlProductFilter.DataTextField = "PRODUCT_NAME";
        ddlProductFilter.DataBind();
        ddlProductFilter.Items.Insert(0, "Select");
    }

    protected void LoadFiscalYear()
    {
        FYEnt = new FISCALYEAR();
        ddlFiscalYear.DataSource = FYSer.GetAll(FYEnt);
        ddlFiscalYear.DataValueField = "FISCAL_YEAR";
        ddlFiscalYear.DataTextField = "FISCAL_YEAR";
        ddlFiscalYear.DataBind();
        ddlFiscalYear.SelectedValue = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
    }
    protected void ddlCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductSubCategory();
        LoadProduct();
    }
    protected void ddlSubCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProduct();
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        LoadStock();
        divhide.Visible = true;
    }
    protected void LoadStock()
    {
        LoadCompany();
        string office_code = "";
        if (ddlBranch.SelectedValue != "-")
        {
            office_code = ddlBranch.SelectedValue;
        }
        string category = ddlCategoryFilter.SelectedValue == "Select" ? null : ddlCategoryFilter.SelectedValue;
        string subcategory = ddlSubCategoryFilter.SelectedValue == "Select" ? "0" : ddlSubCategoryFilter.SelectedValue;
        string product = ddlProductFilter.SelectedValue == "Select" ? null : ddlProductFilter.SelectedValue;

        if (category == null && subcategory == "0" && product == null)
        {
            grdStock.DataSource = hf.getOpeningClosingBalance(ddlFiscalYear.SelectedValue, null, null, null, null, null, null, null, office_code);
            grdStock.DataBind();
        }
        else if ((category != null && subcategory == "0" && product == null))
        {
            grdStock.DataSource = hf.getOpeningClosingBalance(ddlFiscalYear.SelectedValue, category, null, null, null, null, null, null, office_code);
            grdStock.DataBind();
        }
        else
        {
            grdStock.DataSource = hf.getOpeningClosingBalance(ddlFiscalYear.SelectedValue, category, subcategory, product, null, null, null, null, office_code);
            grdStock.DataBind();
        }


        if (hf.ProductBatch() == "1")
            grdStock.Columns[2].Visible = true;
        else
            grdStock.Columns[2].Visible = false;

        if (hf.ProductExpiry() == "1")
            grdStock.Columns[3].Visible = true;
        else
            grdStock.Columns[3].Visible = false;

        if (hf.ProductColor() == "1")
            grdStock.Columns[4].Visible = true;
        else
            grdStock.Columns[4].Visible = false;

        if (hf.ProductSize() == "1")
            grdStock.Columns[5].Visible = true;
        else
            grdStock.Columns[5].Visible = false;

        if (hf.ProductManufacturer() == "1")
            grdStock.Columns[6].Visible = true;
        else
            grdStock.Columns[6].Visible = false;
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void lblProduct_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((LinkButton)sender).Parent.Parent as GridViewRow;

        LinkButton lblProduct = gr.FindControl("lblProduct") as LinkButton;
        Label lblFiscalYear = gr.FindControl("lblFiscalYear") as Label;
        Label lblProduct_id = gr.FindControl("lblProduct_id") as Label;
        Label lblBatchNo = gr.FindControl("lblBatchNo") as Label;
        // Correct query string format using & for additional parameters
        string url = "~/Account/Reports/Monthly_Summary.aspx?productid=" + lblProduct_id.Text + "&fy=" + lblFiscalYear.Text + "&bat=" + lblBatchNo.Text;
        string fullUrl = ResolveUrl(url);

        // Register JavaScript to open the URL in a new tab
        string script = "window.open('" + fullUrl + "', '_blank');";
        ScriptManager.RegisterStartupScript(this, GetType(), "OpenInvoice", script, true);
    }

    decimal purchase, purchase_amt, purchase_ret, purchase_ret_amt, sales, sales_amt, sales_return, sales_return_amt, adjustment, closing_qty,
       clo_amt, opening_qty, opening_amt;
    decimal total_purchase, total_purchase_amt, total_purchase_ret, total_purchase_ret_amt, total_sales, total_sales_amt, total_sales_return,
        total_sales_return_amt, total_adjustment, total_closing_qty, total_clo_amt, total_opening_qty, total_opening_amt;
    decimal totalOpening = 0, totalPurchase = 0, totalPurchaseReturn = 0, totalSales = 0, totalSalesReturn = 0;
    decimal totalAdjustment = 0, totalClosing = 0, totalClosingBalance = 0;

    protected void grdStock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal purchaseAmount = GetDecimalFromLabel(e.Row, "lblPurchaseAmount");
            decimal purchaseQty = GetDecimalFromLabel(e.Row, "lblPurchase");
            decimal OpeningQty = GetDecimalFromLabel(e.Row, "lblOpening");
            decimal OpeningAmt = GetDecimalFromLabel(e.Row, "lblOpeningBalance");
            decimal closingQty = GetDecimalFromLabel(e.Row, "lblClosing");

            decimal closingValue = 0;
            if (closingQty > 0)
            {
                if (purchaseQty != 0)
                {
                    closingValue = (purchaseAmount / purchaseQty) * closingQty;
                }
                else if (OpeningQty != 0)
                {
                    closingValue = (OpeningAmt / OpeningQty) * closingQty;
                }
                else
                {
                    closingValue = GetDecimalFromLabel(e.Row, "lblClosingBalance");
                }

            }

            totalOpening += GetDecimalFromLabel(e.Row, "lblOpening");
            total_opening_amt += GetDecimalFromLabel(e.Row, "lblOpeningBalance");

            totalPurchase += purchaseQty;
            total_purchase_amt += purchaseAmount;
            totalPurchaseReturn += GetDecimalFromLabel(e.Row, "lblPurchaseReturn");
            total_purchase_ret_amt += GetDecimalFromLabel(e.Row, "lblPurchaseReturnAmount");
            totalSales += GetDecimalFromLabel(e.Row, "lblSales");
            total_sales_amt += GetDecimalFromLabel(e.Row, "lblSalesAmount");
            totalSalesReturn += GetDecimalFromLabel(e.Row, "lblSalesReturn");
            total_sales_return_amt += GetDecimalFromLabel(e.Row, "lblSalesReturnAmount");
            totalAdjustment += GetDecimalFromLabel(e.Row, "lblAdjustment");
            totalClosing += closingQty;   // add actual closing quantity
            // add calculated closing value
            totalClosingBalance += closingValue; // add calculated closing value

            Label lblClosing = (Label)e.Row.FindControl("lblClosing");
            if (lblClosing != null)
                lblClosing.Text = closingQty < 0 ? "(" + Math.Abs(closingQty).ToString("N2") + ")" : closingQty.ToString("N2");

            Label lblClosingBalance = (Label)e.Row.FindControl("lblClosingBalance");
            if (closingQty <= 0)
            {
                lblClosingBalance.Text = "0.00";
            }
            else
            {
                if (lblClosingBalance != null)
                    lblClosingBalance.Text = Math.Abs(closingValue).ToString("N2");
            }
        }


        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            SetLabelText(e.Row, "lblOpeningF", totalOpening);
            SetLabelText(e.Row, "lblPurchaseF", totalPurchase);
            SetLabelText(e.Row, "lblPurchaseReturnF", totalPurchaseReturn);
            SetLabelText(e.Row, "lblSalesF", totalSales);
            SetLabelText(e.Row, "lblSalesReturnF", totalSalesReturn);
            SetLabelText(e.Row, "lblAdjustmentF", totalAdjustment);
            SetLabelText(e.Row, "lblPurchaseAmountF", total_purchase_amt);
            SetLabelText(e.Row, "lblSalesAmountF", total_sales_amt);
            SetLabelText(e.Row, "lblSalesReturnAmtF", total_sales_return_amt);
            SetLabelText(e.Row, "lblOpeningBalanceF", total_opening_amt);
            SetLabelText(e.Row, "lblPurchaseReturnAmtF", total_purchase_ret_amt);
            SetLabelText(e.Row, "lblCloQtyF", totalClosing);
            SetLabelText(e.Row, "lblCloValF", totalClosingBalance);
        }
    }

    private decimal GetDecimalFromLabel(GridViewRow row, string labelId)
    {
        Label lbl = row.FindControl(labelId) as Label;
        decimal result = 0;

        if (lbl != null)
        {
            string rawText = lbl.Text.Replace(",", "").Trim();

            if (!string.IsNullOrWhiteSpace(rawText) && decimal.TryParse(rawText, out result))
            {
                return result;
            }
        }

        return 0;
    }



    private void SetLabelText(GridViewRow row, string labelId, decimal value)
    {
        Label lbl = row.FindControl(labelId) as Label;
        if (lbl != null)
        {
            lbl.Text = value.ToString("N2");
        }
    }



    protected void grdStock_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow secondHeader = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);

            // First two columns (S.N and Particulars)
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "S.N";
            cell.RowSpan = 1; // Span across both header rows
            secondHeader.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Particulars";
            cell.RowSpan = 1;
            secondHeader.Cells.Add(cell);

            if (PGPS.ProductBatch())
            {
                // Inward Section (Merging two columns: Quantity & Value)
                cell = new TableHeaderCell();
                cell.Text = "Batch Number";
                cell.ColumnSpan = 1; // Covers Inward Quantity & Inward Value
                cell.HorizontalAlign = HorizontalAlign.Center;
                secondHeader.Cells.Add(cell);
            }
            if (PGPS.ProductExpDate())
            {
                // Inward Section (Merging two columns: Quantity & Value)
                cell = new TableHeaderCell();
                cell.Text = "Expiry Date";
                cell.ColumnSpan = 1; // Covers Inward Quantity & Inward Value
                cell.HorizontalAlign = HorizontalAlign.Center;
                secondHeader.Cells.Add(cell);
            }
            if (PGPS.ProductColor())
            {
                // Inward Section (Merging two columns: Quantity & Value)
                cell = new TableHeaderCell();
                cell.Text = "Color";
                cell.ColumnSpan = 1; // Covers Inward Quantity & Inward Value
                cell.HorizontalAlign = HorizontalAlign.Center;
                secondHeader.Cells.Add(cell);
            }
            if (PGPS.ProductSize())
            {
                // Inward Section (Merging two columns: Quantity & Value)
                cell = new TableHeaderCell();
                cell.Text = "Size";
                cell.ColumnSpan = 1; // Covers Inward Quantity & Inward Value
                cell.HorizontalAlign = HorizontalAlign.Center;
                secondHeader.Cells.Add(cell);
            }
            if (PGPS.ProductManufacture())
            {
                // Inward Section (Merging two columns: Quantity & Value)
                cell = new TableHeaderCell();
                cell.Text = "Manufacturer";
                cell.ColumnSpan = 1;
                // Covers Inward Quantity & Inward Value
                cell.HorizontalAlign = HorizontalAlign.Center;
                secondHeader.Cells.Add(cell);
            }
            // Inward Section (Merging two columns: Quantity & Value)
            cell = new TableHeaderCell();
            cell.Text = "Opening";
            cell.ColumnSpan = 2; // Covers Inward Quantity & Inward Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Purchase";
            cell.ColumnSpan = 2; // Covers Closing Quantity & Closing Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);
            // Outward Section (Merging two columns: Quantity & Value)
            cell = new TableHeaderCell();
            cell.Text = "Purchase Return";
            cell.ColumnSpan = 2; // Covers Outward Quantity & Outward Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);

            // Outward Section (Merging two columns: Quantity & Value)
            cell = new TableHeaderCell();
            cell.Text = "Sales";
            cell.ColumnSpan = 2; // Covers Outward Quantity & Outward Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);// Outward Section (Merging two columns: Quantity & Value)

            cell = new TableHeaderCell();
            cell.Text = "Sales Return";
            cell.ColumnSpan = 2; // Covers Outward Quantity & Outward Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);// Outward Section (Merging two columns: Quantity & Value)

            cell = new TableHeaderCell();
            cell.Text = "Adjustment";
            cell.ColumnSpan = 1; // Covers Outward Quantity & Outward Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);// Outward Section (Merging two columns: Quantity & Value)

            cell = new TableHeaderCell();
            cell.Text = "Closing";
            cell.ColumnSpan = 2; // Covers Outward Quantity & Outward Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);


            // Add the new header row to the GridView
            grdStock.Controls[0].Controls.AddAt(0, secondHeader);
        }
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProduct();
    }
}