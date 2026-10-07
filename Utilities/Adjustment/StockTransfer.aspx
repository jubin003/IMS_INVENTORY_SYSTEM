<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockTransfer.aspx.cs" Inherits="Reports_Adjustment_StockTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="border: 1px solid; width: 100%" runat="server" id="divVoucherEntry" class="gridtable">
        <tr>
            <th>Stock Transfer
            </th>
        </tr>
        <tr>
            <td>Adjustment Date:                   
             <asp:TextBox ID="txtAdjustmentDate" runat="server" Width="150px" CssClass="form-control datepicker" ReadOnly="false"></asp:TextBox></td>
            <%-- <td></td>
            <td style="width: 355px"></td>
            <td></td>
            <td></td>
            <td></td>--%>
        </tr>
        <tr>
            <td style="width: 100%">
                <table style="width: 100%" class="gridtable table-bordered">
                    <tr>
                        <td>
                            <h3>Transfer By:</h3>
                        </td>
                        <td>Product Category<br />
                            <asp:DropDownList ID="ddlCategoryFilter" runat="server" CssClass="form-control"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryFilter_SelectedIndexChanged">
                            </asp:DropDownList></td>
                        <td>Product Sub Category
                    <br />
                            <asp:DropDownList ID="ddlSubCategoryFilter" runat="server" CssClass="form-control"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategoryFilter_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>


                        <td style="width: 355px">Product Name<br />
                            <asp:DropDownList ID="ddlProductBy" OnTextChanged="ddlProduct_TextChanged" AutoPostBack="true" Width="350px" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                            <script>
                                $('#<%=ddlProductBy.ClientID%>').chosen();
                            </script>
                        </td>
                        <td runat="server" visible="true">In Stock<br />
                            <asp:TextBox ID="txtAvilableQty" runat="server" CssClass="form-control" Width="80px" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <br />
                            <asp:Label ID="lblBUnit" runat="server" Text=""></asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <h3>Transfer To:</h3>

                        </td>
                        <td>Product Category<br />
                            <asp:DropDownList ID="ddlProCategory" OnSelectedIndexChanged="ddlProCategory_SelectedIndexChanged" runat="server" CssClass="form-control"
                                AutoPostBack="true">
                            </asp:DropDownList></td>
                        <td>Product Sub Category
                    <br />
                            <asp:DropDownList ID="ddlProSubCategory" OnSelectedIndexChanged="ddlProSubCategory_SelectedIndexChanged" runat="server" CssClass="form-control"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>


                        <td style="width: 355px">Product Name<br />
                            <asp:DropDownList ID="ddlProductTo" AutoPostBack="true" Width="350px" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlProductTo_SelectedIndexChanged">
                            </asp:DropDownList>
                            <script>
                                $('#<%=ddlProductTo.ClientID%>').chosen();
                            </script>
                        </td>
                        <td runat="server" visible="true">In Stock<br />
                            <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" Width="80px" Enabled="false"></asp:TextBox>
                        </td>
                        <td>Basic Unit  <asp:Label ID="lblQUnit" runat="server" Text=""></asp:Label><br />
                            <asp:TextBox ID="txtToUnit" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true"></asp:TextBox>
                           
                        </td>


                    </tr>
                </table>
            </td>


        </tr>

        <tr>
            <td colspan="2">
                <asp:Label ID="Label2" runat="server" Text="Narration"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtNarration" runat="server" TextMode="MultiLine" Width="100%" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btn_save" CssClass="btn btn-primary" OnClick="btn_save_Click" runat="server" Text="Save" />
            </td>
        </tr>
    </table>

</asp:Content>

