<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BRAOnlineQuoteTemplate.ascx.vb" Inherits="EDOC.BRAOnlineQuoteTemplate" %>
<style type="text/css">
    table.contact
    {
        font: bold 13px/normal Arial,Helvetica,sans-serif;
        border-collapse: collapse;
        border-color: #ffffff;
        background: #FFFFFF;
        color: #333;
        width: 100%;
    }
    .contact th
    {
        width: 33%;
        border-collapse: collapse;
        background: #EFF4FB;
    }
    .contact td
    {
        font: 12px/normal Arial,Helvetica,sans-serif;
        border-collapse: collapse;
        background: #FFFFFF;
        color: #333;
    }
    
    table.estoretable
    {
        font: 13px/normal Arial,Helvetica,sans-serif;
        border-collapse: collapse;
        color: #333;
        width: 100%;
    }
    
    .estoretable th
    {
        padding: 3px;
        border-style: solid;
        border-width: 1px;
        border-color: #E5E5E5;
        text-align: center;
        background: #EFF4FB;
    }
    
    .estoretable td
    {
        padding: 3px;
        border-style: solid;
        border-width: 1px;
        border-color: #E5E5E5;
    }
    
    table.estoretable2
    {
        font: 13px/normal Arial,Helvetica,sans-serif;
        border-collapse: collapse;
        color: #333;
        width: 100%;
    }
    .estoretable2 td
    {
        padding: 3px;
        text-align: right;
    }
    
    .boder
    {
        border: 1px solid #E5E5E5;
    }
    
    .estoretable caption
    {
        padding: 0 0 .5em 0;
        text-align: left;
        text-transform: uppercase;
        color: #333;
        background: transparent;
    }
    .cartitem p
    {
        margin: 0;
        padding: 5px 0;
    }
    .cartitem label
    {
        font-weight: bold;
    }
</style>
<table width="720" border="0" cellspacing="0" cellpadding="0" align="center" style="font-family: Arial, Helvetica, sans-serif">
    <tr>
        <td align="left">
            <img src='<%=Util.GetRuntimeSiteIP() %>/Images/Advantech logo.jpg' alt="Advantech eStore" />
        </td>
    </tr>
<%--    <tr>
        <td>
            <table width="720" border="0" cellpadding="0" cellspacing="0" height="30">
                <tr>
                    <td bgcolor="#E5E5E5" style="background-color: #E5E5E5" height="30">
                        <a title="Home" href="http://buy.advantech.com/Default.htm" style="text-decoration: none">
                            <font size="2" color="#767677"><b>Home</b></font></a> &nbsp; &nbsp; &nbsp; <a title="About Us"
                                href="http://buy.advantech.com/AboutUs/Default.htm" style="text-decoration: none">
                                <font size="2" color="#767677"><b>About Us</b></font></a>
                        &nbsp; &nbsp; &nbsp; <a title="Support" href="http://support.advantech.com.tw/" style="text-decoration: none">
                            <font size="2" color="#767677"><b>Support</b></font></a>
                        &nbsp; &nbsp; &nbsp; <a title="Contact Us" href="http://buy.advantech.com/ContactUs/Default.htm"
                            style="text-decoration: none"><font size="2" color="#767677"><b>Contact Us</b></font></a>
                    </td>
                </tr>
                <tr>
                    <td height="18" bgcolor="#708AAC" style="background-color: #708AAC; padding: 5px 10px">
                        <div style="color: #FFFFFF; font-size: 12px" id="officeInformation_USA" runat="server">
                            Advantech Corporation | <asp:Literal runat="server" ID="litOfficeAddr" />
                            <br />
                            <asp:Literal runat="server" ID="litOfficeTelTime1" />
                        </div>
                        <div  style="color: #FFFFFF; font-size: 12px" id="officeInformation_Mexico" runat="server" visible="false">
                            <b>Advantech Electronics</b>
                             <br />
                            Ave. Baja California # 245, Int. 704 
                              <br />
                            Col. Hipódromo Condesa Del. Cuauhtémoc 06100, México D.F., México
                            <br />
                            Toll Free: 1-800-467-2415&nbsp;&nbsp;&nbsp;&nbsp;Phone: +52-55-6275-2777
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>--%>
    <tr>
        <td>
            <table width="100%" border="0" cellspacing="0" cellpadding="6" style="border: 1px solid #CCCCCC;
                background-color: #FFFFE6" bgcolor="#FFFFE6" align="center">
                <tr>
                    <td style="font-size: 13px;">
                        <div>
                            <b>Customer</b></div>
                        <div style="padding-left: 10px">
                            <p>
                                <span lang="EN-US" style="font-size: 10.0pt; font-family: Arial,sans-serif">
                                    Obrigado por escolher os produtos e serviços da Advantech!
                                </span>
                            </p>
                            <center>
                            <table width="70%">
                                <tr>
                                    <td style="font-size: 10.0pt; font-family: Arial,sans-serif">
                                        Comerical:
                                        <br />Advantech Brasil LTDA.
                                        <br />Rua Fagundes Filho, 134 - 12º andar
                                        <br />CEP 04304-010 - Sao Paulo,SP
                                        <br />Tel: 11 5592-5355
                                        <br />www.advantech.com.br
                                    </td>
                                    <td style="font-size: 10.0pt; font-family: Arial,sans-serif">
                                        Faturamento/Fábrica:
                                        <br />Advantech Brasil LTDA.
                                        <br />Rua Dr. Hofmann, 281, Morro Chic,
                                        <br />CEP: 37.500-086, Itajubá, MG.
                                        <br />CNPJ: 03.800.074/0002-81
                                        <br />Inscrição Est.:324.326.504.0079
                                    </td>
                                </tr>
                            </table>
                            </center>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF">
                        <table width="100%" border="0" cellspacing="2" cellpadding="0">
                            <tr>
                                <td align="center" width="28%"><b>Quotação Número: <asp:Label runat="server" ID="LabelQuoteID" /></b></td>
                                <td align="center" width="12%"><b>Versão: <asp:Label runat="server" ID="LabelQuoteRevisionNumber" /></b></td>
                                <td align="center" width="30%"><b>Data da Cotação: <asp:Label runat="server" ID="quoteDate" /></b></td>
                                <td align="center" width="30%"><b>Validade da Cotação: <asp:Label runat="server" ID="expiredDate" /></b></td>
                            </tr>
<%--                            <tr>
                                
                                <td align="center" width="33%"></td>
                                <td align="center" width="33%"></td>
                            </tr>
--%>                            
                            <tr>
                                <td colspan="4" background="http://buy.advantech.com/App_Themes/AUS/line.gif" height="1">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <!-- Contact Info -->
                                    <table width="100%">
                                        <tr valign="top">
                                            <td style="width: 33%">
                                                <asp:Table ID="Table_SoldTo" width="100%" class="contact" runat="server" border="1" cellspacing="0" cellpadding="3" style="border: 1px solid #CCCCCC">
                                                    <asp:TableHeaderRow><asp:TableHeaderCell ColumnSpan="2" style="color: #333333;">Dados de Faturamento:&nbsp;<asp:Label runat="server" ID="lblSoldtoERPID" /></asp:TableHeaderCell></asp:TableHeaderRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Empresa:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_SoldTo_Company" Width="100%"><%=_lbSoldtoCompany%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Endereço:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_SoldTo_Address"><%=_lbSoldtoAddr%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Tel:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_SoldTo_Tel"><%=_lbSoldtoTel%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Contato:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_SoldTo_Attention"><%=_lbSoldtoAttention%></asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 33%">
                                                <asp:Table ID="Table_ShipTo" width="100%" class="contact" runat="server" border="1" cellspacing="0" cellpadding="3" style="border: 1px solid #CCCCCC">
                                                    <asp:TableHeaderRow><asp:TableHeaderCell ColumnSpan="2" style="color: #333333;">Dados de Enterga:&nbsp;<asp:Label runat="server" ID="lblShipToERPID" /></asp:TableHeaderCell></asp:TableHeaderRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Empresa:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_ShipTo_Company" Width="100%"><%=_lbShiptoCompany%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Endereço:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_ShipTo_Address"><%=_lbShiptoAddr%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Endereço2:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="TableCell2"><%=_lbShiptoAddr2%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Tel:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_ShipTo_Tel"><%=_lbShiptoTel%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Contato:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_ShipTo_Attention"><%=_lbShiptoAttention%></asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                            <td style="width: 33%" runat="server" id="tdBill">
                                                <asp:Table ID="Table_BillTo" width="100%" class="contact" runat="server" border="1" cellspacing="0" cellpadding="3" style="border: 1px solid #CCCCCC">
                                                    <asp:TableHeaderRow><asp:TableHeaderCell ColumnSpan="2" style="color: #333333;">Dados de Bill to &nbsp;<asp:Label runat="server" ID="lblBillToERPID" /></asp:TableHeaderCell></asp:TableHeaderRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Empresa:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_BillTo_Company" Width="100%"><%=_lbBilltoCompany%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Endereço:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_BillTo_Address"><%=_lbBilltoAddr%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Tel:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_BillTo_Tel"><%=_lbBilltoTel%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableHeaderCell style="color: #333333;" HorizontalAlign="Right">Contato:</asp:TableHeaderCell>
                                                        <asp:TableCell ID="Cell_BillTo_Attention"><%=_lbBilltoAttention%></asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- Contact Info/ -->
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" height="5">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label runat="server" ID="lblSalesPerson" Font-Bold="true" style="font-size: 14px;" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:GridView DataKeyNames="line_no" ID="gv1" runat="server" AllowPaging="false"
                                        EmptyDataText="no Item." AutoGenerateColumns="false" 
                                        OnRowDataBound="gv1_RowDataBound" Font-Size="10pt" Width="100%">
                                        <AlternatingRowStyle BackColor="#EBEBEB" />
                                        <Columns>
                                            <asp:BoundField DataField="line_no" HeaderText="Item" ItemStyle-HorizontalAlign="center">
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField  ItemStyle-HorizontalAlign="left" ItemStyle-Wrap="true" ItemStyle-Width="20%">
                                                <HeaderTemplate>
                                                    Código
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbPN" Text='<%#Bind("partNo")%>' style="vertical-align:middle"></asp:Label>&nbsp
                                                    <asp:Image runat="server" ID="imgRecyclingFee" ImageUrl="~/Images/Recycle.png" Visible="false" style="vertical-align:middle"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VirtualPartNo" HeaderText="Virtual Part No" ItemStyle-HorizontalAlign="left">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Descrição">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbdescription" Text='<%#Bind("description") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Quant.
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#Bind("qty") %>' ID="lbGVQty"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:TemplateField>
<%--                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    List Price
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#_currencySign %>' ID="lbListPriceSign"></asp:Label>
                                                    <asp:Label runat="server" Text='<%#FormatNumber(Eval("listprice"),2) %>' ID="lbListPrice"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Pr. Unit com impostos (Sem IPI)
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#_currencySign %>' ID="lbNONIPISign"></asp:Label>
                                                    <asp:Label runat="server" Text='<%#FormatNumber((Eval("newunitprice") * Eval("qty") + Eval("bx13") + Eval("bx41") + Eval("bx72") + Eval("bx82") + Eval("bx94") + Eval("bx95") + Eval("bx96")) / Eval("qty"), 2) %>' ID="lbNONIPI"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    Pr. Total com impostos (Sem IPI)
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#_currencySign %>' ID="lbNONIPITotalSign"></asp:Label>
                                                    <asp:Label runat="server" Text='<%#FormatNumber(Eval("newunitprice") * Eval("qty") + Eval("bx13") + Eval("bx41") + Eval("bx72") + Eval("bx82") + Eval("bx94") + Eval("bx95") + Eval("bx96"), 2) %>' ID="lbNONIPITotal"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ICMS">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbicms" Text='<%#Bind("ICVA") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NCM">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbNCM" Text='<%#Bind("NCM") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="IPI">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbIPI" Text='<%#Bind("IPVA") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    V.IPI
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#_currencySign %>' ID="lbUnitPriceSign"></asp:Label>
                                                    <asp:Label runat="server" Text='<%#FormatNumber(Eval("bx23"), 2) %>' ID="lbUnitPrice"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                <HeaderTemplate>
                                                    Extended Price
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%#_currencySign %>' ID="lbSubTotalSign"></asp:Label>
                                                    <asp:Label runat="server" Text="" ID="lbSubTotal"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Right" Wrap="false"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#FF6600" ForeColor="White" />
                                    </asp:GridView>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="right">
                                    <table width="450px" >
                                        <tr style="text-align: right">
                                            <td width="300px">
                                                Valor do ICMS-ST:
                                            </td>
                                            <td width="150px">
                                                <asp:Label ID="LbTotal_BX41_1" runat="server" Text="0.00" /><%= _currencySign %>
                                            </td>
                                        </tr>
                                        <tr style="text-align: right">
                                            <td>
                                                Diferença de ICMS p/ consumo:
                                            </td>
                                            <td>
                                                <asp:Label ID="LbTotal_BX41_2" runat="server" Text="0.00" /><%= _currencySign %>
                                            </td>
                                        </tr>
                                        <tr style="text-align: right">
                                            <td>
                                                Difal Destino - Não Contribuinte do ICMS:
                                            </td>
                                            <td>
                                                <asp:Label ID="LbTotal_BX95_1" runat="server" Text="0.00" /><%= _currencySign %>
                                            </td>
                                        </tr>
                                        <tr style="text-align: right">
                                            <td>
                                                Total de Frete:
                                            </td>
                                            <td>
                                                <asp:Label ID="LbTotal_FK00_1" runat="server" Text="0.00" /><%= _currencySign %>
                                            </td>
                                        </tr>
                                        <tr style="text-align: right">
                                            <td>
                                                Total da Proposta:
                                            </td>
                                            <td>
                                                <asp:Label ID="LabelTotal5" runat="server" Text="0.00" /><%= _currencySign %>
                                            </td>
                                        </tr>
                                        <tr style="text-align: right">
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr style="text-align: right">
                                            <td>
                                                Base de Calculo de ICMS:
                                            </td>
                                            <td>
                                                <asp:Label ID="LbTotal_BK10_1" runat="server" Text="0.00" /><%= _currencySign %>
                                            </td>
                                        </tr>
                                        <tr style="text-align: right">
                                            <td>
                                                ICMS Proprio:
                                            </td>
                                            <td>
                                                <asp:Label ID="LabelTotal7" runat="server" Text="0.00" /><%= _currencySign %>
                                            </td>
                                        </tr>
                                        <tr style="text-align: right">
                                            <td>
                                                Base de Calculo de ICMS-ST:
                                            </td>
                                            <td>
                                                <asp:Label ID="LbTotal_BX40_1" runat="server" Text="0.00" /><%= _currencySign %>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div>
                            Tipo:<asp:Label ID="lbQuotationType" runat="server" Text="" /><br />
                            Frete:<asp:Label ID="lbIncoterm1" runat="server" Text="" /><br />
                            Condição de Pagamento:<asp:Label ID="lbPayment" runat="server" Text="Cash advance - wire transfer" /><br />
                            Prazo de entrega:<asp:Label ID="lbDeliveryDate" runat="server" Text="" /><br />
                        </div>
                        <div style="color: #333; padding-left: 10px">
                            <center><u>CONDIÇÕES COMERCIAIS</u></center>
                            <br />
                            1) Proposta sujeita a aprovação de crédito na data da criação do Pedido e do faturamento.
                            <br />
                            2) Frete Oferecemos frete simples à todo território nacional para compras acima de R$ 1.000,00.
                               Custos de fretes especiais ou taxas de reenvio serão da responsabilidade do destinatário.
                            <br />
                            3) Garantia padrão: 24 meses para equipamentos novos. Consulte opções para garantia estendida de até 5 anos e contrato de manutenção.
                            <br />
                            4) Cancelamento: Todo pedido poderá ser cancelado sob penalidade de 30% do valor total da venda.
                            <br />
                            5) Itens de software e garantia estendida podem ser faturados através de Nota Fiscal de Serviço.
                            <br />
                            6) Será aplicado multa de 1%, mais juros diário de 0,33% para pagamentos em atraso.
                            <br />
<%--                            <center>Sleeping Account ( )</center>--%>

                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>