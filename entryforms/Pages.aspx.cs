using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;

public partial class entryforms_Pages : System.Web.UI.Page
{
    PAGES PEnt = new PAGES();
    PAGESService PSer = new PAGESService();

    Modules MEnt = new Modules();
    ModulesService MSer = new ModulesService();

    SubModules SMEnt = new SubModules();
    SubModulesService SMSer = new SubModulesService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadModule();
            LoadSubModule();
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

    protected void LoadSubModule()
    {
        SMEnt = new SubModules();
        if (ddlModule.SelectedItem.ToString() != "Select")
        {
            SMEnt.MODULE_ID = ddlModule.SelectedValue;
        }
        ddlSubModule.DataSource = SMSer.GetAll(SMEnt);
        ddlSubModule.DataTextField = "SUBMODULE_NAME";
        ddlSubModule.DataValueField = "SUBMODULE_ID";
        ddlSubModule.DataBind();
        ddlSubModule.Items.Insert(0, "Select");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = grid.HeaderRow;
        DropDownList ddlModuleH = (DropDownList)row.FindControl("ddlModuleH");
        DropDownList ddlSubModuleH = (DropDownList)row.FindControl("ddlSubModuleH");
        TextBox txtPageLinkH = (TextBox)row.FindControl("txtPageLinkH");
        TextBox txtPageNameH = (TextBox)row.FindControl("txtPageNameH");
        TextBox txtOrderH = (TextBox)row.FindControl("txtOrderH");


        PEnt = new PAGES();
        PEnt.SHOWINMODULE = ddlModuleH.SelectedValue;
        PEnt.SUBMODULEID = ddlSubModuleH.SelectedValue;
        PEnt.LINKNAME = txtPageNameH.Text; 
        PEnt.PAGENAME = txtPageLinkH.Text.ToLower();
        PEnt.ORDER_BY = txtOrderH.Text;
        PSer.Insert(PEnt);

        LoadData();
    }
    private void LoadData()
    {
        PEnt = new PAGES();
        PEnt.SHOWINMODULE = ddlModule.SelectedValue;
        PEnt.SUBMODULEID = ddlSubModule.SelectedValue;
        grid.DataSource = PSer.GetAll(PEnt);
        grid.DataBind();

        if (grid.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            PEnt = new PAGES();
            a1.Add(PEnt);

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
        DropDownList ddlSubModuleE = (DropDownList)row.FindControl("ddlSubModuleE");
        TextBox txtPageNameE = (TextBox)row.FindControl("txtPageNameE");
        TextBox txtPageLinkE = (TextBox)row.FindControl("txtPageLinkE");
        TextBox txtOrderE = (TextBox)row.FindControl("txtOrderE");

        PEnt = new PAGES();

        PEnt.PK_ID = lblPKIDE.Text;
        PEnt = (PAGES)PSer.GetSingle(PEnt);
        if (PEnt != null)
        {
            PEnt.SHOWINMODULE = ddlModuleE.SelectedValue;
            PEnt.SUBMODULEID = ddlSubModuleE.SelectedValue;
            PEnt.LINKNAME = txtPageNameE.Text;
            PEnt.PAGENAME = txtPageLinkE.Text.ToLower();
            PEnt.ORDER_BY = txtOrderE.Text;
            PSer.Update(PEnt);
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

            SMEnt = new SubModules();
            SMEnt.MODULE_ID = PEnt.SHOWINMODULE;
            ddlSubModuleH.DataSource = SMSer.GetAll(SMEnt);
            ddlSubModuleH.DataTextField = "SUBMODULE_NAME";
            ddlSubModuleH.DataValueField = "SUBMODULE_ID";
            ddlSubModuleH.DataBind();
            ddlSubModuleH.SelectedValue = ddlSubModule.SelectedValue;
        }
        #endregion


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                Label lblPKIDE = (Label)e.Row.FindControl("lblPKIDE");
                DropDownList ddlModuleE = (DropDownList)e.Row.FindControl("ddlModuleE");
                DropDownList ddlSubModuleE = (DropDownList)e.Row.FindControl("ddlSubModuleE");

                PEnt = new PAGES();
                PEnt.PK_ID = lblPKIDE.Text;
                PEnt = (PAGES)PSer.GetSingle(PEnt);
                {
                    MEnt = new Modules();
                    ddlModuleE.DataSource = MSer.GetAll(MEnt);
                    ddlModuleE.DataTextField = "MODULE_NAME";
                    ddlModuleE.DataValueField = "MODULE_ID";
                    ddlModuleE.DataBind();
                    ddlModuleE.SelectedValue = PEnt.SHOWINMODULE;

                    SMEnt = new SubModules();
                    SMEnt.MODULE_ID = PEnt.SHOWINMODULE;
                    ddlSubModuleE.DataSource = SMSer.GetAll(SMEnt);
                    ddlSubModuleE.DataTextField = "SUBMODULE_NAME";
                    ddlSubModuleE.DataValueField = "SUBMODULE_ID";
                    ddlSubModuleE.DataBind();
                    ddlSubModuleE.SelectedValue = PEnt.SUBMODULEID;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblPKID = (Label)e.Row.FindControl("lblPKID");
                Label lblModule = (Label)e.Row.FindControl("lblModule");
                Label lblSubModule = (Label)e.Row.FindControl("lblSubModule");
                PEnt = new PAGES();
                PEnt.PK_ID = lblPKID.Text;
                PEnt = (PAGES)PSer.GetSingle(PEnt);
                {
                    MEnt = new Modules();
                    MEnt.MODULE_ID = PEnt.SHOWINMODULE;
                    MEnt = (Modules)MSer.GetSingle(MEnt);
                    if (MEnt != null)
                    {
                        lblModule.Text = MEnt.MODULE_NAME;
                    }

                    SMEnt = new SubModules();
                    SMEnt.SUBMODULE_ID = PEnt.SUBMODULEID;
                    SMEnt = (SubModules)SMSer.GetSingle(SMEnt);
                    if (SMEnt != null)
                    {
                        lblSubModule.Text = SMEnt.SUBMODULE_NAME;
                    }
                }
            }
        }
    }

    protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSubModule();
    }

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void ddlModuleH_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlModuleH = (DropDownList)grid.HeaderRow.FindControl("ddlModuleH");
        DropDownList ddlSubModuleH = (DropDownList)grid.HeaderRow.FindControl("ddlSubModuleH");        

        SMEnt = new SubModules();
        SMEnt.MODULE_ID = ddlModuleH.SelectedValue;
        ddlSubModuleH.DataSource = SMSer.GetAll(SMEnt);
        ddlSubModuleH.DataTextField = "SUBMODULE_NAME";
        ddlSubModuleH.DataValueField = "SUBMODULE_ID";
        ddlSubModuleH.DataBind();
        ddlSubModuleH.SelectedValue = PEnt.SUBMODULEID;
    }

    protected void btnReOrder_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in grid.Rows)
        {
            Label lblPKID = (Label)gr.FindControl("lblPKID");
            TextBox txtOrderI = (TextBox)gr.FindControl("txtOrderI");

            PEnt = new PAGES();
            PEnt.PK_ID = lblPKID.Text;
            PEnt = (PAGES)PSer.GetSingle(PEnt);
            if (PEnt != null)
            {               
                PEnt.ORDER_BY = txtOrderI.Text;
                PSer.Update(PEnt);
            }
        }
        LoadData();
    }
}