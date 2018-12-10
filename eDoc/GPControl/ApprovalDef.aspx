<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="ApprovalDef.aspx.vb" Inherits="EDOC.ApprovalDef" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                Approval Type:
            </td>
            <td>
                <asp:DropDownList runat="server" ID="drpType" AutoPostBack="true" OnSelectedIndexChanged="drpType_SelectedIndexChanged">
                    <asp:ListItem Text="GP" Value="GP" />
                    <asp:ListItem Text="Amonut" Value="Amount" />
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                Approver :
            </td>
            <td>
                &nbsp;<asp:TextBox runat="server" ID="TxtApprover" Width="250px" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lbCriterion" Text="GP% :"></asp:Label>
            </td>
            <td>
                &nbsp;<asp:Label runat="server" ID="lbRangeFlag" Text=">"></asp:Label><asp:TextBox
                    runat="server" ID="TxtGP" Width="25px" /><asp:Label runat="server" ID="lbPercentageSign"
                        Text="%"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Office :
            </td>
            <td>
                &nbsp;<asp:DropDownList runat="server" ID="drpOffice" 
                    DataSourceID="SqlDataSource2" DataTextField="office_name" 
                    DataValueField="Office_code">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Group :
            </td>
            <td>
                &nbsp;<asp:DropDownList runat="server" ID="drpGroup" 
                    DataSourceID="SqlDataSource3" DataTextField="group_name" 
                    DataValueField="group_code">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Status :
            </td>
            <td>
                &nbsp;<asp:DropDownList runat="server" ID="DrpStatus">
                    <asp:ListItem Text="Active" Value="1" />
                    <asp:ListItem Text="Non-Active" Value="0" />
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td align="center">
                <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text=" Add " />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                Office:
            </td>
            <td>
                <asp:DropDownList runat="server" ID="drpOfficeSelecter" AutoPostBack="True" 
                    OnSelectedIndexChanged="drpOfficeSelecter_SelectedIndexChanged" 
                    DataSourceID="SqlDataSource2" DataTextField="office_name" 
                    DataValueField="Office_code">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td>
                Search By:
            </td>
            <td>
                <asp:DropDownList ID="drpFields" runat="server">
                    <asp:ListItem Value="group_name">Group</asp:ListItem>
                    <asp:ListItem Value="Approver">Approver</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:TextBox ID="txtStr" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnSH" runat="server" Text="Search" OnClick="btnSH_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="GridView1" DataSourceID="SqlDataSource1" AllowPaging="True"
        PageIndex="0" PageSize="30" Width="100%" AutoGenerateColumns="false" DataKeyNames="ID">
        <Columns>
            <asp:BoundField DataField="Office_Name" HeaderText="Office" ReadOnly="true" />
            <asp:BoundField DataField="Group_Name" HeaderText="Group" ReadOnly="true" />
            <asp:BoundField DataField="gp_level" HeaderText="GP%" />
            <asp:BoundField DataField="approver" HeaderText="Approver" />
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <asp:DropDownList runat="server" SelectedValue='<%#Bind("active") %>' ID="active"
                        Enabled="false">
                        <asp:ListItem Value="1">Active</asp:ListItem>
                        <asp:ListItem Value="0">Non-Active</asp:ListItem>
                    </asp:DropDownList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList runat="server" SelectedValue='<%#Bind("active") %>' ID="active">
                        <asp:ListItem Value="1">Active</asp:ListItem>
                        <asp:ListItem Value="0">Non-Active</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowDeleteButton="true" />
            <asp:CommandField ShowEditButton="true" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EQ %>"
        UpdateCommand="update [GPBLOCK_LOGIC] set [approver]=@approver,[active]=@active,[Gp_level]=@gp_level
                                                                 where ID=@ID" DeleteCommand="DELETE FROM [GPBLOCK_LOGIC] WHERE ID=@ID">
        <UpdateParameters>
            <asp:Parameter Type="string" Name="approver" />
            <asp:Parameter Type="string" Name="active" />
            <asp:Parameter Type="string" Name="GP_LEVEL" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EQ %>" 
        SelectCommand="SELECT DISTINCT [Office_code], [office_name] FROM [GPBLOCK_LOGIC] WHERE ([Office_code] not like '7%')">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EQ %>" 
        SelectCommand="SELECT DISTINCT [group_code], [group_name] FROM [GPBLOCK_LOGIC] WHERE ([group_code] not like '7%')">
    </asp:SqlDataSource>
</asp:Content>
