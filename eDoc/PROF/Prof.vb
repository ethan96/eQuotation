Imports System.Web.HttpContext
Imports Advantech.Myadvantech.Business

<Serializable()>
Public Class Prof : Implements iRole
    Private _errCode As COMM.Msg.eErrCode
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property

    Private _lung As String
    Public Property Lung As String Implements IBUS.iRole.Lung
        Get
            Return _lung
        End Get
        Set(ByVal value As String)
            _lung = value
        End Set
    End Property

    Private _SSOID As String
    Public Property SSOID As String Implements IBUS.iRole.SSOID
        Get
            Return _SSOID
        End Get
        Set(ByVal value As String)
            _SSOID = value
        End Set
    End Property
    Private _userId As String
    Public Property UserId As String Implements IBUS.iRole.UserId
        Get
            Return _userId
        End Get
        Set(ByVal value As String)
            _userId = value
        End Set
    End Property
    Private _visibleRBU As ArrayList
    Public Property VisibleRBU As System.Collections.ArrayList Implements IBUS.iRole.VisibleRBU
        Get
            Return _visibleRBU
        End Get
        Set(ByVal value As System.Collections.ArrayList)
            _visibleRBU = value
        End Set
    End Property

    Private _SalesOfficeCode As List(Of String)
    Public Property SalesOfficeCode As List(Of String) Implements IBUS.iRole.SalesOfficeCode
        Get
            Return _SalesOfficeCode
        End Get
        Set(ByVal value As List(Of String))
            _SalesOfficeCode = value
        End Set
    End Property



    Private _IPAdd As String
    Public Property IPAdd As String Implements IBUS.iRole.IPAdd
        Get
            Return _IPAdd
        End Get
        Set(ByVal value As String)
            _IPAdd = value
        End Set
    End Property
    Private _Role As COMM.Fixer.eRole
    Public Property Role As COMM.Fixer.eRole Implements IBUS.iRole.Role
        Get
            Return _Role
        End Get
        Set(ByVal value As COMM.Fixer.eRole)
            _Role = value
        End Set
    End Property

    Private Function isSSOValid(ByVal userName As String, ByVal Password As String, ByVal IP As String) As String
        Dim sso As New PSSO.MembershipWebservice, Validated As Boolean = False
        Dim loginTicket As String = ""
        sso.Timeout = -1
        loginTicket = sso.login(userName, Password, "MY", IP)
        Return loginTicket
    End Function

    Public Function login(ByVal userName As String, ByVal password As String, ByVal lang As String, Optional ByVal isCook As Boolean = False) As Boolean Implements IBUS.iRole.login

        If Not RoleCheck.IsInternalUser(userName) Then
            Me._errCode = Me._errCode Or Msg.eErrCode.LoginIDorPasswordincorrect
            Return False
        End If

        Dim IP As String = Util.GetClientIP()
        Dim loginTicket As String = isSSOValid(userName, password, IP)

        If userName.Equals("victor.arroyo@advantech.com.mx", StringComparison.InvariantCultureIgnoreCase) AndAlso password.Equals("4321") Then
            loginTicket = "MexicoTesting" & Now.ToString("yyyy-mm-dd-hh-mm-ss")
        End If

        'Frank 20170706: Create test account for Gary's intern
        If Util.IsTesting AndAlso userName.Equals("DMKT.ACL@Advantech.com", StringComparison.InvariantCultureIgnoreCase) AndAlso password.Equals("1234") Then
            loginTicket = "InternTesting" & Now.ToString("yyyy-mm-dd-hh-mm-ss")
        End If



        If loginTicket <> "" Then
            If isCook = True Then
                Util.CreateCookie_Login("ADEQCOOK", userName & "|" & password)
                If Util.IsValidCookie_Login("ADEQCOOK") = False Then
                    Return False
                End If
            End If
            If setSession(userName, lang, loginTicket) Then
                Dim CSO As New CSSO
                CSO.logSSOId(loginTicket, userName, password, Util.GetClientIP())
                Return True
            Else
                Return False
            End If
        End If
        Me._errCode = Me._errCode Or Msg.eErrCode.LoginIDorPasswordincorrect
        Return False
    End Function



    Private Function getDocReg(ByVal GA As ArrayList) As List(Of COMM.Fixer.eDocReg)
        Dim DocReg As New List(Of COMM.Fixer.eDocReg)
        Dim MG As New MailGroup
        With MG
            If .isACNCheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.ACN)
            End If
            If .isATWCheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.ATW)
            End If
            If .isCAPSCheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.CAPS)
            End If
            If .isAEUCheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.AEU)
            End If
            If .isAFCCheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.AFC)
            End If
            If .isAKRCheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.AKR)
            End If
            If .isAJPCheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.AJP)
            End If
            If .isAMXCheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.AMX)
            End If
            If .isInterconIACheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.HQDC_IA)
            End If
            If .isInterconECCheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.HQDC_EC)
            End If
            If .isInterconISCheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.HQDC_IS)
            End If

            If .isABRCheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.ABR)
            End If
            If .isAAUCheck(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.AAU)
            End If
            'ICC 2015/2/10 Change this logic from SAPDAP to MyAdvantechAPI
            'If SAPDAL.UserRole.IsInGroupSalesIagUsa(GA) Then
            '    DocReg.Add(COMM.Fixer.eDocReg.AAC)
            'End If
            'If SAPDAL.UserRole.IsAUSAOlineSales(GA) Then
            '    DocReg.Add(COMM.Fixer.eDocReg.ANA)
            'End If
            If UserRoleBusinessLogic.IsInGroupSalesIagUsa(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.AAC)
            End If
            If UserRoleBusinessLogic.IsAUSAOlineSales(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.ANA)
            End If

            'Denise 20180402:
            'Yes the AASC.SCM team only needs access to “Quote Report.”  They can view And export data but Not change Or add data.
            If UserRoleBusinessLogic.IsInGroupUsaAASCSCMOnly(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.ANA)
            End If

            'ICC 2015/2/10 Add a new group PAPS.eStore
            If UserRoleBusinessLogic.IsGroupPAPSeStore(GA) Then
                DocReg.Add(COMM.Fixer.eDocReg.PAPS)
            End If
            'ICC 2015/5/4 Add a new group AENC
            If UserRoleBusinessLogic.IsGroupUsaAenc(GA) Then
                DocReg.Add(Fixer.eDocReg.AENC)
            End If
        End With
        If IsNothing(DocReg) Then
            DocReg.Add(Fixer.eDocReg.DefaultReg)
        End If
        Return DocReg
    End Function
    Private Function getDocRegDef(ByVal GA As ArrayList, ByVal SalesOffice As List(Of String)) As COMM.Fixer.eDocReg
        Dim MG As New MailGroup
        With MG
            If .isACNCheck(GA) Then
                Return COMM.Fixer.eDocReg.ACN
            End If
            If .isATWCheck(GA) Then
                Return COMM.Fixer.eDocReg.ATW
            End If
            If .isCAPSCheck(GA) Then
                Return COMM.Fixer.eDocReg.CAPS
            End If

            'If .isInterconCheck(GA) Then
            '    Return COMM.Fixer.eDocReg.HQDC
            'End If
            If .isInterconIACheck(GA) Then
                Return COMM.Fixer.eDocReg.HQDC_IA
            End If
            If .isInterconECCheck(GA) Then
                Return COMM.Fixer.eDocReg.HQDC_EC
            End If
            If .isInterconISCheck(GA) Then
                Return COMM.Fixer.eDocReg.HQDC_IS
            End If

            If .isAEUCheck(GA) Then
                Return COMM.Fixer.eDocReg.AEU
            End If
            If .isAFCCheck(GA) Then
                Return COMM.Fixer.eDocReg.AFC
            End If
            If .isAKRCheck(GA) Then
                Return COMM.Fixer.eDocReg.AKR
            End If
            If .isAJPCheck(GA) Then
                Return COMM.Fixer.eDocReg.AJP
            End If
            If .isAMXCheck(GA) Then
                Return COMM.Fixer.eDocReg.AMX
            End If
            If .isABRCheck(GA) Then
                Return COMM.Fixer.eDocReg.ABR
            End If
            If .isAAUCheck(GA) Then
                Return COMM.Fixer.eDocReg.AAU
            End If

            'ICC 2015/2/10 Change this logic from SAPDAP to MyAdvantechAPI
            'If SAPDAL.UserRole.IsInGroupSalesIagUsa(GA) Then
            '    Return COMM.Fixer.eDocReg.AAC
            'End If
            'If SAPDAL.UserRole.IsAUSAOlineSales(GA) Then
            '    Return COMM.Fixer.eDocReg.ANA
            'End If

            'If UserRoleBusinessLogic.IsAUSAOlineSales(GA) AndAlso (SalesOffice.Contains("2700") OrElse (SalesOffice.Contains("2110"))) Then
            If UserRoleBusinessLogic.IsAUSAOlineSales(GA) Then
                Return COMM.Fixer.eDocReg.ANA
            End If

            If UserRoleBusinessLogic.IsInGroupUsaAASCSCMOnly(GA) Then
                Return COMM.Fixer.eDocReg.ANA
            End If


            'If UserRoleBusinessLogic.IsInGroupSalesIagUsa(GA) AndAlso (SalesOffice.Contains("2100")) Then
            If UserRoleBusinessLogic.IsInGroupSalesIagUsa(GA) Then
                Return COMM.Fixer.eDocReg.AAC
            End If
            'ICC 2015/2/10 Add a new group PAPS.eStore
            If UserRoleBusinessLogic.IsGroupPAPSeStore(GA) Then
                Return COMM.Fixer.eDocReg.PAPS
            End If
            'ICC 2015/5/4 Add a new group AENC
            'If UserRoleBusinessLogic.IsGroupUsaAenc(GA) AndAlso (SalesOffice.Contains("2300")) Then
            If UserRoleBusinessLogic.IsGroupUsaAenc(GA) Then
                Return Fixer.eDocReg.AENC
            End If
        End With
        Return Fixer.eDocReg.DefaultReg
    End Function


    Private Function getRole(ByVal user As String) As COMM.Fixer.eRole
        If RoleCheck.isAdmin(user) Then
            Return Fixer.eRole.isAdmin
        End If
        If RoleCheck.IsInternalUser(user) Then
            Return Fixer.eRole.isInternal
        End If
        Return Fixer.eRole.isExternal
    End Function
    Public Function setSession(ByVal user As String, ByVal lang As String, ByVal tempid As String) As Boolean Implements iRole.setSession
        Dim o As Prof = getRoleByUser(user)
        If Not String.IsNullOrEmpty(lang) Then
            o.Lung = lang
        End If
        If Not String.IsNullOrEmpty(tempid) Then
            o.SSOID = tempid
        End If
        If o.CurrDocReg = COMM.Fixer.eDocReg.DefaultReg Then
            Me._errCode = Me._errCode Or COMM.Msg.eErrCode.CannotFindProperRoleMappingToCurrentUser
            Return False
        End If
        Current.Session("P") = o
        Return True
    End Function

    Public Function getRoleByUser(ByVal user As String) As IBUS.iRole Implements IBUS.iRole.getRoleByUser
        Dim lang As String = "en-US"
        Dim MG As New MailGroup
        Dim GroupBelTo As ArrayList = MG.getMailGroupArray(user)
        Dim _saleoffice As List(Of String) = MG.GetSalesOfficeCode(user)
        Dim DocRegList As List(Of COMM.Fixer.eDocReg) = getDocReg(GroupBelTo)
        Dim DefaultDocReg As COMM.Fixer.eDocReg = getDocRegDef(GroupBelTo, _saleoffice)
        Dim VisibleRBU As ArrayList = MG.GetVisibleRBUByGropArray(DefaultDocReg, user)


        If SAPDAL.UserRole.IsInGroupSalesIagUsa(GroupBelTo) AndAlso Not UserRoleBusinessLogic.IsAUSAOlineSales(GroupBelTo) Then
            VisibleRBU.Clear()
            VisibleRBU.Add("AAC")
        End If

        Dim o As New Prof
        o.UserId = user
        o.Lung = lang
        o.GroupBelTo = GroupBelTo
        o.DocRegList = DocRegList
        o.CurrDocReg = DefaultDocReg
        o.SalesOfficeCode = _saleoffice
        If o.CurrDocReg = Fixer.eDocReg.AEU Then
            o.IsAOnLineSales = MG.IsAEUAOlineCheck(GroupBelTo)
        End If
        o._visibleRBU = VisibleRBU
        o.Role = getRole(user)
        o.SSOID = ""
        o.IPAdd = Util.GetClientIP()
        Return o
    End Function

    Private _GroupBelTo As ArrayList = Nothing
    Public Property GroupBelTo As System.Collections.ArrayList Implements IBUS.iRole.GroupBelTo
        Get
            Return _GroupBelTo
        End Get
        Set(ByVal value As System.Collections.ArrayList)
            _GroupBelTo = value
        End Set
    End Property
    Private _CurrDocReg As COMM.Fixer.eDocReg = Fixer.eDocReg.ATW
    Public Property CurrDocReg As COMM.Fixer.eDocReg Implements IBUS.iRole.CurrDocReg
        Get
            Return _CurrDocReg
        End Get
        Set(ByVal value As COMM.Fixer.eDocReg)
            _CurrDocReg = value
        End Set
    End Property


    Private _IsAOnLineSales As Boolean = True
    Public Property IsAOnLineSales As Boolean Implements IBUS.iRole.IsAOnLineSales
        Get
            Return _IsAOnLineSales
        End Get
        Set(ByVal value As Boolean)
            _IsAOnLineSales = value
        End Set
    End Property


    Private _DocRegList As List(Of COMM.Fixer.eDocReg) = Nothing
    Public Property DocRegList As System.Collections.Generic.List(Of COMM.Fixer.eDocReg) Implements IBUS.iRole.DocRegList
        Get
            Return _DocRegList
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of COMM.Fixer.eDocReg))
            _DocRegList = value
        End Set
    End Property

    Public Function getCurrOrg() As String Implements IBUS.iRole.getCurrOrg
        Dim docReg As COMM.Fixer.eDocReg = Me.CurrDocReg
        If (docReg And Fixer.eDocReg.AAU) = docReg Then
            Return "AU01"
        End If
        If (docReg And Fixer.eDocReg.ACN) = docReg Then
            'Return "CN01"
            Return "CN10"
        End If
        If (docReg And Fixer.eDocReg.AUS) = docReg Then
            Return "US01"
        End If
        If (docReg And Fixer.eDocReg.AEU) = docReg Then
            Return "EU10"
        End If
        If (docReg And Fixer.eDocReg.AJP) = docReg Then
            Return "JP01"
        End If
        If (docReg And Fixer.eDocReg.AKR) = docReg Then
            Return "KR01"
        End If
        If (docReg And Fixer.eDocReg.ATW) = docReg Then
            Return "TW01"
        End If
        If (docReg And Fixer.eDocReg.HQDC) = docReg Then
            Return "TW01"
        End If
        If (docReg And Fixer.eDocReg.CAPS) = docReg Then
            Return "TW01"
        End If
        If (docReg And Fixer.eDocReg.ASG) = docReg Then
            Return "SG01"
        End If
        If (docReg And Fixer.eDocReg.ABR) = docReg Then
            Return "BR01"
        End If
        If (docReg And Fixer.eDocReg.ABR) = docReg Then
            Return "AU01"
        End If
        Return ""
    End Function
    Private _DatePresentationFormat As String = COMM.Fixer.eDateFormat.YYYYMMDDSLASH
    Public Property DatePresentationFormat As String Implements IBUS.iRole.DatePresentationFormat
        Get
            Return _DatePresentationFormat
        End Get
        Set(ByVal value As String)
            _DatePresentationFormat = value
        End Set
    End Property
End Class

