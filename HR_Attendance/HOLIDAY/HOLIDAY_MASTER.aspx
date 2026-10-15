<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HOLIDAY_MASTER.aspx.cs" Inherits="HR_Attendance_HOLIDAY_HOLIDAY_MASTER" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container">
                <asp:GridView ID="gridHolidayMaster" Width="600px" CssClass="gridtable"
                    OnRowEditing="gridHolidayMaster_RowEditing"
                    OnRowCancelingEdit="gridHolidayMaster_RowCancelingEdit"
                    OnRowUpdating="gridHolidayMaster_RowUpdating"
                    OnRowDataBound="gridHolidayMaster_RowDataBound"
                    AutoGenerateColumns="false" runat="server">
                    <HeaderStyle BackColor="Silver" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label Text="SN" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPK" Visible="false" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:TextBox ID="txtHolidayNameH" Width="350px" placeholder="..Holiday Name" autocomplete="off" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblHolidayName" Width="350px" runat="server" Text='<%# Bind("HOLIDAY_NAME") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtHolidayNameE" Width="350px" placeholder="..Holiday Name" autocomplete="off" runat="server" />
                                <asp:Label Text='<%# Bind("HOLIDAY_NAME") %>' Visible="false" ID="lblHolidayNameE" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:DropDownList ID="ddlStatusH" runat="server">
                                    <asp:ListItem Value="1" Text="Available" />
                                    <asp:ListItem Value="0" Text="Unavailable" />
                                </asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlStatusE" runat="server">
                                    <asp:ListItem Value="1" Text="Available" />
                                    <asp:ListItem Value="0" Text="Unavailable" />
                                </asp:DropDownList>
                                <asp:Label Text='<%# Bind("STATUS") %>' Visible="false" ID="lblStatusE" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Button Text="Add" ID="btnAdd" OnClick="btnAdd_Click" CssClass="btn-default" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="Edit" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="imgbtnUpdate" runat="server" ImageUrl="~/images/icons/upload.png" CommandName="Update" />
                                <asp:ImageButton ID="imgbtnCancel" runat="server" ImageUrl="~/images/icons/delete.gif" CommandName="Cancel" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

