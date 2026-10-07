<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Asset_SubCategory.aspx.cs" Inherits="FixedAsset_MasterData_Asset_SubCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table>
        <tr>

            <td style="width: 200px;">Category
                <br />
                <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server"></asp:DropDownList>
            </td>
            <td>
                <br />&nbsp;&nbsp;
                <asp:Button ID="btnView" runat="server" CssClass="btn btn-primary" Text="View" Width="150px" OnClick="btnView_Click" /></td>
        </tr>
    </table>
    <br />

    <asp:GridView ID="gridSubCategory" runat="server" AutoGenerateColumns="False" CssClass="gridtable" Width="50%" OnRowCancelingEdit="gridSubCategory_RowCancelingEdit" OnRowEditing="gridSubCategory_RowEditing" OnRowUpdating="gridSubCategory_RowUpdating" OnRowDataBound="gridSubCategory_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="Sn">
                <HeaderTemplate>
                    SN.
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                    <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                </ItemTemplate>

            </asp:TemplateField>
            <asp:TemplateField HeaderText="Sub Category Name">
                <HeaderTemplate>
                    Sub Category Name<br />
                    <asp:TextBox ID="txtSubCategoryName" CssClass="form-control" runat="server"></asp:TextBox>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblSubCatName" runat="server" Text='<%# Bind("SUB_CATEGORY_NAME") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtSubCategoryNameE" CssClass="form-control" runat="server" Text='<%# Bind("SUB_CATEGORY_NAME") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status">
                <HeaderTemplate>
                    Status<br />
                    <asp:DropDownList CssClass="form-control" ID="ddlStatus" runat="server">
                        <asp:ListItem Text="Available" Value="1" />
                        <asp:ListItem Text="UnAvailable" Value="0" />
                    </asp:DropDownList>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                    <asp:Label ID="lblStatusShow" runat="server"></asp:Label>

                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label  ID="lblStatusE" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                    <asp:DropDownList CssClass="form-control" ID="ddlStatusE" runat="server">
                        <asp:ListItem Text="Available" Value="1" />
                        <asp:ListItem Text="UnAvailable" Value="0" />
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <br />
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:ImageButton ID="btnEdit" runat="server"  ImageUrl="~/images/icons/edit.png" CommandName="edit" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:ImageButton ID="btnEdit" runat="server"  CommandName="update" ImageUrl="~/images/icons/upload.png" ToolTip="Update" />
                    <asp:ImageButton ID="btnCancel" runat="server"  CommandName="cancel" ImageUrl="~/images/icons/cancel.png" ToolTip="Cancel" />
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

