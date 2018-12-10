Public Class commonUtil

    Public Shared Function isWarrantable(ByVal PN As String, ByVal OrgID As String) As Boolean
        If Not String.Equals(OrgID, "US01", StringComparison.CurrentCultureIgnoreCase) Then
            Return True
        End If
        Dim DT As New DataTable
        DT = dbac.getProductByLine(PN)
        If DT.Rows.Count = 0 Then Return True
        If isSoftware(PN, DT) Then
            Return False
        ElseIf PN.StartsWith("AGS") OrElse PN.StartsWith("OPTION") Then
            Return False
        ElseIf DT.Rows(0).Item("PRODUCT_LINE") IsNot Nothing AndAlso DT.Rows(0).Item("PRODUCT_LINE") = "ASS#" Then
            Return False
        Else
            Return True
        End If
    End Function
    Public Shared Function isSoftware(ByVal PN As String, ByRef DT As DataTable) As Boolean
        'Dim DT As New DataTable
        'DT = dbac.getProductByLine(PN)
        'If DT.Rows.Count = 0 Then Return True
        If PN.StartsWith("96SW") OrElse PN.StartsWith("98MQ") OrElse PN.StartsWith("968Q") Then
            Return True
        ElseIf DT.Rows(0).Item("PRODUCT_LINE") IsNot Nothing AndAlso (DT.Rows(0).Item("PRODUCT_LINE") = "EPCS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "EDOS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WCOM" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WAUT" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "DAAS") Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
