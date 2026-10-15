using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;
public partial class HumanResource_MasterData_hr_termination_type : System.Web.UI.Page
{
    HrEmployeeTerminationType HEnt = new HrEmployeeTerminationType();
    HR_EMP_TERMINATION_TYPEService HSer = new HR_EMP_TERMINATION_TYPEService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadGrid();
        }
    }
    protected void LoadGrid()
    {
        HEnt = new HrEmployeeTerminationType();
        gridTerType.DataSource = HSer.GetAll(HEnt);
        gridTerType.DataBind();
        if (gridTerType.Rows.Count == 0)
        {
            ArrayList AL = new ArrayList();
            AL.Add(HEnt);
            gridTerType.DataSource = AL;
            gridTerType.DataBind();

        }
    }
    protected void gridTerType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lblShowStatus = e.Row.FindControl("lblShowStatus") as Label;

            if (lblStatus.Text == "1")
                lblShowStatus.Text = "Available";
            else
                lblShowStatus.Text = "Unavailable";
        }
    }

    protected void gridTerType_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridTerType.EditIndex = e.NewEditIndex;
        LoadGrid();
    }

    protected void gridTerType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridTerType.Rows[e.RowIndex];
        Label lblPKID = (Label)row.FindControl("lblPKID");
        TextBox txtTerminationTypeE = (TextBox)row.FindControl("txtTerminationTypeE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");

        HEnt = new HrEmployeeTerminationType();
        HEnt.PK_ID = lblPKID.Text;
        HEnt = (HrEmployeeTerminationType)HSer.GetSingle(HEnt);

        if (HEnt != null)
        {
            HEnt.TERMINATION_TYPE = txtTerminationTypeE.Text;
            HEnt.STATUS = ddlStatusE.SelectedValue;
            HSer.Update(HEnt);
            gridTerType.EditIndex = -1;
        }
        LoadGrid();
    }

    protected void gridTerType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridTerType.EditIndex = -1;
        LoadGrid();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

        GridViewRow row = gridTerType.HeaderRow;
        TextBox txtTerminationTypeH = (TextBox)row.FindControl("txtTerminationTypeH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");

        HEnt = new HrEmployeeTerminationType
        {
            TERMINATION_TYPE = txtTerminationTypeH.Text,
            STATUS = ddlStatusH.SelectedValue
        };
        HSer.Insert(HEnt);
        LoadGrid();
    }
}