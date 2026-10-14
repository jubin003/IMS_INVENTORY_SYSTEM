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

                            <%-- Swatch Column with Modal Picker Trigger --%>
                            <asp:TemplateField HeaderText="Swatch">
                                <HeaderStyle Width="220px" />
                                <ItemStyle Width="220px" />
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfSwatchId" runat="server" Value='<%# Eval("SWATCH_ID") %>' />
                                    <asp:DropDownList ID="ddlSwatch" runat="server" Style="display:none;"></asp:DropDownList>

                                    <div class="swatch-cell-picker">
                                        <button type="button"
                                            class='<%# string.IsNullOrEmpty(Eval("SWATCH_ID") as string) ? "swatch-select-btn" : "swatch-select-btn has-selected" %>'
                                            onclick="openSwatchModalForGridRow(this)"
                                            <%# string.IsNullOrEmpty(Eval("SWATCH_TYPE_ID") as string) ? "disabled title='Please select Swatch Type first'" : "" %>>

                                            <img class="swatch-cell-thumb"
                                                src='<%# GetSwatchImageUrl(Eval("SWATCH_ID")) %>'
                                                style='<%# string.IsNullOrEmpty(Eval("SWATCH_ID") as string) ? "display:none;" : "display:inline-block;" %>'
                                                alt="Swatch Preview" />

                                            <span class="swatch-cell-label">
                                                <asp:Literal ID="litSwatchName" runat="server" Text='<%# GetSwatchDisplayName(Eval("SWATCH_ID")) %>'></asp:Literal>
                                            </span>

                                            <svg class="swatch-cell-chevron" fill="none" stroke="currentColor" viewBox="0 0 24 24" width="14" height="14">
                                                <path d="M19 9l-7 7-7-7" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                            </svg>
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- Quantity Column with Embedded Unit Label --%>
                            <asp:TemplateField HeaderText="Quantity">
                                <ItemTemplate>
                                    <div class="input-with-unit">
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Text='<%# Eval("QUANTITY") %>' placeholder="Qty"></asp:TextBox>
                                        <asp:Label ID="lblUnit" runat="server" CssClass="unit-label"></asp:Label>
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
                    <asp:Button ID="btnSaveAndNextDelivery" runat="server" Text="Save & Add Another Delivery" CssClass="btn-add-secondary" OnClick="btnSaveAndNextDelivery_Click" CausesValidation="false" />
                    <asp:Button ID="btnFinalSave" runat="server" Text="Final Save Purchase Order" CssClass="btn-add-primary" OnClick="btnFinalSave_Click" CausesValidation="false" />
                </div>
            </div>
        </asp:Panel>
    </div>

    <!-- ======================================================== -->
    <!-- SWATCH SELECTION OVERLAY MODAL -->
    <!-- ======================================================== -->
    <div id="swatchModalOverlay" class="swatch-modal-overlay" style="display:none;" onclick="handleModalOverlayClick(event)">
        <div class="swatch-modal-dialog" role="dialog" aria-modal="true" aria-labelledby="swatchModalTitle">
            <!-- Header -->
            <div class="swatch-modal-header">
                <div class="swatch-modal-title-wrap">
                    <h3 id="swatchModalTitle" class="swatch-modal-title">Select Swatch</h3>
                    <span id="swatchModalSubtitle" class="swatch-modal-subtitle">Choose a swatch for this item</span>
                </div>
                <button type="button" class="swatch-modal-close" onclick="closeSwatchModal()" aria-label="Close modal">&times;</button>
            </div>

            <!-- Search Bar -->
            <div class="swatch-modal-search-bar">
                <div class="swatch-modal-search-box">
                    <svg class="swatch-search-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24" width="18" height="18">
                        <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                    </svg>
                    <input type="text" id="swatchModalSearchInput" class="swatch-modal-search-input"
                           placeholder="Search swatch by name or code..."
                           oninput="filterSwatchModalCards()" />
                    <button type="button" id="swatchModalClearSearch" class="swatch-modal-clear-btn" style="display:none;" onclick="clearSwatchModalSearch()">&times;</button>
                </div>
                <div class="swatch-count-badge" id="swatchCountBadge">0 Swatches</div>
            </div>

            <!-- Modal Body: Centered Grid View -->
            <div class="swatch-modal-body">
                <div id="swatchModalLoading" class="swatch-modal-state" style="display:none;">
                    <div class="swatch-spinner"></div>
                    <span>Loading swatches...</span>
                </div>

                <div id="swatchModalEmpty" class="swatch-modal-state" style="display:none;">
                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24" width="40" height="40">
                        <path d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                    </svg>
                    <span id="swatchModalEmptyText">No swatches found matching your search.</span>
                </div>

                <div id="swatchModalGrid" class="swatch-modal-grid"></div>
            </div>

            <!-- Confirmation Footer -->
            <div class="swatch-modal-footer">
                <div class="swatch-modal-selected-info">
                    <span class="selected-label">Selected:</span>
                    <span id="swatchModalSelectedName" class="selected-name">None</span>
                </div>
                <div class="swatch-modal-actions">
                    <button type="button" class="btn-swatch-cancel" onclick="closeSwatchModal()">Cancel</button>
                    <button type="button" id="btnConfirmSwatchSelection" class="btn-swatch-confirm" disabled onclick="confirmSwatchSelection()">Confirm Selection</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var activeSwatchRow = null;
        var currentSelectedSwatch = null;

        function openSwatchModalForGridRow(btnElement) {
            var row = btnElement.closest('tr');
            if (!row) return;

            var ddlType = row.querySelector('[id*="ddlSwatchType"]');
            if (!ddlType || !ddlType.value) {
                alert("Please select a Swatch Type for this row first.");
                return;
            }

            activeSwatchRow = row;
            currentSelectedSwatch = null;

            var typeId = ddlType.value;
            var typeName = ddlType.options[ddlType.selectedIndex].text;
            document.getElementById('swatchModalSubtitle').textContent = "Category: " + typeName;

            var hfSwatch = row.querySelector('[id*="hfSwatchId"]');
            var initialSwatchId = hfSwatch ? hfSwatch.value : "";

            document.getElementById('swatchModalSearchInput').value = "";
            document.getElementById('swatchModalClearSearch').style.display = "none";
            document.getElementById('swatchModalSelectedName').textContent = "None";
            document.getElementById('btnConfirmSwatchSelection').disabled = true;

            document.getElementById('swatchModalOverlay').style.display = "flex";
            loadSwatchesForModal(typeId, initialSwatchId);
        }

        function loadSwatchesForModal(typeId, preselectId) {
            var grid = document.getElementById('swatchModalGrid');
            var loading = document.getElementById('swatchModalLoading');
            var empty = document.getElementById('swatchModalEmpty');

            grid.innerHTML = "";
            grid.style.display = "none";
            empty.style.display = "none";
            loading.style.display = "flex";

            var targetUrl = window.location.pathname + "?action=get_swatches&typeId=" + encodeURIComponent(typeId);

            fetch(targetUrl)
                .then(function (res) {
                    if (!res.ok) {
                        throw new Error("HTTP error " + res.status);
                    }
                    return res.json();
                })
                .then(function (data) {
                    loading.style.display = "none";

                    if (!data.success || !data.swatches || data.swatches.length === 0) {
                        empty.style.display = "flex";
                        document.getElementById('swatchModalEmptyText').textContent = "No swatches configured for this category.";
                        document.getElementById('swatchCountBadge').textContent = "0 Swatches";
                        return;
                    }

                    document.getElementById('swatchCountBadge').textContent = data.swatches.length + " Swatches";
                    grid.style.display = "grid";

                    data.swatches.forEach(function (s) {
                        var card = document.createElement('div');
                        card.className = "swatch-card";
                        card.setAttribute("data-id", s.id);
                        card.setAttribute("data-name", s.name);
                        card.setAttribute("data-img", s.imageUrl);

                        card.innerHTML =
                            '<div class="swatch-card-check">' +
                                '<svg fill="none" stroke="currentColor" viewBox="0 0 24 24" width="14" height="14">' +
                                    '<path d="M5 13l4 4L19 7" stroke-width="3" stroke-linecap="round" stroke-linejoin="round"></path>' +
                                '</svg>' +
                            '</div>' +
                            '<div class="swatch-card-thumb-wrap">' +
                                '<img class="swatch-card-thumb" src="' + s.imageUrl + '" alt="' + s.name + '" loading="lazy" />' +
                            '</div>' +
                            '<div class="swatch-card-info">' +
                                '<span class="swatch-card-name" title="' + s.name + '">' + s.name + '</span>' +
                            '</div>';

                        card.onclick = function () { selectSwatchCard(card); };
                        card.ondblclick = function () {
                            selectSwatchCard(card);
                            confirmSwatchSelection();
                        };

                        grid.appendChild(card);

                        if (preselectId && s.id === preselectId) {
                            selectSwatchCard(card);
                        }
                    });

                    document.getElementById('swatchModalSearchInput').focus();
                })
                .catch(function (err) {
                    console.error("Swatch Load Error:", err);
                    loading.style.display = "none";
                    empty.style.display = "flex";
                    document.getElementById('swatchModalEmptyText').textContent = "Error loading swatches.";
                });
        }

        function selectSwatchCard(cardElement) {
            var cards = document.querySelectorAll('.swatch-card');
            cards.forEach(function (c) { c.classList.remove('selected'); });

            cardElement.classList.add('selected');

            currentSelectedSwatch = {
                id: cardElement.getAttribute('data-id'),
                name: cardElement.getAttribute('data-name'),
                imageUrl: cardElement.getAttribute('data-img')
            };

            document.getElementById('swatchModalSelectedName').textContent = currentSelectedSwatch.name;
            document.getElementById('btnConfirmSwatchSelection').disabled = false;
        }

        function confirmSwatchSelection() {
            if (!currentSelectedSwatch || !activeSwatchRow) return;

            // Update hidden field for ASP.NET PostBack save
            var hfSwatch = activeSwatchRow.querySelector('[id*="hfSwatchId"]');
            if (hfSwatch) hfSwatch.value = currentSelectedSwatch.id;

            // Keep hidden DropDownList in sync
            var ddlSwatch = activeSwatchRow.querySelector('[id*="ddlSwatch"]');
            if (ddlSwatch) ddlSwatch.value = currentSelectedSwatch.id;

            var pickerBtn = activeSwatchRow.querySelector('.swatch-select-btn');
            if (pickerBtn) {
                pickerBtn.classList.add('has-selected');

                var label = pickerBtn.querySelector('.swatch-cell-label');
                if (label) {
                    label.textContent = currentSelectedSwatch.name;
                }

                // Ensure image element exists and is visible
                var img = pickerBtn.querySelector('.swatch-cell-thumb');
                if (!img) {
                    img = document.createElement('img');
                    img.className = 'swatch-cell-thumb';
                    img.alt = 'Swatch Preview';
                    pickerBtn.insertBefore(img, label);
                }

                img.src = currentSelectedSwatch.imageUrl;
                img.style.display = 'inline-block';
            }

            closeSwatchModal();
        }

        function filterSwatchModalCards() {
            var input = document.getElementById('swatchModalSearchInput');
            var filter = input.value.trim().toLowerCase();
            var clearBtn = document.getElementById('swatchModalClearSearch');
            clearBtn.style.display = filter.length > 0 ? "block" : "none";

            var cards = document.querySelectorAll('.swatch-card');
            var matchCount = 0;

            cards.forEach(function (card) {
                var name = (card.getAttribute('data-name') || '').toLowerCase();
                if (name.indexOf(filter) > -1) {
                    card.style.display = "flex";
                    matchCount++;
                } else {
                    card.style.display = "none";
                }
            });

            document.getElementById('swatchCountBadge').textContent = matchCount + " Swatches";
            var empty = document.getElementById('swatchModalEmpty');
            if (matchCount === 0) {
                empty.style.display = "flex";
                document.getElementById('swatchModalEmptyText').textContent = "No swatches found matching \"" + filter + "\".";
            } else {
                empty.style.display = "none";
            }
        }

        function clearSwatchModalSearch() {
            var input = document.getElementById('swatchModalSearchInput');
            input.value = "";
            filterSwatchModalCards();
            input.focus();
        }

        function closeSwatchModal() {
            document.getElementById('swatchModalOverlay').style.display = "none";
            activeSwatchRow = null;
            currentSelectedSwatch = null;
        }

        function handleModalOverlayClick(e) {
            if (e.target.id === 'swatchModalOverlay') {
                closeSwatchModal();
            }
        }

        window.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') closeSwatchModal();
        });
    </script>
</asp:Content>