using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using DataHelper.Framework;
using PhyeGanCore;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

public partial class administration_supplier : System.Web.UI.Page
{
    GL_SUB_ACCOUNT SLAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService SLASer = new GL_SUB_ACCOUNTService();

    SUPPLIERS SUPEnt = new SUPPLIERS();
    SUPPLIERSService SUPSer = new SUPPLIERSService();

    SUPPLIER_TYPE STEnt = new SUPPLIER_TYPE();
    SUPPLIER_TYPEService STSer = new SUPPLIER_TYPEService();

    COUNTRY CYEnt = new COUNTRY();
    COUNTRYService CYSer = new COUNTRYService();


    AREA AEnt = new AREA();
    AREAService ASer = new AREAService();
    AGENT AGEnt = new AGENT();
    AGENTService AGSer = new AGENTService();
    PhyeGanDate PGD = new PhyeGanDate();
    Boolean IsPageRefresh = false;
    PhyeGan PG = new PhyeGan();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfile = new UserProfileEntity();
    static string path = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                ViewState["postids"] = System.Guid.NewGuid().ToString();
                Session["postid"] = ViewState["postids"].ToString();
                userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
                if (hf.checkPageAccess(path, userProfile.UserGroupID.ToString()))
                {
                    LoadSupplierType();
                    LoadSupplierTypeList();
                    LoadCountry();
                }
                else
                {
                    Response.Redirect("~/forbidden.aspx");
                }

            }
            catch (Exception ww)
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }

    protected void LoadSupplierType()
    {
        STEnt = new SUPPLIER_TYPE();
        STEnt.STATUS = "1";
        ddlSupplierType.DataSource = STSer.GetAll(STEnt);
        ddlSupplierType.DataTextField = "SUPPLIERS_TYPE";
        ddlSupplierType.DataValueField = "PK_ID";
        ddlSupplierType.DataBind();
        ddlSupplierType.Items.Insert(0, "Select");
    }



    protected void LoadSupplierTypeList()
    {
        STEnt = new SUPPLIER_TYPE();
        STEnt.STATUS = "1";
        ddlSupplierTypeList.DataSource = STSer.GetAll(STEnt);
        ddlSupplierTypeList.DataTextField = "SUPPLIERS_TYPE";
        ddlSupplierTypeList.DataValueField = "PK_ID";
        ddlSupplierTypeList.DataBind();
        ddlSupplierTypeList.Items.Insert(0, "Select");
    }

    protected void LoadCountry()
    {
        CYEnt = new COUNTRY();
        ddlCountry.DataSource = CYSer.GetAll(CYEnt);
        ddlCountry.DataTextField = "COUNTRY_NAME";
        ddlCountry.DataValueField = "PK_ID";
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, "Select");

    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        LoadGrid(ddlSupplierTypeList.SelectedValue);
    }
    protected void ddlSupplierType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSupplierCode(ddlSupplierType.SelectedValue);
    }

    protected void LoadSupplierCode(string supplierType)
    {
        if (supplierType != "Select")
        {
            STEnt = new SUPPLIER_TYPE();
            STEnt.PK_ID = ddlSupplierType.SelectedValue;
            STEnt = (SUPPLIER_TYPE)STSer.GetSingle(STEnt);
            if (STEnt != null)
            {
                //txtSupplierCode.Text = STEnt.PREFIX + hf.getSupplierCode(ddlSupplierType.SelectedValue);
            }
        }
        else
        {
            txtSupplierCode.Text = "";
        }
    }

    protected void LoadGrid(string suppliertype)
    {
        SUPEnt = new SUPPLIERS();
        SUPEnt.SUPPLIER_TYPE = suppliertype;
        gridSupplier.DataSource = SUPSer.GetAll(SUPEnt);
        gridSupplier.DataBind();
    }
    protected void clearFields()
    {
        lblPKIDU.Text = "";
        txtSupplierCode.Text = "";
        txtSupplierName.Text = "";
        txtAddress.Text = "";
        txtContactPerson.Text = "";
        txtEmail.Text = "";
        txtMobileNo.Text = "";
        txtPANNo.Text = "";
        txtPhoneNo.Text = "";
        txtRegNo.Text = "";
        txtRemarks.Text = "";
        ddlStatus.SelectedIndex = 0;
        //ddlCountry.SelectedIndex = 0;
    }


    protected void btnAddMore_Click(object sender, EventArgs e)
    {
        divGrid.Visible = false;
        divAdd.Visible = true;
        clearFields();
        txtSupplierCode.Enabled = true;
        ddlSupplierType.SelectedValue = ddlSupplierTypeList.SelectedValue;
        LoadSupplierCode(ddlSupplierType.SelectedValue);
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        clearFields();
        divGrid.Visible = true;
        divAdd.Visible = false;
        txtSupplierCode.Enabled = true;
    }

    protected void checkPan()
    {
        SUPEnt = new SUPPLIERS();
        SUPEnt.VAT_PAN_NUMBER = txtPANNo.Text;
        SUPEnt = (SUPPLIERS)SUPSer.GetSingle(SUPEnt);
        if (SUPEnt != null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "PAN/VAT already exits");
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!IsPageRefresh)
        {
            if (lblPKIDU.Text == "")
            {
                if (txtSupplierName.Text != "" && txtSupplierCode.Text != "")
                {
                    DistributedTransaction DT = new DistributedTransaction();
                    SUPEnt = new SUPPLIERS();
                    SUPEnt.SUPPLIER_TYPE = ddlSupplierType.SelectedValue;
                    SUPEnt.SUPPLIER_CODE = txtSupplierCode.Text.ToUpper();
                    SUPEnt.SUPPLIER_NAME = txtSupplierName.Text.ToUpper();
                    SUPEnt.VAT_PAN_NUMBER = txtPANNo.Text;
                    SUPEnt.REG_NUMBER = txtRegNo.Text;
                    SUPEnt.CONTACT_PERSON = txtContactPerson.Text;
                    SUPEnt.MOBILE = txtMobileNo.Text;
                    SUPEnt.PHONE = txtPhoneNo.Text;
                    SUPEnt.EMAIL = txtEmail.Text;
                    SUPEnt.STATUS = ddlStatus.SelectedValue;
                    SUPEnt.ADDRESS = txtAddress.Text;
                    SUPEnt.REMARKS = txtRemarks.Text;
                    SUPEnt.COUNTRY = ddlCountry.SelectedValue;
                    string pk_id = SUPSer.Insert(SUPEnt, DT).ToString();

                    SLAEnt = new GL_SUB_ACCOUNT();
                    SLAEnt.SUB_GL_CODE = txtSupplierCode.Text.ToUpper();
                    if (ddlSupplierType.SelectedValue == "1")
                        SLAEnt.GL_CODE = "040301"; // GLCode of Local Suppliers
                    else if (ddlSupplierType.SelectedValue == "2")
                        SLAEnt.GL_CODE = "040302"; // GLCode of Foregin Suppliers
                    else if (ddlSupplierType.SelectedValue == "3")
                        SLAEnt.GL_CODE = "040303"; // GLCode of Agent
                    else if (ddlSupplierType.SelectedValue == "4")
                        SLAEnt.GL_CODE = "040304"; // GLCode of Party
                    SLAEnt.SUB_GL_NAME = txtSupplierName.Text;
                    SLAEnt.STATUS = ddlStatus.SelectedValue;
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
                if (txtSupplierName.Text != "")
                {
                    DistributedTransaction DT = new DistributedTransaction();
                    SUPEnt = new SUPPLIERS();
                    SUPEnt.PK_ID = lblPKIDU.Text;
                    SUPEnt = (SUPPLIERS)SUPSer.GetSingle(SUPEnt);
                    if (SUPEnt != null)
                    {
                        SUPEnt.SUPPLIER_TYPE = ddlSupplierType.SelectedValue;
                        SUPEnt.SUPPLIER_CODE = txtSupplierCode.Text.ToUpper();
                        SUPEnt.SUPPLIER_NAME = txtSupplierName.Text.ToUpper();
                        SUPEnt.VAT_PAN_NUMBER = txtPANNo.Text;
                        SUPEnt.REG_NUMBER = txtRegNo.Text;
                        SUPEnt.CONTACT_PERSON = txtContactPerson.Text;
                        SUPEnt.MOBILE = txtMobileNo.Text;
                        SUPEnt.PHONE = txtPhoneNo.Text;
                        SUPEnt.EMAIL = txtEmail.Text;
                        SUPEnt.STATUS = ddlStatus.SelectedValue;
                        SUPEnt.ADDRESS = txtAddress.Text;
                        SUPEnt.REMARKS = txtRemarks.Text;
                        SUPEnt.COUNTRY = ddlCountry.SelectedValue;
                        SUPSer.Update(SUPEnt, DT);


                        SLAEnt = new GL_SUB_ACCOUNT();
                        SLAEnt.SUB_GL_CODE = txtSupplierCode.Text.ToUpper();
                        SLAEnt = (GL_SUB_ACCOUNT)SLASer.GetSingle(SLAEnt);
                        if (SLAEnt != null)
                        {
                            if (ddlSupplierType.SelectedValue == "1")
                                SLAEnt.GL_CODE = "040301"; // GLCode of Local Suppliers
                            else if (ddlSupplierType.SelectedValue == "2")
                                SLAEnt.GL_CODE = "040302"; // GLCode of Foregin Suppliers
                            else if (ddlSupplierType.SelectedValue == "3")
                                SLAEnt.GL_CODE = "040303"; // GLCode of Agent
                            else if (ddlSupplierType.SelectedValue == "4")
                                SLAEnt.GL_CODE = "040305"; // GLCode of Party
                            SLAEnt.SUB_GL_NAME = txtSupplierName.Text;
                            SLAEnt.STATUS = ddlStatus.SelectedValue;
                            SLASer.Update(SLAEnt, DT);
                        }
                        else
                        {
                            SLAEnt = new GL_SUB_ACCOUNT();
                            SLAEnt.SUB_GL_CODE = txtSupplierCode.Text.ToUpper();
                            if (ddlSupplierType.SelectedValue == "1")
                                SLAEnt.GL_CODE = "040301"; // GLCode of Local Suppliers
                            else if (ddlSupplierType.SelectedValue == "2")
                                SLAEnt.GL_CODE = "040302"; // GLCode of Foregin Suppliers
                            else if (ddlSupplierType.SelectedValue == "3")
                                SLAEnt.GL_CODE = "040303"; // GLCode of Agent
                            else if (ddlSupplierType.SelectedValue == "4")
                                SLAEnt.GL_CODE = "040304"; // GLCode of Party
                            SLAEnt.SUB_GL_NAME = txtSupplierName.Text;
                            SLAEnt.STATUS = ddlStatus.SelectedValue;
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
        LoadGrid(ddlSupplierType.SelectedValue);

    }
    protected void gridSupplier_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblStat = e.Row.FindControl("lblStat") as Label;
            Label lblStatus = e.Row.FindControl("lblStatus") as Label;

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
        }
    }
    protected void gridSupplier_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Change"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblSupplierID = gr.FindControl("lblSupplierID") as Label;

            SUPEnt = new SUPPLIERS();
            SUPEnt.PK_ID = lblSupplierID.Text;
            SUPEnt = (SUPPLIERS)SUPSer.GetSingle(SUPEnt);
            if (SUPEnt != null)
            {
                ddlSupplierType.Text = SUPEnt.SUPPLIER_TYPE;
                lblPKIDU.Text = SUPEnt.PK_ID;
                txtSupplierCode.Text = SUPEnt.SUPPLIER_CODE;
                txtSupplierName.Text = SUPEnt.SUPPLIER_NAME;
                txtPANNo.Text = SUPEnt.VAT_PAN_NUMBER;
                txtRegNo.Text = SUPEnt.REG_NUMBER;
                txtContactPerson.Text = SUPEnt.CONTACT_PERSON;
                txtPhoneNo.Text = SUPEnt.PHONE;
                txtMobileNo.Text = SUPEnt.MOBILE;
                txtEmail.Text = SUPEnt.EMAIL;
                txtAddress.Text = SUPEnt.ADDRESS;
                txtRemarks.Text = SUPEnt.REMARKS;
                ddlCountry.SelectedValue = SUPEnt.COUNTRY;
                ddlStatus.SelectedValue = SUPEnt.STATUS;
            }
            divGrid.Visible = false;
            divAdd.Visible = true;
            txtSupplierCode.Enabled = false;
        }
    }



    protected void gridSupplier_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LoadGrid(ddlSupplierTypeList.SelectedValue);
        gridSupplier.PageIndex = e.NewPageIndex;
        gridSupplier.DataBind();
    }

    protected void txtPANNo_TextChanged(object sender, EventArgs e)
    {
        checkPan();
    }
}