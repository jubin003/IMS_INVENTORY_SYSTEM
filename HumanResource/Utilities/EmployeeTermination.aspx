<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
    CodeFile="EmployeeTermination.aspx.cs" Inherits="human_resource_Utilities_EmployeeTermination" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style>
        .form {
            overflow: hidden;
            padding: 1em 1em 1em 1em;
            font-size: 14px;
            border: 1px solid black;
            box-sizing: border-box;
            min-width: 270px;
            margin-bottom: 10px;
        }

        .sub-container {
            background-color: #fff;
            border: 1px solid black;
            margin: auto;
            padding: 20px;
            box-sizing: border-box;
            margin-bottom: 30px;
            box-shadow: 0 0 5px black;
        }

            .sub-container:last-child {
                background-color: #fff;
                border: 1px solid black;
                margin: auto;
                padding: 5px;
                box-sizing: border-box;
                box-shadow: 0 0 5px black;
            }

        .sub-container-heading {
            height: 30px;
            border-bottom: 1px solid black;
            font-size: 18px;
            line-height: 30px;
            text-align: left;
            font-weight: bold;
            padding: 0 5px;
            box-sizing: border-box;
            margin-bottom: 10px;
        }

        .cancel-control {
            float: right;
            font-size: 50px;
            top: 5px;
        }

        .overlay {
            width: 100vw;
            height: 200vh;
            background-color: rgba(255,255,255, .9);
            z-index: 10000000;
            top: 0;
            position: absolute;
        }

        .popup-container {
            width: 300px;
            background-color: #fff;
            border: 1px solid black;
            margin: auto;
            top: 50%;
            left: calc(50% - 150px);
            transform: translateY(-50%);
            padding: 5px;
            box-sizing: border-box;
            position: fixed;
            z-index: 10000001;
            box-shadow: 0 0 5px black;
            display: block;
            clear: both;
            transition: top 300ms ease-in-out;
            transition: transform 300ms ease-in-out;
        }


        .hidden {
            transform: translateY(0%);
            top: -100%;
        }

        .popup-container-heading {
            height: 30px;
            border-bottom: 1px solid black;
            padding: 0 5px;
            box-sizing: border-box;
            margin-bottom: 10px;
        }

            .popup-container-heading > .popup-title {
                font-size: 18px;
                line-height: 30px;
                text-align: left;
                font-weight: bold;
                float: left;
            }

            .popup-container-heading > .cancel-control {
                font-size: 15px;
                top: 3px;
                right: 3px;
                float: right;
                font-family: Arial;
                color: black;
                cursor: pointer;
            }

                .popup-container-heading > .cancel-control:hover {
                    color: red;
                }


        .row {
            overflow: hidden;
            box-sizing: border-box;
        }

        .label {
            text-align: left;
            position: relative;
            display: block;
            /*background-color: tomato;*/
            padding: .25em;
            float: left;
            box-sizing: border-box;
            margin-right: 1em;
            margin-bottom: 1em;
            width: 100%;
            min-width: 200px;
            /*max-width:calc(100% / 2);*/
            font-weight: bold;
            box-sizing: border-box;
            color: black;
        }

            .label > .input {
                display: block;
                padding: .25em;
                width: 100%;
                margin-top: .25em;
                font-family: inherit;
                font-size: inherit;
            }

            .label > input {
                display: block;
                padding: .25em;
                width: 100%;
                margin-top: .25em;
                font-family: inherit;
                font-size: inherit;
            }



            .label > div {
                margin-top: .25em;
                overflow: hidden;
                font-family: inherit;
                font-size: inherit;
                background-color: rgba(255, 255, 255, .2);
            }

                .label > div > div {
                    float: left;
                    margin-right: .75em;
                }

                    .label > div > div:last-child {
                        float: left;
                        margin-right: 0;
                    }

        .form > .row > .col1 {
            width: calc((100%  / 1) - 1em);
        }

        .form > .row > .col2 {
            width: calc((100%  / 2) - 1em);
        }

        .form > .row > .col3 {
            width: calc((100%  / 3) - 1em);
        }

        .form > .sub-container > .row > .col4 {
            width: calc((100%  / 4) - 1em);
            top: 0px;
            left: 0px;
        }

        .form > .popup-container > .row > .col4 {
            width: calc((100%  / 4) - 1em);
            top: 0px;
            left: 0px;
        }

        .form > .sub-container > .row > .col5 {
            width: calc((100%  / 8));
            top: 0px;
            left: 0px;
        }

        .form > .row > .col6 {
            width: calc((100%  / 6) - 1em);
        }

        .form > .row > .col7 {
            width: calc((100%  / 7) - 1em);
        }

        .form > .row > .col8 {
            width: calc((100%  / 8) - 1em);
        }

        .form > .row > .col9 {
            width: calc((100%  / 9) - 1em);
        }

        .form > .row > .col10 {
            width: calc((100%  / 10) - 1em);
        }

        .form > .row > .col11 {
            width: calc((100%  / 11) - 1em);
        }

        .form > .row > .col12 {
            width: calc((100%  / 12) - 1em);
        }

        .show-popup {
            top: -500px;
        }
    </style>
    <script type="text/javascript">
        window.onload = function () {
            // Initialize Nepali Date Picker on multiple elements
            var elements = ["<%= txtDate.ClientID %>"];
            elements.forEach(function (id) {
                var element = document.getElementById(id);
                if (element) {
                    element.nepaliDatePicker();
                }
            });
        };
    </script>

    <div class="container-fluid">
        <table>
            <tr>
                <td>
                    <table id="EmpSearch" width="100%" runat="server">
                        <tr>
                            <td>
                                <table style="width: 40%">
                                    <tr>
                                        <td>Department
                            <asp:DropDownList ID="ddlDept" AutoPostBack="true" Width="200px" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged">
                            </asp:DropDownList>
                                        </td>
                                        <td>Division:<br />
                                            <asp:DropDownList AutoPostBack="true" CssClass="uc_formcontrol" runat="server" Width="200px" ID="ddlDivision" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td>Unit:<br />
                                            <asp:DropDownList AutoPostBack="true" CssClass="uc_formcontrol" runat="server" Width="200px" ID="ddlUnit"></asp:DropDownList>
                                        </td>
                                        <td>Status:            
                            <asp:DropDownList ID="ddlStatus" Width="100px" runat="server" CssClass="form-control">
                                <asp:ListItem Selected="True" Value="1">Active</asp:ListItem>
                                <asp:ListItem Value="0">InActive</asp:ListItem>
                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <br />
                                            <asp:Button ID="bttnSearch" Width="80px" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="bttnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblEmpIdTemp" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:GridView ID="GridEmpList" CssClass="gridtable" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" EnableModelValidation="True"
                                    OnRowDataBound="GridEmpList_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="EMP ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpID" runat="server" Text='<%# Bind("EMPLOYEEID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Width="170px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PHOTO">
                                            <ItemTemplate>
                                                <asp:Image ID="imgEmpPhoto" Width="50px" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DEPARTMENT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptId" runat="server" Text='<%# Bind("DEPARTMENTID") %>' Visible="False"></asp:Label>
                                                <asp:Label ID="lblDivId" runat="server" Text='<%# Bind("DIVISION") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblUnitId" runat="server" Text='<%# Bind("UNIT") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblDept" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="DIVISION">
                            <ItemTemplate>
                                <asp:Label ID="lblDivId" runat="server" Text='<%# Bind("DIVISION") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblDiv" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="EDIT">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgBttnEdit" runat="server" ImageUrl="~/images/icons/edit.png"
                                                    OnClick="ImgBttnEdit_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <div class="form" id="divEmpDetails" runat="server" visible="false">
            <div id="leave_form" class="sub-container" runat="server">
                <div class="row">
                    <div id="divDetail" runat="server" class="col-md-4">
                        <div class="row">
                            <table>
                                <tr>
                                    <td rowspan="6"><a id="imgLink" runat="server" target="_blank">
                                        <asp:Image Width="35mm" Height="45mm" ID="imgEmp" runat="server" /></a>
                                    </td>

                                    <td><b>Name:</b>
                                        <asp:Label ID="lblEmpID" Width="150px" CssClass="form-control" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>Department:</b>

                                        <asp:Label ID="lblDepartment" runat="server" Text="">    </asp:Label>

                                    </td>
                                </tr>
                                <tr runat="server" id="trDivision">
                                    <td>
                                        <b>Division</b>
                                        <asp:Label ID="lblDivision" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr runat="server" id="trUnit">
                                    <td>
                                        <b>Unit</b>
                                        <asp:Label ID="lblUnit" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>Designation:</b>

                                        <asp:Label ID="lblDesignation" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Status:</b>
                                        <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="lblPKID" runat="server" Visible="false" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>Termination Type:</b>

                                        <asp:DropDownList ID="ddlType" Width="150px" CssClass="form-control" runat="server"></asp:DropDownList>

                                    </td>
                                    <td><b>Termination Date:     </b>

                                        <asp:TextBox ID="txtDate" Width="150px" AutoPostBack="true" CssClass="form-control" Style="font-size: small" AutoComplete="off" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row" runat="server">
                                        <table width="100%">
                                        </table>


                                    </div>

                                    <div class="row mt-5" runat="server">
                                    </div>

                                    <div class="row mt-5">
                                        <div class="col-md-2" style="text-align: left">
                                            <b>Description :</b>
                                        </div>
                                        <div class="col-md-10">
                                            <asp:TextBox ID="txtDescription" CssClass="form-control" Width="100%" TextMode="MultiLine" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row mt-5" style="padding-bottom: 5px">
                                        <div class="col-md-12">
                                            <label class="label col4">
                                                Resignation File:    
                                <asp:FileUpload ID="fileUpload1" runat="server" Height="22px" />

                                            </label>
                                        </div>
                                    </div>
                                    <div class="row mt-5">
                                        <div class="col-md-4">
                                            <asp:Button ID="btnSubmit" Width="80PX" CssClass="btn btn-primary" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                            <asp:Button ID="btnBack" runat="server" Text="Back" Width="100px" OnClick="btnBack_Click" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        window.onload = function () {
            // Initialize all datepickers with their respective formats
            document.getElementById("<%= txtDate.ClientID %>").NepaliDatePicker({ dateFormat: "DD/MM/YYYY", miniEnglishDates: true });
        };
    </script>
</asp:Content>

