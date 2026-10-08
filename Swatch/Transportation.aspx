<%@ Page Title="Transportation Management" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Transportation.aspx.cs" Inherits="Swatch_Transportation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
        .compact-page { max-width: 760px; }

        .compact-page .enterprise-grid th,
        .compact-page .enterprise-grid td {
            padding: 6px 12px;
            font-size: 13px;
            vertical-align: middle;
        }

        .compact-page .enterprise-grid .form-control {
            height: 32px;
            padding: 4px 8px;
            font-size: 13px;
        }

        .compact-page .enterprise-grid .col-sn { width: 60px; }
        .compact-page .enterprise-grid .col-status { width: 130px; }
        .compact-page .enterprise-grid .col-actions { width: 120px; }

        .compact-page .btn-action-icon { width: 18px; height: 18px; }
        .compact-page .btn-add-primary { padding: 5px 12px; font-size: 13px; }
        .compact-page .card-toolbar { padding: 10px 14px; }
    </style>

    <div class="imsv5-container">

        <div class="imsv5-page-header">
            <div class="imsv5-title-block">
                <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                    <span>Swatch</span>
                    <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <span class="active">Transportation Configuration</span>
                </nav>
                <h1 class="imsv5-page-title">Transportation Management</h1>
                <p class="imsv5-page-subtitle">Manage modes of transportation.</p>
            </div>
        </div>

        <div class="compact-page">
            <div class="bs-card">
                <div class="card-toolbar">
                    <div class="toolbar-stats">
                        <span class="badge-stat">Manage Transportation</span>
                    </div>
                </div>

                <div class="table-responsive">
                    <asp:GridView ID="gridDisplay" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="enterprise-grid"
                        OnRowEditing="gridDisplay_RowEditing" OnRowUpdating="gridDisplay_RowUpdating"
                        OnRowCancelingEdit="gridDisplay_RowCancelingEdit" OnRowDeleting="gridDisplay_RowDeleting"
                        OnRowDataBound="gridDisplay_RowDataBound" OnPageIndexChanging="gridDisplay_PageIndexChanging"
                        AllowPaging="True" PageSize="20" EnableModelValidation="True" GridLines="None">
                        <Columns>

                            <asp:TemplateField HeaderText="SN">
                                <HeaderStyle CssClass="col-sn" />
                                <ItemStyle CssClass="text-center col-sn" />
                                <HeaderTemplate>
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-sn')">SN <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="row-sn-text sort-sn"><%# Container.DataItemIndex + 1 %></span>
                                    <asp:Label ID="lblPK_ID" runat="server" Text='<%# Eval("PK_ID") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span class="row-sn-text sort-sn"><%# Container.DataItemIndex + 1 %></span>
                                    <asp:Label ID="lblPK_ID" runat="server" Text='<%# Eval("PK_ID") %>' Visible="false"></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Transportation">
                                <HeaderStyle CssClass="col-name" />
                                <ItemStyle CssClass="col-name font-semibold" />
                                <HeaderTemplate>
                                    <div class="header-field-group">
                                        <span class="sortable-header" onclick="sortTableColumn(this, '.sort-transportation')">Transportation <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                        <asp:TextBox ID="txtTransportation" runat="server" CssClass="form-control inline-input" placeholder="Enter transportation..."></asp:TextBox>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTransportation" runat="server" Text='<%# Eval("TRANSPORTATION_NAME") %>' CssClass="sort-transportation"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTransportationE" runat="server" Text='<%# Eval("TRANSPORTATION_NAME") %>' CssClass="form-control edit-input"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status">
                                <HeaderStyle CssClass="col-status" />
                                <ItemStyle CssClass="col-status" />
                                <HeaderTemplate>
                                    <div class="header-field-group">
                                        <span class="header-label">Status</span>
                                        <asp:DropDownList ID="ddlStatusH" runat="server" CssClass="form-control inline-select">
                                            <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("STATUS") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblStatusShow" runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblStatusE" runat="server" Text='<%# Eval("STATUS") %>' Visible="false"></asp:Label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control edit-select">
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
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="edit" ToolTip="Edit Transportation" CssClass="btn-action-icon edit-icon" />
                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/images/icons/delete.png" CommandName="delete" ToolTip="Delete Transportation" CssClass="btn-action-icon delete-icon" OnClientClick="return confirm('Are you sure you want to delete this transportation?');" />
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="action-btn-group">
                                        <asp:ImageButton ID="btnUpdate" runat="server" CommandName="update" ImageUrl="~/images/icons/upload.png" ToolTip="Save Changes" CssClass="btn-action-icon update-icon" />
                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="cancel" ImageUrl="~/images/icons/cancel.png" ToolTip="Cancel" CssClass="btn-action-icon cancel-icon" />
                                        <asp:ImageButton ID="btnDeleteE" runat="server" ImageUrl="~/images/icons/delete.png" CommandName="delete" ToolTip="Delete Transportation" CssClass="btn-action-icon delete-icon" OnClientClick="return confirm('Are you sure you want to delete this transportation?');" />
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <PagerStyle CssClass="gridview-pager" />
                    </asp:GridView>
                </div>
            </div>
        </div>

    </div>
</asp:Content>