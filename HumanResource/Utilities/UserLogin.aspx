<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserLogin.aspx.cs" Inherits="HumanResource_Utilities_UserLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <table id="EmpSearch" runat="server">
            <tr>
                <td>Department<br />
                    <asp:DropDownList ID="ddlDept" Width="200px" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <td>
                    <br />
                    <asp:Button ID="bttnSearch" Width="80px" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="bttnSearch_Click" />
                </td>
            </tr>
        </table>
        <div class="row">
            <div class="col-md-8">
                <asp:GridView ID="GridEmpList" runat="server" Width="100%" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#CC9966" BorderStyle="None" CssClass="gridtable" EnableModelValidation="True"
                    OnRowDataBound="GridEmpList_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="EMP ID">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpID" runat="server" Text='<%# Bind("EMPLOYEEID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NAME">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnCreate" runat="server" ImageUrl="~/images/icons/login.png" Width="50px" ToolTip="Create Login" OnClick="imgbtnCreate_Click" />
                                <asp:ImageButton ID="imgbtnReset" runat="server" ImageUrl="~/images/icons/reset.png" Width="50px" ToolTip="Reset Login" OnClick="imgbtnReset_Click" />
                                <asp:ImageButton ID="imgbtnBlock" runat="server" ImageUrl="~/images/icons/block.png" Width="50px" ToolTip="Block Login"  OnClick="imgbtnBlock_Click" />
                                <asp:ImageButton ID="imgbtnUnBlock" runat="server" ImageUrl="~/images/icons/unblock.png" Width="50px"  ToolTip="Unblock Login" OnClick="imgbtnUnBlock_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Group">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlGroup" CssClass="form-control" runat="server"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Change Group">
                            <ItemTemplate>
                                <asp:Button ID="btnChangeGroup" runat="server" CssClass="btn btn-primary" Text="Change" OnClick="btnChangeGroup_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="col-md-6">
            </div>
        </div>

    </div>
</asp:Content>

