Public Class USPriceOverWrite1
    Inherits System.Web.UI.Page

    Function getData(ByVal QuoteNo As String, ByVal Revision As String) As IBUS.iCartList
        Dim Qm As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByQuoteNoandRevisionNumber(QuoteNo, Revision, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(Qm) Then
            Dim L As IBUS.iCartList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, Qm.Key, Qm.org).GetListAll(COMM.Fixer.eDocType.EQ)
            Me.hOrg.Value = Qm.org
            Me.hQuoteId.Value = Qm.Key
            If Not IsNothing(L) Then
                Return L
            End If
        End If
        Return Nothing
    End Function
    Function getRevisionList(ByVal QuoteNo As String) As ArrayList
        Dim Qm As IBUS.iDocHeader = Pivot.NewObjDocHeader()
        Return Qm.GetRevisionsAr(QuoteNo)
    End Function



    Private Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Me.gv1.DataSource = getData(Me.txtQuoteId.Text.Trim, Me.drpRevision.SelectedValue)
        gv1.DataBind()
        Me.MPPickRevsion.Hide()
    End Sub
    Public Sub onPriceChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As TextBox = CType(sender, TextBox)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        'Dim lineNo As Integer = Integer.Parse(CType(row.FindControl("HFlineNo"), HiddenField).Value.Trim())
        'Dim DBITEM As IBUS.iCartLine = CType(row.DataItem, IBUS.iCartLine)
        Dim id As Integer = CType(Me.gv1.DataKeys(row.RowIndex).Value, COMM.Field).Value
        Dim CustPN As String = obj.Text.Trim
        Dim c As IBUS.iCart(Of IBUS.iCartLine) = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, Me.hQuoteId.Value, Me.hOrg.Value)
        c.UpdateByLine(String.Format("{0}=N'{1}'", c.Item1.unitPrice.Name, CustPN), id, COMM.Fixer.eDocType.EQ)
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Me.txtQuoteId.Text.Trim = "" Then
            Me.lbMsg.Text = "Please input a Quote No first."
            Exit Sub
        End If
        Dim QID As String = txtQuoteId.Text.Trim
        Dim ar As ArrayList = getRevisionList(QID)
        If Not IsNothing(ar) AndAlso ar.Count > 0 Then
            Dim QM As IBUS.iDocHeader = Pivot.NewObjDocHeader
            Dim ARN As String = QM.GetActiveRevisionNo(QID)
            Me.drpRevision.Items.Clear()
            For Each R As String In ar
                Me.drpRevision.Items.Add(New ListItem(R, R))
            Next
            For Each LI As ListItem In Me.drpRevision.Items
                If LI.Value = ARN Then
                    LI.Selected = True
                End If
            Next
            Me.UPPickRevsion.Update()
            Me.MPPickRevsion.Show()
        End If
    End Sub
End Class