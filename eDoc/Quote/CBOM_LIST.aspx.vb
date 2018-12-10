Public Class CBOM_LIST
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    
        If Not IsPostBack Then
            Dim dt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
            If Not IsNothing(dt) Then
                Dim org As String = Business.getCBOMorg(UID, COMM.Fixer.eDocType.EQ)

                hd_OrgId.Value = org
                Dim dtS As DataTable = Business.getCbomListByCatalogAndOrg(_CatalogType, org, dt.AccErpId, dt.siebelRBU)
                If String.Equals(org, "TW", StringComparison.CurrentCultureIgnoreCase) Then
                    Dim pns As List(Of String) = New List(Of String)
                    For Each i As DataRow In dtS.Rows
                        If i.Item("catalog_name") IsNot Nothing AndAlso Not String.IsNullOrEmpty(i.Item("catalog_name")) Then
                            pns.Add("'" + i.Item("catalog_name") + "'")
                        End If
                    Next
                    Dim sql As String = String.Format("select  PART_NO  from dbo.SAP_PRODUCT_STATUS_ORDERABLE where SALES_ORG='{0}'  and PART_NO in(" + String.Join(",", pns.ToArray()) + ") ", "TW01")
                    Dim dtValid As DataTable = tbOPBase.dbGetDataTable("MY", sql)
                    If dtValid.Rows.Count > 0 AndAlso dtS.Rows.Count > 0 Then
                        Dim dtscopy As DataTable = dtS.Clone()
                        For Each i As DataRow In dtS.Rows
                            Dim pn As DataRow() = dtValid.Select(String.Format("PART_NO='{0}'", i.Item("catalog_name")))
                            If pn.Length > 0 Then
                                Dim dr As DataRow = dtscopy.NewRow
                                dr.Item("CATALOG_ID") = i.Item("CATALOG_ID")
                                dr.Item("CATALOG_NAME") = i.Item("CATALOG_NAME")
                                dr.Item("CATALOG_TYPE") = i.Item("CATALOG_TYPE")
                                dr.Item("CATALOG_ORG") = i.Item("CATALOG_ORG")
                                dr.Item("CATALOG_DESC") = i.Item("CATALOG_DESC")
                                dr.Item("IMAGE_ID") = i.Item("IMAGE_ID")
                                dr.Item("UID") = i.Item("UID")
                                dtscopy.Rows.Add(dr)
                            End If
                        Next
                        dtS = dtscopy
                    End If
                    If dtValid.Rows.Count = 0 Then
                        dtS = dtS.Clone()
                    End If
                End If
                If dtS.Rows.Count > 0 Then
                    Me.AdxGrid1.DataSource = dtS
                End If
                If Not IsPostBack Then
                    Me.AdxGrid1.DataBind()
                End If

            End If

        End If
        'If _CatalogType = "CUSTOM" Then
        '    initSBC()
        'End If
    End Sub
    'Sub initSBC()
    '    '20120724 Rudy: Hide ACP-BTO, IPC-BTO, PPC-BTO
    '    Dim STR() As String = {"TPC-BTO", "IPPC-BTO", "UNO-BTO", "ARK-BTO", "TREK-BTO", "SBC-BTO", "UTC-BTO", "ACP-BTO", "IPC-BTO", "PPC-BTO"}
    '    For Each X As String In STR
    '        Dim hlSBC As New LinkButton
    '        hlSBC.Text = X
    '        AddHandler hlSBC.Click, AddressOf SBCCLICK
    '        plSBC.Controls.Add(hlSBC)
    '        plSBC.Controls.Add(New LiteralControl("<BR/><BR/>"))
    '    Next
    'End Sub

    'Delegate Sub SBCCLICK(ByVal str As String)
    'Sub SBCCLICK(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim obj As LinkButton = CType(sender, LinkButton)
    '    If obj.Text.ToUpper = "ARK-BTO" Or obj.Text.ToUpper = "ACP-BTO" Or obj.Text.ToUpper = "IPC-BTO" Then
    '        Dim myQD As New quotationDetail("EQ", "quotationDetail")
    '        myQD.Delete(String.Format("quoteId='{0}'", Request("UID")))
    '        Business.ADD2QUOTE(Request("UID"), obj.Text.ToUpper, 1, 0, -1, "", 0, 0)
    '        Response.Redirect("~/quote/QuotationDetail.aspx?VIEW=0&UID=" & Request("UID"))
    '    Else
    '        Me.txtFilter.Text = obj.Text.ToUpper.Split("-")(0)
    '        Dim dt As New DataTable
    '        dt = tbOPBase.dbGetDataTable("Estore", String.Format("select distinct BTONo from Product_Ctos where StoreID='aus' and BTONo<>'AndyTestProduct' order by BTONo"))
    '        If dt.Rows.Count > 0 Then

    '            Me.gvBtosList.DataSource = dt
    '            Me.gvBtosList.DataBind()

    '            'Me.btnConfigBList.Height = Me.lsbBList.Height
    '            Me.MPPickBList.Show()
    '        End If
    '    End If
    'End Sub

    'Sub GoConfig(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim obj As LinkButton = CType(sender, LinkButton)
    '    Dim myQD As New quotationDetail("EQ", "quotationDetail")
    '    myQD.Delete(String.Format("quoteId='{0}'", Request("UID")))
    '    Business.ADD2QUOTE(Request("UID"), obj.ID.ToUpper, 1, 0, -1, "", 0, 0)
    '    Response.Redirect("~/quote/QuotationDetail.aspx?VIEW=0&UID=" & Request("UID"))
    'End Sub
    Dim _CatalogType As String = ""
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        _CatalogType = Server.UrlDecode(Request("Catalog_Type"))
        If _CatalogType <> "CTOS" And _CatalogType <> "Pre-Configuration" Then
            'Me.IMAGE_ID.xVisible = True
        Else
            'Me.IMAGE_ID.xVisible = False
        End If
    End Sub

    Protected Sub AdxGrid1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

    End Sub




    Protected Sub rdFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        AdxGrid1.DataBind()
    End Sub

    Protected Sub btnConfig_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As String = CType(row.FindControl("hd_RowCatalogName"), HiddenField).Value
        'Response.Write("<xml>ID1:" + id + "</xml>")
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("B2B").ConnectionString)
        Dim dt As New DataTable, apt As New SqlClient.SqlDataAdapter("select top 1 CATEGORY_ID from CBOM_CATALOG_CATEGORY where CATEGORY_ID=@CATID and ORG=@ORGID", conn)
        apt.SelectCommand.Parameters.AddWithValue("CATID", id) : apt.SelectCommand.Parameters.AddWithValue("ORGID", hd_OrgId.Value)
        apt.Fill(dt)
        If dt.Rows.Count = 0 Then
            id = CType(row.FindControl("hd_RowImgId"), HiddenField).Value
            'Response.Write("<xml>ID2:" + id + "</xml>")
            dt = New DataTable
            apt = New SqlClient.SqlDataAdapter("select top 1 CATEGORY_ID from CBOM_CATALOG_CATEGORY where CATEGORY_ID=@CATID and ORG=@ORGID", conn)
            apt.SelectCommand.Parameters.AddWithValue("CATID", id) : apt.SelectCommand.Parameters.AddWithValue("ORGID", hd_OrgId.Value)
            'Response.Write("<xml>ID2:" + apt.SelectCommand.Parameters("CATID").Value + "," + apt.SelectCommand.Parameters("ORGID").Value + "</xml>")
            If conn.State <> ConnectionState.Open Then conn.Open()
            apt.Fill(dt)
            If dt.Rows.Count = 0 Then
                id = ""
            End If
            'Response.Write("<xml>ID3:" + id + "</xml>")
        End If
        If conn.State <> ConnectionState.Closed Then conn.Close()
        If String.IsNullOrEmpty(id) AndAlso hd_OrgId.Value.Equals("JP", StringComparison.InvariantCultureIgnoreCase) Then
            hd_OrgId.Value = "TW"
            btnConfig_Click(sender, Nothing)
        End If
        If String.IsNullOrEmpty(id) Then
            Exit Sub
        End If
        Dim intQty As Integer = 1
        intQty = CType(row.FindControl("txtQty"), TextBox).Text
        Dim str As String = "~/quote/Configurator.aspx?BTOITEM=" & id & "&QTY=" & intQty & "&UID=" & UID
        Response.Redirect(str)
    End Sub

    'Protected Sub btnConfigBList_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim myQD As New quotationDetail("EQ", "quotationDetail")
    '    myQD.Delete(String.Format("quoteId='{0}'", Request("UID")))
    '    'Business.ADD2QUOTE(Request("UID"), Me.lsbBList.SelectedValue, 1, 0, -1, "", 0, 0)
    '    Response.Redirect("~/quote/QuotationDetail.aspx?VIEW=0&UID=" & Request("UID"))
    'End Sub

End Class