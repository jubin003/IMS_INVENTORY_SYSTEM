<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Bank.aspx.cs" Inherits="Account_MasterData_Bank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table  class="gridtable" >
        <tr>
            <td>Bank Name:</td>
            <td>
                <asp:TextBox ID="txtBank" runat="server" CssClass="form-control"></asp:TextBox></td>
            <td>Status:</td>
            <td>

                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                    <asp:ListItem Value="1">Available</asp:ListItem>
                    <asp:ListItem Value="0">Not Available</asp:ListItem>
                </asp:DropDownList></td>
            <td>
              
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                <asp:Label ID="lblPK_id" runat="server" Text="" Visible="false"></asp:Label>

            </td>
        </tr>  
        <tr>
            <td colspan="5">
                <asp:GridView ID="gridBank" runat="server" AutoGenerateColumns="False" CssClass="gridtable" 
                    OnRowCommand="gridBank_RowCommand" OnRowDataBound="gridBank_RowDataBound" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="S.N">
                            <ItemTemplate>
                                <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPkid" runat="server" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GL Name">
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("BANK_NAME") %>' ID="lblGlname" runat="server" />
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

