<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockAdjustment.aspx.cs" Inherits="Reports_Adjustment_StockAdjustment" %>

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
                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                Product Category<br />
                <asp:DropDownList ID="ddlCategoryFilter" runat="server" CssClass="form-control"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryFilter_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="col-md-3">
                Product Sub Category
                    <br />
                <asp:DropDownList ID="ddlSubCategoryFilter" runat="server" CssClass="form-control"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategoryFilter_SelectedIndexChanged">
                </asp:DropDownList>

            </div>
            <div class="col-md-3">
                Product Name<br />
                <asp:DropDownList ID="ddlProductFilter" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Date From               
                    <asp:TextBox ID="txtDateFrom" runat="server" class="form-control  datepicker"
                        placeholder="dd/mm/yyyy"></asp:TextBox>
            </div>
            <div class="col-md-2">
                To
                    <asp:TextBox ID="txtDateTo" runat="server" class="form-control  datepicker"
                        placeholder="dd/mm/yyyy"></asp:TextBox>
            </div>
            <div class="col-md-2 margin-top:auto;">
                <br />
                <asp:Button ID="btnView" runat="server" Text="View" Width="100%" CssClass="btn btn-success" OnClick="btnView_Click" />
            </div>
            <div class="col-md-2 margin-top:auto;">
                <br />
                <asp:Button ID="btnPrint" runat="server" Text="Print" Width="100%" CssClass="btn btn-success" OnClick="btnPrint_Click" />
            </div>
        </div>
        <div class="row" id="divPrint">
            <div class="col-md-12" id="divhide" runat="server" visible="false">
                <div class="row">
                    <div class="col-lg-12">
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
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:GridView ID="grdStock" runat="server" AutoGenerateColumns="False"
                            CssClass="gridtable" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Adjust Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("DATE_NP") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("PRODUCTNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Batch No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBatchNo" runat="server" Text='<%# Bind("BATCH_NUMBER") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expiry Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Bind("EXPIRY_DATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Colour">
                                    <ItemTemplate>
                                        <asp:Label ID="lblColourId" runat="server" Text='<%# Bind("COLOUR_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Size">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSizeName" runat="server" Text='<%# Bind("SIZE_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Manufacturer">
                                    <ItemTemplate>
                                        <asp:Label ID="lblManufacturerName" runat="server" Text='<%# Bind("MANUFACTURE_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("QUANTITY") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("REMARKS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Adjusted by">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAdjustedby" runat="server" Text='<%# Bind("ADJUSTED_BY") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

