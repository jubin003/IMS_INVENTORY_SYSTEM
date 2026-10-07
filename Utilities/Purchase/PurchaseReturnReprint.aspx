<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PurchaseReturnReprint.aspx.cs" Inherits="Utilities_Purchase_PurchaseReturnReprint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function printPartOfPage() {
            var printContent = document.getElementById('bill_format_1');
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=0,top=0,width=0,height=0');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            // printWindow.close();
        }

    </script>
    <div class="container-fluid form-group-sm">
        <div class="row">
            <div class="col-md-1">
                Date Wise<br />
                <asp:CheckBox ID="chkDateWise" runat="server" OnCheckedChanged="chkDateWise_CheckedChanged" AutoPostBack="true" />
            </div>
            <div class="col-md-2" runat="server" visible="true" id="NoDate1">
                Fiscal Year<br />
                <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-2" runat="server" visible="true" id="NoDate2">
                Debit Note No<br />
                <asp:TextBox ID="txtDebitNoteNo" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-2" runat="server" visible="false" id="WithDate1">
                From Date<br />
                <asp:TextBox ID="txtFromDate" AutoComplete="off" runat="server" CssClass="form-control datepicker"></asp:TextBox>
            </div>
            <div class="col-md-2" runat="server" visible="false" id="WithDate2">
                To Date<br />
                <asp:TextBox ID="txtToDate" AutoComplete="off" runat="server" CssClass="form-control datepicker"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-primary" OnClick="btnShow_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:GridView ID="grdPurchaseReturn" runat="server" AutoGenerateColumns="False" CssClass="gridtable" Width="100%" OnRowCommand="grdPurchaseReturn_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Debit Note Detail">
                            <ItemTemplate>
                                <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Note Number</td>
                                        <td style="font-weight: 700;">:
                                            <asp:Label ID="lblDNNumber" runat="server" Text='<%# Bind("DEBIT_NOTE_NUMBER") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Note Date(BS)</td>
                                        <td>:
                                            <asp:Label ID="lblDNDateNP" runat="server" Text='<%# Bind("NOTE_DATE_EN") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Note Date(AD)</td>
                                        <td>:
                                            <asp:Label ID="lblDNDateEN" runat="server" Text='<%# Bind("NOTE_DATE_NP") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Dakhila Number</td>
                                        <td>: 
                                <asp:Label ID="lblDakhilaNumber" runat="server" Text='<%# Bind("DAKHILA_NUMBER") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Dakhila Date(BS)</td>
                                        <td>:
                                <asp:Label ID="lblDakhilaDateNP" runat="server" Text='<%# Bind("DAKHILA_DATE_NP") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Dakhila Date(AD)</td>
                                        <td>:
                                <asp:Label ID="lblDakhilaDateEN" runat="server" Text='<%# Bind("DAKHILA_DATE_EN") %>'></asp:Label></td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Detail">
                            <ItemTemplate>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Invoice Number</td>
                                        <td>:
                                <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# Bind("SUPPLIER_INVOICE_NO") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Invoice Date(BS)</td>
                                        <td>:
                                <asp:Label ID="lblInvoiceDateNP" runat="server" Text='<%# Bind("INVOICE_DATE_NP") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Invoice Date(AD)</td>
                                        <td>:
                                <asp:Label ID="lblInvoiceDateEN" runat="server" Text='<%# Bind("INVOICE_DATE_EN") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Supplier Name</td>
                                        <td>:  
                                <asp:Label ID="lblSupplierName" runat="server" Text='<%# Bind("SUPPLIER_NAME") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Address </td>
                                        <td>:  
                                <asp:Label ID="lblSupplierAddress" runat="server" Text='<%# Bind("SUPPLIER_ADDRESS") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Contact</td>
                                        <td>:  
                                <asp:Label ID="lblSupplierContact" runat="server" Text='<%# Bind("SUPPLIER_CONTACT") %>'></asp:Label></td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Return Amount Detail">
                            <ItemTemplate>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Total Amount</td>
                                        <td style="text-align: right">:<asp:Label ID="lblTotalAmount" runat="server" Text='<%# Bind("TOTAL_AMOUNT") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Discount Amount</td>
                                        <td style="text-align: right">:<asp:Label ID="lblDiscountAmount" runat="server" Text='<%# Bind("DISCOUNT_AMOUNT") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">VAT Amount</td>
                                        <td style="text-align: right">:<asp:Label ID="lblVATAmount" runat="server" Text='<%# Bind("TAX_VAT_AMOUNT") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Round off</td>
                                        <td style="text-align: right">:<asp:Label ID="lblRoundOff" runat="server" Text='<%# Bind("ROUND_OFF") %>'></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 120px; font-weight: 700;">Return Amount</td>
                                        <td style="text-align: right">:<asp:Label ID="lblReturnAmount" runat="server" Text='<%# Bind("RETURN_AMOUNT") %>'></asp:Label></td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("RETURN_REMARKS") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/view.png" CommandName="Alter" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>
    <div id="printdetail" runat="server" visible="false" style="margin-top: 1000px">
        <div id="print_div" style="width: 210mm; padding: 30px 30px 30px 30px; box-sizing: border-box; margin: auto;">
            <div id="bill_format_1" style="margin-bottom: 50px; box-sizing: border-box; padding: 5px;">
                <table style="width: 100%">
                    <tr>
                         <td style="width: 20%; text-align: center; vertical-align: top;">
                            <asp:Image ID="sImage1" runat="server" Width="100%" ImageUrl="~/images/img.png" Height="156px" />
                        </td>
                        <td style="text-align: center;">
                            <asp:Label ID="lblCompanyName" runat="server" Text="" Style="font-weight: bold; font-size: 32px;"></asp:Label>
                            <br style="display: block; content: ''; margin-top: 0;" />
                            <asp:Label ID="lblCompanyAddress" runat="server" Text="" Style="font-size: 18pt;"></asp:Label>
                            <br style="display: block; content: '';" />
                            <div runat="server" id="divEmail">
                                <asp:Label ID="lblWebsite" runat="server" Text="" Style="font-size: 12pt;"></asp:Label>
                                <asp:Label ID="lblDivider" runat="server" Text="|" Style="font-size: 12pt;"></asp:Label>
                                <asp:Label ID="lblEmail" runat="server" Text="" Style="font-size: 12pt;"></asp:Label>
                                <br style="display: block; content: ''; margin-top: 0;" />
                            </div>
                            <asp:Label ID="lblPhone" runat="server" Text="" Style="font-size: 12pt;"></asp:Label>
                            <br style="display: block; content: ''; margin-top: -5px;" />
                            <asp:Label ID="Label1" runat="server" Style="font-size: 12pt; font-weight: normal; font-family: Verdana;" Text="PAN No."></asp:Label>
                            <asp:Label ID="lblPanNo" runat="server" Style="font-size: 12pt; font-weight: normal; font-family: Verdana;"></asp:Label>
                        </td>
                        <td style="width: 15%; text-align: center; vertical-align: top;"></td>
                    </tr>
                    <tr style="padding-top: 10px; padding-bottom: 10px;">
                        <td style="text-align: center; font-weight: bold; font-family: Verdana; font-size: 26px" colspan="3">
                            <asp:Label ID="lblHeading" runat="server" Text="Debit Note"></asp:Label>
                            <br />
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; font-size: 13pt">
                    <tr>
                        <td>Debit Note No.</td>
                        <td>:                           
                            <asp:Label Font-Size="14pt" Font-Bold="true" ID="lblDebitNoteNo" runat="server"></asp:Label>
                        </td>
                        <td style="text-align: right">Debit Note Date 
                        </td>
                        <td>:<asp:Label ID="lblDNNepaliDate" runat="server"></asp:Label>
                            [<asp:Label ID="lblDNEnglishDate" runat="server"></asp:Label>]
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%;">Dakhila No</td>
                        <td style="width: 30%;">:
                            <asp:Label ID="lblDakhilaNo" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="width: 20%; text-align: right">Dakhila Date</td>
                        <td>:<asp:Label ID="lblDakhilaNepaliDate" runat="server"></asp:Label>
                            [<asp:Label ID="lblDakhilaEnglishiDate" runat="server"></asp:Label>]</td>
                    </tr>
                    <tr>
                        <td>Invoice Number</td>
                        <td>:
                            <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label>
                        </td>
                        <td style="text-align: right">Invoice Date</td>
                        <td>:<asp:Label ID="lblInvoiceNepaliDate" runat="server"></asp:Label>
                            [<asp:Label ID="lblInvoiceEnglishDate" runat="server"></asp:Label>]</td>
                    </tr>
                    <tr>
                        <td>Supplier Name</td>
                        <td colspan="3">:                  
                         <asp:Label ID="lblSupplierName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="">Address</td>
                        <td style="">:
                         <asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                        <td style="text-align: right" colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>PAN No</td>
                        <td>:
                         <asp:Label ID="lblSupplierPanNo" runat="server"></asp:Label></td>
                        <td style="text-align: right" colspan="2">Mode of Payment:
                            <asp:Label ID="lblModeofPayment" runat="server"></asp:Label></td>
                    </tr>
                </table>
                  <style>
                    .custom-grid {
                        border-collapse: collapse;
                        width: 100%;
                        font-family: Arial, sans-serif;
                        font-size: 12px;
                        border-left:1px solid #000;
                        border-right:1px solid #000;
                        border-bottom:1px solid #000;
                        border-top:1px solid #000;
                    }

                        .custom-grid th,
                        .custom-grid td {
                            padding: 4px;
                            vertical-align: top;
                            border-left: 1px solid #000; /* Vertical borders */
                            border-right: 1px solid #000;
                            border-top: none; /* Remove top border */
                            border-bottom: none; /* Remove bottom border by default */
                        }

                        /* Special bottom border for header */
                        .custom-grid th {
                            
                            
                            border-bottom: 1px solid #000; /* Add horizontal line under header */
                            border-top: 1px solid #000; /* Add horizontal line under header */
                        }

                           

                        .custom-grid label {
                            margin: 0;
                            padding: 0;
                            display: inline-block;
                            font-size: 18px;
                        }
                </style>

                <table style="width: 100%;">
                    <tr>
                        <td rowspan="4" style="height: 500px; vertical-align: top">
                            <asp:GridView ID="gridReturnInvoice" runat="server" CssClass="custom-grid"
                                AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.N">
                                        <ItemStyle Height="20px" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblSN" runat="server" Text='<%# Bind("SNO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Particular">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("PARTICULARS") %>' ID="lblProNAme" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemStyle HorizontalAlign="Center" Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("QTY") %>' ID="lblQuantity" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate">
                                        <HeaderTemplate>
                                            <asp:Label Text='Rate' ID="lblHRate" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("RATE") %>' ID="lblRate" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="70px" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderTemplate>
                                            Amount
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("TOTAL") %>' ID="lblTotal" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" Height="8%" Width="5%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>

                <table style="width: 100%; font-size: 12pt; border-collapse: collapse; border-top: 1px solid black;">
                    <tr>
                        <td style="width: 60%; font-size: 12pt; vertical-align: top;">
                            <table style="width: 100%">
                                <tr>
                                    <td>In words:
                            Rs.
                            <asp:Label ID="lblAmountInWord" runat="server" Style="font-size: 12pt"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top: 1px;">Remarks
                                        <br />
                                        <asp:Label ID="lblNoteRemarks" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </td>
                        <td style="width: 40%">
                            <table style="width: 100%; font-size: 12pt; border-collapse: collapse; border: none;">
                                <tr>
                                    <td style="padding-left: 3%">Total</td>
                                    <td style="text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblBillSubTotal" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="border-top: 1px solid black; padding-left: 3%">Discount
                                        <asp:Label ID="lblBillDiscountPercent" runat="server" Text=""></asp:Label>
                                        %</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblDiscount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top: 1px solid black; padding-left: 3%">Taxable amount</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblTaxableAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top: 1px solid black; padding-left: 3%">&nbsp;VAT
                                        <asp:Label ID="lblVATPercent" runat="server"></asp:Label>
                                        %</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblVATAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top: 1px solid black; padding-left: 3%">Total</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblGTotal" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top: 1px solid black; padding-left: 3%">Round off</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblRoundoff" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top: 1px solid black; padding-left: 3%">Total</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblBillAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; font-size: 12pt">

                    <tr>
                        <td>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblInvCreatedBy" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">Debit Note Created by</td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

</asp:Content>

