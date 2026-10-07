using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using PhyeGanCore;


public partial class FixedAsset_MasterData_Asset_SubCategory : System.Web.UI.Page
{
    GL_ACCOUNT GAEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GASer = new GL_ACCOUNTService();

    ASSET_SUB_CATEGORY ASCEnt = new ASSET_SUB_CATEGORY();
    ASSET_SUB_CATEGORYService ASCSer = new ASSET_SUB_CATEGORYService();

    ASSET_CATEGORY ACEnt = new ASSET_CATEGORY();
    ASSET_CATEGORYService ACSer = new ASSET_CATEGORYService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            LoadAssetCategory();

        }
    }
    private void LoadData()
    {
        ASCEnt = new ASSET_SUB_CATEGORY();
        ASCEnt.CATEGORY_ID = ddlCategory.SelectedValue;
        gridSubCategory.DataSource = ASCSer.GetAll(ASCEnt);
        gridSubCategory.DataBind();

        if (gridSubCategory.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            ASCEnt = new ASSET_SUB_CATEGORY();
            ASCEnt.CATEGORY_ID = ddlCategory.SelectedValue;
            a1.Add(ASCEnt);

            gridSubCategory.DataSource = a1;
            gridSubCategory.DataBind();
        }
    }


    protected void LoadAssetCategory()
    {
        ACEnt = new ASSET_CATEGORY();
        ACEnt.STATUS = "1";
        ddlCategory.DataSource = ACSer.GetAll(ACEnt);
        ddlCategory.DataTextField = "CATEGORY_NAME";
        ddlCategory.DataValueField = "PK_ID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, "Select");

    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        if (ddlCategory.SelectedIndex == 0)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please Select Category.");
        }
        else
        {
            LoadData();

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridSubCategory.HeaderRow;

        TextBox txtSubCategoryName = (TextBox)row.FindControl("txtSubCategoryName");
        DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");

        if (string.IsNullOrEmpty(txtSubCategoryName.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Sub-Category Name can not be empty.");
        }
        else
        {
            ASCEnt = new ASSET_SUB_CATEGORY();
            ASCEnt.CATEGORY_ID = ddlCategory.SelectedValue;
            ASCEnt.SUB_CATEGORY_NAME = txtSubCategoryName.Text;
            ASCEnt.STATUS = ddlStatus.SelectedValue;

            ASCSer.Insert(ASCEnt);
            LoadData();
        }
    }

    protected void gridSubCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridSubCategory.Rows[e.RowIndex];

        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");
        TextBox txtSubCategoryNameE = (TextBox)row.FindControl("txtSubCategoryNameE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");

        ASCEnt = new ASSET_SUB_CATEGORY();
        ASCEnt.PK_ID = lblPK_ID.Text;
        ASCEnt = (ASSET_SUB_CATEGORY)ASCSer.GetSingle(ASCEnt);
        if (ASCEnt != null)
        {
            ASCEnt.SUB_CATEGORY_NAME = txtSubCategoryNameE.Text;
            ASCEnt.STATUS = ddlStatusE.SelectedValue;

            ASCSer.Update(ASCEnt);
        }

        gridSubCategory.EditIndex = -1;
        LoadData();
    }

    protected void gridSubCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridSubCategory.EditIndex = -1;
        LoadData();
    }

    protected void gridSubCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridSubCategory.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void gridSubCategory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                Label lblStatusE = (Label)e.Row.FindControl("lblStatusE");
                DropDownList ddlStatusE = (DropDownList)e.Row.FindControl("ddlStatusE");

                ddlStatusE.SelectedValue = lblStatusE.Text;
            }
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Label lblStatusShow = (Label)e.Row.FindControl("lblStatusShow");

                if (lblStatus.Text == "1")
                    lblStatusShow.Text = "Available";
                else
                    lblStatusShow.Text = "UnAvailable";

            }
        }
    }
}