using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Swatch_View_Order : System.Web.UI.Page
{
    PR_PURCHASE_ORDER PoEnt = new PR_PURCHASE_ORDER();
    PR_PURCHASE_ORDERService PoSer = new PR_PURCHASE_ORDERService();

    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    EntityList theList = new EntityList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gridLoad();
        }
    }


    // ================================================================
    // LOAD PURCHASE ORDERS
    // ================================================================

    protected void gridLoad()
    {
        PoEnt = new PR_PURCHASE_ORDER();

        theList = PoSer.GetAll(PoEnt);

        if (theList.Count == 0)
            theList.Add(PoEnt);

        gridDisplay.DataSource = theList;
        gridDisplay.DataBind();
    }


    // ================================================================
    // PAGING
    // ================================================================

    protected void gridDisplay_PageIndexChanging(
        object sender,
        GridViewPageEventArgs e)
    {
        gridDisplay.PageIndex = e.NewPageIndex;

        gridLoad();
    }


    // ================================================================
    // ROW DATA BOUND
    // ================================================================

    protected void gridDisplay_RowDataBound(
        object sender,
        GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;


        Label lblPK_ID =
            (Label)e.Row.FindControl("lblPK_ID");

        Label lblStatus =
            (Label)e.Row.FindControl("lblStatus");

        Label lblStatusShow =
            (Label)e.Row.FindControl("lblStatusShow");

        Label lblCustomer =
            (Label)e.Row.FindControl("lblCustomer");

        ImageButton btnEdit =
            (ImageButton)e.Row.FindControl("btnEdit");


        // ------------------------------------------------------------
        // EDIT BUTTON
        // ------------------------------------------------------------

        if (btnEdit != null &&
            (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text)))
        {
            btnEdit.Visible = false;
        }


        // ------------------------------------------------------------
        // STATUS
        // ------------------------------------------------------------

        if (lblStatus != null &&
            lblStatusShow != null &&
            !string.IsNullOrEmpty(lblStatus.Text))
        {
            if (lblStatus.Text == "1")
            {
                lblStatusShow.Text = "Active";
                lblStatusShow.CssClass =
                    "status-pill status-active";
            }
            else
            {
                lblStatusShow.Text = "Inactive";
                lblStatusShow.CssClass =
                    "status-pill status-inactive";
            }
        }


        // ------------------------------------------------------------
        // CUSTOMER NAME
        // ------------------------------------------------------------

        if (lblCustomer != null &&
            !string.IsNullOrEmpty(lblCustomer.Text))
        {
            CEnt = new CUSTOMER();

            CEnt.PK_ID = lblCustomer.Text;

            CEnt = (CUSTOMER)CSer.GetSingle(CEnt);

            if (CEnt != null)
            {
                lblCustomer.Text = CEnt.CUSTOMER_NAME;
            }
        }
    }


    // ================================================================
    // ROW COMMAND
    // ================================================================

    protected void gridDisplay_RowCommand(
        object sender,
        GridViewCommandEventArgs e)
    {
        if (e.CommandName != "EditRow")
            return;


        string pkId =
            Convert.ToString(e.CommandArgument);

        if (string.IsNullOrEmpty(pkId))
            return;


        // ------------------------------------------------------------
        // For now, send the user to the Create/Edit page.
        // ------------------------------------------------------------

        Response.Redirect(
            "Create_Order.aspx?PK_ID=" + Server.UrlEncode(pkId)
        );
    }


    // ================================================================
    // NEPALI DATE
    // ================================================================

    protected string NepDate(
        object day,
        object month,
        object year)
    {
        if (day == null ||
            month == null ||
            year == null)
        {
            return "";
        }


        string d = day.ToString().Trim();
        string m = month.ToString().Trim();
        string y = year.ToString().Trim();


        if (d == "" ||
            m == "" ||
            y == "")
        {
            return "";
        }


        return d.PadLeft(2, '0')
            + "/"
            + m.PadLeft(2, '0')
            + "/"
            + y;
    }
}
