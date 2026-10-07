<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginUsers.ascx.cs" Inherits="uc_LoginUsers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<style type="text/css">
    .style1 {
        text-align: right;
    }
</style>

<div>
    
    <table>
        <tr>
            <td class="style1">
                <asp:Button ID="btnAddNew" runat="server" OnClick="btnAddNew_Click" Text="Add New" />
            </td>
        </tr>

        <tr valign="top">
            <td valign="top">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridtable"
                    DataKeyNames="LOGINID" OnRowDeleting="GridView1_RowDeleting" AllowPaging="True"
                    OnPageIndexChanging="GridView1_PageIndexChanging" AllowSorting="True"
                    ShowFooter="True" OnRowDataBound="GridView1_RowDataBound">
                    <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next"
                        PreviousPageText="Previous" />
                    <FooterStyle BackColor="#00cccc" Font-Bold="True" ForeColor="White" />

                    <Columns>
                        <asp:TemplateField HeaderText="Login Id">
                            <ItemTemplate>
                                <asp:Label ID="lblLoginid" runat="server" Text='<%# Bind("LOGINID") %>' Visible="false" />
                                <asp:Label ID="lblLoginEnc" runat="server" Text='<%# Bind("PASSWORDQUESTION") %>' Visible="false" />
                                 <asp:Label ID="lblLoginDec" runat="server"  />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtLoginid" runat="server" Width="100px"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Full Name">
                            <ItemTemplate>
                                <asp:Label ID="lblFulldetails" runat="server" Text='<%# Bind("FULLDETAILS") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtFulldetails" runat="server" Width="100px"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Location">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeid" runat="server" Text='<%# Bind("EMPLOYEEID") %>'
                                    Visible="False" />
                                <asp:Label ID="lblOfficeName" runat="server"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtEmployeeid" runat="server" Width="100px"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Group ID">
                            <ItemTemplate>
                                <asp:Label ID="lblGroupid" runat="server" Text='<%# Bind("GROUPID") %>'
                                    Visible="False" />
                                <asp:Label ID="lblGroupName" runat="server"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtGroupid" runat="server" Width="100px"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("EMAIL") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtEmail" runat="server" Width="100px"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Branch">
                            <ItemTemplate>
                                <asp:Label ID="lblBranchPK_ID" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblBranch" runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtBranch" runat="server" Width="100px"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Blocked/Unblocked">
                            <ItemTemplate>
                                <asp:Label ID="lblbu" runat="server" Text='<%# Bind("ACCESSBLOCKED") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtbu" runat="server" Width="100px"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png" ToolTip="Edit" OnClick="btnEdit_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" Visible="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/images/icons/deletes.png"
                                    ToolTip="Delete" CommandName="Delete" OnClientClick="return confirm('Are you sure?');" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnFilter" runat="server" OnClick="btnFilter_Click"
                                    Text="Filter" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
  
</div>
<div style="display: none;">
    <asp:Button ID="btnPopup" runat="server" Text="popup" />
    <cc1:ModalPopupExtender ID="btnPopup_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel" DynamicServicePath="" Enabled="True" PopupControlID="divEdit"
        TargetControlID="btnPopup">
    </cc1:ModalPopupExtender>
</div>
<div id="divEdit" style="display: block; box-shadow: 0px 2px 8px 0px #7795BD; " >
    
    <table class="gridtable">
        <tr style="background-color:#00cccc;">
            <td colspan="2"><b>Registration of new User</b>
            
            </td>
        </tr>
        <tr>
            <td>Login&nbsp; ID
            </td>
            <td>
                <asp:TextBox ID="txtLoginid" runat="server" Width="250px"></asp:TextBox>
            </td>
        </tr>
       
        <tr>
            <td>Name<asp:Label ID="empId" runat="server" Visible="False"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtFulldetails" runat="server" Width="250px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>User Group ID
            </td>
            <td>
                <asp:DropDownList ID="ddlGroupid" runat="server" Width="250px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Password
            </td>
            <td style="margin-left: 40px">
                <asp:TextBox ID="txtPassword" runat="server" Width="250px" TextMode="Password"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>Email
            </td>
            <td style="margin-left: 40px">
                <asp:TextBox ID="txtEmail" runat="server" Width="250px"></asp:TextBox>
            </td>
        </tr>
         <tr runat="server" id="trBranch" visible="false">
            <td>
                Branch:
            </td>
            <td>
                <asp:DropDownList ID="ddlBranch" runat="server" Height="28px" Width="250px"></asp:DropDownList>
            </td>
        </tr>
        
        <tr>
            <td class="style2">Access Blocked
            </td>
            <td class="style2">
                <asp:RadioButton ID="rbtnBlocked" runat="server" Text="Block" GroupName="rbtnAccess" />
                <asp:RadioButton ID="rbtnUnblocked" runat="server" Text="Unblock"
                    GroupName="rbtnAccess" Checked="True" />
            </td>
        </tr>
        <tr>
            <td class="style2" colspan="2">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="88px" Height="26px" OnClick="btnSave_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <%--<asp:Button ID="btnCancel" runat="server" Text="Delete" 
                Width="74px" Height="26px" />--%>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
            </td>
        </tr>
    </table>
</div>
