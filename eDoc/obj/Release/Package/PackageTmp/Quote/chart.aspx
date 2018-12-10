<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/myMaster.master" CodeBehind="chart.aspx.vb" Inherits="EDOC.chart" %>
<%@ Register src="~/Ascx/NVCHART.ascx" tagname="NVCHART" tagprefix="uc1" %>
<%@ Register src="~/Ascx/QuoteReportSalesMultiSelectBlock.ascx" tagname="ascxPickSales" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
        <tr>
            <td>
                <table style="width: auto">
                    <tr>
                        <td>
                            Year:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpYear" AutoPostBack="true" OnSelectedIndexChanged="drpYear_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                            <td>
                            Month From:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpMF" OnSelectedIndexChanged="drpMF_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                             <td>
                            Month To:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpMT" OnSelectedIndexChanged="drpMT_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="11" Value="11"></asp:ListItem>
                            <asp:ListItem Text="12" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            Org:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpOrg" AutoPostBack="true" OnSelectedIndexChanged="drpOrg_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            RBU:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpRBU" AutoPostBack="true" OnSelectedIndexChanged="drpRBU_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Sales:
                        </td>
                        <td>
                           <uc1:ascxPickSales runat="server" id="ascxPickSales"></uc1:ascxPickSales>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr><td align="right"><uc1:NVCHART ID="NVCHART1" runat="server" /></td></tr>
        <tr>
            <td>
                <asp:ImageButton runat="server" ID="imgXls" ImageUrl="~/Images/excel.gif" AlternateText="Download"
                    OnClick="imgXls_Click" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" EmptyDataText="No raw data be found."
                    AllowPaging="true" PageIndex="0" PageSize="15" OnPageIndexChanging="GridView1_PageIndexChanging" AllowSorting="true" OnSorting="GridView1_Sorting" OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound">
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Chart ID="Chart1" runat="server" Width="600px" Height="400px">
                                <Legends>
                                    <asp:Legend TitleFont="Microsoft Sans Serif, 8pt, style=Bold" BackColor="Transparent" Font="Trebuchet MS, 8pt, style=Bold" IsTextAutoFit="false" Name="Default" Docking="Top">
                                    </asp:Legend>
                                </Legends>
                                <Series>
                                   <asp:Series Name="Converted(K $)" ChartType="StackedArea100" Color="185, 0, 120, 255" CustomProperties="DrawingStyle=Cylinder" Font="arial, 9px, style=Bold"> 
                                   </asp:Series>
                                   <asp:Series Name="Not Converted(K $)" ChartType="StackedArea" Color="185, 255, 155, 0" CustomProperties="DrawingStyle=Cylinder" Font="arial, 9px, style=Bold">
                                   </asp:Series>
                                   <asp:Series Name="Monthly Rate(%)" ChartType="StackedArea" Color="185, 255, 0, 0" CustomProperties="DrawingStyle=Cylinder" Font="arial, 9px, style=Bold">
                                   </asp:Series>
                                </Series>
                                 <ChartAreas> 
                                  <asp:ChartArea Name="ChartArea1" BackColor="AntiqueWhite" BackGradientStyle="TopBottom">
                                  <Area3DStyle Enable3D="true" WallWidth="3" Perspective="1" />
                                  <AxisY LineColor="64, 64, 64, 64" >
                                  <MajorGrid LineColor="64, 64, 64, 64"/>
                                  </AxisY>
                                  <AxisX LineColor="64, 64, 64, 64" Interval="1" >
                                  <MajorGrid LineColor="64, 64, 64, 64" Interval="1" />
                                  </AxisX>
                                  </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </td>
                        <td>
                            <table style="width: auto">
                                <tr>
                                    <td>
                                      Total Converted Rate:
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Chart ID="Chart2" runat="server" Width="600px">
                                            <Legends>
                                                <asp:Legend TitleFont="Microsoft Sans Serif, 8pt, style=Bold" BackColor="Transparent"
                                                    IsEquallySpacedItems="True" Font="Trebuchet MS, 8pt, style=Bold" IsTextAutoFit="False"
                                                    Name="Default">
                                                </asp:Legend>
                                            </Legends>
                                            <ChartAreas>
                                                <asp:ChartArea Name="ChartArea1">
                                                    <Area3DStyle Enable3D="true" />
                                                </asp:ChartArea>
                                            </ChartAreas>
                                        </asp:Chart>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
