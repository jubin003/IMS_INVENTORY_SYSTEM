using System.Web.Http;

namespace SalesInvoiceApi
{
    // No OWIN/OAuth here. Register this the classic way from your host
    // project's Global.asax Application_Start:
    //
    //     GlobalConfiguration.Configure(SalesInvoiceApi.WebApiConfig.Register);
    //
    // That's the only wiring this module needs. See README.md for the full
    // drop-in checklist.
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "SalesInvoiceApiDefault",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
