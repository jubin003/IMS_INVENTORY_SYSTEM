<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="hr_division.aspx.cs" Inherits="HumanResource_MasterData_hr_division" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td>Department:
                <asp:DropDownList AutoPostBack="true" CssClass="uc_formcontrol" runat="server" ID="ddlDept" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:GridView runat="server" CssClass="gridtable" ID="gridDivision" AutoGenerateColumns="False" EnableModelValidation="True"
                OnRowCancelingEdit="gridDivision_RowCancelingEdit" OnRowDataBound="gridDivision_RowDataBound"
                OnRowEditing="gridDivision_RowEditing" OnRowUpdating="gridDivision_RowUpdating">
                <Columns>
                    <asp:TemplateField HeaderText="SN">
                        <ItemTemplate>
                            <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Division Code">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDivisionCodeE" runat="server" Text='<%# Bind("DIVISION_CODE") %>' AutoPostBack="true" OnTextChanged="txtDivisionCodeE_TextChanged"></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            Division Code<br />
                            <asp:TextBox ID="txtDivisionCodeH" runat="server" AutoPostBack="true" OnTextChanged="txtDivisionCodeH_TextChanged"></asp:TextBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDivisionCode" runat="server" Text='<%# Bind("DIVISION_CODE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Division Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDivisionNameE" runat="server" Text='<%# Bind("DIVISION_NAME") %>'></asp:TextBox>
                            <asp:Label ID="lblPKIDU" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            Division Name<br />
                            <asp:TextBox ID="txtDivisionNameH" runat="server"></asp:TextBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDivisionName" runat="server" Text='<%# Bind("DIVISION_NAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlStatusE" runat="server">

                                <asp:ListItem Value="1">Available</asp:ListItem>
                                <asp:ListItem Value="0">Unavailable</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblStatusE" runat="server" Text='<%# Bind("STATUS") %>' Visible="False"></asp:Label>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            Status<br />
                            <asp:DropDownList ID="ddlStatusH" runat="server">

                                <asp:ListItem Value="1">Available</asp:ListItem>
                                <asp:ListItem Value="0">Unavailable</asp:ListItem>
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Visible="False"></asp:Label>
                            <asp:Label ID="lblStat" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Unit Exist Status">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlUnitStatusE" runat="server">
                                <asp:ListItem Value="1">Available</asp:ListItem>
                                <asp:ListItem Value="0">Unavailable</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblUnitStatusE" runat="server" Text='<%# Bind("UNIT_EXIST_STATUS") %>' Visible="False"></asp:Label>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            Unit  Status<br />
                            <asp:DropDownList ID="ddlUnitStatusH" runat="server">
                                <asp:ListItem Value="1">Available</asp:ListItem>
                                <asp:ListItem Value="0">Unavailable</asp:ListItem>
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUnitStatus" runat="server" Text='<%# Bind("UNIT_EXIST_STATUS") %>' Visible="False"></asp:Label>
                            <asp:Label ID="lblUnitStat" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remarks">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRemarksE" runat="server" Text='<%# Bind("REMARKS") %>'>                    
                            </asp:TextBox>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            Remarks<br />
                            <asp:TextBox ID="txtRemarksH" runat="server">                       
                            </asp:TextBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("REMARKS") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>

                        <EditItemTemplate>
                            <asp:ImageButton ID="imgUpdate" runat="server" CommandName="Update" ImageUrl="~/images/icons/upload.png" />
                            <asp:ImageButton ID="imgCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/icons/delete.gif" />
                        </EditItemTemplate>
                        <HeaderTemplate>
                            <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgEdit" runat="server" CommandName="Edit" ImageUrl="~/images/icons/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

