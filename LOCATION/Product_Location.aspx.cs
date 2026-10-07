using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;

public partial class LOCATION_PRODUCT_Location : System.Web.UI.Page
{
    PRODUCT MDEnt = new PRODUCT();
    PRODUCTService MDSer = new PRODUCTService();

    STORAGE_SHELF SSEnt = new STORAGE_SHELF();
    STORAGE_SHELFService SSSer = new STORAGE_SHELFService();

    STORAGE_SHELFCOMPART SSCEnt = new STORAGE_SHELFCOMPART();
    STORAGE_SHELFCOMPARTService SSCSer = new STORAGE_SHELFCOMPARTService();

    PRODUCT_STORE_AT MSAEnt = new PRODUCT_STORE_AT();
    PRODUCT_STORE_ATService MSASer = new PRODUCT_STORE_ATService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadShelf();
            LoadPRODUCT();
        }
    }

    protected void LoadPRODUCT()
    {
        MDEnt = new PRODUCT();
        ddlPRODUCT.DataSource = MDSer.GetAll(MDEnt);
        ddlPRODUCT.DataTextField = "PRODUCT_NAME";
        ddlPRODUCT.DataValueField = "pk_id";
        ddlPRODUCT.DataBind();
        ddlPRODUCT.Items.Insert(0, "Select");
    }

    protected void LoadShelf()
    {
        SSEnt = new STORAGE_SHELF();
        ddlShelf.DataSource = SSSer.GetAll(SSEnt);

        ddlShelf.DataTextField = "SHELFNO";
        ddlShelf.DataValueField = "PK_ID";
        ddlShelf.DataBind();
        ddlShelf.Items.Insert(0, "Select");
    }

    protected void LoadShelfCompart()
    {
        SSCEnt = new STORAGE_SHELFCOMPART();
        SSCEnt.SHELFID = ddlShelf.SelectedValue;
        ddlCompart.DataSource = SSCSer.GetAll(SSCEnt);

        ddlCompart.DataTextField = "COMPARTNO";
        ddlCompart.DataValueField = "PK_ID";
        ddlCompart.DataBind();
        ddlCompart.Items.Insert(0, "Select");
    }

    protected void ddlShelf_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadShelfCompart();
    }

    protected void gridPRODUCT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblPRODUCTId = e.Row.FindControl("lblPRODUCTId") as Label;
            Label lblPRODUCTName = e.Row.FindControl("lblPRODUCTName") as Label;
            Label lblPRODUCTCode = e.Row.FindControl("lblPRODUCTCode") as Label;
            Label lblShelf = e.Row.FindControl("lblShelf") as Label;
            Label lblShelfID = e.Row.FindControl("lblShelfID") as Label;
            Label lblCompart = e.Row.FindControl("lblCompart") as Label;
            Label lblCompartID = e.Row.FindControl("lblCompartID") as Label;

            MDEnt = new PRODUCT();
            MDEnt.PK_ID = lblPRODUCTId.Text;
            MDEnt = (PRODUCT)MDSer.GetSingle(MDEnt);
            if (MDEnt != null)
            {
                lblPRODUCTName.Text = MDEnt.PRODUCT_NAME;
                lblPRODUCTCode.Text = MDEnt.PRODUCT_CODE;
            }

            SSCEnt = new STORAGE_SHELFCOMPART();
            SSCEnt.PK_ID = lblCompartID.Text;
            SSCEnt = (STORAGE_SHELFCOMPART)SSCSer.GetSingle(SSCEnt);
            if (SSCEnt != null)
            {
                lblCompart.Text = SSCEnt.COMPARTNO;
                SSEnt = new STORAGE_SHELF();
                SSEnt.PK_ID = SSCEnt.SHELFID;
                SSEnt = (STORAGE_SHELF)SSSer.GetSingle(SSEnt);
                if (SSEnt != null)
                {
                    lblShelf.Text = SSEnt.SHELFNO;
                    lblShelfID.Text = SSEnt.PK_ID;
                }
            }

        }
    }

    protected void gridPRODUCT_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Change"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;

            Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
            Label lblPRODUCTId = gr.FindControl("lblPRODUCTId") as Label;
            Label lblShelf = gr.FindControl("lblShelf") as Label;
            Label lblShelfID = gr.FindControl("lblShelfID") as Label;
            Label lblCompartID = gr.FindControl("lblCompartID") as Label;

            LoadShelf();
            ddlPRODUCT.SelectedValue = lblPRODUCTId.Text;
            ddlShelf.SelectedValue = lblShelfID.Text;
            LoadShelfCompart();
            ddlCompart.SelectedValue = lblCompartID.Text;
            lblPKIDU.Text = lblPK_ID.Text;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string msg = "";
        if (ddlPRODUCT.SelectedValue == "Select")
        {
            msg = "Select PRODUCT";
        }
        if (ddlCompart.SelectedValue == "Select" || ddlCompart.SelectedValue == "")
        {
            msg += "  Select Shelf Compart";
        }

        if (msg == "")
        {
            if (lblPKIDU.Text == "")
            {
                PRODUCT_STORE_AT TempMSAEnt = new PRODUCT_STORE_AT();
                TempMSAEnt.MEDICINE_ID = ddlPRODUCT.SelectedValue;
                TempMSAEnt.COMPART_NO = ddlCompart.SelectedValue;
                TempMSAEnt = (PRODUCT_STORE_AT)MSASer.GetSingle(TempMSAEnt);
                if (TempMSAEnt == null)
                {
                    MSAEnt = new PRODUCT_STORE_AT();
                    MSAEnt.MEDICINE_ID = ddlPRODUCT.SelectedValue;
                    MSAEnt.COMPART_NO = ddlCompart.SelectedValue;
                    MSASer.Insert(MSAEnt);
                }
            }
            else
            {
                MSAEnt = new PRODUCT_STORE_AT();
                MSAEnt.PK_ID = lblPKIDU.Text;
                MSAEnt = (PRODUCT_STORE_AT)MSASer.GetSingle(MSAEnt);
                if (MSAEnt != null)
                {
                    PRODUCT_STORE_AT TempMSAEnt = new PRODUCT_STORE_AT();
                    TempMSAEnt.MEDICINE_ID = ddlPRODUCT.SelectedValue;
                    TempMSAEnt.COMPART_NO = ddlCompart.SelectedValue;
                    TempMSAEnt = (PRODUCT_STORE_AT)MSASer.GetSingle(TempMSAEnt);
                    if (TempMSAEnt == null)
                    {
                        MSAEnt.MEDICINE_ID = ddlPRODUCT.SelectedValue;
                        MSAEnt.COMPART_NO = ddlCompart.SelectedValue;
                        MSASer.Update(MSAEnt);
                    }
                }
            }
            LoadGrid();
        }
        else
            HelperFunction.MsgBox(this, this.GetType(), msg);
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid();
    }
    protected void LoadGrid()
    {
        MSAEnt = new PRODUCT_STORE_AT();
        if (ddlPRODUCT.SelectedValue != "Select")
            MSAEnt.MEDICINE_ID = ddlPRODUCT.SelectedValue;
        if (ddlCompart.SelectedValue != "Select" && ddlCompart.SelectedValue != "")
            MSAEnt.COMPART_NO = ddlCompart.SelectedValue;
        gridPRODUCT.DataSource = MSASer.GetAll(MSAEnt);
        gridPRODUCT.DataBind();
    }

    protected void ddlPRODUCT_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid();
    }
}