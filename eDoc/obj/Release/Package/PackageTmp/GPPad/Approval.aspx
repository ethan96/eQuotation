<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Approval.aspx.vb" Inherits="EDOC.Approval" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="//cdn.bootcss.com/bootstrap/3.3.5/css/bootstrap.min.css">
    <script src="../Js/jquery-latest.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="container">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">QuoteDetail</h3>
                </div>
                <div class="panel-body">
                    <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Both" Height="300">
                        <div runat="server" id="divContent">
                        </div>
                    </asp:Panel>
                </div>
            </div>

            <div class="panel panel-primary" style="text-align:center;">
                <div class="panel-heading">
                    <h3 class="panel-title">Below GP</h3>
                </div>
                <div class="panel-body">
                    <asp:Literal ID="LitDetail" runat="server"></asp:Literal>
                    <asp:GridView runat="server" ID="gvGPQuoteLine" AutoGenerateColumns="false" CssClass="table table-bordered">
                        <Columns>
                            <asp:TemplateField HeaderText="Item" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("LineNo")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Material" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("PartNo")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qty" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("qty")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="List Unit Price" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("ListPrice")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quoted Unit Price" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("UnitPrice")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quoted Discount" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("Discount")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cost (ITP)" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("Cost")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GP" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("GPPercent")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Min GP Threshold" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("ThresholdPercent")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField HeaderText="GP Block" DataField="GPBlock" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                    </asp:GridView>
                    <asp:GridView runat="server" ID="gvAACGPTotalInfo" AutoGenerateColumns="false" CssClass="table table-bordered">
                        <Columns>
                            <asp:TemplateField HeaderText="Total List Price" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("TotalListPrice")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Unit Price" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("TotalUnitPrice")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Discount" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("TotalDiscount")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Cost" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("TotalCost")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total GP" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("TotalGPPercent")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Min. GP Threshold" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("TotalThresholdPercent")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField HeaderText="GP Block" DataField="IsTotalGPBlock" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                    </asp:GridView>

                    <div class="clearfix">.</div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Literal ID="litTxt" runat="server"></asp:Literal>
                           <%-- <div class="alert alert-success" role="alert">
                                <strong>Well done!</strong> You successfully read this important alert message.
                            </div>
                            <div class="alert alert-danger" role="alert">
                                <strong>Oh snap!</strong> Change a few things up and try submitting again.
                            </div>
                            <div class="alert alert-warning" role="alert">
                                <strong>Warning!</strong> Better check yourself, you're not looking too good.
                            </div>--%>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-md-1">
                            <asp:Button ID="btnProcess" Text="Approve" runat="server"  CssClass="btn btn-success" /></div>
                        <div class="col-md-1 text-center"></div>
                        <div class="col-md-1">or </div>
                        <div class="col-md-8">
                            <div class="input-group">
                                <input type="text" class="form-control" name="rtxt" placeholder="if reject it, Please enter the reason" />
                                <span class="input-group-btn">
                                    <asp:Button ID="btnReject" runat="server" Text="Reject" OnClientClick="return check();" CssClass="btn btn-danger" />
                                </span>

                            </div>
                            <!-- /input-group -->


                        </div>
                    </div>


                </div>
            </div>

        </div>
        <script>
            function check()
            {
                var val = $("input[name='rtxt']").val();
                if ($.trim(val) == "")
                {
                   // alert("Please enter the reason");
                  //  return false;
                }

            }

        </script>
        <style>
            .alert {
                padding: 8px;
                margin-bottom: 8px;
            }
        </style>
    </form>
</body>
</html>
