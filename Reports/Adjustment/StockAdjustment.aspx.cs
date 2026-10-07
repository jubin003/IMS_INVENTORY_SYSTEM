using System;
using Entity.Components;
using Service.Components;
using System.Web.UI;
using PhyeGanCore;
using System.Web;

public partial class Reports_Adjustment_StockAdjustment : System.Web.UI.Page
{
    PRODUCT_CATEGORY PCEnt = new PRODUCT_CATEGORY();
    PRODUCT_CATEGORYService PCSer = new PRODUCT_CATEGORYService();

    PRODUCT_SUB_CATEGORY PSCEnt = new PRODUCT_SUB_CATEGORY();
    PRODUCT_SUB_CATEGORYService PSCSer = new PRODUCT_SUB_CATEGORYService();

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();    

    HelperFunction hf = new HelperFunction();
    PhyeGanDate PGD = new PhyeGanDate();
    Boolean IsPageRefresh = false;

    PhyeGan PG = new PhyeGan();

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
                    LoadProductCategory();
                    LoadProductSubCategory();
                    LoadProduct();
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
        ddlBranch.DataSource = PG.getBranchList();
        ddlBranch.DataTextField = "OFFICENAME";
        ddlBranch.DataValueField = "PK_ID";
        ddlBranch.DataBind();

        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (userProfileEnt.LocationTypeID == 1 && PG.checkBranchAccess(path, userProfileEnt.UserGroupID.ToString()) && PG.CompanyBranch_Status())
        {
            divBranch.Visible = true;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
        }
        else
        {
            divBranch.Visible = false;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
        }
    }

    protected void LoadCompany()
    {
        lblCompanyName.Text = PG.CompanyName();
        if (ddlBranch.SelectedValue != "")
        {
            lblAddress.Text = PG.BranchAddress(ddlBranch.SelectedValue);
            lblContact.Text = PG.BranchContact(ddlBranch.SelectedValue);
        }
        else
        {
            lblAddress.Text = "";
            lblContact.Text = "";
        }

    }
    protected void LoadProductCategory()
    {
        PCEnt = new PRODUCT_CATEGORY();
        PCEnt.STATUS = "1";
        ddlCategoryFilter.DataSource = PCSer.GetAll(PCEnt);
        ddlCategoryFilter.DataTextField = "CATEGORY_NAME";
        ddlCategoryFilter.DataValueField = "PK_ID";
        ddlCategoryFilter.DataBind();
        ddlCategoryFilter.Items.Insert(0, "Select");

    }
    protected void LoadProductSubCategory()
    {
        PSCEnt = new PRODUCT_SUB_CATEGORY();
        PSCEnt.CATEGORY_ID = ddlCategoryFilter.SelectedValue;
        PSCEnt.STATUS = "1";
        ddlSubCategoryFilter.DataSource = PSCSer.GetAll(PSCEnt);
        ddlSubCategoryFilter.DataTextField = "SUB_CATEGORY_NAME";
        ddlSubCategoryFilter.DataValueField = "PK_ID";
        ddlSubCategoryFilter.DataBind();
        ddlSubCategoryFilter.Items.Insert(0, "Select");
    }
    protected void LoadProduct()
    {
        PEnt = new PRODUCT();
        PEnt.CATEGORY_ID = ddlCategoryFilter.SelectedValue;
        if (ddlSubCategoryFilter.SelectedValue != "Select")
            PEnt.SUB_CATEGORY_ID = ddlSubCategoryFilter.SelectedValue;
        else
            PEnt.SUB_CATEGORY_ID = "0";
        PEnt.OFFICE_CODE = ddlBranch.SelectedValue;
        ddlProductFilter.DataSource = PSer.GetAll(PEnt);
        ddlProductFilter.DataValueField = "PK_ID";
        ddlProductFilter.DataTextField = "PRODUCT_NAME";
        ddlProductFilter.DataBind();
        ddlProductFilter.Items.Insert(0, "Select");
    }
   
    protected void ddlCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductSubCategory();
        LoadProduct();
    }
    protected void ddlSubCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProduct();
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        LoadStock();
        divhide.Visible = true;
    }
    protected void LoadStock()
    {
        string office_code = "";
        if(ddlBranch.SelectedValue != "")
        {
            office_code = ddlBranch.SelectedValue;
        }
        string category = ddlCategoryFilter.SelectedValue == "Select" ? null : ddlCategoryFilter.SelectedValue;
        string subcategory = ddlSubCategoryFilter.SelectedValue == "Select" ? "0" : ddlSubCategoryFilter.SelectedValue;
        string product = ddlProductFilter.SelectedValue == "Select" ? null : ddlProductFilter.SelectedValue;

        string[] dateFrom = txtDateFrom.Text.Split('/');
        string DateFrom = PGD.ConvertNepaliTOEnglish(dateFrom[0], dateFrom[1], dateFrom[2]);
        string[] dateTo = txtDateTo.Text.Split('/');
        string DateTo = PGD.ConvertNepaliTOEnglish(dateTo[0], dateTo[1], dateTo[2]);


        if (category == null && subcategory == "0" && product == null)
        {
            grdStock.DataSource = hf.getAdjustmentReport(null, null, null, DateFrom, DateTo, office_code);
            grdStock.DataBind();
        }
        else if ((category != null && subcategory == "0" && product == null))
        {
            grdStock.DataSource = hf.getAdjustmentReport(category, null, null, DateFrom, DateTo, office_code);
            grdStock.DataBind();

        }
        else if ((category != null && subcategory == "0" && product != null))
        {
            grdStock.DataSource = hf.getAdjustmentReport(category, null, product, DateFrom, DateTo, office_code);
            grdStock.DataBind();

        }
        else if ((category != null && subcategory != "0" && product == null))
        {
            grdStock.DataSource = hf.getAdjustmentReport(category, subcategory, null, DateFrom, DateTo, office_code);
            grdStock.DataBind();

        }
        else
        {
            grdStock.DataSource = hf.getAdjustmentReport(category, subcategory, product, DateFrom, DateTo, office_code);
            grdStock.DataBind();
        }


        if (hf.ProductBatch() == "1")
            grdStock.Columns[3].Visible = true;
        else
            grdStock.Columns[3].Visible = false;

        if (hf.ProductExpiry() == "1")
            grdStock.Columns[4].Visible = true;
        else
            grdStock.Columns[4].Visible = false;

        if (hf.ProductColor() == "1")
            grdStock.Columns[5].Visible = true;
        else
            grdStock.Columns[5].Visible = false;

        if (hf.ProductSize() == "1")
            grdStock.Columns[6].Visible = true;
        else
            grdStock.Columns[6].Visible = false;

        if (hf.ProductManufacturer() == "1")
            grdStock.Columns[7].Visible = true;
        else
            grdStock.Columns[7].Visible = false;
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProduct();
    }
}