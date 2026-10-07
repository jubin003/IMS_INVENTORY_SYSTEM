<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Product_SubCategory.aspx.cs" Inherits="Administration_Product_SubCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-1">
                Group
            </div>
            <div class="col-md-2">
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>            
            <div class="col-md-1">
                <asp:Button ID="btnLoad" runat="server" Text="Load" CssClass="btn btn-sm btn-success" OnClick="btnLoad_Click" />
                <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
            </div>          
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:GridView ID="grid" runat="server" AutoGenerateColumns="False"
                    OnRowEditing="grid_RowEditing" OnRowUpdating="grid_RowUpdating"
                    OnRowCancelingEdit="grid_RowCancelingEdit" EnableModelValidation="True"
                    CssClass="gridtable" OnRowDataBound="grid_RowDataBound"
                    Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="SN.">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Category" Visible="false">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlCategoryE" runat="server" Enabled="false"  CssClass="form-control"></asp:DropDownList>
                                 <asp:Label ID="lblCategoryE" runat="server" Text='<%# Bind("CATEGORY_ID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblPKIDE" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Category<br />
                                <asp:DropDownList ID="ddlCategoryH" runat="server"  CssClass="form-control"></asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("CATEGORY_ID") %>' Visible="false"></asp:Label>
                                 <asp:Label ID="lblCategoryName" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblPKID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sub Category Code">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSubCategoryCodeE" runat="server" ReadOnly="true"  CssClass="form-control" Text='<%# Bind("SUB_CATEGORY_CODE") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Category Code<br />
                                <asp:TextBox ID="txtSubCategoryCodeH" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSubCategoryCode" runat="server" Text='<%# Bind("SUB_CATEGORY_CODE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sub Category">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSubCategoryE" runat="server" Text='<%# Bind("SUB_CATEGORY_NAME") %>'  CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Category<br />
                                <asp:TextBox ID="txtSubCategoryH" runat="server"  CssClass="form-control"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSubCategory" runat="server" Text='<%# Bind("SUB_CATEGORY_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <EditItemTemplate>
                                <asp:Label ID="lblStatusE" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlStatusE" runat="server"  CssClass="form-control">
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
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="update" ImageUrl="~/images/icons/upload.png" ToolTip="Update" />
                                <asp:ImageButton ID="btnCancel" runat="server" CommandName="cancel" ImageUrl="~/images/icons/cancel.gif" ToolTip="Cancel" />
                            </EditItemTemplate>
                            <HeaderTemplate>
                                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" CssClass="btn btn-success"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

