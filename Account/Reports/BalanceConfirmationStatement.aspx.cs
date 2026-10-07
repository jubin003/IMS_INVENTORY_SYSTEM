using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.IO;
using PhyeGanCore;
using Entity.Framework;

public partial class Account_Reports_BalanceConfirmationStatement : System.Web.UI.Page
{
    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();
    GL_ACCOUNT GLEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLSer = new GL_ACCOUNTService();

    GL_ACCOUNT_MASTER GMEnt = new GL_ACCOUNT_MASTER();
    GL_ACCOUNT_MASTERService GMSer = new GL_ACCOUNT_MASTERService();

    GL_SUB_ACCOUNT GLSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GLSASer = new GL_SUB_ACCOUNTService();

    CUSTOMER CUSEnt = new CUSTOMER();
    CUSTOMERService CUSSer = new CUSTOMERService();

    HelperFunction hf = new HelperFunction();
    AccountFunction af = new AccountFunction();

    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();

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
                    loadBranch();
                    LoadCompany();
                    LoadFiscalYear();
                    LoadGLName();
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
    protected void loadBranch()
    {
        ddlBranch.DataSource = PG.getBranchList();
        ddlBranch.DataTextField = "OFFICENAME";
        ddlBranch.DataValueField = "PK_ID";
        ddlBranch.DataBind();

        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (userProfileEnt.LocationTypeID == 1 && PG.checkBranchAccess(path, userProfileEnt.UserGroupID.ToString()) && PG.CompanyBranch_Status())
        {
            trBranch.Visible = true;
            ddlBranch.Items.Insert(0, "");
        }
        else
        {
            trBranch.Visible = false;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
        }
    }
    protected void LoadCompany()
    {
        lblCompanyName.Text = PG.CompanyName();
        if (ddlBranch.SelectedValue != "")
        {
            lblCompanyAddress.Text = PG.BranchAddress(ddlBranch.SelectedValue);
            lblPhone1.Text = PG.BranchContact(ddlBranch.SelectedValue);
        }
        else
        {
            lblCompanyAddress.Text = "";
            lblPhone1.Text = "";
        }

    }
    protected void LoadFiscalYear()
    {
        FYEnt = new FISCALYEAR();
        FYEnt.ACTIVE = "1";
        ddlFiscalYear.DataSource = FYSer.GetAll(FYEnt);
        ddlFiscalYear.DataTextField = "FISCAL_YEAR";
        ddlFiscalYear.DataValueField = "FISCAL_YEAR";
        ddlFiscalYear.DataBind();
    }

    protected void LoadGLName()
    {
        GLEnt = new GL_ACCOUNT();
        GLEnt.GL_MASTER_CODE = "0103";
        ddlGLName.DataSource = GLSer.GetAll(GLEnt);
        ddlGLName.DataTextField = "GL_NAME";
        ddlGLName.DataValueField = "GL_CODE";
        ddlGLName.DataBind();
        ddlGLName.Items.Insert(0, "Select");
    }
    protected void LoadSubGLName()
    {
        GLSAEnt = new GL_SUB_ACCOUNT();
        GLSAEnt.GL_CODE = ddlGLName.SelectedValue;
        EntityList theList = new EntityList();
        theList = GLSASer.GetAll(GLSAEnt);
        if (theList.Count > 0)
        {
            ddlsubLedger.DataSource = theList;
            ddlsubLedger.DataTextField = "SUB_GL_NAME";
            ddlsubLedger.DataValueField = "SUB_GL_CODE";
            ddlsubLedger.DataBind();
        }
        else
        {
            ddlsubLedger.Items.Clear();
        }

        ddlsubLedger.Items.Insert(0, "Select");

    }
    protected void ddlGLName_SelectedIndexChanged(object sender, EventArgs e)
    {
        GLEnt = new GL_ACCOUNT();
        GLEnt.GL_CODE = ddlGLName.SelectedValue;
        GLEnt = (GL_ACCOUNT)GLSer.GetSingle(GLEnt);
        if (GLEnt != null)
        {
            if (GLEnt.SUB_LEDGER == "1")
            {
                LoadSubGLName();
                lblSubLedger.Visible = true;
                ddlsubLedger.Visible = true;
            }
            else
            {
                lblSubLedger.Visible = false;
                ddlsubLedger.Visible = false;
                ddlsubLedger.Items.Clear();
                ddlsubLedger.Items.Insert(0, "Select");
            }
        }
        else
        {
            ddlsubLedger.Items.Clear();
        }
    }

    protected void btn_view_Click(object sender, EventArgs e)
    {
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
            office_code = ddlBranch.SelectedValue;
        if (ddlGLName.SelectedIndex == 1)
        {
            CUSEnt = new CUSTOMER();
            CUSEnt.CUSTOMER_CODE = ddlsubLedger.SelectedValue;

            CUSEnt = (CUSTOMER)CUSSer.GetSingle(CUSEnt);
            if (CUSEnt != null)
            {
                btnPrint.Visible = true;
                divReport.Visible = true;
                lblCusName.Text = CUSEnt.CUSTOMER_FULLNAME;
                lblCusAddress.Text = CUSEnt.ADDRESS;
                lblCusPan.Text = CUSEnt.VAT_PAN_NUMBER;
                lblDate.Text = PGD.GetTodayNepaliDate();
                lblFyYear.Text = ddlFiscalYear.SelectedValue;
                lblFy.Text = ddlFiscalYear.SelectedValue;
                string[] yearOnly = lblFy.Text.Split('/');
                lblFYear.Text = yearOnly[0];

                lblCompanyN.Text = PG.CompanyName();
                gridBCS.DataSource = af.GET_CONFIRMATION_OF_ACCOUNTS(CUSEnt.CUSTOMER_CODE, ddlFiscalYear.SelectedValue,office_code);
                gridBCS.DataBind();

            }
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }
}