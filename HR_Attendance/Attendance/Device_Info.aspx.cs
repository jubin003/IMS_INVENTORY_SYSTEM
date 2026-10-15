using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Entity.Framework;
using Service.Components;

public partial class HR_Attendance_Attendance_Device_Info : System.Web.UI.Page
{
    ATT_DEVICE_INFO ADIEnt = new ATT_DEVICE_INFO();
    ATT_DEVICE_INFOService ADISer = new ATT_DEVICE_INFOService();

    EntityList theList = new EntityList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loadGrid();
        }
    }

    protected void loadGrid()
    {
        ADIEnt = new ATT_DEVICE_INFO();
        theList = ADISer.GetAll(ADIEnt);
        if (theList.Count == 0)
        {
            theList.Add(ADIEnt);
        }
        grdAttendance.DataSource = theList;
        grdAttendance.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = grdAttendance.HeaderRow;
        TextBox txtMachineNoH = (TextBox)row.FindControl("txtMachineNoH");
        TextBox txtIPAddrH = (TextBox)row.FindControl("txtIPAddrH");
        TextBox txtLocationH = (TextBox)row.FindControl("txtLocationH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");

        ADIEnt = new ATT_DEVICE_INFO();
        ADIEnt.MACHINE_NO = txtMachineNoH.Text;
        ADIEnt.IP_ADDRESS = txtIPAddrH.Text;
        ADIEnt.LOCATION = txtLocationH.Text;
        ADIEnt.STATUS = ddlStatusH.SelectedValue;
        ADISer.Insert(ADIEnt);
        loadGrid();
    }

    protected void grdAttendance_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdAttendance.EditIndex = e.NewEditIndex;
        loadGrid();
    }

    protected void grdAttendance_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdAttendance.EditIndex = -1;
        loadGrid();
    }

    protected void grdAttendance_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = grdAttendance.Rows[e.RowIndex];
        Label lblPK_IDE = row.FindControl("lblPK_IDE") as Label;
        TextBox txtMachineNoE = row.FindControl("txtMachineNoE") as TextBox;
        TextBox txtIPAddrE = row.FindControl("txtIPAddrE") as TextBox;
        TextBox txtLocationE = row.FindControl("txtLocationE") as TextBox;
        DropDownList ddlStatusE = row.FindControl("ddlStatusE") as DropDownList;

        ADIEnt = new ATT_DEVICE_INFO();
        ADIEnt.PK_ID = lblPK_IDE.Text;
        ADIEnt = (ATT_DEVICE_INFO)ADISer.GetSingle(ADIEnt);
        if (ADIEnt != null)
        {
            ADIEnt.MACHINE_NO = txtMachineNoE.Text;
            ADIEnt.IP_ADDRESS = txtIPAddrE.Text;
            ADIEnt.LOCATION = txtLocationE.Text;
            ADIEnt.STATUS = ddlStatusE.SelectedValue;
            ADISer.Update(ADIEnt);
        }
        grdAttendance.EditIndex = -1;
        loadGrid();
    }

    protected void grdAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lblShowStatus = e.Row.FindControl("lblShowStatus") as Label;

            if (lblStatus.Text == "1")
            {
                lblShowStatus.Text = "Available";
            }
            else
            {
                lblShowStatus.Text = "Unavailable";
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            Label lblPK_IDE = e.Row.FindControl("lblPK_IDE") as Label;
            DropDownList ddlStatusE = e.Row.FindControl("ddlStatusE") as DropDownList;

            ADIEnt = new ATT_DEVICE_INFO();
            ADIEnt.PK_ID = lblPK_IDE.Text;
            ADIEnt = (ATT_DEVICE_INFO)ADISer.GetSingle(ADIEnt);
            if (ADIEnt != null)
            {
                ddlStatusE.SelectedValue = ADIEnt.STATUS;
            }

        }
    }
}