Public Interface iCartLine
    ReadOnly Property isValidLine As Boolean
    Property key As COMM.Field
    Property parentLineNo As COMM.Field
    Property lineNo As COMM.Field
    Property partNo As COMM.Field
    Property partDesc As COMM.Field
    Property listPrice As COMM.Field
    Property unitPrice As COMM.Field
    Property newunitPrice As COMM.Field
    Property cost As COMM.Field
    Property newCost As COMM.Field
    Property Qty As COMM.Field
    Property itemType As COMM.Field
    Property ewFlag As COMM.Field
    Property inventory As COMM.Field
    Property reqDate As COMM.Field
    Property dueDate As COMM.Field
    Property canBeConfirmed As COMM.Field
    Property category As COMM.Field
    Property divPlant As COMM.Field
    Property RHOS As COMM.Field
    Property CustMaterial As COMM.Field
    Property satisfyflag As COMM.Field
    Property DMFFlag As COMM.Field
    Property VirtualPartNo As COMM.Field
    Property RecyclingFee As COMM.Field
    Property ShipPoint As COMM.Field
    Property StorageLoc As COMM.Field
    Property DELIVERYGROUP As COMM.Field
    Property RECFIGID As COMM.Field
End Interface
Public Interface iCart(Of T As iCartLine) : Inherits iBase
    ReadOnly Property Key As String
    Function Add(ByVal item As T, ByVal oType As COMM.Fixer.eDocType) As Integer
    Sub RemoveAt(ByVal lineNo As SortedSet(Of Integer), ByVal oType As COMM.Fixer.eDocType)
    Function UpdateByStr(ByVal setStr As String, ByVal whereStr As String, ByVal oType As COMM.Fixer.eDocType) As Integer
    Function UpdateByLine(ByVal setStr As String, ByVal LineNo As Integer, ByVal oType As COMM.Fixer.eDocType) As Integer
    Function isPartialAble() As Boolean
    Function CopyPaste(ByVal NewCart As iCart(Of iCartLine), ByVal oType As COMM.Fixer.eDocType) As iCartList
    Function GetListByParent(ByVal ParentLineNo As Integer, ByVal oType As COMM.Fixer.eDocType) As iCartList
    Function GetListAll(ByVal oType As COMM.Fixer.eDocType) As iCartList
    ReadOnly Property Item(ByVal lineNo As Integer) As T
    ReadOnly Property Item1() As T
    Sub Clear(ByVal oType As COMM.Fixer.eDocType)
    Sub updateEW(ByVal lineNo As SortedSet(Of Integer), ByVal DocReg As COMM.Fixer.eDocReg, ByVal oType As COMM.Fixer.eDocType, Optional Currency As String = "")
End Interface
Public Interface iCartList : Inherits IEnumerable(Of iCartLine) : Inherits iBase
    Sub Add(ByVal item As IBUS.iCartLine)
    Sub Clear()
    Function Contains(ByVal LineNo As Integer) As Boolean
    Function IndexOf(ByVal LineNo As Integer) As Integer
    Sub RemoveAt(ByVal LineNo As Integer)
    Property Item(ByVal LineNo As Integer) As IBUS.iCartLine
    ReadOnly Property Count() As Integer
    Function lineNoOf(ByVal index As Integer) As Integer

    Function GetTaxableAmount(ByVal ShipTo As String) As Decimal
    Function getTotalAmount() As Decimal
    Function getTotalRecyclingFee() As Decimal
    Function getListPriceTotalAmount() As Decimal
    Function getTotalListAmount() As Decimal
    Function getWarrantableTotalAmount(ByVal DocReg As COMM.Fixer.eDocReg) As Decimal
    Function Merge2NewCartList(ByVal MyEnumerable As IEnumerable(Of iCartLine)) As iCartList
    Function ItemValidate(ByVal Type As COMM.Fixer.eCartItemValidateType, ByVal Role As iRole) As iCartList
End Interface
Public Interface iCartF : Inherits iBase
    Function getCartByAppArea(ByVal AppArea As COMM.Fixer.eCartAppArea, ByVal Key As String, ByVal Org As String) As iCart(Of iCartLine)
End Interface

Public Interface iCreditLine
    Property DocID As String
    Property HOLDER As String
    Property EXPIRED As Date
    Property TYPE As String
    Property NUMBER As String
    Property VERIFICATION_VALUE As String
End Interface
Public Interface iCredit : Inherits iBase
    Sub Add(ByVal CreditLine As IBUS.iCreditLine)
    Sub Delete(ByVal DocId As String)
    Function GetListAll(ByVal DocId As String) As List(Of IBUS.iCreditLine)
End Interface

Public Interface iDocTextLine
    Property DocId As String
    Property Txt As String
    Property Type As String
End Interface
Public Interface iDocText
    Function GetListAll(ByVal DocId As String) As List(Of iDocTextLine)
    Sub Add(ByVal TextLine As IBUS.iDocTextLine)
    Sub Delete(ByVal DocId As String)
End Interface

Public Interface iCondLine
    Property DocId As String
    Property Type As String
    Property Value As Decimal
End Interface
Public Interface iCond : Inherits iBase
    Sub Add(ByVal CondLine As IBUS.iCondLine)
    Sub Delete(ByVal DocId As String)
    Function GetListAll(ByVal DocId As String) As List(Of iCondLine)

    'Sub Add(CondLine As iCondLine)

End Interface

Public Interface iPartnerLine
    Property ORDER_ID As String
    Property ROWID As String
    Property ERPID As String
    Property NAME As String
    Property ADDRESS As String
    Property TYPE As String
    Property ATTENTION As String
    Property TEL As String
    Property MOBILE As String
    Property ZIPCODE As String
    Property COUNTRY As String
    Property CITY As String
    Property STREET As String
    Property STATE As String
    Property DISTRICT As String
    Property STREET2 As String
End Interface
Public Interface iPartner : Inherits iBase
    Function GetListAll(ByVal DocId As String, ByVal oType As COMM.Fixer.eDocType) As List(Of iPartnerLine)
    Sub Add(ByVal PartnerLine As IBUS.iPartnerLine, ByVal oType As COMM.Fixer.eDocType)
    Sub Delete(ByVal DocId As String, ByVal oType As COMM.Fixer.eDocType)
End Interface
Public Interface iNoteLine
    Property QUOTEID As String
    Property notetype As String
    Property notetext As String
End Interface
Public Interface iNote : Inherits iBase
    Function GetListAll(ByVal DocId As String, ByVal oType As COMM.Fixer.eDocType) As List(Of iNoteLine)
End Interface

Public Interface iCustomerLine
    Property COMPANY_ID As String
    Property ORG_ID As String
    Property PARENTCOMPANYID As String
    Property COMPANY_NAME As String
    Property ADDRESS As String
    Property FAX_NO As String
    Property TEL_NO As String
    Property COMPANY_TYPE As String
    Property PRICE_CLASS As String
    Property CURRENCY As String
    Property COUNTRY As String
    Property REGION_CODE As String
    Property ZIP_CODE As String
    Property CITY As String
    Property ATTENTION As String
    Property CREDIT_TERM As String
    Property SHIP_VIA As String
    Property URL As String
    Property CREATEDDATE As String
    Property CREATED_BY As String
    Property COMPANY_PRICE_TYPE As String
    Property SHIPCONDITION As String
    Property ATTRIBUTE4 As String
    Property SALESOFFICE As String
    Property SALESGROUP As String
    Property AMT_INSURED As Decimal
    Property CREDIT_LIMIT As Decimal
    Property CONTACT_EMAIL As String
    Property DELETION_FLAG As String
    Property COUNTRY_NAME As String
    Property SALESOFFICENAME As String
    Property SAP_SALESNAME As String
    Property SAP_SALESCODE As String
    Property SAP_ISNAME As String
    Property SAP_OPNAME As String
    Property SECTOR As String
    Property PRIMARY_BAA As String
    Property ACCOUNT_ROW_ID As String
    Property ACCOUNT_NAME As String
    Property ACCOUNT_STATUS As String
    Property RBU As String
    Property PRIMARY_SALES_EMAIL As String
    Property PRIMARY_OWNER_DIVISION As String
    Property BUSINESS_GROUP As String
    Property ACCOUNT_TYPE As String
    Property FACT2005 As Decimal
    Property FACT2006 As Decimal
    Property FACT2007 As Decimal
    Property FACT2008 As Decimal
    Property FACT2009 As Decimal
    Property FACT2010 As Decimal
    Property LAST_BUY_DATE As DateTime
    Property ORDERS_IN_PAST_YEAR As Integer
    Property AMOUNT_IN_PAST_YEAR As Decimal
    Property ORDERS_IN_PAST_HALFYEAR As Integer
    Property CUST_IND As String
    Property VM As String
    Property PRICE_GRP As String
    Property PRICE_LIST As String
    Property INCO1 As String
    Property INCO2 As String
    Property PAYMENT_TERM_CODE As String
End Interface
Public Interface iCustomer : Inherits iBase
    Function isCustomerCompleteDeliv(ByVal CompanyId As String, ByVal org As String) As Boolean
    Function getSalesNotebyCustomer(ByVal companyID As String) As String
    Function is_Valid_Company_Id_All(ByVal company_id As String) As Boolean
    Function getByErpIdOrg(ByVal CompanyiD As String, ByVal org As String) As IBUS.iCustomerLine
End Interface

Public Interface iDocHeaderLine
    Property Key As String
    Property CustomId As String
    Property AccErpId As String
    Property DocDate As Date
    Property CreatedDate As DateTime
    Property CreatedBy As String
    Property AccRowId As String
    Property AccName As String
    Property AccOfficeName As String
    Property AccOfficeCode As String
    Property AccGroupName As String
    Property AccGroupCode As String
    Property salesEmail As String
    Property salesRowId As String
    Property directPhone As String
    Property attentionRowId As String
    Property attentionEmail As String
    Property bankInfo As String
    Property deliveryDate As Date
    Property expiredDate As Date
    Property shipTerm As String
    Property paymentTerm As String
    Property freight As Decimal
    Property insurance As Decimal
    Property specialCharge As Decimal
    Property tax As Decimal
    Property quoteNote As String
    Property relatedInfo As String
    Property preparedBy As String
    Property currency As String
    Property qStatus As COMM.Fixer.eDocStatus
    Property DOCSTATUS As COMM.Fixer.eDocStatus
    Property isShowListPrice As Integer
    Property isShowDiscount As Integer
    Property isShowDueDate As Integer
    Property isLumpSumOnly As Integer
    Property isRepeatedOrder As Integer
    Property delDateFlag As Integer
    Property org As String
    Property siebelRBU As String
    Property DIST_CHAN As String
    Property DIVISION As String
    Property PO_NO As String
    Property CARE_ON As String
    Property isExempt As Integer
    Property Inco1 As String
    Property Inco2 As String
    Property SalesDistrict As String
    Property PRINTOUT_FORMAT As COMM.Fixer.USPrintOutFormat
    Property OriginalQuoteID As String
    Property DocReg As COMM.Fixer.eDocReg
    Property DocType As COMM.Fixer.eDocType
    Property ReqDate As Date
    Property PartialF As Integer
    Property LastUpdatedDate As DateTime
    Property LastUpdatedBy As String
    Property IS_EARLYSHIP As Integer
    Property DocRealType As String
    Property PODate As Date
    Property KEYPERSON As String
    Property EMPLOYEEID As String
    Property ISVIRPARTONLY As COMM.Fixer.eIsVirNoOnly


    Property TAXDEPCITY As String
    Property TAXDSTCITY As String
    Property TRIANGULARINDICATOR As String
    Property TAXCLASS As String
    Property SHIPCUSTPONO As String

    Property quoteNo As String
    Property Revision_Number As Integer
    Property Active As Boolean

End Interface
Public Interface iDocHeader : Inherits iBase

    Function GetByDocID(ByVal DocID As String, ByVal oType As COMM.Fixer.eDocType) As iDocHeaderLine
    Function GetConstantUbiquitousLineByDocID(ByVal DocID As String, ByVal oType As COMM.Fixer.eDocType) As iConstantUbiquitousLine
    Function Add(ByVal Header As iDocHeaderLine, ByVal P As iRole, ByVal oType As COMM.Fixer.eDocType) As iDocHeaderLine
    Function AddByAssignedUID(ByVal UID As String, ByVal Header As iDocHeaderLine, ByVal P As iRole, ByVal oType As COMM.Fixer.eDocType) As iDocHeaderLine
    Function CopyPasteHeaderLine(ByVal orgQuoteId As String, ByVal NewQuoteId As String, ByVal P As iRole, ByVal oType As COMM.Fixer.eDocType) As iDocHeaderLine

    Function Update(ByVal UID As String, ByVal SQLSTR As String, ByVal oType As COMM.Fixer.eDocType) As Integer
    Sub Remove(ByVal Key As String, ByVal oType As COMM.Fixer.eDocType)
    Sub UpdateAccErpId(ByVal rowId As String, ByVal erpId As String, ByVal oType As COMM.Fixer.eDocType)
    'Function CreateQuotationHeaderFromOrder(ByVal DocHeaderLine As iDocHeaderLine, ByVal NewQuoteId As String, ByVal P As iRole) As iDocHeaderLine
    'Function CreateOrderHeaderFromQuotation(ByVal DocHeaderLine As iDocHeaderLine, ByVal NewOrderId As String, ByVal P As iRole) As iDocHeaderLine
    Sub ChangeDocStatus(ByVal DocId As String, ByVal NewStatus As COMM.Fixer.eDocStatus, ByVal oType As COMM.Fixer.eDocType)

    'Frank 2013/07/02
    Function ReviseHeaderLine(ByVal QuoteId As String, ByVal CreateBy As String, ByVal P As iRole, ByVal oType As COMM.Fixer.eDocType) As iDocHeaderLine
    Function GetByQuoteNoandRevisionNumber(ByVal QuoteNo As String, ByVal Revision_Number As Integer, ByVal oType As COMM.Fixer.eDocType) As iDocHeaderLine
    Function GetActiveRevisionQuoteIDByQuoteNo(ByVal QuoteNo As String, ByVal oType As COMM.Fixer.eDocType, Optional ByRef ActiveVersion As String = "") As String
    Sub SetRevisionQuoteToActive(ByVal QuoteId As String, ByVal oType As COMM.Fixer.eDocType)
    Function IsV2_0Quotation(ByVal QuoteId As String) As Boolean

    'Nada Added 20130828 
    Function GetActiveRevisionNo(ByVal QuoteNo As String) As String
    Function GetRevisionsAr(ByVal QuoteNo As String) As ArrayList
End Interface
Public Interface iDoc : Inherits iUID : Inherits iBase
    'Function OrderProcsSave(ByVal OrderId As String, ByVal Msg As String, Optional ByVal Type As String = "E")
    Function getSalesEmployeeList(ByVal Org As String) As DataTable
    Function GetKeyInPerson(ByVal userid As String) As DataTable
    Function GetLocalTime(ByVal org As String) As DateTime
    Function shipCode2Txt(ByVal shipCode As String) As String
    Sub VerifyDisChannelAndDivision(ByVal companyid As String, ByRef disChannel As String, ByRef division As String, ByVal Org As String)
    Function GetDefaultDistChannDivisionSalesGrpOfficeByCompanyId( _
        ByVal CompanyId As String, ByRef dist_chann As String, ByRef division As String, ByRef SalesGroup As String, ByRef SalesOffice As String) As Boolean
    Function CheckSAPQuote(ByVal QuoteId As String) As Boolean
    Sub ProcessAfterOrderSuccess(ByVal M As IBUS.iDocHeaderLine, ByVal C As IBUS.iCartList, ByVal oType As COMM.Fixer.eDocType)
    Sub ProcessAfterOrderFailed(ByVal M As IBUS.iDocHeaderLine, ByVal C As IBUS.iCartList, ByVal oType As COMM.Fixer.eDocType)
    Sub SendFailedOrderMail(ByVal M As IBUS.iDocHeaderLine)
    'Function getPriceBatch(ByVal Header As IBUS.iDocHeaderLine, _
    '                          ByRef Partner As System.Collections.Generic.List(Of IBUS.iPartnerLine), _
    '                          ByRef Items As IBUS.iCartList, _
    '                          ByRef Ret As List(Of IBUS.iOrderProcSLine)) As Boolean
    'Function getInventoryBatch(ByVal Header As IBUS.iDocHeaderLine, _
    '                          ByRef Partner As System.Collections.Generic.List(Of IBUS.iPartnerLine), _
    '                          ByRef Items As IBUS.iCartList, _
    '                          ByRef Ret As List(Of IBUS.iOrderProcSLine)) As Boolean
    Function getBTOWorkingDate(ByVal Org As String) As COMM.Fixer.eBTOAssemblyDays
    Function getBTOParentDueDate(ByVal reqDate As String, ByVal org As String) As String
    Function getCompNextWorkDate(ByVal reqDate As String, ByVal org As String, Optional ByVal days As Integer = 0) As String
    Function GetNextWeeklyShippingDate(ByVal reqDate As Date, ByRef NextWeeklyShipDate As Date, ByVal CompanyID As String) As Boolean
    Function getBTOChildDueDate(ByVal reqDate As String, ByVal org As String) As String
    Function getNextCustDelDate(ByVal ddate As String, ByVal CompanyId As String) As String
    Function UpdateSAPSOShipToAttentionAddress( _
        ByVal SONO As String, ByVal ShipToId As String, ByVal Name As String, ByVal Attention As String, ByVal Street As String, ByVal Street2 As String, ByVal City As String, ByVal State As String, ByVal Zipcode As String, _
        ByVal Country As String, ByRef ReturnTable As DataTable, Optional ByVal IsSAPProductionServer As Boolean = True) As Boolean
    Function UpdateSOZeroPriceItems(ByVal SO_NO As String, ByVal C As IBUS.iCartList, ByRef ReturnTable As DataTable) As Boolean
    Function UpdateSOSpecId(ByVal SO_NO As String, ByVal SpecialIndicator As COMM.Fixer.eEarlyShipment, ByRef ReturnTable As DataTable) As Boolean
    Function VerifyDistChannelDivisionGroupOffice(ByVal Org As String, ByVal SoldToId As String, ByVal ShipToId As String, ByVal strDistChann As String, _
                                       ByVal strDivision As String, ByVal OrderDocType As Integer, ByVal SalesGroup As String, ByVal SalesOffice As String, ByRef ReturnTable As DataTable) As Boolean
    Function UpdateOpportunityStatusRevenue(ByVal OptRowId As String, ByVal status As String, ByVal Revenue As Double, ByVal ConnectToACL As Boolean) As Boolean
End Interface
Public Interface iDocRevisionController
    Function getActiveVersionQuoteID(ByVal QuoteNo As String) As String
    Function changeActiveVersion(ByVal QuoteNo As String, ByVal ActiveVersion As Integer) As Integer
    Sub versionRemove(ByVal QuoteNo As String, ByVal VersionId As Integer)
    Function versionCreateNew(ByVal QuoteNo As String) As String

End Interface
Public Interface iUID : Inherits iBase
    Function NewQuotationNo(ByVal R As COMM.Fixer.eDocReg, ByVal DocType As COMM.Fixer.eDocType) As String
    Function NewUID() As String
    Function IsValidQuotationID(ByVal quoteid As String) As Boolean
    Function CanUserAccessThisQuotation(ByVal quoteid As String, ByVal UserID As String) As Boolean
End Interface

Public Interface iDOCHeaderExtensionLine
    Property Key As String
    Property EmailGreeting As String
    Property SpecialTandC As String
    Property SignatureRowID As String
    Property Warranty As String
    Property LastUpdatedBy As String
    Property LastUpdated As DateTime
End Interface

Public Interface iDOCHeaderExtension : Inherits iBase
    Function SetQuoteExtension(ByVal QuoteExtensionLine As IBUS.iDOCHeaderExtensionLine)
    Function GetQuoteExtension(ByVal QuoteId As String) As IBUS.iDOCHeaderExtensionLine
End Interface

Public Interface iUserSignatureLine
    Property Key As String
    Property UserID As String
    Property SignatureData As Byte()
    Property Active As Boolean
    Property FileName As String
    Property LastUpdated As DateTime
End Interface


Public Interface iUserSignature : Inherits iBase
    'Function GetSignatureDataBySID(ByVal SID As String) As iSignatureLine
    Function Add(ByVal Signatureline As iUserSignatureLine) As Integer
    Function GetDefaultSignature(ByVal UserID As String) As String
End Interface


Public Interface iDueDate : Inherits iBase

End Interface

Public Interface iConstantUbiquitousLine
    Property Key As String
    Property AccErpId As String
    Property DocType As COMM.Fixer.eDocType
    Property DocReg As COMM.Fixer.eDocReg
    Property Org As String
    Property CreatedBy As String
    Property AccRowId As String
    Property DocRealType As String
    Property AccOfficeName As String
    Property AccGroupName As String
    Property AccOfficeCode As String
    Property AccGroupCode As String
    Property SiebelRBU As String
    Property Currency As String
    Property isExempt As Integer
End Interface

Public Interface iOrderProcSLine
    Property ORDER_NO As String
    Property LINE_SEQ As Integer
    Property NUMBER As Integer
    Property MESSAGE As String
    Property CREATED_DATE As Date
    Property STATUS As Integer
    Property TYPE As String
End Interface
Public Interface iOrderProcS
    Function GetListAll(ByVal DocId As String) As List(Of iOrderProcSLine)
    Sub Add(ByVal OrderProcsLine As IBUS.iOrderProcSLine)
    Sub Delete(ByVal DocId As String)
End Interface

Public Interface iOptyQuoteLine
    Property optyId As String
    Property optyName As String
    Property quoteId As String
    Property optyStage As String
    Property Opty_Owner_Email As String
End Interface
Public Interface iOptyQuote : Inherits iBase
    Function GetListByOptyID(ByVal OptyID As String) As List(Of iOptyQuoteLine)
    Function GetListByQuoteID(ByVal QuoteId As String) As List(Of iOptyQuoteLine)
    Sub Add(ByVal OptyQuote As IBUS.iOptyQuoteLine)
    Sub DeleteByQuoteID(ByVal QuoteId As String)
    Sub DeleteByOptyID(ByVal OptyId As String)
End Interface
