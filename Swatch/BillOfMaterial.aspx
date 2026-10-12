<%@ Page Title="Manage Bill of Materials" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillOfMaterial.aspx.cs" Inherits="BillOfMaterial" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="css/ims-webform.css" rel="stylesheet" />
 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="imsv5-container">
                <div class="imsv5-page-header">
                    <div class="imsv5-title-block">
                        <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                            <span>Production</span>
                            <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path></svg>
                            <span class="active">Bill of Materials</span>
                        </nav>
                        <h1 class="imsv5-page-title">Manage Bill of Materials</h1>
                        <p class="imsv5-page-subtitle">Configure finished products and define raw material recipe ratios.</p>
                    </div>
                    <div>
                        <a href="ViewBillOfMaterial.aspx" class="btn-enterprise-outline">View All BOMs</a>
                    </div>
                </div>
 
                <%-- STEP 1: SELECT FINISHED PRODUCT --%>
                <asp:Panel ID="pnlFinishedProduct" runat="server">
                    <div class="bs-card">
                        <div class="card-toolbar">
                            <div class="toolbar-stats">
                                <span class="badge-stat">Step 1: Select Finished Product</span>
                            </div>
                        </div>
 
                        <div class="form-grid form-grid-3">
                            <div class="form-group form-span-2">
                                <label for="<%= ddlFinishedProduct.ClientID %>">Finished Product <span class="req">*</span></label>
                                <asp:DropDownList ID="ddlFinishedProduct" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFinishedProduct_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
 
                <%-- STEP 2: RAW MATERIAL RECIPE GRID WITH INLINE HEADER ADDITION --%>
                <asp:Panel ID="pnlRawMaterials" runat="server" Visible="false">
                    <div class="bs-card">
                        <div class="card-toolbar">
                            <div class="toolbar-stats">
                                <span class="badge-stat">Step 2: Add Raw Materials to Recipe</span>
                            </div>
                        </div>
 
                        <div class="table-responsive">
                            <asp:GridView ID="gvBOMDetails" runat="server"
                                Width="100%"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                CssClass="enterprise-grid"
                                DataKeyNames="MATERIAL_ID"
                                ShowHeaderWhenEmpty="true"
                                OnRowDataBound="gvBOMDetails_RowDataBound"
                                OnRowDeleting="gvBOMDetails_RowDeleting"
                                OnRowEditing="gvBOMDetails_RowEditing"
                                OnRowUpdating="gvBOMDetails_RowUpdating"
                                OnRowCancelingEdit="gvBOMDetails_RowCancelingEdit">
                                <Columns>
                                    <%-- SN COLUMN --%>
                                    <asp:TemplateField HeaderText="SN">
                                        <HeaderStyle CssClass="col-sn" />
                                        <ItemStyle CssClass="text-center col-sn" />
                                        <ItemTemplate>
                                            <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
 
                                    <%-- RAW MATERIAL COLUMN (HEADER SELECT) --%>
                                    <asp:TemplateField HeaderText="Raw Material Name">
                                        <HeaderStyle CssClass="col-name" />
                                        <ItemStyle CssClass="col-name font-semibold" />
                                        <HeaderTemplate>
                                            <div class="header-field-group">
                                                <span class="header-label">Raw Material Name</span>
                                                <asp:DropDownList ID="ddlRawMaterialH" runat="server" 
                                                    CssClass="form-control inline-select" 
                                                    style="max-width: 100% !important; min-width: 260px !important;" 
                                                    AutoPostBack="true" 
                                                    OnSelectedIndexChanged="ddlRawMaterialH_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="cell-entity">
                                                <span class="entity-bullet"></span>
                                                <span><%# Eval("MATERIAL_NAME") %></span>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
 
                                    <%-- UNIT COLUMN (HEADER AUTO-FILLED) --%>
                                    <asp:TemplateField HeaderText="Unit">
                                        <HeaderStyle CssClass="col-status" />
                                        <ItemStyle CssClass="col-status" />
                                        <HeaderTemplate>
                                            <div class="header-field-group">
                                                <span class="header-label">Unit</span>
                                                <asp:Label ID="lblUnitH" runat="server" CssClass="form-control form-readonly" Text="-"></asp:Label>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span><%# Eval("UNIT") %></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
 
                                    <%-- QUANTITY COLUMN (HEADER INPUT) --%>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <HeaderStyle CssClass="col-rate" />
                                        <ItemStyle CssClass="col-rate" />
                                        <HeaderTemplate>
                                            <div class="header-field-group">
                                                <span class="header-label">Quantity</span>
                                                <asp:TextBox ID="txtQtyH" runat="server" CssClass="form-control inline-input" placeholder="Enter qty..."></asp:TextBox>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <span class="cell-rate"><%# Eval("QTY") %></span>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditQty" runat="server" Text='<%# Bind("QTY") %>' CssClass="form-control rate-field"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
 
                                    <%-- ACTIONS COLUMN (HEADER ADD BUTTON) --%>
                                    <asp:TemplateField HeaderText="Actions">
                                        <HeaderStyle CssClass="col-actions text-center" />
                                        <ItemStyle CssClass="col-actions text-center" />
                                        <HeaderTemplate>
                                            <div class="header-action-group">
                                                <asp:Button ID="btnAddMaterial" runat="server" Text="+ Add" CssClass="btn-add-primary" OnClick="btnAddMaterial_Click" CausesValidation="false" />
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="action-btn-group">
                                                <asp:ImageButton ID="btnEditRow" runat="server" CommandName="Edit" ImageUrl="~/images/icons/edit.png" ToolTip="Edit Quantity" CssClass="btn-action-icon edit-icon" CausesValidation="false" />
                                                <asp:ImageButton ID="btnDeleteRow" runat="server" CommandName="Delete" ImageUrl="~/images/icons/deletes.png" ToolTip="Remove Material" CssClass="btn-action-icon delete-icon" CausesValidation="false" OnClientClick="return confirm('Remove this raw material from recipe?');" />
                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="action-btn-group">
                                                <asp:ImageButton ID="btnUpdateRow" runat="server" CommandName="Update" ImageUrl="~/images/icons/upload.png" ToolTip="Save Changes" CssClass="btn-action-icon update-icon" CausesValidation="false" />
                                                <asp:ImageButton ID="btnCancelRow" runat="server" CommandName="Cancel" ImageUrl="~/images/icons/cancel.png" ToolTip="Cancel" CssClass="btn-action-icon cancel-icon" CausesValidation="false" />
                                            </div>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="gridview-pager" />
                            </asp:GridView>
                        </div>
 
                        <div class="form-actions">
                            <asp:Label ID="lblMessage" runat="server" CssClass="badge-stat"></asp:Label>
                            <asp:Button ID="btnSaveBOM" runat="server" Text="Save Final BOM" CssClass="btn-add-primary" OnClick="btnSaveBOM_Click" CausesValidation="false" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>  