<%@ Page Title="Nizi POS Test"
    Language="C#"
    MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true"
    CodeFile="NiziTest.aspx.cs"
    Inherits="DYNAMICQR_NiziTest"
    Async="true" %>
<asp:Content ID="Content1"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">

    <div class="container">

        <h2>Nizi POS Test</h2>

        <div class="form-group">
            <label>Amount</label>
            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Text="100.00"></asp:TextBox>
        </div>

        <div class="form-group">
            <label>Scan Text</label>
            <asp:TextBox ID="txtScan" runat="server" CssClass="form-control" Text="SCAN TO PAY"></asp:TextBox>
        </div>

        <div class="form-group">
            <label>QR Payload</label>
            <asp:TextBox ID="txtPayload" runat="server"
                CssClass="form-control"
                TextMode="MultiLine"
                Rows="5">HELLO123456789</asp:TextBox>
        </div>

        <br />

        <asp:Button ID="btnQR" runat="server" Text="Show QR" CssClass="btn btn-primary" OnClick="btnQR_Click" />

        <asp:Button ID="btnPass" runat="server" Text="PASS" CssClass="btn btn-success" OnClick="btnPass_Click" />

        <asp:Button ID="btnFail" runat="server" Text="FAIL" CssClass="btn btn-danger" OnClick="btnFail_Click" />

        <asp:Button ID="btnWait" runat="server" Text="WAIT" CssClass="btn btn-warning" OnClick="btnWait_Click" />

        <asp:Button ID="btnIdle" runat="server" Text="IDLE" CssClass="btn btn-secondary" OnClick="btnIdle_Click" />

        <hr />

        <asp:Label ID="lblResult" runat="server"></asp:Label>

    </div>

</asp:Content>