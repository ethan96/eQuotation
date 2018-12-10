<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master"
    CodeBehind="USPriceOW_Log.aspx.vb" Inherits="EDOC.USPriceOW_Log" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />Unit Price Overwriting Log<br /><br />
 <fieldset>
        <legend><span style="font-weight: bold; font-size: 12px; color: #666666">Search</span></legend>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td align="left" width="70px">
                                Quote No:
                            </td>
                            <td align="left" width="100px">
                                <asp:TextBox runat="server" ID="txtQuoteId"></asp:TextBox>
                            </td>
                            <td align="left" width="100px">
                                <asp:Button runat="server" ID="btnSearch" Text=">>Search<<" />
                            </td>
                            <td>
                                Click <asp:HyperLink ID="HyperLink1" NavigateUrl="USPriceOW.aspx" runat="server">here</asp:HyperLink> to overwrite price 
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr><td></td></tr>
        </table>
    </fieldset>
    <hr />
    <asp:GridView runat="server" ID="gv1" AutoGenerateColumns="true" AllowPaging="true"
        DataSourceID="SqlDataSource1" AllowSorting="true" EnableViewState="false" EmptyDataText="You can search by a specified quote No. to get a item list.">
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="SELECT QuoteNo,QuoteRevision,Line_No,oPrice,nPrice,eDate,eBy FROM QUOTELINEPRICEMODIFYLOG Where QuoteNo like '%' + @QuoteNo + '%' Order by eDate desc"
       CancelSelectOnNullParameter="false"  ConnectionString="<%$ connectionStrings: EQ %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtQuoteId" Name="QuoteNo" DefaultValue="*" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
