<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="Opportunity_Management.aspx.vb" Inherits="EDOC.Opportunity_Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <link href="../Js/JQueryEasyUI/themes/icon.css" rel="stylesheet" type="text/css" />
     <link href="../Js/JQueryEasyUI/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />
     <script src="../Js/jquery-latest.min.js" type="text/javascript"></script>
     <script src="../Js/JQueryEasyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <%-- <script src="../Js/JQueryEasyUI/datagrid-detailview.js" type="text/javascript"></script> --%>
     <script src="../Js/json2.js" type="text/javascript"></script> 
     <script src="../Js/Numeral/numeral.js" type="text/javascript"></script> 
     <style> 
        .datagrid-header .datagrid-cell-group { 
            margin: 0; 
            overflow: hidden; 
            padding: 4px 2px 4px 4px; 
            text-align: center; 
            white-space: nowrap; 
            word-wrap: normal; 
        } 
        
        span,td,th
        {
            font-family:arial,\5B8B\4F53,Arial Narrow,serif;
            font-size: 12px;
        }
                
        h1
        {
            color: #666;
            letter-spacing: -2px;
            font-weight: normal;
            font-size: 40px;
            padding-bottom: 15px;
            border-bottom: 2px solid #8fc629;
            margin-bottom: 10px;
            background-color: #fff;
        }

        h2
        {
            color: #333;
            font-size: 18px;
            font-weight: normal;
            padding: 10px;
        }
        input
        {
   
            font-family :arial,\5B8B\4F53,Arial Narrow,serif;
            font-size: 12px;
   
            color: #333333;
        }
        textarea 
        {   padding:2px 8px 0pt 3px;
            border:1px solid #CCC;
            font-size: 12px;
            color: #333333;
            }

        select 
        {
            border:1px solid #CCC;
            font-size: 12px;
            color: #333333;
            background-color: #f6f0ce;
        }
        
        select option
        {
            border:1px solid #CCC;
            font-size: 12px;
            color: #333333;
            background-color: #f6f0ce;
        }
        
        select .l1
        {
            font-size: 12px;
            background-color: #fff;
        }

        select .l2
        {
            font-size: 12px;
            background-color: #40854c;
            color: #fff;
        }
        
        body
        {
            font-family:arial,\5B8B\4F53,Arial Narrow,serif;
            font-size: 12px;
            margin: 0px;
            width: 100%;

        }
        
        td
        {
            font-family:arial,\5B8B\4F53,Arial Narrow,serif;
            font-size: 12px;
            border: solid 0px #EEEEEE;
            padding: 4px;
        }
        .gtd td
        {
            border: solid 1px #efefef;
            padding: 2px;
        }
        .ttd td
        {
            border: 0px;
            padding: 5px;
        }
        p
        {
            line-height: 20px;
        }
        a:link
        {
            color: #4e0000;
        }
        a:visited
        {
            color: #4e4444;
        }
        a:hover
        {
            color: #000000;
        }
        a:active
        {
            color: #000000;
        }
        
        </style> 
      <script type="text/javascript">
          $(function () {
              //移除myMaster的css Style
              $('#MStyle').remove();
              $('#titleTB').css("width", "100%");

              //預設頁面enter鍵為Search按鈕
              $(document).keypress(function (e) {
                  if (e.which == 13) {
                      e.preventDefault();
                      $("#btnQry").click();
                  }
              });


              var opt = $('#grid');
              opt.datagrid({
                  title: 'Opportunity Management', //標題
                  width: $(window).width() - 40, //自動寬度
                  height: $(window).height() - 200,  //固定高度
                  nowrap: false, //不截斷內文
                  striped: true,  //列背景切換
                  fitColumns: true,  //自動適應欄寬
                  singleSelect: true,  //單選列
                  remoteSort: false, //true發送命令到服務器請求排序數據，false本地自己排序
                  method: 'post',
                  //                  loader: function (param, success, error) {
                  //                      getData(param, success, error);
                  //                  },
                  idField: 'ERPID',  //主索引
                  pageSize: 20,
                  pageList: [10, 20, 30, 40, 50], //每頁顯示筆數清單
                  pagination: true, //是否啟用分頁
                  rownumbers: true, //是否顯示列數
                  remoteSort: false, //true發送命令到服務器請求排序數據，false本地自己排序
                  columns: [[
                            { field: 'action', title: '<span class="txtcenter"><b>Edit</b></span>', width: 50, align: 'center',
                                formatter: function (value, row, index) {
                                    var s = "";
                                    if (row.action == null) {
                                        s = "<button type='button' onclick='btn_edit(" + index + ")'><img alt='' src='../Images/edit.gif' style='width:20px;height:20px;' /></button>";
                                    } else {
                                        s = "<p><font color='#FF0000'>" + row.action + "</font></p>";
                                    }
                                    return s;
                                }
                            },
                            { field: 'AccountName', title: '<span class="txtcenter"><b>Account Name</b></span>', width: 150, align: 'left', sortable: true },
                            { field: 'ERPID', title: '<span class="txtcenter"><b>ERP ID</b></span>', width: 80, align: 'center', sortable: true },
                            { field: 'ProjectName', title: '<span class="txtcenter"><b>Opportunity Name</b></span>', align: 'left', width: 150, sortable: true },
                            { field: 'AMT', title: '<span class="txtcenter"><b>Amount</b></span>', width: 70, align: 'right', sortable: true,
                                formatter: function (value, row, index) {
                                    //use Numeral.js   
                                    var num = row.CURCY + numeral(value).format('0,0.00');
                                    return num;
                                }
                            },
                  //{ field: 'SalesStageID', title: '<span class="txtcenter"><b>Sales Stage ID</b></span>', width: 120, align: 'center', sortable: true },
                            {field: 'SalesStage', title: '<span class="txtcenter"><b>Sales Stage</b></span>', width: 120, align: 'center', sortable: true },
                            { field: 'DESC', title: '<span class="txtcenter"><b>Description</b></span>', width: 200, align: 'left' },
                            { field: 'PRIMARY', title: '<span class="txtcenter"><b>Primary</b></span>', width: 100, align: 'center', sortable: true },
                            { field: 'CREATED', title: '<span class="txtcenter"><b>Created Date</b></span>', width: 100, align: 'center', sortable: true,
                                formatter: function (val, row, index) {
                                    var date = new Date(val);
                                    return date.getFullYear() + "/" + formatNumber(date.getMonth() + 1) + "/" + formatNumber(date.getDate());
                                }
                            },
                            { field: 'CREATEDBY', title: '<span class="txtcenter"><b>Created By</b></span>', width: 100, align: 'center', sortable: true },
                            { field: 'RBU', title: '<span class="txtcenter"><b>RBU</b></span>', width: 50, align: 'center', sortable: true },
                            { field: 'AccountStatus', title: '<span class="txtcenter"><b>Account Status</b></span>', width: 100, align: 'center', sortable: true },
                            { field: 'AccRowID', title: '<span class="txtcenter"><b>Account Row ID</b></span>' },
                            { field: 'RowID', title: '<span class="txtcenter"><b>Row ID</b></span>' },
                            { field: 'CURCY', title: '<span class="txtcenter"><b>currency</b></span>' }
                         ]],
                  onLoadSuccess: function (data) {
                      if (data.total == 0) {
                          //沒有資料就合併儲存格
                          $('#grid').datagrid('mergeCells', {
                              index: 0,
                              field: 'action',
                              colspan: 10
                          });
                      }

                      //title置中
                      $(".txtcenter").parent().parent().css("text-align", "center");
                  }
                  //                  view: detailview, //detail子頁
                  //                  detailFormatter: function (index, row) {
                  //                      return '<div style="padding:2px"><table id="ddv-' + index + '"></table></div>';
                  //                  },
                  //                  onExpandRow: function (index, row) {
                  //                      $('#ddv-' + index).datagrid({
                  //                          method: 'post',
                  //                          loader: function (param, success, error) {
                  //                              param = { detailID: row.RowID };
                  //                              getDetailData(param, success, error);
                  //                          },
                  //                          fitColumns: true,
                  //                          singleSelect: true,
                  //                          rownumbers: true,
                  //                          height: 'auto',
                  //                          columns: [[
                  //                            { field: 'ID', title: ' Row ID', width: 50, align: 'center' },
                  //                            { field: 'Name', title: 'Project Name', width: 200 },
                  //                            { field: 'AMT', title: 'AMT', width: 50 },
                  //                            { field: 'OOP', title: 'Opportunity', width: 50 },
                  //                            { field: 'OWNER', title: 'OWNER', width: 100 },
                  //                            { field: 'action', title: '操作', width: 100, align: 'center',
                  //                                formatter: function (value, row, index) {
                  //                                    var s = "";
                  //                                    if (row.ID == null) {
                  //                                        s = "";
                  //                                    } else {
                  //                                        s = '<input id="btnEdit" type="button" value="修改" onclick="btn_edit()" style="background-color: #99CCFF"/>';
                  //                                    }
                  //                                    return s;
                  //                                }
                  //                            }
                  //                         ]],
                  //                          onResize: function () {
                  //                              $('#grid').datagrid('fixDetailRowHeight', index);
                  //                          },
                  //                          onLoadSuccess: function (data) {
                  //                              if (data.total == 0) {
                  //                                  //合併儲存格
                  //                                  $('#ddv-' + index).datagrid('mergeCells',{
                  //                                     index:0,
                  //                                     field: 'ID',
                  //                                     colspan:6
                  //                                  });
                  //                              }
                  //                              setTimeout(function () { $('#grid').datagrid('fixDetailRowHeight', index); }, 0);
                  //                          }
                  //                      });
                  //                      $('#grid').datagrid('fixDetailRowHeight', index);
                  //                  }

              });

              //隱藏欄位
              $('#grid').datagrid('hideColumn', 'RowID');
              $('#grid').datagrid('hideColumn', 'AccRowID');
              $('#grid').datagrid('hideColumn', 'CURCY');

              //ERPID下拉選單
              $('#txtERPID').combobox({
                  mode: "remote", //遠端查詢
                  loader: function (param, success, error) {
                      param = { _ERPID: $.trim($('#txtERPID').combobox('getValue')), _RBU: $.trim($('#<%= HF_RBU.ClientID %>').val()) };
                      getERPIDData(param, success, error);
                  },
                  textField: "Key",
                  valueField: "Value",
                  multiple: false, //多選
                  editable: true, //可否編輯
                  panelHeight: "auto",
                  width: 200
              });


              //AccountName下拉選單
              $('#txtAccountName').combobox({
                  mode: "remote", //遠端查詢
                  loader: function (param, success, error) {
                      param = { _AccountName: $.trim($('#txtAccountName').combobox('getValue')), _RBU: $.trim($('#<%= HF_RBU.ClientID %>').val()) };
                      getAccountNamData(param, success, error);
                  },
                  textField: "Key",
                  valueField: "Value",
                  multiple: false,
                  editable: true,
                  panelHeight: "auto",
                  width: 200
              });

              //ProjectName下拉選單
              //              $('#txtProjectName').combobox({
              //                  mode: "remote", //遠端查詢
              //                  loader: function (param, success, error) {
              //                      param = { _ProjectName: $.trim($('#txtProjectName').combobox('getValue')) };
              //                      getProjectNamData(param, success, error);
              //                  },
              //                  textField: "Key",
              //                  valueField: "Value",
              //                  multiple: false,
              //                  editable: true,
              //                  panelHeight: "auto",
              //                  width: 200
              //              });

              //AccountStatus下拉選單
              $('#txtAccountStatus').combobox({
                  loader: function (param, success, error) {
                      getAccountStatusData(param, success, error);
                  },
                  filter: function (q, row) {//大小寫都能匹配選擇
                      var opts = $(this).combobox('options');
                      return (row[opts.textField].toLowerCase().indexOf(q) >= 0 || row[opts.textField].toUpperCase().indexOf(q) >= 0);
                  },
                  textField: "Key",
                  valueField: "Value",
                  multiple: false,
                  editable: true,
                  panelHeight: "auto",
                  width: 200
//                  onSelect: function (data) {
//                      $("#btnQry").click();
//                  }
              });

              //ProjectName
              $('#txtProjectName').keydown(function (e) {
                  if (e.which == 13) {
                      e.preventDefault();
                      $("#btnQry").click();
                  }
              });

              //Sales Stage Redio button
//              $("input[name='rbtn_Stage']:radio").change(function () {
//                  $("#btnQry").click();
//              });

              $("input[name='rbtn_Stage']:radio").keydown(function (e) {
                  if (e.which == 13) {
                      e.preventDefault();
                      $("#btnQry").click();
                  }
              });
          });

          function formatNumber(value) {
              return (value < 10 ? '0' : '') + value;
          }

          function getData(param, success, error) {
              $("#btnQry").attr("disabled", true);
              var page = $('#grid').datagrid('getPager').data("pagination").options.pageNumber;
              var rows = $('.pagination-page-list :selected').val();             
              jQuery.ajax({
                  type: "POST",
                  url: '/Lab/<%=IO.Path.GetFileName(Request.PhysicalPath) %>/GetData',
                  data: JSON.stringify({ ipage: page, irows: rows, _ProjectName: $.trim($("#txtProjectName").val()), _AccountName: $.trim($("#txtAccountName").combobox('getValue')), _AccountStatus: $.trim($("#txtAccountStatus").combobox('getValue')), _ERPID: $.trim($("#txtERPID").combobox('getValue')), _RBU: $.trim($('#<%= HF_RBU.ClientID %>').val()), _Stage: $("input[name='rbtn_Stage']:checked").val() }),
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: function (data) {
                      $("#btnQry").attr("disabled", false);

                      oColumns = $.parseJSON(data.d); success(oColumns);
                  },
                  error: function (msg) {
                      $("#btnQry").attr("disabled", false);
                  }
              });
          }

          function getERPIDData(param, success, error) {
              if (param._ERPID != "") {
                  jQuery.ajax({
                      type: "POST",
                      url: '/Lab/<%=IO.Path.GetFileName(Request.PhysicalPath) %>/getERPIDData',
                      data: JSON.stringify(param),
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      success: function (data) {
                          if (data.d != "") {
                              oColumns = $.parseJSON(data.d); success(oColumns);
                          } else {
                              oColumns = ""; success(oColumns);
                          }
                      },
                      error: function (msg) {
                      }
                  });
              } else {
                  oColumns = ""; success(oColumns);
              }
          }

          function getAccountNamData(param, success, error) {
              if (param._AccountName != "") {
                  jQuery.ajax({
                      type: "POST",
                      url: '/Lab/<%=IO.Path.GetFileName(Request.PhysicalPath) %>/getAccountNamData',
                      data: JSON.stringify(param),
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      success: function (data) {
                          if (data.d != "") {
                              oColumns = $.parseJSON(data.d); success(oColumns);
                          } else {
                              oColumns = ""; success(oColumns);
                          }
                      },
                      error: function (msg) {
                      }
                  });
              } else {
                  oColumns = ""; success(oColumns);
              }
          }

//          function getProjectNamData(param, success, error) {
//              if (param._ProjectName != "") {
//                  jQuery.ajax({
//                      type: "POST",
//                      url: '/Lab/<%=IO.Path.GetFileName(Request.PhysicalPath) %>/getProjectNamData',
//                      data: JSON.stringify(param),
//                      contentType: "application/json; charset=utf-8",
//                      dataType: "json",
//                      success: function (data) {
//                          if (data.d != "") {
//                              oColumns = $.parseJSON(data.d); success(oColumns);
//                          } else {
//                              oColumns = ""; success(oColumns);
//                          }
//                      },
//                      error: function (msg) {
//                      }
//                  });
//              } else {
//                  oColumns = ""; success(oColumns);
//              }
//          }

          function getAccountStatusData(param,success,error){
           jQuery.ajax({
                  type: "POST",
                  url: '/Lab/<%=IO.Path.GetFileName(Request.PhysicalPath) %>/getAccountStatusData',
                  //data: JSON.stringify(param),
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: function (data) {
                         if (data.d !="") {
                          oColumns = $.parseJSON(data.d); success(oColumns);
                      } else {
                          oColumns = ""; success(oColumns);
                      }
                  },
                  error: function (msg) {
                  }
              });
          }
//          function getDetailData(param, success, error) {
//             
//              jQuery.ajax({
//                  type: "POST",
//                  url: '/Lab/<%=IO.Path.GetFileName(Request.PhysicalPath) %>/GetDetailData',
//                  data: JSON.stringify(param),
//                  contentType: "application/json; charset=utf-8",
//                  dataType: "json",
//                  success: function (data) {
//                          oColumns = $.parseJSON(data.d); success(oColumns);
//                  },
//                  error: function (msg) {
//                  }
//              });
//          }

          function Query() {
              $("#btnQry").attr("disabled", true);
//                $('#grid').datagrid('options').pageNumber = 1;
//                var p = $('#grid').datagrid('getPager');
//                if (p) {
//                    $(p).pagination({ pageNumber: 1 });
//                }
              //                    $("#grid").datagrid('reload');
                var opt = $('#grid');
                opt.datagrid({
                    loader: function (param, success, error) {
                        getData(param, success, error);
                    }
                });
                    $("#btnQry").attr("disabled", false);
              return false;
          }

          function btn_edit(index) {
              //先清空欄位
              $('#<%= lbERPID.ClientID %>').text('');
              $('#<%= lbAccountStatus.ClientID %>').text('');
              $('#<%= lbAccountName.ClientID %>').text('');
              $('#<%= lbProjectName.ClientID %>').text('');
              $('#<%= lbRowID.ClientID %>').text('');
              $('#<%= lbAccRowID.ClientID %>').text('');
              $('#<%= lbindex.ClientID %>').text('');
              $('#<%= lbCURCY.ClientID %>').text('');
              $('#<%= lbRBU.ClientID %>').text('');
              $('#txtDESC').val('');
              $('#txtAMT').numberbox('clear');

              //給值
              var row = $('#grid').datagrid('getData').rows[index]; //取得選取的列
              $('#<%= lbERPID.ClientID %>').text((row.ERPID == null) ? '' : row.ERPID);
              $('#<%= lbAccountStatus.ClientID %>').text((row.AccountStatus == null) ? '' : row.AccountStatus);
              $('#<%= lbAccountName.ClientID %>').text((row.AccountName==null) ? '' : row.AccountName);
              $('#<%= lbProjectName.ClientID %>').text((row.ProjectName == null) ? '' : row.ProjectName);
              $('#<%= lbRowID.ClientID %>').text((row.RowID == null) ? '' : row.RowID);
              $('#<%= lbAccRowID.ClientID %>').text((row.AccRowID == null) ? '' : row.AccRowID);
              $('#<%= lbAccountStatus.ClientID %>').text((row.AccountStatus == null) ? '' : row.AccountStatus);
              $('#<%= lbindex.ClientID %>').text(index);
              $('#<%= lbCURCY.ClientID %>').text((row.CURCY == null) ? '' : row.CURCY);
              $('#<%= lbRBU.ClientID %>').text((row.RBU == null) ? '' : row.RBU);
              $('#txtDESC').val((row.DESC == null) ? '' : row.DESC);
              $('#txtAMT').numberbox('setValue', row.AMT);
              //$('#ddl_OOP').combobox('setText',$.trim(row.SalesStage));
              $('#<%= ddl_OOP.ClientID %> option').each(function (i, item) {
                  if ($(item).text() == row.SalesStage) {
                      $(item).attr('selected', true);
                  }
              });

                var screenWidth = $(window).width() / 2 - 20, screenHeight = $(window).height() / 2 - 20;  //當前瀏覽器的寬高
                var scrolltop = $(document).scrollTop() + 10; //獲取當前窗口距離頁面頂部高度 + 10
                var scrollleft = ($(window).width() / 2) - (screenWidth / 2);
                //設定彈出視窗
                $('#PanelEdit').window({ width: screenWidth, height: screenHeight, top: scrolltop, left: scrollleft });
                $('#PanelEdit').window('open'); //顯示彈出視窗
              //return false;
            }

            function UpdateOOP() {
                var postData = JSON.stringify({ RowID: $.trim($('#<%= lbRowID.ClientID %>').text()), AMT: $.trim($('#txtAMT').numberbox('getValue')), OOP: $.trim($('#<%= ddl_OOP.ClientID %> :selected').text()), DESC: $.trim($('#txtDESC').val()) });

                $.ajax(
                {
                    type: "POST", url: "<%=IO.Path.GetFileName(Request.PhysicalPath) %>/UpdateOOP", data: postData,
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (retData) {
                        var result = $.parseJSON(retData.d);

                        if (result == "Updated") {
                            var index = $('#<%= lbindex.ClientID %>').text(); //第幾列
                            var row = $('#grid').datagrid('getData').rows[index]; //取得選取的列

                            //更新欄位
                            row.SalesStage = $.trim($('#<%= ddl_OOP.ClientID %> :selected').text());
                            //row.SalesStage = $('#ddl_OOP').combobox('getText');
                            row.AMT = $('#txtAMT').numberbox('getValue');
                            row.DESC = $('#txtDESC').val();
                            $('#grid').datagrid('refreshRow', index); //重新整理該列
                            $('#PanelEdit').window('close');
                            alert("Updated");
                        }
                        else {
                            var message = result;
                            alert(message);
                        }
                    },
                    error: function (msg) {
                        console.log("call UpdateOOP err:" + msg.d);
                    }
                });
            }
    </script>
    
    <!--多國語系-->
    <span style="font-size:18px"><script type="text/javascript" src="../Js/JQueryEasyUI/locale/easyui-lang-en.js"></script></span>
    <div>
          <p>ERP ID：<input id="txtERPID" type="text" />&nbsp;&nbsp;&nbsp;
         Account Name：<input id="txtAccountName" type="text" />&nbsp;&nbsp;&nbsp;
        Account Status：<input id="txtAccountStatus" type="text" />&nbsp;&nbsp;&nbsp;
        <input id="btnQry" type="button" value="Search" onclick="Query()" /><asp:HiddenField 
                  ID="HF_RBU" runat="server" />
          </p>
        Opportunity Name：<input id="txtProjectName" type="text" />&nbsp;&nbsp;&nbsp;
        Sales Stage:<input type="radio" id="Radio1" name="rbtn_Stage" value="0" checked="checked" /><label for="Radio1">Open</label>
                     <input type="radio" id="Radio2" name="rbtn_Stage" value="1" /><label for="Radio2">All</label>
    </div><hr />
    <table id="grid"></table>
     <div id="PanelEdit" class="easyui-window" title="Update" data-options="modal:true,closed:true,iconCls:'icon-edit',top:'10px'"style="padding: 10px;">
         <table width="100%" cellpadding="1px" cellspacing="0px" style="font-size: 14px">
           <tr>
                <th align="right">Account Name:</th>
                <td ><asp:Label runat="server" ID="lbAccountName" /></td>
                 <th align="right" style="width: 20%">Account Row ID:</th>
                <td><asp:Label runat="server" ID="lbAccRowID" />
                <asp:Label runat="server" ID="lbindex" style="display:none;" /></td>
            </tr>
            <tr>
                <th align="right" style="width: 20%">ERP ID:</th>
                <td colspan="3"><asp:Label runat="server" ID="lbERPID" /></td>
            </tr>
            <tr>
                <th align="right">Opportunity Name:</th>
                <td><asp:Label runat="server" ID="lbProjectName" /></td>
                <th align="right">Opportunity ID:</th>
                <td> <asp:Label runat="server" ID="lbRowID" /></td>
            </tr>
             <tr>
                <th align="right">RBU:</th>
                <td><asp:Label runat="server" ID="lbRBU" /></td>
                <th align="right">Account Status:</th>
                <td> <asp:Label runat="server" ID="lbAccountStatus" />
                 </td>
            </tr>
            <tr>
                 <th align="right">Amount:</th>
                <td><asp:Label runat="server" ID="lbCURCY" /><input id="txtAMT" type="text" class="easyui-numberbox" 
                        data-options="min:0,precision:2" style="width: 80px" /></td>
                <th align="right">Sales Stage:</th>
                <td>
                   <%-- <select ID="ddl_OO" class="easyui-combobox" data-options="editable: false" style="height:auto;">
                        <option Value="1-VXVAIE" selected>0% Lost</option>
                        <option Value="1-VXVAI6">5% New Lead</option>
                        <option Value="1-VXVAI7">10% Validating</option>
                        <option Value="1-VXVAI8">25% Proposing/Quoting</option>
                        <option Value="1-VXVAI9">40% Testing</option>
                        <option Value="1-VXVAIA">50% Negotiating</option>
                        <option Value="1-VXVAIB">75% Waiting for PO/Approval</option>
                        <option Value="1-VXVAIC">90% Expected Flow Business</option>
                        <option Value="1-VXVAID">100% Won-PO Input in SAP</option>
                    </select>--%>
                    <asp:DropDownList ID="ddl_OOP" runat="server">
                        <asp:ListItem Value="0% Lost" Selected="True">0% Lost</asp:ListItem>
                        <asp:ListItem Value="5% New Lead">5% New Lead</asp:ListItem>
                        <asp:ListItem Value="10% Validating">10% Validating</asp:ListItem>
                        <asp:ListItem Value="25% Proposing/Quoting">25% Proposing/Quoting</asp:ListItem>
                        <asp:ListItem Value="40% Testing">40% Testing</asp:ListItem>
                        <asp:ListItem Value="50% Negotiating">50% Negotiating</asp:ListItem>
                        <asp:ListItem Value="75% Waiting for PO/Approval">75% Waiting for PO/Approval</asp:ListItem>
                        <asp:ListItem Value="90% Expected Flow Business">90% Expected Flow Business</asp:ListItem>
                        <asp:ListItem Value="100% Won-PO Input in SAP">100% Won-PO Input in SAP</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                 <th align="right">Description:</th>
                 <td colspan="3"><textarea id="txtDESC" style="height:80px; width: 100%;"></textarea></td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: center"><asp:Button runat="server" ID="btnUpdateOOP" Text="Update" OnClientClick="UpdateOOP();return false;" /></td>
            </tr>          
          </table>
       </div>
</asp:Content>