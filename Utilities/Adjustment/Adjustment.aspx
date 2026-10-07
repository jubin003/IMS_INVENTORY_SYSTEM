  <%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Adjustment.aspx.cs" Inherits="Utilities_Adjustment_Adjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="container-fluid">      
     
        <div class="row" runat="server" id="divAdd">
            <div class="col-md-6">
                <div class="row">
                    <div class="col-md-6">
                        Product Category<br />
                        <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="form-control"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlProductCategory_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        Product Sub Category
                    <br />
                        <asp:DropDownList ID="ddlProductSubCategory" runat="server" CssClass="form-control"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlProductSubCategory_SelectedIndexChanged">
                        </asp:DropDownList>

                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        Product Name<br />
                        <asp:DropDownList ID="ddlproduct" runat="server" CssClass="form-control"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlproduct_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3" runat="server" id="divPackQty" visible="false">
                        Pack Qty
                    <br />
                        <asp:Label ID="lblPackQty" runat="server" Text="" CssClass="form-control" Enabled="false"></asp:Label>
                    </div>
                    <div class="col-md-3" runat="server" id="divUnit" visible="false">
                        Unit
                    <br />
                        <asp:Label ID="lblUnit" runat="server" Text="" CssClass="form-control" Enabled="false"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6" runat="server" id="divPColour" visible="false" >
                        Product Color
                    <br />
                        <asp:DropDownList ID="ddlProductColor" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6" runat="server" id="divPSize" visible="false">
                        Product Size
                    <br />
                        <asp:DropDownList ID="ddlProductSize" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Applicable" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Not Applicable" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6" runat="server" id="divPManufacturer" visible="false">
                        Product Manufacturer
                    <br />
                        <asp:DropDownList ID="ddlProductManufacturer" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Applicable" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Not Applicable" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6" runat="server" id="divPExpiry" visible="false">
                        Expiry Date<br />
                        <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-6" runat="server" id="divPBatch" visible="false">
                        Batch<br />
                        <asp:TextBox ID="txtBatch" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-6" runat="server" id="divQuantity" visible="false">
                        Quantity<br />
                        <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" ></asp:TextBox>
                    </div>
                   <div class="col-md-6" runat="server" id="divRemarks" visible="false">
                        Remarks<br />
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                     <div class="col-md-6" style="margin-top: 20px;">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-success" Width="100%" Visible="false" OnClick="btnAdd_Click" />

                    </div>

                    
                </div>
            </div>
        </div>
    </div>

</asp:Content>

