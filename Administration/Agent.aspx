<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Agent.aspx.cs" Inherits="Administration_Agent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div runat="server" id="divAdd" visible="false">
            <table class="gridtable" style="width: 800px">
                <tr>
                    <td colspan="2">
                        <div class="popup-big-container-heading">
                            <div class="popup-title">Add Agent</div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td id="trBranch" runat="server" colspan="3" >Branch<br />
                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control"></asp:DropDownList>
                </td>
                </tr>
                <tr>
                    <td>Agent Code<br />
                        <asp:TextBox ID="txtAgentCode" CssClass="form-control" runat="server" MaxLength="8" AutoPostBack="true" OnTextChanged="txtAgentCode_TextChanged"></asp:TextBox>
                    </td>
                    <td>Agent Type<br />
                        <asp:DropDownList ID="ddlAgentType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="S">Sales</asp:ListItem>
                            <asp:ListItem Value="P">Purchase</asp:ListItem>
                            <asp:ListItem Value="I">Import</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Agent Name<br />
                        <asp:TextBox ID="txtAgentName" CssClass="form-control" runat="server"></asp:TextBox>
                    </td>
                    <td>Address1<br />
                        <asp:TextBox ID="txtAddress1" runat="server" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Address2
                        <br />
                        <asp:TextBox ID="txtAddress2" runat="server" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td>District
                    <br />
                        <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control"></asp:DropDownList>
                    </td>

                </tr>
                <tr>
                    <td>Mobile No
                    <br />
                        <asp:TextBox ID="txtMobileNo" CssClass="form-control" runat="server"></asp:TextBox></td>
                    <td>Email
                     <br />
                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox></td>
                </tr>
                <tr>

                    <td>Commission Percent
                     <br />
                        <asp:TextBox ID="txtCommissionPercent" CssClass="form-control" runat="server" TextMode="Number" Text="0" AutoPostBack="true" OnTextChanged="txtCommissionPercent_TextChanged"></asp:TextBox></td>
                    <td>Status:
                    <br />
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" >

                            <asp:ListItem Value="1">Available</asp:ListItem>
                            <asp:ListItem Value="0">Unavailable</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>

                <tr>


                    <td>
                        <asp:Button ID="btnSave" runat="server" Width="100%" CssClass="btn btn-success" Text="Save" OnClick="btnSave_Click" /></td>
                    <td>
                        <asp:Button ID="btnReset" runat="server" Width="100%" CssClass="btn btn-danger" Text="Cancel" OnClick="btnReset_Click" /></td>

                </tr>
            </table>
        </div>

        <div id="divGrid" runat="server" visible="true">
            <div class="row">
                <div class="col-md-3" id="divBranchFilter" runat="server">
                    Branch<br />
                    <asp:DropDownList ID="ddlBranchFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranchFilter_SelectedIndexChanged" >
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    Agent Type<br />
                        <asp:DropDownList ID="ddlAgentTypeFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAgentTypeFilter_SelectedIndexChanged">
                            <asp:ListItem Value="0">Select</asp:ListItem>
                            <asp:ListItem Value="S">Sales</asp:ListItem>
                            <asp:ListItem Value="P">Purchase</asp:ListItem>
                            <asp:ListItem Value="I">Import</asp:ListItem>
                        </asp:DropDownList>
                </div>
                 <div class="col-md-1"><br />
                     <asp:Button ID="btnAddMore" runat="server" Text="Add More" CssClass="btn btn-success" OnClick="btnAddMore_Click" />
                      </div>
                <div class="col-md-1">
                     <asp:Label ID="lblPKIDU" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblerror" runat="server"></asp:Label>
                    </div>
            </div>
          
            <asp:GridView ID="gridAgent" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                Width="100%"
                CssClass="gridtable" AllowPaging="True" OnPageIndexChanging="gridAgent_PageIndexChanging" PageSize="5" OnRowCommand="gridAgent_RowCommand" OnRowDataBound="gridAgent_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="SN">
                        <ItemTemplate>
                            <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Agent Code">
                        <ItemTemplate>
                            <asp:Label ID="lblAgentCode" runat="server" Text='<%# Bind("AGENT_CODE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Agent Name">
                        <ItemTemplate>
                            <asp:Label ID="lblAgentName" runat="server" Text='<%# Bind("AGENT_NAME") %>'></asp:Label>
                            <asp:Label ID="lblAgentID" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address1">
                        <ItemTemplate>
                            <asp:Label ID="lblAddress1" runat="server" Text='<%# Bind("AGENT_ADDRESS1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address2">
                        <ItemTemplate>
                            <asp:Label ID="lblAddress2" runat="server" Text='<%# Bind("AGENT_ADDRESS2") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Branch">
                        <ItemTemplate>
                             <asp:Label ID="lblShowBranch" runat="server" ></asp:Label>
                            <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("OFFICE_CODE") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contact Detail">
                        <ItemTemplate>
                            <asp:Image ID="imgMName" runat="server" ImageUrl="~/images/icons/mobile.png"></asp:Image>:<asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("MOBILE") %>'></asp:Label><br />
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/icons/email.png"></asp:Image>:<asp:Label ID="lblEmail" runat="server" Text='<%# Bind("EMAIL") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Bind("STATUS") %>'></asp:Label>
                        <asp:Label ID="lblStat" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Commision Per">
                        <ItemTemplate>
                            <asp:Label ID="lblCommisionPer" runat="server" Text='<%# Bind("COMMISSION_PER") %>'></asp:Label>
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


    </div>
</asp:Content>

