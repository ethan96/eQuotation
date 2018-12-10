Imports Advantech.Myadvantech.Business

Public Class SendeQuotationUI
    Inherits System.Web.UI.UserControl

    Public Property QuoteID As String
        Get
            Return Me.txtQuoteIdFW.Text
        End Get
        Set(ByVal value As String)

            'Me.txtQuoteIdFW.Text = value
            Dim QuoteDt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(value, COMM.Fixer.eDocType.EQ)
            Me.txtQuoteIdFW.Text = QuoteDt.quoteNo


            Me.QuotationViewOptionUC1.QuoteID = value

    
        End Set
    End Property

    Public ReadOnly Property AttachedFiles As List(Of HttpPostedFile)
        Get

            Dim _returnval As New List(Of HttpPostedFile)

            Dim _arraycount As Integer = 0
            If Me.upload_your_file1 IsNot Nothing _
                AndAlso Me.upload_your_file1.PostedFile IsNot Nothing _
                AndAlso Me.upload_your_file1.PostedFile.ContentLength > 0 Then
                _returnval.Add(Me.upload_your_file1.PostedFile)
            End If
            If Me.upload_your_file2 IsNot Nothing _
                AndAlso Me.upload_your_file2.PostedFile IsNot Nothing _
                AndAlso Me.upload_your_file2.PostedFile.ContentLength > 0 Then
                _returnval.Add(Me.upload_your_file2.PostedFile)
            End If
            If Me.upload_your_file3 IsNot Nothing _
                AndAlso Me.upload_your_file3.PostedFile IsNot Nothing _
                AndAlso Me.upload_your_file3.PostedFile.ContentLength > 0 Then
                _returnval.Add(Me.upload_your_file3.PostedFile)
            End If
            If Me.upload_your_file4 IsNot Nothing _
                AndAlso Me.upload_your_file4.PostedFile IsNot Nothing _
                AndAlso Me.upload_your_file4.PostedFile.ContentLength > 0 Then
                _returnval.Add(Me.upload_your_file4.PostedFile)
            End If

            Return _returnval
        End Get
    End Property


    Public Property RecipientEmail As String
        Get
            Return Me.txtEmail.Text
        End Get
        Set(ByVal value As String)
            Me.txtEmail.Text = value
        End Set
    End Property

    Public Property Subject As String
        Get
            Return Me.txtFwdSubject.Text
        End Get
        Set(ByVal value As String)
            Me.txtFwdSubject.Text = value
        End Set
    End Property

    Public Property ContentType As EnumSetting.QuotationForwardType
        Get

            If Me.rbtnFW.SelectedValue.Equals("HTM", StringComparison.InvariantCultureIgnoreCase) Then
                Return EnumSetting.QuotationForwardType.HTML
            Else
                Return EnumSetting.QuotationForwardType.PDF
            End If

        End Get
        Set(ByVal value As EnumSetting.QuotationForwardType)
            Select Case value
                Case EnumSetting.QuotationForwardType.HTML
                    Me.rbtnFW.SelectedValue = "HTM"
                Case Else
                    Me.rbtnFW.SelectedValue = "PDF"
            End Select
        End Set
    End Property

    Public Property EmailGreeting As String
        Get
            Return Me.txtemailgreeting.Content
        End Get
        Set(ByVal value As String)
            Me.txtemailgreeting.Content = value
        End Set
    End Property

    Private _IsJPAonline As Boolean = False, _IsTWAonline As Boolean = False, _IsKRAonline As Boolean = False
    Private _IsIntercon As Boolean = False

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me._IsTWAonline = Role.IsTWAonlineSales()
        Me._IsKRAonline = Role.IsKRAonlineSales()
        Me._IsIntercon = Role.IsHQDCSales
        txtEmail.Focus()
        If Not IsPostBack Then



            If _IsTWAonline OrElse _IsKRAonline Then
                Me.trAttachedFiles.Visible = True

                If _IsTWAonline Then Me.trDisplayFormat.Visible = True
                If _IsKRAonline Then Me.trDisplayFormat.Visible = False

                'ICC 2015/3/20
                '1. ATW mail subject from QuotationMaster.customId
                '2. ATW user can save signature and show signature in page load
                Me.ButtonSaveSignature.Visible = True
                If Not Request("QuoteID") Is Nothing AndAlso Not String.IsNullOrEmpty(Request("QuoteID").ToString()) Then
                    Dim _QuoteID As String = Request("QuoteID").ToString()
                    Dim qMaster As Advantech.Myadvantech.DataAccess.QuotationMaster = QuoteBusinessLogic.GetQuotationMaster(_QuoteID)
                    If Not qMaster Is Nothing AndAlso Not String.IsNullOrEmpty(qMaster.customId) Then
                        txtFwdSubject.Text = txtFwdSubject.Text.Replace(QuoteID, qMaster.customId)
                    End If
                End If

                Dim _DefaultSignature As String = QuoteBusinessLogic.GetDefaultSignature(Pivot.CurrentProfile.UserId)
                Me.txtemailgreeting.Content = _DefaultSignature
            End If

            If _IsIntercon Then
                Me.trAttachedFiles.Visible = True
                Me.trDisplayFormat.Visible = True
                Me.ButtonSaveSignature.Visible = True
                Dim _DefaultSignature As String = QuoteBusinessLogic.GetDefaultSignature(Pivot.CurrentProfile.UserId)
                Me.txtemailgreeting.Content = _DefaultSignature
            End If

        End If
    End Sub


    Protected Sub ButtonSaveSignature_Click(sender As Object, e As EventArgs) Handles ButtonSaveSignature.Click
        QuoteBusinessLogic.SaveDefaultSignature(Pivot.CurrentProfile.UserId, Me.txtemailgreeting.Content)
    End Sub
End Class