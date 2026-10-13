<%@ Page Title="Purchase Order Management" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Create_Order.aspx.cs" Inherits="Production_Purchase_Order" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="imsv5-container">
        <div class="imsv5-page-header">
            <div class="imsv5-title-block">
                <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                    <span>Production</span>
                    <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <span class="active">Purchase Order</span>
                </nav>
            </div>
        </div>

        <asp:HiddenField ID="hfPK_ID" runat="server" />

        <%-- STEP 1: PURCHASE ORDER MASTER --%>
        <asp:Panel ID="pnlPurchaseOrder" runat="server">
            <div class="bs-card">
                <div class="card-toolbar">
                    <div class="toolbar-stats">
                        <asp:Label ID="lblFormTitle" runat="server" CssClass="badge-stat" Text="Step 1: Purchase Order Information"></asp:Label>
                    </div>
                </div>

                <div class="form-grid form-grid-3">
                    <div class="form-group">
                        <label for="<%= ddlCustomer.ClientID %>">Customer</label>
                        <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"></asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label for="<%= txtOrderDate.ClientID %>">Order Date</label>
                        <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-control datepicker" placeholder="Select order date..."></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="<%= ddlPaymentTerm.ClientID %>">Payment Term</label>
                        <asp:DropDownList ID="ddlPaymentTerm" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>

                <asp:Panel ID="pnlCustomerDetails" runat="server" CssClass="form-section" Visible="false">
                    <div class="form-section-title">Customer Details</div>
                    <div class="form-section-body">
                        <div class="form-grid form-grid-4">
                            <div class="form-group">
                                <label>Customer Name</label>
                                <asp:Label ID="lblCustomerName" runat="server" CssClass="form-control form-readonly"></asp:Label>
                            </div>

                            <div class="form-group">
                                <label>Customer ID</label>
                                <asp:Label ID="lblCustomerID" runat="server" CssClass="form-control form-readonly"></asp:Label>
                            </div>

                            <div class="form-group">
                                <label>Address</label>
                                <asp:Label ID="lblCustomerAddress" runat="server" CssClass="form-control form-readonly"></asp:Label>
                            </div>

                            <div class="form-group">
                                <label>Phone</label>
                                <asp:Label ID="lblCustomerPhone" runat="server" CssClass="form-control form-readonly"></asp:Label>
                            </div>

                            <div class="form-group">
                                <label>Email</label>
                                <asp:Label ID="lblCustomerEmail" runat="server" CssClass="form-control form-readonly"></asp:Label>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <div class="form-actions">
                    <asp:Button ID="btnNextMaster" runat="server" Text="Next: Delivery & Product Details" CssClass="btn-add-primary" OnClick="btnNextMaster_Click" CausesValidation="false" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn-clear" OnClick="btnClear_Click" CausesValidation="false" />
                </div>
            </div>
        </asp:Panel>

        <%-- STEP 2: DELIVERY & PRODUCT DETAILS --%>
        <asp:Panel ID="pnlDeliveryAndProducts" runat="server" Visible="false">
            <!-- Delivery Info Card -->
            <div class="bs-card">
                <div class="card-toolbar">
                    <div class="toolbar-stats">
                        <asp:Label ID="lblDeliveryTitle" runat="server" CssClass="badge-stat" Text="Step 2: Delivery Information"></asp:Label>
                    </div>
                </div>

                <div class="form-section-body">
                    <div class="form-grid form-grid-4">
                        <div class="form-group">
                            <label>Delivery Date</label>
                            <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="form-control datepicker" placeholder="Select delivery date..."></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label>Customer Location</label>
                            <asp:DropDownList ID="ddlCustomerLocation" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomerLocation_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <label>Mode of Transportation</label>
                            <asp:DropDownList ID="ddlTransportation" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <label>Remarks</label>
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder="Enter remarks..."></asp:TextBox>
                        </div>
                    </div>
                </div>

                <asp:Panel ID="pnlCustomerLocationDetail" runat="server" CssClass="form-section" Visible="false">
                    <div class="form-section-title">Customer Details</div>
                    <div class="form-section-body">
                        <div class="form-grid form-grid-4">
                            <div class="form-group">
                                <label>Customer Name</label>
                                <asp:Label ID="lblCustomerNameDetail" runat="server" CssClass="form-control form-readonly"></asp:Label>
                            </div>

                            <div class="form-group">
                                <label>Customer Address</label>
                                <asp:Label ID="lblCustomerAddressDetail" runat="server" CssClass="form-control form-readonly"></asp:Label>
                            </div>

                            <div class="form-group">
                                <label>Country Name</label>
                                <asp:Label ID="lblCountryNameDetail" runat="server" CssClass="form-control form-readonly"></asp:Label>
                            </div>

                            <div class="form-group">
                                <label>Phone</label>
                                <asp:Label ID="lblPhoneDetail" runat="server" CssClass="form-control form-readonly"></asp:Label>
                            </div>

                            <div class="form-group">
                                <label>Contact Person</label>
                                <asp:Label ID="lblContactPersonDetail" runat="server" CssClass="form-control form-readonly"></asp:Label>
                            </div>

                            <div class="form-group">
                                <label>Contact Number</label>
                                <asp:Label ID="lblContactNumberDetail" runat="server" CssClass="form-control form-readonly"></asp:Label>
                            </div>

                            <div class="form-group">
                                <label>Email Id</label>
                                <asp:Label ID="lblEmailIdDetail" runat="server" CssClass="form-control form-readonly"></asp:Label>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <!-- Product Grid Card -->
            <div class="bs-card">
                <div class="card-toolbar">
                    <div class="toolbar-stats">
                        <asp:Label ID="lblProductsTitle" runat="server" CssClass="badge-stat" Text="Product Details"></asp:Label>
                    </div>
                    <asp:Button ID="btnAddProduct" runat="server" Text="+ Add Product" CssClass="btn-add-primary" OnClick="btnAddProduct_Click" CausesValidation="false" />
                </div>

                <div class="enterprise-grid">
                    <asp:GridView ID="grdProducts" runat="server"
                        AutoGenerateColumns="false"
                        CssClass="table"
                        GridLines="None"
                        OnRowDataBound="grdProducts_RowDataBound"
                        OnRowCommand="grdProducts_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                    <asp:HiddenField ID="hfProductRowId" runat="server" Value='<%# Eval("ROW_ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Product">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Size">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlSize" runat="server" CssClass="form-control"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Swatch Type">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlSwatchType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSwatchType_SelectedIndexChanged"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Swatch">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlSwatch" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Quantity">
                                <ItemTemplate>
                                    <div style="position: relative; display: flex; align-items: center; width: 100%;">
                                        <!-- Textbox with right padding so typed text never overlaps the unit label -->
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Text='<%# Eval("QUANTITY") %>' placeholder="Qty" style="padding-right: 50px;"></asp:TextBox>
            
                                        <!-- Unit label floating on the right side inside the textbox -->
                                        <asp:Label ID="lblUnit" runat="server" style="position: absolute; right: 10px; color: #6c757d; font-size: 12px; font-weight: 600; pointer-events: none;"></asp:Label>
            
                                        <!-- Critical: hfUnitId MUST remain in the row so ReadProducts() retains the value -->
                                        <asp:HiddenField ID="hfUnitId" runat="server" Value='<%# Eval("UNIT") %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action">
                                <ItemStyle CssClass="text-center" Wrap="False" />
                                <ItemTemplate>
                                    <div style="display: inline-flex; align-items: center; justify-content: center; gap: 8px;">
                                        <!-- Duplicate keeping Size (clears Swatch) -->
                                        
                                        <asp:ImageButton ID="btnDupExceptSwatch" runat="server"
                                            ImageUrl="~/images/icons/size.png"
                                            AlternateText="Duplicate Size"
                                            ToolTip="Duplicate row size"
                                            CssClass="btn-icon"
                                            Style="width:20px; height:20px"
                                            CommandName="DuplicateExceptSwatch"
                                            CommandArgument='<%# Eval("ROW_ID") %>'
                                            CausesValidation="false" />

                                        <!-- Duplicate keeping Swatch (clears Size) -->
                                        <asp:ImageButton ID="btnDupExceptSize" runat="server"
                                            ImageUrl="~/images/icons/swatch.png"
                                            AlternateText="Duplicate Swatch"
                                            ToolTip="Duplicate row swatch"
                                            CssClass="btn-icon"
                                            Style="width:20px; height:20px"
                                            CommandName="DuplicateExceptSize"
                                            CommandArgument='<%# Eval("ROW_ID") %>'
                                            CausesValidation="false" />

                                        <!-- Remove Product -->
                                        <asp:ImageButton ID="btnRemoveProduct" runat="server"
                                            ImageUrl="~/images/icons/deletes.png"
                                            AlternateText="Remove"
                                            ToolTip="Remove"
                                            CommandName="RemoveProductRow"
                                            CommandArgument='<%# Eval("ROW_ID") %>'
                                            CausesValidation="false"
                                            OnClientClick="return confirm('Remove product row?');" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="form-actions">
                    <asp:Button ID="btnBackDelivery" runat="server" Text="Back: Order Information" CssClass="btn-clear" OnClick="btnBackDelivery_Click" CausesValidation="false" />
                    <asp:Button ID="btnSaveAndNextDelivery" runat="server" Text="Save & Add Another Delivery" CssClass="btn-secondary" OnClick="btnSaveAndNextDelivery_Click" CausesValidation="false" />
                    <asp:Button ID="btnFinalSave" runat="server" Text="Final Save Purchase Order" CssClass="btn-add-primary" OnClick="btnFinalSave_Click" CausesValidation="false" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>