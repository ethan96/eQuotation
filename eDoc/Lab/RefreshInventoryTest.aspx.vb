Public Class RefreshInventoryTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Advantech.Myadvantech.Business.QuoteBusinessLogic.RefreshPartInventoryOfUSAOnline("f35183d2d81b4d9", -1)

    End Sub

End Class