Public Class RyanTest
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AJP.QuoteId = "e0aee66625e24e5"
        AJP.LoadData()
    End Sub


End Class