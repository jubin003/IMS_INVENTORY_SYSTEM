using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Entity.Framework;
using Service.Components;
using DataHelper.Framework;
using System.Collections;



public partial class HR_Attendance_HOLIDAY_HOLIDAY : System.Web.UI.Page
{
    HR_DIVISION DEnt = new HR_DIVISION();
    HR_DIVISIONService DSer = new HR_DIVISIONService();

    Months MEnt = new Months();
    MonthsService MSer = new MonthsService();

    HR_HOLIDAY_MASTER HMEnt = new HR_HOLIDAY_MASTER();
    HR_HOLIDAY_MASTERService HMSer = new HR_HOLIDAY_MASTERService();

    HR_HOLIDAY HEnt = new HR_HOLIDAY();
    HR_holidayService HSer = new HR_holidayService();

    HR_HOLIDAY_PROVIDED_TO HPTEnt = new HR_HOLIDAY_PROVIDED_TO();
    HR_HOLIDAY_PROVIDED_TOService HPTSer = new HR_HOLIDAY_PROVIDED_TOService();

    HelperFunction hf = new HelperFunction();
    PhyeGanDate PGD = new PhyeGanDate();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            loadMonth();
            loadHolidayName();
            LoadYear();
            ddlYear.SelectedValue = PGD.NepaliYear();
        }
    }

    protected void loadMonth()
    {
        //MEnt = new Months();
        //ddlMonth.DataSource = MSer.GetAll(MEnt);
        //ddlMonth.DataTextField = "MONTHNAME";
        //ddlMonth.DataValueField = "MONTHID";
        //ddlMonth.DataBind();
        //ddlMonth.Items.Insert(0, "-Month-");
    }
    protected void LoadYear()
    {
        ddlYear.DataSource = GetListOfYears();
        ddlYear.DataBind();
    }       

    protected void btnView_Click(object sender, EventArgs e)
    {
        loadGrid();
        tblAdd.Visible = false;
        tblGrid.Visible = true;
    }
    protected void loadGrid()
    {
        HEnt = new HR_HOLIDAY();
        HEnt.STATUS = "1";
        HEnt.NEPYEAR = ddlYear.SelectedValue;
        gridHoliday.DataSource = HSer.GetAll(HEnt);
        gridHoliday.DataBind();
    }

    protected void gridHoliday_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblHolidayPK = e.Row.FindControl("lblHolidayPK") as Label;
            Label lblApplicableFor = e.Row.FindControl("lblApplicableFor") as Label;
            Label lblGender = e.Row.FindControl("lblGender") as Label;
            Label lblNepDate = e.Row.FindControl("lblNepDate") as Label;
            Label lblHolidayMasterID = e.Row.FindControl("lblHolidayMasterID") as Label;
            Label lblHolidayName = e.Row.FindControl("lblHolidayName") as Label;
            ImageButton ImgBtnEdit = e.Row.FindControl("ImgBtnEdit") as ImageButton;

            #region to display holiday name
            HMEnt = new HR_HOLIDAY_MASTER();
            HMEnt.PK_ID = lblHolidayMasterID.Text;
            HMEnt = (HR_HOLIDAY_MASTER)HMSer.GetSingle(HMEnt);
            if (HMEnt != null)
            {
                lblHolidayName.Text = HMEnt.HOLIDAY_NAME;
            }
            #endregion

            HEnt = new HR_HOLIDAY();
            HEnt.PK_ID = lblHolidayPK.Text;
            HEnt = (HR_HOLIDAY)HSer.GetSingle(HEnt);
            if(HEnt != null)
            {
                lblGender.Text = HEnt.GENDER;
                lblApplicableFor.Text = HEnt.APPLICABLE_FOR ;
                lblNepDate.Text = HEnt.NEPDAY + "/" + HEnt.NEPMONTH + "/" + HEnt.NEPYEAR;
            }
        }

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        tblAdd.Visible = true;
        ddlApplicableFor.SelectedIndex = 0;
        ddlApplicableFor.Enabled = true;
        if (ddlApplicableFor.SelectedValue == "All")
        {
            loadDivision();
            chkDivision.Enabled = false;
            foreach (ListItem item in chkDivision.Items)
                item.Selected = true;
        }
        else
            chkDivision.Enabled = true;

        chkDivision.Visible = true;

        lblAddHeading.Visible = true;
        lblHolidayInfoHeading.Visible = false;
        lblEditHeading.Visible = false;
        tblGrid.Visible = false;
        ddlHolidayName.Enabled = true;
        ddlHolidayName.SelectedIndex = 0;

        txtDate.Enabled = true;
        txtDate.Text = "";
        rbtnStatus.Enabled = true;
        rbtnGender.Enabled = true;
        rbtnGender.SelectedValue = "Both";
        btnAddHoliday.Visible = true;
        btnUpdateHoliday.Visible = false;
        txtHolidayName.Visible = true;
    }


    protected void loadHolidayName()
    {
        HMEnt = new HR_HOLIDAY_MASTER();
        HMEnt.STATUS = "1";
        ddlHolidayName.DataSource = HMSer.GetAll(HMEnt);
        ddlHolidayName.DataTextField = "HOLIDAY_NAME";
        ddlHolidayName.DataValueField = "PK_ID";
        ddlHolidayName.DataBind();
        ddlHolidayName.Items.Insert(0, "Select");
    }


    protected void ddlApplicableFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlApplicableFor.SelectedValue == "Selected")
        {
            chkDivision.Enabled = true;
            chkDivision.Visible = true;
            loadDivision();
        }
        else
        {
            chkDivision.Visible = true;
            chkDivision.Enabled = false;
            foreach (ListItem item in chkDivision.Items)
            {
                item.Selected = true;
            }
        }
    }

    protected void loadDivision()
    {
        DEnt = new HR_DIVISION();
        chkDivision.DataSource = DSer.GetAll(DEnt);
        chkDivision.DataTextField = "DIVISION_NAME";
        chkDivision.DataValueField = "PK_ID";
        chkDivision.DataBind();
    }


    protected void btnAddHoliday_Click(object sender, EventArgs e)
    {
        string[] nepDate = txtDate.Text.Split('/');
        EntityList thelist = new EntityList();
        DistributedTransaction DT = new DistributedTransaction();

        HEnt = new HR_HOLIDAY();
        HEnt.HOLIDAY_MASTER_ID = ddlHolidayName.SelectedValue;
        HEnt.NEPYEAR = nepDate[2];
        HEnt.NEPMONTH = nepDate[1];
        HEnt.NEPDAY = nepDate[0];
        HEnt.APPLICABLE_FOR = ddlApplicableFor.SelectedValue;
        HEnt.ENGDATE = PGD.ConvertNepaliTOEnglish(nepDate[0], nepDate[1], nepDate[2]);
        HEnt.STATUS = "1";
        HEnt.REMARKS = txtHolidayName.Text;
        HEnt.GENDER = rbtnGender.SelectedValue;
        string HR_Holiday_ID = HSer.Insert(HEnt, DT).ToString();


        foreach (ListItem item in chkDivision.Items)
        {
            HPTEnt = new HR_HOLIDAY_PROVIDED_TO();
            HPTEnt.HOLIDAY_ID = HR_Holiday_ID;
            HPTEnt.DIVISION_ID = item.Value;
            HPTEnt.GENDER = rbtnGender.SelectedValue;
            HPTEnt.STATUS = "0";
            HPTSer.Insert(HPTEnt, DT);

        }
        foreach (ListItem item in chkDivision.Items)
        {
            HPTEnt = new HR_HOLIDAY_PROVIDED_TO();
            HPTEnt.HOLIDAY_ID = HR_Holiday_ID;
            HPTEnt.DIVISION_ID = item.Value;
            HPTEnt = (HR_HOLIDAY_PROVIDED_TO)HPTSer.GetSingle(HPTEnt, DT);
            if (HPTEnt != null)
            {
                if (item.Selected)
                {
                    HPTEnt.STATUS = "1";
                    HPTSer.Update(HPTEnt, DT);
                }
            }
        }

        if (DT.HAPPY == true)
        {
            DT.Commit();
            loadGrid();
            HelperFunction.MsgBox(this, this.GetType(), "Inserted Successfully");
            txtDate.Enabled = false;
        }
        else
        {
            DT.Abort();
            HelperFunction.MsgBox(this, this.GetType(), "Unable to Insert");
        }
        DT.Dispose();
    }

    protected void btnUpdateHoliday_Click(object sender, EventArgs e)
    {
        DistributedTransaction DT = new DistributedTransaction();

        string[] nepDate = txtDate.Text.Split('/');
        HEnt = new HR_HOLIDAY();
        HEnt.PK_ID = lblHolidayID.Text;
        HEnt = (HR_HOLIDAY)HSer.GetSingle(HEnt);
        if (HEnt != null)
        {
            HEnt.HOLIDAY_MASTER_ID = ddlHolidayName.SelectedValue;
            HEnt.NEPYEAR = ddlYear.SelectedValue;
            HEnt.NEPYEAR = nepDate[2];
            HEnt.NEPMONTH = nepDate[1];
            HEnt.NEPDAY = nepDate[0];
            HEnt.APPLICABLE_FOR = ddlApplicableFor.SelectedValue;
            HEnt.ENGDATE = PGD.ConvertNepaliTOEnglish(nepDate[0], nepDate[1], nepDate[2]);
            HEnt.REMARKS = txtHolidayName.Text;
            HEnt.STATUS = "1";
            HEnt.GENDER = rbtnGender.SelectedValue;
            HSer.Update(HEnt, DT).ToString();
        }

        foreach (ListItem item in chkDivision.Items)
        {
            HPTEnt = new HR_HOLIDAY_PROVIDED_TO();
            HPTEnt.HOLIDAY_ID = lblHolidayID.Text;
            HPTEnt.DIVISION_ID = item.Value;

            HPTEnt = (HR_HOLIDAY_PROVIDED_TO)HPTSer.GetSingle(HPTEnt, DT);
            if (HPTEnt != null)
            {
                HPTEnt.GENDER = rbtnGender.SelectedValue;
                if (item.Selected)
                {
                    HPTEnt.STATUS = "1";
                    HPTSer.Update(HPTEnt, DT);
                }
                else
                {
                    HPTEnt.STATUS = "0";
                    HPTSer.Update(HPTEnt, DT);
                }
            }
        }

        if (DT.HAPPY == true)
        {
            DT.Commit();
            HelperFunction.MsgBox(this, this.GetType(), "Updated Successfully");
            txtDate.Enabled = false;
            loadGrid();
        }
        else
        {
            DT.Abort();
            HelperFunction.MsgBox(this, this.GetType(), "Unable to Update");
        }
        DT.Dispose();
    }

    protected void gridHoliday_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        UpdatePanel.Visible = true;
        tblAdd.Visible = true;
        GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
        Label lblHolidayPK = gr.FindControl("lblHolidayPK") as Label;
        Label lblHolidayMasterID = gr.FindControl("lblHolidayMasterID") as Label;
        

        ddlHolidayName.SelectedValue = lblHolidayMasterID.Text;

        HEnt = new HR_HOLIDAY();
        HEnt.PK_ID = lblHolidayPK.Text;
        HEnt = (HR_HOLIDAY)HSer.GetSingle(HEnt);
        if (HEnt != null)
        {
            ddlApplicableFor.SelectedValue = HEnt.APPLICABLE_FOR;
            loadDivision();
            chkDivision.Visible = true;
            txtDate.Text = HEnt.NEPDAY + "/" + HEnt.NEPMONTH + "/" + HEnt.NEPYEAR;
            rbtnStatus.SelectedValue = HEnt.STATUS;
            rbtnGender.SelectedValue = HEnt.GENDER;
        }


        foreach (ListItem item in chkDivision.Items)
        {

            HPTEnt = new HR_HOLIDAY_PROVIDED_TO();
            HPTEnt.HOLIDAY_ID = lblHolidayPK.Text;
            HPTEnt.STATUS = "1";
            HPTEnt.DIVISION_ID = item.Value;
            HPTEnt = (HR_HOLIDAY_PROVIDED_TO)HPTSer.GetSingle(HPTEnt);
            if (HPTEnt != null)
                item.Selected = true;
            else
                item.Selected = false;
        }

        if (e.CommandName.Equals("View"))
        {

            lblHolidayInfoHeading.Visible = true;
            lblAddHeading.Visible = false;
            lblEditHeading.Visible = false;

            ddlHolidayName.Enabled = false;
            ddlApplicableFor.Enabled = false;
            chkDivision.Enabled = false;
            chkDivision.Enabled = false;
            txtDate.Enabled = false;
            rbtnStatus.Enabled = false;
            rbtnGender.Enabled = false;
            tblGrid.Visible = false;
            btnAddHoliday.Visible = false;
            btnUpdateHoliday.Visible = false;
            txtHolidayName.Enabled = false;
        }

        if (e.CommandName.Equals("Change"))
        {
            lblHolidayID.Text = lblHolidayPK.Text;


            lblHolidayInfoHeading.Visible = false;
            lblAddHeading.Visible = false;
            lblEditHeading.Visible = true;

            ddlHolidayName.Enabled = true;
            ddlApplicableFor.Enabled = true;
            chkDivision.Enabled = true;
            chkDivision.Enabled = true;
            tblGrid.Visible = false;
            txtDate.Visible = true;           
            txtDate.Enabled = true;
           

            rbtnStatus.Enabled = true;
            rbtnGender.Enabled = true;

            if (ddlApplicableFor.SelectedValue == "Selected")
            {
                chkDivision.Enabled = true;
                chkDivision.Visible = true;
            }
            else
            {
                chkDivision.Visible = true;
                chkDivision.Enabled = false;
            }

            btnAddHoliday.Visible = false;
            btnUpdateHoliday.Visible = true;
            txtHolidayName.Visible = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        UpdatePanel.Visible = false;
    }
    public ArrayList GetListOfYears()
    {
        ArrayList AL = new ArrayList();
        AL.Add((Convert.ToInt32(PGD.NepaliYear()) - 1).ToString());
        AL.Add((Convert.ToInt32(PGD.NepaliYear())).ToString());
        AL.Add((Convert.ToInt32(PGD.NepaliYear()) + 1).ToString());
        return AL;
    }

}