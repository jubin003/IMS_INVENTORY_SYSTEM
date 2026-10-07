using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;

public partial class Reports_Sales_PartyWiseSalesChallan : System.Web.UI.Page
{
    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    PhyeGan pg = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    HelperFunction hf = new HelperFunction();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCustomer();
            txtChalanDateFrom.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
            txtChalanDateTo.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
        }
    }
    protected void LoadCustomer()
    {
        CEnt = new CUSTOMER();
        ddlCustomerName.DataSource = CSer.GetAll(CEnt);
        ddlCustomerName.DataValueField = "PK_ID";
        ddlCustomerName.DataTextField = "CUSTOMER_NAME";
        ddlCustomerName.DataBind();
        ddlCustomerName.Items.Insert(0, "Select");
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        try
        {
            divhide.Visible = true;
            lblDate.Text = txtChalanDateFrom.Text;
            string[] chalandateFrom = txtChalanDateFrom.Text.Split('/');
            string challanDateFrom = PGD.ConvertNepaliTOEnglish(chalandateFrom[0], chalandateFrom[1], chalandateFrom[2]);
            string[] chalandateTo = txtChalanDateTo.Text.Split('/');
            string challanDateTo = PGD.ConvertNepaliTOEnglish(chalandateTo[0], chalandateTo[1], chalandateTo[2]);

            string customer = null;
            if (ddlCustomerName.SelectedValue != "Select")
                customer = ddlCustomerName.SelectedValue;
            grdReport.DataSource = hf.getSalesChallanReport(customer, challanDateFrom, challanDateTo);
            grdReport.DataBind();

            if (hf.ProductBatch() == "1")
                grdReport.Columns[2].Visible = true;
            else
                grdReport.Columns[2].Visible = false;

            if (hf.ProductExpiry() == "1")
                grdReport.Columns[3].Visible = true;
            else
                grdReport.Columns[3].Visible = false;

            if (hf.ProductColor() == "1")
                grdReport.Columns[4].Visible = true;
            else
                grdReport.Columns[4].Visible = false;

            if (hf.ProductSize() == "1")
                grdReport.Columns[5].Visible = true;
            else
                grdReport.Columns[5].Visible = false;
        }
        catch
        {
            HelperFunction.MsgBox(this, this.GetType(), "Invalid Date");
        }
    }

    protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblProductId = (Label)e.Row.FindControl("lblProductId");
            Label lblProductName = (Label)e.Row.FindControl("lblProductName");
            Label lblColourId = (Label)e.Row.FindControl("lblColourId");
            Label lblColourName = (Label)e.Row.FindControl("lblColourName");
            Label lblSizeId = (Label)e.Row.FindControl("lblSizeId");
            Label lblSizeName = (Label)e.Row.FindControl("lblSizeName");
            Label lblManufacturerId = (Label)e.Row.FindControl("lblManufacturerId");
            Label lblManufacturerName = (Label)e.Row.FindControl("lblManufacturerName");
            Label lblQuantity = (Label)e.Row.FindControl("lblQuantity");

            lblProductName.Text = hf.getProductName(lblProductId.Text);
            lblColourName.Text = hf.getColourName(lblColourId.Text);
            lblQuantity.Text = lblQuantity.Text + (hf.getProductUnit(lblProductId.Text));
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }
}