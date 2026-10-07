<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesReturn.aspx.cs" Inherits="Utilities_Sales_SalesReturn" %>

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
            <div class="col-md-1">
            </div>
            <div class="col-md-4">
                <br />
                *
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/ff0033.png" />
                Credit Note have been already created
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:GridView ID="grdSales" runat="server" AutoGenerateColumns="False" BorderStyle="Outset" CssClass="grid-bordered gridtable" Width="100%"
                    OnRowCommand="grdSales_RowCommand" OnRowDataBound="grdSales_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice No">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# Bind("INVOICE_NUMBER") %>'></asp:Label>
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
                        <asp:TemplateField HeaderText="Sales Type">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchaseType" runat="server" Text='<%# Bind("SALES_TYPE_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/add.png" CommandName="Alter" ToolTip="Sales Return" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>
    <br />
    <asp:Label ID="lblPK_ID_Hiden" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Label ID="lblunique_token" runat="server" Text="" Visible="false"></asp:Label>
    <div class="container-fluid form-group-sm" runat="server" id="divInvoiceDetail" visible="false">
        <table style="width: 100%; border-bottom: solid 1px;">
            <tr>
                <td style="width: 700px; padding-right: 5px; vertical-align: top;">
                    <table style="width: 100%">
                        <tr>
                            <td>Credit Note Date
                                <br />
                                <asp:TextBox ID="txtCreditNoteDate" runat="server" CssClass="form-control datepicker" Style="width: 150px" Enabled="false"></asp:TextBox>
                            </td>
                            <td>Invoice No<br />
                                <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="form-control" Enabled="false" Style="width: 150px"></asp:TextBox>
                            </td>
                            <td>Invoice Date
                                <br />
                                <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control" Enabled="false" Style="width: 150px"></asp:TextBox>
                            </td>
                            <td>Sales Type
                            <br />
                                <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control" Enabled="false">
                                </asp:DropDownList></td>
                        </tr>
                        <tr id="divExistingCustomer" runat="server">
                            <td>Customer Code
                                <br />
                                <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="form-control" Style="width: 150px" Enabled="false"></asp:TextBox>
                            </td>
                            <td colspan="3">Name
                                        <br />
                                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control" Width="450px" Enabled="false"
                                    Font-Size="Larger">
                                </asp:DropDownList>
                                <script>
                                    $('#<%=ddlCustomer.ClientID%>').chosen();
                                </script>

                            </td>
                        </tr>
                        <tr id="divWalkInCustomer" runat="server" visible="false">
                            <td colspan="4">Customer Name
                            <br />
                                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>PAN/VAT No
                            <br />
                                <asp:TextBox ID="txtCustomerPANVAT" runat="server" CssClass="form-control" Style="width: 150px" Enabled="false"></asp:TextBox>
                            </td>
                            <td colspan="2">Address
                            <br />
                                <asp:TextBox ID="txtCustomerAddress" runat="server" CssClass="form-control" Style="width: 350px" Enabled="false"></asp:TextBox>
                            </td>
                            <td>Contact No<br />
                                <asp:TextBox ID="txtCustomerContact" runat="server" CssClass="form-control" Style="width: 150px" Enabled="false"></asp:TextBox>
                                <asp:Label ID="lblAgentCode" runat="server" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="lblAreaCode" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </td>
                <td style="width: 500px; padding-left: 15px; border-left: dashed 1px; vertical-align: top;">
                    <table>
                        <tr>
                            <td>Invoice Type
                                <br />
                                <asp:DropDownList ID="ddlExempted" runat="server" CssClass="form-control" Enabled="false" Width="125px">
                                    <asp:ListItem Text="Tax Invoice" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Exempted Invoice" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Credit Note Type<br />
                                <asp:DropDownList ID="ddlCNType" runat="server" CssClass="form-control" Width="125px" AutoPostBack="true" OnSelectedIndexChanged="ddlCNType_SelectedIndexChanged">
                                    <asp:ListItem Value="CR">Credit</asp:ListItem>
                                    <asp:ListItem Value="CS">Cash</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <br />

                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">Return Note<br />
                                <asp:TextBox ID="txtCreditNoteRemarks" runat="server" TextMode="MultiLine" CssClass="form-control" Width="500px" Height="100px"></asp:TextBox>
                                <br />
                            </td>
                        </tr>

                    </table>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <br />
        <table style="border: solid;">
            <tr>
                <td style="width: 900px">
                    <div style="height: 300px; width: 900px; overflow: scroll; overflow-x: hidden; margin-top: 10px;">
                        <div style="background-color: cadetblue; text-align: center; width: 875px">
                            <b>Invoice Detail </b>
                        </div>
                        <asp:GridView ID="grdSalesDetail" runat="server" AutoGenerateColumns="False" Width="875px" CssClass="gridtable" OnRowDataBound="grdSalesDetail_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                        <asp:Label ID="lblProductPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Bind("PRODUCT_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("PRODUCT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Batch" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBatch" runat="server" Text='<%# Bind("BATCH_NO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exp. Date" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Bind("EXPIRY_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inv. Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUQty" runat="server" Text='<%# Bind("UPPER_QUANTITY") %>'></asp:Label>
                                        <asp:Label ID="lblUUnit" runat="server" Text='<%# Bind("U_UNIT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sold Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQty" runat="server" Text='<%# Bind("QUANTITY") %>'></asp:Label>
                                        <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("UNIT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rem. Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemQty" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="lblRUnit" runat="server" Text='<%# Bind("R_UNIT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Return Qty">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRQty" runat="server" Text='<%# Bind("R_QUANTITY") %>'
                                            CssClass="form-control" Width="80px" OnTextChanged="txtRQty_TextChanged"
                                            AutoPostBack="true"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRate" runat="server" Text='<%# Bind("RATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblItemRTotal" AutoPostBack="true" OnTextChanged="lblItemRTotal_TextChanged"
                                            CssClass="form-control" Enabled="false" runat="server" Text='<%# Bind("R_TOTAL") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActualSchDiscount" runat="server" Text='<%# Bind("ACTUAL_DISCOUNT") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblSchDiscount" runat="server" Text=''></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAfterScheDisc" runat="server" Text=''></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>

                    </div>
                </td>
                <td style="vertical-align: top; padding-left: 5px; padding-right: 5px;">
                    <br />
                    <br />
                    <table style="width: 300px;">
                        <tr>
                            <td style="width: 90px;">Sub Total</td>
                            <td style="width: 45px;"></td>
                            <td style="text-align: right; width: 35px;">
                                <asp:Label ID="lblSubTotalAmount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Trade Discount</td>
                            <td>
                                <asp:TextBox ID="txtDiscount" runat="server" Width="35px" Text="0" OnTextChanged="txtDiscount_TextChanged" AutoPostBack="true"></asp:TextBox>
                                %</td>
                            <td style="text-align: right;">
                                <asp:TextBox ID="txtDiscountAmount" runat="server" Width="80px" Style="text-align: right" OnTextChanged="txtDiscountAmount_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Total</td>
                            <td></td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblTotalAmount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr runat="server" id="divVAT1">
                            <td class="auto-style1">VAT</td>
                            <td class="auto-style1">
                                <asp:TextBox ID="txtVATPercent" runat="server" Width="35px" Text="" ReadOnly="true"></asp:TextBox>
                                %</td>
                            <td style="text-align: right;" class="auto-style1">
                                <asp:Label ID="lblVAT" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr runat="server" id="divVAT2">

                            <td>Grand Total</td>
                            <td></td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblGrandTotal" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>

                            <td>Rounding</td>
                            <td></td>
                            <td style="text-align: right;">
                                <asp:TextBox ID="txtRound" runat="server" AutoPostBack="true" Width="80px"
                                    Text="" Style="text-align: right"
                                    TextMode="Number" step="0.01"
                                    OnTextChanged="txtRound_TextChanged"
                                    oninput="if(parseFloat(this.value) > 1) this.value = 1;">
                                </asp:TextBox>


                            </td>
                        </tr>
                        <tr>
                            <td>Invoice Amount</td>
                            <td></td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblInvoiceAmount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" Width="100%" OnClick="btnSave_Click" /><br />
                                <br />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" Width="100%" OnClick="btnCancel_Click1" /><br />

                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblerror" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
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
                        <td style="width: 15%; text-align: center; vertical-align: top;">
                            <asp:Image ID="sImage1" runat="server" Width="100px" ImageUrl="~/images/logo.png" />
                        </td>
                        <td style="text-align: center;">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblCompanyName" runat="server" Text="" Style="font-weight: bold; font-size: 32px;"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblCompanyAddress" runat="server" Text="" Style="font-size: 12pt;"></asp:Label></td>
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
                        <td style="">:
                         <asp:Label ID="lblCNCustomerAddress" runat="server"></asp:Label></td>
                        <td style="text-align: right" colspan="2">&nbsp;</td>
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
                        <td>
                            <asp:GridView ID="gridSalesRetInvoice" runat="server" CssClass="custom-grid" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.N">
                                        <ItemStyle Height="8%" Width="3%" />
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
                                        <ItemStyle HorizontalAlign="Center" Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("U_QTY") %>' ID="lblUQuantity" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
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

