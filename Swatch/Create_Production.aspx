<%@ Page Title="Production Management" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Create_Production.aspx.cs" Inherits="Production_Create_Production" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

                <style>
    .zoomable-swatch {
        border-radius: 4px;
        transition: transform 0.2s ease-in-out;
        cursor: pointer;
        object-fit: cover;
        border: 1px solid #e2e8f0;
        background-color: #fff; 
    }
    .zoomable-swatch:hover {
        transform: scale(2.5);
        z-index: 9999;
        position: relative;
        box-shadow: 0 4px 12px rgba(0,0,0,0.3);
    }
    
    
    #<%= pnlProductionProducts.ClientID %> .table-responsive,
    .bs-card {
        overflow: visible !important;
    }
    
    .enterprise-grid td {
        overflow: visible !important;
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
                    <span class="active">Production Management</span>
                </nav>
                <h1 class="imsv5-page-title">Create Production</h1>
                <p class="imsv5-page-subtitle">Configure production details, stages, and production scheduling.</p>
            </div>
        </div>

        <asp:HiddenField ID="hfProductionId" runat="server" />
        <asp:HiddenField ID="hfCustomerPoId" runat="server" />
        <asp:HiddenField ID="hfMultipleDeliveryId" runat="server" />
        <asp:HiddenField ID="hfAutomatedCompletionDate" runat="server" />

        <%-- ==================== STEP 1: PRODUCTION MASTER ==================== --%>
        <asp:Panel ID="pnlProductionMaster" runat="server">
            <div class="bs-card">
                <div class="card-toolbar">
                    <div class="toolbar-stats">
                        <asp:Label ID="lblMasterTitle" runat="server" CssClass="badge-stat" Text="Step 1: Production Master"></asp:Label>
                    </div>
                </div>

                <div class="form-grid form-grid-2">
                    <div class="form-group">
                        <label>Production Type <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlProductionType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProductionType_SelectedIndexChanged">
                            <asp:ListItem Text="-- Select Production Type --" Value=""></asp:ListItem>
                            <asp:ListItem Text="In-House" Value="INHOUSE"></asp:ListItem>
                            <asp:ListItem Text="Purchase Order" Value="PO"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label for="<%= txtProductionDate.ClientID %>">Production Date *</label>
                        <asp:TextBox ID="txtProductionDate" runat="server" CssClass="form-control datepicker" placeholder="Select production date..."></asp:TextBox>
                    </div>
                </div>

                <%-- Delivery Schedule Grid (Shown only when Production Type is 'PO') --%>
                <asp:Panel ID="pnlPOGrid" runat="server" Visible="false" CssClass="form-section" style="margin-top: 20px;">
                    <div class="card-toolbar">
                        <div class="toolbar-stats">
                            <span class="badge-stat">Scheduled Deliveries (Ranked by Urgency)</span>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="grdPurchaseOrders" runat="server" AutoGenerateColumns="False" 
                            CssClass="enterprise-grid" GridLines="None" 
                            AllowPaging="True" PageSize="20" OnPageIndexChanging="grdPurchaseOrders_PageIndexChanging"
                            AllowSorting="True" OnSorting="grdPurchaseOrders_Sorting"
                            OnRowCommand="grdPurchaseOrders_RowCommand">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="&laquo;" LastPageText="&raquo;" />
                            <PagerStyle CssClass="grid-pager" HorizontalAlign="Right" />
                            <Columns>
                                <asp:TemplateField HeaderText="SN">
                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ORDER_NUMBER" HeaderText="PO Number" SortExpression="ORDER_NUMBER" />
                                <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer" SortExpression="CUSTOMER_NAME" />
                                <asp:BoundField DataField="DELIVERY_DATE" HeaderText="Delivery Date" SortExpression="PARSED_DATE" />
                                <asp:BoundField DataField="CUSTOMER_LOCATION" HeaderText="Location" SortExpression="CUSTOMER_LOCATION" />
                                <asp:TemplateField HeaderText="Urgency" SortExpression="PARSED_DATE">
                                    <ItemTemplate>
                                        <%# GetUrgencyBadge(Eval("DELIVERY_DATE")) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemStyle CssClass="text-center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnViewDetails" runat="server" 
                                            CommandName="ViewDeliveryDetails" 
                                            CommandArgument='<%# Eval("MULTIPLE_DELIVERY_ID") %>' 
                                            CssClass="btn-action-icon" 
                                            ToolTip="View Details" 
                                            CausesValidation="false">
                                            <svg width="18" height="18" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                                            </svg>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnCreateProductionOrder" runat="server" 
                                            CommandName="CreateFromDelivery" 
                                            CommandArgument='<%# Eval("MULTIPLE_DELIVERY_ID") %>' 
                                            CssClass="btn-action-icon text-primary" 
                                            ToolTip="Create Production" 
                                            CausesValidation="false" 
                                            style="margin-left: 8px;">
                                            <svg width="18" height="18" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v3m0 0v3m0-3h3m-3 0H9m12 0a9 9 0 11-18 0 9 9 0 0118 0z" />
                                            </svg>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>

                <div class="form-actions">
                    <asp:Button ID="btnNextMaster" runat="server" Text="Next: Production Details" CssClass="btn-add-primary" OnClick="btnNextMaster_Click" />
                    <asp:Button ID="btnClearMaster" runat="server" Text="Clear" CssClass="btn-clear" OnClick="btnClearMaster_Click" CausesValidation="false" />
                </div>
            </div>
        </asp:Panel>

        <%-- ==================== STEP 2: PRODUCTION DETAILS ==================== --%>
        <asp:Panel ID="pnlProductionDetails" runat="server" Visible="false">
            <div class="bs-card">
                <div class="card-toolbar">
                    <div class="toolbar-stats">
                        <asp:Label ID="lblDetailsTitle" runat="server" CssClass="badge-stat" Text="Step 2: Production Details"></asp:Label>
                    </div>
                </div>

                <div class="form-grid form-grid-2">
                    <div class="form-group">
                        <label>Production Date</label>
                        <asp:Label ID="lblDetailsProductionDate" runat="server" CssClass="form-control form-readonly"></asp:Label>
                    </div>
                    <div class="form-group">
                        <label>Scheduled Delivery Target</label>
                        <asp:Label ID="lblDetailsDeliveryTarget" runat="server" CssClass="form-control form-readonly"></asp:Label>
                    </div>
                </div>

                <asp:Panel ID="pnlProductionProducts" runat="server">
                    <div class="card-toolbar">
                        <div class="toolbar-stats">
                            <asp:Label ID="lblProductTitle" runat="server" CssClass="badge-stat" Text="Production Products"></asp:Label>
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="grdProductionDetails" runat="server" AutoGenerateColumns="False" CssClass="enterprise-grid" GridLines="None" OnRowCommand="grdProductionDetails_RowCommand" >
                    <Columns>
                        <asp:TemplateField HeaderText="SN">
                            <HeaderStyle Width="50px" CssClass="text-center" />
                            <ItemStyle Width="50px" CssClass="text-center" />
                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Product *">
                            <ItemTemplate>
                                <asp:HiddenField ID="hfProductId" runat="server" Value='<%# Eval("PRODUCT_ID") %>' />
                                <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("PRODUCT_NAME") %>' CssClass="form-control-plaintext font-semibold"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Size">
                            <ItemTemplate>
                                <asp:HiddenField ID="hfSizeId" runat="server" Value='<%# Eval("SIZE_ID") %>' />
                                <asp:Label ID="lblSizeName" runat="server" Text='<%# Eval("SIZE_NAME") %>' CssClass="form-control-plaintext"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Swatch">
                            <ItemTemplate>
                                <asp:HiddenField ID="hfSwatchId" runat="server" Value='<%# Eval("SWATCH_ID") %>' />
                                <div style="display: flex; align-items: center; gap: 10px;">
                                    <asp:Image ID="imgSwatch" runat="server" 
                                        ImageUrl='<%# GetSwatchImageUrl(Eval("SWATCH_ID")) %>' 
                                        Width="44px" Height="44px" 
                                        CssClass="zoomable-swatch" />
                                    <asp:Label ID="lblSwatchName" runat="server" Text='<%# Eval("SWATCH_NAME") %>' CssClass="form-control-plaintext"></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Quantity *">
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Text='<%# Eval("QUANTITY") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Unit">
                            <ItemTemplate>
                                <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("UNIT") %>' CssClass="form-control-plaintext"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action">
                            <HeaderStyle CssClass="text-center" Width="100px" />
                            <ItemStyle CssClass="text-center" Width="100px" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:LinkButton ID="btnRemoveProduct" runat="server" 
                                    Text="Remove" 
                                    CommandName="RemoveProduct" 
                                    CommandArgument='<%# Eval("ROW_ID") %>' 
                                    CssClass="btn-clear" 
                                    Style="float: none; margin: 0 auto; display: inline-block;"
                                    CausesValidation="false">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                    </div>
                </asp:Panel>

                <div class="form-actions">
                    <asp:Button ID="btnBackMaster" runat="server" Text="Back" CssClass="btn-clear" OnClick="btnBackMaster_Click" CausesValidation="false" />
                    <asp:Button ID="btnNextDetails" runat="server" Text="Next: Production Stage" CssClass="btn-add-primary" OnClick="btnNextDetails_Click" />
                </div>
            </div>
        </asp:Panel>

        <%-- ==================== STEP 3: PRODUCTION STAGE ==================== --%>
        <asp:Panel ID="pnlProductionStages" runat="server" Visible="false">
            <div class="bs-card">
                <div class="card-toolbar">
                    <div class="toolbar-stats">
                        <asp:Label ID="lblStageTitle" runat="server" CssClass="badge-stat" Text="Step 3: Production Stage"></asp:Label>
                    </div>
                    <asp:Button ID="btnAddStage" runat="server" Text="+ Add Stage" CssClass="btn-add-primary" OnClick="btnAddStage_Click" CausesValidation="false" />
                </div>

                <div class="form-grid form-grid-2" style="margin-bottom: 15px;">
                    <div class="form-group">
                        <label>Order Reference</label>
                        <asp:Label ID="lblStageOrderNumber" runat="server" CssClass="form-control form-readonly"></asp:Label>
                    </div>
                    <div class="form-group">
                        <label>Production Date</label>
                        <asp:Label ID="lblStageProductionDate" runat="server" CssClass="form-control form-readonly"></asp:Label>
                    </div>
                </div>

                <div class="table-responsive">
                    <asp:GridView ID="grdProductionStages" runat="server" AutoGenerateColumns="False" 
                        CssClass="enterprise-grid" GridLines="None" 
                        OnRowCommand="grdProductionStages_RowCommand" 
                        OnRowDataBound="grdProductionStages_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="SN">
                                <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product *">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfStageRowId" runat="server" Value='<%# Eval("ROW_ID") %>' />
                                    <asp:DropDownList ID="ddlStageProduct" runat="server" CssClass="form-control"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stage *">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlStageMaster" runat="server" CssClass="form-control"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit or Employee *">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlStageAssignmentType" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Select..." Value=""></asp:ListItem>
                                        <asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>
                                        <asp:ListItem Text="Employee" Value="Employee"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlStageStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                        <asp:ListItem Text="In Progress" Value="In Progress"></asp:ListItem>
                                        <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnRemoveStage" runat="server" Text="Remove" CommandName="RemoveStage" CommandArgument='<%# Eval("ROW_ID") %>' CssClass="btn-clear" CausesValidation="false"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="form-actions">
                    <asp:Button ID="btnBackDetails" runat="server" Text="Back" CssClass="btn-clear" OnClick="btnBackDetails_Click" CausesValidation="false" />
                    <asp:Button ID="btnNextStages" runat="server" Text="Next: Pipeline Scheduling" CssClass="btn-add-primary" OnClick="btnNextStages_Click" />
                </div>
            </div>
        </asp:Panel>

        <%-- ==================== STEP 4: PRODUCTION PIPELINE ==================== --%>
        <asp:Panel ID="pnlProductionPipeline" runat="server" Visible="false">
            <asp:Panel ID="pnlPipelineUnit" runat="server" Visible="false">
                <div class="bs-card">
                    <div class="card-toolbar">
                        <div class="toolbar-stats">
                            <asp:Label ID="lblPipelineUnitTitle" runat="server" CssClass="badge-stat" Text="Step 4: Production Pipeline - Unit Assignment"></asp:Label>
                        </div>
                        <asp:Button ID="btnAddPipelineUnit" runat="server" Text="+ Add Unit Pipeline Row" CssClass="btn-add-primary" OnClick="btnAddPipelineUnit_Click" CausesValidation="false" />
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="grdProductionPipelineUnit" runat="server" AutoGenerateColumns="False" 
                            CssClass="enterprise-grid" GridLines="None" 
                            OnRowCommand="grdProductionPipelineUnit_RowCommand"
                            OnRowDataBound="grdProductionPipelineUnit_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="SN">
                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product *">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfPipelineUnitRowId" runat="server" Value='<%# Eval("ROW_ID") %>' />
                                        <asp:DropDownList ID="ddlPipelineDetail" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stage *">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlPipelineStage" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit *">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlPipelineUnit" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPipelineUnit_SelectedIndexChanged"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Division *">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlPipelineDivision" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estimated Days *">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtEstimatedDay" runat="server" CssClass="form-control" Text='<%# Eval("ESTIMATED_DAY") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estimated Time">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtEstimatedTime" runat="server" CssClass="form-control" Text='<%# Eval("ESTIMATED_TIME") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnRemovePipelineUnit" runat="server" Text="Remove" CommandName="RemovePipelineUnit" CommandArgument='<%# Eval("ROW_ID") %>' CssClass="btn-clear" CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlPipelineEmployee" runat="server" Visible="false">
                <div class="bs-card">
                    <div class="card-toolbar">
                        <div class="toolbar-stats">
                            <asp:Label ID="lblPipelineEmployeeTitle" runat="server" CssClass="badge-stat" Text="Step 4: Production Pipeline - Employee Assignment"></asp:Label>
                        </div>
                        <asp:Button ID="btnAddPipelineEmployee" runat="server" Text="+ Add Employee Pipeline Row" CssClass="btn-add-primary" OnClick="btnAddPipelineEmployee_Click" CausesValidation="false" />
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="grdProductionPipelineEmployee" runat="server" AutoGenerateColumns="False" 
                            CssClass="enterprise-grid" GridLines="None" 
                            OnRowCommand="grdProductionPipelineEmployee_RowCommand"
                            OnRowDataBound="grdProductionPipelineEmployee_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="SN">
                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product *">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfPipelineEmpRowId" runat="server" Value='<%# Eval("ROW_ID") %>' />
                                        <asp:DropDownList ID="ddlPipelineDetail" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stage *">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlPipelineStage" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee *">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlPipelineEmployee" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estimated Days *">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtEstimatedDay" runat="server" CssClass="form-control" Text='<%# Eval("ESTIMATED_DAY") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estimated Time">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtEstimatedTime" runat="server" CssClass="form-control" Text='<%# Eval("ESTIMATED_TIME") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnRemovePipelineEmployee" runat="server" Text="Remove" CommandName="RemovePipelineEmployee" CommandArgument='<%# Eval("ROW_ID") %>' CssClass="btn-clear" CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>

            <div class="form-actions">
                <asp:Button ID="btnBackStages" runat="server" Text="Back" CssClass="btn-clear" OnClick="btnBackStages_Click" CausesValidation="false" />
                <asp:Button ID="btnSaveProduction" runat="server" Text="Save Production" CssClass="btn-add-primary" OnClick="btnSaveProduction_Click" />
                <asp:Button ID="btnClearAll" runat="server" Text="Clear Form" CssClass="btn-clear" OnClick="btnClearAll_Click" CausesValidation="false" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>