Public Class Login
    Inherits System.Web.UI.Page

    'Dim myTI As New TempInfo("EQ", "tbTempInfo")
    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        'Ryan 20161020 Add for Joe Neary case
        Dim sendername As String = CType(sender, Button).ID
        System.Web.HttpContext.Current.Session("Joe") = sendername
        'End Ryan 20161020 Add


        Dim msg As String = ""
        If Pivot.login(Me.txtUser.Text.Trim.Replace("'", "''"), Me.txtPass.Text.Trim, Me.drpLang.SelectedValue, msg, Me.cbxKeep.Checked) Then
            Dim user As String = Me.txtUser.Text.Trim.Replace("'", "''")
            FormsAuthentication.RedirectFromLoginPage(user, False)
        Else
            Me.lbmsg.Text = msg
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If User.Identity.IsAuthenticated Then
            Response.Redirect("~/home.aspx")
        End If

        ''Frank 20170220 :Winnie decides to let dora wa has 2 group's permission control
        'If User.Identity.Name.Equals("dora.wu@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then
        '    Me.Joebtn_1.Text = "Sales.ATW.AOL-ATC(IIoT)"
        '    Me.Joebtn_2.Text = "Sales.ATW.AOL-EC"
        'End If


        'If Util.IsValidCookie_Login("ADEQCOOK") Then
        '    Dim COOK As HttpCookie = Request.Cookies("ADEQCOOK")
        '    If COOK.Value.Contains("|") Then
        '        Dim temp As String() = COOK.Value.Split("|")
        '        Dim userName As String = temp(0)
        '        Dim passWord As String = temp(1)
        '        If Pivot.login(userName, passWord, Me.drpLang.SelectedValue) Then
        '            If Not IsNothing(Request("RURL")) AndAlso Request("RURL") <> "" Then
        '                Response.Redirect(HttpContext.Current.Server.UrlDecode(Request("RURL")))
        '            End If
        '        End If
        '    End If
        'End If

    End Sub
End Class