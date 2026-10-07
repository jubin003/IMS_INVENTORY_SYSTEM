using Entity.Components;
using PhyeGanCore;
using Service.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_Reports_BalanceSheet : System.Web.UI.Page
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
    string pfy = "";

    double asset_this_year = 0;
    double asset_last_year = 0;
    double total_asset_this_year = 0;
    double total_asset_last_year = 0;

    double equity_this_year = 0;
    double equity_last_year = 0;
    double total_equity_this_year = 0;
    double total_equity_last_year = 0;

    double labilities_this_year = 0;
    double labilities_last_year = 0;
    double total_labilities_this_year = 0;
    double total_labilities_last_year = 0;

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
        lblAddress.Text = PG.CompanyAddress();
        lblContact.Text = PG.CompanyContact();
        if (ddlBranch.SelectedValue != "")
        {
            lblAddress.Text = PG.BranchAddress(ddlBranch.SelectedValue);
            lblContact.Text = PG.BranchContact(ddlBranch.SelectedValue);            
        }
        else
        {
            lblAddress.Text = "";
            lblContact.Text = "";
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


    protected void btnShow_Click(object sender, EventArgs e)
    {
        LoadCompany();
        string[] fy = ddlFiscalYear.SelectedValue.Split('/');
        string year = (Convert.ToDouble(fy[0]) + 1).ToString();
        lblDate.Text = "Aashad," + PGD.NepaliMonthsLastDay(year, "3").ToString() + " " + year;
        pfy = (Convert.ToDouble(fy[0]) - 1).ToString() + "/" + (Convert.ToDouble(fy[1]) - 1).ToString();

        lblThisYearDate.Text = "Aashad," + PGD.NepaliMonthsLastDay(year, "3").ToString() + " " + year;
        lblLastYearDate.Text = "Aashad," + PGD.NepaliMonthsLastDay(fy[0], "3").ToString() + " " + fy[0];
        divPrint.Visible = true;

        grd_Asset.DataSource = af.getBalanceSheetHeading("Assets");
        grd_Asset.DataBind();

        grd_Equity.DataSource = af.getBalanceSheetHeading("Equity");
        grd_Equity.DataBind();

        grd_Labilities.DataSource = af.getBalanceSheetHeading("Labilities");
        grd_Labilities.DataBind();

        lblThisYearLabilitiesEquityTotalGrand.Text = (total_equity_this_year + total_labilities_this_year).ToString("##,##0.00");
        lblLastYearLabilitiesEquityTotalGrand.Text = (total_equity_last_year + total_labilities_last_year).ToString("##,##0.00");
    }



    protected void grd_Asset_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        asset_this_year = 0;
        asset_last_year = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblBS_HEADING_PK_ID = e.Row.FindControl("lblBS_HEADING_PK_ID") as Label;
            Label lblThisYearTotal = e.Row.FindControl("lblThisYearTotal") as Label;
            Label lblLastYearTotal = e.Row.FindControl("lblLastYearTotal") as Label;
            GridView grd_Detail = e.Row.FindControl("grd_Detail") as GridView;

            grd_Detail.DataSource = af.getBalanceSheetSubHeading(lblBS_HEADING_PK_ID.Text);
            grd_Detail.DataBind();


            foreach (GridViewRow inner_gr in grd_Detail.Rows)
            {
                Label lblThisYearBalance = inner_gr.FindControl("lblThisYearBalance") as Label;
                Label lblLastYearBalance = inner_gr.FindControl("lblLastYearBalance") as Label;
                try
                {
                    lblThisYearBalance.Text = Convert.ToDouble(lblThisYearBalance.Text).ToString("##,##0.00");
                    lblLastYearBalance.Text = Convert.ToDouble(lblLastYearBalance.Text).ToString("##,##0.00");
                    asset_this_year += Convert.ToDouble(lblThisYearBalance.Text);
                    asset_last_year += Convert.ToDouble(lblLastYearBalance.Text);
                }
                catch { }
            }
            lblThisYearTotal.Text = asset_this_year.ToString("##,##0.00");
            lblLastYearTotal.Text = asset_last_year.ToString("##,##0.00");

            total_asset_this_year += asset_this_year;
            total_asset_last_year += asset_last_year;

            lblThisYearAssetTotalGrand.Text = total_asset_this_year.ToString("##,##0.00");
            lblLastYearAssetTotalGrand.Text = total_asset_last_year.ToString("##,##0.00");
        }
    }


    protected void grd_Equity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        equity_this_year = 0;
        equity_last_year = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblBS_HEADING_PK_ID = e.Row.FindControl("lblBS_HEADING_PK_ID") as Label;
            Label lblThisYearTotal = e.Row.FindControl("lblThisYearTotal") as Label;
            Label lblLastYearTotal = e.Row.FindControl("lblLastYearTotal") as Label;
            GridView grd_Detail = e.Row.FindControl("grd_Detail") as GridView;

            grd_Detail.DataSource = af.getBalanceSheetSubHeading(lblBS_HEADING_PK_ID.Text);
            grd_Detail.DataBind();


            foreach (GridViewRow inner_gr in grd_Detail.Rows)
            {
                Label lblThisYearBalance = inner_gr.FindControl("lblThisYearBalance") as Label;
                Label lblLastYearBalance = inner_gr.FindControl("lblLastYearBalance") as Label;
                try
                {
                    lblThisYearBalance.Text = Convert.ToDouble(lblThisYearBalance.Text).ToString("##,##0.00");
                    lblLastYearBalance.Text = Convert.ToDouble(lblLastYearBalance.Text).ToString("##,##0.00");
                    equity_this_year += Convert.ToDouble(lblThisYearBalance.Text);
                    equity_last_year += Convert.ToDouble(lblLastYearBalance.Text);
                }
                catch { }
            }
            lblThisYearTotal.Text = equity_this_year.ToString("##,##0.00");
            lblLastYearTotal.Text = equity_last_year.ToString("##,##0.00");

            total_equity_this_year += equity_this_year;
            total_equity_last_year += equity_last_year;

            lblThisYearEquityTotalGrand.Text = total_equity_this_year.ToString("##,##0.00");
            lblLastYearEquityTotalGrand.Text = total_equity_last_year.ToString("##,##0.00");
        }


    }

    protected void grd_Labilities_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        labilities_this_year = 0;
        labilities_last_year = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblBS_HEADING_PK_ID = e.Row.FindControl("lblBS_HEADING_PK_ID") as Label;
            Label lblThisYearTotal = e.Row.FindControl("lblThisYearTotal") as Label;
            Label lblLastYearTotal = e.Row.FindControl("lblLastYearTotal") as Label;
            GridView grd_Detail = e.Row.FindControl("grd_Detail") as GridView;

            grd_Detail.DataSource = af.getBalanceSheetSubHeading(lblBS_HEADING_PK_ID.Text);
            grd_Detail.DataBind();


            foreach (GridViewRow inner_gr in grd_Detail.Rows)
            {
                Label lblThisYearBalance = inner_gr.FindControl("lblThisYearBalance") as Label;
                Label lblLastYearBalance = inner_gr.FindControl("lblLastYearBalance") as Label;
                try
                {
                    lblThisYearBalance.Text = Convert.ToDouble(lblThisYearBalance.Text).ToString("##,##0.00");
                    lblLastYearBalance.Text = Convert.ToDouble(lblLastYearBalance.Text).ToString("##,##0.00");
                    labilities_this_year += Convert.ToDouble(lblThisYearBalance.Text);
                    labilities_last_year += Convert.ToDouble(lblLastYearBalance.Text);
                }
                catch { }
            }
            lblThisYearTotal.Text = labilities_this_year.ToString("##,##0.00");
            lblLastYearTotal.Text = labilities_last_year.ToString("##,##0.00");

            total_labilities_this_year += labilities_this_year;
            total_labilities_last_year += labilities_last_year;

            lblThisYearLabilitiesTotalGrand.Text = total_labilities_this_year.ToString("##,##0.00");
            lblLastYearLabilitiesTotalGrand.Text = total_labilities_last_year.ToString("##,##0.00");
        }


    }



    protected void grd_Detail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
        {
            office_code = ddlBranch.SelectedValue;
        }

        string currentFY = ddlFiscalYear.SelectedValue;
        string prevFY = PGD.getPrevious_FiscalYear(currentFY);

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblBS_SUB_HEADING_PK_ID = e.Row.FindControl("lblBS_SUB_HEADING_PK_ID") as Label;
            Label lblThisYearBalance = e.Row.FindControl("lblThisYearBalance") as Label;
            Label lblLastYearBalance = e.Row.FindControl("lblLastYearBalance") as Label;
            lblThisYearBalance.Text = af.getBalanceSheet(currentFY, lblBS_SUB_HEADING_PK_ID.Text, office_code);
            lblLastYearBalance.Text = af.getBalanceSheet(prevFY, lblBS_SUB_HEADING_PK_ID.Text, office_code);

        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {

    }


}