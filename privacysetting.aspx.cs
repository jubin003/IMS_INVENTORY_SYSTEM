using System;
using System.Web;

using Entity.Components;
using Service.Components;
using System.IO;
using PhyeGanCore;

public partial class privacysetting : System.Web.UI.Page
{
    UserProfileEntity userProfileEnt;
    Entity.Components.Login LEnt = new Entity.Components.Login();
    LoginService LSer = new LoginService();
    HelperFunction hf = new HelperFunction();
    ED encdec = new ED();
    static string employeeid = "";
    string imgfolder;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // lblMsg.Text = "";
            LoadData();
        }


    }
    protected void LoadData()
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
        LEnt = new Entity.Components.Login();
        LEnt.LOGINID = userProfileEnt.UserName;
        LEnt = (Entity.Components.Login)LSer.GetSingle(LEnt);
        if (LEnt != null)
        {
            txtDisplayName.Text = LEnt.FULLDETAILS;
            txtRecoveryEmail.Text = LEnt.EMAIL;
            employeeid = LEnt.EMPLOYEEID;
  
        }
        if (!string.IsNullOrEmpty(employeeid))
        {
            imgfolder = Server.MapPath(@"~/images/user/") + employeeid + ".jpg";
            if (File.Exists(imgfolder))
                imgProfilePic.ImageUrl = @"~/images/user/" + employeeid + ".jpg";
            else
                imgProfilePic.ImageUrl = @"~/images/profile.png";
        }
        else
            imgProfilePic.ImageUrl = @"~/images/profile.png";      


    }
    protected void btnSavePassword_Click(object sender, EventArgs e)
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

        LEnt = new Entity.Components.Login();
        LEnt.EMPLOYEEID = userProfileEnt.EmployeeID;

        LEnt.PASSWORD = encdec.md5(txtOldPassword.Text);
        LEnt = (Entity.Components.Login)LSer.GetSingle(LEnt);
        if (LEnt != null)
        {
            if (txtOldPassword.Text != txtNewPassword.Text)
            {
                if (txtNewPassword.Text == txtConfirmPassword.Text)
                {
                    LEnt.PASSWORD = encdec.md5(txtNewPassword.Text);
                    LEnt.PASSWORDANSWER = ED.Encrypt(txtNewPassword.Text);
                    if (LSer.Update(LEnt) > 0)
                    {
                        lblMsg.Text = "";
                        HelperFunction.MsgBox(this, this.GetType(), "Successfully Password Has Been Changed!!");

                        Session.Clear();

                    }
                    else
                    {
                        lblMsg.Text = "Something Goes Wrong. Please Try Again!!";
                        lblMsg.Visible = true;
                    }
                }
                else
                {
                    lblMsg.Text = "New Password and Confirm Password Doesn't Match. Please Try Again!!";
                    lblMsg.Visible = true; 
                }
            }
            else
            {
                lblMsg.Text = "Old Password and New Password can't be same. Please Try Again!!";
                lblMsg.Visible = true;
            }

        }
        else
        {
            lblMsg.Text = "Your Old Password is not correct, Please Try Again!!";
            lblMsg.Visible = true;
        }

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "", " 

    }

    protected void btnSaveName_Click(object sender, EventArgs e)
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

        LEnt = new Entity.Components.Login();
        LEnt.LOGINID = encdec.md5(userProfileEnt.UserName);
        LEnt = (Entity.Components.Login)LSer.GetSingle(LEnt);
        if (LEnt != null)
        {
            LEnt.FULLDETAILS = txtDisplayName.Text;
            if (LSer.Update(LEnt) > 0)
            {

                HelperFunction.MsgBox(this, this.GetType(), "Successfully Changed Your Display Name");
               
                Response.Redirect("privacysetting.aspx");

            }
        }
    }
    protected void btnSaveEmail_Click(object sender, EventArgs e)
    {
        userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];

        LEnt = new Entity.Components.Login();
        LEnt.LOGINID = encdec.md5(userProfileEnt.UserName);
        LEnt = (Entity.Components.Login)LSer.GetSingle(LEnt);
        if (LEnt != null)
        {
            LEnt.EMAIL = txtRecoveryEmail.Text;
            if (LSer.Update(LEnt) > 0)
            {

                HelperFunction.MsgBox(this, this.GetType(), "Successfully Changed Your Recovery Email Address");
                LoadData();


            }
        }
    }
    protected void btnSaveImage_Click(object sender, EventArgs e)
    {
        string path = "~/images/user/" + employeeid + ".jpg";
        System.IO.File.Delete(HttpContext.Current.Server.MapPath(path));


        string filename = "";
        try
        {
            filename = employeeid + "" + FileUpload1.FileName.Substring(FileUpload1.FileName.IndexOf('.'));
            if (FileUpload1.HasFile)
            {
                FileUpload1.SaveAs(Server.MapPath("~/images/user/" + filename));
                Response.Redirect("privacysetting.aspx");
            }
        }
        catch
        {
        }

        LoadData();
    }
    protected void btnCancelPassword_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
    }
}