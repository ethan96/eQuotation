Imports Microsoft.VisualBasic

Public Class PageBase : Inherits System.Web.UI.Page
    Private _UID As String = ""
    Public ReadOnly Property UID As String
        Get
            If Not IsNothing(Request("UID")) Then
                _UID = Request("UID")
            End If
            Return _UID
        End Get
    End Property
    Private _oType As Integer = 0
    Public Property oType As COMM.Fixer.eDocType
        Get
            Dim CurrPageName As String = LCase(Request.ServerVariables("PATH_INFO"))
            If CurrPageName Like "*orderduedate.aspx*" OrElse CurrPageName Like "*orderpreview.aspx*" Then
                Return COMM.Fixer.eDocType.ORDER
            End If
            Return COMM.Fixer.eDocType.EQ
        End Get
        Set(ByVal value As COMM.Fixer.eDocType)
            _oType = value
        End Set
    End Property
    Private _MasterRef As IBUS.iDocHeaderLine = Nothing
    Public ReadOnly Property MasterRef As IBUS.iDocHeaderLine
        Get
            If IsNothing(_MasterRef) Then
                _MasterRef = Pivot.NewObjDocHeader.GetByDocID(UID, oType)
            End If
            Return _MasterRef
        End Get
    End Property
    Private _isRequestValid As Boolean = True
    Public ReadOnly Property isRequestValid As Boolean
        Get
            Return _isRequestValid
        End Get
    End Property
    Protected Overrides Sub InitializeCulture()
        Try
            If Not IsNothing(Pivot.CurrentProfile) Then
                Util.InitializeCulture(Pivot.CurrentProfile.Lung.ToString)
            End If
        Catch ex As Exception

        End Try
        MyBase.InitializeCulture()
    End Sub

    Private Function CheckUID(ByVal UID As String) As Boolean
        Dim Doc As IBUS.iDoc = Pivot.NewObjDoc
        If Not Doc.IsValidQuotationID(UID) Then
            Util.showMessage("Invalid Doc Id,System will be automatically redirected to : <br/><b> 'Quotation Master Page' </b>.", "/Quote/QuotationMaster.aspx")
            Return False
        End If

        If Not Doc.CanUserAccessThisQuotation(UID, User.Identity.Name) Then
            Util.showMessage("Current user is not permitted to visite Doc '" & UID & "',System will be automatically redirected to : <br/><b>'My Quote List Page' </b>.", "/Quote/quoteByAccountOwner.aspx")
            Return False
        End If
        Return True
    End Function

    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        If Not IsPostBack Then
            If Not IsNothing(Request("UID")) AndAlso UID = "" Then
                Util.showMessage("Invalid Doc Id,System will be automatically redirected to : <br/><b> 'Quotation Master Page' </b>.", "/Quote/QuotationMaster.aspx")
                _isRequestValid = False
            End If
            If UID = "" AndAlso Not Request.ServerVariables("SCRIPT_NAME").ToString.ToLower.Contains("quotationmaster.aspx") Then
                Util.showMessage("Doc id cannot be found, System will be automatically redirected to : <br/><b> 'Quotation Master Page'</b>.", "/Quote/QuotationMaster.aspx")
                Me._isRequestValid = False
                Exit Sub
            End If
            If UID <> "" Then
                If Not CheckUID(UID) Then
                    Me._isRequestValid = False
                    Exit Sub
                End If
            End If

            If IsNothing(MasterRef) AndAlso Not Request.ServerVariables("SCRIPT_NAME").ToString.ToLower.Contains("quotationmaster.aspx") Then
                Util.showMessage("Doc header cannot be initialized, System will be automatically redirected to : <br/><b>'Quotation Master Page' </b>.", "/Quote/QuotationMaster.aspx")
                Me._isRequestValid = False
                Exit Sub
            End If
        End If
        MyBase.OnInit(e)
    End Sub
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        If Not IsPostBack AndAlso Not Me.isRequestValid Then
            Exit Sub
        End If
        MyBase.OnLoad(e)
    End Sub
    Protected Overrides Sub OnPreInit(ByVal e As System.EventArgs)
        If Not IsPostBack AndAlso Not Me.isRequestValid Then
            Exit Sub
        End If
        MyBase.OnPreInit(e)
    End Sub
    'Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
    '    If Not IsPostBack AndAlso Not Me.isRequestValid Then
    '        Exit Sub
    '    End If
    '    MyBase.OnPreRender(e)
    'End Sub
    Protected Overrides Sub OnPreLoad(ByVal e As System.EventArgs)
        If Not IsPostBack AndAlso Not Me.isRequestValid Then
            Exit Sub
        End If
        MyBase.OnPreLoad(e)
    End Sub
    Protected Overrides Sub OnLoadComplete(ByVal e As System.EventArgs)
        If Not IsPostBack AndAlso Not Me.isRequestValid Then
            Exit Sub
        End If
        MyBase.OnLoadComplete(e)
    End Sub
    Protected Overrides Sub OnInitComplete(ByVal e As System.EventArgs)
        If Not IsPostBack AndAlso Not Me.isRequestValid Then
            Exit Sub
        End If
        MyBase.OnInitComplete(e)
    End Sub
    Protected Overrides Sub OnDataBinding(ByVal e As System.EventArgs)
        If Not IsPostBack AndAlso Not Me.isRequestValid Then
            Exit Sub
        End If
        MyBase.OnDataBinding(e)
    End Sub
    'Protected Overrides Sub OnPreRenderComplete(ByVal e As System.EventArgs)
    '    If Not IsPostBack AndAlso Not Me.isRequestValid Then
    '        Exit Sub
    '    End If
    '    MyBase.OnPreRenderComplete(e)
    'End Sub
End Class
