using System;
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
    PR_ORDER_DETAIL OdEnt = new PR_ORDER_DETAIL();
    PR_ORDER_DETAILService OdSer = new PR_ORDER_DETAILService();
    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    PR_CUSTOMER_LOCATION ClEnt = new PR_CUSTOMER_LOCATION();
    PR_CUSTOMER_LOCATIONService ClSer = new PR_CUSTOMER_LOCATIONService();

    PR_MULTIPLE_DELIVERY MdEnt = new PR_MULTIPLE_DELIVERY();
    PR_MULTIPLE_DELIVERYService MdSer = new PR_MULTIPLE_DELIVERYService();

    PhyeGanDate PGD = new PhyeGanDate();
    PAYMENT_TYPE PtEnt = new PAYMENT_TYPE();
    PAYMENT_TYPEService PtSer = new PAYMENT_TYPEService();

    private const string ProductTableKey = "ProductTableKey";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCustomer(ddlCustomer);
            LoadPaymentTerm();
            LoadTransportation();
            LoadCountry();

            txtOrderDate.Text = PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy");
            txtDeliveryDate.Text = PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy");

            InitializeProductTable();
            ShowStep(1);
        }
    }

    private void ShowStep(int step)
    {
        pnlPurchaseOrder.Visible = (step == 1);
        pnlDeliveryAndProducts.Visible = (step == 2);
    }

    private void LoadPaymentTerm()
    {
        PtEnt = new PAYMENT_TYPE();
        PtEnt.SALES_PURCHASE = "P";
        ddlPaymentTerm.DataSource = PtSer.GetAll(PtEnt);
        ddlPaymentTerm.DataTextField = "PAYMENT_NAME";
        ddlPaymentTerm.DataValueField = "PK_ID";
        ddlPaymentTerm.DataBind();
        ddlPaymentTerm.Items.Insert(0, new ListItem("-- Select Payment Term --", ""));
    }

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

    private void LoadCountry()
    {
        COUNTRY ent = new COUNTRY();
        COUNTRYService ser = new COUNTRYService();
        ddlDeliveryCountry.DataSource = ser.GetAll(ent);
        ddlDeliveryCountry.DataTextField = "COUNTRY_NAME";
        ddlDeliveryCountry.DataValueField = "PK_ID";
        ddlDeliveryCountry.DataBind();
        ddlDeliveryCountry.Items.Insert(0, new ListItem("-- Select Country --", ""));
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

        lblCustomerID.Text = CEnt.PK_ID;
        lblCustomerName.Text = CEnt.CUSTOMER_NAME;
        lblCustomerAddress.Text = CEnt.ADDRESS;
        lblCustomerPhone.Text = CEnt.PHONE;
        lblCustomerEmail.Text = CEnt.EMAIL;
        pnlCustomerDetails.Visible = true;

        txtDeliveryAddress.Text = CEnt.ADDRESS;
        txtDeliveryContactPerson.Text = CEnt.CONTACT_PERSON;
        txtDeliveryContactNumber.Text = !string.IsNullOrEmpty(CEnt.PHONE) ? CEnt.PHONE : CEnt.MOBILE;
        txtDeliveryEmail.Text = CEnt.EMAIL;
    }

    private void ClearCustomerDetails()
    {
        lblCustomerID.Text = "";
        lblCustomerName.Text = "";
        lblCustomerAddress.Text = "";
        lblCustomerPhone.Text = "";
        lblCustomerEmail.Text = "";
    }

    protected void btnNextMaster_Click(object sender, EventArgs e)
    {
        string customerId = ddlCustomer.SelectedValue;
        string[] nepDate = (txtOrderDate.Text.Trim() + "//").Split('/');

        if (string.IsNullOrEmpty(hfPK_ID.Value))
        {
            PoEnt = new PR_PURCHASE_ORDER();
            EntityList thlisst = PoSer.GetAll(PoEnt);
            string pid = "";
            int id = 0;
            foreach (PR_PURCHASE_ORDER row in thlisst)
            {
                pid = row.PK_ID;
            }

            int.TryParse(pid, out id);
            id = id + 1;

            PoEnt.CUSTOMER_ORDER_NUMBER = id.ToString();
            PoEnt.CUSTOMER_ID = customerId;
            PoEnt.ORDER_DATE = PGD.GetEnglishDateFromNepali(txtOrderDate.Text, "dd/mm/yyyy");
            PoEnt.ORDER_DAY = nepDate[0];
            PoEnt.ORDER_MONTH = nepDate[1];
            PoEnt.ORDER_YEAR = nepDate[2];
            PoEnt.ORDER_FISCAL_YEAR = PGD.checkFiscalYear(nepDate[1], nepDate[2]);
            PoEnt.PAYMENT_TERM = ddlPaymentTerm.SelectedValue;
            PoEnt.ORDER_NUMBER = id.ToString();

            PoSer.Insert(PoEnt);

            PR_PURCHASE_ORDER PooEnt = new PR_PURCHASE_ORDER();
            EntityList tthlisst = PoSer.GetAll(PooEnt);
            foreach (PR_PURCHASE_ORDER row in tthlisst)
            {
                pid = row.PK_ID;
            }
            hfPK_ID.Value = pid;
        }

        BindProducts();
        ShowStep(2);
    }

    private DataTable CreateProductTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ROW_ID", typeof(string));
        dt.Columns.Add("PRODUCT_ID", typeof(string));
        dt.Columns.Add("SIZE_ID", typeof(string));
        dt.Columns.Add("SWATCH_TYPE_ID", typeof(string));
        dt.Columns.Add("SWATCH_ID", typeof(string));
        dt.Columns.Add("QUANTITY", typeof(string));
        dt.Columns.Add("UNIT", typeof(string));
        return dt;
    }

    private DataTable ProductTable
    {
        get
        {
            if (ViewState[ProductTableKey] == null)
                InitializeProductTable();

            return (DataTable)ViewState[ProductTableKey];
        }
        set { ViewState[ProductTableKey] = value; }
    }

    private void InitializeProductTable()
    {
        DataTable dt = CreateProductTable();
        DataRow nr = dt.NewRow();
        nr["ROW_ID"] = Guid.NewGuid().ToString("N");
        nr["PRODUCT_ID"] = "";
        nr["SIZE_ID"] = "";
        nr["SWATCH_TYPE_ID"] = "";
        nr["SWATCH_ID"] = "";
        nr["QUANTITY"] = "";
        nr["UNIT"] = "";
        dt.Rows.Add(nr);
        ProductTable = dt;
    }

    private void BindProducts()
    {
        grdProducts.DataSource = ProductTable;
        grdProducts.DataBind();
    }

    private void ReadProducts()
    {
        DataTable pTable = ProductTable;

        foreach (GridViewRow row in grdProducts.Rows)
        {
            HiddenField hfProductRowId = row.FindControl("hfProductRowId") as HiddenField;
            if (hfProductRowId == null) continue;

            DataRow[] matches = pTable.Select("ROW_ID = '" + hfProductRowId.Value.Replace("'", "''") + "'");
            if (matches.Length == 0) continue;

            DataRow pRow = matches[0];
            pRow["PRODUCT_ID"] = GetSelectedValue(row, "ddlProduct");
            pRow["SIZE_ID"] = GetSelectedValue(row, "ddlSize");
            pRow["SWATCH_TYPE_ID"] = GetSelectedValue(row, "ddlSwatchType");
            pRow["SWATCH_ID"] = GetSelectedValue(row, "ddlSwatch");
            pRow["QUANTITY"] = GetText(row, "txtQuantity");
            HiddenField hfUnitId = row.FindControl("hfUnitId") as HiddenField;
            pRow["UNIT"] = hfUnitId != null ? hfUnitId.Value : "";
        }

        ProductTable = pTable;
    }

    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        ReadProducts();

        DataTable dt = ProductTable;
        DataRow nr = dt.NewRow();
        nr["ROW_ID"] = Guid.NewGuid().ToString("N");
        nr["PRODUCT_ID"] = "";
        nr["SIZE_ID"] = "";
        nr["SWATCH_TYPE_ID"] = "";
        nr["SWATCH_ID"] = "";
        nr["QUANTITY"] = "";
        nr["UNIT"] = "";
        dt.Rows.Add(nr);

        ProductTable = dt;
        BindProducts();
    }

    protected void grdProducts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RemoveProductRow")
        {
            ReadProducts();

            string rowId = Convert.ToString(e.CommandArgument);
            DataTable dt = ProductTable;

            DataRow[] rows = dt.Select("ROW_ID = '" + rowId.Replace("'", "''") + "'");
            if (rows.Length > 0)
                dt.Rows.Remove(rows[0]);

            if (dt.Rows.Count == 0)
            {
                DataRow nr = dt.NewRow();
                nr["ROW_ID"] = Guid.NewGuid().ToString("N");
                nr["PRODUCT_ID"] = "";
                nr["SIZE_ID"] = "";
                nr["SWATCH_TYPE_ID"] = "";
                nr["SWATCH_ID"] = "";
                nr["QUANTITY"] = "";
                nr["UNIT"] = "";
                dt.Rows.Add(nr);
            }

            ProductTable = dt;
            BindProducts();
        }
    }

    protected void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        DropDownList ddlProduct = e.Row.FindControl("ddlProduct") as DropDownList;
        DropDownList ddlSize = e.Row.FindControl("ddlSize") as DropDownList;
        DropDownList ddlSwatchType = e.Row.FindControl("ddlSwatchType") as DropDownList;
        DropDownList ddlSwatch = e.Row.FindControl("ddlSwatch") as DropDownList;

        LoadProducts(ddlProduct);
        LoadSizes(ddlSize);
        LoadSwatchTypes(ddlSwatchType);

        DataRowView rowView = e.Row.DataItem as DataRowView;
        if (rowView != null)
        {
            SetSelectedValue(ddlProduct, rowView["PRODUCT_ID"]);
            SetSelectedValue(ddlSize, rowView["SIZE_ID"]);

            string swatchTypeId = Convert.ToString(rowView["SWATCH_TYPE_ID"]);
            SetSelectedValue(ddlSwatchType, swatchTypeId);

            if (!string.IsNullOrEmpty(swatchTypeId))
            {
                LoadSwatches(ddlSwatch, swatchTypeId);
                ddlSwatch.Enabled = true;
                SetSelectedValue(ddlSwatch, rowView["SWATCH_ID"]);
            }
            else
            {
                ddlSwatch.Items.Clear();
                ddlSwatch.Items.Insert(0, new ListItem("-- Select Swatch --", ""));
                ddlSwatch.Enabled = false;
            }

            TextBox txtUnit = e.Row.FindControl("txtUnit") as TextBox;
            TextBox txtQuantity = e.Row.FindControl("txtQuantity") as TextBox;

            if (txtUnit != null)
                txtUnit.Text = GetUnitName(Convert.ToString(rowView["UNIT"]));
            if (txtQuantity != null)
                txtQuantity.Text = Convert.ToString(rowView["QUANTITY"]);
        }
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

    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        ReadProducts();

        DropDownList ddlProduct = sender as DropDownList;
        GridViewRow row = ddlProduct != null ? ddlProduct.NamingContainer as GridViewRow : null;
        if (row == null) return;

        HiddenField hfProductRowId = row.FindControl("hfProductRowId") as HiddenField;
        if (hfProductRowId == null) return;

        string unitId = "";
        if (!string.IsNullOrEmpty(ddlProduct.SelectedValue))
        {
            PRODUCT ent = new PRODUCT();
            ent.PK_ID = ddlProduct.SelectedValue;
            ent = (PRODUCT)new PRODUCTService().GetSingle(ent);
            if (ent != null)
                unitId = ent.UNIT_ID;
        }

        DataRow[] matches = ProductTable.Select("ROW_ID = '" + hfProductRowId.Value.Replace("'", "''") + "'");
        if (matches.Length > 0)
        {
            matches[0]["PRODUCT_ID"] = ddlProduct.SelectedValue;
            matches[0]["UNIT"] = unitId;
        }

        BindProducts();
    }

    protected void ddlSwatchType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ReadProducts();

        DropDownList ddlSwatchType = sender as DropDownList;
        GridViewRow row = ddlSwatchType != null ? ddlSwatchType.NamingContainer as GridViewRow : null;
        if (row == null) return;

        HiddenField hfProductRowId = row.FindControl("hfProductRowId") as HiddenField;
        if (hfProductRowId == null) return;

        DataRow[] matches = ProductTable.Select("ROW_ID = '" + hfProductRowId.Value.Replace("'", "''") + "'");
        if (matches.Length > 0)
        {
            matches[0]["SWATCH_TYPE_ID"] = ddlSwatchType.SelectedValue;
            matches[0]["SWATCH_ID"] = "";
        }

        BindProducts();
    }

    private string GetUnitName(string unitId)
    {
        if (string.IsNullOrEmpty(unitId)) return "";
        PRODUCT_UNIT ent = new PRODUCT_UNIT();
        ent.PK_ID = unitId;
        ent = (PRODUCT_UNIT)new PRODUCT_UNITService().GetSingle(ent);

        return ent != null ? ent.UNIT_NAME : "";
    }

    private void SetSelectedValue(DropDownList ddl, object value)
    {
        if (ddl == null || value == null)
            return;

        string selectedValue = value.ToString();
        if (!string.IsNullOrEmpty(selectedValue) && ddl.Items.FindByValue(selectedValue) != null)
            ddl.SelectedValue = selectedValue;
    }

    private string GetSelectedValue(GridViewRow row, string id)
    {
        DropDownList ddl = row.FindControl(id) as DropDownList;
        return ddl != null ? ddl.SelectedValue : "";
    }

    private string GetText(GridViewRow row, string id)
    {
        TextBox txt = row.FindControl(id) as TextBox;
        return txt != null ? txt.Text.Trim() : "";
    }

    protected void btnBackDelivery_Click(object sender, EventArgs e)
    {
        ReadProducts();
        ShowStep(1);
    }

    /// <summary>
    /// Common persistence routine for inserting Delivery Location, Multiple Delivery, and Order Details
    /// </summary>
    private bool SaveCurrentDeliveryRecord()
    {
        ReadProducts();

        string purchaseOrderId = hfPK_ID.Value;
        if (string.IsNullOrEmpty(purchaseOrderId))
            return false;

        // 1. Insert PR_CUSTOMER_LOCATION
        ClEnt = new PR_CUSTOMER_LOCATION();
        ClEnt.CUSTOMER_ID = ddlCustomer.SelectedValue;
        ClEnt.ADDRESS = txtDeliveryAddress.Text.Trim();
        ClEnt.COUNTRY_ID = ddlDeliveryCountry.SelectedValue;
        ClEnt.CONTACT_PERSON = txtDeliveryContactPerson.Text.Trim();
        ClEnt.CONTACT_NUMBER = txtDeliveryContactNumber.Text.Trim();
        ClEnt.EMAIL_ID = txtDeliveryEmail.Text.Trim();
        ClSer.Insert(ClEnt);

        string locationId = ClEnt.PK_ID;

        // 2. Insert PR_MULTIPLE_DELIVERY
        string nepDeliveryDate = txtDeliveryDate.Text.Trim();
        string[] delivDateParts = (nepDeliveryDate + "//").Split('/');

        MdEnt = new PR_MULTIPLE_DELIVERY();
        MdEnt.PURCHASE_ORDER_ID = purchaseOrderId;
        MdEnt.CUSOMTER_LOCATION_ID = locationId;
        MdEnt.MODE_OF_TRANSPORTATION_ID = ddlTransportation.SelectedValue;
        MdEnt.DELIVERY_DATE = PGD.GetEnglishDateFromNepali(nepDeliveryDate, "dd/mm/yyyy");
        MdEnt.DELIVERY_DAY = delivDateParts[0];
        MdEnt.DELIVERY_MONTH = delivDateParts[1];
        MdEnt.DELIVERY_YEAR = delivDateParts[2];
        MdEnt.STATUS = "1";
        MdEnt.REMARKS = txtRemarks.Text.Trim();
        MdSer.Insert(MdEnt);

        EntityList tlst = new EntityList();
        MdEnt = new PR_MULTIPLE_DELIVERY();
        tlst = MdSer.GetAll(MdEnt);
        string mdid = "";
        foreach (PR_MULTIPLE_DELIVERY row in tlst)
        {
            mdid = row.PK_ID;
        }

        // 3. Insert PR_ORDER_DETAIL
        DataTable pTable = ProductTable;
        foreach (DataRow pRow in pTable.Rows)
        {
            if (string.IsNullOrEmpty(Convert.ToString(pRow["PRODUCT_ID"])))
                continue;

            OdEnt = new PR_ORDER_DETAIL();
            OdEnt.MULTIPLE_DELVIERY_ID = mdid;
            OdEnt.PRODUCT_ID = Convert.ToString(pRow["PRODUCT_ID"]);
            OdEnt.SIZE_ID = Convert.ToString(pRow["SIZE_ID"]);
            OdEnt.SWATCH_ID = Convert.ToString(pRow["SWATCH_ID"]);
            OdEnt.UNIT = Convert.ToString(pRow["UNIT"]);
            OdEnt.QUANTITY = Convert.ToString(pRow["QUANTITY"]);
            OdEnt.STATUS = "1";
            OdSer.Insert(OdEnt);
        }

        return true;
    }

    /// <summary>
    /// Saves current delivery and resets Step 2 controls while keeping the same PO
    /// </summary>
    protected void btnSaveAndNextDelivery_Click(object sender, EventArgs e)
    {
        if (SaveCurrentDeliveryRecord())
        {
            ResetDeliveryAndProductsSection();
            HelperFunction.MsgBox(this, this.GetType(), "Delivery details saved. You can now add the next delivery schedule.");
        }
    }

    /// <summary>
    /// Saves current delivery and completes the entire purchase order workflow
    /// </summary>
    protected void btnFinalSave_Click(object sender, EventArgs e)
    {
        if (SaveCurrentDeliveryRecord())
        {
            HelperFunction.MsgBox(this, this.GetType(), "Purchase Order saved successfully.");
            ClearForm();
        }
    }

    private void ResetDeliveryAndProductsSection()
    {
        txtDeliveryDate.Text = PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy");
        ddlTransportation.SelectedIndex = 0;
        txtRemarks.Text = "";

        // Reset customer address defaults if customer is selected
        string customerId = ddlCustomer.SelectedValue;
        if (!string.IsNullOrEmpty(customerId))
        {
            CEnt = new CUSTOMER();
            CEnt.PK_ID = customerId;
            CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
            if (CEnt != null)
            {
                txtDeliveryAddress.Text = CEnt.ADDRESS;
                txtDeliveryContactPerson.Text = CEnt.CONTACT_PERSON;
                txtDeliveryContactNumber.Text = !string.IsNullOrEmpty(CEnt.PHONE) ? CEnt.PHONE : CEnt.MOBILE;
                txtDeliveryEmail.Text = CEnt.EMAIL;
            }
        }
        else
        {
            txtDeliveryAddress.Text = "";
            txtDeliveryContactPerson.Text = "";
            txtDeliveryContactNumber.Text = "";
            txtDeliveryEmail.Text = "";
            ddlDeliveryCountry.SelectedIndex = 0;
        }

        InitializeProductTable();
        BindProducts();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        hfPK_ID.Value = "";
        ViewState.Remove(ProductTableKey);

        txtOrderDate.Text = PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy");
        txtDeliveryDate.Text = PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy");
        ddlPaymentTerm.SelectedIndex = 0;
        ddlCustomer.SelectedIndex = 0;
        ddlTransportation.SelectedIndex = 0;
        ddlDeliveryCountry.SelectedIndex = 0;

        txtDeliveryAddress.Text = "";
        txtDeliveryContactPerson.Text = "";
        txtDeliveryContactNumber.Text = "";
        txtDeliveryEmail.Text = "";
        txtRemarks.Text = "";

        ClearCustomerDetails();
        InitializeProductTable();

        ShowStep(1);
    }
}