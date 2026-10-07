using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;

public partial class Administration_ProductReorderLevel : System.Web.UI.Page
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

    PRODUCT_REORDER_LEVEL PRLEnt = new PRODUCT_REORDER_LEVEL();
    PRODUCT_REORDER_LEVELService PRLSer = new PRODUCT_REORDER_LEVELService();

    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();

    HelperFunction hf = new HelperFunction();
    Boolean IsPageRefresh = false;

    PhyeGan PG = new PhyeGan();
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
                    ddlProductCategory.Enabled = true;
                    LoadProductCategory();
                    loadBranch();
                    #region to show hide product property div
                    if (PGPS.ProductColor())
                    {
                        divPColour.Visible = true;
                        LoadColour();
                    }

                    if (PGPS.ProductSize())
                    {
                        divPSize.Visible = true;
                        LoadSize();
                    }
                    else divPSize.Visible = false;
                    #endregion

                }
                else
                {
                    Response.Redirect("~/forbidden.aspx");
                }
            }
            catch (Exception ee)
            {
                Response.Redirect("~/Login.aspx");
            }
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
        PSCEnt.CATEGORY_ID = ddlProductCategory.SelectedValue;
        PSCEnt.STATUS = "1";
        ddlProductSubCategory.DataSource = PSCSer.GetAll(PSCEnt);
        ddlProductSubCategory.DataTextField = "SUB_CATEGORY_NAME";
        ddlProductSubCategory.DataValueField = "PK_ID";
        ddlProductSubCategory.DataBind();
        ddlProductSubCategory.Items.Insert(0, "Select");

        PSCEnt = new PRODUCT_SUB_CATEGORY();
        PSCEnt.CATEGORY_ID = ddlCategoryFilter.SelectedValue;
        PSCEnt.STATUS = "1";
        ddlSubCategoryFilter.DataSource = PSCSer.GetAll(PSCEnt);
        ddlSubCategoryFilter.DataTextField = "SUB_CATEGORY_NAME";
        ddlSubCategoryFilter.DataValueField = "PK_ID";
        ddlSubCategoryFilter.DataBind();
        ddlSubCategoryFilter.Items.Insert(0, "Select");
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
            divBranch.Visible = true;
            divBranchFilter.Visible = true;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
        }
        else
        {
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
            divBranch.Visible = false;
            divBranchFilter.Visible = false;
        }
    }
    protected void LoadProduct()
    {
        PEnt = new PRODUCT();
        PEnt.CATEGORY_ID = ddlProductCategory.SelectedValue;
        if (ddlProductSubCategory.SelectedValue != "Select")
            PEnt.SUB_CATEGORY_ID = ddlProductSubCategory.SelectedValue;
        else
            PEnt.SUB_CATEGORY_ID = "0";
        PEnt.STATUS = "1";
        PEnt.OFFICE_CODE = ddlBranch.SelectedValue;
        ddlproduct.DataSource = PSer.GetAll(PEnt);
        ddlproduct.DataValueField = "PK_ID";
        ddlproduct.DataTextField = "PRODUCT_NAME";
        ddlproduct.DataBind();
        ddlproduct.Items.Insert(0, "Select");


        PEnt = new PRODUCT();
        PEnt.STATUS = "1";
        PEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
        PEnt.CATEGORY_ID = ddlCategoryFilter.SelectedValue;
        if (ddlSubCategoryFilter.SelectedValue != "Select")
            PEnt.SUB_CATEGORY_ID = ddlSubCategoryFilter.SelectedValue;
        else
            PEnt.SUB_CATEGORY_ID = "0";
        ddlProductFilter.DataSource = PSer.GetAll(PEnt);
        ddlProductFilter.DataValueField = "PK_ID";
        ddlProductFilter.DataTextField = "PRODUCT_NAME";
        ddlProductFilter.DataBind();
        ddlProductFilter.Items.Insert(0, "Select");
    }
    protected void ddlProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductSubCategory();

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
                lblPUnit.Text = PUEnt.UNIT_NAME;

            PUEnt = new PRODUCT_UNIT();
            PUEnt.PK_ID = PEnt.UPPER_UNIT_ID;
            PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
            if (PUEnt != null)
                lblSUnit.Text = PUEnt.UNIT_NAME;

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

            divQuantity.Visible = true;
            btnAdd.Visible = true;
            btnAddandContinue.Visible = true;
            #endregion
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (txtQty.Text != "")
        {
            if (!IsPageRefresh)
            {

                #region to insert
                if (lblOBPK_ID.Text == "") // insert
                {
                    PRLEnt = new PRODUCT_REORDER_LEVEL();
                    PRLEnt.PRODUCT_ID = ddlproduct.SelectedValue;

                    PEnt = new PRODUCT();
                    PEnt.PK_ID = ddlproduct.SelectedValue;
                    PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                    if (PEnt != null)
                    {
                        if (PEnt.COLOUR_STATUS == "1")
                            PRLEnt.COLOUR_ID = ddlProductColor.SelectedValue;

                        if (PEnt.SIZE_STATUS == "1")
                            PRLEnt.SIZE_ID = ddlProductSize.SelectedValue;

                        PRLEnt.OFFICE_CODE = ddlBranch.SelectedValue;

                    }

                    PRLEnt.QUANTITY = txtQty.Text;

                    PRLSer.Insert(PRLEnt);
                }
                #endregion
                #region to update
                else
                {
                    PRLEnt = new PRODUCT_REORDER_LEVEL();
                    PRLEnt.PK_ID = lblOBPK_ID.Text;

                    PRLEnt = (PRODUCT_REORDER_LEVEL)PRLSer.GetSingle(PRLEnt);
                    if (PRLEnt != null)
                    {
                        PEnt = new PRODUCT();
                        PEnt.PK_ID = PRLEnt.PRODUCT_ID;
                        PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                        if (PEnt != null)
                        {
                            if (PEnt.COLOUR_STATUS == "1")
                                PRLEnt.COLOUR_ID = ddlProductColor.SelectedValue;

                            if (PEnt.SIZE_STATUS == "1")
                                PRLEnt.SIZE_ID = ddlProductSize.SelectedValue;
                            PRLEnt.OFFICE_CODE = ddlBranch.SelectedValue;

                        }
                        PRLEnt.QUANTITY = txtQty.Text;
                        PRLSer.Update(PRLEnt);
                    }
                }

                #endregion
            }
            LoadOpeningBalance("post");
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please enter quantity!!");
            txtQty.Focus();
        }



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
        divQuantity.Visible = false;
        btnAdd.Visible = false;
        btnAddandContinue.Visible = false;
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
        if (ddlCategoryFilter.Text != "Select")
        {
            LoadOpeningBalance("pre");
            // divFilter.Visible = false;
            #region to show hide product property div

            #endregion
        }

    }

    protected void LoadOpeningBalance(string loadoption)
    {
        if (loadoption == "pre")
        {
            PRLEnt = new PRODUCT_REORDER_LEVEL();
            PRLEnt.CATEGORY_ID = ddlCategoryFilter.SelectedValue;
            if (ddlSubCategoryFilter.SelectedValue != "Select")
                PRLEnt.SUB_CATEGORY_ID = ddlSubCategoryFilter.SelectedValue;
            if (ddlProductFilter.SelectedValue != "Select")
                PRLEnt.PRODUCT_ID = ddlProductFilter.SelectedValue;
            PRLEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
            grdOpeningBalance.DataSource = PRLSer.GetAll(PRLEnt);
            grdOpeningBalance.DataBind();
        }
        else
        {
            ddlBranchFilter.SelectedValue = ddlBranch.SelectedValue;
            PRLEnt = new PRODUCT_REORDER_LEVEL();
            PRLEnt.CATEGORY_ID = ddlProductCategory.SelectedValue;
            if (ddlProductSubCategory.SelectedValue != "Select")
                PRLEnt.SUB_CATEGORY_ID = ddlProductSubCategory.SelectedValue;
            if (ddlproduct.SelectedValue != "Select")
                PRLEnt.PRODUCT_ID = ddlproduct.SelectedValue;
            PRLEnt.OFFICE_CODE = ddlBranchFilter.SelectedValue;
            grdOpeningBalance.DataSource = PRLSer.GetAll(PRLEnt);
            grdOpeningBalance.DataBind();
        }

        if (hf.ProductColor() == "1")
            grdOpeningBalance.Columns[4].Visible = true;
        else
            grdOpeningBalance.Columns[4].Visible = false;

        if (hf.ProductSize() == "1")
            grdOpeningBalance.Columns[5].Visible = true;
        else
            grdOpeningBalance.Columns[5].Visible = false;

        if (hf.ProductManufacturer() == "1")
            grdOpeningBalance.Columns[6].Visible = true;
        else
            grdOpeningBalance.Columns[6].Visible = false;


        divGrid.Visible = true;
        divFilter.Visible = false;
        divAdd.Visible = false;
    }
    protected void grdOpeningBalance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblCategoryId = e.Row.FindControl("lblCategoryId") as Label;
                Label lblCategoryName = e.Row.FindControl("lblCategoryName") as Label;
                Label lblSubCategoryId = e.Row.FindControl("lblSubCategoryId") as Label;
                Label lblSubCategoryName = e.Row.FindControl("lblSubCategoryName") as Label;
                Label lblProductId = e.Row.FindControl("lblProductId") as Label;
                Label lblProductName = e.Row.FindControl("lblProductName") as Label;
                Label lblColourId = e.Row.FindControl("lblColourId") as Label;
                Label lblColourName = e.Row.FindControl("lblColourName") as Label;
                Label lblSizeId = e.Row.FindControl("lblSizeId") as Label;
                Label lblSizeName = e.Row.FindControl("lblSizeName") as Label;


                PCEnt = new PRODUCT_CATEGORY();
                PCEnt.PK_ID = lblCategoryId.Text;
                PCEnt = (PRODUCT_CATEGORY)PCSer.GetSingle(PCEnt);
                if (PCEnt != null)
                    lblCategoryName.Text = PCEnt.CATEGORY_NAME;

                PSCEnt = new PRODUCT_SUB_CATEGORY();
                PSCEnt.PK_ID = lblSubCategoryId.Text;
                PSCEnt = (PRODUCT_SUB_CATEGORY)PSCSer.GetSingle(PSCEnt);
                if (PSCEnt != null && lblSubCategoryId.Text != "")
                {
                    lblSubCategoryName.Text = PSCEnt.SUB_CATEGORY_NAME;
                }

                PEnt = new PRODUCT();
                PEnt.PK_ID = lblProductId.Text;
                PEnt = (PRODUCT)PSer.GetSingle(PEnt);
                if (PEnt != null)
                    lblProductName.Text = PEnt.PRODUCT_NAME;

                PColEnt = new PRODUCT_COLOUR();
                PColEnt.PK_ID = lblColourId.Text;
                PColEnt = (PRODUCT_COLOUR)PColSer.GetSingle(PColEnt);
                if (PColEnt != null)
                {
                    lblColourName.Text = PColEnt.COLOUR_NAME;
                }

                PSEnt = new PRODUCT_SIZE();
                PSEnt.PK_ID = lblSizeId.Text;
                PSEnt = (PRODUCT_SIZE)PSSer.GetSingle(PSEnt);
                if (PSEnt != null)
                {
                    lblSizeName.Text = PSEnt.SIZE_NAME;
                }
            }
        }
    }

    protected void btnAddDiv_Click(object sender, EventArgs e)
    {
        divAdd.Visible = true;
        divFilter.Visible = false;
        divGrid.Visible = false;
        lblOBPK_ID.Text = "";
        ddlProductCategory.Enabled = true;
        ddlProductSubCategory.Enabled = true;
        ddlproduct.Enabled = true;
        Clear();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        divAdd.Visible = false;
        divFilter.Visible = true;
        divGrid.Visible = false;

    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((Button)sender).Parent.Parent as GridViewRow;
        Label lblpk_id = gr.FindControl("lblPK_ID") as Label;
        #region to load data from opening balance if exist
        PRLEnt = new PRODUCT_REORDER_LEVEL();
        PRLEnt.PK_ID = lblpk_id.Text;
        PRLEnt = (PRODUCT_REORDER_LEVEL)PRLSer.GetSingle(PRLEnt);
        if (PRLEnt != null)
        {
            lblOBPK_ID.Text = lblpk_id.Text;


            PEnt = new PRODUCT();
            PEnt.PK_ID = PRLEnt.PRODUCT_ID;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                ddlBranch.SelectedValue = ddlBranchFilter.SelectedValue;
                LoadProductCategory();
                ddlProductCategory.SelectedValue = PEnt.CATEGORY_ID;
                LoadProductSubCategory();
                if (PGPS.DualQuantity() && PEnt.DUAL_UNIT == "1")
                {
                    divPackQty.Visible = true;

                    lblPackQty.Text = PEnt.PACK_QTY;

                }

                if (PRLEnt.SUB_CATEGORY_ID != "0")
                    ddlProductSubCategory.SelectedValue = PEnt.SUB_CATEGORY_ID;
                LoadProduct();
                ddlproduct.SelectedValue = PRLEnt.PRODUCT_ID;

                PUEnt = new PRODUCT_UNIT();
                PUEnt.PK_ID = PEnt.UNIT_ID;
                PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
                if (PUEnt != null)
                    lblPUnit.Text = PUEnt.UNIT_NAME;
                divUnit.Visible = true;
                divPackQty.Visible = true;

                if (PEnt.COLOUR_STATUS == "1")
                {
                    divPColour.Visible = true;
                    LoadColour();
                    ddlProductColor.SelectedValue = PRLEnt.COLOUR_ID;
                }
                else
                    divPColour.Visible = false;

                if (PEnt.SIZE_STATUS == "1")
                {
                    LoadSize();
                    divPSize.Visible = true;
                    ddlProductSize.SelectedValue = PRLEnt.SIZE_ID;
                }
                else
                    divPSize.Visible = false;

            }
            divQuantity.Visible = true;
            txtQty.Text = PRLEnt.QUANTITY;
            btnAdd.Visible = true;

        }
        #endregion
        divAdd.Visible = true;
        divFilter.Visible = false;
        divGrid.Visible = false;
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
        PRLEnt = new PRODUCT_REORDER_LEVEL();
        PRLEnt.PK_ID = lblpk_id.Text;
        PRLEnt = (PRODUCT_REORDER_LEVEL)PRLSer.GetSingle(PRLEnt);
        if (PRLEnt != null)
        {
            PRLSer.Delete(PRLEnt);
        }
        #endregion

        LoadOpeningBalance("post");
    }

    protected void btnAddandContinue_Click(object sender, EventArgs e)
    {
        #region to insert
        if (lblOBPK_ID.Text == "") // insert
        {
            PRLEnt = new PRODUCT_REORDER_LEVEL();
            PRLEnt.PRODUCT_ID = ddlproduct.SelectedValue;

            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlproduct.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                if (PEnt.COLOUR_STATUS == "1")
                    PRLEnt.COLOUR_ID = ddlProductColor.SelectedValue;

                if (PEnt.SIZE_STATUS == "1")
                    PRLEnt.SIZE_ID = ddlProductSize.SelectedValue;

                PRLEnt.OFFICE_CODE = ddlBranch.SelectedValue;

            }
            PRLEnt.QUANTITY = txtQty.Text;

            PRLSer.Insert(PRLEnt);
        }
        #endregion
        txtQty.Text = "";
    }


}