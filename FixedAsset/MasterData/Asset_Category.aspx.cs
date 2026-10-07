using System;
using System.Web.UI.WebControls;
using System.Collections;
using Entity.Components;
using Service.Components;
using System.Data.SqlClient;
using System.Configuration;
using PhyeGanCore;
using DataHelper.Framework;


public partial class FixedAsset_MasterData_Asset_Category : System.Web.UI.Page
{
    ASSET_CATEGORY ACEnt = new ASSET_CATEGORY();
    ASSET_CATEGORYService ACSer = new ASSET_CATEGORYService();

    GL_ACCOUNT GAEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GASer = new GL_ACCOUNTService();

    PhyeGan PG = new PhyeGan();
    HelperFunction hf = new HelperFunction();
    
    
    AccountFunction af = new AccountFunction();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        DistributedTransaction DT = new DistributedTransaction();
        GridViewRow row = gridProductCategory.HeaderRow;
        TextBox txtPrefix = (TextBox)row.FindControl("txtPrefix");
        TextBox txtCategoryNameH = (TextBox)row.FindControl("txtCategoryNameH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");

        if (string.IsNullOrEmpty(txtCategoryNameH.Text) && string.IsNullOrEmpty(txtPrefix.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Category Code and Name can not be empty.");
        }
        else
        {
            if (txtPrefix.Text.Length == 3)
            {
                GAEnt = new GL_ACCOUNT();
                GAEnt.GL_MASTER_CODE = "0102";
                GAEnt.EDITABLE = "1";
                GAEnt.GL_NAME = txtCategoryNameH.Text;
                GAEnt.GL_TYPE = "G";
                GAEnt.SUB_LEDGER = "1";
                GAEnt.STATUS = "1";
                string code = af.getGLCode("0102");
                GAEnt.GL_CODE = Convert.ToDouble(code).ToString("000000");
                string glCode = GASer.Insert(GAEnt,DT).ToString();

                ACEnt = new ASSET_CATEGORY();
                ACEnt.CATEGORY_NAME = txtCategoryNameH.Text;
                ACEnt.PREFIX = txtPrefix.Text.ToUpper();
                ACEnt.STATUS = ddlStatusH.SelectedValue;
                ACEnt.GL_CODE = GAEnt.GL_CODE;
                string pk_id = ACSer.Insert(ACEnt,DT).ToString();
               
                if (DT.HAPPY)
                {
                    DT.Commit();
                    HelperFunction.MsgBox(this, this.GetType(), "Successfully Inserted");

                }
                else
                {
                    DT.Abort();
                    HelperFunction.MsgBox(this, this.GetType(), "Something went wrong");
                }
                LoadData();
            }
            else
            {
                HelperFunction.MsgBox(this, this.GetType(), "Category code can be only of 3 character");
            }
        }
        DT.Dispose();
    }
    private void LoadData()
    {
        ACEnt = new ASSET_CATEGORY();
        gridProductCategory.DataSource = ACSer.GetAll(ACEnt);
        gridProductCategory.DataBind();

        if (gridProductCategory.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            ACEnt = new ASSET_CATEGORY();
            a1.Add(ACEnt);

            gridProductCategory.DataSource = a1;
            gridProductCategory.DataBind();
        }
    }

    protected void gridProductCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridProductCategory.EditIndex = e.NewEditIndex;
        LoadData();
    }
    protected void gridProductCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridProductCategory.Rows[e.RowIndex];
        Label lblPKIDE = (Label)row.FindControl("lblPKIDE");
        Label lblGlCode = (Label)row.FindControl("lblGlCode");
        TextBox txtPrefix = (TextBox)row.FindControl("txtPrefix");
        TextBox txtCategoryNameE = (TextBox)row.FindControl("txtCategoryNameE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");


        GAEnt = new GL_ACCOUNT();
        GAEnt.GL_CODE = lblGlCode.Text;
        GAEnt = (GL_ACCOUNT)GASer.GetSingle(GAEnt);
        if (GAEnt != null)
        {
            GAEnt.GL_NAME = txtCategoryNameE.Text;
            GAEnt.STATUS = ddlStatusE.SelectedValue;
            GASer.Update(GAEnt);
        }



        ACEnt = new ASSET_CATEGORY();
        ACEnt.PK_ID = lblPKIDE.Text;
        ACEnt = (ASSET_CATEGORY)ACSer.GetSingle(ACEnt);
        if (ACEnt != null)
        {
            ACEnt.PREFIX = txtPrefix.Text.ToUpper();
            ACEnt.CATEGORY_NAME = txtCategoryNameE.Text;
            ACEnt.STATUS = ddlStatusE.SelectedValue;
            ACSer.Update(ACEnt);
        }

        gridProductCategory.EditIndex = -1;
        LoadData();
    }
    protected void gridProductCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridProductCategory.EditIndex = -1;
        LoadData();
    }
    protected void gridProductCategory_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    lblStatusShow.Text = "Active";
                else
                    lblStatusShow.Text = "Inactive";

            }
        }
    }
}