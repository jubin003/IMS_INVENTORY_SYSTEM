using System;
using System.Web.UI.WebControls;
using System.Collections;
using Entity.Components;
using Service.Components;
using System.Data.SqlClient;
using System.Configuration;
using PhyeGanCore;

public partial class Administration_product_category : System.Web.UI.Page
{
    PRODUCT_CATEGORY PCEnt = new PRODUCT_CATEGORY();
    PRODUCT_CATEGORYService PCSer = new PRODUCT_CATEGORYService();
    PhyeGan PG = new PhyeGan();
    HelperFunction hf = new HelperFunction();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridProductCategory.HeaderRow;
        TextBox txtCategoryCodeH = (TextBox)row.FindControl("txtCategoryCodeH");
        TextBox txtCategoryNameH = (TextBox)row.FindControl("txtCategoryNameH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");

        if (string.IsNullOrEmpty(txtCategoryNameH.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Main Product Name Field cant be empty.");
        }
        else
        {
            PCEnt = new PRODUCT_CATEGORY();
            PCEnt.CATEGORY_CODE = txtCategoryCodeH.Text;
            PCEnt.CATEGORY_NAME = txtCategoryNameH.Text;
            PCEnt.STATUS = ddlStatusH.SelectedValue;
            string pk_id = PCSer.Insert(PCEnt).ToString();
            
            LoadData();
        }
    }
    private void LoadData()
    {
        PCEnt = new PRODUCT_CATEGORY();
        gridProductCategory.DataSource = PCSer.GetAll(PCEnt);
        gridProductCategory.DataBind();

        if (gridProductCategory.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            PCEnt = new PRODUCT_CATEGORY();
            a1.Add(PCEnt);

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
        TextBox txtCategoryNameE = (TextBox)row.FindControl("txtCategoryNameE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");
        PCEnt = new PRODUCT_CATEGORY();

        PCEnt.PK_ID = lblPKIDE.Text;
        PCEnt = (PRODUCT_CATEGORY)PCSer.GetSingle(PCEnt);
        if (PCEnt != null)
        {
            PCEnt.CATEGORY_NAME = txtCategoryNameE.Text;
            PCEnt.STATUS = ddlStatusE.SelectedValue;

            PCSer.Update(PCEnt);
           
        }

        gridProductCategory.EditIndex = -1;
        LoadData();
    }

    protected void gridProductCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridProductCategory.EditIndex = -1;
        LoadData();
    }

    protected void gridProductCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridProductCategory.PageIndex = e.NewPageIndex;
        LoadData();
    }
    protected void gridProductCategory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            TextBox txtCategoryCodeH = e.Row.FindControl("txtCategoryCodeH") as TextBox;
            txtCategoryCodeH.Text = Convert.ToDouble(hf.getCategoryCode()).ToString("000");
        }

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