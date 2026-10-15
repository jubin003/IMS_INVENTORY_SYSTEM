<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HR_Contract_Renewal.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="HumanResource_Utilities_HR_Contract_Renewal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table id="EmpSearch" runat="server" style="width: 40%">
        <tr>
            <td>Department
            
                <asp:DropDownList ID="ddlDept" AutoPostBack="true" Width="200px" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged1">
                </asp:DropDownList>
            </td>
            <td>Designation:<br />
                <asp:DropDownList AutoPostBack="true" CssClass="uc_formcontrol" runat="server" Width="200px" ID="ddlDesignation"></asp:DropDownList>
            </td>
            <td>Status<br />

                <asp:DropDownList ID="ddlStatus" Width="150px" runat="server" CssClass="form-control">
                    <asp:ListItem Selected="True" Value="1">All</asp:ListItem>
                    <asp:ListItem Value="2">Ongoing</asp:ListItem>
                    <asp:ListItem Value="3">Nearly Expired</asp:ListItem>
                    <asp:ListItem Value="0">Expired</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <br />
                <asp:Button ID="bttnSearch" Width="80px" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="bttnSearch_Click" />
            </td>
        </tr>
    </table>
    <table id="tblGrid" style="width: 100%" runat="server" visible="false">
        <tr>
            <td>
                <asp:GridView ID="grdAllContract" Visible="true" CssClass="gridtable" Width="100%" runat="server" AutoGenerateColumns="False"
                    OnRowDataBound="grdAllContract_RowDataBound">
                    <Columns>

                        <asp:TemplateField HeaderText="S.no">
                            <ItemTemplate>
                                <asp:Label ID="lblSN_A" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EMP_ID">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpID_A" runat="server" Text='<%#Bind("EMP_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblName_A" runat="server" Text='<%#Bind("EMP_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Department/Designation">
                            <ItemTemplate>
                                <asp:Label ID="lblDept_A" runat="server" Text='<%#Bind("DEP_NAME") %>'></asp:Label>/<asp:Label ID="lblDesignation_A" runat="server" Text='<%#Bind("DESIGNATION_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Level">
                            <ItemTemplate>
                                <asp:Label ID="lblLevel_A" runat="server" Text='<%#Bind("LEVEL_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Date">
                            <ItemTemplate>
                                <asp:Label ID="lblNepStDate_A" runat="server" Text='<%#Bind("START_NEP_DATE") %>'></asp:Label><br />
                                [<asp:Label ID="lblStDate_A" runat="server" Text='<%#Bind("START_DATE_AD") %>'></asp:Label>]
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="End Date">
                            <ItemTemplate>
                                <asp:Label ID="lblNepEndDate_A" runat="server" Text='<%#Bind("END_NEP_DATE") %>'></asp:Label><br />
                                [<asp:Label ID="lblEndDate_A" runat="server" Text='<%#Bind("END_DATE_AD") %>'></asp:Label>]
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remaining Days">
                            <ItemTemplate>
                                <asp:Label ID="lblRemainingDate_A" runat="server" Text='<%#Bind("REMAINING_DAYS") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Renew">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnRenew" runat="server" ImageUrl="~/images/icons/contract.png" Width="28px" CommandName="View" OnClick="ibtnRenew_Click" />
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
                            <h1 style="color: #2c3e50; font-size: 24px; margin: 0; padding: 10px 0;">Employee Contract Information</h1>
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
                                                    <asp:Label ID="lblGradeE" runat="server" Visible="false"></asp:Label>
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
                                    <td colspan="3" style="padding: 10px 0; font-weight: 600; color: #495057; text-decoration: underline; font-size: 20px; text-align: center;">Current/Previous Contract</td>
                                </tr>
                                <tr>
                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Department</td>
                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                        <asp:Label ID="lblDepartment" Width="100px" runat="server"></asp:Label>
                                    </td>
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
                                        <asp:Label ID="lblLevel" Width="200px" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Start Date</td>
                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                        <asp:Label ID="lblStartDate" Width="150px" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">End Date</td>
                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                        <asp:Label ID="lblEndDate" Width="150px" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 12px 15px;" colspan="3">
                                        <asp:ImageButton ID="imgBtnPdf" runat="server" Width="50px" Height="50px" ImageUrl="~/images/icons/pdf.png" OnClick="imgBtnPdf_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 1%; vertical-align: top; border: 1px solid #e0e0e0"></td>
                        <td style="width: 50%; vertical-align: top;">
                            <table style="width: 100%;">
                                <tr>
                                    <td colspan="3" style="padding: 10px 0; font-weight: 600; color: #495057; text-decoration: underline; font-size: 20px; text-align: center;">Next Contract</td>
                                </tr>
                                <tr>
                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Department</td>
                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                        <asp:DropDownList ID="ddlDepartmentE" Width="200px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartmentE_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Designation</td>
                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                        <asp:DropDownList ID="ddlDesignationE" Width="200px" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Level</td>
                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                        <asp:DropDownList ID="ddlLevelE" Width="200px" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Start Date</td>
                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                        <asp:TextBox ID="txtStartDateE" Width="150px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">End Date</td>
                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                        <asp:TextBox ID="txtEndDateE" Width="150px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 12px 15px; font-weight: 600; color: #495057;">Contract</td>
                                    <td style="padding: 12px 0; color: #495057;">:</td>
                                    <td style="padding: 12px 15px; color: #2c3e50;">
                                        <asp:FileUpload ID="fileContract" runat="server" />
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
    <script type="text/javascript">
        window.onload = function () {
            // Initialize all datepickers with their respective formats
            document.getElementById("<%= txtStartDateE.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
            document.getElementById("<%= txtEndDateE.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });

        };
    </script>




</asp:Content>

