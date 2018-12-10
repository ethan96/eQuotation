<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="Quotation2Siebel.aspx.vb" Async="true" Inherits="EDOC.Quotation2Siebel" %>

<%@ Register Assembly="EDOC" Namespace="EDOC" TagPrefix="cc1" %>

<%@ Register Src="~/ascx/CheckUID.ascx" TagName="CheckUID" TagPrefix="myASCX" %>
<%@ Register Src="~/ascx/PickSiebelOpportunity.ascx" TagName="Pickopty1" TagPrefix="myASCX" %>
<%@ Register Src="~/ascx/SendeQuotationUI.ascx" TagName="SendeQuotation1" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        #BgDiv {
            background-color: #000;
            position: absolute;
            z-index: 99;
            left: 0;
            top: 0;
            display: none;
            width: 100%;
            height: 1000px;
            opacity: 0.5;
            filter: alpha(opacity=50);
            -moz-opacity: 0.5;
        }

        #DialogDiv {
            position: absolute;
            width: 600px;
            left: 50%;
            top: 50%;
            margin-left: -300px;
            margin-top: -63px;
            height: 125px;
            z-index: 100;
            background-color: #fff;
            border: 4px #BF7A06 solid;
            padding: 1px;
        }

            #DialogDiv .form {
                padding: 10px;
                line-height: 20px;
                font-weight: bold;
                color: Black;
            }
    </style>
    <% If Request("UID") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request("UID")) AndAlso (_IsATWUser Or _IsCNAonline) Then%>
    <table id="tbFlow" class="Ind" style="width: 1024px" align="center">
        <tr>
            <td width="121">
                <a href="quotationMaster.aspx?UID=<%=Request("UID")%>">
                    <img src="../Images/Master.gif" alt="" /></a>
            </td>
            <td width="121"><a href="QuotationDetail.aspx?VIEW=0&UID=<%=Request("UID")%>">
                <img src="../Images/Detail.gif" alt="" /></a>

            </td>
            <td width="121"><a href="Quotation2Siebel.aspx?UID=<%=Request("UID")%>">
                <img src="../Images/Preview.gif" alt="" /></a>
            </td>
            <td></td>
        </tr>
    </table>
    <% End If%>
    <asp:UpdatePanel ID="UP_QuotationPreview" runat="server" UpdateMode="Conditional"
        Visible="true">
        <ContentTemplate>
            <table style="width: 1024px" id="view_type_option" runat="server"
                align="center">
                <tr>
                    <td style="width: 80px" valign="middle">View Type：
                    </td>
                    <td style="width: 844px">
                        <asp:RadioButtonList ID="RadioButtonList_PriviewMode" runat="server" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="RadioButtonList_PriviewMode_SelectedIndexChanged" AutoPostBack="True"
                            Width="300px">
                            <asp:ListItem Selected="True" Value="true">Internal User</asp:ListItem>
                            <asp:ListItem Value="false">External User</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td valign="middle">
                        <asp:DropDownList ID="drpAJPOffice" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="drpAJPOffice_SelectedIndexChanged" Width="150px">
                            <asp:ListItem Value="0">-</asp:ListItem>
                            <asp:ListItem Value="1">Tokyo Office</asp:ListItem>
                            <asp:ListItem Value="2">Osaka Office</asp:ListItem>
                            <asp:ListItem Value="3">Nagoya Office</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td runat="server" id="tdpdf" visible="false" align="left">
                        <asp:ImageButton ID="ImageBtPdf" runat="server" ImageUrl="~/Images/pdf.gif" OnClick="ImageBtPdf_Click" />
                    </td>
                </tr>
            </table>
            <myASCX:CheckUID runat="server" ID="ascxCheckUID" />
            <asp:HiddenField runat="server" ID="hdQuoteId" />
            <table width="1024px" id="AKR_Pricing_option" runat="server" visible="false" border="0" >
                <tr>
                    <td width="30%" align="right" valign="middle">
                        Nego Price visibility control:
                    </td>
                    <td width="70%" align="left">
<%--Nadia 20170824:Sales can select
1) Nego price only ==> Nego / nego vat / total
2) List price only ==> List / List vat / total
3) List & Nego ==> List/ Nego / Nego vat / total--%>
                        <asp:RadioButtonList ID="RadioButtonList_ListPriceOnly" runat="server" RepeatDirection="Horizontal"
                             AutoPostBack="True"  Width="350px">
                            <asp:ListItem Selected="True" Value="0">List & Nego Price</asp:ListItem>
                            <asp:ListItem  Value="2">Nego Price Only</asp:ListItem>
                            <asp:ListItem  Value="1">List Price Only</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td align="center">
                        <cc1:NoToolBarEditor2 ID="NoToolBarEditor" runat="server" Height="800" Width="1024"
                            PreviewMode="true" PreviewOnInit="true" ShowWaitMessage="true" ShowQuickFormat="false"
                            Submit="false" NoScript="true" />
                        <%-- <uc1:NoToolBarEditor2 ID="NoToolBarEditor" runat="server" Height="800" Width="1024"
                            PreviewMode="true" PreviewOnInit="true" ShowWaitMessage="true" ShowQuickFormat="false"
                            Submit="false" NoScript="true" />--%>
                        <%--<ajaxToolkit:Editor runat="server" ID="NoToolBarEditor" Height="800" Width="1024"
                    PreviewMode="true" PreviewOnInit="false" ShowWaitMessage="true" ShowQuickFormat="false"
                    Submit="false" NoScript="true" />--%>
                    </td>
                </tr>
                <tr runat="server" id="trFreight">
                    <td align="center">
                        <asp:UpdatePanel ID="UPAUSFreight" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table align="center" style="width: 1024px">
                                    <tr id="trAUSFreight" visible="false" runat="server">
                                        <td colspan="2">
                                            <div id="dialog_target"></div>
                                            <div id="divFreightWindow" style="display: none;">
                                                <asp:Label ID="labnomsg" runat="server" Text="" Font-Bold="true" ForeColor="Tomato"> </asp:Label>
                                                <asp:GridView ID="gvship" runat="server" DataKeyNames="freight" AutoGenerateColumns="false" Width="400">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <input name="rdship" type="radio" data-freight="<%#Eval("freight")%>" data-name="<%#Eval("name")%>" />
                                                                <asp:RadioButton ID="rdship" AutoPostBack="true" runat="server" OnCheckedChanged="rdship_CheckedChanged" Visible="false" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="name" HeaderText="Ship Condition" ItemStyle-HorizontalAlign="center">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Freight " HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                $<%#Eval("Freight") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <table width="100%" id="tbbt" runat="server">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button ID="btConfirm" runat="server" Text="Confirm" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:HiddenField ID="HFcopytext" runat="server" />
                                                            <input type="button" id="btcopy" value="Copy" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:HiddenField ID="HFdatafreight" runat="server" />
                                                            <asp:HiddenField ID="HFdataname" runat="server" />
                                                            <input id="btCancel" type="button" onclick="return CloseFreight();" value="Cancel" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <input id="btec" type="button" value=" Freight Calculation" class="hide" onclick="return ShowFreight();" />
                                            <asp:Button ID="btAUSfreight" runat="server" Text=" Freight Calculation" />
                                        </td>
                                    </tr>
                                    <tr id="trAEUFreight" visible="false" runat="server">
                                        <td width="340px" id="tdFreight">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlFreight" runat="server" Visible="false" DataTextField="TextStr" DataValueField="freight">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle">Freight：
                                                        <asp:TextBox ID="TBFreight" runat="server" Style="width: 130px;"></asp:TextBox>
                                                        <asp:Button ID="btAdjus" runat="server" Text="Adjust" />
                                                        &nbsp;&nbsp;
                                                        <asp:Button ID="btRefresh" runat="server" Text="Refresh" />
                                                        <br />
                                                        <asp:Label ID="labnodata" runat="server" Text="" Font-Bold="true" ForeColor="Tomato"> </asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <font color="red">
                                                <asp:Literal ID="LitAEUfreight" runat="server" Visible="false">   Our shipping cost is an estimation, this cost submit to modification
                                        by TNT contract and actual shipment packaging.
                                        <br /> The real shipment cost will show on our official invoice day of shipment.</asp:Literal>
                                            </font>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <script type="text/javascript">
                            $(function () {
                                BindddlFreight(); SetRadioName();
                            });
                            function BindddlFreight() {
                                $("#<%=ddlFreight.ClientID%>").on("change", function () {
                                    $("#<%=TBFreight.ClientID%>").val($(this).val());
                                });
                            }
                            var prm = Sys.WebForms.PageRequestManager.getInstance();
                            prm.add_endRequest(function () {
                                BindddlFreight(); SetRadioName();
                                if (!window.clipboardData) { $("#btcopy").hide(); }
                                $("#divFreightWindow").delegate("#btcopy", "click", function () {
                                    window.clipboardData.clearData();
                                    window.clipboardData.setData("Text", $("#<%=HFcopytext.ClientID%>").val());
                                });
                            });
                            function SetRadioName() {
                                $("#<%=gvship.ClientID%> :radio").click(function () {
                                    $("#<%=HFdatafreight.ClientID%>").val($(this).attr("data-freight"));
                                    $("#<%=HFdataname.ClientID%>").val($(this).attr("data-name"));
                                });
                            }
                        </script>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="RadioButtonList_PriviewMode" />
            <asp:PostBackTrigger ControlID="ImageBtPdf" />
        </Triggers>
    </asp:UpdatePanel>

    <table style="width: 1024px" align="center" runat="server" id="tbopty">
        <tr>
            <td style="background-color: #FFFFFF; height: 20px; width: 80px;" valign="middle">
                <font color="#ff0000">*</font>Opportunity：
            </td>
            <td style="background-color: #FFFFFF; height: 20px; width: 944px;" align="left" colspan="3">
                <asp:UpdatePanel runat="server" ID="UP_Opty" UpdateMode="Conditional" ChildrenAsTriggers="false">
                    <ContentTemplate>
                        <asp:Label ID="LabelOptyName" runat="server" Text="Opportunity Name:" Visible="false" />
                        <asp:TextBox runat="server" ID="txtOptyName" MaxLength="300" Width="150px" ReadOnly="true"
                            BackColor="#ebebe4" Visible="false" />
                        <asp:Label ID="LabelOptyStage" runat="server" Text="Stage:" Visible="false" />
                        <asp:DropDownList ID="DDLOptyStage" runat="server" Visible="false">
                            <asp:ListItem Selected="True">25% Proposing/Quoting</asp:ListItem>
                            <asp:ListItem>50% Negotiating</asp:ListItem>
                            <asp:ListItem>75% Waiting for PO/Approval</asp:ListItem>
                            <asp:ListItem>100% Won-PO Input in SAP</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="labopty_owner_email" runat="server" Text="Opportunity Owner Email:" Visible="false" />
                        <asp:TextBox ID="TBopty_owner_email" runat="server" Visible="false" Width="180" />
                        <asp:ImageButton runat="server" ID="ibtn_PickOpty" ImageUrl="~/Images/search.gif"
                            OnClick="ibtn_PickOpty_Click" />
                        <asp:Button ID="ButtonNewOpty" runat="server" Text="New Opportunity"
                            OnClick="ButtonNewOpty_Click" />
                        <asp:CheckBox ID="cbx_NewOpty" Text="New Opportunity" runat="server" OnCheckedChanged="cbx_NewOpty_CheckedChanged"
                            AutoPostBack="true" Visible="False" />
                        <asp:Label ID="Label24" runat="server" Text="Opportunity ID:" Visible="false" />
                        <asp:TextBox runat="server" ID="txtOptyRowID" Width="150px" ReadOnly="true" BackColor="#ebebe4" Visible="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                (Opportunity will not be created until quotation is released to customer.)
            </td>
        </tr>
    </table>
    <asp:Panel ID="PLPickOpty" runat="server" Style="display: none" CssClass="modalPopup"
        OnPreRender="PLPickOpty_PreRender">
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
    <table>
        <tr>
            <td>
                <asp:UpdatePanel runat="server" ID="upForward" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="PanelForward" Visible="false">
                            <myASCX:SendeQuotation1 ID="forwardEquotationUI" runat="server" />
                            <%--<b>Recipient Email:</b>--%>
                            <asp:TextBox runat="server" ID="txtForwardTo" Width="300" Visible="false" /><%-- <font color="gray">(For multi-Email address please separates with ";")</font>--%>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cbForward" EventName="CheckedChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>

            <td align="center">
                <asp:Button runat="server" ID="btnGoSiebel" Text="Confirm & Transfer To Siebel" OnClick="btnGoSiebel_Click" OnClientClick="return BlockConfirmButton(this)" />
                &nbsp;
                <asp:CheckBox runat="server" ID="cbForward" Text="Forward to Customer" AutoPostBack="true"
                    OnCheckedChanged="cbForward_CheckedChanged" Visible="True" />
                <asp:UpdatePanel runat="server" ID="Up3siebel">
                    <ContentTemplate>
                        <asp:Label ID="lb_dueinvoice" runat="server" Text="" ForeColor="Red" Font-Size="Larger"></asp:Label>
                        <asp:Label ID="Laberror" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </ContentTemplate>
                    <%--            <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGoSiebel" EventName="Click" />
                    </Triggers>--%>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div id="BgDiv"></div>
    <div id="DialogDiv" style="display: none">
        <div class="form">
            Your quotation is being processed and may take several seconds. Please do not close or refresh this page, or your quotation may not be processed successfully, thank you.
    <br />
            <asp:Image runat="server" ID="imgMasterLoad" ImageUrl="~/Images/LoadingRed.gif" />
            <b>Loading ...</b>
        </div>
    </div>

    <asp:Panel ID="PL_AACGP_Confirm" runat="server" Style="display: none" CssClass="modalPopup">
        <div>
            <asp:UpdatePanel ID="UP_AACGPConfirm" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView runat="server" ID="gvGPQuoteLine" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("LineNo")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Material" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("PartNo")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("qty")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="List Unit Price" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("ListPrice")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quoted Unit Price" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("UnitPrice")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quoted Discount" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("Discount") * 100%>%
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cost (ITP)" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("Cost")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GP" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("GPPercent") * 100%>%
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Min GP Threshold" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("ThresholdPercent") * 100%>%
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField HeaderText="GP Block" DataField="GPBlock" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                    </asp:GridView>
                    <asp:GridView runat="server" ID="gvAACGPTotalInfo" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Total List Price" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("TotalListPrice")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Unit Price" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("TotalUnitPrice")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Discount" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("TotalDiscount") * 100%>%
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Cost" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("TotalCost")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total GP" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("TotalGPPercent") * 100%>%
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Min. GP Threshold" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("TotalThresholdPercent") * 100%>%
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField HeaderText="GP Block" DataField="IsTotalGPBlock" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                    </asp:GridView>
                    <script>
                        function checkreason() {
                            if ($.trim($("#ContentPlaceHolder1_HF_isgoingGP").val()) == "0") return true;
                            var val = $.trim($("#ContentPlaceHolder1_GPreason").val());
                            if (val == "") {
                                $("#ContentPlaceHolder1_GPreason").focus();
                                return false;
                            }
                        }
                    </script>
                </ContentTemplate>
            </asp:UpdatePanel>
            <table>
                <tr>
                    <td>
                        <asp:HiddenField ID="HF_isgoingGP" runat="server" Value="0" />
                        <asp:TextBox ID="GPreason" Width="98%" runat="server" TextMode="MultiLine" placeholder="Please enter the reason of low GP"></asp:TextBox>
                        <asp:Label ID="gp2mail" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="CancelButtonConfirm" runat="server" Text="Cancel" />
                        <asp:Button ID="btnAACGPRealConfirm" runat="server" Text="Confirm" OnClientClick="return checkreason();"
                            OnClick="btnRealConfirm_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lbDummyConfirm" runat="server" />
    <ajaxToolkit:ModalPopupExtender ID="MP_AACGP_Confirm" runat="server" TargetControlID="lbDummyConfirm"
        PopupControlID="PL_AACGP_Confirm" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonConfirm"
        DropShadow="true" />

    <script type="text/javascript">

        function BlockConfirmButton(O) {
            ShowDIV('DialogDiv');

            <% If _IsAJPUser Then %>
            AJPupdate();
            <% End If %>

            return true;
        }

        function ShowDIV(thisObjID) {
            $("#BgDiv").css({ display: "block", height: $(document).height() });
            var divId = document.getElementById(thisObjID);
            divId.style.top = ((document.body.clientHeight - divId.clientHeight) / 2 + document.body.scrollTop / 2) + "px";
            $("#" + thisObjID).css("display", "block");
        }


        function AJPupdate() {

            console.log("AJP update");

            var CustomerName = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#JPtable1_1").find("td").eq(1).find("input[type='text']").val();
            var CustomerName2 = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#JPtable1_1").find("td").eq(3).find("input[type='text']").val();
            var CustomerAddr = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#JPtable1_1").find("td").eq(5).find("input[type='text']").val();
            var CustomerAddr2 = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#ctl00_CustomerAddr2").val();
            var CustomerTel = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#ctl00_CustomerTel").val();
            var CustomerFax = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#ctl00_CustomerFax").val();
            var CustomerContact = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#JPtable1_1").find("td").eq(7).find("input[type='text']").val();
            var CustomerEM = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#JPtable1_1").find("td").eq(9).find("input[type='text']").val();
            var CustomerPaymentTerm = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#JPtable1_1").find("td").eq(11).find("input[type='text']").val();
            var CustomerShipMethod = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#JPtable1_1").find("td").eq(13).find("input[type='text']").val();
            var SalesName = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#ctl00_SalesName").val();
            var Creator = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#ctl00_Creator").val();
            var Notes = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#ctl00_txtNotes").val();
            var BTOSParent = $(window.frames["<%=NoToolBarEditor.ClientID%>_ctl02_ctl00"].document).find("#ctl00_RepeaterBtosDetail_btospatno_0").val();

            if (!CustomerAddr2) CustomerAddr2 = "";
            if (!BTOSParent) BTOSParent = "";

            var postData = {
                _QuoteID: "<%=Request("UID")%>",
                _CustomerName: CustomerName,
                _CustomerName2: CustomerName2,
                _CustomerAddr: CustomerAddr,
                _CustomerAddr2: CustomerAddr2,
                _CustomerTel: CustomerTel,
                _CustomerFax: CustomerFax,
                _CustomerContact: CustomerContact,
                _CustomerEM: CustomerEM,
                _CustomerPaymentTerm: CustomerPaymentTerm,
                _CustomerShipMethod: CustomerShipMethod,
                _SalesName: SalesName,
                _Creator: Creator,
                _Notes: Notes,
                _BTOSParent: BTOSParent
            };
            $.ajax({
                url: "<%= Util.GetRuntimeSiteUrl()%>/Services/AJPTerms.asmx/SaveAJPTemplateChanges",
                type: "POST",
                dataType: 'json',
                data: postData,
                success: function (retData) {
                    if (retData == true) {
                        //alert('Data has been updated to database successfully.');
                    }
                    return false;
                },
                error: function (msg) {
                    return false;
                }
            });

            return false;
        }

    </script>
</asp:Content>
