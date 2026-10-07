<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProductReorderLevel.aspx.cs" Inherits="Administration_ProductReorderLevel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="btnAddDiv" runat="server" Text="Add Reorder Level" CssClass="btn btn-success btn-sm" OnClick="btnAddDiv_Click" />
                <asp:Button ID="btnFilter" runat="server" Text="Filter Reorder Level" CssClass="btn btn-success btn-sm" OnClick="btnFilter_Click" />
            </div>
        </div>
        <div class="row" runat="server" id="divFilter" visible="false">
            <div class="col-md-3" id="divBranchFilter" runat="server">
                Branch<br />
                <asp:DropDownList ID="ddlBranchFilter" runat="server" CssClass="form-control"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryFilter_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
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
            <div class="col-md-3 margin-top:auto;">
                <br />
                <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-success" OnClick="btnView_Click" />
                <asp:Label ID="lblOBPK_ID" runat="server" Text="" Visible="false"></asp:Label>
            </div>
        </div>

        <div class="row" runat="server" id="divGrid" visible="false">
            <asp:GridView ID="grdOpeningBalance" runat="server" AutoGenerateColumns="False"
                CssClass="gridtable" Width="100%" OnRowDataBound="grdOpeningBalance_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Sno">
                        <ItemTemplate>
                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                            <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Category">
                        <ItemTemplate>
                            <asp:Label ID="lblCategoryId" runat="server" Text='<%# Bind("CATEGORY_ID") %>' Visible="False"></asp:Label>
                            <asp:Label ID="lblCategoryName" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sub Category">
                        <ItemTemplate>
                            <asp:Label ID="lblSubCategoryId" runat="server" Text='<%# Bind("SUB_CATEGORY_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblSubCategoryName" runat="server" Text=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product">
                        <ItemTemplate>
                            <asp:Label ID="lblProductId" runat="server" Text='<%# Bind("PRODUCT_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblProductName" runat="server" Text=""></asp:Label>
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
                            <asp:Label ID="lblSizeId" runat="server" Text="" Visible="false"></asp:Label>
                            <asp:Label ID="lblSizeName" runat="server" Text=""></asp:Label>
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
        <div class="row" runat="server" id="divAdd" visible="false">
            <div class="col-md-6">
                <div class="row">
                    <div class="col-md-6" id="divBranch" runat="server">
                        Branch<br />
                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
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
                    <div class="col-md-2" runat="server" id="divPackQty" visible="false">
                        Qty in 1
                        <asp:Label ID="lblSUnit" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Label ID="lblPackQty" CssClass="form-control" runat="server" Text=""  Enabled="false"></asp:Label>
                    </div>
                    <div class="col-md-1" runat="server" id="divUnit" visible="false">
                        <br />
                        <asp:Label ID="lblPUnit" runat="server" Text="" Enabled="false"></asp:Label>
                    </div>
                    <div class="col-md-3" runat="server" id="divQuantity" visible="false">
                        Reorder Quantity<br />
                        <asp:TextBox ID="txtQty" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6" id="divPColour" runat="server" >
                        Product Color
                    <br />
                        <asp:DropDownList ID="ddlProductColor" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6" id="divPSize" runat="server" >
                        Product Size
                    <br />
                        <asp:DropDownList ID="ddlProductSize" runat="server" CssClass="form-control">
                            
                        </asp:DropDownList>
                    </div>
                    <%--<div class="col-md-6" id="divPManufacturer" runat="server" >
                        Product Manufacturer
                    <br />
                        <asp:DropDownList ID="ddlProductManufacturer" runat="server" CssClass="form-control">
                            
                        </asp:DropDownList>
                    </div>--%>
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

</asp:Content>

