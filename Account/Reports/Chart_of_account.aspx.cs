
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.IO;
using PhyeGanCore;

public partial class Account_Chart_of_account : System.Web.UI.Page
{
    GL_ACCOUNT GLEnt = new GL_ACCOUNT();
    GL_ACCOUNTService GLSer = new GL_ACCOUNTService();

    GL_ACCOUNT_MASTER GMEnt = new GL_ACCOUNT_MASTER();
    GL_ACCOUNT_MASTERService GMSer = new GL_ACCOUNT_MASTERService();

    AccountFunction af = new AccountFunction();
    HelperFunction hf = new HelperFunction();

    PhyeGanDate PGD = new PhyeGanDate();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadGLAccountHead();
        }
    }
    protected void LoadGLAccountHead()
    {
        ddlAccountsHead.DataSource = af.getAccountHead();
        ddlAccountsHead.DataValueField = "ACCOUNTS_HEAD";
        ddlAccountsHead.DataTextField = "ACCOUNTS_HEAD";
        ddlAccountsHead.DataBind();
        ddlAccountsHead.Items.Insert(0, "Select");

    }
    protected void LoadGLAccountMaster()
    {
        GMEnt = new GL_ACCOUNT_MASTER();
        GMEnt.ACCOUNTS_HEAD = ddlAccountsHead.SelectedValue;
        ddlGLAccMaster.DataSource = GMSer.GetAll(GMEnt);
        ddlGLAccMaster.DataTextField = "GL_MASTER_NAME";
        ddlGLAccMaster.DataValueField = "GL_MASTER_CODE";
        ddlGLAccMaster.DataBind();
        ddlGLAccMaster.Items.Insert(0, "Select");
    }
    protected void ddlAccountsHead_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGLAccountMaster();
    }

    protected void LoadGLName()
    {
        GLEnt = new GL_ACCOUNT();
        GLEnt.STATUS = "1";
        GLEnt.GL_MASTER_CODE = ddlGLAccMaster.SelectedValue;
        ddlGLName.DataSource = GLSer.GetAll(GLEnt);
        ddlGLName.DataTextField = "GL_NAME";
        ddlGLName.DataValueField = "GL_CODE";
        ddlGLName.DataBind();
        ddlGLName.Items.Insert(0, "Select");
    }

    protected void LoadGrid()
    {
        if (ddlEnableSubledger.SelectedValue != "Enable")
        {
            divDetailedChart.Visible = false;
            divChart.Visible = true;
            if (ddlAccountsHead.SelectedValue == "Select")
            {
                gridChart.DataSource = af.getAccountChart("", "", "");
                gridChart.DataBind();
            }
            else if (ddlGLAccMaster.SelectedValue == "Select")
            {
                gridChart.DataSource = af.getAccountChart("", "", ddlAccountsHead.SelectedValue);
                gridChart.DataBind();
            }
            else if (ddlGLName.SelectedValue == "Select")
            {
                gridChart.DataSource = af.getAccountChart(ddlGLAccMaster.SelectedValue, "", ddlAccountsHead.SelectedValue);
                gridChart.DataBind();
            }
            else
            {
                gridChart.DataSource = af.getAccountChart(ddlGLAccMaster.SelectedValue, ddlGLName.SelectedValue, ddlAccountsHead.SelectedValue);
                gridChart.DataBind();
            }
        }
        else
        {
            divChart.Visible = false;
            divDetailedChart.Visible = true;
            if (ddlAccountsHead.SelectedValue == "Select")
            {
                gridChartOfAccDetailed.DataSource = af.getDetailAccountChart("", "", "");
                gridChartOfAccDetailed.DataBind();
            }
            else if (ddlGLAccMaster.SelectedValue == "Select")
            {
                gridChartOfAccDetailed.DataSource = af.getDetailAccountChart("", "", ddlAccountsHead.SelectedValue);
                gridChartOfAccDetailed.DataBind();
            }
            else if (ddlGLName.SelectedValue == "Select")
            {
                gridChartOfAccDetailed.DataSource = af.getDetailAccountChart(ddlGLAccMaster.SelectedValue, "", ddlAccountsHead.SelectedValue);
                gridChartOfAccDetailed.DataBind();
            }
            else
            {
                gridChartOfAccDetailed.DataSource = af.getDetailAccountChart(ddlGLAccMaster.SelectedValue, ddlGLName.SelectedValue, ddlAccountsHead.SelectedValue);
                gridChartOfAccDetailed.DataBind();
            }
        }
    }



    protected void btnView_Click(object sender, EventArgs e)
    {
        LoadGrid();
    }

    protected void ddlGLAccMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGLName();
    }

    protected void gridChartOfAccDetailed_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFinancialStatement = e.Row.FindControl("lblFinancialStatement") as Label;
            Label lblShowFinancialStatement = e.Row.FindControl("lblShowFinancialStatement") as Label;

            if (lblFinancialStatement.Text == "BS")
            {
                lblShowFinancialStatement.Text = "Balance Sheet";
            }
            else
            {
                lblShowFinancialStatement.Text = "Income Statement";
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            MergeRows();
        }
    }

    private void MergeRows()
    {
        for (int rowIndex = gridChartOfAccDetailed.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow currentRow = gridChartOfAccDetailed.Rows[rowIndex];
            GridViewRow nextRow = gridChartOfAccDetailed.Rows[rowIndex + 1];

            if (((Label)currentRow.FindControl("lblGlMasterCode")).Text == ((Label)nextRow.FindControl("lblGlMasterCode")).Text)
            {
                SetRowSpan(currentRow, nextRow, 0, "lblAccHeadCode");
                SetRowSpan(currentRow, nextRow, 1, "lblAccHead");
                SetRowSpan(currentRow, nextRow, 2, "lblShowFinancialStatement");
                SetRowSpan(currentRow, nextRow, 3, "lblGlMasterCode");
                SetRowSpan(currentRow, nextRow, 4, "lblGlMasterName");
                SetRowSpan(currentRow, nextRow, 5, "lblGlCode");
                SetRowSpan(currentRow, nextRow, 6, "lblGlname");
                SetRowSpan(currentRow, nextRow, 7, "lblSubLedCode");
                SetRowSpan(currentRow, nextRow, 8, "lblSubLedName");
            }
        }
    }
    protected void gridChart_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFinancialStatement = e.Row.FindControl("lblFinancialStatement") as Label;
            Label lblShowFinancialStatement = e.Row.FindControl("lblShowFinancialStatement") as Label;

            if (lblFinancialStatement.Text == "BS")
            {
                lblShowFinancialStatement.Text = "Balance Sheet";
            }
            else
            {
                lblShowFinancialStatement.Text = "Income Statement";
            }
        }
    }

    protected void gridChart_DataBound(object sender, EventArgs e)
    {
        for (int rowIndex = gridChart.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow currentRow = gridChart.Rows[rowIndex];
            GridViewRow nextRow = gridChart.Rows[rowIndex + 1];

            if (((Label)currentRow.Cells[3].FindControl("lblGlMasterCode")).Text == ((Label)nextRow.Cells[3].FindControl("lblGlMasterCode")).Text)
            {
                SetRowSpan(currentRow, nextRow, 0, "lblAccHeadCode");
                SetRowSpan(currentRow, nextRow, 1, "lblAccHead");
                SetRowSpan(currentRow, nextRow, 2, "lblShowFinancialStatement");
                SetRowSpan(currentRow, nextRow, 3, "lblGlMasterCode");
                SetRowSpan(currentRow, nextRow, 4, "lblGlMasterName");
                SetRowSpan(currentRow, nextRow, 5, "lblGlCode");
                SetRowSpan(currentRow, nextRow, 6, "lblGlname");
            }
        }

    }


    private void SetRowSpan(GridViewRow currentRow, GridViewRow nextRow, int cellIndex, string labelId)
    {
        Label currentLabel = currentRow.FindControl(labelId) as Label;
        Label nextLabel = nextRow.FindControl(labelId) as Label;

        if (currentLabel != null && nextLabel != null && currentLabel.Text == nextLabel.Text)
        {
            if (nextRow.Cells[cellIndex].RowSpan < 2)
            {
                currentRow.Cells[cellIndex].RowSpan = 2;
            }
            else
            {
                currentRow.Cells[cellIndex].RowSpan = nextRow.Cells[cellIndex].RowSpan + 1;
            }

            nextRow.Cells[cellIndex].Visible = false;
        }
    }




    protected void ddlEnableSubledger_SelectedIndexChanged(object sender, EventArgs e)
    {
        //UpdateGridColumnsVisibility();
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename=ChartOfAccounts_XLS" + "_" + PGD.GetTodayDate("dd/mm/yyyy") + ".xls");
        //Response.ContentEncoding = Encoding.UTF8; 
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        hide.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}