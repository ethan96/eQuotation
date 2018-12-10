<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="USVolumeDiscount.ascx.vb" Inherits="EDOC.USVolumeDiscount" %>

<table>
    <tr>
        <td>
            <asp:GridView ID="gv0" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="PartNo" HeaderText="Part No." ItemStyle-HorizontalAlign="center" />
                    <asp:BoundField DataField="ORG" HeaderText="ORG" ItemStyle-HorizontalAlign="center" />
                    <asp:BoundField DataField="CustomerGroup" HeaderText="Customer Group" ItemStyle-HorizontalAlign="center" />
                    <asp:BoundField DataField="Currency" HeaderText="Currency" ItemStyle-HorizontalAlign="center" />
                    <asp:BoundField DataField="ListPrice" HeaderText="List Price" ItemStyle-HorizontalAlign="center" />
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td style="height:5px"></td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="gv1" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="PartNo" HeaderText="Part No." ItemStyle-HorizontalAlign="center" />
                    <asp:BoundField DataField="PriceGroup" HeaderText="Price Group" ItemStyle-HorizontalAlign="center" />
                    <asp:BoundField DataField="Scale" HeaderText="Scale" ItemStyle-HorizontalAlign="center" />
                    <asp:BoundField DataField="Price" HeaderText="Price" ItemStyle-HorizontalAlign="center" />
                    <asp:BoundField DataField="Valid_From" HeaderText="Valid From" ItemStyle-HorizontalAlign="center" />
                    <asp:BoundField DataField="Valid_To" HeaderText="Valid To" ItemStyle-HorizontalAlign="center" />
                </Columns>
            </asp:GridView>
        </td>
    </tr>

</table>
