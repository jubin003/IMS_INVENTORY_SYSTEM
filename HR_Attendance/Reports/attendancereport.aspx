<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="attendancereport.aspx.cs" Inherits="HR_Attendance_Reports_attendancereport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript">
        function printdiv(printpage) {
            var headstr = "<html><head><title></title></head><style type='text/css'>#div_print    {  font-size: 4px !important;  } </style><body>";
            var footstr = "</body>";
            var newstr = document.all.item(printpage).innerHTML;
            var oldstr = document.body.innerHTML;
            document.body.innerHTML = headstr + newstr + footstr;
            window.print();
            document.body.innerHTML = oldstr;
            return false;
        }
    </script>


    <div id="allreport" runat="server">
        <table>
            <tr>
                <td>Department:</td>

                <td colspan="3">
                    <asp:DropDownList ID="ddlDepartment" runat="server" Height="22px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>View By</td>
                <td>
                    <asp:DropDownList ID="ddlViewBy" runat="server" Height="22px" AutoPostBack="True" OnSelectedIndexChanged="ddlViewBy_SelectedIndexChanged">
                        <asp:ListItem>Select</asp:ListItem>
                        <asp:ListItem Value="M">Month Wise</asp:ListItem>
                        <asp:ListItem Value="D">Date Range </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>Active Employees</td>
                <td>
                    <asp:CheckBox ID="chkActriveEmployees" runat="server" /></td>
            </tr>
            <tr id="monthwise" visible="false" runat="server">
                <td>Month</td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" Height="22px" Visible="false">
                        <asp:ListItem Value="1">January</asp:ListItem>
                        <asp:ListItem Value="2">February </asp:ListItem>
                        <asp:ListItem Value="3">March </asp:ListItem>
                        <asp:ListItem Value="4">April</asp:ListItem>
                        <asp:ListItem Value="5">May</asp:ListItem>
                        <asp:ListItem Value="6">June</asp:ListItem>
                        <asp:ListItem Value="7">July</asp:ListItem>
                        <asp:ListItem Value="8">August</asp:ListItem>
                        <asp:ListItem Value="9">September</asp:ListItem>
                        <asp:ListItem Value="10">October</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlNepaliMonth" runat="server" Height="22px">
                        <asp:ListItem Value="1">Baishak</asp:ListItem>
                        <asp:ListItem Value="2">Jestha </asp:ListItem>
                        <asp:ListItem Value="3">Asad </asp:ListItem>
                        <asp:ListItem Value="4">Sharawan</asp:ListItem>
                        <asp:ListItem Value="5">Bhadra</asp:ListItem>
                        <asp:ListItem Value="6">Aswin</asp:ListItem>
                        <asp:ListItem Value="7">Kartik</asp:ListItem>
                        <asp:ListItem Value="8">Manshir</asp:ListItem>
                        <asp:ListItem Value="9">Poush</asp:ListItem>
                        <asp:ListItem Value="10">Magh</asp:ListItem>
                        <asp:ListItem Value="11">Fagun</asp:ListItem>
                        <asp:ListItem Value="12">Chaitra</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtYear"></asp:TextBox></td>
            </tr>
            <tr id="daterange" runat="server" visible="false">
                <td>From Date</td>
                <td>
                    <asp:TextBox ID="txtFromDateNp" runat="server" Height="22px" CssClass="form-control datepicker"></asp:TextBox>
                </td>
                <td>To Date</td>
                <td>
                    <asp:TextBox ID="txtToDateNp" runat="server" Height="22px" CssClass="form-control datepicker"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnView" runat="server" CssClass="btn btn-primary" Text="View" OnClick="btnView_Click" /></td>
                <td>
                    <input name="b_print_all" onclick="printdiv('div_print_all')" type="button" value="Print" />
                </td>

            </tr>


        </table>
        <br />
        <div id="div_print_all">
            <div id="student_detail">
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: center;"></td>
                    </tr>
                    <tr>
                        <td style="text-align: center;"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblNameCompany" runat="server" Text="" Style="font-size: large; font-weight: 700"></asp:Label><br />
                            <asp:Label ID="lblattendancefor" runat="server" Text="">  </asp:Label>

                            <asp:GridView runat="server" AutoGenerateColumns="False" Width="100%" EnableModelValidation="True" ID="gridAttendance"
                                CellPadding="4" ForeColor="#333333" OnSorting="gridAttendance_Sorting" AllowSorting="true" DataKeyNames="EMPLOYEEID">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SN">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" SortExpression="EMPLOYEEID">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpID" runat="server" Text='<%# Bind("EMPLOYEEID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Employee Name" SortExpression="Name">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkName" runat="server" Text='<%# Bind("Name") %>' OnClick="lnkName_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Days">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWorkingDays" runat="server" Text='<%# Bind("totaldays") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Off Days">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOffdays" runat="server" Text='<%# Bind("OffDays") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Holidays">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHolidays" runat="server" Text='<%# Bind("Holidays") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Present Days">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPresentDays" runat="server" Text='<%# Bind("Presentdays") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Absent Days">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAbsentDays" runat="server" Text='<%# Bind("Absent") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Leave">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApproveLeaveDays" runat="server" Text='<%# Bind("Leave") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Half Day Leave">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHalfLeaveDays" runat="server" Text='<%# Bind("HalfDayLeave") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div id="individual" visible="false" runat="server">

        <table>
            <tr>
                <td>
                    <asp:Button runat="server" Text="Back" ID="btnBack" OnClick="btnBack_Click" />
                </td>
                <td>
                    <input name="b_print" onclick="printdiv('div_print');" type="button" value="Print" /></td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <div id="div_print">
                        <table width="100%">
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td style="text-align: left; width: 150px">Employee ID</td>
                                            <td style="text-align: left">
                                                <asp:Label runat="server" ID="lblEmpId"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left">Name</td>
                                            <td style="text-align: left">
                                                <asp:Label runat="server" ID="lblEmpName"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left">Division</td>
                                            <td style="text-align: left">
                                                <asp:Label runat="server" ID="lblDivision"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left">Designation</td>
                                            <td style="text-align: left">
                                                <asp:Label runat="server" ID="lblDesignation"></asp:Label></td>
                                        </tr>

                                        <tr>
                                            <td style="text-align: left">Date</td>
                                            <td style="text-align: left">
                                                <asp:Label runat="server" ID="lblDate"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" style="text-align: right">
                                    <asp:Image ID="imgEmployee" runat="server" Width="100px" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:GridView runat="server" Width="100%" AutoGenerateColumns="False"
                            EnableModelValidation="True" ID="gridIndAttendance" AllowSorting="True"
                            OnRowDataBound="gridIndAttendance_RowDataBound">
                            <Columns>
                                 <asp:TemplateField HeaderText="Date B.S">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNPDate" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date A.D">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDay" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("ATT_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                                <asp:TemplateField HeaderText="Day">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDayOfWeek" runat="server"></asp:Label>

                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="In Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInTime" runat="server" Text='<%# Bind("IN_TIME") %>' Width="120px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Duty From">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDutyFrom" runat="server" Text="" Width="120px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Out Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutTime" runat="server" Text='<%# Bind("OUT_TIME") %>' Width="120px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Duty To">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDutyTo" runat="server" Text="" Width="120px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Working Hour">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWorkingHour" runat="server" Text='<%# Bind("WORKING_HOUR") %>' Width="120px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Duty Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDutyTime" runat="server" Text='' Width="120px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ovet Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOverTime" runat="server" Text='' Width="120px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("REMARKS") %>'></asp:Label>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        window.onload = function () {
            // Initialize all datepickers with their respective formats
            document.getElementById("<%= txtFromDateNp.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
            document.getElementById("<%= txtToDateNp.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });

        };
    </script>
</asp:Content>

