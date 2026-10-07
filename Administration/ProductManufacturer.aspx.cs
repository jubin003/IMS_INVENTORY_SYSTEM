using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using PhyeGanCore;

public partial class Administration_ProductManufacturer : System.Web.UI.Page
{
    PRODUCT_MANUFACTURE PMEnt = new PRODUCT_MANUFACTURE();
    PRODUCT_MANUFACTUREService PMSer = new PRODUCT_MANUFACTUREService();
    PhyeGan PG = new PhyeGan();
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
        TextBox txtManufacturerNameH = (TextBox)row.FindControl("txtManufacturerNameH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");

        if (string.IsNullOrEmpty(txtManufacturerNameH.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Main Product Name Field cant be empty.");
        }
        else
        {
            PMEnt = new PRODUCT_MANUFACTURE();
            PMEnt.MANUFACTURE_NAME = txtManufacturerNameH.Text;
            PMEnt.STATUS = ddlStatusH.SelectedValue;
            string pk_id = PMSer.Insert(PMEnt).ToString();
            LoadData();
        }
    }
    private void LoadData()
    {
        PMEnt = new PRODUCT_MANUFACTURE();
        grid.DataSource = PMSer.GetAll(PMEnt);
        grid.DataBind();

        if (grid.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            PMEnt = new PRODUCT_MANUFACTURE();
            a1.Add(PMEnt);
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
        TextBox txtManufacturerNameE = (TextBox)row.FindControl("txtManufacturerNameE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");

        PMEnt = new PRODUCT_MANUFACTURE();
        PMEnt.PK_ID = lblPKIDE.Text;
        PMEnt = (PRODUCT_MANUFACTURE)PMSer.GetSingle(PMEnt);
        if (PMEnt != null)
        {
            PMEnt.MANUFACTURE_NAME = txtManufacturerNameE.Text;
            PMEnt.STATUS = ddlStatusE.SelectedValue;
            PMSer.Update(PMEnt);
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