<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FinishGoods.aspx.cs" Inherits="RawMaterial_FinishGoods" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container mt-4">
        <h3 class="mb-3">Finish Goods In</h3>
        <div class="row align-items-end">
            <div class="col-md-3 mb-3">
                <asp:Label runat="server" ID="lblMaterialName" Text="Product Name" CssClass="form-label d-block mb-1"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlMaterialName" CssClass="form-control"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlMaterialName_SelectedIndexChanged">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-3 mb-3">
                <asp:Label runat="server" ID="lblQuantity" Text="Quantity" CssClass="form-label d-block mb-1"></asp:Label>
                <asp:TextBox runat="server" ID="txtQuantity" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-3 mb-3">
                <asp:Label runat="server" ID="lblUnit" Text="Unit" CssClass="form-label d-block mb-1"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlUnit" CssClass="form-control">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="col-md-3 mb-3">
                 <asp:Label runat="server" ID="lblDate" CssClass="form-label d-block mb-1" Text="Date"></asp:Label>
                <asp:TextBox runat="server" ID="txtDate" CssClass="form-control datepicker"></asp:TextBox>
            </div>

            <div class="col-md-3 mb-3">
                <br />
                <asp:Button runat="server" ID="btnAdd" Text="Add" OnClick="btnAdd_Click" CssClass="btn btn-success w-100"/>
            </div>
        </div>

        <h5 class="mt-4">Cost Price</h5>
        <asp:Label runat="server" ID="lblCostPrice" CssClass="fs-5"></asp:Label>
    </div>

    <div class="container">
        <asp:GridView runat="server" ID="grdStoreIN" AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true"
            OnRowDataBound="grdStoreIN_RowDataBound"
            CssClass="table table-bordered table-striped table-hover align-middle"
            HeaderStyle-CssClass="table-light">
            <Columns>
                <asp:TemplateField HeaderText="SN">
                    <ItemTemplate>
                            <asp:Label ID="lblSnG" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Product Name">
                    <ItemTemplate>
                        <asp:Label ID="lblProdname" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblQty"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Rate">
                     <ItemTemplate>
                        <asp:Label runat="server" ID="lblRate"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Unit">
                     <ItemTemplate>
                        <asp:Label runat="server" ID="lblUnit"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Expiry Date">
                        <ItemTemplate>
                           <asp:TextBox runat="server" ID="txtExpDate" CssClass="form-control datepicker"></asp:TextBox>
                        </ItemTemplate>
                  </asp:TemplateField>

                <asp:TemplateField HeaderText="Batch Number">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtbatchno" CssClass="form-control"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Delete">
                     <ItemTemplate>
                         <asp:Button runat="server" ID="btnDelete" CssClass="btn btn-danger btn-sm" OnClick="btnDelete_Click" Text="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

        <div>
            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-success" OnClick="btnSave_Click"/>
        </div>
    </div>
</asp:Content>