<%@ Page Title="View Bill of Materials" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewBillOfMaterial.aspx.cs" Inherits="ViewBillOfMaterial" %>

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
                        <h1 class="imsv5-page-title">Bill of Materials List</h1>
                        <p class="imsv5-page-subtitle">View, search, and manage existing product recipes.</p>
                    </div>
                    <div>
                        <a href="BillOfMaterial.aspx" class="btn-enterprise-outline"><i class="fa fa-plus"></i> Create New BOM</a>
                    </div>
                </div>

                <%-- PANEL 1: MASTER BOM LIST --%>
                <asp:Panel ID="pnlMasterView" runat="server">
                    <div class="bs-card">
                        <div class="card-toolbar" style="flex-direction: column; align-items: flex-start; gap: 12px;">
                            <div style="display: flex; justify-content: space-between; align-items: center; width: 100%;">
                                <div class="toolbar-stats"><span class="badge-stat">Manage BOM Recipes</span></div>
                                <asp:Label ID="lblMessage" runat="server" CssClass="badge-stat" />
                            </div>

                            <%-- REAL-TIME SEARCH BAR DIRECTLY BELOW TEXT --%>
                            <div class="search-box-wrapper" style="width: 100%; max-width: 380px;">
                                <svg class="search-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                                </svg>
                                <input type="text" id="bomSearchInput" class="table-search-input" placeholder="Search product name or code..." onkeyup="filterBOMTable()" autocomplete="off" />
                            </div>
                        </div>

                        <div class="table-responsive">
                            <asp:GridView ID="gvBOMMaster" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="None" CssClass="enterprise-grid" OnRowCommand="gvBOMMaster_RowCommand">
                                <Columns>
                                    <%-- SN COLUMN --%>
                                    <asp:TemplateField HeaderText="SN">
                                        <HeaderStyle Width="50px" CssClass="col-sn" />
                                        <ItemStyle Width="50px" CssClass="text-center col-sn" />
                                        <ItemTemplate>
                                            <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="FINISHED_PRODUCT_ID" HeaderText="Product ID" Visible="false" />

                                    <%-- PRODUCT CODE COLUMN --%>
                                    <asp:BoundField DataField="PRODUCT_CODE" HeaderText="PRODUCT CODE" 
                                        HeaderStyle-Width="120px" 
                                        ItemStyle-Width="120px" 
                                        HeaderStyle-CssClass="col-code" 
                                        ItemStyle-CssClass="col-code font-semibold" />

                                    <%-- FINISHED PRODUCT NAME COLUMN (Takes up remaining flex space) --%>
                                    <asp:TemplateField HeaderText="FINISHED PRODUCT NAME">
                                        <HeaderStyle CssClass="col-name" />
                                        <ItemStyle CssClass="col-name font-semibold" />
                                        <ItemTemplate>
                                            <div class="cell-entity">
                                                <span class="entity-bullet"></span>
                                                <span style="color: #0f172a !important;"><%# Eval("PRODUCT_NAME") %></span>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- RAW MATERIAL COUNT COLUMN (Centered) --%>
                                    <asp:TemplateField HeaderText="RAW MATERIAL COUNT">
                                        <HeaderStyle Width="190px" HorizontalAlign="Center" />
                                        <ItemStyle Width="190px" HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            <div style="text-align: center !important; font-weight: 700; color: #475569; width: 100%;">
                                                RAW MATERIAL COUNT
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="text-align: center !important; width: 100%;">
                                                <span class="cell-rate" style="font-weight: 700; color: #0f172a;"><%# Eval("INGREDIENT_COUNT") %></span>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- ACTIONS COLUMN --%>
                                    <asp:TemplateField HeaderText="ACTIONS">
                                        <HeaderStyle Width="110px" CssClass="col-actions text-center" />
                                        <ItemStyle Width="110px" CssClass="col-actions text-center" />
                                        <ItemTemplate>
                                            <div class="action-btn-group" style="justify-content: center !important;">
                                                <asp:ImageButton ID="btnViewDetails" runat="server" CommandName="ViewDetails" CommandArgument='<%# Eval("FINISHED_PRODUCT_ID") %>' ImageUrl="~/images/icons/view.png" ToolTip="View Recipe" CssClass="btn-action-icon view-icon" CausesValidation="false" />
                                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="EditBOM" CommandArgument='<%# Eval("FINISHED_PRODUCT_ID") %>' ImageUrl="~/images/icons/edit.png" ToolTip="Edit BOM" CssClass="btn-action-icon edit-icon" CausesValidation="false" />
                                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteBOM" CommandArgument='<%# Eval("FINISHED_PRODUCT_ID") %>' ImageUrl="~/images/icons/deletes.png" ToolTip="Delete Recipe" CssClass="btn-action-icon delete-icon" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this BOM recipe?');" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="card-toolbar"><span class="badge-stat">No Bill of Materials found.</span></div>
                                </EmptyDataTemplate>
                                <PagerStyle CssClass="gridview-pager" />
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>

                <%-- PANEL 2: RECIPE BREAKDOWN DETAILS --%>
                <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                    <div class="bs-card">
                        <div class="card-toolbar">
                            <div class="toolbar-stats">
                                <span class="badge-stat">Recipe Breakdown: <asp:Label ID="lblDetailProductName" runat="server" Font-Bold="true" ForeColor="#0284c7" /></span>
                            </div>
                            <div>
                                <asp:Button ID="btnCloseDetails" runat="server" Text="← Back to Recipes" CssClass="btn-clear" OnClick="btnCloseDetails_Click" CausesValidation="false" />
                            </div>
                        </div>

                        <div class="table-responsive">
                            <asp:GridView ID="gvRecipeDetails" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="None" CssClass="enterprise-grid">
                                <Columns>
                                    <asp:TemplateField HeaderText="SN">
                                        <HeaderStyle Width="50px" CssClass="col-sn" />
                                        <ItemStyle Width="50px" CssClass="text-center col-sn" />
                                        <ItemTemplate><span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span></ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="MATERIAL_CODE" HeaderText="MATERIAL CODE" HeaderStyle-Width="130px" ItemStyle-Width="130px" HeaderStyle-CssClass="col-code" ItemStyle-CssClass="col-code font-semibold" />

                                    <asp:TemplateField HeaderText="RAW MATERIAL NAME">
                                        <HeaderStyle CssClass="col-name" />
                                        <ItemStyle CssClass="col-name font-semibold" />
                                        <ItemTemplate>
                                            <div class="cell-entity">
                                                <span class="entity-bullet"></span>
                                                <span style="color: #0f172a !important;"><%# Eval("MATERIAL_NAME") %></span>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="UNIT">
                                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                        <ItemStyle Width="90px" HorizontalAlign="Center" />
                                        <HeaderTemplate>
                                            <div style="text-align: center !important; font-weight: 700; color: #475569;">UNIT</div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="text-align: center !important;">
                                                <span style="color: #475569; font-weight: 600;"><%# Eval("UNIT") %></span>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="REQUIRED QUANTITY">
                                        <HeaderStyle Width="180px" HorizontalAlign="Right" />
                                        <ItemStyle Width="180px" HorizontalAlign="Right" />
                                        <HeaderTemplate>
                                            <div style="text-align: right !important; padding-right: 20px !important; white-space: nowrap !important; font-weight: 700; color: #475569;">REQUIRED QUANTITY</div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="text-align: right !important; padding-right: 20px !important;">
                                                <span class="cell-rate" style="font-weight: 700; color: #0f172a;"><%# Eval("QUANTITY") %></span>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="card-toolbar"><span class="badge-stat">No raw materials found in this recipe.</span></div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function filterBOMTable() {
            var input = document.getElementById("bomSearchInput");
            if (!input) return;
            var filter = input.value.toLowerCase();
            var grid = document.getElementById("<%= gvBOMMaster.ClientID %>");
            if (!grid) return;
            var rows = grid.getElementsByTagName("tr");

            // Loop rows skipping table header
            for (var i = 1; i < rows.length; i++) {
                if (rows[i].classList.contains("gridview-pager")) continue;

                var codeCol = rows[i].getElementsByTagName("td")[1];
                var nameCol = rows[i].getElementsByTagName("td")[2];

                if (codeCol && nameCol) {
                    var codeText = (codeCol.textContent || codeCol.innerText).toLowerCase();
                    var nameText = (nameCol.textContent || nameCol.innerText).toLowerCase();

                    rows[i].style.display = (codeText.indexOf(filter) > -1 || nameText.indexOf(filter) > -1) ? "" : "none";
                }
            }
        }
    </script>
</asp:Content>