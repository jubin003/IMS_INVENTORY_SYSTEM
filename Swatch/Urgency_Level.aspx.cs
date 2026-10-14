using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Swatch_Urgency_Level : System.Web.UI.Page
{
    PR_URGENCY_LEVEL ULEnt = new PR_URGENCY_LEVEL();
    PR_URGENCY_LEVELService ULSer = new PR_URGENCY_LEVELService();

    EntityList theList = new EntityList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }
    }

    private void BindGrid()
    {
        // Try passing null first so the DAO does not generate WHERE field = '' filters.
        // If your framework throws ArgumentNullException on null, revert to: ULSer.GetAll(new PR_URGENCY_LEVEL())
        try
        {
            theList = (EntityList)ULSer.GetAll(null);
        }
        catch
        {
            theList = (EntityList)ULSer.GetAll(new PR_URGENCY_LEVEL());
        }

        // Only add placeholder row if database actually has zero records
        if (theList == null || theList.Count == 0)
        {
            theList = new EntityList();
            ULEnt = new PR_URGENCY_LEVEL();
            ULEnt.PK_ID = "-1"; // Distinct marker for dummy row
            ULEnt.URGENCY_NAME = "";
            ULEnt.DAYS = "";
            ULEnt.COLOR = "#000000";
            ULEnt.STATUS = "1";
            theList.Add(ULEnt);
        }

        gvUrgencyLevel.DataSource = theList;
        gvUrgencyLevel.DataBind();
    }

    protected void gvUrgencyLevel_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            PR_URGENCY_LEVEL item = (PR_URGENCY_LEVEL)e.Row.DataItem;

            // Only hide the explicit dummy placeholder row (-1)
            if (item != null && item.PK_ID == "-1")
            {
                e.Row.Visible = false;
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow headerRow = gvUrgencyLevel.HeaderRow;

        TextBox txtUrgencyName = (TextBox)headerRow.FindControl("txtUrgencyName");
        TextBox txtDays = (TextBox)headerRow.FindControl("txtDays");
        TextBox txtColor = (TextBox)headerRow.FindControl("txtColor");
        DropDownList ddlHeaderStatus = (DropDownList)headerRow.FindControl("ddlHeaderStatus");

        if (string.IsNullOrWhiteSpace(txtUrgencyName.Text))
        {
            return;
        }

        ULEnt = new PR_URGENCY_LEVEL();
        ULEnt.URGENCY_NAME = txtUrgencyName.Text.Trim();
        ULEnt.DAYS = txtDays.Text.Trim();
        ULEnt.COLOR = txtColor.Text.Trim();
        ULEnt.STATUS = ddlHeaderStatus != null ? ddlHeaderStatus.SelectedValue : "1";

        ULSer.Insert(ULEnt);

        Response.Redirect(Request.RawUrl);
    }

    protected void gvUrgencyLevel_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvUrgencyLevel.EditIndex = e.NewEditIndex;
        BindGrid();

        DropDownList ddlEditStatus = (DropDownList)gvUrgencyLevel.Rows[e.NewEditIndex].FindControl("ddlEditStatus");
        if (ddlEditStatus != null && gvUrgencyLevel.DataKeys[e.NewEditIndex].Values["STATUS"] != null)
        {
            ddlEditStatus.SelectedValue = gvUrgencyLevel.DataKeys[e.NewEditIndex].Values["STATUS"].ToString();
        }
    }

    protected void gvUrgencyLevel_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvUrgencyLevel.EditIndex = -1;
        BindGrid();
    }

    protected void gvUrgencyLevel_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gvUrgencyLevel.Rows[e.RowIndex];

        TextBox txtUrgencyNameE = (TextBox)row.FindControl("txtUrgencyNameE");
        TextBox txtDaysE = (TextBox)row.FindControl("txtDaysE");
        TextBox txtColorE = (TextBox)row.FindControl("txtColorE");
        DropDownList ddlEditStatus = (DropDownList)row.FindControl("ddlEditStatus");

        ULEnt = new PR_URGENCY_LEVEL();
        ULEnt.PK_ID = gvUrgencyLevel.DataKeys[e.RowIndex].Value.ToString();
        ULEnt = (PR_URGENCY_LEVEL)ULSer.GetSingle(ULEnt);

        ULEnt.URGENCY_NAME = txtUrgencyNameE.Text.Trim();
        ULEnt.DAYS = txtDaysE.Text.Trim();
        ULEnt.COLOR = txtColorE.Text.Trim();
        ULEnt.STATUS = ddlEditStatus.SelectedValue;

        ULSer.Update(ULEnt);

        gvUrgencyLevel.EditIndex = -1;
        BindGrid();
    }
}