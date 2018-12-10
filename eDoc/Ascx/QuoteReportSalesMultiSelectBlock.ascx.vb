Public Class QuoteReportSalesMultiSelectBlock
    Inherits System.Web.UI.UserControl

    Public Property SalesArrInit As ArrayList
        Get
            Dim A As New ArrayList
            For Each r As ListItem In Me.cbxSales.Items
                A.Add(r.Value.ToString)
            Next
            If A.Count > 0 Then
                Return A
            End If
            Return Nothing
        End Get
        Set(ByVal value As ArrayList)
            Me.cbxSales.Items.Clear()
            For Each r As Object In value
                Me.cbxSales.Items.Add(New ListItem(r.ToString, r.ToString))
            Next
        End Set
    End Property

    Public Sub ClearList()
        Me.cbxSales.Items.Clear()
    End Sub

    Public Property SalesArr As ArrayList
        Get
            Dim A As New ArrayList
            For Each r As ListItem In Me.cbxSales.Items
                If r.Selected Then
                    A.Add(r.Value.ToString)
                End If
            Next
            If A.Count > 0 Then
                Return A
            End If
            Return Nothing
        End Get
        Set(ByVal value As ArrayList)
            For Each r As Object In value
                For Each rc As ListItem In Me.cbxSales.Items
                    If String.Equals(rc.Value, r.ToString, StringComparison.OrdinalIgnoreCase) Then
                        rc.Selected = True
                    End If
                Next
            Next
        End Set
    End Property
   
End Class