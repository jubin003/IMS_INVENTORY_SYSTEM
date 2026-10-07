<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DebitNote.aspx.cs" Inherits="Account_Utilities_DebitNote" %>

<%@ Register Src="~/Account/Utilities/uc/voucher.ascx" TagPrefix="uc1" TagName="voucher" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:voucher runat="server" ID="voucher" />
</asp:Content>

