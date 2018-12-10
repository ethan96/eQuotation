<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="CBOM_eStoreBTO_List3.aspx.vb" Inherits="EDOC.CBOM_eStoreBTO_List3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style type="text/css">
        .eStoreList
        {
            padding: 5px 5px 5px 15px;
            list-style-image: url(../IMAGES/arrow_black2.jpg);
            vertical-align: top;
        }
        .dotlinemidial
        {
            width: 0px;
            background-image: url(../IMAGES/DOTLINE_Mid.gif);
            background-repeat: repeat-y;
        }
        .viewConfigbutton
        {
            background: url(../IMAGES/Complete196.gif) left top;
            background-repeat: no-repeat;
            display: inline-block;
            font-weight: bold;
            line-height: 22px;
            text-decoration: none;
            float: left;
            padding: 0px 0px 0px 15px;
            cursor: pointer;
            white-space: nowrap;
        }
        .viewConfigbutton span
        {
            background: url(../IMAGES/Complete196.gif) right top;
            background-repeat: no-repeat;
            float: left;
            padding: 0px 25px 0px 0px;
            display: inline-block;
        }
        .Pimg
        {
            max-width: 70px;
            max-height: 70px;
            display: block;
        }
        .tdb
        {
            border-bottom-width: 0px;
            border-bottom-style: dotted;
            border-bottom-color: #999;
        }
    </style>
    <table width="100%" border="0" align="center">
        <tr>
            <td>
                <h3>
                    <img src="../images/title-dot.gif" width="25" height="17" /><a href="./CBOM_eStoreBTO_List1.aspx">
                        <%= CATEGORY3_title%></a> >>
                    <%= CATEGORY2_title%></h3>
            </td>
        </tr>
        <tr>
            <td height="2">
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="#CCCCCC"
                    style="vertical-align: top;">
                    <tr>
                        <td bgcolor="#FFFFFF" valign="top">
                            <asp:DataList ID="DataList1" runat="server" RepeatColumns="3" DataSourceID="SqlDataSource1"
                                Width="100%" RepeatDirection="Horizontal" ItemStyle-Width="33.3%" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-VerticalAlign="Top" CellPadding="0" RepeatLayout="Table" CaptionAlign="Top">
                                <ItemTemplate>
                                    <table border="0" align="center" cellpadding="0" cellspacing="0" height="250">
                                        <tr>
                                            <td width="230px" valign="top" height="25" >
                                                <div style="overflow: hidden;height:31px;"><img src="../images/arrow_01.jpg" alt="" />
                                                <b style="color: tomato;">
                                                    <%# Eval("CATEGORY1")%></b></div>
                                            </td>
                                        <%--    <td rowspan="3" class="dotlinemidial">
                                            </td>--%>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" height="194">
                                                    <asp:Repeater ID="Repeater1" runat="server" DataSource='<%# GetData(Eval("CATEGORY1")) %>'>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="tdb">
                                                                    <img alt="" class="Pimg" src="<%# Eval("ImageURL") %>" />
                                                                    <%# Eval("DISPLAYPARTNO") %>
                                                                </td>
                                                                <td align="center" class="tdb">
                                                                    <asp:Button ID="Button1" runat="server" Text="Customize it" OnClick="Button1_Click"
                                                                        CommandArgument='<%# Eval("DISPLAYPARTNO")  %>' />
                                                                </td>
                                                            </tr>
                                                          <%--  <tr>
                                                                <td colspan="2" height="8">
                                                                </td>
                                                            </tr>--%>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="bottom" >
                                                <a href="./CBOM_eStoreBTO_List5.aspx?CATEGORY1=<%# GetURL(Eval("CATEGORY1"))%>&CATEGORY2=<%# Eval("CATEGORY2")%>&CATEGORY3=<%# Eval("CATEGORY3")%>&UID=<%=REQUEST("UID") %>"
                                                    class="viewConfigbutton"><span>View More Complete Selection Now</span></a>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:B2B %>">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
