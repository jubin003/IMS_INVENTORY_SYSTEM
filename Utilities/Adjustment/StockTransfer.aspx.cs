using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;
using System.IO;
using System.Data;
using DataHelper.Framework;
using Entity.Framework;
public partial class Reports_Adjustment_StockTransfer : System.Web.UI.Page
{
    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

    PRODUCT_ADJUSTMENT PAJEnt = new PRODUCT_ADJUSTMENT();
    PRODUCT_ADJUSTMENTService PAJSer = new PRODUCT_ADJUSTMENTService();

    PRODUCT_CATEGORY PCEnt = new PRODUCT_CATEGORY();
    PRODUCT_CATEGORYService PCSer = new PRODUCT_CATEGORYService();

    PRODUCT_SUB_CATEGORY PSCEnt = new PRODUCT_SUB_CATEGORY();
    PRODUCT_SUB_CATEGORYService PSCSer = new PRODUCT_SUB_CATEGORYService();

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    HelperFunction hf = new HelperFunction();
    PhyeGanDate PGD = new PhyeGanDate();

    UserProfileEntity userProfile = new UserProfileEntity();

    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();

   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadProductCategory();
            LoadProductSubCategory();
            LoadProductCategoryTo();
            LoadProduct();
            txtAdjustmentDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
            //txtDateTo.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
        }

    }


    protected void ddlProduct_TextChanged(object sender, EventArgs e)
    {
        PEnt = new PRODUCT();
        if (ddlProductBy.SelectedValue != "Select")
        {
            PEnt.PK_ID = ddlProductBy.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                LoadAvailablity(ddlProductBy.SelectedValue, null, txtAvilableQty);
                lblBUnit.Text = getUnitName(PEnt.UNIT_ID);
            }
        }
        else
        {
            lblBUnit.Text = "";
        }
    }
    protected string getUnitName(string PK_ID)
    {
        string unit_name = "";
        PUEnt = new PRODUCT_UNIT();
        PUEnt.PK_ID = PK_ID;
        PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
        if (PUEnt != null && PK_ID != "")
            unit_name = PUEnt.UNIT_NAME;
        return unit_name;
    }
    protected void LoadAvailablity(string product, string batch, TextBox avaibility)
    {
        userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

        DataTable DT;
        DT = hf.getOpeningClosingBalance(PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), null, null, product, batch, null, null, null, userProfile.LocationID);

        if (DT != null && DT.Rows.Count > 0)
        {
            int closing_columnIndex = DT.Columns.IndexOf("CLOSING");
            // Check if the column exists
            if (closing_columnIndex != -1)
            {
                // Iterate through the rows of the DataTable and retrieve the values in the specified column
                foreach (DataRow row in DT.Rows)
                {
                    // Access the value in the specified column using the column index
                    object columnValue = row[closing_columnIndex];
                    avaibility.Text = columnValue.ToString();
                }
            }
            int expiry_columnIndex = DT.Columns.IndexOf("expiry_date");
            // Check if the column exists
            if (expiry_columnIndex != -1)
            {
                // Iterate through the rows of the DataTable and retrieve the values in the specified column
                foreach (DataRow row in DT.Rows)
                {
                    // Access the value in the specified column using the column index
                    object columnValue = row[expiry_columnIndex];
                    //txtExpDate.Text = columnValue.ToString();
                }
            }
        }
        else
        {
            avaibility.Text = "0";
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
        PEnt.STATUS = "1";
        ddlProductBy.DataSource = PSer.GetAll(PEnt);
        ddlProductBy.DataValueField = "PK_ID";
        ddlProductBy.DataTextField = "PRODUCT_NAME";
        ddlProductBy.DataBind();
        ddlProductBy.Items.Insert(0, "Select");
    }

    protected void LoadProductCategoryTo()
    {
        PCEnt = new PRODUCT_CATEGORY();
        PCEnt.STATUS = "1";
        ddlProCategory.DataSource = PCSer.GetAll(PCEnt);
        ddlProCategory.DataTextField = "CATEGORY_NAME";
        ddlProCategory.DataValueField = "PK_ID";
        ddlProCategory.DataBind();
        ddlProCategory.Items.Insert(0, "Select");

    }
    protected void LoadProductSubCategoryTo()
    {
        PSCEnt = new PRODUCT_SUB_CATEGORY();
        PSCEnt.CATEGORY_ID = ddlProCategory.SelectedValue;
        PSCEnt.STATUS = "1";
        ddlProSubCategory.DataSource = PSCSer.GetAll(PSCEnt);
        ddlProSubCategory.DataTextField = "SUB_CATEGORY_NAME";
        ddlProSubCategory.DataValueField = "PK_ID";
        ddlProSubCategory.DataBind();
        ddlProSubCategory.Items.Insert(0, "Select");
    }
    protected void LoadProductTo()
    {
        PEnt = new PRODUCT();
        PEnt.CATEGORY_ID = ddlProCategory.SelectedValue;
        if (ddlProCategory.SelectedValue != "Select")
            PEnt.SUB_CATEGORY_ID = ddlProSubCategory.SelectedValue;
        else
            PEnt.SUB_CATEGORY_ID = "0";
        PEnt.STATUS = "1";
        ddlProductTo.DataSource = PSer.GetAll(PEnt);
        ddlProductTo.DataValueField = "PK_ID";
        ddlProductTo.DataTextField = "PRODUCT_NAME";
        ddlProductTo.DataBind();
        ddlProductTo.Items.Insert(0, "Select");
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

    protected void ddlProCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductSubCategoryTo();
        LoadProductTo();
    }

    protected void ddlProSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductTo();
    }

    protected void ddlProductTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        PEnt = new PRODUCT();
        if (ddlProductTo.SelectedValue != "Select")
        {
            PEnt.PK_ID = ddlProductTo.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                LoadAvailablity(PEnt.PK_ID, null, txtStock);
                txtToUnit.Focus();
                lblQUnit.Text = getUnitName(PEnt.UNIT_ID);
            }
        }
        else
        {
            lblQUnit.Text = "";
        }
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (Convert.ToDouble(txtAvilableQty.Text) > Convert.ToDouble(txtToUnit.Text))
        {
            userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            #region Transfer By
            PAJEnt = new PRODUCT_ADJUSTMENT();
            PAJEnt.PRODUCT_ID = ddlProductBy.SelectedValue;
            PAJEnt.QUANTITY = (Convert.ToDouble(txtToUnit.Text) * -1.00).ToString("#.##");
            string[] Nepdate = txtAdjustmentDate.Text.Split('/');
            PAJEnt.ADJUSTMENT_DAY = Nepdate[0];
            PAJEnt.ADJUSTMENT_MONTH = Nepdate[1];
            PAJEnt.ADJUSTMENT_YEAR = Nepdate[2];
            PAJEnt.ADJUSTMENT_FY = PGD.checkFiscalYear(Nepdate[1], Nepdate[2]);
            PAJEnt.ADJUSTMENT_DATE = PGD.ConvertNepaliTOEnglish(Nepdate[0], Nepdate[1], Nepdate[2]);
            PAJEnt.REMARKS = txtNarration.Text;
            PAJEnt.OFFICE_CODE = userProfile.LocationID;
            PAJSer.Insert(PAJEnt);
            #endregion
            #region Transfer Ty
            PAJEnt = new PRODUCT_ADJUSTMENT();
            PAJEnt.PRODUCT_ID = ddlProductTo.SelectedValue;
            PAJEnt.QUANTITY = (Convert.ToDouble(txtToUnit.Text)).ToString("#.##");
            string[] Nepdates = txtAdjustmentDate.Text.Split('/');
            PAJEnt.ADJUSTMENT_DAY = Nepdates[0];
            PAJEnt.ADJUSTMENT_MONTH = Nepdates[1];
            PAJEnt.ADJUSTMENT_YEAR = Nepdates[2];
            PAJEnt.ADJUSTMENT_FY = PGD.checkFiscalYear(Nepdates[1], Nepdates[2]);
            PAJEnt.ADJUSTMENT_DATE = PGD.ConvertNepaliTOEnglish(Nepdates[0], Nepdates[1], Nepdates[2]);
            PAJEnt.REMARKS = txtNarration.Text;
            PAJEnt.OFFICE_CODE = userProfile.LocationID;
            PAJSer.Insert(PAJEnt);
            #endregion
            HelperFunction.MsgBox(this, this.GetType(), "Stock Transferred");
            Clearfield();
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Cannot be greater than available quantity");
            txtToUnit.Focus();
        }
    }
    protected void Clearfield()
    {
        ddlCategoryFilter.SelectedIndex = 0;
        ddlSubCategoryFilter.SelectedIndex = 0;
        ddlProductBy.SelectedIndex = 0;
        ddlProCategory.SelectedIndex = 0;
        ddlProSubCategory.SelectedIndex = 0;
        ddlProductTo.SelectedIndex = 0;
        txtNarration.Text = "";
        txtAvilableQty.Text = "";
        txtStock.Text = "";
        txtToUnit.Text = "";
    }
}