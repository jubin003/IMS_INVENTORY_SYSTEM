<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Groups.ascx.cs"  Inherits="uc_Groups" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div runat="server" id="divGroup" visible="true">
            <asp:GridView ID="grdGroups" runat="server" AutoGenerateColumns="False" AllowSorting="True" Visible="false"
                AllowPaging="True" CssClass="gridtable" OnRowCancelingEdit="grdGroups_RowCancelingEdit"
                OnRowDataBound="grdGroups_RowDataBound" OnRowDeleting="grdGroups_RowDeleting" OnRowCommand="grdGroups_RowCommand"
                OnRowEditing="grdGroups_RowEditing" OnRowUpdating="grdGroups_RowUpdating" OnPageIndexChanging="grdGroups_PageIndexChanging1"
                OnSorting="grdGroups_Sorting1" ShowFooter="True">
                <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Previous" />
                <FooterStyle BackColor="#00cccc" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:TemplateField HeaderText="Group ID">
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txtGroupid" runat="server" ReadOnly="true" Text='<%# Bind("GROUPID") %>'></asp:TextBox> --%>
                            <asp:Label ID="lblGroupid" runat="server" Text='<%# Bind("GROUPID") %>'></asp:Label>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            Group ID<br />
                            <asp:TextBox ID="txtGroupid" runat="server"></asp:TextBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGroupid" runat="server" Text='<%# Bind("GROUPID") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtGroupid" runat="server" Width="100px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Group Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGroupname" runat="server" Text='<%# Bind("GROUPNAME") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            Group Name<br />
                            <asp:TextBox ID="txtGroupname" runat="server"></asp:TextBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGroupname" runat="server" Text='<%# Bind("GROUPNAME") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtGroupname" runat="server" Width="100px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Group Precedence">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGroupprecedence" runat="server" Text='<%# Bind("GROUPPRECEDENCE") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            Group Precedence<br />
                            <asp:TextBox ID="txtGroupprecedence" runat="server"></asp:TextBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGroupprecedence" runat="server" Text='<%# Bind("GROUPPRECEDENCE") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtGroupprecedence" runat="server" Width="100px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Group Details">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGroupdetails" runat="server" Text='<%# Bind("GROUPDETAILS") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            Group Details<br />
                            <asp:TextBox ID="txtGroupdetails" runat="server"></asp:TextBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGroupdetails" runat="server" Text='<%# Bind("GROUPDETAILS") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtGroupdetails" runat="server" Width="100px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <EditItemTemplate>
                            &nbsp;<asp:ImageButton ID="btnEdit" runat="server" CommandName="update" ImageUrl="~/images/icons/upload.png" ToolTip="Update" />
                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="delete" ImageUrl="~/images/icons/deletes.png" ToolTip="Delete" />
                            <asp:ImageButton ID="btnCancel" runat="server" CommandName="cancel" ImageUrl="~/images/icons/cancel.png" ToolTip="Cancel" />
                            &nbsp;
                        </EditItemTemplate>
                        <HeaderTemplate>
                            <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/images/icons/Add.jpg" ToolTip="Add" OnClick="btnAdd_Click1" />
                        </HeaderTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnFilter" runat="server" OnClick="btnFilter_Click" Text="Filter" Width="50px" />
                        </FooterTemplate>
                        <ItemTemplate>
                            &nbsp;<asp:ImageButton ID="btnEdit0" runat="server" CommandName="edit" ImageUrl="~/images/icons/edit.png" ToolTip="[Edit]" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>

                        <ItemTemplate>
                            &nbsp;<asp:ImageButton ID="btnPdf" runat="server" CommandName="pdf" Width="25px" Height="25px" ImageUrl="~/images/icons/pdf_image.jpg" ToolTip="Help" OnClick="btnPdf_Click" />
                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
