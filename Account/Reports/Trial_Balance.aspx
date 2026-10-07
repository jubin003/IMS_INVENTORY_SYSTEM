<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Trial_Balance.aspx.cs" Inherits="Account_Reports_Trial_Balance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        window.onload = function () {
            // Initialize Nepali Date Picker on multiple elements
            var elements = ["<%= txtFromDate.ClientID %>", "<%= txtToDate.ClientID %>"];
            elements.forEach(function (id) {
                var element = document.getElementById(id);
                if (element) {
                    element.nepaliDatePicker();
                }
            });
        };
    </script>
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
        <table class="gridtable">
            <tr id="trBranch" runat="server">
                <td colspan="3">Branch<br />
                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Fiscal Year
               <br />
                    <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control"></asp:DropDownList>
                </td>
                <td>From Date
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                </td>
                <td>To Date
                <asp:TextBox ID="txtToDate" AutoPostBack="true" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                </td>
                <td>Trial Balance Type
                    <asp:DropDownList ID="ddlTrialbalanceType" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                        <asp:ListItem Text="Ledger Trial Balance" Value="Ledger Trial Balance"></asp:ListItem>
                        <asp:ListItem Text="Group Trial Balance" Value="Group Trial Balance"></asp:ListItem>
                        <asp:ListItem Text="Account Trial Balance" Value="Account Trial Balance"></asp:ListItem>
                    </asp:DropDownList>
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
        <div id="divTrialBalance" runat="server" visible="false">
            <div id="print_div">
                <table style="border: solid 1px;" class="gridtable">
                    <tr>
                        <td>
                            <table style="width: 200mm;" border="0">
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
                                    <td colspan="3" style="text-align: center;">
                                        <b>
                                            <asp:Label ID="lblTrialBalanceType" runat="server" Style="font-size: 20px"></asp:Label></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center;">Date:
                                        <asp:Label ID="lblDate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div runat="server" id="divLedgerTrialBalance" visible="false">
                                            <asp:GridView ID="gridTrialBalace" runat="server" ShowFooter="True" CssClass="normalTable"
                                                AutoGenerateColumns="False" OnRowDataBound="gridTrialBalace_RowDataBound" Width="1000px">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SN.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Particulars">
                                                        <FooterTemplate>
                                                            <strong>Total</strong>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("GL_NAME") %>' ID="lblGlName" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Opening Debit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblOpeningDrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("OPENING_DR_AMOUNT") %>' ID="lblOpeningDrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Opening Credit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblOpeningCrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("OPENING_CR_AMOUNT") %>' ID="lblOpeningCrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Debit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblDrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("DR_AMOUNT") %>' ID="lblDrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Credit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblCrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("CR_AMOUNT") %>' ID="lblCrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Debit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblTotalDrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("TOTAL_DR_AMOUNT") %>' ID="lblTotalDrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Credit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblTotalCrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("TOTAL_CR_AMOUNT") %>' ID="lblTotalCrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div runat="server" id="divGroupTrialBalance" visible="false">
                                            <asp:GridView ID="gridGroupTrialBalance" OnRowDataBound="gridGroupTrialBalance_RowDataBound" runat="server" AutoGenerateColumns="False"
                                                CssClass="normalTable" ShowFooter="true" Width="1000px">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SN.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Particulars">
                                                        <FooterTemplate>
                                                            <strong>Total</strong>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("GL_MASTER_NAME") %>' ID="lblGlMasterName" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Opening Debit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblOpeningDrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("OPENING_DR_AMOUNT") %>' ID="lblOpeningDrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Opening Credit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblOpeningCrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("OPENING_CR_AMOUNT") %>' ID="lblOpeningCrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Debit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblDrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("DR_AMOUNT") %>' ID="lblDrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Credit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblCrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("CR_AMOUNT") %>' ID="lblCrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Debit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblTotalDrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("TOTAL_DR_AMOUNT") %>' ID="lblTotalDrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Credit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblTotalCrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("TOTAL_CR_AMOUNT") %>' ID="lblTotalCrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <div runat="server" id="divAccountTrialBalance" visible="false">
                                            <asp:GridView ID="gridAccountTrialBalance" OnRowDataBound="gridAccountTrialBalance_RowDataBound" runat="server" ShowFooter="True"
                                                CssClass="normalTable" AutoGenerateColumns="False" Width="1000px">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SN.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Particulars">
                                                        <FooterTemplate>
                                                            <strong>Total</strong>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("ACCOUNTS_HEAD") %>' ID="lblAccountHead" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Opening Debit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblOpeningDrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("OPENING_DR_AMOUNT") %>' ID="lblOpeningDrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Opening Credit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblOpeningCrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("OPENING_CR_AMOUNT") %>' ID="lblOpeningCrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Debit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblDrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("DR_AMOUNT") %>' ID="lblDrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Credit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblCrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("CR_AMOUNT") %>' ID="lblCrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Debit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblTotalDrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("TOTAL_DR_AMOUNT") %>' ID="lblTotalDrAmount" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Credit Amount">
                                                        <FooterTemplate>
                                                            <asp:Label Font-Bold="true" ID="lblTotalCrTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Bind("TOTAL_CR_AMOUNT") %>' ID="lblTotalCrAmount" runat="server" />
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
            <br />
        </div>
    </div>
</asp:Content>

