<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="QuoteNotes.ascx.vb"
    Inherits="EDOC.QuoteNotes" %>
<style>
    body
    {
        font-family: Calibri;
    }
    .QNtext1
    {
        color: #004B9C;
        font-family: Calibri;
        vertical-align: top;
        font-weight: normal;
        font-size: 14px;
        white-space: nowrap;
        border-bottom-width: 1px;
        border-bottom-style: solid;
        border-bottom-color: #004B9C;
    }
    .QNtext2
    {
        color: #000000;
        font-family: Calibri;
        font-weight: bold;
        vertical-align: top;
        white-space: nowrap;
    }
    .QNtext3
    {
        color: #000000;
        font-family: Calibri;
        font-weight: bold;
        vertical-align: top;
    }
    .QNtext4
    {
        color: #004B9C;
        font-family: Calibri;
        font-weight: bold;
        vertical-align: top;
        white-space: nowrap;
    }
    .QNbr1
    {
        border-bottom-width: 1px;
        border-bottom-style: solid;
        border-bottom-color: #004B9C;
    }
    .QNtext5
    {
        color: #004B9C;
        font-family: Calibri;
        vertical-align: top;
        font-weight: normal;
        font-size: 14px;
        white-space: nowrap;
        text-align: right;
    }
    .QNtext6
    {
        color: #000000;
        font-family: Calibri;
        font-weight: bold;
        vertical-align: top;
        white-space: nowrap;
        text-align: right;
    }
    .QNtext7
    {
        color: #004B9C;
        font-family: Calibri;
        font-weight: bold;
        font-size: 20px;
        vertical-align: top;
        white-space: nowrap;
    }
    .QNtext8
    {
        color: #004B9C;
        font-family: Calibri;
        font-weight: bold;
        font-size: 28px;
        vertical-align: top;
        white-space: nowrap;
        line-height: 28px;
    }
    .QNdcs1
    {
        border: 1px dotted #004B9C;
        width: 673px;
        height: 250px;
        padding: 8px;
    }
</style>
<table width="675" align="center" id="table1" runat="server">
    <tr id="trSandCTitle" runat="server">
        <td>
            <span class="QNtext8">Special Terms and Conditions </span>
        </td>
    </tr>
    <tr id="trSandC" runat="server">
        <td>
            <div class="QNdcs1" style="margin-bottom: 10px;">
                <%--     <div class="QNtext4">E.g. Delivery terms: </div>   
Lead-times of the product in mass production will be between eight (8) and fourteen (14) weeks without forecast. When Televes provides Advantech with a 3 months up to date rolling forecast Advantech can reduce the delivery time to 6 to 10 weeks. Notwithstanding the aforementioned, reasonable delivery terms applicable to each delivery shall be confirmed by Advantech’s acceptances to Televes’s orders. If Advantech is unable to commit the delivery schedule, both parties will discuss a reasonable and acceptable solution. If a solution is not reached Televes should notify Advantech in writing of breach of contract.
    <div class="QNtext4">E.g. Warranty: </div>
Product warrantee: two (2) years after shipment of the Product. --%>
                <asp:Literal ID="LitTandC" runat="server" />
            </div>
        </td>
    </tr>
    <tr  id="trQnoteNoteTitle" runat="server">
        <td>
            <span class="QNtext8">Quote Notes: </span>
        </td>
    </tr>
    <tr id="trQnoteNote" runat="server">
        <td>
            <div class="QNdcs1" style="height: 200px;">
                <asp:Literal ID="LitNotes" runat="server"></asp:Literal>
            </div>
        </td>
    </tr>
</table>
