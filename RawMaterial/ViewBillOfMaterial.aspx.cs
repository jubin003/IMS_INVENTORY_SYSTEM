using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class ViewBillOfMaterial : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFilterDropdown();
            BindMasterBOMGrid();
        }
    }

    private void LoadFilterDropdown()
    {
        try
        {
            PRODUCTService prodService = new PRODUCTService();
            PRODUCT filter = new PRODUCT();
            filter.PRODUCT_TYPE_ID = "2"; // Finished Goods
            EntityList list = (EntityList)prodService.GetAll(filter);

            ddlFinishedProductFilter.DataSource = list;
            ddlFinishedProductFilter.DataTextField = "PRODUCT_NAME";
            ddlFinishedProductFilter.DataValueField = "PK_ID";
            ddlFinishedProductFilter.DataBind();
            ddlFinishedProductFilter.Items.Insert(0, new ListItem("-- All Finished Products --", ""));
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading finished products: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    private void BindMasterBOMGrid()
    {
        try
        {
            PR_BILL_OF_MATERIALService bomService = new PR_BILL_OF_MATERIALService();
            PR_BILL_OF_MATERIAL bomFilter = new PR_BILL_OF_MATERIAL();

            if (!string.IsNullOrEmpty(ddlFinishedProductFilter.SelectedValue))
            {
                bomFilter.FINISHED_PRODUCT_ID = ddlFinishedProductFilter.SelectedValue;
            }

            EntityList allBOMs = (EntityList)bomService.GetAll(bomFilter);

            DataTable dtMaster = new DataTable();
            dtMaster.Columns.Add("FINISHED_PRODUCT_ID", typeof(string));
            dtMaster.Columns.Add("PRODUCT_CODE", typeof(string));
            dtMaster.Columns.Add("PRODUCT_NAME", typeof(string));
            dtMaster.Columns.Add("INGREDIENT_COUNT", typeof(int));

            if (allBOMs != null && allBOMs.Count > 0)
            {
                // Count ingredients per finished product
                Dictionary<string, int> groupedCount = new Dictionary<string, int>();
                foreach (PR_BILL_OF_MATERIAL item in allBOMs)
                {
                    if (groupedCount.ContainsKey(item.FINISHED_PRODUCT_ID))
                    {
                        groupedCount[item.FINISHED_PRODUCT_ID] = groupedCount[item.FINISHED_PRODUCT_ID] + 1;
                    }
                    else
                    {
                        groupedCount.Add(item.FINISHED_PRODUCT_ID, 1);
                    }
                }

                // Cache all finished goods for fast lookups
                PRODUCTService prodService = new PRODUCTService();
                PRODUCT pFilter = new PRODUCT();
                pFilter.PRODUCT_TYPE_ID = "2";
                EntityList finishedGoodsList = (EntityList)prodService.GetAll(pFilter);

                Dictionary<string, PRODUCT> finishedLookup = new Dictionary<string, PRODUCT>();
                if (finishedGoodsList != null)
                {
                    foreach (PRODUCT p in finishedGoodsList)
                    {
                        if (!finishedLookup.ContainsKey(p.PK_ID))
                        {
                            finishedLookup.Add(p.PK_ID, p);
                        }
                    }
                }

                foreach (KeyValuePair<string, int> pair in groupedCount)
                {
                    DataRow row = dtMaster.NewRow();
                    row["FINISHED_PRODUCT_ID"] = pair.Key;

                    PRODUCT prod;
                    if (finishedLookup.TryGetValue(pair.Key, out prod))
                    {
                        row["PRODUCT_CODE"] = prod.PRODUCT_CODE;
                        row["PRODUCT_NAME"] = prod.PRODUCT_NAME;
                    }
                    else
                    {
                        row["PRODUCT_CODE"] = "-";
                        row["PRODUCT_NAME"] = "Product #" + pair.Key;
                    }

                    row["INGREDIENT_COUNT"] = pair.Value;
                    dtMaster.Rows.Add(row);
                }
            }

            gvBOMMaster.DataSource = dtMaster;
            gvBOMMaster.DataBind();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading BOM list: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        pnlDetails.Visible = false;
        BindMasterBOMGrid();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlFinishedProductFilter.SelectedIndex = 0;
        pnlDetails.Visible = false;
        BindMasterBOMGrid();
    }

    protected void gvBOMMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string finishedId = e.CommandArgument.ToString();

        if (e.CommandName == "ViewDetails")
        {
            LoadRecipeDetails(finishedId);
        }
        else if (e.CommandName == "DeleteBOM")
        {
            try
            {
                PR_BILL_OF_MATERIALService bomService = new PR_BILL_OF_MATERIALService();
                PR_BILL_OF_MATERIAL delCriteria = new PR_BILL_OF_MATERIAL();
                delCriteria.FINISHED_PRODUCT_ID = finishedId;
                bomService.Delete(delCriteria);

                lblMessage.Text = "BOM deleted successfully.";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                pnlDetails.Visible = false;
                BindMasterBOMGrid();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error deleting BOM: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    private void LoadRecipeDetails(string finishedId)
    {
        try
        {
            PRODUCTService prodService = new PRODUCTService();

            // Get Finished Good name
            PRODUCT pSearch = new PRODUCT();
            pSearch.PK_ID = finishedId;
            EntityList pRes = (EntityList)prodService.GetAll(pSearch);
            if (pRes != null && pRes.Count > 0)
            {
                PRODUCT fp = (PRODUCT)pRes[0];
                lblDetailProductName.Text = fp.PRODUCT_NAME + " (" + fp.PRODUCT_CODE + ")";
            }
            else
            {
                lblDetailProductName.Text = "Product #" + finishedId;
            }

            // Get BOM rows
            PR_BILL_OF_MATERIALService bomService = new PR_BILL_OF_MATERIALService();
            PR_BILL_OF_MATERIAL bFilter = new PR_BILL_OF_MATERIAL();
            bFilter.FINISHED_PRODUCT_ID = finishedId;
            EntityList materials = (EntityList)bomService.GetAll(bFilter);

            // Fetch Raw Materials into lookup
            PRODUCT rawFilter = new PRODUCT();
            rawFilter.PRODUCT_TYPE_ID = "3";
            EntityList rawList = (EntityList)prodService.GetAll(rawFilter);

            Dictionary<string, PRODUCT> rawLookup = new Dictionary<string, PRODUCT>();
            if (rawList != null)
            {
                foreach (PRODUCT p in rawList)
                {
                    if (!rawLookup.ContainsKey(p.PK_ID))
                    {
                        rawLookup.Add(p.PK_ID, p);
                    }
                }
            }

            // Fetch Units for Mapping
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
            catch { /* Ignore if service call fails, falls back to ID */ }

            DataTable dtDetails = new DataTable();
            dtDetails.Columns.Add("MATERIAL_CODE", typeof(string));
            dtDetails.Columns.Add("MATERIAL_NAME", typeof(string));
            dtDetails.Columns.Add("UNIT", typeof(string));
            dtDetails.Columns.Add("QUANTITY", typeof(string));

            if (materials != null)
            {
                foreach (BILL_OF_MATERIAL m in materials)
                {
                    DataRow dr = dtDetails.NewRow();
                    PRODUCT mat;
                    if (rawLookup.TryGetValue(m.MATERIAL_ID, out mat))
                    {
                        dr["MATERIAL_CODE"] = mat.PRODUCT_CODE;
                        dr["MATERIAL_NAME"] = mat.PRODUCT_NAME;

                        string uName;
                        if (!string.IsNullOrEmpty(mat.UNIT_ID) && unitNames.TryGetValue(mat.UNIT_ID, out uName))
                        {
                            dr["UNIT"] = uName;
                        }
                        else
                        {
                            dr["UNIT"] = mat.UNIT_ID; // Fallback to raw ID
                        }
                    }
                    else
                    {
                        dr["MATERIAL_CODE"] = "-";
                        dr["MATERIAL_NAME"] = "Material #" + m.MATERIAL_ID;
                        dr["UNIT"] = "-";
                    }

                    dr["QUANTITY"] = m.QUANTITY;
                    dtDetails.Rows.Add(dr);
                }
            }

            gvRecipeDetails.DataSource = dtDetails;
            gvRecipeDetails.DataBind();
            pnlDetails.Visible = true;
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading recipe details: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void btnCloseDetails_Click(object sender, EventArgs e)
    {
        pnlDetails.Visible = false;
    }
}