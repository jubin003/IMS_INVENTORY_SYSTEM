using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;

public partial class Administration_product_type : System.Web.UI.Page
{
    PRODUCT_TYPE PCEnt = new PRODUCT_TYPE();
    PRODUCT_TYPEService PCSer = new PRODUCT_TYPEService();

    ActiveProductType APT = new ActiveProductType();

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
        TextBox txtNameH = (TextBox)row.FindControl("txtNameH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");
        DropDownList ddlSaStatusH = (DropDownList)row.FindControl("ddlSaStatusH");
        DropDownList ddlPuStatusH = (DropDownList)row.FindControl("ddlPuStatusH");

        if (string.IsNullOrEmpty(txtNameH.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Main Product Name Field cant be empty.");
        }
        else
        {
            PCEnt = new PRODUCT_TYPE();
            PCEnt.PRODUCT_TYPE_NAME = txtNameH.Text;
            PCEnt.STATUS = ddlStatusH.SelectedValue;
            PCEnt.SHOW_IN_PURCHASE = ddlPuStatusH.SelectedValue;
            PCEnt.SHOW_IN_SALES = ddlSaStatusH.SelectedValue;
            PCSer.Insert(PCEnt);

            LoadData();
        }
    }
    private void LoadData()
    {
        PCEnt = new PRODUCT_TYPE();
        grid.DataSource = PCSer.GetAll(PCEnt);
        grid.DataBind();

        if (grid.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            PCEnt = new PRODUCT_TYPE();
            a1.Add(PCEnt);

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
        TextBox txtNameE = (TextBox)row.FindControl("txtNameE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");
        DropDownList ddlSaStatusE = (DropDownList)row.FindControl("ddlSaStatusE");
        DropDownList ddlPuStatusE = (DropDownList)row.FindControl("ddlPuStatusE");

        PCEnt = new PRODUCT_TYPE();
        PCEnt.PK_ID = lblPKIDE.Text;
        PCEnt = (PRODUCT_TYPE)PCSer.GetSingle(PCEnt);
        if (PCEnt != null)
        {
            PCEnt.PRODUCT_TYPE_NAME = txtNameE.Text;
            PCEnt.STATUS = ddlStatusE.SelectedValue;
            PCEnt.SHOW_IN_PURCHASE = ddlPuStatusE.SelectedValue;
            PCEnt.SHOW_IN_SALES = ddlSaStatusE.SelectedValue;
            PCSer.Update(PCEnt);
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
                Label lblSaStatusE = (Label)e.Row.FindControl("lblSaStatusE");
                DropDownList ddlSaStatusE = (DropDownList)e.Row.FindControl("ddlSaStatusE");
                Label lblPuStatusE = (Label)e.Row.FindControl("lblPuStatusE");
                DropDownList ddlPuStatusE = (DropDownList)e.Row.FindControl("ddlPuStatusE");
                ddlStatusE.SelectedValue = lblStatusE.Text;
                ddlPuStatusE.SelectedValue = lblPuStatusE.Text;
                ddlSaStatusE.SelectedValue = lblSaStatusE.Text;
            }

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Label lblStatusShow = (Label)e.Row.FindControl("lblStatusShow");
                Label lblPuStatus = (Label)e.Row.FindControl("lblPuStatus");
                Label lblPuStatusShow = (Label)e.Row.FindControl("lblPuStatusShow");
                Label lblSaStatus = (Label)e.Row.FindControl("lblSaStatus");
                Label lblSaStatusShow = (Label)e.Row.FindControl("lblSaStatusShow");
                if (lblStatus.Text == "1")
                    lblStatusShow.Text = "Active";
                else { lblStatusShow.Text = "Inactive"; }
                if (lblPuStatus.Text == "1")
                { lblPuStatusShow.Text = "Active"; }
                else { lblPuStatusShow.Text = "Inactive"; }
                if (lblSaStatus.Text == "1")
                    lblSaStatusShow.Text = "Active";
                else { lblSaStatusShow.Text = "Inactive"; }

            }
        }
    }
}