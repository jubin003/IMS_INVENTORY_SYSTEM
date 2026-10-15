<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Leave_Approver.aspx.cs" Inherits="HR_Attendance_MasterData_Leave_Approver" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updGrdWrkSchedule" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gridApprove" runat="server" CssClass="gridtable" Width="100%" AutoGenerateColumns="False" OnRowDataBound="gridApprove_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="SN.">
                        <ItemTemplate>
                            <asp:Label ID="lblSN" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                            <asp:Label ID="lblPKIDE" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Department">
                        <HeaderTemplate>
                            Department                   
                            <br />
                            <asp:DropDownList ID="ddlDepartment" AutoPostBack="true" runat="server" Width="200px" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"></asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDepartment" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Division">
                        <HeaderTemplate>
                            Division                   
                            <br />
                            <asp:DropDownList ID="ddlDivision" AutoPostBack="true" runat="server" Width="200px" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"></asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDivision" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit">
                        <HeaderTemplate>
                            Unit                   
                            <br />
                            <asp:DropDownList ID="ddlUnit" AutoPostBack="true" runat="server" Width="200px" OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged"></asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUnit" runat="server"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Employee Name">
                        <HeaderTemplate>
                            Employee Name                   
                            <br />
                            <asp:DropDownList ID="ddlEmpName" runat="server" Width="200px"></asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblEmployeeID" runat="server" Text='<%# Bind("EMP_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblShowName" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Permission">
                        <HeaderTemplate>
                            Permission<br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkRecommendH" runat="server" />Recommend
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkApproveH" runat="server" />Approved
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkRecommend" runat="server" />Recommend
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkApprove" runat="server" />Approved
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <HeaderTemplate>
                            <br />
                            <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Button ID="btnUpdate" runat="server" Text="Save" OnClick="btnUpdate_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save All" OnClick="btnSave_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

