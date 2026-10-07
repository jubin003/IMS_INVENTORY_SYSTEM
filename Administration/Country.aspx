<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Country.aspx.cs" Inherits="Administration_Country" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container mt-3">

        <div class="row mb-3">
            <div class="col-md-4 form-group">
                <asp:Label runat="server" ID="lblCountry" Text="Country"></asp:Label>
                <asp:TextBox runat="server" ID="txtCountry" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-md-4 form-group">
                <asp:Label runat="server" ID="lblNationality" Text="Nationality"></asp:Label>
                <asp:TextBox runat="server" ID="txtNationality" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-md-4 d-flex align-items-end">
                <br />
                <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" CssClass="btn btn-success" Text="Add" />
            </div>
        </div>

        <asp:GridView runat="server" ID="grdCountry" AutoGenerateColumns="false"
            CssClass="table table-bordered"
            OnRowCommand="grdCountry_RowCommand">

            <Columns>
                <asp:TemplateField HeaderText="SN">
                    <ItemTemplate>
                        <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblSnG" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Country">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtgrdCountry" CssClass="form-control" Text='<%# Bind("COUNTRY_NAME") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Nationality">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtgrdNationality" CssClass="form-control" Text='<%# Bind("NATIONALITY") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnUpdate" Text="Update" CssClass="btn btn-primary" CommandName="UpdateRow" />
                        <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="btn btn-danger" CommandName="DeleteRow" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>
</asp:Content>