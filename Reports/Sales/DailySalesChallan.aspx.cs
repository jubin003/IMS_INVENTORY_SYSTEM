using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PhyeGanCore;
using Entity.Components;
using System.Web;

public partial class Reports_Sales_DailySalesChallan : System.Web.UI.Page
{
    HelperFunction hf = new HelperFunction();
    PhyeGan pg = new PhyeGan();
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
                path = path.Replace(pg.Org_Base_URL(), "");

                if (hf.checkPageAccess(path, userProfileEnt.UserGroupID.ToString()))
                {
                    loadBranch();
                    txtChalanDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
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
        ddlBranch.DataSource = pg.getBranchList();
        ddlBranch.DataTextField = "OFFICENAME";
        ddlBranch.DataValueField = "PK_ID";
        ddlBranch.DataBind();

        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (userProfileEnt.LocationTypeID == 1 && pg.checkBranchAccess(path, userProfileEnt.UserGroupID.ToString()) && pg.CompanyBranch_Status())
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
    protected void LoadCompany()
    {
        lblCompanyName.Text = pg.CompanyName();
        if (ddlBranch.SelectedValue != "")
        {
            lblAddress.Text = pg.BranchAddress(ddlBranch.SelectedValue);
            lblContact.Text = pg.BranchContact(ddlBranch.SelectedValue);
        }
        else
        {
            lblAddress.Text = "";
            lblContact.Text = "";
        }

    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
        {
            office_code = ddlBranch.SelectedValue;
        }
        try
        {
            
            divhide.Visible = true;
            lblDate.Text = txtChalanDate.Text;
            string[] chalandate = txtChalanDate.Text.Split('/');
            string challanDate = PGD.ConvertNepaliTOEnglish(chalandate[0], chalandate[1], chalandate[2]);
            grdReport.DataSource = hf.getSalesReport(null, challanDate, challanDate, office_code);
            grdReport.DataBind();            
        }
        catch
        {
            HelperFunction.MsgBox(this, this.GetType(), "Invalid Date");
        }
    }

    

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }
}