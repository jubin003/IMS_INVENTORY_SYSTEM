using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;
using Entity.Framework;
using PhyeGanCore;
using System.Globalization;

public partial class HR_Attendance_Duty_Hour_set_duty_hour_roster : System.Web.UI.Page
{
    HR_EMPLOYEES EEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService ESer = new HR_EMPLOYEESService();

    HR_DIVISION DEnt = new HR_DIVISION();
    HR_DIVISIONService DSer = new HR_DIVISIONService();

    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    SET_HR_DUTY_HOUR SHEnt = new SET_HR_DUTY_HOUR();
    SET_HR_DUTY_HOURService SHSer = new SET_HR_DUTY_HOURService();

    SET_HR_DUTY_ROSTER SHREnt = new SET_HR_DUTY_ROSTER();
    SET_HR_DUTY_ROSTERService SHRSer = new SET_HR_DUTY_ROSTERService();

    WORKING_HOUR WHEnt = new WORKING_HOUR();
    WORKING_HOURService WHSer = new WORKING_HOURService();

    ADBS ABEnt = new ADBS();
    ADBSService ABSer = new ADBSService();
    PhyeGanDate PGD = new PhyeGanDate();

    HelperFunction hf = new HelperFunction();

    string status = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDepartment();
            LoadDivision();          
        }
    }
    protected void LoadDepartment()
    {
        DPEnt = new HR_DEPARTMENT();       
        ddlDept.DataSource = DPSer.GetAll(DPEnt);
        ddlDept.DataTextField = "DEP_NAME";
        ddlDept.DataValueField = "PK_ID";
        ddlDept.DataBind();
        ddlDept.Items.Insert(0, "Select");

    }
    protected void LoadDivision()
    {
        ddlDiv.Items.Clear();
        DEnt = new HR_DIVISION();
        if (ddlDept.SelectedValue == "Select")
        {
            DEnt.DEP_ID = "";
        }
        else;
        DEnt.DEP_ID = ddlDept.SelectedValue;
        ddlDiv.DataSource = DSer.GetAll(DEnt);
        ddlDiv.DataTextField = "DIVISION_NAME";
        ddlDiv.DataValueField = "PK_ID";

        ddlDiv.DataBind();
        ddlDiv.Items.Insert(0, "Select");
        if (ddlDiv == null)
        {
            view.Visible = false;
        }
        else
        {
            view.Visible = true;            
        }
    }
    private void LoadEnglishYears(int startYear, int endYear)
    {
        ddlEngYear.Items.Clear();

        for (int year = startYear; year <= endYear; year++)
        {
            ddlEngYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
        }
        ddlEngYear.SelectedValue = DateTime.Now.Year.ToString();
    }

    private void LoadNepaliYears(int startYear, int endYear)
    {
        string nepDate = PGD.GetTodayNepaliDate();
        string[] parts = nepDate.Split('/');

        string nepDay = parts[0];
        string nepMonth = parts[1];
        string nepYear = parts[2];

        ddlNepYear.Items.Clear();

        for (int year = startYear; year <= endYear; year++)
        {
            ddlNepYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
        }
        ddlNepYear.SelectedValue = nepYear;
    }   
   
    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDivision();
    }
    protected void gridDutyHr_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DropDownList ddlDutyTypeH = e.Row.FindControl("ddlDutyTypeH") as DropDownList;
            WHEnt = new WORKING_HOUR();
            WHEnt.STATUS = "1";
            ddlDutyTypeH.DataSource = WHSer.GetAll(WHEnt);
            ddlDutyTypeH.DataTextField = "TYPE";
            ddlDutyTypeH.DataValueField = "PK_ID";
            ddlDutyTypeH.DataBind();
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblEmpId = (Label)e.Row.FindControl("lblEmpId");
            Label lblNepDayG = (Label)e.Row.FindControl("lblNepDayG");
            Label lblEngDayG = (Label)e.Row.FindControl("lblEngDayG");
            Label lblEngDateG = (Label)e.Row.FindControl("lblEngDateG");
            Label lblNepDateG = (Label)e.Row.FindControl("lblNepDateG");
            Label lblDay = (Label)e.Row.FindControl("lblDay");
            Label lblShowDay = (Label)e.Row.FindControl("lblShowDay");

            CheckBox chkWorking = (CheckBox)e.Row.FindControl("chkWorking");

            DropDownList ddlWorkingDay = (DropDownList)e.Row.FindControl("ddlWorkingDay");
            DropDownList ddlDutyType = (DropDownList)e.Row.FindControl("ddlDutyType");

            DropDownList ddlInAMPM = e.Row.FindControl("ddlInAMPM") as DropDownList;
            DropDownList ddlOutAMPM = e.Row.FindControl("ddlOutAMPM") as DropDownList;
            TextBox txtInHr = e.Row.FindControl("txtInHr") as TextBox;
            TextBox txtInMin = e.Row.FindControl("txtInMin") as TextBox;
            TextBox txtOutHr = e.Row.FindControl("txtOutHr") as TextBox;
            TextBox txtOutMin = e.Row.FindControl("txtOutMin") as TextBox;

            WHEnt = new WORKING_HOUR();
            WHEnt.STATUS = "1";
            ddlDutyType.DataSource = WHSer.GetAll(WHEnt);
            ddlDutyType.DataTextField = "TYPE";
            ddlDutyType.DataValueField = "PK_ID";
            ddlDutyType.DataBind();

            if (ddlDateType.SelectedValue == "Eng")
            {
                lblEngDateG.Visible = true;
                lblNepDateG.Visible = false;
            }
            else
            {
                lblEngDateG.Visible = false;
                lblNepDateG.Visible = true;

            }
            lblShowDay.Text = PGD.dayFromDate(lblDay.Text);

            SHREnt = new SET_HR_DUTY_ROSTER();
            SHREnt.EMP_ID = lblEmployeeID.Text;
            SHREnt.DUTY_DATE = lblEngDateG.Text;
            SHREnt = (SET_HR_DUTY_ROSTER)SHRSer.GetSingle(SHREnt);
            if (SHREnt != null)
            {
                if (SHREnt.WORKING_DAY == "1")
                {
                    chkWorking.Checked = true;
                    e.Row.BackColor = System.Drawing.Color.Empty; // or any color you prefer

                    ddlDutyType.Enabled = true;
                    txtInHr.Enabled = true;
                    txtInMin.Enabled = true;
                    txtOutHr.Enabled = true;
                    txtOutMin.Enabled = true;
                    ddlInAMPM.Enabled = true;
                    ddlOutAMPM.Enabled = true;
                }
                else
                {
                    chkWorking.Checked = false;
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffcccc");
                    ddlDutyType.SelectedValue = "1";
                    txtInHr.Text = "00";
                    txtInMin.Text = "00";
                    txtOutHr.Text = "00";
                    txtOutMin.Text = "00";

                    ddlDutyType.Enabled = false;
                    txtInHr.Enabled = false;
                    txtInMin.Enabled = false;
                    txtOutHr.Enabled = false;
                    txtOutMin.Enabled = false;
                    ddlInAMPM.Enabled = false;
                    ddlOutAMPM.Enabled = false;
                }
                WHEnt = new WORKING_HOUR();
                WHEnt.CODE = SHREnt.DUTY_TYPE;
                WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
                if (WHEnt != null)
                {
                    ddlDutyType.SelectedValue = WHEnt.PK_ID;
                }

                string intime = Convert24To12HourFormat(SHREnt.DUTY_IN_TIME);
                string outTime = Convert24To12HourFormat(SHREnt.DUTY_OUT_TIME);
                if (!string.IsNullOrEmpty(intime))
                {
                    string[] inParts = intime.Split(' '); // ["02:30", "PM"]
                    if (inParts.Length == 2)
                    {
                        string[] inTimeParts = inParts[0].Split(':'); // ["02", "30"]
                        if (inTimeParts.Length == 2)
                        {
                            txtInHr.Text = inTimeParts[0];
                            txtInMin.Text = inTimeParts[1];
                            ddlInAMPM.SelectedValue = inParts[1];
                        }
                    }
                }

                if (!string.IsNullOrEmpty(outTime))
                {
                    string[] outParts = outTime.Split(' '); // ["05:15", "PM"]
                    if (outParts.Length == 2)
                    {
                        string[] outTimeParts = outParts[0].Split(':'); // ["05", "15"]
                        if (outTimeParts.Length == 2)
                        {
                            txtOutHr.Text = outTimeParts[0];
                            txtOutMin.Text = outTimeParts[1];
                            ddlOutAMPM.SelectedValue = outParts[1];
                        }
                    }
                }

            }

        }
    }
    protected void bttnSearch_Click(object sender, EventArgs e)
    {
        if (ddlDept.SelectedValue != "Select")
        {
            loadEmpGrid();
            grdEmployee.Visible = true;
            tblEmpInfo.Visible = false;
            gridDutyHr.Visible = false;
            tblSaveBtn.Visible = false;
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please Select Department");
        }
    }

    protected void loadEmpGrid()
    {
        string divId = "";
        string depId = "";
        if (ddlDiv.SelectedValue != "Select")
        {
            divId = ddlDiv.SelectedValue;
        }
        if (ddlDept.SelectedValue != "Select")
        {
            depId = ddlDept.SelectedValue;
        }

        grdEmployee.DataSource = hf.getEmp_No_Fixed_Time(divId, depId);
        grdEmployee.DataBind();
    }
    protected void LoadGrid()
    {
        string npYearOnly = ddlNepYear.SelectedValue;
        string engYearOnly = ddlEngYear.SelectedValue;
        ABEnt = new ADBS();

        if (ddlDateType.SelectedValue == "Eng")
        {
            ABEnt.AD_YEAR = engYearOnly;
            ABEnt.AD_MONTH = ddlEngMonth.SelectedValue;
        }
        else
        {
            ABEnt.BS_YEAR = npYearOnly;
            ABEnt.BS_MONTH = ddlNepMonth.SelectedValue;
        }
        gridDutyHr.DataSource = ABSer.GetAll(ABEnt);
        gridDutyHr.DataBind();


    }
    protected void gridDutyHr_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SaveRow")
        {
            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblNepDayG = gr.FindControl("lblNepDayG") as Label;
            Label lblEngDayG = (Label)gr.FindControl("lblEngDayG");
            Label lblEngDateG = gr.FindControl("lblEngDateG") as Label;
            CheckBox chkWorking = gr.FindControl("chkWorking") as CheckBox;

            DropDownList ddlDutyType = gr.FindControl("ddlDutyType") as DropDownList;
            DropDownList ddlInAMPM = gr.FindControl("ddlInAMPM") as DropDownList;
            DropDownList ddlOutAMPM = gr.FindControl("ddlOutAMPM") as DropDownList;
            TextBox txtInHr = gr.FindControl("txtInHr") as TextBox;
            TextBox txtInMin = gr.FindControl("txtInMin") as TextBox;
            TextBox txtOutHr = gr.FindControl("txtOutHr") as TextBox;

            TextBox txtOutMin = gr.FindControl("txtOutMin") as TextBox;
            bool chkStatus = false;
            if (chkWorking.Checked == true)
            {
                if (ddlDutyType.SelectedValue != "1")
                {
                    chkStatus = true;
                }
                else
                {
                    chkStatus = false;
                }
            }
            else if (chkWorking.Checked == false)
            {
                chkStatus = true;
            }

            if (chkStatus == true)
            {
                SHREnt = new SET_HR_DUTY_ROSTER();
                SHREnt.DUTY_DATE = lblEngDateG.Text;
                SHREnt.EMP_ID = lblEmployeeID.Text;
                SHREnt = (SET_HR_DUTY_ROSTER)SHRSer.GetSingle(SHREnt);
                if (SHREnt != null)
                {
                    SHREnt.WORKING_DAY = chkWorking.Checked ? "1" : null;

                    string inHours = txtInHr.Text.Trim();
                    string inMins = txtInMin.Text.Trim();
                    string inAmPm = ddlInAMPM.SelectedValue;
                    SHREnt.DUTY_IN_TIME = Convert12To24HourFormat(inHours, inMins, inAmPm);

                    string outHours = txtOutHr.Text.Trim();
                    string outMins = txtOutMin.Text.Trim();
                    string outAmPm = ddlOutAMPM.SelectedValue;
                    SHREnt.DUTY_OUT_TIME = Convert12To24HourFormat(outHours, outMins, outAmPm);

                    WHEnt = new WORKING_HOUR();
                    WHEnt.PK_ID = ddlDutyType.SelectedValue;
                    WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
                    if (WHEnt != null)
                    {
                        SHREnt.DUTY_TYPE = WHEnt.CODE;
                        SHREnt.DUTY_HOUR = WHEnt.DUTY_HOUR;
                    }

                    SHRSer.Update(SHREnt);
                }
                else
                {
                    SHREnt = new SET_HR_DUTY_ROSTER();
                    SHREnt.EMP_ID = lblEmployeeID.Text;
                    SHREnt.DUTY_DATE = lblEngDateG.Text;
                    SHREnt.WORKING_DAY = chkWorking.Checked ? "1" : null;

                    string inHours = txtInHr.Text.Trim();
                    string inMins = txtInMin.Text.Trim();
                    string inAmPm = ddlInAMPM.SelectedValue;
                    SHREnt.DUTY_IN_TIME = Convert12To24HourFormat(inHours, inMins, inAmPm);

                    string outHours = txtOutHr.Text.Trim();
                    string outMins = txtOutMin.Text.Trim();
                    string outAmPm = ddlOutAMPM.SelectedValue;
                    SHREnt.DUTY_OUT_TIME = Convert12To24HourFormat(outHours, outMins, outAmPm);


                    WHEnt = new WORKING_HOUR();
                    WHEnt.PK_ID = ddlDutyType.SelectedValue;
                    WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
                    if (WHEnt != null)
                    {
                        SHREnt.DUTY_TYPE = WHEnt.CODE;
                        SHREnt.DUTY_HOUR = WHEnt.DUTY_HOUR;
                    }

                    SHRSer.Insert(SHREnt);
                }
                status = "Data Saved";
            }
            else
            {
                status = "Shift Cannot be Empty";
            }
        }
        HelperFunction.MsgBox(this, this.GetType(), status);
    }
    protected void btnSaveGrid_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            Label lblNepDayG = gr.FindControl("lblNepDayG") as Label;
            Label lblEngDayG = (Label)gr.FindControl("lblEngDayG");
            Label lblEngDateG = gr.FindControl("lblEngDateG") as Label;
            CheckBox chkWorking = gr.FindControl("chkWorking") as CheckBox;

            DropDownList ddlDutyType = gr.FindControl("ddlDutyType") as DropDownList;
            DropDownList ddlInAMPM = gr.FindControl("ddlInAMPM") as DropDownList;
            DropDownList ddlOutAMPM = gr.FindControl("ddlOutAMPM") as DropDownList;
            TextBox txtInHr = gr.FindControl("txtInHr") as TextBox;
            TextBox txtInMin = gr.FindControl("txtInMin") as TextBox;
            TextBox txtOutHr = gr.FindControl("txtOutHr") as TextBox;

            TextBox txtOutMin = gr.FindControl("txtOutMin") as TextBox;
            bool chkStatus = false;
            if (chkWorking.Checked == true)
            {
                if (ddlDutyType.SelectedValue != "1")
                {
                    chkStatus = true;
                }
                else
                {
                    chkStatus = false;
                }
            }
            else
            {
                chkStatus = true;
            }

            if (chkStatus == true)
            {
                SHREnt = new SET_HR_DUTY_ROSTER();
                SHREnt.DUTY_DATE = lblEngDateG.Text;
                SHREnt.EMP_ID = lblEmployeeID.Text;
                SHREnt = (SET_HR_DUTY_ROSTER)SHRSer.GetSingle(SHREnt);
                if (SHREnt != null)
                {
                    SHREnt.WORKING_DAY = chkWorking.Checked ? "1" : null;

                    string inHours = txtInHr.Text.Trim();
                    string inMins = txtInMin.Text.Trim();
                    string inAmPm = ddlInAMPM.SelectedValue;
                    SHREnt.DUTY_IN_TIME = Convert12To24HourFormat(inHours, inMins, inAmPm);

                    string outHours = txtOutHr.Text.Trim();
                    string outMins = txtOutMin.Text.Trim();
                    string outAmPm = ddlOutAMPM.SelectedValue;
                    SHREnt.DUTY_OUT_TIME = Convert12To24HourFormat(outHours, outMins, outAmPm);

                    WHEnt = new WORKING_HOUR();
                    WHEnt.PK_ID = ddlDutyType.SelectedValue;
                    WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
                    if (WHEnt != null)
                    {
                        SHREnt.DUTY_TYPE = WHEnt.CODE;
                        SHREnt.DUTY_HOUR = WHEnt.DUTY_HOUR;
                    }

                    SHRSer.Update(SHREnt);
                }
                else
                {
                    SHREnt = new SET_HR_DUTY_ROSTER();
                    SHREnt.EMP_ID = lblEmployeeID.Text;
                    SHREnt.DUTY_DATE = lblEngDateG.Text;
                    SHREnt.WORKING_DAY = chkWorking.Checked ? "1" : null;

                    string inHours = txtInHr.Text.Trim();
                    string inMins = txtInMin.Text.Trim();
                    string inAmPm = ddlInAMPM.SelectedValue;
                    SHREnt.DUTY_IN_TIME = Convert12To24HourFormat(inHours, inMins, inAmPm);

                    string outHours = txtOutHr.Text.Trim();
                    string outMins = txtOutMin.Text.Trim();
                    string outAmPm = ddlOutAMPM.SelectedValue;
                    SHREnt.DUTY_OUT_TIME = Convert12To24HourFormat(outHours, outMins, outAmPm);


                    WHEnt = new WORKING_HOUR();
                    WHEnt.PK_ID = ddlDutyType.SelectedValue;
                    WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
                    if (WHEnt != null)
                    {
                        SHREnt.DUTY_TYPE = WHEnt.CODE;
                        SHREnt.DUTY_HOUR = WHEnt.DUTY_HOUR;
                    }

                    SHRSer.Insert(SHREnt);
                }
                status = "Data Saved";
            }
            else
            {
               // status = "Shift Cannot be Empty";
                continue;
            }
        }
        HelperFunction.MsgBox(this, this.GetType(), status);
    }
    private string Convert24To12HourFormat(string time24)
    {
        // Default to midnight if invalid
        if (string.IsNullOrEmpty(time24))
            return "00:00 AM";

        DateTime time;
        if (DateTime.TryParseExact(time24, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
        {
            // Special handling for midnight (00:00)
            if (time.Hour == 0 && time.Minute == 0)
                return "12:00 AM";

            // Standard conversion for other times
            return time.ToString("hh:mm tt", CultureInfo.InvariantCulture);
        }

        // Fallback for invalid formats
        return "00:00 AM";
    }
    private string Convert12To24HourFormat(string hours, string minutes, string ampm)
    {
        // Default to midnight if invalid
        if (string.IsNullOrEmpty(hours) || string.IsNullOrEmpty(minutes) || string.IsNullOrEmpty(ampm))
            return "00:00";

        int hour, minute;

        // Parse with TryParse for safety
        if (!int.TryParse(hours, out hour) || !int.TryParse(minutes, out minute))
            return "00:00";

        // Validate ranges
        if (hour < 1 || hour > 12 || minute < 0 || minute > 59)
            return "00:00";

        // Convert to 24-hour format
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
    protected void ddlDutyTypeH_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlDutyTypeH = gridDutyHr.HeaderRow.FindControl("ddlDutyTypeH") as DropDownList;
        DropDownList ddlInAMPMH = gridDutyHr.HeaderRow.FindControl("ddlInAMPMH") as DropDownList;
        DropDownList ddlOutAMPMH = gridDutyHr.HeaderRow.FindControl("ddlOutAMPMH") as DropDownList;
        TextBox txtInHrH = gridDutyHr.HeaderRow.FindControl("txtInHrH") as TextBox;
        TextBox txtInMinH = gridDutyHr.HeaderRow.FindControl("txtInMinH") as TextBox;
        TextBox txtOutHrH = gridDutyHr.HeaderRow.FindControl("txtOutHrH") as TextBox;
        TextBox txtOutMinH = gridDutyHr.HeaderRow.FindControl("txtOutMinH") as TextBox;


        WHEnt = new WORKING_HOUR();
        if (ddlDutyTypeH.SelectedValue != "-")
        {
            WHEnt.PK_ID = ddlDutyTypeH.SelectedValue;
            WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
            if (WHEnt != null)
            {
                DateTime time;
                if (WHEnt.IN_TIME == "00:00")
                {
                    txtInHrH.Text = "00";
                    txtInMinH.Text = "00";
                    ddlInAMPMH.SelectedValue = "AM";
                }
                else
                {
                    string inTime24 = WHEnt.IN_TIME;


                    if (DateTime.TryParseExact(inTime24, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                    {
                        int inhour24 = time.Hour;
                        int inhour12 = inhour24 % 12;
                        if (inhour12 == 0) inhour12 = 12;

                        string inminute = time.Minute.ToString("00");
                        string inampm = inhour24 < 12 ? "AM" : "PM";

                        // Populate the controls
                        txtInHrH.Text = inhour12.ToString();
                        txtInMinH.Text = inminute;
                        ddlInAMPMH.SelectedValue = inampm;

                    }
                }
                if (WHEnt.OUT_TIME == "00:00")
                {
                    txtOutHrH.Text = "00";
                    txtOutMinH.Text = "00";
                    ddlOutAMPMH.SelectedValue = "AM";
                }
                else
                {
                    string outTime24 = WHEnt.OUT_TIME;
                    if (DateTime.TryParseExact(outTime24, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                    {
                        int outhour24 = time.Hour;
                        int outhour12 = outhour24 % 12;
                        if (outhour12 == 0) outhour12 = 12;

                        string outminute = time.Minute.ToString("00");
                        string outampm = outhour24 < 12 ? "AM" : "PM";

                        // Populate the controls
                        txtOutHrH.Text = outhour12.ToString();
                        txtOutMinH.Text = outminute;
                        ddlOutAMPMH.SelectedValue = outampm;
                    }
                }
                foreach (GridViewRow gr in gridDutyHr.Rows)
                {
                    DropDownList ddlDutyType = gr.FindControl("ddlDutyType") as DropDownList;
                    DropDownList ddlInAMPM = gr.FindControl("ddlInAMPM") as DropDownList;
                    DropDownList ddlOutAMPM = gr.FindControl("ddlOutAMPM") as DropDownList;
                    TextBox txtInHr = gr.FindControl("txtInHr") as TextBox;
                    TextBox txtInMin = gr.FindControl("txtInMin") as TextBox;
                    TextBox txtOutHr = gr.FindControl("txtOutHr") as TextBox;
                    TextBox txtOutMin = gr.FindControl("txtOutMin") as TextBox;

                    ddlDutyType.SelectedValue = ddlDutyTypeH.SelectedValue;
                    txtInHr.Text = txtInHrH.Text;
                    txtInMin.Text = txtInMinH.Text;
                    txtOutHr.Text = txtOutHrH.Text;
                    txtOutMin.Text = txtOutMinH.Text;
                    ddlInAMPM.SelectedValue = ddlInAMPMH.SelectedValue;
                    ddlOutAMPM.SelectedValue = ddlOutAMPMH.SelectedValue;
                }
            }
        }
    }

    protected void ddlDutyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            DropDownList ddlDutyType = gr.FindControl("ddlDutyType") as DropDownList;
            DropDownList ddlInAMPM = gr.FindControl("ddlInAMPM") as DropDownList;
            DropDownList ddlOutAMPM = gr.FindControl("ddlOutAMPM") as DropDownList;
            TextBox txtInHr = gr.FindControl("txtInHr") as TextBox;
            TextBox txtInMin = gr.FindControl("txtInMin") as TextBox;
            TextBox txtOutHr = gr.FindControl("txtOutHr") as TextBox;
            TextBox txtOutMin = gr.FindControl("txtOutMin") as TextBox;
            WHEnt = new WORKING_HOUR();
            if (ddlDutyType.SelectedValue != "Select")
            {
                WHEnt.PK_ID = ddlDutyType.SelectedValue;
                WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
                if (WHEnt != null)
                {
                    DateTime time;
                    if (WHEnt.IN_TIME == "00:00")
                    {
                        txtInHr.Text = "00";
                        txtInMin.Text = "00";
                        ddlInAMPM.SelectedValue = "AM";
                    }
                    else
                    {
                        string inTime24 = WHEnt.IN_TIME;


                        if (DateTime.TryParseExact(inTime24, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                        {
                            int inhour24 = time.Hour;
                            int inhour12 = inhour24 % 12;
                            if (inhour12 == 0) inhour12 = 12;

                            string inminute = time.Minute.ToString("00");
                            string inampm = inhour24 < 12 ? "AM" : "PM";

                            // Populate the controls
                            txtInHr.Text = inhour12.ToString();
                            txtInMin.Text = inminute;
                            ddlInAMPM.SelectedValue = inampm;

                        }
                    }
                    if (WHEnt.OUT_TIME == "00:00")
                    {
                        txtOutHr.Text = "00";
                        txtOutMin.Text = "00";
                        ddlOutAMPM.SelectedValue = "AM";
                    }
                    else
                    {
                        string outTime24 = WHEnt.OUT_TIME;
                        if (DateTime.TryParseExact(outTime24, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                        {
                            int outhour24 = time.Hour;
                            int outhour12 = outhour24 % 12;
                            if (outhour12 == 0) outhour12 = 12;

                            string outminute = time.Minute.ToString("00");
                            string outampm = outhour24 < 12 ? "AM" : "PM";

                            // Populate the controls
                            txtOutHr.Text = outhour12.ToString();
                            txtOutMin.Text = outminute;
                            ddlOutAMPM.SelectedValue = outampm;
                        }
                    }
                }
            }
        }
    }
    protected void ddlDateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDateType.SelectedValue == "Eng")
        {
            ddlEngMonth.Visible = true;
            ddlNepMonth.Visible = false;
            ddlEngYear.Visible = true;
            ddlNepYear.Visible = false;
        }
        else
        {
            ddlEngMonth.Visible = false;
            ddlNepMonth.Visible = true;
            ddlEngYear.Visible = false;
            ddlNepYear.Visible = true;
        }
        LoadGrid();
    }
    protected void grdEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "View")
        {
            LoadEnglishYears(DateTime.Now.Year, DateTime.Now.Year + 1);
            LoadNepaliYears(Convert.ToInt16( PGD.NepaliYear()), Convert.ToInt16(PGD.NepaliYear())+1);
            ddlDiv.Items.Insert(0, "Select");
            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblEmpId = gr.FindControl("lblEmpId") as Label;
            Label lblEmpName = gr.FindControl("lblEmpName") as Label;
            lblEmployeeID.Text = lblEmpId.Text;
            lblEmployeeName.Text = lblEmpName.Text;
            gridDutyHr.Visible = true;
            LoadGrid();

            grdEmployee.Visible = false;
            tblEmpInfo.Visible = true;
            gridDutyHr.Visible = true;
            tblSaveBtn.Visible = true;
        }
    }

    protected void rbtnWorking_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rbtnWorkingH = gridDutyHr.HeaderRow.FindControl("rbtnWorkingH") as RadioButtonList;
        string headerValue = rbtnWorkingH.SelectedValue;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            RadioButtonList rbtnWorking = gr.FindControl("rbtnWorking") as RadioButtonList;
            rbtnWorking.Text = headerValue;
        }
    }

    #region In Time
    protected void txtInHrH_TextChanged(object sender, EventArgs e)
    {
        TextBox txtInHrH = gridDutyHr.HeaderRow.FindControl("txtInHrH") as TextBox;
        string headerValue = txtInHrH.Text;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            TextBox txtInHr = gr.FindControl("txtInHr") as TextBox;
            txtInHr.Text = headerValue;
        }
    }

    protected void txtInMinH_TextChanged(object sender, EventArgs e)
    {
        TextBox txtInMinH = gridDutyHr.HeaderRow.FindControl("txtInMinH") as TextBox;
        string headerValue = txtInMinH.Text;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            TextBox txtInMin = gr.FindControl("txtInMin") as TextBox;
            txtInMin.Text = headerValue;
        }
    }

    protected void ddlInAMPMH_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlInAMPMH = gridDutyHr.HeaderRow.FindControl("ddlInAMPMH") as DropDownList;
        string headerValue = ddlInAMPMH.SelectedValue;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            DropDownList ddlInAMPM = gr.FindControl("ddlInAMPM") as DropDownList;
            ddlInAMPM.SelectedValue = headerValue;
        }
    }

    #endregion

    #region Out Time
    protected void txtOutHrH_TextChanged(object sender, EventArgs e)
    {
        TextBox txtOutHrH = gridDutyHr.HeaderRow.FindControl("txtOutHrH") as TextBox;
        string headerValue = txtOutHrH.Text;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            TextBox txtOutHr = gr.FindControl("txtOutHr") as TextBox;
            txtOutHr.Text = headerValue;
        }
    }

    protected void txtOutMinH_TextChanged(object sender, EventArgs e)
    {
        TextBox txtOutMinH = gridDutyHr.HeaderRow.FindControl("txtOutMinH") as TextBox;
        string headerValue = txtOutMinH.Text;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            TextBox txtOutMin = gr.FindControl("txtOutMin") as TextBox;
            txtOutMin.Text = headerValue;
        }
    }

    protected void ddlOutAMPMH_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlOutAMPMH = gridDutyHr.HeaderRow.FindControl("ddlOutAMPMH") as DropDownList;
        string headerValue = ddlOutAMPMH.SelectedValue;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            DropDownList ddlOutAMPM = gr.FindControl("ddlOutAMPM") as DropDownList;
            ddlOutAMPM.SelectedValue = headerValue;
        }
    }
    #endregion

    protected void chkWorking_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gr = ((CheckBox)sender).Parent.Parent as GridViewRow;
        CheckBox chkWorking = gr.FindControl("chkWorking") as CheckBox;
        DropDownList ddlDutyType = gr.FindControl("ddlDutyType") as DropDownList;
        DropDownList ddlInAMPM = gr.FindControl("ddlInAMPM") as DropDownList;
        DropDownList ddlOutAMPM = gr.FindControl("ddlOutAMPM") as DropDownList;
        TextBox txtInHr = gr.FindControl("txtInHr") as TextBox;
        TextBox txtInMin = gr.FindControl("txtInMin") as TextBox;
        TextBox txtOutHr = gr.FindControl("txtOutHr") as TextBox;
        TextBox txtOutMin = gr.FindControl("txtOutMin") as TextBox;

        if (chkWorking.Checked == false)
        {
            gr.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffcccc");
            ddlDutyType.SelectedValue = "1";
            txtInHr.Text = "00";
            txtInMin.Text = "00";
            txtOutHr.Text = "00";
            txtOutMin.Text = "00";

            ddlDutyType.Enabled = false;
            txtInHr.Enabled = false;
            txtInMin.Enabled = false;
            txtOutHr.Enabled = false;
            txtOutMin.Enabled = false;
            ddlInAMPM.Enabled = false;
            ddlOutAMPM.Enabled = false;
        }
        else
        {
            gr.BackColor = System.Drawing.Color.Empty; // or any color you prefer

            ddlDutyType.Enabled = true;
            txtInHr.Enabled = true;
            txtInMin.Enabled = true;
            txtOutHr.Enabled = true;
            txtOutMin.Enabled = true;
            ddlInAMPM.Enabled = true;
            ddlOutAMPM.Enabled = true;
        }
    }

    protected void ddlEngMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid();
    }

    protected void ddlNepMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid();
    }


    protected void ddlEngYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid();
    }

    protected void ddlNepYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid();
    }
}