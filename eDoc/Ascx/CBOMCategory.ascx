<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CBOMCategory.ascx.vb" Inherits="EDOC.CBOMCategory" %>
<%@ Register TagPrefix="dbwc" Namespace="DBauer.Web.UI.WebControls" Assembly="DBauer.Web.UI.WebControls.DynamicControlsPlaceholder" %>
<asp:HiddenField runat="server" ID="hdCatValue" />
<table width="100%" style="border-style: groove">
    <tr>
        <td style="background-color: Navy">
            <asp:Button runat="server" ID="btnShowHide" Text="-" OnClick="btnShowHide_Click" />
            <asp:Label runat="server" ID="lbCatName" Font-Bold="true" ForeColor="White" />&nbsp;
            <asp:Label runat="server" ID="lbReq" Font-Bold="true" ForeColor="Tomato" Text="(Required)"
                Visible="false" />
        </td>
    </tr>
    <tr>

        <td>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional" EnableViewState="true">
                <ContentTemplate>
                    <asp:Timer runat="server" ID="TimerHandleDefaultSelect" Interval="800" Enabled="false"
                        OnTick="TimerHandleDefaultSelect_Tick" />
                    <table width="100%" runat="server" id="tb_CompList">
                        <tr>
                            <td>
                                <asp:RadioButtonList runat="server" ID="dlComp" OnSelectedIndexChanged="dlComp_SelectedIndexChanged"
                                    AutoPostBack="true" Width="99%">
                                    <asp:ListItem Text="Select..." Value="" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dbwc:DynamicControlsPlaceholder runat="server" ID="ph1" ControlsWithoutIDs="Persist"
                                    OnPostRestore="ph1_PostRestore" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="dlComp" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnShowHide" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>