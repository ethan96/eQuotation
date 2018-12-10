<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="ChartOver.aspx.vb" Inherits="EDOC.ChartOver" %>
<%@ Register Src="~/Ascx/NVCHART.ascx" TagName="NVCHART" TagPrefix="uc1" %>
<%@ Register src="~/Ascx/QuoteReportSalesMultiSelectBlock.ascx" tagname="ascxPickSales" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
        .bg0
        {
            background-color: #eee;
        }
        .bg2
        {
            background-color: #FFEACA;
            text-align: center;
        }
        td
        {
            border: solid 1px #EEEEEE;
            padding: 0px;
            padding-left: 3px;
            padding-top: 2px;
            padding-bottom: 2px;
            cursor: pointer;
        }
    </style>
    <script src="/Js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            // $('.datatable tr:even').addClass('odd'); 
            $('.datatable tr').hover(
                                       function () { $(this).addClass('highlight2'); },
                                       function () { $(this).removeClass('highlight2'); }
                                     );

            $('.datatable a').each(function () {
                var str = $.trim($(this).html());
                if (str == "0") {
                    $("<span>0</span>").insertAfter($(this));
                    $(this).hide();
                }
            });


        });  
    </script>
    <style type="text/css">
        .odd
        {
            background: #91a6cf;
        }
        .highlight
        {
            background: #F63;
        }
        .highlight2
        {
            background: #ccc;
        }
        .sc
        {
            font-size: 14px;
            font-weight: bold;
        }
        .sc a
        {
            color: #fff;
        }
        .highlight2 a
        {
            color: Red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table>
        <tr>
            <td>
                <table style="width: auto">
                    <tr>
                        <td>
                            Year:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpYear" AutoPostBack="true">
                                <%--  <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                            <asp:ListItem Text="2013" Value="2013" Selected="True"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td style="display: none;">
                            Month From:
                        </td>
                        <td style="display: none;">
                            <asp:DropDownList runat="server" ID="drpMF">
                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="display: none;">
                            Month To:
                        </td>
                        <td style="display: none;">
                            <asp:DropDownList runat="server" ID="drpMT">
                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Org:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpOrg" AutoPostBack="true" OnSelectedIndexChanged="drpOrg_SelectedIndexChanged">
                                <asp:ListItem Text="US01" Value="US01"></asp:ListItem>
                                  <asp:ListItem Text="EU10" Value="EU10"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            RBU:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpRBU" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Sales:
                        </td>
                        <td>
                             <uc1:ascxPickSales runat="server" id="ascxPickSales"></uc1:ascxPickSales>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnQuery" Text="Query" OnClick="btnQuery_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right">
                <uc1:NVCHART ID="NVCHART1" runat="server" />
            </td>
        </tr>
    </table>
    <asp:ImageButton runat="server" ID="imgXls" ImageUrl="~/Images/excel.gif" AlternateText="Download"
        OnClick="imgXls_Click" />
    <asp:Panel runat="server" ID="plC" Visible="false">
    </asp:Panel>
    <table width="100%" border="1" class="datatable">
        <tr>
            <td colspan="3" rowspan="2">
                Sales Person
            </td>
            <td colspan="4" align="center">
                <b>Q1</b>
            </td>
            <td colspan="4" align="center">
                <b>Q2</b>
            </td>
            <td colspan="4" align="center">
                <b>Q3</b>
            </td>
            <td colspan="4" align="center">
                <b>Q4</b>
            </td>
            <td align="center">
                <b>YEAR</b>
            </td>
        </tr>
        <tr>
            <td align="center">
                JAN
            </td>
            <td align="center">
                FEB
            </td>
            <td align="center">
                MAR
            </td>
            <td align="center" class="bg2">
                Q1 TOTAL
            </td>
            <td align="center">
                APR
            </td>
            <td align="center">
                MAY
            </td>
            <td align="center">
                JUN
            </td>
            <td align="center" class="bg2">
                Q2 TOTAL
            </td>
            <td align="center">
                JUL
            </td>
            <td align="center">
                AUG
            </td>
            <td align="center">
                SEPT
            </td>
            <td align="center" class="bg2">
                Q3 TOTAL
            </td>
            <td align="center">
                OCT
            </td>
            <td align="center">
                NOV
            </td>
            <td align="center">
                DEC
            </td>
            <td align="center" class="bg2">
                Q4 TOTAL
            </td>
            <td align="center">
                YTD TOTAL
            </td>
        </tr>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr class="bg<%#(Container.ItemIndex +1) mod 2 %> highlight">
                    <td rowspan="7" style="width: 200px;" class="sc" valign="middle">
                        <a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=1&MT=12&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %> ">
                            <%# Eval("Sales")%></a>
                    </td>
                    <td style="width: 80px;" align="center">
                        DISTRICT
                    </td>
                    <td>
                        # of created quotes
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 1, 1, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 2, 1, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 3, 1, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td class="bg2">
                        <%# (GetQ(Eval("Sales"), 1, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 2, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 3, 1, Eval("DISTRICT"))).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 4, 1, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 5, 1, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 6, 1, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td class="bg2">
                        <%# (GetQ(Eval("Sales"), 4, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 5, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 6, 1, Eval("DISTRICT"))).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 7, 1, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 8, 1, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 9, 1, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td class="bg2">
                        <%# (GetQ(Eval("Sales"), 7, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 8, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 9, 1, Eval("DISTRICT"))).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 10, 1, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 11, 1, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 12, 1, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td class="bg2">
                        <%# (GetQ(Eval("Sales"), 10, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 11, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 12, 1, Eval("DISTRICT"))).ToString("f0")%>
                    </td>
                    <td align="center">
                        <%# (GetQ(Eval("Sales"), 1, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 2, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 3, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 4, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 5, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 6, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 7, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 8, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 9, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 10, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 11, 1, Eval("DISTRICT")) + GetQ(Eval("Sales"), 12, 1, Eval("DISTRICT"))).ToString("f0")%>
                    </td>
                </tr>
                <tr class="bg<%#(Container.ItemIndex +1) mod 2 %>">
                    <td rowspan="6" valign="middle" align="center">
                        <%# Eval("DISTRICT")%>
                    </td>
                    <td>
                        # of converted quotes
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 1, 2, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 2, 2, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 3, 2, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td class="bg2">
                        <%# (GetQ(Eval("Sales"), 1, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 2, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 3, 2, Eval("DISTRICT"))).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 4, 2, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 5, 2, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 6, 2, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td class="bg2">
                        <%# (GetQ(Eval("Sales"), 4, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 5, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 6, 2, Eval("DISTRICT"))).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 7, 2, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 8, 2, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 9, 2, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td class="bg2">
                        <%# (GetQ(Eval("Sales"), 7, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 8, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 9, 2, Eval("DISTRICT"))).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 10, 2, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 11, 2, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 12, 2, Eval("DISTRICT")).ToString("f0")%>
                    </td>
                    <td class="bg2">
                        <%# (GetQ(Eval("Sales"), 10, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 11, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 12, 2, Eval("DISTRICT"))).ToString("f0")%>
                    </td>
                    <td align="center">
                        <%# (GetQ(Eval("Sales"), 1, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 2, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 3, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 4, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 5, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 6, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 7, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 8, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 9, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 10, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 11, 2, Eval("DISTRICT")) + GetQ(Eval("Sales"), 12, 2, Eval("DISTRICT"))).ToString("f0")%>
                    </td>
                </tr>
                <tr class="bg<%#(Container.ItemIndex +1) mod 2 %>">
                    <td>
                        Revenue of eQ quotes($)
                    </td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=1&MT=1&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 1, 3, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=2&MT=2&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 2, 3, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=3&MT=3&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 3, 3, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td class="bg2">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=1&MT=3&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 1, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 2, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 3, 3, Eval("DISTRICT"))).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=4&MT=4&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 4, 3, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=5&MT=5&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 5, 3, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=6&MT=6&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 6, 3, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td class="bg2">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=4&MT=6&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 4, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 5, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 6, 3, Eval("DISTRICT"))).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=7&MT=7&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 7, 3, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=8&MT=8&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 8, 3, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=9&MT=9&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 9, 3, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td class="bg2">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=7&MT=9&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 7, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 8, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 9, 3, Eval("DISTRICT"))).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=10&MT=10&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 10, 3, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=11&MT=11&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 11, 3, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=12&MT=12&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 12, 3, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td class="bg2">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=10&MT=12&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 10, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 11, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 12, 3, Eval("DISTRICT"))).ToString("f0")%></a></td>
                    <td align="center">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=1&MT=12&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 1, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 2, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 3, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 4, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 5, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 6, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 7, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 8, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 9, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 10, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 11, 3, Eval("DISTRICT")) + GetQ(Eval("Sales"), 12, 3, Eval("DISTRICT"))).ToString("f0")%></a></td>
                </tr>
                <tr class="bg<%#(Container.ItemIndex +1) mod 2 %>">
                    <td>
                        Revenue of eStore quotes($)
                    </td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=1&MT=1&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 1, 7, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=2&MT=2&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 2, 7, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=3&MT=3&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 3, 7, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td class="bg2">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=1&MT=3&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 1, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 2, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 3, 7, Eval("DISTRICT"))).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=4&MT=4&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 4, 7, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=5&MT=5&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 5, 7, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=6&MT=6&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 6, 7, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td class="bg2">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=4&MT=6&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 4, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 5, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 6, 7, Eval("DISTRICT"))).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=7&MT=7&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 7, 7, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=8&MT=8&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 8, 7, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=9&MT=9&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 9, 7, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td class="bg2">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=7&MT=9&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 7, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 8, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 9, 7, Eval("DISTRICT"))).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=10&MT=10&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 10, 7, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=11&MT=11&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 11, 7, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=12&MT=12&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 12, 7, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td class="bg2">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=10&MT=12&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 10, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 11, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 12, 7, Eval("DISTRICT"))).ToString("f0")%></a></td>
                    <td align="center">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=1&MT=12&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 1, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 2, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 3, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 4, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 5, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 6, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 7, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 8, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 9, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 10, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 11, 7, Eval("DISTRICT")) + GetQ(Eval("Sales"), 12, 7, Eval("DISTRICT"))).ToString("f0")%></a></td>
                </tr>
                <tr class="bg<%#(Container.ItemIndex +1) mod 2 %>">
                    <td>
                        Revenue of converted quotes($)
                    </td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=1&MT=1&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 1, 4, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=2&MT=2&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 2, 4, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=3&MT=3&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 3, 4, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td class="bg2">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=1&MT=3&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 1, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 2, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 3, 4, Eval("DISTRICT"))).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=4&MT=4&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 4, 4, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=5&MT=5&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 5, 4, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=6&MT=6&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 6, 4, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td class="bg2">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=4&MT=6&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 4, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 5, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 6, 4, Eval("DISTRICT"))).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=7&MT=7&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 7, 4, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=8&MT=8&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 8, 4, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=9&MT=9&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 9, 4, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td class="bg2">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=7&MT=9&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 7, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 8, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 9, 4, Eval("DISTRICT"))).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=10&MT=10&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 10, 4, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=11&MT=11&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 11, 4, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td>
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=12&MT=12&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# GetQ(Eval("Sales"), 12, 4, Eval("DISTRICT")).ToString("f0")%></a></td>
                    <td class="bg2">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=10&MT=12&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 10, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 11, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 12, 4, Eval("DISTRICT"))).ToString("f0")%></a></td>
                    <td align="center">
                        $<a href="chart.aspx?U=<%# Eval("Sales")%>&Y=<%= drpYear.SelectedValue %>&MF=1&MT=12&ORG=<%= drpOrg.SelectedValue %>&RBU=<%= drpRBU.SelectedValue %>"
                            target="_blank"><%# (GetQ(Eval("Sales"), 1, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 2, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 3, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 4, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 5, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 6, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 7, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 8, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 9, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 10, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 11, 4, Eval("DISTRICT")) + GetQ(Eval("Sales"), 12, 4, Eval("DISTRICT"))).ToString("f0")%></a></td>
                </tr>
                <tr class="bg<%#(Container.ItemIndex +1) mod 2 %>">
                    <td>
                        Converted quote rate (#)
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 1, 5, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 2, 5, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 3, 5, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td class="bg2">
                        <%# (GetZ(Eval("Sales"), 1, 3, 0, Eval("DISTRICT"))).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 4, 5, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 5, 5, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 6, 5, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td class="bg2">
                         <%# (GetZ(Eval("Sales"), 4, 6, 0, Eval("DISTRICT"))).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 7, 5, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 8, 5, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 9, 5, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td class="bg2">
                      <%# (GetZ(Eval("Sales"), 7, 9, 0, Eval("DISTRICT"))).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 10, 5, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 11, 5, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 12, 5, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td class="bg2">
           <%# (GetZ(Eval("Sales"), 10, 12, 0, Eval("DISTRICT"))).ToString("p0")%>
                    </td>
                    <td align="center">
                     <%# (GetZ(Eval("Sales"), 1, 12, 0, Eval("DISTRICT"))).ToString("p0")%>
                    </td>
                </tr>
                <tr class="bg<%#(Container.ItemIndex +1) mod 2 %>">
                    <td>
                        Convert revenue rate ($$)
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 1, 6, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 2, 6, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 3, 6, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td class="bg2">
                         <%# (GetZ(Eval("Sales"), 1, 3, 1, Eval("DISTRICT"))).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 4, 6, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 5, 6, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 6, 6, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td class="bg2">
                    <%# (GetZ(Eval("Sales"), 4, 6, 1, Eval("DISTRICT"))).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 7, 6, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 8, 6, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 9, 6, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td class="bg2">
                    <%# (GetZ(Eval("Sales"), 7, 9, 1, Eval("DISTRICT"))).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 10, 6, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 11, 6, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td>
                        <%# GetQ(Eval("Sales"), 12, 6, Eval("DISTRICT")).ToString("p0")%>
                    </td>
                    <td class="bg2">
                      <%# (GetZ(Eval("Sales"), 10, 12, 1, Eval("DISTRICT"))).ToString("p0")%>
                    </td>
                    <td align="center">
                        <%# (GetZ(Eval("Sales"), 1, 12, 1, Eval("DISTRICT"))).ToString("p0")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Content>
