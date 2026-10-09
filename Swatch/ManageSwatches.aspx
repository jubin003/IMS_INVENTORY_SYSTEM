<%@ Page Title="Manage Swatches" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageSwatches.aspx.cs" Inherits="ManageSwatches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <!-- Link updated CSS stylesheets -->
    <link href="css/ims-base.css" rel="stylesheet" />
    <link href="css/ims-form.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <style>
        .select2-container .select2-selection--single {
            height: 38px;
            border: 1px solid #cbd5e1;
            border-radius: 7px;
            padding: 5px 12px;
        }
        .select2-container--default .select2-selection--single .select2-selection__arrow {
            height: 36px;
        }
        .select2-container--default .select2-selection--single .select2-selection__rendered {
            line-height: 26px;
            color: #1e293b;
        }
        
        .swatch-thumb {
            width: 44px;
            height: 44px;
            object-fit: cover;
            border-radius: 6px;
            border: 1px solid #cbd5e1;
            box-shadow: 0 1px 2px rgba(0,0,0,0.05);
        }

        .action-link {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 30px;
            height: 30px;
            border-radius: 6px;
            transition: all 0.15s ease-in-out;
            text-decoration: none !important;
        }
        .action-edit {
            color: #d97706;
            background-color: #fef3c7;
        }
        .action-edit:hover {
            background-color: #fde68a;
            color: #b45309;
        }
        .action-delete {
            color: #dc2626;
            background-color: #fee2e2;
        }
        .action-delete:hover {
            background-color: #fecaca;
            color: #b91c1c;
        }
        .action-save {
            color: #059669;
            background-color: #d1fae5;
        }
        .action-save:hover {
            background-color: #a7f3d0;
            color: #047857;
        }
        .action-cancel {
            color: #475569;
            background-color: #f1f5f9;
        }
        .action-cancel:hover {
            background-color: #e2e8f0;
            color: #1e293b;
        }

            .custom-file-upload {
        position: relative;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        gap: 6px;
        width: 100%;
        height: 38px;
        background-color: #ffffff;
        border: 1px dashed #cbd5e1;
        border-radius: 7px;
        color: #475569;
        font-size: 12px;
        font-weight: 600;
        cursor: pointer;
        overflow: hidden;
        transition: all 0.15s ease-in-out;
        padding: 0 8px;
        box-sizing: border-box;
    }

    .custom-file-upload:hover {
        border-color: #0284c7;
        background-color: #f0f9ff;
        color: #0284c7;
    }

    .custom-file-upload input[type="file"] {
        position: absolute;
        left: 0;
        top: 0;
        opacity: 0;
        width: 100%;
        height: 100%;
        cursor: pointer;
    }

    .file-chosen-text {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        max-width: 110px;
    }
    </style>

    <div class="imsv5-container" style="margin-top: 15px;">
        
        <!-- Header -->
        <div class="imsv5-page-header">
            <div class="imsv5-title-block">
                <nav class="imsv5-breadcrumb" aria-label="Breadcrumb">
                    <span>Production</span>
                    <svg class="imsv5-icon-arrow" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path d="M9 5l7 7-7 7" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                    </svg>
                    <span class="active">Swatch Configuration</span>
                </nav>
                <h1 class="imsv5-page-title">Swatch Management</h1>
                <p class="imsv5-page-subtitle">Configure swatch name, status, and images for the selected category</p>
            </div>
        </div>

        <!-- Main Card -->
        <div class="bs-card">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="gvSwatches" />
                </Triggers>
                <ContentTemplate>
                    
                    <!-- Card Toolbar -->
                    <div class="card-toolbar">
                        <div style="display: flex; align-items: center; gap: 16px; flex-wrap: wrap; width: 100%;">
                            
                            <!-- Category Filter Dropdown -->
                            <div style="min-width: 280px;">
                                <asp:DropDownList ID="ddlFilterCategory" runat="server" CssClass="form-control chosen-select" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlFilterCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                            <!-- Search Input (Only renders/displays when a category is selected) -->
                            <asp:PlaceHolder ID="phSearchWrapper" runat="server" Visible="false">
                                <div class="search-box-wrapper">
                                    <svg class="search-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                                    </svg>
                                    <input type="text" id="clientSearchInput" class="table-search-input" placeholder="Search swatches..." onkeyup="filterSwatchTable()" />
                                </div>
                            </asp:PlaceHolder>

                            <div style="margin-left: auto;">
                                <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <!-- Grid Table -->
                    <div class="table-responsive">
<asp:GridView ID="gvSwatches" runat="server" AutoGenerateColumns="false" GridLines="None" 
    Width="100%" CssClass="enterprise-grid" 
    DataKeyNames="PK_ID, SWATCH_TYPE_ID, STATUS" 
    OnRowEditing="gvSwatches_RowEditing" 
    OnRowCancelingEdit="gvSwatches_RowCancelingEdit" 
    OnRowUpdating="gvSwatches_RowUpdating"
    OnRowDeleting="gvSwatches_RowDeleting"
    OnRowDataBound="gvSwatches_RowDataBound"
    Visible="false">
    <HeaderStyle BackColor="#f8fafc" />
    <Columns>
        
        <%-- Serial Number --%>
        <asp:TemplateField>
            <HeaderStyle CssClass="col-sn" />
            <ItemStyle CssClass="col-sn row-sn-text" />
            <HeaderTemplate>
                <div class="header-field-group">
                    <span class="header-label text-center" style="width: 100%;">SN</span>
                    <div style="height: 38px;"></div>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Container.DataItemIndex + 1 %>
            </ItemTemplate>
        </asp:TemplateField>

        <%-- Swatch Image Upload / Display --%>
        <asp:TemplateField>
            <HeaderStyle Width="160px" CssClass="text-center" />
            <ItemStyle CssClass="text-center" VerticalAlign="Middle" />
            <HeaderTemplate>
                <div class="header-field-group">
                    <span class="header-label">IMAGE</span>
                    <label class="custom-file-upload" title="Choose image">
                        <i class="fa fa-cloud-upload"></i>
                        <span class="file-chosen-text" id="addFileLabel">Choose Image</span>
                        <asp:FileUpload ID="fuAddImage" runat="server" onchange="updateFileName(this, 'addFileLabel')" accept="image/*" />
                    </label>
                </div>
            </HeaderTemplate>
            
            <ItemTemplate>
                <asp:Image ID="imgSwatch" runat="server" 
                    ImageUrl='<%# GetSwatchImageUrl(Eval("PK_ID")) %>' 
                    CssClass="swatch-thumb" 
                    AlternateText="Swatch Image" 
                    onclick="showImagePreview(this.src);"
                    style="cursor: zoom-in; transition: opacity 0.2s;"
                    onmouseover="this.style.opacity=0.8" 
                    onmouseout="this.style.opacity=1"
                    ToolTip="Click to enlarge" />
            </ItemTemplate>
            
            <EditItemTemplate>
                <label class="custom-file-upload" style="height: 32px; font-size: 11px;">
                    <i class="fa fa-camera"></i>
                    <span class="file-chosen-text" id='<%# "editFileLabel_" + Container.DataItemIndex %>'>Change</span>
                    <asp:FileUpload ID="fuEditImage" runat="server" onchange='<%# "updateFileName(this, \"editFileLabel_" + Container.DataItemIndex + "\")" %>' accept="image/*" />
                </label>
            </EditItemTemplate>
        </asp:TemplateField>

        <%-- Swatch Name --%>
        <asp:TemplateField>
            <HeaderStyle CssClass="col-name" />
            <ItemStyle CssClass="col-name" />
            <HeaderTemplate>
                <div class="header-field-group">
                    <span class="header-label">SWATCH NAME <span class="req">*</span></span>
                    <asp:TextBox ID="txtSwatchName" runat="server" CssClass="form-control inline-input" placeholder="Enter swatch name..."></asp:TextBox>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="cell-entity">
                    <span class="entity-bullet"></span>
                    <span style="font-weight: 600; color: #0f172a;"><%# Eval("SWATCH_NAME") %></span>
                </div>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtSwatchName" runat="server" Text='<%# Bind("SWATCH_NAME") %>' CssClass="form-control edit-input"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>

        <%-- Status --%>
        <asp:TemplateField>
            <HeaderStyle CssClass="col-status" />
            <ItemStyle CssClass="col-status" />
            <HeaderTemplate>
                <div class="header-field-group">
                    <span class="header-label">STATUS</span>
                    <asp:DropDownList ID="ddlHeaderStatus" runat="server" CssClass="form-control inline-select">
                        <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <span class='<%# Eval("STATUS").ToString() == "1" ? "status-pill status-active" : "status-pill status-inactive" %>'>
                    <%# Eval("STATUS").ToString() == "1" ? "Active" : "Inactive" %>
                </span>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlEditStatus" runat="server" CssClass="form-control edit-select">
                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>

        <%-- Actions --%>
        <asp:TemplateField>
            <HeaderStyle CssClass="col-actions" />
            <ItemStyle CssClass="col-actions" />
            <HeaderTemplate>
                <div class="header-action-group">
                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="+ Add" CssClass="btn-add-primary" style="height: 38px;" />
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="action-btn-group">
                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CssClass="action-link action-edit" ToolTip="Edit Swatch">
                        <i class="fa fa-pencil"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="action-link action-delete" ToolTip="Delete Swatch" OnClientClick="return confirm('Are you sure you want to delete this swatch?');">
                        <i class="fa fa-trash"></i>
                    </asp:LinkButton>
                </div>
            </ItemTemplate>
            <EditItemTemplate>
                <div class="action-btn-group">
                    <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="action-link action-save" ToolTip="Save Changes">
                        <i class="fa fa-check"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="action-link action-cancel" ToolTip="Cancel">
                        <i class="fa fa-times"></i>
                    </asp:LinkButton>
                </div>
            </EditItemTemplate>
        </asp:TemplateField>

    </Columns>
</asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Select2 & Client Filtering Scripts -->
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script type="text/javascript">
        function initializeSelect2() {
            $('.chosen-select').select2({
                width: '100%',
                placeholder: "-- Select Category --",
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

        function filterSwatchTable() {
            var input = document.getElementById("clientSearchInput");
            if (!input) return;
            var filter = input.value.toLowerCase();
            var grid = document.getElementById("<%= gvSwatches.ClientID %>");
            if (!grid) return;
            var rows = grid.getElementsByTagName("tr");
    
            for (var i = 1; i < rows.length; i++) {
                if (rows[i].classList.contains("gridview-pager")) continue;
                // Column index 2 is Swatch Name (0: SN, 1: Image, 2: Name)
                var nameCol = rows[i].getElementsByTagName("td")[2];
                if (nameCol) {
                    var nameValue = nameCol.textContent || nameCol.innerText;
                    rows[i].style.display = nameValue.toLowerCase().indexOf(filter) > -1 ? "" : "none";
                }
            }
        }

        function showImagePreview(imageSrc) {
            if (imageSrc.indexOf('profile.png') > -1) return;
            document.getElementById('vanillaModalImg').src = imageSrc;
            document.getElementById('vanillaImageModal').style.display = 'block';
        }

        function closeImagePreview() {
            document.getElementById('vanillaImageModal').style.display = 'none';
        }

        window.onclick = function (event) {
            var imgModal = document.getElementById('vanillaImageModal');
            if (event.target == imgModal) closeImagePreview();
        };


        function updateFileName(input, targetLabelId) {
            var label = document.getElementById(targetLabelId);
            if (!label) return;
            if (input.files && input.files.length > 0) {
                label.innerText = input.files[0].name;
            } else {
                label.innerText = "Choose Image";
            }
        }
    </script>

    <!-- Modal for Zoom -->
    <div id="vanillaImageModal" style="display:none; position:fixed; z-index:9999; left:0; top:0; width:100%; height:100%; background-color:rgba(15, 23, 42, 0.85); backdrop-filter: blur(4px);">
        <span onclick="closeImagePreview()" style="position:absolute; top:20px; right:35px; color:#f8fafc; font-size:40px; font-weight:bold; cursor:pointer;" onmouseover="this.style.color='#ef4444'" onmouseout="this.style.color='#f8fafc'">&times;</span>
        <img id="vanillaModalImg" style="margin:auto; display:block; max-width:90%; max-height:85vh; position:relative; top:50%; transform:translateY(-50%); border-radius: 8px; box-shadow: 0 10px 25px rgba(0,0,0,0.5);" />
    </div>
</asp:Content>