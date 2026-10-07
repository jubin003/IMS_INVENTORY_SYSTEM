using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class API_TEST_UI_CreateInvoice : System.Web.UI.Page
{
    private const string ApiBaseUrl = "http://192.168.0.29/imsv5_MBCG/";
    private const string ApiEndpoint = "api/sales";

    // Row layout: [0]=ProductId [1]=Quantity [2]=Rate [3]=SchemeDiscount [4]=OfficeCode

    protected void Page_Init(object sender, EventArgs e)
    {
        List<string[]> rows = LoadRowsFromRequest();
        BindDetailRows(rows);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private List<string[]> LoadRowsFromRequest()
    {
        if (IsPostBack)
        {
            string raw = Request.Form[hfDetailsJson.UniqueID];
            if (!string.IsNullOrEmpty(raw))
            {
                try
                {
                    var rows = JsonConvert.DeserializeObject<List<string[]>>(raw);
                    if (rows != null && rows.Count > 0)
                        return rows;
                }
                catch
                {
                    // fall through to default below
                }
            }
        }

        // Initial state: 3 empty rows
        return new List<string[]>
        {
            new[] { "", "", "", "", "" },
            new[] { "", "", "", "", "" },
            new[] { "", "", "", "", "" }
        };
    }

    private void BindDetailRows(List<string[]> rows)
    {
        rptDetails.DataSource = rows;
        rptDetails.DataBind();
    }

    protected void rptDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        var lblSno = (Label)e.Item.FindControl("lblSno");
        lblSno.Text = (e.Item.ItemIndex + 1).ToString();

        var row = (string[])e.Item.DataItem;
        ((TextBox)e.Item.FindControl("txtProductId")).Text = row[0];
        ((TextBox)e.Item.FindControl("txtQty")).Text = row[1];
        ((TextBox)e.Item.FindControl("txtRate")).Text = row[2];
        ((TextBox)e.Item.FindControl("txtSchemeDiscount")).Text = row[3];
    }

    private List<string[]> ReadRowsFromControls()
    {
        var rows = new List<string[]>();

        foreach (RepeaterItem item in rptDetails.Items)
        {
            rows.Add(new[]
            {
                ((TextBox)item.FindControl("txtProductId")).Text,
                ((TextBox)item.FindControl("txtQty")).Text,
                ((TextBox)item.FindControl("txtRate")).Text,
                ((TextBox)item.FindControl("txtSchemeDiscount")).Text
            });
        }

        return rows;
    }

    private void PersistAndRebind(List<string[]> rows)
    {
        hfDetailsJson.Value = JsonConvert.SerializeObject(rows);
        BindDetailRows(rows);
    }

    protected void btnAddRow_Click(object sender, EventArgs e)
    {
        var rows = ReadRowsFromControls();
        rows.Add(new[] { "", "", "", "", "" });
        PersistAndRebind(rows);
    }

    protected void lnkRemove_Click(object sender, EventArgs e)
    {
        var link = (LinkButton)sender;
        int index = int.Parse(link.CommandArgument);

        var rows = ReadRowsFromControls();

        if (rows.Count > 1 && index >= 0 && index < rows.Count)
            rows.RemoveAt(index);

        PersistAndRebind(rows);
    }

    protected async void btnSubmit_Click(object sender, EventArgs e)
    {
        lblResult.Text = "";
        lblResult.CssClass = "";
        string UserNameAPI = "db974238714ca8de634a7ce1d083a14f";
        string PasswordAPI = "83e55696c85bed9bf7b5520302dee0f5";

        if (string.IsNullOrEmpty(UserNameAPI) || string.IsNullOrEmpty(PasswordAPI))
        {
            ShowError("Username and Password are required.");
            return;
        }

        bool isWalkIn = chkWalkIn.Checked;

        if (!isWalkIn && string.IsNullOrEmpty(txtCustomerCode.Text))
        {
            ShowError("Customer code is required unless 'Walk-in' is checked.");
            return;
        }

        var payload = new
        {
            Credentials = new
            {
                USERNAME = UserNameAPI,
                PASSWORD = PasswordAPI
            },
            IsWalkIn = isWalkIn ? 1 : 0,
            Customer = new
            {
                CUSTOMER_CODE = txtCustomerCode.Text,
                CUSTOMER_NAME = txtCustomerName.Text,
                ADDRESS = txtAddress.Text,
                PAN_VAT = txtPanVat.Text,
                PHONE = txtPhone.Text
            },
            Invoice = new
            {
                TRANSACTION_DATE = txtTransDate.Text,
                TRANSACTION_DAY = txtTransDay.Text,
                TRANSACTION_MONTH = txtTransMonth.Text,
                TRANSACTION_YEAR = txtTransYear.Text,
                CUSTOMER_NAME = txtInvCustomerName.Text,
                CUSTOMER_ADDRESS = txtInvCustomerAddress.Text,
                COSTOMER_PAN_VAT = txtInvPanVat.Text,
                DISCOUNT_PERCENT = txtDiscountPercent.Text,
                AGE = txtAge.Text,
                GENDER = ddlGender.SelectedValue,
                MODE_OF_PAYMENT = ddlModeOfPayment.SelectedValue
            },
            Details = BuildDetailLines()
        };

        if (payload.Details.Count == 0)
        {
            ShowError("At least one detail line (Product ID) is required.");
            return;
        }

        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUrl);

                string json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(ApiEndpoint, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    ShowSuccess("Invoice created successfully.<br/>" + responseBody);
                }
                else if ((int)response.StatusCode == 409)
                {
                    ShowError("Duplicate submission detected.<br/>" + responseBody);
                }
                else if ((int)response.StatusCode == 401)
                {
                    ShowError("Incorrect username or password.");
                }
                else
                {
                    ShowError("Error creating invoice (" + response.StatusCode + ").<br/>" + responseBody);
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("Request failed: " + ex.Message);
        }
    }

    private List<object> BuildDetailLines()
    {
        var details = new List<object>();
        int sno = 1;

        foreach (RepeaterItem item in rptDetails.Items)
        {
            string productId = ((TextBox)item.FindControl("txtProductId")).Text;

            if (!string.IsNullOrEmpty(productId))
            {
                details.Add(new
                {
                    SNO = sno.ToString(),
                    PRODUCT_ID = productId,
                    QUANTITY = ((TextBox)item.FindControl("txtQty")).Text,
                    RATE = ((TextBox)item.FindControl("txtRate")).Text,
                    SCHEME_DISCOUNT = ((TextBox)item.FindControl("txtSchemeDiscount")).Text
                });

                sno++;
            }
        }

        return details;
    }

    private void ShowError(string message)
    {
        lblResult.CssClass = "text-danger";
        lblResult.Text = message;
    }

    private void ShowSuccess(string message)
    {
        lblResult.CssClass = "text-success";
        lblResult.Text = message;
    }
}
