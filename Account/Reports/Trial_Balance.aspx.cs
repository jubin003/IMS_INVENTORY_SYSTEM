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

public partial class Account_Reports_Trial_Balance : System.Web.UI.Page
{
    GL_ACCOUNT GLEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLSer = new GL_ACCOUNTService();

    GL_ACCOUNT_MASTER GMEnt = new GL_ACCOUNT_MASTER();
    GL_ACCOUNT_MASTERService GMSer = new GL_ACCOUNT_MASTERService();

    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

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
                    lblDate.Text = PGD.GetTodayDate("dd/mm/yyyy");
                    txtFromDate.Text = PGD.GetTodayNepaliDate();
                    txtToDate.Text = PGD.GetTodayNepaliDate();
                    LoadFiscalYear();
                    ddlFiscalYear.SelectedValue = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
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
        ddlFiscalYear.DataSource = FYSer.GetAll(FYEnt);
        ddlFiscalYear.DataTextField = "FISCAL_YEAR";
        ddlFiscalYear.DataValueField = "FISCAL_YEAR";
        ddlFiscalYear.DataBind();
    }

    double drTotal = 0;
    double crTotal = 0;


    protected void LoadLedgerTrialBalance()
    {
        try
        {
            string[] fromdate = txtFromDate.Text.Split('/');
            string[] todate = txtToDate.Text.Split('/');
            string from_date = PGD.ConvertNepaliTOEnglish(fromdate[0], fromdate[1], fromdate[2]);
            string to_date = PGD.ConvertNepaliTOEnglish(todate[0], todate[1], todate[2]);
            if (ddlBranch.SelectedValue == "")
            {
                gridTrialBalace.DataSource = af.getLedgerTrialBalance(from_date, to_date, ddlFiscalYear.SelectedValue, "");
                gridTrialBalace.DataBind();
            }
            else
            {
                gridTrialBalace.DataSource = af.getLedgerTrialBalance(from_date, to_date, ddlFiscalYear.SelectedValue, ddlBranch.SelectedValue);
                gridTrialBalace.DataBind();
            }
        }
        catch { }
    }

    double TBOpeningdrTotal = 0;
    double TBOpeningcrTotal = 0;
    double TBdrTotal = 0;
    double TBcrTotal = 0;
    double TBTotaldrTotal = 0;
    double TBTotalcrTotal = 0;
    protected void gridTrialBalace_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblOpeningDrAmount = e.Row.FindControl("lblOpeningDrAmount") as Label;
            Label lblOpeningCrAmount = e.Row.FindControl("lblOpeningCrAmount") as Label;

            Label lblDrAmount = e.Row.FindControl("lblDrAmount") as Label;
            Label lblCrAmount = e.Row.FindControl("lblCrAmount") as Label;

            Label lblTotalDrAmount = e.Row.FindControl("lblTotalDrAmount") as Label;
            Label lblTotalCrAmount = e.Row.FindControl("lblTotalCrAmount") as Label;

            Label lblVoucherType = e.Row.FindControl("lblVoucherType") as Label;
            Label lblShowVoucherType = e.Row.FindControl("lblShowVoucherType") as Label;

            if (lblDrAmount != null)
            {
                double drOpeningAmount = Convert.ToDouble(lblOpeningDrAmount.Text);
                double drAmount = Convert.ToDouble(lblDrAmount.Text);
                double drTotalAmount = Convert.ToDouble(lblTotalDrAmount.Text);

                lblOpeningDrAmount.Text = drOpeningAmount.ToString("#,##0.00");
                lblDrAmount.Text = drAmount.ToString("#,##0.00");
                lblTotalDrAmount.Text = drTotalAmount.ToString("#,##0.00");

                TBOpeningdrTotal += drOpeningAmount;
                TBdrTotal += drAmount;
                TBTotaldrTotal += drTotalAmount;
            }

            if (lblCrAmount != null)
            {
                double crOpeningAmount = Convert.ToDouble(lblOpeningCrAmount.Text);
                double crAmount = Convert.ToDouble(lblCrAmount.Text);
                double crTotalAmount = Convert.ToDouble(lblTotalCrAmount.Text);
                lblOpeningCrAmount.Text = crOpeningAmount.ToString("#,##0.00");
                lblCrAmount.Text = crAmount.ToString("#,##0.00");
                lblTotalCrAmount.Text = crTotalAmount.ToString("#,##0.00");


                TBOpeningcrTotal += crOpeningAmount;
                TBcrTotal += crAmount;
                TBTotalcrTotal += crTotalAmount;
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblOpeningDrTotal = e.Row.FindControl("lblOpeningDrTotal") as Label;
            Label lblDrTotal = e.Row.FindControl("lblDrTotal") as Label;
            Label lblTotalDrTotal = e.Row.FindControl("lblTotalDrTotal") as Label;
            Label lblOpeningCrTotal = e.Row.FindControl("lblOpeningCrTotal") as Label;
            Label lblCrTotal = e.Row.FindControl("lblCrTotal") as Label;
            Label lblTotalCrTotal = e.Row.FindControl("lblTotalCrTotal") as Label;

            if (lblDrTotal != null)
            {
                lblOpeningDrTotal.Text = TBOpeningdrTotal.ToString("#,##0.00");
                lblDrTotal.Text = TBdrTotal.ToString("#,##0.00");
                lblTotalDrTotal.Text = TBTotaldrTotal.ToString("#,##0.00");
            }

            if (lblCrTotal != null)
            {
                lblOpeningCrTotal.Text = TBOpeningcrTotal.ToString("#,##0.00");
                lblCrTotal.Text = TBcrTotal.ToString("#,##0.00");
                lblTotalCrTotal.Text = TBTotalcrTotal.ToString("#,##0.00");
            }
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        LoadCompany();
        divGroupTrialBalance.Visible = false;
        divLedgerTrialBalance.Visible = false;
        divAccountTrialBalance.Visible = false;
        if (ddlTrialbalanceType.SelectedValue != "Select")
        {
            divTrialBalance.Visible = true;
            if (ddlTrialbalanceType.SelectedValue == "Group Trial Balance")
            {
                lblTrialBalanceType.Text = "Group Trial Balance";
                divGroupTrialBalance.Visible = true;

                LoadGroupTrialbalance();
            }
            else if (ddlTrialbalanceType.SelectedValue == "Ledger Trial Balance")
            {
                lblTrialBalanceType.Text = "Ledger Trial Balance";
                LoadLedgerTrialBalance();
                divLedgerTrialBalance.Visible = true;
            }
            else
            {
                lblTrialBalanceType.Text = "Account Trial Balance";
                divAccountTrialBalance.Visible = true;
                LoadAccountTrialbalance();
            }
        }
    }
    protected void LoadGroupTrialbalance()
    {
        try
        {
            string[] fromdate = txtFromDate.Text.Split('/');
            string[] todate = txtToDate.Text.Split('/');
            string from_date = PGD.ConvertNepaliTOEnglish(fromdate[0], fromdate[1], fromdate[2]);
            string to_date = PGD.ConvertNepaliTOEnglish(todate[0], todate[1], todate[2]);
            if (ddlBranch.SelectedValue == "")
            {
                gridGroupTrialBalance.DataSource = af.getGroupTrialBalance(from_date, to_date, ddlFiscalYear.SelectedValue, "");
                gridGroupTrialBalance.DataBind();
            }
            else
            {
                gridGroupTrialBalance.DataSource = af.getGroupTrialBalance(from_date, to_date, ddlFiscalYear.SelectedValue, ddlBranch.SelectedValue);
                gridGroupTrialBalance.DataBind();
            }
        }
        catch { }
    }

    protected void gridGroupTrialBalance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblOpeningDrAmount = e.Row.FindControl("lblOpeningDrAmount") as Label;
            Label lblOpeningCrAmount = e.Row.FindControl("lblOpeningCrAmount") as Label;

            Label lblDrAmount = e.Row.FindControl("lblDrAmount") as Label;
            Label lblCrAmount = e.Row.FindControl("lblCrAmount") as Label;

            Label lblTotalDrAmount = e.Row.FindControl("lblTotalDrAmount") as Label;
            Label lblTotalCrAmount = e.Row.FindControl("lblTotalCrAmount") as Label;

            Label lblVoucherType = e.Row.FindControl("lblVoucherType") as Label;
            Label lblShowVoucherType = e.Row.FindControl("lblShowVoucherType") as Label;

            if (lblDrAmount != null)
            {
                double drOpeningAmount = Convert.ToDouble(lblOpeningDrAmount.Text);
                double drAmount = Convert.ToDouble(lblDrAmount.Text);
                double drTotalAmount = Convert.ToDouble(lblTotalDrAmount.Text);

                lblOpeningDrAmount.Text = drOpeningAmount.ToString("#,##0.00");
                lblDrAmount.Text = drAmount.ToString("#,##0.00");
                lblTotalDrAmount.Text = drTotalAmount.ToString("#,##0.00");

                TBOpeningdrTotal += drOpeningAmount;
                TBdrTotal += drAmount;
                TBTotaldrTotal += drTotalAmount;
            }

            if (lblCrAmount != null)
            {
                double crOpeningAmount = Convert.ToDouble(lblOpeningCrAmount.Text);
                double crAmount = Convert.ToDouble(lblCrAmount.Text);
                double crTotalAmount = Convert.ToDouble(lblTotalCrAmount.Text);
                lblOpeningCrAmount.Text = crOpeningAmount.ToString("#,##0.00");
                lblCrAmount.Text = crAmount.ToString("#,##0.00");
                lblTotalCrAmount.Text = crTotalAmount.ToString("#,##0.00");


                TBOpeningcrTotal += crOpeningAmount;
                TBcrTotal += crAmount;
                TBTotalcrTotal += crTotalAmount;
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblOpeningDrTotal = e.Row.FindControl("lblOpeningDrTotal") as Label;
            Label lblDrTotal = e.Row.FindControl("lblDrTotal") as Label;
            Label lblTotalDrTotal = e.Row.FindControl("lblTotalDrTotal") as Label;
            Label lblOpeningCrTotal = e.Row.FindControl("lblOpeningCrTotal") as Label;
            Label lblCrTotal = e.Row.FindControl("lblCrTotal") as Label;
            Label lblTotalCrTotal = e.Row.FindControl("lblTotalCrTotal") as Label;

            if (lblDrTotal != null)
            {
                lblOpeningDrTotal.Text = TBOpeningdrTotal.ToString("#,##0.00");
                lblDrTotal.Text = TBdrTotal.ToString("#,##0.00");
                lblTotalDrTotal.Text = TBTotaldrTotal.ToString("#,##0.00");
            }

            if (lblCrTotal != null)
            {
                lblOpeningCrTotal.Text = TBOpeningcrTotal.ToString("#,##0.00");
                lblCrTotal.Text = TBcrTotal.ToString("#,##0.00");
                lblTotalCrTotal.Text = TBTotalcrTotal.ToString("#,##0.00");
            }
        }
    }

    protected void gridAccountTrialBalance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblOpeningDrAmount = e.Row.FindControl("lblOpeningDrAmount") as Label;
            Label lblOpeningCrAmount = e.Row.FindControl("lblOpeningCrAmount") as Label;

            Label lblDrAmount = e.Row.FindControl("lblDrAmount") as Label;
            Label lblCrAmount = e.Row.FindControl("lblCrAmount") as Label;

            Label lblTotalDrAmount = e.Row.FindControl("lblTotalDrAmount") as Label;
            Label lblTotalCrAmount = e.Row.FindControl("lblTotalCrAmount") as Label;

            Label lblVoucherType = e.Row.FindControl("lblVoucherType") as Label;
            Label lblShowVoucherType = e.Row.FindControl("lblShowVoucherType") as Label;

            if (lblDrAmount != null)
            {
                double drOpeningAmount = Convert.ToDouble(lblOpeningDrAmount.Text);
                double drAmount = Convert.ToDouble(lblDrAmount.Text);
                double drTotalAmount = Convert.ToDouble(lblTotalDrAmount.Text);

                lblOpeningDrAmount.Text = drOpeningAmount.ToString("#,##0.00");
                lblDrAmount.Text = drAmount.ToString("#,##0.00");
                lblTotalDrAmount.Text = drTotalAmount.ToString("#,##0.00");

                TBOpeningdrTotal += drOpeningAmount;
                TBdrTotal += drAmount;
                TBTotaldrTotal += drTotalAmount;
            }

            if (lblCrAmount != null)
            {
                double crOpeningAmount = Convert.ToDouble(lblOpeningCrAmount.Text);
                double crAmount = Convert.ToDouble(lblCrAmount.Text);
                double crTotalAmount = Convert.ToDouble(lblTotalCrAmount.Text);
                lblOpeningCrAmount.Text = crOpeningAmount.ToString("#,##0.00");
                lblCrAmount.Text = crAmount.ToString("#,##0.00");
                lblTotalCrAmount.Text = crTotalAmount.ToString("#,##0.00");


                TBOpeningcrTotal += crOpeningAmount;
                TBcrTotal += crAmount;
                TBTotalcrTotal += crTotalAmount;
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblOpeningDrTotal = e.Row.FindControl("lblOpeningDrTotal") as Label;
            Label lblDrTotal = e.Row.FindControl("lblDrTotal") as Label;
            Label lblTotalDrTotal = e.Row.FindControl("lblTotalDrTotal") as Label;
            Label lblOpeningCrTotal = e.Row.FindControl("lblOpeningCrTotal") as Label;
            Label lblCrTotal = e.Row.FindControl("lblCrTotal") as Label;
            Label lblTotalCrTotal = e.Row.FindControl("lblTotalCrTotal") as Label;

            if (lblDrTotal != null)
            {
                lblOpeningDrTotal.Text = TBOpeningdrTotal.ToString("#,##0.00");
                lblDrTotal.Text = TBdrTotal.ToString("#,##0.00");
                lblTotalDrTotal.Text = TBTotaldrTotal.ToString("#,##0.00");
            }

            if (lblCrTotal != null)
            {
                lblOpeningCrTotal.Text = TBOpeningcrTotal.ToString("#,##0.00");
                lblCrTotal.Text = TBcrTotal.ToString("#,##0.00");
                lblTotalCrTotal.Text = TBTotalcrTotal.ToString("#,##0.00");
            }
        }
    }
    protected void LoadAccountTrialbalance()
    {
        try
        {
            string[] fromdate = txtFromDate.Text.Split('/');
            string[] todate = txtToDate.Text.Split('/');
            string from_date = PGD.ConvertNepaliTOEnglish(fromdate[0], fromdate[1], fromdate[2]);
            string to_date = PGD.ConvertNepaliTOEnglish(todate[0], todate[1], todate[2]);
            if (ddlBranch.SelectedValue == "")
            {
                gridAccountTrialBalance.DataSource = af.getAccountTrialBalance(from_date, to_date, ddlFiscalYear.SelectedValue, "");
                gridAccountTrialBalance.DataBind();
            }
            else
            {
                gridAccountTrialBalance.DataSource = af.getAccountTrialBalance(from_date, to_date, ddlFiscalYear.SelectedValue, ddlBranch.SelectedValue);
                gridAccountTrialBalance.DataBind();
            }
        }
        catch { }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}