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

public partial class Account_Reports_SundryCreditorsBalance : System.Web.UI.Page
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

    VOUCHER_MASTER VMEnt = new VOUCHER_MASTER();
    VOUCHER_MASTERService VMSer = new VOUCHER_MASTERService();

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
            lblAddress.Text = PG.BranchAddress(ddlBranch.SelectedValue);
            lblContact.Text = PG.BranchContact(ddlBranch.SelectedValue);
        }
        else
        {
            lblAddress.Text = "";
            lblContact.Text = "";

        }
        lblCompanyNameledger.Text = PG.CompanyName();
        if (ddlBranch.SelectedValue != "")
        {
            lblAddressledger.Text = PG.BranchAddress(ddlBranch.SelectedValue);
            lblContactledger.Text = PG.BranchContact(ddlBranch.SelectedValue);
        }
        else
        {
            lblAddressledger.Text = "";
            lblContactledger.Text = "";
        }
        lblCompanyNameS.Text = PG.CompanyName();
        if (ddlBranch.SelectedValue != "")
        {
            lblAddressS.Text = PG.BranchAddress(ddlBranch.SelectedValue);
            lblContactS.Text = PG.BranchContact(ddlBranch.SelectedValue);
        }
        else
        {
            lblAddressS.Text = "";
            lblContactS.Text = "";

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
        ddlGLName.DataSource = af.GET_GL_ACC_FROM_ACC_HEAD("04", "");
        ddlGLName.DataTextField = "GL_NAME";
        ddlGLName.DataValueField = "GL_CODE";
        ddlGLName.DataBind();
        ddlGLName.Items.Insert(0, "Select");
    }



    protected void ddlGLAccMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGLName();
    }



    protected void btnView_Click(object sender, EventArgs e)
    {
        if (ddlGLName.SelectedValue != "")
        {
            divToPrint.Visible = true;
            lblDate.Text = PGD.GetTodayDate("dd/mm/yyyy");
            LoadAccountLedger();
            divAccountLedger.Visible = true;
            LoadCompany();
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Enter General Ledger Name. ");
        }

    }
    protected void LoadAccountLedger()
    {
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
            office_code = ddlBranch.SelectedValue;
        try
        {
            string acc_head = ddlGLName.SelectedValue.Substring(0, 2);
            gridAccountLedger.DataSource = af.getBalance(acc_head, ddlGLName.SelectedValue, null, ddlFiscalYear.SelectedValue, office_code);
            gridAccountLedger.DataBind();
        }
        catch
        {

        }
    }



    double balance = 0;
    double total_balance = 0;
    protected void gridAccountLedger_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblBalance = e.Row.FindControl("lblBalance") as Label;
            Label lblPartyBalance = e.Row.FindControl("lblPartyBalance") as Label;
            balance = Convert.ToDouble(lblBalance.Text);
            if (balance >= 0)
                lblPartyBalance.Text = balance.ToString("##,##0.00");
            else
                lblPartyBalance.Text = "(" + (balance * -1).ToString("##,##0.00") + ")";
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblTotalBalance = e.Row.FindControl("lblTotalBalance") as Label;
            foreach (GridViewRow gr in gridAccountLedger.Rows)
            {
                Label lblBalance = gr.FindControl("lblBalance") as Label;
                balance = Convert.ToDouble(lblBalance.Text);
                total_balance += balance;
            }
            lblTotalBalance.Text = total_balance.ToString("##,##0.00");
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }
    double drTotal = 0;
    double crTotal = 0;

    protected void gridSubAccountLedger_RowDataBound(object sender, GridViewRowEventArgs e)
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
            divLedger.Visible = false;
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
        divLedger.Visible = true;
        divToPrint.Visible = false;
    }

    protected void lblParticulars_Click(object sender, EventArgs e)
    {
        string url;
        GridViewRow gr = ((LinkButton)sender).Parent.Parent as GridViewRow;
        if (gr.RowIndex != 0)
        {

            Label lblBill = gr.FindControl("lblBill") as Label;
            Label lblVoucherType = gr.FindControl("lblVoucherType") as Label;
            if (lblVoucherType.Text == "DN")
            {
                url = "~/Reports/Return/ShowPurchaseReturn.aspx?invno=" + lblBill.Text;
            }
            else
            {
                url = "~/reports/purchase/ShowPurchaseInvoice.aspx?dakhno=" + lblBill.Text;
            }

            string fullUrl = ResolveUrl(url);

            // Register JavaScript to open the URL in a new tab
            string script = "window.open('" + fullUrl + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenInvoice", script, true);
        }
    }

    protected void lblName_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((LinkButton)sender).Parent.Parent as GridViewRow;
        Label lblCode = gr.FindControl("lblCode") as Label;
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
            office_code = ddlBranch.SelectedValue;

        try
        {
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];


            gridSubAccountLedger.DataSource = af.getAccountLedger(ddlGLName.SelectedValue, lblCode.Text, PGD.getFiscalYearStartDateEng(ddlFiscalYear.SelectedValue),
                PGD.GetTodayDate("dd/mm/yyyy"), ddlFiscalYear.SelectedValue, office_code);
            gridSubAccountLedger.DataBind();
            if (gridSubAccountLedger.Rows.Count < 1)
            {
                divVoucher.Visible = false;
                divLedger.Visible = false;
                divToPrint.Visible = true;

            }
            else
            {
                divVoucher.Visible = false;
                divLedger.Visible = true;
                divToPrint.Visible = false;

            };
        }
        catch (Exception es)
        {
            e.ToString();
        }
    }
}