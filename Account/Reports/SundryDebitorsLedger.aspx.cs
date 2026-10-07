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

public partial class Account_Reports_SundryDebitorsLedger : System.Web.UI.Page
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

    VOUCHER_MASTER VMEnt = new VOUCHER_MASTER();
    VOUCHER_MASTERService VMSer = new VOUCHER_MASTERService();

    UserProfileEntity userProfileEnt = new UserProfileEntity();

    Boolean IsPageRefresh = false;

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
                    txtFromDate.Text = PGD.GetTodayNepaliDate();
                    txtToDate.Text = PGD.GetTodayNepaliDate();
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
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
            office_code = ddlBranch.SelectedValue;

        GLSAEnt = new GL_SUB_ACCOUNT();
        GLSAEnt.GL_CODE = ddlGLName.SelectedValue;
        GLSAEnt.OFFICE_CODE = office_code;
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

    protected void ddlGLAccMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGLName();
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

    protected void btnView_Click(object sender, EventArgs e)
    {
        if (ddlGLName.SelectedValue != "Select" && ddlGLName.SelectedIndex > 0)
        {
            divToPrint.Visible = true;
            lblDate.Text = PGD.GetTodayDate("dd/mm/yyyy");

            if (ddlsubLedger.SelectedValue != "Select" && ddlsubLedger.SelectedIndex > 0)
            {
                lblLedgerof.Text = "Ledger of " + ddlsubLedger.SelectedItem.ToString() + " Account";
            }
            else
            {
                lblLedgerof.Text = "Ledger of " + ddlGLName.SelectedItem.ToString() + " Account";
            }
            LoadAccountLedger();
            divAccountLedger.Visible = true;
            divVoucher.Visible = false;

        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Enter General Ledger Name ");

        }

    }
    protected void LoadAccountLedger()
    {
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
            office_code = ddlBranch.SelectedValue;

        try
        {
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

            string[] fromdate = txtFromDate.Text.Split('/');
            string[] todate = txtToDate.Text.Split('/');
            string from_date = PGD.ConvertNepaliTOEnglish(fromdate[0], fromdate[1], fromdate[2]);
            string to_date = PGD.ConvertNepaliTOEnglish(todate[0], todate[1], todate[2]);
            gridAccountLedger.DataSource = af.getAccountLedger(ddlGLName.SelectedValue, ddlsubLedger.SelectedValue, from_date, to_date, ddlFiscalYear.SelectedValue, office_code);
            gridAccountLedger.DataBind();
        }
        catch
        {

        }
    }


    double drTotal = 0;
    double crTotal = 0;
    double balance = 0;

    protected void gridAccountLedger_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDrAmount = e.Row.FindControl("lblDrAmount") as Label;
            Label lblCrAmount = e.Row.FindControl("lblCrAmount") as Label;
            Label lblBalanceAmount = e.Row.FindControl("lblBalanceAmount") as Label;
            LinkButton lblVoucherNum = e.Row.FindControl("lblVoucherNum") as LinkButton;
            Label lblVoucherType = e.Row.FindControl("lblVoucherType") as Label;
            Label lblDate = e.Row.FindControl("lblDate") as Label;
            Label lblBill = e.Row.FindControl("lblBill") as Label;
            string[] date = lblDate.Text.Split('/');

            try
            {
                VMEnt = new VOUCHER_MASTER();
                VMEnt.VOUCHER_TYPE = lblVoucherType.Text;
                VMEnt.VOUCHER_NUMBER = lblVoucherNum.Text;
                VMEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                VMEnt.VOUCHER_FY = PGD.checkFiscalYear(date[1], date[2]);
                VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
                if (VMEnt != null)
                {

                    lblBill.Text = VMEnt.REF_ID;
                }
            }
            catch { }


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
            if (balance >= 0)
                lblBalanceAmount.Text = balance.ToString("#0.00");
            else
                lblBalanceAmount.Text = "(" + (balance * -1).ToString("#0.00") + ")";
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
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSubGLName();
    }
    protected void lblVoucherNum_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((LinkButton)sender).Parent.Parent as GridViewRow;
        LinkButton lblVoucherNum = gr.FindControl("lblVoucherNum") as LinkButton;
        Label lblVoucherType = gr.FindControl("lblVoucherType") as Label;
        Label lblDate = gr.FindControl("lblDate") as Label;
        string[] date = lblDate.Text.Split('/');

        VMEnt = new VOUCHER_MASTER();
        VMEnt.VOUCHER_TYPE = lblVoucherType.Text;
        VMEnt.VOUCHER_NUMBER = lblVoucherNum.Text;
        VMEnt.OFFICE_CODE = ddlBranch.SelectedValue;
        VMEnt.VOUCHER_FY = PGD.checkFiscalYear(date[1], date[2]);
        VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
        if (VMEnt != null)
        {

            LoadVoucher(VMEnt.PK_ID);
            divVoucher.Visible = true;
            divToPrint.Visible = false;
        }



    }
    protected void LoadVoucher(string Vou_Pkid)
    {
        VMEnt = new VOUCHER_MASTER();
        VMEnt.PK_ID = Vou_Pkid;
        VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
        if (VMEnt != null)
        {
            lblDates.Text = VMEnt.VOUCHER_DAY + "/" + VMEnt.VOUCHER_MONTH + "/" + VMEnt.VOUCHER_YEAR;
            lblVoucherNo.Text = VMEnt.VOUCHER_NUMBER;
            #region voucher type
            if (VMEnt.VOUCHER_TYPE == "JV")
            {
                lblVoucherType.Text = "Journal Voucher";
            }
            else if (VMEnt.VOUCHER_TYPE == "CV")
            {
                lblVoucherType.Text = "Credit Voucher";
            }
            else if (VMEnt.VOUCHER_TYPE == "DV")
            {
                lblVoucherType.Text = "Debit Voucher";
            }
            else if (VMEnt.VOUCHER_TYPE == "DN")
            {
                lblVoucherType.Text = "Debit Note";
            }
            else if (VMEnt.VOUCHER_TYPE == "CN")
            {
                lblVoucherType.Text = "Credit Note";
            }
            #endregion
            lblNarration.Text = VMEnt.NARRATION;

            lblPreparedBy.Text = hf.getEmployeeName(VMEnt.PREPARE_BY);


            if (VMEnt.STATUS == "1")
            {
                lblCheckedBy.Text = hf.getEmployeeName(VMEnt.CHECKED_BY);
            }
            if (VMEnt.STATUS == "2")
            {
                lblApprovedBy.Text = hf.getEmployeeName(VMEnt.APPROVED_BY);
            }

        }
        LoadCompanyS();
        grdVoucherChild.DataSource = af.getVoucherDetail(Vou_Pkid, "N"); // fetch voucher detail in normal order
        grdVoucherChild.DataBind();
    }

    protected void LoadCompanyS()
    {
        lblCompanyNameS.Text = PG.CompanyName();
        lblAddressS.Text = PG.CompanyAddress();
        lblContactS.Text = PG.CompanyContact();

    }
    protected void grdVoucherChild_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDrAmount = e.Row.FindControl("lblDrAmount") as Label;
            Label lblCrAmount = e.Row.FindControl("lblCrAmount") as Label;

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
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblDrTotal = e.Row.FindControl("lblDrTotal") as Label;
            Label lblCrTotal = e.Row.FindControl("lblCrTotal") as Label;

            if (lblDrTotal != null)
            {
                lblDrTotal.Text = drTotal.ToString("#0.00");
            }

            if (lblCrTotal != null)
            {
                lblCrTotal.Text = crTotal.ToString("#0.00");
            }
        }
        lblAmountsInword.Text = hf.NumWordsWrapper(crTotal).ToUpper() + " ONLY";
    }

    protected void btn_back_Click(object sender, EventArgs e)
    {
        divVoucher.Visible = false;
        divToPrint.Visible = true;
    }

    protected void lblParticulars_Click(object sender, EventArgs e)
    {
        string url;
        GridViewRow gr = ((LinkButton)sender).Parent.Parent as GridViewRow;

       
        Label lblBill = gr.FindControl("lblBill") as Label;
        Label lblVoucherType = gr.FindControl("lblVoucherType") as Label;
        if (lblVoucherType.Text == "CN")
        {
             url = "~/Reports/Return/ShowSalesReturn.aspx?invno=" + lblBill.Text;
        }
        else
        {
            url = "~/utilities/Sales/ShowInvoiceToPrint.aspx?invno=" + lblBill.Text;
        }
       
        string fullUrl = ResolveUrl(url);

        // Register JavaScript to open the URL in a new tab
        string script = "window.open('" + fullUrl + "', '_blank');";
        ScriptManager.RegisterStartupScript(this, GetType(), "OpenInvoice", script, true);
    }
}