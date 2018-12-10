Public Class MarginReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub imgDownload_Click(sender As Object, e As ImageClickEventArgs) Handles imgDownload.Click
        imgDownload.Enabled = False
        Dim sql As String = " select distinct lg.SO_NO, qm.quoteNo, qm.quoteToErpId, qm.quoteToName, qm.quoteId, CONVERT(varchar(8),qm.createdDate, 112), qm.createdBy, qm.currency, " & _
            " (select sum(newunitprice*qty) from quotationDetail where quotationDetail.quoteid=qm.quoteid) as [Total Amount], " & _
            " (select sum(newItp*qty) from quotationDetail where quotationDetail.quoteid=qm.quoteid) as [ITP] " & _
            " from QuotationMaster qm LEFT JOIN QUOTE_TO_ORDER_LOG lg ON qm.quoteId = lg.QUOTEID " & _
            " INNER JOIN MyAdvantechGlobal.dbo.CARTMASTERV2 c2 ON qm.quoteId = c2.QuoteID " & _
            " INNER JOIN MyAdvantechGlobal.dbo.Cart2OrderMaping co ON c2.CartID = co.CartID " & _
            " where qm.org like 'EU%' AND YEAR(qm.createdDate)=2015 and ISNULL(co.OrderNo,'') <> ''  "
        Dim dt As DataTable = tbOPBase.dbGetDataTable("eq", sql)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim result As New DataTable
            result.Columns.Add("SO NO", GetType(String))
            result.Columns.Add("Quote NO", GetType(String))
            result.Columns.Add("ERP ID", GetType(String))
            result.Columns.Add("Account name", GetType(String))
            result.Columns.Add("Margin", GetType(Decimal))
            result.Columns.Add("Total amount", GetType(String))
            result.Columns.Add("ITP", GetType(String))
            result.Columns.Add("Create date", GetType(String))
            result.Columns.Add("Create by", GetType(String))
            For Each dr As DataRow In dt.Rows
                Dim margin As Decimal = Business.getMargin(dr.Item(4).ToString)
                'Margin belows 10% 
                If margin < 10 Then
                    Dim cur As String = dr.Item(7).ToString.Trim + " "
                    Dim row As DataRow = result.NewRow()
                    row.Item(0) = dr.Item(0).ToString 'SO
                    row.Item(1) = dr.Item(1).ToString 'Quote
                    row.Item(2) = dr.Item(2).ToString 'ERP ID
                    row.Item(3) = dr.Item(3).ToString 'Account Name
                    row.Item(4) = margin 'Margin
                    row.Item(5) = cur + dr.Item(8).ToString 'Total amount
                    row.Item(6) = cur + dr.Item(9).ToString ' 'ITP
                    row.Item(7) = dr.Item(5).ToString 'Date
                    row.Item(8) = dr.Item(6).ToString 'User
                    result.Rows.Add(row)
                End If
            Next
            result.DefaultView.Sort = "Margin asc"
            Util.DataTable2ExcelDownload(result.DefaultView.ToTable, "AEU_Below_10_Margin")
        End If
        imgDownload.Enabled = True
    End Sub
End Class