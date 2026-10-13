<%@ Page Title="Stage Management" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StageCreation.aspx.cs" Inherits="Swatch_StageCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="imsv5-container">

        <div class="imsv5-page-header">
            <div class="imsv5-title-block">
                <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                    <span>Swatch</span>
                    <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <span class="active">Stage Configuration</span>
                </nav>
                <h1 class="imsv5-page-title">Stage Management</h1>
                <p class="imsv5-page-subtitle">Manage stage icons, names, descriptions and status.</p>
            </div>
        </div>

        <div class="bs-card">
            <div class="card-toolbar">
                <div class="toolbar-stats">
                    <span class="badge-stat">Manage Stages</span>
                </div>
            </div>

            <div class="table-responsive">
                <asp:GridView ID="grdStage" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="enterprise-grid"
                    OnRowEditing="grdStage_RowEditing" OnRowUpdating="grdStage_RowUpdating"
                    OnRowCancelingEdit="grdStage_RowCancelingEdit" OnRowDeleting="grdStage_RowDeleting"
                    OnRowDataBound="grdStage_RowDataBound" OnPageIndexChanging="grdStage_PageIndexChanging"
                    AllowPaging="True" PageSize="20" EnableModelValidation="True" GridLines="None">
                    <Columns>

                        <asp:TemplateField HeaderText="SN">
                            <HeaderStyle CssClass="col-sn" />
                            <ItemStyle CssClass="text-center col-sn" />
                            <ItemTemplate>
                                <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                <asp:Label ID="lblPK_ID" runat="server" Text='<%# Eval("PK_ID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                <asp:Label ID="lblPK_ID" runat="server" Text='<%# Eval("PK_ID") %>' Visible="false"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Icon">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="header-label">Icon</span>
                                    <asp:FileUpload ID="fuIcon" runat="server" CssClass="form-control inline-input" accept="image/*" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                    <asp:Image ID="imgIcon" runat="server" ImageUrl='<%# GetStageIconUrl(Eval("ICON")) %>' Width="32px" Height="32px" AlternateText="Icon" />                            </ItemTemplate>
                            <EditItemTemplate>
                                    <asp:Image ID="imgIconE" runat="server" ImageUrl='<%# GetStageIconUrl(Eval("ICON")) %>' Width="32px" Height="32px" AlternateText="Icon" />                                <asp:FileUpload ID="fuIconE" runat="server" CssClass="form-control edit-input" accept="image/*" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Name">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name font-semibold" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="header-label">Name</span>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control inline-input" placeholder="Enter stage name..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("NAME") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtNameE" runat="server" Text='<%# Eval("NAME") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Description">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="header-label">Description</span>
                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control inline-input" placeholder="Enter description..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("DESCRIPTION") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescriptionE" runat="server" Text='<%# Eval("DESCRIPTION") %>' CssClass="form-control edit-input"></asp:TextBox>
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
                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="Edit" ToolTip="Edit Stage" CssClass="btn-action-icon edit-icon" />
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/images/icons/deletes.png" CommandName="Delete" ToolTip="Delete Stage" CssClass="btn-action-icon delete-icon" OnClientClick="return confirm('Are you sure you want to delete this stage?');" />
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="action-btn-group">
                                    <asp:ImageButton ID="btnUpdate" runat="server" CommandName="Update" ImageUrl="~/images/icons/upload.png" ToolTip="Save Changes" CssClass="btn-action-icon update-icon" />
                                    <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/icons/cancel.png" ToolTip="Cancel" CssClass="btn-action-icon cancel-icon" />
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>    

                    </Columns>
                    <PagerStyle CssClass="gridview-pager" />
                </asp:GridView>
            </div>
        </div>

    </div>
</asp:Content>