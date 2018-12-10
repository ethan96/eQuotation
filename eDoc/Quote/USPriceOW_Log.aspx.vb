Public Class USPriceOW_Log
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not Page.IsPostBack Then
        '    Dim _sql As String = " SELECT QuoteNo,QuoteRevision,Line_No,oPrice,nPrice,eDate,eBy FROM QUOTELINEPRICEMODIFYLOG "
        '    Dim _where As String = " Where "
        '    Dim _order As String = " Order by eDate desc "

        '    If String.IsNullOrEmpty(Me.txtQuoteId.Text) Then
        '        Me.SqlDataSource1.SelectCommand = _sql & _order
        '    Else
        '        _where &= " QuoteNo like '%" & Me.txtQuoteId.Text.Replace("'", "''") & "%' "
        '        Me.SqlDataSource1.SelectCommand = _sql & _where & _order
        '    End If
        '    Me.SqlDataSource1.DataBind()
        '    Me.gv1.DataBind()

        'End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        'Dim _sql As String = " SELECT QuoteNo,QuoteRevision,Line_No,oPrice,nPrice,eDate,eBy FROM QUOTELINEPRICEMODIFYLOG "
        'Dim _where As String = " Where "
        'Dim _order As String = " Order by eDate desc "

        'If String.IsNullOrEmpty(Me.txtQuoteId.Text) Then
        '    Me.SqlDataSource1.SelectCommand = _sql & _order
        'Else
        '    _where &= " QuoteNo like '%" & Me.txtQuoteId.Text.Replace("'", "''") & "%' "
        '    Me.SqlDataSource1.SelectCommand = _sql & _where & _order
        'End If
        'Me.SqlDataSource1.DataBind()
        'Me.gv1.DataBind()
    End Sub
End Class