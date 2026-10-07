using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PhyeGanCore;

public partial class Reports_Purchase_PurchaseChallanReport : System.Web.UI.Page
{
    HelperFunction hf = new HelperFunction();
    PhyeGan pg = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtChalanDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        try
        {
            divhide.Visible = true;
            lblDate.Text = txtChalanDate.Text;
            string[] chalandate = txtChalanDate.Text.Split('/');
            string challanDate = PGD.ConvertNepaliTOEnglish(chalandate[0], chalandate[1], chalandate[2]);
            grdReport.DataSource = hf.getPurchaseReport(null, challanDate, challanDate);
            grdReport.DataBind();

            if (hf.ProductBatch() == "1")
                grdReport.Columns[6].Visible = true;
            else
                grdReport.Columns[6].Visible = false;

            if (hf.ProductExpiry() == "1")
                grdReport.Columns[7].Visible = true;
            else
                grdReport.Columns[7].Visible = false;

            if (hf.ProductColor() == "1")
                grdReport.Columns[8].Visible = true;
            else
                grdReport.Columns[8].Visible = false;

            if (hf.ProductSize() == "1")
                grdReport.Columns[9].Visible = true;
            else
                grdReport.Columns[9].Visible = false;

            if (hf.ProductManufacturer() == "1")
                grdReport.Columns[10].Visible = true;
            else
                grdReport.Columns[10].Visible = false;
        }
        catch
        {
            HelperFunction.MsgBox(this, this.GetType(), "Invalid Date");
        }
    }

  

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }
}