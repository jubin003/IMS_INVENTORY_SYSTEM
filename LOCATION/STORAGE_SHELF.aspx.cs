using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;

public partial class LOCATION_STORAGE_SHELF : System.Web.UI.Page
{
    STORAGE_SHELF SSEnt = new STORAGE_SHELF();
    STORAGE_SHELFService SSSer = new STORAGE_SHELFService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loadShelfGrid();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (txtShelf.Text == "")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please enter Shelf No.");
            txtShelf.Focus();
        }
        else
        {
            SSEnt = new STORAGE_SHELF();
            SSEnt.SHELFNO = txtShelf.Text;
            SSEnt.STATUS = "1";
            SSSer.Insert(SSEnt);
            loadShelfGrid();
        }

    }

    protected void loadShelfGrid()
    {
        SSEnt = new STORAGE_SHELF();
        SSEnt.STATUS = "1";
        gridShelf.DataSource = SSSer.GetAll(SSEnt);
        gridShelf.DataBind();
        if (gridShelf.Rows.Count == 0)
        {
            gridShelf.Visible = false;
        }
        else
        {
            gridShelf.Visible = true;
        }
    }

    protected void gridShelf_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridShelf.EditIndex = e.NewEditIndex;
        loadShelfGrid();

    }

    protected void gridShelf_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            Label lblShelfE = e.Row.FindControl("lblShelfE") as Label;
            TextBox txtShelfE = e.Row.FindControl("txtShelfE") as TextBox;

            txtShelfE.Text = lblShelfE.Text;
            lblShelfE.Visible = false;
        }
    }



    protected void gridShelf_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridShelf.EditIndex = -1;
        loadShelfGrid();
    }

    protected void gridShelf_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridShelf.Rows[e.RowIndex];
        TextBox txtShelfE = row.FindControl("txtShelfE") as TextBox;
        Label lblShelfE = row.FindControl("lblShelfE") as Label;
        Label lblPK = row.FindControl("lblPK") as Label;

        SSEnt = new STORAGE_SHELF();
        SSEnt.PK_ID = lblPK.Text;
        SSEnt.SHELFNO = lblShelfE.Text;
        SSEnt = (STORAGE_SHELF)SSSer.GetSingle(SSEnt);
        if (SSEnt != null)
        {
            SSEnt.SHELFNO = txtShelfE.Text;
            SSSer.Update(SSEnt);
            gridShelf.EditIndex = -1;
            loadShelfGrid();
        }
    }
}