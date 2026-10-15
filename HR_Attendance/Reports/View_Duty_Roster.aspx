<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="View_Duty_Roster.aspx.cs" Inherits="HR_Attendance_Reports_View_Duty_Roster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function printPartOfPage() {
            var printContent = document.getElementById('<%= divGrid.ClientID %>');
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=0,top=0,width=0,height=0');

            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            //printWindow.close();
        }
    </script>

    <table id="tblAllEmp" runat="server">
        <tr>
            <td><strong>Department
            </strong>
            </td>
            <td>
                <asp:DropDownList ID="ddlDept" runat="server" AutoPostBack="true" Width="200" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td><strong>Division</strong>
            </td>
            <td>
                <asp:DropDownList ID="ddlDiv" runat="server" Width="200px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><strong>Date Type:</strong>
            </td>
            <td>
                <asp:DropDownList ID="ddlDateType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDateType_SelectedIndexChanged">
                    <asp:ListItem Text="Nepali" Value="Nep" />
                    <asp:ListItem Text="English" Value="Eng" />
                </asp:DropDownList>
            </td>
            <td>&nbsp;&nbsp;<strong>Month:</strong>
            </td>
            <td>
                <asp:DropDownList ID="ddlNepMonth" runat="server" Visible="true">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlEngMonth" runat="server" Visible="false">
                </asp:DropDownList>
            </td>
            <td>&nbsp;&nbsp;<strong>Year:</strong>
            </td>
            <td>
                <asp:DropDownList ID="ddlEngYear" runat="server" Visible="false">
                </asp:DropDownList>

                <asp:DropDownList ID="ddlNepYear" runat="server" Visible="true">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" />
            </td>
            <td>
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
            </td>
            <td>
                <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click" Visible="false" />
            </td>
        </tr>
    </table>
    <div id="divGrid" style="width: 100%" runat="server">
        <style>
            @media print {
                .print-color {
                    -webkit-print-color-adjust: exact;
                    print-color-adjust: exact;
                }

                .off-day-print {
                    background-color: #ffcccc !important;
                }
            }
        </style>
        <table id="tblDate" class="gridtable" runat="server" visible="false">
            <tr>
                <td>
                    <span class="th-font" style="font-size: large !important"><strong>Duty Roster of
                        <asp:Label ID="lblDivision" runat="server"></asp:Label>
                        Division for
                        <asp:Label ID="lblMonthE" runat="server"></asp:Label>,
                        <asp:Label ID="lblYearE" runat="server"></asp:Label></strong></span>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdDutyShift" runat="server" CssClass="gridtable" AutoGenerateColumns="false" OnRowDataBound="grdDutyShift_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="S.N">
                                <ItemTemplate>
                                    <asp:Label ID="lblSn" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Id">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpId" runat="server" Text='<%# Bind("EMP_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("EMP_NAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- D1 to D32 --%>
                            <asp:TemplateField HeaderText="1">
                                <ItemTemplate>
                                    <asp:Label ID="lblD1" runat="server" Text='<%# Bind("D1") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="2">
                                <ItemTemplate>
                                    <asp:Label ID="lblD2" runat="server" Text='<%# Bind("D2") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="3">
                                <ItemTemplate>
                                    <asp:Label ID="lblD3" runat="server" Text='<%# Bind("D3") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="4">
                                <ItemTemplate>
                                    <asp:Label ID="lblD4" runat="server" Text='<%# Bind("D4") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="5">
                                <ItemTemplate>
                                    <asp:Label ID="lblD5" runat="server" Text='<%# Bind("D5") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="6">
                                <ItemTemplate>
                                    <asp:Label ID="lblD6" runat="server" Text='<%# Bind("D6") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="7">
                                <ItemTemplate>
                                    <asp:Label ID="lblD7" runat="server" Text='<%# Bind("D7") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="8">
                                <ItemTemplate>
                                    <asp:Label ID="lblD8" runat="server" Text='<%# Bind("D8") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="9">
                                <ItemTemplate>
                                    <asp:Label ID="lblD9" runat="server" Text='<%# Bind("D9") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="10">
                                <ItemTemplate>
                                    <asp:Label ID="lblD10" runat="server" Text='<%# Bind("D10") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="11">
                                <ItemTemplate>
                                    <asp:Label ID="lblD11" runat="server" Text='<%# Bind("D11") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="12">
                                <ItemTemplate>
                                    <asp:Label ID="lblD12" runat="server" Text='<%# Bind("D12") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="13">
                                <ItemTemplate>
                                    <asp:Label ID="lblD13" runat="server" Text='<%# Bind("D13") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="14">
                                <ItemTemplate>
                                    <asp:Label ID="lblD14" runat="server" Text='<%# Bind("D14") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="15">
                                <ItemTemplate>
                                    <asp:Label ID="lblD15" runat="server" Text='<%# Bind("D15") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="16">
                                <ItemTemplate>
                                    <asp:Label ID="lblD16" runat="server" Text='<%# Bind("D16") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="17">
                                <ItemTemplate>
                                    <asp:Label ID="lblD17" runat="server" Text='<%# Bind("D17") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="18">
                                <ItemTemplate>
                                    <asp:Label ID="lblD18" runat="server" Text='<%# Bind("D18") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="19">
                                <ItemTemplate>
                                    <asp:Label ID="lblD19" runat="server" Text='<%# Bind("D19") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="20">
                                <ItemTemplate>
                                    <asp:Label ID="lblD20" runat="server" Text='<%# Bind("D20") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="21">
                                <ItemTemplate>
                                    <asp:Label ID="lblD21" runat="server" Text='<%# Bind("D21") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="22">
                                <ItemTemplate>
                                    <asp:Label ID="lblD22" runat="server" Text='<%# Bind("D22") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="23">
                                <ItemTemplate>
                                    <asp:Label ID="lblD23" runat="server" Text='<%# Bind("D23") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="24">
                                <ItemTemplate>
                                    <asp:Label ID="lblD24" runat="server" Text='<%# Bind("D24") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="25">
                                <ItemTemplate>
                                    <asp:Label ID="lblD25" runat="server" Text='<%# Bind("D25") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="26">
                                <ItemTemplate>
                                    <asp:Label ID="lblD26" runat="server" Text='<%# Bind("D26") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="27">
                                <ItemTemplate>
                                    <asp:Label ID="lblD27" runat="server" Text='<%# Bind("D27") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="28">
                                <ItemTemplate>
                                    <asp:Label ID="lblD28" runat="server" Text='<%# Bind("D28") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="29">
                                <ItemTemplate>
                                    <asp:Label ID="lblD29" runat="server" Text='<%# Bind("D29") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="30">
                                <ItemTemplate>
                                    <asp:Label ID="lblD30" runat="server" Text='<%# Bind("D30") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="31">
                                <ItemTemplate>
                                    <asp:Label ID="lblD31" runat="server" Text='<%# Bind("D31") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>

