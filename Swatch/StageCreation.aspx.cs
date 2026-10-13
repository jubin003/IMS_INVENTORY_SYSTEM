using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Framework;
using Entity.Components;
using Service.Components;

public partial class Swatch_StageCreation : System.Web.UI.Page
{
    PR_STAGE_MASTER SmEnt = new PR_STAGE_MASTER();
    PR_STAGE_MASTERService SmSer = new PR_STAGE_MASTERService();

    EntityList theList = new EntityList();

    private const string IconFolder = "~/images/icons/";
    private static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg", ".gif" };

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadGrid();
    }

    protected void LoadGrid()
    {
        SmEnt = new PR_STAGE_MASTER();
        theList = SmSer.GetAll(SmEnt);

        if (theList.Count == 0)
            theList.Add(SmEnt);

        grdStage.DataSource = theList;
        grdStage.DataBind();
    }


    protected void grdStage_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdStage.EditIndex = -1;
        grdStage.PageIndex = e.NewPageIndex;
        LoadGrid();
    }

    protected void grdStage_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        Label lblPK_ID = (Label)e.Row.FindControl("lblPK_ID");
        bool isPlaceholder = (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text));

        if ((e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            Label lblStatusE = (Label)e.Row.FindControl("lblStatusE");
            DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");

            if (lblStatusE != null && ddlStatus != null && ddlStatus.Items.FindByValue(lblStatusE.Text) != null)
                ddlStatus.SelectedValue = lblStatusE.Text;
        }
        else
        {
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            Label lblStatusShow = (Label)e.Row.FindControl("lblStatusShow");

            if (lblStatus != null && lblStatusShow != null && !string.IsNullOrEmpty(lblStatus.Text))
            {
                if (lblStatus.Text == "1")
                {
                    lblStatusShow.Text = "Active";
                    lblStatusShow.CssClass = "status-pill status-active";
                }
                else
                {
                    lblStatusShow.Text = "Inactive";
                    lblStatusShow.CssClass = "status-pill status-inactive";
                }
            }

            if (isPlaceholder)
            {
                ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
                System.Web.UI.WebControls.Image imgIcon = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgIcon");

                if (btnEdit != null) btnEdit.Visible = false;
                if (btnDelete != null) btnDelete.Visible = false;
                if (imgIcon != null) imgIcon.Visible = false;
            }
        }
    }

    protected void grdStage_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdStage.EditIndex = e.NewEditIndex;
        LoadGrid();
    }

    protected void grdStage_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdStage.EditIndex = -1;
        LoadGrid();
    }

    protected void grdStage_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = grdStage.Rows[e.RowIndex];

        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");
        TextBox txtName = (TextBox)row.FindControl("txtNameE");
        TextBox txtDescription = (TextBox)row.FindControl("txtDescriptionE");
        DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
        FileUpload fuIcon = (FileUpload)row.FindControl("fuIconE");

        SmEnt = new PR_STAGE_MASTER();
        SmEnt.PK_ID = lblPK_ID.Text;
        SmEnt = (PR_STAGE_MASTER)SmSer.GetSingle(SmEnt);

        if (SmEnt != null)
        {
            SmEnt.NAME = txtName.Text.Trim();
            SmEnt.DESCRIPTION = txtDescription.Text.Trim();
            SmEnt.STATUS = ddlStatus.SelectedValue;

            if (fuIcon.HasFile)
            {
                SaveIcon(fuIcon, lblPK_ID.Text);
                SmEnt.ICON = lblPK_ID.Text;  
            }

            SmSer.Update(SmEnt);

            HelperFunction.MsgBox(this, this.GetType(), "Updated");
        }

        grdStage.EditIndex = -1;
        LoadGrid();
    }

    protected void grdStage_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = grdStage.Rows[e.RowIndex];

        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");

        if (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text))
            return;

        SmEnt = new PR_STAGE_MASTER();
        SmEnt.PK_ID = lblPK_ID.Text;
        SmEnt = (PR_STAGE_MASTER)SmSer.GetSingle(SmEnt);

        if (SmEnt != null)
        {
            SmSer.Delete(SmEnt);
            DeleteIcon(lblPK_ID.Text);

            HelperFunction.MsgBox(this, this.GetType(), "Deleted");
        }

        grdStage.EditIndex = -1;
        LoadGrid();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = grdStage.HeaderRow;

        FileUpload fuIcon = (FileUpload)row.FindControl("fuIcon");
        TextBox txtName = (TextBox)row.FindControl("txtName");
        TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
        DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatusH");

        SmEnt = new PR_STAGE_MASTER();
        SmEnt.NAME = txtName.Text.Trim();
        SmEnt.DESCRIPTION = txtDescription.Text.Trim();
        SmEnt.STATUS = ddlStatus.SelectedValue;

        SmSer.Insert(SmEnt);

        if (fuIcon.HasFile)
        {
            string newId = SmEnt.PK_ID;

            if (string.IsNullOrEmpty(newId))
            {
                PR_STAGE_MASTER filter = new PR_STAGE_MASTER();
                filter.NAME = txtName.Text.Trim();
                newId = MaxStageId(SmSer.GetAll(filter)).ToString();
            }

            SaveIcon(fuIcon, newId);

            PR_STAGE_MASTER saved = new PR_STAGE_MASTER();
            saved.PK_ID = newId;
            saved = (PR_STAGE_MASTER)SmSer.GetSingle(saved);

            saved.ICON = newId;
            SmSer.Update(saved);
        }

        HelperFunction.MsgBox(this, this.GetType(), "Inserted");

        LoadGrid();
    }

    protected string GetStageIconUrl(object iconId)
    {
        string id = iconId == null ? "" : iconId.ToString();

        if (!string.IsNullOrEmpty(id))
        {
            string virtualPath = IconFolder + id + ".png";

            if (File.Exists(Server.MapPath(virtualPath)))
                return ResolveUrl(virtualPath);
        }

        return ResolveUrl("~/images/profile.png");
    }

    private bool IsValidIcon(FileUpload fu)
    {
        string ext = Path.GetExtension(fu.FileName).ToLower();
        return Array.IndexOf(AllowedExtensions, ext) >= 0;
    }

    private void SaveIcon(FileUpload fu, string id)
    {
        string folder = Server.MapPath(IconFolder);

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string path = Path.Combine(folder, id + ".png");

        using (System.Drawing.Image img = System.Drawing.Image.FromStream(fu.PostedFile.InputStream))
        {
            img.Save(path, ImageFormat.Png);
        }
    }

    private void DeleteIcon(string id)
    {
        string path = Server.MapPath(IconFolder + id + ".png");

        if (File.Exists(path))
            File.Delete(path);
    }

    private long MaxStageId(EntityList list)
    {
        long max = 0;

        foreach (PR_STAGE_MASTER r in list)
        {
            long v;
            if (long.TryParse(r.PK_ID, out v) && v > max)
                max = v;
        }

        return max;
    }
}