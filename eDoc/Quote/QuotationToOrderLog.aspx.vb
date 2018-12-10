Public Class QuotationToOrderLog
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim _QuoteId As String = Request.Params("QuoteId")
            '_QuoteId = "GQ002335" '<-This QuoteID is a good testing sample because it relates to multi order.
            Me.Label_QoutationID.Text = _QuoteId
            Dim _dt As DataTable = tbOPBase.dbGetDataTable("EQ", Business.getQuoteToOrderLog(_QuoteId, False))

            Me.GridView_Order_Log.DataSource = _dt
            Me.GridView_Order_Log.DataBind()

        End If
    End Sub
End Class