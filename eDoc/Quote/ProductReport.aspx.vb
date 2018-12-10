Public Class ProductReport1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("partno") IsNot Nothing Then
                LitPartNo.Text = Request("partno")
                GridView1.DataSource = tbOPBase.dbGetDataTable("EQ", GetData())
                GridView1.DataBind()
            End If
        End If
    End Sub

    Public Function GetData() As String
        If Request("partno") IsNot Nothing Then
            Dim partno As String = LitPartNo.Text.Trim()
            Dim Y As String = Request("y")
            Dim MF As String = Request("mf")
            Dim MT As String = Request("mt")
            Dim ORG As String = Request("org")
            Dim RBU As String = Request("rbu")
            Dim sb As New StringBuilder()
            sb.AppendFormat(" select M.QuoteNo, ISNULL(O.SO_NO,'') AS OrderNo, D.Qty, M.quoteToErpId AS ERPID,M.quoteToName AS CompanyName, M.SalesEmail,M.CreatedBy,  CONVERT(varchar(100), M.quoteDate, 103) as  CreatedDate")
            sb.AppendFormat(" from  QuotationDetail D  ")
            sb.AppendFormat(" inner join  QuotationMaster M  on M.quoteId = D.quoteId  ")
            sb.AppendFormat(" left join QUOTE_TO_ORDER_LOG O on M.quoteId = O.QUOTEID  ")
            'sb.AppendFormat(" where D.partNo='{0}' AND  (M.quoteNo like 'GQ%' or M.quoteNo like 'AUSQ%' or M.quoteNo like 'AMXQ%'   or M.quoteNo like 'AACQ%')  and M.office<>'' and M.SalesEmail<>'' and M.DOCSTATUS=" & CInt(COMM.Fixer.eDocStatus.QFINISH) & "  ", partno)
            sb.AppendFormat(" where D.partNo='{0}' AND  ( M.quoteNo like 'AUSQ%' or M.quoteNo like 'AMXQ%' or M.quoteNo like 'AACQ%') and M.DOCSTATUS=" & CInt(COMM.Fixer.eDocStatus.QFINISH) & "  ", partno)
            If Not String.IsNullOrEmpty(Y) Then
                sb.AppendFormat(" and  year(M.quoteDate)={0} ", Y)
            End If
            If Not String.IsNullOrEmpty(MF) Then
                sb.AppendFormat("  and Month(M.quoteDate)>={0}  ", MF)
            End If
            If Not String.IsNullOrEmpty(MT) Then
                sb.AppendFormat("  and Month(M.quoteDate)<={0} ", MT)
            End If
            If Not String.IsNullOrEmpty(ORG) Then
                sb.AppendFormat("   and M.Org='{0}' ", ORG)
            End If
            If Not String.IsNullOrEmpty(RBU) Then
                sb.AppendFormat("   and M.office ='{0}' ", RBU)
            End If
            ' sb.AppendFormat(" left join  QuotationMaster M  on M.quoteId = D.quoteId ")
            ' sb.AppendFormat(" where D.partNo='{0}'  ", partno.Replace("'", "''"))
            'sb.AppendFormat(" and (M.quoteno like 'AUSQ%' OR M.quoteno like 'AMXQ%') ")
            'sb.AppendFormat(" group by D.partNo,m.quoteToErpId,M.quoteToName  ")
            sb.AppendFormat(" order by M.quoteDate desc  ")
            'Response.Write(sb.ToString())  

            Return sb.ToString
        End If
    End Function


    Protected Sub ibtnExcel_Click(sender As Object, e As ImageClickEventArgs)
        Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", GetData())

        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
            Util.DataTable2ExcelDownload(dt, LitPartNo.Text)
        End If
    End Sub

End Class