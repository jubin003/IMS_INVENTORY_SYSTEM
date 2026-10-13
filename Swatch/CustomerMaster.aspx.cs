using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using DataHelper.Framework;
using PhyeGanCore;
using System.Web;
using Entity.Framework;

public partial class CustomerMaster : System.Web.UI.Page
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

    PR_CUSTOMER_LOCATION CLocEnt = new PR_CUSTOMER_LOCATION();
    PR_CUSTOMER_LOCATIONService CLocSer = new PR_CUSTOMER_LOCATIONService();

    PhyeGanProductSetting PPS = new PhyeGanProductSetting();
    PhyeGanDate PGD = new PhyeGanDate();
    Boolean IsPageRefresh = false;
    PhyeGan PG = new PhyeGan();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfileEnt = new UserProfileEntity();

    static string path = "";

    private DataTable TempLocations
    {
        get
        {
            if (ViewState["TempLocations"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PK_ID", typeof(string));
                dt.Columns.Add("ADDRESS", typeof(string));
                dt.Columns.Add("CONTACT_PERSON", typeof(string));
                dt.Columns.Add("CONTACT_NUMBER", typeof(string));
                dt.Columns.Add("EMAIL_ID", typeof(string));
                dt.Columns.Add("COUNTRY_ID", typeof(string));
                ViewState["TempLocations"] = dt;
            }
            return (DataTable)ViewState["TempLocations"];
        }
        set
        {
            ViewState["TempLocations"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["postids"] = System.Guid.NewGuid().ToString();
            Session["postid"] = ViewState["postids"].ToString();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
            path = path.Replace(PG.Org_Base_URL(), "");

            loadBranch();
            LoadGrid(ddlBranchFilter.SelectedValue);
            LoadArea();
            LoadAgent();
            LoadCountry();
            LoadCustomerDropdownForLocations();
        }
    }

    #region Outer Tabs Switching

    protected void btnOuterTabCustomers_Click(object sender, EventArgs e)
    {
        pnlCustomerSection.Visible = true;
        pnlDeliverySection.Visible = false;
        btnOuterTabCustomers.CssClass = "outer-nav-tab active";
        btnOuterTabLocations.CssClass = "outer-nav-tab";

        // Reset search filter and reload customer list
        txtSearchCustomerCode.Text = "";
        txtSearchCustomerName.Text = "";
        LoadGrid(ddlBranchFilter.SelectedValue);
    }

    protected void btnOuterTabLocations_Click(object sender, EventArgs e)
    {
        pnlCustomerSection.Visible = false;
        pnlDeliverySection.Visible = true;
        btnOuterTabCustomers.CssClass = "outer-nav-tab";
        btnOuterTabLocations.CssClass = "outer-nav-tab active";

        LoadCustomerDropdownForLocations();
    }

    #endregion

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
        var countries = CYSer.GetAll(CYEnt);

        ddlCountry.DataSource = countries;
        ddlCountry.DataTextField = "COUNTRY_NAME";
        ddlCountry.DataValueField = "PK_ID";
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, "");

        ddlDelCountry.DataSource = countries;
        ddlDelCountry.DataTextField = "COUNTRY_NAME";
        ddlDelCountry.DataValueField = "PK_ID";
        ddlDelCountry.DataBind();
        ddlDelCountry.Items.Insert(0, "");

        ddlOuterDelCountry.DataSource = countries;
        ddlOuterDelCountry.DataTextField = "COUNTRY_NAME";
        ddlOuterDelCountry.DataValueField = "PK_ID";
        ddlOuterDelCountry.DataBind();
        ddlOuterDelCountry.Items.Insert(0, "");
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

    public string GetCustomerImageUrl(object pkId)
    {
        if (pkId != null && !string.IsNullOrEmpty(pkId.ToString()))
        {
            string fileName = pkId.ToString().Trim() + ".jpg";
            string physicalPath = Server.MapPath("~/images/Customer/" + fileName);
            if (File.Exists(physicalPath))
            {
                return "~/images/Customer/" + fileName;
            }
        }
        return "~/images/icons/user.png";
    }

    private void UploadCustomerImageById(string customerId)
    {
        if (fuCustomerPhoto.HasFile)
        {
            string fileExtension = Path.GetExtension(fuCustomerPhoto.FileName).ToLower();
            string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (Array.IndexOf(allowedExtensions, fileExtension) >= 0)
            {
                string folderPath = Server.MapPath("~/images/Customer/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, customerId.Trim() + ".jpg");

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                fuCustomerPhoto.SaveAs(filePath);
            }
            else
            {
                HelperFunction.MsgBox(this, this.GetType(), "Only .jpg, .jpeg, or .png images are allowed.");
            }
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
        txtCreditLimit.Text = "";
        txtJoinDate.Text = "";
        ddlStatus.SelectedIndex = 0;
        ddlCountry.SelectedIndex = 0;
        if (ddlArea.Items.Count > 0) ddlArea.SelectedIndex = 0;
        if (ddlAgent.Items.Count > 0) ddlAgent.SelectedIndex = 0;

        imgCustomerPreview.ImageUrl = "~/images/icons/user.png";

        txtDelAddress.Text = "";
        txtDelContactPerson.Text = "";
        txtDelContactNo.Text = "";
        txtDelEmail.Text = "";
        if (ddlDelCountry.Items.Count > 0) ddlDelCountry.SelectedIndex = 0;
    }

    protected void btnAddMore_Click(object sender, EventArgs e)
    {
        divGrid.Visible = false;
        divAdd.Visible = true;
        txtCustomerCode.Enabled = true;
        clearFields();
        ddlBranch.SelectedValue = ddlBranchFilter.SelectedValue;

        hdnActiveTab.Value = "general";
        secDeliveryLocations.Visible = true;
        TempLocations = null;
        gridCustomerLocations.DataSource = TempLocations;
        gridCustomerLocations.DataBind();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        clearFields();
        txtCustomerCode.Enabled = true;
        divGrid.Visible = true;
        divAdd.Visible = false;
        secDeliveryLocations.Visible = false;

        // Reset search filter and reload customer list
        txtSearchCustomerCode.Text = "";
        txtSearchCustomerName.Text = "";
        LoadGrid(ddlBranchFilter.SelectedValue);
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

                    if (fuCustomerPhoto.HasFile)
                    {
                        UploadCustomerImageById(pk_id);
                    }

                    SLAEnt = new GL_SUB_ACCOUNT();
                    SLAEnt.SUB_GL_CODE = txtCustomerCode.Text.ToUpper();
                    SLAEnt.GL_CODE = "010301";
                    SLAEnt.SUB_GL_NAME = txtCustomerName.Text;
                    SLAEnt.STATUS = ddlStatus.SelectedValue;
                    SLAEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                    SLASer.Insert(SLAEnt, DT);

                    if (TempLocations != null && TempLocations.Rows.Count > 0)
                    {
                        foreach (DataRow row in TempLocations.Rows)
                        {
                            PR_CUSTOMER_LOCATION loc = new PR_CUSTOMER_LOCATION();
                            loc.CUSTOMER_ID = pk_id;
                            loc.ADDRESS = row["ADDRESS"].ToString();
                            loc.CONTACT_PERSON = row["CONTACT_PERSON"].ToString();
                            loc.CONTACT_NUMBER = row["CONTACT_NUMBER"].ToString();
                            loc.EMAIL_ID = row["EMAIL_ID"].ToString();
                            loc.COUNTRY_ID = row["COUNTRY_ID"].ToString();
                            CLocSer.Insert(loc, DT);
                        }
                    }

                    if (DT.HAPPY == true)
                    {
                        HelperFunction.MsgBox(this, this.GetType(), "Successfully Inserted");
                        divGrid.Visible = true;
                        divAdd.Visible = false;
                        secDeliveryLocations.Visible = false;
                        DT.Commit();
                        LoadCustomerDropdownForLocations();
                    }
                    else
                    {
                        HelperFunction.MsgBox(this, this.GetType(), "Something went wrong.");
                        divGrid.Visible = false;
                        divAdd.Visible = true;
                        DT.Abort();
                    }
                    DT.Dispose();
                }
                else
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Customer Name cannot be empty.");
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

                        if (fuCustomerPhoto.HasFile)
                        {
                            UploadCustomerImageById(lblPKIDU.Text);
                        }

                        SLAEnt = new GL_SUB_ACCOUNT();
                        SLAEnt.SUB_GL_CODE = txtCustomerCode.Text.ToUpper();
                        SLAEnt = (GL_SUB_ACCOUNT)SLASer.GetSingle(SLAEnt);
                        if (SLAEnt != null)
                        {
                            SLAEnt.GL_CODE = "010301";
                            SLAEnt.SUB_GL_NAME = txtCustomerName.Text.ToUpper();
                            SLAEnt.STATUS = ddlStatus.SelectedValue;
                            SLAEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                            SLASer.Update(SLAEnt, DT);
                        }
                        else
                        {
                            SLAEnt = new GL_SUB_ACCOUNT();
                            SLAEnt.SUB_GL_CODE = txtCustomerCode.Text.ToUpper();
                            SLAEnt.GL_CODE = "010301";
                            SLAEnt.SUB_GL_NAME = txtCustomerName.Text;
                            SLAEnt.STATUS = ddlStatus.SelectedValue;
                            SLAEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                            SLASer.Insert(SLAEnt, DT);
                        }

                        if (DT.HAPPY == true)
                        {
                            HelperFunction.MsgBox(this, this.GetType(), "Successfully Updated");
                            divGrid.Visible = true;
                            divAdd.Visible = false;
                            secDeliveryLocations.Visible = false;
                            DT.Commit();
                            LoadCustomerDropdownForLocations();
                        }
                        else
                        {
                            HelperFunction.MsgBox(this, this.GetType(), "Something went wrong.");
                            divGrid.Visible = false;
                            divAdd.Visible = true;
                            DT.Abort();
                        }
                        DT.Dispose();
                    }
                    else
                    {
                        HelperFunction.MsgBox(this, this.GetType(), "Customer record not found.");
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

            if (lblStatus != null && !string.IsNullOrEmpty(lblStatus.Text) && lblStatus.Text != "Select")
            {
                if (lblStatus.Text == "1")
                {
                    lblStat.Text = "Available";
                    lblStat.CssClass = "status-pill status-active";
                }
                else
                {
                    lblStat.Text = "Unavailable";
                    lblStat.CssClass = "status-pill status-inactive";
                }
            }

            if (lblBranch != null && !string.IsNullOrEmpty(lblBranch.Text))
            {
                OEnt = new OFFICE();
                OEnt.PK_ID = lblBranch.Text;
                OEnt = (OFFICE)OSer.GetSingle(OEnt);
                if (OEnt != null && lblShowBranch != null)
                {
                    lblShowBranch.Text = ED.Decrypt(OEnt.OFFICENAME) + " - " + OEnt.STREET;
                }
            }
        }
    }

    protected void gridCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Change"))
        {
            GridViewRow gr = ((Control)e.CommandSource).NamingContainer as GridViewRow;
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

                if (!string.IsNullOrEmpty(CEnt.PAN_VAT))
                {
                    string panVatValue = CEnt.PAN_VAT.Trim().ToUpper();
                    if (rbtnPAN_VAT.Items.FindByValue(panVatValue) != null)
                    {
                        rbtnPAN_VAT.SelectedValue = panVatValue;
                    }
                    else
                    {
                        rbtnPAN_VAT.SelectedIndex = 0;
                    }
                }
                else
                {
                    rbtnPAN_VAT.SelectedIndex = 0;
                }

                txtCreditLimit.Text = CEnt.CREDIT_LIMIT;
                txtJoinDate.Text = CEnt.JOIN_DATE_BS;
                ddlArea.SelectedValue = CEnt.AREA_ID;
                LoadAgent();
                if (ddlAgent.Items.FindByValue(CEnt.AGENT_ID) != null)
                {
                    ddlAgent.SelectedValue = CEnt.AGENT_ID;
                }
                ddlBranch.SelectedValue = CEnt.OFFICE_CODE;

                string imageFile = CEnt.PK_ID.Trim() + ".jpg";
                string physicalPath = Server.MapPath("~/images/Customer/" + imageFile);
                if (File.Exists(physicalPath))
                {
                    imgCustomerPreview.ImageUrl = "~/images/Customer/" + imageFile + "?t=" + DateTime.Now.Ticks;
                }
                else
                {
                    imgCustomerPreview.ImageUrl = "~/images/icons/user.png";
                }

                LoadCustomerLocations(CEnt.PK_ID);
                hdnActiveTab.Value = "general";
            }

            divGrid.Visible = false;
            divAdd.Visible = true;
            txtCustomerCode.Enabled = false;
        }
    }

    protected void gridCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        btnSearch_Click(sender, e);
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
            HelperFunction.MsgBox(this, this.GetType(), "Customer code is already taken.");
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
            HelperFunction.MsgBox(this, this.GetType(), "Invalid Date format. Use DD/MM/YYYY format.");
            txtJoinDate.Text = "";
            txtJoinDate.Focus();
        }
    }

    protected void txtSearchCustomerCode_TextChanged(object sender, EventArgs e)
    {
        txtSearchCustomerCode.Text = txtSearchCustomerCode.Text.ToUpper().Trim();
        btnSearch_Click(sender, e);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        CEnt = new CUSTOMER();
        CEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;

        if (!string.IsNullOrEmpty(txtSearchCustomerCode.Text.Trim()))
            CEnt.CUSTOMER_CODE = txtSearchCustomerCode.Text.Trim().ToUpper();

        if (!string.IsNullOrEmpty(txtSearchCustomerName.Text.Trim()))
            CEnt.CUSTOMER_NAME = txtSearchCustomerName.Text.Trim().ToUpper();

        gridCustomer.DataSource = CSer.GetAll(CEnt);
        gridCustomer.DataBind();
    }

    protected void btnSearchClear_Click(object sender, EventArgs e)
    {
        txtSearchCustomerCode.Text = "";
        txtSearchCustomerName.Text = "";
        LoadGrid(ddlBranchFilter.SelectedValue);
    }

    protected void ddlBranchFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnSearch_Click(sender, e);
        LoadCustomerDropdownForLocations();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAgent();
        LoadArea();
    }

    #region Inner Delivery Locations (Customer Add/Edit Wizard)

    protected void LoadCustomerLocations(string customerId)
    {
        if (!string.IsNullOrEmpty(customerId))
        {
            CLocEnt = new PR_CUSTOMER_LOCATION();
            CLocEnt.CUSTOMER_ID = customerId;
            gridCustomerLocations.DataSource = CLocSer.GetAll(CLocEnt);
            gridCustomerLocations.DataBind();
            secDeliveryLocations.Visible = true;
        }
        else
        {
            gridCustomerLocations.DataSource = null;
            gridCustomerLocations.DataBind();
            secDeliveryLocations.Visible = false;
        }
    }

    protected void btnAddLocation_Click(object sender, EventArgs e)
    {
        hdnActiveTab.Value = "delivery";

        if (string.IsNullOrEmpty(txtDelAddress.Text.Trim()))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please enter the delivery address.");
            txtDelAddress.Focus();
            return;
        }

        if (string.IsNullOrEmpty(lblPKIDU.Text))
        {
            DataTable dt = TempLocations;
            DataRow row = dt.NewRow();
            row["PK_ID"] = Guid.NewGuid().ToString();
            row["ADDRESS"] = txtDelAddress.Text.Trim();
            row["CONTACT_PERSON"] = txtDelContactPerson.Text.Trim();
            row["CONTACT_NUMBER"] = txtDelContactNo.Text.Trim();
            row["EMAIL_ID"] = txtDelEmail.Text.Trim();
            row["COUNTRY_ID"] = ddlDelCountry.SelectedValue;
            dt.Rows.Add(row);

            TempLocations = dt;
            gridCustomerLocations.DataSource = TempLocations;
            gridCustomerLocations.DataBind();
        }
        else
        {
            CLocEnt = new PR_CUSTOMER_LOCATION();
            CLocEnt.CUSTOMER_ID = lblPKIDU.Text;
            CLocEnt.ADDRESS = txtDelAddress.Text.Trim();
            CLocEnt.CONTACT_PERSON = txtDelContactPerson.Text.Trim();
            CLocEnt.CONTACT_NUMBER = txtDelContactNo.Text.Trim();
            CLocEnt.EMAIL_ID = txtDelEmail.Text.Trim();
            CLocEnt.COUNTRY_ID = ddlDelCountry.SelectedValue;
            CLocSer.Insert(CLocEnt);

            LoadCustomerLocations(lblPKIDU.Text);
        }

        txtDelAddress.Text = "";
        txtDelContactPerson.Text = "";
        txtDelContactNo.Text = "";
        txtDelEmail.Text = "";
        if (ddlDelCountry.Items.Count > 0) ddlDelCountry.SelectedIndex = 0;
    }

    protected void gridCustomerLocations_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("RemoveLoc"))
        {
            hdnActiveTab.Value = "delivery";
            string locId = e.CommandArgument.ToString();

            if (string.IsNullOrEmpty(lblPKIDU.Text))
            {
                DataTable dt = TempLocations;
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    if (dt.Rows[i]["PK_ID"].ToString() == locId)
                    {
                        dt.Rows.RemoveAt(i);
                        break;
                    }
                }
                TempLocations = dt;
                gridCustomerLocations.DataSource = TempLocations;
                gridCustomerLocations.DataBind();
            }
            else
            {
                CLocEnt = new PR_CUSTOMER_LOCATION();
                CLocEnt.PK_ID = locId;
                CLocSer.Delete(CLocEnt);
                LoadCustomerLocations(lblPKIDU.Text);
            }
        }
    }

    protected void gridCustomerLocations_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblCountryName = e.Row.FindControl("lblCountryName") as Label;
            if (lblCountryName != null && !string.IsNullOrEmpty(lblCountryName.Text))
            {
                CYEnt = new COUNTRY();
                CYEnt.PK_ID = lblCountryName.Text;
                CYEnt = (COUNTRY)CYSer.GetSingle(CYEnt);
                if (CYEnt != null)
                {
                    lblCountryName.Text = CYEnt.COUNTRY_NAME;
                }
            }
        }
    }

    #endregion

    #region Dedicated Outer Delivery Locations Tab Handling

    protected void LoadCustomerDropdownForLocations()
    {
        CEnt = new CUSTOMER();
        CEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
        var custList = CSer.GetAll(CEnt);

        ddlSelectCustomerForLoc.DataSource = custList;
        ddlSelectCustomerForLoc.DataTextField = "CUSTOMER_NAME";
        ddlSelectCustomerForLoc.DataValueField = "PK_ID";
        ddlSelectCustomerForLoc.DataBind();
        ddlSelectCustomerForLoc.Items.Insert(0, new ListItem("-- Select Customer --", ""));
    }

    protected void ddlSelectCustomerForLoc_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlSelectCustomerForLoc.SelectedValue))
        {
            CEnt = new CUSTOMER();
            CEnt.PK_ID = ddlSelectCustomerForLoc.SelectedValue;
            CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
            if (CEnt != null)
            {
                lblOuterCustName.Text = CEnt.CUSTOMER_NAME;
                lblOuterCustCode.Text = CEnt.CUSTOMER_CODE;
                lblOuterCustAddress.Text = string.IsNullOrEmpty(CEnt.ADDRESS) ? "No address specified" : CEnt.ADDRESS;
                lblOuterCustPhone.Text = string.IsNullOrEmpty(CEnt.MOBILE) ? CEnt.PHONE : CEnt.MOBILE;

                pnlOuterCustomerInfo.Visible = true;
                ClearOuterDeliveryLocationForm();
                LoadOuterCustomerLocations(CEnt.PK_ID);
            }
        }
        else
        {
            pnlOuterCustomerInfo.Visible = false;
        }
    }

    protected void LoadOuterCustomerLocations(string customerId)
    {
        if (!string.IsNullOrEmpty(customerId))
        {
            CLocEnt = new PR_CUSTOMER_LOCATION();
            CLocEnt.CUSTOMER_ID = customerId;
            gridOuterCustomerLocations.DataSource = CLocSer.GetAll(CLocEnt);
            gridOuterCustomerLocations.DataBind();
        }
    }

    protected void btnSaveOuterDelLoc_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlSelectCustomerForLoc.SelectedValue))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please select a customer first.");
            return;
        }

        if (string.IsNullOrEmpty(txtOuterDelAddress.Text.Trim()))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please enter the delivery address.");
            txtOuterDelAddress.Focus();
            return;
        }

        if (string.IsNullOrEmpty(hdnOuterLocPKID.Value))
        {
            CLocEnt = new PR_CUSTOMER_LOCATION();
            CLocEnt.CUSTOMER_ID = ddlSelectCustomerForLoc.SelectedValue;
            CLocEnt.ADDRESS = txtOuterDelAddress.Text.Trim();
            CLocEnt.CONTACT_PERSON = txtOuterDelContactPerson.Text.Trim();
            CLocEnt.CONTACT_NUMBER = txtOuterDelContactNo.Text.Trim();
            CLocEnt.EMAIL_ID = txtOuterDelEmail.Text.Trim();
            CLocEnt.COUNTRY_ID = ddlOuterDelCountry.SelectedValue;
            CLocSer.Insert(CLocEnt);
            HelperFunction.MsgBox(this, this.GetType(), "Delivery location added successfully.");
        }
        else
        {
            CLocEnt = new PR_CUSTOMER_LOCATION();
            CLocEnt.PK_ID = hdnOuterLocPKID.Value;
            CLocEnt = (PR_CUSTOMER_LOCATION)CLocSer.GetSingle(CLocEnt);
            if (CLocEnt != null)
            {
                CLocEnt.ADDRESS = txtOuterDelAddress.Text.Trim();
                CLocEnt.CONTACT_PERSON = txtOuterDelContactPerson.Text.Trim();
                CLocEnt.CONTACT_NUMBER = txtOuterDelContactNo.Text.Trim();
                CLocEnt.EMAIL_ID = txtOuterDelEmail.Text.Trim();
                CLocEnt.COUNTRY_ID = ddlOuterDelCountry.SelectedValue;
                CLocSer.Update(CLocEnt);
                HelperFunction.MsgBox(this, this.GetType(), "Delivery location updated successfully.");
            }
        }

        LoadOuterCustomerLocations(ddlSelectCustomerForLoc.SelectedValue);
        ClearOuterDeliveryLocationForm();
    }

    protected void btnCancelOuterLocEdit_Click(object sender, EventArgs e)
    {
        ClearOuterDeliveryLocationForm();
    }

    private void ClearOuterDeliveryLocationForm()
    {
        hdnOuterLocPKID.Value = "";
        txtOuterDelAddress.Text = "";
        txtOuterDelContactPerson.Text = "";
        txtOuterDelContactNo.Text = "";
        txtOuterDelEmail.Text = "";
        if (ddlOuterDelCountry.Items.Count > 0) ddlOuterDelCountry.SelectedIndex = 0;
        btnSaveOuterDelLoc.Text = "+ Add Location";
        lblOuterLocFormTitle.Text = "Add New Delivery Location";
        btnCancelOuterLocEdit.Visible = false;
    }

    protected void gridOuterCustomerLocations_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("EditOuterLoc"))
        {
            string locId = e.CommandArgument.ToString();
            CLocEnt = new PR_CUSTOMER_LOCATION();
            CLocEnt.PK_ID = locId;
            CLocEnt = (PR_CUSTOMER_LOCATION)CLocSer.GetSingle(CLocEnt);
            if (CLocEnt != null)
            {
                hdnOuterLocPKID.Value = CLocEnt.PK_ID;
                txtOuterDelAddress.Text = CLocEnt.ADDRESS;
                txtOuterDelContactPerson.Text = CLocEnt.CONTACT_PERSON;
                txtOuterDelContactNo.Text = CLocEnt.CONTACT_NUMBER;
                txtOuterDelEmail.Text = CLocEnt.EMAIL_ID;
                if (ddlOuterDelCountry.Items.FindByValue(CLocEnt.COUNTRY_ID) != null)
                {
                    ddlOuterDelCountry.SelectedValue = CLocEnt.COUNTRY_ID;
                }

                btnSaveOuterDelLoc.Text = "Update Location";
                lblOuterLocFormTitle.Text = "Edit Delivery Location";
                btnCancelOuterLocEdit.Visible = true;
            }
        }
        else if (e.CommandName.Equals("DeleteOuterLoc"))
        {
            string locId = e.CommandArgument.ToString();
            CLocEnt = new PR_CUSTOMER_LOCATION();
            CLocEnt.PK_ID = locId;
            CLocSer.Delete(CLocEnt);
            HelperFunction.MsgBox(this, this.GetType(), "Delivery location deleted successfully.");
            LoadOuterCustomerLocations(ddlSelectCustomerForLoc.SelectedValue);
            ClearOuterDeliveryLocationForm();
        }
    }

    protected void gridOuterCustomerLocations_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblCountry = e.Row.FindControl("lblOuterGridCountryName") as Label;
            if (lblCountry != null && !string.IsNullOrEmpty(lblCountry.Text))
            {
                CYEnt = new COUNTRY();
                CYEnt.PK_ID = lblCountry.Text;
                CYEnt = (COUNTRY)CYSer.GetSingle(CYEnt);
                if (CYEnt != null)
                {
                    lblCountry.Text = CYEnt.COUNTRY_NAME;
                }
            }
        }
    }

    #endregion
}