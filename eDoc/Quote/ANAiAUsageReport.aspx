<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="ANAiAUsageReport.aspx.vb" Inherits="EDOC.ANAiAUsageReport" %>

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
                        <td>From:<br />
                            
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFromDate" Width="80" />
                            <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Filteredtextboxextender5"
                                TargetControlID="txtFromDate" FilterType="Numbers, Custom" ValidChars="-/" />
                            <ajaxToolkit:CalendarExtender runat="server" ID="CalendarExtender1" CssClass="cal_Theme1"
                                TargetControlID="txtFromDate" Format="yyyy-MM-dd" />

                        </td>
                        <td>To:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtToDate" Width="80" />
                            <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="Filteredtextboxextender1"
                                TargetControlID="txtToDate" FilterType="Numbers, Custom" ValidChars="-/" />
                            <ajaxToolkit:CalendarExtender runat="server" ID="CalendarExtender2" CssClass="cal_Theme1"
                                TargetControlID="txtToDate" Format="yyyy-MM-dd" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnQuery" Text="Query" OnClick="btnQuery_Click" />
                        </td>
                    </tr>
                    <tr><td colspan="4">Date Format:yyyy-MM-dd(2015-01-01)</td></tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:ImageButton runat="server" ID="imgXls" ImageUrl="~/Images/excel.gif" AlternateText="Download"
        OnClick="imgXls_Click" />
    <asp:GridView ID="GV1" runat="server"></asp:GridView>

</asp:Content>
