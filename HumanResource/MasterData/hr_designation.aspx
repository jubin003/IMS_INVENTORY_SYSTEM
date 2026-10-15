<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="hr_designation.aspx.cs" Inherits="human_resource_Master_data_hr_designation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <b>Department:</b><br />
                        <asp:DropDownList ID="ddlDept" Width="200px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <br />
                        <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 50%">
                        <asp:GridView ID="gridDesignation" runat="server" AutoGenerateColumns="False"
                            EnableModelValidation="True" Width="100%" CssClass="gridtable"
                            OnRowEditing="gridDesignation_RowEditing" OnRowCancelingEdit="gridDesignation_RowCancelingEdit"
                            OnRowUpdating="gridDesignation_RowUpdating"
                            MaintainScrollPositionOnPostback="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSerialNum" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                        <asp:Label ID="lblPKID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <br />
                                        Designation Name 
                                      
                            <asp:TextBox ID="txtDesignationH" CssClass="form-control" runat="server" />
                                        <asp:RequiredFieldValidator ErrorMessage="*" ValidationGroup="AddBtn" ControlToValidate="txtDesignationH" runat="server" />
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDesignationE" CssClass="form-control" runat="server" Text='<%# Bind("DESIGNATION_NAME") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("DESIGNATION_NAME") %>' ID="lblDesignationName" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="80px">
                                    <HeaderTemplate>
                                        Level 
                                <asp:TextBox ID="txtLevelH" CssClass="form-control" runat="server" />
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtLevelE" CssClass="form-control" runat="server" Text='<%# Bind("DESIGNATION_LEVEL") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("DESIGNATION_LEVEL") %>' ID="lblLevel" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <br />
                                        <asp:Button Text="Add" ID="btnAdd" CssClass="btn btn-primary" ValidationGroup="AddBtn" runat="server" OnClick="btnAdd_Click" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary" Text="Edit" CommandName="Edit" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button ID="btnUpdate" CssClass="btn btn-primary" runat="server" Text="Update" CommandName="Update" />
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" CommandName="Cancel" />
                                    </EditItemTemplate>

                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

