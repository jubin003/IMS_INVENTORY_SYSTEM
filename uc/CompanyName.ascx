<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompanyName.ascx.cs" Inherits="uc_CompanyName" %>
<table style="width: 100%; text-align: center;">
    <tr>
        <td colspan="4">
            <asp:Label ID="lblNameCompany" runat="server" Text="" Font-Size="24px" Font-Bold="true"></asp:Label></td>
    </tr>
    <tr>
        <td colspan="4">
            <asp:Label ID="lblAddress" runat="server" Text="" Font-Size="14px"></asp:Label><br />
            <div id="divWebsite" runat="server">
                  <asp:Label ID="lblWebsite" runat="server" Text="" Font-Size="14px"></asp:Label>
                     <asp:Label ID="lblEmail" runat="server" Text="" Font-Size="14px"></asp:Label><br />
                </div>
               <asp:Label ID="lblContactDetail" runat="server" Text="" Font-Size="14px"></asp:Label>
        </td>
    </tr>
</table>
