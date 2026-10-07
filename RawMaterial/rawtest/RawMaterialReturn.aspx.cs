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

public partial class RawMaterial_RawMaterialReturn : System.Web.UI.Page
{
    PRODUCT PEnt = new PRODUCT();
    PRODUCTService PSer = new PRODUCTService();
    PRODUCT_TYPE EPrdt = new PRODUCT_TYPE();
    PRODUCT_TYPEService SPrdt = new PRODUCT_TYPEService();
    PRODUCT_UNIT Eunit = new PRODUCT_UNIT();
    PRODUCT_UNITService SUnit = new PRODUCT_UNITService();
    PRODUCT_RATES ERates = new PRODUCT_RATES();
    PRODUCT_RATEService SRates = new PRODUCT_RATEService();
    STORE_OUT EStoreOut = new STORE_OUT();
    STORE_OUTService SStoreOut = new STORE_OUTService();
    STORE_OUT_DETAIL EStoreDetail = new STORE_OUT_DETAIL();
    STORE_OUT_DETAILService SStoreDetail = new STORE_OUT_DETAILService();
    STORE_RETURN EstoreRet = new STORE_RETURN();
    STORE_RETURNService SStoreRet = new STORE_RETURNService();
    STORE_RETURN_DETAIL EStoreRetDet = new STORE_RETURN_DETAIL();
    STORE_RETURN_DETAILService SStoreRetDet = new STORE_RETURN_DETAILService();
    PhyeGanDate PGD = new PhyeGanDate();
    UserProfileEntity userProfileEnt = new UserProfileEntity();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

            BindGrid();
            LoadInHouseProduct();
        }
    }
    private DataTable RawMaterialReturnTable
    {
        get
        {
            if (ViewState["RawMaterialReturnTable"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PK_ID");
                dt.Columns.Add("PRODUCT_ID");
                dt.Columns.Add("QTY");
                dt.Columns.Add("UNIT_ID");
                dt.Columns.Add("RATE");
                dt.Columns.Add("Expiry_Date");
                dt.Columns.Add("Batch_no");
                ViewState["RawMaterialReturnTable"] = dt;
            }
            return (DataTable)ViewState["RawMaterialReturnTable"];
        }
        set { ViewState["RawMaterialReturnTable"] = value; }
    }

    private void BindGrid()
    {
        grdMaterial.DataSource = RawMaterialReturnTable;
        grdMaterial.DataBind();
    }

    protected void ddlProduction_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    private void CaptureGridState()
    {
        DataTable dt = RawMaterialReturnTable;
        if (dt.Rows.Count != grdMaterial.Rows.Count)
            return;

        for (int i = 0; i < grdMaterial.Rows.Count; i++)
        {
            GridViewRow row = grdMaterial.Rows[i];

            DropDownList ddlMaterial = (DropDownList)row.FindControl("ddlMaterial");
            TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
            DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnit");
            TextBox txtRate = (TextBox)row.FindControl("txtRate");
            TextBox txtExpDate = (TextBox)row.FindControl("txtExpDate");
            TextBox txtBatchno = (TextBox)row.FindControl("txtBatchno");

            dt.Rows[i]["PRODUCT_ID"] = ddlMaterial.SelectedValue;
            dt.Rows[i]["QTY"] = txtQuantity.Text;
            dt.Rows[i]["UNIT_ID"] = ddlUnit.SelectedValue;
            dt.Rows[i]["RATE"] = txtRate.Text;
            dt.Rows[i]["Batch_no"] = txtBatchno.Text;
            dt.Rows[i]["Expiry_Date"] = txtExpDate.Text;
            
        }
    }

    //load products
    protected void LoadInHouseProduct()
    {
        EStoreOut = new STORE_OUT();
        EntityList storeOutList = (EntityList)SStoreOut.GetAll(EStoreOut);

        ddlProduction.Items.Clear();

        foreach (STORE_OUT row in storeOutList)
        {
            PRODUCT prod = new PRODUCT();
            prod.PK_ID = row.PRODUCT_ID;

            PRODUCT product = (PRODUCT)PSer.GetSingle(prod);
            if (product != null)
            {
                ddlProduction.Items.Add(new ListItem(product.PRODUCT_NAME, product.PK_ID));
            }
        }

        ddlProduction.Items.Insert(0, new ListItem("Select", ""));
    }

    //grid materials loading
    private void MatProd(DropDownList ddl, string prodid)
    {
        ddl.Items.Clear();

        if (string.IsNullOrEmpty(prodid))
        {
            ddl.Items.Insert(0, new ListItem("Select", ""));
            return;
        }

        STORE_OUT storeOutFilter = new STORE_OUT();
        EntityList storeOutList = (EntityList)SStoreOut.GetAll(storeOutFilter);

        STORE_OUT_DETAIL detailFilter = new STORE_OUT_DETAIL();
        EntityList detailList = (EntityList)SStoreDetail.GetAll(detailFilter);

        EntityList addedMaterials = new EntityList();

        foreach (STORE_OUT storeOutRow in storeOutList)
        {
            if (storeOutRow.PRODUCT_ID != prodid)
                continue;

            foreach (STORE_OUT_DETAIL detailRow in detailList)
            {
                if (detailRow.STORE_OUT_ID != storeOutRow.PK_ID)
                    continue;

                bool alreadyAdded = false;
                foreach (PRODUCT added in addedMaterials)
                {
                    if (added.PK_ID == detailRow.PRODUCT_ID)
                    {
                        alreadyAdded = true;
                        break;
                    }
                }

                if (!alreadyAdded)
                {
                    PRODUCT prod = new PRODUCT();
                    prod.PK_ID = detailRow.PRODUCT_ID;

                    PRODUCT product = (PRODUCT)PSer.GetSingle(prod);
                    if (product != null)
                    {
                        ddl.Items.Add(new ListItem(product.PRODUCT_NAME, product.PK_ID));
                        addedMaterials.Add(product);
                    }
                }
            }
        }

        ddl.Items.Insert(0, new ListItem("Select", ""));
    }

    //total qty materials
    private double GetTotalQtyForMaterial(string prodid, string matid)
    {
        double totalQty = 0;

        if (string.IsNullOrEmpty(prodid) || string.IsNullOrEmpty(matid))
            return totalQty;

        STORE_OUT storeOutFilter = new STORE_OUT();
        EntityList storeOutList = (EntityList)SStoreOut.GetAll(storeOutFilter);

        STORE_OUT_DETAIL detailFilter = new STORE_OUT_DETAIL();
        EntityList detailList = (EntityList)SStoreDetail.GetAll(detailFilter);

        foreach (STORE_OUT storeOutRow in storeOutList)
        {
            if (storeOutRow.PRODUCT_ID != prodid)
                continue;

            foreach (STORE_OUT_DETAIL detailRow in detailList)
            {
                if (detailRow.STORE_OUT_ID == storeOutRow.PK_ID && detailRow.PRODUCT_ID == matid)
                {
                    double qty;
                    double.TryParse(detailRow.QTY, out qty);
                    totalQty += qty;
                }
            }
        }

        return totalQty;
    }

    //calulate total
    protected void totalCalculate(GridViewRow row)
    {
        double matqty = 0;
        double matrate = 0;

        TextBox txtRate = (TextBox)row.FindControl("txtRate");
        double.TryParse(txtRate.Text, out matrate);

        TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
        double.TryParse(txtQuantity.Text, out matqty);

        Label lblTotal = (Label)row.FindControl("lblTotal");
        double total = matrate * matqty;
        lblTotal.Text = Convert.ToString(total);
    }

    //deleting
    protected void grdMaterial_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        CaptureGridState();
        RawMaterialReturnTable.Rows.RemoveAt(e.RowIndex);
        BindGrid();
    }
    protected void grdMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            string matid = drv != null ? drv["PRODUCT_ID"] as string : string.Empty;

            DropDownList ddlMaterial = (DropDownList)e.Row.FindControl("ddlMaterial");
            if (ddlMaterial != null)
            {
                string selectedProdId = ddlProduction.SelectedValue;
                MatProd(ddlMaterial, selectedProdId);

                if (!string.IsNullOrEmpty(matid) && ddlMaterial.Items.FindByValue(matid) != null)
                    ddlMaterial.SelectedValue = matid;
            }

            TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");
            if (txtQuantity != null && drv != null)
                txtQuantity.Text = drv["QTY"] as string;

            TextBox txtRate = (TextBox)e.Row.FindControl("txtRate");
            if (!string.IsNullOrEmpty(matid))
            {
                PRODUCT_RATES pid = new PRODUCT_RATES();
                pid.PRODUCT_ID = matid;

                PRODUCT_RATES res = (PRODUCT_RATES)SRates.GetSingle(pid);
                txtRate.Text = res != null ? res.RATE.ToString() : "";
            }
            else
            {
                txtRate.Text = "";
            }

            DropDownList ddlUnit = (DropDownList)e.Row.FindControl("ddlUnit");
            ddlUnit.Items.Clear();
            if (!string.IsNullOrEmpty(matid))
            {
                PRODUCT productFilter = new PRODUCT();
                productFilter.PK_ID = matid;
                PRODUCT selectedProduct = (PRODUCT)PSer.GetSingle(productFilter);

                if (selectedProduct != null && !string.IsNullOrEmpty(selectedProduct.UNIT_ID))
                {
                    PRODUCT_UNIT unitFilter = new PRODUCT_UNIT();
                    unitFilter.PK_ID = selectedProduct.UNIT_ID;
                    PRODUCT_UNIT uniResult = (PRODUCT_UNIT)SUnit.GetSingle(unitFilter);

                    if (uniResult != null)
                    {
                        ddlUnit.Items.Add(new ListItem(uniResult.UNIT_NAME, uniResult.PK_ID));
                    }
                }
            }

            Label lblTotalQty = (Label)e.Row.FindControl("lblTotalQty");
            if (lblTotalQty != null)
            {
                string selectedProdIdForTotal = ddlProduction.SelectedValue;
                double totalQty = GetTotalQtyForMaterial(selectedProdIdForTotal, matid);
                lblTotalQty.Text = totalQty.ToString();
            }

            TextBox txtExpDate = (TextBox)e.Row.FindControl("txtExpDate");
            txtExpDate.Text = drv["Expiry_Date"] as string;

            TextBox txtBatchno = (TextBox)e.Row.FindControl("txtBatchno");
            txtBatchno.Text = drv["Batch_no"] as string;

            totalCalculate(e.Row);
        }
    }

    protected void ddlMaterial_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlMaterial = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlMaterial.NamingContainer;
        string matid = ddlMaterial.SelectedValue;

        TextBox txtRate = (TextBox)row.FindControl("txtRate");
        DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnit");
        Label lblTotalQty = (Label)row.FindControl("lblTotalQty");

        ddlUnit.Items.Clear();

        if (!string.IsNullOrEmpty(matid))
        {
            PRODUCT_RATES pid = new PRODUCT_RATES();
            pid.PRODUCT_ID = matid;
            PRODUCT_RATES res = (PRODUCT_RATES)SRates.GetSingle(pid);
            txtRate.Text = res != null ? res.RATE.ToString() : "";

            PRODUCT productFilter = new PRODUCT();
            productFilter.PK_ID = matid;
            PRODUCT selectedProduct = (PRODUCT)PSer.GetSingle(productFilter);

            if (selectedProduct != null && !string.IsNullOrEmpty(selectedProduct.UNIT_ID))
            {
                PRODUCT_UNIT unitFilter = new PRODUCT_UNIT();
                unitFilter.PK_ID = selectedProduct.UNIT_ID;
                PRODUCT_UNIT unitResult = (PRODUCT_UNIT)SUnit.GetSingle(unitFilter);

                if (unitResult != null)
                    ddlUnit.Items.Add(new ListItem(unitResult.UNIT_NAME, unitResult.PK_ID));
            }

            if (lblTotalQty != null)
            {
                double totalQty = GetTotalQtyForMaterial(ddlProduction.SelectedValue, matid);
                lblTotalQty.Text = totalQty.ToString();
            }
        }
        else
        {
            txtRate.Text = "";
            if (lblTotalQty != null)
                lblTotalQty.Text = "";
        }

        totalCalculate(row);

        CaptureGridState();
    }

    //
    protected void txtQuantity_TextChanged(object sender, EventArgs e)
    {
        TextBox txtQuantity = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtQuantity.NamingContainer;
        totalCalculate(row);

        CaptureGridState();
    }

    //add button
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        CaptureGridState();
        RawMaterialReturnTable.Rows.Add(RawMaterialReturnTable.NewRow());
        BindGrid();
    }

    //return button
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        string prodid = ddlProduction.SelectedValue;
        double total = 0;
        double grandtotal = 0;
        string[] nepDate = txtDate.Text.Split('/');
        string day = nepDate[0];
        string month = nepDate[1];
        string year = nepDate[2];

        string FY = PGD.checkFiscalYear(month, year);

        string engDat = PGD.GetEnglishDateFromNepali(txtDate.Text, "dd/mm/yyyy");
        foreach (GridViewRow row in grdMaterial.Rows)
        {
            Label lblTotal = (Label)row.FindControl("lblTotal");
            double.TryParse(lblTotal.Text, out total);
            grandtotal = grandtotal + total;
        }
        EstoreRet = new STORE_RETURN();
        EstoreRet.PRODUCT_ID = prodid;
        EstoreRet.TOTAL_AMOUNT = grandtotal.ToString();
        EstoreRet.STORE_RETURN_DAY = day;
        EstoreRet.STORE_RETURN_MONTH = month;
        EstoreRet.STORE_RETURN_YEAR = year;
        EstoreRet.STORE_RETURN_FY = FY;
        EstoreRet.STATUS = "0";
        EstoreRet.ISSUE_TO = txtIssue.Text;
        EstoreRet.STORE_RETURN_DATE = engDat;
        EstoreRet.USER_ID = userProfileEnt.EmployeeID;
        STORE_RETURN alstoreout = new STORE_RETURN();
        EntityList storeLis = (EntityList)SStoreRet.GetAll(alstoreout);
        string prevstore = null;

        foreach (STORE_RETURN row in storeLis)
        {
            prevstore = row.PK_ID;
        }
        int res = 0;

        int.TryParse(prevstore, out res);
        res = res + 1;
        EstoreRet.STORE_RETURN_NUMBER = "SRN" + res;
        SStoreRet.Insert(EstoreRet);
        string laststoreret = null;
        STORE_RETURN allstoreret = new STORE_RETURN();
        EntityList storeRetList = (EntityList)SStoreRet.GetAll(allstoreret);
   
        foreach (STORE_RETURN row in storeRetList)
        {
            laststoreret = row.PK_ID;
        }

        foreach (GridViewRow row in grdMaterial.Rows)
        {
            userProfileEnt = new UserProfileEntity();
            DropDownList ddlMaterial = (DropDownList)row.FindControl("ddlMaterial");
            TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
            DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnit");
            TextBox txtRate = (TextBox)row.FindControl("txtRate");
            TextBox txtExpDate = (TextBox)row.FindControl("txtExpDate");
            TextBox txtBatchno = (TextBox)row.FindControl("txtBatchno");

            string matid = ddlMaterial.SelectedValue;
            string expDate = PGD.GetEnglishDateFromNepali(txtExpDate.Text, "dd/mm/yyyy");


            EStoreRetDet = new STORE_RETURN_DETAIL();
            EStoreRetDet.STORE_RETURN_ID = laststoreret;
            EStoreRetDet.PRODUCT_ID = matid;
            EStoreRetDet.QTY = txtQuantity.Text;
            EStoreRetDet.RATE = txtRate.Text;
            EStoreRetDet.UNIT_ID = ddlUnit.SelectedValue;
            EStoreRetDet.EXPIRY_DATE = expDate;
            EStoreRetDet.BATCH_NUMBER = txtBatchno.Text;
            EStoreRetDet.OFFICE_CODE = userProfileEnt.LocationID;
            SStoreRetDet.Insert(EStoreRetDet);
        }

        ddlProduction.SelectedIndex = 0;
        txtIssue.Text = "";
        txtDate.Text = "";
        RawMaterialReturnTable.Rows.Clear();
        BindGrid();
    }
}