using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Entity.Framework;
using Entity.Components;
using Service.Components;
using System.IO;

using DataHelper.Framework;
using DataAccess.Framework;
using System.Collections;
using System.Data;


public partial class HumanResource_Utilities_Basic_Employee : System.Web.UI.Page
{
    
    GL_SUB_ACCOUNT GSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GSASer = new GL_SUB_ACCOUNTService();

    GL_ACCOUNT GLAEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLASer = new GL_ACCOUNTService();

    HR_EMPLOYEES EEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService ESer = new HR_EMPLOYEESService();

    PROVINCE PEnt = new PROVINCE();
    PROVINCEService PSer = new PROVINCEService();

    District DSEnt = new District();
    DistrictService DSSer = new DistrictService();

    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    HR_DESIGNATION DTEnt = new HR_DESIGNATION();
    HR_DESIGNATIONService DTSer = new HR_DESIGNATIONService();

    COUNTRY CEnt = new COUNTRY();
    COUNTRYService CSer = new COUNTRYService();

    ADDRESS AEnt = new ADDRESS();
    ADDRESSService ASrv = new ADDRESSService();

    BANK BEnt = new BANK();
    BANKService BSer = new BANKService();

    HR_EMP_ACC_DETAIL EADEnt = new HR_EMP_ACC_DETAIL();
    HR_EMP_ACC_DETAILService EADSrv = new HR_EMP_ACC_DETAILService();

    HR_EMPLOYEE_TYPE ETEnt = new HR_EMPLOYEE_TYPE();
    HR_EMPLOYEE_TYPEService ETSer = new HR_EMPLOYEE_TYPEService();

    ETHNICITY ETHEnt = new ETHNICITY();
    ETHNICITYService ETHSer = new ETHNICITYService();

    HR_DIVISION HDIVEnt = new HR_DIVISION();
    HR_DIVISIONService HDIVSer = new HR_DIVISIONService();

    HR_EMP_HISTORY HEHEnt = new HR_EMP_HISTORY();
    HR_EMP_HISTORYService HEHSer = new HR_EMP_HISTORYService();

    HR_UNIT HUEnt = new HR_UNIT();
    HR_UNITService HUSer = new HR_UNITService();

    HR_LEVEL HLEnt = new HR_LEVEL();
    HR_LEVELService HLSer = new HR_LEVELService();

    HR_EMP_CONTRACT HECEnt = new HR_EMP_CONTRACT();
    HR_EMP_CONTRACTService HECSer = new HR_EMP_CONTRACTService();

    HR_GRADE HGEnt = new HR_GRADE();
    HR_GRADEService HGSer = new HR_GRADEService();

    HR_ATTACHMENT_MASTER ATMEnt = new HR_ATTACHMENT_MASTER();
    HR_ATTACHMENT_MASTERService ATMSer = new HR_ATTACHMENT_MASTERService();

    HR_ATTACHMENT ATEnt = new HR_ATTACHMENT();
    HR_ATTACHMENTService ATSer = new HR_ATTACHMENTService();

    LOCAL_BODY LBEnt = new LOCAL_BODY();
    LOCAL_BODYService LBSer = new LOCAL_BODYService();

    PhyeGanDate pgd = new PhyeGanDate();

    HelperFunction hf = new HelperFunction();
    HumanResourceFunction hr = new HumanResourceFunction();
    EntityList theList = new EntityList();

    string emloyee_id;
    string his_id; //used while giving names to the files at the time of update
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            notneededDiv.Visible = false;
            LoadLevel();
            LoadGrade();
            LoadPADistric();
            LoadTADistric();
            LoadBank();
            LoadDepartment();
            LoadEmpType();
            LoadEthnicity();
            LoadBank2();

            LoadPayables();
            LoadAdvance();
            txtDateAd.Text = pgd.GetTodayNepaliDate();
            txtDateBs.Text = pgd.GetTodayNepaliDate();
            txtStartDate.Text = pgd.GetTodayNepaliDate();
            txtEndDate.Text = pgd.GetTodayNepaliDate();
            txtProbDate.Text = pgd.GetTodayNepaliDate();
            txtAppointDate.Text = pgd.GetTodayNepaliDate();

            string employees = "";
            his_id = (Request.QueryString.Get("historyid"));
            employees = (Request.QueryString.Get("employeeid"));
            emloyee_id = employees;
            if (employees != null)
            {
                LoadEmployeesDetail(employees);
                btnBack.Visible = true;
            }
            else
            {
                txtIdNo.Text = hr.getmaxEmpid();
                LoadAttachment();
                btnBack.Visible = false;
            }

            foreach (ListItem item in chkPay.Items)
            {
                item.Selected = true;
            }
            foreach (ListItem item in chkAdvance.Items)
            {
                item.Selected = true;
            }
        }
    }

    protected void LoadLocalPABodyType()
    {
        LBEnt = new LOCAL_BODY();
        LBEnt.DISTRICT_CODE = ddlPA_DISTRICT.SelectedValue;
        ddlPAVDC.DataSource = LBSer.GetAll(LBEnt);
        ddlPAVDC.DataTextField = "NAME_ENG";
        ddlPAVDC.DataValueField = "PK_ID";
        ddlPAVDC.DataBind();
        ddlPAVDC.Items.Insert(0, "Select");
    }
    protected void LoadLocalTABodyType()
    {
        LBEnt = new LOCAL_BODY();
        LBEnt.DISTRICT_CODE = ddlTA_DISTRICT.SelectedValue;
        ddlTAVDC.DataSource = LBSer.GetAll(LBEnt);
        ddlTAVDC.DataTextField = "NAME_ENG";
        ddlTAVDC.DataValueField = "PK_ID";
        ddlTAVDC.DataBind();
        //ddlTAVDC.Items.Insert(0, "Select");
    }
    protected void LoadDepartment()
    {
        DPEnt = new HR_DEPARTMENT();
        DPEnt.STATUS = "1";
        ddlDept.DataSource = DPSer.GetAll(DPEnt);
        ddlDept.DataTextField = "DEP_NAME";
        ddlDept.DataValueField = "PK_ID";
        ddlDept.DataBind();
        //ddlDept.Items.Insert(0, "Select");
        LoadDivision();
        LoadUnit();
        LoadDesignation();
        LoadLevel();
    }
    protected void LoadDivision()
    {
        HDIVEnt = new HR_DIVISION();
        HDIVEnt.STATUS = "1";
        HDIVEnt.DEP_ID = ddlDept.SelectedValue;
        ddlDivision.DataSource = HDIVSer.GetAll(HDIVEnt);
        ddlDivision.DataTextField = "DIVISION_NAME";
        ddlDivision.DataValueField = "PK_ID";
        ddlDivision.DataBind();
        //ddlDivision.Items.Insert(0, "Select");
        if (ddlDivision.Items.Count <= 1)
        {
            ddlUnit.Items.Clear();
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
        //ddlUnit.Items.Insert(0, "Select");
    }
    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUnit();

    }
    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDesignation();
        LoadDivision();
    }
    protected void LoadDesignation()
    {
        DTEnt = new HR_DESIGNATION();
        DTEnt.DEPARTMENT_ID = ddlDept.SelectedValue;
        ddlDesignation.DataSource = DTSer.GetAll(DTEnt);
        ddlDesignation.DataTextField = "DESIGNATION_NAME";
        ddlDesignation.DataValueField = "PK_ID";
        ddlDesignation.DataBind();
        //ddlDesignation.Items.Insert(0, "Select");
    }

    protected void LoadTADistric()
    {
        DSEnt = new District();
        ddlTA_DISTRICT.DataSource = DSSer.GetAll(DSEnt);
        ddlTA_DISTRICT.DataTextField = "DISTRICTNAME";
        ddlTA_DISTRICT.DataValueField = "PK_ID";
        ddlTA_DISTRICT.DataBind();
        ddlTA_DISTRICT.Items.Insert(0, "Select");
    }
    protected void LoadPADistric()
    {
        DSEnt = new District();
        ddlPA_DISTRICT.DataSource = DSSer.GetAll(DSEnt);
        ddlPA_DISTRICT.DataTextField = "DISTRICTNAME";
        ddlPA_DISTRICT.DataValueField = "PK_ID";
        ddlPA_DISTRICT.DataBind();
        ddlPA_DISTRICT.Items.Insert(0, "Select");

    }

    protected void LoadLevel()
    {
        HLEnt = new HR_LEVEL();
        ddlLevel.DataSource = HLSer.GetAll(HLEnt);
        ddlLevel.DataTextField = "LEVEL_NAME";
        ddlLevel.DataValueField = "PK_ID";
        ddlLevel.DataBind();
        //ddlLevel.Items.Insert(0, "Select");
    }

    protected void LoadGrade()
    {
        HGEnt = new HR_GRADE();
        ddlGrade.DataSource = HGSer.GetAll(HGEnt);
        ddlGrade.DataTextField = "GRADE_NAME";
        ddlGrade.DataValueField = "GRADE_NAME";
        ddlGrade.DataBind();
    }
    protected void LoadAttachment()
    {
        ATMEnt = new HR_ATTACHMENT_MASTER();
        ATMEnt.STATUS = "1";
        grdAttachment.DataSource = ATMSer.GetAll(ATMEnt);
        grdAttachment.DataBind();
    }
    protected void LoadBank()
    {
        BEnt = new BANK();
        BEnt.STATUS = "1";
        ddlBankName.DataSource = BSer.GetAll(BEnt);
        ddlBankName.DataTextField = "BANK_NAME";
        ddlBankName.DataValueField = "PK_ID";
        ddlBankName.DataBind();
    }

    protected void LoadBank2()
    {
        BEnt = new BANK();
        BEnt.STATUS = "1";
        ddBankName2.DataSource = BSer.GetAll(BEnt);
        ddBankName2.DataTextField = "BANK_NAME";
        ddBankName2.DataValueField = "PK_ID";
        ddBankName2.DataBind();
    }


    protected void LoadEmpType()
    {
        ETEnt = new HR_EMPLOYEE_TYPE();
        ETEnt.STATUS = "1";
        ddlEmployeeType.DataSource = ETSer.GetAll(ETEnt);
        ddlEmployeeType.DataTextField = "EMPLOYEE_TYPE";
        ddlEmployeeType.DataValueField = "PK_ID";
        ddlEmployeeType.DataBind();
    }

    protected void LoadEthnicity()
    {
        ETHEnt = new ETHNICITY();
        ddlEthnicity.DataSource = ETHSer.GetAll(ETHEnt);
        ddlEthnicity.DataTextField = "ETHNICITY_NAME";
        ddlEthnicity.DataValueField = "PK_ID";
        ddlEthnicity.DataBind();
    }

    protected void BtnCalAD_Click(object sender, EventArgs e)
    {
        GetEnglishDate();
    }
    protected void BtnCalBS_Click(object sender, EventArgs e)
    {
        GetNepaliDate();
    }


    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/humanresource/utilities/Hr_Employee.aspx");
    }
    protected void GetEnglishDate()
    {
        if (txtDateBs.Text != "")
        {
            try
            {
                string[] NepDate = txtDateBs.Text.Split('/');
                txtDateAd.Text = pgd.ConvertNepaliTOEnglish(NepDate[0], NepDate[1], NepDate[2]);
            }
            catch
            {
                HelperFunction.MsgBox(this, this.GetType(), "Please Enter Correct Date");
            }

        }

    }
    protected void GetNepaliDate()
    {
        if (txtDateAd.Text != "")
        {
            try
            {
                string[] NepDate = pgd.GetNepaliDateFromEnglish(txtDateAd.Text,"dd/mm/yyyy").Split('/');
                txtDateBs.Text = NepDate[0] + "/" + NepDate[1] + "/" + NepDate[2];
            }
            catch
            {
                HelperFunction.MsgBox(this, this.GetType(), "Please Enter Correct Date");
            }
        }

    }
    protected void LoadPayables()
    {
        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_MASTER_CODE = "0401";
        GLAEnt.STATUS = "1";
        chkPay.DataSource = GLASer.GetAll(GLAEnt);
        chkPay.DataTextField = "GL_NAME";
        chkPay.DataValueField = "GL_CODE";
        chkPay.DataBind();
    }
    protected void LoadPayableCheck()
    {
        foreach (ListItem item in chkPay.Items)
        {
            GSAEnt = new GL_SUB_ACCOUNT();
            GSAEnt.SUB_GL_CODE = "EMP" + txtIdNo.Text;
            GSAEnt.GL_CODE = item.Value;
            GSAEnt.STATUS = "1";
            GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
            if (GSAEnt != null)
            {
                item.Selected = true;
            }
            else
            {
                item.Selected = false;
            }
        }
    }

    protected void LoadAdvance()
    {
        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_CODE = "010401";
        GLAEnt.STATUS = "1";
        chkAdvance.DataSource = GLASer.GetAll(GLAEnt);
        chkAdvance.DataTextField = "GL_NAME";
        chkAdvance.DataValueField = "GL_CODE";
        chkAdvance.DataBind();
    }

    protected void LoadAdvanceCheck()
    {
        foreach (ListItem item in chkAdvance.Items)
        {
            GSAEnt = new GL_SUB_ACCOUNT();
            GSAEnt.SUB_GL_CODE = "EMP" + txtIdNo.Text;
            GSAEnt.GL_CODE = item.Value;
            GSAEnt.STATUS = "1";
            GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
            if (GSAEnt != null)
            {
                item.Selected = true;
            }
            else
            {
                item.Selected = false;
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        EEnt = new HR_EMPLOYEES();
        EEnt.EMPLOYEEID = txtIdNo.Text;
        EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
        if (EEnt != null && txtIdNo.Text != "")
        {
            updateEmployee();
        }
        else
        {
            saveEmployee();

        }
        //Response.Redirect("hr_employee.aspx");
    }

    protected void saveEmployee()
    {
        string history_id = "";
        DistributedTransaction DT = new DistributedTransaction();
        #region employee
        EEnt = new HR_EMPLOYEES();

        EEnt.EMPLOYEEID = txtIdNo.Text;
        EEnt.FIRSTNAME = hf.CapitalizeFirstAndAfterWhitespace(txtFirstname.Text);
        EEnt.MIDDLE_NAME = hf.CapitalizeFirstAndAfterWhitespace(txtMiddleName.Text);
        EEnt.LASTNAME = hf.CapitalizeFirstAndAfterWhitespace(txtLastname.Text);
        EEnt.ABBREVATION = txtAbbrevation.Text;

        if (ddlDesignation.SelectedValue == "Select")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Select the Designation");
            EEnt.DESIGNATIONID = "";
        }
        else
        {
            EEnt.DESIGNATIONID = ddlDesignation.SelectedValue;
        }

        EEnt.ETHINITICITY_ID = ddlEthnicity.SelectedValue;
        EEnt.JOBTITLE = txtJobTitle.Text;

        if (ddlDept.SelectedValue == "Select")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Select the Department");
            EEnt.DEPARTMENTID = "";
        }
        else
        {
            EEnt.DEPARTMENTID = ddlDept.SelectedValue;
        }
        if (ddlDivision.SelectedValue == "" || ddlDivision.SelectedValue == "Select")
        {
            EEnt.DIVISION = "";
        }
        else
        {
            EEnt.DIVISION = ddlDivision.SelectedValue;
        }
        if (ddlUnit.SelectedValue == "" || ddlUnit.SelectedValue == "Select")
        {
            EEnt.UNIT = "";
        }
        else
        {
            EEnt.UNIT = ddlUnit.SelectedValue;
        }

        if (ddlLevel.SelectedValue != "Select")
        {
            EEnt.HR_LEVELID = ddlLevel.SelectedValue;
        }
        else
        {
            EEnt.HR_LEVELID = "";
        }

        EEnt.HR_GRADEID = ddlGrade.SelectedValue;

        if (rbtnDifAble.SelectedValue == "1")
        {
            EEnt.DISABLED = "1";
            EEnt.DISABLED_REMARKS = txtDisRemark.Text;
        }
        else
        {
            EEnt.DISABLED = "0";
            EEnt.DISABLED_REMARKS = "";
        }

        EEnt.EMAIL = txtEmail.Text;
        EEnt.PHONE = txtPhone.Text;
        EEnt.MOBILE1 = txtMobile1.Text;
        EEnt.MOBILE2 = txtMobile2.Text;

        EEnt.DEVNAGARINAME = txtDevName.Text;
        EEnt.FATHERNAME = hf.CapitalizeFirstAndAfterWhitespace(txtFatherName.Text);
        EEnt.MOTHERNAME = hf.CapitalizeFirstAndAfterWhitespace(txtMotherName.Text);
        EEnt.GRANDFATHERNAME = hf.CapitalizeFirstAndAfterWhitespace(txtGrandFName.Text);
        if (ddlEmployeeType.SelectedValue == "Please Select")
        {

            EEnt.EMPLOYEETYPE = "";
        }
        else
        {
            EEnt.EMPLOYEETYPE = ddlEmployeeType.SelectedValue;
        }
        EEnt.STATUS = "1";
        EEnt.GENDER = rbtnGender.SelectedValue;
        EEnt.MARITALSTATUS = rbtnMaritalStatus.SelectedValue;
        EEnt.RELIGION = ddlReligion.SelectedValue;
        EEnt.BLOODGROUP = ddlBloodGroup.SelectedValue;

        EEnt.NOMINEENAME = hf.CapitalizeFirstAndAfterWhitespace(txtNomineeName.Text);
        EEnt.NOMINEEADDRESS = hf.CapitalizeFirstAndAfterWhitespace(txtNomineeAdd.Text);
        EEnt.NOMINEERELATION = hf.CapitalizeFirstAndAfterWhitespace(txtNomineeRelation.Text);
        EEnt.NOMINEE_CONTACT = txtNomineeContact.Text;
        EEnt.PANNO = txtPANNo.Text;
        EEnt.SSFNO = txtSSFNo.Text;
        EEnt.PFIDNO = txtPfidno.Text;
        EEnt.CITNO = txtCitno.Text;
        if (ChckIsPayroll.Checked == true)
        {
            EEnt.INCLUDE_IN_PAYROLL = "1";
        }
        else
        {
            EEnt.INCLUDE_IN_PAYROLL = "0";
        }
        EEnt.OFFICECODE = "HO";

        if (ddlBankName.SelectedValue == "Please Select")
        {
            EEnt.CURRENTBANK = "";
        }
        else
        {
            EEnt.CURRENTBANK = ddlBankName.SelectedValue;
        }

        EEnt.CURRENTBANKACCNO = txtCurrentbankaccount.Text;
        if (ddBankName2.SelectedValue == "Please Select")
        {
            EEnt.SECONDBANK = "";
        }
        else
        {
            EEnt.SECONDBANK = ddBankName2.SelectedValue;
        }
        EEnt.SECONDBANKACCNO = txtSecondbankaccount.Text;
        EEnt.REMARKS = txtRemarks.Text;

        if (txtDateBs.Text != "")
        {
            try
            {
                string[] NepDate = txtDateBs.Text.Split('/');
                EEnt.BIRTHDAY = NepDate[0];
                EEnt.BIRTHMONTH = NepDate[1];
                EEnt.BIRTHYEAR = NepDate[2];
            }
            catch
            { }
        }
        string Imagename;
        try
        {
            Imagename = txtIdNo.Text + ".jpg";
            if (ImageUpload.HasFile)
            {
                ImageUpload.SaveAs(Server.MapPath("~/images/Employee/" + Imagename));
                ImageUpload.SaveAs(Server.MapPath("~/images/User/" + Imagename));
            }
        }
        catch
        { }
        if (txtDateAd.Text != "")
        {
            EEnt.BIRTHDATE = txtDateAd.Text;
        }

        if (txtAppointDate.Text != "")
        {
            try
            {
                string[] AppoDate = txtAppointDate.Text.Split('/');
                EEnt.APPOINTMENTDATE = pgd.ConvertNepaliTOEnglish(AppoDate[0], AppoDate[1], AppoDate[2]);
                EEnt.APPOINTMENTDAY = AppoDate[0];
                EEnt.APPOINTMENTMONTH = AppoDate[1];
                EEnt.APPOINTMENTYEAR = AppoDate[2];
            }
            catch
            { }
        }
        EEnt.SPOUSENAME = hf.CapitalizeFirstAndAfterWhitespace(txtSpouseName.Text);

        EEnt.EMP_SUB_GL = "EMP" + txtIdNo.Text;

        EEnt.ACCIDENT_CONTACT_NAME = txtAcdName.Text;
        EEnt.ACCIDENT_CONTACT_ADDRESS = txtAcdAddr.Text;
        EEnt.ACCIDENT_CONTACT_RELATION = txtAcdRelation.Text;
        EEnt.ACCIDENT_CONTACT_NUM = txtAcdContact.Text;

        string[] nepDate = txtProbDate.Text.Split('/');
        EEnt.PROBATION_DATE = pgd.ConvertNepaliTOEnglish(nepDate[0], nepDate[1], nepDate[2]);
        EEnt.PROBATION_DAY = nepDate[0];
        EEnt.PROBATION_MONTH = nepDate[1];
        EEnt.PROBATION_YEAR = nepDate[2];

        #region for attachment
        string filename = "";
        string path = "";
        foreach (GridViewRow gr in grdAttachment.Rows)
        {
            Label lblPkId = gr.FindControl("lblPkId") as Label;
            Label lblAttDoc = gr.FindControl("lblAttDoc") as Label;
            FileUpload FileUpload1 = gr.FindControl("FileUpload1") as FileUpload;
            TextBox txtDocumentNum = gr.FindControl("txtDocumentNum") as TextBox;
            TextBox txtIssueDate = gr.FindControl("txtIssueDate") as TextBox;
            TextBox txtExpiryDate = gr.FindControl("txtExpiryDate") as TextBox;
            TextBox txtIssuedBy = gr.FindControl("txtIssuedBy") as TextBox;
            string Doc_Name = lblAttDoc.Text.Replace(" ", "_");

            ATEnt = new HR_ATTACHMENT();
            ATEnt.ATTACHMENT_ID = lblPkId.Text;
            ATEnt.ATTACHMENT_OF_ID = txtIdNo.Text;
            ATEnt = (HR_ATTACHMENT)ATSer.GetSingle(ATEnt);
            bool isAnyFilled =
                       !string.IsNullOrWhiteSpace(txtDocumentNum.Text) ||
                       !string.IsNullOrWhiteSpace(txtIssueDate.Text) ||
                       !string.IsNullOrWhiteSpace(txtExpiryDate.Text) ||
                       !string.IsNullOrWhiteSpace(txtIssuedBy.Text) ||
                       (FileUpload1 != null && FileUpload1.HasFile);

            if (isAnyFilled)
            {
                if (ATEnt == null)
                {
                    ATEnt = new HR_ATTACHMENT();
                    ATEnt.ATTACHMENT_ID = lblPkId.Text;
                    ATEnt.ATTACHMENT_OF_ID = txtIdNo.Text;
                    ATEnt.DOCUMENT_NUMBER = txtDocumentNum.Text;
                    ATEnt.ISSUE_DATE = txtIssueDate.Text;
                    ATEnt.EXPIRY_DATE = txtExpiryDate.Text;
                    ATEnt.ISSUED_BY = txtIssuedBy.Text;
                    ATSer.Insert(ATEnt, DT);
                }
                else
                {
                    ATEnt.DOCUMENT_NUMBER = txtDocumentNum.Text;
                    ATEnt.ISSUE_DATE = txtIssueDate.Text;
                    ATEnt.EXPIRY_DATE = txtExpiryDate.Text;
                    ATEnt.ISSUED_BY = txtIssuedBy.Text;
                    ATSer.Update(ATEnt, DT);
                }
                if (FileUpload1.HasFile)
                {
                    filename = FileUpload1.FileName.Substring(FileUpload1.FileName.IndexOf('.'));
                    if (filename == ".pdf")
                    {
                        try
                        {
                            path = "~/images/Attachment_Staff/Emp_" + Doc_Name + "/";
                            string physicalPath = Server.MapPath(path);

                            // Ensure the directory exists
                            if (!Directory.Exists(physicalPath))
                            {
                                Directory.CreateDirectory(physicalPath);
                            }

                            // Save the file
                            FileUpload1.SaveAs(Path.Combine(physicalPath, txtIdNo.Text + filename));


                        }
                        catch
                        {
                            // Optionally log the error
                        }

                    }
                    else
                    {
                        HelperFunction.MsgBox(this, this.GetType(), "Please update pdf files only.");
                    }
                }
            }
        }
        #endregion
        string empPk = ESer.Insert(EEnt, DT).ToString();

        #endregion

        #region For EMP_History
        HEHEnt = new HR_EMP_HISTORY();
        HEHEnt.EMP_ID = txtIdNo.Text;
        if (ddlDesignation.SelectedValue == "Select")
        {
            HEHEnt.HR_DESIGNATION_ID = "";
        }
        else
        {
            HEHEnt.HR_DESIGNATION_ID = ddlDesignation.SelectedValue;
        }
        HEHEnt.HR_DEPARTMENT_ID = ddlDept.SelectedValue;
        HEHEnt.HR_DIVISION_ID = ddlDivision.SelectedValue;
        if (ddlUnit.SelectedValue != "Select")
            HEHEnt.HR_UNIT_ID = ddlUnit.SelectedValue;
        else
            HEHEnt.HR_UNIT_ID = "";

        if (ddlLevel.SelectedValue != "Select")
            HEHEnt.HR_LEVEL_ID = ddlLevel.SelectedValue;
        else
            HEHEnt.HR_LEVEL_ID = "";

        HEHEnt.HR_GRADE_ID = ddlGrade.SelectedValue;
        string[] effDate = txtAppointDate.Text.Split('/');
        HEHEnt.EFFECTIVE_DATE = pgd.ConvertNepaliTOEnglish(effDate[0], effDate[1], effDate[2]);
        HEHEnt.EFFECTIVE_DAY = effDate[0];
        HEHEnt.EFFECTIVE_MONTH = effDate[1];
        HEHEnt.EFFECTIVE_YEAR = effDate[2];
        HEHEnt.STATUS = "1";
        history_id = HEHSer.Insert(HEHEnt, DT).ToString();
        #endregion

        #region For Contract
        if (ddlEmployeeType.SelectedItem.Text == "Contract")
        {
            HECEnt = new HR_EMP_CONTRACT();
            HECEnt.EMP_ID = txtIdNo.Text;
            HECEnt.STATUS = "1";
            HECEnt = (HR_EMP_CONTRACT)HECSer.GetSingle(HECEnt);
            if (HECEnt != null)
            {
                HECEnt.STATUS = "0";
                HECSer.Update(HECEnt, DT);
            }

            HECEnt = new HR_EMP_CONTRACT();
            HECEnt.EMP_ID = txtIdNo.Text;
            HECEnt.HR_DEPARTMENT_ID = ddlDept.SelectedValue;
            if (ddlDesignation.SelectedValue == "Select")
            {
                HECEnt.HR_DESIGNATION_ID = "";
            }
            else
            {
                HECEnt.HR_DESIGNATION_ID = ddlDesignation.SelectedValue;
            }

            if (ddlLevel.SelectedValue != "Select")
                HECEnt.HR_LEVEL_ID = ddlLevel.SelectedValue;
            else
                HECEnt.HR_LEVEL_ID = "";

            string[] stDate = txtStartDate.Text.Split('/');
            HECEnt.START_DATE = pgd.ConvertNepaliTOEnglish(stDate[0], stDate[1], stDate[2]);
            HECEnt.START_DAY = stDate[0];
            HECEnt.START_MONTH = stDate[1];
            HECEnt.START_YEAR = stDate[2];

            string[] endDate = txtEndDate.Text.Split('/');
            HECEnt.END_DATE = pgd.ConvertNepaliTOEnglish(endDate[0], endDate[1], endDate[2]);
            HECEnt.END_DAY = endDate[0];
            HECEnt.END_MONTH = endDate[1];
            HECEnt.END_YEAR = endDate[2];
            HECEnt.STATUS = "1";
            HECSer.Insert(HECEnt, DT);
        }
        #endregion

        EADEnt = new HR_EMP_ACC_DETAIL();
        EADEnt.EMPLOYEE_ID = empPk;
        EADEnt.PF_AMOUNT = txtPfidAmt.Text;
        EADEnt.BASIC_SALARY = txtBasicSal.Text;
        EADEnt.BENEFICIARY_AMOUNT = txtBeneficiaryAmt.Text;
        EADEnt.CIT_AMOUNT = txtCitPer.Text;
        EADSrv.Insert(EADEnt, DT);


        #region To insert ti gl in chkBox
        foreach (ListItem item in chkPay.Items)
        {
            // If the item is selected, add its value to the list               
            if (item.Selected)
            {
                #region To Insert to GL_SUB_ACCOUNT Under TDS GL Code
                GSAEnt = new GL_SUB_ACCOUNT();
                GSAEnt.SUB_GL_CODE = "EMP" + txtIdNo.Text;
                GSAEnt.GL_CODE = item.Value;
                GSAEnt.SUB_GL_NAME = txtFirstname.Text + " " + txtMiddleName.Text + " " + txtLastname.Text;
                GSAEnt.STATUS = "1";
                GSASer.Insert(GSAEnt, DT);
                #endregion
            }
        }

        #region for Advance Account
        foreach (ListItem item in chkAdvance.Items)
        {
            // If the item is selected, add its value to the list
            if (item.Selected)
            {
                #region To Insert to GL_SUB_ACCOUNT Under TDS GL Code
                GSAEnt = new GL_SUB_ACCOUNT();
                GSAEnt.SUB_GL_CODE = "EMP" + txtIdNo.Text;
                GSAEnt.GL_CODE = item.Value;
                GSAEnt.SUB_GL_NAME = txtFirstname.Text + " " + txtMiddleName.Text + " " + txtLastname.Text;
                GSAEnt.STATUS = "1";
                GSASer.Insert(GSAEnt, DT);
                #endregion
            }
        }
        #endregion
        #endregion
        if (ddlPA_DISTRICT.SelectedValue != "Select" && ddlTA_DISTRICT.SelectedValue != "Select")
        {

            #region Adress Detail
            #region for Nepal
            if (ddlContactAddress.SelectedValue != "Select")
            {

                #region both ADDRESS as same

                AEnt = new ADDRESS();
                AEnt.COUNTRY = "1";

                if (ddlPA_DISTRICT.SelectedIndex == 0)
                {
                    AEnt.DISTRICT = "";
                }
                else
                {
                    AEnt.DISTRICT = ddlPA_DISTRICT.SelectedValue;

                    #region To add EMployee zone and province

                    DSEnt = new District();
                    DSEnt.PK_ID = ddlPA_DISTRICT.SelectedValue;
                    DSEnt = (District)DSSer.GetSingle(DSEnt);
                    if (DSEnt != null)
                    {
                        AEnt.ZONE = DSEnt.ZONEID;
                        AEnt.STATE = DSEnt.PROVINCE_ID;
                    }
                    #endregion
                }

                AEnt.VDC_MUNICIPALITY = ddlPAVDC.SelectedValue;
                AEnt.WARD_NO = txtPA_WARD_NO.Text;
                AEnt.STREET_NAME = txtPA_STREET.Text;
                AEnt.HOUSE_NO = txtPA_HOUSE_NO.Text;
                AEnt.ADDRESS_OF = "Employee";
                AEnt.ADDRESS_OF_ID = txtIdNo.Text; ;
                AEnt.ADDRESS_TYPE = "Both";

                ASrv.Insert(AEnt, DT);



                #endregion
            }
            else
            {
                #region permanent Address

                AEnt = new ADDRESS();
                if (ddlPA_DISTRICT.SelectedIndex == 0)
                {
                    AEnt.DISTRICT = "";
                }
                else
                {
                    AEnt.DISTRICT = ddlPA_DISTRICT.SelectedValue;

                    #region To add EMployee zone and province

                    DSEnt = new District();
                    DSEnt.PK_ID = ddlPA_DISTRICT.SelectedValue;
                    DSEnt = (District)DSSer.GetSingle(DSEnt);
                    if (DSEnt != null)
                    {
                        AEnt.ZONE = DSEnt.ZONEID;
                        AEnt.STATE = DSEnt.PROVINCE_ID;
                    }
                    #endregion
                }
                AEnt.VDC_MUNICIPALITY = ddlPAVDC.SelectedValue;
                AEnt.WARD_NO = txtPA_WARD_NO.Text;
                AEnt.STREET_NAME = txtPA_STREET.Text;
                AEnt.HOUSE_NO = txtPA_HOUSE_NO.Text;
                AEnt.ADDRESS_OF = "Employee";
                AEnt.ADDRESS_OF_ID = txtIdNo.Text; ;
                AEnt.ADDRESS_TYPE = "P";
                AEnt.COUNTRY = "1";
                ASrv.Insert(AEnt, DT);

                #endregion

                #region Contact Address

                AEnt = new ADDRESS();
                if (ddlPA_DISTRICT.SelectedIndex == 0)
                {
                    AEnt.DISTRICT = "";
                }
                else
                {
                    AEnt.DISTRICT = ddlTA_DISTRICT.SelectedValue;

                    #region To add EMployee zone and province

                    DSEnt = new District();
                    DSEnt.PK_ID = ddlPA_DISTRICT.SelectedValue;
                    DSEnt = (District)DSSer.GetSingle(DSEnt);
                    if (DSEnt != null)
                    {
                        AEnt.ZONE = DSEnt.ZONEID;
                        AEnt.STATE = DSEnt.PROVINCE_ID;
                    }
                    #endregion
                }
                AEnt.VDC_MUNICIPALITY = ddlTAVDC.SelectedValue;
                AEnt.WARD_NO = txtTA_WARD_NO.Text;
                AEnt.STREET_NAME = txtTA_STREET.Text;
                AEnt.HOUSE_NO = txtTA_HOUSE_NO.Text;
                AEnt.ADDRESS_OF = "Employee";
                AEnt.ADDRESS_OF_ID = txtIdNo.Text; ;
                AEnt.ADDRESS_TYPE = "C";
                AEnt.COUNTRY = "1";
                ASrv.Insert(AEnt, DT);
                #endregion

            }
            #endregion
            #endregion
        }
        if (DT.HAPPY)
        {
            DT.Commit();
            
            #region Offer Letter
            string offer_letter;
            try
            {
                string empId = txtIdNo.Text.Trim();
                string empFolderPath = Server.MapPath("~/images/Offer_Letter/" + empId + "/");

                if (!Directory.Exists(empFolderPath))
                {
                    Directory.CreateDirectory(empFolderPath);
                }

                offer_letter = "Offer_Letter_" + history_id + ".pdf";
                if (imgOfferLetter.HasFile)
                {
                    HEHEnt = new HR_EMP_HISTORY();
                    HEHEnt.PK_ID = history_id;
                    HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
                    if (HEHEnt != null)
                    {
                        HEHEnt.OFFER_LETTER = "1";
                        HEHSer.Update(HEHEnt);
                    }
                    string fileExtension = Path.GetExtension(imgOfferLetter.FileName).ToLower();
                    string contentType = imgOfferLetter.PostedFile.ContentType.ToLower();

                    if (fileExtension == ".pdf" && contentType == "application/pdf")
                    {
                        string fullPath = Path.Combine(empFolderPath, offer_letter);
                        imgOfferLetter.SaveAs(fullPath);
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
            #endregion

            #region Agreement
            string agreement;
            try
            {
                string empId = txtIdNo.Text.Trim();
                string empFolderPath = Server.MapPath("~/images/Agreement/" + empId + "/");

                if (!Directory.Exists(empFolderPath))
                {
                    Directory.CreateDirectory(empFolderPath);
                }

                agreement = "Agreement_" + history_id + ".pdf";
                if (imgAgreement.HasFile)
                {
                    HEHEnt = new HR_EMP_HISTORY();
                    HEHEnt.PK_ID = history_id;
                    HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
                    if (HEHEnt != null)
                    {
                        HEHEnt.AGREEMENT = "1";
                        HEHSer.Update(HEHEnt);
                    }
                    string fileExtension = Path.GetExtension(imgAgreement.FileName).ToLower();
                    string contentType = imgAgreement.PostedFile.ContentType.ToLower();

                    if (fileExtension == ".pdf" && contentType == "application/pdf")
                    {
                        string fullPath = Path.Combine(empFolderPath, agreement);
                        imgAgreement.SaveAs(fullPath);
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
            #endregion

            #region Contract
            string contract;
            try
            {
                string empId = txtIdNo.Text.Trim();
                string start_Date = txtStartDate.Text.Replace("/", "-").Trim();
                string end_Date = txtEndDate.Text.Replace("/", "-").Trim();

                string empFolderPath = Server.MapPath("~/images/Contracts/" + empId + "/");

                if (!Directory.Exists(empFolderPath))
                {
                    Directory.CreateDirectory(empFolderPath);
                }

                contract = "Contract_" + history_id + ".pdf";
                if (imgContract.HasFile)
                {
                    HEHEnt = new HR_EMP_HISTORY();
                    HEHEnt.PK_ID = history_id;
                    HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
                    if (HEHEnt != null)
                    {
                        HEHEnt.CONTRACT = "1";
                        HEHSer.Update(HEHEnt);
                    }
                    string fileExtension = Path.GetExtension(imgContract.FileName).ToLower();
                    string contentType = imgContract.PostedFile.ContentType.ToLower();

                    if (fileExtension == ".pdf" && contentType == "application/pdf")
                    {
                        string fullPath = Path.Combine(empFolderPath, contract);
                        imgContract.SaveAs(fullPath);
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
            #endregion

            HelperFunction.MsgBox(this, this.GetType(), "Employee Recorded");
            ClearField();

        }
        else
        {
            DT.Abort();
            HelperFunction.MsgBox(this, this.GetType(), "Something went wrong!!");
        }
        DT.Dispose();
    }
    protected void updateEmployee()
    {
        string filename = "";
        DistributedTransaction DT = new DistributedTransaction();
        #region Employees
        EEnt = new HR_EMPLOYEES();
        EEnt.EMPLOYEEID = txtIdNo.Text;
        EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
        if (EEnt != null)
        {
            EEnt.FIRSTNAME = hf.CapitalizeFirstAndAfterWhitespace(txtFirstname.Text);
            EEnt.MIDDLE_NAME = hf.CapitalizeFirstAndAfterWhitespace(txtMiddleName.Text);
            EEnt.LASTNAME = hf.CapitalizeFirstAndAfterWhitespace(txtLastname.Text);
            EEnt.ABBREVATION = txtAbbrevation.Text;

            if (ddlDesignation.SelectedValue == "Select")
            {
                EEnt.DESIGNATIONID = "";
            }
            else
            {
                EEnt.DESIGNATIONID = ddlDesignation.SelectedValue;
            }

            EEnt.ETHINITICITY_ID = ddlEthnicity.SelectedValue;
            EEnt.JOBTITLE = txtJobTitle.Text;

            if (ddlDept.SelectedValue == "Select")
            {
                EEnt.DEPARTMENTID = "";
            }
            else
            {
                EEnt.DEPARTMENTID = ddlDept.SelectedValue;
            }
            if (ddlDivision.SelectedValue == "" || ddlDivision.SelectedValue == "Select")
            {
                EEnt.DIVISION = "";
            }
            else
            {
                EEnt.DIVISION = ddlDivision.SelectedValue;
            }
            if (ddlUnit.SelectedValue == "" || ddlUnit.SelectedValue == "Select")
            {
                EEnt.UNIT = "";
            }
            else
            {
                EEnt.UNIT = ddlUnit.SelectedValue;
            }

            if (ddlLevel.SelectedValue != "Select")
            {
                EEnt.HR_LEVELID = ddlLevel.SelectedValue;
            }
            else
            {
                EEnt.HR_LEVELID = "";
            }

            EEnt.HR_GRADEID = ddlGrade.SelectedValue;

            if (rbtnDifAble.SelectedValue == "1")
            {
                EEnt.DISABLED = "1";
                EEnt.DISABLED_REMARKS = txtDisRemark.Text;
            }
            else
            {
                EEnt.DISABLED = "0";
                EEnt.DISABLED_REMARKS = "";
            }

            EEnt.EMAIL = txtEmail.Text;
            EEnt.PHONE = txtPhone.Text;
            EEnt.MOBILE1 = txtMobile1.Text;
            EEnt.MOBILE2 = txtMobile2.Text;

            EEnt.DEVNAGARINAME = txtDevName.Text;
            EEnt.FATHERNAME = hf.CapitalizeFirstAndAfterWhitespace(txtFatherName.Text);
            EEnt.MOTHERNAME = hf.CapitalizeFirstAndAfterWhitespace(txtMotherName.Text);
            EEnt.GRANDFATHERNAME = hf.CapitalizeFirstAndAfterWhitespace(txtGrandFName.Text);
            if (ddlEmployeeType.SelectedValue == "Please Select")
            {

                EEnt.EMPLOYEETYPE = "";
            }
            else
            {
                EEnt.EMPLOYEETYPE = ddlEmployeeType.SelectedValue;
            }
            EEnt.STATUS = "1";
            EEnt.GENDER = rbtnGender.SelectedValue;
            EEnt.MARITALSTATUS = rbtnMaritalStatus.SelectedValue;
            EEnt.RELIGION = ddlReligion.SelectedValue;
            EEnt.BLOODGROUP = ddlBloodGroup.SelectedValue;

            EEnt.NOMINEENAME = hf.CapitalizeFirstAndAfterWhitespace(txtNomineeName.Text);
            EEnt.NOMINEEADDRESS = hf.CapitalizeFirstAndAfterWhitespace(txtNomineeAdd.Text);
            EEnt.NOMINEERELATION = hf.CapitalizeFirstAndAfterWhitespace(txtNomineeRelation.Text);
            EEnt.NOMINEE_CONTACT = txtNomineeContact.Text;
            EEnt.PANNO = txtPANNo.Text;
            EEnt.SSFNO = txtSSFNo.Text;
            EEnt.PFIDNO = txtPfidno.Text;
            EEnt.CITNO = txtCitno.Text;
            if (ChckIsPayroll.Checked == true)
            {
                EEnt.INCLUDE_IN_PAYROLL = "1";
            }
            else
            {
                EEnt.INCLUDE_IN_PAYROLL = "0";
            }
            EEnt.OFFICECODE = "HO";

            if (ddlBankName.SelectedValue == "Please Select")
            {
                EEnt.CURRENTBANK = "";
            }
            else
            {
                EEnt.CURRENTBANK = ddlBankName.SelectedValue;
            }

            EEnt.CURRENTBANKACCNO = txtCurrentbankaccount.Text;
            if (ddBankName2.SelectedValue == "Please Select")
            {
                EEnt.SECONDBANK = "";
            }
            else
            {
                EEnt.SECONDBANK = ddBankName2.SelectedValue;
            }
            EEnt.SECONDBANKACCNO = txtSecondbankaccount.Text;
            EEnt.REMARKS = txtRemarks.Text;

            if (txtDateBs.Text != "")
            {
                try
                {
                    string[] NepDate = txtDateBs.Text.Split('/');
                    EEnt.BIRTHDAY = NepDate[0];
                    EEnt.BIRTHMONTH = NepDate[1];
                    EEnt.BIRTHYEAR = NepDate[2];
                }
                catch
                { }
            }
            string Imagename;
            try
            {
                Imagename = txtIdNo.Text + ".jpg";
                if (ImageUpload.HasFile)
                {
                    ImageUpload.SaveAs(Server.MapPath("~/images/Employee/" + Imagename));
                    ImageUpload.SaveAs(Server.MapPath("~/images/User/" + Imagename));
                }
            }
            catch
            { }
            if (txtDateAd.Text != "")
            {
                EEnt.BIRTHDATE = txtDateAd.Text;
            }

            if (txtAppointDate.Text != "")
            {
                try
                {
                    string[] AppoDate = txtAppointDate.Text.Split('/');
                    EEnt.APPOINTMENTDATE = pgd.ConvertNepaliTOEnglish(AppoDate[0], AppoDate[1], AppoDate[2]);
                    EEnt.APPOINTMENTDAY = AppoDate[0];
                    EEnt.APPOINTMENTMONTH = AppoDate[1];
                    EEnt.APPOINTMENTYEAR = AppoDate[2];
                }
                catch
                { }
            }
            EEnt.SPOUSENAME = hf.CapitalizeFirstAndAfterWhitespace(txtSpouseName.Text);

            EEnt.EMP_SUB_GL = "EMP" + txtIdNo.Text;

            EEnt.ACCIDENT_CONTACT_NAME = txtAcdName.Text;
            EEnt.ACCIDENT_CONTACT_ADDRESS = txtAcdAddr.Text;
            EEnt.ACCIDENT_CONTACT_RELATION = txtAcdRelation.Text;
            EEnt.ACCIDENT_CONTACT_NUM = txtAcdContact.Text;

            string[] nepDate = txtProbDate.Text.Split('/');
            EEnt.PROBATION_DATE = pgd.ConvertNepaliTOEnglish(nepDate[0], nepDate[1], nepDate[2]);
            EEnt.PROBATION_DAY = nepDate[0];
            EEnt.PROBATION_MONTH = nepDate[1];
            EEnt.PROBATION_YEAR = nepDate[2];

            #region for attachment
            string path = "";
            foreach (GridViewRow gr in grdAttachment.Rows)
            {
                Label lblPkId = gr.FindControl("lblPkId") as Label;
                Label lblAttDoc = gr.FindControl("lblAttDoc") as Label;
                FileUpload FileUpload1 = gr.FindControl("FileUpload1") as FileUpload;
                TextBox txtDocumentNum = gr.FindControl("txtDocumentNum") as TextBox;
                TextBox txtIssueDate = gr.FindControl("txtIssueDate") as TextBox;
                TextBox txtExpiryDate = gr.FindControl("txtExpiryDate") as TextBox;
                TextBox txtIssuedBy = gr.FindControl("txtIssuedBy") as TextBox;
                string Doc_Name = lblAttDoc.Text.Replace(" ", "_");

                ATEnt = new HR_ATTACHMENT();
                ATEnt.ATTACHMENT_ID = lblPkId.Text;
                ATEnt.ATTACHMENT_OF_ID = txtIdNo.Text;
                ATEnt = (HR_ATTACHMENT)ATSer.GetSingle(ATEnt);

                bool isAnyFilled =
                        !string.IsNullOrWhiteSpace(txtDocumentNum.Text) ||
                        !string.IsNullOrWhiteSpace(txtIssueDate.Text) ||
                        !string.IsNullOrWhiteSpace(txtExpiryDate.Text) ||
                        !string.IsNullOrWhiteSpace(txtIssuedBy.Text) ||
                        (FileUpload1 != null && FileUpload1.HasFile);

                if (isAnyFilled)
                {
                    if (ATEnt == null)
                    {
                        ATEnt = new HR_ATTACHMENT();
                        ATEnt.ATTACHMENT_ID = lblPkId.Text;
                        ATEnt.ATTACHMENT_OF_ID = txtIdNo.Text;
                        ATEnt.DOCUMENT_NUMBER = txtDocumentNum.Text;
                        ATEnt.ISSUE_DATE = txtIssueDate.Text;
                        ATEnt.EXPIRY_DATE = txtExpiryDate.Text;
                        ATEnt.ISSUED_BY = txtIssuedBy.Text;
                        ATSer.Insert(ATEnt, DT);
                    }
                    else
                    {
                        ATEnt.DOCUMENT_NUMBER = txtDocumentNum.Text;
                        ATEnt.ISSUE_DATE = txtIssueDate.Text;
                        ATEnt.EXPIRY_DATE = txtExpiryDate.Text;
                        ATEnt.ISSUED_BY = txtIssuedBy.Text;
                        ATSer.Update(ATEnt, DT);
                    }


                    if (ATEnt == null)
                    {
                        ATEnt = new HR_ATTACHMENT();
                        ATEnt.ATTACHMENT_ID = lblPkId.Text;
                        ATEnt.ATTACHMENT_OF_ID = txtIdNo.Text;
                        ATEnt.DOCUMENT_NUMBER = txtDocumentNum.Text;
                        ATEnt.ISSUE_DATE = txtIssueDate.Text;
                        ATEnt.EXPIRY_DATE = txtExpiryDate.Text;
                        ATEnt.ISSUED_BY = txtIssuedBy.Text;
                        ATSer.Insert(ATEnt, DT);
                    }
                    else
                    {
                        ATEnt.DOCUMENT_NUMBER = txtDocumentNum.Text;
                        ATEnt.ISSUE_DATE = txtIssueDate.Text;
                        ATEnt.EXPIRY_DATE = txtExpiryDate.Text;
                        ATEnt.ISSUED_BY = txtIssuedBy.Text;
                        ATSer.Update(ATEnt, DT);
                    }
                    if (FileUpload1.HasFile)
                    {
                        filename = FileUpload1.FileName.Substring(FileUpload1.FileName.IndexOf('.'));
                        if (filename == ".pdf")
                        {
                            try
                            {
                                path = "~/images/Attachment_Staff/Emp_" + Doc_Name + "/";
                                string physicalPath = Server.MapPath(path);

                                // Ensure the directory exists
                                if (!Directory.Exists(physicalPath))
                                {
                                    Directory.CreateDirectory(physicalPath);
                                }

                                // Save the file
                                FileUpload1.SaveAs(Path.Combine(physicalPath, txtIdNo.Text + filename));


                            }
                            catch
                            {
                                // Optionally log the error
                            }

                        }
                        else
                        {
                            HelperFunction.MsgBox(this, this.GetType(), "Please update pdf files only.");
                        }
                    }
                }

                // Proceed with insert or update

            }
            #endregion
            ESer.Update(EEnt, DT);

            #region For Contract
            if (ddlEmployeeType.SelectedItem.Text == "Contract")
            {
                HECEnt = new HR_EMP_CONTRACT();
                HECEnt.EMP_ID = txtIdNo.Text;
                HECEnt.STATUS = "1";
                HECEnt = (HR_EMP_CONTRACT)HECSer.GetSingle(HECEnt);
                if (HECEnt != null)
                {
                    HECEnt.STATUS = "0";
                    HECSer.Update(HECEnt, DT);
                }

                HECEnt = new HR_EMP_CONTRACT();
                HECEnt.EMP_ID = txtIdNo.Text;
                HECEnt.HR_DEPARTMENT_ID = ddlDept.SelectedValue;
                if (ddlDesignation.SelectedValue == "Select")
                {
                    HECEnt.HR_DESIGNATION_ID = "";
                }
                else
                {
                    HECEnt.HR_DESIGNATION_ID = ddlDesignation.SelectedValue;
                }

                if (ddlLevel.SelectedValue != "Select")
                    HECEnt.HR_LEVEL_ID = ddlLevel.SelectedValue;
                else
                    HECEnt.HR_LEVEL_ID = "";

                string[] stDate = txtStartDate.Text.Split('/');
                HECEnt.START_DATE = pgd.ConvertNepaliTOEnglish(stDate[0], stDate[1], stDate[2]);
                HECEnt.START_DAY = stDate[0];
                HECEnt.START_MONTH = stDate[1];
                HECEnt.START_YEAR = stDate[2];

                string[] endDate = txtEndDate.Text.Split('/');
                HECEnt.END_DATE = pgd.ConvertNepaliTOEnglish(endDate[0], endDate[1], endDate[2]);
                HECEnt.END_DAY = stDate[0];
                HECEnt.END_MONTH = stDate[1];
                HECEnt.END_YEAR = stDate[2];
                HECEnt.STATUS = "1";
                HECSer.Insert(HECEnt, DT);
            }
            #endregion

            HEHEnt = new HR_EMP_HISTORY();
            HEHEnt.EMP_ID = txtIdNo.Text;
            HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
            if (HEHEnt != null)
            {
                if (ddlDesignation.SelectedValue == "Select")
                {
                    HEHEnt.HR_DESIGNATION_ID = "";
                }
                else
                {
                    HEHEnt.HR_DESIGNATION_ID = ddlDesignation.SelectedValue;
                }

                HEHEnt.HR_DEPARTMENT_ID = ddlDept.SelectedValue;
                HEHEnt.HR_DIVISION_ID = ddlDivision.SelectedValue;
                if (ddlUnit.SelectedValue != "Select")
                    HEHEnt.HR_UNIT_ID = ddlUnit.SelectedValue;
                else
                    HEHEnt.HR_UNIT_ID = "";

                if (ddlLevel.SelectedValue != "Select")
                    HEHEnt.HR_LEVEL_ID = ddlLevel.SelectedValue;
                else
                    HEHEnt.HR_LEVEL_ID = "";

                HEHEnt.HR_GRADE_ID = ddlGrade.SelectedValue;
                string[] effDate = txtAppointDate.Text.Split('/');
                HEHEnt.EFFECTIVE_DATE = pgd.ConvertNepaliTOEnglish(effDate[0], effDate[1], effDate[2]);
                HEHEnt.EFFECTIVE_DAY = effDate[0];
                HEHEnt.EFFECTIVE_MONTH = effDate[1];
                HEHEnt.EFFECTIVE_YEAR = effDate[2];
                HEHEnt.STATUS = "1";
                HEHSer.Update(HEHEnt, DT).ToString();
            }

            EADEnt = new HR_EMP_ACC_DETAIL();
            EADEnt.EMPLOYEE_ID = EEnt.EMPLOYEEID;
            EADEnt = (HR_EMP_ACC_DETAIL)EADSrv.GetSingle(EADEnt);
            if (EADEnt != null)
            {
                EADEnt.PF_AMOUNT = txtPfidAmt.Text;
                EADEnt.BASIC_SALARY = txtBasicSal.Text;
                EADEnt.BENEFICIARY_AMOUNT = txtBeneficiaryAmt.Text;
                EADEnt.CIT_AMOUNT = txtCitPer.Text;
                EADSrv.Update(EADEnt, DT);
            }
            #region for payment Accounts
            foreach (ListItem item in chkPay.Items)
            {
                // If the item is selected, add its value to the list
                if (item.Selected)
                {
                    GSAEnt = new GL_SUB_ACCOUNT();
                    GSAEnt.SUB_GL_CODE = "EMP" + txtIdNo.Text;
                    GSAEnt.GL_CODE = item.Value;
                    GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
                    if (GSAEnt != null)
                    {
                        GSAEnt.SUB_GL_NAME = txtFirstname.Text + " " + txtMiddleName.Text + " " + txtLastname.Text;
                        GSAEnt.STATUS = "1";
                        GSASer.Update(GSAEnt, DT);
                    }
                    else
                    {
                        GSAEnt = new GL_SUB_ACCOUNT();
                        GSAEnt.SUB_GL_CODE = "EMP" + txtIdNo.Text;
                        GSAEnt.GL_CODE = item.Value;
                        GSAEnt.SUB_GL_NAME = txtFirstname.Text + " " + txtMiddleName.Text + " " + txtLastname.Text;
                        GSAEnt.STATUS = "1";
                        GSASer.Insert(GSAEnt, DT);
                    }
                }
                else
                {
                    GSAEnt = new GL_SUB_ACCOUNT();
                    GSAEnt.SUB_GL_CODE = "EMP" + txtIdNo.Text;
                    GSAEnt.GL_CODE = item.Value;
                    GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
                    if (GSAEnt != null)
                    {
                        GSAEnt.STATUS = "0";
                        GSASer.Update(GSAEnt, DT);
                    }
                }
            }
            #endregion
            #region for Advance Account
            foreach (ListItem item in chkAdvance.Items)
            {
                // If the item is selected, add its value to the list
                if (item.Selected)
                {
                    GSAEnt = new GL_SUB_ACCOUNT();
                    GSAEnt.SUB_GL_CODE = "EMP" + txtIdNo.Text;
                    GSAEnt.GL_CODE = item.Value;
                    GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
                    if (GSAEnt != null)
                    {
                        GSAEnt.SUB_GL_NAME = txtFirstname.Text + " " + txtMiddleName.Text + " " + txtLastname.Text;
                        GSAEnt.STATUS = "1";
                        GSASer.Update(GSAEnt, DT);
                    }
                    else
                    {
                        GSAEnt = new GL_SUB_ACCOUNT();
                        GSAEnt.SUB_GL_CODE = "EMP" + txtIdNo.Text;
                        GSAEnt.GL_CODE = item.Value;
                        GSAEnt.SUB_GL_NAME = txtFirstname.Text + " " + txtMiddleName.Text + " " + txtLastname.Text;
                        GSAEnt.STATUS = "1";
                        GSASer.Insert(GSAEnt, DT);
                    }
                }
                else
                {
                    GSAEnt = new GL_SUB_ACCOUNT();
                    GSAEnt.SUB_GL_CODE = "EMP" + txtIdNo.Text;
                    GSAEnt.GL_CODE = item.Value;
                    GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
                    if (GSAEnt != null)
                    {
                        GSAEnt.STATUS = "0";
                        GSASer.Update(GSAEnt, DT);
                    }
                }
            }
            #endregion
            #endregion
        }


        if (ddlPA_DISTRICT.SelectedValue != "Select" && ddlTA_DISTRICT.SelectedValue != "Select")
        {

            #region Address Detail
            #region if adrress is not available in the first place   
            theList = new EntityList();
            AEnt = new ADDRESS();
            AEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
            AEnt.ADDRESS_OF = "Employee";
            theList = ASrv.GetAll(AEnt);
            if (theList.Count == 0)
            {
                if (ddlContactAddress.SelectedValue != "Select")
                {

                    #region both ADDRESS as same

                    AEnt = new ADDRESS();
                    AEnt.COUNTRY = "1";

                    if (ddlPA_DISTRICT.SelectedIndex == 0)
                    {
                        AEnt.DISTRICT = "";
                    }
                    else
                    {
                        AEnt.DISTRICT = ddlPA_DISTRICT.SelectedValue;

                        #region To add EMployee zone and province

                        DSEnt = new District();
                        DSEnt.PK_ID = ddlPA_DISTRICT.SelectedValue;
                        DSEnt = (District)DSSer.GetSingle(DSEnt);
                        if (DSEnt != null)
                        {
                            AEnt.ZONE = DSEnt.ZONEID;
                            AEnt.STATE = DSEnt.PROVINCE_ID;
                        }
                        #endregion
                    }

                    AEnt.VDC_MUNICIPALITY = ddlPAVDC.SelectedValue;
                    AEnt.WARD_NO = hf.CapitalizeFirstAndAfterWhitespace(txtPA_WARD_NO.Text);
                    AEnt.STREET_NAME = hf.CapitalizeFirstAndAfterWhitespace(txtPA_STREET.Text);
                    AEnt.HOUSE_NO = hf.CapitalizeFirstAndAfterWhitespace(txtPA_HOUSE_NO.Text);
                    AEnt.ADDRESS_OF = "Employee";
                    AEnt.ADDRESS_OF_ID = txtIdNo.Text; ;
                    AEnt.ADDRESS_TYPE = "Both";

                    ASrv.Insert(AEnt, DT);



                    #endregion
                }
                else
                {
                    #region permanent Address

                    AEnt = new ADDRESS();
                    if (ddlPA_DISTRICT.SelectedIndex == 0)
                    {
                        AEnt.DISTRICT = "";
                    }
                    else
                    {
                        AEnt.DISTRICT = ddlPA_DISTRICT.SelectedValue;

                        #region To add EMployee zone and province

                        DSEnt = new District();
                        DSEnt.PK_ID = ddlPA_DISTRICT.SelectedValue;
                        DSEnt = (District)DSSer.GetSingle(DSEnt);
                        if (DSEnt != null)
                        {
                            AEnt.ZONE = DSEnt.ZONEID;
                            AEnt.STATE = DSEnt.PROVINCE_ID;
                        }
                        #endregion
                    }
                    AEnt.VDC_MUNICIPALITY = ddlPAVDC.SelectedValue;
                    AEnt.WARD_NO = hf.CapitalizeFirstAndAfterWhitespace(txtPA_WARD_NO.Text);
                    AEnt.STREET_NAME = hf.CapitalizeFirstAndAfterWhitespace(txtPA_STREET.Text);
                    AEnt.HOUSE_NO = hf.CapitalizeFirstAndAfterWhitespace(txtPA_HOUSE_NO.Text);
                    AEnt.ADDRESS_OF = "Employee";
                    AEnt.ADDRESS_OF_ID = txtIdNo.Text; ;
                    AEnt.ADDRESS_TYPE = "P";
                    AEnt.COUNTRY = "1";
                    ASrv.Insert(AEnt, DT);

                    #endregion

                    #region Contact Address

                    AEnt = new ADDRESS();
                    if (ddlPA_DISTRICT.SelectedIndex == 0)
                    {
                        AEnt.DISTRICT = "";
                    }
                    else
                    {
                        AEnt.DISTRICT = ddlPA_DISTRICT.SelectedValue;

                        #region To add EMployee zone and province

                        DSEnt = new District();
                        DSEnt.PK_ID = ddlPA_DISTRICT.SelectedValue;
                        DSEnt = (District)DSSer.GetSingle(DSEnt);
                        if (DSEnt != null)
                        {
                            AEnt.ZONE = DSEnt.ZONEID;
                            AEnt.STATE = DSEnt.PROVINCE_ID;
                        }
                        #endregion
                    }
                    AEnt.VDC_MUNICIPALITY = ddlTAVDC.SelectedValue;
                    AEnt.WARD_NO = hf.CapitalizeFirstAndAfterWhitespace(txtTA_WARD_NO.Text);
                    AEnt.STREET_NAME = hf.CapitalizeFirstAndAfterWhitespace(txtTA_STREET.Text);
                    AEnt.HOUSE_NO = hf.CapitalizeFirstAndAfterWhitespace(txtTA_HOUSE_NO.Text);
                    AEnt.ADDRESS_OF = "Employee";
                    AEnt.ADDRESS_OF_ID = txtIdNo.Text; ;
                    AEnt.ADDRESS_TYPE = "C";
                    AEnt.COUNTRY = "1";
                    ASrv.Insert(AEnt, DT);
                    #endregion

                }
            }
            #endregion

            if (ddlContactAddress.SelectedValue != "Select") // Both Permanant and Contact is same
            {

                #region both ADDRESS is same when updating new data and old data is different

                theList = new EntityList();
                AEnt = new ADDRESS();
                AEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
                AEnt.ADDRESS_OF = "Employee";
                theList = ASrv.GetAll(AEnt);
                if (theList.Count > 1)
                {
                    AEnt = new ADDRESS();
                    AEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
                    AEnt.ADDRESS_OF = "Employee";
                    AEnt.ADDRESS_TYPE = "C";
                    AEnt = (ADDRESS)ASrv.GetSingle(AEnt);
                    if (AEnt != null)
                    {
                        ASrv.Delete(AEnt); // deleteing old  different data 
                    }

                    AEnt = new ADDRESS();
                    AEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
                    AEnt.ADDRESS_OF = "Employee";
                    AEnt.ADDRESS_TYPE = "P";
                    AEnt = (ADDRESS)ASrv.GetSingle(AEnt);
                    if (AEnt != null)
                    {
                        if (ddlPA_DISTRICT.SelectedIndex == 0)
                        {
                            AEnt.DISTRICT = "";
                        }
                        else
                        {
                            AEnt.DISTRICT = ddlPA_DISTRICT.SelectedValue;

                            #region To add EMployee zone and province

                            DSEnt.PK_ID = ddlPA_DISTRICT.SelectedValue;
                            DSEnt = (District)DSSer.GetSingle(DSEnt);
                            if (DSEnt != null)
                            {
                                AEnt.ZONE = DSEnt.ZONEID;
                                AEnt.STATE = DSEnt.PROVINCE_ID;
                            }
                        }
                        #endregion
                    }
                    AEnt.VDC_MUNICIPALITY = ddlPAVDC.SelectedValue;
                    AEnt.WARD_NO = hf.CapitalizeFirstAndAfterWhitespace(txtPA_WARD_NO.Text);
                    AEnt.STREET_NAME = hf.CapitalizeFirstAndAfterWhitespace(txtPA_STREET.Text);
                    AEnt.HOUSE_NO = hf.CapitalizeFirstAndAfterWhitespace(txtPA_HOUSE_NO.Text);
                    AEnt.ADDRESS_OF = "Employee";
                    AEnt.ADDRESS_OF_ID = txtIdNo.Text;
                    AEnt.ADDRESS_TYPE = "Both";
                    AEnt.COUNTRY = "1";
                    ASrv.Update(AEnt, DT);
                }
            }

            else      // both ADDRESS is same when updateing new data and old data is also same
            {
                AEnt = new ADDRESS();
                AEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
                AEnt.ADDRESS_TYPE = "Both";
                AEnt.ADDRESS_OF = "Employee";
                AEnt = (ADDRESS)ASrv.GetSingle(AEnt);
                if (AEnt != null)
                {

                    if (ddlPA_DISTRICT.SelectedIndex == 0)
                    {
                        AEnt.DISTRICT = "";
                    }
                    else
                    {
                        AEnt.DISTRICT = ddlPA_DISTRICT.SelectedValue;

                        #region To add EMployee zone and province

                        DSEnt = new District();
                        DSEnt.PK_ID = ddlPA_DISTRICT.SelectedValue;
                        DSEnt = (District)DSSer.GetSingle(DSEnt);
                        if (DSEnt != null)
                        {
                            AEnt.ZONE = DSEnt.ZONEID;
                            AEnt.STATE = DSEnt.PROVINCE_ID;
                        }

                        #endregion
                    }

                    AEnt.VDC_MUNICIPALITY = ddlPAVDC.SelectedValue;
                    AEnt.WARD_NO = hf.CapitalizeFirstAndAfterWhitespace(txtPA_WARD_NO.Text);
                    AEnt.STREET_NAME = hf.CapitalizeFirstAndAfterWhitespace(txtPA_STREET.Text);
                    AEnt.HOUSE_NO = hf.CapitalizeFirstAndAfterWhitespace(txtPA_HOUSE_NO.Text);
                    AEnt.ADDRESS_OF = "Employee";
                    AEnt.ADDRESS_OF_ID = txtIdNo.Text;
                    AEnt.ADDRESS_TYPE = "P";
                    AEnt.COUNTRY = "1";
                    ASrv.Update(AEnt, DT);

                    #region for contact address when both 

                    AEnt = new ADDRESS();
                    if (ddlPA_DISTRICT.SelectedIndex == 0)
                    {
                        AEnt.DISTRICT = "";
                    }
                    else
                    {
                        AEnt.DISTRICT = ddlPA_DISTRICT.SelectedValue;

                        #region To add EMployee zone and province

                        DSEnt = new District();
                        DSEnt.PK_ID = ddlTA_DISTRICT.SelectedValue;
                        DSEnt = (District)DSSer.GetSingle(DSEnt);
                        if (DSEnt != null)
                        {
                            AEnt.ZONE = DSEnt.ZONEID;
                            AEnt.STATE = DSEnt.PROVINCE_ID;
                        }


                    }

                    AEnt.VDC_MUNICIPALITY = ddlTAVDC.SelectedValue;
                    AEnt.WARD_NO = hf.CapitalizeFirstAndAfterWhitespace(txtTA_WARD_NO.Text);
                    AEnt.STREET_NAME = hf.CapitalizeFirstAndAfterWhitespace(txtTA_STREET.Text);
                    AEnt.HOUSE_NO = hf.CapitalizeFirstAndAfterWhitespace(txtTA_HOUSE_NO.Text);
                    AEnt.ADDRESS_OF = "Employee";
                    AEnt.ADDRESS_OF_ID = txtIdNo.Text;
                    AEnt.ADDRESS_TYPE = "C";
                    AEnt.COUNTRY = "1";
                    ASrv.Insert(AEnt, DT);


                }
                #endregion
                #region for Contact Address
                AEnt = new ADDRESS();
                AEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
                AEnt.ADDRESS_OF = "Employee";
                AEnt.ADDRESS_TYPE = "C";
                AEnt = (ADDRESS)ASrv.GetSingle(AEnt);
                if (AEnt != null)
                {
                    AEnt.COUNTRY = "1";
                    if (ddlTA_DISTRICT.SelectedIndex == 0)
                    {
                        AEnt.DISTRICT = "";
                    }
                    else
                    {
                        AEnt.DISTRICT = ddlTA_DISTRICT.SelectedValue;
                        #region To add EMployee zone and province
                        DSEnt = new District();
                        DSEnt.PK_ID = ddlPA_DISTRICT.SelectedValue;
                        DSEnt = (District)DSSer.GetSingle(DSEnt);
                        if (DSEnt != null)
                        {
                            AEnt.ZONE = DSEnt.ZONEID;
                            AEnt.STATE = DSEnt.PROVINCE_ID;
                        }

                        #endregion
                    }

                    AEnt.VDC_MUNICIPALITY = ddlTAVDC.SelectedValue;
                    AEnt.WARD_NO = hf.CapitalizeFirstAndAfterWhitespace(txtTA_WARD_NO.Text);
                    AEnt.STREET_NAME = hf.CapitalizeFirstAndAfterWhitespace(txtTA_STREET.Text);
                    AEnt.HOUSE_NO = hf.CapitalizeFirstAndAfterWhitespace(txtTA_HOUSE_NO.Text);
                    AEnt.ADDRESS_OF = "Employee";
                    AEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
                    ASrv.Update(AEnt, DT);
                }

                #endregion
                #region for permanent address update

                AEnt = new ADDRESS();
                if (ddlPA_DISTRICT.SelectedIndex == 0)
                {
                    AEnt.DISTRICT = "";
                }
                else
                {
                    AEnt.DISTRICT = ddlTA_DISTRICT.SelectedValue;

                    #region To add EMployee zone and province

                    DSEnt = new District();
                    DSEnt.PK_ID = ddlPA_DISTRICT.SelectedValue;
                    DSEnt = (District)DSSer.GetSingle(DSEnt);
                    if (DSEnt != null)
                    {
                        AEnt.ZONE = DSEnt.ZONEID;
                        AEnt.STATE = DSEnt.PROVINCE_ID;
                    }

                    #endregion
                }

                AEnt.VDC_MUNICIPALITY = ddlPAVDC.SelectedValue;
                AEnt.WARD_NO = hf.CapitalizeFirstAndAfterWhitespace(txtPA_WARD_NO.Text);
                AEnt.STREET_NAME = hf.CapitalizeFirstAndAfterWhitespace(txtPA_STREET.Text);
                AEnt.HOUSE_NO = hf.CapitalizeFirstAndAfterWhitespace(txtPA_HOUSE_NO.Text);
                AEnt.ADDRESS_OF = "Employee";
                AEnt.ADDRESS_OF_ID = txtIdNo.Text;
                AEnt.ADDRESS_TYPE = "P";
                AEnt.COUNTRY = "1";
                ASrv.Update(AEnt, DT);
                #endregion
                #endregion
                #endregion
                #endregion

            }

        }

        if (DT.HAPPY)
        {
            DT.Commit();
            
            //ClearField();
            // Response.Redirect("~/humanresource/utilities/Hr_EmployeeList.aspx");

            #region Offer Letter
            string offer_letter;
            try
            {
                string empId = txtIdNo.Text.Trim();
                string empFolderPath = Server.MapPath("~/images/Offer_Letter/" + empId + "/");

                if (!Directory.Exists(empFolderPath))
                {
                    Directory.CreateDirectory(empFolderPath);
                }

                offer_letter = "Offer_Letter_" + his_id + ".pdf";
                if (imgOfferLetter.HasFile)
                {
                    HEHEnt = new HR_EMP_HISTORY();
                    HEHEnt.PK_ID = his_id;
                    HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
                    if (HEHEnt != null)
                    {
                        HEHEnt.OFFER_LETTER = "1";
                        HEHSer.Update(HEHEnt);
                    }
                    string fileExtension = Path.GetExtension(imgOfferLetter.FileName).ToLower();
                    string contentType = imgOfferLetter.PostedFile.ContentType.ToLower();

                    if (fileExtension == ".pdf" && contentType == "application/pdf")
                    {
                        string fullPath = Path.Combine(empFolderPath, offer_letter);
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        imgOfferLetter.SaveAs(fullPath);
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
            #endregion

            #region Agreement
            string agreement;
            try
            {
                string empId = txtIdNo.Text.Trim();
                string empFolderPath = Server.MapPath("~/images/Agreement/" + empId + "/");

                if (!Directory.Exists(empFolderPath))
                {
                    Directory.CreateDirectory(empFolderPath);
                }

                agreement = "Agreement_" + his_id + ".pdf";
                if (imgAgreement.HasFile)
                {
                    HEHEnt = new HR_EMP_HISTORY();
                    HEHEnt.PK_ID = his_id;
                    HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
                    if (HEHEnt != null)
                    {
                        HEHEnt.AGREEMENT = "1";
                        HEHSer.Update(HEHEnt);
                    }
                    string fileExtension = Path.GetExtension(imgAgreement.FileName).ToLower();
                    string contentType = imgAgreement.PostedFile.ContentType.ToLower();

                    if (fileExtension == ".pdf" && contentType == "application/pdf")
                    {
                        string fullPath = Path.Combine(empFolderPath, agreement);
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        imgAgreement.SaveAs(fullPath);
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
            #endregion

            #region Contract
            string contract;
            try
            {
                string empId = txtIdNo.Text.Trim();
                string start_Date = txtStartDate.Text.Replace("/", "-").Trim();
                string end_Date = txtEndDate.Text.Replace("/", "-").Trim();

                string empFolderPath = Server.MapPath("~/images/Contracts/" + empId + "/");

                if (!Directory.Exists(empFolderPath))
                {
                    Directory.CreateDirectory(empFolderPath);
                }

                contract = "Contract_" + his_id + ".pdf";
                if (imgContract.HasFile)
                {
                    HEHEnt = new HR_EMP_HISTORY();
                    HEHEnt.PK_ID = his_id;
                    HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
                    if (HEHEnt != null)
                    {
                        HEHEnt.CONTRACT = "1";
                        HEHSer.Update(HEHEnt);
                    }
                    string fileExtension = Path.GetExtension(imgContract.FileName).ToLower();
                    string contentType = imgContract.PostedFile.ContentType.ToLower();

                    if (fileExtension == ".pdf" && contentType == "application/pdf")
                    {
                        string fullPath = Path.Combine(empFolderPath, contract);
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        imgContract.SaveAs(fullPath);
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
            #endregion
        }
        else
        {
            DT.Abort();
            HelperFunction.MsgBox(this, this.GetType(), "Something went wrong!!");
        }
        DT.Dispose();
    }
    protected void ClearField()
    {
        txtIdNo.Text = "";
        txtFirstname.Text = "";
        txtLastname.Text = "";

        ddlPA_DISTRICT.SelectedIndex = 0;
        ddlPAVDC.SelectedValue = "Select";
        txtPA_WARD_NO.Text = "";
        txtPA_STREET.Text = "";
        txtPA_HOUSE_NO.Text = "";

        ddlTA_DISTRICT.SelectedIndex = 0;
        //ddlTAVDC.SelectedIndex = 0;
        txtTA_WARD_NO.Text = "";
        txtTA_STREET.Text = "";
        txtTA_HOUSE_NO.Text = "";

        txtPhone.Text = "";
        txtMobile1.Text = "";
        txtMobile2.Text = "";
        txtFatherName.Text = "";
        txtGrandFName.Text = "";
        txtEmail.Text = "";


        txtNomineeName.Text = "";
        txtNomineeAdd.Text = "";
        txtNomineeRelation.Text = "";
        txtPfidno.Text = "";
        txtCitno.Text = "";

        ChckIsPayroll.Checked = false;

        txtBasicSal.Text = "";
        txtCitPer.Text = "";

        txtCurrentbankaccount.Text = "";

        txtSecondbankaccount.Text = "";
        //chkStatus.Checked = false;

        txtRemarks.Text = "";

        txtDateBs.Text = pgd.GetTodayNepaliDate();

        txtDateAd.Text = pgd.GetTodayNepaliDate();

        txtAppointDate.Text = pgd.GetTodayNepaliDate();

        txtDevName.Text = "";
        txtMotherName.Text = "";
        txtJobTitle.Text = "";

        txtPfidAmt.Text = "";
        txtBeneficiaryAmt.Text = "";

        rbtnGender.SelectedValue = "M";
        rbtnMaritalStatus.SelectedValue = "1";

        //txtEmpIdP.Text = "";
        //txtEmpNameP.Text = "";
        //txtEmpIdR.Text = "";
        //txtEmpNameR.Text = "";
        //txtDescription.Text = "";
        //txtDesignation.Text = "";

        txtAbbrevation.Text = "";
        ddlPayType.SelectedIndex = 0;

        txtSpouseName.Text = "";

        txtNomineeContact.Text = "";
        txtPANNo.Text = "";
        txtSSFNo.Text = "";
        ddlContactAddress.SelectedIndex = 0;
        LoadAttachment();
        ddlReligion.SelectedIndex = 0;
        ddlBloodGroup.SelectedIndex = 0;


    }
    protected void txtIdNo_TextChanged(object sender, EventArgs e)
    {
        LoadEmployeesDetail(txtIdNo.Text);
    }
    protected void LoadEmployeesDetail(string employeeid)
    {
        EEnt = new HR_EMPLOYEES();
        EEnt.EMPLOYEEID = employeeid;
        EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);

        if (EEnt != null)
        {
            txtIdNo.Text = EEnt.EMPLOYEEID;
            LoadAttachment();
            LoadAdvanceCheck();
            LoadPayableCheck();
            txtFirstname.Text = EEnt.FIRSTNAME;
            txtMiddleName.Text = EEnt.MIDDLE_NAME;
            txtLastname.Text = EEnt.LASTNAME;


            if (EEnt.DEPARTMENTID == "")
            {
                ddlDept.SelectedIndex = 0;
            }
            else
            {
                ddlDept.SelectedValue = EEnt.DEPARTMENTID;
                LoadDesignation();
                LoadDivision();
            }
            if (EEnt.DIVISION == "")
            {
                ddlDivision.SelectedIndex = 0;
            }
            else
            {
                ddlDivision.SelectedValue = EEnt.DIVISION;
                LoadUnit();
            }
            if (EEnt.UNIT == "")
            {
                ddlUnit.SelectedIndex = 0;
            }
            else
            {
                ddlUnit.SelectedValue = EEnt.UNIT;

            }

            if (EEnt.HR_GRADEID == "")
            {
                ddlGrade.SelectedIndex = 0;
            }
            else
            {
                ddlGrade.SelectedValue = EEnt.HR_GRADEID;
            }

            if (EEnt.HR_LEVELID == "")
            {
                ddlLevel.SelectedIndex = 0;
            }
            else
            {
                ddlLevel.SelectedValue = EEnt.HR_LEVELID;
            }


            ddlDesignation.SelectedValue = EEnt.DESIGNATIONID;
            txtJobTitle.Text = EEnt.JOBTITLE;

            txtPhone.Text = EEnt.PHONE;
            txtMobile1.Text = EEnt.MOBILE1;
            txtMobile2.Text = EEnt.MOBILE2;
            txtFatherName.Text = EEnt.FATHERNAME;
            txtGrandFName.Text = EEnt.GRANDFATHERNAME;
            if (EEnt.EMPLOYEETYPE == "")
            {
                ddlEmployeeType.SelectedIndex = 0;
            }
            else
            {
                ddlEmployeeType.SelectedValue = EEnt.EMPLOYEETYPE;
                checkEmpType();
            }

            if (EEnt.GENDER == "M")
            {
                rbtnGender.SelectedValue = "M";
            }
            else if (EEnt.GENDER == "F")
            {
                rbtnGender.SelectedValue = "F";
            }
            if (EEnt.DISABLED != "")
            {
                if (EEnt.DISABLED == "1")
                {
                    txtDisRemark.Visible = true;
                    txtDisRemark.Text = EEnt.DISABLED_REMARKS;
                    rbtnGender.SelectedValue = "1";
                }
                else
                {
                    txtDisRemark.Visible = false;
                    txtDisRemark.Text = "";
                    rbtnGender.SelectedValue = "0";
                }
            }
            rbtnMaritalStatus.SelectedValue = EEnt.MARITALSTATUS;
            if (EEnt.BLOODGROUP != "") { ddlBloodGroup.SelectedValue = EEnt.BLOODGROUP; }
            if (EEnt.RELIGION != "")
            {
                ddlReligion.SelectedValue = EEnt.RELIGION;
            }

            txtNomineeName.Text = EEnt.NOMINEENAME;
            txtNomineeAdd.Text = EEnt.NOMINEEADDRESS;
            txtNomineeRelation.Text = EEnt.NOMINEERELATION;
            txtNomineeContact.Text = EEnt.NOMINEE_CONTACT;
            txtPfidno.Text = EEnt.PFIDNO;
            if (EEnt.INCLUDE_IN_PAYROLL == "1")
            {
                ChckIsPayroll.Checked = true;
            }
            else
            {
                ChckIsPayroll.Checked = false;
            }
            txtCitno.Text = EEnt.CITNO;

            //ChckIsPayroll.Checked = EEnt.IS_INCLUDE_IN_PAYROLL;

            txtEmail.Text = EEnt.EMAIL;
            txtDevName.Text = EEnt.DEVNAGARINAME;
            txtMotherName.Text = EEnt.MOTHERNAME;
            if (EEnt.ETHINITICITY_ID != "")
            {
                ddlEthnicity.SelectedValue = EEnt.ETHINITICITY_ID;
            }
            if (EEnt.CURRENTBANK == "")
            {
                ddlBankName.SelectedIndex = 0;
            }
            else
            {
                ddlBankName.SelectedValue = EEnt.CURRENTBANK;
            }
            txtCurrentbankaccount.Text = EEnt.CURRENTBANKACCNO;

            EADEnt = new HR_EMP_ACC_DETAIL();
            EADEnt.EMPLOYEE_ID = EEnt.EMPLOYEEID;
            EADEnt = (HR_EMP_ACC_DETAIL)EADSrv.GetSingle(EADEnt);
            if (EADEnt != null)
            {
                txtPfidAmt.Text = EADEnt.PF_AMOUNT;
                txtBasicSal.Text = EADEnt.BASIC_SALARY;
                txtBeneficiaryAmt.Text = EADEnt.BENEFICIARY_AMOUNT;
                txtCitPer.Text = EADEnt.CIT_AMOUNT;
            }

            if (EEnt.SECONDBANK == "")
            {
                ddBankName2.SelectedIndex = 0;
            }
            else
            {
                ddBankName2.SelectedValue = EEnt.SECONDBANK;
            }

            txtSecondbankaccount.Text = EEnt.SECONDBANKACCNO;
            txtRemarks.Text = EEnt.REMARKS;

            txtDateBs.Text = EEnt.BIRTHDAY + "/" + EEnt.BIRTHMONTH + "/" + EEnt.BIRTHYEAR;

            GetEnglishDate();

            txtAppointDate.Text = EEnt.APPOINTMENTDAY + "/" + EEnt.APPOINTMENTMONTH + "/" + EEnt.APPOINTMENTYEAR;
            txtAbbrevation.Text = EEnt.ABBREVATION;
            txtSpouseName.Text = EEnt.SPOUSENAME;
            txtNomineeContact.Text = EEnt.NOMINEE_CONTACT;
            txtPANNo.Text = EEnt.PANNO;
            txtSSFNo.Text = EEnt.SSFNO;


            txtAcdName.Text = EEnt.ACCIDENT_CONTACT_NAME;
            txtAcdAddr.Text = EEnt.ACCIDENT_CONTACT_ADDRESS;
            txtAcdRelation.Text = EEnt.ACCIDENT_CONTACT_RELATION;
            txtAcdContact.Text = EEnt.ACCIDENT_CONTACT_NUM;

            #region Address

            AEnt = new ADDRESS();
            AEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
            AEnt.ADDRESS_OF = "Employee";
            AEnt.ADDRESS_TYPE = "Both";
            AEnt = (ADDRESS)ASrv.GetSingle(AEnt);
            if (AEnt != null)
            {
                txtPA_HOUSE_NO.Text = AEnt.HOUSE_NO;
                txtPA_STREET.Text = AEnt.STREET_NAME;

                txtPA_WARD_NO.Text = AEnt.WARD_NO;


                if (AEnt.DISTRICT == "")
                {
                    ddlPA_DISTRICT.SelectedIndex = 0;

                }
                else
                {
                    ddlPA_DISTRICT.SelectedValue = AEnt.DISTRICT;
                    LoadLocalPABodyType();
                    ddlPAVDC.SelectedValue = AEnt.VDC_MUNICIPALITY;
                }

                //if (AEnt.ADDRESS_TYPE == "Both")
                //{
                ddlContactAddress.SelectedIndex = 1;
                //}
                //else
                //{
                //    ddlContactAddress.SelectedIndex = 0;
                //}

                txtTA_HOUSE_NO.Text = AEnt.HOUSE_NO;
                txtTA_STREET.Text = AEnt.STREET_NAME;

                txtTA_WARD_NO.Text = AEnt.WARD_NO;

                if (AEnt.DISTRICT == "")
                {
                    ddlTA_DISTRICT.SelectedIndex = 0;
                }
                else
                {
                    ddlTA_DISTRICT.SelectedValue = AEnt.DISTRICT;
                    LoadLocalTABodyType();
                    ddlTAVDC.SelectedValue = AEnt.VDC_MUNICIPALITY;
                }
            }


            AEnt = new ADDRESS();
            AEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
            AEnt.ADDRESS_OF = "Employee";
            AEnt.ADDRESS_TYPE = "P";
            AEnt = (ADDRESS)ASrv.GetSingle(AEnt);
            if (AEnt != null)
            {
                if (AEnt.COUNTRY != "1")
                {
                    lblDistrict_PA.Visible = false;
                    lblSameAsOf.Visible = false;

                }
                else
                {
                    lblDistrict_PA.Visible = true;
                    lblSameAsOf.Visible = true;
                }

                txtPA_HOUSE_NO.Text = AEnt.HOUSE_NO;
                txtPA_STREET.Text = AEnt.STREET_NAME;
                ddlPAVDC.SelectedValue = AEnt.VDC_MUNICIPALITY;
                txtPA_WARD_NO.Text = AEnt.WARD_NO;

                if (AEnt.DISTRICT == "")
                {
                    ddlPA_DISTRICT.SelectedIndex = 0;
                }
                else
                {
                    ddlPA_DISTRICT.SelectedValue = AEnt.DISTRICT;
                    LoadLocalPABodyType();
                    ddlPAVDC.SelectedValue = AEnt.VDC_MUNICIPALITY;
                }

            }

            AEnt = new ADDRESS();
            AEnt.ADDRESS_OF_ID = EEnt.EMPLOYEEID;
            AEnt.ADDRESS_OF = "Employee";
            AEnt.ADDRESS_TYPE = "C";
            AEnt = (ADDRESS)ASrv.GetSingle(AEnt);
            if (AEnt != null)
            {

                txtTA_HOUSE_NO.Text = AEnt.HOUSE_NO;
                txtTA_STREET.Text = AEnt.STREET_NAME;
                txtTA_WARD_NO.Text = AEnt.WARD_NO;

                if (AEnt.DISTRICT == "")
                {
                    ddlTA_DISTRICT.SelectedIndex = 0;
                }
                else
                {

                    ddlTA_DISTRICT.SelectedValue = AEnt.DISTRICT;
                    LoadLocalTABodyType();
                    ddlTAVDC.SelectedValue = AEnt.VDC_MUNICIPALITY;
                }

            }

            #endregion
        }
        else
        {
            string tempid = txtIdNo.Text;
            //ClearField();
            txtIdNo.Text = tempid;
        }

    }
    protected void btnReview_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((Button)sender).Parent.Parent as GridViewRow;
        Label lblAttDoc = gr.FindControl("lblAttDoc") as Label;
        Label lblPkId = gr.FindControl("lblPkId") as Label;
        string Doc_Name = lblAttDoc.Text.Replace(" ", "_");
        Response.Write("<script>window.open ('" + "../images/Attachment_Staff/Emp_" + Doc_Name + "/" + txtIdNo.Text + ".pdf" + "','_blank');</script>");

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
            TextBox txtDocumentNum = e.Row.FindControl("txtDocumentNum") as TextBox;
            TextBox txtIssueDate = e.Row.FindControl("txtIssueDate") as TextBox;
            TextBox txtExpiryDate = e.Row.FindControl("txtExpiryDate") as TextBox;
            TextBox txtIssuedBy = e.Row.FindControl("txtIssuedBy") as TextBox;

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
            ATEnt.ATTACHMENT_OF_ID = txtIdNo.Text;
            ATEnt = (HR_ATTACHMENT)ATSer.GetSingle(ATEnt);
            if (ATEnt != null)
            {
                if (ATEnt.DOCUMENT_NUMBER != "")
                {
                    txtDocumentNum.Text = ATEnt.DOCUMENT_NUMBER;
                }

                if (ATEnt.ISSUE_DATE != "")
                {
                    txtIssueDate.Text = ATEnt.ISSUE_DATE;
                }

                if (ATEnt.EXPIRY_DATE != "")
                {
                    txtExpiryDate.Text = ATEnt.EXPIRY_DATE;
                }

                if (ATEnt.ISSUED_BY != "")
                {
                    txtIssuedBy.Text = ATEnt.ISSUED_BY;
                }


                Button btnReview = e.Row.FindControl("btnReview") as Button;
                if (lblAttDocOf.Text != "")
                {
                    ATEnt = new HR_ATTACHMENT();
                    ATEnt.ATTACHMENT_OF_ID = txtIdNo.Text;

                    theList = ATSer.GetAll(ATEnt);
                    if (theList.Count > 0)
                    {
                        btnReview.Visible = true;
                    }

                }
                else
                {
                    btnReview.Visible = false;
                }
            }

        }
    }
    protected void ddlTA_DISTRICT_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadLocalTABodyType();
    }

    protected void ddlPA_DISTRICT_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadLocalPABodyType();
        if (ddlPAVDC.SelectedValue != "Select")
        {
            AdressSameSet();
        }
    }

    protected void ddlContactAddress_SelectedIndexChanged(object sender, EventArgs e)
    {
        AdressSameSet();

    }

    protected void AdressSameSet()
    {
        if (ddlContactAddress.SelectedValue != "Select")
        {

            lblDistrict_CA.Visible = true;

            ddlTA_DISTRICT.Enabled = false;
            ddlTAVDC.Enabled = false;
            txtTA_WARD_NO.Enabled = false;
            txtTA_STREET.Enabled = false;
            txtTA_HOUSE_NO.Enabled = false;

            LoadTADistric();

            ddlTA_DISTRICT.SelectedValue = ddlPA_DISTRICT.SelectedValue;

            LoadLocalTABodyType();
            ddlTAVDC.SelectedValue = ddlPAVDC.SelectedValue;
            txtTA_WARD_NO.Text = txtPA_WARD_NO.Text;
            txtTA_STREET.Text = txtPA_STREET.Text;
            txtTA_HOUSE_NO.Text = txtPA_HOUSE_NO.Text;



        }
        else
        {
            ddlTA_DISTRICT.Enabled = true;
            ddlTAVDC.Enabled = true;
            txtTA_WARD_NO.Enabled = true;
            txtTA_STREET.Enabled = true;
            txtTA_HOUSE_NO.Enabled = true;


            lblDistrict_CA.Visible = true;


            ddlTA_DISTRICT.SelectedIndex = 0;
            LoadLocalTABodyType();
            //ddlTAVDC.SelectedIndex = 0;
            txtTA_WARD_NO.Text = "";
            txtTA_STREET.Text = "";
            txtTA_HOUSE_NO.Text = "";


        }
    }
    protected void ddlPAVDC_SelectedIndexChanged(object sender, EventArgs e)
    {
        AdressSameSet();
    }

    protected void txtPA_STREET_TextChanged(object sender, EventArgs e)
    {
        AdressSameSet();
    }

    protected void txtPA_WARD_NO_TextChanged(object sender, EventArgs e)
    {
        AdressSameSet();
    }

    protected void txtPA_HOUSE_NO_TextChanged(object sender, EventArgs e)
    {
        AdressSameSet();
    }

    protected void ddlEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkEmpType();
        if (ddlEmployeeType.SelectedItem.Text == "Contract")
            tdContract.Visible = true;
        else
            tdContract.Visible = false;
    }
    protected void checkEmpType()
    {
        if (ddlEmployeeType.SelectedItem.Text == "Contract")
        {
            txtConStDate.Visible = true;
            txtConEndDate.Visible = true;
        }
        else
        {
            txtConStDate.Visible = false;
            txtConEndDate.Visible = false;
        }
    }
}
