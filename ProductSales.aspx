<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProductSales.aspx.cs" Inherits="ProductSales" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function printPartOfPage() {
            var printContent = document.getElementById('bill_format_1');
            var printWindow = window.open('about:blank', 'Print' + new Date().getTime(), 'left=0,top=0,width=800,height=600');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            return false;
        }

        

        function printTOKand80mm() {
            var tokContent = document.getElementById('bill_format_tok_simple');
            var billContent = document.getElementById('bill_format_80mm');

            var printWindow = window.open('about:blank', 'PrintCombined' + new Date().getTime(), 'left=0,top=0,width=400,height=600');

            printWindow.document.write('<style>');
            printWindow.document.write('@page { size: 80mm auto; margin: 0mm; }');
            printWindow.document.write('body { margin: 0; padding: 5px; font-family: Consolas; font-size: 13px; }');
            printWindow.document.write('table { width: 100%; border-collapse: collapse; font-family: Consolas; font-size: 13px; }');
            printWindow.document.write('th { border-top: 1px solid #000 !important; border-bottom: 1px solid #000 !important; border-left: none !important; border-right: none !important; padding: 2px; }');
            printWindow.document.write('td { padding: 2px; border: none !important; }');
            printWindow.document.write('.td-border-top { border-top: 1px solid #000 !important; }');
            printWindow.document.write('.td-border-bottom { border-bottom: 1px solid #000 !important; }');
            printWindow.document.write('.grid80mm { width: 100%; border-collapse: collapse; }');
            printWindow.document.write('.grid80mm thead tr th { border-top: 1px solid #000; border-bottom: 1px solid #000; padding: 2px; border-left: none; border-right: none; }');
            printWindow.document.write('.grid80mm tbody tr td { padding: 2px; border: none; }');
            printWindow.document.write('.page-break { page-break-before: always; border-top: 1px dashed #000; margin: 6px 0; }');
            printWindow.document.write('</style>');

            // TOK first — use outerHTML so the wrapper's border/padding print too
            printWindow.document.write(tokContent.outerHTML);

            // separator / page break, then 80mm bill right after
            printWindow.document.write('<div class="page-break"></div>');
            printWindow.document.write(billContent.innerHTML);

            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            return false;
        }

        function printBill80mmOnly() {
            var billContent = document.getElementById('bill_format_80mm');
            var printWindow = window.open('about:blank', 'Print80mmOffice' + new Date().getTime(), 'left=0,top=0,width=400,height=600');

            printWindow.document.write('<style>');
            printWindow.document.write('@page { size: 80mm auto; margin: 0mm; }');
            printWindow.document.write('body { margin: 0; padding: 5px; font-family: Consolas; font-size: 13px; }');
            printWindow.document.write('table { width: 100%; border-collapse: collapse; font-family: Consolas; font-size: 13px; }');
            printWindow.document.write('th { border-top: 1px solid #000 !important; border-bottom: 1px solid #000 !important; border-left: none !important; border-right: none !important; padding: 2px; }');
            printWindow.document.write('td { padding: 2px; border: none !important; }');
            printWindow.document.write('.td-border-top { border-top: 1px solid #000 !important; }');
            printWindow.document.write('.td-border-bottom { border-bottom: 1px solid #000 !important; }');
            printWindow.document.write('.grid80mm { width: 100%; border-collapse: collapse; }');
            printWindow.document.write('.grid80mm thead tr th { border-top: 1px solid #000; border-bottom: 1px solid #000; padding: 2px; border-left: none; border-right: none; }');
            printWindow.document.write('.grid80mm tbody tr td { padding: 2px; border: none; }');
            printWindow.document.write('</style>');
            printWindow.document.write(billContent.innerHTML);

            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            return false;
        }

        function printOfficeCopyAnd80mm() {
            // A4 office copy first (unchanged existing behaviour)
            //printPartOfPage();
            // then the 80mm copy in its own print window, slightly delayed so the two print dialogs don't collide
            setTimeout(function () { printBill80mmOnly(); }, 700);
            return false;
        }

        function printProforma() {
            var printContent = document.getElementById('bill_format_proforma');
            var printWindow = window.open('about:blank', 'PrintProforma' + new Date().getTime(), 'left=0,top=0,width=800,height=600');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            return false;
        }

        // Proforma first, then the existing A4 tax invoice right after (used on Save)
        function printProformaThenA4() {
            printProforma();
            setTimeout(function () { printPartOfPage(); }, 700);
            return false;
        }

        // Proforma first, then the existing 80mm + TOK combo right after (used on Save, Continuous bill type)
        function printProformaThen80mmTOK() {
            printProforma();
            setTimeout(function () { printTOKand80mm(); }, 700);
            return false;
        }

        function printTOK() {
            var printContent = document.getElementById('bill_format_tok');
            var printWindow = window.open('about:blank', 'PrintTOK' + new Date().getTime(), 'left=0,top=0,width=400,height=600');
            printWindow.document.write('<style>');
            printWindow.document.write('@page { size: 80mm auto; margin: 0mm; }');
            printWindow.document.write('body { margin: 0; padding: 5px; font-family: Consolas; font-size: 13px; }');
            printWindow.document.write('table { width: 100%; border-collapse: collapse; }');
            printWindow.document.write('th { border-top: 1px solid #000 !important; border-bottom: 1px solid #000 !important; border-left: none !important; border-right: none !important; padding: 2px; }');
            printWindow.document.write('td { padding: 2px; border: none !important; }');
            printWindow.document.write('.grid80mm { width: 100%; border-collapse: collapse; }');
            printWindow.document.write('.grid80mm thead tr th { border-top: 1px solid #000; border-bottom: 1px solid #000; padding: 2px; border-left: none; border-right: none; }');
            printWindow.document.write('.grid80mm tbody tr td { padding: 2px; border: none; }');
            printWindow.document.write('</style>');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            return false;
        }
    </script>

    <div class="form-group-sm container-fluid">
        <table style="width: 100%; border-bottom: solid 1px;">
            <tr>
                <td style="width: 700px; padding-right: 5px;">
                    <table style="width: 700px" class="gridtable">
                        <tr>
                           <%-- <td>Walk In Customer 
                             <asp:CheckBox ID="chkWalkIn" runat="server" OnCheckedChanged="chkWalkIn_CheckedChanged" AutoPostBack="true" />
                            </td>--%>
                            <td>Transaction Date
                                <br />
                                <asp:TextBox ID="txtTransactionDate" runat="server" CssClass="form-control datepicker" Style="width: 200px"></asp:TextBox>
                            </td>
                            <td>Invoice Date
                                <br />
                                <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control" Enabled="false" Style="width: 200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="divExistingCustomer" runat="server">
                            <td>Customer Code
                                <br />
                                <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="form-control" Style="width: 300px"
                                    AutoPostBack="true" OnTextChanged="txtCustomerCode_TextChanged"></asp:TextBox>
                            </td>
                            <td colspan="2">Name
                             <br />
                                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control"
                                    Font-Size="Larger" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                                </asp:DropDownList>
                                <script>
                                    $('#<%=ddlCustomer.ClientID%>').chosen();
                                </script>
                            </td>
                        </tr>

                        <tr id="divWalkInCustomer" runat="server" visible="false">
                            <td colspan="3">Customer Name
                            <br />
                                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Address
                            <br />
                                <asp:TextBox ID="txtCustomerAddress" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                            </td>
                            <td>PAN/VAT No
                            <br />
                                <asp:TextBox ID="txtCustomerPANVAT" runat="server" CssClass="form-control" Style="width: 200px" Text="0"
                                    AutoPostBack="true" MaxLength="9" TextMode="Number" OnTextChanged="txtCustomerPANVAT_TextChanged"></asp:TextBox>
                            </td>
                            <td>Contact No<br />
                                <asp:TextBox ID="txtCustomerContact" runat="server" CssClass="form-control" Style="width: 200px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 300px; padding-left: 15px; padding-right: 15px; border-left: dashed 1px; vertical-align: top;">
                    <table style="width: 100%">
                        <tr>
                            <td>Sales Type
                            <br />
                                <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control"
                                    OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged" AutoPostBack="true">
                                     <asp:ListItem Text="Credit" Value="CR"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="padding-left: 8px" runat="server" visible="false">Invoice Type
                                <br />
                                <asp:DropDownList ID="ddlExempted" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlExempted_SelectedIndexChanged">
                                    <asp:ListItem Text="Tax Invoice" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Exempted Invoice" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="divBankDetail" runat="server" visible="false">
                            <td colspan="2">
                                <asp:Label ID="lblQRofBank" runat="server" Text="Bank"></asp:Label><br />
                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control"></asp:DropDownList>
                                <br />
                               
                            </td>
                        </tr>
                        <tr id="divCredit" runat="server" visible="false">
                            <td>Credit Limit
                            <br />
                                <asp:TextBox ID="txtCustomerCreditLimit" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="margin-left: 80px; padding-left: 8px">Balance
                            <br />
                                <asp:TextBox ID="txtCustomerBalance" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trRateType"
                            runat="server">
                            <td colspan="2">Product Rate Type:<asp:DropDownList ID="ddlProductRateType" CssClass="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProductRateType_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 300px; padding-left: 15px; border-left: dashed 1px; vertical-align: top;">
                    <table class="gridtable">
                        <tr>
                            <td runat="server" visible="false">Agent Nane
                            <br />
                                <asp:DropDownList ID="ddlAgentName" runat="server" CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdPO" runat="server" visible="false">Purchase Order Number<br />
                                <asp:TextBox ID="txtPONumber" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><%--Agent Area--%>
                                <br />
                                <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control" Enabled="false" Visible="false"></asp:DropDownList>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 1200px">
            <tr>
                <td>Code<br />
                    <asp:TextBox ID="txtProductCode" runat="server" CssClass="form-control" Width="80px"
                        AutoPostBack="true" OnTextChanged="txtProductCode_TextChanged"></asp:TextBox>
                </td>
                <td>Product Name<br />
                    <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control" Width="400px"
                        Font-Size="Larger" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged">
                    </asp:DropDownList>
                    <script>
                        $('#<%=ddlProduct.ClientID%>').chosen();
                    </script>
                </td>
                <td runat="server" id="divBatch" visible="false">Batch<br />
                    <asp:DropDownList ID="ddlBatch" Width="80px" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"></asp:DropDownList>
                    <asp:TextBox ID="txtExpDate" runat="server" Visible="false"></asp:TextBox>
                </td>
                <td runat="server" id="tdAvaiQty" visible="true">Available Qty<br />
                    <asp:TextBox ID="txtAvilableQty" runat="server" CssClass="form-control" Width="80px" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <br />
                    <asp:Label ID="lblUUnit" runat="server" Text=""></asp:Label>
                </td>
                <td id="tdDualUnit" runat="server" visible="false">Alternate Unit<br />
                    <asp:TextBox ID="txtUQty" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true" OnTextChanged="txtUQty_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <br />
                    <asp:Label ID="lblBUnit" runat="server" Text=""></asp:Label>
                </td>
                <td>Basic Unit<br />
                    <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true" OnTextChanged="txtQty_TextChanged"></asp:TextBox>
                </td>
                <td>Rate/Basic Unit<br />
                    <asp:TextBox ID="txtRate" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true" OnTextChanged="txtRate_TextChanged"></asp:TextBox>
                </td>
                <td>Amount<br />
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true" OnTextChanged="txtAmount_TextChanged"></asp:TextBox>
                </td>
                <td id="tdSchDisc" runat="server" visible="false">Sche Disc.
                    <br />
                    <asp:TextBox ID="txtScheDisc" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                </td>
                <td>
                    <br />
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
        <table style="border: solid; font-size: 12px;">
            <tr>
                <td style="width: 1000px">
                    <div style="height: 300px; width: 1000px; overflow: scroll; overflow-x: hidden; margin-top: 8px;">
                        <div style="background-color: cadetblue; text-align: center; width: 975px">
                            <b>Invoice Detail </b>
                        </div>
                        <asp:GridView ID="grdSalesDetail" runat="server" AutoGenerateColumns="False" Width="975px" CssClass="gridtable"
                            OnRowCommand="grdSalesDetail_RowCommand" Font-Size="11px">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                        <asp:Label ID="lblProductPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblTaxable" runat="server" Text='<%# Bind("TAXABLE") %>' Visible="false"></asp:Label>
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
                                        <asp:Label ID="lblExpDate" runat="server" Text='<%# Bind("EXPIRY_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Alternate Qty" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUQty" runat="server" Text='<%# Bind("U_QUANTITY") %>'></asp:Label>
                                        <asp:Label ID="lblUUnit" runat="server" Text='<%# Bind("U_UNIT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtGridQty" runat="server" Width="50px"
                                            Style="display: inline-block; text-align: right;"
                                            Text='<%# Bind("QUANTITY") %>' AutoPostBack="true" OnTextChanged="txtGridQty_TextChanged"></asp:TextBox>
                                        <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("UNIT") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRate" runat="server" Text='<%# Bind("RATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemTotal" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Disc" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblScheDisc" runat="server" Text='<%# Bind("SCHE_DISC") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAfterScheDisc" runat="server" Text='<%# Bind("AFTER_SCHE_DISC")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton2" CommandName="Remove" ImageUrl="~/images/icons/deletes.png" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" />
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>

                    </div>
                </td>
                <td style="vertical-align: top; padding-left: 5px; padding-right: 5px;">
                    <table style="width: 300px;">
                        <tr>
                            <td style="width: 90px;">Sub Total</td>
                            <td style="width: 45px;"></td>
                            <td style="text-align: right; width: 35px;">
                                <asp:Label ID="lblSubTotalAmount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 90px;">Sche. Disc</td>
                            <td style="width: 45px;">&nbsp;</td>
                            <td style="text-align: right; width: 35px;">
                                <asp:Label ID="lblTotalScheDisc" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 90px;">After Sch.Disc</td>
                            <td style="width: 45px;">&nbsp;</td>
                            <td style="text-align: right; width: 35px;">
                                <asp:Label ID="lblAfterSchDiscount" runat="server" Text=""></asp:Label>
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
                            <td>VAT</td>
                            <td>
                                <asp:TextBox ID="txtVATPercent" runat="server" Width="35px" Text="" AutoPostBack="true" OnTextChanged="txtVATPercent_TextChanged" ReadOnly="true"></asp:TextBox>
                                %</td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblVAT" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr runat="server" id="trVATReturn" visible="false">
                            <td>VAT Return</td>
                            <td>
                                <asp:Label ID="lblVATReturnPercent" runat="server" Text="10"></asp:Label>%</td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblVATReturn" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr runat="server" id="divVAT2">
                            <td>Grand Total</td>
                            <td></td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblGrandTotal" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr runat="server" visible="false" id="trRoundOff">

                            <td>Rounding</td>
                            <td></td>
                            <td style="text-align: right;">
                                <asp:TextBox ID="txtRound" runat="server" Width="80px" Text="" Style="text-align: right"
                                    AutoPostBack="true" OnTextChanged="txtRound_TextChanged" Enabled="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr visible="false" runat="server" id="trRtotal">
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
                                 <asp:Button ID="btnShowQR" runat="server" Text="Show QR" CssClass="btn btn-primary" Width="100%"
                                    Style="margin-bottom: 3px;" OnClick="btnShowQR_Click" CausesValidation="false" visible="false"/>
                                <br />
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" Width="100%" OnClick="btnSave_Click" /><br />
                                <br />
                                <asp:Button ID="btnOfficeCopy" runat="server" Text="Print Office Copy" CssClass="btn btn-primary" Width="100%" OnClick="btnOfficeCopy_Click" Visible="false" />
                                <br />
                                <br />
                                <asp:Button ID="btnPrintTOK" runat="server" Text="Print TOK" CssClass="btn btn-primary" Width="100%"
                                    OnClick="btnPrintTOK_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <strong>
                                    <asp:Label ID="lblerror" runat="server" Style="color: #CC0000"></asp:Label>
                                </strong>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        <table style="width: 100%">
            <tr>
                <td>Remarks
                    <br />
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
        </table>

        <%-- Proforma Invoice only fields. Not saved to the database - used only to populate the Proforma Invoice printout. --%>
        <table style="width: 100%">
            <tr>
                <td style="width: 20%;">Contract No
                    <br />
                    <asp:TextBox ID="txtContractNo" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
                <td style="width: 20%;">Contract Date
                    <br />
                    <asp:TextBox ID="txtContractDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                </td>
                <td style="width: 20%;">Payment Currency
                    <br />
                    <asp:TextBox ID="txtPaymentCurrency" runat="server" CssClass="form-control" Text="US Dollar"></asp:TextBox>
                </td>
                <td style="width: 20%;">Shipment Type
                    <br />
                    <asp:TextBox ID="txtShipmentType" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
                <td style="width: 20%;">Shipment No
                    <br />
                    <asp:TextBox ID="txtShipmentNo" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>

    <div>
        <asp:Label ID="lblunique_token" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblPK_ID" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblOrderNoHidden" runat="server" Text="" Visible="false"></asp:Label>
    </div>

    <div id="printdetail" runat="server" visible="false" style="margin-top: 1000px">
        <div id="print_div" style="width: 210mm; padding: 30px 30px 30px 30px; box-sizing: border-box; margin: auto;">
            <div id="bill_format_1" style="margin-bottom: 50px; box-sizing: border-box; padding: 5px; font-family: Calibri">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 20%; text-align: center; vertical-align: top;">
                            <asp:Image ID="sImage1" runat="server" Width="100%" ImageUrl="~/images/img.png" Height="156px" />
                        </td>
                        <td style="text-align: center;">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblCompanyName" runat="server" Text="" Style="font-weight: bold; font-size: 32px;"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblCompanyAddress" runat="server" Text="" Style="font-size: 18pt;"></asp:Label></td>
                                </tr>
                                <tr runat="server" id="divEmail">
                                    <td style="text-align: center;" class="auto-style1">
                                        <asp:Label ID="lblWebsite" runat="server" Text="" Style="font-size: 8pt;"></asp:Label>
                                        <asp:Label ID="lblDivider" runat="server" Text="|" Style="font-size: 8pt;"></asp:Label>
                                        <asp:Label ID="lblEmail" runat="server" Text="" Style="font-size: 8pt;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblPhone1" runat="server" Text="" Style="font-size: 8pt;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;" style="height: 20px">
                                        <asp:Label ID="Label1" runat="server" Style="font-size: 12pt; font-weight: normal; font-family: Verdana;" Text="PAN No."></asp:Label>
                                        <asp:Label ID="lblPanNo" runat="server" Style="font-size: 12pt; font-weight: normal; font-family: Verdana;"></asp:Label>

                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 20%; text-align: center; vertical-align: top;"></td>
                    </tr>

                    <tr style="padding-top: 8px; padding-bottom: 8px;">
                        <td style="text-align: center; font-weight: bold; font-family: Verdana; font-size: 18px" colspan="3">
                            <asp:Label ID="lblInvoiceHeading" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>

                </table>
                <table style="width: 100%; font-size: 10pt">
                    <tr>
                        <td colspan="3"></td>
                        <td style="width: 40%; text-align: right">Tran. Date:<asp:Label ID="lblTranNepaliDate" runat="server"></asp:Label>
                            [<asp:Label ID="lblTranDate" runat="server"></asp:Label>]</td>
                    </tr>
                    <tr>
                        <td>Invoice No.</td>
                        <td>:
                            <asp:Label Font-Size="10pt" Font-Bold="true" ID="lblInvoiceNo" runat="server"></asp:Label>
                        </td>
                        <td></td>
                        <td style="width: 40%; text-align: right">Bill Date &nbsp;:<asp:Label ID="lblBillNepaliDate" runat="server"></asp:Label>
                            [<asp:Label ID="lblBillEnglishDate" runat="server"></asp:Label>]
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">Name</td>
                        <td colspan="2" class="auto-style3">:                  
                         <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="">Address</td>
                        <td style="">:
                         <asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                        <td></td>
                        <td style="text-align: right"><span style="float: right">
                            <asp:Label ID="lblInvoiceHeading1" runat="server" Font-Bold="True"></asp:Label></span></td>
                    </tr>
                    <tr>
                        <td>PAN No</td>
                        <td>:
                         <asp:Label ID="lblCustomerPanNo" runat="server"></asp:Label></td>
                        <td></td>
                        <td style="text-align: right">Mode of Payment:
                            <asp:Label ID="lblModeofPayment" runat="server"></asp:Label></td>
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

                    .auto-style1 {
                        height: 20px;
                    }
                </style>


                <table style="width: 100%;">
                    <tr>
                        <td rowspan="4" style="vertical-align: top">
                            <asp:GridView ID="gridSalesInvoice" runat="server" CssClass="custom-grid" AutoGenerateColumns="False" Width="100%">
                                <Columns>
                                    <%-- <asp:TemplateField HeaderText="S.N">
                                        <ItemStyle Height="8%" Width="3%" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblSN" runat="server" Text='<%# Bind("SNO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Particular">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("PRODUCT_NAME") %>' ID="lblProNAme" runat="server" />
                                            <br />
                                            <asp:Label Text='<%# "H.S: " + Eval("HS_CODE") %>' ID="lblHSCode" runat="server"
                                                Style="font-size: 9px; color: #555;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Batch" Visible="false">
                                        <ItemStyle Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("BATCH_NO") %>' ID="lblBatchNo" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exp Date" Visible="false">
                                        <ItemStyle Height="8%" Width="9%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("EXPIRY_DATE") %>' ID="lblExpDate" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Upper Qty">
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
                                        <ItemStyle Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("RATE") %>' ID="lblRate" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemStyle HorizontalAlign="Right" Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("TOTAL") %>' ID="lblTotal" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sch.Disc">
                                        <ItemStyle HorizontalAlign="Right" Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblScheDisc" runat="server" Text='<%# Bind("SCHEME_DISCOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Taxable Amount">
                                        <ItemStyle HorizontalAlign="Right" Height="8%" Width="5%" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("TAXABLE_TOTAL") %>' ID="lblTaxableTotal" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
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
                            <br />
                        </td>
                        <td style="width: 40%; vertical-align: top;" rowspan="3">
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
                                <tr runat="server" id="trTaxableAmt">
                                    <td style="border-top: 1px solid black; padding-left: 3%">Taxable amount</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblTaxableAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trInvVat" runat="server">
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
                                <tr id="trInvVATReturn" runat="server" visible="false">
                                    <td style="border-top: 1px solid black; padding-left: 3%">VAT Return</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblInvVATReturn" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trInvRoundOff" runat="server" visible="false">
                                    <td style="border-top: 1px solid black; padding-left: 3%">Round off</td>
                                    <td style="border-top: 1px solid black; text-align: right; padding-right: 5px">
                                        <asp:Label ID="lblRoundoff" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trNetAmount">
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
                            <br />
                            <table runat="server" id="divPO" visible="true">
                                <tr>
                                    <td>PO Number :
                                        <asp:Label ID="lblPONumber" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 60%; vertical-align: top;">
                            <span style="font-size: small">
                                <br />
                                * Please Check the goods before receiving,
                                <br />
                                once sold goods are not returnable
                                 <br />
                                * E.& O.E.            
                                <br />
                                <asp:Label ID="lblRemarks" runat="server" Text=""></asp:Label>
                            </span>

                        </td>
                    </tr>
                </table>
                <table style="width: 100%; font-size: 8pt">
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
                        <td style="font-style: italic" colspan="2">
                            <asp:Label Font-Size="8px" ID="Label4" runat="server" Text="Print Date Time:"></asp:Label>
                            <asp:Label Font-Size="8px" ID="lblTime" runat="server"></asp:Label>
                            &nbsp;<asp:Label Font-Size="8px" ID="Label2" runat="server" Text="Print By:"></asp:Label>
                            <asp:Label Font-Size="8px" ID="lblPrintedBy" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%-- 80MM PRINT FORMAT --%>
    <div id="printdetail80" runat="server" visible="false" style="margin-top: 1000px;">
        <div id="bill_format_80mm" style="width: 80mm; font-family: 'Consolas'; font-size: 13px; font-weight: 100; padding: 5px; box-sizing: border-box;">
            <div style="text-align: center;">
                <asp:Label ID="lblCompanyName80" runat="server" Style="font-weight: bold; font-size: 17px;"></asp:Label><br />
                <asp:Label ID="lblCompanyAddress80" runat="server"></asp:Label><br />
                Contact No:
                <asp:Label ID="lblPhone80" runat="server"></asp:Label><br />
                Vat No:
                <asp:Label ID="lblPanNo80" runat="server"></asp:Label>
            </div>
            <div style="text-align: center; margin: 4px 0;">
                --------------------------------<br />
                <asp:Label ID="lblInvoiceHeading80" runat="server" Style="font-weight: bold;"></asp:Label><br />
                --------------------------------
            </div>
            <div>
                Bill No:
                <asp:Label ID="lblInvoiceNo80" runat="server"></asp:Label><br />
                Bill Date:
                <asp:Label ID="lblBillNepaliDate80" runat="server"></asp:Label>
                [<asp:Label ID="lblBillEnglishDate80" runat="server"></asp:Label>]<br />
                Tran. Date:
                <asp:Label ID="lblTranNepaliDate80" runat="server"></asp:Label>
                [<asp:Label ID="lblTranDate80" runat="server"></asp:Label>]
            </div>
            <div>
                Name:
                <asp:Label ID="lblCustomerName80" runat="server"></asp:Label><br />
                PAN No:
                <asp:Label ID="lblCustomerPanNo80" runat="server"></asp:Label><br />
                Payment:
                <asp:Label ID="lblModeofPayment80" runat="server"></asp:Label>
            </div>
            <asp:GridView ID="gridSalesInvoice80" runat="server" AutoGenerateColumns="False"
                Width="100%" ShowHeader="true" GridLines="None" CssClass="grid80mm" OnRowDataBound="gridSalesInvoice80_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="SN">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="lblSN80" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PARTICULAR">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label Text='<%# Bind("PRODUCT_NAME") %>' ID="lblProName80" runat="server" />
                            <br />
                            <asp:Label Text='<%# "H.S: " + Eval("HS_CODE") %>' ID="lblHSCode80" runat="server"
                                Style="font-size: 10px;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="QTY">
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label Text='<%# Bind("QUANTITY") %>' ID="lblQty80" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RATE">
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label Text='<%# Bind("RATE") %>' ID="lblRate80" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AMT">
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label Text='<%# Bind("TOTAL") %>' ID="lblTotal80" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <table style="width: 100%; font-size: 13px;">
                <tr>
                    <td style="text-align: right;" class="td-border-top">Sub Total:</td>
                    <td style="text-align: right;" class="td-border-top">
                        <asp:Label ID="lblBillSubTotal80" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right;">Discount(<asp:Label ID="lblDiscPercent80" runat="server"></asp:Label>%):</td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblDiscount80" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right;">Taxable Amount:</td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblTaxable80" runat="server"></asp:Label></td>
                </tr>
                <tr id="trVat80" runat="server">
                    <td style="text-align: right;">VAT(<asp:Label ID="lblVATPercent80" runat="server"></asp:Label>%):</td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblVAT80" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right;">Total:</td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblGrandTotal80" runat="server"></asp:Label></td>
                </tr>
                <tr id="trVATReturn80" runat="server" visible="false">
                    <td style="text-align: right;">VAT Return:</td>
                    <td style="text-align: right;">
                        <asp:Label ID="lblVATReturn80" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="text-align: right;" class="td-border-top td-border-bottom"><b>Net Amount:</b></td>
                    <td style="text-align: right;" class="td-border-top td-border-bottom"><b>
                        <asp:Label ID="lblGTotal80" runat="server"></asp:Label></b></td>
                </tr>
            </table>
            <div>---------------------------------------------</div>
            <div>
                In Words: Rs.
                <asp:Label ID="lblAmountInWord80" runat="server"></asp:Label>
            </div>
            <div>---------------------------------------------</div>
            <div style="font-size: 10px;">
                <asp:Label ID="lblRemarks80" runat="server" Text=""></asp:Label>
            </div>
            <div>
                * Goods once sold are not returnable
                <br />
                * E.& O.E.
            </div>
            <div>---------------------------------------------</div>
            <div style="font-size: 13px; margin-top: 4px;">
                Printed By:
                <asp:Label ID="lblPrintedBy80" runat="server"></asp:Label><br />
                Print Time:
                <asp:Label ID="lblTime80" runat="server"></asp:Label><br />
                User:
                <asp:Label ID="lblCreatedBy80" runat="server"></asp:Label>
            </div>
            <div style="text-align: center; margin-top: 4px;">THANK YOU...</div>
            <%-- <div style="font-weight: 700; font-size: 16px; margin-top: 4px;">
                Order No:
                 <asp:Label ID="lblOrderNo80" runat="server"></asp:Label>
            </div>--%>
        </div>
    </div>

    <%-- KOT PRINT FORMAT --%>
    <div id="printdetailTOK" runat="server" visible="false" style="margin-top: 1000px;">
        <div id="bill_format_tok" style="width: 80mm; font-family: 'Consolas'; font-size: 13px; font-weight: 100; padding: 5px; box-sizing: border-box;">
            <div style="text-align: center; margin-bottom: 4px;">
                <asp:Label ID="lblCompanyNameTOK" runat="server" Style="font-weight: bold; font-size: 20px;"></asp:Label><br />
                <asp:Label ID="lblCompanyAddressTOK" runat="server"></asp:Label><br />
                Contact No:
                <asp:Label ID="lblPhoneTOK" runat="server"></asp:Label><br />
                VAT No:
                <asp:Label ID="lblPanNoTOK" runat="server"></asp:Label>
                <div style="font-size: 22px; font-weight: 700; margin-top: 4px;">KOT</div>
                <div style="font-size: 22px; font-weight: 700; margin-top: 4px;">
                    Order NO: 
                      <asp:Label ID="lblOrderNoTOK" runat="server"></asp:Label>
                </div>



            </div>
            <%-- <div style="margin-bottom: 4px; text-align: center; font-size: 18px;">
                <asp:Label ID="lblNameTOK" runat="server"></asp:Label>
            </div>--%>
            <div style="margin-bottom: 4px;">
                Date:
                <asp:Label ID="lblBillDateTOK" runat="server"></asp:Label>
            </div>
            <div style="margin-bottom: 4px;">
                Bill No:
                <asp:Label ID="lblBillNoTOK" runat="server"></asp:Label>
            </div>
            <asp:GridView ID="gridTOK" runat="server" AutoGenerateColumns="False"
                Width="100%" ShowHeader="true" GridLines="None" CssClass="grid80mm">
                <Columns>
                    <asp:TemplateField HeaderText="SN">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="lblSNTOK" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PARTICULAR">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="lblProNameTOK" runat="server" Text='<%# Bind("PRODUCT_NAME") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="QTY">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:Label ID="lblQtyTOK" runat="server" Text='<%# Bind("QUANTITY") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div style="text-align: center; margin-top: 4px;">_________________________________________________________</div>
            <div style="font-size: 13px; margin-top: 4px;">
                Printed By:
                <asp:Label ID="lblPrintedByTOK" runat="server"></asp:Label><br />
                Print Time:
                <asp:Label ID="lblTimeTOK" runat="server"></asp:Label>
            </div>
        </div>

        <%-- Simplified TOK used only for auto-print right after Save --%>
        <div id="bill_format_tok_simple" style="width: 80mm; font-family: 'Consolas'; padding: 15px; box-sizing: border-box; text-align: center; border: 1px solid #000;">
            <div style="font-size: 23px; font-weight: 700; margin-bottom: 14px; text-align: center;">
                Order No:
        <asp:Label ID="lblOrderNoSimpleTOK" runat="server"></asp:Label>
            </div>
            <div style="font-size: 23px; font-weight: 700; margin-bottom: 14px; text-align: center;">
                Date:
        <asp:Label ID="lblDateSimpleTOK" runat="server"></asp:Label>
            </div>
            <div style="font-size: 23px; font-weight: 700; text-align: center;">
                Name:
        <asp:Label ID="lblNameSimpleTOK" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    <%-- PROFORMA INVOICE PRINT FORMAT --%>
    <div id="printdetailProforma" runat="server" visible="false" style="margin-top: 1000px;">
        <div id="print_div_proforma" style="width: 210mm; padding: 30px; box-sizing: border-box; margin: auto;">
            <div id="bill_format_proforma" style="margin-bottom: 50px; box-sizing: border-box; padding: 5px; font-family: Calibri;">
                <div style="text-align: center; font-size: 26px; font-weight: 600; margin-bottom: 12px;">Proforma Invoice</div>
                <table style="width: 100%; border-collapse: collapse; font-size: 12pt;">
                    <tr>
                        <td style="width: 55%; border: 1px solid #000; padding: 6px; vertical-align: top;">
                            <div style="font-weight: 700; font-size: 16px; text-transform: uppercase;">
                                <asp:Label ID="lblProCompanyName" runat="server"></asp:Label>
                            </div>
                            <div>
                                <asp:Label ID="lblProCompanyAddress" runat="server"></asp:Label>
                            </div>
                            <div>Company Registered No:
                                <asp:Label ID="lblProCompanyRegNo" runat="server"></asp:Label>
                            </div>
                            <div>PAN No:
                                <asp:Label ID="lblProCompanyPan" runat="server"></asp:Label>
                            </div>
                        </td>
                        <td style="width: 45%; border: 1px solid #000; border-left: none; padding: 6px; vertical-align: top;">
                            <table style="width: 100%; border-collapse: collapse;">
                                <tr>
                                    <td>Invoice No</td>
                                    <td>:
                                        <asp:Label ID="lblProInvoiceNo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Issue Date</td>
                                    <td>:
                                        <asp:Label ID="lblProIssueDate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Contract No</td>
                                    <td>:
                                        <asp:Label ID="lblProContractNo" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Contract Date</td>
                                    <td>:
                                        <asp:Label ID="lblProContractDate" runat="server"></asp:Label>
                                    </td>
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
                            <div>Payment Currency :
                                <asp:Label ID="lblProPaymentCurrency" runat="server"></asp:Label>
                            </div>
                            <div>Payment Mode :
                                <asp:Label ID="lblProPaymentMode" runat="server"></asp:Label>
                            </div>
                            <div>Shipment Type :
                                <asp:Label ID="lblProShipmentType" runat="server"></asp:Label>
                            </div>
                            <div>Shipment No :
                                <asp:Label ID="lblProShipmentNo" runat="server"></asp:Label>
                            </div>
                        </td>
                    </tr>
                </table>

                <asp:GridView ID="gridProforma" runat="server" AutoGenerateColumns="False" GridLines="Both"
                    Width="100%" CellPadding="6" Style="border-collapse: collapse; border: 1px solid #000; font-size: 11pt; margin-top: -1px;">
                    <HeaderStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" Font-Bold="true" />
                    <RowStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                    <Columns>
                        <asp:TemplateField HeaderText="S.N">
                            <ItemStyle HorizontalAlign="Left" Width="5%" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <ItemTemplate>
                                <asp:Label ID="lblProSN" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HS">
                            <ItemStyle HorizontalAlign="Left" Width="3%" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <ItemTemplate>
                                <asp:Label ID="lblProHS" runat="server" Text='<%# Bind("HS_CODE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Particulars">
                            <ItemStyle HorizontalAlign="Left" Width="45%" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <HeaderStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <ItemTemplate>
                                <asp:Label ID="lblProParticular" runat="server" Text='<%# Bind("PRODUCT_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty">
                            <ItemStyle HorizontalAlign="Right" Width="10%" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <HeaderStyle HorizontalAlign="Right" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <ItemTemplate>
                                <asp:Label ID="lblProQty" runat="server" Text='<%# Bind("QUANTITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rate">
                            <ItemStyle HorizontalAlign="Right" Width="10%" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <HeaderStyle HorizontalAlign="Right" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <ItemTemplate>
                                <asp:Label ID="lblProRate" runat="server" Text='<%# Bind("RATE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemStyle HorizontalAlign="Right" Width="20%" VerticalAlign="Top" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <HeaderStyle HorizontalAlign="Right" BorderStyle="Solid" BorderWidth="1px" BorderColor="#000000" />
                            <ItemTemplate>
                                <asp:Label ID="lblProAmount" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <table style="width: 100%; border-collapse: collapse; border: 1px solid #000; border-top: none; font-size: 11pt;">
                    <tr>
                        <td style="width: 80%; padding: 6px; text-align: left; font-weight: 600;">Sub Total</td>
                        <td style="width: 20%; padding: 6px; text-align: right; font-weight: 600;">
                            <asp:Label ID="lblProSubTotal" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div style="border: 1px solid #000; border-top: none; padding: 6px; font-size: 11pt;">
                    <b>Amount in words:</b>
                    <asp:Label ID="lblProAmountInWord" runat="server"></asp:Label>
                </div>

               <%-- <table style="width: 100%; font-size: 8pt; margin-top: 20px;">
                    <tr>
                        <td colspan="2">
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">............................</td>
                        <td style="text-align: right;">For:
                            <asp:Label ID="lblProForCompanyName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-style: italic" colspan="2">
                            Print Date Time:
                            <asp:Label ID="lblProTime" runat="server"></asp:Label>
                            &nbsp;Print By:
                            <asp:Label ID="lblProPrintedBy" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>--%>
            </div>
        </div>
    </div>

    <asp:Button ID="btnReset" runat="server" Text="" Style="display: none"
        OnClick="btnReset_Click" CausesValidation="false" />

    <div id="qrPopupOverlay" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.5); z-index: 9999;">
        <div style="background: #fff; width: 320px; margin: 80px auto; padding: 20px; border-radius: 6px; text-align: center;">
            <h4>Scan to Pay</h4>
            <div style="font-size: 18px; font-weight: bold; margin-bottom: 10px;">
                Rs.
                <asp:Label ID="lblQRAmount" runat="server" Text=""></asp:Label>
            </div>
            <asp:Image ID="imgQRPopup" runat="server" Style="max-width: 260px;" />
            <p style="font-weight: bold; margin-top: 10px;">
                <asp:Label ID="lblQRStatus" runat="server" Text=""></asp:Label>
            </p>
            <asp:Button ID="btnCloseQR" runat="server" Text="Close" CssClass="btn btn-secondary"
                OnClick="btnCloseQR_Click" CausesValidation="false" />
        </div>
        <div class="form-group" runat="server" visible="false">
            <label>Scan text (optional)</label>
            <asp:TextBox ID="txtScan" runat="server" CssClass="form-control" placeholder="SCAN TO PAY" />
        </div>
    </div>
      <script>
        window.POS_APP_ROOT = '<%= ResolveUrl("~/") %>';
    </script>
    <script src="<%= ResolveUrl("~/js/PosLocalClient.js") %>"></script>


    <script type="text/javascript">
        //var qrPollTimer = null;
        //var qrPollAttempts = 0;
        //var QR_MAX_POLL_ATTEMPTS = 75;

        function showQRPopup() {
            document.getElementById('qrPopupOverlay').style.display = 'block';
        }
        function hideQRPopup() {
            document.getElementById('qrPopupOverlay').style.display = 'none';
            stopQRPolling();
        }
        function setQRStatusText(text) {
            var el = document.getElementById('<%= lblQRStatus.ClientID %>');
        if (el) el.innerText = text;
    }
    //function startQRPolling() {
    //    stopQRPolling();
    //    qrPollAttempts = 0;
    //    qrPollTimer = setInterval(pollQROnce, 4000);
    //}
    //function stopQRPolling() {
    //    if (qrPollTimer) { clearInterval(qrPollTimer); qrPollTimer = null; }
    //}
    //function pollQROnce() {
    //    qrPollAttempts++;
    //    if (qrPollAttempts > QR_MAX_POLL_ATTEMPTS) {
    //        stopQRPolling();
    //        setQRStatusText('Payment timed out.');
    //        return;
    //    }
    //    PageMethods.CheckQRPaymentStatus(onQRStatusSuccess, onQRStatusError);
    //}
    //function onQRStatusSuccess(result) {
    //    switch (result.Status) {
    //        case 'PENDING':
    //            setQRStatusText('Waiting for payment... ' + (result.Message || ''));
    //            break;
    //        case 'SUCCESS':
    //            stopQRPolling();
    //            setQRStatusText('Payment Successful! ' + (result.Message || ''));
    //            setTimeout(hideQRPopup, 4000);
    //            break;
    //        case 'FAILED':
    //            stopQRPolling();
    //            setQRStatusText('Payment Failed. ' + (result.Message || ''));
    //            break;
    //        case 'NONE':
    //            stopQRPolling();
    //            break;
    //    }
    //}
    //function onQRStatusError(err) {
    //    setQRStatusText('Error checking status: ' + err.get_message());
    //}
    </script>
</asp:Content>
