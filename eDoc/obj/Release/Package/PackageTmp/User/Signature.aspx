<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master"
    CodeBehind="Signature.aspx.vb" Inherits="EDOC.Signature" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="Panel1">
        <table width="100%" class="rightcontant3">
            <tr>
                <td align="left" width="50%">
                    Please upload my Signature：
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                    <asp:Label ID="lbUploadMessage" runat="server" ForeColor="#FF3300" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <ul>
                        <li>File Format：PNG, JPG or GIF</li>
                        <li>File Size(Maximal)：Height 150 X Width 500</li>
                    </ul>
                </td>
                <td align="left" valign="bottom">
                    <asp:HyperLink ID="Hysignature" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="Tomato" NavigateUrl="~/Quote/QuotationMaster.aspx">Go back to create Quotation</asp:HyperLink></strong>
                </td>
            </tr>
        </table>
        <asp:Button ID="Button1" runat="server" Text="Delete" />
        <asp:GridView ID="GV1" runat="server" AutoGenerateColumns="False" EmptyDataText="No search results were found."
            Width="100%" DataKeyNames="SID">
            <Columns>
                <asp:TemplateField ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        Check
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkKey" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        Default Signature
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:RadioButtonList ID="RBLIsDefaultSignature" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" OnSelectedIndexChanged="RBLIsDefaultSignature_SelectedIndexChanged">
                            <asp:ListItem Text="Active" Value="1" />
                            <asp:ListItem Text="Inactive" Value="0" Selected="True" />
                        </asp:RadioButtonList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        File Name
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbfilename" runat="server" Text="" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Signature Image
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("SID", "~\Ascx\DisplayImageHandler.ashx?Type=signature&ImageID={0}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource_ProductFamily" runat="server" ConnectionString="<%$ ConnectionStrings:EQ %>"
            SelectCommand="SELECT SID,UserID,SignatureData,Active,FileName,LastUpdated FROM Signature Where UserID='<%=Pivot.CurrentProfile.UserId %>' Order by LastUpdated desc" />
    </asp:Panel>
</asp:Content>
