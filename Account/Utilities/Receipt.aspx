<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Receipt.aspx.cs" Inherits="Account_Utilities_Receipt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function printPartOfPage() {
            var printContent = document.getElementById('divVoucher');
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=0,top=0,width=0,height=0');

            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            //printWindow.close();
        }
    </script>
    <script type="text/javascript">
        window.onload = function () {
            // Initialize Nepali Date Picker on multiple elements
            var elements = ["<%= txtFromDate.ClientID %>", "<%= txtToDate.ClientID %>"];
            elements.forEach(function (id) {
                var element = document.getElementById(id);
                if (element) {
                    element.nepaliDatePicker();
                }
            });
        };
    </script>
    <div class="container-fluid">
        <table style="width: 1000px" class="gridtable">            
            <tr>
                <td>Fiscal Year
                </td>
                <td>
                    <asp:DropDownList ID="ddlFiscalYear" runat="server" CssClass="form-control"></asp:DropDownList>
                </td>
                <td>From Date               
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>

                </td>
                <td>To Date               
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                </td>
                <td>Status                
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnList" runat="server" Text="List" OnClick="btnList_Click" CssClass="btn btn-primary" />
                </td>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CssClass="btn btn-primary" />
                    <asp:Label ID="lblPK_id" runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <asp:GridView ID="grdVoucherList" runat="server" Width="100%" AutoGenerateColumns="False"
                        CssClass="gridtable" OnRowDataBound="grdVoucherList_RowDataBound" OnRowCommand="grdVoucherList_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Sno">
                                <ItemTemplate>
                                    <asp:Label ID="lblSN" runat="server" Text="<%# Container.DataItemIndex+1 %>"></asp:Label>
                                    <asp:Label Text='<%# Bind("PK_ID") %>' ID="lblPkid" runat="server" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Voucher No">
                                <ItemTemplate>
                                    <asp:Label ID="lblVoucheNo" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prepared By">
                                <ItemTemplate>
                                    <asp:Label ID="lblPreparedBy" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Checked By">
                                <ItemTemplate>
                                    <asp:Label ID="lblCheckedBy" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approved By">
                                <ItemTemplate>
                                    <asp:Label ID="lblApprovedBy" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="View">
                                <ItemTemplate>
                                    <asp:Button ID="btnView" CommandName="View" runat="server" CssClass="btn btn-primary" Text="View" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                </td>
            </tr>
        </table>
        <div id="divHide" runat="server" visible="false">
            <asp:ImageButton ID="btnPrint" runat="server" Width="50px" ImageUrl="~/images/icons/print.png" OnClick="btnPrint_Click" />

            <br />
            <br />
            <table  class="gridtable">
                <tr>
                    <td>
                        <div id="divVoucher">
                            <table style="border: solid 1px; width: 200mm;">
                                <tr>
                                    <td>
                                        <table style="width: 200mm;" border="0">
                                            <tr>
                                                <td colspan="3" style="text-align: center;">
                                                    <strong>
                                                        <asp:Label ID="lblCompanyName" runat="server" Style="font-size: 20px"></asp:Label>
                                                    </strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="text-align: center;">
                                                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="text-align: center;">
                                                    <asp:Label ID="lblContact" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="text-align: center;">
                                                    <b>
                                                        <asp:Label ID="lblVoucherType" runat="server" Style="font-size: 20px"></asp:Label></b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left;"><strong>Voucher No:</strong>
                                                    <asp:Label ID="lblVoucherNo" runat="server"></asp:Label>
                                                </td>
                                                <td></td>
                                                <td style="text-align: right;"><strong>Date:               
                                                </strong>
                                                    <asp:Label ID="lblDate" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:GridView ID="grdVoucherChild" runat="server" AutoGenerateColumns="False" CssClass="normalTable"
                                                        ShowFooter="True" OnRowDataBound="grdVoucherChild_RowDataBound" Width="100%">
                                                        <Columns>

                                                            <asp:TemplateField HeaderText="Particulars">
                                                                <FooterTemplate>
                                                                    <strong>Total</strong>
                                                                </FooterTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label Text='<%# Bind("PARTICULARS") %>' ID="lblParticulars" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dr. Amount">
                                                                <FooterTemplate>
                                                                    <asp:Label Font-Bold="true" ID="lblDrTotal" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label Text='<%# Bind("DR_AMOUNT") %>' ID="lblDrAmount" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="30mm" HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cr. Amount">
                                                                <FooterTemplate>
                                                                    <asp:Label Font-Bold="true" ID="lblCrTotal" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label Text='<%# Bind("CR_AMOUNT") %>' ID="lblCrAmount" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="30mm" HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <strong>Amount in Words: </strong>
                                                    <asp:Label ID="lblAmountsInword" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="divReference" visible="false">
                                                <td colspan="3">
                                                    <hr />
                                                    <strong>Receipt Mode: </strong>
                                                    <asp:Label ID="lblDebitRefMode" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;
                                                     <strong>Reference No: </strong>
                                                    <asp:Label ID="lblDebitRefNo" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;
                                                   <strong>Reference Date: </strong>
                                                    <asp:Label ID="lblDebitRefDate" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <hr />
                                                    <strong>Narration: </strong>
                                                    <asp:Label ID="lblNarration" runat="server" Text=""></asp:Label>
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center; width: 650mm;">Prepared By
                                        <br />
                                                    <asp:Label ID="lblPreparedBy" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="text-align: center; width: 650mm;">
                                                    <div id="divCheckedBy" runat="server">
                                                        Checked By
                                         <br />
                                                        <asp:Label ID="lblCheckedBy" runat="server" Text=""></asp:Label>
                                                    </div>
                                                </td>
                                                <td style="text-align: center; width: 650mm;">Approved By
                                         <br />
                                                    <asp:Label ID="lblApprovedBy" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" visible="false" id="divAlternationNote">
                                                <td colspan="3">
                                                    <strong>
                                                        <asp:Label ID="LabelAlterationNote" runat="server" Text=""></asp:Label></strong>
                                                    <asp:Label ID="lblAlterationNote" runat="server" Text=""></asp:Label><br />
                                                    <strong>
                                                        <asp:Label ID="LabelAlterationNoteRequest" runat="server" Text=""></asp:Label></strong>
                                                    <asp:Label ID="lblAlterationNoteRequest" runat="server" Text="Label"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td style="vertical-align: top">
                        <table runat="server" style="width: 90mm" id="divCorrectVoucher" visible="false">
                            <tr>
                                <td>
                                    <strong>Voucher Correction Note</strong>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCorrectionNote" runat="server" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnCorrectVoucher" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnCorrectVoucher_Click" />
                                </td>
                            </tr>
                        </table>
                        <table runat="server" style="width: 90mm" id="divVoucherCancel" visible="false">
                            <tr>
                                <td>
                                    <strong>Voucher Cancel Note</strong>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtVoucherCancelNote" runat="server" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnVoucherCancel" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnVoucherCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <table id="divBtns" runat="server" visible="false">
            <tr>
                <td>
                    <asp:Button ID="btnCheck" runat="server" Text="Check Voucher" CssClass="btn btn-primary" Visible="false" OnClick="btnCheck_Click" />
                </td>
                <td>
                    <asp:Button ID="btnApprove" runat="server" Text="Approve Voucher" CssClass="btn btn-primary" Visible="false" OnClick="btnApprove_Click" />
                </td>
                <td> 
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel Voucher" CssClass="btn btn-primary" Visible="false" OnClick="btnCancel_Click" />
                </td>
                <td>
                    <asp:Button ID="btnCorrect" runat="server" Text="Correct Voucher" CssClass="btn btn-primary" Visible="false" OnClick="btnCorrect_Click" />
                </td>
                <td>
                    <asp:Button ID="btnAlter" runat="server" Text="Alter" Visible="false" CssClass="btn btn-primary" OnClick="btnAlter_Click" /></td>
                <td>
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnClose_Click" />
                </td>
            </tr>
        </table>

        <div runat="server" id="divVoucherEntry" visible="false">
            <table style="border: 1px solid; width: 1000px" runat="server" id="divVoucheDate" visible="false" class="gridtable">
                <tr>
                    <td>Voucher Date</td>
                    <td>
                        <asp:TextBox ID="txtVoucherDate" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:Label ID="lblVoucherPK_ID" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>Payment Mode</td>
                    <td>
                        <asp:DropDownList ID="ddlPaymentMode" runat="server" Width="150px" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentMode_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td colspan="3" style="width: 230px">
                        <table style="width: 100%" runat="server" id="divEntryCash" visible="false" class="gridtable">
                            <tr>
                                <td style="width: 100px">Amount</td>
                                <td>
                                    <asp:TextBox ID="txtCashAmount" runat="server" CssClass="form-control" Style="text-align: right;"
                                        AutoPostBack="true" Width="150px" OnTextChanged="txtCashAmount_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>

                </tr>
            </table>

            <table style="border: 1px solid; width: 1000px" runat="server" id="divEntryBankDetail" visible="false" class="gridtable">
                <tr>
                    <td>Bank </td>
                    <td>
                        <asp:DropDownList ID="ddlBankName" runat="server" AutoPostBack="true" CssClass="form-control"
                            OnSelectedIndexChanged="ddlBankName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>Balance</td>
                    <td><strong>Rs.  
                    <asp:Label ID="lblBalance" runat="server" Text=""></asp:Label></strong></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Ref No:</td>
                    <td>
                        <asp:TextBox ID="txtRefNo" runat="server" CssClass="form-control"></asp:TextBox></td>

                    <td>Ref Date</td>
                    <td>
                        <asp:TextBox ID="txtRefDate" runat="server" CssClass="form-control"></asp:TextBox></td>
                    <td>Amount</td>
                    <td>
                        <asp:TextBox ID="txtRefAmount" runat="server" CssClass="form-control" Style="text-align: right;"></asp:TextBox></td>
                    <td>
                        <asp:Button ID="btnAddBank" runat="server" Text="Add" OnClick="btnAddBank_Click" CssClass="btn btn-primary" /></td>
                </tr>
            </table>

            <table style="border: 1px solid; width: 1000px; padding: 5px" runat="server" id="divShowBankDetail" visible="false" class="gridtable">
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblBank" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblSGL_CODE" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                    <td>RefNo</td>
                    <td>
                        <asp:Label ID="lblRefNo" runat="server" Text=""></asp:Label>
                    </td>
                    <td>Ref Date</td>
                    <td>
                        <asp:Label ID="lblRefDate" runat="server" Text=""></asp:Label>
                    </td>
                    <td>Amount</td>
                    <td>
                        <strong>
                            <asp:Label ID="lblTotalCrAmount" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblTotalDrAmount" runat="server" Text="" Visible="false"></asp:Label>
                        </strong>
                    </td>
                    <td>
                        <asp:Button ID="btnEditBank" runat="server" Text="Edit" OnClick="btnEditBank_Click" CssClass="btn btn-primary" />
                    </td>
                </tr>
            </table>
            <table style="border: 1px solid; width: 1000px; padding: 5px" runat="server" id="divDrEntry" visible="false" class="gridtable">
                <tr>                   
                    <td style="width:80px">
                        <asp:TextBox ID="txtGLCode" runat="server" OnTextChanged="txtGLCode_TextChanged" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td  style="width:350px">
                        <asp:DropDownList ID="ddlGLAccount" runat="server" CssClass="form-control" Width="350px" OnSelectedIndexChanged="ddlGLAccount_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        <script>
                            $('#<%=ddlGLAccount.ClientID%>').chosen();
                        </script>
                    </td>
                    <td style="width:350px">
                        <asp:DropDownList ID="ddlSGLAccount" runat="server"  Visible="false" CssClass="form-control"  Width="350px" ></asp:DropDownList>
                        <script>
                            $('#<%=ddlSGLAccount.ClientID%>').chosen();
                        </script>
                    </td>
                    <td style="width:100px">
                        <asp:TextBox ID="txtAmount" runat="server"  CssClass="form-control"></asp:TextBox></td>
                    <td style="width:80px">
                        <asp:Button ID="btnAddDr" runat="server" Text="Add" CssClass="btn btn-primary" Width="80px" OnClick="btnAddDr_Click"/></td>
                </tr>
            </table>
            <table style="border: 1px solid; width: 1000px" runat="server" id="divDebittPart" visible="false">
                <tr>
                    <td>
                        <asp:GridView ID="grdDebit" runat="server" AutoGenerateColumns="False" ShowHeader="false" Width="100%"
                            CssClass="gridtable" OnRowCommand="grdDebit_RowCommand" ShowFooter="true" OnRowDataBound="grdDebit_RowDataBound">
                            <Columns>                              
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblGLCode" runat="server" Text='<%# Bind("GL_CODE") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="80px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblGLName" runat="server" Text='<%# Bind("GL_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="350px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSGLCode" runat="server" Text='<%# Bind("SGL_CODE") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblSGLName" runat="server" Text='<%# Bind("SGL_NAME") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="lableTotal" runat="server" Text="Total"></asp:Label></b>
                                    </FooterTemplate>
                                    <ItemStyle Width="350px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("AMOUNT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <b>
                                            <asp:Label ID="lblDrTotal" runat="server"></asp:Label></b>
                                    </FooterTemplate>
                                    <ItemStyle Width="100px" HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-primary btn-sm" CommandName="Remove" Width="80px" />
                                    </ItemTemplate>

                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>Narration:<br />
                        <asp:TextBox ID="txtNarration" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
            <td>
                <asp:FileUpload ID="voucherUpload" runat="server" />
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </td>

        </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Button ID="btnSaveVoucher" runat="server" Text="Save" Width="100px" CssClass="btn btn-primary" OnClick="btnSaveVoucher_Click" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Width="100px"  CssClass="btn btn-primary" Visible="false" OnClick="btnUpdate_Click" />
                        <asp:Button ID="btnCancelVoucher" runat="server" Text="Cancel" Width="100px" CssClass="btn btn-primary"  OnClick="btnCancelVoucher_Click" /></td>
                </tr>
            </table>
        </div>
    </div>
     <table runat="server" id="divVoucherImage" visible="false">

        <tr>
            <td>
                <div style="margin-left: 20px; width: 210mm; height: 120mm; position: relative;">
                    <asp:Label ID="lblPk_idPDF" runat="server" Visible="false"></asp:Label>
                    <asp:Image ID="voucherImage" runat="server"
                        Style="width: 110mm; height: 100%; object-fit: cover; position: absolute;" />
                    <asp:ImageButton Height="30px" ImageUrl="~/images/icons/pdf.png"
                        ID="btnPdf" runat="server" Visible="false" OnClick="btnPdf_Click" />
                </div>


            </td>
        </tr>
    </table>
</asp:Content>

