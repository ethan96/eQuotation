Imports Microsoft.VisualBasic

Public Class Franchise
    Shared Function getOrgStrByUser(ByVal userid As String) As String
        Dim str As String = String.Format("select OrgStr from FranchiseUser where userid='{0}'", userid)
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("EQ", str)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("OrgStr")
        End If
        Return ""
    End Function

    Shared Function isFranchiser() As Boolean
        Return Role.IsFranchiser()
    End Function

    Shared Function getSampleERPidForFranchise(ByVal USER As String) As String
        Return getFranchCompanyId(USER)
    End Function
    Public Shared Function getFranchCompanyId(ByVal UserId As String) As String
        Dim str As String = String.Format("SELECT SALES_CODE,COMPANY_ID,EMAIL FROM FRANCHISER WHERE (EMAIL = '{0}')", UserId)
        Dim _dt As New DataTable
        _dt = tbOPBase.dbGetDataTable("MY", str)
        If _dt.Rows.Count > 0 Then
            Return _dt.Rows(0).Item("COMPANY_ID").ToString
        End If
        Return ""
    End Function

End Class
