<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="JPAOnlineQuoteTemplateV2.ascx.vb"
    Inherits="EDOC.JPAOnlineQuoteTemplateV2" %>
<div>
    <style type="text/css">
        table {
            width: 720px;
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
            font-family: メイリオ, sans-serif;
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
            font-family: メイリオ, sans-serif;
            text-align: left;
            vertical-align: middle;
            border-top: 1px solid windowtext;
            border-right: 1px solid windowtext;
            border-bottom: 1px solid windowtext;
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
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: メえイぁリえオ, sans-serif;
            text-align: left;
            vertical-align: middle;
            padding: 0px;
            border-width: 0px;
            height: 22px;
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
            font-family: メえイぁリえオ, sans-serif;
            text-align: left;
            vertical-align: middle;
            border-width: 0px;
        }

        .font6 {
            color: black;
            font-size: 10.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: メイリオ, sans-serif;
        }

        .style2 {
            color: black;
            font-size: 11.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: メイリオ, sans-serif;
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
            font-family: メイリオ, sans-serif;
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
            font-family: メイリオ, sans-serif;
        }

        .hide {
            display: none;
        }

        .fontnormal {
            font-weight: normal;
            width: 50px;
        }

        .notestd {
            background-color: #B6DDE8;
            border: 1px solid windowtext;
            font-size: 11.0pt;
            font-weight: 700;
            font-style: normal;
            text-decoration: none;
            font-family: メイリオ, sans-serif;
            text-align: center;
        }
    </style>
    <div id="divHeader">
        <table width="720" border="0" cellspacing="0" cellpadding="0" align="center">
            <tr>
                <td align="center">
                    <table width="720" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td align="left">
                                <img src='<%=Util.GetRuntimeSiteIP()%>/Images/2010-Logo-with-Slogan.jpg' alt="Advantech"
                                    width="350px" />
                            </td>
                            <td align="right">
                                <img src='<%=Util.GetRuntimeSiteIP()%>/Images/AJPCompanyInfo.png' width="300px"
                                    alt="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <table width="720" border="0" cellspacing="0" cellpadding="0" align="center">
        <tr>
            <td align="center">
                <img src='<%=Util.GetRuntimeSiteIP()%>/Images/AJPQuotationTitle.gif' alt="" />
            </td>
        </tr>
        <tr>
            <td height="20" align="center"></td>
        </tr>
        <tr>
            <td align="center">
                <table id="JPtable1" border="0" class="style1" cellpadding="0" cellspacing="0" width="720" style="border-collapse: collapse;">
                    <tr>
                        <td class="styletd styletdbg" height="24" width="120">貴社名:
                        </td>
                        <td class="styletd" width="240">
                            <asp:Label runat="server" ID="SoldToCompany" />&nbsp;
                        </td>
                        <td class="styletd styletdbg" width="120">見積書番号:
                        </td>
                        <td class="styletd" width="240">
                            <asp:TextBox ID="LabelQuoteID" runat="server" ReadOnly="true" CssClass="styleReadOnly styleReadOnlyleft" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="styletd styletdbg" height="24">貴社営業所：
                        </td>
                        <td class="styletd">
                            <input runat="server" id="CustomerOffice" class="styleReadOnly" style="width: 250px;" />
                            <%--<asp:Label runat="server" ID="CustomerOffice" />&nbsp;--%>
                        </td>
                        <td class="styletd styletdbg">発行日:
                        </td>
                        <td class="styletd">
                            <asp:TextBox ID="quoteDate" runat="server" ReadOnly="true" CssClass="styleReadOnly styleReadOnlyleft" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="styletd styletdbg" height="24">貴社担当者名：
                        </td>
                        <td class="styletd">
                            <asp:Label runat="server" ID="CustomerContact" Text="" />&nbsp;
                        </td>
                        <td class="styletd styletdbg">見積有効期限:
                        </td>
                        <td class="styletd">
                            <asp:TextBox ID="expiredDate" runat="server" ReadOnly="true" CssClass="styleReadOnly styleReadOnlyleft" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="styletd styletdbg" height="24">T E L:
                        </td>
                        <td class="styletd">
                            <asp:TextBox ID="CustomerPhone" runat="server" ReadOnly="true" CssClass="styleReadOnly styleReadOnlyleft" />&nbsp;
                        </td>
                        <td class="styletd styletdbg">営業担当：
                        </td>
                        <td class="styletd">
                            <asp:Label runat="server" ID="SaleName" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="styletd styletdbg" height="24">F A X:
                        </td>
                        <td class="styletd">
                            <input readonly="readonly" value="<%= Business.GetAJPAccountFax(QuoteId,MasterRef.DocType)%>" class="styleReadOnly styleReadOnlyleft" />&nbsp;
                        </td>
                        <td class="styletd styletdbg">作成者:
                        </td>
                        <td class="styletd">
                            <asp:Label runat="server" ID="Creator" />&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="20" align="center"></td>
        </tr>
        <tr>
            <td align="center">
                <asp:Repeater ID="RepeaterDetail" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <table id="JPtable2" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse;"
                                    width="720">
                                    <tr>
                                        <td class="style2" height="25" rowspan="2" width="120">一式型番
                                        </td>
                                        <td class="style_while_backcolor" width="260" rowspan="2" style="text-align: left; padding-left: 5px;">
                                            <input runat="server" id="btospatno" value="<%# CType(Container.DataItem, IBUS.iCartLine).VirtualPartNo.Value%>" class="styleReadOnly" style="width: 250px;" />
                                        </td>
                                        <td class="style2" width="60">数量
                                        </td>
                                        <td class="style2" width="140">仕切単価（税抜）
                                        </td>
                                        <td class="style2" width="140">見積合計（税込）
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
                                            <input runat="server" id="posttax" readonly="readonly" style="width: 130px;" value=""
                                                class="styleReadOnly styleReadOnlycenter" />
                                        </td>
                                    </tr>
                                    <tr <%# SetDisplay(CType(Container.DataItem, IBUS.iCartLine).ewFlag.Value) %>>
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
                                            <%# CType(Container.DataItem, IBUS.iCartLine).partDesc.Value%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="5" align="center"></td>
                        </tr>
                        <tr runat="server" id="btosdetail" visible="false">
                            <td>
                                <font style="font-family: 'メイリオ'; font-size: 11pt;">【詳細】</font>
                                <asp:GridView ID="gv1" runat="server" AllowPaging="false"
                                    EmptyDataText="no Item." AutoGenerateColumns="false" OnRowDataBound="gv1_RowDataBound"
                                    Font-Size="11pt" Width="720" Font-Names="メイリオ" BorderColor="Black" BorderStyle="Solid">
                                    <%-- <AlternatingRowStyle BackColor="#B6DDE8" />--%>
                                    <Columns>
                                        <%--   <asp:BoundField DataField="line_no" HeaderText="No." ItemStyle-HorizontalAlign="center"
                            Visible="false">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>--%>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="left" HeaderText="パーツ型番"  HeaderStyle-Width="30%">
                                            <ItemTemplate>
                                                <input readonly="readonly" runat="server" id="btospatno" style="font-weight: normal;" value="<%# CType(Container.DataItem, IBUS.iCartLine).partNo.Value%>"
                                                    class="styleReadOnly" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="パーツ詳細"  HeaderStyle-Width="65%">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lbdescription" Text=' <%# CType(Container.DataItem, IBUS.iCartLine).partDesc.Value%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="5%">
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
        <%--<tr id="trrd2" runat="server">
        <td align="center">
        <div style="height:90px;"></div>
            <asp:Repeater ID="RepeaterDetail2" runat="server">
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <table border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse;"
                                width="720">
                                <tr>
                                    <td class="style2" height="25" rowspan="2" width="120">
                                        一式型番
                                    </td>
                                    <td class="style_while_backcolor" width="260" rowspan="2" style="text-align: left;
                                        padding-left: 5px;">
                                        <input readonly="readonly" value="<%# Eval("partno")%>" class="styleReadOnly" style="width: 260px;" />
                                    </td>
                                    <td class="style2" width="60">
                                        数量
                                    </td>
                                    <td class="style2" width="140">
                                        仕切単価（税抜）
                                    </td>
                                    <td class="style2" width="140">
                                        見積合計（税込）
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_while_backcolor" height="40" style="text-align: center;">
                                        <input readonly="readonly" style="width: 60px;" value="<%# Eval("qty")%>" class="styleReadOnly styleReadOnlycenter" />
                                    </td>
                                    <td class="style_while_backcolor" style="text-align: center;">
                                        <input readonly="readonly" style="width: 140px;" value="¥ <%# FormatNumber(Eval("unitPrice"), 0, , , TriState.True)%>"
                                            class="styleReadOnly styleReadOnlycenter" />
                                    </td>
                                    <td class="style_while_backcolor" style="text-align: center;">
                                        <input readonly="readonly" style="width: 140px;" value="¥ <%# FormatNumber(Eval("VATinc"), 0, , TriState.True)%>"
                                            class="styleReadOnly styleReadOnlycenter" />
                                    </td>
                                </tr>
                                <tr <%# SetDisplay(Eval("EwFlag")) %>>
                                    <td class="style2" height="42">
                                    </td>
                                    <td class="style_while_backcolor" colspan="5" style="text-align: left; padding-left: 5px;
                                        background-color: #D9D9D9;">
                                        <input readonly="readonly" style="width: 600px; background-color: #d9d9d9;" value="Extended Warranty for <%# Eval("EwFlag")%> Months: ¥ <%# FormatNumber(Eval("EwFlagPrice"), 0, , , TriState.True)%>"
                                            class="styleReadOnly styleReadOnlyEW" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2" height="42">
                                        製品説明
                                    </td>
                                    <td class="style_while_backcolor" colspan="5" style="text-align: left; padding-left: 5px;">
                                        <%# Eval("description")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="5" align="center">
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </td>
    </tr>--%>

        <%--<tr runat="server" visible="true" id="trGV2">
        <td>
            <asp:GridView DataKeyNames="line_no" ID="gv2" runat="server" AllowPaging="false"
                EmptyDataText="no Item." AutoGenerateColumns="false"
                Font-Size="11pt" Width="720" Font-Names="メイリオ" BorderColor="Black" BorderStyle="Solid">
                <Columns>
                    <asp:BoundField DataField="line_no" HeaderText="No." ItemStyle-HorizontalAlign="center"
                        Visible="false">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="left" HeaderText="パーツ型番">
                        <ItemTemplate>
                            <input readonly="readonly" style="font-weight: normal;" value="<%# Eval("partNo") %>"
                                class="styleReadOnly" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="パーツ詳細">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbdescription" Text='<%#Bind("description") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="50">
                        <HeaderTemplate>
                            数量
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%#Bind("qty") %>' ID="lbGVQty" ReadOnly="true"
                                CssClass="styleReadOnly styleReadOnlycenter fontnormal"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="#B6DDE8" ForeColor="Black" />
                <PagerStyle BorderColor="Black" />
            </asp:GridView>
        </td>
    </tr>--%>
        <tr>
            <td height="20" align="center"></td>
        </tr>
        <tr>
            <td align="center">
                <table id="JPtable_notes" border="0" class="style1" cellpadding="0" cellspacing="0" width="720" style="border-collapse: collapse;">
                    <tr style="height: auto">
                        <td width="15%" class="notestd">Notes:
                        </td>
                        <td class="styletd" width="100%">
                            <asp:Literal ID="LitNotes" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="10" align="center"></td>
        </tr>
    </table>

    <div id="divFooter">
        <style type="text/css">
            .style9 {
                height: 17.45pt;
                color: black;
                font-size: 10.0pt;
                font-weight: 400;
                font-family: メイリオ, sans-serif;
            }

            .styleReadOnly2 {
                font-family: メイリオ, sans-serif;
                text-align: left;
                vertical-align: middle;
                border-width: 0px;
            }
        </style>
        <table width="720" border="0" cellspacing="0" cellpadding="0" align="center">
            <tr>
                <td align="center">
                    <table border="0" cellpadding="0" cellspacing="0" width="720">
                        <tr>
                            <td align="left" class="style9" height="21" width="720">
                                <input readonly="readonly" value="＊支払条件：" style="width: 82px;" class="styleReadOnly2" />
                                <asp:Label runat="server" ID="PaymentTerm" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style9" height="21">
                                <input readonly="readonly" value="＊納品場所：" style="width: 82px;" class="styleReadOnly2" />別途お打ち合わせ
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style9" height="21">
                                <input readonly="readonly" value="＊納期は別途ご連絡申し上げます。" style="width: 220px;" class="styleReadOnly2" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style9" height="21">
                                <input readonly="readonly" value=" ＊一回の注文につき税抜き金額が３万円未満の場合は送料が発生致します。（送料1,000円を頂きます。）"
                                    style="width: 630px;" class="styleReadOnly2" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" height="21">
                                <table border="0" cellpadding="0" cellspacing="0" width="720">
                                    <tr>
                                        <td class="style9" height="21" width="100">
                                            <input readonly="readonly" value="＊振込先：" style="width: 100px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9" width="160">
                                            <input readonly="readonly" value="みずほ銀行(0001)" style="width: 160px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9" width="20"></td>
                                        <td class="style9" width="140">
                                            <input readonly="readonly" value="芝支店(054)" style="width: 140px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9" width="20"></td>
                                        <td class="style9" width="100">
                                            <input readonly="readonly" value="当座0151806" style="width: 100px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9" width="20"></td>
                                        <td class="style9" width="160">
                                            <input readonly="readonly" value="アドバンテック株式会社" style="width: 160px;" class="styleReadOnly2" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style9" height="21"></td>
                                        <td class="style9">
                                            <input readonly="readonly" value="三菱東京ＵFＪ銀行(0005)" style="width: 160px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9"></td>
                                        <td class="style9">
                                            <input readonly="readonly" value="浜松町支店(558)" style="width: 140px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9"></td>
                                        <td class="style9">
                                            <input readonly="readonly" value="普通3767981" style="width: 100px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9"></td>
                                        <td class="style9">
                                            <input readonly="readonly" value="アドバンテック株式会社" style="width: 160px;" class="styleReadOnly2" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style9" height="21"></td>
                                        <td class="style9">
                                            <input readonly="readonly" value="三井住友銀行(0009)" style="width: 160px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9"></td>
                                        <td class="style9">
                                            <input readonly="readonly" value="丸の内支店(245)" style="width: 140px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9"></td>
                                        <td class="style9">
                                            <input readonly="readonly" value="当座0247912" style="width: 100px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9"></td>
                                        <td class="style9">
                                            <input readonly="readonly" value=" アドバンテック株式会社" style="width: 160px;" class="styleReadOnly2" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style9" height="21"></td>
                                        <td class="style9">
                                            <input readonly="readonly" value="シティバンク銀行(0401)" style="width: 160px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9"></td>
                                        <td class="style9">
                                            <input readonly="readonly" value="本店(730)" style="width: 140px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9"></td>
                                        <td class="style9">
                                            <input readonly="readonly" value="当座7329383" style="width: 100px;" class="styleReadOnly2" />
                                        </td>
                                        <td class="style9"></td>
                                        <td class="style9">
                                            <input readonly="readonly" value=" アドバンテック株式会社" style="width: 160px;" class="styleReadOnly2" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style9" height="21">
                                <input readonly="readonly" value="＊銀行の振込み手数料は貴社にてご負担願います。" style="width: 580px;"
                                    class="styleReadOnly2" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style9" height="21">
                                <input readonly="readonly" value="＊検収日について、商品到着日を検収日とさせて頂きます。" style="width: 580px;"
                                    class="styleReadOnly2" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="style9" height="21">
                                <input readonly="readonly" value="＊ご発注書受理以降のキャンセル/構成変更は出来ません。予めご了承ください。" style="width: 580px;"
                                    class="styleReadOnly2" />
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
