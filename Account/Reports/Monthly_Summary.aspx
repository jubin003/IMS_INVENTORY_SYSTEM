<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Monthly_Summary.aspx.cs" Inherits="Account_Reports_Monthly_Summary" %>

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
            //  printWindow.close();
        }
    </script>
    <div class="container-fluid">
        <asp:Button ID="btn_back" CssClass="btn btn-primary" runat="server" Text="Back" OnClick="btn_back_Click" />
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrint_Click" />
        <div id="divToPrint" runat="server" visible="false">
            <div id="print_div">
                <table style="width: 100%;" border="0" class="gridtable">
                    <tr>
                        <td colspan="3" style="text-align: center;">
                            <strong>
                                <asp:Label ID="lblCompanyName" runat="server" Style="font-size: 20px"></asp:Label>
                                -<asp:Label ID="lblFyDate" runat="server" Style="font-size: 20px"></asp:Label></strong></td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: center;">
                            <asp:Label ID="lblAddress" runat="server" Style=" font-size: 20px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: center;">
                            <asp:Label ID="lblContact" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: center;">
                            <b>
                                <asp:Label ID="lblProduct" Style="font-size: 17px" runat="server"></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: center;">

                            <asp:Label Text="Monthly Summary" runat="server" Style="font-size: 15px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: center;">
                            <asp:Label ID="lblFisDate" runat="server" Style="font-size: 15px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gridMonthlyProductSummary" AutoGenerateColumns="false" ShowFooter="true" Width="100%" runat="server" OnRowDataBound="gridMonthlyProductSummary_RowDataBound" OnRowCreated="gridMonthlyProductSummary_RowCreated">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Months">
                                                    <FooterTemplate>
                                                        <strong>Total</strong>
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton Text='<%# Bind("PARTICULARS") %>' ID="lblParticulars" runat="server" OnClick="lblParticulars_Click" />
                                                        <asp:Label ID="lblMnth" runat="server" Text='<%# Bind("INV_MTH") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Quantity">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Bind("TOTAL_INWARDS_QTY") %>' ID="lblInQty" runat="server" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label Font-Bold="true" ID="lblInQtyF" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText=" Value">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Bind("TOTAL_INWARDS_VALUE") %>' ID="lblInVal" runat="server" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label Font-Bold="true" ID="lblInValF" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Quantity">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Bind("TOTAL_OUTWARD_QTY") %>' ID="lblOutQty" runat="server" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label Font-Bold="true" ID="lblOutQtyF" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Value">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Bind("TOTAL_OUTWARD_VALUE") %>' ID="lblOutVal" runat="server" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label Font-Bold="true" ID="lblOutValF" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText=" Quantity">
                                                    <FooterTemplate>
                                                        <asp:Label Font-Bold="true" ID="lblCloQtyF" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Bind("TOTAL_CLOSING_QTY") %>' ID="lblCloQty" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Value">
                                                    <FooterTemplate>
                                                        <asp:Label Font-Bold="true" ID="lblCloValF" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCloVal" Text='<%# Bind("TOTAL_CLOSING_VALUE") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

