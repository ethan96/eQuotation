Public Class SSOENTER
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsNothing(Request("ID")) AndAlso Request("ID") <> "" Then
            If Not IsNothing(Request("USER")) AndAlso Request("USER") <> "" Then
                Dim ID As String = Request("ID")
                Dim USER As String = Request("USER")
                If Pivot.NewObjPSSO.isValidSSOId(ID, USER) OrElse _
                    Pivot.NewObjPSSO.isValidSSOMember(Request.ServerVariables("REMOTE_ADDR"), ID, USER) Then

                    'Frank 20140127
                    If Not Pivot.NewObjProfile.setSession(USER, "en-US", ID) Then
                        Dim _msg As String = COMM.errMsg.getErrMsg(2 ^ 11)
                        Response.Write(_msg)
                        Response.End()
                    End If

                    FormsAuthentication.SetAuthCookie(USER, False)

                    If Not IsNothing(Request("RURL")) AndAlso Request("RURL") <> "" Then
                        Response.Redirect(Request("RURL"))
                    Else
                        Response.Redirect("~/home.aspx")
                    End If
                Else
                    Response.Redirect("~/login.aspx")
                End If
            Else
                Response.Redirect("~/login.aspx")
            End If
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

End Class