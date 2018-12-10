Public Class quickConfigQuote
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UID As String = Business.GetNewUID()


        Response.Redirect("~/quote/catalog.aspx?UID=" & UID)
    End Sub

End Class