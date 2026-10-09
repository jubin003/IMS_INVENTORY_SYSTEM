<%@ Page Title="Unit & Division" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EmpUnit.aspx.cs" Inherits="Swatch_EmpUnit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="imsv5-container">

        <div class="imsv5-page-header">
            <div class="imsv5-title-block">
                <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                    <span>Swatch</span>
                    <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <span class="active">Unit &amp; Division Configuration</span>
                </nav>
                <h1 class="imsv5-page-title">Unit &amp; Division Management</h1>
                <p class="imsv5-page-subtitle">Manage units, divisions and the employees mapped to each division.</p>
            </div>
        </div>

        <div class="page-tabs">

            <asp:Button ID="btnTabUnit" runat="server" Text="Unit" CssClass="btn-view-primary active" CommandArgument="0" OnClick="btnTab_Click" CausesValidation="false" />

            <asp:Button ID="btnTabDivision"
                runat="server"
                Text="Division"
                CssClass="btn-view-primary"
                CommandArgument="1"
                OnClick="btnTab_Click"
                CausesValidation="false" />

            <asp:Button ID="btnTabMap"
                runat="server"
                Text="Mapping"
                CssClass="btn-view-primary"
                CommandArgument="2"
                OnClick="btnTab_Click"
                CausesValidation="false" />
        </div>


        <asp:MultiView ID="mvEmp" runat="server" ActiveViewIndex="0">

            <asp:View ID="vUnit" runat="server">

                <div class="bs-card">
                    <div class="card-toolbar">
                        <div class="toolbar-stats">
                            <span class="badge-stat">Manage Units</span>
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gridUnit" runat="server"
                            Width="100%"
                            AutoGenerateColumns="False"
                            CssClass="enterprise-grid"
                            OnRowEditing="gridUnit_RowEditing"
                            OnRowUpdating="gridUnit_RowUpdating"
                            OnRowCancelingEdit="gridUnit_RowCancelingEdit"
                            OnRowDeleting="gridUnit_RowDeleting"
                            OnRowDataBound="gridUnit_RowDataBound"
                            OnPageIndexChanging="gridUnit_PageIndexChanging"
                            AllowPaging="True"
                            PageSize="20"
                            EnableModelValidation="True"
                            GridLines="None">

                            <Columns>

                                <asp:TemplateField HeaderText="SN">
                                    <HeaderStyle CssClass="col-sn" />
                                    <ItemStyle CssClass="text-center col-sn" />

                                    <ItemTemplate>
                                        <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                        <asp:Label ID="lblPK_ID" runat="server"
                                            Text='<%# Eval("PK_ID") %>'
                                            Visible="false">
                                        </asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                        <asp:Label ID="Label1" runat="server"
                                            Text='<%# Eval("PK_ID") %>'
                                            Visible="false">
                                        </asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unit Name">
                                    <HeaderStyle CssClass="col-name" />
                                    <ItemStyle CssClass="col-name font-semibold" />

                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">Unit Name</span>
                                            <asp:TextBox ID="txtUnitName" runat="server"
                                                CssClass="form-control inline-input"
                                                placeholder="Enter unit name...">
                                            </asp:TextBox>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitName" runat="server"
                                            Text='<%# Eval("UNIT_NAME") %>'>
                                        </asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtUnitNameE" runat="server"
                                            Text='<%# Eval("UNIT_NAME") %>'
                                            CssClass="form-control edit-input">
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Head of Unit">
                                    <HeaderStyle CssClass="col-name" />
                                    <ItemStyle CssClass="col-name" />

                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">Head of Unit</span>
                                            <asp:DropDownList ID="ddlHeadOfUnitH" runat="server"
                                                CssClass="form-control inline-select">
                                            </asp:DropDownList>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="lblHeadOfUnitID" runat="server"
                                            Text='<%# Eval("HEAD_OF_UNIT") %>'
                                            Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblHeadOfUnit" runat="server"></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:Label ID="lblHeadOfUnitIDE" runat="server"
                                            Text='<%# Eval("HEAD_OF_UNIT") %>'
                                            Visible="false">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlHeadOfUnit" runat="server"
                                            CssClass="form-control edit-select">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <HeaderStyle CssClass="col-name" />
                                    <ItemStyle CssClass="col-name" />

                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">Description</span>
                                            <asp:TextBox ID="txtDescription" runat="server"
                                                CssClass="form-control inline-input"
                                                placeholder="Enter description...">
                                            </asp:TextBox>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server"
                                            Text='<%# Eval("DESCRIPTION") %>'>
                                        </asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescriptionE" runat="server"
                                            Text='<%# Eval("DESCRIPTION") %>'
                                            CssClass="form-control edit-input">
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Status">
                                    <HeaderStyle CssClass="col-status" />
                                    <ItemStyle CssClass="col-status" />

                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">Status</span>
                                            <asp:DropDownList ID="ddlStatusH" runat="server"
                                                CssClass="form-control inline-select">
                                                <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server"
                                            Text='<%# Eval("STATUS") %>'
                                            Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblStatusShow" runat="server"></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:Label ID="lblStatusE" runat="server"
                                            Text='<%# Eval("STATUS") %>'
                                            Visible="false">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlStatus" runat="server"
                                            CssClass="form-control edit-select">
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
                                            <asp:Button ID="btnAddUnit"
                                                runat="server"
                                                OnClick="btnAddUnit_Click"
                                                Text="+ Add"
                                                CssClass="btn-add-primary" />
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <div class="action-btn-group">
                                            <asp:ImageButton ID="btnEdit"
                                                runat="server"
                                                ImageUrl="~/images/icons/edit.png"
                                                CommandName="edit"
                                                ToolTip="Edit Unit"
                                                CssClass="btn-action-icon edit-icon" />

                                            <asp:ImageButton ID="btnDelete"
                                                runat="server"
                                                ImageUrl="~/images/icons/deletes.png"
                                                CommandName="delete"
                                                ToolTip="Delete Unit"
                                                CssClass="btn-action-icon delete-icon"
                                                OnClientClick="return confirm('Are you sure you want to delete this unit?');" />
                                        </div>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <div class="action-btn-group">
                                            <asp:ImageButton ID="btnUpdate"
                                                runat="server"
                                                CommandName="update"
                                                ImageUrl="~/images/icons/upload.png"
                                                ToolTip="Save Changes"
                                                CssClass="btn-action-icon update-icon" />

                                            <asp:ImageButton ID="btnCancel"
                                                runat="server"
                                                CommandName="cancel"
                                                ImageUrl="~/images/icons/cancel.png"
                                                ToolTip="Cancel"
                                                CssClass="btn-action-icon cancel-icon" />
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                            </Columns>

                            <PagerStyle CssClass="gridview-pager" />

                        </asp:GridView>
                    </div>
                </div>

            </asp:View>

            <asp:View ID="vDivision" runat="server">

                <div class="bs-card">
                    <div class="card-toolbar">
                        <div class="toolbar-stats">
                            <span class="badge-stat">Manage Divisions</span>
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gridDivision" runat="server"
                            Width="100%"
                            AutoGenerateColumns="False"
                            CssClass="enterprise-grid"
                            OnRowEditing="gridDivision_RowEditing"
                            OnRowUpdating="gridDivision_RowUpdating"
                            OnRowCancelingEdit="gridDivision_RowCancelingEdit"
                            OnRowDeleting="gridDivision_RowDeleting"
                            OnRowDataBound="gridDivision_RowDataBound"
                            OnPageIndexChanging="gridDivision_PageIndexChanging"
                            AllowPaging="True"
                            PageSize="20"
                            EnableModelValidation="True"
                            GridLines="None">

                            <Columns>

                                <asp:TemplateField HeaderText="SN">
                                    <HeaderStyle CssClass="col-sn" />
                                    <ItemStyle CssClass="text-center col-sn" />

                                    <ItemTemplate>
                                        <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                        <asp:Label ID="Label2" runat="server"
                                            Text='<%# Eval("PK_ID") %>'
                                            Visible="false">
                                        </asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                        <asp:Label ID="Label3" runat="server"
                                            Text='<%# Eval("PK_ID") %>'
                                            Visible="false">
                                        </asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unit">
                                    <HeaderStyle CssClass="col-name" />
                                    <ItemStyle CssClass="col-name font-semibold" />

                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">Unit</span>
                                            <asp:DropDownList ID="ddlUnitH" runat="server"
                                                CssClass="form-control inline-select">
                                            </asp:DropDownList>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitID" runat="server"
                                            Text='<%# Eval("UNIT_ID") %>'
                                            Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblUnit" runat="server"></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:Label ID="lblUnitIDE" runat="server"
                                            Text='<%# Eval("UNIT_ID") %>'
                                            Visible="false">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlUnit" runat="server"
                                            CssClass="form-control edit-select">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Division Name">
                                    <HeaderStyle CssClass="col-name" />
                                    <ItemStyle CssClass="col-name" />

                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">Division Name</span>
                                            <asp:TextBox ID="txtDivisionName" runat="server"
                                                CssClass="form-control inline-input"
                                                placeholder="Enter division name...">
                                            </asp:TextBox>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="lblDivisionName" runat="server"
                                            Text='<%# Eval("DIVISION_NAME") %>'>
                                        </asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDivisionNameE" runat="server"
                                            Text='<%# Eval("DIVISION_NAME") %>'
                                            CssClass="form-control edit-input">
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <HeaderStyle CssClass="col-name" />
                                    <ItemStyle CssClass="col-name" />

                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">Description</span>
                                            <asp:TextBox ID="TextBox1" runat="server"
                                                CssClass="form-control inline-input"
                                                placeholder="Enter description...">
                                            </asp:TextBox>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server"
                                            Text='<%# Eval("DESCRIPTION") %>'>
                                        </asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server"
                                            Text='<%# Eval("DESCRIPTION") %>'
                                            CssClass="form-control edit-input">
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Status">
                                    <HeaderStyle CssClass="col-status" />
                                    <ItemStyle CssClass="col-status" />

                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">Status</span>
                                            <asp:DropDownList ID="DropDownList1" runat="server"
                                                CssClass="form-control inline-select">
                                                <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server"
                                            Text='<%# Eval("STATUS") %>'
                                            Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="Label6" runat="server"></asp:Label>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:Label ID="Label7" runat="server"
                                            Text='<%# Eval("STATUS") %>'
                                            Visible="false">
                                        </asp:Label>
                                        <asp:DropDownList ID="DropDownList2" runat="server"
                                            CssClass="form-control edit-select">
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
                                            <asp:Button ID="btnAddDivision"
                                                runat="server"
                                                OnClick="btnAddDivision_Click"
                                                Text="+ Add"
                                                CssClass="btn-add-primary" />
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <div class="action-btn-group">
                                            <asp:ImageButton ID="ImageButton1"
                                                runat="server"
                                                ImageUrl="~/images/icons/edit.png"
                                                CommandName="edit"
                                                ToolTip="Edit Division"
                                                CssClass="btn-action-icon edit-icon" />

                                            <asp:ImageButton ID="ImageButton2"
                                                runat="server"
                                                ImageUrl="~/images/icons/deletes.png"
                                                CommandName="delete"
                                                ToolTip="Delete Division"
                                                CssClass="btn-action-icon delete-icon"
                                                OnClientClick="return confirm('Are you sure you want to delete this division?');" />
                                        </div>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <div class="action-btn-group">
                                            <asp:ImageButton ID="ImageButton3"
                                                runat="server"
                                                CommandName="update"
                                                ImageUrl="~/images/icons/upload.png"
                                                ToolTip="Save Changes"
                                                CssClass="btn-action-icon update-icon" />

                                            <asp:ImageButton ID="ImageButton4"
                                                runat="server"
                                                CommandName="cancel"
                                                ImageUrl="~/images/icons/cancel.png"
                                                ToolTip="Cancel"
                                                CssClass="btn-action-icon cancel-icon" />
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                            </Columns>

                            <PagerStyle CssClass="gridview-pager" />

                        </asp:GridView>
                    </div>
                </div>

            </asp:View>

            <asp:View ID="vMap" runat="server">

                <div class="bs-card">
                    <div class="card-toolbar">
                        <div class="toolbar-stats">
                            <span class="badge-stat">Division &amp; Employee Mapping</span>
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gridMap" runat="server"
                            Width="100%"
                            AutoGenerateColumns="False"
                            CssClass="enterprise-grid"
                            OnRowDeleting="gridMap_RowDeleting"
                            OnRowDataBound="gridMap_RowDataBound"
                            OnPageIndexChanging="gridMap_PageIndexChanging"
                            AllowPaging="True"
                            PageSize="20"
                            EnableModelValidation="True"
                            GridLines="None">

                            <Columns>

                                <asp:TemplateField HeaderText="SN">
                                    <HeaderStyle CssClass="col-sn" />
                                    <ItemStyle CssClass="text-center col-sn" />

                                    <ItemTemplate>
                                        <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                        <asp:Label ID="Label8" runat="server"
                                            Text='<%# Eval("PK_ID") %>'
                                            Visible="false">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Division">
                                    <HeaderStyle CssClass="col-name" />
                                    <ItemStyle CssClass="col-name font-semibold" />

                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">Division</span>
                                            <asp:DropDownList ID="ddlMapDivisionH" runat="server"
                                                CssClass="form-control inline-select">
                                            </asp:DropDownList>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="lblDivisionID" runat="server"
                                            Text='<%# Eval("DIVISION_ID") %>'
                                            Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Employee">
                                    <HeaderStyle CssClass="col-name" />
                                    <ItemStyle CssClass="col-name" />

                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">Employee</span>
                                            <asp:DropDownList ID="ddlMapEmployeeH" runat="server"
                                                CssClass="form-control inline-select">
                                            </asp:DropDownList>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="lblEmployeeID" runat="server"
                                            Text='<%# Eval("EMPLOYEE_ID") %>'
                                            Visible="false">
                                        </asp:Label>
                                        <asp:Label ID="lblEmployee" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderStyle CssClass="col-actions text-center" />
                                    <ItemStyle CssClass="col-actions text-center" />

                                    <HeaderTemplate>
                                        <div class="header-action-group">
                                            <asp:Button ID="btnMap"
                                                runat="server"
                                                OnClick="btnMap_Click"
                                                Text="Map"
                                                CssClass="btn-add-primary" />
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <div class="action-btn-group">

                                            <asp:ImageButton ID="btnEdit"
                                                runat="server"
                                                ImageUrl="~/images/icons/edit.png"
                                                CommandName="edit"
                                                ToolTip="Remove Mapping"
                                                CssClass="btn-action-icon edit-icon" />

                                            <asp:ImageButton ID="btnDelete"
                                                runat="server"
                                                ImageUrl="~/images/icons/deletes.png"
                                                CommandName="delete"
                                                ToolTip="Remove Mapping"
                                                CssClass="btn-action-icon deletes-icon"
                                                OnClientClick="return confirm('Are you sure you want to remove this mapping?');" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>

                            <PagerStyle CssClass="gridview-pager" />

                        </asp:GridView>
                    </div>
                </div>

            </asp:View>

        </asp:MultiView>

    </div>

</asp:Content>
