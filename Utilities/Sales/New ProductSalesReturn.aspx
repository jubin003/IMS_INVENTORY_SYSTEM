<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="New ProductSalesReturn.aspx.cs" Inherits="Utilities_Sales_New_ProductSales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="width: 800px;" class="gridtable">
        <tr>
            <td>Return Date<br />
                <asp:TextBox ID="txtReturnDate" class="form-control  datepicker" runat="server" Width="120px"></asp:TextBox>
            </td>
            <td>Fiscal Year<br />
                <asp:TextBox ID="txtFiscalYear" runat="server" CssClass="form-control" Width="120px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Customer Code<br />
                <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="form-control" Width="80px"
                    AutoPostBack="true" OnTextChanged="txtProdCode_TextChanged"></asp:TextBox>
            </td>
            <td>Customer<br />
                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control" Width="300px"
                    Font-Size="Larger" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                </asp:DropDownList>
                <script>
                    $('#<%=ddlCustomer.ClientID%>').chosen();
                </script>
            </td>
            <td>Address
                <asp:TextBox ID="txtCustomerAddress" runat="server" CssClass="form-control" Style="width: 200px"></asp:TextBox>
            </td>
            <td style="padding-left: 15px; border-left: dashed 1px; vertical-align: top;">
                <table>
                    <tr>
                        <td>Return Note<br />
                            <asp:TextBox ID="txtCreditNoteRemarks" runat="server" TextMode="MultiLine" CssClass="form-control" Width="500px" Height="100px"></asp:TextBox>
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
            <td rowspan="3"></td>
        </tr>
        <tr>
            <td>PAN/VAT No
                            <br />
                <asp:TextBox ID="txtCustomerPANVAT" runat="server" CssClass="form-control" Style="width: 200px" Text="0"
                    AutoPostBack="true" MaxLength="9" TextMode="Number"></asp:TextBox>
            </td>
            <td>Contact No<br />
                <asp:TextBox ID="txtCustomerContact" runat="server" CssClass="form-control" Style="width: 200px"></asp:TextBox>
            </td>

        </tr>


    </table>


    <table style="width: 1300px; border-bottom: solid 1px" class="gridtable">
        <tr>
            <td></td>
        </tr>
    </table>
    <br />
    <table style="width: 1200px" class="gridtable">
        <tr>
            <td>Code<br />
                <asp:TextBox ID="txtProductCode" runat="server" CssClass="form-control" Width="80px"
                    AutoPostBack="true" OnTextChanged="txtProductCode_TextChanged"></asp:TextBox>
            </td>
            <td>Product Name<br />
                <asp:DropDownList ID="ddlProductName" runat="server" CssClass="form-control" Width="300px"
                    Font-Size="Larger" AutoPostBack="true" OnSelectedIndexChanged="ddlProductName_SelectedIndexChanged">
                </asp:DropDownList>
                <script>
                    $('#<%=ddlProductName.ClientID%>').chosen();
                </script>
            </td>

            <td runat="server" id="divBatch" visible="false">Batch<br />
                <asp:DropDownList ID="ddlBatch" Width="80px" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"></asp:DropDownList>
                <asp:TextBox ID="txtExpDate" runat="server" Visible="false"></asp:TextBox>
            </td>
            <td runat="server" visible="true">Available Qty<br />
                <asp:TextBox ID="txtAvilableQty" runat="server" CssClass="form-control" Width="80px" Enabled="false"></asp:TextBox>
            </td>
            <td>
                <br />
                <asp:Label ID="lblUUnit" runat="server" Text=""></asp:Label>
            </td>
            <td id="tdDualUnit" runat="server" visible="false">Alternate Unit<br />
                <asp:TextBox ID="txtUQty" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true" OnTextChanged="txtUQty_TextChanged"></asp:TextBox>
            </td>
            <td>Basic Unit<br />
                <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true" OnTextChanged="txtQty_TextChanged"></asp:TextBox>
            </td>
            <td>
                <br />
                <asp:Label ID="lblBUnit" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <br />
                <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" Width="80px" Enabled="false"></asp:DropDownList>
            </td>

            <td>Rate/Basic Unit<br />
                <asp:TextBox ID="txtRate" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true" OnTextChanged="txtRate_TextChanged"></asp:TextBox>
            </td>
            <td>Amount<br />
                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true" OnTextChanged="txtAmount_TextChanged"></asp:TextBox>
            </td>
            <td id="tdSchDisc" runat="server" visible="true">Sche Disc.
                    <br />
                <asp:TextBox ID="txtScheDisc" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
            </td>
            <td>
                <br />
                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
    <br />
    <table style="border: solid; width: 1300px; font-size: 12px;">
        <tr>
            <td style="width: 1000px">
                <div style="height: 300px; width: 1000px; overflow: scroll; overflow-x: hidden; margin-top: 10px;">
                    <div style="background-color: cadetblue; text-align: center; width: 975px">
                        <b>Product Sales Return Detail </b>
                    </div>
                    <br />
                    <asp:GridView ID="grdProdSaleReturn" runat="server" AutoGenerateColumns="False" Width="975px" CssClass="gridtable"
                        Font-Size="11px" OnRowCommand="grdProdSaleReturn_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    <asp:Label ID="lblProductPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Bind("PRODUCT_CODE") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("PRODUCT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Batch">
                                <ItemTemplate>
                                    <asp:Label ID="lblBatch" runat="server" Text='<%# Bind("BATCH") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Expiry Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblExpDate" runat="server" Text='<%# Bind("EXPIRY_DATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Alternate Qty" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblUQty" runat="server" Text='<%# Bind("U_QUANTITY") %>'></asp:Label>
                                    <asp:Label ID="lblUUnit" runat="server" Text='<%# Bind("U_UNIT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" runat="server" Text='<%# Bind("QUANTITY") %>'></asp:Label>
                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("UNIT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate">
                                <ItemTemplate>
                                    <asp:Label ID="lblRate" runat="server" Text='<%# Bind("RATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemTotal" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Discount">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscount" runat="server" Text='<%# Bind("SCHE_DISC") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAfterScheDisc" runat="server" Text='<%# Bind("AFTER_SCHE_DISC")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton2" CommandName="Remove" ImageUrl="~/images/icons/deletes.png" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="50px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
            <td style="vertical-align: top; padding-left: 5px; padding-right: 5px;">
                <table style="width: 300px;">
                    <tr>
                        <br />
                        <td>Sales Return Total</td>
                        <td></td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblSalesReturnTotal" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="trScheDis" runat="server">
                        <td style="width: 90px;">Sche. Disc</td>
                        <td style="width: 45px;">&nbsp;</td>
                        <td style="text-align: right; width: 35px;">
                            <asp:Label ID="lblTotalScheDisc" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr id="trAftScheDis" runat="server">
                        <td style="width: 90px;">After Sch.Disc</td>
                        <td style="width: 45px;">&nbsp;</td>
                        <td style="text-align: right; width: 35px;">
                            <asp:Label ID="lblAfterSchDiscount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Trade Discount</td>
                        <td>
                            <asp:TextBox ID="txtDiscount" runat="server" Width="35px" Text="0" AutoPostBack="true"></asp:TextBox>
                            %</td>
                        <td style="text-align: right;">
                            <asp:TextBox ID="txtDiscountAmount" runat="server" Width="80px" Style="text-align: right" AutoPostBack="true"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Total</td>
                        <td></td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblTotalAmount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" id="divVAT1">
                        <td>VAT</td>
                        <td>
                            <asp:TextBox ID="txtVATPercent" runat="server" Width="35px" Text="13"></asp:TextBox>
                            %</td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblVAT" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr runat="server" id="divVAT2">
                        <td>Grand Total</td>
                        <td></td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblGrandTotal" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" visible="false" id="trRoundOff">

                        <td>Rounding</td>
                        <td></td>
                        <td style="text-align: right;">
                            <asp:TextBox ID="txtRound" runat="server" Width="80px" Text="" Style="text-align: right" OnTextChanged="txtRound_TextChanged" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Invoice Amount</td>
                        <td></td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblInvoiceAmount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" Width="100%" OnClick="btnSave_Click" /><br />
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

