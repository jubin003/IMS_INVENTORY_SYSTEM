<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Stock.aspx.cs" Inherits="Reports_General_Stock" %>

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
        <div class="row" id="divBranch" runat="server">
            <div class="col-md-3">
                Branch
                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ></asp:DropDownList>
            </div>
        </div>
        <div class="col-md-2">
            Product Category<br />
            <asp:DropDownList ID="ddlCategoryFilter" runat="server" CssClass="form-control"
                AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryFilter_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="col-md-2">
            Product Sub Category
                    <br />
            <asp:DropDownList ID="ddlSubCategoryFilter" runat="server" CssClass="form-control"
                AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategoryFilter_SelectedIndexChanged">
            </asp:DropDownList>

        </div>
        <div class="col-md-2">
            Product Name<br />
            <asp:DropDownList ID="ddlProductFilter" runat="server" CssClass="form-control">
            </asp:DropDownList>
            <script>
                $('#<%=ddlProductFilter.ClientID%>').chosen();
            </script>
        </div>

        <div class="row">
            <div class="col-md-2">
                Fiscalyear<br />
                <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="col-md-1 margin-top:auto;">
                <br />
                <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-success" OnClick="btnView_Click" />
            </div>
            <div class="col-md-2 margin-top:auto;">
                <br />

                <asp:Button ID="btnPrint" runat="server" Text="Print" Width="100%" CssClass="btn btn-success" OnClick="btnPrint_Click" />
            </div>
        </div>
        <div class="row" id="divPrint">
            <div class="col-md-12" id="divhide" runat="server" visible="false" style="left: 0px; top: 0px">
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
                            <asp:Label ID="lblHeading" runat="server" Text="Stock" Font-Size="18px" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblDate" runat="server" Text="" Font-Size="14px" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grdStock" runat="server"
                                CssClass="gridtable"  Width="100%" ShowFooter="true" AutoGenerateColumns="false"
                                OnRowCreated="grdStock_RowCreated" OnRowDataBound="grdStock_RowDataBound"
                               >
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblProduct" runat="server" Text='<%# Bind("PRODUCT_NAME") %>' OnClick="lblProduct_Click"></asp:LinkButton>
                                            <asp:Label ID="lblFiscalYear" runat="server" Text='<%# Bind("fiscal_year") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblProduct_id" runat="server" Text='<%# Bind("PRODUCT_ID") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <b>Total</b>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBatchNo" runat="server" Text='<%# Bind("BATCH_NUMBER") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Bind("EXPIRY_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblColourId" runat="server" Text='<%# Bind("COLOUR_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSizeName" runat="server" Text='<%# Bind("SIZE_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblManufacturerName" runat="server" Text='<%# Bind("MANUFACTURE_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOpening" runat="server" Text='<%# Bind("OPENING") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblOpeningF" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Value">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOpeningBalance" runat="server" Text='<%# Bind("OPENING_BALANCES") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblOpeningBalanceF" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchase" runat="server" Text='<%# Bind("PURCHASE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblPurchaseF" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Value">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseAmount" runat="server" Text='<%# Bind("PURCHASE_AMOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblPurchaseAmountF" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseReturn" runat="server" Text='<%# Bind("PURCHASE_RETURN") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblPurchaseReturnF" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseReturnAmount" runat="server" Text='<%# Bind("PURCHASE_RETURN_AMOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblPurchaseReturnAmtF" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSales" runat="server" Text='<%# Bind("SALES") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblSalesF" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalesAmount" runat="server" Text='<%# Bind("SALES_AMOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblSalesAmountF" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalesReturn" runat="server" Text='<%# Bind("SALES_RETURN") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblSalesReturnF" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSalesReturnAmount" runat="server" Text='<%# Bind("SALES_RETURN_AMOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblSalesReturnAmtF" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity">
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
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

