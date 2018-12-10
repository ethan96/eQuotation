<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Wall/Wall.master" CodeBehind="Home_MyAOnlineWall.aspx.vb" Inherits="EDOC.Home_MyAOnlineWall" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">
<div id="NewOnLineWall">
    <asp:Repeater ID="rptRootMenu" runat="server" >
        <ItemTemplate>
            <div class="fileWall">
                <asp:Repeater ID="rptFirstMenu" runat="server">
                    <ItemTemplate>
                        <fieldset>
	                        <legend><%# Eval("MenuName") %></legend>
                            <asp:Repeater ID="rptSecondMenu" runat="server">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li id="secondLi" runat="server"><a href='<%# Eval("Url") %>' target="_blank"><%# Eval("MenuName") %></a></li>
                                    <asp:Repeater ID="rptThreeMenu" runat="server">
                                        <ItemTemplate>
                                            <li class="leftmargin15">
       	                                        <a href='<%# Eval("Url") %>' target="_blank"><%# Eval("MenuName") %></a>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </fieldset>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div style="clear:both"></div>
</div>
</asp:Content>
