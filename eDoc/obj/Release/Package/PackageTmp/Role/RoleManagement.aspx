<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="RoleManagement.aspx.vb" Inherits="EDOC.RoleManagement1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
        <tr>
            <td>
                <asp:Label runat="server" ID="lbURL" Text="<%$ Resources:myRs,URL %>"></asp:Label>
                :
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtURL"></asp:TextBox>
            </td>
        </tr>
           <tr>
            <td>
                <asp:Label runat="server" ID="lbURLName" Text="<%$ Resources:myRs,URLName %>"></asp:Label>
                :
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtURLName"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lbRoleValue" Text="<%$ Resources:myRs,RoleValue %>"></asp:Label>
                :
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtRoleValue"></asp:TextBox>
                (<asp:Label runat="server" ID="lbRoleValueReq" Text="<%$ Resources:myRs,Number %>"></asp:Label>)
            </td>
        </tr>
                <tr>
            <td>
                <asp:Label runat="server" ID="Label1" Text="<%$ Resources:myRs,Seq %>"></asp:Label>
                :
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtSeq"></asp:TextBox>
                (<asp:Label runat="server" ID="Label2" Text="<%$ Resources:myRs,Number %>"></asp:Label>)
            </td>
        </tr>
             
                <tr>
            <td>
                <asp:Label runat="server" ID="Label3" Text="<%$ Resources:myRs,Class %>"></asp:Label>
                :
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtClass"></asp:TextBox>
               
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td align="center">
                <asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:myRs,Add %>" OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
    <hr />
    <asp:GridView DataKeyNames="URL" ID="GridView1" runat="server" AllowPaging="false"
        AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="URL" HeaderText="<%$ Resources:myRs,URL %>" />
             <asp:BoundField DataField="Name" HeaderText="<%$ Resources:myRs,URLName %>" />
            <asp:BoundField DataField="RoleValue" HeaderText="<%$ Resources:myRs,RoleValue %>" />
             <asp:BoundField DataField="Seq" HeaderText="<%$ Resources:myRs,Seq%>" />
              <asp:BoundField DataField="Class" HeaderText="<%$ Resources:myRs,Class%>" />
           <asp:TemplateField>
                <HeaderTemplate>
                <asp:Label runat="server" ID="lbHdEdit" Text="<%$ Resources:myRs,Edit %>"></asp:Label>
                  </HeaderTemplate>
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/edit.gif" runat="server" ID="ibtnEdit" OnClick="ibtnEdit_Click" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                <asp:Label runat="server" ID="lbHdDelete" Text="<%$ Resources:myRs,Delete %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:ImageButton ImageUrl="~/Images/del.gif" runat="server" ID="ibtnDelete" OnClick="ibtnDelete_Click" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
