Public Class PipeLine
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim _QID As String = "c5d67005484f43a"
        Dim QM As Advantech.Myadvantech.DataAccess.QuotationMaster = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationMaster(_QID)
        Dim QD As List(Of Advantech.Myadvantech.DataAccess.QuotationDetail) = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationDetail(_QID)

        Dim _DT As DataTable = tbOPBase.dbGetDataTable("MY", "SELECT URL FROM SIEBEL_ACCOUNT WHERE ROW_ID='" & QM.quoteToRowId & "'")

        Dim _URL As String = String.Empty

        If _DT IsNot Nothing AndAlso _DT.Rows.Count > 0 Then
            _URL = _DT.Rows(0).Item("URL").ToString
        End If

        Dim _salesperson As String = QM.salesEmail
        Dim _district As String = QM.DISTRICT

        Me.TextBox_SalesPerson.Text = _salesperson
        Me.TextBox_District.Text = _district
        Me.TextBox_AccountName.Text = QM.quoteToName
        Me.TextBox_AccoutURL.Text = _URL

        Me.GridView1.DataSource = QD
        Me.GridView1.DataBind()


    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim _item As Advantech.Myadvantech.DataAccess.QuotationDetail = CType(e.Row.DataItem, Advantech.Myadvantech.DataAccess.QuotationDetail)
            e.Row.Cells(5).Text = CInt(_item.qty * _item.newUnitPrice)
        End If
    End Sub
End Class