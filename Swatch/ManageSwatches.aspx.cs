using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class ManageSwatches : System.Web.UI.Page
{
    PR_SWATCH SEnt = new PR_SWATCH();
    PR_SWATCHService SSer = new PR_SWATCHService();

    PR_SWATCH_TYPE STEnt = new PR_SWATCH_TYPE();
    PR_SWATCH_TYPEService STSer = new PR_SWATCH_TYPEService();

    EntityList theList = new EntityList();

    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
    private const string SwatchFolder = "~/images/swatches/";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.Form != null)
        {
            Page.Form.Enctype = "multipart/form-data";
        }

        if (!IsPostBack)
        {
            LoadFilterCategoryDropdown();

            if (Request.QueryString["cat"] != null)
            {
                ddlFilterCategory.SelectedValue = Request.QueryString["cat"];
            }

            BindGrid();
        }
    }

    private void LoadFilterCategoryDropdown()
    {
        STEnt = new PR_SWATCH_TYPE();
        theList = (EntityList)STSer.GetAll(STEnt);
        ddlFilterCategory.DataSource = theList;
        ddlFilterCategory.DataTextField = "SWATCH_NAME";
        ddlFilterCategory.DataValueField = "PK_ID";
        ddlFilterCategory.DataBind();
        ddlFilterCategory.Items.Insert(0, new ListItem("-- Select a Category to View Swatches --", ""));
    }

    protected void ddlFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvSwatches.EditIndex = -1;
        BindGrid();
    }

    private void BindGrid()
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

        SEnt = new PR_SWATCH();
        SEnt.SWATCH_TYPE_ID = ddlFilterCategory.SelectedValue;
        theList = (EntityList)SSer.GetAll(SEnt);

        if (theList.Count == 0)
        {
            SEnt = new PR_SWATCH();
            SEnt.PK_ID = "";
            SEnt.SWATCH_NAME = "";
            SEnt.SWATCH_CODE = "";
            SEnt.SWATCH_TYPE_ID = ddlFilterCategory.SelectedValue;
            SEnt.STATUS = "1";
            theList.Add(SEnt);
        }

        gvSwatches.DataSource = theList;
        gvSwatches.DataBind();
    }

    protected void gvSwatches_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            SEnt = (PR_SWATCH)e.Row.DataItem;
            if (string.IsNullOrEmpty(SEnt.PK_ID))
            {
                e.Row.Visible = false;
            }
        }
    }

    private void SaveSwatchImage(string id, FileUpload fu)
    {
        if (!fu.HasFile || string.IsNullOrEmpty(id)) return;

        string folderPath = Server.MapPath(SwatchFolder);
        string ext = Path.GetExtension(fu.FileName).ToLower();

        foreach (string allowed in AllowedExtensions)
        {
            string oldFile = Path.Combine(folderPath, id + allowed);
            if (File.Exists(oldFile))
            {
                File.Delete(oldFile);
            }
        }

        fu.SaveAs(Path.Combine(folderPath, id + ext));
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
                return ResolveUrl(virtualPath) + "?v=" + File.GetLastWriteTime(physicalPath).Ticks;
            }
        }
        return ResolveUrl("~/images/profile.png");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow headerRow = gvSwatches.HeaderRow;

        TextBox txtSwatchName = (TextBox)headerRow.FindControl("txtSwatchName");
        TextBox txtSwatchCode = (TextBox)headerRow.FindControl("txtSwatchCode");
        DropDownList ddlHeaderStatus = (DropDownList)headerRow.FindControl("ddlHeaderStatus");
        FileUpload fuAddImage = (FileUpload)headerRow.FindControl("fuAddImage");

        SEnt = new PR_SWATCH();
        SEnt.SWATCH_TYPE_ID = ddlFilterCategory.SelectedValue;
        SEnt.SWATCH_NAME = txtSwatchName.Text.Trim();
        SEnt.SWATCH_CODE = txtSwatchCode.Text.Trim();
        SEnt.STATUS = ddlHeaderStatus.SelectedValue;

        SSer.Insert(SEnt);

        SaveSwatchImage(SEnt.PK_ID, fuAddImage);

        HelperFunction.MsgBox(this, this.GetType(), "Swatch added successfully!");
        BindGrid();
    }

    protected void gvSwatches_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSwatches.EditIndex = e.NewEditIndex;
        BindGrid();

        DropDownList ddlEditStatus = (DropDownList)gvSwatches.Rows[e.NewEditIndex].FindControl("ddlEditStatus");
        ddlEditStatus.SelectedValue = gvSwatches.DataKeys[e.NewEditIndex].Values["STATUS"].ToString();
    }

    protected void gvSwatches_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvSwatches.EditIndex = -1;
        BindGrid();
    }

    protected void gvSwatches_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gvSwatches.Rows[e.RowIndex];

        TextBox txtName = (TextBox)row.FindControl("txtSwatchNameE");
        TextBox txtCode = (TextBox)row.FindControl("txtSwatchCodeE");
        DropDownList ddlEditStatus = (DropDownList)row.FindControl("ddlEditStatus");
        FileUpload fuEditImage = (FileUpload)row.FindControl("fuEditImage");

        SEnt = new PR_SWATCH();
        SEnt.PK_ID = gvSwatches.DataKeys[e.RowIndex].Value.ToString();
        SEnt = (PR_SWATCH)SSer.GetSingle(SEnt);

        SEnt.SWATCH_NAME = txtName.Text.Trim();
        SEnt.SWATCH_CODE = txtCode.Text.Trim();
        SEnt.STATUS = ddlEditStatus.SelectedValue;

        SSer.Update(SEnt);

        SaveSwatchImage(SEnt.PK_ID, fuEditImage);

        HelperFunction.MsgBox(this, this.GetType(), "Swatch updated successfully!");
        gvSwatches.EditIndex = -1;
        BindGrid();
    }
}