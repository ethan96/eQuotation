<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NCNRAgreement.ascx.vb" Inherits="EDOC.WebUserControl1" %>
<table width="800" border="0" cellspacing="0" cellpadding="0" align="center" style="font-family: Arial, Helvetica, sans-serif">
    <tr>
        <td align="center" valign="top" style="width: 80%">
            <font style="font-size: 15px; font-weight: bold; font-family: Microsoft Calibri; color: gray">
                <br />
                <br />
                <br />
                 Advantech Corporation<br />
                    380 Fairview Way<br />
                     Milpitas, CA 95035<br />
                     408-519-3800<br />
                <br />
                <br />
              </font>
        </td>
    </tr>
    <tr>
        <td align="center" valign="top" style="width: 100%">
            <font style="font-size: 20px; font-weight: bold; font-family: Microsoft Calibri; text-decoration: underline;">
                <br />
                Non-Cancelable / Non-Returnable (NCNR) Agreement<br />
            </font>
        </td>
    </tr>
    <tr>
        <td>
            <font style="font-size: 14px; font-family: Microsoft Calibri;">
                  <br />
                  <br />
                  This agreement is to acknowledge the status of the following orders from the customer cannot be cancelled, and are not returnable for either credit or refund.<br />
                  <br />
                  Advantech's full warranty does still apply.<br />
                  <br />
             </font>
        </td>
    </tr>

    <tr>
        <td>
            <table width="100%" border="0" cellspacing="5" cellpadding="20" align="center"
                class="stylenormal">
                <tr>
                    <td align="left" width="20%" style="font-family: Calibri">Customer：
                    </td>
                    <td align="left" style="border: 1px solid #000000">
                        <asp:Label runat="server" ID="lbCustomer" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="20%" style="font-family: Calibri">Order No.：
                    </td>
                    <td align="left" style="border: 1px solid #000000">
                        <asp:Label runat="server" ID="lbOrderNo" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" width="20%" style="font-family: Calibri">Reason：
                    </td>
                    <td align="left" style="border: 1px solid #000000">
                        <asp:Label runat="server" ID="lbReason" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td align="left" style="width: 100%; border: 3px solid #000000; padding: 5px">
            <font style="font-size: 16px; font-family: Microsoft Calibri;">
                <br />
                I understand that by signing this agreement that the above orders are now classed as a NCNR (Non-Cancelable, Non-Returnable) status. The items contained in the shipments of the referenced orders cannot be returned for credit or refund, and the orders are not cancelable.
                <br />
            </font>
        </td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <td>
            <table width="100%" border="0" cellspacing="0" cellpadding="5" align="center"
                class="stylenormal">
                <tr>
                    <td>
                        <font style="font-size: 16px; font-family: Microsoft Calibri; padding-top: 10px">
                            Signed By: <br />
                            <br />
                            <br />
                        </font>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 30px">
                        <font style="font-size: 14px; font-family: Microsoft Calibri;">
                            __________________________________<br />
                            Name
                            <br />
                            <br />
                        </font>
                    </td>
                    <td style="padding-top: 30px">
                        <font style="font-size: 14px; font-family: Microsoft Calibri;">
                            __________________________________<br />
                            Signature
                            <br />
                            <br />
                        </font>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 30px">
                        <font style="font-size: 14px; font-family: Microsoft Calibri;">
                            __________________________________<br />
                            Title
                            <br />
                            <br />
                        </font>
                    </td>
                    <td style="padding-top: 30px">
                        <font style="font-size: 14px; font-family: Microsoft Calibri;">
                            __________________________________<br />
                            Date
                            <br />
                            <br />
                        </font>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
