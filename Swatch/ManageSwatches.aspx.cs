using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class ManageSwatches : System.Web.UI.Page
{
    PR_SWATCH_TYPEService typeService = new PR_SWATCH_TYPEService();
    PR_SWATCHService swatchService = new PR_SWATCHService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFilterCategoryDropdown();

            if (Request.QueryString["cat"] != null)
            {
                string catId = Request.QueryString["cat"].ToString();
                if (ddlFilterCategory.Items.FindByValue(catId) != null)
                {
                    ddlFilterCategory.SelectedValue = catId;
                }
            }

            BindGrid();
        }
    }

    private void LoadFilterCategoryDropdown()
    {
        try
        {
            EntityList typeList = (EntityList)typeService.GetAll(new PR_SWATCH_TYPE());
            ddlFilterCategory.DataSource = typeList;
            ddlFilterCategory.DataTextField = "SWATCH_NAME";
            ddlFilterCategory.DataValueField = "PK_ID";
            ddlFilterCategory.DataBind();
            ddlFilterCategory.Items.Insert(0, new ListItem("-- Select a Category to View Swatches --", ""));
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Error loading filters: " + ex.Message);
        }
    }

    protected void ddlFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    private void BindGrid()
    {
        try
        {
            if (string.IsNullOrEmpty(ddlFilterCategory.SelectedValue))
            {
                gvSwatches.DataSource = null;
                gvSwatches.DataBind();
                gvSwatches.Visible = false;
                phSearchWrapper.Visible = false; // Hide search box if no category selected
                return;
            }

            phSearchWrapper.Visible = true; // Show search box when category is selected

            PR_SWATCH searchEntity = new PR_SWATCH();
            searchEntity.SWATCH_TYPE_ID = ddlFilterCategory.SelectedValue;

            EntityList swatchList = (EntityList)swatchService.GetAll(searchEntity);

            if (swatchList == null || swatchList.Count == 0)
            {
                swatchList = new EntityList();
                PR_SWATCH dummy = new PR_SWATCH();
                dummy.PK_ID = "";
                dummy.SWATCH_NAME = "";
                dummy.SWATCH_TYPE_ID = ddlFilterCategory.SelectedValue;
                dummy.STATUS = "1";
                swatchList.Add(dummy);
            }

            gvSwatches.DataSource = swatchList;
            gvSwatches.DataBind();
            gvSwatches.Visible = true;
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Error loading grid: " + ex.Message);
        }
    }

    protected void gvSwatches_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Register btnAdd as a full postback trigger for file upload
        if (e.Row.RowType == DataControlRowType.Header)
        {
            Button btnAdd = e.Row.FindControl("btnAdd") as Button;
            if (btnAdd != null)
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(btnAdd);
            }
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string pkId = DataBinder.Eval(e.Row.DataItem, "PK_ID") != null
                            ? DataBinder.Eval(e.Row.DataItem, "PK_ID").ToString()
                            : "";

            if (string.IsNullOrEmpty(pkId))
            {
                e.Row.Visible = false;
            }

            // Register btnUpdate as full postback trigger when row is in edit mode
            LinkButton btnUpdate = e.Row.FindControl("btnUpdate") as LinkButton;
            if (btnUpdate != null)
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(btnUpdate);
            }
        }
    }

    protected string GetSwatchImageUrl(object pkId)
    {
        if (pkId == null || string.IsNullOrEmpty(pkId.ToString()))
            return ResolveUrl("~/images/profile.png");

        string virtualPath = string.Format("~/images/swatches/{0}.jpg", pkId.ToString());
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
        DropDownList ddlHeaderStatus = headerRow.FindControl("ddlHeaderStatus") as DropDownList;

        if (txtSwatchName == null || string.IsNullOrEmpty(txtSwatchName.Text.Trim()) || string.IsNullOrEmpty(ddlFilterCategory.SelectedValue))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please enter a swatch name.");
            return;
        }

        try
        {
            PR_SWATCH newSwatch = new PR_SWATCH();
            // Automatically uses currently selected filter category
            newSwatch.SWATCH_TYPE_ID = ddlFilterCategory.SelectedValue;
            newSwatch.SWATCH_NAME = txtSwatchName.Text.Trim();
            newSwatch.STATUS = ddlHeaderStatus != null ? ddlHeaderStatus.SelectedValue : "1";

            swatchService.Insert(newSwatch);

            string generatedId = newSwatch.PK_ID;
            if (string.IsNullOrEmpty(generatedId))
            {
                EntityList allSwatches = (EntityList)swatchService.GetAll(new PR_SWATCH());
                if (allSwatches != null && allSwatches.Count > 0)
                {
                    generatedId = ((PR_SWATCH)allSwatches[allSwatches.Count - 1]).PK_ID;
                }
            }

            FileUpload fuAddImage = headerRow.FindControl("fuAddImage") as FileUpload;
            if (fuAddImage != null && fuAddImage.HasFile && !string.IsNullOrEmpty(generatedId))
            {
                try
                {
                    string folderPath = Server.MapPath("~/images/swatches/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string savePath = Path.Combine(folderPath, generatedId + ".jpg");
                    fuAddImage.SaveAs(savePath);
                }
                catch { }
            }

            HelperFunction.MsgBox(this, this.GetType(), "Swatch added successfully!");
            BindGrid();
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Error saving swatch: " + ex.Message);
        }
    }

    protected void gvSwatches_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSwatches.EditIndex = e.NewEditIndex;
        BindGrid();

        GridViewRow row = gvSwatches.Rows[e.NewEditIndex];
        DropDownList ddlEditStatus = row.FindControl("ddlEditStatus") as DropDownList;

        if (ddlEditStatus != null)
        {
            object statusObj = gvSwatches.DataKeys[e.NewEditIndex].Values["STATUS"];
            if (statusObj != null && ddlEditStatus.Items.FindByValue(statusObj.ToString()) != null)
            {
                ddlEditStatus.SelectedValue = statusObj.ToString();
            }
        }
    }

    protected void gvSwatches_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvSwatches.EditIndex = -1;
        BindGrid();
    }

    protected void gvSwatches_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string id = gvSwatches.DataKeys[e.RowIndex].Value.ToString();
            GridViewRow row = gvSwatches.Rows[e.RowIndex];

            TextBox txtName = row.FindControl("txtSwatchName") as TextBox;
            DropDownList ddlEditStatus = row.FindControl("ddlEditStatus") as DropDownList;

            PR_SWATCH updateEntity = new PR_SWATCH();
            updateEntity.PK_ID = id;
            if (txtName != null) updateEntity.SWATCH_NAME = txtName.Text.Trim();
            if (ddlEditStatus != null) updateEntity.STATUS = ddlEditStatus.SelectedValue;

            swatchService.Update(updateEntity);

            FileUpload fuEditImage = row.FindControl("fuEditImage") as FileUpload;
            if (fuEditImage != null && fuEditImage.HasFile)
            {
                try
                {
                    string savePath = Server.MapPath("~/images/swatches/" + id + ".jpg");
                    fuEditImage.SaveAs(savePath);
                }
                catch { }
            }

            HelperFunction.MsgBox(this, this.GetType(), "Swatch updated successfully!");
            gvSwatches.EditIndex = -1;
            BindGrid();
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Error updating swatch: " + ex.Message);
        }
    }

    protected void gvSwatches_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string id = gvSwatches.DataKeys[e.RowIndex].Value.ToString();
            PR_SWATCH deleteEntity = new PR_SWATCH();
            deleteEntity.PK_ID = id;

            swatchService.Delete(deleteEntity);

            string physicalPath = Server.MapPath("~/images/swatches/" + id + ".jpg");
            if (File.Exists(physicalPath))
            {
                try { File.Delete(physicalPath); } catch { }
            }

            HelperFunction.MsgBox(this, this.GetType(), "Swatch deleted successfully!");
            BindGrid();
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Error deleting swatch: " + ex.Message);
        }
    }
}