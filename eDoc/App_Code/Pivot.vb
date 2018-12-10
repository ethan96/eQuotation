Imports Microsoft.VisualBasic

Public Class Pivot
    Public Shared Function login(ByVal userName As String, ByVal password As String, ByVal lang As String, ByRef Msg As String, Optional ByVal isCook As Boolean = False) As Boolean
        Dim Prof As IBUS.iRole = New PROF.Prof
        Dim F As Boolean = Prof.login(userName, password, lang, isCook)
        Msg = COMM.errMsg.getErrMsg(Prof.errCode)
        Return F
    End Function
    Public Shared Function CurrentProfile(Optional ByVal isMand As Boolean = True) As IBUS.iRole

        If Not IsNothing(HttpContext.Current) Then

            If Not IsNothing(HttpContext.Current.Session) Then

                'Frank 2013/12/26  If Session("P") does not exist, then reload user profile to an object and save it to Session("P")
                If IsNothing(HttpContext.Current.Session("P")) Then
                    If Not IsNothing(HttpContext.Current) AndAlso (Not IsNothing(HttpContext.Current.User)) AndAlso HttpContext.Current.User.Identity.IsAuthenticated Then
                        Dim Prof As IBUS.iRole = New PROF.Prof
                        Dim _tempid As String = String.Empty
                        Dim _userid As String = HttpContext.Current.User.Identity.Name
                        Dim _dt As DataTable = tbOPBase.dbGetDataTable("EQ", "Select Top 1 tempId from loginLog where userid='" & _userid & "' order by logTime desc")
                        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                            _tempid = _dt.Rows(0).Item("tempId").ToString
                        End If
                        Prof.setSession(HttpContext.Current.User.Identity.Name, "en-US", _tempid)
                    End If
                End If

                If Not IsNothing(HttpContext.Current.Session("P")) Then
                    Dim temp As IBUS.iRole = CType(HttpContext.Current.Session("P"), IBUS.iRole)
                    Return temp
                End If

            End If
        End If

        If isMand Then
            Throw New Exception("System cannot initialize profile information. Please click <a href='/login.aspx'> here </a> to login.")
        End If
        Return Nothing
    End Function

    Public Shared Function FactProd() As IBUS.iProdF
        Dim pf As IBUS.iProdF = New DOCH.prodF
        Return pf
    End Function
    Public Shared Function FactCart() As IBUS.iCartF
        Dim cF As IBUS.iCartF = New DOCH.CartF
        Return cF
    End Function
    Public Shared Function NewObjCartList() As IBUS.iCartList
        Dim DU As IBUS.iCartList = New DOCH.oCartList
        Return DU
    End Function
    Public Shared Function NewObjDoc() As IBUS.iDoc
        Dim DU As IBUS.iDoc = New DOCH.Doc
        Return DU
    End Function
    Public Shared Function NewObjDocHeader() As IBUS.iDocHeader
        Dim Master As IBUS.iDocHeader = New DOCH.DocHeader
        Return Master
    End Function
    Public Shared Function NewObjDocHeaderExtension() As IBUS.iDOCHeaderExtension
        Dim extension As IBUS.iDOCHeaderExtension = New DOCH.DOCHeaderExtension
        Return extension
    End Function
    Public Shared Function NewObjDueDateUtil() As IBUS.iDueDate
        Dim DueUtil As IBUS.iDueDate = New PATC.DueDate
        Return DueUtil
    End Function
    Public Shared Function NewObjCustomer() As IBUS.iCustomer
        Dim DueUtil As IBUS.iCustomer = New DOCH.Customer
        Return DueUtil
    End Function
    Public Shared Function NewObjCond() As IBUS.iCond
        Dim F As IBUS.iCond = New DOCH.FreightF
        Return F
    End Function
    Public Shared Function NewObjPartner() As IBUS.iPartner
        Dim P As IBUS.iPartner = New DOCH.Partner
        Return P
    End Function
    Public Shared Function NewObjDocText() As IBUS.iDocText
        Dim T As IBUS.iDocText = New DOCH.DocTxt
        Return T
    End Function
    Public Shared Function NewObjPSSO() As IBUS.iSSO
        Dim CSSO As IBUS.iSSO = New PROF.CSSO
        Return CSSO
    End Function
    Public Shared Function NewObjProfile() As IBUS.iRole
        Dim Prof As IBUS.iRole = New PROF.Prof
        Return Prof
    End Function
    Public Shared Function NewObjCred() As IBUS.iCredit
        Dim Cred As IBUS.iCredit = New DOCH.Cred
        Return Cred
    End Function
    Public Shared Function NewObjPatch() As IBUS.iPatch
        Dim P As IBUS.iPatch = New PATC.PATC
        Return P
    End Function
    Public Shared Function NewObjOrderProcS() As IBUS.iOrderProcS
        Dim P As IBUS.iOrderProcS = New DOCH.OrderProcs
        Return P
    End Function
    Public Shared Function NewObjEWUtil() As IBUS.iEWUtil
        Dim E As IBUS.iEWUtil = New DOCH.EWUtil
        Return E
    End Function
    Public Shared Function NewObjUserSignature() As IBUS.iUserSignature
        Dim US As IBUS.iUserSignature = New DOCH.UserSignature
        Return US
    End Function

    Public Shared Function NewLineCU() As IBUS.iConstantUbiquitousLine
        Dim o As IBUS.iCreditLine = New DOCH.CULine
        Return o
    End Function
    Public Shared Function NewLineCred() As IBUS.iCreditLine
        Dim o As IBUS.iCreditLine = New DOCH.CredLine
        Return o
    End Function
    Public Shared Function NewLineDocText() As IBUS.iDocTextLine
        Dim Cred As IBUS.iDocTextLine = New DOCH.DocTxtLine
        Return Cred
    End Function
    Public Shared Function NewLineCond() As IBUS.iCondLine
        Dim o As IBUS.iCondLine = New DOCH.FreightLine
        Return o
    End Function
    Public Shared Function NewLinePartner() As IBUS.iPartnerLine
        Dim o As IBUS.iPartnerLine = New DOCH.PartnerLine
        Return o
    End Function
    Public Shared Function NewLineHeader() As IBUS.iDocHeaderLine
        Dim o As IBUS.iDocHeaderLine = New DOCH.HeaderLine
        Return o
    End Function
    Public Shared Function NewLineHeaderExtension() As IBUS.iDOCHeaderExtensionLine
        Dim o As IBUS.iDOCHeaderExtensionLine = New DOCH.DOCHeaderExtensionLine
        Return o
    End Function
    Public Shared Function NewLineCustomer() As IBUS.iCustomerLine
        Dim o As IBUS.iCustomerLine = New DOCH.CustomerLine
        Return o
    End Function
    Public Shared Function NewLineCart(ByVal key As String, ByVal lineNo As Integer, ByVal parentLineNo As Integer,
           ByVal p As IBUS.iProd, ByVal newUnitPrice As Decimal, ByVal listPrice As Decimal, ByVal newCost As Decimal, _
           ByVal Qty As Integer, ByVal EWFlag As Integer, ByVal reqDate As DateTime, ByVal ItemType As COMM.CartFixer.eItemType, ByVal category As String) As IBUS.iCartLine
        Return New DOCH.CartLineQ(key, lineNo, parentLineNo, p, newUnitPrice, listPrice, newCost, Qty, EWFlag, reqDate, ItemType, category)
    End Function
    Public Shared Function NewLineCart(ByVal key As String, ByVal lineNo As Integer, ByVal parentLineNo As Integer,
           ByVal partNo As String, ByVal partDesc As String, ByVal listPrice As Decimal, ByVal newUnitPrice As Decimal, ByVal newCost As Decimal, _
           ByVal Qty As Integer, ByVal EWFlag As Integer, ByVal divPlant As String, ByVal reqDate As DateTime, ByVal ItemType As COMM.CartFixer.eItemType, ByVal category As String, ByVal VirtualPartNo As String, ByVal RecyclingFee As Decimal) As IBUS.iCartLine
        Return New DOCH.CartLineQ(key, lineNo, parentLineNo, partNo, partDesc, listPrice, newUnitPrice, newCost, Qty, EWFlag, divPlant, reqDate, ItemType, category, VirtualPartNo, RecyclingFee)
    End Function
    Public Shared Function NewLineOrderProcS() As IBUS.iOrderProcSLine
        Dim P As IBUS.iOrderProcSLine = New DOCH.OrderProcsLine()
        Return P
    End Function
    Public Shared Function NewLineEW() As IBUS.iEWTypeLine
        Dim P As IBUS.iEWTypeLine = New DOCH.EWType
        Return P
    End Function
    Public Shared Function NewLineOptyQuote() As IBUS.iOptyQuoteLine
        Dim P As IBUS.iOptyQuoteLine = New PATC.OptyQuoteLine
        Return P
    End Function
    Public Shared Function NewObjOptyQuote() As IBUS.iOptyQuote
        Dim P As IBUS.iOptyQuote = New PATC.OptyQuoteC
        Return P
    End Function
    Public Shared Function NewLineUserSignature() As IBUS.iUserSignatureLine
        Dim o As IBUS.iUserSignatureLine = New DOCH.UserSignatureLine
        Return o
    End Function

    Public Shared Function NewObjNote() As IBUS.iNote
        Dim P As IBUS.iNote = New DOCH.Note
        Return P
    End Function
End Class
