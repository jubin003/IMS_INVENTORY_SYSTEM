<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BalanceSheet.aspx.cs"
    Inherits="Account_Reports_BalanceSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-3" id="divBranch" runat="server">
                Branch
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-2">Fiscal Year
                <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-1"><br />
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" CssClass="btn btn-primary" />
            </div>
            <div class="col-md-1"><br>
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
                            <b>Balance Sheet As on                                        
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
                                    <td></td>
                                    <td style="width: 200px; text-align: right"><b><span style="text-decoration: underline">As on </span>
                                        <asp:Label ID="lblThisYearDate" runat="server" Style="text-decoration: underline"></asp:Label></b>
                                    </td>
                                    <td style="width: 200px; text-align: right"><b><span style="text-decoration: underline">As on </span>
                                        <asp:Label ID="lblLastYearDate" runat="server" Style="text-decoration: underline"></asp:Label></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td><u><strong>ASSETS</strong></u>
                                    </td>
                                    <td style="width: 200px;"></td>
                                    <td style="width: 200px;"></td>
                                </tr>
                            </table>
                            <asp:GridView ID="grd_Asset" runat="server" CssClass="gridnoboder" Width="100%" ShowHeader="false"
                                AutoGenerateColumns="False" OnRowDataBound="grd_Asset_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Assets">
                                        <ItemTemplate>
                                            <b>
                                                <asp:Label ID="lblBS_HEADING" runat="server" Text='<%# Bind("BS_HEADING") %>'></asp:Label></b>
                                            <asp:Label ID="lblBS_HEADING_PK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>

                                            <asp:GridView ID="grd_Detail" runat="server" Width="100%" CssClass="gridnoboder" AutoGenerateColumns="False"
                                                ShowHeader="false" OnRowDataBound="grd_Detail_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Particulars">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblBS_SUB_HEADING" runat="server" Text='<%# Bind("BS_SUB_HEADING") %>'>LinkButton</asp:LinkButton>
                                                            <asp:Label ID="lblBS_SUB_HEADING_PK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance This Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblThisYearBalance" runat="server" Text=''></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance Last Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastYearBalance" runat="server" Text=''></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <table style="width: 100%;" class="gridnoboder">
                                                <tr>
                                                    <td><strong>Total
                                                        <asp:Label ID="lblT_BS_HEADING" runat="server" Text='<%# Bind("BS_HEADING") %>'></asp:Label>
                                                    </strong>
                                                    </td>
                                                    <td style="width: 200px; text-align: right; border-top-style: solid; border-bottom-style: solid"><strong>
                                                        <asp:Label ID="lblThisYearTotal" runat="server" Text=""></asp:Label>
                                                    </strong>
                                                    </td>
                                                    <td style="width: 200px; text-align: right; border-top-style: solid; border-bottom-style: solid"><strong>
                                                        <asp:Label ID="lblLastYearTotal" runat="server" Text=""></asp:Label>
                                                    </strong>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <table style="width: 100%;" class="gridnoboder">
                                <tr>
                                    <td><strong>Total Assets</strong>
                                    </td>
                                    <td style="width: 200px; text-align: right; border-bottom-style: double"><strong>
                                        <asp:Label ID="lblThisYearAssetTotalGrand" runat="server" Text=""></asp:Label>
                                    </strong>
                                    </td>
                                    <td style="width: 200px; text-align: right; border-bottom-style: double"><strong>
                                        <asp:Label ID="lblLastYearAssetTotalGrand" runat="server" Text=""></asp:Label>
                                    </strong>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table style="width: 100%;" class="gridtable">
                                <tr>
                                    <td><u><strong>EQUITY AND LABILITIES</strong></u>
                                    </td>
                                    <td style="width: 200px;"></td>
                                    <td style="width: 200px;"></td>
                                </tr>
                            </table>



                            <asp:GridView ID="grd_Equity" runat="server" CssClass="gridnoboder" Width="100%" ShowHeader="false"
                                AutoGenerateColumns="False" OnRowDataBound="grd_Equity_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Assets">
                                        <ItemTemplate>
                                            <b>
                                                <asp:Label ID="lblBS_HEADING" runat="server" Text='<%# Bind("BS_HEADING") %>'></asp:Label></b>
                                            <asp:Label ID="lblBS_HEADING_PK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                            <asp:GridView ID="grd_Detail" runat="server" Width="100%" CssClass="gridnoboder" AutoGenerateColumns="False"
                                                ShowHeader="false" OnRowDataBound="grd_Detail_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Particulars">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblBS_SUB_HEADING" runat="server" Text='<%# Bind("BS_SUB_HEADING") %>'>LinkButton</asp:LinkButton>
                                                            <asp:Label ID="lblBS_SUB_HEADING_PK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance This Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblThisYearBalance" runat="server" Text=''></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance Last Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastYearBalance" runat="server" Text=''></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                            <table style="width: 100%;" class="gridnoboder">
                                                <tr>
                                                    <td><strong>Total
                                                        <asp:Label ID="lblT_BS_HEADING" runat="server" Text='<%# Bind("BS_HEADING") %>'></asp:Label>
                                                    </strong>
                                                    </td>
                                                    <td style="width: 200px; text-align: right; border-top-style: solid; border-bottom-style: solid"><strong>
                                                        <asp:Label ID="lblThisYearTotal" runat="server" Text=""></asp:Label>
                                                    </strong>
                                                    </td>
                                                    <td style="width: 200px; text-align: right; border-top-style: solid; border-bottom-style: solid"><strong>
                                                        <asp:Label ID="lblLastYearTotal" runat="server" Text=""></asp:Label>
                                                    </strong>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <table style="width: 100%;" class="gridnoboder" runat="server" visible="false">
                                <tr>
                                    <td><strong>Total Equity</strong>
                                    </td>
                                    <td style="width: 200px; text-align: right; border-bottom-style: double"><strong>
                                        <asp:Label ID="lblThisYearEquityTotalGrand" runat="server" Text=""></asp:Label>
                                    </strong>
                                    </td>
                                    <td style="width: 200px; text-align: right; border-bottom-style: double"><strong>
                                        <asp:Label ID="lblLastYearEquityTotalGrand" runat="server" Text=""></asp:Label>
                                    </strong>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%;" class="gridtable">
                                <tr>
                                    <td><strong>Labilities</strong>
                                    </td>
                                    <td style="width: 200px;"></td>
                                    <td style="width: 200px;"></td>
                                </tr>
                            </table>

                            <asp:GridView ID="grd_Labilities" runat="server" CssClass="gridnoboder" Width="100%" ShowHeader="false"
                                AutoGenerateColumns="False" OnRowDataBound="grd_Labilities_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Assets">
                                        <ItemTemplate>
                                            <b>
                                                <asp:Label ID="lblBS_HEADING" runat="server" Text='<%# Bind("BS_HEADING") %>'></asp:Label></b>
                                            <asp:Label ID="lblBS_HEADING_PK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                            <asp:GridView ID="grd_Detail" runat="server" Width="100%" CssClass="gridnoboder" AutoGenerateColumns="False"
                                                ShowHeader="false" OnRowDataBound="grd_Detail_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Particulars">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblBS_SUB_HEADING" runat="server" Text='<%# Bind("BS_SUB_HEADING") %>'>LinkButton</asp:LinkButton>
                                                            <asp:Label ID="lblBS_SUB_HEADING_PK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance This Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblThisYearBalance" runat="server" Text=''></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance Last Year">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastYearBalance" runat="server" Text=''></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                            <table style="width: 100%;" class="gridnoboder">
                                                <tr>
                                                    <td><strong>Total
                                                        <asp:Label ID="lblT_BS_HEADING" runat="server" Text='<%# Bind("BS_HEADING") %>'></asp:Label>
                                                    </strong>
                                                    </td>
                                                    <td style="width: 200px; text-align: right; border-top-style: solid; border-bottom-style: solid"><strong>
                                                        <asp:Label ID="lblThisYearTotal" runat="server" Text=""></asp:Label>
                                                    </strong>
                                                    </td>
                                                    <td style="width: 200px; text-align: right; border-top-style: solid; border-bottom-style: solid"><strong>
                                                        <asp:Label ID="lblLastYearTotal" runat="server" Text=""></asp:Label>
                                                    </strong>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <table style="width: 100%;" class="gridnoboder">
                                <tr>
                                    <td><strong>Total Labilities</strong>
                                    </td>
                                    <td style="width: 200px; text-align: right; border-bottom-style: double"><strong>
                                        <asp:Label ID="lblThisYearLabilitiesTotalGrand" runat="server" Text=""></asp:Label>
                                    </strong>
                                    </td>
                                    <td style="width: 200px; text-align: right; border-bottom-style: double"><strong>
                                        <asp:Label ID="lblLastYearLabilitiesTotalGrand" runat="server" Text=""></asp:Label>
                                    </strong>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%;" class="gridnoboder">
                                <tr>
                                    <td><strong>Total Equity and Labilities</strong>
                                    </td>
                                    <td style="width: 200px; text-align: right; border-bottom-style: double"><strong>
                                        <asp:Label ID="lblThisYearLabilitiesEquityTotalGrand" runat="server" Text=""></asp:Label>
                                    </strong>
                                    </td>
                                    <td style="width: 200px; text-align: right; border-bottom-style: double"><strong>
                                        <asp:Label ID="lblLastYearLabilitiesEquityTotalGrand" runat="server" Text=""></asp:Label>
                                    </strong>
                                    </td>
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

