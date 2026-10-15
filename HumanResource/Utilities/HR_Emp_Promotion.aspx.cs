using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;
using System.IO;
using Entity.Framework;
using DataHelper.Framework;

public partial class HumanResource_Utilities_HR_Emp_Promotion : System.Web.UI.Page
{
    string imgfolder;
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

    HelperFunction hf = new HelperFunction();
    PhyeGanDate pgd = new PhyeGanDate();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserProfileEntity userProfileEnt = new UserProfileEntity();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

            LoadData();
            LoadDesignation();
            LoadLevel();
            LoadGrade();
            loadMonth_Year();
        }
    }

    protected void LoadData()
    {
        LoadDepartment();
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

    protected void LoadDesignation()
    {
        HDEnt = new HR_DESIGNATION();
        if (lblDepartment.Text != "")
        {
            HDEnt.DEPARTMENT_ID = lblDepartment.Text;
        }
        ddlDesignation.DataSource = HDSer.GetAll(HDEnt);
        ddlDesignation.DataTextField = "DESIGNATION_NAME";
        ddlDesignation.DataValueField = "PK_ID";
        ddlDesignation.DataBind();
        ddlDesignation.Items.Insert(0, "Select");
    }

    protected void LoadLevel()
    {
        HLEnt = new HR_LEVEL();
        ddlLevel.DataSource = HLSer.GetAll(HLEnt);
        ddlLevel.DataTextField = "LEVEL_NAME";
        ddlLevel.DataValueField = "PK_ID";
        ddlLevel.DataBind();
        ddlLevel.Items.Insert(0, "Select");
    }

    protected void LoadGrade()
    {
        HGEnt = new HR_GRADE();
        ddlGrade.DataSource = HGSer.GetAll(HGEnt);
        ddlGrade.DataTextField = "GRADE_NAME";
        ddlGrade.DataValueField = "GRADE_NAME";
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, "Select");
    }
    protected void LoadGridData()
    {
        EEnt = new HR_EMPLOYEES();
        theEntList = new EntityList();
        theEntList = ESer.GetAll(EEnt);

        if (theEntList.Count > 0)
        {
            GridEmpList.DataSource = theEntList;
            GridEmpList.DataBind();
        }

        if (GridEmpList.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            EEnt = new HR_EMPLOYEES();
            a1.Add(EEnt);

            GridEmpList.DataSource = a1;
            GridEmpList.DataBind();
        }

    }

    protected void loadMonth_Year()
    {
        string[] nepaliMonths = new string[]
        {
            "Baisakh", "Jestha", "Ashadh", "Shrawan", "Bhadra", "Ashwin",
            "Kartik", "Mangsir", "Poush", "Magh", "Falgun", "Chaitra"
        };

        for (int i = 0; i < nepaliMonths.Length; i++)
        {
            string value = (i + 1).ToString("D2");
            ddlMonth.Items.Add(new ListItem(nepaliMonths[i], value));
        }

        string[] date = (pgd.GetTodayNepaliDate()).Split('/');
        int currentYear = int.Parse(date[2]);
        ddlYear.Items.Clear();
        for (int year = currentYear; year <= currentYear + 10; year++)
        {
            ddlYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
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


        EmpSearch.Visible = false;
        tblEmpDetail.Visible = true;
        loadEmpDetail(lblEmpID.Text);

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

    protected void loadEmpDetail(string empId)
    {
        EEnt = new HR_EMPLOYEES();
        EEnt.EMPLOYEEID = empId;
        EEnt.STATUS = "1";
        EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
        if (EEnt != null)
        {
            //Emp Details
            lblEmployeeID.Text = EEnt.EMPLOYEEID;
            lblFullName.Text = EEnt.FULL_NAME;

            DPEnt = new HR_DEPARTMENT();
            DPEnt.PK_ID = EEnt.DEPARTMENTID;
            DPEnt = (HR_DEPARTMENT)DPSer.GetSingle(DPEnt);
            if (DPEnt != null)
            {
                lblDepartment.Text = DPEnt.DEP_NAME;
                lblDeptID.Text = DPEnt.PK_ID;
            }

            HDIVEnt = new HR_DIVISION();
            HDIVEnt.PK_ID = EEnt.DIVISION;
            HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
            if (HDIVEnt != null)
            {
                lblDivision.Text = HDIVEnt.DIVISION_NAME;
                lblDivId.Text = HDIVEnt.PK_ID;
            }

            if (EEnt.UNIT != "")
            {
                trUnit.Visible = true;
                HUEnt = new HR_UNIT();
                HUEnt.PK_ID = EEnt.UNIT;
                HUEnt = (HR_UNIT)HUSer.GetSingle(HUEnt);
                if (HUEnt != null)
                {
                    lblUnit.Text = HUEnt.UNIT_NAME;
                    lblUnitId.Text = HUEnt.PK_ID;
                }
            }
            else
            {
                trUnit.Visible = false;
            }


            //Current Position 
            if (EEnt.DESIGNATIONID != "")
            {
                HDEnt = new HR_DESIGNATION();
                HDEnt.PK_ID = EEnt.DESIGNATIONID;
                HDEnt = (HR_DESIGNATION)HDSer.GetSingle(HDEnt);
                if (HDEnt != null)
                {
                    lblDesignation.Text = HDEnt.DESIGNATION_NAME;
                }
            }
            else
            {
                lblDesignation.Text = "";
            }

            if (HLEnt.LEVELS != "")
            {
                HLEnt = new HR_LEVEL();
                HLEnt.PK_ID = EEnt.HR_LEVELID;
                HLEnt = (HR_LEVEL)HLSer.GetSingle(HLEnt);
                if (HLEnt != null)
                {
                    lblLevel.Text = HLEnt.LEVEL_NAME;
                }
            }
            else
            {
                lblLevel.Text = "";
            }

            if (EEnt.HR_GRADEID != "")
            {
                HGEnt = new HR_GRADE();
                HGEnt.PK_ID = EEnt.HR_GRADEID;
                HGEnt = (HR_GRADE)HGSer.GetSingle(HGEnt);
                if (HGEnt != null)
                {
                    lblGrade.Text = HGEnt.GRADE_NAME;
                }
            }
            else
            {
                lblGrade.Text = "";
            }

            // Next Position
            if (EEnt.HR_LEVELID != "")
                ddlLevel.SelectedValue = EEnt.HR_LEVELID;
            else
                ddlLevel.SelectedValue = "Select";

            if (EEnt.HR_GRADEID != "")
                ddlGrade.SelectedValue = EEnt.HR_GRADEID;
            else
                ddlGrade.SelectedValue = "Select";

            if (EEnt.DESIGNATIONID != "")
                ddlDesignation.SelectedValue = EEnt.DESIGNATIONID;
            else
                ddlDesignation.SelectedValue = "Select";



            HEHEnt = new HR_EMP_HISTORY();
            HEHEnt.EMP_ID = EEnt.EMPLOYEEID;
            HEHEnt.STATUS = "1";
            HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
            if (HEHEnt != null)
            {
                // Current Position
                lblEffDate.Text = HEHEnt.EFFECTIVE_DAY + "/" + HEHEnt.EFFECTIVE_MONTH + "/" + HEHEnt.EFFECTIVE_YEAR;

                // Next Position
                string effectiveMonth = HEHEnt.EFFECTIVE_MONTH;

                if (effectiveMonth == "01") ddlMonth.SelectedIndex = 0;
                else if (effectiveMonth == "02") ddlMonth.SelectedIndex = 1;
                else if (effectiveMonth == "03") ddlMonth.SelectedIndex = 2;
                else if (effectiveMonth == "04") ddlMonth.SelectedIndex = 3;
                else if (effectiveMonth == "05") ddlMonth.SelectedIndex = 4;
                else if (effectiveMonth == "06") ddlMonth.SelectedIndex = 5;
                else if (effectiveMonth == "07") ddlMonth.SelectedIndex = 6;
                else if (effectiveMonth == "08") ddlMonth.SelectedIndex = 7;
                else if (effectiveMonth == "09") ddlMonth.SelectedIndex = 8;
                else if (effectiveMonth == "10") ddlMonth.SelectedIndex = 9;
                else if (effectiveMonth == "11") ddlMonth.SelectedIndex = 10;
                else if (effectiveMonth == "12") ddlMonth.SelectedIndex = 11;

                ddlYear.SelectedItem.Text = HEHEnt.EFFECTIVE_YEAR;
                ddlMonth.SelectedValue = HEHEnt.EFFECTIVE_MONTH;
                ddlYear.SelectedValue = HEHEnt.EFFECTIVE_YEAR;

            }

            imgfolder = Server.MapPath(@"~/images/Employee/") + EEnt.EMPLOYEEID + ".jpg";
            if (File.Exists(imgfolder))
            {
                imgEmpPhotoD.ImageUrl = @"~/images/Employee/" + EEnt.EMPLOYEEID + ".jpg";
            }
            else
            {
                if (EEnt.GENDER == "M")
                {
                    imgEmpPhotoD.ImageUrl = @"~/images/Employee/male.jpg";
                }
                else
                    imgEmpPhotoD.ImageUrl = @"~/images/Employee/female.jpg";
            }
        }
    }

    protected void imgBtnPdf_Click(object sender, ImageClickEventArgs e)
    {        
        string empId = lblEmployeeID.Text.Trim();
        HEHEnt = new HR_EMP_HISTORY();
        HEHEnt.EMP_ID = empId;
        //HEHEnt.STATUS = "1";
        HEHEnt.OFFER_LETTER = "1";
        HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
        if (HEHEnt != null)
        {            
            string pdfFileName = "Offer_Letter_" + HEHEnt.PK_ID + ".pdf";
            string relativeUrl = "~/images/Offer_Letter/" + empId + "/" + pdfFileName;

            string physicalPath = Server.MapPath(relativeUrl);

            if (File.Exists(physicalPath))
            {
                string fullUrl = ResolveUrl(relativeUrl);
                string script = "window.open('" + fullUrl + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openPDF", script, true);
            }
            else
            {
                string errorScript = "alert('The requested PDF file was not found.');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "pdfNotFound", errorScript, true);
            }
        }
        else
        {
            string errorScript = "alert('The requested PDF file was not found.');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "pdfNotFound", errorScript, true);
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string history_id = "";
        string date = "01/" + ddlMonth.SelectedValue + "/" + ddlYear.SelectedValue;
        string[] effDate = date.Split('/');
        DistributedTransaction DT = new DistributedTransaction();

        EEnt = new HR_EMPLOYEES();
        EEnt.EMPLOYEEID = lblEmployeeID.Text;
        EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
        if (EEnt != null)
        {
            if (ddlDesignation.SelectedValue != "Select")
                EEnt.DESIGNATIONID = ddlDesignation.SelectedValue;
            else
                EEnt.DESIGNATIONID = "";

            if (ddlLevel.SelectedValue != "Select")
                EEnt.HR_LEVELID = ddlLevel.SelectedValue;
            else
                EEnt.HR_LEVELID = "";

            if (ddlGrade.SelectedValue != "Select")
                EEnt.HR_GRADEID = ddlGrade.SelectedValue;
            else
                EEnt.HR_GRADEID = "";

            ESer.Update(EEnt, DT);
        }

        HEHEnt = new HR_EMP_HISTORY();
        HEHEnt.EMP_ID = lblEmployeeID.Text;
        HEHEnt.STATUS = "1";
        HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
        if (HEHEnt != null)
        {
            HEHEnt.STATUS = "0";
            HEHSer.Update(HEHEnt, DT);
        }

        HEHEnt = new HR_EMP_HISTORY();
        HEHEnt.EMP_ID = lblEmployeeID.Text;
        HEHEnt.HR_DEPARTMENT_ID = lblDeptID.Text;
        HEHEnt.HR_DIVISION_ID = lblDivId.Text;
        HEHEnt.HR_UNIT_ID = lblUnitId.Text;

        if (ddlDesignation.SelectedValue != "Select")
            HEHEnt.HR_DESIGNATION_ID = ddlDesignation.SelectedValue;
        else
            HEHEnt.HR_DESIGNATION_ID = "";

        if (ddlLevel.SelectedValue != "Select")
            HEHEnt.HR_LEVEL_ID = ddlLevel.SelectedValue;
        else
            HEHEnt.HR_LEVEL_ID = "";

        if (ddlGrade.SelectedValue != "Select")
            HEHEnt.HR_GRADE_ID = ddlGrade.SelectedValue;
        else
            HEHEnt.HR_GRADE_ID = "";

        HEHEnt.EFFECTIVE_DATE = pgd.ConvertNepaliTOEnglish(effDate[0], effDate[1], effDate[2]);
        HEHEnt.EFFECTIVE_DAY = effDate[0];
        HEHEnt.EFFECTIVE_MONTH = effDate[1];
        HEHEnt.EFFECTIVE_YEAR = effDate[2];
        HEHEnt.OFFER_LETTER = "1";
        HEHEnt.STATUS = "1";

        history_id = HEHSer.Insert(HEHEnt, DT).ToString();

        if (DT.HAPPY)
        {
            DT.Commit();
            string contract;
            try
            {
                string empId = lblEmployeeID.Text.Trim();

                string empFolderPath = Server.MapPath("~/images/Offer_Letter/" + empId + "/");

                if (!Directory.Exists(empFolderPath))
                {
                    Directory.CreateDirectory(empFolderPath);
                }

                contract = "Offer_Letter_" + history_id + ".pdf";
                if (fileContract.HasFile)
                {
                    string fileExtension = Path.GetExtension(fileContract.FileName).ToLower();
                    string contentType = fileContract.PostedFile.ContentType.ToLower();

                    if (fileExtension == ".pdf" && contentType == "application/pdf")
                    {
                        string fullPath = Path.Combine(empFolderPath, contract);
                        fileContract.SaveAs(fullPath);
                    }
                    else
                    {
                        HelperFunction.MsgBox(this, this.GetType(), "Only PDF files are allowed.");
                    }
                }
            }
            catch (Exception ex)
            {
            }
            HelperFunction.MsgBox(this, this.GetType(), "Updated !!");
            loadEmpDetail(lblEmployeeID.Text);
        }
        else
        {
            DT.Abort();
            HelperFunction.MsgBox(this, this.GetType(), "Something went wrong !!");
        }
        DT.Dispose();

    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lblEmployeeID.Text = "";
        tblEmpDetail.Visible = false;
        EmpSearch.Visible = true;
    }
}