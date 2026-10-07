<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Pages.aspx.cs" Inherits="entryforms_Pages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-1">
                Module
            </div>
            <div class="col-md-2">
                <asp:DropDownList ID="ddlModule" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                Sub Module
            </div>
            <div class="col-md-2">
                <asp:DropDownList ID="ddlSubModule" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnLoad" runat="server" Text="Load" CssClass="btn btn-sm btn-success" OnClick="btnLoad_Click" />
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnReOrder" runat="server" Text="Reorder" CssClass="btn btn-sm btn-success" OnClick="btnReOrder_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12" style="margin-bottom:5em">
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
                                <asp:DropDownList ID="ddlModuleE" runat="server"  Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:Label ID="lblPKIDE" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Module<br />
                                <asp:DropDownList ID="ddlModuleH" runat="server"  Width="100%" CssClass="form-control"
                                     OnSelectedIndexChanged="ddlModuleH_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblModule" runat="server" Text=''></asp:Label>
                                <asp:Label ID="lblPKID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>                           
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sub Module">
                            <EditItemTemplate>
                                 <asp:DropDownList ID="ddlSubModuleE" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Sub Module<br />
                                <asp:DropDownList ID="ddlSubModuleH" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSubModule" runat="server" Text=''></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Page Name">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPageNameE" runat="server" Text='<%# Bind("LINKNAME") %>' Width="100%" CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Page Name<br />
                                <asp:TextBox ID="txtPageNameH" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPageName" runat="server" Text='<%# Bind("LINKNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Link">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPageLinkE" runat="server" Text='<%# Bind("PAGENAME") %>' Width="100%" CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Page Link<br />
                                <asp:TextBox ID="txtPageLinkH" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPageLink" runat="server" Text='<%# Bind("PAGENAME") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Order">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtOrderE" runat="server" Text='<%# Bind("ORDER_BY") %>'  Width="50px" CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Order<br />
                                <asp:TextBox ID="txtOrderH" runat="server" Width="50px" CssClass="form-control"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                               <asp:TextBox ID="txtOrderI" runat="server" Text='<%# Bind("ORDER_BY") %>' Width="50px" CssClass="form-control"></asp:TextBox>
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

