Public Class MyUtil
    Private _context As HttpContext = HttpContext.Current
    Public Sub New()
    End Sub
    Public Shared ReadOnly Property Current() As MyUtil
        Get
            If HttpContext.Current Is Nothing Then
                Return Nothing
            End If
            If HttpContext.Current.Items("MyUtil") Is Nothing Then
                Dim _MyDC As New MyUtil()
                HttpContext.Current.Items.Add("MyUtil", _MyDC)
                Return _MyDC
            End If
            Return DirectCast(HttpContext.Current.Items("MyUtil"), MyUtil)
        End Get
    End Property
    Private _CurrentLing2Sql As Ling2SqlDataContext
    Public Property CurrentDataContext() As Ling2SqlDataContext
        Get
            If _CurrentLing2Sql Is Nothing Then
                _CurrentLing2Sql = New Ling2SqlDataContext()
            End If
            Return _CurrentLing2Sql
        End Get
        Set(value As Ling2SqlDataContext)
            _CurrentLing2Sql = value
        End Set
    End Property
    Dim _CurrentOrgID As Object = Nothing
    Public ReadOnly Property CurrentOrgID() As String
        Get
            If _CurrentOrgID Is Nothing Then
                _CurrentOrgID = Pivot.CurrentProfile.getCurrOrg
            End If
            Return _CurrentOrgID
        End Get
    End Property
    Public Shared Function GetOrgStyle() As String
        Dim sb As New StringBuilder
        sb.Append("<style>")
        Dim org As String = MyUtil.Current.CurrentOrgID.Substring(0, 2)
        sb.AppendFormat(".{0}show{{ display:inline-block !important;}}", org) 'inline
        sb.AppendFormat(".{0}hide{{ display:none !important;}}", org)
        sb.Append("</style>")
        Return sb.ToString
    End Function

    Public Property Item(ByVal key As String) As Object
        Get
            If Me._context Is Nothing Then
                Return Nothing
            End If
            If Me._context.Items(key) IsNot Nothing Then
                Return Me._context.Items(key)
            End If
            Return Nothing
        End Get
        Set(ByVal value As Object)
            If Me._context IsNot Nothing Then
                Me._context.Items.Remove(key)

                Me._context.Items.Add(key, value)
            End If
        End Set
    End Property
End Class
