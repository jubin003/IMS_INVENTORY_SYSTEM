using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;
using System.Globalization;
using Entity.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;
using DataHelper.Framework;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Web;

public partial class Utilities_OpeningBalance : System.Web.UI.Page
{
    NAME_COMPANY NCEnt = new NAME_COMPANY();
    NAME_COMPANYService NCSer = new NAME_COMPANYService();

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

    OPENING_BALANCE OBEnt = new OPENING_BALANCE();
    OPENING_BALANCEService OBSer = new OPENING_BALANCEService();

    OFFICE OEnt = new OFFICE();
    OFFICEService OSer = new OFFICEService();

    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

    HelperFunction hf = new HelperFunction();
    PhyeGanDate PGD = new PhyeGanDate();

    PhyeGan PG = new PhyeGan();

    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();
    EntityList theList = new EntityList();

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
                    loadCurrentFY();
                    if (PGPS.ProductBatch() && PGPS.ProductExpDate())
                    {
                        divWhole.Visible = false;
                        divSolo.Visible = true;
                        divexcleUp.Visible = false;
                    }
                    else
                    {
                        divWhole.Visible = true;
                        divSolo.Visible = false;
                        divexcleUp.Visible = false;
                    }
                    LoadFY();
                    LoadFYSolo();
                    checkFY();
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

    protected void loadCurrentFY()
    {
        ddlFiscalYear.DataSource = FYSer.GetAll(FYEnt);
        ddlFiscalYear.DataTextField = "FISCAL_YEAR";
        ddlFiscalYear.DataValueField = "FISCAL_YEAR";
        ddlFiscalYear.DataBind();
    }
    protected void checkFY()
    {
        if (PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()) == lblFYSolo.Text)
        {
            LoadCategory();
            ddlProductCategory.Enabled = true;
            LoadProductCategory();
            divFilter.Visible = true;
            divAlert.Visible = false;
            divFilterSolo.Visible = true;
            btnAddDivSolo.Visible = true;
            btnFilterSolo.Visible = true;
            btnExcelUp.Visible = true;

            #region to show hide product property div
            ddlProductSize.SelectedValue = hf.ProductSize();
            if (hf.ProductSize() == "1")
                divPSize.Visible = true;
            else divPSize.Visible = false;

            ddlProductManufacturer.SelectedValue = hf.ProductManufacturer();
            if (hf.ProductManufacturer() == "1")
                divPManufacturer.Visible = true;
            else
                divPManufacturer.Visible = false;
            #endregion
        }
        else
        {
            ddlProductCategory.Enabled = false;
            divFilter.Visible = false;
            divAdd.Visible = false;
            divFilterSolo.Visible = false;
            divAlert.Visible = true;

            btnAddDivSolo.Visible = false;
            btnFilterSolo.Visible = false;
            btnExcelUp.Visible = false;
            //HelperFunction.MsgBox(this, this.GetType(), "You cannot enter Opening balance for this fiscal year. Perform closing balance.");
        }
    }
    protected void loadBranch()
    {
        ddlBranchFilter.DataSource = PG.getBranchList();
        ddlBranchFilter.DataTextField = "OFFICENAME";
        ddlBranchFilter.DataValueField = "PK_ID";
        ddlBranchFilter.DataBind();

        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (userProfileEnt.LocationTypeID == 1 && PG.checkBranchAccess(path, userProfileEnt.UserGroupID.ToString()) && PG.CompanyBranch_Status())
        {
            divBranchFilter.Visible = true;
            ddlBranchFilter.SelectedValue = userProfileEnt.LocationID;
        }
        else
        {
            divBranchFilter.Visible = false;
            ddlBranchFilter.SelectedValue = userProfileEnt.LocationID;
        }
    }
    protected void LoadFY()
    {
        OEnt = new OFFICE();
        OEnt.PK_ID = ddlBranchFilter.SelectedValue;
        OEnt = (OFFICE)OSer.GetSingle(OEnt);
        if (OEnt != null)
        {
            lblFY.Text = OEnt.OPENING_FISCAL_YEAR;
        }
        //NCEnt = new NAME_COMPANY();
        //NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
        //if (NCEnt != null)
        //    lblFY.Text = NCEnt.OPENING_FY;
    }
    protected void LoadFYSolo()
    {
        OEnt = new OFFICE();
        OEnt.PK_ID = ddlBranchFilter.SelectedValue;
        OEnt = (OFFICE)OSer.GetSingle(OEnt);
        if (OEnt != null)
        {
            lblFYSolo.Text = OEnt.OPENING_FISCAL_YEAR;
        }
        //NCEnt = new NAME_COMPANY();
        //NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
        //if (NCEnt != null)
        //    lblFYSolo.Text = NCEnt.OPENING_FY;
    }
    protected void LoadCategory()
    {
        PCEnt = new PRODUCT_CATEGORY();
        PCEnt.STATUS = "1";
        ddlCategoryFilter.DataSource = PCSer.GetAll(PCEnt);
        ddlCategoryFilter.DataTextField = "CATEGORY_NAME";
        ddlCategoryFilter.DataValueField = "PK_ID";
        ddlCategoryFilter.DataBind();
        ddlCategoryFilter.Items.Insert(0, "Select");
    }
    protected void LoadSubCategory()
    {
        PSCEnt = new PRODUCT_SUB_CATEGORY();
        PSCEnt.STATUS = "1";
        PSCEnt.CATEGORY_ID = ddlCategoryFilter.SelectedValue;
        ddlSubCategoryFilter.DataSource = PSCSer.GetAll(PSCEnt);
        ddlSubCategoryFilter.DataTextField = "SUB_CATEGORY_NAME";
        ddlSubCategoryFilter.DataValueField = "PK_ID";
        ddlSubCategoryFilter.DataBind();
        ddlSubCategoryFilter.Items.Insert(0, "Select");
    }

    protected void ddlCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSubCategory();
    }
    protected void Clear()
    {
        ddlCategoryFilter.SelectedIndex = 0;
        ddlSubCategoryFilter.SelectedIndex = 0;
        divAdd.Visible = false;
        divFilter.Visible = true;
    }
    protected void btnAddDiv_Click(object sender, EventArgs e)
    {
        // divAdd.Visible = true;
        divFilter.Visible = false;
        lblOBPK_ID.Text = "";
        Clear();
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        // divAdd.Visible = false;
        divFilter.Visible = true;

    }
    protected void LoadToAdd()
    {
        PEnt = new PRODUCT();
        PEnt.CATEGORY_ID = ddlCategoryFilter.SelectedValue;
        PEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
        PEnt.STATUS = "1";
        if (ddlSubCategoryFilter.SelectedValue != "Select")
        { PEnt.SUB_CATEGORY_ID = ddlSubCategoryFilter.SelectedValue; }
        gridAddOpen.DataSource = PSer.GetAll(PEnt);
        gridAddOpen.DataBind();
    }
    protected void gridAddOpen_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblProductCode = e.Row.FindControl("lblProductCode") as Label;
                Label lblProductId = e.Row.FindControl("lblProductId") as Label;
                Label lblProductName = e.Row.FindControl("lblProductName") as Label;
                TextBox txtRate = e.Row.FindControl("txtRate") as TextBox;
                TextBox txtQuantity = e.Row.FindControl("txtQuantity") as TextBox;
                TextBox txtCloVal = e.Row.FindControl("txtCloVal") as TextBox;

                PEnt = new PRODUCT();
                PEnt.PK_ID = lblProductId.Text;
                PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                if (PEnt != null)
                {
                    lblProductName.Text = PEnt.PRODUCT_NAME;
                    lblProductCode.Text = PEnt.PRODUCT_CODE;
                }
                OBEnt = new OPENING_BALANCE();
                OBEnt.PRODUCT_ID = lblProductId.Text;
                OBEnt = (OPENING_BALANCE)OBSer.GetSingle(OBEnt);
                if (OBEnt != null)
                {
                    txtQuantity.Text = OBEnt.QUANTITY;
                    txtRate.Text = OBEnt.RATE;
                    if (txtRate.Text != "" && txtQuantity.Text != "")
                    {
                        decimal Rate = Convert.ToDecimal(txtRate.Text);
                        decimal Quantity = Convert.ToDecimal(txtQuantity.Text);
                        decimal CloValue = Rate * Quantity;
                        txtCloVal.Text = Convert.ToString(CloValue);

                    }
                }
                else
                {
                    txtQuantity.Text = "0";
                    txtRate.Text = "0";
                    txtCloVal.Text = "0";
                }
            }
        }
    }
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {

        TextBox txtRate = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtRate.NamingContainer;


        TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
        TextBox txtCloVal = (TextBox)row.FindControl("txtCloVal");
        decimal rate = 0;
        decimal quantity = 0;


        if (decimal.TryParse(txtRate.Text, out rate) && decimal.TryParse(txtQuantity.Text, out quantity))
        {
            decimal cloValue = rate * quantity;
            txtCloVal.Text = cloValue.ToString("0.00");
        }

    }
    protected void Add_Click(object sender, EventArgs e)
    {
        LoadToAdd();
        divAdd.Visible = true;
    }
    protected void btn_add_new_Click(object sender, EventArgs e)
    {
        string currentFY = ddlFiscalYear.SelectedValue;
        string previousFY = PGD.getPrevious_FiscalYear(currentFY);
        if (!IsPageRefresh)
        {
            foreach (GridViewRow gr in gridAddOpen.Rows)
            {
                Label lblpk_id = gr.FindControl("lblPK_ID") as Label;
                TextBox txtRate = gr.FindControl("txtRate") as TextBox;
                TextBox txtQuantity = gr.FindControl("txtQuantity") as TextBox;
                TextBox txtCloVal = gr.FindControl("txtCloVal") as TextBox;

                #region for current year
                OBEnt = new OPENING_BALANCE();
                OBEnt.PRODUCT_ID = lblpk_id.Text;
                OBEnt.FISCAL_YEAR = currentFY;
                OBEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
                OBEnt = (OPENING_BALANCE)OBSer.GetSingle(OBEnt);
                if (OBEnt != null)
                {
                    OBEnt.RATE = txtRate.Text;
                    OBEnt.QUANTITY = txtQuantity.Text;
                    OBEnt.CLOSING_VALUE = txtCloVal.Text;
                    OBSer.Update(OBEnt);
                }
                else
                {
                    OBEnt = new OPENING_BALANCE();
                    OBEnt.PRODUCT_ID = lblpk_id.Text;
                    OBEnt.RATE = txtRate.Text;
                    OBEnt.QUANTITY = txtQuantity.Text;
                    OBEnt.CLOSING_VALUE = txtCloVal.Text;
                    OBEnt.FISCAL_YEAR = currentFY;
                    OBEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
                    OBSer.Insert(OBEnt);
                }
                #endregion

                #region for previous year
                OBEnt = new OPENING_BALANCE();
                OBEnt.PRODUCT_ID = lblpk_id.Text;
                OBEnt.FISCAL_YEAR = previousFY;
                OBEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
                OBEnt = (OPENING_BALANCE)OBSer.GetSingle(OBEnt);
                if (OBEnt != null)
                {
                    OBEnt.RATE = txtRate.Text;
                    OBEnt.QUANTITY = txtQuantity.Text;
                    OBEnt.CLOSING_VALUE = txtCloVal.Text;
                    OBSer.Update(OBEnt);
                }
                else
                {
                    OBEnt = new OPENING_BALANCE();
                    OBEnt.PRODUCT_ID = lblpk_id.Text;
                    OBEnt.RATE = txtRate.Text;
                    OBEnt.QUANTITY = txtQuantity.Text;
                    OBEnt.CLOSING_VALUE = txtCloVal.Text;
                    OBEnt.FISCAL_YEAR = previousFY;
                    OBEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
                    OBSer.Insert(OBEnt);
                }
                #endregion
                LoadToAdd();
            }
        }
    }
    protected void txtQuantity_TextChanged(object sender, EventArgs e)
    {
        //Imp Code Text Change INside Grid
        TextBox txtQuantity = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtQuantity.NamingContainer;


        TextBox txtRate = (TextBox)row.FindControl("txtRate");
        TextBox txtCloVal = (TextBox)row.FindControl("txtCloVal");


        decimal rate = 0;
        decimal quantity = 0;


        if (decimal.TryParse(txtRate.Text, out rate) && decimal.TryParse(txtQuantity.Text, out quantity))
        {

            decimal cloValue = rate * quantity;
            txtCloVal.Text = cloValue.ToString("0.00");
        }
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Clear();
    }
    #region

    protected void LoadProductCategory()
    {
        PCEnt = new PRODUCT_CATEGORY();
        PCEnt.STATUS = "1";
        ddlProductCategory.DataSource = PCSer.GetAll(PCEnt);
        ddlProductCategory.DataTextField = "CATEGORY_NAME";
        ddlProductCategory.DataValueField = "PK_ID";
        ddlProductCategory.DataBind();
        ddlProductCategory.Items.Insert(0, "Select");

        PCEnt = new PRODUCT_CATEGORY();
        PCEnt.STATUS = "1";
        ddlCategoryFilterSolo.DataSource = PCSer.GetAll(PCEnt);
        ddlCategoryFilterSolo.DataTextField = "CATEGORY_NAME";
        ddlCategoryFilterSolo.DataValueField = "PK_ID";
        ddlCategoryFilterSolo.DataBind();
        ddlCategoryFilterSolo.Items.Insert(0, "Select");

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

        PSCEnt = new PRODUCT_SUB_CATEGORY();
        PSCEnt.CATEGORY_ID = ddlCategoryFilterSolo.SelectedValue;
        PSCEnt.STATUS = "1";
        ddlSubCategoryFilterSolo.DataSource = PSCSer.GetAll(PSCEnt);
        ddlSubCategoryFilterSolo.DataTextField = "SUB_CATEGORY_NAME";
        ddlSubCategoryFilterSolo.DataValueField = "PK_ID";
        ddlSubCategoryFilterSolo.DataBind();
        ddlSubCategoryFilterSolo.Items.Insert(0, "Select");
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
        PEnt = new PRODUCT();
        PEnt.CATEGORY_ID = ddlProductCategory.SelectedValue;
        if (ddlProductSubCategory.SelectedValue != "Select")
            PEnt.SUB_CATEGORY_ID = ddlProductSubCategory.SelectedValue;
        else
            PEnt.SUB_CATEGORY_ID = "0";
        PEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
        ddlproduct.DataSource = PSer.GetAll(PEnt);
        ddlproduct.DataValueField = "PK_ID";
        ddlproduct.DataTextField = "PRODUCT_NAME";
        ddlproduct.DataBind();
        ddlproduct.Items.Insert(0, "Select");


        PEnt = new PRODUCT();
        PEnt.CATEGORY_ID = ddlCategoryFilterSolo.SelectedValue;
        if (ddlSubCategoryFilterSolo.SelectedValue != "Select")
            PEnt.SUB_CATEGORY_ID = ddlSubCategoryFilterSolo.SelectedValue;
        else
            PEnt.SUB_CATEGORY_ID = "0";

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
        LoadManufacturer();
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



            divPSize.Visible = PGPS.ProductSize();
            divPManufacturer.Visible = PGPS.ProductManufacture();
            divPExpiry.Visible = PGPS.ProductExpDate();
            divPBatch.Visible = PGPS.ProductBatch();

            divRate.Visible = true;
            divQuantity.Visible = true;
            btnAdd.Visible = true;
            btnAddandContinue.Visible = true;
            #endregion
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string currentFY = ddlFiscalYear.SelectedValue;
        string previousFY = PGD.getPrevious_FiscalYear(currentFY);

        if (txtQty.Text != "" && txtRate.Text != "")
        {

            #region for current year
            OBEnt = new OPENING_BALANCE();
            OBEnt.PRODUCT_ID = ddlproduct.SelectedValue;
            OBEnt.FISCAL_YEAR = currentFY;
            OBEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
            OBEnt = (OPENING_BALANCE)OBSer.GetSingle(OBEnt);
            if (OBEnt != null)
            {
                PEnt = new PRODUCT();
                PEnt.PK_ID = OBEnt.PRODUCT_ID;
                PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                if (PEnt != null)
                {

                    if (PEnt.SIZE_STATUS == "1")
                        OBEnt.SIZE_ID = ddlProductSize.SelectedValue;

                    if (PEnt.MANUFACTURE_STATUS == "1")
                        if (ddlProductManufacturer.SelectedItem.Text != "-")
                            OBEnt.MANUFACTURER_ID = ddlProductManufacturer.SelectedValue;

                    if (PEnt.EXPIRY_STATUS == "1")
                        OBEnt.EXPIRY_DATE = txtExpiryDate.Text;

                    if (PEnt.BATCH_STATUS == "1")
                        OBEnt.BATCH_NUMBER = txtBatch.Text;
                }
                OBEnt.QUANTITY = txtQty.Text;
                OBEnt.RATE = txtRate.Text;
                try
                {
                    double closing_value;
                    closing_value = Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtRate.Text);
                    OBEnt.CLOSING_VALUE = closing_value.ToString();
                }
                catch
                {
                    OBEnt.CLOSING_VALUE = "";
                }
                OBSer.Update(OBEnt);
            }
            else
            {
                OBEnt = new OPENING_BALANCE();
                OBEnt.PRODUCT_ID = ddlproduct.SelectedValue;
                OBEnt.FISCAL_YEAR = currentFY;
                PEnt = new PRODUCT();
                PEnt.PK_ID = ddlproduct.SelectedValue;
                PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                if (PEnt != null)
                {
                    if (PEnt.SIZE_STATUS == "1")
                        OBEnt.SIZE_ID = ddlProductSize.SelectedValue;

                    if (PEnt.MANUFACTURE_STATUS == "1")
                        if (ddlProductManufacturer.SelectedItem.Text != "-")
                            OBEnt.MANUFACTURER_ID = ddlProductManufacturer.SelectedValue;

                    if (PEnt.EXPIRY_STATUS == "1")
                        OBEnt.EXPIRY_DATE = txtExpiryDate.Text;

                    if (PEnt.BATCH_STATUS == "1")
                        OBEnt.BATCH_NUMBER = txtBatch.Text;
                }
                OBEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
                OBEnt.QUANTITY = txtQty.Text;
                OBEnt.RATE = txtRate.Text;
                try
                {
                    double closing_value;
                    closing_value = Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtRate.Text);
                    OBEnt.CLOSING_VALUE = closing_value.ToString();
                }
                catch
                {
                    OBEnt.CLOSING_VALUE = "";
                }
                OBSer.Insert(OBEnt);
            }
            #endregion

            #region for previous year
            OBEnt = new OPENING_BALANCE();
            OBEnt.PRODUCT_ID = ddlproduct.SelectedValue;
            OBEnt.FISCAL_YEAR = previousFY;
            OBEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
            OBEnt = (OPENING_BALANCE)OBSer.GetSingle(OBEnt);
            if (OBEnt != null)
            {
                PEnt = new PRODUCT();
                PEnt.PK_ID = OBEnt.PRODUCT_ID;
                PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                if (PEnt != null)
                {

                    if (PEnt.SIZE_STATUS == "1")
                        OBEnt.SIZE_ID = ddlProductSize.SelectedValue;

                    if (PEnt.MANUFACTURE_STATUS == "1")
                        if (ddlProductManufacturer.SelectedItem.Text != "-")
                            OBEnt.MANUFACTURER_ID = ddlProductManufacturer.SelectedValue;

                    if (PEnt.EXPIRY_STATUS == "1")
                        OBEnt.EXPIRY_DATE = txtExpiryDate.Text;

                    if (PEnt.BATCH_STATUS == "1")
                        OBEnt.BATCH_NUMBER = txtBatch.Text;
                }
                OBEnt.QUANTITY = txtQty.Text;
                OBEnt.RATE = txtRate.Text;
                try
                {
                    double closing_value;
                    closing_value = Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtRate.Text);
                    OBEnt.CLOSING_VALUE = closing_value.ToString();
                }
                catch
                {
                    OBEnt.CLOSING_VALUE = "";
                }
                OBSer.Update(OBEnt);
            }
            else
            {
                OBEnt = new OPENING_BALANCE();
                OBEnt.PRODUCT_ID = ddlproduct.SelectedValue;
                OBEnt.FISCAL_YEAR = previousFY;
                PEnt = new PRODUCT();
                PEnt.PK_ID = ddlproduct.SelectedValue;
                PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                if (PEnt != null)
                {
                    if (PEnt.SIZE_STATUS == "1")
                        OBEnt.SIZE_ID = ddlProductSize.SelectedValue;

                    if (PEnt.MANUFACTURE_STATUS == "1")
                        if (ddlProductManufacturer.SelectedItem.Text != "-")
                            OBEnt.MANUFACTURER_ID = ddlProductManufacturer.SelectedValue;

                    if (PEnt.EXPIRY_STATUS == "1")
                        OBEnt.EXPIRY_DATE = txtExpiryDate.Text;

                    if (PEnt.BATCH_STATUS == "1")
                        OBEnt.BATCH_NUMBER = txtBatch.Text;
                }
                OBEnt.QUANTITY = txtQty.Text;
                OBEnt.RATE = txtRate.Text;
                try
                {
                    double closing_value;
                    closing_value = Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtRate.Text);
                    OBEnt.CLOSING_VALUE = closing_value.ToString();
                }
                catch
                {
                    OBEnt.CLOSING_VALUE = "";
                }
                OBEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
                OBSer.Insert(OBEnt);
            }
            #endregion
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Quantity and Rate cannot be empty!");
        }

        LoadOpeningBalance("post");
    }
    protected void ClearSolo()
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
        divPSize.Visible = false;
        divPManufacturer.Visible = false;
        divPExpiry.Visible = false;
        divPBatch.Visible = false;
        divQuantity.Visible = false;
        btnAdd.Visible = false;
        btnAddandContinue.Visible = false;
    }


    protected void ddlCategoryFilterSolo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductSubCategory();
        LoadProduct();
    }

    protected void ddlSubCategoryFilterSolo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProduct();
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        if (ddlCategoryFilterSolo.Text != "Select")
            LoadOpeningBalance("pre");
    }

    protected void LoadOpeningBalance(string loadoption)
    {
        if (loadoption == "pre")
        {
            OBEnt = new OPENING_BALANCE();
            OBEnt.CATEGORY_ID = ddlCategoryFilterSolo.SelectedValue;
            OBEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
            OBEnt.FISCAL_YEAR = ddlFiscalYear.SelectedValue;
            if (ddlSubCategoryFilterSolo.SelectedValue != "Select")
                OBEnt.SUB_CATEGORY_ID = ddlSubCategoryFilterSolo.SelectedValue;
            grdOpeningBalance.DataSource = OBSer.GetAll(OBEnt);
            grdOpeningBalance.DataBind();
        }
        else
        {
            OBEnt = new OPENING_BALANCE();
            OBEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
            OBEnt.CATEGORY_ID = ddlProductCategory.SelectedValue;
            OBEnt.FISCAL_YEAR = ddlFiscalYear.SelectedValue;
            if (ddlProductSubCategory.SelectedValue != "Select")
                OBEnt.SUB_CATEGORY_ID = ddlProductSubCategory.SelectedValue;
            if (ddlproduct.SelectedValue != "Select")
                OBEnt.PRODUCT_ID = ddlproduct.SelectedValue;
            grdOpeningBalance.DataSource = OBSer.GetAll(OBEnt);
            grdOpeningBalance.DataBind();
        }
        if (hf.ProductColor() == "1")
            grdOpeningBalance.Columns[3].Visible = true;
        else
            grdOpeningBalance.Columns[3].Visible = false;

        if (hf.ProductSize() == "1")
            grdOpeningBalance.Columns[4].Visible = true;
        else
            grdOpeningBalance.Columns[4].Visible = false;

        if (hf.ProductManufacturer() == "1")
            grdOpeningBalance.Columns[5].Visible = true;
        else
            grdOpeningBalance.Columns[5].Visible = false;
        if (hf.ProductExpiry() == "1")
            grdOpeningBalance.Columns[7].Visible = true;
        else
            grdOpeningBalance.Columns[7].Visible = false;
        divGridSolo.Visible = true;
        divFilterSolo.Visible = false;
        divAddSolo.Visible = false;
    }
    protected void grdOpeningBalance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblProductCode = e.Row.FindControl("lblProductCode") as Label;
                Label lblProductId = e.Row.FindControl("lblProductId") as Label;
                Label lblProductName = e.Row.FindControl("lblProductName") as Label;
                Label lblColourId = e.Row.FindControl("lblColourId") as Label;
                Label lblColourName = e.Row.FindControl("lblColourName") as Label;

                Label lblManufacturerId = e.Row.FindControl("lblManufacturerId") as Label;
                Label lblManufacturerName = e.Row.FindControl("lblManufacturerName") as Label;

                PEnt = new PRODUCT();
                PEnt.PK_ID = lblProductId.Text;
                PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                if (PEnt != null)
                {
                    lblProductName.Text = PEnt.PRODUCT_NAME;
                    lblProductCode.Text = PEnt.PRODUCT_CODE;
                }

                PMEnt = new PRODUCT_MANUFACTURE();
                PMEnt.PK_ID = lblManufacturerId.Text;
                PMEnt = (PRODUCT_MANUFACTURE)PMSer.GetSingle(PMEnt);
                if (PMEnt != null)
                {
                    lblManufacturerName.Text = PMEnt.MANUFACTURE_NAME;
                }
            }
        }
    }

    protected void btnAddDivSolo_Click(object sender, EventArgs e)
    {
        divAddSolo.Visible = true;
        divFilterSolo.Visible = false;
        divGridSolo.Visible = false;
        divRate.Visible = false;
        lblOBPK_ID.Text = "";
        ddlProductCategory.Enabled = true;
        ddlProductSubCategory.Enabled = true;
        ddlproduct.Enabled = true;
        ClearSolo();
    }

    protected void btnFilterSolo_Click(object sender, EventArgs e)
    {
        divAddSolo.Visible = false;
        divFilterSolo.Visible = true;
        divGridSolo.Visible = false;

    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((Button)sender).Parent.Parent as GridViewRow;
        Label lblpk_id = gr.FindControl("lblPK_ID") as Label;
        #region to load data from opening balance if exist

        OBEnt = new OPENING_BALANCE();
        OBEnt.PK_ID = lblpk_id.Text;
        OBEnt = (OPENING_BALANCE)OBSer.GetSingle(OBEnt);
        if (OBEnt != null)
        {

            lblOBPK_ID.Text = lblpk_id.Text;
            LoadProductCategory();
            ddlProductCategory.SelectedValue = OBEnt.CATEGORY_ID;
            LoadProductSubCategory();

            if (OBEnt.SUB_CATEGORY_ID != "0")
                ddlProductSubCategory.SelectedValue = OBEnt.SUB_CATEGORY_ID;
            LoadProduct();
            ddlproduct.SelectedValue = OBEnt.PRODUCT_ID;

            PEnt = new PRODUCT();
            PEnt.PK_ID = OBEnt.PRODUCT_ID;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                lblPackQty.Text = PEnt.PACK_QTY;
                PUEnt = new PRODUCT_UNIT();
                PUEnt.PK_ID = PEnt.UNIT_ID;
                PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
                if (PUEnt != null)
                    lblUnit.Text = PUEnt.UNIT_NAME;
                divUnit.Visible = true;
                divPackQty.Visible = true;



                if (PEnt.SIZE_STATUS == "1")
                {
                    LoadSize();
                    divPSize.Visible = true;
                    ddlProductSize.SelectedValue = OBEnt.SIZE_ID;
                }
                else
                    divPSize.Visible = false;

                if (PEnt.MANUFACTURE_STATUS == "1")
                {
                    LoadManufacturer();
                    divPManufacturer.Visible = true;
                    if (OBEnt.MANUFACTURER_ID != "" && OBEnt.MANUFACTURER_ID != null)
                    {
                        ddlProductManufacturer.SelectedValue = OBEnt.MANUFACTURER_ID;
                    }
                    else
                    {
                        ddlProductManufacturer.SelectedItem.Text = "-";

                    }
                }
                else
                    divPManufacturer.Visible = false;

                if (PEnt.EXPIRY_STATUS == "1")
                {
                    txtExpiryDate.Text = OBEnt.EXPIRY_DATE;
                    divPExpiry.Visible = true;
                }
                else
                    divPExpiry.Visible = false;

                if (PEnt.BATCH_STATUS == "1")
                {
                    divPBatch.Visible = true;
                    txtBatch.Text = OBEnt.BATCH_NUMBER;
                }
                else
                    divPBatch.Visible = false;
            }
            divQuantity.Visible = true;
            divRate.Visible = true;
            txtQty.Text = OBEnt.QUANTITY;
            txtRate.Text = OBEnt.RATE;
            btnAdd.Visible = true;

        }
        #endregion
        divAddSolo.Visible = true;
        divFilterSolo.Visible = false;
        divGridSolo.Visible = false;
        ddlProductCategory.Enabled = false;
        ddlProductSubCategory.Enabled = false;
        ddlproduct.Enabled = false;
        btnAddandContinue.Visible = false;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((Button)sender).Parent.Parent as GridViewRow;
        Label lblpk_id = gr.FindControl("lblPK_ID") as Label;
        #region to load data from opening balance if exist
        OBEnt = new OPENING_BALANCE();
        OBEnt.PK_ID = lblpk_id.Text;
        OBEnt = (OPENING_BALANCE)OBSer.GetSingle(OBEnt);
        if (OBEnt != null)
        {
            OBSer.Delete(OBEnt);
        }
        #endregion

        LoadOpeningBalance("post");
    }

    protected void btnAddandContinue_Click(object sender, EventArgs e)
    {
        string currentFY = ddlFiscalYear.SelectedValue;
        string previousFY = PGD.getPrevious_FiscalYear(currentFY);
        if (txtQty.Text != "" && txtRate.Text != "")
        {
            #region for current year
            OBEnt = new OPENING_BALANCE();
            OBEnt.PRODUCT_ID = ddlproduct.SelectedValue;
            OBEnt.FISCAL_YEAR = currentFY;
            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlproduct.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                if (PEnt.SIZE_STATUS == "1")
                    OBEnt.SIZE_ID = ddlProductSize.SelectedValue;

                if (PEnt.MANUFACTURE_STATUS == "1")
                    OBEnt.MANUFACTURER_ID = ddlProductManufacturer.SelectedValue;

                if (PEnt.EXPIRY_STATUS == "1")
                    OBEnt.EXPIRY_DATE = txtExpiryDate.Text;

                if (PEnt.BATCH_STATUS == "1")
                    OBEnt.BATCH_NUMBER = txtBatch.Text;
            }
            OBEnt.QUANTITY = txtQty.Text;

            OBSer.Insert(OBEnt);

            #endregion

            #region for previous year

            OBEnt = new OPENING_BALANCE();
            OBEnt.PRODUCT_ID = ddlproduct.SelectedValue;
            OBEnt.FISCAL_YEAR = previousFY;
            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlproduct.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                if (PEnt.SIZE_STATUS == "1")
                    OBEnt.SIZE_ID = ddlProductSize.SelectedValue;

                if (PEnt.MANUFACTURE_STATUS == "1")
                    OBEnt.MANUFACTURER_ID = ddlProductManufacturer.SelectedValue;

                if (PEnt.EXPIRY_STATUS == "1")
                    OBEnt.EXPIRY_DATE = txtExpiryDate.Text;

                if (PEnt.BATCH_STATUS == "1")
                    OBEnt.BATCH_NUMBER = txtBatch.Text;
            }
            OBEnt.QUANTITY = txtQty.Text;

            OBSer.Insert(OBEnt);

            #endregion
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Quantity and Rate cannot be empty !");
        }

        txtQty.Text = "";
    }
    #endregion

    protected void btnExcelUpload_Click(object sender, EventArgs e)
    {
        #region to import excel file
        if (FileExcelUp.HasFile)
        {
            // Save uploaded file to server
            string folderPath = Server.MapPath("~/Uploads/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(folderPath, Path.GetFileName(FileExcelUp.FileName));
            FileExcelUp.SaveAs(filePath);
            btnSave.Visible = true;
            // Set the LicenseContext property for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.Commercial; // Use LicenseContext.NonCommercial if applicable

            // Load the Excel file
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Select the first worksheet
                DataTable dt = new DataTable();

                // Read the header
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    dt.Columns.Add(worksheet.Cells[1, col].Text);
                }

                // Read the rest of the data
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    DataRow dr = dt.NewRow();
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        dr[col - 1] = worksheet.Cells[row, col].Text;
                    }
                    dt.Rows.Add(dr);
                }

                // Bind data to GridView
                GridView1.DataSource = dt;
                GridView1.DataBind();

            }
        }
        #endregion

    }
    protected void DownloadFile(object sender, EventArgs e)
    {
        string filePath = Server.MapPath("~/Uploads/OpeningBalanceSample.xlsx");

        if (File.Exists(filePath))
        {
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("Content-Disposition", "attachment; filename=OpeningBalanceSample.xlsx");
            Response.TransmitFile(filePath);
            Response.End();
        }
        else
        {
            Response.Write("File not found!");
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count > 0)
        {
            try
            {
                foreach (GridViewRow row in GridView1.Rows)
                {
                    string column1Value = row.Cells[0].Text.Replace("&nbsp;", "").Trim(); // ProductCode
                    string column2Value = row.Cells[1].Text.Replace("&nbsp;", "").Trim(); // Product Name
                    string column3Value = row.Cells[2].Text.Replace("&nbsp;", "").Trim(); // Expiry Date
                    string column4Value = row.Cells[3].Text.Replace("&nbsp;", "").Trim(); // Batch No
                    string column5Value = row.Cells[4].Text.Replace("&nbsp;", "").Trim(); // Quantity
                    string column6Value = row.Cells[5].Text.Replace("&nbsp;", "").Trim(); // Rate

                    if (!string.IsNullOrWhiteSpace(column1Value)) // Ensure ProductCode is valid
                    {
                        // Replace null/empty values with 0 for Quantity and Rate
                        column5Value = string.IsNullOrWhiteSpace(column5Value) ? "0" : column5Value;
                        column6Value = string.IsNullOrWhiteSpace(column6Value) ? "0" : column6Value;

                        // Convert to double safely
                        double quantity = Convert.ToDouble(column5Value);
                        double rate = Convert.ToDouble(column6Value);
                        double closing_value = (quantity == 0 || rate == 0) ? 0 : (quantity * rate);

                        PEnt = new PRODUCT { PRODUCT_CODE = column1Value };
                        PEnt = (PRODUCT)PSer.GetSingle(PEnt);

                        if (PEnt != null)
                        {
                            OBEnt = new OPENING_BALANCE { PRODUCT_ID = PEnt.PK_ID };

                            if (!string.IsNullOrWhiteSpace(column4Value)) // Check if batch number is available
                            {
                                OBEnt.BATCH_NUMBER = column4Value;
                            }

                            OBEnt = (OPENING_BALANCE)OBSer.GetSingle(OBEnt);

                            if (OBEnt != null)
                            {
                                // Skip update if Batch Number or Expiry Date is null, empty, or whitespace
                                if (string.IsNullOrWhiteSpace(column3Value) || string.IsNullOrWhiteSpace(column4Value))
                                    continue;

                                OBEnt.EXPIRY_DATE = column3Value;
                                OBEnt.BATCH_NUMBER = column4Value;
                                OBEnt.QUANTITY = quantity.ToString();
                                OBEnt.RATE = rate.ToString();
                                OBEnt.CLOSING_VALUE = closing_value.ToString();
                                OBEnt.FISCAL_YEAR = lblFY.Text;

                                OBSer.Update(OBEnt);
                            }
                            else
                            {
                                // Skip insert if Batch Number or Expiry Date is null, empty, or whitespace
                                if (string.IsNullOrWhiteSpace(column3Value) || string.IsNullOrWhiteSpace(column4Value))
                                    continue;

                                OBEnt = new OPENING_BALANCE
                                {
                                    PRODUCT_ID = PEnt.PK_ID,
                                    EXPIRY_DATE = column3Value,
                                    BATCH_NUMBER = column4Value,
                                    QUANTITY = quantity.ToString(),
                                    RATE = rate.ToString(),
                                    CLOSING_VALUE = closing_value.ToString(),
                                    FISCAL_YEAR = lblFY.Text


                                };

                                OBSer.Insert(OBEnt);
                            }
                        }

                    }
                }

                HelperFunction.MsgBox(this, this.GetType(), "Uploaded Successfully");

            }
            catch
            {
                HelperFunction.MsgBox(this, this.GetType(), "Error in format, Please try again");
            }
        }

    }


    protected void btnExcelUp_Click(object sender, EventArgs e)
    {
        divexcleUp.Visible = true;
        divWhole.Visible = false;
        divSolo.Visible = false;
    }

    protected void btnbackE_Click(object sender, EventArgs e)
    {

        string script = "window.history.back();";
        ScriptManager.RegisterStartupScript(this, GetType(), "GoBack", script, true);


    }

    protected void ddlBranchFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadFY();
        LoadFYSolo();
        checkFY();
        LoadProduct();
        divAddSolo.Visible = false;
    }
}