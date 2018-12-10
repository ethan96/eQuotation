<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PickAccount.ascx.vb" Inherits="EDOC.PickAccount" %>
<style type="text/css">
.H 
{
    max-height:400px;
    max-width:800px;
    }
.T td
{
    padding:1px;
    margin:1px;
    text-align:left;
    }

.C td
{
   white-space:nowrap; 
    
    }
</style>
<asp:HiddenField ID="hType" runat="server" />
<ajaxToolkit:TabContainer runat="server" ID="Tabs" OnClientActiveTabChanged="ActiveTabChanged"
    ActiveTabIndex="0">
    <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Siebel Account">
        <ContentTemplate>
            <asp:Panel DefaultButton="btnSH" runat="server" ID="pldd" Width="800px">
                <table class="T">
                    <tr>
                        <td align="left" style="color: #333333">
                            Name:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtName" Width="80px" />
                        </td>
                        <td align="left" style="color: #333333">
                            ERP ID:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtID" Width="80px" />
                        </td>
                        <td align="left" style="color: #333333" runat="server" id="rbutd">
                            ORG(RBU):
                        </td>
                        <td>
                        <asp:LinkButton runat="server" ID="lbtnChoose" Text="Choose"></asp:LinkButton>      
                        <div style="display:none; position:absolute;" id="divChoose">
                    
                        <asp:CheckBoxList runat="server" ID="cblRBU" RepeatDirection="Horizontal" CssClass="C" RepeatColumns="3"
                                        BackColor="Silver">
                        </asp:CheckBoxList>
                        </div>
                        </td>
                       
                    </tr>
                    <tr>
                        <td align="left" style="color: #333333">
                            Address1:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtAddress1" Width="170px" />
                        </td>
                        <td align="left" style="color: #333333">
                            City:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtcity" Width="60px" />
                        </td>
                        <td align="left" style="color: #333333">
                            State/Province:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtProvince" Width="60px" />
                            <asp:TextBox runat="server" ID="txtState" Width="30px" Visible="False" />
                        </td>
                        
                    </tr>
                    <tr>
                        <td align="left" style="color: #333333">
                            Primary Sales:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPri" Width="170px" />
                        </td>
                        <td align="left" style="color: #333333">
                            Site:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtLocation" Width="60px" />
                        </td>
                        <td align="left" style="color: #333333">
                            Country:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCounrty" Width="60px" />
                        </td>
                        
                    </tr>
                    <tr>
                     <td align="left" style="color: #333333" runat="server">
                            Status:
                     </td>
                        <td runat="server">
                            <asp:DropDownList runat="server" ID="drpStatus" Width="200px">
                                <asp:ListItem Value="N/A">Select...</asp:ListItem>
                                <asp:ListItem Value="01-Premier Channel Partner">01-Premier Channel Partner</asp:ListItem>
                                <asp:ListItem Value="02-Gold Channel Partner">02-Gold Channel Partner</asp:ListItem>
                                <asp:ListItem Value="03-Certified Channel Partner">03-Certified Channel Partner</asp:ListItem>
                                <asp:ListItem Value="04-DMS Premier Key">04-DMS Premier Key</asp:ListItem>
                                <asp:ListItem Value="Account">Account</asp:ListItem>
                                <asp:ListItem Value="06-Key Account">06-Key Account</asp:ListItem>
                                <asp:ListItem Value="06P-Potential Key Account">06P-Potential Key Account</asp:ListItem>
                                <asp:ListItem Value="07-General Account">07-General Account</asp:ListItem>
                                <asp:ListItem Value="08-Partner's Existing ">08-Partner's Existing </asp:ListItem>
                                <asp:ListItem Value="Customer">Customer</asp:ListItem>
                                <asp:ListItem Value="09-Assigned to Partner">09-Assigned to Partner</asp:ListItem>
                                <asp:ListItem Value="10-Sales Contact">10-Sales Contact</asp:ListItem>
                                <asp:ListItem Value="11-Prospect">11-Prospect</asp:ListItem>
                                <asp:ListItem Value="12-Leads">12-Leads</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="left" style="color: #333333" runat="server">
                            Zip:
                        </td>
                        <td runat="server">
                            <asp:TextBox runat="server" ID="txtZip" Width="50px" />
                        </td>
                        <td colspan="2" runat="server">
                            <asp:Button runat="server" ID="btnSH" OnClick="btnSH_Click" Text="Search" />
                        </td>
                    </tr>
                    <tr runat="server" id="trTaxNumber1" visible="False">
                        <td runat="server">Tax Number 1</td>
                        <td runat="server"><asp:TextBox runat="server" ID="txtTaxNumber1" /></td>
                        <td runat="server"></td>
                        <td runat="server"></td>
                        <td runat="server"></td>
                        <td runat="server"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="PanelSearchResult" Height="400px" Width="800px" ScrollBars="Auto" CssClass="H"
                HorizontalAlign="Center">
                <asp:GridView DataKeyNames="ROW_ID,erpid,companyname" ID="GridView1" AllowPaging="True" PageSize="5" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label runat="server" ID="lbPick" Text="Pick"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lbtnPick" Text="Pick" OnClick="lbtnPick_Click"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Name" DataField="companyname" >
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="ERP ID" DataField="erpid" >
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="ORG(RBU)" DataField="RBU" >
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Account Status" DataField="Status" >
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Account Primary" DataField="priSales" >
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Address Information">
                            <ItemTemplate>
                                <table width="100%">
                                    <tr align="left">
                                        <td align="left" style="color: #333333; width: 34%; background-color: #EEEEEE">
                                            Country
                                        </td>
                                        <td align="left" style="color: #333333; width: 33%; background-color: #EEEEEE">
                                            city
                                        </td>
                                        <td align="left" style="color: #333333; width: 33%; background-color: #EEEEEE">
                                            Site
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <%#Eval("COUNTRY")%>
                                        </td>
                                        <td align="left">
                                            <%#Eval("city")%>
                                        </td>
                                        <td align="left">
                                            <%#Eval("location")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="color: #333333; background-color: #EEEEEE">
                                            State
                                        </td>
                                        <td align="left" style="color: #333333; background-color: #EEEEEE">
                                            Province
                                        </td>
                                        <td align="left" style="color: #333333; background-color: #EEEEEE">
                                            Zip
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <%#Eval("State")%>
                                        </td>
                                        <td align="left">
                                            <%#Eval("province")%>
                                        </td>
                                        <td align="left">
                                            <%#Eval("ZIPCODE")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="color: #333333; background-color: #EEEEEE">
                                            Address1
                                        </td>
                                        <td align="left" style="color: #333333; background-color: #EEEEEE">
                                            Address2
                                        </td>
                                        <td align="left" style="color: #333333; background-color: #EEEEEE">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <%#Eval("Address")%>
                                        </td>
                                        <td align="left">
                                            <%#Eval("Address2")%>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="350px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
          
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
   
</ajaxToolkit:TabContainer>
<asp:Label runat="server" ID="CurrentTab" />
<%--<asp:Label ID="TEST" runat="server" Text="Label" Visible="false" />--%>
<script type="text/javascript">
    function ActiveTabChanged(sender, e) {
        var CurrentTab = $get('<%=Me.CurrentTab.ClientID%>');
        //CurrentTab.innerHTML = sender.get_activeTab().get_headerText();
        //Highlight(CurrentTab);
    }

    function divRBUShow() {var objDiv = $("#divChoose");$(objDiv).css("display", "block");}
    function divRBUHide() {var objDiv = $("#divChoose");$(objDiv).css("display", "none");}

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(RBUReady); 
    function RBUReady() {var t;
        $("#<%=Me.lbtnChoose.ClientID %>").mouseenter(function () { if ($("#divChoose").css("display") == "none") { divRBUShow(); } });
        $("#<%=Me.lbtnChoose.ClientID %>").mouseleave(function () { t = setTimeout("divRBUHide()", 200); });
        $("#divChoose").mouseenter(function () { clearTimeout(t); });
        $("#divChoose").mouseleave(function () { if ($("#divChoose").css("display") == "block") { divRBUHide(); } });}
    $(function () { RBUReady(); });
        
</script>
