using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;

public partial class Account_MasterData_Bank_account : System.Web.UI.Page
{
    BANK BEnt = new BANK();
    BANKService BSer = new BANKService();

    BANK_ACCOUNT BAEnt = new BANK_ACCOUNT();
    BANK_ACCOUNTService BASer = new BANK_ACCOUNTService();

    GL_SUB_ACCOUNT GSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GSASer = new GL_SUB_ACCOUNTService();

    GL_ACCOUNT GAEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GASer = new GL_ACCOUNTService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadBank();
        }
    }

    protected void LoadBank()
    {
        BEnt = new BANK();
        BEnt.STATUS = "1";
        ddlBanks.DataSource = BSer.GetAll(BEnt);
        ddlBanks.DataTextField = "BANK_NAME";
        ddlBanks.DataValueField = "PK_ID";
        ddlBanks.DataBind();
        ddlBanks.Items.Insert(0, "Select");


    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        #region Insert
        if (lblPK_id.Text == "")
        {
            if (txtBankCode.Text != "" && ddlAccountType.SelectedValue != "Select" && txtAccNumber.Text != ""&&ddlShowInReceipt.SelectedValue!="Select"&&ddlStatus.SelectedValue!="Select")
            {
                string bankName = "";
                BAEnt = new BANK_ACCOUNT();
                BAEnt.BANK_ID = ddlBanks.SelectedValue;
                BEnt = new BANK();
                BEnt.PK_ID = ddlBanks.SelectedValue;
                BEnt = (BANK)BSer.GetSingle(BEnt);
                if (BEnt != null)
                {
                    BAEnt.BANK_NAME = BEnt.BANK_NAME;
                    bankName = BEnt.BANK_NAME;
                }
                BAEnt.BANK_CODE = txtBankCode.Text;
                BAEnt.BRANCH = txtBranch.Text;
                BAEnt.ACCOUNT_TYPE = ddlAccountType.SelectedValue;
                BAEnt.ACCOUNT_NUMBER = txtAccNumber.Text;
                BAEnt.BANK_CHARGE = "0";
                BAEnt.SHOW_IN_RECEIPT = ddlShowInReceipt.SelectedValue;
                BAEnt.STATUS = ddlStatus.SelectedValue;
                BASer.Insert(BAEnt);
                GSAEnt = new GL_SUB_ACCOUNT();
                GSAEnt.SUB_GL_CODE = txtBankCode.Text;
                GAEnt = new GL_ACCOUNT();
                GAEnt.GL_NAME = "Bank";
                GAEnt = (GL_ACCOUNT)GASer.GetSingle(GAEnt);
                if (GAEnt != null)
                {
                    GSAEnt.GL_CODE = GAEnt.GL_CODE;
                }

                GSAEnt.SUB_GL_NAME = bankName + "-" + txtAccNumber.Text;
                GSAEnt.STATUS = ddlStatus.SelectedValue;
                GSASer.Insert(GSAEnt);
            }
            else
            {
                HelperFunction.MsgBox(this, this.GetType(), "Complete Fields");
            }
        }
        #endregion
        #region Update
        else
        {
            string bankName = "";
            txtBankCode.ReadOnly = true;
            BAEnt = new BANK_ACCOUNT();
            BAEnt.PK_ID = lblPK_id.Text;
            BAEnt = (BANK_ACCOUNT)BASer.GetSingle(BAEnt);
            if (BAEnt != null)
            {
                BAEnt.BANK_ID = ddlBanks.SelectedValue;
                BEnt = new BANK();
                BEnt.PK_ID = ddlBanks.SelectedValue;
                BEnt = (BANK)BSer.GetSingle(BEnt);
                if (BEnt != null)
                {
                    BAEnt.BANK_NAME = BEnt.BANK_NAME;
                    bankName = BEnt.BANK_NAME;
                }
                BAEnt.BANK_CODE = txtBankCode.Text;
                BAEnt.BRANCH = txtBranch.Text;
                BAEnt.ACCOUNT_TYPE = ddlAccountType.SelectedValue;
                BAEnt.ACCOUNT_NUMBER = txtAccNumber.Text;
                BAEnt.BANK_CHARGE = "0";
                BAEnt.SHOW_IN_RECEIPT = ddlShowInReceipt.SelectedValue;
                BAEnt.STATUS = ddlStatus.SelectedValue;
                BASer.Update(BAEnt);
            }

            GSAEnt = new GL_SUB_ACCOUNT();
            GSAEnt.SUB_GL_CODE = txtBankCode.Text;
            GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
            if (GSAEnt != null)
            {
                GSAEnt.SUB_GL_CODE = txtBankCode.Text;
                GAEnt = new GL_ACCOUNT();
                GAEnt.GL_NAME = "Bank";
                GAEnt = (GL_ACCOUNT)GASer.GetSingle(GAEnt);
                if (GAEnt != null)
                {
                    GSAEnt.GL_CODE = GAEnt.GL_CODE;
                }

                GSAEnt.SUB_GL_NAME = bankName + "-" + txtAccNumber.Text;
                GSAEnt.STATUS = ddlStatus.SelectedValue;
                GSASer.Update(GSAEnt);
            }
            else
            {
                GSAEnt = new GL_SUB_ACCOUNT();
                GSAEnt.SUB_GL_CODE = txtBankCode.Text;
                GAEnt = new GL_ACCOUNT();
                GAEnt.GL_NAME = "Bank";
                GAEnt = (GL_ACCOUNT)GASer.GetSingle(GAEnt);
                if (GAEnt != null)
                {
                    GSAEnt.GL_CODE = GAEnt.GL_CODE;
                }

                GSAEnt.SUB_GL_NAME = bankName + "-" + txtAccNumber.Text;
                GSAEnt.STATUS = ddlStatus.SelectedValue;
                GSASer.Insert(GSAEnt);
            }
        }
        #endregion
        LoadGrid();
        CLearField();
        divAddBank.Visible = false;
    }

    protected void LoadGrid()
    {
        BAEnt = new BANK_ACCOUNT();
        BAEnt.STATUS = "1";
        BAEnt.BANK_ID = ddlBanks.SelectedValue;
        gridBankAccount.DataSource = BASer.GetAll(BAEnt);
        gridBankAccount.DataBind();
    }

    protected void gridBankAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            Label lblStat = (Label)e.Row.FindControl("lblStat");
            Label lblShowInReceipt = (Label)e.Row.FindControl("lblShowInReceipt");
            Label lblShowInRec = (Label)e.Row.FindControl("lblShowInRec");

            if (lblStatus.Text != "0")
            {
                lblStat.Text = "Available";
            }
            else
            {
                lblStat.Text = " Not Available";

            }
            if (lblShowInReceipt.Text != "0")
            {
                lblShowInRec.Text = "Yes";
            }
            else
            {
                lblShowInRec.Text = " No";
            }
        }
    }

    protected void gridBankAccount_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("View"))
        {
            divAddBank.Visible = true;
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblpkid = gr.FindControl("lblpkid") as Label;
            BAEnt = new BANK_ACCOUNT();
            BAEnt.PK_ID = lblpkid.Text;
            BAEnt = (BANK_ACCOUNT)BASer.GetSingle(BAEnt);
            if (BAEnt != null)
            {
                lblPK_id.Text = lblpkid.Text;
                txtBankCode.Text = BAEnt.BANK_CODE;
                ddlBanks.SelectedValue = BAEnt.BANK_ID;
                ddlStatus.SelectedValue = BEnt.STATUS;
                txtBranch.Text = BAEnt.BRANCH;
                ddlAccountType.SelectedValue = BAEnt.ACCOUNT_TYPE;
                txtAccNumber.Text = BAEnt.ACCOUNT_NUMBER;
                ddlShowInReceipt.SelectedValue = BAEnt.SHOW_IN_RECEIPT;
                ddlStatus.SelectedValue = BAEnt.STATUS;
            }
        }
    }

    protected void ddlBanks_SelectedIndexChanged(object sender, EventArgs e)
    {
        BAEnt = new BANK_ACCOUNT();
        BAEnt.BANK_ID = ddlBanks.SelectedValue;
        gridBankAccount.DataSource = BASer.GetAll(BAEnt);
        gridBankAccount.DataBind();
    }
    protected void CLearField()
    {
        txtBankCode.Text = "";
        txtBranch.Text = "";
        ddlAccountType.SelectedValue = "";
        txtAccNumber.Text = "";
        lblPK_id.Text = "";
        ddlShowInReceipt.SelectedValue = "Select";
        ddlStatus.SelectedValue = "Select";
        ddlAccountType.SelectedValue = "Select";
    }
    protected void txtBankCode_TextChanged(object sender, EventArgs e)
    {
        txtBankCode.Text = txtBankCode.Text.ToUpper();
        GSAEnt = new GL_SUB_ACCOUNT();
        GSAEnt.SUB_GL_CODE = txtBankCode.Text;
        GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
        if (GSAEnt != null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Service Code Already exist");
            txtBankCode.Text = "";
            txtBankCode.Focus();
        }
        else
        {
            txtBranch.Focus();
        }
    }

    protected void btnAddBank_Click(object sender, EventArgs e)
    {
        divAddBank.Visible = true;
    }
}