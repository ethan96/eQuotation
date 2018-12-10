<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="JPAOnlineQuoteTemplateV4.ascx.vb"
    Inherits="EDOC.JPAOnlineQuoteTemplateV4" %>
<div>
    <style type="text/css">
        .w25p {
            width: 25%;
        }

        .w30p {
            width: 30%;
        }

        .w100p {
            width: 100%;
        }

        .h210 {
            height: 210px;
        }

        .textR {
            text-align: right;
        }

        .padding5 {
            padding: 5px 5px 5px 5px;
        }

        .double_underline {
            border-bottom: 3px double;
        }

        .style1 {
            color: black;
            font-size: 11.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Meiryo UI, sans-serif;
            text-align: left;
            vertical-align: middle;
            border-left: 1px solid windowtext;
            border-top: 1px solid windowtext;
            padding: 0px;
        }


        .styletd {
            color: black;
            font-size: 11.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Meiryo UI, sans-serif;
            text-align: left;
            vertical-align: middle;
            border-top: 1px solid windowtext;
            border-right: 1px solid windowtext;
            border-bottom: 1px solid windowtext;
            padding: 0px;
            padding-left: 3px;
        }

        .styletdhidetopborder {
            color: black;
            font-size: 11.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Meiryo UI, sans-serif;
            text-align: left;
            vertical-align: middle;
            border-top: none;
            border-right: 1px solid windowtext;
            border-bottom: none;
            padding: 0px;
            padding-left: 3px;
        }

        .styletdbg {
            background: #B6DDE8;
            background-color: #B6DDE8;
        }

        .styleReadOnly {
            color: black;
            font-size: 11.0pt;
            font-weight: 600;
            font-style: normal;
            text-decoration: none;
            /*font-family: メえイぁリえオ, sans-serif;*/
            font-family: Meiryo UI, sans-serif;
            text-align: left;
            vertical-align: middle;
            padding: 0px;
            border-width: 0px;
            margin-bottom: 1px;
            margin-top: 1px;
            margin-left: 1px;
        }

        .styleReadOnlycenter {
            text-align: center;
        }

        .styleReadOnlyleft {
            text-align: left;
        }

        .styleReadOnlyEW {
            text-align: left;
        }

        .styleReadOnly2 {
            /*font-family: メえイぁリえオ, sans-serif;*/
            font-family: Meiryo UI, sans-serif;            
            text-align: left;
            vertical-align: middle;
            border-width: 0px;
        }

        .styleReadOnly3 {
            /*font-family: メえイぁリえオ, sans-serif;*/
            font-family: Meiryo UI, sans-serif;            
            vertical-align: middle;
            border: 1px solid;
            background: #ffffff;
            border: 1px solid #000000;
        }

        .styleReadOnly4 {
            /*font-family: メえイぁリえオ, sans-serif;*/
            font-family: Meiryo UI, sans-serif;
            vertical-align: middle;            
            background: #ffffff;
            font-size:14px;
        }

        .font6 {
            color: black;
            font-size: 10.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Meiryo UI, sans-serif;
        }

        .style2 {
            color: black;
            font-size: 11.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Meiryo UI, sans-serif;
            text-align: center;
            vertical-align: middle;
            white-space: nowrap;
            border-left: 1px solid windowtext;
            border-right: 1px solid windowtext;
            border-top: 1px solid windowtext;
            border-bottom: 1px solid windowtext;
            padding: 0px;
            background: #B6DDE8;
        }

        .style_while_backcolor {
            color: black;
            font-size: 11.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Meiryo UI, sans-serif;
            vertical-align: middle;
            white-space: nowrap;
            border-left: 1px solid windowtext;
            border-right: 1px solid windowtext;
            border-top: 1px solid windowtext;
            border-bottom: 1px solid windowtext;
            /*       border-top: 1px solid #B6DDE8;
            border-right: 1px solid #B6DDE8;
            border-bottom: 1px solid #B6DDE8;*/
            padding: 0px;
        }

        .style9 {
            height: 17.45pt;
            color: black;
            font-size: 10.0pt;
            font-weight: 400;
            font-family: Meiryo UI, sans-serif;
        }

        .hide {
            display: none;
        }

        .fontnormal {
            font-weight: normal;
            width: 50px;
        }

        .style3 {
            color: black;
            font-size: 11.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Meiryo UI, sans-serif;
            vertical-align: middle;
            white-space: nowrap;
            border-bottom: 1px solid windowtext;
            text-align: right;
        }

        .style4 {
            color: black;
            font-size: 11.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: Meiryo UI, sans-serif;
            vertical-align: middle;
            white-space: nowrap;
            border-top: 3px solid windowtext;
            text-align: right;
        }

        span, td, th {
            font-size: 10.0pt;
            font-family: Meiryo UI, sans-serif;
        }

        .gtd td {
            border: 1px solid #000000;
        }
    </style>
    <div id="divHeader">
        <table width="830" border="0" cellspacing="0" cellpadding="0" align="center">
            <tr>
                <td align="center">
                    <table width="830" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td align="left">
                                <img src='<%=Util.GetRuntimeSiteIP()%>/Images/2010-Logo-with-Slogan.jpg' alt="Advantech"
                                    width="350px" />
                            </td>
                            <td align="right">
                                <img src='<%=Util.GetRuntimeSiteIP()%>/Images/AJPCompanyInfoV2.png' width="300px"
                                    alt="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <table style="width: 830px; border: 0px;" cellspacing="0" cellpadding="0" align="center">
        <tr>
            <td align="center" colspan="2">
                <img src='<%=Util.GetRuntimeSiteIP()%>/Images/AJPQuotationTitle.jpg' alt="" />
            </td>
        </tr>
        <tr>
            <td height="20" align="center" colspan="2"></td>
        </tr>
        <tr>
            <td style="width: 480px">
                <table id="JPtable1_1" border="0" class="style1 h210" cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                    <tr>
                        <td class="styletd styletdbg w25p" height="24">貴社名:
                        </td>
                        <td class="styletd">
                            <input runat="server" id="CustomerName" class="styleReadOnly" style="width: 99%;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletd styletdbg" height="24">部署:
                        </td>
                        <td class="styletd">
                            <input runat="server" id="CustomerName2" class="styleReadOnly" style="width: 99%;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletd styletdbg" height="48">住所:
                        </td>
                        <td class="styletd">
                            <input runat="server" id="CustomerAddr" class="styleReadOnly" style="width: 99%;" />
                            <input runat="server" id="CustomerAddr2" visible="false" class="styleReadOnly" style="width: 77%; margin-left: 75px;" />
                            Tel:<input runat="server" id="CustomerTel" class="styleReadOnly" style="width: 40%;" />
                            Fax:<input runat="server" id="CustomerFax" class="styleReadOnly" style="width: 40%;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletd styletdbg" height="24">御担当者名:
                        </td>
                        <td class="styletd">
                            <input runat="server" id="CustomerContact" class="styleReadOnly" style="width: 99%;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletd styletdbg" height="24">エンドユーザ:
                        </td>
                        <td class="styletd">
                            <input runat="server" id="CustomerEM" class="styleReadOnly" style="width: 99%;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletd styletdbg" height="24">支払条件:
                        </td>
                        <td class="styletd">
                            <input runat="server" id="CustomerPaymentTerm" class="styleReadOnly" style="width: 99%;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletd styletdbg" height="24">納品場所:
                        </td>
                        <td class="styletd">
                            <input runat="server" id="CustomerShipMethod" class="styleReadOnly" style="width: 99%;" />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right" style="width: 350px; vertical-align: top;">
                <table id="JPtable1_2" border="0" class="style1 h210" cellpadding="0" cellspacing="0" style="border-collapse: collapse; margin-left: 15px">
                    <tr style="height: 24">
                        <td class="styletd styletdbg" height="24">見積書番号:
                        </td>
                        <td class="styletd">
                            <input runat="server" id="LabelQuoteID" readonly="readonly" class="styleReadOnly" style="width: 99%;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletd styletdbg" height="24">発行日:
                        </td>
                        <td class="styletd">
                            <input runat="server" id="quoteDate" readonly="readonly" class="styleReadOnly" style="width: 99%;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletd styletdbg" height="24">見積有効期限:
                        </td>
                        <td class="styletd">
                            <input runat="server" id="expiredDate" readonly="readonly" class="styleReadOnly" style="width: 99%;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletdhidetopborder" colspan="2" height="24">
                            <asp:TextBox ID="txtAdvantech" Text="アドバンテック株式会社" runat="server" ReadOnly="true" CssClass="styleReadOnly styleReadOnlyleft" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletdhidetopborder" colspan="2" height="24">営業担当:
                            <input runat="server" id="SalesName" class="styleReadOnly" style="width: 75%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletdhidetopborder" colspan="2" height="24">作成者:
                            <input runat="server" id="Creator" class="styleReadOnly" style="width: 80%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletdhidetopborder" colspan="2" height="24">
                            <input runat="server" id="AdvantechAddr" readonly="readonly" class="styleReadOnly" style="width: 99%;" />
                        </td>
                    </tr>
                    <tr runat="server" id="trAdvantechAddr2" visible="false">
                        <td class="styletdhidetopborder" colspan="2" height="24">
                            <input runat="server" id="AdvantechAddr2" readonly="readonly" class="styleReadOnly" style="margin-left: 80px; width: 40%;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="styletdhidetopborder" style="border-bottom: 1px solid windowtext;" colspan="2" height="24">Tel:<input runat="server" id="AdvantechTel" readonly="readonly" class="styleReadOnly" style="width: 37%;" />
                            Fax:<input runat="server" id="AdvantechFax" readonly="readonly" class="styleReadOnly" style="width: 37%;" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="10" align="center" colspan="2"></td>
        </tr>
        <tr>
            <td height="20" align="left" colspan="2">下記の通り御見積いたします。
            </td>
        </tr>
        <tr>
            <td height="20" align="center" colspan="2"></td>
        </tr>
        <tr runat="server" id="trBTOS" visible="false">
            <td align="center" colspan="2" style="width: 100%">
                <asp:Repeater ID="RepeaterBtosDetail" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="center" colspan="2">
                                <table id="JPtable2" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse;"
                                    width="830">
                                    <tr>
                                        <td class="style2" height="25" rowspan="2" width="10%">一式型番
                                        </td>
                                        <td class="style_while_backcolor" rowspan="2" style="text-align: left; padding-left: 5px;">
                                            <input runat="server" id="btospartno" value="<%# CType(Container.DataItem, IBUS.iCartLine).VirtualPartNo.Value%>" class="styleReadOnly" style="width: 99%" />
                                        </td>
                                        <td class="style2" width="7%">数量
                                        </td>
                                        <td class="style2" width="15%">仕切単価（税抜）
                                        </td>
                                        <td class="style2" width="15%">見積合計（税抜）
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style_while_backcolor" height="40" style="text-align: center;">
                                            <input readonly="readonly" style="width: 50px;" value="<%# CType(Container.DataItem, IBUS.iCartLine).Qty.Value%>" class="styleReadOnly styleReadOnlycenter" />
                                        </td>
                                        <td class="style_while_backcolor" style="text-align: center;">
                                            <input readonly="readonly" runat="server" id="newunitPrice" style="width: 130px;" value=""
                                                class="styleReadOnly styleReadOnlycenter" />
                                        </td>
                                        <td class="style_while_backcolor" style="text-align: center;">
                                            <input readonly="readonly" runat="server" id="subtotal" style="width: 130px;" value=""
                                                class="styleReadOnly styleReadOnlycenter" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trEWAmount" visible="false">
                                        <td class="style2" height="42"></td>
                                        <td class="style_while_backcolor" colspan="5" style="text-align: left; padding-left: 5px; background-color: #D9D9D9;">
                                            <input runat="server" id="EWAmount" readonly="readonly" style="width: 600px; background-color: #d9d9d9;" value=""
                                                class="styleReadOnly styleReadOnlyEW" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style2" height="42">製品説明
                                        </td>
                                        <td class="style_while_backcolor" colspan="5" style="text-align: left; padding-left: 5px;">
                                            <textarea runat="server" id="btosdesc" value=" <%# CType(Container.DataItem, IBUS.iCartLine).partDesc.Value%>" class="styleReadOnly" style="width: 99%;" rows="2" />
                                            <%--<input runat="server" id="btosdesc" value=" <%# CType(Container.DataItem, IBUS.iCartLine).partDesc.Value%>" class="styleReadOnly" style="width: 99%; height:100px; word-break: break-word;" />      --%>                                     
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="5" align="center" colspan="2"></td>
                        </tr>
                        <tr runat="server" id="btosdetail" visible="false">
                            <td colspan="2">
                                <font style="font-family: 'メイリオ'; font-size: 11pt;">【詳細】</font>
                                <asp:GridView ID="gv1" runat="server" AllowPaging="false"
                                    EmptyDataText="no Item." AutoGenerateColumns="false" OnRowDataBound="gv1_RowDataBound"
                                    Font-Size="11pt" Width="830" Font-Names="メイリオ" BorderColor="Black" BorderStyle="Solid">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="left" HeaderText="パーツ型番" HeaderStyle-Width="30%" HeaderStyle-CssClass="style2" ItemStyle-CssClass="styleReadOnly3">
                                            <ItemTemplate>
                                                <input readonly="readonly" runat="server" id="btospartno" style="font-weight: normal;" value="<%# CType(Container.DataItem, IBUS.iCartLine).partNo.Value%>"
                                                    class="styleReadOnly" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="パーツ詳細" HeaderStyle-Width="67%" HeaderStyle-CssClass="style2" ItemStyle-CssClass="styleReadOnly3">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbdescription" Style="font-weight: normal;" Text="<%# CType(Container.DataItem, IBUS.iCartLine).partDesc.Value%>"
                                                    class="styleReadOnly w100p" />
                                                <%--                                                <input readonly="readonly" runat="server" id="lbdescription" style="font-weight: normal;" value="<%# CType(Container.DataItem, IBUS.iCartLine).partDesc.Value%>"
                                                    class="styleReadOnly w100p" />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="3%" HeaderStyle-CssClass="style2" ItemStyle-CssClass="styleReadOnly3">
                                            <HeaderTemplate>
                                                数量
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" Text='<%# CType(Container.DataItem, IBUS.iCartLine).Qty.Value%>' ID="lbGVQty" ReadOnly="true"
                                                    CssClass="styleReadOnly styleReadOnlycenter fontnormal"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#B6DDE8" ForeColor="Black" />
                                    <PagerStyle BorderColor="Black" />
                                </asp:GridView>
                                <div style="height: 10px;"></div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </td>
        </tr>
        <tr runat="server" id="trLooseItems" visible="false">
            <td align="center" colspan="2" style="width: 100%">
                <asp:GridView ID="gvLooseItems" runat="server" AllowPaging="false" EmptyDataText="no Item." AutoGenerateColumns="false"
                    Font-Size="11pt" Width="830" Font-Names="メイリオ" BorderColor="Black" BorderStyle="Solid" OnRowDataBound="gvLooseItems_RowDataBound">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="left" HeaderText="No" HeaderStyle-Width="4%" HeaderStyle-CssClass="style2" ItemStyle-CssClass="styleReadOnly3">
                            <ItemTemplate>
                                <asp:TextBox ID="txtLooseItemLineNo" runat="server" Text='<%#Eval("line_no") %>' ReadOnly="true" CssClass="styleReadOnly styleReadOnlycenter fontnormal"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="製品型番" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="style2" ItemStyle-CssClass="styleReadOnly3">
                            <ItemTemplate>
                                <asp:Label ID="lbLooseItemPartNo" runat="server" Text='<%#Eval("partNo") %>' ReadOnly="true"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="製品説明" HeaderStyle-Width="40%" HeaderStyle-CssClass="style2" ItemStyle-CssClass="styleReadOnly3">
                            <ItemTemplate>
                                <asp:Label ID="lbLooseItemDesc" runat="server" Text='<%#Eval("part_Desc") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="数量" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="style2" ItemStyle-CssClass="styleReadOnly3">
                            <ItemTemplate>
                                <asp:Label ID="lbLooseItemQty" runat="server" Text='<%#Eval("QTY") %>' ReadOnly="true"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="単価（税抜）" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="style2" ItemStyle-CssClass="styleReadOnly3">
                            <ItemTemplate>
                                <asp:Label ID="lbLooseItemUnitPrice" runat="server" Text='<%#Eval("newunitPrice") %>' ReadOnly="true"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="合計（税抜）" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="style2" ItemStyle-CssClass="styleReadOnly3">
                            <ItemTemplate>
                                <asp:Label ID="lbLooseItemSubTotal" runat="server" Text='<%#Eval("newunitPrice") * Eval("QTY") %>' ReadOnly="true"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="#B6DDE8" ForeColor="Black" />
                    <PagerStyle BorderColor="Black" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="Right" colspan="2">
                <table class="w30p" cellpadding="0" cellspacing="0" style="border-collapse: collapse; margin: 15px 15px 15px 15px;">
                    <tr>
                        <td class="style3" style="width: 100px;">小計</td>
                        <td class="style3">
                            <asp:Label runat="server" ID="lbPretaxAmount"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">消費税</td>
                        <td class="style3">
                            <asp:Label runat="server" ID="lbTaxAmount"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">税込合計金額</td>
                        <td class="style4">
                            <asp:Label runat="server" ID="lbPosttaxAmount"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="styletd styletdbg" style="width: 60%; border-left: 1px solid windowtext;">備考
            </td>
        </tr>
        <tr>
            <td align="center" class="style_while_backcolor" colspan="2">
                <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="4" Width="100%" Font-Size="14px" CssClass="styleReadOnly4" />
            </td>
        </tr>
        <tr>
            <td height="10" align="center" colspan="2"></td>
        </tr>
    </table>

    <div id="divFooter">
        <style type="text/css">
            .style9 {
                height: 17.45pt;
                color: black;
                font-size: 9.0pt;
                font-weight: 400;
                font-family: メイリオ, sans-serif;
            }

            .styleReadOnly2 {
                font-family: メイリオ, sans-serif;
                text-align: left;
                vertical-align: middle;
                border-width: 0px;
                width: 825px;
            }
        </style>
        <table width="830" border="0" cellspacing="0" cellpadding="0" align="center">
            <tr>
                <td align="center">
                    <table border="0" cellpadding="0" cellspacing="0" width="830">
                        <tr>
                            <td align="left" class="style9" height="21">
                                <input readonly="readonly" value="＊一回の注文につき税抜き金額が３万円未満の場合は送料が発生致します。（送料1,000円を頂きます。）"
                                    class="styleReadOnly2" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style9" height="21">
                                <input readonly="readonly" value="＊組み立てシステムの場合、ご発注書受理以降のキャンセル/構成変更は出来ません。予めご了承ください。"
                                    class="styleReadOnly2" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style9" height="21">
                                <input readonly="readonly" value="＊検収日について、商品到着日を検収日とさせて頂きます。"
                                    class="styleReadOnly2" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style9" height="21">
                                <input readonly="readonly" value="＊前金お振込みのお客様：ご発注内容が上記と同様の場合、この見積書を弊社請求書と併用させて頂きます。"
                                    class="styleReadOnly2" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" height="21">
                                <table border="0" cellpadding="0" cellspacing="0" width="830">
                                    <tr>
                                        <td class="style9" height="21">
                                            <input readonly="readonly" value="＊振込先：シティバンク、エヌ・エイ銀行（0401）東京支店 (730)　普通口座7329383　アドバンテック(株)" class="styleReadOnly2" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style9" height="21">
                                            <input readonly="readonly" value="※なお銀行の振込み手数料は貴社にてご負担願います。" class="styleReadOnly2" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <hr style="color: Black;" />
                </td>
            </tr>
        </table>
    </div>
</div>
