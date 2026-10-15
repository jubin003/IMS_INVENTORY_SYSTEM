using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class HR_Attendance_MasterData_Leave_Approver : System.Web.UI.Page
{
    HR_DEPARTMENT HDEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService HDSer = new HR_DEPARTMENTService();

    HR_EMPLOYEES HEEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService HESer = new HR_EMPLOYEESService();

    HR_DIVISION HDIVEnt = new HR_DIVISION();
    HR_DIVISIONService HDIVSer = new HR_DIVISIONService();

    HR_UNIT HUEnt = new HR_UNIT();
    HR_UNITService HUSer = new HR_UNITService();

    LEAVE_APPROVER LAEnt = new LEAVE_APPROVER();
    LEAVE_APPROVERService LASer = new LEAVE_APPROVERService();

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
        LAEnt = new LEAVE_APPROVER();
        theList = LASer.GetAll(LAEnt);
        if (theList.Count == 0)
        {            
            theList.Add(LAEnt);
            btnSave.Visible = false;
        }
        else
        {
            btnSave.Visible = true;
        }
        gridApprove.DataSource = theList;
        gridApprove.DataBind();
    }
    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gr = ((DropDownList)sender).Parent.Parent as GridViewRow;

        DropDownList ddlDepartment = gr.FindControl("ddlDepartment") as DropDownList;
        DropDownList ddlEmpName = gr.FindControl("ddlEmpName") as DropDownList;
        DropDownList ddlDivision = gr.FindControl("ddlDivision") as DropDownList;
        DropDownList ddlUnit = gr.FindControl("ddlUnit") as DropDownList;

        HDIVEnt = new HR_DIVISION();
        HDIVEnt.STATUS = "1";

        HDIVEnt.DEP_ID = ddlDepartment.SelectedValue;
        ddlDivision.DataSource = HDIVSer.GetAll(HDIVEnt);
        ddlDivision.DataTextField = "DIVISION_NAME";
        ddlDivision.DataValueField = "PK_ID";
        ddlDivision.DataBind();
        ddlDivision.Items.Insert(0, "Select");

        HEEnt = new HR_EMPLOYEES();
        HEEnt.DEPARTMENTID = ddlDepartment.SelectedValue;
        HEEnt.STATUS = "1";
        ddlEmpName.DataSource = HESer.GetAll(HEEnt);
        ddlEmpName.DataTextField = "FULL_NAME";
        ddlEmpName.DataValueField = "EMPLOYEEID";
        ddlEmpName.DataBind();
        ddlEmpName.Items.Insert(0, "Select");

        ddlUnit.SelectedValue = "Select";
    }

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gr = ((DropDownList)sender).Parent.Parent as GridViewRow;

        DropDownList ddlDepartment = gr.FindControl("ddlDepartment") as DropDownList;
        DropDownList ddlEmpName = gr.FindControl("ddlEmpName") as DropDownList;
        DropDownList ddlDivision = gr.FindControl("ddlDivision") as DropDownList;
        DropDownList ddlUnit = gr.FindControl("ddlUnit") as DropDownList;

        HUEnt = new HR_UNIT();
        HUEnt.STATUS = "1";
        HUEnt.DIVISION_ID = ddlDivision.SelectedValue;
        ddlUnit.DataSource = HUSer.GetAll(HUEnt);
        ddlUnit.DataTextField = "UNIT_NAME";
        ddlUnit.DataValueField = "PK_ID";
        ddlUnit.DataBind();
        ddlUnit.Items.Insert(0, "Select");

        HEEnt = new HR_EMPLOYEES();
        HEEnt.DEPARTMENTID = ddlDepartment.SelectedValue;
        if (ddlDivision.SelectedValue != "Select")
        {
            HEEnt.DIVISION = ddlDivision.SelectedValue;
        }
        HEEnt.STATUS = "1";
        ddlEmpName.DataSource = HESer.GetAll(HEEnt);
        ddlEmpName.DataTextField = "FULL_NAME";
        ddlEmpName.DataValueField = "EMPLOYEEID";
        ddlEmpName.DataBind();
        ddlEmpName.Items.Insert(0, "Select");
    }

    protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gr = ((DropDownList)sender).Parent.Parent as GridViewRow;

        DropDownList ddlDepartment = gr.FindControl("ddlDepartment") as DropDownList;
        DropDownList ddlEmpName = gr.FindControl("ddlEmpName") as DropDownList;
        DropDownList ddlDivision = gr.FindControl("ddlDivision") as DropDownList;
        DropDownList ddlUnit = gr.FindControl("ddlUnit") as DropDownList;

        HEEnt = new HR_EMPLOYEES();
        HEEnt.DEPARTMENTID = ddlDepartment.SelectedValue;
        HEEnt.DIVISION = ddlDivision.SelectedValue;
        if (ddlUnit.SelectedValue != "Select")
        {
            HEEnt.UNIT = ddlUnit.SelectedValue;
        }
        ddlEmpName.DataSource = HESer.GetAll(HEEnt);
        ddlEmpName.DataTextField = "FULL_NAME";
        ddlEmpName.DataValueField = "EMPLOYEEID";
        ddlEmpName.DataBind();
        ddlEmpName.Items.Insert(0, "Select");
    }


    protected void gridApprove_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DropDownList ddlDepartment = (DropDownList)e.Row.FindControl("ddlDepartment");
            HDEnt = new HR_DEPARTMENT();
            HDEnt.STATUS = "1";
            ddlDepartment.DataSource = HDSer.GetAll(HDEnt);
            ddlDepartment.DataTextField = "DEP_NAME";
            ddlDepartment.DataValueField = "PK_ID";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, "Select");
        }
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblEmployeeID = (Label)e.Row.FindControl("lblEmployeeID");
            Label lblDepartment = (Label)e.Row.FindControl("lblDepartment");
            Label lblDivision = (Label)e.Row.FindControl("lblDivision");
            Label lblUnit = (Label)e.Row.FindControl("lblUnit");
            Label lblShowName = (Label)e.Row.FindControl("lblShowName");           

            CheckBox chkRecommend = (CheckBox)e.Row.FindControl("chkRecommend");
            CheckBox chkApprove = (CheckBox)e.Row.FindControl("chkApprove");

            LAEnt = new LEAVE_APPROVER();
            LAEnt.EMP_ID = lblEmployeeID.Text;
            LAEnt = (LEAVE_APPROVER)LASer.GetSingle(LAEnt);
            if(LAEnt != null)
            {
                if (LAEnt.RECOMMEND_BY == "1")
                    chkRecommend.Checked = true;
                else
                    chkRecommend.Checked = false;

                if (LAEnt.APPROVED_BY == "1")
                    chkApprove.Checked = true;
                else
                    chkApprove.Checked = false;               
            }

            

            HEEnt = new HR_EMPLOYEES();
            HEEnt.EMPLOYEEID = lblEmployeeID.Text;
            HEEnt = (HR_EMPLOYEES)HESer.GetSingle(HEEnt);
            if (HEEnt != null)
            {
                lblShowName.Text = HEEnt.FULL_NAME;
                HDEnt = new HR_DEPARTMENT();
                HDEnt.PK_ID = HEEnt.DEPARTMENTID;
                HDEnt = (HR_DEPARTMENT)HDSer.GetSingle(HDEnt);
                if (HDEnt != null)
                {
                    lblDepartment.Text = HDEnt.DEP_NAME;
                }
                if (string.IsNullOrWhiteSpace(HEEnt.DIVISION))
                {
                    lblDivision.Text = "--";

                }
                else
                {
                    HDIVEnt = new HR_DIVISION();
                    HDIVEnt.PK_ID = HEEnt.DIVISION;
                    HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
                    if (HDIVEnt != null)
                    {
                        lblDivision.Text = HDIVEnt.DIVISION_NAME;
                    }
                }
                if (string.IsNullOrWhiteSpace(HEEnt.UNIT))
                {
                    lblUnit.Text = "--";

                }

                else
                {
                    HUEnt = new HR_UNIT();
                    HUEnt.PK_ID = HEEnt.UNIT;
                    HUEnt = (HR_UNIT)HUSer.GetSingle(HUEnt);
                    if (HUEnt != null)
                    {
                        lblUnit.Text = HUEnt.UNIT_NAME;
                    }

                }
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridApprove.HeaderRow;

        DropDownList ddlDepartment = (DropDownList)row.FindControl("ddlDepartment");
        DropDownList ddlEmpName = (DropDownList)row.FindControl("ddlEmpName");
        CheckBox chkRecommendH = (CheckBox)row.FindControl("chkRecommendH");
        CheckBox chkApproveH = (CheckBox)row.FindControl("chkApproveH");

        if (string.IsNullOrEmpty(ddlDepartment.Text) || string.IsNullOrEmpty(ddlEmpName.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Department or Employee Name can not be empty.");
        }
        else
        {
            if (ddlEmpName.SelectedValue != "Select" && ddlEmpName.SelectedValue != "")
            {
                LAEnt = new LEAVE_APPROVER();
                LAEnt.EMP_ID = ddlEmpName.SelectedValue;
                if (chkRecommendH.Checked)
                    LAEnt.RECOMMEND_BY = "1";
                else
                    LAEnt.RECOMMEND_BY = "0";

                if (chkApproveH.Checked)
                    LAEnt.APPROVED_BY = "1";
                else
                    LAEnt.APPROVED_BY = "0";               
                LASer.Insert(LAEnt);
            }
            else
            {
                HelperFunction.MsgBox(this, this.GetType(), "Employee Name cannot be empty.");
            }
        }
        loadGrid();
    }   

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((Button)sender).NamingContainer;

        Label lblPKIDE = row.FindControl("lblPKIDE") as Label;
        CheckBox chkRecommend = row.FindControl("chkRecommend") as CheckBox;
        CheckBox chkApprove = row.FindControl("chkApprove") as CheckBox;

        LAEnt = new LEAVE_APPROVER();
        LAEnt.PK_ID = lblPKIDE.Text;
        LAEnt = (LEAVE_APPROVER)LASer.GetSingle(LAEnt);
        if (LAEnt != null)
        {
            if (chkRecommend.Checked)
                LAEnt.RECOMMEND_BY = "1";
            else
                LAEnt.RECOMMEND_BY = "0";

            if (chkApprove.Checked)
                LAEnt.APPROVED_BY = "1";
            else
                LAEnt.APPROVED_BY = "0";
            LASer.Update(LAEnt);
            HelperFunction.MsgBox(this, this.GetType(), "Data Saved.");
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        foreach(GridViewRow gr in gridApprove.Rows)
        {
            Label lblPKIDE = gr.FindControl("lblPKIDE") as Label;
            CheckBox chkRecommend = gr.FindControl("chkRecommend") as CheckBox;
            CheckBox chkApprove = gr.FindControl("chkApprove") as CheckBox;

            LAEnt = new LEAVE_APPROVER();
            LAEnt.PK_ID = lblPKIDE.Text;
            LAEnt = (LEAVE_APPROVER)LASer.GetSingle(LAEnt);
            if (LAEnt != null)
            {
                if (chkRecommend.Checked)
                    LAEnt.RECOMMEND_BY = "1";
                else
                    LAEnt.RECOMMEND_BY = "0";

                if (chkApprove.Checked)
                    LAEnt.APPROVED_BY = "1";
                else
                    LAEnt.APPROVED_BY = "0";
               LASer.Update(LAEnt);                
            }
        }
        loadGrid();
        HelperFunction.MsgBox(this, this.GetType(), "Data Saved.");
    }
}