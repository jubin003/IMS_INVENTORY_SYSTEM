<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SubModule.aspx.cs" Inherits="entryforms_SubModule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="container-fluid">
        <div class="row">
            <div class="col-md-1">
                Module
            </div>
            <div class="col-md-2">
                <asp:DropDownList ID="ddlModule" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>          
            <div class="col-md-1">
                <asp:Button ID="btnLoad" runat="server" Text="Load" CssClass="btn btn-sm btn-success" OnClick="btnLoad_Click" />
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnReOrder" runat="server" Text="Reorder" CssClass="btn btn-sm btn-success" OnClick="btnReOrder_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:GridView ID="grid" runat="server" AutoGenerateColumns="False"
                    OnRowEditing="grid_RowEditing" OnRowUpdating="grid_RowUpdating"
                    OnRowCancelingEdit="grid_RowCancelingEdit" EnableModelValidation="True" 
                    CssClass="gridtable"  OnRowDataBound="grid_RowDataBound"
                    Width="100%"
                    >
                    <Columns>
                        <asp:TemplateField HeaderText="SN.">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Module">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlModuleE" runat="server"></asp:DropDownList>
                                <asp:Label ID="lblPKIDE" runat="server" Text='<%# Bind("SUBMODULE_ID") %>' Visible="false"></asp:Label>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Module<br />
                                <asp:DropDownList ID="ddlModuleH" runat="server"></asp:DropDownList>                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblModule" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblPKID" runat="server" Text='<%# Bind("SUBMODULE_ID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>                           
                        </asp:TemplateField>                      

                        <asp:TemplateField HeaderText="SubModule">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSubModuleE" runat="server" Text='<%# Bind("SUBMODULE_NAME") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                SubModule<br />
                                <asp:TextBox ID="txtSubModuleH" runat="server"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSubModuleName" runat="server" Text='<%# Bind("SUBMODULE_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Order">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtOrderE" runat="server" Text='<%# Bind("ORDER_BY") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Order<br />
                                <asp:TextBox ID="txtOrderH" runat="server"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                               <asp:TextBox ID="txtOrderI" runat="server" Text='<%# Bind("ORDER_BY") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="update" ImageUrl="~/images/icons/upload.png" ToolTip="Update" />
                                <asp:ImageButton ID="btnCancel" runat="server" CommandName="cancel" ImageUrl="~/images/icons/cancel.gif" ToolTip="Cancel" />
                            </EditItemTemplate>
                            <HeaderTemplate>
                                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" Style="height: 26px" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

