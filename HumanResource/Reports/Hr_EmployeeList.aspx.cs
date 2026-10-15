using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHelper.Framework;
using System.Collections.Generic;
using System.Linq;
using Entity.Framework;
using Entity.Components;
using Service.Components;
using System.Collections;
using System.IO;


public partial class HumanResource_Reports_HR_EmployeeList : System.Web.UI.Page
{

    string imgfolder;

    OFFICE OEnt = new OFFICE();
    OFFICEService OSer = new OFFICEService();


    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    HR_EMPLOYEES EEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService ESer = new HR_EMPLOYEESService();

    HR_UNIT UEnt = new HR_UNIT();
    HR_UNITService USer = new HR_UNITService();

    District DREnt = new District();
    DistrictService DRSer = new DistrictService();

    ADDRESS ADEnt = new ADDRESS();
    ADDRESSService ADSer = new ADDRESSService();

    ETHNICITY ETHEnt = new ETHNICITY();
    ETHNICITYService ETHSer = new ETHNICITYService();

    BANK BEnt = new BANK();
    BANKService BSer = new BANKService();


    HR_LEVEL HLEnt = new HR_LEVEL();
    HR_LEVELService HLSer = new HR_LEVELService();

    HR_GRADE HGEnt = new HR_GRADE();
    HR_GRADEService HGSer = new HR_GRADEService();

    HR_EMP_HISTORY HEHEnt = new HR_EMP_HISTORY();
    HR_EMP_HISTORYService HEHSer = new HR_EMP_HISTORYService();

    HR_DESIGNATION DGEnt = new HR_DESIGNATION();
    HR_DESIGNATIONService DGSer = new HR_DESIGNATIONService();

    HR_ATTACHMENT_MASTER ATMEnt = new HR_ATTACHMENT_MASTER();
    HR_ATTACHMENT_MASTERService ATMSer = new HR_ATTACHMENT_MASTERService();

    HR_ATTACHMENT ATEnt = new HR_ATTACHMENT();
    HR_ATTACHMENTService ATSer = new HR_ATTACHMENTService();

    HR_EMPLOYEE_TYPE ETEnt = new HR_EMPLOYEE_TYPE();
    HR_EMPLOYEE_TYPEService ETSer = new HR_EMPLOYEE_TYPEService();

    HR_DIVISION HDIVEnt = new HR_DIVISION();
    HR_DIVISIONService HDIVSer = new HR_DIVISIONService();

    HR_UNIT HUEnt = new HR_UNIT();
    HR_UNITService HUSer = new HR_UNITService();

    
    LOCAL_BODY LBEnt = new LOCAL_BODY();
    LOCAL_BODYService LBSer = new LOCAL_BODYService();

    HrEmployeeTermination HETEnt = new HrEmployeeTermination();
    hrEmployeeTerminationService HETSer = new hrEmployeeTerminationService();

    HrEmployeeTerminationType HETTEnt = new HrEmployeeTerminationType();
    HR_EMP_TERMINATION_TYPEService HETTSer = new HR_EMP_TERMINATION_TYPEService();

    HelperFunction hf = new HelperFunction();
    EntityList theEntList = new EntityList();

    static string username;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserProfileEntity userProfileEnt = new UserProfileEntity();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

            LoadData();
            // LoadGridData();
            DetailEmployee.Visible = false;
            print.Visible = false;
            attach.Visible = false;
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

    protected void bttnSearch_Click(object sender, EventArgs e)
    {
        HR_EMPLOYEES EEnt = new HR_EMPLOYEES();

        // Apply status filter
        EEnt.STATUS = ddlStatus.SelectedValue;

        // Apply department filter only if selected
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
        // Fetch all employees based on filters
        IList rawEmployeeList = ESer.GetAll(EEnt);
        List<HR_EMPLOYEES> employeeList = rawEmployeeList.Cast<HR_EMPLOYEES>().ToList();

        // Filter out invalid department entries
        var workingList = employeeList
            .Where(x => x.DEPARTMENTID != "-1")
            .ToList();

        if (workingList.Count > 0)
        {
            GridEmpList.DataSource = workingList;
            GridEmpList.DataBind();
            GridEmpList.Visible = true;

            

            // Hide column 5 if status is inactive
            if (ddlStatus.SelectedValue == "0")
            {
                GridEmpList.Columns[5].Visible = false;
            }

            btnPrintEmp.Visible = true;
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Data not found.");
            GridEmpList.Visible = false;
            btnPrintEmp.Visible = false;
        }
    }


    protected void ImgBttnEdit_Click(object sender, ImageClickEventArgs e)
    {
        string history_id = "";
        GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");

        HEHEnt = new HR_EMP_HISTORY();
        HEHEnt.EMP_ID = lblEmpID.Text;
        HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
        if(HEHEnt != null)
        {
            history_id = HEHEnt.PK_ID;
        }
        Response.Redirect("~/HumanResource/Utilities/Create_Employee.aspx?employeeid=" + lblEmpID.Text + "&historyid=" + history_id);

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

                DPEnt = new HR_DEPARTMENT();
                DPEnt.PK_ID = lblDeptId.Text;
                DPEnt = (HR_DEPARTMENT)DPSer.GetSingle(DPEnt);
                if (DPEnt != null)
                {
                    lblDept.Text = DPEnt.DEP_NAME;

                    if (EEnt.DIVISION != "")
                    {
                        HDIVEnt = new HR_DIVISION();
                        HDIVEnt.PK_ID = EEnt.DIVISION;
                        HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
                        if (HDIVEnt != null)
                        {
                            lblDept.Text = DPEnt.DEP_NAME + "/" + HDIVEnt.DIVISION_NAME;
                        }
                    }

                    if (EEnt.UNIT != "")
                    {
                        UEnt = new HR_UNIT();
                        UEnt.PK_ID = EEnt.UNIT;
                        UEnt = (HR_UNIT)USer.GetSingle(UEnt);
                        if (UEnt != null)
                        {
                            lblDept.Text = DPEnt.DEP_NAME + "/" + HDIVEnt.DIVISION_NAME + "/" + UEnt.UNIT_NAME;

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
    }

    protected void grdPrint_RowDataBound(object sender, GridViewRowEventArgs e)
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
            Label lblEmpIdTemp = e.Row.FindControl("lblEmpIdTemp") as Label;

            EEnt = new HR_EMPLOYEES();
            EEnt.EMPLOYEEID = lblEmpID.Text;
            EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
            if (EEnt != null)
            {
                lblName.Text = EEnt.FULL_NAME;

                DPEnt = new HR_DEPARTMENT();
                DPEnt.PK_ID = lblDeptId.Text;
                DPEnt = (HR_DEPARTMENT)DPSer.GetSingle(DPEnt);
                if (DPEnt != null)
                {
                    lblDept.Text = DPEnt.DEP_NAME;

                    HDIVEnt = new HR_DIVISION();
                    HDIVEnt.PK_ID = EEnt.DIVISION;
                    HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
                    if (HDIVEnt != null)
                    {
                        lblDept.Text = DPEnt.DEP_NAME + "/" + HDIVEnt.DIVISION_NAME;
                    }

                    if (EEnt.UNIT != "")
                    {
                        UEnt = new HR_UNIT();
                        UEnt.PK_ID = EEnt.UNIT;
                        UEnt = (HR_UNIT)USer.GetSingle(UEnt);
                        if (UEnt != null)
                        {
                            lblDept.Text = DPEnt.DEP_NAME + "/" + HDIVEnt.DIVISION_NAME + "/" + UEnt.UNIT_NAME;

                        }
                    }

                }
            }

        }
    }

    protected void ImgBttnView_Click(object sender, ImageClickEventArgs e)
    {
        EmpSearch.Visible = false;
        GridEmpList.Visible = false;

        GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");

        print.Visible = true;
        attach.Visible = true;
        DetailEmployee.Visible = true;
        EEnt = new HR_EMPLOYEES();
        EEnt.EMPLOYEEID = lblEmpID.Text;
        EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
        if (EEnt != null)
        {
            lblPhoto.Text = lblEmpID.Text;
            imgfolder = Server.MapPath(@"~/images/Employee/") + EEnt.EMPLOYEEID + ".jpg";
            if (File.Exists(imgfolder))
            {
                ImgPhoto.ImageUrl = @"~/images/Employee/" + EEnt.EMPLOYEEID + ".jpg";
            }
            else
            {
                if (EEnt.GENDER == "M")
                {
                    ImgPhoto.ImageUrl = @"~/images/Employee/male.jpg";
                }
                else
                    ImgPhoto.ImageUrl = @"~/images/Employee/female.jpg";
            }



            lblEmployeeID.Text = lblEmpID.Text;
            LoadAttachment();
            lblFullName.Text = EEnt.FULL_NAME;
            lblFullName0.Text = EEnt.FULL_NAME;
            lblAbbrevation.Text = EEnt.ABBREVATION;
            DGEnt = new HR_DESIGNATION();
            DGEnt.PK_ID = EEnt.DESIGNATIONID;
            DGEnt = (HR_DESIGNATION)DGSer.GetSingle(DGEnt);
            if (DGEnt != null)
            {
                lblDesig.Text = DGEnt.DESIGNATION_NAME;
            }
            DPEnt = new HR_DEPARTMENT();
            DPEnt.PK_ID = EEnt.DEPARTMENTID;
            DPEnt = (HR_DEPARTMENT)DPSer.GetSingle(DPEnt);
            if (DPEnt != null)
            {
                lblDept.Text = DPEnt.DEP_NAME;
            }

            HLEnt = new HR_LEVEL();
            HLEnt.PK_ID = EEnt.HR_LEVELID;
            HLEnt = (HR_LEVEL)HLSer.GetSingle(HLEnt);
            if (HLEnt != null)
            {
                lblLevel.Text = HLEnt.LEVEL_NAME;
            }
            lblGrade.Text = EEnt.HR_GRADEID;

            DREnt = new District();

            lblPhone.Text = EEnt.PHONE;
            lblMobile.Text = EEnt.MOBILE1;



            lblFatherName.Text = EEnt.FATHERNAME;
            lblGrandFather.Text = EEnt.GRANDFATHERNAME;

            ETEnt = new HR_EMPLOYEE_TYPE();
            ETEnt.PK_ID = EEnt.EMPLOYEETYPE;
            ETEnt = (HR_EMPLOYEE_TYPE)ETSer.GetSingle(ETEnt);
            if (ETEnt != null)
            {
                lblEmpTyp.Text = ETEnt.EMPLOYEE_TYPE;
            }
            lblGender.Text = EEnt.GENDER;
            lblMaritalStatus.Text = EEnt.MARITALSTATUS;
            lblJobTItle.Text = EEnt.JOBTITLE;
            ETHEnt = new ETHNICITY();
            ETHEnt.PK_ID = EEnt.ETHINITICITY_ID;
            ETHEnt = (ETHNICITY)ETHSer.GetSingle(ETHEnt);
            if (ETHEnt != null)
            {
                lblEthnicity.Text = ETHEnt.ETHNICITY_NAME;
            }
            lblBloodGroup.Text = EEnt.BLOODGROUP;
            lblReligion.Text = EEnt.RELIGION;
            ADEnt = new ADDRESS();
            ADEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
            ADEnt.ADDRESS_TYPE = "Both";
            ADEnt = (ADDRESS)ADSer.GetSingle(ADEnt);
            if (ADEnt != null)
            {
                DREnt.PK_ID = ADEnt.DISTRICT;
                DREnt = (District)DRSer.GetSingle(DREnt);
                if (DREnt != null)
                {
                    lblDistrict.Text = DREnt.DISTRICTNAME;
                }

                LBEnt = new LOCAL_BODY();
                LBEnt.PK_ID = ADEnt.VDC_MUNICIPALITY;
                LBEnt = (LOCAL_BODY)LBSer.GetSingle(LBEnt);
                if (LBEnt != null)
                {
                    lblVdc.Text = LBEnt.NAME_ENG;
                }

                lblWard.Text = ADEnt.WARD_NO;
                lblTole.Text = ADEnt.STREET_NAME;

                contactTable.Visible = false;
            }
            else
            {
                ADEnt = new ADDRESS();
                ADEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
                ADEnt.ADDRESS_TYPE = "P";
                ADEnt = (ADDRESS)ADSer.GetSingle(ADEnt);
                if (ADEnt != null)
                {
                    DREnt.PK_ID = ADEnt.DISTRICT;
                    DREnt = (District)DRSer.GetSingle(DREnt);
                    if (DREnt != null)
                    {
                        lblDistrict.Text = DREnt.DISTRICTNAME;
                    }

                    LBEnt = new LOCAL_BODY();
                    LBEnt.PK_ID = ADEnt.VDC_MUNICIPALITY;
                    LBEnt = (LOCAL_BODY)LBSer.GetSingle(LBEnt);
                    if (LBEnt != null)
                    {
                        lblVdc.Text = LBEnt.NAME_ENG;
                    }

                    lblWard.Text = ADEnt.WARD_NO;
                    lblTole.Text = ADEnt.STREET_NAME;
                }
                ADEnt = new ADDRESS();
                ADEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
                ADEnt.ADDRESS_TYPE = "C";
                ADEnt = (ADDRESS)ADSer.GetSingle(ADEnt);
                if (ADEnt != null)
                {
                    DREnt.PK_ID = ADEnt.DISTRICT;
                    DREnt = (District)DRSer.GetSingle(DREnt);
                    if (DREnt != null)
                    {
                        lblCDistrict.Text = DREnt.DISTRICTNAME;
                    }

                    LBEnt = new LOCAL_BODY();
                    LBEnt.PK_ID = ADEnt.VDC_MUNICIPALITY;
                    LBEnt = (LOCAL_BODY)LBSer.GetSingle(LBEnt);
                    if (LBEnt != null)
                    {
                        lblCMunicipality.Text = LBEnt.NAME_ENG;
                    }

                    lblCWard.Text = ADEnt.WARD_NO;
                    lblCTole.Text = ADEnt.STREET_NAME;
                }
            }
            lblNomineeName.Text = EEnt.NOMINEENAME;
            lblNomineeAddress.Text = EEnt.NOMINEEADDRESS;
            lblNomineeRelation.Text = EEnt.NOMINEERELATION;
            lblPFIDNo.Text = EEnt.PFIDNO;
            lblCITNo.Text = EEnt.CITNO;
            lblEmailID.Text = EEnt.EMAIL;
            lblDevName.Text = EEnt.DEVNAGARINAME;
            lblDevName0.Text = EEnt.DEVNAGARINAME;
            lblMotherName.Text = EEnt.MOTHERNAME;

            HUEnt = new HR_UNIT();
            HUEnt.PK_ID = EEnt.UNIT;
            HUEnt = (HR_UNIT)HUSer.GetSingle(HUEnt);
            if (HUEnt != null)
            {
                lblUnit.Text = HUEnt.UNIT_NAME;
            }
            else
            {
                tdUnit.Visible = false;
            }
            HDIVEnt = new HR_DIVISION();
            HDIVEnt.PK_ID = EEnt.DIVISION;
            HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
            if (HDIVEnt != null)
            {
                lblDivision.Text = HDIVEnt.DIVISION_NAME;
            }
            else
            {
                tdDivision.Visible = false;
            }
            BEnt = new BANK();
            BEnt.PK_ID = EEnt.CURRENTBANK;
            BEnt = (BANK)BSer.GetSingle(BEnt);
            if (BEnt != null)
            {
                lblCurrentBank.Text = BEnt.BANK_NAME;
            }
            lblCurrentBankAcc.Text = EEnt.CURRENTBANKACCNO;

            BEnt = new BANK();
            BEnt.PK_ID = EEnt.SECONDBANK;
            BEnt = (BANK)BSer.GetSingle(BEnt);
            if (BEnt != null)
            {
                lblSecondBank.Text = BEnt.BANK_NAME;
            }
            lblSecondBankAcc.Text = EEnt.SECONDBANKACCNO;

            if (EEnt.STATUS != "0")
            {
                chkStatus.Checked = false;
                terminationTable.Visible = false;

            }
            else
            {
                chkStatus.Checked = true;
                terminationTable.Visible = true;
                HETEnt = new HrEmployeeTermination();
                HETEnt.EMPLOYEE_ID = EEnt.EMPLOYEEID;
                HETEnt = (HrEmployeeTermination)HETSer.GetSingle(HETEnt);
                if (HETEnt != null)
                {
                    lblTerminationDate.Text = HETEnt.T_DAY + "/" + HETEnt.T_MONTH + "/" + HETEnt.T_YEAR;
                    lblTerDes.Text = HETEnt.DESCRIPTION;
                    HETTEnt = new HrEmployeeTerminationType();
                    HETTEnt.PK_ID = HETEnt.EMP_TERMINATION_ID;
                    HETTEnt = (HrEmployeeTerminationType)HETTSer.GetSingle(HETTEnt);
                    if (HETTEnt != null)
                    {
                        lblTerRea.Text = HETTEnt.TERMINATION_TYPE;
                    }
                }

            }

            lblRemarks.Text = EEnt.REMARKS;
            lblBirthDate.Text = EEnt.BIRTHDAY + "/" + EEnt.BIRTHMONTH + "/" + EEnt.BIRTHYEAR;
            lblAppointment.Text = EEnt.APPOINTMENTDAY + "/" + EEnt.APPOINTMENTMONTH + "/" + EEnt.APPOINTMENTYEAR;

        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "ERROR");
        }
    }

    protected void bttnCancel_Click(object sender, EventArgs e)
    {
        DetailEmployee.Visible = false;
        EmpSearch.Visible = true;
        GridEmpList.Visible = true;
        print.Visible = false;
        attach.Visible = false;
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }
    protected void ImgCtizenship_Click(object sender, ImageClickEventArgs e)
    {
        string url = "";
        imgfolder = Server.MapPath(@"~/images/Employee_Citizenship/") + lblEmployeeID.Text + "_ctz.pdf";
        if (File.Exists(imgfolder))
        {
            url = @"../images/Employee_Citizenship/" + lblEmployeeID.Text + "_ctz.pdf";

            Response.Write("<script> window.open ('" + url + "','_blank'); </script>");
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "File not Uploaded.");
        }
    }
    protected void ImgCV_Click(object sender, ImageClickEventArgs e)
    {
        string url = "";
        imgfolder = Server.MapPath(@"~/images/Employee_CV/") + lblEmployeeID.Text + "_cv.pdf";
        if (File.Exists(imgfolder))
        {
            url = @"../images/Employee_CV/" + lblEmployeeID.Text + "_cv.pdf";

            Response.Write("<script> window.open ('" + url + "','_blank'); </script>");
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "File not Uploaded.");
        }
    }
    protected void ImgCertificates_Click(object sender, ImageClickEventArgs e)
    {
        string url = "";
        imgfolder = Server.MapPath(@"~/images/Employee_Certificates/") + lblEmployeeID.Text + "_certificate.pdf";
        if (File.Exists(imgfolder))
        {
            url = @"../images/Employee_Certificates/" + lblEmployeeID.Text + "_certificate.pdf";

            Response.Write("<script> window.open ('" + url + "','_blank'); </script>");
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "File not Uploaded.");
        }
    }
    protected void LoadAttachment()
    {
        ATMEnt = new HR_ATTACHMENT_MASTER();
        ATMEnt.STATUS = "1";
        grdAttachment.DataSource = ATMSer.GetAll(ATMEnt);
        grdAttachment.DataBind();
    }
    protected void grdAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblPkId = e.Row.FindControl("lblPkId") as Label;
            Label lblAttDocOf = e.Row.FindControl("lblAttDocOf") as Label;
            Label lblmsgE = e.Row.FindControl("lblmsgE") as Label;
            Label lblmsgI = e.Row.FindControl("lblmsgI") as Label;
            Label lblmsg = e.Row.FindControl("lblmsg") as Label;
            Label lblmsgD = e.Row.FindControl("lblmsgD") as Label;
            Label txtDocumentNum = e.Row.FindControl("txtDocumentNum") as Label;
            Label txtIssueDate = e.Row.FindControl("txtIssueDate") as Label;
            Label txtExpiryDate = e.Row.FindControl("txtExpiryDate") as Label;
            Label txtIssuedBy = e.Row.FindControl("txtIssuedBy") as Label;

            ATMEnt = new HR_ATTACHMENT_MASTER();
            ATMEnt.PK_ID = lblPkId.Text;
            ATMEnt = (HR_ATTACHMENT_MASTER)ATMSer.GetSingle(ATMEnt);
            if (ATMEnt != null)
            {
                if (ATMEnt.DOC_NUM_APPLICABLE == "1")
                {
                    txtDocumentNum.Visible = true;
                    lblmsgD.Visible = false;
                }
                else
                {
                    txtDocumentNum.Visible = false;
                    lblmsgD.Visible = true;
                }
                if (ATMEnt.ISSUE_DATE_APPLICABLE == "1")
                {
                    txtIssueDate.Visible = true;
                    lblmsgI.Visible = false;
                }
                else
                {
                    txtIssueDate.Visible = false;
                    lblmsgI.Visible = true;
                }
                if (ATMEnt.EXP_DATE_APPLICABLE == "1")
                {
                    txtExpiryDate.Visible = true;
                    lblmsgE.Visible = false;
                }
                else
                {
                    txtExpiryDate.Visible = false;
                    lblmsgE.Visible = true;
                }
                if (ATMEnt.ISSUE_BY_APPLICABLE == "1")
                {
                    txtIssuedBy.Visible = true;
                    lblmsg.Visible = false;
                }
                else
                {
                    txtIssuedBy.Visible = false;
                    lblmsg.Visible = true;
                }
            }

            ATEnt = new HR_ATTACHMENT();

            ATEnt.ATTACHMENT_ID = lblPkId.Text;
            ATEnt.ATTACHMENT_OF_ID = lblEmployeeID.Text;

            ATEnt = (HR_ATTACHMENT)ATSer.GetSingle(ATEnt);
            if (ATEnt != null)
            {
                ATEnt = new HR_ATTACHMENT();

                ATEnt.ATTACHMENT_ID = lblPkId.Text;
                ATEnt.ATTACHMENT_OF_ID = lblEmployeeID.Text;
                ATEnt = (HR_ATTACHMENT)ATSer.GetSingle(ATEnt);
                if (ATEnt != null)
                {
                    if (ATEnt.DOCUMENT_NUMBER != "")
                    {
                        txtDocumentNum.Text = ATEnt.DOCUMENT_NUMBER;
                    }
                    else
                    {
                        txtDocumentNum.Text = "Yet to Submit!!";
                    }
                    if (ATEnt.ISSUE_DATE != "")
                    {
                        txtIssueDate.Text = ATEnt.ISSUE_DATE;
                    }
                    else
                    {
                        txtIssueDate.Text = "Yet to Submit!!";
                    }
                    if (ATEnt.EXPIRY_DATE != "")
                    {
                        txtExpiryDate.Text = ATEnt.EXPIRY_DATE;
                    }
                    else
                    {
                        txtExpiryDate.Text = "Yet to Submit!!";
                    }
                    if (ATEnt.ISSUED_BY != "")
                    {
                        txtIssuedBy.Text = ATEnt.ISSUED_BY;
                    }
                    else
                    {
                        txtIssuedBy.Text = "Yet to Submit!!";
                    }
                    Button btnReview = e.Row.FindControl("btnReview") as Button;
                    if (lblAttDocOf.Text != "")
                    {
                        ATEnt = new HR_ATTACHMENT();
                        ATEnt.ATTACHMENT_OF_ID = lblEmployeeID.Text;

                        theEntList = ATSer.GetAll(ATEnt);
                        if (theEntList.Count > 0)
                        {
                            btnReview.Visible = true;
                        }

                    }
                    else
                    {
                        btnReview.Visible = false;
                    }
                }
                else
                {
                    e.Row.Visible = false;
                }

            }
        }
    }
    protected void btnReview_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((Button)sender).Parent.Parent as GridViewRow;
        Label lblAttDoc = gr.FindControl("lblAttDoc") as Label;
        Label lblPkId = gr.FindControl("lblPkId") as Label;
        //Label lblEmployeeID = gr.FindControl("lblEmployeeID") as Label;

        string docName = lblAttDoc.Text.Replace(" ", "_");
        string employeeId = lblEmployeeID.Text;

        string relativePath = "~/images/Attachment_Staff/Emp_" + docName + "/" + employeeId + ".pdf";
        string absolutePath = Server.MapPath(relativePath);

        if (File.Exists(absolutePath))
        {
            string script = "window.open('" + ResolveUrl(relativePath) + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPDF", script, true);
        }
        else
        {
            string msg = "Attachment is missing.";
            string script = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MissingFile", script, true);
        }
    }


    protected void ddlDept_SelectedIndexChanged1(object sender, EventArgs e)
    {
        LoadDivision();
    }

    protected void btnPrintEmp_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage1();", true);
    }

    protected void grdEmpHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblPK_IDG = e.Row.FindControl("lblPK_IDG") as Label;
            Label lblEmpIdG = e.Row.FindControl("lblEmpIdG") as Label;
            Label lblDesignationG = e.Row.FindControl("lblDesignationG") as Label;
            Label lblShowDesignationG = e.Row.FindControl("lblShowDesignationG") as Label;
            Label lblDepartmentG = e.Row.FindControl("lblDepartmentG") as Label;
            Label lblShowDepartmentG = e.Row.FindControl("lblShowDepartmentG") as Label;
            Label lblDivisionG = e.Row.FindControl("lblDivisionG") as Label;
            Label lblShowDivisionG = e.Row.FindControl("lblShowDivisionG") as Label;
            Label lblUnitG = e.Row.FindControl("lblUnitG") as Label;
            Label lblShowUnitG = e.Row.FindControl("lblShowUnitG") as Label;
            Label lblLevelG = e.Row.FindControl("lblLevelG") as Label;
            Label lblShowLevelG = e.Row.FindControl("lblShowLevelG") as Label;
            Label lblGradeG = e.Row.FindControl("lblGradeG") as Label;
            Label lblShowGradeG = e.Row.FindControl("lblShowGradeG") as Label;
            Label lblNepEffDate = e.Row.FindControl("lblNepEffDate") as Label;


            DPEnt = new HR_DEPARTMENT();
            DPEnt.PK_ID = lblDepartmentG.Text;
            DPEnt = (HR_DEPARTMENT)DPSer.GetSingle(DPEnt);
            if (DPEnt != null)
            {
                lblShowDepartmentG.Text = DPEnt.DEP_NAME;
            }


            HDIVEnt = new HR_DIVISION();
            HDIVEnt.PK_ID = lblDivisionG.Text;
            HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
            if (HDIVEnt != null)
            {
                lblShowDivisionG.Text = HDIVEnt.DIVISION_NAME;
            }


            if (lblUnitG != null && !string.IsNullOrWhiteSpace(lblUnitG.Text))
            {
                HUEnt = new HR_UNIT();
                HUEnt.PK_ID = lblUnitG.Text;
                HUEnt = (HR_UNIT)HUSer.GetSingle(HUEnt);
                if (HUEnt != null)
                {
                    lblShowUnitG.Text = HUEnt.UNIT_NAME;
                }
            }

            //Current Position
            DGEnt = new HR_DESIGNATION();
            DGEnt.PK_ID = lblDesignationG.Text;
            DGEnt = (HR_DESIGNATION)DGSer.GetSingle(DGEnt);
            if (DGEnt != null)
            {
                lblShowDesignationG.Text = DGEnt.DESIGNATION_NAME;
            }

            HLEnt = new HR_LEVEL();
            HLEnt.PK_ID = lblLevelG.Text;
            HLEnt = (HR_LEVEL)HLSer.GetSingle(HLEnt);
            if (HLEnt != null)
            {
                lblShowLevelG.Text = HLEnt.LEVEL_NAME;
            }

            HGEnt = new HR_GRADE();
            HGEnt.PK_ID = lblGradeG.Text;
            HGEnt = (HR_GRADE)HGSer.GetSingle(HGEnt);
            if (HGEnt != null)
            {
                lblShowGradeG.Text = HGEnt.GRADE_NAME;
            }


            HEHEnt = new HR_EMP_HISTORY();
            HEHEnt.PK_ID = lblPK_IDG.Text;
            HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
            if (HEHEnt != null)
            {
                lblNepEffDate.Text = HEHEnt.EFFECTIVE_DAY + "/" + HEHEnt.EFFECTIVE_MONTH + "/" + HEHEnt.EFFECTIVE_YEAR;
            }
        }
    }

    protected void btnEmpHistory_Click(object sender, EventArgs e)
    {
        if (btnEmpHistory.Text == "View Emp History")
        {
            loadHistoryGrid(lblEmployeeID.Text); // load and bind data
            tblEmpHistory.Visible = true;
            btnEmpHistory.Text = "Hide Emp History";
        }
        else
        {
            tblEmpHistory.Visible = false;
            btnEmpHistory.Text = "View Emp History";
        }
    }


    protected void loadHistoryGrid(string empId)
    {
        HEHEnt = new HR_EMP_HISTORY();
        HEHEnt.EMP_ID = empId;
        grdEmpHistory.DataSource = HEHSer.GetAll(HEHEnt);
        grdEmpHistory.DataBind();
    }
}