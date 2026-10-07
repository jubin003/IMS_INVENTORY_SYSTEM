using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Data;
using PhyeGanCore;

public partial class Account_MasterData_opening_balance : System.Web.UI.Page
{
    GL_ACCOUNT GLEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLSer = new GL_ACCOUNTService();

    GL_SUB_ACCOUNT GSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GSASer = new GL_SUB_ACCOUNTService();

    GL_ACCOUNT_MASTER GMEnt = new GL_ACCOUNT_MASTER();
    GL_ACCOUNT_MASTERService GMSer = new GL_ACCOUNT_MASTERService();

    LEDGER_BALANCE LBEnt = new LEDGER_BALANCE();
    LEDGER_BALANCEService LBSer = new LEDGER_BALANCEService();

    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

    OFFICE OEnt = new OFFICE();
    OFFICEService OSer = new OFFICEService();

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
                    LoadGLAccountHead();
                    LoadFiscalYear();
                    getBranchFY();
                    checkFY();
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

    protected void getBranchFY()
    {
        if (ddlBranch.SelectedValue == "")
        {
            lblBranchFY.Text = "";
        }
        else
        {
            OEnt = new OFFICE();
            OEnt.PK_ID = ddlBranch.SelectedValue;
            OEnt = (OFFICE)OSer.GetSingle(OEnt);
            if (OEnt != null)
            {
                lblBranchFY.Text = OEnt.OPENING_FISCAL_YEAR;
            }
        }

    }
    protected void checkFY()
    {
        if (PG.CompanyBranch_Status())
        {
            if (lblBranchFY.Text != "")
            {
                if (lblBranchFY.Text == ddlFiscalYear.SelectedValue)
                {
                    formTbl.Visible = true;
                    grdTbl.Visible = true;
                    tblAlert.Visible = false;
                }
                else
                {
                    formTbl.Visible = false;
                    grdTbl.Visible = false;
                    tblAlert.Visible = true;
                }
            }
        }
        else
        {
            formTbl.Visible = true;
            grdTbl.Visible = true;
            tblAlert.Visible = false;
        }

    }
    protected void LoadGLAccountHead()
    {
        ddlAccountsHead.DataSource = af.getAccountHead();
        ddlAccountsHead.DataValueField = "ACC_HEAD_CODE";
        ddlAccountsHead.DataTextField = "ACCOUNTS_HEAD";
        ddlAccountsHead.DataBind();
        ddlAccountsHead.Items.Insert(0, "Select");
    }
    protected void LoadGLAccountMaster()
    {
        GMEnt = new GL_ACCOUNT_MASTER();

        GMEnt.ACC_HEAD_CODE = ddlAccountsHead.SelectedValue;
        ddlGLAccMaster.DataSource = GMSer.GetAll(GMEnt);
        ddlGLAccMaster.DataTextField = "GL_MASTER_NAME";
        ddlGLAccMaster.DataValueField = "GL_MASTER_CODE";
        ddlGLAccMaster.DataBind();
        ddlGLAccMaster.Items.Insert(0, "Select");
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
    protected void ddlAccountsHead_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGLAccountMaster();
    }

    protected void ddlGLAccMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGl();
    }
    protected void LoadGl()
    {
        GLEnt = new GL_ACCOUNT();
        GLEnt.GL_MASTER_CODE = ddlGLAccMaster.SelectedValue;
        //GLEnt.EDITABLE = "1";
        ddlGl.DataSource = GLSer.GetAll(GLEnt);
        ddlGl.DataTextField = "GL_NAME";
        ddlGl.DataValueField = "GL_CODE";
        ddlGl.DataBind();
        ddlGl.Items.Insert(0, "Select");
    }
    protected void ddlGl_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadGrid();

    }

    protected void loadGrid()
    {
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
        {
            office_code = ddlBranch.SelectedValue;
        }

        GSAEnt = new GL_SUB_ACCOUNT();
        GSAEnt.GL_CODE = ddlGl.SelectedValue;
        GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
        if (GSAEnt != null)
        {
            gridOpeningBalance.DataSource = af.getSub_GL_LB(ddlGl.SelectedValue, ddlFiscalYear.SelectedValue, ddlBranch.SelectedValue);
            gridOpeningBalance.DataBind();
            gridGlOpeningBalance.Visible = false;
            gridOpeningBalance.Visible = true;
        }
        else
        {
            gridGlOpeningBalance.DataSource = af.getGL_LB(ddlGl.SelectedValue, ddlFiscalYear.SelectedValue, ddlBranch.SelectedValue);
            gridGlOpeningBalance.DataBind();
            gridOpeningBalance.Visible = false;
            gridGlOpeningBalance.Visible = true;
        }

    }

    protected void gridOpeningBalance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
        {
            office_code = ddlBranch.SelectedValue;
        }

        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblPK_ID = e.Row.FindControl("lblPK_ID") as Label;
            Label lblPK_ID_L = e.Row.FindControl("lblPK_ID_L") as Label;
            Label lblGlCode = e.Row.FindControl("lblGlCode") as Label;
            Label lblGLName = e.Row.FindControl("lblGLName") as Label;
            Label lblSubGlCode = e.Row.FindControl("lblSubGlCode") as Label;
            Label lblDrAmount = e.Row.FindControl("lblDrAmount") as Label;
            Label lblCrAmount = e.Row.FindControl("lblCrAmount") as Label;
            TextBox txtDrCrAmt = e.Row.FindControl("txtDrCrAmt") as TextBox;
            DropDownList ddlDrCr = e.Row.FindControl("ddlDrCr") as DropDownList;

            GLEnt = new GL_ACCOUNT();
            GLEnt.GL_CODE = lblGlCode.Text;
            GLEnt = (GL_ACCOUNT)GLSer.GetSingle(GLEnt);
            if (GLEnt != null)
            {
                lblGLName.Text = GLEnt.GL_NAME;
            }

            LBEnt = new LEDGER_BALANCE();
            LBEnt.GL_CODE = lblGlCode.Text;
            LBEnt.SGL_CODE = lblSubGlCode.Text;
            LBEnt.OFFICE_CODE = office_code;
            LBEnt.LEDGER_FY = ddlFiscalYear.SelectedValue;
            LBEnt = (LEDGER_BALANCE)LBSer.GetSingle(LBEnt);
            if (LBEnt != null)
            {
                lblPK_ID_L.Text = LBEnt.PK_ID;
                if (LBEnt.DR_AMOUNT != "0")
                {
                    ddlDrCr.SelectedValue = "debit";
                    txtDrCrAmt.Text = LBEnt.DR_AMOUNT;
                }
                else if (LBEnt.CR_AMOUNT != "0")
                {
                    ddlDrCr.SelectedValue = "credit";
                    txtDrCrAmt.Text = LBEnt.CR_AMOUNT;
                }
            }
        }

    }

    protected void gridOpeningBalance_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SaveRow")
        {
            string currentFY = ddlFiscalYear.SelectedValue;
            string previousFY = PGD.getPrevious_FiscalYear(currentFY);

            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
            Label lblPK_ID_L = gr.FindControl("lblPK_ID_L") as Label;
            Label lblSubGlCode = gr.FindControl("lblSubGlCode") as Label;
            TextBox txtDrCrAmt = gr.FindControl("txtDrCrAmt") as TextBox;
            DropDownList ddlDrCr = gr.FindControl("ddlDrCr") as DropDownList;


            #region for current year
            LBEnt = new LEDGER_BALANCE();
            LBEnt.SGL_CODE = lblSubGlCode.Text;
            LBEnt.GL_CODE = ddlGl.SelectedValue;
            LBEnt.LEDGER_FY = currentFY;
            LBEnt.OFFICE_CODE = ddlBranch.SelectedValue;
            LBEnt = (LEDGER_BALANCE)LBSer.GetSingle(LBEnt);
            if (LBEnt != null)
            {
                #region update for current fiscal year                   
                if (ddlDrCr.SelectedValue == "debit")
                {
                    LBEnt.DR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.CR_AMOUNT = "0";
                }
                else
                {
                    LBEnt.CR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.DR_AMOUNT = "0";
                }
                LBSer.Update(LBEnt);
                #endregion

            }
            else
            {
                LBEnt = new LEDGER_BALANCE();
                LBEnt.GL_CODE = ddlGl.SelectedValue;
                LBEnt.LEDGER_FY = currentFY;
                LBEnt.SGL_CODE = lblSubGlCode.Text;
                LBEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                if (ddlDrCr.SelectedValue == "debit")
                {
                    LBEnt.DR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.CR_AMOUNT = "0";
                }
                else
                {
                    LBEnt.CR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.DR_AMOUNT = "0";
                }
                LBSer.Insert(LBEnt);
            }
            #endregion

            #region for previous year
            LBEnt = new LEDGER_BALANCE();
            LBEnt.SGL_CODE = lblSubGlCode.Text;
            LBEnt.GL_CODE = ddlGl.SelectedValue;
            LBEnt.LEDGER_FY = previousFY;
            LBEnt.OFFICE_CODE = ddlBranch.SelectedValue;
            LBEnt = (LEDGER_BALANCE)LBSer.GetSingle(LBEnt);
            if (LBEnt != null)
            {
                #region update for current fiscal year                   
                if (ddlDrCr.SelectedValue == "debit")
                {
                    LBEnt.DR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.CR_AMOUNT = "0";
                }
                else
                {
                    LBEnt.CR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.DR_AMOUNT = "0";
                }
                LBSer.Update(LBEnt);
                #endregion

            }
            else
            {
                LBEnt = new LEDGER_BALANCE();
                LBEnt.GL_CODE = ddlGl.SelectedValue;
                LBEnt.LEDGER_FY = previousFY;
                LBEnt.SGL_CODE = lblSubGlCode.Text;
                LBEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                if (ddlDrCr.SelectedValue == "debit")
                {
                    LBEnt.DR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.CR_AMOUNT = "0";
                }
                else
                {
                    LBEnt.CR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.DR_AMOUNT = "0";
                }
                LBSer.Insert(LBEnt);
            }
            #endregion
            HelperFunction.MsgBox(this, this.GetType(), "Saved");
        }
    }

    protected void gridGlOpeningBalance_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "SaveRow")
        {
            string currentFY = ddlFiscalYear.SelectedValue;
            string previousFY = PGD.getPrevious_FiscalYear(currentFY);
            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
            TextBox txtDrCrAmt = gr.FindControl("txtDrCrAmt") as TextBox;
            DropDownList ddlDrCr = gr.FindControl("ddlDrCr") as DropDownList;

            LBEnt = new LEDGER_BALANCE();

            if (lblPK_ID.Text == "")
            {
                LBEnt = new LEDGER_BALANCE();
                LBEnt.GL_CODE = ddlGl.SelectedValue;
                LBEnt.LEDGER_FY = ddlFiscalYear.SelectedValue;
                LBEnt.SGL_CODE = "";

                if (ddlDrCr.SelectedValue == "debit")
                {
                    LBEnt.DR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.CR_AMOUNT = "0";
                }
                else
                {
                    LBEnt.CR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.DR_AMOUNT = "0";
                }

                LBSer.Insert(LBEnt);
                HelperFunction.MsgBox(this, this.GetType(), "Saved");
            }
            else
            {
                LBEnt = new LEDGER_BALANCE();
                LBEnt.PK_ID = lblPK_ID.Text;
                LBEnt = (LEDGER_BALANCE)LBSer.GetSingle(LBEnt);

                if (LBEnt != null)
                {
                    LBEnt.GL_CODE = ddlGl.SelectedValue;
                    LBEnt.LEDGER_FY = ddlFiscalYear.SelectedValue;
                    LBEnt.SGL_CODE = "";

                    if (ddlDrCr.SelectedValue == "debit")
                    {
                        LBEnt.DR_AMOUNT = txtDrCrAmt.Text;
                        LBEnt.CR_AMOUNT = "0";
                    }
                    else
                    {
                        LBEnt.CR_AMOUNT = txtDrCrAmt.Text;
                        LBEnt.DR_AMOUNT = "0";
                    }

                    LBSer.Update(LBEnt);
                    HelperFunction.MsgBox(this, this.GetType(), "Updated");
                }
            }

            #region for previous year
            LBEnt = new LEDGER_BALANCE();
            LBEnt.SGL_CODE = "";
            LBEnt.GL_CODE = ddlGl.SelectedValue;
            LBEnt.LEDGER_FY = previousFY;
            LBEnt.OFFICE_CODE = ddlBranch.SelectedValue;
            LBEnt = (LEDGER_BALANCE)LBSer.GetSingle(LBEnt);
            if (LBEnt != null)
            {
                #region update for current fiscal year                   
                if (ddlDrCr.SelectedValue == "debit")
                {
                    LBEnt.DR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.CR_AMOUNT = "0";
                }
                else
                {
                    LBEnt.CR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.DR_AMOUNT = "0";
                }
                LBSer.Update(LBEnt);
                #endregion

            }
            else
            {
                LBEnt = new LEDGER_BALANCE();
                LBEnt.GL_CODE = ddlGl.SelectedValue;
                LBEnt.LEDGER_FY = previousFY;
                LBEnt.SGL_CODE ="";
                LBEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                if (ddlDrCr.SelectedValue == "debit")
                {
                    LBEnt.DR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.CR_AMOUNT = "0";
                }
                else
                {
                    LBEnt.CR_AMOUNT = txtDrCrAmt.Text;
                    LBEnt.DR_AMOUNT = "0";
                }
                LBSer.Insert(LBEnt);
            }
            #endregion
        }
    }
    protected void gridGlOpeningBalance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblDrAmount = e.Row.FindControl("lblDrAmount") as Label;
            Label lblCrAmount = e.Row.FindControl("lblCrAmount") as Label;
            TextBox txtDrCrAmt = e.Row.FindControl("txtDrCrAmt") as TextBox;
            DropDownList ddlDrCr = e.Row.FindControl("ddlDrCr") as DropDownList;

            if (lblDrAmount.Text != "0")
            {
                txtDrCrAmt.Text = lblDrAmount.Text;
                ddlDrCr.SelectedValue = "debit";
            }
            else
            {
                txtDrCrAmt.Text = lblCrAmount.Text;
                ddlDrCr.SelectedValue = "credit";
            }
        }
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        getBranchFY();
        checkFY();
        if (ddlGl.SelectedValue != "")
        {
            loadGrid();

        }
    }
}