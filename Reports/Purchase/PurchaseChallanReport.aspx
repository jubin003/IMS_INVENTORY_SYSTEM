<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PurchaseChallanReport.aspx.cs" Inherits="Reports_Purchase_PurchaseChallanReport" %>
<%@ Register src="../../uc/CompanyName.ascx" tagname="CompanyName" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                    Challan Date 
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtChalanDate" runat="server" class="form-control  datepicker"
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
        <div class="row" id="divPrint" >
            <div class="col-md-12" id="divhide" runat="server" visible="false">
                <table style="width: 780px; text-align: center;">
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
                            <asp:GridView ID="grdReport" runat="server" Width="100%"
                                AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sno">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dakhila No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDakhilaNumber" runat="server" Text='<%# Bind("DAKHILA_NO") %>' ></asp:Label>                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Challan No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChallanNumber" runat="server" Text='<%# Bind("SUPPLIER_CHALAN_NO") %>' ></asp:Label>                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Challan Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChallanDate" runat="server" Text='<%# Bind("CHALLANDATE") %>' ></asp:Label>                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Entry Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEntryDate" runat="server" Text='<%# Bind("ENTRY_DATE") %>' ></asp:Label>                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Particular">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("PRODUCTNAME") %>' ></asp:Label>                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Batch No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBatch" runat="server" Text='<%# Bind("BATCH_NUMBER") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Expiry Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Bind("EXPIRY_DATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Colour">
                                        <ItemTemplate>
                                            <asp:Label ID="lblColourName" runat="server" Text='<%# Bind("COLOUR_NAME") %>'></asp:Label>
                                          
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Size">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSizeName" runat="server" Text='<%# Bind("SIZE_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Manufacturer" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblManufacturerName" runat="server" Text='<%# Bind("MANUFACTURE_NAME") %>'></asp:Label>                                           
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

