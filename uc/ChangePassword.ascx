<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.ascx.cs" 
    Inherits="uc_ChangePassword" %>
<h4>
       
        Change User Password</h4>
    <table class="gridtable">
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMsg" runat="server" ></asp:Label>
            </td>
            
        </tr>     
        <tr>
            <td>
                Login Id
            </td>
            <td>
                <asp:Label ID="lblLoginid" runat="server"></asp:Label>
            </td>
        </tr>       
        <tr>
            <td>
              EmployeeId
            </td>
            <td>
                <asp:Label ID="lblEmployeeID" runat="server"></asp:Label>
            </td>
        </tr>
         <tr>
            <td>
                Employee Name
            </td>
            <td>
                <asp:Label ID="lblEmployeeName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                User Group
            </td>
            <td>
               <asp:Label ID="lblGroup" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Old Password
            </td>
            <td>
                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="*" ControlToValidate="txtOldPassword"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                New Password</td>
            <td>
                <asp:TextBox ID="txtPassword1" runat="server" TextMode="Password" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ErrorMessage="*" ControlToValidate="txtPassword1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Confirm Password
            </td>
            <td>
                <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ErrorMessage="*" ControlToValidate="txtPassword2"></asp:RequiredFieldValidator>
                <asp:CompareValidator
                    ID="CompareValidator1" runat="server" ErrorMessage="Password Missmatch" 
                    ControlToValidate="txtPassword2" ControlToCompare="txtPassword1"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td class="style2" colspan="2">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="88px" Height="26px" OnClick="btnSave_Click" />
                &nbsp;&nbsp;
               
            </td>
        </tr>
    </table>