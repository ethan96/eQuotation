Imports System.Text
Imports System.Web
Imports System.Configuration
Imports System.Xml
Imports System.IO
Imports Sgml
Public Class Fixer
    Public Enum eDocStatus As Integer
        <EnumDescription("Draft Quotation")> _
        QDRAFT = 0
        <EnumDescription("Finish Quotation")> _
        QFINISH = 1
        <EnumDescription("Deleted Quotation")> _
        QDELETED = 2
        <EnumDescription("Expired Quotation")> _
        QEXPIRED = 3
        <EnumDescription("Draft Order")> _
        ODRAFT = 4
        <EnumDescription("Finish Order")> _
        OFINISH = 5
        <EnumDescription("Failed Order")> _
        OFAILED = 6
    End Enum
    Public NotInheritable Class eDateFormat
        Public Shared YYYYMMDDSLASH As String = "yyyy/MM/dd"
        Public Shared YYYYMMDDDASH As String = "yyyy-MM-dd"
        Public Shared MMDDYYYYSLASH As String = "MM/dd/yyyy"
        Public Shared MMDDYYYYDASH As String = "MM-dd-yyyy"
        Public Shared DDMMYYYYSLASH As String = "dd/MM/yyyy"
        Public Shared DDMMYYYYDASH As String = "dd-MM-yyyy"
        Public Shared SAPDATE As String = YYYYMMDDDASH
    End Class

    Public Enum eEarlyShipment As Integer
        Early_Shipment_Allowed = 1
        Early_Shipment_Not_Allowed = 2
    End Enum
    Public Enum eQuoteExpDur As Integer
        EU = 30
        US = 30
        TW = 30
        KR = 30
        HQDC = 30
        BR = 15
        JP = 30
        OTHER = 45
    End Enum
    Public Enum eCartItemValidateType As Integer
        IsOrderable = 0
    End Enum
    Public Enum eDocType As Integer
        EQ = 0
        ORDER = 1
    End Enum
    Public Enum eBTOAssemblyDays As Integer
        EU = 7
        US = 5
    End Enum
    Public Enum eDocReg As Long
        DefaultReg = 0
        AUS = 2 ^ 9 Or 2 ^ 8 Or 2 ^ 7 Or 2 ^ 6 Or 2 ^ 5
        AEU = 2 ^ 4 Or 2 ^ 3 Or 2 ^ 2 Or 2 ^ 1 Or 2 ^ 0
        ATW = 2 ^ 14 Or 2 ^ 13 Or 2 ^ 12 Or 2 ^ 11 Or 2 ^ 10

        'ACN = 2 ^ 19 Or 2 ^ 18 Or 2 ^ 17 Or 2 ^ 16 Or 2 ^ 15
        ACN = 2 ^ 15

        CAPS = 2 ^ 16


        'AJP = 2 ^ 24 Or 2 ^ 23 Or 2 ^ 22 Or 2 ^ 21 Or 2 ^ 20
        AJP = 2 ^ 20
        AKR = 2 ^ 21 'Frank 20151008 arranged for AKR sales teams

        'Frank 20160523
        ABR = 2 ^ 24

        ASG = 2 ^ 25
        HQDC = 2 ^ 29 Or 2 ^ 28 Or 2 ^ 27 'Frank 20151026 Intercon
        HQDC_IA = 2 ^ 27 'i Automation
        HQDC_EC = 2 ^ 28 'Embedded Core
        HQDC_IS = 2 ^ 29 'i Service

        AAU = 2 ^ 30

        AMX = 2 ^ 5
        AFC = 2 ^ 10
        ANA = 2 ^ 7 Or 2 ^ 6 Or 2 ^ 5
        AAC = 2 ^ 7
        PAPS = 2 ^ 8 'ICC 2015/2/10 Add a new group PAPS.eStore in AUS
        AENC = 2 ^ 9 'ICC 2015/5/4 Add a new group AENC

    End Enum
    Public Enum eItemType As Integer
        Parent = 1
        Others = 0
    End Enum
    Public Enum eInsertOrRemove As Integer
        Insert = 0
        Remove = 1
    End Enum

    Public Enum eRole As Integer
        isAdmin = 1
        isInternal = 2
        isExternal = 3
    End Enum
    Public Enum ePriceATPOption
        PriceOnly = 0
        ATPOnly = 1
        PriceAndATP = 2
    End Enum
    Public Const StartLine As Integer = 0
    Public Const LineLimiter As Integer = eCartLineStep.Parent / eCartLineStep.Others
    Public Enum eProdType As Integer
        ItemStandard = 0
        ItemPTD = 1
        ItemAGSEW = 2
        ItemBTO = 3
        Dummy = 4
        HardDrive = 5
    End Enum
    Protected Enum eCartLineStep As Integer
        Parent = 100
        Others = 1
    End Enum
    Public Enum eCartAppArea As Integer
        EQ = 0
        Order = 1
    End Enum
    Public Const StartDate As String = "1900-01-01"
    Public Const AGSEWPrefix As String = "AGS-EW"
    Public Enum eUndef As Integer
        X = -999
    End Enum
    Enum eIsVirNoOnly As Integer
        CustPartNoOnly = 1
        BothPartNoAndCustMaterial = 2
        PartNoOnly = 0
    End Enum
    Enum USPrintOutFormat As Integer
        MAIN_ITEM_ONLY = 0
        SUB_ITEM_WITH_SUB_ITEM_PRICE = 1
        SUB_ITEM_WITHOUT_SUB_ITEM_PRICE = 2
        SUB_ITEM_WITHPARTNO_WITHOUT_SUB_ITEM_PRICE = 3
        IS_SHOW_VIRTUAL_ITEM_ONLY = 4
        ATW_Quote_PDF_TraditionalChinese = 20
        ATW_Quote_PDF_Englisn = 21
        ATW_Quote_Word_TraditionalChinese = 22
        ATW_Quote_Word_Englisn = 23

        AJP_Quote_Normal = 31
        AJP_Quote_ChildHidden = 32
    End Enum
End Class
Public Class MasterFixer : Inherits COMM.Fixer
    Public Shared Function ISRBU(ByVal COMPANYID As String) As Boolean
        If COMPANYID.ToUpper = "UUAAESC" Or COMPANYID.ToUpper = "EUKA001" Or COMPANYID.ToUpper = "ENLA001" Or _
           COMPANYID.ToUpper = "EPLA001" Or COMPANYID.ToUpper = "EITW005" Or COMPANYID.ToUpper = "EFRA005" Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function getExpDaysByReg(ByVal Reg As Fixer.eDocReg) As Fixer.eQuoteExpDur
        If Reg = eDocReg.AEU Then
            Return eQuoteExpDur.EU
        End If
        If Reg = eDocReg.AUS OrElse Reg = eDocReg.ANA OrElse Reg = eDocReg.ACN OrElse Reg = eDocReg.AMX OrElse Reg = eDocReg.AENC OrElse Reg = eDocReg.AAC Then
            Return eQuoteExpDur.US
        End If
        If Reg = eDocReg.ATW Then
            Return eQuoteExpDur.TW
        End If
        If Reg = eDocReg.AKR Then
            Return eQuoteExpDur.KR
        End If
        If Reg = eDocReg.HQDC OrElse Reg = eDocReg.HQDC_IA OrElse Reg = eDocReg.HQDC_EC OrElse Reg = eDocReg.HQDC_IS Then
            Return eQuoteExpDur.HQDC
        End If
        If Reg = eDocReg.ABR Then
            Return eQuoteExpDur.BR
        End If

        If Reg = eDocReg.CAPS Then
            Return eQuoteExpDur.TW
        End If

        If Reg = eDocReg.AJP Then
            Return eQuoteExpDur.JP
        End If

        Return eQuoteExpDur.OTHER

    End Function
    Public Shared Function getQuotePrefixByReg(ByVal Reg As Fixer.eDocReg) As String
        If Reg = eDocReg.AEU Then
            Return "GQ"
        End If
        If Reg = eDocReg.ACN Then
            Return "ACNQ"
        End If
        If Reg = eDocReg.AMX Then
            Return "AMXQ"
        End If
        If Reg = eDocReg.AAC Then
            Return "AACQ"
        End If
        If Reg = eDocReg.ANA Then
            Return "AUSQ"
        End If
        If Reg = eDocReg.AJP Then
            Return "AJPQ"
        End If
        If Reg = eDocReg.AKR Then
            Return "AKRQ"
        End If
        If Reg = eDocReg.HQDC_IA Then
            Return "AIAQ"
        End If
        If Reg = eDocReg.HQDC_EC Then
            Return "AIEQ"
        End If
        If Reg = eDocReg.HQDC_IS Then
            Return "AISQ"
        End If
        If Reg = eDocReg.ABR Then
            Return "ABRQ"
        End If
        If Reg = eDocReg.AAU Then
            Return "AAUQ"
        End If
        If Reg = eDocReg.ATW Then
            Return "TWQ"
        End If
        If Reg = eDocReg.CAPS Then
            Return "CAPQ"
        End If
        If Reg = eDocReg.PAPS Then 'ICC 2015/2/10 Add a new group PAPS.eStore and set quote no prefix as PUSQ
            Return "PUSQ"
        End If
        If Reg = eDocReg.AENC Then 'ICC 2015/5/4 Add a new group AENC and set quote prefix as AENQ
            Return "AENQ"
        End If
        Return "GQ"
    End Function

    Shared Function getOrderPrefixByReg(ByVal Reg As Fixer.eDocReg) As String
        Dim preFix As String = ""
        If Reg = eDocReg.AEU Then
            preFix = "FU"
        ElseIf Reg = eDocReg.ATW Then
            preFix = "QT"
        ElseIf Reg = eDocReg.ANA Then
            preFix = "AUSO"
        ElseIf Reg = eDocReg.AMX Then
            preFix = "AMXO"
        ElseIf Reg = eDocReg.AUS Then
            preFix = "BT"
        ElseIf Reg = eDocReg.ASG Then
            preFix = "SP"
        ElseIf Reg = eDocReg.AJP Then
            preFix = "JP"
        ElseIf Reg = eDocReg.ACN Then
            preFix = "SC"
            'ElseIf Reg = eDocReg.AAC Then
            '    preFix = "AACQ"
        End If
        Return preFix
    End Function

    'Public Shared Function getAccStatus(ByVal Status As String) As String
    '    Select Case Status
    '        Case "01-Platinum Channel Partner", "01-Premier Channel Partner", "02-Gold Channel Partner", "03-Certified Channel Partner"
    '            Return "CP"
    '        Case "03-Premier Key Account", "04-Premier Key Account", "06G-Golden Key Account(ACN)", "06-Key Account"
    '            Return "KA"
    '        Case "05-D&Ms PKA"
    '            Return "DMS"
    '        Case "06P-Potential Key Account", "07-General Account", "08-General Account(List Price)", "12-Leads", "11-Prospect", "10-Sales Contact", "11-Sales Contact"
    '            Return "GA"
    '        Case Else
    '            Return "GA"
    '    End Select
    '    Return "GA"
    'End Function
End Class
Public Class CartFixer : Inherits COMM.Fixer

    Private Shared Function isParent(ByVal LineNo As Integer) As Boolean
        If LineNo > StartLine AndAlso ((LineNo - StartLine) Mod (LineLimiter * eCartLineStep.Others)) = 0 Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function isValidParentLineNo(ByVal parentLineNo As Integer) As Boolean
        If parentLineNo = StartLine Then
            Return True
        End If
        If isParent(parentLineNo) Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function isParentUndStartLine(ByVal ItemType As eItemType, ByVal HigherLevel As Integer) As Boolean
        If ItemType = eItemType.Parent AndAlso HigherLevel = StartLine Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function isItemTypeIncorrect(ByVal ItemType As eItemType, ByVal LineNo As Integer) As Boolean
        If isValidParentLineNo(LineNo) Then
            If ItemType <> eItemType.Parent Then
                Return True
            End If
        Else
            If ItemType <> eItemType.Others Then
                Return True
            End If
        End If

        Return False
    End Function
    Public Shared Function isValidItemType(ByVal type As eItemType) As Boolean
        Dim t As Type = GetType(eItemType)
        For Each r As Integer In [Enum].GetValues(t)
            If r = type Then
                Return True
            End If
        Next
        Return False
    End Function


    Public Shared Function getParentLineNoFromLineNo(ByVal LineNo As Integer) As Integer
        If (LineNo - StartLine) Mod (LineLimiter * eCartLineStep.Others) = 0 Then
            Return StartLine
        End If
        Return (LineNo \ (LineLimiter * eCartLineStep.Others)) * (LineLimiter * eCartLineStep.Others)
    End Function
    Public Shared Function getStepByParent(ByVal ParentLineNo As Integer, ByVal ItemType As eItemType) As Integer
        If ParentLineNo = StartLine Then
            If ItemType = eItemType.Parent Then
                Return eCartLineStep.Parent
            End If
        End If
        Return eCartLineStep.Others
    End Function
    Public Shared Function getStartByParent(ByVal ParentLineNo As Integer) As Integer
        If ParentLineNo = StartLine Then
            Return StartLine
        End If
        Return ParentLineNo
    End Function

End Class

Public Class Util
    Public Shared Function IsInternalUser(ByVal User_Id As String) As Boolean
        Dim MailDomain As String = "", role As String = ""
        Dim uArray() As String = Split(User_Id, "@")
        Try
            MailDomain = LCase(Trim(uArray(1)))
        Catch ex As Exception
            Return False
        End Try
        Select Case LCase(MailDomain)
            Case "advantech.de", "advantech.pl", "advantech-uk.com", "advantech.fr", "advantech.it",
                 "advantech.nl", "advantech-nl.nl", "advantech.com.tw", "advantech.com.cn", "advantech.com",
                 "advantech.eu", "advantech.co.jp", "advantech.kr", "advantech.my", "advantech.sg",
                 "advantechsg.com.sg", "advantech.corp", "advantech.uk", "advantech.co.kr", "advantech.br",
                 "innocoregaming.com", "advantech.com.br", "dlog.com", "advantech.com.mx", "advanixs.com", "advantech-dlog.com", "advantech-bb.com"
                Return True
            Case Else
                Return False
        End Select
    End Function
    Shared Function isEmail(ByVal str As String) As Boolean
        Dim regExp As New RegularExpressions.Regex("^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$")
        If regExp.Match(str).Success Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function DateFormat(ByVal DATESTR As String, ByVal FF As String, ByVal TF As String, ByVal FSP As String, ByVal TSP As String) As String
        Dim Year As String = ""
        Dim Month As String = ""
        Dim Day As String = ""

        If FF.ToUpper = "YYYYMMDD" Then
            If FSP = "" Then
                Year = Left(DATESTR, 4)
                Month = Mid(DATESTR, 5, 2)
                Day = Right(DATESTR, 2)
            Else
                Year = DATESTR.Split(FSP)(0)
                Month = DATESTR.Split(FSP)(1)
                Day = DATESTR.Split(FSP)(2)
            End If
        End If
        If FF.ToUpper = "MMDDYYYY" Then
            If FSP = "" Then
                Year = Right(DATESTR, 4)
                Month = Left(DATESTR, 2)
                Day = Mid(DATESTR, 3, 2)
            Else
                Year = DATESTR.Split(FSP)(2)
                Month = DATESTR.Split(FSP)(0)
                Day = DATESTR.Split(FSP)(1)
            End If

        End If
        If FF.ToUpper = "DDMMYYYY" Then
            If FSP = "" Then
                Year = Right(DATESTR, 4)
                Month = Mid(DATESTR, 3, 2)
                Day = Left(DATESTR, 2)
            Else
                Year = DATESTR.Split(FSP)(2)
                Month = DATESTR.Split(FSP)(1)
                Day = DATESTR.Split(FSP)(0)
            End If
        End If
        If FF.ToUpper = "YYYYDDMM" Then
            If FSP = "" Then
                Year = Left(DATESTR, 4)
                Month = Right(DATESTR, 2)
                Day = Mid(DATESTR, 5, 2)
            Else
                Year = DATESTR.Split(FSP)(0)
                Month = DATESTR.Split(FSP)(2)
                Day = DATESTR.Split(FSP)(1)
            End If

        End If
        If FF.ToUpper = "MMYYYYDD" Then
            If FSP = "" Then
                Year = Mid(DATESTR, 3, 4)
                Month = Left(DATESTR, 2)
                Day = Right(DATESTR, 2)
            Else
                Year = DATESTR.Split(FSP)(1)
                Month = DATESTR.Split(FSP)(0)
                Day = DATESTR.Split(FSP)(2)
            End If
        End If
        If FF.ToUpper = "DDYYYYMM" Then
            If FSP = "" Then
                Year = Mid(DATESTR, 3, 4)
                Month = Right(DATESTR, 2)
                Day = Left(DATESTR, 2)
            Else
                Year = DATESTR.Split(FSP)(1)
                Month = DATESTR.Split(FSP)(2)
                Day = DATESTR.Split(FSP)(0)
            End If
        End If

        If TF.ToUpper = "YYYYMMDD" Then
            Return Year & TSP & Month & TSP & Day
        End If
        If TF.ToUpper = "MMDDYYYY" Then
            Return Month & TSP & Day & TSP & Year
        End If
        If TF.ToUpper = "DDMMYYYY" Then
            Return Day & TSP & Month & TSP & Year
        End If
        If TF.ToUpper = "YYYYDDMM" Then
            Return Year & TSP & Day & TSP & Month
        End If
        If TF.ToUpper = "MMYYYYDD" Then
            Return Month & TSP & Year & TSP & Day
        End If
        If TF.ToUpper = "DDYYYYMM" Then
            Return Day & TSP & Year & TSP & Month
        End If
        Return ""
    End Function
    Shared Function GetClientIP() As String
        Dim _ip As String = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If _ip = "" OrElse _ip.ToLower = "unknown" Then
            _ip = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        End If
        Return _ip
    End Function

    Shared Sub ClearCookie_Login(ByVal K As String)
        Dim DMCOOK As HttpCookie = New HttpCookie(K)
        DMCOOK.Expires = DateTime.Now.AddDays(-1)
        HttpContext.Current.Response.Cookies.Add(DMCOOK)
    End Sub

    Shared Sub CreateCookie_Login(ByVal K As String, ByVal V As String)
        If Not IsValidCookie_Login(K) Then
            Dim DMCOOK As HttpCookie = New HttpCookie(K)
            DMCOOK.Expires = DateTime.Now.AddDays(7)
            DMCOOK.Value = V
            HttpContext.Current.Response.Cookies.Add(DMCOOK)
        End If
    End Sub

    Shared Function IsValidCookie_Login(ByVal K As String) As Boolean
        Dim ADEQCOOK As HttpCookie = HttpContext.Current.Request.Cookies(K)
        If IsNothing(ADEQCOOK) Then
            Return False
        Else
            Return True
        End If
    End Function

    Shared Sub ShowCookie_Login(ByVal K As String)
        Dim ADEQCOOK As HttpCookie = HttpContext.Current.Request.Cookies(K)
        If Not IsNothing(ADEQCOOK) Then
            HttpContext.Current.Response.Write(ADEQCOOK.Value)
        End If
    End Sub

    Public Shared Function GetRuntimeSiteUrl() As String
        With HttpContext.Current
            Return String.Format("http://{0}{1}{2}", _
                                 .Request.ServerVariables("SERVER_NAME"), _
                                 IIf(.Request.ServerVariables("SERVER_PORT") = "80", "", ":" + .Request.ServerVariables("SERVER_PORT")), _
                                 IIf(HttpRuntime.AppDomainAppVirtualPath = "/", "", HttpRuntime.AppDomainAppVirtualPath))
        End With

    End Function
    Public Shared Function IsTesting() As Boolean
        With HttpContext.Current
            If .Request.ServerVariables("SERVER_PORT") <> "5001" And .Request.ServerVariables("SERVER_PORT") <> "80" And .Request.ServerVariables("SERVER_PORT") <> "3000" Then
                Return True
            End If
        End With
        Return False
    End Function

    Public Shared Sub SendEmail(ByVal FROM_Email As String, ByVal TO_Email As String, _
                                    ByVal CC_Email As String, ByVal BCC_Email As String, _
                                    ByVal Subject_Email As String, ByVal AttachFile As String, _
                                    ByVal MailBody As String, ByVal str_type As String, _
                                    Optional ByVal atts As System.IO.Stream = Nothing, Optional ByVal attsName As String = "")

        Dim msg As New System.Net.Mail.MailMessage
        If Util.isEmail(FROM_Email) Then
            msg.From = New System.Net.Mail.MailAddress(FROM_Email)
        Else
            msg.From = New System.Net.Mail.MailAddress("myadvantech@advantech.com")
        End If
        If TO_Email <> "" Then
            'nada added for send email bug...........
            TO_Email = TO_Email.Replace(",", ";")
            '/nada added for send email bug...........
            Dim ToArray As String() = Split(TO_Email, ";")
            For i As Integer = 0 To ToArray.Length - 1
                If ToArray(i) <> "" Then
                    'nada added for send email bug...........
                    If Util.isEmail(ToArray(i)) Then
                        '/nada added for send email bug...........
                        msg.To.Add(ToArray(i))
                    End If
                End If
            Next
        End If
        If CC_Email <> "" Then
            'nada added for send email bug...........
            CC_Email = CC_Email.Replace(",", ";")
            '/nada added for send email bug...........
            Dim CcArray As String() = Split(CC_Email, ";")
            For i As Integer = 0 To CcArray.Length - 1
                If CcArray(i) <> "" Then
                    If Util.isEmail(CcArray(i)) Then
                        msg.CC.Add(CcArray(i))
                    End If
                End If
            Next
        End If
        If BCC_Email <> "" Then
            'nada added for send email bug...........
            BCC_Email = BCC_Email.Replace(",", ";")
            '/nada added for send email bug...........
            Dim BccArray As String() = Split(BCC_Email, ";")
            For i As Integer = 0 To BccArray.Length - 1
                If BccArray(i) <> "" Then
                    'nada added for send email bug...........
                    If Util.isEmail(BccArray(i)) Then
                        '/nada added for send email bug...........
                        msg.Bcc.Add(BccArray(i))
                    End If
                End If
            Next
        End If

        '20060316 TC: Handle MailBody image resources
        If InStr(MailBody, "<img") > 0 Then
            'Try
            Dim send_mail_body As String = MailBody
            MailBody = "<xml>" & MailBody & "</xml>"
            Dim prefix As String = "<img", postfix As String = ">", imgarr As New ArrayList
            GetImgArr(MailBody, prefix, postfix, imgarr)
            Dim xml_img As String = "<xml>"
            For i As Integer = 0 To imgarr.Count - 1
                If InStr(imgarr(i).ToString(), "/>") <= 0 Then
                    xml_img &= Replace(imgarr(i).ToString(), ">", " />")
                Else
                    xml_img &= imgarr(i).ToString()
                End If
            Next
            xml_img &= "</xml>"

            Dim xmlDoc As New System.Xml.XmlDataDocument
            xmlDoc.LoadXml(xml_img)

            Dim ImgLinkSrcArray(0) As System.Net.Mail.LinkedResource
            Dim ImageCounter As Integer = 0
            Dim n As System.Xml.XmlNode = xmlDoc.DocumentElement

            EmbedChildNodeImageSrc(n, ImageCounter, ImgLinkSrcArray)

            MailBody = send_mail_body

            Dim xn As System.Xml.XmlNode
            Dim count As Integer = 0
            'Try
            For Each xn In n.ChildNodes
                'Response.Write(xn.Attributes("src").Value)
                MailBody = Replace(MailBody, imgarr(count).ToString(), xn.OuterXml)
                count += 1
            Next
            'Catch ex As Exception

            'End Try

            Dim av1 As System.Net.Mail.AlternateView = _
            System.Net.Mail.AlternateView.CreateAlternateViewFromString(MailBody, System.Text.Encoding.UTF8, System.Net.Mime.MediaTypeNames.Text.Html)


            For i As Integer = 0 To ImgLinkSrcArray.Length - 1
                Try
                    av1.LinkedResources.Add(ImgLinkSrcArray(i))
                Catch ex As Exception
                End Try
            Next
            msg.AlternateViews.Add(av1)

            'Catch ex As Exception

            'End Try
        End If
        msg.Body = MailBody : msg.IsBodyHtml = True : msg.SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8") : msg.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8")
        msg.Subject = Subject_Email

        If Trim(AttachFile) <> "" Then
            Dim attArray As String() = Split(AttachFile, ";")
            For i As Integer = 0 To attArray.Length - 1
                If attArray(i) <> "" Then
                    Dim Att As New System.Net.Mail.Attachment(attArray(i))
                    msg.Attachments.Add(Att)

                End If
            Next
        End If
        If Not IsNothing(atts) Then
            Dim FileName As String = IIf(String.IsNullOrEmpty(attsName.Trim), "EQ_pdf.pdf", attsName.Trim())
            Dim Att As New System.Net.Mail.Attachment(atts, FileName)
            msg.Attachments.Add(Att)
        End If
        If Util.IsTesting() Then
            msg.Subject = msg.Subject + vbTab + "TO:" + msg.To.ToString + vbTab + "CC:" + msg.CC.ToString + vbTab + "BCC:" + msg.Bcc.ToString
            msg.To.Clear() : msg.To.Add("myadvantech@advantech.com")
            msg.CC.Clear() : msg.Bcc.Clear()
        End If
        Try
            Dim m1 As New System.Net.Mail.SmtpClient
            m1.Host = ConfigurationManager.AppSettings("SMTPServer")
            m1.Send(msg)
        Catch ex As Exception
            Dim m1 As New System.Net.Mail.SmtpClient
            m1.Host = ConfigurationManager.AppSettings("SMTPServerBAK")
            m1.Send(msg)
        End Try


        For i As Integer = 0 To msg.Attachments.Count - 1
            msg.Attachments.Item(i).ContentStream.Close()
        Next
        For i As Integer = 0 To msg.AlternateViews.Count - 1
            For j As Integer = 0 To msg.AlternateViews.Item(i).LinkedResources.Count - 1
                msg.AlternateViews.Item(i).LinkedResources.Item(j).ContentStream.Close()
            Next
        Next

    End Sub
    Public Shared Sub SendEmailV3(ByVal FROM_Email As String, ByVal TO_Email As String, _
                                    ByVal CC_Email As String, ByVal BCC_Email As String, _
                                    ByVal Subject_Email As String, ByVal AttachFile As String, _
                                    ByVal MailBody As String, ByVal str_type As String, Optional ByVal IsTesting As Boolean = True, _
                                    Optional ByVal atts As System.IO.Stream = Nothing, Optional ByVal attsName As String = "")

        Dim msg As New System.Net.Mail.MailMessage
        If Util.isEmail(FROM_Email) Then
            msg.From = New System.Net.Mail.MailAddress(FROM_Email)
        Else
            msg.From = New System.Net.Mail.MailAddress("myadvantech@advantech.com")
        End If
        If TO_Email <> "" Then
            'nada added for send email bug...........
            TO_Email = TO_Email.Replace(",", ";")
            '/nada added for send email bug...........
            Dim ToArray As String() = Split(TO_Email, ";")
            For i As Integer = 0 To ToArray.Length - 1
                If ToArray(i) <> "" Then
                    'nada added for send email bug...........
                    If Util.isEmail(ToArray(i)) Then
                        '/nada added for send email bug...........
                        msg.To.Add(ToArray(i))
                    End If
                End If
            Next
        End If
        If CC_Email <> "" Then
            'nada added for send email bug...........
            CC_Email = CC_Email.Replace(",", ";")
            '/nada added for send email bug...........
            Dim CcArray As String() = Split(CC_Email, ";")
            For i As Integer = 0 To CcArray.Length - 1
                If CcArray(i) <> "" Then
                    If Util.isEmail(CcArray(i)) Then
                        msg.CC.Add(CcArray(i))
                    End If
                End If
            Next
        End If
        If BCC_Email <> "" Then
            'nada added for send email bug...........
            BCC_Email = BCC_Email.Replace(",", ";")
            '/nada added for send email bug...........
            Dim BccArray As String() = Split(BCC_Email, ";")
            For i As Integer = 0 To BccArray.Length - 1
                If BccArray(i) <> "" Then
                    'nada added for send email bug...........
                    If Util.isEmail(BccArray(i)) Then
                        '/nada added for send email bug...........
                        msg.Bcc.Add(BccArray(i))
                    End If
                End If
            Next
        End If

        '20060316 TC: Handle MailBody image resources
        If InStr(MailBody, "<img") > 0 Then
            'Try
            Dim send_mail_body As String = MailBody
            MailBody = "<xml>" & MailBody & "</xml>"
            Dim prefix As String = "<img", postfix As String = ">", imgarr As New ArrayList
            GetImgArr(MailBody, prefix, postfix, imgarr)
            Dim xml_img As String = "<xml>"
            For i As Integer = 0 To imgarr.Count - 1
                If InStr(imgarr(i).ToString(), "/>") <= 0 Then
                    xml_img &= Replace(imgarr(i).ToString(), ">", " />")
                Else
                    xml_img &= imgarr(i).ToString()
                End If
            Next
            xml_img &= "</xml>"

            Dim xmlDoc As New System.Xml.XmlDataDocument
            xmlDoc.LoadXml(xml_img)

            Dim ImgLinkSrcArray(0) As System.Net.Mail.LinkedResource
            Dim ImageCounter As Integer = 0
            Dim n As System.Xml.XmlNode = xmlDoc.DocumentElement

            EmbedChildNodeImageSrc(n, ImageCounter, ImgLinkSrcArray)

            MailBody = send_mail_body

            Dim xn As System.Xml.XmlNode
            Dim count As Integer = 0
            'Try
            For Each xn In n.ChildNodes
                'Response.Write(xn.Attributes("src").Value)
                MailBody = Replace(MailBody, imgarr(count).ToString(), xn.OuterXml)
                count += 1
            Next
            'Catch ex As Exception

            'End Try

            Dim av1 As System.Net.Mail.AlternateView = _
            System.Net.Mail.AlternateView.CreateAlternateViewFromString(MailBody, System.Text.Encoding.UTF8, System.Net.Mime.MediaTypeNames.Text.Html)


            For i As Integer = 0 To ImgLinkSrcArray.Length - 1
                Try
                    av1.LinkedResources.Add(ImgLinkSrcArray(i))
                Catch ex As Exception
                End Try
            Next
            msg.AlternateViews.Add(av1)

            'Catch ex As Exception

            'End Try
        End If
        msg.Body = MailBody : msg.IsBodyHtml = True : msg.SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8") : msg.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8")
        msg.Subject = Subject_Email

        If Trim(AttachFile) <> "" Then
            Dim attArray As String() = Split(AttachFile, ";")
            For i As Integer = 0 To attArray.Length - 1
                If attArray(i) <> "" Then
                    Dim Att As New System.Net.Mail.Attachment(attArray(i))
                    msg.Attachments.Add(Att)

                End If
            Next
        End If
        If Not IsNothing(atts) Then
            Dim FileName As String = IIf(String.IsNullOrEmpty(attsName.Trim), "EQ_pdf.pdf", attsName.Trim())
            Dim Att As New System.Net.Mail.Attachment(atts, FileName)
            msg.Attachments.Add(Att)
        End If
        If IsTesting Then
            msg.Subject = msg.Subject + vbTab + "TO:" + msg.To.ToString + vbTab + "CC:" + msg.CC.ToString + vbTab + "BCC:" + msg.Bcc.ToString
            msg.To.Clear() : msg.To.Add("eQ.Helpdesk@advantech.com")
            msg.CC.Clear()
            msg.Bcc.Clear()
            'Frank 20160608, Send GP approval request to Cathee on test site.
            If msg.Subject.IndexOf("AUSQ") > 0 Then
                'msg.CC.Add("cathee.cao@advantech.com")
            End If
        End If
        Try
            Dim m1 As New System.Net.Mail.SmtpClient
            m1.Host = ConfigurationManager.AppSettings("SMTPServer")
            m1.Send(msg)
        Catch ex As Exception
            Dim m1 As New System.Net.Mail.SmtpClient
            m1.Host = ConfigurationManager.AppSettings("SMTPServerBAK")
            m1.Send(msg)
        End Try


        For i As Integer = 0 To msg.Attachments.Count - 1
            msg.Attachments.Item(i).ContentStream.Close()
        Next
        For i As Integer = 0 To msg.AlternateViews.Count - 1
            For j As Integer = 0 To msg.AlternateViews.Item(i).LinkedResources.Count - 1
                msg.AlternateViews.Item(i).LinkedResources.Item(j).ContentStream.Close()
            Next
        Next

    End Sub

    Public Shared Sub SendEmailV2(ByVal FROM_Email As String, ByVal TO_Email As String, _
                               ByVal CC_Email As String, ByVal BCC_Email As String, _
                               ByVal Subject_Email As String, ByVal AttachedFileStream() As System.IO.Stream, _
                               ByVal AttachedFileName() As String, _
                               ByVal MailBody As String, ByVal str_type As String)


        Dim msg As New System.Net.Mail.MailMessage
        If Util.isEmail(FROM_Email) Then
            msg.From = New System.Net.Mail.MailAddress(FROM_Email)
        Else
            msg.From = New System.Net.Mail.MailAddress("myadvantech@advantech.com")
        End If
        If TO_Email <> "" Then
            'nada added for send email bug...........
            TO_Email = TO_Email.Replace(",", ";")
            '/nada added for send email bug...........
            Dim ToArray As String() = Split(TO_Email, ";")
            For i As Integer = 0 To ToArray.Length - 1
                If ToArray(i) <> "" Then
                    'nada added for send email bug...........
                    If Util.isEmail(ToArray(i)) Then
                        '/nada added for send email bug...........
                        msg.To.Add(ToArray(i))
                    End If
                End If
            Next
        End If
        If CC_Email <> "" Then
            'nada added for send email bug...........
            CC_Email = CC_Email.Replace(",", ";")
            '/nada added for send email bug...........
            Dim CcArray As String() = Split(CC_Email, ";")
            For i As Integer = 0 To CcArray.Length - 1
                If CcArray(i) <> "" Then
                    If Util.isEmail(CcArray(i)) Then
                        msg.CC.Add(CcArray(i))
                    End If
                End If
            Next
        End If
        If BCC_Email <> "" Then
            'nada added for send email bug...........
            BCC_Email = BCC_Email.Replace(",", ";")
            '/nada added for send email bug...........
            Dim BccArray As String() = Split(BCC_Email, ";")
            For i As Integer = 0 To BccArray.Length - 1
                If BccArray(i) <> "" Then
                    'nada added for send email bug...........
                    If Util.isEmail(BccArray(i)) Then
                        '/nada added for send email bug...........
                        msg.Bcc.Add(BccArray(i))
                    End If
                End If
            Next
        End If

        '20060316 TC: Handle MailBody image resources
        If InStr(MailBody, "<img") > 0 Then
            'Try
            Dim send_mail_body As String = MailBody
            MailBody = "<xml>" & MailBody & "</xml>"
            Dim prefix As String = "<img", postfix As String = ">", imgarr As New ArrayList
            GetImgArr(MailBody, prefix, postfix, imgarr)
            Dim xml_img As String = "<xml>"
            For i As Integer = 0 To imgarr.Count - 1
                If InStr(imgarr(i).ToString(), "/>") <= 0 Then
                    xml_img &= Replace(imgarr(i).ToString(), ">", " />")
                Else
                    xml_img &= imgarr(i).ToString()
                End If
            Next
            xml_img &= "</xml>"

            Dim xmlDoc As New System.Xml.XmlDataDocument
            xmlDoc.LoadXml(xml_img)

            Dim ImgLinkSrcArray(0) As System.Net.Mail.LinkedResource
            Dim ImageCounter As Integer = 0
            Dim n As System.Xml.XmlNode = xmlDoc.DocumentElement

            EmbedChildNodeImageSrc(n, ImageCounter, ImgLinkSrcArray)

            MailBody = send_mail_body

            Dim xn As System.Xml.XmlNode
            Dim count As Integer = 0
            'Try
            For Each xn In n.ChildNodes
                'Response.Write(xn.Attributes("src").Value)
                MailBody = Replace(MailBody, imgarr(count).ToString(), xn.OuterXml)
                count += 1
            Next
            'Catch ex As Exception

            'End Try

            Dim av1 As System.Net.Mail.AlternateView = _
            System.Net.Mail.AlternateView.CreateAlternateViewFromString(MailBody, System.Text.Encoding.UTF8, System.Net.Mime.MediaTypeNames.Text.Html)


            For i As Integer = 0 To ImgLinkSrcArray.Length - 1
                Try
                    av1.LinkedResources.Add(ImgLinkSrcArray(i))
                Catch ex As Exception
                End Try
            Next
            msg.AlternateViews.Add(av1)

            'Catch ex As Exception

            'End Try
        End If
        msg.Body = MailBody : msg.IsBodyHtml = True : msg.SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8") : msg.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8")
        msg.Subject = Subject_Email

        If AttachedFileStream IsNot Nothing AndAlso AttachedFileStream.Length > 0 Then
            'Dim attArray As String() = Split(AttachFile, ";")
            For i As Integer = 0 To AttachedFileStream.Length - 1
                Dim Att As New System.Net.Mail.Attachment(AttachedFileStream(i), AttachedFileName(i))
                msg.Attachments.Add(Att)
            Next
        End If
        'If Not IsNothing(atts) Then
        '    Dim FileName As String = IIf(String.IsNullOrEmpty(attsName.Trim), "EQ_pdf.pdf", attsName.Trim())
        '    Dim Att As New System.Net.Mail.Attachment(atts, FileName)
        '    msg.Attachments.Add(Att)
        'End If
        If Util.IsTesting() Then
            msg.Subject = msg.Subject + vbTab + "TO:" + msg.To.ToString + vbTab + "CC:" + msg.CC.ToString + vbTab + "BCC:" + msg.Bcc.ToString
            msg.To.Clear() : msg.To.Add("myadvantech@advantech.com")
            msg.CC.Clear() : msg.Bcc.Clear()
        End If
        Try
            Dim m1 As New System.Net.Mail.SmtpClient
            m1.Host = ConfigurationManager.AppSettings("SMTPServer")
            m1.Send(msg)
        Catch ex As Exception
            Dim m1 As New System.Net.Mail.SmtpClient
            m1.Host = ConfigurationManager.AppSettings("SMTPServerBAK")
            m1.Send(msg)
        End Try


        For i As Integer = 0 To msg.Attachments.Count - 1
            msg.Attachments.Item(i).ContentStream.Close()
        Next
        For i As Integer = 0 To msg.AlternateViews.Count - 1
            For j As Integer = 0 To msg.AlternateViews.Item(i).LinkedResources.Count - 1
                msg.AlternateViews.Item(i).LinkedResources.Item(j).ContentStream.Close()
            Next
        Next

    End Sub


    Private Shared Function GetImgArr(ByVal str As String, ByVal prefix As String, ByVal postfix As String, ByRef ImgArr As ArrayList) As Integer
        Dim len_prefix = InStr(str, prefix)
        str = Mid(str, InStr(str, prefix))
        Dim len_postfix = InStr(str, postfix)
        '--{2006-06-28}--Daive: Avoid the duplicate image in attachment
        Dim ImgCode As String = Left(str, InStr(str, postfix))
        Dim i As Integer = 0
        Dim ExistFlag As Boolean = False
        For i = 0 To ImgArr.Count - 1
            If ImgArr.Item(i).ToString.Trim.ToUpper = ImgCode.Trim.ToUpper Then
                ExistFlag = True
                Exit For
            End If
        Next
        If ExistFlag = False Then ImgArr.Add(ImgCode)

        'ImgArr.Add(Left(str, InStr(str, postfix)))
        str = Mid(str, len_postfix + 1)
        If InStr(str, prefix) > 0 Then
            GetImgArr(str, prefix, postfix, ImgArr)
        End If
        Return 1
    End Function
    Private Shared Sub EmbedChildNodeImageSrc(ByRef sn As System.Xml.XmlNode, ByRef ImageCounter As Integer, ByRef LinkSrcArray As System.Net.Mail.LinkedResource())

        Dim ssn As System.Xml.XmlNode
        Try
            For Each ssn In sn.ChildNodes

                If LCase(ssn.Name) = "img" Then

                    If File.Exists(HttpContext.Current.Server.MapPath(ssn.Attributes("src").Value)) Then

                        Dim ImgLinkSrc1 As System.Net.Mail.LinkedResource = Nothing
                        'Try
                        ImgLinkSrc1 = New System.Net.Mail.LinkedResource(HttpContext.Current.Server.MapPath(ssn.Attributes("src").Value))
                        'Catch ex As Exception
                        '    HttpContext.Current.Response.Write(ex.Message)
                        'End Try

                        ImgLinkSrc1.ContentId = "Img" & ImageCounter
                        ImgLinkSrc1.ContentType.Name = "Img" & ImageCounter
                        ssn.Attributes("src").Value = "cid:Img" & ImageCounter
                        ImageCounter += 1
                        ReDim Preserve LinkSrcArray(ImageCounter - 1)
                        LinkSrcArray(ImageCounter - 1) = ImgLinkSrc1

                    End If

                End If

                If ssn.ChildNodes.Count > 0 Then
                    EmbedChildNodeImageSrc(ssn, ImageCounter, LinkSrcArray)
                End If
            Next
        Catch ex As Exception
            Exit Sub
        End Try

    End Sub

    Public Shared Function Format2SAPItem(ByVal Part_No As String) As String
        Try
            If IsNumericItem(Part_No) And Not Part_No.Substring(0, 1).Equals("0") Then
                Dim zeroLength As Integer = 18 - Part_No.Length
                For i As Integer = 0 To zeroLength - 1
                    Part_No = "0" & Part_No
                Next
                Return Part_No
            Else
                Return Part_No
            End If
        Catch ex As Exception
            Return Part_No
        End Try
    End Function
    Public Shared Function Format2LocalItem(ByVal Part_No As String) As String
        Try
            If IsNumericItem(Part_No) Then Return Part_No.ToUpper.ToString.Trim.TrimStart("0")
            Return Part_No
        Catch ex As Exception
            Return Part_No
        End Try
    End Function
    Shared Function HtmlToXML(ByVal URL As String, ByRef XMLDOC As System.Xml.XmlDocument) As String
        Dim HtmlWriter As New StringWriter()
        Dim HtmlPage As String = ""
        Dim mysgmlReader As New SgmlReader
        Dim strWriter As New StringWriter()
        Dim xmlWriter As New XmlTextWriter(strWriter)
        'Try
        HttpContext.Current.Server.Execute(URL, HtmlWriter)
        HtmlPage = HtmlWriter.ToString
        mysgmlReader.DocType = "HTML"
        mysgmlReader.InputStream = New System.IO.StringReader(HtmlPage)

        xmlWriter.Formatting = Formatting.Indented
        While mysgmlReader.Read()
            If mysgmlReader.NodeType <> XmlNodeType.Whitespace Then
                xmlWriter.WriteNode(mysgmlReader, True)
            End If
        End While
        XMLDOC.LoadXml(strWriter.ToString)

        'Catch EX As Exception
        '    Return "E:" & EX.ToString
        'End Try
        Return "S"
    End Function
    Public Shared Function getCurrSign(ByVal currency As String) As String
        Select Case UCase(Trim(currency))
            Case "EUR"
                Return "&euro;"
            Case "USD", "US"
                Return "$"
            Case "YEN", "JPY"
                Return "&yen;"
            Case "NTD", "TWD"
                Return "NT "
            Case "RMB"
                Return "RMB "
            Case "GBP"
                Return "&pound;"
            Case "AUD"
                Return "AUD"
            Case Else
                Return currency
        End Select
    End Function
    Public Shared Function getGridViewCellByHeader(ByVal GvHeader As System.Web.UI.WebControls.GridViewRow, ByVal GvRow As System.Web.UI.WebControls.GridViewRow, ByVal Header As String) As System.Web.UI.WebControls.TableCell
        Dim n As Integer = 0
        Dim Destn As Integer = -1
        For Each c As System.Web.UI.WebControls.TableCell In GvHeader.Cells
            n += 1
            Dim str As String = ""
            If c.Text.Trim <> "" Then
                str &= c.Text
            End If
            If c.HasControls Then
                For Each r As System.Web.UI.Control In c.Controls
                    If r.GetType = GetType(System.Web.UI.WebControls.LinkButton) Then
                        str &= "|" & CType(r, System.Web.UI.WebControls.LinkButton).Text
                    End If
                    If r.GetType = GetType(System.Web.UI.WebControls.Label) Then
                        str &= "|" & CType(r, System.Web.UI.WebControls.Label).Text
                    End If
                    If r.GetType = GetType(System.Web.UI.LiteralControl) Then
                        str &= "|" & CType(r, System.Web.UI.LiteralControl).Text
                    End If
                    If r.GetType = GetType(System.Web.UI.WebControls.TextBox) Then
                        str &= "|" & CType(r, System.Web.UI.WebControls.TextBox).Text
                    End If
                Next
            End If
            If str.ToUpper.Contains(Header.ToUpper) Then
                Destn = n - 1
                Exit For
            End If
        Next
        If Destn <> -1 Then
            Return GvRow.Cells(Destn)
        End If
        Return Nothing
    End Function

    Public Shared Function FormatSAPDate(ByVal xDate As String) As String
        Dim xYear As String = "0000"
        Dim xMonth As String = "00"
        Dim xDay As String = "00"
        Try


            If IsDate(xDate) = True Then
                xYear = Year(xDate).ToString
                xMonth = Month(xDate).ToString
                xDay = Day(xDate).ToString
            Else
                Dim ArrDate() As String = xDate.Split("/")

                If ArrDate(0).Length = 4 Then
                    xYear = ArrDate(0)
                    xMonth = ArrDate(1)
                    xDay = ArrDate(2)
                ElseIf UBound(ArrDate) >= 2 Then
                    xYear = ArrDate(2)
                    xMonth = ArrDate(0)
                    xDay = ArrDate(1)
                ElseIf UBound(ArrDate) = 0 Then
                    If ArrDate(0).Length = 8 Then
                        xYear = Left(ArrDate(0), 4)
                        xMonth = Mid(ArrDate(0), 5, 2)
                        xDay = Right(ArrDate(0), 2)
                    End If
                End If
            End If

            If xMonth.Length = 1 Then
                xMonth = "0" & xMonth
            End If
            If xDay.Length = 1 Then
                xDay = "0" & xDay
            End If
        Catch ex As Exception

        End Try
        If xYear = "0000" And xMonth = "00" And xDay = "00" Then
            Return ""
        Else
            Return xYear & "/" & xMonth & "/" & xDay
        End If
    End Function

    Public Shared Function IsNumericItem(ByVal part_no As String) As Boolean

        Dim pChar() As Char = part_no.ToCharArray()

        For i As Integer = 0 To pChar.Length - 1
            If Not IsNumeric(pChar(i)) Then
                Return False
                Exit Function
            End If
        Next

        Return True
    End Function
    Public Shared Function RemoveZeroString(ByVal NumericPart_No As String) As String
        If IsNumericItem(NumericPart_No) Then
            For i As Integer = 0 To NumericPart_No.Length - 1
                If Not NumericPart_No.Substring(i, 1).Equals("0") Then
                    Return NumericPart_No.Substring(i)
                    Exit For
                End If
            Next
            Return NumericPart_No
        Else
            Return NumericPart_No
        End If
    End Function


    Public Shared Function showDT(ByVal ODT As DataTable) As String
        HttpContext.Current.Response.Write("<table border=""1""><tr>")
        For i As Integer = 0 To ODT.Columns.Count - 1
            HttpContext.Current.Response.Write("<td>" & ODT.Columns(i).Caption & "</td>")
        Next
        HttpContext.Current.Response.Write("</tr>")

        For j As Integer = 0 To ODT.Rows.Count - 1
            HttpContext.Current.Response.Write("<tr>")
            For k As Integer = 0 To ODT.Columns.Count - 1
                HttpContext.Current.Response.Write("<td>")
                If IsDBNull(ODT.Rows(j)(k)) Then
                    ODT.Rows(j)(k) = ""
                End If
                HttpContext.Current.Response.Write(ODT.Rows(j)(k))
                HttpContext.Current.Response.Write("<br/></td>")
            Next
            HttpContext.Current.Response.Write("</tr>")
        Next
        HttpContext.Current.Response.Write("</table>")
        Return "ok"
    End Function
    Public Shared Function getDTHtml(ByVal ODT As DataTable) As String
        Dim str As String = ""
        If Not IsNothing(ODT) AndAlso ODT.Rows.Count > 0 Then
            str = "<table border=""1"" width='100%'><tr>"
            For i As Integer = 0 To ODT.Columns.Count - 1
                str &= "<td>" & ODT.Columns(i).Caption & "</td>"
            Next
            str &= "</tr>"

            For j As Integer = 0 To ODT.Rows.Count - 1
                str &= "<tr>"
                For k As Integer = 0 To ODT.Columns.Count - 1
                    str &= "<td>"
                    str &= ODT.Rows(j)(k)
                    str &= "<br/></td>"
                Next
                str &= "</tr>"
            Next
            str &= "</table>"
        End If
        Return str
    End Function
    Public Overloads Shared Function showMessage(ByVal str As String, Optional ByVal DestUrl As String = "") As Integer
        Dim P As System.Web.UI.Page = DirectCast(HttpContext.Current.Handler, System.Web.UI.Page)
        If Not IsNothing(P) Then
            P.Items("err") = str
            P.Items("desturl") = DestUrl
        End If
        Return 1
    End Function
    Shared Function getXmlBlockByID(ByVal TYPE As String, ByVal ID As String, _
                                 ByVal XMLDOC As System.Xml.XmlDocument, ByRef retXMLBlock As String) As String

        Dim root As XmlNodeList = XMLDOC.DocumentElement.GetElementsByTagName(TYPE.ToLower)
        For Each x As XmlNode In root
            Dim ex As XmlElement = CType(x, XmlElement)
            If ex.Attributes("id") IsNot Nothing AndAlso ex.Attributes("id").Value.ToLower = ID.ToLower Then
                retXMLBlock = x.OuterXml.ToString
            End If
        Next
        Return "S"

    End Function
    Shared Function HtmlStrToXML(ByVal MyString As String, ByRef XMLDOC As System.Xml.XmlDocument, ByVal elementName As String) As String

        Dim mysgmlReader As New SgmlReader
        Dim strWriter As New StringWriter()
        Dim xmlWriter As New XmlTextWriter(strWriter)

        mysgmlReader.DocType = elementName
        mysgmlReader.InputStream = New System.IO.StringReader(MyString)

        xmlWriter.Formatting = Formatting.Indented
        While mysgmlReader.Read()
            If mysgmlReader.NodeType <> XmlNodeType.Whitespace Then
                xmlWriter.WriteNode(mysgmlReader, True)
            End If
        End While
        XMLDOC.LoadXml(strWriter.ToString)
        Return "S"
    End Function
    Public Shared Sub Write2Fie(ByVal value As String)
        Dim p As String = "d:\a.txt"
        Using fs As System.IO.FileStream = New System.IO.FileStream(p, System.IO.FileMode.Append)
            Dim sr As New System.IO.StreamWriter(fs)
            sr.WriteLine(value)
            sr.Close()
            sr = Nothing
            fs.Close()
            fs.Dispose()
        End Using
    End Sub
End Class









