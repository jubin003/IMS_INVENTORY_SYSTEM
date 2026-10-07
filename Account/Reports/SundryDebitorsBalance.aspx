<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SundryDebitorsBalance.aspx.cs" Inherits="Account_Reports_SundryDebitorsBalance" %>

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
    <div class="Container">
        <table style="width: 600px" class="gridtable">
            <tr id="trBranch" runat="server">
                <td colspan="3">Branch<br />
                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">Fiscal Year<br />
                    <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList></td>
                <td style="width: 200px">General Ledger
                    <br />
                    <asp:DropDownList ID="ddlGLName" runat="server" CssClass="form-control"></asp:DropDownList>
                </td>
                <td>
                    <br />
                    <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" CssClass="btn btn-primary" />
                </td>
                <td>
                    <br />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" CssClass="btn btn-primary" />
                </td>

            </tr>
        </table>
        <div id="divToPrint" runat="server" visible="false">
            <div id="print_div">
                <table style="border: solid 1px;">
                    <tr>
                        <td>
                            <table style="width: 200mm;" border="0" class="gridtable">
                                <tr>
                                    <td colspan="3" style="text-align: center;">
                                        <strong>
                                            <asp:Label ID="lblCompanyName" runat="server" Style="font-size: 20px"></asp:Label>
                                        </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center;">
                                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center;">
                                        <asp:Label ID="lblContact" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">Balance Print Date:<asp:Label ID="lblDate" runat="server"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div runat="server" id="divAccountLedger" visible="false">
                                            <asp:GridView ID="gridAccountLedger" runat="server" AutoGenerateColumns="False" CssClass="normalTable"
                                                Width="100%" ShowFooter="true" OnRowDataBound="gridAccountLedger_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SN.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Code">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("SUB_GL_CODE") %>' ID="lblCode" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name">
                                                        <ItemTemplate>
                                                            <asp:LinkButton OnClick="lblName_Click" Text='<%# Bind("SUB_GL_NAME") %>' ID="lblName" runat="server" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            Total
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("BALANCE") %>' ID="lblBalance" runat="server" Visible="false" />
                                                            <asp:Label ID="lblPartyBalance" runat="server" Text=""></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalBalance" runat="server" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>

                                </tr>

                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divLedger" visible="false" runat="server">
            <div id="ledger_div">
                <table style="border: solid 1px;">
                    <tr>
                        <td>
                            <table style="width: 200mm;" border="0" class="gridtable">
                                <tr>
                                    <td colspan="3" style="text-align: center;">
                                        <strong>
                                            <asp:Label ID="lblCompanyNameledger" runat="server" Style="font-size: 20px"></asp:Label>
                                        </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center;">
                                        <asp:Label ID="lblAddressledger" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center;">
                                        <asp:Label ID="lblContactledger" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center;">
                                        <b>
                                            <asp:Label ID="lblLedgerof" runat="server" Style="font-size: 20px"></asp:Label></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">Ledger Print Date:<asp:Label ID="Label1" runat="server"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div runat="server" id="divSubAccountLedgers">
                                            <asp:GridView ID="gridSubAccountLedgers" runat="server" AutoGenerateColumns="False" CssClass="normalTable"
                                                Width="100%" ShowFooter="true" OnRowDataBound="gridSubAccountLedgers_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SN.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("DATE") %>' ID="lblDate" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="V. Type">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("VOUCHER_TYPE") %>' ID="lblVoucherType" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="V. No.">
                                                        <ItemTemplate>
                                                            <asp:LinkButton OnClick="lblVoucherNum_Click" Text='<%# Bind("VOUCHER_NUMBER") %>' ID="lblVoucherNum" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Branch" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%--<%# Bind("VOUCHER_NUMBER") %>--%>' ID="lblBranch" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Particulars">
                                                        <FooterTemplate>
                                                            <strong>Total</strong>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton OnClick="lblParticulars_Click" Style="color: black" Text='<%# Bind("PARTICULARS") %>' ID="lblParticulars" runat="server" />
                                                            <asp:Label Visible="false" ID="lblBill" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dr Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblDrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("DR_AMOUNT") %>' ID="lblDrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cr Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblCrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("CR_AMOUNT") %>' ID="lblCrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblTotalBalance" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text="" ID="lblBalanceAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>

                                </tr>

                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divVoucher" runat="server" visible="false">
            <div class=" row">
                <div class="col-md-2">
                    <table>
                        <tr>
                            <td>
                                <br />
                                <asp:Button ID="btn_back" CssClass="btn btn-primary" Width="80px" OnClick="btn_back_Click" runat="server" Text="Back" /></td>
                        </tr>
                    </table>
                </div>

            </div>

            <table style="border: solid 1px; width: 200mm;">
                <tr>
                    <td>
                        <table style="width: 200mm;" border="0">
                            <tr>
                                <td colspan="3" style="text-align: center; height: 27px;">

                                    <strong>
                                        <asp:Label ID="lblCompanyNameS" runat="server" Style="font-size: 20px"></asp:Label>
                                    </strong>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center;">
                                    <asp:Label ID="lblAddressS" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center;">
                                    <asp:Label ID="lblContactS" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align: center;">
                                    <b>
                                        <asp:Label ID="lblVoucherType" runat="server" Style="font-size: 20px"></asp:Label></b>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;"><strong>Voucher No:</strong>
                                    <asp:Label ID="lblVoucherNo" runat="server"></asp:Label>
                                </td>
                                <td></td>
                                <td style="text-align: right;"><strong>Date:               
                                </strong>
                                    <asp:Label ID="lblDates" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:GridView ID="grdVoucherChild" runat="server" AutoGenerateColumns="False" CssClass="normalTable"
                                        ShowFooter="True" OnRowDataBound="grdVoucherChild_RowDataBound" Width="100%">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Particulars">
                                                <FooterTemplate>
                                                    <strong>Total</strong>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Bind("PARTICULARS") %>' ID="lblParticulars" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dr. Amount">
                                                <FooterTemplate>
                                                    <asp:Label Font-Bold="true" ID="lblDrTotal" runat="server"></asp:Label>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Bind("DR_AMOUNT") %>' ID="lblDrAmount" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="30mm" HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cr. Amount">
                                                <FooterTemplate>
                                                    <asp:Label Font-Bold="true" ID="lblCrTotal" runat="server"></asp:Label>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Bind("CR_AMOUNT") %>' ID="lblCrAmount" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="30mm" HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <strong>Amount in Words: </strong>
                                    <asp:Label ID="lblAmountsInword" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr />
                                    <strong>Narration: </strong>
                                    <asp:Label ID="lblNarration" runat="server" Text=""></asp:Label>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; width: 650mm;">Prepared By
                                        <br />
                                    <asp:Label ID="lblPreparedBy" runat="server" Text=""></asp:Label>
                                </td>
                                <td style="text-align: center; width: 650mm;">
                                    <div id="divCheckedBy" runat="server">
                                        Checked By
                                         <br />
                                        <asp:Label ID="lblCheckedBy" runat="server" Text=""></asp:Label>
                                    </div>
                                </td>
                                <td style="text-align: center; width: 650mm;">Approved By
                                         <br />
                                    <asp:Label ID="lblApprovedBy" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; width: 650mm;">&nbsp;</td>
                                <td style="text-align: center; width: 650mm;">&nbsp;</td>
                                <td style="text-align: center; width: 650mm;">&nbsp;</td>
                            </tr>
                            <tr runat="server" visible="false" id="divAlternationNote">
                                <td colspan="3">
                                    <strong>
                                        <asp:Label ID="LabelAlterationNote" runat="server" Text=""></asp:Label></strong>
                                    <asp:Label ID="lblAlterationNote" runat="server" Text=""></asp:Label><br />
                                    <strong>
                                        <asp:Label ID="LabelAlterationNoteRequest" runat="server" Text=""></asp:Label></strong>
                                    <asp:Label ID="lblAlterationNoteRequest" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>

                </tr>
            </table>
        </div>
    </div>
</asp:Content>

