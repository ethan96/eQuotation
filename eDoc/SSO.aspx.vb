Public Class SSO2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Request("id")) AndAlso Not String.IsNullOrEmpty(Request("id")) _
            AndAlso Not IsNothing(Request("tempid")) AndAlso Not String.IsNullOrEmpty(Request("tempid")) _
            AndAlso COMM.Util.IsInternalUser(Request("id")) Then
            Dim id As String = Request("id"), tempid As String = Request("tempid")
            If Pivot.NewObjPSSO.isValidSSOMember(COMM.Util.GetClientIP(), tempid, id) Then
                Pivot.NewObjProfile.setSession(id, "en-US", tempid)
            End If
            FormsAuthentication.SetAuthCookie(id, False)
            If Not IsNothing(Request("ReturnUrl")) AndAlso Request("ReturnUrl") <> "" Then Response.Redirect(Request("ReturnUrl"))
            Response.Redirect("~/home.aspx")
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

End Class