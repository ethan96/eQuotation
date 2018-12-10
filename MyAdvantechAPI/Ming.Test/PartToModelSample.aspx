<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartToModelSample.aspx.cs" Inherits="Ming.Test.PartToModelSample" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Model header:
       <asp:GridView ID="GridView2" runat="server" />
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


        Part specs
       <asp:GridView ID="GVSpec" runat="server" />

        <br />
        SAP Description:
        <asp:Label ID="SAPDescription" runat="server" />
        <br />
        PIS Description:
        <asp:Label ID="PISDescription" runat="server"  />
    </div>
    </form>
</body>
</html>
