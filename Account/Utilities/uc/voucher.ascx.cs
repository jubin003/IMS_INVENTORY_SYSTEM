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

public partial class Account_Utilities_uc_voucher : System.Web.UI.UserControl
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


    GL_ACCOUNT GLAEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLASer = new GL_ACCOUNTService();
    GL_SUB_ACCOUNT GLSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GLSASer = new GL_SUB_ACCOUNTService();



    UserProfileEntity userProfileEnt = new UserProfileEntity();

    EMPLOYEES EEnt = new EMPLOYEES();
    EMPLOYEESService ESer = new EMPLOYEESService();

    HelperFunction hf = new HelperFunction();
    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    AccountFunction af = new AccountFunction();


    static Boolean VoucherCreateAccess = false;
    static Boolean VoucherCheckAccess = false;
    static Boolean VoucherApproveAccess = false;
    double drTotal = 0;
    double crTotal = 0;
    public decimal Dr;
    public decimal Cr;
    static decimal Dr_total = 0;
    static decimal Cr_total = 0;
    public int CrCount;
    public decimal Dhold;
    public decimal Chold;
    public static string AcType = null;
    Boolean IsPageRefresh = false;
    static string voucher_type = "";
    static string voucher_heading = "";
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
                string[] vouchertype = path.Split('/');

                if (vouchertype[2] == "journalvoucher.aspx")
                {
                    voucher_type = "JV";
                    voucher_heading = "Journal Voucher";
                }

                else if (vouchertype[2] == "creditvoucher.aspx")
                {
                    voucher_type = "CV";
                    voucher_heading = "Credit Voucher";
                }

                else if (vouchertype[2] == "creditnote.aspx")
                {
                    voucher_type = "CN";
                    voucher_heading = "Credit Note";
                }

                else if (vouchertype[2] == "debitnote.aspx")
                {
                    voucher_type = "DN";
                    voucher_heading = "Debit Note";
                }



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
        ddlFiscalYear.SelectedValue = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
    }
    protected void LoadVoucherStatus()
    {
        // PK_ID	STATUS_NAME	    VOUCHER_STATUS	STATUS
        // 1        Prepare By      0               1
        // 2        Check By        1               0
        // 3        Approved By     2               1
        // 4        Cancelled       -1              1

        //DB ma Check by ko status 0 cha bhaye prepare by ko voucher status 1 huncha 
        //tara Check by ko status 1 cha bhaye prepare by ko voucher status 0 huncha 
        //DB ma Check by ko status 0 bhanu ko artha check by ko step escape gare ko cha 
        //jasle garda voucher ko status prepere garne biti kai 1 hucnha

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
        divBtns.Visible = false;
        divVoucherEntry.Visible = false;
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
        VMEnt.VOUCHER_TYPE = voucher_type;
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
                ShowImage(VMEnt.PK_ID);
            }
            grdVoucherList.Visible = false;
            divHide.Visible = true;
            divBtns.Visible = true;
            divVoucherImage.Visible = true;
            LoadVoucher();
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

            lblVoucherType.Text = voucher_heading;
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
        divHide.Visible = false;
        grdVoucherList.Visible = false;
        divVoucherEntry.Visible = true;
        btnSaveVoucher.Visible = true;
        btnUpdate.Visible = false;
        CreateGridFirst();
        divVoucherImage.Visible = false;
        txtVoucherDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
    }
    protected DataTable CreateGridFirst()
    {
        int row = grdVoucher.Rows.Count;
        DataTable dummyTable = new DataTable();

        dummyTable.Columns.Add("GL_CODE");
        dummyTable.Columns.Add("SGL_CODE");
        dummyTable.Columns.Add("CREDIT");
        dummyTable.Columns.Add("DEBIT");


        DataRow dummyRw = dummyTable.NewRow();
        dummyRw["GL_CODE"] = "";
        dummyRw["SGL_CODE"] = "";
        dummyRw["CREDIT"] = "0";
        dummyRw["DEBIT"] = "";

        dummyTable.Rows.Add(dummyRw);

        dummyRw = dummyTable.NewRow();
        dummyRw["GL_CODE"] = "";
        dummyRw["SGL_CODE"] = "";
        dummyRw["CREDIT"] = "";
        dummyRw["DEBIT"] = "0";

        dummyTable.Rows.Add(dummyRw);

        DataView dv = new DataView(dummyTable);
        grdVoucher.DataSource = dv;
        grdVoucher.DataBind();
        return dummyTable;
    }
    protected DataTable CreateGrid()
    {
        int row = grdVoucher.Rows.Count;
        DataTable dummyTable = new DataTable();

        dummyTable.Columns.Add("GL_CODE");
        dummyTable.Columns.Add("GL_ACCOUNT");
        dummyTable.Columns.Add("SGL_CODE");
        dummyTable.Columns.Add("SGL_NAME");
        dummyTable.Columns.Add("CREDIT");
        dummyTable.Columns.Add("DEBIT");

        if (row > 0)
        {
            foreach (GridViewRow r in grdVoucher.Rows)
            {
                DropDownList ddlByTo = grdVoucher.Rows[r.RowIndex].FindControl("ddlByTo") as DropDownList;
                TextBox txtGLCode = grdVoucher.Rows[r.RowIndex].FindControl("txtGLCode") as TextBox;
                DropDownList ddlGLAccount = grdVoucher.Rows[r.RowIndex].FindControl("ddlGLAccount") as DropDownList;
                DropDownList ddlSGLAccount = grdVoucher.Rows[r.RowIndex].FindControl("ddlSGLAccount") as DropDownList;
                Label lblSGLCode = grdVoucher.Rows[r.RowIndex].FindControl("lblSGLCode") as Label;
                TextBox Debit = grdVoucher.Rows[r.RowIndex].FindControl("txtDebit") as TextBox;
                TextBox Credit = grdVoucher.Rows[r.RowIndex].FindControl("txtCredit") as TextBox;

                DataRow dummyRow = dummyTable.NewRow();
                AcType = ddlByTo.SelectedValue;

                dummyRow["GL_CODE"] = txtGLCode.Text;
                dummyRow["GL_ACCOUNT"] = "";
                dummyRow["SGL_CODE"] = lblSGLCode.Text;
                dummyRow["SGL_NAME"] = "";
                dummyRow["CREDIT"] = Credit.Text == "" ? "0" : Credit.Text;
                dummyRow["DEBIT"] = Debit.Text == "" ? "0" : Debit.Text;
                dummyTable.Rows.Add(dummyRow);
            }
        }

        DataView dv = new DataView(dummyTable);
        grdVoucher.DataSource = dv;
        grdVoucher.DataBind();
        return dummyTable;
    }
    protected DataTable CreateGrid(int RowIndex, bool addRemoveFlag)
    {
        int row = grdVoucher.Rows.Count;
        DataTable dummyTable = new DataTable();

        dummyTable.Columns.Add("GL_CODE");
        dummyTable.Columns.Add("GL_ACCOUNT");
        dummyTable.Columns.Add("SGL_CODE");
        dummyTable.Columns.Add("SGL_NAME");
        dummyTable.Columns.Add("CREDIT");
        dummyTable.Columns.Add("DEBIT");


        if (row > 0)
        {
            foreach (GridViewRow r in grdVoucher.Rows)
            {
                DropDownList ddlByTo = grdVoucher.Rows[r.RowIndex].FindControl("ddlByTo") as DropDownList;
                TextBox txtGLCode = grdVoucher.Rows[r.RowIndex].FindControl("txtGLCode") as TextBox;
                DropDownList ddlGLAccount = grdVoucher.Rows[r.RowIndex].FindControl("ddlGLAccount") as DropDownList;
                DropDownList ddlSGLAccount = grdVoucher.Rows[r.RowIndex].FindControl("ddlSGLAccount") as DropDownList;
                Label lblSGLCode = grdVoucher.Rows[r.RowIndex].FindControl("lblSGLCode") as Label;
                TextBox Debit = grdVoucher.Rows[r.RowIndex].FindControl("txtDebit") as TextBox;
                TextBox Credit = grdVoucher.Rows[r.RowIndex].FindControl("txtCredit") as TextBox;

                DataRow dummyRow = dummyTable.NewRow();
                AcType = ddlByTo.SelectedValue;
                dummyRow["GL_CODE"] = txtGLCode.Text;
                dummyRow["GL_ACCOUNT"] = "";
                dummyRow["SGL_CODE"] = lblSGLCode.Text;
                dummyRow["SGL_NAME"] = "";
                dummyRow["CREDIT"] = Credit.Text == "" ? "0" : Credit.Text;
                dummyRow["DEBIT"] = Debit.Text == "" ? "0" : Debit.Text;

                dummyTable.Rows.Add(dummyRow);
                if (r.RowIndex == RowIndex && addRemoveFlag == true)
                {
                    DataRow dummyRw = dummyTable.NewRow();
                    dummyRow["GL_CODE"] = txtGLCode.Text;
                    dummyRow["GL_ACCOUNT"] = "";
                    dummyRow["SGL_CODE"] = lblSGLCode.Text;
                    dummyRow["SGL_NAME"] = "";
                    dummyRw["CREDIT"] = "0";
                    dummyRw["DEBIT"] = "";
                    dummyTable.Rows.Add(dummyRw);
                }
                else if (r.RowIndex == RowIndex && addRemoveFlag == false)
                    dummyTable.Rows.Remove(dummyRow);
            }
        }

        DataView dv = new DataView(dummyTable);
        grdVoucher.DataSource = dv;
        grdVoucher.DataBind();
        return dummyTable;
    }
    protected void grdVoucher_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ToBy = e.Row.FindControl("ddlByTo") as DropDownList;
            DropDownList ddlByTo = e.Row.FindControl("ddlByTo") as DropDownList;
            TextBox txtGLCode = e.Row.FindControl("txtGLCode") as TextBox;
            DropDownList ddlGLAccount = e.Row.FindControl("ddlGLAccount") as DropDownList;
            DropDownList ddlSGLAccount = e.Row.FindControl("ddlSGLAccount") as DropDownList;
            Label lblSGLCode = e.Row.FindControl("lblSGLCode") as Label;
            TextBox Debit = e.Row.FindControl("txtDebit") as TextBox;
            TextBox Credit = e.Row.FindControl("txtCredit") as TextBox;
            Label lblPkid = (Label)e.Row.FindControl("lblPkid") as Label;


            if (grdVoucher.Rows.Count == 0)
            {
                ToBy.SelectedValue = "By";
            }
            else if (!(Debit.Text.Equals("0") || Debit.Text.Equals("0.00")))
            {
                ToBy.SelectedValue = "By";
            }
            else if (!(Credit.Text.Equals("0") || Credit.Text.Equals("0.00")))
            {
                ToBy.SelectedValue = "To";
            }
            else
            {
                ToBy.SelectedValue = AcType;
            }
            ddlGLAccount.Items.Clear();
            GLAEnt = new GL_ACCOUNT();
            GLAEnt.STATUS = "1";
            ddlGLAccount.DataSource = GLASer.GetAll(GLAEnt);
            ddlGLAccount.DataTextField = "GL_NAME";
            ddlGLAccount.DataValueField = "GL_CODE";
            ddlGLAccount.DataBind();
            ddlGLAccount.Items.Insert(0, "Select");
            if (txtGLCode.Text != "")
            {
                ddlGLAccount.SelectedValue = txtGLCode.Text;
            }

            ddlSGLAccount.Items.Clear();
            GLSAEnt = new GL_SUB_ACCOUNT();

            GLSAEnt.GL_CODE = ddlGLAccount.SelectedValue;
            ddlSGLAccount.DataSource = GLSASer.GetAll(GLSAEnt);
            ddlSGLAccount.DataTextField = "SUB_GL_NAME";
            ddlSGLAccount.DataValueField = "SUB_GL_CODE";
            ddlSGLAccount.DataBind();
            ddlSGLAccount.Items.Insert(0, "Select");
            ddlSGLAccount.SelectedValue = lblSGLCode.Text;

            GLAEnt = new GL_ACCOUNT();
            GLAEnt.GL_CODE = txtGLCode.Text;
            GLAEnt = (GL_ACCOUNT)GLASer.GetSingle(GLAEnt);
            if (GLAEnt != null && txtGLCode.Text != "")
            {
                if (GLAEnt.SUB_LEDGER == "1")
                    ddlSGLAccount.Visible = true;
                else
                    ddlSGLAccount.Visible = false;
            }



        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox txtTotalDebit = e.Row.FindControl("txtTotalDebit") as TextBox;
            TextBox txtTotalCredit = e.Row.FindControl("txtTotalCredit") as TextBox;
            AddDrCr();
            txtTotalCredit.Text = Cr.ToString();
            txtTotalDebit.Text = Dr.ToString();

            Dr_total = Dr;
            Cr_total = Cr;
        }

    }
    protected void grdVoucher_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Add"))
        {
            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            CreateGrid(gr.RowIndex, true);
            DropDownList ddlByTo = grdVoucher.Rows[gr.RowIndex + 1].FindControl("ddlByTo") as DropDownList;
            TextBox Debit = grdVoucher.Rows[gr.RowIndex + 1].FindControl("txtDebit") as TextBox;
            TextBox Credit = grdVoucher.Rows[gr.RowIndex + 1].FindControl("txtCredit") as TextBox;
            ddlByTo.SelectedIndex = ((DropDownList)gr.FindControl("ddlByTo")).SelectedIndex;
            if (ddlByTo.SelectedValue.Equals("By"))
            {
                Debit.ReadOnly = false;
                Credit.ReadOnly = true;
                Debit.Text = "";
                Credit.Text = "0";
            }
            else
            {
                Debit.ReadOnly = true;
                Credit.ReadOnly = false;
                Debit.Text = "0";
                Credit.Text = "";
            }
            ddlByTo.Focus();

            //AddRow = 1;
        }

        if (e.CommandName.Equals("Remove"))
        {
            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            CreateGrid(gr.RowIndex, false);

            AddDrCr();
            foreach (GridViewRow grdRow in grdVoucher.Rows)
            {
                DropDownList ToBy = (DropDownList)grdRow.FindControl("ddlByTo");
                TextBox txtDebit = (TextBox)grdRow.FindControl("txtDebit");
                TextBox txtCredit = (TextBox)grdRow.FindControl("txtCredit");

                if (ToBy.SelectedValue == "To")
                {
                    if (!string.IsNullOrEmpty(txtCredit.Text))
                    {
                        if (Dr > Cr)
                            txtCredit.Text = (Convert.ToDecimal(txtCredit.Text) + Math.Abs(Dr - Cr)).ToString();
                        else
                            txtCredit.Text = (Convert.ToDecimal(txtCredit.Text) - Math.Abs(Dr - Cr)).ToString();
                    }
                    else
                        txtCredit.Text = (Math.Abs(Dr - Cr)).ToString();
                    break;
                }
            }
            AddDrCr();
            TextBox txtTotalDebit = (TextBox)grdVoucher.FooterRow.FindControl("txtTotalDebit");
            TextBox txtTotalCredit = (TextBox)grdVoucher.FooterRow.FindControl("txtTotalCredit");
            txtTotalCredit.Text = Cr.ToString();
            txtTotalDebit.Text = Dr.ToString();
        }
    }
    protected void AddDrCr()
    {
        CrCount = 0;
        Dr = 0;
        Cr = 0;
        foreach (GridViewRow row in grdVoucher.Rows)
        {
            TextBox txtParticular = row.FindControl("txtParticular") as TextBox;
            DropDownList ddlByTo = row.FindControl("ddlByTo") as DropDownList;
            TextBox DebitAmt = row.FindControl("txtDebit") as TextBox;
            TextBox CreditAmt = row.FindControl("txtCredit") as TextBox;
            if (ddlByTo.SelectedValue.Equals("To"))
            {
                CrCount++;
            }

            if (DebitAmt.Text != "")
            {
                Dhold = Convert.ToDecimal(DebitAmt.Text);
                Dr = Dr + Dhold;
            }
            if (CreditAmt.Text != "")
            {
                Chold = Convert.ToDecimal(CreditAmt.Text);
                Cr = Cr + Chold;
            }
        }

    }
    protected void txtDebit_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gr = (GridViewRow)((TextBox)sender).Parent.Parent;
        DropDownList ddlByTo = gr.FindControl("ddlByTo") as DropDownList;
        TextBox Debit = gr.FindControl("txtDebit") as TextBox;
        TextBox Credit = gr.FindControl("txtCredit") as TextBox;

        if (!string.IsNullOrEmpty(Debit.Text))
        {
            try
            {
                Debit.Text = (Convert.ToDouble(Debit.Text)).ToString("#0.00");
                AddDrCr();

                if (Dr != Cr)
                {
                    foreach (GridViewRow grdRow in grdVoucher.Rows)
                    {
                        DropDownList ToBy = (DropDownList)grdRow.FindControl("ddlByTo");
                        TextBox txtDebit = (TextBox)grdRow.FindControl("txtDebit");
                        TextBox txtCredit = (TextBox)grdRow.FindControl("txtCredit");

                        if (ToBy.SelectedValue == "To")
                        {

                            if (!string.IsNullOrEmpty(txtCredit.Text))
                            {
                                if (Dr > Cr)
                                    txtCredit.Text = (Convert.ToDecimal(txtCredit.Text) + Math.Abs(Dr - Cr)).ToString();
                                else
                                    txtCredit.Text = (Convert.ToDecimal(txtCredit.Text) - Math.Abs(Dr - Cr)).ToString();
                            }
                            else
                            {
                                txtCredit.Text = (Math.Abs(Dr - Cr)).ToString();

                            }
                            break;
                        }
                    }
                }

                CreateGrid();

                if (gr.RowIndex + 1 < grdVoucher.Rows.Count)
                {
                    DropDownList ddlByTo1 = grdVoucher.Rows[gr.RowIndex + 1].FindControl("ddlByTo") as DropDownList;
                    ddlByTo1.Focus();
                }
                else
                    txtNarration.Focus();

            }
            catch
            {
                HelperFunction.MsgBox(this, this.GetType(), "Enter Number Only");
                Debit.Text = "";
                Debit.Focus();
            }
        }
        else
            Debit.Focus();


    }
    protected void txtCredit_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gr = (GridViewRow)((TextBox)sender).Parent.Parent;
        DropDownList ddlByTo = gr.FindControl("ddlByTo") as DropDownList;
        TextBox Debit = gr.FindControl("txtDebit") as TextBox;
        TextBox Credit = gr.FindControl("txtCredit") as TextBox;
        DropDownList ddlByToAll;
        TextBox CreditPrev = null;

        foreach (GridViewRow gr1 in grdVoucher.Rows)
        {
            ddlByToAll = gr1.FindControl("ddlByTo") as DropDownList;
            if (ddlByToAll.SelectedItem.Text == "To")
            {
                CreditPrev = gr1.FindControl("txtCredit") as TextBox;
                break;
            }
        }

        if (!string.IsNullOrEmpty(Credit.Text))
        {
            try
            {
                Credit.Text = (Convert.ToDouble(Credit.Text)).ToString("#0.00");
                AddDrCr();
                if (CreditPrev != null)
                {
                    if (Dr != Cr)
                    {
                        if (CrCount > 1)
                        {
                            if (gr.RowIndex + 1 == grdVoucher.Rows.Count)
                            {
                                if (!string.IsNullOrEmpty(CreditPrev.Text))
                                {
                                    if (Dr > Cr)
                                        CreditPrev.Text = (Convert.ToDecimal(CreditPrev.Text) + Math.Abs(Dr - Cr)).ToString();
                                    else
                                        CreditPrev.Text = (Convert.ToDecimal(CreditPrev.Text) - Math.Abs(Dr - Cr)).ToString();

                                }
                                else
                                {
                                    CreditPrev.Text = (Math.Abs(Dr - Cr)).ToString();

                                }
                            }
                            else
                            {
                                TextBox CreditNext = grdVoucher.Rows[gr.RowIndex + 1].FindControl("txtCredit") as TextBox;
                                if (!string.IsNullOrEmpty(CreditNext.Text))
                                {
                                    if (Dr > Cr)
                                        CreditNext.Text = (Convert.ToDecimal(CreditNext.Text) + Math.Abs(Dr - Cr)).ToString();
                                    else
                                        CreditNext.Text = (Convert.ToDecimal(CreditNext.Text) - Math.Abs(Dr - Cr)).ToString();


                                }
                                else
                                {
                                    CreditNext.Text = (Math.Abs(Dr - Cr)).ToString();

                                }
                            }
                        }
                        else
                        {
                            TextBox DebitPrev = grdVoucher.Rows[gr.RowIndex - 1].FindControl("txtDebit") as TextBox;
                            if (!string.IsNullOrEmpty(DebitPrev.Text))
                            {
                                if (Dr > Cr)
                                    DebitPrev.Text = (Convert.ToDecimal(DebitPrev.Text) - Math.Abs(Dr - Cr)).ToString();
                                else
                                    DebitPrev.Text = (Convert.ToDecimal(DebitPrev.Text) + Math.Abs(Dr - Cr)).ToString();
                            }
                            else
                                DebitPrev.Text = (Math.Abs(Dr - Cr)).ToString();


                        }
                    }
                }

                CreateGrid();
                if (gr.RowIndex + 1 < grdVoucher.Rows.Count)
                {
                    DropDownList ddlByTo1 = grdVoucher.Rows[gr.RowIndex + 1].FindControl("ddlByTo") as DropDownList;
                    ddlByTo1.Focus();
                }
                else
                    txtNarration.Focus();
            }
            catch
            {
                HelperFunction.MsgBox(this, this.GetType(), "Enter Number only");
                Credit.Text = "";
                Credit.Focus();
            }
        }
        else
            Credit.Focus();

    }
    protected void txtAccNo_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gr = (GridViewRow)((TextBox)sender).Parent.Parent;
        TextBox txtGLCode = gr.FindControl("txtGLCode") as TextBox;
        DropDownList ddlGLAccount = gr.FindControl("ddlGLAccount") as DropDownList;
        DropDownList ddlSGLAccount = gr.FindControl("ddlSGLAccount") as DropDownList;

        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_CODE = txtGLCode.Text;
        GLAEnt = (GL_ACCOUNT)GLASer.GetSingle(GLAEnt);
        if (GLAEnt != null)
        {
            GLAEnt = new GL_ACCOUNT();
            GLAEnt.STATUS = "1";
            ddlGLAccount.DataSource = GLASer.GetAll(GLAEnt);
            ddlGLAccount.DataTextField = "GL_NAME";
            ddlGLAccount.DataValueField = "GL_CODE";
            ddlGLAccount.DataBind();
            ddlGLAccount.Items.Insert(0, "Select");
            ddlGLAccount.SelectedValue = txtGLCode.Text;

            GLSAEnt = new GL_SUB_ACCOUNT();
            GLSAEnt.GL_CODE = ddlGLAccount.SelectedValue;
            ddlSGLAccount.DataSource = GLSASer.GetAll(GLSAEnt);
            ddlSGLAccount.DataTextField = "SUB_GL_NAME";
            ddlSGLAccount.DataValueField = "SUB_GL_CODE";
            ddlSGLAccount.DataBind();
            ddlGLAccount.Items.Insert(0, "Select");
        }

    }
    protected void ddlGLAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gr = (GridViewRow)((DropDownList)sender).Parent.Parent;
        TextBox txtGLCode = gr.FindControl("txtGLCode") as TextBox;
        DropDownList ddlGLAccount = gr.FindControl("ddlGLAccount") as DropDownList;
        DropDownList ddlSGLAccount = gr.FindControl("ddlSGLAccount") as DropDownList;

        GLSAEnt = new GL_SUB_ACCOUNT();

        GLSAEnt.GL_CODE = ddlGLAccount.SelectedValue;
        ddlSGLAccount.DataSource = GLSASer.GetAll(GLSAEnt);
        ddlSGLAccount.DataTextField = "SUB_GL_NAME";
        ddlSGLAccount.DataValueField = "SUB_GL_CODE";
        ddlSGLAccount.DataBind();
        ddlSGLAccount.Items.Insert(0, "Select");

        txtGLCode.Text = ddlGLAccount.SelectedValue;

        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_CODE = txtGLCode.Text;
        GLAEnt = (GL_ACCOUNT)GLASer.GetSingle(GLAEnt);
        if (GLAEnt != null)
        {
            if (GLAEnt.SUB_LEDGER == "1")
                ddlSGLAccount.Visible = true;
            else
                ddlSGLAccount.Visible = false;
        }


    }
    protected void btnSaveVoucher_Click(object sender, EventArgs e)
    {

        if (!IsPageRefresh) // to check if it is post back 
        {
            DistributedTransaction DT = new DistributedTransaction();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            if (Dr_total == Cr_total)
            {
                #region to insert in Voucher Master
                string[] nepalidate = txtVoucherDate.Text.Split('/');
                VMEnt = new VOUCHER_MASTER();
                VMEnt.VOUCHER_TYPE = voucher_type;
                VMEnt.VOUCHER_NUMBER = af.getNext_VM_ID(voucher_type, PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()),userProfileEnt.LocationID);
                string engdate = PGD.ConvertNepaliTOEnglish(nepalidate[0], nepalidate[1], nepalidate[2]);
                VMEnt.VOUCHER_DATE = engdate;

                VMEnt.VOUCHER_DAY = nepalidate[0];
                VMEnt.VOUCHER_MONTH = nepalidate[1];
                VMEnt.VOUCHER_YEAR = nepalidate[2];
                VMEnt.VOUCHER_FY = PGD.checkFiscalYear(nepalidate[1], nepalidate[2]);
                VMEnt.SYSTEM_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                VMEnt.TRN_AMOUNT = Dr_total.ToString("0.00");
                VMEnt.NARRATION = txtNarration.Text;
                VMEnt.REF_TABLE = "";
                VMEnt.REF_ID = "";
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
                VMEnt.OFFICE_CODE = userProfileEnt.LocationID;
                string voucher_pk_id = VMSer.Insert(VMEnt, DT).ToString();
                #endregion


                #region to insert in to voucher child
                int sno = 1;
                foreach (GridViewRow gr in grdVoucher.Rows)
                {
                    DropDownList ddlByTo = gr.FindControl("ddlByTo") as DropDownList;
                    TextBox txtGLCode = gr.FindControl("txtGLCode") as TextBox;
                    DropDownList ddlGLAccount = gr.FindControl("ddlGLAccount") as DropDownList;
                    Label lblSGLCode = gr.FindControl("lblSGLCode") as Label;
                    DropDownList ddlSGLAccount = gr.FindControl("ddlSGLAccount") as DropDownList;
                    TextBox txtDebit = gr.FindControl("txtDebit") as TextBox;
                    TextBox txtCredit = gr.FindControl("txtCredit") as TextBox;

                    #region for Dr Part
                    if (ddlByTo.SelectedValue == "By")
                    {
                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = voucher_pk_id;
                        VCEnt.SNO = sno.ToString();
                        VCEnt.GL_CODE = txtGLCode.Text;
                        if (lblSGLCode.Text != "Select")
                            VCEnt.SGL_CODE = lblSGLCode.Text;
                        VCEnt.DR_AMOUNT = txtDebit.Text;
                        VCEnt.CR_AMOUNT = txtCredit.Text;
                        VCEnt.REMARKS = "By";
                        VCEnt.OFFICE_CODE = userProfileEnt.LocationID;
                        VCSer.Insert(VCEnt, DT);
                        sno++;
                    }

                    #region for Cr Part
                    if (ddlByTo.SelectedValue == "To")
                    {
                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = voucher_pk_id;
                        VCEnt.SNO = sno.ToString();
                        VCEnt.GL_CODE = txtGLCode.Text;
                        if (lblSGLCode.Text != "Select")
                            VCEnt.SGL_CODE = lblSGLCode.Text;
                        VCEnt.DR_AMOUNT = txtDebit.Text;
                        VCEnt.CR_AMOUNT = txtCredit.Text;
                        VCEnt.REMARKS = "To";
                        VCEnt.OFFICE_CODE = userProfileEnt.LocationID;
                        VCSer.Insert(VCEnt, DT);
                        sno++;
                    }
                    #endregion
                    #endregion
                }
                #endregion

                if (DT.HAPPY == true)
                {
                    DT.Commit();
                    LoadList();
                    divVoucherEntry.Visible = false;
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

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!IsPageRefresh) // to check if it is post back 
        {
            DistributedTransaction DT = new DistributedTransaction();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            if (Dr_total == Cr_total)
            {
                #region to update in Voucher Master               
                VMEnt = new VOUCHER_MASTER();
                VMEnt.PK_ID = lblPK_id.Text;
                VMEnt = (VOUCHER_MASTER)VMSer.GetSingle(VMEnt);
                if (VMEnt != null)
                {
                    VMEnt.TRN_AMOUNT = Dr_total.ToString("0.00");
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
                    VMEnt.OFFICE_CODE = userProfileEnt.LocationID;
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
                foreach (GridViewRow gr in grdVoucher.Rows)
                {
                    DropDownList ddlByTo = gr.FindControl("ddlByTo") as DropDownList;
                    TextBox txtGLCode = gr.FindControl("txtGLCode") as TextBox;
                    DropDownList ddlGLAccount = gr.FindControl("ddlGLAccount") as DropDownList;
                    Label lblSGLCode = gr.FindControl("lblSGLCode") as Label;
                    DropDownList ddlSGLAccount = gr.FindControl("ddlSGLAccount") as DropDownList;
                    TextBox txtDebit = gr.FindControl("txtDebit") as TextBox;
                    TextBox txtCredit = gr.FindControl("txtCredit") as TextBox;

                    #region for Dr Part
                    if (ddlByTo.SelectedValue == "By")
                    {
                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = lblPK_id.Text;
                        VCEnt.SNO = sno.ToString();
                        VCEnt.GL_CODE = txtGLCode.Text;
                        if (lblSGLCode.Text != "Select")
                            VCEnt.SGL_CODE = lblSGLCode.Text;
                        VCEnt.DR_AMOUNT = txtDebit.Text;
                        VCEnt.CR_AMOUNT = txtCredit.Text;
                        VCEnt.REMARKS = "By";
                        VCEnt.OFFICE_CODE = userProfileEnt.LocationID;
                        VCSer.Insert(VCEnt, DT);
                        
                        sno++;
                    }

                    #region for Cr Part
                    if (ddlByTo.SelectedValue == "To")
                    {
                        VCEnt = new VOUCHER_CHILD();
                        VCEnt.VOUCHER_ID = lblPK_id.Text;
                        VCEnt.SNO = sno.ToString();
                        VCEnt.GL_CODE = txtGLCode.Text;
                        if (lblSGLCode.Text != "Select")
                            VCEnt.SGL_CODE = lblSGLCode.Text;
                        VCEnt.DR_AMOUNT = txtDebit.Text;
                        VCEnt.CR_AMOUNT = txtCredit.Text;
                        VCEnt.REMARKS = "To";
                        VCEnt.OFFICE_CODE = userProfileEnt.LocationID;
                        VCSer.Insert(VCEnt, DT);
                        sno++;
                    }
                    #endregion
                    #endregion
                }
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
                    VAREnt.OFFICE_CODE = userProfileEnt.LocationID;
                    VARSer.Update(VAREnt, DT);
                }
                #endregion
                if (DT.HAPPY == true)
                {
                    DT.Commit();
                    LoadList();
                    divVoucherEntry.Visible = false;
                    uploadFile(lblVoucherPK_ID.Text);
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

    protected void btnCancelVoucherEntry_Click(object sender, EventArgs e)
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

    protected void btnCorrect_Click(object sender, EventArgs e)
    {
        divCorrectVoucher.Visible = true;
        divBtns.Visible = false;
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        LoadList();
    }

    protected void ddlSGLAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gr = (GridViewRow)((DropDownList)sender).Parent.Parent;
        Label lblSGLCode = gr.FindControl("lblSGLCode") as Label;
        DropDownList ddlSGLAccount = gr.FindControl("ddlSGLAccount") as DropDownList;
        lblSGLCode.Text = ddlSGLAccount.SelectedValue;
    }

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
        divVoucherEntry.Visible = true;
        LoadinVoucherDetail(lblPK_id.Text);
        
        divBtns.Visible = false;
        divHide.Visible = false;
        voucherImage.Visible = false;
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
            dummyTable.Columns.Add("SGL_CODE");
            dummyTable.Columns.Add("CREDIT");
            dummyTable.Columns.Add("DEBIT");

            if (theList.Count > 0)
            {
                foreach (EntityBase theEntityBase in theList)
                {
                    VCEnt = (VOUCHER_CHILD)theEntityBase;
                    DataRow dummyRow = dummyTable.NewRow();
                    dummyRow["GL_CODE"] = VCEnt.GL_CODE;
                    dummyRow["SGL_CODE"] = VCEnt.SGL_CODE;
                    dummyRow["CREDIT"] = VCEnt.CR_AMOUNT;
                    dummyRow["DEBIT"] = VCEnt.DR_AMOUNT;
                    dummyTable.Rows.Add(dummyRow);
                }
            }
            DataView dv = new DataView(dummyTable);
            grdVoucher.DataSource = dv;
            grdVoucher.DataBind();
        }
    }


    #endregion
    #region display uploaded voucher
    protected void ShowImage(string pk_id)
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


    #endregion
    protected void btnPdf_Click(object sender, EventArgs e)
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


    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadList();
    }
}