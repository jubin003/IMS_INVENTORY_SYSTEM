<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreditNote.aspx.cs"
    Inherits="Account_Utilities_CreditNote" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Account/Utilities/uc/voucher.ascx" TagPrefix="uc1" TagName="voucher" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:voucher runat="server" ID="voucher" />
</asp:Content>

