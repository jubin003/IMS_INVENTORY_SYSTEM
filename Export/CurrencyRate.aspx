<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CurrencyRate.aspx.cs" Inherits="Export_CurrencyRate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCurrencyName" runat="server" Text="Currency Name: "></asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList></td>
                <td>&nbsp;</td>
                <td>
                    <asp:Label ID="lblExchangeDate" runat="server" Text="Exchange Date: "></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtExchangeDate" runat="server" CssClass="form-control datepicker" AutoPostBack="true"></asp:TextBox></td>
                <td>&nbsp;</td>
                <td>
                    <asp:Label ID="lblExchangeRate" runat="server" Text="Exchange Rate: "></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtExchangeRate" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox></td>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="btnsave" runat="server" OnClick="btnsave_Click" Text="Save" CssClass="btn btn-success" /></td>

            </tr>
        </table>
        <br />
        <div id="divgrid">
            <asp:GridView ID="gvTodayRates" runat="server" AutoGenerateColumns="False"
                CssClass="gridtable" Width="600px" Font-Size="11px"
                OnRowDataBound="gvTodayRates_RowDataBound"
                EmptyDataText="No rates found for today.">
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <asp:Label ID="lblSno" runat="server" Text='<%# Container.DataItemIndex + 1 %>' />
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Currency">
                        <ItemTemplate>
                            <asp:Label ID="lblCurrency" runat="server" Text='<%# Bind("CURRENCY_TYPE_ID") %>' Visible="false" />
                             <asp:Label ID="lblCurrencyName" runat="server" Text='' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText=" Today's Exchange Rate">
                        <ItemTemplate>
                            <asp:Label ID="lblRate" runat="server" Text='<%# Bind("EXCHANGE_RATE") %>' />
                        </ItemTemplate>
                        <ItemStyle Width="120px" HorizontalAlign="Right" />
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>

    </div>
</asp:Content>

