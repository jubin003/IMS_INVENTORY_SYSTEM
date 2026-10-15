<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="set_duty_hour.aspx.cs" Inherits="HR_Attendance_Duty_Hour_set_duty_hour" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updGrdWrkSchedule" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table id="EmpSearch" class="gridtable" runat="server">

                <tr id="view" runat="server">
                    <td>Department
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDept" runat="server" AutoPostBack="true" Width="200" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>Division
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDiv" runat="server" Width="200">
                        </asp:DropDownList>
                    </td>
                    <td>Status
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="200">
                            <asp:ListItem Selected="True" Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="0">InActive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="bttnSearch" runat="server" Text="Search" OnClick="bttnSearch_Click" />
                    </td>
                </tr>
            </table>

            <div runat="server" visible="false" id="divGrid">
                <asp:GridView ID="gridDutyHr" OnRowCommand="gridDutyHr_RowCommand" CssClass="gridtable" AutoGenerateColumns="false" Width="100%"
                    OnRowDataBound="gridDutyHr_RowDataBound" runat="server">
                    <Columns>
                        <asp:TemplateField HeaderText="S.N">
                            <ItemTemplate>
                                <asp:Label ID="lblSn" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Emp Id">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpId" runat="server" Text='<%#Bind("EMPLOYEEID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpName" runat="server"></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fix Time">
                            <HeaderTemplate>
                                Fix Time<br />
                                <asp:RadioButtonList ID="rdBtnFixDutyH" AutoPostBack="true" RepeatDirection="Horizontal" runat="server" OnSelectedIndexChanged="rdBtnFixDutyH_SelectedIndexChanged">
                                    <asp:ListItem Text="Y" Value="1" Selected="True" />
                                    <asp:ListItem Text="N" Value="0" />
                                </asp:RadioButtonList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:RadioButtonList ID="rdBtnFixDuty" AutoPostBack="true" RepeatDirection="Horizontal" runat="server" OnSelectedIndexChanged="rdBtnFixDuty_SelectedIndexChanged">
                                    <asp:ListItem Text="Y" Value="1" Selected="True" />
                                    <asp:ListItem Text="N" Value="0" />
                                </asp:RadioButtonList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Duty Type">
                            <HeaderTemplate>
                                Duty Type
                    <br />
                                <asp:DropDownList ID="ddlDutyTypeH" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlDutyTypeH_SelectedIndexChanged"></asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDutyType" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlDutyType_SelectedIndexChanged"></asp:DropDownList>
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="In Time">
                            <HeaderTemplate>
                                In Time
                    <br />
                                <asp:DropDownList CssClass="form-control" ID="ddlInTimeH" Width="100px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInTimeH_SelectedIndexChanged"></asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control" ID="ddlInTime" Width="100px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlInTime_SelectedIndexChanged"></asp:DropDownList>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Out Time">
                            <HeaderTemplate>
                                Out Time
                    <br />
                                <asp:DropDownList CssClass="form-control" ID="ddlOutTimeH" Width="100px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOutTimeH_SelectedIndexChanged"></asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control" ID="ddlOutTime" Width="100px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlOutTime_SelectedIndexChanged"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Duty Hour">
                            <HeaderTemplate>
                                Duty Hour<br />
                                <asp:TextBox ID="txtDutyHrH" runat="server" Width="50px" Height="25px" AutoPostBack="true" OnTextChanged="txtDutyHrH_TextChanged"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txtDutyHr" runat="server" Width="50px" Height="25px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Pre In Round Off">
                            <HeaderTemplate>
                                Pre In Round
                                <br />
                                <asp:TextBox ID="txtPreInH" runat="server" Width="50px" Height="25px" AutoPostBack="true" OnTextChanged="txtPreInH_TextChanged"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txtPre_in" CssClass="form-control" TextMode="Number" runat="server" Width="50px" Height="25px"></asp:TextBox>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Post In Round Off">
                            <HeaderTemplate>
                                Pre Out Round
                                <br />
                                <asp:TextBox ID="txtPostInH" runat="server" AutoPostBack="true" Width="50px" Height="25px" OnTextChanged="txtPostInH_TextChanged"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txtPost_in" CssClass="form-control" TextMode="Number" runat="server" Width="50px" Height="25px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Weekend 1">
                            <HeaderTemplate>
                                Weekend 1
                    <br />
                                <asp:DropDownList CssClass="form-control" ID="ddlWeekEnd1H" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlWeekEnd1H_SelectedIndexChanged">
                                    <asp:ListItem Text="-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Sunday" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Monday" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Tuesday" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Wenesday" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Thrusday" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Friday" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Saturday" Value="7" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control" ID="ddlWeekEnd1" runat="server" Width="100%">
                                    <asp:ListItem Text="-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Sunday" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Monday" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Tuesday" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Wenesday" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Thrusday" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Friday" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Saturday" Value="7"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Weekend 2">
                            <HeaderTemplate>
                                Weekend 2
                    <br />
                                <asp:DropDownList CssClass="form-control" ID="ddlWeekEnd2H" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlWeekEnd2H_SelectedIndexChanged">
                                    <asp:ListItem Text="-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Sunday" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Monday" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Tuesday" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Wenesday" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Thrusday" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Friday" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Saturday" Value="7" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control" ID="ddlWeekEnd2" runat="server" Width="100%">
                                    <asp:ListItem Text="-" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Sunday" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Monday" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Tuesday" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Wenesday" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Thrusday" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Friday" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Saturday" Value="7"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Save">
                            <ItemTemplate>
                                <asp:Button runat="server" Text="Save" CommandName="SaveRow" CssClass="btn btn-primary" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSaveGrid" CssClass="btn btn-primary" runat="server" Text="Save All" OnClick="btnSaveGrid_Click" />
                        </td>
                    </tr>
                </table>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

