using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Production_Purchase_Order_Detail : System.Web.UI.Page
{
    PURCHASE_ORDER_DETAIL PEnt = new PURCHASE_ORDER_DETAIL();
    PURCHASE_ORDER_DETAILService PSer = new PURCHASE_ORDER_DETAILService();
    EntityList theList = new EntityList();

    PRODUCT ProEnt = new PRODUCT();
    PRODUCTService ProSer = new PRODUCTService();

    PRODUCT_UNIT UnEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService UnSer = new PRODUCT_UNITService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gridLoad();
        }
    }

    protected void gridLoad()
    {
        PEnt = new PURCHASE_ORDER_DETAIL();
        EntityList EList = new EntityList();
        theList = PSer.GetAll(PEnt);

        if (theList.Count == 0)
            theList.Add(PEnt);

        gridDisplay.DataSource = theList;
        gridDisplay.DataBind();
    }

    protected void gridDisplay_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            var ddlProduct = (DropDownList)e.Row.FindControl("ddlProduct");
            var ddlUnit = (DropDownList)e.Row.FindControl("ddlUnit");

            ProEnt = new PRODUCT();
            ddlProduct.DataSource = ProSer.GetAll(ProEnt);
            ddlProduct.DataTextField = "PRODUCT_NAME";
            ddlProduct.DataValueField = "PK_ID";
            ddlProduct.DataBind();
            ddlProduct.Items.Insert(0,"Select");

            UnEnt = new PRODUCT_UNIT();
            ddlUnit.DataSource = UnSer.GetAll(UnEnt);
            ddlUnit.DataTextField = "UNIT_NAME";
            ddlUnit.DataValueField = "PK_ID";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, "Select");
        }
    }
    protected void gridDisplay_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridDisplay.EditIndex = e.NewEditIndex;
        gridLoad();
    }

    protected void gridDisplay_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridDisplay.Rows[e.RowIndex];

        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");
        TextBox txtPurchaseOrderID = (TextBox)row.FindControl("txtPurchaseOrderIDE");
        TextBox txtSNO = (TextBox)row.FindControl("txtSNOE");
        TextBox txtProductID = (TextBox)row.FindControl("txtProductIDE");
        TextBox txtSizeID = (TextBox)row.FindControl("txtSizeIDE");
        TextBox txtSwatchID = (TextBox)row.FindControl("txtSwatchIDE");
        TextBox txtQuantity = (TextBox)row.FindControl("txtQuantityE");
        TextBox txtUnit = (TextBox)row.FindControl("txtUnitE");

        if (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text))
            return;

        PEnt = new PURCHASE_ORDER_DETAIL();
        PEnt.PK_ID = lblPK_ID.Text;
        PEnt = (PURCHASE_ORDER_DETAIL)PSer.GetSingle(PEnt);

        if (PEnt != null)
        {
            PEnt.PURCHASE_ORDER_ID = txtPurchaseOrderID.Text;
            PEnt.SNO = txtSNO.Text;
            PEnt.PRODUCT_ID = txtProductID.Text;
            PEnt.SIZE_ID = txtSizeID.Text;
            PEnt.SWATCH_ID = txtSwatchID.Text;
            PEnt.QUANTITY = txtQuantity.Text;
            PEnt.UNIT = txtUnit.Text;

            PSer.Update(PEnt);
        }

        gridDisplay.EditIndex = -1;
        gridLoad();
    }

    protected void gridDisplay_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridDisplay.EditIndex = -1;
        gridLoad();
    }



    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridDisplay.HeaderRow;

        TextBox txtPurchaseOrderID = (TextBox)row.FindControl("txtPurchaseOrderID");
        TextBox txtSNO = (TextBox)row.FindControl("txtSNO");
        TextBox txtProductID = (TextBox)row.FindControl("txtProductID");
        TextBox txtSizeID = (TextBox)row.FindControl("txtSizeID");
        TextBox txtSwatchID = (TextBox)row.FindControl("txtSwatchID");
        TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
        TextBox txtUnit = (TextBox)row.FindControl("txtUnit");

        if (string.IsNullOrEmpty(txtPurchaseOrderID.Text))
            HelperFunction.MsgBox(this, this.GetType(), "Purchase Order ID can not be empty.");
        else if (string.IsNullOrEmpty(txtProductID.Text))
            HelperFunction.MsgBox(this, this.GetType(), "Product ID can not be empty.");
        else if (string.IsNullOrEmpty(txtQuantity.Text))
            HelperFunction.MsgBox(this, this.GetType(), "Quantity can not be empty.");
        else
        {
            PEnt = new PURCHASE_ORDER_DETAIL();

            PEnt.PURCHASE_ORDER_ID = txtPurchaseOrderID.Text;
            PEnt.SNO = txtSNO.Text;
            PEnt.PRODUCT_ID = txtProductID.Text;
            PEnt.SIZE_ID = txtSizeID.Text;
            PEnt.SWATCH_ID = txtSwatchID.Text;
            PEnt.QUANTITY = txtQuantity.Text;
            PEnt.UNIT = txtUnit.Text;

            PSer.Insert(PEnt);

            HelperFunction.MsgBox(this, this.GetType(), "Inserted");

            gridLoad();
        }
    }
}