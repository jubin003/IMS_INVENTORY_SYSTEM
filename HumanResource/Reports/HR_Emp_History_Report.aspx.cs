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


public partial class HumanResource_Reports_HR_Emp_History_Report : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserProfileEntity userProfileEnt = new UserProfileEntity();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

            LoadData();
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

    protected void btnView_Click(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((Button)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");

        EmpSearch.Visible = false;
        tblEmpDetail.Visible = true;
        loadEmpDetail(lblEmpID.Text);
        loadEmpJoinDetails(lblEmpID.Text);
        loadHistoryGrid(lblEmpID.Text);
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
        if (HEHEnt != null)
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
            }

            HDIVEnt = new HR_DIVISION();
            HDIVEnt.PK_ID = EEnt.DIVISION;
            HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
            if (HDIVEnt != null)
            {
                lblDivision.Text = HDIVEnt.DIVISION_NAME;
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
                }
            }
            else
            {
                trUnit.Visible = false;
            }


            //Current Position
            HDEnt = new HR_DESIGNATION();
            HDEnt.PK_ID = EEnt.DESIGNATIONID;
            HDEnt = (HR_DESIGNATION)HDSer.GetSingle(HDEnt);
            if (HDEnt != null)
            {
                lblDesignation.Text = HDEnt.DESIGNATION_NAME;
            }

            if (EEnt.HR_LEVELID != "")
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


            HEHEnt = new HR_EMP_HISTORY();
            HEHEnt.EMP_ID = EEnt.EMPLOYEEID;
            HEHEnt.STATUS = "1";
            HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
            if (HEHEnt != null)
            {
                // Current Position
                lblEffDate.Text = HEHEnt.EFFECTIVE_DAY + "/" + HEHEnt.EFFECTIVE_MONTH + "/" + HEHEnt.EFFECTIVE_YEAR;
            }
            else
            {
                lblEffDate.Text = EEnt.APPOINTMENTDAY + "/" + EEnt.APPOINTMENTMONTH + "/" + EEnt.APPOINTMENTYEAR;
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

    protected void loadEmpJoinDetails(string empId)
    {
        HEHEnt = new HR_EMP_HISTORY();
        HEHEnt.EMP_ID = empId;
        HEHEnt.STATUS = "0";
        HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
        if (HEHEnt != null)
        {
            DPEnt = new HR_DEPARTMENT();
            DPEnt.PK_ID = HEHEnt.HR_DEPARTMENT_ID;
            DPEnt = (HR_DEPARTMENT)DPSer.GetSingle(DPEnt);
            if (DPEnt != null)
            {
                lblJnDepartment.Text = DPEnt.DEP_NAME;
            }

            HDIVEnt = new HR_DIVISION();
            HDIVEnt.PK_ID = HEHEnt.HR_DIVISION_ID;
            HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
            if (HDIVEnt != null)
            {
                lblJnDivision.Text = HDIVEnt.DIVISION_NAME;
            }

            if (EEnt.UNIT != "")
            {
                trJnUnit.Visible = true;
                HUEnt = new HR_UNIT();
                HUEnt.PK_ID = HEHEnt.HR_UNIT_ID;
                HUEnt = (HR_UNIT)HUSer.GetSingle(HUEnt);
                if (HUEnt != null)
                {
                    lblJnUnit.Text = HUEnt.UNIT_NAME;
                }
            }
            else
            {
                trJnUnit.Visible = false;
            }

            HDEnt = new HR_DESIGNATION();
            HDEnt.PK_ID = HEHEnt.HR_DESIGNATION_ID;
            HDEnt = (HR_DESIGNATION)HDSer.GetSingle(HDEnt);
            if (HDEnt != null)
            {
                lblJnDesignation.Text = HDEnt.DESIGNATION_NAME;
            }


            HLEnt = new HR_LEVEL();
            HLEnt.PK_ID = HEHEnt.HR_LEVEL_ID;
            HLEnt = (HR_LEVEL)HLSer.GetSingle(HLEnt);
            if (HLEnt != null)
            {
                lblJnLevel.Text = HLEnt.LEVEL_NAME;
            }

            HGEnt = new HR_GRADE();
            HGEnt.PK_ID = HEHEnt.HR_GRADE_ID;
            HGEnt = (HR_GRADE)HGSer.GetSingle(HGEnt);
            if (HGEnt != null)
            {
                lblJnGrade.Text = HGEnt.GRADE_NAME;
            }

            lblJnEffDate.Text = HEHEnt.EFFECTIVE_DAY + "/" + HEHEnt.EFFECTIVE_MONTH + "/" + HEHEnt.EFFECTIVE_YEAR;
        }
        else
        {
            EEnt = new HR_EMPLOYEES();
            EEnt.EMPLOYEEID = empId;
            EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
            if (EEnt != null)
            {
                DPEnt = new HR_DEPARTMENT();
                DPEnt.PK_ID = EEnt.DEPARTMENTID;
                DPEnt = (HR_DEPARTMENT)DPSer.GetSingle(DPEnt);
                if (DPEnt != null)
                {
                    lblJnDepartment.Text = DPEnt.DEP_NAME;
                }

                HDIVEnt = new HR_DIVISION();
                HDIVEnt.PK_ID = EEnt.DIVISION;
                HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
                if (HDIVEnt != null)
                {
                    lblJnDivision.Text = HDIVEnt.DIVISION_NAME;
                }

                if (EEnt.UNIT != "")
                {
                    trJnUnit.Visible = true;
                    HUEnt = new HR_UNIT();
                    HUEnt.PK_ID = EEnt.UNIT;
                    HUEnt = (HR_UNIT)HUSer.GetSingle(HUEnt);
                    if (HUEnt != null)
                    {
                        lblJnUnit.Text = HUEnt.UNIT_NAME;
                    }
                }
                else
                {
                    trJnUnit.Visible = false;
                }

                HDEnt = new HR_DESIGNATION();
                HDEnt.PK_ID = EEnt.DESIGNATIONID;
                HDEnt = (HR_DESIGNATION)HDSer.GetSingle(HDEnt);
                if (HDEnt != null)
                {
                    lblJnDesignation.Text = HDEnt.DESIGNATION_NAME;
                }

                if (EEnt.HR_LEVELID != "")
                {
                    HLEnt = new HR_LEVEL();
                    HLEnt.PK_ID = EEnt.HR_LEVELID;
                    HLEnt = (HR_LEVEL)HLSer.GetSingle(HLEnt);
                    if (HLEnt != null)
                    {
                        lblJnLevel.Text = HLEnt.LEVEL_NAME;
                    }
                }
                else
                {
                    lblJnLevel.Text = "";
                }


                if (EEnt.HR_GRADEID != "")
                {
                    HGEnt = new HR_GRADE();
                    HGEnt.PK_ID = EEnt.HR_GRADEID;
                    HGEnt = (HR_GRADE)HGSer.GetSingle(HGEnt);
                    if (HGEnt != null)
                    {
                        lblJnGrade.Text = HGEnt.GRADE_NAME;
                    }
                }
                else
                {
                    lblJnGrade.Text = "";
                }


                HEHEnt = new HR_EMP_HISTORY();
                HEHEnt.EMP_ID = EEnt.EMPLOYEEID;
                HEHEnt.STATUS = "1";
                HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
                if (HEHEnt != null)
                {
                    lblJnEffDate.Text = HEHEnt.EFFECTIVE_DAY + "/" + HEHEnt.EFFECTIVE_MONTH + "/" + HEHEnt.EFFECTIVE_YEAR;
                }
                else
                {
                    lblJnEffDate.Text = EEnt.APPOINTMENTDAY + "/" + EEnt.APPOINTMENTMONTH + "/" + EEnt.APPOINTMENTYEAR;
                }
            }
        }

    }

    protected void loadHistoryGrid(string empId)
    {
        HEHEnt = new HR_EMP_HISTORY();
        HEHEnt.EMP_ID = empId;
        grdEmpHistory.DataSource = HEHSer.GetAll(HEHEnt);
        grdEmpHistory.DataBind();
    }

    protected void grdEmpHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblPK_IDG = e.Row.FindControl("lblPK_IDG") as Label;
            Label lblEmpIdG = e.Row.FindControl("lblEmpIdG") as Label;
            Label lblDesignationG = e.Row.FindControl("lblDesignationG") as Label;
            Label lblDepartmentG = e.Row.FindControl("lblDepartmentG") as Label;
            Label lblDivisionG = e.Row.FindControl("lblDivisionG") as Label;
            Label lblUnitG = e.Row.FindControl("lblUnitG") as Label;
            Label lblWrkPlaceG = e.Row.FindControl("lblWrkPlaceG") as Label;
            Label lblLevelG = e.Row.FindControl("lblLevelG") as Label;
            Label lblShowLevelG = e.Row.FindControl("lblShowLevelG") as Label;
            Label lblGradeG = e.Row.FindControl("lblGradeG") as Label;
            Label lblShowGradeG = e.Row.FindControl("lblShowGradeG") as Label;
            Label lblNepEffDate = e.Row.FindControl("lblNepEffDate") as Label;

            ImageButton imgBtnOfferLetter = e.Row.FindControl("imgBtnOfferLetter") as ImageButton;
            ImageButton imgBtnAgreement = e.Row.FindControl("imgBtnAgreement") as ImageButton;
            ImageButton imgBtnPdf = e.Row.FindControl("imgBtnPdf") as ImageButton;


            DPEnt = new HR_DEPARTMENT();
            DPEnt.PK_ID = lblDepartmentG.Text;
            DPEnt = (HR_DEPARTMENT)DPSer.GetSingle(DPEnt);
            if (DPEnt != null)
            {
                lblWrkPlaceG.Text = DPEnt.DEP_NAME;

                if (EEnt.DIVISION != "")
                {
                    HDIVEnt = new HR_DIVISION();
                    HDIVEnt.PK_ID = lblDivisionG.Text;
                    HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
                    if (HDIVEnt != null)
                    {
                        lblWrkPlaceG.Text = DPEnt.DEP_NAME + "/" + HDIVEnt.DIVISION_NAME;
                    }
                }

                if (lblDesignationG.Text != "")
                {
                    HDEnt = new HR_DESIGNATION();
                    HDEnt.PK_ID = lblDesignationG.Text;
                    HDEnt = (HR_DESIGNATION)HDSer.GetSingle(HDEnt);
                    if (HDEnt != null)
                    {
                        lblWrkPlaceG.Text = DPEnt.DEP_NAME + "/" + HDIVEnt.DIVISION_NAME + "/" + HDEnt.DESIGNATION_NAME;
                    }
                }

                if (lblUnitG.Text != "")
                {
                    HUEnt = new HR_UNIT();
                    HUEnt.PK_ID = lblUnitG.Text;
                    HUEnt = (HR_UNIT)HUSer.GetSingle(HUEnt);
                    if (HUEnt != null)
                    {
                        lblWrkPlaceG.Text = DPEnt.DEP_NAME + "/" + HDIVEnt.DIVISION_NAME + "/" + HDEnt.DESIGNATION_NAME + "/" + HUEnt.UNIT_NAME;

                    }
                }

            }
            if (lblLevelG.Text != "")
            {
                HLEnt = new HR_LEVEL();
                HLEnt.PK_ID = lblLevelG.Text;
                HLEnt = (HR_LEVEL)HLSer.GetSingle(HLEnt);
                if (HLEnt != null)
                {
                    lblShowLevelG.Text = HLEnt.LEVEL_NAME;
                }
            }
            else
            {
                lblShowLevelG.Text = "";
            }

            if (lblGradeG.Text != "")
            {
                HGEnt = new HR_GRADE();
                HGEnt.PK_ID = lblGradeG.Text;
                HGEnt = (HR_GRADE)HGSer.GetSingle(HGEnt);
                if (HGEnt != null)
                {
                    lblShowGradeG.Text = HGEnt.GRADE_NAME;
                }
            }
            else
            {
                lblShowGradeG.Text = "";
            }


            HEHEnt = new HR_EMP_HISTORY();
            HEHEnt.PK_ID = lblPK_IDG.Text;
            HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
            if (HEHEnt != null)
            {
                lblNepEffDate.Text = HEHEnt.EFFECTIVE_DAY + "/" + HEHEnt.EFFECTIVE_MONTH + "/" + HEHEnt.EFFECTIVE_YEAR;

                if (HEHEnt.OFFER_LETTER == "1")
                {
                    imgBtnOfferLetter.Visible = true;
                }
                else
                {
                    imgBtnOfferLetter.Visible = false;
                }

                if (HEHEnt.AGREEMENT == "1")
                {
                    imgBtnAgreement.Visible = true;
                }
                else
                {
                    imgBtnAgreement.Visible = false;
                }

                if (HEHEnt.CONTRACT == "1")
                {
                    imgBtnPdf.Visible = true;
                }
                else
                {
                    imgBtnPdf.Visible = false;
                }
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblEmployeeID.Text = "";
        tblEmpDetail.Visible = false;
        EmpSearch.Visible = true;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void grdEmpHistory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("View1"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_IDG = gr.FindControl("lblPK_IDG") as Label;

            string empId = lblEmployeeID.Text.Trim();

            string pdfFileName = "Offer_Letter_" + lblPK_IDG.Text + ".pdf";
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

        else if (e.CommandName.Equals("View2"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_IDG = gr.FindControl("lblPK_IDG") as Label;

            string empId = lblEmployeeID.Text.Trim();

            string pdfFileName = "Agreement_" + lblPK_IDG.Text + ".pdf";
            string relativeUrl = "~/images/Agreement/" + empId + "/" + pdfFileName;

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

        else if (e.CommandName.Equals("View3"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_IDG = gr.FindControl("lblPK_IDG") as Label;

            string empId = lblEmployeeID.Text.Trim();

            string pdfFileName = "Contract_" + lblPK_IDG.Text + ".pdf";
            string relativeUrl = "~/images/Contracts/" + empId + "/" + pdfFileName;

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
    }
}