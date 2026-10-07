<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RawMaterialReturn.aspx.cs" Inherits="RawMaterial_RawMaterialReturn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="imsv5-container">
        <div class="imsv5-page-header">
            <div>
                <div class="imsv5-breadcrumb">
                    <span>Inventory</span>
                    <span>›</span>
                    <span class="active">Raw Material Return</span>
                </div>

                <h1 class="imsv5-page-title"><asp:Label runat="server" ID="lblTitle">Raw Material Return</asp:Label></h1>
                <div class="imsv5-page-subtitle">Return unused raw materials from production back to inventory.</div>
            </div>
        </div>
        <div class="bs-card filter-card">
            <div class="filter-card-header">
                <div class="filter-title"><span class="filter-dot"></span> Material Return Details</div>
                <div class="filter-hint">Select production and enter return information</div>
            </div>
            <div class="filter-card-body">
                <div class="filter-field-group">
                    <asp:Label runat="server" ID="lblProduction" CssClass="filter-label" Text="For Production"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlProduction" CssClass="filter-select" AutoPostBack="true" OnSelectedIndexChanged="ddlProduction_SelectedIndexChanged">
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

            </div>

        </div>
        <div class="bs-card">

            <div class="card-toolbar">
                <div class="filter-title"><span class="filter-dot"></span> Raw Material</div>
            </div>

            <div class="table-responsive">

                <asp:GridView runat="server" ID="grdMaterial" AutoGenerateColumns="false" CssClass="enterprise-grid" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDeleting="grdMaterial_RowDeleting" OnRowDataBound="grdMaterial_RowDataBound">

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

                        <asp:TemplateField HeaderText="Total Quantity">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblTotalQty" Width="100px"></asp:Label>
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

                        <asp:TemplateField HeaderText="Expiry Date">
                            <ItemTemplate>
                               <asp:TextBox runat="server" ID="txtExpDate" CssClass="form-control datepicker inline-input"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Batch no.">
                            <ItemTemplate>
                               <asp:TextBox runat="server" ID="txtBatchno" CssClass="form-control inline-input"></asp:TextBox>
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

            <div style="padding:16px 20px; display:flex; justify-content:flex-end; gap:8px;">
                <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="Add" CssClass="btn-add-primary" />
                <asp:Button runat="server" ID="btnReturn" OnClick="btnReturn_Click" Text="Return" CssClass="btn-add-primary" />
            </div>

        </div>

    </div>

</asp:Content>
