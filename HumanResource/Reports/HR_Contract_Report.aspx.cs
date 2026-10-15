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

public partial class HumanResource_Reports_HR_Contract_Report : System.Web.UI.Page
{
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

    HR_UNIT HUEnt = new HR_UNIT();
    HR_UNITService HUSer = new HR_UNITService();

    PhyeGanDate pgd = new PhyeGanDate();

    EntityList theEntList = new EntityList();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserProfileEntity userProfileEnt = new UserProfileEntity();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

            LoadData();
            lblDate.Text = pgd.GetTodayNepaliDate();
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

    }

    protected void ddlDept_SelectedIndexChanged1(object sender, EventArgs e)
    {
        LoadDesignation();
    }
    
    protected void LoadDesignation()
    {
        HDEnt = new HR_DESIGNATION(); ;
        HDEnt.DEPARTMENT_ID = ddlDept.SelectedValue;
        ddlDesignation.DataSource = HDSer.GetAll(HDEnt);
        ddlDesignation.DataTextField = "DESIGNATION_NAME";
        ddlDesignation.DataValueField = "PK_ID";
        ddlDesignation.DataBind();
        ddlDesignation.Items.Insert(0, "Select");       

    }

    protected void bttnSearch_Click(object sender, EventArgs e)
    {
        loadAllContract();
        btnPrintEmp.Visible = true;
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

        grdAllContract.DataSource = hf.getAllContractDetails(pgd.GetTodayDate("dd/mm/yyyy"), depid, desId, ddlStatus.SelectedValue);
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

    protected void btnPrintEmp_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }


}