using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;

public partial class HR_Attendance_HOLIDAY_HOLIDAY_MASTER : System.Web.UI.Page
{
    HR_HOLIDAY_MASTER HMEnt = new HR_HOLIDAY_MASTER();
    HR_HOLIDAY_MASTERService HMSer = new HR_HOLIDAY_MASTERService();

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
            catch { }
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
        HMEnt = new HR_HOLIDAY_MASTER();
        gridHolidayMaster.DataSource = HMSer.GetAll(HMEnt);
        gridHolidayMaster.DataBind();
        if (gridHolidayMaster.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            a1.Add(HMEnt);
            gridHolidayMaster.DataSource = a1;
            gridHolidayMaster.DataBind();
        }

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!IsPageRefresh)
        {

            GridViewRow row = gridHolidayMaster.HeaderRow;
            DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");
            TextBox txtHolidayNameH = (TextBox)row.FindControl("txtHolidayNameH");
            if (txtHolidayNameH.Text != "")
            {
                HMEnt = new HR_HOLIDAY_MASTER();
                HMEnt.HOLIDAY_NAME = txtHolidayNameH.Text;
                HMEnt.STATUS = ddlStatusH.SelectedValue;
                HMSer.Insert(HMEnt);
            }
            else
                HelperFunction.MsgBox(this, this.GetType(), "Please enter Holiday Name");
            
        }
        loadGrid();
    }

    protected void gridHolidayMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridHolidayMaster.EditIndex = e.NewEditIndex;
        loadGrid();
    }



    protected void gridHolidayMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridHolidayMaster.EditIndex = -1;
        loadGrid();
    }

    protected void gridHolidayMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            if (lblStatus.Text == "1")
                lblStatus.Text = "Available";
            else
                lblStatus.Text = "Unavailable";
        }
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            Label lblStatusE = e.Row.FindControl("lblStatusE") as Label;
            Label lblHolidayNameE = e.Row.FindControl("lblHolidayNameE") as Label;
            TextBox txtHolidayNameE = e.Row.FindControl("txtHolidayNameE") as TextBox;
            DropDownList ddlStatusE = e.Row.FindControl("ddlStatusE") as DropDownList;
            txtHolidayNameE.Text = lblHolidayNameE.Text;
            ddlStatusE.SelectedValue = lblStatusE.Text;

        }
    }

    protected void gridHolidayMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridHolidayMaster.Rows[e.RowIndex];
        DropDownList ddlStatusE = row.FindControl("ddlStatusE") as DropDownList;
        TextBox txtHolidayNameE = row.FindControl("txtHolidayNameE") as TextBox;
        Label lblPK = row.FindControl("lblPK") as Label;

        HMEnt = new HR_HOLIDAY_MASTER();
        HMEnt.PK_ID = lblPK.Text;
        HMEnt = (HR_HOLIDAY_MASTER)HMSer.GetSingle(HMEnt);
        if (HMEnt != null)
        {
            HMEnt.STATUS = ddlStatusE.SelectedValue;
            HMEnt.HOLIDAY_NAME = txtHolidayNameE.Text;
            HMSer.Update(HMEnt);
            gridHolidayMaster.EditIndex = -1;
            loadGrid();
        }
    }
}