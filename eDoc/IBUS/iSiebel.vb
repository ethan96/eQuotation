Public Interface iSiebel : Inherits iBase
    Function UpdateOpportunityStatusRevenue(ByVal OptRowId As String, ByVal status As String, ByVal Revenue As Double, ByVal ConnectToACL As Boolean) As Boolean

End Interface
