<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="InvoiceReprint.aspx.cs" Inherits="Utilities_Sales_InvoiceReprint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .grid80mm {
            width: 100%;
            border-collapse: collapse;
            font-family: Consolas;
            font-size: 13px;
        }

            .grid80mm thead tr th {
                border-top: 1px solid #000;
                border-bottom: 1px solid #000;
                padding: 2px;
                text-align: left;
                border-left: none;
                border-right: none;
            }

            .grid80mm tbody tr td {
                padding: 2px;
                vertical-align: top;
                border: none;
            }

        .page-break {
            page-break-after: always;
            break-after: page;
        }
    </style>

    <script type="text/javascript">
        function printPartOfPage() {
            var printContent = document.getElementById('bill_format_80mm');
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=0,top=0,width=0,height=0');
            printWindow.document.write('<style>');
            printWindow.document.write('@page { size: 80mm auto; margin: 0mm; }');
            printWindow.document.write('body { margin: 0; padding: 5px; font-family: Consolas; font-size: 13px; font-weight: 100; }');
            printWindow.document.write('table { width: 100%; border-collapse: collapse; font-family: Consolas; font-size: 13px; }');
            printWindow.document.write('th { border-top: 1px solid #000 !important; border-bottom: 1px solid #000 !important; border-left: none !important; border-right: none !important; padding: 2px; }');
            printWindow.document.write('td { padding: 2px; border: none !important; }');
            printWindow.document.write('.td-border-top { border-top: 1px solid #000 !important; }');
            printWindow.document.write('.td-border-bottom { border-bottom: 1px solid #000 !important; }');
            printWindow.document.write('.grid80mm { width: 100%; border-collapse: collapse; font-family: Consolas; font-size: 13px; }');
            printWindow.document.write('.grid80mm thead tr th { border-top: 1px solid #000; border-bottom: 1px solid #000; padding: 2px; text-align: left; border-left: none; border-right: none; }');
            printWindow.document.write('.grid80mm tbody tr td { padding: 2px; vertical-align: top; border: none; }');
            printWindow.document.write('</style>');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
        }

        function printTOK() {
            var printContent = document.getElementById('bill_format_tok');
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'PrintTOK' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=0,top=0,width=0,height=0');
            printWindow.document.write('<style>');
            printWindow.document.write('@page { size: 80mm auto; margin: 0mm; }');
            printWindow.document.write('body { margin: 0; padding: 5px; font-family: Consolas; font-size: 13px; font-weight: 100; }');
            printWindow.document.write('table { width: 100%; border-collapse: collapse; font-family: Consolas; font-size: 13px; }');
            printWindow.document.write('th { border-top: 1px solid #000 !important; border-bottom: 1px solid #000 !important; border-left: none !important; border-right: none !important; padding: 2px; }');
            printWindow.document.write('td { padding: 2px; border: none !important; }');
            printWindow.document.write('.grid80mm { width: 100%; border-collapse: collapse; font-family: Consolas; font-size: 13px; }');
            printWindow.document.write('.grid80mm thead tr th { border-top: 1px solid #000; border-bottom: 1px solid #000; padding: 2px; text-align: left; border-left: none; border-right: none; }');
            printWindow.document.write('.grid80mm tbody tr td { padding: 2px; vertical-align: top; border: none; }');
            printWindow.document.write('</style>');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
        }

        function printA4Only() {
            var printContent = document.getElementById('bill_format_1');
            var printWindow = window.open('about:blank', 'PrintA4' + new Date().getTime(), 'left=0,top=0,width=800,height=600');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
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
                Invoice No<br />
                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control"></asp:TextBox>
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
            <div class="col-md-12" style="margin-bottom: 5em">
                <asp:GridView ID="grdSales" runat="server" AutoGenerateColumns="False" CssClass="gridtable" Width="100%" OnRowCommand="grdSales_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblInvoiceNo" runat="server" Text='<%# Bind("INVOICE_NUMBER") %>' OnClick="lblInvoiceNo_Click"></asp:LinkButton>
                                <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceDay" runat="server" Text='<%# Bind("INVOICE_DATE_NP") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CUSTOMER_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# Bind("SUB_TOTAL") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discount">
                            <ItemTemplate>
                                <asp:Label ID="lblDiscount" runat="server" Text='<%# Bind("TAXABLE_DISC_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sub Total">
                            <ItemTemplate>
                                <asp:Label ID="lblSubTotal" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VAT">
                            <ItemTemplate>
                                <asp:Label ID="lblVAT" runat="server" Text='<%# Bind("TAX_VAT_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Grand Total">
                            <ItemTemplate>
                                <asp:Label ID="lblGrandTotal" runat="server" Text='<%# Bind("GRAND_TOTAL") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Round off">
                            <ItemTemplate>
                                <asp:Label ID="lblRoundOff" runat="server" Text='<%# Bind("ROUND_OFF") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payable">
                            <ItemTemplate>
                                <asp:Label ID="lblPayable" runat="server" Text='<%# Bind("INVOICE_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sales Type">
                            <ItemTemplate>
                                <asp:Label ID="lblSalesType" runat="server" Text='<%# Bind("SALES_TYPE_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/print.gif" CommandName="Alter" ToolTip="Print Invoice" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KOT">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnTOK" runat="server" ImageUrl="~/images/icons/print.gif" CommandName="TOK" ToolTip="Print Invoice TOK" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>

    <div id="printdetail" runat="server" visible="false" style="margin-top: 1000px">
        <div id="print_div" style="width: 210mm; padding: 30px 30px 30px 30px; box-sizing: border-box; margin: auto;">
            <div id="bill_format_1" style="margin-bottom: 50px; box-sizing: border-box; padding: 5px; font-family: Calibri">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 20%; text-align: center; vertical-align: top;">
                            <asp:Image ID="sImage1" runat="server" Width="100%" ImageUrl="~/images/img.png" Height="156px" />
                        </td>
                        <td style="text-align: center;">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblCompanyName" runat="server" Text="" Style="font-weight: bold; font-size: 32px;"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblCompanyAddress" runat="server" Text="" Style="font-size: 18pt;"></asp:Label></td>
                                </tr>
                                <tr runat="server" id="divEmail">
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblWebsite" runat="server" Text="" Style="font-size: 8pt;"></asp:Label>
                                        <asp:Label ID="lblDivider" runat="server" Text="|" Style="font-size: 8pt;"></asp:Label>
                                        <asp:Label ID="lblEmail" runat="server" Text="" Style="font-size: 8pt;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblPhone1" runat="server" Text="" Style="font-size: 8pt;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; height: 20px">
                                        <asp:Label ID="Label1" runat="server" Style="font-size: 12pt; font-weight: normal; font-family: Verdana;" Text="PAN No."></asp:Label>
                                        <asp:Label ID="lblPanNo" runat="server" Style="font-size: 12pt; font-weight: normal; font-family: Verdana;"></asp:Label>

                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 20%; text-align: center; vertical-align: top;"></td>
                    </tr>

                    <tr style="padding-top: 8px; padding-bottom: 8px;">
                        <td style="text-align: center; font-weight: bold; font-family: Verdana; font-size: 18px" colspan="3">
                            <asp:Label ID="lblInvoiceHeading" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; font-size: 13pt">

                    <tr>
                        <td colspan="3"></td>
                        <td style="width: 40%; text-align: right">Tran. Date:<asp:Label ID="lblTranNepaliDate" runat="server"></asp:Label>
                            [<asp:Label ID="lblTranDate" runat="server"></asp:Label>]</td>
                    </tr>
                    <tr>
                        <td>Invoice No.</td>
                        <td>:
                            <asp:Label Font-Size="10pt" Font-Bold="true" ID="lblInvoiceNo" runat="server"></asp:Label>
                        </td>
                        <td></td>
                        <td style="width: 40%; text-align: right">Bill Date &nbsp;:<asp:Label ID="lblBillNepaliDate" runat="server"></asp:Label>
                            [<asp:Label ID="lblBillEnglishDate" runat="server"></asp:Label>]
                        </td>
                    </tr>
                    <tr>
                        <td>Name</td>
                        <td colspan="2">:                  
                         <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="">Address</td>
                        <td style="">:
                         <asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                        <td></td>
                        <td style="text-align: right"><span style="float: right">
                            <asp:Label ID="lblInvoiceHeading1" runat="server" Font-Bold="True"></asp:Label></span></td>
                    </tr>
                    <tr>
                        <td>PAN No</td>
                        <td>:
                         <asp:Label ID="lblCustomerPanNo" runat="server"></asp:Label></td>
                        <td></td>
                        <td style="text-align: right">Mode of Payment:
                            <asp:Label ID="lblModeofPayment" runat="server"></asp:Label></td>
                    </tr>
                </table>
                <style>
                    .custom-grid {
                        border-collapse: collapse;
                        width: 100%;
                        font-family: Arial, sans-serif;
                        font-size: 12px;
                        border-left: 1px solid #000;
                        border-right: 1px solid #000;
                        border-bottom: 1px solid #000;
                        border-top: 1px solid #000;
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
                        <td rowspan="4" style="vertical-align: top">
                            <asp:GridView ID="gridSalesInvoice" runat="server" CssClass="custom-grid" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.N">
                                        <ItemStyle Height="8%" Width="3%" />
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblSN" runat="server" Text='<%# Bind("SNO") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Particular">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("PRODUCT_NAME") %>' ID="lblProNAme" runat="server" />
                                            <br />
                                            <asp:Label Text='<%# "H.S: " + Eval("HS_CODE") %>' ID="lblHSCode" runat="server"
                                                Style="font-size: 10px; color: #555;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Batch" Visible="false">
                                        <ItemStyle Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("BATCH_NO") %>' ID="lblBatchNo" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exp Date" Visible="false">
                                        <ItemStyle Height="8%" Width="9%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("EXPIRY_DATE") %>' ID="lblExpDate" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Upper Qty">
                                        <ItemStyle HorizontalAlign="Center" Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("UPPER_QUANTITY") %>' ID="lblUQty" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemStyle HorizontalAlign="Center" Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("QUANTITY") %>' ID="lblQuantity" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate">
                                        <ItemStyle Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("RATE") %>' ID="lblRate" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemStyle HorizontalAlign="Right" Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("TOTAL") %>' ID="lblTotal" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sch.Disc">
                                        <ItemStyle HorizontalAlign="Right" Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblScheDisc" runat="server" Text='<%# Bind("SCHEME_DISCOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Taxable Amount">
                                        <ItemStyle HorizontalAlign="Right" Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("TAXABLE_TOTAL") %>' ID="lblTaxableTotal" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </td>
                    </tr>
                </table>
                <table style="width: 100%; font-size: 12pt; border-collapse: collapse; border-top: 1px solid black;">
                    <tr>
                        <td style="width: 60%; vertical-align: top;">In words:
                            Rs.
                            <asp:Label ID="lblAmountInWord" runat="server" Style="font-size: 12pt"></asp:Label>
                        </td>
                        <td style="width: 40%" rowspan="3">
                            <table style="width: 100%; border-collapse: collapse; border: none;">
                                <tr>
                                    <td style="padding-left: 3%">Sub Total</td>
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
                                <tr id="trInvVat" runat="server" visible="false">
                                    <td style="border-top: 1px solid black; padding-left: 3%">VAT
        <asp:Label ID="lblVATPercent" runat="server"></asp:Label>
                                        % </td>
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
                                <tr id="trInvVATReturn" runat="server" visible="false">
                                    <td style="border-top: 1px solid black; padding-left: 3%">VAT Return</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblInvVATReturn" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trInvRoundOff" runat="server" visible="false">
                                    <td style="border-top: 1px solid black; padding-left: 3%">Round off</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblRoundoff" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top: 1px solid black; padding-left: 3%">Net Amount</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblBillAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 60%; vertical-align: top;">
                            <br />
                            <table runat="server" id="divPO" visible="true">
                                <tr>
                                    <td>PO Number :
                                        <asp:Label ID="lblPONumber" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 60%; vertical-align: top;">
                            <span style="font-size: small">
                                <br />
                                * Please Check the goods before receiving,
                                <br />
                                once sold goods are not returnable
                                 <br />
                                * E.& O.E.            
                                <br />
                                <asp:Label ID="lblRemarks" runat="server" Text=""></asp:Label>
                            </span>

                        </td>
                    </tr>
                </table>
                <table style="width: 100%; font-size: 12pt">
                    <tr>
                        <td colspan="2">
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">............................</td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblInvCreatedBy" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">Received By</td>
                        <td style="text-align: right;">For:
                            <asp:Label ID="lblForCompanyName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-style: italic" colspan="2">
                            <asp:Label Font-Size="12px" ID="Label4" runat="server" Text="Print Date Time:"></asp:Label>
                            <asp:Label Font-Size="12px" ID="lblTime" runat="server"></asp:Label>
                            &nbsp;<asp:Label Font-Size="12px" ID="Label2" runat="server" Text="Print By:"></asp:Label>
                            <asp:Label Font-Size="12px" ID="lblPrintedBy" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>







    <%--<%--new GRID-- %>--%>
    <%-- NEW 80MM PRINT FORMAT --%><div id="printdetail80" runat="server" visible="false" style="margin-top: 1000px;">
        <div id="bill_format_80mm" style="width: 80mm; font-family: 'Consolas'; font-size: 13px; font-weight: 100; padding: 5px; box-sizing: border-box;">
            <div style="text-align: center;">
                <asp:Label ID="lblCompanyName80" runat="server" Style="font-weight: bold; font-size: 17px;"></asp:Label><br />
                <asp:Label ID="lblCompanyAddress80" runat="server"></asp:Label><br />
                Contact No:
                <asp:Label ID="lblPhone80" runat="server"></asp:Label><br />
                Vat No:
                <asp:Label ID="lblPanNo80" runat="server"></asp:Label>
            </div>

            <div style="text-align: center; margin: 4px 0;">
                --------------------------------<br />
                <asp:Label ID="lblInvoiceHeading80" runat="server" Style="font-weight: bold;"></asp:Label><br />
                --------------------------------
            </div>

            <%-- Invoice + Date info --%>
            <div style="align-content: flex-start">
                Bill No:
                <asp:Label ID="lblInvoiceNo80" runat="server"></asp:Label><br />
                Bill Date:
                <asp:Label ID="lblBillNepaliDate80" runat="server"></asp:Label>
                [<asp:Label ID="lblBillEnglishDate80" runat="server"></asp:Label>]<br />
                Tran. Date:
                <asp:Label ID="lblTranNepaliDate80" runat="server"></asp:Label>
                [<asp:Label ID="lblTranDate80" runat="server"></asp:Label>]
            </div>


            <%-- Customer Details --%>
            <div style="text-align: left">
                Name:
                <asp:Label ID="lblCustomerName80" runat="server"></asp:Label><br />
                <%--Address: <asp:Label ID="lblAddress80" runat="server"></asp:Label><br />--%>
        PAN No:
                <asp:Label ID="lblCustomerPanNo80" runat="server"></asp:Label><br />
                Payment:
                <asp:Label ID="lblModeofPayment80" runat="server"></asp:Label>
            </div>





            <asp:GridView ID="gridSalesInvoice80" runat="server" AutoGenerateColumns="False"
                Width="100%" ShowHeader="true" GridLines="None" CssClass="grid80mm" OnRowDataBound="gridSalesInvoice80_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="SN">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="lblSN80" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PARTICULAR">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label Text='<%# Bind("PRODUCT_NAME") %>' ID="lblProName80" runat="server" />
                            <br />
                            <asp:Label Text='<%# "H.S: " + Eval("HS_CODE") %>' ID="lblHSCode80" runat="server"
                                Style="font-size: 10px;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="QTY">
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label Text='<%# Bind("QUANTITY") %>' ID="lblQty80" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RATE">
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label Text='<%# Bind("RATE") %>' ID="lblRate80" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AMT">
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label Text='<%# Bind("TOTAL") %>' ID="lblTotal80" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>


            <table style="width: 100%; font-size: 13px;">
                <tr>
                    <td style="text-align: right;" class="td-border-top">Sub Total:</td>
                    <td style="text-align: right;" class="td-border-top">
                        <asp:Label ID="lblBillSubTotal80" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right;">Discount(<asp:Label ID="lblDiscPercent80" runat="server"></asp:Label>%):</td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblDiscount80" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right;">Taxable Amount:</td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblTaxable80" runat="server"></asp:Label></td>
                </tr>
                <tr id="trVat80" runat="server">
                    <td style="text-align: right;">VAT(<asp:Label ID="lblVATPercent80" runat="server"></asp:Label>%):</td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblVAT80" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right;">Total:</td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblGrandTotal80" runat="server"></asp:Label></td>
                </tr>
                <tr id="trVATReturn80" runat="server" visible="false">
                    <td style="text-align: right;">VAT Return:</td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblVATReturn80" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right;" class="td-border-top td-border-bottom"><b>Net Amount:</b></td>
                    <td style="text-align: right;" class="td-border-top td-border-bottom"><b>
                        <asp:Label ID="lblGTotal80" runat="server"></asp:Label></b></td>
                </tr>
            </table>


            <div>---------------------------------------------</div>

            <div>
                In Words: Rs.
                <asp:Label ID="lblAmountInWord80" runat="server"></asp:Label>
            </div>

            <div>---------------------------------------------</div>

            <%-- Remarks --%>
            <div style="font-size: 10px;">
                <asp:Label ID="lblRemarks80" runat="server" Text=""></asp:Label>
            </div>

            <div style="text-align: left; margin-top: 4px;">
                * Goods once sold are not returnable
                <br />
                * E.& O.E.
            </div>

            <div>---------------------------------------------</div>


            <div style="font-size: 13px; margin-top: 4px;">
                Printed By:
    <asp:Label ID="lblPrintedBy80" runat="server"></asp:Label><br />
                Print Time:
    <asp:Label ID="lblTime80" runat="server"></asp:Label><br />
                User:
    <asp:Label ID="lblCreatedBy80" runat="server"></asp:Label>
            </div>
            <%-- Copy info --%>
            <div style="text-align: left;">
                <asp:Label ID="lblInvoiceHeading180" runat="server"></asp:Label>
            </div>

            <div style="text-align: center; margin-top: 4px;">THANK YOU...</div>
            <%-- <div style="font-size: 13px; margin-top: 4px;">Order No:
                 <asp:Label ID="lblOrderNo80" runat="server"></asp:Label>
            </div>--%>
        </div>
    </div>

    <%-- TOK PRINT FORMAT --%>
    <div id="printdetailTOK" runat="server" visible="false" style="margin-top: 1000px;">
        <div id="bill_format_tok" style="width: 80mm; font-family: 'Consolas'; font-size: 13px; font-weight: 100; padding: 5px; box-sizing: border-box;">
            <div style="text-align: center; margin-bottom: 4px;">
                <asp:Label ID="lblCompanyNameTOK" runat="server"
                    Style="font-weight: bold; font-size: 20px;"></asp:Label><br />
                <asp:Label ID="lblCompanyAddressTOK" runat="server"></asp:Label><br />
                Contact No:
                <asp:Label ID="lblPhoneTOK" runat="server"></asp:Label><br />

                VAT No:
                <asp:Label ID="lblPanNoTOK" runat="server"></asp:Label>
                <div style="text-align: center; margin-bottom: 4px; font-size: 22px; font-weight: 700;">KOT</div>
            </div>
            <div style="margin-bottom: 4px; text-align: center; font-size: 18px;">

                <asp:Label ID="lblNameTOK" runat="server"></asp:Label><br />
            </div>
            <div style="margin-bottom: 4px;">
                Date:
                <asp:Label ID="lblBillDateTOK" runat="server"></asp:Label>
            </div>

            <div style="margin-bottom: 4px;">
                Bill No:
                <asp:Label ID="lblBillNoTOK" runat="server"></asp:Label>
            </div>


            <asp:GridView ID="gridTOK" runat="server" AutoGenerateColumns="False"
                Width="100%" ShowHeader="true" GridLines="None" CssClass="grid80mm">
                <Columns>
                    <asp:TemplateField HeaderText="SN">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="lblSNTOK" runat="server"
                                Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="PARTICULAR">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="lblProNameTOK" runat="server"
                                Text='<%# Bind("PRODUCT_NAME") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="QTY">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="lblQtyTOK" runat="server"
                                Text='<%# Bind("QUANTITY") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <div style="text-align: center; margin-top: 4px;">_________________________________________________________</div>
            <div style="font-size: 13px; margin-top: 4px;">
                Printed By:
   
                <asp:Label ID="lblPrintedByTOK" runat="server"></asp:Label><br />
                Print Time:
   
                <asp:Label ID="lblTimeTOK" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>

