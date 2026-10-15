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
using System.Data;

public partial class HumanResource_Utilities_HR_Emp_Training : System.Web.UI.Page
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

    HR_EMP_TRAINING HETREnt = new HR_EMP_TRAINING();
    HR_EMP_TRAININGService HETRSer = new HR_EMP_TRAININGService();

    HelperFunction hf = new HelperFunction();
    EntityList theEntList = new EntityList();

    PhyeGanDate pgd = new PhyeGanDate();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserProfileEntity userProfileEnt = new UserProfileEntity();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

            LoadData();
            txtTrainingDate.Text = pgd.GetTodayNepaliDate();
            txtCertificateDate.Text = pgd.GetTodayNepaliDate();
            // LoadGridData();
            //DetailEmployee.Visible = false;
            //print.Visible = false;
            //attach.Visible = false;
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

    protected void ddlDept_SelectedIndexChanged1(object sender, EventArgs e)
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        tblAddTrainig.Visible = true;
        tblEmpDetail.Visible = false;
        EmpSearch.Visible = false;
        GridViewRow row = (GridViewRow)((Button)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");

        lblPK_ID.Text = null;
        loadEmployeeForAdd(lblEmpID.Text);

    }

    protected void btnAddMore_Click(object sender, EventArgs e)
    {
        tblAddTrainig.Visible = true;
        EmpSearch.Visible = false;
        tblEmpDetail.Visible = false;
        lblPK_ID.Text = null;
        loadEmployeeForAdd(lblEmployeeID.Text);
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        tblAddTrainig.Visible = false;
        tblEmpDetail.Visible = true;
        EmpSearch.Visible = false;
        GridViewRow row = (GridViewRow)((Button)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");
        loadEmployeeView(lblEmpID.Text);

    }

    protected void loadEmployeeForAdd(string empId)
    {
        EEnt = new HR_EMPLOYEES();
        EEnt.EMPLOYEEID = empId;
        EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
        if (EEnt != null)
        {
            txtEmpId.Text = EEnt.EMPLOYEEID;
            txtEmpName.Text = EEnt.FULL_NAME;
        }
    }

    protected void loadEmployeeView(string empId)
    {
        #region Employee Details
        EEnt = new HR_EMPLOYEES();
        EEnt.EMPLOYEEID = empId;
        EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
        if (EEnt != null)
        {
            lblEmployeeID.Text = EEnt.EMPLOYEEID;
            lblFullName.Text = EEnt.FULL_NAME;
            lblDeptID.Text = EEnt.DEPARTMENTID;
            lblDivId.Text = EEnt.DIVISION;
            lblUnitId.Text = EEnt.UNIT;

            DPEnt = new HR_DEPARTMENT();
            DPEnt.PK_ID = lblDeptID.Text;
            DPEnt = (HR_DEPARTMENT)DPSer.GetSingle(DPEnt);
            if (DPEnt != null)
            {
                lblPosition.Text = DPEnt.DEP_NAME;

                HDIVEnt = new HR_DIVISION();
                HDIVEnt.PK_ID = lblDivId.Text;
                HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
                if (HDIVEnt != null)
                {
                    lblPosition.Text = DPEnt.DEP_NAME + "/" + HDIVEnt.DIVISION_NAME;
                }

                if (lblUnitId.Text != "")
                {
                    HUEnt = new HR_UNIT();
                    HUEnt.PK_ID = lblUnitId.Text;
                    HUEnt = (HR_UNIT)HUSer.GetSingle(HUEnt);
                    if (HUEnt != null)
                    {
                        lblPosition.Text = DPEnt.DEP_NAME + "/" + HDIVEnt.DIVISION_NAME + "/" + HUEnt.UNIT_NAME;

                    }
                }
            }
            HLEnt = new HR_LEVEL();
            HLEnt.PK_ID = EEnt.HR_LEVELID;
            HLEnt = (HR_LEVEL)HLSer.GetSingle(HLEnt);
            if (HLEnt != null)
            {
                lblLevel.Text = HLEnt.LEVEL_NAME;
            }

            HGEnt = new HR_GRADE();
            HGEnt.PK_ID = EEnt.HR_GRADEID;
            HGEnt = (HR_GRADE)HGSer.GetSingle(HGEnt);
            if (HGEnt != null)
            {
                lblGrade.Text = HGEnt.GRADE_NAME;
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
        #endregion

        #region grid
        HETREnt = new HR_EMP_TRAINING();
        HETREnt.EMP_ID = empId;
        var data = HETRSer.GetAll(HETREnt); // Returns EntityList<HR_EMP_TRAINING>

        grdEmpTraining.DataSource = data;
        grdEmpTraining.DataBind();

        lblMessage.Text = "";

        // Check if the data is empty
        if (data == null || data.Count == 0)
        {
            grdEmpTraining.DataSource = null;
            grdEmpTraining.DataBind();
            lblMessage.Text = "No training records found.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }

        #endregion
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        tblAddTrainig.Visible = false;
        tblEmpDetail.Visible = false;
        EmpSearch.Visible = true;
    }

    protected void btnBackAdd_Click(object sender, EventArgs e)
    {
        tblAddTrainig.Visible = false;
        tblEmpDetail.Visible = false;
        EmpSearch.Visible = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtTrainingTitle.Text != "" && txtTrainingDate.Text != "" && txtCertificateDate.Text != "")
        {
            if (string.IsNullOrEmpty(lblPK_ID.Text))
            {
                HETREnt = new HR_EMP_TRAINING();
                HETREnt.EMP_ID = txtEmpId.Text;
                HETREnt.TRAINING_TITLE = txtTrainingTitle.Text;
                HETREnt.TRAINING_DETAILS = txtTrainingDetails.Text;

                string[] tNPDate = txtTrainingDate.Text.Split('/');
                HETREnt.TRAINING_DAY = tNPDate[0];
                HETREnt.TRAINING_MONTH = tNPDate[1];
                HETREnt.TRAINING_YEAR = tNPDate[2];
                HETREnt.TRAINING_DATE = pgd.ConvertNepaliTOEnglish(tNPDate[0], tNPDate[1], tNPDate[2]);

                string[] cNPDate = txtCertificateDate.Text.Split('/');
                HETREnt.CERTIFICATE_DAY = cNPDate[0];
                HETREnt.CERTIFICATE_MONTH = cNPDate[1];
                HETREnt.CERTIFICATE_YEAR = cNPDate[2];
                HETREnt.CERTIFICATE_DATE = pgd.ConvertNepaliTOEnglish(cNPDate[0], cNPDate[1], cNPDate[2]);

                HETREnt.STATUS = "1";

                if (chkInhouse.Checked == true)
                {
                    HETREnt.IN_HOUSE_STATUS = "1";
                }
                else
                {
                    HETREnt.IN_HOUSE_STATUS = "0";
                    HETREnt.ORG_NAME = txtOrgName.Text;
                }

                lblPK_ID.Text = HETRSer.Insert(HETREnt).ToString();
                HelperFunction.MsgBox(this, this.GetType(), "Data Added !!");
            }
            else
            {
                HETREnt = new HR_EMP_TRAINING();
                HETREnt.PK_ID = lblPK_ID.Text;
                HETREnt = (HR_EMP_TRAINING)HETRSer.GetSingle(HETREnt);
                if (HETREnt != null)
                {
                    HETREnt.TRAINING_TITLE = txtTrainingTitle.Text;
                    HETREnt.TRAINING_DETAILS = txtTrainingDetails.Text;

                    string[] tNPDate = txtTrainingDate.Text.Split('/');
                    HETREnt.TRAINING_DAY = tNPDate[0];
                    HETREnt.TRAINING_MONTH = tNPDate[1];
                    HETREnt.TRAINING_YEAR = tNPDate[2];
                    HETREnt.TRAINING_DATE = pgd.ConvertNepaliTOEnglish(tNPDate[0], tNPDate[1], tNPDate[2]);

                    string[] cNPDate = txtCertificateDate.Text.Split('/');
                    HETREnt.CERTIFICATE_DAY = cNPDate[0];
                    HETREnt.CERTIFICATE_MONTH = cNPDate[1];
                    HETREnt.CERTIFICATE_YEAR = cNPDate[2];
                    HETREnt.CERTIFICATE_DATE = pgd.ConvertNepaliTOEnglish(cNPDate[0], cNPDate[1], cNPDate[2]);

                    HETREnt.STATUS = "1";

                    if (chkInhouse.Checked)
                    {
                        HETREnt.IN_HOUSE_STATUS = "1";
                    }
                    else
                    {
                        HETREnt.IN_HOUSE_STATUS = "0";
                        HETREnt.ORG_NAME = txtOrgName.Text;
                    }

                    HETRSer.Update(HETREnt);
                    HelperFunction.MsgBox(this, this.GetType(), "Data Updated !!");
                }
            }

            string certificate;
            try
            {
                string empId = txtEmpId.Text.Trim();
                string trainingName = txtTrainingTitle.Text;
                string certificateDate = txtCertificateDate.Text.Replace("/", "-").Trim();

                string empFolderPath = Server.MapPath("~/images/Certificates/" + empId + "/");

                if (!Directory.Exists(empFolderPath))
                {
                    Directory.CreateDirectory(empFolderPath);
                }

                certificate = "Certificate_" + lblPK_ID.Text + ".pdf";
                if (fileCertificate.HasFile)
                {
                    string fileExtension = Path.GetExtension(fileCertificate.FileName).ToLower();
                    string contentType = fileCertificate.PostedFile.ContentType.ToLower();

                    if (fileExtension == ".pdf" && contentType == "application/pdf")
                    {
                        string fullPath = Path.Combine(empFolderPath, certificate);
                        fileCertificate.SaveAs(fullPath);
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


            tblAddTrainig.Visible = false;
            tblEmpDetail.Visible = true;
            EmpSearch.Visible = false;
            loadEmployeeView(txtEmpId.Text);

        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please enter all required fields !!");
        }
        clearAddField();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearAddField();
        tblAddTrainig.Visible = false;
        tblEmpDetail.Visible = true;
        EmpSearch.Visible = false;
        loadEmployeeView(txtEmpId.Text);
    }

    protected void clearAddField()
    {
        lblPK_ID.Text = "";
        txtTrainingTitle.Text = "";
        txtTrainingDate.Text = pgd.GetTodayNepaliDate();
        txtCertificateDate.Text = pgd.GetTodayNepaliDate();
        txtTrainingDetails.Text = "";
    }

    protected void ibtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        tblAddTrainig.Visible = true;
        EmpSearch.Visible = false;
        tblEmpDetail.Visible = false;
        GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");
        Label lblPK_IDG = (Label)row.FindControl("lblPK_IDG");

        lblPK_ID.Text = lblPK_IDG.Text;
        HETREnt = new HR_EMP_TRAINING();
        HETREnt.PK_ID = lblPK_IDG.Text;
        HETREnt = (HR_EMP_TRAINING)HETRSer.GetSingle(HETREnt);
        if (HETREnt != null)
        {

            EEnt = new HR_EMPLOYEES();
            EEnt.EMPLOYEEID = HETREnt.EMP_ID;
            EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
            if (EEnt != null)
            {
                txtEmpId.Text = EEnt.EMPLOYEEID;
                txtEmpName.Text = EEnt.FULL_NAME;
            }
            txtTrainingTitle.Text = HETREnt.TRAINING_TITLE;
            txtTrainingDetails.Text = HETREnt.TRAINING_DETAILS;
            txtTrainingDate.Text = HETREnt.TRAINING_DAY + "/" + HETREnt.TRAINING_MONTH + "/" + HETREnt.TRAINING_YEAR;
            txtCertificateDate.Text = HETREnt.CERTIFICATE_DAY + "/" + HETREnt.CERTIFICATE_MONTH + "/" + HETREnt.CERTIFICATE_YEAR;
            if (HETREnt.IN_HOUSE_STATUS == "1")
            {
                chkInhouse.Checked = true;
            }
            else
            {
                chkInhouse.Checked = false;
            }
            checkCheckBox();

        }
    }

    protected void grdEmpTraining_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblPK_IDG = e.Row.FindControl("lblPK_IDG") as Label;
            Label lblNepTDate = e.Row.FindControl("lblNepTDate") as Label;
            Label lblNepCDate = e.Row.FindControl("lblNepCDate") as Label;
            Label lblInHouse = e.Row.FindControl("lblInHouse") as Label;
            Label lblShowInHouse = e.Row.FindControl("lblShowInHouse") as Label;

            HETREnt = new HR_EMP_TRAINING();
            HETREnt.PK_ID = lblPK_IDG.Text;
            HETREnt = (HR_EMP_TRAINING)HETRSer.GetSingle(HETREnt);
            if (HETREnt != null)
            {
                lblNepTDate.Text = HETREnt.TRAINING_DAY + "/" + HETREnt.TRAINING_MONTH + "/" + HETREnt.TRAINING_YEAR;
                lblNepCDate.Text = HETREnt.CERTIFICATE_DAY + "/" + HETREnt.CERTIFICATE_MONTH + "/" + HETREnt.CERTIFICATE_YEAR;
            }

            if (lblInHouse.Text == "1")
            {
                lblShowInHouse.Text = "Yes";
            }
            else
            {
                lblShowInHouse.Text = "No";
            }
        }
    }


    protected void chkInhouse_CheckedChanged(object sender, EventArgs e)
    {
        checkCheckBox();
    }

    protected void checkCheckBox()
    {
        if (chkInhouse.Checked)
            trTrName.Visible = false;
        else
            trTrName.Visible = true;

    }

    protected void imgBtnPdf_Click(object sender, ImageClickEventArgs e)
    {
        string empId = lblEmployeeID.Text.Trim();
        HEHEnt = new HR_EMP_HISTORY();
        HEHEnt.EMP_ID = empId;
        HEHEnt.CONTRACT = "1";
        HEHEnt.STATUS = "1";
        HEHEnt = (HR_EMP_HISTORY)HEHSer.GetSingle(HEHEnt);
        if (HEHEnt != null)
        {

            string pdfFileName = "Contract_" + HEHEnt.PK_ID + ".pdf";
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


    protected void grdEmpTraining_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("View"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_IDG = gr.FindControl("lblPK_IDG") as Label;

            string pdfFileName = "Certificate_" + lblPK_IDG.Text + ".pdf";
            string relativeUrl = "~/images/Certificates/" + lblEmployeeID.Text + "/" + pdfFileName;

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