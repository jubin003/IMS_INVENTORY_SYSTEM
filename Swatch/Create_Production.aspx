<%@ Page Title="Production Management" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Create_Production.aspx.cs" Inherits="Production_Create_Production" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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

        <%-- ==================== STEP 1: PRODUCTION MASTER ==================== --%>
        <asp:Panel ID="pnlProductionMaster" runat="server">
            <div class="bs-card">
                <div class="card-toolbar">
                    <div class="toolbar-stats">
                        <asp:Label ID="lblMasterTitle" runat="server" CssClass="badge-stat" Text="Step 1: Production Master"></asp:Label>
                    </div>
                </div>

                <div class="form-grid form-grid-3">
                    <div class="form-group">
                        <label for="<%= txtProductionDate.ClientID %>">Production Date *</label>
                        <asp:TextBox ID="txtProductionDate" runat="server" CssClass="form-control datepicker" placeholder="Select production date..."></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="<%= txtCompletionDate.ClientID %>">Completion Date *</label>
                        <asp:TextBox ID="txtCompletionDate" runat="server" CssClass="form-control datepicker" placeholder="Select completion date..."></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="<%= txtProductionType.ClientID %>">Production Type *</label>
                        <asp:TextBox ID="txtProductionType" runat="server" CssClass="form-control" placeholder="Enter production type..."></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="<%= txtCustomerPOId.ClientID %>">Customer PO ID *</label>
                        <asp:TextBox ID="txtCustomerPOId" runat="server" CssClass="form-control" placeholder="Enter customer PO ID..."></asp:TextBox>
                    </div>
                </div>

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

                <div class="form-grid form-grid-3">
                    <div class="form-group">
                        <label for="<%= txtMultipleDeliveryId.ClientID %>">Multiple Delivery ID *</label>
                        <asp:TextBox ID="txtMultipleDeliveryId" runat="server" CssClass="form-control" placeholder="Enter delivery ID..."></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Production Date</label>
                        <asp:Label ID="lblDetailsProductionDate" runat="server" CssClass="form-control form-readonly"></asp:Label>
                    </div>
                    <div class="form-group">
                        <label>Completion Date</label>
                        <asp:Label ID="lblDetailsCompletionDate" runat="server" CssClass="form-control form-readonly"></asp:Label>
                    </div>
                </div>

                <div class="card-toolbar">
                    <div class="toolbar-stats">
                        <asp:Label ID="lblProductTitle" runat="server" CssClass="badge-stat" Text="Production Products"></asp:Label>
                    </div>
                    <asp:Button ID="btnAddProduct" runat="server" Text="+ Add Product" CssClass="btn-add-primary" OnClick="btnAddProduct_Click" CausesValidation="false" />
                </div>

                <div class="table-responsive">
                    <asp:GridView ID="grdProductionDetails" runat="server" AutoGenerateColumns="False" CssClass="enterprise-grid" GridLines="None" OnRowCommand="grdProductionDetails_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="SN">
                                <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product ID *">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtProductId" runat="server" CssClass="form-control" Text='<%# Eval("PRODUCT_ID") %>' placeholder="Product ID"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Size ID">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSizeId" runat="server" CssClass="form-control" Text='<%# Eval("SIZE_ID") %>' placeholder="Size ID"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Swatch ID">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtSwatchId" runat="server" CssClass="form-control" Text='<%# Eval("SWATCH_ID") %>' placeholder="Swatch ID"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity *">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Text='<%# Eval("QUANTITY") %>' placeholder="Quantity"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtUnit" runat="server" CssClass="form-control" Text='<%# Eval("UNIT") %>' placeholder="Unit ID"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemStyle CssClass="text-center" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnRemoveProduct" runat="server" Text="Remove" CommandName="RemoveProduct" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn-clear" CausesValidation="false"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

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

                <div class="table-responsive">
                    <asp:GridView ID="grdProductionStages" runat="server" AutoGenerateColumns="False" CssClass="enterprise-grid" GridLines="None" OnRowCommand="grdProductionStages_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="SN">
                                <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Production Detail ID *">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtStageDetailId" runat="server" CssClass="form-control" Text='<%# Eval("PRODUCTION_DETAIL_ID") %>' placeholder="Detail ID"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stage Master ID *">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtStageMasterId" runat="server" CssClass="form-control" Text='<%# Eval("STAGE_MASTER_ID") %>' placeholder="Stage ID"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit or Employee *">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlStageAssignmentType" runat="server" CssClass="form-control" SelectedValue='<%# Eval("UNIT_OR_EMPLOYEE") %>'>
                                        <asp:ListItem Text="Select..." Value=""></asp:ListItem>
                                        <asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>
                                        <asp:ListItem Text="Employee" Value="Employee"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlStageStatus" runat="server" CssClass="form-control" SelectedValue='<%# Eval("STATUS") %>'>
                                        <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                        <asp:ListItem Text="In Progress" Value="In Progress"></asp:ListItem>
                                        <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnRemoveStage" runat="server" Text="Remove" CommandName="RemoveStage" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn-clear" CausesValidation="false"></asp:LinkButton>
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
            <div class="bs-card">
                <div class="card-toolbar">
                    <div class="toolbar-stats">
                        <asp:Label ID="lblPipelineTitle" runat="server" CssClass="badge-stat" Text="Step 4: Production Pipeline"></asp:Label>
                    </div>
                    <asp:Button ID="btnAddPipeline" runat="server" Text="+ Add Pipeline Row" CssClass="btn-add-primary" OnClick="btnAddPipeline_Click" CausesValidation="false" />
                </div>

                <div class="table-responsive">
                    <asp:GridView ID="grdProductionPipeline" runat="server" AutoGenerateColumns="False" CssClass="enterprise-grid" GridLines="None" OnRowCommand="grdProductionPipeline_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="SN">
                                <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Production Detail ID *">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPipelineDetailId" runat="server" CssClass="form-control" Text='<%# Eval("PRODUCTION_DETAIL_ID") %>' placeholder="Detail ID"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stage ID *">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPipelineStageId" runat="server" CssClass="form-control" Text='<%# Eval("STAGE_ID") %>' placeholder="Stage ID"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- Index 3: Unit ID --%>
                            <asp:TemplateField HeaderText="Unit ID">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPipelineUnitId" runat="server" CssClass="form-control" Text='<%# Eval("PRODUCTION_UNIT") %>' placeholder="Unit ID"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- Index 4: Division ID --%>
                            <asp:TemplateField HeaderText="Division ID">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPipelineDivisionId" runat="server" CssClass="form-control" Text='<%# Eval("PRODUCTION_DIVISION") %>' placeholder="Division ID"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- Index 5: Employee ID --%>
                            <asp:TemplateField HeaderText="Employee ID">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPipelineEmployeeId" runat="server" CssClass="form-control" Text='<%# Eval("EMPLOYEE_ID") %>' placeholder="Employee ID"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Estimated Days *">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtEstimatedDay" runat="server" CssClass="form-control" Text='<%# Eval("ESTIMATED_DAY") %>' placeholder="Days"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estimated Time">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtEstimatedTime" runat="server" CssClass="form-control" Text='<%# Eval("ESTIMATED_TIME") %>' placeholder="HH:mm"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Production Date *">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPipelineProductionDate" runat="server" CssClass="form-control datepicker" Text='<%# Eval("PRODUCTION_DATE") %>' placeholder="Date"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Production Time">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPipelineProductionTime" runat="server" CssClass="form-control" Text='<%# Eval("PRODUCTION_TIME") %>' placeholder="HH:mm"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Completion Date *">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPipelineCompletionDate" runat="server" CssClass="form-control datepicker" Text='<%# Eval("COMPLETION_DATE") %>' placeholder="Date"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Completion Time">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPipelineCompletionTime" runat="server" CssClass="form-control" Text='<%# Eval("COMPLETION_TIME") %>' placeholder="HH:mm"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPipelineStatus" runat="server" CssClass="form-control" SelectedValue='<%# Eval("STATUS") %>'>
                                        <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                        <asp:ListItem Text="In Progress" Value="In Progress"></asp:ListItem>
                                        <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnRemovePipeline" runat="server" Text="Remove" CommandName="RemovePipeline" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn-clear" CausesValidation="false"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="form-actions">
                    <asp:Button ID="btnBackStages" runat="server" Text="Back" CssClass="btn-clear" OnClick="btnBackStages_Click" CausesValidation="false" />
                    <asp:Button ID="btnSaveProduction" runat="server" Text="Save Production" CssClass="btn-add-primary" OnClick="btnSaveProduction_Click" />
                    <asp:Button ID="btnClearAll" runat="server" Text="Clear Form" CssClass="btn-clear" OnClick="btnClearAll_Click" CausesValidation="false" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>