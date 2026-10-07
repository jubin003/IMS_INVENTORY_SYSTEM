using System;
using System.Web;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DataHelper.Framework;
using System.Web.UI;
using Entity.Framework;

public partial class DYNAMICQR_Keys : System.Web.UI.Page
{
    DYNAMIC_QR DQREnt = new DYNAMIC_QR();
    DYNAMIC_QRService DQRSer = new DYNAMIC_QRService();

    UserProfileEntity userProfile = new UserProfileEntity();
    HelperFunction hf = new HelperFunction();
    string path = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                userProfile = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();

                LoadExisting();
            }
            catch (System.Threading.ThreadAbortException)
            {
                Response.Redirect("~/forbidden.aspx");
            }
            catch (Exception)
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }

    // Loads the single Dynamic_QR configuration row (if one already exists) so the
    // form behaves as an "edit" screen instead of always inserting a new row.
    protected void LoadExisting()
    {
        DQREnt = new DYNAMIC_QR();
        DQREnt = (DYNAMIC_QR)DQRSer.GetSingle(DQREnt);

        if (DQREnt != null)
        {
            lblPK_ID.Text = DQREnt.PK_ID;

            ddlQrStatus.SelectedValue = DQREnt.QR_STATUS;
            ddlQrDevice.SelectedValue = DQREnt.QR_DEVICE;
            ddlQrScreen.SelectedValue = DQREnt.QR_SCREEN;

            txtWsUrl.Text = DQREnt.NPI_WSURL;
            txtWsUsername.Text = DQREnt.NPI_WSUSERNAME;
            txtWsApiToken.Text = DQREnt.NPI_WSAPITOKEN;
            txtNchlPublicKey.Text = DQREnt.NPI_NCHLPUBLICKEY;
            txtBaseUrl.Text = DQREnt.NPI_BASEURL;
            txtUsername.Text = DQREnt.NPI_USERNAME;
            txtPassword.Text = DQREnt.NPI_PASSWORD;
            txtPfxPassword.Text = DQREnt.NPI_PFXPASSWORD;
            txtAcquirerId.Text = DQREnt.NPI_ACQUIRERID;
            txtMerchantId.Text = DQREnt.NPI_MERCHANTID;
            txtMerchantName.Text = DQREnt.NPI_MERCHANTNAME;
            txtMerchantCategoryCode.Text = DQREnt.NPI_MERCHANTCATEGORYCODE;
            txtMerchantCity.Text = DQREnt.NPI_MERCHANTCITY;
            txtMerchantCountry.Text = DQREnt.NPI_MERCHANTCOUNTRY;
            txtMerchantPostalCode.Text = DQREnt.NPI_MERCHANTPOSTALCODE;
            txtStoreLabel.Text = DQREnt.NPI_STORELABEL;
            txtTerminalLabel.Text = DQREnt.NPI_TERMINALLABEL;
            txtUserId.Text = DQREnt.NPI_USERID;
            txtPOSAPIKey.Text = DQREnt.POS_API_KEY;
            txtPOSBaseURL.Text = DQREnt.POS_BASE_URL;
            txtNPIpfxpath.Text = DQREnt.NPI_PFXPATH;
        }
    }
    
    protected void btnUploadPfx_Click(object sender, EventArgs e)
    {
        lblUploadMsg.Text = "";

        if (!fuPfxFile.HasFile)
        {
            lblUploadMsg.Text = "Please choose a file to upload.";
            return;
        }

       
        try
        {
            string certsFolder = Server.MapPath("~/certs/");
            if (!System.IO.Directory.Exists(certsFolder))
                System.IO.Directory.CreateDirectory(certsFolder);

            string fileName = System.IO.Path.GetFileName(fuPfxFile.FileName);
            string fullPath = System.IO.Path.Combine(certsFolder, fileName);

            if (System.IO.File.Exists(fullPath))
            {
                lblUploadMsg.Text = "File already exists.";
                return;
            }

            fuPfxFile.SaveAs(fullPath);
            txtNPIpfxpath.Text = fullPath;
            lblUploadMsg.Text = "File uploaded successfully.";
        }
        catch (Exception ex)
        {
            lblUploadMsg.Text = "Upload failed: " + ex.Message;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string msg = "";

        if (ddlQrStatus.SelectedValue == "")
            msg = msg + "Select QR Status. ";

        if (msg != "")
        {
            HelperFunction.MsgBox(this, this.GetType(), msg);
            return;
        }

        DQREnt = new DYNAMIC_QR();
        DQREnt.QR_STATUS = ddlQrStatus.SelectedValue;
        DQREnt.QR_DEVICE = ddlQrDevice.SelectedValue;
        DQREnt.QR_SCREEN = ddlQrScreen.SelectedValue;

        DQREnt.NPI_WSURL = txtWsUrl.Text;
        DQREnt.NPI_WSUSERNAME = txtWsUsername.Text;
        DQREnt.NPI_WSAPITOKEN = txtWsApiToken.Text;
        DQREnt.NPI_NCHLPUBLICKEY = txtNchlPublicKey.Text;
        DQREnt.NPI_BASEURL = txtBaseUrl.Text;
        DQREnt.NPI_USERNAME = txtUsername.Text;
        DQREnt.NPI_PASSWORD = txtPassword.Text;
        DQREnt.NPI_PFXPASSWORD = txtPfxPassword.Text;
        DQREnt.NPI_ACQUIRERID = txtAcquirerId.Text;
        DQREnt.NPI_MERCHANTID = txtMerchantId.Text;
        DQREnt.NPI_MERCHANTNAME = txtMerchantName.Text;
        DQREnt.NPI_MERCHANTCATEGORYCODE = txtMerchantCategoryCode.Text;
        DQREnt.NPI_MERCHANTCITY = txtMerchantCity.Text;
        DQREnt.NPI_MERCHANTCOUNTRY = txtMerchantCountry.Text;
        DQREnt.NPI_MERCHANTPOSTALCODE = txtMerchantPostalCode.Text;
        DQREnt.NPI_STORELABEL = txtStoreLabel.Text;
        DQREnt.NPI_TERMINALLABEL = txtTerminalLabel.Text;
        DQREnt.NPI_USERID = txtUserId.Text;
        DQREnt.POS_API_KEY = txtPOSAPIKey.Text;
        DQREnt.POS_BASE_URL = txtPOSBaseURL.Text;
        DQREnt.NPI_PFXPATH = txtNPIpfxpath.Text;

        if (lblPK_ID.Text != "")
        {
            // existing row -> update
            DQREnt.PK_ID = lblPK_ID.Text;
            DQRSer.Update(DQREnt);
            HelperFunction.MsgBox(this, this.GetType(), "Updated Successfully.");
        }
        else
        {
            // no existing row -> insert
            string pk_id = DQRSer.Insert(DQREnt).ToString();
            lblPK_ID.Text = pk_id;
            HelperFunction.MsgBox(this, this.GetType(), "Saved Successfully.");
        }

        LoadExisting();
    }
}
