<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Product_Unit.aspx.cs" Inherits="Administration_Product_Unit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="container-fluid">
        <asp:GridView ID="grid" runat="server" AutoGenerateColumns="False"
            OnRowEditing="grid_RowEditing" OnRowUpdating="grid_RowUpdating"
            OnRowCancelingEdit="grid_RowCancelingEdit" CssClass="gridtable">
            <Columns>
                <asp:TemplateField HeaderText="SN.">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Unit Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtUnitNameE" runat="server"  CssClass="form-control" Text='<%# Bind("UNIT_NAME") %>'></asp:TextBox>
                        <asp:Label ID="lblPKIDE" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        Unit Name<br />
                        <asp:TextBox ID="txtUnitNameH" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:Label ID="lblPKIDH" runat="server" Text='<%# Bind("PK_ID") %>'></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUnitName" runat="server" Text='<%# Bind("UNIT_NAME") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>             
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="btnEdit" runat="server" CommandName="update" ImageUrl="~/images/icons/upload.png" ToolTip="Update" />
                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="cancel" ImageUrl="~/images/icons/cancel.gif" ToolTip="Cancel" />
                    </EditItemTemplate>
                    <HeaderTemplate>
                        <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" CssClass="btn btn-success" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="edit" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

