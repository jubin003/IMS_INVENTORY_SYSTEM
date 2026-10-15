using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;

public partial class HumanResource_MasterData_hr_employee_type : System.Web.UI.Page
{
    HR_EMPLOYEE_TYPE HEnt = new HR_EMPLOYEE_TYPE();
    HR_EMPLOYEE_TYPEService HSer = new HR_EMPLOYEE_TYPEService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadGrid();
        }
    }
    protected void LoadGrid()
    {
        HEnt = new HR_EMPLOYEE_TYPE();
        gridEmpType.DataSource = HSer.GetAll(HEnt);
        gridEmpType.DataBind();
        if (gridEmpType.Rows.Count == 0)
        {
            ArrayList AL = new ArrayList();
            AL.Add(HEnt);
            gridEmpType.DataSource = AL;
            gridEmpType.DataBind();

        }
    }
    protected void gridEmpType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridEmpType.Rows[e.RowIndex];
        Label lblPKID = (Label)row.FindControl("lblPKID");
        TextBox txtEmployeeTypeE = (TextBox)row.FindControl("txtEmployeeTypeE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");

        HEnt = new HR_EMPLOYEE_TYPE();
        HEnt.PK_ID = lblPKID.Text;
        HEnt = (HR_EMPLOYEE_TYPE)HSer.GetSingle(HEnt);

        if (HEnt != null)
        {
            HEnt.EMPLOYEE_TYPE = txtEmployeeTypeE.Text;
            HEnt.STATUS = ddlStatusE.SelectedValue;
            HSer.Update(HEnt);
            gridEmpType.EditIndex = -1;
        }
        LoadGrid();
    }

    protected void gridEmpType_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void gridEmpType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridEmpType.EditIndex = -1;
        LoadGrid();
    }

    protected void gridEmpType_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridEmpType.EditIndex = e.NewEditIndex;
        LoadGrid();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        GridViewRow row = gridEmpType.HeaderRow;
        TextBox txtEmployeeTypeH = (TextBox)row.FindControl("txtEmployeeTypeH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");

        HEnt = new HR_EMPLOYEE_TYPE
        {
            EMPLOYEE_TYPE = txtEmployeeTypeH.Text,
            STATUS = ddlStatusH.SelectedValue
        };
        HSer.Insert(HEnt);
        LoadGrid();
    }


}