<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PurchaseReturn.aspx.cs" 
    Inherits="Utilities_Purchase_PurchaseReturn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid form-group-sm">
        <div class="row">
            <div class="col-md-1">
                Date Wise<br />
                <asp:CheckBox ID="chkDateWise" runat="server" OnCheckedChanged="chkDateWise_CheckedChanged" AutoPostBack="true" />
            </div>
            <div class="col-md-2" runat="server" visible="true" id="NoDate1">
                Fiscal Year<br />
                <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-2" runat="server" visible="true" id="NoDate2">
                Dakhila No<br />
                <asp:TextBox ID="txtDakhilNo" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-2" runat="server" visible="false" id="WithDate1">
                From Date<br />
                <asp:TextBox ID="txtFromDate" AutoComplete="off" runat="server" CssClass="form-control datepicker"></asp:TextBox>
            </div>
            <div class="col-md-2" runat="server" visible="false" id="WithDate2">
                To Date<br />
                <asp:TextBox ID="txtToDate" AutoComplete="off" runat="server" CssClass="form-control datepicker"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-primary" OnClick="btnShow_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:GridView ID="grdPurchase" runat="server" AutoGenerateColumns="False" CssClass="gridtable" Width="100%" OnRowCommand="grdPurchase_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Sno">
                            <ItemTemplate>
                                <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dakhila No">
                            <ItemTemplate>
                                <asp:Label ID="lblDakhilaNumber" runat="server" Text='<%# Bind("DAKHILA_NUMBER") %>'></asp:Label>
                                <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dakhila Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDakhilaDay" runat="server" Text='<%# Bind("DAKHILA_DATE_NP") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Supplier ">
                            <ItemTemplate>
                                <asp:Label ID="lblSupplierName" runat="server" Text='<%# Bind("SUPPLIER_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bill Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblBillAmount" runat="server" Text='<%# Bind("SUB_TOTAL_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discount">
                            <ItemTemplate>
                                <asp:Label ID="lblDiscount" runat="server" Text='<%# Bind("DISCOUNT_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                             <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sub Total">
                            <ItemTemplate>
                                <asp:Label ID="lblSubTotal" runat="server" Text='<%# Bind("TOTAL_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                             <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VAT">
                            <ItemTemplate>
                                <asp:Label ID="lblVAT" runat="server" Text='<%# Bind("TAX_VAT_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                             <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Grand Total">
                            <ItemTemplate>
                                <asp:Label ID="lblGrandTotal" runat="server" Text='<%# Bind("GRAND_TOTAL") %>'></asp:Label>
                            </ItemTemplate>
                             <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Round off">
                            <ItemTemplate>
                                <asp:Label ID="lblRoundOff" runat="server" Text='<%# Bind("ROUND_OFF") %>'></asp:Label>
                            </ItemTemplate>
                             <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payable">
                            <ItemTemplate>
                                <asp:Label ID="lblPayable" runat="server" Text='<%# Bind("INVOICE_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                             <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Purchase Type">
                            <ItemTemplate>
                                <asp:Label ID="lblPurchaseType" runat="server" Text='<%# Bind("PURCHASE_TYPE_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="Alter" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

