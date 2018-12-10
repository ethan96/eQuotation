Public Class CSSO : Implements IBUS.iSSO

    Public Sub logSSOId(ByVal tempId As String, ByVal userId As String, ByVal pw As String, ByVal ipAddr As String) Implements IBUS.iSSO.logSSOId
        Dim myloginLog As New loginLog()
        myloginLog.Add(tempId, userId, pw, ipAddr)
    End Sub
    Public Function isValidSSOId(ByVal tempId As String, ByVal userId As String) As Boolean Implements IBUS.iSSO.isValidSSOId
        Dim myloginLog As New loginLog()
        If myloginLog.IsExists(String.Format("tempId='{0}' and userId='{1}'", tempId, userId)) = 1 Then
            Return True
        End If
        Return False
    End Function
    Public Function isValidSSOMember(ByVal localIP As String, ByVal TempId As String, ByVal eMail As String) As Boolean Implements iSSO.isValidSSOMember
        Dim sso As New PSSO.MembershipWebservice
        Return sso.validateTemidEmail(localIP, TempId, "MY", eMail)
    End Function
    Private _errCode As COMM.Msg.eErrCode
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property
End Class
