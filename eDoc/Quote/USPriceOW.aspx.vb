Imports Microsoft.VisualBasic
Public Class USPriceOW
    Inherits System.Web.UI.Page



    Function getData(ByVal QuoteNo As String, ByVal Revision As String) As IBUS.iCartList
        Dim Qm As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByQuoteNoandRevisionNumber(QuoteNo, Revision, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(Qm) Then
            Dim L As IBUS.iCartList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, Qm.Key, Qm.org).GetListAll(COMM.Fixer.eDocType.EQ)
            Me.hOrg.Value = Qm.org
            Me.hQuoteId.Value = Qm.Key
            Me.hQNO.Value = Qm.quoteNo
            Me.hRID.Value = Qm.Revision_Number
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



    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click

        Me.gv1.DataSource = getData(Me.txtQuoteId.Text.Trim, Me.drpRevision.SelectedValue)

        Me.gv1.DataBind()

        'Me.MPPickRevsion.Hide()
    End Sub

    Protected Sub txtUnitPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As System.Web.UI.WebControls.TextBox = CType(sender, TextBox)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim id As Integer = Integer.Parse(CType(row.FindControl("txtLineNo"), TextBox).Text.Trim())
        'Dim DBITEM As IBUS.iCartLine = CType(row.DataItem, IBUS.iCartLine)
        Dim P As Decimal = obj.Text.Trim
        Dim c As IBUS.iCart(Of IBUS.iCartLine) = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, Me.hQuoteId.Value, Me.hOrg.Value)
        Dim cl As IBUS.iCartLine = c.Item(id)
        c.UpdateByLine(String.Format("{0}=N'{1}'", c.Item1.newunitPrice.Name, P), id, COMM.Fixer.eDocType.EQ)
        logauditingrec(Me.hQNO.Value, Me.hRID.Value, id, cl.newunitPrice.Value, P)

        Dim _lbLineUpdateMsg As Label = CType(row.FindControl("lbLineUpdateMsg"), Label)
        _lbLineUpdateMsg.Text = "Updated!"
    End Sub


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Me.lbMsg.Text = "" : Me.btnUpdate.Enabled = True
        If Me.txtQuoteId.Text.Trim = "" Then
            Me.lbMsg.Text = "Please input a Quote No first."
            Me.gv1.DataSource = Nothing : Me.gv1.DataBind() : Me.btnUpdate.Enabled = False
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
    Function logauditingrec(ByVal QNO As String, ByVal RID As String, ByVal LineNo As String, ByVal oPrice As Decimal, ByVal nPrice As Decimal) As Boolean
        If oPrice <> nPrice Then
            Dim str As String = String.Format("insert into QUOTELINEPRICEMODIFYLOG VALUES('{0}','{1}','{2}','{3}','{4}',getdate(),'{5}')", QNO, RID, LineNo, oPrice, nPrice, Pivot.CurrentProfile.UserId)
            tbOPBase.dbExecuteNoQuery("EQ", str)
            Return True
        End If
        Return False
    End Function


    Private Sub gv1_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv1.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim _txtLineNo As TextBox = CType(e.Row.FindControl("txtLineNo"), TextBox)
            Dim _LineNo As Integer = CInt(_txtLineNo.Text)

            If _LineNo >= 100 AndAlso (_LineNo Mod 100 = 0) Then
                CType(e.Row.FindControl("txtUnitPrice"), TextBox).Enabled = False
            End If

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'Hello Jay, 
        'Per Fei, please provide Fei and me the access right to allow leveling pricing per ERP ID and/or quote.
        'Gabriela has a new customer that will need to be quoted L4 pricing
        'Regards, 
        'Denise Kwong

        'Hi Frank,
        'Can you also grant Denise access to eQuotation price change function too?
        'Thanks,
        'Jay

        'Hi Cathee
        'Since I have been questioned by Ween on our margin drop by AOnline, I need to have Jay reverse all the GP block authority to me.
        'Starting from today.
        'Thanks, 
        'Fei

        'Hi Frank,
        'Can you temporarily remove Cathee’s access at overwriting eQuotation price and grant this access to Fei?
        'Thanks,
        'Jay

        'If Pivot.CurrentProfile.UserId.ToUpper.Contains(("Fei.Khong").ToUpper) OrElse _
        '      Pivot.CurrentProfile.UserId.ToUpper.Contains(("Denise.Kwong").ToUpper) OrElse _
        '      Pivot.CurrentProfile.UserId.ToUpper.Contains(("Jay.Lee").ToUpper) OrElse _
        '      Pivot.CurrentProfile.UserId.ToUpper.Contains(("Gary.Lee").ToUpper) OrElse _
        '      Role.IsAdmin Then
        If Role.IsAUSpowerUser(Pivot.CurrentProfile.UserId) Then
        Else
            Response.Redirect("~/home.aspx")
        End If
    End Sub
End Class