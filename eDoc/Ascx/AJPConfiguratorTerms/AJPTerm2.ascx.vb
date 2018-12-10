Public Class AJPTerm2
    Inherits System.Web.UI.UserControl
    Dim UID As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Role.IsJPAonlineSales Then
            If (Not Request("UID") Is Nothing AndAlso Not String.IsNullOrEmpty(Request("UID").ToString)) Then
                UID = Request("UID").ToString
            End If
            SetValuetoForm()
        End If
    End Sub

    Public Sub SetValuetoForm()


    End Sub

End Class