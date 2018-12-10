Public Class tempSyncPN
    Inherits System.Web.UI.Page


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim TID As String = Pivot.CurrentProfile.SSOID
        Dim USER As String = Pivot.CurrentProfile.UserId
        Dim company As String = "EDDEVI07"
        Dim RURL As String = HttpContext.Current.Server.UrlEncode(String.Format("/Admin/SyncSingleProduct.aspx"))
        Response.Redirect(String.Format("http://my.advantech.com/ORDER/SSOENTER.ASPX?ID={0}&USER={1}&COMPANY={2}&RURL={3}", TID, USER, company, RURL))
    End Sub

End Class