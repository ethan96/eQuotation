<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModelDetail.aspx.cs" Inherits="PISTest.ModelDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Category:<asp:Label ID="CategoryPath" runat="server" Text="ModelName" /><br />
            <hr />
            Model Name:<asp:Label ID="ModelName" runat="server" Text="ModelName" /><br />
            Model Description:<asp:Label ID="ModelDescription" runat="server" Text="ModelDescription" /><br />
            Model Introduction:<asp:Label ID="ModelIntroduction" runat="server" Text="ModelIntroduction" /><br />

            <hr />
            Model's default features:
       <asp:GridView ID="GridView3" runat="server" />
            <hr />
            Model's features in Simplified Chinese:
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
            <hr />
            Model specs
       <asp:GridView ID="GVSpec" runat="server" />
            <hr />
            Parts
       <asp:GridView ID="GVParts" runat="server" />

            <hr />
            Fisrt Part's spec
       <asp:GridView ID="GVFistPartSpec" runat="server" />

        </div>
    </form>
</body>
</html>
