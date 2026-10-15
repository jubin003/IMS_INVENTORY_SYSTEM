using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;
using PhyeGanCore;
using DataHelper.Framework;
using System.IO;

public partial class HumanResource_MasterData_hr_attachment_master : System.Web.UI.Page
{
    HR_ATTACHMENT_MASTER ATMEnt = new HR_ATTACHMENT_MASTER();
    HR_ATTACHMENT_MASTERService ATMSer = new HR_ATTACHMENT_MASTERService();

    HelperFunction hf = new HelperFunction();

    PhyeGan PG = new PhyeGan();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
        }
    }

    protected void LoadData()
    {

        ATMEnt = new HR_ATTACHMENT_MASTER();
        grdAttachment.DataSource = ATMSer.GetAll(ATMEnt);
        grdAttachment.DataBind();

        if (grdAttachment.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            ATMEnt = new HR_ATTACHMENT_MASTER();
            a1.Add(ATMEnt);
            grdAttachment.DataSource = a1;
            grdAttachment.DataBind();
        }


    }

    protected void grdAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            DropDownList ddlStatusE = e.Row.FindControl("ddlStatusE") as DropDownList;
            Label lblStatusE = e.Row.FindControl("lblStatusE") as Label;
            DropDownList ddlDocNumE = e.Row.FindControl("ddlDocNumE") as DropDownList;
            Label lblDocNumE = e.Row.FindControl("lblDocNumE") as Label;
            DropDownList ddlIssueDateE = e.Row.FindControl("ddlIssueDateE") as DropDownList;
            Label lblIssueDateE = e.Row.FindControl("lblIssueDateE") as Label;
            DropDownList ddlExpAppliE = e.Row.FindControl("ddlExpAppliE") as DropDownList;
            Label lblExpAppliE = e.Row.FindControl("lblExpAppliE") as Label;
            DropDownList ddlIssueAppliE = e.Row.FindControl("ddlIssueAppliE") as DropDownList;
            Label lblIssueAppliE = e.Row.FindControl("lblIssueAppliE") as Label;


            ddlStatusE.SelectedValue = lblStatusE.Text;
            ddlDocNumE.SelectedValue = lblDocNumE.Text;
            ddlIssueDateE.SelectedValue = lblIssueDateE.Text;
            ddlExpAppliE.SelectedValue = lblExpAppliE.Text;
            ddlIssueAppliE.SelectedValue = lblIssueAppliE.Text;

        }

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lbllblStatusName = e.Row.FindControl("lbllblStatusName") as Label;
            Label lblDocNum = e.Row.FindControl("lblDocNum") as Label;
            Label lblDocNumName = e.Row.FindControl("lblDocNumName") as Label;
            Label lblIssueDate = e.Row.FindControl("lblIssueDate") as Label;
            Label lblIssueDateName = e.Row.FindControl("lblIssueDateName") as Label;
            Label lblExpAppli = e.Row.FindControl("lblExpAppli") as Label;
            Label lblExpAppliName = e.Row.FindControl("lblExpAppliName") as Label;
            Label lblIssueAppli = e.Row.FindControl("lblIssueAppli") as Label;
            Label lblIssueAppliName = e.Row.FindControl("lblIssueAppliName") as Label;

            if (lblStatus.Text == "1")
            {
                lbllblStatusName.Text = "Available";
            }
            else
            {
                lbllblStatusName.Text = "Unavailable";

            }
            if (lblDocNum.Text == "1")
            {
                lblDocNumName.Text = "Yes";
            }
            else
            {
                lblDocNumName.Text = "No";

            }
            if (lblIssueDate.Text == "1")
            {
                lblIssueDateName.Text = "Yes";
            }
            else
            {
                lblIssueDateName.Text = "No";

            }
            if (lblExpAppli.Text == "1")
            {
                lblExpAppliName.Text = "Yes";
            }
            else
            {
                lblExpAppliName.Text = "No";

            }
            if (lblIssueAppli.Text == "1")
            {
                lblIssueAppliName.Text = "Yes";
            }
            else
            {
                lblIssueAppliName.Text = "No";

            }
        }
    }
    protected void grdAttachment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdAttachment.EditIndex = -1;
        LoadData();
    }

    protected void grdAttachment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdAttachment.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void grdAttachment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = grdAttachment.Rows[e.RowIndex];
        Label lblPkid = row.FindControl("lblPkid") as Label;
        DropDownList ddlStatusE = row.FindControl("ddlStatusE") as DropDownList;
        DropDownList ddlDocNumE = row.FindControl("ddlDocNumE") as DropDownList;
        DropDownList ddlIssueDateE = row.FindControl("ddlIssueDateE") as DropDownList;
        DropDownList ddlExpAppliE = row.FindControl("ddlExpAppliE") as DropDownList;
        DropDownList ddlIssueAppliE = row.FindControl("ddlIssueAppliE") as DropDownList;
        TextBox txtAttDocE = row.FindControl("txtAttDocE") as TextBox;
        TextBox txtOrder = row.FindControl("txtOrder") as TextBox;
        Label lblAttDocE = row.FindControl("lblAttDocE") as Label;

        string foldername = lblAttDocE.Text.Replace(" ", "_");
        string Doc_Name = txtAttDocE.Text.Replace(" ", "_");
        ATMEnt = new HR_ATTACHMENT_MASTER();
        ATMEnt.PK_ID = lblPkid.Text;
        ATMEnt = (HR_ATTACHMENT_MASTER)ATMSer.GetSingle(ATMEnt);
        if (ATMEnt != null)
        {
            ATMEnt.ATTACHMENT_DOC = txtAttDocE.Text;
            ATMEnt.ORDER_BY = txtOrder.Text;
            ATMEnt.STATUS = ddlStatusE.SelectedValue;
            ATMEnt.DOC_NUM_APPLICABLE = ddlDocNumE.SelectedValue;
            ATMEnt.ISSUE_DATE_APPLICABLE = ddlIssueDateE.SelectedValue;
            ATMEnt.EXP_DATE_APPLICABLE = ddlExpAppliE.SelectedValue;
            ATMEnt.ISSUE_BY_APPLICABLE = ddlIssueAppliE.SelectedValue;

            ATMSer.Update(ATMEnt);
        }
        try
        {
            string path = "~/images/Attachment_Staff/Emp_" + foldername + "/";
            string physicalPath = Server.MapPath(path);

            // Ensure the directory exists
            if (!Directory.Exists(physicalPath))
            {
                Directory.CreateDirectory(physicalPath);
            }
            else
            {
                if (Doc_Name != foldername)
                {
                    Directory.Delete(physicalPath);
                    string newPath = "~/images/Attachment_Staff/Emp_" + Doc_Name + "/";
                    string NewphysicalPath = Server.MapPath(newPath);

                    // Ensure the directory exists
                    if (!Directory.Exists(NewphysicalPath))
                    {
                        Directory.CreateDirectory(NewphysicalPath);
                    }
                }

            }
            // Save the file



        }
        catch
        {
            // Optionally log the error
        }


        grdAttachment.EditIndex = -1;
        LoadData();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

        GridViewRow row = grdAttachment.HeaderRow;
        DropDownList ddlStatusH = row.FindControl("ddlStatusH") as DropDownList;
        DropDownList ddlDocNumH = row.FindControl("ddlDocNumH") as DropDownList;
        DropDownList ddlIssueDateH = row.FindControl("ddlIssueDateH") as DropDownList;
        DropDownList ddlExpAppliH = row.FindControl("ddlExpAppliH") as DropDownList;
        DropDownList ddlIssueAppliH = row.FindControl("ddlIssueAppliH") as DropDownList;
        TextBox txtAttDocH = row.FindControl("txtAttDocH") as TextBox;
        TextBox txtOrder = row.FindControl("txtOrder") as TextBox;

        ATMEnt = new HR_ATTACHMENT_MASTER();

        ATMEnt.ATTACHMENT_DOC = txtAttDocH.Text;
        ATMEnt.ORDER_BY = txtOrder.Text;

        ATMEnt.STATUS = ddlStatusH.SelectedValue;
        ATMEnt.DOC_NUM_APPLICABLE = ddlDocNumH.SelectedValue;
        ATMEnt.ISSUE_DATE_APPLICABLE = ddlIssueDateH.SelectedValue;
        ATMEnt.EXP_DATE_APPLICABLE = ddlExpAppliH.SelectedValue;
        ATMEnt.ISSUE_BY_APPLICABLE = ddlIssueAppliH.SelectedValue;


        ATMSer.Insert(ATMEnt);

        string foldername = "";
        foldername = txtAttDocH.Text.Replace(" ", "_");
        try
        {
            string path = "~/images/Attachment_Staff/Emp_" + foldername + "/";
            string physicalPath = Server.MapPath(path);

            // Ensure the directory exists
            if (!Directory.Exists(physicalPath))
            {
                Directory.CreateDirectory(physicalPath);
            }

            // Save the file



        }
        catch
        {
            // Optionally log the error
        }



        LoadData();
    }


}