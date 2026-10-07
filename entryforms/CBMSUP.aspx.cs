using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.Components;
using Service.Components;
using PhyeGanCore;

public partial class entryforms_CBMSUP : System.Web.UI.Page
{
    NAME_COMPANY NCEnt = new NAME_COMPANY();
    NAME_COMPANYService NCSer = new NAME_COMPANYService();

    UserProfileEntity userProfileEnt = new UserProfileEntity();
    PhyeGan PG = new PhyeGan();
    HelperFunction hf = new HelperFunction();
    static string path = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
                path = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
                path = path.Replace(PG.Org_Base_URL(), "");
                if (hf.checkPageAccess(path, userProfileEnt.UserGroupID.ToString()))
                {
                    if (userProfileEnt.UserName == "phyegan")
                    {
                        NCEnt = new NAME_COMPANY();
                        NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
                        if (NCEnt != null)
                        {
                            try
                            {
                                txtCBMSPassword.Text = NCEnt.CBMS_PASSWORD;
                                txtCBMSUsername.Text = NCEnt.CBMS_USERNAME;
                            }
                            catch { }
                        }
                    }
                    else
                        Response.Redirect("~/forbidden.aspx");
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


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        NCEnt = new NAME_COMPANY();
        NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);

        if (NCEnt != null)
        {
            NCEnt.CBMS_PASSWORD = txtCBMSPassword.Text;
            NCEnt.CBMS_USERNAME = txtCBMSUsername.Text;
            NCSer.Update(NCEnt);
        }
        else
        {
            NCEnt = new NAME_COMPANY();
            NCEnt.CBMS_PASSWORD = txtCBMSPassword.Text;
            NCEnt.CBMS_USERNAME = txtCBMSUsername.Text;
            NCSer.Insert(NCEnt);
        }
    }
}