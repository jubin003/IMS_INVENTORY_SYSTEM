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
using Entity.Framework;

public partial class reports_UserLogReport : System.Web.UI.Page
{
    HelperFunction hf = new HelperFunction();
    NAME_COMPANY NEnt = new NAME_COMPANY();
    NAME_COMPANYService NSer = new NAME_COMPANYService();

    LOGIN_LOG LLEnt = new LOGIN_LOG();
    LOGIN_LOGService LLSer = new LOGIN_LOGService();

    OFFICE OEnt = new OFFICE();
    OFFICEService OSer = new OFFICEService();

    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();

    Entity.Components.Login LEnt = new Entity.Components.Login();
    LoginService LSer = new LoginService();

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
                    txtFDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
                    txtTDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
                    LoadUsers();
                    LoadCompanyDetail();
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
            divBranch.Visible = true;
            ddlBranch.Items.Insert(0, "");
        }
        else
        {
            divBranch.Visible = false;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
        }
    }

    protected void LoadUsers()
    {
        LEnt = new Entity.Components.Login();
        EntityList theList = new EntityList();
        EntityList newList = new EntityList();
        theList = LSer.GetAll(LEnt);
        foreach (Entity.Components.Login r1 in theList)
        {
            if (r1.GROUPID != "1")
                newList.Add(r1);
        }

        ddlUser.DataSource = newList;
        ddlUser.DataTextField = "FULLDETAILS";
        ddlUser.DataValueField = "EMPLOYEEID";

        ddlUser.DataBind();
        ddlUser.Items.Insert(0, "Select");
    }

    protected void LoadCompanyDetail()
    {
        PhyeGan PG = new PhyeGan();
        lblCompanyName.Text = PG.CompanyName();
        if (ddlBranch.SelectedValue != "")
        {
            lblCompanyAddress.Text = PG.BranchAddress(ddlBranch.SelectedValue);
            lblPhone1.Text = PG.BranchContact(ddlBranch.SelectedValue);
            lblEmail.Text = PG.BranchEmail(ddlBranch.SelectedValue);
        }
        else
        {
            lblCompanyAddress.Text = "";
            lblPhone1.Text = "";
            lblEmail.Text = "";
        }
        lblEmail.Text = PG.CompanyEmail();
        lblWebsite.Text = PG.CompanyWebsite();
        lblPanNo.Text = PG.CompanyVATPan();

        lblRegNo.Text = PG.CompanyRegistration();
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        LoadCompanyDetail();
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
        {
            office_code = ddlBranch.SelectedValue;
        }
        string user = "";
        if (ddlUser.SelectedValue != "Select")
        {
            user = ddlUser.SelectedValue;
        }
        if (txtFDate.Text != "" && txtTDate.Text != "")
        {
            try
            {
                hide.Visible = true;
                LLEnt = new LOGIN_LOG();
                string[] fromdate = txtFDate.Text.Split('/');
                string[] to_date = txtTDate.Text.Split('/');
                LLEnt.LOGIN_DATE = PGD.ConvertNepaliTOEnglish(fromdate[0], fromdate[1], fromdate[2]);
                LLEnt.TO_LOGIN_DATE = PGD.ConvertNepaliTOEnglish(to_date[0], to_date[1], to_date[2]);
                LLEnt.USER_ID = user;
                LLEnt.OFFICE_CODE = office_code;
                gridLog.DataSource = LLSer.GetAll(LLEnt);
                gridLog.DataBind();

                lblFDate.Text = txtFDate.Text;
                lblTDate.Text = txtTDate.Text;
                lblUser.Text = ddlUser.SelectedItem.ToString();
            }
            catch (Exception eee) { }
        }
        else
        {
            hide.Visible = false;
        }

    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void gridLog_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblBranchID = (Label)e.Row.FindControl("lblBranchID");
            Label lblUserName = (Label)e.Row.FindControl("lblUserName");
            Label lblBranchName = (Label)e.Row.FindControl("lblBranchName");

            OEnt = new OFFICE();
            OEnt.PK_ID = lblBranchID.Text;
            OEnt = (OFFICE)OSer.GetSingle(OEnt);
            if(OEnt != null)
            {
                lblBranchName.Text = ED.Decrypt(OEnt.OFFICENAME) + " - " + OEnt.STREET;
            }

            LEnt = new Entity.Components.Login();
            LEnt.EMPLOYEEID = lblUserID.Text;
            LEnt = (Entity.Components.Login)LSer.GetSingle(LEnt);
            if(LEnt != null)
            {
                lblUserName.Text = LEnt.FULLDETAILS;
            }

        }
    }
}