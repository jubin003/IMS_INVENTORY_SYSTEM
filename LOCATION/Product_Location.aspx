<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Product_Location.aspx.cs" Inherits="LOCATION_PRODUCT_Location" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="container-fluid">
        <table style="text-align: left">
            <tr>
                <td>PRODUCT&nbsp; &nbsp;<asp:DropDownList ID="ddlPRODUCT"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlPRODUCT_SelectedIndexChanged" runat="server">
                </asp:DropDownList>
                    <script>
                        $('#<%=ddlPRODUCT.ClientID%>').chosen();
                    </script>
                </td>
                <td>Shelf&nbsp;&nbsp;
                </td>
                <td>
                    <asp:DropDownList ID="ddlShelf" runat="server" Height="22px" AutoPostBack="true" Width="100px"
                        OnSelectedIndexChanged="ddlShelf_SelectedIndexChanged">
                    </asp:DropDownList>
                    <script>
                        $('#<%=ddlShelf.ClientID%>').chosen();
                    </script>
                </td>
                <td>Compart&nbsp;&nbsp;
                </td>
                <td>
                    <asp:DropDownList ID="ddlCompart" runat="server" Height="22px" Width="100px"></asp:DropDownList>
                    <script>
                        $('#<%=ddlCompart.ClientID%>').chosen();
                    </script>
                </td>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-success" OnClick="btnAdd_Click" />
                    &nbsp;&nbsp;
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-success" OnClick="btnFilter_Click" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPKIDU" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
        <div id="hidden" runat="server">
            <br />
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:GridView ID="gridPRODUCT" runat="server" AutoGenerateColumns="False" CssClass="gridtable" EnableModelValidation="True"
                            OnRowDataBound="gridPRODUCT_RowDataBound" OnRowCommand="gridPRODUCT_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="SN">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" HeaderText="PRODUCT ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblPRODUCTId" runat="server" Text='<%# Bind("MEDICINE_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Product Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPRODUCTCode" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPRODUCTName" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Shelf">
                                    <ItemTemplate>
                                        <asp:Label ID="lblShelfID" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lblShelf" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Compart">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompartID" runat="server" Text='<%# Bind("COMPART_NO") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblCompart" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgEdit" runat="server" CommandName="Change" ImageUrl="~/images/icons/edit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>


</asp:Content>

