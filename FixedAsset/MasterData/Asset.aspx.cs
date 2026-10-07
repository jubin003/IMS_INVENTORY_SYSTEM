using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using DataHelper.Framework;
public partial class FixedAsset_MasterData_Asset : System.Web.UI.Page
{
    ASSET AEnt = new ASSET();
    ASSETService ASer = new ASSETService();

    PRODUCT_UNIT IPUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService IPUSer = new PRODUCT_UNITService();

    ASSET_CATEGORY ACEnt = new ASSET_CATEGORY();
    ASSET_CATEGORYService ACSer = new ASSET_CATEGORYService();

    ASSET_SUB_CATEGORY ASCEnt = new ASSET_SUB_CATEGORY();
    ASSET_SUB_CATEGORYService ASCSer = new ASSET_SUB_CATEGORYService();

    GL_ACCOUNT GAEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GASer = new GL_ACCOUNTService();

    GL_SUB_ACCOUNT GSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GSASer = new GL_SUB_ACCOUNTService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadAssetCategory();
            LoadUnit();
        }
    }
    protected void LoadAssetCategory()
    {
        ACEnt = new ASSET_CATEGORY();
        ACEnt.STATUS = "1";
        ddlCategory.DataSource = ACSer.GetAll(ACEnt);
        ddlCategory.DataTextField = "CATEGORY_NAME";
        ddlCategory.DataValueField = "PK_ID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, "Select");

    }

    protected void LoadUnit()
    {
        IPUEnt = new PRODUCT_UNIT();
        ddlUnit.DataSource = IPUSer.GetAll(IPUEnt);
        ddlUnit.DataTextField = "UNIT_NAME";
        ddlUnit.DataValueField = "PK_ID";
        ddlUnit.DataBind();
        ddlUnit.Items.Insert(0, "Select");
    }

    protected void LoadGrid()
    {
        AEnt = new ASSET();
        AEnt.SUB_CATEGORY_ID = ddlSubCategory.SelectedValue;
        gridAsset.DataSource = ASer.GetAll(AEnt);
        gridAsset.DataBind();
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAssetSubCategory();
    }

    protected void LoadAssetSubCategory()
    {
        ASCEnt = new ASSET_SUB_CATEGORY();
        ASCEnt.CATEGORY_ID = ddlCategory.SelectedValue;
        ddlSubCategory.DataSource = ASCSer.GetAll(ASCEnt);
        ddlSubCategory.DataTextField = "SUB_CATEGORY_NAME";
        ddlSubCategory.DataValueField = "PK_ID";
        ddlSubCategory.DataBind();
        ddlSubCategory.Items.Insert(0, "Select");

    }


    protected void btnView_Click(object sender, EventArgs e)
    {
        if (ddlSubCategory.SelectedValue == "Select" || ddlCategory.SelectedValue == "Select")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please Select Category and Sub-Category.");
        }
        else
        {
            LoadGrid();
            Addform.Visible = false;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (ddlSubCategory.SelectedValue == "Select" || ddlCategory.SelectedValue == "Select")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please Select Category and Sub-Category.");
        }
        else
        {
            Addform.Visible = true;
        }
    }

    protected void ClearField()
    {
        txtAssetCode.Text = "";
        txtAssetName.Text = "";
        ddlUnit.SelectedIndex = 0;

        ddlStatus.SelectedValue = "1";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DistributedTransaction DT = new DistributedTransaction();
        AEnt = new ASSET();
        if (string.IsNullOrEmpty(txtAssetCode.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Asset Code can not be empty.");
        }
        else if (string.IsNullOrEmpty(txtAssetName.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Asset Name can not be empty.");
        }
        else
        {
            if (lblPk_Id.Text == "")
            {

                AEnt.SUB_CATEGORY_ID = ddlSubCategory.SelectedValue;
                AEnt.ASSET_CODE = txtAssetCode.Text;
                AEnt.ASSET_NAME = txtAssetName.Text;
                AEnt.UNIT = ddlUnit.SelectedValue;
                AEnt.STATUS = ddlStatus.SelectedValue;
                ASer.Insert(AEnt, DT);

                GSAEnt = new GL_SUB_ACCOUNT();
                GSAEnt.STATUS = ddlStatus.SelectedValue;
                GSAEnt.SUB_GL_CODE = txtAssetCode.Text;
                GSAEnt.SUB_GL_NAME = txtAssetName.Text;
                ACEnt = new ASSET_CATEGORY();
                ACEnt.PK_ID = ddlCategory.SelectedValue;
                ACEnt = (ASSET_CATEGORY)ACSer.GetSingle(ACEnt);
                if (ACEnt != null)
                {
                    GSAEnt.GL_CODE = ACEnt.GL_CODE;
                }
                GSASer.Insert(GSAEnt, DT);
                LoadGrid();
                ClearField();
                
            }
            else
            {
                AEnt.PK_ID = lblPk_Id.Text;
                AEnt = (ASSET)ASer.GetSingle(AEnt);
                if (AEnt != null)
                {
                    GSAEnt = new GL_SUB_ACCOUNT();
                    ACEnt = new ASSET_CATEGORY();
                    ACEnt.PK_ID = ddlCategory.SelectedValue;
                    ACEnt = (ASSET_CATEGORY)ACSer.GetSingle(ACEnt);
                    if (ACEnt != null)
                    {
                        GSAEnt.GL_CODE = ACEnt.GL_CODE;
                    }
                    GSAEnt.SUB_GL_CODE = AEnt.ASSET_CODE;
                    GSAEnt = (GL_SUB_ACCOUNT)GSASer.GetSingle(GSAEnt);
                    if (GSAEnt != null)
                    {
                        GSAEnt.STATUS = ddlStatus.SelectedValue;
                        GSAEnt.SUB_GL_CODE = txtAssetCode.Text;
                        GSAEnt.SUB_GL_NAME = txtAssetName.Text;
                        GSASer.Update(GSAEnt, DT);
                    }
                    AEnt.SUB_CATEGORY_ID = ddlSubCategory.SelectedValue;
                    AEnt.ASSET_CODE = txtAssetCode.Text;
                    AEnt.ASSET_NAME = txtAssetName.Text;
                    AEnt.UNIT = ddlUnit.SelectedValue;
                    AEnt.STATUS = ddlStatus.SelectedValue;
                    ASer.Update(AEnt, DT);
                }
            }
            if (DT.HAPPY)
            {
                LoadGrid();
                ClearField();
                HelperFunction.MsgBox(this, this.GetType(), "Successfull");
                DT.Commit();
            }
            else
            {
                DT.Abort();
                HelperFunction.MsgBox(this, this.GetType(), "Something Went Wrong");
            }
            DT.Dispose();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearField();
    }

    protected void gridAsset_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblUnit = e.Row.FindControl("lblUnit") as Label;
            Label lblSnowUnit = e.Row.FindControl("lblSnowUnit") as Label;

            Label lblStatus = e.Row.FindControl("lblStatus") as Label;
            Label lblShowStatus = e.Row.FindControl("lblShowStatus") as Label;

            //For Unit
            IPUEnt = new PRODUCT_UNIT();
            IPUEnt.PK_ID = lblUnit.Text;
            IPUEnt = (PRODUCT_UNIT)IPUSer.GetSingle(IPUEnt);
            if (IPUEnt != null)
            {
                lblSnowUnit.Text = IPUEnt.UNIT_NAME;
            }

            // For Price Category

            // for status
            if (lblStatus.Text == "1")
            {
                lblShowStatus.Text = "Available";
            }
            else
            {
                lblShowStatus.Text = "UnAvailable";
            }

        }
    }

    protected void gridAsset_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "send")
        {
            Addform.Visible = true;
            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPKIDG = gr.FindControl("lblPKIDG") as Label;

            AEnt = new ASSET();
            AEnt.PK_ID = lblPKIDG.Text;
            AEnt = (ASSET)ASer.GetSingle(AEnt);
            if (AEnt != null)
            {
                lblPk_Id.Text = AEnt.PK_ID;
                ddlSubCategory.SelectedValue = AEnt.SUB_CATEGORY_ID;
                txtAssetCode.Text = AEnt.ASSET_CODE;
                txtAssetName.Text = AEnt.ASSET_NAME;
                ddlUnit.SelectedValue = AEnt.UNIT;

                ddlStatus.SelectedValue = AEnt.STATUS;
            }
        }
    }
}