<%@ Page Language="C#" AutoEventWireup="true" CodeFile="License.aspx.cs" Inherits="License" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IMS</title>
    <link rel="icon" href="images/icons/logo.png" type="image/x-icon" />
    <link rel="stylesheet" type="text/css" href="~/css/layout.css" />
    <link id="csstheme" rel="stylesheet" type="text/css" href="~/css/theme/default.css" />
    <link href="~/css/Custom-cs.css" rel="stylesheet" />
         <!-- Bootstrap For Nepali Calender-->
    
    <link href="~/css/jquery-ui.min.css" rel="stylesheet" />
    <link href="~/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="~/css/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="~/css/nepali.datepicker.v2.2.min.css" rel="stylesheet" />
   
    <!-- Bootstrap For Nepali Calender END-->  
  


    <link href="~/css/fresh-bootstrap-table.css" rel="stylesheet" />
    <link href="~/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="~/css/print.min.css" media="print" />
    <link href="~/css/Custom-cs.css" rel="stylesheet" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-modal/2.2.6/css/bootstrap-modal.min.css" />

    <link href="~/css/animate.css" rel="stylesheet" />

    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.css" />
</head>
<body>
    <div class="navbar navbar-default navbar-fixed-top" role="navigation">
        <div style="padding-right: 100px">
            <div class="navbar-header">
                <a href="~/Default.aspx" class="navbar-brand" runat="server">
                    <span class="">
                         <asp:Image ID="Image1" runat="server" ImageUrl="~/images/topbar_logo.png" Width="150px" /></span>
                </a>
            </div>
        </div>
    </div>

    <div id="main-login">
        <div id="login">
            <div id="heading" style="margin-top:50px">
                <div class="big">
                    License Renew                   
                </div>
                <br />
                <asp:Label ID="lblPreMsgExpired" runat="server" Text="" ForeColor="Red"></asp:Label>
                <asp:Label ID="lblPreMsgRemainingDays" runat="server" Text=""></asp:Label>
            </div>
            <div id="form" style="width: 500px;">
                <form id="loginform" runat="server">
                    <div id="key_entry_form" runat="server">
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <div>
                            <div>
                                Enter the serial number for renewal of software.                                
                            </div>
                            <asp:TextBox ID="txtkey1" runat="server" placeholder="XXXX" autocomplete="off" Width="70px" Height="30px"
                                Style="padding: 5px 10px; margin: 5px 0; text-align: center;" MaxLength="4" Font-Bold="True"></asp:TextBox>-                      
                            <asp:TextBox ID="txtkey2" runat="server" placeholder="XXXX" autocomplete="off" Width="70px" Height="30px"
                                Style="padding: 5px 10px; margin: 5px 0; text-align: center;" MaxLength="4" Font-Bold="True"></asp:TextBox>-                       
                            <asp:TextBox ID="txtkey3" runat="server" placeholder="XXXX" autocomplete="off" Width="70px" Height="30px"
                                Style="padding: 5px 10px; margin: 5px 0; text-align: center;" MaxLength="4" Font-Bold="True"></asp:TextBox>-                       
                            <asp:TextBox ID="txtkey4" runat="server" placeholder="XXXX" autocomplete="off" Width="70px" Height="30px"
                                Style="padding: 5px 10px; margin: 5px 0; text-align: center;" MaxLength="4" Font-Bold="True"></asp:TextBox>
                        </div>

                        <div id="error-message">
                            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                        <div style="text-align: right;">
                            <asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click" />
                        </div>
                    </div>
                    <div id="renew" runat="server" visible="false">
                        <h2><strong>
                            <asp:Label ID="lblRenewMsg" runat="server" ForeColor="Red"></asp:Label>
                        </strong></h2>
                        <asp:Button ID="btnNext" runat="server" Text="Get in Login Page" OnClick="btnNext_Click" />
                    </div>
                </form>
            </div>


 


        </div>
    </div>
    <!------------Footer------------>

    <footer class="footer-pos">
       
            <div class="col-md-12 text-left">
                <p class="font-12"> Copy Right Phye Gan</p>
            </div>
    </footer>




    <!------------Footer------------>
</body>
</html>
