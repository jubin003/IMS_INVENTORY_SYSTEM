using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Entity.Components;
using Entity.Framework;
using Service.Components;
using DataHelper.Framework;
using PhyeGanCore;

public partial class uc_LoginUsers : System.Web.UI.UserControl
{
    UserProfileEntity userProfileEnt = new UserProfileEntity();
    string m_strSortExp;
    System.Web.UI.WebControls.SortDirection m_SortDirection;
    DataSet theEntListdb;
    EntityList theEntList;
    DataView dv;
    UserPageAccess entUserPageAcc = new UserPageAccess();

    EMPLOYEES EEnt = new EMPLOYEES();
    EMPLOYEESService Eser = new EMPLOYEESService();

    OFFICE OEnt = new OFFICE();
    OFFICEService OSer = new OFFICEService();

    Entity.Components.Login LEnt = new Entity.Components.Login();
    LoginService LSer = new LoginService();

    HelperFunction hf = new HelperFunction();
    PhyeGan PG = new PhyeGan();
    ED encdec = new ED();
    static string path = "";
    private void ClearFields()
    {
        txtLoginid.Text = string.Empty;
        txtFulldetails.Text = string.Empty;
        empId.Text = string.Empty;
        ddlGroupid.SelectedIndex = 0;
        txtPassword.Text = string.Empty;
        ddlBranch.SelectedValue = "1";

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {

                ViewState["postids"] = System.Guid.NewGuid().ToString();
                Session["postid"] = ViewState["postids"].ToString();
                userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
                if (hf.checkPageAccess(path, userProfileEnt.UserGroupID.ToString()))
                {
                    LoadGroup();
                    LoadGrid();
                    loadBranch();
                }
                else
                {
                    Response.Redirect("~/forbidden.aspx");
                }

            }
            catch (Exception ww)
            {
                Response.Redirect("~/Login.aspx");
            }

        }
    }

    public void LoadGroup()
    {
        Groups anEntity = new Groups();
        GroupsService myService = new GroupsService();
        EntityList theList = new EntityList();
        theList = myService.GetAll(anEntity);
        EntityList theListNew = new EntityList();
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (userProfileEnt.UserGroupID == 1)// If Power user PhyeGan
        {
            ddlGroupid.DataSource = theList;
        }
        else
        {
            foreach (Groups g in theList)
            {
                if (g.GROUPNAME != "Administrator")
                    theListNew.Add(g);
            }
            ddlGroupid.DataSource = theListNew;
        }
        ddlGroupid.DataTextField = "GROUPNAME";
        ddlGroupid.DataValueField = "GROUPID";
        ddlGroupid.DataBind();
        ListItem item = new ListItem("Please Select", "0");
        ddlGroupid.Items.Insert(0, item);
    }

    public void LoadData(string loginid)
    {

        LEnt = new Entity.Components.Login();


        LEnt.LOGINID = loginid;
        LEnt = (Entity.Components.Login)LSer.GetSingle(LEnt);

        if (LEnt != null)
        {
            txtLoginid.Text = ED.Decrypt(LEnt.PASSWORDQUESTION);
            txtFulldetails.Text = LEnt.FULLDETAILS;
            empId.Text = LEnt.EMPLOYEEID;
            ddlGroupid.SelectedValue = LEnt.GROUPID;
            txtPassword.Text = ED.Decrypt(LEnt.PASSWORDANSWER);  
            txtEmail.Text = LEnt.EMAIL;
            if (LEnt.ACCESSBLOCKED == "1")
            {
                rbtnBlocked.Checked = true;
                rbtnUnblocked.Checked = false;
            }
            else
            {
                rbtnBlocked.Checked = false;
                rbtnUnblocked.Checked = true;
            }
        }
        else
        {
            txtLoginid.Text = null;
            txtFulldetails.Text = null;
            ddlGroupid.SelectedIndex = 0;
            txtPassword.Text = null;

        }
    }

    private void LoadGrid()
    {
        LEnt = new Entity.Components.Login();
        EntityList theList = new EntityList();
        theList = LSer.GetAll(LEnt);
        EntityList theListNew = new EntityList();
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (userProfileEnt.UserGroupID == 1) // If Power user PhyeGan
        {
            GridView1.DataSource = theList;
            GridView1.DataBind();
        }
        else
        {
            foreach (Entity.Components.Login ln in theList)
            {
                if (ln.GROUPID != "1") // If Not Power user PhyeGan
                    theListNew.Add(ln);
            }
            GridView1.DataSource = theListNew;
            GridView1.DataBind();
        }

        if (GridView1.Rows.Count == 0)
        {
            ArrayList a1 = new ArrayList();
            LEnt = new Entity.Components.Login();
            a1.Add(LEnt);

            GridView1.DataSource = a1;
            GridView1.DataBind();
        }
    }

    protected void loadBranch()
    {
        ddlBranch.DataSource =PG.getBranchList();
        ddlBranch.DataTextField = "OFFICENAME";
        ddlBranch.DataValueField = "PK_ID";
        ddlBranch.DataBind();

        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (userProfileEnt.LocationTypeID == 1 && PG.checkBranchAccess(path, userProfileEnt.UserGroupID.ToString()) && PG.CompanyBranch_Status())
        {
            trBranch.Visible = true;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
        }
        else
        {
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
            trBranch.Visible = false;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DistributedTransaction DT = new DistributedTransaction();

        if (empId.Text == "")
        {
            EEnt = new EMPLOYEES();
            string newempid = hf.GetNewEmployeeId();
            EEnt.EMPLOYEEID = newempid;
            EEnt.FIRSTNAME = txtFulldetails.Text;
            EEnt.DEPARTMENTID = "1";
            EEnt.DESIGNATIONID = "1";
            if (ddlBranch.SelectedValue == "1")
            {
                EEnt.OFFICE_CODE = null;
            }
            else
            {
                EEnt.OFFICE_CODE = ddlBranch.SelectedValue;
            }
            Eser.Insert(EEnt, DT);



            Entity.Components.Login LEnt = new Entity.Components.Login();
            LEnt.LOGINID = encdec.md5(txtLoginid.Text);
            LEnt.PASSWORD = encdec.md5(txtPassword.Text);
            LEnt.EMAIL = txtEmail.Text;
            LEnt.FULLDETAILS = txtFulldetails.Text;
            LEnt.EMPLOYEEID = newempid;
            LEnt.GROUPID = ddlGroupid.SelectedValue;
            LEnt.PASSWORDQUESTION = ED.Encrypt(txtLoginid.Text);
            LEnt.PASSWORDANSWER = ED.Encrypt(txtPassword.Text);

            try
            {
                DateTime lld = new DateTime();
                LEnt.LDOLOGIN = lld.GetDateTimeFormats()[6].ToString();
                if (LEnt.LDOLOGIN == "0001-01-01")
                    LEnt.LDOLOGIN = "";
            }
            catch
            {
                LEnt.LDOLOGIN = "";
            }

            if (rbtnBlocked.Checked == true)
            {
                LEnt.ACCESSBLOCKED = "1";
            }
            else
            {
                LEnt.ACCESSBLOCKED = "0";
            }
            LSer.Insert(LEnt, DT);
        }
        else
        {
            EEnt.EMPLOYEEID = empId.Text;
            EEnt = (EMPLOYEES)Eser.GetSingle(EEnt);
            if (EEnt != null)
            {
                EEnt.FIRSTNAME = txtFulldetails.Text;
                EEnt.DEPARTMENTID = "1";
                EEnt.DESIGNATIONID = "1";
                if (ddlBranch.SelectedValue == "1")
                {
                    EEnt.OFFICE_CODE = null;
                }
                else
                {
                    EEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                }
                Eser.Update(EEnt, DT);

            }

            Entity.Components.Login LEnt = new Entity.Components.Login();
            LEnt.LOGINID = encdec.md5(txtLoginid.Text);
            LEnt.EMPLOYEEID = empId.Text;
            LEnt = (Entity.Components.Login)LSer.GetSingle(LEnt);
            if (LEnt != null)
            {
                LEnt.FULLDETAILS = txtFulldetails.Text;

                LEnt.GROUPID = ddlGroupid.SelectedValue;
                try
                {
                    DateTime lld = new DateTime();
                    LEnt.LDOLOGIN = lld.GetDateTimeFormats()[6].ToString();
                    if (LEnt.LDOLOGIN == "0001-01-01")
                        LEnt.LDOLOGIN = "";
                }
                catch
                {
                    LEnt.LDOLOGIN = "";
                }

                LEnt.PASSWORD = encdec.md5(txtPassword.Text);
                LEnt.PASSWORDANSWER = ED.Encrypt(txtPassword.Text);
                LEnt.EMAIL = txtEmail.Text;
                if (rbtnBlocked.Checked == true)
                {
                    LEnt.ACCESSBLOCKED = "1";
                }
                else
                {
                    LEnt.ACCESSBLOCKED = "0";
                }


                LSer.Update(LEnt, DT);
            }



        }

        if (DT.HAPPY == true)
        {
            DT.Commit();
            HelperFunction.MsgBox(this, this.GetType(), "Data saved Succesfully!!!");
        }
        else
        {
            DT.Abort();
        }
        DT.Dispose();

        txtLoginid.Text = string.Empty;
        txtFulldetails.Text = string.Empty;
        empId.Text = string.Empty;
        ddlGroupid.SelectedIndex = 0;
        txtPassword.Text = string.Empty;
        LoadGrid();

    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((ImageButton)sender).NamingContainer;
        Label lblLoginid = (Label)row.FindControl("lblLoginid");
        Label lblBranch = (Label)row.FindControl("lblBranch");
        Label lblBranchPK_ID = (Label)row.FindControl("lblBranchPK_ID");
        txtLoginid.Enabled = false;
        LoadData(lblLoginid.Text);
        btnPopup_ModalPopupExtender.Show();

        ddlBranch.SelectedValue = lblBranchPK_ID.Text;
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = GridView1.Rows[e.RowIndex];
        Label lblLoginid = (Label)row.FindControl("lblLoginid");
        LEnt.LOGINID = lblLoginid.Text;
        LSer.Delete(LEnt);

        GridView1.EditIndex = -1;
        LoadGrid();
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        ClearFields();
        btnPopup_ModalPopupExtender.Show();
        txtLoginid.ReadOnly = false;
        txtLoginid.Enabled = true;
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        LoadGrid();
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        GridViewRow grdrow = (GridViewRow)GridView1.FooterRow;
        TextBox txtLoginid = (TextBox)GridView1.FooterRow.FindControl("txtLoginid");
        TextBox txtFulldetails = (TextBox)GridView1.FooterRow.FindControl("txtFulldetails");
        TextBox txtEmployeeid = (TextBox)GridView1.FooterRow.FindControl("txtEmployeeid");
        TextBox txtGroupid = (TextBox)GridView1.FooterRow.FindControl("txtGroupid");
        TextBox txtEmail = (TextBox)GridView1.FooterRow.FindControl("txtEmail");
        TextBox txtbu = (TextBox)GridView1.FooterRow.FindControl("txtbu");


        LEnt = new Entity.Components.Login();
        LSer = new LoginService();
        EntityList theList = new EntityList();
        if (txtLoginid != null && txtLoginid.Text != "")
        {
            LEnt.LOGINID = txtLoginid.Text + "%";
        }
        if (txtFulldetails != null && txtFulldetails.Text != "")
        {
            LEnt.FULLDETAILS = txtFulldetails.Text + "%";
        }
        if (txtEmployeeid != null && txtEmployeeid.Text != "")
        {
            LEnt.EMPLOYEEID = txtEmployeeid.Text + "%";
        }
        if (txtGroupid != null && txtGroupid.Text != "")
        {
            LEnt.EMPLOYEEID = txtEmployeeid.Text + "%";
        }
        theList = LSer.GetAll(LEnt);
        if (theList.Count == 0)
        {
            LEnt = new Entity.Components.Login();
            theList.Add(LEnt);
        }
        GridView1.DataSource = theList;
        GridView1.DataBind();

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView1.Columns[2].Visible = false;
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {
            Label lblLoginid = e.Row.FindControl("lblLoginid") as Label;
            Label lblLoginEnc = e.Row.FindControl("lblLoginEnc") as Label;
            Label lblLoginDec = e.Row.FindControl("lblLoginDec") as Label;

            Label lblGroupid = e.Row.FindControl("lblGroupid") as Label;
            Label lblGroupName = e.Row.FindControl("lblGroupName") as Label;

            Label lblOfficeName = e.Row.FindControl("lblOfficeName") as Label;
            Label lblEmployeeid = e.Row.FindControl("lblEmployeeid") as Label;

            Label lblBranch = e.Row.FindControl("lblBranch") as Label;
            Label lblBranchPK_ID = e.Row.FindControl("lblBranchPK_ID") as Label;


            lblLoginDec.Text = ED.Decrypt(lblLoginEnc.Text);
            EMPLOYEES entEmp = new EMPLOYEES();
            EMPLOYEESService srvEmp = new EMPLOYEESService();

            if (lblEmployeeid != null)
            {
                if (lblEmployeeid.Text != "")
                {
                    entEmp.EMPLOYEEID = lblEmployeeid.Text;
                    entEmp = (EMPLOYEES)srvEmp.GetSingle(entEmp);
                    if (entEmp != null)
                    {
                        OFFICE entOffice = new OFFICE();
                        OFFICEService srvOffice = new OFFICEService();
                        entOffice.PK_ID = entEmp.OFFICE_CODE;
                        entOffice = (OFFICE)srvOffice.GetSingle(entOffice);
                        if (entOffice != null)
                        {
                            lblBranchPK_ID.Text = entOffice.PK_ID;
                            lblBranch.Text = entOffice.STREET;
                        }
                        else
                        {
                            lblOfficeName.Text = "N/A";
                        }
                    }
                    else
                    {
                        lblOfficeName.Text = "N/A";
                    }
                }
                else
                {
                    lblOfficeName.Text = "N/A";
                }
            }
            else
            {
                lblOfficeName.Text = "N/A";
            }

            Groups entGroup = new Groups();
            GroupsService srvGroups = new GroupsService();

            if (lblGroupid != null)
            {
                if (lblGroupid.Text != "")
                {
                    entGroup.GROUPID = lblGroupid.Text;
                    entGroup = (Groups)srvGroups.GetSingle(entGroup);
                    if (entGroup != null)
                        lblGroupName.Text = entGroup.GROUPNAME;
                    else
                        lblGroupName.Text = "N/A";
                }
                else
                {
                    lblGroupName.Text = "N/A";
                }
            }
            else
            {
                lblGroupName.Text = "N/A";
            }
        }
    }

}
