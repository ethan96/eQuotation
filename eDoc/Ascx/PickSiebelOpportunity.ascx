<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PickSiebelOpportunity.ascx.vb" Inherits="EDOC.PickSiebelOpportunity" %>
<br />
<asp:Label ID="Label1" runat="server" Text="Opportunity Name:" Visible="False" />
<asp:Label ID="Label_OpportunityName" runat="server" Text="" Visible="False" />
<asp:HiddenField runat= "server" ID="h_rowid" />
<asp:HiddenField runat= "server" ID="HFoptyID" />
<ajaxToolkit:TabContainer runat="server" ID="TabContainer1">
    <ajaxToolkit:TabPanel runat="server" ID="tbPickOpportunity" HeaderText="Pick Opportunity">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Panel runat="server" ID="PanelSearchOpty" DefaultButton="btnSearchOpty">
                            <b>Project Name:</b><asp:TextBox runat="server" ID="txtSearchOptyName" Width="250px" />&nbsp;<asp:Button runat="server" ID="btnSearchOpty" Text="Search" Width="60px" OnClick="btnSearchOpty_Click" />
                        
                        <span id="outMsg" style="color:tomato;  font-weight:200;"></span>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#F0F0F0">
                        &nbsp;
                        <asp:GridView DataKeyNames="ROW_ID,Name" ID="GridView1" AllowPaging="True" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" ID="lbPick" Text="Pick"></asp:Label>  
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lbtnPick" Text="Pick" OnClick="lbtnPick_Click"></asp:LinkButton> 
                                        <asp:Literal ID="LitOptyID" runat="server" Text=' <%#Eval("ROW_ID")%>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Project Name
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#Eval("Name")%>
                                       <%-- <asp:LinkButton runat="server" ID="lbtnName" Text='<%#Bind("Name")%>' ></asp:LinkButton>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Account Name" DataField="ACCOUNT_NAME" >
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Channel Partner" DataField="ChannelPartner" >
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Created" DataField="CREATED" >
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Primary" DataField="Primary" >
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddl_OOP" runat="server" Width="100">
                        <asp:ListItem Value="1-VXVAIE" Selected="True">0% Lost</asp:ListItem>
                        <asp:ListItem Value="1-VXVAI6">5% New Lead</asp:ListItem>
                        <asp:ListItem Value="1-VXVAI7">10% Validating</asp:ListItem>
                        <asp:ListItem Value="1-VXVAI8">25% Proposing/Quoting</asp:ListItem>
                        <asp:ListItem Value="1-VXVAI9">40% Testing</asp:ListItem>
                        <asp:ListItem Value="1-VXVAIA">50% Negotiating</asp:ListItem>
                        <asp:ListItem Value="1-VXVAIB">75% Waiting for PO/Approval</asp:ListItem>
                        <asp:ListItem Value="1-VXVAIC">90% Expected Flow Business</asp:ListItem>
                        <asp:ListItem Value="1-VXVAID">100% Won-PO Input in SAP</asp:ListItem>
                    </asp:DropDownList>
                                        <asp:Button ID="BTupdateOtp" runat="server" Text="Update" OnClick="btupdate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Contact" DataField="Contact" >
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    <ajaxToolkit:TabPanel runat="server" ID="tbNewOpportunity" HeaderText="New Opportunity">
        <ContentTemplate>
            <asp:Panel runat="server" ID="PanelCreate" DefaultButton="ButtonOK">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>New Project Name:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtOptyName" MaxLength="300" />
                            <asp:Label ID="LabelErrleMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Opportunity Stage:</td>
                        <td>
                            <asp:DropDownList ID="DDLOptyStage" runat="server">
                                <asp:ListItem Selected="True">25% Proposing/Quoting</asp:ListItem>
                                <asp:ListItem>50% Negotiating</asp:ListItem>
                                <asp:ListItem>75% Waiting for PO/Approval</asp:ListItem>
                                <asp:ListItem>100% Won-PO Input in SAP</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Button ID="ButtonOK" runat="server" Text="Accept" OnClick="ButtonOK_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
</ajaxToolkit:TabContainer>