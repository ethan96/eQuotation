<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="CBOM_LIST.aspx.vb" Inherits="EDOC.CBOM_LIST" %>
<%@ Register Src="~/ascx/CheckUID.ascx" TagName="CheckUID" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:HiddenField runat="server" ID="hd_OrgId" />
    <myASCX:CheckUID runat="server" ID="ascxCheckUID" />
    <table>
        <tr>
            <td>
                <label class="lbStyle">
                    Search:</label>
                <input id="Text1" type="text" onkeyup="filter('ContentPlaceHolder1_AdxGrid1',this.value)" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:RadioButtonList runat="server" ID="rdFilter" RepeatDirection="Horizontal" RepeatColumns="4"
                    Visible="false" AutoPostBack="true" OnSelectedIndexChanged="rdFilter_SelectedIndexChanged">
                    <asp:ListItem Value="1U (up to 3 Slots)" Selected="True">1U (up to 3 Slots)</asp:ListItem>
                    <asp:ListItem Value="2U (up to 5 Slots)">2U (up to 5 Slots)</asp:ListItem>
                    <asp:ListItem Value="4U BP Rackmount (up to 14-Slots)">4U BP Rackmount (up to 14-Slots)</asp:ListItem>
                    <asp:ListItem Value="4U BP Rackmount (up to 20-Slots)">4U BP Rackmount (up to 20-Slots)</asp:ListItem>
                    <asp:ListItem Value="4U MB Rackmount (up to 7-Slots)">4U MB Rackmount (up to 7-Slots)</asp:ListItem>
                    <asp:ListItem Value="Wallmount (up to 6 Slots)">Wallmount (up to 6 Slots)</asp:ListItem>
                    <asp:ListItem Value="Wallmount (up to 7 Slots)">Wallmount (up to 7 Slots)</asp:ListItem>
                    <asp:ListItem Value="Wallmount (up to 8 Slots)">Wallmount (up to 8 Slots)</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr valign="top">
            <td align="center">
                <asp:GridView runat="server" ID="AdxGrid1" AutoGenerateColumns="false" OnRowDataBound="AdxGrid1_RowDataBound"
                    DataKeyNames="CATALOG_NAME">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                                <asp:HiddenField runat="server" ID="hd_RowCatalogName" Value='<%#Eval("CATALOG_NAME") %>' />
                                <asp:HiddenField runat="server" ID="hd_RowImgId" Value='<%#Eval("IMAGE_ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CATALOG_NAME" HeaderText="BTO Description" ItemStyle-Wrap="false" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="CATALOG_DESC" HeaderText="Group Description" />
                        <asp:TemplateField HeaderText="QTY" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtQty" Text="1" Width="30px" />
                                <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ft3" TargetControlID="txtQty"
                                    FilterType="Numbers, Custom" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Assemble" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="btnConfig" runat="server" Text="Config" OnClick="btnConfig_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table>
                    <tr>
                        <td valign="top">
                            <asp:Panel runat="server" ID="plSBC">
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="gvDebug" Visible="false" />
<%--    <asp:Panel ID="PLBList" runat="server" Style="display: none" CssClass="modalPopup">
        <div style="text-align: right;">
            <asp:ImageButton ID="CancelButtonBList" runat="server" ImageUrl="~/Images/del.gif" />
        </div>
        <div>
            <asp:UpdatePanel ID="UPBList" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="0px" cellspacing="0px" style="width:auto">
                        <tr><td>&nbsp;</td>
                            <td style="padding:0px">
                            <div style=" width:300px;height:500px;overflow:scroll">
                            <label class="lbStyle">
                                Search:</label>
                                <asp:TextBox ID="txtFilter" runat="server" onkeyup="filter('ContentPlaceHolder1_gvBtosList',this.value)"></asp:TextBox>
                           <input id="Text2" type="text" onkeyup="filter('ContentPlaceHolder1_gvBtosList',this.value)" />
                                <asp:GridView runat="server" ID="gvBtosList" AutoGenerateColumns="true">
                                 
                                </asp:GridView>
                                <script type="text/javascript">
                                    function firstFilter() {
                                        var acNameClientId = '<%=Me.txtFilter.ClientID %>';
                                        var acName = document.getElementById(acNameClientId);
                                        //alert(acName.value)
                                        if (acName != null)
                                            filter('ContentPlaceHolder1_gvBtosList', acName.value);
                                    }
                                </script>
                            </div>
                              <asp:ListBox runat="server" ID="lsbBList"></asp:ListBox>
                            </td>
                            <td style="padding:0px">
                                <asp:Button runat="server" ID="btnConfigBList" Text=" Config " OnClick="btnConfigBList_Click" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lbDummyBList" runat="server"></asp:LinkButton>
    <ajaxToolkit:ModalPopupExtender ID="MPPickBList" runat="server" TargetControlID="lbDummyBList"
        PopupControlID="PLBList" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonBList"
        DropShadow="true">
       
        <Animations>
        <OnShown>
        <ScriptAction Script="firstFilter();" />
        </OnShown>
        </Animations>
        </ajaxToolkit:ModalPopupExtender>--%>
    <script type="text/javascript" language="javascript">
        function filter(name, q) {
            var regex = new RegExp(q, 'i');

            $('#' + name + ' tr').slice(1).each(function (i, tr) {
                tr = $(tr);
                var str = tr.text();
                if (regex.test(str)) {
                    tr.show();
                } else {
                    tr.hide();
                }
            });
        }
     
    </script>
</asp:Content>
