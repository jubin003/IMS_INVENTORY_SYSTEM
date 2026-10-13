<%@ Page Title="Customer Master" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="administration_customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <style type="text/css">
        .radio-inline-list label {
            margin-right: 15px;
            font-weight: 500;
            color: #334155;
            cursor: pointer;
        }

        .radio-inline-list input[type="radio"] {
            margin-right: 5px;
            vertical-align: middle;
        }

        /* Scrollable container for delivery locations */
        .del-loc-container::-webkit-scrollbar {
            width: 4px;
        }
        .del-loc-container::-webkit-scrollbar-track {
            background: #f1f5f9;
            border-radius: 4px;
        }
        .del-loc-container::-webkit-scrollbar-thumb {
            background: #cbd5e1;
            border-radius: 4px;
        }
        .del-loc-container::-webkit-scrollbar-thumb:hover {
            background: #94a3b8;
        }

        /* Customer Avatar */
        .customer-avatar {
            width: 44px;
            height: 44px;
            border-radius: 50%;
            object-fit: cover;
            border: 1px solid #cbd5e1;
            flex-shrink: 0;
            background-color: #ffffff;
        }

        .customer-avatar-table {
            width: 38px;
            height: 38px;
            border-radius: 50%;
            object-fit: cover;
            border: 1px solid #e2e8f0;
            flex-shrink: 0;
            background-color: #ffffff;
        }
    </style>

    <div class="imsv5-container">
        <!-- Page Header -->
        <div class="imsv5-page-header">
            <div>
                <div class="imsv5-breadcrumb">
                    <span>Administration</span>
                    <svg class="imsv5-icon-arrow" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <polyline points="9 18 15 12 9 6"></polyline>
                    </svg>
                    <span class="active">Customer Management</span>
                </div>
                <h1 class="imsv5-page-title">Customer Master</h1>
                <p class="imsv5-page-subtitle">Manage client accounts, photos, contact information, and delivery addresses</p>
            </div>
        </div>

        <!-- Add / Edit Form Card -->
        <div runat="server" id="divAdd" visible="false" class="bs-card">
            <div class="card-toolbar">
                <span class="filter-title">
                    <span class="filter-dot"></span> Customer Details
                </span>
            </div>

            <!-- Branch Section -->
            <div id="trBranch" runat="server" class="form-grid form-grid-3" style="padding-bottom: 0;">
                <div class="form-group form-span-2">
                    <label>Branch <span class="req">*</span></label>
                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>

            <!-- General Info Section with Picture Upload -->
            <div class="form-section">
                <div class="form-section-title">General Information</div>
                <div class="form-grid form-grid-3">
                    <div class="form-group">
                        <label>Customer Code <span class="req">*</span></label>
                        <asp:TextBox ID="txtCustomerCode" CssClass="form-control" runat="server" MaxLength="8" OnTextChanged="txtCustomerCode_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Customer Name <span class="req">*</span></label>
                        <asp:TextBox ID="txtCustomerName" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Country</label>
                        <asp:DropDownList ID="ddlCountry" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>

                    <div class="form-group form-span-2">
                        <label>Address</label>
                        <asp:TextBox ID="txtAddress" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                    <!-- Profile Picture Upload Field -->
                    <div class="form-group">
                        <label>Profile Picture</label>
                        <div style="display: flex; align-items: center; gap: 10px;">
                            <asp:Image ID="imgCustomerPreview" runat="server" ImageUrl="~/images/icons/user.png" CssClass="customer-avatar" />
                            <div style="flex: 1;">
                                <asp:FileUpload ID="fuCustomerPhoto" runat="server" CssClass="form-control" Style="height: auto; padding: 4px; font-size: 11.5px;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Tax & Registration Section -->
            <div class="form-section">
                <div class="form-section-title">Tax & Registration</div>
                <div class="form-grid form-grid-4">
                    <div class="form-group">
                        <label>Register In</label>
                        <asp:RadioButtonList ID="rbtnPAN_VAT" runat="server" RepeatDirection="Horizontal" CssClass="radio-inline-list">
                            <asp:ListItem Selected="True">PAN</asp:ListItem>
                            <asp:ListItem>VAT</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>

                    <div class="form-group">
                        <label>PAN / VAT No</label>
                        <asp:TextBox ID="txtPANNo" runat="server" CssClass="form-control" TextMode="Number" Text="0" MaxLength="9"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Registration No</label>
                        <asp:TextBox ID="txtRegNo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Join Date (BS)</label>
                        <asp:TextBox ID="txtJoinDate" CssClass="form-control datepicker" AutoComplete="off" runat="server" AutoPostBack="true" OnTextChanged="txtJoinDate_TextChanged"></asp:TextBox>
                    </div>
                </div>
            </div>

            <!-- Contact & Classification Section -->
            <div class="form-section">
                <div class="form-section-title">Contact & Classification</div>
                <div class="form-grid form-grid-4">
                    <div class="form-group">
                        <label>Contact Person</label>
                        <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Phone No</label>
                        <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Mobile No</label>
                        <asp:TextBox ID="txtMobileNo" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Email</label>
                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Area</label>
                        <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label>Agent</label>
                        <asp:DropDownList ID="ddlAgent" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label>Credit Limit</label>
                        <asp:TextBox runat="server" ID="txtCreditLimit" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label>Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Value="1">Available</asp:ListItem>
                            <asp:ListItem Value="0">Unavailable</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group form-span-4">
                        <label>Remarks</label>
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                </div>
            </div>

            <!-- Delivery Locations Section -->
            <div class="form-section" id="secDeliveryLocations" runat="server">
                <div class="form-section-title">Delivery Locations / Addresses</div>
                <div class="form-section-body">
                    <div class="form-grid form-grid-3">
                        <div class="form-group">
                            <label>Delivery Address <span class="req">*</span></label>
                            <asp:TextBox ID="txtDelAddress" runat="server" CssClass="form-control" placeholder="Street / Warehouse address"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Contact Person</label>
                            <asp:TextBox ID="txtDelContactPerson" runat="server" CssClass="form-control" placeholder="Contact person name"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Contact Number</label>
                            <asp:TextBox ID="txtDelContactNo" runat="server" CssClass="form-control" placeholder="Phone or mobile"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Email</label>
                            <asp:TextBox ID="txtDelEmail" runat="server" CssClass="form-control" placeholder="email@address.com"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Country</label>
                            <asp:DropDownList ID="ddlDelCountry" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group" style="justify-content: flex-end;">
                            <label>&nbsp;</label>
                            <asp:Button ID="btnAddLocation" runat="server" CssClass="btn-view-primary" Text="+ Add Location" OnClick="btnAddLocation_Click" />
                        </div>
                    </div>

                    <!-- Delivery Locations Grid -->
                    <div class="table-responsive" style="border-top: 1px solid #e2e8f0;">
                        <asp:GridView ID="gridCustomerLocations" runat="server" AutoGenerateColumns="False"
                            CssClass="enterprise-grid" Width="100%" GridLines="None"
                            OnRowCommand="gridCustomerLocations_RowCommand" OnRowDataBound="gridCustomerLocations_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="SN" HeaderStyle-CssClass="col-sn" ItemStyle-CssClass="col-sn">
                                    <ItemTemplate>
                                        <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Address">
                                    <ItemTemplate>
                                        <div class="cell-entity">
                                            <span class="entity-bullet"></span>
                                            <strong><asp:Label ID="lblAddress" runat="server" Text='<%# Eval("ADDRESS") %>'></asp:Label></strong>
                                        </div>
                                        <asp:Label ID="lblLocPKID" runat="server" Text='<%# Eval("PK_ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Contact Person">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContactPerson" runat="server" Text='<%# Eval("CONTACT_PERSON") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Phone / Mobile">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContactNo" runat="server" Text='<%# Eval("CONTACT_NUMBER") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Email">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("EMAIL_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Country">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCountryName" runat="server" Text='<%# Eval("COUNTRY_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="col-actions" ItemStyle-CssClass="col-actions">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgDeleteLoc" runat="server" CommandName="RemoveLoc" 
                                            CommandArgument='<%# Eval("PK_ID") %>' 
                                            ImageUrl="~/images/icons/delete.png" CssClass="btn-action-icon" ToolTip="Delete Location"
                                            OnClientClick="return confirm('Are you sure you want to remove this delivery location?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- Form Actions -->
            <div class="form-actions">
                <asp:Button ID="btnReset" runat="server" Text="Cancel" CssClass="btn-clear" OnClick="btnReset_Click" />
                <asp:Button ID="btnSave" runat="server" Text="Save Customer" CssClass="btn-view-primary" OnClick="btnSave_Click" />
            </div>
        </div>

        <!-- Grid & Filter View -->
        <div id="divGrid" runat="server" visible="true">
            <!-- Filter Toolbar Card -->
            <div class="bs-card filter-card">
                <div class="filter-card-header">
                    <span class="filter-title">
                        <span class="filter-dot"></span> SEARCH & FILTER CUSTOMERS
                    </span>
                    <asp:Button ID="btnAddMore" runat="server" CssClass="btn-add-primary" Text="+ Add Customer" OnClick="btnAddMore_Click" />
                </div>
                
                <div class="filter-card-body">
                    <div id="trBranchFilter" runat="server" class="filter-field-group">
                        <label class="filter-label">Branch</label>
                        <asp:DropDownList ID="ddlBranchFilter" runat="server" CssClass="filter-select" AutoPostBack="true" OnSelectedIndexChanged="ddlBranchFilter_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>

                    <div class="filter-field-group" style="min-width: 140px; max-width: 160px;">
                        <label class="filter-label">Customer Code</label>
                        <asp:TextBox ID="txtSearchCustomerCode" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtSearchCustomerCode_TextChanged" placeholder="e.g. CUS001"></asp:TextBox>
                    </div>

                    <div class="filter-field-group" style="flex: 1; min-width: 280px;">
                        <label class="filter-label">Customer Name</label>
                        <div class="search-box-wrapper" style="max-width: 100%;">
                            <svg class="search-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                <circle cx="11" cy="11" r="8"></circle>
                                <line x1="21" y1="21" x2="16.65" y2="16.65"></line>
                            </svg>
                            <asp:TextBox ID="txtSearchCustomerName" runat="server" CssClass="table-search-input" placeholder="Type customer name and click Search..."></asp:TextBox>
                        </div>
                    </div>

                    <div>
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn-view-primary" Text="Search" OnClick="btnSearch_Click" />
                        <asp:Label ID="lblPKIDU" runat="server" Visible="False"></asp:Label>
                    </div>
                </div>
            </div>

            <!-- Customer Data Grid Card -->
            <div class="bs-card">
                <div class="table-responsive">
                    <asp:GridView ID="gridCustomer" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                        OnRowCommand="gridCustomer_RowCommand" OnRowDataBound="gridCustomer_RowDataBound"
                        CssClass="enterprise-grid" AllowPaging="True" OnPageIndexChanging="gridCustomer_PageIndexChanging" PageSize="10"
                        GridLines="None">
                        <PagerStyle CssClass="gridview-pager" />
                        <Columns>
                            <asp:TemplateField HeaderText="SN" HeaderStyle-CssClass="col-sn" ItemStyle-CssClass="col-sn">
                                <ItemTemplate>
                                    <span class="row-sn-text"><%# Container.DataItemIndex + 1 %></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- Customer Detail with Picture in Grid ({PK_ID}.jpg) --%>
                            <asp:TemplateField HeaderText="Customer Detail" HeaderStyle-CssClass="col-name">
                                <ItemTemplate>
                                    <div style="display: flex; align-items: flex-start; gap: 10px;">
                                        <asp:Image ID="imgGridAvatar" runat="server" CssClass="customer-avatar-table"
                                            ImageUrl='<%# GetCustomerImageUrl(Eval("PK_ID")) %>' />
                                        <div>
                                            <div class="cell-entity">
                                                <strong><asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CUSTOMER_NAME") %>'></asp:Label></strong>
                                            </div>
                                            <div style="font-size: 12px; color: #64748b; margin-top: 3px; line-height: 1.4;">
                                                Code: <strong><asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("CUSTOMER_CODE") %>'></asp:Label></strong>
                                                <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label><br />
                                                Address: <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("ADDRESS") %>'></asp:Label><br />
                                                Branch: <asp:Label ID="lblShowBranch" runat="server"></asp:Label>
                                                <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("OFFICE_CODE") %>' Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Tax / Reg. Detail">
                                <ItemTemplate>
                                    <div style="font-size: 13px; line-height: 1.5;">
                                        <span style="color: #64748b;">Type:</span> <asp:Label ID="lblRegIn" runat="server" Text='<%# Bind("PAN_VAT") %>'></asp:Label><br />
                                        <span style="color: #64748b;">No:</span> <asp:Label ID="lblPAN_VATNO" runat="server" Text='<%# Bind("VAT_PAN_NUMBER") %>'></asp:Label><br />
                                        <span style="color: #64748b;">Reg No:</span> <asp:Label ID="lblRegNo" runat="server" Text='<%# Bind("REG_NUMBER") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Contact Info">
                                <ItemTemplate>
                                    <div style="font-size: 13px; line-height: 1.5;">
                                        <span style="color: #64748b;">M:</span> <asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("MOBILE") %>'></asp:Label><br />
                                        <span style="color: #64748b;">P:</span> <asp:Label ID="lblPhoneNo" runat="server" Text='<%# Bind("PHONE") %>'></asp:Label><br />
                                        <span style="color: #64748b;">E:</span> <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("EMAIL") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Join Date">
                                <ItemTemplate>
                                    <div style="font-size: 13px; line-height: 1.5;">
                                        <div><span style="color: #64748b;">BS:</span> <asp:Label ID="lblJoinDateBS" runat="server" Text='<%# Bind("JOIN_DATE_BS") %>'></asp:Label></div>
                                        <div><span style="color: #64748b;">AD:</span> <asp:Label ID="lblJoinDateAD" runat="server" Text='<%# Bind("JOIN_DATE_AD") %>'></asp:Label></div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="col-status" ItemStyle-CssClass="col-status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Bind("STATUS") %>'></asp:Label>
                                    <asp:Label ID="lblStat" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- Scrollable Delivery Locations Column with Count Badge --%>
                            <asp:TemplateField HeaderText="Delivery Locations" HeaderStyle-Width="220px" ItemStyle-Width="220px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnCustId" runat="server" Value='<%# Bind("PK_ID") %>' />
                                    
                                    <div style="margin-bottom: 4px;">
                                        <asp:Label ID="lblLocCount" runat="server" CssClass="badge-stat"></asp:Label>
                                    </div>
                                    
                                    <div class="del-loc-container" style="max-height: 110px; overflow-y: auto; padding-right: 4px;">
                                        <asp:Repeater ID="rptDeliveryLocations" runat="server">
                                            <ItemTemplate>
                                                <div style="font-size: 11.5px; margin-bottom: 4px; padding: 4px 6px; background: #ffffff; border: 1px solid #e2e8f0; border-radius: 5px; box-shadow: 0 1px 2px rgba(0,0,0,0.03);">
                                                    <div style="color: #0f172a; font-weight: 600;">📍 <%# Eval("ADDRESS") %></div>
                                                    <div style="color: #64748b; font-size: 11px;">
                                                        <%# !string.IsNullOrEmpty(Convert.ToString(Eval("CONTACT_PERSON"))) ? Eval("CONTACT_PERSON") + " | " : "" %>
                                                        <%# Eval("CONTACT_NUMBER") %>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    
                                    <asp:Label ID="lblNoLoc" runat="server" Text="No locations added" Visible="false" Font-Size="12px" ForeColor="#94a3b8"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <span style="font-size: 13px; color: #64748b;">
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("REMARKS") %>'></asp:Label>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="col-actions" ItemStyle-CssClass="col-actions">
                                <ItemTemplate>
                                    <div class="action-btn-group">
                                        <asp:ImageButton ID="imgEdit" runat="server" CommandName="Change" ImageUrl="~/images/icons/edit.png" CssClass="btn-action-icon" ToolTip="Edit Customer" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>