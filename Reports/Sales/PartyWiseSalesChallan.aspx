<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PartyWiseSalesChallan.aspx.cs" Inherits="Reports_Sales_PartyWiseSalesChallan" %>

<%@ Register src="../../uc/CompanyName.ascx" tagname="CompanyName" tagprefix="uc1" %>

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
        <div class="row" id="divGrid" runat="server">
            <div class="row">
                <div class="col-md-2">
                    Party Name
                <asp:DropDownList ID="ddlCustomerName" runat="server" class="form-control">
                </asp:DropDownList>

                </div>
                <div class="col-md-2">
                    Challan Date From
               
                    <asp:TextBox ID="txtChalanDateFrom" runat="server" class="form-control  datepicker"
                        placeholder="dd/mm/yyyy"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    To
                
                    <asp:TextBox ID="txtChalanDateTo" runat="server" class="form-control  datepicker"
                        placeholder="dd/mm/yyyy"></asp:TextBox>
                </div>

                <div class="col-md-2 margin-top:auto;">
                    <asp:Button ID="btnList" runat="server" Text="List" Width="100%" CssClass="btn btn-success" OnClick="btnList_Click" />
                </div>
                <div class="col-md-2 margin-top:auto;">
                    <asp:Button ID="btnPrint" runat="server" Text="Print" Width="100%" CssClass="btn btn-success" OnClick="btnPrint_Click" />
                </div>
            </div>
        </div>
        <div class="row" id="divPrint">
            <div class="col-md-12" id="divhide" runat="server" visible="false">
                <table style="width: 200mm; text-align: center;">
                    <tr>
                        <td colspan="4">
                            <uc1:CompanyName ID="CompanyName1" runat="server" />
                           </td>
                    </tr>
                   
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblChalan" runat="server" Text="Daily Sales" Font-Size="18px" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblDate" runat="server" Text="" Font-Size="14px" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grdReport" runat="server" Width="780px"
                                AutoGenerateColumns="False" OnRowDataBound="grdReport_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sno">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Particular">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductId" runat="server" Text='<%# Bind("PRODUCT_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblProductName" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Batch No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBatch" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Expiry Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpiryDate" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Colour">
                                        <ItemTemplate>
                                            <asp:Label ID="lblColourId" runat="server" Text='<%# Bind("COLOUR_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblColourName" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Size">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSizeId" runat="server" Text='<%# Bind("SIZE_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblSizeName" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Manufacturer" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblManufacturerId" runat="server" Text='<%# Bind("MANUFACTURER_ID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblManufacturerName" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("QUANTITY") %>'></asp:Label>
                                        </ItemTemplate>
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

