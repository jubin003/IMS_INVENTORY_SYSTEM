<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="AddDeviceID.aspx.cs" Inherits="HR_Attendance_Attendance_AddDeviceID" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table id="EmpSearch" width="100%" runat="server">
        <tr>
            <td>
                <table style="width: 40%">
                    <tr>
                        <td>Department
                            <asp:DropDownList ID="ddlDept" AutoPostBack="true" Width="200px" runat="server" CssClass="form-control" >
                            </asp:DropDownList>
                        </td>
                        <td>Status:
                            <asp:DropDownList ID="ddlStatus" Width="100px" runat="server" CssClass="form-control">
                                <asp:ListItem Selected="True" Value="1">Active</asp:ListItem>
                                <asp:ListItem Value="0">InActive</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <br />
                            <asp:Button ID="bttnSearch" Width="80px" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="bttnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <table>
        <tr>
            <td>

                <asp:GridView runat="server" Width="100%" AutoGenerateColumns="False"
                    EnableModelValidation="True" ID="gridMappingID" AllowSorting="True" CssClass="gridtable" OnRowDataBound="gridMappingID_RowDataBound">
                    <Columns>

                        <asp:TemplateField HeaderText="Employee ID">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeID" runat="server" Text='<%# Bind("EMPLOYEEID") %>'></asp:Label>
                                <asp:Label ID="lblPK_ID" runat="server" Visible="false" Text='<%# Bind("PK_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblFullName" runat="server" Text='<%# Bind("FULL_NAME") %>' Width="100%"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Mapping ID">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDeviceID" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtDeviceID_TextChanged"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Same as Employee ID" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <%--<asp:ImageButton ID="ImgAsEmp" runat="server" ImageUrl="~/images/icons/Plus.gif" OnClick="ImgAsEmp_Click"/>--%>
                                <asp:CheckBox ID="chckAsEmp" runat="server" OnCheckedChanged="chckAsEmp_CheckedChanged" AutoPostBack="true"/>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>

                <br />

                <asp:Button ID="bttnSave" runat="server" Text="Save" CssClass="btn btn-success" OnClick="bttnSave_Click" width="10%"/>

            </td>
        </tr>
    </table>

</asp:Content>
