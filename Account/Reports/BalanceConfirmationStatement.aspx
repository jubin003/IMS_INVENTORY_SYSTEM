<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BalanceConfirmationStatement.aspx.cs" Inherits="Account_Reports_BalanceConfirmationStatement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">

        function printPartOfPage() {
            var printContent = document.getElementById('printDiv');
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
    <div class="container">
         <div class="row" id="trBranch" runat="server">
            <div class="col-md-3">
                Branch<br />
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Fiscal Year
                <asp:DropDownList CssClass="form-control" ID="ddlFiscalYear" runat="server">
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                General Ledger
                               <asp:DropDownList ID="ddlGLName" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlGLName_SelectedIndexChanged"></asp:DropDownList>

            </div>
            <div class="col-md-4">
                <asp:Label ID="lblSubLedger" runat="server" Text="Sub Ledger"></asp:Label>
                <br />
                <asp:DropDownList ID="ddlsubLedger" Width="100%" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <br />
                <asp:Button ID="btn_view" CssClass="btn btn-primary" runat="server" Text="View" OnClick="btn_view_Click" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="false" OnClick="btnPrint_Click" CssClass="btn btn-primary" />
            </div>

            <div class="col-md-2">
                <br />



            </div>
        </div>
        <br />
        <div id="divReport" runat="server" class="col-lg-12" visible="false">
            <div id="printDiv" class="row">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblCompanyName" runat="server" Text="" Style="font-weight: bold; font-size: 32px;"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblCompanyAddress" runat="server" Text="" Style="font-size: 12pt;"></asp:Label></td>
                                </tr>

                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblPhone1" runat="server" Text="" Style="font-size: 12pt;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center; height: 22px">
                                        <asp:Label ID="Label1" runat="server" Style="font-size: 12pt; font-weight: normal; font-family: Verdana;" Text="VAT No."></asp:Label>
                                        <asp:Label ID="lblVatNo" runat="server" Style="font-size: 12pt; font-weight: normal; font-family: Verdana;"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right"><b style="font-size: 20px">Date:<asp:Label ID="lblDate" Font-Bold="true" runat="server"></asp:Label></b>
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 70%"><b style="font-size: 20px">M/S:</b>
                            <asp:Label ID="lblCusName" Font-Size="20px" Font-Bold="true" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><b style="font-size: 20px">ADDRESS:</b>
                            <asp:Label ID="lblCusAddress" Font-Size="20px" Font-Bold="true" runat="server"></asp:Label></td>

                    </tr>
                    <tr>
                        <td><b style="font-size: 20px">PAN NO:</b>
                            <asp:Label Font-Size="20px" ID="lblCusPan" Font-Bold="true" runat="server"></asp:Label><br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%; height: 500px; vertical-align: top">
                                <tr>
                                    <td style="width: 100%; vertical-align: top">
                                        <br />
                                        <p style="align-content: center; font-size: 20px;text-align:center">
                                            Subject:<b> Confirmation of Accounts Transaction and Closing Balance<br /> for the fiscal year
                                                <asp:Label ID="lblFy" runat="server" Font-Size="20px"></asp:Label></b><br />
                                            <br />
                                        </p>
                                        <p>
                                            Dear Sir,<br />
                                            <br />

                                            As a part of Finalization of our audit for the fiscal year
                                        <asp:Label ID="lblFyYear" runat="server" ></asp:Label>, we would like to confirm that the transactions incurred and
                                         closing balance outstanding as on Ashadh end
                                            <asp:Label ID="lblFYear" runat="server" ></asp:Label>
                                            as per our books stands as under:
                                        </p>
                                        <br />
                                        <asp:GridView ID="gridBCS" runat="server" Visible="true" BorderWidth="2" AutoGenerateColumns="False" Width="100%">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Font-Bold="true" HeaderStyle-BorderWidth="2" HeaderText="Opening Balance">
                                                    <HeaderStyle Font-Bold="true" />
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Bind("OPENING_BALANCE") %>' ID="lblOpeningBalance" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Font-Bold="true" HeaderStyle-BorderWidth="2" HeaderText="Sales Transaction">
                                                    <HeaderStyle Font-Bold="true" />
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Bind("SALES_TRANSACTION") %>' ID="lblSalesTrans" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Font-Bold="true" HeaderStyle-BorderWidth="2" HeaderText="Net Sales">
                                                    <HeaderStyle Font-Bold="true" />
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Bind("NET_SALES") %>' ID="lblNetSales" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Font-Bold="true" HeaderStyle-BorderWidth="2" HeaderText="VAT">
                                                    <HeaderStyle Font-Bold="true" />
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Bind("VAT") %>' ID="lblVat" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Font-Bold="true" HeaderStyle-BorderWidth="2" HeaderText="Payment">
                                                    <HeaderStyle Font-Bold="true" />
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Bind("PAYMENT") %>' ID="lblPayment" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />

                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Font-Bold="true" HeaderStyle-BorderWidth="2" HeaderText="Closing Balance">
                                                    <HeaderStyle Font-Bold="true" />
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Bind("CLOSING_BALANCE") %>' ID="lblClosingBalance" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                        <p style="align-content: flex-start;">
                                            We kindly request you to confirm the above-mention details and send us the signed and stamped copy of the letter within 7 days of its receipt.
                                         If no information is received within the period, the aforementioned details shall be assumed to be final.
                                        <br />
                                            <br />
                                            Your cooperation in this matter is highly appreciated.
                                        <br />
                                            <br />
                                            <br />
                                            <br />


                                        </p>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <p >Your sincerly,</p>
                                                </td>
                                                <td style="width: 70%"></td>
                                                <td style="width: 20%">
                                                    <p >
                                                        Received and Confirmed By
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td style="width: 90%"></td>
                                                <td>
                                                    <p>
                                                        Seal & Sign
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>

                                                <td>
                                                    <p>
                                                        .......................................
                                                    
                                                    </p>
                                                </td>
                                                <td style="width: 75%"></td>
                                                <td>
                                                    <p>
                                                        .......................................
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <p>
                                                        <asp:Label ID="lblCompanyN" runat="server"></asp:Label>
                                                    </p>
                                                </td>
                                                <td style="width: 80%"></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
</asp:Content>

