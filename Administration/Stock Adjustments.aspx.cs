using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using PhyeGanCore;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;
using System.Data;


public partial class Utilities_New_Adjustment_Stock_Adjustments : System.Web.UI.Page
{
    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();

    PRODUCT_RATES PREnt = new PRODUCT_RATES();
    PRODUCT_RATEService PRSer = new PRODUCT_RATEService();

    PRODUCT_RATE_TYPE PRTEnt = new PRODUCT_RATE_TYPE();
    PRODUCT_RATE_TYPEService PRTSer = new PRODUCT_RATE_TYPEService();

    PRODUCT_ADJUSTMENT PAEnt = new PRODUCT_ADJUSTMENT();
    PRODUCT_ADJUSTMENTService PASer = new PRODUCT_ADJUSTMENTService();

    Boolean flag = false;
    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();

    HelperFunction hf = new HelperFunction();
    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGan PG = new PhyeGan();

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
                    loadProductName();
                    ddlProductRateType.Visible = PGPS.MultipleRate();
                    loadProductRateType();
                    txtAdjustDate.Text = PGD.GetTodayNepaliDate();
                    txtFiscalYear.Text = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
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

    protected void loadProductName()
    {
        PEnt = new PRODUCT();
        ddlProductName.DataSource = PSer.GetAll(PEnt);
        ddlProductName.DataTextField = "PRODUCT_NAME";
        ddlProductName.DataValueField = "PK_ID";
        ddlProductName.DataBind();
        ddlProductName.Items.Insert(0, "SELECT");
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
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        txtProductCode.Text = txtProductCode.Text.ToUpper();
        if (txtProductCode.Text != "")
        {
            PEnt = new PRODUCT();
            PEnt.PRODUCT_CODE = txtProductCode.Text;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                ddlProductName.SelectedValue = PEnt.PK_ID;
                LoadBatch(txtProductCode.Text);
                string batch = null;
                if (ddlBatch.SelectedValue != "")
                {
                    batch = ddlBatch.SelectedValue;
                }
            }
        }
    }

    protected void LoadBatch(string product)
    {
        ArrayList batch_array = new ArrayList();
        DataTable DT;
        string batch = null;
        if (ddlBatch.SelectedValue != "")
            batch = ddlBatch.SelectedValue;
        DT = hf.getOpeningClosingBalance(PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), null, null, product, null, null, null, null,"");

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
        if (ddlProductName.SelectedItem.ToString() == "Select")
        {
            msg = msg + "Select Product";
        }
        if (msg == "")
        {
            if (grdAdjustmentStock.Rows.Count <= 20)
                CreateGrid(1, true);
            else
                HelperFunction.MsgBox(this, this.GetType(), "Only 20 items can be sold for this bill.");
            ClearProduct();
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), msg);
        }
        AdjustmentTotal();
    }
    protected void ClearProduct()
    {
        txtProductCode.Text = "";
        ddlProductName.SelectedValue = "SELECT";
        txtQty.Text = "";
        lblBUnit.Text = "";
        txtRate.Text = "";
        txtAmount.Text = "";
        txtRemarks.Text = "";
        txtProductCode.Focus();
    }

    protected void ddlProductName_SelectedIndexChanged(object sender, EventArgs e)
    {
        PEnt = new PRODUCT();
        if (ddlProductName.SelectedValue != "Select")
        {
            PEnt.PK_ID = ddlProductName.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                txtProductCode.Text = PEnt.PRODUCT_CODE;
                LoadBatch(ddlProductName.SelectedValue);
                loadRate();

                string batch = null;
                if (ddlBatch.SelectedValue != "")
                {
                    batch = ddlBatch.SelectedValue;
                }

                LoadAvailablity(txtProductCode.Text, batch);

                lblBUnit.Text = getUnitName(PEnt.UNIT_ID);
            }
        }
        else
        {
            txtProductCode.Text = "";
            txtProductCode.Focus();
            lblBUnit.Text = "";
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
    protected void LoadAvailablity(string product, string batch)
    {
        DataTable DT;
        DT = hf.getOpeningClosingBalance(PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), null, null, product, batch, null, null, null,"");

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
    }
    protected void loadRate()
    {
        PREnt = new PRODUCT_RATES();
        PREnt.CUSTOMER_TYPE_ID = ddlProductRateType.SelectedValue;
        PREnt.PRODUCT_ID = ddlProductName.SelectedValue;
        PREnt = (PRODUCT_RATES)PRSer.GetSingle(PREnt);
        if (PREnt != null)
        {
            txtQty.Text = "1";
            txtRate.Text = PREnt.RATE;
            LoadQty();
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
    protected void LoadQty()
    {
        double Qty = 0;
        double rate = 0;
        string msg = "";
        try
        {
            Qty = Convert.ToDouble(txtQty.Text);
            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlProductName.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {

                if (Convert.ToDouble(txtAvilableQty.Text) - Convert.ToDouble(txtQty.Text) < 0)
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Avilable qty is less then sales qty. Are you sure to make invoice?");
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

    protected void txtAvilableQty_TextChanged(object sender, EventArgs e)
    {
        LoadQty();
    }
    protected void loadProductRateType()
    {
        PRTEnt = new PRODUCT_RATE_TYPE();
        ddlProductRateType.DataSource = PRTSer.GetAll(PRTEnt);
        ddlProductRateType.DataTextField = "RATE_TYPE_NAME";
        ddlProductRateType.DataValueField = "PK_ID";
        ddlProductRateType.DataBind();
        //ddlCustomerType.Items.Insert(0, "Select");

    }

    protected DataTable CreateGrid(int RowIndex, bool addRemoveFlag)
    {
        int row = grdAdjustmentStock.Rows.Count;
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("TAXABLE");
        dummyTable.Columns.Add("PRODUCT_CODE");
        dummyTable.Columns.Add("PRODUCT");
        dummyTable.Columns.Add("BATCH_NO");
        dummyTable.Columns.Add("EXPIRY_DATE");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("TOTAL");
        dummyTable.Columns.Add("REMARKS");


        if (row > 0)
        {
            foreach (GridViewRow r in grdAdjustmentStock.Rows)
            {
                Label lblProductPK_ID = r.FindControl("lblProductPK_ID") as Label;
                Label lblProductCode = r.FindControl("lblProductCode") as Label;
                Label lblProductName = r.FindControl("lblProductName") as Label;
                Label lblBatch = r.FindControl("lblBatch") as Label;
                Label lblExpDate = r.FindControl("lblExpDate") as Label;
                Label lblQty = r.FindControl("lblQty") as Label;
                Label lblUnit = r.FindControl("lblUnit") as Label;
                Label lblRate = r.FindControl("lblRate") as Label;
                Label lblItemTotal = r.FindControl("lblItemTotal") as Label;
                Label lblRemarks = r.FindControl("lblRemarks") as Label;
                DataRow dummyRw = dummyTable.NewRow();

                if (lblProductPK_ID.Text == ddlProductName.SelectedValue && lblRate.Text == txtRate.Text && lblQty.Text == txtQty.Text) // the selected product is already in the grid update the qty and rate
                {
                    dummyRw["PK_ID"] = lblProductPK_ID.Text;
                    dummyRw["PRODUCT_CODE"] = lblProductCode.Text;
                    dummyRw["PRODUCT"] = lblProductName.Text;
                    dummyRw["BATCH_NO"] = lblBatch.Text;
                    dummyRw["EXPIRY_DATE"] = lblExpDate.Text;
                    dummyRw["QUANTITY"] = txtQty.Text;
                    dummyRw["UNIT"] = lblUnit.Text;
                    dummyRw["RATE"] = txtRate.Text;
                    dummyRw["TOTAL"] = txtAmount.Text;
                    dummyRw["REMARKS"] = txtRemarks.Text;
                    flag = true;
                }
                else // the selected product is not in the grid add the product detail in the grid
                {
                    dummyRw["PK_ID"] = lblProductPK_ID.Text;
                    dummyRw["PRODUCT_CODE"] = lblProductCode.Text;
                    dummyRw["PRODUCT"] = lblProductName.Text;
                    dummyRw["BATCH_NO"] = lblBatch.Text;
                    dummyRw["EXPIRY_DATE"] = lblExpDate.Text;
                    dummyRw["QUANTITY"] = lblQty.Text;
                    dummyRw["UNIT"] = lblUnit.Text;
                    dummyRw["RATE"] = lblRate.Text;
                    dummyRw["TOTAL"] = lblItemTotal.Text;
                    dummyRw["REMARKS"] = lblRemarks.Text;
                }
                dummyTable.Rows.Add(dummyRw);
                flag = false;
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
            PEnt.PK_ID = ddlProductName.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                dummyRow["PK_ID"] = PEnt.PK_ID;
                dummyRow["TAXABLE"] = PEnt.TAX_STATUS;
                dummyRow["PRODUCT_CODE"] = PEnt.PRODUCT_CODE;
                dummyRow["PRODUCT"] = PEnt.PRODUCT_NAME;
                dummyRow["BATCH_NO"] = "";
                dummyRow["EXPIRY_DATE"] = "";
                dummyRow["QUANTITY"] = txtQty.Text;
                dummyRow["UNIT"] = getUnitName(PEnt.UNIT_ID);
                dummyRow["RATE"] = txtRate.Text;
                dummyRow["TOTAL"] = txtAmount.Text;
                dummyRow["REMARKS"] = txtRemarks.Text;

                dummyTable.Rows.Add(dummyRow);
            }
        }

        DataView dv = new DataView(dummyTable);

        grdAdjustmentStock.DataSource = dummyTable;
        grdAdjustmentStock.DataBind();

        //getTotal();
        return dummyTable;
    }


    protected void grdAdjustmentStock_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            CreateGrid(gr.RowIndex, false);
        }
    }
    protected void AdjustmentTotal()
    {
        double total = 0;
        foreach (GridViewRow gr in grdAdjustmentStock.Rows)
        {
            Label lblItemTotal = gr.FindControl("lblItemTotal") as Label;
            total = total + (Convert.ToDouble(lblItemTotal.Text));
        }
        lblAdjustmentTotal.Text = total.ToString("#0.00");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        #region to insert in to sales detail
        foreach (GridViewRow gr in grdAdjustmentStock.Rows)
        {
            Label lblSno = (Label)gr.FindControl("lblSno");
            Label lblProductPK_ID = (Label)gr.FindControl("lblProductPK_ID");
            Label lblQty = (Label)gr.FindControl("lblQty");
            Label lblRate = (Label)gr.FindControl("lblRate");
            Label lblBatch = (Label)gr.FindControl("lblBatch");
            Label lblExpDate = (Label)gr.FindControl("lblExpDate");
            Label lblRemarks = (Label)gr.FindControl("lblRemarks");
           


            PAEnt = new PRODUCT_ADJUSTMENT();
            PAEnt.ADJUSTMENT_FY = txtFiscalYear.Text;
            string engDate = PGD.GetEnglishDateFromNepali(txtAdjustDate.Text, "dd/mm/yyyy");
            PAEnt.ADJUSTMENT_DATE = engDate;
            string[] nepdate = txtAdjustDate.Text.Split('/');
            PAEnt.ADJUSTMENT_DAY = nepdate[0];
            PAEnt.ADJUSTMENT_MONTH = nepdate[1];
            PAEnt.ADJUSTMENT_YEAR = nepdate[2];
            PAEnt.QUANTITY = lblQty.Text;
            PAEnt.PRODUCT_ID = lblProductPK_ID.Text;
            PAEnt.EXPIRY_DATE = lblExpDate.Text;
            PAEnt.BATCH_NUMBER = lblBatch.Text;
            PAEnt.REMARKS = lblRemarks.Text;
            PASer.Insert(PAEnt);

        }

        #endregion
    }
    protected void SetVisibilty()
    {

        grdAdjustmentStock.Columns[3].Visible = PGPS.ProductBatch();
        grdAdjustmentStock.Columns[4].Visible = PGPS.ProductExpDate();
        grdAdjustmentStock.Columns[5].Visible = PGPS.DualQuantity();


    }
}