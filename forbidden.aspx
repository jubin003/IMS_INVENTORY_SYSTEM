<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="forbidden.aspx.cs" Inherits="forbidden" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
       <div style="text-align: center">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/401Error.gif" Width="400px" />
        </div>
        <div style="text-align: center">
          
            <p class="auto-style3">We are sorry, but you do not have access to this page or resource.
            </p>
        </div>
        
    </div>
</asp:Content>

