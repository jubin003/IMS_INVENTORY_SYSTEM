using Entity.Components;
using Service.Components;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using PhyeGanCore;
using DataHelper.Framework;
using System.Web;

public partial class Administration_Agent : System.Web.UI.Page
{
    AGENT AEnt = new AGENT();
    AGENTService ASer = new AGENTService();

    GL_SUB_ACCOUNT SLAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService SLASer = new GL_SUB_ACCOUNTService();

    OFFICE OEnt = new OFFICE();
    OFFICEService OSer = new OFFICEService();

    District DEnt = new District();
    DistrictService DSer = new DistrictService();

    PhyeGan PG = new PhyeGan();

    Boolean IsPageRefresh = false;

    HelperFunction hf = new HelperFunction();

    UserProfileEntity userProfileEnt = new UserProfileEntity();

    static string path = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                ViewState["postids"] = System.Guid.NewGuid().ToString();
                Session["postid"] = ViewState["postids"].ToString();
                userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
                if (hf.checkPageAccess(path, userProfileEnt.UserGroupID.ToString()))
                {
                    loadBranch();
                    LoadDistrict();
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

    protected void LoadDistrict()
    {
        DEnt = new District();
        ddlDistrict.DataSource = DSer.GetAll(DEnt);
        ddlDistrict.DataTextField = "DISTRICTNAME";
        ddlDistrict.DataValueField = "PK_ID";
        ddlDistrict.DataBind();
        ddlDistrict.Items.Insert(0, "Select");
    }
    protected void LoadGrid(string agent_type, string office_code)
    {
        AEnt = new AGENT();
        AEnt.AGENT_TYPE = agent_type;
        AEnt.OFFICE_CODE = office_code;
        gridAgent.DataSource = ASer.GetAll(AEnt);
        gridAgent.DataBind();

        if (gridAgent.Rows.Count != 0)
        {
            gridAgent.Visible = true;
        }
        else
        {
            gridAgent.Visible = false;
        }
    }
    protected void txtAgentCode_TextChanged(object sender, EventArgs e)
    {
        txtAgentCode.Text = txtAgentCode.Text.ToUpper();
        AEnt = new AGENT();
        AEnt.AGENT_CODE = txtAgentCode.Text.ToUpper();
        AEnt = (AGENT)ASer.GetSingle(AEnt);
        if (AEnt != null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "This agent code is already taken");
            txtAgentCode.Text = "";
            txtAgentCode.Focus();
        }
        else
        {
            txtAgentName.Focus();
        }

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
            divBranchFilter.Visible = true;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
            ddlBranchFilter.SelectedValue = userProfileEnt.LocationID;
        }
        else
        {
            trBranch.Visible = false;
            divBranchFilter.Visible = false;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
            ddlBranchFilter.SelectedValue = userProfileEnt.LocationID;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!IsPageRefresh)
        {
            if (lblPKIDU.Text == "")
            {
                if (txtAgentCode.Text != "" && txtAgentName.Text != "")
                {
                    DistributedTransaction DT = new DistributedTransaction();
                    AEnt = new AGENT();
                    AEnt.AGENT_CODE = txtAgentCode.Text.ToUpper();
                    AEnt.AGENT_NAME = txtAgentName.Text;
                    AEnt.AGENT_ADDRESS1 = txtAddress1.Text;
                    AEnt.AGENT_ADDRESS2 = txtAddress2.Text;
                    AEnt.DISTRICT = ddlDistrict.SelectedValue;
                    AEnt.MOBILE = txtMobileNo.Text;
                    AEnt.EMAIL = txtEmail.Text;
                    AEnt.STATUS = ddlStatus.SelectedValue;
                    AEnt.COMMISSION_PER = txtCommissionPercent.Text;
                    AEnt.AGENT_TYPE = ddlAgentType.SelectedValue;
                    AEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                    string pk_ID = ASer.Insert(AEnt, DT).ToString();

                    SLAEnt = new GL_SUB_ACCOUNT();
                    SLAEnt.SUB_GL_CODE = txtAgentCode.Text.ToUpper();
                    SLAEnt.GL_CODE = "040302"; // GLCode of Agent
                    SLAEnt.SUB_GL_NAME = txtAgentName.Text;
                    SLAEnt.STATUS = ddlStatus.SelectedValue;
                    SLAEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                    SLASer.Insert(SLAEnt, DT);

                    divGrid.Visible = true;
                    divAdd.Visible = false;

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
                    HelperFunction.MsgBox(this, this.GetType(), "Agent Name and Code can't be empty.");
                }
            }

            else
            {
                if (txtAgentCode.Text != "" && txtAgentName.Text != "")
                {
                    DistributedTransaction DT = new DistributedTransaction();
                    AEnt = new AGENT();
                    AEnt.PK_ID = lblPKIDU.Text;
                    AEnt = (AGENT)ASer.GetSingle(AEnt);
                    if (AEnt != null)
                    {
                        AEnt.AGENT_CODE = txtAgentCode.Text.ToUpper();
                        AEnt.AGENT_NAME = txtAgentName.Text;
                        AEnt.AGENT_ADDRESS1 = txtAddress1.Text;
                        AEnt.AGENT_ADDRESS2 = txtAddress2.Text;
                        AEnt.DISTRICT = ddlDistrict.SelectedValue;
                        AEnt.MOBILE = txtMobileNo.Text;
                        AEnt.EMAIL = txtEmail.Text;
                        AEnt.STATUS = ddlStatus.SelectedValue;
                        AEnt.COMMISSION_PER = txtCommissionPercent.Text;
                        AEnt.AGENT_TYPE = ddlAgentType.SelectedValue;
                        AEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                        ASer.Update(AEnt, DT);


                        SLAEnt = new GL_SUB_ACCOUNT();
                        SLAEnt.SUB_GL_CODE = txtAgentCode.Text.ToUpper();
                        SLAEnt = (GL_SUB_ACCOUNT)SLASer.GetSingle(SLAEnt);
                        if (SLAEnt != null)
                        {
                            SLAEnt.GL_CODE = "040302"; // GLCode of Agent
                            SLAEnt.SUB_GL_NAME = txtAgentName.Text;
                            SLAEnt.STATUS = ddlStatus.SelectedValue;
                            SLAEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                            SLASer.Update(SLAEnt, DT);
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
                        HelperFunction.MsgBox(this, this.GetType(), "Agent Name and Code can't be empty.");
                    }

                }
            }
        }
        else
        {
            divAdd.Visible = false;
        }
        ddlBranchFilter.SelectedValue = ddlBranch.SelectedValue;
        ddlAgentTypeFilter.SelectedValue = ddlAgentType.SelectedValue;
        LoadGrid(ddlAgentType.SelectedValue, ddlBranch.SelectedValue);
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        clearFields();
        divGrid.Visible = true;
        divAdd.Visible = false;
    }
    protected void clearFields()
    {
        lblPKIDU.Text = "";
        txtAgentCode.Text = "";
        txtAgentName.Text = "";
        txtAddress1.Text = "";
        txtEmail.Text = "";
        txtMobileNo.Text = "";
        txtAddress2.Text = "";
        ddlStatus.SelectedIndex = 0;
    }
    protected void btnAddMore_Click(object sender, EventArgs e)
    {
        divGrid.Visible = false;
        divAdd.Visible = true;
        ddlBranch.SelectedValue = ddlBranchFilter.SelectedValue;
        if (ddlAgentTypeFilter.SelectedValue != "0")
        {
            ddlAgentType.SelectedValue = ddlAgentTypeFilter.SelectedValue;
        }
        else
        {
            ddlAgentTypeFilter.SelectedValue = "S";
        }
        clearFields();
    }
    protected void gridAgent_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LoadGrid(ddlAgentTypeFilter.SelectedValue, ddlBranchFilter.SelectedValue);
        gridAgent.PageIndex = e.NewPageIndex;
        gridAgent.DataBind();
    }

    protected void gridAgent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Change"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;

            Label lblAgentID = gr.FindControl("lblAgentID") as Label;

            AEnt = new AGENT();
            AEnt.PK_ID = lblAgentID.Text;
            AEnt = (AGENT)ASer.GetSingle(AEnt);
            if (AEnt != null)
            {
                lblPKIDU.Text = lblAgentID.Text;
                txtAgentCode.Text = AEnt.AGENT_CODE;
                txtAgentName.Text = AEnt.AGENT_NAME;
                txtAddress1.Text = AEnt.AGENT_ADDRESS1;
                txtAddress2.Text = AEnt.AGENT_ADDRESS2;
                ddlDistrict.SelectedValue = AEnt.DISTRICT;
                txtMobileNo.Text = AEnt.MOBILE;
                txtEmail.Text = AEnt.EMAIL;
                ddlStatus.SelectedValue = AEnt.STATUS;
                txtCommissionPercent.Text = AEnt.COMMISSION_PER;
                ddlAgentType.SelectedValue = AEnt.AGENT_TYPE;
            }
            divGrid.Visible = false;
            divAdd.Visible = true;
        }
    }

    protected void txtCommissionPercent_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double cPercent = Convert.ToDouble(txtCommissionPercent.Text);
        }
        catch
        {
            txtCommissionPercent.Text = "0";
        }
    }

    protected void ddlAgentTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid(ddlAgentTypeFilter.SelectedValue, ddlBranchFilter.SelectedValue);
    }

    protected void ddlBranchFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid(ddlAgentTypeFilter.SelectedValue, ddlBranchFilter.SelectedValue);
    }

    protected void gridAgent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblShowBranch = e.Row.FindControl("lblShowBranch") as Label;
            Label lblBranch = e.Row.FindControl("lblBranch") as Label;

            OEnt = new OFFICE();
            OEnt.PK_ID = lblBranch.Text;
            OEnt = (OFFICE)OSer.GetSingle(OEnt);
            if (OEnt != null)
            {
                lblShowBranch.Text = ED.Decrypt(OEnt.OFFICENAME) + " - " + OEnt.STREET;
            }
        }
    }
}