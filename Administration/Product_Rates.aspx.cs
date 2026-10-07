using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using DataHelper.Framework;
using PhyeGanCore;
using Entity.Framework;
using System.Web;
using System.Web.UI;

public partial class Administration_Product_Rates : System.Web.UI.Page
{
    PRODUCT_CATEGORY PCEnt = new PRODUCT_CATEGORY();
    PRODUCT_CATEGORYService PCSer = new PRODUCT_CATEGORYService();

    PRODUCT_SUB_CATEGORY PSCEnt = new PRODUCT_SUB_CATEGORY();
    PRODUCT_SUB_CATEGORYService PSCSer = new PRODUCT_SUB_CATEGORYService();

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    EMPLOYEES Ent = new EMPLOYEES();
    EMPLOYEESService ESer = new EMPLOYEESService();

    OFFICE OEnt = new OFFICE();
    OFFICEService OSer = new OFFICEService();

    PRODUCT_RATE_TYPE CTEnt = new PRODUCT_RATE_TYPE();
    PRODUCT_RATE_TYPEService CTSer = new PRODUCT_RATE_TYPEService();
   

    PhyeGanProductSetting PPS = new PhyeGanProductSetting();

    PRODUCT_RATES PREnt = new PRODUCT_RATES();
    PRODUCT_RATEService PRSer = new PRODUCT_RATEService();   

    HelperFunction hf = new HelperFunction();

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
                    trRateType.Visible = PPS.MultipleRate();                    
                    LoadProductCategory();
                    loadBranch();
                    loadCustomerType();
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
    protected void LoadProductCategory()
    {
        PCEnt = new PRODUCT_CATEGORY();
        PCEnt.STATUS = "1";
        ddlProductGroup.DataSource = PCSer.GetAll(PCEnt);
        ddlProductGroup.DataTextField = "CATEGORY_NAME";
        ddlProductGroup.DataValueField = "PK_ID";
        ddlProductGroup.DataBind();
        ddlProductGroup.Items.Insert(0, "Select");
    }
    protected void LoadProductSubCategory()
    {
        PSCEnt = new PRODUCT_SUB_CATEGORY();
        PSCEnt.CATEGORY_ID = ddlProductGroup.SelectedValue;
        PSCEnt.STATUS = "1";
        ddlProductCategory.DataSource = PSCSer.GetAll(PSCEnt);
        ddlProductCategory.DataTextField = "SUB_CATEGORY_NAME";
        ddlProductCategory.DataValueField = "PK_ID";
        ddlProductCategory.DataBind();
        ddlProductCategory.Items.Insert(0, "Select");


    }
    protected void loadCustomerType()
    {
        CTEnt = new PRODUCT_RATE_TYPE();
        CTEnt.OFFICE_CODE = ddlBranch.SelectedValue;
        CTEnt.STATUS = "1";
        ddlCustomerType.DataSource = CTSer.GetAll(CTEnt);
        ddlCustomerType.DataTextField = "RATE_TYPE_NAME";
        ddlCustomerType.DataValueField = "PK_ID";
        ddlCustomerType.DataBind();
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
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
            trBranch.Visible = false;
        }
    }    
    protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
    {

        LoadProductSubCategory();
    }

    protected void ddlCustomerType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //loadCustomerType();
    }

    protected void LoadGrid()
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        PEnt = new PRODUCT();
        PEnt.OFFICE_CODE = userProfileEnt.LocationID;
        PEnt.CATEGORY_ID = ddlProductGroup.SelectedValue;
        PEnt.SUB_CATEGORY_ID = ddlProductCategory.SelectedValue;
        PEnt.STATUS = "1";
        gridRate.DataSource = PSer.GetAll(PEnt);
        gridRate.DataBind();
    }

    protected void ddlProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in gridRate.Rows)
        {
            Label lblPk_id = (Label)gr.FindControl("lblPk_id");
            TextBox txtRate = (TextBox)gr.FindControl("txtRate");
            TextBox txtExVatRate = (TextBox)gr.FindControl("txtExVatRate");
            Label lbl_Pr_ID = (Label)gr.FindControl("lbl_Pr_ID");
            if (lbl_Pr_ID.Text == "")
            {
                PREnt = new PRODUCT_RATES();
                PREnt.CUSTOMER_TYPE_ID = ddlCustomerType.SelectedValue;
                PREnt.PRODUCT_ID = lblPk_id.Text;
                PREnt.RATE = txtExVatRate.Text;
                PREnt.STATUS = "1";
                PREnt.OFFICE_CODE = ddlBranch.SelectedValue;
                PRSer.Insert(PREnt);
            }
            else
            {
                PREnt = new PRODUCT_RATES();
                PREnt.PK_ID = lbl_Pr_ID.Text;
                PREnt = (PRODUCT_RATES)PRSer.GetSingle(PREnt);
                if (PREnt != null)
                {
                    PREnt.RATE = txtExVatRate.Text;
                    PREnt.OFFICE_CODE = ddlBranch.SelectedValue;
                    PRSer.Update(PREnt);
                }
            }
        }
        HelperFunction.MsgBox(this, this.GetType(), "Saved ");
    }

    protected void gridRate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblPk_id = (Label)e.Row.FindControl("lblPk_id");
            Label lbl_Pr_ID = (Label)e.Row.FindControl("lbl_Pr_ID");
            TextBox txtRate = (TextBox)e.Row.FindControl("txtRate");
            TextBox txtExVatRate = (TextBox)e.Row.FindControl("txtExVatRate");

            if (PG.CompanyTAXType() == "None")
            {
                gridRate.Columns[3].Visible = false;

                txtExVatRate.Enabled = true;

            }

            PREnt = new PRODUCT_RATES();
            PREnt.PRODUCT_ID = lblPk_id.Text;
            PREnt.OFFICE_CODE = ddlBranch.SelectedValue;
            PREnt.CUSTOMER_TYPE_ID = ddlCustomerType.SelectedValue;
            PREnt = (PRODUCT_RATES)PRSer.GetSingle(PREnt);
            if (PREnt != null)
            {
                txtExVatRate.Text = PREnt.RATE;
                txtRate.Text = (Convert.ToDouble(txtExVatRate.Text) * 1.13).ToString("0.0000");
                lbl_Pr_ID.Text = PREnt.PK_ID;
            }
            else
            {
                txtRate.Text = "0.00";
                txtExVatRate.Text = "0.00";
            }
        }
    }

    protected void btn_View_Click(object sender, EventArgs e)
    {
        LoadGrid();
        divGrid.Visible = true;
    }

    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        TextBox txtRate = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)txtRate.NamingContainer;
        TextBox txtExVatRate = (TextBox)currentRow.FindControl("txtExVatRate");

        try
        {
            double rate = Convert.ToDouble(txtRate.Text);
            txtRate.Text = rate.ToString("#0.0000");
            txtExVatRate.Text = (rate / 1.13).ToString("0.0000");

            // Try to focus the txtRate of the next row
            int nextRowIndex = currentRow.RowIndex + 1;
            if (nextRowIndex < gridRate.Rows.Count) // Replace 'GridView1' with your actual GridView ID
            {
                GridViewRow nextRow = gridRate.Rows[nextRowIndex];
                TextBox nextTxtRate = (TextBox)nextRow.FindControl("txtRate");
                if (nextTxtRate != null)
                {
                    ScriptManager.GetCurrent(this).SetFocus(nextTxtRate); // For UpdatePanel-safe focus
                }
            }
        }
        catch
        {
            txtRate.Text = "0.00";
            txtRate.Focus();
            HelperFunction.MsgBox(this, this.GetType(), "Enter Number Only.");
        }
    }




    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadCustomerType();
    }
}