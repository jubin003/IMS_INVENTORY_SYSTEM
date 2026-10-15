<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Device_Info.aspx.cs" Inherits="HR_Attendance_Attendance_Device_Info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="grdAttendance" runat="server" CssClass="gridtable" AutoGenerateColumns="False" OnRowCancelingEdit="grdAttendance_RowCancelingEdit" OnRowDataBound="grdAttendance_RowDataBound" OnRowEditing="grdAttendance_RowEditing" OnRowUpdating="grdAttendance_RowUpdating">
        <Columns>

            <asp:TemplateField HeaderText="S.no">
                <ItemTemplate>
                    <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="Machine No">
                <HeaderTemplate>Machine No <br />
                    <asp:TextBox ID="txtMachineNoH" runat="server"></asp:TextBox>
                </HeaderTemplate>
                  <ItemTemplate>
                    <asp:Label ID="lblMachineNoI" runat="server" Text='<%# Bind("MACHINE_NO") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="lblPK_IDE" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                    <asp:TextBox ID="txtMachineNoE" runat="server" Text='<%# Bind("MACHINE_NO") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="IP Address">
                  <HeaderTemplate>IP Address <br />
                    <asp:TextBox ID="txtIPAddrH" runat="server"></asp:TextBox>
                </HeaderTemplate>
                  <ItemTemplate>
                    <asp:Label ID="lblIPAddrI" runat="server" Text='<%# Bind("IP_ADDRESS") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtIPAddrE" runat="server" Text='<%# Bind("IP_ADDRESS") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="Location">
                  <HeaderTemplate>Location <br />
                    <asp:TextBox ID="txtLocationH" runat="server"></asp:TextBox>
                </HeaderTemplate>
                  <ItemTemplate>
                    <asp:Label ID="lblLocationI" runat="server" Text='<%# Bind("LOCATION") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtLocationE" runat="server" Text='<%# Bind("LOCATION") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="Status">
                  <HeaderTemplate>Status<br />
                      <asp:DropDownList ID="ddlStatusH" runat="server">
                          <asp:ListItem Text="Available" Value="1"/>
                          <asp:ListItem Text="Available" Value="1"/>
                      </asp:DropDownList>
                </HeaderTemplate>
                  <ItemTemplate>
                    <asp:Label ID="lblShowStatus" runat="server" ></asp:Label>
                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                   <asp:DropDownList ID="ddlStatusE" runat="server">
                          <asp:ListItem Text="Available" Value="1"/>
                          <asp:ListItem Text="Available" Value="1"/>
                      </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Add">
                <HeaderTemplate>
                    <asp:Button ID="btnAdd" runat="server"  Text="Add" OnClick="btnAdd_Click"/>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit" ImageUrl="~/images/icons/edit.png"  />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:ImageButton ID="btnUpdate" runat="server" CommandName="Update" ImageUrl="~/images/icons/upload.png"  />
                    <asp:ImageButton ID="btnCancel" runat="server"  CommandName="Cancel" ImageUrl="~/images/icons/cancel.png" />
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

