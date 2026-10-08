<%@ Page Title="Manage Swatches" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageSwatches.aspx.cs" Inherits="ManageSwatches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="css/ims-webform.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <style>
        .select2-container .select2-selection--single {
            height: 34px;
            border: 1px solid #cbd5e1;
            border-radius: 4px;
            padding: 4px 10px;
        }
        .select2-container--default .select2-selection--single .select2-selection__arrow {
            height: 32px;
        }
        .select2-container--default .select2-selection--single .select2-selection__rendered {
            line-height: 24px;
            color: #1e293b;
        }
        
        .swatch-thumb {
            width: 40px;
            height: 40px;
            object-fit: cover;
            border-radius: 4px;
            border: 1px solid #cbd5e1;
        }

        .enterprise-grid th {
            vertical-align: bottom !important;
            padding-bottom: 12px !important;
        }
        .header-field-group {
            display: flex;
            flex-direction: column;
            gap: 6px;
        }
        .header-label {
            font-size: 12px;
            font-weight: 700;
            color: #475569;
            text-transform: uppercase;
        }
        
        .enterprise-grid th.col-actions,
        .enterprise-grid td.col-actions {
            width: 120px !important;
            vertical-align: middle !important;
            text-align: center !important;
        }
    </style>

    <div class="imsv5-container" style="margin-top: 15px;">
        
        <div class="imsv5-page-header">
            <h2 style="margin: 0; color: #0f172a; font-weight: bold;">Manage Swatches</h2>
        </div>
        
        <div class="bs-card">
            <!-- Everything wrapped in UpdatePanel for smooth filtering -->
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="gvSwatches" />
                </Triggers>
                <ContentTemplate>
                    
                    <!-- Search & Toolbar -->
                    <div class="card-toolbar" style="border-bottom: 1px solid #f1f5f9; padding: 15px;">
                        <div class="row" style="width: 100%; margin: 0; align-items: center;">
                            
                            <!-- Master Category Filter -->
                            <div class="col-md-4" style="padding-left: 0;">
                                <asp:DropDownList ID="ddlFilterCategory" runat="server" CssClass="form-control chosen-select" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlFilterCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                            <!-- Search Box -->
                            <div class="col-md-4">
                                <div style="position: relative; width: 100%;">
                                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24" style="position: absolute; left: 10px; top: 8px; width: 18px; color: #94a3b8;">
                                        <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"></path>
                                    </svg>
                                    <input type="text" id="clientSearchInput" class="form-control" style="width: 100%; padding-left: 35px; border-radius: 6px; height: 34px;" placeholder="Search swatch name..." onkeyup="filterSwatchTable()" />
                                </div>
                            </div>

                            <!-- Message Label -->
                            <div class="col-md-4 text-right">
                                <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <!-- Grid Data -->
                    <div class="table-responsive">
                        <asp:GridView ID="gvSwatches" runat="server" AutoGenerateColumns="false" GridLines="None" 
                            Width="100%" CssClass="enterprise-grid table table-hover table-bordered" 
                            DataKeyNames="PK_ID, SWATCH_TYPE_ID, STATUS" 
                            OnRowEditing="gvSwatches_RowEditing" 
                            OnRowCancelingEdit="gvSwatches_RowCancelingEdit" 
                            OnRowUpdating="gvSwatches_RowUpdating"
                            OnRowDeleting="gvSwatches_RowDeleting"
                            Visible="false">
                            <HeaderStyle BackColor="#f8fafc" />
                            <Columns>
                                
                                <%-- SN Column --%>
                                <asp:TemplateField HeaderText="SN">
                                    <HeaderStyle Width="50px" CssClass="text-center" />
                                    <ItemStyle CssClass="text-center text-muted fw-bold" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%-- Image Column --%>
                                <asp:TemplateField HeaderText="IMAGE">
                                    <HeaderStyle Width="140px" CssClass="text-center" />
                                    <ItemStyle CssClass="text-center" VerticalAlign="Middle" />
                                    
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">UPLOAD</span>
                                            <asp:FileUpload ID="fuAddImage" runat="server" CssClass="form-control" style="height: 34px; padding: 4px;" />
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
                                        <asp:FileUpload ID="fuEditImage" runat="server" CssClass="form-control" style="height: 34px; padding: 4px;" />
                                        <small class="text-muted" style="display:block; margin-top:4px; font-size: 11px;">Leave empty to keep current</small>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- Swatch Name Column --%>
                                <asp:TemplateField>
                                    <HeaderStyle Width="35%" />
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">SWATCH NAME <span class="req text-danger">*</span></span>
                                            <asp:TextBox ID="txtSwatchName" runat="server" CssClass="form-control" placeholder="Enter swatch name..."></asp:TextBox>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div class="cell-entity" style="display: flex; align-items: center; gap: 8px;">
                                            <span class="entity-bullet" style="width: 6px; height: 6px; border-radius: 50%; background-color: #cbd5e1;"></span>
                                            <span style="font-weight: 600; color: #0f172a;"><%# Eval("SWATCH_NAME") %></span>
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSwatchName" runat="server" Text='<%# Bind("SWATCH_NAME") %>' CssClass="form-control"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- Category Column --%>
                                <asp:TemplateField>
                                    <HeaderStyle Width="25%" />
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">CATEGORY <span class="req text-danger">*</span></span>
                                            <asp:DropDownList ID="ddlHeaderSwatchType" runat="server" CssClass="form-control chosen-select"></asp:DropDownList>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# GetCategoryName(Eval("SWATCH_TYPE_ID")) %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditSwatchType" runat="server" CssClass="form-control chosen-select"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- Status Column --%>
                                <asp:TemplateField>
                                    <HeaderStyle Width="15%" />
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">STATUS</span>
                                            <asp:DropDownList ID="ddlHeaderStatus" runat="server" CssClass="form-control">
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
                                        <asp:DropDownList ID="ddlEditStatus" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <%-- Actions Column --%>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="col-actions text-center" />
                                    <ItemStyle CssClass="col-actions text-center" />
                                    <HeaderTemplate>
                                        <div class="header-field-group">
                                            <span class="header-label">&nbsp;</span>
                                            <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="+ Add" CssClass="btn btn-success btn-block" style="height: 34px;" />
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="display: flex; gap: 5px; justify-content: center;">
                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-sm btn-warning" ToolTip="Edit Swatch">
                                                <i class="fa fa-pencil"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-danger" ToolTip="Delete Swatch" OnClientClick="return confirm('Are you sure you want to delete this swatch?');">
                                                <i class="fa fa-trash"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="display: flex; gap: 5px; justify-content: center;">
                                            <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="btn btn-sm btn-success" ToolTip="Save Changes">
                                                <i class="fa fa-check"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-sm btn-default" ToolTip="Cancel">
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

    <!-- Select2 & Live Search Logic -->
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script type="text/javascript">
        
        function initializeSelect2() {
            $('.chosen-select').select2({
                width: '100%',
                placeholder: "-- Select Category --",
                allowClear: true
            });
        }

        function autoClearMessage() {
            setTimeout(function () {
                var lbl = document.getElementById('<%= lblMessage.ClientID %>');
                if (lbl && lbl.innerHTML.trim() !== '') {
                    $(lbl).fadeOut('fast', function() {
                        lbl.innerHTML = '';
                        $(lbl).show(); 
                    });
                }
            }, 3000); 
        }

        $(document).ready(function () {
            initializeSelect2();
            autoClearMessage(); 
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            initializeSelect2();
            autoClearMessage(); 
        });

        function filterSwatchTable() {
            var input = document.getElementById("clientSearchInput");
            var filter = input.value.toLowerCase();
            var grid = document.getElementById("<%= gvSwatches.ClientID %>");
            if (!grid) return;
            var rows = grid.getElementsByTagName("tr");
    
            for (var i = 1; i < rows.length; i++) {
                if (rows[i].classList.contains("gridview-pager")) continue;
        
                var nameCol = rows[i].getElementsByTagName("td")[2]; // Index 2 is Swatch Name
        
                if (nameCol) {
                    var nameValue = nameCol.textContent || nameCol.innerText;
                    if (nameValue.toLowerCase().indexOf(filter) > -1) {
                        rows[i].style.display = "";
                    } else {
                        rows[i].style.display = "none";
                    }
                }
            }
        }

        function showImagePreview(imageSrc) {
            if (imageSrc.indexOf('profile.png') > -1) {
                return;
            }
            document.getElementById('vanillaModalImg').src = imageSrc;
            document.getElementById('vanillaImageModal').style.display = 'block';
        }

        function closeImagePreview() {
            document.getElementById('vanillaImageModal').style.display = 'none';
        }

        window.onclick = function (event) {
            var modal = document.getElementById('vanillaImageModal');
            if (event.target == modal) {
                closeImagePreview();
            }
        }

    </script>

    <!-- Vanilla JS Image Preview Modal -->
    <div id="vanillaImageModal" style="display:none; position:fixed; z-index:9999; left:0; top:0; width:100%; height:100%; background-color:rgba(15, 23, 42, 0.85); backdrop-filter: blur(4px);">
        <span onclick="closeImagePreview()" style="position:absolute; top:20px; right:35px; color:#f8fafc; font-size:40px; font-weight:bold; cursor:pointer; transition: 0.2s;" onmouseover="this.style.color='#ef4444'" onmouseout="this.style.color='#f8fafc'">&times;</span>
        <img id="vanillaModalImg" style="margin:auto; display:block; max-width:90%; max-height:85vh; position:relative; top:50%; transform:translateY(-50%); border-radius: 8px; box-shadow: 0 10px 25px rgba(0,0,0,0.5);" />
    </div>
</asp:Content>