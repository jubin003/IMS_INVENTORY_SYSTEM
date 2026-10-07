<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RawMaterialOut.aspx.cs" Inherits="Account_Utilities_RawMaterialOut" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="imsv5-container">
        <!-- PAGE HEADER -->
        <div class="imsv5-page-header">
            <div>
                <div class="imsv5-breadcrumb">
                    <span>Inventory</span>
                    <span>›</span>
                    <span class="active">Raw Material Out</span>
                </div>

                <h1 class="imsv5-page-title"><asp:Label runat="server" ID="lblTitle">Raw Material Out</asp:Label></h1>
                <div class="imsv5-page-subtitle">Issue raw materials for production and manage material quantities.</div>
            </div>
        </div>
        <!-- MAIN INFORMATION -->
        <div class="bs-card filter-card">

            <div class="filter-card-header">
                <div class="filter-title"><span class="filter-dot"></span> Material Issue Details</div>
                <div class="filter-hint">Enter production and issue information</div>
            </div>

            <div class="filter-card-body">

                <div class="filter-field-group">
                    <asp:Label runat="server" ID="lblProduction" CssClass="filter-label" Text="For Production"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlProduction" CssClass="filter-select">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="filter-field-group">
                    <asp:Label runat="server" ID="lblIssue" CssClass="filter-label" Text="Issue to"></asp:Label>
                    <asp:TextBox runat="server" ID="txtIssue" CssClass="filter-select"></asp:TextBox>
                </div>

                <div class="filter-field-group">
                    <asp:Label runat="server" ID="lblDate" CssClass="filter-label" Text="Date"></asp:Label>
                    <asp:TextBox runat="server" ID="txtDate" CssClass="filter-select datepicker"></asp:TextBox>
                </div>

<%--                <div class="filter-field-group">
                    <asp:Label runat="server" ID="lblBatchno" CssClass="filter-label" Text="Batch Number"></asp:Label>
                    <asp:TextBox runat="server" ID="txtBatchno" CssClass="filter-select"></asp:TextBox>
                </div>--%>

            </div>

        </div>


        <!-- RAW MATERIAL TABLE -->
        <div class="bs-card">

            <div class="card-toolbar">
                <div class="filter-title"><span class="filter-dot"></span> Raw Material</div>
            </div>

            <div class="table-responsive">

                <asp:GridView runat="server" ID="grdRawMaterial" AutoGenerateColumns="false" CssClass="enterprise-grid" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDeleting="grdRawMaterial_RowDeleting" OnRowDataBound="grdRawMaterial_RowDataBound">

                    <Columns>

                        <asp:TemplateField HeaderText="SN">
                            <HeaderStyle CssClass="col-sn" />
                            <ItemStyle CssClass="col-sn" />
                            <ItemTemplate>
                                <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblSnG" runat="server" CssClass="row-sn-text" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Material">
                            <ItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlMaterial" CssClass="form-control inline-select" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Available Qty">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtAvQty" CssClass="form-control inline-input" AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtQuantity" CssClass="form-control inline-input" AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Unit">
                            <ItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlUnit" CssClass="form-control inline-select" Enabled="false">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Rate">
                            <HeaderStyle CssClass="col-rate" />
                            <ItemStyle CssClass="col-rate" />
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtRate" CssClass="form-control rate-field"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Batch no.">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtBatchno" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Expiry Date">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtExpDate" CssClass="form-control datepicker"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTotal" Width="100px" CssClass="cell-rate"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Delete">
                            <HeaderStyle CssClass="col-actions" />
                            <ItemStyle CssClass="col-actions" />
                            <ItemTemplate>
                                <asp:Button runat="server" ID="btnDelete" CommandName="Delete" Text="Delete" CssClass="btn btn-danger btn-sm" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

            <!-- ACTION BUTTONS -->
            <div style="padding:16px 20px; display:flex; justify-content:flex-end; gap:8px;">
                <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="Add" CssClass="btn-add-primary" />
                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Save" CssClass="btn-add-primary" />
            </div>

        </div>

    </div>

</asp:Content>
