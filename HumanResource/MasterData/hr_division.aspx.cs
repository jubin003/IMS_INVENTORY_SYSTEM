using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;

public partial class HumanResource_MasterData_hr_division : System.Web.UI.Page
{
    HR_DIVISION HDEnt = new HR_DIVISION();
    HR_DIVISIONService HDSer = new HR_DIVISIONService();

    HR_DEPARTMENT HEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService HSer = new HR_DEPARTMENTService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            LoadDepartment();
        }
    }

    protected void LoadDepartment()
    {
        HEnt = new HR_DEPARTMENT();
        HEnt.STATUS = "1";
        HEnt.DIVISION_EXIST_STATUS = "1";
        ddlDept.DataSource = HSer.GetAll(HEnt);
        ddlDept.DataTextField = "DEP_NAME";
        ddlDept.DataValueField = "PK_ID";
        ddlDept.DataBind();
        ddlDept.Items.Insert(0, "Select");

    }

    protected void loadGrid()
    {
        HDEnt = new HR_DIVISION();
        HDEnt.DEP_ID = ddlDept.SelectedValue;
        gridDivision.DataSource = HDSer.GetAll(HDEnt);
        gridDivision.DataBind();

        if (gridDivision.Rows.Count == 0)
        {
            HDEnt = new HR_DIVISION();
            ArrayList alist = new ArrayList();
            alist.Add(HDEnt);
            gridDivision.DataSource = alist;
            gridDivision.DataBind();
        }
    }

    protected void gridDivision_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lblStat = e.Row.FindControl("lblStat") as Label;
            Label lblUnitStatus = e.Row.FindControl("lblUnitStatus") as Label;
            Label lblUnitStat = e.Row.FindControl("lblUnitStat") as Label;

            if (lblStatus.Text == "1")
            {
                lblStat.Text = "Available";
            }
            else
            {
                lblStat.Text = "Unavailable";

            }
            if (lblUnitStatus.Text == "1")
            {
                lblUnitStat.Text = "Available";
            }
            else
            {
                lblUnitStat.Text = "Unavailable";

            }

        }

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            Label lblStatusE = e.Row.FindControl("lblStatusE") as Label;
            Label lblUnitStatusE = e.Row.FindControl("lblUnitStatusE") as Label;
            DropDownList ddlStatusE = e.Row.FindControl("ddlStatusE") as DropDownList;
            DropDownList ddlUnitStatusE = e.Row.FindControl("ddlUnitStatusE") as DropDownList;
            ddlStatusE.SelectedValue = lblStatusE.Text;
            ddlUnitStatusE.SelectedValue = lblUnitStatusE.Text;

        }

    }
    protected void gridDivision_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridDivision.Rows[e.RowIndex];

        Label lblPKIDU = (Label)row.FindControl("lblPKIDU");
        TextBox txtDivisionNameE = (TextBox)row.FindControl("txtDivisionNameE");
        TextBox txtRemarksE = (TextBox)row.FindControl("txtRemarksE");
        TextBox txtDivisionCodeE = (TextBox)row.FindControl("txtDivisionCodeE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");
        DropDownList ddlUnitStatusE = (DropDownList)row.FindControl("ddlUnitStatusE");


        HDEnt = new HR_DIVISION();
        HDEnt.PK_ID = lblPKIDU.Text;
        HDEnt = (HR_DIVISION)HDSer.GetSingle(HDEnt);
        if (HDEnt != null)
        {
        HDEnt.DEP_ID = ddlDept.SelectedValue;
            HDEnt.DIVISION_CODE = txtDivisionCodeE.Text;
            HDEnt.DIVISION_NAME = txtDivisionNameE.Text;
            HDEnt.STATUS = ddlStatusE.SelectedValue;
            HDEnt.UNIT_EXIST_STATUS = ddlUnitStatusE.SelectedValue;
            HDEnt.REMARKS = txtRemarksE.Text;
            if (HDSer.Update(HDEnt) >= 1)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Successfully Updated");
                loadGrid();
            }

        }
        gridDivision.EditIndex = -1;
        loadGrid();
    }
    protected void gridDivision_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridDivision.EditIndex = e.NewEditIndex;
        loadGrid();

    }
    protected void gridDivision_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridDivision.EditIndex = -1;
        loadGrid();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridDivision.HeaderRow;
        TextBox txtDivisionNameH = (TextBox)row.FindControl("txtDivisionNameH");
        TextBox txtDivisionCodeH = (TextBox)row.FindControl("txtDivisionCodeH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");
        TextBox txtRemarksH = (TextBox)row.FindControl("txtRemarksH");
        DropDownList ddlUnitStatusH = (DropDownList)row.FindControl("ddlUnitStatusH");

        HDEnt = new HR_DIVISION();
        HDEnt.DIVISION_NAME = txtDivisionNameH.Text;
        HDEnt = (HR_DIVISION)HDSer.GetSingle(HDEnt);
        if(HDEnt != null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Division Already Exits");
        }
        else
        {
            HDEnt = new HR_DIVISION();
            HDEnt.DEP_ID = ddlDept.SelectedValue;
            HDEnt.DIVISION_NAME = txtDivisionNameH.Text;
            HDEnt.DIVISION_CODE = txtDivisionCodeH.Text;
            HDEnt.STATUS = ddlStatusH.SelectedValue;
            HDEnt.REMARKS = txtRemarksH.Text;
            HDEnt.UNIT_EXIST_STATUS = ddlUnitStatusH.SelectedValue;
            if (HDSer.Insert(HDEnt) >= 1)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Successfully Inserted");                
            }
        }
        loadGrid();

    }

    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDept.SelectedIndex != 0)
        {
            loadGrid();
        }
    }

    protected void txtDivisionCodeH_TextChanged(object sender, EventArgs e)
    {
        TextBox txtDivisionCodeH = gridDivision.HeaderRow.FindControl("txtDivisionCodeH") as TextBox;
        TextBox txtDivisionNameH = gridDivision.HeaderRow.FindControl("txtDivisionNameH") as TextBox;
        Button btnAdd = gridDivision.HeaderRow.FindControl("btnAdd") as Button;

        HDEnt = new HR_DIVISION();
        HDEnt.DIVISION_CODE = txtDivisionCodeH.Text;
        HDEnt = (HR_DIVISION)HDSer.GetSingle(HDEnt);
        if (HDEnt != null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Code already exits. Please add another code !!");
            txtDivisionCodeH.Text = "";
            btnAdd.Enabled = false;
        }
        else
        {
            btnAdd.Enabled = true;
            txtDivisionNameH.Focus();
        }
    }

    protected void txtDivisionCodeE_TextChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridDivision.Rows)
        {
            if (gr.RowIndex == gridDivision.EditIndex) // Only the row in edit mode
            {
                TextBox txtDivisionCodeE = gr.FindControl("txtDivisionCodeE") as TextBox;
                TextBox txtDivisionNameE = gr.FindControl("txtDivisionNameE") as TextBox;
                ImageButton imgUpdate = gr.FindControl("imgUpdate") as ImageButton;

                HDEnt = new HR_DIVISION();
                HDEnt.DIVISION_CODE = txtDivisionCodeE.Text;
                HDEnt = (HR_DIVISION)HDSer.GetSingle(HDEnt);
                if (HDEnt != null)
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Code already exits. Please add another code !!");
                    imgUpdate.Enabled = false;
                }
                else
                {
                    imgUpdate.Enabled = true;
                    txtDivisionNameE.Focus();
                }
            }
        }
    }
}