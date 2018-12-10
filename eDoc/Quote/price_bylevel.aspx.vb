Public Class price_bylevel
    Inherits System.Web.UI.Page

    Public strCurrT As String, strDollarSign As String, T_strselect As String, titler As String
    Public Function GetSql() As String
        Dim l_strSQLCmd = "select distinct " & _
    "PROD_LINE, " & _
    "PART_NO, " & _
    "PROD_GROUP, " & _
    "CURR, " & _
    "PRICE_COST, " & _
    "MARGIN, " & _
    "PRICE_GP_CTRL, " & _
    "RLP, " & _
      "PRICE_D1, " & _
    "DISCOUNT_D1, " & _
    "case PRICE_D1 when 0 then '--%' else convert(varchar,round((PRICE_D1-FOB_AESC)/PRICE_D1*100,2))+ '%' end as GP_D1, " & _
     "PRICE_D2, " & _
    "DISCOUNT_D2, " & _
    "case PRICE_D2 when 0 then '--%' else convert(varchar,round((PRICE_D2-FOB_AESC)/PRICE_D2*100,2))+ '%' end as GP_D2, " & _
    "PRICE_D3, " & _
    "DISCOUNT_D3, " & _
    "case PRICE_D3 when 0 then '--%' else convert(varchar,round((PRICE_D3-FOB_AESC)/PRICE_D3*100,2))+ '%' end as GP_D3, " & _
    "PRICE_D4, " & _
    "DISCOUNT_D4, " & _
    "case PRICE_D4 when 0 then '--%' else convert(varchar,round((PRICE_D4-FOB_AESC)/PRICE_D4*100,2))+ '%' end as GP_D4, " & _
    "PRICE_D5, " & _
    "DISCOUNT_D5, " & _
    "case PRICE_D5 when 0 then '--%' else convert(varchar,round((PRICE_D5-FOB_AESC)/PRICE_D5*100,2))+ '%' end as GP_D5, " & _
    "FOB_AESC, " & _
    "'' as FOB_ACL " & _
    "from PRICE_LIST_LEVEL "
        'Response.Write(l_strSQLCmd)
        Dim l_strWhere = "where 1=1 and (RLP is not NULL) and (FOB_AESC is not NULL) "
        If Request("curr") = "" Then
            strCurrT = "EUR"
        ElseIf Request("curr") = "EUR','USD','GBP" Then
            strCurrT = "ALL"
        Else
            strCurrT = Request("curr")
        End If
        If Request("curr") <> "" Then
            l_strWhere = l_strWhere + " and CURR in ('" & Request("curr") & "') "
        Else
            l_strWhere = l_strWhere + " and CURR in ('" & strCurrT & "') "
        End If
        Select Case UCase(strCurrT)
            Case "GBP"
                strDollarSign = "&pound;"
            Case "USD"
                strDollarSign = "$"
            Case "EUR"
                strDollarSign = "&euro;"
            Case Else
                strDollarSign = ""
        End Select
        Session("strDollarSign") = strDollarSign
        If Request("part_no") <> "" Then
            l_strWhere = l_strWhere + " and PART_NO = '" & Request("part_no") & "' "
        End If
        If Request("prod_line") <> "" Then
            l_strWhere = l_strWhere + " and PROD_LINE = '" & Request("prod_line") & "' "
        End If
        T_strselect = l_strSQLCmd + l_strWhere + " order by PROD_LINE asc, PART_NO asc"
        Return T_strselect
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.SqlDataSource1.SelectCommand = GetSql()
        If Not Page.IsPostBack Then
            Me.GridView1.DataBind()
            Me.titler = "Price List ( " & strCurrT & " " & Session("strDollarSign") & ")"
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(0).Text = "Prod Ln"
            e.Row.Cells(1).Text = "Part No"
            e.Row.Cells(2).Text = "Grp"
            e.Row.Cells(7).Text = "RLP"
            e.Row.Cells(9).Text = "D1%"
            e.Row.Cells(10).Text = "GP1%"
            e.Row.Cells(12).Text = "D2%"
            e.Row.Cells(13).Text = "GP2%"
            e.Row.Cells(15).Text = "D3%"
            e.Row.Cells(16).Text = "GP3%"
            e.Row.Cells(18).Text = "D4%"
            e.Row.Cells(19).Text = "GP4%"
            e.Row.Cells(21).Text = "D5%"
            e.Row.Cells(22).Text = "GP5%"
            e.Row.Cells(8).Text = "D1" & Session("strDollarSign") 'strDollarSign
            e.Row.Cells(11).Text = "D2" & Session("strDollarSign")
            e.Row.Cells(14).Text = "D3" & Session("strDollarSign")
            e.Row.Cells(17).Text = "D4" & Session("strDollarSign")
            e.Row.Cells(20).Text = "D5" & Session("strDollarSign")
        End If
        If e.Row.RowType <> DataControlRowType.Pager Then
            e.Row.Cells(3).Visible = False
            e.Row.Cells(4).Visible = False
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
            e.Row.Cells(23).Visible = False
            e.Row.Cells(24).Visible = False
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            'part no
            e.Row.Cells(2).Text = "<b>" & UCase(e.Row.Cells(2).Text) & "</b>"

            If Not IsNumeric(e.Row.Cells(9).Text) Then
                e.Row.Cells(9).Text = "--%"
            Else
                e.Row.Cells(9).Text = CInt(e.Row.Cells(9).Text) & "%"
            End If
            If Not IsNumeric(e.Row.Cells(12).Text) Then
                e.Row.Cells(12).Text = "--%"
            Else
                e.Row.Cells(12).Text = CInt(e.Row.Cells(12).Text) & "%"
            End If
            If Not IsNumeric(e.Row.Cells(14).Text) Then
                e.Row.Cells(15).Text = "--%"
            Else
                e.Row.Cells(15).Text = CInt(e.Row.Cells(15).Text) & "%"
            End If
            If Not IsNumeric(e.Row.Cells(18).Text) Then
                e.Row.Cells(18).Text = "--%"
            Else
                e.Row.Cells(18).Text = CInt(e.Row.Cells(18).Text) & "%"
            End If
            If Not IsNumeric(e.Row.Cells(21).Text) Then
                e.Row.Cells(21).Text = "--%"
            Else
                e.Row.Cells(21).Text = CInt(e.Row.Cells(21).Text) & "%"
            End If

            Dim i As Integer = 0, xTempLevel As String = "", price As String = ""
            For i = 10 To 22
                Select Case i
                    Case 10, 13, 16, 19, 22, 24, 27
                        xTempLevel = "D" & i.ToString()
                        price = e.Row.Cells(i - 2).Text

                        Dim fITP = e.Row.Cells(23).Text 'rs("FOB_AESC")
                        Dim fTemp As String = "", fPRICED0 As String = ""
                        If Not IsNumeric(fITP) Or fITP = "0" Then
                            fTemp = "--"
                        ElseIf Not IsNumeric(price) Or price = "0" Then
                            fTemp = "--"
                        Else
                            fPRICED0 = price
                            If fPRICED0 <> 0 Then
                                fTemp = FormatNumber((CDbl(fPRICED0) - CDbl(fITP)) / CDbl(fPRICED0) * 100, 2)
                            End If
                        End If
                        e.Row.Cells(i).Text = fTemp & "%"
                End Select
            Next
            'RLP
            If IsDBNull(e.Row.Cells(7).Text) Or e.Row.Cells(7).Text = "" Then
                e.Row.Cells(7).Text = "--"
            ElseIf Not IsNumeric(e.Row.Cells(7).Text) Then
                e.Row.Cells(7).Text = "--"
            Else
                e.Row.Cells(7).Text = FormatNumber(e.Row.Cells(7).Text, 2)
            End If

            Select Case UCase(e.Row.Cells(3).Text)
                Case "GBP"
                    strDollarSign = ""
                    'strDollarSign= "&pound;"
                Case "USD"
                    strDollarSign = ""
                    'strDollarSign= "&pound;"
                Case "EUR"
                    strDollarSign = ""
                    'strDollarSign= "&pound;"
            End Select
            For i = 6 To 20
                Select Case i
                    Case 6, 8, 11, 14, 18, 20, 25, 28
                        Dim strValue As String = ""
                        strValue = e.Row.Cells(i).Text
                        If IsDBNull(strValue) Or strValue = "" Then
                            e.Row.Cells(i).Text = ""
                        ElseIf Not IsNumeric(strValue) Then
                            e.Row.Cells(i).Text = ""
                        ElseIf CInt(strValue) < 0 Then
                            e.Row.Cells(i).Text = "0.00"
                        Else
                            e.Row.Cells(i).Text = FormatNumber(strValue, 2)
                        End If
                End Select
            Next
            For n As Integer = 0 To e.Row.Cells.Count - 1
                If IsNumeric(IgnoreSign(e.Row.Cells(n).Text.Trim)) Then
                    e.Row.Cells(n).HorizontalAlign = HorizontalAlign.Right
                Else
                    e.Row.Cells(n).HorizontalAlign = HorizontalAlign.Left
                End If
            Next
        End If
    End Sub
    Public Function IgnoreSign(ByVal str As String) As String
        str = Replace(str, "%", "")
        str = Replace(str, "&euro;", "")
        str = Replace(str, "NT", "")
        str = Replace(str, "US$", "")
        str = Replace(str, "&yen;", "")
        str = Replace(str, "&pound;", "")
        Return str
    End Function
    Protected Sub sh(ByVal sender As Object, ByVal e As EventArgs)
        Me.SqlDataSource1.FilterExpression = Me.drpFields.SelectedValue & " like '%" & Me.txtStr.Text.Trim & "%'"
    End Sub
    Protected Sub Export2XLS() Handles BtnSave2Excel.Click
        Dim DT As DataTable = tbOPBase.dbGetDataTable("MyLocal", GetSql())
        Util.DataTable2ExcelDownload(DT, "DxPrice.xls")
    End Sub

End Class