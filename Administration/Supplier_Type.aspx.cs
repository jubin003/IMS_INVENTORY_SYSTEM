using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Administration_Supplier_Type : System.Web.UI.Page
{
    SUPPLIER_TYPE STEnt = new SUPPLIER_TYPE();
    SUPPLIER_TYPEService STSer = new SUPPLIER_TYPEService();
    EntityList theList = new EntityList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadGrid();
        }
    }
    protected void LoadGrid()
    {
        STEnt = new SUPPLIER_TYPE();
        theList = STSer.GetAll(STEnt);
        if (theList.Count == 0)
        {
            theList.Add(STEnt);
        }
        gridSupplierType.DataSource = theList;
        gridSupplierType.DataBind();
    }
    protected void gridSupplierType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblSN = e.Row.FindControl("lblSN") as Label;
            Label lblSupplierType = e.Row.FindControl("lblSupplierType") as Label;            
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lblShowStatus = e.Row.FindControl("lblShowStatus") as Label;
            if (lblStatus.Text == "0")
            {
                lblShowStatus.Text = "Disabled";
            }
            else { lblShowStatus.Text = "Enabled"; }            
        }
    }

    protected void gridSupplierType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridSupplierType.Rows[e.RowIndex];
        Label lblPKIDU = (Label)row.FindControl("lblPKIDU");
        TextBox txtSupplierTypeE = (TextBox)row.FindControl("txtSupplierTypeE");
        TextBox txtPrefixE = (TextBox)row.FindControl("txtPrefixE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");

        STEnt = new SUPPLIER_TYPE();
        STEnt.PK_ID = lblPKIDU.Text;
        STEnt = (SUPPLIER_TYPE)STSer.GetSingle(STEnt);
        if (STEnt != null)
        {
            STEnt.SUPPLIERS_TYPE = txtSupplierTypeE.Text;
            STEnt.PREFIX = txtPrefixE.Text;
            STEnt.STATUS = ddlStatusE.SelectedValue;
            STSer.Update(STEnt);
        }
        gridSupplierType.EditIndex = -1;
        LoadGrid();
    }

    protected void gridSupplierType_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridSupplierType.EditIndex = e.NewEditIndex;
        LoadGrid();
    }

    protected void gridSupplierType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridSupplierType.EditIndex = -1;
        LoadGrid();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridSupplierType.HeaderRow;
        TextBox txtSupplierTypeH = (TextBox)row.FindControl("txtSupplierTypeH");
        TextBox txtPrefixH = (TextBox)row.FindControl("txtPrefixH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");
        if (ddlStatusH.SelectedValue != "Select")
        {
            STEnt = new SUPPLIER_TYPE();
            STEnt.SUPPLIERS_TYPE = txtSupplierTypeH.Text;
            STEnt.PREFIX= txtPrefixH.Text;
            STEnt.STATUS = ddlStatusH.SelectedValue;
            STSer.Insert(STEnt);
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Select the Supplier Status");
        }
        LoadGrid();
    }
}