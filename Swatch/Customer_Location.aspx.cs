using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Production_Customer_Location : System.Web.UI.Page
{
    PR_CUSTOMER_LOCATION CLEnt = new PR_CUSTOMER_LOCATION();

    PR_CUSTOMER_LOCATIONService CLSer = new PR_CUSTOMER_LOCATIONService();

    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();
    EntityList theList = new EntityList();

    COUNTRY CtyEnt = new COUNTRY();
    COUNTRYService CtySer = new COUNTRYService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            gridLoad();
    }

    protected void gridLoad()
    {
        CLEnt = new PR_CUSTOMER_LOCATION();
        theList = CLSer.GetAll(CLEnt);

        if (theList.Count == 0)
            theList.Add(CLEnt);

        gridDisplay.DataSource = theList;
        gridDisplay.DataBind();
    }

    protected void gridDisplay_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridDisplay.EditIndex = e.NewEditIndex;
        gridLoad();
    }

    protected void gridDisplay_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridDisplay.Rows[e.RowIndex];

        Label lblPK_ID = (Label)row.FindControl("lblPK_ID");
        DropDownList ddlCustomer = (DropDownList)row.FindControl("ddlCustomer");
        TextBox txtAddress = (TextBox)row.FindControl("txtAddressE");
        DropDownList ddlCountry = (DropDownList)row.FindControl("ddlCountry");
        TextBox txtContactNumber = (TextBox)row.FindControl("txtContactNumberE");
        TextBox txtEmail = (TextBox)row.FindControl("txtEmailE");
        TextBox txtContactPersonE = (TextBox)row.FindControl("txtContactPersonE");

        if (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text))
            return;

        CLEnt = new PR_CUSTOMER_LOCATION();
        CLEnt.PK_ID = lblPK_ID.Text;
        CLEnt = (PR_CUSTOMER_LOCATION)CLSer.GetSingle(CLEnt);

        if (CLEnt != null)
        {
            CLEnt.CUSTOMER_ID = ddlCustomer.SelectedValue;
            CLEnt.ADDRESS = txtAddress.Text;
            CLEnt.COUNTRY_ID = ddlCountry.SelectedValue;
            CLEnt.CONTACT_NUMBER = txtContactNumber.Text;
            CLEnt.EMAIL_ID = txtEmail.Text;
            CLEnt.CONTACT_PERSON = txtContactPersonE.Text;

            CLSer.Update(CLEnt);
        }

        gridDisplay.EditIndex = -1;
        gridLoad();
    }

    protected void gridDisplay_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridDisplay.EditIndex = -1;
        gridLoad();
    }

    protected void gridDisplay_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DropDownList ddlCustomerH = (DropDownList)e.Row.FindControl("ddlCustomerH");
            DropDownList ddlCountryH = (DropDownList)e.Row.FindControl("ddlCountryH");

            if (ddlCustomerH != null)
                LoadCustomer(ddlCustomerH);

            if (ddlCountryH != null)
                LoadCountry(ddlCountryH);
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                Label lblPK_ID = (Label)e.Row.FindControl("lblPK_ID");
                DropDownList ddlCustomer = (DropDownList)e.Row.FindControl("ddlCustomer");
                DropDownList ddlCountry = (DropDownList)e.Row.FindControl("ddlCountry");

                CLEnt = new PR_CUSTOMER_LOCATION();
                CLEnt.PK_ID = lblPK_ID.Text;
                CLEnt = (PR_CUSTOMER_LOCATION)CLSer.GetSingle(CLEnt);

                if (ddlCustomer != null)
                {
                    LoadCustomer(ddlCustomer);

                    if (CLEnt != null && ddlCustomer.Items.FindByValue(CLEnt.CUSTOMER_ID) != null)
                        ddlCustomer.SelectedValue = CLEnt.CUSTOMER_ID;
                }

                if (ddlCountry != null)
                {
                    LoadCountry(ddlCountry);

                    if (CLEnt != null && ddlCountry.Items.FindByValue(CLEnt.COUNTRY_ID) != null)
                        ddlCountry.SelectedValue = CLEnt.COUNTRY_ID;
                }
            }
            else
            {
                Label lblCustomer = (Label)e.Row.FindControl("lblCustomer");
                Label lblCountryID = (Label)e.Row.FindControl("lblCountryID");
                Label lblCountry = (Label)e.Row.FindControl("lblCountry");

                if (lblCustomer != null && !string.IsNullOrEmpty(lblCustomer.Text))
                {
                    CEnt = new CUSTOMER();
                    CEnt.PK_ID = lblCustomer.Text;
                    CEnt = (CUSTOMER)CSer.GetSingle(CEnt);

                    if (CEnt != null)
                        lblCustomer.Text = CEnt.CUSTOMER_NAME;
                }

                if (lblCountry != null && lblCountryID != null && !string.IsNullOrEmpty(lblCountryID.Text))
                {
                    CtyEnt = new COUNTRY();
                    CtyEnt.PK_ID = lblCountryID.Text;
                    CtyEnt = (COUNTRY)CtySer.GetSingle(CtyEnt);

                    if (CtyEnt != null)
                        lblCountry.Text = CtyEnt.COUNTRY_NAME;
                }
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridDisplay.HeaderRow;

        DropDownList ddlCustomer = (DropDownList)row.FindControl("ddlCustomerH");
        TextBox txtAddress = (TextBox)row.FindControl("txtAddress");
        DropDownList ddlCountry = (DropDownList)row.FindControl("ddlCountryH");
        TextBox txtContactNumber = (TextBox)row.FindControl("txtContactNumber");
        TextBox txtEmail = (TextBox)row.FindControl("txtEmail");
        TextBox txtContactPerson = (TextBox)row.FindControl("txtContactPerson");

        if (string.IsNullOrEmpty(ddlCustomer.SelectedValue))
            HelperFunction.MsgBox(this, this.GetType(), "Customer can not be empty.");
        else if (string.IsNullOrEmpty(txtAddress.Text))
            HelperFunction.MsgBox(this, this.GetType(), "Address can not be empty.");
        else if (string.IsNullOrEmpty(ddlCountry.SelectedValue))
            HelperFunction.MsgBox(this, this.GetType(), "Country can not be empty.");
        else
        {
            CLEnt = new PR_CUSTOMER_LOCATION();
            CLEnt.CUSTOMER_ID = ddlCustomer.SelectedValue;
            CLEnt.ADDRESS = txtAddress.Text;
            CLEnt.COUNTRY_ID = ddlCountry.SelectedValue;
            CLEnt.CONTACT_NUMBER = txtContactNumber.Text;
            CLEnt.EMAIL_ID = txtEmail.Text;
            CLEnt.CONTACT_PERSON = txtContactPerson.Text;

            CLSer.Insert(CLEnt);

            HelperFunction.MsgBox(this, this.GetType(), "Inserted");

            gridLoad();
        }
    }

    private void LoadCustomer(DropDownList ddlCustomer)
    {
        CEnt = new CUSTOMER();
        CEnt.STATUS = "1";

        ddlCustomer.DataSource = CSer.GetAll(CEnt);
        ddlCustomer.DataTextField = "CUSTOMER_NAME";
        ddlCustomer.DataValueField = "PK_ID";
        ddlCustomer.DataBind();
        ddlCustomer.Items.Insert(0, new ListItem("-- Select Customer --", ""));
    }

    private void LoadCountry(DropDownList ddlCountry)
    {
        CtyEnt = new COUNTRY();

        ddlCountry.DataSource = CtySer.GetAll(CtyEnt);
        ddlCountry.DataTextField = "COUNTRY_NAME";
        ddlCountry.DataValueField = "PK_ID";
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("-- Select Country --", ""));
    }
}