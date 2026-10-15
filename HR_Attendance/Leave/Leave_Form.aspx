<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Leave_Form.aspx.cs" Inherits="HR_Attendance_Leave_Leave_Form" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .form {
            background-color: #fff;
            padding: 20px;
            border: 1px solid #ccc;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.05);
            border-radius: 8px;
            font-size: 15px;
            width: 100%;
            /*max-width: 1000px;*/
            margin-left: 0 !important;
            margin-right: auto !important;
            float: left;
        }

        .sub-container-heading {
            font-size: 20px;
            font-weight: 600;
            border-bottom: 2px solid #007acc;
            padding-bottom: 10px;
            margin-bottom: 20px;
            color: #333;
        }

        .form td {
            padding: 12px;
            vertical-align: top;
        }

        .form input[type="text"],
        .form textarea,
        .form select {
            width: 95%;
            height: 33px;
            padding: 6px 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
            font-size: 14px;
            box-sizing: border-box;
        }

        .form textarea {
            resize: vertical;
        }

        .form .label {
            font-weight: 500;
            color: #333;
        }

        .form .info-label {
            font-weight: bold;
            margin-top: 6px;
            display: inline-block;
        }

        .form .highlight-label {
            display: inline-block;
            background-color: #007acc;
            color: #fff;
            padding: 3px 8px;
            border-radius: 4px;
            font-size: 13px;
        }

        .form .btn {
            background-color: #007acc;
            color: white;
            border: none;
            padding: 8px 20px;
            font-size: 15px;
            border-radius: 5px;
            cursor: pointer;
            width: 100px;
        }

            .form .btn:hover {
                background-color: #005fa3;
            }
    </style>

    <asp:Label ID="lblPk_idLeave" Visible="false" runat="server" />

    <div id="divEditGrid" runat="server">
        <table>
            <tr>
                <td>From Date<br />
                    <asp:TextBox ID="txtFDate" runat="server" />
                </td>
                <td>To Date
                    <br />
                    <asp:TextBox ID="txtTDate" runat="server" />

                </td>
                <td>Department<br />
                    <asp:DropDownList runat="server" Width="200px" ID="ddlDepartmentSearch" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartmentSearch_SelectedIndexChanged" />
                </td>
                <td>Division<br />
                    <asp:DropDownList runat="server" Width="200px" ID="ddlDivisionSearch" AutoPostBack="true" OnSelectedIndexChanged="ddlDivisionSearch_SelectedIndexChanged" />
                </td>
                <td>Employee<br />
                    <asp:DropDownList runat="server" Width="200px" ID="ddlEmployeeSearch" AutoPostBack="true" />
                </td>
                <td>
                    <asp:Button Text="View" OnClick="btn_view_Click" ID="btn_view" runat="server" />
                </td>
                <td>
                    <asp:Button Text="Add" OnClick="btn_add_Click" ID="btn_add" runat="server" />
                </td>
            </tr>

        </table>
        <table>
            <tr>

                <td colspan="2">
                    <asp:GridView ID="gridLeaveEdit" Width="100%" 
                        OnRowCommand="gridLeaveEdit_RowCommand" AutoGenerateColumns="false" CssClass="gridtable" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderText="Sn">
                                <ItemTemplate>
                                    <asp:Label ID="lblSN" Text='<%#Container.DataItemIndex+1%>' runat="server" />
                                    <asp:Label Text='<%#Bind("PK_ID") %>' ID="lblPk_ID" Visible="false" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Apply Dates">
                                <ItemTemplate>
                                    <asp:Label Text='<%#Bind("APPLY_DATE") %>' ID="lblApplyDate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="From Dates">
                                <ItemTemplate>
                                    <asp:Label Text='<%#Bind("FROM_DATE") %>' ID="lblFDate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To Date">
                                <ItemTemplate>
                                    <asp:Label Text='<%#Bind("TO_DATES") %>' ID="lblTDate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leave Type">
                                <ItemTemplate>
                                    <asp:Label Text='<%#Bind("LEAVE_TYPE") %>' ID="lblType" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leave Days">
                                <ItemTemplate>
                                    <asp:Label Text='<%#Bind("DAYS") %>' ID="lblDays" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdit" runat="server" CommandName="Change" ImageUrl="~/images/icons/edit.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" OnClientClick="return confirm('Confirm delete?');" runat="server" ImageUrl="~/images/icons/deletes.png" CommandName="Alter" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>

    <div id="divLeave" visible="false" runat="server">
        <table>
            <tr>
                <td>
                    <table id="leaveForm" class="form" style="width: 1000px; border-collapse: collapse;" runat="server" visible="true">
                        <tr>
                            <td colspan="5" class="sub-container-heading" style="font-weight: bold; font-size: large;">Leave Form</td>
                        </tr>
                        <tr>
                            <td style="width: 120px;">Apply Date:<br />
                                <asp:TextBox ID="txtApplyDate" AutoComplete="off" runat="server" />
                            </td>
                            <td>Department:<br />
                                <asp:DropDownList runat="server" ID="ddlDept" Height="33px" AutoPostBack="true" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged" />
                            </td>
                            <td style="width: 200px;">Division:<br />
                                <asp:DropDownList runat="server" ID="ddlDiv" AutoPostBack="true" OnSelectedIndexChanged="ddlDiv_SelectedIndexChanged1" />
                            </td>
                            <td style="width: 250px;" colspan="2">Employee:<br />
                                <asp:DropDownList runat="server" ID="ddlEmployees" AutoPostBack="true" OnSelectedIndexChanged="ddlEmployees_SelectedIndexChanged" />
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 200px">Leave Type:<br />
                                <asp:DropDownList runat="server" ID="ddlLeaveType" AutoPostBack="True" Enabled="false" OnSelectedIndexChanged="ddlLeaveType_SelectedIndexChanged" />
                                <br />
                                <br />
                                <asp:Label Text="Remaining Days:" ID="lblRemDaysLabel" CssClass="p-5" ForeColor="OrangeRed" Visible="false" runat="server" />
                                <asp:Label Text="" ID="lblRemLeave" CssClass="p-5" ForeColor="OrangeRed" Visible="true" runat="server" />
                            </td>
                            <td style="width: 120px;">From Date:<br />
                                <asp:TextBox ID="txtFromDate" AutoPostBack="true" AutoComplete="off" OnTextChanged="txtFromDate_TextChanged" runat="server" />
                                <br />
                                <br />
                                <asp:Label Text="Request Days:" ID="lblRequestDaysLabel" ForeColor="OrangeRed" Visible="false" runat="server" />
                                <asp:Label Text="" ID="lblRequestDays" ForeColor="OrangeRed" Visible="true" runat="server" />
                            </td>
                            <td style="width: 120px;">To Date:<br />
                                <asp:TextBox ID="txtToDate" AutoPostBack="true" AutoComplete="off" OnTextChanged="txtToDate_TextChanged" runat="server" />
                            </td>
                            <td style="width: 200px;">Leave Nature:<br />

                                <asp:DropDownList ID="ddlLeaveNature" Width="100%" runat="server">
                                    <asp:ListItem Value="FD" Text="Full Day" />
                                    <asp:ListItem Value="HD" Text="Half Day" />
                                </asp:DropDownList>

                            </td>
                            <td style="width: 200px;">
                                <br />
                                <asp:Button ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" />

                            </td>
                        </tr>

                        <tr>
                            <td colspan="5">Remarks:<br />
                                <asp:TextBox ID="txtRemarks" Width="100%" Height="100px" TextMode="MultiLine" runat="server" />
                            </td>
                        </tr>

                        <tr>
                            <td>Recommend By:<br />
                                <asp:DropDownList ID="ddlRecommendBy" Height="32px" Width="100%" runat="server" />
                            </td>
                            <td></td>
                            <td></td>
                            <td colspan="2">Approved By:<br />
                                <asp:DropDownList ID="ddlApprovedBy" Height="32px" Width="100%" runat="server" />
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: center;"></td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                            </td>
                            <td></td>
                        </tr>

                    </table>

                </td>
                <td style="vertical-align: top">
                    <asp:GridView ID="gridDate" runat="server" Visible="true" CssClass="gridtable" AutoGenerateColumns="False" EnableModelValidation="True"
                        OnRowDataBound="gridDate_RowDataBound">
                        <%--  <%-- OnRowCreated="gridDate_RowCreated"--%>
                        <AlternatingRowStyle BackColor="#FFCCFF" />
                        <Columns>
                            <asp:TemplateField HeaderText="SN">
                                <ItemTemplate>
                                    <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="true" HeaderText="Date AD">
                                <ItemTemplate>
                                    <asp:Label ID="lblEngDate" runat="server" Text='<%# Bind ("Date") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date BS">
                                <ItemTemplate>
                                    <asp:Label ID="lblNepDate" runat="server"></asp:Label>
                                    <asp:Label ID="lblNepDay" runat="server"  Text="" Visible="false"></asp:Label>
                                    <asp:Label ID="lblNepMonth" runat="server" Text="" Visible="false"></asp:Label>
                                    <asp:Label ID="lblNepYear" runat="server" Text="" Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Day">
                                <ItemTemplate>
                                    <asp:Label ID="lblDay" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Days">
                                <ItemTemplate>
                                    <asp:Label ID="lblDays" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>




    </div>

    <script type="text/javascript">
        window.onload = function () {

            function initNepaliDatePicker(id) {
                var el = document.getElementById(id);
                if (el) {
                    el.NepaliDatePicker({
                        dateFormat: "DD/MM/YYYY",
                        miniEnglishDates: true
                    });
                }
            }

            initNepaliDatePicker("<%= txtFDate.ClientID %>");
            initNepaliDatePicker("<%= txtTDate.ClientID %>");
            initNepaliDatePicker("<%= txtApplyDate.ClientID %>");
            initNepaliDatePicker("<%= txtFromDate.ClientID %>");
            initNepaliDatePicker("<%= txtToDate.ClientID %>");
        };
    </script>

</asp:Content>
