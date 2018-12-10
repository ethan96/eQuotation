<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CreateNewCust.aspx.vb" Inherits="EDOC.CreateNewCust" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>Customer Account Group:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="drpCustAccGroup">
                            <asp:ListItem Text="Sold To" Value="Z001"></asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Customer Number:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCustID"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="txtCustID" ErrorMessage="Invalid customer id."></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Sales Organization:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtSalesOrg" Width="50px" Text="US01"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Distribution Channel:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="drpDistChannel"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Division:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="drpDivision"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Customer Name:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtName" Width="265px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
            ControlToValidate="txtName" ErrorMessage="Invalid customer name."></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Street:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtStreet" Width="443px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
            ControlToValidate="txtStreet" ErrorMessage="Invalid street."></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>City:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCity"></asp:TextBox></td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
            ControlToValidate="txtCity" ErrorMessage="Invalid city."></asp:RequiredFieldValidator>
                </tr>
                <tr>
                    <td>Region (State, Province, County):</td>
                    <td>
                        <asp:DropDownList runat="server" ID="drpRegion" onchange="reCTaxJuri();"></asp:DropDownList>
                      </td>
                </tr>
                <tr>
                    <td>City postal code:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPostalCode" onkeyup="reCTaxJuri();"></asp:TextBox>
                        <script type="text/javascript" >
                            function reCTaxJuri() {
                                var txtRegCode = document.getElementById("<%=Me.drpRegion.ClientID%>")
                                var vRegCode = txtRegCode.options[txtRegCode.selectedIndex].value;
                                var txtZipCode = document.getElementById("<%=Me.txtPostalCode.ClientID%>")
                                var vZipCode = txtZipCode.value;

                                var obj = document.getElementById("<%=Me.txtJurisdiction.ClientID%>")
                                obj.value = vRegCode + vZipCode
                            }

                        </script>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
            ControlToValidate="txtPostalCode" ErrorMessage="Invalid postal code."></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Country Key:</td>
                    <td>
                         <asp:DropDownList runat="server" ID="drpCountry"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Tax Jurisdiction:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtJurisdiction"></asp:TextBox>
                        
                    </td>
                </tr>
                <tr>
                    <td>First telephone no.:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPhone" Width="265px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>E-Mail Address:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEmail" Width="265px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Sales District:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="drpSalesDistrict"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Sales Office:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="drpSalesOffice"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Sales Group:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="drpSalesGroup"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Currency:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCurrency" Text="USD" Width="30px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Shipping Conditions:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="drpShipCondition"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Incoterms (Part 1):</td>
                    <td>
                        <asp:DropDownList runat="server" ID="drpIncoterm"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Incoterms (Part 2) / Ship Via:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtIncoterms2"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Terms of Payment Key:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="drpPaymentTerm"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Is Tax Exempt:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="drpTaxExempt">
                            <asp:ListItem Value="1">No</asp:ListItem>
                            <asp:ListItem Value="0">Yes</asp:ListItem>
                            
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>VAT Registration Number:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtVAT" Width="265px"></asp:TextBox></td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnSubmit" Text="Create" /></td>
                    <td>
                        <asp:Label runat="server" ID="lbMsg"></asp:Label></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
