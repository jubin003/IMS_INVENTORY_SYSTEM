<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OpeningBalance.aspx.cs" Inherits="Utilities_OpeningBalance" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row" id="divBranchFilter" runat="server">
        <div class="col-md-4">
            Branch<br />
            <asp:DropDownList ID="ddlBranchFilter" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlBranchFilter_SelectedIndexChanged">
            </asp:DropDownList>  
            <asp:DropDownList ID="ddlFiscalYear" runat="server" Visible="false"></asp:DropDownList>           
        </div>
    </div>
    <div class="container-fluid" runat="server" id="divWhole">
        <div class="row">
            <div class="popup-big-container-heading">
                <div class="popup-title">
                    Opening Balance for Fiscal Year
                            <asp:Label ID="lblFY" runat="server" Text=""></asp:Label>
                   
                </div>
            </div>
        </div>

        <div class="row" runat="server" id="divFilter">

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
                        AutoPostBack="true">
                    </asp:DropDownList>

                </div>

                <div class="col-md-3 margin-top:auto;">
                    <br />

                    <asp:Button ID="btn_ADD" CssClass="btn btn-success" runat="server" Text="View" OnClick="Add_Click" />
                </div>
            </div>
        </div>

        <div class="row" runat="server" id="divGrid" visible="false">
        </div>
        <div class="row" runat="server" id="divAdd" visible="false">
            <asp:GridView ID="gridAddOpen" runat="server" AutoGenerateColumns="False"
                CssClass="gridtable" Width="100%" OnRowDataBound="gridAddOpen_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Sno">
                        <ItemTemplate>
                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                            <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product Code">
                        <ItemTemplate>
                            <asp:Label ID="lblProductCode" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product">
                        <ItemTemplate>
                            <asp:Label ID="lblProductId" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblProductName" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:TextBox CssClass="form-control" ID="txtQuantity" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:TextBox CssClass="form-control" ID="txtRate" AutoPostBack="true" runat="server" OnTextChanged="txtRate_TextChanged"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Closing Value">
                        <ItemTemplate>
                            <asp:TextBox CssClass="form-control" ID="txtCloVal" ReadOnly="true" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            <div>
                <br />
                <asp:Button ID="btn_add_new" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btn_add_new_Click" />
                &nbsp;<asp:Button ID="btn_back" CssClass="btn btn-primary" runat="server" Text="Back" OnClick="btn_back_Click" />
            </div>
        </div>

    </div>
    <div class="container-fluid" runat="server" id="divSolo">
        <div class="row">
            <div class="popup-big-container-heading">
                <div class="popup-title">
                    Opening Balance for Fiscal Year
                            <asp:Label ID="lblFYSolo" runat="server" Text=""></asp:Label>
                    <asp:Button ID="btnAddDivSolo" runat="server" Text="Add Opening Balance" CssClass="btn btn-success btn-sm" OnClick="btnAddDivSolo_Click" />
                    <asp:Button ID="btnFilterSolo" runat="server" Text="Filter Opening Balance" CssClass="btn btn-success btn-sm" OnClick="btnFilterSolo_Click" />
                    <asp:Button ID="btnExcelUp" runat="server" Text="Upload Excel" CssClass="btn btn-success btn-sm" OnClick="btnExcelUp_Click" />
                </div>

            </div>
        </div>

        <div class="row" runat="server" id="divFilterSolo">           
            <div class="row">
                <div class="col-md-3">
                    Product Category<br />
                    <asp:DropDownList ID="ddlCategoryFilterSolo" runat="server" CssClass="form-control"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryFilterSolo_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    Product Sub Category
                    <br />
                    <asp:DropDownList ID="ddlSubCategoryFilterSolo" runat="server" CssClass="form-control"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategoryFilterSolo_SelectedIndexChanged">
                    </asp:DropDownList>

                </div>

                <div class="col-md-3 margin-top:auto;">
                    <br />
                    <asp:Button ID="btnViewSolo" runat="server" Text="View" CssClass="btn btn-success" OnClick="btnView_Click" />
                    <asp:Label ID="lblOBPK_ID" runat="server" Text="" Visible="false"></asp:Label>
                </div>
            </div>
        </div>

        <div class="row" runat="server" id="divGridSolo" visible="false">
            <asp:GridView ID="grdOpeningBalance" runat="server" AutoGenerateColumns="False"
                CssClass="gridtable" Width="100%" OnRowDataBound="grdOpeningBalance_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Sno">
                        <ItemTemplate>
                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                            <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product Code">
                        <ItemTemplate>
                            <asp:Label ID="lblProductCode" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product">
                        <ItemTemplate>
                            <asp:Label ID="lblProductId" runat="server" Text='<%# Bind("PRODUCT_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblProductName" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Size">
                        <ItemTemplate>
                            <asp:Label ID="lblSizeId" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblSizeName" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Manufacturer" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblManufacturerId" runat="server" Text='<%# Bind("MANUFACTURER_ID") %>' Visible="false" ></asp:Label>
                            <asp:Label ID="lblManufacturerName" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Batch No." Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblBatch" runat="server" Text='<%# Bind("BATCH_NUMBER") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expiry Date">
                        <ItemTemplate>
                            <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Bind("EXPIRY_DATE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:Label ID="lblRate" runat="server" Text='<%# Bind("RATE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("QUANTITY") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-sm btn-success" OnClick="btnEdit_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn  btn-sm btn-success"
                                OnClick="btnDelete_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="row" runat="server" id="divAddSolo" visible="false">
            <div class="col-md-6">               
                <div class="row">
                    <div class="col-md-6">
                        Product Category<br />
                        <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="form-control"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlProductCategory_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        Product Sub Category
                    <br />
                        <asp:DropDownList ID="ddlProductSubCategory" runat="server" CssClass="form-control"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlProductSubCategory_SelectedIndexChanged">
                        </asp:DropDownList>

                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        Product Name<br />
                        <asp:DropDownList ID="ddlproduct" runat="server" CssClass="form-control"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlproduct_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3" runat="server" id="divPackQty" visible="false">
                        Pack Qty
                    <br />
                        <asp:Label ID="lblPackQty" runat="server" Text="" CssClass="form-control" Enabled="false"></asp:Label>
                    </div>
                    
                    <div class="col-md-3" runat="server" id="divUnit" visible="false">
                        Unit
                    <br />
                        <asp:Label ID="lblUnit" runat="server" Text="" CssClass="form-control" Enabled="false"></asp:Label>
                    </div>
                </div>
                <div class="row">

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
                            <%--<asp:ListItem Text="Applicable" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Not Applicable" Value="0"></asp:ListItem>--%>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6" runat="server" id="divPExpiry">
                        Expiry Date<br />
                        <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-6" runat="server" id="divPBatch">
                        Batch<br />
                        <asp:TextBox ID="txtBatch" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-6" runat="server" id="divQuantity" visible="true">
                        Quantity<br />
                        <asp:TextBox ID="txtQty" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-6" runat="server" id="divRate" visible="false">
                        Rate
                    <br />
                        <asp:TextBox ID="txtRate" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6" style="margin-top: 20px;">
                        <asp:Button ID="btnAdd" runat="server" Text="Add and List" CssClass="btn btn-success" Width="100%" Visible="false" OnClick="btnAdd_Click" />

                    </div>

                    <div class="col-md-6" style="margin-top: 20px;">
                        <asp:Button ID="btnAddandContinue" runat="server" Text="Add and Continue" CssClass="btn btn-success" Width="100%" Visible="false" OnClick="btnAddandContinue_Click" />
                    </div>
                </div>
            </div>
        </div>

    </div>

    <div id="divexcleUp" visible="false" runat="server">
        <b class="font-50">Download Sample for excel Upload</b><br />
        <asp:Button ID="btnDownload" CssClass="btn btn-primary" runat="server" Text="Download File" OnClick="DownloadFile" />

        <br />
        <br />

        <asp:FileUpload ID="FileExcelUp" CssClass="btn btn-success" runat="server" />
        <label style="color: red">*Make sure product code in system and sheet matches</label>
        <br />
        <asp:Button ID="btnExcelUpload" runat="server" CssClass="btn btn-primary" Text="Upload" OnClick="btnExcelUpload_Click" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnSave" runat="server" Visible="false" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnbackE" runat="server" CssClass="btn btn-primary" Text="Back" OnClientClick="window.history.back(); return false;" OnClick="btnbackE_Click" />
        <br />
        <asp:GridView ID="GridView1" CssClass="gridtable" runat="server"></asp:GridView>
    </div>

    <div id="divAlert" runat="server" visible="false" style="font-size: large">
        <strong style="color:red">You cannot enter Opening balance for this fiscal year. Perform closing balance.
    </strong>
    </div>

</asp:Content>

