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

    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
    private const string SwatchFolder = "~/images/swatches/";

    protected void Page_Load(object sender, EventArgs e)
    {
        // File uploads need a multipart form
        if (Page.Form != null)
        {
            Page.Form.Enctype = "multipart/form-data";
        }

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

    // ---------------------------------------------------------------
    // Loading
    // ---------------------------------------------------------------

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
            HelperFunction.MsgBox(this, this.GetType(), "Error loading filters: " + CleanMsg(ex.Message));
        }
    }

    protected void ddlFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvSwatches.EditIndex = -1;
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
                pnlSwatchesGrid.Visible = false;
                phSearchWrapper.Visible = false;
                return;
            }

            pnlSwatchesGrid.Visible = true;
            phSearchWrapper.Visible = true;

            PR_SWATCH searchEntity = new PR_SWATCH();
            searchEntity.SWATCH_TYPE_ID = ddlFilterCategory.SelectedValue;

            EntityList swatchList = (EntityList)swatchService.GetAll(searchEntity);

            // Placeholder row so the header (with the add controls) still shows for an empty category
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
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Error loading grid: " + CleanMsg(ex.Message));
        }
    }

    protected void gvSwatches_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            object pk = DataBinder.Eval(e.Row.DataItem, "PK_ID");
            if (pk == null || string.IsNullOrEmpty(pk.ToString()))
            {
                e.Row.Visible = false;
            }
        }
    }

    private static string CleanMsg(string message)
    {
        if (message == null) return "";
        return message.Replace("\\", "/").Replace("'", "").Replace("\r", " ").Replace("\n", " ");
    }

    private string GetValidImageExtension(FileUpload fu)
    {
        string extension = Path.GetExtension(fu.FileName).ToLower();
        foreach (string allowed in AllowedExtensions)
        {
            if (extension == allowed) return extension;
        }
        return null;
    }

    /// <summary>Saves the upload as {id}{ext} and removes any older image for the same swatch (e.g. an old .png when a .jpg is uploaded).</summary>
    private void SaveSwatchImage(string id, FileUpload fu, string extension)
    {
        string folderPath = Server.MapPath(SwatchFolder);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        foreach (string ext in AllowedExtensions)
        {
            string oldFile = Path.Combine(folderPath, id + ext);
            if (File.Exists(oldFile))
            {
                File.Delete(oldFile);
            }
        }

        fu.SaveAs(Path.Combine(folderPath, id + extension));
    }

    protected string GetSwatchImageUrl(object pkId)
    {
        if (pkId == null || string.IsNullOrEmpty(pkId.ToString()))
            return ResolveUrl("~/images/profile.png");

        foreach (string ext in AllowedExtensions)
        {
            string virtualPath = SwatchFolder + pkId.ToString() + ext;
            string physicalPath = Server.MapPath(virtualPath);

            if (File.Exists(physicalPath))
            {
                // Version stamp so a replaced image is not served from the browser cache
                return ResolveUrl(virtualPath) + "?v=" + File.GetLastWriteTime(physicalPath).Ticks;
            }
        }
        return ResolveUrl("~/images/profile.png");
    }

    // ---------------------------------------------------------------
    // Add
    // ---------------------------------------------------------------

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Control headerRow = gvSwatches.HeaderRow;
        if (headerRow == null) return;

        TextBox txtSwatchName = headerRow.FindControl("txtSwatchName") as TextBox;
        DropDownList ddlHeaderStatus = headerRow.FindControl("ddlHeaderStatus") as DropDownList;
        FileUpload fuAddImage = headerRow.FindControl("fuAddImage") as FileUpload;

        if (txtSwatchName == null || string.IsNullOrEmpty(txtSwatchName.Text.Trim()) || string.IsNullOrEmpty(ddlFilterCategory.SelectedValue))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please enter a swatch name.");
            return;
        }

        // Validate the image BEFORE inserting, so a swatch is never created with a missing/invalid image
        if (fuAddImage == null || !fuAddImage.HasFile)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please select an image.");
            return;
        }

        string extension = GetValidImageExtension(fuAddImage);
        if (extension == null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Only JPG, JPEG and PNG images are allowed.");
            return;
        }

        try
        {
            PR_SWATCH newSwatch = new PR_SWATCH();
            newSwatch.SWATCH_TYPE_ID = ddlFilterCategory.SelectedValue;
            newSwatch.SWATCH_NAME = txtSwatchName.Text.Trim();
            newSwatch.STATUS = ddlHeaderStatus != null ? ddlHeaderStatus.SelectedValue : "1";

            swatchService.Insert(newSwatch);

            string generatedId = newSwatch.PK_ID;
            if (string.IsNullOrEmpty(generatedId))
            {
                // Find the row just inserted: same category + name, highest numeric PK_ID
                PR_SWATCH lookup = new PR_SWATCH();
                lookup.SWATCH_TYPE_ID = ddlFilterCategory.SelectedValue;
                EntityList found = (EntityList)swatchService.GetAll(lookup);
                long best = -1;
                if (found != null)
                {
                    foreach (PR_SWATCH s in found)
                    {
                        long n;
                        if (s.SWATCH_NAME == newSwatch.SWATCH_NAME && long.TryParse(s.PK_ID, out n) && n > best)
                        {
                            best = n;
                            generatedId = s.PK_ID;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(generatedId))
            {
                HelperFunction.MsgBox(this, this.GetType(), "Swatch saved, but its ID could not be determined, so the image was not uploaded. Please edit the swatch to upload the image.");
                BindGrid();
                return;
            }

            try
            {
                SaveSwatchImage(generatedId, fuAddImage, extension);
            }
            catch (Exception imgEx)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Swatch saved, but the image could not be saved: " + CleanMsg(imgEx.Message) + " Use Edit on the swatch to upload the image again.");
                BindGrid();
                return;
            }

            HelperFunction.MsgBox(this, this.GetType(), "Swatch added successfully!");
            BindGrid();
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Error saving swatch: " + CleanMsg(ex.Message));
        }
    }

    // ---------------------------------------------------------------
    // Edit / Update
    // ---------------------------------------------------------------

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

            TextBox txtName = row.FindControl("txtSwatchNameE") as TextBox;
            DropDownList ddlEditStatus = row.FindControl("ddlEditStatus") as DropDownList;
            FileUpload fuEditImage = row.FindControl("fuEditImage") as FileUpload;

            string extension = null;
            bool hasNewImage = fuEditImage != null && fuEditImage.HasFile;
            if (hasNewImage)
            {
                extension = GetValidImageExtension(fuEditImage);
                if (extension == null)
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Only JPG, JPEG and PNG images are allowed.");
                    return;
                }
            }

            PR_SWATCH updateEntity = new PR_SWATCH();
            updateEntity.PK_ID = id;
            updateEntity = (PR_SWATCH)swatchService.GetSingle(updateEntity);

            if (txtName != null) updateEntity.SWATCH_NAME = txtName.Text.Trim();
            if (ddlEditStatus != null) updateEntity.STATUS = ddlEditStatus.SelectedValue;

            swatchService.Update(updateEntity);

            if (hasNewImage)
            {
                try
                {
                    SaveSwatchImage(id, fuEditImage, extension);
                }
                catch (Exception imgEx)
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Swatch updated, but the image could not be saved: " + CleanMsg(imgEx.Message));
                    gvSwatches.EditIndex = -1;
                    BindGrid();
                    return;
                }
            }

            HelperFunction.MsgBox(this, this.GetType(), "Swatch updated successfully!");
            gvSwatches.EditIndex = -1;
            BindGrid();
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Error updating swatch: " + CleanMsg(ex.Message));
        }
    }
}