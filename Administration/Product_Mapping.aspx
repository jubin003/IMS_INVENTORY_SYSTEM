<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Product_Mapping.aspx.cs" Inherits="Administration_Product_Maapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .custom-hover-effect:hover {
            cursor: pointer;
            transform: scale(1.5); /* Scale up the div to 150% */
        }

        .auto-style1 {
            position: relative;
            min-height: 1px;
            top: 0px;
            left: 0px;
            float: left;
            width: 50%;
            padding-left: 15px;
            padding-right: 15px;
        }
    </style>

    <script>
        function printBarcode(imgUrl) {
            var html = '<!DOCTYPE html>\n<html>\n<head>\n<title>Print Barcode</title>\n';

            // Add head content (if needed, you can include additional styles or scripts here)
            html += '</head>\n<body>\n';

            // Insert the barcode image
            html += `<img src="${imgUrl}" alt="Barcode" />\n`;

            // Close the body and HTML tags
            html += '</body>\n</html>';

            // Open a new print window
            var printWindow = window.open('', '_blank');  // Open a new blank window
            printWindow.document.open();  // Open the document stream for writing
            printWindow.document.write(html);  // Write the constructed HTML into the new window
            printWindow.document.close();  // Ensure the document is fully written

            // Now add the print trigger script (this is placed after the document is opened)
            printWindow.onload = function () {
                printWindow.print();  // Trigger the print dialog
                printWindow.close();  // Close the window after printing
            };
        }
    </script>





    <div class="container-fluid">      
      
        <div id="divAddFilter" runat="server" visible="true">
            <table class="gridtable" style="width: 900px">
                <tr>
                    <td colspan="2" id="trBranch" runat="server">
                        Branch <br />
                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Product Group<br />
                        <asp:DropDownList ID="ddlProductCategoryF" runat="server" CssClass="form-control"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlProductCategoryF_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>Product  Category
                    <br />
                        <asp:DropDownList ID="ddlProductSubCategoryF" runat="server" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>Product  Type
                        <asp:DropDownList ID="ddlProTypeF" runat="server" CssClass="form-control"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-success" OnClick="btnFilter_Click" />

                        <asp:Label ID="lblPKIDU" runat="server" Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divEditProductCode" runat="server" visible="true">
            <table class="gridtable" style="width: 900px">
                <tr>
                    <td>Product Code
                         <asp:TextBox ID="txtProductCode" runat="server" CssClass="form-control"
                             AutoPostBack="true" OnTextChanged="txtProductCode_TextChanged"></asp:TextBox>
                    </td>
                    <td>Product Name
                        <br />
                        <asp:DropDownList ID="ddlProductList" runat="server" CssClass="form-control" Font-Size="Larger"
                            AutoPostBack="true" Width="400px" OnSelectedIndexChanged="ddlProductList_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Button ID="btnSearch" runat="server" Text="Filter" CssClass="btn btn-success" OnClick="btnSearch_Click"  />
                        <script>
                            $('#<%=ddlProductList.ClientID%>').chosen();
                        </script>
                    </td>
                    <td></td>

                </tr>

            </table>
        </div>
        <div>
        </div>
        <div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
        </div>
        <div id="divGrid" runat="server">
            <asp:GridView ID="grid" runat="server" CssClass="gridtable" Width="100%"
                AutoGenerateColumns="False" OnRowDataBound="grid_RowDataBound"
                OnRowCommand="grid_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="SN">
                        <ItemTemplate>
                            <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                            <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product Image">
                        <ItemTemplate>
                            <asp:Image ID="productImg" class="custom-hover-effect" runat="server" Height="80px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HS Code">
                        <ItemTemplate>
                            <asp:Label ID="lblHSCode" runat="server" Text='<%# Bind("HS_CODE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product Code">
                        <ItemTemplate>
                            <asp:Label ID="lblProductCode" runat="server" Text='<%# Bind("PRODUCT_CODE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Group">
                        <ItemTemplate>
                            <asp:Label ID="lblCategoryId" runat="server" Text='<%# Bind("CATEGORY_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblCategory" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Category">
                        <ItemTemplate>
                            <asp:Label ID="lblSubCategoryId" runat="server" Text='<%# Bind("SUB_CATEGORY_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblSubCategory" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product">
                        <ItemTemplate>
                            <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("PRODUCT_NAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product Type">
                        <ItemTemplate>
                            <asp:Label ID="lblProductType" runat="server" Text='<%# Bind("PRODUCT_TYPE_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblShowProductType" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Secondary Unit">
                        <ItemTemplate>
                            <asp:Label ID="lblUpperUnitID" runat="server" Text='<%# Bind("UPPER_UNIT_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblUpperUnit" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Primary Unit Qty/Secondary Unit">
                        <ItemTemplate>
                            <asp:Label ID="lblQtyPerPAck" runat="server" Text='<%# Bind("PACK_QTY") %>'></asp:Label>
                            <asp:Label ID="lblBasicUnitID" runat="server" Text='<%# Bind("UNIT_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblBasicUnit" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Manufacturer Name">
                        <ItemTemplate>
                            <asp:Label ID="lblManufName" Visible="false" runat="server" Text='<%# Bind("MANUFACTURE_STATUS") %>'></asp:Label>
                            <asp:Label ID="lblShowName" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatusID" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Barcode">
                        <ItemTemplate>
                            <asp:Image ID="barcodeImg" class="custom-hover-effect" Height="80px" Width="100px" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Include in Branch">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkInclude" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="Save" OnClick="btnSave_Click1" Visible="false" />
                </td>
            </tr>
        </table>

    </div>
</asp:Content>

