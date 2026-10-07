<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="onelakhplustransaction.aspx.cs" Inherits="Reports_IRD_onelakhplustransaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row" id="divBranch" runat="server">
            <div class="col-md-4">
                Branch<br />
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Month<br />
                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control">
                    <asp:ListItem Value="1">Baishak</asp:ListItem>
                    <asp:ListItem Value="2">Jestha</asp:ListItem>
                    <asp:ListItem Value="3">Asar</asp:ListItem>
                    <asp:ListItem Value="4">Shrawan</asp:ListItem>
                    <asp:ListItem Value="5">Bhadra</asp:ListItem>
                    <asp:ListItem Value="6">Aswin</asp:ListItem>
                    <asp:ListItem Value="7">Kartik</asp:ListItem>
                    <asp:ListItem Value="8">Manshir</asp:ListItem>
                    <asp:ListItem Value="9">Poush</asp:ListItem>
                    <asp:ListItem Value="10">Magh</asp:ListItem>
                    <asp:ListItem Value="11">Fagun</asp:ListItem>
                    <asp:ListItem Value="12">Chaitra</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-1">
                Year<br />
                <asp:TextBox ID="txtYear" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-primary" OnClick="btnShow_Click" />
            </div>
            <div class="col-md-1">
                <asp:ImageButton ID="ImageButton1" runat="server" OnClick="ImageButton1_Click" Width="50px" ImageUrl="~/images/icons/excel.png" />
            </div>
        </div>
        <div id="hide" runat="server">
            <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="false" CssClass="gridtable" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="PAN">
                        <ItemTemplate>
                            <asp:Label ID="lblPAN" runat="server" Text='<%# Bind("COSTOMER_PAN_VAT") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name of Tax Payer">
                        <ItemTemplate>
                            <asp:Label ID="lblNameofTaxPayer" runat="server" Text='<%# Bind("CUSTOMER_NAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Trade Name Type">
                        <ItemTemplate>
                            <asp:Label ID="lblTradeNameType" runat="server" Text='<%# Bind("TradeNameType") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Purchase/Sale">
                        <ItemTemplate>
                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("PURCHASE_SALES") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Taxable Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblTaxableAmount" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Exempted Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblExemptedAmount" runat="server" Text='<%# Bind("ExemptedAmount") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

