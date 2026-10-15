<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RemainingLeave.aspx.cs" Inherits="HR_Attendance_Reports_RemainingLeave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td>Department       <br />     
                <asp:DropDownList ID="ddlDept" AutoPostBack="true" Width="200px" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>Division<br />
                <asp:DropDownList AutoPostBack="true" CssClass="uc_formcontrol" runat="server" Width="200px" ID="ddlDivision" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"></asp:DropDownList>
            </td>
            <td>Unit:<br />
                <asp:DropDownList AutoPostBack="true" CssClass="uc_formcontrol" runat="server" Width="200px" ID="ddlUnit" OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged"></asp:DropDownList>
            </td>  
            <td>Employee<br />
                <asp:DropDownList runat="server" Width="200px" ID="ddlEmployee"></asp:DropDownList>
            </td>   
             <td>Year<br />
                 <asp:TextBox ID="txtYear" runat="server"></asp:TextBox>
            </td>           
            <td>
                <br />
                <asp:Button ID="bttnView" Width="80px" CssClass="btn btn-primary" runat="server" Text="View" OnClick="bttnView_Click"  />
            </td>
        </tr>
    </table>
    <asp:GridView ID="grdRemainindLeave" runat="server" AutoGenerateColumns="False" CssClass="gridtable">
        <Columns>
            <asp:TemplateField HeaderText="Sno">
                <ItemTemplate>
                                    <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Leave Type">
                <ItemTemplate>
                    <asp:Label ID="lblLeaveType" runat="server" Text='<%# Bind("LEAVE_TYPE") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Alloted">
                 <ItemTemplate>
                    <asp:Label ID="lblLeaveType" runat="server" Text='<%# Bind("ALLOTED") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Utilized">
                 <ItemTemplate>
                    <asp:Label ID="lblUtilize" runat="server" Text='<%# Bind("UTILIZED") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Remaining">
                 <ItemTemplate>
                    <asp:Label ID="lblRemaining" runat="server" Text='<%# Bind("REMAININGDAYS") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

