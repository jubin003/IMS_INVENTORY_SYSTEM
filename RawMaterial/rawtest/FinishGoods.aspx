<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true"
    CodeFile="FinishGoods.aspx.cs"
    Inherits="RawMaterial_FinishGoods" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">

    <div class="hms-container">

        <div class="hms-page-header">
            <div>
                <div class="hms-breadcrumb">
                    <span>Inventory</span>
                    <span>›</span>
                    <span class="active">Finished Goods</span>
                </div>

                <h1 class="hms-page-title">Finished Goods In</h1>

                <div class="hms-page-subtitle">
                    Record finished goods received into inventory.
                </div>
            </div>
        </div>


        <div class="bs-card filter-card">

            <div class="filter-card-header">
                <div class="filter-title">
                    <span class="filter-dot"></span>
                    Finished Goods Entry
                </div>

                <div class="filter-hint">
                    Enter finished goods information
                </div>
            </div>


            <div class="filter-card-body">

                <div class="filter-field-group">
                    <asp:Label
                        runat="server"
                        ID="lblMaterialName"
                        Text="Material Name"
                        CssClass="filter-label">
                    </asp:Label>

                    <asp:DropDownList
                        runat="server"
                        ID="ddlMaterialName"
                        CssClass="filter-select">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </div>


                <div class="filter-field-group">
                    <asp:Label
                        runat="server"
                        ID="lblQuantity"
                        Text="Quantity"
                        CssClass="filter-label">
                    </asp:Label>

                    <asp:TextBox
                        runat="server"
                        ID="txtQuantity"
                        CssClass="filter-select">
                    </asp:TextBox>
                </div>


                <div class="filter-field-group">
                    <asp:Label
                        runat="server"
                        ID="lblUnit"
                        Text="Unit"
                        CssClass="filter-label">
                    </asp:Label>

                    <asp:DropDownList
                        runat="server"
                        ID="ddlUnit"
                        CssClass="filter-select">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </div>


                <div class="filter-field-group">
                    <asp:Label
                        runat="server"
                        ID="lblDate"
                        Text="Date"
                        CssClass="filter-label">
                    </asp:Label>

                    <asp:TextBox
                        runat="server"
                        ID="txtDate"
                        CssClass="filter-select datepicker">
                    </asp:TextBox>
                </div>


                <div>
                    <asp:Button
                        runat="server"
                        ID="btnAdd"
                        Text="Add"
                        OnClick="btnAdd_Click"
                        CssClass="btn-add-primary" />
                </div>

            </div>


            <div style="margin-top:20px;">
                <span class="filter-label">Cost Price</span>

                <div class="cell-rate" style="margin-top:6px;">
                    Rs.
                    <asp:Label
                        runat="server"
                        ID="lblCostPrice">
                    </asp:Label>
                </div>
            </div>

        </div>


        <div class="bs-card">

            <div class="card-toolbar">
                <div>
                    <strong>Finished Goods</strong>
                </div>
            </div>


            <div class="table-responsive">

                <asp:GridView
                    runat="server"
                    ID="grdStoreIN"
                    AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true"
                    OnRowDataBound="grdStoreIN_RowDataBound"
                    CssClass="enterprise-grid"
                    GridLines="None">

                    <Columns>

                        <asp:TemplateField HeaderText="SN">
                            <HeaderStyle CssClass="col-sn" />
                            <ItemStyle CssClass="col-sn" />

                            <ItemTemplate>
                                <asp:Label
                                    ID="lblSnG"
                                    runat="server"
                                    CssClass="row-sn-text"
                                    Text='<%# Container.DataItemIndex + 1 %>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Product Name">
                            <ItemTemplate>

                                <div class="cell-entity">
                                    <span class="entity-bullet"></span>

                                    <asp:Label
                                        ID="lblProdname"
                                        runat="server">
                                    </asp:Label>
                                </div>

                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Quantity">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblQty">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Rate">
                            <HeaderStyle CssClass="col-rate" />
                            <ItemStyle CssClass="col-rate" />

                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblRate"
                                    CssClass="cell-rate">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Unit">
                            <ItemTemplate>
                                <asp:Label
                                    runat="server"
                                    ID="lblUnit">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Action">
                            <HeaderStyle CssClass="col-actions" />
                            <ItemStyle CssClass="col-actions" />

                            <ItemTemplate>
                                <asp:Button
                                    runat="server"
                                    ID="btnDelete"
                                    OnClick="btnDelete_Click"
                                    CssClass="btn btn-danger btn-sm"
                                    Text="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>


            <div style="padding:16px 20px; text-align:right;">

                <asp:Button
                    runat="server"
                    ID="btnSave"
                    Text="Save"
                    CssClass="btn-add-primary"
                    OnClick="btnSave_Click" />

            </div>

        </div>

    </div>

</asp:Content>