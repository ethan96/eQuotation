Public Class Siebel : Implements IBUS.iSiebel

    Private _errCode As COMM.Msg.eErrCode = Msg.eErrCode.OK
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property

    Public Function UpdateOpportunityStatusRevenue(ByVal OptRowId As String, ByVal status As String, ByVal Revenue As Double, ByVal ConnectToACL As Boolean) As Boolean Implements IBUS.iSiebel.UpdateOpportunityStatusRevenue
        Dim wsOpty As New SBWS.Siebel_WS
        wsOpty.Timeout = -1
        wsOpty.UseDefaultCredentials = True
        Return wsOpty.UpdateOpportunityStatusRevenue(OptRowId, "Won", Revenue, False)
    End Function
End Class
