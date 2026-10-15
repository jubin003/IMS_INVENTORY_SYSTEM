<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Hr_Employee.aspx.cs" Inherits="HumanResource_Utilities_Hr_Employee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css" media="print">
        #printGrid {
            display: table-cell !important; /* Show the <td> in print */
        }

        }
    </style>   

    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table id="EmpSearch" width="100%" runat="server">
                <tr>
                    <td>
                        <table style="width: 40%">
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
                                    <asp:Button ID="btnAddEmployee" Width="120px" CssClass="btn btn-primary" runat="server" Text="Add Employee" OnClick="btnAddEmployee_Click"  />
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
                        <asp:GridView ID="GridEmpList" CssClass="gridtable" runat="server" Width="100%" AutoGenerateColumns="False" BackColor="White"
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
                                <asp:TemplateField HeaderText="POSITION">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeptId" runat="server" Text='<%# Bind("DEPARTMENTID") %>' Visible="False"></asp:Label>
                                        <asp:Label ID="lblDept" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Contact Detail">
                                    <ItemTemplate>
                                        <asp:Image ID="imgPhone" runat="server" ImageUrl="~/images/icons/phone.png"></asp:Image>:<asp:Label ID="lblMContactNo" runat="server" Text='<%# Bind("PHONE") %>'></asp:Label><br />
                                        <asp:Image ID="imgMobile" runat="server" ImageUrl="~/images/icons/mobile.png"></asp:Image>:<asp:Label ID="lblMobile" runat="server" Text='<%# Bind("MOBILE1") %>'></asp:Label><br />
                                        <asp:Image ID="imgEmail" runat="server" ImageUrl="~/images/icons/regdno.png"></asp:Image>:<asp:Label ID="Label1" runat="server" Text='<%# Bind("EMAIL") %>'></asp:Label><br />

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="EDIT">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgBttnEdit" runat="server" ImageUrl="~/images/icons/edit.png"
                                            OnClick="ImgBttnEdit_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="VIEW">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgBttnView" runat="server" ImageUrl="~/images/icons/view.png"
                                            OnClick="ImgBttnView_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>                
            </table>
            <table runat="server" id="print">
                <tr>
                    <td>
                        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnEmpHistory" runat="server" Text="View Emp History" OnClick="btnEmpHistory_Click" />
                    </td>
                    <td>
                        <asp:Button ID="bttnCancel" runat="server" OnClick="bttnCancel_Click" Text="Back" />
                    </td>
                </tr>
            </table>
            <br />
            <div id="div_print">

                <table id="DetailEmployee" border="1" runat="server" style="background-color: White; width: 200mm"
                    cellpadding="10" cellspacing="0">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <fieldset style="text-align: left; padding-left: 10px">
                                        <table style="width: 100%">
                                            <tr>
                                                <td colspan="4" style="text-align: center; height: 30px;">
                                                    <h1>Employee Information</h1>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px">Employee ID
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblEmployeeID" runat="server"></asp:Label>
                                                </td>
                                                <td rowspan="4" align="right">
                                                    <asp:Label ID="lblPhoto" runat="server" Visible="false"></asp:Label>
                                                    <asp:Image ID="ImgPhoto" runat="server" Width="100px" />
                                                    &nbsp;
                                                </td>
                                                <td align="right" rowspan="3">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Employee Name
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFullName" runat="server"></asp:Label>(<asp:Label ID="lblAbbrevation" runat="server"></asp:Label>)
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Name In Devnagari
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDevName" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset style="text-align: left; padding-left: 10px">
                                        <table style="width: 100%">
                                            <tr>
                                                <td colspan="4">
                                                    <b>Employee Details</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">Employee Name
                                                </td>
                                                <td style="width: 175px">
                                                    <asp:Label ID="lblFullName0" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 150px;">Name In Devnagari
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDevName0" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Father&#39;s Name
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFatherName" runat="server"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Grand Father&#39;s Name
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblGrandFather" runat="server"></asp:Label>
                                                </td>
                                                <td>Mother&#39;s Name
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMotherName" runat="server"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>Department
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDept" runat="server"></asp:Label>
                                                </td>
                                                <td>Designation
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDesig" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Level
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblLevel" runat="server"></asp:Label>
                                                </td>
                                                <td>Grade
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblGrade" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="tdDivision" runat="server">Division
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                                </td>
                                                <td id="tdUnit" runat="server">Unit
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblUnit" runat="server"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>Employee Type
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblEmpTyp" runat="server"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Job Title
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblJobTItle" runat="server"></asp:Label>
                                                </td>

                                                <td>Marital Status
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMaritalStatus" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Gender
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblGender" runat="server"></asp:Label>
                                                </td>
                                                <td>Blood Group
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBloodGroup" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Ethnicity
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblEthnicity" runat="server"></asp:Label>
                                                </td>
                                                <td>Religion
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblReligion" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <br />
                                    <fieldset style="text-align: left; padding-left: 10px">
                                        <table runat="server" width="100%">
                                            <tr>
                                                <td colspan="4">
                                                    <b>Permanent Address</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">Tole
                                                </td>
                                                <td style="width: 146px">
                                                    <asp:Label Style="width: 150px;" ID="lblTole" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 150px;">District
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDistrict" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">VDC/Municipality
                                                </td>
                                                <td style="width: 150px">
                                                    <asp:Label ID="lblVdc" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 150px;">Ward No.
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWard" runat="server"></asp:Label>
                                                </td>
                                            </tr>


                                        </table>
                                        <table id="contactTable" width="100%" runat="server">
                                            <tr>
                                                <td colspan="4">
                                                    <b>Contact Address</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">Tole
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lblCTole" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 150px;">District
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCDistrict" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">VDC/Municipality
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCMunicipality" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 150px;">Ward No.
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCWard" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td colspan="4">
                                                    <b>Contact Details</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">Email
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lblEmailID" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 150px;">Phone
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lblPhone" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">Mobile
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lblMobile" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 150px;">Birth Date
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lblBirthDate" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <br />
                                    <fieldset style="text-align: left; padding-left: 10px">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="4">
                                                    <b>Nominee Details </b>&nbsp; &nbsp; &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px">Nominee Name
                                                </td>
                                                <td style="width: 175px">
                                                    <asp:Label ID="lblNomineeName" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 150px;">Nominee Address
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblNomineeAddress" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Nominee Relation
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblNomineeRelation" runat="server"></asp:Label>
                                                </td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <br />
                                    <fieldset style="text-align: left; padding-left: 10px">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="4">
                                                    <b>Other Details</b> &nbsp; &nbsp; &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 46px">Appointment Date
                                                </td>
                                                <td style="height: 46px">
                                                    <asp:Label ID="lblAppointment" runat="server"></asp:Label>
                                                </td>

                                            </tr>

                                            <tr>
                                                <td style="width: 150px">PFID No.
                                                </td>
                                                <td style="width: 175px">
                                                    <asp:Label ID="lblPFIDNo" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 150px;">PFID Amount
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPFIDAmt" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Basic Salary
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBasicSalary" runat="server"></asp:Label>
                                                </td>
                                                <td>Actual Salary
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblActualSalary" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>CIT No.
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCITNo" runat="server"></asp:Label>
                                                </td>
                                                <td>CIT Amount
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCITAmt" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Primary Bank Name
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCurrentBank" runat="server"></asp:Label>
                                                </td>
                                                <td>Account No.
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblCurrentBankAcc" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Secondary Bank Name
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSecondBank" runat="server"></asp:Label>
                                                </td>
                                                <td>Account No.
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSecondBankAcc" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Remarks
                                                </td>
                                                <td colspan="3">
                                                    <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                                                    &nbsp; &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Status
                                                </td>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="chkStatus" runat="server" Text="Currently Inactive"
                                                        Enabled="False" />
                                                    &nbsp; &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>

                                    <fieldset>
                                        <table width="100%" runat="server" id="terminationTable">
                                            <tr>
                                                <td colspan="4">
                                                    <b>Termination Detail</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">Termination Date
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lblTerminationDate" runat="server"></asp:Label>
                                                    &nbsp; &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px;">Termination Reason
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lblTerRea" runat="server" />
                                                    &nbsp; &nbsp;
                                                </td>
                                                <td style="width: 150px;">Termination Descriptation
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label runat="server" ID="lblTerDes" />
                                                    &nbsp; &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table id="tblEmpHistory" style="width: 200mm; page-break-before: always;" runat="server" visible="false">
                    <tr>
                        <td>
                            <asp:GridView ID="grdEmpHistory" CssClass="gridtable" Width="100%" runat="server" AutoGenerateColumns="False"
                                OnRowDataBound="grdEmpHistory_RowDataBound">
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
                                    <asp:TemplateField HeaderText="Designation">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShowDesignationG" runat="server"></asp:Label>
                                            <asp:Label ID="lblDesignationG" runat="server" Text='<%#Bind("HR_DESIGNATION_ID") %>' Visible="false">></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShowDepartmentG" runat="server"></asp:Label>
                                            <asp:Label ID="lblDepartmentG" runat="server" Text='<%#Bind("HR_DEPARTMENT_ID") %>' Visible="false">></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Division">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShowDivisionG" runat="server"></asp:Label>
                                            <asp:Label ID="lblDivisionG" runat="server" Text='<%#Bind("HR_DIVISION_ID") %>' Visible="false">></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShowUnitG" runat="server"></asp:Label>
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

                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>


            <table runat="server" id="attach">
                <tr>
                    <td>
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
                                <asp:TemplateField HeaderText="Document Number" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmsgD" Visible="false" runat="server" Text="-"></asp:Label>

                                        <asp:Label ID="txtDocumentNum" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Issue Date" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="txtIssueDate" runat="server"></asp:Label>
                                        <asp:Label ID="lblmsgI" Visible="false" runat="server" Text="-"></asp:Label>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Expiry Date" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmsgE" Visible="false" runat="server" Text="-"></asp:Label>
                                        <asp:Label ID="txtExpiryDate" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Issued By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmsg" Visible="false" runat="server" Text="-"></asp:Label>
                                        <asp:Label ID="txtIssuedBy" runat="server"></asp:Label>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
