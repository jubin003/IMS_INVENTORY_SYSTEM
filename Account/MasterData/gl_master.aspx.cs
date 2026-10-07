using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;

public partial class Account_MasterData_gl_master : System.Web.UI.Page
{
    GL_ACCOUNT_MASTER GMEnt = new GL_ACCOUNT_MASTER();
    GL_ACCOUNT_MASTERService GMSer = new GL_ACCOUNT_MASTERService();

    HelperFunction hf = new HelperFunction();
    AccountFunction af = new AccountFunction();

    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();

    BS_HEADINGS BHEnt = new BS_HEADINGS();
    BS_HEADINGSService BHSer = new BS_HEADINGSService();

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
                    ddlHeading.Items.Insert(0, "Select");
                    LoadGLAccountHead();
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
   
    protected void ddlAccountsHead_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAccountsHead.SelectedValue != "Select")
        {
            string code = af.getGLMasterCode(ddlAccountsHead.SelectedValue);
            txtCode.Text = Convert.ToDouble(code).ToString("0000");
            txtCode.ReadOnly = true;
            loadHeading();
            LoadGrid();
        }
    }

    protected void LoadGLAccountHead()
    {
        GMEnt = new GL_ACCOUNT_MASTER();
        ddlAccountsHead.DataSource = af.getAccountHead();
        ddlAccountsHead.DataValueField = "ACC_HEAD_CODE";
        ddlAccountsHead.DataTextField = "ACCOUNTS_HEAD";
        ddlAccountsHead.DataBind();
        ddlAccountsHead.Items.Insert(0, "Select");
    }

    protected void loadHeading()
    {
        BHEnt = new BS_HEADINGS();
        if (ddlAccountsHead.SelectedItem.Text == "Assets" || ddlAccountsHead.SelectedItem.Text == "Liabilities" || ddlAccountsHead.SelectedItem.Text == "Equity")
        {
            BHEnt.BS_MAIN_HEADING = ddlAccountsHead.SelectedItem.Text;
            ddlHeading.Items.Clear();
            ddlHeading.DataSource = BHSer.GetAll(BHEnt);
            ddlHeading.DataTextField = "BS_HEADING";
            ddlHeading.DataValueField = "PK_ID";
            ddlHeading.DataBind();
            ddlHeading.Items.Insert(0, "Select");
            tdBSHeading.Visible = true;
        }
        else
        {
            ddlHeading.Items.Clear();
            ddlHeading.Items.Add(new ListItem("Select", "0"));
            tdBSHeading.Visible = false;
        }     
        
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string bsHead = "";
        if (ddlHeading.SelectedValue != "Select")
        {
            bsHead = ddlHeading.SelectedValue;
        }
        if (lblPK_id.Text =="")
        {
            GMEnt = new GL_ACCOUNT_MASTER();
            GMEnt.ACC_HEAD_CODE = ddlAccountsHead.SelectedValue;
            GMEnt.ACCOUNTS_HEAD = ddlAccountsHead.SelectedItem.ToString();
            GMEnt.GL_MASTER_CODE = txtCode.Text;
            GMEnt.FINANCIAL_STATEMENT = ddlFinancialStatement.SelectedValue;
            GMEnt.GL_MASTER_NAME = txtGlName.Text;
            GMEnt.ORDER_BY = txtOrderBy.Text;
            GMEnt.BS_HEADING = bsHead;
            GMSer.Insert(GMEnt);
            HelperFunction.MsgBox(this, this.GetType(), "Saved");
        }
        else
        {
            GMEnt = new GL_ACCOUNT_MASTER();
            GMEnt.PK_ID = lblPK_id.Text;
            GMEnt = (GL_ACCOUNT_MASTER)GMSer.GetSingle(GMEnt);
            if (GMEnt != null)
            {
                GMEnt.ACC_HEAD_CODE = ddlAccountsHead.SelectedValue;
                GMEnt.ACCOUNTS_HEAD = ddlAccountsHead.SelectedItem.ToString();
                GMEnt.GL_MASTER_CODE = txtCode.Text;
                GMEnt.FINANCIAL_STATEMENT = ddlFinancialStatement.SelectedValue;
                GMEnt.GL_MASTER_NAME = txtGlName.Text;
                GMEnt.ORDER_BY = txtOrderBy.Text;
                GMEnt.BS_HEADING = bsHead;
                GMSer.Update(GMEnt);               

            }
            HelperFunction.MsgBox(this, this.GetType(), "Updated");
            lblPK_id.Text = "";
        }
        LoadGrid();
    }
    protected void LoadGrid()
    {
        string bsHead = "";
        if (ddlHeading.SelectedValue != "Select")
        {
            bsHead = ddlHeading.SelectedValue;
        }
        GMEnt = new GL_ACCOUNT_MASTER();
        GMEnt.ACC_HEAD_CODE = ddlAccountsHead.SelectedValue;
        GMEnt.BS_HEADING = bsHead;
        gridMaster.DataSource = GMSer.GetAll(GMEnt);
        gridMaster.DataBind();

    }

    protected void gridMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFinancialStatement = (Label)e.Row.FindControl("lblFinancialStatement");
            Label lblFS = (Label)e.Row.FindControl("lblFS");

            if (lblFinancialStatement.Text != "IS")
            {
                lblFS.Text = "Balance Sheet";
            }
            else
            {
                lblFS.Text = "Income Statement";
            }
        }
    }

    protected void gridMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("View"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblpkid = gr.FindControl("lblpkid") as Label;
            GMEnt = new GL_ACCOUNT_MASTER();
            GMEnt.PK_ID = lblpkid.Text;
            GMEnt = (GL_ACCOUNT_MASTER)GMSer.GetSingle(GMEnt);
            if (GMEnt != null)
            {
                lblPK_id.Text = lblpkid.Text;
                ddlAccountsHead.SelectedValue = GMEnt.ACC_HEAD_CODE;
                ddlFinancialStatement.SelectedValue = GMEnt.FINANCIAL_STATEMENT;
                txtCode.Text = GMEnt.GL_MASTER_CODE;
                txtGlName.Text = GMEnt.GL_MASTER_NAME;
                txtOrderBy.Text = GMEnt.ORDER_BY;
            }
        }
    }

    protected void ddlHeading_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid();
    }
}