using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Entity.Framework;
using Service.Components;

public partial class Mapping_sales_Invoice_To_Dakhila : System.Web.UI.Page
{
    EntityList theList = new EntityList();
    PURCHASE_SALES_MAPPING EPsm = new PURCHASE_SALES_MAPPING();
    PURCHASE_SALES_MAPPINGService SPsm = new PURCHASE_SALES_MAPPINGService();
    PURCHASE_INVOICE_MASTER EPim = new PURCHASE_INVOICE_MASTER();
    PURCHASE_INVOICE_MASTERService SPim = new PURCHASE_INVOICE_MASTERService();
    SALES_INVOICE_MASTER ESalesinvoice = new SALES_INVOICE_MASTER();
    SALES_INVOICE_MASTERService SSalesinvoice = new SALES_INVOICE_MASTERService();
    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();
    PhyeGanDate PGD = new PhyeGanDate();

    private string CurrentDisplayedInvoiceId
    {
        get { return ViewState["CurrentDisplayedInvoiceId"] as string; }
        set { ViewState["CurrentDisplayedInvoiceId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFY();
            LoadFYdakh();
            string currentFY = ddlFYdakh.SelectedItem != null ? ddlFYdakh.SelectedItem.Text : string.Empty;
            LoadDakhila(currentFY);
            grdITDList.DataSource = null;
            grdITDList.DataBind();
        }
    }

    protected void LoadFY()
    {
        FYEnt = new FISCALYEAR();
        EntityList fyList = (EntityList)FYSer.GetAll(FYEnt);

        ddlFY.DataSource = fyList;
        ddlFY.DataTextField = "FISCAL_YEAR";
        ddlFY.DataValueField = "PK_ID";
        ddlFY.DataBind();
        ddlFY.Items.Insert(0, new ListItem("Select", ""));

        SelectCurrentFiscalYear(ddlFY, fyList);
    }

    protected void LoadFYdakh()
    {
        FYEnt = new FISCALYEAR();
        EntityList fyList = (EntityList)FYSer.GetAll(FYEnt);

        ddlFYdakh.DataSource = fyList;
        ddlFYdakh.DataTextField = "FISCAL_YEAR";
        ddlFYdakh.DataValueField = "PK_ID";
        ddlFYdakh.DataBind();
        ddlFYdakh.Items.Insert(0, new ListItem("Select", ""));

        SelectCurrentFiscalYear(ddlFYdakh, fyList);
    }

    private void SelectCurrentFiscalYear(DropDownList ddl, EntityList fyList)
    {
        string highestPkId = null;
        int highestValue = int.MinValue;

        foreach (FISCALYEAR row in fyList)
        {
            int currentValue;
            if (int.TryParse(row.PK_ID, out currentValue))
            {
                if (currentValue > highestValue)
                {
                    highestValue = currentValue;
                    highestPkId = row.PK_ID;
                }
            }
        }

        if (!string.IsNullOrEmpty(highestPkId))
        {
            ListItem item = ddl.Items.FindByValue(highestPkId);
            if (item != null)
            {
                ddl.SelectedValue = highestPkId;
            }
        }
    }

    protected void ddlFYdakh_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedFY = ddlFYdakh.SelectedItem != null ? ddlFYdakh.SelectedItem.Text : string.Empty;
        LoadDakhila(selectedFY);
    }

    protected void LoadDakhila(string FY)
    {
        PURCHASE_SALES_MAPPING allMappings = new PURCHASE_SALES_MAPPING();
        EntityList mappingList = (EntityList)SPsm.GetAll(allMappings);

        PURCHASE_INVOICE_MASTER allrows = new PURCHASE_INVOICE_MASTER();
        theList = SPim.GetAll(allrows);

        EntityList fyRows = new EntityList();

        foreach (PURCHASE_INVOICE_MASTER row in theList)
        {
            if (string.IsNullOrEmpty(row.DAKHILA_FY))
                continue;

            if (!string.IsNullOrEmpty(FY) && row.DAKHILA_FY != FY)
                continue;

            bool alreadyMapped = false;
            foreach (PURCHASE_SALES_MAPPING mapping in mappingList)
            {
                if (mapping.DAKHILA_ID == row.PK_ID)
                {
                    alreadyMapped = true;
                    break;
                }
            }

            if (!alreadyMapped)
            {
                fyRows.Add(row);
            }
        }

        ddlDakhila.DataSource = fyRows;
        ddlDakhila.DataTextField = "DAKHILA_NUMBER";
        ddlDakhila.DataValueField = "PK_ID";
        ddlDakhila.DataBind();
        ddlDakhila.Items.Insert(0, new ListItem("Select", ""));
    }


    //show all the mapped invoice and dakhilaaa for the invoiceee
    private void LoadMappedGridForInvoice(string invoiceId)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("MAPPING_ID");
        dt.Columns.Add("INVOICE_NUMBER");
        dt.Columns.Add("INVOICE_FY");
        dt.Columns.Add("DAKHILA_NUMBER");
        dt.Columns.Add("DAKHILA_FY");

        if (!string.IsNullOrEmpty(invoiceId))
        {
            PURCHASE_SALES_MAPPING allMappings = new PURCHASE_SALES_MAPPING();
            EntityList mappingList = (EntityList)SPsm.GetAll(allMappings);

            foreach (PURCHASE_SALES_MAPPING mapping in mappingList)
            {
                if (mapping.INVOICE_ID != invoiceId)
                    continue;

                SALES_INVOICE_MASTER invFilter = new SALES_INVOICE_MASTER();
                invFilter.PK_ID = mapping.INVOICE_ID;
                SALES_INVOICE_MASTER invoice = (SALES_INVOICE_MASTER)SSalesinvoice.GetSingle(invFilter);

                PURCHASE_INVOICE_MASTER dakhilaFilter = new PURCHASE_INVOICE_MASTER();
                dakhilaFilter.PK_ID = mapping.DAKHILA_ID;
                PURCHASE_INVOICE_MASTER dakhila = (PURCHASE_INVOICE_MASTER)SPim.GetSingle(dakhilaFilter);

                DataRow row = dt.NewRow();
                row["MAPPING_ID"] = mapping.PK_ID;
                row["INVOICE_NUMBER"] = invoice != null ? invoice.INVOICE_NUMBER : "";
                row["INVOICE_FY"] = invoice != null ? invoice.INVOICE_FY : "";
                row["DAKHILA_NUMBER"] = dakhila != null ? dakhila.DAKHILA_NUMBER : "";
                row["DAKHILA_FY"] = dakhila != null ? dakhila.DAKHILA_FY : "";
                dt.Rows.Add(row);
            }
        }

        grdITDList.DataSource = dt;
        grdITDList.DataBind();
    }

    protected void grdITDList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView rowData = (DataRowView)e.Row.DataItem;

            Label lblInvoicenum = (Label)e.Row.FindControl("lblInvoicenum");
            Label lblIFY = (Label)e.Row.FindControl("lblIFY");
            Label lblDakhilanum = (Label)e.Row.FindControl("lblDakhilanum");
            Label lblDFY = (Label)e.Row.FindControl("lblDFY");

            lblInvoicenum.Text = rowData["INVOICE_NUMBER"].ToString();
            lblIFY.Text = rowData["INVOICE_FY"].ToString();
            lblDakhilanum.Text = rowData["DAKHILA_NUMBER"].ToString();
            lblDFY.Text = rowData["DAKHILA_FY"].ToString();
        }
    }

    protected void btnMap_Click(object sender, EventArgs e)
    {
        string invoicename = txtInvoice.Text;
        string FY = ddlFY.SelectedItem.Text;
        string invoiceid = hdnInvoiceId.Value;

        if (string.IsNullOrEmpty(invoiceid))
        {
            lblInvoiceStatus.Text = "No invoice found with that number for the selected fiscal year.";
            return;
        }

        EPsm = new PURCHASE_SALES_MAPPING();
        EPsm.DAKHILA_ID = ddlDakhila.SelectedValue;
        EPsm.INVOICE_ID = invoiceid;
        EPsm.STATUS = "1";

        SPsm.Insert(EPsm);
        CurrentDisplayedInvoiceId = invoiceid;
        LoadMappedGridForInvoice(invoiceid);

        hdnInvoiceId.Value = "";
        lblInvoiceStatus.Text = "";



        string dakhilaFY = ddlFYdakh.SelectedItem != null ? ddlFYdakh.SelectedItem.Text : string.Empty;
        LoadDakhila(dakhilaFY);

        HelperFunction.MsgBox(this, this.GetType(), "Invoice and Dakhila mapped Successfully");
        checkinvoice();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Button btnDelete = (Button)sender;
        GridViewRow row = (GridViewRow)btnDelete.NamingContainer;

        Label lblMappingId = (Label)row.FindControl("lblMappingId");
        string mappingId = lblMappingId.Text;

        if (!string.IsNullOrEmpty(mappingId))
        {
            PURCHASE_SALES_MAPPING deleteEntity = new PURCHASE_SALES_MAPPING();
            deleteEntity.PK_ID = mappingId;
            SPsm.Delete(deleteEntity);
            LoadMappedGridForInvoice(CurrentDisplayedInvoiceId);

            string currentFY = ddlFYdakh.SelectedItem != null ? ddlFYdakh.SelectedItem.Text : string.Empty;
            LoadDakhila(currentFY);
        }
    }



    //check if invoice existss oorrr not in the certain fiscal yearr///

    protected void checkinvoice()
    {
        string invoicename = txtInvoice.Text;
        string FY = ddlFY.SelectedItem != null ? ddlFY.SelectedItem.Text : string.Empty;

        hdnInvoiceId.Value = "";
        lblInvoiceStatus.Text = "";

        if (string.IsNullOrEmpty(invoicename))
        {
            CurrentDisplayedInvoiceId = null;
            LoadMappedGridForInvoice(null);
            return;
        }

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
            CurrentDisplayedInvoiceId = null;
            LoadMappedGridForInvoice(null);
        }
        else
        {
            hdnInvoiceId.Value = invoiceid;
            CurrentDisplayedInvoiceId = invoiceid;
            LoadMappedGridForInvoice(invoiceid);
        }
    }
    protected void txtInvoice_TextChanged(object sender, EventArgs e)
    {
        checkinvoice();
    }

    protected void ddlFY_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkinvoice();
    }
}