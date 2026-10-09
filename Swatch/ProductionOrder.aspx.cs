using System;
using System.Data;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class ProductionOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdowns();
            BindGrid();
        }
    }

    private void LoadDropdowns()
    {
        try
        {
            // Load Purchase Orders[cite: 3]
            PR_PURCHASE_ORDERService poService = new PR_PURCHASE_ORDERService();
            EntityList poList = (EntityList)poService.GetAll(new PR_PURCHASE_ORDER());
            ddlCustomerPO.DataSource = poList;
            ddlCustomerPO.DataTextField = "PO_NUMBER"; // Assuming field name
            ddlCustomerPO.DataValueField = "PK_ID";
            ddlCustomerPO.DataBind();
            ddlCustomerPO.Items.Insert(0, new ListItem("-- Select PO --", ""));

            // Load Products
            PRODUCTService prodService = new PRODUCTService();
            PRODUCT prodFilter = new PRODUCT();
            prodFilter.PRODUCT_TYPE_ID = "2"; // Finished Goods
            EntityList prodList = (EntityList)prodService.GetAll(prodFilter);
            ddlProduct.DataSource = prodList;
            ddlProduct.DataTextField = "PRODUCT_NAME";
            ddlProduct.DataValueField = "PK_ID";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0, new ListItem("-- Select Product --", ""));

            // Load Swatches[cite: 3]
            PR_SWATCHService swatchService = new PR_SWATCHService();
            EntityList swatchList = (EntityList)swatchService.GetAll(new PR_SWATCH());
            ddlSwatch.DataSource = swatchList;
            ddlSwatch.DataTextField = "SWATCH_NAME"; // Assuming field name
            ddlSwatch.DataValueField = "PK_ID";
            ddlSwatch.DataBind();
            ddlSwatch.Items.Insert(0, new ListItem("-- Select Swatch --", ""));

            // Load Units[cite: 3]
            PR_UNITService unitService = new PR_UNITService();
            EntityList unitList = (EntityList)unitService.GetAll(new PR_UNIT());
            ddlUnit.DataSource = unitList;
            ddlUnit.DataTextField = "UNIT_NAME";
            ddlUnit.DataValueField = "PK_ID";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, new ListItem("-- Select Unit --", ""));
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading form data: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    private void BindGrid()
    {
        gvProductionDetails.DataSource = ProductionLineTable;
        gvProductionDetails.DataBind();
    }

    protected void btnAddDetail_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlProduct.SelectedValue) || string.IsNullOrEmpty(txtQty.Text))
        {
            lblMessage.Text = "Product and Quantity are required.";
            lblMessage.CssClass = "text-danger";
            return;
        }

        DataRow newRow = ProductionLineTable.NewRow();
        newRow["PRODUCT_ID"] = ddlProduct.SelectedValue;
        newRow["PRODUCT_NAME"] = ddlProduct.SelectedItem.Text;
        newRow["SIZE_ID"] = ddlSize.SelectedValue;
        newRow["SIZE_NAME"] = string.IsNullOrEmpty(ddlSize.SelectedValue) ? "-" : ddlSize.SelectedItem.Text;
        newRow["SWATCH_ID"] = ddlSwatch.SelectedValue;
        newRow["SWATCH_NAME"] = string.IsNullOrEmpty(ddlSwatch.SelectedValue) ? "-" : ddlSwatch.SelectedItem.Text;
        newRow["QUANTITY"] = txtQty.Text.Trim();
        newRow["UNIT_ID"] = ddlUnit.SelectedValue;
        newRow["UNIT_NAME"] = string.IsNullOrEmpty(ddlUnit.SelectedValue) ? "-" : ddlUnit.SelectedItem.Text;

        ProductionLineTable.Rows.Add(newRow);
        BindGrid();

        // Reset inputs
        ddlProduct.SelectedIndex = 0;
        ddlSize.SelectedIndex = 0;
        ddlSwatch.SelectedIndex = 0;
        txtQty.Text = "";
        lblMessage.Text = "";
    }

    protected void gvProductionDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ProductionLineTable.Rows.RemoveAt(e.RowIndex);
        BindGrid();
    }

    protected void btnSaveOrder_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtProductionNo.Text) || ProductionLineTable.Rows.Count == 0)
        {
            lblMessage.Text = "Production number and at least one line item are required.";
            lblMessage.CssClass = "text-danger";
            return;
        }

        try
        {
            // 1. Save Master Record[cite: 3]
            PR_PRODUCTION_MASTERService masterService = new PR_PRODUCTION_MASTERService();
            PR_PRODUCTION_MASTER masterObj = new PR_PRODUCTION_MASTER();

            masterObj.PRODUCTION_NUMBER = txtProductionNo.Text.Trim();
            masterObj.PRODUCTION_TYPE = ddlProductionType.SelectedValue;
            masterObj.CUSTOMER_PO_ID = ddlCustomerPO.SelectedValue;
            masterObj.STATUS = ddlStatus.SelectedValue;

            DateTime prodDate;
            if (DateTime.TryParse(txtProductionDate.Text, out prodDate))
            {
                masterObj.PRODUCTION_DATE = prodDate.ToString("yyyy-MM-dd");
                masterObj.PRODUCTION_DAY = prodDate.Day.ToString();
                masterObj.PRODUCTION_MONTH = prodDate.Month.ToString();
                masterObj.PRODUCTION_YEAR = prodDate.Year.ToString();
            }

            DateTime compDate;
            if (DateTime.TryParse(txtCompletionDate.Text, out compDate))
            {
                masterObj.COMPLITION_DATE = compDate.ToString("yyyy-MM-dd");
                masterObj.COMPLITION_DAY = compDate.Day.ToString();
                masterObj.COMPLITION_MONTH = compDate.Month.ToString();
                masterObj.COMPLITION_YEAR = compDate.Year.ToString();
            }

            masterService.Insert(masterObj);

            // Retrieve the generated Master PK_ID (assuming DB triggers/identity or service logic handles this, mocking the ID retrieval)
            string generatedMasterId = masterObj.PK_ID;

            // 2. Save Detail Records[cite: 3]
            // 2. Save Detail Records
            PR_PRODCUTION_DETAILService detailService = new PR_PRODCUTION_DETAILService();

            foreach (DataRow row in ProductionLineTable.Rows)
            {
                PR_PRODCUTION_DETAIL detailObj = new PR_PRODCUTION_DETAIL();

                // Using the exact property names from your entity class
                detailObj.PRODUCTION_ID = generatedMasterId;
                detailObj.PRODUCT_ID = row["PRODUCT_ID"].ToString();
                detailObj.SIZE_ID = row["SIZE_ID"].ToString();
                detailObj.SWATCH_ID = row["SWATCH_ID"].ToString();
                detailObj.QUANTITY = row["QUANTITY"].ToString();
                detailObj.UNIT = row["UNIT_ID"].ToString();
                detailObj.STATUS = ddlStatus.SelectedValue;

                detailObj.PRODUCTION_DATE = masterObj.PRODUCTION_DATE;
                detailObj.PRODUCTION_DAY = masterObj.PRODUCTION_DAY;
                detailObj.PRODUCTION_MONTH = masterObj.PRODUCTION_MONTH;
                detailObj.PRODUCTION_YEAR = masterObj.PRODUCTION_YEAR;

                detailService.Insert(detailObj);
            }

            // Reset Form
            ProductionLineTable.Rows.Clear();
            BindGrid();
            txtProductionNo.Text = "";
            txtProductionDate.Text = "";
            txtCompletionDate.Text = "";

            lblMessage.Text = "Production Order saved successfully.";
            lblMessage.CssClass = "text-success status-active";
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error saving data: " + ex.Message;
            lblMessage.CssClass = "text-danger status-inactive";
        }
    }

    private DataTable ProductionLineTable
    {
        get
        {
            if (ViewState["ProductionLineTable"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PRODUCT_ID", typeof(string));
                dt.Columns.Add("PRODUCT_NAME", typeof(string));
                dt.Columns.Add("SIZE_ID", typeof(string));
                dt.Columns.Add("SIZE_NAME", typeof(string));
                dt.Columns.Add("SWATCH_ID", typeof(string));
                dt.Columns.Add("SWATCH_NAME", typeof(string));
                dt.Columns.Add("QUANTITY", typeof(string));
                dt.Columns.Add("UNIT_ID", typeof(string));
                dt.Columns.Add("UNIT_NAME", typeof(string));
                ViewState["ProductionLineTable"] = dt;
            }
            return (DataTable)ViewState["ProductionLineTable"];
        }
        set { ViewState["ProductionLineTable"] = value; }
    }
}