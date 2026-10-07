<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="privacysetting.aspx.cs" Inherits="privacysetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="container">
        <div id="main-privacy">
            <h2>Privacy Setting</h2>


            <div class="privacy-container">
                <div class="container-heading">
                    Change Display Name
                <div class="container-collapse">
                </div>

                </div>
                <div class="container-body">
                    <div class="input">
                        <span class="input-label">Display Name</span>

                    </div>
                    <div class="input">

                        <asp:TextBox ID="txtDisplayName" runat="server" Height="30px" Width="250px" Style="font-size: 1.2em"></asp:TextBox>
                    </div>
                    <div class="input">

                        <asp:Button ID="btnSaveName" runat="server" Height="30px" Style="padding: 0 20px" Text="Save" OnClick="btnSaveName_Click" />


                        <asp:Button ID="Button2" runat="server" Height="30px" Style="padding: 0 20px" Text="Cancel" />


                    </div>
                </div>
            </div>

            <div class="privacy-container">
                <div class="container-heading">
                    Change Password
                <div class="container-collapse">
                </div>

                </div>
                <div class="container-body-password">
                    <div class="input">
                        <span class="input-label">Old Password</span>
                    </div>
                    <div class="input">
                        <asp:TextBox ID="txtOldPassword" runat="server" Height="30px" Width="250px" TextMode="Password" Style="font-size: 1.2em"></asp:TextBox>
                    </div>
                    <div class="input">
                        <span class="input-label">New Password</span>
                    </div>
                    <div class="input">
                        <asp:TextBox ID="txtNewPassword" runat="server" Height="30px" Width="250px" TextMode="Password" Style="font-size: 1.2em"></asp:TextBox>
                    </div>
                    <div class="input">
                        <span class="input-label">Confirm Password</span>
                    </div>
                    <div class="input">
                        <asp:TextBox ID="txtConfirmPassword" runat="server" Height="30px" Width="250px" TextMode="Password" Style="font-size: 1.2em"></asp:TextBox>
                    </div>
                    <div class="input">
                        <asp:Button ID="btnSavePassword" runat="server" Height="30px" Style="padding: 0 20px" Text="Save" OnClick="btnSavePassword_Click" />
                        <asp:Button ID="btnCancelPassword" runat="server" Height="30px" Style="padding: 0 20px" Text="Cancel" OnClick="btnCancelPassword_Click" />
                    </div>
                    <div class="input">
                        <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="privacy-container">
                <div class="container-heading">
                    Change Profile Picture
                <div class="container-collapse">
                </div>

                </div>
                <div class="container-body">
                    <div class="input">
                        <asp:Image ID="imgProfilePic" runat="server" Width="100px" />
                    </div>
                    <div class="input">
                        <asp:FileUpload ID="FileUpload1" runat="server" Height="30px" />
                    </div>
                    <div class="input">

                        <asp:Button ID="btnSaveImage" runat="server" Height="30px" Style="padding: 0 20px" Text="Save" OnClick="btnSaveImage_Click" />

                        <asp:Button ID="Button6" runat="server" Height="30px" Style="padding: 0 20px" Text="Cancel" />
                    </div>

                </div>
            </div>

            <div class="privacy-container">
                <div class="container-heading">
                    Change Recovery Email
                <div class="container-collapse">
                </div>

                </div>
                <div class="container-body">
                    <div class="input">
                        <span class="input-label">Your Email Address</span>

                    </div>
                    <div class="input">

                        <asp:TextBox ID="txtRecoveryEmail" runat="server" Height="30px" Width="250px" Style="font-size: 1.2em"></asp:TextBox>
                    </div>
                    <div class="input">

                        <asp:Button ID="btnSaveEmail" runat="server" Height="30px" Style="padding: 0 20px" Text="Save" OnClick="btnSaveEmail_Click" />


                        <asp:Button ID="Button4" runat="server" Height="30px" Style="padding: 0 20px" Text="Cancel" />


                    </div>
                </div>
            </div>


        </div>
    </div>
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#main-privacy>.privacy-container > .container-heading > .container-collapse').html('&#9660');

            $('#main-privacy>.privacy-container > .container-body').slideUp('fast');

            $('#main-privacy>.privacy-container > .container-heading > .container-collapse').click(function () {


                $(this).parent().next().slideToggle();


                if ($(this).parent().next().height() == 1) {

                    $(this).html('&#9650');
                }
                else {
                    $(this).html('&#9660');
                }


            });



            if ($('#main-privacy>.privacy-container > .container-body-password').height() == 255) {
                $('#main-privacy>.privacy-container > .container-body-password').slideUp('fast');
            }
            else {

            }


        });
    </script>

</asp:Content>

