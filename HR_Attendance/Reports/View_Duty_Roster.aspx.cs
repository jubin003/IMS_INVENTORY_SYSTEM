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
using System.IO;

public partial class HR_Attendance_Reports_View_Duty_Roster : System.Web.UI.Page
{
    SET_HR_DUTY_ROSTER SHREnt = new SET_HR_DUTY_ROSTER();
    SET_HR_DUTY_ROSTERService SHRSer = new SET_HR_DUTY_ROSTERService();

    HR_DIVISION DEnt = new HR_DIVISION();
    HR_DIVISIONService DSer = new HR_DIVISIONService();

    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    PhyeGanDate PGD = new PhyeGanDate();
    HelperFunction hf = new HelperFunction();

    PhyeGanDate pgd = new PhyeGanDate();

    ADBS ADBSEnt = new ADBS();
    ADBSService ADBSSer = new ADBSService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDepartment();
            LoadEngMonthDropdown();
            LoadNepMonthDropdown();
            LoadEnglishYears(2000, DateTime.Now.Year + 5);
            LoadNepaliYears(2070, 2090);
            ddlDiv.Items.Insert(0, "Select");
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
        DEnt.DEP_ID = ddlDept.SelectedValue;
        ddlDiv.DataSource = DSer.GetAll(DEnt);
        ddlDiv.DataTextField = "DIVISION_NAME";
        ddlDiv.DataValueField = "PK_ID";

        ddlDiv.DataBind();
        ddlDiv.Items.Insert(0, "Select");
    }
    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDivision();
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
        //LoadGrid();
    }
    private void LoadEngMonthDropdown()
    {
        ddlEngMonth.Items.Clear();

        ddlEngMonth.Items.Add(new ListItem("-- Select Month --", "0"));
        ddlEngMonth.Items.Add(new ListItem("January", "01"));
        ddlEngMonth.Items.Add(new ListItem("February", "02"));
        ddlEngMonth.Items.Add(new ListItem("March", "03"));
        ddlEngMonth.Items.Add(new ListItem("April", "04"));
        ddlEngMonth.Items.Add(new ListItem("May", "05"));
        ddlEngMonth.Items.Add(new ListItem("June", "06"));
        ddlEngMonth.Items.Add(new ListItem("July", "07"));
        ddlEngMonth.Items.Add(new ListItem("August", "08"));
        ddlEngMonth.Items.Add(new ListItem("September", "09"));
        ddlEngMonth.Items.Add(new ListItem("October", "10"));
        ddlEngMonth.Items.Add(new ListItem("November", "11"));
        ddlEngMonth.Items.Add(new ListItem("December", "12"));

        //ddlEngMonth.SelectedValue = DateTime.Now.Month.ToString();
    }
    private void LoadNepMonthDropdown()
    {
        ddlNepMonth.Items.Clear();

        ddlNepMonth.Items.Add(new ListItem("-- Select Month --", "0"));
        ddlNepMonth.Items.Add(new ListItem("Baisakh", "01"));
        ddlNepMonth.Items.Add(new ListItem("Jestha", "02"));
        ddlNepMonth.Items.Add(new ListItem("Ashadh", "03"));
        ddlNepMonth.Items.Add(new ListItem("Shrawan", "04"));
        ddlNepMonth.Items.Add(new ListItem("Bhadra", "05"));
        ddlNepMonth.Items.Add(new ListItem("Ashwin", "06"));
        ddlNepMonth.Items.Add(new ListItem("Kartik", "07"));
        ddlNepMonth.Items.Add(new ListItem("Mangsir", "08"));
        ddlNepMonth.Items.Add(new ListItem("Poush", "09"));
        ddlNepMonth.Items.Add(new ListItem("Magh", "10"));
        ddlNepMonth.Items.Add(new ListItem("Falgun", "11"));
        ddlNepMonth.Items.Add(new ListItem("Chaitra", "12"));

        //ddlEngMonth.SelectedValue = DateTime.Now.Month.ToString();
    }
    protected void loadMonth_Year()
    {
        if (ddlDateType.SelectedValue == "Eng")
        {
            // Convert to int to remove leading zero, then back to string
            int monthNum = int.Parse(ddlEngMonth.SelectedValue);
            lblMonthE.Text = PGD.getEnglishMonth(monthNum.ToString());
            lblYearE.Text = ddlEngYear.SelectedValue;
        }
        else
        {
            int nepMonthNum = int.Parse(ddlNepMonth.SelectedValue);
            lblMonthE.Text = PGD.getNepaliMonth(nepMonthNum.ToString());
            lblYearE.Text = ddlNepYear.SelectedValue;
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
        string nepDate = pgd.GetTodayNepaliDate();
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
    protected void btnView_Click(object sender, EventArgs e)
    {
        loadGrid();
        loadMonth_Year();
        tblDate.Visible = true;

        if (ddlDiv.SelectedValue != "Select")
            lblDivision.Text = ddlDiv.SelectedItem.Text;
        else
            lblDivision.Text = "";
    }
    protected void loadGrid()
    {
        //int month = PGD.NepaliMonthsLastDay(ddlNepYear.SelectedValue, ddlNepMonth.SelectedValue);
        string fromDate = "";
        string toDate = "";

        if (ddlDateType.SelectedValue == "Eng")
        {
            if (ddlEngMonth.SelectedValue == "0")
            {
                HelperFunction.MsgBox(this, this.GetType(), "Please Select the Month");
            }
            else
            {
                fromDate = "01/" + ddlEngMonth.SelectedValue + ddlEngYear.SelectedValue;
                toDate = pgd.EnglishMonthsLastDay(ddlEngYear.SelectedValue, ddlEngMonth.SelectedValue).ToString() + "/" + ddlEngMonth.SelectedValue + "/" + ddlEngYear.SelectedValue;

            }
        }
        else
        {
            if (ddlNepMonth.SelectedValue == "0")
            {
                HelperFunction.MsgBox(this, this.GetType(), "Please Select the Month");
            }
            else
            {
                fromDate = pgd.ConvertNepaliTOEnglish("01", ddlNepMonth.SelectedValue, ddlNepYear.SelectedValue);
                toDate = pgd.ConvertNepaliTOEnglish(PGD.NepaliMonthsLastDay(ddlNepYear.SelectedValue, ddlNepMonth.SelectedValue).ToString(), ddlNepMonth.SelectedValue, ddlNepYear.SelectedValue);
            }
        }
        string dep_id = "";
        string div_id = "";
        if (ddlDept.SelectedValue != "Select")
        {
            dep_id = ddlDept.SelectedValue;
        }
        if (ddlDiv.SelectedValue != "Select")
        {
            div_id = ddlDiv.SelectedValue;
        }

        grdDutyShift.DataSource = hf.getDutyShift(fromDate, toDate, dep_id, div_id);
        grdDutyShift.DataBind();
        btnExport.Visible = true;
    }
    protected void grdDutyShift_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Loop through all days (D1 to D31)
            for (int day = 1; day <= 31; day++)
            {                
                Label lblDay = (Label)e.Row.FindControl("lblD" + day);

                if (lblDay != null)
                {
                    if (lblDay.Text.Trim().ToUpper() == "OFF")
                    {
                        TableCell cell = (TableCell)lblDay.Parent;
                        cell.Attributes["class"] = "print-color off-day-print";
                        cell.Style["background-color"] = "#ffcccc";
                    }
                }
            }
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {

        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename=Duty_Roster" + "_" + lblMonthE.Text + "(" + lblYearE.Text + ")" + ".xls");
        //Response.ContentEncoding = Encoding.UTF8; 
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        divGrid.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.End();
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }
}