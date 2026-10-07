<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="IRD_Report.aspx.cs"
    Inherits="Reports_IRD_IRD_Report" %>

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
            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
        </div>
    </div>

    <table style="width: 1200px">
        <tr>
            <td>From Date:</td>
            <td style="width: 150px">
                <asp:TextBox ID="txtFromDate" AutoComplete="off" runat="server" CssClass="form-control datepicker"></asp:TextBox>
            </td>
            <td>&nbsp;</td>
            <td>To Date:</td>
            <td style="width: 150px">
                <asp:TextBox ID="txtToDate" AutoComplete="off" runat="server" CssClass="form-control datepicker"></asp:TextBox>
            </td>
            <td>Invoice Type</td>
            <td>
                <asp:DropDownList ID="ddlExempted" runat="server" CssClass="form-control">
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    <asp:ListItem Text="Tax Invoice" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Exempted Invoice" Value="1"></asp:ListItem>
                </asp:DropDownList></td>
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
                        <span style="font-size: 16pt;">Materialized View
                                <asp:Label ID="lblRegNo" runat="server" Text="" Visible="false"></asp:Label></span>
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
                    <td style="text-align: center; font-family: Verdana; font-size: 14px">&nbsp;</td>
                    <td style="text-align: right; font-size: 10pt">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gridIRDReport" runat="server" CssClass="gridtable-sm" ShowFooter="true"
                            AutoGenerateColumns="False" EnableModelValidation="True" Width="100%" OnRowDataBound="gridIRDReport_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Fiscal_Year">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFiscalYear" runat="server" Text='<%# Bind("FISCAL_YEAR") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bill_No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillNo" runat="server" Text='<%# Bind("BILL_NO") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer_Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CUSTOMER_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        Total
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer_PAN">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerPan" runat="server" Text='<%# Bind("CUSTOMER_PAN_VAT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bill_Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBillDate" runat="server" Text='<%# Bind("BILL_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterTemplate>
                                        <asp:Label ID="lblFTotalAmount" runat="server" Text=''></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiscount" runat="server" Text='<%# Bind("DISCOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalDiscount" runat="server" Text=''></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Taxable_Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxableAmount" runat="server" Text='<%# Bind("TAXABLE_AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalTaxableAmount" runat="server" Text=''></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax_Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%# Bind("TAX_AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalTaxAmount" runat="server" Text=''></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total_Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Bind("TOTAL_AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalTotalAmount" runat="server" Text=''></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sync_with_IRD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSyncWithIRD" runat="server" Text='<%# Bind("SYNC_WITH_IRD") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is_Printed">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsPrinted" runat="server" Text='<%# Bind("IS_PRINTED") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is_Bill_Active">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsActive" runat="server" Text='<%# Bind("IS_ACTIVE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Printed_Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrintedTime" runat="server" Text='<%# Bind("PRINTED_TIME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Entered_By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEnteredBy" runat="server" Text='<%# Bind("ENTERED_BY") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Printed_By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrintedBy" runat="server" Text='<%# Bind("PRINT_BY") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is_Realtime">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsRealtime" runat="server" Text='<%# Bind("IS_REALTIME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payment_Method">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPayment_Method" runat="server" Text='<%# Bind("Payment_Method") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>

        </div>
    </div>
</asp:Content>

