using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Framework;
using Entity.Components;
using Service.Components;

public partial class RawMaterial_FinishGoods : System.Web.UI.Page
{
    STORE_OUT EStoreOut = new STORE_OUT();
    STORE_OUTService SStoreout = new STORE_OUTService();
    STORE_OUT_DETAIL EStore_OutDetial = new STORE_OUT_DETAIL();
    STORE_OUT_DETAILService SStore_OutDetial = new STORE_OUT_DETAILService();
    STORE_IN EStoreIn = new STORE_IN();
    STORE_INService SStoreIn = new STORE_INService();
    STORE_IN_DETAIL EStore_InDetail = new STORE_IN_DETAIL();
    STORE_IN_DETAILService SStore_InDetail = new STORE_IN_DETAILService();
    PRODUCT EProd = new PRODUCT();
    PRODUCTService SProd = new PRODUCTService();
    PRODUCT_UNIT EUnit = new PRODUCT_UNIT();
    PRODUCT_UNITService SUnit = new PRODUCT_UNITService();
    UserProfileEntity userProfileEnt = new UserProfileEntity();
    PhyeGanDate PGD = new PhyeGanDate();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

            loadMaterial();
            BindGrid();
        }
    }


    private DataTable FinishGoodsTable
    {
        get
        {
            if (ViewState["FinishGoodsTable"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PRODUCT_ID");
                dt.Columns.Add("PRODUCT_NAME");
                dt.Columns.Add("QTY");
                dt.Columns.Add("RATE");
                dt.Columns.Add("UNIT_ID");
                dt.Columns.Add("UNIT_NAME");
                dt.Columns.Add("Expiry_Date");
                dt.Columns.Add("Batch_Number");
                ViewState["FinishGoodsTable"] = dt;
            }
            return (DataTable)ViewState["FinishGoodsTable"];
        }
        set { ViewState["FinishGoodsTable"] = value; }
    }


    private void BindGrid()
    {
        grdStoreIN.DataSource = FinishGoodsTable;
        grdStoreIN.DataBind();
    }

    private void UpdateCostPrice()
    {
        double grandTotal = 0;
        foreach (DataRow r in FinishGoodsTable.Rows)
        {
            double rowQty = 0;
            double rowRate = 0;
            double.TryParse(r["QTY"].ToString(), out rowQty);
            double.TryParse(r["RATE"].ToString(), out rowRate);
            grandTotal += rowQty * rowRate;
        }
        lblCostPrice.Text = grandTotal.ToString();
    }


    private void CaptureGridState()
    {
        DataTable dt = FinishGoodsTable;
        if (dt.Rows.Count != grdStoreIN.Rows.Count)
            return;

        for (int i = 0; i < grdStoreIN.Rows.Count; i++)
        {
            GridViewRow row = grdStoreIN.Rows[i];

            TextBox txtExpDate = (TextBox)row.FindControl("txtExpDate");
            TextBox txtbatchno = (TextBox)row.FindControl("txtbatchno");

            if (txtExpDate != null)
                dt.Rows[i]["Expiry_Date"] = txtExpDate.Text;

            if (txtbatchno != null)
                dt.Rows[i]["Batch_Number"] = txtbatchno.Text;
        }
    }

    protected void loadMaterial()
    {
        EStoreOut = new STORE_OUT();
        EntityList storeoutlist = (EntityList)SStoreout.GetAll(EStoreOut);

        EntityList addedProducts = new EntityList();

        foreach (STORE_OUT row in storeoutlist)
        {
            bool alreadyAdded = false;
            foreach (PRODUCT added in addedProducts)
            {
                if (added.PK_ID == row.PRODUCT_ID)
                {
                    alreadyAdded = true;
                    break;
                }
            }

            if (!alreadyAdded)
            {
                PRODUCT prod = new PRODUCT();
                prod.PK_ID = row.PRODUCT_ID;

                PRODUCT product = (PRODUCT)SProd.GetSingle(prod);
                if (product != null)
                {
                    ddlMaterialName.Items.Add(new ListItem(product.PRODUCT_NAME, product.PK_ID));
                    addedProducts.Add(product);
                }
            }
        }
    }


    protected void loadUnit(string prodid)
    {
        ddlUnit.Items.Clear();

        if (string.IsNullOrEmpty(prodid))
            return;

        PRODUCT prodFilter = new PRODUCT();
        prodFilter.PK_ID = prodid;
        PRODUCT prod = (PRODUCT)SProd.GetSingle(prodFilter);

        if (prod == null || string.IsNullOrEmpty(prod.UNIT_ID))
            return;

        PRODUCT_UNIT unitFilter = new PRODUCT_UNIT();
        unitFilter.PK_ID = prod.UNIT_ID;
        PRODUCT_UNIT unit = (PRODUCT_UNIT)SUnit.GetSingle(unitFilter);

        if (unit != null)
        {
            ddlUnit.Items.Add(new ListItem(unit.UNIT_NAME, unit.PK_ID));
        }
    }

    protected void ddlMaterialName_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadUnit(ddlMaterialName.SelectedValue);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlMaterialName.SelectedValue) || string.IsNullOrEmpty(txtQuantity.Text) || string.IsNullOrEmpty(ddlUnit.SelectedValue))
        {
            return;
        }

        CaptureGridState();

        PRODUCT prodFilter = new PRODUCT();
        prodFilter.PK_ID = ddlMaterialName.SelectedValue;
        PRODUCT product = (PRODUCT)SProd.GetSingle(prodFilter);

        EStoreOut = new STORE_OUT();
        EntityList storeoutlist = (EntityList)SStoreout.GetAll(EStoreOut);

        STORE_OUT matchedStoreOut = null;
        foreach (STORE_OUT row in storeoutlist)
        {
            if (row.PRODUCT_ID == ddlMaterialName.SelectedValue)
            {
                matchedStoreOut = row;
                break;
            }
        }

        string totalamt = matchedStoreOut != null ? matchedStoreOut.TOTAL_AMOUNT : "0";
        string qty = txtQuantity.Text;

        double amt = 0;
        double qt = 0;
        double.TryParse(totalamt, out amt);
        double.TryParse(qty, out qt);

        double lineTotal = amt * qt;

        string productName = product != null ? product.PRODUCT_NAME : ddlMaterialName.SelectedItem.Text;
        string unitName = ddlUnit.SelectedItem.Text;

        DataRow newRow = FinishGoodsTable.NewRow();
        newRow["PRODUCT_ID"] = ddlMaterialName.SelectedValue;
        newRow["PRODUCT_NAME"] = productName;
        newRow["QTY"] = qty;
        newRow["RATE"] = totalamt;
        newRow["UNIT_ID"] = ddlUnit.SelectedValue;
        newRow["UNIT_NAME"] = unitName;
        newRow["Expiry_Date"] = "";
        newRow["Batch_Number"] = "";
        FinishGoodsTable.Rows.Add(newRow);

        BindGrid();
        UpdateCostPrice();

        ddlMaterialName.SelectedIndex = 0;
        txtQuantity.Text = "";
        ddlUnit.Items.Clear();
    }

    protected void grdStoreIN_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView rowData = (DataRowView)e.Row.DataItem;

            Label lblProdname = (Label)e.Row.FindControl("lblProdname");
            Label lblQty = (Label)e.Row.FindControl("lblQty");
            Label lblRate = (Label)e.Row.FindControl("lblRate");
            Label lblUnitName = (Label)e.Row.FindControl("lblUnit");

            lblProdname.Text = rowData["PRODUCT_NAME"].ToString();
            lblQty.Text = rowData["QTY"].ToString();
            lblRate.Text = rowData["RATE"].ToString();
            lblUnitName.Text = rowData["UNIT_NAME"].ToString();

            TextBox txtExpDate = (TextBox)e.Row.FindControl("txtExpDate");
            if (txtExpDate != null)
                txtExpDate.Text = rowData["Expiry_Date"] as string;

            TextBox txtbatchno = (TextBox)e.Row.FindControl("txtbatchno");
            if (txtbatchno != null)
                txtbatchno.Text = rowData["Batch_Number"] as string;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        CaptureGridState();

        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int rowIndex = row.RowIndex;

        FinishGoodsTable.Rows.RemoveAt(rowIndex);
        BindGrid();
        UpdateCostPrice();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        CaptureGridState();

        if (FinishGoodsTable.Rows.Count == 0)
        {
            return;
        }

        double grandtotal = 0;
        foreach (DataRow row in FinishGoodsTable.Rows)
        {
            double rowQty = 0;
            double rowRate = 0;
            double.TryParse(row["QTY"].ToString(), out rowQty);
            double.TryParse(row["RATE"].ToString(), out rowRate);
            grandtotal += rowQty * rowRate;
        }

        string prevstorein = null;

        STORE_IN allstorein_before = new STORE_IN();
        EntityList storeInList_before = (EntityList)SStoreIn.GetAll(allstorein_before);

        foreach (STORE_IN row in storeInList_before)
        {
            prevstorein = row.PK_ID;
        }

        string[] nepDate = txtDate.Text.Split('/');
        string day = nepDate[0];
        string month = nepDate[1];
        string year = nepDate[2];
        string engDat = PGD.GetEnglishDateFromNepali(txtDate.Text, "dd/mm/yyyy");

        string FY = PGD.checkFiscalYear(month, year);

        int res = 0;
        int.TryParse(prevstorein, out res);
        res = res + 1;

        EStoreIn = new STORE_IN();
        EStoreIn.TOTAL_AMOUNT = grandtotal.ToString();
        EStoreIn.STATUS = "1";
        EStoreIn.STORE_IN_NUMBER = "SIN-" + res;
        EStoreIn.USER_ID = userProfileEnt.EmployeeID;

        EStoreIn.STORE_IN_DATE = engDat;
        EStoreIn.STORE_IN_DAY = day;
        EStoreIn.STORE_IN_MONTH = month;
        EStoreIn.STORE_IN_YEAR = year;
        EStoreIn.STORE_IN_FY = FY;

        SStoreIn.Insert(EStoreIn);

        string laststorein = null;

        STORE_IN allstorein_after = new STORE_IN();
        EntityList storeInList_after = (EntityList)SStoreIn.GetAll(allstorein_after);

        foreach (STORE_IN row in storeInList_after)
        {
            laststorein = row.PK_ID;
        }

        foreach (DataRow row in FinishGoodsTable.Rows)
        {
            string expDateNep = row["Expiry_Date"].ToString();
            string expDateEng = !string.IsNullOrEmpty(expDateNep)
                ? PGD.GetEnglishDateFromNepali(expDateNep, "dd/mm/yyyy")
                : "";

            STORE_IN_DETAIL detailRow = new STORE_IN_DETAIL();
            detailRow.STORE_IN_ID = laststorein;
            detailRow.PRODUCT_ID = row["PRODUCT_ID"].ToString();
            detailRow.QTY = row["QTY"].ToString();
            detailRow.RATE = row["RATE"].ToString();
            detailRow.UNIT_ID = row["UNIT_ID"].ToString();
            detailRow.EXPIRY_DATE = expDateEng;
            detailRow.BATCH_NUMBER = row["Batch_Number"].ToString();

            SStore_InDetail.Insert(detailRow);
        }
        FinishGoodsTable.Rows.Clear();
        BindGrid();
        lblCostPrice.Text = "";
        HelperFunction.MsgBox(this, this.GetType(), "Finish goods saved successfully");
    }
}