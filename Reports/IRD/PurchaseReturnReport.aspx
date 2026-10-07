<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PurchaseReturnReport.aspx.cs"
     Inherits="Reports_IRD_PurchaseReturnReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">

        function printPartOfPage() {
            var printContent = document.getElementById('print_div');
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
     <div class="row" id="divBranch" runat="server">
        <div class="col-md-4">
            Branch<br />
            <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
    </div>
    <table style="width:1000px">
        <tr>
            <td>From Date:</td>
            <td>
                <asp:TextBox ID="txtFromDate" AutoComplete="off" runat="server" CssClass="form-control datepicker"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
            <td>To Date:</td>
            <td>
                <asp:TextBox ID="txtToDate" AutoComplete="off" runat="server" CssClass="form-control datepicker"></asp:TextBox>
            </td>

            <td>
                <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" CssClass="btn btn-success" />
            </td>
            <td>
                <asp:ImageButton ID="btnPrint" runat="server" Width="50px" ImageUrl="~/images/icons/print.png" OnClick="btnPrint_Click" />
                &nbsp;
                 <asp:ImageButton ID="btnExcel" runat="server" OnClick="btnExcel_Click" Width="50px" ImageUrl="~/images/icons/excel.png" />

                &nbsp;
                 <asp:ImageButton ID="btnJson" runat="server" Width="50px" ImageUrl="~/images/icons/json.png" OnClick="btnJson_Click" />
                &nbsp;
                 <asp:ImageButton ID="btnXml" runat="server" Width="50px" ImageUrl="~/images/icons/xml.png" OnClick="btnXml_Click" />
            </td>
        </tr>

    </table>
    <br>
    <div id="hide" runat="server" visible="false">
        <div id="print_div">
            <table style="width: 100%; text-align: center;">
                <tr>
                    <td>
                        <span style="font-weight: bold; font-size: 16pt;">
                            <asp:Label ID="lblCompanyName" runat="server" Text=""></asp:Label></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span style="font-size: 12pt;">
                            <asp:Label ID="lblCompanyAddress" runat="server" Text=""></asp:Label></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span style="font-size: 12pt;">VAT/PAN No: 
                                <asp:Label ID="lblPanNo" runat="server" Text=""></asp:Label></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span style="font-size: 12pt;">Registration No: 
                                <asp:Label ID="lblRegNo" runat="server" Text=""></asp:Label></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span style="font-size: 12pt;">From Date: 
                                <asp:Label ID="lblFromDate" runat="server" Text=""></asp:Label>
                            - To Date: 
                                <asp:Label ID="lblToDate" runat="server" Text=""></asp:Label></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span style="font-size: 16px; font-weight: 700; text-align: center;">DEBIT NOTE</span></td>
                </tr>
                <tr>
                    <td style="text-align: center; font-family: Verdana; font-size: 14px">&nbsp;</td>
                    <td style="text-align: right; font-size: 10pt">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gridIRDReport" runat="server" CssClass="gridtable"
                            AutoGenerateColumns="False" EnableModelValidation="True" Width="100%">
                            <Columns>
                                  <asp:TemplateField HeaderText="Debit Note Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDNDate" runat="server" Text='<%# Bind("NOTE_DATE_NP") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Debit Note Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDNDate" runat="server" Text='<%# Bind("DEBIT_NOTE_NUMBER") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                               <asp:TemplateField HeaderText="Ref. Bill Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillDate" runat="server" Text='<%# Bind("INVOICE_DATE_NP") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ref. Bill No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNo" runat="server" Text='<%# Bind("SUPPLIER_INVOICE_NO") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupplierName" runat="server" Text='<%# Bind("SUPPLIER_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier PAN/VAT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupplierPan" runat="server" Text='<%# Bind("SUPPLIER_PAN_VAT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                               
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("TOTAL_AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiscount" runat="server" Text='<%# Bind("DISCOUNT_AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Taxable Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxableAmount" runat="server" Text='<%# Bind("TAXABLE_AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%# Bind("TAX_VAT_AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Bind("RETURN_AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>                                
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>

        </div>
    </div>
</asp:Content>

