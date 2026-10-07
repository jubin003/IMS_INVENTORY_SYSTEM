using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using PhyeGanCore;

public partial class Administration_Product_SubCategory : System.Web.UI.Page
{
    PRODUCT_CATEGORY PCEnt = new PRODUCT_CATEGORY();
    PRODUCT_CATEGORYService PCSer = new PRODUCT_CATEGORYService();

    PRODUCT_SUB_CATEGORY PSCEnt = new PRODUCT_SUB_CATEGORY();
    PRODUCT_SUB_CATEGORYService PSCSer = new PRODUCT_SUB_CATEGORYService();
    PhyeGan PG = new PhyeGan();
    HelperFunction hf = new HelperFunction();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCategory();
        }
    }

    protected void LoadCategory()
    {
        PCEnt = new PRODUCT_CATEGORY();
        ddlCategory.DataSource = PCSer.GetAll(PCEnt);
        ddlCategory.DataTextField = "CATEGORY_NAME";
        ddlCategory.DataValueField = "PK_ID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, "Select");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = grid.HeaderRow;
        TextBox txtSubCategoryCodeH = (TextBox)row.FindControl("txtSubCategoryCodeH");
        TextBox txtSubCategoryH = (TextBox)row.FindControl("txtSubCategoryH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");


        PSCEnt = new PRODUCT_SUB_CATEGORY();
        PSCEnt.CATEGORY_ID = ddlCategory.SelectedValue;
        PSCEnt.SUB_CATEGORY_CODE = txtSubCategoryCodeH.Text;
        PSCEnt.SUB_CATEGORY_NAME = txtSubCategoryH.Text;
        PSCEnt.STATUS = ddlStatusH.SelectedValue;
        string pk_id = PSCSer.Insert(PSCEnt).ToString();        
        LoadData();
    }
    private void LoadData()
    {
        PSCEnt = new PRODUCT_SUB_CATEGORY();
        PSCEnt.CATEGORY_ID = ddlCategory.SelectedValue;
        grid.DataSource = PSCSer.GetAll(PSCEnt);
        grid.DataBind();

        if (grid.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            PSCEnt = new PRODUCT_SUB_CATEGORY();
            a1.Add(PSCEnt);

            grid.DataSource = a1;
            grid.DataBind();
        }
    }

    protected void grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grid.EditIndex = e.NewEditIndex;
        LoadData();
    }
    protected void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = grid.Rows[e.RowIndex];
        Label lblPKIDE = (Label)row.FindControl("lblPKIDE");
        DropDownList ddlCategoryE = (DropDownList)row.FindControl("ddlCategoryE");
        TextBox txtSubCategoryCodeE = (TextBox)row.FindControl("txtSubCategoryCodeE");
        TextBox txtSubCategoryE = (TextBox)row.FindControl("txtSubCategoryE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");

        PSCEnt = new PRODUCT_SUB_CATEGORY();

        PSCEnt.PK_ID = lblPKIDE.Text;
        PSCEnt = (PRODUCT_SUB_CATEGORY)PSCSer.GetSingle(PSCEnt);
        if (PSCEnt != null)
        {
            PSCEnt.CATEGORY_ID = ddlCategoryE.SelectedValue;
            PSCEnt.SUB_CATEGORY_CODE = txtSubCategoryCodeE.Text;
            PSCEnt.SUB_CATEGORY_NAME = txtSubCategoryE.Text;
            PSCEnt.STATUS = ddlStatusE.SelectedValue;
            PSCSer.Update(PSCEnt);
        }

        grid.EditIndex = -1;
        LoadData();
    }

    protected void grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grid.EditIndex = -1;
        LoadData();
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        #region Header
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DropDownList ddlCategoryH = (DropDownList)e.Row.FindControl("ddlCategoryH");
            TextBox txtSubCategoryCodeH = e.Row.FindControl("txtSubCategoryCodeH") as TextBox;

            PCEnt = new PRODUCT_CATEGORY();
            ddlCategoryH.DataSource = PCSer.GetAll(PCEnt);
            ddlCategoryH.DataTextField = "CATEGORY_NAME";
            ddlCategoryH.DataValueField = "PK_ID";
            ddlCategoryH.DataBind();
            ddlCategoryH.SelectedValue = ddlCategory.SelectedValue;

            txtSubCategoryCodeH.Text = Convert.ToDouble(hf.getSubCategoryCode(ddlCategoryH.SelectedValue)).ToString("000");
         

        }
        #endregion


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                Label lblCategoryE = (Label)e.Row.FindControl("lblCategoryE");
                Label lblPKIDE = (Label)e.Row.FindControl("lblPKIDE");
                DropDownList ddlCategoryE = (DropDownList)e.Row.FindControl("ddlCategoryE");
                DropDownList ddlStatusE = (DropDownList)e.Row.FindControl("ddlStatusE");

                PCEnt = new PRODUCT_CATEGORY();
                ddlCategoryE.DataSource = PCSer.GetAll(PCEnt);
                ddlCategoryE.DataTextField = "CATEGORY_NAME";
                ddlCategoryE.DataValueField = "PK_ID";
                ddlCategoryE.DataBind();
                ddlCategoryE.SelectedValue = lblCategoryE.Text;

                PSCEnt = new PRODUCT_SUB_CATEGORY();
                PSCEnt.PK_ID = lblPKIDE.Text;
                PSCEnt = (PRODUCT_SUB_CATEGORY)PSCSer.GetSingle(PSCEnt);
                if (PSCEnt != null)
                {
                    ddlStatusE.SelectedValue = PSCEnt.STATUS;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblCategory = (Label)e.Row.FindControl("lblCategory");
                Label lblCategoryName = (Label)e.Row.FindControl("lblCategoryName");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Label lblStatusShow = (Label)e.Row.FindControl("lblStatusShow");


                PCEnt = new PRODUCT_CATEGORY();
                PCEnt.PK_ID = lblCategory.Text;
                PCEnt = (PRODUCT_CATEGORY)PCSer.GetSingle(PCEnt);
                if (PCEnt != null)
                {
                    lblCategoryName.Text = PCEnt.CATEGORY_NAME;
                }

              
                if (lblStatus.Text == "1")
                    lblStatusShow.Text = "Active";
                else
                    lblStatusShow.Text = "Inactive";

            }
        }
    }
    

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        LoadData();
    } 
}