Public Class CMApprovalDetail
    Public Property IsApproved As Boolean : Public Property ApprovalComment As String
    Public Sub New()
        IsApproved = False : ApprovalComment = ""
    End Sub
End Class
