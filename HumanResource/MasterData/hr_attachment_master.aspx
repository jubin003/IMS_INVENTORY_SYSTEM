<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="hr_attachment_master.aspx.cs" Inherits="HumanResource_MasterData_hr_attachment_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-6">
                        <asp:GridView ID="grdAttachment" runat="server" Width="100%" CssClass="gridtable" AutoGenerateColumns="False" EnableModelValidation="True"
                            OnRowCancelingEdit="grdAttachment_RowCancelingEdit" OnRowDataBound="grdAttachment_RowDataBound"
                            OnRowEditing="grdAttachment_RowEditing" OnRowUpdating="grdAttachment_RowUpdating">
                            <AlternatingRowStyle BackColor="#eee6ff" />
                            <Columns>
                                <asp:TemplateField HeaderText="SN">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSn" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                        <asp:Label ID="lblPkid" runat="server" Text='<%# Bind("PK_ID") %>' Visible="False"></asp:Label>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Attachment Document">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAttDocE" CssClass="form-control" runat="server" Text='<%# Bind("ATTACHMENT_DOC") %>' Width="100%"></asp:TextBox>
                                        <asp:Label ID="lblAttDocE" runat="server" Text='<%# Bind("ATTACHMENT_DOC") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderTemplate>
                                        Attachment Document<br />
                                        <asp:TextBox ID="txtAttDocH" CssClass="form-control" runat="server" Text="" Width="100%"></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAttDoc" runat="server" Text='<%# Bind("ATTACHMENT_DOC") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlStatusE" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Available" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Unavailable" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblStatusE" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderTemplate>
                                        Status<br />
                                        <asp:DropDownList ID="ddlStatusH" CssClass="form-control" runat="server" AutoPostBack="True">
                                            <asp:ListItem Text="Available" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Unavailable" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lbllblStatusName" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Number Applicable">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlDocNumE" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblDocNumE" runat="server" Text='<%# Bind("DOC_NUM_APPLICABLE") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderTemplate>
                                        Document Number Applicable<br />
                                        <asp:DropDownList ID="ddlDocNumH" CssClass="form-control" runat="server" AutoPostBack="True">
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocNum" runat="server" Text='<%# Bind("DOC_NUM_APPLICABLE") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblDocNumName" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlIssueDateE" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblIssueDateE" runat="server" Text='<%# Bind("ISSUE_DATE_APPLICABLE") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderTemplate>
                                        Issue Date Applicable<br />
                                        <asp:DropDownList ID="ddlIssueDateH" CssClass="form-control" runat="server" AutoPostBack="True">
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssueDate" runat="server" Text='<%# Bind("ISSUE_DATE_APPLICABLE") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblIssueDateName" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expiry Date Applicable">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlExpAppliE" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblExpAppliE" runat="server" Text='<%# Bind("EXP_DATE_APPLICABLE") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderTemplate>
                                        Expiry Date Applicable<br />
                                        <asp:DropDownList ID="ddlExpAppliH" CssClass="form-control" runat="server" AutoPostBack="True">
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpAppli" runat="server" Text='<%# Bind("EXP_DATE_APPLICABLE") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblExpAppliName" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Issue By Applicable">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlIssueAppliE" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblIssueAppliE" runat="server" Text='<%# Bind("ISSUE_BY_APPLICABLE") %>' Visible="false"></asp:Label>
                                    </EditItemTemplate>
                                    <HeaderTemplate>
                                        Issue By Applicable<br />
                                        <asp:DropDownList ID="ddlIssueAppliH" CssClass="form-control" runat="server" AutoPostBack="True">
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblIssueAppli" runat="server" Text='<%# Bind("ISSUE_BY_APPLICABLE") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblIssueAppliName" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order">
                                    <HeaderTemplate>
                                        Order<br />
                                        <asp:TextBox ID="txtOrder" TextMode="Number" Width="50px" CssClass="form-control" runat="server" Text='<%# Bind("ORDER_BY") %>'></asp:TextBox>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtOrder" Enabled="false" TextMode="Number" runat="server" Width="50px" CssClass="form-control" Text='<%# Bind("ORDER_BY") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtOrder" TextMode="Number" runat="server" Width="50px" CssClass="form-control" Text='<%# Bind("ORDER_BY") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/icons/upload.png" CommandName="Update" />
                                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/icons/delete.gif" CommandName="Cancel" />
                                    </EditItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary btn-sm" Text="Add" OnClick="btnAdd_Click" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/icons/edit.png" CommandName="Edit" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

