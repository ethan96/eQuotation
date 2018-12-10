Public Class OrderFromCombinedQuotation
    Inherits System.Web.UI.Page

    Protected Sub btnCombine_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim OrderNo As String = Me.txtRefId.Text
        If Business.isOrderable(OrderNo) Then
            Me.lboxId.Items.Add(Me.txtRefId.Text)
        Else
            Util.showMessage("Invalid Quote Id!")
        End If
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.lboxId.Items.Remove(Me.lboxId.SelectedItem)
    End Sub

    Protected Sub btnOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each x As ListItem In Me.lboxId.Items
            Response.Write(x.Text)
        Next
    End Sub

    Protected Sub ibtnPickQuote_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.ascxPickQuote.ShowData("")
        Me.UPPickQuote.Update()
        Me.MPPickQuote.Show()
    End Sub
    Public Sub PickQuoteFinishEnd(ByVal str As Object)
        Dim quoteId As String = str(0).ToString
        Me.txtRefId.Text = quoteId
        Me.UPForm.Update()
        Me.MPPickQuote.Hide()
    End Sub
End Class