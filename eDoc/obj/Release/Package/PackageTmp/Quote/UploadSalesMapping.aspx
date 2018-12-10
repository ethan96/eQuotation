<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="UploadSalesMapping.aspx.vb" Inherits="EDOC.UploadSalesMapping" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table>
        <tr>
            <td class="menu_title">
                Upload ITP
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #d7d0d0; padding: 10px">
                <table>
                    <tr>
                        <td>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <hr />
   <%-- <p style="margin-left: 10px">
        <b>Use this interface to upload your order via a MS Excel spreadsheet, listing product
            numbers and quantities. </b>
        <br />
        <br />
        1. Fill out the spreadsheet with the columns as shown below. (Note: It is necessary
        to use the full Advantech product numbers. Ex.: AIMB-554G2-00A1E)
        <br />
        2. Choose the File Format of your upload
        <br />
        3. Click "Browse" to choose the file on your system
        <br />
        4. Once selected, click "Upload"
    </p>--%>
    <asp:GridView runat="server" ID="gv1" Width="100%" AllowPaging="false" AutoGenerateColumns="true"
        ShowHeaderWhenEmpty="true">
    </asp:GridView>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:Button ID="btnImPort" runat="server" Text="Import" OnClick="btnImPort_Click" Visible="false"/>
            </td>
        </tr>
    </table>
    <hr />
    <table>
      <tr>
    <td>
   <asp:HyperLink NavigateUrl="~/files/SalesMappingSample.xlsx" runat="server" ID="HLKExcelSample" Text="Click Here for Downloadable Sample (MS Excel)"></asp:HyperLink>
    </td>
    </tr>
    <tr>
    <td>
   <asp:Image ImageUrl="~/Images/SalesMappingSample.jpg" runat="server" ID="imgExcelSample" />
    </td>
    </tr>
    </table>
</asp:Content>

