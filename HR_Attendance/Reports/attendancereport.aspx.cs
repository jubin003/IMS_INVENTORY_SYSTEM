using System;
using System.Web;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Globalization;
using System.IO;
using System.Data;

public partial class HR_Attendance_Reports_attendancereport : System.Web.UI.Page
{
    Months MEnt = new Months();
    MonthsService MSer = new MonthsService();

    HR_DEPARTMENT DVEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DVSer = new HR_DEPARTMENTService();

    HR_ATTENDANCE ATEnt = new HR_ATTENDANCE();
    HR_ATTENDANCE ATSer = new HR_ATTENDANCE();

    HR_EMPLOYEES EMPEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService EMPSer = new HR_EMPLOYEESService();

    HR_DESIGNATION DGEnt = new HR_DESIGNATION();
    HR_DESIGNATIONService DGSer = new HR_DESIGNATIONService();

    SET_HR_DUTY_HOUR DHEnt = new SET_HR_DUTY_HOUR();
    SET_HR_DUTY_HOURService DHSer = new SET_HR_DUTY_HOURService();

    SET_HR_DUTY_ROSTER DREnt = new SET_HR_DUTY_ROSTER();
    SET_HR_DUTY_ROSTERService DRSer = new SET_HR_DUTY_ROSTERService();

    HelperFunction hf = new HelperFunction();
    PhyeGanDate PGD = new PhyeGanDate();
    static string fdate = "";
    string tdate = "";

    string fromdate = "";
    string todate = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserProfileEntity userProfileEnt = new UserProfileEntity();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            LoadDivision();
            txtYear.Text = PGD.NepaliYear();

            //txtYear.Text = DateTime.Now.Year.ToString();
            string[] date = (PGD.GetTodayNepaliDate()).Split('/');
            string currentmonth = int.Parse(date[1]).ToString();

            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
            ddlNepaliMonth.SelectedValue = currentmonth;
        }
    }

    protected void LoadDivision()
    {
        DVEnt = new HR_DEPARTMENT();
        ddlDepartment.DataSource = DVSer.GetAll(DVEnt);
        ddlDepartment.DataTextField = "DEP_NAME";
        ddlDepartment.DataValueField = "PK_ID";
        ddlDepartment.DataBind();
    }


    protected void lnkName_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((LinkButton)sender).Parent.Parent as GridViewRow;
        Label lblEmpID = (Label)gr.FindControl("lblEmpID");
        LoadIndividualAttendance(lblEmpID.Text, ddlDepartment.SelectedValue, ddlMonth.SelectedValue, txtYear.Text);
        allreport.Visible = false;
        individual.Visible = true;
        //  Response.Redirect("individualAttendance.aspx?empid=" + lblEmpID.Text+ "&officecode=" + ddlOfficeType.SelectedValue + "&fdate=" +fdate+"&tdate="+tdate);
    }

    protected void LoadIndividualAttendance(string empid, string officetype, string month, string year)
    {
        LoadInfo(empid);
        LoadGrid(empid);
    }

    protected void LoadInfo(string empid)
    {
        EMPEnt = new HR_EMPLOYEES();
        EMPEnt.EMPLOYEEID = empid;
        EMPEnt.DEPARTMENTID = ddlDepartment.SelectedValue;
        EMPEnt = (HR_EMPLOYEES)EMPSer.GetSingle(EMPEnt);
        if (EMPEnt != null)
        {
            lblEmpId.Text = EMPEnt.EMPLOYEEID;
            lblEmpName.Text = EMPEnt.FIRSTNAME + " " + EMPEnt.MIDDLE_NAME + " " + EMPEnt.LASTNAME;


            string imgfolder = "";
            imgfolder = Server.MapPath(@"~/images/Employee/") + empid + ".jpg";
            if (File.Exists(imgfolder))
            {
                imgEmployee.ImageUrl = "~/images/Employee/" + empid + ".jpg";

            }
            else
            {
                if (EMPEnt.GENDER.Trim() == "M")
                {
                    imgEmployee.ImageUrl = "~/images/Employee/male.jpg";
                }
                else if (EMPEnt.GENDER.Trim() == "F")
                {
                    imgEmployee.ImageUrl = "~/images/Employee /female.jpg";
                }
            }
        }

        DGEnt = new HR_DESIGNATION();
        DGEnt.PK_ID = EMPEnt.DESIGNATIONID;
        DGEnt = (HR_DESIGNATION)DGSer.GetSingle(DGEnt);
        if (DGEnt != null)
        {
            lblDesignation.Text = DGEnt.DESIGNATION_NAME;
        }

        lblDivision.Text = ddlDepartment.SelectedItem.ToString();

        if (ddlViewBy.SelectedValue == "M")
        {
            lblDate.Text = ddlMonth.SelectedItem.ToString() + " - " + txtYear.Text;
        }
        else if (ddlViewBy.SelectedValue == "D")
        {
            lblDate.Text = txtFromDateNp.Text + " - " + txtToDateNp.Text;
        }
    }

    protected void LoadGrid(string empid)
    {
        string nepali_lastday = "";
        if (ddlViewBy.SelectedValue == "M")
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            nepali_lastday = PGD.NepaliMonthsLastDay(txtYear.Text, ddlNepaliMonth.SelectedValue).ToString();
            fromdate = PGD.ConvertNepaliTOEnglish("1", ddlNepaliMonth.SelectedValue, txtYear.Text);
            todate = PGD.ConvertNepaliTOEnglish(nepali_lastday, ddlNepaliMonth.SelectedValue, txtYear.Text);
        }
        else if (ddlViewBy.SelectedValue == "D")
        {
            string[] nepdatefrom = txtFromDateNp.Text.Split('/');
            fromdate = PGD.ConvertNepaliTOEnglish(nepdatefrom[0], nepdatefrom[1], nepdatefrom[2]);
            string[] nepdateto = txtToDateNp.Text.Split('/');
            todate = PGD.ConvertNepaliTOEnglish(nepdateto[0], nepdateto[1], nepdateto[2]);

        }
        gridIndAttendance.DataSource = hf.GETATTENDANCE(fromdate, todate, empid);
        gridIndAttendance.DataBind();
    }

    protected void gridIndAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDate = e.Row.FindControl("lblDate") as Label;
            Label lblNPDate = e.Row.FindControl("lblNPDate") as Label;
            Label lblDay = e.Row.FindControl("lblDay") as Label;
            Label lblDayOfWeek = e.Row.FindControl("lblDayOfWeek") as Label;
            Label lblWorkingHour = e.Row.FindControl("lblWorkingHour") as Label;
            Label lblInTime = e.Row.FindControl("lblInTime") as Label;
            Label lblOutTime = e.Row.FindControl("lblOutTime") as Label;
            Label lblDutyFrom = e.Row.FindControl("lblDutyFrom") as Label;
            Label lblDutyTo = e.Row.FindControl("lblDutyTo") as Label;
            Label lblDutyTime = e.Row.FindControl("lblDutyTime") as Label;
            Label lblRemarks = e.Row.FindControl("lblRemarks") as Label;
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lblOverTime = e.Row.FindControl("lblOverTime") as Label;


            string[] dateandtime = lblDate.Text.Split(' ');
            string[] date = dateandtime[0].Split('/');
            int day = Convert.ToInt32(date[0]);
            int month = Convert.ToInt32(date[1]);            
            int year = Convert.ToInt32(date[2]);

            lblDay.Text = day.ToString();
            lblDayOfWeek.Text = new DateTime(year, month, day).DayOfWeek.ToString();

            string[] npdate = PGD.ConvertEnglishToNepali(lblDate.Text);
            lblNPDate.Text = npdate[2] + "/" + npdate[1] + "/" + npdate[0];

            DHEnt = new SET_HR_DUTY_HOUR();
            DHEnt.EMP_ID = lblEmpId.Text;
            DHEnt = (SET_HR_DUTY_HOUR)DHSer.GetSingle(DHEnt);
            if (DHEnt != null && DHEnt.FIXED_DUTY_ROSTER == "1")
            {
                #region
                string[] duty_from = DHEnt.DUTY_IN_TIME.Split(':');
                if (Convert.ToDouble(duty_from[0]) > 12)
                {
                    duty_from[0] = (Convert.ToDouble(duty_from[0]) - 12).ToString();
                    lblDutyFrom.Text = duty_from[0] + ":" + duty_from[1] + ":00 PM";
                }
                else
                {
                    lblDutyFrom.Text = duty_from[0] + ":" + duty_from[1] + ":00 AM";
                }

                string[] duty_to = DHEnt.DUTY_OUT_TIME.Split(':');
                if (Convert.ToDouble(duty_to[0]) > 12)
                {
                    duty_to[0] = (Convert.ToDouble(duty_to[0]) - 12).ToString();
                    lblDutyTo.Text = duty_to[0] + ":" + duty_to[1] + ":00 PM";
                }
                else
                {
                    lblDutyTo.Text = duty_to[0] + ":" + duty_to[1] + ":00 AM";
                }
                #endregion
            }
            else
            {
                DREnt = new SET_HR_DUTY_ROSTER();
                DREnt.EMP_ID = lblEmpId.Text;
                DREnt.DUTY_DATE = lblDate.Text;
                DREnt = (SET_HR_DUTY_ROSTER)DRSer.GetSingle(DREnt);
                if (DREnt != null)
                {
                    try
                    {
                        string[] duty_from = DREnt.DUTY_IN_TIME.Split(':');
                        if (Convert.ToDouble(duty_from[0]) > 12)
                        {
                            duty_from[0] = (Convert.ToDouble(duty_from[0]) - 12).ToString();
                            lblDutyFrom.Text = duty_from[0] + ":" + duty_from[1] + ":00 PM";
                        }
                        else
                        {
                            lblDutyFrom.Text = duty_from[0] + ":" + duty_from[1] + ":00 AM";
                        }

                        string[] duty_to = DREnt.DUTY_OUT_TIME.Split(':');
                        if (Convert.ToDouble(duty_to[0]) > 12)
                        {
                            duty_to[0] = (Convert.ToDouble(duty_to[0]) - 12).ToString();
                            lblDutyTo.Text = duty_to[0] + ":" + duty_to[1] + ":00 PM";
                        }
                        else
                        {
                            lblDutyTo.Text = duty_to[0] + ":" + duty_to[1] + ":00 AM";
                        }
                    }
                    catch // if duty roster is nte set then make it 0
                    {
                        lblDutyFrom.Text = "00:00:00 AM";
                        lblDutyTo.Text = "00:00:00 AM";
                    }
                }

            }


            #region to calculate the total duty time

            double fromsec = 0;
            double tosec = 0;
            string[] fduty = lblDutyFrom.Text.Split(' ');
            string[] fsec = fduty[0].Split(':');
            try
            {
                if (fduty[1] == "PM")
                {
                    fromsec = (Convert.ToDouble(fsec[0]) + 12) * 3600 + Convert.ToDouble(fsec[1]) * 60 + Convert.ToDouble(fsec[2]);
                }
                else
                {
                    fromsec = (Convert.ToDouble(fsec[0]) * 3600 + Convert.ToDouble(fsec[1]) * 60 + Convert.ToDouble(fsec[2]));
                }
            }
            catch
            {
                //fromsec = (Convert.ToDouble(fsec[0]) * 3600 + Convert.ToDouble(fsec[1]) * 60);
            }


            string[] tduty = lblDutyTo.Text.Split(' ');
            string[] tsec = tduty[0].Split(':');
            try
            {
                if (tduty[1] == "PM")
                {
                    tosec = (Convert.ToDouble(tsec[0]) + 12) * 3600 + Convert.ToDouble(tsec[1]) * 60 + Convert.ToDouble(tsec[2]);
                }
                else
                {
                    tosec = (Convert.ToDouble(tsec[0]) * 3600 + Convert.ToDouble(tsec[1]) * 60 + Convert.ToDouble(tsec[2]));
                }
            }
            catch
            {

            }

            TimeSpan dutytime = TimeSpan.FromSeconds(Convert.ToInt32(tosec - fromsec));
            DateTime finaldutytime = DateTime.Today.Add(dutytime);
            lblDutyTime.Text = finaldutytime.ToString("hh:mm:ss");

            #endregion

            if (Convert.ToDateTime(hf.SwapDayMonthFormat(lblDate.Text)) > System.DateTime.Today)
            {
                lblRemarks.Text = "-";
            }

            if (lblInTime.Text == lblOutTime.Text && (lblOutTime.Text != "") && (lblOutTime.Text != ""))
            {
                e.Row.Attributes["style"] = "color:#0000cc";
                lblRemarks.Text = "One time Punch";
            }
            if (lblStatus.Text == "0") // Off Day
            {
                e.Row.Attributes["style"] = "background-color: #cc0000; color:#ffffff";
                lblDutyFrom.Text = "00:00:00";
                lblDutyTo.Text = "00:00:00";
                lblDutyTime.Text = "00:00:00";
                lblDutyFrom.Visible = false;
                lblDutyTo.Visible = false;
                lblDutyTime.Visible = false;
            }
            if (lblStatus.Text == "2") //Absent
            {
                lblRemarks.Text = "Absent";
            }
            if (lblStatus.Text == "3") // Holiday
            {
                e.Row.Attributes["style"] = "background-color: #999900; color:#ffffff";
                lblDutyFrom.Text = "00:00:00";
                lblDutyTo.Text = "00:00:00";
                lblDutyTime.Text = "00:00:00";
                lblDutyFrom.Visible = false;
                lblDutyTo.Visible = false;
                lblDutyTime.Visible = false;
            }
            if (lblStatus.Text == "4") // Leave
            {
                lblRemarks.Text = "Leave";
            }
            if (lblStatus.Text == "5") // Half Day Leave
            {
                lblRemarks.Text = "Half Day Leave";
            }
            
            double workhoursec = 0;
            double dutyhoursec = 0;
            string[] whsec = lblDutyTime.Text.Split(':');
            string[] dhsec = lblWorkingHour.Text.Split(':');
            try
            {
                workhoursec = (Convert.ToDouble(whsec[0]) * 3600 + Convert.ToDouble(whsec[1]) * 60);
            }
            catch
            {

            }
            try
            {
                dutyhoursec = (Convert.ToDouble(dhsec[0]) * 3600 + Convert.ToDouble(dhsec[1]) * 60);
            }
            catch
            {
            }
            if (dutyhoursec > 0)
            {
                TimeSpan diff = TimeSpan.FromSeconds(dutyhoursec - workhoursec);
                // If positive → Overtime, If negative → Undertime
                string prefix = diff.Ticks >= 0 ? "+" : "-";

                // Format with sign
                lblOverTime.Text = prefix + diff.Duration().ToString(@"hh\:mm\:ss");

                if (dutyhoursec < workhoursec)
                {
                    // Set the ForeColor property to Red
                    lblOverTime.ForeColor = System.Drawing.Color.Red;
                    // Set the Font property to bold
                    lblOverTime.Font.Bold = true;
                }


                if (lblOverTime.Text == "00:00:00")
                {
                    lblOverTime.Visible = false;
                }
            }
        }




    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        individual.Visible = false;
        allreport.Visible = true;
    }
    protected void ddlViewBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlViewBy.SelectedValue == "M")
        {
            monthwise.Visible = true;
            daterange.Visible = false;
        }
        else if (ddlViewBy.SelectedValue == "D")
        {
            monthwise.Visible = false;
            daterange.Visible = true;
        }
        else
        {
            monthwise.Visible = false;
            daterange.Visible = false;

        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        if (ddlViewBy.SelectedValue == "M")
        {
            string nepali_lastday = "";
            CultureInfo provider = CultureInfo.InvariantCulture;
            nepali_lastday = PGD.NepaliMonthsLastDay(txtYear.Text, ddlNepaliMonth.SelectedValue).ToString();
            fdate = PGD.ConvertNepaliTOEnglish("1", ddlNepaliMonth.SelectedValue, txtYear.Text);
            tdate = PGD.ConvertNepaliTOEnglish(nepali_lastday, ddlNepaliMonth.SelectedValue, txtYear.Text);
            lblattendancefor.Text = " Attendance for :" + ddlNepaliMonth.SelectedItem.ToString() + " " + txtYear.Text;
        }
        else if (ddlViewBy.SelectedValue == "D")
        {
            string[] nepdatefrom = txtFromDateNp.Text.Split('/');
            fdate = PGD.ConvertNepaliTOEnglish(nepdatefrom[0], nepdatefrom[1], nepdatefrom[2]);
            string[] nepdateto = txtToDateNp.Text.Split('/');
            tdate = PGD.ConvertNepaliTOEnglish(nepdateto[0], nepdateto[1], nepdateto[2]);
        }

        string status = "";
        if (chkActriveEmployees.Checked == true)
            status = "1";
        gridAttendance.DataSource = hf.GETATTENDANCEREPORT(ddlDepartment.SelectedValue, fdate, tdate, status);
        gridAttendance.DataBind();

        ViewState["dirState"] = hf.GETATTENDANCEREPORT(ddlDepartment.SelectedValue, fdate, tdate, status);
        ViewState["sortdr"] = "Asc";
        lblNameCompany.Text = ddlDepartment.SelectedItem.ToString();

    }

    protected void gridAttendance_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dtrslt = (DataTable)ViewState["dirState"];
        if (dtrslt.Rows.Count > 0)
        {
            if (Convert.ToString(ViewState["sortdr"]) == "Asc")
            {
                dtrslt.DefaultView.Sort = e.SortExpression + " Desc";
                ViewState["sortdr"] = "Desc";
            }
            else
            {
                dtrslt.DefaultView.Sort = e.SortExpression + " Asc";
                ViewState["sortdr"] = "Asc";
            }
            gridAttendance.DataSource = dtrslt;
            gridAttendance.DataBind();
        }
    }


}