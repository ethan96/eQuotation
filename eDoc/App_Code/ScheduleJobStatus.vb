Public Class ScheduleJobStatus
    Public Shared SiebelJobIsWorking As Boolean = False
    Public Shared jobloglist As List(Of joblog) = Nothing
End Class
Public Class joblog
    Public Property title() As String
    Public Property worktime() As DateTime
End Class


