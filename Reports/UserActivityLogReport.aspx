<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserActivityLogReport.aspx.cs" Inherits="reports_UserActivityLogReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">

        function printPartOfPage() {
            var printContent = document.getElementById('print_div');
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
    <div class="row" id="divBranch" runat="server">
        <div class="col-md-3">
            Branch<br />
            <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
    </div>
    <table>
        <tr>
            <td>From Date:</td>
            <td>
                <asp:TextBox ID="txtFDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                
            </td>
            <td>&nbsp;</td>
            <td>To Date:</td>
            <td>
                <asp:TextBox ID="txtTDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>
               
            </td>
       

            <td>User</td>
            <td>
                <asp:DropDownList ID="ddlUser" runat="server" CssClass="form-control">
                </asp:DropDownList></td>
            <td>
                <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click"  CssClass="btn btn-primary"/>
            </td>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click"  CssClass="btn btn-primary"/>
            </td>
        </tr>

    </table>
    <br>
    <div id="hide" runat="server" visible="false">
        <div id="print_div">
            <table style="width: 100%">
                <tr>
                    <td style="width: 20%; text-align: center" rowspan="4">
                        <asp:Image ID="Image1" runat="server" Height="80px" ImageUrl="~/images/img.png" />
                    </td>
                    <td style="text-align: center;">

                        <span style="font-weight: bold; font-size: 16px;">
                            <asp:Label ID="lblCompanyName" runat="server" Text=""></asp:Label></span>
                        <br>
                        <span style="font-size: 10pt;">
                            <asp:Label ID="lblCompanyAddress" runat="server" Text=""></asp:Label></span>
                        <br>
                        <span style="font-size: 10pt;">Website:
                                <asp:Label ID="lblWebsite" runat="server" Text=""></asp:Label>, Email:
                                <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></span>


                    </td>

                    <td style="width: 20%; text-align: right; font-size: 10pt">
                        <span></span>
                        <br>
                        <span>&#9742;
                                <asp:Label ID="lblPhone1" runat="server" Text=""></asp:Label></span>
                        <br>
                        <span>&#9742;
                                <asp:Label ID="lblPhone2" runat="server" Text=""></asp:Label></span>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; font-weight: bold; font-family: Verdana; font-size: 14px">Report:Activity Log Report
                    </td>
                    <td style="text-align: right; font-size: 10pt">Pan No:<asp:Label ID="lblPanNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; font-family: Verdana; font-size: 14px">From Date:<asp:Label ID="lblFDate" runat="server"></asp:Label>
                        &nbsp;To Date:<asp:Label ID="lblTDate" runat="server"></asp:Label>
                    </td>
                    <td style="text-align: right; font-size: 10pt">Reg No:<asp:Label ID="lblRegNo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; font-family: Verdana; font-size: 14px">&nbsp;</td>
                    <td style="text-align: right; font-size: 10pt">&nbsp;</td>
                </tr>
            </table>
            <div id="div_border" style="background-color: white; width: 100%; border: 1px solid black; padding: 10px; margin-bottom: 20px; box-sizing: border-box; box-shadow: 0px 2px 10px rgba(0,50,240,0.5);">
               
                <asp:GridView ID="gridLog" runat="server" AutoGenerateColumns="False" EnableModelValidation="True" Width="100%" OnRowDataBound="gridLog_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="SN">
                            <ItemTemplate>
                                <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                     
                        <asp:TemplateField HeaderText="Time">
                            <ItemTemplate>
                                <asp:Label ID="lblTime" runat="server" Text='<%# Bind("LOG_TIME") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                               
                                <asp:Label ID="lblAction" runat="server" Text='<%# Bind("LOG_ACTION") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="User">
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" ></asp:Label>
                                <asp:Label ID="lblUser" runat="server" Text='<%# Bind("LOG_USER") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Branch">
                            <ItemTemplate>
                               <asp:Label ID="lblBranchName" runat="server"  ></asp:Label>
                                <asp:Label ID="lblBranchID" runat="server" Text='<%# Bind("OFFICE_CODE") %>' Visible="false"></asp:Label>                           
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reference Table">
                            <ItemTemplate>
                                <asp:Label ID="lblTable" runat="server" Text='<%# Bind("EFFECT_TABLE") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                       
                        <asp:TemplateField HeaderText="Reference No">
                            <ItemTemplate>
                                <asp:Label ID="lblReferenceNo" runat="server" Text='<%# Bind("REF_NO") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>



