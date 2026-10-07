<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Export_Issue_List.aspx.cs" Inherits="Mapping_sales_Export_Issue_List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function printPartOfPage() {
            var printContent = document.getElementById('<%= div_print.ClientID %>');

            if (!printContent) {
                alert('Nothing to print.');
                return false;
            }

            var printWindow = window.open('', 'Print' + new Date().getTime(),
                'left=0,top=0,width=800,height=600');

            printWindow.document.write(printContent.innerHTML);

            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
            return false;
        }
    </script>
    <div class="container">
        <h2>Export Invoice List</h2>
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblFY">Fiscal Year</asp:Label>
                    <asp:DropDownList runat="server" ID="ddlFY" 
                        CssClass="form-control width-200"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlFY_SelectedIndexChanged">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>



                </td>
                <td style="padding: 10px;">
                    <asp:Label runat="server" ID="lblInvoice">Invoice No.</asp:Label>
                    <asp:TextBox runat="server" ID="txtInvoice" 
                        CssClass="form-control width-200"
                        AutoPostBack="true"
                        OnTextChanged="txtInvoice_TextChanged">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdnInvoiceId" runat="server" Value="" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnShow" OnClick="btnShow_Click" CssClass="btn btn-success" Text="Show" />
                </td>
                <td style="padding-left:10px;">
                    <asp:Button runat="server" ID="btnPrint" OnClientClick="return printPartOfPage();" CssClass="btn btn-success" Text="Print" />

                </td>
            </tr>
        </table>



        <div class="container" id="div_print" visible="true" runat="server">

            <table style="width: 100%" id="tblCDetail" runat="server" visible="false">
                <tr>
                    <td style="width: 15%; text-align: center; vertical-align: top;">
                        <asp:Image ID="sImage1" runat="server" Width="100px" ImageUrl="~/images/logo.png" />
                    </td>
                    <td style="text-align: center;">
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Label ID="lblCompanyName" runat="server" Text="" Style="font-weight: bold; font-size: 32px;"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Label ID="lblCompanyAddress" runat="server" Text="" Style="font-size: 12pt;"></asp:Label></td>
                            </tr>
                            <tr runat="server" id="divEmail">
                                <td style="text-align: center;">
                                    <asp:Label ID="lblWebsite" runat="server" Text="" Style="font-size: 12pt;"></asp:Label>
                                    <asp:Label ID="lblDivider" runat="server" Text="|" Style="font-size: 12pt;"></asp:Label>
                                    <asp:Label ID="lblEmail" runat="server" Text="" Style="font-size: 12pt;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Label ID="lblPhone1" runat="server" Text="" Style="font-size: 12pt;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; height: 22px">
                                    <asp:Label ID="Label1" runat="server" Style="font-size: 12pt; font-weight: normal; verdana;" Text="PAN NO"></asp:Label>
                                    <asp:Label ID="lblPanNo" runat="server" Style="font-size: 12pt; font-weight: normal; verdana;"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Label ID="lblHeading" runat="server" Text="" Style="font-size: 16pt;"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <div class="container" runat="server" visible="false" id="detail">
                <div class="col text-end">
                    <div>
                        <strong>
                            <asp:Label runat="server" ID="lbliv">Invoice No.:</asp:Label></strong>
                        <asp:Label runat="server" ID="lblivn"></asp:Label>
                    </div>
                    <div>
                        <strong>
                            <asp:Label runat="server" ID="lblc">Customer Name:</asp:Label></strong>
                        <asp:Label runat="server" ID="lblcn"></asp:Label>
                    </div>
                    <div>
                        <strong>
                            <asp:Label runat="server" ID="lbla">Address:</asp:Label></strong>
                        <asp:Label runat="server" ID="lblad"></asp:Label>,<asp:Label runat="server" ID="lblCountry"></asp:Label>
                    </div>
                </div>
            </div>

            <asp:GridView runat="server" AutoGenerateColumns="false" ID="grdDakhila"
                OnRowDataBound="grdDakhila_RowDataBound"
                ShowHeaderWhenEmpty="false"
                CssClass="table table-bordered table-striped table-hover align-middle fs-6"
                HeaderStyle-CssClass="table-light" Width="100%">
                <Columns>

                    <asp:TemplateField HeaderText="SN">
                        <ItemTemplate>
                            <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblSnG" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Dakhila No">
                        <ItemTemplate>
                            <asp:Label ID="lblDakhilano" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Supplier">
                        <ItemTemplate>
                            <asp:Label ID="lblSupplier" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Dakhila Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDakhiladate" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:Label ID="lblAmount" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>


                </Columns>
            </asp:GridView>

        </div>
    </div>
</asp:Content>
