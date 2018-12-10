Public Class UIUtil
    Public Shared Function GetAscxStr(ByVal M As IBUS.iDocHeaderLine, ByVal TypeInt As Integer) As String
        Dim path As String = ""
        If TypeInt = 0 Then
            path = "~/ASCX/Order/soldtoshipto.ascx"
        ElseIf TypeInt = 1 Then
            path = "~/ASCX/Order/OrderInfo.ascx"
        End If
        If String.IsNullOrEmpty(path) Then Return ""
        Dim pageHolder As New TBBasePage()
        pageHolder.IsVerifyRender = False
        Dim cw1 As UserControl = CType(pageHolder.LoadControl(path), UserControl)
        Dim viewControlType As Type = cw1.GetType
        Dim Ma As Reflection.PropertyInfo = viewControlType.GetProperty("QM")
        Ma.SetValue(cw1, M, Nothing)
        pageHolder.Controls.Add(cw1)
        Dim output As New IO.StringWriter()
        HttpContext.Current.Server.Execute(pageHolder, output, False)
        Return output.ToString
    End Function
End Class
