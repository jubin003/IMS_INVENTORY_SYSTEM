using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using PhyeGanCore;
using Entity.Framework;

public partial class Administration_Area : System.Web.UI.Page
{
    AREA AEnt = new AREA();
    AREAService ASer = new AREAService();

    HelperFunction hf = new HelperFunction();

    PhyeGan PG = new PhyeGan();

    UserProfileEntity userProfileEnt = new UserProfileEntity();

    EntityList theList = new EntityList();

    static string path = "";
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
                    loadBranch();
                    LoadData();
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

    protected void loadBranch()
    {
        ddlBranch.DataSource = PG.getBranchList();
        ddlBranch.DataTextField = "OFFICENAME";
        ddlBranch.DataValueField = "PK_ID";
        ddlBranch.DataBind();

        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (userProfileEnt.LocationTypeID == 1 && PG.checkBranchAccess(path, userProfileEnt.UserGroupID.ToString()) && PG.CompanyBranch_Status())
        {
            divBranch.Visible = true;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
        }
        else
        {
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
            divBranch.Visible = false;
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        GridViewRow row = grid.HeaderRow;
        TextBox txtAreaCodeH = (TextBox)row.FindControl("txtAreaCodeH");
        TextBox txtAreaNameH = (TextBox)row.FindControl("txtAreaNameH");
        bool check = true;
        if (string.IsNullOrEmpty(txtAreaCodeH.Text))
        {
            check = false;
        }
        if (string.IsNullOrEmpty(txtAreaNameH.Text))
        {           
            check = false;
        }

       
        if (check==true)
        {
            AEnt = new AREA();
            AEnt.AREA_CODE = txtAreaCodeH.Text.ToUpper();
            AEnt = (AREA)ASer.GetSingle(AEnt);
            if (AEnt != null)
            {
                HelperFunction.MsgBox(this, this.GetType(), "Area Code already exist.");
                txtAreaCodeH.Text = "";
                txtAreaCodeH.Focus();
            }
            else
            {
                AEnt = new AREA();
                AEnt.AREA_CODE = txtAreaCodeH.Text.ToUpper();
                AEnt.AREA_NAME = txtAreaNameH.Text;
                AEnt.OFFICE_CODE = ddlBranch.SelectedValue;
                ASer.Insert(AEnt);              
                LoadData();
            }
        }
        else
        {
            HelperFunction.MsgBox(this, this.GetType(), "Fields cant be empty.");
        }
    }
    private void LoadData()
    {
        AEnt = new AREA();
        AEnt.OFFICE_CODE = ddlBranch.SelectedValue;
        theList = ASer.GetAll(AEnt);
        if(theList.Count == 0)
        {
            theList.Add(AEnt);
        }

        grid.DataSource =theList;
        grid.DataBind();

        //grid.DataSource = ASer.GetAll(AEnt);
        //grid.DataBind();

        //if (grid.Rows.Count == 0)
        //{
        //    ArrayList a1 = new ArrayList();
        //    AEnt = new AREA();
        //    AEnt.OFFICE_CODE = ddlBranch.SelectedValue;
        //    a1.Add(AEnt);

        //    grid.DataSource = a1;
        //    grid.DataBind();
        //}
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
        TextBox txtAreaCodeE = (TextBox)row.FindControl("txtAreaCodeE");
        TextBox txtAreaNameE = (TextBox)row.FindControl("txtAreaNameE");

        AEnt = new AREA();
        AEnt.PK_ID = lblPKIDE.Text;
        AEnt = (AREA)ASer.GetSingle(AEnt);
        if (AEnt != null)
        {
            AEnt.AREA_CODE = txtAreaCodeE.Text.ToUpper();
            AEnt.AREA_NAME = txtAreaNameE.Text;
            AEnt.OFFICE_CODE = ddlBranch.SelectedValue;
            ASer.Update(AEnt);
           
        }

        grid.EditIndex = -1;
        LoadData();
    }
    protected void grid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grid.EditIndex = -1;
        LoadData();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
    }
}