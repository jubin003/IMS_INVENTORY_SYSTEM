<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="gl_sub_account.aspx.cs"
    Inherits="Account_MasterData_gl_sub_account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table class="gridtable" style="width: 1000px;">
        <tr>
            <td>GL Account Head
            <br />
                <asp:DropDownList ID="ddlAccountsHead" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAccountsHead_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>GL Account Master  
                <br />
                <asp:DropDownList ID="ddlGLAccMaster" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGLAccMaster_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td>General Ledger  
                <br />
                <asp:DropDownList ID="ddlGl" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGl_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td></td>
        </tr>
        <tr>
            <td>Sub GL Code  
                <br />
                <asp:TextBox ID="txtSubGlCode" CssClass="form-control" runat="server" AutoPostBack="true" OnTextChanged="txtSubGlCode_TextChanged"></asp:TextBox>
            </td>
            <td>Sub GL Name
                <br />
                <asp:TextBox ID="txtSubGlName" CssClass="form-control" runat="server"></asp:TextBox>
                <asp:Label ID="lblPK_id" runat="server" Text="" Visible="false"></asp:Label>
            </td>
            <td>Status  
                <br />
                <asp:DropDownList CssClass="form-control" ID="ddlStatus" runat="server">
                    <asp:ListItem Value="1">Available</asp:ListItem>
                    <asp:ListItem Value="0">Not Available</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <br />
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:GridView ID="gridSubGl" runat="server" AutoGenerateColumns="False" CssClass="gridtable"  Width="750px"
                    OnRowDataBound="gridSubGl_RowDataBound" OnRowCommand="gridSubGl_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="S.N">
                            <ItemTemplate>
                                <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPkid" runat="server" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GL Code">
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("GL_CODE") %>' ID="lblGlCode" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sub GL Code">
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("SUB_GL_CODE") %>' ID="lblSubGlCode" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sub GL Name">
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("SUB_GL_NAME") %>' ID="lblSubGlname" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("STATUS") %>' ID="lblStatus" runat="server" Visible="false" />
                                <asp:Label ID="lblStat" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" CommandName="View" runat="server" ImageUrl="~/images/icons/edit.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>

