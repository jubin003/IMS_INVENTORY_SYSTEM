<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="supplier.aspx.cs" Inherits="administration_supplier" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div runat="server" id="divAdd" visible="false" class="form-group-sm">
        <table class="gridtable" style="width: 1000px">
            <tr>
                <td colspan="4">
                    <div class="popup-big-container-heading">
                        <div class="popup-title">Add Vendor</div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>Supplier Type<br />
                    <asp:DropDownList ID="ddlSupplierType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSupplierType_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 25%">Supplier Code<br />
                    <asp:TextBox ID="txtSupplierCode" CssClass="form-control" runat="server"></asp:TextBox>
                </td>
                <td style="width: 25%">Supplier Name<br />
                    <asp:TextBox ID="txtSupplierName" CssClass="form-control" runat="server"></asp:TextBox>
                </td>

                <td style="width: 25%">Address:
                     <br />
                    <asp:TextBox ID="txtAddress" CssClass="form-control" runat="server"></asp:TextBox>
                </td>
                 <td style="width: 25%">Country:
                     <br />
                     <asp:DropDownList ID="ddlCountry" CssClass="form-control" runat="server"></asp:DropDownList>
                </td>

                <td style="width: 25%">
                    <br />
                </td>

            </tr>
            <tr>
                <td>Registration No<br />
                    <asp:TextBox ID="txtRegNo" runat="server" CssClass="form-control"></asp:TextBox>
                </td>


                <td>PAN/VAT No:
                    <br />
                    <asp:TextBox ID="txtPANNo" runat="server" CssClass="form-control" Style="display: inline-block" AutoPostBack="true" OnTextChanged="txtPANNo_TextChanged"></asp:TextBox>
                </td>
                <td>Status:
                    <br />
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Value="1">Available</asp:ListItem>
                        <asp:ListItem Value="0">Unavailable</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td>Contact Person<br />
                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control"></asp:TextBox>

                </td>

                <td>Phone No<br />
                    <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
                <td>Mobile No
                    <br />
                    <asp:TextBox ID="txtMobileNo" CssClass="form-control" runat="server"></asp:TextBox></td>
                <td>Email
                     <br />
                    <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox></td>
            </tr>

            <tr>
                <td colspan="2">Remarks:
                    <br />
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox></td>

            </tr>
            <tr>


                <td>
                    <asp:Button ID="btnSave" runat="server" Width="100%" CssClass="btn btn-success" Text="Save" OnClick="btnSave_Click" /></td>
                <td>
                    <asp:Button ID="btnReset" runat="server" Width="100%" CssClass="btn btn-danger" Text="Cancel" OnClick="btnReset_Click" /></td>

            </tr>
        </table>
    </div>

    <div id="divGrid" runat="server">
        <table style="text-align: left; width:500px;">
            <tr>
                <td>Sypplier Type</td>
                <td>
                    <asp:DropDownList ID="ddlSupplierTypeList" runat="server" CssClass="form-control"></asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnList" runat="server" Text="List" CssClass="btn btn-success" OnClick="btnList_Click"  />
                </td>
                <td>
                    <asp:Button ID="btnAddMore" runat="server" Text="Add More" CssClass="btn btn-success" OnClick="btnAddMore_Click" />
                </td>

                <td>
                    <asp:Label ID="lblPKIDU" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gridSupplier" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
            OnRowCommand="gridSupplier_RowCommand" OnRowDataBound="gridSupplier_RowDataBound" Width="100%"
            CssClass="gridtable" AllowPaging="True" OnPageIndexChanging="gridSupplier_PageIndexChanging" PageSize="5">
            <Columns>
                <asp:TemplateField HeaderText="SN">
                    <ItemTemplate>
                        <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Supplier Detail">
                    <ItemTemplate>
                        Code: 
                        <asp:Label ID="lblSupplierCode" runat="server" Text='<%# Bind("SUPPLIER_CODE") %>'></asp:Label><br />
                        Name:<asp:Label ID="lblSupplierName" runat="server" Text='<%# Bind("SUPPLIER_NAME") %>'></asp:Label>
                        <asp:Label ID="lblSupplierID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label><br />
                        Address:   
                        <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("ADDRESS") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Reg. Detail">
                    <ItemTemplate>
                        PAN/VAT :&nbsp;<asp:Label ID="lblPAN_VATNO" runat="server" Text='<%# Bind("VAT_PAN_NUMBER") %>'></asp:Label><br />
                        Reg. No &nbsp;:&nbsp;<asp:Label ID="lblRegNo" runat="server" Text='<%# Bind("REG_NUMBER") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Contact Detail">
                    <ItemTemplate>
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/images/icons/user.png"></asp:Image>:<asp:Label ID="lblContactPerson" runat="server" Text='<%# Bind("CONTACT_PERSON") %>'></asp:Label><br />
                        <asp:Image ID="imgMName" runat="server" ImageUrl="~/images/icons/mobile.png"></asp:Image>:<asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("MOBILE") %>'></asp:Label><br />
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/phone.png"></asp:Image>:<asp:Label ID="lblPhoneNo" runat="server" Text='<%# Bind("PHONE") %>'></asp:Label><br />
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/icons/email.png"></asp:Image>:<asp:Label ID="lblEmail" runat="server" Text='<%# Bind("EMAIL") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Bind("STATUS") %>'></asp:Label>
                        <asp:Label ID="lblStat" runat="server" Text=""></asp:Label>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="Remarks">
                    <ItemTemplate>
                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("REMARKS") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" runat="server" CommandName="Change" ImageUrl="~/images/icons/edit.png" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>
</asp:Content>

