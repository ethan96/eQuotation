Public Class TestServicePart
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim aaa As Boolean = Advantech.Myadvantech.Business.PartBusinessLogic.IsServicePart("INSTALL-OS-64BIT", "US01")
        Dim bbb As Boolean = Advantech.Myadvantech.Business.PartBusinessLogic.IsServicePart("ADAM-4520-EE", "US01")
        Dim ccc = 1
    End Sub

End Class