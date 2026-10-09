<%@ Page Title="Swatch Type Management" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Swatch_Type.aspx.cs" Inherits="Production_Swatch_Type" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="imsv5-container">

        <div class="imsv5-page-header">
            <div class="imsv5-title-block">
                <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                    <span>Production</span>
                    <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <span class="active">Swatch Type Configuration</span>
                </nav>
                <h1 class="imsv5-page-title">Swatch Type Management</h1>
                <p class="imsv5-page-subtitle">Configure swatch types, codes, remarks and operational status.</p>
            </div>
        </div>

        <div class="bs-card">
            <div class="card-toolbar">
                <div class="toolbar-stats">
                    <span class="badge-stat">Manage Swatch Types</span>
                </div>
            </div>

            <div class="    ">
                <asp:GridView ID="gridDisplay" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="enterprise-grid" OnRowEditing="gridDisplay_RowEditing" OnRowUpdating="gridDisplay_RowUpdating" OnRowCancelingEdit="gridDisplay_RowCancelingEdit" AllowPaging="True" PageSize="20" EnableModelValidation="True" OnRowDataBound="gridDisplay_RowDataBound" GridLines="None">
                    <Columns>

                        <asp:TemplateField HeaderText="SN">
                            <HeaderStyle CssClass="col-sn" />
                            <ItemStyle CssClass="text-center col-sn" />
                            <HeaderTemplate>
                                <span class="sortable-header" onclick="sortTableColumn(this, '.sort-sn')">SN <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="row-sn-text sort-sn"><%# Container.DataItemIndex + 1 %></span>
                                <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Swatch Name">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name font-semibold" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-swatch-name')">Swatch Name <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                    <asp:TextBox ID="txtSwatchName" runat="server" CssClass="form-control inline-input" placeholder="Enter swatch name..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="cell-entity sort-swatch-name">
                                    <span class="entity-bullet"></span>
                                    <asp:Label ID="lblSwatchName" runat="server" Text='<%# Bind("Swatch_Name") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Swatch_Name") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Swatch Code">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-swatch-code')">Swatch Code <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                    <asp:TextBox ID="txtSwatchCode" runat="server" CssClass="form-control inline-input" placeholder="Enter code..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSwatchCode" runat="server" Text='<%# Bind("Swatch_Code") %>' CssClass="sort-swatch-code"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Swatch_Code") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Remarks">
                            <HeaderStyle CssClass="col-remarks" />
                            <ItemStyle CssClass="col-remarks" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="header-label">Remarks</span>
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control inline-input" placeholder="Enter remarks..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Remarks") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Status">
                            <HeaderStyle CssClass="col-status" />
                            <ItemStyle CssClass="col-status" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="header-label">Status</span>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control inline-select">
                                        <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblStatusShow" runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Status") %>' Visible="false"></asp:Label>
                                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control edit-select">
                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle CssClass="col-actions text-center" />
                            <ItemStyle CssClass="col-actions text-center" />
                            <HeaderTemplate>
                                <div class="header-action-group">
                                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="+ Add" CssClass="btn-add-primary" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="action-btn-group">
                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="edit" ToolTip="Edit Swatch Type" CssClass="btn-action-icon edit-icon" />
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="action-btn-group">
                                    <asp:ImageButton ID="ImageButton1" runat="server" CommandName="update" ImageUrl="~/images/icons/upload.png" ToolTip="Save Changes" CssClass="btn-action-icon update-icon" />
                                    <asp:ImageButton ID="btnCancel" runat="server" CommandName="cancel" ImageUrl="~/images/icons/cancel.png" ToolTip="Cancel" CssClass="btn-action-icon cancel-icon" />
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
        function filterSwatchTable() {
            var input = document.getElementById("clientSearchInput");
            var filter = input.value.toLowerCase();
            var grid = document.getElementById("<%= gridDisplay.ClientID %>");
            if (!grid) return;
            var rows = grid.getElementsByTagName("tr");
            for (var i = 1; i < rows.length; i++) {
                if (rows[i].classList.contains("gridview-pager")) continue;
                var rowText = rows[i].textContent || rows[i].innerText;
                rows[i].style.display = rowText.toLowerCase().indexOf(filter) > -1 ? "" : "none";
            }
        }
    </script>

</asp:Content>