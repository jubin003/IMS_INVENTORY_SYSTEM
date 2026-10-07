<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Asset.aspx.cs" Inherits="FixedAsset_MasterData_Asset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table>
        <tr>
            <td style="width: 200px">Asset Category
                <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                </asp:DropDownList></td>
            <td style="width: 200px; padding-left: 10px">Asset Sub category
                <asp:DropDownList ID="ddlSubCategory" CssClass="form-control" runat="server"></asp:DropDownList>
            </td>
            <td style="padding-left: 10px">
                <br />
                <asp:Button ID="btnView" CssClass="btn btn-primary" runat="server" Text="View" Width="100px" OnClick="btnView_Click" />
            </td>
            <td>
                <br />
                <asp:Button ID="btnAdd" CssClass="btn btn-primary" runat="server" Text="Add" Width="100px" OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
    <br />
    <div class="tickettable" id="Addform" runat="server" visible="false">
        <table style="width: 50%">


            <tr>
                <td style="width: 120px">Asset Code<br />
                    <asp:Label ID="lblPk_Id" runat="server" Visible="false"></asp:Label>
                    <asp:TextBox ID="txtAssetCode" CssClass="form-control" runat="server"></asp:TextBox></td>

                <td style="padding-left: 10px">Asset Name<br />
                    <asp:TextBox ID="txtAssetName" CssClass="form-control" runat="server"></asp:TextBox></td>

                <td style="padding-left: 10px">Unit<br />
                    <asp:DropDownList ID="ddlUnit" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table style="width: 50%">
            <tr>
                <td>Status<br />
                    <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server">
                        <asp:ListItem Text="Available" Value="1" />
                        <asp:ListItem Text="UnAvailable" Value="0" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save" Width="150px" OnClick="btnSave_Click" />
        <asp:Button ID="btnCancel" CssClass="btn btn-primary" runat="server" Text="Cancel" Width="150px" OnClick="btnCancel_Click" />
    </div>
    <br />

    <asp:GridView ID="gridAsset" runat="server" AutoGenerateColumns="False" CssClass="gridtable" OnRowCommand="gridAsset_RowCommand" OnRowDataBound="gridAsset_RowDataBound" Width="60%">
        <Columns>
            <asp:TemplateField HeaderText="SN">
                <ItemTemplate>
                    <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                    <asp:Label ID="lblPKIDG" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Asset Code">
                <ItemTemplate>
                    <asp:Label ID="lblAssetCode" runat="server" Text='<%# Bind("ASSET_CODE") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Asset Name">
                <ItemTemplate>
                    <asp:Label ID="lblAssetName" runat="server" Text='<%# Bind("ASSET_NAME") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Unit">
                <ItemTemplate>
                    <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("UNIT") %>' Visible="false"></asp:Label>
                    <asp:Label ID="lblSnowUnit" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                    <asp:Label ID="lblShowStatus" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Edit">
                <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server" CommandName="send" Text="Edit" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

