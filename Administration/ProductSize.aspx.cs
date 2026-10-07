using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;

public partial class Administration_ProductSize : System.Web.UI.Page
{
    PRODUCT_SIZE PSEnt = new PRODUCT_SIZE();
    PRODUCT_SIZEService PSSer = new PRODUCT_SIZEService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = grid.HeaderRow;
        TextBox txtSizeCodeH = (TextBox)row.FindControl("txtSizeCodeH");
        TextBox txtProductSizeNameH = (TextBox)row.FindControl("txtProductSizeNameH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");

        if (string.IsNullOrEmpty(txtProductSizeNameH.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Main Product Name Field cant be empty.");
        }
        else
        {
            PSEnt = new PRODUCT_SIZE();
            PSEnt.SIZE_CODE = txtSizeCodeH.Text;
            PSEnt.SIZE_NAME = txtProductSizeNameH.Text;
            PSEnt.STATUS = ddlStatusH.SelectedValue;
            PSSer.Insert(PSEnt);

            LoadData();
        }
    }
    private void LoadData()
    {
        PSEnt = new PRODUCT_SIZE();
        grid.DataSource = PSSer.GetAll(PSEnt);
        grid.DataBind();

        if (grid.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            PSEnt = new PRODUCT_SIZE();
            a1.Add(PSEnt);

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
        TextBox txtSizeCodeE = (TextBox)row.FindControl("txtSizeCodeE");
        TextBox txtProductSizeNameE = (TextBox)row.FindControl("txtProductSizeNameE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");

        PSEnt = new PRODUCT_SIZE();
        PSEnt.PK_ID = lblPKIDE.Text;
        PSEnt = (PRODUCT_SIZE)PSSer.GetSingle(PSEnt);
        if (PSEnt != null)
        {
            PSEnt.SIZE_CODE = txtSizeCodeE.Text;
            PSEnt.SIZE_NAME = txtProductSizeNameE.Text;
            PSEnt.STATUS = ddlStatusE.SelectedValue;
            PSSer.Update(PSEnt);
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