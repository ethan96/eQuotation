<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="OrderAddress.ascx.vb" Inherits="EDOC.OrderAddress" %>
<%@ Register Src="~/ASCX/Order/USAOnlineShipBillTo.ascx" TagName="ShipToUS" TagPrefix="uc1" %>
<asp:UpdatePanel ID="upShipTo" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table>
            <tr id="trerpid" runat="server">
                <td class="h5" style="width: 25%">
                    ERP ID:
                </td>
                <td>
                    <asp:TextBox runat="server" Width="145" ID="txtShipTo" AutoPostBack="true" Enabled="false"></asp:TextBox>
                    <asp:Button runat="server" Text=" Pick " ID="btnShipPick" OnClick="btnShipPick_Click" />
                </td>
            </tr>
             <tr id="trErpName" runat="server">
                <td class="h5" style="width: 25%">
                    Name:
                </td>
                <td>
                    <asp:TextBox runat="server" Width="145" ID="txtShipToName"  Enabled="false"></asp:TextBox>
                </td>
            </tr>
               <tr>
                <td class="h5" style="width: 25%">
                   Address:
                </td>
                <td>
                    <table>
                        <tr>
                            <td class="h5" colspan="4">
                                Street1:
                                <asp:TextBox runat="server" ID="txtShipToStreet" Enabled="false" Width="160" onblur="return checkdate(this,'35')"></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td class="h5" colspan="4">
                                Street2:
                                <asp:TextBox runat="server" ID="txtShipToStreet2" Enabled="false" Width="160" onblur="return checkdate(this,'35')"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="h5" style="width: 25%">
                                City
                            </td>
                            <td class="h5" style="width: 25%">
                                State
                            </td>
                            <td class="h5" style="width: 25%">
                                Zipcode
                            </td>
                            <td class="h5" style="width: 25%">
                                Country
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtShipToCity" Width="50" Enabled="false" onblur="return checkdate(this,'100')"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtShipToState" Width="50" Enabled="false" onblur="return checkdate(this,'100')"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtShipToZipcode" Width="50" Enabled="false" onblur="return checkdate(this,'100')"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtShipToCountry" Width="30" Enabled="false" onblur="return checkdate(this,'100')"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="trAttention">
                <td class="h5" style="width: 25%">
                    Attention:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtShipToAttention"  Width="184"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="h5" style="width: 25%">
                   Tel:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtShipToTel" Enabled="false" Width="184"></asp:TextBox>
                </td>
            </tr>
         
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:LinkButton runat="server" ID="lk_shipto" />
<ajaxToolkit:ModalPopupExtender runat="server" ID="MP_shipto" PopupControlID="PL_shipto"
    PopupDragHandleControlID="PL_shipto" TargetControlID="lk_shipto" BackgroundCssClass="modalBackground" />
<asp:Panel runat="server" ID="PL_shipto" BackColor="#FFFFFF" Height="80%">
    <asp:UpdatePanel runat="server" ID="up_shipto_c" UpdateMode="Conditional">
        <ContentTemplate>
         <%--   <uc1:shipto id="ucShipTo" runat="server" />--%>
            <uc1:shiptous id="ucShipToUS" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>