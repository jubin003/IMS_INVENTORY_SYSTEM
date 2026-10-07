<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordReset.aspx.cs" Inherits="PasswordReset" %>

<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>IMS</title>
    <link rel="icon" href="assets/images/logo.png" type="image/x-icon" />
    <link href="https://fonts.googleapis.com/css?family=Karla:400,700&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="https://cdn.materialdesignicons.com/4.8.95/css/materialdesignicons.min.css">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css">
    <link rel="stylesheet" href="assets/css/login.css">
</head>
<body>
    <main class="d-flex align-items-center min-vh-100 py-3 py-md-0">
        <div class="container">
            <div class="card login-card">
                <div class="row no-gutters">
                    <div class="col-md-5">
                     <img src="assets/images/login.jpg" alt="login" class="login-card-img">
                    </div>
                    <div class="col-md-7">
                        <div class="card-body">
                            <div class="brand-wrapper">
                                <img src="assets/images/logo.png" alt="logo" class="logo">
                            </div>
                            <p class="login-card-description">Forget Password Reset


                            </p>
            <div id="form">
                <form id="loginform" runat="server">

                    <div class="input">
                        <asp:TextBox ID="txtEmployeeID" runat="server" Width="250px" Height="30px" Style="padding: 5px 10px; margin: 5px 0; display: block" Visible="false" ReadOnly="True"></asp:TextBox>
                    </div>
                    <div class="input">
                        <asp:TextBox ID="txtUserName" runat="server" placeholder="Enter Your Username" autocomplete="off" Width="250px" Height="30px" Style="padding: 5px 10px; margin: 5px 0; display: block" ReadOnly="True"></asp:TextBox>
                    </div>
                    <div class="input">
                        <asp:Label ID="txtConfirmCode" runat="server" Width="250px" Height="30px" Style="padding: 5px 10px; margin: 5px 0; display: block" Visible="false"></asp:Label>
                    </div>
                    <div class="input">
                        <asp:TextBox ID="txtPassword" runat="server" placeholder="New Password" Width="250px" Height="30px" Style="padding: 5px 10px; margin: 5px 0; display: block" TextMode="Password"></asp:TextBox>
                    </div>
                    <div class="input">
                        <asp:TextBox ID="txtRePassword" runat="server" placeholder="Confirm Password" Width="250px" Height="30px" Style="padding: 5px 10px; margin: 5px 0; display: block" TextMode="Password"></asp:TextBox>
                    </div>
                    <div id="error-message">
                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Style="font-size: 15px"></asp:Label>
                    </div>
                    <div class="input">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="250px" Height="30px" Style="padding: 5px 10px; margin: 5px 0; display: block; color: white; background-color: #4d90fe;" OnClick="btnSave_Click" />
                    </div>
                </form>
            <a href="ForgotPassword.aspx" class="forgot-password-link">Forgot password?</a>
                              <asp:Label ID="lblLicenseMsg" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                    <a href="license.aspx" id="linkRenew" runat="server" visible="false">Renew Licence</a>
                            
                            <nav class="login-card-footer-nav">
                                <a href="#!">Terms of use.</a>
                                <a href="#!">Privacy policy</a>
                            </nav>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </main>
    <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js"></script>
</body>
</html>