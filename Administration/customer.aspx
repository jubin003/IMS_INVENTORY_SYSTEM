<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="customer.aspx.cs" Inherits="administration_customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div runat="server" id="divAdd" visible="false" class="form-group-sm">
        <table class="gridtable" style="width: 1000px">
            <tr >
                <td colspan="8">
                    <div class="popup-big-container-heading">
                        <div class="popup-title">Add Customer</div>
                    </div>
                </td>
            </tr>
            <tr>
                <td id="trBranch" runat="server" colspan="3" >Branch<br />
                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 25%" colspan="2">Customer Code<br />
                    <asp:TextBox ID="txtCustomerCode" CssClass="form-control" runat="server" MaxLength="8"  OnTextChanged="txtCustomerCode_TextChanged" AutoPostBack="true"></asp:TextBox>
                </td>
                <td style="width: 25%" colspan="2">Customer Name<br />
                    <asp:TextBox ID="txtCustomerName" CssClass="form-control" runat="server"></asp:TextBox>
                </td>

                <td style="width: 25%" colspan="2">Address:
                     <br />
                    <asp:TextBox ID="txtAddress" CssClass="form-control" runat="server"></asp:TextBox>
                </td>
               <td style="width: 25%">Country:
                     <br />
                   <asp:DropDownList ID="ddlCountry" CssClass="form-control" runat="server"></asp:DropDownList>
                </td>

            </tr>
            <tr>
                <td colspan="2">Registration No<br />
                    <asp:TextBox ID="txtRegNo" runat="server" CssClass="form-control"></asp:TextBox>

                </td>
                <td colspan="2">Register in
                <asp:RadioButtonList ID="rbtnPAN_VAT" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True">PAN</asp:ListItem>
                    <asp:ListItem>VAT</asp:ListItem>
                </asp:RadioButtonList>
                </td>
                <td colspan="2">PAN/VAT No:
                    <br />
                    <asp:TextBox ID="txtPANNo" runat="server" CssClass="form-control" TextMode="Number" Text="0" MaxLength="9"></asp:TextBox>

                </td>
                <td style="width: 25%" colspan="2">Join Date<br />
                    <asp:TextBox ID="txtJoinDate" CssClass="form-control datepicker" AutoComplete="off" runat="server" AutoPostBack="true" OnTextChanged="txtJoinDate_TextChanged"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td colspan="2">Contact Person<br />
                    <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
                <td colspan="2">Phone No<br />
                    <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control"></asp:TextBox>
                </td>
                <td colspan="2">Mobile No
                    <br />
                    <asp:TextBox ID="txtMobileNo" CssClass="form-control" runat="server"></asp:TextBox></td>
                <td colspan="2">Email
                     <br />
                    <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2">Area<br />
                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </td>
                <td colspan="2">Agent<br />
                    <asp:DropDownList ID="ddlAgent" runat="server" CssClass="form-control">
                    </asp:DropDownList></td>
                <td>Credit Limit<br />
                    <asp:TextBox runat="server" ID="txtCreditLimit" Text="" CssClass="form-control"></asp:TextBox>
                </td>
                <td>Status:
                    <br />
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Value="1">Available</asp:ListItem>
                        <asp:ListItem Value="0">Unavailable</asp:ListItem>
                    </asp:DropDownList></td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td colspan="4">Remarks:
                    <br />
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox></td>

            </tr>
            <tr>


                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" Width="100%" Text="Save" CssClass="btn btn-success" OnClick="btnSave_Click" /></td>
                <td colspan="2">
                    <asp:Button ID="btnReset" runat="server" Width="100%" Text="Cancel" CssClass="btn btn-danger" OnClick="btnReset_Click" /></td>

            </tr>
        </table>
    </div>

    <div id="divGrid" runat="server" visible="true">
        <table style="width: 600px">
            <tr>
                <td colspan="2" id="trBranchFilter" runat="server" style="padding-bottom: 5px">Branch<br />
                    <asp:DropDownList ID="ddlBranchFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranchFilter_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Customer Code
                                <br />
                    <asp:TextBox ID="txtSearchCustomerCode" runat="server" CssClass="form-control" Style="width: 120px"
                        AutoPostBack="true" OnTextChanged="txtSearchCustomerCode_TextChanged"></asp:TextBox>
                </td>
                <td>Name
                             <br />
                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control" Style="width: 300px; height: 50px"
                        Font-Size="Larger" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                    </asp:DropDownList>
                    <script>
                        $('#<%=ddlCustomer.ClientID%>').chosen();
                    </script>
                </td>
                <td>
                    <br />
                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-success" Text="Search" OnClick="btnSearch_Click" />
                </td>
                <td>
                    <br />
                    <asp:Button ID="btnAddMore" runat="server" CssClass="btn btn-success" Text="Add More" OnClick="btnAddMore_Click" />
                </td>
                <td>
                    <asp:Label ID="lblPKIDU" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
        </table>


        <asp:GridView ID="gridCustomer" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
            OnRowCommand="gridCustomer_RowCommand" OnRowDataBound="gridCustomer_RowDataBound" Width="100%"
            CssClass="gridtable" AllowPaging="True" OnPageIndexChanging="gridCustomer_PageIndexChanging" PageSize="5">
            <Columns>
                <asp:TemplateField HeaderText="SN">
                    <ItemTemplate>
                        <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Detail">
                    <ItemTemplate>
                        Code :   
                        <asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("CUSTOMER_CODE") %>'></asp:Label>
                        <br />
                        Name:  
                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CUSTOMER_NAME") %>'></asp:Label>
                        <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label><br />
                        Address :    
                        <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("ADDRESS") %>'></asp:Label><br />
                        Branch :
                        <asp:Label ID="lblShowBranch" runat="server" ></asp:Label>
                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("OFFICE_CODE") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Reg. Detail">
                    <ItemTemplate>
                        Reg. In &nbsp;&nbsp; :&nbsp;<asp:Label ID="lblRegIn" runat="server" Text='<%# Bind("PAN_VAT") %>'></asp:Label><br />
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
                <asp:TemplateField HeaderText="JoinDate">
                    <ItemTemplate>
                        BS :
                        <asp:Label ID="lblJoinDateBS" runat="server" Text='<%# Bind("JOIN_DATE_BS") %>'></asp:Label><br />
                        AD : 
                        <asp:Label ID="lblJoinDateAD" runat="server" Text='<%# Bind("JOIN_DATE_AD") %>'></asp:Label>
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

