
#region things to remember
//status:0 in attendance is Off Day
//status:1 in attendance is Present
//status:2 in attendance is Absent
//status:3 in attendance is Holiday
//status:4 in attendance is Leave
//status:5 in attendance is Half Day Leave
//status:6 in attendance is Less Than Half Day
//status:7 in attendance is date greater than today. Attendance is not done yet

#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Globalization;
using Entity.Framework;
using System.IO;
using System.Collections;
using PhyeGanCore;

public partial class HR_Attendance_Attendance_individualAttendance : System.Web.UI.Page
{
    string imgfolder;

    Months MEnt = new Months();
    MonthsService MSer = new MonthsService();

    HR_ATTENDANCE ATEnt = new HR_ATTENDANCE();
    HR_AttendanceService ATSer = new HR_AttendanceService();

    HR_EMPLOYEES EMPEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService EMPSer = new HR_EMPLOYEESService();

    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    HR_EMPLOYEES EEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService ESer = new HR_EMPLOYEESService();

    HR_DIVISION HDIVEnt = new HR_DIVISION();
    HR_DIVISIONService HDIVSer = new HR_DIVISIONService();

    HR_UNIT HUEnt = new HR_UNIT();
    HR_UNITService HUSer = new HR_UNITService();

    HR_DESIGNATION HDEnt = new HR_DESIGNATION();
    HR_DESIGNATIONService HDSer = new HR_DESIGNATIONService();

    HR_EMP_HISTORY HEHEnt = new HR_EMP_HISTORY();
    HR_EMP_HISTORYService HEHSer = new HR_EMP_HISTORYService();

    HR_LEVEL HLEnt = new HR_LEVEL();
    HR_LEVELService HLSer = new HR_LEVELService();

    HR_GRADE HGEnt = new HR_GRADE();
    HR_GRADEService HGSer = new HR_GRADEService();

    EntityList theEntList = new EntityList();

    PhyeGanDate PGD = new PhyeGanDate();

    OFFICE OFEnt = new OFFICE();
    OFFICEService OFSer = new OFFICEService();

    HelperFunction hf = new HelperFunction();

    UserProfileEntity userProfileEnt = new UserProfileEntity();

    static string username;

    string[] nepDate;
    string engDate, inHours, inMins, inAmPm, inTime24, outHours, outMins, outAmPm, outTime24, workingHr = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDepartment();
            rbtnDays.SelectedValue = "0";
            rbtnStatus.SelectedValue = "1";
            multi.Visible = false;
            txtDate.Text = PGD.GetTodayNepaliDate();
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

    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDivision();
    }
    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDivision.SelectedIndex != 0)
        {
            LoadUnit();
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

    protected void bttnSearch_Click(object sender, EventArgs e)
    {
        HR_EMPLOYEES EEnt = new HR_EMPLOYEES();

        EEnt.STATUS = ddlStatus.SelectedValue;

        if (ddlDept.SelectedValue != "Select")
        {
            EEnt.DEPARTMENTID = ddlDept.SelectedValue;
        }
        if (ddlDivision.SelectedValue != "Select")
        {
            EEnt.DIVISION = ddlDivision.SelectedValue;
        }
        if (ddlUnit.SelectedValue != "Select")
        {
            EEnt.UNIT = ddlUnit.SelectedValue;

        }

        IList rawEmployeeList = ESer.GetAll(EEnt);
        List<HR_EMPLOYEES> employeeList = rawEmployeeList.Cast<HR_EMPLOYEES>().ToList();


        var workingList = employeeList
            .Where(x => x.DEPARTMENTID != "-1")
            .ToList();

        if (workingList.Count > 0)
        {
            GridEmpList.DataSource = workingList;
            GridEmpList.DataBind();
            GridEmpList.Visible = true;

            if (ddlStatus.SelectedValue == "0")
            {
                GridEmpList.Columns[5].Visible = false;
            }
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Data not found.");
            GridEmpList.Visible = false;
        }
    }
    protected void ImgBttnEdit_Click(object sender, ImageClickEventArgs e)
    {

        GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");
        tblButtons.Visible = true;
        txtDate.Text = PGD.GetTodayNepaliDate();
        EmpSearch.Visible = false;
        tblEmpDetail.Visible = true;
        detaildiv
            .Visible = true;
        loadEmpDetail(lblEmpID.Text);
        LoadInfo();

    }

    protected void GridEmpList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblEmpID = e.Row.FindControl("lblEmpID") as Label;
            Label lblName = e.Row.FindControl("lblName") as Label;
            Label lblOffLocId = e.Row.FindControl("lblOffLocId") as Label;
            Label lblOffLocName = e.Row.FindControl("lblOffLocName") as Label;
            Label lblOfficeNameId = e.Row.FindControl("lblOfficeNameId") as Label;
            Label lblOffName = e.Row.FindControl("lblOffName") as Label;
            Label lblDiviId = e.Row.FindControl("lblDiviId") as Label;
            Label lblDivi = e.Row.FindControl("lblDivi") as Label;
            Label lblDeptId = e.Row.FindControl("lblDeptId") as Label;
            Label lblDept = e.Row.FindControl("lblDept") as Label;
            Label lblJobTitle = e.Row.FindControl("lblJobTitle") as Label;
            Label lblDiv = e.Row.FindControl("lblDiv") as Label;
            Label lblDivId = e.Row.FindControl("lblDivId") as Label;
            Label lblUnitId = e.Row.FindControl("lblUnitId") as Label;
            Image imgEmpPhoto = (Image)e.Row.FindControl("imgEmpPhoto");
            ImageButton ImgBttnEdit = (ImageButton)e.Row.FindControl("ImgBttnEdit");

            Label lblEmpIdTemp = e.Row.FindControl("lblEmpIdTemp") as Label;

            EEnt = new HR_EMPLOYEES();
            EEnt.EMPLOYEEID = lblEmpID.Text;
            EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
            if (EEnt != null)
            {
                lblName.Text = EEnt.FULL_NAME;
                if (EEnt.STATUS == "0")
                {
                    ImgBttnEdit.Visible = false;
                }
            }

            DPEnt = new HR_DEPARTMENT();
            DPEnt.PK_ID = lblDeptId.Text;
            DPEnt = (HR_DEPARTMENT)DPSer.GetSingle(DPEnt);
            if (DPEnt != null)
            {
                lblDept.Text = DPEnt.DEP_NAME;

                if (EEnt.DIVISION != "")
                {
                    HDIVEnt = new HR_DIVISION();
                    HDIVEnt.PK_ID = lblDivId.Text;
                    HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
                    if (HDIVEnt != null)
                    {
                        lblDept.Text = DPEnt.DEP_NAME + "/" + HDIVEnt.DIVISION_NAME;
                    }
                }

                if (lblUnitId.Text != "")
                {
                    HUEnt = new HR_UNIT();
                    HUEnt.PK_ID = lblUnitId.Text;
                    HUEnt = (HR_UNIT)HUSer.GetSingle(HUEnt);
                    if (HUEnt != null)
                    {
                        lblDept.Text = DPEnt.DEP_NAME + "/" + HDIVEnt.DIVISION_NAME + "/" + HUEnt.UNIT_NAME;

                    }
                }

            }
            imgfolder = Server.MapPath(@"~/images/Employee/") + EEnt.EMPLOYEEID + ".jpg";
            if (File.Exists(imgfolder))
            {
                imgEmpPhoto.ImageUrl = @"~/images/Employee/" + EEnt.EMPLOYEEID + ".jpg";
            }
            else
            {
                if (EEnt.GENDER == "M")
                {
                    imgEmpPhoto.ImageUrl = @"~/images/Employee/male.jpg";
                }
                else
                    imgEmpPhoto.ImageUrl = @"~/images/Employee/female.jpg";
            }
        }
    }

    protected void rbtnDays_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnDays.SelectedValue == "O")
        {
            multi.Visible = false;
            one.Visible = true;
        }
        else if (rbtnDays.SelectedValue == "M")
        {
            multi.Visible = true;
            one.Visible = false;
        }
    }

    protected void loadEmpDetail(string empId)
    {
        EMPEnt = new HR_EMPLOYEES();
        EMPEnt.EMPLOYEEID = empId;

        EMPEnt = (HR_EMPLOYEES)EMPSer.GetSingle(EMPEnt);
        if (EMPEnt != null)
        {
            lblEmpId.Text = EMPEnt.EMPLOYEEID;
            lblEmpName.Text = EMPEnt.FIRSTNAME + " " + EMPEnt.LASTNAME;

        }
        else
        {
            lblEmpId.Text = "";
            lblEmpName.Text = "";
        }
    }

    protected void LoadInfo()
    {
        EMPEnt = new HR_EMPLOYEES();
        EMPEnt.EMPLOYEEID = lblEmpId.Text;

        EMPEnt = (HR_EMPLOYEES)EMPSer.GetSingle(EMPEnt);
        if (EMPEnt != null)
        {
            detaildiv.Visible = true;
            lblEmpId.Text = EMPEnt.EMPLOYEEID;
            lblEmpName.Text = EMPEnt.FIRSTNAME + " " + EMPEnt.LASTNAME;
            lblJobTitle.Text = EMPEnt.JOBTITLE;
            HDEnt = new HR_DESIGNATION();
            HDEnt.PK_ID = EMPEnt.DESIGNATIONID;
            HDEnt = (HR_DESIGNATION)HDSer.GetSingle(HDEnt);
            if (HDEnt != null)
            {
                lblDesignation.Text = HDEnt.DESIGNATION_NAME;
            }
        }
        else
        {
            detaildiv.Visible = false;
        }
    }

    protected void btnSearchOne_Click(object sender, EventArgs e)
    {
        if (txtDate.Text != "")
        {
            processDate();
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Date cannot be empty");
        }
    }

    protected void processDate()
    {
        string[] date = txtDate.Text.Split('/');
        string engDate = PGD.ConvertNepaliTOEnglish(date[0], date[1], date[2]);
        string todayDate = DateTime.Today.ToString("dd/MM/yyyy");

        DateTime engDateValue = DateTime.ParseExact(engDate, "dd/MM/yyyy", null);

        if (engDateValue <= DateTime.Today)
        {
            ATEnt = new HR_ATTENDANCE();
            ATEnt.EMPLOYEE_ID = lblEmpId.Text;
            ATEnt.ATT_DATE = engDate;
            ATEnt = (HR_ATTENDANCE)ATSer.GetSingle(ATEnt);
            if (ATEnt != null)
            {
                btnSave.Enabled = true;
                tblButtons.Visible = true;
                if (ATEnt.IN_TIME == "00:00:00")
                {
                    txtInTimeHr.Text = "00";
                    txtInTimeMin.Text = "00";
                    ddlInAMPM.SelectedValue = "AM";
                }
                else
                {
                    if (ATEnt.IN_TIME != "")
                    {
                        string timeStr = ATEnt.IN_TIME;
                        string[] timeParts = timeStr.Split(' ');
                        string timeComponent = timeParts[0];
                        string period = string.Empty;

                        if (timeParts.Length > 1)
                        {
                            period = timeParts[1]; // AM or PM
                        }
                        else
                        {
                            period = "";
                        }

                        // Split hours, minutes, seconds
                        string[] timeSplit = timeComponent.Split(':');
                        string hr = timeSplit[0];
                        string min = timeSplit[1];
                        string sec = timeSplit[2];

                        txtInTimeHr.Text = hr;
                        txtInTimeMin.Text = min;
                        ddlInAMPM.SelectedValue = period;

                    }
                    else
                    {
                        txtInTimeHr.Text = "00";
                        txtInTimeMin.Text = "00";
                        ddlInAMPM.SelectedValue = "AM";
                    }

                    if (ATEnt.OUT_TIME == "00:00:00")
                    {
                        txtOutTimeHr.Text = "00";
                        txtOutTimeMin.Text = "00";
                        ddlOutAMPM.SelectedValue = "AM";
                    }
                    else
                    {
                        if (ATEnt.OUT_TIME != "")
                        {
                            string timeStr = ATEnt.OUT_TIME;
                            string[] timeParts = timeStr.Split(' ');
                            string timeComponent = timeParts[0];
                            string period = string.Empty;

                            if (timeParts.Length > 1)
                            {
                                period = timeParts[1]; // AM or PM
                            }
                            else
                            {
                                period = "";
                            }

                            // Split hours, minutes, seconds
                            string[] timeSplit = timeComponent.Split(':');
                            string hr = timeSplit[0];
                            string min = timeSplit[1];
                            string sec = timeSplit[2];

                            txtOutTimeHr.Text = hr;
                            txtOutTimeMin.Text = min;
                            ddlOutAMPM.SelectedValue = period;
                        }
                        else
                        {
                            txtOutTimeHr.Text = "00";
                            txtOutTimeMin.Text = "00";
                            ddlOutAMPM.SelectedValue = "AM";
                        }

                    }
                    txtWorkingHour.Text = ATEnt.WORKING_HOUR;          
                    txtRemarks.Text = ATEnt.REMARKS;
                }
            }
            else
            {
                HelperFunction.MsgBox(this, this.GetType(), "Data not Found.");
                btnSave.Enabled = false;
                tblButtons.Visible = false;
            }
        }

        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Cannot edit on this date.");
            txtDate.Text = "";
        }
    }

    protected string getWorkingTime(string inHours, string inMins, string inAmPm, string outHours, string outMins, string outAmPm)
    {
        string inTime24 = Convert12To24HourFormat(inHours, inMins, inAmPm);
        string outTime24 = Convert12To24HourFormat(outHours, outMins, outAmPm);

        try
        {
            DateTime inTime = DateTime.ParseExact(inTime24, "HH:mm", CultureInfo.InvariantCulture);
            DateTime outTime = DateTime.ParseExact(outTime24, "HH:mm", CultureInfo.InvariantCulture);

            // Handle overnight shifts (e.g., 22:00 → 06:00)
            if (outTime < inTime)
                outTime = outTime.AddDays(1);

            TimeSpan workDuration = outTime - inTime;

            // Return formatted string "HH:mm:ss"
            return string.Format(
                "{0:00}:{1:00}:{2:00}",
                (int)workDuration.TotalHours,
                workDuration.Minutes,
                workDuration.Seconds
            );
        }
        catch
        {
            return "00:00:00"; // Fallback on error
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        nepDate = txtDate.Text.Split('/');
        engDate = PGD.ConvertNepaliTOEnglish(nepDate[0], nepDate[1], nepDate[2]);
        //getWorkingTime();
        ATEnt = new HR_ATTENDANCE();
        ATEnt.EMPLOYEE_ID = lblEmpId.Text;
        ATEnt.ATT_DATE = engDate;

        ATEnt = (HR_ATTENDANCE)ATSer.GetSingle(ATEnt);
        if (ATEnt != null)
        {
            ATEnt.IN_TIME = txtInTimeHr.Text + ":" + txtInTimeMin.Text + ":00 " + ddlInAMPM.SelectedValue;
            ATEnt.OUT_TIME = txtOutTimeHr.Text + ":" + txtOutTimeMin.Text + ":00 " + ddlOutAMPM.SelectedValue;
            ATEnt.WORKING_HOUR = txtWorkingHour.Text;
            ATEnt.REMARKS = txtRemarks.Text;
            ATEnt.STATUS = rbtnStatus.SelectedValue;
            ATEnt.LOGINID = userProfileEnt.UserName;
            ATEnt.DATE_TIME = DateTime.Now.ToString();
            if (ATSer.Update(ATEnt) > 0)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Updated Successfully");
                clearFields();
                tblEmpDetail.Visible = false;
                detaildiv.Visible = false;
                EmpSearch.Visible = true;
            }
        }
    }
    private string Convert12To24HourFormat(string hours, string minutes, string ampm)
    {
        if (string.IsNullOrEmpty(hours) || string.IsNullOrEmpty(minutes) || string.IsNullOrEmpty(ampm))
            return "00:00";

        int hour, minute;
        
        if (!int.TryParse(hours, out hour) || !int.TryParse(minutes, out minute))
            return "00:00";
        
        if (hour < 1 || hour > 12 || minute < 0 || minute > 59)
            return "00:00";
        
        if (ampm.Equals("PM", StringComparison.OrdinalIgnoreCase) && hour < 12)
        {
            hour += 12;
        }
        else if (ampm.Equals("AM", StringComparison.OrdinalIgnoreCase) && hour == 12)
        {
            hour = 0; // 12 AM becomes 00
        }

        return string.Format("{0:00}:{1:00}", hour, minute);
    }
    private string Convert24To12HourFormat(string time24)
    {
        if (string.IsNullOrEmpty(time24))
            return "00:00 AM";

        DateTime time;
        if (DateTime.TryParseExact(time24, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
        {
            if (time.Hour == 0 && time.Minute == 0)
                return "12:00 AM";
            
            return time.ToString("hh:mm tt", CultureInfo.InvariantCulture);
        }
        
        return "00:00 AM";
    }
    protected void clearFields()
    {
        lblEmpId.Text = "";
        lblEmpName.Text = "";
        lblDesignation.Text = "";
        lblJobTitle.Text = "";
        txtDate.Text = "";
        txtInTimeHr.Text = "00";
        txtOutTimeHr.Text = "00";
        txtInTimeMin.Text = "00";
        txtOutTimeMin.Text = "00";
        ddlInAMPM.SelectedValue = "AM";
        ddlOutAMPM.SelectedValue = "AM";
        txtWorkingHour.Text = "00:00:00";
        txtRemarks.Text = "";
        rbtnStatus.SelectedValue = "1";
    }

    protected void btnSearchM_Click(object sender, EventArgs e)
    {
        if (txtFDate.Text != "" && txtTDate.Text != "")
        {
            LoadGrid();
        }
    }

    protected void LoadGrid()
    {
        string[] fdate = txtFDate.Text.Split('/');
        string[] tdate = txtTDate.Text.Split('/');
        string fromdate = PGD.ConvertNepaliTOEnglish(fdate[0], fdate[1], fdate[2]);
        string toDate = PGD.ConvertNepaliTOEnglish(tdate[0], tdate[1], tdate[2]);
        gridAttendance.DataSource = hf.GETATTENDANCE(fromdate, toDate, lblEmpId.Text);
        gridAttendance.DataBind();

    }
    protected void gridAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDate = e.Row.FindControl("lblDate") as Label;
            Label lblNPDate = e.Row.FindControl("lblNPDate") as Label;
            Label lblDay = e.Row.FindControl("lblDay") as Label;
            Label lblDayOfWeek = e.Row.FindControl("lblDayOfWeek") as Label;

            Label lblInTime = e.Row.FindControl("lblInTime") as Label;
            TextBox txtInHr_M = e.Row.FindControl("txtInHr_M") as TextBox;
            TextBox txtInMin_M = e.Row.FindControl("txtInMin_M") as TextBox;
            DropDownList ddlInAMPM_M = e.Row.FindControl("ddlInAMPM_M") as DropDownList;

            Label lblOutTime = e.Row.FindControl("lblOutTime") as Label;
            TextBox txtOutHr_M = e.Row.FindControl("txtOutHr_M") as TextBox;
            TextBox txtOutMin_M = e.Row.FindControl("txtOutMin_M") as TextBox;
            DropDownList ddlOutAMPM_M = e.Row.FindControl("ddlOutAMPM_M") as DropDownList;

            TextBox txtWorkingHour = e.Row.FindControl("txtWorkingHour") as TextBox;

            string[] dateandtime = lblDate.Text.Split(' ');
            string[] date = dateandtime[0].Split('/');

            int month = Convert.ToInt32(date[0]);
            int day = Convert.ToInt32(date[1]);
            int year = Convert.ToInt32(date[2]);


            lblDay.Text = day.ToString();

            lblDayOfWeek.Text = new DateTime(year, month, day).DayOfWeek.ToString();


            if (lblDayOfWeek.Text == "Saturday")
            {
                e.Row.Attributes["style"] = "background-color: #cc0000; color:#ffffff";

            }

            if (txtWorkingHour.Text == "12:00:00")
            {
                e.Row.Attributes["style"] = "color:#0000cc";
            }


            string[] npdate = PGD.GetEnglishDateFromNepali(lblDate.Text,"dd/mm/yyyy").Split('/');

            lblNPDate.Text = npdate[0] + "/" + npdate[1] + "/" + npdate[2];

            #region In Time
            if (lblInTime.Text != "")
            {
                string intimeStr = lblInTime.Text;
                string[] intimeParts = intimeStr.Split(' ');

                string intimeComponent = intimeParts[0];
                string inperiod = string.Empty;

                if (intimeParts.Length > 1)
                {
                    inperiod = intimeParts[1]; // AM or PM
                }
                else
                {
                    inperiod = "";
                }

                // Split hours, minutes, seconds
                string[] intimeSplit = intimeComponent.Split(':');
                string inhr = intimeSplit[0];
                string inmin = intimeSplit[1];
                string insec = intimeSplit[2];
                txtInHr_M.Text = inhr;
                txtInMin_M.Text = inmin;
                ddlInAMPM.SelectedValue = inperiod;
            }
            #endregion

            #region Out Time
            if (lblOutTime.Text != "")
            {
                string outtimeStr = lblOutTime.Text;
                string[] outtimeParts = outtimeStr.Split(' ');
                string outtimeComponent = outtimeParts[0];
                string outperiod = string.Empty;
                if (outtimeParts.Length > 1)
                {
                    outperiod = outtimeParts[1]; // AM or PM
                }
                else
                {
                    outperiod = "";
                }

                // Split hours, minutes, seconds
                string[] outtimeSplit = outtimeComponent.Split(':');
                string outhr = outtimeSplit[0];
                string outmin = outtimeSplit[1];
                string outsec = outtimeSplit[2];

                txtOutHr_M.Text = outhr;
                txtOutMin_M.Text = outmin;
                ddlOutAMPM.SelectedValue = outperiod;
            }
            #endregion



        }
    }
    protected void btnSavee_Click(object sender, EventArgs e)
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

        if (gridAttendance.Rows.Count != 0)
        {
            foreach (GridViewRow gr in gridAttendance.Rows)
            {
                Label lblPkId = gr.FindControl("lblPkId") as Label;

                TextBox txtInHr_M = gr.FindControl("txtInHr_M") as TextBox;
                TextBox txtInMin_M = gr.FindControl("txtInMin_M") as TextBox;
                DropDownList ddlInAMPM_M = gr.FindControl("ddlInAMPM_M") as DropDownList;

                TextBox txtOutHr_M = gr.FindControl("txtOutHr_M") as TextBox;
                TextBox txtOutMin_M = gr.FindControl("txtOutMin_M") as TextBox;
                DropDownList ddlOutAMP_M = gr.FindControl("ddlOutAMP_M") as DropDownList;

                TextBox txtWorkingHour = gr.FindControl("txtWorkingHour") as TextBox;
                TextBox txtRemarks = gr.FindControl("txtRemarks") as TextBox;

                ATEnt = new HR_ATTENDANCE();
                ATEnt.PK_ID = lblPkId.Text;
                ATEnt = (HR_ATTENDANCE)ATSer.GetSingle(ATEnt);
                if (ATEnt != null)
                {
                    ATEnt.STATUS = "4";
                    ATEnt.IN_TIME = txtInHr_M.Text + ":" + txtInMin_M.Text + ":00 " + ddlInAMPM.SelectedValue;
                    ATEnt.OUT_TIME = txtOutHr_M.Text + ":" + txtOutMin_M.Text + ":00 " + ddlOutAMP_M.SelectedValue;
                    ATEnt.WORKING_HOUR = txtWorkingHour.Text;
                    ATEnt.REMARKS = txtRemarks.Text;
                    ATEnt.LOGINID = userProfileEnt.UserName;
                    ATEnt.DATE_TIME = DateTime.Now.ToString();
                    ATSer.Update(ATEnt);

                }
            }
            HelperFunction.MsgBox(this, this.GetType(), "Attendance Updated");
        }
        else if (gridAttendance.Rows.Count == 0)
        {
            string[] fDate = txtFDate.Text.Split('/');
            string[] tDate = txtTDate.Text.Split('/');

            string fromDate = PGD.ConvertNepaliTOEnglish(fDate[0], fDate[1], fDate[2]);
            string toDate = PGD.ConvertNepaliTOEnglish(tDate[0], tDate[1], tDate[2]);

            DateTime fromDt = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDt = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (fromDt < toDt)
            {
                DateTime todaydate = DateTime.Today;
                if (todaydate < toDt)
                {
                    int days = (toDt - fromDt).Days;
                    for (int i = 0; i <= days; i++)
                    {
                        DateTime date = fromDt.AddDays(i);
                        string dateofMonth = date.ToString("MM/dd/yyyy");

                        #region to insert in attendance table

                        ATEnt = new HR_ATTENDANCE();
                        ATEnt.EMPLOYEE_ID = lblEmpId.Text;
                        ATEnt.ATT_DATE = dateofMonth;
                        ATEnt = (HR_ATTENDANCE)ATSer.GetSingle(ATEnt);
                        if (ATEnt == null)
                        {
                            string officecode = "";

                            EMPEnt = new HR_EMPLOYEES();
                            EMPEnt.EMPLOYEEID = lblEmpId.Text;
                            EMPEnt = (HR_EMPLOYEES)EMPSer.GetSingle(EMPEnt);
                            if (EMPEnt != null)
                            {
                                //officelocation = EMPEnt.OFFICELOCATION;
                                officecode = EMPEnt.OFFICECODE;
                            }

                            EMPEnt = new HR_EMPLOYEES();
                            EMPEnt.EMPLOYEEID = lblEmpId.Text;
                            EMPEnt = (HR_EMPLOYEES)EMPSer.GetSingle(EMPEnt);
                            if (EMPEnt != null)
                            {
                                ATEnt = new HR_ATTENDANCE();
                                ATEnt.EMPLOYEE_ID = lblEmpId.Text;
                                ATEnt.ATT_DATE = dateofMonth;
                                ATEnt.IN_TIME = "00:00:00 AM";
                                ATEnt.OUT_TIME = "00:00:00 AM";
                                ATEnt.WORKING_HOUR = "00:00:00";
                                ATEnt.STATUS = "1";
                                ATEnt.REMARKS = "Absent";
                                ATSer.Insert(ATEnt);
                            }
                        }
                    }
                    #endregion
                }
            }
        }

        LoadGrid();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearFields();
        tblEmpDetail.Visible = false;
        detaildiv.Visible = false;
        EmpSearch.Visible = true;
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
    protected void rbtnStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnStatus.SelectedValue == "2")
        {
            txtRemarks.Text = "Absent";
            txtInTimeHr.Text = "00";
            txtInTimeMin.Text = "00";
            txtOutTimeHr.Text = "00";
            txtOutTimeMin.Text = "00";
            ddlInAMPM.SelectedValue = "AM";
            ddlOutAMPM.SelectedValue = "AM";
            txtWorkingHour.Text = "00:00:00";
            txtDate.Focus();

            txtRemarks.Enabled = false;
            txtInTimeHr.Enabled = false;
            txtInTimeMin.Enabled = false;
            txtOutTimeHr.Enabled = false;
            txtOutTimeMin.Enabled = false;
            txtWorkingHour.Enabled = false;
            ddlInAMPM.Enabled = false;
            ddlOutAMPM.Enabled = false;
        }
        else
        {
            txtRemarks.Text = "";

            txtRemarks.Enabled = true;
            txtInTimeHr.Enabled = true;
            txtInTimeMin.Enabled = true;
            txtOutTimeHr.Enabled = true;
            txtOutTimeMin.Enabled = true;
            txtWorkingHour.Enabled = true;
            ddlInAMPM.Enabled = true;
            ddlOutAMPM.Enabled = true;

            txtDate.Focus();
        }
    }

    protected void txtInTimeHr_TextChanged(object sender, EventArgs e)
    {
        ValidateHourTextBox(sender as TextBox, "In Hour");
        txtWorkingHour.Text = getWorkingTime(txtInTimeHr.Text, txtInTimeMin.Text, ddlInAMPM.SelectedValue, txtOutTimeHr.Text, txtOutTimeMin.Text, ddlOutAMPM.SelectedValue);
    }

    protected void txtInTimeMin_TextChanged(object sender, EventArgs e)
    {
        ValidateMinuteTextBox(sender as TextBox, "In Minute");
        txtWorkingHour.Text = getWorkingTime(txtInTimeHr.Text, txtInTimeMin.Text, ddlInAMPM.SelectedValue, txtOutTimeHr.Text, txtOutTimeMin.Text, ddlOutAMPM.SelectedValue);
    }

    protected void ddlInAMPM_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtWorkingHour.Text = getWorkingTime(txtInTimeHr.Text, txtInTimeMin.Text, ddlInAMPM.SelectedValue, txtOutTimeHr.Text, txtOutTimeMin.Text, ddlOutAMPM.SelectedValue);
    }

    protected void txtOutTimeHr_TextChanged(object sender, EventArgs e)
    {
        ValidateHourTextBox(sender as TextBox, "Out Hour");
        txtWorkingHour.Text = getWorkingTime(txtInTimeHr.Text, txtInTimeMin.Text, ddlInAMPM.SelectedValue, txtOutTimeHr.Text, txtOutTimeMin.Text, ddlOutAMPM.SelectedValue);
    }

    protected void txtOutTimeMin_TextChanged(object sender, EventArgs e)
    {
        ValidateMinuteTextBox(sender as TextBox, "Out Minute");
        txtWorkingHour.Text = getWorkingTime(txtInTimeHr.Text, txtInTimeMin.Text, ddlInAMPM.SelectedValue, txtOutTimeHr.Text, txtOutTimeMin.Text, ddlOutAMPM.SelectedValue);
    }

    protected void ddlOutAMPM_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtWorkingHour.Text = getWorkingTime(txtInTimeHr.Text, txtInTimeMin.Text, ddlInAMPM.SelectedValue, txtOutTimeHr.Text, txtOutTimeMin.Text, ddlOutAMPM.SelectedValue);
    }

    private void ValidateHourTextBox(TextBox txt, string label)
    {
        int val;
        if (txt != null)
        {
            if (!int.TryParse(txt.Text, out val) || val < 1 || val > 12)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertHour", "alert('" + label + " must be between 1 and 12.');", true);
                txt.Text = "00";
            }
            else
            {
                txt.Text = val.ToString("D2");
            }
        }
    }

    private void ValidateMinuteTextBox(TextBox txt, string label)
    {
        int val;
        if (txt != null)
        {
            if (!int.TryParse(txt.Text, out val) || val < 0 || val > 59)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alertMinute", "alert('" + label + " must be between 0 and 59.');", true);
                txt.Text = "00";
            }
            else
            {
                txt.Text = val.ToString("D2");
            }
        }
    }

    protected void checkWorkingHrGrid(string label)
    {

    }

    protected void txtInHr_M_TextChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridAttendance.Rows)
        {
            TextBox txtInHr_M = gr.FindControl("txtInHr_M") as TextBox;
            TextBox txtInMin_M = gr.FindControl("txtInMin_M") as TextBox;
            DropDownList ddlInAMPM_M = gr.FindControl("ddlInAMPM_M") as DropDownList;
            TextBox txtOutHr_M = gr.FindControl("txtOutHr_M") as TextBox;
            TextBox txtOutMin_M = gr.FindControl("txtOutMin_M") as TextBox;
            DropDownList ddlOutAMP_M = gr.FindControl("ddlOutAMP_M") as DropDownList;

            TextBox txtWorkingHour = gr.FindControl("txtWorkingHour") as TextBox;

            ValidateHourTextBox(sender as TextBox, "In Hour");
            txtWorkingHour.Text = getWorkingTime(txtInHr_M.Text, txtInMin_M.Text, ddlInAMPM_M.SelectedValue, txtOutHr_M.Text, txtOutMin_M.Text, ddlOutAMP_M.SelectedValue);

        }
    }

    protected void txtInMin_M_TextChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridAttendance.Rows)
        {
            TextBox txtInHr_M = gr.FindControl("txtInHr_M") as TextBox;
            TextBox txtInMin_M = gr.FindControl("txtInMin_M") as TextBox;
            DropDownList ddlInAMPM_M = gr.FindControl("ddlInAMPM_M") as DropDownList;
            TextBox txtOutHr_M = gr.FindControl("txtOutHr_M") as TextBox;
            TextBox txtOutMin_M = gr.FindControl("txtOutMin_M") as TextBox;
            DropDownList ddlOutAMP_M = gr.FindControl("ddlOutAMP_M") as DropDownList;

            TextBox txtWorkingHour = gr.FindControl("txtWorkingHour") as TextBox;

            ValidateHourTextBox(sender as TextBox, "In Minute");
            txtWorkingHour.Text = getWorkingTime(txtInHr_M.Text, txtInMin_M.Text, ddlInAMPM_M.SelectedValue, txtOutHr_M.Text, txtOutMin_M.Text, ddlOutAMP_M.SelectedValue);

        }
    }

    protected void ddlInAMPM_M_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridAttendance.Rows)
        {
            TextBox txtInHr_M = gr.FindControl("txtInHr_M") as TextBox;
            TextBox txtInMin_M = gr.FindControl("txtInMin_M") as TextBox;
            DropDownList ddlInAMPM_M = gr.FindControl("ddlInAMPM_M") as DropDownList;
            TextBox txtOutHr_M = gr.FindControl("txtOutHr_M") as TextBox;
            TextBox txtOutMin_M = gr.FindControl("txtOutMin_M") as TextBox;
            DropDownList ddlOutAMP_M = gr.FindControl("ddlOutAMP_M") as DropDownList;

            TextBox txtWorkingHour = gr.FindControl("txtWorkingHour") as TextBox;

            txtWorkingHour.Text = getWorkingTime(txtInHr_M.Text, txtInMin_M.Text, ddlInAMPM_M.SelectedValue, txtOutHr_M.Text, txtOutMin_M.Text, ddlOutAMP_M.SelectedValue);

        }
    }

    protected void txtOutHr_M_TextChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridAttendance.Rows)
        {
            TextBox txtInHr_M = gr.FindControl("txtInHr_M") as TextBox;
            TextBox txtInMin_M = gr.FindControl("txtInMin_M") as TextBox;
            DropDownList ddlInAMPM_M = gr.FindControl("ddlInAMPM_M") as DropDownList;
            TextBox txtOutHr_M = gr.FindControl("txtOutHr_M") as TextBox;
            TextBox txtOutMin_M = gr.FindControl("txtOutMin_M") as TextBox;
            DropDownList ddlOutAMP_M = gr.FindControl("ddlOutAMP_M") as DropDownList;

            TextBox txtWorkingHour = gr.FindControl("txtWorkingHour") as TextBox;

            ValidateHourTextBox(sender as TextBox, "Out Hour");
            txtWorkingHour.Text = getWorkingTime(txtInHr_M.Text, txtInMin_M.Text, ddlInAMPM_M.SelectedValue, txtOutHr_M.Text, txtOutMin_M.Text, ddlOutAMP_M.SelectedValue);

        }
    }

    protected void txtOutMin_M_TextChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridAttendance.Rows)
        {
            TextBox txtInHr_M = gr.FindControl("txtInHr_M") as TextBox;
            TextBox txtInMin_M = gr.FindControl("txtInMin_M") as TextBox;
            DropDownList ddlInAMPM_M = gr.FindControl("ddlInAMPM_M") as DropDownList;
            TextBox txtOutHr_M = gr.FindControl("txtOutHr_M") as TextBox;
            TextBox txtOutMin_M = gr.FindControl("txtOutMin_M") as TextBox;
            DropDownList ddlOutAMP_M = gr.FindControl("ddlOutAMP_M") as DropDownList;

            TextBox txtWorkingHour = gr.FindControl("txtWorkingHour") as TextBox;

            ValidateHourTextBox(sender as TextBox, "Out Minute");
            txtWorkingHour.Text = getWorkingTime(txtInHr_M.Text, txtInMin_M.Text, ddlInAMPM_M.SelectedValue, txtOutHr_M.Text, txtOutMin_M.Text, ddlOutAMP_M.SelectedValue);

        }
    }

    protected void ddlOutAMP_M_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridAttendance.Rows)
        {
            TextBox txtInHr_M = gr.FindControl("txtInHr_M") as TextBox;
            TextBox txtInMin_M = gr.FindControl("txtInMin_M") as TextBox;
            DropDownList ddlInAMPM_M = gr.FindControl("ddlInAMPM_M") as DropDownList;
            TextBox txtOutHr_M = gr.FindControl("txtOutHr_M") as TextBox;
            TextBox txtOutMin_M = gr.FindControl("txtOutMin_M") as TextBox;
            DropDownList ddlOutAMP_M = gr.FindControl("ddlOutAMP_M") as DropDownList;

            TextBox txtWorkingHour = gr.FindControl("txtWorkingHour") as TextBox;

            txtWorkingHour.Text = getWorkingTime(txtInHr_M.Text, txtInMin_M.Text, ddlInAMPM_M.SelectedValue, txtOutHr_M.Text, txtOutMin_M.Text, ddlOutAMP_M.SelectedValue);

        }
    }
}