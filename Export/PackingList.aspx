<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PackingList.aspx.cs" Inherits="ExportBackup_PackingList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function printPartOfPage() {
            var printContent = document.getElementById('Paking_List_Print');
            var printWindow = window.open('about:blank', 'Print' + new Date().getTime(), 'left=0,top=0,width=800,height=600');

            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();

            var pg = printWindow.document.getElementById('divPackingGridOnly');
            if (pg) { pg.style.display = 'block'; }

            var header = printWindow.document.getElementById('printOnlyHeader');
            var footer = printWindow.document.getElementById('divFooterDetails');
            if (header && footer) {
                header.style.display = 'block';
                footer.style.display = 'block';
            }

            printWindow.focus();
            printWindow.print();
            return false;
        }
        function printSticker() {
            var printContent = document.getElementById('divPrintSticker');
            var printWindow = window.open('about:blank', 'Print' + new Date().getTime(), 'left=0,top=0,width=600,height=600');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            return false;
        }
    </script>
    <div class="container">
        <asp:Label ID="lblSALES_PK_ID" Visible="false" runat="server"></asp:Label>
        <table>
            <tr>
                <td class="col-2">
                    <asp:Label ID="lblFY" Text="Fiscal Year" runat="server"></asp:Label>
                    <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlFiscalYear_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </td>
                <td class="col-md-1"></td>
                <td class="col-2">
                    <asp:Label ID="lblInvoiceNumber" Text="Invoice No." runat="server"></asp:Label>
                    <asp:TextBox ID="txtInvoiceNo" runat="server" AutoPostBack="true" OnTextChanged="txtInvoiceNo_TextChanged" CssClass="form-control"></asp:TextBox>

                </td>
                <td class="col-md-2">
                    <asp:Button ID="btnprint" Text="Print" OnClick="btnprint_Click" runat="server" CssClass="btn btn-primary" />
                </td>
            </tr>
        </table>


    </div>
    <div id="divGrid" runat="server" style="display: block;">
        <div id="print_div_packinglist" style="width: 210mm; padding: 30px 30px 30px 30px; box-sizing: border-box; auto;">
            <div id="Paking_List_Print" style="50px; box-sizing: border-box; padding: 5px; calibri">
                <div id="printOnlyHeader" style="display: none;">
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
                                            <asp:Label ID="lblPhone1" runat="server" Text="" Style="font-size: 12pt;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;" style="height: 22px">
                                            <asp:Label ID="Label1" runat="server" Style="font-size: 12pt; font-weight: normal; verdana;" Text="PAN NO"></asp:Label>
                                            <asp:Label ID="lblPanNo" runat="server" Style="font-size: 12pt; font-weight: normal; verdana;"></asp:Label>

                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 15%; text-align: center; vertical-align: top;"></td>
                        </tr>

                        <tr style="padding-top: 10px; padding-bottom: 10px;">
                            <td style="text-align: center; font-weight: bold; verdana; font-size: 22px" colspan="3">
                                <asp:Label ID="lblInvoiceHeading" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>

                    </table>
                    <table style="width: 100%; font-size: 13pt">
                        <tr>
                            <td colspan="2">
                                <asp:Image ID="imgn" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td style="width: 40%; text-align: right">Tran. Date:<asp:Label ID="lblTranNepaliDate" runat="server"></asp:Label>
                                [<asp:Label ID="lblTranDate" runat="server"></asp:Label>]</td>
                        </tr>
                        <tr>
                            <td >Invoice No.</td>
                            <td style="width: 45%;">:
                            <asp:Label Font-Size="14pt" Font-Bold="true" ID="lblInvoiceNo" runat="server"></asp:Label>
                            </td>
                            <td style="text-align: right">Bill Date &nbsp;:<asp:Label ID="lblBillNepaliDate" runat="server"></asp:Label>
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
                         <asp:Label ID="lblCustomerAddress" runat="server"></asp:Label>,<asp:Label ID="lblCustomerCountry" runat="server"></asp:Label></td>
                            <%--<td style="text-align: right"><span style="float: right">
                            <asp:Label ID="lblInvoiceHeading1" runat="server" Font-Bold="True"></asp:Label></span></td>--%>
                        </tr>
                       <%-- <tr>
                            <td>PAN No</td>
                            <td>:
                         <asp:Label ID="lblCustomerPanNo" runat="server"></asp:Label></td>
                            <%-- <td style="text-align: right">Mode of Payment :
                            <asp:Label ID="lblModeofPayment" runat="server"></asp:Label></td>
                        </tr>--%>
                    </table>
                </div>
                <style>
                    .custom-grid {
                        border-collapse: collapse;
                        width: 100%;
                        Arial, sans-serif;
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
                            border-left: 1px solid #000;
                            border-right: 1px solid #000;
                            border-top: none;
                            border-bottom: none;
                        }

                        .custom-grid th {
                            background-color: #f2f2f2;
                            border-bottom: 1px solid #000;
                            border-top: 1px solid #000;
                        }

                        .custom-grid label {
                            0;
                            padding: 0;
                            display: inline-block;
                            font-size: 18px;
                        }

                    .auto-style1 {
                        height: 20px;
                    }
                </style>

                <table style="width: 100%;">
                    <tr>
                        <td rowspan="4" style="vertical-align: top">
                           <div id="divPackingGridOnly" runat="server" ClientIDMode="Static" style="display: none;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td rowspan="4" style="vertical-align: top">
                                            <asp:GridView ID="gridPackingList" runat="server" CssClass="custom-grid" AutoGenerateColumns="False" Width="100%" OnRowDataBound="gridPackingList_RowDataBound" OnRowCommand="gridPackingList_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Packing No." ItemStyle-Width="5%">
                                                        <ItemStyle Height="8%" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBoxNoPL" runat="server" Text='<%# Bind("PACKING_NO") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="DESCRIPTION" ItemStyle-Width="52%" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("PRODUCT_ID") %>' ID="lblProNamePL" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="QUANTITY" Visible="true">
                                                        <ItemStyle Height="8%" Width="5%" />
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("QTY") %>' ID="lblQTYPL" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Total Quantity" ItemStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalQtyPL" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </td>
                    </tr>
                </table>
                <div id="divFooterDetails" class="divFooterDetails" style="display: none;">

                    <table style="float: right;">
                        <tr>
                            <td style="text-align: right;">For:
                            <asp:Label ID="lblCompanyNameL" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div id="divPrintSticker" style="display: none;">
        <table style="width: 100mm; border-collapse: collapse; font-size: 16pt;">
            <tr>
                <td style="border: 1px solid #000; padding: 6px; vertical-align: top;">

                    <table style="width: 100%; border-collapse: collapse; font-size: 20pt;">

                        <tr>
                            <td colspan="2" style="padding-bottom: 5px;">
                                <asp:Image ID="Image1" runat="server" Width="90mm" />
                            </td>
                        </tr>
                        <tr>
                            <td>Invoice No</td>
                            <td>:                           
                                <asp:Label ID="lblProInvoiceNo" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Packing No.</td>
                            <td>:                           
                             <b>
                                 <asp:Label ID="lblPackingNo" runat="server"></asp:Label></b>
                            </td>
                        </tr>
                        <tr>
                            <td>Pc Count</td>
                            <td>:                           
                                <asp:Label ID="lblPcCount" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; border-top: 1px solid #000; border-left: 1px solid #000; border-right: 1px solid #000; padding: 6px; vertical-align: top;">
                    <div style="font-weight: 700; text-transform: uppercase;">
                        <asp:Label ID="lblProCompanyName" runat="server"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblProCompanyAddress" runat="server"></asp:Label>
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


            </tr>

            <tr>
                <td style="width: 100%; border-top: 1px solid #000; border-left: 1px solid #000; border-right: 1px solid #000; border-bottom: 1px solid #000; padding: 6px; vertical-align: top;">
                    <div style="text-decoration: underline; font-weight: 600;">Invoice To</div>
                    <div>
                        <asp:Label ID="lblProCustomerName" runat="server"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblProCustomerAddress" runat="server"></asp:Label>,<asp:Label ID="lblProCustomerCountry" runat="server"></asp:Label>
                    </div>
                </td>

            </tr>
        </table>
    </div>
    <div id="divPackPrint" runat="server" style="display: block;">
        <table>
            <tr>
                <td rowspan="4" style="vertical-align: top">
                    <asp:GridView ID="grdPck" runat="server" CssClass="custom-grid" AutoGenerateColumns="False" Width="100%" OnRowDataBound="gridPackingList_RowDataBound" OnRowCommand="gridPackingList_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Packing No." ItemStyle-Width="5%">
                                <ItemStyle Height="8%" />
                                <ItemTemplate>
                                    <asp:Label ID="lblBoxNoPL" runat="server" Text='<%# Bind("PACKING_NO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="DESCRIPTION" ItemStyle-Width="52%" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Bind("PRODUCT_ID") %>' ID="lblProNamePL" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="QUANTITY" Visible="true">
                                <ItemStyle Height="8%" Width="5%" />
                                <ItemTemplate>
                                    <asp:Label Text='<%# Bind("QTY") %>' ID="lblQTYPL" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Total Quantity" ItemStyle-Width="10%">
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalQtyPL" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="" ItemStyle-Width="10%"
                                ItemStyle-CssClass="no-print"
                                HeaderStyle-CssClass="no-print">
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Button ID="btnPrint" CssClass="btn btn-sm btn-primary"
                                        Text="Print"
                                        runat="server"
                                        Visible="false"
                                        CommandName="printClick"
                                        CommandArgument='<%# Bind("PACKING_NO") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
