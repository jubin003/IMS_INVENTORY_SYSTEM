using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PhyeGanCore;
using System.IO;
using Entity.Components;
using System.Web;

public partial class Reports_Purchase_PurchaseReport : System.Web.UI.Page
{
    HelperFunction hf = new HelperFunction();
    PhyeGan pg = new PhyeGan();
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
                path = path.Replace(pg.Org_Base_URL(), "");

                if (hf.checkPageAccess(path, userProfileEnt.UserGroupID.ToString()))
                {
                    loadBranch();
                    txtFromDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
                    txtToDate.Text = PGD.NepaliDay() + "/" + PGD.NepaliMonth() + "/" + PGD.NepaliYear();
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
        ddlBranch.DataSource = pg.getBranchList();
        ddlBranch.DataTextField = "OFFICENAME";
        ddlBranch.DataValueField = "PK_ID";
        ddlBranch.DataBind();

        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        if (userProfileEnt.LocationTypeID == 1 && pg.checkBranchAccess(path, userProfileEnt.UserGroupID.ToString()) && pg.CompanyBranch_Status())
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
    protected void LoadCompany()
    {
        lblCompanyName.Text = pg.CompanyName();
        if (ddlBranch.SelectedValue != "")
        {
            lblAddress.Text = pg.BranchAddress(ddlBranch.SelectedValue);
            lblContact.Text = pg.BranchContact(ddlBranch.SelectedValue);
        }
        else
        {
            lblAddress.Text = "";
            lblContact.Text = "";
        }

    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        string office_code = "";
        if(ddlBranch.SelectedValue != "")
        {
            office_code = ddlBranch.SelectedValue;
        }
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        grdPurchase.DataSource = hf.getPurchaseList("", "", PGD.GetEnglishDateFromNepali(txtFromDate.Text, "dd/mm/yyyy"), PGD.GetEnglishDateFromNepali(txtToDate.Text, "dd/mm/yyyy"), office_code);
        grdPurchase.DataBind();
        divhide.Visible = true;
        lblReportTitle.Text = "Date Wise Purchase";
        if (txtFromDate.Text == txtToDate.Text)
        {
            lblDate.Text = txtFromDate.Text;
        }
        else
        {
            lblDate.Text = txtFromDate.Text + " - " + txtToDate.Text;
        }
        LoadCompany();
    }
    protected void lblInvoiceNo_Click(object sender, EventArgs e)
    {
        GridViewRow gr = ((LinkButton)sender).Parent.Parent as GridViewRow;

        LinkButton lblInvoiceNo = gr.FindControl("lblInvoiceNo") as LinkButton;
        Label lblInvoiceDay = gr.FindControl("lblInvoiceDay") as Label;
        Label lblPK_ID = gr.FindControl("lblPK_ID") as Label;
        string url = "~/reports/purchase/ShowPurchaseInvoice.aspx?dakhno=" + lblPK_ID.Text;
        string fullUrl = ResolveUrl(url);

        // Register JavaScript to open the URL in a new tab
        string script = "window.open('" + fullUrl + "', '_blank');";
        ScriptManager.RegisterStartupScript(this, GetType(), "OpenInvoice", script, true);

    }
    public override void VerifyRenderingInServerForm(Control control)
    {

        // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control.
    }

    protected void btnExcel_Click(object sender, ImageClickEventArgs e)
    {
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename=PURCHASE_XLS" + "_" + PGD.GetTodayNepaliDate() + ".xls");
        //Response.ContentEncoding = Encoding.UTF8; 
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        divhide.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.End();
    }
}