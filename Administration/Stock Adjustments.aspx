<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Stock Adjustments.aspx.cs" Inherits="Utilities_New_Adjustment_Stock_Adjustments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="width: 1300px">
        <tr id="divBranch" runat="server">
            <td colspan="2">
                Branch<br />
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Adjustment Date<br />
                <asp:TextBox ID="txtAdjustDate" class="form-control  datepicker" runat="server"></asp:TextBox>
            </td>
            <td>Fiscal Year<br />
                <asp:TextBox ID="txtFiscalYear" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Code<br />
                <asp:TextBox ID="txtProductCode" runat="server" CssClass="form-control" Width="80px"
                    AutoPostBack="true" OnTextChanged="txtProductCode_TextChanged"></asp:TextBox>
            </td>
            <td>Product Name<br />
                <asp:DropDownList ID="ddlProductName" runat="server" CssClass="form-control" Width="200px"
                    Font-Size="Larger" AutoPostBack="true" OnSelectedIndexChanged="ddlProductName_SelectedIndexChanged">
                </asp:DropDownList>
                <script>
                    $('#<%=ddlProductName.ClientID%>').chosen();
                </script>
            </td>
            <td runat="server" id="divBatch">Batch<br />
                <asp:DropDownList ID="ddlBatch" Width="80px" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                <asp:TextBox ID="txtExpDate" runat="server" Visible="false"></asp:TextBox>
            </td>
            <td runat="server" visible="true">Available Qty<br />
                <asp:TextBox ID="txtAvilableQty" runat="server" CssClass="form-control" Width="80px" Enabled="false" OnTextChanged="txtAvilableQty_TextChanged"></asp:TextBox>
            </td>
            <td>Basic Unit<br />
                <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true"></asp:TextBox>
            </td>

            <td>
                <br />
                <asp:Label ID="lblBUnit" runat="server" Text=""></asp:Label>
            </td>
            <td>Rate/Basic Unit<br />
                <asp:TextBox ID="txtRate" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true" OnTextChanged="txtRate_TextChanged"></asp:TextBox>
            </td>
            <td>Amount<br />
                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true"></asp:TextBox>
            </td>
            <td>Product Rate Type<br />
                <asp:DropDownList ID="ddlProductRateType" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true"></asp:DropDownList>
            </td>
            <td>Adjustment Reason<br />
                <asp:TextBox ID="txtRemarks" CssClass="form-control" runat="server"></asp:TextBox>
            </td>
            <td>
                <br />
                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
    <br />
    <table style="border: solid; font-size: 12px;">
        <tr>
            <td style="width: 1000px">
                <div style="height: 300px; width: 1000px; overflow: scroll; overflow-x: hidden; margin-top: 10px;">
                    <div style="background-color: cadetblue; text-align: center; width: 975px">
                        <b>Adjustment Detail </b>
                    </div>
                    <br />
                    <asp:GridView ID="grdAdjustmentStock" runat="server" AutoGenerateColumns="False" Width="975px" CssClass="gridtable"
                        Font-Size="11px" OnRowCommand="grdAdjustmentStock_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    <asp:Label ID="lblProductPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Bind("PRODUCT_CODE") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("PRODUCT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Batch" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblBatch" runat="server" Text='<%# Bind("BATCH_NO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Exp. Date" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblExpDate" runat="server" Text='<%# Bind("EXPIRY_DATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" runat="server" Text='<%# Bind("QUANTITY") %>'></asp:Label>
                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("UNIT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate">
                                <ItemTemplate>
                                    <asp:Label ID="lblRate" runat="server" Text='<%# Bind("RATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total">
                                <ItemTemplate>
                                    <asp:Label ID="lblItemTotal" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("REMARKS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton2" CommandName="Remove" ImageUrl="~/images/icons/deletes.png" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="50px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
            <td style="vertical-align: top; padding-left: 5px; padding-right: 5px;">
                <table style="width: 300px;">
                    <tr>
                        <br />
                        <td>Adjustment Total</td>
                        <td></td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblAdjustmentTotal" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" Width="50%" OnClick="btnSave_Click" /><br />
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>

