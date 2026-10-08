using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Swatch_Transportation : System.Web.UI.Page
{
    PR_TRANSPORTATION TrnEnt = new PR_TRANSPORTATION();
    PR_TRANSPORTATIONService TrnSer = new PR_TRANSPORTATIONService();
    EntityList theList = new EntityList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            gridLoad();
    }

    protected void gridLoad()
    {
        TrnEnt = new PR_TRANSPORTATION();
        theList = TrnSer.GetAll(TrnEnt);

        if (theList.Count == 0)
            theList.Add(TrnEnt);

        gridDisplay.DataSource = theList;
        gridDisplay.DataBind();
    }

    protected void gridDisplay_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        Label lblPK_ID = (Label)e.Row.FindControl("lblPK_ID");
        bool isPlaceholder = (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text));

        if ((e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            // Edit mode: preselect the current status
            Label lblStatusE = (Label)e.Row.FindControl("lblStatusE");
            DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");

            if (lblStatusE != null && ddlStatus != null && ddlStatus.Items.FindByValue(lblStatusE.Text) != null)
                ddlStatus.SelectedValue = lblStatusE.Text;
        }
        else
        {
            // Normal mode: show the status pill
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            Label lblStatusShow = (Label)e.Row.FindControl("lblStatusShow");

            if (lblStatus != null && lblStatusShow != null && !string.IsNullOrEmpty(lblStatus.Text))
            {
                if (lblStatus.Text == "1")
                {
                    lblStatusShow.Text = "Active";
                    lblStatusShow.CssClass = "status-pill status-active";
                }
                else
                {
                    lblStatusShow.Text = "Inactive";
                    lblStatusShow.CssClass = "status-pill status-inactive";
                }
            }

            // Hide edit/delete buttons on the blank placeholder row
            if (isPlaceholder)
            {
                ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");

                if (btnEdit != null) btnEdit.Visible = false;
                if (btnDelete != null) btnDelete.Visible = false;
            }
        }
    }

    protected void gridDisplay_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridDisplay.EditIndex = e.NewEditIndex;
        gridLoad();
    }

    protected void gridDisplay_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridDisplay.EditIndex = -1;
        gridLoad();
    }

    protected void gridDisplay_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridDisplay.Rows[e.RowIndex];

        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");
        TextBox txtTransportation = (TextBox)row.FindControl("txtTransportationE");
        DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");

        if (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text))
            return;

        if (string.IsNullOrWhiteSpace(txtTransportation.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Transportation can not be empty.");
            return;
        }

        TrnEnt = new PR_TRANSPORTATION();
        TrnEnt.PK_ID = lblPK_ID.Text;
        TrnEnt = (PR_TRANSPORTATION)TrnSer.GetSingle(TrnEnt);

        if (TrnEnt != null)
        {
            TrnEnt.TRANSPORTATION_NAME = txtTransportation.Text.Trim();
            TrnEnt.STATUS = ddlStatus.SelectedValue;

            TrnSer.Update(TrnEnt);

            HelperFunction.MsgBox(this, this.GetType(), "Updated");
        }

        gridDisplay.EditIndex = -1;
        gridLoad();
    }

    protected void gridDisplay_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = gridDisplay.Rows[e.RowIndex];

        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");

        if (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text))
            return;

        TrnEnt = new PR_TRANSPORTATION();
        TrnEnt.PK_ID = lblPK_ID.Text;
        TrnEnt = (PR_TRANSPORTATION)TrnSer.GetSingle(TrnEnt);

        if (TrnEnt != null)
        {
            TrnSer.Delete(TrnEnt);

            HelperFunction.MsgBox(this, this.GetType(), "Deleted");
        }

        gridDisplay.EditIndex = -1;
        gridLoad();
    }

    protected void gridDisplay_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridDisplay.EditIndex = -1;
        gridDisplay.PageIndex = e.NewPageIndex;
        gridLoad();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridDisplay.HeaderRow;

        TextBox txtTransportation = (TextBox)row.FindControl("txtTransportation");
        DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatusH");

        if (string.IsNullOrWhiteSpace(txtTransportation.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Transportation can not be empty.");
            return;
        }

        TrnEnt = new PR_TRANSPORTATION();
        TrnEnt.TRANSPORTATION_NAME = txtTransportation.Text.Trim();
        TrnEnt.STATUS = ddlStatus.SelectedValue;

        TrnSer.Insert(TrnEnt);

        HelperFunction.MsgBox(this, this.GetType(), "Inserted");

        gridLoad();
    }
}