<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Supplier_Type.aspx.cs" 
    Inherits="Administration_Supplier_Type" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <table>
            <tr>
                <asp:GridView ID="gridSupplierType" runat="server" AutoGenerateColumns="False" CssClass="gridtable" OnRowDataBound="gridSupplierType_RowDataBound"
                    OnRowCancelingEdit="gridSupplierType_RowCancelingEdit" OnRowEditing="gridSupplierType_RowEditing"
                    OnRowUpdating="gridSupplierType_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="S.N">
                            <ItemTemplate>
                                <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Supplier Type">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSupplierTypeE" runat="server" Text='<%# Bind("SUPPLIERS_TYPE") %>'></asp:TextBox>
                                <asp:Label ID="lblPKIDU" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Supplier Type<br />
                                <asp:TextBox ID="txtSupplierTypeH" runat="server"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSupplierType" runat="server" Text='<%# Bind("SUPPLIERS_TYPE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Prefix">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPrefixE" runat="server" Text='<%# Bind("PREFIX") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Prefix<br />
                                <asp:TextBox ID="txtPrefixH" runat="server"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPrefix" runat="server" Text='<%# Bind("PREFIX") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Status">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlStatusE" runat="server" Text='<%# Bind("STATUS") %>'>
                                    <asp:ListItem Value="Select">Select</asp:ListItem>
                                    <asp:ListItem Value="1">Enabled</asp:ListItem>
                                    <asp:ListItem Value="0">Disabled</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Status<br />
                                <asp:DropDownList ID="ddlStatusH" runat="server">
                                    <asp:ListItem Value="Select">Select</asp:ListItem>
                                    <asp:ListItem Value="1">Enabled</asp:ListItem>
                                    <asp:ListItem Value="0">Disabled</asp:ListItem>
                                </asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblShowStatus" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:ImageButton ID="imgUpdate" runat="server" CommandName="Update" ImageUrl="~/images/icons/upload.png" />
                                <asp:ImageButton ID="imgCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/icons/delete.gif" />
                            </EditItemTemplate>
                            <HeaderTemplate>
                                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" Style="height: 29px" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CommandName="Edit" ImageUrl="~/images/icons/edit.png" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>
            </tr>
        </table>
    </div>
</asp:Content>

