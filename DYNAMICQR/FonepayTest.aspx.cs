using System;
using System.Web.UI;

// ============================================================================
// FonepayTest.aspx.cs
//
// Purely the UI shell -- no server logic here. All QR generation / status
// checking happens via plain fetch() calls from the page's client-side JS to
// FonepayApi.aspx, which wraps FonepayDynamicQrService using a properly
// awaited async page task (RegisterAsyncTask). See FonepayApi.aspx.cs.
// ============================================================================

public partial class FonepayTest : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
}
