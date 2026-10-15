using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;
public partial class human_resource_Master_data_hr_designation : System.Web.UI.Page
{

    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    HR_DESIGNATION DETEnt = new HR_DESIGNATION();
    HR_DESIGNATIONService DETSer = new HR_DESIGNATIONService();

    UserProfileEntity userProfileEnt = new UserProfileEntity();

   

    HelperFunction hf = new HelperFunction();

    Boolean IsPageRefresh = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDepartment();
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

    protected void loadGrid()
    {

        if (ddlDept.SelectedValue == "Select")
        {
            gridDesignation.DataSource = null;
            gridDesignation.DataBind();
            HelperFunction.MsgBox(this, this.GetType(), "Select Department");
        }
        else
        {
            DETEnt = new HR_DESIGNATION();
            DETEnt.DEPARTMENT_ID = ddlDept.SelectedValue;
            gridDesignation.DataSource = DETSer.GetAll(DETEnt);
            gridDesignation.DataBind();
            #region to add blank data in grid
            if (gridDesignation.Rows.Count == 0)
            {
                ArrayList a1 = new ArrayList();
                a1.Add(DETEnt);
                gridDesignation.DataSource = a1;
                gridDesignation.DataBind();
            }
            #endregion
        }

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!IsPageRefresh)
        {
            GridViewRow row = gridDesignation.HeaderRow;
            TextBox txtDesignationH = (TextBox)row.FindControl("txtDesignationH");
            TextBox txtLevelH = (TextBox)row.FindControl("txtLevelH");

            DETEnt = new HR_DESIGNATION();
            DETEnt.DESIGNATION_NAME = txtDesignationH.Text;
            DETEnt.DEPARTMENT_ID = ddlDept.SelectedValue;
            DETEnt.DESIGNATION_LEVEL = txtLevelH.Text;
            DETSer.Insert(DETEnt);
        }
        loadGrid();
    }
    protected void gridDesignation_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridDesignation.EditIndex = e.NewEditIndex;
        loadGrid();
    }

    protected void gridDesignation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridDesignation.EditIndex = -1;
        loadGrid();
    }

   

    protected void gridDesignation_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridDesignation.Rows[e.RowIndex];
        Label lblPKID = (Label)row.FindControl("lblPKID");
        TextBox txtDesignationE = (TextBox)row.FindControl("txtDesignationE");
        TextBox txtLevelE = (TextBox)row.FindControl("txtLevelE");



        DETEnt = new HR_DESIGNATION();
        DETEnt.PK_ID = lblPKID.Text;
        DETEnt = (HR_DESIGNATION)DETSer.GetSingle(DETEnt);
        if (DETEnt != null)
        {
            DETEnt.DESIGNATION_NAME = txtDesignationE.Text;
            DETEnt.DESIGNATION_LEVEL = txtLevelE.Text;
            DETSer.Update(DETEnt);
            gridDesignation.EditIndex = -1;
        }
        loadGrid();
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        loadGrid();
    }
}