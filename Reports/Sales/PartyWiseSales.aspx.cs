using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;

public partial class Reports_Sales_PartyWiseSales : System.Web.UI.Page
{
    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    PhyeGan pg = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    HelperFunction hf = new HelperFunction();

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
                    LoadCustomer();
                    txtDateFrom.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
                    txtDateTo.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
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
    protected void LoadCustomer()
    {
        CEnt = new CUSTOMER();
        CEnt.OFFICE_CODE = ddlBranch.SelectedValue;
        ddlCustomerName.DataSource = CSer.GetAll(CEnt);
        ddlCustomerName.DataValueField = "PK_ID";
        ddlCustomerName.DataTextField = "CUSTOMER_NAME";
        ddlCustomerName.DataBind();
        ddlCustomerName.Items.Insert(0, "Select");
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
            LoadCompany();
            divhide.Visible = true;
            lblDate.Text = txtDateFrom.Text;
            string[] chalandateFrom = txtDateFrom.Text.Split('/');
            string challanDateFrom = PGD.ConvertNepaliTOEnglish(chalandateFrom[0], chalandateFrom[1], chalandateFrom[2]);
            string[] chalandateTo = txtDateTo.Text.Split('/');
            string challanDateTo = PGD.ConvertNepaliTOEnglish(chalandateTo[0], chalandateTo[1], chalandateTo[2]);

            string customer = null;
            if (ddlCustomerName.SelectedValue != "Select")
                customer = ddlCustomerName.SelectedValue;
            grdReport.DataSource = hf.getSalesReport(customer, challanDateFrom, challanDateTo, office_code);
            grdReport.DataBind();

          
        }
        catch (Exception ee)
        {
            HelperFunction.MsgBox(this, this.GetType(), ee.ToString());
        }
    }

   

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer();
    }
}