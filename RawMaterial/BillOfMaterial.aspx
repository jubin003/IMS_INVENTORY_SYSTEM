<%@ Page Title="Manage Bill of Materials" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BillOfMaterial.aspx.cs" Inherits="BillOfMaterial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
    <link href="css/ims-webform.css" rel="stylesheet" />
    
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
                
                <div class="row" style="margin-bottom: 20px;">
                    <div class="col-md-6">
                        <h2 style="margin: 0; color: #0f172a; font-weight: bold;">Manage Bill of Materials</h2>
                    </div>
                    <div class="col-md-6 text-right">
                        <a href="ViewBillOfMaterial.aspx" class="btn btn-default">
                            <i class="fa fa-list"></i> View All BOMs
                        </a>
                    </div>
                </div>

                <div class="bs-card mb-4" style="margin-bottom: 20px;">
                    <div class="card-toolbar" style="border-bottom: 1px solid #f1f5f9; padding: 15px;">
                        <h4 style="margin: 0; font-size: 14px; font-weight: bold; color: #334155;">
                            <span style="color: #0284c7;">●</span> 1. SELECT FINISHED PRODUCT
                        </h4>
                    </div>
                    <div class="panel-body" style="padding: 20px;">
                        <div class="row">
                            <div class="col-md-6 form-group">
                                <label style="font-weight: bold; color: #475569;">Finished Product <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlFinishedProduct" runat="server" CssClass="form-control chosen-select" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlFinishedProduct_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="bs-card">
                    <div class="card-toolbar" style="border-bottom: 1px solid #f1f5f9; padding: 15px;">
                        <h4 style="margin: 0; font-size: 14px; font-weight: bold; color: #334155;">
                            <span style="color: #0284c7;">●</span> 2. ADD RAW MATERIALS TO RECIPE
                        </h4>
                    </div>
                    <div class="panel-body" style="padding: 20px;">
                        
                        <div class="row" style="margin-bottom: 20px; align-items: flex-end;">
                            <div class="col-md-5 form-group mb-0">
                                <label style="font-weight: bold; color: #475569;">Raw Material <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlRawMaterial" runat="server" CssClass="form-control chosen-select"></asp:DropDownList>
                            </div>
                            <div class="col-md-4 form-group mb-0">
                                <label style="font-weight: bold; color: #475569;">Required Quantity <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" placeholder="Enter qty"></asp:TextBox>
                            </div>
                            <div class="col-md-3 form-group mb-0" style="margin-top: 24px;">
                                <asp:Button ID="btnAddMaterial" runat="server" Text="+ Add Material" CssClass="btn btn-primary btn-block" OnClick="btnAddMaterial_Click" />
                            </div>
                        </div>

                        <div class="table-responsive">
                            <asp:GridView ID="gvBOMDetails" runat="server" AutoGenerateColumns="false" GridLines="None" 
                                CssClass="table table-bordered table-hover table-striped"
                                DataKeyNames="MATERIAL_ID"
                                OnRowDeleting="gvBOMDetails_RowDeleting"
                                OnRowEditing="gvBOMDetails_RowEditing"
                                OnRowUpdating="gvBOMDetails_RowUpdating"
                                OnRowCancelingEdit="gvBOMDetails_RowCancelingEdit">
                                <HeaderStyle BackColor="#f8fafc" ForeColor="#475569" Font-Bold="true" />
                                <Columns>
                                    <asp:BoundField DataField="MATERIAL_ID" HeaderText="Material ID" Visible="false" ReadOnly="true" />
                                    <asp:BoundField DataField="MATERIAL_NAME" HeaderText="Raw Material Name" ItemStyle-Font-Bold="true" ReadOnly="true" />
                                    <asp:BoundField DataField="UNIT" HeaderText="Unit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ReadOnly="true" />
                                    
                                    <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Eval("QTY") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditQty" runat="server" Text='<%# Bind("QTY") %>' CssClass="form-control input-sm text-right" style="max-width: 100px; display: inline-block;"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="140px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditRow" runat="server" CommandName="Edit" CssClass="text-primary mr-2" style="text-decoration:none; font-weight:bold; margin-right: 8px;" CausesValidation="false">
                                                <i class="fa fa-pencil"></i> Edit
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDeleteRow" runat="server" CommandName="Delete" CssClass="text-danger" style="text-decoration:none; font-weight:bold;" CausesValidation="false">
                                                <i class="fa fa-trash"></i> Remove
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="btnUpdateRow" runat="server" CommandName="Update" CssClass="text-success mr-2" style="text-decoration:none; font-weight:bold; margin-right: 8px;" CausesValidation="false">
                                                <i class="fa fa-check"></i> Update
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnCancelRow" runat="server" CommandName="Cancel" CssClass="text-muted" style="text-decoration:none; font-weight:bold;" CausesValidation="false">
                                                <i class="fa fa-times"></i> Cancel
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="text-center text-muted" style="padding: 30px; background-color: #f9f9f9; border-radius: 4px;">
                                        No raw materials added yet. Select a material and enter quantity above to build the recipe.
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-top: 20px; margin-bottom: 40px;">
                    <div class="col-md-8 text-left">
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-4 text-right">
                        <asp:Button ID="btnSaveBOM" runat="server" Text="Save Final BOM" CssClass="btn btn-success btn-lg" OnClick="btnSaveBOM_Click" />
                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script type="text/javascript">
        
        function initializeSelect2() {
            $('.chosen-select').select2({
                width: '100%',
                placeholder: "-- Search and select --",
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