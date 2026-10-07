<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UnMappedDakhila.aspx.cs" Inherits="Mapping_sales_UnMappedDakhila" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:GridView ID="grdUnMappedDakhila" runat="server" AutoGenerateColumns="False" CssClass="gridtable" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="Sno">
                <ItemTemplate>
                    <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Dakhila No">
                <ItemTemplate>
                    <asp:LinkButton ID="lblDakhilaNumber" runat="server" Text='<%# Bind("DAKHILA_NUMBER") %>' OnClick="lblDakhilaNo_Click"></asp:LinkButton>
                    <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Dakhila Date">
                <ItemTemplate>
                    <asp:Label ID="lblDakhilaDay" runat="server" Text='<%# Bind("DAKHILA_DATE") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Dakhila FY.">
                <ItemTemplate>
                    <asp:Label ID="lblDakhilaFY" runat="server" Text='<%# Bind("DAKHILA_FY") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Supplier ">
                <ItemTemplate>
                    <asp:Label ID="lblSupplierName" runat="server" Text='<%# Bind("SUPPLIER_NAME") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Invoice Amount">
                <ItemTemplate>
                    <asp:Label ID="lblInoiveAmount" runat="server" Text='<%# Bind("INVOICE_AMOUNT") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Right" />
            </asp:TemplateField>


        </Columns>

    </asp:GridView>
</asp:Content>

