using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;
using System.IO;
using System.Data;
using DataHelper.Framework;
using Entity.Framework;

public partial class Account_Utilities_Payment : System.Web.UI.Page
{
    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();
    ACCOUNT_SETTING ASEnt = new ACCOUNT_SETTING();
    ACCOUNT_SETTINGService ASSer = new ACCOUNT_SETTINGService();
    VOUCHER_MASTER VMEnt = new VOUCHER_MASTER();
    VOUCHER_MASTERService VMSer = new VOUCHER_MASTERService();
    VOUCHER_CHILD VCEnt = new VOUCHER_CHILD();
    VOUCHER_CHILDService VCSer = new VOUCHER_CHILDService();
    VOUCHER_CHILD_REFERENCE VCREnt = new VOUCHER_CHILD_REFERENCE();
    VOUCHER_CHILD_REFERENCEService VCRSer = new VOUCHER_CHILD_REFERENCEService();
    VOUCHER_ALTER_REQUEST VAREnt = new VOUCHER_ALTER_REQUEST();
    VOUCHER_ALTER_REQUESTService VARSer = new VOUCHER_ALTER_REQUESTService();
    GL_ACCOUNT_MASTER GLAMEnt = new GL_ACCOUNT_MASTER();
    GL_ACCOUNT_MASTERService GLAMSer = new GL_ACCOUNT_MASTERService();
    GL_ACCOUNT GLAEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLASer = new GL_ACCOUNTService();
    GL_SUB_ACCOUNT GLSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GLSASer = new GL_SUB_ACCOUNTService();
    PAYMENT_TYPE PTEnt = new PAYMENT_TYPE();
    PAYMENT_TYPEService PTSer = new PAYMENT_TYPEService();

    UserProfileEntity userProfileEnt = new UserProfileEntity();

    EMPLOYEES EEnt = new EMPLOYEES();
    EMPLOYEESService ESer = new EMPLOYEESService();

    HelperFunction hf = new HelperFunction();
    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    AccountFunction af = new AccountFunction();
    Boolean flag = false;

    Boolean IsPageRefresh = false;
    static Boolean VoucherCreateAccess = false;
    static Boolean VoucherCheckAccess = false;
    static Boolean VoucherApproveAccess = false;
    double drTotal = 0;
    double crTotal = 0;
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
                    VoucherCreateAccess = PG.VoucherCreateAccess(path, userProfileEnt.UserGroupID.ToString());
                    VoucherCheckAccess = PG.VoucherCheckAccess(path, userProfileEnt.UserGroupID.ToString());
                    VoucherApproveAccess = PG.VoucherApproveAccess(path, userProfileEnt.UserGroupID.ToString());
                    LoadFiscalYear();
                    LoadVoucherStatus();
                    if (!VoucherCreateAccess)
                    {
                        btnAdd.Visible = false;
                    }
                    LoadCompany();
                    txtFromDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
                    txtToDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
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
    protected void LoadCompany()
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        lblCompanyName.Text = PG.CompanyName();
        lblAddress.Text = PG.BranchAddress(userProfileEnt.LocationID);
        lblContact.Text = PG.BranchContact(userProfileEnt.LocationID);
    }
    protected void LoadFiscalYear()
    {
        FYEnt = new FISCALYEAR();
        ddlFiscalYear.DataSource = FYSer.GetAll(FYEnt);
        ddlFiscalYear.DataTextField = "FISCAL_YEAR";
        ddlFiscalYear.DataValueField = "FISCAL_YEAR";
        ddlFiscalYear.DataBind();
    }
    protected void LoadVoucherStatus()
    {
        ACCOUNT_SETTING ASEnt = new ACCOUNT_SETTING();
        ASEnt.STATUS = "1";
        ddlStatus.DataSource = ASSer.GetAll(ASEnt);
        ddlStatus.DataTextField = "STATUS_NAME";
        ddlStatus.DataValueField = "VOUCHER_STATUS";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, "Select");
    }   

    #region to list the vouchers
    protected void btnList_Click(object sender, EventArgs e)
    {
        LoadList();
    }

    protected void LoadList()
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        btnAdd.Visible = true;
        divVoucherEntry.Visible = false;
        divBtns.Visible = false;
        divVoucheDate.Visible = false;
        grdVoucherList.Visible = true;
        divHide.Visible = false;
        divVoucherImage.Visible = false;
        string fromdate = "";
        string todate = "";
        try
        {
            string[] nfromdate = txtFromDate.Text.Split('/');
            string[] ntodate = txtToDate.Text.Split('/');
            fromdate = PGD.ConvertNepaliTOEnglish(nfromdate[0], nfromdate[1], nfromdate[2]);
            todate = PGD.ConvertNepaliTOEnglish(ntodate[0], ntodate[1], ntodate[2]);
        }
        catch { }

        VMEnt = new VOUCHER_MASTER();
        VMEnt.VOUCHER_FY = ddlFiscalYear.SelectedValue;
        if (ddlStatus.SelectedValue != "Select")
            VMEnt.STATUS = ddlStatus.SelectedValue;
        if (fromdate != "")
            VMEnt.FROM_DATE = fromdate;
        if (todate != "")
            VMEnt.TO_DATE = todate;
        VMEnt.VOUCHER_TYPE = "DV";
        VMEnt.OFFICE_CODE = userProfileEnt.LocationID;
        grdVoucherList.DataSource = VMSer.GetAll(VMEnt);
        grdVoucherList.DataBind();

        if (af.VoucherCheckProvision()) //if org have provision of checking voucher
            grdVoucherList.Columns[5].Visible = true;
        else
            grdVoucherList.Columns[5].Visible = false;
    }
    protected void grdVoucherList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblPkid = (Label)e.Row.FindControl("lblPkid");
            Label lblDate = (Label)e.Row.FindControl("lblDate");
            Label lblVoucheNo = (Label)e.Row.FindControl("lblVoucheNo");
            Label lblAmount = (Label)e.Row.FindControl("lblAmount");
            Label lblPreparedBy = (Label)e.Row.FindControl("lblPreparedBy");
            Label lblCheckedBy = (Label)e.Row.FindControl("lblCheckedBy");
            Label lblApprovedBy = (Label)e.Row.FindControl("lblApprovedBy");
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");

            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

            VMEnt = new VOUCHER_MASTER();
            VMEnt.PK_ID = lblPkid.Text;
            VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
            if (VMEnt != null)
            {
                lblDate.Text = VMEnt.VOUCHER_DAY + "/" + VMEnt.VOUCHER_MONTH + "/" + VMEnt.VOUCHER_YEAR;
                lblVoucheNo.Text = VMEnt.VOUCHER_NUMBER;
                lblAmount.Text = Convert.ToDouble(VMEnt.TRN_AMOUNT).ToString("0.00");
                lblPreparedBy.Text = hf.getEmployeeName(VMEnt.PREPARE_BY);
                lblCheckedBy.Text = hf.getEmployeeName(VMEnt.CHECKED_BY);
                lblApprovedBy.Text = hf.getEmployeeName(VMEnt.APPROVED_BY);

                #region to indicate voucher status

                if (VMEnt.STATUS == "-1")// voucher cancel status
                {
                    VAREnt = new VOUCHER_ALTER_REQUEST();
                    VAREnt.VOUCHER_PK_ID = VMEnt.PK_ID;
                    VAREnt.STATUS = "2"; // voucher cancel status
                    VAREnt = (VOUCHER_ALTER_REQUEST)VARSer.GetSingle(VAREnt);
                    if (VAREnt != null)
                    {
                        lblStatus.Text = "Canceled";
                    }

                }
                //org lai check by ko access cha bhaye status 0 huncha na bhaye 1 
                else if (VMEnt.STATUS == "0" || VMEnt.STATUS == "1")// voucher is created or alter requested                  
                {
                    if (af.VoucherCheckProvision() && VMEnt.STATUS == "0") // voucher check garni permission cha ra voucher ko status 0 bhaye matra status check garni
                    {
                        VAREnt = new VOUCHER_ALTER_REQUEST();
                        VAREnt.VOUCHER_PK_ID = VMEnt.PK_ID;
                        VAREnt.STATUS = "0"; // voucher alter request
                        VAREnt = (VOUCHER_ALTER_REQUEST)VARSer.GetSingle(VAREnt);
                        if (VAREnt != null)
                        {
                            lblStatus.Text = "Alter Requested";
                        }
                    }
                    if (!af.VoucherCheckProvision() && VMEnt.STATUS == "1") // voucher check garni permission chana ra voucher ko status 1 bhaye matra status check garni
                                                                            // false return aauni bhaye ko not rakhera true gare ko
                    {
                        VAREnt = new VOUCHER_ALTER_REQUEST();
                        VAREnt.VOUCHER_PK_ID = VMEnt.PK_ID;
                        VAREnt.STATUS = "0"; // voucher alter request
                        VAREnt = (VOUCHER_ALTER_REQUEST)VARSer.GetSingle(VAREnt);
                        if (VAREnt != null)
                        {
                            lblStatus.Text = "Alter Requested";
                        }
                    }
                }
                #endregion
            }
        }
    }

    protected void grdVoucherList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("View"))
        {
            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblpkid = gr.FindControl("lblpkid") as Label;

            lblPK_id.Text = lblpkid.Text;
            VMEnt = new VOUCHER_MASTER();
            VMEnt.PK_ID = lblPK_id.Text;
            VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
            if (VMEnt != null)
            {
                ShowVoucher(VMEnt.PK_ID);
            }
            LoadVoucher();
            grdVoucherList.Visible = false;
            divHide.Visible = true;
            divBtns.Visible = true;
            divVoucherImage.Visible = true;
            lblVoucherPK_ID.Text = VMEnt.PK_ID; //For update
            lblPk_idPDF.Text = VMEnt.PK_ID; // For opening pdf file
            LoadCompany();
        }
    }
    #endregion

    #region to print voucher
    protected void LoadVoucher()
    {
        VMEnt = new VOUCHER_MASTER();
        VMEnt.PK_ID = lblPK_id.Text;
        VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
        if (VMEnt != null)
        {
            lblDate.Text = VMEnt.VOUCHER_DAY + "/" + VMEnt.VOUCHER_MONTH + "/" + VMEnt.VOUCHER_YEAR;
            lblVoucherNo.Text = VMEnt.VOUCHER_NUMBER;

            lblVoucherType.Text = "Payment Voucher";
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

            VCREnt = new VOUCHER_CHILD_REFERENCE();
            VCREnt.VC_PK_ID = lblPK_id.Text;
            VCREnt = (VOUCHER_CHILD_REFERENCE)VCRSer.GetSingle(VCREnt);
            if (VCREnt != null)
            {
                lblDebitRefMode.Text = VCREnt.MOP;
                lblDebitRefNo.Text = VCREnt.REF_NUMBER;
                lblDebitRefDate.Text = VCREnt.REF_DAY + "/" + VCREnt.REF_MONTH + "/" + VCREnt.REF_YEAR;
                divPaymentMode.Visible = true;
            }
            else
            {
                divPaymentMode.Visible = false;
            }

            #region button visibility Check
            #region to show check by button           
            if (af.VoucherCheckProvision())
            {
                // yedi yo org lai check garni access cha bhaye check by ko div visible hunu ka sathai               
                divCheckedBy.Visible = true;
                // yedi yo voucher check bhaye ko chaina bhane check garni button visible garni ki na garni bhane ra check garni
                if (VMEnt.STATUS == "0") // yo vouche create matra bhayo
                {
                    if (VoucherCheckAccess)
                    {
                        //yedi login garni manche lai check garni authority cha bhaye button visible huncha
                        btnCheck.Visible = true;
                    }
                    else
                    {
                        //yedi login garni manche lai check garni authority chaina bhaye button visible hudaina
                        btnCheck.Visible = false;
                    }
                }
                else
                {
                    //yedi voucher check bhai sakyo bhane check ko button visible hudaina
                    btnCheck.Visible = false;

                }
            }
            else
            {
                // yedi yo org lai check garni access chana bhaye check by ko div visible hudaina    
                divCheckedBy.Visible = false;
            }
            #endregion
            #region to show approve by button
            if (VMEnt.STATUS == "1") // voucher ko status 1 bhane ko check bhai sakyo
            {
                // yo voucher check bhaisakyoo tara approved bhaye ko chaina
                // voucher approved garnalai login garni manche ko authority cha ki chaina check garcha 
                if (VoucherApproveAccess == true)
                {
                    //authority cha bhaye btn visible huncha
                    btnApprove.Visible = true;
                    btnCorrect.Visible = true;
                }
                else
                {
                    //authority chaina bhaye btn visible hudaina
                    btnApprove.Visible = false;
                    btnCorrect.Visible = false;
                }
            }
            else
            {
                //voucher approved bhai sakyo bhane button visible hudaina
                btnApprove.Visible = false;
                btnCorrect.Visible = false;
            }
            #endregion
            #region to show cancel button           
            if (af.VoucherCheckProvision())
            {
                // yedi yo org lai check garni access cha bhaye, prepare ko status 0 huncha              
                // yedi yo voucher ko status 0 cha bhaye matra cancel garna paucha
                // check bhai sakyo bhaye correct voucher gare ra status lai 0 banaunu parcha
                // ra login user lai voucher create access cha bhaya matra cancel garna paucha
                if (VMEnt.STATUS == "0" && VoucherCreateAccess == true)
                {
                    btnCancel.Visible = true;
                }
                else
                {
                    btnCancel.Visible = false;
                }
            }
            else
            {
                // yedi yo org lai check garni access chana bhaye prepare ko status 1 huncha including check
                // ra login user lai voucher create access cha bhaya matra cancel garna paucha
                if (VMEnt.STATUS == "1" && VoucherCreateAccess == true)
                {
                    btnCancel.Visible = true;
                }
                else
                {
                    btnCancel.Visible = false;
                }
            }

            #endregion

            #region to show alter button
            if (VMEnt.STATUS == "0" || VMEnt.STATUS == "1") // voucher  status 0 or 1  bhaye matra alter status check garni
            {
                VAREnt = new VOUCHER_ALTER_REQUEST();
                VAREnt.VOUCHER_PK_ID = VMEnt.PK_ID;
                VAREnt.STATUS = "0"; // voucher alter request
                VAREnt = (VOUCHER_ALTER_REQUEST)VARSer.GetSingle(VAREnt);
                if (VAREnt != null) // alter request bhaye khi garna dina bhaye na
                {
                    btnCheck.Visible = false;
                    btnApprove.Visible = false;
                    btnCorrect.Visible = false;

                    // if voucher alter garni request cha ra 
                    // voucher create garni manche ra login garni manche eutai ho bhaye matra alter ko button visible garni
                    userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                    if (VMEnt.PREPARE_BY == userProfileEnt.EmployeeID)
                    {
                        btnAlter.Visible = true;
                    }

                    divAlternationNote.Visible = true;
                    LabelAlterationNote.Text = "Alteration Note:";
                    LabelAlterationNoteRequest.Text = "Requested By:";
                    lblAlterationNote.Text = VAREnt.ALTER_DETAIL;
                    lblAlterationNoteRequest.Text = hf.getEmployeeName(VAREnt.ALTER_REQUEST_BY);
                }
                else
                {
                    divAlternationNote.Visible = false;
                    btnAlter.Visible = false;
                }
            }
            else
            {
                divAlternationNote.Visible = false;
                btnAlter.Visible = false;
            }
            #endregion
            #region for cancelation note
            if (VMEnt.STATUS == "-1") // voucher  cancel gare ko cha bhaye
            {
                VAREnt = new VOUCHER_ALTER_REQUEST();
                VAREnt.VOUCHER_PK_ID = VMEnt.PK_ID;
                VAREnt.STATUS = "2"; // voucher alter request
                VAREnt = (VOUCHER_ALTER_REQUEST)VARSer.GetSingle(VAREnt);
                if (VAREnt != null) // alter request bhaye khi garna dina bhaye na
                {
                    divAlternationNote.Visible = true;
                    LabelAlterationNote.Text = "Cancel Note:";
                    LabelAlterationNoteRequest.Text = "Canceled By:";
                    lblAlterationNote.Text = VAREnt.ALTER_DETAIL;
                    lblAlterationNoteRequest.Text = hf.getEmployeeName(VAREnt.ALTERED_BY);
                }
            }

            #endregion
            #endregion
        }
        grdVoucherChild.DataSource = af.getVoucherDetail(lblPK_id.Text, "N"); // fetch voucher detail in normal order
        grdVoucherChild.DataBind();

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

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    #endregion

    #region to create voucher
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        btnSaveVoucher.Visible = true;
        btnUpdate.Visible = false;
        divVoucherEntry.Visible = true;
        divHide.Visible = false;
        grdVoucherList.Visible = false;
        divVoucheDate.Visible = true;
        divVoucherImage.Visible = false;
        ClearVoucher();
        LaodBank();
        LoadModeOfPayment();
        LaodGLAccount();
    }
    protected void ClearVoucher()
    {
        txtVoucherDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
        lblBalance.Text = "";
        txtNarration.Text = "";
        txtRefAmount.Text = "";
        txtRefDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
        txtRefNo.Text = "";
        divShowBankDetail.Visible = false;
        divEntryBankDetail.Visible = false;
        divEntryCash.Visible = true;
        divDrEntry.Visible = true;
        grdDebit.DataSource = null;
        grdDebit.DataBind();
        ddlSGLAccount.Visible = false;

        lblBank.Text = "";
        lblSGL_CODE.Text = "";
        lblRefNo.Text = "";
        lblRefDate.Text = "";
        lblTotalDrAmount.Text = "";
        lblTotalCrAmount.Text = "";
    }
    protected void LaodBank()
    {
        GLSAEnt = new GL_SUB_ACCOUNT();
        GLSAEnt.GL_CODE = "010101"; // BANK GL CODE
        GLSAEnt.STATUS = "1";
        ddlBankName.DataSource = GLSASer.GetAll(GLSAEnt);
        ddlBankName.DataTextField = "SUB_GL_NAME";
        ddlBankName.DataValueField = "SUB_GL_CODE";
        ddlBankName.DataBind();
        ddlBankName.Items.Insert(0, "Select");
    }
    protected void LoadModeOfPayment()
    {
        PTEnt = new PAYMENT_TYPE();
        PTEnt.SALES_PURCHASE = "DV"; // Debit Voucher Status
        PTEnt.STATUS = "1";
        ddlPaymentMode.DataSource = PTSer.GetAll(PTEnt);
        ddlPaymentMode.DataTextField = "PAYMENT_NAME";
        ddlPaymentMode.DataValueField = "PAYMENT_NAME";
        ddlPaymentMode.DataBind();
    }
    protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPaymentMode.SelectedValue== "Cash" || ddlPaymentMode.SelectedValue == "Petty Cash")
        {
            divEntryBankDetail.Visible = false;
            divEntryCash.Visible = true;
            divDrEntry.Visible = true;
            divShowBankDetail.Visible = false;
        }
        else
        {
            divEntryBankDetail.Visible = true ;
            divEntryCash.Visible = false;
            divDrEntry.Visible = false;
        }
    }

    protected void ddlBankName_SelectedIndexChanged(object sender, EventArgs e)
    {
        string fiscalyear = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
        lblBalance.Text = af.getLedgerOpeningBalance("010101", ddlBankName.SelectedValue, fiscalyear, PGD.GetTodayDate("dd/mm/yyyy"));
    }

    protected void txtCashAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblTotalCrAmount.Text = Convert.ToDouble(txtCashAmount.Text).ToString("0.00");
            txtCashAmount.Text = lblTotalCrAmount.Text;
        }
        catch
        {
            txtCashAmount.Text = "";
            txtCashAmount.Focus();
            HelperFunction.MsgBox(this, this.GetType(), "Enter Number only.");
        }
    }
    protected void btnAddBank_Click(object sender, EventArgs e)
    {
        if (ddlBankName.SelectedValue != "Select")
        {
            try
            {
                lblBank.Text = ddlBankName.SelectedItem.ToString();
                lblSGL_CODE.Text = ddlBankName.SelectedValue;
                lblRefNo.Text = txtRefNo.Text;
                lblRefDate.Text = txtRefDate.Text;
                lblTotalCrAmount.Text = Convert.ToDouble(txtRefAmount.Text).ToString("0.00");
                divShowBankDetail.Visible = true;
                divEntryBankDetail.Visible = false;
                divDrEntry.Visible = true;
            }
            catch
            {
                HelperFunction.MsgBox(this, this.GetType(), "Bank Detail is not entered properly. ");
            }
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Bank Detail is not entered properly. ");
        }
    }
    protected void btnEditBank_Click(object sender, EventArgs e)
    {
        divShowBankDetail.Visible = false;
        divEntryBankDetail.Visible = true;
        ddlBankName.SelectedValue = lblSGL_CODE.Text;
        txtRefNo.Text = lblRefNo.Text;
        txtRefDate.Text = lblRefDate.Text;
        txtRefAmount.Text = lblTotalCrAmount.Text;
        lblTotalCrAmount.Text = "0";
    }
    protected void LaodGLAccount()
    {       
        ddlGLAccount.DataSource =  af.GET_GL_ACC_FROM_ACC_HEAD("03", "04");
        ddlGLAccount.DataTextField = "GL_NAME";
        ddlGLAccount.DataValueField = "GL_CODE";
        ddlGLAccount.DataBind();
        ddlGLAccount.Items.Insert(0, "Select");
    }

    protected DataTable CreateGrid(int RowIndex, bool addRemoveFlag)
    {
        int row = grdDebit.Rows.Count;

        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("GL_CODE");
        dummyTable.Columns.Add("GL_NAME");
        dummyTable.Columns.Add("SGL_CODE");
        dummyTable.Columns.Add("SGL_NAME");
        dummyTable.Columns.Add("AMOUNT");

        DataRow dummyRow = dummyTable.NewRow();
        if (row > 0)
        {
            foreach (GridViewRow r in grdDebit.Rows)
            {
                Label lblGLCode = grdDebit.Rows[r.RowIndex].FindControl("lblGLCode") as Label;
                Label lblGLName = grdDebit.Rows[r.RowIndex].FindControl("lblGLName") as Label;
                Label lblSGLCode = grdDebit.Rows[r.RowIndex].FindControl("lblSGLCode") as Label;
                Label lblSGLName = grdDebit.Rows[r.RowIndex].FindControl("lblSGLName") as Label;
                Label lblAmount = grdDebit.Rows[r.RowIndex].FindControl("lblAmount") as Label;

                DataRow dummyRw = dummyTable.NewRow();

                dummyRw["GL_CODE"] = lblGLCode.Text;
                dummyRw["GL_NAME"] = lblGLName.Text;
                dummyRw["SGL_CODE"] = lblSGLCode.Text;
                dummyRw["SGL_NAME"] = lblSGLName.Text;
                dummyRw["AMOUNT"] = lblAmount.Text;


                dummyTable.Rows.Add(dummyRw);
                if (r.RowIndex == RowIndex && addRemoveFlag == false)
                {
                    dummyTable.Rows.Remove(dummyRw);
                    flag = true;
                }
            }

            if (flag == false)
            {
                dummyRow["GL_CODE"] = txtGLCode.Text;
                dummyRow["GL_NAME"] = ddlGLAccount.SelectedItem.ToString();
                if (ddlSGLAccount.SelectedValue != "")
                {
                    dummyRow["SGL_CODE"] = ddlSGLAccount.SelectedValue;
                    dummyRow["SGL_NAME"] = ddlSGLAccount.SelectedItem.ToString();
                }
                dummyRow["AMOUNT"] = Convert.ToDouble(txtAmount.Text).ToString("0.00");
                dummyTable.Rows.Add(dummyRow);
            }
        }
        else
        {
            dummyRow["GL_CODE"] = txtGLCode.Text;
            dummyRow["GL_NAME"] = ddlGLAccount.SelectedItem.ToString();
            if (ddlSGLAccount.SelectedValue != "")
            {
                dummyRow["SGL_CODE"] = ddlSGLAccount.SelectedValue;
                dummyRow["SGL_NAME"] = ddlSGLAccount.SelectedItem.ToString();
            }
            dummyRow["AMOUNT"] = Convert.ToDouble(txtAmount.Text).ToString("0.00");
            dummyTable.Rows.Add(dummyRow);
        }
        int drow = dummyTable.Rows.Count;
        grdDebit.DataSource = dummyTable;
        grdDebit.DataBind();
        return dummyTable;
    }

    protected void btnAddDr_Click(object sender, EventArgs e)
    {
        try
        {
            double amt = Convert.ToDouble(txtAmount.Text);
            if (ddlGLAccount.SelectedValue != "Select" && amt != 0)
            {
                CreateGrid(1, true);
                txtGLCode.Text = "";
                LaodGLAccount();
                ddlSGLAccount.Visible = false;
                txtAmount.Text = "";
                divDebittPart.Visible = true;
            }
            else
            {
                HelperFunction.MsgBox(this, this.GetType(), "Select General Ledger.");
            }
        }
        catch (Exception kerror)
        {
            HelperFunction.MsgBox(this, this.GetType(), kerror.ToString());
        }
    }

    protected void ddlGLAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_CODE = ddlGLAccount.SelectedValue;
        GLAEnt = (GL_ACCOUNT)GLASer.GetSingle(GLAEnt);
        if (GLAEnt != null)
        {
            txtGLCode.Text = GLAEnt.GL_CODE;
            if (GLAEnt.SUB_LEDGER == "1")
            {
                ddlSGLAccount.Visible = true;
                GLSAEnt = new GL_SUB_ACCOUNT();

                GLSAEnt.GL_CODE = ddlGLAccount.SelectedValue;
                ddlSGLAccount.DataSource = GLSASer.GetAll(GLSAEnt);
                ddlSGLAccount.DataTextField = "SUB_GL_NAME";
                ddlSGLAccount.DataValueField = "SUB_GL_CODE";
                ddlSGLAccount.DataBind();
                ddlSGLAccount.Items.Insert(0, "Select");
            }
            else
            {
                ddlSGLAccount.DataSource = null;
                ddlSGLAccount.DataBind();
                ddlSGLAccount.Visible = false;
            }
        }
    }

    protected void grdDebit_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            if (grdDebit.Rows.Count > 0)
            {
                CreateGrid(gr.RowIndex, false);
            }
        }
    }

    protected void grdDebit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        #region Footer
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblDrTotal = (Label)e.Row.FindControl("lblDrTotal");
            Double sum = 0.0;

            foreach (GridViewRow gr in grdDebit.Rows)
            {
                if ((gr.RowState == DataControlRowState.Normal || gr.RowState == DataControlRowState.Alternate))
                {
                    Label lblAmount = (Label)gr.FindControl("lblAmount");
                    if (!string.IsNullOrEmpty(lblAmount.Text))
                        sum += Double.Parse(lblAmount.Text);
                }
            }
            lblDrTotal.Text = sum.ToString("0.00");
            lblTotalDrAmount.Text = sum.ToString("0.00");
        }
        #endregion
    }

    protected void txtGLCode_TextChanged(object sender, EventArgs e)
    {
        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_CODE = txtGLCode.Text;
        GLAEnt = (GL_ACCOUNT)GLASer.GetSingle(GLAEnt);
        if (GLAEnt != null)
        {
            txtGLCode.Text = GLAEnt.GL_CODE;
            if (GLAEnt.SUB_LEDGER == "1")
            {
                ddlSGLAccount.Visible = true;
                GLSAEnt = new GL_SUB_ACCOUNT();

                GLSAEnt.GL_CODE = ddlGLAccount.SelectedValue;
                ddlSGLAccount.DataSource = GLSASer.GetAll(GLSAEnt);
                ddlSGLAccount.DataTextField = "SUB_GL_NAME";
                ddlSGLAccount.DataValueField = "SUB_GL_CODE";
                ddlSGLAccount.DataBind();
            }
            else
            {
                ddlSGLAccount.DataSource = null;
                ddlSGLAccount.DataBind();
                ddlSGLAccount.Visible = false;
            }
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Invalid GL Code.");
        }
    }

    protected void btnSaveVoucher_Click(object sender, EventArgs e)
    {
        if (!IsPageRefresh) // to check if it is post back 
        {
            DistributedTransaction DT = new DistributedTransaction();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            if (lblTotalCrAmount.Text == lblTotalDrAmount.Text)
            {
                #region to insert in Voucher Master
                string[] nepalidate = txtVoucherDate.Text.Split('/');
                VMEnt = new VOUCHER_MASTER();
                VMEnt.VOUCHER_TYPE = "DV";
                VMEnt.VOUCHER_NUMBER = af.getNext_VM_ID("DV", PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), userProfileEnt.LocationID);
                VMEnt.VOUCHER_DATE = PGD.GetTodayDate("dd/mm/yyyy");

                VMEnt.VOUCHER_DAY = nepalidate[0];
                VMEnt.VOUCHER_MONTH = nepalidate[1];
                VMEnt.VOUCHER_YEAR = nepalidate[2];
                VMEnt.VOUCHER_FY = PGD.checkFiscalYear(nepalidate[1], nepalidate[2]);
                VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                VMEnt.TRN_AMOUNT = lblTotalDrAmount.Text;
                VMEnt.NARRATION = txtNarration.Text;
                VMEnt.REF_TABLE = "";
                VMEnt.REF_ID = "";
                VMEnt.OFFICE_CODE = userProfileEnt.LocationID;
                VMEnt.PREPARE_BY = userProfileEnt.EmployeeID;

                if (af.VoucherCheckProvision())   //  organization ko voucher check garni permission cha bhaya
                {
                    VMEnt.CHECKED_BY = "";
                    VMEnt.STATUS = "0";//status 0 is voucher created
                }
                else // yedi org ko voucher check garni permission chaina bhaye teyo voucher create garni bela ma automatically checked huncha
                {
                    VMEnt.CHECKED_BY = userProfileEnt.EmployeeID;
                    VMEnt.STATUS = "1";//status 1  voucher is created and checked
                }
                VMEnt.APPROVED_BY = "";
                string voucher_pk_id = VMSer.Insert(VMEnt, DT).ToString();
                #endregion
                #region to insert in to voucher child
                int sno = 1;
                #region for Dr Part
                foreach (GridViewRow gr in grdDebit.Rows)
                {
                    Label lblGLCode = gr.FindControl("lblGLCode") as Label;
                    Label lblSGLCode = gr.FindControl("lblSGLCode") as Label;
                    Label lblAmount = gr.FindControl("lblAmount") as Label;


                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = voucher_pk_id;
                    VCEnt.SNO = sno.ToString();
                    VCEnt.GL_CODE = lblGLCode.Text;
                    VCEnt.SGL_CODE = lblSGLCode.Text;
                    VCEnt.DR_AMOUNT = lblAmount.Text;
                    VCEnt.CR_AMOUNT = "0";
                    VCEnt.REMARKS = "By";
                    VCEnt.OFFICE_CODE = userProfileEnt.LocationID;
                    VCSer.Insert(VCEnt, DT);
                    sno++;
                }

                #endregion
                #region for Cr Part
                VCEnt = new VOUCHER_CHILD();
                VCEnt.VOUCHER_ID = voucher_pk_id;
                VCEnt.SNO = sno.ToString();
                if (ddlPaymentMode.SelectedValue == "Cash")
                {
                    VCEnt.GL_CODE = "010102";//bank GL Code
                    VCEnt.SGL_CODE = "";
                }
                else if (ddlPaymentMode.SelectedValue == "Petty Cash")
                {
                    VCEnt.GL_CODE = "010103";//bank GL Code
                    VCEnt.SGL_CODE = "";
                }
                else
                {
                    VCEnt.GL_CODE = "010101";//bank GL Code
                    VCEnt.SGL_CODE = lblSGL_CODE.Text;
                }
                VCEnt.DR_AMOUNT = "0";
                VCEnt.CR_AMOUNT = lblTotalCrAmount.Text;
                VCEnt.REMARKS = "To";
                VCEnt.OFFICE_CODE = userProfileEnt.LocationID;
                string VC_PK_ID = VCSer.Insert(VCEnt, DT).ToString(); ;
                #endregion

                #region for voucher child Reference
                VCREnt = new VOUCHER_CHILD_REFERENCE();
                VCREnt.VC_PK_ID = voucher_pk_id;
                VCREnt.MOP = ddlPaymentMode.SelectedValue;
                VCREnt.REF_NUMBER = txtRefNo.Text;
                string[] refDate = txtRefDate.Text.Split('/');
                VCREnt.REF_DAY = refDate[0];
                VCREnt.REF_MONTH = refDate[1];
                VCREnt.REF_YEAR = refDate[2];
                VCREnt.REF_FY = PGD.checkFiscalYear(refDate[1], refDate[2]);
                VCREnt.REF_DATE = PGD.ConvertNepaliTOEnglish(refDate[0], refDate[1], refDate[2]);
                VCREnt.AMOUNT = lblTotalCrAmount.Text;
                VCREnt.STATUS = "1";
                VCREnt.OFFICE_CODE = userProfileEnt.LocationID;
                VCRSer.Insert(VCREnt, DT);
                #endregion
                #endregion

                if (DT.HAPPY == true)
                {
                    DT.Commit();
                    lblPK_id.Text = voucher_pk_id;
                    LoadList();
                    divVoucheDate.Visible = false;
                    divEntryBankDetail.Visible = false;
                    divShowBankDetail.Visible = false;
                    divDrEntry.Visible = false;
                    divDebittPart.Visible = false;
                    uploadFile(voucher_pk_id);
                }
                else
                {
                    DT.Abort();
                    HelperFunction.MsgBox(this, this.GetType(), "Something goes wrong");
                }
                DT.Dispose();
            }
            else
            {
                HelperFunction.MsgBox(this, this.GetType(), "Debit and Credit Amount are not equal.");
            }
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        LoadList();
    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        VMEnt = new VOUCHER_MASTER();
        VMEnt.PK_ID = lblPK_id.Text;
        VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
        if (VMEnt != null)
        {
            VMEnt.CHECKED_BY = userProfileEnt.EmployeeID;
            VMEnt.STATUS = "1";
            VMSer.Update(VMEnt);
            LoadList();
        }
    }

    protected void btnCorrect_Click(object sender, EventArgs e)
    {
        divCorrectVoucher.Visible = true;
        divBtns.Visible = false;
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        VMEnt = new VOUCHER_MASTER();
        VMEnt.PK_ID = lblPK_id.Text;
        VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
        if (VMEnt != null)
        {
            VMEnt.APPROVED_BY = userProfileEnt.EmployeeID;
            VMEnt.STATUS = "2";
            VMEnt.APPROVED_DATE = PGD.GetTodayDate("dd/mm/yyyy");
            VMEnt.APPROVED_DAY = PGD.NepaliDay();
            VMEnt.APPROVED_MONTH = PGD.NepaliMonth();
            VMEnt.APPROVED_YEAR = PGD.NepaliYear();
            VMEnt.APPROVED_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
            VMSer.Update(VMEnt);
            LoadList();
        }
    }

    protected void btnCancelVoucher_Click(object sender, EventArgs e)
    {
        LoadList();

    }
    #endregion



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divVoucherCancel.Visible = true;
        divBtns.Visible = false;
    }

    protected void btnCorrectVoucher_Click(object sender, EventArgs e)
    {
        DistributedTransaction DT = new DistributedTransaction();
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        VMEnt = new VOUCHER_MASTER();
        VMEnt.PK_ID = lblPK_id.Text;
        VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
        if (VMEnt != null)
        {
            if (af.VoucherCheckProvision())
            {
                // yedi yo org lai check garni access cha bhaye           
                VMEnt.STATUS = "0";
            }
            else
            {
                // yedi yo org lai check garni access chaina bhaye           
                VMEnt.STATUS = "1";
            }
            VMSer.Update(VMEnt, DT);


            VAREnt = new VOUCHER_ALTER_REQUEST();
            VAREnt.VOUCHER_PK_ID = lblPK_id.Text;
            VAREnt.ALTER_REQUEST_BY = userProfileEnt.EmployeeID;
            VAREnt.ALTER_DETAIL = txtCorrectionNote.Text;
            VAREnt.ALTERED_DATE = PGD.GetTodayDate("dd/mm/yyyy");
            VAREnt.STATUS = "0";// status -1 is correction request; 1 is corrected; 2 is voucher cancel
            VAREnt.OFFICE_CODE = userProfileEnt.LocationID;
            VARSer.Insert(VAREnt, DT);

            if (DT.HAPPY == true)
            {
                DT.Commit();
                LoadList();
                divCorrectVoucher.Visible = false;
                divBtns.Visible = false;
                txtCorrectionNote.Text = "";
            }
            else
            {
                DT.Abort();
                HelperFunction.MsgBox(this, this.GetType(), "Something goes wrong. ");
            }
            DT.Dispose();
        }
    }

    protected void btnVoucherCancel_Click(object sender, EventArgs e)
    {
        DistributedTransaction DT = new DistributedTransaction();
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        VMEnt = new VOUCHER_MASTER();
        VMEnt.PK_ID = lblPK_id.Text;
        VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
        if (VMEnt != null)
        {
            VMEnt.STATUS = "-1";
            VMSer.Update(VMEnt, DT);

        }

        VAREnt = new VOUCHER_ALTER_REQUEST();
        VAREnt.VOUCHER_PK_ID = lblPK_id.Text;
        VAREnt.ALTERED_BY = userProfileEnt.EmployeeID;
        VAREnt.ALTER_DETAIL = txtVoucherCancelNote.Text;
        VAREnt.ALTERED_DATE = PGD.GetTodayDate("dd/mm/yyyy");
        VAREnt.STATUS = "2";// status -1 is correction request; 1 is corrected; 2 is voucher cancel
        VAREnt.OFFICE_CODE = userProfileEnt.LocationID;
        VARSer.Insert(VAREnt, DT);
        if (DT.HAPPY == true)
        {
            DT.Commit();
            LoadList();
            divVoucherCancel.Visible = false;
            divBtns.Visible = false;
            txtVoucherCancelNote.Text = "";
        }
        else
        {
            DT.Abort();
            HelperFunction.MsgBox(this, this.GetType(), "Voucher is not Cancled. Something goes wrong. ");
        }
        DT.Dispose();
    }


    protected void btnAlter_Click(object sender, EventArgs e)
    {
        LaodBank();
        LoadModeOfPayment();
        LaodGLAccount();
        LoadinVoucherDetail(lblPK_id.Text);
        divVoucherEntry.Visible = true;        
        divDrEntry.Visible = true;
        divDebittPart.Visible = true;
        divBtns.Visible = false;
        divHide.Visible = false;
        btnSaveVoucher.Visible = false;
        btnUpdate.Visible = true;
        divVoucherImage.Visible = false;
    }
    protected void LoadinVoucherDetail(string VoucherMasterId)
    {
        VMEnt = new VOUCHER_MASTER();
        VMEnt.PK_ID = VoucherMasterId;
        VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
        if (VMEnt != null)
        {
            txtVoucherDate.Text = VMEnt.VOUCHER_DAY + "/" + VMEnt.VOUCHER_MONTH + "/" + VMEnt.VOUCHER_YEAR;
            txtNarration.Text = VMEnt.NARRATION;

            VCEnt = new VOUCHER_CHILD();
            VCEnt.VOUCHER_ID = VoucherMasterId;
            EntityList theList = new EntityList();
            theList = VCSer.GetAll(VCEnt);

            DataTable dummyTable = new DataTable();
            dummyTable.Columns.Add("GL_CODE");
            dummyTable.Columns.Add("GL_NAME");
            dummyTable.Columns.Add("SGL_CODE");
            dummyTable.Columns.Add("SGL_NAME");
            dummyTable.Columns.Add("AMOUNT");

            if (theList.Count > 0)
            {
                foreach (EntityBase theEntityBase in theList)
                {
                    VCEnt = (VOUCHER_CHILD)theEntityBase;

                    if (VCEnt.REMARKS == "By")
                    {
                        DataRow dummyRow = dummyTable.NewRow();
                        dummyRow["GL_CODE"] = VCEnt.GL_CODE;
                        dummyRow["GL_NAME"] = af.getGLAccountName(VCEnt.GL_CODE);
                        dummyRow["SGL_CODE"] = VCEnt.SGL_CODE;
                        dummyRow["SGL_NAME"] = af.getSGLAccountName(VCEnt.SGL_CODE);
                        dummyRow["AMOUNT"] = VCEnt.DR_AMOUNT;
                        dummyTable.Rows.Add(dummyRow);
                    }
                    else if (VCEnt.REMARKS == "To")
                    {
                        if (VCEnt.GL_CODE == "010101") // Bank bhaye
                        {
                            GLSAEnt = new GL_SUB_ACCOUNT();
                            GLSAEnt.SUB_GL_CODE = VCEnt.SGL_CODE;
                            GLSAEnt = (GL_SUB_ACCOUNT)GLSASer.GetSingle(GLSAEnt);
                            if (GLSAEnt != null)
                            {
                                lblBank.Text = GLSAEnt.SUB_GL_NAME;
                            }
                            lblSGL_CODE.Text = VCEnt.SGL_CODE;
                            lblTotalCrAmount.Text = Convert.ToDouble(VCEnt.CR_AMOUNT).ToString("0.00");


                            VCREnt = new VOUCHER_CHILD_REFERENCE();
                            VCREnt.VC_PK_ID = VoucherMasterId;
                            VCREnt = (VOUCHER_CHILD_REFERENCE)VCRSer.GetSingle(VCREnt);
                            if (VCREnt != null)
                            {
                                ddlPaymentMode.SelectedValue = VCREnt.MOP;
                                lblRefNo.Text = VCREnt.REF_NUMBER;
                                lblRefDate.Text = VCREnt.REF_DAY + "/" + VCREnt.REF_MONTH + "/" + VCREnt.REF_YEAR;
                            }
                            divShowBankDetail.Visible = true;
                        }
                        else // cash bahye
                        {
                            lblTotalCrAmount.Text = Convert.ToDouble(VCEnt.CR_AMOUNT).ToString("0.00");
                            txtCashAmount.Text = lblTotalCrAmount.Text;
                            divVoucheDate.Visible = true;
                            divEntryCash.Visible = true;
                            divShowBankDetail.Visible = false;
                        }
                    }
                }
            }
            DataView dv = new DataView(dummyTable);
            grdDebit.DataSource = dv;
            grdDebit.DataBind();
        }
    }


    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!IsPageRefresh) // to check if it is post back 
        {
            DistributedTransaction DT = new DistributedTransaction();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            if (lblTotalCrAmount.Text == lblTotalDrAmount.Text)
            {
                #region to insert in Voucher Master
                string[] nepalidate = txtVoucherDate.Text.Split('/');
                VMEnt = new VOUCHER_MASTER();
                VMEnt.PK_ID = lblPK_id.Text;
                VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
                if (VMEnt != null)
                {
                    VMEnt.TRN_AMOUNT = lblTotalDrAmount.Text;
                    VMEnt.NARRATION = txtNarration.Text;
                    VMEnt.PREPARE_BY = userProfileEnt.EmployeeID;

                    if (af.VoucherCheckProvision())   //  organization ko voucher check garni permission cha bhaya
                    {
                        VMEnt.CHECKED_BY = "";
                        VMEnt.STATUS = "0";//status 0 is voucher created
                    }
                    else // yedi org ko voucher check garni permission chaina bhaye teyo voucher create garni bela ma automatically checked huncha
                    {
                        VMEnt.CHECKED_BY = userProfileEnt.EmployeeID;
                        VMEnt.STATUS = "1";//status 1  voucher is created and checked
                    }
                    VMEnt.APPROVED_BY = "";
                    VMSer.Update(VMEnt, DT);
                }
                #endregion
                #region to delect voucher child record
                // voucher child ma update nagari pahila voucher child ma bahye ko teyo voucher ko sabai record delete garni
                // naya voucher child ko rows ra purano voucher child ko rows farak huna sakni bhaye ko le yesto gare ko
                VCEnt = new VOUCHER_CHILD();
                VCEnt.VOUCHER_ID = lblPK_id.Text;
                VCSer.Delete(VCEnt, DT);
                #endregion
                #region to insert in to voucher child
                int sno = 1;
                #region for Dr Part
                foreach (GridViewRow gr in grdDebit.Rows)
                {
                    Label lblGLCode = gr.FindControl("lblGLCode") as Label;
                    Label lblSGLCode = gr.FindControl("lblSGLCode") as Label;
                    Label lblAmount = gr.FindControl("lblAmount") as Label;


                    VCEnt = new VOUCHER_CHILD();
                    VCEnt.VOUCHER_ID = lblPK_id.Text;
                    VCEnt.SNO = sno.ToString();
                    VCEnt.GL_CODE = lblGLCode.Text;
                    VCEnt.SGL_CODE = lblSGLCode.Text;
                    VCEnt.DR_AMOUNT = lblAmount.Text;
                    VCEnt.CR_AMOUNT = "0";
                    VCEnt.REMARKS = "By";
                    VCREnt.OFFICE_CODE = userProfileEnt.LocationID;
                    VCSer.Insert(VCEnt, DT);
                    sno++;
                }

                #endregion
                #region for Cr Part
                VCEnt = new VOUCHER_CHILD();
                VCEnt.VOUCHER_ID = lblPK_id.Text;
                VCEnt.SNO = sno.ToString();
                if (ddlPaymentMode.SelectedValue == "Cash")
                {
                    VCEnt.GL_CODE = "010102";//bank GL Code
                    VCEnt.SGL_CODE = "";
                }
                else if (ddlPaymentMode.SelectedValue == "Petty Cash")
                {
                    VCEnt.GL_CODE = "010103";//bank GL Code
                    VCEnt.SGL_CODE = "";
                }
                else
                {
                    VCEnt.GL_CODE = "010101";//bank GL Code
                    VCEnt.SGL_CODE = lblSGL_CODE.Text;
                }
                VCEnt.DR_AMOUNT = "0";
                VCEnt.CR_AMOUNT = lblTotalCrAmount.Text;
                VCEnt.REMARKS = "To";
                VCREnt.OFFICE_CODE = userProfileEnt.LocationID;
                string VC_PK_ID = VCSer.Insert(VCEnt, DT).ToString(); ;
                #endregion

                #region for voucher child Reference
                VCREnt = new VOUCHER_CHILD_REFERENCE();
                VCREnt.VC_PK_ID = lblPK_id.Text;
                VCREnt = (VOUCHER_CHILD_REFERENCE)VCRSer.GetSingle(VCREnt);
                if (VCREnt != null)
                {
                    VCREnt.MOP = ddlPaymentMode.SelectedValue;
                    VCREnt.REF_NUMBER = lblRefNo.Text;
                    string[] refDate = lblRefDate.Text.Split('/');
                    VCREnt.REF_DAY = refDate[0];
                    VCREnt.REF_MONTH = refDate[1];
                    VCREnt.REF_YEAR = refDate[2];
                    VCREnt.REF_FY = PGD.checkFiscalYear(refDate[1], refDate[2]);
                    VCREnt.REF_DATE = PGD.ConvertNepaliTOEnglish(refDate[0], refDate[1], refDate[2]);
                    VCREnt.AMOUNT = lblTotalCrAmount.Text;
                    VCREnt.STATUS = "1";
                    VCREnt.OFFICE_CODE = userProfileEnt.LocationID;
                    VCRSer.Update(VCREnt, DT);
                }
                else
                {
                    VCREnt = new VOUCHER_CHILD_REFERENCE();
                    VCREnt.MOP = ddlPaymentMode.SelectedValue;
                    VCREnt.REF_NUMBER = lblRefNo.Text;
                    string[] refDate = lblRefDate.Text.Split('/');
                    VCREnt.REF_DAY = refDate[0];
                    VCREnt.REF_MONTH = refDate[1];
                    VCREnt.REF_YEAR = refDate[2];
                    VCREnt.REF_FY = PGD.checkFiscalYear(refDate[1], refDate[2]);
                    VCREnt.REF_DATE = PGD.ConvertNepaliTOEnglish(refDate[0], refDate[1], refDate[2]);
                    VCREnt.AMOUNT = lblTotalCrAmount.Text;
                    VCREnt.STATUS = "1";
                    VCREnt.OFFICE_CODE = userProfileEnt.LocationID;
                    VCRSer.Insert(VCREnt, DT);
                }
                #endregion
                #endregion
                #region to update voucher alter request table
                VAREnt = new VOUCHER_ALTER_REQUEST();
                VAREnt.VOUCHER_PK_ID = lblPK_id.Text;
                VAREnt.STATUS = "0";
                VAREnt = (VOUCHER_ALTER_REQUEST)VARSer.GetSingle(VAREnt, DT);
                if (VAREnt != null)
                {
                    VAREnt.STATUS = "1";
                    VAREnt.ALTERED_BY = userProfileEnt.EmployeeID;
                    VAREnt.ALTERED_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    VCREnt.OFFICE_CODE = userProfileEnt.LocationID;
                    VARSer.Update(VAREnt, DT);
                }
                #endregion
                if (DT.HAPPY == true)
                {
                    DT.Commit();
                    LoadList();
                    uploadFile(lblVoucherPK_ID.Text);
                    divVoucheDate.Visible = false;
                    divEntryBankDetail.Visible = false;
                    divShowBankDetail.Visible = false;
                    divDrEntry.Visible = false;
                    divDebittPart.Visible = false;
                }
                else
                {
                    DT.Abort();
                    HelperFunction.MsgBox(this, this.GetType(), "Something goes wrong");
                }
                DT.Dispose();
            }
            else
            {
                HelperFunction.MsgBox(this, this.GetType(), "Debit and Credit Amount are not equal.");
            }
        }
    }

    protected void uploadFile(String voucher_pk_id)
    {
        VMEnt = new VOUCHER_MASTER();
        VMEnt.PK_ID = voucher_pk_id;
        VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
        if (VMEnt != null)
        {
            #region upload file
            // Check if a file has been uploaded
            if (voucherUpload.HasFile)
            {
                try
                {
                    // Specify the folder path where the image will be saved
                    string folderPath = Server.MapPath("~/images/vouchers/");

                    // Ensure the folder exists; create it if it does not exist
                    if (!System.IO.Directory.Exists(folderPath))
                    {
                        System.IO.Directory.CreateDirectory(folderPath);
                    }

                    // Validate the uploaded file type (allow only image files)
                    string fileExtension = System.IO.Path.GetExtension(voucherUpload.FileName).ToLower();
                    if (fileExtension != ".pdf" || fileExtension != ".jpeg" || fileExtension != ".jpg" || fileExtension != ".png")
                    {
                        lblMsg.Text = "Only Images and PDF files are allowed.";
                    }

                    // Generate a new file name (e.g., based on some unique identifier)
                    string fileName = "Voucher" + VMEnt.PK_ID + fileExtension;

                    // Combine the folder path with the new file name
                    string filePath = System.IO.Path.Combine(folderPath, fileName);


                    // Extract the file name without extension
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

                    // Check if a file with that name exists (ignoring extension)
                    foreach (string existingFile in Directory.GetFiles(folderPath))
                    {
                        if (Path.GetFileNameWithoutExtension(existingFile) == fileNameWithoutExtension)
                        {
                            // Delete the existing file
                            File.Delete(existingFile);
                            break; // Only delete the first matching file
                        }
                    }

                    // Save the uploaded file to the specified path
                    voucherUpload.SaveAs(filePath);
                }
                catch (Exception ex)
                {
                    // Handle and display errors
                    lblMsg.Text = "An error occurred while uploading the file: " + ex.Message;
                }
            }

            #endregion
        }
    }

    protected void ShowVoucher(string pk_id)
    {

        string folderVirtualPath = "~/images/vouchers/";
        string folderPhysicalPath = Server.MapPath(folderVirtualPath);
        string[] supportedExtensions = { ".png", ".jpg", ".jpeg", ".pdf" };
        string imgVirtualPath;
        foreach (var extension in supportedExtensions)
        {
            string fileName = "Voucher" + pk_id + extension;
            string filePhysicalPath = System.IO.Path.Combine(folderPhysicalPath, fileName);

            if (File.Exists(filePhysicalPath))
            {
                if (extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    // Display PDF button
                    string pdfVirtualPath = folderVirtualPath + fileName;
                    btnPdf.Visible = true;
                    btnPdf.CommandArgument = pk_id;
                    voucherImage.Visible = false;
                }
                else
                {
                    // Display image
                    imgVirtualPath = folderVirtualPath + fileName;
                    voucherImage.ImageUrl = imgVirtualPath;
                    voucherImage.Visible = true;

                    btnPdf.Visible = false;
                }
                return;
            }
            else
            {
                voucherImage.Visible = false;
            }
        }

    }

    protected void btnPdf_Click(object sender, ImageClickEventArgs e)
    {
        string folderVirtualPath = "~/images/vouchers/";
        string folderPhysicalPath = Server.MapPath(folderVirtualPath);
        string supportedExtensions = ".pdf";
        string fileName = "Voucher" + lblPK_id.Text + supportedExtensions;
        string filePhysicalPath = System.IO.Path.Combine(folderPhysicalPath, fileName);
        string voucherVirtualPath = string.Empty;
        bool fileFound = false;
        if (File.Exists(filePhysicalPath))
        {
            voucherVirtualPath = folderVirtualPath + fileName;
            fileFound = true;
        }
        if (fileFound)
        {
            string imgUrl = ResolveUrl(voucherVirtualPath);
            Response.Write("<script>window.open('" + imgUrl + "', '_blank');</script>");
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "File not uploaded or not found.");
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadList();
    }
}