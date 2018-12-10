<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="Home.aspx.vb" Inherits="EDOC.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset style="margin-top: 3%; width: 500px;">
        <legend style="font-weight: bold; font-size: 14px">eQuotation Menu</legend>
        <table width="350px" class="NoBord" style="margin-top: 1%" runat="server" id="SubTableForANASCMTeamOnly" visible="false">
            <tr runat="server" id="tr1" visible="true">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="HyperLink12" NavigateUrl="~/Quote/ChartOver.aspx"
                        Text="Quote Report" />
                </td>
            </tr>
        </table>
        <table width="350px" class="NoBord" style="margin-top: 1%" runat="server" id="MainTable">
            <tr>
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="hyNewQuote" NavigateUrl="~/Quote/QuotationMaster.aspx"
                        Text="Create New Quotation" />
                </td>
            </tr>
            <tr runat="server" id="trMyTeamQuote" visible="true">
                <td style="width: 3px;">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="hyMyTeamQuote" NavigateUrl="~/Quote/myTeamsQuotation.aspx"
                        Text="My Team's Quotation" />
                    <%--    <asp:HyperLink runat="server" ID="hyMyTeamQuote1" NavigateUrl="~/Quote/quoteByAccountOwner.aspx?Mode=MyTeam"
                    Text="My Team's Quotation" />--%>
                </td>
            </tr>
            <tr>
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="hyMyAccountQuote" NavigateUrl="~/Quote/quoteByAccountOwner.aspx?Mode=MyTeam"
                        Text="My Account's Quotation" />
                </td>
            </tr>
            <tr>
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="hyMyQuote" NavigateUrl="~/Quote/quoteByAccountOwner.aspx"
                        Text="My Quotation" />
                </td>
            </tr>
            <tr runat="server" id="trMyPipeline" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:LinkButton ID="LinkButtonMyPipeline" runat="server" OnClientClick="return USPipelineYearSelect();">My Pipeline</asp:LinkButton>
                </td>
            </tr>
            <tr runat="server" id="trCopyQuotation">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="hyCopyQuote" NavigateUrl="~/Quote/CreateQuotationByRef.aspx"
                        Text="Create Quotation By Copy" />
                </td>
            </tr>
            <tr runat="server" id="trCopySAPOrder">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="hyOrder2Quote" NavigateUrl="~/Quote/CreateQuotationByCopy.aspx"
                        Text="Copy SAP Order to eQuotation" />
                </td>
            </tr>
            <tr runat="server" id="trAEUITP" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="hyAEUITP" NavigateUrl="~/Quote/ITPInquiry.aspx"
                        Text="AEU ITP Inquiry" />
                </td>
            </tr>
            <tr runat="server" id="trDxPrice" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="hyDxPrice" NavigateUrl="~/Quote/price_bylevel.aspx"
                        Text="Dx Price Inquiry" />
                </td>
            </tr>
            <tr runat="server" id="trGP" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl="~/GPControl/ApprovalDef.aspx"
                        Text="GP Approval Setting" />
                </td>
            </tr>
            <tr runat="server" id="trUsQuickConfig" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="hyConfigBTOS" NavigateUrl="~/Quote/quickConfigQuote.aspx"
                        Text="BTOS/CTOS Quotation" />
                </td>
            </tr>
            <tr runat="server" id="trUsSallBTOS" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="hySallBTOS" NavigateUrl="~/Quote/BtosSearch.aspx"
                        Text="Search All BTOS" />
                </td>
            </tr>
            <tr runat="server" id="trReport" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="HyperLink2" NavigateUrl="~/Quote/ChartOver.aspx"
                        Text="Quote Report" />
                </td>
            </tr>
            <tr runat="server" id="trANAiAUsageReport" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="HyperLink8" NavigateUrl="~/Quote/ANAiAUsageReport.aspx"
                        Text="ANA iA Usage Report" />
                </td>
            </tr>
            <tr runat="server" id="trSignature" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="HyperLink3" NavigateUrl="~/User/Signature.aspx"
                        Text="Upload Signature" />
                </td>
            </tr>
            <tr runat="server" id="trUSPriceOW" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="HyperLink4" NavigateUrl="~/Quote/USPriceOW.aspx"
                        Text="Quote Line Price Overwrite" />
                </td>
            </tr>
            <tr runat="server" id="trOptyManagement" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="HyperLink5" NavigateUrl="~/Lab/Opportunity_Management.aspx"
                        Text="Opportunity Management" />
                </td>
            </tr>
            <tr runat="server" id="trSiebelQuoteToOrder" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="HyperLink6" NavigateUrl="~/ATW/SiebelQuote.aspx"
                        Text="Transfer Siebel quote to order" />
                </td>
            </tr>
            <tr runat="server" id="trSOPforATWAOnline" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="HyperLink7" NavigateUrl="~/SOP/SOPforATWAOnline.pptx"
                        Text="ATW AOnline SOP" />
                </td>
            </tr>
            <tr runat="server" id="trSOPforAKRAOnline" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="HyperLink9" NavigateUrl="~/SOP/2015_AKR_eQuotation SOP_Final_Iris_10.30.ppt"
                        Text="AKR AOnline SOP" />
                </td>
            </tr>
            <tr runat="server" id="trSOPforIntercon" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="HyperLink11" NavigateUrl="~/SOP/2016_eQuotation_SOP_Intercon_0630.ppt"
                        Text="Intercon SOP" />
                </td>
            </tr>
            <tr runat="server" id="trPipeline" visible="false">
                <td style="width: 3px">
                    <img src="Images/square_gray.gif" style="width: 4px;" alt="" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="HyperLink10" NavigateUrl="http://172.21.1.133/Aonline%20Pipeline/Report.aspx"
                        Text="Pipeline Reports" Target="_blank" />
                </td>
            </tr>
        </table>
    </fieldset>
    <div style="display: none">
        <div id="divUSPipelineSelect" style="width: 100%; height: 90%;">
            <div style="height: 80%; width: 100%; text-align: center;">
                <h2 style="color: #1b1b69; text-align: center;">Please select a year for pipeline report generation.</h2>
                <div>
                    <div style="float: left; height: 100%; width: 49%; text-align: center">
                        <p style="font-size: 15px;">Pipeline Year</p>
                        <div style="margin-left: 35%">
                            <asp:RadioButtonList ID="rblPipelineYear" runat="server">
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div style="float: left; height: 100%; width: 49%;">
                        <p style="font-size: 15px;">Opp Type</p>
                        <div style="margin-left: 35%">
                            <asp:RadioButtonList ID="rblPipelineOppType" runat="server">
                                <asp:ListItem Value="OPEN" Selected="True">Open</asp:ListItem>
                                <asp:ListItem Value="CLOSE">Closed</asp:ListItem>
                                <asp:ListItem Value="ALL">All</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
            </div>
            <div style="height: 20%; text-align: center; margin-top: 15px">
                <asp:Button runat="server" ID="btnUSPipelineYearSelect" Text="Submit" OnClick="btnUSPipelineYearSelect_Click" OnClientClick="$.fancybox.close();" />
            </div>
        </div>
    </div>

    <link rel="Stylesheet" href="../Js/FancyBox/jquery.fancybox.css" type="text/css" />
    <script type="text/javascript" src="../Js/FancyBox/jquery.fancybox.js"></script>
    <script language="javascript" type="text/javascript">
        function USPipelineYearSelect() {
            var gallery = [{
                href: "#divUSPipelineSelect"
            }];

            $.fancybox(gallery, {
                'autoSize': false,
                'width': 450,
                'height': 300,
                'autoCenter': true
            });
            return false;
        }
    </script>
</asp:Content>
