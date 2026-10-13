using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;
using DataHelper.Framework;

public partial class Production_Create_Production : System.Web.UI.Page
{
    PR_PRODUCTION_MASTER ProdMasterEnt = new PR_PRODUCTION_MASTER();
    PR_PRODUCTION_MASTERService ProdMasterSer = new PR_PRODUCTION_MASTERService();

    PR_PRODCUTION_DETAIL ProdDetEnt = new PR_PRODCUTION_DETAIL();
    PR_PRODCUTION_DETAILService ProdDetSer = new PR_PRODCUTION_DETAILService();

    PR_PRODUCTION_STAGE ProdStageEnt = new PR_PRODUCTION_STAGE();
    PR_PRODUCTION_STAGEService ProdStageSer = new PR_PRODUCTION_STAGEService();

    PR_PRODUCITON_PIPELINE ProdPipeEnt = new PR_PRODUCITON_PIPELINE();
    PR_PRODUCITON_PIPELINEService ProdPipeSer = new PR_PRODUCITON_PIPELINEService();

    PR_PURCHASE_ORDER PoEnt = new PR_PURCHASE_ORDER();
    PR_PURCHASE_ORDERService PoSer = new PR_PURCHASE_ORDERService();

    PR_MULTIPLE_DELIVERY MdEnt = new PR_MULTIPLE_DELIVERY();
    PR_MULTIPLE_DELIVERYService MdSer = new PR_MULTIPLE_DELIVERYService();

    PR_ORDER_DETAIL OdEnt = new PR_ORDER_DETAIL();
    PR_ORDER_DETAILService OdSer = new PR_ORDER_DETAILService();

    CUSTOMER CustEnt = new CUSTOMER();
    CUSTOMERService CustSer = new CUSTOMERService();

    PR_CUSTOMER_LOCATION LocEnt = new PR_CUSTOMER_LOCATION();
    PR_CUSTOMER_LOCATIONService LocSer = new PR_CUSTOMER_LOCATIONService();

    PhyeGanDate PGD = new PhyeGanDate();

    private const string ProdDetailsKey = "ProdDetailsTable";
    private const string ProdStagesKey = "ProdStagesTable";
    private const string PipeUnitKey = "PipeUnitTable";
    private const string PipeEmpKey = "PipeEmpTable";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtProductionDate.Text = PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy");

            InitializeTables();
            ShowStep(1);
        }
    }

    private void ShowStep(int step)
    {
        pnlProductionMaster.Visible = (step == 1);
        pnlProductionDetails.Visible = (step == 2);
        pnlProductionStages.Visible = (step == 3);
        pnlProductionPipeline.Visible = (step == 4);
    }

    private void InitializeTables()
    {
        DataTable dtDetails = new DataTable();
        dtDetails.Columns.Add("ROW_ID", typeof(string));
        dtDetails.Columns.Add("PRODUCT_ID", typeof(string));
        dtDetails.Columns.Add("PRODUCT_NAME", typeof(string));
        dtDetails.Columns.Add("SIZE_ID", typeof(string));
        dtDetails.Columns.Add("SIZE_NAME", typeof(string));
        dtDetails.Columns.Add("SWATCH_ID", typeof(string));
        dtDetails.Columns.Add("SWATCH_NAME", typeof(string));
        dtDetails.Columns.Add("QUANTITY", typeof(string));
        dtDetails.Columns.Add("UNIT", typeof(string));
        dtDetails.Columns.Add("MULTIPLE_DELVIERY_ID", typeof(string));
        ViewState[ProdDetailsKey] = dtDetails;

        DataTable dtStages = new DataTable();
        dtStages.Columns.Add("ROW_ID", typeof(string));
        dtStages.Columns.Add("PRODUCTION_DETAIL_ROW_ID", typeof(string));
        dtStages.Columns.Add("STAGE_MASTER_ID", typeof(string));
        dtStages.Columns.Add("UNIT_OR_EMPLOYEE", typeof(string));
        dtStages.Columns.Add("STATUS", typeof(string));
        ViewState[ProdStagesKey] = dtStages;

        DataTable dtPipeUnit = new DataTable();
        dtPipeUnit.Columns.Add("ROW_ID", typeof(string));
        dtPipeUnit.Columns.Add("PRODUCTION_DETAIL_ROW_ID", typeof(string));
        dtPipeUnit.Columns.Add("STAGE_ROW_ID", typeof(string));
        dtPipeUnit.Columns.Add("PRODUCTION_UNIT", typeof(string));
        dtPipeUnit.Columns.Add("PRODUCTION_DIVISION", typeof(string));
        dtPipeUnit.Columns.Add("ESTIMATED_DAY", typeof(string));
        dtPipeUnit.Columns.Add("ESTIMATED_TIME", typeof(string));
        ViewState[PipeUnitKey] = dtPipeUnit;

        DataTable dtPipeEmp = new DataTable();
        dtPipeEmp.Columns.Add("ROW_ID", typeof(string));
        dtPipeEmp.Columns.Add("PRODUCTION_DETAIL_ROW_ID", typeof(string));
        dtPipeEmp.Columns.Add("STAGE_ROW_ID", typeof(string));
        dtPipeEmp.Columns.Add("EMPLOYEE_ID", typeof(string));
        dtPipeEmp.Columns.Add("ESTIMATED_DAY", typeof(string));
        dtPipeEmp.Columns.Add("ESTIMATED_TIME", typeof(string));
        ViewState[PipeEmpKey] = dtPipeEmp;
    }

    #region STEP 1: PRODUCTION MASTER & SCHEDULED DELIVERIES

    private DataTable ScheduledDeliveriesTable
    {
        get { return ViewState["ScheduledDeliveriesTable"] as DataTable; }
        set { ViewState["ScheduledDeliveriesTable"] = value; }
    }

    private string GridSortExpression
    {
        get { return ViewState["GridSortExpression"] as string ?? "PARSED_DATE"; }
        set { ViewState["GridSortExpression"] = value; }
    }

    private string GridSortDirection
    {
        get { return ViewState["GridSortDirection"] as string ?? "ASC"; }
        set { ViewState["GridSortDirection"] = value; }
    }

    protected void ddlProductionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedType = ddlProductionType.SelectedValue;

        if (selectedType == "PO")
        {
            btnNextMaster.Visible = false;
            pnlPOGrid.Visible = true;
            LoadScheduledDeliveriesGrid();
        }
        else
        {
            btnNextMaster.Visible = true;
            pnlPOGrid.Visible = false;
            hfCustomerPoId.Value = "";
            hfMultipleDeliveryId.Value = "";
            hfAutomatedCompletionDate.Value = "";
        }
    }

    private DateTime ParseDateToDateTime(string dateStr)
    {
        if (string.IsNullOrWhiteSpace(dateStr))
            return DateTime.MaxValue;

        string trimmed = dateStr.Trim();
        DateTime parsedDate;
        string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "yyyy/MM/dd", "yyyy-MM-dd" };

        if (DateTime.TryParseExact(trimmed, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            return parsedDate;

        if (DateTime.TryParse(trimmed, out parsedDate))
            return parsedDate;

        try
        {
            string engDate = PGD.GetEnglishDateFromNepali(trimmed, "dd/mm/yyyy");
            if (DateTime.TryParse(engDate, out parsedDate))
                return parsedDate;
        }
        catch { }

        return DateTime.MaxValue;
    }

    private void LoadScheduledDeliveriesGrid()
    {
        DataTable dtDeliveries = new DataTable();
        dtDeliveries.Columns.Add("MULTIPLE_DELIVERY_ID", typeof(string));
        dtDeliveries.Columns.Add("ORDER_NUMBER", typeof(string));
        dtDeliveries.Columns.Add("CUSTOMER_NAME", typeof(string));
        dtDeliveries.Columns.Add("DELIVERY_DATE", typeof(string));
        dtDeliveries.Columns.Add("PARSED_DATE", typeof(DateTime));
        dtDeliveries.Columns.Add("CUSTOMER_LOCATION", typeof(string));

        MdEnt = new PR_MULTIPLE_DELIVERY();
        MdEnt.STATUS = "1";
        EntityList mdList = MdSer.GetAll(MdEnt);

        if (mdList != null)
        {
            foreach (PR_MULTIPLE_DELIVERY md in mdList)
            {
                string orderNumber = "";
                string customerName = "";

                if (!string.IsNullOrEmpty(md.PURCHASE_ORDER_ID))
                {
                    PoEnt = new PR_PURCHASE_ORDER();
                    PoEnt.PK_ID = md.PURCHASE_ORDER_ID;
                    PoEnt = (PR_PURCHASE_ORDER)PoSer.GetSingle(PoEnt);

                    if (PoEnt != null)
                    {
                        orderNumber = PoEnt.ORDER_NUMBER;
                        if (!string.IsNullOrEmpty(PoEnt.CUSTOMER_ID))
                        {
                            CustEnt = new CUSTOMER();
                            CustEnt.PK_ID = PoEnt.CUSTOMER_ID;
                            CustEnt = (CUSTOMER)CustSer.GetSingle(CustEnt);
                            customerName = CustEnt != null ? CustEnt.CUSTOMER_NAME : "";
                        }
                    }
                }

                string locAddress = "";
                if (!string.IsNullOrEmpty(md.CUSOMTER_LOCATION_ID))
                {
                    LocEnt = new PR_CUSTOMER_LOCATION();
                    LocEnt.PK_ID = md.CUSOMTER_LOCATION_ID;
                    LocEnt = (PR_CUSTOMER_LOCATION)LocSer.GetSingle(LocEnt);
                    locAddress = LocEnt != null ? LocEnt.ADDRESS : "";
                }

                DataRow dr = dtDeliveries.NewRow();
                dr["MULTIPLE_DELIVERY_ID"] = md.PK_ID;
                dr["ORDER_NUMBER"] = orderNumber;
                dr["CUSTOMER_NAME"] = customerName;
                dr["DELIVERY_DATE"] = md.DELIVERY_DATE;
                dr["PARSED_DATE"] = ParseDateToDateTime(md.DELIVERY_DATE);
                dr["CUSTOMER_LOCATION"] = locAddress;
                dtDeliveries.Rows.Add(dr);
            }
        }

        GridSortExpression = "PARSED_DATE";
        GridSortDirection = "ASC";
        ScheduledDeliveriesTable = dtDeliveries;

        BindSortedDeliveries();
    }

    private void BindSortedDeliveries()
    {
        DataTable dt = ScheduledDeliveriesTable;
        if (dt != null)
        {
            DataView dv = dt.DefaultView;
            dv.Sort = GridSortExpression + " " + GridSortDirection;
            grdPurchaseOrders.DataSource = dv;
            grdPurchaseOrders.DataBind();
        }
    }

    protected void grdPurchaseOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPurchaseOrders.PageIndex = e.NewPageIndex;
        BindSortedDeliveries();
    }

    protected void grdPurchaseOrders_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (GridSortExpression == e.SortExpression)
        {
            GridSortDirection = GridSortDirection == "ASC" ? "DESC" : "ASC";
        }
        else
        {
            GridSortExpression = e.SortExpression;
            GridSortDirection = "ASC";
        }

        grdPurchaseOrders.PageIndex = 0;
        BindSortedDeliveries();
    }

    public string GetUrgencyBadge(object deliveryDateObj)
    {
        if (deliveryDateObj == null || string.IsNullOrEmpty(deliveryDateObj.ToString()))
            return "<span class=\"badge-stat\">N/A</span>";

        DateTime deliveryDate = ParseDateToDateTime(deliveryDateObj.ToString());
        if (deliveryDate == DateTime.MaxValue)
            return "<span class=\"badge-stat\">" + deliveryDateObj.ToString() + "</span>";

        int remainingDays = (int)(deliveryDate.Date - DateTime.Now.Date).TotalDays;

        if (remainingDays < 0)
        {
            return "<span style=\"color:#e53e3e; font-weight:600; padding:3px 8px; background:#fff5f5; border:1px solid #feb2b2; border-radius:4px;\">Overdue (" + Math.Abs(remainingDays) + "d)</span>";
        }
        else if (remainingDays <= 5)
        {
            return "<span style=\"color:#dd6b20; font-weight:600; padding:3px 8px; background:#fffaf0; border:1px solid #fbd38d; border-radius:4px;\">Urgent (" + remainingDays + "d)</span>";
        }
        else if (remainingDays <= 15)
        {
            return "<span style=\"color:#3182ce; font-weight:600; padding:3px 8px; background:#ebf8ff; border:1px solid #bee3f8; border-radius:4px;\">Moderate (" + remainingDays + "d)</span>";
        }
        else
        {
            return "<span style=\"color:#38a169; font-weight:600; padding:3px 8px; background:#f0fff4; border:1px solid #c6f6d5; border-radius:4px;\">Normal (" + remainingDays + "d)</span>";
        }
    }

    protected void grdPurchaseOrders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CreateFromDelivery")
        {
            string mdId = Convert.ToString(e.CommandArgument);
            hfMultipleDeliveryId.Value = mdId;

            MdEnt = new PR_MULTIPLE_DELIVERY();
            MdEnt.PK_ID = mdId;
            MdEnt = (PR_MULTIPLE_DELIVERY)MdSer.GetSingle(MdEnt);

            if (MdEnt != null)
            {
                hfCustomerPoId.Value = MdEnt.PURCHASE_ORDER_ID;
                hfAutomatedCompletionDate.Value = MdEnt.DELIVERY_DATE;

                lblDetailsDeliveryTarget.Text = MdEnt.DELIVERY_DATE;

                PoEnt = new PR_PURCHASE_ORDER();
                PoEnt.PK_ID = MdEnt.PURCHASE_ORDER_ID;
                PoEnt = (PR_PURCHASE_ORDER)PoSer.GetSingle(PoEnt);
                lblStageOrderNumber.Text = PoEnt != null ? PoEnt.ORDER_NUMBER : "";
            }

            lblDetailsProductionDate.Text = txtProductionDate.Text;
            lblStageProductionDate.Text = txtProductionDate.Text;

            LoadProductsFromDelivery(mdId);
            ShowStep(2);
        }
        else if (e.CommandName == "ViewDeliveryDetails")
        {
            string mdId = Convert.ToString(e.CommandArgument);
            Response.Redirect("View_Delivery_Details.aspx?DeliveryId=" + Server.UrlEncode(mdId));
        }
    }

    protected void btnNextMaster_Click(object sender, EventArgs e)
    {
        hfCustomerPoId.Value = "";
        hfMultipleDeliveryId.Value = "";
        hfAutomatedCompletionDate.Value = "";

        lblDetailsProductionDate.Text = txtProductionDate.Text;
        lblDetailsDeliveryTarget.Text = "IN-HOUSE (Automated)";
        lblStageProductionDate.Text = txtProductionDate.Text;
        lblStageOrderNumber.Text = "IN-HOUSE";

        ShowStep(2);
    }

    protected void btnClearMaster_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    #endregion

    #region STEP 2: PRODUCTION DETAILS

    private void LoadProductsFromDelivery(string mdId)
    {
        OdEnt = new PR_ORDER_DETAIL();
        OdEnt.MULTIPLE_DELVIERY_ID = mdId;
        EntityList odList = OdSer.GetAll(OdEnt);

        DataTable dtProd = (DataTable)ViewState[ProdDetailsKey];
        dtProd.Rows.Clear();

        if (odList != null)
        {
            foreach (PR_ORDER_DETAIL od in odList)
            {
                DataRow nr = dtProd.NewRow();
                nr["ROW_ID"] = Guid.NewGuid().ToString("N");
                nr["PRODUCT_ID"] = od.PRODUCT_ID;
                nr["PRODUCT_NAME"] = GetProductName(od.PRODUCT_ID);
                nr["SIZE_ID"] = od.SIZE_ID;
                nr["SIZE_NAME"] = GetSizeName(od.SIZE_ID);
                nr["SWATCH_ID"] = od.SWATCH_ID;
                nr["SWATCH_NAME"] = GetSwatchName(od.SWATCH_ID);
                nr["QUANTITY"] = od.QUANTITY;
                nr["UNIT"] = od.UNIT;
                nr["MULTIPLE_DELVIERY_ID"] = mdId;
                dtProd.Rows.Add(nr);
            }
        }

        ViewState[ProdDetailsKey] = dtProd;
        grdProductionDetails.DataSource = dtProd;
        grdProductionDetails.DataBind();
    }

    private void ReadProductionDetails()
    {
        DataTable dtProd = (DataTable)ViewState[ProdDetailsKey];

        foreach (GridViewRow row in grdProductionDetails.Rows)
        {
            LinkButton btnRemove = row.FindControl("btnRemoveProduct") as LinkButton;
            if (btnRemove == null) continue;

            string rowId = btnRemove.CommandArgument;
            DataRow[] matches = dtProd.Select("ROW_ID = '" + rowId.Replace("'", "''") + "'");
            if (matches.Length == 0) continue;

            TextBox txtQty = row.FindControl("txtQuantity") as TextBox;
            TextBox txtU = row.FindControl("txtUnit") as TextBox;

            matches[0]["QUANTITY"] = txtQty != null ? txtQty.Text.Trim() : "";
            matches[0]["UNIT"] = txtU != null ? txtU.Text.Trim() : "";
        }

        ViewState[ProdDetailsKey] = dtProd;
    }

    protected void grdProductionDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RemoveProduct")
        {
            ReadProductionDetails();
            string rowId = Convert.ToString(e.CommandArgument);
            DataTable dtProd = (DataTable)ViewState[ProdDetailsKey];

            DataRow[] matches = dtProd.Select("ROW_ID = '" + rowId.Replace("'", "''") + "'");
            if (matches.Length > 0)
                dtProd.Rows.Remove(matches[0]);

            ViewState[ProdDetailsKey] = dtProd;
            grdProductionDetails.DataSource = dtProd;
            grdProductionDetails.DataBind();
        }
    }

    protected void btnBackMaster_Click(object sender, EventArgs e)
    {
        ReadProductionDetails();
        ShowStep(1);
    }

    protected void btnNextDetails_Click(object sender, EventArgs e)
    {
        ReadProductionDetails();
        BindStagesGrid();
        ShowStep(3);
    }

    #endregion

    #region STEP 3: PRODUCTION STAGE

    private void BindStagesGrid()
    {
        DataTable dtStages = (DataTable)ViewState[ProdStagesKey];
        grdProductionStages.DataSource = dtStages;
        grdProductionStages.DataBind();
    }

    private void ReadStages()
    {
        DataTable dtStages = (DataTable)ViewState[ProdStagesKey];

        foreach (GridViewRow row in grdProductionStages.Rows)
        {
            HiddenField hfRowId = row.FindControl("hfStageRowId") as HiddenField;
            if (hfRowId == null) continue;

            DataRow[] matches = dtStages.Select("ROW_ID = '" + hfRowId.Value.Replace("'", "''") + "'");
            if (matches.Length == 0) continue;

            DropDownList ddlProd = row.FindControl("ddlStageProduct") as DropDownList;
            DropDownList ddlStg = row.FindControl("ddlStageMaster") as DropDownList;
            DropDownList ddlAssign = row.FindControl("ddlStageAssignmentType") as DropDownList;
            DropDownList ddlStat = row.FindControl("ddlStageStatus") as DropDownList;

            matches[0]["PRODUCTION_DETAIL_ROW_ID"] = ddlProd != null ? ddlProd.SelectedValue : "";
            matches[0]["STAGE_MASTER_ID"] = ddlStg != null ? ddlStg.SelectedValue : "";
            matches[0]["UNIT_OR_EMPLOYEE"] = ddlAssign != null ? ddlAssign.SelectedValue : "";
            matches[0]["STATUS"] = ddlStat != null ? ddlStat.SelectedValue : "";
        }

        ViewState[ProdStagesKey] = dtStages;
    }

    protected void btnAddStage_Click(object sender, EventArgs e)
    {
        ReadStages();

        DataTable dtStages = (DataTable)ViewState[ProdStagesKey];
        DataRow nr = dtStages.NewRow();
        nr["ROW_ID"] = Guid.NewGuid().ToString("N");
        nr["PRODUCTION_DETAIL_ROW_ID"] = "";
        nr["STAGE_MASTER_ID"] = "";
        nr["UNIT_OR_EMPLOYEE"] = "";
        nr["STATUS"] = "Pending";
        dtStages.Rows.Add(nr);

        ViewState[ProdStagesKey] = dtStages;
        BindStagesGrid();
    }

    protected void grdProductionStages_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RemoveStage")
        {
            ReadStages();
            string rowId = Convert.ToString(e.CommandArgument);
            DataTable dtStages = (DataTable)ViewState[ProdStagesKey];

            DataRow[] matches = dtStages.Select("ROW_ID = '" + rowId.Replace("'", "''") + "'");
            if (matches.Length > 0)
                dtStages.Rows.Remove(matches[0]);

            ViewState[ProdStagesKey] = dtStages;
            BindStagesGrid();
        }
    }

    protected void grdProductionStages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        DropDownList ddlProd = e.Row.FindControl("ddlStageProduct") as DropDownList;
        DropDownList ddlStg = e.Row.FindControl("ddlStageMaster") as DropDownList;
        DropDownList ddlAssign = e.Row.FindControl("ddlStageAssignmentType") as DropDownList;
        DropDownList ddlStat = e.Row.FindControl("ddlStageStatus") as DropDownList;

        DataTable dtProd = (DataTable)ViewState[ProdDetailsKey];
        if (ddlProd != null && dtProd != null)
        {
            ddlProd.DataSource = dtProd;
            ddlProd.DataTextField = "PRODUCT_NAME";
            ddlProd.DataValueField = "ROW_ID";
            ddlProd.DataBind();
            ddlProd.Items.Insert(0, new ListItem("-- Select Product --", ""));
        }

        if (ddlStg != null)
        {
            ddlStg.Items.Clear();
            ddlStg.Items.Insert(0, new ListItem("-- Select Stage --", ""));
            try
            {
                PR_STAGE_MASTER smEnt = new PR_STAGE_MASTER();
                PR_STAGE_MASTERService smSer = new PR_STAGE_MASTERService();
                ddlStg.DataSource = smSer.GetAll(smEnt);
                ddlStg.DataTextField = "STAGE_NAME";
                ddlStg.DataValueField = "PK_ID";
                ddlStg.DataBind();
                ddlStg.Items.Insert(0, new ListItem("-- Select Stage --", ""));
            }
            catch { }
        }

        DataRowView rv = e.Row.DataItem as DataRowView;
        if (rv != null)
        {
            SetSelectedValue(ddlProd, rv["PRODUCTION_DETAIL_ROW_ID"]);
            SetSelectedValue(ddlStg, rv["STAGE_MASTER_ID"]);
            SetSelectedValue(ddlAssign, rv["UNIT_OR_EMPLOYEE"]);
            SetSelectedValue(ddlStat, rv["STATUS"]);
        }
    }

    protected void btnBackDetails_Click(object sender, EventArgs e)
    {
        ReadStages();
        ShowStep(2);
    }

    protected void btnNextStages_Click(object sender, EventArgs e)
    {
        ReadStages();

        DataTable dtStages = (DataTable)ViewState[ProdStagesKey];
        bool hasUnit = false;
        bool hasEmployee = false;

        foreach (DataRow r in dtStages.Rows)
        {
            string assignType = Convert.ToString(r["UNIT_OR_EMPLOYEE"]);
            if (assignType == "Unit") hasUnit = true;
            if (assignType == "Employee") hasEmployee = true;
        }

        pnlPipelineUnit.Visible = hasUnit;
        pnlPipelineEmployee.Visible = hasEmployee;

        BindPipelineUnitGrid();
        BindPipelineEmployeeGrid();

        ShowStep(4);
    }

    #endregion

    #region STEP 4: PRODUCTION PIPELINE & AUTOMATED COMPLETION DATE

    private void BindPipelineUnitGrid()
    {
        DataTable dt = (DataTable)ViewState[PipeUnitKey];
        grdProductionPipelineUnit.DataSource = dt;
        grdProductionPipelineUnit.DataBind();
    }

    private void BindPipelineEmployeeGrid()
    {
        DataTable dt = (DataTable)ViewState[PipeEmpKey];
        grdProductionPipelineEmployee.DataSource = dt;
        grdProductionPipelineEmployee.DataBind();
    }

    private void ReadPipelineUnit()
    {
        DataTable dt = (DataTable)ViewState[PipeUnitKey];

        foreach (GridViewRow row in grdProductionPipelineUnit.Rows)
        {
            HiddenField hfRowId = row.FindControl("hfPipelineUnitRowId") as HiddenField;
            if (hfRowId == null) continue;

            DataRow[] matches = dt.Select("ROW_ID = '" + hfRowId.Value.Replace("'", "''") + "'");
            if (matches.Length == 0) continue;

            DropDownList ddlProd = row.FindControl("ddlPipelineDetail") as DropDownList;
            DropDownList ddlStg = row.FindControl("ddlPipelineStage") as DropDownList;
            DropDownList ddlU = row.FindControl("ddlPipelineUnit") as DropDownList;
            DropDownList ddlDiv = row.FindControl("ddlPipelineDivision") as DropDownList;
            TextBox txtD = row.FindControl("txtEstimatedDay") as TextBox;
            TextBox txtT = row.FindControl("txtEstimatedTime") as TextBox;

            matches[0]["PRODUCTION_DETAIL_ROW_ID"] = ddlProd != null ? ddlProd.SelectedValue : "";
            matches[0]["STAGE_ROW_ID"] = ddlStg != null ? ddlStg.SelectedValue : "";
            matches[0]["PRODUCTION_UNIT"] = ddlU != null ? ddlU.SelectedValue : "";
            matches[0]["PRODUCTION_DIVISION"] = ddlDiv != null ? ddlDiv.SelectedValue : "";
            matches[0]["ESTIMATED_DAY"] = txtD != null ? txtD.Text.Trim() : "";
            matches[0]["ESTIMATED_TIME"] = txtT != null ? txtT.Text.Trim() : "";
        }

        ViewState[PipeUnitKey] = dt;
    }

    private void ReadPipelineEmployee()
    {
        DataTable dt = (DataTable)ViewState[PipeEmpKey];

        foreach (GridViewRow row in grdProductionPipelineEmployee.Rows)
        {
            HiddenField hfRowId = row.FindControl("hfPipelineEmpRowId") as HiddenField;
            if (hfRowId == null) continue;

            DataRow[] matches = dt.Select("ROW_ID = '" + hfRowId.Value.Replace("'", "''") + "'");
            if (matches.Length == 0) continue;

            DropDownList ddlProd = row.FindControl("ddlPipelineDetail") as DropDownList;
            DropDownList ddlStg = row.FindControl("ddlPipelineStage") as DropDownList;
            DropDownList ddlEmp = row.FindControl("ddlPipelineEmployee") as DropDownList;
            TextBox txtD = row.FindControl("txtEstimatedDay") as TextBox;
            TextBox txtT = row.FindControl("txtEstimatedTime") as TextBox;

            matches[0]["PRODUCTION_DETAIL_ROW_ID"] = ddlProd != null ? ddlProd.SelectedValue : "";
            matches[0]["STAGE_ROW_ID"] = ddlStg != null ? ddlStg.SelectedValue : "";
            matches[0]["EMPLOYEE_ID"] = ddlEmp != null ? ddlEmp.SelectedValue : "";
            matches[0]["ESTIMATED_DAY"] = txtD != null ? txtD.Text.Trim() : "";
            matches[0]["ESTIMATED_TIME"] = txtT != null ? txtT.Text.Trim() : "";
        }

        ViewState[PipeEmpKey] = dt;
    }

    protected void btnAddPipelineUnit_Click(object sender, EventArgs e)
    {
        ReadPipelineUnit();

        DataTable dt = (DataTable)ViewState[PipeUnitKey];
        DataRow nr = dt.NewRow();
        nr["ROW_ID"] = Guid.NewGuid().ToString("N");
        nr["PRODUCTION_DETAIL_ROW_ID"] = "";
        nr["STAGE_ROW_ID"] = "";
        nr["PRODUCTION_UNIT"] = "";
        nr["PRODUCTION_DIVISION"] = "";
        nr["ESTIMATED_DAY"] = "";
        nr["ESTIMATED_TIME"] = "";
        dt.Rows.Add(nr);

        ViewState[PipeUnitKey] = dt;
        BindPipelineUnitGrid();
    }

    protected void btnAddPipelineEmployee_Click(object sender, EventArgs e)
    {
        ReadPipelineEmployee();

        DataTable dt = (DataTable)ViewState[PipeEmpKey];
        DataRow nr = dt.NewRow();
        nr["ROW_ID"] = Guid.NewGuid().ToString("N");
        nr["PRODUCTION_DETAIL_ROW_ID"] = "";
        nr["STAGE_ROW_ID"] = "";
        nr["EMPLOYEE_ID"] = "";
        nr["ESTIMATED_DAY"] = "";
        nr["ESTIMATED_TIME"] = "";
        dt.Rows.Add(nr);

        ViewState[PipeEmpKey] = dt;
        BindPipelineEmployeeGrid();
    }

    protected void grdProductionPipelineUnit_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RemovePipelineUnit")
        {
            ReadPipelineUnit();
            string rowId = Convert.ToString(e.CommandArgument);
            DataTable dt = (DataTable)ViewState[PipeUnitKey];

            DataRow[] matches = dt.Select("ROW_ID = '" + rowId.Replace("'", "''") + "'");
            if (matches.Length > 0)
                dt.Rows.Remove(matches[0]);

            ViewState[PipeUnitKey] = dt;
            BindPipelineUnitGrid();
        }
    }

    protected void grdProductionPipelineEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RemovePipelineEmployee")
        {
            ReadPipelineEmployee();
            string rowId = Convert.ToString(e.CommandArgument);
            DataTable dt = (DataTable)ViewState[PipeEmpKey];

            DataRow[] matches = dt.Select("ROW_ID = '" + rowId.Replace("'", "''") + "'");
            if (matches.Length > 0)
                dt.Rows.Remove(matches[0]);

            ViewState[PipeEmpKey] = dt;
            BindPipelineEmployeeGrid();
        }
    }

    protected void grdProductionPipelineUnit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        DropDownList ddlProd = e.Row.FindControl("ddlPipelineDetail") as DropDownList;
        DropDownList ddlStg = e.Row.FindControl("ddlPipelineStage") as DropDownList;
        DropDownList ddlU = e.Row.FindControl("ddlPipelineUnit") as DropDownList;
        DropDownList ddlDiv = e.Row.FindControl("ddlPipelineDivision") as DropDownList;

        PopulatePipelineDropdowns(ddlProd, ddlStg);

        if (ddlU != null)
        {
            ddlU.Items.Clear();
            ddlU.Items.Insert(0, new ListItem("-- Select Unit --", ""));
        }

        DataRowView rv = e.Row.DataItem as DataRowView;
        if (rv != null)
        {
            SetSelectedValue(ddlProd, rv["PRODUCTION_DETAIL_ROW_ID"]);
            SetSelectedValue(ddlStg, rv["STAGE_ROW_ID"]);
            SetSelectedValue(ddlU, rv["PRODUCTION_UNIT"]);
            SetSelectedValue(ddlDiv, rv["PRODUCTION_DIVISION"]);
        }
    }

    protected void grdProductionPipelineEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        DropDownList ddlProd = e.Row.FindControl("ddlPipelineDetail") as DropDownList;
        DropDownList ddlStg = e.Row.FindControl("ddlPipelineStage") as DropDownList;
        DropDownList ddlEmp = e.Row.FindControl("ddlPipelineEmployee") as DropDownList;

        PopulatePipelineDropdowns(ddlProd, ddlStg);

        if (ddlEmp != null)
        {
            ddlEmp.Items.Clear();
            ddlEmp.Items.Insert(0, new ListItem("-- Select Employee --", ""));
        }

        DataRowView rv = e.Row.DataItem as DataRowView;
        if (rv != null)
        {
            SetSelectedValue(ddlProd, rv["PRODUCTION_DETAIL_ROW_ID"]);
            SetSelectedValue(ddlStg, rv["STAGE_ROW_ID"]);
            SetSelectedValue(ddlEmp, rv["EMPLOYEE_ID"]);
        }
    }

    private void PopulatePipelineDropdowns(DropDownList ddlProd, DropDownList ddlStg)
    {
        DataTable dtProd = (DataTable)ViewState[ProdDetailsKey];
        if (ddlProd != null && dtProd != null)
        {
            ddlProd.DataSource = dtProd;
            ddlProd.DataTextField = "PRODUCT_NAME";
            ddlProd.DataValueField = "ROW_ID";
            ddlProd.DataBind();
            ddlProd.Items.Insert(0, new ListItem("-- Select Product --", ""));
        }

        DataTable dtStages = (DataTable)ViewState[ProdStagesKey];
        if (ddlStg != null && dtStages != null)
        {
            ddlStg.DataSource = dtStages;
            ddlStg.DataTextField = "STAGE_MASTER_ID";
            ddlStg.DataValueField = "ROW_ID";
            ddlStg.DataBind();
            ddlStg.Items.Insert(0, new ListItem("-- Select Stage --", ""));
        }
    }

    protected void ddlPipelineUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlU = sender as DropDownList;
        GridViewRow row = ddlU != null ? ddlU.NamingContainer as GridViewRow : null;
        if (row == null) return;

        DropDownList ddlDiv = row.FindControl("ddlPipelineDivision") as DropDownList;
        if (ddlDiv == null) return;

        ddlDiv.Items.Clear();
        ddlDiv.Items.Insert(0, new ListItem("-- Select Division --", ""));
    }

    protected void btnBackStages_Click(object sender, EventArgs e)
    {
        ReadPipelineUnit();
        ReadPipelineEmployee();
        ShowStep(3);
    }

    #endregion

    #region SAVE PRODUCTION WITH DISTRIBUTED TRANSACTION

    private string CalculateAutomatedCompletionDate(string productionDateNep)
    {
        if (!string.IsNullOrEmpty(hfAutomatedCompletionDate.Value))
        {
            return hfAutomatedCompletionDate.Value;
        }

        int maxDays = 0;
        DataTable dtUnitPipe = (DataTable)ViewState[PipeUnitKey];
        if (dtUnitPipe != null)
        {
            foreach (DataRow r in dtUnitPipe.Rows)
            {
                int d = 0;
                int.TryParse(Convert.ToString(r["ESTIMATED_DAY"]), out d);
                if (d > maxDays) maxDays = d;
            }
        }

        DataTable dtEmpPipe = (DataTable)ViewState[PipeEmpKey];
        if (dtEmpPipe != null)
        {
            foreach (DataRow r in dtEmpPipe.Rows)
            {
                int d = 0;
                int.TryParse(Convert.ToString(r["ESTIMATED_DAY"]), out d);
                if (d > maxDays) maxDays = d;
            }
        }

        try
        {
            string engProdDate = PGD.GetEnglishDateFromNepali(productionDateNep, "dd/mm/yyyy");
            DateTime dt = DateTime.Parse(engProdDate);
            DateTime compDt = dt.AddDays(maxDays);
            return PGD.GetNepaliDateFromEnglish(compDt.ToString("dd/MM/yyyy"), "dd/mm/yyyy");
        }
        catch
        {
            return productionDateNep;
        }
    }

    protected void btnSaveProduction_Click(object sender, EventArgs e)
    {
        ReadPipelineUnit();
        ReadPipelineEmployee();

        DistributedTransaction DT = new DistributedTransaction();

        try
        {
            string nepProdDate = txtProductionDate.Text.Trim();
            string[] nepProdParts = (nepProdDate + "//").Split('/');

            string autoCompDate = CalculateAutomatedCompletionDate(nepProdDate);
            string[] autoCompParts = (autoCompDate + "//").Split('/');

            // 1. INSERT PR_PRODUCTION_MASTER
            ProdMasterEnt = new PR_PRODUCTION_MASTER();

            EntityList pmList = ProdMasterSer.GetAll(new PR_PRODUCTION_MASTER());
            string lastPid = "";
            int pidNum = 0;
            if (pmList != null)
            {
                foreach (PR_PRODUCTION_MASTER row in pmList) { lastPid = row.PK_ID; }
            }
            int.TryParse(lastPid, out pidNum);
            pidNum = pidNum + 1;

            ProdMasterEnt.PRODUCTION_NUMBER = pidNum.ToString();
            ProdMasterEnt.PRODUCTION_DATE = PGD.GetEnglishDateFromNepali(nepProdDate, "dd/mm/yyyy");
            ProdMasterEnt.PRODUCTION_DAY = nepProdParts[0];
            ProdMasterEnt.PRODUCTION_MONTH = nepProdParts[1];
            ProdMasterEnt.PRODUCTION_YEAR = nepProdParts[2];
            ProdMasterEnt.COMPLITION_DATE = PGD.GetEnglishDateFromNepali(autoCompDate, "dd/mm/yyyy");
            ProdMasterEnt.COMPLITION_DAY = autoCompParts[0];
            ProdMasterEnt.COMPLITION_MONTH = autoCompParts[1];
            ProdMasterEnt.COMPLITION_YEAR = autoCompParts[2];
            ProdMasterEnt.PRODUCTION_TYPE = ddlProductionType.SelectedValue;
            ProdMasterEnt.CUSTOMER_PO_ID = hfCustomerPoId.Value;
            ProdMasterEnt.STATUS = "1";

            string prodMasterId = ProdMasterSer.Insert(ProdMasterEnt, DT).ToString();

            // 2. INSERT PR_PRODCUTION_DETAIL
            DataTable dtProd = (DataTable)ViewState[ProdDetailsKey];
            System.Collections.Generic.Dictionary<string, string> detailRowIdToPkMap = new System.Collections.Generic.Dictionary<string, string>();

            foreach (DataRow r in dtProd.Rows)
            {
                ProdDetEnt = new PR_PRODCUTION_DETAIL();
                ProdDetEnt.PRODUCTION_ID = prodMasterId;
                ProdDetEnt.MULTIPLE_DELVIERY_ID = Convert.ToString(r["MULTIPLE_DELVIERY_ID"]);
                ProdDetEnt.PRODUCT_ID = Convert.ToString(r["PRODUCT_ID"]);
                ProdDetEnt.SIZE_ID = Convert.ToString(r["SIZE_ID"]);
                ProdDetEnt.SWATCH_ID = Convert.ToString(r["SWATCH_ID"]);
                ProdDetEnt.QUANTITY = Convert.ToString(r["QUANTITY"]);
                ProdDetEnt.UNIT = Convert.ToString(r["UNIT"]);
                ProdDetEnt.STATUS = "1";
                ProdDetEnt.PRODUCTION_DATE = PGD.GetEnglishDateFromNepali(nepProdDate, "dd/mm/yyyy");
                ProdDetEnt.PRODUCTION_DAY = nepProdParts[0];
                ProdDetEnt.PRODUCTION_MONTH = nepProdParts[1];
                ProdDetEnt.PRODUCTION_YEAR = nepProdParts[2];
                ProdDetEnt.COMPLITION_DATE = PGD.GetEnglishDateFromNepali(autoCompDate, "dd/mm/yyyy");
                ProdDetEnt.COMPLITION_DAY = autoCompParts[0];
                ProdDetEnt.COMPLITION_MONTH = autoCompParts[1];
                ProdDetEnt.COMPLITION_YEAR = autoCompParts[2];

                string detailPkId = ProdDetSer.Insert(ProdDetEnt, DT).ToString();
                detailRowIdToPkMap[Convert.ToString(r["ROW_ID"])] = detailPkId;
            }

            // 3. INSERT PR_PRODUCTION_STAGE
            DataTable dtStages = (DataTable)ViewState[ProdStagesKey];
            System.Collections.Generic.Dictionary<string, string> stageRowIdToPkMap = new System.Collections.Generic.Dictionary<string, string>();

            foreach (DataRow r in dtStages.Rows)
            {
                string pRowId = Convert.ToString(r["PRODUCTION_DETAIL_ROW_ID"]);
                string mappedDetailId = detailRowIdToPkMap.ContainsKey(pRowId) ? detailRowIdToPkMap[pRowId] : "";

                ProdStageEnt = new PR_PRODUCTION_STAGE();
                ProdStageEnt.PRODCTION_MASTER_ID = prodMasterId;
                ProdStageEnt.PRODUCTION_DETAIL_ID = mappedDetailId;
                ProdStageEnt.STAGE_MASTER_ID = Convert.ToString(r["STAGE_MASTER_ID"]);
                ProdStageEnt.UNIT_OR_EMPOYEE = Convert.ToString(r["UNIT_OR_EMPLOYEE"]);
                ProdStageEnt.STATUS = Convert.ToString(r["STATUS"]);

                DataRow[] prMatch = dtProd.Select("ROW_ID = '" + pRowId.Replace("'", "''") + "'");
                ProdStageEnt.PRODUCT_ID = prMatch.Length > 0 ? Convert.ToString(prMatch[0]["PRODUCT_ID"]) : "";

                string stagePkId = ProdStageSer.Insert(ProdStageEnt, DT).ToString();
                stageRowIdToPkMap[Convert.ToString(r["ROW_ID"])] = stagePkId;
            }

            // 4. INSERT PR_PRODUCITON_PIPELINE
            // A. Unit Pipeline
            DataTable dtUnitPipe = (DataTable)ViewState[PipeUnitKey];
            foreach (DataRow r in dtUnitPipe.Rows)
            {
                string pRowId = Convert.ToString(r["PRODUCTION_DETAIL_ROW_ID"]);
                string sRowId = Convert.ToString(r["STAGE_ROW_ID"]);

                ProdPipeEnt = new PR_PRODUCITON_PIPELINE();
                ProdPipeEnt.PRODUCTION_DETAIL_ID = detailRowIdToPkMap.ContainsKey(pRowId) ? detailRowIdToPkMap[pRowId] : "";
                ProdPipeEnt.STAGE_ID = stageRowIdToPkMap.ContainsKey(sRowId) ? stageRowIdToPkMap[sRowId] : "";
                ProdPipeEnt.PRODUCITON_UNIT = Convert.ToString(r["PRODUCTION_UNIT"]);
                ProdPipeEnt.PRODUCTION_DIVISION = Convert.ToString(r["PRODUCTION_DIVISION"]);
                ProdPipeEnt.EMPLOYEE_ID = "";
                ProdPipeEnt.ESTIMATED_DAY = Convert.ToString(r["ESTIMATED_DAY"]);
                ProdPipeEnt.ESTIMATED_TIME = Convert.ToString(r["ESTIMATED_TIME"]);
                ProdPipeEnt.PRODUCTION_DATE = PGD.GetEnglishDateFromNepali(nepProdDate, "dd/mm/yyyy");
                ProdPipeEnt.PRODUCTION_DAY = nepProdParts[0];
                ProdPipeEnt.PRODUCTION_MONTH = nepProdParts[1];
                ProdPipeEnt.PRODUCTION_YEAR = nepProdParts[2];
                ProdPipeEnt.PRODUCTION_TIME = DateTime.Now.ToString("HH:mm:ss");
                ProdPipeEnt.COMPLITION_DATE = PGD.GetEnglishDateFromNepali(autoCompDate, "dd/mm/yyyy");
                ProdPipeEnt.COMPLITION_DAY = autoCompParts[0];
                ProdPipeEnt.COMPLITION_MONTH = autoCompParts[1];
                ProdPipeEnt.COMPLITION_YEAR = autoCompParts[2];
                ProdPipeEnt.COMPLITION_TIME = "";
                ProdPipeEnt.STATUS = "1";

                ProdPipeSer.Insert(ProdPipeEnt, DT);
            }

            // B. Employee Pipeline
            DataTable dtEmpPipe = (DataTable)ViewState[PipeEmpKey];
            foreach (DataRow r in dtEmpPipe.Rows)
            {
                string pRowId = Convert.ToString(r["PRODUCTION_DETAIL_ROW_ID"]);
                string sRowId = Convert.ToString(r["STAGE_ROW_ID"]);

                ProdPipeEnt = new PR_PRODUCITON_PIPELINE();
                ProdPipeEnt.PRODUCTION_DETAIL_ID = detailRowIdToPkMap.ContainsKey(pRowId) ? detailRowIdToPkMap[pRowId] : "";
                ProdPipeEnt.STAGE_ID = stageRowIdToPkMap.ContainsKey(sRowId) ? stageRowIdToPkMap[sRowId] : "";
                ProdPipeEnt.PRODUCITON_UNIT = "";
                ProdPipeEnt.PRODUCTION_DIVISION = "";
                ProdPipeEnt.EMPLOYEE_ID = Convert.ToString(r["EMPLOYEE_ID"]);
                ProdPipeEnt.ESTIMATED_DAY = Convert.ToString(r["ESTIMATED_DAY"]);
                ProdPipeEnt.ESTIMATED_TIME = Convert.ToString(r["ESTIMATED_TIME"]);
                ProdPipeEnt.PRODUCTION_DATE = PGD.GetEnglishDateFromNepali(nepProdDate, "dd/mm/yyyy");
                ProdPipeEnt.PRODUCTION_DAY = nepProdParts[0];
                ProdPipeEnt.PRODUCTION_MONTH = nepProdParts[1];
                ProdPipeEnt.PRODUCTION_YEAR = nepProdParts[2];
                ProdPipeEnt.PRODUCTION_TIME = DateTime.Now.ToString("HH:mm:ss");
                ProdPipeEnt.COMPLITION_DATE = PGD.GetEnglishDateFromNepali(autoCompDate, "dd/mm/yyyy");
                ProdPipeEnt.COMPLITION_DAY = autoCompParts[0];
                ProdPipeEnt.COMPLITION_MONTH = autoCompParts[1];
                ProdPipeEnt.COMPLITION_YEAR = autoCompParts[2];
                ProdPipeEnt.COMPLITION_TIME = "";
                ProdPipeEnt.STATUS = "1";

                ProdPipeSer.Insert(ProdPipeEnt, DT);
            }

            // 5. COMMIT OR ABORT
            if (DT.HAPPY == true)
            {
                DT.Commit();
                HelperFunction.MsgBox(this, this.GetType(), "Production Created Successfully.");
                ClearAll();
            }
            else
            {
                DT.Abort();
                HelperFunction.MsgBox(this, this.GetType(), "Failed to save production. Transaction rolled back.");
            }
        }
        catch (Exception ex)
        {
            DT.Abort();
            HelperFunction.MsgBox(this, this.GetType(), "Transaction error: " + ex.Message);
        }
        finally
        {
            DT.Dispose();
        }
    }

    #endregion

    #region UTILITY & LOOKUP METHODS

    private string GetProductName(string id)
    {
        if (string.IsNullOrEmpty(id)) return "";
        PRODUCT ent = new PRODUCT();
        ent.PK_ID = id;
        ent = (PRODUCT)new PRODUCTService().GetSingle(ent);
        return ent != null ? ent.PRODUCT_NAME : "";
    }

    private string GetSizeName(string id)
    {
        if (string.IsNullOrEmpty(id)) return "";
        PRODUCT_SIZE ent = new PRODUCT_SIZE();
        ent.PK_ID = id;
        ent = (PRODUCT_SIZE)new PRODUCT_SIZEService().GetSingle(ent);
        return ent != null ? ent.SIZE_NAME : "";
    }

    private string GetSwatchName(string id)
    {
        if (string.IsNullOrEmpty(id)) return "";
        PR_SWATCH ent = new PR_SWATCH();
        ent.PK_ID = id;
        ent = (PR_SWATCH)new PR_SWATCHService().GetSingle(ent);
        return ent != null ? ent.SWATCH_NAME : "";
    }

    private void SetSelectedValue(DropDownList ddl, object value)
    {
        if (ddl == null || value == null) return;
        string v = value.ToString();
        if (!string.IsNullOrEmpty(v) && ddl.Items.FindByValue(v) != null)
            ddl.SelectedValue = v;
    }

    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    private void ClearAll()
    {
        hfProductionId.Value = "";
        hfCustomerPoId.Value = "";
        hfMultipleDeliveryId.Value = "";
        hfAutomatedCompletionDate.Value = "";

        ddlProductionType.SelectedIndex = 0;
        txtProductionDate.Text = PGD.GetNepaliDateFromEnglish(PGD.GetTodayDate("dd/mm/yyyy"), "dd/mm/yyyy");

        btnNextMaster.Visible = true;
        pnlPOGrid.Visible = false;

        InitializeTables();
        ShowStep(1);
    }

    #endregion
}