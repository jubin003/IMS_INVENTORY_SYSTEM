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

            if (!string.IsNullOrWhiteSpace(txtProductionDate.Text))
            {
                string rawProdDate = txtProductionDate.Text.Trim();
                char[] delimiters = new char[] { '/', '-' };
                string[] prodParts = rawProdDate.Split(delimiters);

                if (prodParts.Length == 3)
                {
                    if (prodParts[0].Length == 4)
                    {
                        masterObj.PRODUCTION_YEAR = prodParts[0];
                        masterObj.PRODUCTION_MONTH = prodParts[1];
                        masterObj.PRODUCTION_DAY = prodParts[2];
                    }
                    else
                    {
                        masterObj.PRODUCTION_DAY = prodParts[0];
                        masterObj.PRODUCTION_MONTH = prodParts[1];
                        masterObj.PRODUCTION_YEAR = prodParts[2];
                    }
                }

                masterObj.PRODUCTION_DATE = ConvertDateToEnglish(rawProdDate);
            }

            if (!string.IsNullOrWhiteSpace(txtCompletionDate.Text))
            {
                string rawCompDate = txtCompletionDate.Text.Trim();
                char[] delimiters = new char[] { '/', '-' };
                string[] compParts = rawCompDate.Split(delimiters);

                if (compParts.Length == 3)
                {
                    if (compParts[0].Length == 4)
                    {
                        masterObj.COMPLITION_YEAR = compParts[0];
                        masterObj.COMPLITION_MONTH = compParts[1];
                        masterObj.COMPLITION_DAY = compParts[2];
                    }
                    else
                    {
                        masterObj.COMPLITION_DAY = compParts[0];
                        masterObj.COMPLITION_MONTH = compParts[1];
                        masterObj.COMPLITION_YEAR = compParts[2];
                    }
                }

                masterObj.COMPLITION_DATE = ConvertDateToEnglish(rawCompDate);
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

    private string ConvertDateToEnglish(string inputDate)
    {
        try
        {
            DateTime dt;
            if (DateTime.TryParse(inputDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            return inputDate;
        }
        catch
        {
            return inputDate;
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