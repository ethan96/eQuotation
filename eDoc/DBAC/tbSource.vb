Imports COMM
Public Class tbSource
    Sub New(ByVal conn As String, ByVal tbName As String)
        Me._conn = conn
        Me._tbName = tbName
    End Sub
    Sub New(ByVal AppArea As Fixer.eCartAppArea)
        Select Case AppArea
            Case Fixer.eCartAppArea.EQ
                Me._conn = "EQ"
                Me._tbName = "QuotationDetail"
                'Case Fixer.eCartAppArea.Cart
                '    Me._conn = "MY"
                '    Me._tbName = "Cart_Detail"
            Case Fixer.eCartAppArea.Order
                Me._conn = "EQ"
                Me._tbName = "OrderDetail"
        End Select
    End Sub
    Private _conn As String
    Private _tbName As String
    Public Property conn As String
        Get
            Return _conn
        End Get
        Set(ByVal value As String)
            _conn = value
        End Set
    End Property
    Public Property tbName As String
        Get
            Return _tbName
        End Get
        Set(ByVal value As String)
            _tbName = value
        End Set
    End Property
End Class
