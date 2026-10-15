using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class HR_Attendance_Attendance_AddDeviceID : System.Web.UI.Page
{
    HR_EMPLOYEES HREEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService HRESer = new HR_EMPLOYEESService();

    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    HelperFunction hf = new HelperFunction();
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

    protected void bttnSearch_Click(object sender, EventArgs e)
    {
        HREEnt = new HR_EMPLOYEES();
        HREEnt.STATUS = ddlStatus.SelectedValue;

        if (ddlDept.SelectedValue != "Select")
        {
            HREEnt.DEPARTMENTID = ddlDept.SelectedValue;
        }

        gridMappingID.DataSource = HRESer.GetAll(HREEnt);
        gridMappingID.DataBind();
    }

    protected void gridMappingID_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        Label lblEmployeeID = (Label)e.Row.FindControl("lblEmployeeID");
        Label lblPK_ID = (Label)e.Row.FindControl("lblPK_ID");
        TextBox txtDeviceID = (TextBox)e.Row.FindControl("txtDeviceID");

        HREEnt = new HR_EMPLOYEES();
        HREEnt.PK_ID = lblPK_ID.Text;
        HREEnt = (HR_EMPLOYEES)HRESer.GetSingle(HREEnt);

        if (HREEnt != null && !string.IsNullOrWhiteSpace(HREEnt.DEVICE_ID))
        {
            txtDeviceID.Text = HREEnt.DEVICE_ID.Trim();
        }
        else
        {
            txtDeviceID.Text = "";
        }
    }

    protected void bttnSave_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in gridMappingID.Rows)
            {
                if (row.RowType != DataControlRowType.DataRow) continue;

                Label lblPK_ID = (Label)row.FindControl("lblPK_ID");
                TextBox txtDeviceID = (TextBox)row.FindControl("txtDeviceID");

                HREEnt = new HR_EMPLOYEES();
                HREEnt.PK_ID = lblPK_ID.Text;
                HREEnt = (HR_EMPLOYEES)HRESer.GetSingle(HREEnt);

                if (HREEnt != null)
                {
                    HREEnt.DEVICE_ID = txtDeviceID.Text.Trim();
                    HRESer.Update(HREEnt);
                }
            }

            HelperFunction.MsgBox(this, this.GetType(), "Record saved successfully.");

            bttnSearch_Click(null, null);
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Error while saving.");
        }
    }

    protected void txtDeviceID_TextChanged(object sender, EventArgs e)
    {
        TextBox txtDeviceID = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtDeviceID.NamingContainer;
        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");

        HR_EMPLOYEES HREEnt = new HR_EMPLOYEES();
        HREEnt.DEVICE_ID = txtDeviceID.Text.Trim();

        foreach (HR_EMPLOYEES HREmp in HRESer.GetAll(HREEnt))
        {
            if (HREmp.PK_ID != lblPK_ID.Text && HREmp.DEVICE_ID == txtDeviceID.Text.Trim())
            {
                HelperFunction.MsgBox(this, this.GetType(), "Mapping ID already exists.");
                txtDeviceID.Text = "";
                return;
            }
        }
    }
    

    protected void chckAsEmp_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chckAsEmp = (CheckBox)sender;
        GridViewRow row = (GridViewRow)chckAsEmp.NamingContainer;

        Label lblEmployeeID = (Label)row.FindControl("lblEmployeeID");
        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");
        TextBox txtDeviceID = (TextBox)row.FindControl("txtDeviceID");
        if (chckAsEmp.Checked)
        {
            txtDeviceID.Text = lblEmployeeID.Text;
        }
        else
        {
            HREEnt = new HR_EMPLOYEES();
            HREEnt.PK_ID = lblPK_ID.Text;
            HREEnt = (HR_EMPLOYEES)HRESer.GetSingle(HREEnt);

            if (HREEnt != null)
            {
                txtDeviceID.Text = HREEnt.DEVICE_ID;
            }
            else
            {
                txtDeviceID.Text = "";
            }
        }
    }
}