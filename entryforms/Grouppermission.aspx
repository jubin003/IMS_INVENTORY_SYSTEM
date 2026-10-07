<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Grouppermission.aspx.cs" Inherits="Administration_Grouppermission" %>
<%@ Register src="../uc/GroupPermission.ascx" tagname="GroupPermission" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
   
    <uc1:GroupPermission ID="GroupPermission1" runat="server" />
   
   
</asp:Content>

