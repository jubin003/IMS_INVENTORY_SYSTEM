<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="STORAGE_SHELF.aspx.cs" Inherits="LOCATION_STORAGE_SHELF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-2">
                            Shelf No. &nbsp;
                        <asp:TextBox ID="txtShelf" Width="150px" CssClass="form-control" runat="server" />
                        </div>
                        <div class="col-md-2">
                            <br />
                            <asp:Button Text="Add" CssClass="btn btn-success" ID="btnAdd" OnClick="btnAdd_Click" runat="server" />

                        </div>



                    </div>
                    <div class="row">
                        <br />
                        <asp:GridView ID="gridShelf" CssClass="gridtable" AutoGenerateColumns="false" Width="500px" runat="server"
                            OnRowEditing="gridShelf_RowEditing"
                            OnRowDataBound="gridShelf_RowDataBound"
                            OnRowCancelingEdit="gridShelf_RowCancelingEdit"
                            OnRowUpdating="gridShelf_RowUpdating"
                            >

                            <Columns>
                                <asp:TemplateField HeaderText="SNo.">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Container.DataItemIndex+1 %>' ID="lblSno" runat="server" />
                                        <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPK" Visible="false" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Shelf">
                                    <EditItemTemplate>
                                        <asp:TextBox CssClass="form-control" ID="txtShelfE" runat="server" />
                                        <asp:Label Text='<%# Bind("SHELFNO") %>' Visible="false" ID="lblShelfE" runat="server" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Bind("SHELFNO") %>' ID="lblShelf" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" ImageUrl="~/images/icons/upload.png" ToolTip="Update" runat="server" />
                                        <asp:ImageButton ID="imgbtnCancel" CommandName="Cancel" ImageUrl="~/images/icons/delete.gif" ToolTip="Cancel" runat="server" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton CommandName="Edit" ID="imgbtnEdit"
                                            ToolTip="Edit" runat="server" ImageUrl="~/images/icons/edit.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                          
                        </asp:GridView>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>

