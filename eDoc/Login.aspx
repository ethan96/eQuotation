<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="EDOC.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Advantech eQuotation </title>
    <style type="text/css">
        body, td, input, span {
            font-family: Arial,宋体；;
            font-size: 12px;
            margin: 0px;
        }
    </style>


    <%--Ryan 20161020 Add for Joe Neary case --%>
    <link href="Css/JqueryUI.css" rel="stylesheet" type="text/css" />
    <script src="<%=Util.GetRuntimeSiteUrl() %>/Js/jquery-latest.min.js" type="text/javascript"></script>
    <script src="<%=Util.GetRuntimeSiteUrl() %>/Js/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ShowDialogforJoe() {
            var userid = $('#txtUser').val().toLowerCase();

            //if is joe's email....    
            //if (userid == "josephn@advantech.com" || userid == "dora.wu@advantech.com.tw" || userid == "ingrid.lin@advantech.com.tw") {
            if (userid == "dora.wu@advantech.com.tw" || userid == "ingrid.lin@advantech.com.tw") {

                if (userid == "dora.wu@advantech.com.tw") {
                    $('#Joebtn_1').attr('value', 'Sales.ATW.AOL-ATC(IIoT)');
                    $('#Joebtn_2').attr('value', 'Sales.ATW.AOL-EC');
                }

                if (userid == "ingrid.lin@advantech.com.tw") {
                    $('#Joebtn_1').attr('value', 'InterCon.Embedded');
                    $('#Joebtn_2').attr('value', 'Sales.AEU');
                }


                $("#dialogMsg").html("Please select the group you want to log in with.<br /><br /> If you want to switch to another group after you logged in, please kindly log out and log in again.<br /><br />");
                $("#showDialog").dialog({
                    autoOpen: false,
                    draggable: false,
                    show: "blind",
                    hide: "blind",
                    width: 'auto',
                    height: 'auto'
                });

                $('#showDialog').parent().appendTo($("form:first"));
                $('#showDialog').dialog('open');
                return false;
            }
            else {
                return true;
            }
        }

    </script>
    <%--End Ryan 20161020--%>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 8%">
            <center>
                <table cellpadding="0" style="border: 1px solid #e0e0e0;">
                    <tr>
                        <td>
                            <img alt="ZTP online order" src="Images/loginTop.jpg" />
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url('Images/loginMid.jpg'); background-repeat: repeat-x">
                            <img alt="ZTP online order" src="Images/loginMid.jpg" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" id="tdLogin" style="padding: 20px" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lbUserName" Text="<%$ Resources:myRs,UserName %>"></asp:Label>
                                        :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox runat="server" ID="txtUser" Width="240"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="Label1" Text="<%$ Resources:myRs,Password %>"></asp:Label>
                                        :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtPass" runat="server" TextMode="Password" Width="120"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>
                                        <asp:Label runat="server" ID="Label2" Text="<%$ Resources:myRs,Language %>"></asp:Label>
                                        :
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="drpLang" runat="server">
                                            <asp:ListItem Value="en-US">English</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td></td>
                                    <td align="left">
                                        <asp:CheckBox ID="cbxKeep" runat="server" />
                                        Keep me logged in
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td width="400" align="right">
                                                    <asp:Button ID="btnLogin" runat="server" Text="<%$ Resources:myRs,Login %>" OnClientClick="return ShowDialogforJoe();" OnClick="btnLogin_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="400" align="left">
                                                    <asp:Label runat="server" ID="lbmsg" ForeColor="#FF6633" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </center>
        </div>

        <%--Ryan 20161020 Add for Joe Neary case --%>
        <div id="showDialog" style="display: none" title="">
            <div id="dialogMsg">
            </div>
            <div>
                <asp:Button ID="Joebtn_1" runat="server" Text="IAG AOnline" OnClick="btnLogin_Click" />
                <asp:Button ID="Joebtn_2" runat="server" Text="IAG KA/CP" OnClick="btnLogin_Click" />
            </div>
        </div>
        <%--End Ryan 20161020--%>
    </form>
</body>
</html>
