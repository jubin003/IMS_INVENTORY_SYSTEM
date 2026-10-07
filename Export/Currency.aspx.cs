using System;
using System.Web.UI.WebControls;
using System.Collections;
using Entity.Components;
using Service.Components;
using System.Data.SqlClient;
using System.Configuration;
using PhyeGanCore;

public partial class Export_Currency : System.Web.UI.Page
{
    FOREIGN_CURRENCY FCEnt = new FOREIGN_CURRENCY();
    FOREIGN_CURRENCYService FCSer = new FOREIGN_CURRENCYService();
    PhyeGan PG = new PhyeGan();
    HelperFunction hf = new HelperFunction();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridCurrency.HeaderRow;
        TextBox txtCurrencyCodeH = (TextBox)row.FindControl("txtCurrencyCodeH");
        TextBox txtCurrencyNameH = (TextBox)row.FindControl("txtCurrencyNameH");
        DropDownList ddlStatusH = (DropDownList)row.FindControl("ddlStatusH");

        if (string.IsNullOrEmpty(txtCurrencyNameH.Text))
        {
            HelperFunction.MsgBox(this, this.GetType(), "Main Product Name Field cant be empty.");
        }
        else
        {
            FCEnt = new FOREIGN_CURRENCY();
            FCEnt.CURRENCY_CODE = txtCurrencyCodeH.Text;
            FCEnt.CURRENCY_NAME = txtCurrencyNameH.Text;
            FCEnt.STATUS = ddlStatusH.SelectedValue;
            string pk_id = FCSer.Insert(FCEnt).ToString();

            LoadData();
        }
    }
    private void LoadData()
    {
        FCEnt = new FOREIGN_CURRENCY();
        gridCurrency.DataSource = FCSer.GetAll(FCEnt);
        gridCurrency.DataBind();

        if (gridCurrency.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            FCEnt = new FOREIGN_CURRENCY();
            a1.Add(FCEnt);

            gridCurrency.DataSource = a1;
            gridCurrency.DataBind();
        }
    }

    protected void gridCurrency_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridCurrency.EditIndex = e.NewEditIndex;
        LoadData();
    }
    protected void gridCurrency_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridCurrency.Rows[e.RowIndex];
        Label lblPKIDE = (Label)row.FindControl("lblPKIDE");
        TextBox txtCurrencyNameE = (TextBox)row.FindControl("txtCurrencyNameE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");

        FCEnt = new FOREIGN_CURRENCY();

        FCEnt.PK_ID = lblPKIDE.Text;
        FCEnt = (FOREIGN_CURRENCY)FCSer.GetSingle(FCEnt);
        if (FCEnt != null)
        {
            FCEnt.CURRENCY_NAME = txtCurrencyNameE.Text;
            FCEnt.STATUS = ddlStatusE.SelectedValue;

            FCSer.Update(FCEnt);

        }

        gridCurrency.EditIndex = -1;
        LoadData();
    }

    protected void gridCurrency_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridCurrency.EditIndex = -1;
        LoadData();
    }

    protected void gridCurrency_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridCurrency.PageIndex = e.NewPageIndex;
        LoadData();
    }
    protected void gridCurrency_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            TextBox txtCurrencyCodeH = e.Row.FindControl("txtCurrencyCodeH") as TextBox;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                Label lblStatusE = (Label)e.Row.FindControl("lblStatusE");
                DropDownList ddlStatusE = (DropDownList)e.Row.FindControl("ddlStatusE");
                ddlStatusE.SelectedValue = lblStatusE.Text;
            }

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Label lblStatusShow = (Label)e.Row.FindControl("lblStatusShow");
                if (lblStatus.Text == "1")
                    lblStatusShow.Text = "Active";
                else
                    lblStatusShow.Text = "Inactive";
            }
        }
    }
}