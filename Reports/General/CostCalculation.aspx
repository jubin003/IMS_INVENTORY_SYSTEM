<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CostCalculation.aspx.cs" Inherits="Reports_General_CostCalculation" EnableEventValidation="false" %>

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
    <div class="container-fluid">
        <div class="row" id="divBranch" runat="server">
            <div class="col-md-3">
                Branch
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
            <div class="col-md-1">
                Fiscal
                Year<br />
                <asp:TextBox ID="txtFiscalYear" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnShow" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnShow_Click" />
            </div>

        </div>
        <asp:GridView ID="grdReport" runat="server" AutoGenerateColumns="False" CssClass="gridtable" Width="100%" OnRowDataBound="grdReport_RowDataBound" OnRowCommand="grdReport_RowCommand">
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
                <asp:TemplateField HeaderText="Cost Calculation">
                    <ItemTemplate>
                        <asp:Button ID="btnCalculate" runat="server" Text="Calculate" CssClass="btn btn-primary" CommandName="Calculate" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Label ID="lblPPNNumber" runat="server" Text="" Visible="false"></asp:Label>
        <div id="divhide" runat="server" visible="false">

            <div id="print_div">
                <asp:GridView ID="grdCostCalculation" runat="server" AutoGenerateColumns="False" Width="100%"
                    CssClass="gridtable" OnRowDataBound="grdCostCalculation_RowDataBound" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                <asp:Label ID="lblPP_Number" runat="server" Text='<%# Bind("PP_NUMBER") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblPurchaseMaster_id" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Name">
                            <FooterTemplate>
                                <b>Total</b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("PRODUCT_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit Price">
                            <ItemTemplate>
                                <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Bind("FC_RATE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty">
                            <ItemTemplate>
                                <asp:Label ID="lblQty" runat="server" Text='<%# Bind("QUANTITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit">
                            <ItemTemplate>
                                <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("UNIT_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalFCAmount" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("FC_SUB_TOTAL") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Conversion Rate">
                            <ItemTemplate>
                                <asp:Label ID="lblConversionRate" runat="server" Text='<%# Bind("CONVERSION_RATE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total NRS">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalNRSAmount" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTotalNRS" runat="server" Text='<%# Bind("SUB_TOTAL") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Other Cost Up To Border">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalOtherCostUpToBorder" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOtherCostUpToBorder" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblTotalOtherCostUpToBorder" runat="server" Text='' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Custom Service">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalCustomService" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCustomService" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblTotalCustomService" runat="server" Text='' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cargo Vehicle Fee">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalCargoVehicleFee" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCargoVehicleFee" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblTotalCargoVehicleFee" runat="server" Text='' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Import Duty">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalImportDuty" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblImportDuty" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblTotalImportDuty" runat="server" Text='' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Extra Import Duty">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalOtherImportDuty" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOtherImportDuty" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblTotalOtherImportDuty" runat="server" Text='' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Excise Duty">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalExciseDuty" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblExciseDuty" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblTotalExciseDuty" runat="server" Text='' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nepal Freight Charge">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalNepalFreightCharge" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblNepalFreightCharge" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblTotalNepalFreightCharge" runat="server" Text='' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Insurance">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalInsurance" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblInsurance" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblTotalInsurance" runat="server" Text='' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bank Charge">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalBankCharge" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBankCharge" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblTotalBankCharge" runat="server" Text='' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Loading & Unloading Charges & Carriage Inward">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalLoadingUnloadingCharges" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLoadingUnloadingCharges" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblTotalLoadingUnloadingCharges" runat="server" Text='' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Other Cost">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalOtherCost" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOtherCost" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblTotalOtherCost" runat="server" Text='' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VAT On Import">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalVATOnImport" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVATOnImport" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblTotalVATOnImport" runat="server" Text='' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Additional Cost">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalAdditionalCost" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTotalAdditionalCost" runat="server" Text=''></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Cost">
                            <FooterTemplate>
                                <b>
                                    <asp:Label ID="lblTotalCost" runat="server" Text=""></asp:Label></b>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTotalCost" runat="server" Text=''></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cost Per Unit With Out VAT">
                            <ItemTemplate>
                                <asp:Label ID="lblCostPerUnitWithOutVAT" runat="server" Text=''></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>
        <div class="row" id="divBtn" runat="server" visible="false">
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrint_Click" />
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnExport" runat="server" Text="Excel" CssClass="btn btn-primary" OnClick="btnExport_Click" />
            </div>
        </div>
    </div>
</asp:Content>

