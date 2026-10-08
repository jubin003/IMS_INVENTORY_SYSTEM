using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Production_Purchase_Order : System.Web.UI.Page
{
    PR_PURCHASE_ORDER PoEnt = new PR_PURCHASE_ORDER();
    PR_PURCHASE_ORDERService PoSer = new PR_PURCHASE_ORDERService();

    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();
    EntityList theList = new EntityList();
    PhyeGanDate PGD = new PhyeGanDate();

    PR_CUSTOMER_LOCATION ClEnt = new PR_CUSTOMER_LOCATION();
    PR_CUSTOMER_LOCATIONService ClSer = new PR_CUSTOMER_LOCATIONService();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCustomer(ddlCustomer);
            gridLoad();
        }
    }

    protected void gridLoad()
    {
        PoEnt = new PR_PURCHASE_ORDER();
        theList = PoSer.GetAll(PoEnt);

        if (theList.Count == 0)
            theList.Add(PoEnt);

        gridDisplay.DataSource = theList;
        gridDisplay.DataBind();
    }

    protected void gridDisplay_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridDisplay.PageIndex = e.NewPageIndex;
        gridLoad();
    }

    protected void gridDisplay_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        Label lblPK_ID = (Label)e.Row.FindControl("lblPK_ID");
        Label lblStatus = (Label)e.Row.FindControl("lblStatus");
        Label lblStatusShow = (Label)e.Row.FindControl("lblStatusShow");
        Label lblCustomer = (Label)e.Row.FindControl("lblCustomer");
        ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");

        if (btnEdit != null && (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text)))
            btnEdit.Visible = false;

        if (lblStatus != null && lblStatusShow != null && !string.IsNullOrEmpty(lblStatus.Text))
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

        if (lblCustomer != null && !string.IsNullOrEmpty(lblCustomer.Text))
        {
            CEnt = new CUSTOMER();
            CEnt.PK_ID = lblCustomer.Text;
            CEnt = (CUSTOMER)CSer.GetSingle(CEnt);

            if (CEnt != null)
                lblCustomer.Text = CEnt.CUSTOMER_NAME;
        }
    }

    protected void gridDisplay_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "EditRow")
            return;

        string pkId = Convert.ToString(e.CommandArgument);
        if (string.IsNullOrEmpty(pkId))
            return;

        PoEnt = new PR_PURCHASE_ORDER();
        PoEnt.PK_ID = pkId;
        PoEnt = (PR_PURCHASE_ORDER)PoSer.GetSingle(PoEnt);

        if (PoEnt == null)
            return;

        hfPK_ID.Value = pkId;

        txtOrderNumber.Text = PoEnt.ORDER_NUMBER;
        txtOrderDate.Text = NepDate(PoEnt.ORDER_DAY, PoEnt.ORDER_MONTH, PoEnt.ORDER_YEAR);

        txtPaymentTerm.Text = PoEnt.PAYMENT_TERM;

        if (ddlCustomer.Items.FindByValue(PoEnt.CUSTOMER_ID) != null)
            ddlCustomer.SelectedValue = PoEnt.CUSTOMER_ID;
        else
            ddlCustomer.SelectedIndex = 0;

        if (ddlStatus.Items.FindByValue(PoEnt.STATUS) != null)
            ddlStatus.SelectedValue = PoEnt.STATUS;

        lblFormTitle.Text = "Edit Purchase Order";
        btnSave.Text = "Update";
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (string.IsNullOrWhiteSpace(txtOrderDate.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Order Date can not be empty.");
            return;
        }
        if (string.IsNullOrWhiteSpace(txtOrderNumber.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Order Number can not be empty.");
            return;
        }
        if (string.IsNullOrEmpty(ddlCustomer.SelectedValue))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Customer can not be empty.");
            return;
        }

        string[] nepDate = txtOrderDate.Text.Trim().Split('/');
        if (nepDate.Length != 3)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Order Date must be in dd/mm/yyyy format.");
            return;
        }

        bool isEdit = !string.IsNullOrEmpty(hfPK_ID.Value);

        PoEnt = new PR_PURCHASE_ORDER();

        if (isEdit)
        {
            PoEnt.PK_ID = hfPK_ID.Value;
            PoEnt = (PR_PURCHASE_ORDER)PoSer.GetSingle(PoEnt);

            if (PoEnt == null)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Record not found.");
                return;
            }
        }

        EntityList theList = new EntityList();
        theList = PoSer.GetAll(PoEnt);
        int id=0;
        foreach(PURCHASE_ORDER row in theList)
        {
            int.TryParse(row.PK_ID, out id);
        }
        id = id + 1;



        PoEnt.CUSTOMER_ORDER_NUMBER = id.ToString();
        PoEnt.ORDER_NUMBER = txtOrderNumber.Text.Trim();
        PoEnt.CUSTOMER_ID = ddlCustomer.SelectedValue;

        PoEnt.PAYMENT_TERM = txtPaymentTerm.Text.Trim();
        PoEnt.STATUS = ddlStatus.SelectedValue;

        PoEnt.ORDER_DATE = PGD.GetEnglishDateFromNepali(txtOrderDate.Text.Trim(), "dd/mm/yyyy");
        PoEnt.ORDER_DAY = nepDate[0];
        PoEnt.ORDER_MONTH = nepDate[1];
        PoEnt.ORDER_YEAR = nepDate[2];
        PoEnt.ORDER_FISCAL_YEAR = PGD.checkFiscalYear(nepDate[1], nepDate[2]).ToString();
        PoEnt.STATUS = ddlStatus.SelectedValue;

        if (isEdit)
        {
            PoSer.Update(PoEnt);
            HelperFunction.MsgBox(this, this.GetType(), "Updated");
        }
        else
        {
            PoSer.Insert(PoEnt);
            HelperFunction.MsgBox(this, this.GetType(), "Inserted");
        }

        ClearForm();
        gridLoad();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        hfPK_ID.Value = "";


        txtOrderDate.Text = "";
        txtOrderNumber.Text = "";
        txtPaymentTerm.Text = "";

        if (ddlCustomer.Items.Count > 0)
            ddlCustomer.SelectedIndex = 0;

        ddlStatus.SelectedValue = "1";

        lblFormTitle.Text = "Add Purchase Order";
        btnSave.Text = "+ Add";
    }


    private void LoadCustomer(DropDownList ddl)
    {
        CEnt = new CUSTOMER();
        CEnt.STATUS = "1";

        ddl.DataSource = CSer.GetAll(CEnt);
        ddl.DataTextField = "CUSTOMER_NAME";
        ddl.DataValueField = "PK_ID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-- Select Customer --", ""));
    }

    protected string NepDate(object day, object month, object year)
    {
        if (day == null || month == null || year == null) return "";

        string d = day.ToString().Trim();
        string m = month.ToString().Trim();
        string y = year.ToString().Trim();

        if (d == "" || m == "" || y == "") return "";

        return d.PadLeft(2, '0') + "/" + m.PadLeft(2, '0') + "/" + y;
    }

    protected void btnDeliNext_Click(object sender, EventArgs e)
    {
        
    }

    protected void btnOrdDet_Click(object sender, EventArgs e)
    {

    }
}