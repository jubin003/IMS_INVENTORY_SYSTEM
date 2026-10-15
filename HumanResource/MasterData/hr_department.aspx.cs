using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;

public partial class human_resource_Master_data_hr_department : System.Web.UI.Page
{
    HR_DEPARTMENT HDEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService HDSer = new HR_DEPARTMENTService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loadGrid();
        }
    }

    protected void loadGrid()
    {
        HDEnt = new HR_DEPARTMENT();
        //HDEnt.STATUS = "1";
        gridDepartment.DataSource = HDSer.GetAll(HDEnt);
        gridDepartment.DataBind();

        if (gridDepartment.Rows.Count == 0)
        {
            HDEnt = new HR_DEPARTMENT();
            ArrayList alist = new ArrayList();
            alist.Add(HDEnt);
            gridDepartment.DataSource = alist;
            gridDepartment.DataBind();
        }
    }

    protected void gridDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lblStat = e.Row.FindControl("lblStat") as Label;
            Label lblDivisionStatus = e.Row.FindControl("lblDivisionStatus") as Label;
            Label lblDivisionStat = e.Row.FindControl("lblDivisionStat") as Label;

            if (lblStatus.Text == "1")
            {
                lblStat.Text = "Available";
            }
            else
            {
                lblStat.Text = "Unavailable";

            }
            if (lblDivisionStatus.Text == "1")
            {
                lblDivisionStat.Text = "Available";
            }
            else
            {
                lblDivisionStat.Text = "Unavailable";

            }

        }

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            Label lblStatusE = e.Row.FindControl("lblStatusE") as Label;
            Label lblDivisionStatusE = e.Row.FindControl("lblDivisionStatusE") as Label;
            DropDownList ddlStatusE = e.Row.FindControl("ddlStatusE") as DropDownList;
            DropDownList ddlDivisionStatusE = e.Row.FindControl("ddlDivisionStatusE") as DropDownList;
            ddlStatusE.SelectedValue = lblStatusE.Text;
            ddlDivisionStatusE.SelectedValue = lblDivisionStatusE.Text;

        }

    }
    protected void gridDepartment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridDepartment.Rows[e.RowIndex];

        Label lblPKIDU = (Label)row.FindControl("lblPKIDU");
        TextBox txtDeptNameE = (TextBox)row.FindControl("txtDeptNameE");
        TextBox txtRemarksE = (TextBox)row.FindControl("txtRemarksE");
        TextBox txtDeptCodeE = (TextBox)row.FindControl("txtDeptCodeE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");
        DropDownList ddlDivisionStatusE = (DropDownList)row.FindControl("ddlDivisionStatusE");


        HDEnt = new HR_DEPARTMENT();
        HDEnt.PK_ID = lblPKIDU.Text;
        HDEnt = (HR_DEPARTMENT)HDSer.GetSingle(HDEnt);
        if (HDEnt != null)
        {
            HDEnt.DEP_CODE = txtDeptCodeE.Text;
            HDEnt.DEP_NAME = txtDeptNameE.Text;
            HDEnt.STATUS = ddlStatusE.SelectedValue;
            HDEnt.DIVISION_EXIST_STATUS = ddlDivisionStatusE.SelectedValue;
            HDEnt.REMARKS = txtRemarksE.Text;
            if (HDSer.Update(HDEnt) >= 1)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Successfully Updated");
                loadGrid();
            }

        }
        gridDepartment.EditIndex = -1;
        loadGrid();
    }
    protected void gridDepartment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridDepartment.EditIndex = e.NewEditIndex;
        loadGrid();

    }
    protected void gridDepartment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridDepartment.EditIndex = -1;
        loadGrid();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridDepartment.HeaderRow;
        TextBox txtDeptNameH = (TextBox)row.FindControl("txtDeptNameH");
        TextBox txtDeptCodeH = (TextBox)row.FindControl("txtDeptCodeH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");
        TextBox txtRemarksH = (TextBox)row.FindControl("txtRemarksH");
        DropDownList ddlDivisionStatusH = (DropDownList)row.FindControl("ddlDivisionStatusH");

        HDEnt = new HR_DEPARTMENT();
        HDEnt.DEP_NAME = txtDeptNameH.Text;
        HDEnt.DEP_CODE = txtDeptCodeH.Text;
        HDEnt.STATUS = ddlStatusH.SelectedValue;
        HDEnt.REMARKS = txtRemarksH.Text;
        HDEnt.DIVISION_EXIST_STATUS = ddlDivisionStatusH.SelectedValue;
        if (HDSer.Insert(HDEnt) >= 1)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Successfully Inserted");
            loadGrid();
        }

    }

    protected void txtDeptCodeH_TextChanged(object sender, EventArgs e)
    {
        TextBox txtDeptCodeH = gridDepartment.HeaderRow.FindControl("txtDeptCodeH") as TextBox;
        TextBox txtDeptNameH = gridDepartment.HeaderRow.FindControl("txtDeptNameH") as TextBox;
        Button btnAdd = gridDepartment.HeaderRow.FindControl("btnAdd") as Button;

        HDEnt = new HR_DEPARTMENT();
        HDEnt.DEP_CODE = txtDeptCodeH.Text;
        HDEnt = (HR_DEPARTMENT)HDSer.GetSingle(HDEnt);
        if (HDEnt != null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Code already exits. Please add another code !!");
            txtDeptCodeH.Text = "";
            btnAdd.Enabled = false;
        }
        else
        {
            btnAdd.Enabled = true;
            txtDeptNameH.Focus();
        }
    }

    protected void txtDeptCodeE_TextChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridDepartment.Rows)
        {
            if (gr.RowIndex == gridDepartment.EditIndex) // Only the row in edit mode
            {
                TextBox txtDeptCodeE = gr.FindControl("txtDeptCodeE") as TextBox;
                TextBox txtDeptNameE = gr.FindControl("txtDeptNameE") as TextBox;
                ImageButton imgUpdate = gr.FindControl("imgUpdate") as ImageButton;

                HDEnt = new HR_DEPARTMENT();
                HDEnt.DEP_CODE = txtDeptCodeE.Text;
                HDEnt = (HR_DEPARTMENT)HDSer.GetSingle(HDEnt);
                if (HDEnt != null)
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Code already exits. Please add another code !!");
                    imgUpdate.Enabled = false;
                }
                else
                {
                    imgUpdate.Enabled = true;
                    txtDeptNameE.Focus();
                }
            }
        }
    }
}