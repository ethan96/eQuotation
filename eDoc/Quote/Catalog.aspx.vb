Public Class Catalog
    Inherits PageBase
    Public _IsUSAonline As Boolean = False

    Protected Sub BuildBtosServiceCenter(ByVal UID As String)

        Dim BtosDT As DataTable = Business.getCatalogByUID(UID, COMM.Fixer.eDocType.EQ)


        Dim i As Integer = 0
        Me.p_catalog.Controls.Add(New LiteralControl("<table style=""" & _
                                            "color:#000099;font-weight:bold;font-size:90%;""" & _
                                            "width=""100%"" border=""0"">"))

        'Ryan 20160629 Hide Catalog for USAonline
        If Not _IsUSAonline Then
            Do While i <= BtosDT.Rows.Count - 1

                Dim HL_CBOM_CTR As New HyperLink
                HL_CBOM_CTR.Text = BtosDT.Rows(i).Item("Catalog_Type")
                If BtosDT.Rows(i).Item("Catalog_Type").ToString.ToUpper = "IPPC" Then
                    HL_CBOM_CTR.Text = "Industrial Panel PC (IPPC)"
                End If
                If BtosDT.Rows(i).Item("Catalog_Type").ToString.ToUpper = "Tablet PC" Then
                    HL_CBOM_CTR.Text = "Tablet PC (MARS)"
                End If
                If BtosDT.Rows(i).Item("Catalog_Type").ToString.ToUpper = "PRE-CONFIGURATION " Then
                    If Role.IsJPAonlineSales() Then
                        HL_CBOM_CTR.Text = "eStore Configuration"
                    Else
                        HL_CBOM_CTR.Text = "Pre-Configuration for AEU eStore (buy.advantech.eu) Configuration"
                    End If

                End If
                Me.p_catalog.Controls.Add(New LiteralControl("<tr style=""height:15px"">" & _
                                                    "<td style=""padding-left:10px;width:10px;padding-top:1px;""><img alt="""" src=""../images/plus.gif"" /></td>" & _
                                                    "<td>"))
                HL_CBOM_CTR.NavigateUrl = "~/quote/CBOM_List.aspx?Catalog_Type=" & Server.UrlEncode(BtosDT.Rows(i).Item("Catalog_Type")) & "&UID=" & UID
                Me.p_catalog.Controls.Add(HL_CBOM_CTR)
                Me.p_catalog.Controls.Add(New LiteralControl("</td></tr>"))
                i = i + 1
            Loop
        End If
        'ICC 20180122 For AEU quote. Hide eStore BTOS link.
        If Not Role.IsEUSales() Then
            Dim estore_HL_CBOM_CTR As New HyperLink
            estore_HL_CBOM_CTR.Text = "eStore BTOS"
            Me.p_catalog.Controls.Add(New LiteralControl("<tr style=""height:15px"">" & _
                                               "<td style=""padding-left:10px;width:10px;padding-top:1px;""><img alt="""" src=""../images/plus.gif"" /></td>" & _
                                               "<td>"))
            estore_HL_CBOM_CTR.NavigateUrl = "~/quote/EstoreCBOMCategory.aspx?UID=" & UID
            Me.p_catalog.Controls.Add(estore_HL_CBOM_CTR)
        End If
        If Left(Business.getOrgByQuoteId(UID, COMM.Fixer.eDocType.EQ), 2) = "US" _
          OrElse Left(Business.getOrgByQuoteId(UID, COMM.Fixer.eDocType.EQ), 2) = "JP" Then
            Dim Custom_HL_CBOM_CTR As New HyperLink
            Custom_HL_CBOM_CTR.Text = "Custom BTOS"
            Me.p_catalog.Controls.Add(New LiteralControl("<tr style=""height:15px"">" & _
                                               "<td style=""padding-left:10px;width:10px;padding-top:1px;""><img alt="""" src=""../images/plus.gif"" /></td>" & _
                                               "<td>"))
            Custom_HL_CBOM_CTR.NavigateUrl = "~/quote/CustomBtos.aspx?Catalog_Type=CUSTOM&UID=" & UID
            Me.p_catalog.Controls.Add(Custom_HL_CBOM_CTR)
        End If

        Me.p_catalog.Controls.Add(New LiteralControl("</td></tr>"))
        Me.p_catalog.Controls.Add(New LiteralControl("</table>"))
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _IsUSAonline = Role.IsAonlineUsa()
        BuildBtosServiceCenter(UID)
    End Sub

End Class