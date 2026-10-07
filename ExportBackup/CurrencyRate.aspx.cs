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
            lblTodaysExchangeDate.Text = PGD.GetTodayDate("dd/mm/yyyy");
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
        if(ddlCurrency.SelectedItem.Text=="Select")
        {
            HelperFunction.MsgBox(this, this.GetType(), "Choose the Currency");
            return;
        }
        string[] parts = txtExchangeDate.Text.Split('/');
        string engDate = PGD.ConvertNepaliTOEnglish(parts[0], parts[1], parts[2]);
        DateTime dt = DateTime.ParseExact(engDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        string oracleDate = dt.ToString("MM/dd/yyyy");

        CURRENCY_RATE search = new CURRENCY_RATE();
        search.CURRENCY_TYPE_ID = ddlCurrency.SelectedValue;
        search.CONVERSION_DATE = oracleDate;

        CURRENCY_RATE existing = (CURRENCY_RATE)CRSer.GetSingle(search);

        if (existing != null)
        {
            existing.EXCHANGE_RATE = txtExchangeRate.Text;
            CRSer.Update(existing);
            HelperFunction.MsgBox(this, this.GetType(), "Rate updated.");
        }
        else
        {
            search.EXCHANGE_RATE = txtExchangeRate.Text;
            CRSer.Insert(search);
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
        string todayNepali = PGD.GetTodayNepaliDate();
        string[] nepParts = todayNepali.Split('/');
        string engDate = PGD.ConvertNepaliTOEnglish(nepParts[0], nepParts[1], nepParts[2]);
        DateTime dt = DateTime.ParseExact(engDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        string todayStored = dt.ToString("MM/dd/yyyy");

        FOREIGN_CURRENCY fcSearch = new FOREIGN_CURRENCY();
        fcSearch.STATUS = "1";
        var currencies = FCSer.GetAll(fcSearch);

        CURRENCY_RATE rateSearch = new CURRENCY_RATE();
        rateSearch.CONVERSION_DATE = todayStored;
        var todayRates = CRSer.GetAll(rateSearch);

        DataTable dtRates = new DataTable();
        dtRates.Columns.Add("CURRENCY_NAME", typeof(string));
        dtRates.Columns.Add("EXCHANGE_RATE", typeof(string));   
        dtRates.Columns.Add("CONVERSION_DATE", typeof(string)); 

        if (currencies != null)
        {
            foreach (FOREIGN_CURRENCY curr in currencies)
            {
                CURRENCY_RATE rate = null;
                if (todayRates != null)
                {
                    foreach (CURRENCY_RATE r in todayRates)
                    {
                        if (r.CURRENCY_TYPE_ID == curr.PK_ID)
                        {
                            rate = r;
                            break;
                        }
                    }
                }

                DataRow row = dtRates.NewRow();
                row["CURRENCY_NAME"] = curr.CURRENCY_NAME;

                if (rate != null && !string.IsNullOrEmpty(rate.EXCHANGE_RATE))
                {
                    decimal rateValue = Convert.ToDecimal(rate.EXCHANGE_RATE);
                    row["EXCHANGE_RATE"] = rateValue.ToString("N2");
                }
                else
                {
                    row["EXCHANGE_RATE"] = "N/A";
                }

                if (rate != null && !string.IsNullOrEmpty(rate.CONVERSION_DATE))
                {
                    DateTime rateDate = DateTime.ParseExact(rate.CONVERSION_DATE, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    row["CONVERSION_DATE"] = rateDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    row["CONVERSION_DATE"] = "";
                }

                dtRates.Rows.Add(row);
            }
        }

        gvTodayRates.DataSource = dtRates;
        gvTodayRates.DataBind();
    }

    protected void gvTodayRates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
     
    }
}