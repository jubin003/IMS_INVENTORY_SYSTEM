using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;
using System.Globalization;

public partial class ProductionMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdowns();
        }
    }

    private void LoadDropdowns()
    {
        try
        {
            PR_PURCHASE_ORDERService poService = new PR_PURCHASE_ORDERService();
            EntityList poList = (EntityList)poService.GetAll(new PR_PURCHASE_ORDER());

            ddlCustomerPO.DataSource = poList;
            //ddlCustomerPO.DataTextField = "PO_NUMBER";
            ddlCustomerPO.DataTextField = "CUSTOMER_ORDER_NUMBER";
            ddlCustomerPO.DataValueField = "PK_ID";
            ddlCustomerPO.DataBind();
            ddlCustomerPO.Items.Insert(0, new ListItem("-- Select PO --", ""));
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading form data: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    protected void btnSaveMaster_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtProductionNo.Text) || string.IsNullOrEmpty(ddlProductionType.SelectedValue))
        {
            lblMessage.Text = "Production number and type are required.";
            lblMessage.CssClass = "text-danger";
            return;
        }

        try
        {
            PR_PRODUCTION_MASTERService masterService = new PR_PRODUCTION_MASTERService();
            PR_PRODUCTION_MASTER masterObj = new PR_PRODUCTION_MASTER();

            masterObj.PRODUCTION_NUMBER = txtProductionNo.Text.Trim();
            masterObj.PRODUCTION_TYPE = ddlProductionType.SelectedValue;
            masterObj.CUSTOMER_PO_ID = ddlCustomerPO.SelectedValue;
            masterObj.STATUS = ddlStatus.SelectedValue;

            // Force English (Gregorian) parsing and formatting using InvariantCulture
            DateTime prodDate;
            if (DateTime.TryParse(txtProductionDate.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out prodDate))
            {
                masterObj.PRODUCTION_DATE = prodDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                masterObj.PRODUCTION_DAY = prodDate.Day.ToString(CultureInfo.InvariantCulture);
                masterObj.PRODUCTION_MONTH = prodDate.Month.ToString(CultureInfo.InvariantCulture);
                masterObj.PRODUCTION_YEAR = prodDate.Year.ToString(CultureInfo.InvariantCulture);
            }

            DateTime compDate;
            if (DateTime.TryParse(txtCompletionDate.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out compDate))
            {
                masterObj.COMPLITION_DATE = compDate.ToString("dd-MM-yy", CultureInfo.InvariantCulture);
                masterObj.COMPLITION_DAY = compDate.Day.ToString(CultureInfo.InvariantCulture);
                masterObj.COMPLITION_MONTH = compDate.Month.ToString(CultureInfo.InvariantCulture);
                masterObj.COMPLITION_YEAR = compDate.Year.ToString(CultureInfo.InvariantCulture);
            }

            masterService.Insert(masterObj);

            ResetForm();
            lblMessage.Text = "Production Master saved successfully.";
            lblMessage.CssClass = "text-success status-active";
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error saving data: " + ex.Message;
            lblMessage.CssClass = "text-danger status-inactive";
        }
    }

    private void ResetForm()
    {
        txtProductionNo.Text = "";
        ddlProductionType.SelectedIndex = 0;
        ddlCustomerPO.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        txtProductionDate.Text = "";
        txtCompletionDate.Text = "";
    }
}