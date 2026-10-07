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

public partial class Administration_Product : System.Web.UI.Page
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
                    dualUnit.Visible = PGPS.DualQuantity();

                    SetGridVisibility();
                    LoadProductCategory();
                    LoadProductUnit();
                    LoadProductUpperUnit();
                    LoadProductList();
                    if (PG.ProductManufacturerStatus() )
                    {
                        LoadProductType();
                        divPType.Visible = true;
                    }
                    else
                    {
                        divPType.Visible = false;
                    }

                    #region to show hide product property div


                    divPExpiry.Visible = PGPS.ProductExpDate();

                    divPBatch.Visible = PGPS.ProductBatch();

                    divPColour.Visible = PGPS.ProductColor();

                    divPSize.Visible = PGPS.ProductSize();

                    divPManufacturer.Visible = PGPS.ProductManufacture();

                    #endregion
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
        ddlProType.DataSource = APT.GetActiveProductType();
        ddlProType.DataTextField = "PRODUCT_TYPE_NAME";
        ddlProType.DataValueField = "PK_ID";
        ddlProType.DataBind();

        ddlProTypeF.DataSource = APT.GetActiveProductType();
        ddlProTypeF.DataTextField = "PRODUCT_TYPE_NAME";
        ddlProTypeF.DataValueField = "PK_ID";
        ddlProTypeF.DataBind();
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
        PSCEnt.CATEGORY_ID = ddlProductCategory.SelectedValue;
        PSCEnt.STATUS = "1";
        ddlProductSubCategory.DataSource = PSCSer.GetAll(PSCEnt);
        ddlProductSubCategory.DataTextField = "SUB_CATEGORY_NAME";
        ddlProductSubCategory.DataValueField = "PK_ID";
        ddlProductSubCategory.DataBind();
        ddlProductSubCategory.Items.Insert(0, "Select");

        PSCEnt = new PRODUCT_SUB_CATEGORY();
        PSCEnt.CATEGORY_ID = ddlProductCategoryF.SelectedValue;
        PSCEnt.STATUS = "1";
        ddlProductSubCategoryF.DataSource = PSCSer.GetAll(PSCEnt);
        ddlProductSubCategoryF.DataTextField = "SUB_CATEGORY_NAME";
        ddlProductSubCategoryF.DataValueField = "PK_ID";
        ddlProductSubCategoryF.DataBind();
        ddlProductSubCategoryF.Items.Insert(0, "Select");
    }
    protected void LoadProductUnit()
    {
        PUEnt = new PRODUCT_UNIT();
        ddlUnit.DataSource = PUSer.GetAll(PUEnt);
        ddlUnit.DataValueField = "PK_ID";
        ddlUnit.DataTextField = "UNIT_NAME";
        ddlUnit.DataBind();
    }

    protected void LoadProductUpperUnit()
    {
        PUEnt = new PRODUCT_UNIT();
        ddlUpperUnit.DataSource = PUSer.GetAll(PUEnt);
        ddlUpperUnit.DataValueField = "PK_ID";
        ddlUpperUnit.DataTextField = "UNIT_NAME";
        ddlUpperUnit.DataBind();
    }

    protected void btnAddMore_Click(object sender, EventArgs e)
    {
        lblPKIDU.Text = "";
        divAddFilter.Visible = false;
        divAdd.Visible = true;
        divAddFilter.Visible = false;
        divGrid.Visible = false;
        divProductImage.Visible = false;
        ClearFields();
        //txtPCode.Text = hf.getProductCode();
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        divAddFilter.Visible = true;
        divEditProductCode.Visible = true;
        divAdd.Visible = false;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        divAddFilter.Visible = true;
        divAdd.Visible = false;
        divEditProductCode.Visible = false;
        divProductImage.Visible = false;
        LoadProductCategory();
        LoadProductSubCategory();
        ddlProductCategoryF.SelectedValue = ddlProductCategory.SelectedValue;
        ddlProductSubCategoryF.SelectedValue = ddlProductSubCategory.SelectedValue;
        LoadProduct();
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
        if (PG.CompanyBranch_Status())
        {
            PEnt.OFFICE_CODE = "-";
        }
        else
        {
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            PEnt.OFFICE_CODE = userProfileEnt.LocationID;
        }
       
        PEnt.PRODUCT_TYPE_ID = ddlProTypeF.SelectedValue;
        grid.DataSource = PSer.GetAll(PEnt);
        grid.DataBind();

        divGrid.Visible = true;


    }
    protected void LoadProductList()
    {
        PEnt = new PRODUCT();
        // PEnt.PK_ID = ddlProductList.SelectedValue;
        PEnt.STATUS = "1";
        if (PG.CompanyBranch_Status())
        {
            PEnt.OFFICE_CODE = "-";
        }
        else
        {
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            PEnt.OFFICE_CODE = userProfileEnt.LocationID;
        }
        ddlProductList.DataSource = PSer.GetAll(PEnt);
        ddlProductList.DataTextField = "PRODUCT_FULLNAME";
        ddlProductList.DataValueField = "PK_ID";
        ddlProductList.DataBind();
        ddlProductList.Items.Insert(0, "Select");

    }

    protected void ClearFields()
    {
        lblPKIDU.Text = "";
        txtPCode.Text = "";
        txtPName.Text = "";
        txtPackQty.Text = "";
        txtHSCode.Text = "";
        ddlExpiryStatus.SelectedValue = hf.ProductExpiry();// 0- Not Applicable  , 1 -Applicable
        ddlProductColor.SelectedValue = hf.ProductColor();
        ddlProductSize.SelectedValue = hf.ProductSize();
        ddlProductManufacturer.SelectedValue = hf.ProductManufacturer();
        ddlAvailability.SelectedValue = "1";// Available
        chkDualUnit.Checked = false;
        divDualUnit.Visible = false;
        divEditProductCode.Visible = false;
        chkIsService.Checked = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (chkDualUnit.Checked == true)
        {
            double packqty = 0;
            try
            {
                packqty = Convert.ToDouble(txtPackQty.Text);
            }
            catch
            {
                txtPackQty.Text = "0";
            }
        }
        SaveData();
        divAdd.Visible = false;
        divGrid.Visible = true;
        divAddFilter.Visible = true;
        divProductImage.Visible = false;
        ClearFields();
        LoadProductSubCategory();
        ddlProductCategoryF.SelectedValue = ddlProductCategory.SelectedValue;
        ddlProductSubCategoryF.SelectedValue = ddlProductSubCategory.SelectedValue;
        LoadProduct();
        Response.Redirect(Request.RawUrl);
        LoadProductList();

    }

    protected void btnSaveContinue_Click(object sender, EventArgs e)
    {
        SaveData();
        divAdd.Visible = true;
        divGrid.Visible = false;
        divAddFilter.Visible = false;
        divProductImage.Visible = false;
        ClearFields();

    }

    protected void SaveData()
    {
        if (!string.IsNullOrEmpty(txtPName.Text) && !string.IsNullOrEmpty(txtPCode.Text))
        {
            try
            {
                if (lblPKIDU.Text == "")
                {
                    PEnt = new PRODUCT();
                    PEnt.PRODUCT_CODE = txtPCode.Text.ToUpper();
                    PEnt.CATEGORY_ID = ddlProductCategory.SelectedValue;
                    if (ddlProductSubCategory.SelectedValue != "Select")
                        PEnt.SUB_CATEGORY_ID = ddlProductSubCategory.SelectedValue;
                    else
                        PEnt.SUB_CATEGORY_ID = "0";
                    PEnt.PRODUCT_NAME = txtPName.Text.ToUpper();
                    PEnt.HS_CODE = txtHSCode.Text;
                    PEnt.UNIT_ID = ddlUnit.SelectedValue;
                    PEnt.PRICE_CATEGORY = ddlPriceCategory.SelectedValue;
                    PEnt.EXPIRY_STATUS = ddlExpiryStatus.SelectedValue;
                    PEnt.COLOUR_STATUS = ddlProductColor.SelectedValue;
                    if (PG.ProductManufacturerStatus() == true)
                    {
                        PEnt.PRODUCT_TYPE_ID = ddlProType.SelectedValue;
                    }
                    else
                    {
                        PEnt.PRODUCT_TYPE_ID = "1";
                    }
                    PEnt.SIZE_STATUS = ddlProductSize.SelectedValue;
                    if (ddlProductManufacturer.SelectedValue != "Select")
                    {
                        PEnt.MANUFACTURE_STATUS = ddlProductManufacturer.SelectedValue;
                    }

                    PEnt.BATCH_STATUS = ddlProductBatch.SelectedValue;
                    PEnt.STATUS = ddlAvailability.SelectedValue;
                    PEnt.TAX_STATUS = ddlTaxable.SelectedValue;
                    if (chkDualUnit.Checked == true)
                    {
                        PEnt.DUAL_UNIT = "1";
                        PEnt.UPPER_UNIT_ID = ddlUpperUnit.SelectedValue;
                        PEnt.PACK_QTY = txtPackQty.Text;
                    }
                    else
                    {
                        PEnt.DUAL_UNIT = "0";
                        PEnt.UPPER_UNIT_ID = "";
                        PEnt.PACK_QTY = "1";
                    }
                    if (chkIsService.Checked == true)
                    {
                        PEnt.IS_SERVICE = "1";
                    }
                    else
                    {
                        PEnt.IS_SERVICE = "0";
                    }
                    if (PG.CompanyBranch_Status())
                    {
                        PEnt.OFFICE_CODE = "-";
                    }
                    else
                    {
                        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                        PEnt.OFFICE_CODE = userProfileEnt.LocationID;
                    }
                    string pk_id = PSer.Insert(PEnt).ToString();

                    HelperFunction.MsgBox(this, this.GetType(), "Data Saved Sucessfully.");

                    PEnt = new PRODUCT();
                    PEnt.PK_ID = pk_id;
                    PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                    if (PEnt != null)
                    {
                        try
                        {

                            BarcodeLib.Barcode barcode = new BarcodeLib.Barcode()
                            {
                                IncludeLabel = false, // Disable text label
                                Alignment = AlignmentPositions.CENTER,
                                Width = 250,
                                Height = 50,
                                RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                                BackColor = Color.White,
                                ForeColor = Color.Black,
                            };

                            Bitmap bitmap = new Bitmap(barcode.Encode(TYPE.CODE128B, PEnt.PRODUCT_CODE));
                            bitmap.Save(Server.MapPath("~/images/BarCode/Product/" + PEnt.PRODUCT_CODE + ".jpg"), ImageFormat.Jpeg);

                        }
                        catch
                        { }
                    }

                }
                else
                {
                    PEnt = new PRODUCT();
                    PEnt.PK_ID = lblPKIDU.Text;
                    PEnt.PRODUCT_CODE = txtPCode.Text;
                    PEnt.CATEGORY_ID = ddlProductCategory.SelectedValue;
                    if (ddlProductSubCategory.SelectedValue != "Select")
                        PEnt.SUB_CATEGORY_ID = ddlProductSubCategory.SelectedValue;
                    else
                        PEnt.SUB_CATEGORY_ID = "0";
                    PEnt.PRODUCT_NAME = txtPName.Text.ToUpper();
                    PEnt.UNIT_ID = ddlUnit.SelectedValue;
                    PEnt.HS_CODE = txtHSCode.Text;
                    PEnt.PRICE_CATEGORY = ddlPriceCategory.SelectedValue;
                    PEnt.EXPIRY_STATUS = ddlExpiryStatus.SelectedValue;
                    PEnt.COLOUR_STATUS = ddlProductColor.SelectedValue;
                    PEnt.PRODUCT_TYPE_ID = ddlProType.SelectedValue;
                    PEnt.SIZE_STATUS = ddlProductSize.SelectedValue;
                    if (ddlProductManufacturer.SelectedValue != "Select")
                    {
                        PEnt.MANUFACTURE_STATUS = ddlProductManufacturer.SelectedValue;
                    }
                    PEnt.BATCH_STATUS = ddlProductBatch.SelectedValue;
                    PEnt.STATUS = ddlAvailability.SelectedValue;
                    PEnt.TAX_STATUS = ddlTaxable.SelectedValue;
                    if (chkDualUnit.Checked == true)
                    {
                        PEnt.DUAL_UNIT = "1";
                        PEnt.UPPER_UNIT_ID = ddlUpperUnit.SelectedValue;
                        PEnt.PACK_QTY = txtPackQty.Text;
                    }
                    else
                    {
                        PEnt.DUAL_UNIT = "0";
                        PEnt.PACK_QTY = "1";
                    }
                    if (chkIsService.Checked == true)
                    {
                        PEnt.IS_SERVICE = "1";
                    }
                    else
                    {
                        PEnt.IS_SERVICE = "0";
                    }
                    if (PG.CompanyBranch_Status())
                    {
                        PEnt.OFFICE_CODE = "-";
                    }
                    else
                    {
                        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                        PEnt.OFFICE_CODE = userProfileEnt.LocationID;
                    }
                    PSer.Update(PEnt);
                }
                if (fileAttachment.HasFile)
                {
                    string lblMsg;
                    try
                    {
                        // Specify the folder path where the image will be saved
                        string folderPath = Server.MapPath("~/images/Product_Image/");

                        // Ensure the folder exists; create it if it does not exist
                        if (!System.IO.Directory.Exists(folderPath))
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }

                        // Validate the uploaded file type (allow only image files)
                        string fileExtension = System.IO.Path.GetExtension(fileAttachment.FileName).ToLower();
                        if (fileExtension != ".jpg" || fileExtension != ".jpeg" || fileExtension != ".png" || fileExtension != ".pdf")
                        {
                            lblMsg = "Only Images are allowed.";
                        }

                        // Generate a new file name (e.g., based on some unique identifier)
                        string fileName = "Product_Image" + txtPCode.Text + fileExtension;
                        string filePath = System.IO.Path.Combine(folderPath, fileName);
                        if (File.Exists(filePath))
                        {
                            // Delete the existing file
                            File.Delete(filePath);
                        }
                        // Combine the folder path with the new file name


                        // Save the uploaded file to the specified path
                        fileAttachment.SaveAs(filePath);
                    }
                    catch (Exception ex)
                    {
                        // Handle and display errors
                        lblMsg = "An error occurred while uploading the file: " + ex.Message;
                    }

                }
                HelperFunction.MsgBox(this, this.GetType(), "Data Saved Sucessfully.");
                try
                {
                    ddlProductSubCategoryF.SelectedValue = ddlProductSubCategory.SelectedValue;
                    ddlProductCategoryF.SelectedValue = ddlProductCategory.SelectedValue;
                }
                catch { }

            }
            catch
            {
                HelperFunction.MsgBox(this, this.GetType(), "Please fill all fields.");
            }
        }

        else
            HelperFunction.MsgBox(this, this.GetType(), "Please fill all fields.");

    }
    protected void txtPCode_TextChanged(object sender, EventArgs e)
    {
        txtPCode.Text = txtPCode.Text.ToUpper();
        PEnt = new PRODUCT();
        PEnt.PRODUCT_CODE = txtPCode.Text.ToUpper();
        PEnt = (PRODUCT)PSer.GetSingle(PEnt);
        if (PEnt != null && txtPCode.Text != "")
        {
            HelperFunction.MsgBox(this, this.GetType(), "This code is already taken.");
            txtPCode.Text = "";
            txtPCode.Focus();
        }
        else
        {
            txtPName.Focus();
        }
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
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

            }


        }
    }
    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string product_pk_id;
        if (e.CommandName.Equals("Change"))
        {
            ClearFields(); ;
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
            lblPKIDU.Text = lblPK_ID.Text;
            product_pk_id = lblPK_ID.Text;
            PEnt = new PRODUCT();
            PEnt.PK_ID = lblPK_ID.Text;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                txtPCode.Text = PEnt.PRODUCT_CODE;
                ddlProductCategory.SelectedValue = PEnt.CATEGORY_ID;
                if (PEnt.SUB_CATEGORY_ID != "0")
                {
                    LoadProductSubCategory();
                    ddlProductSubCategory.SelectedValue = PEnt.SUB_CATEGORY_ID;
                }
                txtPName.Text = PEnt.PRODUCT_NAME;
                txtPackQty.Text = PEnt.PACK_QTY;
                ddlUnit.SelectedValue = PEnt.UNIT_ID;
                txtHSCode.Text = PEnt.HS_CODE;
                ddlPriceCategory.SelectedValue = PEnt.PRICE_CATEGORY;
                ddlExpiryStatus.SelectedValue = PEnt.EXPIRY_STATUS;
                ddlProType.SelectedValue = PEnt.PRODUCT_TYPE_ID;
                ddlProductSize.SelectedValue = PEnt.SIZE_STATUS;
                ddlProductManufacturer.SelectedValue = PEnt.MANUFACTURE_STATUS;
                ddlProductBatch.SelectedValue = PEnt.BATCH_STATUS;
                ddlAvailability.SelectedValue = PEnt.STATUS;
                ddlTaxable.SelectedValue = PEnt.TAX_STATUS;
                if (PEnt.DUAL_UNIT == "1")
                {
                    divDualUnit.Visible = true;
                    chkDualUnit.Checked = true;
                    ddlUpperUnit.SelectedValue = PEnt.UPPER_UNIT_ID;
                    txtPackQty.Text = PEnt.PACK_QTY;
                    divDualUnit.Visible = true;
                    lblUpperQtyLable.Text = "Qty in 1 " + ddlUpperUnit.SelectedItem.ToString();
                }

                if (PEnt.IS_SERVICE == "1")
                {
                    chkIsService.Checked = true;
                }
                else
                {
                    chkIsService.Checked = false;
                }
            }
            divGrid.Visible = false;
            divAdd.Visible = true;
            divAddFilter.Visible = false;
            divProductImage.Visible = true;
            showProductImg(product_pk_id);
        }
    }
    protected void chkDualUnit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDualUnit.Checked == true)
        {
            divDualUnit.Visible = true;
            LoadProductUpperUnit();
        }
        else
        {
            divDualUnit.Visible = false;
        }
    }

    protected void ddlUpperUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblUpperQtyLable.Text = "Qty in 1 " + ddlUpperUnit.SelectedItem.ToString();
        lblLowerQtyLable.Text = ddlUnit.SelectedItem.ToString();
    }

    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        txtProductCode.Text = txtProductCode.Text.ToUpper();
        PEnt = new PRODUCT();
        PEnt.PRODUCT_CODE = txtProductCode.Text;
        if (PG.CompanyBranch_Status())
        {
            PEnt.OFFICE_CODE = "-";
        }
        else
        {
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            PEnt.OFFICE_CODE = userProfileEnt.LocationID;
        }
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

    protected void ddlBranchFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductList();
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        grid.DataSource = null;
        grid.DataBind();
        PEnt = new PRODUCT();

        PEnt.PK_ID = ddlProductList.SelectedValue;
        grid.DataSource = PSer.GetAll(PEnt);
        grid.DataBind();

        divGrid.Visible = true;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow gr = btn.NamingContainer as GridViewRow;

        // Find the barcode image control in the row (if applicable)
        System.Web.UI.WebControls.Image barcodeImg = gr.FindControl("barcodeImg") as System.Web.UI.WebControls.Image;

        if (barcodeImg != null && !string.IsNullOrEmpty(barcodeImg.ImageUrl))
        {
            // Pass the image URL to the JavaScript print function
            string script = "printBarcode('" + barcodeImg.ImageUrl + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PrintBarcode", script, true);
        }
    }

    protected void showProductImg(string productImgPkId)
    {
        string folderVirtualPath = "~/images/Product_image/";
        string folderPhysicalPath = Server.MapPath(folderVirtualPath);
        string[] supportedExtensions = { ".png", ".jpg", ".jpeg" };
        string imgVirtualPath;
        foreach (var extension in supportedExtensions)
        {
            string fileName = "Product_imageP" + productImgPkId + extension;
            string filePhysicalPath = System.IO.Path.Combine(folderPhysicalPath, fileName);

            if (File.Exists(filePhysicalPath))
            {
                imgVirtualPath = folderVirtualPath + fileName;
                imgProduct.ImageUrl = imgVirtualPath;
                divProductImage.Visible = true;

            }
            else
            {
                divProductImage.Visible = false;
            }
        }
    }

    protected void SetGridVisibility()
    {

        grid.Columns[7].Visible = PG.ProductManufacturerStatus();
        grid.Columns[8].Visible = PGPS.DualQuantity();
        grid.Columns[9].Visible = PGPS.ProductManufacture();
    }


}