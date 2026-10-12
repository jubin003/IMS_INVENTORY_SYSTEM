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

    private const string DeliveryTableKey = "DeliveryTableKey";
    private const string ProductTableKey = "ProductTableKey";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCustomer(ddlCustomer);
            LoadPaymentTerm();

            string pkId = Request.QueryString["PK_ID"];

                txtOrderDate.Text = PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy");

            InitializeDeliveryTable();
            InitializeProductTable();

            ShowStep(1);
        }
    }

    private void ShowStep(int step)
    {
        pnlPurchaseOrder.Visible = (step == 1);
        pnlDelivery.Visible = (step == 2);
        pnlOrderDetails.Visible = (step == 3);
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

        DataTable dt = DeliveryTable;
        if (dt.Rows.Count > 0)
        {
            dt.Rows[0]["ADDRESS"] = CEnt.ADDRESS;
            dt.Rows[0]["CONTACT_PERSON"] = CEnt.CONTACT_PERSON;
            dt.Rows[0]["CONTACT_NUMBER"] = !string.IsNullOrEmpty(CEnt.PHONE) ? CEnt.PHONE : CEnt.MOBILE;
            dt.Rows[0]["EMAIL_ID"] = CEnt.EMAIL;
            DeliveryTable = dt;
        }
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

        BindDeliveries();
        ShowStep(2);
    }

    private DataTable CreateDeliveryTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("DELIVERY_ROW_ID", typeof(string));
        dt.Columns.Add("DELIVERY_DATE", typeof(string));
        dt.Columns.Add("MODE_OF_TRANSPORTATION_ID", typeof(string));
        dt.Columns.Add("ADDRESS", typeof(string));
        dt.Columns.Add("COUNTRY_ID", typeof(string));
        dt.Columns.Add("CONTACT_PERSON", typeof(string));
        dt.Columns.Add("CONTACT_NUMBER", typeof(string));
        dt.Columns.Add("EMAIL_ID", typeof(string));
        dt.Columns.Add("REMARKS", typeof(string));
        return dt;
    }

    private DataTable DeliveryTable
    {
        get
        {
            if (ViewState[DeliveryTableKey] == null)
                InitializeDeliveryTable();

            return (DataTable)ViewState[DeliveryTableKey];
        }
        set { ViewState[DeliveryTableKey] = value; }
    }

    private void InitializeDeliveryTable()
    {
        DataTable dt = CreateDeliveryTable();
        dt.Rows.Add(Guid.NewGuid().ToString("N"), PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy"), "", "", "", "", "", "", "");
        DeliveryTable = dt;
    }

    private void BindDeliveries()
    {
        rptDeliveries.DataSource = DeliveryTable;
        rptDeliveries.DataBind();
    }

    private void ReadDeliveries()
    {
        DataTable dt = DeliveryTable;

        for (int i = 0; i < rptDeliveries.Items.Count; i++)
        {
            RepeaterItem item = rptDeliveries.Items[i];

            if (i >= dt.Rows.Count)
                dt.Rows.Add(Guid.NewGuid().ToString("N"), "", "", "", "", "", "", "", "");

            TextBox txtDeliveryDate = item.FindControl("txtDeliveryDate") as TextBox;
            DropDownList ddlTransportation = item.FindControl("ddlTransportation") as DropDownList;
            TextBox txtDeliveryAddress = item.FindControl("txtDeliveryAddress") as TextBox;
            DropDownList ddlDeliveryCountry = item.FindControl("ddlDeliveryCountry") as DropDownList;
            TextBox txtDeliveryContactPerson = item.FindControl("txtDeliveryContactPerson") as TextBox;
            TextBox txtDeliveryContactNumber = item.FindControl("txtDeliveryContactNumber") as TextBox;
            TextBox txtDeliveryEmail = item.FindControl("txtDeliveryEmail") as TextBox;
            TextBox txtRemarks = item.FindControl("txtRemarks") as TextBox;

            dt.Rows[i]["DELIVERY_DATE"] = txtDeliveryDate != null ? txtDeliveryDate.Text.Trim() : "";
            dt.Rows[i]["MODE_OF_TRANSPORTATION_ID"] = ddlTransportation != null ? ddlTransportation.SelectedValue : "";
            dt.Rows[i]["ADDRESS"] = txtDeliveryAddress != null ? txtDeliveryAddress.Text.Trim() : "";
            dt.Rows[i]["COUNTRY_ID"] = ddlDeliveryCountry != null ? ddlDeliveryCountry.SelectedValue : "";
            dt.Rows[i]["CONTACT_PERSON"] = txtDeliveryContactPerson != null ? txtDeliveryContactPerson.Text.Trim() : "";
            dt.Rows[i]["CONTACT_NUMBER"] = txtDeliveryContactNumber != null ? txtDeliveryContactNumber.Text.Trim() : "";
            dt.Rows[i]["EMAIL_ID"] = txtDeliveryEmail != null ? txtDeliveryEmail.Text.Trim() : "";
            dt.Rows[i]["REMARKS"] = txtRemarks != null ? txtRemarks.Text.Trim() : "";
        }

        DeliveryTable = dt;
    }

    protected void rptDeliveries_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        Label lblDeliveryCardTitle = e.Item.FindControl("lblDeliveryCardTitle") as Label;
        if (lblDeliveryCardTitle != null)
            lblDeliveryCardTitle.Text = "Delivery Date #" + (e.Item.ItemIndex + 1);

        DropDownList ddlTransportation = e.Item.FindControl("ddlTransportation") as DropDownList;
        if (ddlTransportation != null)
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

        DropDownList ddlDeliveryCountry = e.Item.FindControl("ddlDeliveryCountry") as DropDownList;
        if (ddlDeliveryCountry != null)
        {
            COUNTRY ent = new COUNTRY();
            COUNTRYService ser = new COUNTRYService();
            ddlDeliveryCountry.DataSource = ser.GetAll(ent);
            ddlDeliveryCountry.DataTextField = "COUNTRY_NAME";
            ddlDeliveryCountry.DataValueField = "PK_ID";
            ddlDeliveryCountry.DataBind();
            ddlDeliveryCountry.Items.Insert(0, new ListItem("-- Select Country --", ""));
        }

        DataRowView rowView = e.Item.DataItem as DataRowView;
        if (rowView != null)
        {
            SetSelectedValue(ddlTransportation, rowView["MODE_OF_TRANSPORTATION_ID"]);
            SetSelectedValue(ddlDeliveryCountry, rowView["COUNTRY_ID"]);
        }
    }

    protected void btnAddDelivery_Click(object sender, EventArgs e)
    {
        ReadDeliveries();

        DataTable dt = DeliveryTable;
        if (dt.Rows.Count > 0)
        {
            DataRow lastRow = dt.Rows[dt.Rows.Count - 1];
            DataRow newRow = dt.NewRow();
            newRow.ItemArray = lastRow.ItemArray.Clone() as object[];
            newRow["DELIVERY_ROW_ID"] = Guid.NewGuid().ToString("N");
            newRow["DELIVERY_DATE"] = PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy");
            dt.Rows.Add(newRow);
        }
        else
        {
            dt.Rows.Add(Guid.NewGuid().ToString("N"), PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy"), "", "", "", "", "", "", "");
        }

        DeliveryTable = dt;
        BindDeliveries();
    }

    protected void rptDeliveries_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "RemoveDelivery")
            return;

        ReadDeliveries();
        int index = Convert.ToInt32(e.CommandArgument);
        DataTable dt = DeliveryTable;

        if (index >= 0 && index < dt.Rows.Count)
        {
            string delRowId = Convert.ToString(dt.Rows[index]["DELIVERY_ROW_ID"]);
            dt.Rows.RemoveAt(index);

            DataTable pTable = ProductTable;
            DataRow[] linked = pTable.Select("DELIVERY_ROW_ID = '" + delRowId.Replace("'", "''") + "'");
            foreach (DataRow r in linked)
                pTable.Rows.Remove(r);
            ProductTable = pTable;
        }

        if (dt.Rows.Count == 0)
            dt.Rows.Add(Guid.NewGuid().ToString("N"), PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy"), "", "", "", "", "", "", "");

        DeliveryTable = dt;
        BindDeliveries();
    }

    protected void btnBackDelivery_Click(object sender, EventArgs e)
    {
        ReadDeliveries();
        ShowStep(1);
    }

    protected void btnNextDelivery_Click(object sender, EventArgs e)
    {
        ReadDeliveries();
        EnsureProductRowsForDeliveries();
        BindDeliveryProducts();
        ShowStep(3);
    }

    private string GetUnitName(string unitId)
    {

        PRODUCT_UNIT ent = new PRODUCT_UNIT();
        ent.PK_ID = unitId;
        ent = (PRODUCT_UNIT)new PRODUCT_UNITService().GetSingle(ent);

        return ent.UNIT_NAME;
    }
    private DataTable CreateProductTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ROW_ID", typeof(string));
        dt.Columns.Add("DELIVERY_ROW_ID", typeof(string));
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
        ProductTable = CreateProductTable();
    }

    private void EnsureProductRowsForDeliveries()
    {
        DataTable dTable = DeliveryTable;
        DataTable pTable = ProductTable;

        foreach (DataRow dRow in dTable.Rows)
        {
            string dId = Convert.ToString(dRow["DELIVERY_ROW_ID"]);
            DataRow[] existing = pTable.Select("DELIVERY_ROW_ID = '" + dId.Replace("'", "''") + "'");
            if (existing.Length == 0)
            {
                DataRow nr = pTable.NewRow();
                nr["ROW_ID"] = Guid.NewGuid().ToString("N");
                nr["DELIVERY_ROW_ID"] = dId;
                nr["PRODUCT_ID"] = "";
                nr["SIZE_ID"] = "";
                nr["SWATCH_TYPE_ID"] = "";
                nr["SWATCH_ID"] = "";
                nr["QUANTITY"] = "";
                nr["UNIT"] = "";
                pTable.Rows.Add(nr);
            }
        }

        ProductTable = pTable;
    }

    private void BindDeliveryProducts()
    {
        rptDeliveryProducts.DataSource = DeliveryTable;
        rptDeliveryProducts.DataBind();
    }

    protected void rptDeliveryProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        DataRowView dRow = e.Item.DataItem as DataRowView;
        if (dRow == null)
            return;

        string deliveryRowId = Convert.ToString(dRow["DELIVERY_ROW_ID"]);

        Label lblTitle = e.Item.FindControl("lblDeliveryDateTitle") as Label;
        if (lblTitle != null)
            lblTitle.Text = "Products for Delivery Date: " + Convert.ToString(dRow["DELIVERY_DATE"]) + " (#" + (e.Item.ItemIndex + 1) + ")";

        GridView grid = e.Item.FindControl("grdProducts") as GridView;
        if (grid != null)
        {
            DataTable pTable = ProductTable;
            DataRow[] rows = pTable.Select("DELIVERY_ROW_ID = '" + deliveryRowId.Replace("'", "''") + "'");

            DataTable filtered = rows.Length > 0 ? rows.CopyToDataTable() : CreateProductTable();
            grid.DataSource = filtered;
            grid.DataBind();
        }
    }

    protected void rptDeliveryProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "AddProductForDate")
        {
            ReadStep3Products();

            string deliveryRowId = Convert.ToString(e.CommandArgument);
            DataTable dt = ProductTable;

            DataRow nr = dt.NewRow();
            nr["ROW_ID"] = Guid.NewGuid().ToString("N");
            nr["DELIVERY_ROW_ID"] = deliveryRowId;
            nr["PRODUCT_ID"] = "";
            nr["SIZE_ID"] = "";
            nr["SWATCH_TYPE_ID"] = "";
            nr["SWATCH_ID"] = "";
            nr["QUANTITY"] = "";
            nr["UNIT"] = "";
            dt.Rows.Add(nr);

            ProductTable = dt;
            BindDeliveryProducts();
        }
    }

    protected void grdProducts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RemoveProductRow")
        {
            ReadStep3Products();

            string rowId = Convert.ToString(e.CommandArgument);
            DataTable dt = ProductTable;

            DataRow[] rows = dt.Select("ROW_ID = '" + rowId.Replace("'", "''") + "'");
            if (rows.Length > 0)
                dt.Rows.Remove(rows[0]);

            ProductTable = dt;
            BindDeliveryProducts();
        }
    }

    private void ReadStep3Products()
    {
        DataTable pTable = ProductTable;

        foreach (RepeaterItem item in rptDeliveryProducts.Items)
        {
            HiddenField hfDeliveryRowId = item.FindControl("hfDeliveryRowId") as HiddenField;
            GridView grid = item.FindControl("grdProducts") as GridView;

            if (hfDeliveryRowId == null || grid == null)
                continue;

            string dId = hfDeliveryRowId.Value;

            foreach (GridViewRow row in grid.Rows)
            {
                HiddenField hfProductRowId = row.FindControl("hfProductRowId") as HiddenField;
                if (hfProductRowId == null)
                    continue;

                DataRow[] matches = pTable.Select("ROW_ID = '" + hfProductRowId.Value.Replace("'", "''") + "'");
                if (matches.Length == 0)
                    continue;

                DataRow pRow = matches[0];
                pRow["DELIVERY_ROW_ID"] = dId;
                pRow["PRODUCT_ID"] = GetSelectedValue(row, "ddlProduct");
                pRow["SIZE_ID"] = GetSelectedValue(row, "ddlSize");
                pRow["SWATCH_TYPE_ID"] = GetSelectedValue(row, "ddlSwatchType");
                pRow["SWATCH_ID"] = GetSelectedValue(row, "ddlSwatch");
                pRow["QUANTITY"] = GetText(row, "txtQuantity");
                HiddenField hfUnitId = row.FindControl("hfUnitId") as HiddenField;
                pRow["UNIT"] = hfUnitId != null ? hfUnitId.Value : "";
            }
        }

        ProductTable = pTable;
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
        DropDownList ddlProduct = sender as DropDownList;
        GridViewRow row = ddlProduct != null ? ddlProduct.NamingContainer as GridViewRow : null;
        if (row == null) return;

        TextBox txtUnit = row.FindControl("txtUnit") as TextBox;
        HiddenField hfUnitId = row.FindControl("hfUnitId") as HiddenField;

        string unitId = "";

        if (!string.IsNullOrEmpty(ddlProduct.SelectedValue))
        {
            PRODUCT ent = new PRODUCT();
            ent.PK_ID = ddlProduct.SelectedValue;
            ent = (PRODUCT)new PRODUCTService().GetSingle(ent);

            unitId = ent.UNIT_ID;
        }

        hfUnitId.Value = unitId;
        txtUnit.Text = GetUnitName(unitId);
    }

    protected void ddlSwatchType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlSwatchType = sender as DropDownList;
        GridViewRow row = ddlSwatchType != null ? ddlSwatchType.NamingContainer as GridViewRow : null;
        if (row == null) return;

        DropDownList ddlSwatch = row.FindControl("ddlSwatch") as DropDownList;
        if (ddlSwatch == null) return;

        string selectedType = ddlSwatchType.SelectedValue;
        if (!string.IsNullOrEmpty(selectedType))
        {
            LoadSwatches(ddlSwatch, selectedType);
            ddlSwatch.Enabled = true;
        }
        else
        {
            ddlSwatch.Items.Clear();
            ddlSwatch.Items.Insert(0, new ListItem("-- Select Swatch --", ""));
            ddlSwatch.Enabled = false;
        }
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

    protected void btnBackDetails_Click(object sender, EventArgs e)
    {
        ReadStep3Products();
        BindDeliveries();
        ShowStep(2);
    }
    protected void btnFinalSave_Click(object sender, EventArgs e)
    {
        ReadStep3Products();

        DataTable dTable = DeliveryTable;
        DataTable pTable = ProductTable;

        // PK_ID of PR_PURCHASE_ORDER, saved in step 1
        string purchaseOrderId = hfPK_ID.Value;

        foreach (DataRow dRow in dTable.Rows)
        {
            string deliveryRowId = Convert.ToString(dRow["DELIVERY_ROW_ID"]);

            // 1. Insert PR_CUSTOMER_LOCATION
            ClEnt = new PR_CUSTOMER_LOCATION();
            ClEnt.CUSTOMER_ID = ddlCustomer.SelectedValue;
            ClEnt.ADDRESS = Convert.ToString(dRow["ADDRESS"]);
            ClEnt.COUNTRY_ID = Convert.ToString(dRow["COUNTRY_ID"]);
            ClEnt.CONTACT_PERSON = Convert.ToString(dRow["CONTACT_PERSON"]);
            ClEnt.CONTACT_NUMBER = Convert.ToString(dRow["CONTACT_NUMBER"]);
            ClEnt.EMAIL_ID = Convert.ToString(dRow["EMAIL_ID"]);
            ClSer.Insert(ClEnt);

            string locationId = ClEnt.PK_ID;

            // 2. Insert PR_MULTIPLE_DELIVERY -> PURCHASE_ORDER_ID = PR_PURCHASE_ORDER.PK_ID
            string nepDeliveryDate = Convert.ToString(dRow["DELIVERY_DATE"]);
            string[] delivDateParts = (nepDeliveryDate + "//").Split('/');

            MdEnt = new PR_MULTIPLE_DELIVERY();
            MdEnt.PURCHASE_ORDER_ID = purchaseOrderId;
            MdEnt.CUSOMTER_LOCATION_ID = locationId;
            MdEnt.MODE_OF_TRANSPORTATION_ID = Convert.ToString(dRow["MODE_OF_TRANSPORTATION_ID"]);
            MdEnt.DELIVERY_DATE = PGD.GetEnglishDateFromNepali(nepDeliveryDate, "dd/mm/yyyy");
            MdEnt.DELIVERY_DAY = delivDateParts[0];
            MdEnt.DELIVERY_MONTH = delivDateParts[1];
            MdEnt.DELIVERY_YEAR = delivDateParts[2];
            MdEnt.STATUS = "1";
            MdEnt.REMARKS = Convert.ToString(dRow["REMARKS"]);
            MdSer.Insert(MdEnt);

            EntityList tlst = new EntityList();
            MdEnt = new PR_MULTIPLE_DELIVERY();
            tlst = MdSer.GetAll(MdEnt);
            string mdid = "";
            foreach(PR_MULTIPLE_DELIVERY row in tlst)
            {
                mdid = row.PK_ID;
            }


            // 3. Insert PR_ORDER_DETAIL -> MULTIPLE_DELIVERY_ID = PR_MULTIPLE_DELIVERY.PK_ID
            DataRow[] linkedProducts = pTable.Select("DELIVERY_ROW_ID = '" + deliveryRowId.Replace("'", "''") + "'");
            foreach (DataRow pRow in linkedProducts)
            {
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
        }

        HelperFunction.MsgBox(this, this.GetType(), "Purchase Order saved successfully.");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        hfPK_ID.Value = "";
        ViewState.Remove(DeliveryTableKey);
        ViewState.Remove(ProductTableKey);

        txtOrderDate.Text = PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy");
        ddlPaymentTerm.SelectedIndex = 0;
        ddlCustomer.SelectedIndex = 0;

        ClearCustomerDetails();
        InitializeDeliveryTable();
        InitializeProductTable();

        ShowStep(1);
    }

}