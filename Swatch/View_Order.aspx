<%@ Page Title="View Purchase Orders" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="View_Order.aspx.cs" Inherits="Swatch_View_Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="css/ims-webform.css" rel="stylesheet" />

    <asp:UpdatePanel ID="upOrderView" runat="server">
        <ContentTemplate>
            <div class="imsv5-container">
                <div class="imsv5-page-header">
                    <div class="imsv5-title-block">
                        <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                            <span>Production</span>
                            <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path></svg>
                            <span class="active">Purchase Orders</span>
                        </nav>
                        <h1 class="imsv5-page-title">Purchase Orders</h1>
                        <p class="imsv5-page-subtitle">View and manage purchase order records, schedules, and delivery details.</p>
                    </div>
                </div>

                <%-- PANEL 1: MASTER PURCHASE ORDER GRID --%>
                <asp:Panel ID="pnlOrderList" runat="server">
                    <div class="bs-card">
                        <div class="card-toolbar">
                            <div class="toolbar-stats">
                                <span class="badge-stat">Manage Purchase Orders</span>
                            </div>
                        </div>

                        <div class="table-responsive">
                            <asp:GridView ID="gridDisplay" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="enterprise-grid" AllowPaging="True" PageSize="20" EnableModelValidation="True" GridLines="None" OnRowCommand="gridDisplay_RowCommand" OnRowDataBound="gridDisplay_RowDataBound" OnPageIndexChanging="gridDisplay_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="SN">
                                        <HeaderStyle CssClass="col-sn" />
                                        <ItemStyle CssClass="text-center col-sn" />
                                        <ItemTemplate>
                                            <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                            <asp:Label ID="lblPK_ID" runat="server" Text='<%# Eval("PK_ID") %>' Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Customer Order Number">
                                        <HeaderStyle CssClass="col-code" />
                                        <ItemStyle CssClass="col-code" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkViewOrder" runat="server" CommandName="ViewOrder" CommandArgument='<%# Eval("PK_ID") %>' Text='<%# Eval("CUSTOMER_ORDER_NUMBER") %>' Font-Bold="true" ForeColor="#0284c7" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Order Date">
                                            <HeaderStyle CssClass="col-date" />
                                            <ItemStyle CssClass="col-date" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderDate" runat="server" Text='<%# Eval("ORDER_DAY") + "/" + Eval("ORDER_MONTH") + "/" + Eval("ORDER_YEAR") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Order Number">
                                        <HeaderStyle CssClass="col-code" />
                                        <ItemStyle CssClass="col-code" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderNumber" runat="server" Text='<%# Eval("ORDER_NUMBER") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Customer">
                                        <HeaderStyle CssClass="col-name" />
                                        <ItemStyle CssClass="col-name font-semibold" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("CUSTOMER_ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Payment Term">
                                        <HeaderStyle CssClass="col-name" />
                                        <ItemStyle CssClass="col-name" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaymentTerm" runat="server" Text='<%# Eval("PAYMENT_TERM") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status">
                                        <HeaderStyle CssClass="col-status" />
                                        <ItemStyle CssClass="col-status" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("STATUS") %>' Visible="false" />
                                            <asp:Label ID="lblStatusShow" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <HeaderStyle CssClass="col-actions text-center" />
                                        <ItemStyle CssClass="col-actions text-center" />
                                        <ItemTemplate>
                                            <div class="action-btn-group">
                                                <asp:ImageButton ID="btnView" runat="server" ImageUrl="~/images/icons/view.png" CommandName="ViewOrder" CommandArgument='<%# Eval("PK_ID") %>' ToolTip="View Details" CssClass="btn-action-icon view-icon" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="gridview-pager" />
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>

                <%-- PANEL 2: ORDER DETAILS BREAKDOWN --%>
                <asp:Panel ID="pnlOrderDetail" runat="server" Visible="false">
                    
                    <%-- CARD 1: PURCHASE ORDER HEADER DETAILS --%>
                    <div class="bs-card">
                        <div class="card-toolbar">
                            <div class="toolbar-stats">
                                <span class="badge-stat">1. Purchase Order Information</span>
                            </div>
                            <div>
                                <asp:Button ID="btnBackToList" runat="server" Text="← Back to Orders" CssClass="btn-clear" OnClick="btnBackToList_Click" CausesValidation="false" />
                            </div>
                        </div>

                        <div class="form-grid form-grid-4">
                            <div class="form-group">
                                <label>Order Number</label>
                                <asp:Label ID="lblDetOrderNo" runat="server" CssClass="form-control form-readonly" />
                            </div>
                            <div class="form-group">
                                <label>Customer Order Number</label>
                                <asp:Label ID="lblDetCustOrderNo" runat="server" CssClass="form-control form-readonly" />
                            </div>
                            <div class="form-group">
                                <label>Order Date</label>
                                <asp:Label ID="lblDetOrderDate" runat="server" CssClass="form-control form-readonly" />
                            </div>
                            <div class="form-group">
                                <label>Customer</label>
                                <asp:Label ID="lblDetCustomer" runat="server" CssClass="form-control form-readonly" />
                            </div>
                            <div class="form-group">
                                <label>Payment Term</label>
                                <asp:Label ID="lblDetPaymentTerm" runat="server" CssClass="form-control form-readonly" />
                            </div>
                            <div class="form-group">
                                <label>Fiscal Year</label>
                                <asp:Label ID="lblDetFiscalYear" runat="server" CssClass="form-control form-readonly" />
                            </div>
                            <div class="form-group">
                                <label>Status</label>
                                <asp:Label ID="lblDetStatus" runat="server" CssClass="form-control form-readonly" />
                            </div>
                        </div>
                    </div>

                    <%-- CARD 2: DELIVERY SCHEDULE SUMMARY OVERVIEW --%>
                    <div class="bs-card">
                        <div class="card-toolbar">
                            <div class="toolbar-stats">
                                <span class="badge-stat">2. Delivery Dates Overview</span>
                            </div>
                        </div>

                        <div class="table-responsive">
                            <asp:GridView ID="gridDeliverySummary" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None" CssClass="enterprise-grid">
                                <Columns>
                                    <asp:TemplateField HeaderText="SN">
                                        <HeaderStyle CssClass="col-sn" />
                                        <ItemStyle CssClass="text-center col-sn" />
                                        <ItemTemplate>
                                            <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="DELIVERY_DATE" HeaderText="Delivery Date" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date font-semibold" />
                                    <asp:BoundField DataField="MODE_OF_TRANSPORTATION" HeaderText="Transportation" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                    <asp:BoundField DataField="LOCATION_ADDRESS" HeaderText="Destination Address" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                    <asp:BoundField DataField="REMARKS" HeaderText="Remarks" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                    <asp:BoundField DataField="STATUS_LABEL" HeaderText="Status" HeaderStyle-CssClass="col-status" ItemStyle-CssClass="col-status" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="card-toolbar"><span class="badge-stat">No scheduled delivery dates found.</span></div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>

                    <%-- CARD 3 REPEATER: EACH MULTIPLE DELIVERY DATE COMBINED WITH LOCATION & PRODUCTS --%>
                    <asp:Repeater ID="rptDeliveries" runat="server" OnItemDataBound="rptDeliveries_ItemDataBound">
                        <ItemTemplate>
                            <div class="bs-card">
                                <div class="card-toolbar">
                                    <div class="toolbar-stats">
                                        <span class="badge-stat">3.<%# Container.ItemIndex + 1 %> Delivery Schedule: <%# Eval("DELIVERY_DATE") %></span>
                                    </div>
                                    <span class="row-sn-text">Remarks: <%# string.IsNullOrEmpty(Eval("REMARKS") as string) ? "None" : Eval("REMARKS") %></span>
                                </div>

                                <%-- Customer Location Details for this Specific Delivery --%>
                                <div class="form-section">
                                    <div class="form-section-title">Delivery Location & Recipient</div>
                                    <div class="form-section-body">
                                        <div class="form-grid form-grid-4">
                                            <div class="form-group form-span-2">
                                                <label>Delivery Address</label>
                                                <asp:Label ID="lblDelAddress" runat="server" CssClass="form-control form-readonly" />
                                            </div>
                                            <div class="form-group">
                                                <label>Contact Person</label>
                                                <asp:Label ID="lblDelContactPerson" runat="server" CssClass="form-control form-readonly" />
                                            </div>
                                            <div class="form-group">
                                                <label>Contact Number</label>
                                                <asp:Label ID="lblDelContactNumber" runat="server" CssClass="form-control form-readonly" />
                                            </div>
                                            <div class="form-group">
                                                <label>Recipient Email</label>
                                                <asp:Label ID="lblDelEmail" runat="server" CssClass="form-control form-readonly" />
                                            </div>
                                            <div class="form-group">
                                                <label>Country ID</label>
                                                <asp:Label ID="lblDelCountry" runat="server" CssClass="form-control form-readonly" />
                                            </div>
                                            <div class="form-group">
                                                <label>Mode of Transportation</label>
                                                <asp:Label ID="lblDelTransport" runat="server" CssClass="form-control form-readonly" />
                                            </div>
                                            <div class="form-group">
                                                <label>Schedule Status</label>
                                                <asp:Label ID="lblDelStatus" runat="server" CssClass="form-control form-readonly" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%-- Assigned Products Grid for this Delivery Date --%>
                                <div class="form-section">
                                    <div class="form-section-title">Assigned Products & Swatches</div>
                                    <div class="form-section-body">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gridOrderProducts" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="None" CssClass="enterprise-grid">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SN">
                                                        <HeaderStyle CssClass="col-sn" />
                                                        <ItemStyle CssClass="text-center col-sn" />
                                                        <ItemTemplate>
                                                            <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Product">
                                                        <HeaderStyle CssClass="col-name" />
                                                        <ItemStyle CssClass="col-name font-semibold" />
                                                        <ItemTemplate>
                                                            <div class="cell-entity">
                                                                <span class="entity-bullet"></span>
                                                                <span><%# Eval("PRODUCT_NAME") %></span>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="SIZE_NAME" HeaderText="Size" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                    <asp:BoundField DataField="SWATCH_NAME" HeaderText="Swatch" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                                                    <asp:BoundField DataField="UNIT" HeaderText="Unit" HeaderStyle-CssClass="col-status" ItemStyle-CssClass="col-status" />

                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <HeaderStyle CssClass="col-rate text-right" />
                                                        <ItemStyle CssClass="col-rate text-right" />
                                                        <ItemTemplate>
                                                            <span class="cell-rate"><%# Eval("QUANTITY") %></span>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <div class="card-toolbar"><span class="badge-stat">No products assigned for this delivery.</span></div>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>