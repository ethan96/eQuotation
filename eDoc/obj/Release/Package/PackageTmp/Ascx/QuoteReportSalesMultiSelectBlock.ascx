<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="QuoteReportSalesMultiSelectBlock.ascx.vb"
    Inherits="EDOC.QuoteReportSalesMultiSelectBlock" %>
<style type="text/css">
    .C td
    {
        white-space: nowrap;
    }
</style>
<asp:LinkButton runat="server" ID="lbtnChoose" Text="Pick Sales"></asp:LinkButton>
<div style="display: none; position: absolute; z-index: 9999; height: 600px; overflow: auto;
    background-color: White" id="divChoose">
    <table>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkKey" runat="server" OnClick="GetAllCheckBox(this)" Text="All"
                                Font-Bold="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBoxList runat="server" ID="cbxSales" CssClass="C">
                </asp:CheckBoxList>
            </td>
        </tr>
    </table>
</div>
<script type="text/javascript">
    function divSalesShow() { var objDiv = $("#divChoose"); $(objDiv).css("display", "block"); }
    function divSalesHide() { var objDiv = $("#divChoose"); $(objDiv).css("display", "none"); }
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SalesReady);
    function SalesReady() {
        var t;
        $("#<%=Me.lbtnChoose.ClientID %>").mouseenter(function () { if ($("#divChoose").css("display") == "none") { divSalesShow(); } });
        $("#<%=Me.lbtnChoose.ClientID %>").mouseleave(function () { t = setTimeout("divSalesHide()", 200); });
        $("#divChoose").mouseenter(function () { clearTimeout(t); });
        $("#divChoose").mouseleave(function () { if ($("#divChoose").css("display") == "block") { divSalesHide(); } });
    }
    $(function () { SalesReady(); });
    function GetAllCheckBox(cbAll) {
        var items = $("#divChoose").find("input");
        for (i = 0; i < items.length; i++) {
            if (items[i].type == "checkbox") {
                items[i].checked = cbAll.checked;
            }
        }
    }    
</script>
