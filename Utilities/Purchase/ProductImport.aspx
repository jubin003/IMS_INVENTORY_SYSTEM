<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProductImport.aspx.cs"
    Inherits="Utilities_Purchase_ProductImport" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid form-group-sm">
        <asp:Label ID="lblOldPK_ID" runat="server" Text="" Visible="false"></asp:Label>
        <%-- Purchse Invoice Block Start--%>
        <div id="divStep1" runat="server">
            <table style="width: 100%; border-bottom: solid 1px;">
                <tr>
                    <td style="width: 700px; padding-right: 5px;">
                        <table style="width: 100%">
                            <tr runat="server" id="divSupplier" visible="true">
                                <td>Supplier Code<br />
                                    <asp:TextBox ID="txtSupplierCode" runat="server" CssClass="form-control"
                                        OnTextChanged="txtSupplierCode_TextChanged" AutoPostBack="true" Width="150px">
                                    </asp:TextBox>
                                </td>
                                <td colspan="2">Supplier Name<br />
                                    <asp:DropDownList ID="ddlSupplier" runat="server" Width="400px"
                                        Font-Size="Larger" AutoPostBack="true" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <script>
                                        $('#<%=ddlSupplier.ClientID%>').chosen();
                                    </script>
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
                            <tr>
                                <td>Invoice Number<br />
                                    <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                                <td>Invoice Date<br />
                                    <asp:TextBox ID="txtInvoiceDate" AutoComplete="off" runat="server" CssClass="form-control datepicker" Width="275px"></asp:TextBox>
                                </td>
                                <td>Dakhila Date
                            <br />
                                    <asp:TextBox ID="txtDakhilaDate" AutoComplete="off" runat="server" CssClass="form-control" Enabled="false" Width="275px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 500px; padding-left: 15px; border-left: dashed 1px; vertical-align: top;">
                        <table class="gridtable">
                            <tr>
                                <td colspan="2">Pragyapan Patra Number<br />
                                    <asp:TextBox ID="txtPPNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Purchase Mode
                            <br />
                                    <asp:DropDownList ID="ddlPaymentType" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </td>
                                <td id="divCurrency" runat="server">Currency
                            <br />
                                    <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td id="divExchangeRate" runat="server">Exchange Rate
                            <br />
                                    <asp:TextBox ID="txtExchangeRate" runat="server" Width="100px" CssClass="form-control" Text="1"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table style="width: 1300px; padding-right: 5px;">
                <tr>
                    <td>Code
                    <br />
                        <asp:TextBox ID="txtProductCode" runat="server" CssClass="form-control" OnTextChanged="txtProductCode_TextChanged" AutoPostBack="true" Width="100px"></asp:TextBox></td>
                    <td>Product Name
                    <br />
                        <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control" Width="500px"
                            Font-Size="Larger" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged">
                        </asp:DropDownList>
                        <script>
                            $('#<%=ddlProduct.ClientID%>').chosen();
                        </script>
                    </td>
                    <td>Weight
                    <br />
                        <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control" Width="80px"></asp:TextBox>
                    </td>
                    <td>
                        <br />
                        Kg
                    </td>
                    <td>Qty<br />
                        <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" Width="80px" AutoPostBack="true" OnTextChanged="txtQty_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <br />
                        <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control" Width="80px" Enabled="false"></asp:DropDownList>
                    </td>

                    <td>Rate<br />
                        <asp:TextBox ID="txtRate" runat="server" CssClass="form-control" Width="90px" AutoPostBack="true" OnTextChanged="txtRate_TextChanged"></asp:TextBox>
                    </td>
                    <td>Amount<br />
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Width="90px" AutoPostBack="true" OnTextChanged="txtAmount_TextChanged"></asp:TextBox>
                    </td>
                    <td>PP Amount<br />
                        <asp:TextBox ID="txtPPAmount" runat="server" CssClass="form-control" Width="90px"></asp:TextBox>
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
                    <td>
                        <div style="height: 300px; width: 1300px; overflow: scroll; overflow-x: hidden; margin-top: 10px;">
                            <div style="background-color: cadetblue; text-align: center; width: 1275px"><b>Purchase Detail </b></div>
                            <asp:GridView ID="grdPurchaseDetail" runat="server" AutoGenerateColumns="False" Width="1275px"
                                CssClass="gridtable" OnRowCommand="grdPurchaseDetail_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sno">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                            <asp:Label ID="lblPK_ID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductCode" runat="server" Text='<%# Bind("PRODUCT_CODE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" />
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
                                    <asp:TemplateField HeaderText="Weight">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWeight" runat="server" Text='<%# Bind("WEIGHT") %>'></asp:Label>
                                            <asp:Label ID="lblWeightUnit" runat="server" Text="Kg"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Bind("QUANTITY") %>'></asp:Label>
                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("UNIT") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FC Rate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFCRate" runat="server" Text='<%# Bind("FC_RATE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRate" runat="server" Text='<%# Bind("RATE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exchange Rate" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExchangeRate" runat="server" Text='<%# Bind("EXCHANGE_RATE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FC Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFC_ItemTotal" runat="server" Text='<%# Bind("FC_TOTAL") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemTotal" runat="server" Text='<%# Bind("TOTAL") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PP Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPPItemTotal" runat="server" Text='<%# Bind("PP_TOTAL") %>'></asp:Label>
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
                            <div style="text-align: right; width: 1275px; padding-right: 50px; font-size: large;">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 800px">&nbsp
                                        </td>
                                        <td>
                                            <table id="tblPurchase" style="width: 100%" runat="server" visible="true" class="gridtable">
                                                <tr>
                                                    <td style="width: 90px;">Invoice Total</td>
                                                    <td></td>
                                                    <td style="width: 45px;"> <asp:Label ID="lblInvoiceTotal" runat="server" Text=""></asp:Label></td>
                                                    <td style="text-align: right; width: 35px;">
                                                       
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <hr />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 90px;">Kharid Khata Amount</td>
                                                    <td style="width: 70px;"></td>
                                                    <td style="text-align: right; width: 35px;">
                                                        <asp:Label ID="lblKharidKhataAmount" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="height: 25px">Kharid Khata VAT</td>
                                                    <td style="height: 25px">
                                                        <asp:TextBox ID="txtKharidKhataVAT" runat="server" Width="35px" Text="13"></asp:TextBox>
                                                        %</td>
                                                    <td style="text-align: right; height: 25px;">
                                                        <asp:Label ID="lblKharidKhataVAT" runat="server" Text=""></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td>Kharid Khata Total</td>
                                                    <td></td>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblKharidKhataTotal" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>

            </table>
            <br />
            <div style="text-align: center; width: 1200px">
                <asp:Button ID="btnMoveToCustom" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="btnMoveToCustom_Click" />
            </div>
        </div>
        <%-- Purchse Invoice Block End--%>
        <%-- Custom Charge Block Start--%>
        <div id="divStep2" runat="server" visible="false">
            <table style="width: 1200px;" runat="server" class="gridtable">
                <tr>
                    <td>
                        <div style="background-color: cornflowerblue; text-align: center; width: 1000px"><b>Custom Expenses </b></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="row">
                            <div class="col-md-3" runat="server" id="div1">
                                Custom<br />
                                <asp:DropDownList ID="ddlCustom" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3" runat="server" id="divStep2Agent">
                                Agent<br />
                                <asp:DropDownList ID="ddlCustomExpensesAgent" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>

                        </div>
                    </td>
                </tr>
            </table>
            <table id="tblImport" style="border: solid; width: 1000px;" runat="server" visible="true" class="gridtable">

                <tr>
                    <td>
                        <asp:GridView ID="grdImportCost" runat="server" AutoGenerateColumns="False" CssClass="gridtable" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Heading">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGL_NAME" runat="server" Text='<%# Bind("GL_NAME") %>' Width="450px"></asp:Label>
                                        <asp:Label ID="lblGL_CODE" runat="server" Text='<%# Bind("GL_CODE") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Style="text-align: right;" Width="100%"
                                            TextMode="Number" AutoPostBack="true" Text="0" OnTextChanged="txtAmount_Step2_TextChanged"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cost Effect">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCostEffect" Checked="true" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; padding-right: 150px;">
                        <div class="row">
                            <div class="col-md-7"></div>
                            <div class="col-md-2">Sub Total : </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblTotalImportExpenses" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </td>
                </tr>


            </table>
            <br />
            <div style="text-align: center; width: 1100px">
                <asp:Button ID="btnBackToInvoice" runat="server" Text="Previous" CssClass="btn btn-primary" OnClick="btnBackToInvoice_Click" />
                <asp:Button ID="btnMoveToForeginFreight" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="btnMoveToForeginFreight_Click" />
            </div>
        </div>
        <%-- Custom Charge Block End--%>
        <%-- Foregin Freight Block Start--%>
        <div id="divStep3" runat="server" visible="false">
            <table style="width: 1000px;" runat="server" class="gridtable">
                <tr>
                    <td>
                        <div style="background-color: cornflowerblue; text-align: center; width: 1000px"><b>Foreign Freight </b></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="row">
                            <div class="col-md-3" runat="server" id="divStep3Agent">
                                Agent<br />
                                <asp:DropDownList ID="ddlForeignFreightAgent" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdForeignFreight" runat="server" AutoGenerateColumns="False" CssClass="gridtable" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Heading">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGL_NAME" runat="server" Text='<%# Bind("GL_NAME") %>' Width="450px"></asp:Label>
                                        <asp:Label ID="lblGL_CODE" runat="server" Text='<%# Bind("GL_CODE") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Style="text-align: right;" Width="100%"
                                            TextMode="Number" AutoPostBack="true" Text="0" OnTextChanged="txtAmount_Step3_TextChanged"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cost Effect">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCostEffect" Checked="true" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; padding-right: 150px;">
                        <div class="row">
                            <div class="col-md-7"></div>
                            <div class="col-md-2">Total : </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblForeginFreightTotal" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <div style="text-align: center; width: 1100px">
                <asp:Button ID="btnBackToCustom" runat="server" Text="Previous" CssClass="btn btn-primary" OnClick="btnBackToCustom_Click" />
                <asp:Button ID="btnMoveToNepalFreight" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="btnMoveToNepalFreight_Click" />
            </div>
        </div>
        <%-- Foregin Freight Block End--%>
        <%-- Nepal Freight Start--%>
        <div id="divStep4" runat="server" visible="false">
            <table style="width: 1000px;" runat="server" class="gridtable">
                <tr>
                    <td>
                        <div style="background-color: cornflowerblue; text-align: center; width: 1000px"><b>Nepal Freight </b></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="row">
                            <div class="col-md-4">
                                Agent<br />
                                <asp:DropDownList ID="ddlNepalFreightAgent" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlNepalFreightAgent_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                PAV/VAT No.<br />
                                <asp:TextBox ID="txtNepalFreightAgentPANVAT" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="col-md-2" >
                                Address<br />
                                <asp:TextBox ID="txtNepalFreightAgentAddress" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-md-2">
                                Invoice Number<br />
                                <asp:TextBox ID="txtNepalFreightInvoiceNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                Invoice Date<br />
                                <asp:TextBox ID="txtNepalFreightInvoiceDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                            </div>
                            <div class="col-md-2">Purchase Mode
                            <br />
                                    <asp:DropDownList ID="ddlNepalFreight" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="CR">Credit</asp:ListItem>
                                    </asp:DropDownList>
                              </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdNepalFreight" runat="server" AutoGenerateColumns="False" CssClass="gridtable" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Heading">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGL_NAME" runat="server" Text='<%# Bind("GL_NAME") %>' Width="450px"></asp:Label>
                                        <asp:Label ID="lblGL_CODE" runat="server" Text='<%# Bind("GL_CODE") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Style="text-align: right;" Width="100%"
                                            TextMode="Number" AutoPostBack="true" Text="0" OnTextChanged="txtAmount_Step4_TextChanged"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cost Effect">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCostEffect" Checked="true" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; padding-right: 100px;">
                        <div class="row">
                            <div class="col-md-7"></div>
                            <div class="col-md-2">Sub Total : </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblNepalFreightSubTotal" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; padding-right: 100px;">
                        <div class="row">
                            <div class="col-md-7"></div>
                            <div class="col-md-2">
                                VAT :
                                <asp:TextBox ID="txtNepalFreightVAT" runat="server" Width="35px" Text="13"></asp:TextBox>%
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="lblNepalFreightVAT" runat="server" Style="text-align: right"></asp:TextBox>
                            </div>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; padding-right: 100px;">
                        <div class="row">
                            <div class="col-md-7"></div>
                            <div class="col-md-2">Total : </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblNepalFreightTotal" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <div style="text-align: center; width: 1100px">
                <asp:Button ID="bntBackToForeginFreight" runat="server" Text="Previous" CssClass="btn btn-primary" OnClick="bntBackToForeginFreight_Click" />
                <asp:Button ID="bntMoveToOtherExpenses" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="bntMoveToOtherExpenses_Click" />

            </div>
        </div>
        <%--  Nepal Freight Block End--%>
        <%-- Other Charge Block Start--%>
        <div id="divStep5" runat="server" visible="false">
            <table style="width: 1100px;" runat="server" class="gridtable">
                <tr>
                    <td>
                        <div style="background-color: cornflowerblue; text-align: center; width: 1100px"><b>Other Expenses </b></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="row">
                            <div class="col-md-4">
                                Expenses Heading
                                <br />
                                <asp:DropDownList ID="ddlExpensesHeading" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2" runat="server" visible="false">
                                VAT Applicable
                                <br />
                                <asp:CheckBox ID="chkVATApplicable" runat="server" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnAddOtherCharges" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAddOtherCharges_Click" />
                            </div>
                        </div>
                        <div id="divOtherChargeAddition" runat="server" visible="false">
                            <div class="row">
                                <div class="col-md-4" id="divOtherChargeBank" runat="server" visible="false">
                                    Bank
                                <br />
                                    <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-4" id="divOtherCharge" runat="server" visible="false">
                                    Party
                                <br />
                                    <asp:DropDownList ID="ddlParty" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    Amount
                                <br />
                                    <asp:TextBox ID="txtOtherExpenseAmount" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    Remarks
                                <br />
                                    <asp:TextBox ID="txtOtherExpenseRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <br />
                                    <asp:Button ID="btnAddOtherExpenses" runat="server" Text="Add To List" CssClass="btn btn-primary" OnClick="btnAddOtherExpenses_Click" />
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdOtherExpenses" runat="server" CssClass="gridtable" AutoGenerateColumns="False" Width="100%" OnRowCommand="grdOtherExpenses_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expenses Heading">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpenseCode" runat="server" Text='<%# Bind("ExpensesCode") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblExpenseHeading" runat="server" Text='<%# Bind("ExpensesHeading") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Party">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartyCode" runat="server" Text='<%# Bind("PartyCode") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblPartyName" runat="server" Text='<%# Bind("PartyName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpenseAmount" runat="server" Text='<%# Bind("ExpenseAmount") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpenseAmountRemarks" runat="server" Text='<%# Bind("ExpenseRemarks") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cost Effect">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCostEffect" Checked="true" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnRemoveExpenses" CommandName="Remove" ImageUrl="~/images/icons/deletes.png" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <br />
            <div style="text-align: center; width: 1100px">
                <asp:Button ID="btnBackToNepalFreight" runat="server" Text="Previous" CssClass="btn btn-primary" OnClick="btnBackToNepalFreight_Click" />
                <asp:Button ID="btnMoveToCostCalculation" runat="server" Text="Next" CssClass="btn btn-primary" OnClick="btnMoveToCostCalculation_Click" />
            </div>
        </div>
        <%-- Other Invoice Block End--%>
        <%-- Cost Calculation Block Start--%>
        <div id="divStep6" runat="server" visible="false">
            <table style="width: 100%;" runat="server" class="gridtable">
                <tr>
                    <td>
                        <div style="background-color: cornflowerblue; text-align: center; width: 100%"><b>Cost Calculation </b></div>
                    </td>
                </tr>
                <tr>
                    <td>Upload Pragyapan Patra File</td>
                </tr>
                <tr>
                    <td><asp:FileUpload ID="ppFile" runat="server" /></td>   
                </tr>
                <tr>
                    <td>Upload Foreign Frieght File</td>
                </tr>
                <tr>
                    <td><asp:FileUpload ID="ffFile" runat="server" /></td>   
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdCostCAlculation" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="gridtable">
                            <Columns>
                                <asp:TemplateField HeaderText="Sno">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSno" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Item Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemName" runat="server" Text="Product Name"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit Price (Box/PCS)"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty"></asp:TemplateField>
                                <asp:TemplateField HeaderText="INR"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Total INR"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Exchange Rate"></asp:TemplateField>
                                <asp:TemplateField HeaderText="NRS"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Custom Service"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Custom Duty"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Import Duty"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Foregin Feright"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Nepal Freight"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Insurance"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Bank Charges"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Clearing and Handling Charges"></asp:TemplateField>
                                <asp:TemplateField HeaderText="VAT on Import"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Additional Cost"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Cost"></asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate per unit With out VAT"></asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>

            </table>
            <br />
            <div style="text-align: center; width: 1100px">
                <asp:Button ID="btnBackToOtherExpenses" runat="server" Text="Previous" CssClass="btn btn-primary" OnClick="btnBackToOtherExpenses_Click" />
                <asp:Button ID="btnSaveImport" runat="server" Text="Save" CssClass="btn btn-primary" Width="150px" OnClick="btnSaveImport_Click" />
            </div>
        </div>
        <%-- Cost Calculation Block End--%>
        <asp:Label ID="lblerror" runat="server" Style="font-size: large; color: #FF0000"></asp:Label>
    </div>


</asp:Content>

