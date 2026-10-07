<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="materializedView_old.aspx.cs" Inherits="Reports_old_materializedView_old" %>

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
                    ID="gvOldMaterialized"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="gridtable"
                    EmptyDataText="No records found">

                    <Columns>
                        <asp:BoundField DataField="PK_ID" HeaderText="ID" />

                        <asp:BoundField DataField="FISCAL_YEAR" HeaderText="Fiscal Year" />

                        <asp:BoundField DataField="BILL_NO" HeaderText="Bill No" />

                        <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer Name" />

                        <asp:BoundField DataField="CUSTOMER_PAN" HeaderText="Customer PAN" />

                        <asp:BoundField DataField="BILL_DATE" HeaderText="Bill Date" />

                        <asp:BoundField DataField="AMOUNT" HeaderText="Amount" />

                        <asp:BoundField DataField="DISCOUNT" HeaderText="Discount" />

                        <asp:BoundField DataField="TAXABLE_AMOUNT" HeaderText="Taxable Amount" />

                        <asp:BoundField DataField="TAX_AMOUNT" HeaderText="Tax Amount" />

                        <asp:BoundField DataField="TOTAL_AMOUNT" HeaderText="Total Amount" />

                        <asp:BoundField DataField="SYNC_WITH_IRD" HeaderText="Synced with IRD" />

                        <asp:BoundField DataField="IS_PRINTED" HeaderText="Printed" />

                        <asp:BoundField DataField="IS_BILL_ACTIVE" HeaderText="Bill Active" />

                        <asp:BoundField DataField="PRINTED_TIME" HeaderText="Printed Time" />

                        <asp:BoundField DataField="ENTERED_BY" HeaderText="Entered By" />

                        <asp:BoundField DataField="PRINTED_BY" HeaderText="Printed By" />

                        <asp:BoundField DataField="IS_REALTIME" HeaderText="Realtime" />

                        <asp:BoundField DataField="PAYMENT_METHOD" HeaderText="Payment Method" />
                    </Columns>
                </asp:GridView>

            </div>
        </div>
    </div>
</asp:Content>

