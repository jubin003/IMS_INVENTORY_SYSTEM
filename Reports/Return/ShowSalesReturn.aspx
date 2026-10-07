<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShowSalesReturn.aspx.cs" Inherits="Reports_Return_ShowSalesReturn" %>

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
    <div class="row p-t-10">
        <div class="col-md-1">
            <asp:Button ID="btn_back" CssClass="btn btn-primary" runat="server" Text="Back" OnClick="btn_back_Click" />
        </div>
    </div>
    <div id="printdetail" style="width: 100%" runat="server" visible="true">
        <div id="print_div" style="width: 100%; padding: 30px 30px 30px; box-sizing: border-box; margin: auto;">
            <div id="bill_format_1" style="box-sizing: border-box; padding: 5px;">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 15%; text-align: center; vertical-align: top;">
                            &nbsp;</td>
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
                <table style="width: 100%; min-height: 500px">
                    <tr>
                        <td rowspan="4" style="height: 500px; vertical-align: top">
                            <asp:GridView ID="gridSalesRetInvoice" runat="server" CssClass="custom-grid" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.N">
                                        <ItemStyle Height="20px" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
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
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderTemplate>
                                            Discount
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("DISC") %>' ID="lblScheDisc" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" Height="8%" Width="5%" />
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
                                <tr id="trInvVat" runat="server" visible="false">
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

