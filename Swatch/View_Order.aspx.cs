using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Swatch_View_Order : System.Web.UI.Page
{
    PR_PURCHASE_ORDERService poService = new PR_PURCHASE_ORDERService();
    PR_MULTIPLE_DELIVERYService deliveryService = new PR_MULTIPLE_DELIVERYService();
    PR_ORDER_DETAILService orderDetailService = new PR_ORDER_DETAILService();
    PR_CUSTOMER_LOCATIONService locationService = new PR_CUSTOMER_LOCATIONService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadPurchaseOrders();
        }
    }

    private void LoadPurchaseOrders()
    {
        PR_PURCHASE_ORDER filter = new PR_PURCHASE_ORDER();
        EntityList poList = (EntityList)poService.GetAll(filter);

        gridDisplay.DataSource = poList;
        gridDisplay.DataBind();
    }

    protected void gridDisplay_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridDisplay.PageIndex = e.NewPageIndex;
        LoadPurchaseOrders();
    }

    protected void gridDisplay_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            Label lblStatusShow = (Label)e.Row.FindControl("lblStatusShow");

            if (lblStatus != null && lblStatusShow != null)
            {
                if (lblStatus.Text == "1")
                {
                    lblStatusShow.Text = "Active";
                    lblStatusShow.CssClass = "status-pill status-active";
                }
                else
                {
                    lblStatusShow.Text = "Inactive";
                    lblStatusShow.CssClass = "status-pill status-inactive";
                }
            }
        }
    }

    protected void gridDisplay_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewOrder")
        {
            string purchaseOrderId = e.CommandArgument.ToString();
            LoadOrderDetails(purchaseOrderId);
        }
    }

    private void LoadOrderDetails(string poId)
    {
        PR_PURCHASE_ORDER poFilter = new PR_PURCHASE_ORDER();
        poFilter.PK_ID = poId;
        PR_PURCHASE_ORDER po = (PR_PURCHASE_ORDER)poService.GetSingle(poFilter);

        if (po == null) return;

        // Card 1: Master Order Header Details
        lblDetOrderNo.Text = po.ORDER_NUMBER;
        lblDetCustOrderNo.Text = po.CUSTOMER_ORDER_NUMBER;
        lblDetOrderDate.Text = NepDate(po.ORDER_DAY, po.ORDER_MONTH, po.ORDER_YEAR);
        lblDetCustomer.Text = po.CUSTOMER_ID;
        lblDetPaymentTerm.Text = po.PAYMENT_TERM;
        lblDetFiscalYear.Text = po.ORDER_FISCAL_YEAR;
        lblDetStatus.Text = po.STATUS == "1" ? "Active" : "Inactive";

        // Query Multiple Delivery Dates for this PO
        PR_MULTIPLE_DELIVERY delFilter = new PR_MULTIPLE_DELIVERY();
        delFilter.PURCHASE_ORDER_ID = poId;
        EntityList deliveries = (EntityList)deliveryService.GetAll(delFilter);

        // Card 2: Summary Overview Grid
        DataTable dtDeliverySummary = new DataTable();
        dtDeliverySummary.Columns.Add("DELIVERY_DATE", typeof(string));
        dtDeliverySummary.Columns.Add("MODE_OF_TRANSPORTATION", typeof(string));
        dtDeliverySummary.Columns.Add("LOCATION_ADDRESS", typeof(string));
        dtDeliverySummary.Columns.Add("REMARKS", typeof(string));
        dtDeliverySummary.Columns.Add("STATUS_LABEL", typeof(string));

        if (deliveries != null)
        {
            for (int i = 0; i < deliveries.Count; i++)
            {
                PR_MULTIPLE_DELIVERY del = (PR_MULTIPLE_DELIVERY)deliveries[i];
                DataRow dr = dtDeliverySummary.NewRow();
                dr["DELIVERY_DATE"] = !string.IsNullOrEmpty(del.DELIVERY_DATE) ? del.DELIVERY_DATE : NepDate(del.DELIVERY_DAY, del.DELIVERY_MONTH, del.DELIVERY_YEAR);
                dr["MODE_OF_TRANSPORTATION"] = !string.IsNullOrEmpty(del.MODE_OF_TRANSPORTATION_ID) ? del.MODE_OF_TRANSPORTATION_ID : "-";
                dr["REMARKS"] = !string.IsNullOrEmpty(del.REMARKS) ? del.REMARKS : "-";
                dr["STATUS_LABEL"] = del.STATUS == "1" ? "Active" : "Inactive";

                string locAddress = "-";
                if (!string.IsNullOrEmpty(del.CUSOMTER_LOCATION_ID))
                {
                    PR_CUSTOMER_LOCATION locFilter = new PR_CUSTOMER_LOCATION();
                    locFilter.PK_ID = del.CUSOMTER_LOCATION_ID;
                    PR_CUSTOMER_LOCATION loc = (PR_CUSTOMER_LOCATION)locationService.GetSingle(locFilter);
                    if (loc != null && !string.IsNullOrEmpty(loc.ADDRESS)) locAddress = loc.ADDRESS;
                }
                dr["LOCATION_ADDRESS"] = locAddress;

                dtDeliverySummary.Rows.Add(dr);
            }
        }
        gridDeliverySummary.DataSource = dtDeliverySummary;
        gridDeliverySummary.DataBind();

        // Card 3: Repeater for each delivery with its location and product grid
        rptDeliveries.DataSource = deliveries;
        rptDeliveries.DataBind();

        // Switch Panels
        pnlOrderList.Visible = false;
        pnlOrderDetail.Visible = true;
    }

    protected void rptDeliveries_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            PR_MULTIPLE_DELIVERY delivery = (PR_MULTIPLE_DELIVERY)e.Item.DataItem;

            Label lblDelAddress = (Label)e.Item.FindControl("lblDelAddress");
            Label lblDelContactPerson = (Label)e.Item.FindControl("lblDelContactPerson");
            Label lblDelContactNumber = (Label)e.Item.FindControl("lblDelContactNumber");
            Label lblDelEmail = (Label)e.Item.FindControl("lblDelEmail");
            Label lblDelCountry = (Label)e.Item.FindControl("lblDelCountry");
            Label lblDelTransport = (Label)e.Item.FindControl("lblDelTransport");
            Label lblDelStatus = (Label)e.Item.FindControl("lblDelStatus");

            if (lblDelTransport != null) lblDelTransport.Text = !string.IsNullOrEmpty(delivery.MODE_OF_TRANSPORTATION_ID) ? delivery.MODE_OF_TRANSPORTATION_ID : "-";
            if (lblDelStatus != null) lblDelStatus.Text = delivery.STATUS == "1" ? "Active" : "Inactive";

            // Resolve Customer Location Information
            if (!string.IsNullOrEmpty(delivery.CUSOMTER_LOCATION_ID))
            {
                PR_CUSTOMER_LOCATION locFilter = new PR_CUSTOMER_LOCATION();
                locFilter.PK_ID = delivery.CUSOMTER_LOCATION_ID;
                PR_CUSTOMER_LOCATION loc = (PR_CUSTOMER_LOCATION)locationService.GetSingle(locFilter);

                if (loc != null)
                {
                    if (lblDelAddress != null) lblDelAddress.Text = loc.ADDRESS;
                    if (lblDelContactPerson != null) lblDelContactPerson.Text = loc.CONTACT_PERSON;
                    if (lblDelContactNumber != null) lblDelContactNumber.Text = loc.CONTACT_NUMBER;
                    if (lblDelEmail != null) lblDelEmail.Text = !string.IsNullOrEmpty(loc.EMAIL_ID) ? loc.EMAIL_ID : "-";
                    if (lblDelCountry != null) lblDelCountry.Text = !string.IsNullOrEmpty(loc.COUNTRY_ID) ? loc.COUNTRY_ID : "-";
                }
            }
            else
            {
                if (lblDelAddress != null) lblDelAddress.Text = "-";
                if (lblDelContactPerson != null) lblDelContactPerson.Text = "-";
                if (lblDelContactNumber != null) lblDelContactNumber.Text = "-";
                if (lblDelEmail != null) lblDelEmail.Text = "-";
                if (lblDelCountry != null) lblDelCountry.Text = "-";
            }

            // Products Grid for this specific delivery
            GridView gridOrderProducts = (GridView)e.Item.FindControl("gridOrderProducts");
            if (gridOrderProducts != null)
            {
                PR_ORDER_DETAIL detFilter = new PR_ORDER_DETAIL();
                detFilter.MULTIPLE_DELVIERY_ID = delivery.PK_ID;
                EntityList orderItems = (EntityList)orderDetailService.GetAll(detFilter);

                DataTable dtProducts = new DataTable();
                dtProducts.Columns.Add("PRODUCT_NAME", typeof(string));
                dtProducts.Columns.Add("SIZE_NAME", typeof(string));
                dtProducts.Columns.Add("SWATCH_NAME", typeof(string));
                dtProducts.Columns.Add("UNIT", typeof(string));
                dtProducts.Columns.Add("QUANTITY", typeof(string));

                PRODUCTService prodService = new PRODUCTService();
                PR_SWATCHService swatchService = new PR_SWATCHService();

                if (orderItems != null)
                {
                    for (int i = 0; i < orderItems.Count; i++)
                    {
                        PR_ORDER_DETAIL item = (PR_ORDER_DETAIL)orderItems[i];
                        DataRow dr = dtProducts.NewRow();

                        PRODUCT pFilter = new PRODUCT();
                        pFilter.PK_ID = item.PRODUCT_ID;
                        PRODUCT p = (PRODUCT)prodService.GetSingle(pFilter);
                        dr["PRODUCT_NAME"] = p != null ? p.PRODUCT_NAME : item.PRODUCT_ID;

                        if (!string.IsNullOrEmpty(item.SWATCH_ID))
                        {
                            PR_SWATCH sFilter = new PR_SWATCH();
                            sFilter.PK_ID = item.SWATCH_ID;
                            PR_SWATCH s = (PR_SWATCH)swatchService.GetSingle(sFilter);
                            dr["SWATCH_NAME"] = s != null ? s.SWATCH_NAME : item.SWATCH_ID;
                        }
                        else
                        {
                            dr["SWATCH_NAME"] = "-";
                        }

                        dr["SIZE_NAME"] = !string.IsNullOrEmpty(item.SIZE_ID) ? item.SIZE_ID : "-";
                        dr["UNIT"] = !string.IsNullOrEmpty(item.UNIT) ? item.UNIT : "-";
                        dr["QUANTITY"] = item.QUANTITY;

                        dtProducts.Rows.Add(dr);
                    }
                }

                gridOrderProducts.DataSource = dtProducts;
                gridOrderProducts.DataBind();
            }
        }
    }

    protected void btnBackToList_Click(object sender, EventArgs e)
    {
        pnlOrderDetail.Visible = false;
        pnlOrderList.Visible = true;
        LoadPurchaseOrders();
    }

    public string NepDate(object day, object month, object year)
    {
        if (day == null || month == null || year == null) return string.Empty;
        string d = day.ToString().Trim();
        string m = month.ToString().Trim();
        string y = year.ToString().Trim();

        if (string.IsNullOrEmpty(y) && string.IsNullOrEmpty(m) && string.IsNullOrEmpty(d)) return string.Empty;
        return string.Format("{0}/{1}/{2}", y, m, d);
    }
}