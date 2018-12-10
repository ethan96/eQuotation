Public Interface iRole : Inherits iBase
    Property CurrDocReg As COMM.Fixer.eDocReg
    Property DocRegList As List(Of COMM.Fixer.eDocReg)
    Property Role As COMM.Fixer.eRole
    Property VisibleRBU As ArrayList
    Property GroupBelTo As ArrayList
    'Frank 20160314 keep the sales office code for identifying the sales team
    Property SalesOfficeCode As List(Of String)

    Property IsAOnLineSales As Boolean
    Property UserId As String
    Property DatePresentationFormat As String
    Property Lung As String
    Property SSOID As String
    Property IPAdd As String
    Function login(ByVal userName As String, ByVal password As String, ByVal lang As String, Optional ByVal isCook As Boolean = False) As Boolean
    Function getRoleByUser(ByVal user As String) As iRole
    Function setSession(ByVal user As String, ByVal lang As String, ByVal tempid As String) As Boolean
    Function getCurrOrg() As String
End Interface

Public Interface iSSO : Inherits iBase
    Sub logSSOId(ByVal tempId As String, ByVal userId As String, ByVal pw As String, ByVal ipAddr As String)
    Function isValidSSOId(ByVal tempId As String, ByVal userId As String) As Boolean
    Function isValidSSOMember(ByVal localIP As String, ByVal TempId As String, ByVal eMail As String) As Boolean
End Interface