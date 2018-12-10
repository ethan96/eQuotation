Public Class HTMLEditor
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If NewHeight <> 0 Then
                editorFeature.Height = NewHeight
            End If
            If NewWidth <> 0 Then
                editorFeature.Width = NewWidth
            End If
        End If

        lblCurrentMaxMinByte.Text = String.Format("MinLength: {0} / MaxLength: {1} byte)", MixLengthX, MaxLengthX)
    End Sub

    Public Sub SetHtmlEditor(ByVal content As String)
        editorFeature.Content = content
        hfEditorFeature.Value = GetHtmlEditor()
    End Sub

    Public Function GetHtmlEditor() As String
        Return editorFeature.Content
    End Function

    Private _MaxLengthX As Integer = 500
    Public Property MaxLengthX() As Integer
        Get
            Return _MaxLengthX
        End Get
        Set(ByVal value As Integer)
            _MaxLengthX = value
        End Set
    End Property

    Private _MixLengthX As Integer = 0
    Public Property MixLengthX() As Integer
        Get
            Return _MixLengthX
        End Get
        Set(ByVal value As Integer)
            _MixLengthX = value
        End Set
    End Property

    Private _newHeight As Integer
    Public Property NewHeight() As Integer
        Get
            Return _newHeight
        End Get
        Set(ByVal value As Integer)
            _newHeight = value
        End Set
    End Property

    Private _newWidth As Integer
    Public Property NewWidth() As Integer
        Get
            Return _newWidth
        End Get
        Set(ByVal value As Integer)
            _newWidth = value
        End Set
    End Property

End Class