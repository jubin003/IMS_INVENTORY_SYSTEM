<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Invoice_To_Dakhila.aspx.cs" Inherits="Mapping_sales_Invoice_To_Dakhila" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <script type="text/javascript">
    function printPartOfPage() {
        var printContent = document.getElementById('<%= div_print.ClientID %>');
        if (!printContent) {
            alert('Nothing to print.');
            return false;
        }

        var printWindow = window.open('', 'Print' + new Date().getTime(),
            'width=1000,height=700,menubar=no,toolbar=no,location=no,status=no,scrollbars=yes');

        printWindow.document.open();
        printWindow.document.write(
            '<html><head><title>Print</title>' +
            '<style>' +
            '  body { margin: 0 40px; font-family: Arial, sans-serif; font-size: 12px; }' +
            '  table { width: 100%; border-collapse: collapse; }' +
            '  th, td { border: 1px solid #444; padding: 4px 6px; }' +
            '  th { background: #eee; text-align: left; }' +
            '</style></head><body>' +
            printContent.innerHTML +
            '</body></html>'
        );
        printWindow.document.close();

        printWindow.onload = function () {
            printWindow.focus();
            printWindow.print();
            printWindow.close();
        };

        return false;
    }
</script>
    <div class="container mt-4">
        <div class="row align-items-end">
            <div class="col-md-3">
                <asp:Label runat="server" ID="lblFY" CssClass="form-label d-block mb-1">Fiscal Year (Invoice)</asp:Label>
                <asp:DropDownList runat="server" ID="ddlFY" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFY_SelectedIndexChanged">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
            </div>

          <div class="col-md-3">
                <asp:Label runat="server" ID="lblInvoice" CssClass="form-label d-block mb-1">Invoice No.</asp:Label>
                <asp:TextBox runat="server" ID="txtInvoice" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtInvoice_TextChanged"></asp:TextBox>
                <asp:HiddenField runat="server" ID="hdnInvoiceId" />
               </div>

            <div class="col-md-3">
                <asp:Label runat="server" ID="lblFYdakh" CssClass="form-label d-block mb-1">Fiscal Year (Dakhila)</asp:Label>
                <asp:DropDownList runat="server" ID="ddlFYdakh" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFYdakh_SelectedIndexChanged">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="col-md-3">
                <asp:Label runat="server" ID="lblDakhila" CssClass="form-label d-block mb-1">Dakhhila No.</asp:Label>
                <asp:DropDownList runat="server" ID="ddlDakhila" CssClass="form-control">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="col-md-1">
                <br />
                <asp:Button runat="server" ID="btnMap" OnClick="btnMap_Click" CssClass="btn btn-success" Text="Map" />
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Label runat="server" ID="lblInvoiceStatus" CssClass="text-danger"></asp:Label>
                </div>
            </div>
            <div class="col-md-1">
                <br />
                <asp:Button runat="server" ID="btnPrint" OnClientClick="return printPartOfPage();" CssClass="btn btn-success" Text="Print" />
            </div>
        </div>
    </div>

    <div class="container" id="div_print" runat="server">
        <br />
        <asp:GridView runat="server" ID="grdITDList" AutoGenerateColumns="false"
            OnRowDataBound="grdITDList_RowDataBound"
            CssClass="table table-bordered table-striped table-hover align-middle"
            ShowHeaderWhenEmpty="true" Width="100%"
            HeaderStyle-CssClass="table-light">
            <Columns>

                <asp:TemplateField HeaderText="SN">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSnG" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Invoice number">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblInvoicenum"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Fiscal Year">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblIFY"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Dakhila number">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDakhilanum"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Fiscal Year">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDFY"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblMappingId" Text='<%# Eval("MAPPING_ID") %>' Visible="false"></asp:Label>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" CssClass="btn btn-danger" />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
