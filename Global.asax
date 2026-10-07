<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        RegisterRoutes(System.Web.Routing.RouteTable.Routes);
        System.Web.Http.GlobalConfiguration.Configure(SalesInvoiceApi.WebApiConfig.Register);
    }

    public static void RegisterRoutes(System.Web.Routing.RouteCollection routes)
    {
        // Maps the exact URL NCHL calls (no file extension) to the real
        // physical page. Works for POST as well as GET — PageRouteHandler
        // just resolves which .aspx to run; Page_Load still executes
        // normally and does its own check for the JSON-POST webhook call.
        routes.MapPageRoute(
            "NCHLConfirmPayment",
            "nepalpay/confirm-payment",
            "~/DYNAMICQR/DYNAMICQR_POS.aspx"
        );
    }

</script>
