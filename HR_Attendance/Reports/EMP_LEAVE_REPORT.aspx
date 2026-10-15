<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EMP_LEAVE_REPORT.aspx.cs" Inherits="HR_Attendance_Reports_EMP_LEAVE_REPORT" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td>Report Type
                <asp:DropDownList ID="ddlReportType" AutoPostBack="true" Height="22px"  runat="server">
                    <asp:ListItem Text="Detail" />
                    <asp:ListItem Text="Summary" />
                </asp:DropDownList>
            </td>
            <td>Year<br />
                <asp:TextBox ID="txtYear" runat="server" Width="80px" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td>Department<br />
                <asp:DropDownList runat="server" Width="200px" ID="ddlDepartmentSearch" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartmentSearch_SelectedIndexChanged" />
            </td>
            <td>Division<br />
                <asp:DropDownList runat="server" Width="200px" ID="ddlDivisionSearch" AutoPostBack="true" OnSelectedIndexChanged="ddlDivisionSearch_SelectedIndexChanged" />
            </td>
            <td>Employee<br />
                <asp:DropDownList runat="server" Width="200px" ID="ddlEmployeeSearch" AutoPostBack="true" />
            </td>

        </tr>
        <tr>
            <td colspan="4">
                <table>
                    <tr>
                        <td>
                            <asp:Button Text="View" ID="btnView" OnClick="btnView_Click" runat="server" Style="height: 26px" /></td>
                        <td>
                            <asp:Button Text="Print" ID="btnPrint" OnClick="btnPrint_Click" runat="server" /></td>
                        <td>
                            <asp:Button Text="Export to Excel" ID="btnExcel" OnClick="btnExcel_Click" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div runat="server" id="hide" visible="false" class="row" style="padding: 0px 30px 0 30px;">
        <div id="print">
            <table id="tblExportExcel" runat="server" align="center">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblCompanyName" runat="server" Text="" Style="font-weight: bold; font-size: 20px;"></asp:Label><br>
                        <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label><br />
                        Contact No:<asp:Label ID="lblContactNo" runat="server" Text=""></asp:Label>
                        Website:<asp:Label ID="lblWebsite" runat="server" Text=""></asp:Label><br />
                        <asp:Label ID="lblReport" runat="server" Text="" Style="font-weight: bold; font-size: 14px"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" id="trSummaryReport" visible="false">
                    <td>
                        <asp:Label Text="" ID="lblEmpIDSumRprt" Visible="false" runat="server" />
                        <asp:Label Text="" ID="lblEmpNameSumRprt" Visible="false" runat="server" />
                        <asp:GridView ID="gridEmpLeave" Visible="false" OnRowDataBound="gridEmpLeave_RowDataBound" Width="100%" CssClass="gridtable" AutoGenerateColumns="false" runat="server">
                            <Columns>
                                <asp:TemplateField HeaderText="SN">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Container.DataItemIndex+1 %>' ID="lblSN" runat="server" />
                                        <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPK_ID" Visible="false" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Leave Type">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("LEAVE_TYPE") %>' ID="lblleaveType" Visible="false" runat="server" />
                                        <asp:Label Text="" ID="lblLeaveName" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Apply Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("APPLY_DATE") %>' ID="lblAPPLY_DATE" runat="server" /><br />
                                        <asp:Label Text="" ID="lblApplyDate" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="From Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("FROM_DATE") %>' ID="lblFROM_DATE" runat="server" /><br />
                                        <asp:Label Text="" ID="lblFromDate" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("TO_DATES") %>' ID="lblTO_DATE" runat="server" /><br />
                                        <asp:Label Text="" ID="lblToDate" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("REMARKS") %>' ID="lblREMARKS" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="No. of Days">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("DAYS") %>' ID="lblDAYS" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Recommended By">
                                    <ItemTemplate>
                                        <asp:Label Text="" ID="lblRECOMMEND_BY" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Approved By">
                                    <ItemTemplate>
                                        <asp:Label Text="" ID="lblAPPROVE_BY" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </td>
                </tr>

                <tr runat="server" id="trGenReport" visible="false">
                    <td>

                        <asp:GridView ID="grdRemainindLeave" runat="server" AutoGenerateColumns="False" CssClass="gridtable">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Leave Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLeaveType" runat="server" Text='<%# Bind("LEAVE_TYPE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Alloted">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLeaveType" runat="server" Text='<%# Bind("ALLOTED") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Utilized">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUtilize" runat="server" Text='<%# Bind("UTILIZED") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remaining">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemaining" runat="server" Text='<%# Bind("REMAININGDAYS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>

    </div>
    <script src="../../js/print.js"></script>

</asp:Content>

