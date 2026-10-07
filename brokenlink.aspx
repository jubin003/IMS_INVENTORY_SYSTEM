<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="brokenlink.aspx.cs" Inherits="brokenlink" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="container-fluid">
       <div style="text-align: center">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/404Error.gif" Width="400px" />
        </div>
        <div style="text-align: center">
           
            <p >We are sorry, but page your are trying to access not found.
            </p>
        </div>
        
    </div>
</asp:Content>

