using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Production_Create_Production : System.Web.UI.Page
{
    PhyeGanDate PGD = new PhyeGanDate();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtProductionDate.Text = GetTodayNepali();
            txtCompletionDate.Text = GetTodayNepali();

            BindProductionDetails();
            BindProductionStages();
            BindProductionPipeline();

            ShowStep(1);
        }
    }

    // =====================================================
    // DATE
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

        return new string[]
        {
            parts[0],
            parts[1],
            parts[2]
        };
    }

    // =====================================================
    // STEP NAVIGATION
    // =====================================================

    private void ShowStep(int step)
    {
        pnlProductionMaster.Visible = step == 1;
        pnlProductionDetails.Visible = step == 2;
        pnlProductionStages.Visible = step == 3;

        if (step == 2)
        {
            lblDetailsProductionDate.Text = txtProductionDate.Text;
            lblDetailsCompletionDate.Text = txtCompletionDate.Text;
        }
    }

    protected void btnNextMaster_Click(object sender, EventArgs e)
    {
        if (!ValidateMaster())
            return;

        ShowStep(2);
    }

    protected void btnBackMaster_Click(object sender, EventArgs e)
    {
        ReadProductionDetails();
        ShowStep(1);
    }

    protected void btnNextDetails_Click(object sender, EventArgs e)
    {
        ReadProductionDetails();

        if (!ValidateProductionDetails())
            return;

        ShowStep(3);
    }

    protected void btnBackDetails_Click(object sender, EventArgs e)
    {
        ReadProductionStages();
        ReadProductionPipeline();
        ShowStep(2);
    }

    // =====================================================
    // PRODUCTION MASTER
    // =====================================================

    private bool ValidateMaster()
    {
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

        if (string.IsNullOrWhiteSpace(txtProductionType.Text))
        {
            HelperFunction.MsgBox(this, GetType(), "Production type cannot be empty.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtCustomerPOId.Text))
        {
            HelperFunction.MsgBox(this, GetType(), "Customer PO ID cannot be empty.");
            return false;
        }

        return true;
    }

    protected void btnClearMaster_Click(object sender, EventArgs e)
    {
        txtProductionDate.Text = GetTodayNepali();
        txtCompletionDate.Text = GetTodayNepali();
        txtProductionType.Text = "";
        txtCustomerPOId.Text = "";
        hfProductionId.Value = "";

        ShowStep(1);
    }

    // =====================================================
    // PRODUCTION DETAILS
    // =====================================================

    private DataTable ProductionDetailTable
    {
        get
        {
            if (ViewState["ProductionDetailTable"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PRODUCT_ID");
                dt.Columns.Add("SIZE_ID");
                dt.Columns.Add("SWATCH_ID");
                dt.Columns.Add("QUANTITY");
                dt.Columns.Add("UNIT");
                dt.Rows.Add("", "", "", "", "");
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
                dt.Rows.Add("", "", "", "", "");

            dt.Rows[i]["PRODUCT_ID"] = GetText(row, "txtProductId");
            dt.Rows[i]["SIZE_ID"] = GetText(row, "txtSizeId");
            dt.Rows[i]["SWATCH_ID"] = GetText(row, "txtSwatchId");
            dt.Rows[i]["QUANTITY"] = GetText(row, "txtQuantity");
            dt.Rows[i]["UNIT"] = GetText(row, "txtUnit");
        }

        while (dt.Rows.Count > grdProductionDetails.Rows.Count)
            dt.Rows.RemoveAt(dt.Rows.Count - 1);

        ProductionDetailTable = dt;
    }

    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        ReadProductionDetails();

        DataTable dt = ProductionDetailTable;
        dt.Rows.Add("", "", "", "", "");

        ProductionDetailTable = dt;
        BindProductionDetails();
    }

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
            dt.Rows.Add("", "", "", "", "");

        ProductionDetailTable = dt;
        BindProductionDetails();
    }

    private bool ValidateProductionDetails()
    {
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

        if (string.IsNullOrWhiteSpace(txtMultipleDeliveryId.Text))
        {
            HelperFunction.MsgBox(this, GetType(), "Multiple Delivery ID cannot be empty.");
            return false;
        }

        return true;
    }

    // =====================================================
    // PRODUCTION STAGES
    // =====================================================

    private DataTable ProductionStageTable
    {
        get
        {
            if (ViewState["ProductionStageTable"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PRODUCTION_DETAIL_ID");
                dt.Columns.Add("STAGE_MASTER_ID");
                dt.Columns.Add("UNIT_OR_EMPLOYEE");
                dt.Columns.Add("STATUS");
                dt.Rows.Add("", "", "", "Pending");
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

    private void ReadProductionStages()
    {
        DataTable dt = ProductionStageTable;

        for (int i = 0; i < grdProductionStages.Rows.Count; i++)
        {
            GridViewRow row = grdProductionStages.Rows[i];

            if (i >= dt.Rows.Count)
                dt.Rows.Add("", "", "", "Pending");

            dt.Rows[i]["PRODUCTION_DETAIL_ID"] = GetText(row, "txtStageDetailId");
            dt.Rows[i]["STAGE_MASTER_ID"] = GetText(row, "txtStageMasterId");
            dt.Rows[i]["UNIT_OR_EMPLOYEE"] = GetSelectedValue(row, "ddlStageAssignmentType");
            dt.Rows[i]["STATUS"] = GetSelectedValue(row, "ddlStageStatus");
        }

        while (dt.Rows.Count > grdProductionStages.Rows.Count)
            dt.Rows.RemoveAt(dt.Rows.Count - 1);

        ProductionStageTable = dt;
    }

    protected void btnAddStage_Click(object sender, EventArgs e)
    {
        ReadProductionStages();

        DataTable dt = ProductionStageTable;
        dt.Rows.Add("", "", "", "Pending");

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
            dt.Rows.Add("", "", "", "Pending");

        ProductionStageTable = dt;
        BindProductionStages();
    }

    // =====================================================
    // PRODUCTION PIPELINE
    // =====================================================

    private DataTable ProductionPipelineTable
    {
        get
        {
            if (ViewState["ProductionPipelineTable"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PRODUCTION_DETAIL_ID");
                dt.Columns.Add("STAGE_ID");
                dt.Columns.Add("PRODUCTION_UNIT");
                dt.Columns.Add("PRODUCTION_DIVISION");
                dt.Columns.Add("EMPLOYEE_ID");
                dt.Columns.Add("ESTIMATED_DAY");
                dt.Columns.Add("ESTIMATED_TIME");
                dt.Columns.Add("PRODUCTION_DATE");
                dt.Columns.Add("PRODUCTION_TIME");
                dt.Columns.Add("COMPLETION_DATE");
                dt.Columns.Add("COMPLETION_TIME");
                dt.Columns.Add("STATUS");

                dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "Pending");
                ViewState["ProductionPipelineTable"] = dt;
            }

            return (DataTable)ViewState["ProductionPipelineTable"];
        }
        set
        {
            ViewState["ProductionPipelineTable"] = value;
        }
    }

    private void BindProductionPipeline()
    {
        grdProductionPipeline.DataSource = ProductionPipelineTable;
        grdProductionPipeline.DataBind();
    }

    private void ReadProductionPipeline()
    {
        DataTable dt = ProductionPipelineTable;

        for (int i = 0; i < grdProductionPipeline.Rows.Count; i++)
        {
            GridViewRow row = grdProductionPipeline.Rows[i];

            if (i >= dt.Rows.Count)
                dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "Pending");

            dt.Rows[i]["PRODUCTION_DETAIL_ID"] = GetText(row, "txtPipelineDetailId");
            dt.Rows[i]["STAGE_ID"] = GetText(row, "txtPipelineStageId");
            dt.Rows[i]["PRODUCTION_UNIT"] = GetText(row, "txtPipelineUnitId");
            dt.Rows[i]["PRODUCTION_DIVISION"] = GetText(row, "txtPipelineDivisionId");
            dt.Rows[i]["EMPLOYEE_ID"] = GetText(row, "txtPipelineEmployeeId");
            dt.Rows[i]["ESTIMATED_DAY"] = GetText(row, "txtEstimatedDay");
            dt.Rows[i]["ESTIMATED_TIME"] = GetText(row, "txtEstimatedTime");
            dt.Rows[i]["PRODUCTION_DATE"] = GetText(row, "txtPipelineProductionDate");
            dt.Rows[i]["PRODUCTION_TIME"] = GetText(row, "txtPipelineProductionTime");
            dt.Rows[i]["COMPLETION_DATE"] = GetText(row, "txtPipelineCompletionDate");
            dt.Rows[i]["COMPLETION_TIME"] = GetText(row, "txtPipelineCompletionTime");
            dt.Rows[i]["STATUS"] = GetSelectedValue(row, "ddlPipelineStatus");
        }

        while (dt.Rows.Count > grdProductionPipeline.Rows.Count)
            dt.Rows.RemoveAt(dt.Rows.Count - 1);

        ProductionPipelineTable = dt;
    }

    protected void btnAddPipeline_Click(object sender, EventArgs e)
    {
        ReadProductionPipeline();

        DataTable dt = ProductionPipelineTable;
        dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "Pending");

        ProductionPipelineTable = dt;
        BindProductionPipeline();
    }

    protected void grdProductionPipeline_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "RemovePipeline")
            return;

        ReadProductionPipeline();

        int index = Convert.ToInt32(e.CommandArgument);
        DataTable dt = ProductionPipelineTable;

        if (index >= 0 && index < dt.Rows.Count)
            dt.Rows.RemoveAt(index);

        if (dt.Rows.Count == 0)
            dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "Pending");

        ProductionPipelineTable = dt;
        BindProductionPipeline();
    }

    // =====================================================
    // GRID HELPERS
    // =====================================================

    private string GetText(GridViewRow row, string controlId)
    {
        TextBox txt = row.FindControl(controlId) as TextBox;
        return txt == null ? "" : txt.Text.Trim();
    }

    private string GetSelectedValue(GridViewRow row, string controlId)
    {
        DropDownList ddl = row.FindControl(controlId) as DropDownList;
        return ddl == null ? "" : ddl.SelectedValue;
    }

    // =====================================================
    // FINAL VALIDATION / CLEAR
    // =====================================================

    protected void btnSaveProduction_Click(object sender, EventArgs e)
    {
        ReadProductionDetails();
        ReadProductionStages();
        ReadProductionPipeline();

        if (!ValidateMaster() || !ValidateProductionDetails())
            return;

        DataTable stageTable = ProductionStageTable;

        foreach (DataRow row in stageTable.Rows)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(row["STAGE_MASTER_ID"])))
                continue;

            if (string.IsNullOrWhiteSpace(Convert.ToString(row["PRODUCTION_DETAIL_ID"]))
                || string.IsNullOrWhiteSpace(Convert.ToString(row["UNIT_OR_EMPLOYEE"])))
            {
                HelperFunction.MsgBox(this, GetType(), "Every configured stage requires a production detail and an assignment type.");
                return;
            }
        }

        DataTable pipelineTable = ProductionPipelineTable;

        foreach (DataRow row in pipelineTable.Rows)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(row["STAGE_ID"])))
                continue;

            if (string.IsNullOrWhiteSpace(Convert.ToString(row["PRODUCTION_DETAIL_ID"]))
                || string.IsNullOrWhiteSpace(Convert.ToString(row["ESTIMATED_DAY"]))
                || !IsValidDate(Convert.ToString(row["PRODUCTION_DATE"]))
                || !IsValidDate(Convert.ToString(row["COMPLETION_DATE"])))
            {
                HelperFunction.MsgBox(this, GetType(), "Complete the detail, estimated days, production date, and completion date for every pipeline row.");
                return;
            }
        }

        string[] productionDateParts = GetDateParts(txtProductionDate.Text);
        string[] completionDateParts = GetDateParts(txtCompletionDate.Text);

        /*
         * DATABASE SAVE GOES HERE.
         *
         * Master:
         * PRODUCTION_DAY = productionDateParts[0]
         * PRODUCTION_MONTH = productionDateParts[1]
         * PRODUCTION_YEAR = productionDateParts[2]
         *
         * COMPLITION_DAY = completionDateParts[0]
         * COMPLITION_MONTH = completionDateParts[1]
         * COMPLITION_YEAR = completionDateParts[2]
         *
         * Insert the master first and capture its PK_ID.
         * Then insert the production details using that ID.
         * Then insert stage assignments using the master/detail IDs.
         * Finally, insert the pipeline rows using the detail/stage IDs.
         *
         * Use one database transaction so partial production records
         * are not left behind if an insert fails.
         *
         * The actual Entity/Service insert calls must match the methods
         * exposed by your generated classes.
         */

        HelperFunction.MsgBox(this, GetType(), "Validation passed. Connect the database insert methods to save production.");
    }

    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        txtProductionDate.Text = GetTodayNepali();
        txtCompletionDate.Text = GetTodayNepali();
        txtProductionType.Text = "";
        txtCustomerPOId.Text = "";
        txtMultipleDeliveryId.Text = "";
        hfProductionId.Value = "";

        ViewState.Remove("ProductionDetailTable");
        ViewState.Remove("ProductionStageTable");
        ViewState.Remove("ProductionPipelineTable");

        BindProductionDetails();
        BindProductionStages();
        BindProductionPipeline();

        ShowStep(1);
    }
}