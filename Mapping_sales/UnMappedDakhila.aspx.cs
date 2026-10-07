using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;

public partial class Mapping_sales_UnMappedDakhila : System.Web.UI.Page
{
    UserProfileEntity userProfileEnt = new UserProfileEntity();

    HelperFunction hf = new HelperFunction();

    PhyeGan pg = new PhyeGan();

    PhyeGanDate PGD = new PhyeGanDate();

    static string path = "";

    Boolean IsPageRefresh = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            grdUnMappedDakhila.DataSource = hf.LoadUnMappedDakhila();
            grdUnMappedDakhila.DataBind();
        }

    }

    protected void lblDakhilaNo_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((LinkButton)sender).Parent.Parent as GridViewRow;

        LinkButton lblDakhilaNumber = gr.FindControl("lblDakhilaNumber") as LinkButton;
        Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
        string url = "~/reports/purchase/ShowPurchaseInvoice.aspx?dakhno=" + lblPK_ID.Text;
        string fullUrl = ResolveUrl(url);

        // Register JavaScript to open the URL in a new tab
        string script = "window.open('" + fullUrl + "', '_blank');";
        ScriptManager.RegisterStartupScript(this, GetType(), "OpenInvoice", script, true);

    }
}