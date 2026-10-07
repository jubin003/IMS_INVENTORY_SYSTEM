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

public partial class Administration_Product_Unit : System.Web.UI.Page
{
    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();
    HelperFunction hf = new HelperFunction();
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
        TextBox txtUnitNameH = (TextBox)row.FindControl("txtUnitNameH");

        if (string.IsNullOrEmpty(txtUnitNameH.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Product unit can not be empty.");
        }
        else
        {
            PUEnt = new PRODUCT_UNIT();
            PUEnt.UNIT_NAME = txtUnitNameH.Text;
            string pk_id = PUSer.Insert(PUEnt).ToString();           
            LoadData();
        }
    }
    private void LoadData()
    {
        PUEnt = new PRODUCT_UNIT();
        grid.DataSource = PUSer.GetAll(PUEnt);
        grid.DataBind();

        if (grid.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            PUEnt = new PRODUCT_UNIT();
            a1.Add(PUEnt);

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
        TextBox txtUnitNameE = (TextBox)row.FindControl("txtUnitNameE");      


        PUEnt = new PRODUCT_UNIT();
        PUEnt.PK_ID = lblPKIDE.Text;
        PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
        if (PUEnt != null)
        {
            PUEnt.UNIT_NAME = txtUnitNameE.Text;
            PUSer.Update(PUEnt);
           
        }

        grid.EditIndex = -1;
        LoadData();
    }
    protected void grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grid.EditIndex = -1;
        LoadData();
    }
  

}