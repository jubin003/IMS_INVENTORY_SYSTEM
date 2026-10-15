using Entity.Components;
using Service.Components;
using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PhyeGanCore;


public partial class HR_Attendance_Reports_EMP_LEAVE_REPORT : System.Web.UI.Page
{
    NAME_COMPANY NCEnt = new NAME_COMPANY();
    NAME_COMPANYService NCSer = new NAME_COMPANYService();

    PhyeGan PG = new PhyeGan();

    HR_LEAVE_TYPE LTEnt = new HR_LEAVE_TYPE();
    HR_LEAVE_TYPEService LTSer = new HR_LEAVE_TYPEService();

    HR_EMP_LEAVE_MASTER ELMEnt = new HR_EMP_LEAVE_MASTER();
    HR_EMP_LEAVE_MASTERService ELMSer = new HR_EMP_LEAVE_MASTERService();

    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    HR_DIVISION DivEnt = new HR_DIVISION();
    HR_DIVISIONService DivSrv = new HR_DIVISIONService();

    HR_EMPLOYEES EEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService ESer = new HR_EMPLOYEESService();

    HelperFunction hf = new HelperFunction();
    HumanResourceFunction hrf = new HumanResourceFunction();

    PhyeGanDate pgd = new PhyeGanDate();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtYear.Text = pgd.NepaliYear();
            loadDepartment();
            LoadDivisionSearch();
        }
    }

    protected void loadDepartment()
    {
        DPEnt = new HR_DEPARTMENT();
        ddlDepartmentSearch.DataSource = DPSer.GetAll(DPEnt);
        ddlDepartmentSearch.DataTextField = "DEP_NAME";
        ddlDepartmentSearch.DataValueField = "PK_ID";
        ddlDepartmentSearch.DataBind();
        ddlDepartmentSearch.Items.Insert(0, "Select");

    }
    protected void LoadDivisionSearch()
    {
        DivEnt = new HR_DIVISION();
        if (ddlDepartmentSearch.SelectedValue != "Select")
        {
            DivEnt.DEP_ID = ddlDepartmentSearch.SelectedValue;
        }
        else
        {
            DivEnt.DEP_ID = "";
        }
        DivEnt.STATUS = "1";

        ddlDivisionSearch.DataSource = DivSrv.GetAll(DivEnt);
        ddlDivisionSearch.DataTextField = "DIVISION_NAME";
        ddlDivisionSearch.DataValueField = "PK_ID";
        ddlDivisionSearch.DataBind();
        ddlDivisionSearch.Items.Insert(0, "Select");
    }


    protected void LoadEmployeesSearch()
    {
        ddlEmployeeSearch.Items.Clear();
        EEnt = new HR_EMPLOYEES();
        if (ddlDepartmentSearch.SelectedValue != "Select")
        {
            EEnt.DEPARTMENTID = ddlDepartmentSearch.SelectedValue;
        }
        if (ddlDivisionSearch.SelectedValue != "Select")
        {
            EEnt.DIVISION = ddlDivisionSearch.SelectedValue;
        }
        else
        {
            EEnt.DIVISION = "";
        }
        EEnt.STATUS = "1";

        ddlEmployeeSearch.DataSource = ESer.GetAll(EEnt);
        ddlEmployeeSearch.DataTextField = "FULL_NAME";
        ddlEmployeeSearch.DataValueField = "EMPLOYEEID";
        ddlEmployeeSearch.DataBind();
        ddlEmployeeSearch.Items.Insert(0, "Select");
    }


    protected void btnView_Click(object sender, EventArgs e)
    {
        if (ddlEmployeeSearch.SelectedValue != "Select" && ddlEmployeeSearch.SelectedValue != "")
        {
            LoadCompanyDetail();
            if (ddlReportType.SelectedValue == "Detail")
            {
                hide.Visible = true;
                trSummaryReport.Visible = true;
                trGenReport.Visible = false;

                ELMEnt = new HR_EMP_LEAVE_MASTER();
                ELMEnt.EMPLOYEE_ID = ddlEmployeeSearch.SelectedValue;
                ELMEnt.STATUS = "1";
                ELMEnt.TO_YEAR = txtYear.Text;
                gridEmpLeave.DataSource = ELMSer.GetAll(ELMEnt);
                gridEmpLeave.DataBind();
                gridEmpLeave.Visible = true;

            }

            else
            {
                grdRemainindLeave.DataSource = hrf.RemainingLeaveDaysRpt(ddlEmployeeSearch.SelectedValue, txtYear.Text);
                grdRemainindLeave.DataBind();

                hide.Visible = true;
                gridEmpLeave.Visible = false;
                trGenReport.Visible = true;
            }
        }
        else
        {
            hide.Visible = false;
            HelperFunction.MsgBox(this, this.GetType(), "Select Employee.");
        }
    }

    protected void gridEmpLeave_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblPK_ID = e.Row.FindControl("lblPK_ID") as Label;
            Label lblleaveType = e.Row.FindControl("lblleaveType") as Label;
            Label lblLeaveName = e.Row.FindControl("lblLeaveName") as Label;
            Label lblApplyDate = e.Row.FindControl("lblApplyDate") as Label;
            Label lblFromDate = e.Row.FindControl("lblFromDate") as Label;
            Label lblToDate = e.Row.FindControl("lblToDate") as Label;
            Label lblRECOMMEND_BY = e.Row.FindControl("lblRECOMMEND_BY") as Label;
            Label lblAPPROVE_BY = e.Row.FindControl("lblAPPROVE_BY") as Label;

            LTEnt = new HR_LEAVE_TYPE();
            LTEnt.PK_ID = lblleaveType.Text;
            LTEnt = (HR_LEAVE_TYPE)LTSer.GetSingle(LTEnt);
            if (LTEnt != null)
                lblLeaveName.Text = LTEnt.LEAVE_TYPE;


            ELMEnt = new HR_EMP_LEAVE_MASTER();
            ELMEnt.PK_ID = lblPK_ID.Text;
            ELMEnt = (HR_EMP_LEAVE_MASTER)ELMSer.GetSingle(ELMEnt);
            if (ELMEnt != null)
            {
                lblApplyDate.Text = ELMEnt.APPLY_YEAR + "/" + ELMEnt.APPLY_MONTH + "/" + ELMEnt.APPLY_DAY;
                lblFromDate.Text = ELMEnt.FROM_YEAR + "/" + ELMEnt.FROM_MONTH + "/" + ELMEnt.FROM_DAY;
                lblToDate.Text = ELMEnt.TO_YEAR + "/" + ELMEnt.TO_MONTH + "/" + ELMEnt.TO_DAY;
                lblRECOMMEND_BY.Text = hf.getEmployeeName(ELMEnt.RECOMMEND_BY);
                lblAPPROVE_BY.Text = hf.getEmployeeName(ELMEnt.APPROVE_BY);
            }
        }
    }


    protected void ddlDepartmentSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDivisionSearch();
    }

    protected void ddlDivisionSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadEmployeesSearch();
    }

    protected void LoadCompanyDetail()
    {
        NCEnt = new NAME_COMPANY();
        NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
        if (NCEnt != null)
        {
            lblCompanyName.Text = ED.Decrypt(NCEnt.ORG_NAME);
            lblAddress.Text = ED.Decrypt(NCEnt.ORG_ADDRESS);
            lblContactNo.Text = NCEnt.CONTACT_DETAIL;
        }

        if (ddlReportType.SelectedValue == "Summary")
        {
            lblReport.Text = "Summary Leave Report - " + ddlEmployeeSearch.SelectedItem.ToString();
        }
        else
        {
            lblReport.Text = "Detail Leave Report- " + ddlEmployeeSearch.SelectedItem.ToString();
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "CallPrint();", true);
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.AddHeader("Content-Disposition", "attachment;filename=Emp_Leave_XLS" + "_" + ddlEmployeeSearch.SelectedItem.ToString() + ".xls");
        Response.ContentType = "application/excel";
        Response.ContentEncoding = Encoding.UTF8;

        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);

        tblExportExcel.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.End();
    }




    protected void gridEmpRemLeave_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblAllotedDays = e.Row.FindControl("lblAllotedDays") as Label;
            Label lblleave_taken = e.Row.FindControl("lblleave_taken") as Label;
            Label lblREM_DAYS = e.Row.FindControl("lblREM_DAYS") as Label;

            lblREM_DAYS.Text = (Convert.ToDouble(lblAllotedDays.Text) - Convert.ToDouble(lblleave_taken.Text)).ToString();
        }
    }


}