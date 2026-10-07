using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;

public partial class Production_Customer_Location : System.Web.UI.Page
{
    CUSTOMER_LOCATION LEnt = new CUSTOMER_LOCATION();
    CUSTOMER_LOCATIONService LSer = new CUSTOMER_LOCATIONService();
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
        LEnt = new CUSTOMER_LOCATION();
        theList = LSer.GetAll(LEnt);

        if (theList.Count == 0)
            theList.Add(LEnt);

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

        if (lblPK_ID == null || string.IsNullOrEmpty(lblPK_ID.Text))
            return;

        LEnt = new CUSTOMER_LOCATION();
        LEnt.PK_ID = lblPK_ID.Text;
        LEnt = (CUSTOMER_LOCATION)LSer.GetSingle(LEnt);

        if (LEnt != null)
        {
            LEnt.CUSTOMER_ID = ddlCustomer.SelectedValue;
            LEnt.ADDRESS = txtAddress.Text;
            LEnt.COUNTRY_ID = ddlCountry.SelectedValue;
            LEnt.CONTACT_NUMBER = txtContactNumber.Text;
            LEnt.EMAIL_ID = txtEmail.Text;

            LSer.Update(LEnt);
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

                LEnt = new CUSTOMER_LOCATION();
                LEnt.PK_ID = lblPK_ID.Text;
                LEnt = (CUSTOMER_LOCATION)LSer.GetSingle(LEnt);

                if (ddlCustomer != null)
                {
                    LoadCustomer(ddlCustomer);

                    if (LEnt != null && ddlCustomer.Items.FindByValue(LEnt.CUSTOMER_ID) != null)
                        ddlCustomer.SelectedValue = LEnt.CUSTOMER_ID;
                }

                if (ddlCountry != null)
                {
                    LoadCountry(ddlCountry);

                    if (LEnt != null && ddlCountry.Items.FindByValue(LEnt.COUNTRY_ID) != null)
                        ddlCountry.SelectedValue = LEnt.COUNTRY_ID;
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

        if (string.IsNullOrEmpty(ddlCustomer.SelectedValue))
            HelperFunction.MsgBox(this, this.GetType(), "Customer can not be empty.");
        else if (string.IsNullOrEmpty(txtAddress.Text))
            HelperFunction.MsgBox(this, this.GetType(), "Address can not be empty.");
        else if (string.IsNullOrEmpty(ddlCountry.SelectedValue))
            HelperFunction.MsgBox(this, this.GetType(), "Country can not be empty.");
        else
        {
            LEnt = new CUSTOMER_LOCATION();
            LEnt.CUSTOMER_ID = ddlCustomer.SelectedValue;
            LEnt.ADDRESS = txtAddress.Text;
            LEnt.COUNTRY_ID = ddlCountry.SelectedValue;
            LEnt.CONTACT_NUMBER = txtContactNumber.Text;
            LEnt.EMAIL_ID = txtEmail.Text;

            LSer.Insert(LEnt);

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