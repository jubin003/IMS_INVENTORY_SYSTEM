using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;

public partial class HumanResource_MasterData_hr_unit : System.Web.UI.Page
{
    HR_UNIT HDEnt = new HR_UNIT();
    HR_UNITService HDSer = new HR_UNITService();

    HR_DIVISION HEnt = new HR_DIVISION();
    HR_DIVISIONService HSer = new HR_DIVISIONService();

    HR_DEPARTMENT HDPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService HDPSer = new HR_DEPARTMENTService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDepartment();
            
        }
    }
    protected void LoadDepartment()
    {
        HDPEnt = new HR_DEPARTMENT();
        HDPEnt.STATUS = "1";
        HDPEnt.DIVISION_EXIST_STATUS = "1";
        ddlDept.DataSource = HDPSer.GetAll(HDPEnt);
        ddlDept.DataTextField = "DEP_NAME";
        ddlDept.DataValueField = "PK_ID";
        ddlDept.DataBind();
        ddlDept.Items.Insert(0, "Select");

    }
    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDept.SelectedIndex != 0)
        {
            LoadDivision();
        }
    }
    protected void LoadDivision()
    {
        HEnt = new HR_DIVISION();
        HEnt.STATUS = "1";
        HEnt.UNIT_EXIST_STATUS = "1";
        HEnt.DEP_ID = ddlDept.SelectedValue;
        ddlDivision.DataSource = HSer.GetAll(HEnt);
        ddlDivision.DataTextField = "DIVISION_NAME";
        ddlDivision.DataValueField = "PK_ID";
        ddlDivision.DataBind();
        ddlDivision.Items.Insert(0, "Select");

    }

    protected void loadGrid()
    {
        HDEnt = new HR_UNIT();
       HDEnt.DIVISION_ID = ddlDivision.SelectedValue;
        gridUnit.DataSource = HDSer.GetAll(HDEnt);
        gridUnit.DataBind();

        if (gridUnit.Rows.Count == 0)
        {
            HDEnt = new HR_UNIT();
            ArrayList alist = new ArrayList();
            alist.Add(HDEnt);
            gridUnit.DataSource = alist;
            gridUnit.DataBind();
        }
    }

    protected void gridUnit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lblStat = e.Row.FindControl("lblStat") as Label;


            if (lblStatus.Text == "1")
            {
                lblStat.Text = "Available";
            }
            else
            {
                lblStat.Text = "Unavailable";

            }


        }

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            Label lblStatusE = e.Row.FindControl("lblStatusE") as Label;
            DropDownList ddlStatusE = e.Row.FindControl("ddlStatusE") as DropDownList;
            ddlStatusE.SelectedValue = lblStatusE.Text;

        }

    }
    protected void gridUnit_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridUnit.Rows[e.RowIndex];

        Label lblPKIDU = (Label)row.FindControl("lblPKIDU");
        TextBox txtUnitNameE = (TextBox)row.FindControl("txtUnitNameE");
        TextBox txtRemarksE = (TextBox)row.FindControl("txtRemarksE");
        TextBox txtUnitCodeE = (TextBox)row.FindControl("txtUnitCodeE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");


        HDEnt = new HR_UNIT();
        HDEnt.PK_ID = lblPKIDU.Text;
        HDEnt = (HR_UNIT)HDSer.GetSingle(HDEnt);
        if (HDEnt != null)
        {
            HDEnt.DIVISION_ID = ddlDivision.SelectedValue;
            HDEnt.UNIT_CODE = txtUnitCodeE.Text;
            HDEnt.UNIT_NAME = txtUnitNameE.Text;
            HDEnt.STATUS = ddlStatusE.SelectedValue;
            HDEnt.REMARKS = txtRemarksE.Text;
            if (HDSer.Update(HDEnt) >= 1)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Successfully Updated");
                loadGrid();
            }

        }
        gridUnit.EditIndex = -1;
        loadGrid();
    }
    protected void gridUnit_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridUnit.EditIndex = e.NewEditIndex;
        loadGrid();

    }
    protected void gridUnit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridUnit.EditIndex = -1;
        loadGrid();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridUnit.HeaderRow;
        TextBox txtUnitNameH = (TextBox)row.FindControl("txtUnitNameH");
        TextBox txtUnitCodeH = (TextBox)row.FindControl("txtUnitCodeH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");
        TextBox txtRemarksH = (TextBox)row.FindControl("txtRemarksH");

        HDEnt = new HR_UNIT();
        HDEnt.DIVISION_ID = ddlDivision.SelectedValue;
        HDEnt.UNIT_NAME = txtUnitNameH.Text;
        HDEnt.UNIT_CODE = txtUnitCodeH.Text;
        HDEnt.STATUS = ddlStatusH.SelectedValue;
        HDEnt.REMARKS = txtRemarksH.Text;
        if (HDSer.Insert(HDEnt) >= 1)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Successfully Inserted");
            loadGrid();
        }

    }

    protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDivision.SelectedIndex != 0)
        {
            loadGrid();
        }
    }

    protected void txtUnitCodeH_TextChanged(object sender, EventArgs e)
    {
        TextBox txtUnitCodeH = gridUnit.HeaderRow.FindControl("txtUnitCodeH") as TextBox;
        TextBox txtUnitNameH = gridUnit.HeaderRow.FindControl("txtUnitNameH") as TextBox;
        Button btnAdd = gridUnit.HeaderRow.FindControl("btnAdd") as Button;

        HDEnt = new HR_UNIT();
        HDEnt.UNIT_CODE = txtUnitCodeH.Text;
        HDEnt = (HR_UNIT)HDSer.GetSingle(HDEnt);
        if (HDEnt != null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Code already exits. Please add another code !!");
            txtUnitCodeH.Text = "";
            btnAdd.Enabled = false;
        }
        else
        {
            btnAdd.Enabled = true;
            txtUnitNameH.Focus();
        }
    }

    protected void txtUnitCodeE_TextChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridUnit.Rows)
        {
            if (gr.RowIndex == gridUnit.EditIndex) // Only the row in edit mode
            {
                TextBox txtUnitCodeE = gr.FindControl("txtUnitCodeE") as TextBox;
                TextBox txtUnitNameE = gr.FindControl("txtUnitNameE") as TextBox;
                ImageButton imgUpdate = gr.FindControl("imgUpdate") as ImageButton;

                HDEnt = new HR_UNIT();
                HDEnt.UNIT_CODE = txtUnitCodeE.Text;
                HDEnt = (HR_UNIT)HDSer.GetSingle(HDEnt);
                if (HDEnt != null)
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Code already exits. Please add another code !!");
                    imgUpdate.Enabled = false;
                }
                else
                {
                    imgUpdate.Enabled = true;
                    txtUnitNameE.Focus();
                }
            }
        }
    }
}