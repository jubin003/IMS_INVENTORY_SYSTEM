using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Production_Create_Production : System.Web.UI.Page
{
    PhyeGanDate PGD = new PhyeGanDate();
    PRODUCT_TYPE PTEnt = new PRODUCT_TYPE();
    PRODUCT_TYPEService PTSer = new PRODUCT_TYPEService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtProductionDate.Text = GetTodayNepali();
            txtCompletionDate.Text = GetTodayNepali();

            LoadProductionType();
            LoadCustomersDropdown();

            BindProductionStages();
            BindProductionPipelineUnit();
            BindProductionPipelineEmployee();

            ShowStep(1);
        }
    }

    protected void LoadProductionType()
    {
        PTEnt = new PRODUCT_TYPE();
        PTEnt.STATUS = "1";

        ddlProductionType.DataSource = PTSer.GetAll(PTEnt);
        ddlProductionType.DataTextField = "PRODUCT_TYPE_NAME";
        ddlProductionType.DataValueField = "PK_ID";
        ddlProductionType.DataBind();

        ddlProductionType.Items.Insert(0, new ListItem("-- Select Production Type --", "0"));
    }

    // =====================================================
    // DATE HELPERS
    // =====================================================

    private string GetTodayNepali()
    {
        string engDate = PGD.GetTodayDate("dd/mm/yyyy");
        return PGD.GetNepaliDateFromEnglish(engDate, "dd/mm/yyyy");
    }

    private bool IsValidDate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        string[] parts = value.Trim().Split('/');

        int day, month, year;

        return parts.Length == 3
            && int.TryParse(parts[0], out day)
            && int.TryParse(parts[1], out month)
            && int.TryParse(parts[2], out year)
            && day > 0 && day <= 32
            && month > 0 && month <= 12
            && year > 0;
    }

    private string[] GetDateParts(string value)
    {
        string[] parts = value.Trim().Split('/');
        return new string[] { parts[0], parts[1], parts[2] };
    }

    private string FormatDeliveryDate(PR_MULTIPLE_DELIVERY item)
    {
        if (item == null) return "";

        if (!string.IsNullOrWhiteSpace(item.DELIVERY_DAY) &&
            !string.IsNullOrWhiteSpace(item.DELIVERY_MONTH) &&
            !string.IsNullOrWhiteSpace(item.DELIVERY_YEAR))
        {
            string day = item.DELIVERY_DAY.Trim().PadLeft(2, '0');
            string month = item.DELIVERY_MONTH.Trim().PadLeft(2, '0');
            string year = item.DELIVERY_YEAR.Trim();
            return string.Format("{0}/{1}/{2}", day, month, year);
        }

        if (!string.IsNullOrWhiteSpace(item.DELIVERY_DATE))
        {
            return item.DELIVERY_DATE.Trim().Split(' ')[0];
        }

        return "";
    }

    // =====================================================
    // STEP NAVIGATION
    // =====================================================

    private void ShowStep(int step)
    {
        pnlProductionMaster.Visible = step == 1;
        pnlProductionDetails.Visible = step == 2;
        pnlProductionStages.Visible = step == 3;
        pnlProductionPipeline.Visible = step == 4;

        if (step == 2)
        {
            lblDetailsProductionDate.Text = txtProductionDate.Text;
            lblDetailsCompletionDate.Text = txtCompletionDate.Text;
        }
        else if (step == 3)
        {
            lblStageOrderNumber.Text = ddlCustomerOrder.SelectedIndex > 0 ? ddlCustomerOrder.SelectedItem.Text : "-";
            lblStageProductionDate.Text = txtProductionDate.Text;
            lblStageCompletionDate.Text = txtCompletionDate.Text;
            BindProductionStages();
        }
        else if (step == 4)
        {
            ConfigureStep4Pipelines();
        }
    }

    private void ConfigureStep4Pipelines()
    {
        DataTable dtStages = ProductionStageTable;
        bool hasUnit = false;
        bool hasEmployee = false;

        DataTable dtUnit = CreateEmptyPipelineUnitTable();
        DataTable dtEmp = CreateEmptyPipelineEmployeeTable();

        foreach (DataRow row in dtStages.Rows)
        {
            string assignment = Convert.ToString(row["UNIT_OR_EMPLOYEE"]);
            string detailId = Convert.ToString(row["PRODUCTION_DETAIL_ID"]);
            string stageId = Convert.ToString(row["STAGE_MASTER_ID"]);

            if (string.Equals(assignment, "Unit", StringComparison.OrdinalIgnoreCase))
            {
                hasUnit = true;
                dtUnit.Rows.Add(detailId, stageId, "", "", "", "");
            }
            else if (string.Equals(assignment, "Employee", StringComparison.OrdinalIgnoreCase))
            {
                hasEmployee = true;
                dtEmp.Rows.Add(detailId, stageId, "", "", "");
            }
        }

        pnlPipelineUnit.Visible = hasUnit;
        pnlPipelineEmployee.Visible = hasEmployee;

        if (hasUnit)
        {
            // Seed from Step 3 if Unit table is still blank
            if (ViewState["ProductionPipelineUnitTable"] == null || IsPipelineTableBlank(ProductionPipelineUnitTable))
                ProductionPipelineUnitTable = dtUnit;

            BindProductionPipelineUnit();
        }

        if (hasEmployee)
        {
            // Seed from Step 3 if Employee table is still blank
            if (ViewState["ProductionPipelineEmployeeTable"] == null || IsPipelineTableBlank(ProductionPipelineEmployeeTable))
                ProductionPipelineEmployeeTable = dtEmp;

            BindProductionPipelineEmployee();
        }
    }

    private bool IsPipelineTableBlank(DataTable dt)
    {
        if (dt == null || dt.Rows.Count == 0) return true;
        if (dt.Rows.Count == 1 &&
            string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["PRODUCTION_DETAIL_ID"])) &&
            string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["STAGE_ID"])))
        {
            return true;
        }
        return false;
    }

    protected void btnNextMaster_Click(object sender, EventArgs e)
    {
        //if (!ValidateMaster())
        //    return;

        LoadMultipleDeliveriesByPO(ddlCustomerOrder.SelectedValue);
        ShowStep(2);
    }

    protected void btnBackMaster_Click(object sender, EventArgs e)
    {
        if (pnlProductionProducts.Visible)
            ReadProductionDetails();

        ShowStep(1);
    }

    protected void btnNextDetails_Click(object sender, EventArgs e)
    {
        if (pnlProductionProducts.Visible)
            ReadProductionDetails();

        //if (!ValidateProductionDetails())
        //    return;

        ShowStep(3);
    }

    protected void btnBackDetails_Click(object sender, EventArgs e)
    {
        ReadProductionStages();
        ShowStep(2);
    }

    protected void btnNextStages_Click(object sender, EventArgs e)
    {
        ReadProductionStages();

        //if (!ValidateProductionStages())
        //    return;

        ShowStep(4);
    }

    protected void btnBackStages_Click(object sender, EventArgs e)
    {
        if (pnlPipelineUnit.Visible)
            ReadProductionPipelineUnit();

        if (pnlPipelineEmployee.Visible)
            ReadProductionPipelineEmployee();

        ShowStep(3);
    }

    // =====================================================
    // STEP 1: PRODUCTION MASTER & CUSTOMER SELECTION
    // =====================================================

    private void LoadCustomersDropdown()
    {
        try
        {
            CUSTOMERService custService = new CUSTOMERService();
            CUSTOMER filter = new CUSTOMER();
            EntityList custList = (EntityList)custService.GetAll(filter);

            ddlCustomer.DataSource = custList;
            ddlCustomer.DataTextField = "CUSTOMER_NAME";
            ddlCustomer.DataValueField = "PK_ID";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("-- Select Customer --", ""));
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, GetType(), "Error loading customers: " + ex.Message);
        }
    }

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedCustomerId = ddlCustomer.SelectedValue;

        ddlCustomerOrder.Items.Clear();

        if (string.IsNullOrEmpty(selectedCustomerId))
        {
            ddlCustomerOrder.Items.Insert(0, new ListItem("-- Select Customer First --", ""));
            return;
        }

        try
        {
            PR_PURCHASE_ORDERService poService = new PR_PURCHASE_ORDERService();
            PR_PURCHASE_ORDER filter = new PR_PURCHASE_ORDER();
            filter.CUSTOMER_ID = selectedCustomerId;

            EntityList orderList = (EntityList)poService.GetAll(filter);

            if (orderList != null && orderList.Count > 0)
            {
                ddlCustomerOrder.DataSource = orderList;
                ddlCustomerOrder.DataTextField = "ORDER_NUMBER";
                ddlCustomerOrder.DataValueField = "PK_ID";
                ddlCustomerOrder.DataBind();
                ddlCustomerOrder.Items.Insert(0, new ListItem("-- Select Customer Order --", ""));
            }
            else
            {
                ddlCustomerOrder.Items.Insert(0, new ListItem("No Orders Found", ""));
            }
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, GetType(), "Error loading customer orders: " + ex.Message);
        }
    }

    private bool ValidateMaster()
    {
        if (string.IsNullOrWhiteSpace(ddlCustomer.SelectedValue))
        {
            HelperFunction.MsgBox(this, GetType(), "Please select a customer.");
            return false;
        }

        if (!IsValidDate(txtProductionDate.Text))
        {
            HelperFunction.MsgBox(this, GetType(), "Enter a valid production date in dd/mm/yyyy format.");
            return false;
        }

        if (!IsValidDate(txtCompletionDate.Text))
        {
            HelperFunction.MsgBox(this, GetType(), "Enter a valid completion date in dd/mm/yyyy format.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(ddlProductionType.SelectedValue) || ddlProductionType.SelectedValue == "0")
        {
            HelperFunction.MsgBox(this, GetType(), "Please select a production type.");
            return false;
        }

        return true;
    }

    protected void btnClearMaster_Click(object sender, EventArgs e)
    {
        txtProductionDate.Text = GetTodayNepali();
        txtCompletionDate.Text = GetTodayNepali();

        if (ddlProductionType.Items.Count > 0)
            ddlProductionType.SelectedIndex = 0;

        hfProductionId.Value = "";

        if (ddlCustomer.Items.Count > 0)
            ddlCustomer.SelectedIndex = 0;

        ddlCustomerOrder.Items.Clear();
        ddlCustomerOrder.Items.Insert(0, new ListItem("-- Select Customer First --", ""));

        ShowStep(1);
    }

    // =====================================================
    // STEP 2: PRODUCTION DETAILS & MULTIPLE DELIVERY
    // =====================================================

    private void LoadMultipleDeliveriesByPO(string purchaseOrderId)
    {
        string previousSelection = ddlMultipleDelivery.SelectedValue;

        ddlMultipleDelivery.Items.Clear();
        pnlSelectedDeliveryInfo.Visible = false;
        pnlProductionProducts.Visible = false;

        if (string.IsNullOrEmpty(purchaseOrderId))
        {
            ddlMultipleDelivery.Items.Insert(0, new ListItem("-- Select PO in Step 1 First --", ""));
            return;
        }

        try
        {
            PR_MULTIPLE_DELIVERYService mdService = new PR_MULTIPLE_DELIVERYService();
            PR_MULTIPLE_DELIVERY filter = new PR_MULTIPLE_DELIVERY();
            filter.PURCHASE_ORDER_ID = purchaseOrderId;

            EntityList deliveryList = (EntityList)mdService.GetAll(filter);

            if (deliveryList != null && deliveryList.Count > 0)
            {
                for (int i = 0; i < deliveryList.Count; i++)
                {
                    PR_MULTIPLE_DELIVERY item = (PR_MULTIPLE_DELIVERY)deliveryList[i];
                    string formattedDate = FormatDeliveryDate(item);
                    string remarks = string.IsNullOrWhiteSpace(item.REMARKS) ? "" : " (" + item.REMARKS.Trim() + ")";

                    string label = "Delivery #" + (i + 1) + " - " + formattedDate + remarks;
                    ddlMultipleDelivery.Items.Add(new ListItem(label, item.PK_ID));
                }
                ddlMultipleDelivery.Items.Insert(0, new ListItem("-- Select Delivery Date --", ""));

                if (!string.IsNullOrEmpty(previousSelection) && ddlMultipleDelivery.Items.FindByValue(previousSelection) != null)
                {
                    ddlMultipleDelivery.SelectedValue = previousSelection;
                    BindSelectedDeliveryDetails(previousSelection);
                    LoadOrderDetailsByDelivery(previousSelection);
                }
            }
            else
            {
                ddlMultipleDelivery.Items.Insert(0, new ListItem("No Delivery Dates Found", ""));
            }
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, GetType(), "Error loading delivery dates: " + ex.Message);
        }
    }

    protected void ddlMultipleDelivery_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedDeliveryId = ddlMultipleDelivery.SelectedValue;

        BindSelectedDeliveryDetails(selectedDeliveryId);
        LoadOrderDetailsByDelivery(selectedDeliveryId);
    }

    private void BindSelectedDeliveryDetails(string selectedDeliveryId)
    {
        if (string.IsNullOrEmpty(selectedDeliveryId))
        {
            pnlSelectedDeliveryInfo.Visible = false;
            grdSelectedDelivery.DataSource = null;
            grdSelectedDelivery.DataBind();
            return;
        }

        try
        {
            PR_MULTIPLE_DELIVERYService mdService = new PR_MULTIPLE_DELIVERYService();
            PR_MULTIPLE_DELIVERY filter = new PR_MULTIPLE_DELIVERY();
            filter.PK_ID = selectedDeliveryId;

            EntityList list = (EntityList)mdService.GetAll(filter);
            if (list != null && list.Count > 0)
            {
                PR_TRANSPORTATIONService transService = new PR_TRANSPORTATIONService();
                PR_CUSTOMER_LOCATIONService locService = new PR_CUSTOMER_LOCATIONService();

                foreach (PR_MULTIPLE_DELIVERY delivery in list)
                {
                    delivery.DELIVERY_DATE = FormatDeliveryDate(delivery);

                    if (!string.IsNullOrEmpty(delivery.MODE_OF_TRANSPORTATION_ID))
                    {
                        PR_TRANSPORTATION transFilter = new PR_TRANSPORTATION();
                        transFilter.PK_ID = delivery.MODE_OF_TRANSPORTATION_ID;

                        PR_TRANSPORTATION transObj = (PR_TRANSPORTATION)transService.GetSingle(transFilter);
                        if (transObj != null && !string.IsNullOrEmpty(transObj.TRANSPORTATION_NAME))
                        {
                            delivery.MODE_OF_TRANSPORTATION_ID = transObj.TRANSPORTATION_NAME;
                        }
                    }

                    if (!string.IsNullOrEmpty(delivery.CUSOMTER_LOCATION_ID))
                    {
                        PR_CUSTOMER_LOCATION locFilter = new PR_CUSTOMER_LOCATION();
                        locFilter.PK_ID = delivery.CUSOMTER_LOCATION_ID;

                        PR_CUSTOMER_LOCATION locObj = (PR_CUSTOMER_LOCATION)locService.GetSingle(locFilter);
                        if (locObj != null && !string.IsNullOrEmpty(locObj.ADDRESS))
                        {
                            delivery.CUSOMTER_LOCATION_ID = locObj.ADDRESS;
                        }
                    }
                    else
                    {
                        delivery.CUSOMTER_LOCATION_ID = "-";
                    }

                    if (delivery.STATUS == "1")
                        delivery.STATUS = "Active";
                    else if (delivery.STATUS == "0")
                        delivery.STATUS = "Inactive";
                }

                grdSelectedDelivery.DataSource = list;
                grdSelectedDelivery.DataBind();
                pnlSelectedDeliveryInfo.Visible = true;
            }
            else
            {
                pnlSelectedDeliveryInfo.Visible = false;
            }
        }
        catch (Exception ex)
        {
            HelperFunction.MsgBox(this, GetType(), "Error loading delivery info: " + ex.Message);
        }
    }

    private string GetProductNameById(string productId, PRODUCTService prodService)
    {
        if (string.IsNullOrWhiteSpace(productId))
            return "";

        PRODUCT filter = new PRODUCT();
        filter.PK_ID = productId.Trim();

        PRODUCT obj = (PRODUCT)prodService.GetSingle(filter);
        if (obj != null && !string.IsNullOrEmpty(obj.PRODUCT_NAME))
            return obj.PRODUCT_NAME;

        return productId;
    }

    private string GetSizeNameById(string sizeId, PRODUCT_SIZEService sizeService)
    {
        if (string.IsNullOrWhiteSpace(sizeId))
            return "";

        PRODUCT_SIZE filter = new PRODUCT_SIZE();
        filter.PK_ID = sizeId.Trim();

        PRODUCT_SIZE obj = (PRODUCT_SIZE)sizeService.GetSingle(filter);
        if (obj != null && !string.IsNullOrEmpty(obj.SIZE_NAME))
            return obj.SIZE_NAME;

        return sizeId;
    }

    private string GetSwatchNameById(string swatchId, PR_SWATCHService swatchService)
    {
        if (string.IsNullOrWhiteSpace(swatchId))
            return "";

        PR_SWATCH filter = new PR_SWATCH();
        filter.PK_ID = swatchId.Trim();

        PR_SWATCH obj = (PR_SWATCH)swatchService.GetSingle(filter);
        if (obj != null && !string.IsNullOrEmpty(obj.SWATCH_NAME))
            return obj.SWATCH_NAME;

        return swatchId;
    }

    private void LoadOrderDetailsByDelivery(string selectedDeliveryId)
    {
        if (string.IsNullOrEmpty(selectedDeliveryId))
        {
            pnlProductionProducts.Visible = false;
            ViewState.Remove("ProductionDetailTable");
            grdProductionDetails.DataSource = null;
            grdProductionDetails.DataBind();
            return;
        }

        try
        {
            DataTable dt = CreateEmptyProductionDetailTable();

            PR_ORDER_DETAILService orderDetailService = new PR_ORDER_DETAILService();
            PR_ORDER_DETAIL filter = new PR_ORDER_DETAIL();
            filter.MULTIPLE_DELVIERY_ID = selectedDeliveryId;

            EntityList detailList = (EntityList)orderDetailService.GetAll(filter);

            if (detailList != null && detailList.Count > 0)
            {
                PRODUCTService prodService = new PRODUCTService();
                PRODUCT_SIZEService sizeService = new PRODUCT_SIZEService();
                PR_SWATCHService swatchService = new PR_SWATCHService();

                foreach (PR_ORDER_DETAIL item in detailList)
                {
                    string productName = GetProductNameById(item.PRODUCT_ID, prodService);
                    string sizeName = GetSizeNameById(item.SIZE_ID, sizeService);
                    string swatchName = GetSwatchNameById(item.SWATCH_ID, swatchService);

                    dt.Rows.Add(
                        item.PRODUCT_ID,
                        productName,
                        item.SIZE_ID,
                        sizeName,
                        item.SWATCH_ID,
                        swatchName,
                        item.QUANTITY,
                        item.UNIT
                    );
                }
            }
            else
            {
                dt.Rows.Add("", "", "", "", "", "", "", "");
            }

            ProductionDetailTable = dt;
            BindProductionDetails();
            pnlProductionProducts.Visible = true;
        }
        catch (Exception ex)
        {
            string safeMessage = ex.Message
                .Replace("'", "")
                .Replace("\"", "")
                .Replace("\r", " ")
                .Replace("\n", " ");
            HelperFunction.MsgBox(this, GetType(), "Error loading production products: " + safeMessage);
        }
    }

    private DataTable CreateEmptyProductionDetailTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PRODUCT_ID");
        dt.Columns.Add("PRODUCT_NAME");
        dt.Columns.Add("SIZE_ID");
        dt.Columns.Add("SIZE_NAME");
        dt.Columns.Add("SWATCH_ID");
        dt.Columns.Add("SWATCH_NAME");
        dt.Columns.Add("QUANTITY");
        dt.Columns.Add("UNIT");
        return dt;
    }

    private DataTable ProductionDetailTable
    {
        get
        {
            if (ViewState["ProductionDetailTable"] == null)
            {
                DataTable dt = CreateEmptyProductionDetailTable();
                dt.Rows.Add("", "", "", "", "", "", "", "");
                ViewState["ProductionDetailTable"] = dt;
            }

            return (DataTable)ViewState["ProductionDetailTable"];
        }
        set
        {
            ViewState["ProductionDetailTable"] = value;
        }
    }

    private void BindProductionDetails()
    {
        grdProductionDetails.DataSource = ProductionDetailTable;
        grdProductionDetails.DataBind();
    }

    private void ReadProductionDetails()
    {
        DataTable dt = ProductionDetailTable;

        for (int i = 0; i < grdProductionDetails.Rows.Count; i++)
        {
            GridViewRow row = grdProductionDetails.Rows[i];

            if (i >= dt.Rows.Count)
                dt.Rows.Add("", "", "", "", "", "", "", "");

            dt.Rows[i]["PRODUCT_ID"] = GetHiddenValue(row, "hfProductId");
            dt.Rows[i]["PRODUCT_NAME"] = GetText(row, "txtProductName");
            dt.Rows[i]["SIZE_ID"] = GetHiddenValue(row, "hfSizeId");
            dt.Rows[i]["SIZE_NAME"] = GetText(row, "txtSizeName");
            dt.Rows[i]["SWATCH_ID"] = GetHiddenValue(row, "hfSwatchId");
            dt.Rows[i]["SWATCH_NAME"] = GetText(row, "txtSwatchName");
            dt.Rows[i]["QUANTITY"] = GetText(row, "txtQuantity");
            dt.Rows[i]["UNIT"] = GetText(row, "txtUnit");
        }

        while (dt.Rows.Count > grdProductionDetails.Rows.Count)
            dt.Rows.RemoveAt(dt.Rows.Count - 1);

        ProductionDetailTable = dt;
    }

    //protected void btnAddProduct_Click(object sender, EventArgs e)
    //{
    //    ReadProductionDetails();

    //    DataTable dt = ProductionDetailTable;
    //    dt.Rows.Add("", "", "", "", "", "", "", "");

    //    ProductionDetailTable = dt;
    //    BindProductionDetails();
    //}

    protected void grdProductionDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "RemoveProduct")
            return;

        ReadProductionDetails();

        int index = Convert.ToInt32(e.CommandArgument);
        DataTable dt = ProductionDetailTable;

        if (index >= 0 && index < dt.Rows.Count)
            dt.Rows.RemoveAt(index);

        if (dt.Rows.Count == 0)
            dt.Rows.Add("", "", "", "", "", "", "", "");

        ProductionDetailTable = dt;
        BindProductionDetails();
    }

    private bool ValidateProductionDetails()
    {
        if (string.IsNullOrWhiteSpace(ddlMultipleDelivery.SelectedValue))
        {
            HelperFunction.MsgBox(this, GetType(), "Please select a Multiple Delivery Date.");
            return false;
        }

        DataTable dt = ProductionDetailTable;
        bool hasProduct = false;

        foreach (DataRow row in dt.Rows)
        {
            string productId = Convert.ToString(row["PRODUCT_ID"]);
            string quantity = Convert.ToString(row["QUANTITY"]);

            if (string.IsNullOrWhiteSpace(productId))
                continue;

            hasProduct = true;

            decimal qty;
            if (!decimal.TryParse(quantity, out qty) || qty <= 0)
            {
                HelperFunction.MsgBox(this, GetType(), "Enter a valid positive quantity for every product.");
                return false;
            }
        }

        if (!hasProduct)
        {
            HelperFunction.MsgBox(this, GetType(), "Add at least one production product.");
            return false;
        }

        return true;
    }

    // =====================================================
    // SHARED DROPDOWN BINDERS (STEP 3 & STEP 4)
    // =====================================================

    private void PopulateProductDetailDropdown(DropDownList ddl, string selectedValue)
    {
        if (ddl == null) return;

        ddl.Items.Clear();
        ddl.Items.Add(new ListItem("-- Select Product --", ""));

        DataTable dtDetails = ProductionDetailTable;
        for (int i = 0; i < dtDetails.Rows.Count; i++)
        {
            string prodId = Convert.ToString(dtDetails.Rows[i]["PRODUCT_ID"]);
            string prodName = Convert.ToString(dtDetails.Rows[i]["PRODUCT_NAME"]);
            string sizeName = Convert.ToString(dtDetails.Rows[i]["SIZE_NAME"]);
            string qty = Convert.ToString(dtDetails.Rows[i]["QUANTITY"]);

            if (!string.IsNullOrWhiteSpace(prodId))
            {
                string displayText = string.Format("{0} (Size: {1}, Qty: {2})", prodName, sizeName, qty);
                if (ddl.Items.FindByValue(prodId) == null)
                    ddl.Items.Add(new ListItem(displayText, prodId));
            }
        }

        if (!string.IsNullOrEmpty(selectedValue) && ddl.Items.FindByValue(selectedValue) != null)
        {
            ddl.SelectedValue = selectedValue;
        }
    }

    private void PopulateStageMasterDropdown(DropDownList ddl, string selectedValue)
    {
        if (ddl == null) return;

        PR_STAGE_MASTERService stageService = new PR_STAGE_MASTERService();
        PR_STAGE_MASTER filter = new PR_STAGE_MASTER();
        filter.STATUS = "1";

        ddl.DataSource = stageService.GetAll(filter);
        ddl.DataTextField = "NAME";
        ddl.DataValueField = "PK_ID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-- Select Stage --", ""));

        if (!string.IsNullOrEmpty(selectedValue) && ddl.Items.FindByValue(selectedValue) != null)
        {
            ddl.SelectedValue = selectedValue;
        }
    }

    private void PopulateUnitDropdown(DropDownList ddl, string selectedValue)
    {
        if (ddl == null) return;

        PR_UNITService unitService = new PR_UNITService();
        PR_UNIT filter = new PR_UNIT();
        filter.STATUS = "1";

        ddl.DataSource = unitService.GetAll(filter);
        ddl.DataTextField = "UNIT_NAME";
        ddl.DataValueField = "PK_ID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-- Select Unit --", ""));

        if (!string.IsNullOrEmpty(selectedValue) && ddl.Items.FindByValue(selectedValue) != null)
        {
            ddl.SelectedValue = selectedValue;
        }
    }

    private void PopulateDivisionDropdown(DropDownList ddl, string unitId, string selectedValue)
    {
        if (ddl == null) return;

        ddl.Items.Clear();

        if (string.IsNullOrWhiteSpace(unitId))
        {
            ddl.Items.Insert(0, new ListItem("-- Select Unit First --", ""));
            return;
        }

        PR_DIVISIONService divService = new PR_DIVISIONService();
        PR_DIVISION filter = new PR_DIVISION();
        filter.UNIT_ID = unitId;
        filter.STATUS = "1";

        EntityList divList = (EntityList)divService.GetAll(filter);
        if (divList != null && divList.Count > 0)
        {
            ddl.DataSource = divList;
            ddl.DataTextField = "DIVISION_NAME";
            ddl.DataValueField = "PK_ID";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("-- Select Division --", ""));
        }
        else
        {
            ddl.Items.Insert(0, new ListItem("No Divisions Found", ""));
        }

        if (!string.IsNullOrEmpty(selectedValue) && ddl.Items.FindByValue(selectedValue) != null)
        {
            ddl.SelectedValue = selectedValue;
        }
    }

    private void PopulateEmployeeDropdown(DropDownList ddl, string selectedValue)
    {
        if (ddl == null) return;

        PR_DIVISION_EMPLOYEEService empService = new PR_DIVISION_EMPLOYEEService();
        PR_DIVISION_EMPLOYEE filter = new PR_DIVISION_EMPLOYEE();
        filter.STATUS = "1";

        EntityList empList = (EntityList)empService.GetAll(filter);

        ddl.Items.Clear();
        ddl.Items.Add(new ListItem("-- Select Employee --", ""));

        if (empList != null)
        {
            foreach (PR_DIVISION_EMPLOYEE item in empList)
            {
                if (!string.IsNullOrWhiteSpace(item.EMPLOYEE_ID) && ddl.Items.FindByValue(item.EMPLOYEE_ID) == null)
                {
                    ddl.Items.Add(new ListItem("Employee #" + item.EMPLOYEE_ID, item.EMPLOYEE_ID));
                }
            }
        }

        if (!string.IsNullOrEmpty(selectedValue) && ddl.Items.FindByValue(selectedValue) != null)
        {
            ddl.SelectedValue = selectedValue;
        }
    }

    // =====================================================
    // STEP 3: PRODUCTION STAGES
    // =====================================================

    private DataTable ProductionStageTable
    {
        get
        {
            if (ViewState["ProductionStageTable"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PRODUCTION_DETAIL_ID");
                dt.Columns.Add("PRODUCT_ID");
                dt.Columns.Add("STAGE_MASTER_ID");
                dt.Columns.Add("UNIT_OR_EMPLOYEE");
                dt.Columns.Add("STATUS");
                dt.Rows.Add("", "", "", "", "Pending");
                ViewState["ProductionStageTable"] = dt;
            }

            return (DataTable)ViewState["ProductionStageTable"];
        }
        set
        {
            ViewState["ProductionStageTable"] = value;
        }
    }

    private void BindProductionStages()
    {
        grdProductionStages.DataSource = ProductionStageTable;
        grdProductionStages.DataBind();
    }

    protected void grdProductionStages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        DropDownList ddlStageProduct = (DropDownList)e.Row.FindControl("ddlStageProduct");
        HiddenField hfStageProductId = (HiddenField)e.Row.FindControl("hfStageProductId");
        PopulateProductDetailDropdown(ddlStageProduct, hfStageProductId != null ? hfStageProductId.Value : "");

        DropDownList ddlStageMaster = (DropDownList)e.Row.FindControl("ddlStageMaster");
        HiddenField hfStageMasterId = (HiddenField)e.Row.FindControl("hfStageMasterId");
        PopulateStageMasterDropdown(ddlStageMaster, hfStageMasterId != null ? hfStageMasterId.Value : "");
    }

    private void ReadProductionStages()
    {
        DataTable dt = ProductionStageTable;

        for (int i = 0; i < grdProductionStages.Rows.Count; i++)
        {
            GridViewRow row = grdProductionStages.Rows[i];

            if (i >= dt.Rows.Count)
                dt.Rows.Add("", "", "", "", "Pending");

            string selectedProductId = GetSelectedValue(row, "ddlStageProduct");

            dt.Rows[i]["PRODUCT_ID"] = selectedProductId;
            dt.Rows[i]["PRODUCTION_DETAIL_ID"] = selectedProductId;
            dt.Rows[i]["STAGE_MASTER_ID"] = GetSelectedValue(row, "ddlStageMaster");
            dt.Rows[i]["UNIT_OR_EMPLOYEE"] = GetSelectedValue(row, "ddlStageAssignmentType");
            dt.Rows[i]["STATUS"] = GetSelectedValue(row, "ddlStageStatus");
        }

        while (dt.Rows.Count > grdProductionStages.Rows.Count)
            dt.Rows.RemoveAt(dt.Rows.Count - 1);

        ProductionStageTable = dt;
    }

    private bool ValidateProductionStages()
    {
        DataTable stageTable = ProductionStageTable;
        bool hasStage = false;

        foreach (DataRow row in stageTable.Rows)
        {
            string stageMasterId = Convert.ToString(row["STAGE_MASTER_ID"]);
            if (string.IsNullOrWhiteSpace(stageMasterId))
                continue;

            hasStage = true;

            if (string.IsNullOrWhiteSpace(Convert.ToString(row["PRODUCTION_DETAIL_ID"]))
                || string.IsNullOrWhiteSpace(Convert.ToString(row["UNIT_OR_EMPLOYEE"])))
            {
                HelperFunction.MsgBox(this, GetType(), "Every configured stage requires a product and an assignment type (Unit or Employee).");
                return false;
            }
        }

        if (!hasStage)
        {
            HelperFunction.MsgBox(this, GetType(), "Configure at least one production stage.");
            return false;
        }

        return true;
    }

    protected void btnAddStage_Click(object sender, EventArgs e)
    {
        ReadProductionStages();

        DataTable dt = ProductionStageTable;
        dt.Rows.Add("", "", "", "", "Pending");

        ProductionStageTable = dt;
        BindProductionStages();
    }

    protected void grdProductionStages_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "RemoveStage")
            return;

        ReadProductionStages();

        int index = Convert.ToInt32(e.CommandArgument);
        DataTable dt = ProductionStageTable;

        if (index >= 0 && index < dt.Rows.Count)
            dt.Rows.RemoveAt(index);

        if (dt.Rows.Count == 0)
            dt.Rows.Add("", "", "", "", "Pending");

        ProductionStageTable = dt;
        BindProductionStages();
    }

    // =====================================================
    // STEP 4A: PRODUCTION PIPELINE (UNIT)
    // =====================================================

    private DataTable CreateEmptyPipelineUnitTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PRODUCTION_DETAIL_ID");
        dt.Columns.Add("STAGE_ID");
        dt.Columns.Add("PRODUCTION_UNIT");
        dt.Columns.Add("PRODUCTION_DIVISION");
        dt.Columns.Add("ESTIMATED_DAY");
        dt.Columns.Add("ESTIMATED_TIME");
        return dt;
    }

    private DataTable ProductionPipelineUnitTable
    {
        get
        {
            if (ViewState["ProductionPipelineUnitTable"] == null)
            {
                DataTable dt = CreateEmptyPipelineUnitTable();
                dt.Rows.Add("", "", "", "", "", "");
                ViewState["ProductionPipelineUnitTable"] = dt;
            }

            return (DataTable)ViewState["ProductionPipelineUnitTable"];
        }
        set
        {
            ViewState["ProductionPipelineUnitTable"] = value;
        }
    }

    private void BindProductionPipelineUnit()
    {
        grdProductionPipelineUnit.DataSource = ProductionPipelineUnitTable;
        grdProductionPipelineUnit.DataBind();
    }

    protected void grdProductionPipelineUnit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        DropDownList ddlPipelineDetail = (DropDownList)e.Row.FindControl("ddlPipelineDetail");
        HiddenField hfPipelineDetailId = (HiddenField)e.Row.FindControl("hfPipelineDetailId");
        PopulateProductDetailDropdown(ddlPipelineDetail, hfPipelineDetailId != null ? hfPipelineDetailId.Value : "");

        DropDownList ddlPipelineStage = (DropDownList)e.Row.FindControl("ddlPipelineStage");
        HiddenField hfPipelineStageId = (HiddenField)e.Row.FindControl("hfPipelineStageId");
        PopulateStageMasterDropdown(ddlPipelineStage, hfPipelineStageId != null ? hfPipelineStageId.Value : "");

        DropDownList ddlPipelineUnit = (DropDownList)e.Row.FindControl("ddlPipelineUnit");
        HiddenField hfPipelineUnitId = (HiddenField)e.Row.FindControl("hfPipelineUnitId");
        string selectedUnitId = hfPipelineUnitId != null ? hfPipelineUnitId.Value : "";
        PopulateUnitDropdown(ddlPipelineUnit, selectedUnitId);

        DropDownList ddlPipelineDivision = (DropDownList)e.Row.FindControl("ddlPipelineDivision");
        HiddenField hfPipelineDivisionId = (HiddenField)e.Row.FindControl("hfPipelineDivisionId");
        string selectedDivisionId = hfPipelineDivisionId != null ? hfPipelineDivisionId.Value : "";
        PopulateDivisionDropdown(ddlPipelineDivision, selectedUnitId, selectedDivisionId);
    }

    protected void ddlPipelineUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlUnit = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlUnit.NamingContainer;
        DropDownList ddlDivision = (DropDownList)row.FindControl("ddlPipelineDivision");

        PopulateDivisionDropdown(ddlDivision, ddlUnit.SelectedValue, "");
        ReadProductionPipelineUnit();
    }

    private void ReadProductionPipelineUnit()
    {
        DataTable dt = ProductionPipelineUnitTable;

        for (int i = 0; i < grdProductionPipelineUnit.Rows.Count; i++)
        {
            GridViewRow row = grdProductionPipelineUnit.Rows[i];

            if (i >= dt.Rows.Count)
                dt.Rows.Add("", "", "", "", "", "");

            dt.Rows[i]["PRODUCTION_DETAIL_ID"] = GetSelectedValue(row, "ddlPipelineDetail");
            dt.Rows[i]["STAGE_ID"] = GetSelectedValue(row, "ddlPipelineStage");
            dt.Rows[i]["PRODUCTION_UNIT"] = GetSelectedValue(row, "ddlPipelineUnit");
            dt.Rows[i]["PRODUCTION_DIVISION"] = GetSelectedValue(row, "ddlPipelineDivision");
            dt.Rows[i]["ESTIMATED_DAY"] = GetText(row, "txtEstimatedDay");
            dt.Rows[i]["ESTIMATED_TIME"] = GetText(row, "txtEstimatedTime");
        }

        while (dt.Rows.Count > grdProductionPipelineUnit.Rows.Count)
            dt.Rows.RemoveAt(dt.Rows.Count - 1);

        ProductionPipelineUnitTable = dt;
    }

    protected void btnAddPipelineUnit_Click(object sender, EventArgs e)
    {
        ReadProductionPipelineUnit();

        DataTable dt = ProductionPipelineUnitTable;
        dt.Rows.Add("", "", "", "", "", "");

        ProductionPipelineUnitTable = dt;
        BindProductionPipelineUnit();
    }

    protected void grdProductionPipelineUnit_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "RemovePipelineUnit")
            return;

        ReadProductionPipelineUnit();

        int index = Convert.ToInt32(e.CommandArgument);
        DataTable dt = ProductionPipelineUnitTable;

        if (index >= 0 && index < dt.Rows.Count)
            dt.Rows.RemoveAt(index);

        if (dt.Rows.Count == 0)
            dt.Rows.Add("", "", "", "", "", "");

        ProductionPipelineUnitTable = dt;
        BindProductionPipelineUnit();
    }

    // =====================================================
    // STEP 4B: PRODUCTION PIPELINE (EMPLOYEE)
    // =====================================================

    private DataTable CreateEmptyPipelineEmployeeTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PRODUCTION_DETAIL_ID");
        dt.Columns.Add("STAGE_ID");
        dt.Columns.Add("EMPLOYEE_ID");
        dt.Columns.Add("ESTIMATED_DAY");
        dt.Columns.Add("ESTIMATED_TIME");
        return dt;
    }

    private DataTable ProductionPipelineEmployeeTable
    {
        get
        {
            if (ViewState["ProductionPipelineEmployeeTable"] == null)
            {
                DataTable dt = CreateEmptyPipelineEmployeeTable();
                dt.Rows.Add("", "", "", "", "");
                ViewState["ProductionPipelineEmployeeTable"] = dt;
            }

            return (DataTable)ViewState["ProductionPipelineEmployeeTable"];
        }
        set
        {
            ViewState["ProductionPipelineEmployeeTable"] = value;
        }
    }

    private void BindProductionPipelineEmployee()
    {
        grdProductionPipelineEmployee.DataSource = ProductionPipelineEmployeeTable;
        grdProductionPipelineEmployee.DataBind();
    }

    protected void grdProductionPipelineEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        DropDownList ddlPipelineDetail = (DropDownList)e.Row.FindControl("ddlPipelineDetail");
        HiddenField hfPipelineDetailId = (HiddenField)e.Row.FindControl("hfPipelineDetailId");
        PopulateProductDetailDropdown(ddlPipelineDetail, hfPipelineDetailId != null ? hfPipelineDetailId.Value : "");

        DropDownList ddlPipelineStage = (DropDownList)e.Row.FindControl("ddlPipelineStage");
        HiddenField hfPipelineStageId = (HiddenField)e.Row.FindControl("hfPipelineStageId");
        PopulateStageMasterDropdown(ddlPipelineStage, hfPipelineStageId != null ? hfPipelineStageId.Value : "");

        DropDownList ddlPipelineEmployee = (DropDownList)e.Row.FindControl("ddlPipelineEmployee");
        HiddenField hfPipelineEmployeeId = (HiddenField)e.Row.FindControl("hfPipelineEmployeeId");
        PopulateEmployeeDropdown(ddlPipelineEmployee, hfPipelineEmployeeId != null ? hfPipelineEmployeeId.Value : "");
    }

    private void ReadProductionPipelineEmployee()
    {
        DataTable dt = ProductionPipelineEmployeeTable;

        for (int i = 0; i < grdProductionPipelineEmployee.Rows.Count; i++)
        {
            GridViewRow row = grdProductionPipelineEmployee.Rows[i];

            if (i >= dt.Rows.Count)
                dt.Rows.Add("", "", "", "", "");

            dt.Rows[i]["PRODUCTION_DETAIL_ID"] = GetSelectedValue(row, "ddlPipelineDetail");
            dt.Rows[i]["STAGE_ID"] = GetSelectedValue(row, "ddlPipelineStage");
            dt.Rows[i]["EMPLOYEE_ID"] = GetSelectedValue(row, "ddlPipelineEmployee");
            dt.Rows[i]["ESTIMATED_DAY"] = GetText(row, "txtEstimatedDay");
            dt.Rows[i]["ESTIMATED_TIME"] = GetText(row, "txtEstimatedTime");
        }

        while (dt.Rows.Count > grdProductionPipelineEmployee.Rows.Count)
            dt.Rows.RemoveAt(dt.Rows.Count - 1);

        ProductionPipelineEmployeeTable = dt;
    }

    protected void btnAddPipelineEmployee_Click(object sender, EventArgs e)
    {
        ReadProductionPipelineEmployee();

        DataTable dt = ProductionPipelineEmployeeTable;
        dt.Rows.Add("", "", "", "", "");

        ProductionPipelineEmployeeTable = dt;
        BindProductionPipelineEmployee();
    }

    protected void grdProductionPipelineEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "RemovePipelineEmployee")
            return;

        ReadProductionPipelineEmployee();

        int index = Convert.ToInt32(e.CommandArgument);
        DataTable dt = ProductionPipelineEmployeeTable;

        if (index >= 0 && index < dt.Rows.Count)
            dt.Rows.RemoveAt(index);

        if (dt.Rows.Count == 0)
            dt.Rows.Add("", "", "", "", "");

        ProductionPipelineEmployeeTable = dt;
        BindProductionPipelineEmployee();
    }

    // =====================================================
    // GRID HELPERS
    // =====================================================

    private string GetText(GridViewRow row, string controlId)
    {
        TextBox txt = row.FindControl(controlId) as TextBox;
        return txt == null ? "" : txt.Text.Trim();
    }

    private string GetHiddenValue(GridViewRow row, string controlId)
    {
        HiddenField hf = row.FindControl(controlId) as HiddenField;
        return hf == null ? "" : hf.Value.Trim();
    }

    private string GetSelectedValue(GridViewRow row, string controlId)
    {
        DropDownList ddl = row.FindControl(controlId) as DropDownList;
        return ddl == null ? "" : ddl.SelectedValue;
    }

    // =====================================================
    // FINAL VALIDATION & SAVE
    // =====================================================

    protected void btnSaveProduction_Click(object sender, EventArgs e)
    {
        if (pnlProductionProducts.Visible)
            ReadProductionDetails();

        ReadProductionStages();

        if (pnlPipelineUnit.Visible)
            ReadProductionPipelineUnit();

        if (pnlPipelineEmployee.Visible)
            ReadProductionPipelineEmployee();

        HelperFunction.MsgBox(this, GetType(), "Ready to persist Production, Unit Pipeline, and Employee Pipeline!");
    }

    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        txtProductionDate.Text = GetTodayNepali();
        txtCompletionDate.Text = GetTodayNepali();

        if (ddlProductionType.Items.Count > 0)
            ddlProductionType.SelectedIndex = 0;

        ddlMultipleDelivery.Items.Clear();
        ddlMultipleDelivery.Items.Insert(0, new ListItem("-- Select Delivery Date --", ""));
        pnlSelectedDeliveryInfo.Visible = false;
        pnlProductionProducts.Visible = false;

        grdSelectedDelivery.DataSource = null;
        grdSelectedDelivery.DataBind();

        grdProductionDetails.DataSource = null;
        grdProductionDetails.DataBind();

        hfProductionId.Value = "";

        if (ddlCustomer.Items.Count > 0)
            ddlCustomer.SelectedIndex = 0;

        ddlCustomerOrder.Items.Clear();
        ddlCustomerOrder.Items.Insert(0, new ListItem("-- Select Customer First --", ""));

        ViewState.Remove("ProductionDetailTable");
        ViewState.Remove("ProductionStageTable");
        ViewState.Remove("ProductionPipelineUnitTable");
        ViewState.Remove("ProductionPipelineEmployeeTable");

        BindProductionStages();
        BindProductionPipelineUnit();
        BindProductionPipelineEmployee();

        ShowStep(1);
    }
}