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
using System.Web.Mail;
using Entity.Components;
using Service.Components;
using System.Net.Mail;
using System.Net;
using PhyeGanCore;

public partial class ForgotPassword : System.Web.UI.Page
{
    HelperFunction hf = new HelperFunction();
    PhyeGan pg = new PhyeGan();
    ED encdec = new ED();
    Entity.Components.Login theLEntity = new Entity.Components.Login();
    LoginService theLService = new LoginService();

    Entity.Components.PasswordReset PREnt = new Entity.Components.PasswordReset();
    PasswordResetService PRSer = new PasswordResetService();

    NAME_COMPANY NCEnt = new NAME_COMPANY();
    NAME_COMPANYService NCSer = new NAME_COMPANYService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack != true)
        {

            lblErrorMessage.Text = "";

        }
    }
    protected void txtUserName_TextChanged(object sender, System.EventArgs e)
    {
        if (txtUserName.Text != "")
        {
            Entity.Components.Login entlogin = new Entity.Components.Login();
            LoginService srvLogin = new LoginService();
            entlogin.LOGINID = encdec.md5(txtUserName.Text);
            entlogin = (Entity.Components.Login)srvLogin.GetSingle(entlogin);
            if (entlogin != null)
            {
                EMPLOYEES entEmp = new EMPLOYEES();
                EMPLOYEESService srvEmp = new EMPLOYEESService();
                entEmp.EMPLOYEEID = entlogin.EMPLOYEEID;
                entEmp = (EMPLOYEES)srvEmp.GetSingle(entEmp);
                if (entEmp != null)
                {
                    txtEmpId.Text = entEmp.EMPLOYEEID;
                    txtName.Text = hf.getEmployeeName(entEmp.EMPLOYEEID);
                    try
                    {
                        lblEmail.Text = entlogin.EMAIL;
                        string[] email = entlogin.EMAIL.Split('@');
                        string showemail = email[0].Substring(0, 4) + "****@" + email[1];
                        txtEmail.Text = showemail;
                    }
                    catch { HelperFunction.MsgBox(this, this.GetType(), "Sorry Your email address is not regestered. For more help please contact Administrator."); }
                    hidden.Visible = true;
                    lblErrorMessage.Text = "";
                    btnSend.Visible = true;
                }
            }
            else
            {
                lblErrorMessage.Text = "There is no such user";
                hidden.Visible = false;
            }
        }
    }

    protected void btnSend_Click(object sender, System.EventArgs e)
    {
        if (txtEmail.Text != "")
        {
            string reset_ip = "";
            NCEnt = new NAME_COMPANY();
            NCEnt = (NAME_COMPANY)NCSer.GetSingle(NCEnt);
            if (NCEnt != null)
            {
                reset_ip = NCEnt.RESET_IP;
            }

            Random randomNumber = new Random();
            string confirmationcode = "";
            int generatedNo = randomNumber.Next(111111, 999999);

            confirmationcode = generatedNo.ToString();

            PREnt = new Entity.Components.PasswordReset();
            PREnt.EMPLOYEE_ID = txtEmpId.Text;
            PREnt = (Entity.Components.PasswordReset)PRSer.GetSingle(PREnt);
            if (PREnt != null)
            {
                PREnt.CONFIRMATION_CODE = confirmationcode;
                PRSer.Update(PREnt);
            }
            else
            {
                PREnt = new Entity.Components.PasswordReset();
                PREnt.EMPLOYEE_ID = txtEmpId.Text;
                PREnt.CONFIRMATION_CODE = confirmationcode;
                PRSer.Insert(PREnt);
            }
            try
            {
                MailAddress fromAddress = new MailAddress("noreplyphyegan@gmail.com");
                MailAddress toAddress = new MailAddress(lblEmail.Text);

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(fromAddress.Address, toAddress.Address);
                mail.Subject = "Password Recovery ";
                mail.Body += " <html>";
                mail.Body += "<head>";

                mail.Body += "</head>";
                mail.Body += "<body>";
                mail.Body += "<div style='width: 450px;margin:auto;text-align: center;padding: 10px;background-color:#ccc;'><div style='background-color: white; overflow: hidden;'><h1 style='color: #5379ff;'>Hi, " + 
                    txtName.Text + "!</h1><p style='font-size: 16px; margin:30px 0;'></p><p style='font-size: 16px; margin:30px 0;'>Click on that button will take you to new page <br>which will you to reset your password.</p><p style='font-size: 16px; margin:30px 0;'><a href='" + 
                    reset_ip + "/PasswordReset.aspx?id=" + txtEmpId.Text + "&confirm=" + confirmationcode + "' style='font-weight: bold;padding: 5px 10px;background-color: #3356ff;text-align: center;font-size: 24px;text-decoration: none;color:white;border:1px inset #5656ff;border-radius: 5px;'>Click Here</a></p><p style='font-size: 16px; margin:30px 0;'>If You are unable to click the button, follow the link:</p><p style='font-size: 16px; margin:30px 0;'><a href='" + reset_ip + "/PasswordReset.aspx?id=" + txtEmpId.Text + "&confirm=" + confirmationcode + "'>" + reset_ip + "/PasswordReset.aspx</a></p><br><p style='font-size: 16px; margin:30px 0;'>For More Support <br>Contact Phyegan.</p><p style='font-size: 16px; margin:30px 0;'><img style='height: 40px ;margin-right: 10px;' src='https://www.phyegan.com/images/logo.png'></p></div></div>";
                mail.Body += "</body>";
                mail.Body += "</html>";
                mail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("noreplyphyegan@gmail.com", "~!@#$%^^%$#@!~");
                client.Send(mail);








                //MailAddress fromAddress = new MailAddress("noreply@phyegan.com.np");
                //MailAddress toAddress = new MailAddress(txtEmail.Text);

                //System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(fromAddress.Address, toAddress.Address);
                //mail.Subject = "Password Recovery ";
                //mail.Body += " <html>";
                //mail.Body += "<head>";

                //mail.Body += "</head>";
                //mail.Body += "<body>";
                //mail.Body += "<div style='width: 450px;margin:auto;text-align: center;padding: 10px;background-color:#ccc;'><div style='background-color: white; overflow: hidden;'><h1 style='color: #5379ff;'>Hi, " + txtName.Text + "!</h1><p style='font-size: 16px; margin:30px 0;'></p><p style='font-size: 16px; margin:30px 0;'>Click on that button will take you to new page <br>which will you to reset your password.</p><p style='font-size: 16px; margin:30px 0;'><a href='" + reset_ip + "/PasswordReset.aspx?id=" + txtEmpId.Text + "&confirm=" + confirmationcode + "' style='font-weight: bold;padding: 5px 10px;background-color: #3356ff;text-align: center;font-size: 24px;text-decoration: none;color:white;border:1px inset #5656ff;border-radius: 5px;'>Click Here</a></p><p style='font-size: 16px; margin:30px 0;'>If You are unable to click the button, follow the link:</p><p style='font-size: 16px; margin:30px 0;'><a href='" + reset_ip + "/PasswordReset.aspx?id=" + txtEmpId.Text + "&confirm=" + confirmationcode + "'>" + reset_ip + "/PasswordReset.aspx</a></p><br><p style='font-size: 16px; margin:30px 0;'>For More Support <br>Contact Phyegan.</p><p style='font-size: 16px; margin:30px 0;'><img style='height: 40px ;margin-right: 10px;' src='https://www.phyegan.com/images/logo.png'></p></div></div>";
                //mail.Body += "</body>";
                //mail.Body += "</html>";
                //mail.IsBodyHtml = true;

                //SmtpClient client = new SmtpClient();
                //client.Host = "mail.phyegan.com.np";
                //client.Port = 2525;
                //client.EnableSsl = true;
                //client.Timeout = 1000000;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new NetworkCredential("noreply@phyegan.com.np", "N0r3pLy@Phy3g@n");
                //client.Send(mail);




            }
            catch (Exception kerror)
            {
                HelperFunction.MsgBox(this, this.GetType(), kerror.ToString());
            }
            Response.AppendHeader("Refresh", "1;url=login.aspx");
        }
        else
        {
         
            HelperFunction.MsgBox(this, this.GetType(), "Sorry You haven't included your Email Id. For more help please contact Administrator.");

        }

    }
}
