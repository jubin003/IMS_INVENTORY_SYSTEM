using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PhyeGanCore;
using Entity.Components;
using Service.Components;
using System.Data;

public partial class Account_Reports_Item_Register : System.Web.UI.Page
{
    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();
    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();
    AccountFunction AF = new AccountFunction();
    SALES_INVOICE_MASTER SIMEnt = new SALES_INVOICE_MASTER();
    SALES_INVOICE_MASTERService SIMSer = new SALES_INVOICE_MASTERService();

    PURCHASE_INVOICE_MASTER PIMEnt = new PURCHASE_INVOICE_MASTER();
    PURCHASE_INVOICE_MASTERService PIMSer = new PURCHASE_INVOICE_MASTERService();
    PhyeGan PG = new PhyeGan();
    string fiscal_year, product_id, batch, month_id, month_name;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadGridWithOpeningBalance();
        }
    }

    private void LoadGridWithOpeningBalance()
    {
        if (Request.QueryString["fy"] != null &&
            Request.QueryString["productid"] != null &&
            Request.QueryString["mthid"] != null &&
            Request.QueryString["mnth"] != null)
        {
            fiscal_year = Request.QueryString["fy"].ToString();
            product_id = Request.QueryString["productid"].ToString();
            batch = Request.QueryString["bat"] != null ? Request.QueryString["bat"].ToString() : "";
            month_id = Request.QueryString["mthid"].ToString();
            month_name = Request.QueryString["mnth"].ToString();

            // Get the grid data
            DataTable dt = AF.getMonthlyItemRegister(fiscal_year, product_id, batch, month_id);

            // Get Closing Quantity, Closing Value, Incoming Quantity, Incoming Value, Outgoing Quantity, and Outgoing Value from QueryString
            string closingQty = Request.QueryString["closingQty"] != null ? Request.QueryString["closingQty"].ToString() : "0";
            string closingVal = Request.QueryString["closingVal"] != null ? Request.QueryString["closingVal"].ToString() : "0";
            string inQty = Request.QueryString["inQty"] != null ? Request.QueryString["inQty"].ToString() : "0";
            string inVal = Request.QueryString["inVal"] != null ? Request.QueryString["inVal"].ToString() : "0";
            string outQty = Request.QueryString["outQty"] != null ? Request.QueryString["outQty"].ToString() : "0";
            string outVal = Request.QueryString["outVal"] != null ? Request.QueryString["outVal"].ToString() : "0";

            // Insert Opening Balance if Closing Values exist
            if (closingQty != "0" || closingVal != "0")
            {
                DataRow openingRow = dt.NewRow();
                openingRow["PARTICULARS"] = "Opening Balance";
                openingRow["TOTAL_INWARDS_QTY"] = inQty; // Incoming Quantity
                openingRow["TOTAL_INWARDS_VALUE"] = inVal; // Incoming Value
                openingRow["TOTAL_OUTWARD_QTY"] = outQty; // Outgoing Quantity
                openingRow["TOTAL_OUTWARD_VALUE"] = outVal; // Outgoing Value
                openingRow["TOTAL_CLOSING_QTY"] = closingQty;
                openingRow["TOTAL_CLOSING_VALUE"] = closingVal;

                dt.Rows.InsertAt(openingRow, 0); // Insert at the first row
            }

            // Bind the updated DataTable to GridView
            gridItemReg.DataSource = dt;
            gridItemReg.DataBind();

            // Display page elements
            divToPrint.Visible = true;
            lblCompanyName.Text = PG.CompanyName();
            lblAddress.Text = PG.CompanyAddress();
            lblContact.Text = PG.CompanyContact();
            lblFyDate.Text = fiscal_year;
            lblMonth.Text = month_name;

            // Get Product Name
            PEnt = new PRODUCT();
            PEnt.PK_ID = product_id;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                lblProduct.Text = PEnt.PRODUCT_NAME;
            }
        }
    }



    double inward_qty, inward_val, outward_qty, out_ward_val, closing_qty, closing_val;
    double total_inward_qty, total_inward_val, total_outward_qty, total_out_ward_val, total_closing_qty, total_closing_val;

    double previousCloQty = 0;
    double previousCloVal = 0;
    double preCloRate = 0;
    protected void gridItemReg_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblInQtyF = e.Row.FindControl("lblInQtyF") as Label;
            Label lblInValF = e.Row.FindControl("lblInValF") as Label;
            Label lblOutQtyF = e.Row.FindControl("lblOutQtyF") as Label;
            Label lblOutValF = e.Row.FindControl("lblOutValF") as Label;
            Label lblCloQtyF = e.Row.FindControl("lblCloQtyF") as Label;
            Label lblCloValF = e.Row.FindControl("lblCloValF") as Label;
            Label lblInRateF = e.Row.FindControl("lblInRateF") as Label;
            Label lblOutRateF = e.Row.FindControl("lblOutRateF") as Label;
            Label lblCloRateF = e.Row.FindControl("lblCloRateF") as Label;

            double total_inward_qty = 0, total_inward_val = 0;
            double total_outward_qty = 0, total_outward_val = 0;

            double last_closing_qty = 0, last_closing_val = 0; // Will hold last row's values

            int rowCount = gridItemReg.Rows.Count;

            for (int i = 0; i < rowCount; i++)
            {
                GridViewRow gr = gridItemReg.Rows[i];

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

            // Calculate and set footer rates
            lblInRateF.Text = total_inward_qty > 0 ? (total_inward_val / total_inward_qty).ToString("#0.00") : "0.00";
            lblOutRateF.Text = total_outward_qty > 0 ? (total_outward_val / total_outward_qty).ToString("#0.00") : "0.00";
            lblCloRateF.Text = last_closing_qty > 0 ? (last_closing_val / last_closing_qty).ToString("#0.00") : "0.00";
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblInQty = e.Row.FindControl("lblInQty") as Label;
            Label lblInVal = e.Row.FindControl("lblInVal") as Label;
            Label lblOutQty = e.Row.FindControl("lblOutQty") as Label;
            Label lblOutVal = e.Row.FindControl("lblOutVal") as Label;
            Label lblCloQty = e.Row.FindControl("lblCloQty") as Label;
            Label lblCloVal = e.Row.FindControl("lblCloVal") as Label;

            Label lblInRate = e.Row.FindControl("lblInRate") as Label;
            Label lblOutRate = e.Row.FindControl("lblOutRate") as Label;
            Label lblCloRate = e.Row.FindControl("lblCloRate") as Label;

            double inward_qty = string.IsNullOrEmpty(lblInQty.Text) ? 0 : Convert.ToDouble(lblInQty.Text);
            double inward_val = string.IsNullOrEmpty(lblInVal.Text) ? 0 : Convert.ToDouble(lblInVal.Text);

            double outward_qty = string.IsNullOrEmpty(lblOutQty.Text) ? 0 : Convert.ToDouble(lblOutQty.Text);
            double outward_val = string.IsNullOrEmpty(lblOutVal.Text) ? 0 : Convert.ToDouble(lblOutVal.Text);

            double closing_qty = string.IsNullOrEmpty(lblCloQty.Text) ? 0 : Convert.ToDouble(lblCloQty.Text);
            double closing_val = string.IsNullOrEmpty(lblCloVal.Text) ? 0 : Convert.ToDouble(lblCloVal.Text);

            // Calculate Inward Rate
            lblInRate.Text = inward_qty > 0 ? (inward_val / inward_qty).ToString("#0.00") : "0.00";

            // Calculate Outward Rate
            lblOutRate.Text = outward_qty > 0 ? (outward_val / outward_qty).ToString("#0.00") : "0.00";

            // Calculate Closing Rate


            // Calculate Closing Quantity and Value for current row
            if (e.Row.RowIndex == 0) // First row (Opening Balance)
            {
                previousCloQty = closing_qty; // Set as initial balance for first row
                previousCloVal = closing_val; // Set as initial balance for first row
            }
            else
            {
                // Calculate the closing quantity and closing value from the previous row
                double cloqty = previousCloQty + inward_qty - outward_qty;
                double cloval = previousCloVal + inward_val - outward_val;

                // Assign calculated values to the current row's labels
                lblCloQty.Text = cloqty.ToString("#0.00");
                lblCloVal.Text = cloval.ToString("#0.00");
                if (lblCloQty.Text == "0.00")
                {
                    lblCloQty.Text = "0.00";
                    lblCloVal.Text = "0.00";
                    lblCloRate.Text = "0.00";

                }
                else
                {
                    lblCloRate.Text = (cloval / cloqty).ToString("#0.00");
                }

                // Update previous values for next row
                previousCloQty = cloqty;
                previousCloVal = cloval;
            }
        }
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        string script = "window.close();";
        ScriptManager.RegisterStartupScript(this, GetType(), "CloseTab", script, true);
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }



    protected void gridItemReg_RowCreated(object sender, GridViewRowEventArgs e)
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
            cell.Text = "Date";
            cell.RowSpan = 1;
            cell.ColumnSpan = 2;
            // Spanning across both header rows
            secondHeader.Cells.Add(cell);


            cell = new TableHeaderCell();
            cell.Text = "Particulars";
            cell.RowSpan = 1;
            cell.ColumnSpan = 2;
            secondHeader.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Voucher Type";
            cell.RowSpan = 1;
            secondHeader.Cells.Add(cell);
            // Inward Section Header
            cell = new TableHeaderCell();
            cell.Text = "Inward";
            cell.ColumnSpan = 3; // Covers Inward Quantity & Inward Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);

            // Outward Section Header
            cell = new TableHeaderCell();
            cell.Text = "Outward";
            cell.ColumnSpan = 3; // Covers Outward Quantity & Outward Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);

            // Closing Section Header
            cell = new TableHeaderCell();
            cell.Text = "Closing";
            cell.ColumnSpan = 3; // Covers Closing Quantity & Closing Value
            cell.HorizontalAlign = HorizontalAlign.Center;
            secondHeader.Cells.Add(cell);

            // Insert the second header row at the top
            gridItemReg.Controls[0].Controls.AddAt(0, secondHeader);
        }
    }

    protected void lblInvNum_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((LinkButton)sender).Parent.Parent as GridViewRow;

        LinkButton lblInvNum = gr.FindControl("lblInvNum") as LinkButton;
        Label lblVcType = gr.FindControl("lblVcType") as Label;

        if (lblVcType.Text == "PURCHASE")
        {

            PIMEnt = new PURCHASE_INVOICE_MASTER
            {
                SUPPLIER_INVOICE_NO = lblInvNum.Text
            };
            PIMEnt = (PURCHASE_INVOICE_MASTER)PIMSer.GetSingle(PIMEnt);
            if (PIMEnt != null)
            {
                string url = "~/reports/purchase/ShowPurchaseInvoice.aspx?dakhno=" + PIMEnt.PK_ID;
                string fullUrl = ResolveUrl(url);
                string script = "window.open('" + fullUrl + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenInvoice", script, true);
            }
            // Register JavaScript to open the URL in a new tab

        }
        else if (lblVcType.Text == "PURCHASE RETURN")
        {

            PURCHASE_RETURN_MASTER PREnt  = new PURCHASE_RETURN_MASTER
            {
                DEBIT_NOTE_NUMBER = lblInvNum.Text
            };
            PURCHASE_RETURN_MASTERService PRSer = new PURCHASE_RETURN_MASTERService();
            PREnt = (PURCHASE_RETURN_MASTER)PRSer.GetSingle(PREnt);
            if (PREnt != null)
            {
                string url = "~/Reports/Return/ShowPurchaseReturn.aspx?invno=" + PREnt.PK_ID;
                string fullUrl = ResolveUrl(url);
                string script = "window.open('" + fullUrl + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenInvoice", script, true);
            }
        }
        else if (lblVcType.Text == "SALES")
        {
            SIMEnt = new SALES_INVOICE_MASTER();
            SIMEnt.INVOICE_NUMBER = lblInvNum.Text;
            SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
            if (SIMEnt != null)
            {
                string url = "~/utilities/Sales/ShowInvoiceToPrint.aspx?invno=" + SIMEnt.PK_ID;
                string fullUrl = ResolveUrl(url);

                // Register JavaScript to open the URL in a new tab
                string script = "window.open('" + fullUrl + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenInvoice", script, true);
            }
        }
        else if (lblVcType.Text == "SALES RETURN")
        {
            SALES_RETURN_MASTER SRMEnt  = new SALES_RETURN_MASTER();
            SRMEnt.CREDIT_NOTE_NUMBER = lblInvNum.Text;
            SALES_RETURN_MASTERService SRMSer = new SALES_RETURN_MASTERService();
            SRMEnt = (SALES_RETURN_MASTER)SRMSer.GetSingle(SRMEnt);
            if (SRMEnt != null)
            {
                string url = "~/Reports/Return/ShowSalesReturn.aspx?invno=" + SRMEnt.PK_ID;
                string fullUrl = ResolveUrl(url);

                // Register JavaScript to open the URL in a new tab
                string script = "window.open('" + fullUrl + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenInvoice", script, true);
            }

        }

    }
}