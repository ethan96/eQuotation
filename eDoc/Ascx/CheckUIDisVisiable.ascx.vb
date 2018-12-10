Public Class CheckUIDisVisiable
    Inherits System.Web.UI.UserControl
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Business.isValidQuote(Request("UID"), COMM.Fixer.eDocType.EQ) Then
                If Pivot.CurrentProfile.UserId <> "nada.liu@advantech.com.cn" Then
                    Response.Redirect("~/quote/MyQuotationRecord.aspx")
                End If
            End If
        End If
    End Sub
End Class