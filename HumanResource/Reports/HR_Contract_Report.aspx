<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HR_Contract_Report.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="HumanResource_Reports_HR_Contract_Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function printPartOfPage() {
            var printContent = document.getElementById('div_print');
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=0,top=0,width=0,height=0');

            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            //printWindow.close();
        }
    </script>
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 40%">
                <tr>

                    <td>Department
            
                <asp:DropDownList ID="ddlDept" AutoPostBack="true" Width="200px" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged1">
                </asp:DropDownList>
                    </td>
                    <td>Designation:<br />
                        <asp:DropDownList AutoPostBack="true" CssClass="uc_formcontrol" runat="server" Width="200px" ID="ddlDesignation"></asp:DropDownList>
                    </td>
                    <td>Status<br />

                        <asp:DropDownList ID="ddlStatus" Width="150px" runat="server" CssClass="form-control">
                            <asp:ListItem Selected="True" Value="1">All</asp:ListItem>
                            <asp:ListItem Value="2">Ongoing</asp:ListItem>
                            <asp:ListItem Value="3">Nearly Expired</asp:ListItem>
                            <asp:ListItem Value="0">Expired</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <br />
                        <asp:Button ID="bttnSearch" Width="80px" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="bttnSearch_Click" />
                    </td>
                    <td>
                        <br />
                        <asp:Button ID="btnPrintEmp" runat="server" Text="Print" Visible="false" OnClick="btnPrintEmp_Click" />
                    </td>
                </tr>
            </table>

            <div id="div_print">
                <table id="tblGrid" style="width: 100%" runat="server" visible="false">
                    <tr>
                        <td>
                            <div style="text-align: center;">
                                <strong><span style="font-size: medium">Employee Contract Report</span></strong><br />
                                <span style="font-size: medium"><strong>As of</strong></span><span style="font-size: small"> </span>
                                <strong>
                                    <asp:Label ID="lblDate" runat="server" Text="Label" Style="font-size: medium"></asp:Label>
                                </strong>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grdAllContract" Visible="true" CssClass="gridtable" Width="100%" runat="server" AutoGenerateColumns="False" OnRowDataBound="grdAllContract_RowDataBound">
                                <Columns>

                                    <asp:TemplateField HeaderText="S.no">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSN_A" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="EMP_ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpID_A" runat="server" Text='<%#Bind("EMP_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName_A" runat="server" Text='<%#Bind("EMP_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDept_A" runat="server" Text='<%#Bind("DEP_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignation_A" runat="server" Text='<%#Bind("DESIGNATION_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Level">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLevel_A" runat="server" Text='<%#Bind("LEVEL_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNepStDate_A" runat="server" Text='<%#Bind("START_NEP_DATE") %>'></asp:Label><br />
                                            [<asp:Label ID="lblStDate_A" runat="server" Text='<%#Bind("START_DATE_AD") %>'></asp:Label>]
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNepEndDate_A" runat="server" Text='<%#Bind("END_NEP_DATE") %>'></asp:Label><br />
                                            [<asp:Label ID="lblEndDate_A" runat="server" Text='<%#Bind("END_DATE_AD") %>'></asp:Label>]
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remaining Days">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemainingDate_A" runat="server" Text='<%#Bind("REMAINING_DAYS") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

