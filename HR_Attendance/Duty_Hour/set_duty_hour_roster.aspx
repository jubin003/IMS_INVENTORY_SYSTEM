<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="set_duty_hour_roster.aspx.cs"
    Inherits="HR_Attendance_Duty_Hour_set_duty_hour_roster" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updGrdWrkSchedule" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table id="EmpSearch" class="gridtable" runat="server">
                <tr id="view" runat="server">
                    <td><strong>Department</strong>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDept" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td><strong>Division</strong>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDiv" runat="server" Width="200px" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                  
                    <td>
                        <asp:Button ID="bttnSearch" runat="server" Text="Search" OnClick="bttnSearch_Click" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="grdEmployee" runat="server" CssClass="gridtable" AutoGenerateColumns="false" OnRowCommand="grdEmployee_RowCommand" Visible="false">
                <Columns>
                    <asp:TemplateField HeaderText="S.N">
                        <ItemTemplate>
                            <asp:Label ID="lblSn" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Id">
                        <ItemTemplate>
                            <asp:Label ID="lblEmpId" runat="server" Text='<%#Bind("EMP_ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Name">
                        <ItemTemplate>
                            <asp:Label ID="lblEmpName" runat="server" Text='<%#Bind("EMPLOYEE_NAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Add Duty Roster">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CommandName="View" Text="Add Duty Roster" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <table id="tblEmpInfo" runat="server" style="margin-top: 25px" visible="false">
                <tr>
                    <td style="padding: 5px"><strong>Employee Id:
                    </strong>
                    </td>
                    <td colspan="6" style="padding: 5px">
                        <asp:Label ID="lblEmployeeID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 5px"><strong>Employee Name:</strong>
                    </td>
                    <td colspan="6" style="padding: 5px">
                        <asp:Label ID="lblEmployeeName" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td style="padding: 5px"><strong>Date Type:</strong>
                    </td>
                    <td style="padding: 5px">
                        <asp:DropDownList ID="ddlDateType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDateType_SelectedIndexChanged">
                            <asp:ListItem Text="Nepali" Value="Nep" Selected="True" />
                            <asp:ListItem Text="English" Value="Eng" />
                        </asp:DropDownList>
                    </td>
                    <td style="padding: 5px">&nbsp;&nbsp;<strong>Month:</strong>
                    </td>
                    <td style="padding: 5px">
                        <asp:DropDownList ID="ddlEngMonth" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlEngMonth_SelectedIndexChanged">
                            <asp:ListItem Text="January" Value="1"></asp:ListItem>
                            <asp:ListItem Text="February" Value="2"></asp:ListItem>
                            <asp:ListItem Text="March" Value="3"></asp:ListItem>
                            <asp:ListItem Text="April" Value="4"></asp:ListItem>
                            <asp:ListItem Text="May" Value="5"></asp:ListItem>
                            <asp:ListItem Text="June" Value="6"></asp:ListItem>
                            <asp:ListItem Text="July" Value="7"></asp:ListItem>
                            <asp:ListItem Text="August" Value="8"></asp:ListItem>
                            <asp:ListItem Text="September" Value="9"></asp:ListItem>
                            <asp:ListItem Text="October" Value="10"></asp:ListItem>
                            <asp:ListItem Text="November" Value="11"></asp:ListItem>
                            <asp:ListItem Text="December" Value="12"></asp:ListItem>
                        </asp:DropDownList>

                        <asp:DropDownList ID="ddlNepMonth" runat="server" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="ddlNepMonth_SelectedIndexChanged">
                            <asp:ListItem Text="Baisakh" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Jestha" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Ashadh" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Shrawan" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Bhadra" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Ashwin" Value="6"></asp:ListItem>
                            <asp:ListItem Text="Kartik" Value="7"></asp:ListItem>
                            <asp:ListItem Text="Mangsir" Value="8"></asp:ListItem>
                            <asp:ListItem Text="Poush" Value="9"></asp:ListItem>
                            <asp:ListItem Text="Magh" Value="10"></asp:ListItem>
                            <asp:ListItem Text="Falgun" Value="11"></asp:ListItem>
                            <asp:ListItem Text="Chaitra" Value="12"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding: 5px">
                        <strong>Year:</strong>
                    </td>
                    <td style="padding: 5px">
                        <asp:DropDownList ID="ddlEngYear" Visible="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEngYear_SelectedIndexChanged"></asp:DropDownList>
                        <asp:DropDownList ID="ddlNepYear" Visible="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNepYear_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>

            </table>

            <asp:GridView ID="gridDutyHr" OnRowCommand="gridDutyHr_RowCommand" CssClass="gridtable" AutoGenerateColumns="false" Visible="false"
                OnRowDataBound="gridDutyHr_RowDataBound" runat="server">
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>

                            <asp:Label ID="lblNepDateG" runat="server" Text='<%#Bind("BS_DATE") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblNepDayG" runat="server" Text='<%#Bind("BS_DAY") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblEngDayG" runat="server" Text='<%#Bind("AD_DAY") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblEngDateG" runat="server" Text='<%#Bind("AD_DATE_F") %>' Visible="true"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Day">
                        <ItemTemplate>
                            <asp:Label ID="lblShowDay" runat="server" Text="Label"></asp:Label>
                            <asp:Label ID="lblDay" runat="server" Text='<%#Bind("DAY_N") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Working Day">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkWorking" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="chkWorking_CheckedChanged" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Shift">
                        <HeaderTemplate>
                            Shift<br />
                            <asp:DropDownList ID="ddlDutyTypeH" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDutyTypeH_SelectedIndexChanged"></asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDutyType" runat="server" Visible="false"></asp:Label>
                            <asp:DropDownList CssClass="form-control" ID="ddlDutyType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDutyType_SelectedIndexChanged"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="In Time">
                        <HeaderTemplate>
                            In Time
                    <br />
                            <asp:TextBox ID="txtInHrH" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="txtInHrH_TextChanged"></asp:TextBox>&nbsp;:
                    <asp:TextBox ID="txtInMinH" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="txtInMinH_TextChanged"></asp:TextBox>
                            <asp:DropDownList ID="ddlInAMPMH" runat="server" Width="50px" AutoPostBack="true" OnSelectedIndexChanged="ddlInAMPMH_SelectedIndexChanged">
                                <asp:ListItem Text="AM" Value="AM" />
                                <asp:ListItem Text="PM" Value="PM" />
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblInTime" runat="server" Visible="false"></asp:Label>
                            <asp:TextBox ID="txtInHr" runat="server" Width="30px" Text="00"></asp:TextBox>&nbsp;:
                    <asp:TextBox ID="txtInMin" runat="server" Width="30px" Text="00"></asp:TextBox>

                            <asp:DropDownList ID="ddlInAMPM" Width="50px" runat="server">
                                <asp:ListItem Text="AM" Value="AM" />
                                <asp:ListItem Text="PM" Value="PM" />
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Out Time">
                        <HeaderTemplate>
                            Out Time<br />
                            <asp:TextBox ID="txtOutHrH" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="txtOutHrH_TextChanged"></asp:TextBox>&nbsp;:
                    <asp:TextBox ID="txtOutMinH" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="txtOutMinH_TextChanged"></asp:TextBox>
                            <asp:DropDownList ID="ddlOutAMPMH" runat="server" Width="50px" AutoPostBack="true" OnSelectedIndexChanged="ddlOutAMPMH_SelectedIndexChanged">
                                <asp:ListItem Text="AM" Value="AM" />
                                <asp:ListItem Text="PM" Value="PM" />
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtOutHr" runat="server" Width="30px" Text="00"></asp:TextBox>&nbsp;:
                    <asp:TextBox ID="txtOutMin" runat="server" Width="30px" Text="00"></asp:TextBox>
                            <asp:DropDownList ID="ddlOutAMPM" Width="50px" runat="server">
                                <asp:ListItem Text="AM" Value="AM" />
                                <asp:ListItem Text="PM" Value="PM" />
                            </asp:DropDownList>
                            <asp:Label ID="lblOutTime" runat="server" Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Save">
                        <ItemTemplate>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CommandName="SaveRow" CssClass="btn btn-primary" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

            <table id="tblSaveBtn" runat="server" visible="false">
                <tr>
                    <td>
                        <asp:Button ID="btnSaveGrid" CssClass="btn btn-primary" runat="server" Text="Save All" OnClick="btnSaveGrid_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

