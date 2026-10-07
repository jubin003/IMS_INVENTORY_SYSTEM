<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="sales_old.aspx.cs" Inherits="Reports_old_sales_old" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-2">
                Fiscal Year<br />
                <asp:DropDownList ID="ddlfy" CssClass="form-control" runat="server">

                    <asp:ListItem Text="2081/82" />
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <br />
                <asp:Button Text="View" ID="btn_view" CssClass="btn btn-success" OnClick="btn_view_Click" runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <br />
                <asp:GridView
                    ID="gvOldSales"
                    runat="server"
                    AutoGenerateColumns="False"
                   CssClass="gridtable"
                    EmptyDataText="No records found">

                    <Columns>
                        <asp:BoundField DataField="PK_ID" HeaderText="ID" />

                        <asp:BoundField DataField="INVOICE_FY" HeaderText="Invoice FY" />

                        <asp:BoundField DataField="INVOICE_DATE" HeaderText="Invoice Date" />

                        <asp:BoundField DataField="INVOICE_NUMBER" HeaderText="Invoice No" />

                        <asp:BoundField DataField="PAN_VAT_NO" HeaderText="PAN / VAT No" />

                        <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer Name" />

                        <asp:BoundField DataField="SUB_TOTAL" HeaderText="Sub Total" />

                        <asp:BoundField DataField="QUANTITY" HeaderText="Quantity" />

                        <asp:BoundField DataField="DISCOUNT" HeaderText="Discount" />

                        <asp:BoundField DataField="TOTAL" HeaderText="Total" />

                        <asp:BoundField DataField="VAT" HeaderText="VAT" />

                        <asp:BoundField DataField="GRAND_TOTAL" HeaderText="Grand Total" />

                        <asp:BoundField DataField="ROUND_OFF" HeaderText="Round Off" />

                        <asp:BoundField DataField="INVOICE_AMOUNT" HeaderText="Invoice Amount" />

                        <asp:BoundField DataField="SALES_MODE" HeaderText="Sales Mode" />
                    </Columns>
                </asp:GridView>

            </div>
        </div>
    </div>
</asp:Content>


