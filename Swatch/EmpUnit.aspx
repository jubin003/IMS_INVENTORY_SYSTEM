<%@ Page Title="Unit & Division" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EmpUnit.aspx.cs" Inherits="Swatch_EmpUnit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
                <p class="imsv5-page-subtitle">
                    Manage units, divisions and the employees mapped to each division.
                </p>
            </div>
        </div>

        <div class="page-tabs">
            <asp:Button ID="btnTabUnit"
                runat="server"
                Text="Unit"
                CssClass="page-tab active"
                CommandArgument="0"
                OnClick="btnTab_Click"
                CausesValidation="false" />

            <asp:Button ID="btnTabDivision"
                runat="server"
                Text="Division"
                CssClass="page-tab"
                CommandArgument="1"
                OnClick="btnTab_Click"
                CausesValidation="false" />

            <asp:Button ID="btnTabMap"
                runat="server"
                Text="Mapping"
                CssClass="page-tab"
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

                        <asp:GridView ID="gridUnit"
                            runat="server"
                            CssClass="enterprise-grid"
                            AutoGenerateColumns="False"
                            AllowPaging="True"
                            PageSize="20"
                            OnRowEditing="gridUnit_RowEditing"
                            OnRowUpdating="gridUnit_RowUpdating"
                            OnRowCancelingEdit="gridUnit_RowCancelingEdit"
                            OnRowDeleting="gridUnit_RowDeleting"
                            OnRowDataBound="gridUnit_RowDataBound"
                            OnPageIndexChanging="gridUnit_PageIndexChanging">

                            <Columns>

                                <asp:TemplateField HeaderText="SN" ItemStyle-CssClass="col-sn" HeaderStyle-CssClass="col-sn">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unit Name" ItemStyle-CssClass="col-name" HeaderStyle-CssClass="col-name">
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <asp:Label runat="server" Text="Unit Name" CssClass="header-label"></asp:Label>
                                            <asp:TextBox ID="txtUnitName" runat="server"
                                                CssClass="form-control inline-input"
                                                placeholder="Unit name..."></asp:TextBox>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <%# Eval("UNIT_NAME") %>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditUnitName" runat="server"
                                            Text='<%# Bind("UNIT_NAME") %>'
                                            CssClass="form-control edit-input">
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Head of Unit">
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <asp:Label runat="server" Text="Head of Unit" CssClass="header-label"></asp:Label>
                                            <asp:DropDownList ID="ddlHead" runat="server"
                                                CssClass="form-control inline-select">
                                            </asp:DropDownList>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <%# Eval("HEAD_NAME") %>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditHead" runat="server"
                                            CssClass="form-control edit-select">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <asp:Label runat="server" Text="Description" CssClass="header-label"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server"
                                                CssClass="form-control inline-input"
                                                placeholder="Description...">
                                            </asp:TextBox>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <%# Eval("DESCRIPTION") %>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditDescription" runat="server"
                                            Text='<%# Bind("DESCRIPTION") %>'
                                            CssClass="form-control edit-input">
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="col-status" HeaderStyle-CssClass="col-status">
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <asp:Label runat="server" Text="Status" CssClass="header-label"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server"
                                                CssClass="form-control inline-select">
                                                <asp:ListItem Text="-- Status --" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <%# Eval("STATUS") %>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditStatus" runat="server"
                                            CssClass="form-control edit-select">
                                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="col-actions" HeaderStyle-CssClass="col-actions">
                                    <HeaderTemplate>
                                        <div class="header-action-group">
                                            <asp:Button ID="btnAddUnit" runat="server"
                                                Text="Add"
                                                CssClass="btn-add-primary"
                                                CommandName="AddUnit"
                                                CausesValidation="false" />
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <div class="action-btn-group">
                                            <asp:ImageButton ID="btnEdit" runat="server"
                                                ImageUrl="~/images/edit.svg"
                                                CssClass="btn-action-icon"
                                                CommandName="Edit"
                                                CausesValidation="false" />

                                            <asp:ImageButton ID="btnDelete" runat="server"
                                                ImageUrl="~/images/delete.svg"
                                                CssClass="btn-action-icon"
                                                CommandName="Delete"
                                                CausesValidation="false" />
                                        </div>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <div class="action-btn-group">
                                            <asp:ImageButton ID="btnUpdate" runat="server"
                                                ImageUrl="~/images/save.svg"
                                                CssClass="btn-action-icon"
                                                CommandName="Update"
                                                CausesValidation="false" />

                                            <asp:ImageButton ID="btnCancel" runat="server"
                                                ImageUrl="~/images/cancel.svg"
                                                CssClass="btn-action-icon"
                                                CommandName="Cancel"
                                                CausesValidation="false" />
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                            </Columns>

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

                        <asp:GridView ID="gridDivision"
                            runat="server"
                            CssClass="enterprise-grid"
                            AutoGenerateColumns="False"
                            AllowPaging="True"
                            PageSize="20"
                            OnRowEditing="gridDivision_RowEditing"
                            OnRowUpdating="gridDivision_RowUpdating"
                            OnRowCancelingEdit="gridDivision_RowCancelingEdit"
                            OnRowDeleting="gridDivision_RowDeleting"
                            OnRowDataBound="gridDivision_RowDataBound"
                            OnPageIndexChanging="gridDivision_PageIndexChanging">

                            <Columns>

                                <asp:TemplateField HeaderText="SN" ItemStyle-CssClass="col-sn" HeaderStyle-CssClass="col-sn">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Unit">
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <asp:Label runat="server" Text="Unit" CssClass="header-label"></asp:Label>
                                            <asp:DropDownList ID="ddlUnit" runat="server"
                                                CssClass="form-control inline-select">
                                            </asp:DropDownList>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <%# Eval("UNIT_NAME") %>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditUnit" runat="server"
                                            CssClass="form-control edit-select">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Division Name" ItemStyle-CssClass="col-name" HeaderStyle-CssClass="col-name">
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <asp:Label runat="server" Text="Division Name" CssClass="header-label"></asp:Label>
                                            <asp:TextBox ID="txtDivisionName" runat="server"
                                                CssClass="form-control inline-input"
                                                placeholder="Division name...">
                                            </asp:TextBox>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <%# Eval("DIVISION_NAME") %>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditDivisionName" runat="server"
                                            Text='<%# Bind("DIVISION_NAME") %>'
                                            CssClass="form-control edit-input">
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <asp:Label runat="server" Text="Description" CssClass="header-label"></asp:Label>
                                            <asp:TextBox ID="TextBox1" runat="server"
                                                CssClass="form-control inline-input"
                                                placeholder="Description...">
                                            </asp:TextBox>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <%# Eval("DESCRIPTION") %>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server"
                                            Text='<%# Bind("DESCRIPTION") %>'
                                            CssClass="form-control edit-input">
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="col-status" HeaderStyle-CssClass="col-status">
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <asp:Label runat="server" Text="Status" CssClass="header-label"></asp:Label>
                                            <asp:DropDownList ID="DropDownList1" runat="server"
                                                CssClass="form-control inline-select">
                                                <asp:ListItem Text="-- Status --" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </asp:TemplateField>

                                    <ItemTemplate>
                                        <%# Eval("STATUS") %>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <asp:DropDownList ID="DropDownList2" runat="server"
                                            CssClass="form-control edit-select">
                                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="col-actions" HeaderStyle-CssClass="col-actions">
                                    <HeaderTemplate>
                                        <div class="header-action-group">
                                            <asp:Button ID="btnAddDivision" runat="server"
                                                Text="Add"
                                                CssClass="btn-add-primary"
                                                CommandName="AddDivision"
                                                CausesValidation="false" />
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <div class="action-btn-group">
                                            <asp:ImageButton ID="ImageButton1" runat="server"
                                                ImageUrl="~/images/edit.svg"
                                                CssClass="btn-action-icon"
                                                CommandName="Edit"
                                                CausesValidation="false" />

                                            <asp:ImageButton ID="ImageButton2" runat="server"
                                                ImageUrl="~/images/delete.svg"
                                                CssClass="btn-action-icon"
                                                CommandName="Delete"
                                                CausesValidation="false" />
                                        </div>
                                    </ItemTemplate>

                                    <EditItemTemplate>
                                        <div class="action-btn-group">
                                            <asp:ImageButton ID="ImageButton3" runat="server"
                                                ImageUrl="~/images/save.svg"
                                                CssClass="btn-action-icon"
                                                CommandName="Update"
                                                CausesValidation="false" />

                                            <asp:ImageButton ID="ImageButton4" runat="server"
                                                ImageUrl="~/images/cancel.svg"
                                                CssClass="btn-action-icon"
                                                CommandName="Cancel"
                                                CausesValidation="false" />
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                            </Columns>

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

                        <asp:GridView ID="gridMap"
                            runat="server"
                            CssClass="enterprise-grid"
                            AutoGenerateColumns="False"
                            AllowPaging="True"
                            PageSize="20"
                            OnRowDeleting="gridMap_RowDeleting"
                            OnRowDataBound="gridMap_RowDataBound"
                            OnPageIndexChanging="gridMap_PageIndexChanging">

                            <Columns>

                                <asp:TemplateField HeaderText="SN" ItemStyle-CssClass="col-sn" HeaderStyle-CssClass="col-sn">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Division">
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <asp:Label runat="server" Text="Division" CssClass="header-label"></asp:Label>
                                            <asp:DropDownList ID="ddlDivision" runat="server"
                                                CssClass="form-control inline-select">
                                            </asp:DropDownList>
                                        </div>
                                    </asp:TemplateField>

                                    <ItemTemplate>
                                        <%# Eval("DIVISION_NAME") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Employee">
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <asp:Label runat="server" Text="Employee" CssClass="header-label"></asp:Label>
                                            <asp:DropDownList ID="ddlEmployee" runat="server"
                                                CssClass="form-control inline-select">
                                            </asp:DropDownList>
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <%# Eval("EMPLOYEE_NAME") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="col-actions" HeaderStyle-CssClass="col-actions">
                                    <HeaderTemplate>
                                        <div class="header-action-group">
                                            <asp:Button ID="btnAddMap" runat="server"
                                                Text="Add"
                                                CssClass="btn-add-primary"
                                                CommandName="AddMapping"
                                                CausesValidation="false" />
                                        </div>
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <div class="action-btn-group">
                                            <asp:ImageButton ID="ImageButton5" runat="server"
                                                ImageUrl="~/images/delete.svg"
                                                CssClass="btn-action-icon"
                                                CommandName="Delete"
                                                CausesValidation="false" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>

                        </asp:GridView>

                    </div>
                </div>

            </asp:View>

        </asp:MultiView>

    </div>

</asp:Content>