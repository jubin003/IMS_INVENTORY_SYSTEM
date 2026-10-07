<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="product_type.aspx.cs" Inherits="Administration_product_type" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="container-fluid">
        <asp:GridView ID="grid" runat="server" AutoGenerateColumns="False"
            OnRowEditing="grid_RowEditing" OnRowUpdating="grid_RowUpdating"
            OnRowCancelingEdit="grid_RowCancelingEdit" CssClass="gridtable"
            OnRowDataBound="grid_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="SN.">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
               
               
                 <asp:TemplateField HeaderText="Type Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNameE" runat="server"  CssClass="form-control" Text='<%# Bind("PRODUCT_TYPE_NAME") %>'></asp:TextBox>
                        <asp:Label ID="lblPKIDE" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        Type Name <br />
                        <asp:TextBox ID="txtNameH" runat="server"  CssClass="form-control"></asp:TextBox>
                        <asp:Label ID="lblPKIDH" runat="server" Text='<%# Bind("PK_ID") %>'></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("PRODUCT_TYPE_NAME") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <EditItemTemplate>
                        <asp:Label ID="lblStatusE" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                        <asp:DropDownList ID="ddlStatusE" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        Status<br />
                        <asp:DropDownList ID="ddlStatusH" runat="server"  CssClass="form-control">
                            <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblStatusShow" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Show in Purchase">
                    <EditItemTemplate>
                        <asp:Label ID="lblPuStatusE" runat="server" Text='<%# Bind("SHOW_IN_PURCHASE") %>' Visible="false"></asp:Label>
                        <asp:DropDownList ID="ddlPuStatusE" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        Show in Purchase<br />
                        <asp:DropDownList ID="ddlPuStatusH" runat="server"  CssClass="form-control">
                            <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPuStatus" runat="server" Text='<%# Bind("SHOW_IN_PURCHASE") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblPuStatusShow" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField><asp:TemplateField HeaderText="Show in Sales">
                    <EditItemTemplate>
                        <asp:Label ID="lblSaStatusE" runat="server" Text='<%# Bind("SHOW_IN_SALES") %>' Visible="false"></asp:Label>
                        <asp:DropDownList ID="ddlSaStatusE" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        Show in Sales<br />
                        <asp:DropDownList ID="ddlSaStatusH" runat="server"  CssClass="form-control">
                            <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblSaStatus" runat="server" Text='<%# Bind("SHOW_IN_SALES") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblSaStatusShow" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
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

