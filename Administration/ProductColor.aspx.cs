using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;

public partial class Administration_ProductColor : System.Web.UI.Page
{
    PRODUCT_COLOUR PCEnt = new PRODUCT_COLOUR();
    PRODUCT_COLOURService PCSer = new PRODUCT_COLOURService();

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
        TextBox txtColourCodeH = (TextBox)row.FindControl("txtColourCodeH");
        TextBox txtColourNameH = (TextBox)row.FindControl("txtColourNameH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");

        if (string.IsNullOrEmpty(txtColourNameH.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Main Product Name Field cant be empty.");
        }
        else
        {
            PCEnt = new PRODUCT_COLOUR();
            PCEnt.COLOUR_CODE = txtColourCodeH.Text;
            PCEnt.COLOUR_NAME = txtColourNameH.Text;
            PCEnt.STATUS = ddlStatusH.SelectedValue;
            PCSer.Insert(PCEnt);

            LoadData();
        }
    }
    private void LoadData()
    {
        PCEnt = new PRODUCT_COLOUR();
        grid.DataSource = PCSer.GetAll(PCEnt);
        grid.DataBind();

        if (grid.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            PCEnt = new PRODUCT_COLOUR();
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
        TextBox txtColourCodeE = (TextBox)row.FindControl("txtColourCodeE");
        TextBox txtColourNameE = (TextBox)row.FindControl("txtColourNameE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");

        PCEnt = new PRODUCT_COLOUR();
        PCEnt.PK_ID = lblPKIDE.Text;
        PCEnt = (PRODUCT_COLOUR)PCSer.GetSingle(PCEnt);
        if (PCEnt != null)
        {
            PCEnt.COLOUR_CODE = txtColourCodeE.Text;
            PCEnt.COLOUR_NAME = txtColourNameE.Text;
            PCEnt.STATUS = ddlStatusE.SelectedValue;
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