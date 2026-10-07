using Entity.Components;
using Entity.Framework;
using PhyeGanCore;
using Service.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_Reports_ProfitLoss : System.Web.UI.Page
{
    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

    GL_ACCOUNT_MASTER GLAMEnt = new GL_ACCOUNT_MASTER();
    GL_ACCOUNT_MASTERService GLAMSer = new GL_ACCOUNT_MASTERService();
    GL_ACCOUNT GLAEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLASer = new GL_ACCOUNTService();

    HelperFunction hf = new HelperFunction();
    AccountFunction af = new AccountFunction();

    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();

    double total_asset_this_year = 0;
    double total_asset_last_year = 0;
    double total_equity_this_year = 0;
    double total_equity_last_year = 0;
    double total_labilities_this_year = 0;
    double total_labilities_last_year = 0;
    string pfy = "";

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
                    LoadCompany();
                    LoadFiscalYear();
                    loadBranch();
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
            divBranch.Visible = true;
            ddlBranch.Items.Insert(0, "");
        }
        else
        {
            divBranch.Visible = false;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
        }
    }
    protected void LoadCompany()
    {
        lblCompanyName.Text = PG.CompanyName();
        lblAddress.Text = PG.BranchAddress(ddlBranch.SelectedValue);
        lblContact.Text = PG.BranchContact(ddlBranch.SelectedValue);
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


    protected void btnShow_Click(object sender, EventArgs e)
    {
        string[] fy = ddlFiscalYear.SelectedValue.Split('/');
        string year = (Convert.ToDouble(fy[0]) + 1).ToString();
        lblDate.Text = "Aashad," + PGD.NepaliMonthsLastDay(year, "3").ToString() + " " + year;
        pfy = (Convert.ToDouble(fy[0]) - 1).ToString() + "/" + (Convert.ToDouble(fy[1]) - 1).ToString();

        lblThisYearDate.Text = "Aashad," + PGD.NepaliMonthsLastDay(year, "3").ToString() + " " + year;
        lblLastYearDate.Text = "Aashad," + PGD.NepaliMonthsLastDay(fy[0], "3").ToString() + " " + fy[0];
        divPrint.Visible = true;
        LoadGLMaster();
        LoadGLMasterExpenses();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void LoadGLMaster()
    {
        GLAMEnt = new GL_ACCOUNT_MASTER();
        GLAMEnt.ACC_HEAD_CODE = "02";

        grdGL_MASTER_Income.DataSource = GLAMSer.GetAll(GLAMEnt);
        grdGL_MASTER_Income.DataBind();
    }
    
    protected void LoadGLMasterExpenses()
    {
        GLAMEnt = new GL_ACCOUNT_MASTER();
        GLAMEnt.ACC_HEAD_CODE = "03";

        grdGL_MASTER_Expenses.DataSource = GLAMSer.GetAll(GLAMEnt);
        grdGL_MASTER_Expenses.DataBind();
    }


    protected void grdGL_MASTER_Income_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblGLMaster_code = e.Row.FindControl("lblGLMaster_code") as Label;
            Label lblGLMaster_Name = e.Row.FindControl("lblGLMaster_Name") as Label;
            GridView grdGLAccount_Income = e.Row.FindControl("grdGLAccount_Income") as GridView;

            DataTable newDT = hf.getBalanceSheet(ddlFiscalYear.SelectedValue, lblGLMaster_code.Text, "");
            if (newDT.Rows.Count > 0)
            {
                grdGLAccount_Income.DataSource = newDT;
                grdGLAccount_Income.DataBind();
                lblGLMaster_Name.Visible = true;
            }
            else
            {
                lblGLMaster_Name.Visible = false;
            }

            foreach (GridViewRow inner_gr in grdGLAccount_Income.Rows)
            {
                Label lblThisYearBalance = inner_gr.FindControl("lblThisYearBalance") as Label;
                Label lblLastYearBalance = inner_gr.FindControl("lblLastYearBalance") as Label;
                try
                {
                    lblThisYearBalance.Text = Convert.ToDouble(lblThisYearBalance.Text).ToString("0.00");
                    total_asset_this_year += Convert.ToDouble(lblThisYearBalance.Text);
                    total_asset_last_year += Convert.ToDouble(lblLastYearBalance.Text);
                }
                catch { }
            }

            lblThisYearAssetTotal.Text = total_asset_this_year.ToString("0.00");
            lblLastYearAssetTotal.Text = total_asset_last_year.ToString("0.00");
        }
    }
    protected void grdGL_MASTER_Expenses_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblGLMaster_code = e.Row.FindControl("lblGLMaster_code") as Label;
            Label lblGLMaster_Name = e.Row.FindControl("lblGLMaster_Name") as Label;
            GridView grdGLAccount_Expenses = e.Row.FindControl("grdGLAccount_Expenses") as GridView;

            DataTable newDT = hf.getBalanceSheet(ddlFiscalYear.SelectedValue, lblGLMaster_code.Text, "");
            if (newDT.Rows.Count > 0)
            {
                grdGLAccount_Expenses.DataSource = newDT;
                grdGLAccount_Expenses.DataBind();
                lblGLMaster_Name.Visible = true;
            }
            else
            {
                lblGLMaster_Name.Visible = false;
            }

            foreach (GridViewRow inner_gr in grdGLAccount_Expenses.Rows)
            {
                Label lblThisYearBalance = inner_gr.FindControl("lblThisYearBalance") as Label;
                Label lblLastYearBalance = inner_gr.FindControl("lblLastYearBalance") as Label;
                try
                {
                    lblThisYearBalance.Text = (Convert.ToDouble(lblThisYearBalance.Text)*-1).ToString("0.00");
                    total_equity_this_year += Convert.ToDouble(lblThisYearBalance.Text);
                    total_equity_last_year += Convert.ToDouble(lblLastYearBalance.Text);
                }
                catch { }
            }

            lblThisYearEquityTotal.Text = total_equity_this_year.ToString("0.00");
            lblLastYearEquityTotal.Text = total_equity_last_year.ToString("0.00");
        }
    }
   protected void grdGLAccount_Income_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblGL_Master_Code = e.Row.FindControl("lblGL_Master_Code") as Label;
            Label lblGL_Code = e.Row.FindControl("lblGL_Code") as Label;
            Label lblLastYearBalance = e.Row.FindControl("lblLastYearBalance") as Label;
            DataTable newDT = hf.getBalanceSheet(pfy, lblGL_Master_Code.Text, lblGL_Code.Text);
            if (newDT.Rows.Count > 0)
            {
                lblLastYearBalance.Text = newDT.Rows[0][5].ToString();
                try
                {
                    lblLastYearBalance.Text = Convert.ToDouble(lblLastYearBalance.Text).ToString("0.00");
                }
                catch { }
            }
            else
            {
                lblLastYearBalance.Text = "0.00";
            }
        }
    }
    protected void grdGLAccount_Expenses_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblGL_Master_Code = e.Row.FindControl("lblGL_Master_Code") as Label;
            Label lblGL_Code = e.Row.FindControl("lblGL_Code") as Label;
            Label lblLastYearBalance = e.Row.FindControl("lblLastYearBalance") as Label;
            DataTable newDT = hf.getBalanceSheet(pfy, lblGL_Master_Code.Text, lblGL_Code.Text);
            if (newDT.Rows.Count > 0)
            {
                lblLastYearBalance.Text = newDT.Rows[0][5].ToString();
                try
                {
                    lblLastYearBalance.Text = Convert.ToDouble(lblLastYearBalance.Text).ToString("0.00");
                }
                catch { }
            }
            else
            {
                lblLastYearBalance.Text = "0.00";
            }
        }
    }
  }