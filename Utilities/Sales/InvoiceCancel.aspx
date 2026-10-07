<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="InvoiceCancel.aspx.cs" Inherits="Utilities_Sales_InvoiceCancel" %>

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
            //printWindow.close();
        }
    </script>


    <div class="container-fluid form-group-sm" runat="server" id="divInitial">
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
            <div class="col-md-12">
                <asp:GridView ID="grdSales" runat="server" AutoGenerateColumns="False" CssClass="gridtable" Width="100%"
                    OnRowCommand="grdSales_RowCommand">
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
                        <asp:TemplateField HeaderText="Customer">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CUSTOMER_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bill Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblBillAmount" runat="server" Text='<%# Bind("SUB_TOTAL") %>'></asp:Label>
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
                        <asp:TemplateField HeaderText="Purchase Type">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchaseType" runat="server" Text='<%# Bind("SALES_TYPE_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/cancel.gif" CommandName="Alter" ToolTip="Cancel Invoice" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>

    <div class="container-fluid form-group-sm" runat="server" id="divInvoiceDetail" visible="false">
        <table style="width: 1300px;">
            <tr>
                <td style="width: 800px; padding-right: 10px; border-right: dashed 1px;">
                    <table style="width: 800px;">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblPK_ID_Hiden" runat="server" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="lblunique_token" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 40%; text-align: right">Tran. Date:<asp:Label ID="lblTranNepaliDate" runat="server"></asp:Label>
                                [<asp:Label ID="lblTranDate" runat="server"></asp:Label>]</td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">Invoice No.</td>
                            <td style="width: 45%;">:
                            <asp:Label Font-Size="14pt" Font-Bold="true" ID="lblInvoiceNo" runat="server"></asp:Label>
                            </td>
                            <td style="width: 40%; text-align: right">Bill Date &nbsp;:<asp:Label ID="lblBillNepaliDate" runat="server"></asp:Label>
                                [<asp:Label ID="lblBillEnglishDate" runat="server"></asp:Label>]
                            </td>
                        </tr>
                        <tr>
                            <td>Name</td>
                            <td colspan="2">:                  
                         <asp:Label ID="lblCustomerName" runat="server"></asp:Label><asp:Label ID="lblCustomerID" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="">Address</td>
                            <td style="">:
                         <asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                            <td style="text-align: right"><span style="float: right">
                                <asp:Label ID="lblInvoiceHeading1" runat="server" Font-Bold="True"></asp:Label></span></td>
                        </tr>
                        <tr>
                            <td>PAN No</td>
                            <td>:
                         <asp:Label ID="lblCustomerPanNo" runat="server"></asp:Label></td>
                            <td style="text-align: right">Mode of Payment:
                            <asp:Label ID="lblModeofPayment" runat="server"></asp:Label></td>
                        </tr>

                        <tr>
                            <td colspan="3">
                                <asp:GridView ID="grdInvoiceDetail" runat="server" Width="100%" AutoGenerateColumns="False"
                                    OnRowDataBound="grdInvoiceDetail_RowDataBound" CssClass="gridtable">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Particulars">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductId" runat="server" Text='<%# Bind("PRODUCT_ID") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblProductCode" runat="server" Text="" Visible="false"></asp:Label>
                                                <asp:Label ID="lblProductName" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Batch">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBatch" runat="server" Text='<%# Bind("BATCH_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Expiry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Bind("EXPIRY_DATE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUQty" runat="server" Text='<%# Bind("UPPER_QUANTITY") %>'></asp:Label>
                                                <asp:Label ID="lblUUnit" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" Text='<%# Bind("QUANTITY") %>'></asp:Label>
                                                <asp:Label ID="lblUnit" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Disc">
                                            <ItemTemplate>
                                                <asp:Label ID="lblScheDisc" runat="server" Text='<%# Bind("SCHEME_DISCOUNT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAfterScheDisc" runat="server" Text='<%# Bind("TAXABLE_TOTAL")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="3">
                                <table style="width: 100%; font-size: 12pt; border-collapse: collapse; border-top: 1px solid black;">
                                    <tr>
                                        <td style="width: 60%; font-size: 12pt; vertical-align: top;">In words:
                            Rs.
                            <asp:Label ID="lblAmountInWord" runat="server" Style="font-size: 12pt"></asp:Label>
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
                                                <tr id="trInvVat" runat="server" visible="true">
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
                                                <tr id="trInvRoundOffS" runat="server" visible="false">
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
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td style="text-align: right">&nbsp;</td>
                        </tr>
                    </table>
                </td>
                <td style="padding-left: 10px; vertical-align: top;">
                    <table style="width: 100%">
                        <tr>
                            <td>Credit Note Date
                                <br />
                                <asp:TextBox ID="txtCreditNoteDate" runat="server" CssClass="form-control" Style="width: 150px"
                                    Enabled="false"></asp:TextBox>
                            </td>
                            <td runat="server" id="divSalesReturnType" visible="false">Sales Return Type
                            <br />
                                <asp:DropDownList ID="ddlCNType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCNType_SelectedIndexChanged">
                                </asp:DropDownList></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr id="divBankDetail" runat="server" visible="false">
                            <td colspan="2">
                                <asp:Label ID="lblQRofBank" runat="server" Text="Bank"></asp:Label><br />
                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td colspan="2">Note Remarks</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:TextBox ID="txtCreditNoteRemarks" runat="server" Width="100%" TextMode="MultiLine" Height="120px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" Width="100px" OnClick="btnSave_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" Width="100px" OnClick="btnCancel_Click" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lblerror" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>



    </div>
    <div class="container-fluid form-group-sm" runat="server" id="divButton" visible="false">
        <asp:Button ID="btnClear" runat="server" Text="Cancel New Invoice" CssClass="btn btn-success" OnClick="btnClear_Click" />
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
                                        <asp:Label ID="lblWebsite" runat="server" Text="" Style="font-size: 12pt;"></asp:Label>
                                        <asp:Label ID="lblDivider" runat="server" Text="|" Style="font-size: 12pt;"></asp:Label>
                                        <asp:Label ID="lblEmail" runat="server" Text="" Style="font-size: 12pt;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblPhone" runat="server" Text="" Style="font-size: 12pt;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="Label1" runat="server" Style="font-size: 12pt; font-weight: normal; font-family: Verdana;" Text="PAN No."></asp:Label>
                                        <asp:Label ID="lblPanNo" runat="server" Style="font-size: 12pt; font-weight: normal; font-family: Verdana;"></asp:Label>

                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 15%; text-align: center; vertical-align: top;"></td>
                    </tr>
                    <tr style="padding-top: 10px; padding-bottom: 10px;">
                        <td style="text-align: center; font-weight: bold; font-family: Verdana; font-size: 26px" colspan="3">
                            <asp:Label ID="lblHeading" runat="server" Text="Credit Note"></asp:Label>
                        </td>
                    </tr>

                </table>
              <table style="width: 100%; font-size: 13pt">
                    <tr>
                        <td>Credit Note No.</td>
                        <td>:                           
                            <asp:Label Font-Size="14pt" Font-Bold="true" ID="lblCreditNoteNo" runat="server"></asp:Label>
                        </td>
                        <td style="text-align: right">Credit Note Date 
                        </td>
                        <td style="text-align: right">:<asp:Label ID="lblCNNepaliDate" runat="server"></asp:Label>
                            [<asp:Label ID="lblCNEnglishDate" runat="server"></asp:Label>]
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">Invoice Number</td>
                        <td style="width: 30%">:
                            <asp:Label ID="lblCNInvoiceNumber" runat="server"></asp:Label>
                        </td>
                        <td style="width: 20%; text-align: right">Invoice Date</td>
                        <td style="text-align: right">:<asp:Label ID="lblInvoiceNepaliDate" runat="server"></asp:Label>
                            [<asp:Label ID="lblInvoiceEnglishDate" runat="server"></asp:Label>]</td>
                    </tr>

                    <tr>
                        <td>Customer Name</td>
                        <td colspan="3">:                  
                         <asp:Label ID="lblCNCustomerName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                      <td style="">Address</td>
                        <td colspan="2" style="">:
                         <asp:Label ID="lblCNCustomerAddress" runat="server"></asp:Label></td>
                        <td style="text-align: right" >&nbsp;</td>
                    </tr>
                    <tr>
                        <td>PAN No</td>
                        <td>:
                         <asp:Label ID="lblCNCustomerPanNo" runat="server"></asp:Label></td>
                        <td style="text-align: right" colspan="2">Mode of Payment:
                            <asp:Label ID="lblCNModeofPayment" runat="server"></asp:Label></td>
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
                        <td>
                            <asp:GridView ID="gridSalesRetInvoice" runat="server" CssClass="custom-grid" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.N">
                                     
                                        <ItemTemplate>
                                            <asp:Label ID="lblSN" runat="server" Text='<%# Bind("SNO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Particular">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("PARTICULARS") %>' ID="lblProNAme" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Upper Qty">
                                        <ItemStyle HorizontalAlign="Center"  Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("U_QTY") %>' ID="lblUQuantity" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
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
                                        <ItemStyle HorizontalAlign="Right"  Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderTemplate>
                                            Discount
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("DISC") %>' ID="lblScheDisc" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderTemplate>
                                            Taxable Total
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("AMT_AFT_DISC") %>' ID="lblTaxTot" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right"  Width="5%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>

                <table style="width: 100%; font-size: 12pt; border-collapse: collapse; border-top: 1px solid black;">
                    <tr>
                        <td style="width: 60%; vertical-align: top;">
                            <table style="width: 100%">
                                <tr>
                                    <td>In words:
                                        Rs.
                                        <asp:Label ID="lblCNAmounInWords" runat="server" Style="font-size: 12pt"></asp:Label><br />
                                    </td>
                                </tr>
                                <tr style="border-top: solid 1px">
                                    <td>
                                        <b>Credit Note Remarks:</b>
                                        <br />
                                        <asp:Label ID="lblCNRemarks" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </td>
                        <td style="width: 40%">
                            <table style="width: 100%; border-collapse: collapse; border: none;">
                                <tr>
                                    <td style="padding-left: 3%">Total</td>
                                    <td style="text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblCNSubTotal" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="border-top: 1px solid black; padding-left: 3%">Discount
                                        <asp:Label ID="lblCNDiscountPercent" runat="server" Text=""></asp:Label>
                                        %</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblCNDiscountAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top: 1px solid black; padding-left: 3%">Taxable amount</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblCNTaxableAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trInvVats" runat="server" visible="true">
                                    <td style="border-top: 1px solid black; padding-left: 3%">VAT
                                        <asp:Label ID="lblCNVATPercent" runat="server"></asp:Label>
                                        % </td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblCNVATAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top: 1px solid black; padding-left: 3%">Total</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblCNGrandTotal" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trInvRoundOff" runat="server" visible="false">
                                    <td style="border-top: 1px solid black; padding-left: 3%">Round off</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblCNRoundoff" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trInvRoTot">
                                    <td style="border-top: 1px solid black; padding-left: 3%">Total</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblCNInvoiceAmount" runat="server"></asp:Label>
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
                        <td style="text-align: right;">Credit Note Created By</td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

</asp:Content>

