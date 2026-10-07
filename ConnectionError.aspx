<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConnectionError.aspx.cs" Inherits="ConnectionError" %>

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
    <style type="text/css">
        .auto-style1 {
            position: relative;
            min-height: 1px;
            top: 0px;
            left: 0px;
            float: left;
            width: 100%;
            font-size: large;
            padding-left: 15px;
            padding-right: 15px;
        }
    </style>
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
        <div class="row">
            <div class="col-md-12" style="text-align:center">
                 <asp:Image ID="Image2" runat="server" ImageUrl="~/images/icons/7001ConnectionError.gif" />
            </div>
        </div>
          <div class="row">
            <div class="auto-style1" style="text-align:center">
                <strong>Connection Error Please Contact Administrator
            </strong>
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
