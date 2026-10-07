using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;

public partial class entryforms_SubModule : System.Web.UI.Page
{
    Modules MEnt = new Modules();
    ModulesService MSer = new ModulesService();

    SubModules SMEnt = new SubModules();
    SubModulesService SMSer = new SubModulesService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadModule();
        }
    }
    protected void LoadModule()
    {
        MEnt = new Modules();
        ddlModule.DataSource = MSer.GetAll(MEnt);
        ddlModule.DataTextField = "MODULE_NAME";
        ddlModule.DataValueField = "MODULE_ID";
        ddlModule.DataBind();
        ddlModule.Items.Insert(0, "Select");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = grid.HeaderRow;
        DropDownList ddlModuleH = (DropDownList)row.FindControl("ddlModuleH");
        TextBox txtSubModuleH = (TextBox)row.FindControl("txtSubModuleH");
        TextBox txtOrderH = (TextBox)row.FindControl("txtOrderH");


        SMEnt = new SubModules();
        SMEnt.MODULE_ID = ddlModuleH.SelectedValue;
        SMEnt.SUBMODULE_NAME = txtSubModuleH.Text;
        SMEnt.ORDER_BY = txtOrderH.Text;
        SMSer.Insert(SMEnt);

        LoadData();
    }
    private void LoadData()
    {
        SMEnt = new SubModules();
        SMEnt.MODULE_ID = ddlModule.SelectedValue;
        grid.DataSource = SMSer.GetAll(SMEnt);
        grid.DataBind();

        if (grid.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            SMEnt = new SubModules();
            a1.Add(SMEnt);

            grid.DataSource = a1;
            grid.DataBind();
        }
    }

    protected void grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grid.EditIndex = e.NewEditIndex;
        LoadData();
    }
    protected void grid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = grid.Rows[e.RowIndex];
        Label lblPKIDE = (Label)row.FindControl("lblPKIDE");
        DropDownList ddlModuleE = (DropDownList)row.FindControl("ddlModuleE");
        TextBox txtSubModuleE = (TextBox)row.FindControl("txtSubModuleE");
        TextBox txtOrderE = (TextBox)row.FindControl("txtOrderE");

        SMEnt = new SubModules();

        SMEnt.SUBMODULE_ID = lblPKIDE.Text;
        SMEnt = (SubModules)SMSer.GetSingle(SMEnt);
        if (SMEnt != null)
        {
            SMEnt.MODULE_ID = ddlModuleE.SelectedValue;
            SMEnt.SUBMODULE_NAME = txtSubModuleE.Text;
            SMEnt.ORDER_BY = txtOrderE.Text;
            SMSer.Update(SMEnt);
        }

        grid.EditIndex = -1;
        LoadData();
    }

    protected void grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grid.EditIndex = -1;
        LoadData();
    }

    protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        #region Header
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DropDownList ddlModuleH = (DropDownList)e.Row.FindControl("ddlModuleH");
            DropDownList ddlSubModuleH = (DropDownList)e.Row.FindControl("ddlSubModuleH");

            MEnt = new Modules();
            ddlModuleH.DataSource = MSer.GetAll(MEnt);
            ddlModuleH.DataTextField = "MODULE_NAME";
            ddlModuleH.DataValueField = "MODULE_ID";
            ddlModuleH.DataBind();
            ddlModuleH.SelectedValue = ddlModule.SelectedValue;
            
        }
        #endregion


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                Label lblPKIDE = (Label)e.Row.FindControl("lblPKIDE");
                DropDownList ddlModuleE = (DropDownList)e.Row.FindControl("ddlModuleE");
                DropDownList ddlSubModuleE = (DropDownList)e.Row.FindControl("ddlSubModuleE");

                SMEnt = new SubModules();
                SMEnt.SUBMODULE_ID = lblPKIDE.Text;
                SMEnt = (SubModules)SMSer.GetSingle(SMEnt);
                {
                    MEnt = new Modules();
                    ddlModuleE.DataSource = MSer.GetAll(MEnt);
                    ddlModuleE.DataTextField = "MODULE_NAME";
                    ddlModuleE.DataValueField = "MODULE_ID";
                    ddlModuleE.DataBind();
                    ddlModuleE.SelectedValue = SMEnt.MODULE_ID;

                    
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblPKID = (Label)e.Row.FindControl("lblPKID");
                Label lblModule = (Label)e.Row.FindControl("lblModule");
                Label lblSubModule = (Label)e.Row.FindControl("lblSubModule");
                SMEnt = new SubModules();
                SMEnt.SUBMODULE_ID = lblPKID.Text;
                SMEnt = (SubModules)SMSer.GetSingle(SMEnt);
                {
                    MEnt = new Modules();
                    MEnt.MODULE_ID = SMEnt.MODULE_ID;
                    MEnt = (Modules)MSer.GetSingle(MEnt);
                    if (MEnt != null)
                    {
                        lblModule.Text = MEnt.MODULE_NAME;
                    }
                }
            }
        }
    }

   
    protected void btnLoad_Click(object sender, EventArgs e)
    {
        LoadData();
    }
   

    protected void btnReOrder_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in grid.Rows)
        {
            Label lblPKID = (Label)gr.FindControl("lblPKID");
            TextBox txtOrderI = (TextBox)gr.FindControl("txtOrderI");

            SMEnt = new SubModules();
            SMEnt.SUBMODULE_ID = lblPKID.Text;
            SMEnt = (SubModules)SMSer.GetSingle(SMEnt);
            if (SMEnt != null)
            {
                SMEnt.ORDER_BY = txtOrderI.Text;
                SMSer.Update(SMEnt);
            }
        }
        LoadData();
    }
}