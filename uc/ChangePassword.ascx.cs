using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Entity.Components;
using Service.Components;
public partial class uc_ChangePassword : System.Web.UI.UserControl
{
    UserProfileEntity userProfileEnt;
    Entity.Components.Login anEntity;
    LoginService myService = new LoginService();
    Groups entGroup;
    GroupsService serGroup = new GroupsService();
    HelperFunction hf = new HelperFunction();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            lblLoginid.Text = userProfileEnt.UserName;
            lblEmployeeID.Text = userProfileEnt.EmployeeID;
            lblEmployeeName.Text = hf.getEmployeeName(userProfileEnt.EmployeeID);
            entGroup = new Groups();
            entGroup.GROUPID = userProfileEnt.UserGroupID.ToString(); ;
            entGroup = (Groups)serGroup.GetSingle(entGroup);
            if (entGroup != null)
                lblGroup.Text = entGroup.GROUPNAME;

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        if (lblLoginid.Text == "")
            return;
        anEntity = new Entity.Components.Login();
        anEntity.LOGINID = lblLoginid.Text;
        anEntity.PASSWORD = txtOldPassword.Text;
        anEntity = (Entity.Components.Login)myService.GetSingle(anEntity);
        if (anEntity != null)
        {
            anEntity.PASSWORD = txtPassword1.Text;
            try
            {
                myService.Update(anEntity);
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Password Changed successfully. Please logout and re-login.";
            }
            catch
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Error occured while saving new password.";
            }
            
        }
        else
        {
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Text = "Username or Password didn't match";
        }
    }
    
}
