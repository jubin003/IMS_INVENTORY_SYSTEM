<%@ Page Title="Manage Urgency Levels" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Urgency_Level.aspx.cs" Inherits="Swatch_Urgency_Level" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="imsv5-container">
        <!-- Header -->
        <div class="imsv5-page-header">
            <div class="imsv5-title-block">
                <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                    <span>Production</span>
                    <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <span class="active">Urgency Configuration</span>
                </nav>
                <h1 class="imsv5-page-title">Urgency Level Management</h1>
                <p class="imsv5-page-subtitle">Configure urgency name, days, hex color, and status</p>
            </div>
        </div>

        <!-- Grid Card -->
        <div class="bs-card">
            <div class="card-toolbar">
                <div class="toolbar-stats">
                    <span class="badge-stat">Manage Urgency Levels</span>
                </div>

                <div class="search-box-wrapper">
                    <svg class="search-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <input type="text" id="clientSearchInput" class="table-search-input" placeholder="Search urgency levels..." onkeyup="filterUrgencyTable()" />
                </div>

                <asp:Label ID="lblMessage" runat="server" CssClass="badge-stat"></asp:Label>
            </div>

            <div class="table-responsive">
                <asp:GridView ID="gvUrgencyLevel" runat="server"
                    AutoGenerateColumns="false"
                    GridLines="None"
                    Width="100%"
                    CssClass="enterprise-grid"
                    DataKeyNames="PK_ID, STATUS"
                    OnRowEditing="gvUrgencyLevel_RowEditing"
                    OnRowCancelingEdit="gvUrgencyLevel_RowCancelingEdit"
                    OnRowUpdating="gvUrgencyLevel_RowUpdating"
                    OnRowDataBound="gvUrgencyLevel_RowDataBound">
                    <Columns>
                        <%-- Serial Number --%>
                        <asp:TemplateField HeaderText="SN">
                            <HeaderStyle CssClass="col-sn" />
                            <ItemStyle CssClass="text-center col-sn" />
                            <ItemTemplate>
                                <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- Urgency Name --%>
                        <asp:TemplateField HeaderText="Urgency Name">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name font-semibold" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="header-label">Urgency Name <span class="req">*</span></span>
                                    <asp:TextBox ID="txtUrgencyName" runat="server" CssClass="form-control inline-input" placeholder="Enter urgency name..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="cell-entity">
                                    <span class="entity-bullet"></span>
                                    <span><%# Eval("URGENCY_NAME") %></span>
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtUrgencyNameE" runat="server" Text='<%# Bind("URGENCY_NAME") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Days (Numeric Only) --%>
                        <asp:TemplateField HeaderText="Days">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name font-semibold" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="header-label">Days <span class="req">*</span></span>
                                    <asp:TextBox ID="txtDays" runat="server" type="number" min="0" step="1" onkeypress="return event.charCode >= 48 && event.charCode <= 57" CssClass="form-control inline-input" placeholder="Enter days..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span><%# Eval("DAYS") %></span>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDaysE" runat="server" Text='<%# Bind("DAYS") %>' type="number" min="0" step="1" onkeypress="return event.charCode >= 48 && event.charCode <= 57" CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <%-- Color (Combined Swatch Picker + Editable Hex Input) --%>
                        <asp:TemplateField HeaderText="Color">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name font-semibold" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="header-label">Color <span class="req">*</span></span>
                                    <div class="form-control inline-input" style="display: flex; align-items: center; gap: 8px; padding: 4px 8px;">
                                        <input type="color" value="#000000"
                                            style="width: 24px; height: 24px; border: none; padding: 0; background: none; cursor: pointer; flex-shrink: 0;"
                                            oninput="this.nextElementSibling.value = this.value;" />
                                        <asp:TextBox ID="txtColor" runat="server" Text="#000000" MaxLength="7" placeholder="#000000"
                                            Style="border: none; outline: none; background: transparent; width: 100%; font-family: monospace;"
                                            oninput="if(/^#[0-9A-Fa-f]{6}$/.test(this.value)) this.previousElementSibling.value = this.value;"></asp:TextBox>
                                    </div>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style="display: inline-flex; align-items: center; gap: 8px;">
                                    <span style='<%# "display:inline-block; width:22px; height:22px; border-radius:4px; border:1px solid #ccc; background-color:" + Eval("COLOR") + ";" %>'></span>
                                    <span style="font-family: monospace;"><%# Eval("COLOR") %></span>
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="form-control edit-input" style="display: flex; align-items: center; gap: 8px; padding: 4px 8px;">
                                    <input type="color" value='<%# Eval("COLOR") %>'
                                        style="width: 24px; height: 24px; border: none; padding: 0; background: none; cursor: pointer; flex-shrink: 0;"
                                        oninput="this.nextElementSibling.value = this.value;" />
                                    <asp:TextBox ID="txtColorE" runat="server" Text='<%# Bind("COLOR") %>' MaxLength="7" placeholder="#000000"
                                        Style="border: none; outline: none; background: transparent; width: 100%; font-family: monospace;"
                                        oninput="if(/^#[0-9A-Fa-f]{6}$/.test(this.value)) this.previousElementSibling.value = this.value;"></asp:TextBox>
                                </div>
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
                                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit" ImageUrl="~/images/icons/edit.png" ToolTip="Edit Urgency Level" CssClass="btn-action-icon edit-icon" CausesValidation="false" />
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
    </div>

    <script type="text/javascript">
        function filterUrgencyTable() {
            var input = document.getElementById("clientSearchInput");
            if (!input) return;
            var filter = input.value.toLowerCase();
            var grid = document.getElementById("<%= gvUrgencyLevel.ClientID %>");
            if (!grid) return;
            var rows = grid.getElementsByTagName("tr");

            for (var i = 1; i < rows.length; i++) {
                if (rows[i].classList.contains("gridview-pager")) continue;
                var nameCol = rows[i].getElementsByTagName("td")[1];
                var daysCol = rows[i].getElementsByTagName("td")[2];
                var colorCol = rows[i].getElementsByTagName("td")[3];
                if (nameCol && daysCol && colorCol) {
                    var rowText = (nameCol.textContent || nameCol.innerText) + " " +
                        (daysCol.textContent || daysCol.innerText) + " " +
                        (colorCol.textContent || colorCol.innerText);
                    rows[i].style.display = rowText.toLowerCase().indexOf(filter) > -1 ? "" : "none";
                }
            }
        }
    </script>
</asp:Content>