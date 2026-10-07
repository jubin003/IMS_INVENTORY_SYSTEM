<%@ WebHandler Language="C#" Class="PosConfig" %>
using System;
using System.Web;
using Newtonsoft.Json.Linq;
using Entity.Components;
using Entity.Framework;
using Service.Components;
// Serves POS connection config (API key + base URL) to the cashier's OWN
// browser, read from the DYNAMIC_QR table in the database instead of
// web.config — so nothing is hardcoded in the JS file and settings can be
// changed without redeploying.
//
// The key still travels to the local NiziPOS service in plain sight of
// DevTools once the browser uses it — that's unavoidable for a client-side
// call. This app has no login system, so this endpoint is open to anyone
// who can reach it on the network — there's no way to restrict it further
// without adding authentication to the app.
public class PosConfig : IHttpHandler
{
    public bool IsReusable
    {
        get { return false; }
    }
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        var json = new JObject();
        try
        {
            DYNAMIC_QRService DQSer = new DYNAMIC_QRService();
            DYNAMIC_QR DQEnt = new DYNAMIC_QR();
            EntityList settingsList = DQSer.GetAll(DQEnt);

            if (settingsList == null || settingsList.Count == 0)
                throw new InvalidOperationException("DYNAMIC_QR settings row not found in database.");

            DQEnt = (DYNAMIC_QR)settingsList[0];

            string apiKey = DQEnt.POS_API_KEY;
            string baseUrl = DQEnt.POS_BASE_URL;

            // Falls back to the standard local NiziPOS address if POS_BASE_URL
            // isn't set in the DB. It needs to resolve correctly on EVERY
            // cashier's machine, not just one, so keep it as 127.0.0.1:9121
            // (or whatever port every terminal's NiziPOS service listens on).
            if (string.IsNullOrEmpty(baseUrl))
                baseUrl = "http://127.0.0.1:9121";

            json["apiKey"] = apiKey;
            json["baseUrl"] = baseUrl;
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            json["error"] = "Could not load POS settings: " + ex.Message;
        }

        context.Response.Write(json.ToString());
    }
}
