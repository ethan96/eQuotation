Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ForeStore
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetQuotationMasterByQuoteID(ByVal quoteID As String) As Advantech.Myadvantech.DataAccess.Quote
        Dim master As Advantech.Myadvantech.DataAccess.QuotationMaster = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationMasterForeStore(quoteID, Advantech.Myadvantech.DataAccess.QuoteDocStatus.Finish)
        If Not master Is Nothing Then
            Dim quote As New Advantech.Myadvantech.DataAccess.Quote()
            quote.QuoteID = master.quoteId
            quote.QuoteNo = master.quoteNo
            quote.CustomID = master.customId
            quote.AttentionEmail = master.attentionEmail
            quote.CreatedDate = master.createdDate
            quote.ExpiredDate = master.expiredDate

            If master.org = "TW01" Then
                quote.TotalAmount = MyQuoteX.GetATWTotalAmount(master.quoteId)
            Else
                quote.TotalAmount = master.Revenue
            End If

            quote.DoctStatus = master.DOCSTATUS
            quote.Currency = master.currency
            Return quote
        End If
        Return Nothing
    End Function

    '<WebMethod()> _
    'Public Function GetQuotationByCustomerEmail(ByVal email As String) As List(Of Advantech.Myadvantech.DataAccess.Quote)
    '    Dim list As List(Of Advantech.Myadvantech.DataAccess.QuotationMaster) = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationMasterByCustomerEmail(email, Advantech.Myadvantech.DataAccess.QuoteDocStatus.Finish)
    '    Return list.Select(Function(p) New Advantech.Myadvantech.DataAccess.Quote With _
    '                                                   {.QuoteID = p.quoteId, .QuoteNo = p.quoteNo, .CreatedDate = p.createdDate, .ExpiredDate = p.expiredDate, .TotalAmount = p.Revenue, .DoctStatus = p.DOCSTATUS, .Currency = p.currency}).ToList()
    'End Function

    <WebMethod()> _
    Public Function GetQuotationTotalByQuoteID(ByVal quoteID As String) As Advantech.Myadvantech.DataAccess.Quote
        Dim master As Advantech.Myadvantech.DataAccess.QuotationMaster = Advantech.Myadvantech.Business.QuoteBusinessLogic.GetQuotationMasterForeStore(quoteID, Advantech.Myadvantech.DataAccess.QuoteDocStatus.Finish)
        If Not master Is Nothing Then
            Dim quote As Advantech.Myadvantech.DataAccess.Quote = New Advantech.Myadvantech.DataAccess.Quote(master)
            If master.org = "TW01" Then quote.TotalAmount = MyQuoteX.GetATWTotalAmount(master.quoteId)
            Return quote
        End If
        Return Nothing
    End Function
End Class