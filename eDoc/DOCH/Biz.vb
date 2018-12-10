Public Class Biz
    Public Shared Function isCustomerCompleteDeliv(ByVal CompanyId As String, ByVal OrgId As String) As Boolean
        CompanyId = UCase(CompanyId) : OrgId = UCase(OrgId)
        Dim str As String = String.Format("select KUNNR from sapRDP.KNVV WHERE KUNNR='{0}' AND MANDT='168' and vkorg='{1}' and KZTLF='C'", CompanyId, OrgId)
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", str)
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function


End Class
