using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Production_Purchase_Order : System.Web.UI.Page
{
    PR_PURCHASE_ORDER PoEnt = new PR_PURCHASE_ORDER();
    PR_PURCHASE_ORDERService PoSer = new PR_PURCHASE_ORDERService();
    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();
    PhyeGanDate PGD = new PhyeGanDate();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCustomer(ddlCustomer);
            LoadTransportation();
            pnlCustomerDetails.Visible = false;
            pnlDelivery.Visible = false;

            string pkId = Request.QueryString["PK_ID"];

            if (!string.IsNullOrEmpty(pkId))
                LoadPurchaseOrder(pkId);
            else
                txtOrderDate.Text = GetTodayNepali();
        }
    }

    private string GetTodayNepali()
    {
        string engdate = PGD.GetTodayDate("dd/mm/yyyy");
        return PGD.GetNepaliDateFromEnglish(engdate, "dd/mm/yyyy");
    }

    // =====================================================
    // CUSTOMER / ORDER INFO
    // =====================================================

    private void LoadCustomer(DropDownList ddl)
    {
        CEnt = new CUSTOMER();
        CEnt.STATUS = "1";

        ddl.DataSource = CSer.GetAll(CEnt);
        ddl.DataTextField = "CUSTOMER_NAME";
        ddl.DataValueField = "PK_ID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-- Select Customer --", ""));
    }

    private void LoadTransportation()
    {
        PR_TRANSPORTATION ent = new PR_TRANSPORTATION();
        ent.STATUS = "1";

        PR_TRANSPORTATIONService ser = new PR_TRANSPORTATIONService();

        ddlTransportation.DataSource = ser.GetAll(ent);
        ddlTransportation.DataTextField = "TRANSPORTATION_NAME";
        ddlTransportation.DataValueField = "PK_ID";
        ddlTransportation.DataBind();
        ddlTransportation.Items.Insert(0, new ListItem("-- Select Transportation --", ""));
    }

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        string customerId = ddlCustomer.SelectedValue;

        if (string.IsNullOrEmpty(customerId))
        {
            ClearCustomerDetails();
            pnlCustomerDetails.Visible = false;
            return;
        }

        LoadCustomerDetails(customerId);
    }

    private void LoadCustomerDetails(string customerId)
    {
        ClearCustomerDetails();

        if (string.IsNullOrEmpty(customerId))
        {
            pnlCustomerDetails.Visible = false;
            return;
        }

        CEnt = new CUSTOMER();
        CEnt.PK_ID = customerId;
        CEnt = (CUSTOMER)CSer.GetSingle(CEnt);

        if (CEnt == null)
        {
            pnlCustomerDetails.Visible = false;
            return;
        }

        lblCustomerID.Text = CEnt.PK_ID;
        lblCustomerName.Text = CEnt.CUSTOMER_NAME;
        lblCustomerAddress.Text = CEnt.ADDRESS;
        lblCustomerPhone.Text = CEnt.PHONE;
        lblCustomerEmail.Text = CEnt.EMAIL;
        pnlCustomerDetails.Visible = true;
    }

    private void ClearCustomerDetails()
    {
        lblCustomerID.Text = "";
        lblCustomerName.Text = "";
        lblCustomerAddress.Text = "";
        lblCustomerPhone.Text = "";
        lblCustomerEmail.Text = "";
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlCustomer.SelectedValue))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Customer can not be empty.");
            return;
        }

        if (string.IsNullOrWhiteSpace(txtOrderDate.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Order Date can not be empty.");
            return;
        }

        string[] nepDate = txtOrderDate.Text.Trim().Split('/');

        if (nepDate.Length != 3)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Order Date must be in dd/mm/yyyy format.");
            return;
        }

        string customerId = ddlCustomer.SelectedValue;

        /*
         * CREATE/UPDATE PR_PURCHASE_ORDER HERE.
         *
         * After saving, hfPK_ID must contain the PR_PURCHASE_ORDER.PK_ID.
         *
         * Example:
         *
         * PoEnt = new PR_PURCHASE_ORDER();
         * PoEnt.PK_ID = hfPK_ID.Value;
         * PoEnt.CUSTOMER_ID = customerId;
         * PoEnt.ORDER_DATE = txtOrderDate.Text.Trim();
         * PoEnt.ORDER_DAY = nepDate[0];
         * PoEnt.ORDER_MONTH = nepDate[1];
         * PoEnt.ORDER_YEAR = nepDate[2];
         * PoEnt.PAYMENT_TERM = txtPaymentTerm.Text.Trim();
         *
         * PoSer.Save/Insert/Update(...)
         */

        // Keep the delivery location if the user comes back from the Back button
        string previousLocation = ddlCustomerLocation.SelectedValue;

        LoadCustomerLocation(customerId);

        if (!string.IsNullOrEmpty(previousLocation) && ddlCustomerLocation.Items.FindByValue(previousLocation) != null)
        {
            ddlCustomerLocation.SelectedValue = previousLocation;
            LoadLocationDetails(previousLocation);
        }

        LoadProductTable();

        pnlPurchaseOrder.Visible = false;
        pnlDelivery.Visible = true;
    }

    // =====================================================
    // DELIVERY
    // =====================================================

    private void LoadCustomerLocation(string customerId)
    {
        PR_CUSTOMER_LOCATION ent = new PR_CUSTOMER_LOCATION();
        ent.CUSTOMER_ID = customerId;

        PR_CUSTOMER_LOCATIONService ser = new PR_CUSTOMER_LOCATIONService();

        ddlCustomerLocation.DataSource = ser.GetAll(ent);
        ddlCustomerLocation.DataTextField = "ADDRESS";
        ddlCustomerLocation.DataValueField = "PK_ID";
        ddlCustomerLocation.DataBind();
        ddlCustomerLocation.Items.Insert(0, new ListItem("-- Select Delivery Location --", ""));

        ClearDeliveryDetails();
    }

    protected void ddlCustomerLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadLocationDetails(ddlCustomerLocation.SelectedValue);
    }

    private void LoadLocationDetails(string locationId)
    {
        if (string.IsNullOrEmpty(locationId))
        {
            ClearDeliveryDetails();
            return;
        }

        PR_CUSTOMER_LOCATION ent = new PR_CUSTOMER_LOCATION();
        ent.PK_ID = locationId;

        PR_CUSTOMER_LOCATIONService ser = new PR_CUSTOMER_LOCATIONService();
        ent = (PR_CUSTOMER_LOCATION)ser.GetSingle(ent);

        if (ent == null)
        {
            ClearDeliveryDetails();
            return;
        }

        lblDeliveryAddress.Text = ent.ADDRESS;
        lblDeliveryContactPerson.Text = ent.CONTACT_PERSON;
        lblDeliveryContactNumber.Text = ent.CONTACT_NUMBER;
        lblDeliveryEmail.Text = ent.EMAIL_ID;

        LoadCountry(ent.COUNTRY_ID);
    }

    private void LoadCountry(string countryId)
    {
        if (string.IsNullOrEmpty(countryId))
        {
            lblDeliveryCountry.Text = "";
            return;
        }

        COUNTRY ent = new COUNTRY();
        ent.PK_ID = countryId;

        COUNTRYService ser = new COUNTRYService();
        ent = (COUNTRY)ser.GetSingle(ent);

        lblDeliveryCountry.Text = ent == null ? "" : ent.COUNTRY_NAME;
    }

    private void ClearDeliveryDetails()
    {
        lblDeliveryAddress.Text = "";
        lblDeliveryCountry.Text = "";
        lblDeliveryContactPerson.Text = "";
        lblDeliveryContactNumber.Text = "";
        lblDeliveryEmail.Text = "";
    }

    // =====================================================
    // PRODUCT GRID
    // =====================================================

    private DataTable ProductTable
    {
        get
        {
            if (ViewState["ProductTable"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ROW_ID");
                dt.Rows.Add("1");
                ViewState["ProductTable"] = dt;
            }

            return (DataTable)ViewState["ProductTable"];
        }
        set
        {
            ViewState["ProductTable"] = value;
        }
    }

    private void LoadProductTable()
    {
        grdProducts.DataSource = ProductTable;
        grdProducts.DataBind();
    }

    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        SaveProductRows();

        DataTable dt = ProductTable;
        dt.Rows.Add((dt.Rows.Count + 1).ToString());

        ProductTable = dt;
        LoadProductTable();
    }

    // Remembers what the user picked in every row so the grid can be rebound without losing it.
    // Unit is not stored: it is read-only and always derived from the selected product.
    private void SaveProductRows()
    {
        for (int i = 0; i < grdProducts.Rows.Count; i++)
        {
            GridViewRow row = grdProducts.Rows[i];

            DropDownList ddlProduct = row.FindControl("ddlProduct") as DropDownList;
            DropDownList ddlSize = row.FindControl("ddlSize") as DropDownList;
            DropDownList ddlSwatchType = row.FindControl("ddlSwatchType") as DropDownList;
            DropDownList ddlSwatch = row.FindControl("ddlSwatch") as DropDownList;
            TextBox txtQuantity = row.FindControl("txtQuantity") as TextBox;
            TextBox txtDeliveryDate = row.FindControl("txtDeliveryDate") as TextBox;

            ViewState["Product_" + i] = ddlProduct == null ? "" : ddlProduct.SelectedValue;
            ViewState["Size_" + i] = ddlSize == null ? "" : ddlSize.SelectedValue;
            ViewState["SwatchType_" + i] = ddlSwatchType == null ? "" : ddlSwatchType.SelectedValue;
            ViewState["Swatch_" + i] = ddlSwatch == null ? "" : ddlSwatch.SelectedValue;
            ViewState["Quantity_" + i] = txtQuantity == null ? "" : txtQuantity.Text;
            ViewState["DeliveryDate_" + i] = txtDeliveryDate == null ? "" : txtDeliveryDate.Text;
        }
    }

    private void ClearProductState()
    {
        List<string> keys = new List<string>();

        foreach (string key in ViewState.Keys)
        {
            if (key.StartsWith("Product_") || key.StartsWith("Size_") || key.StartsWith("SwatchType_")
                || key.StartsWith("Swatch_") || key.StartsWith("Quantity_") || key.StartsWith("DeliveryDate_"))
                keys.Add(key);
        }

        foreach (string key in keys)
            ViewState.Remove(key);
    }

    protected void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        DropDownList ddlProduct = e.Row.FindControl("ddlProduct") as DropDownList;
        DropDownList ddlSize = e.Row.FindControl("ddlSize") as DropDownList;
        DropDownList ddlSwatchType = e.Row.FindControl("ddlSwatchType") as DropDownList;
        DropDownList ddlSwatch = e.Row.FindControl("ddlSwatch") as DropDownList;
        TextBox txtUnit = e.Row.FindControl("txtUnit") as TextBox;
        TextBox txtQuantity = e.Row.FindControl("txtQuantity") as TextBox;
        TextBox txtDeliveryDate = e.Row.FindControl("txtDeliveryDate") as TextBox;

        int index = e.Row.RowIndex;

        // Product
        LoadProducts(ddlProduct);
        SetSelectedValue(ddlProduct, ViewState["Product_" + index]);

        // Unit (read-only, comes from the selected product)
        if (txtUnit != null)
            txtUnit.Text = GetProductUnitName(ddlProduct.SelectedValue);

        // Size
        LoadSizes(ddlSize);
        SetSelectedValue(ddlSize, ViewState["Size_" + index]);

        // Swatch type, then swatches filtered by that type
        string swatchTypeId = Convert.ToString(ViewState["SwatchType_" + index]);

        LoadSwatchTypes(ddlSwatchType);
        SetSelectedValue(ddlSwatchType, swatchTypeId);

        LoadSwatches(ddlSwatch, swatchTypeId);
        SetSelectedValue(ddlSwatch, ViewState["Swatch_" + index]);

        if (txtQuantity != null)
            txtQuantity.Text = Convert.ToString(ViewState["Quantity_" + index]);

        if (txtDeliveryDate != null)
            txtDeliveryDate.Text = Convert.ToString(ViewState["DeliveryDate_" + index]);
    }

    private void LoadProducts(DropDownList ddl)
    {
        PRODUCT ent = new PRODUCT();
        ent.STATUS = "1";

        PRODUCTService ser = new PRODUCTService();

        ddl.DataSource = ser.GetAll(ent);
        ddl.DataTextField = "PRODUCT_NAME";
        ddl.DataValueField = "PK_ID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-- Select Product --", ""));
    }

    private void LoadSizes(DropDownList ddl)
    {
        PRODUCT_SIZE ent = new PRODUCT_SIZE();
        ent.STATUS = "1";

        PRODUCT_SIZEService ser = new PRODUCT_SIZEService();

        ddl.DataSource = ser.GetAll(ent);
        ddl.DataTextField = "SIZE_NAME";
        ddl.DataValueField = "PK_ID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-- Select Size --", ""));
    }

    private void LoadSwatchTypes(DropDownList ddl)
    {
        PR_SWATCH_TYPE ent = new PR_SWATCH_TYPE();
        ent.STATUS = "1";

        PR_SWATCH_TYPEService ser = new PR_SWATCH_TYPEService();

        ddl.DataSource = ser.GetAll(ent);
        ddl.DataTextField = "SWATCH_NAME";
        ddl.DataValueField = "PK_ID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-- Select Type --", ""));
    }

    private void LoadSwatches(DropDownList ddl, string swatchTypeId)
    {
        PR_SWATCH ent = new PR_SWATCH();
        ent.STATUS = "1";

        if (!string.IsNullOrEmpty(swatchTypeId))
            ent.SWATCH_TYPE_ID = swatchTypeId;

        PR_SWATCHService ser = new PR_SWATCHService();

        ddl.DataSource = ser.GetAll(ent);
        ddl.DataTextField = "SWATCH_NAME";
        ddl.DataValueField = "PK_ID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-- Select Swatch --", ""));
    }

    // Returns the unit name of a product (PRODUCT.UNIT_ID -> PRODUCT_UNIT.UNIT_NAME)
    private string GetProductUnitName(string productId)
    {
        if (string.IsNullOrEmpty(productId))
            return "";

        PRODUCT product = new PRODUCT();
        product.PK_ID = productId;
        product = (PRODUCT)new PRODUCTService().GetSingle(product);

        if (product == null || string.IsNullOrEmpty(product.UNIT_ID))
            return "";

        PRODUCT_UNIT unit = new PRODUCT_UNIT();
        unit.PK_ID = product.UNIT_ID;
        unit = (PRODUCT_UNIT)new PRODUCT_UNITService().GetSingle(unit);

        return unit == null ? "" : unit.UNIT_NAME;
    }

    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlProduct = sender as DropDownList;

        if (ddlProduct == null)
            return;

        GridViewRow row = ddlProduct.NamingContainer as GridViewRow;

        if (row == null)
            return;

        TextBox txtUnit = row.FindControl("txtUnit") as TextBox;

        if (txtUnit != null)
            txtUnit.Text = GetProductUnitName(ddlProduct.SelectedValue);
    }

    protected void ddlSwatchType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlSwatchType = sender as DropDownList;

        if (ddlSwatchType == null)
            return;

        GridViewRow row = ddlSwatchType.NamingContainer as GridViewRow;

        if (row == null)
            return;

        DropDownList ddlSwatch = row.FindControl("ddlSwatch") as DropDownList;

        LoadSwatches(ddlSwatch, ddlSwatchType.SelectedValue);

        SaveProductRows();
    }

    private void SetSelectedValue(DropDownList ddl, object value)
    {
        if (ddl == null || value == null)
            return;

        string selectedValue = value.ToString();

        if (!string.IsNullOrEmpty(selectedValue) && ddl.Items.FindByValue(selectedValue) != null)
            ddl.SelectedValue = selectedValue;
    }

    // =====================================================
    // NAVIGATION / SAVE
    // =====================================================

    protected void btnBack_Click(object sender, EventArgs e)
    {
        SaveProductRows();

        pnlDelivery.Visible = false;
        pnlPurchaseOrder.Visible = true;
    }

    protected void btnFinalSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(hfPK_ID.Value))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Purchase Order ID is missing.");
            return;
        }

        if (string.IsNullOrEmpty(ddlCustomerLocation.SelectedValue))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Delivery Location can not be empty.");
            return;
        }

        if (string.IsNullOrEmpty(ddlTransportation.SelectedValue))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Mode of Transportation can not be empty.");
            return;
        }

        SaveProductRows();

        for (int i = 0; i < grdProducts.Rows.Count; i++)
        {
            GridViewRow row = grdProducts.Rows[i];

            DropDownList ddlProduct = row.FindControl("ddlProduct") as DropDownList;
            TextBox txtQuantity = row.FindControl("txtQuantity") as TextBox;
            TextBox txtDeliveryDate = row.FindControl("txtDeliveryDate") as TextBox;

            if (ddlProduct == null || string.IsNullOrEmpty(ddlProduct.SelectedValue))
            {
                HelperFunction.MsgBox(this, this.GetType(), "Product is required in row " + (i + 1) + ".");
                return;
            }

            decimal qty;
            if (txtQuantity == null || !decimal.TryParse(txtQuantity.Text.Trim(), out qty) || qty <= 0)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Enter a valid quantity in row " + (i + 1) + ".");
                return;
            }

            if (txtDeliveryDate == null || string.IsNullOrWhiteSpace(txtDeliveryDate.Text))
            {
                HelperFunction.MsgBox(this, this.GetType(), "Delivery Date is required in row " + (i + 1) + ".");
                return;
            }

            string[] date = txtDeliveryDate.Text.Trim().Split('/');

            if (date.Length != 3)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Delivery Date must be in dd/mm/yyyy format in row " + (i + 1) + ".");
                return;
            }
        }

        /*
         * FINAL SAVE:
         *
         * 1. Read all product rows.
         * 2. Group rows by Delivery Date.
         * 3. For each unique Delivery Date:
         *
         *    Create PR_MULTIPLE_DELIVERY:
         *      PURCHASE_ORDER_ID = hfPK_ID.Value
         *      CUSOMTER_LOCATION_ID = ddlCustomerLocation.SelectedValue
         *      MODE_OF_TRANSPORTATION_ID = ddlTransportation.SelectedValue
         *      DELIVERY_DATE = row delivery date
         *      DELIVERY_DAY = date[0]
         *      DELIVERY_MONTH = date[1]
         *      DELIVERY_YEAR = date[2]
         *      STATUS = your initial status
         *      REMARKS = txtRemarks.Text
         *
         *    Get the new delivery PK_ID.
         *
         * 4. For every product belonging to that delivery date:
         *
         *    Create PR_PURCHASE_ORDER_DETAIL:
         *      MULTIPLE_DELIVERY_ID = delivery.PK_ID
         *      SNO = row number
         *      PRODUCT_ID = ddlProduct.SelectedValue
         *      SIZE_ID = ddlSize.SelectedValue
         *      SWATCH_ID = ddlSwatch.SelectedValue
         *      QUANTITY = txtQuantity.Text
         *      UNIT = unit id of the product (PRODUCT.UNIT_ID), not txtUnit.Text
         *      STATUS = your initial status
         */

        HelperFunction.MsgBox(this, this.GetType(), "Purchase Order details are ready to be saved.");
    }

    // =====================================================
    // EDIT / CLEAR
    // =====================================================

    private void LoadPurchaseOrder(string pkId)
    {
        PoEnt = new PR_PURCHASE_ORDER();
        PoEnt.PK_ID = pkId;
        PoEnt = (PR_PURCHASE_ORDER)PoSer.GetSingle(PoEnt);

        if (PoEnt == null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Purchase Order not found.");
            return;
        }

        hfPK_ID.Value = pkId;

        if (ddlCustomer.Items.FindByValue(PoEnt.CUSTOMER_ID) != null)
        {
            ddlCustomer.SelectedValue = PoEnt.CUSTOMER_ID;
            LoadCustomerDetails(PoEnt.CUSTOMER_ID);
        }

        txtOrderDate.Text = NepDate(PoEnt.ORDER_DAY, PoEnt.ORDER_MONTH, PoEnt.ORDER_YEAR);
        txtPaymentTerm.Text = PoEnt.PAYMENT_TERM;

        lblFormTitle.Text = "Edit Purchase Order";
        btnNext.Text = "Next";
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        hfPK_ID.Value = "";
        txtOrderDate.Text = GetTodayNepali();
        txtPaymentTerm.Text = "";
        txtRemarks.Text = "";
        ddlCustomer.SelectedIndex = 0;

        ClearCustomerDetails();

        ddlCustomerLocation.Items.Clear();
        ddlTransportation.SelectedIndex = 0;

        ClearDeliveryDetails();

        ClearProductState();
        ProductTable = null;
        grdProducts.DataSource = null;
        grdProducts.DataBind();

        pnlCustomerDetails.Visible = false;
        pnlDelivery.Visible = false;
        pnlPurchaseOrder.Visible = true;

        lblFormTitle.Text = "Purchase Order Information";
        btnNext.Text = "Next";
    }

    protected string NepDate(object day, object month, object year)
    {
        if (day == null || month == null || year == null)
            return "";

        string d = day.ToString().Trim();
        string m = month.ToString().Trim();
        string y = year.ToString().Trim();

        if (d == "" || m == "" || y == "")
            return "";

        return d.PadLeft(2, '0') + "/" + m.PadLeft(2, '0') + "/" + y;
    }
}