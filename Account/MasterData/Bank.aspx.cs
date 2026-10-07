using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;

public partial class Account_MasterData_Bank : System.Web.UI.Page
{
    BANK BEnt = new BANK();
    BANKService BSer = new BANKService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadGrid();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (lblPK_id.Text == "")
        {
            BEnt = new BANK();
            BEnt.BANK_NAME = txtBank.Text;
            BEnt.STATUS = ddlStatus.SelectedValue;
            BSer.Insert(BEnt);
            txtBank.Text = "";
        }
        else
        {
            BEnt = new BANK();
            BEnt.PK_ID = lblPK_id.Text;
            BEnt = (BANK)BSer.GetSingle(BEnt);
            if (BEnt != null)
            {                
                BEnt.BANK_NAME = txtBank.Text;
                BEnt.STATUS = ddlStatus.SelectedValue;
                BSer.Update(BEnt);
                txtBank.Text = "";
                lblPK_id.Text = "";
            }
        }
        LoadGrid();
    }
    protected void LoadGrid()
    {
        BEnt = new BANK();
        gridBank.DataSource = BSer.GetAll(BEnt);
        gridBank.DataBind();
    }
    protected void gridBank_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            Label lblStat = (Label)e.Row.FindControl("lblStat");

            if (lblStatus.Text != "0")
            {
                lblStat.Text = "Available";
            }
            else
            {
                lblStat.Text = " Not Available";
            }
        }
    }
    


    protected void gridBank_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("View"))
        {
            GridViewRow gr = ((ImageButton)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblpkid = gr.FindControl("lblpkid") as Label;
            BEnt = new BANK();
            BEnt.PK_ID = lblpkid.Text;
            BEnt = (BANK)BSer.GetSingle(BEnt);
            if (BEnt != null)
            {
                lblPK_id.Text = lblpkid.Text;
                txtBank.Text = BEnt.BANK_NAME;
                ddlStatus.SelectedValue = BEnt.STATUS;
            }
        }
    }
}