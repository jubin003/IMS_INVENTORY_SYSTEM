using Entity.Components;
using PhyeGanCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_IRD_kharid_khata : System.Web.UI.Page
{

    HelperFunction hf = new HelperFunction();
    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();
    Boolean IsPageRefresh = false;

    UserProfileEntity userProfileEnt = new UserProfileEntity();

    static string path = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["postids"] = System.Guid.NewGuid().ToString();
                Session["postid"] = ViewState["postids"].ToString();

                userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");

                if (hf.checkPageAccess(path, userProfileEnt.UserGroupID.ToString()))
                {
                    loadBranch();
                    txtFiscalYear.Text = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                    int month = Convert.ToInt32(PGD.NepaliMonth());
                    ddlMonth.SelectedValue = month.ToString();

                }
                else
                {
                    Response.Redirect("~/forbidden.aspx");
                }
            }
            else
            {
                if (ViewState["postids"].ToString() != Session["postid"].ToString())
                {
                    IsPageRefresh = true;
                }
                Session["postid"] = System.Guid.NewGuid().ToString();
                ViewState["postids"] = Session["postid"].ToString();
            }
        }
        catch (System.Threading.ThreadAbortException)
        {
            Response.Redirect("~/forbidden.aspx");
        }
        catch
        {
            Response.Redirect("~/Login.aspx");
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
            ddlBranch.Items.Insert(0, "");
        }
        else
        {
            divBranch.Visible = false;
            ddlBranch.SelectedValue = userProfileEnt.LocationID;
        }
    }

    protected void LoadCompanyDetail()
    {
        lblCompanyName.Text = PG.CompanyName();
        if (ddlBranch.SelectedValue != "")
        {
            lblCompanyAddress.Text = PG.BranchAddress(ddlBranch.SelectedValue);           
        }
        else
        {
            lblCompanyAddress.Text = "";
        }
        lblRegNo.Text = PG.CompanyRegistration();
        lblPanNo.Text = PG.CompanyVATPan();
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
        {
            office_code = ddlBranch.SelectedValue;
        }
        grdReport.DataSource = hf.getKHARID_KHATA(ddlMonth.SelectedValue, txtFiscalYear.Text, "",office_code);
        grdReport.DataBind();
        LoadCompanyDetail();
        hide.Visible = true;
        lblMonth.Text = ddlMonth.SelectedItem.ToString();
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename=IRDReport_XLS" + "_" + PGD.GetTodayDate("dd/mm/yyyy") + ".xls");
        //Response.ContentEncoding = Encoding.UTF8; 
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        hide.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblPurchaseMasterID = (Label)e.Row.FindControl("lblPurchaseMasterID");
            Label lblProductDetail = (Label)e.Row.FindControl("lblProductDetail");
            Label lblQuantity = (Label)e.Row.FindControl("lblQuantity");
            Label lblUnit = (Label)e.Row.FindControl("lblUnit");

            lblProductDetail.Text = hf.getKHARID_KHATA_PRODUCT_DETAIL(lblPurchaseMasterID.Text);
            lblQuantity.Text = hf.getKHARID_KHATA_QUANTITY_DETAIL(lblPurchaseMasterID.Text);
            lblUnit.Text = hf.getKHARID_KHATA_UNIT_DETAIL(lblPurchaseMasterID.Text);
        }
    }
}