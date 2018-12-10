Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
<Script.Services.ScriptService> _
Public Class GetQuoteForeStore
    Inherits System.Web.Services.WebService



    <WebMethod()> _
    Public Function GetQuotationByCustomerEmail(ByVal email As String) As List(Of Advantech.Myadvantech.DataAccess.Quote)
        'Dim list As List(Of Advantech.Myadvantech.DataAccess.QuotationMaster) = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationMasterByCustomerEmail(email)
        'If Not list Is Nothing Then
        '    Return list.Select(Function(x) New MyTestQuote With {
        '                           .CreatedDate = x.createdDate, .ExpiredDate = x.expiredDate, .QuoteID = x.quoteId, .TotalAmount = x.Revenue, .QuoteNo = x.quoteNo}).ToList()
        'End If

        'Dim masters As List(Of QuotationMaster) = QuoteBusinessLogic.GetQuotationMasterByCustomerEmail(email)
        'If Not masters Is Nothing Then Return masters.Select(Function(q) New Quote(q, Advantech.Myadvantech.DataAccess.GetQuoteForeStore.QuotationMaster)).ToList()


        Return New List(Of Advantech.Myadvantech.DataAccess.Quote)


    End Function

    <WebMethod()> _
    Public Function GetQuoteByQuoteID(ByVal quoteID As String) As Advantech.Myadvantech.DataAccess.Quote
        'Dim master As Advantech.Myadvantech.DataAccess.QuotationMaster = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationMaster(quoteID)
        'If Not master Is Nothing Then Return New MyQuote(master)
        'If Not master Is Nothing Then Return New Quote(master, Advantech.Myadvantech.DataAccess.GetQuoteForeStore.QuotationAll)


        Return New Advantech.Myadvantech.DataAccess.Quote()
    End Function
End Class

'<Serializable()> _
'Public Class MyQuote
'    Sub New()

'    End Sub

'    Sub New(ByVal master As Advantech.Myadvantech.DataAccess.QuotationMaster)
'        Me.QuotationMaster = New MyTestQuote(master)
'        Me.QuotationDetail = master.quoteDetailList
'        Me.QuotationExtension = master.QuoteExtensionX
'    End Sub

'    Private qm As MyTestQuote
'    Public Property QuotationMaster As MyTestQuote
'        Get
'            Return qm
'        End Get
'        Set(ByVal value As MyTestQuote)
'            qm = value
'        End Set
'    End Property

'    Private qd As List(Of Advantech.Myadvantech.DataAccess.QuotationDetail)
'    Public Property QuotationDetail As List(Of Advantech.Myadvantech.DataAccess.QuotationDetail)
'        Get
'            Return qd
'        End Get
'        Set(ByVal value As List(Of Advantech.Myadvantech.DataAccess.QuotationDetail))
'            qd = value
'        End Set
'    End Property

'    Private qe As Advantech.Myadvantech.DataAccess.QuotationExtension
'    Public Property QuotationExtension As Advantech.Myadvantech.DataAccess.QuotationExtension
'        Get
'            Return qe
'        End Get
'        Set(ByVal value As Advantech.Myadvantech.DataAccess.QuotationExtension)
'            qe = value
'        End Set
'    End Property

'End Class

'<Serializable()> _
'Public Class MyTestQuote
'    Sub New()

'    End Sub
'    Sub New(ByVal master As Advantech.Myadvantech.DataAccess.QuotationMaster)
'        Me.CreatedDate = master.createdDate
'        Me.ExpiredDate = master.expiredDate
'        Me.QuoteNo = master.quoteNo
'        Me.QuoteID = master.quoteId
'        Me.TotalAmount = master.Revenue
'    End Sub

'    Private ID As String
'    Public Property QuoteID As String
'        Get
'            Return ID
'        End Get
'        Set(ByVal value As String)
'            ID = value
'        End Set
'    End Property

'    Private No As String
'    Public Property QuoteNo As String
'        Get
'            Return No
'        End Get
'        Set(ByVal value As String)
'            No = value
'        End Set
'    End Property

'    Private created_Date As DateTime
'    Public Property CreatedDate As DateTime
'        Get
'            Return created_Date
'        End Get
'        Set(ByVal value As DateTime)
'            created_Date = value
'        End Set
'    End Property

'    Private exp_Date As DateTime
'    Public Property ExpiredDate As DateTime
'        Get
'            Return exp_Date
'        End Get
'        Set(ByVal value As DateTime)
'            exp_Date = value
'        End Set
'    End Property

'    Private total_Amount As Decimal
'    Public Property TotalAmount As Decimal
'        Get
'            Return total_Amount
'        End Get
'        Set(ByVal value As Decimal)
'            total_Amount = value
'        End Set
'    End Property

'    Private ship_To As String
'    Public Property ShipTo As String
'        Get
'            Return ship_To
'        End Get
'        Set(ByVal value As String)
'            ship_To = value
'        End Set
'    End Property

'    Private q_Status As String
'    Public Property QuoteStatus As String
'        Get
'            Return q_Status
'        End Get
'        Set(ByVal value As String)
'            q_Status = value
'        End Set
'    End Property

'End Class