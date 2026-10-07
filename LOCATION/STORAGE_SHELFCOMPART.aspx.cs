using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;


public partial class LOCATION_STORAGE_SHELFCOMPART : System.Web.UI.Page
{

    STORAGE_SHELF SSEnt = new STORAGE_SHELF();
    STORAGE_SHELFService SSSer = new STORAGE_SHELFService();

    STORAGE_SHELFCOMPART SSCEnt = new STORAGE_SHELFCOMPART();
    STORAGE_SHELFCOMPARTService SSCSer = new STORAGE_SHELFCOMPARTService();

    Boolean IsPageRefresh = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["postids"] = System.Guid.NewGuid().ToString();
            Session["postid"] = ViewState["postids"].ToString();
            try
            {

                loadGrid();
            }
            catch { }
        }
        else
        {
            if (ViewState["postids"].ToString() != Session["postid"].ToString())
            {
                IsPageRefresh = true;
            }
            Session["postid"] = System.Guid.NewGuid().ToString();
            ViewState["postids"] = Session["postid"].ToString();

        }
    }

    protected void loadGrid()
    {

        SSCEnt = new STORAGE_SHELFCOMPART();
        gridShelfCompart.DataSource = SSCSer.GetAll(SSCEnt);
        gridShelfCompart.DataBind();
        if (gridShelfCompart.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            a1.Add(SSCEnt);
            gridShelfCompart.DataSource = a1;
            gridShelfCompart.DataBind();
        }

    }

    protected void gridShelfCompart_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DropDownList ddlShelfH = e.Row.FindControl("ddlShelfH") as DropDownList;

            SSEnt = new STORAGE_SHELF();
            SSEnt.STATUS = "1";
            ddlShelfH.DataSource = SSSer.GetAll(SSEnt);
            ddlShelfH.DataTextField = "SHELFNO";
            ddlShelfH.DataValueField = "PK_ID";
            ddlShelfH.DataBind();
            ddlShelfH.Items.Insert(0, "Select");
            if (lblSelectedShelf.Text != "0")
            {
                ddlShelfH.SelectedValue = lblSelectedShelf.Text;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblShelfid = e.Row.FindControl("lblShelfid") as Label;
            SSEnt = new STORAGE_SHELF();
            SSEnt.PK_ID = lblShelfid.Text;
            SSEnt = (STORAGE_SHELF)SSSer.GetSingle(SSEnt);
            if (SSEnt != null)
            {
                lblShelfid.Text = SSEnt.SHELFNO;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            Label lblCompartNoE = e.Row.FindControl("lblCompartNoE") as Label;
            TextBox txtCompartnoE = e.Row.FindControl("txtCompartnoE") as TextBox;
            DropDownList ddlShelfE = e.Row.FindControl("ddlShelfE") as DropDownList;
            txtCompartnoE.Text = lblCompartNoE.Text;
            SSEnt = new STORAGE_SHELF();
            ddlShelfE.DataSource = SSSer.GetAll(SSEnt);
            ddlShelfE.DataTextField = "SHELFNO";
            ddlShelfE.DataValueField = "PK_ID";
            ddlShelfE.DataBind();

        }

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridShelfCompart.HeaderRow;
        DropDownList ddlShelfH = (DropDownList)row.FindControl("ddlShelfH");
        TextBox txtCompartNoH = (TextBox)row.FindControl("txtCompartNoH");
        if (ddlShelfH.SelectedValue != "Select")
        {
            SSCEnt = new STORAGE_SHELFCOMPART();
            SSCEnt.SHELFID = ddlShelfH.SelectedValue;
            SSCEnt.COMPARTNO = txtCompartNoH.Text;
            SSCSer.Insert(SSCEnt);
        }
        else
            HelperFunction.MsgBox(this, this.GetType(), "Select Shelf.");
        loadGrid();
    }

    protected void gridShelfCompart_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridShelfCompart.EditIndex = -1;
        loadGrid();
    }

    protected void gridShelfCompart_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridShelfCompart.EditIndex = e.NewEditIndex;
        loadGrid();
    }

    protected void gridShelfCompart_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridShelfCompart.Rows[e.RowIndex];
        DropDownList ddlShelfE = row.FindControl("ddlShelfE") as DropDownList;
        TextBox txtCompartnoE = row.FindControl("txtCompartnoE") as TextBox;
        Label lblPK = row.FindControl("lblPK") as Label;

        SSCEnt = new STORAGE_SHELFCOMPART();
        SSCEnt.PK_ID = lblPK.Text;
        SSCEnt = (STORAGE_SHELFCOMPART)SSCSer.GetSingle(SSCEnt);
        if (SSCEnt != null)
        {
            SSCEnt.SHELFID = ddlShelfE.SelectedValue;
            SSCEnt.COMPARTNO = txtCompartnoE.Text;
            SSCSer.Update(SSCEnt);
            gridShelfCompart.EditIndex = -1;
            loadGrid();
        }
    }

    protected void ddlShelfH_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!IsPageRefresh)
        {
            GridViewRow gr = ((DropDownList)sender).Parent.Parent as GridViewRow;
            DropDownList ddlShelfH = gr.FindControl("ddlShelfH") as DropDownList;

            if (ddlShelfH.SelectedIndex == 0)
            {
                lblSelectedShelf.Text = "0";
                SSCEnt = new STORAGE_SHELFCOMPART();
                gridShelfCompart.DataSource = SSCSer.GetAll(SSCEnt);
                gridShelfCompart.DataBind();
                if (gridShelfCompart.Rows.Count == 0)
                {
                    ArrayList a1 = new ArrayList();
                    a1.Add(SSCEnt);
                    gridShelfCompart.DataSource = a1;
                    gridShelfCompart.DataBind();
                }
            }
            else
            {
                lblSelectedShelf.Text = ddlShelfH.SelectedValue;
                SSCEnt = new STORAGE_SHELFCOMPART();
                SSCEnt.SHELFID = ddlShelfH.SelectedValue;
                gridShelfCompart.DataSource = SSCSer.GetAll(SSCEnt);
                gridShelfCompart.DataBind();
                if (gridShelfCompart.Rows.Count == 0)
                {
                    ArrayList a1 = new ArrayList();
                    a1.Add(SSCEnt);
                    gridShelfCompart.DataSource = a1;
                    gridShelfCompart.DataBind();
                }
            }
        }
    }
}