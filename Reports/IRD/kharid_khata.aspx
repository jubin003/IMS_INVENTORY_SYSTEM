<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="kharid_khata.aspx.cs" Inherits="Reports_IRD_kharid_khata" %>

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
            printWindow.close();
        }
    </script>
    <div class="container-fluid">
        <div class="row" id="divBranch" runat="server">
            <div class="col-md-4">
                Branch<br />
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Month<br />
                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control">
                    <asp:ListItem Value="1">Baishak</asp:ListItem>
                    <asp:ListItem Value="2">Jestha</asp:ListItem>
                    <asp:ListItem Value="3">Asar</asp:ListItem>
                    <asp:ListItem Value="4">Shrawan</asp:ListItem>
                    <asp:ListItem Value="5">Bhadra</asp:ListItem>
                    <asp:ListItem Value="6">Aswin</asp:ListItem>
                    <asp:ListItem Value="7">Kartik</asp:ListItem>
                    <asp:ListItem Value="8">Manshir</asp:ListItem>
                    <asp:ListItem Value="9">Poush</asp:ListItem>
                    <asp:ListItem Value="10">Magh</asp:ListItem>
                    <asp:ListItem Value="11">Fagun</asp:ListItem>
                    <asp:ListItem Value="12">Chaitra</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2" >
                Fiscal Year<br />
                <asp:TextBox ID="txtFiscalYear" runat="server" CssClass="form-control" ></asp:TextBox>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnShow" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnShow_Click" />
            </div>
            <div class="col-md-1">
                <asp:ImageButton ID="ImageButton1" runat="server" OnClick="ImageButton1_Click" Width="50px" ImageUrl="~/images/icons/excel.png" />
            </div>
        </div>
        <div id="hide" runat="server" visible="false">
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
                        <span style="font-size: 12pt;">Month: 
                                <asp:Label ID="lblMonth" runat="server" Text=""></asp:Label>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span style="font-weight: bold; font-size: 16pt;">Kharid Khata</span></td>

                </tr>
                <tr>
                    <td style="text-align: center; font-family: Verdana; font-size: 14px">&nbsp;</td>
                    <td style="text-align: right; font-size: 10pt">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" CssClass="gridtable" OnRowDataBound="grdReport_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("DATE") %>'></asp:Label>
                                        <asp:Label ID="lblPurchaseMasterID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("SUPPLIER_INVOICE_NO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PP No No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPPNo" runat="server" Text='<%# Bind("PP_NUMBER") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Suppliers Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSuppliersName" runat="server" Text='<%# Bind("SUPPLIER_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Suppliers PAN/VAT No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSuppliersPANVATNo" runat="server" Text='<%# Bind("SUPPLIER_PAN_VAT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Detail">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductDetail" runat="server" Text=''></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server" Text=''></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnit" runat="server" Text=''></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exempted">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExempted" runat="server" Text='<%# Bind("EXEMPTED") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Taxable Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxableAmount" runat="server" Text='<%# Bind("SUB_TOTAL_AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TAX">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTAX" runat="server" Text='<%# Bind("TAX_VAT_AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Import Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblImportAmount" runat="server" Text='<%# Bind("IMPORT_AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Import Tax">
                                    <ItemTemplate>
                                        <asp:Label ID="lblImportTax" runat="server" Text='<%# Bind("IMPORT_VAT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Purchase Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetPurchaseAmount" runat="server" Text=''></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Tax">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetTax" runat="server" Text=''></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

