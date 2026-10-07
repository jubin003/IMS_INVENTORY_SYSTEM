<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Area.aspx.cs" 
    Inherits="Administration_Area" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class ="row" style="margin-bottom:10px; margin-left:5px">
            <div class="col-md-3" id="divBranch" runat="server" >Branch <br />
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
     <div class="container-fluid">
        <asp:GridView ID="grid" runat="server" AutoGenerateColumns="False"
            OnRowEditing="grid_RowEditing" OnRowUpdating="grid_RowUpdating"
            OnRowCancelingEdit="grid_RowCancelingEdit" CssClass="gridtable">
            <Columns>
                <asp:TemplateField HeaderText="SN.">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                   <asp:TemplateField HeaderText="Area Code">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAreaCodeE" runat="server"  CssClass="form-control" Text='<%# Bind("AREA_CODE") %>'></asp:TextBox>
                        <asp:Label ID="lblPKIDE" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        Area Code<br />
                        <asp:TextBox ID="txtAreaCodeH" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:Label ID="lblPKIDH" runat="server" Text='<%# Bind("PK_ID") %>'></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAreaCode" runat="server" Text='<%# Bind("AREA_CODE") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>        
                <asp:TemplateField HeaderText="Area Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAreaNameE" runat="server"  CssClass="form-control" Text='<%# Bind("AREA_NAME") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <HeaderTemplate>
                        Area Name<br />
                        <asp:TextBox ID="txtAreaNameH" runat="server" CssClass="form-control"></asp:TextBox>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAreaName" runat="server" Text='<%# Bind("AREA_NAME") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>             
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="btnEdit" runat="server" CommandName="update" ImageUrl="~/images/icons/upload.png" ToolTip="Update" />
                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="cancel" ImageUrl="~/images/icons/cancel.gif" ToolTip="Cancel" />
                    </EditItemTemplate>
                    <HeaderTemplate>
                        <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" CssClass="btn btn-success" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="edit" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

