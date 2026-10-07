using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PhyeGanCore;
using System.IO;
using Entity.Components;
using Service.Components;

public partial class Old_Report_Old_Materialized_View : System.Web.UI.Page
{
    HelperFunction hf = new HelperFunction();
    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();

    Boolean IsPageRefresh = false;

    UserProfileEntity userProfileEnt = new UserProfileEntity();

    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

    static string path = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFiscalYear();
        }
    }

    protected void LoadFiscalYear()
    {
        FYEnt = new FISCALYEAR();
        ddlFiscalYear.DataSource = FYSer.GetAll(FYEnt);
        ddlFiscalYear.DataValueField = "FISCAL_YEAR";
        ddlFiscalYear.DataTextField = "FISCAL_YEAR";
        ddlFiscalYear.DataBind();
        ddlFiscalYear.SelectedValue = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
    }

    protected void LoadCompanyDetail()
    {
        lblCompanyName.Text = PG.CompanyName();
        //if (ddlBranch.SelectedValue != "")
        //{
        //    lblCompanyAddress.Text = PG.BranchAddress(ddlBranch.SelectedValue);
        //}
        //else
        //{
        //    lblCompanyAddress.Text = "";
        //}
        lblRegNo.Text = PG.CompanyRegistration();
        lblPanNo.Text = PG.CompanyVATPan();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        string office_code = "";
        //if (ddlBranch.SelectedValue != "")
        //{
        //    office_code = ddlBranch.SelectedValue;
        //}
        LoadCompanyDetail();
        hide.Visible = true;

        //gridIRDReport.DataSource = hf.get_ird_report(PGD.GetEnglishDateFromNepali(txtFromDate.Text, "dd/mm/yyyy"), PGD.GetEnglishDateFromNepali(txtToDate.Text, "dd/mm/yyyy"), ddlExempted.SelectedValue, office_code);
        //gridIRDReport.DataBind();

        //lblFromDate.Text = txtFromDate.Text;
        //lblToDate.Text = txtToDate.Text;

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename=IRDReport_XLS" + "_" + PGD.GetTodayDate("dd/mm/yyyy") + ".xls");
        //Response.ContentEncoding = Encoding.UTF8; 
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        hide.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.End();
    }
    //protected void btnPdf_Click(object sender, EventArgs e)
    //{
    //    Response.ContentType = "application/pdf";
    //    Response.AddHeader("content-disposition", "attachment;filename=BillWiseCollection_PDF" + "_" + hf.GetTodayDate() + ".pdf");
    //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //    StringWriter sw = new StringWriter();
    //    HtmlTextWriter hw = new HtmlTextWriter(sw);
    //    gridBillWiseCollection.RenderControl(hw);
    //    StringReader sr = new StringReader(sw.ToString());
    //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
    //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
    //    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
    //    pdfDoc.Open();
    //    htmlparser.Parse(sr);
    //    pdfDoc.Close();
    //    Response.Write(pdfDoc);
    //    Response.End();
    //    gridBillWiseCollection.AllowPaging = true;
    //    gridBillWiseCollection.DataBind();
    //}
    protected void btnJson_Click(object sender, EventArgs e)
    {

        string json = hf.DataTableToJSON(hf.GridViewToDataTable(gridIRDReport));

        Response.ContentType = "application/text";
        Response.AddHeader("Content-Disposition", "attachment;filename=IRDReport_JSON" + "_" + PGD.GetTodayDate("dd/mm/yyyy") + ".txt");
        Response.Write(json);
        Response.End();
    }
    protected void btnXml_Click(object sender, EventArgs e)
    {
        string xml = hf.DataTableToXML(hf.GridViewToDataTable(gridIRDReport));

        Response.ContentType = "application/xml";
        Response.AddHeader("Content-Disposition", "attachment;filename=IRDReport_XML" + "_" + PGD.GetTodayDate("dd/mm/yyyy") + ".xml");
        Response.Write(xml);
        Response.End();
    }


    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }


    protected void gridIRDReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        #region Footer
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblFTotalAmount = (Label)e.Row.FindControl("lblFTotalAmount");
            Label lblTotalDiscount = (Label)e.Row.FindControl("lblTotalDiscount");
            Label lblTotalTaxableAmount = (Label)e.Row.FindControl("lblTotalTaxableAmount");
            Label lblTotalTaxAmount = (Label)e.Row.FindControl("lblTotalTaxAmount");
            Label lblTotalTotalAmount = (Label)e.Row.FindControl("lblTotalTotalAmount");
            Double TotalAmount = 0.0;
            Double TotalDiscount = 0.0;
            Double TotalTaxableAmount = 0.0;
            Double TotalTaxAmount = 0.0;
            Double TotalTotalAmount = 0.0;
            foreach (GridViewRow gr in gridIRDReport.Rows)
            {
                Label lblAmount = (Label)gr.FindControl("lblAmount");
                Label lblDiscount = (Label)gr.FindControl("lblDiscount");
                Label lblTaxableAmount = (Label)gr.FindControl("lblTaxableAmount");
                Label lblTaxAmount = (Label)gr.FindControl("lblTaxAmount");
                Label lblTotalAmount = (Label)gr.FindControl("lblTotalAmount");

                TotalAmount += Double.Parse(lblAmount.Text);
                TotalDiscount += Double.Parse(lblDiscount.Text);
                TotalTaxableAmount += Double.Parse(lblTaxableAmount.Text);
                TotalTaxAmount += Double.Parse(lblTaxAmount.Text);
                TotalTotalAmount += Double.Parse(lblTotalAmount.Text);
            }

            lblFTotalAmount.Text = TotalAmount.ToString("##0.00");
            lblTotalDiscount.Text = TotalDiscount.ToString("##0.00");
            lblTotalTaxableAmount.Text = TotalTaxableAmount.ToString("##0.00");
            lblTotalTaxAmount.Text = TotalTaxAmount.ToString("##0.00");
            lblTotalTotalAmount.Text = TotalTotalAmount.ToString("##0.00");

        }
        #endregion
    }
}