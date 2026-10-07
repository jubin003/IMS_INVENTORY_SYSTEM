using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Entity.Framework;
using Service.Components;

public partial class Administration_Country : System.Web.UI.Page
{

    COUNTRY CYEnt = new COUNTRY();
    COUNTRYService CYSer = new COUNTRYService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadGrid();
        }
    }

    protected void LoadGrid()
    {
        CYEnt = new COUNTRY();
        grdCountry.DataSource = CYSer.GetAll(CYEnt);
        grdCountry.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        CYEnt = new COUNTRY();
        CYEnt.COUNTRY_NAME = txtCountry.Text;
        CYEnt.NATIONALITY = txtNationality.Text;
        CYSer.Insert(CYEnt);

        LoadGrid();
        ClearField();
    }

    protected void ClearField()
    {
        txtCountry.Text = "";
        txtNationality.Text = "";
    }

    protected void grdCountry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Button btn = (Button)e.CommandSource;
        GridViewRow row = (GridViewRow)btn.NamingContainer;

        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");

        if (e.CommandName == "UpdateRow")
        {
            TextBox txtgrdCountry = (TextBox)row.FindControl("txtgrdCountry");
            TextBox txtgrdNationality = (TextBox)row.FindControl("txtgrdNationality");

            CYEnt = new COUNTRY();
            CYEnt.PK_ID = lblPK_ID.Text;
            CYEnt.COUNTRY_NAME = txtgrdCountry.Text;
            CYEnt.NATIONALITY = txtgrdNationality.Text;
            CYSer.Update(CYEnt);

            LoadGrid();
        }
        else if (e.CommandName == "DeleteRow")
        {
            CYEnt = new COUNTRY();
            CYEnt.PK_ID = lblPK_ID.Text;
            CYSer.Delete(CYEnt);

            LoadGrid();
        }
    }
}