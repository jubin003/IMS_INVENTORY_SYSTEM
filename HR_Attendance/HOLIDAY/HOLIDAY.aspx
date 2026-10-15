<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HOLIDAY.aspx.cs" Inherits="HR_Attendance_HOLIDAY_HOLIDAY" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('[id*=lstDivision]').multiselect({
                includeSelectAllOption: true
            });
            $("#Button1").click(function () {
                alert($(".multiselect-selected-text").html());
            });
        });
    </script>

    <script src='<%= ResolveUrl("~/multiselectdropdown/jquery.min.js") %>'></script>
    <script src='<%= ResolveUrl("~/multiselectdropdown/bootstrap.min.js") %>'></script>
    <script src='<%= ResolveUrl("~/multiselectdropdown/bootstrap-multiselect.min.js") %>'></script>

    <link href='<%= ResolveUrl("~/multiselectdropdown/bootstrap-multiselect.css") %>' rel="stylesheet" />
    <link href='<%= ResolveUrl("~/multiselectdropdown/bootstrap.min.css") %>' rel="stylesheet" />


    <div style="width: 100vw; height: 100vh;">
        <table class="gridtable">
            <tr>
                <td>Year:
                    <asp:DropDownList ID="ddlYear" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button Text="View" ID="btnView" OnClick="btnView_Click" runat="server" />
                </td>
                <td>
                    <asp:Button Text="Add" ID="btnAdd" OnClick="btnAdd_Click" runat="server" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td id="tblGrid" runat="server" visible="false" style="width: auto  ; vertical-align: top; padding-right:50px">
                    <asp:GridView ID="gridHoliday" Width="100%" CssClass="gridtable"
                        OnRowDataBound="gridHoliday_RowDataBound"
                        OnRowCommand="gridHoliday_RowCommand"
                        AutoGenerateColumns="false" runat="server">
                        <HeaderStyle BackColor="Silver" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label Text="SN" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblHolidayPK" Visible="false" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label Text="Holiday" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblHolidayMasterID" Width="250px" Visible="false" runat="server" Text='<%# Bind("HOLIDAY_MASTER_ID") %>'></asp:Label>
                                    <asp:Label ID="lblHolidayName" Width="200px" runat="server" Text=''></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label Text="Applicable Division" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblApplicableFor" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label Text="Gender" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label Text="Date" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNepDate" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ImageUrl="~/images/icons/view.png" CommandName="View" ToolTip="View" ID="imgbtnView" runat="server" />
                                    <asp:ImageButton ID="ImgBtnEdit" runat="server" CommandName="Change" ImageUrl="~/images/icons/edit.png" ToolTip="Edit" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </td>
                <td id="tblAdd" runat="server" visible="false" style="width: 600px">
                    <asp:Panel ID="UpdatePanel" runat="server">
                        <table style="width: 100%; border: 1px solid #ccc; border-radius: 5px; background: #f9fcff; padding: 10px; font-family: Segoe UI, sans-serif; box-shadow: 0 0 8px rgba(0,0,0,0.1);">
                            <tr>
                                <td colspan="4" style="background-color: #d9edf7; padding: 8px; font-weight: bold; color: #31708f;">
                                    <asp:Label Text="Add Holiday" ID="lblAddHeading" Visible="false" runat="server" />
                                    <asp:Label Text="Holiday Info" ID="lblHolidayInfoHeading" Visible="false" runat="server" />
                                    <asp:Label Text="Edit Holiday" ID="lblEditHeading" Visible="false" runat="server" />
                                    <asp:Label Text="" ID="lblHolidayID" Visible="false" runat="server" />

                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px; padding-left: 10px">Holiday Name</td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlHolidayName" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHolidayName" runat="server" placeholder="..add Other Holiday" />
                                </td>
                            </tr>
                            <tr runat="server" visible="false">
                                <td colspan="4">
                                    <table style="width: 100%; float: left; margin-top: 5px; margin-bottom: 5px;" >
                                        <tr>
                                            <td style="padding-left: 10px; width: 19%;">Applicable For:</td>
                                            <td style="width: 25%; padding-left: 5px">
                                                <asp:DropDownList ID="ddlApplicableFor" AutoPostBack="true" OnSelectedIndexChanged="ddlApplicableFor_SelectedIndexChanged" runat="server">
                                                    <asp:ListItem Value="All">All</asp:ListItem>
                                                    <asp:ListItem Value="Selected">Selected</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="padding-left: 10px; width: 54%;">
                                                <asp:ListBox ID="chkDivision" SelectionMode="Multiple" CssClass="multiselect" Width="100%" ClientIDMode="Static" runat="server" />

                                                <script>
                                                    $(function () {
                                                        $('#chkDivision').multiselect({
                                                            buttonWidth: '100%',
                                                            nonSelectedText: 'Select Division(s)'
                                                            // includeSelectAllOption: true,
                                                            // enableFiltering: true
                                                        });
                                                    });
                                                </script>
                                            </td>

                                        </tr>
                                    </table>
                                </td>

                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3"></td>
                            </tr>
                            <tr>
                                <td style="padding-left: 10px">Date:</td>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtDate" AutoCompleteTypeF="None" placeholder="DD/MM/YYYY" runat="server"></asp:TextBox>

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding-left: 10px">
                                    <strong>Gender:</strong><br />
                                    <asp:RadioButtonList ID="rbtnGender" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                        <asp:ListItem Selected="True" Text="Both" Value="Both" />
                                        <asp:ListItem Text="Male" Value="Male" />
                                        <asp:ListItem Text="Female" Value="Female" />
                                    </asp:RadioButtonList>

                                </td>
                                <td></td>
                                <td style="padding-left: 10px"><strong>Status:</strong><br />
                                    <asp:RadioButtonList ID="rbtnStatus" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Selected="True" Text="Active" />
                                        <asp:ListItem Value="0" Text="Inactive" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="padding-left: 10px; padding-bottom:10px">
                                    <table>
                                        <tr>
                                            <td style="padding-left: 4px">
                                                <asp:Button ID="btnAddHoliday" runat="server" Text="Add" Width="100px" OnClick="btnAddHoliday_Click" />
                                            </td>
                                            <td style="padding-left: 4px">
                                                <asp:Button ID="btnUpdateHoliday" OnClick="btnUpdateHoliday_Click" Width="100px" runat="server" Text="Update" />
                                            </td>
                                            <td style="padding-left: 4px">
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" OnClick="btnCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        window.onload = function () {
            // Initialize all datepickers with their respective formats
            document.getElementById("<%= txtDate.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
        };
    </script>
</asp:Content>

