<%@ Control Language="C#" AutoEventWireup="true" CodeFile="voucher.ascx.cs" Inherits="Account_Utilities_uc_voucher" %>

<style type="text/css">
    .auto-style1 {
        height: 27px;
    }
</style>

<script type="text/javascript">
    function printPartOfPage() {
        var printContent = document.getElementById('divVoucher');
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

<div class="container-fluid">
    <table class="gridtable">       
        <tr>
            <td>Fiscal Year
            </td>
            <td>
                <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
            <td>From Date               
            </td>
            <td>
                <asp:TextBox ID="txtFromDate" AutoComplete="off" runat="server" CssClass="form-control datepicker"></asp:TextBox>

            </td>
            <td>To Date               
            </td>
            <td>
                <asp:TextBox ID="txtToDate" AutoComplete="off" runat="server" CssClass="form-control datepicker"></asp:TextBox>
            </td>
            <td>Status                
            </td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnList" runat="server" Text="List" OnClick="btnList_Click" CssClass="btn btn-primary" />
            </td>
            <td>
                <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CssClass="btn btn-primary" />
                <asp:Label ID="lblPK_id" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="10">
                <asp:GridView ID="grdVoucherList" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="gridtable" OnRowDataBound="grdVoucherList_RowDataBound" OnRowCommand="grdVoucherList_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPkid" runat="server" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Voucher No">
                            <ItemTemplate>
                                <asp:Label ID="lblVoucheNo" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Prepared By">
                            <ItemTemplate>
                                <asp:Label ID="lblPreparedBy" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Checked By">
                            <ItemTemplate>
                                <asp:Label ID="lblCheckedBy" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approved By">
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedBy" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="View">
                            <ItemTemplate>
                                <asp:Button ID="btnView" CommandName="View" runat="server" Text="View" CssClass="btn btn-sm btn-primary" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </td>

        </tr>
    </table>
    <div id="divHide" runat="server" visible="false">
        <asp:ImageButton ID="btnPrint" runat="server" Width="50px" ImageUrl="~/images/icons/print.png" OnClick="btnPrint_Click" />

        <br />
        <br />
        <table class="gridtable">
            <tr>
                <td>
                    <div id="divVoucher">
                        <div class="row" style="width: 210mm; border: 1px solid">
                            <div class="col-md-12">
                                <table style="width: 200mm;" border="0">
                                    <tr>
                                        <td colspan="3" style="text-align: center;" class="auto-style1">
                                            <strong>
                                                <asp:Label ID="lblCompanyName" runat="server" Style="font-size: 20px"></asp:Label>
                                            </strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: center;">
                                            <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: center;">
                                            <asp:Label ID="lblContact" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: center;">
                                            <b>
                                                <asp:Label ID="lblVoucherType" runat="server" Style="font-size: 20px"></asp:Label></b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;"><strong>Voucher No:</strong>
                                            <asp:Label ID="lblVoucherNo" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                        <td style="text-align: right;"><strong>Date:               
                                        </strong>
                                            <asp:Label ID="lblDate" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView ID="grdVoucherChild" runat="server" AutoGenerateColumns="False" CssClass="normalTable"
                                                ShowFooter="True" OnRowDataBound="grdVoucherChild_RowDataBound" Width="100%">
                                                <Columns>

                                                    <asp:TemplateField HeaderText="Particulars">
                                                        <FooterTemplate>
                                                            <strong>Total</strong>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("PARTICULARS") %>' ID="lblParticulars" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dr. Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblDrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("DR_AMOUNT") %>' ID="lblDrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="30mm" HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cr. Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblCrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("CR_AMOUNT") %>' ID="lblCrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="30mm" HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <strong>Amount in Words: </strong>
                                            <asp:Label ID="lblAmountsInword" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <hr />
                                            <strong>Narration: </strong>
                                            <asp:Label ID="lblNarration" runat="server" Text=""></asp:Label>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; width: 650mm;">Prepared By
                                        <br />
                                            <asp:Label ID="lblPreparedBy" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="text-align: center; width: 650mm;">
                                            <div id="divCheckedBy" runat="server">
                                                Checked By
                                         <br />
                                                <asp:Label ID="lblCheckedBy" runat="server" Text=""></asp:Label>
                                            </div>
                                        </td>
                                        <td style="text-align: center; width: 650mm;">Approved By
                                         <br />
                                            <asp:Label ID="lblApprovedBy" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; width: 650mm;">&nbsp;</td>
                                        <td style="text-align: center; width: 650mm;">&nbsp;</td>
                                        <td style="text-align: center; width: 650mm;">&nbsp;</td>
                                    </tr>
                                    <tr runat="server" visible="false" id="divAlternationNote">
                                        <td colspan="3">
                                            <strong>
                                                <asp:Label ID="LabelAlterationNote" runat="server" Text=""></asp:Label></strong>
                                            <asp:Label ID="lblAlterationNote" runat="server" Text=""></asp:Label><br />
                                            <strong>
                                                <asp:Label ID="LabelAlterationNoteRequest" runat="server" Text=""></asp:Label></strong>
                                            <asp:Label ID="lblAlterationNoteRequest" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </td>
                <td>
                    <asp:Image ID="imgViewVoucher" runat="server" />
                </td>
                <td style="vertical-align: top">
                    <table runat="server" style="width: 90mm" id="divCorrectVoucher" visible="false">
                        <tr>
                            <td>
                                <strong>Voucher Correction Note</strong>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtCorrectionNote" runat="server" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnCorrectVoucher" runat="server" Text="Save" OnClick="btnCorrectVoucher_Click" />
                            </td>
                        </tr>
                    </table>
                    <table runat="server" style="width: 90mm" id="divVoucherCancel" visible="false">
                        <tr>
                            <td class="auto-style1">
                                <strong>Voucher Cancel Note</strong>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtVoucherCancelNote" runat="server" TextMode="MultiLine" Width="100%" Height="100px" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnVoucherCancel" runat="server" Text="Save" OnClick="btnVoucherCancel_Click" CssClass="btn btn-primary" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
    </div>
    <table id="divBtns" runat="server" visible="false">
        <tr>
            <td>
                <asp:Button ID="btnCheck" runat="server" Text="Check Voucher" Visible="false" OnClick="btnCheck_Click" CssClass="btn btn-primary" />
            </td>
            <td>&nbsp;
                <asp:Button ID="btnApprove" runat="server" Text="Approve Voucher" Visible="false" OnClick="btnApprove_Click" CssClass="btn btn-primary" />
            </td>
            <td>&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel Voucher" Visible="false" OnClick="btnCancel_Click" CssClass="btn btn-primary" />
            </td>
            <td>&nbsp;
                <asp:Button ID="btnCorrect" runat="server" Text="Correct Voucher" Visible="false" OnClick="btnCorrect_Click" CssClass="btn btn-primary" />
            </td>
            <td>&nbsp;
                <asp:Button ID="btnAlter" runat="server" Text="Alter" Visible="false" OnClick="btnAlter_Click" CssClass="btn btn-primary" /></td>
            <td>
                <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" CssClass="btn btn-primary" />
            </td>

        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>

    </table>


    <table style="border: 1px solid; width: 100%" runat="server" id="divVoucherEntry" visible="false" class="gridtable">
        <tr>
            <td>Voucher Date:                   
             <asp:TextBox ID="txtVoucherDate" runat="server" Width="150px" CssClass="form-control" ReadOnly="false"></asp:TextBox>
                <asp:Label ID="lblVoucherPK_ID" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="grdVoucher" runat="server" AutoGenerateColumns="False"
                    OnRowDataBound="grdVoucher_RowDataBound" ShowFooter="True"
                    OnRowCommand="grdVoucher_RowCommand" Width="100%" CssClass="normalTable">
                    <Columns>
                        <asp:TemplateField HeaderText="Particulars">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <%--<asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPkid" runat="server" Visible="false" />s--%>
                                            <asp:DropDownList ID="ddlByTo" runat="server" AutoPostBack="True" Width="65px" CssClass="form-control">
                                                <asp:ListItem Value="By">By</asp:ListItem>
                                                <asp:ListItem Value="To">To</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGLCode" runat="server" AutoPostBack="True" OnTextChanged="txtAccNo_TextChanged"
                                                Text='<%# Bind("GL_CODE") %>' Width="80px" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGLAccount" runat="server" Width="350px" CssClass="form-control chosen-select"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlGLAccount_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <script>
                                                $(document).ready(function () {
                                                    $('.chosen-select').chosen();
                                                });
                                            </script>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSGLAccount" runat="server" Width="350px" CssClass="form-control chosen-select"
                                                AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlSGLAccount_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <script>
                                                $(document).ready(function () {
                                                    $('.chosen-select').chosen();
                                                });
                                            </script>
                                            <asp:Label ID="lblSGLCode" runat="server" Text='<%# Bind("SGL_CODE") %>' Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="Label1" runat="server" Text="Total:"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Debit">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDebit" runat="server" Text='<% #Bind("DEBIT") %>' OnTextChanged="txtDebit_TextChanged"
                                    CssClass="form-control" Width="100px" AutoPostBack="True" Style="text-align: right;"></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtTotalDebit" runat="server" ReadOnly="True" CssClass="form-control" Width="100px" Style="text-align: right;"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCredit" runat="server" Text='<% #Bind("CREDIT") %>' OnTextChanged="txtCredit_TextChanged"
                                    CssClass="form-control" Width="100px" AutoPostBack="True" Style="text-align: right;"></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtTotalCredit" runat="server" ReadOnly="True" CssClass="form-control" Width="100px" Style="text-align: right;"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnAddRow" CommandName="Add" runat="server" Text="+" Width="40px" CssClass="btn btn-primary" />
                                <asp:Button ID="btnRemoveRow" CommandName="Remove" runat="server" Text="-" Width="40px" CssClass="btn btn-primary" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="Label2" runat="server" Text="Narration"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtNarration" runat="server" TextMode="MultiLine" Width="100%" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:FileUpload ID="voucherUpload" runat="server" />
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </td>

        </tr>
        <tr>
            <td style="text-align: center" colspan="2">
                <asp:Button ID="btnSaveVoucher" runat="server" Text="Save" Width="100px" OnClick="btnSaveVoucher_Click" CssClass="btn btn-primary" />
                <asp:Button ID="btnUpdate" runat="server" Text="Update" Width="100px" Visible="false" OnClick="btnUpdate_Click" CssClass="btn btn-primary" />
                <asp:Button ID="btnCancelVoucherEntry" runat="server" Text="Cancel" Width="100px" OnClick="btnCancelVoucherEntry_Click" CssClass="btn btn-primary" /></td>
        </tr>
    </table>
    <table runat="server" id="divVoucherImage" visible="false">

        <tr>
            <td>
                <div style="margin-left: 20px; width: 210mm; height: 120mm; position: relative;">
                    <asp:Label ID="lblPk_idPDF" runat="server" Visible="false"></asp:Label>
                    <asp:Image ID="voucherImage" runat="server"
                        Style="width: 110mm; height: 100%; object-fit: cover; position: absolute;" />
                    <asp:ImageButton Height="30px" ImageUrl="~/images/icons/pdf.png"
                        ID="btnPdf" runat="server" Visible="false" OnClick="btnPdf_Click" />
                </div>


            </td>
        </tr>
    </table>
</div>


