<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AssetDakhila.aspx.cs" Inherits="FixedAsset_Utilities_AssetDakhila" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid form-group-sm">
        <asp:Label ID="lblOldPK_ID" runat="server" Text="" Visible="false"></asp:Label>
        <table style="width: 100%; border-bottom: solid 1px;" class="gridtable">
            <tr>
                <td style="width: 750px; padding-right: 5px;">
                    <table style="width: 100%">
                        <tr runat="server" id="divSupplier" visible="true">
                            <td>Supplier Code<br />
                                <asp:TextBox ID="txtSupplierCode" runat="server" CssClass="form-control"
                                    OnTextChanged="txtSupplierCode_TextChanged" AutoPostBack="true" Width="150px">
                                </asp:TextBox>
                            </td>
                            <td colspan="2">Supplier Name<br />
                                <asp:DropDownList ID="ddlSupplier" runat="server" Width="450px"
                                    Font-Size="Larger" AutoPostBack="true" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged">
                                </asp:DropDownList>
                                <script>
                                    $('#<%=ddlSupplier.ClientID%>').chosen();
                                </script>
                                &nbsp;Walk in&nbsp;
                                <asp:CheckBox ID="chkSuppliers" runat="server" AutoPostBack="true" OnCheckedChanged="chkSuppliers_CheckedChanged" /></td>
                        </tr>
                        <tr runat="server" id="divWalkinSupplier" visible="false">
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>Supplier Name
                                            <br />
                                            <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" Width="600px"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;&nbsp;Walk in&nbsp;
                                            <asp:CheckBox ID="chkWalkIn" runat="server" AutoPostBack="true" OnCheckedChanged="chkWalkIn_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>PAN/VAT No
                                <br />
                                <asp:TextBox ID="txtSupplierPANVAT" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td>Address<br />
                                <asp:TextBox ID="txtSupplierAddress" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>

                            <td>Contact<br />
                                <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>

                    </table>
                </td>
                <td style="width: 500px; padding-left: 15px; border-left: dashed 1px; vertical-align: top;">
                    <table class="gridtable">
                        <tr>
                            <td>Purchase Mode
                            <br />
                                <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control"
                                    OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>Invoice Number<br />
                                <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                            </td>
                            <td>Invoice Date<br />
                                <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control datepicker" Width="150px"></asp:TextBox>
                            </td>
                            <td>Dakhila Date
                            <br />
                                <asp:TextBox ID="txtDakhilaDate" runat="server" CssClass="form-control" Enabled="false" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <table style="width: 1100px; padding-right: 5px;" class="gridtable">
            <tr>
                <td>Code
                <br />
                    <asp:TextBox ID="txtASSETCode" runat="server" CssClass="form-control" OnTextChanged="txtASSETCode_TextChanged" AutoPostBack="true" Width="150px"></asp:TextBox></td>
                <td>Asset Name
                <br />
                    <asp:DropDownList ID="ddlASSET" runat="server" CssClass="form-control" Width="500px"
                        Font-Size="Larger" AutoPostBack="true" OnSelectedIndexChanged="ddlASSET_SelectedIndexChanged">
                    </asp:DropDownList>
                    <script>
                        $('#<%=ddlASSET.ClientID%>').chosen();
                    </script>
                </td>
                <td runat="server" id="divBatch" visible="false">Batch<br />
                    <asp:TextBox ID="txtBatch" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
                <td runat="server" id="divExpDate" visible="false">Expiry Date<br />
                    <table style="width: 300px;" class="gridtable">
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlExpMonth" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="01">Jan</asp:ListItem>
                                    <asp:ListItem Value="02">Feb</asp:ListItem>
                                    <asp:ListItem Value="03">Mar</asp:ListItem>
                                    <asp:ListItem Value="04">Apr</asp:ListItem>
                                    <asp:ListItem Value="05">May</asp:ListItem>
                                    <asp:ListItem Value="06">Jun</asp:ListItem>
                                    <asp:ListItem Value="07">Jul</asp:ListItem>
                                    <asp:ListItem Value="08">Aug</asp:ListItem>
                                    <asp:ListItem Value="09">Sep</asp:ListItem>
                                    <asp:ListItem Value="10">Oct</asp:ListItem>
                                    <asp:ListItem Value="11">Nov</asp:ListItem>
                                    <asp:ListItem Value="12">Dec</asp:ListItem>
                                </asp:DropDownList></td>
                            <td>
                                <asp:DropDownList ID="ddlExpYear" runat="server" CssClass="form-control"></asp:DropDownList></td>
                        </tr>
                    </table>

                </td>
            </tr>
        </table>
        <table style="width: 600px; padding-right: 5px;" class="gridtable">
            <tr>
               
                <td>
                    <br />
                    <asp:Label ID="lblUnit" runat="server"></asp:Label>
                </td>
                <td>Qty<br />
                    <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" Width="120px" AutoPostBack="true" OnTextChanged="txtQty_TextChanged"></asp:TextBox>
                </td>

                <td>Rate<br />
                    <asp:TextBox ID="txtRate" runat="server" CssClass="form-control" Width="120px" AutoPostBack="true" OnTextChanged="txtRate_TextChanged"></asp:TextBox>
                </td>
                <td>Amount<br />
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Width="120px" AutoPostBack="true" OnTextChanged="txtAmount_TextChanged"></asp:TextBox>
                </td>
                <td>
                    <br />
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>


        <br />
        <table style="border: solid;">
            <tr>
                <td style="width: 900px">
                    <div style="height: 300px; width: 900px; overflow: scroll; overflow-x: hidden; margin-top: 10px;">
                        <div style="background-color: cadetblue; text-align: center; width: 875px"><b>Purchase Detail </b></div>
                        <asp:GridView ID="grdPurchaseDetail" runat="server" AutoGenerateColumns="False" Width="875px"
                            CssClass="gridtable" OnRowCommand="grdPurchaseDetail_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                        <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblASSETCode" runat="server" Text='<%# Bind("ASSET_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="120px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblASSETName" runat="server" Text='<%# Bind("ASSET") %>'></asp:Label>
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
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemTotal" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
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
                    <br />
                    <br />
                    <table id="tblLocalPurchase" style="width: 300px;" runat="server" visible="true">
                        <tr>
                            <td style="width: 90px;">Total</td>
                            <td style="width: 45px;"></td>
                            <td style="text-align: right; width: 35px;">
                                <asp:Label ID="lblSubTotalAmount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Trade Discount</td>
                            <td>
                                <asp:TextBox ID="txtDiscount" runat="server" Width="35px" Text="0" OnTextChanged="txtDiscount_TextChanged" AutoPostBack="true"></asp:TextBox>
                                %</td>
                            <td style="text-align: right;">
                                <asp:TextBox ID="txtDiscountAmount" runat="server" Width="80px" Style="text-align: right" OnTextChanged="txtDiscountAmount_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Sub Total</td>
                            <td></td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblTotalAmount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 25px">VAT</td>
                            <td style="height: 25px">
                                <asp:TextBox ID="txtVATPercent" runat="server" Width="35px" Text="13"></asp:TextBox>
                                %</td>
                            <td style="text-align: right; height: 25px;">
                                <asp:Label ID="lblVAT" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>

                            <td>Grand Total</td>
                            <td></td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblGrandTotal" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>

                            <td>Rounding</td>
                            <td></td>
                            <td style="text-align: right;">
                                <asp:TextBox ID="txtRound" runat="server" Width="80px" Text="" Style="text-align: right"
                                    AutoPostBack="true" OnTextChanged="txtRound_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Total</td>
                            <td></td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblInvoiceAmount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:FileUpload ID="fileAttachment" runat="server" />

                            </td>

                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" Width="100%" OnClick="btnSavePurchase_Click" /></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblerror" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>

    </div>
</asp:Content>

