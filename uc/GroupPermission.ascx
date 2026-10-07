<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GroupPermission.ascx.cs" Inherits="uc_Administration_GroupPermission" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<table>
    <tr>
        <td style="text-align: left;" >Group name
        </td>
        <td style="text-align: left" class="auto-style2">
            <asp:DropDownList ID="ddlGroup" runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="text-align: left; ">Module name
        </td>
        <td style="text-align: left">
            <asp:DropDownList ID="ddlModulename" runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ddlModulename_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: right">
            <asp:Button ID="btnAdd" runat="server" Text="Add Permission" Width="109px"
                OnClick="btnAdd_Click" />
        </td>
    </tr>

    <tr>
        <td colspan="2">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowDeleting="GridView1_RowDeleting"
                OnRowDataBound="GridView1_RowDataBound" CssClass="gridtable"
                AllowPaging="True" AllowSorting="True"
                ShowFooter="True"
                OnPageIndexChanging="GridView1_PageIndexChanging">
                <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next"
                    PreviousPageText="Previous" />
                <FooterStyle BackColor="#00cccc" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:TemplateField HeaderText="Module Name">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddModuleedit" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblModule" runat="server" Text='<%# Eval("MODULE_ID") %>'></asp:Label>
                            <asp:Label ID="lblPermid" runat="server" Visible="false" Text='<%# Eval("PERM_ID") %>'></asp:Label>
                            <asp:Label ID="lblModuleID" runat="server" Text="" Visible="False"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtModulename" runat="server" Width="100px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Submodule Name">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddSubmoduleedit" runat="server"></asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblSubmodule" runat="server" Text='<%# Eval("SUBMODULE_ID") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtSubmodulename" runat="server" Width="100px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Page Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPageName" runat="server" Text='<%# Eval("LinkName") %>'></asp:Label>
                            <asp:Label ID="lblPageid" runat="server" Visible="False" Text='<%# Eval("PageID") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtPagename" runat="server" Width="100px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Permission Assigned">
                        <ItemTemplate>
                            <asp:Label ID="lblPermission" runat="server" Text='<%# Eval("PERMISSION_SUB") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnFilter" runat="server" OnClick="btnFilter_Click"
                                Text="Filter" />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit"
                                CausesValidation="false" OnClick="btnEdit_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete"
                                OnClientClick="return confirm('Are you sure?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>

<div style="display: none;">
    <asp:Button ID="btnDummy" Text="Dummy" runat="server" />
</div>

<cc1:ModalPopupExtender ID="AddEdit_ModalPopupExtender" runat="server"
    BackgroundCssClass="modalBackground" CancelControlID="btnCancel"
    DynamicServicePath="" Enabled="True" PopupControlID="divAddEditPermission" PopupDragHandleControlID="divAddEditPermission"
    TargetControlID="btnDummy">
</cc1:ModalPopupExtender>

<div id="divAddEditPermission" style="box-shadow: 0px 2px 8px 0px #7795BD;">
    <table class="gridtable">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                        <table>
                            <tr>
                                <td>Group Name:</td>
                                <td>
                                    <asp:DropDownList ID="ddlGrouppop" runat="server" Width="250px"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Module Name:</td>
                                <td>
                                    <asp:DropDownList ID="ddlModule" runat="server" Width="250px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlModule_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Submodule Name:   
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSubmodule" runat="server" Width="250px"
                                        AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlSubmodule_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td>Permission</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gvPages" runat="server" AutoGenerateColumns="False"
                                        OnRowDataBound="gvPages_RowDataBound" AllowPaging="True"
                                        OnPageIndexChanging="gvPages_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="PageID" HeaderText="Page ID" />
                                            <asp:TemplateField HeaderText="Page Name">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPage_Name" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Read">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_Read" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Modify">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_Modify" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Add">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_Add" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Branch Permission">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_Delete" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="V. Check">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_aCreator" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="V. Pass">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_AApprover" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="V. Approve">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_AViewer" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td align="center" colspan="2" style="margin-left: 80px">
                            <asp:Button ID="btnSave" runat="server" Text="Save"
                                Width="62px" OnClick="btnSave_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel"
                    Width="68px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
