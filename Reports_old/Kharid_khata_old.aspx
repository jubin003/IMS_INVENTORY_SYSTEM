<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Kharid_khata_old.aspx.cs" Inherits="Reports_old_Kharid_khata_old" %>

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
                <asp:Button Text="View" ID="btn_view" CssClass="btn btn-success" OnClick="btn_view_Click"   runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <br />
                <asp:GridView
                    ID="gvOldKharidKhata"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="gridtable"
                    EmptyDataText="No records found">

                    <Columns>
                        <asp:BoundField DataField="PK_ID" HeaderText="ID" />

                        <asp:BoundField DataField="FISCAL_YEAR" HeaderText="Fiscal Year" />

                        <asp:BoundField DataField="TRANSACTION_DATE" HeaderText="Transaction Date" />

                        <asp:BoundField DataField="INVOICE_NUMBER" HeaderText="Invoice No" />

                        <asp:BoundField DataField="PP_NO" HeaderText="PP No" />

                        <asp:BoundField DataField="SUPPLIER_NAME" HeaderText="Supplier Name" />

                        <asp:BoundField DataField="SUPPLIER_PAN_VAT" HeaderText="Supplier PAN/VAT" />

                        <asp:BoundField DataField="PRODUCT_DETAIL" HeaderText="Product Detail" />

                        <asp:BoundField DataField="QUANTITY" HeaderText="Quantity" />

                        <asp:BoundField DataField="UNIT" HeaderText="Unit" />

                        <asp:BoundField DataField="TOTAL_AMOUNT" HeaderText="Total Amount" />

                        <asp:BoundField DataField="EXEMPTED" HeaderText="Exempted" />

                        <asp:BoundField DataField="TAXABLE_AMOUNT" HeaderText="Taxable Amount" />

                        <asp:BoundField DataField="TAX_AMOUNT" HeaderText="Tax Amount" />

                        <asp:BoundField DataField="IMPORT_AMOUNT" HeaderText="Import Amount" />

                        <asp:BoundField DataField="IMPORT_TAX" HeaderText="Import Tax" />

                        <asp:BoundField DataField="ASSET_PURCHASE_AMOUNT" HeaderText="Asset Purchase Amount" />

                        <asp:BoundField DataField="ASSET_TAX" HeaderText="Asset Tax" />
                    </Columns>
                </asp:GridView>

            </div>
        </div>
    </div>
</asp:Content>

