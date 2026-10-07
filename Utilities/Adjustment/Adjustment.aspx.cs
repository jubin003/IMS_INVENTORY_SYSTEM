using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Web;
using PhyeGanCore;

public partial class Utilities_Adjustment_Adjustment : System.Web.UI.Page
{
    PRODUCT_CATEGORY PCEnt = new PRODUCT_CATEGORY();
    PRODUCT_CATEGORYService PCSer = new PRODUCT_CATEGORYService();

    PRODUCT_SUB_CATEGORY PSCEnt = new PRODUCT_SUB_CATEGORY();
    PRODUCT_SUB_CATEGORYService PSCSer = new PRODUCT_SUB_CATEGORYService();

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();

    PRODUCT_COLOUR PColEnt = new PRODUCT_COLOUR();
    PRODUCT_COLOURService PColSer = new PRODUCT_COLOURService();

    PRODUCT_SIZE PSEnt = new PRODUCT_SIZE();
    PRODUCT_SIZEService PSSer = new PRODUCT_SIZEService();

    PRODUCT_MANUFACTURE PMEnt = new PRODUCT_MANUFACTURE();
    PRODUCT_MANUFACTUREService PMSer = new PRODUCT_MANUFACTUREService();

    PRODUCT_ADJUSTMENT PAEnt = new PRODUCT_ADJUSTMENT();
    PRODUCT_ADJUSTMENTService PASer = new PRODUCT_ADJUSTMENTService();

    OPENING_BALANCE OBEnt = new OPENING_BALANCE();
    OPENING_BALANCEService OBSer = new OPENING_BALANCEService();

    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();

    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfile = new UserProfileEntity();
    Boolean IsPageRefresh = false;
    AccountFunction af = new AccountFunction();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            ViewState["postids"] = System.Guid.NewGuid().ToString();
            Session["postid"] = ViewState["postids"].ToString();
            try
            {
                userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                string path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
                if (hf.checkPageAccess(path, userProfile.UserGroupID.ToString()))
                {
                    LoadProductCategory();
                }
                else
                {
                    Response.Redirect("~/forbidden.aspx");
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

        UserProfileEntity userProfileEnt = new UserProfileEntity();
        try
        {
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            if (!IsPostBack)
            {
                ViewState["postids"] = System.Guid.NewGuid().ToString();
                Session["postid"] = ViewState["postids"].ToString();
                try
                {

                }
                catch
                {
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
        catch
        {
            Response.Redirect("~/Login.aspx");
        }
    }

    protected void LoadProductCategory()
    {
        PCEnt = new PRODUCT_CATEGORY();
        PCEnt.STATUS = "1";
        ddlProductCategory.DataSource = PCSer.GetAll(PCEnt);
        ddlProductCategory.DataTextField = "CATEGORY_NAME";
        ddlProductCategory.DataValueField = "PK_ID";
        ddlProductCategory.DataBind();
        ddlProductCategory.Items.Insert(0, "Select");
    }
    protected void LoadProductSubCategory()
    {
        PSCEnt = new PRODUCT_SUB_CATEGORY();
        PSCEnt.CATEGORY_ID = ddlProductCategory.SelectedValue;
        PSCEnt.STATUS = "1";
        ddlProductSubCategory.DataSource = PSCSer.GetAll(PSCEnt);
        ddlProductSubCategory.DataTextField = "SUB_CATEGORY_NAME";
        ddlProductSubCategory.DataValueField = "PK_ID";
        ddlProductSubCategory.DataBind();
        ddlProductSubCategory.Items.Insert(0, "Select");
    }
    protected void LoadColour()
    {
        PColEnt = new PRODUCT_COLOUR();
        PColEnt.STATUS = "1";
        ddlProductColor.DataSource = PColSer.GetAll(PColEnt);
        ddlProductColor.DataValueField = "PK_ID";
        ddlProductColor.DataTextField = "COLOUR_NAME";
        ddlProductColor.DataBind();
    }
    protected void LoadSize()
    {
        PSEnt = new PRODUCT_SIZE();
        PSEnt.STATUS = "1";
        ddlProductSize.DataSource = PSSer.GetAll(PSEnt);
        ddlProductSize.DataValueField = "PK_ID";
        ddlProductSize.DataTextField = "SIZE_NAME";
        ddlProductSize.DataBind();
    }
    protected void LoadManufacturer()
    {
        PMEnt = new PRODUCT_MANUFACTURE();
        PMEnt.STATUS = "1";
        ddlProductManufacturer.DataSource = PMSer.GetAll(PMEnt);
        ddlProductManufacturer.DataValueField = "PK_ID";
        ddlProductManufacturer.DataTextField = "MANUFACTURE_NAME";
        ddlProductManufacturer.DataBind();
    }
    protected void LoadProduct()
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        PEnt = new PRODUCT();
        PEnt.CATEGORY_ID = ddlProductCategory.SelectedValue;
        if (ddlProductSubCategory.SelectedValue != "Select")
            PEnt.SUB_CATEGORY_ID = ddlProductSubCategory.SelectedValue;
        else
            PEnt.SUB_CATEGORY_ID = "0";
        PEnt.OFFICE_CODE = userProfile.LocationID;
        ddlproduct.DataSource = PSer.GetAll(PEnt);
        ddlproduct.DataValueField = "PK_ID";
        ddlproduct.DataTextField = "PRODUCT_NAME";
        ddlproduct.DataBind();
        ddlproduct.Items.Insert(0, "Select");
    }
    protected void ddlProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductSubCategory();
        LoadProduct();
    }


    protected void ddlProductSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProduct();
    }

    protected void ddlproduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductDetail();
    }
    protected void LoadProductDetail()
    {
        PEnt = new PRODUCT();
        PEnt.PK_ID = ddlproduct.SelectedValue;
        PEnt = (PRODUCT)PSer.GetSingle(PEnt);
        if (PEnt != null)
        {
            #region to load initial data
            lblPackQty.Text = PEnt.PACK_QTY;
            PUEnt = new PRODUCT_UNIT();
            PUEnt.PK_ID = PEnt.UNIT_ID;
            PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
            if (PUEnt != null)
                lblUnit.Text = PUEnt.UNIT_NAME;
            divUnit.Visible = true;
            divPackQty.Visible = true;

            if (PEnt.COLOUR_STATUS == "1")
            {
                divPColour.Visible = true;
                LoadColour();
            }
            else
                divPColour.Visible = false;

            if (PEnt.SIZE_STATUS == "1")
            {
                LoadSize();
                divPSize.Visible = true;
            }
            else
                divPSize.Visible = false;

            if (PEnt.MANUFACTURE_STATUS == "1")
            {
                LoadManufacturer();
                divPManufacturer.Visible = true;
            }
            else
                divPManufacturer.Visible = false;

            if (PEnt.EXPIRY_STATUS == "1")
                divPExpiry.Visible = true;
            else
                divPExpiry.Visible = false;

            if (PEnt.BATCH_STATUS == "1")
                divPBatch.Visible = true;
            else
                divPBatch.Visible = false;

            OBEnt = new OPENING_BALANCE();
            OBEnt.PRODUCT_ID = PEnt.PK_ID;
            OBEnt.OFFICE_CODE = userProfile.LocationID;
            OBEnt = (OPENING_BALANCE)OBSer.GetSingle(OBEnt);
            if(OBEnt != null)
            {
                if(OBEnt.MANUFACTURER_ID == "")
                {
                    ddlProductManufacturer.SelectedItem.Text = "-";
                }
                else
                {
                    ddlProductManufacturer.SelectedValue = OBEnt.MANUFACTURER_ID;
                }

                txtQty.Text = OBEnt.QUANTITY;                
            }

            divQuantity.Visible = true;
            divRemarks.Visible = true;
            btnAdd.Visible = true;
            #endregion

        }

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        #region to insert
        if (!IsPageRefresh)
        {
            userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            PAEnt = new PRODUCT_ADJUSTMENT();
            PAEnt.PRODUCT_ID = ddlproduct.SelectedValue;


            PAEnt.ADJUSTMENT_DATE = PGD.GetTodayDate("dd/mm/yyyy");
            PAEnt.ADJUSTMENT_DAY = PGD.NepaliDay();
            PAEnt.ADJUSTMENT_MONTH = PGD.NepaliMonth();
            PAEnt.ADJUSTMENT_YEAR = PGD.NepaliYear();
            PAEnt.ADJUSTMENT_FY = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
            PAEnt.ADJUSTED_BY = userProfile.EmployeeID;
            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlproduct.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                if (PEnt.COLOUR_STATUS == "1" && PGPS.ProductColor())
                    PAEnt.COLOUR_ID = ddlProductColor.SelectedValue;

                if (PEnt.SIZE_STATUS == "1" && PGPS.ProductSize())
                    PAEnt.SIZE_ID = ddlProductSize.SelectedValue;

                if (PEnt.MANUFACTURE_STATUS == "1" && PGPS.ProductManufacture())
                    if (ddlProductManufacturer.SelectedItem.Text != "-")
                        PAEnt.MANUFACTURER_ID = ddlProductManufacturer.SelectedValue;

                if (PEnt.EXPIRY_STATUS == "1" && PGPS.ProductExpDate())
                    PAEnt.EXPIRY_DATE = txtExpiryDate.Text;

                if (PEnt.BATCH_STATUS == "1" && PGPS.ProductBatch())
                    PAEnt.BATCH_NUMBER = txtBatch.Text;
            }
            PAEnt.QUANTITY = txtQty.Text;
            PAEnt.REMARKS = txtRemarks.Text;
            PAEnt.OFFICE_CODE = userProfile.LocationID;
            PASer.Insert(PAEnt);
            Clear();
            HelperFunction.MsgBox(this, this.GetType(), "Stock Adjusted.");
        }
        #endregion
    }
    protected void Clear()
    {
        try
        {
            ddlProductCategory.SelectedIndex = 0;
            ddlProductSubCategory.SelectedIndex = 0;
            ddlproduct.SelectedIndex = 0;
        }
        catch { }
        txtQty.Text = "";
        divUnit.Visible = false;
        divPackQty.Visible = false;
        divPColour.Visible = false;
        divPSize.Visible = false;
        divPManufacturer.Visible = false;
        divPExpiry.Visible = false;
        divPBatch.Visible = false;
        divQuantity.Visible = false;
        divRemarks.Visible = false;
        btnAdd.Visible = false;
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
    protected void btnAddDiv_Click(object sender, EventArgs e)
    {
        divAdd.Visible = true;
        ddlProductCategory.Enabled = true;
        ddlProductSubCategory.Enabled = true;
        ddlproduct.Enabled = true;
        Clear();
    }


}