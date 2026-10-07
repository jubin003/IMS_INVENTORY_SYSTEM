<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

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
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.15.3/css/fontawesome.min.css" />
    <link rel="stylesheet" href="assets/css/login.css">
</head>
<body>
    <main class="d-flex align-items-center min-vh-100 py-3 py-md-0">
        <div class="container">
            <div class="card login-card">
                <div class="row no-gutters">
                    <div class="col-md-5">
                        <img src="assets/images/login.jpg" alt="login" class="login-card-img" width="300px">
                    </div>
                    <div class="col-md-7">
                        <div class="card-body">
                            <div class="brand-wrapper">
                                <img src="assets/images/logo.png" alt="logo" class="logo">
                            </div>
                            <p class="login-card-description">Login</p>
                            <form id="loginform" runat="server">
                                <div class="form-group">
                                    <label for="username" class="sr-only">Username</label>
                                    <asp:TextBox ID="txtUserName" runat="server" placeholder="Username" autocomplete="off" class="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group mb-4">
                                    <label for="password" class="sr-only">Password</label>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="form-control" placeholder="Password"></asp:TextBox>

                                </div>
                                <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                                <asp:Button ID="btnSignIn" runat="server" Text="Sign In" cssclass="btn btn-block login-btn mb-4" OnClick="btnSignIn_Click" />

                            </form>
                            <a href="ForgotPassword.aspx" class="forgot-password-link">Forgot password?</a>
                            <asp:Label ID="lblLicenseMsg" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                            <a href="license.aspx" id="linkRenew" runat="server" visible="false">Renew Licence</a>
                             <a href="RegisterCompany.aspx" id="linkRegister" runat="server" visible="false">Register Company</a>
                         

                            <nav class="login-card-footer-nav">
                                <a href="https://phyegan.com/" target="_blank">About</a> |
                                <a href="TermsAndConditions.aspx"  target="_blank">Terms of use.</a> |
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
