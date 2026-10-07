<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ClosingBalance.aspx.cs" Inherits="Administration_ClosingBalance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row" id="divBranch" runat="server">
            <div class="col-md-4">
                Branch<br />
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
        <div class="row" runat="server" id="divFilter">
            <div class="col-md-3">
                Perform Closing Balance for Fiscal Year<br />
                <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>

            <div class="col-md-3 margin-top:auto;">
                <br />
                <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-success" OnClick="btnView_Click" />
                <asp:Button ID="btnClosing" runat="server" Text="Do Closing" CssClass="btn btn-success" OnClick="btnClosing_Click" />
                <asp:CheckBox ID="chkFinalClosing" runat="server" Text="Do Final Closing" Checked="true"/>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                 <asp:GridView ID="grdStock" runat="server"
                    CssClass="gridtable" Width="100%" ShowFooter="true" AutoGenerateColumns="false"
                    OnRowDataBound="grdStock_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product">
                            <ItemTemplate>
                                <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("PRODUCT_NAME") %>'></asp:Label>
                                <asp:Label ID="lblFiscalYear" runat="server" Text='<%# Bind("fiscal_year") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblProduct_id" runat="server" Text='<%# Bind("PRODUCT_ID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>Total</b>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Batch No.">
                            <ItemTemplate>
                                <asp:Label ID="lblBatchNo" runat="server" Text='<%# Bind("BATCH_NUMBER") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Expiry Date">
                            <ItemTemplate>
                                <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Bind("EXPIRY_DATE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblManufacturerID" runat="server" Text='<%# Bind("manufacture_id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:Label ID="lblOpening" runat="server" Text='<%# Bind("OPENING") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblOpeningF" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Value">
                            <ItemTemplate>
                                <asp:Label ID="lblOpeningBalance" runat="server" Text='<%# Bind("OPENING_BALANCES") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblOpeningBalanceF" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchase" runat="server" Text='<%# Bind("PURCHASE") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblPurchaseF" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Value">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchaseAmount" runat="server" Text='<%# Bind("PURCHASE_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblPurchaseAmountF" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchaseReturn" runat="server" Text='<%# Bind("PURCHASE_RETURN") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblPurchaseReturnF" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" HeaderText="Value">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchaseReturnAmount" runat="server" Text='<%# Bind("PURCHASE_RETURN_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblPurchaseReturnAmtF" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:Label ID="lblSales" runat="server" Text='<%# Bind("SALES") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblSalesF" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" HeaderText="Value">
                            <ItemTemplate>
                                <asp:Label ID="lblSalesAmount" runat="server" Text='<%# Bind("SALES_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblSalesAmountF" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:Label ID="lblSalesReturn" runat="server" Text='<%# Bind("SALES_RETURN") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblSalesReturnF" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" HeaderText="Value">
                            <ItemTemplate>
                                <asp:Label ID="lblSalesReturnAmount" runat="server" Text='<%# Bind("SALES_RETURN_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblSalesReturnAmtF" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:Label ID="lblAdjustment" runat="server" Text='<%# Bind("ADJUSTMENT") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblAdjustmentF" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:Label ID="lblClosing" runat="server" Text='<%# Bind("CLOSING") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label Font-Bold="true" ID="lblCloQtyF" runat="server"></asp:Label>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Value">
                            <ItemTemplate>
                                <asp:Label ID="lblClosingBalance" runat="server" Text='<%# Bind("closing_balance") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label Font-Bold="true" ID="lblCloValF" runat="server"></asp:Label>
                            </FooterTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
       

    </div>
</asp:Content>

