Public Class Pivot

    Friend Shared CurrentProfile As Object

    Public Shared Function NewObjSAPTools() As IBUS.iSAP
        Dim P As IBUS.iSAP = New EXSY.SAP()
        Return P
    End Function
    Public Shared Function NewObjSiebelTools() As IBUS.iSiebel
        Dim P As IBUS.iSiebel = New EXSY.Siebel
        Return P
    End Function
    Public Shared Function NewObjPatch() As IBUS.iPatch
        Dim P As IBUS.iPatch = New PATC.PATC
        Return P
    End Function
End Class
