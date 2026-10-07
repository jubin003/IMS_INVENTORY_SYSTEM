using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Entity.Components;
using Service.Components;

/// <summary>
/// Summary description for AccountFunction
/// </summary>
public class GraphFunction
{

    public GraphFunction()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //------------------------------------------------GRAPHS------------------------------------------------------------------------------------
    public DataTable monthlyTopSales(string offc_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_chart.TOP_SALES_MONTH";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = offc_code;
        objCmd.Parameters.Add(_p1);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        if (dtable != null)
            return dtable;
        else
            return null;

    }

    public DataTable OperatingCostLastSix(string offc_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_chart.INDIRECT_COST_LAST_SIX";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = offc_code;
        objCmd.Parameters.Add(_p1);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        if (dtable != null)
            return dtable;
        else
            return null;

    }

    public DataTable CashFlowThisLastMth(string offc_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_chart.CASH_FLOW_PAST_PRESENT";



        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = offc_code;
        objCmd.Parameters.Add(_p1);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        if (dtable != null)
            return dtable;
        else
            return null;

    }

    public DataTable GetTopDebtorsCreditors(string acc_head_code, string gl_code, string fiscal_year, string offc_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_chart.HIGHEST_DEBITOR_CREDITOR";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = acc_head_code;
        objCmd.Parameters.Add(_p1);


        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = gl_code;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = fiscal_year;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        _p4.Value = offc_code;
        objCmd.Parameters.Add(_p4);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        if (dtable != null)
            return dtable;
        else
            return null;

    }

    public DataTable DailySales(string salestype, string offc_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_chart.DAILY_SALES";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        if (salestype != "")
            _p1.Value = salestype;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = offc_code;
        objCmd.Parameters.Add(_p2);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        if (dtable != null)
            return dtable;
        else
            return null;
    }
    public DataTable DailyPurchase(string purchasetype, string offc_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_chart.DAILY_PURCHASE";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        if (purchasetype != "")
            _p1.Value = purchasetype;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = offc_code;
        objCmd.Parameters.Add(_p2);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        if (dtable != null)
            return dtable;
        else
            return null;
    }

    #region Admission Date Count For Chart
    public string getmonthlySalesSummary(string day, string month, string year, string offc_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_CHART.GET_MONTHLY_SALES_SUMMARY";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = day;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = month;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = year;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        _p4.Value = offc_code;
        objCmd.Parameters.Add(_p4);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        if (dtable != null)
            return dtable.Rows[0][0].ToString();
        else
            return null;
    }
    public string getmonthlyPurchaseSummary(string day, string month, string year, string offc_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_CHART.GET_MONTHLY_PURCHASE_SUMMARY";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = day;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = month;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = year;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        _p4.Value = offc_code;
        objCmd.Parameters.Add(_p4);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        if (dtable != null)
            return dtable.Rows[0][0].ToString();
        else
            return null;
    }
    #endregion
}