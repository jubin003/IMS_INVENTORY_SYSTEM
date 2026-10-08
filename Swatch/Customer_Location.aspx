<%@ Page Title="Customer Location Management" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Customer_Location.aspx.cs" Inherits="Production_Customer_Location" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="imsv5-container">

        <div class="imsv5-page-header">
            <div class="imsv5-title-block">
                <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                    <span>Customer</span>
                    <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <span class="active">Customer Location Configuration</span>
                </nav>
                <h1 class="imsv5-page-title">Customer Location Management</h1>
                <p class="imsv5-page-subtitle">Manage customer addresses, contact numbers, countries and email information.</p>
            </div>
        </div>

        <div class="bs-card">
            <div class="card-toolbar">
                <%--<div class="search-box-wrapper">
                    <svg class="search-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <input type="text" id="clientSearchInput" class="form-control table-search-input" placeholder="Search customer location..." onkeyup="filterCustomerLocationTable()" />
                </div>--%>
                <div class="toolbar-stats">
                    <span class="badge-stat">Manage Customer Locations</span>
                </div>
            </div>

            <div class="table-responsive">
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

                        <asp:TemplateField HeaderText="Customer">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name font-semibold" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="header-label">Customer</span>
                                    <asp:DropDownList ID="ddlCustomerH" runat="server" CssClass="form-control inline-select"></asp:DropDownList>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("CUSTOMER_ID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control edit-select"></asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Contact Person">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name font-semibold" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="header-label">Contact Person</span>
                                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control inline-select"></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblContactPerson" runat="server" Text='<%# Bind("CONTACT_PERSON") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtContactPersonE" runat="server" CssClass="form-control edit-select"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>




                        <asp:TemplateField HeaderText="Address">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-address')">Address <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control inline-input" placeholder="Enter address..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("ADDRESS") %>' CssClass="sort-address"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAddressE" runat="server" Text='<%# Bind("ADDRESS") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Country">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="header-label">Country</span>
                                    <asp:DropDownList ID="ddlCountryH" runat="server" CssClass="form-control inline-select"></asp:DropDownList>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCountryID" runat="server" Text='<%# Bind("COUNTRY_ID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblCountry" runat="server" CssClass="sort-country"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control edit-select"></asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Contact Number">
                            <HeaderStyle CssClass="col-code" />
                            <ItemStyle CssClass="col-code" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-contact')">Contact Number <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                    <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control inline-input" placeholder="Enter contact number..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblContactNumber" runat="server" Text='<%# Bind("CONTACT_NUMBER") %>' CssClass="sort-contact"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtContactNumberE" runat="server" Text='<%# Bind("CONTACT_NUMBER") %>' CssClass="form-control edit-input"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Email">
                            <HeaderStyle CssClass="col-name" />
                            <ItemStyle CssClass="col-name" />
                            <HeaderTemplate>
                                <div class="header-field-group">
                                    <span class="sortable-header" onclick="sortTableColumn(this, '.sort-email')">Email <i class="fa fa-chevron-up sort-icon" style="display:none;"></i></span>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control inline-input" placeholder="Enter email..."></asp:TextBox>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("EMAIL_ID") %>' CssClass="sort-email"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEmailE" runat="server" Text='<%# Bind("EMAIL_ID") %>' CssClass="form-control edit-input"></asp:TextBox>
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
                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="edit" ToolTip="Edit Customer Location" CssClass="btn-action-icon edit-icon" />
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="action-btn-group">
                                    <asp:ImageButton ID="btnUpdate" runat="server" CommandName="update" ImageUrl="~/images/icons/upload.png" ToolTip="Save Changes" CssClass="btn-action-icon update-icon" />
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
        function filterCustomerLocationTable() {
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