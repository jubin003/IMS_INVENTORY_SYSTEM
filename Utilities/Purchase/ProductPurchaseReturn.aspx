<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ProductPurchaseReturn.aspx.cs" Inherits="Utilities_Purchase_ProductPurchaseReturn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
            //   printWindow.close();
        }

    </script>
    <div runat="server" id="divHide">
        <div class="container-fluid form-group-sm">
            <asp:Label ID="lblOldPK_ID" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblAgentID" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblAreaId" runat="server" Text="" Visible="false"></asp:Label>
            <table style="width: 100%; border-bottom: solid 1px;">

                <tr>
                    <td style="width: 700px; padding-right: 5px;">
                        <table style="width: 100%">
                            <tr>
                                <td>Debit Note Date<br />
                                    <asp:TextBox ID="txtDebitNoteDate" runat="server" CssClass="form-control" Enabled="false" Width="150px"></asp:TextBox>
                                </td>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td>Dakhila No
                                        <br />
                                                <asp:TextBox ID="txtDakhilaNo" runat="server" CssClass="form-control" Enabled="false" BackColor="#ff99ff"
                                                    Font-Bold="true" Font-Size="Medium" ForeColor="#000000" Width="150px"></asp:TextBox></td>
                                            <td>Dakhila Date
                                            <br />
                                                <asp:TextBox ID="txtDakhilaDate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>Sales Type
                            <br />
                                    <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control" Enabled="false">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr runat="server" id="divSupplier" visible="true">
                                <td>Supplier Code<br />
                                    <asp:TextBox ID="txtSupplierCode" runat="server" CssClass="form-control"
                                        Width="150px" Enabled="false">
                                    </asp:TextBox>
                                </td>
                                <td colspan="2">Supplier Name<br />
                                    <asp:DropDownList ID="ddlSupplier" runat="server" Width="450px"
                                        Font-Size="Larger" Enabled="false">
                                    </asp:DropDownList>
                                    <script>
                                        $('#<%=ddlSupplier.ClientID%>').chosen();
                                    </script>
                                    &nbsp;Walk in&nbsp;
                                <asp:CheckBox ID="chkSuppliers" runat="server" Enabled="false" /></td>
                            </tr>
                            <tr runat="server" id="divWalkinSupplier" visible="false">
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>Supplier Name
                                            <br />
                                                <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" Width="600px"></asp:TextBox>
                                            </td>
                                            <td>&nbsp;&nbsp;Walk in&nbsp;
                                            <asp:CheckBox ID="chkWalkIn" runat="server" Enabled="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>PAN/VAT No
                                <br />
                                    <asp:TextBox ID="txtSupplierPANVAT" runat="server" CssClass="form-control" Enabled="false" Width="150px"></asp:TextBox>
                                </td>
                                <td>Address<br />
                                    <asp:TextBox ID="txtSupplierAddress" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </td>

                                <td>Contact<br />
                                    <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Invoice Number<br />
                                    <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="form-control" Enabled="false" Width="150px"></asp:TextBox>
                                </td>
                                <td>Invoice Date<br />
                                    <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control datepicker" Width="150px" Enabled="false"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 500px; padding-left: 15px; border-left: dashed 1px; vertical-align: top;">
                        <table>
                            <tr runat="server" id="notetype" visible="false">
                                <td>Debit Note Type<br />
                                    <asp:DropDownList ID="ddlDNType" runat="server" CssClass="form-control" Width="150px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Return Note<br />
                                    <asp:TextBox ID="txtReturnNote" runat="server" TextMode="MultiLine" CssClass="form-control" Width="500px" Height="120px"></asp:TextBox>
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
                            <div style="background-color: cadetblue; text-align: center; width: 875px"><b>Purchase Detail </b></div>
                            <asp:GridView ID="grdPurchaseDetail" runat="server" AutoGenerateColumns="False" Width="875px"
                                CssClass="gridtable" OnRowDataBound="grdPurchaseDetail_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sno">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                            <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductCode" runat="server" Text='<%# Bind("PRODUCT_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" />
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
                                            <asp:Label ID="lblExpDate" runat="server" Text='<%# Bind("EXPIRY_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Bind("QUANTITY") %>'></asp:Label>
                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("UNIT") %>'></asp:Label>
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
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemTotal" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Return Total">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemRTotal" runat="server" Text='<%# Bind("R_TOTAL") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
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
                                <td style="width: 90px;">Total</td>
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
                                <td>Sub Total</td>
                                <td></td>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblTotalAmount" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>VAT</td>
                                <td>
                                    <asp:TextBox ID="txtVATPercent" runat="server" Width="35px" Text="13"></asp:TextBox>
                                    %</td>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblVAT" runat="server" Text=""></asp:Label></td>
                            </tr>
                            <tr>

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
                                    <asp:TextBox ID="txtRound" runat="server" Width="80px" Text="" Style="text-align: right"
                                        AutoPostBack="true" OnTextChanged="txtRound_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Total</td>
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
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" Width="100%" OnClick="btnSavePurchase_Click" /></td>
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
    </div>
    <div runat="server" id="divShow" visible="false">
        <asp:Button ID="btnBack" runat="server" Text="Get Back to Purchase Return" CssClass="btn btn-success" OnClick="btnBack_Click" />
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
                            background-color: #f2f2f2;
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


                 <table style="width: 100%; ">
                    <tr>
                        <td  style="vertical-align: top">
                            <asp:GridView ID="gridReturnInvoice" runat="server" CssClass="custom-grid"
                                 AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                     <asp:TemplateField HeaderText="S.N">
                                        <ItemStyle  Width="5%" />
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
                                        <ItemStyle HorizontalAlign="Center"  Width="5%" />
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

