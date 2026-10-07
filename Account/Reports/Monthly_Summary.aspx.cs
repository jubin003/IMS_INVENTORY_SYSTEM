using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;

public partial class Account_Reports_Monthly_Summary : System.Web.UI.Page
{
    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    PRODUCT_CATEGORY PCEnt = new PRODUCT_CATEGORY();
    PRODUCT_CATEGORYService PCSer = new PRODUCT_CATEGORYService();

    PRODUCT_SUB_CATEGORY PSCEnt = new PRODUCT_SUB_CATEGORY();
    PRODUCT_SUB_CATEGORYService PSCSer = new PRODUCT_SUB_CATEGORYService();

    AccountFunction AF = new AccountFunction();
    PhyeGan PG = new PhyeGan();
    string fiscal_year, product_id, batch;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            SetFiscalYear();
        {
            fiscal_year = Request.QueryString["fy"].ToString();
            product_id = Request.QueryString["productid"].ToString();
            batch = Request.QueryString["bat"].ToString();

            if (fiscal_year != "" && product_id != "")
            {
                if(batch ==null ){
                    batch = "";
                }
                gridMonthlyProductSummary.DataSource = AF.getMonthlyProductSummary(fiscal_year, product_id, batch);
                gridMonthlyProductSummary.DataBind();
                divToPrint.Visible = true;
                lblCompanyName.Text = PG.CompanyName();
                lblAddress.Text = PG.CompanyAddress();
                lblContact.Text = PG.CompanyContact();
                lblFyDate.Text = fiscal_year;
                PEnt = new PRODUCT();
                PEnt.PK_ID = product_id;
                PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                if (PEnt != null)
                {
                    lblProduct.Text = PEnt.PRODUCT_NAME;
                }
            }
        }
    }

    double inward_qty, inward_val, outward_qty, out_ward_val, closing_qty, closing_val;
    double total_inward_qty, total_inward_val, total_outward_qty, total_out_ward_val, total_closing_qty, total_closing_val;
    double previousCloQty = 0;
    double previousCloVal = 0;
    protected void gridMonthlyProductSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblInQtyF = e.Row.FindControl("lblInQtyF") as Label;
            Label lblInValF = e.Row.FindControl("lblInValF") as Label;
            Label lblOutQtyF = e.Row.FindControl("lblOutQtyF") as Label;
            Label lblOutValF = e.Row.FindControl("lblOutValF") as Label;
            Label lblCloQtyF = e.Row.FindControl("lblCloQtyF") as Label;
            Label lblCloValF = e.Row.FindControl("lblCloValF") as Label;

            double total_inward_qty = 0, total_inward_val = 0;
            double total_outward_qty = 0, total_outward_val = 0;

            double last_closing_qty = 0, last_closing_val = 0; // Will hold last row's values

            int rowCount = gridMonthlyProductSummary.Rows.Count;

            for (int i = 0; i < rowCount; i++)
            {
                GridViewRow gr = gridMonthlyProductSummary.Rows[i];

                Label lblInQty = gr.FindControl("lblInQty") as Label;
                Label lblInVal = gr.FindControl("lblInVal") as Label;
                Label lblOutQty = gr.FindControl("lblOutQty") as Label;
                Label lblOutVal = gr.FindControl("lblOutVal") as Label;
                Label lblCloQty = gr.FindControl("lblCloQty") as Label;
                Label lblCloVal = gr.FindControl("lblCloVal") as Label;

                double inward_qty = string.IsNullOrEmpty(lblInQty.Text) ? 0 : Convert.ToDouble(lblInQty.Text);
                double inward_val = string.IsNullOrEmpty(lblInVal.Text) ? 0 : Convert.ToDouble(lblInVal.Text);

                double outward_qty = string.IsNullOrEmpty(lblOutQty.Text) ? 0 : Convert.ToDouble(lblOutQty.Text);
                double outward_val = string.IsNullOrEmpty(lblOutVal.Text) ? 0 : Convert.ToDouble(lblOutVal.Text);

                // Sum totals
                total_inward_qty += inward_qty;
                total_inward_val += inward_val;
                total_outward_qty += outward_qty;
                total_outward_val += outward_val;

                // Save last row's closing values
                if (i == rowCount - 1)
                {
                    last_closing_qty = string.IsNullOrEmpty(lblCloQty.Text) ? 0 : Convert.ToDouble(lblCloQty.Text);
                    last_closing_val = string.IsNullOrEmpty(lblCloVal.Text) ? 0 : Convert.ToDouble(lblCloVal.Text);
                }
            }

            // Set footer totals
            lblInQtyF.Text = total_inward_qty.ToString("#0.00");
            lblInValF.Text = total_inward_val.ToString("#0.00");
            lblOutQtyF.Text = total_outward_qty.ToString("#0.00");
            lblOutValF.Text = total_outward_val.ToString("#0.00");

            // Set last row's closing values in the footer
            lblCloQtyF.Text = last_closing_qty.ToString("#0.00");
            lblCloValF.Text = last_closing_val.ToString("#0.00");


        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblInQty = e.Row.FindControl("lblInQty") as Label;
            Label lblInVal = e.Row.FindControl("lblInVal") as Label;
            Label lblOutQty = e.Row.FindControl("lblOutQty") as Label;
            Label lblOutVal = e.Row.FindControl("lblOutVal") as Label;
            Label lblCloQty = e.Row.FindControl("lblCloQty") as Label;
            Label lblCloVal = e.Row.FindControl("lblCloVal") as Label;
            LinkButton lblParticulars = (LinkButton)e.Row.FindControl("lblParticulars");

            // Default to zero if empty
            double inQty = string.IsNullOrEmpty(lblInQty.Text) ? 0 : Convert.ToDouble(lblInQty.Text);
            double inVal = string.IsNullOrEmpty(lblInVal.Text) ? 0 : Convert.ToDouble(lblInVal.Text);
            double outQty = string.IsNullOrEmpty(lblOutQty.Text) ? 0 : Convert.ToDouble(lblOutQty.Text);
            double outVal = string.IsNullOrEmpty(lblOutVal.Text) ? 0 : Convert.ToDouble(lblOutVal.Text);
            double CloQty = string.IsNullOrEmpty(lblCloQty.Text) ? 0 : Convert.ToDouble(lblCloQty.Text);
            double CloVal = string.IsNullOrEmpty(lblCloVal.Text) ? 0 : Convert.ToDouble(lblCloVal.Text);

            if (e.Row.RowIndex == 0) // First row
            {
                if (lblParticulars.Text.Trim().Equals("Opening Balance", StringComparison.OrdinalIgnoreCase))
                {
                    if (lblCloQty.Text.Trim() == "0" && lblCloVal.Text.Trim() == "0")
                    {
                        e.Row.Visible = false; // Hide the row
                    }
                    else
                    {// Execute for Opening Balance
                        previousCloQty = Convert.ToDouble(lblCloQty.Text);
                        previousCloVal = Convert.ToDouble(lblCloVal.Text);
                    }

                }

            }

            else
            {
                double cloqty;
                double cloval;
                if (previousCloQty == 0)
                {
                    cloqty = previousCloQty + inQty - outQty;
                    cloval = previousCloVal + inVal - outVal;

                    // Assign calculated values to the labels
                    lblCloQty.Text = cloqty.ToString();
                    lblCloVal.Text = cloval.ToString();

                    // Update for the next row processing
                    previousCloQty = cloqty;
                    previousCloVal = cloval;
                }
                else
                {
                    cloqty = previousCloQty + inQty - outQty;
                    cloval = previousCloVal + inVal - outVal;

                    //cloqty = previousCloQty - outQty;
                    //cloval = previousCloVal - outVal;

                    // Assign calculated values to the labels
                    if (cloqty != 0)
                    {
                        lblCloQty.Text = cloqty.ToString();
                        lblCloVal.Text = cloval.ToString();
                    }
                    else
                    {
                        lblCloQty.Text = "0.00";
                        lblCloVal.Text = "0.00";
                    }

                    // Update for the next row processing
                    previousCloQty = cloqty;
                    previousCloVal = cloval;
                }
                // Calculate the closing quantity and closing value from the previous row

            }
        }
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        string script = "window.close();";
        ScriptManager.RegisterStartupScript(this, GetType(), "CloseTab", script, true);
    }
    private void SetFiscalYear()
    {
        // Get the current date
        DateTime currentDate = DateTime.Now;
        int currentYear = currentDate.Year;

        // Determine Fiscal Year
        DateTime fiscalStart = new DateTime(currentYear, 7, 17); // 17-Jul-currentYear
        DateTime fiscalEnd;

        int startYear, endYear;

        if (currentDate < fiscalStart)
        {
            // If today is before 17-Jul, the fiscal year belongs to the previous year
            startYear = currentYear - 1;
            endYear = currentYear;
        }
        else
        {
            // Otherwise, use the current year
            startYear = currentYear;
            endYear = currentYear + 1;
        }

        // Fixed month-date, only year changes
        fiscalStart = new DateTime(startYear, 7, 17);
        fiscalEnd = new DateTime(endYear, 7, 16);

        // **Nepali Year Calculation (Approximate Conversion)**
        int nepaliStartYear = startYear + 57; // AD to BS conversion
        int nepaliEndYear = endYear + 57;

        // Ensure Nepali Year is properly formatted as a string
        string nepaliStart = nepaliStartYear.ToString() + "-04-01";
        string nepaliEnd = nepaliEndYear.ToString() + "-04-01";

        // Display Fiscal Year
        lblFisDate.Text = fiscalStart.ToString("dd-MMM-yyyy") + " to " + fiscalEnd.ToString("dd-MMM-yyyy") +
                             " (" + nepaliStart + " to " + nepaliEnd + ")";

    }

    protected void lblParticulars_Click(object sender, EventArgs e)
    {
        fiscal_year = Request.QueryString["fy"].ToString();
        product_id = Request.QueryString["productid"].ToString();
        batch = Request.QueryString["bat"].ToString();

        GridViewRow gr = ((LinkButton)sender).Parent.Parent as GridViewRow;
        int rowIndex = gr.RowIndex;  // Get the current row index

        // Get the labels for the current row
        LinkButton lblParticulars = gr.FindControl("lblParticulars") as LinkButton;
        Label lblMnth = gr.FindControl("lblMnth") as Label;
        Label lblCloQty = gr.FindControl("lblCloQty") as Label;   // Closing Quantity
        Label lblCloVal = gr.FindControl("lblCloVal") as Label;   // Closing Value
        Label lblInQty = gr.FindControl("lblInQty") as Label;     // Incoming Quantity
        Label lblInVal = gr.FindControl("lblInVal") as Label;     // Incoming Value
        Label lblOutQty = gr.FindControl("lblOutQty") as Label;   // Outgoing Quantity
        Label lblOutVal = gr.FindControl("lblOutVal") as Label;   // Outgoing Value

        string closingQty, closingVal, inQty, inVal, outQty, outVal;

        if (rowIndex == 0)
        {
            // If it's the first row, send the current row's values
            closingQty = lblCloQty.Text;
            closingVal = lblCloVal.Text;
            inQty = lblInQty.Text;
            inVal = lblInVal.Text;
            outQty = lblOutQty.Text;
            outVal = lblOutVal.Text;
        }
        else
        {
            // Get the previous row (one row up)
            GridViewRow previousRow = gridMonthlyProductSummary.Rows[rowIndex - 1];

            // Get values from the previous row
            Label lblCloQtyPrev = previousRow.FindControl("lblCloQty") as Label;
            Label lblCloValPrev = previousRow.FindControl("lblCloVal") as Label;
            Label lblInQtyPrev = previousRow.FindControl("lblInQty") as Label;
            Label lblInValPrev = previousRow.FindControl("lblInVal") as Label;
            Label lblOutQtyPrev = previousRow.FindControl("lblOutQty") as Label;
            Label lblOutValPrev = previousRow.FindControl("lblOutVal") as Label;

            // Use values from the previous row
            closingQty = lblCloQtyPrev.Text;
            closingVal = lblCloValPrev.Text;
            inQty = lblInQtyPrev.Text;
            inVal = lblInValPrev.Text;
            outQty = lblOutQtyPrev.Text;
            outVal = lblOutValPrev.Text;
        }

        // Construct the URL with additional parameters
        string url = "~/Account/Reports/Item_Register.aspx?productid=" + product_id +
                     "&fy=" + fiscal_year + "&bat=" + batch +
                     "&mthid=" + lblMnth.Text + "&mnth=" + lblParticulars.Text +
                     "&closingQty=" + closingQty +
                     "&closingVal=" + closingVal +
                     "&inQty=" + inQty +
                     "&inVal=" + inVal +
                     "&outQty=" + outQty +
                     "&outVal=" + outVal;

        string fullUrl = ResolveUrl(url);

        // Open the new page in a new tab
        string script = "window.open('" + fullUrl + "', '_blank');";
        ScriptManager.RegisterStartupScript(this, GetType(), "OpenInvoice", script, true);
    }





    protected void gridMonthlyProductSummary_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow secondHeader = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);

            // First two columns (S.N and Particulars)
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "S.N";
            cell.RowSpan = 1; 
            // Spanning across both header rows
            secondHeader.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Particulars";
            cell.RowSpan = 1;
            secondHeader.Cells.Add(cell);

            // Inward Section Header
            cell = new TableHeaderCell();
            cell.Text = "Inward";
            cell.ColumnSpan = 2; // Covers Inward Quantity & Inward Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);

            // Outward Section Header
            cell = new TableHeaderCell();
            cell.Text = "Outward";
            cell.ColumnSpan = 2; // Covers Outward Quantity & Outward Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);

            // Closing Section Header
            cell = new TableHeaderCell();
            cell.Text = "Closing";
            cell.ColumnSpan = 2; // Covers Closing Quantity & Closing Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);

            // Insert the second header row at the top
            gridMonthlyProductSummary.Controls[0].Controls.AddAt(0, secondHeader);
        }
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }
}