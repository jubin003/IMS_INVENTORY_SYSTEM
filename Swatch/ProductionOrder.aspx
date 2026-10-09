<%@ Page Title="Create Production Order" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProductionOrder.aspx.cs" Inherits="ProductionOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="css/ims-webform.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <style>
        .select2-container .select2-selection--single {
            height: 34px; border: 1px solid #cbd5e1; border-radius: 6px; padding: 4px 12px;
        }
        .form-section { padding: 20px; }
    </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="imsv5-container">
                
                <div class="imsv5-page-header">
                    <div>
                        <h2 class="imsv5-page-title">Create Production Order</h2>
                        <div class="imsv5-page-subtitle">Define master production details and attach manufacturing line items.</div>
                    </div>
                </div>

                <div class="bs-card">
                    <div class="card-toolbar">
                        <h4 style="margin: 0; font-size: 14px; font-weight: bold; color: #334155;">
                            <span style="color: #0284c7;">●</span> 1. PRODUCTION MASTER DETAILS
                        </h4>
                    </div>
                    <div class="form-section">
                        <div class="row" style="margin-bottom: 15px;">
                            <div class="col-md-3 form-group">
                                <label class="filter-label">Production Number <span class="req">*</span></label>
                                <asp:TextBox ID="txtProductionNo" runat="server" CssClass="form-control" placeholder="e.g. PRD-2026-001"></asp:TextBox>
                            </div>
                            <div class="col-md-3 form-group">
                                <label class="filter-label">Production Type <span class="req">*</span></label>
                                <asp:DropDownList ID="ddlProductionType" runat="server" CssClass="form-control chosen-select">
                                    <asp:ListItem Text="-- Select Type --" Value=""></asp:ListItem>
                                    <asp:ListItem Text="In-House Manufacturing" Value="InHouse"></asp:ListItem>
                                    <asp:ListItem Text="Sub-Contracting" Value="SubContract"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3 form-group">
                                <label class="filter-label">Customer Purchase Order <span class="req">*</span></label>
                                <asp:DropDownList ID="ddlCustomerPO" runat="server" CssClass="form-control chosen-select"></asp:DropDownList>
                            </div>
                            <div class="col-md-3 form-group">
                                <label class="filter-label">Status <span class="req">*</span></label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control chosen-select">
                                    <asp:ListItem Text="Planned" Value="Planned"></asp:ListItem>
                                    <asp:ListItem Text="In Progress" Value="InProgress"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3 form-group">
                                <label class="filter-label">Target Production Date</label>
                                <asp:TextBox ID="txtProductionDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3 form-group">
                                <label class="filter-label">Target Completion Date</label>
                                <asp:TextBox ID="txtCompletionDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="bs-card">
                    <div class="card-toolbar">
                        <h4 style="margin: 0; font-size: 14px; font-weight: bold; color: #334155;">
                            <span style="color: #0284c7;">●</span> 2. ADD PRODUCTION LINE ITEMS
                        </h4>
                    </div>
                    <div class="form-section">
                        <div class="row" style="margin-bottom: 20px; align-items: flex-end;">
                            <div class="col-md-3 form-group mb-0">
                                <label class="filter-label">Product <span class="req">*</span></label>
                                <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control chosen-select"></asp:DropDownList>
                            </div>
                            <div class="col-md-2 form-group mb-0">
                                <label class="filter-label">Size</label>
                                <asp:DropDownList ID="ddlSize" runat="server" CssClass="form-control chosen-select">
                                    <asp:ListItem Text="-- Select --" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Small" Value="S"></asp:ListItem>
                                    <asp:ListItem Text="Medium" Value="M"></asp:ListItem>
                                    <asp:ListItem Text="Large" Value="L"></asp:ListItem>
                                    <asp:ListItem Text="X-Large" Value="XL"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 form-group mb-0">
                                <label class="filter-label">Swatch (Color/Fabric)</label>
                                <asp:DropDownList ID="ddlSwatch" runat="server" CssClass="form-control chosen-select"></asp:DropDownList>
                            </div>
                            <div class="col-md-2 form-group mb-0">
                                <label class="filter-label">Quantity <span class="req">*</span></label>
                                <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" placeholder="Qty"></asp:TextBox>
                            </div>
                            <div class="col-md-2 form-group mb-0">
                                <label class="filter-label">Unit <span class="req">*</span></label>
                                <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control chosen-select"></asp:DropDownList>
                            </div>
                            <div class="col-md-1 form-group mb-0">
                                <asp:LinkButton ID="btnAddDetail" runat="server" CssClass="btn-add-primary w-100" OnClick="btnAddDetail_Click">
                                    <i class="fa fa-plus"></i> Add
                                </asp:LinkButton>
                            </div>
                        </div>

                        <div class="table-responsive">
                            <asp:GridView ID="gvProductionDetails" runat="server" AutoGenerateColumns="false" GridLines="None" 
                                CssClass="enterprise-grid" DataKeyNames="PRODUCT_ID,SIZE_ID,SWATCH_ID" OnRowDeleting="gvProductionDetails_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="PRODUCT_NAME" HeaderText="Product" ItemStyle-CssClass="cell-entity" />
                                    <asp:BoundField DataField="SIZE_NAME" HeaderText="Size" />
                                    <asp:BoundField DataField="SWATCH_NAME" HeaderText="Swatch" />
                                    <asp:BoundField DataField="QUANTITY" HeaderText="Qty" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="UNIT_NAME" HeaderText="Unit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                    
                                    <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="col-actions" HeaderStyle-CssClass="col-actions">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnRemove" runat="server" CommandName="Delete" CssClass="text-danger action-btn-group" CausesValidation="false">
                                                <i class="fa fa-trash btn-action-icon"></i> Remove
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="text-center text-muted" style="padding: 20px;">
                                        No line items added yet. Select attributes above to build the production order.
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 40px;">
                    <div class="col-md-8 text-left">
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-4 text-right">
                        <asp:Button ID="btnSaveOrder" runat="server" Text="Save Production Order" CssClass="btn-view-primary" OnClick="btnSaveOrder_Click" />
                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script type="text/javascript">
        function initializeSelect2() {
            $('.chosen-select').select2({ width: '100%', allowClear: true });
        }
        $(document).ready(function () { initializeSelect2(); });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () { initializeSelect2(); });
    </script>
</asp:Content>