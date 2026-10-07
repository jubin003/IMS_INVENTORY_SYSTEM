<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Item_Register.aspx.cs" Inherits="Account_Reports_Item_Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- DataTables CSS -->


    <div class="container-fluid">
        <asp:Button ID="btn_back" CssClass="btn btn-primary" runat="server" Text="Back"  OnClick="btn_back_Click" />
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrint_Click" />

    </div>

    <script>
        $(document).ready(function () {
            $("#<%= gridItemReg.ClientID %>").addClass("display"); // Add DataTable styling
            $("#<%= gridItemReg.ClientID %>").DataTable({
                "paging": false,  // Disable pagination
                "ordering": true, // Enable sorting
                "info": false,    // Hide table info
                "searching": false // Disable search box
            });
        });

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

    <style>
        table.dataTable thead th {
            cursor: pointer;
            background-color: #f2f2f2;
        }
    </style>

    <div id="divToPrint" runat="server" visible="false">
        <div id="print_div">
            <table style="width: 100%" border="0" class="gridtable">
                <tr>
                    <td colspan="3" style="text-align: center;">
                        <strong>
                            <asp:Label ID="lblCompanyName" runat="server" Style="font-size: 20px"></asp:Label>
                            -<asp:Label ID="lblFyDate" runat="server" Style="font-size: 20px"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: center;">
                        <asp:Label ID="lblAddress" runat="server" Style="text-decoration: underline; font-size: 20px"></asp:Label>
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
                        <strong>
                            <asp:Label Text="Item Register" runat="server" Style="font-size: 15px"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: center;">
                        <strong>
                            <asp:Label ID="lblMonth" runat="server" Style="font-size: 25px"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: center;">
                        <asp:Label ID="lblFisDate" runat="server" Style="font-size: 15px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:GridView ID="gridItemReg" CssClass="display" AutoGenerateColumns="false" OnRowDataBound="gridItemReg_RowDataBound" ShowFooter="true" AllowSorting="True" Width="100%" runat="server" OnRowCreated="gridItemReg_RowCreated">
                            <Columns>
                                <asp:TemplateField HeaderText="SN.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("DATES") %>' ID="lblDate" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Miti" SortExpression="">

                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("MITI") %>' ID="lblMiti" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice Number">
                                    <ItemTemplate>
                                        <asp:LinkButton Text='<%# Bind("INVOICE_NUMBER") %>' Font-Bold="true" ID="lblInvNum" runat="server" OnClick="lblInvNum_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplers/Customers">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("PARTICULARS") %>' Font-Bold="true" ID="lblParticulars" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate><strong>Total</strong></FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("VOUCHER_TYPE") %>' ID="lblVcType" Font-Bold="true" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inward Quantity">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("TOTAL_INWARDS_QTY") %>' ID="lblInQty" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Font-Bold="true" ID="lblInQtyF" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inward Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInRate" runat="server" Text="" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblInRateF" runat="server" Text="0.00" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inward Value">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("TOTAL_INWARDS_VALUE") %>' ID="lblInVal" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Font-Bold="true" ID="lblInValF" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Outward Quantity">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("TOTAL_OUTWARD_QTY") %>' ID="lblOutQty" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Font-Bold="true" ID="lblOutQtyF" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Outward Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutRate" runat="server" Text="" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblOutRateF" runat="server" Text="0.00" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Outward Value">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("TOTAL_OUTWARD_VALUE") %>' ID="lblOutVal" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label Font-Bold="true" ID="lblOutValF" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Closing Quantity">
                                    <FooterTemplate>
                                        <asp:Label Font-Bold="true" ID="lblCloQtyF" runat="server"></asp:Label>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("TOTAL_CLOSING_QTY") %>' ID="lblCloQty" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Closing Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCloRate" runat="server" Text="" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblCloRateF" runat="server" Text="0.00" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Closing Value">
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
        </div>
    </div>
</asp:Content>
