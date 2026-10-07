<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShowPurchaseInvoice.aspx.cs" Inherits="Reports_Purchase_ShowPurchaseInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        /* Apply border-collapse to ensure borders are managed correctly */
        .custom-grid {
            border-collapse: collapse;
            width: 100%;
            border: none;
        }

            /* Style for table header cells */
            .custom-grid th {
                padding: 8px;
                background-color: #f2f2f2;
                border: none;
                border-top: solid 1px;
                border-bottom: solid 1px;
            }

            /* Style for table body cells */
            .custom-grid td {
                padding: 8px;
            }

            /* Add a horizontal border to rows, excluding the last row */
            .custom-grid tr td {
                border: none;
            }
    </style>
    <div id="printdetail" runat="server">

        <div class="row">
            <div class="col-md-1">
                <asp:Button ID="btn_back" CssClass="btn btn-primary" runat="server" Text="Back" OnClick="btn_back_Click" />
            </div>
            <div class="col-md-10">
                <div id="print_div" style="padding: 30px 30px 30px 30px; box-sizing: border-box; border: solid; margin: auto;">
                    <div id="bill_format_1" style="margin-bottom: 50px; box-sizing: border-box; padding: 5px; font-family: Calibri">
                        <table style="width: 100%">
                            <thead>
                                <asp:Label Font-Size="20px" Font-Italic="true" ID="Label1" runat="server" Text="*Use as refernce Only"></asp:Label>
                            </thead>
                            <tr>
                                <td>
                                    <table style="width: 100%">
                                        <tr style="padding-top: 10px; padding-bottom: 10px;">
                                            <td style="font-weight: bold; font-family: Verdana; font-size: 26px" colspan="3">
                                                <asp:Label ID="lblInvoiceHeading" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%; font-size: 13pt">
                                        <tr>
                                            <td colspan="2"></td>
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
                                            <td style="width: 15%;">Dakhila No.</td>
                                            <td style="width: 45%;">:
                            <asp:Label Font-Size="14pt" Font-Bold="true" ID="lblDakhilaNo" runat="server"></asp:Label>
                                            </td>
                                            <td></td>
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
                                            <td style="text-align: right"><span style="float: right"></td>
                                            <%--<asp:Label ID="lblInvoiceHeading1" runat="server" Font-Bold="True"></asp:Label></span></td>--%>
                                        </tr>
                                        <tr>
                                            <td>PAN No</td>
                                            <td>:
                         <asp:Label ID="lblCustomerPanNo" runat="server"></asp:Label></td>
                                            <td style="text-align: right">Mode of Payment:
                            <asp:Label ID="lblModeofPayment" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>


                                    <table style="width: 100%; min-height: 500px">

                                        <tr>
                                            <td rowspan="4" style="height: 500px; vertical-align: top">
                                                <asp:GridView ID="gridSalesInvoice" runat="server" CssClass="custom-grid" AutoGenerateColumns="False" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.N">
                                                            <ItemStyle Height="20px" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="HS Code">
                                                            <ItemStyle Height="8%" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%# Bind("HS_CODE") %>' ID="lblHSCode" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Particular">
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%# Bind("PRODUCT_NAME") %>' ID="lblProNAme" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Batch" Visible="false">
                                                            <ItemStyle Height="8%" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%# Bind("BATCH") %>' ID="lblBatchNo" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Expiry Date" Visible="false">
                                                            <ItemStyle Height="8%" Width="5%" />
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%# Bind("EXPIRY_DATE") %>' ID="lblExpDate" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Upper Qty" Visible="false">
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
                                                    <tr id="trInvVat" runat="server" visible="true">
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
                                                    <tr id="trInvRoundOff" runat="server">
                                                        <td style="border-top: 1px solid black; padding-left: 3%">Round off</td>
                                                        <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                                            <asp:Label ID="lblRoundoff" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trInvRoTot">
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
                                                <span style="font-size: small">
                                                    <br />
                                                    * Please Check the goods before receiving.
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
                                            <td style="font-style: italic" colspan="2">&nbsp;<asp:Label Font-Size="12px" ID="Label2" runat="server" Text="Entry By:"></asp:Label>
                                                <asp:Label Font-Size="12px" ID="lblPrintedBy" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td rowspan="4"></td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

