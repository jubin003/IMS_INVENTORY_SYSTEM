using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using DataHelper.Framework;
using System.Data;
using System.Globalization;
using PhyeGanCore;
using System.Web.UI;

public partial class HR_Attendance_Leave_Leave_Form : System.Web.UI.Page
{
    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    HR_PARAMETER PREnt = new HR_PARAMETER();
    HR_PARAMETERService PRSer = new HR_PARAMETERService();

    HR_EMPLOYEES EEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService ESer = new HR_EMPLOYEESService();

    HR_ATTENDANCE HAEnt = new HR_ATTENDANCE();
    HR_AttendanceService HASrv = new HR_AttendanceService();

    HR_EMP_LEAVE HELEnt = new HR_EMP_LEAVE();
    HR_EMP_LEAVEService HELSrv = new HR_EMP_LEAVEService();

    HR_EMP_LEAVE_MASTER ELMEnt = new HR_EMP_LEAVE_MASTER();
    HR_EMP_LEAVE_MASTERService ELMSer = new HR_EMP_LEAVE_MASTERService();

    HR_DIVISION DivEnt = new HR_DIVISION();
    HR_DIVISIONService DivSrv = new HR_DIVISIONService();

    HR_LEAVE_TYPE HLTEnt = new HR_LEAVE_TYPE();
    HR_LEAVE_TYPEService HLTSrv = new HR_LEAVE_TYPEService();

    LEAVE_APPROVER LAEnt = new LEAVE_APPROVER();
    LEAVE_APPROVERService LASer = new LEAVE_APPROVERService();

    SET_HR_DUTY_HOUR SHDHEnt = new SET_HR_DUTY_HOUR();
    SET_HR_DUTY_HOURService SHDHSer = new SET_HR_DUTY_HOURService();

    SET_HR_DUTY_ROSTER SHDREnt = new SET_HR_DUTY_ROSTER();
    SET_HR_DUTY_ROSTERService SHDRSer = new SET_HR_DUTY_ROSTERService();

    HR_HOLIDAY HHEnt = new HR_HOLIDAY();
    HR_holidayService HHSer = new HR_holidayService();

    ADBS ADBSEnt = new ADBS();
    ADBSService ADBSSer = new ADBSService();

    PhyeGanDate pgd = new PhyeGanDate();


    HelperFunction hf = new HelperFunction();
    HumanResourceFunction hr = new HumanResourceFunction();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtApplyDate.Text = pgd.GetTodayNepaliDate();
            txtFromDate.Text = pgd.GetTodayNepaliDate();
            txtFDate.Text = pgd.GetTodayNepaliDate();
            txtTDate.Text = pgd.GetTodayNepaliDate();

            txtToDate.Text = pgd.GetTodayNepaliDate();
            loadDepartment();
            LoadDivision();
            LoadLeaveType();
            loadApprovedBy();
            loadRecommendBy();
            ddlEmployees.Items.Insert(0, "Select");
        }
    }

    protected void loadDepartment()
    {
        DPEnt = new HR_DEPARTMENT();
        ddlDept.DataSource = DPSer.GetAll(DPEnt);
        ddlDept.DataTextField = "DEP_NAME";
        ddlDept.DataValueField = "PK_ID";
        ddlDept.DataBind();
        ddlDept.Items.Insert(0, "Select");

        DPEnt = new HR_DEPARTMENT();
        ddlDepartmentSearch.DataSource = DPSer.GetAll(DPEnt);
        ddlDepartmentSearch.DataTextField = "DEP_NAME";
        ddlDepartmentSearch.DataValueField = "PK_ID";
        ddlDepartmentSearch.DataBind();
        ddlDepartmentSearch.Items.Insert(0, "Select");

    }
    protected void LoadDivision()
    {
        DivEnt = new HR_DIVISION();
        if (ddlDept.SelectedValue != "Select")
        {
            DivEnt.DEP_ID = ddlDept.SelectedValue;
        }
        else
        {
            DivEnt.DEP_ID = "";
        }
        DivEnt.STATUS = "1";
        ddlDiv.DataSource = DivSrv.GetAll(DivEnt);
        ddlDiv.DataTextField = "DIVISION_NAME";
        ddlDiv.DataValueField = "PK_ID";
        ddlDiv.DataBind();
        ddlDiv.Items.Insert(0, "Select");
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
    protected void LoadEmployees()
    {
        ddlEmployees.Items.Clear();
        EEnt = new HR_EMPLOYEES();
        if (ddlDept.SelectedValue != "Select")
        {
            EEnt.DEPARTMENTID = ddlDept.SelectedValue;
        }
        if (ddlDiv.SelectedValue != "Select")
        {
            EEnt.DIVISION = ddlDiv.SelectedValue;
        }
        else
        {
            EEnt.DIVISION = "";
        }
        EEnt.STATUS = "1";

        ddlEmployees.DataSource = ESer.GetAll(EEnt);
        ddlEmployees.DataTextField = "FULL_NAME";
        ddlEmployees.DataValueField = "EMPLOYEEID";
        ddlEmployees.DataBind();
        ddlEmployees.Items.Insert(0, "Select");
    }

    protected void LoadEmployeesSearch()
    {
        ddlEmployees.Items.Clear();
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

    protected void loadRecommendBy()
    {
        ddlRecommendBy.DataSource = hf.getLeaveApprover("1", null);
        ddlRecommendBy.DataTextField = "EMPLOYEE_NAME";
        ddlRecommendBy.DataValueField = "EMP_ID";
        ddlRecommendBy.DataBind();
        ddlRecommendBy.Items.Insert(0, "Select");

    }

    protected void loadApprovedBy()
    {
        ddlApprovedBy.DataSource = hf.getLeaveApprover(null, "1");
        ddlApprovedBy.DataTextField = "EMPLOYEE_NAME";
        ddlApprovedBy.DataValueField = "EMP_ID";
        ddlApprovedBy.DataBind();
        ddlApprovedBy.Items.Insert(0, "Select");

    }

    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDivision();
        LoadEmployees();
    }

    protected void ddlDiv_SelectedIndexChanged1(object sender, EventArgs e)
    {
        LoadEmployees();
        loadApprovedBy();
    }

    protected void ddlEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEmployees.SelectedValue != "Select")
        {
            EEnt = new HR_EMPLOYEES();
            EEnt.EMPLOYEEID = ddlEmployees.SelectedValue;
            EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
            if (EEnt != null)
            {
                if (EEnt.DEPARTMENTID == "-1")
                {
                    ddlDept.SelectedValue = "Select";
                }
                else if (EEnt.DEPARTMENTID != "")
                {
                    ddlDept.SelectedValue = EEnt.DEPARTMENTID;
                }
                else
                {
                    ddlDept.SelectedValue = "Select";
                }
                LoadDivision();
                if (EEnt.DIVISION != "")
                {
                    ddlDiv.SelectedValue = EEnt.DIVISION;
                }
                else
                {
                    ddlDiv.SelectedValue = "Select";
                }
            }
            ddlLeaveType.SelectedValue = "Select";
            ddlLeaveType.Enabled = true;
            lblRemDaysLabel.Visible = false;
            lblRemLeave.Visible = false;
        }
        else
        {
            ddlLeaveType.SelectedValue = "Select";
            ddlLeaveType.Enabled = false;
        }
    }
    protected void clearFields()
    {
        ddlDiv.SelectedIndex = 0;
        ddlLeaveType.SelectedIndex = 0;
        txtRemarks.Text = "";
        ddlRecommendBy.SelectedValue = "Select";
        ddlApprovedBy.SelectedValue = "Select";
        txtFromDate.Text = "";
        txtApplyDate.Text = "";
        txtToDate.Text = "";
    }
    protected void LoadLeaveType()
    {
        HLTEnt = new HR_LEAVE_TYPE();
        HLTEnt.STATUS = "1";
        ddlLeaveType.DataSource = HLTSrv.GetAll(HLTEnt);
        ddlLeaveType.DataTextField = "LeaveDays";
        ddlLeaveType.DataValueField = "PK_ID";
        ddlLeaveType.DataBind();
        ddlLeaveType.Items.Insert(0, "Select");
    }
    protected void LoadLeaveDetail()
    {
        double totalLeaveDays = 0;
        #region to make dummy grid to count leave date
        DataTable dt = new DataTable();

        gridDate.DataSource = dt;
        gridDate.DataBind();

        dt.Columns.Add("Date");
        string nextday = "";
        int row = 0;
        string[] Fromdate = txtFromDate.Text.Split('/');
        string[] todate = txtToDate.Text.Split('/');

        string fdate = pgd.ConvertNepaliTOEnglish(Fromdate[0], Fromdate[1], Fromdate[2]);
        string tdate = pgd.ConvertNepaliTOEnglish(todate[0], todate[1], todate[2]);

        DateTime FDay = DateTime.ParseExact(fdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        DateTime TDay = DateTime.ParseExact(tdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        double datediff = (TDay - FDay).TotalDays;
        for (double i = 0; i <= datediff; i++)
        {
            nextday = FDay.AddDays(row).ToString("dd/MM/yyyy");
            dt.Rows.Add();
            dt.Rows[row]["Date"] = nextday;
            row++;
        }

        gridDate.DataSource = dt;
        gridDate.DataBind();
        #endregion
        foreach (GridViewRow gr in gridDate.Rows)
        {
            Label lblDays = gr.FindControl("lblDays") as Label;
            totalLeaveDays += Convert.ToDouble(lblDays.Text);
        }
        lblRequestDays.Text = totalLeaveDays.ToString();
        lblRemLeave.Text = hr.getRemainingLeaveDays(ddlEmployees.SelectedValue, ddlLeaveType.SelectedValue, pgd.NepaliYear());
        if (Convert.ToDouble(lblRequestDays.Text) > Convert.ToDouble(lblRemLeave.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Requested Days exceed Remaining Days. Please select another leave type");
            ddlLeaveType.Focus();
        }
    }
    protected bool checkDays(out string msg)
    {
        bool valid = true;
        string[] Fromdate = txtFromDate.Text.Split('/');
        string[] todate = txtToDate.Text.Split('/');

        string fdate = pgd.ConvertNepaliTOEnglish(Fromdate[0], Fromdate[1], Fromdate[2]);
        string tdate = pgd.ConvertNepaliTOEnglish(todate[0], todate[1], todate[2]);
        DateTime fromDate = DateTime.ParseExact(fdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        DateTime toDate = DateTime.ParseExact(tdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        // difference in days (inclusive)
        double days = (toDate - fromDate).Days + 1;

        if (ddlLeaveNature.SelectedValue == "HD") // Half Day
        {
            days = days / 2;
        }
        if (Convert.ToDouble(lblRequestDays.Text) > Convert.ToDouble(lblRemLeave.Text))
        {
            valid = false;
            msg = "Insufficient Leave Days";
        }
        else
        {
            valid = Convert.ToDouble(lblRequestDays.Text != "" ? lblRequestDays.Text : "0") == days;
            msg = "Date not aligned try clicking calculate first!!";
        }

        return valid;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        string leave_master_id = "";
        string[] Fromdate = txtFromDate.Text.Split('/');
        string[] todate = txtToDate.Text.Split('/');

        string fdate = pgd.ConvertNepaliTOEnglish(Fromdate[0], Fromdate[1], Fromdate[2]);
        string tdate = pgd.ConvertNepaliTOEnglish(todate[0], todate[1], todate[2]);
        string msg;


        if (lblRequestDays.Text != "" && ddlEmployees.SelectedValue != "Select" && ddlLeaveType.SelectedValue != "Select"
            && ddlRecommendBy.SelectedValue != "Select" && ddlApprovedBy.SelectedValue != "Select")
        {
            if (!checkDays(out msg))
            {
                HelperFunction.MsgBox(this, this.GetType(), msg);
                return;

            }
            DistributedTransaction DT = new DistributedTransaction();
            if (lblPk_idLeave.Text == "")
            {
                #region to insert in HR_EMP_LEAVE_MASTER
                ELMEnt = new HR_EMP_LEAVE_MASTER();
                ELMEnt.EMPLOYEE_ID = ddlEmployees.SelectedValue;
                ELMEnt.FROM_DATE = fdate;
                ELMEnt.TO_DATES = tdate;
                string[] Applydate = txtApplyDate.Text.Split('/');
                ELMEnt.APPLY_DAY = Applydate[0];
                ELMEnt.APPLY_MONTH = Applydate[1];
                ELMEnt.APPLY_YEAR = Applydate[2];
                ELMEnt.APPLY_DATE = pgd.ConvertNepaliTOEnglish(Applydate[0], Applydate[1], Applydate[2]);

                ELMEnt.LEAVE_TYPE = ddlLeaveType.SelectedValue;
                ELMEnt.STATUS = "1";
                ELMEnt.REMARKS = txtRemarks.Text;
                ELMEnt.RECOMMEND_BY = ddlRecommendBy.SelectedValue;
                ELMEnt.APPROVE_BY = ddlApprovedBy.SelectedValue;

                ELMEnt.FROM_DAY = Fromdate[0];
                ELMEnt.FROM_MONTH = Fromdate[1];
                ELMEnt.FROM_YEAR = Fromdate[2];
                ELMEnt.FROM_DATE = pgd.ConvertNepaliTOEnglish(Fromdate[0], Fromdate[1], Fromdate[2]);

                ELMEnt.TO_DAY = todate[0];
                ELMEnt.TO_MONTH = todate[1];
                ELMEnt.TO_YEAR = todate[2];
                ELMEnt.TO_DATES = pgd.ConvertNepaliTOEnglish(todate[0], todate[1], todate[2]);
                ELMEnt.DAYS = lblRequestDays.Text;
                leave_master_id = ELMSer.Insert(ELMEnt, DT).ToString();

                #endregion

                #region to insert in leave detail and affect attendance
                foreach (GridViewRow gr in gridDate.Rows)
                {
                    Label lblEngDate = gr.FindControl("lblEngDate") as Label;
                    Label lblNepDay = gr.FindControl("lblNepDay") as Label;
                    Label lblNepMonth = gr.FindControl("lblNepMonth") as Label;
                    Label lblNepYear = gr.FindControl("lblNepYear") as Label;
                    Label lblDays = gr.FindControl("lblDays") as Label;

                    HELEnt = new HR_EMP_LEAVE();
                    HELEnt.HR_EMP_LEAVE_MASTER_ID = leave_master_id;
                    HELEnt.EMP_ID = ddlEmployees.SelectedValue;
                    HELEnt.LEAVE_DAY = lblNepDay.Text;
                    HELEnt.LEAVE_MONTH = lblNepMonth.Text;
                    HELEnt.LEAVE_YEAR = lblNepYear.Text;
                    HELEnt.LEAVE_DATE = lblEngDate.Text;
                    if (ddlLeaveNature.SelectedValue == "FD")
                        HELEnt.LEAVE_NATURE = "1"; // Full Day Leave
                    else
                        HELEnt.LEAVE_NATURE = "0.5"; // Half Day Leave
                    HELEnt.STATUS = "1";
                    if (Convert.ToDouble(lblDays.Text) > 0)
                    {
                        HELSrv.Insert(HELEnt, DT);

                        HAEnt = new HR_ATTENDANCE();
                        HAEnt.ATT_DATE = lblEngDate.Text;
                        HAEnt.EMPLOYEE_ID = ddlEmployees.SelectedValue;
                        HAEnt = (HR_ATTENDANCE)HASrv.GetSingle(HAEnt);
                        if (HAEnt != null)
                        {
                            HAEnt.IN_TIME = "00:00:00 AM";
                            HAEnt.OUT_TIME = "00:00:00 AM";
                            HAEnt.WORKING_HOUR = "00:00:00";
                            if (ddlLeaveNature.SelectedValue == "FD")
                                HAEnt.STATUS = "4"; // Full Day Leave
                            else
                                HAEnt.STATUS = "5"; // Half Day Leave
                            HAEnt.REMARKS = txtRemarks.Text;
                            HASrv.Update(HAEnt, DT);
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region to update in HR_EMP_LEAVE_MASTER
                ELMEnt = new HR_EMP_LEAVE_MASTER();
                ELMEnt.PK_ID = lblPk_idLeave.Text;
                ELMEnt = (HR_EMP_LEAVE_MASTER)ELMSer.GetSingle(ELMEnt);
                if (ELMEnt != null)
                {
                    ELMEnt.EMPLOYEE_ID = ddlEmployees.SelectedValue;
                    ELMEnt.FROM_DATE = fdate;
                    ELMEnt.TO_DATES = tdate;
                    string[] Applydate = txtApplyDate.Text.Split('/');
                    ELMEnt.APPLY_DAY = Applydate[0];
                    ELMEnt.APPLY_MONTH = Applydate[1];
                    ELMEnt.APPLY_YEAR = Applydate[2];
                    ELMEnt.APPLY_DATE = pgd.ConvertNepaliTOEnglish(Applydate[0], Applydate[1], Applydate[2]);

                    ELMEnt.LEAVE_TYPE = ddlLeaveType.SelectedValue;
                    ELMEnt.STATUS = "1";
                    ELMEnt.REMARKS = txtRemarks.Text;
                    ELMEnt.RECOMMEND_BY = ddlRecommendBy.SelectedValue;
                    ELMEnt.APPROVE_BY = ddlApprovedBy.SelectedValue;

                    ELMEnt.FROM_DAY = Fromdate[0];
                    ELMEnt.FROM_MONTH = Fromdate[1];
                    ELMEnt.FROM_YEAR = Fromdate[2];
                    ELMEnt.FROM_DATE = pgd.ConvertNepaliTOEnglish(Fromdate[0], Fromdate[1], Fromdate[2]);

                    ELMEnt.TO_DAY = todate[0];
                    ELMEnt.TO_MONTH = todate[1];
                    ELMEnt.TO_YEAR = todate[2];
                    ELMEnt.TO_DATES = pgd.ConvertNepaliTOEnglish(todate[0], todate[1], todate[2]);
                    ELMEnt.DAYS = lblRequestDays.Text;
                    ELMSer.Update(ELMEnt, DT).ToString();
                }

                #endregion
                #region to Delete existing records i.e leaves
                HELEnt = new HR_EMP_LEAVE();
                HELEnt.HR_EMP_LEAVE_MASTER_ID = lblPk_idLeave.Text; ;

                HELSrv.Delete(HELEnt);


                #endregion
                #region to insert in leave detail and affect attendance
                foreach (GridViewRow gr in gridDate.Rows)
                {
                    Label lblEngDate = gr.FindControl("lblEngDate") as Label;
                    Label lblNepDay = gr.FindControl("lblNepDay") as Label;
                    Label lblNepMonth = gr.FindControl("lblNepMonth") as Label;
                    Label lblNepYear = gr.FindControl("lblNepYear") as Label;
                    Label lblDays = gr.FindControl("lblDays") as Label;

                    HELEnt = new HR_EMP_LEAVE();
                    HELEnt.HR_EMP_LEAVE_MASTER_ID = lblPk_idLeave.Text;
                    HELEnt.EMP_ID = ddlEmployees.SelectedValue;
                    HELEnt.LEAVE_DAY = lblNepDay.Text;
                    HELEnt.LEAVE_MONTH = lblNepMonth.Text;
                    HELEnt.LEAVE_YEAR = lblNepYear.Text;
                    HELEnt.LEAVE_DATE = lblEngDate.Text;
                    if (ddlLeaveNature.SelectedValue == "FD")
                        HELEnt.LEAVE_NATURE = "1"; // Full Day Leave
                    else
                        HELEnt.LEAVE_NATURE = "0.5"; // Half Day Leave
                    HELEnt.STATUS = "1";
                    if (Convert.ToDouble(lblDays.Text) > 0)
                    {
                        HELSrv.Insert(HELEnt, DT);

                        HAEnt = new HR_ATTENDANCE();
                        HAEnt.ATT_DATE = lblEngDate.Text;
                        HAEnt.EMPLOYEE_ID = ddlEmployees.SelectedValue;
                        HAEnt = (HR_ATTENDANCE)HASrv.GetSingle(HAEnt);
                        if (HAEnt != null)
                        {
                            HAEnt.IN_TIME = "";
                            HAEnt.OUT_TIME = "";
                            HAEnt.WORKING_HOUR = "";
                            if (ddlLeaveNature.SelectedValue == "FD")
                                HAEnt.STATUS = "4"; // Full Day Leave
                            else
                                HAEnt.STATUS = "5"; // Half Day Leave
                            HAEnt.REMARKS = txtRemarks.Text;
                            HASrv.Update(HAEnt, DT);
                        }
                    }
                }
                #endregion
            }
            if (DT.HAPPY == true)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Data Saved Successfully");
                DT.Commit();
                clearField();
                LoadLeaveGrid();
                divEditGrid.Visible = true;
                divLeave.Visible = false;
            }
            else
            {
                HelperFunction.MsgBox(this, this.GetType(), "Not inserted ");
                DT.Abort();
            }
            DT.Dispose();
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please fill the form compeletely.");
        }
    }

    protected void gridDate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblEngDate = e.Row.FindControl("lblEngDate") as Label;
            Label lblNepDate = e.Row.FindControl("lblNepDate") as Label;
            Label lblDay = e.Row.FindControl("lblDay") as Label;
            Label lblDays = e.Row.FindControl("lblDays") as Label;

            Label lblNepDay = e.Row.FindControl("lblNepDay") as Label;
            Label lblNepMonth = e.Row.FindControl("lblNepMonth") as Label;
            Label lblNepYear = e.Row.FindControl("lblNepYear") as Label;

            string[] nepdate = pgd.ConvertEnglishToNepali(lblEngDate.Text);
            lblNepDay.Text = nepdate[0];
            lblNepMonth.Text = nepdate[1];
            lblNepYear.Text = nepdate[2];


            lblNepDate.Text = pgd.GetNepaliDateFromEnglish(lblEngDate.Text, "yyyy/mm/dd");
            lblDay.Text = pgd.getDayfromDate(lblEngDate.Text);


            PREnt = new HR_PARAMETER();
            PREnt = (HR_PARAMETER)PRSer.GetSingle(PREnt);
            if (PREnt != null && PREnt.CALCULATE_SANDWICH == "Y") // calculatinng offdays and holiday as sandwich leave
            {
                if (ddlLeaveNature.SelectedValue == "FD")
                {
                    lblDays.Text = "1";
                }
                else if (ddlLeaveNature.SelectedValue == "HD")
                {
                    lblDays.Text = "0.5";
                }
            }
            else// not to calculat offdays and holiday as sandwich leave
            {
                bool is_offday = false;
                SHDHEnt = new SET_HR_DUTY_HOUR();
                SHDHEnt.EMP_ID = ddlEmployeeSearch.SelectedValue;
                SHDHEnt = (SET_HR_DUTY_HOUR)SHDHSer.GetSingle(SHDHEnt);
                if (SHDHEnt != null)
                {
                    if (SHDHEnt.FIXED_DUTY_ROSTER == "1") // duty roster fix bhaye off day pani fix huncha so fix off day check garni
                    {
                        ADBSEnt = new ADBS();
                        ADBSEnt.AD_DATE = lblEngDate.Text;
                        ADBSEnt = (ADBS)ADBSSer.GetSingle(ADBSEnt);
                        if (ADBSEnt != null)
                        {
                            if (ADBSEnt.DAY_N == SHDHEnt.WEEKEND1) // fix off day 1
                                is_offday = true;
                            if (ADBSEnt.DAY_N == SHDHEnt.WEEKEND2) // fix off day 2
                                is_offday = true;
                        }
                    }

                    HHEnt = new HR_HOLIDAY();
                    HHEnt.ENGDATE = lblEngDate.Text;
                    HHEnt = (HR_HOLIDAY)HHSer.GetSingle(HHEnt);
                    if (HHEnt != null)
                    {
                        is_offday = true;
                    }

                    if (is_offday == false)
                    {
                        if (ddlLeaveNature.SelectedValue == "FD")
                        {
                            lblDays.Text = "1";
                        }
                        else if (ddlLeaveNature.SelectedValue == "HD")
                        {
                            lblDays.Text = "0.5";
                        }
                    }
                    else
                    {
                        lblDays.Text = "0";
                    }
                }

            }

        }
    }

    protected void ddlDiv_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadEmployees();
    }

    protected void ddlLeaveType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLeaveType.SelectedValue == "Select")
        {
            lblRemDaysLabel.Visible = false;
            lblRemLeave.Visible = false;
        }
        else
        {
            lblRemDaysLabel.Visible = true;
            lblRemLeave.Visible = true;
            lblRemLeave.Text = hr.getRemainingLeaveDays(ddlEmployees.SelectedValue, ddlLeaveType.SelectedValue, pgd.NepaliYear());
            double remleavedays = Convert.ToDouble(hr.getRemainingLeaveDays(ddlEmployees.SelectedValue, ddlLeaveType.SelectedValue, pgd.NepaliYear()));
            HLTEnt = new HR_LEAVE_TYPE();
            HLTEnt.PK_ID = ddlLeaveType.SelectedValue;
            HLTEnt = (HR_LEAVE_TYPE)HLTSrv.GetSingle(HLTEnt);
            if (HLTEnt != null)
            {
                if (remleavedays < 1)
                {
                    HelperFunction.MsgBox(this, this.GetType(), "leave exceeded. Please choose another Leave Type");
                    btnSave.Enabled = false;
                }
                else
                    btnSave.Enabled = true;
            }
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        if (txtToDate.Text != "")
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);

                if (FromDate.Year != ToDate.Year)
                {
                    btnSave.Visible = false;
                    HelperFunction.MsgBox(this, this.GetType(), "Please select the date of same year. Next Year's leaves are calculated separately");
                }
                else
                {
                    LoadLeaveDetail();
                    btnSave.Visible = true;
                }
            }
            catch { }
        }

    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        if (txtFromDate.Text != "")
        {
            try
            {
                DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);

                if (FromDate.Year != ToDate.Year)
                {
                    btnSave.Visible = false;
                    HelperFunction.MsgBox(this, this.GetType(), "Please select the date of same year. Next Year's leaves are calculated separately");
                }
                else
                {
                    LoadLeaveDetail();
                    btnSave.Visible = true;
                }
            }
            catch { }
            finally
            {

            }
        }
    }

    public string checkSaturday(string date)
    {

        string dayofweek = "";
        string[] dd = date.Split('/');

        int month = Convert.ToInt32(dd[0]);
        int day = Convert.ToInt32(dd[1]);
        int year = Convert.ToInt32(dd[2]);
        dayofweek = new DateTime(year, month, day).DayOfWeek.ToString();

        return dayofweek;

    }

    protected void clearField()
    {
        ddlDept.SelectedValue = "Select";
        ddlDiv.SelectedValue = "Select";
        ddlLeaveType.SelectedValue = "Select";
        txtFromDate.Text = pgd.GetTodayNepaliDate();
        txtToDate.Text = pgd.GetTodayNepaliDate();
        txtApplyDate.Text = pgd.GetTodayNepaliDate();
        ddlRecommendBy.SelectedValue = "Select";
        ddlApprovedBy.SelectedValue = "Select";
        ddlLeaveType.SelectedIndex = 0;
        ddlLeaveType.Enabled = false;
        lblRemDaysLabel.Visible = false;
        lblRemLeave.Visible = false;
        txtRemarks.Text = "";
        lblRequestDays.Text = "";
        lblRequestDaysLabel.Visible = false;
        DataTable dt = new DataTable();
        lblPk_idLeave.Text = "";
        gridDate.DataSource = dt;
        gridDate.DataBind();
    }


    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        LoadLeaveDetail();
        lblRequestDaysLabel.Visible = true;
        lblRequestDays.Visible = true;
    }

    protected void btn_view_Click(object sender, EventArgs e)
    {
        LoadLeaveGrid();
        divEditGrid.Visible = true;
        divLeave.Visible = false;
    }

    protected void btn_add_Click(object sender, EventArgs e)
    {
        divEditGrid.Visible = false;
        divLeave.Visible = true;
        clearFields();
        clearField();
    }

    DataTable dt = new DataTable();

    protected void LoadExistingLeaveDetail(string emp_leave_master_id)
    {
        // Create datatable with columns if not already created
        if (dt.Columns.Count == 0)
        {
            dt.Columns.Add("DATE");
            dt.Columns.Add("NEP_YEAR");
            dt.Columns.Add("NEP_MONTH");
            dt.Columns.Add("NEP_DAY");
            dt.Columns.Add("DAYS");
        }

        HELEnt = new HR_EMP_LEAVE();
        HELEnt.HR_EMP_LEAVE_MASTER_ID = emp_leave_master_id;
        var emp_leave = HELSrv.GetAll(HELEnt);

        foreach (HR_EMP_LEAVE l in emp_leave)
        {
            DataRow row = dt.NewRow();
            row["DATE"] = l.LEAVE_DATE;
            row["NEP_YEAR"] = l.LEAVE_YEAR;
            row["NEP_MONTH"] = l.LEAVE_MONTH;
            row["NEP_DAY"] = l.LEAVE_DAY;
            row["DAYS"] = 1;

            dt.Rows.Add(row);
        }
        gridDate.DataSource = dt;
        gridDate.DataBind();

    }

    protected void gridLeaveEdit_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Alter"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
            ELMEnt = new HR_EMP_LEAVE_MASTER();
            ELMEnt.PK_ID = lblPK_ID.Text;
            ELMEnt = (HR_EMP_LEAVE_MASTER)ELMSer.GetSingle(ELMEnt);
            if (ELMEnt != null)
            {
                ELMEnt.STATUS = "0";
                ELMSer.Update(ELMEnt);
            }
        }
        if (e.CommandName.Equals("Change"))
        {
            divEditGrid.Visible = false;
            divLeave.Visible = true;
            GridViewRow row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            int rowIndex = row.RowIndex;
            Label lblPk_ID = (Label)row.FindControl("lblPk_ID");
            Label lblEmpId = (Label)row.FindControl("lblEmpId");
            lblPk_idLeave.Text = lblPk_ID.Text;
            ELMEnt = new HR_EMP_LEAVE_MASTER();
            ELMEnt.PK_ID = lblPk_ID.Text;
            ELMEnt = (HR_EMP_LEAVE_MASTER)ELMSer.GetSingle(ELMEnt);
            if (ELMEnt != null)
            {
                txtApplyDate.Text = pgd.GetNepaliDateFromEnglish(ELMEnt.APPLY_DATE, "dd/mm/yyyy");
                txtRemarks.Text = ELMEnt.REMARKS;
                ddlRecommendBy.SelectedValue = ELMEnt.RECOMMEND_BY;
                ddlApprovedBy.SelectedValue = ELMEnt.APPROVE_BY;
                txtFromDate.Text = pgd.GetNepaliDateFromEnglish(ELMEnt.FROM_DATE, "dd/mm/yyyy");
                txtToDate.Text = pgd.GetNepaliDateFromEnglish(ELMEnt.TO_DATES, "dd/mm/yyyy");
                ddlLeaveType.SelectedValue = ELMEnt.LEAVE_TYPE;
                ddlLeaveType.Enabled = true;

                // determine leave nature from DB before loading grid
                HELEnt = new HR_EMP_LEAVE();
                HELEnt.HR_EMP_LEAVE_MASTER_ID = ELMEnt.PK_ID;
                var leaveList = HELSrv.GetAll(HELEnt);
                ddlLeaveNature.SelectedValue = "FD"; // default
                foreach (HR_EMP_LEAVE l in leaveList)
                {
                    if (l.LEAVE_NATURE == "0.5")
                    {
                        ddlLeaveNature.SelectedValue = "HD";
                    }
                    break; // only check first record
                }

                LoadExistingLeaveDetail(ELMEnt.PK_ID); // only once, no LoadLeaveDetail()

                EEnt = new HR_EMPLOYEES();
                EEnt.EMPLOYEEID = ELMEnt.EMPLOYEE_ID;
                EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
                if (EEnt != null)
                {
                    loadDepartment();
                    ddlDept.SelectedValue = EEnt.DEPARTMENTID;
                    LoadDivision();
                    ddlDiv.SelectedValue = EEnt.DIVISION;
                    LoadEmployees();
                    ddlEmployees.SelectedValue = ELMEnt.EMPLOYEE_ID;

                    double LeaveDays = 0;
                    foreach (GridViewRow gr in gridDate.Rows)
                    {
                        Label lblDays = gr.FindControl("lblDays") as Label;
                        LeaveDays += Convert.ToDouble(lblDays.Text);
                    }
                    lblRequestDays.Text = LeaveDays.ToString();

                    double rem_day = Convert.ToDouble(hr.getRemainingLeaveDays(ddlEmployees.SelectedValue, ddlLeaveType.SelectedValue, pgd.NepaliYear()));
                    double edit_leaveteken = Convert.ToDouble(ELMEnt.DAYS);
                    lblRemLeave.Text = (rem_day + edit_leaveteken).ToString();

                    lblRequestDays.Visible = true;
                    lblRemDaysLabel.Visible = true;
                    lblRemLeave.Visible = true;
                    lblRequestDaysLabel.Visible = true;
                }
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

    protected void LoadLeaveGrid()
    {
        string[] fdate = txtFDate.Text.Split('/');
        string[] tDate = txtTDate.Text.Split('/');
        gridLeaveEdit.DataSource = hr.getLeaves(pgd.ConvertNepaliTOEnglish(fdate[0], fdate[1], fdate[2]),
            pgd.ConvertNepaliTOEnglish(tDate[0], tDate[1], tDate[2]), ddlEmployeeSearch.SelectedValue);
        gridLeaveEdit.DataBind();
        if (gridLeaveEdit.Rows.Count == 0)
        {
            HelperFunction.MsgBox(this, this.GetType(), "No data found");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divEditGrid.Visible = true;
        divLeave.Visible = false;
    }
}