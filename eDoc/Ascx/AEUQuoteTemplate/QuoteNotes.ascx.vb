Public Class QuoteNotes
    Inherits System.Web.UI.UserControl
    Private _UID As String = String.Empty
    Public Property UID As String
        Get
            Return _UID
        End Get
        Set(ByVal value As String)
            _UID = value
        End Set
    End Property
    Private _QM As IBUS.iDocHeaderLine = Nothing
    Public ReadOnly Property QM As IBUS.iDocHeaderLine
        Get
            Return Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not IsNothing(QM) Then
                LitNotes.Text = QM.quoteNote.Replace(vbCrLf, "<br/>")
                Dim QuoteExtensionline As IBUS.iDOCHeaderExtensionLine = Pivot.NewObjDocHeaderExtension.GetQuoteExtension(UID)
                If Not IsNothing(QuoteExtensionline) Then
                    With QuoteExtensionline
                        LitTandC.Text = .SpecialTandC.Replace(vbCrLf, "<br/>")
                    End With
                End If
            End If

            'Frank 2013/10/13
            Dim _isNullLitTandC As Boolean = False, _isNullLitNotes As Boolean = False
            If String.IsNullOrEmpty(LitTandC.Text.Trim) Then _isNullLitTandC = True
            If String.IsNullOrEmpty(LitNotes.Text.Trim) Then _isNullLitNotes = True

            If _isNullLitTandC AndAlso _isNullLitNotes Then
                Me.table1.Visible = False
            ElseIf _isNullLitTandC AndAlso Not _isNullLitNotes Then
                Me.trSandCTitle.Visible = False : Me.trSandC.Visible = False
            ElseIf Not _isNullLitTandC AndAlso _isNullLitNotes Then
                Me.trQnoteNoteTitle.Visible = False : Me.trQnoteNote.Visible = False
            End If


        End If
    End Sub

End Class