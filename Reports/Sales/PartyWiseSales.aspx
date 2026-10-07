<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PartyWiseSales.aspx.cs"
    Inherits="Reports_Sales_PartyWiseSales" %>

<%@ Register Src="../../uc/CompanyName.ascx" TagName="CompanyName" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">

        function printPartOfPage() {
            var printContent = document.getElementById('divPrint');
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
        <div class="row" id="divGrid" runat="server">
            <div class="row" id="divBranch" runat="server">
                <div class="col-md-3">
                    Branch
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
            <div class="row">

                <div class="col-md-2">
                    Date From
               
                    <asp:TextBox ID="txtDateFrom" AutoComplete="off" runat="server" class="form-control  datepicker"
                        placeholder="dd/mm/yyyy"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    To
                
                    <asp:TextBox ID="txtDateTo" AutoComplete="off" runat="server" class="form-control  datepicker"
                        placeholder="dd/mm/yyyy"></asp:TextBox>
                </div>
                <div class="col-md-4">
                    Party Name
                <asp:DropDownList ID="ddlCustomerName" runat="server" class="form-control">
                </asp:DropDownList>

                </div>
                <div class="col-md-2 margin-top:auto;"><br />
                    <asp:Button ID="btnList" runat="server" Text="List" Width="100%" CssClass="btn btn-success" OnClick="btnList_Click" />
                </div>
                <div class="col-md-2 margin-top:auto;"><br />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" Width="100%" CssClass="btn btn-success" OnClick="btnPrint_Click" />
                </div>
            </div>
        </div>
        <div class="row" id="divPrint">
            <div class="col-md-12" style="margin-bottom:5em" id="divhide" runat="server" visible="false">
                <table style="width: 100%; text-align: center;">
                    <tr>
                        <td colspan="4">
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
                        <td colspan="4">
                            <asp:Label ID="lblHeading" runat="server" Text="Daily Sales" Font-Size="18px" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblDate" runat="server" Text="" Font-Size="14px" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grdReport" runat="server" Width="100%" CssClass="gridtable"
                                AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sno">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvNo" runat="server" Text='<%# Bind("INVOICE_NUMBER") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvDate" runat="server" Text='<%# Bind("INVOICE_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CUSTOMER_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub Total">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubTotal" runat="server" Text='<%# Bind("SUB_TOTAL") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiscount" runat="server" Text='<%# Bind("DISCOUNT_AMOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VAT">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVAT" runat="server" Text='<%# Bind("TAX_VAT_AMOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Grand Total">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrandTotal" runat="server" Text='<%# Bind("GRAND_TOTAL") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Round Off">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRoundOff" runat="server" Text='<%# Bind("ROUND_OFF") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# Bind("INVOICE_AMOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

