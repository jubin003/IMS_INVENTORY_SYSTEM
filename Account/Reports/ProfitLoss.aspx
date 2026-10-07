<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProfitLoss.aspx.cs" Inherits="Account_Reports_ProfitLoss" %>

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
        <div class="row">
            <div class="col-md-3" id="divBranch" runat="server">Branch
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-2">Fiscal Year
                <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-1"><br />
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" CssClass="btn btn-primary" />
            </div>
            <div class="col-md-1"><br />
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" CssClass="btn btn-primary" />
            </div>
        </div>

        <div id="divPrint" runat="server" visible="false">
            <div id="print_div">
                <table style="border: solid 1px; width: 210mm; border-collapse: collapse;" class="normalTable">
                    <tr>
                        <td style="text-align: center;">
                            <strong>
                                <asp:Label ID="lblCompanyName" runat="server" Style="font-size: 20px"></asp:Label>
                            </strong>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Label ID="lblAddress" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Label ID="lblContact" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; font-size: 16px;">
                            <b>Profit and Loss Account As on                                        
                                <asp:Label ID="lblDate" runat="server"></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%;" class="gridtable">
                                <tr>
                                    <td><span style="text-decoration: underline"><strong>Particulars</strong></span>
                                    </td>
                                    <td style="width: 200px; text-align: right"><b><span style="text-decoration: underline">As on </span>
                                        <asp:Label ID="lblThisYearDate" runat="server" Style="text-decoration: underline"></asp:Label></b>
                                    </td>
                                    <td style="width: 200px; text-align: right"><b><span style="text-decoration: underline">As on </span>
                                        <asp:Label ID="lblLastYearDate" runat="server" Style="text-decoration: underline"></asp:Label></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td><u><strong>Income</strong></u>
                                    </td>
                                    <td style="width: 200px;"></td>
                                    <td style="width: 200px;"></td>
                                </tr>
                            </table>
                            <asp:GridView ID="grdGL_MASTER_Income" runat="server" CssClass="gridnoboder" Width="100%" ShowHeader="false"
                                AutoGenerateColumns="False" OnRowDataBound="grdGL_MASTER_Income_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Income">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGLMaster_code" runat="server" Text='<%# Bind("GL_MASTER_CODE") %>' Visible="false"></asp:Label>
                                            <b>
                                                <asp:Label ID="lblGLMaster_Name" runat="server" Text='<%# Bind("GL_MASTER_NAME") %>'></asp:Label></b>
                                            <asp:GridView ID="grdGLAccount_Income" runat="server" Width="100%" CssClass="gridnoboder" AutoGenerateColumns="False"
                                                ShowHeader="false" OnRowDataBound="grdGLAccount_Income_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Particulars">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGL_Master_Code" runat="server" Text='<%# Bind("GL_MASTER_CODE") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblGL_Code" runat="server" Text='<%# Bind("GL_CODE") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblParticulars" runat="server" Text='<%# Bind("GL_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance This Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblThisYearBalance" runat="server" Text='<%# Bind("BALANCE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance Last Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastYearBalance" runat="server" Text='<%# Bind("BALANCE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <table style="width: 100%;" class="gridnoboder">
                                <tr>
                                    <td><strong>Total Income
                                    </strong>
                                    </td>
                                    <td style="width: 200px; text-align: right; border-top-style: double"><strong>
                                        <asp:Label ID="lblThisYearAssetTotal" runat="server" Text=""></asp:Label>
                                    </strong>
                                    </td>
                                    <td style="width: 200px; text-align: right; border-top-style: double;"><strong>
                                        <asp:Label ID="lblLastYearAssetTotal" runat="server" Text=""></asp:Label>
                                    </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3"></td>
                                </tr>
                            </table>
                            <br />
                            <table style="width: 100%;" class="gridtable">
                                <tr>
                                    <td><u><strong>Expenses</strong></u>
                                    </td>
                                    <td style="width: 200px;"></td>
                                    <td style="width: 200px;"></td>
                                </tr>
                            </table>
                            <asp:GridView ID="grdGL_MASTER_Expenses" runat="server" CssClass="gridnoboder" Width="100%" ShowHeader="false"
                                AutoGenerateColumns="False" OnRowDataBound="grdGL_MASTER_Expenses_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Expenses">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGLMaster_code" runat="server" Text='<%# Bind("GL_MASTER_CODE") %>' Visible="false"></asp:Label>
                                            <b>
                                                <asp:Label ID="lblGLMaster_Name" runat="server" Text='<%# Bind("GL_MASTER_NAME") %>'></asp:Label></b>
                                            <asp:GridView ID="grdGLAccount_Expenses" runat="server" Width="100%" CssClass="gridnoboder" AutoGenerateColumns="False"
                                                ShowHeader="false" OnRowDataBound="grdGLAccount_Expenses_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Particulars">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGL_Master_Code" runat="server" Text='<%# Bind("GL_MASTER_CODE") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblGL_Code" runat="server" Text='<%# Bind("GL_CODE") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblParticulars" runat="server" Text='<%# Bind("GL_NAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance This Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblThisYearBalance" runat="server" Text='<%# Bind("BALANCE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance Last Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastYearBalance" runat="server" Text='<%# Bind("BALANCE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <table style="width: 100%;" class="gridnoboder">
                                <tr>
                                    <td><strong>Total Expenses
                                    </strong>
                                    </td>
                                    <td style="width: 200px; text-align: right; border-top-style: solid;" ><strong>
                                        <asp:Label ID="lblThisYearEquityTotal" runat="server" Text=""></asp:Label>
                                    </strong>
                                    </td>
                                    <td style="width: 200px; text-align: right; border-top-style: solid;"><strong>
                                        <asp:Label ID="lblLastYearEquityTotal" runat="server" Text=""></asp:Label>
                                                                                                          </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3"></td>
                                </tr>
                            </table>
                            <br />
                                                
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

