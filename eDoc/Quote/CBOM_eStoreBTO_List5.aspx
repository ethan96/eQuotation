<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="CBOM_eStoreBTO_List5.aspx.vb" Inherits="EDOC.CBOM_eStoreBTO_List5" %>
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
            width: 3px;
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
            max-width: 150px;
            max-height: 150px;
        }
    </style>
    <table width="100%" border="0" align="center">
        <tr>
            <td>
                <h3>
                    <img src="../images/title-dot.gif" width="25" height="17" /><a href="./CBOM_eStoreBTO_List1.aspx">
                        <%= CATEGORY3_title%></a> >> <a href="./CBOM_eStoreBTO_List3.aspx?CATEGORY2=<%= CATEGORY2_title%>&CATEGORY3=<%= CATEGORY3_title%>">
                            ><%= CATEGORY2_title%></a> >>
                    <%= CATEGORY3_title%>
                </h3>
            </td>
        </tr>
        <tr>
            <td height="10">
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="#CCCCCC">
                    <tr>
                        <td bgcolor="#FFFFFF">
                            <asp:DataList ID="DataList1" runat="server" DataKeyField="DisplayPartno" RepeatColumns="3"
                                DataSourceID="SqlDataSource1" Width="100%" RepeatDirection="Horizontal" ItemStyle-Width="33.3%"
                                ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" CellPadding="0"
                                RepeatLayout="Flow" OnItemDataBound="DataList1_ItemDataBound">
                                <ItemTemplate>
                                    <table border="0" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="2" height="15">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="25px" width="230px" align="center">
                                                <b>
                                                    <%# Eval("DisplayPartno")%></b>
                                            </td>
                                    <%--        <td rowspan="3" class="dotlinemidial">
                                            </td>--%>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <img alt="" class="Pimg" src="<%# Eval("ImageURL") %>" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="Button1" runat="server" Text="Customize it" OnClick="Button1_Click" />
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
