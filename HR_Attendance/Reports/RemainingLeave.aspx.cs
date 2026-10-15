using Entity.Components;
using Service.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_Attendance_Reports_RemainingLeave : System.Web.UI.Page
{
    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    HR_EMPLOYEES EEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService ESer = new HR_EMPLOYEESService();

    HR_UNIT HUEnt = new HR_UNIT();
    HR_UNITService HUSer = new HR_UNITService();

    HR_DIVISION HDIVEnt = new HR_DIVISION();
    HR_DIVISIONService HDIVSer = new HR_DIVISIONService();
    HumanResourceFunction hrf = new HumanResourceFunction();
    HelperFunction hf = new HelperFunction();

    PhyeGanDate pgd = new PhyeGanDate();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserProfileEntity userProfileEnt = new UserProfileEntity();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

            LoadDepartment();
            txtYear.Text = pgd.NepaliYear();
        }
    }

    protected void LoadDepartment()
    {
        DPEnt = new HR_DEPARTMENT();
        DPEnt.STATUS = "1";
        ddlDept.DataSource = DPSer.GetAll(DPEnt);
        ddlDept.DataTextField = "DEP_NAME";
        ddlDept.DataValueField = "PK_ID";
        ddlDept.DataBind();
        ddlDept.Items.Insert(0, "Select");

    }

    protected void LoadDivision()
    {
        HDIVEnt = new HR_DIVISION();
        HDIVEnt.DEP_ID = ddlDept.SelectedValue;
        HDIVEnt.STATUS = "1";
        ddlDivision.DataSource = HDIVSer.GetAll(HDIVEnt);
        ddlDivision.DataTextField = "DIVISION_NAME";
        ddlDivision.DataValueField = "PK_ID";
        ddlDivision.DataBind();
        ddlDivision.Items.Insert(0, "Select");
        if (ddlDivision.Items.Count <= 1)
        {
            ddlUnit.Items.Clear();
        }

    }

    protected void LoadUnit()
    {
        HUEnt = new HR_UNIT();
        HUEnt.STATUS = "1";
        HUEnt.DIVISION_ID = ddlDivision.SelectedValue;
        ddlUnit.DataSource = HUSer.GetAll(HUEnt);
        ddlUnit.DataTextField = "UNIT_NAME";
        ddlUnit.DataValueField = "PK_ID";
        ddlUnit.DataBind();
        ddlUnit.Items.Insert(0, "Select");
    }

    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDivision();
    }
    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDivision.SelectedIndex != 0)
        {
            LoadUnit();
            LoadEmployees();
        }
    }

    protected void LoadEmployees()
    {
        EEnt = new HR_EMPLOYEES();
        EEnt.DEPARTMENTID = ddlDept.SelectedValue;
        EEnt.DIVISION = ddlDivision.SelectedValue;
        if(ddlUnit.SelectedValue!="" && ddlUnit.SelectedValue!="Select")
        {
            EEnt.UNIT = ddlUnit.SelectedValue;
        }
        ddlEmployee.DataSource = ESer.GetAll(EEnt);
        ddlEmployee.DataValueField = "EMPLOYEEID";
        ddlEmployee.DataTextField= "FULL_NAME";
        ddlEmployee.DataBind();
        ddlEmployee.Items.Insert(0, "Select");
    }
    protected void bttnView_Click(object sender, EventArgs e)
    {
        grdRemainindLeave.DataSource = hrf.RemainingLeaveDaysRpt(ddlEmployee.SelectedValue,txtYear.Text);
        grdRemainindLeave.DataBind();
    }

    protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadEmployees();
    }

    
}