<%@ Page Title="Manage Swatches" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageSwatches.aspx.cs" Inherits="ManageSwatches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="css/ims-webform.css" rel="stylesheet" />

    <div class="imsv5-container">
        <!-- Header -->
        <div class="imsv5-page-header">
            <div class="imsv5-title-block">
                <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                    <span>Production</span>
                    <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <span class="active">Swatch Configuration</span>
                </nav>
                <h1 class="imsv5-page-title">Swatch Management</h1>
                <p class="imsv5-page-subtitle">Configure swatch name, status, and images for the selected category</p>
            </div>
        </div>

        <!-- Filter Card (Step 1: Category Selection) -->
        <div class="bs-card">
            <div class="card-toolbar">
                <div class="toolbar-stats">
                    <span class="badge-stat">Select Swatch Category</span>
                </div>
            </div>

            <div class="form-grid form-grid-3">
                <div class="form-group form-span-2">
                    <label for="<%= ddlFilterCategory.ClientID %>">Category <span class="req">*</span></label>
                    <asp:DropDownList ID="ddlFilterCategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterCategory_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
        </div>

        <!-- Swatches Card (Step 2: Inline Grid Management) -->
        <%-- Plain full postbacks (no UpdatePanel) so file uploads reach the server, same as Table_Image --%>
        <asp:Panel ID="pnlSwatchesGrid" runat="server" Visible="false">
            <div class="bs-card">
                <!-- Card Toolbar -->
                <div class="card-toolbar">
                    <div class="toolbar-stats">
                        <span class="badge-stat">Manage Swatches</span>
                    </div>

                    <asp:PlaceHolder ID="phSearchWrapper" runat="server">
                        <div class="search-box-wrapper">
                            <svg class="search-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                            </svg>
                            <input type="text" id="clientSearchInput" class="table-search-input" placeholder="Search swatches..." onkeyup="filterSwatchTable()" />
                        </div>
                    </asp:PlaceHolder>

                    <asp:Label ID="lblMessage" runat="server" CssClass="badge-stat"></asp:Label>
                </div>

                <!-- Grid Table -->
                <div class="table-responsive">
                    <asp:GridView ID="gvSwatches" runat="server"
                        AutoGenerateColumns="false"
                        GridLines="None"
                        Width="100%"
                        CssClass="enterprise-grid"
                        DataKeyNames="PK_ID, SWATCH_TYPE_ID, STATUS"
                        OnRowEditing="gvSwatches_RowEditing"
                        OnRowCancelingEdit="gvSwatches_RowCancelingEdit"
                        OnRowUpdating="gvSwatches_RowUpdating"
                        OnRowDataBound="gvSwatches_RowDataBound">
                        <Columns>
                            <%-- Serial Number --%>
                            <asp:TemplateField HeaderText="SN">
                                <HeaderStyle CssClass="col-sn" />
                                <ItemStyle CssClass="text-center col-sn" />
                                <ItemTemplate>
                                    <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- Swatch Image Upload / Display --%>
                            <asp:TemplateField HeaderText="Image">
                                <HeaderStyle Width="140px" CssClass="text-center" />
                                <ItemStyle CssClass="text-center" />
                                <HeaderTemplate>
                                    <div class="header-field-group">
                                        <span class="header-label text-center">Image</span>
                                        <asp:FileUpload ID="fuAddImage" runat="server" CssClass="form-control inline-input" accept="image/*" />
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Image ID="imgSwatch" runat="server"
                                        ImageUrl='<%# GetSwatchImageUrl(Eval("PK_ID")) %>'
                                        AlternateText="Swatch Image"
                                        Width="44px"
                                        Height="44px" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:FileUpload ID="fuEditImage" runat="server" CssClass="form-control edit-input" accept="image/*" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <%-- Swatch Name --%>
                            <asp:TemplateField HeaderText="Swatch Name">
                                <HeaderStyle CssClass="col-name" />
                                <ItemStyle CssClass="col-name font-semibold" />
                                <HeaderTemplate>
                                    <div class="header-field-group">
                                        <span class="header-label">Swatch Name <span class="req">*</span></span>
                                        <asp:TextBox ID="txtSwatchName" runat="server" CssClass="form-control inline-input" placeholder="Enter swatch name..."></asp:TextBox>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="cell-entity">
                                        <span class="entity-bullet"></span>
                                        <span><%# Eval("SWATCH_NAME") %></span>
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSwatchNameE" runat="server" Text='<%# Bind("SWATCH_NAME") %>' CssClass="form-control edit-input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Swatch Code">
                                <HeaderStyle CssClass="col-name" />
                                <ItemStyle CssClass="col-name font-semibold" />
                                <HeaderTemplate>
                                    <div class="header-field-group">
                                        <span class="header-label">Swatch Code <span class="req">*</span></span>
                                        <asp:TextBox ID="txtSwatchCode" runat="server" CssClass="form-control inline-input" placeholder="Enter swatch code..."></asp:TextBox>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="cell-entity">
                                        <span class="entity-bullet"></span>
                                        <span><%# Eval("SWATCH_CODE") %></span>
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSwatchCodeE" runat="server" Text='<%# Bind("SWATCH_CODE") %>' CssClass="form-control edit-input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <%-- Status --%>
                            <asp:TemplateField HeaderText="Status">
                                <HeaderStyle CssClass="col-status" />
                                <ItemStyle CssClass="col-status" />
                                <HeaderTemplate>
                                    <div class="header-field-group">
                                        <span class="header-label">Status</span>
                                        <asp:DropDownList ID="ddlHeaderStatus" runat="server" CssClass="form-control inline-select">
                                            <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class='<%# Eval("STATUS").ToString() == "1" ? "status-pill status-active" : "status-pill status-inactive" %>'>
                                        <%# Eval("STATUS").ToString() == "1" ? "Active" : "Inactive" %>
                                    </span>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlEditStatus" runat="server" CssClass="form-control edit-select">
                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <%-- Actions --%>
                            <asp:TemplateField HeaderText="Actions">
                                <HeaderStyle CssClass="col-actions text-center" />
                                <ItemStyle CssClass="col-actions text-center" />
                                <HeaderTemplate>
                                    <div class="header-action-group">
                                        <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="+ Add" CssClass="btn-add-primary" CausesValidation="false" />
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="action-btn-group">
                                        <asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit" ImageUrl="~/images/icons/edit.png" ToolTip="Edit Swatch" CssClass="btn-action-icon edit-icon" CausesValidation="false" />
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="action-btn-group">
                                        <asp:ImageButton ID="btnUpdate" runat="server" CommandName="Update" ImageUrl="~/images/icons/upload.png" ToolTip="Save Changes" CssClass="btn-action-icon update-icon" CausesValidation="false" />
                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/icons/cancel.png" ToolTip="Cancel" CssClass="btn-action-icon cancel-icon" CausesValidation="false" />
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="gridview-pager" />
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>

    <script type="text/javascript">
        function filterSwatchTable() {
            var input = document.getElementById("clientSearchInput");
            if (!input) return;
            var filter = input.value.toLowerCase();
            var grid = document.getElementById("<%= gvSwatches.ClientID %>");
            if (!grid) return;
            var rows = grid.getElementsByTagName("tr");

            for (var i = 1; i < rows.length; i++) {
                if (rows[i].classList.contains("gridview-pager")) continue;
                // Column index 2 is Swatch Name (0: SN, 1: Image, 2: Name)
                var nameCol = rows[i].getElementsByTagName("td")[2];
                if (nameCol) {
                    var nameValue = nameCol.textContent || nameCol.innerText;
                    rows[i].style.display = nameValue.toLowerCase().indexOf(filter) > -1 ? "" : "none";
                }
            }
        }
    </script>
</asp:Content>
