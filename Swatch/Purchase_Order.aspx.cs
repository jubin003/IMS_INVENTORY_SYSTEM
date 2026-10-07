using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Production_Purchase_Order : System.Web.UI.Page
{
    PURCHASE_ORDER PEnt = new PURCHASE_ORDER();
    PURCHASE_ORDERService PSer = new PURCHASE_ORDERService();
    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();
    EntityList theList = new EntityList();
    PhyeGanDate PGD = new PhyeGanDate();

    CUSTOMER_LOCATION ClEnt = new CUSTOMER_LOCATION();
    CUSTOMER_LOCATIONService ClSer = new CUSTOMER_LOCATIONService();


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
        PEnt = new PURCHASE_ORDER();
        theList = PSer.GetAll(PEnt);

        if (theList.Count == 0)
            theList.Add(PEnt);

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

        PEnt = new PURCHASE_ORDER();
        PEnt.PK_ID = pkId;
        PEnt = (PURCHASE_ORDER)PSer.GetSingle(PEnt);

        if (PEnt == null)
            return;

        hfPK_ID.Value = pkId;

        txtOrderNumber.Text = PEnt.ORDER_NUMBER;
        txtOrderDate.Text = NepDate(PEnt.ORDER_DAY, PEnt.ORDER_MONTH, PEnt.ORDER_YEAR);
        txtDispatchedDate.Text = NepDate(PEnt.DISPATCHED_DAY, PEnt.DISPATCHED_MONTH, PEnt.DISPATCHED_YEAR);
        txtModeOfTransportationID.Text = PEnt.MODE_OF_TRANSPORTATION_ID;

        txtDispatchedLocation.Text = PEnt.DISPATCHED_LOCATION;
        txtPaymentTerm.Text = PEnt.PAYMENT_TERM;

        if (ddlCustomer.Items.FindByValue(PEnt.CUSTOMER_ID) != null)
            ddlCustomer.SelectedValue = PEnt.CUSTOMER_ID;
        else
            ddlCustomer.SelectedIndex = 0;

        if (ddlStatus.Items.FindByValue(PEnt.STATUS) != null)
            ddlStatus.SelectedValue = PEnt.STATUS;

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

        string[] nepDateDis = null;
        if (!string.IsNullOrWhiteSpace(txtDispatchedDate.Text))
        {
            nepDateDis = txtDispatchedDate.Text.Trim().Split('/');
            if (nepDateDis.Length != 3)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Dispatched Date must be in dd/mm/yyyy format.");
                return;
            }
        }

        bool isEdit = !string.IsNullOrEmpty(hfPK_ID.Value);

        PEnt = new PURCHASE_ORDER();

        if (isEdit)
        {
            PEnt.PK_ID = hfPK_ID.Value;
            PEnt = (PURCHASE_ORDER)PSer.GetSingle(PEnt);

            if (PEnt == null)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Record not found.");
                return;
            }
        }

        EntityList theList = new EntityList();
        theList = PSer.GetAll(PEnt);
        int id=0;
        foreach(PURCHASE_ORDER row in theList)
        {
            int.TryParse(row.PK_ID, out id);
        }
        id = id + 1;



        PEnt.CUSTOMER_ORDER_NUMBER = id.ToString();
        PEnt.ORDER_NUMBER = txtOrderNumber.Text.Trim();
        PEnt.CUSTOMER_ID = ddlCustomer.SelectedValue;
        PEnt.MODE_OF_TRANSPORTATION_ID = txtModeOfTransportationID.Text.Trim();
        PEnt.DISPATCHED_LOCATION = txtDispatchedLocation.Text.Trim();
        PEnt.PAYMENT_TERM = txtPaymentTerm.Text.Trim();
        PEnt.STATUS = ddlStatus.SelectedValue;

        PEnt.ORDER_DATE = PGD.GetEnglishDateFromNepali(txtOrderDate.Text.Trim(), "dd/mm/yyyy");
        PEnt.ORDER_DAY = nepDate[0];
        PEnt.ORDER_MONTH = nepDate[1];
        PEnt.ORDER_YEAR = nepDate[2];
        PEnt.ORDER_FISCAL_YEAR = PGD.checkFiscalYear(nepDate[1], nepDate[2]).ToString();

        if (nepDateDis != null)
        {
            PEnt.DISPATCHED_DATE = PGD.GetEnglishDateFromNepali(txtDispatchedDate.Text.Trim(), "dd/mm/yyyy");
            PEnt.DISPATCHED_DAY = nepDateDis[0];
            PEnt.DISPATCHED_MONTH = nepDateDis[1];
            PEnt.DISPATCHED_YEAR = nepDateDis[2];
        }

        if (isEdit)
        {
            PSer.Update(PEnt);
            HelperFunction.MsgBox(this, this.GetType(), "Updated");
        }
        else
        {
            PSer.Insert(PEnt);
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
        txtDispatchedDate.Text = "";
        txtModeOfTransportationID.Text = "";
        txtDispatchedLocation.Text = "";
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
}