<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestAPI.aspx.cs" Inherits="Ming.Test.TestAPI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        <hr />
        <asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="125px"></asp:DetailsView>
        <hr />
        Model header:
       <asp:GridView ID="GridView2" runat="server" />
        <hr />
        Model default features:
       <asp:GridView ID="GridView3" runat="server" />
        <hr />
        Model features in Simplified Chinese:
       <asp:GridView ID="GVFeature" runat="server" />
        <hr />
        Model's main picture
       <asp:GridView ID="GVMainPIC" runat="server">
           <Columns>
               <asp:ImageField DataImageUrlField="HTTP_URL"></asp:ImageField>
           </Columns>
       </asp:GridView>
        <hr />
        Model's big picture
       <asp:GridView ID="GVBigPIC" runat="server">
           <Columns>
               <asp:ImageField DataImageUrlField="HTTP_URL"></asp:ImageField>
           </Columns>
       </asp:GridView>
        <hr />
        Model's small picture
       <asp:GridView ID="GVSmallPIC" runat="server">
           <Columns>
               <asp:ImageField DataImageUrlField="HTTP_URL"></asp:ImageField>
           </Columns>
       </asp:GridView>

    </form>
</body>
</html>
