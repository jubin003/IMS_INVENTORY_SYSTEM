using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;

public partial class Account_MasterData_gl_account : System.Web.UI.Page
{
    GL_ACCOUNT GLEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLSer = new GL_ACCOUNTService();

    GL_ACCOUNT_MASTER GMEnt = new GL_ACCOUNT_MASTER();
    GL_ACCOUNT_MASTERService GMSer = new GL_ACCOUNT_MASTERService();

    PL_HEADINGS PHEnt = new PL_HEADINGS();
    PL_HEADINGSService PHSer = new PL_HEADINGSService();

    BS_HEADINGS BHEnt = new BS_HEADINGS();
    BS_HEADINGSService BHSer = new BS_HEADINGSService();

    HelperFunction hf = new HelperFunction();
    AccountFunction af = new AccountFunction();

    PhyeGan PG = new PhyeGan();

    Boolean IsPageRefresh = false;

    UserProfileEntity userProfileEnt = new UserProfileEntity();

    static string path = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["postids"] = System.Guid.NewGuid().ToString();
                Session["postid"] = ViewState["postids"].ToString();

                userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");

                if (hf.checkPageAccess(path, userProfileEnt.UserGroupID.ToString()))
                {
                    LoadGLAccountHead();
                    ddlHeadings.Items.Insert(0, "Select");
                }
                else
                {
                    Response.Redirect("~/forbidden.aspx");
                }
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
        catch (System.Threading.ThreadAbortException)
        {
            Response.Redirect("~/forbidden.aspx");
        }
        catch
        {
            Response.Redirect("~/Login.aspx");
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

    protected void loadPLHeading()
    {
        PHEnt = new PL_HEADINGS();
        if (ddlAccountsHead.SelectedItem.Text == "Expenses" || ddlAccountsHead.SelectedItem.Text == "Income")
        {
            PHEnt.PL_MAIN_HEADING = ddlAccountsHead.SelectedItem.Text.ToUpper();
            ddlHeadings.Items.Clear();
            ddlHeadings.DataSource = PHSer.GetAll(PHEnt);
            ddlHeadings.DataTextField = "PL_HEADING";
            ddlHeadings.DataValueField = "PK_ID";
            ddlHeadings.DataBind();
            ddlHeadings.Items.Insert(0, "Select");
        }
        else if (ddlAccountsHead.SelectedItem.Text == "Assets" || ddlAccountsHead.SelectedItem.Text == "Liabilities" || ddlAccountsHead.SelectedItem.Text == "Equity")
        {
            BHEnt.BS_MAIN_HEADING = ddlAccountsHead.SelectedItem.Text;
            ddlHeadings.Items.Clear();
            ddlHeadings.DataSource = BHSer.GetAll(BHEnt);
            ddlHeadings.DataTextField = "BS_HEADING";
            ddlHeadings.DataValueField = "PK_ID";
            ddlHeadings.DataBind();
            ddlHeadings.Items.Insert(0, "Select");
        }

        else
        {
            ddlHeadings.Items.Clear();
            ddlHeadings.Items.Add(new ListItem("Select", "0"));
        }
    }


    protected void ddlAccountsHead_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadPLHeading();
        LoadGLAccountMaster();
    }

    protected void ddlGLAccMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        string code = af.getGLCode(ddlGLAccMaster.SelectedValue);
        txtGlCode.Text = Convert.ToDouble(code).ToString("000000");
        txtGLname.Text = "";
        loadGrid();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string bsHead = "";
        if (ddlHeadings.SelectedValue != "Select")
        {
            bsHead = ddlHeadings.SelectedValue;
        }
        if (lblPK_id.Text == "")
        {
            GLEnt = new GL_ACCOUNT();
            GLEnt.GL_MASTER_CODE = ddlGLAccMaster.SelectedValue;
            GLEnt.GL_NAME = txtGLname.Text;
            GLEnt.GL_TYPE = "G";
            GLEnt.SUB_LEDGER = ddlSubLedger.SelectedValue;
            GLEnt.STATUS = ddlStatus.SelectedValue;
            GLEnt.GL_CODE = txtGlCode.Text;
            GLEnt.BS_HEADING = bsHead;
            GLEnt.EDITABLE = "0";
            GLSer.Insert(GLEnt);
            HelperFunction.MsgBox(this, this.GetType(), "Saved");
        }
        else
        {
            GLEnt = new GL_ACCOUNT();
            GLEnt.PK_ID = lblPK_id.Text;
            GLEnt = (GL_ACCOUNT)GLSer.GetSingle(GLEnt);
            if (GLEnt != null)
            {
                GLEnt.GL_MASTER_CODE = ddlGLAccMaster.SelectedValue;
                GLEnt.GL_NAME = txtGLname.Text;
                GLEnt.GL_TYPE = "G";
                GLEnt.SUB_LEDGER = ddlSubLedger.SelectedValue;
                GLEnt.STATUS = ddlStatus.SelectedValue;
                GLEnt.GL_CODE = txtGlCode.Text;
                GLEnt.BS_HEADING = bsHead;
                GLEnt.EDITABLE = "0";
                GLSer.Update(GLEnt);
                HelperFunction.MsgBox(this, this.GetType(), "Updated");
            }
            lblPK_id.Text = "";

        }
        string code = af.getGLCode(ddlGLAccMaster.SelectedValue);
        txtGlCode.Text = Convert.ToDouble(code).ToString("000000");
        txtGLname.Text = "";
        loadGrid();
    }
    protected void loadGrid()
    {
        string bsHead = "";
        if (ddlHeadings.SelectedValue != "Select")
        {
            bsHead = ddlHeadings.SelectedValue;
        }
        GLEnt = new GL_ACCOUNT();
        GLEnt.GL_MASTER_CODE = ddlGLAccMaster.SelectedValue;
        GLEnt.BS_HEADING = bsHead;
        gridGlAccount.DataSource = GLSer.GetAll(GLEnt);
        gridGlAccount.DataBind();
    }


    protected void gridGlAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblSubLedger = (Label)e.Row.FindControl("lblSubLedger");
            Label lblSub = (Label)e.Row.FindControl("lblSub");
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            Label lblStat = (Label)e.Row.FindControl("lblStat");
            Label lbleditable = (Label)e.Row.FindControl("lbleditable");


            ImageButton imgEdit = (ImageButton)e.Row.FindControl("imgEdit");

            if (lblSubLedger.Text != "0")
            {
                lblSub.Text = "Available";
            }
            else
            {
                lblSub.Text = " Not Available";

            }

            if (lblStatus.Text != "0")
            {
                lblStat.Text = "Available";
            }
            else
            {
                lblStat.Text = " Not Available";

            }
            if (lbleditable.Text != "0")
            {
                imgEdit.Visible = true;
            }
            else
            {
                imgEdit.Visible = false;

            }


        }
    }

    protected void gridGlAccount_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("View"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblpkid = gr.FindControl("lblpkid") as Label;
            GLEnt = new GL_ACCOUNT();
            GLEnt.PK_ID = lblpkid.Text;
            GLEnt = (GL_ACCOUNT)GLSer.GetSingle(GLEnt);
            if (GLEnt != null)
            {
                lblPK_id.Text = lblpkid.Text;
                ddlGLAccMaster.SelectedValue = GLEnt.GL_MASTER_CODE;
                txtGlCode.Text = GLEnt.GL_CODE;
                ddlStatus.SelectedValue = GLEnt.STATUS;
                ddlSubLedger.SelectedValue = GLEnt.SUB_LEDGER;
                GMEnt = new GL_ACCOUNT_MASTER();
                GMEnt.GL_MASTER_CODE = GLEnt.GL_MASTER_CODE;
                GMEnt = (GL_ACCOUNT_MASTER)GMSer.GetSingle(GMEnt);
                if (GMEnt != null)
                {
                    ddlAccountsHead.SelectedValue = GMEnt.ACCOUNTS_HEAD;
                }

            }
        }
    }

    protected void ddlHeadings_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadGrid();
    }
}