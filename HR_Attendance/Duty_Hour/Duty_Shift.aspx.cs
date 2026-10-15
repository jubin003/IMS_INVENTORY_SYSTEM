using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;
using System.Globalization;

public partial class HR_Attendance_Duty_Hour_Duty_Shift : System.Web.UI.Page
{
    WORKING_HOUR WHEnt = new WORKING_HOUR();
    WORKING_HOURService WHSer = new WORKING_HOURService();

    EntityList theList = new EntityList();

    Boolean IsPageRefresh = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loadGrid();
        }
    }

    protected void loadGrid()
    {
        WHEnt = new WORKING_HOUR();
        theList = WHSer.GetAll(WHEnt);
        if (theList.Count == 0)
        {
            theList.Add(WHEnt);
        }
        grdWrkSchedule.DataSource = theList;
        grdWrkSchedule.DataBind();
    }

    protected void btnSaveH_Click(object sender, EventArgs e)
    {
        if (!IsPageRefresh)
        {
            GridViewRow row = grdWrkSchedule.HeaderRow;
            TextBox txtShiftH = (TextBox)row.FindControl("txtShiftH");
            TextBox txtCodeH = (TextBox)row.FindControl("txtCodeH");
            TextBox txtInHrH = (TextBox)row.FindControl("txtInHrH");
            TextBox txtInMinH = (TextBox)row.FindControl("txtInMinH");
            TextBox txtOutHrH = (TextBox)row.FindControl("txtOutHrH");
            TextBox txtOutMinH = (TextBox)row.FindControl("txtOutMinH");
            TextBox txtDutyHrH = (TextBox)row.FindControl("txtDutyHrH");
            DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");
            DropDownList ddlInAMPMH = (DropDownList)row.FindControl("ddlInAMPMH");
            DropDownList ddlOutAMPMH = (DropDownList)row.FindControl("ddlOutAMPMH");

            WHEnt = new WORKING_HOUR();
            WHEnt.TYPE = txtShiftH.Text;
            WHEnt.CODE = txtCodeH.Text;

            string inHours = txtInHrH.Text.Trim();
            string inMins = txtInMinH.Text.Trim();
            string inAmPm = ddlInAMPMH.SelectedValue;
            WHEnt.IN_TIME = Convert12To24HourFormat(inHours, inMins, inAmPm);

            string outHours = txtOutHrH.Text.Trim();
            string outMins = txtOutMinH.Text.Trim();
            string outAmPm = ddlOutAMPMH.SelectedValue;
            WHEnt.OUT_TIME = Convert12To24HourFormat(outHours, outMins, outAmPm);

            WHEnt.DUTY_HOUR = txtDutyHrH.Text;
            WHEnt.STATUS = ddlStatusH.SelectedValue;
            WHSer.Insert(WHEnt);
            loadGrid();
        }
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

    protected void TimeComponent_Changed(object sender, EventArgs e)
    {
        DropDownList ddlInAMPMH = grdWrkSchedule.HeaderRow.FindControl("ddlInAMPMH") as DropDownList;
        DropDownList ddlOutAMPMH = grdWrkSchedule.HeaderRow.FindControl("ddlOutAMPMH") as DropDownList;
        TextBox txtInHrH = grdWrkSchedule.HeaderRow.FindControl("txtInHrH") as TextBox;
        TextBox txtInMinH = grdWrkSchedule.HeaderRow.FindControl("txtInMinH") as TextBox;
        TextBox txtOutHrH = grdWrkSchedule.HeaderRow.FindControl("txtOutHrH") as TextBox;
        TextBox txtOutMinH = grdWrkSchedule.HeaderRow.FindControl("txtOutMinH") as TextBox;

        TextBox txtDutyHrH = grdWrkSchedule.HeaderRow.FindControl("txtDutyHrH") as TextBox;

        try
        {
            int inHr = int.Parse(txtInHrH.Text.Trim());
            int inMin = int.Parse(txtInMinH.Text.Trim());
            int outHr = int.Parse(txtOutHrH.Text.Trim());
            int outMin = int.Parse(txtOutMinH.Text.Trim());

            string inAMPM = ddlInAMPMH.SelectedValue;
            string outAMPM = ddlOutAMPMH.SelectedValue;

            if (inAMPM == "PM" && inHr != 12)
                inHr += 12;
            else if (inAMPM == "AM" && inHr == 12)
                inHr = 0;

            if (outAMPM == "PM" && outHr != 12)
                outHr += 12;
            else if (outAMPM == "AM" && outHr == 12)
                outHr = 0;

            TimeSpan inTime = new TimeSpan(inHr, inMin, 0);
            TimeSpan outTime = new TimeSpan(outHr, outMin, 0);

            TimeSpan duration = outTime - inTime;

            if (duration.TotalMinutes < 0)
                duration = duration.Add(new TimeSpan(24, 0, 0));

            double totalMinutes = duration.TotalMinutes;
            int totalHours = (int)(totalMinutes / 60);
            double remainingMinutes = totalMinutes % 60;

            // Logic for formatting
            string dutyHour;

            if (remainingMinutes >= 15 && remainingMinutes <= 45)
            {
                dutyHour = totalHours.ToString() + ".5";
            }
            else if (remainingMinutes < 15)
            {
                dutyHour = totalHours.ToString() + ".0";
            }
            else
            {
                dutyHour = (totalHours + 1).ToString(); // Round up
            }

            txtDutyHrH.Text = dutyHour;
        }
        catch
        {
            txtDutyHrH.Text = "";
        }
    }

    protected void TimeComponentI_Changed(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in grdWrkSchedule.Rows)
        {
            if (gr.RowIndex == grdWrkSchedule.EditIndex) // Only the row in edit mode
            {
                DropDownList ddlInAMPM = gr.FindControl("ddlInAMPM") as DropDownList;
                DropDownList ddlOutAMPM = gr.FindControl("ddlOutAMPM") as DropDownList;
                TextBox txtInHr = gr.FindControl("txtInHr") as TextBox;
                TextBox txtInMin = gr.FindControl("txtInMin") as TextBox;
                TextBox txtOutHr = gr.FindControl("txtOutHr") as TextBox;
                TextBox txtOutMin = gr.FindControl("txtOutMin") as TextBox;

                TextBox txtDutyHrE = gr.FindControl("txtDutyHrE") as TextBox;

                try
                {
                    int inHr = int.Parse(txtInHr.Text.Trim());
                    int inMin = int.Parse(txtInMin.Text.Trim());
                    int outHr = int.Parse(txtOutHr.Text.Trim());
                    int outMin = int.Parse(txtOutMin.Text.Trim());

                    string inAMPM = ddlInAMPM.SelectedValue;
                    string outAMPM = ddlOutAMPM.SelectedValue;

                    if (inAMPM == "PM" && inHr != 12)
                        inHr += 12;
                    else if (inAMPM == "AM" && inHr == 12)
                        inHr = 0;

                    if (outAMPM == "PM" && outHr != 12)
                        outHr += 12;
                    else if (outAMPM == "AM" && outHr == 12)
                        outHr = 0;

                    TimeSpan inTime = new TimeSpan(inHr, inMin, 0);
                    TimeSpan outTime = new TimeSpan(outHr, outMin, 0);

                    TimeSpan duration = outTime - inTime;

                    if (duration.TotalMinutes < 0)
                        duration = duration.Add(new TimeSpan(24, 0, 0));

                    double totalMinutes = duration.TotalMinutes;
                    int totalHours = (int)(totalMinutes / 60);
                    double remainingMinutes = totalMinutes % 60;

                    // Logic for formatting
                    string dutyHour;

                    if (remainingMinutes >= 15 && remainingMinutes <= 45)
                    {
                        dutyHour = totalHours.ToString() + ".5";
                    }
                    else if (remainingMinutes < 15)
                    {
                        dutyHour = totalHours.ToString() + ".0";
                    }
                    else
                    {
                        dutyHour = (totalHours + 1).ToString(); // Round up
                    }

                    txtDutyHrE.Text = dutyHour;
                }
                catch
                {
                    txtDutyHrE.Text = "";
                }
            }
        }
    }

    protected void txtCodeH_TextChanged(object sender, EventArgs e)
    {
        TextBox txtCodeH = grdWrkSchedule.HeaderRow.FindControl("txtCodeH") as TextBox;
        Button btnSaveH = grdWrkSchedule.HeaderRow.FindControl("btnSaveH") as Button;

        WHEnt = new WORKING_HOUR();
        WHEnt.CODE = txtCodeH.Text;
        WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
        if (WHEnt != null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Code already exits. Please add another code !!");
            txtCodeH.Text = "";
            btnSaveH.Enabled = false;
        }
        else
        {
            btnSaveH.Enabled = true;
        }
    }

    protected void txtCodeE_TextChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in grdWrkSchedule.Rows)
        {
            if (gr.RowIndex == grdWrkSchedule.EditIndex) // Only the row in edit mode
            {
                TextBox txtCodeE = gr.FindControl("txtCodeE") as TextBox;
                Button btnSaveE = gr.FindControl("btnSaveE") as Button;

                string code = txtCodeE.Text;

                WHEnt = new WORKING_HOUR();
                WHEnt.CODE = txtCodeE.Text;
                WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
                if (WHEnt != null)
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Code already exits. Please add another code !!");
                    btnSaveE.Enabled = false;
                }
                else
                {
                    btnSaveE.Enabled = true;
                }
            }
        }
    }

    protected void grdWrkSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblPK_ID = e.Row.FindControl("lblPK_ID") as Label;
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lblShowStatus = e.Row.FindControl("lblShowStatus") as Label;
            if (lblStatus.Text == "1")
            {
                lblShowStatus.Text = "Available";
            }
            else
            {
                lblShowStatus.Text = "Unavailable";

            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            Label lblPK_ID = e.Row.FindControl("lblPK_ID") as Label;
            DropDownList ddlStatusE = e.Row.FindControl("ddlStatusE") as DropDownList;
            TextBox txtInHr = (TextBox)e.Row.FindControl("txtInHr");
            TextBox txtInMin = (TextBox)e.Row.FindControl("txtInMin");
            TextBox txtOutHr = (TextBox)e.Row.FindControl("txtOutHr");
            TextBox txtOutMin = (TextBox)e.Row.FindControl("txtOutMin");
            DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
            DropDownList ddlInAMPM = (DropDownList)e.Row.FindControl("ddlInAMPM");
            DropDownList ddlOutAMPM = (DropDownList)e.Row.FindControl("ddlOutAMPM");
            string inTime12 = "";
            string outTime12 = "";
            WHEnt = new WORKING_HOUR();
            WHEnt.PK_ID = lblPK_ID.Text;
            WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
            if (WHEnt != null)
            {
                ddlStatusE.SelectedValue = WHEnt.STATUS;
                inTime12 = Convert24To12HourFormat(WHEnt.IN_TIME);
                outTime12 = Convert24To12HourFormat(WHEnt.OUT_TIME);
            }

            #region IN Time
            inTime12 = Convert24To12HourFormat(WHEnt.IN_TIME);

            string[] intimeParts = inTime12.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string inhour12 = intimeParts[0];
            string inminute = intimeParts[1];
            string inampm = intimeParts[2];

            // Assign to controls
            txtInHr.Text = inhour12;
            txtInMin.Text = inminute;
            ddlInAMPM.SelectedValue = inampm;
            #endregion

            #region Out Time
            outTime12 = Convert24To12HourFormat(WHEnt.OUT_TIME);

            string[] outTimeParts = outTime12.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string outhour12 = outTimeParts[0];
            string outminute = outTimeParts[1];
            string outampm = outTimeParts[2];

            // Assign to controls
            txtOutHr.Text = outhour12;
            txtOutMin.Text = outminute;
            ddlOutAMPM.SelectedValue = outampm;
            #endregion

        }
    }

    protected void grdWrkSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void grdWrkSchedule_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdWrkSchedule.EditIndex = e.NewEditIndex;
        loadGrid();
    }

    protected void grdWrkSchedule_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdWrkSchedule.EditIndex = -1;
        loadGrid();
    }

    protected void grdWrkSchedule_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = grdWrkSchedule.Rows[e.RowIndex];
        Label lblPK_ID = row.FindControl("lblPK_ID") as Label;
        Label lblStatus = row.FindControl("lblStatus") as Label;

        TextBox txtShiftE = (TextBox)row.FindControl("txtShiftE");
        TextBox txtCodeE = (TextBox)row.FindControl("txtCodeE");
        TextBox txtInHr = (TextBox)row.FindControl("txtInHr");
        TextBox txtInMin = (TextBox)row.FindControl("txtInMin");
        TextBox txtOutHr = (TextBox)row.FindControl("txtOutHr");
        TextBox txtOutMin = (TextBox)row.FindControl("txtOutMin");
        TextBox txtDutyHrE = (TextBox)row.FindControl("txtDutyHrE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");
        DropDownList ddlInAMPM = (DropDownList)row.FindControl("ddlInAMPM");
        DropDownList ddlOutAMPM = (DropDownList)row.FindControl("ddlOutAMPM");

        WHEnt = new WORKING_HOUR();
        WHEnt.PK_ID = lblPK_ID.Text;
        WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
        if (WHEnt != null)
        {
            WHEnt.TYPE = txtShiftE.Text;
            WHEnt.CODE = txtCodeE.Text;

            string inHours = txtInHr.Text.Trim();
            string inMins = txtInMin.Text.Trim();
            string inAmPm = ddlInAMPM.SelectedValue;
            WHEnt.IN_TIME = Convert12To24HourFormat(inHours, inMins, inAmPm);

            string outHours = txtOutHr.Text.Trim();
            string outMins = txtOutMin.Text.Trim();
            string outAmPm = ddlOutAMPM.SelectedValue;
            WHEnt.OUT_TIME = Convert12To24HourFormat(outHours, outMins, outAmPm);

            WHEnt.DUTY_HOUR = txtDutyHrE.Text;
            WHEnt.STATUS = ddlStatusE.SelectedValue;
            WHSer.Update(WHEnt);
        }
        grdWrkSchedule.EditIndex = -1;
        loadGrid();
    }

}