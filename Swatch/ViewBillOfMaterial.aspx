<%@ Page Title="View Bill of Materials" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewBillOfMaterial.aspx.cs" Inherits="ViewBillOfMaterial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="css/ims-webform.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <style>
        .select2-container .select2-selection--single {
            height: 34px;
            border: 1px solid #ccc;
            border-radius: 4px;
            padding: 4px 12px;
        }
        .select2-container--default .select2-selection--single .select2-selection__arrow {
            height: 32px;
        }
        .select2-container--default .select2-selection--single .select2-selection__rendered {
            line-height: 24px;
            color: #333;
        }
    </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid" style="margin-top: 15px;">
                
                <!-- Page Header -->
                <div class="row" style="margin-bottom: 20px;">
                    <div class="col-md-6">
                        <h2 style="margin: 0; color: #0f172a; font-weight: bold;">Bill of Materials List</h2>
                    </div>
                    <div class="col-md-6 text-right">
                        <a href="BillOfMaterial.aspx" class="btn btn-primary">
                            <i class="fa fa-plus"></i> Create New BOM
                        </a>
                    </div>
                </div>

                <!-- Filter Card -->
                <div class="bs-card mb-4">
                    <div class="panel-body" style="padding: 20px;">
                        <div class="row">
                            <div class="col-md-6 form-group">
                                <label style="font-weight: bold; color: #475569;">Filter by Finished Product</label>
                                <asp:DropDownList ID="ddlFinishedProductFilter" runat="server" CssClass="form-control chosen-select"></asp:DropDownList>
                            </div>
                            <!-- Using an empty label ensures the button perfectly aligns with the dropdown -->
                            <div class="col-md-3 form-group">
                                <label>&nbsp;</label>
                                <asp:Button ID="btnFilter" runat="server" Text="Search" CssClass="btn btn-primary btn-block" OnClick="btnFilter_Click" />
                            </div>
                            <div class="col-md-3 form-group">
                                <label>&nbsp;</label>
                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-default btn-block" OnClick="btnReset_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <asp:Label ID="lblMessage" runat="server" CssClass="mb-3 d-block font-weight-bold"></asp:Label>

                <!-- Master BOM Table -->
                <div class="table-responsive" style="margin-bottom: 30px;">
                    <asp:GridView ID="gvBOMMaster" runat="server" AutoGenerateColumns="false" GridLines="None" 
                        CssClass="table table-bordered table-hover table-striped" OnRowCommand="gvBOMMaster_RowCommand">
                        <HeaderStyle BackColor="#f8fafc" ForeColor="#475569" Font-Bold="true" />
                        <Columns>
                            <asp:BoundField DataField="FINISHED_PRODUCT_ID" HeaderText="Product ID" Visible="false" />
                            <asp:BoundField DataField="PRODUCT_CODE" HeaderText="Product Code" ItemStyle-Font-Bold="true" />
                            <asp:BoundField DataField="PRODUCT_NAME" HeaderText="Finished Product Name" />
                            <asp:BoundField DataField="INGREDIENT_COUNT" HeaderText="Raw Materials Count" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            
                            <%-- Added ItemStyle-Wrap="false" to prevent buttons from stacking --%>
                            <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnView" runat="server" CommandName="ViewDetails" 
                                        CommandArgument='<%# Eval("FINISHED_PRODUCT_ID") %>' CssClass="btn btn-sm btn-info" style="color:white; margin-right:4px;">
                                        <i class="fa fa-eye"></i> View Recipe
                                    </asp:LinkButton>
                                    <a href='BillOfMaterial.aspx?id=<%# Eval("FINISHED_PRODUCT_ID") %>' class="btn btn-sm btn-warning" style="color:black; margin-right:4px;">
                                        <i class="fa fa-edit"></i> Edit
                                    </a>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteBOM" 
                                        CommandArgument='<%# Eval("FINISHED_PRODUCT_ID") %>' 
                                        OnClientClick="return confirm('Are you sure you want to delete this BOM recipe?');"
                                        CssClass="btn btn-sm btn-danger">
                                        <i class="fa fa-trash"></i> Delete
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="text-center text-muted" style="padding: 30px; background-color: #f9f9f9; border-radius: 4px;">
                                No Bill of Materials found.
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>

                <!-- Recipe Detail Drawer / Panel -->
                <asp:Panel ID="pnlDetails" runat="server" Visible="false" CssClass="bs-card" style="border: 1px solid #17a2b8; border-radius: 8px;">
                    <div class="panel-body" style="padding: 20px;">
                        
                        <!-- Header with floating layout to ensure right-alignment of button -->
                        <div style="margin-bottom: 15px; border-bottom: 1px solid #eee; padding-bottom: 10px;">
                            <h4 style="margin: 0; float: left; color: #334155; padding-top: 5px;">
                                Recipe Breakdown: <asp:Label ID="lblDetailProductName" runat="server" ForeColor="#0284c7" Font-Bold="true"></asp:Label>
                            </h4>
                            <asp:Button ID="btnCloseDetails" runat="server" Text="Close Details" CssClass="btn btn-default btn-sm pull-right" OnClick="btnCloseDetails_Click" />
                            <div style="clear: both;"></div> <!-- Clears the float -->
                        </div>
                        
                        <div class="table-responsive">
                            <asp:GridView ID="gvRecipeDetails" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-bordered table-striped">
                                <HeaderStyle BackColor="#f8fafc" ForeColor="#475569" Font-Bold="true" />
                                <Columns>
                                    <asp:BoundField DataField="MATERIAL_CODE" HeaderText="Material Code" />
                                    <asp:BoundField DataField="MATERIAL_NAME" HeaderText="Raw Material Name" ItemStyle-Font-Bold="true" />
                                    <asp:BoundField DataField="UNIT" HeaderText="Unit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="QUANTITY" HeaderText="Required Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Select2 Scripts -->
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script type="text/javascript">
        function initializeSelect2() {
            $('.chosen-select').select2({
                width: '100%',
                placeholder: "-- All Finished Products --",
                allowClear: true
            });
        }

        $(document).ready(function () {
            initializeSelect2();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            initializeSelect2();
        });
    </script>
</asp:Content>