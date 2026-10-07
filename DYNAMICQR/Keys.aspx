<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Keys.aspx.cs" Inherits="DYNAMICQR_Keys" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="form-group-sm container-fluid">

        <table style="width: 100%">
            <tr>
                <td>QR Status
                   
                    <br />
                    <asp:DropDownList ID="ddlQrStatus" runat="server" CssClass="form-control" Style="width: 200px">
                        <asp:ListItem Text="Select" Value="" />
                        <asp:ListItem Text="Active" Value="1" />
                        <asp:ListItem Text="Inactive" Value="0" />
                    </asp:DropDownList>
                </td>
                <td>QR Device
                   
                    <br />
                    <asp:DropDownList ID="ddlQrDevice" runat="server" CssClass="form-control" Style="width: 200px">
                        <asp:ListItem Text="Select" Value="" />
                        <asp:ListItem Text="Active" Value="1" />
                        <asp:ListItem Text="Inactive" Value="0" />
                    </asp:DropDownList>
                </td>
                <td>QR Screen
                   
                    <br />
                    <asp:DropDownList ID="ddlQrScreen" runat="server" CssClass="form-control" Style="width: 200px">
                        <asp:ListItem Text="Select" Value="" />
                        <asp:ListItem Text="Active" Value="1" />
                        <asp:ListItem Text="Inactive" Value="0" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 100%" class="gridtable">
            <tr>
                <td>WS URL
                   
                    <br />
                    <asp:TextBox ID="txtWsUrl" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>WS Username
                   
                    <br />
                    <asp:TextBox ID="txtWsUsername" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>WS API Token
                   
                    <br />
                    <asp:TextBox ID="txtWsApiToken" runat="server" TextMode="MultiLine" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>NCHL Public Key
                   
                    <br />
                    <asp:TextBox ID="txtNchlPublicKey" runat="server" TextMode="MultiLine" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>Base URL
                   
                    <br />
                    <asp:TextBox ID="txtBaseUrl" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>Username
                   
                    <br />
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Password
                   
                    <br />
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>PFX Password
                   
                    <br />
                    <asp:TextBox ID="txtPfxPassword" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>Acquirer ID
                   
                    <br />
                    <asp:TextBox ID="txtAcquirerId" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Merchant ID
                   
                    <br />
                    <asp:TextBox ID="txtMerchantId" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>Merchant Name
                   
                    <br />
                    <asp:TextBox ID="txtMerchantName" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>Merchant Category Code
                   
                    <br />
                    <asp:TextBox ID="txtMerchantCategoryCode" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Merchant City
                   
                    <br />
                    <asp:TextBox ID="txtMerchantCity" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>Merchant Country
                   
                    <br />
                    <asp:TextBox ID="txtMerchantCountry" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>Merchant Postal Code
                   
                    <br />
                    <asp:TextBox ID="txtMerchantPostalCode" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Store Label
                   
                    <br />
                    <asp:TextBox ID="txtStoreLabel" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>Terminal Label
                   
                    <br />
                    <asp:TextBox ID="txtTerminalLabel" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>User ID
                   
                    <br />
                    <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>POS API Key
                   
                    <br />
                    <asp:TextBox ID="txtPOSAPIKey" TextMode="MultiLine" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>
                <td>POS Base URL
                   
                    <br />
                    <asp:TextBox ID="txtPOSBaseURL" runat="server" CssClass="form-control" Style="width: 300px"></asp:TextBox>
                </td>

                <td>POS PFX PATH
                   
                    <br />
                    <asp:TextBox ID="txtNPIpfxpath" runat="server" CssClass="form-control" Style="width: 300px" ReadOnly="true"></asp:TextBox>
                </td>
               
            </tr>
            <tr>
                <td colspan="3">
                    Upload PFX Certificate
                    <br />
                    <asp:FileUpload ID="fuPfxFile" runat="server" CssClass="form-control" Style="width: 300px; display: inline-block" />
                    <asp:Button ID="btnUploadPfx" runat="server" Text="Upload" CssClass="btn btn-secondary" OnClick="btnUploadPfx_Click" CausesValidation="false" />
                    <br />
                    <asp:Label ID="lblUploadMsg" runat="server" Style="color: #CC0000"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                </td>
                <td>
                    <strong>
                        <asp:Label ID="lblerror" runat="server" Style="color: #CC0000"></asp:Label>
                    </strong>
                </td>
            </tr>
        </table>

    </div>

    <asp:Label ID="lblPK_ID" runat="server" Text="" Visible="false"></asp:Label>

</asp:Content>
