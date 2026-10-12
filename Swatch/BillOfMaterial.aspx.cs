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
            LoadFinishedProductsDropdown();
            BindGrid();

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                string finishedId = Request.QueryString["id"];
                if (ddlFinishedProduct.Items.FindByValue(finishedId) != null)
                {
                    ddlFinishedProduct.SelectedValue = finishedId;
                    SelectFinishedProduct(finishedId);
                }
            }
            else
            {
                pnlRawMaterials.Visible = false;
            }
        }
    }

    private void BindGrid()
    {
        DataTable dt = BOMTable;
        if (dt.Rows.Count == 0)
        {
            DataTable dummyTable = dt.Clone();
            dummyTable.Rows.Add(dummyTable.NewRow());
            gvBOMDetails.DataSource = dummyTable;
            gvBOMDetails.DataBind();
            gvBOMDetails.Rows[0].Visible = false;
        }
        else
        {
            gvBOMDetails.DataSource = dt;
            gvBOMDetails.DataBind();
        }
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
                for (int i = 0; i < allUnits.Count; i++)
                {
                    Entity.Components.PRODUCT_UNIT u = (Entity.Components.PRODUCT_UNIT)allUnits[i];
                    if (u != null && !unitNames.ContainsKey(u.PK_ID))
                    {
                        unitNames.Add(u.PK_ID, u.UNIT_NAME);
                    }
                }
            }
        }
        catch { }

        return unitNames;
    }

    private void LoadFinishedProductsDropdown()
    {
        try
        {
            PRODUCTService prodService = new PRODUCTService();
            PRODUCT finishedFilter = new PRODUCT();
            finishedFilter.PRODUCT_TYPE_ID = "2";
            EntityList finishedList = (EntityList)prodService.GetAll(finishedFilter);

            ddlFinishedProduct.DataSource = finishedList;
            ddlFinishedProduct.DataTextField = "PRODUCT_NAME";
            ddlFinishedProduct.DataValueField = "PK_ID";
            ddlFinishedProduct.DataBind();
            ddlFinishedProduct.Items.Insert(0, new ListItem("-- Select Finished Product --", ""));
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading products: " + ex.Message;
        }
    }

    private void LoadRawMaterialDropdown(DropDownList ddl)
    {
        PRODUCTService prodService = new PRODUCTService();
        PRODUCT rawFilter = new PRODUCT();
        rawFilter.PRODUCT_TYPE_ID = "3";
        EntityList rawList = (EntityList)prodService.GetAll(rawFilter);

        ddl.Items.Clear();
        ddl.Items.Add(new ListItem("-- Select Raw Material --", ""));

        if (rawList != null)
        {
            for (int i = 0; i < rawList.Count; i++)
            {
                PRODUCT p = (PRODUCT)rawList[i];
                if (p != null && p.PRODUCT_TYPE_ID == "3")
                {
                    ddl.Items.Add(new ListItem(p.PRODUCT_NAME, p.PK_ID));
                }
            }
        }
    }

    protected void gvBOMDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DropDownList ddlRaw = (DropDownList)e.Row.FindControl("ddlRawMaterialH");
            if (ddlRaw != null && ddlRaw.Items.Count <= 1)
            {
                LoadRawMaterialDropdown(ddlRaw);
            }
        }
    }

    protected void ddlRawMaterialH_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlRaw = (DropDownList)sender;
        GridViewRow headerRow = gvBOMDetails.HeaderRow;

        if (headerRow != null)
        {
            Label lblUnitH = (Label)headerRow.FindControl("lblUnitH");
            string selectedMaterialId = ddlRaw.SelectedValue;

            if (lblUnitH != null)
            {
                if (string.IsNullOrEmpty(selectedMaterialId))
                {
                    lblUnitH.Text = "-";
                }
                else
                {
                    PRODUCTService prodService = new PRODUCTService();
                    PRODUCT pFilter = new PRODUCT();
                    pFilter.PK_ID = selectedMaterialId;
                    EntityList res = (EntityList)prodService.GetAll(pFilter);

                    if (res != null && res.Count > 0)
                    {
                        PRODUCT selectedProd = (PRODUCT)res[0];
                        Dictionary<string, string> units = GetUnitDictionary();

                        if (!string.IsNullOrEmpty(selectedProd.UNIT_ID) && units.ContainsKey(selectedProd.UNIT_ID))
                            lblUnitH.Text = units[selectedProd.UNIT_ID];
                        else
                            lblUnitH.Text = selectedProd.UNIT_ID ?? "-";
                    }
                    else
                    {
                        lblUnitH.Text = "-";
                    }
                }
            }
        }
    }

    protected void btnAddMaterial_Click(object sender, EventArgs e)
    {
        GridViewRow headerRow = gvBOMDetails.HeaderRow;
        if (headerRow == null) return;

        DropDownList ddlRaw = (DropDownList)headerRow.FindControl("ddlRawMaterialH");
        Label lblUnitH = (Label)headerRow.FindControl("lblUnitH");
        TextBox txtQtyH = (TextBox)headerRow.FindControl("txtQtyH");

        if (ddlRaw == null || string.IsNullOrEmpty(ddlRaw.SelectedValue) || txtQtyH == null || string.IsNullOrEmpty(txtQtyH.Text.Trim()))
        {
            lblMessage.Text = "Please select a material and enter a valid quantity.";
            return;
        }

        for (int i = 0; i < BOMTable.Rows.Count; i++)
        {
            if (BOMTable.Rows[i]["MATERIAL_ID"].ToString() == ddlRaw.SelectedValue)
            {
                lblMessage.Text = "This material is already in the recipe.";
                return;
            }
        }

        DataRow newRow = BOMTable.NewRow();
        newRow["MATERIAL_ID"] = ddlRaw.SelectedValue;
        newRow["MATERIAL_NAME"] = ddlRaw.SelectedItem.Text;
        newRow["UNIT"] = lblUnitH != null ? lblUnitH.Text : "-";
        newRow["QTY"] = txtQtyH.Text.Trim();

        BOMTable.Rows.Add(newRow);
        BindGrid();
        lblMessage.Text = "";
    }

    protected void ddlFinishedProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        string finishedId = ddlFinishedProduct.SelectedValue;
        if (!string.IsNullOrEmpty(finishedId))
        {
            SelectFinishedProduct(finishedId);
        }
        else
        {
            pnlRawMaterials.Visible = false;
            BOMTable.Rows.Clear();
            BindGrid();
            lblMessage.Text = "";
        }
    }

    private void SelectFinishedProduct(string finishedProductId)
    {
        pnlRawMaterials.Visible = true;
        LoadExistingBOM(finishedProductId);
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
                for (int i = 0; i < rawList.Count; i++)
                {
                    PRODUCT p = (PRODUCT)rawList[i];
                    if (p != null && p.PRODUCT_TYPE_ID == "3")
                    {
                        if (!materialLookup.ContainsKey(p.PK_ID))
                        {
                            materialLookup.Add(p.PK_ID, p);
                        }
                    }
                }
            }

            for (int i = 0; i < existingBoms.Count; i++)
            {
                PR_BILL_OF_MATERIAL item = (PR_BILL_OF_MATERIAL)existingBoms[i];
                DataRow dr = BOMTable.NewRow();
                dr["MATERIAL_ID"] = item.MATERIAL_ID;

                PRODUCT matObj;
                if (materialLookup.TryGetValue(item.MATERIAL_ID, out matObj))
                {
                    dr["MATERIAL_NAME"] = matObj.PRODUCT_NAME;
                    if (!string.IsNullOrEmpty(matObj.UNIT_ID) && unitNames.ContainsKey(matObj.UNIT_ID))
                        dr["UNIT"] = unitNames[matObj.UNIT_ID];
                    else
                        dr["UNIT"] = matObj.UNIT_ID ?? "-";
                }
                else
                {
                    dr["MATERIAL_NAME"] = "Material #" + item.MATERIAL_ID;
                    dr["UNIT"] = "-";
                }

                dr["QTY"] = item.QUANTITY;
                BOMTable.Rows.Add(dr);
            }

            lblMessage.Text = "Existing BOM loaded (" + existingBoms.Count.ToString() + " item(s)).";
        }
        else
        {
            lblMessage.Text = "No existing recipe found for this product. You can create a new one.";
        }

        BindGrid();
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
            return;
        }

        if (BOMTable.Rows.Count == 0)
        {
            lblMessage.Text = "Please add at least one raw material to the BOM.";
            return;
        }

        try
        {
            PR_BILL_OF_MATERIALService bomService = new PR_BILL_OF_MATERIALService();
            string finishedProductId = ddlFinishedProduct.SelectedValue;

            PR_BILL_OF_MATERIAL delFilter = new PR_BILL_OF_MATERIAL();
            delFilter.FINISHED_PRODUCT_ID = finishedProductId;
            bomService.Delete(delFilter);

            for (int i = 0; i < BOMTable.Rows.Count; i++)
            {
                DataRow row = BOMTable.Rows[i];
                PR_BILL_OF_MATERIAL bomEntity = new PR_BILL_OF_MATERIAL();
                bomEntity.FINISHED_PRODUCT_ID = finishedProductId;
                bomEntity.MATERIAL_ID = row["MATERIAL_ID"].ToString();
                bomEntity.QUANTITY = row["QTY"].ToString();
                bomService.Insert(bomEntity);
            }

            BOMTable.Rows.Clear();
            BindGrid();
            ddlFinishedProduct.SelectedIndex = 0;
            pnlRawMaterials.Visible = false;

            lblMessage.Text = "Bill of Materials saved successfully!";
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error saving BOM: " + ex.Message;
        }
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