using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Entity.Framework;
using PhyeGanCore;
using System.IO;
using BarcodeLib;
using System.Drawing.Imaging;
using System.Drawing;
using System.Web.UI;
using System.Web;

public partial class Administration_Product_Maapping : System.Web.UI.Page
{
    PRODUCT_CATEGORY PCEnt = new PRODUCT_CATEGORY();
    PRODUCT_CATEGORYService PCSer = new PRODUCT_CATEGORYService();

    PRODUCT_SUB_CATEGORY PSCEnt = new PRODUCT_SUB_CATEGORY();
    PRODUCT_SUB_CATEGORYService PSCSer = new PRODUCT_SUB_CATEGORYService();

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();

    PhyeGan PG = new PhyeGan();
    HelperFunction hf = new HelperFunction();

    ActiveProductType APT = new ActiveProductType();

    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();

    EMPLOYEES Ent = new EMPLOYEES();
    EMPLOYEESService ESer = new EMPLOYEESService();

    OFFICE OEnt = new OFFICE();
    OFFICEService OSer = new OFFICEService();

    PRODUCT_MANUFACTURE PMEnt = new PRODUCT_MANUFACTURE();
    PRODUCT_MANUFACTUREService PMSer = new PRODUCT_MANUFACTUREService();

    PRODUCT_RATE_TYPE PRTEnt = new PRODUCT_RATE_TYPE();
    PRODUCT_RATE_TYPEService PRTSer = new PRODUCT_RATE_TYPEService();

    PRODUCT_RATES PREnt = new PRODUCT_RATES();
    PRODUCT_RATEService PRSer = new PRODUCT_RATEService();

    PRODUCT_TYPE PTEnt = new PRODUCT_TYPE();
    PRODUCT_TYPEService PTSer = new PRODUCT_TYPEService();

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
                    LoadProductCategory();
                    LoadProductList();
                    LoadProductType();
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

    protected void LoadProductType()
    {
        ddlProTypeF.DataSource = APT.GetActiveProductType();
        ddlProTypeF.DataTextField = "PRODUCT_TYPE_NAME";
        ddlProTypeF.DataValueField = "PK_ID";
        ddlProTypeF.DataBind();
    }
    protected void LoadProductCategory()
    {
        PCEnt = new PRODUCT_CATEGORY();
        PCEnt.STATUS = "1";
        ddlProductCategoryF.DataSource = PCSer.GetAll(PCEnt);
        ddlProductCategoryF.DataTextField = "CATEGORY_NAME";
        ddlProductCategoryF.DataValueField = "PK_ID";
        ddlProductCategoryF.DataBind();
        ddlProductCategoryF.Items.Insert(0, "Select");

    }

    protected void LoadProductSubCategory()
    {
        PSCEnt = new PRODUCT_SUB_CATEGORY();
        PSCEnt.CATEGORY_ID = ddlProductCategoryF.SelectedValue;
        PSCEnt.STATUS = "1";
        ddlProductSubCategoryF.DataSource = PSCSer.GetAll(PSCEnt);
        ddlProductSubCategoryF.DataTextField = "SUB_CATEGORY_NAME";
        ddlProductSubCategoryF.DataValueField = "PK_ID";
        ddlProductSubCategoryF.DataBind();
        ddlProductSubCategoryF.Items.Insert(0, "Select");
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
            trBranch.Visible = true;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
        }
        else
        {
            trBranch.Visible = false;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
        }
    }
    protected void ddlProductCategoryF_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductSubCategory();
    }
    protected void ddlProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductSubCategory();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadProduct();
    }
    protected void LoadProduct()
    {
        grid.DataSource = null;
        grid.DataBind();
        PEnt = new PRODUCT();
        if (ddlProductSubCategoryF.SelectedValue != "Select" || ddlProductList.SelectedValue != "Select")
        {
            PEnt.SUB_CATEGORY_ID = ddlProductSubCategoryF.SelectedValue;
        }
        else
            PEnt.SUB_CATEGORY_ID = "0";
        PEnt.PRODUCT_TYPE_ID = ddlProTypeF.SelectedValue;
        PEnt.OFFICE_CODE = "-";
        //PEnt.STATUS = "1";
        grid.DataSource = PSer.GetAll(PEnt);
        grid.DataBind();

        divGrid.Visible = true;
        btnSave.Visible = true;

    }
    protected void LoadProductList()
    {
        PEnt = new PRODUCT();
        PEnt.STATUS = "1";
        PEnt.OFFICE_CODE = "-";
        ddlProductList.DataSource = PSer.GetAll(PEnt);
        ddlProductList.DataTextField = "PRODUCT_FULLNAME";
        ddlProductList.DataValueField = "PK_ID";
        ddlProductList.DataBind();
        ddlProductList.Items.Insert(0, "Select");

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //if (chkDualUnit.Checked == true)
        //{
        //    double packqty = 0;
        //    try
        //    {
        //        packqty = Convert.ToDouble(txtPackQty.Text);
        //    }
        //    catch
        //    {
        //        txtPackQty.Text = "0";
        //    }
        //}
        //SaveData();
        //divAdd.Visible = false;
        //divGrid.Visible = true;
        //divAddFilter.Visible = true;
        //divProductImage.Visible = false;
        //ClearFields();
        //LoadProductSubCategory();
        //ddlProductCategoryF.SelectedValue = ddlProductCategory.SelectedValue;
        //ddlProductSubCategoryF.SelectedValue = ddlProductSubCategory.SelectedValue;
        //LoadProduct();
        //Response.Redirect(Request.RawUrl);
        //LoadProductList();

    }


    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblPK_ID = e.Row.FindControl("lblPK_ID") as Label;
                Label lblCategoryId = e.Row.FindControl("lblCategoryId") as Label;
                Label lblCategory = e.Row.FindControl("lblCategory") as Label;
                Label lblSubCategoryId = e.Row.FindControl("lblSubCategoryId") as Label;
                Label lblSubCategory = e.Row.FindControl("lblSubCategory") as Label;
                Label lblBasicUnitID = e.Row.FindControl("lblBasicUnitID") as Label;
                Label lblBasicUnit = e.Row.FindControl("lblBasicUnit") as Label;
                Label lblUpperUnitID = e.Row.FindControl("lblUpperUnitID") as Label;
                Label lblUpperUnit = e.Row.FindControl("lblUpperUnit") as Label;
                Label lblStatusID = e.Row.FindControl("lblStatusID") as Label;
                Label lblStatus = e.Row.FindControl("lblStatus") as Label;
                Label lblManufName = e.Row.FindControl("lblManufName") as Label;
                Label lblShowName = e.Row.FindControl("lblShowName") as Label;
                Label lblProductType = e.Row.FindControl("lblProductType") as Label;
                Label lblShowProductType = e.Row.FindControl("lblShowProductType") as Label;
                System.Web.UI.WebControls.Image productImg = e.Row.FindControl("productImg") as System.Web.UI.WebControls.Image;
                System.Web.UI.WebControls.Image barcodeImg = e.Row.FindControl("barcodeImg") as System.Web.UI.WebControls.Image;
                Label lblProductCode = e.Row.FindControl("lblProductCode") as Label;
                CheckBox chkInclude = e.Row.FindControl("chkInclude") as CheckBox;

                #region for Category, Units, Upper Units and Status
                PCEnt = new PRODUCT_CATEGORY();
                PCEnt.PK_ID = lblCategoryId.Text;
                PCEnt = (PRODUCT_CATEGORY)PCSer.GetSingle(PCEnt);
                if (PCEnt != null)
                    lblCategory.Text = PCEnt.CATEGORY_NAME;

                PSCEnt = new PRODUCT_SUB_CATEGORY();
                PSCEnt.PK_ID = lblSubCategoryId.Text;
                PSCEnt = (PRODUCT_SUB_CATEGORY)PSCSer.GetSingle(PSCEnt);
                if (PSCEnt != null && lblSubCategoryId.Text != "")
                {
                    lblSubCategory.Text = PSCEnt.SUB_CATEGORY_NAME;
                }

                PUEnt = new PRODUCT_UNIT();
                PUEnt.PK_ID = lblBasicUnitID.Text;
                PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
                if (PUEnt != null)
                    lblBasicUnit.Text = PUEnt.UNIT_NAME;

                if (lblUpperUnitID.Text != "")
                {
                    PUEnt = new PRODUCT_UNIT();
                    PUEnt.PK_ID = lblUpperUnitID.Text;
                    PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
                    if (PUEnt != null)
                        lblUpperUnit.Text = "1 " + PUEnt.UNIT_NAME;
                }
                if (lblStatusID.Text == "1")
                {
                    lblStatus.Text = "Available";
                }
                else
                {
                    lblStatus.Text = "Unavailable";
                }
                #endregion
                #region for Product Type
                PTEnt = new PRODUCT_TYPE();
                PTEnt.PK_ID = lblProductType.Text;
                PTEnt = (PRODUCT_TYPE)PTSer.GetSingle(PTEnt);
                if (PTEnt != null)
                {
                    lblShowProductType.Text = PTEnt.PRODUCT_TYPE_NAME;
                }
                PMEnt = new PRODUCT_MANUFACTURE();
                PMEnt.PK_ID = lblManufName.Text;
                PMEnt = (PRODUCT_MANUFACTURE)PMSer.GetSingle(PMEnt);
                if (PMEnt != null)
                {
                    lblShowName.Text = PMEnt.MANUFACTURE_NAME;
                }
                #endregion
                #region for Image and Barcode
                string folderVirtualPath = "~/images/Product_image/";
                string folderPhysicalPath = Server.MapPath(folderVirtualPath);
                string[] supportedExtensions = { ".png", ".jpg", ".jpeg" };
                string imgVirtualPath;
                foreach (var extension in supportedExtensions)
                {
                    string fileName = "Product_image" + lblProductCode.Text + extension;
                    string filePhysicalPath = System.IO.Path.Combine(folderPhysicalPath, fileName);

                    if (File.Exists(filePhysicalPath))
                    {
                        imgVirtualPath = folderVirtualPath + fileName;
                        productImg.ImageUrl = imgVirtualPath;
                        //divImg.Visible = true;
                    }
                }

                string folderVirtualPaths = "~/images/BarCode/Product/";
                string folderPhysicalPaths = Server.MapPath(folderVirtualPaths);

                string imgVirtualPaths = string.Empty;

                // Generate the file name based on the product code
                string fileNames = lblProductCode.Text + ".jpg";

                // Combine the physical path for the file
                string filePhysicalPaths = Path.Combine(folderPhysicalPaths, fileNames);

                // Check if the file exists
                if (File.Exists(filePhysicalPaths))
                {
                    // If the file exists, set the virtual path for the image
                    imgVirtualPaths = folderVirtualPaths + fileNames;

                    // Resolve the virtual path and set the ImageUrl
                    barcodeImg.ImageUrl = ResolveUrl(imgVirtualPaths);
                }
                #endregion
                #region for Including in the branch(Checkbox)
                PEnt = new PRODUCT();
                PEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                PEnt.PRODUCT_CODE = lblProductCode.Text;
                PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                if (PEnt != null)
                {
                    if (PEnt.STATUS == "1")
                    {
                        chkInclude.Checked = true;
                    }
                    else
                    {
                        chkInclude.Checked = false;
                    }
                }
                else
                {
                    chkInclude.Checked = false;
                }
                #endregion

            }
        }
    }
    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string product_pk_id;
        //if (e.CommandName.Equals("Change"))
        //{
        //    ClearFields(); ;
        //    GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
        //    Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
        //    lblPKIDU.Text = lblPK_ID.Text;
        //    product_pk_id = lblPK_ID.Text;
        //    PEnt = new PRODUCT();
        //    PEnt.PK_ID = lblPK_ID.Text;
        //    PEnt = (PRODUCT)PSer.GetSingle(PEnt);
        //    if (PEnt != null)
        //    {
        //        txtPCode.Text = PEnt.PRODUCT_CODE;
        //        ddlProductCategory.SelectedValue = PEnt.CATEGORY_ID;
        //        if (PEnt.SUB_CATEGORY_ID != "0")
        //        {
        //            LoadProductSubCategory();
        //            ddlProductSubCategory.SelectedValue = PEnt.SUB_CATEGORY_ID;
        //        }
        //        txtPName.Text = PEnt.PRODUCT_NAME;
        //        txtPackQty.Text = PEnt.PACK_QTY;
        //        ddlUnit.SelectedValue = PEnt.UNIT_ID;
        //        txtHSCode.Text = PEnt.HS_CODE;
        //        ddlPriceCategory.SelectedValue = PEnt.PRICE_CATEGORY;
        //        ddlExpiryStatus.SelectedValue = PEnt.EXPIRY_STATUS;
        //        ddlProType.SelectedValue = PEnt.PRODUCT_TYPE_ID;
        //        ddlProductSize.SelectedValue = PEnt.SIZE_STATUS;
        //        ddlProductManufacturer.SelectedValue = PEnt.MANUFACTURE_STATUS;
        //        ddlProductBatch.SelectedValue = PEnt.BATCH_STATUS;
        //        ddlAvailability.SelectedValue = PEnt.STATUS;
        //        ddlTaxable.SelectedValue = PEnt.TAX_STATUS;
        //        if (PEnt.DUAL_UNIT == "1")
        //        {
        //            divDualUnit.Visible = true;
        //            chkDualUnit.Checked = true;
        //            ddlUpperUnit.SelectedValue = PEnt.UPPER_UNIT_ID;
        //            txtPackQty.Text = PEnt.PACK_QTY;
        //            divDualUnit.Visible = true;
        //            lblUpperQtyLable.Text = "Qty in 1 " + ddlUpperUnit.SelectedItem.ToString();
        //        }

        //        if (PEnt.IS_SERVICE == "1")
        //        {
        //            chkIsService.Checked = true;
        //        }
        //        else
        //        {
        //            chkIsService.Checked = false;
        //        }
        //    }
        //    divGrid.Visible = false;
        //    divAdd.Visible = true;
        //    divAddFilter.Visible = false;
        //    divProductImage.Visible = true;
        //showProductImg(product_pk_id);
    }


    protected void ddlProductList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProductList.SelectedValue != "Select")
        {
            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlProductList.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                txtProductCode.Text = PEnt.PRODUCT_CODE;
            }
        }
        else
        {
            txtProductCode.Text = "";
            txtProductCode.Focus();
        }
    }

    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        txtProductCode.Text = txtProductCode.Text.ToUpper();
        PEnt = new PRODUCT();
        PEnt.PRODUCT_CODE = txtProductCode.Text;
        PEnt = (PRODUCT)PSer.GetSingle(PEnt);
        if (PEnt != null && txtProductCode.Text != "")
        {
            ddlProductList.SelectedValue = PEnt.PK_ID;

        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Not a valid Product Code.");
            txtProductCode.Text = "";
            txtProductCode.Focus();
            ddlProductList.SelectedValue = "Select";

        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grid.DataSource = null;
        grid.DataBind();
        PEnt = new PRODUCT();

        PEnt.PK_ID = ddlProductList.SelectedValue;
        grid.DataSource = PSer.GetAll(PEnt);
        grid.DataBind();

        divGrid.Visible = true;
        btnSave.Visible = true;
    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        string status = "";
        foreach (GridViewRow gr in grid.Rows)
        {
            Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
            Label lblProductCode = gr.FindControl("lblProductCode") as Label;
            CheckBox chkInclude = gr.FindControl("chkInclude") as CheckBox;

            PEnt = new PRODUCT();
            PEnt.PRODUCT_CODE = lblProductCode.Text;
            PEnt.OFFICE_CODE = ddlBranch.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                if (chkInclude.Checked)
                {                    
                    PEnt.STATUS = "1";
                }
                else
                {
                    PEnt.STATUS = "0";
                }
                PSer.Update(PEnt);                
            }
            else
            {
                if (chkInclude.Checked)
                {
                    PEnt = new PRODUCT();
                    PRODUCT tempEnt = new PRODUCT();
                    tempEnt.PK_ID = lblPK_ID.Text;
                    tempEnt = (PRODUCT)PSer.GetSingle(tempEnt);
                    if (tempEnt != null)
                    {
                        PEnt.PRODUCT_CODE = tempEnt.PRODUCT_CODE;
                        PEnt.CATEGORY_ID = tempEnt.CATEGORY_ID;
                        PEnt.SUB_CATEGORY_ID = tempEnt.SUB_CATEGORY_ID;
                        PEnt.PRODUCT_NAME = tempEnt.PRODUCT_NAME;
                        PEnt.PACK_QTY = tempEnt.PACK_QTY;
                        PEnt.UNIT_ID = tempEnt.UNIT_ID;
                        PEnt.PRICE_CATEGORY = tempEnt.PRICE_CATEGORY;
                        PEnt.EXPIRY_STATUS = tempEnt.EXPIRY_STATUS;
                        PEnt.STATUS = "1";
                        PEnt.COLOUR_STATUS = tempEnt.COLOUR_STATUS;
                        PEnt.SIZE_STATUS = tempEnt.SIZE_STATUS;
                        PEnt.MANUFACTURE_STATUS = tempEnt.MANUFACTURE_STATUS;
                        PEnt.BATCH_STATUS = tempEnt.BATCH_STATUS;
                        PEnt.TAX_STATUS = tempEnt.TAX_STATUS;
                        PEnt.DUAL_UNIT = tempEnt.DUAL_UNIT;
                        PEnt.UPPER_UNIT_ID = tempEnt.UPPER_UNIT_ID;
                        PEnt.HS_CODE = tempEnt.HS_CODE;
                        PEnt.PRODUCT_TYPE_ID = tempEnt.PRODUCT_TYPE_ID;
                        PEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                        PSer.Insert(PEnt);
                    }
                }
            }
        }
        HelperFunction.MsgBox(this, this.GetType(), "Data Updated !!");
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProduct();
    }
}
