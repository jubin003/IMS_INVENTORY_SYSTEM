<%@ Page Title="View Purchase Orders"
    Language="C#"
    MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true"
    CodeFile="View_Order.aspx.cs"
    Inherits="Swatch_View_Order" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">

    <div class="imsv5-container">

        <!-- ========================================================= -->
        <!-- PAGE HEADER                                                -->
        <!-- ========================================================= -->

        <div class="imsv5-page-header">

            <div class="imsv5-title-block">

                <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">

                    <span>Production</span>

                    <svg class="imsv5-icon-arrow"
                        fill="none"
                        stroke="currentColor"
                        viewBox="0 0 24 24">

                        <path d="M9 5l7 7-7 7"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                            stroke-width="2">
                        </path>

                    </svg>

                    <span class="active">
                        View Purchase Orders
                    </span>

                </nav>

                <h1 class="imsv5-page-title">
                    Purchase Orders
                </h1>

                <p class="imsv5-page-subtitle">
                    View and manage existing purchase orders.
                </p>

            </div>

        </div>


        <!-- ========================================================= -->
        <!-- PURCHASE ORDER GRID                                       -->
        <!-- ========================================================= -->

        <div class="bs-card">

            <div class="card-toolbar">

                <div class="toolbar-stats">

                    <span class="badge-stat">
                        Manage Purchase Orders
                    </span>

                </div>

            </div>


            <div class="table-responsive">

                <asp:GridView
                    ID="gridDisplay"
                    runat="server"
                    Width="100%"
                    AutoGenerateColumns="False"
                    CssClass="enterprise-grid"
                    AllowPaging="True"
                    PageSize="20"
                    EnableModelValidation="True"
                    GridLines="None"
                    OnRowCommand="gridDisplay_RowCommand"
                    OnRowDataBound="gridDisplay_RowDataBound"
                    OnPageIndexChanging="gridDisplay_PageIndexChanging">

                    <Columns>


                        <asp:TemplateField HeaderText="SN">

                            <HeaderStyle CssClass="col-sn" />
                            <ItemStyle CssClass="text-center col-sn" />

                            <HeaderTemplate>

                                <span
                                    class="sortable-header"
                                    onclick="sortTableColumn(this, '.sort-sn')">

                                    SN

                                    <i
                                        class="fa fa-chevron-up sort-icon"
                                        style="display:none;">
                                    </i>

                                </span>

                            </HeaderTemplate>

                            <ItemTemplate>

                                <span class="row-sn-text sort-sn">
                                    <%# Container.DataItemIndex + 1 %>
                                </span>

                                <asp:Label
                                    ID="lblPK_ID"
                                    runat="server"
                                    Text='<%# Eval("PK_ID") %>'
                                    Visible="false">
                                </asp:Label>

                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Customer Order Number">

                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />

                            <HeaderTemplate>

                                <span
                                    class="sortable-header"
                                    onclick="sortTableColumn(this, '.sort-customer-order-number')">

                                    Customer Order Number

                                    <i
                                        class="fa fa-chevron-up sort-icon"
                                        style="display:none;">
                                    </i>

                                </span>

                            </HeaderTemplate>

                            <ItemTemplate>

                                <asp:Label
                                    ID="lblCustomerOrderNumber"
                                    runat="server"
                                    Text='<%# Eval("CUSTOMER_ORDER_NUMBER") %>'
                                    CssClass="sort-customer-order-number">
                                </asp:Label>

                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Order Date">

                            <HeaderStyle CssClass="col-date" />
                            <ItemStyle CssClass="col-date" />

                            <ItemTemplate>

                                <asp:Label
                                    ID="lblOrderDate"
                                    runat="server"
                                    Text='<%# NepDate(Eval("ORDER_DAY"), Eval("ORDER_MONTH"), Eval("ORDER_YEAR")) %>'>
                                </asp:Label>

                            </ItemTemplate>

                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Order Number">

                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />

                            <HeaderTemplate>

                                <span
                                    class="sortable-header"
                                    onclick="sortTableColumn(this, '.sort-order-number')">

                                    Order Number

                                    <i
                                        class="fa fa-chevron-up sort-icon"
                                        style="display:none;">
                                    </i>

                                </span>

                            </HeaderTemplate>

                            <ItemTemplate>

                                <asp:Label
                                    ID="lblOrderNumber"
                                    runat="server"
                                    Text='<%# Eval("ORDER_NUMBER") %>'
                                    CssClass="sort-order-number">
                                </asp:Label>

                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Customer">

                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name font-semibold" />

                            <ItemTemplate>

                                <asp:Label
                                    ID="lblCustomer"
                                    runat="server"
                                    Text='<%# Eval("CUSTOMER_ID") %>'>
                                </asp:Label>

                            </ItemTemplate>

                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Payment Term">

                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name" />

                            <ItemTemplate>

                                <asp:Label
                                    ID="lblPaymentTerm"
                                    runat="server"
                                    Text='<%# Eval("PAYMENT_TERM") %>'>
                                </asp:Label>

                            </ItemTemplate>

                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Status">

                            <HeaderStyle CssClass="col-status" />
                            <ItemStyle CssClass="col-status" />

                            <ItemTemplate>

                                <asp:Label
                                    ID="lblStatus"
                                    runat="server"
                                    Text='<%# Eval("STATUS") %>'
                                    Visible="false">
                                </asp:Label>

                                <asp:Label
                                    ID="lblStatusShow"
                                    runat="server">
                                </asp:Label>

                            </ItemTemplate>

                        </asp:TemplateField>



                        <asp:TemplateField HeaderText="Action">

                            <HeaderStyle CssClass="col-actions text-center" />
                            <ItemStyle CssClass="col-actions text-center" />

                            <ItemTemplate>

                                <div class="action-btn-group">

                                    <asp:ImageButton
                                        ID="btnEdit"
                                        runat="server"
                                        ImageUrl="~/images/icons/edit.png"
                                        CommandName="EditRow"
                                        CommandArgument='<%# Eval("PK_ID") %>'
                                        ToolTip="Edit Purchase Order"
                                        CssClass="btn-action-icon edit-icon" />

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
