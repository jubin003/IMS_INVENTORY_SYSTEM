<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HR_Emp_Promotion.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="HumanResource_Utilities_HR_Emp_Promotion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updGrdWrkSchedule" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <style type="text/css">
                .btn-back {
                    background-color: #6c757d;
                    color: white;
                    border: none;
                    padding: 8px 20px;
                    border-radius: 4px;
                    cursor: pointer;
                    font-size: 14px;
                    transition: background-color 0.3s;
                }

                    .btn-back:hover {
                        background-color: #5a6268;
                    }

                .gridtable {
                    border-collapse: separate;
                    border-spacing: 0;
                }

                    .gridtable th {
                        border-bottom: 2px solid #dee2e6;
                        color: #495057;
                        text-align: left;
                    }

                    .gridtable td {
                        border-bottom: 1px solid #e9ecef;
                        color: #495057;
                    }

                .form-control:focus {
                    border-color: #80bdff;
                    outline: 0;
                    box-shadow: 0 0 0 0.2rem rgba(0,123,255,0.25);
                }

                .auto-style1 {
                    height: 64px;
                }
            </style>

            <table>
                <tr>
                    <td>
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
                        <table id="tblEmpDetail" style="width: 800px;" runat="server" visible="false">
                            <tr>
                                <td>
                                    <table style="width: 100%; border: 1px solid #e0e0e0; border-radius: 5px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);">
                                        <tr>
                                            <td colspan="3" style="text-align: center; height: 50px; background-color: #f8f9fa;">
                                                <h1 style="color: #2c3e50; font-size: 24px; margin: 0; padding: 10px 0;">Employee Information</h1>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <table style="width: 100%; border-bottom: 1px solid #e0e0e0;">
                                                    <tr>
                                                        <td colspan="2">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td style="width: 150px; padding: 12px 15px; font-weight: 600; color: #495057;">Employee ID</td>
                                                                    <td style="width: 10px; padding: 12px 0; color: #495057;">:</td>
                                                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                                                        <asp:Label ID="lblEmployeeID" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Employee Name</td>
                                                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                                                        <asp:Label ID="lblFullName" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Department</td>
                                                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                                                        <asp:Label ID="lblDepartment" runat="server"></asp:Label>
                                                                        <asp:Label ID="lblDeptID" runat="server" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Division</td>
                                                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                                                        <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                                                        <asp:Label ID="lblDivId" runat="server" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trUnit" runat="server">
                                                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Unit</td>
                                                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                                                        <asp:Label ID="lblUnit" runat="server"></asp:Label>
                                                                        <asp:Label ID="lblUnitId" runat="server" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td colspan="1">
                                                            <asp:Image ID="imgEmpPhotoD" Width="100px" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="width: 49%; vertical-align: top;">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td colspan="3" style="padding: 10px 0; font-weight: 600; color: #495057; text-decoration: underline; font-size: 20px; text-align: center;">Current Position</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Designation</td>
                                                        <td style="padding: 12px 0; color: #495057;">:</td>
                                                        <td style="padding: 12px 15px; color: #2c3e50;">
                                                            <asp:Label ID="lblDesignation" Width="200px" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Level</td>
                                                        <td style="padding: 12px 0; color: #495057;">:</td>
                                                        <td style="padding: 12px 15px; color: #2c3e50;">
                                                            <asp:Label ID="lblLevel" Width="200px" runat="server" Text="Label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Grade</td>
                                                        <td style="padding: 12px 0; color: #495057;">:</td>
                                                        <td style="padding: 12px 15px; color: #2c3e50;">
                                                            <asp:Label ID="lblGrade" Width="100px" runat="server" Text="Label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Effective Date</td>
                                                        <td style="padding: 12px 0; color: #495057;">:</td>
                                                        <td style="padding: 12px 15px; color: #2c3e50;">
                                                            <asp:Label ID="lblEffDate" Width="150px" runat="server" Text="Label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td style="padding: 12px 15px;" colspan="3">
                                                            <asp:ImageButton ID="imgBtnPdf" runat="server" Width="50px" Height="50px" ImageUrl="~/images/icons/pdf.png" OnClick="imgBtnPdf_Click" />
                                                        </td>
                                                    </tr>--%>
                                                </table>
                                            </td>
                                            <td style="width: 1%; vertical-align: top; border: 1px solid #e0e0e0"></td>
                                            <td style="width: 50%; vertical-align: top;">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td colspan="3" style="padding: 10px 0; font-weight: 600; color: #495057; text-decoration: underline; font-size: 20px; text-align: center;">Next Position</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Designation</td>
                                                        <td style="padding: 12px 0; color: #495057;">:</td>
                                                        <td style="padding: 12px 15px; color: #2c3e50;">
                                                            <asp:DropDownList ID="ddlDesignation" Width="200px" runat="server"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Level</td>
                                                        <td style="padding: 12px 0; color: #495057;">:</td>
                                                        <td style="padding: 12px 15px; color: #2c3e50;">
                                                            <asp:DropDownList ID="ddlLevel" Width="200px" runat="server"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Grade</td>
                                                        <td style="padding: 12px 0; color: #495057;">:</td>
                                                        <td style="padding: 12px 15px; color: #2c3e50;">
                                                            <asp:DropDownList ID="ddlGrade" Width="100px" runat="server"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Effective Date</td>
                                                        <td style="padding: 12px 0; color: #495057;">:</td>
                                                        <td style="padding: 12px 15px; color: #2c3e50;">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Offer Letter</td>
                                                        <td style="padding: 12px 0; color: #495057;">:</td>
                                                        <td style="padding: 12px 15px; color: #2c3e50;">
                                                            <asp:FileUpload ID="fileContract" runat="server"  />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="padding: 12px 15px; color: #2c3e50; text-align: center">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSave" Width="100px" runat="server" Text="Save" OnClick="btnSave_Click" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnCancel" Width="100px" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
    <%--<script>
        window.onload = function () {

            var dobDateElement = document.getElementById("<%= txtEffectiveDate.ClientID %>");
            if (dobDateElement) {
                $(dobDateElement).nepaliDatePicker({
                    onChange: function () {
                        setTimeout(function () {
                            var nepaliDate = dobDateElement.value;
                            var dateParts = nepaliDate.split('-'); // Split to [yyyy, MM, dd]

                            if (dateParts.length === 3) {
                                // Format as dd/MM/yyyy
                                dobDateElement.value = dateParts[2] + '/' + dateParts[1] + '/' + dateParts[0];
                            }
                        });
                    }
                });
            }

        };
    </script>--%>
</asp:Content>

