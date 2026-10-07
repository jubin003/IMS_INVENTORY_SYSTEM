<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShowPurchaseReturn.aspx.cs" Inherits="Reports_Return_ShowPurchaseReturn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
        .custom-grid {
            border-collapse: collapse;
            width: 100%;
            border: none;
            font-family: Arial, sans-serif;
            font-size: 12px;
        }

            .custom-grid th {
                padding: 8px;
                text-align: left;
                background-color: #f2f2f2;
                border: none;
                border-top: solid 1px;
                border-bottom: solid 1px;
            }

            .custom-grid td {
                padding: 2px 4px;
                border: none;
                vertical-align: top;
            }

            .custom-grid tr td {
                border: none;
            }

            .custom-grid label {
                margin: 0;
                padding: 0;
                display: inline-block;
                font-size: 8px;
            }
    </style>
    <div class="row p-t-10">
        <div class="col-md-1">
            <asp:Button ID="btn_back" CssClass="btn btn-primary" runat="server" Text="Back" OnClick="btn_back_Click" />
        </div>
    </div>
    <div id="printdetail" runat="server">
        <div id="print_div" style="width: 100%; padding: 30px 30px 30px 30px; box-sizing: border-box; margin: auto;">
            <div id="bill_format_1" style="margin-bottom: 50px; box-sizing: border-box; padding: 5px;">
                <table style="width: 100%">
                    <tr>
                        <th>
                            <h1><b>
                                Debit Note
                                </b>
                            </h1>

                        </th>
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

                <table style="width: 100%; min-height: 500px">
                    <tr>
                        <td rowspan="4" style="height: 500px; vertical-align: top">
                            <asp:GridView ID="gridReturnInvoice" runat="server" CssClass="custom-grid" AutoGenerateColumns="False" Width="100%">
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
                                <tr id="trInvRoundOff" runat="server" visible="false">
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

