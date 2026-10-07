<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true"
    CodeFile="GENERATE.aspx.cs"
    Inherits="DYNAMICQR_GENERATE"
    Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="container">

        <div class="row">

            <div class="col-md-4">

                <label>Amount</label>

                <asp:TextBox
                    ID="txtAmount"
                    runat="server"
                    CssClass="form-control"
                    Text="0.00">
                </asp:TextBox>

                <br />

                <asp:Button
                    ID="btnGenerateQR"
                    runat="server"
                    CssClass="btn btn-primary"
                    Text="Generate Dynamic QR"
                    OnClick="btnGenerateQR_Click" />

                <br /><br />

                <asp:Label
                    ID="lblMessage"
                    runat="server"
                    CssClass="text-success">
                </asp:Label>

            </div>

            <div class="col-md-4">

                <asp:Image
                    ID="imgQR"
                    runat="server"
                    Width="250px"
                    Height="250px" />

            </div>

        </div>

    </div>

</asp:Content>