using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class ManageSwatches : System.Web.UI.Page
{
    private Dictionary<string, PR_SWATCH_TYPE> _categoryCache = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFilterCategoryDropdown();
            BindGrid(); // Grid will be hidden initially because filter is empty
        }
    }

    private Dictionary<string, PR_SWATCH_TYPE> GetCategoryCache()
    {
        if (_categoryCache == null)
        {
            _categoryCache = new Dictionary<string, PR_SWATCH_TYPE>();
            try
            {
                PR_SWATCH_TYPEService typeService = new PR_SWATCH_TYPEService();
                EntityList typeList = (EntityList)typeService.GetAll(new PR_SWATCH_TYPE());

                if (typeList != null)
                {
                    foreach (PR_SWATCH_TYPE type in typeList)
                    {
                        if (!_categoryCache.ContainsKey(type.PK_ID))
                        {
                            _categoryCache.Add(type.PK_ID, type);
                        }
                    }
                }
            }
            catch { /* Fail silently, fallback handles it */ }
        }
        return _categoryCache;
    }

    protected string GetCategoryName(object categoryId)
    {
        if (categoryId == null || string.IsNullOrEmpty(categoryId.ToString())) return "-";

        string id = categoryId.ToString();
        Dictionary<string, PR_SWATCH_TYPE> cache = GetCategoryCache();

        if (cache.ContainsKey(id)) return cache[id].SWATCH_NAME;

        return id;
    }

    private void LoadFilterCategoryDropdown()
    {
        try
        {
            PR_SWATCH_TYPEService typeService = new PR_SWATCH_TYPEService();
            EntityList typeList = (EntityList)typeService.GetAll(new PR_SWATCH_TYPE());

            ddlFilterCategory.Items.Clear();
            ddlFilterCategory.Items.Add(new ListItem("-- Select a Category to View Swatches --", ""));

            if (typeList != null)
            {
                foreach (PR_SWATCH_TYPE type in typeList)
                {
                    ddlFilterCategory.Items.Add(new ListItem(type.SWATCH_NAME, type.PK_ID));
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading filters: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    protected void ddlFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
        PopulateHeaderDropdowns();

        // Auto-select the chosen category in the "Add New" header row for convenience
        if (gvSwatches.HeaderRow != null && !string.IsNullOrEmpty(ddlFilterCategory.SelectedValue))
        {
            DropDownList ddlHeaderSwatchType = gvSwatches.HeaderRow.FindControl("ddlHeaderSwatchType") as DropDownList;
            if (ddlHeaderSwatchType != null && ddlHeaderSwatchType.Items.FindByValue(ddlFilterCategory.SelectedValue) != null)
            {
                ddlHeaderSwatchType.SelectedValue = ddlFilterCategory.SelectedValue;
            }
        }
    }

    private void BindGrid()
    {
        try
        {
            if (string.IsNullOrEmpty(ddlFilterCategory.SelectedValue))
            {
                // No category selected: clear and hide the grid
                gvSwatches.DataSource = null;
                gvSwatches.DataBind();
                gvSwatches.Visible = false;
                return;
            }

            PR_SWATCH searchEntity = new PR_SWATCH();
            searchEntity.SWATCH_TYPE_ID = ddlFilterCategory.SelectedValue;

            PR_SWATCHService swatchService = new PR_SWATCHService();
            gvSwatches.DataSource = swatchService.GetAll(searchEntity);
            gvSwatches.DataBind();
            gvSwatches.Visible = true;
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading grid: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    private void PopulateHeaderDropdowns()
    {
        if (!gvSwatches.Visible) return;

        Control headerRow = gvSwatches.HeaderRow;
        if (headerRow != null)
        {
            DropDownList ddlHeaderSwatchType = headerRow.FindControl("ddlHeaderSwatchType") as DropDownList;
            if (ddlHeaderSwatchType != null)
            {
                LoadCategoryDropdown(ddlHeaderSwatchType);
            }
        }
    }

    private void LoadCategoryDropdown(DropDownList ddl)
    {
        try
        {
            PR_SWATCH_TYPEService typeService = new PR_SWATCH_TYPEService();
            EntityList typeList = (EntityList)typeService.GetAll(new PR_SWATCH_TYPE());

            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("", ""));

            if (typeList != null)
            {
                foreach (PR_SWATCH_TYPE type in typeList)
                {
                    ddl.Items.Add(new ListItem(type.SWATCH_NAME, type.PK_ID));
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading categories: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    protected string GetSwatchImageUrl(object pkId)
    {
        if (pkId == null || string.IsNullOrEmpty(pkId.ToString()))
            return ResolveUrl("~/images/profile.png");

        string virtualPath = string.Format("~/images/swatch/{0}.jpg", pkId.ToString());
        string physicalPath = Server.MapPath(virtualPath);

        if (File.Exists(physicalPath))
        {
            return ResolveUrl(virtualPath);
        }
        return ResolveUrl("~/images/profile.png");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Control headerRow = gvSwatches.HeaderRow;
        if (headerRow == null) return;

        TextBox txtSwatchName = headerRow.FindControl("txtSwatchName") as TextBox;
        DropDownList ddlHeaderSwatchType = headerRow.FindControl("ddlHeaderSwatchType") as DropDownList;
        DropDownList ddlHeaderStatus = headerRow.FindControl("ddlHeaderStatus") as DropDownList;

        if (txtSwatchName == null || ddlHeaderSwatchType == null || string.IsNullOrEmpty(txtSwatchName.Text.Trim()) || string.IsNullOrEmpty(ddlHeaderSwatchType.SelectedValue))
        {
            lblMessage.Text = "Please fill in all required fields.";
            lblMessage.CssClass = "text-danger";
            return;
        }

        try
        {
            PR_SWATCH newSwatch = new PR_SWATCH();
            newSwatch.SWATCH_TYPE_ID = ddlHeaderSwatchType.SelectedValue;
            newSwatch.SWATCH_NAME = txtSwatchName.Text.Trim();
            newSwatch.STATUS = ddlHeaderStatus != null ? ddlHeaderStatus.SelectedValue : "1";

            PR_SWATCHService swatchService = new PR_SWATCHService();
            swatchService.Insert(newSwatch);

            // SAVE IMAGE AFTER INSERT (Relies on newSwatch.PK_ID being populated by the ORM/Service)
            FileUpload fuAddImage = headerRow.FindControl("fuAddImage") as FileUpload;
            if (fuAddImage != null && fuAddImage.HasFile)
            {
                try
                {
                    string savePath = Server.MapPath("~/images/swatch/" + newSwatch.PK_ID + ".jpg");
                    fuAddImage.SaveAs(savePath);
                }
                catch (Exception imgEx)
                {
                    lblMessage.Text = "Swatch added, but image failed to save: " + imgEx.Message;
                    lblMessage.CssClass = "text-warning";
                }
            }
            else
            {
                lblMessage.Text = "Swatch added successfully!";
                lblMessage.CssClass = "text-success";
            }

            BindGrid();
            PopulateHeaderDropdowns();

            // Re-select the filter category in the header row after rebinding
            if (gvSwatches.HeaderRow != null && !string.IsNullOrEmpty(ddlFilterCategory.SelectedValue))
            {
                DropDownList rebindDdlHeader = gvSwatches.HeaderRow.FindControl("ddlHeaderSwatchType") as DropDownList;
                if (rebindDdlHeader != null && rebindDdlHeader.Items.FindByValue(ddlFilterCategory.SelectedValue) != null)
                {
                    rebindDdlHeader.SelectedValue = ddlFilterCategory.SelectedValue;
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error saving swatch: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    protected void gvSwatches_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSwatches.EditIndex = e.NewEditIndex;
        BindGrid();
        PopulateHeaderDropdowns();

        GridViewRow row = gvSwatches.Rows[e.NewEditIndex];

        DropDownList ddlEditSwatchType = row.FindControl("ddlEditSwatchType") as DropDownList;
        DropDownList ddlEditStatus = row.FindControl("ddlEditStatus") as DropDownList;

        if (ddlEditSwatchType != null)
        {
            LoadCategoryDropdown(ddlEditSwatchType);

            object typeIdObj = gvSwatches.DataKeys[e.NewEditIndex].Values["SWATCH_TYPE_ID"];
            if (typeIdObj != null)
            {
                string currentTypeId = typeIdObj.ToString();
                if (ddlEditSwatchType.Items.FindByValue(currentTypeId) != null)
                {
                    ddlEditSwatchType.SelectedValue = currentTypeId;
                }
            }
        }

        if (ddlEditStatus != null)
        {
            object statusObj = gvSwatches.DataKeys[e.NewEditIndex].Values["STATUS"];
            if (statusObj != null)
            {
                string currentStatus = statusObj.ToString();
                if (ddlEditStatus.Items.FindByValue(currentStatus) != null)
                {
                    ddlEditStatus.SelectedValue = currentStatus;
                }
            }
        }
    }

    protected void gvSwatches_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvSwatches.EditIndex = -1;
        BindGrid();
        PopulateHeaderDropdowns();
        lblMessage.Text = "";
    }

    protected void gvSwatches_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string id = gvSwatches.DataKeys[e.RowIndex].Value.ToString();
            GridViewRow row = gvSwatches.Rows[e.RowIndex];

            TextBox txtName = row.FindControl("txtSwatchName") as TextBox;
            DropDownList ddlEditSwatchType = row.FindControl("ddlEditSwatchType") as DropDownList;
            DropDownList ddlEditStatus = row.FindControl("ddlEditStatus") as DropDownList;

            PR_SWATCH updateEntity = new PR_SWATCH();
            updateEntity.PK_ID = id;
            if (txtName != null) updateEntity.SWATCH_NAME = txtName.Text.Trim();
            if (ddlEditSwatchType != null) updateEntity.SWATCH_TYPE_ID = ddlEditSwatchType.SelectedValue;
            if (ddlEditStatus != null) updateEntity.STATUS = ddlEditStatus.SelectedValue;

            PR_SWATCHService swatchService = new PR_SWATCHService();
            swatchService.Update(updateEntity);

            // SAVE IMAGE AFTER UPDATE
            FileUpload fuEditImage = row.FindControl("fuEditImage") as FileUpload;
            if (fuEditImage != null && fuEditImage.HasFile)
            {
                try
                {
                    string savePath = Server.MapPath("~/images/swatch/" + id + ".jpg");
                    fuEditImage.SaveAs(savePath); // Overwrites automatically
                }
                catch (Exception imgEx)
                {
                    lblMessage.Text = "Swatch updated, but image failed to save: " + imgEx.Message;
                    lblMessage.CssClass = "text-warning";
                }
            }
            else
            {
                lblMessage.Text = "Swatch updated successfully!";
                lblMessage.CssClass = "text-success";
            }

            gvSwatches.EditIndex = -1;
            BindGrid();
            PopulateHeaderDropdowns();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error updating swatch: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    protected void gvSwatches_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string id = gvSwatches.DataKeys[e.RowIndex].Value.ToString();
            PR_SWATCH deleteEntity = new PR_SWATCH();
            deleteEntity.PK_ID = id;

            PR_SWATCHService swatchService = new PR_SWATCHService();
            swatchService.Delete(deleteEntity);

            // OPTIONAL: Delete the physical image file if it exists
            string physicalPath = Server.MapPath("~/images/swatch/" + id + ".jpg");
            if (File.Exists(physicalPath))
            {
                try { File.Delete(physicalPath); } catch { /* Ignore file lock issues */ }
            }

            lblMessage.Text = "Swatch deleted successfully!";
            lblMessage.CssClass = "text-success";

            BindGrid();
            PopulateHeaderDropdowns();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error deleting swatch: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }
}