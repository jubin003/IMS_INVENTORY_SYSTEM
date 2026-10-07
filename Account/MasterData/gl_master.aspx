<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="gl_master.aspx.cs" Inherits="Account_MasterData_gl_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table class="gridtable" style="width: 1000px;">
        <tr>
            <td>Accounts 
            <br />
                <asp:DropDownList ID="ddlAccountsHead" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAccountsHead_SelectedIndexChanged">
                    
                </asp:DropDownList>
            </td>
            <td>Financial Statement
              <br />
                <asp:DropDownList ID="ddlFinancialStatement" runat="server" CssClass="form-control">
                    <asp:ListItem Value="BS">Balance Sheet</asp:ListItem>
                    <asp:ListItem Value="IS">Income Statement</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>GL Code
              <br />
                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </td>

            <td>GL Name
               <br />
                <asp:TextBox ID="txtGlName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:Label ID="lblPK_id" runat="server" Text="" Visible="false"></asp:Label>

            </td>
            <td id="tdBSHeading" runat="server">BS Heading<br />
                <asp:DropDownList ID="ddlHeading" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHeading_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td style="width:100px">Order By<br />
                <asp:TextBox ID="txtOrderBy" runat="server"  CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                <br />
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            </td>
        </tr>


    </table>

    <table>
        <tr>
            <td>
                <asp:GridView ID="gridMaster" runat="server" AutoGenerateColumns="False" CssClass="gridtable" OnRowDataBound="gridMaster_RowDataBound" OnRowCommand="gridMaster_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="S.N">
                            <ItemTemplate>
                                <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPkid" runat="server" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GL Code">
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("GL_MASTER_CODE") %>' ID="lblGlMasterCode" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Financial Statement">
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("FINANCIAL_STATEMENT") %>' ID="lblFinancialStatement" runat="server" Visible="false" />
                                <asp:Label ID="lblFS" runat="server" Text=""></asp:Label>

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GL Master Name">
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("GL_MASTER_NAME") %>' ID="lblGlMastername" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Order By">
                            <ItemTemplate>
                                <asp:Label Text='<%# Bind("ORDER_BY") %>' ID="lblOrderBy" runat="server"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgEdit" runat="server" CommandName="View" ImageUrl="~/images/icons/edit.png" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>
            </td>
        </tr>
    </table>


</asp:Content>

