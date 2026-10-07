<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="opening_balance_sundry.aspx.cs" Inherits="Account_Utilities_opening_balance_sundry" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
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
        }
    </script>
    <div class="container">
        <div class="row" id="trBranch" runat="server">
            <div class="col-md-3">
                Branch<br />
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
                <asp:Label ID="lblBranchFY" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Fiscal Year:<br />
                <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control"></asp:DropDownList>

            </div>
            <div class="col-md-2">
                Sundry Type:<br />
                <asp:DropDownList ID="ddlSundryType" AutoPostBack="true" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlSundryType_SelectedIndexChanged">
                    <asp:ListItem Text="Select" />
                    <asp:ListItem Text="Debitors" Value="0103" />
                    <asp:ListItem Text="Creditors" Value="0403" />
                </asp:DropDownList>

            </div>
            <div class="col-md-2">
                General Ledger:<br />

                <asp:DropDownList ID="ddlGeneralLedgerCode" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
            <br />
            <div class="col-md-3" style="display: flex; align-items: center;">
                <asp:Button ID="btnView" CssClass="btn btn-primary" runat="server" Text="View" OnClick="btnView_Click" Style="margin-right: 10px;" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrint_Click" />
                <asp:ImageButton ID="btnExcel" runat="server" Width="50px" ImageUrl="~/images/icons/excel.png" Visible="false" OnClick="btnExcel_Click" />
            </div>


        </div>

        <br />
        <div class="row">
            <div id="divToPrint" runat="server" visible="false">
                <div id="print_div">
                    <table style="border: solid 1px;">
                        <tr>
                            <td>
                                <table style="width: 200mm; border: solid 1px;" border="0" class="gridtable">
                                    <tr>
                                        <td style="text-align: center;">
                                            <strong>
                                                <asp:Label ID="lblCompanyName" runat="server" Style="font-size: 20px"></asp:Label>
                                            </strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; height: 25px;">
                                            <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:Label ID="lblContact" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <b>
                                                <asp:Label ID="lblLedgerof" runat="server" Style="font-size: 20px"></asp:Label>
                                            </b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">Fiscal Year:<asp:Label ID="lblDate" runat="server"></asp:Label></td>

                                    </tr>

                                    <tr>
                                        <td>
                                            <div runat="server" id="divAccountLedger" visible="true">
                                                <asp:GridView ID="gridOpeningbalance" runat="server" AutoGenerateColumns="False" CssClass="normalTable"
                                                    Width="100%" ShowFooter="true" OnRowDataBound="gridOpeningbalance_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SN.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSubGl" runat="server" Text='<%# Bind("SGL_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Particulars">
                                                            <FooterTemplate>
                                                                <strong>Total</strong>
                                                            </FooterTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%# Bind("PARTICULARS") %>' ID="lblParticulars" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Opening Balance">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOpneningBalance" Text='<%# Bind("OPENING_BALANCES") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dr Transaction">
                                                            <FooterTemplate>
                                                                <asp:Label Font-Bold="true" ID="lblDrTotal" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDrAmount" Text='<%# Bind("DR_AMOUNT") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cr Transaction">
                                                            <FooterTemplate>
                                                                <asp:Label Font-Bold="true" ID="lblCrTotal" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCrAmount" Text='<%# Bind("CR_AMOUNT") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Closing Balance">
                                                            <FooterTemplate>
                                                                <asp:Label Font-Bold="true" ID="lblTotalBalance" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClosingBalanceAmount" Text='<%# Bind("CLOSING_BALANCE") %>' runat="server" />
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
        </div>
    </div>
</asp:Content>

