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

public partial class human_resource_Utilities_EmployeeTermination : System.Web.UI.Page
{
    HR_EMPLOYEES EEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService ESer = new HR_EMPLOYEESService();

    HrEmployeeTermination ETEnt = new HrEmployeeTermination();
    hrEmployeeTerminationService ETSer = new hrEmployeeTerminationService();

    Entity.Components.Login theLEntity = new Entity.Components.Login();
    LoginService theLService = new LoginService();

    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    HR_DESIGNATION DTEnt = new HR_DESIGNATION();
    HR_DESIGNATIONService DTSer = new HR_DESIGNATIONService();

    HrEmployeeTerminationType ETTEnt = new HrEmployeeTerminationType();
    HR_EMP_TERMINATION_TYPEService ETTSer = new HR_EMP_TERMINATION_TYPEService();

    HR_DIVISION HDIVEnt = new HR_DIVISION();
    HR_DIVISIONService HDIVSer = new HR_DIVISIONService();

    HR_UNIT HUEnt = new HR_UNIT();
    HR_UNITService HUSer = new HR_UNITService();

    UserProfileEntity userProfileEnt = new UserProfileEntity();

    //PhyeGanCore PG = new PhyeGanCore();

    HelperFunction hf = new HelperFunction();

    Boolean IsPageRefresh = false;

    PhyeGanDate pgd = new PhyeGanDate();

    string imgfolder;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDepartment();
            loadTerminationType();
            txtDate.Text = pgd.GetTodayNepaliDate();
        }
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

    protected void loadTerminationType()
    {
        ETTEnt = new HrEmployeeTerminationType();
        ETTEnt.STATUS = "1";
        ddlType.DataSource = ETTSer.GetAll(ETTEnt);
        ddlType.DataTextField = "TERMINATION_TYPE";
        ddlType.DataValueField = "PK_ID";
        ddlType.DataBind();
        ddlType.Items.Insert(0, "Select");
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
                        HUEnt = new HR_UNIT();
                        HUEnt.PK_ID = EEnt.UNIT;
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
    }
    protected void ImgBttnEdit_Click(object sender, ImageClickEventArgs e)
    {

        GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");

        EmpSearch.Visible = false;
        divEmpDetails.Visible = true;
        loadEmployeeInfo(lblEmpID.Text);
    }

    protected void loadEmployeeInfo(string empId)
    {
        if (empId != "")
        {
            lblEmpID.Text = empId;
            EEnt = new HR_EMPLOYEES();
            EEnt.EMPLOYEEID = empId;
            EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
            if (EEnt != null)
            {
                if (EEnt.DEPARTMENTID != "-1")
                {
                    lblName.Text = EEnt.FULL_NAME;
                    if (EEnt.STATUS == "1")
                    {

                        lblStatus.Text = "Available";
                    }
                    else
                    {
                        lblStatus.Text = "Unavailable";
                    }

                    if (!string.IsNullOrEmpty(lblEmpID.Text))
                    {
                        imgfolder = Server.MapPath(@"~/images/Employee/") + empId + ".jpg";
                        if (File.Exists(imgfolder))
                        {
                            imgEmp.ImageUrl = "~/images/Employee/" + empId + ".jpg";
                            imgLink.HRef = ResolveUrl(imgEmp.ImageUrl);
                        }
                        else
                        {
                            if (EEnt.GENDER.Trim() == "M")
                            {
                                imgEmp.ImageUrl = "~/images/Employee/male.jpg";
                                imgLink.HRef = ResolveUrl(imgEmp.ImageUrl);
                            }
                            if (EEnt.GENDER.Trim() == "F")
                            {
                                imgEmp.ImageUrl = "~/images/Employee/female.jpg";
                                imgLink.HRef = ResolveUrl(imgEmp.ImageUrl);
                            }
                        }
                    }


                    //DEnt = new DIVISION();
                    //DEnt. = EEnt.DIVISION;
                    //DEnt = (Division)DSEr.GetSingle(DEnt);
                    //lblDivision.Text = DEnt.DIVNAME;

                    DPEnt = new HR_DEPARTMENT();
                    DPEnt.PK_ID = EEnt.DEPARTMENTID;
                    DPEnt = (HR_DEPARTMENT)DPSer.GetSingle(DPEnt);
                    lblDepartment.Text = DPEnt.DEP_NAME;

                    if (EEnt.DIVISION != "")
                    {
                        HDIVEnt = new HR_DIVISION();
                        HDIVEnt.PK_ID = EEnt.DIVISION;
                        HDIVEnt = (HR_DIVISION)HDIVSer.GetSingle(HDIVEnt);
                        if (HDIVEnt != null)
                        {
                            trDivision.Visible = true;
                            lblDivision.Text = HDIVEnt.DIVISION_NAME;
                        }
                    }
                    else
                    {

                        trDivision.Visible = false;
                    }

                    HUEnt = new HR_UNIT();
                    HUEnt.PK_ID = EEnt.UNIT;
                    HUEnt = (HR_UNIT)HUSer.GetSingle(HUEnt);
                    if (HUEnt != null)
                    {
                        trUnit.Visible = true;
                        lblUnit.Text = HUEnt.UNIT_NAME;
                    }
                    else
                    {

                        trUnit.Visible = false;
                    }

                    if (EEnt.DESIGNATIONID != "")
                    {
                        DTEnt = new HR_DESIGNATION();
                        DTEnt.PK_ID = EEnt.DESIGNATIONID;
                        DTEnt = (HR_DESIGNATION)DTSer.GetSingle(DTEnt);
                        if (DTEnt != null)
                        {
                            lblDesignation.Text = DTEnt.DESIGNATION_NAME;
                        }
                    }
                    else
                    {
                        lblDesignation.Text = "";
                    }


                    ETEnt = new HrEmployeeTermination();
                    ETEnt.EMPLOYEE_ID = lblEmpID.Text;
                    ETEnt = (HrEmployeeTermination)ETSer.GetSingle(ETEnt);
                    if (ETEnt != null)
                    {
                        lblPKID.Text = ETEnt.PK_ID;
                        ddlType.SelectedValue = ETEnt.EMP_TERMINATION_ID;
                        txtDate.Text = ETEnt.TERMINATION_DATE;
                        txtDescription.Text = ETEnt.DESCRIPTION;
                    }
                    else
                    {
                        ddlType.SelectedIndex = 0;
                        txtDate.Text = pgd.GetTodayNepaliDate();
                        txtDescription.Text = "";
                    }
                    divDetail.Visible = true;
                }
                else
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Employee Does Not Exist");
                    ClearField();
                    divDetail.Visible = false;
                }
            }

            else
            {
                HelperFunction.MsgBox(this, this.GetType(), "Employee Does Not Exist");
                ClearField();
                divDetail.Visible = false;
            }


        }
        else
        {
            ClearField();
        }

    }

    protected void ClearField()
    {
        lblEmpID.Text = "";
        ddlType.SelectedIndex = 0;
        txtDate.Text = pgd.GetTodayNepaliDate();
        txtDescription.Text = "";
        lblDesignation.Text = "";
        lblDepartment.Text = "";
        lblName.Text = "";
        lblStatus.Text = "";
        lblPKID.Text = "";
        divDetail.Visible = false;
        imgEmp.ImageUrl = "";
        txtDescription.Text = "";
    }

    protected void saveTermination()
    {

        string filename = "";
        if (!IsPageRefresh)
        {
            ETEnt = new HrEmployeeTermination();
            ETEnt.EMPLOYEE_ID = lblEmpID.Text;
            ETEnt.EMP_TERMINATION_ID = ddlType.SelectedValue;
            ETEnt.DESCRIPTION = txtDescription.Text;

            //for  saving image in folder
            try
            {
                filename = lblEmpID.Text + fileUpload1.FileName.Substring(fileUpload1.FileName.IndexOf('.'));
                if (fileUpload1.HasFile)
                {
                    // fileUpload1.SaveAs(Server.MapPath("~/images/EmployeeResignation/" + filename));
                    // EEnt.IMAGE = ddlItemCategory.SelectedValue + "_" + txtItemCode.Text;
                    string path = "~/images/Attachment_Staff/EmployeeResignation/";
                    string physicalPath = Server.MapPath(path);

                    // Ensure the directory exists
                    if (!Directory.Exists(physicalPath))
                    {
                        Directory.CreateDirectory(physicalPath);
                    }

                    // Save the file
                    fileUpload1.SaveAs(Path.Combine(physicalPath, lblEmpID.Text + filename));
                }
            }
            catch
            {
            }
            if (txtDate.Text != "")
            {
                try
                {
                    string[] NepDate = txtDate.Text.Split('/');

                    ETEnt.T_DAY = NepDate[0];
                    ETEnt.T_MONTH = NepDate[1];
                    ETEnt.T_YEAR = NepDate[2];
                    ETEnt.TERMINATION_DATE = pgd.ConvertNepaliTOEnglish(NepDate[0], NepDate[1], NepDate[2]);
                }
                catch
                { }
            }
            ETSer.Insert(ETEnt);


            EEnt = new HR_EMPLOYEES();
            EEnt.EMPLOYEEID = lblEmpID.Text;
            EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
            if (EEnt != null)
            {
                EEnt.STATUS = "0";
                ESer.Update(EEnt);
            }

            theLEntity = new Entity.Components.Login();
            theLEntity.EMPLOYEEID = lblEmpID.Text;
            theLEntity = (Entity.Components.Login)theLService.GetSingle(theLEntity);
            if(theLEntity != null)
            {
                theLService.Delete(theLEntity);
            }


            HelperFunction.MsgBox(this, this.GetType(), "Termination Record Saved successfully");
            EmpSearch.Visible = true;
            divEmpDetails.Visible = false;
        }
        ClearField();
    }

    protected void updateTermination()
    {
        string filename = "";
        if (!IsPageRefresh)
        {
            ETEnt = new HrEmployeeTermination();
            ETEnt.PK_ID = lblPKID.Text;
            ETEnt.EMPLOYEE_ID = lblEmpID.Text;
            ETEnt.EMP_TERMINATION_ID = ddlType.SelectedValue;



            string[] date = txtDate.Text.Split('/');
            ETEnt.T_DAY = date[0];
            ETEnt.T_MONTH = date[1];
            ETEnt.T_YEAR = date[2];
            ETEnt.TERMINATION_DATE = pgd.ConvertNepaliTOEnglish(date[0], date[1], date[2]);
            ETEnt.DESCRIPTION = txtDescription.Text;

            //for  saving image in folder
            try
            {
                filename = lblEmpID.Text + fileUpload1.FileName.Substring(fileUpload1.FileName.IndexOf('.'));
                if (fileUpload1.HasFile)
                {
                    // fileUpload1.SaveAs(Server.MapPath("~/images/EmployeeResignation/" + filename));
                    // EEnt.IMAGE = ddlItemCategory.SelectedValue + "_" + txtItemCode.Text;
                    string path = "~/images/Attachment_Staff/EmployeeResignation/";
                    string physicalPath = Server.MapPath(path);

                    // Ensure the directory exists
                    if (!Directory.Exists(physicalPath))
                    {
                        Directory.CreateDirectory(physicalPath);
                    }

                    // Save the file
                    fileUpload1.SaveAs(Path.Combine(physicalPath, lblEmpID.Text + filename));
                }
            }
            catch
            {
            }

            ETSer.Update(ETEnt);


            EEnt = new HR_EMPLOYEES();
            EEnt.EMPLOYEEID = lblEmpID.Text;
            EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
            if (EEnt != null)
            {
                EEnt.STATUS = "0";
                ESer.Update(EEnt);
            }
            HelperFunction.MsgBox(this, this.GetType(), "Termination Record Updated successfully");
            EmpSearch.Visible = true;
            divEmpDetails.Visible = false;
        }
        ClearField();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtDate.Text == "" || txtDescription.Text == "" || ddlType.SelectedIndex == 0)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please fill all the information.");
        }
        else
        {
            ETEnt = new HrEmployeeTermination();
            ETEnt.EMPLOYEE_ID = lblEmpID.Text;
            ETEnt = (HrEmployeeTermination)ETSer.GetSingle(ETEnt);
            if (ETEnt != null)
            {
                updateTermination();
            }
            else
            {
                saveTermination();
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        ClearField();
        EmpSearch.Visible = true;
        divEmpDetails.Visible = false;
    }
}