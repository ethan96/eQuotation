<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="SiebelQuote.aspx.vb" Inherits="EDOC.SiebelQuote" %>

<%@ Register Src="~/ascx/PickSiebelOpportunity.ascx" TagName="Pickopty1" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        th
        {
            text-align: center;
            color: #000000;
        }
        .gtd TH
        {
            color: #FFFFFF;
        }
        .gtd .HeaderStyle
        {
            line-height: 20px;
        }
        .displaynone
        {
            display: none;
        }
        .gtd p
        {
            margin: 0px;
            text-align: left;
            line-height: 15px;
            padding: 0px;
        }
        table.tb, table.tb td, table.tb th
        {
            border: 1px solid #D8D8D8;
            padding: 0 20px;
            line-height: 35px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function ShowQuoteDiv() {
            var divEC = document.getElementById('div_AllQuote');
            var divECData = document.getElementById('div_AllQuoteData');
            var qQName = document.getElementById('txtQName').value;
            var qAccName = document.getElementById('txtQAcc').value;
            divEC.style.display = 'block';
            divECData.innerHTML = "<center><img src='/Images/loader.gif' alt='Loading...' />Loading SIEBEL Quotation...</center> ";
            PageMethods.GetSIEBELQuotes(qQName, qAccName,
                function (pagedResult, eleid, methodName) {
                    divECData.innerHTML = pagedResult;
                },
                function (error, userContext, methodName) {
                    alert(error.get_message());
                    divECData.innerHTML = "";
                });
        }
        function DivOffOrOn(id) {
            var div = document.getElementById(id);
            if (div.style.display == 'block') { div.style.display = 'none'; } else { div.style.display = 'block'; }
        }
        function checkdate(id, Maximum) {
            var id = document.getElementById("ctl00__main_" + id);
            if (id.value.length > Maximum) {
                alert('More than ' + Maximum + ' characters')
                id.focus()
                return false
            }
            else {
                return true
            }
        }
        function setDueDate() {
            var ii = "00";
            var obj1 = document.getElementById('<%=Me.txtDueDate1.ClientID%>')
            if (obj1.value == '') { return; }
            for (i = 2; i <= 99; i++) {
                if (i < 10) { ii = "0" + i; }
                else { ii = i; }
                var obj = document.getElementById("ctl00__main_gvItems_ctl" + ii + "_txtDueDate");
                if (obj == null) { return; }
                obj.value = obj1.value;
            }
        }
        function setQty() {
            var ii = "00";
            var obj1 = document.getElementById('<%=Me.txtQty1.ClientID%>')
            if (obj1.value == '') { return; }
            for (i = 2; i <= 99; i++) {
                if (i < 10) { ii = "0" + i; }
                else { ii = i; }
                var obj = document.getElementById("ctl00__main_gvItems_ctl" + ii + "_txtQty");
                if (obj == null) { return; }
                obj.value = obj1.value;
            }
        }
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 1100000;
        }
    </script>
    <asp:HiddenField runat="server" ID="hd_Qid" />
    <%--<asp:GridView ID="GridView1" runat="server"></asp:GridView>--%>
    <table width="100%">
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lbMsg" Font-Bold="true" ForeColor="Tomato" />
                        </td>
                    </tr>
                    <tr>
                        <th align='left' style="color: #658AC3; height: 25px; line-height: 25px; background-color: #D8D8D8;">
                            <b style="color: #000000;">Quotation: </b>
                            <asp:Label runat="server" ID="lbPickedQuoteName" Font-Bold="true" ForeColor="Navy" />&nbsp;
                            <input type="image" src="../images/search.gif" style="border-width: 0px;" onclick="ShowQuoteDiv(); return false;"
                                width="18" height="18" />
                            <div id="div_AllQuote" style="display: none; position: absolute; background-color: white;
                                border: solid 1px silver; padding: 10px; margin-left: 20%; width: 700px; height: 200px;
                                overflow: auto;">
                                <table width="100%">
                                    <tr valign="top">
                                        <td>
                                            Quotation Name:
                                        </td>
                                        <td>
                                            <input type="text" id="txtQName" />
                                        </td>
                                        <td>
                                            Account Name:
                                        </td>
                                        <td>
                                            <input type="text" id="txtQAcc" />
                                        </td>
                                        <td>
                                            <input type="button" id="btnQueryQuote" value="Query" onclick='ShowQuoteDiv(); return false;' />
                                        </td>
                                        <td align="right">
                                            <a href="javascript:void(0)" onclick="javascript:document.getElementById('div_AllQuote').style.display='none';">
                                                Close</a>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td colspan="6" id='div_AllQuoteData'>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" style="border-color: Gray" class="tb">
                                <tr>
                                    <th style="text-align:right" width="15%">
                                        Account's ERPID:
                                    </th>
                                    <td width="35%">
                                        <asp:Label runat="server" ID="lbERPID" />
                                    </td>
                                    <th style="text-align:right" width="15%">
                                        Sales Person:
                                    </th>
                                    <td width="35%">
                                        <asp:Label runat="server" ID="lbSalesPersonLstName" /><asp:Label runat="server" ID="lbSalesPersonFstName" />
                                    </td>
                                </tr>
                                <tr>
                                    <th style="text-align:right">
                                        Account:
                                    </th>
                                    <td>
                                        <asp:Label runat="server" ID="lbAccount" />
                                    </td>
                                    <th style="text-align:right">
                                        Opp. ID:
                                    </th>
                                    <td>
                                        <asp:Label ID="lbOptyid" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="text-align:right">
                                        Address:
                                    </th>
                                    <td>
                                        <asp:Label ID="lbAdr" runat="server" Text=""></asp:Label>
                                    </td>
                                    <th style="text-align:right">
                                        Project Name:
                                    </th>
                                    <td>
                                        <asp:Label runat="server" ID="lbOptyName" />
                                    </td>
                                </tr>
                                <tr>
                                    <th style="text-align:right">
                                        Attention:
                                    </th>
                                    <td>
                                        <asp:Label runat="server" ID="lbLstName" /><asp:Label runat="server" ID="lbFstName" />
                                    </td>
                                    <th style="text-align:right">
                                        Currency:
                                    </th>
                                    <td>
                                        <asp:Label runat="server" ID="lbCurr" />
                                    </td>
                                </tr>
                                <tr>
                                    <th style="text-align:right">
                                        Tel:
                                    </th>
                                    <td>
                                        <asp:Label ID="lbtel" runat="server" Text="" />
                                    </td>
                                    <th style="text-align:right">
                                        Total Revenue
                                    </th>
                                    <td>
                                        <asp:TextBox runat="server"  ID="lbTotal"> </asp:TextBox>
                                         <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender1" TargetControlID="lbTotal"
                                FilterType="Numbers, Custom" ValidChars="." />
                                        <asp:Label runat="server" ID="lbQuoteName" Visible="false" />
                                        <asp:Label runat="server" ID="lbQuoteNum" Visible="false" />
                                        <asp:Label ID="Labaccountrowid" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label runat="server" ID="lbQuoteStatus" Visible="false" />
                                        <asp:Label runat="server" ID="lbDue" Visible="false" /><asp:Label runat="server"
                                            ID="lbSalesRep" Visible="false" />
                                        <asp:Label runat="server" ID="lbEffDate" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <th align='left' style="color: #658AC3; text-align: left;">
                            Line Items
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td align="right">
                            <ajaxToolkit:CalendarExtender TargetControlID="txtDueDate1" CssClass="MyCalendar"
                                runat="server" Format="yyyy/MM/dd" ID="gcalDate1" />
                            <asp:TextBox runat="server" ID="txtQty1"></asp:TextBox>
                            <input id="btnQty" type="button" onclick="setQty()" value="set Qty" />
                            <asp:TextBox runat="server" ID="txtDueDate1"></asp:TextBox>
                            <input id="btnDueDate" type="button" onclick="setDueDate()" value="set DueDate" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 100%;position: relative" id="tbgv">
                                        <asp:GridView runat="server" ID="gvItems" Width="100%" AutoGenerateColumns="false"
                                            OnPageIndexChanging="gvItems_PageIndexChanging" OnRowUpdating="gvItems_RowUpdating"
                                            OnSorting="gvItems_Sorting" OnRowDataBound="gvItems_RowDataBound" OnRowEditing="gvItems_RowEditing"
                                            DataKeyNames="LN_NUM" ><%--DataSourceID="src1"--%>
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        No.
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="all" runat="server" Checked="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="item" runat="server" Checked="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Product">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtRowPN" Text='<%#Eval("PART_NO") %>' />
                                                        <asp:Literal ID="Literal_err" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%# SiebelQuote.ShowProdStatus(Eval("PART_NO"))%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Description" DataField="DESC_TEXT" />
                                                <asp:BoundField HeaderText="Product(Rpt)" DataField="Product_Rpt" Visible="false" />
                                                <asp:BoundField HeaderText="Description(Rpt)" DataField="Description_Rpt" Visible="false" />
                                                <asp:BoundField HeaderText="Start Price" DataField="START_PRICE" ItemStyle-HorizontalAlign="Right"
                                                    Visible="false">
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Item Net Price">
                                                    <ItemTemplate>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtPrice"
                                                            runat="server" FilterType="Numbers,Custom" ValidChars=".">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                        <asp:TextBox runat="server" ID="txtPrice" Text='<%#Eval("DISCOUNT_PRICE") %>' Width="80"
                                                            Style="text-align: right" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Discount" DataField="DISC" ItemStyle-HorizontalAlign="Center"
                                                    Visible="false">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Qty.">
                                                    <ItemTemplate>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtQty"
                                                            runat="server" FilterType="Numbers">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                        <asp:TextBox runat="server" ID="txtQty" Text='<%#Eval("QTY_REQ") %>' Width="50" Style="text-align: right" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Sub Total" DataField="" ItemStyle-HorizontalAlign="right">
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Due Date">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtDueDate" OnClientShown="calendarShown" Text='<%#Eval("duedate") %>'
                                                            Width="80" Style="text-align: right" />
                                                        <ajaxToolkit:CalendarExtender TargetControlID="txtDueDate" runat="server" OnClientShown="calendarShown"
                                                            Format="yyyy/MM/dd" ID="gcalDate" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="LN_NUM" DataField="LN_NUM" HeaderStyle-CssClass="displaynone"
                                                    ItemStyle-CssClass="displaynone">
                                                    <HeaderStyle CssClass="displaynone"></HeaderStyle>
                                                    <ItemStyle CssClass="displaynone"></ItemStyle>
                                                </asp:BoundField>
                                            </Columns>
                                            <%--       <CascadeCheckboxes>
                                                <sgv:CascadeCheckbox ChildCheckboxID="item" ParentCheckboxID="all" />
                                            </CascadeCheckboxes>
                                            <FixRowColumn FixColumns="-1" FixRows="-1" TableHeight="100%" TableWidth="100%" />--%>
                                        </asp:GridView>
                                        <asp:SqlDataSource runat="server" ID="src1" ConnectionString="<%$ConnectionStrings:CRM %>" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="background-color: #FFFFFF; height: 20px; width: 5%;" valign="top">
                                        <div align="right">
                                            <font color="#ffa500">*Opportunity:</font><br />
                                        </div>
                                    </td>
                                    <td style="background-color: #FFFFFF; height: 20px; width: 90%;" align="left" colspan="3">
                                        <asp:UpdatePanel runat="server" ID="UP_Opty" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <asp:Label ID="LabelOptyName" runat="server" Text="Opportunity Name:" Visible="false" />
                                                <asp:TextBox runat="server" ID="txtOptyName" MaxLength="300" Width="150px" ReadOnly="true"
                                                    BackColor="#ebebe4" />
                                                <asp:Label ID="LabelOptyStage" runat="server" Text="Stage:" />
                                                <asp:DropDownList ID="DDLOptyStage" runat="server">
                                                    <asp:ListItem Selected="True">25% Proposing/Quoting</asp:ListItem>
                                                    <asp:ListItem>50% Negotiating</asp:ListItem>
                                                    <asp:ListItem>75% Waiting for PO/Approval</asp:ListItem>
                                                    <asp:ListItem>100% Won-PO Input in SAP</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:ImageButton runat="server" ID="ibtn_PickOpty" ImageUrl="~/Images/search.gif"
                                                    OnClick="ibtn_PickOpty_Click" />
                                                <asp:Button ID="ButtonNewOpty" runat="server" Text="New Opportunity" OnClick="ButtonNewOpty_Click" />
                                                <asp:CheckBox ID="cbx_NewOpty" Text="New Opportunity" runat="server" OnCheckedChanged="cbx_NewOpty_CheckedChanged"
                                                    AutoPostBack="true" Visible="false" />
                                                <asp:Label ID="Label24" runat="server" Text="Opportunity ID:" Visible="false" />
                                                <asp:TextBox runat="server" ID="txtOptyRowID" Width="150px" ReadOnly="true" BackColor="#ebebe4"
                                                    Visible="false" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        (Opportunity will not be created until quotation is released to customer.)
                                    </td>
                                </tr>
                            </table>
                            <%--Pick Opportunity Panel--%>
                            <asp:Panel ID="PLPickOpty" runat="server" Style="display: none" CssClass="modalPopup">
                                <div style="text-align: right;">
                                    <asp:ImageButton ID="CancelButtonOpty" runat="server" ImageUrl="~/Images/del.gif" />
                                </div>
                                <div>
                                    <asp:UpdatePanel ID="UPPickOpty" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <myASCX:Pickopty1 ID="ascxPickopty1" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </asp:Panel>
                            <asp:LinkButton ID="lbDummyOpty" runat="server"></asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="MPPickOpty" runat="server" TargetControlID="lbDummyOpty"
                                PopupControlID="PLPickOpty" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonOpty"
                                DropShadow="false" />
                            <asp:HiddenField ID="HFOriginalQuoteID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%# SiebelQuote.ShowProdStatus(Eval("PART_NO"))%><%--       <CascadeCheckboxes>
                                                <sgv:CascadeCheckbox ChildCheckboxID="item" ParentCheckboxID="all" />
                                            </CascadeCheckboxes>
                                            <FixRowColumn FixColumns="-1" FixRows="-1" TableHeight="100%" TableWidth="100%" />--%>
                            <asp:Label runat="server" ID="lbPOPMsg" Font-Bold="true" ForeColor="Tomato" />
                            <%# SiebelQuote.ShowProdStatus(Eval("PART_NO"))%>
                            <br />
                            <asp:Button runat="server" ID="btnQuote2Order" Text="Add to Cart" OnClientClick="return showloading();" OnClick="btnQuote2Order_Click" />
                            &nbsp;  &nbsp;  &nbsp;
                             <asp:Button runat="server" ID="btnQuote2eQ" Text="Add to eQuotation" />
                            <input type="button" id="cbtnDirect2SAP" value="Direct2SAP" onclick="DivOffOrOn('div_OrderInfo')"
                                style="display: none;" />
                            <%--               <uc1:ChangeCompany runat="server" ID="chgCompany" Visible="false" />--%>
                            <div id="div_OrderInfo" style="display: none;">
                                <table>
                                    <tr align="left" valign="top">
                                        <td style="background-color: #FFFFFF">
                                            <div align="right">
                                                Required Date:</div>
                                        </td>
                                        <td style="width: 100px; height: 10px; background-color: #FFFFFF" align="left">
                                            <ajaxToolkit:CalendarExtender TargetControlID="txtReqDate" runat="server" Format="yyyy/MM/dd"
                                                ID="calDate" />
                                            <asp:TextBox ID="txtReqDate" runat="server" MaxLength="20" Width="95px" />
                                            ( YYYY/ MM / DD )
                                        </td>
                                        <td style="background-color: #FFFFFF; height: 66px">
                                            <div align="right">
                                                External Notes:<br />
                                                (Maximum 1000 Characters)</div>
                                        </td>
                                        <td style="background-color: #FFFFFF; height: 66px" align="left">
                                            <asp:TextBox ID="txtOrderNote" runat="server" TextMode="MultiLine" onblur="return checkdate( 'txtOrderNote','1000')"
                                                Columns="50" Rows="5" MaxLength="28" />
                                        </td>
                                        <td style="background-color: #FFFFFF; height: 66px">
                                            <div align="right">
                                                Sales Notes:<br>
                                                (Maximum 300 Characters)</div>
                                        </td>
                                        <td style="background-color: #FFFFFF; height: 66px" align="left">
                                            <asp:TextBox ID="txtSalesNote" runat="server" TextMode="MultiLine" onblur="return checkdate( 'txtSalesNote','300')"
                                                Columns="50" Rows="5" MaxLength="28" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Button runat="server" ID="btnConfirmOrder" Text="Convert to Order in SAP" Enabled="true"
                                    OnClick="btnConfirmOrder_Click" />&nbsp;
                                <asp:UpdatePanel runat="server" ID="upOrderMsg" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="lbOrderMsg" ForeColor="Tomato" Font-Bold="true" Font-Size="Larger" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnConfirmOrder" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <input type="button" id="cbtnClose" value="Close" onclick="DivOffOrOn('div_OrderInfo')"
                                    style="display: none;" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="hf_qty" runat="server" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="Panel1"
        TargetControlID="HiddenField1" BackgroundCssClass="modalBackground" CancelControlID="HiddenField1"
        BehaviorID="Panel1">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;
        width: 400px;">
        <asp:UpdatePanel ID="UPPickFm" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table width="100%" border="0" align="center" cellspacing="2" cellpadding="2">
                    <tr>
                        <td>
                        </td>
                        <td align="right">
                            【<a href="#" onclick="return closepanel('Panel1');">Close</a>】
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="40%">
                            <strong>ER Employee :</strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtERE" runat="server"></asp:TextBox>
                            <%--      <asp:ImageButton ID="pickERE" runat="server" ImageUrl="~/images/pickPick.gif" OnClick="pickERE_Click" />--%>
                            <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ft1" TargetControlID="txtERE"
                                FilterType="Numbers, Custom" />
                        </td>
                    </tr>
                    <tr>
                        <td height="30">
                        </td>
                        <td align="left">
                            <asp:Button ID="btnQuote2Ordernew" runat="server" Text="Confirm" OnClientClick="return checkPar();"
                                OnClick="btnQuote2Ordernew_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td height="30" colspan="2">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:LinkButton ID="lbDummyERE" runat="server"></asp:LinkButton>
    <div id="prediv" style="display:none;"> <img src='../images/LoadingRed.gif' alt='Loading...' />  Loading... </div>
    <%--    <ajaxtoolkit:modalpopupextender id="MPPickERE" runat="server" targetcontrolid="lbDummyERE"
        popupcontrolid="PLPickERE" backgroundcssclass="modalBackground" cancelcontrolid="CancelButtonERE"
        dropshadow="true" />
        <asp:Panel ID="PLPickERE" runat="server" Style="display: none" CssClass="modalPopup">
        <div style="text-align: right;">
            <asp:LinkButton ID="CancelButtonERE" runat="server" Text="Close" />
        </div>
        <div>
            <asp:UpdatePanel ID="UPPickERE" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:ERE ID="ascxPickERE" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>--%>
    <script language="javascript" type="text/javascript">
        function openpanel(panel_id) {
            $find(panel_id).show();
            return false;
        }
        function closepanel(panel_id) {
            $find(panel_id).hide();
            return false;
        }
        function checkPar() {
            var par1 = document.getElementById("<%=txtERE.ClientID %>");
            alert(par1.value);
            if (par1.value == '') {
                par1.style.backgroundColor = "#ff0000";
                return false;
            }
        }
        function showloading() {
            var par1 = document.getElementById("<%=lbPOPMsg.ClientID%>");
            var par2 = document.getElementById("<%=txtOptyName.ClientID%>");
            par1.innerText = "";
            if (par2.value == '') {
                par2.style.backgroundColor = "#ff0000";
                par1.innerText = "Please specify an opportunity for this quote.";
                return false;
            }
            $("#prediv").show();
            // var html = "<img src='http://eq.advantech.com/images/LoadingRed.gif' alt='Loading...' />  Loading... ";
            // $("#<%=lbPOPMsg.ClientID%>").html(html);
            return true;
        }
    </script>
</asp:Content>