<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="QuotationViewOption.ascx.vb" Inherits="EDOC.QuotationViewOption" %>
<asp:UpdatePanel ID="UP_QuotationViewOption" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:HiddenField ID="HFQuoteID" runat="server" Value="" />
        <asp:RadioButtonList runat="server" ID="RadioButtonList_FormatOptions" RepeatDirection="Vertical"
            AutoPostBack="True">
            <asp:ListItem Text="Display main item only & total system price" Value="0" />
            <asp:ListItem Text="Display line item part numbers & line item prices" Value="1" />
            <asp:ListItem Text="Display line item category & total system price" Value="2" />
            <asp:ListItem Text="Display line item part numbers & total system price" Value="3" />
            <asp:ListItem Text="External User – Channel" Value="4" />
            <asp:ListItem Text="External User – Channel & total list price" Value="6" />
            <asp:ListItem Text="Display line item & breakpoint" Value="5" />
        </asp:RadioButtonList>

        <asp:RadioButtonList runat="server" ID="rbtnIsVirtualItmeOnly" RepeatDirection="Vertical"
            AutoPostBack="True">
            <asp:ListItem Text="Virtual Part No Only" Value="1" />
            <asp:ListItem Text="Part No Only" Value="0" />
            <%--        <asp:ListItem Text="Both Part No & Virtual Part No" Value="2" />--%>
        </asp:RadioButtonList>

       <asp:RadioButtonList runat="server" ID="RadioButtonList_ATWFormatOptions" RepeatDirection="Vertical"
            AutoPostBack="True" Visible="false">
            <asp:ListItem Text="Download PDF format quotation in Traditional Chinese version" Value="20" />
            <asp:ListItem Text="Download PDF format quotation in English version" Value="21" />
            <asp:ListItem Text="Download Word format quotation in Traditional Chinese version" Value="22" />
            <asp:ListItem Text="Download Word format quotation in English version" Value="23" />
        </asp:RadioButtonList>

        <asp:RadioButtonList runat="server" ID="RadioButtonList_AJPFormatOptions" RepeatDirection="Vertical"
            AutoPostBack="True" Visible="false">
            <asp:ListItem Text="Display quotation in PDF with all items." Value="31" Selected="True" />
            <asp:ListItem Text="Display quotation in PDF without BTOS child part numbers." Value="32" />
        </asp:RadioButtonList>

        <asp:CheckBox runat="server" ID="cbNCNR" Visible="false" Text="Non-Cancelable / Non-Returnable (NCNR) Agreement" />
        <asp:CheckBox runat="server" ID="cbChangeQuoteTitle" Visible="false" Text="Revise Quotation title to Proforma Invoice" />       
    
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="RadioButtonList_FormatOptions" />
        <asp:AsyncPostBackTrigger ControlID="rbtnIsVirtualItmeOnly" />
    </Triggers>
</asp:UpdatePanel>
