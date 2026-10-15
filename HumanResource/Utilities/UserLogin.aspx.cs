using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Entity.Framework;
using Service.Components;
using PhyeGanCore;

public partial class HumanResource_Utilities_UserLogin : System.Web.UI.Page
{
    HR_EMPLOYEES EEnt = new HR_EMPLOYEES();
    HR_EMPLOYEESService ESer = new HR_EMPLOYEESService();

    HR_DEPARTMENT DPEnt = new HR_DEPARTMENT();
    HR_DEPARTMENTService DPSer = new HR_DEPARTMENTService();

    HR_DIVISION HDIVEnt = new HR_DIVISION();
    HR_DIVISIONService HDIVSer = new HR_DIVISIONService();

    Entity.Components.Login LEnt = new Entity.Components.Login();
    LoginService LSer = new LoginService();

    Groups GEnt = new Groups();
    GroupsService GSer = new GroupsService();

    NAME_COMPANY NCEnt = new NAME_COMPANY();
    NAME_COMPANYService NCSer = new NAME_COMPANYService();

    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfileEnt = new UserProfileEntity();
    PhyeGan pg = new PhyeGan();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                LoadDepartment();
                //LoadEmployees();
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }

    protected void LoadDepartment()
    {
        DPEnt = new HR_DEPARTMENT();
        DPEnt.STATUS = "1";
        ddlDept.DataSource = DPSer.GetAll(DPEnt);
        ddlDept.DataTextField = "DEP_NAME";
        ddlDept.DataValueField = "PK_ID";
        ddlDept.DataBind();
        ddlDept.Items.Insert(0, "Select");
    }

    protected void LoadEmployees()
    {
        if (ddlDept.SelectedValue != "Select")
        {
            EEnt = new HR_EMPLOYEES();
            EntityList EList = new EntityList();
            EEnt.DEPARTMENTID = ddlDept.SelectedValue;
            EEnt.STATUS = "1";
            EList = ESer.GetAll(EEnt);
            if (EList.Count > 0)
            {
                GridEmpList.DataSource = EList;
                GridEmpList.DataBind();
            }
            else
            {
                GridEmpList.DataSource = null;
                GridEmpList.DataBind();
                HelperFunction.MsgBox(this, this.GetType(), "No Employee found !!");
            }
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Please select the department !!");
        }

    }
    protected void bttnSearch_Click(object sender, EventArgs e)
    {
        LoadEmployees();
    }
    protected void GridEmpList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == 0)
        {
            Label lblEmpID = e.Row.FindControl("lblEmpID") as Label;
            Label lblName = e.Row.FindControl("lblName") as Label;
            DropDownList ddlGroup = e.Row.FindControl("ddlGroup") as DropDownList;
            ImageButton imgbtnCreate = e.Row.FindControl("imgbtnCreate") as ImageButton;
            ImageButton imgbtnReset = e.Row.FindControl("imgbtnReset") as ImageButton;
            ImageButton imgbtnBlock = e.Row.FindControl("imgbtnBlock") as ImageButton;
            ImageButton imgbtnUnBlock = e.Row.FindControl("imgbtnUnBlock") as ImageButton;

            GEnt = new Groups();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            GEnt.GROUPPRECEDENCE = "1";
            EntityList thelist = GSer.GetAll(GEnt);
            EntityList newList = new EntityList();
            if (userProfileEnt.UserName != "phyegan")
            {
                foreach (Groups g in thelist)
                {
                    if (g.GROUPID != "2")
                        newList.Add(g);
                }
                ddlGroup.DataSource = newList;
            }
            else
                ddlGroup.DataSource = thelist;

            ddlGroup.DataTextField = "GROUPNAME";
            ddlGroup.DataValueField = "GROUPID";
            ddlGroup.DataBind();

            EEnt = new HR_EMPLOYEES();
            EEnt.EMPLOYEEID = lblEmpID.Text;
            EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
            if (EEnt != null)
            {
                lblName.Text = EEnt.FULL_NAME;
            }


            LEnt = new Entity.Components.Login();
            LEnt.EMPLOYEEID = lblEmpID.Text;
            LEnt = (Entity.Components.Login)LSer.GetSingle(LEnt);
            if (LEnt != null)
            {
                ddlGroup.SelectedValue = LEnt.GROUPID;
                imgbtnCreate.Visible = false;
                if (LEnt.ACCESSBLOCKED == "0")
                {
                    imgbtnBlock.Visible = true;
                    imgbtnUnBlock.Visible = false;
                    imgbtnReset.Visible = true;
                }
                else
                {
                    imgbtnBlock.Visible = false;
                    imgbtnUnBlock.Visible = true;
                    imgbtnReset.Visible = false;
                }

            }
            else
            {
                imgbtnCreate.Visible = true;
                imgbtnBlock.Visible = false;
                imgbtnUnBlock.Visible = false;
                imgbtnReset.Visible = false;
            }

        }
    }

    protected void imgbtnCreate_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");
        Label lblName = (Label)row.FindControl("lblName");
        DropDownList ddlGroup = row.FindControl("ddlGroup") as DropDownList;

        string passwordString = "";
        NCEnt = new NAME_COMPANY();
        EEnt = new HR_EMPLOYEES();
        EEnt.EMPLOYEEID = lblEmpID.Text;
        EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
        if (EEnt != null)
        {

            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
                passwordString = ED.Decrypt(NCEnt.DEFAULT_PASSWORD);

            LEnt = new Entity.Components.Login();
            LEnt.LOGINID = ED.md5(lblEmpID.Text);
            LEnt.FULLDETAILS = lblName.Text;
            LEnt.EMPLOYEEID = lblEmpID.Text;
            LEnt.GROUPID = ddlGroup.SelectedValue;
            LEnt.PASSWORD = ED.md5(passwordString);
            LEnt.EMAIL = EEnt.EMAIL;
            LEnt.PASSWORDQUESTION = ED.Encrypt(lblEmpID.Text);
            LEnt.PASSWORDANSWER = NCEnt.DEFAULT_PASSWORD;
            LEnt.ACCESSBLOCKED = "0";
            LSer.Insert(LEnt);
            HelperFunction.MsgBox(this, this.GetType(), "Default Password is " + passwordString);
        }
        LoadEmployees();
    }

    protected void imgbtnReset_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");
        string passwordString = "";
        EEnt = new HR_EMPLOYEES();
        EEnt.EMPLOYEEID = lblEmpID.Text;
        EEnt = (HR_EMPLOYEES)ESer.GetSingle(EEnt);
        if (EEnt != null)
        {
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
                passwordString = ED.Decrypt(NCEnt.DEFAULT_PASSWORD);
            LEnt = new Entity.Components.Login();
            LEnt.LOGINID = ED.md5(lblEmpID.Text);   
            LEnt = (Entity.Components.Login)LSer.GetSingle(LEnt);
            if (LEnt != null)
            {
                LEnt.PASSWORD = ED.md5(passwordString);
                LEnt.PASSWORDQUESTION = ED.Encrypt(lblEmpID.Text);
                LEnt.PASSWORDANSWER = ED.Encrypt(passwordString);
                LSer.Update(LEnt);
                HelperFunction.MsgBox(this, this.GetType(), "Password has been set to " + passwordString);
            }
        }
        LoadEmployees();
    }

    protected void imgbtnBlock_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");
        ImageButton imgbtnBlock = (ImageButton)row.FindControl("imgbtnBlock");
        ImageButton imgbtnUnBlock = (ImageButton)row.FindControl("imgbtnUnBlock");

        LEnt = new Entity.Components.Login();
        LEnt.LOGINID = ED.md5(lblEmpID.Text);
        LEnt = (Entity.Components.Login)LSer.GetSingle(LEnt);
        if (LEnt != null)
        {
            LEnt.ACCESSBLOCKED = "1";
            LSer.Update(LEnt);
            imgbtnUnBlock.Visible = true;
            imgbtnBlock.Visible = false;
        }
        LoadEmployees();
    }

    protected void imgbtnUnBlock_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");
        ImageButton imgbtnBlock = (ImageButton)row.FindControl("imgbtnBlock");
        ImageButton imgbtnUnBlock = (ImageButton)row.FindControl("imgbtnUnBlock");

        LEnt = new Entity.Components.Login();
        LEnt.LOGINID = ED.md5(lblEmpID.Text);
        LEnt = (Entity.Components.Login)LSer.GetSingle(LEnt);
        if (LEnt != null)
        {
            LEnt.ACCESSBLOCKED = "0";
            LSer.Update(LEnt);

            imgbtnUnBlock.Visible = false;
            imgbtnBlock.Visible = true;
        }
        LoadEmployees();
    }
    protected string password()
    {
        #region to generate password
        string allowedChars = "";
        allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";

        allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";

        allowedChars += "1,2,3,4,5,6,7,8,9,0";

        char[] sep = { ',' };

        string[] arr = allowedChars.Split(sep);

        string passwordString = "";

        string temp = "";

        Random rand = new Random();

        for (int i = 0; i < 5; i++)
        {
            temp = arr[rand.Next(0, arr.Length)];
            passwordString += temp;
        }
        #endregion
        return passwordString;
    }

    protected void btnChangeGroup_Click(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((Button)sender).NamingContainer;
        Label lblEmpID = (Label)row.FindControl("lblEmpID");
        DropDownList ddlGroup = row.FindControl("ddlGroup") as DropDownList;
        LEnt = new Entity.Components.Login();
        LEnt.EMPLOYEEID = lblEmpID.Text;
        LEnt = (Entity.Components.Login)LSer.GetSingle(LEnt);
        if (LEnt != null)
        {
            LEnt.GROUPID = ddlGroup.SelectedValue;
            LSer.Update(LEnt);
        }
        LoadEmployees();
        HelperFunction.MsgBox(this, this.GetType(), "Group changed sucessfully.");
    }


}