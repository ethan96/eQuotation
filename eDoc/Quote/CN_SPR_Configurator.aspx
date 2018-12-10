<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="CN_SPR_Configurator.aspx.vb" Inherits="EDOC.CN_SPR_Configurator" %>
<%@ Register Src="~/Ascx/HierarchyConfigUI.ascx" TagName="HierarchyConfigUI" TagPrefix="uc1" %>
<%@ Register TagPrefix="dbwc" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.DynamicControlsPlaceholder" %>
<%@ Register TagPrefix="obout" Namespace="OboutInc.Flyout2" Assembly="obout_Flyout2_NET" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .tree table
        {
            width: auto;
        }
        .tree td
        {
            border: 0px;
            padding: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel runat="server" ID="up1">
        <ContentTemplate>
            <asp:Timer runat="server" ID="TimerExpandAll" Interval="200" Enabled="false" OnTick="TimerExpandAll_Tick" />
            <asp:HiddenField runat="server" ID="hd_BTOItem" />
            <asp:HiddenField runat="server" ID="hd_Qty" Value="1" />
            <asp:HiddenField runat="server" ID="FlyOutTargetClientId" />
            <table width="100%">
                <tr>
                    <td>
                        <asp:LinkButton runat="server" ID="btnExpandAll" Text="Expand All" OnClick="btnExpandAll_Click" />
                        <asp:Button  runat="server" ID="btnNext1" Text="Next" OnClick="btnNext1_Click" Visible="false" />
                    </td>
                </tr>
                <tr valign="top">
                    <td align="center" style="width: 90%">
                        <obout:Flyout runat="server" ID="CatFlyOut" CloseEvent="NONE" Position="BOTTOM_CENTER">
                            <asp:Panel runat="server" ID="lbFlyPanel" Width="350px" Height="25px" BackColor="Gray"
                                ForeColor="White" Font-Bold="true">
                                <h3>
                                    Please select one component of this category</h3>
                            </asp:Panel>
                        </obout:Flyout>
                        <div class="ttd" style="text-align: left">
                            <dbwc:DynamicControlsPlaceholder runat="server" ID="ph1" ControlsWithoutIDs="Persist"
                                OnPostRestore="ph1_PostRestore" />
                        </div>
                    </td>
                    <td>
                        <ajaxToolkit:AlwaysVisibleControlExtender runat="server" ID="AlwaysVisibleControlExtender1"
                            TargetControlID="panel2" HorizontalOffset="50" VerticalOffset="200" HorizontalSide="Right" />
                        <asp:Panel runat="server" ID="panel2" BackColor="#ebebe4">
                            <asp:Button runat="server" ID="btnHide" Text="Hide/Show" OnClick="btnHide_Click"  Visible="false"/>
                         
                            <asp:Panel runat="server" ID="panel1" Width="300px" Height="300px" ScrollBars="Auto">
                                <table width="100%">
                                 <tr>
                                      <td align="left" style="color:Red;">Required项为必选, 按需勾选后请先点击下面的 “保存配置” 功能, 然后再点击弹窗下方 “確定” 按钮</td>
                                      </tr>
                                     <tr  style="margin-top:10px;"> <td align="center">
                                      <asp:Button runat="server" ID="btnNext3" Width="280" Height="30" Text="保存配置" OnClick="btnNext1_Click"  Font-Bold="True"  /></td></tr>
                                     
                                    <tr valign="top">
                                        <td>
                                            <table>
                                                <tr>
                                                    <th align="left" style="color: #333333">
                                                        Total Price:
                                                    </th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lbTotalPrice" />
                                                    </td>
                                                    <th align="left" style="color: #333333">
                                                        Max Due Date:
                                                    </th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lbMaxDueDate" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <asp:TreeView runat="server" ID="tv1" CssClass="tree" />
                                        </td>
                                    </tr>
                                   
                                </table>
                            </asp:Panel>
                            <ajaxToolkit:AlwaysVisibleControlExtender runat="server" ID="avcext2" TargetControlID="panel3"
                                HorizontalSide="Center" />
                            <asp:Panel runat="server" ID="panel3" Width="600px" ScrollBars="Auto" BackColor="LightGray"  BorderWidth="5px" BorderColor="orange" Visible="false">
                                <style type="text/css">
                                    .lhlab
                                    {
                                        line-height: 30px;
                                        padding: 8px;
                                    }
                                </style>
                                <asp:Label runat="server" ID="LbMessage" Text="test" CssClass="lhlab" />
                            </asp:Panel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <ajaxToolkit:AlwaysVisibleControlExtender ID="AlwaysVisibleControlExtender2" runat="server"
                TargetControlID="PanelDone" HorizontalSide="Center" VerticalSide="Middle"
                HorizontalOffset="1" VerticalOffset="1" />
            <asp:Panel runat="server" ID="PanelDone" Visible="false" Width="80%" Height="80%" BackColor="LightGray">
                <table>
                    <tr>
                        <td>
                            <h3>Configuration Result:</h3>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:HyperLink runat="server" ID="hyBackToSPR" Text="Back to SPR" Target="_blank" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView runat="server" ID="gvConfiguredItems" DataKeyNames="CATEGORY_TYPE" Width="95%" AutoGenerateColumns="false" OnRowDataBound="gvConfiguredItems_RowDataBound">
                                <Columns>                                    
                                    <asp:BoundField HeaderText="PartNo" DataField="CATEGORY_NAME" />
                                    <asp:BoundField HeaderText="Category Type" DataField="CATEGORY_TYPE" Visible="false"/>
                                    <asp:BoundField HeaderText="Description" DataField="PARENT_CATEGORY_ID"  />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <script type="text/javascript">
                function ShowFlyOut() { 
                    //alert('1');
                    <%=CatFlyOut.ClientID %>.Close();
                    if(document.getElementById('<%=FlyOutTargetClientId.ClientID %>').value!=''){
                        var at = document.getElementById('<%=FlyOutTargetClientId.ClientID %>').value;                        
                        <%=CatFlyOut.ClientID %>.AttachTo(at);
                        <%=CatFlyOut.ClientID %>.Open();                            
                        ScrollToElement(document.getElementById(at));                        
                        document.getElementById('<%=FlyOutTargetClientId.ClientID %>').value='';                        
                        setTimeout(function(){ShowFlyOut();},5000);   
                    }         
                    else{
                        setTimeout(function(){ShowFlyOut();},700);   
                    }            
                }

                function ScrollToElement(theElement) {
                var selectedPosX = 0;
                var selectedPosY = 0;
                while (theElement != null) {
                    selectedPosX += theElement.offsetLeft;
                    selectedPosY += theElement.offsetTop;
                    theElement = theElement.offsetParent;
                }
                window.scrollTo(selectedPosX, selectedPosY-50);
            }

                 setTimeout(function(){ShowFlyOut();},3000);
            </script>
        </ContentTemplate>   
    </asp:UpdatePanel>
</asp:Content>
