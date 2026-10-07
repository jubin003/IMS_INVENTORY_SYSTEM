<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="entryforms_Register" %>

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
                            <td>Opening FY</td>
                            <td>
                                <asp:TextBox ID="txtOpeningFiscalYear" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>Base URL</td>
                            <td>
                                <asp:TextBox ID="txtBaseURL" runat="server" CssClass="form-control"></asp:TextBox>
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
                            <td>TAX Type</td>
                            <td>
                                <asp:DropDownList ID="ddlTaxType" runat="server" CssClass="form-control" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlTaxType_SelectedIndexChanged">
                                    <asp:ListItem>VAT</asp:ListItem>
                                    <asp:ListItem>PAN</asp:ListItem>
                                    <asp:ListItem>HST</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>TAX Percent</td>
                            <td>
                                <asp:TextBox ID="txtTAXPercent" runat="server" CssClass="form-control"></asp:TextBox></td>
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
                            
                            <td>CBMS PUSH</td>
                            <td>
                                <asp:RadioButtonList ID="rbtnCBMS" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True">OFF</asp:ListItem>
                                    <asp:ListItem>ON</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                              <td>CBMS Approved Date</td>
                            <td>
                                <asp:TextBox ID="txtCBMSApprovedDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY" ></asp:TextBox>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>CBMS URL</td>
                            <td>
                                <asp:TextBox ID="txtCBMSURL" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>

                            <td>Server IP
                            </td>
                            <td>
                                <asp:TextBox ID="txtResetIP" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
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
                                    <asp:ListItem Selected="True">DISABLE</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;&nbsp;</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>SEND URL</td>
                            <td>
                                <asp:TextBox ID="txtSMSSend" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>GET URL</td>
                            <td>
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
                <asp:Button ID="btnAdd" runat="server" Text="Update" OnClick="btnAdd_Click" Width="100px" CssClass="btn btn-success" />
            </div>
        </div>
    </div>


    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#main-privacy>.privacy-container > .container-heading > .container-collapse').html('&#9660');
            $('#main-privacy>.privacy-container > .container-body').slideUp('fast');
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
</asp:Content>

