<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Duty_Shift.aspx.cs" Inherits="HR_Attendance_Duty_Hour_Duty_Shift" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updGrdWrkSchedule" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="grdWrkSchedule" Width="100%" runat="server" CssClass="gridtable" AutoGenerateColumns="False"
                OnRowDataBound="grdWrkSchedule_RowDataBound"
                OnRowCancelingEdit="grdWrkSchedule_RowCancelingEdit"
                OnRowCommand="grdWrkSchedule_RowCommand"
                OnRowEditing="grdWrkSchedule_RowEditing"
                OnRowUpdating="grdWrkSchedule_RowUpdating">
                <Columns>
                    <asp:TemplateField HeaderText="S.no">
                        <ItemTemplate>
                            <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblSn" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Shift">
                        <HeaderTemplate>
                            Shift<br />
                            <asp:TextBox ID="txtShiftH" runat="server"></asp:TextBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblShift" runat="server" Text='<%# Bind("TYPE") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtShiftE" runat="server" Text='<%# Bind("TYPE") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Code">
                        <HeaderTemplate>
                            Code<br />
                            <asp:TextBox ID="txtCodeH" runat="server" AutoPostBack="true" OnTextChanged="txtCodeH_TextChanged"></asp:TextBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCodeE" runat="server" Text='<%# Bind("Code") %>' AutoPostBack="true" OnTextChanged="txtCodeE_TextChanged"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="IN Time">
                        <HeaderTemplate>
                            In_Time<br />
                            <asp:TextBox ID="txtInHrH" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="TimeComponent_Changed"></asp:TextBox>&nbsp;:
                            <asp:TextBox ID="txtInMinH" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="TimeComponent_Changed"></asp:TextBox>
                            <asp:DropDownList ID="ddlInAMPMH" runat="server" Width="50px" OnTextChanged="TimeComponent_Changed">
                                <asp:ListItem Text="AM" Value="AM" />
                                <asp:ListItem Text="PM" Value="PM" />
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblIn_Time" runat="server" Text='<%# Bind("IN_TIME") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtInHr" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="TimeComponentI_Changed"></asp:TextBox>&nbsp;:
                            <asp:TextBox ID="txtInMin" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="TimeComponentI_Changed"></asp:TextBox>

                            <asp:DropDownList ID="ddlInAMPM" Width="60px" runat="server" AutoPostBack="true" OnTextChanged="TimeComponentI_Changed">
                                <asp:ListItem Text="AM" Value="AM" />
                                <asp:ListItem Text="PM" Value="PM" />
                            </asp:DropDownList>
                            <asp:Label ID="lblIn_Time" runat="server" Text='<%# Bind("IN_TIME") %>' Visible="false"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Out Time">
                        <HeaderTemplate>
                            Out_Time<br />
                            <asp:TextBox ID="txtOutHrH" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="TimeComponent_Changed"></asp:TextBox>&nbsp;:
                            <asp:TextBox ID="txtOutMinH" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="TimeComponent_Changed"></asp:TextBox>
                            <asp:DropDownList ID="ddlOutAMPMH" runat="server" Width="50px" AutoPostBack="true" OnTextChanged="TimeComponent_Changed">
                                <asp:ListItem Text="AM" Value="AM" />
                                <asp:ListItem Text="PM" Value="PM" />
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblOut_Time" runat="server" Text='<%# Bind("OUT_TIME") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtOutHr" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="TimeComponentI_Changed"></asp:TextBox>&nbsp;:
                            <asp:TextBox ID="txtOutMin" runat="server" Width="30px" Text="00" AutoPostBack="true" OnTextChanged="TimeComponentI_Changed"></asp:TextBox>
                            <asp:DropDownList ID="ddlOutAMPM" Width="60px" runat="server" AutoPostBack="true" OnTextChanged="TimeComponentI_Changed">
                                <asp:ListItem Text="AM" Value="AM" />
                                <asp:ListItem Text="PM" Value="PM" />
                            </asp:DropDownList>
                            <asp:Label ID="lblOtTime" runat="server" Text='<%# Bind("OUT_TIME") %>' Visible="false"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Duty Hour">
                        <HeaderTemplate>
                            Duty Hour<br />
                            <asp:TextBox ID="txtDutyHrH" runat="server" Text="0" AutoPostBack="true"></asp:TextBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDutyHr" runat="server" Text='<%# Bind("DUTY_HOUR") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDutyHrE" runat="server" Text='<%# Bind("DUTY_HOUR") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status">
                        <HeaderTemplate>
                            Status<br />
                            <asp:DropDownList ID="ddlStatusH" runat="server">
                                <asp:ListItem Text="Available" Value="1" />
                                <asp:ListItem Text="Unavailable" Value="0" />
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblShowStatus" runat="server"></asp:Label>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlStatusE" runat="server">
                                <asp:ListItem Text="Available" Value="1" />
                                <asp:ListItem Text="Unavailable" Value="0" />
                            </asp:DropDownList>
                            <asp:Label ID="lblStatusE" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Save">
                        <HeaderTemplate>
                            <asp:Button ID="btnSaveH" runat="server" Text="Save" Width="80px" OnClick="btnSaveH_Click" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Button ID="Edit" runat="server" Width="80px" CommandName="edit" Text="Edit" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnSaveE" runat="server" Width="80px" CommandName="Update" Text="Save" />
                            <asp:Button ID="btnCancel" runat="server" Width="80px" CommandName="Cancel" Text="Cancel" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
           <%-- <asp:AsyncPostBackTrigger ControlID="grdWrkSchedule" EventName="RowEditing" />
            <asp:AsyncPostBackTrigger ControlID="grdWrkSchedule" EventName="RowUpdating" />
            <asp:AsyncPostBackTrigger ControlID="grdWrkSchedule" EventName="RowCancelingEdit" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


