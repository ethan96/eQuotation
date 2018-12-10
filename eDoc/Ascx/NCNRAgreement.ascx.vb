Public Class WebUserControl1
    Inherits System.Web.UI.UserControl
    Public _QuoteId As String = String.Empty

    Public Property QuoteId As String
        Get
            Return _QuoteId
        End Get
        Set(ByVal value As String)
            _QuoteId = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Public Sub LoadData()
        Me.lbCustomer.Text = _QuoteId
        Me.lbOrderNo.Text = ""
        Me.lbReason.Text = ""
    End Sub

End Class