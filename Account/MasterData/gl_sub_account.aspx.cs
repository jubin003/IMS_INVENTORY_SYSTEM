using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;

public partial class Account_MasterData_gl_sub_account : System.Web.UI.Page
{
    GL_ACCOUNT GLEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLSer = new GL_ACCOUNTService();

    GL_SUB_ACCOUNT GSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GSASer = new GL_SUB_ACCOUNTService();

    GL_ACCOUNT_MASTER GMEnt = new GL_ACCOUNT_MASTER();
    GL_ACCOUNT_MASTERService GMSer = new GL_ACCOUNT_MASTERService();

    HelperFunction hf = new HelperFunction();
    AccountFunction af = new AccountFunction();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadGLAccountHead();
        }
    }
    protected void LoadGLAccountHead()
    {
        GMEnt = new GL_ACCOUNT_MASTER();

        ddlAccountsHead.DataSource = af.getAccountHead();
        ddlAccountsHead.DataValueField = "ACCOUNTS_HEAD";
        ddlAccountsHead.DataTextField = "ACCOUNTS_HEAD";
        ddlAccountsHead.DataBind();
        ddlAccountsHead.Items.Insert(0, "Select");
    }
    protected void LoadGLAccountMaster()
    {
        GMEnt = new GL_ACCOUNT_MASTER();

        GMEnt.ACCOUNTS_HEAD = ddlAccountsHead.SelectedValue;
        ddlGLAccMaster.DataSource = GMSer.GetAll(GMEnt);
        ddlGLAccMaster.DataTextField = "GL_MASTER_NAME";
        ddlGLAccMaster.DataValueField = "GL_MASTER_CODE";
        ddlGLAccMaster.DataBind();
        ddlGLAccMaster.Items.Insert(0, "Select");
    }
    protected void LoadGl()
    {
        GLEnt = new GL_ACCOUNT();
        GLEnt.GL_MASTER_CODE = ddlGLAccMaster.SelectedValue;
        // GLEnt.EDITABLE = "1";
        ddlGl.DataSource = GLSer.GetAll(GLEnt);
        ddlGl.DataTextField = "GL_NAME";
        ddlGl.DataValueField = "GL_CODE";
        ddlGl.DataBind();
        ddlGl.Items.Insert(0, "Select");
    }
    protected void ddlAccountsHead_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGLAccountMaster();
    }

    protected void ddlGLAccMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGl();
    }
    protected void LoadGrid()
    {
        GSAEnt = new GL_SUB_ACCOUNT();
        GSAEnt.GL_CODE = ddlGl.SelectedValue;
        gridSubGl.DataSource = GSASer.GetAll(GSAEnt);
        gridSubGl.DataBind();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ddlGl.SelectedValue != "Select" && txtSubGlCode.Text != "")
        {
            if (lblPK_id.Text == "")
            {
                GSAEnt = new GL_SUB_ACCOUNT();
                GSAEnt.GL_CODE = ddlGl.SelectedValue;
                GSAEnt.SUB_GL_CODE = txtSubGlCode.Text;
                GSAEnt.SUB_GL_NAME = txtSubGlName.Text;
                GSAEnt.STATUS = ddlStatus.SelectedValue;
                GSASer.Insert(GSAEnt);
                HelperFunction.MsgBox(this, this.GetType(), "Saved");
            }
            else
            {
                txtSubGlCode.ReadOnly = true;
                GSAEnt = new GL_SUB_ACCOUNT();
                GSAEnt.PK_ID = lblPK_id.Text;
                GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
                if (GSAEnt != null)
                {
                    GSAEnt.GL_CODE = ddlGl.SelectedValue;
                    GSAEnt.SUB_GL_CODE = txtSubGlCode.Text;
                    GSAEnt.SUB_GL_NAME = txtSubGlName.Text;
                    GSAEnt.STATUS = ddlStatus.SelectedValue;
                    GSASer.Update(GSAEnt);
                    HelperFunction.MsgBox(this, this.GetType(), "Updated");
                }
            }
        }
        LoadGrid();
        txtSubGlCode.Text = "";
        txtSubGlName.Text = "";
    }

    protected void txtSubGlCode_TextChanged(object sender, EventArgs e)
    {
        txtSubGlCode.Text = txtSubGlCode.Text.ToUpper();
        GSAEnt = new GL_SUB_ACCOUNT();
        GSAEnt.SUB_GL_CODE = txtSubGlCode.Text;
        GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
        if (GSAEnt != null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Service Code Already exist");
            txtSubGlCode.Text = "";
            txtSubGlCode.Focus();
        }
        else
        {
            txtSubGlName.Focus();
        }
    }

    protected void ddlGl_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid();
    }

    protected void gridSubGl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            Label lblStat = (Label)e.Row.FindControl("lblStat");

            if (lblStatus.Text != "0")
            {
                lblStat.Text = "Available";
            }
            else
            {
                lblStat.Text = " Not Available";
            }
        }
    }

    protected void gridSubGl_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("View"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblpkid = gr.FindControl("lblpkid") as Label;
            GSAEnt = new GL_SUB_ACCOUNT();
            GSAEnt.PK_ID = lblpkid.Text;
            GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
            if (GSAEnt != null)
            {
                lblPK_id.Text = lblpkid.Text;
                ddlGl.SelectedValue = GSAEnt.GL_CODE;
                GLEnt = new GL_ACCOUNT();
                GLEnt.GL_CODE = GSAEnt.GL_CODE;
                GLEnt = (GL_ACCOUNT)GLSer.GetSingle(GLEnt);
                if (GLEnt != null)
                {
                    ddlGLAccMaster.SelectedValue = GLEnt.GL_MASTER_CODE;
                    GMEnt = new GL_ACCOUNT_MASTER();
                    GMEnt.GL_MASTER_CODE = GLEnt.GL_MASTER_CODE;
                    GMEnt = (GL_ACCOUNT_MASTER)GMSer.GetSingle(GMEnt);
                    if (GMEnt != null)
                    {
                        ddlAccountsHead.SelectedValue = GMEnt.ACCOUNTS_HEAD;
                    }
                }
                txtSubGlCode.Text = GSAEnt.SUB_GL_CODE;
                txtSubGlName.Text = GSAEnt.SUB_GL_NAME;
            }
        }
    }
}
