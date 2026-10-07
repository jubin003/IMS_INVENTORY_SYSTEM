using System;
using System.Web.UI.WebControls;
using Entity.Components;
using Entity.Framework;
using Service.Components;
using PhyeGanCore;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data;
using Newtonsoft.Json;
using System.Web.UI;
using System.Collections.Generic;
using System.Web;

public partial class BillSettings_BillSetting : System.Web.UI.Page
{
    OFFICE OEnt = new OFFICE();
    OFFICEService OSer = new OFFICEService();

    EntityList theList = new EntityList();
    PhyeGanDate PGD = new PhyeGanDate();
    PhyeGan PG = new PhyeGan();

    UserProfileEntity userProfileEnt = new UserProfileEntity();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
        }
    }
   

}