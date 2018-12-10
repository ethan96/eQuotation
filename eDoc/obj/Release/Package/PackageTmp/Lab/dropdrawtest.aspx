<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="dropdrawtest.aspx.vb" Inherits="EDOC.dropdrawtest" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.8.0.js" type="text/javascript"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.22/jquery-ui.js"></script>
<link rel="Stylesheet" href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.10/themes/redmond/jquery-ui.css" />
<script type="text/javascript">
    $(function () {
        $(".drag_drop_grid").sortable({
            items: 'tr:not(tr:first-child)',
            cursor: 'crosshair',
            connectWith: '.drag_drop_grid',
            axis: 'y',
            dropOnEmpty: true,
            receive: function (e, ui) {
                $(this).find("tbody").append(ui.item);
            }
        });
        $("[id*=gvDest] tr:not(tr:first-child)").remove();
    });
</script>
<style type="text/css">
    .GridSrc td
    {
        background-color: #A1DCF2;
        color: black;
        font-size: 10pt;
        font-family:Arial;
        line-height: 200%;
        cursor: pointer;
        width:100px
    }
    .GridSrc th
    {
        background-color: #3AC0F2;
        color: White;
        font-family:Arial;
        font-size: 10pt;
        line-height: 200%;
        width:100px;
    }
    .GridDest td
    {
        background-color: #eee !important;
        color: black;
        font-family:Arial;
        font-size: 10pt;
        line-height: 200%;
        cursor: pointer;
        width:100px
    }
    .GridDest th
    {
        background-color: #6C6C6C !important;
        color: White;
        font-family:Arial;
        font-size: 10pt;
        line-height: 200%;
        width:100px
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:GridView EnableTheming="false" ID="gvSource" runat="server" CssClass="drag_drop_grid GridSrc" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="Item" HeaderText="Item"/>
            <asp:BoundField DataField="Price" HeaderText="Price"/>
        </Columns>
    </asp:GridView>
    </div>
    </form>
</body>
</html>
