<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillSetting.aspx.cs" Inherits="BillSettings_BillSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
    function updateFontPreview() {
        var input = document.getElementById('<%= txtFontSize.ClientID %>');
        var inputs = document.getElementById('<%= txtTSize.ClientID %>');
        var preview = document.getElementById('fontPreview');
        var previews = document.getElementById('fontPreviews');
        
        var fontSize = parseInt(input.value);
        var fontSizes = parseInt(inputs.value); // fixed: was input.value

        if (!isNaN(fontSize) && fontSize >= 8 && fontSize <= 30) {
            preview.style.fontSize = fontSize + 'px';
        }

        if (!isNaN(fontSizes) && fontSizes >= 8 && fontSizes <= 30) {
            previews.style.fontSize = fontSizes + 'px'; // fixed: was fontSizes
        }
    }

    // Trigger on page load
    window.onload = function () {
        updateFontPreview();
    };
</script>


    <div class="container-fluid">
        <div class="row">
            <div class="col-md-2">
                Select Bill Type:
                <asp:DropDownList ID="ddlBillTitle" CssClass="form-control" runat="server">
                    <asp:ListItem Text="Sales Invoice" />
                    <asp:ListItem Text="Debit Note" />
                    <asp:ListItem Text="Credit Note" />
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <br />
                <asp:Button ID="btnView" CssClass="btn btn-success" runat="server" Text="View" />
            </div>
        </div>
        <div runat="server" visible="true" id="divEntry">
            <div class="row">
                <div class="col-md-2">
                    Logo Url
                    <asp:TextBox ID="txtLogoUrl" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    Page Size                    
                    <asp:DropDownList ID="ddlPageSize" CssClass="form-control" runat="server">
                        <asp:ListItem Text="A4 (210 × 297 mm)" Value="A4" />
                        <asp:ListItem Text="A5 (148 × 210 mm)" Value="A5" />
                        <asp:ListItem Text="Letter (8.5 × 11 in)" Value="Letter" />
                        <asp:ListItem Text="Legal (8.5 × 14 in)" Value="Legal" />
                        <asp:ListItem Text="Thermal 80mm" Value="Thermal80" />
                        <asp:ListItem Text="Thermal 58mm" Value="Thermal58" />
                        <asp:ListItem Text="Custom Size" Value="Custom" />
                    </asp:DropDownList>

                </div>
                <div class="col-md-2">
                      Font Size in px
                    <asp:TextBox ID="txtTSize" CssClass="form-control" TextMode="Number" Width="100%" Min="8" Max="30" runat="server"
                        onkeyup="updateFontPreview()" onchange="updateFontPreview()">8</asp:TextBox>


                    <label id="fontPreviews" style=" font-size: 12px;">
                        Sample preview text
                    </label>
                </div>
                <div class="col-md-2">
                    Font Size in px
                    <asp:TextBox ID="txtFontSize" CssClass="form-control" TextMode="Number" Width="100%" Min="8" Max="30" runat="server"
                        onkeyup="updateFontPreview()" onchange="updateFontPreview()">8</asp:TextBox>


                    <label id="fontPreview" style=" font-size: 12px; ">
                        Sample preview text
                    </label>
                </div>

                <div class="col-md-2">
                    Font Family
                   <asp:DropDownList ID="ddlFontFamily" CssClass="form-control" runat="server" AutoPostBack="false"
                       OnChange="fontChanged()" ClientIDMode="Static">
                       <asp:ListItem Text="Arial" Value="Arial" />
                       <asp:ListItem Text="Calibri" Value="Calibri" />
                       <asp:ListItem Text="Tahoma" Value="Tahoma" />
                       <asp:ListItem Text="Verdana" Value="Verdana" />
                       <asp:ListItem Text="Times New Roman" Value="Times New Roman" />
                       <asp:ListItem Text="Courier New" Value="Courier New" />
                       <asp:ListItem Text="Segoe UI" Value="Segoe UI" />
                       <asp:ListItem Text="Poppins (Web)" Value="Poppins" />
                       <asp:ListItem Text="Roboto (Web)" Value="Roboto" />
                   </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                   Height in px
                    <asp:TextBox ID="TextBox1" CssClass="form-control" TextMode="Number" Min="8" Max="30" runat="server">8</asp:TextBox>

                </div>
                <div class="col-md-2">
                    Width in px
                    <asp:TextBox ID="TextBox2" CssClass="form-control" TextMode="Number" Min="8" Max="30" runat="server">8</asp:TextBox>
                </div>
                <div class="col-md-2">
                    Min Height of item Box in px
                    <asp:TextBox ID="TextBox3" CssClass="form-control" TextMode="Number" Min="8" Max="30" runat="server">8</asp:TextBox>

                </div>
                <div class="col-md-2">
                   Margin top in px
                    <asp:TextBox ID="TextBox4" CssClass="form-control" TextMode="Number" Min="8" Max="30" runat="server">8</asp:TextBox>



                </div>
                <div class="col-md-2">
                    Margin Bottom in px
                    <asp:TextBox ID="TextBox5" CssClass="form-control" TextMode="Number" Min="8" Max="30" runat="server">8</asp:TextBox>

                </div>
            </div>
            <div class="row">
                <div class="col-md-8">
                    Footer Text
                    <asp:TextBox CssClass="form-control" Width="100%" Height="100%"  TextMode="MultiLine" ID="txtFooterText" runat="server"></asp:TextBox>

                </div>
            </div>
        </div>

    </div>
</asp:Content>

