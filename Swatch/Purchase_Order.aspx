<%@ Page Title="Purchase Order Management" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Purchase_Order.aspx.cs" Inherits="Production_Purchase_Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
        .po-form-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
            gap: 16px;
            padding: 20px;
        }
        .po-form-group label {
            display: block;
            font-size: 13px;
            font-weight: 600;
            margin-bottom: 6px;
        }
        .po-form-actions {
            display: flex;
            gap: 10px;
            padding: 0 20px 20px 20px;
        }
        .btn-clear {
            padding: 8px 18px;
            border: 1px solid #cbd5e1;
            background: #fff;
            border-radius: 6px;
            cursor: pointer;
        }
    </style>

    <div class="imsv5-container">

        <div class="imsv5-page-header">
            <div class="imsv5-title-block">
                <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                    <span>Production</span>
                    <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <span class="active">Purchase Order Configuration</span>
                </nav>
                <h1 class="imsv5-page-title">Purchase Order Management</h1>
                <p class="imsv5-page-subtitle">Manage customer orders, dates, transportation, dispatch and payment terms.</p>
            </div>
        </div>

        <div class="bs-card">
            <div class="card-toolbar">
                <div class="toolbar-stats">
                    <asp:Label ID="lblFormTitle" runat="server" CssClass="badge-stat" Text="Add Purchase Order"></asp:Label>
                </div>
            </div>

            <asp:HiddenField ID="hfPK_ID" runat="server" />

            <div class="po-form-grid">
                

                <div class="po-form-group">
                    <label for="<%= txtOrderDate.ClientID %>">Order Date *</label>
                    <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-control datepicker" placeholder="Select order date..."></asp:TextBox>
                </div>

                <div class="po-form-group">
                    <label for="<%= txtOrderNumber.ClientID %>">Order Number *</label>
                    <asp:TextBox ID="txtOrderNumber" runat="server" CssClass="form-control" placeholder="Enter order no..."></asp:TextBox>
                </div>

                <div class="po-form-group">
                    <label for="<%= ddlCustomer.ClientID %>">Customer *</label>
                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>

                <div class="po-form-group">
                    <label for="<%= txtDispatchedDate.ClientID %>">Dispatched Date</label>
                    <asp:TextBox ID="txtDispatchedDate" runat="server" CssClass="form-control datepicker" placeholder="Select dispatch date..."></asp:TextBox>
                </div>

                <div class="po-form-group">
                    <label for="<%= txtModeOfTransportationID.ClientID %>">Mode Of Transportation ID</label>
                    <asp:TextBox ID="txtModeOfTransportationID" runat="server" CssClass="form-control" placeholder="Enter ID..."></asp:TextBox>
                </div>

                <div class="po-form-group">
                    <label for="<%= txtDispatchedLocation.ClientID %>">Dispatched Location</label>
                    <asp:TextBox ID="txtDispatchedLocation" runat="server" CssClass="form-control" placeholder="Enter location..."></asp:TextBox>
                </div>

                <div class="po-form-group">
                    <label for="<%= txtPaymentTerm.ClientID %>">Payment Term</label>
                    <asp:TextBox ID="txtPaymentTerm" runat="server" CssClass="form-control" placeholder="Enter payment term..."></asp:TextBox>
                </div>

                <div class="po-form-group">
                    <label for="<%= ddlStatus.ClientID %>">Status</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="po-form-actions">
                <asp:Button ID="btnSave" runat="server" Text="+ Add" CssClass="btn-add-primary" OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn-clear" OnClick="btnClear_Click" CausesValidation="false" />
            </div>
        </div>

        <!-- ============ GRID ============ -->
        <div class="bs-card">
            <div class="card-toolbar">
                <div class="toolbar-stats">
                    <span class="badge-stat">Manage Purchase Orders</span>
                </div>
            </div>

            <div class="table-responsive">
                <asp:GridView ID="gridDisplay" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="enterprise-grid"
                    AllowPaging="True" PageSize="20" EnableModelValidation="True" GridLines="None"
                    OnRowCommand="gridDisplay_RowCommand" OnRowDataBound="gridDisplay_RowDataBound" OnPageIndexChanging="gridDisplay_PageIndexChanging">
                    <Columns>

                        <asp:TemplateField HeaderText="SN">
                            <HeaderStyle CssClass="col-sn" />
                            <ItemStyle CssClass="text-center col-sn" />
                            <HeaderTemplate>
                                <span class="sortable-header" onclick="sortTableColumn(this, '.sort-sn')">SN <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="row-sn-text sort-sn"><%# Container.DataItemIndex + 1 %></span>
                                <asp:Label ID="lblPK_ID" runat="server" Text='<%# Eval("PK_ID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Customer Order Number">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <HeaderTemplate>
                                <span class="sortable-header" onclick="sortTableColumn(this, '.sort-customer-order-number')">Customer Order Number <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerOrderNumber" runat="server" Text='<%# Eval("CUSTOMER_ORDER_NUMBER") %>' CssClass="sort-customer-order-number"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Order Date">
                            <HeaderStyle CssClass="col-date" />
                            <ItemStyle CssClass="col-date" />
                            <ItemTemplate>
                                <asp:Label ID="lblOrderDate" runat="server" Text='<%# NepDate(Eval("ORDER_DAY"), Eval("ORDER_MONTH"), Eval("ORDER_YEAR")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Order Number">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <HeaderTemplate>
                                <span class="sortable-header" onclick="sortTableColumn(this, '.sort-order-number')">Order Number <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOrderNumber" runat="server" Text='<%# Eval("ORDER_NUMBER") %>' CssClass="sort-order-number"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Customer">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name font-semibold" />
                            <ItemTemplate>
                                <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("CUSTOMER_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Dispatched Date">
                            <HeaderStyle CssClass="col-date" />
                            <ItemStyle CssClass="col-date" />
                            <ItemTemplate>
                                <asp:Label ID="lblDispatchedDate" runat="server" Text='<%# NepDate(Eval("DISPATCHED_DAY"), Eval("DISPATCHED_MONTH"), Eval("DISPATCHED_YEAR")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Mode Of Transportation ID">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <ItemTemplate>
                                <asp:Label ID="lblModeOfTransportationID" runat="server" Text='<%# Eval("MODE_OF_TRANSPORTATION_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Dispatched Location">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name" />
                            <ItemTemplate>
                                <asp:Label ID="lblDispatchedLocation" runat="server" Text='<%# Eval("DISPATCHED_LOCATION") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Payment Term">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name" />
                            <ItemTemplate>
                                <asp:Label ID="lblPaymentTerm" runat="server" Text='<%# Eval("PAYMENT_TERM") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Status">
                            <HeaderStyle CssClass="col-status" />
                            <ItemStyle CssClass="col-status" />
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("STATUS") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblStatusShow" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action">
                            <HeaderStyle CssClass="col-actions text-center" />
                            <ItemStyle CssClass="col-actions text-center" />
                            <ItemTemplate>
                                <div class="action-btn-group">
                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png"
                                        CommandName="EditRow" CommandArgument='<%# Eval("PK_ID") %>'
                                        ToolTip="Edit Purchase Order" CssClass="btn-action-icon edit-icon" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <PagerStyle CssClass="gridview-pager" />
                </asp:GridView>
            </div>
        </div>

    </div>

</asp:Content>