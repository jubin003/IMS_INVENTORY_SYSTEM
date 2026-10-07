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

public partial class uc_Administration_UserGroupMapping : System.Web.UI.UserControl
{
    UserProfileEntity userProfileEnt = new UserProfileEntity();
    string m_strSortExp;
    System.Web.UI.WebControls.SortDirection m_SortDirection;
    DataSet theEntListdb;

    DataView dv;
    UserPageAccess entUserPageAcc = new UserPageAccess();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { 
            LoadData();
        }
    }

    private void LoadData()
    {
        USERGROUPService myService = new USERGROUPService();
        USERGROUP anEntity = new USERGROUP();        
        GridView1.DataSource = myService.GetAll(anEntity);
        GridView1.DataBind();

        if (GridView1.Rows.Count == 0)
        {
            ArrayList al = new ArrayList();
            Entity.Components.USERGROUP anEntityTemp = new Entity.Components.USERGROUP();
            al.Add(anEntityTemp);

            GridView1.DataSource = al;
            GridView1.DataBind();
        }
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lblUsergroupid = GridView1.Rows[e.RowIndex].FindControl("lblUsergroupid") as Label;

        USERGROUPService myService = new USERGROUPService();
        USERGROUP anEntity = new USERGROUP();

        anEntity.USERGROUPID = lblUsergroupid.Text;
        myService.Delete(anEntity);

        GridView1.EditIndex = -1;
        LoadData();

    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        LoadData();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DropDownList ddGroupidedit = GridView1.Rows[e.RowIndex].FindControl("ddGroupidedit") as DropDownList;
        DropDownList ddUseridedit = GridView1.Rows[e.RowIndex].FindControl("ddUseridedit") as DropDownList;
        Label lblUsergroupid =GridView1.Rows[e.RowIndex].FindControl("lblUsergroupid") as Label;

        USERGROUPService myService = new USERGROUPService();
        USERGROUP anEntity = new USERGROUP();

        anEntity.USERGROUPID = lblUsergroupid.Text;
        anEntity = (USERGROUP)myService.GetSingle(anEntity);
        anEntity.USERID = ddUseridedit.SelectedValue;
        anEntity.GROUPID = ddGroupidedit.SelectedValue;
        myService.Update(anEntity);

        GridView1.EditIndex = -1;
        LoadData();       
    }    
   
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            ((LinkButton)e.Row.FindControl("btnDelete")).Attributes["onclick"] = "if(!confirm('Are you sure?'))return false";

            DropDownList ddUseridedit = e.Row.FindControl("ddUseridedit") as DropDownList;
            DropDownList ddGroupidedit = e.Row.FindControl("ddGroupidedit") as DropDownList;
           
            if (ddUseridedit != null)
            {
                Entity.Components.Login theEntity = new Entity.Components.Login();
                LoginService theService = new LoginService();
                ddUseridedit.DataSource = theService.GetAll(theEntity);
                ddUseridedit.DataValueField = "LOGINID";
                ddUseridedit.DataTextField = "FULLDETAILS";
                ddUseridedit.DataBind();

                Groups anEntity = new Groups();
                GroupsService myService = new GroupsService();
                ddGroupidedit.DataSource = myService.GetAll(anEntity);
                ddGroupidedit.DataTextField = "GROUPNAME";
                ddGroupidedit.DataValueField = "GROUPID";
                ddGroupidedit.DataBind();
                Label lblUsergroupid = e.Row.FindControl("lblUsergroupid") as Label;
                USERGROUP en = new USERGROUP();
                USERGROUPService se = new USERGROUPService();
                en.USERGROUPID = lblUsergroupid.Text;
                en = (USERGROUP)se.GetSingle(en);
                ddGroupidedit.SelectedValue = en.GROUPID;
                ddUseridedit.SelectedValue = en.USERID;
            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            DropDownList ddUserid = e.Row.FindControl("ddUserid") as DropDownList;
            DropDownList ddGroupid = e.Row.FindControl("ddGroupid") as DropDownList;

            if (ddUserid != null)
            {
                Entity.Components.Login theEntity = new Entity.Components.Login();
                LoginService theService = new LoginService();
                ddUserid.DataSource = theService.GetAll(theEntity);
                ddUserid.DataValueField = "LOGINID";
                ddUserid.DataTextField = "FULLDETAILS";
                ddUserid.DataBind();

                Groups anEntity = new Groups();
                GroupsService myService = new GroupsService();
                ddGroupid.DataSource = myService.GetAll(anEntity);
                ddGroupid.DataTextField = "GROUPNAME";
                ddGroupid.DataValueField = "GROUPID";
                ddGroupid.DataBind();
            }
        }
       
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblUserid = e.Row.FindControl("lblUserid") as Label;
            Label lblGroupid = e.Row.FindControl("lblGroupid") as Label;

            Entity.Components.Login theEntity = new Entity.Components.Login();
            LoginService theService = new LoginService();

            Groups anEntity = new Groups();
            GroupsService myService = new GroupsService();
           
            if (!lblUserid.Text.Equals(""))
            {
                theEntity.LOGINID = lblUserid.Text;
                theEntity = (Entity.Components.Login)theService.GetSingle(theEntity);
                lblUserid.Text = theEntity.FULLDETAILS;

                anEntity.GROUPID = lblGroupid.Text;
                anEntity = (Groups)myService.GetSingle(anEntity);
                lblGroupid.Text = anEntity.GROUPNAME;
            }
        }
        
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow row2 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();

            cell = new TableHeaderCell();
            cell.Text = "User Name";
            row2.Cells.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Group Name";
            row2.Cells.Add(cell);


            cell = new TableHeaderCell();
            cell.Text = "";
            row2.Cells.Add(cell);

            GridView1.Controls[0].Controls.Add(row2);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        DropDownList ddUserid = GridView1.HeaderRow.FindControl("ddUserid") as DropDownList;
        DropDownList ddGroupid = GridView1.HeaderRow.FindControl("ddGroupid") as DropDownList;
        Label lblUserid = GridView1.HeaderRow.FindControl("lblUserid") as Label;
        Label lblUsergroupid = GridView1.HeaderRow.FindControl("lblUsergroupid") as Label;

        USERGROUP anEntity = new USERGROUP();
        USERGROUPService myService = new USERGROUPService();
       
            anEntity.USERGROUPID = "1";
            anEntity.USERID = ddUserid.SelectedValue;
            anEntity.GROUPID = ddGroupid.SelectedValue;

            myService.Insert(anEntity);

            GridView1.EditIndex = -1;
            LoadData();      
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        GridViewRow grdrow = (GridViewRow)GridView1.FooterRow;
        TextBox txtGroupid= (TextBox)GridView1.FooterRow.FindControl("txtGroupid");
        TextBox txtUserid = (TextBox)GridView1.FooterRow.FindControl("txtUserid");

        USERGROUP anEntity = new USERGROUP();
        USERGROUPService myService = new USERGROUPService();
        anEntity.USERID = txtUserid.Text;
        anEntity.GROUPID = txtGroupid.Text;        

        try
        {
            theEntListdb = myService.GetDataSet(anEntity);

            GridView1.DataSource = theEntListdb;
            GridView1.DataBind();
        }
        catch (Exception error)
        {
            if (theEntListdb.Tables.Count > 0)
            {
                GridView1.DataSource = theEntListdb;
                GridView1.DataBind();
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }

    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        LoadData();
    }
}
