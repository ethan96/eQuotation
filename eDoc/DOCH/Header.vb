Imports System.Configuration
Imports System.Text
Public Class HeaderLine : Implements iDocHeaderLine

    Private _Key As String = ""
    Public Property Key As String Implements iDocHeaderLine.Key
        Get
            Return _Key
        End Get
        Set(ByVal value As String)
            _Key = value
        End Set
    End Property
    Private _CustomId As String = ""
    Public Property CustomId As String Implements iDocHeaderLine.CustomId
        Get
            Return _CustomId
        End Get
        Set(ByVal value As String)
            _CustomId = value
        End Set
    End Property
    Private _AccErpId As String = ""
    Public Property AccErpId As String Implements iDocHeaderLine.AccErpId
        Get
            Return _AccErpId
        End Get
        Set(ByVal value As String)
            _AccErpId = value
        End Set
    End Property
    Private _DocDate As Date = Now.Date.ToShortDateString
    Public Property DocDate As Date Implements iDocHeaderLine.DocDate
        Get
            Return _DocDate
        End Get
        Set(ByVal value As Date)
            _DocDate = value
        End Set
    End Property
    Private _CreatedDate As DateTime = Now.Date.ToShortDateString
    Public Property CreatedDate As DateTime Implements iDocHeaderLine.CreatedDate
        Get
            Return _CreatedDate
        End Get
        Set(ByVal value As DateTime)
            _CreatedDate = value
        End Set
    End Property
    Private _CreatedBy As String = ""
    Public Property CreatedBy As String Implements iDocHeaderLine.CreatedBy
        Get
            Return _CreatedBy
        End Get
        Set(ByVal value As String)
            _CreatedBy = value
        End Set
    End Property
    Private _AccRowId As String = ""
    Public Property AccRowId As String Implements iDocHeaderLine.AccRowId
        Get
            Return _AccRowId
        End Get
        Set(ByVal value As String)
            _AccRowId = value
        End Set
    End Property
    Private _AccName As String = ""
    Public Property AccName As String Implements iDocHeaderLine.AccName
        Get
            Return _AccName
        End Get
        Set(ByVal value As String)
            _AccName = value
        End Set
    End Property
    Private _AccOfficeName As String = ""
    Public Property AccOfficeName As String Implements iDocHeaderLine.AccOfficeName
        Get
            Return _AccOfficeName
        End Get
        Set(ByVal value As String)
            _AccOfficeName = value
        End Set
    End Property
    Private _AccOfficeCode As String = ""
    Public Property AccOfficeCode As String Implements iDocHeaderLine.AccOfficeCode
        Get
            Return _AccOfficeCode
        End Get
        Set(ByVal value As String)
            _AccOfficeCode = value
        End Set
    End Property
    Private _AccGroupName As String = ""
    Public Property AccGroupName As String Implements iDocHeaderLine.AccGroupName
        Get
            Return _AccGroupName
        End Get
        Set(ByVal value As String)
            _AccGroupName = value
        End Set
    End Property
    Private _AccGroupCode As String = ""
    Public Property AccGroupCode As String Implements iDocHeaderLine.AccGroupCode
        Get
            Return _AccGroupCode
        End Get
        Set(ByVal value As String)
            _AccGroupCode = value
        End Set
    End Property
    Private _salesEmail As String = ""
    Public Property salesEmail As String Implements iDocHeaderLine.salesEmail
        Get
            Return _salesEmail
        End Get
        Set(ByVal value As String)
            _salesEmail = value
        End Set
    End Property
    Private _salesRowId As String = ""
    Public Property salesRowId As String Implements iDocHeaderLine.salesRowId
        Get
            Return _salesRowId
        End Get
        Set(ByVal value As String)
            _salesRowId = value
        End Set
    End Property
    Private _directPhone As String = ""
    Public Property directPhone As String Implements iDocHeaderLine.directPhone
        Get
            Return _directPhone
        End Get
        Set(ByVal value As String)
            _directPhone = value
        End Set
    End Property
    Private _attentionRowId As String = ""
    Public Property attentionRowId As String Implements iDocHeaderLine.attentionRowId
        Get
            Return _attentionRowId
        End Get
        Set(ByVal value As String)
            _attentionRowId = value
        End Set
    End Property
    Private _attentionEmail As String = ""
    Public Property attentionEmail As String Implements iDocHeaderLine.attentionEmail
        Get
            Return _attentionEmail
        End Get
        Set(ByVal value As String)
            _attentionEmail = value
        End Set
    End Property
    Private _bankInfo As String = ""
    Public Property bankInfo As String Implements iDocHeaderLine.bankInfo
        Get
            Return _bankInfo
        End Get
        Set(ByVal value As String)
            _bankInfo = value
        End Set
    End Property
    Private _deliveryDate As Date = Now.Date.ToShortDateString
    Public Property deliveryDate As Date Implements iDocHeaderLine.deliveryDate
        Get
            Return _deliveryDate
        End Get
        Set(ByVal value As Date)
            _deliveryDate = value
        End Set
    End Property
    Private _expiredDate As Date = CDate(COMM.Fixer.StartDate)
    Public Property expiredDate As Date Implements iDocHeaderLine.expiredDate
        Get
            Return _expiredDate
        End Get
        Set(ByVal value As Date)
            _expiredDate = value
        End Set
    End Property
    Private _shipTerm As String = ""
    Public Property shipTerm As String Implements iDocHeaderLine.shipTerm
        Get
            Return _shipTerm
        End Get
        Set(ByVal value As String)
            _shipTerm = value
        End Set
    End Property
    Private _paymentTerm As String = ""
    Public Property paymentTerm As String Implements iDocHeaderLine.paymentTerm
        Get
            Return _paymentTerm
        End Get
        Set(ByVal value As String)
            _paymentTerm = value
        End Set
    End Property
    Private _freight As Decimal = 0
    Public Property freight As Decimal Implements iDocHeaderLine.freight
        Get
            Return _freight
        End Get
        Set(ByVal value As Decimal)
            _freight = value
        End Set
    End Property
    Private _insurance As Decimal = 0
    Public Property insurance As Decimal Implements iDocHeaderLine.insurance
        Get
            Return _insurance
        End Get
        Set(ByVal value As Decimal)
            _insurance = value
        End Set
    End Property
    Private _specialCharge As Decimal = 0
    Public Property specialCharge As Decimal Implements iDocHeaderLine.specialCharge
        Get
            Return _specialCharge
        End Get
        Set(ByVal value As Decimal)
            _specialCharge = value
        End Set
    End Property
    Private _tax As Decimal = 0
    Public Property tax As Decimal Implements iDocHeaderLine.tax
        Get
            Return _tax
        End Get
        Set(ByVal value As Decimal)
            _tax = value
        End Set
    End Property
    Private _quoteNote As String = ""
    Public Property quoteNote As String Implements iDocHeaderLine.quoteNote
        Get
            Return _quoteNote
        End Get
        Set(ByVal value As String)
            _quoteNote = value
        End Set
    End Property
    Private _relatedInfo As String = ""
    Public Property relatedInfo As String Implements iDocHeaderLine.relatedInfo
        Get
            Return _relatedInfo
        End Get
        Set(ByVal value As String)
            _relatedInfo = value
        End Set
    End Property
    Private _preparedBy As String = ""
    Public Property preparedBy As String Implements iDocHeaderLine.preparedBy
        Get
            Return _preparedBy
        End Get
        Set(ByVal value As String)
            _preparedBy = value
        End Set
    End Property
    Private _qStatus As COMM.Fixer.eDocStatus = Fixer.eDocStatus.QDRAFT
    Public Property qStatus As COMM.Fixer.eDocStatus Implements iDocHeaderLine.qStatus
        Get
            Return _qStatus
        End Get
        Set(ByVal value As COMM.Fixer.eDocStatus)
            _qStatus = value
        End Set
    End Property
    Private _DOCSTATUS As COMM.Fixer.eDocStatus = Fixer.eDocStatus.QDRAFT
    Public Property DOCSTATUS As COMM.Fixer.eDocStatus Implements iDocHeaderLine.DOCSTATUS
        Get
            Return _DOCSTATUS
        End Get
        Set(ByVal value As COMM.Fixer.eDocStatus)
            _DOCSTATUS = value
        End Set
    End Property
    Private _isShowListPrice As Integer = 0
    Public Property isShowListPrice As Integer Implements iDocHeaderLine.isShowListPrice
        Get
            Return _isShowListPrice
        End Get
        Set(ByVal value As Integer)
            _isShowListPrice = value
        End Set
    End Property
    Private _isShowDiscount As Integer = 0
    Public Property isShowDiscount As Integer Implements iDocHeaderLine.isShowDiscount
        Get
            Return _isShowDiscount
        End Get
        Set(ByVal value As Integer)
            _isShowDiscount = value
        End Set
    End Property
    Private _isShowDueDate As Integer = 0
    Public Property isShowDueDate As Integer Implements iDocHeaderLine.isShowDueDate
        Get
            Return _isShowDueDate
        End Get
        Set(ByVal value As Integer)
            _isShowDueDate = value
        End Set
    End Property
    Private _isLumpSumOnly As Integer = 1
    Public Property isLumpSumOnly As Integer Implements iDocHeaderLine.isLumpSumOnly
        Get
            Return _isLumpSumOnly
        End Get
        Set(ByVal value As Integer)
            _isLumpSumOnly = value
        End Set
    End Property
    Private _isRepeatedOrder As Integer = 0
    Public Property isRepeatedOrder As Integer Implements iDocHeaderLine.isRepeatedOrder
        Get
            Return _isRepeatedOrder
        End Get
        Set(ByVal value As Integer)
            _isRepeatedOrder = value
        End Set
    End Property
    Private _delDateFlag As Integer = 0
    Public Property delDateFlag As Integer Implements iDocHeaderLine.delDateFlag
        Get
            Return _delDateFlag
        End Get
        Set(ByVal value As Integer)
            _delDateFlag = value
        End Set
    End Property
    Private _org As String = ""
    Public Property org As String Implements iDocHeaderLine.org
        Get
            Return _org
        End Get
        Set(ByVal value As String)
            _org = value
        End Set
    End Property
    Private _siebelRBU As String = ""
    Public Property siebelRBU As String Implements iDocHeaderLine.siebelRBU
        Get
            Return _siebelRBU
        End Get
        Set(ByVal value As String)
            _siebelRBU = value
        End Set
    End Property
    Private _DIST_CHAN As String = ""
    Public Property DIST_CHAN As String Implements iDocHeaderLine.DIST_CHAN
        Get
            Return _DIST_CHAN
        End Get
        Set(ByVal value As String)
            _DIST_CHAN = value
        End Set
    End Property
    Private _DIVISION As String = ""
    Public Property DIVISION As String Implements iDocHeaderLine.DIVISION
        Get
            Return _DIVISION
        End Get
        Set(ByVal value As String)
            _DIVISION = value
        End Set
    End Property
    Private _PO_NO As String = ""
    Public Property PO_NO As String Implements iDocHeaderLine.PO_NO
        Get
            Return _PO_NO
        End Get
        Set(ByVal value As String)
            _PO_NO = value
        End Set
    End Property
    Private _CARE_ON As String = ""
    Public Property CARE_ON As String Implements iDocHeaderLine.CARE_ON
        Get
            Return _CARE_ON
        End Get
        Set(ByVal value As String)
            _CARE_ON = value
        End Set
    End Property
    Private _isExempt As Integer = 0
    Public Property isExempt As Integer Implements iDocHeaderLine.isExempt
        Get
            Return _isExempt
        End Get
        Set(ByVal value As Integer)
            _isExempt = value
        End Set
    End Property
    Private _Inco1 As String = ""
    Public Property Inco1 As String Implements iDocHeaderLine.Inco1
        Get
            Return _Inco1
        End Get
        Set(ByVal value As String)
            _Inco1 = value
        End Set
    End Property
    Private _Inco2 As String = ""
    Public Property Inco2 As String Implements iDocHeaderLine.Inco2
        Get
            Return _Inco2
        End Get
        Set(ByVal value As String)
            _Inco2 = value
        End Set
    End Property
    Private _SalesDistrict As String = ""
    Public Property SalesDistrict As String Implements iDocHeaderLine.SalesDistrict
        Get
            Return _SalesDistrict
        End Get
        Set(ByVal value As String)
            _SalesDistrict = value
        End Set
    End Property
    Private _OriginalQuoteID As String = ""
    Public Property OriginalQuoteID As String Implements iDocHeaderLine.OriginalQuoteID
        Get
            Return _OriginalQuoteID
        End Get
        Set(ByVal value As String)
            _OriginalQuoteID = value
        End Set
    End Property
    Private _Currency As String = ""
    Public Property Currency As String Implements iDocHeaderLine.currency
        Get
            Return _Currency
        End Get
        Set(ByVal value As String)
            _Currency = value
        End Set
    End Property
    Private _DocReg As COMM.Fixer.eDocReg = COMM.Fixer.eDocReg.ATW
    Public Property DocReg As COMM.Fixer.eDocReg Implements iDocHeaderLine.DocReg
        Get
            Return _DocReg
        End Get
        Set(ByVal value As COMM.Fixer.eDocReg)
            _DocReg = value
        End Set
    End Property

    Private _DocType As COMM.Fixer.eDocType = Fixer.eDocType.EQ
    Public Property DocType As COMM.Fixer.eDocType Implements iDocHeaderLine.DocType
        Get
            Return _DocType
        End Get
        Set(ByVal value As COMM.Fixer.eDocType)
            _DocType = value
        End Set
    End Property
    Private _LastUpdatedBy As String = _CreatedBy
    Public Property LastUpdatedBy As String Implements iDocHeaderLine.LastUpdatedBy
        Get
            Return _LastUpdatedBy
        End Get
        Set(ByVal value As String)
            _LastUpdatedBy = value
        End Set
    End Property
    Private _LastUpdatedDate As DateTime = Now.Date.ToShortDateString
    Public Property LastUpdatedDate As Date Implements iDocHeaderLine.LastUpdatedDate
        Get
            Return _LastUpdatedDate
        End Get
        Set(ByVal value As Date)
            _LastUpdatedDate = value
        End Set
    End Property
    Private _ReqDate As Date = Now.Date.ToShortDateString
    Public Property ReqDate As Date Implements iDocHeaderLine.ReqDate
        Get
            Return _ReqDate
        End Get
        Set(ByVal value As Date)
            _ReqDate = value
        End Set
    End Property
    Private _PartialF As Integer = 0
    Public Property PartialF As Integer Implements iDocHeaderLine.PartialF
        Get
            Return _PartialF
        End Get
        Set(ByVal value As Integer)
            _PartialF = value
        End Set
    End Property

    Private _DocRealType As String = ""
    Public Property DocRealType As String Implements IBUS.iDocHeaderLine.DocRealType
        Get
            Return _DocRealType
        End Get
        Set(ByVal value As String)
            _DocRealType = value
        End Set
    End Property
    Private _IS_EARLYSHIP As Integer = 0
    Public Property IS_EARLYSHIP As Integer Implements IBUS.iDocHeaderLine.IS_EARLYSHIP
        Get
            Return _IS_EARLYSHIP
        End Get
        Set(ByVal value As Integer)
            _IS_EARLYSHIP = value
        End Set
    End Property
    Private _PRINTOUT_FORMAT As COMM.Fixer.USPrintOutFormat = COMM.Fixer.USPrintOutFormat.SUB_ITEM_WITH_SUB_ITEM_PRICE
    Public Property PRINTOUT_FORMAT As COMM.Fixer.USPrintOutFormat Implements IBUS.iDocHeaderLine.PRINTOUT_FORMAT
        Get
            Return _PRINTOUT_FORMAT
        End Get
        Set(ByVal value As COMM.Fixer.USPrintOutFormat)
            _PRINTOUT_FORMAT = value
        End Set
    End Property
    Private _PODate As Date = Now.Date.ToShortDateString
    Public Property PODate As Date Implements IBUS.iDocHeaderLine.PODate
        Get
            Return _PODate
        End Get
        Set(ByVal value As Date)
            _PODate = value
        End Set
    End Property
    Private _EMPLOYEEID As String = ""
    Public Property EMPLOYEEID As String Implements IBUS.iDocHeaderLine.EMPLOYEEID
        Get
            Return _EMPLOYEEID
        End Get
        Set(ByVal value As String)
            _EMPLOYEEID = value
        End Set
    End Property
    Private _KEYPERSON As String = ""
    Public Property KEYPERSON As String Implements IBUS.iDocHeaderLine.KEYPERSON
        Get
            Return _KEYPERSON
        End Get
        Set(ByVal value As String)
            _KEYPERSON = value
        End Set
    End Property
    Private _ISVIRPARTONLY As COMM.Fixer.eIsVirNoOnly = Fixer.eIsVirNoOnly.PartNoOnly
    Public Property ISVIRPARTONLY As COMM.Fixer.eIsVirNoOnly Implements IBUS.iDocHeaderLine.ISVIRPARTONLY
        Get
            Return _ISVIRPARTONLY
        End Get
        Set(ByVal value As COMM.Fixer.eIsVirNoOnly)
            _ISVIRPARTONLY = value
        End Set
    End Property
    Private _SHIPCUSTPONO As String = ""
    Public Property SHIPCUSTPONO As String Implements IBUS.iDocHeaderLine.SHIPCUSTPONO
        Get
            Return _SHIPCUSTPONO
        End Get
        Set(ByVal value As String)
            _SHIPCUSTPONO = value
        End Set
    End Property
    Private _TAXCLASS As String = ""
    Public Property TAXCLASS As String Implements IBUS.iDocHeaderLine.TAXCLASS
        Get
            Return _TAXCLASS
        End Get
        Set(ByVal value As String)
            _TAXCLASS = value
        End Set
    End Property
    Private _TAXDEPCITY As String = ""
    Public Property TAXDEPCITY As String Implements IBUS.iDocHeaderLine.TAXDEPCITY
        Get
            Return _TAXDEPCITY
        End Get
        Set(ByVal value As String)
            _TAXDEPCITY = value
        End Set
    End Property
    Private _TAXDSTCITY As String = ""
    Public Property TAXDSTCITY As String Implements IBUS.iDocHeaderLine.TAXDSTCITY
        Get
            Return _TAXDSTCITY
        End Get
        Set(ByVal value As String)
            _TAXDSTCITY = value
        End Set
    End Property
    Private _TRIANGULARINDICATOR As String = ""
    Public Property TRIANGULARINDICATOR As String Implements IBUS.iDocHeaderLine.TRIANGULARINDICATOR
        Get
            Return _TRIANGULARINDICATOR
        End Get
        Set(ByVal value As String)
            _TRIANGULARINDICATOR = value
        End Set
    End Property
    Private _quoteNo As String = ""
    Public Property quoteNo As String Implements iDocHeaderLine.quoteNo
        Get
            Return _quoteNo
        End Get
        Set(ByVal value As String)
            _quoteNo = value
        End Set
    End Property
    Private _Revision_Number As Integer = 0
    Public Property Revision_Number As Integer Implements iDocHeaderLine.Revision_Number
        Get
            Return _Revision_Number
        End Get
        Set(ByVal value As Integer)
            _Revision_Number = value
        End Set
    End Property
    Private _Active As Boolean = False
    Public Property Active As Boolean Implements iDocHeaderLine.Active
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property
End Class
Public Class Doc : Implements IBUS.iDoc

    Private _errCode As COMM.Msg.eErrCode
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property

    Protected Friend Function SO_GetNumber(ByVal preFix As String) As String
        Dim NUM As New Object
        'Frank 2013/07/23 Get next Order\Quote number from production\staging table base on current environment
        Dim str As String = String.Empty, tablename As String = "OrderNo"
        If Util.IsTesting() Then tablename = "OrderNoStaging"
        str = String.Format("begin tran SELECT num+1 as num FROM " & tablename & " with (tablockx) where prefix='{0}' update " & tablename & " set num=num+1 where prefix='{0}' commit tran", preFix)


        NUM = dbUtil.dbExecuteScalar("MY", str)
        If IsNumeric(NUM) Then
            Return preFix & CDbl(NUM).ToString("000000")
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Create new quotation uid, ex ERHJDSSFUW
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NewUID() As String Implements IBUS.iUID.NewUID

        Dim _NewUID As String = String.Empty

        For i As Integer = 1 To 5
            _NewUID = System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15)
            If IsValidUID(_NewUID) = False Then Exit For
        Next

        Return _NewUID

    End Function

    ''' <summary>
    ''' Create new quotation no, ex AUSQxxxxxx  AJPQxxxxxx
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NewQuotationNo(ByVal Reg As COMM.Fixer.eDocReg, ByVal DocType As COMM.Fixer.eDocType) As String Implements IBUS.iUID.NewQuotationNo
        Dim Prefix As String = String.Empty
        If DocType = Fixer.eDocType.EQ Then
            Prefix = COMM.MasterFixer.getQuotePrefixByReg(Reg)
        Else
            Prefix = COMM.MasterFixer.getOrderPrefixByReg(Reg)
        End If
        Dim num As String = ""
        num = SO_GetNumber(Prefix.ToUpper)
        Return num
    End Function

    Public Function GetDefaultDistChannDivisionSalesGrpOfficeByCompanyId(ByVal CompanyId As String, ByRef dist_chann As String, ByRef division As String, ByRef SalesGroup As String, ByRef SalesOffice As String) As Boolean Implements IBUS.iDoc.GetDefaultDistChannDivisionSalesGrpOfficeByCompanyId
        Dim strSql As String = _
          " select b.vtweg as dist_chann, b.SPART as division, b.VKBUR as SalesOffice, b.VKGRP as SalesGroup " + _
          " from saprdp.knvv b " + _
          " where b.mandt='168' and b.kunnr = '" + UCase(CompanyId) + "' and rownum=1 " + _
          " order by b.VKGRP, b.VKBUR "
        Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", strSql)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                dist_chann = .Item("dist_chann") : division = .Item("division") : SalesGroup = .Item("SalesGroup") : SalesOffice = .Item("SalesOffice")
            End With
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetKeyInPersonV2(ByVal userid As String) As System.Data.DataTable Implements IBUS.iDoc.GetKeyInPerson
        userid = userid.Trim
        Dim str As String = " select distinct a.FULL_NAME, a.SALES_CODE, IsNull(a.EMAIL,'') as EMAIL " & _
                                " from SAP_EMPLOYEE a " & _
                                " where a.EMAIL='" & userid & "' " & _
                                " order by a.SALES_CODE desc "
        Dim dt As New DataTable
        dt = dbUtil.dbGetDataTable("B2B", str)
        Return dt
    End Function

    Public Function GetLocalTime(ByVal org As String) As Date Implements IBUS.iDoc.GetLocalTime
        Dim localtime As DateTime = DateTime.Now
        Dim utcTime As DateTime = DateTime.Now.ToUniversalTime()
        Dim timezone As Object = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 isnull(timezonename,'') as timezonename from TIMEZONE where org like '%{0}'", org))
        If timezone IsNot Nothing AndAlso Not String.IsNullOrEmpty(timezone) Then
            Dim TZI As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone)
            Dim TS As TimeSpan = TZI.GetUtcOffset(utcTime)
            localtime = utcTime.Add(TS)
        End If
        Return localtime
    End Function

    Public Function getSalesEmployeeList(ByVal Org As String) As System.Data.DataTable Implements IBUS.iDoc.getSalesEmployeeList
        Dim str As String = " select distinct a.FULL_NAME, a.SALES_CODE, IsNull(a.EMAIL,'') as EMAIL " & _
                           " from SAP_EMPLOYEE a " & _
                           " where a.PERS_AREA like '" & Left(Org, 2) & "%' " & _
                           " order by a.FULL_NAME "
        Dim dt As New DataTable
        dt = dbUtil.dbGetDataTable("MY", str)
        Return dt
    End Function




    Public Function shipCode2Txt(ByVal shipCode As String) As String Implements IBUS.iDoc.shipCode2Txt
        Dim ret = ""
        Select Case shipCode.Trim()
            Case "01"
                ret = "Truck / Sea"
            Case "02"
                ret = "Pick up by customer"
            Case "03"
                ret = "Fedex"
            Case "04"
                ret = "UPS Economy"
            Case "05"
                ret = "DHL Economy"
            Case "06"
                ret = "By Material"
            Case "07"
                ret = "Air"
            Case "08"
                ret = "Service"
            Case "09"
                ret = "TNT Economy"
            Case "10"
                ret = "TNT Global"
            Case "11"
                ret = "UPS Global"
            Case "12"
                ret = "DHL Air Express"
            Case "13"
                ret = "Hand Carry"
            Case "14"
                ret = "Courier"
            Case "15"
                ret = "UPS Standard"
            Case "16"
                ret = "Cust. Own Forwarder"
            Case "17"
                ret = "TNT Economy Special"
            Case "18"
                ret = "By Sea to AKMC&ADMC"
            Case "19"
                ret = "By Sea/Air (to ACSC)"
            Case "20"
                ret = "UPS Express Saver"
            Case "21"
                ret = "UPS Expres Plus 9:00"
            Case "22"
                ret = "UPS Express 12:00"
            Case "23"
                ret = "DHL Europlus"
        End Select
        Return ret
    End Function


    Public Function CheckSAPQuote(ByVal QuoteId As String) As Boolean Implements IBUS.iDoc.CheckSAPQuote
        Dim strSql As String = " select vbak.vbeln from saprdp.vbak where vbak.vbeln = '" + UCase(QuoteId) + "'"
        Dim ConnectionName As String = "SAP_PRD"
        If Util.IsTesting() Then ConnectionName = "SAP_Test"
        Dim dt As DataTable = OraDbUtil.dbGetDataTable(ConnectionName, strSql)
        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub ProcessAfterOrderFailed(ByVal M As IBUS.iDocHeaderLine, ByVal C As IBUS.iCartList, ByVal oType As COMM.Fixer.eDocType) Implements IBUS.iDoc.ProcessAfterOrderFailed
        Dim MM As New DocHeader
        MM.ChangeDocStatus(M.Key, Fixer.eDocStatus.OFAILED, oType)
        SendFailedOrderMail(M)
        If Not M.Key.StartsWith("AUSO", StringComparison.CurrentCultureIgnoreCase) _
            AndAlso Not M.Key.StartsWith("AMXO", StringComparison.CurrentCultureIgnoreCase) Then
            SendPI(M)
        End If

    End Sub

    Public Sub ProcessAfterOrderSuccess(ByVal M As IBUS.iDocHeaderLine, ByVal C As IBUS.iCartList, ByVal oType As COMM.Fixer.eDocType) Implements IBUS.iDoc.ProcessAfterOrderSuccess
        Dim b1 As Boolean = False
        updateEWFlag(C)
        'Try
        Dim MM As New DocHeader
        MM.ChangeDocStatus(M.Key, Fixer.eDocStatus.OFINISH, Fixer.eDocType.ORDER)
        'Frank Set this quote to active status, other quotes with the same quote no become to inactive status
        MM.SetRevisionQuoteToActive(M.OriginalQuoteID, COMM.Fixer.eDocType.EQ)
        SendPI(M)



        Dim Pat As IBUS.iPatch = Pivot.NewObjPatch
        If Pat.isHasBto(C) Then
            'sendSheet(Order_No)
        End If
        Dim OptyId As String = ""
        'Dim dtOptyDetail As DataTable = myOrderDetail.GetDT(String.Format("order_id='{0}' and optyid<>''", Order_No), "")
        If M.OriginalQuoteID <> "" Then
            OptyId = getOptyIdByQuoteId(M.OriginalQuoteID)

        End If

        If OptyId <> "" Then
            Dim OPTYrevenue As Decimal = 0.0
            OPTYrevenue = C.getTotalAmount


            Try
                b1 = Pivot.NewObjSiebelTools.UpdateOpportunityStatusRevenue(OptyId, "Won", OPTYrevenue, False)
            Catch ex As Exception
                Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", _
                               String.Format("Update Opty to Won for SO:{0} OptyID:{1}", M.Key, OptyId), ex.ToString(), True, "", "", "")
            End Try

        End If
        'Catch ex As Exception
        '    ErrMsg = ex.Message
        '    Return False
        'End Try

    End Sub
    Shared Function getOptyIdByQuoteId(ByVal quoteId As String) As String
        Dim optyID As String = String.Empty
        Dim dt As DataTable = Nothing
        Dim DAPT As New optyQuote

        dt = DAPT.GetDT(String.Format("QuoteId='{0}'", quoteId), "")

        If dt.Rows.Count > 0 Then
            optyID = dt.Rows(0).Item("optyId")
        End If

        Return optyID
    End Function
    Public Shared Function SendPI(ByVal m As IBUS.iDocHeaderLine, Optional ByVal IsRecover As Boolean = False, Optional ByVal isSendBtosNote As Boolean = False) As Integer
        Try
            Dim returnint As Integer = SendSPR_NOPI(m)
        Catch ex As Exception
            Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "Send Sprno PI error", "", "", "")
        End Try
        Dim QuotationDBName As String = "eQuotation":  If Util.IsTesting() Then QuotationDBName = "eQuotationStaging"
        Dim orderCompDt As New DataTable
        Dim apt As New SqlClient.SqlDataAdapter( _
            " select top 1 a.PO_NO, a.QuoteToErpId, b.COMPANY_NAME from " + QuotationDBName + ".dbo.OrderMaster a " + _
            " inner join SAP_DIMCOMPANY b on a.QuoteToErpId=b.COMPANY_ID where a.QuoteId=@ONO", _
            ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        apt.SelectCommand.Parameters.AddWithValue("ONO", m.Key)
        apt.Fill(orderCompDt) : apt.SelectCommand.Connection.Close()
        If orderCompDt.Rows.Count = 0 Then Return 0
        Dim FROM_Email As String = "myadvantech@advantech.com", TO_Email As String = m.CreatedBy
        If m.org.ToString.Equals("SG01", StringComparison.OrdinalIgnoreCase) Then FROM_Email = "asg.op@advantech.com"
        If m.org.ToString.Equals("US01", StringComparison.OrdinalIgnoreCase) Then
            FROM_Email = m.CreatedBy
            Dim EmployeeEmail As Object = dbUtil.dbExecuteScalar("MY", String.Format(" select top 1 A.EMAIL from  dbo.SAP_EMPLOYEE A INNER JOIN " + QuotationDBName + ".DBO.OrderMaster B ON B.EMPLOYEEID=A.SALES_CODE where B.QuoteId= '{0}' and dbo.IsEmail(A.EMAIL)=1 and A.PERS_AREA='US01'", m.Key))
            If EmployeeEmail IsNot Nothing AndAlso Not String.IsNullOrEmpty(EmployeeEmail) AndAlso Util.isEmail(EmployeeEmail.ToString.Trim) Then
                FROM_Email = EmployeeEmail
            End If
        End If
        Dim CC_Email As String = "", BCC_Email As String = "myadvantech@advantech.com"
        Dim pono As String = orderCompDt.Rows(0).Item("po_no"), compName As String = orderCompDt.Rows(0).Item("company_name"), soldtoId As String = orderCompDt.Rows(0).Item("QuoteToErpId")
        Dim subject_email As String = "Advantech Order (" + pono + "/" + m.quoteNo + ") for " + compName + " (" + soldtoId + ")"
        Dim attachfile As String = "", mailbody As String = ""
        mailbody = GetPI(m.Key, 0)
        Dim strCC As String = "", strCC_External As String = ""
        Dim j As Integer = GetPIcc(m, strCC, strCC_External)
        TO_Email = m.CreatedBy
        If strCC_External.Trim <> "" Then
            TO_Email = TO_Email + ";" + strCC_External
        End If
        CC_Email = ""

        TO_Email = strCC
        'CC_Email = "eBusiness.AEU@advantech.eu;"


        If m.org.ToString.Trim.ToUpper = "EU10" Then
            CC_Email = CC_Email + "claudio.cerqueti@advantech.nl;"
            If isSendBtosNote = False Then
                CC_Email = CC_Email + "margot.vandommelen@advantech.nl;jos.vanberlo@advantech.nl;"
            End If
        End If
        If m.org.ToString.Equals("SG01", StringComparison.OrdinalIgnoreCase) Then
            CC_Email = CC_Email + "asg.op@advantech.com;"
        End If

        If isSendBtosNote Then
            Dim arr As ArrayList = GetBTOSOrderNotifyList(m.org, m.DocReg)
            TO_Email += String.Join(";", arr.ToArray())
        End If

        If m.Key.ToUpper.StartsWith("AUSO") Then
            Dim Expandstr As String = String.Format("From:{0}<hr/>To:{1}<hr/>CC:{2}<hr/>Bcc:{3}<hr/>", FROM_Email, TO_Email, CC_Email, BCC_Email)
            Util.SendEmail("ming.zhao@advantech.com.cn", "tc.chen@advantech.com.tw;ming.zhao@advantech.com.cn;frank.chung@advantech.com.tw", "", "", subject_email, attachfile, Expandstr + mailbody, "")
        Else
            Call Util.SendEmail(FROM_Email, TO_Email, CC_Email, BCC_Email, subject_email, attachfile, mailbody, "")
        End If
        Return 1
    End Function
    Public Shared Function GetPIcc(ByVal m As IBUS.iDocHeaderLine, ByRef Str_cc As String, ByRef Str_cc_External As String) As Integer
        Dim InvalidOrg As String = ConfigurationManager.AppSettings("InvalidOrg").ToString.Trim()
        Dim OracleSCP As New StringBuilder
        OracleSCP.AppendLine(" select b.kunnr as COMPANY_ID, b.vkorg as ORG_ID, b.vtweg as DIST_CHANN, ")
        OracleSCP.AppendLine(" b.spart as DIVISION, b.parvw as PARTNER_FUNCTION, b.kunn2 as PARENT_COMPANY_ID, ")
        OracleSCP.AppendLine(" b.lifnr as VENDOR_CREDITOR, b.pernr as SALES_CODE, b.parnr as PARTNER_NUMBER, b.KNREF,  ")
        OracleSCP.AppendLine(" b.DEFPA from saprdp.kna1 a inner join saprdp.knvp b on a.kunnr=b.kunnr ")
        OracleSCP.AppendLine(" where a.mandt='168' and b.mandt='168' ")
        'OracleSCP.AppendLine(" and b.vkorg in ('AU01','BR01','CN01','CN10','EU10','JP01','KR01','MY01','SG01','TL01','TW01','US01') ")
        OracleSCP.AppendFormat(" and b.vkorg in ('{0}') ", m.org)
        Dim OracleSEM As New StringBuilder
        OracleSEM.AppendLine(" select a.pernr as sales_code, a.werks as pers_area, a.persg as emp_group, a.persk as sub_emp_group,  ")
        OracleSEM.AppendLine(" (select b.stras from saprdp.pa0006 b where b.pernr=a.pernr and rownum=1 and b.mandt='168') as address, ")
        OracleSEM.AppendLine(" (select b.land1 from saprdp.pa0006 b where b.pernr=a.pernr and rownum=1 and b.mandt='168') as country,  a.sname, a.ename, ")
        OracleSEM.AppendLine(" (select b.vorna from saprdp.pa0002 b where b.pernr=a.pernr and rownum=1 and b.mandt='168') as first_name, ")
        OracleSEM.AppendLine(" (select b.nachn from saprdp.pa0002 b where b.pernr=a.pernr and rownum=1 and b.mandt='168') as last_name, ")
        OracleSEM.AppendLine(" concat(concat((select b.vorna from saprdp.pa0002 b where b.pernr=a.pernr and rownum=1 and  ")
        OracleSEM.AppendLine(" b.mandt='168'),'.'),(select b.nachn from saprdp.pa0002 b where b.pernr=a.pernr and rownum=1 and b.mandt='168')) as full_name, ")
        OracleSEM.AppendLine(" (select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='0020' and rownum=1) as tel, ")
        OracleSEM.AppendLine(" decode((select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='MAIL'  ")
        OracleSEM.AppendLine(" and rownum=1),null,(select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and  ")
        OracleSEM.AppendLine(" b.subty='0010' and rownum=1),(select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='MAIL' and rownum=1)) as email, ")
        OracleSEM.AppendLine(" (select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='CELL' and rownum=1) as cellphone, ")
        OracleSEM.AppendLine(" (select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='MPHN' and rownum=1) as otherphone, ")
        OracleSEM.AppendLine(" decode((select b.anzkd from saprdp.pa0002 b where b.mandt='168' and rownum=1 and b.pernr=a.pernr),null,0,(select b.anzkd from saprdp.pa0002 b where b.mandt='168' and rownum=1 and b.pernr=a.pernr)) as num_of_child, ")
        OracleSEM.AppendLine(" a.Abkrs as payr_area from saprdp.pa0001 a where a.mandt='168' ")

        Dim sql As String = "select distinct b.EMAIL  from (" + OracleSCP.ToString() + ") a inner join (" + OracleSEM.ToString() + ") b on a.SALES_CODE=b.SALES_CODE " _
                                                             & " and a.ORG_ID not in " + InvalidOrg + "" _
                                                             & " where a.COMPANY_ID='" + m.AccErpId + "'  ORDER BY b.EMAIL " ') and dbo.IsEmail(b.EMAIL)=1
        Dim CDT As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", sql)
        If CDT.Rows.Count > 0 Then
            For i As Integer = 0 To CDT.Rows.Count - 1
                With CDT.Rows(i)
                    If Not IsDBNull(.Item("EMAIL")) AndAlso Util.isEmail(.Item("EMAIL").ToString) Then
                        Str_cc = Str_cc & .Item("EMAIL").ToString & ";"
                    End If
                End With
            Next
        End If
        Dim QuotationDBName As String = "eQuotation" : If Util.IsTesting() Then QuotationDBName = "eQuotationStaging"
        Dim sql1 As String = "select CONTACT_EMAIL from SAP_COMPANY_CONTACTS where COMPANY_ID='" + m.AccErpId + "'"
        Dim sql2 As String = "select a.EMAIL as KEYEmail from sap_employee a inner join " + QuotationDBName + ".DBO.OrderMaster b on a.SALES_CODE=b.KEYPERSON WHERE a.PERS_AREA='" + m.org + "'  and dbo.IsEmail(a.EMAIL)=1 and b.QuoteId='" + m.Key + "'"
        Dim dt As DataTable = dbUtil.dbGetDataTable("MY", sql1 + " UNION " + sql2)
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                With dt.Rows(i)
                    If Not IsDBNull(.Item("CONTACT_EMAIL")) AndAlso .Item("CONTACT_EMAIL").ToString <> "" AndAlso Util.isEmail(.Item("CONTACT_EMAIL")) Then
                        If Util.IsInternalUser(.Item("CONTACT_EMAIL").ToString) Then
                            Str_cc = Str_cc & .Item("CONTACT_EMAIL").ToString & ";"
                        Else
                            Str_cc_External = Str_cc_External & .Item("CONTACT_EMAIL").ToString & ";"
                        End If
                    End If
                End With
            Next
        End If
        Return 1
    End Function
    Public Shared Function GetPI(ByVal Order_No As String, ByVal strPIType As String) As String
        Dim myDoc As New System.Xml.XmlDocument
        Util.HtmlToXML("~/ORDER/PI.aspx?UID=" & Order_No, myDoc)
        Return myDoc.OuterXml
    End Function
    Public Shared Function GetBTOSOrderNotifyList(ByVal SAPOrg As String, ByVal DocReg As COMM.Fixer.eDocReg) As ArrayList

        Dim arr As New ArrayList

        Select Case Left(UCase(SAPOrg), 2)
            Case "EU"
                arr.Add("Erik.Smulders@advantech.eu") : arr.Add("tam.tran@advantech.eu") : arr.Add("jos.vanberlo@advantech.nl") : arr.Add("margot.vandommelen@advantech.nl")
                arr.Add("e-btos@advantech.eu") : arr.Add("louis.lin@advantech.nl") : arr.Add("Michael.Zoon@advantech.eu")
                arr.Add("eSCM-AESC@advantech.nl")
            Case "SG", "MY"
                arr.Add("asg.op@advantech.com")
            Case "US"
                'Frank 2012/07/02:If AOnline sales place the CTOS order, then just cc to Mark
                If (DocReg And Fixer.eDocReg.ANA) = DocReg Then
                    arr.Add("Mark.Yang@advantech.com")
                Else
                    arr.Add("Mark.Yang@advantech.com") : arr.Add("Shufen.Chen@advantech.com") ' : arr.Add("Charles.Chi@advantech.com")
                End If
            Case Else
                arr.Add("myadvantech@advantech.com") : arr.Add("brian.tsai@advantech.com.tw")
        End Select
        Return arr
    End Function
    Public Shared Sub updateEWFlag(ByVal Cart As IBUS.iCartList)
        System.Threading.Thread.Sleep(2000)
        If Not IsNothing(Cart) Then
            Dim nDT As New DataTable
            nDT.Columns.Add("so_no") : nDT.Columns.Add("line_no") : nDT.Columns.Add("exwarranty_flag")
            For Each R As IBUS.iCartLine In Cart
                Dim rDT As DataRow = nDT.NewRow
                rDT.Item("so_no") = R.key.Value
                rDT.Item("line_no") = R.lineNo.Value
                rDT.Item("exwarranty_flag") = IIf(R.ewFlag.Value = "99", "36", CInt(R.ewFlag.Value).ToString("00"))
                nDT.Rows.Add(rDT)
            Next
            Dim SAP As IBUS.iSAP = Pivot.NewObjSAPTools
            SAP.UpdateSOWarrantyFlagByTable(nDT, "", True)
        End If

    End Sub
    Public Shared Function SendSPR_NOPI(ByVal M As IBUS.iDocHeaderLine) As Integer
        'Dim quote_id As Object = dbUtil.dbExecuteScalar("b2b", "select top 1 OptyID from order_detail where OptyID is not null and OptyID <> '' and ORDER_ID ='" + order_no + "'")
        'If quote_id Is Nothing Then
        '    Return 0
        '    Exit Function
        'End If
        'Dim quoteId As String = quote_id.ToString()
        'Dim IsSend As Boolean = False
        'Dim WS As New quote.quoteExit
        'WS.Timeout = -1
        'If WS.isQuoteExpired(quoteId) Then
        '    Return 0
        '    Exit Function
        'End If
        'Dim ds As New DataSet
        'WS.getQuotationDetailById(quoteId, ds)
        'If ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
        '    Dim DT As DataTable = ds.Tables(0)
        '    If DT.Rows.Count > 0 Then
        '        Dim MailBody As String = "<table width=""90%"" border=""1""><tr><td>Part_NO</td><td>Line</td><td>QTY</td><td>Spr_NO</td></tr>"
        '        For i As Integer = 0 To DT.Rows.Count - 1
        '            With DT.Rows(i)
        '                If Not IsDBNull(.Item("sprno")) AndAlso Not String.IsNullOrEmpty(.Item("sprno")) Then
        '                    MailBody += String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", .Item("partno"), .Item("line_no"), .Item("qty"), .Item("sprno"))
        '                    IsSend = True
        '                End If
        '            End With
        '        Next
        '        MailBody += "</table>"
        '        Dim Subject_Email As String = "Advantech Order(" + order_no + ") contains SPR Number"
        '        If IsSend Then
        '            MailUtil.Utility_EMailPage("eBusiness.AEU@advantech.eu", "eSCM-AESC@advantech.nl", "", "ming.zhao@advantech.com.cn;tc.chen@advantech.com.tw", Subject_Email, "", MailBody)
        '        End If
        '        'MailUtil.Utility_EMailPage("eBusiness.AEU@advantech.eu", "ming.zhao@advantech.com.cn;nada.liu@advantech.com.cn;tc.chen@advantech.com.tw", "", "nada.liu@advantech.com.cn;tc.chen@advantech.com.tw", Subject_Email, "", MailBody)
        '    End If
        'End If
        'Return 1
        Return 1
    End Function

    Public Sub VerifyDisChannelAndDivision(ByVal companyid As String, ByRef disChannel As String, ByRef division As String, ByVal Org As String) Implements IBUS.iDoc.VerifyDisChannelAndDivision
        disChannel = "10" : division = "00"
        companyid = UCase(companyid)
        Dim strOrg As String = Org.ToString().ToUpper(), strSalesGrp As String = "", strSalesOffice As String = ""
        Dim apt As New SqlClient.SqlDataAdapter("select top 1 COMPANY_ID, ORG_ID, SALESGROUP, SALESOFFICE from SAP_DIMCOMPANY where COMPANY_ID=@COMPANYID and ORG_ID=@ORGID", ConfigurationManager.ConnectionStrings("MY").ConnectionString)
        apt.SelectCommand.Parameters.AddWithValue("COMPANYID", companyid) : apt.SelectCommand.Parameters.AddWithValue("ORGID", strOrg)
        Dim dt As New DataTable
        apt.Fill(dt)
        If dt.Rows.Count = 0 Then
            dt = OraDbUtil.dbGetDataTable("SAP_PRD", _
                " select a.kunnr as COMPANY_ID, b.vkorg as ORG_ID, b.VKBUR as SALESOFFICE, b.VKGRP as SALESGROUP " + _
                " from saprdp.kna1 a inner join saprdp.knvv b on a.kunnr=b.kunnr " + _
                " where a.mandt='168' and b.mandt='168' and b.vkorg ='" + UCase(strOrg) + "'  " + _
                " and a.ktokd in ('Z001','Z002') and a.kunnr='" + UCase(companyid) + "' and rownum=1 ")
        End If
        If dt.Rows.Count = 1 Then
            strOrg = dt.Rows(0).Item("ORG_ID") : strSalesGrp = dt.Rows(0).Item("SALESGROUP").ToString().ToUpper() : strSalesOffice = dt.Rows(0).Item("SALESOFFICE").ToString().ToUpper()
            If strOrg = "US01" Then
                Select Case strSalesOffice
                    Case "2100", "2700"
                        disChannel = "30" : division = "10"
                        Dim SAP As IBUS.iSAP = Pivot.NewObjSAPTools
                        If Not SAP.VerifyDistChannelDivisionGroupOffice(strOrg, companyid, companyid, disChannel, division, _
                                                                            "ZOR", strSalesGrp, strSalesOffice, Nothing) Then
                            Dim CandidateDt As New DataTable, CanApt As New SqlClient.SqlDataAdapter("", ConfigurationManager.ConnectionStrings("MY").ConnectionString)
                            CanApt.SelectCommand.CommandText = _
                                " select distinct DIST_CHANN, DIVISION, SALESGROUP, SALESOFFICE " + _
                                " from SAP_COMPANY_LOV " + _
                                " where ORG_ID=@ORGID and SALESOFFICE=@SOFFICE and DIST_CHANN in ('10','30') and SALESGROUP=@SGRP " + _
                                " order by DIST_CHANN, DIVISION "
                            With CanApt.SelectCommand.Parameters
                                .AddWithValue("ORGID", strOrg) : .AddWithValue("SOFFICE", strSalesOffice) : .AddWithValue("SGRP", strSalesGrp)
                            End With
                            CanApt.Fill(CandidateDt)
                            CanApt.SelectCommand.Connection.Close()
                            For Each CandidateRow As DataRow In CandidateDt.Rows
                                If SAP.VerifyDistChannelDivisionGroupOffice(strOrg, companyid, companyid, CandidateRow.Item("DIST_CHANN"), CandidateRow.Item("DIVISION"), _
                                                                              "ZOR", strSalesGrp, strSalesOffice, Nothing) Then
                                    disChannel = CandidateRow.Item("DIST_CHANN") : division = CandidateRow.Item("DIVISION")
                                    Exit For
                                End If
                            Next
                        End If
                    Case "2200", "2300"
                        '20120627 TC: Added office 2200 for US CheckPoint UZISCHE01
                        disChannel = "10" : division = "20"
                End Select
            Else

            End If
        End If
    End Sub

    Public Sub SendFailedOrderMail(ByVal M As IBUS.iDocHeaderLine) Implements IBUS.iDoc.SendFailedOrderMail
        Dim strStyle As String = "", strBody As String = "", t_strHTML As String = ""
        Dim FROM_Email As String = "", TO_Email As String = "", CC_Email As String = "", BCC_Email As String = "", Subject_Email As String = "", AttachFile As String = "", MailBody As String = ""
        Dim sbStyle As New System.Text.StringBuilder
        With sbStyle
            .AppendLine("<style>")
            .AppendLine("BODY,TD,INPUT,SELECT,TEXTAREA {FONT-SIZE: 8pt;FONT-FAMILY: Arial,Helvetica,Sans-Serif} ")
            .AppendLine("A, A:visited {COLOR: #6666cc;TEXT-DECORATION: none} ")
            .AppendLine("A:active  {TEXT-DECORATION: none} ")
            .AppendLine("A:hover   {TEXT-DECORATION: underline} ")
            .AppendLine("</style>")
        End With
        strStyle = sbStyle.ToString()
        Dim titlestr As String = "Order"
        If Not String.IsNullOrEmpty(M.DocType = Fixer.eDocType.EQ) Then
            titlestr = "Quote"
        End If
        Dim sbBody As New System.Text.StringBuilder
        Dim strOrderNO As String = M.Key
        With sbBody
            .AppendLine("<html><body><center>")
            .AppendLine("<table width=""731"" border=""0"" cellspacing=""0"" cellpadding=""0"">")
            .AppendLine("<tr><td colspan=""3"">")
            .AppendLine("&nbsp;<font size=5 color=""#000000""><b>Failed " + titlestr + " Message</b></font>&nbsp;&nbsp;&nbsp;&nbsp;" & "<br><br>")
            .AppendLine("</td></tr>")
            .AppendLine("</table>")

            .AppendLine("<table width=""731"" border=""0"" cellspacing=""0"" cellpadding=""0"">")
            .AppendLine("<tr><td align=""left"" style=""padding-left:10px;border-bottom:#ffffff 1px solid"" valign=""middle"" height=""20"" bgcolor=""#6699CC""><font color=""#ffffff"">")
            .AppendLine("&nbsp;<b>Message</b>")
            .AppendLine("</td></tr>")
            .AppendLine("<tr><td align=""left"" width=""100%"" style=""padding-left:10px;border-bottom:#ffffff 1px solid"" valign=""middle"" height=""18"" bgcolor=""#d8e4f8""><font color=""#316ac5"">")
            .AppendLine("&nbsp;<b>Order Process Message(<font color=""green"">" & strOrderNO & "</font>)</b>")
            .AppendLine("</td></tr>")
            .AppendLine("<tr><td>")
            .AppendLine("<table width=""731"" bgcolor=""#DCDCDC"" style=""border:#CFCFCF 1px solid"" class=""text"" cellspacing=""0"" cellpadding=""0"">")
        End With
        strBody = sbBody.ToString()
        Dim strCC As String = "", l_strSQLCmd As String = "", strCC_External As String = ""
        Dim k As Integer = GetPIcc(strOrderNO, strCC, strCC_External, M.org, M.AccErpId)
        Dim Message_DT As New DataTable, myOrderStatus As New ORDER_PROC_STATUS2
        Message_DT = myOrderStatus.GetDT(String.Format("order_no='{0}'", strOrderNO), "LINE_SEQ")
        If Message_DT.Rows.Count > 0 Then
            Dim j As Integer = 0
            While j <= Message_DT.Rows.Count - 1
                strBody = strBody & "<tr><td bgcolor=""#ffffff""><font size=3>"
                strBody = strBody & "&nbsp;&nbsp;+&nbsp;<font color=""red"">" & Message_DT.Rows(j).Item("MESSAGE")
                strBody = strBody & "</font></td></tr>"
                j = j + 1
            End While
        Else
            strBody = strBody & "<tr><td bgcolor=""#ffffff""><font size=3>"
            strBody = strBody & "&nbsp;&nbsp;+&nbsp;<font color=""red"">" & "No message"
            strBody = strBody & "</font></td></tr>"
        End If
        If String.IsNullOrEmpty(M.Key.Trim) Then
            strBody = strBody & "<tr><td height=""5"" bgcolor=""#ffffff"">"
            strBody = strBody & "&nbsp;"
            strBody = strBody & "</td></tr>"
            strBody = strBody & "<tr><td height=""5"" align=""center"" bgcolor=""#ffffff""><font size=3><i><u>"
            strBody = strBody & "<a href=""http://" & System.Web.HttpContext.Current.Request.ServerVariables("HTTP_HOST") & "/order/OrderPreview.aspx?OID=" & strOrderNO & """><i><b><font size=4 color=""red"">Press Link To Recover This Order</font></b></i></a>"
            strBody = strBody & "</u></i></font></td></tr>"
        End If
        strBody = strBody & "</table>"
        strBody = strBody & "</td></tr>"
        strBody = strBody & "</table>"
        strBody = strBody & "</body></html>"

        t_strHTML = Replace(strBody, "<body>", "<body>" & strStyle)
        Dim strPONo As String = "", strCompanyId As String = ""
        strPONo = M.PO_NO : strCompanyId = M.AccErpId

        Dim strCompanyName As String = ""
        strCompanyName = M.AccName

        FROM_Email = "myadvantech@advantech.com" : TO_Email = strCC : CC_Email = "myadvantech@advantech.com;"
        Try
            Dim FailedNotifyList As ArrayList = GetFailedOrderNotifyList(strCompanyId, M.org)
            If FailedNotifyList.Count > 0 Then
                CC_Email += String.Join(";", FailedNotifyList.ToArray())
            End If
        Catch ex As Exception
            Throw ex
        End Try

        BCC_Email = ""
        Subject_Email = "Advantech Failed Order(" & strPONo & "/" & strOrderNO & ") for " & strCompanyName & " (" & strCompanyId & ")"
        If strOrderNO.StartsWith("AUSQ", StringComparison.CurrentCultureIgnoreCase) _
                OrElse strOrderNO.StartsWith("AMXQ", StringComparison.CurrentCultureIgnoreCase) Then
            Subject_Email = "Create SAP Quote Failed: " & strOrderNO & " for (" & M.Key & ")"
        End If
        AttachFile = "" : MailBody = t_strHTML


        If strOrderNO.StartsWith("AUSQ", StringComparison.CurrentCultureIgnoreCase) _
            OrElse strOrderNO.StartsWith("AMXO", StringComparison.CurrentCultureIgnoreCase) _
            OrElse strOrderNO.StartsWith("AMXQ", StringComparison.CurrentCultureIgnoreCase) Then
            Dim Expandstr As String = String.Format("From:{0}<hr/>To:{1}<hr/>CC:{2}<hr/>Bcc:{3}<hr/>", FROM_Email, TO_Email, CC_Email, BCC_Email)


            Util.SendEmail(FROM_Email, "MyAdvantech@advantech.com", CC_Email, BCC_Email, Subject_Email, AttachFile, MailBody, "")
        Else
            Util.SendEmail(FROM_Email, TO_Email, CC_Email, BCC_Email, Subject_Email, AttachFile, MailBody, "")
        End If

    End Sub
    Public Function GetPIcc(ByVal Order_no As String, ByRef Str_cc As String, ByRef Str_cc_External As String, ByRef Org As String, ByRef CompanyId As String) As Integer
        Dim InvalidOrg As String = ConfigurationManager.AppSettings("InvalidOrg").ToString.Trim()
        Dim OracleSCP As New System.Text.StringBuilder
        OracleSCP.AppendLine(" select b.kunnr as COMPANY_ID, b.vkorg as ORG_ID, b.vtweg as DIST_CHANN, ")
        OracleSCP.AppendLine(" b.spart as DIVISION, b.parvw as PARTNER_FUNCTION, b.kunn2 as PARENT_COMPANY_ID, ")
        OracleSCP.AppendLine(" b.lifnr as VENDOR_CREDITOR, b.pernr as SALES_CODE, b.parnr as PARTNER_NUMBER, b.KNREF,  ")
        OracleSCP.AppendLine(" b.DEFPA from saprdp.kna1 a inner join saprdp.knvp b on a.kunnr=b.kunnr ")
        OracleSCP.AppendLine(" where a.mandt='168' and b.mandt='168' ")
        'OracleSCP.AppendLine(" and b.vkorg in ('AU01','BR01','CN01','CN10','EU10','JP01','KR01','MY01','SG01','TL01','TW01','US01') ")
        OracleSCP.AppendFormat(" and b.vkorg in ('{0}') ", Org)
        Dim OracleSEM As New System.Text.StringBuilder
        OracleSEM.AppendLine(" select a.pernr as sales_code, a.werks as pers_area, a.persg as emp_group, a.persk as sub_emp_group,  ")
        OracleSEM.AppendLine(" (select b.stras from saprdp.pa0006 b where b.pernr=a.pernr and rownum=1 and b.mandt='168') as address, ")
        OracleSEM.AppendLine(" (select b.land1 from saprdp.pa0006 b where b.pernr=a.pernr and rownum=1 and b.mandt='168') as country,  a.sname, a.ename, ")
        OracleSEM.AppendLine(" (select b.vorna from saprdp.pa0002 b where b.pernr=a.pernr and rownum=1 and b.mandt='168') as first_name, ")
        OracleSEM.AppendLine(" (select b.nachn from saprdp.pa0002 b where b.pernr=a.pernr and rownum=1 and b.mandt='168') as last_name, ")
        OracleSEM.AppendLine(" concat(concat((select b.vorna from saprdp.pa0002 b where b.pernr=a.pernr and rownum=1 and  ")
        OracleSEM.AppendLine(" b.mandt='168'),'.'),(select b.nachn from saprdp.pa0002 b where b.pernr=a.pernr and rownum=1 and b.mandt='168')) as full_name, ")
        OracleSEM.AppendLine(" (select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='0020' and rownum=1) as tel, ")
        OracleSEM.AppendLine(" decode((select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='MAIL'  ")
        OracleSEM.AppendLine(" and rownum=1),null,(select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and  ")
        OracleSEM.AppendLine(" b.subty='0010' and rownum=1),(select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='MAIL' and rownum=1)) as email, ")
        OracleSEM.AppendLine(" (select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='CELL' and rownum=1) as cellphone, ")
        OracleSEM.AppendLine(" (select b.usrid_long from saprdp.pa0105 b where b.pernr=a.pernr and b.subty='MPHN' and rownum=1) as otherphone, ")
        OracleSEM.AppendLine(" decode((select b.anzkd from saprdp.pa0002 b where b.mandt='168' and rownum=1 and b.pernr=a.pernr),null,0,(select b.anzkd from saprdp.pa0002 b where b.mandt='168' and rownum=1 and b.pernr=a.pernr)) as num_of_child, ")
        OracleSEM.AppendLine(" a.Abkrs as payr_area from saprdp.pa0001 a where a.mandt='168' ")

        Dim sql As String = "select distinct b.EMAIL  from (" + OracleSCP.ToString() + ") a inner join (" + OracleSEM.ToString() + ") b on a.SALES_CODE=b.SALES_CODE " _
                                                             & " and a.ORG_ID not in " + InvalidOrg + "" _
                                                             & " where a.COMPANY_ID='" + CompanyId + "'  ORDER BY b.EMAIL " ') and dbo.IsEmail(b.EMAIL)=1
        Dim CDT As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", sql)
        If CDT.Rows.Count > 0 Then
            For i As Integer = 0 To CDT.Rows.Count - 1
                With CDT.Rows(i)
                    If Not IsDBNull(.Item("EMAIL")) AndAlso Util.isEmail(.Item("EMAIL").ToString) Then
                        Str_cc = Str_cc & .Item("EMAIL").ToString & ";"
                    End If
                End With
            Next
        End If
        Dim QuotationDBName As String = "eQuotation" : If Util.IsTesting() Then QuotationDBName = "eQuotationStaging"
        Dim sql1 As String = "select CONTACT_EMAIL from SAP_COMPANY_CONTACTS where COMPANY_ID='" + CompanyId + "'"
        Dim sql2 As String = "select a.EMAIL as KEYEmail from sap_employee a inner join " + QuotationDBName + ".dbo.OrderMaster b on a.SALES_CODE=b.KEYPERSON WHERE a.PERS_AREA='" + Org + "'  and dbo.IsEmail(a.EMAIL)=1 and b.QuoteID='" + Order_no + "'"
        Dim dt As DataTable = dbUtil.dbGetDataTable("my", sql1 + " UNION " + sql2)
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                With dt.Rows(i)
                    If Not IsDBNull(.Item("CONTACT_EMAIL")) AndAlso .Item("CONTACT_EMAIL").ToString <> "" AndAlso Util.isEmail(.Item("CONTACT_EMAIL")) Then
                        If Util.IsInternalUser(.Item("CONTACT_EMAIL").ToString) Then
                            Str_cc = Str_cc & .Item("CONTACT_EMAIL").ToString & ";"
                        Else
                            Str_cc_External = Str_cc_External & .Item("CONTACT_EMAIL").ToString & ";"
                        End If
                    End If
                End With
            Next
        End If
        Return 1
    End Function
    Public Shared Function GetFailedOrderNotifyList(ByVal CompanyId As String, ByVal Org As String, Optional ByVal IsComponentOrder As Boolean = True) As ArrayList
        Dim arr As New ArrayList
        Select Case Left(UCase(Org), 2)
            Case "EU"
                arr.Add("eSCM-AESC@advantech.nl")
            Case "SG", "MY"
                arr.Add("asg.op@advantech.com")
            Case "US"
                arr.Add("Jay.Lee@advantech.com") : arr.Add("Mike.Liu@advantech.com")
            Case Else
                arr.Add("myadvantech@advantech.com")
        End Select
        Return arr
    End Function







    Public Function getBTOChildDueDate(ByVal reqDate As String, ByVal org As String) As String Implements IBUS.iDoc.getBTOChildDueDate
        Dim SAP As IBUS.iSAP = Pivot.NewObjSAPTools
        Return SAP.getBTOChildDueDate(reqDate, org)
    End Function

    Public Function getBTOParentDueDate(ByVal reqDate As String, ByVal org As String) As String Implements IBUS.iDoc.getBTOParentDueDate
        Dim SAP As IBUS.iSAP = Pivot.NewObjSAPTools
        Return SAP.getBTOParentDueDate(reqDate, org)
    End Function

    Public Function getCompNextWorkDate(ByVal reqDate As String, ByVal org As String, Optional ByVal days As Integer = 0) As String Implements IBUS.iDoc.getCompNextWorkDate
        Dim SAP As IBUS.iSAP = Pivot.NewObjSAPTools
        Return SAP.getCompNextWorkDate(reqDate, org, days)
    End Function



    Public Function getNextCustDelDate(ByVal ddate As String, ByVal CompanyId As String) As String Implements IBUS.iDoc.getNextCustDelDate
        Dim SAP As IBUS.iSAP = Pivot.NewObjSAPTools
        Return SAP.getNextCustDelDate(ddate, CompanyId)
    End Function

    Public Function GetNextWeeklyShippingDate(ByVal reqDate As Date, ByRef NextWeeklyShipDate As Date, ByVal CompanyID As String) As Boolean Implements IBUS.iDoc.GetNextWeeklyShippingDate
        Dim SAP As IBUS.iSAP = Pivot.NewObjSAPTools
        Return SAP.GetNextWeeklyShippingDate(reqDate, NextWeeklyShipDate, CompanyID)
    End Function





    Public Function UpdateOpportunityStatusRevenue(ByVal OptRowId As String, ByVal status As String, ByVal Revenue As Double, ByVal ConnectToACL As Boolean) As Boolean Implements IBUS.iDoc.UpdateOpportunityStatusRevenue
        Dim SAP As IBUS.iSiebel = Pivot.NewObjSiebelTools
        Return SAP.UpdateOpportunityStatusRevenue(OptRowId, status, Revenue, ConnectToACL)
    End Function

    Public Function UpdateSAPSOShipToAttentionAddress(ByVal SONO As String, ByVal ShipToId As String, ByVal Name As String, ByVal Attention As String, ByVal Street As String, ByVal Street2 As String, ByVal City As String, ByVal State As String, ByVal Zipcode As String, ByVal Country As String, ByRef ReturnTable As System.Data.DataTable, Optional ByVal IsSAPProductionServer As Boolean = True) As Boolean Implements IBUS.iDoc.UpdateSAPSOShipToAttentionAddress
        Dim SAP As IBUS.iSAP = Pivot.NewObjSAPTools
        Return SAP.UpdateSAPSOShipToAttentionAddress(SONO, ShipToId, Name, Attention, Street, Street2, City, State, Zipcode, Country, ReturnTable, IsSAPProductionServer)
    End Function

    Public Function UpdateSOSpecId(ByVal SO_NO As String, ByVal SpecialIndicator As COMM.Fixer.eEarlyShipment, ByRef ReturnTable As System.Data.DataTable) As Boolean Implements IBUS.iDoc.UpdateSOSpecId
        Dim SAP As IBUS.iSAP = Pivot.NewObjSAPTools
        Return SAP.UpdateSOSpecId(SO_NO, SpecialIndicator, ReturnTable)
    End Function

    Public Function UpdateSOZeroPriceItems(ByVal SO_NO As String, ByVal C As IBUS.iCartList, ByRef ReturnTable As System.Data.DataTable) As Boolean Implements IBUS.iDoc.UpdateSOZeroPriceItems
        Dim SAP As IBUS.iSAP = Pivot.NewObjSAPTools
        Return SAP.UpdateSOZeroPriceItems(SO_NO, C, ReturnTable)
    End Function

    Public Function VerifyDistChannelDivisionGroupOffice(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal strDistChann As String, ByVal strDivision As String, ByVal OrderDocType As Integer, ByVal SalesGroup As String, ByVal SalesOffice As String, ByRef ReturnTable As System.Data.DataTable) As Boolean Implements IBUS.iDoc.VerifyDistChannelDivisionGroupOffice
        Dim SAP As IBUS.iSAP = Pivot.NewObjSAPTools
        Return SAP.VerifyDistChannelDivisionGroupOffice(Org, SoldToId, ShipToId, strDistChann, strDivision, OrderDocType, SalesGroup, SalesOffice, ReturnTable)
    End Function

    Private Function IsValidUID(ByVal UID As String) As Boolean
        Dim O As New quotationMaster(COMM.Fixer.eDocType.EQ)
        'Dim DT As DataTable = O.GetDT(String.Format("QuoteId='{0}'", UID), "", "Count(QuoteId) as QuoteCount")
        'If IsNothing(DT) = False AndAlso DT.Rows.Count > 0 AndAlso DT.Rows(0).Item("QuoteCount") > 0 Then
        '    Return True
        'End If
        Dim _RecortCount As Integer = O.GetScalar(String.Format("QuoteId='{0}'", UID), "", "Count(QuoteId) as QuoteCount")
        If IsNothing(_RecortCount) = False AndAlso _RecortCount > 0 Then
            Return True
        End If
        O = New quotationMaster(COMM.Fixer.eDocType.ORDER)
        _RecortCount = O.GetScalar(String.Format("QuoteId='{0}'", UID), "", "Count(QuoteId) as QuoteCount")
        If IsNothing(_RecortCount) = False AndAlso _RecortCount > 0 Then
            Return True
        End If
        Return False
    End Function
    Public Function IsValidQuotationID(ByVal quoteid As String) As Boolean Implements IBUS.iDoc.IsValidQuotationID

        If String.IsNullOrEmpty(quoteid) Then Return False

        Return IsValidUID(quoteid)


    End Function


    Public Function CanUserAccessThisQuotation(ByVal quoteid As String, ByVal UserID As String) As Boolean Implements IBUS.iDoc.CanUserAccessThisQuotation

        Return True

        'Below code need to be implemented for each rbu user
        'If Role.IsUsaUser() Then
        'End If

        'If Role.IsEUSales() Then
        'End If

        'If Role.IsJPAonlineSales() Then
        'End If

    End Function

    Public Function getBTOWorkingDate(ByVal Org As String) As COMM.Fixer.eBTOAssemblyDays Implements IBUS.iDoc.getBTOWorkingDate
        Return Pivot.NewObjSAPTools.getBTOWorkingDate(Org)
    End Function
End Class

Public Class DocHeader : Implements iDocHeader


    Public Function Add(ByVal Header As IBUS.iDocHeaderLine, ByVal P As IBUS.iRole, ByVal oType As COMM.Fixer.eDocType) As iDocHeaderLine Implements IBUS.iDocHeader.Add
        Return AddByAssignedUID("", Header, P, oType)
    End Function

    Public Function GetByQuoteNoandRevisionNumber(ByVal QuoteNo As String, ByVal Revision_Number As Integer, ByVal oType As COMM.Fixer.eDocType) As IBUS.iDocHeaderLine Implements IBUS.iDocHeader.GetByQuoteNoandRevisionNumber

        Dim O As New quotationMaster(oType)
        Dim DT As DataTable = O.GetDT(String.Format("QuoteNo='{0}' and Revision_Number={1}", QuoteNo, Revision_Number), "", "quoteId")

        If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
            Return GetByUID(DT.Rows(0).Item("quoteId").ToString, oType)
        End If

        Return Nothing
    End Function

    Public Function GetActiveRevisionQuoteIDByQuoteNo(ByVal QuoteNo As String, ByVal oType As COMM.Fixer.eDocType, Optional ByRef ActiveVersion As String = "") As String Implements IBUS.iDocHeader.GetActiveRevisionQuoteIDByQuoteNo

        Dim O As New quotationMaster(oType)
        Dim DT As DataTable = O.GetDT(String.Format("QuoteNo='{0}' and Active=1", QuoteNo), "", "quoteId,Revision_Number")

        If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
            ActiveVersion = DT.Rows(0).Item("Revision_Number").ToString
            Return DT.Rows(0).Item("quoteId").ToString
        End If
        Return String.Empty
    End Function

    Public Function GetByUID(ByVal UID As String, ByVal oType As COMM.Fixer.eDocType) As IBUS.iDocHeaderLine Implements IBUS.iDocHeader.GetByDocID
        Dim O As New quotationMaster(oType)
        Dim DT As DataTable = O.GetDT(String.Format("QuoteId='{0}'", UID), "")
        If Not IsNothing(DT) AndAlso DT.Rows.Count > 0 Then
            Dim M As New HeaderLine
            If Not IsDBNull(DT.Rows(0).Item("QuoteId")) Then
                M.Key = DT.Rows(0).Item("QuoteId")
            End If

            'Frank 2013/06/26
            If Not IsDBNull(DT.Rows(0).Item("quoteNo")) Then
                M.quoteNo = DT.Rows(0).Item("quoteNo")
            End If
            If Not IsDBNull(DT.Rows(0).Item("Revision_Number")) Then
                M.Revision_Number = DT.Rows(0).Item("Revision_Number")
            End If
            If Not IsDBNull(DT.Rows(0).Item("Active")) Then
                M.Active = DT.Rows(0).Item("Active")
            End If

            If Not IsDBNull(DT.Rows(0).Item("CustomId")) Then
                M.CustomId = DT.Rows(0).Item("CustomId")
            End If
            If Not IsDBNull(DT.Rows(0).Item("quoteToErpId")) Then
                M.AccErpId = DT.Rows(0).Item("quoteToErpId")
            End If
            If Not IsDBNull(DT.Rows(0).Item("quoteDate")) Then
                M.DocDate = DT.Rows(0).Item("quoteDate")
            End If
            If Not IsDBNull(DT.Rows(0).Item("createdDate")) Then
                M.CreatedDate = DT.Rows(0).Item("createdDate")
            End If
            If Not IsDBNull(DT.Rows(0).Item("createdBy")) Then
                M.CreatedBy = DT.Rows(0).Item("createdBy")
            End If
            If Not IsDBNull(DT.Rows(0).Item("quoteToRowId")) Then
                M.AccRowId = DT.Rows(0).Item("quoteToRowId")
            End If
            If Not IsDBNull(DT.Rows(0).Item("quoteToName")) Then
                M.AccName = DT.Rows(0).Item("quoteToName")
            End If
            If Not IsDBNull(DT.Rows(0).Item("office")) Then
                M.AccOfficeName = DT.Rows(0).Item("office")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SALESOFFICE")) Then
                M.AccOfficeCode = DT.Rows(0).Item("SALESOFFICE")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ogroup")) Then
                M.AccGroupName = DT.Rows(0).Item("ogroup")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SALESGROUP")) Then
                M.AccGroupCode = DT.Rows(0).Item("SALESGROUP")
            End If
            If Not IsDBNull(DT.Rows(0).Item("salesEmail")) Then
                M.salesEmail = DT.Rows(0).Item("salesEmail")
            End If
            If Not IsDBNull(DT.Rows(0).Item("salesRowId")) Then
                M.salesRowId = DT.Rows(0).Item("salesRowId")
            End If
            If Not IsDBNull(DT.Rows(0).Item("directPhone")) Then
                M.directPhone = DT.Rows(0).Item("directPhone")
            End If
            If Not IsDBNull(DT.Rows(0).Item("attentionRowId")) Then
                M.attentionRowId = DT.Rows(0).Item("attentionRowId")
            End If
            If Not IsDBNull(DT.Rows(0).Item("attentionEmail")) Then
                M.attentionEmail = DT.Rows(0).Item("attentionEmail")
            End If
            If Not IsDBNull(DT.Rows(0).Item("bankInfo")) Then
                M.bankInfo = DT.Rows(0).Item("bankInfo")
            End If
            If Not IsDBNull(DT.Rows(0).Item("deliveryDate")) Then
                M.deliveryDate = DT.Rows(0).Item("deliveryDate")
            End If
            If Not IsDBNull(DT.Rows(0).Item("expiredDate")) Then
                M.expiredDate = DT.Rows(0).Item("expiredDate")
            End If
            If Not IsDBNull(DT.Rows(0).Item("shipTerm")) Then
                M.shipTerm = DT.Rows(0).Item("shipTerm")
            End If
            If Not IsDBNull(DT.Rows(0).Item("paymentTerm")) Then
                M.paymentTerm = DT.Rows(0).Item("paymentTerm")
            End If
            If Not IsDBNull(DT.Rows(0).Item("freight")) Then
                M.freight = DT.Rows(0).Item("freight")
            End If
            If Not IsDBNull(DT.Rows(0).Item("insurance")) Then
                M.insurance = DT.Rows(0).Item("insurance")
            End If
            If Not IsDBNull(DT.Rows(0).Item("specialCharge")) Then
                M.specialCharge = DT.Rows(0).Item("specialCharge")
            End If
            If Not IsDBNull(DT.Rows(0).Item("tax")) Then
                M.tax = DT.Rows(0).Item("tax")
            End If
            If Not IsDBNull(DT.Rows(0).Item("quoteNote")) Then
                M.quoteNote = DT.Rows(0).Item("quoteNote")
            End If
            If Not IsDBNull(DT.Rows(0).Item("relatedInfo")) Then
                M.relatedInfo = DT.Rows(0).Item("relatedInfo")
            End If
            If Not IsDBNull(DT.Rows(0).Item("preparedBy")) Then
                M.preparedBy = DT.Rows(0).Item("preparedBy")
            End If
            If Not IsDBNull(DT.Rows(0).Item("currency")) Then
                M.Currency = DT.Rows(0).Item("currency")
            End If
            If Not IsDBNull(DT.Rows(0).Item("DOCSTATUS")) Then
                M.qStatus = DT.Rows(0).Item("DOCSTATUS")
            End If
            If Not IsDBNull(DT.Rows(0).Item("DOCSTATUS")) Then
                M.DOCSTATUS = DT.Rows(0).Item("DOCSTATUS")
            End If
            If Not IsDBNull(DT.Rows(0).Item("isShowListPrice")) Then
                M.isShowListPrice = DT.Rows(0).Item("isShowListPrice")
            End If
            If Not IsDBNull(DT.Rows(0).Item("isShowDiscount")) Then
                M.isShowDiscount = DT.Rows(0).Item("isShowDiscount")
            End If
            If Not IsDBNull(DT.Rows(0).Item("isShowDueDate")) Then
                M.isShowDueDate = DT.Rows(0).Item("isShowDueDate")
            End If
            If Not IsDBNull(DT.Rows(0).Item("isLumpSumOnly")) Then
                M.isLumpSumOnly = DT.Rows(0).Item("isLumpSumOnly")
            End If
            If Not IsDBNull(DT.Rows(0).Item("isRepeatedOrder")) Then
                M.isRepeatedOrder = DT.Rows(0).Item("isRepeatedOrder")
            End If
            If Not IsDBNull(DT.Rows(0).Item("DelDateFlag")) Then
                M.delDateFlag = DT.Rows(0).Item("DelDateFlag")
            End If
            If Not IsDBNull(DT.Rows(0).Item("org")) Then
                M.org = DT.Rows(0).Item("org")
            End If
            If Not IsDBNull(DT.Rows(0).Item("siebelRBU")) Then
                M.siebelRBU = DT.Rows(0).Item("siebelRBU")
            End If
            If Not IsDBNull(DT.Rows(0).Item("DIST_CHAN")) Then
                M.DIST_CHAN = DT.Rows(0).Item("DIST_CHAN")
            End If
            If Not IsDBNull(DT.Rows(0).Item("DIVISION")) Then
                M.DIVISION = DT.Rows(0).Item("DIVISION")
            End If
            If Not IsDBNull(DT.Rows(0).Item("PO_NO")) Then
                M.PO_NO = DT.Rows(0).Item("PO_NO")
            End If
            If Not IsDBNull(DT.Rows(0).Item("CARE_ON")) Then
                M.CARE_ON = DT.Rows(0).Item("CARE_ON")
            End If
            If Not IsDBNull(DT.Rows(0).Item("isExempt")) Then
                M.isExempt = DT.Rows(0).Item("isExempt")
            End If
            If Not IsDBNull(DT.Rows(0).Item("INCO1")) Then
                M.Inco1 = DT.Rows(0).Item("INCO1")
            End If
            If Not IsDBNull(DT.Rows(0).Item("INCO2")) Then
                M.Inco2 = DT.Rows(0).Item("INCO2")
            End If
            If Not IsDBNull(DT.Rows(0).Item("DISTRICT")) Then
                M.SalesDistrict = DT.Rows(0).Item("DISTRICT")
            End If
            If Not IsDBNull(DT.Rows(0).Item("PRINTOUT_FORMAT")) Then
                M.PRINTOUT_FORMAT = DT.Rows(0).Item("PRINTOUT_FORMAT")
            End If
            If Not IsDBNull(DT.Rows(0).Item("OriginalQuoteID")) Then
                M.OriginalQuoteID = DT.Rows(0).Item("OriginalQuoteID")
            End If
            If Not IsDBNull(DT.Rows(0).Item("DocReg")) Then
                M.DocReg = DT.Rows(0).Item("DocReg")
            End If
            'Ming  for Quote2.0   2013-08-22
            If String.Equals(UID, M.quoteNo, StringComparison.CurrentCultureIgnoreCase) Then
                If M.quoteNo.StartsWith("AUSQ", StringComparison.CurrentCultureIgnoreCase) Then
                    M.DocReg = 96
                End If
            End If
            'end Ming for Quote2.0   2013-08-22
            If Not IsDBNull(DT.Rows(0).Item("DocType")) Then
                M.DocType = DT.Rows(0).Item("DocType")
            End If
            If Not IsDBNull(DT.Rows(0).Item("ReqDate")) Then
                M.ReqDate = DT.Rows(0).Item("ReqDate")
            End If
            If Not IsDBNull(DT.Rows(0).Item("Partial")) Then
                M.PartialF = DT.Rows(0).Item("Partial")
            End If
            If Not IsDBNull(DT.Rows(0).Item("LastUpdatedDate")) Then
                M.LastUpdatedDate = DT.Rows(0).Item("LastUpdatedDate")
            End If
            If Not IsDBNull(DT.Rows(0).Item("LastUpdatedBy")) Then
                M.LastUpdatedBy = DT.Rows(0).Item("LastUpdatedBy")
            End If
            If Not IsDBNull(DT.Rows(0).Item("IS_EARLYSHIP")) Then
                M.IS_EARLYSHIP = DT.Rows(0).Item("IS_EARLYSHIP")
            End If
            If Not IsDBNull(DT.Rows(0).Item("DocRealType")) Then
                M.DocRealType = DT.Rows(0).Item("DocRealType")
            End If
            If Not IsDBNull(DT.Rows(0).Item("PODate")) Then
                M.PODate = DT.Rows(0).Item("PODate")
            End If
            If Not IsDBNull(DT.Rows(0).Item("KEYPERSON")) Then
                M.KEYPERSON = DT.Rows(0).Item("KEYPERSON")
            End If
            If Not IsDBNull(DT.Rows(0).Item("EMPLOYEEID")) Then
                M.EMPLOYEEID = DT.Rows(0).Item("EMPLOYEEID")
            End If
            If Not IsDBNull(DT.Rows(0).Item("isvirpartOnly")) Then
                M.ISVIRPARTONLY = DT.Rows(0).Item("isvirpartOnly")
            End If

            If Not IsDBNull(DT.Rows(0).Item("TAXDEPCITY")) Then
                M.TAXDEPCITY = DT.Rows(0).Item("TAXDEPCITY")
            End If
            If Not IsDBNull(DT.Rows(0).Item("TAXDSTCITY")) Then
                M.TAXDSTCITY = DT.Rows(0).Item("TAXDSTCITY")
            End If
            If Not IsDBNull(DT.Rows(0).Item("TRIANGULARINDICATOR")) Then
                M.TRIANGULARINDICATOR = DT.Rows(0).Item("TRIANGULARINDICATOR")
            End If
            If Not IsDBNull(DT.Rows(0).Item("TAXCLASS")) Then
                M.TAXCLASS = DT.Rows(0).Item("TAXCLASS")
            End If
            If Not IsDBNull(DT.Rows(0).Item("SHIPCUSTPONO")) Then
                M.SHIPCUSTPONO = DT.Rows(0).Item("SHIPCUSTPONO")
            End If
            If Not IsDBNull(DT.Rows(0).Item("quoteNo")) Then
                M.quoteNo = DT.Rows(0).Item("quoteNo")
            End If
            If Not IsDBNull(DT.Rows(0).Item("Revision_Number")) Then
                M.Revision_Number = DT.Rows(0).Item("Revision_Number")
            End If
            If Not IsDBNull(DT.Rows(0).Item("Active")) Then
                M.Active = DT.Rows(0).Item("Active")
            End If

            Return M

        End If
        Return Nothing
    End Function


    Public Function AddByAssignedUID(ByVal UID As String, ByVal Header As IBUS.iDocHeaderLine, ByVal P As IBUS.iRole, ByVal oType As COMM.Fixer.eDocType) As iDocHeaderLine Implements IBUS.iDocHeader.AddByAssignedUID
        Dim doc As New Doc
        Dim _CreatedBy As Object = dbUtil.dbExecuteScalar("EQ", String.Format("select  top 1 createdBy from QuotationMaster where quoteId ='{0}'", UID))
        If _CreatedBy IsNot Nothing Then
            Header.CreatedBy = _CreatedBy.ToString.Trim
        Else
            Header.CreatedBy = P.UserId
        End If
        Header.LastUpdatedBy = P.UserId

        'Ryan 20180420 Created Date should take input parameters instead of using now.date
        'Header.CreatedDate = doc.GetLocalTime(Now.Date.ToShortDateString)

        If Header.DocType = Fixer.eDocType.EQ Then
            Header.qStatus = CInt(COMM.Fixer.eDocStatus.QDRAFT)
        Else
            Header.qStatus = CInt(COMM.Fixer.eDocStatus.ODRAFT)
        End If
        Header.DocDate = doc.GetLocalTime(Header.CreatedDate)
        If Header.expiredDate = CDate(COMM.Fixer.StartDate) Then
            Header.expiredDate = CDate(Header.DocDate).AddDays(COMM.MasterFixer.getExpDaysByReg(P.CurrDocReg)).ToShortDateString
        End If
        Header.deliveryDate = DateAdd(DateInterval.Day, 3, Header.DocDate)

        If Header.DocReg = Fixer.eDocReg.ATW Then
            Header.DocReg = P.CurrDocReg
            'Frank 20171003
            'Updating column "Preparedby" to current user
            Header.salesEmail = P.UserId
        End If
        If String.IsNullOrEmpty(UID) Then
            'UID = (New Doc).NewId(P.CurrDocReg, Header.DocType)
            UID = (New Doc).NewUID()
        End If

        'Frank 2013/07/01
        If String.IsNullOrEmpty(Header.quoteNo) Then

            'Frank 20141203 If user belongs to Aonline and AAC group, then generate the quote number by
            ' quote to account status,
            ' AACQ for CP and KA
            ' AUSQ for IAG AOnline
            If (P.DocRegList.Contains(COMM.Fixer.eDocReg.AAC) AndAlso P.SalesOfficeCode.Contains("2100")) AndAlso _
                (P.DocRegList.Contains(COMM.Fixer.eDocReg.ANA) AndAlso P.SalesOfficeCode.Contains("2110")) Then

                'Get Account status
                Dim _Account_Status As String = dbUtil.dbExecuteScalar("MY" _
                , "select ACCOUNT_STATUS from MyAdvantechGlobal.dbo.SIEBEL_ACCOUNT where ROW_ID='" & Header.AccRowId.Replace("'", "''") & "'")
                Dim _account_status_enum As SAPDAL.UserRole.AccountStatus
                'Dim _account_statusid As String = COMM.MasterFixer.getAccStatus(_Account_Status)
                Dim _account_statusid As String = SAPDAL.UserRole.getAccStatus(_Account_Status)
                [Enum].TryParse(Of SAPDAL.UserRole.AccountStatus)(_account_statusid, _account_status_enum)
                Dim _TempOrderPrefix As String = SAPDAL.UserRole.GetAUSDocNumberPrefixStringByGroup(P.GroupBelTo, _account_status_enum, SAPDAL.UserRole.DocType.Quote)
                Header.quoteNo = (New Doc).SO_GetNumber(_TempOrderPrefix)

            Else
                Header.quoteNo = (New Doc).NewQuotationNo(P.CurrDocReg, Header.DocType)
            End If

        End If

        Header.Key = UID
        Remove(UID, oType)
        Dim o As New quotationMaster(oType)
        With Header
            'o.Add()
            o.Add(UID, .CustomId, .AccRowId, .AccErpId, .AccName, _
                  .AccOfficeName, .currency, .salesEmail, .salesRowId, .directPhone, _
                  .attentionRowId, .attentionEmail, .bankInfo, .DocDate, .deliveryDate, _
                  .expiredDate, .shipTerm, .paymentTerm, .freight, .insurance, _
                  .specialCharge, .tax, .quoteNote, .relatedInfo, .CreatedBy, _
                  .CreatedDate, .preparedBy, .qStatus, .isShowListPrice, .isShowDiscount, _
                  .isShowDueDate, .isLumpSumOnly, .isRepeatedOrder, .AccGroupName, .delDateFlag, _
                  .org, .siebelRBU, .DIST_CHAN, .DIVISION, .AccGroupCode, _
                  .AccOfficeCode, .PO_NO, .CARE_ON, .isExempt, .Inco1, _
                  .Inco2, .SalesDistrict, .PRINTOUT_FORMAT, .OriginalQuoteID, .DocReg, _
                  .DocType, .ReqDate, .PartialF, .IS_EARLYSHIP, .DocRealType, _
                  .LastUpdatedDate, .LastUpdatedBy, .PODate, .KEYPERSON, .EMPLOYEEID, _
                  .ISVIRPARTONLY, .TAXDEPCITY, .TAXDSTCITY, .TRIANGULARINDICATOR, .TAXCLASS, _
                  .SHIPCUSTPONO, .quoteNo, .Revision_Number, .Active)
        End With
        Return Header
    End Function

    Public Function Update(ByVal UID As String, ByVal SQLSTR As String, ByVal oType As COMM.Fixer.eDocType) As Integer Implements IBUS.iDocHeader.Update
        Dim o As New quotationMaster(oType)
        Return o.Update(String.Format("quoteId='{0}'", UID), SQLSTR)
    End Function

    Public Sub Remove(ByVal Key As String, ByVal oType As COMM.Fixer.eDocType) Implements IBUS.iDocHeader.Remove
        Dim o As New quotationMaster(oType)
        o.Delete(String.Format("quoteId='{0}'", Key))
    End Sub

    Public Sub UpdateAccErpId(ByVal rowId As String, ByVal erpId As String, ByVal oType As COMM.Fixer.eDocType) Implements IBUS.iDocHeader.UpdateAccErpId
        Dim myQM As New quotationMaster(oType)
        myQM.Update(String.Format("quoteToRowId='{0}'", rowId), String.Format("quoteToErpId='{0}'", erpId))
    End Sub
    Private _errCode As COMM.Msg.eErrCode = Msg.eErrCode.OK
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property

    Public Sub ChangeDocStatus(ByVal DocId As String, ByVal NewStatus As COMM.Fixer.eDocStatus, ByVal oType As COMM.Fixer.eDocType) Implements IBUS.iDocHeader.ChangeDocStatus
        Me.Update(DocId, String.Format("DOCSTATUS='{0}'", CInt(NewStatus)), oType)
    End Sub

    Public Function GetConstantUbiquitousLineByDocID(ByVal DocID As String, ByVal oType As COMM.Fixer.eDocType) As IBUS.iConstantUbiquitousLine Implements IBUS.iDocHeader.GetConstantUbiquitousLineByDocID
        Dim HL As IBUS.iDocHeaderLine = GetByUID(DocID, oType)
        If Not IsNothing(HL) Then
            Dim CFL As New CULine
            CFL.AccErpId = HL.AccErpId
            CFL.AccRowId = HL.AccRowId
            CFL.CreatedBy = HL.CreatedBy
            CFL.DocReg = HL.DocReg
            CFL.DocType = HL.DocType
            CFL.Org = HL.org
            CFL.AccGroupCode = HL.AccOfficeCode
            CFL.AccGroupName = HL.AccGroupName
            CFL.AccOfficeCode = HL.AccOfficeCode
            CFL.AccOfficeName = HL.AccOfficeName
            CFL.DocRealType = HL.DocRealType
            CFL.Key = HL.Key
            CFL.SiebelRBU = HL.siebelRBU
            CFL.Currency = HL.currency
            CFL.isExempt = HL.isExempt
            Return CFL
        End If
        Return Nothing
    End Function



    Public Function CopyPasteHeaderLine(ByVal OrgQuoteId As String, ByVal NewQuoteId As String, ByVal P As IBUS.iRole, ByVal oType As COMM.Fixer.eDocType) As IBUS.iDocHeaderLine Implements IBUS.iDocHeader.CopyPasteHeaderLine
        Dim dth As iDocHeaderLine = GetByUID(OrgQuoteId, oType)
        dth.OriginalQuoteID = OrgQuoteId
        dth.CustomId = ""
        dth.PO_NO = ""
        'Frank 2013/08/06 Need to assign new quote no 移除QuoteNo后面会根据AccountRowid 取得相应单号
        dth.quoteNo = "" ' (New Doc).NewQuotationNo(P.CurrDocReg, oType)
        dth.Revision_Number = 1 : dth.Active = True
        'Frank 2013/09/19: Reset the value of expired date, therefore it can be added the default working date
        dth.expiredDate = CDate(COMM.Fixer.StartDate)
        'Ming 20150130 copy的时候运费相关不进行copy
        dth.Inco2 = String.Empty
        dth.freight = 0
        dth.LastUpdatedDate = Now.Date

        If P.DocRegList.Contains(COMM.Fixer.eDocReg.ATW) Then
            dth.tax = 0.05
        End If

        Return AddByAssignedUID(NewQuoteId, dth, P, oType)

    End Function

    Public Function ReviseHeaderLine(ByVal QuoteId As String, ByVal CreateBy As String, ByVal P As IBUS.iRole, ByVal oType As COMM.Fixer.eDocType) As IBUS.iDocHeaderLine Implements IBUS.iDocHeader.ReviseHeaderLine
        Dim dth As iDocHeaderLine = GetByUID(QuoteId, oType)
        Dim NewQuoteId = (New Doc).NewUID()
        dth.OriginalQuoteID = QuoteId

        'Dim o As New quotationMaster

        Dim O As New quotationMaster(oType)
        O.SetAllQuoteInactiveByQuoteNo(dth.quoteNo)
        Dim DT As DataTable = O.GetDT(String.Format("QuoteNo='{0}'", dth.quoteNo), "", "(max(revision_number) + 1) as next_revision_number")

        If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
            dth.Revision_Number = DT.Rows(0).Item("next_revision_number")
        Else
            dth.Revision_Number = 1
        End If

        dth.Active = True
        dth.CreatedBy = CreateBy
        dth.CreatedDate = Now.Date
        'Doc Date maps to quote date
        'dth.DocDate = Now.Date
        dth.LastUpdatedBy = CreateBy
        dth.LastUpdatedDate = Now.Date
        'dth.PODate = Now.Date
        If dth.quoteNo.StartsWith("TWQ", StringComparison.InvariantCultureIgnoreCase) Then
            dth.expiredDate = DateAdd(DateInterval.Day, 30, Now.Date)
        End If

        Return AddByAssignedUID(NewQuoteId, dth, P, oType)
    End Function


    Public Sub SetRevisionQuoteToActive(ByVal QuoteId As String, ByVal oType As COMM.Fixer.eDocType) Implements IBUS.iDocHeader.SetRevisionQuoteToActive
        Dim dth As iDocHeaderLine = GetByUID(QuoteId, oType)
        'Dim NewQuoteId = (New Doc).NewUID()
        'dth.OriginalQuoteID = QuoteId

        Dim O As New quotationMaster(oType)
        O.SetAllQuoteInactiveByQuoteNo(dth.quoteNo)
        O.SetQuoteActive(dth.quoteNo, dth.Revision_Number)

    End Sub

    Public Function IsV2_0Quotation(ByVal QuoteId As String) As Boolean Implements IBUS.iDocHeader.IsV2_0Quotation
        Dim dth As iDocHeaderLine = GetByUID(QuoteId, Fixer.eDocType.EQ)
        If dth Is Nothing Then Return False
        If dth.Key.Equals(dth.quoteNo, StringComparison.InvariantCultureIgnoreCase) Then Return True
        Return False
    End Function

    Public Function GetRevisionsAr(ByVal QuoteNo As String) As System.Collections.ArrayList Implements IBUS.iDocHeader.GetRevisionsAr
        Dim o As New quotationMaster(COMM.Fixer.eDocType.EQ)
        Return o.getRevisionAr(QuoteNo)
    End Function

    Public Function GetActiveRevisionNo(ByVal QuoteNo As String) As String Implements IBUS.iDocHeader.GetActiveRevisionNo
        Dim o As New quotationMaster(COMM.Fixer.eDocType.EQ)
        Return o.getActiveRevisionNo(QuoteNo)
    End Function
End Class


Public Class CULine : Implements IBUS.iConstantUbiquitousLine

    Private _AccErpId As String = ""
    Public Property AccErpId As String Implements IBUS.iConstantUbiquitousLine.AccErpId
        Get
            Return _AccErpId
        End Get
        Set(ByVal value As String)
            _AccErpId = value
        End Set
    End Property
    Private _AccRowId As String = ""
    Public Property AccRowId As String Implements IBUS.iConstantUbiquitousLine.AccRowId
        Get
            Return _AccRowId
        End Get
        Set(ByVal value As String)
            _AccRowId = value
        End Set
    End Property
    Private _CreatedBy As String = ""
    Public Property CreatedBy As String Implements IBUS.iConstantUbiquitousLine.CreatedBy
        Get
            Return _CreatedBy
        End Get
        Set(ByVal value As String)
            _CreatedBy = value
        End Set
    End Property
    Private _DocReg As COMM.Fixer.eDocReg = Fixer.eDocReg.ATW
    Public Property DocReg As COMM.Fixer.eDocReg Implements IBUS.iConstantUbiquitousLine.DocReg
        Get
            Return _DocReg
        End Get
        Set(ByVal value As COMM.Fixer.eDocReg)
            _DocReg = value
        End Set
    End Property
    Private _DocType As COMM.Fixer.eDocType = Fixer.eDocType.EQ
    Public Property DocType As COMM.Fixer.eDocType Implements IBUS.iConstantUbiquitousLine.DocType
        Get
            Return _DocType
        End Get
        Set(ByVal value As COMM.Fixer.eDocType)
            _DocType = value
        End Set
    End Property
    Private _Org As String = ""
    Public Property Org As String Implements IBUS.iConstantUbiquitousLine.Org
        Get
            Return _Org
        End Get
        Set(ByVal value As String)
            _Org = value
        End Set
    End Property
    Private _AccGroupCode As String = ""
    Public Property AccGroupCode As String Implements IBUS.iConstantUbiquitousLine.AccGroupCode
        Get
            Return _AccGroupCode
        End Get
        Set(ByVal value As String)
            _AccGroupCode = value
        End Set
    End Property
    Private _AccGroupName As String = ""
    Public Property AccGroupName As String Implements IBUS.iConstantUbiquitousLine.AccGroupName
        Get
            Return _AccGroupName
        End Get
        Set(ByVal value As String)
            _AccGroupName = value
        End Set
    End Property
    Private _AccOfficeCode As String = ""
    Public Property AccOfficeCode As String Implements IBUS.iConstantUbiquitousLine.AccOfficeCode
        Get
            Return _AccOfficeCode
        End Get
        Set(ByVal value As String)
            _AccOfficeCode = value
        End Set
    End Property
    Private _AccOfficeName As String = ""
    Public Property AccOfficeName As String Implements IBUS.iConstantUbiquitousLine.AccOfficeName
        Get
            Return _AccOfficeName
        End Get
        Set(ByVal value As String)
            _AccOfficeName = value
        End Set
    End Property
    Private _DocRealType As String = ""
    Public Property DocRealType As String Implements IBUS.iConstantUbiquitousLine.DocRealType
        Get
            Return _DocRealType
        End Get
        Set(ByVal value As String)
            _DocRealType = value
        End Set
    End Property
    Private _Key As String = ""
    Public Property Key As String Implements IBUS.iConstantUbiquitousLine.Key
        Get
            Return _Key
        End Get
        Set(ByVal value As String)
            _Key = value
        End Set
    End Property
    Private _SiebelRBU As String = ""
    Public Property SiebelRBU As String Implements IBUS.iConstantUbiquitousLine.SiebelRBU
        Get
            Return _SiebelRBU
        End Get
        Set(ByVal value As String)
            _SiebelRBU = value
        End Set
    End Property
    Private _Currency As String = ""
    Public Property Currency As String Implements IBUS.iConstantUbiquitousLine.Currency
        Get
            Return _Currency
        End Get
        Set(ByVal value As String)
            _Currency = value
        End Set
    End Property
    Private _isExempt As Integer = 0
    Public Property isExempt As Integer Implements IBUS.iConstantUbiquitousLine.isExempt
        Get
            Return _isExempt
        End Get
        Set(ByVal value As Integer)
            _isExempt = value
        End Set
    End Property
End Class

Public Class DOCHeaderExtension : Implements IBUS.iDOCHeaderExtension

    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get

        End Get
    End Property

    Public Function GetDOCHeaderExtension(ByVal QuoteId As String) As IBUS.iDOCHeaderExtensionLine Implements IBUS.iDOCHeaderExtension.GetQuoteExtension

        Dim O As New QuotationExtension
        Dim DT As DataTable = O.GetDT(String.Format("QuoteId='{0}'", QuoteId), "")

        If DT Is Nothing Then Return Nothing
        If DT.Rows.Count = 0 Then Return Nothing

        Dim _headerExtensionLine As New DOCHeaderExtensionLine
        If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
            _headerExtensionLine.Key = DT.Rows(0).Item("QuoteID")
            _headerExtensionLine.EmailGreeting = DT.Rows(0).Item("EmailGreeting")
            _headerExtensionLine.SpecialTandC = DT.Rows(0).Item("SpecialTandC")
            _headerExtensionLine.SignatureRowID = DT.Rows(0).Item("SignatureRowID")
            _headerExtensionLine.Warranty = DT.Rows(0).Item("Warranty")
            _headerExtensionLine.LastUpdatedBy = DT.Rows(0).Item("LastUpdatedBy")
            _headerExtensionLine.LastUpdated = DT.Rows(0).Item("LastUpdated")
        End If
        DT = Nothing
        Return _headerExtensionLine

    End Function

    Public Function SetDOCHeaderExtension(ByVal DOCHeaderLine As IBUS.iDOCHeaderExtensionLine) As Object Implements IBUS.iDOCHeaderExtension.SetQuoteExtension

        Dim quoteExtension As New QuotationExtension

        Return quoteExtension.Add(DOCHeaderLine.Key, DOCHeaderLine.EmailGreeting, DOCHeaderLine.SpecialTandC, _
                                  DOCHeaderLine.SignatureRowID, DOCHeaderLine.Warranty, DOCHeaderLine.LastUpdatedBy)
    End Function


End Class

''' <summary>
''' 
''' </summary>
''' <remarks>Created by Frank</remarks>
Public Class DOCHeaderExtensionLine : Implements IBUS.iDOCHeaderExtensionLine

    Private _EmailGreeting As String = String.Empty
    Public Property EmailGreeting As String Implements IBUS.iDOCHeaderExtensionLine.EmailGreeting
        Get
            Return _EmailGreeting
        End Get
        Set(ByVal value As String)
            _EmailGreeting = value
        End Set
    End Property

    Private _Key As String = String.Empty
    Public Property Key As String Implements IBUS.iDOCHeaderExtensionLine.Key
        Get
            Return _Key
        End Get
        Set(ByVal value As String)
            _Key = value
        End Set
    End Property
    Private _LastUpdated As DateTime = Now
    Public Property LastUpdated As Date Implements IBUS.iDOCHeaderExtensionLine.LastUpdated
        Get
            Return _LastUpdated
        End Get
        Set(ByVal value As Date)
            _LastUpdated = value
        End Set
    End Property

    Private _LastUpdatedBy As String = String.Empty
    Public Property LastUpdatedBy As String Implements IBUS.iDOCHeaderExtensionLine.LastUpdatedBy
        Get
            Return _LastUpdatedBy
        End Get
        Set(ByVal value As String)
            _LastUpdatedBy = value
        End Set
    End Property

    Private _SignatureRowID As String = String.Empty
    Public Property SignatureRowID As String Implements IBUS.iDOCHeaderExtensionLine.SignatureRowID
        Get
            Return _SignatureRowID
        End Get
        Set(ByVal value As String)
            _SignatureRowID = value
        End Set
    End Property

    Private _SpecialTandC As String = String.Empty
    Public Property SpecialTandC As String Implements IBUS.iDOCHeaderExtensionLine.SpecialTandC
        Get
            Return _SpecialTandC
        End Get
        Set(ByVal value As String)
            _SpecialTandC = value
        End Set
    End Property

    Private _Warranty As String = String.Empty
    Public Property Warranty As String Implements IBUS.iDOCHeaderExtensionLine.Warranty
        Get
            Return _Warranty
        End Get
        Set(ByVal value As String)
            _Warranty = value
        End Set
    End Property

End Class



''' <summary>
''' 
''' </summary>
''' <remarks>Create by Frank for upload user's signature</remarks>
Public Class UserSignature : Implements IBUS.iUserSignature


    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get

        End Get
    End Property

    Public Function Add(UserSignatureline As IBUS.iUserSignatureLine) As Integer Implements IBUS.iUserSignature.Add
        Dim sign As New Signature
        Return sign.Add(UserSignatureline.Key, UserSignatureline.UserID, UserSignatureline.SignatureData, UserSignatureline.Active, UserSignatureline.FileName)
    End Function

    Public Function GetDefaultSignature(Userid As String) As String Implements IBUS.iUserSignature.GetDefaultSignature
        Dim sign As New Signature
        Dim _dt As DataTable = sign.GetDT(String.Format("UserId='{0}' and Active=1", Userid), "", "SID")
        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
            Return _dt.Rows(0).Item("SID")
        Else
            Return String.Empty
        End If
    End Function

End Class


Public Class UserSignatureLine : Implements IBUS.iUserSignatureLine
    Private _Active As Boolean = False
    Public Property Active As Boolean Implements IBUS.iUserSignatureLine.Active
        Get
            Return _Active
        End Get
        Set(value As Boolean)
            _Active = value
        End Set
    End Property

    Private _FileName As String = String.Empty
    Public Property FileName As String Implements IBUS.iUserSignatureLine.FileName
        Get
            Return _FileName
        End Get
        Set(value As String)
            _FileName = value
        End Set
    End Property

    Private _Key As String = String.Empty
    Public Property Key As String Implements IBUS.iUserSignatureLine.Key
        Get
            Return _Key
        End Get
        Set(value As String)
            _Key = value
        End Set
    End Property
    Private _LastUpdated As DateTime = Now
    Public Property LastUpdated As Date Implements IBUS.iUserSignatureLine.LastUpdated
        Get
            Return _LastUpdated
        End Get
        Set(value As Date)
            _LastUpdated = value
        End Set
    End Property
    Private _SignatureData() As Byte = Nothing
    Public Property SignatureData As Byte() Implements IBUS.iUserSignatureLine.SignatureData
        Get
            Return _SignatureData
        End Get
        Set(value As Byte())
            _SignatureData = value
        End Set
    End Property
    Private _UserID As String = String.Empty
    Public Property UserID As String Implements IBUS.iUserSignatureLine.UserID
        Get
            Return _UserID
        End Get
        Set(value As String)
            _UserID = value
        End Set
    End Property
End Class


