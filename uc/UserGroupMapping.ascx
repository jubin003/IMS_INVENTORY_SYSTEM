<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserGroupMapping.ascx.cs"
    Inherits="uc_Administration_UserGroupMapping" %>
<h3>User Group Mapping:</h3>
<asp:Label ID="lbl" runat="server" Text=""></asp:Label>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <%--<asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        --%><asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCancelingEdit="GridView1_RowCancelingEdit"
            OnRowDeleting="GridView1_RowDeleting"
            OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating"
            OnRowDataBound="GridView1_RowDataBound"
            OnRowCreated="GridView1_RowCreated" CssClass="gridtable"
            AllowPaging="True" AllowSorting="True"
            ShowFooter="True"
            OnPageIndexChanging="GridView1_PageIndexChanging">
            <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next"
                PreviousPageText="Previous" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddUseridedit" runat="server"></asp:DropDownList>
                        <asp:Label ID="lblUsergroupid" runat="server" Text='<%# Eval("USERGROUPID") %>' Visible="false"></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUserid" runat="server" Text='<%# Eval("USERID") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <asp:DropDownList ID="ddUserid" runat="server"></asp:DropDownList>
                    </HeaderTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtlblUserid" runat="server" Width="100px"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddGroupidedit" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblGroupid" runat="server" Text='<%# Eval("GROUPID") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderTemplate>
                        <asp:DropDownList ID="ddGroupid" runat="server"></asp:DropDownList>
                    </HeaderTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtGroupid" runat="server" Width="100px"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:LinkButton ID="btnUpdate" runat="server" CommandName="update" Text="Update" ValidationGroup="Edit" />
                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="delete" Text="Delete" CausesValidation="false" />
                        <asp:LinkButton ID="btnCancel" runat="server" CommandName="cancel" Text="Cancel" CausesValidation="false" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="edit" Text="Edit" CausesValidation="false" />
                    </ItemTemplate>
                    <HeaderTemplate>
                        <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" />
                    </HeaderTemplate>
                    <FooterTemplate>
                        <asp:Button ID="btnFilter" runat="server" OnClick="btnFilter_Click"
                            Text="Filter" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </ContentTemplate>
</asp:UpdatePanel>
