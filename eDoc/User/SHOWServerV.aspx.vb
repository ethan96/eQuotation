Public Class SHOWServerV
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each s As String In Request.ServerVariables.AllKeys()
            Response.Write(s)
            Response.Write(" <font color=""#f0000"">|</font> ")
            Response.Write(Request.ServerVariables.Item(s))
            Response.Write("<br/>")
        Next
    End Sub

End Class