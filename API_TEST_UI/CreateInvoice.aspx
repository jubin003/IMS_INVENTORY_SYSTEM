<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateInvoice.aspx.cs" Inherits="API_TEST_UI_CreateInvoice" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="row">
            <asp:CheckBox ID="chkWalkIn" runat="server" Text="Walk-in / Cash sale" />
            <div class="col-md-4">
                <h4>Customer</h4>


                <label>Customer Code</label>
                <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="form-control"></asp:TextBox>
                <br />

                <label>Customer Name</label>
                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control"></asp:TextBox>
                <br />

                <label>Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
                <br />

                <label>PAN/VAT</label>
                <asp:TextBox ID="txtPanVat" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                <br />

                <label>Phone</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                <br />
            </div>

            <div class="col-md-4">
                <h4>Invoice</h4>
                <label>Transaction Date</label>

                <asp:TextBox ID="txtTransDate" runat="server" placeholder="dd/mm/yyyy" />
                <br />
                <label>Transaction Day</label>
                <asp:TextBox ID="txtTransDay" runat="server" CssClass="form-control"></asp:TextBox>
                <br />

                <label>Transaction Month</label>
                <asp:TextBox ID="txtTransMonth" runat="server" CssClass="form-control"></asp:TextBox>
                <br />

                <label>Transaction Year</label>
                <asp:TextBox ID="txtTransYear" runat="server" CssClass="form-control"></asp:TextBox>
                <br />

                <label>Customer Name</label>
                <asp:TextBox ID="txtInvCustomerName" runat="server" CssClass="form-control"></asp:TextBox>
                <br />

                <label>Customer Address</label>
                <asp:TextBox ID="txtInvCustomerAddress" runat="server" CssClass="form-control"></asp:TextBox>
                <br />

                <label>Age</label>
                <asp:TextBox ID="txtAge" runat="server" placeholder="Age" CssClass="form-control" />
                <br />

                <label>Gender</label>
                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Select" Value="" />
                    <asp:ListItem Text="Male" Value="M" />
                    <asp:ListItem Text="Female" Value="F" />
                    <asp:ListItem Text="Other" Value="O" />
                </asp:DropDownList>
                <br/>

                <label>Mode of Payment</label>
                <asp:DropDownList ID="ddlModeOfPayment" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Select" Value="" />
                    <asp:ListItem Text="Cash" Value="CS" />
                    <asp:ListItem Text="Credit" Value="CR" />
                    <asp:ListItem Text="QR" Value="QR" />
                </asp:DropDownList>
                <br/>

                <label>Customer PAN/VAT</label>
                <asp:TextBox ID="txtInvPanVat" runat="server" CssClass="form-control"></asp:TextBox>
                <br />

                <label>Discount Percent</label>
                <asp:TextBox ID="txtDiscountPercent" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                <br />
            </div>
        </div>

        <div class="row">
            <div class="col-md-8">
                <h4>Detail Lines</h4>

                <asp:HiddenField ID="hfDetailsJson" runat="server" />

                <asp:Repeater ID="rptDetails" runat="server" OnItemDataBound="rptDetails_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table table-bordered">
                            <tr>
                                <th>SNO</th>
                                <th>Product ID</th>
                                <th>Quantity</th>
                                <th>Rate</th>
                                <th>Scheme Discount</th>
                                <th></th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lblSno" runat="server"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtProductId" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtQty" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtRate" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtSchemeDiscount" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:LinkButton ID="lnkRemove" runat="server" CssClass="btn btn-danger btn-sm"
                                    CommandArgument='<%# Container.ItemIndex %>'
                                    OnClick="lnkRemove_Click" CausesValidation="false">Remove</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

                <asp:Button ID="btnAddRow" runat="server" Text="+ Add Row" CssClass="btn btn-secondary"
                    OnClick="btnAddRow_Click" CausesValidation="false" />

                <em class="d-block mt-2">Leave Product ID blank to skip a row.</em>
            </div>
        </div>

        <div class="row">
            <div class="col-md-4">
                <br />
                <asp:Button
                    ID="btnSubmit"
                    runat="server"
                    CssClass="btn btn-primary"
                    Text="Create Invoice"
                    OnClick="btnSubmit_Click" />
                <br />
                <br />
                <asp:Label ID="lblResult" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
