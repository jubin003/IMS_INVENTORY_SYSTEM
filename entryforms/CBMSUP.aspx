<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CBMSUP.aspx.cs" Inherits="entryforms_CBMSUP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <table>
            <tr>
                <td>Username</td>
                <td>Password</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtCBMSUsername" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtCBMSPassword" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
