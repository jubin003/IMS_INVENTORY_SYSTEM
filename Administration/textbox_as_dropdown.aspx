<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="textbox_as_dropdown.aspx.cs" Inherits="Administration_textbox_as_dropdown" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#showOptions').change(function () {
                var showOptions = $(this).is(':checked');
                if (showOptions) {
                    $('#dropdownOptions').show();
                } else {
                    $('#dropdownOptions').hide();
                }
            });

            $('.dropdown').click(function () {
                if ($('#showOptions').is(':checked')) {
                    $('#dropdownOptions').toggle();
                }
            });

            $('#dropdownOptions ul li').click(function () {
                var selectedOption = $(this).text();
                $('.dropdown').val(selectedOption);
                $('#dropdownOptions').hide();
            });
        });

        function validateForm() {
            var errorMessages = [];

            var textBoxValue = document.getElementById("<%= TextBox1.ClientID %>").value;
            var dropdownValue = document.getElementById("<%= DropDownList1.ClientID %>").value;
            var checkboxChecked = document.getElementById("<%= CheckBox1.ClientID %>").checked;

            if (textBoxValue.trim() === "") {
                errorMessages.push("Textbox is empty. Please enter a value.");
            }

            if (dropdownValue === "Select") {
                errorMessages.push("Dropdown selection is required. Please select an option.");
            }

            if (!checkboxChecked) {
                errorMessages.push("Checkbox is not checked. Please check the box.");
            }

            // Additional validation for other fields

            if (errorMessages.length > 0) {
                alert(errorMessages.join("\n"));
                return false; // Prevent form submission
            }

            return true; // Allow form submission
        }
    </script>



    <div class="row">
        <div class="col-md-2">
            Text Box with Dropdown option
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="TextBox1" runat="server" CssClass="dropdown form-control" TextMode="MultiLine"></asp:TextBox>
            <div id="dropdownOptions" class="dropdown-options" style="display: none;">
                <ul style="list-style-type: none; padding: 0; margin: 0;">
                    <li>Option 1</li>
                    <li>Option 2</li>
                    <li>Option 3</li>
                </ul>
            </div>
            <asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem>Select</asp:ListItem>
                <asp:ListItem>a1</asp:ListItem>
                <asp:ListItem>a2</asp:ListItem>
            </asp:DropDownList>
            <asp:CheckBox ID="CheckBox1" runat="server" />
        </div>
        <div class="col-md-2">
            <input type="checkbox" id="showOptions" name="optionVisibility" checked="checked" />
            <label for="showOptions">Show options</label>
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return validateForm();" OnClick="btnSave_Click" />
            <br />
            <br />
            <br />
            <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1">
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
        </div>
    </div>



</asp:Content>

