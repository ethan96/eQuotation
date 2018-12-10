<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="quoteByAccountOwner.aspx.vb" Inherits="EDOC.quoteByAccountOwner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Panel runat="server" ID="pldummy" DefaultButton="btnSH">
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label3" Text="Quote No"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtQuoteId"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="lbRoleName" Text="Quote Desc"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCustomId"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lbRoleValue" Text="<%$ Resources:myRs,AccountName %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAccountName"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label1" Text="<%$ Resources:myRs,CompanyID %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCompanyID"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label4" Text="<%$ Resources:myRs,SalesOrderNO %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="TextBox_SONO"></asp:TextBox>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label5" Text="<%$ Resources:myRs,PurchaseOrderNO %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="TextBox_PONO"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" style="color: Black">
                    <asp:DropDownList runat="server" ID="drpDatePeriod">
                        <asp:ListItem Text="Created Date" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Last Updated Date" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>From:&nbsp;<ajaxToolkit:CalendarExtender runat="server" ID="CalExt1" TargetControlID="txtQuoteCreateFrom"
                    Format="yyyy/MM/dd" />
                    <asp:TextBox runat="server" ID="txtQuoteCreateFrom" Width="80px" />
                </td>
                <td style="color: Black">To:
                </td>
                <td>
                    <ajaxToolkit:CalendarExtender runat="server" ID="CalExt2" TargetControlID="txtQuoteCreateTo"
                        Format="yyyy/MM/dd" />
                    <asp:TextBox runat="server" ID="txtQuoteCreateTo" Width="80px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label2" Text="<%$ Resources:myRs,Status %>"></asp:Label>
                    :
                </td>
                <td>
                    <asp:DropDownList ID="drpStatus" runat="server">
                        <asp:ListItem Value="">All</asp:ListItem>
                        <asp:ListItem Value="0">Draft</asp:ListItem>
                        <asp:ListItem Value="1">Finish</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label runat="server" ID="LbCreatedBy" Text="Created By:" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCreatedBy" />
                </td>
            </tr>
            <tr runat="server" id="OptyProjectNameRow" visible="false">
                <td>Opportunity Project Name：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtOPTYProjectName" />
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>
        <table>
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnSH" OnClick="btnSH_Click" Text="Search" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="<%$ connectionStrings: EQ %>"
        SelectCommand="" OnSelecting="SqlDataSource1_Selecting"></asp:SqlDataSource>
    Delete the draft of
    <asp:TextBox ID="txtMonth" runat="server" MaxLength="2"></asp:TextBox>
    month ago.&nbsp;&nbsp;<asp:Button ID="btnDeleteBatch" runat="server" Text="Delete" OnClick="btnDeleteBatch_Click" />
    &nbsp;&nbsp;&nbsp;<asp:Button ID="btnewquote" runat="server" Text="Create New Quotation" />
    <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Fn" TargetControlID="txtMonth"
        FilterType="Numbers, Custom" />
    <asp:GridView DataKeyNames="quoteId" ID="GridView1" DataSourceID="SqlDataSource1"
        AllowSorting="true" AllowPaging="true" PageIndex="0" PageSize="30" runat="server"
        AutoGenerateColumns="false" OnPageIndexChanging="GridView1_PageIndexChanging"
        Width="100%" OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting">
        <Columns>
            <%--            <asp:BoundField HeaderText="Quote ID" DataField="quoteId" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteId" />
            --%>
            <asp:TemplateField HeaderText="Quote No">
                <ItemStyle Wrap="False" />
                <ItemTemplate>
                    <%--<asp:Image ID="Image_Expand_Collapse" runat="server" alt="Expand/Collapse" ImageUrl="~/Images/expand.jpg" />--%>
                    <asp:Label runat="server" ID="lbquoteNo" Text='<%#Bind("quoteNo")%>' />
                    <asp:Label runat="server" ID="lbRevisionNumberTitle" Text="V" /><asp:Label runat="server"
                        ID="lbRevisionNumber" Text='<%#Bind("Revision_Number")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <%--            <asp:BoundField HeaderText="Quote No" DataField="quoteNo" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteNo" />
            --%>
            <asp:BoundField HeaderText="Quote Desc" DataField="customId" ItemStyle-HorizontalAlign="Left"
                SortExpression="customId" />
            <%--   <asp:BoundField HeaderText="ERP ID" DataField="quoteToErpId" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteToErpId" />--%>

            <asp:TemplateField>
                <HeaderTemplate>
                    ERP ID
                </HeaderTemplate>
                <ItemTemplate>
                    <%#Eval("quoteToErpId") %>
                    <input type="image" id="ibtn_CreateCust" visible='<%# isShowCreateCust(Eval("quoteToErpId")) %>' class="ibtn_CreateCust" runat="server" src="../Images/pickPN.png" value='<%#Eval("quoteId")%>' />

                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<%$ Resources:myRs,SiebelQuoteId %>">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lbSiebelQuoteId" Text=""></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Account Name" DataField="quoteToName" ItemStyle-HorizontalAlign="Left"
                SortExpression="quoteToName" />
            <asp:TemplateField HeaderText="<%$ Resources:myRs,GetErpIdFromSiebel %>">
                <ItemTemplate>
                    <asp:Button runat="server" ID="btnGetErpId" Text="Get" OnClick="btnGetErpId_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="GP Flow" ItemStyle-VerticalAlign="Middle">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lbGpFlow"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Quote Date" DataField="quoteDate" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Left" SortExpression="quoteDate" />
            <asp:BoundField HeaderText="Created Date" DataField="CreatedDate" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Left" SortExpression="CreatedDate" />
            <asp:BoundField HeaderText="Last Updated Date" DataField="LastUpdatedDate" DataFormatString="{0:d}"
                ItemStyle-HorizontalAlign="Left" SortExpression="LastUpdatedDate" />
            <asp:TemplateField HeaderText="Pipeline Status" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lbPipelineStatus" Text="" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbStatus" Text="Status"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" ID="lbStatusV" Text='<%#Bind("DOCSTATUS")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Created By" SortExpression="createdBy">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lbCreatedby" Text="" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbOrder_log" Text="Order Log"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <div align="center">

                        <table id="Last_Order_Log" runat="server">
                            <tr>
                                <td align="left">SO:
                                </td>
                                <td>
                                    <asp:Label ID="Label_SO_NO" runat="server" Text="" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="HyperLink_CheckAll" runat="server" NavigateUrl='<%#Bind("quoteId","QuotationToOrderLog.aspx?quoteid={0}")%>'
                                        Target="_blank">Display All</asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">PO:
                                </td>
                                <td>
                                    <asp:Label ID="Label_PO_NO" runat="server" Text="" />
                                </td>
                                <td align="left">Order Date:
                                    <asp:Label ID="Label_ORDER_DATE" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbHdEdit" Text="<%$ Resources:myRs,Order %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Button runat="server" ID="btnOrder" Text="Order" OnClick="btnOrder_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label runat="server" ID="lbHdEdit" Text="Actions"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <div style="width: 150px; display: block;">
                        <!--For not ANA User -->
                        <asp:ImageButton ID="ibtnPdf_ForNotANAUser" ImageUrl="~/Images/pdf_icon.jpg" runat="server" OnClick="ibtnPdf_Click" Visible="false" />
                        <!--For ANA User -->
                        <asp:ImageButton ID="ibtnPdf_ForANAUser" runat="server" ImageUrl="~/Images/pdf_icon.jpg"
                            OnClientClick='<%#"openUpdateformatwin("""+ Eval("QuoteId") + """,""" + Eval("QuoteNo") + """);return false;" %>' />
                        &nbsp;
                        <!--Quote to Word For ATW, ACN User -->
                        <asp:ImageButton ID="ibtnWord" ImageUrl="~/Images/word_icon.png" runat="server" OnClick="ibtnWord_Click" Visible="false" />
                        <!--Quote to Excel For AAC KA, CP User -->
                        <asp:ImageButton ID="ibtnExcel" ImageUrl="~/Images/excel.gif" runat="server" OnClick="ibtnExcel_Click" Visible="false" />
                        <!--Image for Pipeline -->
                        <input type="image" id="ibtn_Pipeline" class="ibtn_Pipeline" runat="server" src="../Images/pipeline.png" value='<%#Eval("quoteId")%>' visible="false" />
                        &nbsp;
                        <asp:ImageButton ID="ibtnSendMail" ImageUrl="~/Images/mail.png" runat="server" OnClientClick='<%# Eval("quoteId", "openwin(""{0}"");return false;") %>' />
                        &nbsp;
                            <asp:ImageButton ImageUrl="~/Images/copyimg.gif" runat="server" ID="ibtnCopy" ToolTip="Copy" OnClick="ibtnCopy_Click" data-key='<%#Eval("quoteId")%>' OnClientClick="return EUCopyPurpose(this);" />
                        &nbsp;
                        <asp:ImageButton ImageUrl="~/Images/edit.gif" runat="server" ID="ibtnEdit" OnClick="ibtnEdit_Click" ToolTip="Edit" />
                        &nbsp;
                        <asp:ImageButton ImageUrl="~/Images/del.gif" runat="server" ID="ibtnDelete" OnClick="ibtnDelete_Click" ToolTip="Delete"
                            OnClientClick="return confirm('Are you sure to delete this Quotation?')" />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-CssClass="hide">
                <ItemTemplate>
                    <ul class="myul">
                        <asp:GridView ID="GridView2" DataKeyNames="quoteId" ShowHeader="false" runat="server"
                            DataSource='<%# GetData(Eval("quoteNo")) %>' AutoGenerateColumns="false" OnRowDataBound="GridView2_RowDataBound">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="Label6" Text='<%#Bind("quoteNo")%>' />
                                        <asp:Label runat="server" ID="lbStatus" Text="V" /><asp:Label runat="server" ID="lbStatusV"
                                            Text='<%#Bind("Revision_Number")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Desc." DataField="customId" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="customId" ItemStyle-Width="120" />
                                <asp:BoundField HeaderText="ERP ID" DataField="quoteToErpId" ItemStyle-HorizontalAlign="Left"
                                    SortExpression="quoteToErpId" ItemStyle-Width="150" />
                                <asp:BoundField HeaderText="Quote Date" ItemStyle-Width="150" DataField="quoteDate" DataFormatString="{0:d}"
                                    ItemStyle-HorizontalAlign="Left" SortExpression="quoteDate" />
                                <asp:TemplateField HeaderText="GP Flow" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbGpFlow1"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pipeline Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbPipelineStatus" Text="" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbStatusV_2" Text='<%#Bind("DOCSTATUS")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Created By" SortExpression="createdBy">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbCreatedby1" Text="" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" ID="lbOrder_log" Text="Order Log"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div align="center">
                                            <table id="Last_Order_Log1" runat="server">
                                                <tr>
                                                    <td align="left">SO:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label_SO_NO1" runat="server" Text="" />
                                                    </td>
                                                    <td>
                                                        <asp:HyperLink ID="HyperLink_CheckAll1" runat="server" NavigateUrl='<%#Bind("quoteId","QuotationToOrderLog.aspx?quoteid={0}")%>'
                                                            Target="_blank">Display All</asp:HyperLink>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">PO:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label_PO_NO1" runat="server" Text="" />
                                                    </td>
                                                    <td align="left">Order Date:
                                                        <asp:Label ID="Label_ORDER_DATE1" runat="server" Text="" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnOrder1" Text="Order" OnClick="btnOrder1_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <!--For not ANA User -->
                                        <asp:ImageButton ID="ibtn_SubGrid_Pdf_ForNotANAUser" ImageUrl="~/Images/pdf_icon.jpg" runat="server" OnClick="ibtn_SubGrid_Pdf_ForNotANAUser_Click" Visible="false" />
                                        <!--For ANA User -->
                                        <!--Frank 2013/10/16 OnClientClick code has been generated by GridView2_RowDataBound event -->
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/pdf_icon.jpg" />
                                        <!--Quote to Word For ATW, ACN User -->
                                        <asp:ImageButton ID="ibtn_SubGrid_Word" ImageUrl="~/Images/word_icon.png" runat="server" OnClick="ibtn_SubGrid_Word_Click" Visible="false" />
                                        <!--Quote to Excel For AAC KA, CP User -->
                                        <asp:ImageButton ID="ibtn_SubGrid_Excel" ImageUrl="~/Images/excel.gif" runat="server" OnClick="ibtn_SubGrid_Excel_Click" Visible="false" />
                                        &nbsp;
                                        <!--Image for Pipeline -->
                                        <input type="image" id="ibtn_Pipeline" class="ibtn_Pipeline" runat="server" src="../Images/pipeline.png" value='<%#Eval("quoteId")%>' visible="false" />
                                        &nbsp;
                                  <asp:ImageButton ID="ImageButton2" ImageUrl="~/Images/mail.png" runat="server" OnClientClick='<%# Eval("quoteId", "openwin(""{0}"");return false;") %>' />
                                        &nbsp;
                                  <asp:ImageButton ImageUrl="~/Images/copyimg.gif" runat="server" ID="ImageButton3"
                                      ToolTip="Copy" OnClick="ibtnCopy1_Click" />
                                        &nbsp;
                                  <asp:ImageButton ImageUrl="~/Images/edit.gif" runat="server" ID="ibtnEdit1" OnClick="ibtnEdit1_Click"
                                      ToolTip="Edit" />
                                        &nbsp;
                                  <asp:ImageButton ImageUrl="~/Images/del.gif" runat="server" ID="ImageButton5" OnClick="ibtnDelete1_Click"
                                      ToolTip="Delete" OnClientClick="return confirm('Are you sure to delete this Quotation?')" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ul>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
    <asp:Panel ID="PLmessage" runat="server" Style="display: none" CssClass="modalPopup"
        Width="400" Height="300" ScrollBars="Auto">
        <div style="text-align: right;">
            <asp:ImageButton ID="CancelButtonAccount" runat="server" ImageUrl="~/Images/del.gif" />
        </div>
        <asp:UpdatePanel ID="UPForm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table>
                    <tr>
                        <td align="left" height="200" valign="top">
                            <asp:Literal ID="LabCopymessage" runat="server" Text="test"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="BTCopyNext" runat="server" Text="Confirm" OnClick="BTCopyNext_Click" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:LinkButton ID="lbDummyAccount" runat="server" />
    <div id="showpipelinepage" style="display: none" title="Pipeline">
        <iframe id="myIframe" src="" width="100%" height="100%" frameborder="0"></iframe>
    </div>

    <div id="showcreatenewcust" style="display: none" title="Create New Customer">
        <iframe id="myIframeCreateNewCust" src="" width="100%" height="100%" frameborder="0"></iframe>
    </div>
    <ajaxToolkit:ModalPopupExtender ID="MPCopy" runat="server" TargetControlID="lbDummyAccount"
        PopupControlID="PLmessage" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonAccount" />
    <div id="dialog_target"></div>

    <div style="display: none">
        <div id="divCopyPurpose" style="width: 100%; height: 90%;">
            <div style="height: 80%; width: 100%; text-align: center;">
                <h2 style="color: #1b1b69; text-align: center;">Please select your copy purpose.</h2>
                <asp:RadioButtonList ID="CopyPurpose" runat="server" Style="margin-left: 25%; width: 75%">
                    <asp:ListItem Value="1" Selected="True">Create New Quotation/Opty</asp:ListItem>
                    <asp:ListItem Value="2">Revise Quotation</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div style="height: 20%; text-align: center">
                <asp:Button runat="server" ID="btnCopyPurpose" Text="Submit" OnClick="btnCopyPurpose_Click" />
            </div>
            <asp:HiddenField runat="server" ID="hfCopyPurpose" />
        </div>
    </div>

    <link rel="Stylesheet" href="../Js/FancyBox/jquery.fancybox.css" type="text/css" />
    <script type="text/javascript" src="../Js/FancyBox/jquery.fancybox.js"></script>
    <script language="javascript" type="text/javascript">
        function openwin(QuoteID) {
            var url = '<%=_ServerPath%>/Quote/QuoteForward.aspx?QuoteID=' + QuoteID;
            var iTop = 50;
            var iLeft = (window.screen.width - 10 - 546) / 2;
            window.open(url, 'newwindow', 'height=550, width=600, top=' + iTop + ',left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no')
        }

        function openUpdateformatwin(QuoteID, QuoteNo) {
            var url = '<%=_ServerPath%>/URLREWRITER/UpdateQuotePrintFormat/' + QuoteID + '/' + QuoteNo + '.aspx';
            var iTop = (window.screen.height - 30 - 310) / 2;
            var iLeft = (window.screen.width - 10 - 546) / 2;
            window.open(url, 'newwindow', 'height=310, width=546, top=' + iTop + ',left=' + iLeft + ', toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, status=no')
        }
    </script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $(".myul").each(function () {
                var html = $.trim($(this).html());
                if (html.indexOf('table') > 0) {
                    //if (html != "") {
                    var wp = $(this).parent().parent().children().length
                    var str = String.format('<tr><td colspan="{0}">', wp);
                    str += ' <ul class="myul"><i class="pct"></i>  ';
                    str += html;
                    str += '</ul></td></tr>';
                    $(this).parent().parent().after(str);
                }
            })
        });
    </script>
    <style>
        .divgp {
            width: 200px;
        }

            .divgp span {
                cursor: pointer;
            }

        .ui-dialog .ui-dialog-content {
            margin: 10px;
        }

        .ui-dialog table.ui-dialog-content {
            border: 0px solid red;
        }

        #dialog_target td, #dialog_target th {
            border: 1px solid #000000;
        }

        .ui-dialog .ui-dialog-content th {
            color: black;
        }

        .DialogClass .ui-dialog-titlebar {
            color: #FFFFFF;
            background: #1f367a;
            font-weight: bold;
        }

        .DialogClass .ui-dialog .ui-dialog-title {
            float: left;
            margin: .1em 0;
            white-space: nowrap;
            width: 90%;
            overflow: hidden;
            text-overflow: ellipsis;
            font-size: 130%;
        }

        .DialogClass .ui-dialog-content th {
            color: #FFFFFF;
            text-align: center;
            vertical-align: central;
            font-weight: bold;
            font-size: 110%;
        }
    </style>
    <script>
        $(".divgp").delegate("span", "click", function (event) {
            var tbid = $(this).parent().find("table").attr("id");
            $("#dialog_target").html($("#" + tbid).prop("outerHTML"));
            $("#dialog_target").dialog(
                  {
                      width: '470',
                      modal: true,
                      open: function (type, data) {
                          $("#dialog_target").find("table").show();
                          //  $("#" + tbid).parent().appendTo("#dialog_target");
                      },
                      buttons: {
                          Ok: function () {
                              $(this).dialog("close");
                          }
                      },
                      close: function (type, data) { }
                  }
                 );
        });
    </script>

    <script type="text/javascript">

        $(function () {
            var wWidth = $(window).width();
            var dWidth = wWidth * 0.55;
            var wHeight = $(window).height();
            var dHeight = wHeight * 0.75;


            $('.ibtn_Pipeline').click(function () {
                var btn_value = $(this).attr("value");

                $("#showpipelinepage").dialog({
                    autoOpen: false,
                    show: "blind",
                    hide: "blind",
                    modal: true,
                    height: dHeight,
                    width: dWidth,
                    dialogClass: 'DialogClass',
                    open: function (ev, ui) {
                        $('#myIframe').attr('src', '<%=_ServerPath%>/Quote/PipeLine.aspx?QuoteID=' + btn_value);
                    },
                });
                $("#showpipelinepage").dialog('open');

                return false;
            });


            $('.ibtn_CreateCust').click(function () {
                var btn_value = $(this).attr("value");

                $("#showcreatenewcust").dialog({
                    autoOpen: false,
                    show: "blind",
                    hide: "blind",
                    modal: true,
                    height: dHeight,
                    width: dWidth,
                    dialogClass: 'DialogClass',
                    open: function (ev, ui) {
                        $('#myIframeCreateNewCust').attr('src', '<%=_ServerPath%>/Quote/CreateNewCust.aspx?QuoteID=' + btn_value);
                    },
                });
                $("#showcreatenewcust").dialog('open');

                return false;
            });
        });

            function EUCopyPurpose(node) {
                // Ryan 20161228 Add, EU User will not use ibtnCopy_Click anymore, will use btnCopyPurpose_Click

                <% If _IsEUUser Then%>
                var key = $(node).attr("data-key");
                $('#<%=hfCopyPurpose.ClientID%>').val(key);


                var gallery = [{
                    href: "#divCopyPurpose"
                }];

                $.fancybox(gallery, {
                    'autoSize': false,
                    'width': 350,
                    'height': 200,
                    'autoCenter': true
                });
                return false;
                <% Else%>
                return true;
                <% End If%>
            }
    </script>
</asp:Content>
