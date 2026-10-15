<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="attendanceEdit.aspx.cs" Inherits="HR_Attendance_Attendance_individualAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table id="EmpSearch" width="100%" runat="server">
        <tr>
            <td>
                <table style="width: 40%">
                    <tr>
                        <td>Department
                            <asp:DropDownList ID="ddlDept" AutoPostBack="true" Width="200px" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>Division:<br />
                            <asp:DropDownList AutoPostBack="true" CssClass="uc_formcontrol" runat="server" Width="200px" ID="ddlDivision" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td>Unit:<br />
                            <asp:DropDownList AutoPostBack="true" CssClass="uc_formcontrol" runat="server" Width="200px" ID="ddlUnit"></asp:DropDownList>
                        </td>
                        <td>Status:            
                            <asp:DropDownList ID="ddlStatus" Width="100px" runat="server" CssClass="form-control">
                                <asp:ListItem Selected="True" Value="1">Active</asp:ListItem>
                                <asp:ListItem Value="0">InActive</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <br />
                            <asp:Button ID="bttnSearch" Width="80px" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="bttnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="lblEmpIdTemp" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:GridView ID="GridEmpList" CssClass="gridtable" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" EnableModelValidation="True"
                    OnRowDataBound="GridEmpList_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="EMP ID">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpID" runat="server" Text='<%# Bind("EMPLOYEEID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NAME">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Width="170px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PHOTO">
                            <ItemTemplate>
                                <asp:Image ID="imgEmpPhoto" Width="50px" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DEPARTMENT">
                            <ItemTemplate>
                                <asp:Label ID="lblDeptId" runat="server" Text='<%# Bind("DEPARTMENTID") %>' Visible="False"></asp:Label>
                                <asp:Label ID="lblDivId" runat="server" Text='<%# Bind("DIVISION") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblUnitId" runat="server" Text='<%# Bind("UNIT") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblDept" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="DIVISION">
                            <ItemTemplate>
                                <asp:Label ID="lblDivId" runat="server" Text='<%# Bind("DIVISION") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblDiv" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="EDIT">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBttnEdit" runat="server" ImageUrl="~/images/icons/edit.png"
                                    OnClick="ImgBttnEdit_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>

    <table id="tblEmpDetail" visible="false" runat="server">
        <tr>
            <td colspan="3" runat="server" visible="false">
                <asp:RadioButtonList ID="rbtnDays" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rbtnDays_SelectedIndexChanged">
                    <asp:ListItem Value="O" Selected="True">One Day</asp:ListItem>
                    <asp:ListItem Value="M">Multi Day</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <br />
    <div id="detaildiv" visible="false" runat="server" style="background-color: #ccc; width: 100%; padding: 5px">

        <div id="info_div" style="background-color: #fff; margin: 10px; box-shadow: 1px 3px 8px #000;">
            <table>
                <tr>
                    <td><strong>Employee ID</strong></td>
                    <td>:</td>
                    <td>
                        <asp:Label runat="server" ID="lblEmpId"></asp:Label></td>
                </tr>
                <tr>
                    <td><strong>Name</strong></td>
                    <td>:</td>
                    <td>
                        <asp:Label runat="server" ID="lblEmpName"></asp:Label></td>
                </tr>
                <tr>
                    <td><strong>Office Location</strong></td>
                    <td>:</td>
                    <td>
                        <asp:Label runat="server" ID="lblOfficeLocation"></asp:Label></td>
                </tr>
                <tr>
                    <td><strong>Designation</strong></td>
                    <td>:</td>
                    <td>
                        <asp:Label runat="server" ID="lblDesignation"></asp:Label></td>
                </tr>
                <tr>
                    <td><strong>Job Title</strong></td>
                    <td>:</td>
                    <td>
                        <asp:Label runat="server" ID="lblJobTitle"></asp:Label></td>
                </tr>
            </table>
        </div>

        <div id="one" runat="server" style="background-color: #fff; margin: 10px; box-shadow: 1px 3px 8px #000;">
            <table>
                <tr>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rbtnStatus" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rbtnStatus_SelectedIndexChanged">
                            <asp:ListItem Value="1">Full Day</asp:ListItem>
                            <asp:ListItem Value="5">Half Day</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>Date</td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" Height="22px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnSearchOne" runat="server" Text="Search" OnClick="btnSearchOne_Click" />
                    </td>
                </tr>
                <tr>
                    <td>In Time</td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtInTimeHr" runat="server" Height="22px" Width="52px" Text="00" AutoPostBack="true" OnTextChanged="txtInTimeHr_TextChanged"></asp:TextBox>:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInTimeMin" runat="server" Height="22px" Width="52px" Text="00" AutoPostBack="true" OnTextChanged="txtInTimeMin_TextChanged"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlInAMPM" Width="52px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInAMPM_SelectedIndexChanged">
                                        <asp:ListItem Text="AM" Value="AM" />
                                        <asp:ListItem Text="PM" Value="PM" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>Out Time</td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtOutTimeHr" runat="server" Height="22px" Width="52px" Text="00" AutoPostBack="true" OnTextChanged="txtOutTimeHr_TextChanged"></asp:TextBox>:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOutTimeMin" runat="server" Height="22px" Width="52px" Text="00" AutoPostBack="true" OnTextChanged="txtOutTimeMin_TextChanged"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOutAMPM" Width="52px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOutAMPM_SelectedIndexChanged">
                                        <asp:ListItem Text="AM" Value="AM" />
                                        <asp:ListItem Text="PM" Value="PM" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td></td>
                </tr>

                <tr>
                    <td>Working Hour</td>
                    <td>
                        <asp:TextBox ID="txtWorkingHour" runat="server" Height="22px" ReadOnly="True" Text="00:00:00"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>Remarks</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table style="width: 80%">
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="100%" Height="55px"></asp:TextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td id="tblButtons" visible="true" runat="server">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="120px" OnClick="btnSave_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnCancelOne" runat="server" Text="Cancel" Width="100px" Visible="true" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>

                </tr>
            </table>
        </div>


        <div id="multi" runat="server" style="background-color: #fff; margin: 10px; box-shadow: 1px 3px 8px #000;">
            <table>

                <tr>
                    <td>From Date</td>
                    <td>
                        <asp:TextBox ID="txtFDate" runat="server" Height="22px" AutoPostBack="True"></asp:TextBox>
                    </td>
                    <td rowspan="2">
                        <asp:Button ID="btnSearchM" runat="server" Text="Search" OnClick="btnSearchM_Click" /></td>
                </tr>

                <tr>
                    <td>To Date</td>
                    <td>
                        <asp:TextBox ID="txtTDate" runat="server" Height="22px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Remarks</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table style="width: 80%">
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtRemarksSecond" runat="server" TextMode="MultiLine" Width="100%" Height="55px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSavee" runat="server" Text="Save" Width="100px" OnClick="btnSavee_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnCancelMultiple" runat="server" Text="Cancel" Width="100px" Visible="true" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>

                </tr>
            </table>

            <table style="width: 100%">
                <tr>
                    <td>

                        <asp:GridView runat="server" Width="100%" AutoGenerateColumns="False" EnableModelValidation="True" ID="gridAttendance" AllowSorting="True" OnRowDataBound="gridAttendance_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Date A.D">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDay" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("ATT_DATE") %>'></asp:Label>
                                        <asp:Label ID="lblPkId" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date B.S">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNPDate" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Day">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDayOfWeek" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="In Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInTime" runat="server" Text='<%# Bind("IN_TIME") %>' Visible="false"></asp:Label>
                                        <asp:TextBox ID="txtInHr_M" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="txtInHr_M_TextChanged"></asp:TextBox>&nbsp;:
                                        <asp:TextBox ID="txtInMin_M" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="txtInMin_M_TextChanged"></asp:TextBox>
                                        <asp:DropDownList ID="ddlInAMPM_M" runat="server" Width="50px" AutoPostBack="true" OnSelectedIndexChanged="ddlInAMPM_M_SelectedIndexChanged">
                                            <asp:ListItem Text="AM" Value="AM" />
                                            <asp:ListItem Text="PM" Value="PM" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Out Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutTime" runat="server" Text='<%# Bind("OUT_TIME") %>' Visible="false"></asp:Label>
                                        <asp:TextBox ID="txtOutHr_M" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="txtOutHr_M_TextChanged"></asp:TextBox>&nbsp;:
                                        <asp:TextBox ID="txtOutMin_M" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="txtOutMin_M_TextChanged"></asp:TextBox>
                                        <asp:DropDownList ID="ddlOutAMP_M" runat="server" Width="50px" AutoPostBack="true" OnSelectedIndexChanged="ddlOutAMP_M_SelectedIndexChanged">
                                            <asp:ListItem Text="AM" Value="AM" />
                                            <asp:ListItem Text="PM" Value="PM" />
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Working Hour">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtWorkingHour" runat="server" Text='<%# Bind("WORKING_HOUR") %>' Width="100%"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("REMARKS") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        window.onload = function () {
            // Initialize all datepickers with their respective formats
            document.getElementById("<%= txtDate.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
        };
    </script>

    <%--<script type="text/javascript">
        window.onload = function () {
            // Get the selected value from the radio button list
            var selectedValue = document.querySelector('input[name="<%= rbtnDays.UniqueID %>"]:checked')?.value;

            if (selectedValue === "O") {            
                document.getElementById("<%= txtDate.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
            } else if (selectedValue === "M") {            
                document.getElementById("<%= txtFDate.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
                document.getElementById("<%= txtTDate.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
            }
        };
    </script>--%>

    <script type="text/javascript">
        function validateHour(input) {
            let val = parseInt(input.value, 10);
            if (isNaN(val) || val < 1 || val > 12) {
                alert("Hour must be between 1 and 12.");
                input.value = "00";
            } else {
                input.value = val.toString().padStart(2, '0');
            }
        }

        function validateMinute(input) {
            let val = parseInt(input.value, 10);
            if (isNaN(val) || val < 0 || val > 59) {
                alert("Minute must be between 0 and 59.");
                input.value = "00";
            } else {
                input.value = val.toString().padStart(2, '0');
            }
        }
    </script>

  

</asp:Content>

