using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Production_Swatch_Type : System.Web.UI.Page
{
    PR_SWATCH_TYPE SEnt = new PR_SWATCH_TYPE();
    PR_SWATCH_TYPEService SSer = new PR_SWATCH_TYPEService();
    EntityList theList = new EntityList();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gridLoad();
        }
    }


    protected void gridLoad()
    {
        SEnt = new PR_SWATCH_TYPE();

        EntityList EList = new EntityList();

        theList = SSer.GetAll(SEnt);

        if (theList.Count == 0)
        {
            theList.Add(SEnt);
        }

        gridDisplay.DataSource = theList;
        gridDisplay.DataBind();
    }


    protected void gridDisplay_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridDisplay.EditIndex = e.NewEditIndex;
        gridLoad();
    }


    protected void gridDisplay_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridDisplay.Rows[e.RowIndex];

        Label lblPK_ID =
            (Label)row.FindControl("lblPK_ID");

        TextBox txtSwatchName =
            (TextBox)row.FindControl("txtSwatchName");

        TextBox txtSwatchCode =
            (TextBox)row.FindControl("txtSwatchCode");

        TextBox txtRemarks =
            (TextBox)row.FindControl("txtRemarks");

        DropDownList ddlStatusE =
            (DropDownList)row.FindControl("ddlStatus");


        SEnt = new PR_SWATCH_TYPE();

        SEnt.PK_ID = lblPK_ID.Text;

        SEnt = (PR_SWATCH_TYPE)SSer.GetSingle(SEnt);


        if (SEnt != null)
        {
            SEnt.SWATCH_NAME = txtSwatchName.Text;
            SEnt.SWATCH_CODE = txtSwatchCode.Text;
            SEnt.REMARKS = txtRemarks.Text;
            SEnt.STATUS = ddlStatusE.SelectedValue;

            SSer.Update(SEnt);
        }


        gridDisplay.EditIndex = -1;

        gridLoad();
    }


    protected void gridDisplay_RowCancelingEdit(
        object sender,
        GridViewCancelEditEventArgs e)
    {
        gridDisplay.EditIndex = -1;

        gridLoad();
    }


    protected void gridDisplay_RowDataBound(
        object sender,
        GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if ((e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                Label lblStatus =
                    (Label)e.Row.FindControl("lblStatus");

                DropDownList ddlStatus =
                    (DropDownList)e.Row.FindControl("ddlStatus");

                if (lblStatus != null && ddlStatus != null)
                {
                    ddlStatus.SelectedValue = lblStatus.Text;
                }
            }


            if ((e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblStatus =
                    (Label)e.Row.FindControl("lblStatus");

                Label lblStatusShow =
                    (Label)e.Row.FindControl("lblStatusShow");


                if (lblStatus != null && lblStatusShow != null)
                {
                    if (lblStatus.Text == "1")
                    {
                        lblStatusShow.Text = "Active";
                        lblStatusShow.CssClass =
                            "status-pill status-active";
                    }
                    else
                    {
                        lblStatusShow.Text = "Inactive";
                        lblStatusShow.CssClass =
                            "status-pill status-inactive";
                    }
                }
            }
        }
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridDisplay.HeaderRow;


        TextBox txtSwatchName =
            (TextBox)row.FindControl("txtSwatchName");

        TextBox txtSwatchCode =
            (TextBox)row.FindControl("txtSwatchCode");

        TextBox txtRemarks =
            (TextBox)row.FindControl("txtRemarks");

        DropDownList ddlStatus =
            (DropDownList)row.FindControl("ddlStatus");


        if (string.IsNullOrEmpty(txtSwatchName.Text))
        {
            HelperFunction.MsgBox(
                this,
                this.GetType(),
                "Swatch Name can not be empty."
            );
        }
        else if (string.IsNullOrEmpty(txtSwatchCode.Text))
        {
            HelperFunction.MsgBox(
                this,
                this.GetType(),
                "Swatch Code can not be empty."
            );
        }
        else
        {
            SEnt = new PR_SWATCH_TYPE();

            SEnt.SWATCH_NAME = txtSwatchName.Text;
            SEnt.SWATCH_CODE = txtSwatchCode.Text;
            SEnt.REMARKS = txtRemarks.Text;
            SEnt.STATUS = ddlStatus.SelectedValue;

            SSer.Insert(SEnt);

            HelperFunction.MsgBox(
                this,
                this.GetType(),
                "Inserted"
            );

            gridLoad();
        }
    }
}