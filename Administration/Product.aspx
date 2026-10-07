<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Product.aspx.cs" Inherits="Administration_Product" %>

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
        <div>
            <asp:Button ID="btnAddMore" runat="server" Text="Add Product" CssClass="btn btn-success" OnClick="btnAddMore_Click" />
            <asp:Button ID="btnEdit" runat="server" Text="Edit Product" CssClass="btn btn-success" OnClick="btnEdit_Click" />

        </div>
        <div runat="server" id="divAdd" visible="false">
            <div class="row">

                <div class="col-md-6">
                    <div class="popup-big-container-heading">
                        <div class="popup-title">Add Product</div>
                    </div>
                    <div class="row" id="divBranch" runat="server">
                    </div>
                    <div class="row">
                        <div class="auto-style1">
                            Product Group<br />
                            <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="form-control"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlProductCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            Product  Category
                    <br />
                            <asp:DropDownList ID="ddlProductSubCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-3">
                            Product Code<br />
                            <asp:TextBox ID="txtPCode" runat="server" CssClass="form-control"
                                AutoPostBack="true" OnTextChanged="txtPCode_TextChanged"></asp:TextBox>
                        </div>
                        <div class="col-md-9">
                            Product Name<br />
                            <asp:TextBox ID="txtPName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            Product Unit  (Primary)               
                            <br />
                            <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            Product Availability                   
                            <br />
                            <asp:DropDownList ID="ddlAvailability" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Available" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Not Available" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            VAT Applicable
                            <br />
                            <asp:DropDownList ID="ddlTaxable" runat="server" CssClass="form-control" Visible="true">
                                <asp:ListItem Text="Applicable" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Non Applicable" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6" runat="server" id="divPExpiry">
                            Product Expiry                    
                            <br />
                            <asp:DropDownList ID="ddlExpiryStatus" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Applicable" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Not Applicable" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6" runat="server" id="divPBatch">
                            Product Batch                   
                            <br />
                            <asp:DropDownList ID="ddlProductBatch" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Applicable" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Not Applicable" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6" runat="server" id="divPColour">
                            Product Colour                   
                            <br />
                            <asp:DropDownList ID="ddlProductColor" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Applicable" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Not Applicable" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6" runat="server" id="divPType">
                            Product Type                   
                            <br />
                            <asp:DropDownList ID="ddlProType" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6" runat="server" id="divPSize">
                            Product Size                   
                            <br />
                            <asp:DropDownList ID="ddlProductSize" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Applicable" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Not Applicable" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6" runat="server" id="divPManufacturer">
                            Product Manufacturer                   
                            <br />
                            <asp:DropDownList ID="ddlProductManufacturer" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Applicable" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Not Applicable" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>


                    </div>
                    <div class="row" runat="server" id="div1" visible="true">

                        <div class="col-md-2">
                            HS Code<br />
                            <asp:TextBox ID="txtHSCode" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="col-md-2" runat="server" visible="false">
                            Price Category                  
                            <br />
                            <asp:DropDownList ID="ddlPriceCategory" runat="server" CssClass="form-control">
                                <asp:ListItem Text="High" Value="H"></asp:ListItem>
                                <asp:ListItem Text="Medium" Value="M"></asp:ListItem>
                                <asp:ListItem Text="Low" Value="L"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2" id="dualUnit" runat="server" visible="false">
                            Dual Unit Applicable                   
                            <br />
                            <asp:CheckBox ID="chkDualUnit" runat="server" AutoPostBack="true" OnCheckedChanged="chkDualUnit_CheckedChanged" />
                        </div>
                        <div class="col-md-2">
                            Is Service<br />
                            <asp:CheckBox ID="chkIsService" runat="server" />
                        </div>
                    </div>

                    <div class="row" runat="server" id="divDualUnit" visible="false">
                        <div class="col-md-4">
                            Product Unit (Secondary)                         
                            <br />
                            <asp:DropDownList ID="ddlUpperUnit" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUpperUnit_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblLowerQtyLable" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblUpperQtyLable" runat="server" Text=""></asp:Label>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="txtPackQty" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" runat="server" id="div2">
                        <div class="col-md-2">
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="fileAttachment" runat="server" />
                                    </td>
                                    <td></td>
                                </tr>
                            </table>

                        </div>
                    </div>

                    <div class="row mt-10">

                        <br />
                        <div class="col-md-3">
                            <asp:Button ID="btnSave" CssClass="btn btn-success" Width="100%" runat="server" Text="Save" OnClick="btnSave_Click" />
                        </div>
                        <div class="col-md-5">
                            <asp:Button ID="btnSaveContinue" CssClass="btn btn-success" Width="100%" runat="server" Text="Save and Continue" OnClick="btnSaveContinue_Click" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnCancel" CssClass="btn btn-danger" Width="100%" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div style="display: flex; justify-content: flex-end; align-items: flex-start; height: 100vh; padding: 20px; padding-right: 150px">
                        <div id="divProductImage" style="width: 300px; height: 300px; display: flex; align-items: center; justify-content: center; overflow: hidden;" runat="server" visible="false">
                            <asp:Image ID="imgProduct" runat="server" Style="width: 100%; height: 100%; object-fit: contain;" />
                            <br />
                            <%--<asp:Label ID="lblProductImage" runat="server" Text="Product Image" Style="font-weight: bold;"></asp:Label>--%>
                        </div>

                    </div>


                </div>
            </div>
        </div>

        <div id="divAddFilter" runat="server" visible="false">
            <table class="gridtable" style="width: 900px">
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
        <div id="divEditProductCode" runat="server" visible="false">
            <table class="gridtable" style="width: 900px">
                <tr>
                    <td>Product Code
                         <asp:TextBox ID="txtProductCode" runat="server" CssClass="form-control"
                             AutoPostBack="true" OnTextChanged="txtProductCode_TextChanged"></asp:TextBox>
                    </td>
                    <td>Product Name
                        <br />
                        <asp:DropDownList ID="ddlProductList" runat="server" CssClass="form-control" Font-Size="Larger"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlProductList_SelectedIndexChanged" Width="400px">
                        </asp:DropDownList>
                        <asp:Button ID="btnSearch" runat="server" Text="Filter" CssClass="btn btn-success" OnClick="btnSearch_Click" />
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
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEdit" runat="server" CommandName="Change" ImageUrl="~/images/icons/edit.png" />
                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
                        </ItemTemplate>

                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>


    </div>
</asp:Content>

