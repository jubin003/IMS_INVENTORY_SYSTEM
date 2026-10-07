<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="bikri_khata_old.aspx.cs" Inherits="Reports_old_bikri_khata_old" %>


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
                    ID="gvBikrikhata"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="gridtable"
                    EmptyDataText="No records found">

                    <Columns>
                        <asp:BoundField DataField="PK_ID" HeaderText="ID" />

                        <asp:BoundField DataField="FISCAL_YEAR" HeaderText="Fiscal Year" />

                        <asp:BoundField DataField="TRANSACTION_DATE" HeaderText="Transaction Date" />

                        <asp:BoundField DataField="INVOICE_NUMBER" HeaderText="Invoice No" />

                        <asp:BoundField DataField="PARTY_NAME" HeaderText="Party Name" />

                        <asp:BoundField DataField="PARTY_PAN_VAT" HeaderText="Party PAN/VAT" />

                        <asp:BoundField DataField="PRODUCT_DETAIL" HeaderText="Product Detail" />

                        <asp:BoundField DataField="QUANTITY" HeaderText="Quantity" />

                        <asp:BoundField DataField="UNIT" HeaderText="Unit" />

                        <asp:BoundField DataField="TOTAL_AMOUNT" HeaderText="Total Amount" />

                        <asp:BoundField DataField="EXEMPTED" HeaderText="Exempted" />

                        <asp:BoundField DataField="TAXABLE_AMOUNT" HeaderText="Taxable Amount" />

                        <asp:BoundField DataField="TAX_AMOUNT" HeaderText="Tax Amount" />

                        <asp:BoundField DataField="COUNTRY_OF_EXPORT" HeaderText="Country of Export" />

                        <asp:BoundField DataField="PP_NO" HeaderText="PP No" />

                        <asp:BoundField DataField="EXPORT_DATE" HeaderText="Export Date" />
                    </Columns>
                </asp:GridView>


            </div>
        </div>
    </div>
</asp:Content>

