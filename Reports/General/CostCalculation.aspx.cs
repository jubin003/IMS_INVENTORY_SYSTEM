using PhyeGanCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using System.IO;

public partial class Reports_General_CostCalculation : System.Web.UI.Page
{
    IMPORT_INVOICE_DETAIL IIDEnt = new IMPORT_INVOICE_DETAIL();
    IMPORT_INVOICE_DETAILService IIDSer = new IMPORT_INVOICE_DETAILService();

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
                    ddlMonth.SelectedValue = PGD.NepaliMonth();
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
    protected void btnShow_Click(object sender, EventArgs e)
    {
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
        {
            office_code = ddlBranch.SelectedValue;
        }
        grdReport.DataSource = hf.getKHARID_KHATA(ddlMonth.SelectedValue, txtFiscalYear.Text, "I", office_code);
        grdReport.DataBind();
        grdReport.Visible = true;
        grdCostCalculation.Visible = false;
        divhide.Visible = false;
        divBtn.Visible = false;
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



    protected void grdReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string office_code = "";
        if (ddlBranch.SelectedValue != "")
        {
            office_code = ddlBranch.SelectedValue;
        }
        if (e.CommandName.Equals("Calculate"))
        {
            GridViewRow gr = ((Button)e.CommandSource).Parent.Parent as GridViewRow;
            Label lblPPNo = gr.FindControl("lblPPNo") as Label;
            lblPPNNumber.Text = lblPPNo.Text;
            grdCostCalculation.DataSource = hf.getCOST_CALCULATION(lblPPNo.Text, txtFiscalYear.Text, office_code);
            grdCostCalculation.DataBind();
            grdReport.Visible = false;
            grdCostCalculation.Visible = true;
            divhide.Visible = true;
            divBtn.Visible = true;

        }
    }

    protected void grdCostCalculation_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        double totalQty = 0;
        double ratio = 1;
        double ToatlAdditionalCost = 0;
        double totalCost = 0;
        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {            
            Label lblPurchaseMaster_id = (Label)e.Row.FindControl("lblPurchaseMaster_id");
            Label lblPP_Number = (Label)e.Row.FindControl("lblPP_Number");
            Label lblQty = (Label)e.Row.FindControl("lblQty");
            Label lblTotalNRS = (Label)e.Row.FindControl("lblTotalNRS");
            

            Label lblOtherCostUpToBorder = (Label)e.Row.FindControl("lblOtherCostUpToBorder");
            Label lblCustomService = (Label)e.Row.FindControl("lblCustomService");
            Label lblCargoVehicleFee = (Label)e.Row.FindControl("lblCargoVehicleFee");
            Label lblImportDuty = (Label)e.Row.FindControl("lblImportDuty");
            Label lblOtherImportDuty = (Label)e.Row.FindControl("lblOtherImportDuty");
            Label lblExciseDuty = (Label)e.Row.FindControl("lblExciseDuty");
            Label lblNepalFreightCharge = (Label)e.Row.FindControl("lblNepalFreightCharge");
            Label lblInsurance = (Label)e.Row.FindControl("lblInsurance");
            Label lblBankCharge = (Label)e.Row.FindControl("lblBankCharge");
            Label lblLoadingUnloadingCharges = (Label)e.Row.FindControl("lblLoadingUnloadingCharges");
            Label lblOtherCost = (Label)e.Row.FindControl("lblOtherCost");
            Label lblVATOnImport = (Label)e.Row.FindControl("lblVATOnImport");

            Label lblTotalAdditionalCost = (Label)e.Row.FindControl("lblTotalAdditionalCost");
            Label lblTotalCost = (Label)e.Row.FindControl("lblTotalCost");
            Label lblCostPerUnitWithOutVAT = (Label)e.Row.FindControl("lblCostPerUnitWithOutVAT");


            Label lblTotalOtherCostUpToBorder = (Label)e.Row.FindControl("lblTotalOtherCostUpToBorder");
            Label lblTotalCustomService = (Label)e.Row.FindControl("lblTotalCustomService");
            Label lblTotalCargoVehicleFee = (Label)e.Row.FindControl("lblTotalCargoVehicleFee");
            Label lblTotalImportDuty = (Label)e.Row.FindControl("lblTotalImportDuty");
            Label lblTotalOtherImportDuty = (Label)e.Row.FindControl("lblTotalOtherImportDuty");
            Label lblTotalExciseDuty = (Label)e.Row.FindControl("lblTotalExciseDuty");
            Label lblTotalNepalFreightCharge = (Label)e.Row.FindControl("lblTotalNepalFreightCharge");
            Label lblTotalInsurance = (Label)e.Row.FindControl("lblTotalInsurance");
            Label lblTotalBankCharge = (Label)e.Row.FindControl("lblTotalBankCharge");
            Label lblTotalLoadingUnloadingCharges = (Label)e.Row.FindControl("lblTotalLoadingUnloadingCharges");
            Label lblTotalOtherCost = (Label)e.Row.FindControl("lblTotalOtherCost");
            Label lblTotalVATOnImport = (Label)e.Row.FindControl("lblTotalVATOnImport");


            totalQty = Convert.ToDouble(hf.getKHARID_KHATA_TOTAL_QUANTITY(lblPurchaseMaster_id.Text));
            ratio = Convert.ToDouble(lblQty.Text) / totalQty;


            lblTotalOtherCostUpToBorder.Text = GetTotalCost(lblPP_Number.Text, "030401");
            lblOtherCostUpToBorder.Text = (ratio * Convert.ToDouble(lblTotalOtherCostUpToBorder.Text)).ToString("0.00");

            lblTotalCustomService.Text = GetTotalCost(lblPP_Number.Text, "030303");
            lblCustomService.Text = (ratio * Convert.ToDouble(lblTotalCustomService.Text)).ToString("0.00");

            lblTotalCargoVehicleFee.Text = GetTotalCost(lblPP_Number.Text, "030305");
            lblCargoVehicleFee.Text = (ratio * Convert.ToDouble(lblTotalCargoVehicleFee.Text)).ToString("0.00");

            lblTotalImportDuty.Text = GetTotalCost(lblPP_Number.Text, "030301");
            lblImportDuty.Text = (ratio * Convert.ToDouble(lblTotalImportDuty.Text)).ToString("0.00");

            lblTotalOtherImportDuty.Text = GetTotalCost(lblPP_Number.Text, "030304");
            lblOtherImportDuty.Text = (ratio * Convert.ToDouble(lblTotalOtherImportDuty.Text)).ToString("0.00");

            lblTotalExciseDuty.Text = GetTotalCost(lblPP_Number.Text, "030302");
            lblExciseDuty.Text = (ratio * Convert.ToDouble(lblTotalExciseDuty.Text)).ToString("0.00");

            lblTotalNepalFreightCharge.Text = GetTotalCost(lblPP_Number.Text, "030501");
            lblNepalFreightCharge.Text = (ratio * Convert.ToDouble(lblTotalNepalFreightCharge.Text)).ToString("0.00");

            lblTotalBankCharge.Text = GetTotalCost(lblPP_Number.Text, "030601");
            lblBankCharge.Text = (ratio * Convert.ToDouble(lblTotalBankCharge.Text)).ToString("0.00");

            lblTotalInsurance.Text = GetTotalCost(lblPP_Number.Text, "030602");
            lblInsurance.Text = (ratio * Convert.ToDouble(lblTotalInsurance.Text)).ToString("0.00");            

            lblTotalLoadingUnloadingCharges.Text = GetTotalCost(lblPP_Number.Text, "030603");
            lblLoadingUnloadingCharges.Text = (ratio * Convert.ToDouble(lblTotalLoadingUnloadingCharges.Text)).ToString("0.00");

            lblTotalOtherCost.Text = GetTotalCost(lblPP_Number.Text, "030604");
            lblOtherCost.Text = (ratio * Convert.ToDouble(lblTotalOtherCost.Text)).ToString("0.00");

            lblTotalVATOnImport.Text = GetTotalCost(lblPP_Number.Text, "030305");
            lblVATOnImport.Text = (ratio * Convert.ToDouble(lblTotalVATOnImport.Text)).ToString("0.00");

            ToatlAdditionalCost = Convert.ToDouble(lblOtherCostUpToBorder.Text) + Convert.ToDouble(lblCustomService.Text) +
                                         Convert.ToDouble(lblCargoVehicleFee.Text) + Convert.ToDouble(lblImportDuty.Text) +
                                         Convert.ToDouble(lblOtherImportDuty.Text) + Convert.ToDouble(lblExciseDuty.Text) +
                                         Convert.ToDouble(lblNepalFreightCharge.Text) + Convert.ToDouble(lblInsurance.Text) +
                                         Convert.ToDouble(lblBankCharge.Text) + Convert.ToDouble(lblLoadingUnloadingCharges.Text) + 
                                         Convert.ToDouble(lblOtherCost.Text);
            lblTotalAdditionalCost.Text = ToatlAdditionalCost.ToString("0.00");
            totalCost = (Convert.ToDouble(lblTotalNRS.Text) + ToatlAdditionalCost);
            lblTotalCost.Text = totalCost.ToString("0.00");
            lblCostPerUnitWithOutVAT.Text = (totalCost / Convert.ToDouble(lblQty.Text)).ToString("0.00");
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblTotalNRSAmount = (Label)e.Row.FindControl("lblTotalNRSAmount");
            double NRStotal = 0;
            foreach(GridViewRow gr in grdCostCalculation.Rows)
            {
                Label lblTotalNRS = (Label)gr.FindControl("lblTotalNRS");
                NRStotal += Convert.ToDouble(lblTotalNRS.Text);
            }

            lblTotalNRSAmount.Text = NRStotal.ToString();


            Label lblTotalOtherCostUpToBorder = (Label)e.Row.FindControl("lblTotalOtherCostUpToBorder");
            Label lblTotalCustomService = (Label)e.Row.FindControl("lblTotalCustomService");
            Label lblTotalCargoVehicleFee = (Label)e.Row.FindControl("lblTotalCargoVehicleFee");
            Label lblTotalImportDuty = (Label)e.Row.FindControl("lblTotalImportDuty");
            Label lblTotalOtherImportDuty = (Label)e.Row.FindControl("lblTotalOtherImportDuty");
            Label lblTotalExciseDuty = (Label)e.Row.FindControl("lblTotalExciseDuty");
            Label lblTotalNepalFreightCharge = (Label)e.Row.FindControl("lblTotalNepalFreightCharge");
            Label lblTotalInsurance = (Label)e.Row.FindControl("lblTotalInsurance");
            Label lblTotalBankCharge = (Label)e.Row.FindControl("lblTotalBankCharge");
            Label lblTotalLoadingUnloadingCharges = (Label)e.Row.FindControl("lblTotalLoadingUnloadingCharges");
            Label lblTotalOtherCost = (Label)e.Row.FindControl("lblTotalOtherCost");
            Label lblTotalVATOnImport = (Label)e.Row.FindControl("lblTotalVATOnImport");

            Label lblTotalAdditionalCost = (Label)e.Row.FindControl("lblTotalAdditionalCost");
            Label lblTotalCost = (Label)e.Row.FindControl("lblTotalCost");

            lblTotalOtherCostUpToBorder.Text = GetTotalCost(lblPPNNumber.Text, "030401");
            lblTotalCustomService.Text = GetTotalCost(lblPPNNumber.Text, "030303");
            lblTotalCargoVehicleFee.Text = GetTotalCost(lblPPNNumber.Text, "030305");
            lblTotalImportDuty.Text = GetTotalCost(lblPPNNumber.Text, "030301");
            lblTotalOtherImportDuty.Text = GetTotalCost(lblPPNNumber.Text, "030304");
            lblTotalExciseDuty.Text = GetTotalCost(lblPPNNumber.Text, "030302");
            lblTotalNepalFreightCharge.Text = GetTotalCost(lblPPNNumber.Text, "030501");
            lblTotalBankCharge.Text = GetTotalCost(lblPPNNumber.Text, "030601");
            lblTotalInsurance.Text = GetTotalCost(lblPPNNumber.Text, "030602");            
            lblTotalLoadingUnloadingCharges.Text = GetTotalCost(lblPPNNumber.Text, "030603");
            lblTotalOtherCost.Text = GetTotalCost(lblPPNNumber.Text, "030604");
            lblTotalVATOnImport.Text = GetTotalCost(lblPPNNumber.Text, "030305");

            ToatlAdditionalCost = Convert.ToDouble(lblTotalOtherCostUpToBorder.Text) + Convert.ToDouble(lblTotalCustomService.Text) +
                                        Convert.ToDouble(lblTotalCargoVehicleFee.Text) + Convert.ToDouble(lblTotalImportDuty.Text) +
                                        Convert.ToDouble(lblTotalOtherImportDuty.Text) + Convert.ToDouble(lblTotalExciseDuty.Text) +
                                        Convert.ToDouble(lblTotalNepalFreightCharge.Text) + Convert.ToDouble(lblTotalInsurance.Text) +
                                        Convert.ToDouble(lblTotalBankCharge.Text) + Convert.ToDouble(lblTotalLoadingUnloadingCharges.Text) +
                                        Convert.ToDouble(lblTotalOtherCost.Text);
            lblTotalAdditionalCost.Text = ToatlAdditionalCost.ToString();
             totalCost = NRStotal + ToatlAdditionalCost;
            lblTotalCost.Text = totalCost.ToString();
        }
    }

    protected string GetTotalCost(string PP_number, string account_head)
    {
        string total = "0.00";
        IIDEnt = new IMPORT_INVOICE_DETAIL();
        IIDEnt.PP_NUMBER = PP_number;
        IIDEnt.FISCAL_YEAR = txtFiscalYear.Text;
        IIDEnt.ACC_HEADING = account_head;
        IIDEnt = (IMPORT_INVOICE_DETAIL)IIDSer.GetSingle(IIDEnt);
        if (IIDEnt != null)
        {
            if (IIDEnt.COST_EFFECT == "1")
                total = IIDEnt.AMOUNT;
        }
        return total;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename=IRDReport_XLS" + "_" + PGD.GetTodayDate("dd/mm/yyyy") + ".xls");
        //Response.ContentEncoding = Encoding.UTF8; 
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        divhide.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "", "printPartOfPage();", true);
    }
}