Public Class CheckUIDisCanInsert2Siebel
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Business.isCanInsert2Siebel(Request("UID")) Then
                Util.showMessage("Invalid UID or Status!")
                If Pivot.CurrentProfile.UserId <> "nada.liu@advantech.com.cn" Then
                    Response.Redirect("~/quote/MyQuotationRecord.aspx")
                End If
            End If
        End If
    End Sub

End Class