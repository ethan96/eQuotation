Imports System.Configuration
Imports System.Text

Public Class MailGroup
    Public Function IsInMailGroupEQ(ByVal userid As String, ParamArray RoleNames As String()) As Boolean
        Dim MGroups As ArrayList = getMailGroupArray(userid)
        For Each RoleName In RoleNames
            For Each group As String In MGroups
                If String.Equals(group, RoleName, StringComparison.CurrentCultureIgnoreCase) Then
                    Return True
                End If
            Next
        Next
        Return False
    End Function

    Public Function IsInMailGroupMyA(ByVal userid As String, ByVal GroupName As String) As Boolean
        Dim dt As DataTable = GetMailGroupAsDatatable(userid)
        Dim _rows() As DataRow = dt.Select("Name='" & GroupName.Replace("'", "''") & "'")

        If (_rows IsNot Nothing AndAlso _rows.Count > 0) Then
            Return True
        End If
        Return False
    End Function
    'Public Function GetMailGroupAsDatatable(ByVal User As String) As DataTable
    '    Dim str1 As String = String.Format(" select distinct C.Name " & _
    '                            " from ADVANTECH_ADDRESSBOOK a left join ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID inner join ADVANTECH_ADDRESSBOOK_GROUP c on a.ID=c.ID   " & _
    '                            " where b.Email = N'{0}' or a.PrimarySmtpAddress=N'{0}' ", User)
    '    Return dbUtil.dbGetDataTable("MY", str1)

    'End Function

    Public Function GetMailGroupAsDatatable(ByVal User As String) As DataTable
        Dim str1 As String = String.Format(" Select distinct C.Group_Name as Name " &
                                " From AD_MEMBER a Left Join AD_MEMBER_ALIAS b on a.PrimarySmtpAddress=b.EMAIL Inner Join AD_MEMBER_GROUP c on a.PrimarySmtpAddress=c.EMAIL " &
                                " Where b.ALIAS_EMAIL = N'{0}' or a.PrimarySmtpAddress=N'{0}' ", User)
        Return dbUtil.dbGetDataTable("MY", str1)

    End Function



    Public Function GetSalesOfficeCode(ByVal User As String) As List(Of String)

        Dim _returnlist As New List(Of String)

        If (String.IsNullOrEmpty(User)) Then
            Return _returnlist
        End If

        User = User.Replace("'", "''")

        Dim _sql As New StringBuilder

        _sql.AppendLine(" select email,salesoffice from MyAdvantechGlobal.dbo.SAP_EMPLOYEE ")
        _sql.AppendLine(" where Email = N'" & User & "' ")
        _sql.AppendLine(" union ")
        _sql.AppendLine(" select email,salesoffice from MultiSalesTeamUser ")
        _sql.AppendLine(" where Email = N'" & User & "' ")

        Dim _dt As DataTable = dbUtil.dbGetDataTable("EQ", _sql.ToString)

        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
            For Each _row As DataRow In _dt.Rows
                _returnlist.Add(_row.Item("salesoffice").ToString)
            Next
        End If

        Return _returnlist
    End Function

    Public Function getMailGroupArray(ByVal User As String) As ArrayList
        Dim Ar As New ArrayList

        'Frank 20170706: Create test account for Gary's intern
        If User.Equals("DMKT.ACL@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then
            Ar.Add(VG.AEU.ToUpper)
            Return Ar
        End If

        If User.Equals("frank.chung@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then
            Ar.Add(VG.ATW.ToUpper)
            Return Ar
        End If

        If User.Equals("ADalghan@advantech-bb.com", StringComparison.InvariantCultureIgnoreCase) Then
            Ar.Add(("IA.eSales").ToUpper)
            Return Ar
        End If


        'Frank 20170411:Release test site for ADlog contact window Felix and Linda to test eQ
        If User.Equals("felix.pfefferkorn@advantech-dlog.com", StringComparison.InvariantCultureIgnoreCase) _
            OrElse User.Equals("linda.li@advantech-dlog.com", StringComparison.InvariantCultureIgnoreCase) Then
            If Util.IsTesting Then
                Ar.Add(VG.AEU.ToUpper)
            End If
            Return Ar
        End If


        'Frank 20141127: Gary.Lee need to access equotation as AUS view
        If User.Equals("gary.lee@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then
            'Jay: Gary.Lee only can check Aonline.USA.IAG's quote
            Ar.Add("Aonline.USA.IAG".ToUpper())
            Return Ar
        End If

        'Frank: Assign AKR permission to Iris
        If User.Equals("iris.tsai@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then
            Ar.Add(VG.ATW.ToUpper)
            Return Ar
        End If

        'Frank: Assign AKR permission to Julia.Wong
        If User.Equals("Julia.Wong@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then
            Ar.Add(VG.HQDC)
            Return Ar
        End If

        'Ryan 20161020 Add for Joe Neary case
        'If User.Equals("josephn@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then
        '    Dim groupforJoe As String = System.Web.HttpContext.Current.Session("Joe").ToString
        '    If Not String.IsNullOrEmpty(groupforJoe) Then
        '        If groupforJoe.Equals("Joebtn_1", StringComparison.InvariantCultureIgnoreCase) Then
        '            Ar.Add("AOnline.USA.IAG".ToUpper())
        '            Return Ar
        '        ElseIf groupforJoe.Equals("Joebtn_2", StringComparison.InvariantCultureIgnoreCase) Then
        '            Ar.Add("SALES.IAG.USA".ToUpper())
        '            Return Ar
        '        End If
        '    End If
        'End If
        'End Ryan 20161020 Add

        'Frank 20170220 Add for Dora Wu case
        If User.Equals("dora.wu@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then
            Dim groupforJoe As String = System.Web.HttpContext.Current.Session("Joe").ToString
            If Not String.IsNullOrEmpty(groupforJoe) Then
                If groupforJoe.Equals("Joebtn_1", StringComparison.InvariantCultureIgnoreCase) Then
                    Ar.Add("Sales.ATW.AOL-ATC(IIoT)".ToUpper())
                    Return Ar
                ElseIf groupforJoe.Equals("Joebtn_2", StringComparison.InvariantCultureIgnoreCase) Then
                    Ar.Add("Sales.ATW.AOL-EC".ToUpper())
                    Return Ar
                End If
            End If
        End If
        'End Frank 20170220 Add

        'Frank 20170830 Add for Ingrid.Lin case
        If User.Equals("Ingrid.Lin@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then
            Dim groupforJoe As String = System.Web.HttpContext.Current.Session("Joe").ToString
            If Not String.IsNullOrEmpty(groupforJoe) Then
                If groupforJoe.Equals("Joebtn_1", StringComparison.InvariantCultureIgnoreCase) Then
                    Ar.Add("InterCon.Embedded".ToUpper())
                    Return Ar
                ElseIf groupforJoe.Equals("Joebtn_2", StringComparison.InvariantCultureIgnoreCase) Then
                    Ar.Add("Sales.AEU".ToUpper())
                    Return Ar
                End If
            End If
        End If
        'End Frank 20170220 Add



        Dim str As String = String.Format("SELECT SALES_CODE,COMPANY_ID,EMAIL FROM FRANCHISER WHERE (EMAIL = '{0}')", User)
        Dim _dt As New DataTable
        _dt = dbUtil.dbGetDataTable("MY", str)
        If _dt.Rows.Count > 0 Then
            Ar.Add(VG.AFC.ToUpper)
        End If

        If User.EndsWith("@advantech.com.mx", StringComparison.InvariantCultureIgnoreCase) Then Ar.Add(VG.AMX.ToUpper)

        If User.Equals("manami.doi@advantech.com", StringComparison.InvariantCultureIgnoreCase) OrElse
           User.Equals("eileen.soh@advantech.com", StringComparison.InvariantCultureIgnoreCase) OrElse
           User.Equals("Christina.Liu@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then
            Ar.Add(VG.AJP.ToUpper)
            Return Ar
        End If

        'Frank ABR employee verification
        str = String.Format("SELECT EMAIL_ADDR,ORG_NAME,ORG_TYPE FROM EZ_EMPLOYEE WHERE (EMAIL_ADDR = '{0}')", User)
        _dt = New DataTable
        _dt = dbUtil.dbGetDataTable("MY", str)
        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 _
            AndAlso _dt.Rows(0).Item("ORG_NAME").ToString.Equals("ABR", StringComparison.InvariantCultureIgnoreCase) Then
            Ar.Add(VG.ABR.ToUpper)
            Return Ar
        End If


        Dim strEUDomains() As String = {"advantech.fr", "advantech-uk.com", "advantech.pl", "advantech.de", "advantech.nl", "advantech.com.tw", "advantech.it", "advantech.eu", "advantech-bb.com"}
        If strEUDomains.Contains(User.Split("@")(1).ToLower) Then
            Ar.Add(VG.AEU.ToUpper)
        End If

        Dim dt As DataTable = GetMailGroupAsDatatable(User)
        If dt.Rows.Count > 0 Then
            For Each r As DataRow In dt.Rows
                Ar.Add(r.Item("Name").ToString.ToUpper)
            Next
        End If
        Return Ar
    End Function
    Public Function isACNCheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(VG.ACN.ToUpper) _
            OrElse GA.Contains(("AONLINE.ACN").ToUpper) Then
            Return True
        End If
        Return False
    End Function

    Public Function isCAPSCheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(VG.CAPS.ToUpper) _
            OrElse GA.Contains(("CAPS.BD").ToUpper) Then
            Return True
        End If
        Return False
    End Function


    Public Function isATWCheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(VG.ATW.ToUpper) _
            OrElse GA.Contains(("Sales.ATW.AOL-Neihu(IIoT)").ToUpper) _
            OrElse GA.Contains(("Sales.ATW.AOL-EC").ToUpper) _
            OrElse GA.Contains(("Sales.ATW.Emb'Core").ToUpper) _
            OrElse GA.Contains(("Sales.ATW.KA.ES-KA1").ToUpper) _
            OrElse GA.Contains(("Sales.ATW.KA.ES-KA2").ToUpper) _
            OrElse GA.Contains(("Sales.ATW.KA.ES-KA3").ToUpper) _
            OrElse GA.Contains(("Sales.ATW.KA.ES-KA4").ToUpper) _
            OrElse GA.Contains(("Sales.ATW.KA.IAKA").ToUpper) _
            OrElse GA.Contains("ATP.IA.ACL") _
            OrElse GA.Contains(("CallCenter.IA.ACL").ToUpper) _
            OrElse GA.Contains("MA.IA.ACL") _
            OrElse GA.Contains(("Sales.ATW.AiS").ToUpper) _
            OrElse GA.Contains(("Sales.ATW.Ucity").ToUpper) _
            OrElse GA.Contains(("Sales.ATW.AOL-ATC(IIoT)").ToUpper) Then
            'OrElse GA.Contains(("ACL.Sourcer").ToUpper) Then 'ACL.Sourcer 這個群組想試用eQuotation 2015/07/06
            Return True
        End If
        Return False
    End Function
    Public Function isAFCCheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(VG.AFC.ToUpper) Then
            Return True
        End If
        Return False
    End Function

    Public Function isAKRCheck(ByVal GA As ArrayList) As Boolean

        'Dear Frank

        'Since PROS.AKR is only for AOL and Marketing. We cannot add Bright into PROS.AKR. 
        'I think, Sales. AKR should be the one. Please refer to attached e-mail. 
        'I will ask Sangjo to update SALES.AKR to the latest version since I can see some e-mail invalid. 
        '        Best Regards
        '        Nadia


        'If GA.Contains(VG.AKR.ToUpper) OrElse GA.Contains(("PROS.AKR").ToUpper) Then
        If GA.Contains(VG.AKR.ToUpper) OrElse GA.Contains(("SALES.AKR").ToUpper) Then
            Return True
        End If
        Return False
    End Function

    Public Function isInterconCheck(ByVal GA As ArrayList) As Boolean
        If isInterconIACheck(GA) OrElse isInterconECCheck(GA) Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Intercon i Automation user identification
    ''' </summary>
    ''' <param name="GA"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isInterconIACheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(VG.HQDC.ToUpper) OrElse GA.Contains(("IA.eSales").ToUpper) Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Intercon Embedded Core user identification
    ''' </summary>
    ''' <param name="GA"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isInterconECCheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(VG.HQDC.ToUpper) OrElse GA.Contains(("InterCon.Embedded").ToUpper) Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Intercon iService user identification
    ''' </summary>
    ''' <param name="GA"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isInterconISCheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(VG.HQDC.ToUpper) OrElse
            GA.Contains(("InterCon.iService").ToUpper) OrElse
            GA.Contains(("InterCon.iLogistic").ToUpper) Then
            Return True
        End If
        Return False
    End Function


    Public Function isABRCheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(VG.ABR.ToUpper) Then
            Return True
        End If
        Return False
    End Function

    Public Function isAAUCheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(VG.AAU.ToUpper) OrElse GA.Contains(("Sales.AAU").ToUpper) Then
            Return True
        End If
        Return False
    End Function


    Public Function isAJPCheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(VG.AJP.ToUpper) _
            OrElse GA.Contains(("ajp_sales_all").ToUpper) _
            OrElse GA.Contains(("ajsc.ctos").ToUpper) Then
            'Frank 20170202: 
            'TC建議請也開放AJP eQ給ajsc.ctos這個群組，這樣Jack與Ronald都可以試用了, Jack之後應該會指派Ronald接續測試eQ CTOS的部分
            Return True
        End If
        Return False
    End Function
    Public Function isAMXCheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(VG.AMX.ToUpper) OrElse
            (GA.Contains(("Aonline.Mexico").ToUpper) AndAlso Not SAPDAL.UserRole.IsAUSAOlineSales(GA)) Then
            Return True
        End If
        Return False
    End Function
    Public Function isAEUCheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(VG.AEU.ToUpper) OrElse
            GA.Contains(("Advantech-Innocore").ToUpper) OrElse
            GA.Contains(("AEU.AOnline").ToUpper) OrElse
            GA.Contains(("Sales.AEU").ToUpper) OrElse
            GA.Contains(("EMPLOYEE.APL").ToUpper) Then

            'Frank commented out below code because this team did not create any quote for AEU customer
            'GA.Contains(("SC.AENC.USA").ToUpper) OrElse _
            'Let group SC.AENC.USA can create quote for AEU's customer
            Return True
        End If

        Return False
    End Function

    Public Function IsAEUAOlineCheck(ByVal GA As ArrayList) As Boolean
        If GA.Contains(("AEU.AOnline").ToUpper) Then
            Return True
        End If
        Return False
    End Function

    ' ''' <summary>
    ' ''' Return true if user is AUS Aonline sales(including Aonline.USA and Aonline.USA.IAG)
    ' ''' </summary>
    ' ''' <param name="GA"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function IsAUSAOlineSales(ByVal GA As ArrayList) As Boolean
    '    If IsInGroupAonlineUsa(GA) OrElse IsInGroupAonlineUsaIag(GA) Then
    '        Return True
    '    End If
    '    Return False
    'End Function

    ' ''' <summary>
    ' ''' Return true if user belongs to mail group Aonline.USA or SALES.AISA.USA
    ' ''' </summary>
    ' ''' <param name="GA"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function IsInGroupAonlineUsa(ByVal GA As ArrayList) As Boolean
    '    If GA.Contains(("Aonline.USA").ToUpper) OrElse _
    '        GA.Contains("SALES.AISA.USA") Then
    '        Return True
    '    End If
    '    Return False
    'End Function

    ' ''' <summary>
    ' ''' Return true if user belongs to mail group Aonline.USA.IAG
    ' ''' </summary>
    ' ''' <param name="GA"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function IsInGroupAonlineUsaIag(ByVal GA As ArrayList) As Boolean
    '    If GA.Contains(("Aonline.USA.IAG").ToUpper) Then
    '        Return True
    '    End If
    '    Return False
    'End Function

    ' ''' <summary>
    ' ''' Return true if user belongs to mail group SALES.IAG.USA(AAC CP or KA sales)
    ' ''' </summary>
    ' ''' <param name="GA"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function IsInGroupSalesIagUsa(ByVal GA As ArrayList) As Boolean
    '    If GA.Contains("SALES.IAG.USA") Then
    '        Return True
    '    End If
    '    Return False
    'End Function


    Public Function GetVisibleRBUByGropArray(ByVal DocReg As COMM.Fixer.eDocReg, ByVal userid As String) As ArrayList
        If (DocReg And COMM.Fixer.eDocReg.AEU) = DocReg Then

            Dim _DRBU As New ArrayList()

            'Frank 2017-04-11:ADlog team can only make quote for ADLOG customer
            If userid.EndsWith("advantech-dlog.com", StringComparison.InvariantCultureIgnoreCase) Then
                _DRBU.Add("ADLOG")
                Return _DRBU
            End If

            _DRBU.Add("ADL")
            _DRBU.Add("AEE")
            _DRBU.Add("ABN")
            _DRBU.Add("ANR")
            _DRBU.Add("AUK")
            _DRBU.Add("APL")
            _DRBU.Add("AFR")
            _DRBU.Add("AIB")
            _DRBU.Add("AIT")
            _DRBU.Add("AEU")
            _DRBU.Add("AINNOCORE")
            _DRBU.Add("AMEA-MEDICAL")
            _DRBU.Add("AGPEG")
            _DRBU.Add("ADLOG")

            'Ryan 20170817 Add B+B RBU for all AEU users
            '_DRBU.Add("ABB")
            'Frank 2017
            _DRBU.Add("ABBEU")
            Return _DRBU
        End If
        If (DocReg And COMM.Fixer.eDocReg.AUS) = DocReg Then
            Dim _DRBU As New ArrayList()

            'ICC 2015/5/11 Only AENC(RBU) can be chosen for AENC group
            If DocReg = COMM.Fixer.eDocReg.AENC Then
                _DRBU.Add("AENC")
                Return _DRBU
            End If
            _DRBU.Add("AAC")
            _DRBU.Add("AENC")
            _DRBU.Add("ANADMF")
            _DRBU.Add("ANA")
            _DRBU.Add("HQDC")
            _DRBU.Add("AMX")
            _DRBU.Add("ALA")
            _DRBU.Add("AiSA")
            Return _DRBU
        End If
        If (DocReg And COMM.Fixer.eDocReg.ATW) = DocReg Then
            Dim _DRBU As New ArrayList()
            _DRBU.Add("ATW")
            _DRBU.Add("ACL")
            Return _DRBU
        End If
        If (DocReg And COMM.Fixer.eDocReg.CAPS) = DocReg Then
            Dim _DRBU As New ArrayList()
            _DRBU.Add("ATW")
            _DRBU.Add("ACL")
            Return _DRBU
        End If
        If (DocReg And COMM.Fixer.eDocReg.AKR) = DocReg Then
            Dim _DRBU As New ArrayList()
            _DRBU.Add("AKR")
            Return _DRBU
        End If
        If (DocReg And COMM.Fixer.eDocReg.AJP) = DocReg Then
            Dim _DRBU As New ArrayList()
            _DRBU.Add("AJP")
            Return _DRBU
        End If
        If (DocReg And COMM.Fixer.eDocReg.HQDC) = DocReg Then
            Dim _DRBU As New ArrayList()
            '_DRBU.Add("HQDC")
            'Return _DRBU

            If userid.Equals("Julia.Wong@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then
                _DRBU.Add("HQDC")
                _DRBU.Add("ARU")
                _DRBU.Add("AIN")
                Return _DRBU
            End If

            Dim Str As String = String.Format("select distinct a.BU_NAME as RBU from SIEBEL_POSITION a inner join SIEBEL_DIV_HIERARCHY b on a.PRIMARY_POSITION_DIVISION_ROW_ID=b.ROW_ID   WHERE (a.EMAIL_ADDR = '{0}')", userid)
            Dim _dt As DataTable = New DataTable
            _dt = dbUtil.dbGetDataTable("MY", Str)
            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                For Each _row As DataRow In _dt.Rows
                    _DRBU.Add(_row.Item("RBU"))
                Next
            Else
                _DRBU.Add("HQDC")
            End If

            Return _DRBU

        End If
        If (DocReg And COMM.Fixer.eDocReg.AAU) = DocReg Then
            Dim _DRBU As New ArrayList()
            _DRBU.Add("AAU")
            Return _DRBU
        End If
        If (DocReg And COMM.Fixer.eDocReg.ACN) = DocReg Then
            Dim _DRBU As New ArrayList()
            _DRBU.Add("ACN")
            _DRBU.Add("ABJ")
            _DRBU.Add("ASH")
            Return _DRBU
        End If
        If (DocReg And COMM.Fixer.eDocReg.ASG) = DocReg Then
            Dim _DRBU As New ArrayList()
            _DRBU.Add("ASG")
            Return _DRBU
        End If
        If (DocReg And COMM.Fixer.eDocReg.ABR) = DocReg Then
            Dim _DRBU As New ArrayList()
            _DRBU.Add("ABR")
            Return _DRBU
        End If
        If (DocReg And COMM.Fixer.eDocReg.AAU) = DocReg Then
            Dim _DRBU As New ArrayList()
            _DRBU.Add("AAU")
            Return _DRBU
        End If
        Return Nothing
    End Function




End Class

Public NotInheritable Class RoleCheck
    Public Shared Function IsInternalUser(ByVal User_Id As String) As Boolean
        Return COMM.Util.IsInternalUser(User_Id)
    End Function

    Public Shared Function isAdmin(ByVal userName As String) As Boolean
        If userName Is Nothing Then Return False
        If userName.ToLower.StartsWith("tc.chen", StringComparison.OrdinalIgnoreCase) Or
           userName.ToLower.StartsWith("frank.chung", StringComparison.OrdinalIgnoreCase) Or
           userName.ToLower.StartsWith("yl.huang", StringComparison.OrdinalIgnoreCase) Or
           userName.ToLower.StartsWith("jay.lee", StringComparison.OrdinalIgnoreCase) Or
           userName.ToLower.StartsWith("ic.chen", StringComparison.OrdinalIgnoreCase) Then
            Return True
        End If
        Return False
    End Function
End Class
Public NotInheritable Class VG
    'Public Const ANA As String = "VG_ANA"
    Public Const AEU As String = "VG_AEU"
    Public Const AMX As String = "VG_AMX"
    Public Const AJP As String = "VG_AJP"
    Public Const AKR As String = "VG_AKR"
    Public Const ACN As String = "VG_ACN"
    Public Const AFC As String = "VG_AFC"
    Public Const ATW As String = "VG_ATW"
    Public Const HQDC As String = "VG_HQDC"
    Public Const ABR As String = "VG_ABR"
    Public Const AAU As String = "VG_AAU"
    Public Const CAPS As String = "VG_CAPS"
End Class

