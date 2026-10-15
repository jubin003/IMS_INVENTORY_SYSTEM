<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="hr_department.aspx.cs" Inherits="human_resource_Master_data_hr_department" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updGrdWrkSchedule" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView runat="server" CssClass="gridtable" ID="gridDepartment" AutoGenerateColumns="False" EnableModelValidation="True"
                OnRowCancelingEdit="gridDepartment_RowCancelingEdit" OnRowDataBound="gridDepartment_RowDataBound"
                OnRowEditing="gridDepartment_RowEditing" OnRowUpdating="gridDepartment_RowUpdating">
                <Columns>
                    <asp:TemplateField HeaderText="SN">
                        <ItemTemplate>
                            <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Department Code">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDeptCodeE" runat="server" Text='<%# Bind("DEP_CODE") %>' AutoPostBack="true" OnTextChanged="txtDeptCodeE_TextChanged"></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            Department Code<br />
                            <asp:TextBox ID="txtDeptCodeH" runat="server" AutoPostBack="true" OnTextChanged="txtDeptCodeH_TextChanged"></asp:TextBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDeptCode" runat="server" Text='<%# Bind("DEP_CODE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Department Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDeptNameE" runat="server" Text='<%# Bind("DEP_NAME") %>'></asp:TextBox>
                            <asp:Label ID="lblPKIDU" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            Department Name<br />
                            <asp:TextBox ID="txtDeptNameH" runat="server"></asp:TextBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDeptName" runat="server" Text='<%# Bind("DEP_NAME") %>'></asp:Label>
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
                    <asp:TemplateField HeaderText="Division Exist Status">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlDivisionStatusE" runat="server">

                                <asp:ListItem Value="1">Available</asp:ListItem>
                                <asp:ListItem Value="0">Unavailable</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblDivisionStatusE" runat="server" Text='<%# Bind("DIVISION_EXIST_STATUS") %>' Visible="False"></asp:Label>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            Division Exist Status<br />
                            <asp:DropDownList ID="ddlDivisionStatusH" runat="server">

                                <asp:ListItem Value="1">Available</asp:ListItem>
                                <asp:ListItem Value="0">Unavailable</asp:ListItem>
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDivisionStatus" runat="server" Text='<%# Bind("DIVISION_EXIST_STATUS") %>' Visible="False"></asp:Label>
                            <asp:Label ID="lblDivisionStat" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remarks">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRemarksE" Text='<%# Bind("REMARKS") %>' runat="server">                    
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

