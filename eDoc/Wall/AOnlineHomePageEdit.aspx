<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Wall/Wall.Master" CodeBehind="AOnlineHomePageEdit.aspx.vb" Inherits="EDOC.AOnlineHomePageEdit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContent" runat="server">
    <div class="left" style=" width:25%; height:300px;">
        <div class="treeContainer clear" style=" margin-left:15px;">
            <asp:TreeView ID="tvWallMenu" runat="server"  NodeWrap="True"  ImageSet="Arrows">
                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                <NodeStyle Font-Size="10pt" ForeColor="Black" HorizontalPadding="4px" NodeSpacing="0px" Width="100%"
                    VerticalPadding="0px" />
                <ParentNodeStyle Font-Bold="False" />
                <SelectedNodeStyle ForeColor="#FF8000" HorizontalPadding="0px" VerticalPadding="0px" />
            </asp:TreeView>
        </div>
    </div>
    <div class="fieldset right" style=" width:73%;">
        <div id="messageDiv" runat="server" style=" color:Red; font-size:20px; font-weight:bold; padding-left:20%;padding-top:150px;">Please Select Menu!</div>
        <fieldset id="fieldDetail" runat="server" visible="false" style=" width:70%;">
            <legend>Details</legend>
            <table>
                <tr>
                    <td width="100px">MenuID:</td>
                    <td><asp:Label ID="lblMenuId" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>MenuName:</td>
                    <td>
                        <asp:TextBox ID="txtMenuName" runat="server" Width="250"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Publish:</td>
                    <td>
                        <asp:RadioButtonList ID="rblPublish" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Y" Value="True" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="N" Value="False"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trEditUrl" runat="server">
                    <td>Url:</td>
                    <td>
                        <asp:TextBox ID="txtUrl" runat="server" Width="250"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnUpdateWallMenu" runat="server" Text="Update" onclick="btnUpdateWallMenu_Click"/></td>
                </tr>
            </table>
        </fieldset>

        <fieldset id="fieldAddMenu" runat="server" visible="false" style=" width:70%;">
            <legend>Sub Menus</legend>
            <asp:Repeater ID="rptWallMenu" runat="server">
                <HeaderTemplate>
                    <ul class="eStoreList">
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <asp:Label ID="lblMenuName" runat="server" Width="75%" ToolTip='<%# Eval("MenuName")%>' style="height:20px; line-height:30px; overflow:hidden;">
                                                    <%# Eval("MenuName")%></asp:Label>
                        <asp:Button ID="btnDelete" Text="Remove" Font-Size="Small" runat="server" OnClientClick="javascript:return confirm('Are you Sure to Delte?');"
                                CommandArgument='<%#Eval("Id") %>' onclick="btnDeleteMenu_Click" />
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <fieldset id="AddMenu" runat="server">
                <legend>Add New subMenu</legend>
                <table>
                    <tr>
                        <td width="100px">MenuName:</td>
                        <td>
                            <asp:TextBox ID="txtAddMenuName" runat="server" Width="250"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAddNewMenuName" runat="server" ErrorMessage="RequiredFieldValidator" 
                                ControlToValidate="txtAddMenuName" Text="Please Enter MenuName!" Display="Dynamic" ForeColor="Red" ValidationGroup="addMenuName"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trAddUrl" runat="server">
                        <td>Url:</td>
                        <td><asp:TextBox ID="txtAddUrl" runat="server" Width="250"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Button ID="btnAddMenuName" Text="Add" Font-Size="Small" runat="server" OnClick="btnAddMenuName_Click" ValidationGroup="addMenuName" /></td>
                    </tr>
                </table>
            </fieldset>
        </fieldset>
    </div>
</asp:Content>
