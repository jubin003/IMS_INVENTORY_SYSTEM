using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using DataHelper.Framework;
using PhyeGanCore;

public partial class Administration_ClosingBalance : System.Web.UI.Page
{
    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();

    CLOSING_STATUS CSEnt = new CLOSING_STATUS();
    CLOSING_STATUSService CSSer = new CLOSING_STATUSService();

    PRODUCT_CATEGORY PCEnt = new PRODUCT_CATEGORY();
    PRODUCT_CATEGORYService PCSer = new PRODUCT_CATEGORYService();

    PRODUCT_SUB_CATEGORY PSCEnt = new PRODUCT_SUB_CATEGORY();
    PRODUCT_SUB_CATEGORYService PSCSer = new PRODUCT_SUB_CATEGORYService();

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();

    PRODUCT_COLOUR PColEnt = new PRODUCT_COLOUR();
    PRODUCT_COLOURService PColSer = new PRODUCT_COLOURService();

    PRODUCT_SIZE PSEnt = new PRODUCT_SIZE();
    PRODUCT_SIZEService PSSer = new PRODUCT_SIZEService();

    PRODUCT_MANUFACTURE PMEnt = new PRODUCT_MANUFACTURE();
    PRODUCT_MANUFACTUREService PMSer = new PRODUCT_MANUFACTUREService();

    OPENING_BALANCE OBEnt = new OPENING_BALANCE();
    OPENING_BALANCEService OBSer = new OPENING_BALANCEService();

    HelperFunction hf = new HelperFunction();
    PhyeGan PG = new PhyeGan();
    PhyeGanDate pgd = new PhyeGanDate();
    Boolean IsPageRefresh = false;

    UserProfileEntity userProfileEnt = new UserProfileEntity();

    static string path = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {


            loadBranch();
            checkBranch();
            LoadFiscalYear();
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
    protected void LoadFiscalYear()
    {
        FYEnt = new FISCALYEAR();
        ddlFiscalYear.DataSource = FYSer.GetAll(FYEnt);
        ddlFiscalYear.DataValueField = "FISCAL_YEAR";
        ddlFiscalYear.DataTextField = "FISCAL_YEAR";
        ddlFiscalYear.DataBind();
        ddlFiscalYear.SelectedValue = pgd.checkFiscalYear(pgd.NepaliMonth(), pgd.NepaliYear());
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        LoadStock();
    }
    protected void LoadStock()
    {
        string office_code = "";
        if (ddlBranch.SelectedValue != "-")
        {
            office_code = ddlBranch.SelectedValue;
        }
        grdStock.DataSource = hf.getOpeningClosingBalance(ddlFiscalYear.SelectedValue, null, null, null, null, null, null, null, office_code);
        grdStock.DataBind();

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

    protected void btnClosing_Click(object sender, EventArgs e)
    {
        CSEnt = new CLOSING_STATUS();
        CSEnt.FISCAL_YEAR = ddlFiscalYear.SelectedValue;
         CSEnt.STATUS = "1";
        CSEnt = (CLOSING_STATUS)CSSer.GetSingle(CSEnt);
        if (CSEnt == null)
        {
            if (chkFinalClosing.Checked)
            {
                if (ddlFiscalYear.SelectedValue == pgd.checkFiscalYear(pgd.NepaliMonth(), pgd.NepaliYear()))
                {
                    string lastdateofFY = "";

                    #region fiscalyear ko last day find out gare ko 

                    lastdateofFY = pgd.ConvertNepaliTOEnglish("32", "3", pgd.NepaliYear());

                    if (lastdateofFY == "")
                    {
                        lastdateofFY = pgd.ConvertNepaliTOEnglish("31", "3", pgd.NepaliYear());
                    }
                    else if (lastdateofFY == "")
                    {
                        lastdateofFY = pgd.ConvertNepaliTOEnglish("30", "3", pgd.NepaliYear());
                    }
                    else if (lastdateofFY == "")
                    {
                        lastdateofFY = pgd.ConvertNepaliTOEnglish("29", "3", pgd.NepaliYear());
                    }

                    #endregion

                    #region if selected fiscal year ra current fiscal year same ho ra aja ko date fiscal year ko last date ho vaye closing garne.. else not

                    if (lastdateofFY == pgd.ConvertNepaliTOEnglish(pgd.NepaliDay(), pgd.NepaliMonth(), pgd.NepaliYear()))
                    {
                        PerformClosing();
                    }
                    else
                    {

                        HelperFunction.MsgBox(this, this.GetType(), "Sorry today is not the end day of this Fiscal year.");
                    }
                    #endregion
                }
                else
                {
                    #region select gareko fiscal year ra current year same hoina vaye selected fiscal year ko closing garne
                    PerformClosing();
                    #endregion
                }
            }
            else
            {
                PerformClosing();
            }
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Closing of Fiscal Year: " + ddlFiscalYear.SelectedValue + " is previously done.");
        }

    }
    protected void PerformClosing()
    {
        DistributedTransaction DT = new DistributedTransaction();

        foreach (GridViewRow gr in grdStock.Rows)
        {
            Label lblProduct_id = gr.FindControl("lblProduct_id") as Label;
            Label lblBatchNo = gr.FindControl("lblBatchNo") as Label;
            Label lblExpiryDate = gr.FindControl("lblExpiryDate") as Label;

            Label lblManufacturerID = gr.FindControl("lblManufacturerID") as Label;
            Label lblClosing = gr.FindControl("lblClosing") as Label;
            Label lblClosingBalance = gr.FindControl("lblClosingBalance") as Label;

            OBEnt = new OPENING_BALANCE();
            OBEnt.PRODUCT_ID = lblProduct_id.Text;
            OBEnt.BATCH_NUMBER = lblBatchNo.Text;
            OBEnt.EXPIRY_DATE = lblExpiryDate.Text;

            OBEnt.MANUFACTURER_ID = lblManufacturerID.Text;

            string[] currfy = ddlFiscalYear.SelectedValue.Split('/');
            string fpart = (Convert.ToDouble(currfy[0]) + 1).ToString();
            string spart = (Convert.ToDouble(currfy[1]) + 1).ToString();
            OBEnt.FISCAL_YEAR = fpart + "/" + spart;
            double closing_value, rate, qty;
            try
            {
                if (lblClosing.Text.Contains("("))
                {
                    qty = Convert.ToDouble(lblClosing.Text.Replace("(", "").Replace(")", "")) * -1;
                    closing_value = Convert.ToDouble(lblClosingBalance.Text);
                    if (qty != 0)
                    {
                        rate = closing_value / qty;
                    }
                    else
                    {
                        rate = 0;
                    }
                    OBEnt.QUANTITY = qty.ToString("#0.00");
                }
                else
                {
                    qty = Convert.ToDouble(lblClosing.Text);
                    closing_value = Convert.ToDouble(lblClosingBalance.Text);
                    if (qty != 0)
                    {
                        rate = closing_value / qty;
                    }
                    else
                    {
                        rate = 0;
                    }
                    OBEnt.QUANTITY = qty.ToString("#0.00");
                }


            }
            catch
            {
                closing_value = 0;
                OBEnt.QUANTITY = 0.ToString("#0.00");
                rate = 0;
            }
            OBEnt.RATE = rate.ToString("#0.00");
            OBEnt.CLOSING_VALUE = closing_value.ToString("#0.00");
            OBEnt.OFFICE_CODE = ddlBranch.SelectedValue;

            OBSer.Insert(OBEnt, DT);
        }

        CSEnt = new CLOSING_STATUS();
        CSEnt.FISCAL_YEAR = ddlFiscalYear.SelectedValue;
        CSEnt.OFFICE_CODE = ddlBranch.SelectedValue;
        if (chkFinalClosing.Checked)
        {
            CSEnt.STATUS = "1";
        }
        else
        {
            CSEnt.STATUS = "0";
        }
        CSSer.Insert(CSEnt, DT);

        if (DT.HAPPY == true)
        {
            DT.Commit();
            HelperFunction.MsgBox(this, this.GetType(), "Successfully Closing of Fiscal Year: " + ddlFiscalYear.SelectedValue + " is done");
        }
        else
        {
            DT.Abort();
            HelperFunction.MsgBox(this, this.GetType(), "Something Goes Wrong. Please Try Again");
        }
        DT.Dispose();
    }

    protected void checkBranch()
    {
        if (ddlBranch.SelectedValue == "-")
        {
            btnClosing.Visible = false;
        }
        else
        {
            btnClosing.Visible = true;
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkBranch();
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
                {
                    lblClosingBalance.Text = Math.Abs(closingValue).ToString("N2");
                }

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

            //Label lblTotalClosing = (Label)e.Row.FindControl("lblCloQtyF");
            //if (lblTotalClosing != null)
            //    lblTotalClosing.Text = totalClosing < 0 ? "(" + Math.Abs(totalClosing).ToString("N2") + ")" : totalClosing.ToString("N2");

            //Label lblTotalClosingBalance = (Label)e.Row.FindControl("lblCloValF");
            //if (lblTotalClosingBalance.Text != null || lblTotalClosingBalance.Text != "0.00")
            //    lblTotalClosingBalance.Text = Math.Abs(totalClosingBalance).ToString("N2");
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
}
