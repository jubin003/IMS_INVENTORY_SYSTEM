using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PhyeGanCore;
using Entity.Components;

public partial class Reports_General_StockOut : System.Web.UI.Page
{
    HelperFunction hf = new HelperFunction();
    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGan pg = new PhyeGan();
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
                    loadData();
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
    protected void loadData()
    {
        grdStock.DataSource = hf.getStockOut(PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()),ddlBranch.SelectedValue);
        grdStock.DataBind();

        if (hf.ProductColor() == "1")
            grdStock.Columns[2].Visible = true;
        else
            grdStock.Columns[2].Visible = false;

        if (hf.ProductSize() == "1")
            grdStock.Columns[3].Visible = true;
        else
            grdStock.Columns[3].Visible = false;

        if (hf.ProductManufacturer() == "1")
            grdStock.Columns[4].Visible = true;
        else
            grdStock.Columns[4].Visible = false;
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
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadData();
        LoadCompany();
    }
}