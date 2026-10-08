using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class BillOfMaterial : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdowns();
            BindGrid();

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string finishedId = Request.QueryString["id"];
                if (ddlFinishedProduct.Items.FindByValue(finishedId) != null)
                {
                    ddlFinishedProduct.SelectedValue = finishedId;
                    LoadExistingBOM(finishedId);
                }
            }
        }
    }

    private void BindGrid()
    {
        gvBOMDetails.DataSource = BOMTable;
        gvBOMDetails.DataBind();
    }

    private Dictionary<string, string> GetUnitDictionary()
    {
        Dictionary<string, string> unitNames = new Dictionary<string, string>();
        try
        {
            object unitService = new Service.Components.PRODUCT_UNITService();
            Entity.Components.PRODUCT_UNIT puFilter = new Entity.Components.PRODUCT_UNIT();
            EntityList allUnits = (EntityList)((Service.Framework.AbstractService)unitService).GetAll(puFilter);

            if (allUnits != null)
            {
                foreach (Entity.Components.PRODUCT_UNIT u in allUnits)
                {
                    if (!unitNames.ContainsKey(u.PK_ID))
                    {
                        unitNames.Add(u.PK_ID, u.UNIT_NAME);
                    }
                }
            }
        }
        catch { }

        return unitNames;
    }

    private void LoadDropdowns()
    {
        try
        {
            PRODUCTService prodService = new PRODUCTService();
            Dictionary<string, string> unitNames = GetUnitDictionary();

            PRODUCT finishedFilter = new PRODUCT();
            finishedFilter.PRODUCT_TYPE_ID = "2";
            EntityList finishedList = (EntityList)prodService.GetAll(finishedFilter);

            ddlFinishedProduct.DataSource = finishedList;
            ddlFinishedProduct.DataTextField = "PRODUCT_NAME";
            ddlFinishedProduct.DataValueField = "PK_ID";
            ddlFinishedProduct.DataBind();
            ddlFinishedProduct.Items.Insert(0, new ListItem("-- Select Finished Product --", ""));

            PRODUCT rawFilter = new PRODUCT();
            rawFilter.PRODUCT_TYPE_ID = "3";
            EntityList rawList = (EntityList)prodService.GetAll(rawFilter);

            ddlRawMaterial.Items.Clear();
            ddlRawMaterial.Items.Add(new ListItem("-- Select Raw Material --", ""));

            if (rawList != null)
            {
                foreach (PRODUCT p in rawList)
                {
                    ddlRawMaterial.Items.Add(new ListItem(p.PRODUCT_NAME, p.PK_ID));
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading products: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    protected void ddlFinishedProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        string finishedId = ddlFinishedProduct.SelectedValue;
        if (!string.IsNullOrEmpty(finishedId))
        {
            LoadExistingBOM(finishedId);
        }
        else
        {
            BOMTable.Rows.Clear();
            BindGrid();
            lblMessage.Text = "";
        }
    }

    private void LoadExistingBOM(string finishedProductId)
    {
        BOMTable.Rows.Clear();

        PR_BILL_OF_MATERIALService bomService = new PR_BILL_OF_MATERIALService();
        PR_BILL_OF_MATERIAL filter = new PR_BILL_OF_MATERIAL();
        filter.FINISHED_PRODUCT_ID = finishedProductId;
        EntityList existingBoms = (EntityList)bomService.GetAll(filter);

        if (existingBoms != null && existingBoms.Count > 0)
        {
            PRODUCTService prodService = new PRODUCTService();
            PRODUCT rawFilter = new PRODUCT();
            rawFilter.PRODUCT_TYPE_ID = "3";
            EntityList rawList = (EntityList)prodService.GetAll(rawFilter);

            Dictionary<string, string> unitNames = GetUnitDictionary();
            Dictionary<string, PRODUCT> materialLookup = new Dictionary<string, PRODUCT>();

            if (rawList != null)
            {
                foreach (PRODUCT p in rawList)
                {
                    if (!materialLookup.ContainsKey(p.PK_ID))
                    {
                        materialLookup.Add(p.PK_ID, p);
                    }
                }
            }

            foreach (PR_BILL_OF_MATERIAL item in existingBoms)
            {
                DataRow dr = BOMTable.NewRow();
                dr["MATERIAL_ID"] = item.MATERIAL_ID;

                PRODUCT matObj;
                if (materialLookup.TryGetValue(item.MATERIAL_ID, out matObj))
                {
                    dr["MATERIAL_NAME"] = matObj.PRODUCT_NAME;

                    if (!string.IsNullOrEmpty(matObj.UNIT_ID) && unitNames.ContainsKey(matObj.UNIT_ID))
                    {
                        dr["UNIT"] = unitNames[matObj.UNIT_ID];
                    }
                    else
                    {
                        dr["UNIT"] = matObj.UNIT_ID ?? "-";
                    }
                }
                else
                {
                    dr["MATERIAL_NAME"] = "Material #" + item.MATERIAL_ID;
                    dr["UNIT"] = "-";
                }

                dr["QTY"] = item.QUANTITY;
                BOMTable.Rows.Add(dr);
            }

            lblMessage.Text = "Existing BOM loaded (" + existingBoms.Count.ToString() + " item(s)). Modifying and saving will update this recipe.";
            lblMessage.CssClass = "text-primary";
        }
        else
        {
            lblMessage.Text = "No existing recipe found for this product. You can create a new one.";
            lblMessage.CssClass = "text-secondary";
        }

        BindGrid();
    }

    protected void btnAddMaterial_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlRawMaterial.SelectedValue) || string.IsNullOrEmpty(txtQty.Text.Trim()))
        {
            lblMessage.Text = "Please select a material and enter a valid quantity.";
            lblMessage.CssClass = "text-danger";
            return;
        }

        foreach (DataRow row in BOMTable.Rows)
        {
            if (row["MATERIAL_ID"].ToString() == ddlRawMaterial.SelectedValue)
            {
                lblMessage.Text = "This material is already in the recipe.";
                lblMessage.CssClass = "text-danger";
                return;
            }
        }

        PRODUCTService prodService = new PRODUCTService();
        PRODUCT pFilter = new PRODUCT();
        pFilter.PK_ID = ddlRawMaterial.SelectedValue;
        EntityList res = (EntityList)prodService.GetAll(pFilter);

        string unitDisplay = "-";
        if (res != null && res.Count > 0)
        {
            PRODUCT selectedProd = (PRODUCT)res[0];
            Dictionary<string, string> unitNames = GetUnitDictionary();

            if (!string.IsNullOrEmpty(selectedProd.UNIT_ID) && unitNames.ContainsKey(selectedProd.UNIT_ID))
            {
                unitDisplay = unitNames[selectedProd.UNIT_ID];
            }
            else
            {
                unitDisplay = selectedProd.UNIT_ID ?? "-";
            }
        }

        string cleanMaterialName = ddlRawMaterial.SelectedItem.Text;

        DataRow newRow = BOMTable.NewRow();
        newRow["MATERIAL_ID"] = ddlRawMaterial.SelectedValue;
        newRow["MATERIAL_NAME"] = cleanMaterialName;
        newRow["UNIT"] = unitDisplay;
        newRow["QTY"] = txtQty.Text.Trim();

        BOMTable.Rows.Add(newRow);
        BindGrid();

        ddlRawMaterial.SelectedIndex = 0;
        txtQty.Text = "";
        lblMessage.Text = "";
    }

    protected void gvBOMDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvBOMDetails.EditIndex = e.NewEditIndex;
        BindGrid();
    }

    protected void gvBOMDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvBOMDetails.EditIndex = -1;
        BindGrid();
    }

    protected void gvBOMDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gvBOMDetails.Rows[e.RowIndex];
        TextBox txtEditQty = (TextBox)row.FindControl("txtEditQty");

        if (txtEditQty != null)
        {
            string newQty = txtEditQty.Text.Trim();

            if (string.IsNullOrEmpty(newQty))
            {
                lblMessage.Text = "Quantity cannot be empty.";
                lblMessage.CssClass = "text-danger";
                return;
            }

            BOMTable.Rows[e.RowIndex]["QTY"] = newQty;
        }

        gvBOMDetails.EditIndex = -1;
        BindGrid();
        lblMessage.Text = "";
    }

    protected void gvBOMDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BOMTable.Rows.RemoveAt(e.RowIndex);
        gvBOMDetails.EditIndex = -1;
        BindGrid();
    }

    protected void btnSaveBOM_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlFinishedProduct.SelectedValue))
        {
            lblMessage.Text = "Please select a finished product.";
            lblMessage.CssClass = "text-danger";
            return;
        }

        if (BOMTable.Rows.Count == 0)
        {
            lblMessage.Text = "Please add at least one raw material to the BOM.";
            lblMessage.CssClass = "text-danger";
            return;
        }

        try
        {
            PR_BILL_OF_MATERIALService bomService = new PR_BILL_OF_MATERIALService();
            string finishedProductId = ddlFinishedProduct.SelectedValue;

            PR_BILL_OF_MATERIAL delFilter = new PR_BILL_OF_MATERIAL();
            delFilter.FINISHED_PRODUCT_ID = finishedProductId;
            bomService.Delete(delFilter);

            foreach (DataRow row in BOMTable.Rows)
            {
                PR_BILL_OF_MATERIAL bomEntity = new PR_BILL_OF_MATERIAL();
                bomEntity.FINISHED_PRODUCT_ID = finishedProductId;
                bomEntity.MATERIAL_ID = row["MATERIAL_ID"].ToString();
                bomEntity.QUANTITY = row["QTY"].ToString();

                bomService.Insert(bomEntity);
            }

            ResetForm();
            lblMessage.Text = "Bill of Materials saved successfully!";
            lblMessage.CssClass = "text-success";
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error saving BOM: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    private void ResetForm()
    {
        BOMTable.Rows.Clear();
        BindGrid();
        if (ddlFinishedProduct.Items.Count > 0) ddlFinishedProduct.SelectedIndex = 0;
        if (ddlRawMaterial.Items.Count > 0) ddlRawMaterial.SelectedIndex = 0;
        txtQty.Text = "";
    }

    private DataTable BOMTable
    {
        get
        {
            if (ViewState["BOMTable"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("MATERIAL_ID", typeof(string));
                dt.Columns.Add("MATERIAL_NAME", typeof(string));
                dt.Columns.Add("UNIT", typeof(string));
                dt.Columns.Add("QTY", typeof(string));
                ViewState["BOMTable"] = dt;
            }
            return (DataTable)ViewState["BOMTable"];
        }
        set { ViewState["BOMTable"] = value; }
    }
}