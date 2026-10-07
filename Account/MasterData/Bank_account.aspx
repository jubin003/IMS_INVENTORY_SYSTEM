<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Bank_account.aspx.cs"
    Inherits="Account_MasterData_Bank_account" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form-group-sm container-fluid">
        <div class="row">
            <div class="col-md-2">
                Bank Name
           <br />
                <asp:DropDownList ID="ddlBanks" runat="server" OnSelectedIndexChanged="ddlBanks_SelectedIndexChanged"
                    CssClass="form-control" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="col-md-1">
                  <br />
                <asp:Button ID="btnAddBank" runat="server" Text="Add Bank" OnClick="btnAddBank_Click" CssClass="btn btn-primary" />
            </div>
        </div>

        <div id="divAddBank" visible="false" runat="server">
            <div class="row">
                <div class="col-md-2">
                    Bank Code
                <br />
                    <asp:TextBox ID="txtBankCode" runat="server" AutoPostBack="true" CssClass="form-control" OnTextChanged="txtBankCode_TextChanged"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    Branch
                <br />
                    <asp:TextBox ID="txtBranch" runat="server" CssClass="form-control"></asp:TextBox></td>
                </div>
                <div class="col-md-2">
                    Account Type
                <br />
                    <asp:DropDownList ID="ddlAccountType" runat="server" Width="100%" CssClass="form-control">
                        <asp:ListItem Value="Select">Select</asp:ListItem>
                        <asp:ListItem Value="Current">Current</asp:ListItem>
                        <asp:ListItem Value="Call">Call</asp:ListItem>
                        <asp:ListItem Value="Saving">Saving</asp:ListItem>
                        <asp:ListItem Value="Fixed">Fixed</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    Account Number
              <br />
                    <asp:TextBox ID="txtAccNumber" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    Show In Receipt
                <br />
                    <asp:DropDownList ID="ddlShowInReceipt" runat="server" Width="100%" CssClass="form-control">
                        <asp:ListItem Value="Select">Select</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    Status
               <br />
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Value="Select">Select</asp:ListItem>
                        <asp:ListItem Value="1">Available</asp:ListItem>
                        <asp:ListItem Value="0">Not Available</asp:ListItem>
                    </asp:DropDownList>
                </div>
            
                <div class="col-md-1">
                    <br />
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-sm btn-primary" />
                    <asp:Label ID="lblPK_id" runat="server" Text="" Visible="false"></asp:Label>

                </div>
            </div>
        </div>
        <table>
            <tr>
                <td>
                    <asp:GridView ID="gridBankAccount" runat="server" AutoGenerateColumns="False" CssClass="gridtable" OnRowCommand="gridBankAccount_RowCommand" OnRowDataBound="gridBankAccount_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="S.N">
                                <ItemTemplate>
                                    <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPkid" runat="server" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bank Code">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Bind("BANK_CODE") %>' ID="lblBank" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Bind("BRANCH") %>' ID="lblBranch" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account Type">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Bind("Account_Type") %>' ID="lbAccountType" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account Number">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Bind("Account_Number") %>' ID="lblAccountNumber" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Show In Reciept">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Bind("Show_In_Receipt") %>' ID="lblShowInReceipt" runat="server" Visible="false" />
                                    <asp:Label ID="lblShowInRec" runat="server" Text=""></asp:Label>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Bind("STATUS") %>' ID="lblStatus" runat="server" Visible="false" />
                                    <asp:Label ID="lblStat" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdit" CommandName="View" runat="server" ImageUrl="~/images/icons/edit.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

