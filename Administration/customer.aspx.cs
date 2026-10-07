using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using DataHelper.Framework;
using PhyeGanCore;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using Entity.Framework;

public partial class administration_customer : System.Web.UI.Page
{
    GL_SUB_ACCOUNT SLAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService SLASer = new GL_SUB_ACCOUNTService();

    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    AREA AEnt = new AREA();
    AREAService ASer = new AREAService();

    PRODUCT_RATE_TYPE PRTEnt = new PRODUCT_RATE_TYPE();
    PRODUCT_RATE_TYPEService PRTSer = new PRODUCT_RATE_TYPEService();

    AGENT AGEnt = new AGENT();
    AGENTService AGSer = new AGENTService();

    OFFICE OEnt = new OFFICE();
    OFFICEService OSer = new OFFICEService();

    COUNTRY CYEnt = new COUNTRY();
    COUNTRYService CYSer = new COUNTRYService();

    PhyeGanProductSetting PPS = new PhyeGanProductSetting();

    PhyeGanDate PGD = new PhyeGanDate();

    Boolean IsPageRefresh = false;

    PhyeGan PG = new PhyeGan();

    HelperFunction hf = new HelperFunction();

    UserProfileEntity userProfileEnt = new UserProfileEntity();

    

    static string path = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //try
           // {
                ViewState["postids"] = System.Guid.NewGuid().ToString();
                Session["postid"] = ViewState["postids"].ToString();
                userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
               // if (hf.checkPageAccess(path, userProfileEnt.UserGroupID.ToString()))
               {
                    loadBranch();
                    LoadGrid(ddlBranchFilter.SelectedValue);
                    LoadArea();
                    LoadAgent();
                    LoadCustomers();
                    LoadCountry();
                }
                //else
                //{
                //    Response.Redirect("~/forbidden.aspx");
                //}

            //}
           // catch (Exception ww)
           // {
                //    Response.Redirect("~/Login.aspx");
           // }
        }
    }
    protected void LoadCustomers()
    {
        CEnt = new CUSTOMER();
        CEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
        CEnt.STATUS = "1";
        ddlCustomer.DataSource = CSer.GetAll(CEnt);
        ddlCustomer.DataTextField = "CUSTOMER_FULLNAME";
        ddlCustomer.DataValueField = "PK_ID";
        ddlCustomer.DataBind();
        ddlCustomer.Items.Insert(0, "Select");
    }
    protected void LoadArea()
    {
        AEnt = new AREA();
        AEnt.OFFICE_CODE = ddlBranch.SelectedValue;
        ddlArea.DataSource = ASer.GetAll(AEnt);
        ddlArea.DataTextField = "AREA_NAME";
        ddlArea.DataValueField = "PK_ID";
        ddlArea.DataBind();
        ddlArea.Items.Insert(0, "");
    }

    protected void LoadCountry()
    {

        CYEnt = new COUNTRY();
        ddlCountry.DataSource = CYSer.GetAll(CYEnt);
        ddlCountry.DataTextField = "COUNTRY_NAME";
        ddlCountry.DataValueField = "PK_ID";
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0,"");
    }

    protected void LoadAgent()
    {
        AGEnt = new AGENT();
        AGEnt.STATUS = "1";
        AGEnt.OFFICE_CODE = ddlBranch.SelectedValue;
        ddlAgent.DataSource = AGSer.GetAll(AGEnt);
        ddlAgent.DataTextField = "AGENT_NAME";
        ddlAgent.DataValueField = "PK_ID";
        ddlAgent.DataBind();
        ddlAgent.Items.Insert(0, "");
    }
    protected void LoadGrid(string office_code)
    {
        CEnt = new CUSTOMER();
        CEnt.OFFICE_CODE = office_code;
        gridCustomer.DataSource = CSer.GetAll(CEnt);
        gridCustomer.DataBind();
    }

    protected void loadBranch()
    {
        ddlBranch.DataSource = PG.getBranchList();
        ddlBranch.DataTextField = "OFFICENAME";
        ddlBranch.DataValueField = "PK_ID";
        ddlBranch.DataBind();

        ddlBranchFilter.DataSource = PG.getBranchList();
        ddlBranchFilter.DataTextField = "OFFICENAME";
        ddlBranchFilter.DataValueField = "PK_ID";
        ddlBranchFilter.DataBind();

        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (userProfileEnt.LocationTypeID == 1 && PG.checkBranchAccess(path, userProfileEnt.UserGroupID.ToString()) && PG.CompanyBranch_Status())
        {
            trBranch.Visible = true;
            trBranchFilter.Visible = true;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
            ddlBranchFilter.SelectedValue = userProfileEnt.LocationID;
        }
        else
        {
            trBranch.Visible = false;
            trBranchFilter.Visible = false;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
            ddlBranchFilter.SelectedValue = userProfileEnt.LocationID;
        }
    }

    protected void clearFields()
    {
        lblPKIDU.Text = "";
        txtCustomerCode.Text = "";
        txtCustomerName.Text = "";
        txtAddress.Text = "";
        txtContactPerson.Text = "";
        txtEmail.Text = "";
        txtMobileNo.Text = "";
        txtPANNo.Text = "";
        txtPhoneNo.Text = "";
        txtRegNo.Text = "";
        txtRemarks.Text = "";
        ddlStatus.SelectedIndex = 0;
        //ddlArea.SelectedIndex = 0;
        //ddlCountry.SelectedIndex = 0;
    }
    protected void btnAddMore_Click(object sender, EventArgs e)
    {
        divGrid.Visible = false;
        divAdd.Visible = true;
        txtCustomerCode.Enabled = true;
        clearFields();
        //txtCustomerCode.Text = hf.getCustomerCode();
        ddlBranch.SelectedValue = ddlBranchFilter.SelectedValue;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        clearFields();
        txtCustomerCode.Enabled = true;
        divGrid.Visible = true;
        divAdd.Visible = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!IsPageRefresh)
        {
            if (lblPKIDU.Text == "")
            {
                if (txtCustomerName.Text != "")
                {
                    DistributedTransaction DT = new DistributedTransaction();
                    CEnt = new CUSTOMER();
                    CEnt.CUSTOMER_CODE = txtCustomerCode.Text.ToUpper();
                    CEnt.CUSTOMER_NAME = txtCustomerName.Text.ToUpper();
                    CEnt.VAT_PAN_NUMBER = txtPANNo.Text;
                    CEnt.REG_NUMBER = txtRegNo.Text;
                    CEnt.CONTACT_PERSON = txtContactPerson.Text;
                    CEnt.MOBILE = txtMobileNo.Text;
                    CEnt.PHONE = txtPhoneNo.Text;
                    CEnt.EMAIL = txtEmail.Text;
                    CEnt.STATUS = ddlStatus.SelectedValue;
                    CEnt.ADDRESS = txtAddress.Text;
                    CEnt.COUNTRY = ddlCountry.SelectedValue;
                    CEnt.REMARKS = txtRemarks.Text;
                    CEnt.PAN_VAT = rbtnPAN_VAT.SelectedValue;
                    CEnt.CREDIT_LIMIT = txtCreditLimit.Text;
                    CEnt.JOIN_DATE_BS = txtJoinDate.Text;
                    CEnt.JOIN_DATE_AD = PGD.GetEnglishDateFromNepali(txtJoinDate.Text, "dd/mm/yyyy");
                    CEnt.AREA_ID = ddlArea.SelectedValue;
                    CEnt.AGENT_ID = ddlAgent.SelectedValue;
                    CEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                    string pk_id = CSer.Insert(CEnt, DT).ToString();


                    SLAEnt = new GL_SUB_ACCOUNT();
                    SLAEnt.SUB_GL_CODE = txtCustomerCode.Text.ToUpper();
                    SLAEnt.GL_CODE = "010301"; // GLCode of Customer
                    SLAEnt.SUB_GL_NAME = txtCustomerName.Text;
                    SLAEnt.STATUS = ddlStatus.SelectedValue;
                    SLAEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                    SLASer.Insert(SLAEnt, DT);


                    if (DT.HAPPY == true)
                    {
                        HelperFunction.MsgBox(this, this.GetType(), "Successfully Inserted");
                        divGrid.Visible = true;
                        divAdd.Visible = false;                        
                        DT.Commit();
                    }
                    else
                    {
                        HelperFunction.MsgBox(this, this.GetType(), "Something goes wrong.");
                        divGrid.Visible = false;
                        divAdd.Visible = true;
                        DT.Abort();
                    }
                    DT.Dispose();
                }
                else
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Supplier Name can't be empty.");
                }
            }

            else
            {
                if (txtCustomerName.Text != "")
                {
                    DistributedTransaction DT = new DistributedTransaction();
                    CEnt = new CUSTOMER();
                    CEnt.PK_ID = lblPKIDU.Text;
                    CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
                    if (CEnt != null)
                    {
                        CEnt.CUSTOMER_CODE = txtCustomerCode.Text.ToUpper();
                        CEnt.CUSTOMER_NAME = txtCustomerName.Text.ToUpper();
                        CEnt.VAT_PAN_NUMBER = txtPANNo.Text;
                        CEnt.REG_NUMBER = txtRegNo.Text;
                        CEnt.CONTACT_PERSON = txtContactPerson.Text;
                        CEnt.MOBILE = txtMobileNo.Text;
                        CEnt.PHONE = txtPhoneNo.Text;
                        CEnt.EMAIL = txtEmail.Text;
                        CEnt.STATUS = ddlStatus.SelectedValue;
                        CEnt.ADDRESS = txtAddress.Text;
                        CEnt.COUNTRY = ddlCountry.SelectedValue;
                        CEnt.REMARKS = txtRemarks.Text;
                        CEnt.PAN_VAT = rbtnPAN_VAT.SelectedValue;
                        CEnt.CREDIT_LIMIT = txtCreditLimit.Text;
                        CEnt.JOIN_DATE_BS = txtJoinDate.Text;
                        CEnt.JOIN_DATE_AD = PGD.GetEnglishDateFromNepali(txtJoinDate.Text, "dd/mm/yyyy");
                        CEnt.AREA_ID = ddlArea.SelectedValue;
                        CEnt.AGENT_ID = ddlAgent.SelectedValue;
                        CEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                        CSer.Update(CEnt, DT);

                        SLAEnt = new GL_SUB_ACCOUNT();
                        SLAEnt.SUB_GL_CODE = txtCustomerCode.Text.ToUpper();
                        SLAEnt = (GL_SUB_ACCOUNT)SLASer.GetSingle(SLAEnt);
                        if (SLAEnt != null)
                        {
                            SLAEnt.GL_CODE = "010301"; // GLCode of Customer
                            SLAEnt.SUB_GL_NAME = txtCustomerName.Text.ToUpper();
                            SLAEnt.STATUS = ddlStatus.SelectedValue;
                            SLAEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                            SLASer.Update(SLAEnt, DT);
                        }
                        else
                        {
                            SLAEnt = new GL_SUB_ACCOUNT();
                            SLAEnt.SUB_GL_CODE = txtCustomerCode.Text.ToUpper();
                            SLAEnt.GL_CODE = "010301"; // GLCode of Customer
                            SLAEnt.SUB_GL_NAME = txtCustomerName.Text;
                            SLAEnt.STATUS = ddlStatus.SelectedValue;
                            SLAEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                            SLASer.Insert(SLAEnt, DT);
                        }
                        if (DT.HAPPY == true)
                        {
                            HelperFunction.MsgBox(this, this.GetType(), "Successfully Inserted");
                            divGrid.Visible = true;
                            divAdd.Visible = false;
                            DT.Commit();
                        }
                        else
                        {
                            HelperFunction.MsgBox(this, this.GetType(), "Something goes wrong.");
                            divGrid.Visible = false;
                            divAdd.Visible = true;
                            DT.Abort();
                        }
                        DT.Dispose();
                    }
                    else
                    {
                        HelperFunction.MsgBox(this, this.GetType(), "Supplier Name can't be empty.");
                    }
                }
            }
        }
        else
        {
            divAdd.Visible = false;
        }
        ddlBranchFilter.SelectedValue = ddlBranch.SelectedValue;
        LoadGrid(ddlBranch.SelectedValue);

    }
    protected void gridCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblStat = e.Row.FindControl("lblStat") as Label;
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lblShowBranch = e.Row.FindControl("lblShowBranch") as Label;
            Label lblBranch = e.Row.FindControl("lblBranch") as Label;
            Label lblCustomerID = e.Row.FindControl("lblCustomerID") as Label;

            if (lblStatus.Text != "" && lblStatus.Text != "Select")
            {
                if (lblStatus.Text == "1")
                {
                    lblStat.Text = "Available";
                }
                else
                {
                    lblStat.Text = "Unavailable";
                }
            }

            OEnt = new OFFICE();
            OEnt.PK_ID = lblBranch.Text;
            OEnt = (OFFICE)OSer.GetSingle(OEnt);
            if(OEnt != null)
            {
                lblShowBranch.Text = ED.Decrypt(OEnt.OFFICENAME) + " - " + OEnt.STREET;
            }

        }

    }
    protected void gridCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Change"))
        {

            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblCustomerID = gr.FindControl("lblCustomerID") as Label;

            CEnt = new CUSTOMER();
            CEnt.PK_ID = lblCustomerID.Text;
            CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
            if (CEnt != null)
            {
                lblPKIDU.Text = CEnt.PK_ID;
                txtCustomerCode.Text = CEnt.CUSTOMER_CODE;
                txtCustomerName.Text = CEnt.CUSTOMER_NAME;
                txtPANNo.Text = CEnt.VAT_PAN_NUMBER;
                txtRegNo.Text = CEnt.REG_NUMBER;
                txtContactPerson.Text = CEnt.CONTACT_PERSON;
                txtPhoneNo.Text = CEnt.PHONE;
                txtMobileNo.Text = CEnt.MOBILE;
                txtEmail.Text = CEnt.EMAIL;
                txtAddress.Text = CEnt.ADDRESS;
                ddlCountry.SelectedValue = CEnt.COUNTRY;
                txtRemarks.Text = CEnt.REMARKS;
                ddlStatus.SelectedValue = CEnt.STATUS;
                rbtnPAN_VAT.SelectedValue = CEnt.PAN_VAT;
                txtJoinDate.Text = CEnt.JOIN_DATE_BS;
                ddlArea.SelectedValue = CEnt.AREA_ID;
                LoadAgent(); // ensure ddlAgent is populated
                if (ddlAgent.Items.FindByValue(CEnt.AGENT_ID) != null)
                {
                    ddlAgent.SelectedValue = CEnt.AGENT_ID;
                }
                ddlBranch.SelectedValue = CEnt.OFFICE_CODE;
            }

            divGrid.Visible = false;
            divAdd.Visible = true;
            txtCustomerCode.Enabled = false;
        }
    }

    protected void gridCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LoadGrid(ddlBranchFilter.SelectedValue);
        gridCustomer.PageIndex = e.NewPageIndex;
        gridCustomer.DataBind();
    }

    protected void txtCustomerCode_TextChanged(object sender, EventArgs e)
    {
        SLAEnt = new GL_SUB_ACCOUNT();
        SLAEnt.SUB_GL_CODE = txtCustomerCode.Text.ToUpper();
        SLAEnt = (GL_SUB_ACCOUNT)SLASer.GetSingle(SLAEnt);
        if (SLAEnt != null || txtCustomerCode.Text == "")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Supplier code is already taken.");
            txtCustomerCode.Text = "";
            txtCustomerCode.Focus();
        }
        else
        {
            txtCustomerCode.Text = txtCustomerCode.Text.ToUpper();
            txtCustomerName.Focus();
        }
    }

    protected void txtJoinDate_TextChanged(object sender, EventArgs e)
    {

        if (PGD.GetEnglishDateFromNepali(txtJoinDate.Text, "dd/mm/yyyy") == "")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Invalid Date format. use DD/MM/YYYY format");
            txtJoinDate.Text = "";
            txtJoinDate.Focus();
        }
    }


    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCustomer.SelectedValue != "Select")
        {
            CEnt = new CUSTOMER();
            CEnt.PK_ID = ddlCustomer.SelectedValue;
            CEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
            CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
            if (CEnt != null)
            {
                txtSearchCustomerCode.Text = CEnt.CUSTOMER_CODE;
            }
        }
        else
        {
            txtSearchCustomerCode.Text = "";
        }
    }

    protected void txtSearchCustomerCode_TextChanged(object sender, EventArgs e)
    {
        txtSearchCustomerCode.Text = txtSearchCustomerCode.Text.ToUpper();
        CEnt = new CUSTOMER();
        CEnt.CUSTOMER_CODE = txtSearchCustomerCode.Text;
        CEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
        CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
        if (CEnt != null && !string.IsNullOrEmpty(txtSearchCustomerCode.Text))
        {
            ddlCustomer.SelectedValue = CEnt.PK_ID;
        }
        else
        {
            ddlCustomer.SelectedValue = "Select";
            txtSearchCustomerCode.Text = "";
            txtSearchCustomerCode.Focus();
            HelperFunction.MsgBox(this, this.GetType(), "Not a Valid Customer Code.");
        }
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        CEnt = new CUSTOMER();
        if (txtSearchCustomerCode.Text != "")
            CEnt.CUSTOMER_CODE = txtSearchCustomerCode.Text;
        CEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
        gridCustomer.DataSource = CSer.GetAll(CEnt);
        gridCustomer.DataBind();

    }


    protected void ddlBranchFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid(ddlBranchFilter.SelectedValue);
        LoadCustomers();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAgent();
        LoadArea();
    }
}