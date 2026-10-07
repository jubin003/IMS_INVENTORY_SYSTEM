using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Entity.Framework;
using Service.Components;
using PhyeGanCore;

public partial class Mapping_sales_Export_Issue_List : System.Web.UI.Page
{
    PURCHASE_INVOICE_MASTER EPim = new PURCHASE_INVOICE_MASTER();
    PURCHASE_INVOICE_MASTERService SPim = new PURCHASE_INVOICE_MASTERService();

    PURCHASE_SALES_MAPPING EPsm = new PURCHASE_SALES_MAPPING();
    PURCHASE_SALES_MAPPINGService SPsm = new PURCHASE_SALES_MAPPINGService();

    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

    SALES_INVOICE_MASTER ESalesinvoice = new SALES_INVOICE_MASTER();
    SALES_INVOICE_MASTERService SSalesinvoice = new SALES_INVOICE_MASTERService();

    CUSTOMER CUSEnt = new CUSTOMER();
    CUSTOMERService CUSSer = new CUSTOMERService();

    COUNTRY COUEnt = new COUNTRY();
    COUNTRYService COUSer = new COUNTRYService();

    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGan PG = new PhyeGan();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFY();
            detail.Visible = false;
        }
    }


    protected void LoadFY()
    {
        FYEnt = new FISCALYEAR();
        ddlFY.DataSource = FYSer.GetAll(FYEnt);
        ddlFY.DataTextField = "FISCAL_YEAR";
        ddlFY.DataValueField = "FISCAL_YEAR";
        ddlFY.DataBind();
        ddlFY.SelectedValue = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
    }


    protected void CheckInvoice()
    {
        string invoicename = txtInvoice.Text;
        string FY = ddlFY.SelectedItem != null ? ddlFY.SelectedItem.Text : string.Empty;

        hdnInvoiceId.Value = "";

        if (string.IsNullOrEmpty(invoicename))
            return;

        SALES_INVOICE_MASTER Esalesid = new SALES_INVOICE_MASTER();
        EntityList fylist = (EntityList)SSalesinvoice.GetAll(Esalesid);

        string invoiceid = null;

        foreach (SALES_INVOICE_MASTER invnum in fylist)
        {
            if (invnum.INVOICE_FY == FY && invnum.INVOICE_NUMBER == invoicename)
            {
                invoiceid = invnum.PK_ID;
                break;
            }
        }

        if (string.IsNullOrEmpty(invoiceid))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Invoice for this Fiscal year does not exist");
            hdnInvoiceId.Value = "";
            detail.Visible = false;
            grdDakhila.DataSource = null;
            grdDakhila.DataBind();
        }
        else
        {
            hdnInvoiceId.Value = invoiceid;
        }
    }


    protected void ddlFY_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckInvoice();

    }


    protected void txtInvoice_TextChanged(object sender, EventArgs e)
    {
        CheckInvoice();
        if (!string.IsNullOrEmpty(hdnInvoiceId.Value))
        {
            LoadDakhilaDetail(hdnInvoiceId.Value);
        }
    }


    protected void btnShow_Click(object sender, EventArgs e)
    {
        CheckInvoice();
        if (!string.IsNullOrEmpty(hdnInvoiceId.Value))
        {
            LoadDakhilaDetail(hdnInvoiceId.Value);
        }
    }


    protected void LoadDakhilaDetail(string invoiceid)
    {
        EntityList mappingList = (EntityList)SPsm.GetAll(new PURCHASE_SALES_MAPPING());
        EntityList allInvoiceMaster = (EntityList)SPim.GetAll(new PURCHASE_INVOICE_MASTER());

        EntityList dakilaIdList = new EntityList();
        foreach (PURCHASE_SALES_MAPPING mapping in mappingList)
        {
            if (mapping.INVOICE_ID == invoiceid)
            {
                PURCHASE_INVOICE_MASTER temp = new PURCHASE_INVOICE_MASTER();
                temp.PK_ID = mapping.DAKHILA_ID;
                dakilaIdList.Add(temp);
            }
        }

        EntityList dakiladetail = new EntityList();
        foreach (PURCHASE_INVOICE_MASTER item in allInvoiceMaster)
        {
            foreach (PURCHASE_INVOICE_MASTER dakhila in dakilaIdList)
            {
                if (dakhila.PK_ID == item.PK_ID)
                {
                    dakiladetail.Add(item);
                    break;
                }
            }
        }

        ESalesinvoice.PK_ID = invoiceid;
        ESalesinvoice = (SALES_INVOICE_MASTER)SSalesinvoice.GetSingle(ESalesinvoice);

        div_print.Visible = true;
        tblCDetail.Visible = true;

        LoadCompanyDetails();

        lblad.Text = ESalesinvoice.CUSTOMER_ADDRESS;
        lblcn.Text = ESalesinvoice.CUSTOMER_NAME;


        CUSEnt = new CUSTOMER();
        CUSEnt.PK_ID = ESalesinvoice.CUSTOMER_ID;
        CUSEnt = (CUSTOMER)CUSSer.GetSingle(CUSEnt);
        if (CUSEnt != null)
        {
            COUEnt = new COUNTRY();
            COUEnt.PK_ID = CUSEnt.COUNTRY;
            COUEnt = (COUNTRY)COUSer.GetSingle(COUEnt);
            if (COUEnt != null)
            {
                lblCountry.Text = COUEnt.COUNTRY_NAME;
            }
        }

        lblivn.Text = txtInvoice.Text;
        detail.Visible = true;

        grdDakhila.DataSource = dakiladetail;
        grdDakhila.DataBind();
    }


    protected void grdDakhila_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            PURCHASE_INVOICE_MASTER row = (PURCHASE_INVOICE_MASTER)e.Row.DataItem;

            Label lblDakhilano = (Label)e.Row.FindControl("lblDakhilano");
            Label lblSupplier = (Label)e.Row.FindControl("lblSupplier");
            Label lblDakhiladate = (Label)e.Row.FindControl("lblDakhiladate");
            Label lblAmount = (Label)e.Row.FindControl("lblAmount");

            lblDakhilano.Text = row.DAKHILA_NUMBER;
            lblSupplier.Text = row.SUPPLIER_NAME;
            lblDakhiladate.Text = row.DAKHILA_DATE;
            lblAmount.Text = row.TOTAL_AMOUNT;
        }
    }


    protected void LoadCompanyDetails()
    {
        lblCompanyName.Text = PG.CompanyName();
        lblCompanyAddress.Text = PG.CompanyAddress();
        lblWebsite.Text = PG.CompanyWebsite();
        lblEmail.Text = PG.CompanyEmail();
        lblPhone1.Text = PG.CompanyContact();
        lblPanNo.Text = PG.CompanyVATPan();
        lblHeading.Text = "Dakhila Details";
    }
}