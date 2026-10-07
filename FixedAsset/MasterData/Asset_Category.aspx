<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Asset_Category.aspx.cs" Inherits="FixedAsset_MasterData_Asset_Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <asp:GridView ID="gridProductCategory" runat="server" AutoGenerateColumns="False"
            OnRowEditing="gridProductCategory_RowEditing" OnRowUpdating="gridProductCategory_RowUpdating"
            OnRowCancelingEdit="gridProductCategory_RowCancelingEdit"
            AllowPaging="True"
            PageSize="20" EnableModelValidation="True" CssClass="gridtable"
            OnRowDataBound="gridProductCategory_RowDataBound" Width="600px">
            <Columns>
                <asp:TemplateField HeaderText="SN.">
                    <ItemTemplate>
                        <asp:Label ID="lblSn" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Code ">
                    <HeaderTemplate>
                        Code<br />
                        <asp:TextBox ID="txtPrefix" runat="server" Text="" Width="50px"></asp:TextBox>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPrefix" runat="server" Text='<%# Bind("PREFIX") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>                       
                        <asp:Label ID="lblPKIDE" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblGlCode" runat="server" Text='<%# Bind("GL_CODE") %>' Visible="false"></asp:Label>
                        <asp:TextBox ID="txtPrefix" runat="server" Text='<%# Bind("PREFIX") %>' Width="50px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Group Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCategoryNameE" runat="server" CssClass="form-control" Text='<%# Bind("CATEGORY_NAME") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        Category Name<br />
                        <asp:TextBox ID="txtCategoryNameH" runat="server" CssClass="form-control"></asp:TextBox>
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
                        <asp:DropDownList ID="ddlStatusH" runat="server" CssClass="form-control">
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
                        <asp:ImageButton ID="btnEdit" runat="server" CommandName="update" ImageUrl="~/images/icons/upload.png" ToolTip="Update" Width="20px" />
                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="cancel" ImageUrl="~/images/icons/cancel.png" ToolTip="Cancel" Width="20px" />
                    </EditItemTemplate>
                    <HeaderTemplate>
                        <br />
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

