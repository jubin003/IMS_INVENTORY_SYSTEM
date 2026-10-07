using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Entity.Components;
using Service.Components;

public partial class Reports_old_sales_old : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }



    protected void BindOldExportKhata()
    {
        using (OracleConnection con = new OracleConnection(
               ConfigurationManager.ConnectionStrings["cnMIS"].ConnectionString))
        {
            using (OracleCommand cmd = new OracleCommand("pkj_reports.GET_OLD_SALES", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("VAR_FISCAL_YEAR", OracleDbType.Varchar2).Value =
                    string.IsNullOrEmpty(ddlfy.SelectedValue)
                    ? (object)DBNull.Value
                    : ddlfy.SelectedValue;

                cmd.Parameters.Add("RESULT", OracleDbType.RefCursor)
                               .Direction = ParameterDirection.Output;

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvOldSales.DataSource = dt;
                gvOldSales.DataBind();
            }

        }
    }

    protected void btn_view_Click(object sender, EventArgs e)
    {
        BindOldExportKhata();
    }
}
