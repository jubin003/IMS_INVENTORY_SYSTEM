<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Product_Rates.aspx.cs" Inherits="Administration_Product_Rates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table class="gridtable">
        <tr id="trBranch" runat="server" visible="false">
            <td colspan="4">Branch:
    <asp:DropDownList ID="ddlBranch" CssClass="form-control" Width="400px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Product Group:
    <asp:DropDownList ID="ddlProductGroup" CssClass="form-control" Width="250px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProductGroup_SelectedIndexChanged"></asp:DropDownList></td>


            <td>Product Category:
     <asp:DropDownList ID="ddlProductCategory" CssClass="form-control" Width="250px"
         AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProductCategory_SelectedIndexChanged">
     </asp:DropDownList></td>


            <td id="trRateType" runat="server">
                <br />
                Product Rate Type:
    <asp:DropDownList ID="ddlCustomerType" CssClass="form-control" Width="250px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCustomerType_SelectedIndexChanged" ></asp:DropDownList><br />
            </td>

            <td>
                <br />
                <asp:Button ID="btn_View" CssClass="btn btn-primary" runat="server" Text="View" OnClick="btn_View_Click" /></td>
        </tr>
    </table>

    <br />

    <div id="divGrid" runat="server" visible="false">
        <asp:GridView ID="gridRate" runat="server" AutoGenerateColumns="False" CssClass="gridtable" OnRowDataBound="gridRate_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="S.N">
                    <ItemTemplate>
                        <asp:Label ID="lblSN" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                        <asp:Label ID="lbl_Pr_ID" Visible="false" runat="server"></asp:Label>
                        <asp:Label ID="lblPk_id" Visible="false" runat="server" Text='<%# Bind("PK_ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Product Code">
                    <ItemTemplate>
                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Bind("PRODUCT_CODE") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Product Name">
                    <ItemTemplate>
                        <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("PRODUCT_NAME") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rate With VAT ">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRate" Text="0.00" AutoPostBack="true" CssClass="form-control" runat="server" OnTextChanged="txtRate_TextChanged"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actual Rate">
                    <ItemTemplate>
                        <asp:TextBox ID="txtExVatRate" Enabled="false" Text="0.00" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
        <br />
        <asp:Button ID="btn_Save" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btn_Save_Click" />
    </div>

</asp:Content>

