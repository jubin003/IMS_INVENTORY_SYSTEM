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
public class AccountFunction
{
    GL_ACCOUNT GLAEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLASer = new GL_ACCOUNTService();

    GL_SUB_ACCOUNT GLSAEnt = new GL_SUB_ACCOUNT();
    GL_SUB_ACCOUNTService GLSASer = new GL_SUB_ACCOUNTService();

    ACCOUNT_SETTING ASEnt = new ACCOUNT_SETTING();
    ACCOUNT_SETTINGService ASSer = new ACCOUNT_SETTINGService();
    public AccountFunction()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public bool VoucherCheckProvision()
    {
        bool access = false;
        ASEnt = new ACCOUNT_SETTING();
        ASEnt.STATUS_NAME = "Checked";//CheckBy
        ASEnt.STATUS = "1";
        ASEnt = (ACCOUNT_SETTING)ASSer.GetSingle(ASEnt);
        if (ASEnt != null)
        {
            access = true;
        }
        return access;
    }

    public string getGLAccountName(string GL_CODE)
    {
        string GL_name = "";
        GLAEnt = new GL_ACCOUNT();
        GLAEnt.GL_CODE = GL_CODE;
        GLAEnt = (GL_ACCOUNT)GLASer.GetSingle(GLAEnt);
        if (GLAEnt != null)
        {
            GL_name = GLAEnt.GL_NAME;
        }
        return GL_name;
    }

    public string getSGLAccountName(string SGL_CODE)
    {
        string SGL_name = "";
        GLSAEnt = new GL_SUB_ACCOUNT();
        GLSAEnt.SUB_GL_CODE = SGL_CODE;
        GLSAEnt = (GL_SUB_ACCOUNT)GLSASer.GetSingle(GLSAEnt);
        if (GLSAEnt != null)
        {
            SGL_name = GLSAEnt.SUB_GL_NAME;
        }
        return SGL_name;
    }

    public DataTable GET_GL_ACC_FROM_ACC_HEAD(string acc_head_1, string acc_head_2)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_SELECT.GET_GL_ACC_FROM_ACC_HEAD";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = acc_head_1;

        objCmd.Parameters.Add(_p1);
        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        if (acc_head_2 != "")
            _p2.Value = acc_head_2;
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
    public DataTable getSub_GL_LB(string gl_code, string fiscalYear, string office_code)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_SUB_GL_LB";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = gl_code;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = fiscalYear;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = office_code;
        objCmd.Parameters.Add(_p3);

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
    public DataTable getGL_LB(string gl_code, string fiscalYear, string office_code)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_GL_LB";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;
        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = gl_code;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = fiscalYear;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = office_code;
        objCmd.Parameters.Add(_p3);

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

    public DataTable getAccountHead()
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_ACCOUNT_HEAD";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

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

    public DataTable getAccountChart(string glcode, string gl_master_code, string gl_account_head)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_CHART_OF_ACCOUNTS";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;
        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = gl_master_code;
        objCmd.Parameters.Add(_p1);
        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = glcode;
        objCmd.Parameters.Add(_p2);
        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = gl_account_head;
        objCmd.Parameters.Add(_p3);
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
    public DataTable getDetailAccountChart(string glcode, string gl_master_code, string gl_account_head)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_DETAIL_CHART_OF_ACCOUNTS";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;
        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = gl_master_code;
        objCmd.Parameters.Add(_p1);
        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = glcode;
        objCmd.Parameters.Add(_p2);
        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = gl_account_head;
        objCmd.Parameters.Add(_p3);
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

    public DataTable getAccountLedger(string glcode, string sglcode, string fdate, string tdate, string fiscal_year,string office_code)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_ACCOUNT_LEDGER";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = glcode;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        if (sglcode != "Select")
            _p2.Value = sglcode;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = fdate;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        _p4.Value = tdate;
        objCmd.Parameters.Add(_p4);

        OracleParameter _p5 = new OracleParameter();
        _p5.Direction = ParameterDirection.Input;
        _p5.Value = fiscal_year;
        objCmd.Parameters.Add(_p5);

        OracleParameter _p6 = new OracleParameter();
        _p6.Direction = ParameterDirection.Input;
        _p6.Value = office_code;
        objCmd.Parameters.Add(_p6);

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
    public DataTable getNewBalance(string gl_code, string fiscal_year,string office_code)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_BALANCE_NEW";
        objCmd.Connection = conn;

        objCmd.CommandType = CommandType.StoredProcedure;
        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = gl_code;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        if (gl_code != "")
            _p2.Value = fiscal_year;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        if (office_code != "")
            _p3.Value = office_code;
        objCmd.Parameters.Add(_p3);

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
    public DataTable getBalance(string acc_head_code, string gl_code, string sub_gl_code , string fiscal_year, string office_code)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_BALANCE";
        objCmd.Connection = conn;

        objCmd.CommandType = CommandType.StoredProcedure;
        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = acc_head_code;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        if (gl_code != "")
            _p2.Value = gl_code;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = sub_gl_code;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        _p4.Value = fiscal_year;
        objCmd.Parameters.Add(_p4);

        OracleParameter _p5 = new OracleParameter();
        _p5.Direction = ParameterDirection.Input;
        _p5.Value = office_code;
        objCmd.Parameters.Add(_p5);

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

    public DataTable getLedgerTrialBalance(string fdate, string tdate,string fiscalyear, string office_code)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_LEDGER_TRIALBALANCE";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = fdate;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = tdate;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = fiscalyear;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        _p4.Value = office_code;
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

    public DataTable getGroupTrialBalance(string fdate, string tdate, string fiscalyear, string office_code)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_GROUP_TRIALBALANCE";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = fdate;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = tdate;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = fiscalyear;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        _p4.Value = office_code;
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

    public DataTable GET_CONFIRMATION_OF_ACCOUNTS(string sgl_code, string fiscalyear,string OFFICE_CODE)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_CONFIRMATION_OF_ACCOUNTS";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = sgl_code;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = fiscalyear;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = OFFICE_CODE;
        objCmd.Parameters.Add(_p3);

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


    public DataTable getAccountTrialBalance(string fdate, string tdate, string fiscalyear, string office_code)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_ACCOUNT_TRIALBALANCE";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = fdate;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = tdate;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = fiscalyear;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        _p4.Value = office_code;
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
    public DataTable getVoucherType(string fromdate, string todate, string voucherType)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_VOUCHER_TYPE";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;
        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = fromdate;
        objCmd.Parameters.Add(_p1);
        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = todate;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = voucherType;
        objCmd.Parameters.Add(_p3);

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
    public DataTable getVoucherDetail(string voucher_id, string order_by)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_VOUCHER_DETAIL";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = voucher_id;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = order_by;
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
    
    public DataTable getMonthlyProductSummary(string  fiscal_year, string product_id, string batch)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GETMONTHLY_SUMMARY";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = fiscal_year;
        objCmd.Parameters.Add(_p1);


        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = product_id;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = batch;
        objCmd.Parameters.Add(_p3);

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
    public DataTable getMonthlyItemRegister(string fiscal_year, string product_id, string batch, string month)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_ITEM_REGISTER";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = fiscal_year;
        objCmd.Parameters.Add(_p1);


        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = product_id;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = batch;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        _p4.Value = month;
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
    public DataTable getSundryDebtorsBalance(string fiscal_year, string gl_code)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS. GetCustomerBalances";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = fiscal_year;
        objCmd.Parameters.Add(_p1);


        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = gl_code;
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

    public string getLedgerOpeningBalance(string ledger_code, string sub_ledger_code, string fiscalyear, string fromdate)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_LEDGER_OPENING_BALANCE";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = ledger_code;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = sub_ledger_code;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = fiscalyear;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        _p4.Value = fromdate;
        objCmd.Parameters.Add(_p4);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);
        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();
        if (dtable != null)
        {
            string dr_amt = dtable.Rows[0][0].ToString();
            string cr_amt = dtable.Rows[0][1].ToString();
            return (Convert.ToDouble(dr_amt) - Convert.ToDouble(cr_amt)).ToString("0.00");

        }

        else
            return "0.00";
    }

    public string getIncome_Statement(string gl_master_code, string fisaclyear)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_ACCOUNT_REPORTS.GET_INCOME_STATEMENT";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = gl_master_code;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = fisaclyear;
        objCmd.Parameters.Add(_p2);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);
        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();
        if (dtable != null)
        {
            string dr_amt = dtable.Rows[0][0].ToString();
            double amt = Convert.ToDouble(dr_amt);
            if (amt < 0)
                amt = amt * -1;
            return amt.ToString("0.00");
        }
        else
            return null;
    }

    public string getGLMasterCode(string account_head)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_account_select.GET_GL_MASTER_CODE";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;
        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = account_head;
        objCmd.Parameters.Add(_p1);
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
    public string getGLCode(string account_Master)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_account_select.GET_GL_CODE";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = account_Master;
        objCmd.Parameters.Add(_p1);

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

    public string getNext_VM_ID(string Voucher_type, string Voucher_FY, string office_code)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_account_select.GET_NEXT_VOUCHER_NO";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = Voucher_type;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = Voucher_FY;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = office_code;
        objCmd.Parameters.Add(_p3);

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

    public DataTable getBalanceSheetHeading(string BS_MAIN_HEADING)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_account_reports.GET_BALANCESHEET_HEADING";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = BS_MAIN_HEADING;
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
    public DataTable getBalanceSheetSubHeading(string BS_HEADING_ID)
    {
        DataTable dtable = new DataTable();
        OracleDataReader ora_reader;
        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_account_reports.GET_BALANCESHEET_SUB_HEADING";
        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = BS_HEADING_ID;
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

    public string getBalanceSheet(string fiscalYear, string BS_Sub_heading_ID, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_account_reports.GET_BALANCESHEET_AMOUNT";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = fiscalYear;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = BS_Sub_heading_ID;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = office_code;
        objCmd.Parameters.Add(_p3);


        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        string amt = "0";
        try
        {
            amt = dtable.Rows[0][1].ToString();
        }
        catch
        {

        }

        return amt;
    }

}