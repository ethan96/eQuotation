<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="CBOM_eStoreBTO_List1.aspx.vb" Inherits="EDOC.CBOM_eStoreBTO_List1" %>
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
        .ft h1
        {
            font-size: 25px;
            margin-bottom: 1px;
            letter-spacing: 1px;
            padding-bottom: 1px;
        }
        .ft
        {
            padding: 1px;
        }
        table
        {
            border-style: none;
        }
    </style>
    <table width="100%" border="0" align="center" cellspacing="0" cellpadding="0">
        <tr>
            <td class="ft">
                <h1>
                    Show All Systems</h1>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                    <tr>
                        <td bgcolor="#FFFFFF">
                            <asp:DataList ID="DataList1" runat="server" DataKeyField="CATEGORY3" RepeatColumns="3"
                                DataSourceID="SqlDataSource1" Width="100%" RepeatDirection="Horizontal" ItemStyle-Width="33%"
                                ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" OnItemDataBound="DataList1_ItemDataBound">
                                <ItemTemplate>
                                    <table border="0" align="center">
                                        <tr>
                                            <td valign="top" width="242px" height="210px" style="background-image: url(../images/bg1.jpg);
                                                background-repeat: no-repeat; background-position: left top; padding-left: 8px;
                                                padding-top: 15px;" onmouseover="this.style.background='url(../images/bg2.jpg)';this.style.backgroundRepeat='no-repeat'"
                                                onmouseout="this.style.background='url(../images/bg1.jpg)'; this.style.backgroundRepeat='no-repeat'">
                                                <h3>
                                                    <%--<a href="./CBOM_eStoreBTO_List2.aspx?CATEGORY=<%# Eval("CATEGORY3")%>"> --%><%# Eval("CATEGORY3")%><%--</a>--%></h3>
                                                <ul class="eStoreList">
                                                    <asp:Repeater ID="Repeater1" runat="server" DataSource='<%# GetData(Eval("CATEGORY3")) %>'>
                                                        <ItemTemplate>
                                                            <li><a href="./CBOM_eStoreBTO_List3.aspx?CATEGORY2=<%# Eval("CATEGORY2")%>&CATEGORY3=<%# Eval("Parent")%>&UID=<%=Request("UID")%>">
                                                                <%# Eval("CATEGORY2")%>
                                                            </a></li>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ul>
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
