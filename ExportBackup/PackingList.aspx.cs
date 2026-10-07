using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;


public partial class ExportBackup_PackingList : System.Web.UI.Page
{
    PACKING_LIST PLEnt = new PACKING_LIST();
    PACKING_LISTService PLSer = new PACKING_LISTService();

    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    SALES_INVOICE_MASTER SIMEnt = new SALES_INVOICE_MASTER();
    SALES_INVOICE_MASTERService SIMSer = new SALES_INVOICE_MASTERService();

    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

    PhyeGan PG = new PhyeGan();

    PhyeGanDate PGD = new PhyeGanDate();

    EXPORT_INVOICE_MASTER EIMEnt = new EXPORT_INVOICE_MASTER();
    EXPORT_INVOICE_MASTERService EISer = new EXPORT_INVOICE_MASTERService();
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
    protected void LoadPackingListgrid()
    {
        PLEnt = new PACKING_LIST();
        PLEnt.INVOICE_ID = lblSALES_PK_ID.Text;
        gridPackingList.DataSource = PLSer.GetAll(PLEnt);
        gridPackingList.DataBind();

        grdPck.DataSource = PLSer.GetAll(PLEnt);
        grdPck.DataBind();
    }

    protected void gridPackingList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblProNamePL = (Label)e.Row.FindControl("lblProNamePL");

            PEnt = new PRODUCT();
            PEnt.PK_ID = lblProNamePL.Text;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);

            if (PEnt != null)
                lblProNamePL.Text = PEnt.PRODUCT_NAME;
            else
                lblProNamePL.Text = lblProNamePL.Text;
        }
    }

    protected void LoadtoPrint(string pk_id)
    {

        lblCompanyName.Text = PG.CompanyName();
        lblCompanyAddress.Text = PG.CompanyAddress();
        lblWebsite.Text = PG.CompanyWebsite();
        lblEmail.Text = PG.CompanyEmail();
        lblPhone1.Text = PG.CompanyContact();
        lblPanNo.Text = PG.CompanyVATPan();
        lblInvoiceHeading.Text = "PACKING LIST";
        lblCustomerName.Text = SIMEnt.CUSTOMER_NAME;
        lblCustomerAddress.Text = SIMEnt.CUSTOMER_ADDRESS;
        lblCustomerPanNo.Text = SIMEnt.COSTOMER_PAN_VAT;
        lblTranDate.Text = SIMEnt.TRANSACTION_DATE;
        lblTranNepaliDate.Text = SIMEnt.TRANSACTION_DAY + "/" + SIMEnt.TRANSACTION_MONTH + "/" + SIMEnt.TRANSACTION_YEAR;
        lblBillEnglishDate.Text = SIMEnt.INVOICE_DATE;
        lblBillNepaliDate.Text = SIMEnt.INVOICE_DAY + "/" + SIMEnt.INVOICE_MONTH + "/" + SIMEnt.INVOICE_YEAR;
        lblInvoiceNo.Text = txtInvoiceNo.Text;
        lblCompanyNameL.Text = PG.CompanyName();


        LoadPackingListgrid();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.INVOICE_NUMBER = txtInvoiceNo.Text.Trim();
        SIMEnt.INVOICE_FY = ddlFiscalYear.SelectedValue;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);

        if (SIMEnt != null)
        {
            lblSALES_PK_ID.Text = SIMEnt.PK_ID;
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Invoice number not found in this Fiscal Year");
            return;
        }


        LoadtoPrint(lblSALES_PK_ID.Text);
        ShowTotalQty(grdPck);
        ShowTotalQty(gridPackingList);

        imgn.ImageUrl = "~/images/BarCode/Invoice/" + "18693" + ".jpg";


    }

    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {

        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.INVOICE_NUMBER = txtInvoiceNo.Text;
        SIMEnt.INVOICE_FY = ddlFiscalYear.SelectedValue;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);

        if (SIMEnt == null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Invoice number not found in this Fiscal Year");
            txtInvoiceNo.Text = "";
            return;
        }
        lblSALES_PK_ID.Text = SIMEnt.PK_ID;
        LoadPackingListgrid();
        //divPackingGridOnly.Style["display"] = "block";
        //divPackingGridOnly.Style["display"] = "block";

        ShowTotalQty(grdPck);
        ShowTotalQty(gridPackingList);
    }

    protected void ddlFiscalYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        SIMEnt = new SALES_INVOICE_MASTER();
        SIMEnt.INVOICE_NUMBER = txtInvoiceNo.Text;
        SIMEnt.INVOICE_FY = ddlFiscalYear.SelectedValue;
        SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);

        if (SIMEnt == null)
        {
            HelperFunction.MsgBox(this, this.GetType(), "Invoice number not found in this Fiscal Year");
            ddlFiscalYear.SelectedValue = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
            return;
        }

        LoadPackingListgrid();
    }

    protected void ShowTotalQty(GridView gv)
    {
        GridViewRow previousRow = null;
        string FRow = "";
        double Count = 0;

        foreach (GridViewRow gr in gv.Rows)
        {
            Label lblBoxNoPL = gr.FindControl("lblBoxNoPL") as Label;
            Label lblQTYPL = gr.FindControl("lblQTYPL") as Label;

            string currentBox = lblBoxNoPL.Text;

            if (FRow == "")
            {
                FRow = currentBox;
                Count = Convert.ToDouble(lblQTYPL.Text);
            }
            else if (FRow == currentBox)
            {
                Count += Convert.ToDouble(lblQTYPL.Text);
            }
            else
            {
                Label previousTotal = previousRow.FindControl("lblTotalQtyPL") as Label;
                previousTotal.Text = Count.ToString();

                Button btnPrint = previousRow.FindControl("btnPrint") as Button;
                if (btnPrint != null) btnPrint.Visible = true;

                FRow = currentBox;
                Count = Convert.ToDouble(lblQTYPL.Text);
            }

            previousRow = gr;
        }

        if (previousRow != null)
        {
            Label lastTotal = previousRow.FindControl("lblTotalQtyPL") as Label;
            lastTotal.Text = Count.ToString();

            Button btnPrint = previousRow.FindControl("btnPrint") as Button;
            if (btnPrint != null) btnPrint.Visible = true;
        }
    }

    protected void gridPackingList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "printClick")
        {
            lblPackingNo.Text = Convert.ToString(e.CommandArgument);
            EXPORT_INVOICE_MASTER EIMEnt = new EXPORT_INVOICE_MASTER();
            EIMEnt.INVOICE_NUMBER = txtInvoiceNo.Text;
            EIMEnt.INVOICE_FY = ddlFiscalYear.SelectedValue;
            EIMEnt = (EXPORT_INVOICE_MASTER)EISer.GetSingle(EIMEnt);

            if (EIMEnt == null)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Invoice number not found in this Fiscal Year");
                return;
            }

            lblProCompanyName.Text = PG.CompanyName();
            lblProCompanyAddress.Text = PG.CompanyAddress();
            lblProCompanyEmail.Text = PG.CompanyEmail();
            lblProCompanyRegNo.Text = PG.CompanyRegistration();
            lblProEximCode.Text = PG.EXIMCODE();

            lblProInvoiceNo.Text = EIMEnt.INVOICE_NUMBER;
            //lblProIssueDate.Text = EIMEnt.INVOICE_DATE;
            //lblProContractNo.Text = EIMEnt.CONTRACT_NUMBER;
            //lblProContractDate.Text = EIMEnt.CONTRACT_DATE;


            SALES_INVOICE_MASTER SIMEnt = new SALES_INVOICE_MASTER();
            SIMEnt.INVOICE_NUMBER = txtInvoiceNo.Text;
            SIMEnt.INVOICE_FY = ddlFiscalYear.SelectedValue;
            SIMEnt = (SALES_INVOICE_MASTER)SIMSer.GetSingle(SIMEnt);
            if (SIMEnt != null)
            {
                lblProCustomerName.Text = SIMEnt.CUSTOMER_NAME;
                lblProCustomerAddress.Text = SIMEnt.CUSTOMER_ADDRESS;
            }



            Image1.ImageUrl = "~/images/BarCode/Invoice/" + "18693" + ".jpg";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "printSticker", "printSticker();", true);
        }
    }




}

