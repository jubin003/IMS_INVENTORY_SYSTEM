using System;
using System.Web;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DataHelper.Framework;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.UI;
using System.Net;
using System.Net.Security;
using System.Collections;
using System.IO;

public partial class Account_Utilities_opening_balance_sundry : System.Web.UI.Page
{
    GL_ACCOUNT GLEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLSer = new GL_ACCOUNTService();

    GL_ACCOUNT_MASTER GMEnt = new GL_ACCOUNT_MASTER();
    GL_ACCOUNT_MASTERService GMSer = new GL_ACCOUNT_MASTERService();

    GL_SUB_ACCOUNT GLSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GLSASer = new GL_SUB_ACCOUNTService();

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
                    loadFiscalYear();
                    LoadCompany();
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
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
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
        lblDate.Text = ddlFiscalYear.SelectedValue;
        lblAddress.Text = PG.BranchAddress(ddlBranch.SelectedValue);
        lblContact.Text = PG.BranchContact(ddlBranch.SelectedValue);

    }

    protected void loadFiscalYear()
    {
        FYEnt = new FISCALYEAR();
        ddlFiscalYear.DataSource = FYSer.GetAll(FYEnt);
        ddlFiscalYear.DataTextField = "FISCAL_YEAR";
        ddlFiscalYear.DataValueField = "FISCAL_YEAR";
        ddlFiscalYear.DataBind();
        ddlFiscalYear.SelectedValue = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
    }
    string acc_head;
    protected void LoadGLName()
    {
        GLEnt = new GL_ACCOUNT();
        GLEnt.GL_MASTER_CODE = ddlSundryType.SelectedValue;
        if (ddlSundryType.SelectedValue == "0103")
        {
            acc_head = "01";
            ddlGeneralLedgerCode.DataSource = GLSer.GetAll(GLEnt);
            ddlGeneralLedgerCode.DataTextField = "GL_NAME";
            ddlGeneralLedgerCode.DataValueField = "GL_CODE";
            ddlGeneralLedgerCode.DataBind();
            ddlGeneralLedgerCode.Items.Insert(0, "Select");
        }
        else
        {
            acc_head = "04";
            ddlGeneralLedgerCode.DataSource = GLSer.GetAll(GLEnt);
            ddlGeneralLedgerCode.DataTextField = "GL_NAME";
            ddlGeneralLedgerCode.DataValueField = "GL_CODE";
            ddlGeneralLedgerCode.DataBind();
            ddlGeneralLedgerCode.Items.Insert(0, "Select");
        }

    }

    protected void ddlSundryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGLName();
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void loadGrid()
    {
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
            office_code = ddlBranch.SelectedValue;
       
        gridOpeningbalance.DataSource = af.getNewBalance( ddlGeneralLedgerCode.SelectedValue, ddlFiscalYear.SelectedValue,office_code);
        gridOpeningbalance.DataBind();
        if (gridOpeningbalance.Rows.Count > 0)
        {
            divToPrint.Visible = true;
        }
    }
    double drTotal = 0;
    double crTotal = 0;
    double balance = 0;
    protected void gridOpeningbalance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDrAmount = e.Row.FindControl("lblDrAmount") as Label;
            Label lblCrAmount = e.Row.FindControl("lblCrAmount") as Label;
            Label lblClosingBalanceAmount = e.Row.FindControl("lblClosingBalanceAmount") as Label;
            Label lblOpneningBalance = e.Row.FindControl("lblOpneningBalance") as Label;


            if (lblDrAmount != null)
            {
                double drAmount = Convert.ToDouble(lblDrAmount.Text);
                lblDrAmount.Text = drAmount.ToString("#0.00");
                drTotal += drAmount;
            }

            if (lblCrAmount != null)
            {
                double crAmount = Convert.ToDouble(lblCrAmount.Text);
                lblCrAmount.Text = crAmount.ToString("#0.00");
                crTotal += crAmount;
            }
            balance = drTotal - crTotal;
            //if (balance >= 0)
            //    lblClosingBalanceAmount.Text = balance.ToString("#0.00");
            //else
            //    lblClosingBalanceAmount.Text = "(" + (balance * -1).ToString("#0.00") + ")";
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblDrTotal = e.Row.FindControl("lblDrTotal") as Label;
            Label lblCrTotal = e.Row.FindControl("lblCrTotal") as Label;
            Label lblTotalBalance = e.Row.FindControl("lblTotalBalance") as Label;

            if (lblDrTotal != null)
            {
                lblDrTotal.Text = drTotal.ToString("#0.00");
            }

            if (lblCrTotal != null)
            {
                lblCrTotal.Text = crTotal.ToString("#0.00");
            }
            if (balance >= 0)
                lblTotalBalance.Text = balance.ToString("#0.00");
            else
                lblTotalBalance.Text = "(" + (balance * -1).ToString("#0.00") + ")";

        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        loadGrid();
        LoadCompany();
        btnExcel.Visible = true;
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

        // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control.
    }
    protected void btnExcel_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename=OPENING_BALANCE_SUNDRY_XLS" + "_" + PGD.GetTodayNepaliDate() + ".xls");
        //Response.ContentEncoding = Encoding.UTF8; 
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw); 
        divToPrint.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.End();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlGeneralLedgerCode.SelectedValue != "Select")
        {
            loadGrid();
            LoadCompany();
        }
    }
}