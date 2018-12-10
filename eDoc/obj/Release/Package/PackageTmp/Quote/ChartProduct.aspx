<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="ChartProduct.aspx.vb" Inherits="EDOC.ChartProduct" %>
<%@ Register src="~/Ascx/NVCHART.ascx" tagname="NVCHART" tagprefix="uc1" %>
<%@ Register src="~/Ascx/QuoteReportSalesMultiSelectBlock.ascx" tagname="ascxPickSales" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/JQueryEasyUI/jquery.easyui.min.js"></script>
    <link href="../Js/JQueryEasyUI/themes/default/easyui.css" rel="stylesheet" />
    <link href="../Js/JQueryEasyUI/themes/icon.css" rel="stylesheet" />
    <table>
        <tr>
            <td>
                <table style="width: auto">
                    <tr>
                        <td>
                            Year:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpYear" AutoPostBack="true" OnSelectedIndexChanged="drpYear_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                            <td>
                            Month From:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpMF" OnSelectedIndexChanged="drpMF_SelectedIndexChanged" AutoPostBack="true">
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
                            Month To:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpMT" OnSelectedIndexChanged="drpMT_SelectedIndexChanged" AutoPostBack="true">
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
                            </asp:DropDownList>
                        </td>
                        <td>
                            RBU:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpRBU" AutoPostBack="true" OnSelectedIndexChanged="drpRBU_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Sales:
                        </td>
                        <td>
                            <uc1:ascxPickSales runat="server" id="ascxPickSales"></uc1:ascxPickSales>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr><td align="right"><uc1:NVCHART ID="NVCHART1" runat="server" /></td></tr>
        <tr>
            <td>  <table width="100%"><tr><td align="left">         <asp:TextBox ID="TBpartno" runat="server" onkeydown="return OnTxtPersonInfoKeyDown();"></asp:TextBox>
                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="TBpartno"
                                                                    ServicePath="~/Services/AutoComplete.asmx" ServiceMethod="GetPartNo" MinimumPrefixLength="2"
                                                                    CompletionSetCount="15">
                                                                </ajaxToolkit:AutoCompleteExtender>
                        <asp:Button ID="Button1" runat="server" Text="Search" /></td><td align="right">           <asp:ImageButton runat="server" ID="imgXls" ImageUrl="~/Images/excel.gif" AlternateText="Download"
                    OnClick="imgXls_Click" /></td></tr>
                  </table>
     
                                                
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" EmptyDataText="No raw data be found."
                    AllowPaging="true" PageIndex="0" PageSize="50" OnPageIndexChanging="GridView1_PageIndexChanging" AllowSorting="true" OnSorting="GridView1_Sorting" OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound">
                </asp:GridView>
            </td>
        </tr>
    </table>
    <div style="display: none;">
        <div id="dialogdiv">
        </div>
    </div>
    <script>
        function openWin(url)
        {
            $('#dialogdiv').dialog(
             {
                 title: 'More Detail',
                 width: 900,
                 height:400,
                 href: url,
                 closed: false,
                 cache: false,
                 modal: true,
                 buttons: [ {
                     text: 'Close',
                     handler: function () { $('#dialogdiv').dialog('close'); }
                 }]
             });
            return false;
        }
        function OnTxtPersonInfoKeyDown() {
                    var acNameClientId = '<%=Me.AutoCompleteExtender1.ClientID %>';
                    var acName = $find(acNameClientId);
                    if (acName != null)
                        acName.set_contextKey("US01");
                }
    </script>
</asp:Content>
