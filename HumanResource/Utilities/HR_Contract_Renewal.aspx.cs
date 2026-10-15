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
using System.Collections;

public partial class HumanResource_Utilities_HR_Contract_Renewal : System.Web.UI.Page
{
    string imgfolder;

    HelperFunction hf = new HelperFunction();

    HR_EMP_CONTRACT HECEnt = new HR_EMP_CONTRACT();
    HR_EMP_CONTRACTService HECSer = new HR_EMP_CONTRACTService();

    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    HR_EMPLOYEES EEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService ESer = new HR_EMPLOYEESService();

    HR_DESIGNATION HDEnt = new HR_DESIGNATION();
    HR_DESIGNATIONService HDSer = new HR_DESIGNATIONService();

    HR_DIVISION HDIVEnt = new HR_DIVISION();
    HR_DIVISIONService HDIVSer = new HR_DIVISIONService();

    HR_LEVEL HLEnt = new HR_LEVEL();
    HR_LEVELService HLSer = new HR_LEVELService();

    HR_UNIT HUEnt = new HR_UNIT();
    HR_UNITService HUSer = new HR_UNITService();

    HR_EMP_HISTORY HEHEnt = new HR_EMP_HISTORY();
    HR_EMP_HISTORYService HEHSer = new HR_EMP_HISTORYService();

    EntityList theEntList = new EntityList();

    PhyeGanDate pgd = new PhyeGanDate();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            UserProfileEntity userProfileEnt = new UserProfileEntity();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

            LoadData();
            LoadDesignation();
            loadLevel();
            txtStartDateE.Text = pgd.GetTodayNepaliDate();
            txtEndDateE.Text = pgd.GetTodayNepaliDate();

            // LoadGridData();

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

        ddlDepartmentE.DataSource = DPSer.GetAll(DPEnt);
        ddlDepartmentE.DataTextField = "DEP_NAME";
        ddlDepartmentE.DataValueField = "PK_ID";
        ddlDepartmentE.DataBind();
        ddlDepartmentE.Items.Insert(0, "Select");

    }

    protected void loadLevel()
    {
        HLEnt = new HR_LEVEL();
        HLEnt.STATUS = "1";
        ddlLevelE.DataSource = HLSer.GetAll(HLEnt);
        ddlLevelE.DataTextField = "LEVEL_NAME";
        ddlLevelE.DataValueField = "PK_ID";
        ddlLevelE.DataBind();
        ddlLevelE.Items.Insert(0, "Select");
    }

    protected void ddlDept_SelectedIndexChanged1(object sender, EventArgs e)
    {
        LoadDesignation();
    }
    protected void ddlDepartmentE_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDesignation();
    }

    protected void LoadDesignation()
    {
        HDEnt = new HR_DESIGNATION();
        if (ddlDept.SelectedValue != "Select")
            HDEnt.DEPARTMENT_ID = ddlDept.SelectedValue;
        else
            HDEnt.DEPARTMENT_ID = "";
        ddlDesignation.DataSource = HDSer.GetAll(HDEnt);
        ddlDesignation.DataTextField = "DESIGNATION_NAME";
        ddlDesignation.DataValueField = "PK_ID";
        ddlDesignation.DataBind();
        ddlDesignation.Items.Insert(0, "Select");

        ddlDesignationE.DataSource = HDSer.GetAll(HDEnt);
        ddlDesignationE.DataTextField = "DESIGNATION_NAME";
        ddlDesignationE.DataValueField = "PK_ID";
        ddlDesignationE.DataBind();
        ddlDesignationE.Items.Insert(0, "Select");

    }

    protected void bttnSearch_Click(object sender, EventArgs e)
    {
        loadAllContract();
        tblGrid.Visible = true;
    }
    protected void loadAllContract()
    {
        string depid = "";
        string desId = "";
        if (ddlDept.SelectedValue != "Select")
            depid = ddlDept.SelectedValue;

        if (ddlDesignation.SelectedValue != "Select")
            desId = ddlDesignation.SelectedValue;

        grdAllContract.DataSource = hf.getAllContractDetails(pgd.GetTodayNepaliDate(), depid, desId, ddlStatus.SelectedValue);
        grdAllContract.DataBind();
    }
    protected void grdAllContract_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblStDate_A = e.Row.FindControl("lblStDate_A") as Label;
            Label lblEndDate_A = e.Row.FindControl("lblEndDate_A") as Label;

            DateTime dt;
            if (DateTime.TryParse(lblStDate_A.Text, out dt))
            {
                lblStDate_A.Text = dt.ToString("dd/MM/yyyy");
            }

            if (DateTime.TryParse(lblEndDate_A.Text, out dt))
            {
                lblEndDate_A.Text = dt.ToString("dd/MM/yyyy");
            }
        }
    }

    protected void ibtnRenew_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID_A");


        EmpSearch.Visible = false;
        tblGrid.Visible = false;
        tblEmpDetail.Visible = true;
        loadEmpDetail(lblEmpID.Text);
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
            HECEnt = new HR_EMP_CONTRACT();
            HECEnt.EMP_ID = empId;
            HECEnt.STATUS = "1";
            HECEnt = (HR_EMP_CONTRACT)HECSer.GetSingle(HECEnt);
            if (HECEnt != null)
            {
                DPEnt = new HR_DEPARTMENT();
                DPEnt.PK_ID = HECEnt.HR_DEPARTMENT_ID;
                DPEnt = (HR_DEPARTMENT)DPSer.GetSingle(DPEnt);
                if (DPEnt != null)
                {
                    lblDepartment.Text = DPEnt.DEP_NAME;
                    ddlDepartmentE.SelectedValue = DPEnt.PK_ID;
                    LoadDesignation();
                }

                //Current Position
                if (HECEnt.HR_DESIGNATION_ID != "")
                {
                    HDEnt = new HR_DESIGNATION();
                    HDEnt.PK_ID = HECEnt.HR_DESIGNATION_ID;
                    HDEnt = (HR_DESIGNATION)HDSer.GetSingle(HDEnt);
                    if (HDEnt != null)
                    {
                        lblDesignation.Text = HDEnt.DESIGNATION_NAME;
                        ddlDesignationE.SelectedValue = HDEnt.PK_ID;
                    }
                }
                else
                {
                    lblDesignation.Text = "";
                    ddlDesignationE.SelectedValue = "Select";
                }

                HLEnt = new HR_LEVEL();
                HLEnt.PK_ID = HECEnt.HR_LEVEL_ID;
                HLEnt = (HR_LEVEL)HLSer.GetSingle(HLEnt);
                if (HLEnt != null)
                {
                    lblLevel.Text = HLEnt.LEVEL_NAME;
                    ddlLevelE.SelectedValue = HLEnt.PK_ID;
                }

                lblStartDate.Text = HECEnt.START_DAY + "/" + HECEnt.START_MONTH + "/" + HECEnt.START_YEAR;
                lblEndDate.Text = HECEnt.END_DAY + "/" + HECEnt.END_MONTH + "/" + HECEnt.END_YEAR;

            }

            imgfolder = Server.MapPath(@"~/images/Employee/") + empId + ".jpg";
            if (File.Exists(imgfolder))
            {
                imgEmpPhotoD.ImageUrl = @"~/images/Employee/" + empId + ".jpg";
            }
            else
            {
                if (EEnt.GENDER == "M")
                    imgEmpPhotoD.ImageUrl = @"~/images/Employee/male.jpg";
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DistributedTransaction DT = new DistributedTransaction();
        string history_id = "";
        HECEnt.EMP_ID = lblEmployeeID.Text;
        HECEnt.STATUS = "1";
        HECEnt = (HR_EMP_CONTRACT)HECSer.GetSingle(HECEnt);
        if (HECEnt != null)
        {
            HECEnt.STATUS = "0";
            HECSer.Update(HECEnt, DT);
        }

        HECEnt = new HR_EMP_CONTRACT();
        HECEnt.EMP_ID = lblEmployeeID.Text;
        if (ddlDepartmentE.SelectedValue != "Select")
            HECEnt.HR_DEPARTMENT_ID = ddlDepartmentE.SelectedValue;
        else
            HECEnt.HR_DEPARTMENT_ID = "";

        if (ddlDesignationE.SelectedValue != "Select")
            HECEnt.HR_DESIGNATION_ID = ddlDesignationE.SelectedValue;
        else
            HECEnt.HR_DESIGNATION_ID = "";

        if (ddlLevelE.SelectedValue != "Select")
            HECEnt.HR_LEVEL_ID = ddlLevelE.SelectedValue;
        else
            HECEnt.HR_LEVEL_ID = "";

        string[] stDate = txtStartDateE.Text.Split('/');
        HECEnt.START_DATE = pgd.ConvertNepaliTOEnglish(stDate[0], stDate[1], stDate[2]);
        HECEnt.START_DAY = stDate[0];
        HECEnt.START_MONTH = stDate[1];
        HECEnt.START_YEAR = stDate[2];

        string[] endDate = txtEndDateE.Text.Split('/');
        HECEnt.END_DATE = pgd.ConvertNepaliTOEnglish(endDate[0], endDate[1], endDate[2]);
        HECEnt.END_DAY = endDate[0];
        HECEnt.END_MONTH = endDate[1];
        HECEnt.END_YEAR = endDate[2];
        HECEnt.STATUS = "1";
        HECSer.Insert(HECEnt, DT);

        EEnt = new HR_EMPLOYEES();
        EEnt.EMPLOYEEID = lblEmployeeID.Text;
        EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
        if (EEnt != null)
        {
            if (ddlDepartmentE.SelectedValue != "Select")
                EEnt.DEPARTMENTID = ddlDept.SelectedValue;
            else
                EEnt.DEPARTMENTID = "";

            if (ddlDesignationE.SelectedValue != "Select")
                EEnt.DESIGNATIONID = ddlDesignation.SelectedValue;
            else
                EEnt.DESIGNATIONID = "";

            if (ddlLevelE.SelectedValue != "Select")
                EEnt.HR_LEVELID = ddlLevelE.SelectedValue;
            else
                EEnt.HR_LEVELID = "";

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
        HEHEnt.HR_DEPARTMENT_ID = ddlDepartmentE.SelectedValue;
        HEHEnt.HR_DIVISION_ID = ddlDepartmentE.SelectedValue;

        if (ddlDesignationE.SelectedValue != "Select")
            HEHEnt.HR_DESIGNATION_ID = ddlDesignationE.SelectedValue;
        else
            HEHEnt.HR_DESIGNATION_ID = "";

        if (ddlLevelE.SelectedValue != "Select")
            HEHEnt.HR_LEVEL_ID = ddlLevelE.SelectedValue;
        else
            HEHEnt.HR_LEVEL_ID = "";
      
        HEHEnt.EFFECTIVE_DATE = pgd.ConvertNepaliTOEnglish(stDate[0], stDate[1], stDate[2]);
        HEHEnt.EFFECTIVE_DAY = stDate[0];
        HEHEnt.EFFECTIVE_MONTH = stDate[1];
        HEHEnt.EFFECTIVE_YEAR = stDate[2];
        HEHEnt.CONTRACT = "1";
        HEHEnt.STATUS = "1";

        history_id = HEHSer.Insert(HEHEnt, DT).ToString();

        if (DT.HAPPY)
        {
            DT.Commit();

            string contract;
            try
            {
                string empId = lblEmployeeID.Text.Trim();
                string empFolderPath = Server.MapPath("~/images/Contracts/" + empId + "/");

                if (!Directory.Exists(empFolderPath))
                {
                    Directory.CreateDirectory(empFolderPath);
                }

                contract = "Contract_" + history_id + ".pdf";
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
            HelperFunction.MsgBox(this, this.GetType(), "Employee Updated");
            loadEmpDetail(lblEmployeeID.Text);
        }
        else
        {
            DT.Abort();
            HelperFunction.MsgBox(this, this.GetType(), "Something went wrong!!");
        }
        DT.Dispose();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearField();
        tblEmpDetail.Visible = false;
        tblGrid.Visible = false;
        EmpSearch.Visible = true;
    }

    protected void ClearField()
    {
        lblEmployeeID.Text = "";
        ddlDept.SelectedValue = "Select";
        ddlDesignation.SelectedValue = "Select";
        ddlStatus.SelectedValue = "1";
        ddlDepartmentE.SelectedValue = "Select";
        ddlDesignationE.SelectedValue = "Select";
        ddlLevelE.SelectedValue = "Select";
    }
}