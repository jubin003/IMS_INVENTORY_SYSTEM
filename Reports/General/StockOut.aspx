<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockOut.aspx.cs" Inherits="Reports_General_StockOut" %>

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
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ></asp:DropDownList>
                </div>
            </div>
         <div class="row" id="divGrid" runat="server">
            <div class="row">   
                <div class="col-md-2 margin-top:auto;">
                    <asp:Button ID="btnPrint" runat="server" Text="Print" Width="100%" CssClass="btn btn-success" OnClick="btnPrint_Click" />
                </div>
            </div>
        </div>
        <div class="row" id="divPrint">
            <div class="col-md-12" id="divhide" runat="server" visible="true">
                <table style="width: 200mm; border-bottom:outset; text-align: center;">
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
                            <asp:Label ID="lblHeading" runat="server" Text="Stock Out" Font-Size="18px" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblDate" runat="server" Text="" Font-Size="14px" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grdStock" runat="server" AutoGenerateColumns="False"
                                CssClass="gridtable" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sno">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("PRODUCTNAME") %>'></asp:Label>
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
                                     <asp:TemplateField HeaderText="Reorder">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReorder" runat="server" Text='<%# Bind("REORDER") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>  
                                    <asp:TemplateField HeaderText="Stock">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStock" runat="server" Text='<%# Bind("STOCK") %>'></asp:Label>
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

