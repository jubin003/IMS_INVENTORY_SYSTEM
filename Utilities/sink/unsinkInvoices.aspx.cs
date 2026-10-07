using Entity.Components;
using Service.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Utilities_sink_unsinkInvoices : System.Web.UI.Page
{
    SALES_INVOICE_MASTER SIMEnt = new SALES_INVOICE_MASTER();
    SALES_INVOICE_MASTERService SIMSer = new SALES_INVOICE_MASTERService();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSink_Click(object sender, EventArgs e)
    {
        SIMEnt = new SALES_INVOICE_MASTER();

    }
}