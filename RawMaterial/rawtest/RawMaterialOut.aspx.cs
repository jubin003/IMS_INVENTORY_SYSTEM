using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;
using PhyeGanCore;


public partial class Account_Utilities_RawMaterialOut : System.Web.UI.Page
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
    PURCHASE_INVOICE_DETAIL EntPinv = new PURCHASE_INVOICE_DETAIL();
    PURCHASE_INVOICE_DETAILService SerPinv = new PURCHASE_INVOICE_DETAILService();
    PURCHASE_INVOICE_MASTER EntPinM = new PURCHASE_INVOICE_MASTER();
    PURCHASE_INVOICE_MASTERService SerPinM = new PURCHASE_INVOICE_MASTERService();

    PhyeGanDate PGD = new PhyeGanDate();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfileEnt = new UserProfileEntity();
    UserProfileEntity userProfile = new UserProfileEntity();
    PhyeGan PG = new PhyeGan();
    static string path = "";

    // Fiscal year used when calculating available quantity (change this in one place)
    private const string AvailableQtyFY = "2083/84";


    protected void Page_Load(object sender, EventArgs e)
    {
        // Must be set on every request, not just the first load,
        // otherwise btnSave_Click sees an empty profile after postback
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

        if (!IsPostBack)
        {
            //try
            //{
            //  ViewState["postids"] = System.Guid.NewGuid().ToString();
            //  Session["postid"] = ViewState["postids"].ToString();
            //  userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            //   path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
            //   path = path.Replace(PG.Org_Base_URL(), "");
            //   if (hf.checkPageAccess(path, userProfile.UserGroupID.ToString()))
            {
                BindGrid();
                LoadInhouseProdut();
            }
            //   else
            //  {
            //    Response.Redirect("~/forbidden.aspx");
            //  }
            //  }
            // catch (Exception ww)
            // {
            //    Response.Redirect("~/Login.aspx");
            // }
        }
    }

    private DataTable RawMaterialTable
    {
        get
        {
            if (ViewState["RawMaterialTable"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PK_ID");
                dt.Columns.Add("PRODUCT_ID");
                dt.Columns.Add("Available_Qty");
                dt.Columns.Add("QTY");
                dt.Columns.Add("UNIT_ID");
                dt.Columns.Add("RATE");
                dt.Columns.Add("Expiry_Date");
                dt.Columns.Add("Batch_no");
                ViewState["RawMaterialTable"] = dt;
            }
            return (DataTable)ViewState["RawMaterialTable"];
        }
        set { ViewState["RawMaterialTable"] = value; }
    }

    protected void LoadInhouseProdut()
    {
        PEnt = new PRODUCT();
        PEnt.PRODUCT_TYPE_ID = "5";
        ddlProduction.DataSource = PSer.GetAll(PEnt);
        ddlProduction.DataTextField = "PRODUCT_NAME";
        ddlProduction.DataValueField = "PK_ID";
        ddlProduction.DataBind();
        ddlProduction.Items.Insert(0,"Select");
    }

    private void BindGrid()
    {
        grdRawMaterial.DataSource = RawMaterialTable;
        grdRawMaterial.DataBind();
    }


    private string GetAvailableQty(string matid)
    {
        if (string.IsNullOrEmpty(matid))
            return "0";

        // Masters for the fiscal year
        PURCHASE_INVOICE_MASTER masterFilter = new PURCHASE_INVOICE_MASTER();
        EntityList allMasters = (EntityList)SerPinM.GetAll(masterFilter);

        EntityList fyMasters = new EntityList();
        foreach (PURCHASE_INVOICE_MASTER m in allMasters)
        {
            if (m.DAKHILA_FY == AvailableQtyFY)
            {
                fyMasters.Add(m);
            }
        }

        PURCHASE_INVOICE_DETAIL detailFilter = new PURCHASE_INVOICE_DETAIL();
        EntityList allDetails = (EntityList)SerPinv.GetAll(detailFilter);

        double total = 0;
        foreach (PURCHASE_INVOICE_DETAIL d in allDetails)
        {
            if (d.PRODUCT_ID != matid)
                continue;

            foreach (PURCHASE_INVOICE_MASTER m in fyMasters)
            {
                if (m.PK_ID == d.PURCHASE_INVOICE_ID)
                {
                    double q;
                    double.TryParse(d.QUANTITY, out q);
                    total += q;
                    break;   
                }
            }
        }

        return total.ToString("0.##");
    }

    private void CaptureGridState()
    {
        DataTable dt = RawMaterialTable;
        if (dt.Rows.Count != grdRawMaterial.Rows.Count)
            return;

        for (int i = 0; i < grdRawMaterial.Rows.Count; i++)
        {
            GridViewRow row = grdRawMaterial.Rows[i];

            DropDownList ddlMaterial = (DropDownList)row.FindControl("ddlMaterial");
            TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
            DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnit");
            TextBox txtRate = (TextBox)row.FindControl("txtRate");
            TextBox txtAvQty = (TextBox)row.FindControl("txtAvQty");
            TextBox txtExpDate = (TextBox)row.FindControl("txtExpDate");
            TextBox txtBatchno = (TextBox)row.FindControl("txtBatchno");
            

            dt.Rows[i]["PRODUCT_ID"] = ddlMaterial.SelectedValue;
            dt.Rows[i]["Available_Qty"] = txtAvQty.Text;
            dt.Rows[i]["QTY"] = txtQuantity.Text;
            dt.Rows[i]["UNIT_ID"] = ddlUnit.SelectedValue;
            dt.Rows[i]["RATE"] = txtRate.Text;
            dt.Rows[i]["Expiry_Date"] = txtExpDate.Text;
            dt.Rows[i]["Batch_no"] = txtBatchno;
        }
    }

    protected void grdRawMaterial_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        CaptureGridState();
        RawMaterialTable.Rows.RemoveAt(e.RowIndex);
        BindGrid();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        CaptureGridState();
        RawMaterialTable.Rows.Add(RawMaterialTable.NewRow());
        BindGrid();
    }

    protected void grdRawMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = e.Row.DataItem as DataRowView;
            string matid = drv != null ? drv["PRODUCT_ID"] as string : string.Empty;

            // Material dropdown
            DropDownList ddlMaterial = (DropDownList)e.Row.FindControl("ddlMaterial");
            if (ddlMaterial != null)
            {
                PRODUCT rawMaterialFilter = new PRODUCT();
                rawMaterialFilter.PRODUCT_TYPE_ID = "3";
                ddlMaterial.DataSource = PSer.GetAll(rawMaterialFilter);
                ddlMaterial.DataTextField = "PRODUCT_NAME";
                ddlMaterial.DataValueField = "PK_ID";
                ddlMaterial.DataBind();
                ddlMaterial.Items.Insert(0, new ListItem("Select", ""));

                if (!string.IsNullOrEmpty(matid) && ddlMaterial.Items.FindByValue(matid) != null)
                    ddlMaterial.SelectedValue = matid;
            }

            // Available quantity
            TextBox txtAvQty = (TextBox)e.Row.FindControl("txtAvQty");
            if (txtAvQty != null)
            {
                txtAvQty.Text = GetAvailableQty(matid);
            }

            // Quantity
            TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");
            txtQuantity.Text = drv["QTY"] as string;

            // Rate
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

            // Unit
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
                    PRODUCT_UNIT unitResult = (PRODUCT_UNIT)SUnit.GetSingle(unitFilter);

                    if (unitResult != null)
                        ddlUnit.Items.Add(new ListItem(unitResult.UNIT_NAME, unitResult.PK_ID));
                }
            }
            //exp date
            TextBox txtExpDate = (TextBox)e.Row.FindControl("txtExpDate");
            txtExpDate.Text = drv["Expiry_Date"] as string; ;

            TextBox txtBatchno = (TextBox)e.Row.FindControl("txtBatchno");
            txtBatchno.Text = drv["Batch_no"] as string; ;

            totalCalculate(e.Row);
        }
    }

    protected void totalCalculate(GridViewRow row)
    {
        double matqty = 0;
        double matarte = 0;

        TextBox txtRate = (TextBox)row.FindControl("txtRate");
        double.TryParse(txtRate.Text, out matarte);

        TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
        double.TryParse(txtQuantity.Text, out matqty);

        Label lblTotal = (Label)row.FindControl("lblTotal");
        double total = matarte * matqty;
        lblTotal.Text = Convert.ToString(total);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string prodid = ddlProduction.SelectedValue;
        double total = 0;
        double grandtotal = 0;

        string issue = (txtIssue.Text).ToString();

        string engDat;
        engDat = PGD.GetEnglishDateFromNepali(txtDate.Text, "dd/mm/yyyy");


        string[] nepDate = txtDate.Text.Split('/');
        string day = nepDate[0];
        string month = nepDate[1];
        string year = nepDate[2];

        string FY = PGD.checkFiscalYear(month, year);
        foreach (GridViewRow row in grdRawMaterial.Rows)
        {
            Label lblTotal = (Label)row.FindControl("lblTotal");
            double.TryParse(lblTotal.Text, out total);
            grandtotal += total;
        }
        string userid = (string)userProfileEnt.ID;

        EStoreOut = new STORE_OUT();
        EStoreOut.PRODUCT_ID = prodid;
        EStoreOut.TOTAL_AMOUNT = grandtotal.ToString("0.##");
        EStoreOut.ISSUE_TO = issue;
        EStoreOut.STORE_OUT_DAY = day;
        EStoreOut.STORE_OUT_MONTH = month;
        EStoreOut.STORE_OUT_YEAR = year;
        EStoreOut.STORE_OUT_FY = FY;
        EStoreOut.STATUS = "1";
        EStoreOut.STORE_OUT_DATE = engDat;
        EStoreOut.USER_ID = userProfileEnt.EmployeeID;

        string prevstore = null;

        STORE_OUT alstoreout = new STORE_OUT();
        EntityList storeLis = (EntityList)SStoreOut.GetAll(alstoreout);

        foreach (STORE_OUT row in storeLis)
        {
            prevstore = row.PK_ID;
        }
        int res = 0;

        int.TryParse(prevstore, out res);
        res = res + 1;

        EStoreOut.STORE_OUT_NUMBER = "STR" + res;

        SStoreOut.Insert(EStoreOut);

        string laststore = null;

        STORE_OUT allstoreout = new STORE_OUT();
        EntityList storeList = (EntityList)SStoreOut.GetAll(allstoreout);

        foreach (STORE_OUT row in storeList)
        {
            laststore = row.PK_ID;
        }

        foreach (GridViewRow row in grdRawMaterial.Rows)
        {
            DropDownList ddlMaterial = (DropDownList)row.FindControl("ddlMaterial");
            TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
            DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnit");
            TextBox txtRate = (TextBox)row.FindControl("txtRate");
            TextBox txtExpDate = (TextBox)row.FindControl("txtExpDate");
            TextBox txtBatchno = (TextBox)row.FindControl("txtBatchno");

            string matid = ddlMaterial.SelectedValue;
            string qty = txtQuantity.Text;
            string unit = ddlUnit.SelectedValue;
            string rate = txtRate.Text;
            string expDate = PGD.GetEnglishDateFromNepali(txtExpDate.Text, "dd/mm/yyyy");

            STORE_OUT_DETAIL detailRow = new STORE_OUT_DETAIL();
            detailRow.STORE_OUT_ID = laststore;
            detailRow.PRODUCT_ID = matid;
            detailRow.QTY = qty;
            detailRow.RATE = rate;
            detailRow.UNIT_ID = unit;
            detailRow.SNO = matid;
            detailRow.BATCH_NUMBER = txtBatchno.Text;
            detailRow.EXPIRE_DATE = expDate;            

            SStoreDetail.Insert(detailRow);
        }

        ddlProduction.SelectedIndex = 0;
        txtIssue.Text = "";
        txtDate.Text = "";
        RawMaterialTable.Rows.Clear();
        BindGrid();
    }

    protected void ddlMaterial_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlMaterial = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlMaterial.NamingContainer;
        string matid = ddlMaterial.SelectedValue;

        TextBox txtRate = (TextBox)row.FindControl("txtRate");
        DropDownList ddlUnit = (DropDownList)row.FindControl("ddlUnit");
        TextBox txtAvQty = (TextBox)row.FindControl("txtAvQty");

        ddlUnit.Items.Clear();
        txtAvQty.Text = GetAvailableQty(matid);  

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
        }
        else
        {
            txtRate.Text = "";
        }

        totalCalculate(row);
        CaptureGridState();
    }

    protected void txtQuantity_TextChanged(object sender, EventArgs e)
    {
        TextBox txtQuantity = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtQuantity.NamingContainer;
        totalCalculate(row);
        CaptureGridState();
    }
}