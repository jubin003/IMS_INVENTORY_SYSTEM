using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Service.Components;
using Entity.Components;
using Entity.Framework;


public partial class uc_Administration_GroupPermission : System.Web.UI.UserControl
{
    UserProfileEntity userProfileEnt = new UserProfileEntity();
    string m_strSortExp;
    System.Web.UI.WebControls.SortDirection m_SortDirection;
    DataSet theEntListdb;

    DataView dv;
    UserPageAccess entUserPageAcc = new UserPageAccess();

    Groups gEntity;
    GroupsService gService;
    permission pEntity;
    permissionService pService;
    Modules mEntity;
    ModulesService mService;
    SubModules sEntity;
    SubModulesService sService;
    permission OldPEntity;
    EntityList theEntList = new EntityList();
    string strunused = "0";
    string strCreator;
    string strApprover;
    string strViewer;
    string strread;
    string stradd;
    string strdelete;
    string strmodify;
    string strpermission;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadGroup(); // Loads the Group Names Combo
            LoadGrid(); // Load the values in grid
            LoadModule(); // Loads the moduels names in the Combo
            LoadSubmodule(); // Loads the sub modules in the combo
            LoadGrouppop(); // Loads the grups names in the popup combo
            LoadModulename();// Loads the module name in the module name combo
        }
    }
    private void LoadGrid()
    {
        pEntity = new permission();
        pService = new permissionService();
        pEntity.GROUP_ID = ddlGroup.SelectedValue.ToString();
        pEntity.MODULE_ID = ddlModulename.SelectedValue.ToString();
        theEntList = pService.GetAllExistingPer(pEntity);
        GridView1.DataSource = theEntList;
        GridView1.DataBind();
    }
    private void LoadGroup()
    {
        gEntity = new Groups();
        gService = new GroupsService();
        ddlGroup.DataSource = gService.GetAll(gEntity);
        ddlGroup.DataTextField = "GROUPNAME";
        ddlGroup.DataValueField = "GROUPID";
        ddlGroup.DataBind();
    }
    public void LoadGrouppop()
    {
        gEntity = new Groups();
        gService = new GroupsService();
        ddlGrouppop.DataSource = gService.GetAll(gEntity);
        ddlGrouppop.DataTextField = "GROUPNAME";
        ddlGrouppop.DataValueField = "GROUPID";
        ddlGrouppop.DataBind();

    }
    public void LoadModule()
    {
        mEntity = new Modules();
        mService = new ModulesService();
        ddlModule.DataSource = mService.GetAll(mEntity);
        ddlModule.DataTextField = "MODULE_NAME";
        ddlModule.DataValueField = "MODULE_ID";
        ddlModule.DataBind();
    }
    public void LoadModulename()
    {
        mEntity = new Modules();
        mService = new ModulesService();
        ddlModulename.DataSource = mService.GetAll(mEntity);
        ddlModulename.DataTextField = "MODULE_NAME";
        ddlModulename.DataValueField = "MODULE_ID";
        ddlModulename.DataBind();
    }
    public void LoadSubmodule()
    {
        ddlSubmodule.Items.Clear();
        sEntity = new SubModules();
        sService = new SubModulesService();
        try
        {
            if (Convert.ToInt32(ddlModule.SelectedItem.Value) > 0)
            {
                sEntity.MODULE_ID = ddlModule.SelectedValue.ToString();
                ddlSubmodule.DataSource = sService.GetAll(sEntity);
                ddlSubmodule.DataValueField = "SUBMODULE_ID";
                ddlSubmodule.DataTextField = "SUBMODULE_NAME";
                ddlSubmodule.DataBind();
            }
        }
        catch
        { }
    }
    public void LoadSubmodule(string ModuleID)
    {
        sEntity = new SubModules();
        sService = new SubModulesService();
        sEntity.MODULE_ID = ModuleID;
        ddlSubmodule.DataSource = sService.GetAll(sEntity);
        ddlSubmodule.DataValueField = "SUBMODULE_ID";
        ddlSubmodule.DataTextField = "SUBMODULE_NAME";
        ddlSubmodule.DataBind();
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lblPermid = GridView1.Rows[e.RowIndex].FindControl("lblPermid") as Label;
        pEntity = new permission();
        pService = new permissionService();

        pEntity.PERM_ID = lblPermid.Text;
        pService.Delete(pEntity);

        GridView1.EditIndex = -1;
        LoadGrid();

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblModule = e.Row.FindControl("lblModule") as Label;
            Label lblSubmodule = e.Row.FindControl("lblSubmodule") as Label;
            Label lblPermission = e.Row.FindControl("lblPermission") as Label;
            Label lblModuleGrid = (Label)e.Row.FindControl("lblModuleID");
            Label lblPageName = (Label)e.Row.FindControl("lblPageName");
            Label lblPageid = (Label)e.Row.FindControl("lblPageid");


            PageLinkNodesEntity PageEnt = new PageLinkNodesEntity();

            mEntity = new Modules();
            mService = new ModulesService();

            sEntity = new SubModules();
            sService = new SubModulesService();

            mEntity.MODULE_ID = lblModule.Text;
            mEntity = (Modules)mService.GetSingle(mEntity);
            lblModule.Text = mEntity.MODULE_NAME;
            lblModuleGrid.Text = mEntity.MODULE_ID;


            sEntity.SUBMODULE_ID = lblSubmodule.Text;
            sEntity = (SubModules)sService.GetSingle(sEntity);
            lblSubmodule.Text = sEntity.SUBMODULE_NAME;

            string str = lblPermission.Text;
            string strp = "";
            if (str != "")
            {
                if (str.Substring(0, 1) == "1")
                {
                    strp += "V. Check";
                }
                if (str.Substring(1, 1) == "1")
                {
                    strp += "V. Pass";
                }
                if (str.Substring(2, 1) == "1")
                {
                    strp += "V. Approve ";
                }
                if (str.Substring(4, 1) == "1")
                {
                    strp += "Read ";
                }
                if (str.Substring(5, 1) == "1")
                {
                    strp += "Add ";
                }
                if (str.Substring(6, 1) == "1")
                {
                    strp += "Modify ";
                }
                if (str.Substring(7, 1) == "1")
                {
                    strp += "Branch Permission ";
                }
                lblPermission.Text = strp;
            }
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ClearControls();
        OldPEntity = new permission();
        pService = new permissionService();
        GridViewRow row = (GridViewRow)((Button)sender).NamingContainer;
        string officeCode = GridView1.Rows[row.RowIndex].Cells[0].Text;
        Label lblPermid = GridView1.Rows[row.RowIndex].FindControl("lblPermid") as Label;
        Label lblModuleIDGrid = (Label)row.Cells[0].FindControl("lblModuleID");
        OldPEntity.PERM_ID = lblPermid.Text.ToString();
        OldPEntity = (permission)pService.GetSingle(OldPEntity);
        if (lblModuleIDGrid != null)
        {
            LoadSubmodule(lblModuleIDGrid.Text);
            LoadPopup(lblPermid.Text);
            AddEdit_ModalPopupExtender.Show();
        }
    }
    public void LoadPopup(string permissioncode)
    {

        pEntity = new permission();
        pService = new permissionService();

        pEntity.PERM_ID = permissioncode;
        pEntity = (permission)pService.GetSingle(pEntity);
        if (pEntity != null)
        {
            ddlGrouppop.SelectedValue = pEntity.GROUP_ID;
            ddlModule.SelectedValue = pEntity.MODULE_ID;
            ddlSubmodule.SelectedValue = pEntity.SUBMODULE_ID;

            string str = pEntity.PERMISSION_SUB;
            //if (str != "")
            //{
            //    if (str.Substring(0, 1) == "1")
            //        chkCreator.Checked = true;
            //    else
            //        chkCreator.Checked = false;

            //    if (str.Substring(1, 1) == "1")
            //        chkApprover.Checked = true;
            //    else
            //        chkApprover.Checked = false;

            //    if (str.Substring(2, 1) == "1")
            //        chkviewer.Checked = true;
            //    else
            //        chkviewer.Checked = false;

            //    if (str.Substring(4, 1) == "1")
            //        chkRead.Checked = true;
            //    else
            //        chkRead.Checked = false;

            //    if (str.Substring(5, 1) == "1")
            //        chkAdd.Checked = true;
            //    else
            //        chkAdd.Checked = false;

            //    if (str.Substring(6, 1) == "1")
            //        chkModify.Checked = true;
            //    else
            //        chkModify.Checked = false;

            //    if (str.Substring(7, 1) == "1")
            //        chkDelete.Checked = true;
            //    else
            //        chkDelete.Checked = false;
            //}
        }
    }
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGrid();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        LoadGrouppop();
        LoadModule();
        ddlModule.Items.Insert(0, "-- Select Items --");
        LoadSubmodule();
        ddlSubmodule.Items.Insert(0, "-- Select Items --");
        ClearControls();
        ddlGrouppop.SelectedItem.Text = ddlGroup.SelectedItem.Text;
        ddlModule.SelectedItem.Text = ddlModulename.SelectedItem.Text;
        ddlGrouppop.SelectedItem.Value = ddlGroup.SelectedItem.Value;
        ddlModule.SelectedItem.Value = ddlModulename.SelectedItem.Value;

        gvPages.DataSource = null;
        gvPages.DataBind();
        LoadSubmodule();
        LoadPages();

        AddEdit_ModalPopupExtender.Show();
    }
    private void ClearControls()
    {
        ddlGrouppop.SelectedIndex = 0;
        ddlModule.SelectedIndex = 0;
        ddlSubmodule.SelectedIndex = -1;
        //chkRead.Checked = false;
        //chkAdd.Checked = false;
        //chkDelete.Checked = false;        
        //chkModify.Checked = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        pEntity = new permission();
        pEntity.GROUP_ID = ddlGrouppop.SelectedValue.ToString();
        OldPEntity = new permission();
        OldPEntity.SUBMODULE_ID = pEntity.SUBMODULE_ID;
        OldPEntity.GROUP_ID = pEntity.GROUP_ID;
        foreach (GridViewRow dr in gvPages.Rows)
        {
            if (dr.RowType == DataControlRowType.DataRow)
            {
                string PageNo = dr.Cells[0].Text.ToString();
                try
                {
                    OldPEntity.PageID = PageNo;
                }
                catch
                {
                }
                CheckBox chk_Read = (CheckBox)dr.FindControl("chk_Read");
                CheckBox chk_Modify = (CheckBox)dr.FindControl("chk_Modify");
                CheckBox chk_Add = (CheckBox)dr.FindControl("chk_Add");
                CheckBox chk_Delete = (CheckBox)dr.FindControl("chk_Delete");
                CheckBox chk_aCreator = (CheckBox)dr.FindControl("chk_aCreator");
                CheckBox chk_AApprover = (CheckBox)dr.FindControl("chk_AApprover");
                CheckBox chk_AViewer = (CheckBox)dr.FindControl("chk_AViewer");

                if (chk_aCreator.Checked == true)
                    strCreator = "1";
                else
                    strCreator = "0";

                if (chk_AApprover.Checked == true)
                    strApprover = "1";
                else
                    strApprover = "0";

                if (chk_AViewer.Checked == true)
                    strViewer = "1";
                else
                    strViewer = "0";

                if (chk_Add.Checked == true)
                    stradd = "1";
                else
                    stradd = "0";

                if (chk_Read.Checked == true)
                    strread = "1";
                else
                    strread = "0";

                if (chk_Modify.Checked == true)
                    strmodify = "1";
                else
                    strmodify = "0";

                if (chk_Delete.Checked == true)
                    strdelete = "1";
                else
                    strdelete = "0";

                pService = new permissionService();
                strpermission = strCreator + strApprover + strViewer + strunused + strread + stradd + strmodify + strdelete;
                pEntity.PERMISSION_SUB = strpermission;
                pEntity.PageID = PageNo;
                OldPEntity = new permission();
                OldPEntity.GROUP_ID = ddlGrouppop.SelectedValue.ToString();
                OldPEntity.PageID = PageNo;
                OldPEntity = (permission)pService.GetSingle(OldPEntity);

                if (OldPEntity == null)
                    pService.Insert(pEntity);
                else
                {
                    pEntity.PERM_ID = OldPEntity.PERM_ID;
                    pService.Update(pEntity);
                }
            }

        }
        LoadGrid();
    }
    private void LoadPages()
    {
        permission ent = new permission();
        permissionService srv = new permissionService();
        EntityList entListNodes = new EntityList();
        ent.SUBMODULE_ID = ddlSubmodule.SelectedValue;
        ent.GROUP_ID = ddlGrouppop.SelectedValue.ToString();
        entListNodes = srv.GetAll(ent);
        gvPages.DataSource = entListNodes;
        gvPages.DataBind();
        SiteMapNode nodeLink = null;

    }
    protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSubmodule();
        LoadPages();
    }
    protected void ddlSubmodule_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPages();
    }
    protected void gvPages_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Get Page Name
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string PageNo = e.Row.Cells[0].Text.ToString();
            PageLinkNodesEntity entPageDetails = new PageLinkNodesEntity();
            PageServices srvPageDetails = new PageServices();
            entPageDetails.pageID = PageNo;
            entPageDetails = (PageLinkNodesEntity)srvPageDetails.GetSingle(entPageDetails);
            if (entPageDetails != null)
            {
                Label lblPageName = (Label)e.Row.FindControl("lblPage_Name");
                if (lblPageName != null)
                {
                    lblPageName.Text = entPageDetails.Pagename;
                }
            }

            pEntity = new permission();
            pService = new permissionService();

            pEntity.GROUP_ID = ddlGrouppop.SelectedValue.ToString();
            pEntity.MODULE_ID = ddlModule.SelectedValue.ToString();
            pEntity.SUBMODULE_ID = ddlSubmodule.SelectedValue.ToString();
            pEntity.PageID = PageNo;
            pEntity = (permission)pService.GetSingle(pEntity);
            if (pEntity != null)
            {
                CheckBox chk_Read = (CheckBox)e.Row.FindControl("chk_Read");
                CheckBox chk_Modify = (CheckBox)e.Row.FindControl("chk_Modify");
                CheckBox chk_Add = (CheckBox)e.Row.FindControl("chk_Add");
                CheckBox chk_Delete = (CheckBox)e.Row.FindControl("chk_Delete");
                CheckBox chk_aCreator = (CheckBox)e.Row.FindControl("chk_aCreator");
                CheckBox chk_AApprover = (CheckBox)e.Row.FindControl("chk_AApprover");
                CheckBox chk_AViewer = (CheckBox)e.Row.FindControl("chk_AViewer");

                ddlGrouppop.SelectedValue = pEntity.GROUP_ID;
                ddlModule.SelectedValue = pEntity.MODULE_ID;
                ddlSubmodule.SelectedValue = pEntity.SUBMODULE_ID;

                string str = pEntity.PERMISSION_SUB;
                if (str != "")
                {
                    if (str.Substring(0, 1) == "1")
                    {
                        if (chk_aCreator != null)
                            chk_aCreator.Checked = true;
                        else
                            chk_aCreator.Checked = false;
                    }

                    if (str.Substring(1, 1) == "1")
                    {
                        if (chk_AApprover != null)
                            chk_AApprover.Checked = true;
                        else
                            chk_AApprover.Checked = false;
                    }

                    if (str.Substring(2, 1) == "1")
                    {
                        if (chk_AViewer != null)
                            chk_AViewer.Checked = true;
                        else
                            chk_AViewer.Checked = false;
                    }

                    if (str.Substring(4, 1) == "1")
                    {
                        if (chk_Read != null)
                            chk_Read.Checked = true;
                        else
                            chk_Read.Checked = false;
                    }

                    if (str.Substring(5, 1) == "1")
                    {
                        if (chk_Add != null)
                            chk_Add.Checked = true;
                        else
                            chk_Add.Checked = false;
                    }

                    if (str.Substring(6, 1) == "1")
                    {
                        if (chk_Modify != null)
                            chk_Modify.Checked = true;
                        else
                            chk_Modify.Checked = false;
                    }

                    if (str.Substring(7, 1) == "1")
                    {
                        if (chk_Delete != null)
                            chk_Delete.Checked = true;
                        else
                            chk_Delete.Checked = false;
                    }
                }
            }
        }


        // Set the page Permissions
    }
    protected void gvPages_SelectedIndexChanged(object sender, EventArgs e)
    {
        ModuleSubmoduleEntity ent = new ModuleSubmoduleEntity();
        ModuleSubModuleService srv = new ModuleSubModuleService();
        EntityList entListNodes = new EntityList();
        PageLinkNodesEntity theEnt = new PageLinkNodesEntity();
        entListNodes = srv.GetLinkNodes(ent);
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        pEntity = new permission();
        pService = new permissionService();
        pEntity.GROUP_ID = ddlGroup.SelectedValue.ToString();
        pEntity.MODULE_ID = ddlModulename.SelectedValue.ToString();
        theEntList = pService.GetAllExistingPer(pEntity);

        if (theEntList != null)
        {
            GridView1.DataSource = theEntList;
        }
        else
        {
            GridView1.DataSource = null;
        }
        GridView1.DataBind();
    }
    protected void ddlModulename_SelectedIndexChanged(object sender, EventArgs e)
    {
        pEntity = new permission();
        pService = new permissionService();
        pEntity.GROUP_ID = ddlGroup.SelectedValue.ToString();
        pEntity.MODULE_ID = ddlModulename.SelectedValue.ToString();
        theEntList = pService.GetAllExistingPer(pEntity);
        GridView1.DataSource = theEntList;
        GridView1.DataBind();

    }
    protected void gvPages_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPages.PageIndex = e.NewPageIndex;
        LoadPages();
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        GridViewRow grdrow = (GridViewRow)GridView1.FooterRow;
        TextBox txtModulename = (TextBox)GridView1.FooterRow.FindControl("txtModulename");
        TextBox txtSubmodulename = (TextBox)GridView1.FooterRow.FindControl("txtSubmodulename");
        TextBox txtPagename = (TextBox)GridView1.FooterRow.FindControl("txtPagename");



        pEntity = new permission();
        pService = new permissionService();
        Modules mEnt = new Modules();
        ModulesService mSer = new ModulesService();
        SubModules sEnt = new SubModules();
        SubModulesService sSer = new SubModulesService();
        PAGES pageEnt = new PAGES();
        PAGESService pageSer = new PAGESService();

        if (!txtModulename.Text.Equals(string.Empty))
        {
            mEnt.MODULE_NAME = txtModulename.Text;
            mEnt = (Modules)mSer.GetSingle(mEnt);
            if (mEnt != null)
            {
                pEntity.MODULE_ID = mEnt.MODULE_ID;
            }
            else
            {
                pEntity.MODULE_ID = null;
            }
        }

        if (!txtSubmodulename.Text.Equals(string.Empty))
        {
            sEnt.SUBMODULE_NAME = txtSubmodulename.Text;
            sEnt = (SubModules)sSer.GetSingle(sEnt);
            if (sEnt != null)
            {
                pEntity.SUBMODULE_ID = sEnt.SUBMODULE_ID;
            }
            else
            {
                pEntity.SUBMODULE_ID = null;
            }
        }

        if (!txtPagename.Text.Equals(string.Empty))
        {
            pageEnt.PAGENAME = txtPagename.Text;
            pageEnt = (PAGES)pageSer.GetSingle(pageEnt);
            if (pageEnt != null)
            {
                pEntity.PageID = pageEnt.PK_ID;
            }
            else
            {
                pEntity.PageID = null;
            }
        }

        try
        {
            theEntListdb = pService.GetDataSet(pEntity);

            //if (theEntList.Count == 0)
            //{
            //    theEntList.Add(new Pdo());
            //}
            GridView1.DataSource = theEntListdb;
            GridView1.DataBind();
            //lblErrorSearch.Visible = false;
        }
        catch (Exception error)
        {
            if (theEntListdb.Tables.Count > 0)
            {
                GridView1.DataSource = theEntListdb;
                GridView1.DataBind();
                //lblErrorSearch.Visible = true;
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                // lblErrorSearch.ForeColor = System.Drawing.Color.Red;
                // lblErrorSearch.Text = "Sorry NO Data Found!!!!!!! ";
            }
        }

    }

}
