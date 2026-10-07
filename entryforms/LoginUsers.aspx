<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"CodeFile="LoginUsers.aspx.cs" 
Inherits="entryforms_LoginUsers" Title="Untitled Page" %>
<%@ Register src="../uc/LoginUsers.ascx" tagname="LoginUsers" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
 
    <uc1:LoginUsers ID="LoginUsers1" runat="server" />
   
    </asp:Content>

