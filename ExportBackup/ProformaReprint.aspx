<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProformaReprint.aspx.cs" Inherits="Export_ProformaReprint" %>

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
        function printProforma() {
            var printContent = document.getElementById('bill_format_proforma');
            var printWindow = window.open('about:blank', 'PrintProforma' + new Date().getTime(), 'left=0,top=0,width=800,height=600');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            return false;
        }

        function formatProformaFooter() {
            var table = document.getElementById('<%= gridProforma.ClientID %>');
            if (!table || !table.rows.length) return;

            var footerRow = table.rows[table.rows.length - 1];
            if (!footerRow || footerRow.cells.length < 6) return;

            footerRow.cells[0].innerText = '';
            footerRow.cells[0].style.border = '1px solid #000';

            footerRow.cells[1].innerText = '';
            footerRow.cells[1].style.border = '1px solid #000';

            var particularsCell = footerRow.cells[2];
            particularsCell.colSpan = 3;
            particularsCell.innerText = 'Sub Total';
            particularsCell.style.textAlign = 'left';
            particularsCell.style.border = '1px solid #000';

            footerRow.deleteCell(3);
            footerRow.deleteCell(3);

            var amountCell = footerRow.cells[3];
            var hiddenSubTotal = document.getElementById('<%= hdnProSubTotal.ClientID %>');
            amountCell.innerText = hiddenSubTotal ? hiddenSubTotal.value : '';
            amountCell.style.textAlign = 'right';
            amountCell.style.border = '1px solid #000';
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
                <asp:GridView ID="grdProforma" runat="server" AutoGenerateColumns="False" CssClass="gridtable" Width="100%" OnRowCommand="grdProforma_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblInvoiceNo" runat="server" Text='<%# Bind("INVOICE_NUMBER") %>' OnClick="lblInvoiceNo_Click"></asp:LinkButton>
                                <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceDay" runat="server" Text='<%# Bind("INVOICE_DATE_NP") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CUSTOMER_NAME") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Currency">
                            <ItemTemplate>
                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Bind("CURRENCY_CODE") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount (FC)">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("FINAL_AMOUNT") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnPrint" runat="server" ImageUrl="~/images/icons/print.gif" CommandName="Print" ToolTip="Print Proforma Invoice" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- PROFORMA PRINT LAYOUT -->
    <div id="printdetailProforma" runat="server" visible="false" style="margin-top: 1000px;">
        <div id="print_div_proforma" style="width: 210mm; padding: 30px; box-sizing: border-box; margin: auto;">
            <div id="bill_format_proforma" style="margin-bottom: 50px; box-sizing: border-box; padding: 5px; font-family: Calibri;">
                <div style="text-align: center; font-size: 26px; font-weight: 600; margin-bottom: 12px;">Invoice</div>
                <table style="width: 100%; border-collapse: collapse; font-size: 12pt;">
                    <tr>
                        <td colspan="2" style="width: 100%; text-align: right">
                            <asp:Image ID="imgn" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 55%; border: 1px solid #000; padding: 6px; vertical-align: top;">
                            <div style="font-weight: 700; font-size: 16px; text-transform: uppercase;">
                                <asp:Label ID="lblProCompanyName" runat="server"></asp:Label>
                            </div>
                            <div>
                                <asp:Label ID="lblProCompanyAddress" runat="server"></asp:Label>
                            </div>
                            <div>
                                Company Registered No:
                            <asp:Label ID="lblProCompanyRegNo" runat="server"></asp:Label>
                            </div>
                            <div>
                                EXIM Code:
                            <asp:Label ID="lblProEximCode" runat="server"></asp:Label>
                            </div>
                            <div>
                                Email:
                            <asp:Label ID="lblProCompanyEmail" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td style="width: 45%; border: 1px solid #000; border-left: none; padding: 6px; vertical-align: top;">
                            <table style="width: 100%; border-collapse: collapse;">
                                <tr>
                                    <td>Invoice No</td>
                                    <td>:
                                        <asp:Label ID="lblProInvoiceNo" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>Issue Date</td>
                                    <td>:
                                        <asp:Label ID="lblProIssueDate" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>Contract No</td>
                                    <td>:
                                        <asp:Label ID="lblProContractNo" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>Contract Date</td>
                                    <td>:
                                        <asp:Label ID="lblProContractDate" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 55%; border: 1px solid #000; border-top: none; padding: 6px; vertical-align: top;">
                            <div style="text-decoration: underline; font-weight: 600;">Invoice To</div>
                            <div>
                                <asp:Label ID="lblProCustomerName" runat="server"></asp:Label>
                            </div>
                            <div>
                                <asp:Label ID="lblProCustomerAddress" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td style="width: 45%; border: 1px solid #000; border-left: none; border-top: none; padding: 6px; vertical-align: top;">
                            <table style="width: 100%; border-collapse: collapse;">
                                <tr>
                                    <td style="white-space: nowrap; width: 1%; padding-right: 8px;">Payment Currency</td>
                                    <td>:
                                        <asp:Label ID="lblProPaymentCurrency" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="white-space: nowrap; width: 1%; padding-right: 8px;">Terms</td>
                                    <td>:
                                        <asp:Label ID="lblProPaymentMode" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="white-space: nowrap; width: 1%; padding-right: 8px;">Shipment Type</td>
                                    <td>:
                                        <asp:Label ID="lblProShipmentType" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="white-space: nowrap; width: 1%; padding-right: 8px;">Shipment No</td>
                                    <td>:
                                        <asp:Label ID="lblProShipmentNo" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

                <asp:GridView ID="gridProforma" runat="server" AutoGenerateColumns="False" GridLines="Both" ShowFooter="true"
                    Width="100%" CellPadding="6" Style="border-collapse: collapse; border: 1px solid #000; font-size: 11pt; margin-top: -1px;">
                    <HeaderStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" Font-Bold="true" />
                    <RowStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                    <FooterStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" Font-Bold="true" />
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemStyle HorizontalAlign="Left" Width="4%" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <ItemTemplate>
                                <asp:Label ID="lblProSN" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HS Code">
                            <ItemStyle HorizontalAlign="Left" Width="3%" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <ItemTemplate>
                                <asp:Label ID="lblProHS" runat="server" Text='<%# Bind("HS_CODE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Particulars">
                            <ItemStyle HorizontalAlign="Left" Width="45%" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <HeaderStyle HorizontalAlign="Left" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <ItemTemplate>
                                <asp:Label ID="lblProParticular" runat="server" Text='<%# Bind("PRODUCT_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty">
                            <ItemStyle HorizontalAlign="Right" Width="10%" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <ItemTemplate>
                                <asp:Label ID="lblProQty" runat="server" Text='<%# Bind("QUANTITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rate">
                            <ItemStyle HorizontalAlign="Right" Width="10%" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <ItemTemplate>
                                <asp:Label ID="lblProRate" runat="server" Text='<%# Bind("RATE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemStyle HorizontalAlign="Right" Width="15%" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <ItemTemplate>
                                <asp:Label ID="lblProAmount" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hdnProSubTotal" runat="server" />

                <div style="border: 1px solid #000; border-top: none; padding: 6px; font-size: 11pt;">
                    <b>Amount in words:</b>
                    <asp:Label ID="lblProAmountInWord" runat="server"></asp:Label>
                </div>

                <table style="width: 100%; font-size: 8pt; margin-top: 20px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblProRemarks" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">For: 
                        <asp:Label ID="lblProUserName" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

