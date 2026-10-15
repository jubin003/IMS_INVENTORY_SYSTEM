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
using System.Globalization;

public partial class HR_Attendance_Duty_Hour_set_duty_hour : System.Web.UI.Page
{
    HR_EMPLOYEES EEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService ESer = new HR_EMPLOYEESService();

    HR_DIVISION DEnt = new HR_DIVISION();
    HR_DIVISIONService DSer = new HR_DIVISIONService();

    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    SET_HR_DUTY_HOUR SHEnt = new SET_HR_DUTY_HOUR();
    SET_HR_DUTY_HOURService SHSer = new SET_HR_DUTY_HOURService();

    WORKING_HOUR WHEnt = new WORKING_HOUR();
    WORKING_HOURService WHSer = new WORKING_HOURService();

    HelperFunction hf = new HelperFunction();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDepartment();
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
        DEnt = new HR_DIVISION();
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

    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDivision();
    }
    protected void gridDutyHr_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            TextBox txtPreInH = (TextBox)e.Row.FindControl("txtPreInH");
            TextBox txtPostInH = (TextBox)e.Row.FindControl("txtPostInH");
            TextBox txtDutyHrH = (TextBox)e.Row.FindControl("txtDutyHrH");
            DropDownList ddlInTimeH = (DropDownList)e.Row.FindControl("ddlInTimeH");
            DropDownList ddlOutTimeH = (DropDownList)e.Row.FindControl("ddlOutTimeH");
            DropDownList ddlWeekEnd1H = (DropDownList)e.Row.FindControl("ddlWeekEnd1H");
            DropDownList ddlWeekEnd2H = (DropDownList)e.Row.FindControl("ddlWeekEnd2H");
            DropDownList ddlDutyTypeH = (DropDownList)e.Row.FindControl("ddlDutyTypeH");

            ddlWeekEnd2H.SelectedValue = "0";
            #region Duty Hour
            WHEnt = new WORKING_HOUR();
            ddlDutyTypeH.DataSource = hf.GetDutyType();
            ddlDutyTypeH.DataTextField = "TYPE";
            ddlDutyTypeH.DataValueField = "CODE";
            ddlDutyTypeH.DataBind();
            #endregion
            #region out time
            ddlOutTimeH.Items.Clear();

            for (int hour = 0; hour < 24; hour++)
            {
                for (int minute = 0; minute < 60; minute += 30) // 15-minute intervals
                {
                    string time = string.Format("{0:D2}:{1:D2}", hour, minute);
                    ddlOutTimeH.Items.Add(new ListItem(time, time));
                }
            }
            #endregion
            #region In time
            ddlInTimeH.Items.Clear();

            for (int hour = 0; hour < 24; hour++)
            {
                for (int minute = 0; minute < 60; minute += 30) // 15-minute intervals
                {
                    string time = string.Format("{0:D2}:{1:D2}", hour, minute);
                    ddlInTimeH.Items.Add(new ListItem(time, time));
                }
            }
            #endregion

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblEmpId = (Label)e.Row.FindControl("lblEmpId");
            Label lblEmpName = (Label)e.Row.FindControl("lblEmpName");
            TextBox txtPre_in = (TextBox)e.Row.FindControl("txtPre_in");
            TextBox txtPost_in = (TextBox)e.Row.FindControl("txtPost_in");
            TextBox txtDutyHr = (TextBox)e.Row.FindControl("txtDutyHr");
            DropDownList ddlInTime = (DropDownList)e.Row.FindControl("ddlInTime");
            DropDownList ddlOutTime = (DropDownList)e.Row.FindControl("ddlOutTime");
            DropDownList ddlWeekEnd1 = (DropDownList)e.Row.FindControl("ddlWeekEnd1");
            DropDownList ddlWeekEnd2 = (DropDownList)e.Row.FindControl("ddlWeekEnd2");
            RadioButtonList rdBtnFixDuty = (RadioButtonList)e.Row.FindControl("rdBtnFixDuty");
            DropDownList ddlDutyType = (DropDownList)e.Row.FindControl("ddlDutyType");

            lblEmpName.Text = hf.getEmployeeName(lblEmpId.Text);

            if (rdBtnFixDuty.SelectedValue == "0")
            {

            }
            else
            {

            }

            #region Duty Hour
            WHEnt = new WORKING_HOUR();
            ddlDutyType.DataSource = hf.GetDutyType();
            ddlDutyType.DataTextField = "TYPE";
            ddlDutyType.DataValueField = "CODE";
            ddlDutyType.DataBind();
            #endregion
            #region out time
            ddlOutTime.Items.Clear();

            for (int hour = 0; hour < 24; hour++)
            {
                for (int minute = 0; minute < 60; minute += 30) // 15-minute intervals
                {
                    string time = string.Format("{0:D2}:{1:D2}", hour, minute);
                    ddlOutTime.Items.Add(new ListItem(time, time));
                }
            }
            #endregion
            #region In time
            ddlInTime.Items.Clear();

            for (int hour = 0; hour < 24; hour++)
            {
                for (int minute = 0; minute < 60; minute += 30) // 15-minute intervals
                {
                    string time = string.Format("{0:D2}:{1:D2}", hour, minute);
                    ddlInTime.Items.Add(new ListItem(time, time));
                }
            }
            #endregion

            SHEnt = new SET_HR_DUTY_HOUR();
            SHEnt.EMP_ID = lblEmpId.Text;
            SHEnt = (SET_HR_DUTY_HOUR)SHSer.GetSingle(SHEnt);
            if (SHEnt != null)
            {
                if (SHEnt.FIXED_DUTY_ROSTER == "1")
                {
                    rdBtnFixDuty.SelectedValue = SHEnt.FIXED_DUTY_ROSTER;
                    ddlInTime.Enabled = true;
                    ddlOutTime.Enabled = true;
                    ddlWeekEnd1.Enabled = true;
                    ddlWeekEnd2.Enabled = true;
                    ddlDutyType.Enabled = true;
                    txtDutyHr.Text = "0";
                }
                else
                {
                    rdBtnFixDuty.SelectedValue = "0";

                    ddlInTime.Enabled = false;
                    ddlOutTime.Enabled = false;
                    ddlWeekEnd1.Enabled = false;
                    ddlWeekEnd2.Enabled = false;
                    ddlDutyType.Enabled = false;
                    ddlWeekEnd1.SelectedValue = "0";
                }
                ddlInTime.SelectedValue = SHEnt.DUTY_IN_TIME;
                ddlOutTime.SelectedValue = SHEnt.DUTY_OUT_TIME;
                ddlWeekEnd1.SelectedValue = SHEnt.WEEKEND1;
                ddlWeekEnd2.SelectedValue = SHEnt.WEEKEND2;
                txtPre_in.Text = SHEnt.PRE_IN_OUT_ROUND;
                txtPost_in.Text = SHEnt.POST_IN_OUT_ROUND;
                txtDutyHr.Text = SHEnt.DUTY_HOUR;
                ddlDutyType.SelectedValue = SHEnt.DUTY_TYPE;
            }
            else
            {
                checkFixTime();
            }
        }
    }

    protected void bttnSearch_Click(object sender, EventArgs e)
    {
        LoadGrid();
    }
    protected void LoadGrid()
    {
        EEnt = new HR_EMPLOYEES();
        EntityList EList = new EntityList();
        if (ddlDiv.SelectedValue != "Select")
        {
            EEnt.DIVISION = ddlDiv.SelectedValue;
        }
        if (ddlDept.SelectedValue != "Select")
        {
            EEnt.DEPARTMENTID = ddlDept.SelectedValue;
        }
        EEnt.STATUS = ddlStatus.SelectedValue;

        EList = ESer.GetAll(EEnt);
        gridDutyHr.DataSource = EList;
        gridDutyHr.DataBind();
        divGrid.Visible = (gridDutyHr.Rows.Count > 1);
    }
    protected void gridDutyHr_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SaveRow")
        {
            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblEmpId = gr.FindControl("lblEmpId") as Label;
            TextBox txtPre_in = gr.FindControl("txtPre_in") as TextBox;
            TextBox txtPost_in = gr.FindControl("txtPost_in") as TextBox;
            TextBox txtDutyHr = gr.FindControl("txtDutyHr") as TextBox;
            TextBox txtCutOff = gr.FindControl("txtCutOff") as TextBox;
            DropDownList ddlInTime = gr.FindControl("ddlInTime") as DropDownList;
            DropDownList ddlOutTime = gr.FindControl("ddlOutTime") as DropDownList;
            DropDownList ddlWeekEnd1 = gr.FindControl("ddlWeekEnd1") as DropDownList;
            DropDownList ddlWeekEnd2 = gr.FindControl("ddlWeekEnd2") as DropDownList;
            DropDownList ddlDutyType = gr.FindControl("ddlDutyType") as DropDownList;
            RadioButtonList rdBtnFixDuty = gr.FindControl("rdBtnFixDuty") as RadioButtonList;


            SHEnt = new SET_HR_DUTY_HOUR();
            SHEnt.EMP_ID = lblEmpId.Text;
            SHEnt = (SET_HR_DUTY_HOUR)SHSer.GetSingle(SHEnt);
            if (SHEnt != null)
            {
                SHEnt.PRE_IN_OUT_ROUND = txtPre_in.Text;
                SHEnt.POST_IN_OUT_ROUND = txtPost_in.Text;
                SHEnt.DUTY_HOUR = txtDutyHr.Text;
                SHEnt.FIXED_DUTY_ROSTER = rdBtnFixDuty.SelectedValue;
                if (rdBtnFixDuty.SelectedValue == "0")
                {
                    SHEnt.FIXED_DUTY_ROSTER = null;
                    SHEnt.DUTY_IN_TIME = "";
                    SHEnt.DUTY_OUT_TIME = "";
                    SHEnt.WEEKEND1 = "";
                    SHEnt.WEEKEND2 = "";
                    SHEnt.DUTY_TYPE = "";
                }
                else
                {
                    SHEnt.DUTY_IN_TIME = ddlInTime.SelectedItem.Text;
                    SHEnt.DUTY_OUT_TIME = ddlOutTime.SelectedItem.Text;
                    SHEnt.WEEKEND1 = ddlWeekEnd1.SelectedValue;
                    SHEnt.WEEKEND2 = ddlWeekEnd2.SelectedValue;
                    SHEnt.DUTY_TYPE = ddlDutyType.SelectedValue;
                }
                SHSer.Update(SHEnt);
            }
            else
            {
                SHEnt = new SET_HR_DUTY_HOUR();
                SHEnt.EMP_ID = lblEmpId.Text;
                SHEnt.PRE_IN_OUT_ROUND = txtPre_in.Text;
                SHEnt.POST_IN_OUT_ROUND = txtPost_in.Text;
                SHEnt.DUTY_HOUR = txtDutyHr.Text;
                SHEnt.FIXED_DUTY_ROSTER = rdBtnFixDuty.SelectedValue;
                if (rdBtnFixDuty.SelectedValue == "0")
                {
                    SHEnt.DUTY_IN_TIME = "";
                    SHEnt.DUTY_OUT_TIME = "";
                    SHEnt.WEEKEND1 = "";
                    SHEnt.WEEKEND2 = "";
                    SHEnt.DUTY_TYPE = "";
                }
                else
                {
                    SHEnt.DUTY_IN_TIME = ddlInTime.SelectedItem.Text;
                    SHEnt.DUTY_OUT_TIME = ddlOutTime.SelectedItem.Text;
                    SHEnt.WEEKEND1 = ddlWeekEnd1.SelectedValue;
                    SHEnt.WEEKEND2 = ddlWeekEnd2.SelectedValue;
                    SHEnt.DUTY_TYPE = ddlDutyType.SelectedValue;
                }
                SHSer.Insert(SHEnt);
            }
            HelperFunction.MsgBox(this, this.GetType(), "Data Updated");
            LoadGrid();

            if (rdBtnFixDuty.SelectedValue == "0")
            {
                ddlDutyType.Enabled = false;
                ddlInTime.Enabled = false;
                ddlOutTime.Enabled = false;
                ddlWeekEnd1.Enabled = false;
                ddlWeekEnd2.Enabled = false;
            }
            else
            {
                ddlDutyType.Enabled = true;
                ddlInTime.Enabled = true;
                ddlOutTime.Enabled = true;
                ddlWeekEnd1.Enabled = true;
                ddlWeekEnd2.Enabled = true;
            }
        }
    }
    protected void btnSaveGrid_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            Label lblEmpId = gr.FindControl("lblEmpId") as Label;
            TextBox txtPre_in = gr.FindControl("txtPre_in") as TextBox;
            TextBox txtPost_in = gr.FindControl("txtPost_in") as TextBox;
            TextBox txtDutyHr = gr.FindControl("txtDutyHr") as TextBox;
            TextBox txtCutOff = gr.FindControl("txtCutOff") as TextBox;
            DropDownList ddlInTime = gr.FindControl("ddlInTime") as DropDownList;
            DropDownList ddlOutTime = gr.FindControl("ddlOutTime") as DropDownList;
            DropDownList ddlWeekEnd1 = gr.FindControl("ddlWeekEnd1") as DropDownList;
            DropDownList ddlWeekEnd2 = gr.FindControl("ddlWeekEnd2") as DropDownList;
            DropDownList ddlDutyType = gr.FindControl("ddlDutyType") as DropDownList;
            RadioButtonList rdBtnFixDuty = gr.FindControl("rdBtnFixDuty") as RadioButtonList;

            SHEnt = new SET_HR_DUTY_HOUR();
            SHEnt.EMP_ID = lblEmpId.Text;
            SHEnt = (SET_HR_DUTY_HOUR)SHSer.GetSingle(SHEnt);
            if (SHEnt != null)
            {
                SHEnt.PRE_IN_OUT_ROUND = txtPre_in.Text;
                SHEnt.POST_IN_OUT_ROUND = txtPost_in.Text;
                SHEnt.DUTY_HOUR = txtDutyHr.Text;
                SHEnt.FIXED_DUTY_ROSTER = rdBtnFixDuty.SelectedValue;
                if (rdBtnFixDuty.SelectedValue == "0")
                {
                    SHEnt.FIXED_DUTY_ROSTER = null;
                    SHEnt.DUTY_IN_TIME = "";
                    SHEnt.DUTY_OUT_TIME = "";
                    SHEnt.WEEKEND1 = "";
                    SHEnt.WEEKEND2 = "";
                    SHEnt.DUTY_TYPE = "";
                }
                else
                {
                    SHEnt.DUTY_IN_TIME = ddlInTime.SelectedItem.Text;
                    SHEnt.DUTY_OUT_TIME = ddlOutTime.SelectedItem.Text;
                    SHEnt.WEEKEND1 = ddlWeekEnd1.SelectedValue;
                    SHEnt.WEEKEND2 = ddlWeekEnd2.SelectedValue;
                    SHEnt.DUTY_TYPE = ddlDutyType.SelectedValue;
                }
                SHSer.Update(SHEnt);
            }
            else
            {
                SHEnt = new SET_HR_DUTY_HOUR();
                SHEnt.EMP_ID = lblEmpId.Text;
                SHEnt.PRE_IN_OUT_ROUND = txtPre_in.Text;
                SHEnt.POST_IN_OUT_ROUND = txtPost_in.Text;
                SHEnt.DUTY_HOUR = txtDutyHr.Text;
                SHEnt.FIXED_DUTY_ROSTER = rdBtnFixDuty.SelectedValue;
                if (rdBtnFixDuty.SelectedValue == "0")
                {
                    SHEnt.DUTY_IN_TIME = "";
                    SHEnt.DUTY_OUT_TIME = "";
                    SHEnt.WEEKEND1 = "";
                    SHEnt.WEEKEND2 = "";
                    SHEnt.DUTY_TYPE = "";
                }
                else
                {
                    SHEnt.DUTY_IN_TIME = ddlInTime.SelectedItem.Text;
                    SHEnt.DUTY_OUT_TIME = ddlOutTime.SelectedItem.Text;
                    SHEnt.WEEKEND1 = ddlWeekEnd1.SelectedValue;
                    SHEnt.WEEKEND2 = ddlWeekEnd2.SelectedValue;
                    SHEnt.DUTY_TYPE = ddlDutyType.SelectedValue;
                }
                SHSer.Insert(SHEnt);
            }
        }
        LoadGrid();
        HelperFunction.MsgBox(this, this.GetType(), "Saved");
    }

    #region For HeaderRow
    protected void txtDutyHrH_TextChanged(object sender, EventArgs e)
    {
        addDutyHr();
    }
    protected void addDutyHr()
    {
        TextBox txtDutyHrH = gridDutyHr.HeaderRow.FindControl("txtDutyHrH") as TextBox;
        string headerValue = txtDutyHrH.Text;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            TextBox txtDutyHr = gr.FindControl("txtDutyHr") as TextBox;
            txtDutyHr.Text = headerValue;
        }
    }
    protected void rdBtnFixDutyH_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rdBtnFixDutyH = gridDutyHr.HeaderRow.FindControl("rdBtnFixDutyH") as RadioButtonList;
        DropDownList ddlInTimeH = gridDutyHr.HeaderRow.FindControl("ddlInTimeH") as DropDownList;
        DropDownList ddlOutTimeH = gridDutyHr.HeaderRow.FindControl("ddlOutTimeH") as DropDownList;
        DropDownList ddlWeekEnd1H = gridDutyHr.HeaderRow.FindControl("ddlWeekEnd1H") as DropDownList;
        DropDownList ddlWeekEnd2H = gridDutyHr.HeaderRow.FindControl("ddlWeekEnd2H") as DropDownList;
        DropDownList ddlDutyTypeH = gridDutyHr.HeaderRow.FindControl("ddlDutyTypeH") as DropDownList;

        if (rdBtnFixDutyH.SelectedValue == "0")
        {
            ddlInTimeH.Enabled = false;
            ddlOutTimeH.Enabled = false;
            ddlWeekEnd1H.Enabled = false;
            ddlWeekEnd2H.Enabled = false;
            ddlDutyTypeH.Enabled = false;
            ddlInTimeH.SelectedItem.Text = "00:00";
            ddlOutTimeH.SelectedItem.Text = "00:00";
            ddlDutyTypeH.SelectedValue = "OFF";
        }
        else
        {
            ddlInTimeH.Enabled = true;
            ddlOutTimeH.Enabled = true;
            ddlWeekEnd1H.Enabled = true;
            ddlWeekEnd2H.Enabled = true;
            ddlDutyTypeH.Enabled = true;
            LoadGrid();
        }

        string headerValue = rdBtnFixDutyH.Text;
        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            RadioButtonList rdBtnFixDuty = gr.FindControl("rdBtnFixDuty") as RadioButtonList;
            DropDownList ddlInTime = gr.FindControl("ddlInTime") as DropDownList;
            DropDownList ddlOutTime = gr.FindControl("ddlOutTime") as DropDownList;
            DropDownList ddlWeekEnd1 = gr.FindControl("ddlWeekEnd1") as DropDownList;
            DropDownList ddlWeekEnd2 = gr.FindControl("ddlWeekEnd2") as DropDownList;
            DropDownList ddlDutyType = gr.FindControl("ddlDutyType") as DropDownList;

            rdBtnFixDuty.SelectedValue = headerValue;
            if (rdBtnFixDuty.SelectedValue == "0")
            {
                ddlInTime.Enabled = false;
                ddlOutTime.Enabled = false;
                ddlWeekEnd1.Enabled = false;
                ddlWeekEnd2.Enabled = false;
                ddlDutyType.Enabled = false;
                ddlDutyType.SelectedItem.Text = "-";
                ddlInTime.SelectedItem.Text = "00:00";
                ddlOutTime.SelectedItem.Text = "00:00";

            }
            else
            {
                ddlInTime.Enabled = true;
                ddlOutTime.Enabled = true;
                ddlWeekEnd1.Enabled = true;
                ddlWeekEnd2.Enabled = true;
                ddlDutyType.Enabled = true;
            }
        }
    }
    protected void ddlInTimeH_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlInTimeH = gridDutyHr.HeaderRow.FindControl("ddlInTimeH") as DropDownList;
        string headerValue = ddlInTimeH.SelectedItem.Text;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            DropDownList ddlInTime = gr.FindControl("ddlInTime") as DropDownList;
            ddlInTime.SelectedItem.Text = headerValue;
        }
        calculateDutyHrHeader();
    }
    protected void ddlOutTimeH_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlOutTimeH = gridDutyHr.HeaderRow.FindControl("ddlOutTimeH") as DropDownList;
        string headerValue = ddlOutTimeH.SelectedItem.Text;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            DropDownList ddlOutTime = gr.FindControl("ddlOutTime") as DropDownList;
            ddlOutTime.SelectedItem.Text = headerValue;
        }
        calculateDutyHrHeader();
    }
    protected void ddlWeekEnd1H_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlWeekEnd1H = gridDutyHr.HeaderRow.FindControl("ddlWeekEnd1H") as DropDownList;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            DropDownList ddlWeekEnd1 = gr.FindControl("ddlWeekEnd1") as DropDownList;
            ddlWeekEnd1.SelectedValue = ddlWeekEnd1H.SelectedValue;
        }
    }
    protected void ddlWeekEnd2H_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlWeekEnd2H = gridDutyHr.HeaderRow.FindControl("ddlWeekEnd2H") as DropDownList;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            DropDownList ddlWeekEnd2 = gr.FindControl("ddlWeekEnd2") as DropDownList;
            ddlWeekEnd2.SelectedValue = ddlWeekEnd2H.SelectedValue;
        }
    }
    protected void txtCutOffH_TextChanged(object sender, EventArgs e)
    {
        TextBox txtCutOffH = gridDutyHr.HeaderRow.FindControl("txtCutOffH") as TextBox;
        string headerValue = txtCutOffH.Text;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            TextBox txtCutOff = gr.FindControl("txtCutOff") as TextBox;
            txtCutOff.Text = headerValue;
        }
    }
    protected void txtPreInH_TextChanged(object sender, EventArgs e)
    {
        TextBox txtPreInH = gridDutyHr.HeaderRow.FindControl("txtPreInH") as TextBox;
        string headerValue = txtPreInH.Text;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            TextBox txtPre_in = gr.FindControl("txtPre_in") as TextBox;
            txtPre_in.Text = headerValue;
        }
    }
    protected void txtPostInH_TextChanged(object sender, EventArgs e)
    {
        TextBox txtPostInH = gridDutyHr.HeaderRow.FindControl("txtPostInH") as TextBox;
        string headerValue = txtPostInH.Text;

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            TextBox txtPost_in = gr.FindControl("txtPost_in") as TextBox;
            txtPost_in.Text = headerValue;
        }
    }
    protected void calculateDutyHrHeader()
    {
        DropDownList ddlInTimeH = gridDutyHr.HeaderRow.FindControl("ddlInTimeH") as DropDownList;
        DropDownList ddlOutTimeH = gridDutyHr.HeaderRow.FindControl("ddlOutTimeH") as DropDownList;
        TextBox txtDutyHrH = gridDutyHr.HeaderRow.FindControl("txtDutyHrH") as TextBox;

        if (ddlInTimeH != null && ddlOutTimeH != null && txtDutyHrH != null)
        {
            try
            {
                DateTime inTime = DateTime.ParseExact(ddlInTimeH.SelectedValue, "HH:mm", CultureInfo.InvariantCulture);
                DateTime outTime = DateTime.ParseExact(ddlOutTimeH.SelectedValue, "HH:mm", CultureInfo.InvariantCulture);

                if (outTime < inTime)
                {
                    outTime = outTime.AddDays(1);
                }

                TimeSpan duration = outTime - inTime;

                double totalHours = duration.TotalHours;
                txtDutyHrH.Text = totalHours.ToString();
            }
            catch (Exception ex)
            {
                txtDutyHrH.Text = "";
            }
        }
    }

    #endregion

    #region For ItemTemplate    
    protected void rdBtnFixDuty_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkFixTime();
    }
    protected void ddlInTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        calculateDutyHr();
    }
    protected void ddlOutTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        calculateDutyHr();
    }

    protected void calculateDutyHr()
    {
        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            TextBox txtDutyHr = gr.FindControl("txtDutyHr") as TextBox;
            DropDownList ddlInTime = gr.FindControl("ddlInTime") as DropDownList;
            DropDownList ddlOutTime = gr.FindControl("ddlOutTime") as DropDownList;
            if (ddlInTime != null && ddlOutTime != null && txtDutyHr != null)
            {
                try
                {
                    DateTime inTime = DateTime.ParseExact(ddlInTime.SelectedValue, "HH:mm", CultureInfo.InvariantCulture);
                    DateTime outTime = DateTime.ParseExact(ddlOutTime.SelectedValue, "HH:mm", CultureInfo.InvariantCulture);

                    if (outTime < inTime)
                    {
                        outTime = outTime.AddDays(1);
                    }

                    TimeSpan duration = outTime - inTime;

                    double totalHours = duration.TotalHours;
                    txtDutyHr.Text = totalHours.ToString();
                }
                catch (Exception ex)
                {
                    txtDutyHr.Text = "";
                }
            }
        }

    }
    protected void checkFixTime()
    {
        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            RadioButtonList rdBtnFixDuty = gr.FindControl("rdBtnFixDuty") as RadioButtonList;
            TextBox txtDutyHr = gr.FindControl("txtDutyHr") as TextBox;
            DropDownList ddlInTime = gr.FindControl("ddlInTime") as DropDownList;
            DropDownList ddlOutTime = gr.FindControl("ddlOutTime") as DropDownList;
            DropDownList ddlWeekEnd1 = gr.FindControl("ddlWeekEnd1") as DropDownList;
            DropDownList ddlWeekEnd2 = gr.FindControl("ddlWeekEnd2") as DropDownList;
            DropDownList ddlDutyType = gr.FindControl("ddlDutyType") as DropDownList;

            if (rdBtnFixDuty.SelectedValue == "0")
            {
                ddlInTime.Enabled = false;
                ddlOutTime.Enabled = false;
                ddlWeekEnd1.Enabled = false;
                ddlWeekEnd2.Enabled = false;
                ddlDutyType.Enabled = false;
                txtDutyHr.Text = "0";
                ddlWeekEnd1.SelectedValue = "0";

            }
            else
            {
                ddlInTime.Enabled = true;
                ddlOutTime.Enabled = true;
                ddlWeekEnd1.Enabled = true;
                ddlWeekEnd2.Enabled = true;
                ddlDutyType.Enabled = true;
                ddlWeekEnd1.SelectedValue = "7";
            }
        }
    }

    #endregion


    protected void ddlDutyTypeH_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlDutyTypeH = gridDutyHr.HeaderRow.FindControl("ddlDutyTypeH") as DropDownList;
        DropDownList ddlInTimeH = gridDutyHr.HeaderRow.FindControl("ddlInTimeH") as DropDownList;
        DropDownList ddlOutTimeH = gridDutyHr.HeaderRow.FindControl("ddlOutTimeH") as DropDownList;
        TextBox txtDutyHrH = gridDutyHr.HeaderRow.FindControl("txtDutyHrH") as TextBox;

        WHEnt = new WORKING_HOUR();
        WHEnt.CODE = ddlDutyTypeH.SelectedValue;
        WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
        if (WHEnt != null)
        {
            txtDutyHrH.Text = WHEnt.DUTY_HOUR;
            ddlInTimeH.SelectedValue = WHEnt.IN_TIME;
            ddlOutTimeH.SelectedValue = WHEnt.OUT_TIME;
        }

        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            Label lblEmpId = gr.FindControl("lblEmpId") as Label;
            TextBox txtDutyHr = gr.FindControl("txtDutyHr") as TextBox;
            DropDownList ddlInTime = gr.FindControl("ddlInTime") as DropDownList;
            DropDownList ddlOutTime = gr.FindControl("ddlOutTime") as DropDownList;
            DropDownList ddlDutyType = gr.FindControl("ddlDutyType") as DropDownList;

            ddlDutyType.SelectedValue = ddlDutyTypeH.SelectedValue;
            txtDutyHr.Text = txtDutyHrH.Text;
            ddlInTime.SelectedValue = ddlInTimeH.SelectedValue;
            ddlOutTime.SelectedValue = ddlOutTimeH.SelectedValue;
        }

    }

    protected void ddlDutyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadTime();
    }
    protected void loadTime()
    {
        foreach (GridViewRow gr in gridDutyHr.Rows)
        {
            DropDownList ddlDutyType = gr.FindControl("ddlDutyType") as DropDownList;
            TextBox txtDutyHr = gr.FindControl("txtDutyHr") as TextBox;
            DropDownList ddlInTime = gr.FindControl("ddlInTime") as DropDownList;
            DropDownList ddlOutTime = gr.FindControl("ddlOutTime") as DropDownList;

            WHEnt = new WORKING_HOUR();
            WHEnt.CODE = ddlDutyType.SelectedValue;
            WHEnt = (WORKING_HOUR)WHSer.GetSingle(WHEnt);
            if (WHEnt != null)
            {
                txtDutyHr.Text = WHEnt.DUTY_HOUR;
                ddlInTime.SelectedValue = WHEnt.IN_TIME;
                ddlOutTime.SelectedValue = WHEnt.OUT_TIME;
            }
        }
    }


}
