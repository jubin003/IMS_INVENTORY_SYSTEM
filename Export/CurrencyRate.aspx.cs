using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;

public partial class Export_CurrencyRate : System.Web.UI.Page
{
    FOREIGN_CURRENCY FCEnt = new FOREIGN_CURRENCY();
    FOREIGN_CURRENCYService FCSer = new FOREIGN_CURRENCYService();

    CURRENCY_RATE CREnt = new CURRENCY_RATE();
    CURRENCY_RATEService CRSer = new CURRENCY_RATEService();

    PhyeGanDate PGD = new PhyeGanDate();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            LoadCurrencyName();
            txtExchangeDate.Text = PGD.GetTodayNepaliDate();
            LoadGrid();
        }
    }

    protected void LoadCurrencyName()
    {
        FCEnt = new FOREIGN_CURRENCY();
        FCEnt.STATUS = "1";
        ddlCurrency.DataSource = FCSer.GetAll(FCEnt);
        ddlCurrency.DataTextField = "CURRENCY_NAME";
        ddlCurrency.DataValueField = "PK_ID";
        ddlCurrency.DataBind();
        ddlCurrency.Items.Insert(0, "Select");
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        if(ddlCurrency.SelectedItem.Text == "Select")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Select Currency");
            return;
        }
        CREnt = new CURRENCY_RATE();
        CREnt.CURRENCY_TYPE_ID = ddlCurrency.SelectedValue;
        CREnt.CONVERSION_DATE = PGD.GetTodayDate("dd/mm/yyyy");
        CREnt = (CURRENCY_RATE)CRSer.GetSingle(CREnt);
        if (CREnt != null)
        {
            CREnt.EXCHANGE_RATE = txtExchangeRate.Text;
            CRSer.Update(CREnt);
            HelperFunction.MsgBox(this, this.GetType(), "Rate updated.");
        }
        else
        {
            CREnt = new CURRENCY_RATE();
            CREnt.CURRENCY_TYPE_ID = ddlCurrency.SelectedValue;
            CREnt.CONVERSION_DATE = PGD.GetTodayDate("dd/mm/yyyy");
            CREnt.EXCHANGE_RATE = txtExchangeRate.Text;
            CRSer.Insert(CREnt);
            HelperFunction.MsgBox(this, this.GetType(), "Rate inserted.");
        }

        ClearField();
        LoadGrid();
    }

    protected void ClearField()
    {
        ddlCurrency.SelectedIndex = 0;
        txtExchangeRate.Text = "";
    }

    private void LoadGrid()
    {
        CREnt = new CURRENCY_RATE();
        CREnt.CONVERSION_DATE = PGD.GetTodayDate("dd/mm/yyyy");
        gvTodayRates.DataSource = CRSer.GetAll(CREnt);
        gvTodayRates.DataBind();
    }

    protected void gvTodayRates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblCurrency = e.Row.FindControl("lblCurrency") as Label;
            Label lblCurrencyName = e.Row.FindControl("lblCurrencyName") as Label;

            FCEnt = new FOREIGN_CURRENCY();
            FCEnt.PK_ID = lblCurrency.Text;
            FCEnt = (FOREIGN_CURRENCY)FCSer.GetSingle(FCEnt);
            if (FCEnt != null)
            {
                lblCurrencyName.Text = FCEnt.CURRENCY_NAME;
            }
        }
    }
}