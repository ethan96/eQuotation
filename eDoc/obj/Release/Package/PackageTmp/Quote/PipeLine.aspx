<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PipeLine.aspx.vb" Inherits="EDOC.PipeLine" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../Js/jquery-latest.min.js"></script>
    <script type="text/javascript">
        function GetAllCheckBox(cbAll) {
            var items = document.getElementsByTagName("input");
            for (i = 0; i < items.length; i++) {
                if (items[i].type == "checkbox") {
                    var id = items[i].id;
                    if (id.indexOf("CheckBox1") !=-1) {
                        items[i].checked = cbAll.checked;
                    }
                }
            }
        }
        function CheckAll() {
            var All = document.getElementById("GridView1_chkKey");
            var items = document.getElementsByTagName("input");

            var isAllchecked = true;
            for (i = 0; i < items.length; i++) {
                if (items[i].type == "checkbox") {
                    var id = items[i].id;
                    if (id.indexOf("CheckBox1") != -1) {
                        if (isAllchecked == true) {
                            isAllchecked = items[i].checked;
                        }
                    }
                }
            }
            All.checked = isAllchecked;
        }
        function GetTotal() {
            var total = 0;
            var gridView = document.getElementById("<%=GridView1.ClientID %>");
            var inputs = gridView.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox" && inputs[i].id.indexOf("CheckBox1") != -1 && inputs[i].checked) {
                    total = total + 1;
                }
                if (inputs[i].type == "text") {
                    total = total + 1;
                }

            }
            //alert(total);
        }
    </script>

    <style type="text/css" >

        .modalPopup
{
	background-color: #ffffff;
	border-width: 1px;
	border-style: solid;
	border-color: Gray;
	padding: 4px;
   

}


    </style>
</head>
<body onload="CheckAll()">
    <form id="form1" runat="server">    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <table style="width: 100%;">
                <tr>
                    <td>Sales Person:
                    </td>
                    <td>
                    
                        <asp:TextBox ID="TextBox_SalesPerson" runat="server" />
                        <asp:RequiredFieldValidator runat="server" ID="req_SalesPerson" ControlToValidate="TextBox_SalesPerson"
                            ErrorMessage="◄" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vg"></asp:RequiredFieldValidator>
                        <br />
                    </td>
                    <td>District:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_District" runat="server" />
                        <asp:RequiredFieldValidator runat="server" ID="req_District" ControlToValidate="TextBox_District"
                            ErrorMessage="◄" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vg"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Company Name:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_AccountName" runat="server" />
                        <asp:RequiredFieldValidator runat="server" ID="req_AccountName" ControlToValidate="TextBox_AccountName"
                            ErrorMessage="◄" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vg"></asp:RequiredFieldValidator>
                    </td>
                    <td>Company URL:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_AccoutURL" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Sales Group:
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList_SalesGroup" runat="server">
                            <asp:ListItem Value="-1"> - </asp:ListItem>
                            <%--                            <asp:ListItem Value="1">iSys-West</asp:ListItem>
                            <asp:ListItem Value="2">iSys-Central</asp:ListItem>
                            <asp:ListItem Value="3">iSys-East</asp:ListItem>
                            <asp:ListItem Value="4">AEG</asp:ListItem>
                            <asp:ListItem Value="5">ICG</asp:ListItem>
                            <asp:ListItem Value="6">iService</asp:ListItem>
                            <asp:ListItem Value="7">Embcore</asp:ListItem>
                            <asp:ListItem Value="8">LATAM</asp:ListItem>--%>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="req_SalesGroup" ControlToValidate="DropDownList_SalesGroup"
                            ErrorMessage="◄" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vg"
                            InitialValue="-1"></asp:RequiredFieldValidator>
                        <%--<asp:TextBox ID="TextBox_SalesGroup" runat="server" />
                        <asp:RequiredFieldValidator runat="server" ID="req_SalesGroup" ControlToValidate="TextBox_SalesGroup"
                            ErrorMessage="◄" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vg"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td>Application:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_Application" runat="server" />
                        <asp:RequiredFieldValidator runat="server" ID="req_Application" ControlToValidate="TextBox_Application"
                            ErrorMessage="◄" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vg"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Sales Stage:
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList_SalesStage" runat="server">
                            <asp:ListItem Value="-1"> - </asp:ListItem>
                            <asp:ListItem Value="0.00">0%</asp:ListItem>
                            <asp:ListItem Value="0.25">25%</asp:ListItem>
                            <asp:ListItem Value="0.50">50%</asp:ListItem>
                            <asp:ListItem Value="0.75">75%</asp:ListItem>
                            <asp:ListItem Value="1.00">100%</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="req_SalesStage" ControlToValidate="DropDownList_SalesStage"
                            ErrorMessage="◄" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vg"
                            InitialValue="-1"></asp:RequiredFieldValidator>
                    </td>
                    <td>Type of Customer:
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList_CustomerType" runat="server">
                            <asp:ListItem Value="-1"> - </asp:ListItem>
                            <asp:ListItem Value="1">SI</asp:ListItem>
                            <asp:ListItem Value="2">End User</asp:ListItem>
                            <asp:ListItem Value="3">OEM</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="req_CustomerType" ControlToValidate="DropDownList_CustomerType"
                            ErrorMessage="◄" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vg"
                            InitialValue="-1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Ship Month:
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList_ShipMonth" runat="server">
                            <asp:ListItem Value="-1"> - </asp:ListItem>
                            <asp:ListItem Value="1">Jan</asp:ListItem>
                            <asp:ListItem Value="2">Feb</asp:ListItem>
                            <asp:ListItem Value="3">Mar</asp:ListItem>
                            <asp:ListItem Value="4">Apr</asp:ListItem>
                            <asp:ListItem Value="5">May</asp:ListItem>
                            <asp:ListItem Value="6">Jun</asp:ListItem>
                            <asp:ListItem Value="7">Jul</asp:ListItem>
                            <asp:ListItem Value="8">Aug</asp:ListItem>
                            <asp:ListItem Value="9">Sept</asp:ListItem>
                            <asp:ListItem Value="10">Oct</asp:ListItem>
                            <asp:ListItem Value="11">Nov</asp:ListItem>
                            <asp:ListItem Value="12">Dec</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="req_ShipMonth" ControlToValidate="DropDownList_ShipMonth"
                            ErrorMessage="◄" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vg"
                            InitialValue="-1"></asp:RequiredFieldValidator>
                    </td>
                    <td>Ship Year:
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList_ShipYear" runat="server">
                            <asp:ListItem Value="-1"> - </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="req_ShipYear" ControlToValidate="DropDownList_ShipYear"
                            ErrorMessage="◄" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vg"
                            InitialValue="-1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Industry:
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="DropDownList_Industry" runat="server">
                            <asp:ListItem Value="-1"> - </asp:ListItem>
                            <%--<asp:ListItem>Agricultural/Farming</asp:ListItem>
                            <asp:ListItem>Aircraft/aerospace</asp:ListItem>
                            <asp:ListItem>Automotive</asp:ListItem>
                            <asp:ListItem>Building Automation</asp:ListItem>
                            <asp:ListItem>Business Applications & Infrastructure</asp:ListItem>
                            <asp:ListItem>Chemical</asp:ListItem>
                            <asp:ListItem>Consulting/Engineering/System Integrator</asp:ListItem>
                            <asp:ListItem>Digi Signage/Info Display/Narrow Casting/Streaming</asp:ListItem>
                            <asp:ListItem>Education/eLearning</asp:ListItem>
                            <asp:ListItem>Entertainment/Gaming</asp:ListItem>
                            <asp:ListItem>Factory Automation</asp:ListItem>
                            <asp:ListItem>Food/Beverage</asp:ListItem>
                            <asp:ListItem>Government/Military</asp:ListItem>
                            <asp:ListItem>Graphics/Video/Broadcast</asp:ListItem>
                            <asp:ListItem>Home Automation</asp:ListItem>
                            <asp:ListItem>Industrial Equipment Manufacturing</asp:ListItem>
                            <asp:ListItem>Internet Security</asp:ListItem>
                            <asp:ListItem>Internet Service Provider</asp:ListItem>
                            <asp:ListItem>In-vehicle Computing</asp:ListItem>
                            <asp:ListItem>KIOSK/Point of Sale Terminals</asp:ListItem>
                            <asp:ListItem>Logistics/Warehouse Management</asp:ListItem>
                            <asp:ListItem>Machine Automation</asp:ListItem>
                            <asp:ListItem>Machine Vision/Optical Inspection</asp:ListItem>
                            <asp:ListItem>Marine</asp:ListItem>
                            <asp:ListItem>Mining & Drilling</asp:ListItem>
                            <asp:ListItem>Oil & Gas</asp:ListItem>
                            <asp:ListItem>Packaging</asp:ListItem>
                            <asp:ListItem>Pharmaceutical/Medical/Healthcare</asp:ListItem>
                            <asp:ListItem>Plastics/Textiles/Fibers</asp:ListItem>
                            <asp:ListItem>Power & Energy</asp:ListItem>
                            <asp:ListItem>Process Automation & Control</asp:ListItem>
                            <asp:ListItem>Pulp/Paper</asp:ListItem>
                            <asp:ListItem>Remote Monitoring & Control</asp:ListItem>
                            <asp:ListItem>Research</asp:ListItem>
                            <asp:ListItem>Scientific</asp:ListItem>
                            <asp:ListItem>Security & Video Surveillance</asp:ListItem>
                            <asp:ListItem>Semiconductor</asp:ListItem>
                            <asp:ListItem>Telecommunications</asp:ListItem>
                            <asp:ListItem>Test/Measurement/Instrumentation</asp:ListItem>
                            <asp:ListItem>Transportation</asp:ListItem>
                            <asp:ListItem>Water/Wastewater</asp:ListItem>
                            <asp:ListItem>Water & Air Quality Control</asp:ListItem>

                            <asp:ListItem>Others</asp:ListItem>--%>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="req_Industry" ControlToValidate="DropDownList_Industry"
                            ErrorMessage="◄" ForeColor="Red" SetFocusOnError="true" Display="Dynamic" ValidationGroup="vg"
                            InitialValue="-1"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <%--<tr>
                    <td>EAU:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_EAU" runat="server" />
                        <asp:CompareValidator runat="server" Operator="DataTypeCheck" Type="Integer"
                            ControlToValidate="TextBox_EAU" ErrorMessage="Numbers only" ForeColor="Red"
                            SetFocusOnError="true" ValidationGroup="vg" Display="Dynamic" />
                    </td>
                    <td>Total Order Revenue:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox_Revenue" runat="server" />
                        <asp:CompareValidator runat="server" Operator="DataTypeCheck" Type="Currency"
                            ControlToValidate="TextBox_Revenue" ErrorMessage="Numbers only" ForeColor="Red"
                            SetFocusOnError="true" ValidationGroup="vg" Display="Dynamic" />
                    </td>
                    <td>Total Order Revenue:
                    </td>
                    <td>
                        <asp:Label ID="lb_TotalRevenue" runat="server" />
                    </td>
                </tr>--%>
                <tr>
                    <td>Notes:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="TextBox_Notes" runat="server" TextMode="MultiLine" Width="100%" Height="50px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:HiddenField runat="server" ID="hfCurrentMultishipLine" />
                        <asp:Panel ID="PLMultiShip" runat="server" Style="display: none;" CssClass="modalPopup">
                            <div style="text-align: right;">
                                <asp:ImageButton ID="CancelButtonMultiShip" runat="server" ImageUrl="~/Images/del.gif" />
                            </div>
                            <div>
                                <asp:UpdatePanel ID="UPMultiShip" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table><tr><td>Jan</td><td>Feb</td><td>Mar</td><td>Apr</td><td>May</td><td>Jun</td><td>Jul</td><td>Aug</td>
                                            <td>Sep</td><td>Oct</td>
                                            <td>Nov</td><td>Dec</td>
                                               </tr>

                                            <tr><td><asp:TextBox runat="server" ID="txtJan" Width="40px" /></td><td>
                                                <asp:TextBox runat="server" ID="txtFeb" Width="40px"/></td><td>
                                                    <asp:TextBox runat="server" ID="txtMar" Width="40px"/></td><td>
                                                        <asp:TextBox runat="server" ID="txtApr" Width="40px"/></td><td><asp:TextBox runat="server" ID="txtMay" Width="40px"/></td><td>
                                                            <asp:TextBox runat="server" ID="txtJun" Width="40px"/></td><td>
                                                                <asp:TextBox runat="server" ID="txtJul" Width="40px"/></td><td>
                                                                   <asp:TextBox runat="server" ID="txtAug" Width="40px"/> </td>
                                            <td><asp:TextBox runat="server" ID="txtSep" Width="40px"/> </td><td><asp:TextBox runat="server" ID="txtOct" Width="40px"/></td>
                                            <td><asp:TextBox runat="server" ID="txtNov" Width="40px"/></td><td><asp:TextBox runat="server" ID="txtDec" Width="40px"/></td>
                                               </tr>
                                        </table>
                                       
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                             <table style ="width:100%;"><tr><td style="text-align:center"><asp:Button runat="server" Text="Confirm" ID="btnConfirm" OnClick="btnConfirm_Click" /></td></tr></table>
                        </asp:Panel>
                        <asp:LinkButton ID="lbDummyMultiShip" runat="server" />
                        <ajaxToolkit:ModalPopupExtender ID="MPMultiShip" runat="server" TargetControlID="lbDummyMultiShip"
                            PopupControlID="PLMultiShip" BackgroundCssClass="modalBackground" CancelControlID="CancelButtonMultiShip" />
                     
                       
                        <asp:GridView Width="100%" ID="GridView1" runat="server" DataKeyNames="Line_No" AutoGenerateColumns="false">
                            <Columns>

                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkKey" runat="server" OnClick="GetAllCheckBox(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" OnClick="CheckAll()" OnCheckedChanged="CheckBox1_CheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                               

                                <asp:BoundField HeaderText="Line No." DataField="Line_No" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField HeaderText="Part No." DataField="PartNo" ItemStyle-HorizontalAlign="Left" />

                                <asp:BoundField HeaderText="Qty" DataField="Qty" ItemStyle-HorizontalAlign="Center" />

                                <asp:TemplateField HeaderText="EAU" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtBox_eau" Text='<%#Bind("Qty")%>'
                                            Style="text-align: right" Width="40px" OnTextChanged ="txtBox_eau_TextChanged"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField HeaderText="Unit Price" DataField="NewUnitPrice" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="XXX" />
                                <asp:TemplateField HeaderText="Sub Total" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Multiple Ship" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>

                                         <asp:ImageButton ImageUrl="~/Images/edit.gif" runat="server" ID="ibtnEdit" OnClick="ibtnEdit_Click"
                                      ToolTip="Edit" />
                                        <asp:CheckBox ID="cbxMultiShip" runat="server" Enabled="false"  />
                                     
                                    </ItemTemplate>

                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td height="5"></td>
                </tr>
                <tr>
                    <td colspan="4" align="right">
                        <asp:Label ID="TotalAmount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="Pipeline_Button" runat="server" Text="Send to Pipeline" OnClick="Pipeline_Button_Click" ValidationGroup="vg" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="PipelineResult" runat="server" Text="" ForeColor="Red"></asp:Label>
                        <asp:ValidationSummary
                            ID="ValidationSummary1"
                            runat="server"
                            HeaderText="Please fill in all information for further processing."
                            CssClass="validation_summary"
                            DisplayMode="BulletList"
                            ValidationGroup="vg"
                            ForeColor="Red" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
    <style>
        .validation_summary ul {
            display: none;
            visibility: hidden;
        }
    </style>
</body>
</html>

