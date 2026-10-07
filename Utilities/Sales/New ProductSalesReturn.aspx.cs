using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;
using System.Collections;
using System.Data;

public partial class Utilities_Sales_New_ProductSales : System.Web.UI.Page
{
    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();

    PRODUCT_UNIT PUEnt = new PRODUCT_UNIT();
    PRODUCT_UNITService PUSer = new PRODUCT_UNITService();

    SALES_RETURN_DETAIL SRDEnt = new SALES_RETURN_DETAIL();
    SALES_RETURN_DETAILService SRDSer = new SALES_RETURN_DETAILService();

    SALES_RETURN_MASTER SRMEnt = new SALES_RETURN_MASTER();
    SALES_RETURN_MASTERService SRMSer = new SALES_RETURN_MASTERService();

    CUSTOMER CEnt = new CUSTOMER();
    CUSTOMERService CSer = new CUSTOMERService();

    AccountFunction af = new AccountFunction();

    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGan PG = new PhyeGan();
    PhyeGanProductSetting PGPS = new PhyeGanProductSetting();
    HelperFunction hf = new HelperFunction();
    Boolean flag = false;
    Boolean IsPageRefresh = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetVisibilty();
            loadProductName();
            LoadCustomers();
            LoadProductUnit();
            CreateGridFirst();
            txtFiscalYear.Text = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
            txtReturnDate.Text = PGD.GetTodayNepaliDate();
        }

    }
    protected void loadProductName()
    {
        PEnt = new PRODUCT();
        ddlProductName.DataSource = PSer.GetAll(PEnt);
        ddlProductName.DataTextField = "PRODUCT_NAME";
        ddlProductName.DataValueField = "PK_ID";
        ddlProductName.DataBind();
        ddlProductName.Items.Insert(0, "SELECT");
    }

    protected void LoadCustomers()
    {
        CEnt = new CUSTOMER();
        CEnt.STATUS = "1";
        ddlCustomer.DataSource = CSer.GetAll(CEnt);
        ddlCustomer.DataTextField = "CUSTOMER_FULLNAME";
        ddlCustomer.DataValueField = "PK_ID";
        ddlCustomer.DataBind();
        ddlCustomer.Items.Insert(0, "SELECT");
    }

    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        txtProductCode.Text = txtProductCode.Text.ToUpper();
        PEnt = new PRODUCT();
        PEnt.PRODUCT_CODE = txtProductCode.Text;
        PEnt = (PRODUCT)PSer.GetSingle(PEnt);
        if (PEnt != null && txtProductCode.Text != "")
        {
            ddlProductName.SelectedValue = PEnt.PK_ID;
            txtQty.Focus();
            ddlUnit.SelectedValue = PEnt.UNIT_ID;
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Not a valid Product Code.");
            txtProductCode.Text = "";
            txtProductCode.Focus();
            ddlProductName.SelectedValue = "Select";
            ddlUnit.SelectedValue = "";
        }

    }

    protected void ddlProductName_SelectedIndexChanged(object sender, EventArgs e)
    {
        PEnt = new PRODUCT();
        if (ddlProductName.SelectedValue != "Select")
        {
            PEnt.PK_ID = ddlProductName.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                txtProductCode.Text = PEnt.PRODUCT_CODE;
                LoadBatch(txtProductCode.Text);
                //loadRate();

                string batch = null;
                if (ddlBatch.SelectedValue != "")
                {
                    batch = ddlBatch.SelectedValue;
                }

                LoadAvailablity(ddlProductName.SelectedValue, batch);
                if (PEnt.DUAL_UNIT == "1")
                {
                    txtUQty.Enabled = true;
                    txtUQty.Focus();
                    lblUUnit.Text = getUnitName(PEnt.UPPER_UNIT_ID);
                }
                else
                {
                    txtUQty.Enabled = false;
                    txtQty.Focus();
                    txtUQty.Text = "";
                    lblUUnit.Text = "";
                }
                lblBUnit.Text = getUnitName(PEnt.UNIT_ID);
            }
        }
        else
        {
            txtProductCode.Text = "";
            txtProductCode.Focus();
            lblUUnit.Text = "";
            lblBUnit.Text = "";
        }
        PEnt = new PRODUCT();
        if (ddlProductName.SelectedValue != "Select")
        {
            PEnt.PK_ID = ddlProductName.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                txtProductCode.Text = PEnt.PRODUCT_CODE;
                LoadBatch(ddlProductName.SelectedValue);
                //loadRate();

                string batch = null;
                if (ddlBatch.SelectedValue != "")
                {
                    batch = ddlBatch.SelectedValue;
                }

                LoadAvailablity(ddlProductName.SelectedValue, batch);

            }
            else
            {
                txtProductCode.Text = "";
                txtProductCode.Focus();
                ddlUnit.SelectedValue = "";
            }
        }
    }
    protected string getUnitName(string PK_ID)
    {
        string unit_name = "";
        PUEnt = new PRODUCT_UNIT();
        PUEnt.PK_ID = PK_ID;
        PUEnt = (PRODUCT_UNIT)PUSer.GetSingle(PUEnt);
        if (PUEnt != null && PK_ID != "")
            unit_name = PUEnt.UNIT_NAME;
        return unit_name;
    }
    protected void LoadProductUnit()
    {
        PUEnt = new PRODUCT_UNIT();
        ddlUnit.DataSource = PUSer.GetAll(PUEnt);
        ddlUnit.DataTextField = "UNIT_NAME";
        ddlUnit.DataValueField = "PK_ID";
        ddlUnit.DataBind();
        ddlUnit.Items.Insert(0, "");
    }

    protected void LoadBatch(string product)
    {
        ArrayList batch_array = new ArrayList();
        DataTable DT;
        string batch = null;
        if (ddlBatch.SelectedValue != "")
            batch = ddlBatch.SelectedValue;
        DT = hf.getOpeningClosingBalance(PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), null, null, product, null, null, null, null,"");

        if (DT != null && DT.Rows.Count > 0)
        {
            int columnIndex = DT.Columns.IndexOf("batch_number");
            // Check if the column exists
            if (columnIndex != -1)
            {
                // Iterate through the rows of the DataTable and retrieve the values in the specified column
                foreach (DataRow row in DT.Rows)
                {
                    // Access the value in the specified column using the column index
                    object columnValue = row[columnIndex];
                    batch_array.Add(columnValue.ToString());
                }
            }
        }
        ddlBatch.DataSource = batch_array;
        ddlBatch.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        double Qty = 0;
        double rate = 0;
        string msg = "";
        try
        {
            Qty = Convert.ToDouble(txtQty.Text);
        }
        catch
        {
            msg = "Enter Number Only. ";
            txtQty.Focus();
            txtQty.Text = "";
        }
        try
        {
            rate = Convert.ToDouble(txtRate.Text);
        }
        catch
        {
            msg = "Enter Number Only. ";
            txtRate.Focus();
            txtRate.Text = "";
        }
        if (ddlProductName.SelectedItem.ToString() == "Select")
        {
            msg = msg + "Select Product";
        }
        if (msg == "")
        {
            CreateGrid(1, true);
            ClearProduct();
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), msg);
        }
    }
    protected DataTable CreateGrid(int RowIndex, bool addRemoveFlag)
    {
        int row = grdProdSaleReturn.Rows.Count;
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("PRODUCT_CODE");
        dummyTable.Columns.Add("PRODUCT");
        dummyTable.Columns.Add("BATCH");
        dummyTable.Columns.Add("EXPIRY_DATE");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("U_QUANTITY");
        dummyTable.Columns.Add("U_UNIT");
        dummyTable.Columns.Add("SCHE_DISC");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("TOTAL");
        dummyTable.Columns.Add("AFTER_SCHE_DISC");

        if (row > 0)
        {
            foreach (GridViewRow r in grdProdSaleReturn.Rows)
            {
                Label lblProductPK_ID = r.FindControl("lblProductPK_ID") as Label;
                Label lblProductCode = r.FindControl("lblProductCode") as Label;
                Label lblProductName = r.FindControl("lblProductName") as Label;
                Label lblQty = r.FindControl("lblQty") as Label;
                Label lblBatch = r.FindControl("lblBatch") as Label;
                Label lblExpDate = r.FindControl("lblExpDate") as Label;
                Label lblUQty = r.FindControl("lblUQty") as Label;
                Label lblUUnit = r.FindControl("lblUUnit") as Label;
                Label lblDiscount = r.FindControl("lblDiscount") as Label;
                Label lblUnit = r.FindControl("lblUnit") as Label;
                Label lblRate = r.FindControl("lblRate") as Label;
                Label lblItemTotal = r.FindControl("lblItemTotal") as Label;
                Label lblAfterScheDisc = r.FindControl("lblAfterScheDisc") as Label;

                DataRow dummyRw = dummyTable.NewRow();

                if (lblProductPK_ID.Text == ddlProductName.SelectedValue && lblRate.Text == txtRate.Text && lblUQty.Text == txtUQty.Text) // the selected product is already in the grid update the qty and rate
                {
                    dummyRw["PK_ID"] = lblProductPK_ID.Text;
                    dummyRw["PRODUCT_CODE"] = lblProductCode.Text;
                    dummyRw["PRODUCT"] = lblProductName.Text;
                    dummyRw["BATCH"] = lblBatch.Text;
                    dummyRw["EXPIRY_DATE"] = lblExpDate.Text;
                    dummyRw["U_QUANTITY"] = lblUQty.Text;
                    dummyRw["U_UNIT"] = lblUUnit.Text;
                    dummyRw["SCHE_DISC"] = lblDiscount.Text;
                    dummyRw["QUANTITY"] = txtQty.Text;
                    dummyRw["UNIT"] = hf.getProductUnit(lblProductPK_ID.Text);
                    dummyRw["RATE"] = Convert.ToDouble(txtRate.Text).ToString("0.00"); ;
                    dummyRw["TOTAL"] = Convert.ToDouble(txtAmount.Text).ToString("0.00"); ;
                    dummyRw["AFTER_SCHE_DISC"] = lblAfterScheDisc.Text;

                    flag = true;
                }

                else // the selected product is not in the grid add the product detail in the grid
                {
                    dummyRw["PK_ID"] = lblProductPK_ID.Text;
                    dummyRw["PRODUCT_CODE"] = lblProductCode.Text;
                    dummyRw["PRODUCT"] = lblProductName.Text;
                    dummyRw["BATCH"] = lblBatch.Text;
                    dummyRw["EXPIRY_DATE"] = lblExpDate.Text;
                    dummyRw["SCHE_DISC"] = lblDiscount.Text;
                    dummyRw["QUANTITY"] = lblQty.Text;
                    dummyRw["UNIT"] = hf.getProductUnit(lblProductPK_ID.Text);
                    dummyRw["RATE"] = Convert.ToDouble(lblRate.Text).ToString("0.00"); ;
                    dummyRw["AFTER_SCHE_DISC"] = lblAfterScheDisc.Text;
                    dummyRw["TOTAL"] = Convert.ToDouble(lblItemTotal.Text).ToString("0.00");
                }

                dummyTable.Rows.Add(dummyRw);

                //  flag = false;

                if (r.RowIndex == RowIndex && addRemoveFlag == false)
                {
                    dummyTable.Rows.Remove(dummyRw);
                    flag = true;
                }
            }
        }

        if (flag == false)
        {
            DataRow dummyRow = dummyTable.NewRow();

            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlProductName.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                dummyRow["PK_ID"] = PEnt.PK_ID;
                dummyRow["PRODUCT_CODE"] = PEnt.PRODUCT_CODE;
                dummyRow["PRODUCT"] = PEnt.PRODUCT_NAME;
                dummyRow["BATCH"] = ddlBatch.SelectedItem.Text;
                dummyRow["EXPIRY_DATE"] = "";
                dummyRow["U_QUANTITY"] = txtUQty.Text;
                if (txtUQty.Text != "" && txtUQty.Text != "0")
                    dummyRow["U_UNIT"] = getUnitName(PEnt.UPPER_UNIT_ID);
                dummyRow["QUANTITY"] = txtQty.Text;
                dummyRow["UNIT"] = hf.getProductUnit(PEnt.PK_ID);
                dummyRow["RATE"] = Convert.ToDouble(txtRate.Text).ToString("0.00");
                dummyRow["TOTAL"] = Convert.ToDouble(txtAmount.Text).ToString("0.00"); ;
                double sd = 0;
                try
                {
                    sd = Convert.ToDouble(txtScheDisc.Text);
                }
                catch { }
                dummyRow["SCHE_DISC"] = sd.ToString();
                dummyRow["AFTER_SCHE_DISC"] = txtScheDisc.Text;
                double asd = 0;
                try
                {
                    asd = Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtScheDisc.Text);
                    dummyRow["AFTER_SCHE_DISC"] = asd.ToString();
                }
                catch
                {
                    dummyRow["AFTER_SCHE_DISC"] = txtAmount.Text;
                }
                dummyTable.Rows.Add(dummyRow);
            }
        }

        DataView dv = new DataView(dummyTable);

        grdProdSaleReturn.DataSource = dummyTable;
        grdProdSaleReturn.DataBind();

        getTotal();
        return dummyTable;
    }
    protected DataTable CreateGridFirst()
    {
        int row = grdProdSaleReturn.Rows.Count;
        DataTable dummyTable = new DataTable();
        dummyTable.Columns.Add("PK_ID");
        dummyTable.Columns.Add("PRODUCT_CODE");
        dummyTable.Columns.Add("PRODUCT");
        dummyTable.Columns.Add("BATCH_NO");
        dummyTable.Columns.Add("EXPIRY_DATE");
        dummyTable.Columns.Add("U_QUANTITY");
        dummyTable.Columns.Add("U_UNIT");
        dummyTable.Columns.Add("QUANTITY");
        dummyTable.Columns.Add("UNIT");
        dummyTable.Columns.Add("RATE");
        dummyTable.Columns.Add("TOTAL");
        dummyTable.Columns.Add("SCHE_DISC");
        dummyTable.Columns.Add("AFTER_SCHE_DISC");


        DataRow dummyRw = dummyTable.NewRow();
        dummyRw["PK_ID"] = "";
        dummyRw["PRODUCT_CODE"] = "";
        dummyRw["PRODUCT"] = "";
        dummyRw["EXPIRY_DATE"] = "";
        dummyRw["BATCH_NO"] = "";
        dummyRw["U_QUANTITY"] = "";
        dummyRw["U_UNIT"] = "";
        dummyRw["QUANTITY"] = "";
        dummyRw["UNIT"] = "";
        dummyRw["RATE"] = "";
        dummyRw["TOTAL"] = "";
        dummyRw["SCHE_DISC"] = "";
        dummyRw["AFTER_SCHE_DISC"] = "";

        DataView dv = new DataView(dummyTable);
        grdProdSaleReturn.DataSource = dv;
        grdProdSaleReturn.DataBind();
        return dummyTable;
    }
    protected void LoadAvailablity(string product, string batch)
    {
        DataTable DT;
        DT = hf.getOpeningClosingBalance(PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear()), null, null, product, batch, null, null, null,"");

        if (DT != null && DT.Rows.Count > 0)
        {
            int closing_columnIndex = DT.Columns.IndexOf("CLOSING");
            // Check if the column exists
            if (closing_columnIndex != -1)
            {
                // Iterate through the rows of the DataTable and retrieve the values in the specified column
                foreach (DataRow row in DT.Rows)
                {
                    // Access the value in the specified column using the column index
                    object columnValue = row[closing_columnIndex];
                    txtAvilableQty.Text = columnValue.ToString();
                }
            }
            int expiry_columnIndex = DT.Columns.IndexOf("expiry_date");
            // Check if the column exists
            if (expiry_columnIndex != -1)
            {
                // Iterate through the rows of the DataTable and retrieve the values in the specified column
                foreach (DataRow row in DT.Rows)
                {
                    // Access the value in the specified column using the column index
                    object columnValue = row[expiry_columnIndex];
                    txtExpDate.Text = columnValue.ToString();
                }
            }
        }
        else
        {
            txtAvilableQty.Text = "0";
        }
    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        string batch = null;
        if (ddlBatch.SelectedValue != "")
            batch = ddlBatch.SelectedValue;
        LoadAvailablity(txtProductCode.Text, batch);
    }
    protected void LoadQty()
    {
        double Qty = 0;
        double rate = 0;
        string msg = "";
        try
        {
            Qty = Convert.ToDouble(txtQty.Text);
            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlProductName.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                if (PEnt.DUAL_UNIT == "1")
                    txtUQty.Text = (Convert.ToDouble(txtQty.Text) / Convert.ToDouble(PEnt.PACK_QTY)).ToString("0.##");
                txtRate.Focus();
                if (Convert.ToDouble(txtAvilableQty.Text) - Convert.ToDouble(txtQty.Text) < 0)
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Available qty is less then sales qty. Are you sure to make invoice?");
                }
            }
        }
        catch
        {
            msg = "Enter Number Only. ";
            txtQty.Focus();
            txtQty.Text = "";
            HelperFunction.MsgBox(this, this.GetType(), msg);
        }
        try
        {
            rate = Convert.ToDouble(txtRate.Text);
            txtAmount.Text = (Qty * rate).ToString("#0.00");
        }
        catch
        {
            txtRate.Focus();
            txtRate.Text = "";
        }
    }
    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        LoadQty();

    }

    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        double Qty = 0;
        double rate = 0;
        string msg = "";
        try
        {
            Qty = Convert.ToDouble(txtQty.Text);
        }
        catch
        {
            txtQty.Focus();
            txtQty.Text = "";
        }
        try
        {
            rate = Convert.ToDouble(txtRate.Text);
            txtRate.Text = Convert.ToDouble(txtRate.Text).ToString("#0.00");
        }
        catch
        {
            msg = "Enter Number Only. ";
            txtRate.Focus();
            txtRate.Text = "";
            HelperFunction.MsgBox(this, this.GetType(), msg);
        }
        txtAmount.Text = (Qty * rate).ToString("#0.00");
    }

    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        double Qty = 0;
        double amount = 0;
        string msg = "";
        try
        {
            Qty = Convert.ToDouble(txtQty.Text);
        }
        catch
        {
            txtQty.Focus();
            txtQty.Text = "";
        }
        try
        {
            amount = Convert.ToDouble(txtAmount.Text);
            txtAmount.Text = Convert.ToDouble(txtAmount.Text).ToString("#0.00");
        }
        catch
        {
            msg = "Enter Number Only. ";
            txtAmount.Focus();
            txtAmount.Text = "";
            HelperFunction.MsgBox(this, this.GetType(), msg);
        }
        txtRate.Text = (amount / Qty).ToString("#0.00");
    }

    protected void grdProdSaleReturn_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Remove"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            CreateGrid(gr.RowIndex, false);
        }
    }
    protected void getTotal()
    {
        double total = 0;
        double ScheDisc = 0;
        double discount_percent = 0;
        double discount = 0;
        double beforedecimal = 0;
        double afterdecimal = 0;
        foreach (GridViewRow gr in grdProdSaleReturn.Rows)
        {
            Label lblItemTotal = gr.FindControl("lblItemTotal") as Label;
            Label lblDiscount = gr.FindControl("lblDiscount") as Label;
            total = total + (Convert.ToDouble(lblItemTotal.Text));
            try
            {
                ScheDisc = ScheDisc + (Convert.ToDouble(lblDiscount.Text));
            }
            catch { }
        }
        try
        {
            discount_percent = Convert.ToDouble(txtDiscount.Text);
        }
        catch
        { txtDiscount.Text = "0"; }
        if (discount_percent > 0)
        {
            txtDiscountAmount.Text = ((total * Convert.ToDouble(txtDiscount.Text)) / 100).ToString("#0.00");
            discount = Convert.ToDouble(txtDiscountAmount.Text);
        }
        else
        {
            try
            {
                discount = Convert.ToDouble(txtDiscountAmount.Text);
            }
            catch { txtDiscountAmount.Text = "0"; }
        }
        lblSalesReturnTotal.Text = total.ToString("#0.00");
        lblTotalScheDisc.Text = ScheDisc.ToString("#0.00");
        lblAfterSchDiscount.Text = (total - ScheDisc).ToString("#0.00");
        lblTotalAmount.Text = (total - ScheDisc - discount).ToString("#0.00");
        double vat = Convert.ToDouble(txtVATPercent.Text) / 100;
        lblVAT.Text = ((total - ScheDisc - discount) * vat).ToString("#0.00");
        lblGrandTotal.Text = (Convert.ToDouble(lblVAT.Text) + Convert.ToDouble(lblTotalAmount.Text)).ToString("#0.00");
        if (PGPS.RoundOff() == true)
        {
            beforedecimal = Math.Truncate(Convert.ToDouble(lblGrandTotal.Text));
            afterdecimal = Convert.ToDouble(lblGrandTotal.Text) - beforedecimal;
            if (afterdecimal == 0)
            {
                txtRound.Text = (afterdecimal).ToString("0.00");
                lblInvoiceAmount.Text = beforedecimal.ToString("0.00");
            }
            else if (afterdecimal >= 0.5)
            {
                beforedecimal = beforedecimal + 1;
                txtRound.Text = (1 - afterdecimal).ToString("0.00");
                lblInvoiceAmount.Text = beforedecimal.ToString("0.00");
            }
            else
            {
                txtRound.Text = "-" + (afterdecimal).ToString("0.00");
                lblInvoiceAmount.Text = beforedecimal.ToString("0.00");
            }
        }
        else
        {
            txtRound.Text = "0.00";
            lblInvoiceAmount.Text = (Convert.ToDouble(lblVAT.Text) + Convert.ToDouble(lblTotalAmount.Text)).ToString("#0.00");
        }


    }
    protected void txtRound_TextChanged(object sender, EventArgs e)
    {
        string round = "";
        try
        {
            round = Convert.ToDouble(txtRound.Text).ToString();
        }
        catch
        { txtRound.Text = "0.00"; }
        getTotal();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
      
        //if (!(grdProdSaleReturn.Rows.Count > 0))
        //{
        //    msg = msg + "Atleast 1 product must be added to save the sales.";
        //}
        #region to insert in to sales return master
        SRMEnt = new SALES_RETURN_MASTER();
        SRMEnt.NOTE_FY = txtFiscalYear.Text;
        string engDate = PGD.GetEnglishDateFromNepali(txtReturnDate.Text, "dd/mm/yyyy");
        SRMEnt.NOTE_DATE = engDate;
        string[] nepdate = txtReturnDate.Text.Split('/');
        SRMEnt.NOTE_DAY = nepdate[0];
        SRMEnt.NOTE_MONTH = nepdate[1];
        SRMEnt.NOTE_YEAR = nepdate[2];
        SRMEnt.TAXABLE_AMOUNT = lblSalesReturnTotal.Text;
        SRMEnt.TAX_VAT_AMOUNT = lblVAT.Text;
        SRMEnt.CUSTOMER_NAME = ddlCustomer.SelectedItem.Text;
        SRMEnt.CUSTOMER_ADDRESS = txtCustomerAddress.Text;
        SRMEnt.COSTOMER_PAN_VAT = txtCustomerPANVAT.Text;
        SRMEnt.CUSTOMER_CONTACT_NUMBER = txtCustomerContact.Text;
        SRMEnt.RETURN_REMARKS = txtCreditNoteRemarks.Text;
        SRMEnt.NOTE_TIME = DateTime.Now.ToString("HH:mm:ss");
        SRMEnt.GRAND_TOTAL = lblInvoiceAmount.Text;
        string pk_id = SRMSer.Insert(SRMEnt).ToString();

        #endregion

        #region to insert in to sales detail
        foreach (GridViewRow gr in grdProdSaleReturn.Rows)
        {
            Label lblSno = (Label)gr.FindControl("lblSno");
            Label lblProductPK_ID = (Label)gr.FindControl("lblProductPK_ID");
            Label lblUQty = (Label)gr.FindControl("lblUQty");
            Label lblQty = (Label)gr.FindControl("lblQty");
            Label lblRate = (Label)gr.FindControl("lblRate");
            Label lblItemTotal = (Label)gr.FindControl("lblItemTotal");
            Label lblDiscount = (Label)gr.FindControl("lblDiscount");
            Label lblAfterScheDisc = (Label)gr.FindControl("lblAfterScheDisc");

            SRDEnt = new SALES_RETURN_DETAIL();
            SRDEnt.SALES_RETURN_ID = pk_id;
            SRDEnt.SNO = lblSno.Text;
            SRDEnt.QUANTITY = lblQty.Text;
            SRDEnt.UPPER_QUANTITY = lblUQty.Text;
            SRDEnt.RATE = lblRate.Text;
            SRDEnt.TOTAL = lblItemTotal.Text;
            SRDEnt.PRODUCT_ID = lblProductPK_ID.Text;
            SRDEnt.SCHEME_DISCOUNT = lblDiscount.Text;
            SRDEnt.TAXABLE_TOTAL = (Convert.ToDouble(lblAfterScheDisc.Text)).ToString();
            SRDEnt.TAX_AMOUNT = ((Convert.ToDouble(lblAfterScheDisc.Text)) * Convert.ToDouble(PG.CompanyTAXPercent()) / 100).ToString();
            SRDEnt.AMOUNT_AFTER_TAX = (Convert.ToDouble(lblAfterScheDisc.Text) + (Convert.ToDouble(lblAfterScheDisc.Text)) * Convert.ToDouble(PG.CompanyTAXPercent()) / 100).ToString();
            SRDSer.Insert(SRDEnt);

        }
    }


    #endregion

    protected void SetVisibilty()
    {
        // tdPO.Visible = PGPS.ShowPO();
        //trRoundOff.Visible = PGPS.RoundOff();       
        divBatch.Visible = false;
        trScheDis.Visible = PGPS.ItemWiseDiscount();
        trAftScheDis.Visible = PGPS.ItemWiseDiscount();
        tdDualUnit.Visible = PGPS.DualQuantity();
        tdSchDisc.Visible = PGPS.ItemWiseDiscount();
        grdProdSaleReturn.Columns[3].Visible = PGPS.ProductBatch();
        grdProdSaleReturn.Columns[4].Visible = PGPS.ProductExpDate();
        grdProdSaleReturn.Columns[5].Visible = PGPS.DualQuantity();
        //grdProdSaleReturn.Columns[9].Visible = PGPS.ItemWiseDiscount();
        //grdProdSaleReturn.Columns[10].Visible = PGPS.ItemWiseDiscount();
    }
    protected void txtUQty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            PEnt = new PRODUCT();
            PEnt.PK_ID = ddlProductName.SelectedValue;
            PEnt = (PRODUCT)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {
                txtQty.Text = (Convert.ToDouble(txtUQty.Text) * Convert.ToDouble(PEnt.PACK_QTY)).ToString();
                txtRate.Focus();
                if (Convert.ToDouble(txtAvilableQty.Text) - Convert.ToDouble(txtQty.Text) < 0)
                {
                    HelperFunction.MsgBox(this, this.GetType(), "Available qty is less then sales qty. Are you sure to make invoice?");
                }
            }
        }
        catch
        {
            txtUQty.Text = "0";
            txtUQty.Focus();
            HelperFunction.MsgBox(this, this.GetType(), "Enter Number Only. ");
        }
    }
    protected void ClearProduct()
    {
        txtProductCode.Text = "";
        ddlProductName.SelectedValue = "SELECT";
        txtUQty.Text = "";
        txtQty.Text = "";
        lblUUnit.Text = "";
        lblBUnit.Text = "";
        txtRate.Text = "";
        txtAmount.Text = "";
        txtScheDisc.Text = "";
        txtAvilableQty.Text = "";
        txtCreditNoteRemarks.Text = "";
        txtProductCode.Focus();
    }

    protected void txtProdCode_TextChanged(object sender, EventArgs e)
    {
        txtCustomerCode.Text = txtCustomerCode.Text.ToUpper();
        CEnt = new CUSTOMER();
        CEnt.CUSTOMER_CODE = txtCustomerCode.Text;
        CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
        if (CEnt != null && !string.IsNullOrEmpty(txtCustomerCode.Text))
        {
            ddlCustomer.SelectedValue = CEnt.PK_ID;
            txtCustomerAddress.Text = CEnt.ADDRESS;
            txtCustomerPANVAT.Text = CEnt.VAT_PAN_NUMBER;
            txtCustomerContact.Text = CEnt.MOBILE;

        }
        else
        {
            ddlCustomer.SelectedValue = "SELECT";
            txtCustomerCode.Text = "";
            txtCustomerCode.Focus();
            HelperFunction.MsgBox(this, this.GetType(), "Not a Valid Customer Code.");
        }
    }

    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        CEnt = new CUSTOMER();
        CEnt.PK_ID = ddlCustomer.SelectedValue;
        CEnt = (CUSTOMER)CSer.GetSingle(CEnt);
        if (CEnt != null)
        {
            txtCustomerCode.Text = CEnt.CUSTOMER_CODE;
            txtCustomerAddress.Text = CEnt.ADDRESS;
            txtCustomerPANVAT.Text = CEnt.VAT_PAN_NUMBER;
            txtCustomerContact.Text = CEnt.MOBILE;
        }
    }
}



