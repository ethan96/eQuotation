Public Class Wall
    Inherits System.Web.UI.MasterPage

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not IsNothing(HttpContext.Current) AndAlso (Not IsNothing(HttpContext.Current.User)) AndAlso HttpContext.Current.User.Identity.IsAuthenticated Then
            lblUserId.Text = HttpContext.Current.User.Identity.Name
        Else
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If LCase(Request.ServerVariables("URL")).EndsWith("/wallhome.aspx") Then
                tdLeftMenu.Visible = True
                ltLine.Text = "<hr/>"

                Me.aLogo.HRef = "~/Wall/WallHome.aspx"
                Me.imgLogo.ImageUrl = "~/Images/Logo_AonlineDocCenter.jpg"

                lkbMyAdvantech.Text = "eQuotation"
                tdDefaultHome.Visible = False  'wallhome页面已经有eQuotation
                tableTopLink.Style.Add("width", "350px")
            ElseIf LCase(Request.ServerVariables("URL")).EndsWith("/aonlinehomepageedit.aspx") Then
                Me.aLogo.HRef = "~/Wall/AOnlineHomePageEdit.aspx"
                hyHome.NavigateUrl = "~/Wall/WallHome.aspx"
                hyHome.Text = "AOnline Doc Center"
            ElseIf LCase(Request.ServerVariables("URL")).EndsWith("/home_myaonlinewall.aspx") Then
                lblVersion.Visible = False
                hyMyAOnlineWall.Text = "AOnline Doc Center"
                hyMyAOnlineWall.NavigateUrl = "~/Wall/WallHome.aspx"
            End If

          

        End If

    End Sub

    Private Sub lkbMyAdvantech_Click(sender As Object, e As System.EventArgs) Handles lkbMyAdvantech.Click
        If Not LCase(Request.ServerVariables("URL")).EndsWith("/wallhome.aspx") Then
            Dim TID As String = Pivot.CurrentProfile.SSOID, USER As String = Pivot.CurrentProfile.UserId
            Response.Redirect(String.Format("http://my.advantech.com/ORDER/SSOENTER.ASPX?ID={0}&USER={1}", TID, USER))
        Else
            Response.Redirect("~/Home.aspx")
        End If
        
    End Sub

    Private Sub lbLogOut_Click(sender As Object, e As System.EventArgs) Handles lbLogOut.Click
        Session.Abandon() : Util.ClearCookie_Login("ADEQCOOK")
        Response.Redirect("~/login.aspx")
    End Sub
End Class