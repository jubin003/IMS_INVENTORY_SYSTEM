<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="product_category.aspx.cs" Inherits="Administration_product_category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <asp:GridView ID="gridProductCategory" runat="server" AutoGenerateColumns="False"
            OnRowEditing="gridProductCategory_RowEditing" OnRowUpdating="gridProductCategory_RowUpdating"
            OnRowCancelingEdit="gridProductCategory_RowCancelingEdit" 
            AllowPaging="True" OnPageIndexChanging="gridProductCategory_PageIndexChanging"
            PageSize="20" EnableModelValidation="True" CssClass="gridtable"
            OnRowDataBound="gridProductCategory_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="SN.">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Group Code">
                    <EditItemTemplate>
                        <asp:Label ID="lblCategoryCodeU" runat="server" Text='<%# Bind("CATEGORY_CODE") %>'></asp:Label>
                        <asp:Label ID="lblPKIDE" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        Group Code<br />
                        <asp:TextBox ID="txtCategoryCodeH" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                      
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCategoryCode" runat="server" Text='<%# Bind("CATEGORY_CODE") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Group Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCategoryNameE" runat="server" CssClass="form-control" Text='<%# Bind("CATEGORY_NAME") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        Group Name<br />
                        <asp:TextBox ID="txtCategoryNameH" runat="server"  CssClass="form-control"></asp:TextBox>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCategoryName" runat="server" Text='<%# Bind("CATEGORY_NAME") %>'></asp:Label>
                    </ItemTemplate>
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
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="btnEdit" runat="server" CommandName="update" ImageUrl="~/images/icons/upload.png" ToolTip="Update" />
                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="cancel" ImageUrl="~/images/icons/cancel.gif" ToolTip="Cancel" />
                    </EditItemTemplate>
                    <HeaderTemplate>
                        <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add"  CssClass="btn btn-success" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="edit" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>
</asp:Content>

