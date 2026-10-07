<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Company.aspx.cs" Inherits="entryforms_Company" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div id="main-privacy">
            <div class="privacy-container">
                <div class="container-heading">
                    Organizational Detail
                <div class="container-collapse">
                </div>

                </div>
                <div class="container-body">
                    <table style="width: 100%" class="gridtable">
                        <tr>
                            <td>Organization Code
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>Base URL</td>
                            <td>
                                <asp:TextBox ID="txtBaseURL" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Organization Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>Address
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Contact Detail
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactDetail" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>FAX
                            </td>
                            <td>
                                <asp:TextBox ID="txtFax" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Email
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>Website
                            </td>
                            <td>
                                <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>PAN_NO
                            </td>
                            <td>
                                <asp:TextBox ID="txtPanNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>Reg. No
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegno" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>TAX Type</td>
                            <td>
                                <asp:DropDownList ID="ddlTaxType" AutoPostBack="true" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlTaxType_SelectedIndexChanged">
                                    <asp:ListItem>None</asp:ListItem>
                                    <asp:ListItem>ET</asp:ListItem>
                                    <asp:ListItem>VAT</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>TAX Percent</td>
                            <td>
                                <asp:TextBox ID="txtTAXPercent" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>EXIM Code</td>
                            <td><asp:TextBox ID="txtEximCode" runat="server" CssClass="form-control"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Is Manufacturer
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdbtnManu" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Yes" Value="1" />
                                    <asp:ListItem Text="No" Value="0" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </div>


            </div>
            <div class="privacy-container">
                <div class="container-heading">
                    <b>Product Setting</b>
                    <div class="container-collapse">
                    </div>
                </div>
                <div class="container-body">
                    <table style="width: 100%" class="gridtable">


                        <tr>
                            <td>
                                <asp:CheckBox ID="chkDual" AutoPostBack="true" runat="server" OnCheckedChanged="chkDual_CheckedChanged" />&nbsp; Dual Quantity                           
                                
                            </td>
                            <td>
                                <asp:CheckBox ID="chkShowDual" runat="server" />&nbsp;Show Dual Quantity                           
                                
                            </td>
                            <td>
                                <asp:CheckBox ID="chkItemDis" runat="server" />&nbsp;Itemwise Discount                           
                                
                            </td>
                            <td>
                                <asp:CheckBox ID="chkRateM" AutoPostBack="true" runat="server" OnCheckedChanged="chkRateM_CheckedChanged" />&nbsp;Multiple Rate                          
                                
                            </td>
                            <td runat="server" visible="false" id="tdEditable">
                                <asp:CheckBox ID="chkEditable" runat="server" />&nbsp;Rate Editable                          
                                
                            </td>
                            <td>
                                <asp:CheckBox ID="chkOnlyStock" runat="server" />&nbsp;Sale Available Stock Only                          
                                
                            </td>
                            <td>
                                <asp:CheckBox ID="chkAVailQty" runat="server" />&nbsp;Show Available Quantity                      
                                
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkRoundOff" runat="server" />&nbsp;Rounding                     
                                
                            </td>

                            <td>
                                <asp:CheckBox ID="chkManU" runat="server" />&nbsp;Product Manufacturer                           
                                
                            </td>
                            <td>
                                <asp:CheckBox ID="chkShowPO" runat="server" />&nbsp;Show PO Number                           
                                
                            </td>
                            <td>
                                <asp:CheckBox ID="chkExpDate" runat="server" />&nbsp;Expiry Date                           
                                
                            </td>
                            <td>
                                <asp:CheckBox ID="chkBatNum" runat="server" />&nbsp;Batch Number                          
                                
                            </td>
                            <td>
                                <asp:CheckBox ID="chkProCol" runat="server" />&nbsp;Product Color</td>

                            <td>
                                <asp:CheckBox ID="chkProSize" runat="server" />&nbsp;Product Size                       
                                
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <div class="privacy-container">
                <div class="container-heading">
                    Invoice Detail
                <div class="container-collapse">
                </div>
                </div>
                <div class="container-body">
                    <table style="width: 100%" class="gridtable">
                        <tr>
                            <td>Invoice Type</td>
                            <td>
                                <asp:DropDownList ID="ddlInvoiceType" runat="server" CssClass="form-control">
                                    <%--   <asp:ListItem Value="A5L">A5L (8 X 5.5)</asp:ListItem>--%>
                                    <asp:ListItem Value="A5P">A5(5.5 X 8 ) Potrate</asp:ListItem>
                                    <asp:ListItem Value="A5C">A5C (5 X ...) Continious</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Invoice Header</td>
                            <td>
                                <asp:DropDownList ID="ddlInvoiceHeader" runat="server" CssClass="form-control">
                                    <asp:ListItem>DEMO</asp:ListItem>
                                    <asp:ListItem>ESTIMATE</asp:ListItem>
                                    <asp:ListItem>INVOICE</asp:ListItem>
                                    <asp:ListItem>TAX INVOICE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Invoice Prefix</td>
                            <td>
                                <asp:TextBox ID="txtInvoicePrefix" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>CBMS PUSH</td>
                            <td>
                                <asp:RadioButtonList ID="rbtnCBMS" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True">OFF</asp:ListItem>
                                    <asp:ListItem>ON</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Username</td>
                            <td>
                                <asp:TextBox ID="txtCBMSUsername" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>Password</td>
                            <td>
                                <asp:TextBox ID="txtCBMSPassword" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>CBMS URL</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtCBMSURL" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>CBMS Activate On</td>
                            <td>
                                <asp:TextBox ID="txtCBMS_ACtivation_Date" runat="server" CssClass="form-control"></asp:TextBox>

                            </td>
                            <td>System Installed On</td>
                            <td>
                                <asp:TextBox ID="txtSystem_installed_date" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Server IP
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtResetIP" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Expiry Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtExpiryDate" runat="server" placeholder="DD/MM/YYYY" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>Status
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbtnStatus" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True">On</asp:ListItem>
                                    <asp:ListItem>Off</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="privacy-container">
                <div class="container-heading">
                    SMS Detail
                <div class="container-collapse">
                </div>
                </div>
                <div class="container-body">
                    <table style="width: 100%" class="gridtable">
                        <tr>
                            <td>SMS</td>
                            <td>
                                <asp:DropDownList ID="ddlSMSStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem>ENABLE</asp:ListItem>
                                    <asp:ListItem>DISABLE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;&nbsp;</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>SEND URL</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtSMSSend" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>GET URL</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtSMSGet" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>SMS Username</td>
                            <td>
                                <asp:TextBox ID="txtSMSUsername" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>SMS Password</td>
                            <td>
                                <asp:TextBox ID="txtSMSPassword" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="privacy-container">
                <div class="container-heading">
                    Other Details
                <div class="container-collapse">
                </div>
                </div>
                <div class="container-body">
                    <table style="width: 100%" class="gridtable">
                        <tr>
                            <td>Version</td>
                            <td>
                                <asp:TextBox ID="txtVersion" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>Opening FY</td>
                            <td>
                                <asp:TextBox ID="txtOpeningFiscalYear" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Version Controll
                            </td>
                            <td>
                                <asp:TextBox ID="txtVersionControll" CssClass="form-control" runat="server"></asp:TextBox>
                            </td>
                            <td>Branch Status
                            </td>
                            <td>
                                <asp:RadioButtonList ID="RadioBranchStatus" CssClass="radio-primary" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioBranchStatus_SelectedIndexChanged">
                                    <asp:ListItem Text="Enable" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Disable" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="grdBranchDetail" runat="server" Width="100%" AutoGenerateColumns="False" OnRowCommand="grdBranchDetail_RowCommand" OnRowDataBound="grdBranchDetail_RowDataBound">
                                    <Columns>

                                        <asp:TemplateField HeaderText="S.no">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblSnG" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Office Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBrCODE" runat="server" CssClass="form-control" Text='<%# Bind("OFFICECODE") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Opening Fiscal Year">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBrFiscalYear" runat="server" CssClass="form-control" Text='<%# Bind("OPENING_FISCAL_YEAR") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBrName" runat="server" CssClass="form-control" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Address">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBrAddress" runat="server" CssClass="form-control" Text='<%# Bind("STREET") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                        <asp:TemplateField HeaderText="Invoice Prefix">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBrInvPrefix" runat="server" CssClass="form-control" Text='<%# Bind("INVOICE_PREFIX") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Purchase Prefix">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBrPURPrefix" runat="server" CssClass="form-control" Text='<%# Bind("PURCHASE_PREFIX") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                        <asp:TemplateField HeaderText="Purchase Retrun Prefix">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBrPRPrefix" runat="server" CssClass="form-control" Text='<%# Bind("PR_PREFIX") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sales Return Prefix">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBrSRPrefix" runat="server" CssClass="form-control" Text='<%# Bind("SR_PREFIX") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Phone">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBrPhone" runat="server" CssClass="form-control" Text='<%# Bind("PHONE_NO") %>' ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBrEmail" runat="server" CssClass="form-control" Text='<%# Bind("EMAIL") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div style="white-space: nowrap;">
                                                    <asp:Button ID="btnAddMore" runat="server" Text="+" CommandName="Add" Style="margin-right: 5px;" />
                                                    <asp:Button ID="btnRemove" runat="server" Text="-" CommandName="Remove" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                        </tr>
                    </table>
                </div>
            </div>

            <div class="privacy-container">
                <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CssClass="btn btn-primary" />
            </div>
        </div>
        <script type="text/javascript" src="js/jquery.min.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#main-privacy>.privacy-container > .container-heading > .container-collapse').html('&#9660');
                //$('#main-privacy>.privacy-container > .container-body').slideUp('fast');
                $('#main-privacy>.privacy-container > .container-heading > .container-collapse').click(function () {
                    $(this).parent().next().slideToggle();
                    if ($(this).parent().next().height() == 1) {
                        $(this).html('&#9650');
                    }
                    else {
                        $(this).html('&#9660');
                    }
                });
                if ($('#main-privacy>.privacy-container > .container-body-password').height() == 255) {
                    $('#main-privacy>.privacy-container > .container-body-password').slideUp('fast');
                }
                else {
                }
            });
        </script>
        <div>
            <asp:GridView ID="gridCusType" runat="server" AutoGenerateColumns="False" Visible="false" CssClass="gridtable">
                <Columns>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblPK" runat="server" Visible="false" Text='<%# Bind("PK_ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>


            <asp:GridView ID="gridProType" runat="server" AutoGenerateColumns="False" Visible="false" CssClass="gridtable">
                <Columns>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblPK" runat="server" Visible="false" Text='<%# Bind("PK_ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Show in Purchase">


                        <ItemTemplate>
                            <asp:Label ID="lblPuStatus" runat="server" Text='<%# Bind("SHOW_IN_PURCHASE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Show in Sales">

                        <ItemTemplate>
                            <asp:Label ID="lblSaStatus" runat="server" Text='<%# Bind("SHOW_IN_SALES") %>'></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
</asp:Content>

