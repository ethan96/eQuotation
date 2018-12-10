<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="QuotationToOrderLog.aspx.vb" Inherits="EDOC.QuotationToOrderLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Advantech eQuotation</title>
    <link href="..\css\Ajax.css" rel="stylesheet" type="text/css" />
     <link href="..\css\stylesheet.css" rel="stylesheet" type="text/css" />
</head>

<body>
    <form id="form1" runat="server">
    <asp:Label ID="Label1" runat="server" Text="Label">Qoutation ID:</asp:Label>
    <asp:Label ID="Label_QoutationID" runat="server" Text="Label"></asp:Label>
    <br>
    <div>
        <asp:GridView ID="GridView_Order_Log" runat="server" AutoGenerateColumns="false" >
            <EmptyDataTemplate>
                <div align="center">
                    No Data Found
                </div>
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField HeaderText="Order No" DataField="SO_NO" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField HeaderText="Amount" DataField="Amount" ItemStyle-HorizontalAlign="Left" DataFormatString="${0:F2}"   />
                <asp:BoundField HeaderText="Status" DataField="ORDER_STATUS" ItemStyle-HorizontalAlign="Left"  />
                <asp:BoundField HeaderText="Created Date" DataField="ORDER_DATE" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Left"  />
                <%--<asp:BoundField HeaderText="Expires On" DataField="DUE_DATE" DataFormatString="{0:yyyy/MM/dd}" ItemStyle-HorizontalAlign="Left"  />--%>
                <asp:BoundField HeaderText="PO No" DataField="PO_NO" ItemStyle-HorizontalAlign="Left"   />
                <asp:BoundField HeaderText="SoldTo ID" DataField="SOLDTO_ID" ItemStyle-HorizontalAlign="Left"    />
                <asp:BoundField HeaderText="ShipTo ID" DataField="SHIPTO_ID" ItemStyle-HorizontalAlign="Left"   />
                <asp:BoundField HeaderText="BillTo ID" DataField="BILLTO_ID" ItemStyle-HorizontalAlign="Left"  />
            </Columns>
        </asp:GridView>    
    </div>
    </form>
</body>
</html>