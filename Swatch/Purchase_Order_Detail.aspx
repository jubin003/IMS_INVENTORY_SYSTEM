<%@ Page Title="Purchase Order Detail Management" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Purchase_Order_Detail.aspx.cs" Inherits="Production_Purchase_Order_Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="imsv5-container">

        <div class="imsv5-page-header">
            <div class="imsv5-title-block">
                <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                    <span>Purchase Order</span>
                    <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <span class="active">Purchase Order Detail Configuration</span>
                </nav>
                <h1 class="imsv5-page-title">Purchase Order Detail Management</h1>
                <p class="imsv5-page-subtitle">Configure purchase order products, sizes, swatches, quantities and units.</p>
            </div>
        </div>

        <div class="bs-card">
            <div class="card-toolbar">
               <%-- <div class="search-box-wrapper">
                    <svg class="search-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <input type="text" id="clientSearchInput" class="form-control table-search-input" placeholder="Search purchase order detail..." onkeyup="filterPurchaseOrderDetailTable()" />
                </div>--%>
                <div class="toolbar-stats">
                    <span class="badge-stat">Manage Purchase Order Details</span>
                </div>
            </div>

            <div class="table-responsive">
                <asp:GridView ID="gridDisplay" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="enterprise-grid" OnRowEditing="gridDisplay_RowEditing" OnRowUpdating="gridDisplay_RowUpdating" OnRowCancelingEdit="gridDisplay_RowCancelingEdit" AllowPaging="True" PageSize="20" EnableModelValidation="True" OnRowDataBound="gridDisplay_RowDataBound" GridLines="None">
                    <Columns>

                        <asp:TemplateField HeaderText="SN">
                            <HeaderStyle CssClass="col-sn" />
                            <ItemStyle CssClass="text-center col-sn" />
                            <HeaderTemplate>
                                <span class="sortable-header" onclick="sortTableColumn(this, '.sort-sn')">SN <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="row-sn-text sort-sn"><%# Container.DataItemIndex + 1 %></span>
                                <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Purchase Order ID">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-purchase-order-id')">Purchase Order ID <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                    <asp:TextBox ID="txtPurchaseOrderID" runat="server" CssClass="form-control inline-input" placeholder="Enter purchase order ID..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPurchaseOrderID" runat="server" Text='<%# Bind("PURCHASE_ORDER_ID") %>' CssClass="sort-purchase-order-id"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPurchaseOrderIDE" runat="server" Text='<%# Bind("PURCHASE_ORDER_ID") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="SNO">
                            <HeaderStyle CssClass="col-sn" />
                            <ItemStyle CssClass="text-center col-sn" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-sno')">SNO <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                    <asp:TextBox ID="txtSNO" runat="server" CssClass="form-control inline-input" placeholder="Enter SNO..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSNO" runat="server" Text='<%# Bind("SNO") %>' CssClass="sort-sno"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSNOE" runat="server" Text='<%# Bind("SNO") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Product">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-product-id')">Product <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                    <asp:DropDownList runat="server" ID="ddlProduct" CssClass="form-control">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblProductID" runat="server" Text='<%# Bind("PRODUCT_ID") %>' CssClass="sort-product-id"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtProductIDE" runat="server" Text='<%# Bind("PRODUCT_ID") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Size ID">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-size-id')">Size ID <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                    <asp:TextBox ID="txtSizeID" runat="server" CssClass="form-control inline-input" placeholder="Enter size ID..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSizeID" runat="server" Text='<%# Bind("SIZE_ID") %>' CssClass="sort-size-id"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSizeIDE" runat="server" Text='<%# Bind("SIZE_ID") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Swatch ID">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-swatch-id')">Swatch ID <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                    <asp:TextBox ID="txtSwatchID" runat="server" CssClass="form-control inline-input" placeholder="Enter swatch ID..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSwatchID" runat="server" Text='<%# Bind("SWATCH_ID") %>' CssClass="sort-swatch-id"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSwatchIDE" runat="server" Text='<%# Bind("SWATCH_ID") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Quantity">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-quantity')">Quantity <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control inline-input" placeholder="Enter quantity..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("QUANTITY") %>' CssClass="sort-quantity"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtQuantityE" runat="server" Text='<%# Bind("QUANTITY") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Unit">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-unit')">Unit <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                     <asp:DropDownList runat="server" ID="ddlUnit" CssClass="form-control">
                                         <asp:ListItem></asp:ListItem>
                                     </asp:DropDownList>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("UNIT") %>' CssClass="sort-unit"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtUnitE" runat="server" Text='<%# Bind("UNIT") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle CssClass="col-actions text-center" />
                            <ItemStyle CssClass="col-actions text-center" />
                            <HeaderTemplate>
                                <div class="header-action-group">
                                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="+ Add" CssClass="btn-add-primary" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="action-btn-group">
                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="edit" ToolTip="Edit Purchase Order Detail" CssClass="btn-action-icon edit-icon" />
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="action-btn-group">
                                    <asp:ImageButton ID="ImageButton1" runat="server" CommandName="update" ImageUrl="~/images/icons/upload.png" ToolTip="Save Changes" CssClass="btn-action-icon update-icon" />
                                    <asp:ImageButton ID="btnCancel" runat="server" CommandName="cancel" ImageUrl="~/images/icons/cancel.png" ToolTip="Cancel" CssClass="btn-action-icon cancel-icon" />
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <PagerStyle CssClass="gridview-pager" />
                </asp:GridView>
            </div>
        </div>

    </div>

    <script type="text/javascript">
        function filterPurchaseOrderDetailTable() {
            var input = document.getElementById("clientSearchInput");
            var filter = input.value.toLowerCase();
            var grid = document.getElementById("<%= gridDisplay.ClientID %>");
            if (!grid) return;
            var rows = grid.getElementsByTagName("tr");
            for (var i = 1; i < rows.length; i++) {
                if (rows[i].classList.contains("gridview-pager")) continue;
                var rowText = rows[i].textContent || rows[i].innerText;
                rows[i].style.display = rowText.toLowerCase().indexOf(filter) > -1 ? "" : "none";
            }
        }
    </script>

</asp:Content>