<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Product_Rate_type.aspx.cs" Inherits="Administration_Product_Rate_type" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class ="row" style="margin-bottom:10px">
            <div class="col-md-3" id="divBranch" runat="server">Branch <br />
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-md-10">
                <asp:GridView ID="gridCusType" runat="server" AutoGenerateColumns="False" CssClass="gridtable" OnRowCancelingEdit="gridCusType_RowCancelingEdit" OnRowDataBound="gridCusType_RowDataBound" OnRowEditing="gridCusType_RowEditing" OnRowUpdating="gridCusType_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="S.N">
                            <ItemTemplate>
                                <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                <asp:Label ID="lblPK" runat="server" Visible="false" Text='<%# Bind("PK_ID") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Customer Type Name
                            <asp:TextBox ID="txtCustomerType" CssClass="form-control" runat="server"></asp:TextBox>
                            </HeaderTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCustomerTypeE" CssClass="form-control" runat="server" Text='<%# Bind("RATE_TYPE_NAME") %>'>></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCustname" runat="server" Text='<%# Bind("RATE_TYPE_NAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Status
                    <br />
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Available" Value="1" />
                                    <asp:ListItem Text="UnAvailable" Value="0" />
                                </asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Bind("STATUS") %>'>></asp:Label>
                                <asp:Label ID="lblShowstatus" runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlStatusE" CssClass="form-control" runat="server">
                                    <asp:ListItem Text="Available" Value="1" />
                                    <asp:ListItem Text="UnAvailable" Value="0" />

                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Rate Editable
                    <br />
                                <asp:DropDownList ID="ddlEditable" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Available" Value="1" />
                                    <asp:ListItem Text="UnAvailable" Value="0" />
                                </asp:DropDownList>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEditable" runat="server" Visible="false" Text='<%# Bind("RATE_EDITABLE") %>'>></asp:Label>
                                <asp:Label ID="lblShowEditable" runat="server"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlEditableE" CssClass="form-control" runat="server">
                                    <asp:ListItem Text="Available" Value="1" />
                                    <asp:ListItem Text="UnAvailable" Value="0" />
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Order">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtOrderE" TextMode="Number" runat="server" Text='<%# Bind("ORDER_BY") %>' Width="50px" CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                Order<br />
                                <asp:TextBox ID="txtOrderH" TextMode="Number" runat="server" Width="50px" CssClass="form-control"></asp:TextBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="lblOrder" runat="server" Text='<%# Bind("ORDER_BY") %>' Width="50px" CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-primary" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" CommandName="Update" CssClass="btn btn-primary" Text="Update" />
                                <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-primary" Text="Cancel" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>

