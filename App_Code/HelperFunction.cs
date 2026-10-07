using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Reflection;
using Entity.Components;
using Service.Components;
using DataHelper.Framework;
using Oracle.DataAccess.Client;
using System.IO;
using System.Globalization;
using System.Linq;

/// <summary>
/// Summary description for HelperFunction
/// </summary>
/// 

public class HelperFunction
{
    ADBS ADBSEnt = new ADBS();
    ADBSService ADBSSer = new ADBSService();

    SUPPLIERS SEnt = new SUPPLIERS();
    SUPPLIERSService SSer = new SUPPLIERSService();

    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();


    #region menu bar
    public DataTable getModule(string groupid)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_useracces.getmodule";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;


        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = groupid;
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

    public DataTable getSubModule(string groupid, string moduleid)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_useracces.getsubmodule";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;


        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = groupid;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = moduleid;
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

    public DataTable getlinkname(string moduleid, string submoduleid, string groupid)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_useracces.getlinknameparent";


        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;


        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = moduleid;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = submoduleid;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = groupid;
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

    #endregion

    #region page access
    public Boolean checkPageAccess(string pageurl, string groupid)
    {

        string pageid = "";
        PAGES PGEnt = new PAGES();
        PAGESService PGSer = new PAGESService();
        PGEnt.PAGENAME = pageurl.ToLower();
        PGEnt = (PAGES)PGSer.GetSingle(PGEnt);
        if (PGEnt != null)
        {
            pageid = PGEnt.PK_ID;
            string permission = getPagePermission(pageid, groupid);

            if (permission != "00000000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public string getPagePermission(string pageid, string groupid)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "PKJ_USERACCES.getPagePermission";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = pageid;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = groupid;
        objCmd.Parameters.Add(_p2);

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

    public string ConvertDtToJSon(DataTable dt)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            { row.Add(col.ColumnName.Trim(), dr[col]); }
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }
    public DataTable GridViewToDataTable(GridView dtg)
    {
        try
        {
            DataTable dt = new DataTable();
            // add the columns to the datatable 
            if (dtg.HeaderRow != null)
            {
                for (int i = 0; i < dtg.HeaderRow.Cells.Count; i++)
                {
                    if (dtg.HeaderRow.Cells[i].Text != "")
                    {
                        dt.Columns.Add(dtg.HeaderRow.Cells[i].Text);
                    }
                    else
                    {
                        if (dtg.HeaderRow.Cells[i].HasControls())
                        {
                            //   Literal li = new Literal();
                            //  li.Text = (dtg.HeaderRow.Cells[i].Controls[1] as Label).Text;
                            dt.Columns.Add((dtg.HeaderRow.Cells[i].Controls[1] as Label).Text);
                        }
                    }
                }
            }

            // add each of the data rows to the table
            foreach (GridViewRow row in dtg.Rows)
            {
                DataRow dr;
                dr = dt.NewRow();
                Literal l = new Literal();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row.Cells[i].HasControls())
                    {
                        try
                        {
                            l.Text = (row.Cells[i].Controls[1] as LinkButton).Text;
                            dr[i] = l.Text.ToString();
                        }
                        catch
                        {
                            l.Text = (row.Cells[i].Controls[1] as Label).Text;
                            dr[i] = l.Text.ToString();
                        }
                    }
                    else
                    {
                        dr[i] = row.Cells[i].Text.Replace(" ", "");
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public string DataTableToJSON(DataTable table)
    {
        var JSONString = new StringBuilder();
        if (table.Rows.Count > 0)
        {
            JSONString.Append("[");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                JSONString.Append("{");
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    if (j < table.Columns.Count - 1)
                    {
                        JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                    }
                    else if (j == table.Columns.Count - 1)
                    {
                        JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                    }
                }
                if (i == table.Rows.Count - 1)
                {
                    JSONString.Append("}");
                }
                else
                {
                    JSONString.Append("},");
                }
            }
            JSONString.Append("]");
        }


        return JSONString.ToString();
        // System.IO.File.WriteAllText(@"C:\Users\Public\" + filename + ".txt", JSONString.ToString());


    }
    public string DataTableToXML(DataTable table)
    {
        DataSet dataSet = new DataSet();
        dataSet.Tables.Add(table);

        StringWriter sw = new StringWriter();
        dataSet.WriteXml(sw);

        return sw.ToString();

    }
    private static Hashtable m_executingPages = new Hashtable();

    public HelperFunction()
    {
        // TODO: Add constructor logic here
        //       
    }
    public static void MsgBox(Control updatePanUsed, Type UpdatePanType, string alert)
    {
        Guid gMessage = Guid.NewGuid();
        string script = @"alert('" + alert + "');";
        ScriptManager.RegisterStartupScript(updatePanUsed, UpdatePanType, gMessage.ToString(), script, true);
    }
    public static void OpenNewWindow(Control updatePanUsed, Type UpdatePanType, string url)
    {
        Guid gMessage = Guid.NewGuid();
        string script = @"window.open('" + url + "','','location=0,status=0,scrollbars=1,resizable=1 ');";
        ScriptManager.RegisterStartupScript(updatePanUsed, UpdatePanType, gMessage.ToString(), script, true);
    }
    public static void Show(string sMessage)
    {
        // If this is the first time a page has called this method then
        if (!m_executingPages.Contains(HttpContext.Current.Handler))
        {
            // Attempt to cast HttpHandler as a Page.
            Page executingPage = HttpContext.Current.Handler as Page;
            if (executingPage != null)
            {
                // Create a Queue to hold one or more messages.
                Queue messageQueue = new Queue();
                // Add our message to the Queue
                messageQueue.Enqueue(sMessage);
                // Add our message queue to the hash table. Use our page reference
                // (IHttpHandler) as the key.
                m_executingPages.Add(HttpContext.Current.Handler, messageQueue);
                // Wire up Unload event so that we can inject 
                // some JavaScript for the alerts.
                executingPage.Unload += new EventHandler(ExecutingPage_Unload);
            }
        }
        else
        {
            // If were here then the method has allready been 
            // called from the executing Page.
            // We have allready created a message queue and stored a
            // reference to it in our hastable. 
            Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];
            // Add our message to the Queue
            queue.Enqueue(sMessage);
        }
    }
    private static void ExecutingPage_Unload(object sender, EventArgs e)
    {
        // Get our message queue from the hashtable

        // Our page has finished rendering so lets output the
        // JavaScript to produce the alert's

        Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];
        if (queue != null)
        {
            StringBuilder sb = new StringBuilder();
            // How many messages have been registered?
            int iMsgCount = queue.Count;
            // Use StringBuilder to build up our client slide JavaScript.
            sb.Append("<script language='javascript'>");
            // Loop round registered messages
            string sMsg;
            while (iMsgCount-- > 0)
            {
                sMsg = (string)queue.Dequeue();
                sMsg = sMsg.Replace("\n", "\\n");
                sMsg = sMsg.Replace("\"", "'");
                sb.Append(@"alert( """ + sMsg + @""" );");
            }
            // Close our JS
            sb.Append(@"</script>");
            // Were done, so remove our page reference from the hashtable
            m_executingPages.Remove(HttpContext.Current.Handler);
            // Write the JavaScript to the end of the response stream.
            HttpContext.Current.Response.Write(sb.ToString());
        }
    }
    public void moveToNextControl(Control control)
    {
        int counter = 0;
        foreach (Control c in control.Controls)
        {
            if (c.Controls.Count > 0)
            {
                moveToNextControl(c);
            }
            else if (c is TextBox)
            {
                ((TextBox)(c)).Attributes.Add("onkeydown", "moveToNext(this,false," + counter + ")");

            }
            else if (c is Button)
            {
                ((Button)(c)).UseSubmitBehavior = false;

            }
            else if (c is RadioButton)
            {
                ((RadioButton)(c)).Attributes.Add("onkeydown", "moveToNext(this,false)");
            }
            else if (c is CheckBox)
            {
                ((CheckBox)(c)).Attributes.Add("onkeydown", "moveToNext(this,false)");
            }
            else if (c is DropDownList)
            {
                ((DropDownList)(c)).Attributes.Add("onkeydown", "moveToNext(this,false)");
            }
            else if (c is ListBox)
            {
                ((ListBox)(c)).Attributes.Add("onkeydown", "moveToNext(this,false)");
            }
        }
    }
    //To convert number to words
    #region Dec - Arrays
    private string[] arrOnes = new string[] {
            "Zero",
            "One",
            "Two",
            "Three",
            "Four",
            "Five",
            "Six",
            "Seven",
            "Eight",
            "Nine"
        };
    private string[] arrMisc = new string[] {
            "",
            "Eleven",
            "Twelve",
            "Thirteen",
            "Fourteen",
            "Fifteen",
            "Sixteen",
            "Seventeen",
            "Eighteen",
            "Nineteen",
        };
    private string[] arrTens = new string[] {
            "",
            "Ten",
            "Twenty",
            "Thirty",
            "Forty",
            "Fifty",
            "Sixty",
            "Seventy",
            "Eighty",
            "Ninety",
        };

    #endregion

    #region Constants
    private const string HUNDRED_TEXT = " Hundred";
    private const string THOUSAND_TEXT = " Thousand";
    private const string LAKH_TEXT = " Lakh";
    private const string CORORE_TEXT = " Crore";
    private const string ARAB_TEXT = " Arab";
    private const string KHARAB_TEXT = " Kharab";
    private const string AND_TEXT = " and ";
    private const string POINT_TEXT = " point ";
    private const string TOO_HIGH_TEXT = "Too high value";

    private const int ONE = 1;
    private const int TEN = 2;
    private const int HUNDRED = 3;
    private const int THOUSAND = 4;
    private const int TENTHOUSAND = 5;
    private const int LAKH = 6;
    private const int TENLAKH = 7;
    private const int CORORE = 8;
    private const int TENCORORE = 9;
    private const int ARAB = 10;
    private const int TENARAB = 11;
    private const int KHARAB = 12;
    private const int TENKHARAB = 13;
    private const int MAX_ALLOWED_LENGTH = TENKHARAB;

    public const int ERR_TOOLENGTH = -1;
    public const int ERR_INPUT = -2;
    public const int RET_SUCCESS = 0;
    #endregion

    public int ConvertNumberToText(String sNumber, out string sResult)
    {
        sResult = "";
        string[] str = sNumber.Split(new char[] { '.' });
        string sStrAfterDecimal = str.Length > 1 ? str[1] : "";
        string sStrBeforeDecimal = str.Length >= 1 ? str[0] : "";

        if (sStrBeforeDecimal.Length > MAX_ALLOWED_LENGTH)
        {
            sResult = TOO_HIGH_TEXT;
            return ERR_TOOLENGTH;
        }

        if (GetNumberText(sStrBeforeDecimal, false, ref sResult) == RET_SUCCESS)
        {
            //if (sStrAfterDecimal != "")
            //{
            if (sStrAfterDecimal != "00" & sStrAfterDecimal != "")
            {
                sResult += AND_TEXT;
                if (GetNumberText(sStrAfterDecimal, true, ref sResult) != RET_SUCCESS)
                {
                    sResult = "Error while computing..";
                    return ERR_INPUT;
                }
            }
            //}
            sResult += " Only";
            return RET_SUCCESS;
        }
        else
        {
            sResult = "Error while computing..";
            return ERR_INPUT;
        }
    }

    private string GetTwoDigitString(string str)
    {
        if (str == "00") return "";
        string retString = "";
        if (str[1] == '0')
            retString += arrTens[int.Parse(str[0].ToString())];
        else if (str[0] == '1')
            retString += arrMisc[int.Parse(str[1].ToString())];
        else
        {
            retString += arrTens[int.Parse(str[0].ToString())] + " ";
            retString += arrOnes[int.Parse(str[1].ToString())];
        }
        return retString;
    }

    // To Convert English Number to nepali unicode
    public String getNepaliText(String item)
    {
        String str = item;
        String newStr = "";
        int len = str.Length;
        int i;
        for (i = 0; i < len; i++)
        {
            switch (str.Substring(i, 1))
            {
                case "0":
                    newStr = newStr + "\u0966";
                    break;

                case "1":
                    newStr = newStr + "\u0967";
                    break;

                case "2":
                    newStr = newStr + "\u0968";
                    break;

                case "3":
                    newStr = newStr + "\u0969";
                    break;

                case "4":
                    newStr = newStr + "\u096a";
                    break;

                case "5":
                    newStr = newStr + "\u096b";
                    break;

                case "6":
                    newStr = newStr + "\u096c";
                    break;

                case "7":
                    newStr = newStr + "\u096d";
                    break;

                case "8":
                    newStr = newStr + "\u096e";
                    break;

                case "9":
                    newStr = newStr + "\u096f";
                    break;

                case "/":
                    newStr = newStr + "/";
                    break;

                default:
                    newStr = newStr + str.Substring(i, 1);
                    break;
            }
        }
        return newStr;
    }

    // To Convert English Number to nepali unicode end

    // To Convert  nepali unicode to English Number 
    public String getEnglishText(String item)
    {
        String str = item;
        String newStr = "";
        int len = str.Length;
        int i;
        for (i = 0; i < len; i++)
        {
            switch (str.Substring(i, 1))
            {
                case "\u0966":
                    newStr = newStr + "0";
                    break;

                case "\u0967":
                    newStr = newStr + "1";
                    break;

                case "\u0968":
                    newStr = newStr + "2";
                    break;

                case "\u0969":
                    newStr = newStr + "3";
                    break;

                case "\u096a":
                    newStr = newStr + "4";
                    break;

                case "\u096b":
                    newStr = newStr + "5";
                    break;

                case "\u096c":
                    newStr = newStr + "6";
                    break;

                case "\u096d":
                    newStr = newStr + "7";
                    break;

                case "\u096e":
                    newStr = newStr + "8";
                    break;

                case "\u096f":
                    newStr = newStr + "9";
                    break;

                case "/":
                    newStr = newStr + "/";
                    break;

                default:
                    newStr = newStr + str.Substring(i, 1);
                    break;
            }
        }
        return newStr;
    }
    public static void EmptyGridFix(GridView grdView)
    {

        if (grdView.Rows.Count == 0 && grdView.DataSource != null)
        {
            System.Data.DataTable dt = null;
            // need to clone sources otherwise it will be indirectly adding to the original source
            if (grdView.DataSource is DataSet)
            {
                dt = ((DataSet)grdView.DataSource).Tables[0].Clone();
            }
            else if (grdView.DataSource is System.Data.DataTable)
            {
                dt = ((System.Data.DataTable)grdView.DataSource).Clone();
            }
            if (dt == null)
            {
                return;
            }
            dt.Rows.Add(dt.NewRow()); // add empty row
            grdView.DataSource = dt;
            grdView.DataBind();
            // hide row
            grdView.Rows[0].Visible = false;
            grdView.Rows[0].Controls.Clear();
        }


        // normally executes at all postbacks
        if (grdView.Rows.Count == 1 && grdView.DataSource == null)
        {
            bool bIsGridEmpty = true;
            // check first row that all cells empty
            for (int i = 0; i < grdView.Rows[0].Cells.Count; i++)
            {
                if (grdView.Rows[0].Cells[i].Text != string.Empty)
                {
                    bIsGridEmpty = false;
                }
            }
            // hide row
            if (bIsGridEmpty)
            {
                grdView.Rows[0].Visible = false;
                grdView.Rows[0].Controls.Clear();
            }
        }
    }
    public void GetPageName(Label lblPageName, String page)
    {
        string CurrentModule = HttpContext.Current.Session["UserModule"].ToString();
        System.IO.FileInfo oInfo = new System.IO.FileInfo(page);
        int startIndex = Convert.ToInt16(oInfo.ToString().IndexOf('/', 1));
        string newPage = oInfo.ToString().Substring(startIndex + 1);
        //Pages pageEntity = new Pages();
        //PagesService pageService = new PagesService();
        //pageEntity.PAGENAME = newPage;
        //pageEntity = (Pages)pageService.GetSingle(pageEntity);
        //lblPageName.Text = pageEntity.LINKNAME;
        try
        {
            lblPageName.Text = Convert.ToString(DataAccessHelper.ExecuteScalar("PKJ_Select.selectPageTitle", CommandType.StoredProcedure, CreatePageTitleParmans(newPage, CurrentModule)));
        }
        catch { }
    }
    private IDbDataParameter[] CreatePageTitleParmans(string pagename, string module_id)
    {
        List<IDbDataParameter> cmdParams = new List<IDbDataParameter>();
        cmdParams.Add(DataAccessFactory.CreateDataParameter("page_name", pagename));
        cmdParams.Add(DataAccessFactory.CreateDataParameter("module_id", module_id));
        cmdParams.Add(DataAccessFactory.CreateDataParameter("Result", ""));
        return cmdParams.ToArray();
    }
    private IDbDataParameter[] CreateNullParmans()
    {
        List<IDbDataParameter> cmdParams = new List<IDbDataParameter>();
        cmdParams.Add(DataAccessFactory.CreateDataParameter("Result", ""));
        return cmdParams.ToArray();
    }
    public int GetNumberText(string str, bool isAfterDecimal, ref string retStr)
    {
        bool addAndString = false;
        try
        {
            if (isAfterDecimal && str.Length >= 1)
            {
                if (str[0] == '0')
                    retStr += arrOnes[int.Parse(str[1].ToString())];
                else if (str[0] == '1')
                    retStr += arrMisc[int.Parse(str[1].ToString())];
                else
                {
                    retStr += arrTens[int.Parse(str[0].ToString())] + " ";
                    if (str.Length > 1)
                        retStr += arrOnes[int.Parse(str[1].ToString())];
                }
                retStr += " Paisa ";
                //for (int i = 0; i < str.Length; i++)
                //    retStr += arrOnes[int.Parse(str[i].ToString())] + " ";
                //retStr = retStr.Remove(retStr.Length - 1);
            }
            else
            {
                while (str.Length > 0)
                {
                    switch (str.Length)
                    {
                        case ONE:
                            retStr += arrOnes[int.Parse(str[0].ToString())];
                            str = "";
                            break;
                        case TEN:
                            {
                                string temp = GetTwoDigitString(str);
                                if (temp != "")
                                    retStr += addAndString ? AND_TEXT + temp : temp;
                                str = "";
                            }
                            break;
                        case HUNDRED:
                            if (str[0] != '0')
                                retStr += string.Format("{0}{1}", arrOnes[int.Parse(str[0].ToString())], HUNDRED_TEXT);

                            if (str.Substring(1) != "00")
                                addAndString = true;
                            str = str.Remove(0, 1);
                            break;
                        case THOUSAND:
                            retStr += string.Format("{0}{1} ", arrOnes[int.Parse(str[0].ToString())], THOUSAND_TEXT);
                            str = str.Remove(0, 1);
                            break;
                        case TENTHOUSAND:
                            {
                                string temp = GetTwoDigitString(str.Substring(0, 2));
                                if (temp != "")
                                    retStr += string.Format("{0}{1} ", temp, THOUSAND_TEXT);
                                str = str.Remove(0, 2);
                            }
                            break;
                        case LAKH:
                            retStr += string.Format("{0}{1} ", arrOnes[int.Parse(str[0].ToString())], LAKH_TEXT);
                            str = str.Remove(0, 1);
                            break;
                        case TENLAKH:
                            {
                                string temp = GetTwoDigitString(str.Substring(0, 2));
                                if (temp != "")
                                    retStr += string.Format("{0}{1} ", temp, LAKH_TEXT);
                                str = str.Remove(0, 2);
                            }
                            break;
                        case CORORE:
                            retStr += string.Format("{0}{1} ", arrOnes[int.Parse(str[0].ToString())], CORORE_TEXT);
                            str = str.Remove(0, 1);
                            break;
                        case TENCORORE:
                            {
                                string temp = GetTwoDigitString(str.Substring(0, 2));
                                if (temp != "")
                                    retStr += string.Format("{0}{1} ", temp, CORORE_TEXT);
                                str = str.Remove(0, 2);
                            }
                            break;


                        // changes by binod
                        case ARAB:
                            retStr += string.Format("{0}{1} ", arrOnes[int.Parse(str[0].ToString())], ARAB_TEXT);
                            str = str.Remove(0, 1);
                            break;
                        case TENARAB:
                            {
                                string temp = GetTwoDigitString(str.Substring(0, 2));
                                if (temp != "")
                                    retStr += string.Format("{0}{1} ", temp, ARAB_TEXT);
                                str = str.Remove(0, 2);
                            }
                            break;
                        case KHARAB:
                            retStr += string.Format("{0}{1} ", arrOnes[int.Parse(str[0].ToString())], KHARAB_TEXT);
                            str = str.Remove(0, 1);
                            break;
                        case TENKHARAB:
                            {
                                string temp = GetTwoDigitString(str.Substring(0, 2));
                                if (temp != "")
                                    retStr += string.Format("{0}{1} ", temp, KHARAB_TEXT);
                                str = str.Remove(0, 2);
                            }
                            break;
                        //changes end

                        default:
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return ERR_INPUT;
        }
        return RET_SUCCESS;
    }
    public int GetNumberTextDollar(string str, bool isAfterDecimal, ref string retStr)
    {
        bool addAndString = false;
        try
        {
            if (isAfterDecimal && str.Length >= 1)
            {
                if (str[0] == '0')
                    retStr += arrOnes[int.Parse(str[1].ToString())];
                else if (str[0] == '1')
                    retStr += arrMisc[int.Parse(str[1].ToString())];
                else
                {
                    retStr += arrTens[int.Parse(str[0].ToString())] + " ";
                    retStr += arrOnes[int.Parse(str[1].ToString())];
                }
                retStr += " Cents ";
                //for (int i = 0; i < str.Length; i++)
                //    retStr += arrOnes[int.Parse(str[i].ToString())] + " ";
                //retStr = retStr.Remove(retStr.Length - 1);
            }
            else
            {
                while (str.Length > 0)
                {
                    switch (str.Length)
                    {
                        case ONE:
                            retStr += arrOnes[int.Parse(str[0].ToString())];
                            str = "";
                            break;
                        case TEN:
                            {
                                string temp = GetTwoDigitString(str);
                                if (temp != "")
                                    retStr += addAndString ? AND_TEXT + temp : temp;
                                str = "";
                            }
                            break;
                        case HUNDRED:
                            if (str[0] != '0')
                                retStr += string.Format("{0}{1}", arrOnes[int.Parse(str[0].ToString())], HUNDRED_TEXT);

                            if (str.Substring(1) != "00")
                                addAndString = true;
                            str = str.Remove(0, 1);
                            break;
                        case THOUSAND:
                            retStr += string.Format("{0}{1} ", arrOnes[int.Parse(str[0].ToString())], THOUSAND_TEXT);
                            str = str.Remove(0, 1);
                            break;
                        case TENTHOUSAND:
                            {
                                string temp = GetTwoDigitString(str.Substring(0, 2));
                                if (temp != "")
                                    retStr += string.Format("{0}{1} ", temp, THOUSAND_TEXT);
                                str = str.Remove(0, 2);
                            }
                            break;
                        case LAKH:
                            retStr += string.Format("{0}{1} ", arrOnes[int.Parse(str[0].ToString())], LAKH_TEXT);
                            str = str.Remove(0, 1);
                            break;
                        case TENLAKH:
                            {
                                string temp = GetTwoDigitString(str.Substring(0, 2));
                                if (temp != "")
                                    retStr += string.Format("{0}{1} ", temp, LAKH_TEXT);
                                str = str.Remove(0, 2);
                            }
                            break;
                        case CORORE:
                            retStr += string.Format("{0}{1} ", arrOnes[int.Parse(str[0].ToString())], CORORE_TEXT);
                            str = str.Remove(0, 1);
                            break;
                        case TENCORORE:
                            {
                                string temp = GetTwoDigitString(str.Substring(0, 2));
                                if (temp != "")
                                    retStr += string.Format("{0}{1} ", temp, CORORE_TEXT);
                                str = str.Remove(0, 2);
                            }
                            break;


                        // changes by binod
                        case ARAB:
                            retStr += string.Format("{0}{1} ", arrOnes[int.Parse(str[0].ToString())], ARAB_TEXT);
                            str = str.Remove(0, 1);
                            break;
                        case TENARAB:
                            {
                                string temp = GetTwoDigitString(str.Substring(0, 2));
                                if (temp != "")
                                    retStr += string.Format("{0}{1} ", temp, ARAB_TEXT);
                                str = str.Remove(0, 2);
                            }
                            break;
                        case KHARAB:
                            retStr += string.Format("{0}{1} ", arrOnes[int.Parse(str[0].ToString())], KHARAB_TEXT);
                            str = str.Remove(0, 1);
                            break;
                        case TENKHARAB:
                            {
                                string temp = GetTwoDigitString(str.Substring(0, 2));
                                if (temp != "")
                                    retStr += string.Format("{0}{1} ", temp, KHARAB_TEXT);
                                str = str.Remove(0, 2);
                            }
                            break;
                        //changes end

                        default:
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return ERR_INPUT;
        }
        return RET_SUCCESS;
    }

    #region Reporting tools
    public static DataTable ListToDataTable<T>(IList<T> list)
    {
        DataTable dt = new DataTable();

        foreach (PropertyInfo info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
        }
        foreach (T t in list)
        {
            DataRow row = dt.NewRow();
            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null);
            }
            dt.Rows.Add(row);
        }
        return dt;
    }
    public static DataTable EntityListToDataTable<T>(IList list)
    {
        DataTable dt = new DataTable();

        foreach (System.Reflection.PropertyInfo info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
        }
        foreach (T t in list)
        {
            DataRow row = dt.NewRow();
            foreach (System.Reflection.PropertyInfo info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null);
            }
            dt.Rows.Add(row);
        }
        return dt;
    }
    //This function will export the repeater to excel
    public void ExportToExcel(Repeater repeater, string excelFileName, string extraHeader, HttpResponse Response)
    {
        Response.Clear();
        if (excelFileName == "")
            excelFileName = "ExcelReport";
        string headerContent = "attachment;filename=" + excelFileName + ".xls";
        Response.AddHeader("content-disposition", headerContent);

        Response.Charset = "";

        // If you want the option to open the Excel file without saving than

        // comment out the line below

        //Response.Cache.SetCacheability(HttpCacheability.NoCache);

        Response.ContentType = "application/vnd.xls";

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();

        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

        if (extraHeader != "")
        {
            string header = "<center><h3>" + extraHeader + "</h3></center> <br/><br/>";
            htmlWrite.Write(header);
        }

        repeater.RenderControl(htmlWrite);

        Response.Write(stringWrite.ToString());

        Response.End();
    }
    public void ExportGridToExcel(GridView grid, string excelFileName, string extraHeader, HttpResponse Response)
    {
        Response.Clear();
        if (excelFileName == "")
            excelFileName = "ExcelReport";
        string headerContent = "attachment;filename=" + excelFileName + ".xls";
        Response.AddHeader("content-disposition", headerContent);

        Response.Charset = "";

        // If you want the option to open the Excel file without saving than

        // comment out the line below

        //Response.Cache.SetCacheability(HttpCacheability.NoCache);

        Response.ContentType = "application/vnd.xls";

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();

        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

        if (extraHeader != "")
        {
            string header = "<center><h3>" + extraHeader + "</h3></center> <br/><br/>";
            htmlWrite.Write(header);
        }

        for (int rowPos = 0; rowPos < grid.Rows.Count; ++rowPos)
        {
            for (int colPos = 0; colPos < grid.Rows[rowPos].Cells.Count; ++colPos)
            {
                grid.Rows[rowPos].Cells[colPos].Attributes.Add("class", "NumberString");
            }
        }
        grid.RenderControl(htmlWrite);
        Response.Write(stringWrite.ToString());
        Response.End();
    }
    public void ExportPanelToExcel(Panel pnl, string excelFileName, string extraHeader, HttpResponse Response)
    {
        Response.Clear();
        if (excelFileName == "")
            excelFileName = "ExcelReport";
        string headerContent = "attachment;filename=" + excelFileName + ".xls";
        Response.AddHeader("content-disposition", headerContent);

        Response.Charset = "";

        // If you want the option to open the Excel file without saving than

        // comment out the line below

        //Response.Cache.SetCacheability(HttpCacheability.NoCache);

        Response.ContentType = "application/vnd.xls";

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();

        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

        if (extraHeader != "")
        {
            string header = "<center><h3>" + extraHeader + "</h3></center> <br/><br/>";
            htmlWrite.Write(header);
        }

        pnl.RenderControl(htmlWrite);

        Response.Write(stringWrite.ToString());

        Response.End();
    }

    #endregion

    #region to convert the number into words

    public String NumWordsWrapper(double n)
    {
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        string words = "";
        double intPart;
        double decPart = 0;
        if (n == 0)
            return "zero";
        try
        {
            string[] splitter = n.ToString().Split('.');
            intPart = double.Parse(splitter[0]);
            decPart = double.Parse(splitter[1]);
        }
        catch
        {
            intPart = n;
        }

        words = NumWords(intPart);

        if (decPart > 0)
        {
            if (words != "")
                words += " and ";
            int counter = decPart.ToString().Length;
            switch (counter)
            {
                case 1: words += NumWords(decPart) + " tenths"; break;
                case 2: words += NumWords(decPart) + " paisa"; break;/* " hundredths"; break;*/
                case 3: words += NumWords(decPart) + " thousandths"; break;
                case 4: words += NumWords(decPart) + " ten-thousandths"; break;
                case 5: words += NumWords(decPart) + " hundred-thousandths"; break;
                case 6: words += NumWords(decPart) + " millionths"; break;
                case 7: words += NumWords(decPart) + " ten-millionths"; break;
            }
        }
        return textInfo.ToTitleCase(words);
    }

    static String NumWords(double n) //converts double to words
    {
        string[] numbersArr = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
        string[] tensArr = new string[] { "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninty" };
        string[] suffixesArr = new string[] { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septdecillion", "Octodecillion", "Novemdecillion", "Vigintillion" };
        string words = "";

        bool tens = false;

        if (n < 0)
        {
            words += "negative ";
            n *= -1;
        }

        int power = (suffixesArr.Length + 1) * 3;

        while (power > 3)
        {
            double pow = Math.Pow(10, power);
            if (n >= pow)
            {
                if (n % pow > 0)
                {
                    words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1] + ", ";
                }
                else if (n % pow == 0)
                {
                    words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1];
                }
                n %= pow;
            }
            power -= 3;
        }
        if (n >= 1000)
        {
            if (n % 1000 > 0) words += NumWords(Math.Floor(n / 1000)) + " thousand  ";
            else words += NumWords(Math.Floor(n / 1000)) + " thousand";
            n %= 1000;
        }
        if (0 <= n && n <= 999)
        {
            if ((int)n / 100 > 0)
            {
                words += NumWords(Math.Floor(n / 100)) + " hundred";
                n %= 100;
            }
            if ((int)n / 10 > 1)
            {
                if (words != "")
                    words += " ";
                words += tensArr[(int)n / 10 - 2];
                tens = true;
                n %= 10;
            }

            if (n < 20 && n > 0)
            {
                if (words != "" && tens == false)
                    words += " ";
                words += (tens ? "-" + numbersArr[(int)n - 1] : numbersArr[(int)n - 1]);
                n -= Math.Floor(n);
            }
        }

        return words;

    }

    #endregion
    public string getPaymentType(string PaymentCode)
    {
        string paymentName = "";
        PAYMENT_TYPE PTEnt = new PAYMENT_TYPE();
        PAYMENT_TYPEService PTSEr = new PAYMENT_TYPEService();
        PTEnt.PAYMENT_CODE = PaymentCode;
        PTEnt = (PAYMENT_TYPE)PTSEr.GetSingle(PTEnt);
        if (PTEnt != null)
        {
            paymentName = PTEnt.PAYMENT_NAME;
        }
        return paymentName;
    }
    public string getEmployeeName(string EmpId)
    {
        if (EmpId == "")
            return "";
        string name = "";
        EMPLOYEES EEnt = new EMPLOYEES();
        EMPLOYEESService ESrv = new EMPLOYEESService();

        EEnt.EMPLOYEEID = EmpId;
        EEnt = (EMPLOYEES)ESrv.GetSingle(EEnt);
        if (EEnt != null)
        {
            name = EEnt.FIRSTNAME + " " + EEnt.LASTNAME;
        }
        return name;
    }
    public string GetNewEmployeeId()
    {
        // Getting the New Unique Itemcode
        LoginService SrvMaster = new LoginService();
        string CurrentCode = "";
        CurrentCode = SrvMaster.GetSingleValue("PKJ_SELECT.getmaxempId", System.Data.CommandType.StoredProcedure, CreateNullParmans());
        if (CurrentCode == "" || CurrentCode.ToUpper() == "NULL")
        {
            CurrentCode = "1";
        }
        return CurrentCode;
    }
    #region utilities
    public string getCategoryCode()
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.getCategoryCode";

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
            return dtable.Rows[0][0].ToString();
        else
            return null;


    }
    public string getSubCategoryCode(string CategoryCode)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.getSubCategoryCode";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = CategoryCode;
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
    public string getProductName(string product_id)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.getProductName";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = product_id;
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

    public string getColourName(string colour_id)
    {
        string colourName = "";
        PRODUCT_COLOUR PCEnt = new PRODUCT_COLOUR();
        PRODUCT_COLOURService PCSer = new PRODUCT_COLOURService();
        PCEnt.PK_ID = colour_id;
        PCEnt = (PRODUCT_COLOUR)PCSer.GetSingle(PCEnt);
        if (PCEnt != null && colour_id != "")
            colourName = PCEnt.COLOUR_NAME;
        return colourName;
    }

    public string getSizeName(string size_id)
    {
        string SizeName = "";
        PRODUCT_SIZE PSEnt = new PRODUCT_SIZE();
        PRODUCT_SIZEService PSSer = new PRODUCT_SIZEService();
        PSEnt.PK_ID = size_id;
        PSEnt = (PRODUCT_SIZE)PSSer.GetSingle(PSEnt);
        if (PSEnt != null && size_id != "")
            SizeName = PSEnt.SIZE_NAME;
        return SizeName;
    }

    public string getPackQty(string product_id)
    {
        string PackQty = "";
        PRODUCT PEnt = new PRODUCT();
        PRODUCTService PSer = new PRODUCTService();
        PEnt.PK_ID = product_id;
        PEnt = (PRODUCT)PSer.GetSingle(PEnt);
        if (PEnt != null && product_id != "")
            PackQty = PEnt.PACK_QTY;
        return PackQty;
    }

    public string getProductUnit(string product_id)
    {
        string ProductUnit = "";
        PRODUCT PEnt = new PRODUCT();
        PRODUCTService PSer = new PRODUCTService();
        PEnt.PK_ID = product_id;
        PEnt = (PRODUCT)PSer.GetSingle(PEnt);
        if (PEnt != null && product_id != "")
        {
            PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
            PRODUCT_UNITService PUSer = new PRODUCT_UNITService();
            PUEnt.PK_ID = PEnt.UNIT_ID;
            PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
            if (PUEnt != null && PEnt.UNIT_ID != "")
                ProductUnit = PUEnt.UNIT_NAME;
        }
        return ProductUnit;
    }

    public string getProductUnitUpper(string product_id)
    {
        string ProductUnit = "";
        PRODUCT PEnt = new PRODUCT();
        PRODUCTService PSer = new PRODUCTService();
        PEnt.PK_ID = product_id;
        PEnt = (PRODUCT)PSer.GetSingle(PEnt);
        if (PEnt != null && product_id != "")
        {
            PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
            PRODUCT_UNITService PUSer = new PRODUCT_UNITService();
            PUEnt.PK_ID = PEnt.UPPER_UNIT_ID;
            PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
            if (PUEnt != null && PEnt.UPPER_UNIT_ID != "")
                ProductUnit = PUEnt.UNIT_NAME;
        }
        return ProductUnit;
    }

    public string getSupplierCode(string SypplierType)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_select.SELECT_SUPPLIER_CODE";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = SypplierType;
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

    public string getCustomerCode()
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_select.SELECT_CUSTOMER_CODE";

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
            return dtable.Rows[0][0].ToString();
        else
            return null;


    }

    public string getCustomerCodeFromId(string CustomerID)
    {
        string customerCode = "";
        CEnt = new CUSTOMER();
        CEnt.PK_ID = CustomerID;
        CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
        if (CEnt != null)
        {
            customerCode = CEnt.CUSTOMER_CODE;
        }
        return customerCode;
    }
    public string getProductCode()
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_select.SELECT_PRODUCT_CODE";

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
            return dtable.Rows[0][0].ToString();
        else
            return null;


    }
    public string SetExpiryDate(string month, string year)
    {
        string expdate = "";
        if (year != "")
        {
            if (month == "01")
                expdate = "31/01/" + year;
            else if (month == "02")
                if (Convert.ToDouble(year) % 4 == 0)
                    expdate = "29/02/" + year;
                else
                    expdate = "28/02/" + year;
            else if (month == "03")
                expdate = "31/03/" + year;
            else if (month == "04")
                expdate = "30/04/" + year;
            else if (month == "05")
                expdate = "31/05/" + year;
            else if (month == "06")
                expdate = "30/06/" + year;
            else if (month == "07")
                expdate = "31/07/" + year;
            else if (month == "08")
                expdate = "31/08/" + year;
            else if (month == "09")
                expdate = "30/09/" + year;
            else if (month == "10")
                expdate = "31/10/" + year;
            else if (month == "11")
                expdate = "30/11/" + year;
            else if (month == "12")
                expdate = "31/12/" + year;
        }
        return expdate;
    }

    public string GetExpiryDate(string medExpDate)
    {
        string expdate = "";
        string[] date = medExpDate.Split('/');
        expdate = date[2] + "/" + getEnglishMonth(date[1]);
        return expdate;
    }
    public string getEnglishMonth(string Month)
    {
        if (Month == "1" || Month == "01")
            return "Jan";
        else if (Month == "2" || Month == "02")
            return "Feb";
        else if (Month == "3" || Month == "03")
            return "Mar";
        else if (Month == "4" || Month == "04")
            return "Apr";
        else if (Month == "5" || Month == "05")
            return "May";
        else if (Month == "6" || Month == "06")
            return "Jun";
        else if (Month == "7" || Month == "07")
            return "Jul";
        else if (Month == "8" || Month == "08")
            return "Aug";
        else if (Month == "9" || Month == "09")
            return "Sep";
        else if (Month == "10")
            return "Oct";
        else if (Month == "11")
            return "Nov";
        else if (Month == "12")
            return "Dec";
        else
            return "";
    }

    #endregion
    #region for unique_token - used in invoice and Credit Note
    public string getmaxinvid()
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.getmaxinvid";

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
            return dtable.Rows[0][0].ToString();
        else
            return null;

    }
    public string getmaxCNid()
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.getmaxCNid";

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
            return dtable.Rows[0][0].ToString();
        else
            return null;

    }

    #endregion
    #region product setting

    PRODUCT_SETTING PSETEnt = new PRODUCT_SETTING();
    PRODUCT_SETTINGService PSETSer = new PRODUCT_SETTINGService();

    public string ProductExpiry()
    {
        PSETEnt = new PRODUCT_SETTING();
        PSETEnt = (PRODUCT_SETTING)PSETSer.GetSingle(PSETEnt);
        return PSETEnt.EXPIRY_DATE;
    }
    public string ProductBatch()
    {
        PSETEnt = new PRODUCT_SETTING();
        PSETEnt = (PRODUCT_SETTING)PSETSer.GetSingle(PSETEnt);
        return PSETEnt.BATCH_NUMBER;
    }
    public string ProductColor()
    {
        PSETEnt = new PRODUCT_SETTING();
        PSETEnt = (PRODUCT_SETTING)PSETSer.GetSingle(PSETEnt);
        return PSETEnt.PRODUCT_COLOUR;
    }
    public string ProductSize()
    {
        PSETEnt = new PRODUCT_SETTING();
        PSETEnt = (PRODUCT_SETTING)PSETSer.GetSingle(PSETEnt);
        return PSETEnt.PRODUCT_SIZE;
    }
    public string ProductManufacturer()
    {
        PSETEnt = new PRODUCT_SETTING();
        PSETEnt = (PRODUCT_SETTING)PSETSer.GetSingle(PSETEnt);
        return PSETEnt.PRODUCT_MANUFACTURER;
    }
    public string DualQuantity()
    {
        PSETEnt = new PRODUCT_SETTING();
        PSETEnt = (PRODUCT_SETTING)PSETSer.GetSingle(PSETEnt);
        return PSETEnt.DUAL_QUANTITY;
    }
    public string ShowDualQuantity()
    {
        PSETEnt = new PRODUCT_SETTING();
        PSETEnt = (PRODUCT_SETTING)PSETSer.GetSingle(PSETEnt);
        return PSETEnt.SHOW_DUAL_QUANTITY;
    }
    public string ItemWiseDiscount()
    {
        PSETEnt = new PRODUCT_SETTING();
        PSETEnt = (PRODUCT_SETTING)PSETSer.GetSingle(PSETEnt);
        return PSETEnt.ITEM_WISE_DISCOUNT;
    }
    public string MultipleRate()
    {
        PSETEnt = new PRODUCT_SETTING();
        PSETEnt = (PRODUCT_SETTING)PSETSer.GetSingle(PSETEnt);
        return PSETEnt.MULTIPLE_RATE;
    }
    public string SHOW_AVAILABILITY()
    {
        PSETEnt = new PRODUCT_SETTING();
        PSETEnt = (PRODUCT_SETTING)PSETSer.GetSingle(PSETEnt);
        return PSETEnt.SHOW_AVAILABILITY;
    }


    #endregion
    #region reports
    public DataTable getOpeningClosingBalance(string fiscalyear, string category_id, string subcategory_id, string product_code,
      string batch, string colour_id, string size_id, string manufacturer_id, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.OpeningClosingBalance";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = fiscalyear;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = category_id;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        if (subcategory_id != null)
            _p3.Value = subcategory_id;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        if (product_code != null)
            _p4.Value = product_code;
        objCmd.Parameters.Add(_p4);

        OracleParameter _p5 = new OracleParameter();
        _p5.Direction = ParameterDirection.Input;
        if (batch != null)
            _p5.Value = batch;
        objCmd.Parameters.Add(_p5);

        OracleParameter _p6 = new OracleParameter();
        _p6.Direction = ParameterDirection.Input;
        if (colour_id != null)
            _p6.Value = colour_id;
        objCmd.Parameters.Add(_p6);

        OracleParameter _p7 = new OracleParameter();
        _p7.Direction = ParameterDirection.Input;
        if (size_id != null)
            _p7.Value = size_id;
        objCmd.Parameters.Add(_p7);

        OracleParameter _p8 = new OracleParameter();
        _p8.Direction = ParameterDirection.Input;
        if (manufacturer_id != null)
            _p8.Value = manufacturer_id;
        objCmd.Parameters.Add(_p8);

        OracleParameter _p9 = new OracleParameter();
        _p9.Direction = ParameterDirection.Input;
        if (manufacturer_id != "")
            _p9.Value = office_code;
        objCmd.Parameters.Add(_p9);

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

    public DataTable getSalesReport(string customer_id, string from_date, string to_date, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.SalesReport";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        if (customer_id != null)
            _p1.Value = customer_id;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = from_date;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = to_date;
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
    public DataTable getSalesChallanReport(string customer_id, string from_date, string to_date)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.SalesChallanReport";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        if (customer_id != null)
            _p1.Value = customer_id;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = from_date;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = to_date;
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

    public DataTable getPurchaseChalanReport(string supplier_id, string from_date, string to_date)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.PurchaseChalanReport";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        if (supplier_id != null)
            _p1.Value = supplier_id;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = from_date;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = to_date;
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

    public DataTable getPurchaseReport(string supplier_id, string from_date, string to_date)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.PurchaseReport";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        if (supplier_id != null)
            _p1.Value = supplier_id;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = from_date;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = to_date;
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
    public DataTable getPurchaseList(string dakhila_fy, string dakhila_number, string from_date, string to_date, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.PURCHASE_LIST";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        if (dakhila_fy != "")
            _p1.Value = dakhila_fy;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        if (dakhila_number != "")
            _p2.Value = dakhila_number;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        if (from_date != "")
            _p3.Value = from_date;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        if (to_date != "")
            _p4.Value = to_date;
        objCmd.Parameters.Add(_p4);

        OracleParameter _p5 = new OracleParameter();
        _p5.Direction = ParameterDirection.Input;
        if (office_code != "")
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
    public DataTable getSalesList(string invoice_fy, string invoice_number, string from_date, string to_date, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.SALES_LIST";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        if (invoice_fy != "")
            _p1.Value = invoice_fy;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        if (invoice_number != "")
            _p2.Value = invoice_number;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        if (from_date != "")
            _p3.Value = from_date;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        if (to_date != "")
            _p4.Value = to_date;
        objCmd.Parameters.Add(_p4);

        OracleParameter _p5 = new OracleParameter();
        _p5.Direction = ParameterDirection.Input;
        if (office_code != "")
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

    public DataTable getExportSalesList(string invoice_fy, string invoice_number, string from_date, string to_date, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.EXPORT_SALES_LIST";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        if (invoice_fy != "")
            _p1.Value = invoice_fy;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        if (invoice_number != "")
            _p2.Value = invoice_number;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        if (from_date != "")
            _p3.Value = from_date;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        if (to_date != "")
            _p4.Value = to_date;
        objCmd.Parameters.Add(_p4);

        OracleParameter _p5 = new OracleParameter();
        _p5.Direction = ParameterDirection.Input;
        if (office_code != "")
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

    public DataTable getPurchaseReturnList(string debit_note_fy, string debit_note_number, string from_date, string to_date, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.PURCHASE_RETURN_LIST";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        if (debit_note_fy != "")
            _p1.Value = debit_note_fy;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        if (debit_note_number != "")
            _p2.Value = debit_note_number;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        if (from_date != "")
            _p3.Value = from_date;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        if (to_date != "")
            _p4.Value = to_date;
        objCmd.Parameters.Add(_p4);

        OracleParameter _p5 = new OracleParameter();
        _p5.Direction = ParameterDirection.Input;
        if (office_code != "")
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
    public DataTable getSalesReturnList(string debit_note_fy, string debit_note_number, string from_date, string to_date, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.SALES_RETURN_LIST";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        if (debit_note_fy != "")
            _p1.Value = debit_note_fy;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        if (debit_note_number != "")
            _p2.Value = debit_note_number;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        if (from_date != "")
            _p3.Value = from_date;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        if (to_date != "")
            _p4.Value = to_date;
        objCmd.Parameters.Add(_p4);

        OracleParameter _p5 = new OracleParameter();
        _p5.Direction = ParameterDirection.Input;
        if (office_code != "")
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
    public DataTable getAdjustmentReport(string category_id, string subcategory_id, string product_id, string from_date, string to_date, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.AdjustmentReport";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        if (category_id != null)
            _p1.Value = category_id;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        if (subcategory_id != null)
            _p2.Value = subcategory_id;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        if (product_id != null)
            _p3.Value = product_id;
        objCmd.Parameters.Add(_p3);

        OracleParameter _p4 = new OracleParameter();
        _p4.Direction = ParameterDirection.Input;
        _p4.Value = from_date;
        objCmd.Parameters.Add(_p4);

        OracleParameter _p5 = new OracleParameter();
        _p5.Direction = ParameterDirection.Input;
        _p5.Value = to_date;
        objCmd.Parameters.Add(_p5);

        OracleParameter _p6 = new OracleParameter();
        _p6.Direction = ParameterDirection.Input;
        if (office_code != "")
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
    public DataTable getStockOut(string fiscalyear, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.STOCK_OUT";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = fiscalyear;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = office_code;
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
    public DataTable get_ird_report(string fromdate, string todate, string is_exempted, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.get_ird_report";

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
        if (is_exempted != "All")
            _p3.Value = is_exempted;
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
    public DataTable getDebitNoteList(string from_date, string to_date, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.DEBIT_NOTE_LIST";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = from_date;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = to_date;
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
    public DataTable getDCreditNoteList(string from_date, string to_date, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.CREDIT_NOTE_LIST";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = from_date;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = to_date;
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
    public DataTable getOneLakhPlusTransaction(string month, string year, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.TRAN_ABOVE_ONE_LAKH";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = month;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = year;
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
    public DataTable getUserActivityLog(string fromDate, string toDate, string User, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.GET_USER_ACTIVITY_LOG";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = fromDate;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = toDate;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        _p3.Value = User;
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

    public DataTable getKHARID_KHATA(string month, string fiscal_year, string purchase_type, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.KHARID_KHATA";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = month;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = fiscal_year;
        objCmd.Parameters.Add(_p2);


        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        if (purchase_type != "")
            _p3.Value = purchase_type;
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
    public string getKHARID_KHATA_PRODUCT_DETAIL(string purchase_master_id)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.KHARID_KHATA_PRODUCT_DETAIL";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = purchase_master_id;
        objCmd.Parameters.Add(_p1);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        string p_detail = null;
        try
        {
            p_detail = dtable.Rows[0][0].ToString();
        }
        catch
        {

        }
        return p_detail;
    }

    public string getKHARID_KHATA_QUANTITY_DETAIL(string purchase_master_id)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.KHARID_KHATA_QUANTITY_DETAIL";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = purchase_master_id;
        objCmd.Parameters.Add(_p1);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        string qty = null;
        try
        {
            qty = dtable.Rows[0][0].ToString();
        }
        catch
        {

        }
        return qty;

    }

    public string getKHARID_KHATA_UNIT_DETAIL(string purchase_master_id)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.KHARID_KHATA_UNIT_DETAIL";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = purchase_master_id;
        objCmd.Parameters.Add(_p1);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        string unit = null;
        try
        {
            unit = dtable.Rows[0][0].ToString();
        }
        catch
        {

        }
        return unit;

    }
    public DataTable getCOST_CALCULATION(string pp_Number, string fiscal_year, string office_code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.COST_CALCULATION";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = pp_Number;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = fiscal_year;
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

    public string getKHARID_KHATA_TOTAL_QUANTITY(string purchase_master_id)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.KHARID_KHATA_TOTAL_QUANTITY";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = purchase_master_id;
        objCmd.Parameters.Add(_p1);

        OracleParameter result = new OracleParameter("result", OracleDbType.RefCursor);
        result.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(result);

        conn.Open();
        ora_reader = objCmd.ExecuteReader();
        dtable.Load(ora_reader);
        conn.Close();

        string qty = null;
        try
        {
            qty = dtable.Rows[0][0].ToString();
        }
        catch
        {

        }
        return qty;

    }


    public DataTable getBalanceSheet(string fiscalYear, string GL_Master_Code, string GL_Code)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_account_reports.GET_BALANCESHEET";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = fiscalYear;
        objCmd.Parameters.Add(_p1);

        OracleParameter _p2 = new OracleParameter();
        _p2.Direction = ParameterDirection.Input;
        _p2.Value = GL_Master_Code;
        objCmd.Parameters.Add(_p2);

        OracleParameter _p3 = new OracleParameter();
        _p3.Direction = ParameterDirection.Input;
        if (GL_Code != "")
            _p3.Value = GL_Code;
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
    #endregion

    public DataTable LoadSalesInvoice(string pk_id)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.LOAD_SALES_INVOICE";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = pk_id;
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
    public DataTable LoadPurchaseInvoice(string pk_id)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.LOAD_PURCHASE_INVOICE";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = pk_id;
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
    public DataTable LoadSalesReturnInvoice(string pk_id)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.LOAD_SALES_RETURN_INVOICE";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = pk_id;
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

    public DataTable LoadPurchaseReturnInvoice(string pk_id)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.LOAD_PURCHASE_RETURN_INVOICE";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = pk_id;
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
    #region unsink list
    public DataTable getUNSINK_INVOICE(string date)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_select.SELECT_UNSINK_INVOICE";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = date;
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

    public DataTable getUNSINK_CREDITNOTE(string date)
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_select.SELECT_UNSINK_CREDITNOTE";

        objCmd.Connection = conn;
        objCmd.CommandType = CommandType.StoredProcedure;

        OracleParameter _p1 = new OracleParameter();
        _p1.Direction = ParameterDirection.Input;
        _p1.Value = date;
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
    #endregion


    public DataTable LoadUnMappedDakhila()
    {
        OracleDataReader ora_reader;
        DataTable dtable = new DataTable();

        OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString);
        OracleCommand objCmd = new OracleCommand();
        objCmd.CommandText = "pkj_reports.UNMAPPED_DAKHILA";

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




}



//----------------------************************-----------------------------**********************----------------------------

public class ActiveProductType
{
    // Converts a generic list to a DataTable
    public static DataTable ListToDataTable<T>(List<T> list)
    {
        DataTable dt = new DataTable();
        if (list == null || list.Count == 0)
            return dt;

        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (PropertyInfo prop in properties)
        {
            dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        }

        foreach (T item in list)
        {
            DataRow row = dt.NewRow();
            foreach (PropertyInfo prop in properties)
            {
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }

        return dt;
    }

    // Fetches active product types based on SHREEYL flag
    public DataTable GetActiveProductType()
    {
        DataTable return_value = new DataTable();

        PRODUCT_TYPE PTEnt = new PRODUCT_TYPE();
        PRODUCT_TYPEService PTSer = new PRODUCT_TYPEService();

        NAME_COMPANY NCEnt = new NAME_COMPANY();
        NAME_COMPANYService NCSer = new NAME_COMPANYService();

        NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);

        if (NCEnt != null && NCEnt.IS_MANUFACTURER == "0")
        {
            IList rawList = PTSer.GetAll(PTEnt); // non-generic list
            List<PRODUCT_TYPE> list = rawList.Cast<PRODUCT_TYPE>().ToList(); // convert to generic

            // Optional: filter only active types
            var activeList = list.Where(x => x.PK_ID == "1").ToList();

            return_value = ListToDataTable(activeList);
        }
        else
        {
            {
                IList rawList = PTSer.GetAll(PTEnt); // non-generic list
                List<PRODUCT_TYPE> list = rawList.Cast<PRODUCT_TYPE>().ToList(); // convert to generic

                // Optional: filter only active types
                var activeList = list.Where(x => x.PK_ID != "1").ToList();

                return_value = ListToDataTable(activeList);
            }
        }

        return return_value;
    }
}
public class GridDecorator
{
    public static void MergeRows(GridView gridView, int TillCol)
    {
        for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = gridView.Rows[rowIndex];
            GridViewRow previousRow = gridView.Rows[rowIndex + 1];

            for (int cellIndex = 0; cellIndex < TillCol; cellIndex++)
            {
                if (row.Cells[cellIndex].Text == previousRow.Cells[cellIndex].Text)
                {
                    row.Cells[cellIndex].RowSpan = previousRow.Cells[cellIndex].RowSpan < 2 ? 2 : previousRow.Cells[cellIndex].RowSpan + 1;
                    previousRow.Cells[cellIndex].Visible = false;
                }
            }
        }
    }
}


