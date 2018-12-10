Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
 Public Class quoteExit1
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function getShipToERPID(ByVal ERPID As String) As String
        Return Business.getShiptoERPID(ERPID)
    End Function
    <WebMethod()> _
    Public Function getBillToERPID(ByVal ERPID As String) As String
        Return Business.getBILLtoERPID(ERPID)
    End Function
    <WebMethod()> _
    Public Function isPhaseOut(ByVal pn As String, ByVal org As String) As Boolean
        Return Business.isInvalidPhaseOutV2(pn, org, "", "", 0)
    End Function
    <WebMethod()> _
    Public Sub InitApprovalFlow(ByVal UID As String, ByVal ROWID As String, ByVal ERPID As String, ByVal Detail As List(Of struct_GP_Detail))
        GPControl.InitApprovalFlow(UID, ROWID, ERPID, Detail)
    End Sub
    '<WebMethod()> _
    'Public Sub InitApprovalFlow(ByVal UID As String, ByVal ROWID As String, ByVal ERPID As String, ByVal Detail As List(Of struct_GP_Detail), ByVal itptype As Relics.SAPDAL.itpType)
    '    GPControl.InitApprovalFlow(UID, ROWID, ERPID, Detail, itptype)
    'End Sub
    '<WebMethod()> _
    'Public Sub InitApprovalFlow(ByVal UID As String, ByVal ROWID As String, ByVal ERPID As String, ByVal Detail As List(Of struct_GP_Detail), ByVal itptype As Relics.SAPDAL.itpType, ByVal office As String, ByVal group As String)
    '    GPControl.InitApprovalFlow(UID, ROWID, ERPID, Detail, itptype, office, group)
    'End Sub
    '<WebMethod()> _
    'Public Function SyncCompanyIdFromSAP(ByVal companyid As String) As Boolean
    '    Return Business.SyncCompanyIdFromSAP(companyid)
    'End Function
    <WebMethod()> _
    Public Function isApproved(ByVal id As String) As Boolean
        Return GPControl.isApproved(id)
    End Function
    <WebMethod()> _
    Public Function isRejected(ByVal id As String) As Boolean
        Return GPControl.isRejected(id)
    End Function
    <WebMethod()> _
    Public Function isInApproved(ByVal id As String) As Boolean
        Return GPControl.isInApproval(id)
    End Function
    <WebMethod()> _
    Public Function getLevel(ByVal rowid As String, ByVal erpid As String, ByVal Detail As List(Of struct_GP_Detail)) As Integer
        Return GPControl.getLevel(rowid, erpid, Detail)
    End Function
    '<WebMethod()> _
    'Public Function getLevel(ByVal rowid As String, ByVal erpid As String, ByVal Detail As List(Of struct_GP_Detail), ByVal itpType As Relics.SAPDAL.itpType) As Integer
    '    Return GPControl.getLevel(rowid, erpid, Detail, itpType)
    'End Function
    '<WebMethod()> _
    'Public Function getLevel(ByVal rowid As String, ByVal erpid As String, ByVal Detail As List(Of struct_GP_Detail), ByVal itpType As Relics.SAPDAL.itpType, ByVal office As String, ByVal group As String) As Integer
    '    Return GPControl.getLevel(rowid, erpid, Detail, itpType, office, group)
    'End Function
    <WebMethod()> _
    Public Function toQuotation(ByVal erpid As String, ByVal createdBy As String, ByVal Desc As String, ByVal comment As String, ByVal isRepeatedOrder As Integer, ByVal org As String, ByVal Detail As List(Of struct_Quote_Detail)) As String
        Return Business.createQuotationByERPIDAndUser(erpid, createdBy, Desc, comment, isRepeatedOrder, org, Detail, COMM.Fixer.eDocType.EQ)
    End Function
    <WebMethod()> _
    Public Sub getQuotationListByCompany(ByVal ERPID As String, ByRef ds As DataSet)
        Dim str As String = String.Format("select * from quotationMaster where quoteToErpId='{0}' and DOCSTATUS='" & CInt(COMM.Fixer.eDocStatus.QFINISH) & "'", ERPID)
        Dim QMDT As New DataTable
        QMDT = tbOPBase.dbGetDataTable("EQ", str)
        ds.Tables.Add(QMDT)
    End Sub

    <WebMethod()> _
    Public Sub getQuotationMasterById(ByVal quoteId As String, ByRef ds As DataSet)
        Dim str As String = String.Format("select * from quotationMaster where quoteId='{0}'", quoteId)
        Dim QMDT As New DataTable
        QMDT = tbOPBase.dbGetDataTable("EQ", str)
        ds.Tables.Add(QMDT)
    End Sub
    <WebMethod()> _
    Public Sub getQuotationDetailById(ByVal quoteId As String, ByRef ds As DataSet)
        Dim myQD As New QuotationDetail("EQ", "quotationDetail")
        Dim QDDT As New DataTable
        QDDT = myQD.GetDT(String.Format("quoteId='{0}'", quoteId), "line_No")
        ds.Tables.Add(QDDT)
    End Sub
    <WebMethod()> _
    Public Sub getQuotationMasterByIdV2(ByVal quoteId As String, ByRef ds As DataSet)
        Dim str As String = String.Format("select * from quotationMaster where quoteId='{0}'", quoteId)
        Dim QMDT As New DataTable
        QMDT = tbOPBase.dbGetDataTable("EQ", str)
        ds.Tables.Add(QMDT)
        Dim DtSB As New DataTable : DtSB.TableName = "ShipToBillTo"
        Dim EqPartner As New EQPARTNER("EQ", "EQPARTNER")
        DtSB = EqPartner.GetDT(String.Format("QuoteId='{0}' ", quoteId), "")
        ds.Tables.Add(DtSB)
    End Sub
    '<WebMethod()> _
    'Public Function getQuotationMasterByIdV3(ByVal quoteId As String, ByRef QuoteMaster As IBUS.iDocHeaderLine, _
    '                                    ByRef QuoteDetail As EQDS.QuotationDetailDataTable, ByRef QuotePartner As EQDS.EQPARTNERDataTable) As Boolean
    '    Dim QD As New EQDSTableAdapters.QuotationDetailTableAdapter, QP As New EQDSTableAdapters.EQPARTNERTableAdapter
    '    QuoteMaster = Pivot.NewObjDocHeader.GetByDocID(quoteId, COMM.Fixer.eDocType.EQ) : QuoteDetail = QD.GetQuoteDetailById(quoteId) : QuotePartner = QP.GetPartnersByQuoteId(quoteId)
    '    If Not IsNothing(QuoteMaster) Then
    '        Return True
    '    End If
    '    Return False
    'End Function

    '<WebMethod()> _
    'Public Function getQuotationMasterByIdV4(ByVal quoteId As String, ByRef QuoteMaster As IBUS.iDocHeaderLine, ByRef QuoteDetail As EQDS.QuotationDetailDataTable, _
    '                                         ByRef QuotePartner As EQDS.EQPARTNERDataTable, ByRef QuoteNotes As EQDS.QuotationNoteDataTable) As Boolean
    '    Dim QD As New EQDSTableAdapters.QuotationDetailTableAdapter, QP As New EQDSTableAdapters.EQPARTNERTableAdapter
    '    Dim QNoteApt As New EQDSTableAdapters.QuotationNoteTableAdapter
    '    QuoteMaster = Pivot.NewObjDocHeader.GetByDocID(quoteId, COMM.Fixer.eDocType.EQ) : QuoteDetail = QD.GetQuoteDetailById(quoteId) : QuotePartner = QP.GetPartnersByQuoteId(quoteId)
    '    QuoteNotes = QNoteApt.GetNoteTextBYQuoteId(quoteId)
    '    If Not IsNothing(QuoteMaster) And QuoteDetail.Count > 0 Then
    '        Return True
    '    End If
    '    Return False
    'End Function

    <WebMethod()> _
    Public Function WriteQuoteToOrderLog(ByRef Quote2OrderDt As EQDS.QUOTE_TO_ORDER_LOGDataTable) As Boolean
        If Quote2OrderDt.Rows.Count > 0 Then
            Dim Quote2OrderA As New EQDSTableAdapters.QUOTE_TO_ORDER_LOGTableAdapter
            For Each dr As EQDS.QUOTE_TO_ORDER_LOGRow In Quote2OrderDt
                Dim retInt As Integer = -1
                retInt = Quote2OrderA.InsertQoute2OrderLog(dr.QUOTEID, dr.SO_NO, dr.PO_NO, dr.ORDER_DATE, dr.ORDER_BY)
                If retInt = 0 Then
                    Return False
                End If
            Next
            Return True
        End If
        Return False
    End Function
    <WebMethod()>
    Public Function isExpired(ByVal Id As String, ByVal quoteDate As Date, ByVal expiredDate As Date, ByVal isRepeatedOrder As Integer) As Boolean
        Return Business.isExpired(Id, quoteDate, expiredDate, isRepeatedOrder)
    End Function
    <WebMethod()> _
    Public Function isQuoteExpired(ByVal quoteId As String) As Boolean
        Return Business.isExpired(quoteId, COMM.Fixer.eDocType.EQ)
    End Function
    '<WebMethod()> _
    'Public Function getLevelandAppoverList(ByVal rowId As String, ByVal erpid As String, ByRef GPLevel_and_Approver As DataTable, ByVal type As Relics.SAPDAL.itpType) As String
    '    Return GPControl.getLevelandAppoverList(rowId, erpid, GPLevel_and_Approver, type)
    'End Function
    <WebMethod()> _
    Public Function getLevelandAppoverList(ByVal rowId As String, ByVal erpid As String, ByRef GPLevel_and_Approver As DataTable) As String
        Return GPControl.getLevelandAppoverList(rowId, erpid, GPLevel_and_Approver)
    End Function
    <WebMethod()> _
    Public Sub LogSSOId(ByVal tempId As String, ByVal userId As String, ByVal PW As String, ByVal IP As String)
        Pivot.NewObjPSSO.logSSOId(tempId, userId, "", IP)
    End Sub

    <WebMethod()> _
    Public Function isValidSSO(ByVal tempId As String, ByVal userId As String) As Boolean
        Return Pivot.NewObjPSSO.isValidSSOId(tempId, userId)
    End Function

    <WebMethod()> _
    Public Sub ApprovalProcess(ByVal UId As String, ByVal ACId As String)
        Business.ApprovalProcess(UId, ACId)
    End Sub

    <WebMethod()> _
    Public Function getITP(ByVal ORGId As String, ByVal PARTNO As String, ByVal CURR As String, ByVal companyId As String) As Decimal
        Return SAPDAL.SAPDAL.getItp(ORGId, PARTNO, CURR, companyId, SAPDAL.SAPDAL.itpType.EU)
    End Function

    <WebMethod()> _
    Public Sub setITP(ByVal ORGId As String, ByVal PARTNO As String, ByVal CURR As String, ByVal ITP As Decimal)
        SAPDAL.SAPDAL.writeItpBack(ORGId, PARTNO, CURR, ITP)
    End Sub
    <WebMethod()> _
    Public Function isCanRepeatOrder(ByVal rowID As String) As Boolean
        Return SiebelTools.IsCanRepeatOrder(rowID)
    End Function

    '<WebMethod()> _
    'Public Sub CreateAccountContactQuotationFromSPR()
    '    Dim ST As New OP_SiebelTools
    '    ST.CreateAccountContactQuotationFromSPR()
    'End Sub
    <WebMethod()> _
    Public Function getQuotePageStrCustomer(ByVal UID As String) As String
        Dim str As String = ""
        Dim headerBlock As String = "", detailBlock As String = ""
        Dim url As String = String.Format("~/Quote/{1}?UID={0}&ROLE=1", UID, Business.getPiPage(UID, COMM.Fixer.eDocReg.AEU))
        Dim MyDOC As New System.Xml.XmlDocument
        Util.HtmlToXML(url, MyDOC)
        Util.getXmlBlockByID("div", "divMaster", MyDOC, headerBlock)
        Util.getXmlBlockByID("div", "divDetail", MyDOC, detailBlock)
        str = headerBlock & detailBlock
        Return str
    End Function

    <WebMethod()> _
    Public Function getOptyIdByQuoteId(ByVal quoteId As String) As String
        Return Business.getOptyIdByQuoteId(quoteId)
    End Function

    <WebMethod()> _
    Public Function IsPTD(ByVal PartNo As String) As Boolean
        Return Business.IsPTD(PartNo)
    End Function

    <WebMethod()> _
        <Web.Script.Services.ScriptMethod()> _
    Public Function IsMatchSalesDistrict(ByVal SalesCode As String, ByVal DistrictOnForm As String) As Boolean
        Return True
    End Function

    'ICC 2015/10/16 Add Check Distric value is ok
    'ICC 2015/11/10 Seperate IA & EC group rule
    <WebMethod()> _
    <Web.Script.Services.ScriptMethod()> _
    Public Function CheckANASalesDistrict(ByVal SalesID As String, ByVal District As String) As Boolean

        Dim region As Advantech.Myadvantech.DataAccess.AOnlineRegion = Advantech.Myadvantech.Business.UserRoleBusinessLogic.GetTeamOfAonlineSalesBySalesCode(SalesID.Trim())
        'Select Case region
        '    Case Advantech.Myadvantech.DataAccess.AOnlineRegion.AUS_AOnline_IAG
        '        Return Advantech.Myadvantech.Business.QuoteBusinessLogic.CheckUSAOnlineIagDistrict(SalesID.Trim(), District.Trim())
        '    Case Advantech.Myadvantech.DataAccess.AOnlineRegion.AUS_AOnline
        '        Return Advantech.Myadvantech.Business.QuoteBusinessLogic.CheckANASalesDistrict(SalesID.Trim(), District.Trim())
        '    Case Else
        '        Return False
        'End Select

        'Frank 20180207: TC has help sync all ANA sales district data to CurationPool database, so I call new function to check district
        Return Advantech.Myadvantech.Business.QuoteBusinessLogic.CheckSalesDistrict(SalesID.Trim(), District.Trim())


    End Function

    'ICC 2015/10/16 Add Check PO no. exists
    <WebMethod()> _
    <Web.Script.Services.ScriptMethod()> _
    Public Function IsPONumberExisting(ByVal ORGID As String, ByVal ERPID As String, ByVal PO As String) As Boolean
        Return Advantech.Myadvantech.Business.OrderBusinessLogic.IsPONumberExisting(ORGID.Trim(), ERPID.Trim(), PO.Trim())
    End Function
End Class