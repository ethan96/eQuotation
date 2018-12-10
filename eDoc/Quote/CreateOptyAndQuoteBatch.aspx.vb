Public Class CreateOptyAndQuoteBatch
    Inherits System.Web.UI.Page

    Protected Sub btnSingle_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'OP_SiebelTools.CreateQuotationbyQuoteid(Me.txtQuoteId.Text)
        OP_SiebelTools.CreateOptyByQuoteId(Me.txtQuoteId.Text, "QUOTE")
    End Sub

    Protected Sub btnBatch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ST As New OP_SiebelTools
        ST.CreateAccountContactQuotationFromSPR()
    End Sub

End Class