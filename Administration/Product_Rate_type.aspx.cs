using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using Entity.Framework;
using PhyeGanCore;

public partial class Administration_Product_Rate_type : System.Web.UI.Page
{
    PRODUCT_RATE_TYPE CTEnt = new PRODUCT_RATE_TYPE();
    PRODUCT_RATE_TYPEService CTSer = new PRODUCT_RATE_TYPEService();


    EntityList theList = new EntityList();
    PhyeGan PG = new PhyeGan();
    HelperFunction hf = new HelperFunction();
    UserProfileEntity userProfileEnt = new UserProfileEntity();
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
                    loadGrid();
                    
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

    protected void loadGrid()
    {
        CTEnt = new PRODUCT_RATE_TYPE();
        CTEnt.OFFICE_CODE = ddlBranch.SelectedValue;
        IList rawList = CTSer.GetAll(CTEnt);

        // Convert to generic list
        List<PRODUCT_RATE_TYPE> filterEnt = rawList.Cast<PRODUCT_RATE_TYPE>().ToList();

        // Filter out PK_ID == "1"
        var filteredList = filterEnt.Where(x => x.PK_ID != "1").ToList();

        // If no data, add a blank row
        if (filteredList == null || filteredList.Count == 0)
        {
            filteredList.Add(new PRODUCT_RATE_TYPE()); // Add empty object
            gridCusType.DataSource = filteredList;
            gridCusType.DataBind();

            // Hide the empty row or disable controls if needed
            gridCusType.Rows[0].Visible = false;
        }
        else
        {
            gridCusType.DataSource = filteredList;
            gridCusType.DataBind();
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        GridViewRow row = gridCusType.HeaderRow;
        TextBox txtCustomerType = (TextBox)row.FindControl("txtCustomerType");
        TextBox txtOrderH = (TextBox)row.FindControl("txtOrderH");
        DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
        DropDownList ddlEditable = (DropDownList)row.FindControl("ddlEditable");
        if (txtCustomerType.Text != "")
        {
            CTEnt = new PRODUCT_RATE_TYPE();
            CTEnt.RATE_TYPE_NAME = txtCustomerType.Text;
            CTEnt.STATUS = ddlStatus.SelectedValue;
            CTEnt.ORDER_BY = txtOrderH.Text;
            CTEnt.OFFICE_CODE = ddlBranch.SelectedValue;
            CTSer.Insert(CTEnt);
            loadGrid();
        }
        else
        {
            HelperFunction.MsgBox(this, GetType(), "Enter Name");
        }
    }

    protected void gridCusType_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridCusType.EditIndex = e.NewEditIndex;
        loadGrid();
    }

    protected void gridCusType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridCusType.EditIndex = -1;
        loadGrid();
    }

    protected void gridCusType_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gridCusType.Rows[e.RowIndex];
        TextBox txtCustomerTypeE = (TextBox)row.FindControl("txtCustomerTypeE");
        TextBox txtOrderE = (TextBox)row.FindControl("txtOrderE");
        DropDownList ddlStatusE = (DropDownList)row.FindControl("ddlStatusE");
        DropDownList ddlEditableE = (DropDownList)row.FindControl("ddlEditableE");
        Label lblPK = (Label)row.FindControl("lblPK");

        CTEnt = new PRODUCT_RATE_TYPE();
        CTEnt.PK_ID = lblPK.Text;
        CTEnt = (PRODUCT_RATE_TYPE)CTSer.GetSingle(CTEnt);
        if (CTEnt != null)
        {
            CTEnt.RATE_TYPE_NAME = txtCustomerTypeE.Text;
            CTEnt.STATUS = ddlStatusE.SelectedValue;
            CTEnt.RATE_EDITABLE = ddlEditableE.SelectedValue;
            CTEnt.ORDER_BY = txtOrderE.Text;
            CTSer.Update(CTEnt);
        }
        gridCusType.EditIndex = -1;
        loadGrid();
    }

    protected void gridCusType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Check if it's a data row and it's not the first row
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Hide the first row

            {
                // Labels and status for non-first rows
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Label lblShowstatus = (Label)e.Row.FindControl("lblShowstatus");
                Label lblEditable = (Label)e.Row.FindControl("lblEditable");
                Label lblShowEditable = (Label)e.Row.FindControl("lblShowEditable");

                if (lblStatus != null)
                {
                    lblShowstatus.Text = lblStatus.Text == "0" ? "UnAvailable" : "Available";
                }

                if (lblEditable != null)
                {
                    lblShowEditable.Text = lblEditable.Text == "0" ? "UnAvailable" : "Available";
                }
            }

            // Handle row editing logic
            if (e.Row.RowType == DataControlRowType.DataRow && gridCusType.EditIndex == e.Row.RowIndex)
            {
                Label lblPK = (Label)e.Row.FindControl("lblPK");
                DropDownList ddlStatusE = (DropDownList)e.Row.FindControl("ddlStatusE");
                DropDownList ddlEditableE = (DropDownList)e.Row.FindControl("ddlEditableE");

                if (lblPK != null)
                {
                    CTEnt = new PRODUCT_RATE_TYPE();
                    CTEnt.PK_ID = lblPK.Text;
                    CTEnt = (PRODUCT_RATE_TYPE)CTSer.GetSingle(CTEnt);

                    if (CTEnt != null)
                    {
                        ddlStatusE.SelectedValue = CTEnt.STATUS;
                        ddlEditableE.SelectedValue = CTEnt.RATE_EDITABLE;
                    }
                }
            }
        }

    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadGrid();
    }
}