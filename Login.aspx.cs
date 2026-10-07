using System;
using System.Configuration;
using System.Web.UI;
using Service.Components;
using System.Web.Security;
using Entity.Components;
using System.Data.OracleClient;
using System.Net;
using System.Web;
using PhyeGanCore;
using System.Data.SqlClient;

public partial class Login : System.Web.UI.Page
{
    UserProfileEntity userProfileEnt = new UserProfileEntity();

    FISCALYEAR FYEnt = new FISCALYEAR();
    FISCALYEARService FYSer = new FISCALYEARService();

    Entity.Components.Login theLEntity = new Entity.Components.Login();
    LoginService theLService = new LoginService();


    LOGIN_LOG LLEnt = new LOGIN_LOG();
    LOGIN_LOGService LLSer=new LOGIN_LOGService();


    HelperFunction hf = new HelperFunction();
    ED encdec = new ED();
    PhyeGan PG = new PhyeGan();
    PhyeGanDate PGD = new PhyeGanDate();

    string EmployeeId;
    string imgfolder;
    string cokUsername, cokPassword;
    bool flag = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack != true)
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Session.Clear();
            }
          
        }
        txtUserName.Focus();

        try
        {
            #region to update fiscalyear
            FISCALYEAR FYEnt = new FISCALYEAR();
            FISCALYEARService FYSer = new FISCALYEARService();
            FYEnt = new FISCALYEAR();
            FYEnt.FISCAL_YEAR = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
            FYEnt = (FISCALYEAR)FYSer.GetSingle(FYEnt);
            if (FYEnt == null)//if there is no current FY go in
            {
                FYEnt = new FISCALYEAR();
                FYEnt.ACTIVE = "1";
                FYEnt = (FISCALYEAR)FYSer.GetSingle(FYEnt);
                if (FYEnt != null) // if there is active previous FY go in 
                {
                    FYEnt.ACTIVE = "0";
                    FYSer.Update(FYEnt);
                }
                // add current fiscal year
                FYEnt = new FISCALYEAR();
                FYEnt.FISCAL_YEAR = PGD.checkFiscalYear(PGD.NepaliMonth(), PGD.NepaliYear());
                FYEnt.ACTIVE = "1";
                FYSer.Insert(FYEnt);
            }
            #endregion

            #region to check expiry date
            if (PG.CompanyName() != "")
            {
                string date = PG.SoftwareExpiryDate();
                DateTime myDate = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                TimeSpan ts1 = myDate - DateTime.Today;
                if (ts1.Days <= 35 && ts1.Days > 1)
                {
                    lblLicenseMsg.Text = "Software will expire on " + ts1.Days.ToString() + " days.";
                }
                else if (ts1.Days < 1)
                {
                    lblLicenseMsg.Text = "Software expired. Please Contact Service provide for renewal";
                    linkRenew.Visible = true;
                    linkRegister.Visible = false;
                }
            }
            else
            {
                linkRenew.Visible = false;
                linkRegister.Visible = true;
            }
            #endregion
        }
        catch(Exception eee)
        {
            Response.Redirect("~/ConnectionError.aspx");
        }
    }


    protected void LoginCheck(string username, string password)
    {
        if (PG.CompanyName() != "")// for new Company where there is no data in name cmpany
        {
            LoginServices loginServ = new LoginServices();
            UserProfileEntity userProfileEnt = new UserProfileEntity();

            userProfileEnt = (UserProfileEntity)loginServ.Validate(encdec.md5(username), encdec.md5(txtPassword.Text), null);

            theLEntity = new Entity.Components.Login();
            theLService = new LoginService();

            theLEntity.LOGINID = encdec.md5(username);
            theLEntity.PASSWORD = encdec.md5(txtPassword.Text);
            theLEntity = (Entity.Components.Login)theLService.GetSingle(theLEntity);
            if (theLEntity != null)
            {
                if (PG.SoftwareValidDate() == "On") // for license that gets expired--> On for Expire Off for no expire
                {
                    string date = PG.SoftwareExpiryDate();
                    DateTime myDate = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    TimeSpan ts1 = myDate - DateTime.Today;
                    if (myDate >= DateTime.Today)
                    {
                        EmployeeId = theLEntity.EMPLOYEEID;
                    }
                    else
                    {
                        EmployeeId = "0";
                        lblErrorMessage.Text = "License Expired";
                        return;
                    }
                }
                else if (PG.SoftwareValidDate() == "Off")// for licence that never get expire
                {
                    EmployeeId = theLEntity.EMPLOYEEID;
                }
                userProfileEnt.UserName = ED.Decrypt(theLEntity.PASSWORDQUESTION);
            }

            else
            {
                EmployeeId = "0";
                lblErrorMessage.Text = "Incorrect Username or Password";
                return;
            }
            // start of new code

            EMPLOYEES EEnt = new EMPLOYEES();
            EMPLOYEESService ESer = new EMPLOYEESService();
            EEnt.EMPLOYEEID = EmployeeId;
            EEnt = (EMPLOYEES)ESer.GetSingle(EEnt);
            if (EEnt != null)
            {
                userProfileEnt.LocationID = EEnt.OFFICE_CODE;

                OFFICE oEntity = new OFFICE();
                OFFICEService oService = new OFFICEService();
                oEntity.PK_ID = EEnt.OFFICE_CODE;
                oEntity = (OFFICE)oService.GetSingle(oEntity);
                if (oEntity != null)
                {
                    userProfileEnt.LocationName = oEntity.STREET;
                    userProfileEnt.LocationTypeID = Convert.ToInt32(oEntity.OFFICETYPEID);
                }
            }

            if (userProfileEnt.LoginAccess != true)
            {
                Page.Session["UserProfile"] = userProfileEnt;
                FormsAuthentication.RedirectFromLoginPage(userProfileEnt.UserName, true);
            }
            else
            {
                string NYear = PGD.NepaliYear();
                string NMonth = PGD.NepaliMonth();
                Page.Session["UserProfile"] = userProfileEnt;
                userProfileEnt.FiscalYear = PGD.checkFiscalYear(NMonth, NYear);
                userProfileEnt.EmployeeID = theLEntity.EMPLOYEEID;

                #region for login log
                LLEnt = new LOGIN_LOG();
                LLEnt.USER_ID = theLEntity.EMPLOYEEID;
                EEnt.EMPLOYEEID = EmployeeId;
                EEnt = (EMPLOYEES)ESer.GetSingle(EEnt);
                if (EEnt != null)
                {
                    userProfileEnt.LocationID = EEnt.OFFICE_CODE;
                    LLEnt.LOGIN_DATE_TIME = DateTime.Now.ToString();
                    LLEnt.LOGIN_DATE = PGD.GetTodayDate("dd/mm/yyyy");
                    LLEnt.OFFICE_CODE = EEnt.OFFICE_CODE;
                    LLEnt.LOGIN_IP = GetIPAddress();
                    LLSer.Insert(LLEnt);
                }
                #endregion

                Response.Cookies["uid"].Value = userProfileEnt.UserName;
                Response.Cookies["uid"].Expires = DateTime.Now.AddDays(1);
                Response.Cookies["ug"].Value = userProfileEnt.UserGroupID.ToString();
                Response.Cookies["ug"].Expires = DateTime.Now.AddDays(1);

                Response.Redirect("~/Default.aspx");

            }
        }
       
    }
    protected void btnSignIn_Click(object sender, EventArgs e)
    {
        if (txtUserName.Text != "" && txtPassword.Text != "")
        {
            LoginCheck(txtUserName.Text, txtPassword.Text);
        }
        else
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = "Please Complete Fields";
        }
    }

    protected string GetIPAddress()
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipAddress))
        {
            string[] addresses = ipAddress.Split(',');
            if (addresses.Length != 0)
            {
                return addresses[0];
            }
        }

        return context.Request.ServerVariables["REMOTE_ADDR"];
    }

    protected string GetUser_IP()
    {
        string VisitorsIPAddr = string.Empty;
        if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        {
            VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
        {
            VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
        }
        return VisitorsIPAddr;
    }
}