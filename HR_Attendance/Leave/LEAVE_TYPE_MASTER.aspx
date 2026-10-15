<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LEAVE_TYPE_MASTER.aspx.cs" Inherits="HR_Attendance_Leave_LEAVE_TYPE_MASTER" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updGrdWrkSchedule" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container">
                <asp:GridView ID="gridLeaveType" Width="600px" CssClass="gridtable" OnRowEditing="gridLeaveType_RowEditing"
                    OnRowCancelingEdit="gridLeaveType_RowCancelingEdit" OnRowUpdating="gridLeaveType_RowUpdating"
                    OnRowDataBound="gridLeaveType_RowDataBound"
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
                                <asp:TextBox ID="txtLeaveNameH" Width="350px" placeholder="..Leave Name" autocomplete="off" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLeaveName" Width="350px" runat="server" Text='<%# Bind("LEAVE_TYPE") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLeaveNameE" Width="350px" placeholder="..Leave Name" autocomplete="off" runat="server" />
                                <asp:Label Text='<%# Bind("LEAVE_TYPE") %>' Visible="false" ID="lblLeaveNameE" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:TextBox ID="txtDaysH" Width="80px"  autocomplete="off" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDays" Width="80px" runat="server" Text='<%# Bind("DAYS") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDaysE" Width="80px"  autocomplete="off" runat="server" />
                                <asp:Label Text='<%# Bind("DAYS") %>' Visible="false" ID="lblDaysE" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:DropDownList ID="ddlCommonH" Width="120px" runat="server">
                                    <asp:ListItem Value="1" Text="Common" />
                                    <asp:ListItem Value="0" Text="Special" />
                                </asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCommon" runat="server" Text='<%# Bind("Common") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlCommonE" runat="server">
                                    <asp:ListItem Value="1" Text="Common" />
                                    <asp:ListItem Value="0" Text="Special" />
                                </asp:DropDownList>
                                <asp:Label Text='<%# Bind("Common") %>'  ID="lblCommonE" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:DropDownList ID="ddlStatusH" Width="120px" runat="server">
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
                                <asp:Label Text='<%# Bind("STATUS") %>'  ID="lblStatusE" runat="server" />
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

