<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Chart_of_account.aspx.cs" Inherits="Account_Chart_of_account" %>

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
            printWindow.close();
        }
    </script>
    <table style="width:100%">
        <tr>
            <td>Account Head<br />
                <asp:DropDownList ID="ddlAccountsHead" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAccountsHead_SelectedIndexChanged">
                  
                </asp:DropDownList></td>
            <td>Account Group<br />
                <asp:DropDownList ID="ddlGLAccMaster" runat="server"  CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlGLAccMaster_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td>General Ledger <br />
                <asp:DropDownList ID="ddlGLName"  CssClass="form-control" runat="server"></asp:DropDownList>
            </td>
            <td>Sub-ledger<br />
                <asp:DropDownList ID="ddlEnableSubledger" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlEnableSubledger_SelectedIndexChanged">
                    <asp:ListItem Value="Enable">Show</asp:ListItem>
                    <asp:ListItem Value="Disable">Hide</asp:ListItem>
                </asp:DropDownList>
            </td>
      
            <td>
                <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" CssClass="btn btn-primary" />
            </td>
            <td>
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click"  CssClass="btn btn-primary"/>
              </td>
            <td> 
                 <asp:Button ID="btnExcel" runat="server" Text="Excel" OnClick="btnExcel_Click"  CssClass="btn btn-primary" /></td>
        </tr>
    </table>
    <div id="hide" runat="server">
        <div id="print_div">
            <table runat="server">
                <tr>
                    <td>
                        <div id="divDetailedChart" visible="false" runat="server">
                            <asp:GridView ID="gridChartOfAccDetailed" runat="server" AutoGenerateColumns="False" CssClass="gridtable"
                                OnRowDataBound="gridChartOfAccDetailed_RowDataBound"  >
                                <Columns>
                                    <asp:TemplateField HeaderText="Account Head Code">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("ACC_HEAD_CODE") %>' ID="lblAccHeadCode" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Head">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("ACCOUNTS_HEAD") %>' ID="lblAccHead" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Financial Statement" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("FINANCIAL_STATEMENT") %>' ID="lblFinancialStatement" Visible="false" runat="server" />
                                            <asp:Label ID="lblShowFinancialStatement" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Group Code">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("GL_MASTER_CODE") %>' ID="lblGlMasterCode" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Group Name">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("GL_MASTER_NAME") %>' ID="lblGlMasterName" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="General Ledger Code">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("GL_CODE") %>' ID="lblGlCode" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="General Ledger Name">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("GL_NAME") %>' ID="lblGlname" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub Ledger Code" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("SUB_GL_CODE") %>' ID="lblSubLedCode" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub Ledger Name" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("SUB_GL_NAME") %>' ID="lblSubLedName" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>

                </tr>
                <tr>
                    <td>
                        <div runat="server" id="divChart" visible="false">
                            <asp:GridView ID="gridChart" runat="server" AutoGenerateColumns="False" CssClass="normalTable"
                                OnRowDataBound="gridChart_RowDataBound" OnDataBound="gridChart_DataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Account Head Code">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("ACC_HEAD_CODE") %>' ID="lblAccHeadCode" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Head">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("ACCOUNTS_HEAD") %>' ID="lblAccHead" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Financial Statement" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("FINANCIAL_STATEMENT") %>' ID="lblFinancialStatement" Visible="false" runat="server" />
                                            <asp:Label ID="lblShowFinancialStatement" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="General Ledger Master Code">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("GL_MASTER_CODE") %>' ID="lblGlMasterCode" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="General Ledger Master Name">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("GL_MASTER_NAME") %>' ID="lblGlMasterName" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GL Code">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("GL_CODE") %>' ID="lblGlCode" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GL Name">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Bind("GL_NAME") %>' ID="lblGlname" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>

