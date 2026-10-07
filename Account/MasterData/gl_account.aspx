<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="gl_account.aspx.cs" Inherits="Account_MasterData_gl_account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form-group-sm">
        <table class="gridtable" style="width: 1000px">
            <tr>
                <td>Account Head
               <br />
                    <asp:DropDownList ID="ddlAccountsHead" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAccountsHead_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>Account Group   <br />
                    <asp:DropDownList ID="ddlGLAccMaster" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGLAccMaster_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td>
                    Headings <br />
                    <asp:DropDownList ID="ddlHeadings" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHeadings_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>General Ledger Code   <br />
                    <asp:TextBox ID="txtGlCode" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                </td>

                <td>General Ledger Name  <br />
                    <asp:TextBox ID="txtGLname" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:Label ID="lblPK_id" runat="server" Width="150px" Text="" Visible="false"></asp:Label>
                </td>
                <td>Sub Ledger  <br />
                    <asp:DropDownList CssClass="form-control" ID="ddlSubLedger" runat="server">
                        <asp:ListItem Value="1">Available</asp:ListItem>
                        <asp:ListItem Value="0">Not Available</asp:ListItem>
                    </asp:DropDownList>

                </td>
                 </tr>
            <tr>                
                <td>Status   
                    <asp:DropDownList CssClass="form-control" ID="ddlStatus" runat="server">
                        <asp:ListItem Value="1">Available</asp:ListItem>
                        <asp:ListItem Value="0">Not Available</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <br />
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" /></td>
                <td></td>
            </tr>
        </table>
    </div>
    <table>
        <tr>
            <td>
                <asp:GridView ID="gridGlAccount" runat="server" AutoGenerateColumns="False" CssClass="gridtable"  Width="750px"
                     OnRowDataBound="gridGlAccount_RowDataBound" OnRowCommand="gridGlAccount_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="S.N">
                            <ItemTemplate>
                                <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPkid" runat="server" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="GL Code">
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("gL_CODE") %>' ID="lblGLCode" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GL Name">
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("GL_NAME") %>' ID="lblGlname" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sub Ledger">
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("SUB_LEDGER") %>' ID="lblSubLedger" runat="server" Visible="false" />
                                <asp:Label ID="lblSub" runat="server" Text=""></asp:Label>
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
                                <asp:Label ID="lbleditable" runat="server" Text='<%# Bind("EDITABLE") %>' Visible="false"></asp:Label>
                                <asp:ImageButton ID="imgEdit" CommandName="View" runat="server" ImageUrl="~/images/icons/edit.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>

