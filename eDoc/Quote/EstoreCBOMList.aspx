<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="EstoreCBOMList.aspx.vb" Inherits="EDOC.EstoreCBOMList" %>
<%@ Register Src="~/ascx/CheckUID.ascx" TagName="CheckUID" TagPrefix="myASCX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<myASCX:CheckUID runat="server" ID="ascxCheckUID" />
    <table>
        <tr>
            <td>
                <label class="lbStyle">
                    Search:</label>
                <input id="Text1" type="text" onkeyup="filter('ContentPlaceHolder1_AdxGrid1',this.value)" />
            </td>
        </tr>
        <tr valign="top">
            <td align="left">
                <asp:GridView runat="server" ID="AdxGrid1" DataSourceID="SqlDataSource1" AutoGenerateColumns="false"  DataKeyNames="DisplayPartno" Width="700px">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                No.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>                       
                        <asp:BoundField DataField="DisplayPartno" HeaderText="Part NO" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="ProductDesc" HeaderText="Description" ItemStyle-Width="300px" HtmlEncode="false" />
                        <asp:TemplateField HeaderText="QTY" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtQty" Text="1" Width="30px" />
                                   <ajaxToolkit:FilteredTextBoxExtender runat="server" ID="ft3" TargetControlID="txtQty"
                            FilterType="Numbers, Custom" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Assemble" ItemStyle-HorizontalAlign="center" ItemStyle-Width="100px" >
                            <ItemTemplate>
                                <asp:Button ID="btnConfig" runat="server" Text="Config" OnClick="btnConfig_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                     <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:B2B %>">
                            </asp:SqlDataSource>
                </td>
                </TR><TR>
                <td align="left">
            </td>
        </tr>
    </table>
  
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
