<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HR_Emp_History_Report.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="HumanResource_Reports_HR_Emp_History_Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function printPartOfPage() {
            var printContent = document.getElementById('div_print');
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
    <asp:UpdatePanel ID="updGrdWrkSchedule" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
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
                                            <asp:TemplateField HeaderText="View">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>

                        <table id="tblEmpDetail" style="width: 850px;" runat="server" visible="false">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnBack" Width="100px" runat="server" Text="Back" OnClick="btnBack_Click" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnPrint" Width="100px" runat="server" Text="Print" OnClick="btnPrint_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="div_print">
                                        <table style="width: 100%; border: 1px solid #e0e0e0; border-radius: 5px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);">
                                            <tr>
                                                <td style="text-align: center; height: 50px; background-color: #f8f9fa;">
                                                    <h1 style="color: #2c3e50; font-size: 24px; margin: 0; padding: 10px 0;">Employee History Information</h1>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-bottom: 15px">

                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 20%; padding: 5px 15px; font-weight: 600; color: #495057;">Employee ID</td>
                                                            <td style="width: 1%; padding: 5px 0; color: #495057;">:</td>
                                                            <td style="width: 50%; padding: 5px 15px; color: #2c3e50;">
                                                                <asp:Label ID="lblEmployeeID" runat="server"></asp:Label>
                                                            </td>
                                                            <td rowspan="2" style="width: 29%; vertical-align: top;">
                                                                <asp:Image ID="imgEmpPhotoD" Width="100px" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Employee Name</td>
                                                            <td style="padding: 5px 0; color: #495057;">:</td>
                                                            <td style="padding: 5px 15px; color: #2c3e50;">
                                                                <asp:Label ID="lblFullName" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" style="border: 1px solid #e0e0e0">
                                                        <tr>
                                                            <td style="width: 49%; vertical-align: top;">
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td colspan="3" style="padding: 10px 0; font-weight: 600; color: #495057; text-decoration: underline; font-size: 20px; text-align: center;">Joined Position</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Department</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblJnDepartment" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Division</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblJnDivision" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trJnUnit" runat="server">
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Unit</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblJnUnit" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Designation</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblJnDesignation" Width="200px" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Level</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblJnLevel" Width="200px" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Grade</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblJnGrade" Width="100px" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Joined Date</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblJnEffDate" Width="150px" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td style="width: 1%; vertical-align: top; border: 1px solid #e0e0e0"></td>
                                                            <td style="width: 50%; vertical-align: top;">
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td colspan="3" style="padding: 10px 0; font-weight: 600; color: #495057; text-decoration: underline; font-size: 20px; text-align: center;">Current Position</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Department</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblDepartment" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Division</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trUnit" runat="server">
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Unit</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblUnit" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Designation</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblDesignation" Width="200px" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Level</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblLevel" Width="200px" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Grade</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblGrade" Width="100px" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 5px 15px; font-weight: 600; color: #495057;">Effective Date</td>
                                                                        <td style="padding: 5px 0; color: #495057;">:</td>
                                                                        <td style="padding: 5px 15px; color: #2c3e50;">
                                                                            <asp:Label ID="lblEffDate" Width="150px" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="width: 100%; vertical-align: top;">
                                                    <asp:GridView ID="grdEmpHistory" CssClass="gridtable" Width="100%" runat="server" AutoGenerateColumns="False"
                                                        OnRowDataBound="grdEmpHistory_RowDataBound" OnRowCommand="grdEmpHistory_RowCommand">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="S.no">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                                                    <asp:Label ID="lblPK_IDG" runat="server" Text='<%#Bind("PK_ID") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Emp Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmpIdG" runat="server" Text='<%#Bind("EMP_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Work Place">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblWrkPlaceG" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblDepartmentG" runat="server" Text='<%#Bind("HR_DEPARTMENT_ID") %>' Visible="false">></asp:Label>
                                                                    <asp:Label ID="lblDesignationG" runat="server" Text='<%#Bind("HR_DESIGNATION_ID") %>' Visible="false">></asp:Label>
                                                                    <asp:Label ID="lblDivisionG" runat="server" Text='<%#Bind("HR_DIVISION_ID") %>' Visible="false">></asp:Label>
                                                                    <asp:Label ID="lblUnitG" runat="server" Text='<%#Bind("HR_UNIT_ID") %>' Visible="false">></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Level">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShowLevelG" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblLevelG" runat="server" Text='<%#Bind("HR_LEVEL_ID") %>' Visible="false">></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Grade">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShowGradeG" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblGradeG" runat="server" Text='<%#Bind("HR_GRADE_ID") %>' Visible="false">></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Effective Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNepEffDate" runat="server"></asp:Label><br />
                                                                    [<asp:Label ID="lblEffDate" runat="server" Text='<%#Bind("EFFECTIVE_DATE") %>'></asp:Label>]
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Offer Letter">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnOfferLetter" runat="server" Width="25px" CommandName="View1" ImageUrl="~/images/icons/pdf.png" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Agreement">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnAgreement" runat="server" Width="25px" CommandName="View2" ImageUrl="~/images/icons/pdf.png" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Contracts">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgBtnPdf" runat="server" Width="25px" CommandName="View3" ImageUrl="~/images/icons/pdf.png" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>

                                            </tr>

                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

