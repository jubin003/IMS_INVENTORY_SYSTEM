<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="opening_balance.aspx.cs" Inherits="Account_MasterData_opening_balance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr id="trBranch" runat="server">
            <td colspan="2">Branch<br />
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
                <asp:Label ID="lblBranchFY" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
    <table class="gridtable" id="formTbl" runat="server" style="width: 1000px;">
        <tr>
            <td style="width: 150px">Fiscal Year<br />
                <asp:DropDownList ID="ddlFiscalYear" CssClass="form-control" runat="server"></asp:DropDownList>
            </td>
            <td style="width: 150px">GL Account Head
            <br />
                <asp:DropDownList ID="ddlAccountsHead" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAccountsHead_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style="width: 200px">GL Account Master 
                <br />
                <asp:DropDownList ID="ddlGLAccMaster" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGLAccMaster_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td style="width: 200px">General Ledger 
                <br />
                <asp:DropDownList ID="ddlGl" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGl_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <%--  <%--<td id="tdSubGl" runat="server" visible="false">Sub GL Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:DropDownList ID="ddlSubGlName" Width="150px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSubGlName_SelectedIndexChanged"></asp:DropDownList>
                <%--<asp:Label ID="lblPK_id" runat="server" Text="" Visible="false"></asp:Label>--%>
        </tr>
    </table>
    <table id="grdTbl" runat="server">
        <tr>
            <td>
                <asp:GridView ID="gridOpeningBalance" runat="server" CssClass="gridtable" AutoGenerateColumns="False" Width="1000px"
                    OnRowDataBound="gridOpeningBalance_RowDataBound" OnRowCommand="gridOpeningBalance_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="S.N">
                            <ItemTemplate>
                                <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPK_ID" runat="server" Visible="false" />
                                <asp:Label ID="lblPK_ID_L" runat="server" Visible="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="General Ledger Name">
                            <ItemTemplate>
                                <asp:Label ID="lblGlCode" runat="server" Text='<%# Bind("GL_Code") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblGLName" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ledger Code">
                            <ItemTemplate>
                                <asp:Label ID="lblSubGlCode" Text='<%# Bind("SUB_GL_CODE") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ledger Name">
                            <ItemTemplate>
                                <asp:Label ID="lblSubGlName" Text='<%# Bind("SUB_GL_NAME") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount Type">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDrCr" CssClass="form-control" runat="server">
                                    <asp:ListItem Text="Debit" Value="debit" />
                                    <asp:ListItem Text="Credit" Value="credit" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDrCrAmt" Text="" runat="server" TextMode="Number" CssClass="form-control" Style="text-align: right;"></asp:TextBox>
                                <%--  <asp:Label Text='<%# Bind("DR_AMOUNT") %>' ID="lblDrAmount" runat="server" Visible="false" />
                                <asp:Label Text='<%# Bind("CR_AMOUNT") %>' ID="lblCrAmount" runat="server" Visible="false" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Save">
                            <ItemTemplate>
                                <asp:Button runat="server" Text="Save" CommandName="SaveRow" CssClass="btn btn-primary" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
                <asp:GridView ID="gridGlOpeningBalance" runat="server" CssClass="gridtable" AutoGenerateColumns="False" Width="1000px"
                    OnRowDataBound="gridGlOpeningBalance_RowDataBound" OnRowCommand="gridGlOpeningBalance_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="S.N">
                            <ItemTemplate>
                                <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPK_ID" runat="server" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="General Ledger Name">
                            <ItemTemplate>
                                <asp:Label ID="lblGlCode" Text='<%# Bind("GL_NAME") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount Type">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDrCr" CssClass="form-control" runat="server">
                                    <asp:ListItem Text="Debit" Value="debit" />
                                    <asp:ListItem Text="Credit" Value="credit" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDrCrAmt" runat="server" TextMode="Number" CssClass="form-control" Style="text-align: right;"></asp:TextBox>
                                <asp:Label Text='<%# Bind("DR_AMOUNT") %>' ID="lblDrAmount" runat="server" Visible="false" />
                                <asp:Label Text='<%# Bind("CR_AMOUNT") %>' ID="lblCrAmount" runat="server" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Save">
                            <ItemTemplate>
                                <asp:Button runat="server" Text="Save" CommandName="SaveRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
       
    </table>

    <table id="tblAlert" runat="server" visible="false">
        <tr>
            <td>
                <span style="font-size: large; color: red"><strong>You cannot enter Opening balance for this branch</strong></span>.
            </td>
        </tr>
    </table>
</asp:Content>

