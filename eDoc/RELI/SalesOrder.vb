

Partial Public Class SalesOrder
    Partial Class PartnerAddressesDataTable

        Private Sub PartnerAddressesDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.Adr_NotesColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
