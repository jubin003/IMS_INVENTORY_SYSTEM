using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class ViewBillOfMaterial : System.Web.UI.Page
{
    PR_BILL_OF_MATERIALService bomService = new PR_BILL_OF_MATERIALService();
    PRODUCTService prodService = new PRODUCTService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadMasterBOMGrid();
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

    private void LoadMasterBOMGrid()
    {
        try
        {
            PR_BILL_OF_MATERIAL bomFilter = new PR_BILL_OF_MATERIAL();
            EntityList allBoms = (EntityList)bomService.GetAll(bomFilter);

            // Group by FINISHED_PRODUCT_ID and count distinct raw materials
            Dictionary<string, int> bomCounts = new Dictionary<string, int>();
            if (allBoms != null)
            {
                for (int i = 0; i < allBoms.Count; i++)
                {
                    PR_BILL_OF_MATERIAL bom = (PR_BILL_OF_MATERIAL)allBoms[i];
                    if (bom != null && !string.IsNullOrEmpty(bom.FINISHED_PRODUCT_ID))
                    {
                        if (bomCounts.ContainsKey(bom.FINISHED_PRODUCT_ID))
                        {
                            bomCounts[bom.FINISHED_PRODUCT_ID]++;
                        }
                        else
                        {
                            bomCounts.Add(bom.FINISHED_PRODUCT_ID, 1);
                        }
                    }
                }
            }

            DataTable dtMaster = new DataTable();
            dtMaster.Columns.Add("FINISHED_PRODUCT_ID", typeof(string));
            dtMaster.Columns.Add("PRODUCT_CODE", typeof(string));
            dtMaster.Columns.Add("PRODUCT_NAME", typeof(string));
            dtMaster.Columns.Add("INGREDIENT_COUNT", typeof(int));

            PRODUCT prodFilter = new PRODUCT();
            prodFilter.PRODUCT_TYPE_ID = "2";
            EntityList finishedList = (EntityList)prodService.GetAll(prodFilter);

            Dictionary<string, PRODUCT> prodLookup = new Dictionary<string, PRODUCT>();
            if (finishedList != null)
            {
                for (int i = 0; i < finishedList.Count; i++)
                {
                    PRODUCT p = (PRODUCT)finishedList[i];
                    if (p != null && !prodLookup.ContainsKey(p.PK_ID))
                    {
                        prodLookup.Add(p.PK_ID, p);
                    }
                }
            }

            foreach (KeyValuePair<string, int> pair in bomCounts)
            {
                DataRow dr = dtMaster.NewRow();
                dr["FINISHED_PRODUCT_ID"] = pair.Key;
                dr["INGREDIENT_COUNT"] = pair.Value;

                if (prodLookup.ContainsKey(pair.Key))
                {
                    PRODUCT matchedProd = prodLookup[pair.Key];
                    dr["PRODUCT_CODE"] = !string.IsNullOrEmpty(matchedProd.PRODUCT_CODE) ? matchedProd.PRODUCT_CODE : "-";
                    dr["PRODUCT_NAME"] = matchedProd.PRODUCT_NAME;
                }
                else
                {
                    dr["PRODUCT_CODE"] = "-";
                    dr["PRODUCT_NAME"] = "Product #" + pair.Key;
                }

                dtMaster.Rows.Add(dr);
            }

            gvBOMMaster.DataSource = dtMaster;
            gvBOMMaster.DataBind();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading BOM recipes: " + ex.Message;
        }
    }

    protected void gvBOMMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string finishedProductId = e.CommandArgument.ToString();

        if (e.CommandName == "ViewDetails")
        {
            ShowRecipeDetails(finishedProductId);
        }
        else if (e.CommandName == "EditBOM")
        {
            Response.Redirect("BillOfMaterial.aspx?id=" + finishedProductId);
        }
        else if (e.CommandName == "DeleteBOM")
        {
            DeleteBOMRecipe(finishedProductId);
        }
    }

    private void ShowRecipeDetails(string finishedProductId)
    {
        try
        {
            PRODUCT pFilter = new PRODUCT();
            pFilter.PK_ID = finishedProductId;
            PRODUCT finishedProd = (PRODUCT)prodService.GetSingle(pFilter);
            lblDetailProductName.Text = finishedProd != null ? finishedProd.PRODUCT_NAME : "Product #" + finishedProductId;

            PR_BILL_OF_MATERIAL filter = new PR_BILL_OF_MATERIAL();
            filter.FINISHED_PRODUCT_ID = finishedProductId;
            EntityList bomItems = (EntityList)bomService.GetAll(filter);

            PRODUCT rawFilter = new PRODUCT();
            rawFilter.PRODUCT_TYPE_ID = "3";
            EntityList rawList = (EntityList)prodService.GetAll(rawFilter);

            Dictionary<string, PRODUCT> rawLookup = new Dictionary<string, PRODUCT>();
            if (rawList != null)
            {
                for (int i = 0; i < rawList.Count; i++)
                {
                    PRODUCT r = (PRODUCT)rawList[i];
                    if (r != null && !rawLookup.ContainsKey(r.PK_ID))
                    {
                        rawLookup.Add(r.PK_ID, r);
                    }
                }
            }

            Dictionary<string, string> units = GetUnitDictionary();

            DataTable dtDetails = new DataTable();
            dtDetails.Columns.Add("MATERIAL_CODE", typeof(string));
            dtDetails.Columns.Add("MATERIAL_NAME", typeof(string));
            dtDetails.Columns.Add("UNIT", typeof(string));
            dtDetails.Columns.Add("QUANTITY", typeof(string));

            if (bomItems != null)
            {
                for (int i = 0; i < bomItems.Count; i++)
                {
                    PR_BILL_OF_MATERIAL item = (PR_BILL_OF_MATERIAL)bomItems[i];
                    DataRow dr = dtDetails.NewRow();

                    if (rawLookup.ContainsKey(item.MATERIAL_ID))
                    {
                        PRODUCT raw = rawLookup[item.MATERIAL_ID];
                        dr["MATERIAL_CODE"] = !string.IsNullOrEmpty(raw.PRODUCT_CODE) ? raw.PRODUCT_CODE : "-";
                        dr["MATERIAL_NAME"] = raw.PRODUCT_NAME;

                        if (!string.IsNullOrEmpty(raw.UNIT_ID) && units.ContainsKey(raw.UNIT_ID))
                            dr["UNIT"] = units[raw.UNIT_ID];
                        else
                            dr["UNIT"] = raw.UNIT_ID ?? "-";
                    }
                    else
                    {
                        dr["MATERIAL_CODE"] = "-";
                        dr["MATERIAL_NAME"] = "Material #" + item.MATERIAL_ID;
                        dr["UNIT"] = "-";
                    }

                    dr["QUANTITY"] = item.QUANTITY;
                    dtDetails.Rows.Add(dr);
                }
            }

            gvRecipeDetails.DataSource = dtDetails;
            gvRecipeDetails.DataBind();

            pnlMasterView.Visible = false;
            pnlDetails.Visible = true;
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading recipe details: " + ex.Message;
        }
    }

    private void DeleteBOMRecipe(string finishedProductId)
    {
        try
        {
            PR_BILL_OF_MATERIAL delFilter = new PR_BILL_OF_MATERIAL();
            delFilter.FINISHED_PRODUCT_ID = finishedProductId;
            bomService.Delete(delFilter);

            lblMessage.Text = "BOM recipe deleted successfully.";
            LoadMasterBOMGrid();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error deleting BOM: " + ex.Message;
        }
    }

    protected void btnCloseDetails_Click(object sender, EventArgs e)
    {
        pnlDetails.Visible = false;
        pnlMasterView.Visible = true;
    }
}