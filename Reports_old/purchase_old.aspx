<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="purchase_old.aspx.cs" Inherits="Reports_old_purchase_old" %>

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
                    ID="gvOldPurchase"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="gridtable"
                    EmptyDataText="No records found">

                    <Columns>
                        <asp:BoundField DataField="PK_ID" HeaderText="ID" />

                        <asp:BoundField DataField="DAKHILA_NO" HeaderText="Dakhila No" />

                        <asp:BoundField DataField="DAKHILA_FISCAL_YEAR" HeaderText="Fiscal Year" />

                        <asp:BoundField DataField="DAKHILA_DATE" HeaderText="Dakhila Date" />

                        <asp:BoundField DataField="SUPPLIER_INVOICE_NO" HeaderText="Supplier Invoice No" />

                        <asp:BoundField DataField="SUPPLIER" HeaderText="Supplier Name" />

                        <asp:BoundField DataField="QUANTITY" HeaderText="Quantity" />

                        <asp:BoundField DataField="BILL_AMOUNT" HeaderText="Bill Amount" />

                        <asp:BoundField DataField="DISCOUNT" HeaderText="Discount" />

                        <asp:BoundField DataField="SUB_TOTAL" HeaderText="Sub Total" />

                        <asp:BoundField DataField="VAT" HeaderText="VAT" />

                        <asp:BoundField DataField="GRAND_TOTAL" HeaderText="Grand Total" />

                        <asp:BoundField DataField="ROUND_OFF" HeaderText="Round Off" />

                        <asp:BoundField DataField="PAYABLE" HeaderText="Payable Amount" />
                    </Columns>
                </asp:GridView>

            </div>
        </div>
    </div>
</asp:Content>

