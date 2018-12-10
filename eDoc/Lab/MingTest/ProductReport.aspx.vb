Public Class ProductReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
           
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sb As New StringBuilder()
        sb.AppendFormat(" select D.partNo,sum(D.qty) AS TotalQty, ")
        sb.AppendFormat(" sum(D.unitprice)*sum(D.qty) as ToalPrice, ")
        sb.AppendFormat(" M.quoteToErpId AS ERPID,M.quoteToName AS CompanyName ")
        sb.AppendFormat(" from  QuotationDetail D ")
        sb.AppendFormat(" left join  QuotationMaster M  on M.quoteId = D.quoteId ")
        sb.AppendFormat(" where D.partNo='{0}'  ", TBpartno.Text.Replace("'", "''"))
        sb.AppendFormat(" and (M.quoteno like 'AUSQ%' OR M.quoteno like 'AMXQ%') ")
        sb.AppendFormat(" group by D.partNo,m.quoteToErpId,M.quoteToName  ")
        sb.AppendFormat(" order by M.quoteToName  ")
        GridView1.DataSource = tbOPBase.dbGetDataTable("EQ", sb.ToString())
        GridView1.DataBind()
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.RowIndex > 0 Then
                e.Row.Cells(0).Text = ""
            End If
        End If
    End Sub
End Class