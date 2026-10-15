<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Create_Employee.aspx.cs" Inherits="hr_MasterData_Create_Employee" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updGrdWrkSchedule" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <style>
                .blue-radio input[type="radio"] {
                    accent-color: #474ef6; /* Modern browsers */
                }

                /* Fallback for older browsers if needed */
                .blue-radio label {
                    color: black; /* Optional label color */
                }
            </style>

            <script type="text/javascript">
                window.onload = function () {
                    // Initialize Nepali Date Picker on multiple elements
                    var elements = ["<%= txtDateBs.ClientID %>", "<%= txtAppointDate.ClientID %>"];
                    elements.forEach(function (id) {
                        var element = document.getElementById(id);
                        if (element) {
                            element.nepaliDatePicker();
                        }
                    });
                };
            </script>

            <br />

            <asp:Panel ID="Panel1" runat="server">
                <cc1:TabContainer ID="EmployeeDetailContainer" align="left" runat="server" ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" HeaderText="Employee Information" ID="EmployeePanel">
                        <HeaderTemplate>
                            Employees Information
                        </HeaderTemplate>
                        <ContentTemplate>
                            <fieldset>
                                <table runat="server" width="100%" cellpadding="8" cellspacing="0" style="border-collapse: collapse; font-family: Arial, sans-serif; font-size: 14px;">
                                    <tr style="background-color: #f2f2f2;">
                                        <th colspan="4" style="text-align: left; font-size: 16px; padding: 10px;">Employee Details</th>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%">Employee ID No:<br />
                                            <asp:TextBox ID="txtIdNo" runat="server" Width="100%" Enabled="False" AutoPostBack="True" CssClass="form-control" OnTextChanged="txtIdNo_TextChanged"></asp:TextBox>
                                        </td>
                                        <td style="width: 25%">First Name:<br />
                                            <asp:TextBox ID="txtFirstname" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td style="width: 25%">Middle Name:<br />
                                            <asp:TextBox ID="txtMiddleName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td style="width: 25%">Last Name:<br />
                                            <asp:TextBox ID="txtLastname" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Abbreviation:<br />
                                            <asp:TextBox ID="txtAbbrevation" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Name in Devnagiri:<br />
                                            <asp:TextBox ID="txtDevName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>Gender:<br />
                                            <asp:RadioButtonList ID="rbtnGender" CssClass="blue-radio" Width="50%" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="M">Male</asp:ListItem>
                                                <asp:ListItem Value="F">Female</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>Marital Status:<br />
                                            <asp:RadioButtonList ID="rbtnMaritalStatus" CssClass="blue-radio" Width="50%" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True">Unmarried</asp:ListItem>
                                                <asp:ListItem>Married</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Phone No:<br />
                                            <asp:TextBox ID="txtPhone" CssClass="form-control" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Mobile No (1):<br />
                                            <asp:TextBox ID="txtMobile1" CssClass="form-control" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="M1Validation" runat="server"
                                                ControlToValidate="txtMobile1" ErrorMessage="Enter 10-digit number" ValidationExpression="[0-9]{10}" ForeColor="Red" Display="Dynamic" />
                                        </td>
                                        <td>Mobile No (2):<br />
                                            <asp:TextBox ID="txtMobile2" CssClass="form-control" runat="server"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                ControlToValidate="txtMobile2" ErrorMessage="Enter 10-digit number" ValidationExpression="[0-9]{10}" ForeColor="Red" Display="Dynamic" />
                                        </td>
                                        <td>Email:<br />
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="ReExl"
                                                runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid email format" ForeColor="Red"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Birth Date (BS):<br />
                                            <asp:TextBox ID="txtDateBs" Width="200px" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                            <asp:Button ID="BtnCalAD" runat="server" Width="100px" CssClass="btn btn-primary btn-sm mt-1" Text="Convert" OnClick="BtnCalAD_Click" />
                                        </td>
                                        <td>Birth Date (AD):<br />
                                            <asp:TextBox ID="txtDateAd" runat="server" CssClass="form-control" Width="200px"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateAd" Enabled="True" Format="dd/MM/yyyy" />
                                            <asp:Button ID="BtnCalBS" runat="server" Width="100px" CssClass="btn btn-primary btn-sm mt-1" Text="Convert" OnClick="BtnCalBS_Click" />
                                        </td>
                                        <td>Appointment Date (BS):<br />
                                            <asp:TextBox ID="txtAppointDate" runat="server" CssClass="form-control datepicker" Width="100%"></asp:TextBox>
                                        </td>
                                        <td>Probation Period:<br />
                                            <asp:TextBox ID="txtProbDate" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Father's Name:<br />
                                            <asp:TextBox ID="txtFatherName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>Mother's Name:<br />
                                            <asp:TextBox ID="txtMotherName" CssClass="form-control" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Grandfather's Name:<br />
                                            <asp:TextBox ID="txtGrandFName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td id="lblSpouse" runat="server">Spouse's Name:<br />
                                            <asp:TextBox ID="txtSpouseName" CssClass="form-control" runat="server"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>Department:<br />
                                                        <asp:DropDownList ID="ddlDept" CssClass="form-control" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>
                                                    <td style="width: 50%">Division:<br />
                                                        <asp:DropDownList AutoPostBack="True" CssClass="form-control" runat="server" Width="100%" ID="ddlDivision" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>

                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 50%">Unit:<br />
                                                        <asp:DropDownList AutoPostBack="True" CssClass="form-control" runat="server" Width="100%" ID="ddlUnit"></asp:DropDownList>
                                                    </td>
                                                    <td style="width: 50%">Designation:<br />
                                                        <asp:DropDownList ID="ddlDesignation" CssClass="form-control" Width="100%" runat="server"></asp:DropDownList>
                                                    </td>

                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 50%">Level<br />
                                                        <asp:DropDownList ID="ddlLevel" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td style="width: 50%">Grade:<br />
                                                        <asp:DropDownList ID="ddlGrade" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>
                                        <td>Job Title:<br />
                                            <asp:TextBox ID="txtJobTitle" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>Employee Type:<br />
                                            <asp:DropDownList ID="ddlEmployeeType" CssClass="form-control" Width="100%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEmployeeType_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td id="txtConStDate" runat="server" visible="False">Start Date:<br />
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control datepicker" Width="100%"></asp:TextBox>
                                        </td>
                                        <td id="txtConEndDate" runat="server" visible="False">End Date:<br />
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control datepicker" Width="100%"></asp:TextBox>
                                        </td>
                                        <td>Differently Abled:<br />
                                            <asp:RadioButtonList ID="rbtnDifAble" CssClass="blue-radio" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:TextBox ID="txtDisRemark" runat="server" CssClass="form-control mt-1" Visible="False" Width="100%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Ethnicity:<br />
                                            <asp:DropDownList ID="ddlEthnicity" CssClass="form-control" Width="100%" runat="server"></asp:DropDownList>
                                        </td>
                                        <td>Religion:<br />
                                            <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Hinduism" Value="Hinduism" />
                                                <asp:ListItem Text="Buddhism" Value="Buddhism" />
                                                <asp:ListItem Text="Christianity" Value="Christianity" />
                                                <asp:ListItem Text="Islam" Value="Islam" />
                                                <asp:ListItem Text="Sikhism" Value="Sikhism" />
                                                <asp:ListItem Text="Atheist" Value="Atheist" />
                                                <asp:ListItem Text="Other" Value="Other" />
                                            </asp:DropDownList>
                                        </td>
                                        <td>Blood Group:<br />
                                            <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="A+" Value="A+" />
                                                <asp:ListItem Text="A-" Value="A-" />
                                                <asp:ListItem Text="B+" Value="B+" />
                                                <asp:ListItem Text="B-" Value="B-" />
                                                <asp:ListItem Text="AB+" Value="AB+" />
                                                <asp:ListItem Text="AB-" Value="AB-" />
                                                <asp:ListItem Text="O+" Value="O+" />
                                                <asp:ListItem Text="O-" Value="O-" />
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                </table>

                            </fieldset>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel2" HeaderText="Contact Details" runat="server">
                        <HeaderTemplate>
                            Address
                        </HeaderTemplate>
                        <ContentTemplate>
                            <fieldset>
                                <table runat="server" width="100%" cellpadding="8" cellspacing="0" style="border-collapse: collapse; font-family: Arial, sans-serif; font-size: 14px;">
                                    <tr style="background-color: #f2f2f2;">
                                        <th colspan="4" style="text-align: left; font-size: 16px; padding: 10px;">Permanent Address:</th>
                                    </tr>
                                    <tr>

                                        <td runat="server" id="lblDistrict_PA">District:
					<br />
                                            <asp:DropDownList ID="ddlPA_DISTRICT" Width="100%" AutoPostBack="True" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlPA_DISTRICT_SelectedIndexChanged"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>VDC/Municipality:
					<asp:DropDownList ID="ddlPAVDC" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPAVDC_SelectedIndexChanged"></asp:DropDownList></td>
                                        <td>Ward No:
			 <asp:TextBox ID="txtPA_WARD_NO" Width="100%" CssClass="form-control" AutoPostBack="True" TextMode="Number" runat="server" OnTextChanged="txtPA_WARD_NO_TextChanged"></asp:TextBox></td>
                                        <td>Street Name:
			 <asp:TextBox ID="txtPA_STREET" Width="100%" CssClass="form-control" AutoPostBack="True" runat="server" OnTextChanged="txtPA_STREET_TextChanged"></asp:TextBox></td>
                                        <td>House No:
			 <asp:TextBox ID="txtPA_HOUSE_NO" Width="100%" CssClass="form-control" AutoPostBack="True" runat="server" OnTextChanged="txtPA_HOUSE_NO_TextChanged"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="lblSameAsOf" runat="server">Contact Address Same as of &nbsp<asp:DropDownList ID="ddlContactAddress" runat="server" Width="50%" CssClass="form-control"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlContactAddress_SelectedIndexChanged">
                                            <asp:ListItem Text="Select" Value="Select" />
                                            <asp:ListItem Text="Permanent Address" Value="Parmenent Address" />
                                        </asp:DropDownList>

                                        </td>
                                    </tr>
                                    <tr>

                                        <td runat="server" id="lblDistrict_CA">District:
					<br />
                                            <asp:DropDownList ID="ddlTA_DISTRICT" AutoPostBack="True" Width="100%" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlTA_DISTRICT_SelectedIndexChanged"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>VDC/Municipality:
										<asp:DropDownList ID="ddlTAVDC" runat="server"></asp:DropDownList>
                                        </td>
                                        <td>Ward No:
			 <asp:TextBox ID="txtTA_WARD_NO" Width="100%" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox></td>
                                        <td>Street Name:
			 <asp:TextBox ID="txtTA_STREET" Width="100%" CssClass="form-control" runat="server"></asp:TextBox></td>
                                        <td>House No:
			 <asp:TextBox ID="txtTA_HOUSE_NO" Width="100%" CssClass="form-control" runat="server"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </fieldset>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel3" HeaderText="Nominee Details" runat="server">
                        <HeaderTemplate>
                            Nominee Details                                   
                        </HeaderTemplate>
                        <ContentTemplate>
                            <fieldset>
                                <table style="width: 100%">
                                    <tr>
                                        <td>Nominee Name:
						 <br />
                                            <asp:TextBox ID="txtNomineeName" Width="100%" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>

                                        </td>
                                        <td>Nominee Address:
						 <br />
                                            <asp:TextBox ID="txtNomineeAdd" Width="100%" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>

                                        </td>
                                        <td>Nominee Relation:
						 <br />
                                            <asp:TextBox ID="txtNomineeRelation" Width="100%" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>

                                        </td>
                                        <td>Contact No:
						 <br />
                                            <asp:TextBox ID="txtNomineeContact" Width="100%" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>

                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel1" HeaderText="Accidental Contact Details" runat="server">
                        <HeaderTemplate>
                            Accidental Contact Details                                   
                        </HeaderTemplate>
                        <ContentTemplate>
                            <fieldset>
                                <table style="width: 100%">
                                    <tr>
                                        <td>Name:
						 <br />
                                            <asp:TextBox ID="txtAcdName" Width="100%" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>

                                        </td>
                                        <td>Address:
						 <br />
                                            <asp:TextBox ID="txtAcdAddr" Width="100%" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>

                                        </td>
                                        <td>Relation:
						 <br />
                                            <asp:TextBox ID="txtAcdRelation" Width="100%" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>

                                        </td>
                                        <td>Contact No:
						 <br />
                                            <asp:TextBox ID="txtAcdContact" Width="100%" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>

                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tabpanel5" runat="server" HeaderText="Account Details">
                        <ContentTemplate>
                            <fieldset>
                                <table>
                                    <tr>
                                        <td>Primary Bank:	
                                                <br />
                                            <asp:DropDownList ID="ddlBankName" Width="300px" CssClass="form-control" Style="display: inline-block" runat="server">
                                            </asp:DropDownList></td>
                                        <td>Primary Bank Ac No:
                                                <br />
                                            <asp:TextBox TextMode="Number" ID="txtCurrentbankaccount" Width="300px" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>
                                        </td>
                                        <td>Secondary Bank:
			                                    <br />
                                            <asp:DropDownList ID="ddBankName2" Width="300px" CssClass="form-control" Style="display: inline-block" runat="server">
                                            </asp:DropDownList></td>
                                        <td>Secondary Bank Ac No:
						                        <br />
                                            <asp:TextBox TextMode="Number" ID="txtSecondbankaccount" Width="300px" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>
                                        </td>
                                      
                                    </tr>

                                    <tr>
                                        <td>SSF No:
						 <br />
                                            <asp:TextBox ID="txtSSFNo" Width="300px" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox></td>
                                        <td>PFID No:
						 <br />
                                            <asp:TextBox ID="txtPfidno" Width="300px" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>

                                        </td>
                                        <td>PAN No:
						 <br />
                                            <asp:TextBox ID="txtPANNo" Width="300px" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>

                                        </td>
                                        <td>CIT No:
						 <br />
                                            <asp:TextBox ID="txtCitno" Width="300px" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox></td>
                                           <td>Include in Payroll:
						                        <br />
                                             <asp:CheckBox ID="ChckIsPayroll" runat="server"  Style="display: inline-block" />
                                        </td>
                                    </tr>


                                    <tr>
                                        <td colspan="2" runat="server" visible="false"><b>Payable  Accounts</b>

                                            <asp:CheckBoxList ID="chkPay" runat="server">
                                            </asp:CheckBoxList>
                                        </td>
                                        <td colspan="1" style="vertical-align: top" runat="server" visible="false"><b>Advance Payment Account</b>
                                            <asp:CheckBoxList ID="chkAdvance" runat="server">
                                            </asp:CheckBoxList>
                                        </td>
                                        <td>
                                            <label class="label col4" runat="server" visible="False">
                                                PFID Amount:						
                                                    <br />
                                                <asp:TextBox ID="txtPfidAmt" Width="100%" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>
                                            </label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <caption>
                                                <label id="paytype_label" runat="server" class="label col4" visible="false">
                                                    Pay Type:
                                            <br />
                                                    <asp:DropDownList ID="ddlPayType" runat="server" CssClass="form-control" Style="display: inline-block" Width="100%">
                                                        <asp:ListItem>Select</asp:ListItem>
                                                        <asp:ListItem>Hourly Basis</asp:ListItem>
                                                        <asp:ListItem>Package Wise</asp:ListItem>
                                                    </asp:DropDownList>
                                                </label>
                                                <label id="basicsalary_label" runat="server" class="label col4" visible="False">
                                                    Basic Salary:
                                            <br />
                                                    <asp:TextBox ID="txtBasicSal" runat="server" CssClass="form-control" Style="display: inline-block" Width="100%"></asp:TextBox>
                                                </label>
                                                <label id="beneficiaryAmt_label" runat="server" class="label col4" visible="False">
                                                    Beneficiary Amount:
                                            <br />
                                                    <asp:TextBox ID="txtBeneficiaryAmt" runat="server" CssClass="form-control" Style="display: inline-block" Width="100%"></asp:TextBox>
                                                </label>
                                            </caption>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td runat="server" visible="False">CIT Amount:
						                            <br />
                                            <asp:TextBox ID="txtCitPer" Width="100%" CssClass="form-control" Style="display: inline-block" runat="server"></asp:TextBox>
                                        </td>

                                    </tr>
                                </table>
                            </fieldset>

                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel4" HeaderText="Other Details" runat="server">
                        <HeaderTemplate>
                            Other Details  
                        </HeaderTemplate>
                        <ContentTemplate>
                            <fieldset>
                                <table style="width: 100%">
                                    <tr>
                                        <td runat="server" style="height: 20px">Photo:<br />
                                            <asp:Label ID="lblHistory_Id" runat="server"></asp:Label>
                                            <asp:FileUpload ID="ImageUpload"  runat="server" Height="22px" onchange="previewImage(this)" />
                                        </td>
                                        <td>
                                            <asp:Image ID="imgPreview" runat="server" alt="Preview"
                                                Style="max-width: 100px; max-height: 100px; display: none;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" runat="server" style="height: 20px">Offer Letter:<br />
                                            <asp:FileUpload ID="imgOfferLetter"  runat="server" Height="22px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" runat="server" style="height: 20px">Agreement:<br />
                                            <asp:FileUpload ID="imgAgreement"  runat="server" Height="22px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" id="tdContract" runat="server" visible="false">Contract:
                                            <br />
                                            <asp:FileUpload ID="imgContract" runat="server" Height="22px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Remarks:
						                        <br />
                                            <asp:TextBox ID="txtRemarks" Width="100%" CssClass="form-control" TextMode="MultiLine" Style="display: inline-block" runat="server" Height="92px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:GridView ID="grdAttachment" runat="server" Width="100%" CssClass="gridtable"
                                                OnRowDataBound="grdAttachment_RowDataBound" AutoGenerateColumns="False">
                                                <AlternatingRowStyle BackColor="#EEE6FF" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SN">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                                            <asp:Label ID="lblPkId" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Attachment Document">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAttDocOf" runat="server" Text='<%# Bind("ATTACHMENT_DOC") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblAttDoc" runat="server" Text='<%# Bind("ATTACHMENT_DOC") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmsgD" Visible="false" runat="server" Text="-"></asp:Label>

                                                            <asp:TextBox ID="txtDocumentNum" CssClass="uc_formcontrol ndp-nepali-calendar-div" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document Issue Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtIssueDate" CssClass="uc_formcontrol ndp-nepali-calendar-div" runat="server"></asp:TextBox>
                                                            <asp:Label ID="lblmsgI" Visible="false" runat="server" Text="-"></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document Expiry Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmsgE" Visible="false" runat="server" Text="-"></asp:Label>
                                                            <asp:TextBox ID="txtExpiryDate" CssClass="form-control datepicker" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document Issued By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmsg" Visible="false" runat="server" Text="-"></asp:Label>
                                                            <asp:TextBox ID="txtIssuedBy" CssClass="form-control" runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Upload File">
                                                        <ItemTemplate>
                                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnReview" runat="server" Text="View" ToolTip="View" CssClass="btn btn-primary" OnClick="btnReview_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </asp:Panel>

            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Button ID="btnSave" Width="150px" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary" Width="150px" Text="Reset" />
                    </td>
                    <td>
                        <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary" Width="150px" Text="Back" OnClick="btnBack_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnReset" />
            <asp:PostBackTrigger ControlID="btnBack" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
    function previewImage(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                var img = document.getElementById("<%= imgPreview.ClientID %>");
                img.src = e.target.result;
                img.style.display = "block"; // show the preview
            }

            reader.readAsDataURL(input.files[0]);
        }
    }
</script>

    <script type="text/javascript">
        window.onload = function () {
            // Initialize all datepickers with their respective formats
            document.getElementById("<%= txtDateBs.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
            document.getElementById("<%= txtProbDate.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
            document.getElementById("<%= txtAppointDate.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
            document.getElementById("<%= txtStartDate.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
            document.getElementById("<%= txtEndDate.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
        };
    </script>
</asp:Content>

