<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RawMaterialReturn.aspx.cs" Inherits="RawMaterial_RawMaterialReturn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container mt-4">
        <h4 class="mb-3">
            <asp:Label runat="server" ID="lblTitle">Raw Material Return</asp:Label>
        </h4>

        <div class="row align-items-end mb-3">
            <div class="col-md-4">
                <asp:Label runat="server" ID="lblProduction" CssClass="form-label d-block mb-1" Text="For Production"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlProduction" CssClass="form-control"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlProduction_SelectedIndexChanged">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="col-md-4">
                <asp:Label runat="server" ID="lblIssue" CssClass="form-label d-block mb-1" Text="Issue to"></asp:Label>
                <asp:TextBox runat="server" ID="txtIssue" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-md-4">
                <asp:Label runat="server" ID="lblDate" CssClass="form-label d-block mb-1" Text="Date"></asp:Label>
                <asp:TextBox runat="server" ID="txtDate" CssClass="form-control datepicker"></asp:TextBox>
            </div>

        </div>

        <br />

        <h5 class="mb-2">Raw Material</h5>

        <div class="table-responsive mb-3">
            <asp:GridView runat="server" ID="grdMaterial" AutoGenerateColumns="false"
                CssClass="table table-bordered table-hover align-middle"
                ShowHeaderWhenEmpty="true"   
                OnRowDeleting="grdMaterial_RowDeleting" 
                OnRowDataBound="grdMaterial_RowDataBound">
                <HeaderStyle CssClass="table-light" />
                <Columns>
                    <asp:TemplateField HeaderText="SN">
                        <ItemTemplate>
                            <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblSnG" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Material">
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="ddlMaterial" CssClass="form-control form-control-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged">
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
                            <asp:TextBox runat="server" ID="txtQuantity" CssClass="form-control form-control-sm" AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit">
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="ddlUnit" CssClass="form-control form-control-sm" Enabled="false">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtRate" CssClass="form-control text-center"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expiry Date">
                        <ItemTemplate>
                           <asp:TextBox runat="server" ID="txtExpDate" CssClass="form-control datepicker"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Batch no.">
                        <ItemTemplate>
                           <asp:TextBox runat="server" ID="txtBatchno" CssClass="form-control"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Total">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblTotal" Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:Button runat="server" ID="btnDelete" CommandName="Delete" Text="Delete" CssClass="btn btn-danger btn-sm" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="Add" CssClass="btn btn-success me-2" />
        <asp:Button runat="server" ID="btnReturn" OnClick="btnReturn_Click" Text="Return" CssClass="btn btn-success" />
    </div>
</asp:Content>