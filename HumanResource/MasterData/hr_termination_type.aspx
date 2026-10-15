<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="hr_termination_type.aspx.cs" Inherits="HumanResource_MasterData_hr_termination_type" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:GridView ID="gridTerType" CssClass="gridtable" runat="server" AutoGenerateColumns="False" OnRowCancelingEdit="gridTerType_RowCancelingEdit" OnRowDataBound="gridTerType_RowDataBound" OnRowEditing="gridTerType_RowEditing" OnRowUpdating="gridTerType_RowUpdating">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSn" Text="<%#Container.DataItemIndex+1 %>" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Termination Type:
                    <br />
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtTerminationTypeH" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTerminationType" Text='<%#Bind("Termination_TYPE") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" CssClass="form-control" ID="txtTerminationTypeE" Text='<%#Bind("Termination_TYPE") %>' />
                                        <asp:Label ID="lblPkid" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Status:<br />
                                        <asp:DropDownList CssClass="form-control" ID="ddlStatusH" runat="server">
                                            <asp:ListItem Text="Available" Value="1" />
                                            <asp:ListItem Text="Unavailable" Value="0" />
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList CssClass="form-control" ID="ddlStatusE" runat="server">
                                            <asp:ListItem Text="Available" Value="1" />
                                            <asp:ListItem Text="Unavailable" Value="0" />
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" Visible="false" Text='<%#Bind("STATUS") %>' runat="server" />
                                        <asp:Label ID="lblShowStatus" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
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

