<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HR_Emp_Training.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="HumanResource_Utilities_HR_Emp_Training" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .training-table {
            width: 100%;
            max-width: 800px;
            border-collapse: separate;
            border-spacing: 10px;
            background-color: #f9f9f9;
            border: 1px solid #ddd;
            padding: 15px;
            border-radius: 10px;
            /*font-family: Arial, sans-serif;*/
            box-shadow: 0 2px 6px rgba(0,0,0,0.1);
        }

            .training-table th {
                text-align: left;
                font-size: 18px;
                color: #333;
                padding-bottom: 10px;
            }

            .training-table td {
                vertical-align: top;
            }

            .training-table label {
                font-weight: bold;
                color: #444;
                display: inline-block;
                margin-bottom: 5px;
            }

            .training-table input[type="text"],
            .training-table textarea {
                width: 100%;
                padding: 6px 10px;
                border: 1px solid #ccc;
                border-radius: 5px;
                font-size: 14px;
                box-sizing: border-box;
            }

            .training-table textarea {
                resize: vertical;
            }
    </style>
    <asp:UpdatePanel ID="updGrdWrkSchedule" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <table id="EmpSearch" runat="server" style="width: 40%">
                <tr>
                    <td>Department
            
                <asp:DropDownList ID="ddlDept" AutoPostBack="true" Width="200px" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged1">
                </asp:DropDownList>
                    </td>
                    <td>Division:<br />
                        <asp:DropDownList AutoPostBack="true" CssClass="uc_formcontrol" runat="server" Width="200px" ID="ddlDivision" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td>Unit:<br />
                        <asp:DropDownList AutoPostBack="true" CssClass="uc_formcontrol" runat="server" Width="200px" ID="ddlUnit"></asp:DropDownList>
                    </td>
                    <td>Status<br />

                        <asp:DropDownList ID="ddlStatus" Width="150px" runat="server" CssClass="form-control">
                            <asp:ListItem Selected="True" Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="0">InActive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <br />
                        <asp:Button ID="bttnSearch" Width="80px" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="bttnSearch_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:Button ID="btnPrintEmp" runat="server" Text="Print" Visible="false" />
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
                                <asp:TemplateField HeaderText="Add">
                                    <ItemTemplate>
                                        <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
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

            <table id="tblEmpDetail" style="width: 80%;" runat="server" visible="false">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnBack" runat="server" Width="100px" Text="Back" OnClick="btnBack_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnAddMore" runat="server" Width="100px" Text="Add More" OnClick="btnAddMore_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>

                </tr>
                <tr>
                    <td>
                        <table style="width: 100%; border: 1px solid #e0e0e0; border-radius: 5px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);">
                            <tr>
                                <td colspan="3" style="text-align: center; height: 50px; background-color: #f8f9fa;">
                                    <h1 style="color: #2c3e50; font-size: 24px; margin: 0; padding: 10px 0;">Employee Training Information</h1>
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
                                                        <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Position</td>
                                                        <td style="padding: 12px 0; color: #495057;">:</td>
                                                        <td style="padding: 12px 15px; color: #2c3e50;">
                                                            <asp:Label ID="lblPosition" runat="server"></asp:Label>
                                                            <asp:Label ID="lblDeptID" runat="server" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblDivId" runat="server" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblUnitId" runat="server" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Level</td>
                                                        <td style="padding: 12px 0; color: #495057;">:</td>
                                                        <td style="padding: 12px 15px; color: #2c3e50;">
                                                            <asp:Label ID="lblLevel" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="trUnit" runat="server">
                                                        <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Grade</td>
                                                        <td style="padding: 12px 0; color: #495057;">:</td>
                                                        <td style="padding: 12px 15px; color: #2c3e50;">
                                                            <asp:Label ID="lblGrade" runat="server"></asp:Label>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td colspan="1">
                                                <asp:Image ID="imgEmpPhotoD" Width="150px" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdEmpTraining" runat="server" Width="100%" CssClass="gridtable" AutoGenerateColumns="False"
                                        OnRowDataBound="grdEmpTraining_RowDataBound" OnRowCommand="grdEmpTraining_RowCommand">
                                        <Columns>

                                            <asp:TemplateField HeaderText="S.no">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                                    <asp:Label ID="lblPK_IDG" runat="server" Text='<%#Bind("PK_ID") %>' Visible="true"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="In House">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblShowInHouse" runat="server"></asp:Label>
                                                    <asp:Label ID="lblInHouse" runat="server" Text='<%#Bind("IN_HOUSE_STATUS") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Org Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrgName" runat="server" Text='<%#Bind("ORG_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Title">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" Text='<%#Bind("TRAINING_TITLE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Training Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNepTDate" runat="server"></asp:Label><br />
                                                    [<asp:Label ID="lblTDate" runat="server" Text='<%#Bind("TRAINING_DATE") %>'></asp:Label>]
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Certificate Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNepCDate" runat="server"></asp:Label><br />
                                                    [<asp:Label ID="lblCDate" runat="server" Text='<%#Bind("CERTIFICATE_DATE") %>'></asp:Label>]
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Details">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDetailss" runat="server" Text='<%#Bind("TRAINING_DETAILS") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="42%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Certificate">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgBtnPdf" runat="server" ImageUrl="~/images/icons/pdf.png" Width="25px" CommandName="View" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnEdit" runat="server" ImageUrl="~/images/icons/edit.png" OnClick="ibtnEdit_Click" />
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; font-size: 20px;">
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

            </table>



            <table id="tblAddTrainig" runat="server" visible="false">
                <tr>
                    <td>
                        <asp:Button ID="btnBackAdd" runat="server" Width="100px" Text="Back" OnClick="btnBackAdd_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table class="training-table" style="width: 100%">
                            <tr>
                                <th colspan="3" style="text-decoration: underline">Add Training Information</th>
                            </tr>
                            <tr>
                                <td style="width: 33%;">
                                    <asp:Label ID="lblPK_ID" runat="server" Visible="true" Text="label"></asp:Label>
                                    <label for="txtEmpId">Emp Id:</label><br />
                                    <asp:TextBox ID="txtEmpId" runat="server" Enabled="false" />
                                </td>
                                <td colspan="2" style="width: 67%;">
                                    <label for="txtEmpName">Emp Name:</label><br />
                                    <asp:TextBox ID="txtEmpName" runat="server" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkInhouse" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="chkInhouse_CheckedChanged" />In House
                                            </td>
                                        </tr>
                                        <tr id="trTrName" runat="server" visible="false">
                                            <td>
                                                <label for="txtOrgName">Training Center/ Org Name:</label><br />
                                                <asp:TextBox ID="txtOrgName" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                            </tr>
                            <tr>
                                <td colspan="2">
                                    <label for="txtTrainingTitle">Training Title:</label>*<br />
                                    <asp:TextBox ID="txtTrainingTitle" runat="server" />
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <label for="txtTrainingDate">Training Date:</label>*<br />
                                                <asp:TextBox ID="txtTrainingDate" runat="server" />
                                            </td>
                                            <td>
                                                <label for="txtCertificateDate">Certificate Date:</label>*<br />
                                                <asp:TextBox ID="txtCertificateDate" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <label for="txtTrainingDetails">Training Details:</label><br />
                                    <asp:TextBox ID="txtTrainingDetails" runat="server" TextMode="MultiLine" Rows="5" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <label for="fileCertificate">Certificate:</label><br />
                                    <asp:FileUpload ID="fileCertificate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
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
            <asp:PostBackTrigger ControlID="bttnSearch" />
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnBack" />
            <asp:PostBackTrigger ControlID="btnAddMore" />
            <asp:PostBackTrigger ControlID="btnBackAdd" />
            <asp:PostBackTrigger ControlID="btnCancel" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
        window.onload = function () {
            // Initialize all datepickers with their respective formats
            document.getElementById("<%= txtTrainingDate.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
            document.getElementById("<%= txtCertificateDate.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });

        };
    </script>

</asp:Content>

