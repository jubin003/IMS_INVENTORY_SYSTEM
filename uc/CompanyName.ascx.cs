using System;
using PhyeGanCore;


public partial class uc_CompanyName : System.Web.UI.UserControl
{
    PhyeGan pg = new PhyeGan();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblNameCompany.Text = pg.CompanyName();
        lblAddress.Text = pg.CompanyAddress();
        if(pg.CompanyEmail()=="" && pg.CompanyWebsite()=="")
        {
            divWebsite.Visible = false;
        }
        else
        {
            divWebsite.Visible = true;
        }
        lblEmail.Text =  pg.CompanyEmail();
        lblWebsite.Text =  pg.CompanyWebsite();
        lblContactDetail.Text = pg.CompanyContact();
    }
}