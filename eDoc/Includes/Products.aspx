<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Products.aspx.vb" Inherits="EDOC.Products" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../Js/jquery-latest.min.js" type="text/javascript"></script>
    <link href="../Js/JQueryEasyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../Js/JQueryEasyUI/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <script src="../Js/JQueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../Js/SiteUtil.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var opt = $('#MyJQGrid');
            opt.datagrid({
                width: '695', //寬度
                //height: 'auto',  //高度'auto'
                height: '360',  //高度強制指定為360，因為某些使用者反應框縮小後會擋到料號不好點選
                nowrap: true, //不截斷內文
                striped: true,  //列背景切換
                fitColumns: false,  //自動適應欄寬
                singleSelect: true,  //單選列
                remoteSort: false, //true發送命令到服務器請求排序數據，false本地自己排序
                // sortName: 'Gltrp,Vbeln,Posnr',
                // sortOrder: 'asc,asc,asc',
                multiSort: false, //多欄排序
                //queryParams: qParams,  //參數
                //loadMsg: '正在加載數據...',
                //method: 'post',
                loader: function (param, success, error) {
                    getServerData(param, success, error);
                },
                onLoadSuccess: function (data) {
                    var panel = $(this).datagrid('getPanel');
                    var tr = panel.find('div.datagrid-header tr');
                    tr.children("td").addClass("strongfont");
                },
                //url: 'WOList.aspx/LoadQueriedData',  //資料處理頁
                idField: 'Part_no',  //主索引
                //  frozenColumns: [[{ field: 'ck', checkbox: true}]], //顯示核取方塊
                // pageList: [10, 15, 20, 30, 50], //每頁顯示筆數清單
                pagination: true, //是否啟用分頁
                rownumbers: true, //是否顯示列數
                //pagePosition: 'botton', //可用值有'top','botton','both'，預設值為：'bottom'
                columns: [[

                   { field: 'Part_no', title: 'Part NO', width: 130, align: 'left', sortable: true, formatter: function (value, row, index) {
                       return "<a href='javascript:void(0);' onclick='willdo(this)'  >" + value + "</a><img src='../images/edss_showall.gif' name=" + value + " onclick='show(this)' />";
                   }
                   },
                     { field: 'model_no', title: 'Model No', width: 100, align: 'center', sortable: true },
                       { field: 'product_desc', title: 'Description', align: 'left', sortable: true, formatter: function (value, row, index) {
                           return "<span title=\"" + value + "\">" + value + "</span>";
                       }
                       },
                         { field: 'product_status', title: 'Status', width: 80, align: 'center', sortable: true }

               ]],
                onSortColumn: function (sort, order) {
                }
            });

        });
        function getServerData(param, success, error) {
            var _org = $("#<%=Horg.ClientID %>").val(); // find("option:selected").val();
            var _partNo = $("#<%=txtName.ClientID %>").val();
            var _Desc = $("#<%=txtDesc.ClientID %>").val();
            var page = $('#MyJQGrid').datagrid('getPager').data("pagination").options.pageNumber;
            var rows = $('.pagination-page-list :selected').val();
            var _btos = $("#<%=BTOS.ClientID%>").val();

            jQuery.ajax({
                type: "POST",
                url: '<%=IO.Path.GetFileName(Request.PhysicalPath) %>/GetData',
                data: JSON.stringify({ ipage: page, irows: rows, org: _org, partNo: _partNo, Desc: _Desc, Btos: _btos }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if ($.trim(data.d) != "") { 
                     oColumns = $.parseJSON(data.d);
                    //  console.log("Json" + data.d.toString());
                    success(oColumns);
                    }
                   
                }
            });
        }
        function Query() {
            $('#MyJQGrid').datagrid('options').pageNumber = 1;
            var p = $('#MyJQGrid').datagrid('getPager');
            if (p) {
                $(p).pagination({ pageNumber: 1 });
            }
            $("#MyJQGrid").datagrid('reload');
        }
    </script>
    <script>
        function willdo(_this) {
            var _partno = $(_this).text();
            var dotype = $.getUrlParam('dotype');
            switch (dotype) {
                case "1":
                    window.parent.PickPnEnd(_partno);
                    break;
                case "2":
                    window.parent.PickPnEnd2(_partno);
                    break;
            }
        }
        function show(_this) {

            var _partno = $(_this).attr("name");
         //  var url = "<%= Util.GetRuntimeSiteUrl()%>" + "/includes/ProductRelatedInfo.aspx?partno=" + _partno + "&orgid=<%=Pivot.CurrentProfile.getCurrOrg %>";
         //   $('#Myframe').attr('src', url);
            $("#divProductInfo").show();
            $("#divProductInfo").dialog({ modal: true, width: '550', height: '200', title: _partno })
            var loadingimg = "<img src='../images/LoadingRed.gif'/>"
            $('#ProductRelatedInfoFrame').html(loadingimg);
            var postData = JSON.stringify({ strPartNo: _partno, strOrgid: "<%=Pivot.CurrentProfile.getCurrOrg %>", AccountCurrency: "<%=Hcurrency.Value%>" });
            $.ajax({
                type: "POST",
                url: "<%= Util.GetRuntimeSiteUrl()%>/includes/ProductRelatedInfo.aspx/GetProductATP",
                data: postData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var retMsg = $.parseJSON(msg.d);
                    if (retMsg.ProcessStatus == true) {
                        $('#ProductRelatedInfoFrame').html(retMsg.ProcessMessage);
                    }
                    else {
                        $('#ProductRelatedInfoFrame').html('Internal Error! Please Contact <a href="mailto:MyAdvantech@advantech.com">MyAdvantech Team</a> to report this issue, thank you.');
                    }

//                    if ($.trim(msg.d) != "") {
//                        $('#ProductRelatedInfoFrame').html(msg.d);
//                    }
                },
                error: function (msg) {
                        $('#ProductRelatedInfoFrame').html('Internal Error! Please Contact <a href="mailto:MyAdvantech@advantech.com">MyAdvantech Team</a> to report this issue, thank you.');
//                    if ($.trim(msg.d) != "") {
//                        $('#ProductRelatedInfoFrame').html(msg.d);
//                    } 
                }
            }
            );
        }
        function OnTxtPersonInfoKeyDown() {
            var H = document.getElementById('<%=Me.Horg.ClientID %>');
            var acNameClientId = '<%=Me.AutoCompleteExtender1.ClientID %>';
            var acName = $find(acNameClientId);
            if (acName != null)
                acName.set_contextKey(H.value);
        }
        function keydownevent() {
            if (window.event.keyCode == 13)
                Query();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" style="height: 100%;" onkeydown="keydownevent();">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="50" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:HiddenField ID="Horg" runat="server" />
    <asp:HiddenField ID="BTOS" runat="server" />
    <asp:HiddenField ID="Hcurrency" runat="server" />
    <div id="divProductInfo" style="text-align: center; display: none;">
         <div id="ProductRelatedInfoFrame"  ></div>
    </div>
    <%--<asp:DropDownList runat="server" ID="drpOrg">
        <asp:ListItem Text="AU01" Value="AU01" />
        <asp:ListItem Text="BR01" Value="BR01" />
        <asp:ListItem Text="CN01" Value="CN01" />
        <asp:ListItem Text="JP01" Value="JP01" />
        <asp:ListItem Text="KR01" Value="KR01" />
        <asp:ListItem Text="MY01" Value="MY01" />
        <asp:ListItem Text="SG01" Value="SG01" />
        <asp:ListItem Text="TL01" Value="TL01" />
        <asp:ListItem Text="TW01" Value="TW01" />
        <asp:ListItem Text="US01" Value="US01" />
        <asp:ListItem Text="EU10" Value="EU10" />
    </asp:DropDownList>--%>
    <table width="98%">
        <tr>
            <td>
                <strong>Part No:</strong><asp:TextBox runat="server" ID="txtName" onkeydown="return OnTxtPersonInfoKeyDown();"></asp:TextBox>
                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtName"
                    ServicePath="~/Services/AutoComplete.asmx" ServiceMethod="GetPartNo" MinimumPrefixLength="2">
                </ajaxToolkit:AutoCompleteExtender>
            </td>
            <td>
                <strong>Description:</strong><asp:TextBox runat="server" ID="txtDesc"></asp:TextBox>
            </td>
            <td>
                <strong>Sales Org:</strong><asp:Label ID="LitOrgid" runat="server" ForeColor="Red"
                    Font-Bold="true"></asp:Label>
            </td>
            <td>
                <input id="Button1" type="button" value="Search" onclick="Query();" />
            </td>
        </tr>
    </table>
    <p>
        <table id="MyJQGrid">
        </table>
    </p>
    </form>
</body>
<style type="text/css">
    body
    {
        margin-left: 0px;
        margin-top: 0px;
        margin-right: 0px;
        margin-bottom: 0px;
        font-family: arial,\5B8B\4F53,Arial Narrow,serif;
        font-size: 12px;
        margin: 0px;
        width: 100%;
    }
    a:hover
    {
        color: #000000;
    }
    a:visited
    {
        color: #4e4444;
    }
    a:link
    {
        color: #4e0000;
    }
    .datagrid-cell-c1-product_desc
    {
        overflow: hidden;
        width: 345px;
    }
    .strongfont
    {
        font-weight: bold;
    }
    .datagrid-cell-c1-Part_no img
    {
        float: right;
        padding-right: 5px;
        cursor: pointer;
    }
    .datagrid-row-selected .datagrid-cell-c1-Part_no a
    {
        color: #fff;
    }
     #ProductRelatedInfoFrame   .HeaderStyle
    {
        color: white;
        background-color: #FF6600;
    }
   #ProductRelatedInfoFrame   .black
    {
        color: #000000;
        font-weight: bold;
    }
 #ProductRelatedInfoFrame     td, tbody
    {
        vertical-align: top;
    }
</style>
</html>
