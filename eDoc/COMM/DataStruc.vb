<Serializable()> _
Public Class Field
    Sub New(ByVal Name As String, ByVal defaultValue As Object, Optional ByVal Value As Object = Nothing)
        Me._Name = Name
        Me._defaultValue = defaultValue
        If IsNothing(Value) Then
            Me.Value = defaultValue
        Else
            Me.Value = _Value
        End If
    End Sub
    Private _Name As String = ""
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property
    Private _Value As Object = Nothing
    Public Property Value As Object
        Get
            Return _Value
        End Get
        Set(ByVal value As Object)
            _Value = value
        End Set
    End Property
    Private _defaultValue As Object = Nothing
    Public ReadOnly Property defaultValue As Object
        Get
            Return _defaultValue
        End Get
    End Property
End Class
