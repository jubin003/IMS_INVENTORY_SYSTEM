<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="Groups.aspx.cs" Inherits="Administration_Groups" %>

<%@ Register src="../uc/Groups.ascx" tagname="Groups" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <uc1:Groups ID="Groups1" runat="server" />
    
</asp:Content>

