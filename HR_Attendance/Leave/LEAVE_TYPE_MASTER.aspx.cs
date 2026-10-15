using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;

public partial class HR_Attendance_Leave_LEAVE_TYPE_MASTER : System.Web.UI.Page
{
    HR_LEAVE_TYPE HLTEnt = new HR_LEAVE_TYPE();
    HR_LEAVE_TYPEService HLTSer = new HR_LEAVE_TYPEService();

    Boolean IsPageRefresh = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["postids"] = System.Guid.NewGuid().ToString();
            Session["postid"] = ViewState["postids"].ToString();
            try
            {

                loadGrid();
            }
            catch(Exception ee)
            { }
        }
        else
        {
            if (ViewState["postids"].ToString() != Session["postid"].ToString())
            {
                IsPageRefresh = true;
            }
            Session["postid"] = System.Guid.NewGuid().ToString();
            ViewState["postids"] = Session["postid"].ToString();

        }
    }

    protected void loadGrid()
    {
        HLTEnt = new HR_LEAVE_TYPE();
        gridLeaveType.DataSource = HLTSer.GetAll(HLTEnt);
        gridLeaveType.DataBind();
        if (gridLeaveType.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            a1.Add(HLTEnt);
            gridLeaveType.DataSource = a1;
            gridLeaveType.DataBind();
        }

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!IsPageRefresh)
        {

            GridViewRow row = gridLeaveType.HeaderRow;
            DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");
            DropDownList ddlCommonH = (DropDownList)row.FindControl("ddlCommonH");
            TextBox txtLeaveNameH = (TextBox)row.FindControl("txtLeaveNameH");
            TextBox txtDaysH = (TextBox)row.FindControl("txtDaysH");
            if (txtLeaveNameH.Text != "")
            {
                HLTEnt = new HR_LEAVE_TYPE();
                HLTEnt.LEAVE_TYPE = txtLeaveNameH.Text;
                HLTEnt.DAYS = txtDaysH.Text;
                HLTEnt.STATUS = ddlStatusH.SelectedValue;
                HLTEnt.COMMON = ddlCommonH.SelectedValue;
                HLTSer.Insert(HLTEnt);
            }
            else
                HelperFunction.MsgBox(this, this.GetType(), "Please enter Holiday Name");
            
        }
        loadGrid();
    }

    protected void gridLeaveType_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridLeaveType.EditIndex = e.NewEditIndex;
        loadGrid();
    }

    protected void gridLeaveType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridLeaveType.EditIndex = -1;
        loadGrid();
    }

    protected void gridLeaveType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lblCommon = e.Row.FindControl("lblCommon") as Label;
            if (lblStatus.Text == "1")
                lblStatus.Text = "Available";
            else
                lblStatus.Text = "Unavailable";

            if (lblCommon.Text == "1")
                lblCommon.Text = "Common";
            else
                lblCommon.Text = "Special";
        }
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            Label lblStatusE = e.Row.FindControl("lblStatusE") as Label;
            Label lblLeaveNameE = e.Row.FindControl("lblLeaveNameE") as Label;
            Label lblDaysE = e.Row.FindControl("lblDaysE") as Label;

            TextBox txtLeaveNameE = e.Row.FindControl("txtLeaveNameE") as TextBox;
            TextBox txtDaysE = e.Row.FindControl("txtDaysE") as TextBox;
            DropDownList ddlStatusE = e.Row.FindControl("ddlStatusE") as DropDownList;

            txtLeaveNameE.Text = lblLeaveNameE.Text;
            ddlStatusE.SelectedValue = lblStatusE.Text;
            txtDaysE.Text = lblDaysE.Text;

        }
    }

    protected void gridLeaveType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridLeaveType.Rows[e.RowIndex];
        DropDownList ddlStatusE = row.FindControl("ddlStatusE") as DropDownList;
        TextBox txtLeaveNameE = row.FindControl("txtLeaveNameE") as TextBox;
        TextBox txtDaysE = row.FindControl("txtDaysE") as TextBox;
        Label lblPK = row.FindControl("lblPK") as Label;

        HLTEnt = new HR_LEAVE_TYPE();
        HLTEnt.PK_ID = lblPK.Text;
        HLTEnt = (HR_LEAVE_TYPE)HLTSer.GetSingle(HLTEnt);
        if (HLTEnt != null)
        {
            HLTEnt.STATUS = ddlStatusE.SelectedValue;
            HLTEnt.LEAVE_TYPE = txtLeaveNameE.Text;
            HLTEnt.DAYS = txtDaysE.Text;
            HLTSer.Update(HLTEnt);
            gridLeaveType.EditIndex = -1;
            loadGrid();
        }
    }
}