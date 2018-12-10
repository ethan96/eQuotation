Public Class Conditions
    Inherits System.Web.UI.UserControl
    Private _UID As String = String.Empty
    Public Property UID As String
        Get
            Return _UID
        End Get
        Set(ByVal value As String)
            _UID = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class