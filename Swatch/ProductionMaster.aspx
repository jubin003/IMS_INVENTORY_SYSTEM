<%@ Page Title="Production Master Setup" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProductionMaster.aspx.cs" Inherits="ProductionMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="css/ims-webform.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <style>
        .select2-container .select2-selection--single {
            height: 34px; border: 1px solid #cbd5e1; border-radius: 6px; padding: 4px 12px;
        }
        .form-section { padding: 20px; }
    </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="imsv5-container">
                
                <div class="imsv5-page-header">
                    <div>
                        <h2 class="imsv5-page-title">Create Production Plan</h2>
                        <div class="imsv5-page-subtitle">Set up the master schedule and type for a new production run.</div>
                    </div>
                </div>

                <div class="bs-card">
                    <div class="card-toolbar">
                        <h4 style="margin: 0; font-size: 14px; font-weight: bold; color: #334155;">
                            <span style="color: #0284c7;">●</span> MASTER DETAILS
                        </h4>
                    </div>
                    <div class="form-section">
                        <div class="row" style="margin-bottom: 20px;">
                            <div class="col-md-4 form-group">
                                <label class="filter-label">Production Number <span class="req">*</span></label>
                                <asp:TextBox ID="txtProductionNo" runat="server" CssClass="form-control" placeholder="e.g. PRD-2026-001"></asp:TextBox>
                            </div>
                            <div class="col-md-4 form-group">
                                <label class="filter-label">Production Type <span class="req">*</span></label>
                                <asp:DropDownList ID="ddlProductionType" runat="server" CssClass="form-control chosen-select">
                                    <asp:ListItem Text="-- Select Type --" Value=""></asp:ListItem>
                                    <asp:ListItem Text="In-House Manufacturing" Value="InHouse"></asp:ListItem>
                                    <asp:ListItem Text="Sub-Contracting" Value="SubContract"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4 form-group">
                                <label class="filter-label">Customer Purchase Order <span class="req">*</span></label>
                                <asp:DropDownList ID="ddlCustomerPO" runat="server" CssClass="form-control chosen-select"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="row" style="margin-bottom: 20px;">
                            <div class="col-md-4 form-group">
                                <label class="filter-label">Status <span class="req">*</span></label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control chosen-select">
                                    <asp:ListItem Text="Planned" Value="Planned"></asp:ListItem>
                                    <asp:ListItem Text="In Progress" Value="InProgress"></asp:ListItem>
                                    <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4 form-group">
                                <label class="filter-label">Target Production Date</label>
                                <asp:TextBox ID="txtProductionDate" runat="server" CssClass="form-control nepali-date"></asp:TextBox>
                            </div>
                            <div class="col-md-4 form-group">
                                <label class="filter-label">Target Completion Date</label>
                                <asp:TextBox ID="txtCompletionDate" runat="server" CssClass="form-control nepali-date"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-bottom: 40px;">
                    <div class="col-md-8 text-left">
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-4 text-right">
                        <asp:Button ID="btnSaveMaster" runat="server" Text="Save Master Plan" CssClass="btn-view-primary" OnClick="btnSaveMaster_Click" />
                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script src="js/nepali.datepicker.v2.2.min.js"></script>
    
    <script type="text/javascript">
        function initializePlugins() {
            $('.chosen-select').select2({ width: '100%', allowClear: true });
            
            $('.nepali-date').nepaliDatePicker({
                npdMonth: true,
                npdYear: true,
                npdYearCount: 10
            });
        }
        
        $(document).ready(function () { 
            initializePlugins(); 
        });
        
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () { 
            initializePlugins(); 
        });
    </script>
</asp:Content>