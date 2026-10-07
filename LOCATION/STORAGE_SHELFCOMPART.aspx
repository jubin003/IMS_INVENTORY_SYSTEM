<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="STORAGE_SHELFCOMPART.aspx.cs" Inherits="LOCATION_STORAGE_SHELFCOMPART" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-6">


                <asp:Label Text="" ID="lblSelectedShelf" Visible="false" runat="server" />
                <asp:GridView Width="100%" ID="gridShelfCompart" CssClass="gridtable" AutoGenerateColumns="false" runat="server"
                    OnRowEditing="gridShelfCompart_RowEditing" OnRowCancelingEdit="gridShelfCompart_RowCancelingEdit"
                    OnRowUpdating="gridShelfCompart_RowUpdating" OnRowDataBound="gridShelfCompart_RowDataBound">

                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>SN</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPK" Visible="false" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Shelf
                            <asp:DropDownList CssClass="form-control" OnSelectedIndexChanged="ddlShelfH_SelectedIndexChanged" Style="color: #000000" ID="ddlShelfH" AutoPostBack="true" runat="server">
                            </asp:DropDownList>
                            </HeaderTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlShelfE" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                                <asp:Label Text='<%# Bind("SHELFID") %>' Visible="false" ID="lblShelfidE" runat="server" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("SHELFID") %>' ID="lblShelfid" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <br />
                                Compart No.
                                <asp:TextBox ID="txtCompartNoH" CssClass="form-control" Style="color: #000000" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="*" ValidationGroup="AddBtn" ControlToValidate="txtCompartNoH" runat="server" />
                            </HeaderTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCompartnoE" CssClass="form-control" runat="server" />
                                <asp:Label Text='<%# Bind("COMPARTNO") %>' Visible="false" ID="lblCompartNoE" runat="server" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("COMPARTNO") %>' ID="lblCompartNo" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <br />
                                <asp:Button Text="Add" CssClass="btn btn-success" ID="btnAdd" OnClick="btnAdd_Click" ValidationGroup="AddBtn" runat="server" />
                            </HeaderTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="imgbtnUpdate" runat="server" ImageUrl="~/images/icons/upload.png" CommandName="Update" />
                                <asp:ImageButton ID="imgbtnCancel" runat="server" ImageUrl="~/images/icons/delete.gif" CommandName="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>                  
                </asp:GridView>

            </div>
        </div>
    </div>
</asp:Content>

