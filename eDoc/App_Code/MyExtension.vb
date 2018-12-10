Public Module ExtensionMethods
    <System.Runtime.CompilerServices.Extension> _
    Public Function StartsWithV2(_context As String, ParamArray _args As String()) As Boolean
        For Each i As String In _args
            If _context.StartsWith(i, StringComparison.InvariantCultureIgnoreCase) Then
                Return True
                Exit For
            End If
        Next
        Return False
    End Function
End Module
