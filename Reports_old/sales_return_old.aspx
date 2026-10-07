<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="sales_return_old.aspx.cs" Inherits="Reports_old_sales_return_old" %>

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
                    ID="gvOldSalesRet"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="gridtable"
                    EmptyDataText="No records found">

                    <Columns>
                        <asp:BoundField DataField="PK_ID" HeaderText="ID" />

                        <asp:BoundField DataField="FISCAL_YEAR" HeaderText="Fiscal Year" />

                        <asp:BoundField DataField="CREDIT_NOTE_DATE" HeaderText="Credit Note Date" />

                        <asp:BoundField DataField="CREDIT_NOTE_NUMBER" HeaderText="Credit Note No" />

                        <asp:BoundField DataField="REF_BILL_NO" HeaderText="Ref Bill No" />

                        <asp:BoundField DataField="REF_BILL_DATE" HeaderText="Ref Bill Date" />

                        <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer Name" />

                        <asp:BoundField DataField="CUSTOMER_PAN_VAT" HeaderText="Customer PAN/VAT" />

                        <asp:BoundField DataField="AMOUNT" HeaderText="Amount" />

                        <asp:BoundField DataField="DISCOUNT" HeaderText="Discount" />

                        <asp:BoundField DataField="TAXABLE_AMOUNT" HeaderText="Taxable Amount" />

                        <asp:BoundField DataField="TAX_AMOUNT" HeaderText="Tax Amount" />

                        <asp:BoundField DataField="TOTAL_AMOUNT" HeaderText="Total Amount" />

                        <asp:BoundField DataField="CREATED_DATE" HeaderText="Created Date" />

                        <asp:BoundField DataField="LAST_UPDATED" HeaderText="Last Updated" />
                    </Columns>
                </asp:GridView>

            </div>
        </div>
    </div>
</asp:Content>


