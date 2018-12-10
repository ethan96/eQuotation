Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports Advantech.Myadvantech.DataAccess

Public Class SiebelTools

    Private Shared Function GetPrimarySMTPandAliasListFromADDRESSBOOKALIASByEmail(ByVal _Email As String) As ArrayList

        'Dim cmd As New SqlClient.SqlCommand("Select aa.Email as AliasEmail,ab.PrimarySmtpAddress as PrimaryEmail from [ADVANTECH_ADDRESSBOOK_ALIAS] aa left join [ADVANTECH_ADDRESSBOOK] ab on aa.ID=ab.ID" + _
        '                                    " where aa.ID in " + _
        '                                    "  ( Select b.id from [ADVANTECH_ADDRESSBOOK] a inner join [ADVANTECH_ADDRESSBOOK_ALIAS] b on a.ID=b.ID" + _
        '                                    " where b.Email= @email) and aa.Email<>ab.PrimarySmtpAddress", _
        '                                    New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))

        Dim cmd As New SqlClient.SqlCommand("Select aa.ALIAS_EMAIL as AliasEmail,ab.PrimarySmtpAddress as PrimaryEmail from [AD_MEMBER_ALIAS] aa left join [AD_MEMBER] ab on aa.EMAIL=ab.PrimarySmtpAddress" + _
                                            " where aa.EMAIL in " + _
                                            "  ( Select b.EMAIL from [AD_MEMBER] a inner join [AD_MEMBER_ALIAS] b on a.PrimarySmtpAddress=b.EMAIL" + _
                                            " where b.ALIAS_EMAIL= @email) and aa.ALIAS_EMAIL<>ab.PrimarySmtpAddress", _
                                            New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))



        cmd.Parameters.AddWithValue("email", _Email)
        cmd.Connection.Open()

        Dim _dr As SqlDataReader = cmd.ExecuteReader

        Dim _returnarr As New ArrayList
        If _dr.HasRows Then
            Dim _IsReadPrimarySMTPEmail As Boolean = False, _emailitem As String = String.Empty
            While _dr.Read
                If Not _IsReadPrimarySMTPEmail Then
                    _emailitem = _dr("PrimaryEmail").ToString.ToLower
                    If Util.isEmail(_emailitem) AndAlso Not _returnarr.Contains(_emailitem) Then
                        _returnarr.Add(_emailitem)
                        _IsReadPrimarySMTPEmail = True
                    End If
                End If
                _emailitem = _dr("AliasEmail").ToString.ToLower
                If Util.isEmail(_emailitem) AndAlso Not _returnarr.Contains(_emailitem) Then
                    _returnarr.Add(_emailitem)
                End If
            End While
        End If
        _dr.Close()
        cmd.Connection.Close()

        Return _returnarr

    End Function

    Public Shared Function TransferOpportunityOwnerEmail(ByVal _Email As String) As String

        If String.IsNullOrEmpty(_Email) Then Return ""

        If String.IsNullOrEmpty(SiebelTools.getSiebelLoginNameByEmail(_Email)) Then
            Dim _emaillist As ArrayList = GetPrimarySMTPandAliasListFromADDRESSBOOKALIASByEmail(_Email)
            If _emaillist.Count = 0 Then Return ""
            'Scanning primary smtp and alias email list
            For Each _emailitem As String In _emaillist
                If _Email.Equals(_emailitem, StringComparison.InvariantCultureIgnoreCase) Then Continue For
                If String.IsNullOrEmpty(SiebelTools.getSiebelLoginNameByEmail(_emailitem)) Then Continue For
                'Go to here means primary SMTP or alias mail address can be linked to email of Siebel login account
                Return _emailitem
            Next
        Else
            'Go to here means the quotation owner or creator's email can be linked to email of Siebel login account
            Return _Email
        End If
        'Go to here means all the quotation owner\creator's email, PrimarySmtpAddress and alias email 
        'can not be linked to email of Siebel login account
        Return ""

    End Function

    Public Shared Function CreateOpportunity(ByVal quoteId As String, ByVal strAdminEmail As String, ByVal strContactAccountId As String, _
                                        ByVal strContactId As String, ByVal strOptyName As String, ByVal strSALES_STAGE As String, _
                                        ByVal RBU As String, ByVal strOptyComment As String, ByVal strSrcId As String, Optional OptyCurrency As String = "USD") As String


        Dim eCovWs As New eCovWS.WSSiebel, emp As New eCovWS.EMPLOYEE, opty As New eCovWS.OPPTY
        emp.USER_ID = ConfigurationManager.AppSettings("CRMHQId") : emp.PASSWORD = ConfigurationManager.AppSettings("CRMHQPwd")

        'Frank 2012/10/11: Confire this with TC:「For the time being, yes, please change it to ANADMF when it is AMX, thanks.」
        If RBU.Equals("AMX", StringComparison.InvariantCultureIgnoreCase) Then RBU = "ANADMF"


        With opty
            .ACC_ROW_ID = strContactAccountId : .CLOSE_DATE = DateAdd(DateInterval.Month, 1, Now)
            '.CURRENCY_CODE = "USD" : .DESP = strOptyComment : .ORG = strRBU : .OWNER_EMAIL = strAdminEmail
            .CURRENCY_CODE = OptyCurrency : .DESP = strOptyComment : .ORG = RBU : .OWNER_EMAIL = strAdminEmail
            .PROJ_NAME = strOptyName : .SALES_METHOD = "Funnel Sales Methodology" : .SALES_STAGE = strSALES_STAGE
            ' .REVENUE = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, quoteId, Pivot.CurrentProfile.getCurrOrg).GetListAll(COMM.Fixer.eDocType.EQ).getTotalAmount
            If Role.IsTWAonlineSales() Then
                .REVENUE = MyQuoteX.GetATWTotalAmount(quoteId)
            Else
                .REVENUE = MyQuoteX.GetTotalPrice(quoteId)
            End If
            If String.IsNullOrEmpty(strContactId) = False Then .CON_ROW_ID = strContactId
            If String.IsNullOrEmpty(strSrcId) = False Then .SRC_ID = strSrcId
        End With
        Dim res As eCovWS.RESULT = Nothing
        Try
            'Throw New Exception("Create opportunity failed. Test by Frank.")
            eCovWs.Timeout = 10000
            res = eCovWs.AddOppty(emp, opty)

            'Frank 2012/08/27:Send opportunity created result to Myadvantech team
            Dim MessageStr As String = ""
            Dim MailSubject As String = "Creating New Opportunity by eQuotation"
            If Not IsNothing(res) Then
                If String.IsNullOrEmpty(res.ERR_MSG) Then
                    MailSubject &= "(Success)"
                Else
                    MailSubject &= "(Failed!)"
                End If
                MessageStr &= "Return OptyRowId:" & res.ROW_ID & "<br>"
                MessageStr &= "Return Error Message:<font color='red'>" & res.ERR_MSG & "</font><br>"
                MessageStr &= "================================" & "<br>"
                MessageStr &= "Call Method:eCovWs.AddOppty" & "<br>"
                MessageStr &= "OPPTY.PROJ_NAME =" & opty.PROJ_NAME & "<br>"
                MessageStr &= "OPPTY.SALES_STAGE =" & opty.SALES_STAGE & "<br>"
                MessageStr &= "OPPTY.ACC_ROW_ID =" & opty.ACC_ROW_ID & "<br>"
                MessageStr &= "OPPTY.CLOSE_DATE =" & opty.CLOSE_DATE & "<br>"
                MessageStr &= "OPPTY.CURRENCY_CODE =" & opty.CURRENCY_CODE & "<br>"
                MessageStr &= "OPPTY.DESP =" & opty.DESP & "<br>"
                MessageStr &= "OPPTY.ORG =" & opty.ORG & "<br>"
                MessageStr &= "OPPTY.OWNER_EMAIL =" & opty.OWNER_EMAIL & "<br>"
                MessageStr &= "OPPTY.SALES_METHOD =" & opty.SALES_METHOD & "<br>"
                MessageStr &= "OPPTY.CON_ROW_ID =" & opty.CON_ROW_ID & "<br>"
                MessageStr &= "OPPTY.SRC_ID =" & opty.SRC_ID & "<br>"
                Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", MailSubject, "", MessageStr, "")
                'Util.SendEmail("eBusiness.AEU@advantech.eu", "frank.chung@advantech.com.tw", "", "", MailSubject, "", MessageStr, "")

                Return res.ROW_ID

            End If
        Catch ex As Exception
            Dim _siebellogAPT As New SiebelDSTableAdapters.SiebelWSFailedLogTableAdapter
            Dim _Parameter As String = strContactAccountId & "###" & DateAdd(DateInterval.Month, 1, Now)
            _Parameter &= "###" & OptyCurrency & "###" & strOptyComment & "###" & RBU & "###" & strAdminEmail & "###" & strOptyName
            _Parameter &= "###" & "Funnel Sales Methodology" & "###" & strSALES_STAGE & "###"

            _siebellogAPT.Insert(quoteId, "CreateOpportunity", _Parameter, strAdminEmail, ex.Message, Now, Now, False)
            _siebellogAPT = Nothing
        End Try

        Return Nothing

    End Function

    Public Shared Function GetSalesOwnerRBU(ByVal AdminEmail As String, ByRef AdminRBU As String) As Boolean
        Dim cmd As New SqlCommand( _
                   "select top 1 OrgId from SIEBEL_CONTACT where EMAIL_ADDRESS =@ADMINMAIL and OrgId is not null and OrgId<>'' order by ROW_ID ", _
                   New SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString))
        cmd.Parameters.AddWithValue("ADMINMAIL", AdminEmail)
        cmd.Connection.Open()
        Dim obj As Object = cmd.ExecuteScalar()
        cmd.Connection.Close()
        If obj IsNot Nothing Then
            AdminRBU = obj.ToString() : Return True
        End If
        Return False
    End Function


    Public Shared Function GetContactRowIdByEmail(ByVal Email As String, ByRef ContactId As String, ByRef AccountId As String) As Boolean
        Dim apt As New SqlDataAdapter( _
            " select top 1 A.ROW_ID, IsNull(B.ROW_ID,'') as ACCOUNT_ROW_ID " + _
            " from S_CONTACT A LEFT JOIN S_ORG_EXT B ON A.PR_DEPT_OU_ID = B.PAR_ROW_ID " + _
            " where lower(A.EMAIL_ADDR)=@CEMAIL order by A.ROW_ID", _
            New SqlConnection(ConfigurationManager.ConnectionStrings("CRM").ConnectionString))
        apt.SelectCommand.Parameters.AddWithValue("CEMAIL", LCase(Email))
        Dim dt As New DataTable
        apt.Fill(dt)
        apt.SelectCommand.Connection.Close()
        If dt.Rows.Count = 1 Then
            ContactId = dt.Rows(0).Item("ROW_ID") : AccountId = dt.Rows(0).Item("ACCOUNT_ROW_ID") : Return True
        Else
            Return False
        End If
    End Function

    Shared Function IsCanRepeatOrder(ByVal Row_id As String) As String
        If Role.IsUsaUser() Then
            Return False
        End If
        Dim str As String = String.Format("SELECT COUNT(*) FROM S_ORG_EXT WHERE ROW_ID='{0}' AND CUST_STAT_CD IN ('06-Key Account','06G-Golden Key Account(ACN)','04-Premier Key Account')", Row_id)
        Dim n As Integer = tbOPBase.dbExecuteScalar("CRM", str)
        If n > 0 Then
            Return True
        End If
        Return False
    End Function
    'Shared Function GETSHIPTO(ByVal PERPID As String, ByVal ERPID As String, ByVal NAME As String, ByVal ORG As String) As DataTable
    '    Dim DT As New DataTable
    '    DT = SAPTools.SearchAllSAPCompanySoldBillShipTo(ERPID, ORG, NAME, "", "", "", "")
    '    Return DT
    'End Function
    Shared Function getShipSiebel(ByVal rowid As String) As String
        Dim str As String = " SELECT " & _
      " (ISNULL(T16.STATE,'') + char(10) +" & _
      " ISNULL(T16.ADDR,'')+ char(10) +" & _
      " ISNULL(T16.ADDR_LINE_2,'')+ char(10) +" & _
      " ISNULL(T16.ZIPCODE,'')+ char(10) +" & _
      " ISNULL(T16.ADDR_NAME,'')+ char(10) +" & _
      " ISNULL(T16.CITY,'')+ char(10) +" & _
      " ISNULL(T16.COUNTRY,'')) as ad" & _
      " FROM S_ORG_EXT T2" & _
      " LEFT OUTER JOIN dbo.S_ADDR_ORG T16 ON T2.PR_SHIP_ADDR_ID = T16.ROW_ID" & _
      " WHERE T2.ROW_ID='" & rowid & "'"
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("CRM", str)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("AD")
        End If
        Return ""
    End Function

    Shared Function getBillSiebel(ByVal rowid As String) As String
        Dim str As String = " SELECT " & _
              " ISNULL(T16.STATE,'') + char(10) + " & _
              " ISNULL(T16.ADDR,'') + char(10) + " & _
              " ISNULL(T16.ADDR_LINE_2,'') + char(10) + " & _
              " ISNULL(T16.ZIPCODE,'') + char(10) + " & _
              " ISNULL(T16.ADDR_NAME,'') + char(10) + " & _
              " ISNULL(T16.CITY,'') + char(10) + " & _
              " ISNULL(T16.COUNTRY,'') AS AD " & _
        " FROM S_ORG_EXT T2 " & _
        " LEFT OUTER JOIN dbo.S_ADDR_ORG T16 ON T2.PR_BL_ADDR_ID = T16.ROW_ID " & _
        " WHERE T2.ROW_ID='" & rowid & "'"
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("CRM", str)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("AD")
        End If
        Return ""
    End Function


    Shared Function getSBListSiebel(ByVal rowid As String) As String
        Dim str As String = " SELECT " & _
              " ISNULL(b.STATE,'') + char(10) + " & _
              " ISNULL(b.ADDR,'') + char(10) + " & _
              " ISNULL(b.ADDR_LINE_2,'') + char(10) + " & _
              " ISNULL(b.ZIPCODE,'') + char(10) + " & _
              " ISNULL(b.ADDR_NAME,'') + char(10) + " & _
              " ISNULL(b.CITY,'') + char(10) + " & _
              " ISNULL(b.COUNTRY,'') AS AD " & _
              " from S_ADDR_ORG b " & _
              " where b.ROW_ID='" & rowid & "'"

        Return str
    End Function

    Shared Function Get_PRIMARY_SALES_EMAIL_ByAccountROWID(ByVal AccountRowid As String) As String
        Dim sb As New StringBuilder
        sb.AppendFormat(" SELECT TOP 1 ISNULL(PRIMARY_SALES_EMAIL,'') AS OwnerEmail  FROM  SIEBEL_ACCOUNT where ROW_ID='{0}' ", AccountRowid)
        sb.AppendFormat(" AND PRIMARY_SALES_EMAIL <> '' AND PRIMARY_SALES_EMAIL IS not NULL  ")
        Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", sb.ToString)
        If dt.Rows.Count = 1 Then
            Return dt.Rows(0).Item("OwnerEmail")
        End If
        Return ""
    End Function
    Shared Function GET_Siebel_Account_List(ByVal Name As String, ByVal RBU As String, ByVal erpid As String,
                                            ByVal country As String, ByVal location As String, ByVal state As String,
                                            ByVal province As String, ByVal status As String, ByVal address1 As String, ByVal ZipCode As String, ByVal City As String, ByVal PriSales As String) As String
        Dim str As String = " select TOP 100 a.ROW_ID AS ROW_ID, a.NAME as COMPANYNAME, IsNull(b.ATTRIB_05,'') as ERPID, " &
                            " IsNull(d.COUNTRY,'') as COUNTRY, IsNull(d.CITY,'') as CITY, Isnull(a.LOC,'') as LOCATION, " &
                            " IsNull(c.NAME, '') as RBU, IsNull(d.STATE,'') as STATE,IsNull(d.PROVINCE,'') as PROVINCE, " +
                            " IsNull(a.CUST_STAT_CD,'') as STATUS ,IsNull(d.ADDR,'') as ADDRESS, IsNull(d.ZIPCODE,'') as ZIPCODE, IsNull(d.ADDR_LINE_2,'') as ADDRESS2," &
                            " (SELECT TOP 1 isnull(EMAIL_ADDR,'') FROM S_CONTACT WHERE ROW_ID=(SELECT TOP 1 PR_EMP_ID from S_POSTN where ROW_ID = (SELECT TOP 1 PR_POSTN_ID FROM S_ORG_EXT WHERE ROW_ID=a.ROW_ID)) and EMAIL_ADDR is not null) AS priSales " &
                            " from S_ORG_EXT a left join S_ORG_EXT_X b on a.ROW_ID=b.ROW_ID " &
                            " left join S_PARTY c on a.BU_ID=c.ROW_ID " &
                            " left join S_ADDR_PER d on a.PR_ADDR_ID=d.ROW_ID where 1=1 "
        If Not String.IsNullOrEmpty(Name) Then
            str += String.Format(" and Upper(ISNULL(a.NAME,'')) like Upper(N'%{0}%') ", Name.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(erpid) Then
            str += String.Format(" and Upper(ISNULL(b.ATTRIB_05,'')) like Upper(N'%{0}%') ", erpid.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(RBU) Then
            Dim SRBU() As String = RBU.Split(",")
            If SRBU.Length > 1 Then
                Dim temp As String = ""
                For Each r As String In SRBU
                    temp = temp + "'" + r.ToUpper + "',"
                Next
                temp = temp.Trim(",")
                str += String.Format(" and Upper(c.NAME) in ({0}) ", temp)

            Else
                str += String.Format(" and Upper(c.NAME) = Upper(N'{0}') ", RBU.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
            End If
            For Each R As String In SRBU

            Next

        End If
        If Not String.IsNullOrEmpty(country) Then
            str += String.Format(" and Upper(d.COUNTRY) like Upper(N'%{0}%') ", country.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(location) Then
            str += String.Format(" and Upper(a.LOC) like Upper(N'%{0}%') ", location.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(state) AndAlso Not String.IsNullOrEmpty(province) Then
            str += String.Format(" and (Upper(d.STATE) like Upper(N'%{0}%') or Upper(d.PROVINCE) like Upper(N'%{1}%'))", state.Trim().ToUpper().Replace("'", "''").Replace("*", "%"), province.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        Else
            If Not String.IsNullOrEmpty(state) Then
                str += String.Format(" and Upper(d.STATE) like Upper(N'%{0}%') ", state.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
            End If

            If Not String.IsNullOrEmpty(province) Then
                str += String.Format(" and Upper(d.PROVINCE) like Upper(N'%{0}%') ", province.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
            End If
        End If
        If Not String.IsNullOrEmpty(status) Then
            str += String.Format(" and Upper(a.CUST_STAT_CD) = Upper(N'{0}') ", status.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If

        If Not String.IsNullOrEmpty(address1) Then
            str += String.Format(" and Upper(d.ADDR) like Upper(N'%{0}%') ", address1.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If

        If Not String.IsNullOrEmpty(ZipCode) Then
            str += String.Format(" and Upper(d.ZIPCODE) like Upper(N'%{0}%') ", ZipCode.ToUpper())
        End If
        If Not String.IsNullOrEmpty(City) Then
            str += String.Format(" and Upper(d.CITY) like Upper(N'%{0}%') ", City.ToUpper())
        End If

        'Frank 2013/10/09 Jay no need to search account who does not belong to AUS and AMX
        'If Not Pivot.CurrentProfile.Role = COMM.Fixer.eRole.isAdmin And _
        '    Not Pivot.CurrentProfile.UserId.ToString().Equals("jay.lee@advantech.com", StringComparison.OrdinalIgnoreCase) Then

        If Not Pivot.CurrentProfile.Role = COMM.Fixer.eRole.isAdmin Then
            Dim arrRBU As ArrayList = Pivot.CurrentProfile.VisibleRBU

            'Ryan 20170419 Special Logic for intercon
            If ((Not String.IsNullOrEmpty(Name) AndAlso Name.Equals("Alitek", StringComparison.OrdinalIgnoreCase)) OrElse
                Not String.IsNullOrEmpty(erpid) AndAlso erpid.Equals("ETRA002", StringComparison.OrdinalIgnoreCase)) AndAlso
                    (Pivot.CurrentProfile.UserId.Equals("Freda.Yen@advantech.com.tw", StringComparison.OrdinalIgnoreCase) OrElse
                    Pivot.CurrentProfile.UserId.Equals("Sabrina.Lin@advantech.com.tw", StringComparison.OrdinalIgnoreCase) OrElse
                    Pivot.CurrentProfile.UserId.Equals("Vickie.Lee@advantech.com.tw", StringComparison.OrdinalIgnoreCase)) Then
                arrRBU.Add("ATR")
            End If
            'End Ryan 20170419

            'Part I, conditions by RBU
            Dim str1 As String = String.Empty
            If arrRBU.Count > 0 Then
                Dim strRBU(arrRBU.Count - 1) As String
                For i As Integer = 0 To arrRBU.Count - 1
                    strRBU(i) = "'" + Replace(UCase(arrRBU.Item(i)), "'", "''") + "'"
                Next
                Dim strJoinedRBU As String = String.Join(",", strRBU)
                str1 = " c.NAME is not null and Upper(c.NAME) in (" + strJoinedRBU + ") "
            End If

            'Part II, conditions by ERPID
            Dim str2 As String = String.Empty
            Dim SAPcount As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select count(*) from EAI_IDMAP a inner join SAP_COMPANY_EMPLOYEE b on a.id_sap = b.SALES_CODE where a.id_email ='{0}' and b.PARTNER_FUNCTION = 'VE' and b.COMPANY_ID = '{1}'", Pivot.CurrentProfile.UserId, erpid))
            If SAPcount IsNot Nothing AndAlso Integer.Parse(SAPcount) > 0 Then
                str2 = " 1=1 "
            Else
                str2 = " 1=0 "
            End If

            'Part III, combine these two strings            
            If Not String.IsNullOrEmpty(str1) Then
                str += String.Format(" and (({0}) or {1})", str1, str2)
            Else
                str += String.Format(" and ({0}) ", str2)
            End If
        End If

        'If Role.IsACNSales Then

        '    Dim LST As String = getAccountOwnerByUser(Pivot.CurrentProfile.UserId)
        '    If LST.Length > 0 Then
        '        str += String.Format(" and a.ROW_ID in ({0}) ", LST)
        '    Else
        '        str += String.Format(" and 1<>1 ")
        '    End If
        'End If
        If PriSales <> "" Then
            str += " AND a.PR_POSTN_ID IN (SELECT ROW_ID from S_POSTN WHERE S_POSTN.PR_EMP_ID IN(SELECT ROW_ID FROM S_CONTACT WHERE Upper(S_CONTACT.EMAIL_ADDR) like '%" & PriSales.ToUpper & "%')) "
        End If
        str += " order by a.ROW_ID "

        'Util.SendEmail("eBusiness.AEU@advantech.eu", "nada.liu@advantech.com.cn", "", "", "eQuotation Error Massage by ", "", str, "")
        Return str
    End Function

    Shared Function GET_Siebel_Account_ListForAiST(ByVal Name As String, ByVal RBU As String, ByVal erpid As String,
                                            ByVal country As String, ByVal location As String, ByVal state As String,
                                            ByVal province As String, ByVal status As String, ByVal address1 As String, ByVal ZipCode As String, ByVal City As String, ByVal PriSales As String) As String
        Dim str As String = " select TOP 100 a.ROW_ID AS ROW_ID, a.NAME as COMPANYNAME, IsNull(b.ATTRIB_05,'') as ERPID, " &
                            " IsNull(d.COUNTRY,'') as COUNTRY, IsNull(d.CITY,'') as CITY, Isnull(a.LOC,'') as LOCATION, " &
                            " IsNull(c.NAME, '') as RBU, IsNull(d.STATE,'') as STATE,IsNull(d.PROVINCE,'') as PROVINCE, " +
                            " IsNull(a.CUST_STAT_CD,'') as STATUS ,IsNull(d.ADDR,'') as ADDRESS, IsNull(d.ZIPCODE,'') as ZIPCODE, IsNull(d.ADDR_LINE_2,'') as ADDRESS2," &
                            " (SELECT TOP 1 isnull(EMAIL_ADDR,'') FROM S_CONTACT WHERE ROW_ID=(SELECT TOP 1 PR_EMP_ID from S_POSTN where ROW_ID = (SELECT TOP 1 PR_POSTN_ID FROM S_ORG_EXT WHERE ROW_ID=a.ROW_ID)) and EMAIL_ADDR is not null) AS priSales " &
                            " from S_ORG_EXT a left join S_ORG_EXT_X b on a.ROW_ID=b.ROW_ID " &
                            " left join S_PARTY c on a.BU_ID=c.ROW_ID " &
                            " left join S_ADDR_PER d on a.PR_ADDR_ID=d.ROW_ID where 1=1 "
        If Not String.IsNullOrEmpty(Name) Then
            str += String.Format(" and Upper(ISNULL(a.NAME,'')) like Upper(N'%{0}%') ", Name.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(erpid) Then
            str += String.Format(" and Upper(ISNULL(b.ATTRIB_05,'')) like Upper(N'%{0}%') ", erpid.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(RBU) Then
            Dim SRBU() As String = RBU.Split(",")
            If SRBU.Length > 1 Then
                Dim temp As String = ""
                For Each r As String In SRBU
                    temp = temp + "'" + r.ToUpper + "',"
                Next
                temp = temp.Trim(",")
                str += String.Format(" and Upper(c.NAME) in ({0}) ", temp)

            Else
                str += String.Format(" and Upper(c.NAME) = Upper(N'{0}') ", RBU.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
            End If
            For Each R As String In SRBU

            Next

        End If
        If Not String.IsNullOrEmpty(country) Then
            str += String.Format(" and Upper(d.COUNTRY) like Upper(N'%{0}%') ", country.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(location) Then
            str += String.Format(" and Upper(a.LOC) like Upper(N'%{0}%') ", location.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(state) AndAlso Not String.IsNullOrEmpty(province) Then
            str += String.Format(" and (Upper(d.STATE) like Upper(N'%{0}%') or Upper(d.PROVINCE) like Upper(N'%{1}%'))", state.Trim().ToUpper().Replace("'", "''").Replace("*", "%"), province.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        Else
            If Not String.IsNullOrEmpty(state) Then
                str += String.Format(" and Upper(d.STATE) like Upper(N'%{0}%') ", state.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
            End If

            If Not String.IsNullOrEmpty(province) Then
                str += String.Format(" and Upper(d.PROVINCE) like Upper(N'%{0}%') ", province.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
            End If
        End If
        If Not String.IsNullOrEmpty(status) Then
            str += String.Format(" and Upper(a.CUST_STAT_CD) = Upper(N'{0}') ", status.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If

        If Not String.IsNullOrEmpty(address1) Then
            str += String.Format(" and Upper(d.ADDR) like Upper(N'%{0}%') ", address1.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If

        If Not String.IsNullOrEmpty(ZipCode) Then
            str += String.Format(" and Upper(d.ZIPCODE) like Upper(N'%{0}%') ", ZipCode.ToUpper())
        End If
        If Not String.IsNullOrEmpty(City) Then
            str += String.Format(" and Upper(d.CITY) like Upper(N'%{0}%') ", City.ToUpper())
        End If

        'Frank 2013/10/09 Jay no need to search account who does not belong to AUS and AMX
        'If Not Pivot.CurrentProfile.Role = COMM.Fixer.eRole.isAdmin And _
        '    Not Pivot.CurrentProfile.UserId.ToString().Equals("jay.lee@advantech.com", StringComparison.OrdinalIgnoreCase) Then

        If Not Pivot.CurrentProfile.Role = COMM.Fixer.eRole.isAdmin Then
            Dim arrRBU As ArrayList = Pivot.CurrentProfile.VisibleRBU

            'Part I, conditions by RBU
            Dim str1 As String = " c.NAME is not null and Upper(c.NAME) in ('AIST') "

            'Part II, conditions by ERPID
            'Dim str2 As String = String.Empty
            'Dim SAPcount As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select count(*) from EAI_IDMAP a inner join SAP_COMPANY_EMPLOYEE b on a.id_sap = b.SALES_CODE where a.id_email ='{0}' and b.PARTNER_FUNCTION = 'VE' and b.COMPANY_ID = '{1}'", Pivot.CurrentProfile.UserId, erpid))
            'If SAPcount IsNot Nothing AndAlso Integer.Parse(SAPcount) > 0 Then
            '    str2 = " 1=1 "
            'Else
            '    str2 = " 1=0 "
            'End If

            'Part III, conditions by co-own
            Dim dtCoOwn As DataTable = tbOPBase.dbGetDataTable("MY", "select a.ACCOUNT_ROW_ID from SIEBEL_ACCOUNT_RBU a inner join SIEBEL_ACCOUNT b on a.ACCOUNT_ROW_ID = b.ROW_ID where a.RBU = 'AIST' and b.RBU = 'ATW'")
            Dim str3 As String = String.Empty
            If dtCoOwn IsNot Nothing AndAlso dtCoOwn.Rows.Count > 0 Then
                Dim strAccId(dtCoOwn.Rows.Count - 1) As String
                For i As Integer = 0 To dtCoOwn.Rows.Count - 1
                    strAccId(i) = "'" + Replace(UCase(dtCoOwn.Rows(i)("ACCOUNT_ROW_ID").ToString), "'", "''") + "'"
                Next
                Dim strJoinedAccId As String = String.Join(",", strAccId)
                str3 = " a.ROW_ID in (" + strJoinedAccId + ") "
            End If

            'Part IV, combine these two strings            
            'If Not String.IsNullOrEmpty(str1) Then
            '    str += String.Format(" and (({0}) or {1}) and {2}", str1, str2, str3)
            'Else
            '    str += String.Format(" and ({0}) and {1} ", str2)
            'End If

            If Not String.IsNullOrEmpty(str3) Then
                str += String.Format(" and (({0}) or ({1}))", str1, str3)
            Else
                str += String.Format(" and ({0}) ", str1)
            End If
        End If

        'If Role.IsACNSales Then

        '    Dim LST As String = getAccountOwnerByUser(Pivot.CurrentProfile.UserId)
        '    If LST.Length > 0 Then
        '        str += String.Format(" and a.ROW_ID in ({0}) ", LST)
        '    Else
        '        str += String.Format(" and 1<>1 ")
        '    End If
        'End If
        If PriSales <> "" Then
            str += " AND a.PR_POSTN_ID IN (SELECT ROW_ID from S_POSTN WHERE S_POSTN.PR_EMP_ID IN(SELECT ROW_ID FROM S_CONTACT WHERE Upper(S_CONTACT.EMAIL_ADDR) like '%" & PriSales.ToUpper & "%')) "
        End If
        str += " order by a.ROW_ID "

        'Util.SendEmail("eBusiness.AEU@advantech.eu", "nada.liu@advantech.com.cn", "", "", "eQuotation Error Massage by ", "", str, "")
        Return str
    End Function

    Shared Function GET_Siebel_Account_ListForABR(ByVal Name As String, ByVal RBU As String, ByVal erpid As String, _
                                             ByVal country As String, ByVal location As String, ByVal state As String, _
                                             ByVal province As String, ByVal status As String, ByVal address1 As String, _
                                             ByVal ZipCode As String, ByVal City As String, ByVal PriSales As String, _
                                             ByVal SAPTaxNumber1 As String) As String
        Dim str As String = " select TOP 100 a.ROW_ID AS ROW_ID, a.NAME as COMPANYNAME, IsNull(b.ATTRIB_05,'') as ERPID, " & _
                            " IsNull(d.COUNTRY,'') as COUNTRY, IsNull(d.CITY,'') as CITY, Isnull(a.LOC,'') as LOCATION, " & _
                            " IsNull(c.NAME, '') as RBU, IsNull(d.STATE,'') as STATE,IsNull(d.PROVINCE,'') as PROVINCE, " + _
                            " IsNull(a.CUST_STAT_CD,'') as STATUS ,IsNull(d.ADDR,'') as ADDRESS, IsNull(d.ZIPCODE,'') as ZIPCODE, IsNull(d.ADDR_LINE_2,'') as ADDRESS2," & _
                            " (SELECT TOP 1 isnull(EMAIL_ADDR,'') FROM S_CONTACT WHERE ROW_ID=(SELECT TOP 1 PR_EMP_ID from S_POSTN where ROW_ID = (SELECT TOP 1 PR_POSTN_ID FROM S_ORG_EXT WHERE ROW_ID=a.ROW_ID)) and EMAIL_ADDR is not null) AS priSales " & _
                            " from S_ORG_EXT a left join S_ORG_EXT_X b on a.ROW_ID=b.ROW_ID " & _
                            " left join S_PARTY c on a.BU_ID=c.ROW_ID " & _
                            " left join S_ADDR_PER d on a.PR_ADDR_ID=d.ROW_ID where 1=1 "
        If Not String.IsNullOrEmpty(Name) Then
            str += String.Format(" and Upper(ISNULL(a.NAME,'')) like Upper(N'%{0}%') ", Name.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(erpid) Then
            str += String.Format(" and Upper(ISNULL(b.ATTRIB_05,'')) like Upper(N'%{0}%') ", erpid.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(RBU) Then
            Dim SRBU() As String = RBU.Split(",")
            If SRBU.Length > 1 Then
                Dim temp As String = ""
                For Each r As String In SRBU
                    temp = temp + "'" + r.ToUpper + "',"
                Next
                temp = temp.Trim(",")
                str += String.Format(" and Upper(c.NAME) in ({0}) ", temp)

            Else
                str += String.Format(" and Upper(c.NAME) = Upper(N'{0}') ", RBU.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
            End If
            For Each R As String In SRBU

            Next

        End If
        If Not String.IsNullOrEmpty(country) Then
            str += String.Format(" and Upper(d.COUNTRY) like Upper(N'%{0}%') ", country.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(location) Then
            str += String.Format(" and Upper(a.LOC) like Upper(N'%{0}%') ", location.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(state) AndAlso Not String.IsNullOrEmpty(province) Then
            str += String.Format(" and (Upper(d.STATE) like Upper(N'%{0}%') or Upper(d.PROVINCE) like Upper(N'%{1}%'))", state.Trim().ToUpper().Replace("'", "''").Replace("*", "%"), province.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        Else
            If Not String.IsNullOrEmpty(state) Then
                str += String.Format(" and Upper(d.STATE) like Upper(N'%{0}%') ", state.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
            End If

            If Not String.IsNullOrEmpty(province) Then
                str += String.Format(" and Upper(d.PROVINCE) like Upper(N'%{0}%') ", province.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
            End If
        End If
        If Not String.IsNullOrEmpty(status) Then
            str += String.Format(" and Upper(a.CUST_STAT_CD) = Upper(N'{0}') ", status.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If

        If Not String.IsNullOrEmpty(address1) Then
            str += String.Format(" and Upper(d.ADDR) like Upper(N'%{0}%') ", address1.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If

        If Not String.IsNullOrEmpty(ZipCode) Then
            str += String.Format(" and Upper(d.ZIPCODE) like Upper(N'%{0}%') ", ZipCode.ToUpper())
        End If
        If Not String.IsNullOrEmpty(City) Then
            str += String.Format(" and Upper(d.CITY) like Upper(N'%{0}%') ", City.ToUpper())
        End If

        'Frank 2013/10/09 Jay no need to search account who does not belong to AUS and AMX
        'If Not Pivot.CurrentProfile.Role = COMM.Fixer.eRole.isAdmin And _
        '    Not Pivot.CurrentProfile.UserId.ToString().Equals("jay.lee@advantech.com", StringComparison.OrdinalIgnoreCase) Then

        If Not Pivot.CurrentProfile.Role = COMM.Fixer.eRole.isAdmin Then
            Dim arrRBU As ArrayList = Pivot.CurrentProfile.VisibleRBU

            If arrRBU.Count > 0 Then
                Dim strRBU(arrRBU.Count - 1) As String
                For i As Integer = 0 To arrRBU.Count - 1
                    strRBU(i) = "'" + Replace(UCase(arrRBU.Item(i)), "'", "''") + "'"
                Next
                Dim strJoinedRBU As String = String.Join(",", strRBU)
                str += " and c.NAME is not null and Upper(c.NAME) in (" + strJoinedRBU + ") "

            End If
        End If

        'If Role.IsACNSales Then

        '    Dim LST As String = getAccountOwnerByUser(Pivot.CurrentProfile.UserId)
        '    If LST.Length > 0 Then
        '        str += String.Format(" and a.ROW_ID in ({0}) ", LST)
        '    Else
        '        str += String.Format(" and 1<>1 ")
        '    End If
        'End If
        If PriSales <> "" Then
            str += " AND a.PR_POSTN_ID IN (SELECT ROW_ID from S_POSTN WHERE S_POSTN.PR_EMP_ID IN(SELECT ROW_ID FROM S_CONTACT WHERE Upper(S_CONTACT.EMAIL_ADDR) like '%" & PriSales.ToUpper & "%')) "
        End If

        If Not String.IsNullOrEmpty(SAPTaxNumber1) Then
            Dim _sqlTaxNumber As New StringBuilder
            _sqlTaxNumber.AppendLine(" Select KUNNR as ERPID,STCD1 as TaxNumber1 from saprdp.KNA1 ")
            _sqlTaxNumber.AppendLine(" Where MANDT=168 ")
            _sqlTaxNumber.AppendLine(" And STCD1='" & SAPTaxNumber1.Replace("'", "''") & "' ")
            Dim _dtTaxNumber1 As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", _sqlTaxNumber.ToString())
            Dim _strtax As String = String.Empty
            For Each _row As DataRow In _dtTaxNumber1.Rows
                _strtax += "'" & _row.Item("ERPID") & "',"
            Next
            If Not String.IsNullOrEmpty(_strtax) Then
                str += " AND b.ATTRIB_05 in (" & _strtax.TrimEnd(",", "") & ")"
            End If
        End If

        str += " order by a.ROW_ID "

        'Util.SendEmail("eBusiness.AEU@advantech.eu", "nada.liu@advantech.com.cn", "", "", "eQuotation Error Massage by ", "", str, "")
        Return str
    End Function


    Shared Function GET_Siebel_Account_List_ForAAC(ByVal Name As String, ByVal RBU As String, ByVal erpid As String, _
                                        ByVal country As String, ByVal location As String, ByVal state As String, _
                                        ByVal province As String, ByVal status As String, ByVal address1 As String, ByVal ZipCode As String, ByVal City As String, ByVal AccountOwner As String) As String
        Dim str As String = " select TOP 100 a.ROW_ID AS ROW_ID, a.NAME as COMPANYNAME, IsNull(b.ATTRIB_05,'') as ERPID, " & _
                            " IsNull(d.COUNTRY,'') as COUNTRY, IsNull(d.CITY,'') as CITY, Isnull(a.LOC,'') as LOCATION, " & _
                            " IsNull(c.NAME, '') as RBU, IsNull(d.STATE,'') as STATE,IsNull(d.PROVINCE,'') as PROVINCE, " + _
                            " IsNull(a.CUST_STAT_CD,'') as STATUS ,IsNull(d.ADDR,'') as ADDRESS, IsNull(d.ZIPCODE,'') as ZIPCODE, IsNull(d.ADDR_LINE_2,'') as ADDRESS2," & _
                            " (SELECT TOP 1 isnull(EMAIL_ADDR,'') FROM S_CONTACT WHERE ROW_ID=(SELECT TOP 1 PR_EMP_ID from S_POSTN where ROW_ID = (SELECT TOP 1 PR_POSTN_ID FROM S_ORG_EXT WHERE ROW_ID=a.ROW_ID)) and EMAIL_ADDR is not null) AS priSales " & _
                            " from S_ORG_EXT a left join S_ORG_EXT_X b on a.ROW_ID=b.ROW_ID " & _
                            " left join S_PARTY c on a.BU_ID=c.ROW_ID " & _
                            " left join S_ADDR_PER d on a.PR_ADDR_ID=d.ROW_ID where 1=1 "

        str += " and a.ROW_ID in ("
        str += " select distinct a.ROW_ID "
        str += " from S_ORG_EXT a left join S_ACCNT_POSTN b on a.ROW_ID=b.OU_EXT_ID "
        str += " left join S_POSTN c on b.POSITION_ID=c.ROW_ID left join S_CONTACT d on c.PR_EMP_ID=d.ROW_ID "
        str += " where Upper(d.EMAIL_ADDR)='" & AccountOwner.Trim().ToUpper & "') "

        If Not String.IsNullOrEmpty(Name) Then
            str += String.Format(" and Upper(ISNULL(a.NAME,'')) like Upper(N'%{0}%') ", Name.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(erpid) Then
            str += String.Format(" and Upper(ISNULL(b.ATTRIB_05,'')) like Upper(N'%{0}%') ", erpid.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(RBU) Then
            Dim SRBU() As String = RBU.Split(",")
            If SRBU.Length > 1 Then
                Dim temp As String = ""
                For Each r As String In SRBU
                    temp = temp + "'" + r.ToUpper + "',"
                Next
                temp = temp.Trim(",")
                str += String.Format(" and Upper(c.NAME) in ({0}) ", temp)

            Else
                str += String.Format(" and Upper(c.NAME) = Upper(N'{0}') ", RBU.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
            End If
            For Each R As String In SRBU

            Next

        End If
        If Not String.IsNullOrEmpty(country) Then
            str += String.Format(" and Upper(d.COUNTRY) like Upper(N'%{0}%') ", country.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(location) Then
            str += String.Format(" and Upper(a.LOC) like Upper(N'%{0}%') ", location.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If
        If Not String.IsNullOrEmpty(state) AndAlso Not String.IsNullOrEmpty(province) Then
            str += String.Format(" and (Upper(d.STATE) like Upper(N'%{0}%') or Upper(d.PROVINCE) like Upper(N'%{1}%'))", state.Trim().ToUpper().Replace("'", "''").Replace("*", "%"), province.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        Else
            If Not String.IsNullOrEmpty(state) Then
                str += String.Format(" and Upper(d.STATE) like Upper(N'%{0}%') ", state.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
            End If

            If Not String.IsNullOrEmpty(province) Then
                str += String.Format(" and Upper(d.PROVINCE) like Upper(N'%{0}%') ", province.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
            End If
        End If
        If Not String.IsNullOrEmpty(status) Then
            str += String.Format(" and Upper(a.CUST_STAT_CD) = Upper(N'{0}') ", status.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If

        If Not String.IsNullOrEmpty(address1) Then
            str += String.Format(" and Upper(d.ADDR) like Upper(N'%{0}%') ", address1.Trim().ToUpper().Replace("'", "''").Replace("*", "%"))
        End If

        If Not String.IsNullOrEmpty(ZipCode) Then
            str += String.Format(" and Upper(d.ZIPCODE) like Upper(N'%{0}%') ", ZipCode.ToUpper())
        End If
        If Not String.IsNullOrEmpty(City) Then
            str += String.Format(" and Upper(d.CITY) like Upper(N'%{0}%') ", City.ToUpper())
        End If

        'Frank 2013/10/09 Jay no need to search account who does not belong to AUS and AMX
        'If Not Pivot.CurrentProfile.Role = COMM.Fixer.eRole.isAdmin And _
        '    Not Pivot.CurrentProfile.UserId.ToString().Equals("jay.lee@advantech.com", StringComparison.OrdinalIgnoreCase) Then

        'If Not Pivot.CurrentProfile.Role = COMM.Fixer.eRole.isAdmin Then
        '    Dim arrRBU As ArrayList = Pivot.CurrentProfile.VisibleRBU

        '    If arrRBU.Count > 0 Then
        '        Dim strRBU(arrRBU.Count - 1) As String
        '        For i As Integer = 0 To arrRBU.Count - 1
        '            strRBU(i) = "'" + Replace(UCase(arrRBU.Item(i)), "'", "''") + "'"
        '        Next
        '        Dim strJoinedRBU As String = String.Join(",", strRBU)
        '        str += " and c.NAME is not null and Upper(c.NAME) in (" + strJoinedRBU + ") "

        '    End If
        'End If

        'If Role.IsACNSales Then

        '    Dim LST As String = getAccountOwnerByUser(Pivot.CurrentProfile.UserId)
        '    If LST.Length > 0 Then
        '        str += String.Format(" and a.ROW_ID in ({0}) ", LST)
        '    Else
        '        str += String.Format(" and 1<>1 ")
        '    End If
        'End If
        'If PriSales <> "" Then
        '    str += " AND a.PR_POSTN_ID IN (SELECT ROW_ID from S_POSTN WHERE S_POSTN.PR_EMP_ID IN(SELECT ROW_ID FROM S_CONTACT WHERE Upper(S_CONTACT.EMAIL_ADDR) like '%" & PriSales.ToUpper & "%')) "
        'End If
        str += " order by a.ROW_ID "

        'Util.SendEmail("eBusiness.AEU@advantech.eu", "nada.liu@advantech.com.cn", "", "", "eQuotation Error Massage by ", "", str, "")
        Return str
    End Function


    Shared Function getAccountOwnerByUser(ByVal user As String) As String
        Dim str As String = ""
        str += String.Format("SELECT DISTINCT ACCOUNT_ROW_ID FROM SIEBEL_ACCOUNT_OWNER_EMAIL AS z WHERE (ACCOUNT_ROW_ID IS NOT NULL) AND (EMAIL_ADDRESS = '{0}')", user)
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("EQ", str)
        Dim LST As String = ""
        If dt.Rows.Count > 0 Then
            For Each R As DataRow In dt.Rows
                LST += "'" & R.Item("ACCOUNT_ROW_ID") & "',"
            Next
            LST = LST.Trim(",")
        End If
        Return LST
    End Function

    'Shared Function GET_Attention_By_AccountRowID(ByVal row_id As String, ByVal name As String) As String
    '    Dim str As String = String.Format(" SELECT A.ROW_ID, " & _
    '                                    " IsNull(A.FST_NAME, '') AS 'FirstName', " & _
    '                                    " IsNull(A.MID_NAME, '') as 'MiddleName', " & _
    '                                    " IsNull(A.LAST_NAME, '') AS 'LastName', " & _
    '                                    " IsNull(A.JOB_TITLE, '') as JOB_TITLE, " & _
    '                                    " IsNull(A.EMAIL_ADDR, '') AS 'EMAIL_ADDRESS', " & _
    '                                    " IsNull(A.PER_TITLE, '') as Salutation " & _
    '                                    " FROM S_CONTACT A INNER JOIN S_ORG_EXT B ON A.PR_DEPT_OU_ID = B.PAR_ROW_ID " & _
    '                                    " WHERE A.ROW_ID = A.PAR_ROW_ID " & _
    '                                    " and B.ROW_ID='{0}' and (Upper(A.EMAIL_ADDR) like Upper('%{1}%') or Upper(A.FST_NAME) like Upper(N'%{1}%') or Upper(A.MID_NAME) like Upper(N'%{1}%') or Upper(A.LAST_NAME) like Upper(N'%{1}%'))" & _
    '                                    " order by A.ROW_ID ", row_id, name)
    '    Return str
    'End Function
    Shared Function GET_Attention_By_AccountRowID(ByVal row_id As String, ByVal name As String) As String
        Dim str As String = String.Format(" SELECT A.ROW_ID, " & _
                                " IsNull(A.FST_NAME, '') AS 'FirstName', " & _
                                " IsNull(A.MID_NAME, '') as 'MiddleName', " & _
                                " IsNull(A.LAST_NAME, '') AS 'LastName', " & _
                                " IsNull(A.JOB_TITLE, '') as JOB_TITLE, " & _
                                " IsNull(A.EMAIL_ADDR, '') AS 'EMAIL_ADDRESS', " & _
                                " IsNull(A.PER_TITLE, '') as Salutation " & _
                                " FROM S_PARTY_PER B LEFT JOIN S_CONTACT A ON A.ROW_ID=B.PERSON_ID " & _
                                " WHERE B.PARTY_ID='{0}' " & _
                                " and (Upper(A.EMAIL_ADDR) like Upper('%{1}%') or Upper(A.FST_NAME) like Upper(N'%{1}%') or Upper(A.MID_NAME) like Upper(N'%{1}%') or Upper(A.LAST_NAME) like Upper(N'%{1}%'))" & _
                                " order by A.ROW_ID ", row_id, name)
        Return str
    End Function
    Shared Function GET_Contact_Info_by_RowID(ByVal sales_ROWID As String) As DataTable
        Dim dt As New DataTable
        Dim str As String = String.Format("select TOP 1 ROW_ID, ISNULL(FST_NAME,'') AS FirstName ,ISNULL(MID_NAME,'') AS MiddleName, ISNULL(LAST_NAME,'') AS lastName, ISNULL(WORK_PH_NUM,'') AS workPhone,ISNULL(EMAIL_ADDR ,'')AS email_address,ISNULL(FAX_PH_NUM,'') as FaxNumber, ISNULL(CELL_PH_NUM,'') AS CellPhone, ISNULL(JOB_TITLE,'') AS JobTitle from S_CONTACT where ROW_ID='{0}'", sales_ROWID)
        dt = tbOPBase.dbGetDataTable("CRM", str)
        Return dt
    End Function

    ''' <summary>
    ''' Get BAA list(Standard verion)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GET_Standard_BAA_List() As DataTable
        Dim dt As New DataTable
        Dim str As String = String.Format("SELECT [VALUE],[TEXT] FROM SIEBEL_CONTACT_BAA_LOV Where IS_STANDARD='1' Order by [TEXT]")
        dt = tbOPBase.dbGetDataTable("MY", str)
        Return dt
    End Function


    Shared Function getPriSalesEmailByAccountROWID(ByVal ROWID As String) As String
        Dim Str As Object = String.Format("SELECT TOP 1 isnull(EMAIL_ADDR,'') FROM S_CONTACT WHERE ROW_ID=(SELECT TOP 1 PR_EMP_ID from S_POSTN where ROW_ID = (SELECT TOP 1 PR_POSTN_ID FROM S_ORG_EXT WHERE ROW_ID='{0}')) and EMAIL_ADDR is not null", ROWID)
        'Try
        'Dim EMAIL As String = tbOPBase.dbExecuteScalar("CRM", Str) 
        Dim EMAIL = tbOPBase.dbExecuteScalar("CRM", Str)

        If EMAIL Is Nothing Then Return ""

        Return EMAIL.ToString

        'If Not String.IsNullOrEmpty(EMAIL) Then
        '    Return EMAIL.ToString
        'End If

        'Catch ex As Exception

        'End Try
        'Return ""
    End Function

    Shared Function GET_CustomerContactRowID_by_Email(ByVal email As String) As String
        Dim str As String = String.Format("SELECT TOP 1 ROW_ID FROM S_CONTACT WHERE UPPER(EMAIL_ADDR)='{0}'", email.ToUpper)
        Dim ROWID As Object = tbOPBase.dbExecuteScalar("CRM", str)
        If Not IsNothing(ROWID) AndAlso ROWID.ToString <> "" Then
            Return ROWID.ToString
        End If
        Return "1-2SUYGX"
    End Function


    Shared Function GET_ContactRowID_by_Email(ByVal email As String) As String
        Dim str As String = String.Format("SELECT TOP 1 ROW_ID FROM S_CONTACT WHERE UPPER(EMAIL_ADDR)='{0}' and ROW_ID IN (select ROW_ID from S_USER WHERE UPPER(LOGIN) not like 'DELETE%')", email.ToUpper)
        Dim ROWID As Object = tbOPBase.dbExecuteScalar("CRM", str)
        If Not IsNothing(ROWID) AndAlso ROWID.ToString <> "" Then
            Return ROWID.ToString
        End If
        Return "1-2SUYGX"
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="email"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function getSiebelLoginNameByEmail(ByVal email As String) As String
        If String.IsNullOrEmpty(email) Then Return ""

        Dim Str As String = String.Format("select TOP 1 a.LOGIN from S_USER a left join [S_CONTACT] b on a.ROW_ID=b.ROW_ID" + _
                                          " WHERE UPPER(a.LOGIN) not like 'DELETE%' and Lower(b.EMAIL_ADDR)='{0}'", email.ToLower)
        'Try
        Dim LOGIN As String = tbOPBase.dbExecuteScalar("CRM", Str)

        If String.IsNullOrEmpty(LOGIN) Then Return ""

        Return LOGIN.ToString
        'Catch ex As Exception

        'End Try
        Return ""
    End Function


    Shared Function getSiebelLoginNameByRowid(ByVal rowId As String) As String
        Dim Str As String = String.Format("select TOP 1 LOGIN from S_USER WHERE ROW_ID='{0}' AND UPPER(LOGIN) not like 'DELETE%'", rowId)
        'Try
        Dim LOGIN As String = tbOPBase.dbExecuteScalar("CRM", Str)

        'Frank 2013/10/17
        If String.IsNullOrEmpty(LOGIN) Then Return ""

        Return LOGIN.ToString
        'Catch ex As Exception

        'End Try
        Return "MYADVANTECH"
    End Function

    'Shared Function GETOptyByName(ByVal OptyName As String, ByVal AccountRowId As String) As String
    '    Dim sb As New System.Text.StringBuilder
    '    With sb
    '        .AppendLine("Select [NAME]")
    '        .AppendLine(String.Format(" From S_OPTY"))
    '        'Frank 2012/07/17:iebel is case sensitive
    '        'so if you upper case the name then compare, you cannot distinguish the difference between “test” and “TEST”.
    '        '.AppendLine(String.Format(" Where upper(NAME) = N'{0}'", OptyName.Trim.ToUpper().Replace("'", "''")))
    '        .AppendLine(String.Format(" Where NAME = N'{0}' and PR_DEPT_OU_ID='{1}'", OptyName.Trim.Replace("'", "''"), AccountRowId))
    '    End With
    '    Return sb.ToString()
    'End Function



    'Shared Function GET_Opty(ByVal row_id As String, Optional ByVal OptyName As String = "", Optional ByVal PartNo As String = "") As String
    '    Dim sb As New System.Text.StringBuilder
    '    With sb
    '        .AppendLine(String.Format(" select A.ROW_ID, A.PR_DEPT_OU_ID as ACCOUNT_ROW_ID, A.NAME, A.CREATED, B.NAME as ACCOUNT_NAME, "))
    '        .AppendLine(String.Format(" IsNull((select top 1 z.NAME from S_ORG_EXT z where z.ROW_ID=A.PR_PRTNR_ID),'') as ChannelPartner "))
    '        .AppendLine(String.Format(" from S_OPTY A inner join S_ORG_EXT B on (A.PR_DEPT_OU_ID=B.ROW_ID or A.PR_PRTNR_ID=B.ROW_ID) "))
    '        .AppendLine(String.Format(" where A.PR_DEPT_OU_ID is not null   "))
    '        OptyName = OptyName.Trim.Replace("*", "%").Replace("'", "''").ToUpper()
    '        If OptyName <> "" Then
    '            .AppendLine(String.Format(" and (upper(A.NAME) like N'{0}%')  ", OptyName))
    '        End If
    '        .AppendLine(String.Format(" and B.ROW_ID='{0}' ", row_id))
    '        If PartNo <> "" Then
    '            .AppendLine(String.Format(" and A.ROW_ID IN (select OPTY_ID from S_REVN where PROD_ID in (select ROW_ID from S_PROD_INT where upper(NAME) LIKE '%{0}%') AND ROW_ID IN (SELECT PAR_ROW_ID FROM S_REVN_X))  ", PartNo.Trim.ToUpper().Replace("'", "''")))
    '        End If
    '        If OptyName.Trim = "" AndAlso row_id = "" AndAlso PartNo = "" Then .AppendLine(" and 1<>1 ")
    '        .AppendLine(String.Format(" order by A.NAME asc, A.CREATED desc   "))
    '    End With
    '    Return sb.ToString()
    'End Function

    Public Shared Function GetContactInfoWithSIEBEL_POSITION(ByVal _emailaddress As String) As DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(" SELECT  b.FirstName,b.MiddleName,b.LastName,b.WorkPhone,b.CellPhone,b.FaxNumber,b.OrgID,b.Sales_Rep ")
            .AppendLine(" ,b.EMAIL_ADDRESS,b.ACCOUNT_ROW_ID,b.ACCOUNT,b.COUNTRY,b.CREATED,b.LAST_UPDATED ")
            .AppendLine(" FROM SIEBEL_POSITION a left join SIEBEL_CONTACT b on a.CONTACT_ID = b.ROW_ID WHERE a.EMAIL_ADDR ='" & _emailaddress.Replace("'", "''") & "'")
        End With
        Return tbOPBase.dbGetDataTable("MY", sb.ToString())
    End Function

    Public Shared Function GetOwnerOfAccount(ByVal account_row_id As String) As DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" SELECT  b.USER_LOGIN, a.primary_flag, b.ROW_ID as POSITION_ID "))
            .AppendLine(String.Format(" FROM SIEBEL_ACCOUNT_OWNER AS a INNER JOIN SIEBEL_POSITION AS b ON a.OWNER_ID = b.CONTACT_ID "))
            .AppendLine(String.Format(" where b.USER_LOGIN is not null and b.USER_LOGIN<>'' and a.account_row_id='{0}' ", account_row_id.Replace("'", "")))
            .AppendLine(String.Format(" order by a.primary_flag desc  "))
        End With
        Return tbOPBase.dbGetDataTable("b2b", sb.ToString())
    End Function

    Shared Function CreateQuotationbyQuoteid(ByVal argid As String, ByVal optyId As String, ByVal oType As COMM.Fixer.eDocType) As String

        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim QuoteRowID As String = ""
        Dim quote_master As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(argid, oType)
        If Not IsNothing(quote_master) Then
            Dim em As String = ""
            Dim cur As String = "", Siebel_Quote_Name As String = "", due_date As String = "", quote_note As String = "", _
            customer_name As String = "", xRowId As String = "", xQuoteNumber As String = "", sales_email As String = "", _
            first_name As String = "", last_name As String = "", del_date1 As String = "", loginName As String = "", RBU As String = ""

            cur = quote_master.currency
            Siebel_Quote_Name = quote_master.quoteNo
            If IsNothing(quote_master.Revision_Number) = False AndAlso quote_master.Revision_Number > 0 Then
                Siebel_Quote_Name = Siebel_Quote_Name & " V" & quote_master.Revision_Number
            End If
            quote_note = quote_master.quoteNote
            customer_name = quote_master.AccRowId
            sales_email = quote_master.salesEmail
            del_date1 = quote_master.DocDate
            loginName = getSiebelLoginNameByRowid(quote_master.salesRowId)
            RBU = quote_master.siebelRBU

            Dim quote_detail As New DataTable
            Dim quote_DS As New DataSet
            quote_detail = myQD.GetDTbySelectStr(String.Format("select partNo as part_no,qty,newUnitPrice as price,getDate() as del_date from {0} where quoteId='{1}' order by line_no", myQD.tb, argid))
            quote_DS.Tables.Add(quote_detail)
            'QuoteRowID = CreateQuotationWithOpty(argid, xRowId, xQuoteNumber, Siebel_Quote_Name, cur, del_date1, quote_note, customer_name, sales_email, quote_DS, optyId, loginName, RBU, em)
            QuoteRowID = CreateQuotationWithOpty(argid, xQuoteNumber, Siebel_Quote_Name, cur, del_date1, quote_note, customer_name, sales_email, quote_DS, optyId, loginName, RBU, em)
        End If
        Return QuoteRowID
    End Function

    Shared Function CreateQuotationWithOpty(ByVal quoteId As String, ByVal xQuoteNumber As String, ByVal Siebel_Quote_Name As String, ByVal cur As String, ByVal del_date1 As String, ByVal quote_note As String, ByVal customer_name As String, ByVal sOwner As String, ByVal quote_DS As DataSet, ByVal opty_id As String, ByVal LoginName As String, ByVal RBU As String, ByRef EM As String) As String

        Dim SiebelQuoteRowID As String = String.Empty
        Try
            'Dim WS As New SiebelWS.Siebel_WS
            'WS.Timeout = -1
            'Throw New Exception("Create QuotationWithOpty failed. Test by Frank.")
            'WS.CreateSiebelQuotationWithOpportunity3(xRowId, xQuoteNumber, Siebel_Quote_Name, cur, del_date1, quote_note, customer_name, sOwner, quote_DS, opty_id, LoginName, RBU, EM)

            Dim USER_ID = ConfigurationManager.AppSettings("CRMHQId")
            Dim PASSWORD = ConfigurationManager.AppSettings("CRMHQPwd")

            'SiebelQuoteRowID = Advantech.Myadvantech.Business.SiebelBusinessLogic.CreateSiebelQuoteWithOpty(quoteId, opty_id, USER_ID, PASSWORD)

            Dim MessageStr As String = ""
            MessageStr &= "Return QuotationRowId:" & SiebelQuoteRowID
            MessageStr &= System.Environment.NewLine
            MessageStr &= "Call Method:" & "WS.CreateSiebelQuotationWithOpportunity(" & SiebelQuoteRowID & "," & xQuoteNumber & "," & Siebel_Quote_Name & ", " & cur & "," & del_date1 & "," & quote_note & "," & customer_name & "," & sOwner & ",quote_DS," & opty_id & "," & LoginName & "," & RBU & "," & EM & ")"
            MessageStr &= System.Environment.NewLine
            'WS.Dispose()
            Util.SendEmail("myadvantech@advantech.com", "MyAdvantech@advantech.com", "", "", "Create Quote2Siebel", "", MessageStr, "")
        Catch ex As Exception
            Dim _siebellogAPT As New SiebelDSTableAdapters.SiebelWSFailedLogTableAdapter
            Dim _parameter As String = SiebelQuoteRowID & "###" & xQuoteNumber & "###" & Siebel_Quote_Name & "###" & cur & "###" & del_date1 & "###" & quote_note & "###" & customer_name & "###" & sOwner & "###" & " " & "###" & opty_id & "###" & LoginName & "###" & RBU & "###" & EM
            _siebellogAPT.Insert(quoteId, "CreateQuotationbyQuoteid", _parameter, sOwner, ex.Message, Now, Now, False)
            _siebellogAPT = Nothing
        End Try
        Return SiebelQuoteRowID

    End Function


    'Shared Function CreateQuotationWithOpty(ByVal quoteId As String, ByVal xRowId As String, ByVal xQuoteNumber As String, ByVal Siebel_Quote_Name As String, ByVal cur As String, ByVal del_date1 As String, ByVal quote_note As String, ByVal customer_name As String, ByVal sOwner As String, ByVal quote_DS As DataSet, ByVal opty_id As String, ByVal LoginName As String, ByVal RBU As String, ByRef EM As String) As String
    '    Try
    '        Dim WS As New SiebelWS.Siebel_WS
    '        WS.Timeout = -1
    '        'Throw New Exception("Create QuotationWithOpty failed. Test by Frank.")
    '        WS.CreateSiebelQuotationWithOpportunity3(xRowId, xQuoteNumber, Siebel_Quote_Name, cur, del_date1, quote_note, customer_name, sOwner, quote_DS, opty_id, LoginName, RBU, EM)
    '        Dim MessageStr As String = ""
    '        MessageStr &= "Return QuotationRowId:" & xRowId
    '        MessageStr &= System.Environment.NewLine
    '        MessageStr &= "Call Method:" & "WS.CreateSiebelQuotationWithOpportunity(" & xRowId & "," & xQuoteNumber & "," & Siebel_Quote_Name & ", " & cur & "," & del_date1 & "," & quote_note & "," & customer_name & "," & sOwner & ",quote_DS," & opty_id & "," & LoginName & "," & RBU & "," & EM & ")"
    '        MessageStr &= System.Environment.NewLine
    '        WS.Dispose()
    '        Util.SendEmail("myadvantech@advantech.com", "MyAdvantech@advantech.com", "", "", "Create Quote2Siebel", "", MessageStr, "")
    '    Catch ex As Exception
    '        Dim _siebellogAPT As New SiebelDSTableAdapters.SiebelWSFailedLogTableAdapter
    '        Dim _parameter As String = xRowId & "###" & xQuoteNumber & "###" & Siebel_Quote_Name & "###" & cur & "###" & del_date1 & "###" & quote_note & "###" & customer_name & "###" & sOwner & "###" & " " & "###" & opty_id & "###" & LoginName & "###" & RBU & "###" & EM
    '        _siebellogAPT.Insert(quoteId, "CreateQuotationbyQuoteid", _parameter, sOwner, ex.Message, Now, Now, False)
    '        _siebellogAPT = Nothing
    '    End Try
    '    Return xRowId

    'End Function
    Shared Function CreateOptyByQuoteId(ByVal argid As String, ByVal optyName As String, ByVal oType As COMM.Fixer.eDocType) As String

        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim OPTY_NO As String = ""
        Dim M As IBUS.iDocHeader = Pivot.NewLineHeader
        Dim DT_OPTY As IBUS.iDocHeaderLine = M.GetByDocID(argid, oType)
        If Not IsNothing(DT_OPTY) Then
            'Dim odt As DataTable = GetOwnerOfAccount(DT_OPTY.Rows(0).Item("quoteToRowId"))
            Dim strowner As String = ""
            ''Dim strPosid As String = ""
            Dim closeDate As Date = Now.AddDays(30)
            'If odt.Rows.Count > 0 Then
            '    strowner = odt.Rows(0).Item("USER_LOGIN")
            '    'strPosid = odt.Rows(0).Item("POSITION_ID")
            'End If
            strowner = getSiebelLoginNameByRowid(DT_OPTY.salesRowId)
            Dim RBU As String = DT_OPTY.siebelRBU
            OPTY_NO = CreateOpty(DT_OPTY.AccRowId, optyName, DT_OPTY.quoteNote, myQD.getTotalAmount(argid), DT_OPTY.currency, closeDate, RBU, strowner, DT_OPTY.CreatedBy)

            'If OPTY_NO <> "" Then
            '    Dim DTDETAIL As DataTable = tbOPBase.dbGetDataTable("EQ", String.Format("SELECT PARTNO AS PART_NO,QTY FROM QUOTATIONDETAIL WHERE QUOTEID='{0}'", argid))
            '    DTDETAIL.TableName = "ws"
            '    CreateOptyForcast(OPTY_NO, DTDETAIL)
            'End If
        End If
        Return OPTY_NO
    End Function


    Shared Function CreateOptyByQuoteId_V2(ByVal argid As String, ByVal optyName As String, ByVal optyStage As String, ByVal oType As COMM.Fixer.eDocType, Optional ByVal OptyOwnerEmail As String = "") As String

        Dim OPTY_NO As String = "", M As IBUS.iDocHeader = Pivot.NewObjDocHeader
        Dim myQD As New EQDSTableAdapters.QuotationDetailTableAdapter
        Dim _dtQM As IBUS.iDocHeaderLine = M.GetByDocID(argid, oType)

        If Not IsNothing(_dtQM) Then
            Dim strowner As String = "", creatoremail As String = "", salesemail As String = "", siebelAccountRowId As String = ""
            Dim closeDate As Date = Now.AddDays(30), _rowQM As IBUS.iDocHeaderLine = _dtQM

            '20120725 Rudy: Get Owner Email
            salesemail = _rowQM.salesEmail : creatoremail = _rowQM.CreatedBy : siebelAccountRowId = _rowQM.AccRowId
            'Frank 2012/07/14
            If (Not IsNothing(OptyOwnerEmail)) AndAlso (Not String.IsNullOrEmpty(OptyOwnerEmail.Trim)) Then
                strowner = OptyOwnerEmail
            Else
                'Frank 2012/08/29:Below 3 step was confirmed with Jay
                '1. check owner email by Prepared For:(Internal Sales) <--QuotationMaster.salesEmail
                If Not String.IsNullOrEmpty(salesemail) Then
                    strowner = TransferOpportunityOwnerEmail(salesemail)
                End If
                '2. check owner email by quotation creator  <--QuotationMaster.createdBy
                If String.IsNullOrEmpty(strowner) AndAlso (Not String.IsNullOrEmpty(creatoremail)) Then
                    strowner = TransferOpportunityOwnerEmail(creatoremail)
                End If
                '3. check owner email by siebel account primary sales email <--QuotationMaster.quoteToRowId
                If String.IsNullOrEmpty(strowner) AndAlso (Not String.IsNullOrEmpty(siebelAccountRowId)) Then
                    strowner = SiebelTools.getPriSalesEmailByAccountROWID(siebelAccountRowId)
                End If
                ''4. If PriSalesEmailByAccountROWID is still empty, then just let strowner to be empty
                'If String.IsNullOrEmpty(strowner) Then
                '    '? TBD
                'End If
            End If
            OPTY_NO = CreateOpty_V2(argid, _rowQM.AccRowId, optyName, _rowQM.quoteNote, myQD.getTotalAmount(argid), _
                                    _rowQM.currency, closeDate, _rowQM.siebelRBU, strowner, _rowQM.CreatedBy, _
                                    optyStage, _rowQM.attentionRowId)

        End If
        Return OPTY_NO
    End Function



    Shared Function CreateOpty_V2(ByVal quoteId As String, ByVal AccountRowID As String, ByVal PROJ_NAME As String, ByVal PROJ_DESC As String, ByVal TOTALrevenue As String, ByVal Curr As String, ByVal closedate As Date, ByVal RBU As String, ByVal strOwner As String, ByVal CreatedBY As String, ByVal OptyStage As String, ByVal ContactID As String) As String

        Dim opty_no As String = ""
        'Try
        Dim strComment As String = PROJ_DESC & " [This is an auto-created OPTY linked with a quotation created by: " & CreatedBY & "]"

        '====Frank 2012/07/12 Creating opportunity in Siebel=======
        opty_no = SiebelTools.CreateOpportunity(quoteId, strOwner, AccountRowID, ContactID, PROJ_NAME, OptyStage, RBU, "", "", Curr)
        '====Frank 2012/07/12 Creating opportunity in Siebel End=======

        'Catch ex As Exception

        'End Try
        Return opty_no
    End Function



    Shared Function CreateOpty(ByVal AccountRowID As String, ByVal PROJ_NAME As String, ByVal PROJ_DESC As String, ByVal TOTALrevenue As String, ByVal Curr As String, ByVal closedate As Date, ByVal RBU As String, ByVal strOwner As String, ByVal CreatedBY As String) As String
        Dim opty_no As String = ""
        'Try
        Dim strComment As String = PROJ_DESC & " [This is an auto-created OPTY linked with a quotation created by: " & CreatedBY & "]"

        Dim WS As New SiebelWS.Siebel_WS
        WS.Timeout = -1
        'opty_no = WS.Import_Opportunity(strPosid, strOwner, AccountRowID, "", PROJ_NAME, strComment, "Funnel Sales Methodology", _
        '"50% Negotiating", closedate, TOTALrevenue, "50", "Pending", "", Curr)
        opty_no = WS.ImportOpportunity(PROJ_NAME, PROJ_DESC, "", "", False, closedate, RBU, "", "", "", TOTALrevenue, "", "", Curr, "Funnel Sales Methodology", "50% Negotiating", AccountRowID, "", "", strOwner, "")
        Dim MessageStr As String = ""
        MessageStr &= "Return OptyRowId:" & opty_no
        MessageStr &= System.Environment.NewLine
        MessageStr &= "Call Method:" & " WS.ImportOpportunity(" & PROJ_NAME & "," & PROJ_DESC & ", """", """", False, " & closedate & "," & RBU & ", """", """", """"," & TOTALrevenue & ", """", """"," & Curr & ", ""Funnel Sales Methodology"" , ""50% Negotiating""," & AccountRowID & ", """", """"," & strOwner & ", """")"
        'MessageStr &= System.Environment.NewLine
        WS.Dispose()
        Util.SendEmail("myadvantech@advantech.com", "MyAdvantech@advantech.com", "", "", "Create Quote2Siebel", "", MessageStr, "")
        'Catch ex As Exception

        'End Try
        Return opty_no
    End Function

    Shared Function CreateOptyForcast(ByVal opty_ID As String, ByVal DT As DataTable) As Boolean
        Dim WS As New SiebelWS.Siebel_WS
        WS.Timeout = -1
        WS.ImportOptyForecast(opty_ID, DT)
        WS.Dispose()
        Return True
    End Function
    Shared Function GET_Curr_By_AccoutRow_id(ByVal accountRowID As String, ByVal RBU As String) As String
        Dim str As Object = String.Format("select ISNULL(BASE_CURCY_CD,'') from S_ORG_EXT where ROW_ID='{0}'", accountRowID)
        Dim curr = tbOPBase.dbExecuteScalar("CRM", str)
        If Not IsNothing(curr) Then
            'HttpContext.Current.Response.Write("RBU") : HttpContext.Current.Response.End()
            If curr <> "" Then
                Return Business.Correctcurrecy(curr)
            Else
                If RBU = "ATW" Then
                    Return "TWD"
                ElseIf RBU = "HQDC" Then
                    Return "USD"
                Else
                    Return "EUR"
                End If
            End If
        End If

        'Ryan 20160919 Set Default Currency
        If Role.IsEUSales() Then
            Return "EUR"
        ElseIf Role.IsUsaUser Or Role.IsHQDCSales Then
            Return "USD"
        Else
            Return "TWD"
        End If

    End Function

    Shared Function GET_Account_Info_By_ID(ByVal argID As String) As DataTable
        Dim str As String = String.Format("select top 1 a.ROW_ID AS ROW_ID, IsNull(c.NAME, '') as RBU, a.NAME as COMPANYNAME, IsNull(b.ATTRIB_05,'') as ERPID, " & _
                            " IsNull(d.COUNTRY,'') as CITY, Isnull(a.LOC,'') as LOCATION, " & _
                            " IsNull( " & _
                                   " ( " & _
                                          " select top 1 e.NAME from S_PARTY e inner join S_POSTN f on e.ROW_ID=f.OU_ID where f.ROW_ID in " & _
                                                 " ( " & _
                                                        " select PR_POSTN_ID " & _
                                                        " from S_ORG_EXT " & _
                                                        " where ROW_ID=a.ROW_ID " & _
                                                 " ) " & _
                            " ),'')  as PriOwnerDivision, " & _
                            " IsNull( " & _
                                   " ( " & _
                                          " select top 1 f.NAME from S_POSTN f where f.ROW_ID in " & _
                                                 " ( " & _
                                                        " select PR_POSTN_ID " & _
                                                        " from S_ORG_EXT " & _
                                                        " where ROW_ID=a.ROW_ID " & _
                                                 " ) " & _
                            " ),'')  as PriOwnerPosition, IsNull(c.NAME, '') as RBU, " & _
                            " IsNull((select top 1 S_ADDR_PER.ADDR from S_ADDR_PER where S_ADDR_PER.ROW_ID=a.PR_ADDR_ID),'') as ADDRESS " & _
                            " from S_ORG_EXT a left join S_ORG_EXT_X b on a.ROW_ID=b.ROW_ID " & _
                            " left join S_PARTY c on a.BU_ID=c.ROW_ID " & _
                            " left join S_ADDR_PER d on a.PR_ADDR_ID=d.ROW_ID " & _
                            " where Upper(isnull(a.ROW_ID,''))='{0}'", argID.ToUpper)
        Dim dt As New DataTable
        dt = tbOPBase.dbGetDataTable("CRM", str)
        Return dt
    End Function

    Shared Function getErpIdFromSiebelByRowId(ByVal rowId As String) As String
        Dim STR As String = String.Format("SELECT TOP 1 ISNULL(ATTRIB_05,'') FROM S_ORG_EXT_X WHERE ROW_ID='" & rowId & "'")
        Dim ERPID As Object = tbOPBase.dbExecuteScalar("CRM", STR)
        If Not IsNothing(ERPID) AndAlso Business.is_Valid_Company_Id(ERPID.ToString) Then
            Return ERPID
        End If
        Return ""
    End Function
    Shared Function get_PriOwnerDiv_By_RowId(ByVal rowid As String) As String
        Dim str As String = String.Format("select top 1 ISNULL(e.NAME,'') from S_PARTY e inner join S_POSTN f on e.ROW_ID=f.OU_ID where f.ROW_ID =(select TOP 1 PR_POSTN_ID from S_ORG_EXT where ROW_ID='{0}')", rowid)
        Dim PriOwnerDiv As Object = tbOPBase.dbExecuteScalar("CRM", str)
        If Not IsNothing(PriOwnerDiv) Then
            Return PriOwnerDiv.ToString
        End If
        Return ""
    End Function
    Shared Function get_PriOwnerPos_By_RowId(ByVal rowid As String) As String
        Dim str As String = String.Format("select top 1 ISNULL(f.NAME,'') from S_POSTN f where f.ROW_ID in (select PR_POSTN_ID from S_ORG_EXT where ROW_ID='{0}')", rowid)
        Dim PriOwnerPos As Object = tbOPBase.dbExecuteScalar("CRM", str)
        If Not IsNothing(PriOwnerPos) Then
            Return PriOwnerPos.ToString
        End If
        Return ""
    End Function
    Shared Function GET_Account_info_By_ERPID(ByVal ERPID As String) As String
        Dim STR As String = String.Format("select top 1 a.ROW_ID AS ROW_ID, IsNull(c.NAME, '') as RBU, a.NAME as COMPANYNAME, IsNull(b.ATTRIB_05,'') as ERPID, " & _
                            " IsNull(d.COUNTRY,'') as CITY, Isnull(a.LOC,'') as LOCATION, " & _
                            " IsNull( " & _
                                   " ( " & _
                                          " select top 1 e.NAME from S_PARTY e inner join S_POSTN f on e.ROW_ID=f.OU_ID where f.ROW_ID in " & _
                                                 " ( " & _
                                                        " select PR_POSTN_ID " & _
                                                        " from S_ORG_EXT " & _
                                                        " where ROW_ID=a.ROW_ID " & _
                                                 " ) " & _
                            " ),'')  as PriOwnerDivision, " & _
                            " IsNull( " & _
                                   " ( " & _
                                          " select top 1 f.NAME from S_POSTN f where f.ROW_ID in " & _
                                                 " ( " & _
                                                        " select PR_POSTN_ID " & _
                                                        " from S_ORG_EXT " & _
                                                        " where ROW_ID=a.ROW_ID " & _
                                                 " ) " & _
                            " ),'')  as PriOwnerPosition, " & _
                            " IsNull((select top 1 S_ADDR_ORG.ADDR from S_ADDR_ORG where S_ADDR_ORG.ROW_ID=a.PR_ADDR_ID),'') as ADDRESS, " & _
                            " IsNull((select top 1 S_ADDR_ORG.COUNTRY from S_ADDR_ORG where S_ADDR_ORG.ROW_ID=a.PR_ADDR_ID),'') as COUNTRY, " & _
                            " IsNull((select top 1 S_ADDR_ORG.CITY from S_ADDR_ORG where S_ADDR_ORG.ROW_ID=a.PR_ADDR_ID),'') as CITY, " & _
                            " IsNull((select top 1 S_ADDR_ORG.STATE from S_ADDR_ORG where S_ADDR_ORG.ROW_ID=a.PR_ADDR_ID),'') as STATE, " & _
                            " IsNull((select top 1 S_ADDR_ORG.ZIPCODE from S_ADDR_ORG where S_ADDR_ORG.ROW_ID=a.PR_ADDR_ID),'') as ZIPCODE " & _
                            " from S_ORG_EXT a left join S_ORG_EXT_X b on a.ROW_ID=b.ROW_ID " & _
                            " left join S_PARTY c on a.BU_ID=c.ROW_ID " & _
                            " left join S_ADDR_ORG d on a.PR_ADDR_ID=d.ROW_ID " & _
                            " where ltrim(rtrim(Upper(isnull(b.ATTRIB_05,''))))='{0}'", ERPID.ToUpper)
        Return STR
    End Function
    'Shared Function getAccountListBySaleEmail(ByVal SaleEmail As String) As DataTable
    '    Dim i As Integer = InStr(SaleEmail, "@")
    '    If i <> 0 Then
    '        SaleEmail = SaleEmail.Substring(0, i)
    '    End If
    '    Dim sqlstr As New StringBuilder
    '    sqlstr.Append(" select a.ACCOUNT_ROW_ID, b.EMAIL_ADDRESS, c.ERP_ID ")
    '    sqlstr.Append(" from SIEBEL_ACCOUNT_OWNER a inner join SIEBEL_CONTACT b ")
    '    sqlstr.Append(" on a.OWNER_ID=b.ROW_ID inner join SIEBEL_ACCOUNT c on a.ACCOUNT_ROW_ID=c.ROW_ID")
    '    sqlstr.AppendFormat(" where b.EMAIL_ADDRESS like '{0}%'", SaleEmail)
    '    Dim dt As DataTable = tbOPBase.dbGetDataTable("B2B", sqlstr.ToString())
    '    If dt.Rows.Count > 0 Then
    '        Return dt
    '    End If
    '    Return Nothing
    'End Function

    Public Shared Function GetEnglishNameByeMail(ByVal eMail As String) As String
        Dim EnglishName As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select top 1 isnull( fst_name + ' ' +  lst_name,'') as EnglishName from EZ_EMPLOYEE  where  email_addr='{0}'", eMail.Trim))
        If EnglishName IsNot Nothing AndAlso Not String.IsNullOrEmpty(EnglishName) Then
            Return EnglishName.ToString.Trim
        End If
        Return String.Empty
    End Function


    Public Shared Function GetLocalNameByeMail(ByVal eMail As String) As String
        Dim localname As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select top 1 isnull(local_name,'') as localname from EZ_EMPLOYEE  where  email_addr='{0}'", eMail.Trim))
        If localname IsNot Nothing AndAlso Not String.IsNullOrEmpty(localname) Then
            Return localname.ToString.Trim
        End If
        Return String.Empty
    End Function
    Public Shared Function GeteMailByLocalName(ByVal LocalName As String) As String
        Dim email As Object = tbOPBase.dbExecuteScalar("MY", String.Format("select top 1 isnull(email_addr,'') as email_addr from EZ_EMPLOYEE  where  local_name=N'{0}'  and local_name <> '' ", LocalName.Trim))
        If email IsNot Nothing AndAlso Not String.IsNullOrEmpty(email) Then
            Return email.ToString.Trim
        End If
        Return String.Empty
    End Function
    Shared Function GetQuoteHeader(ByVal QuoteId As String) As System.Data.DataTable
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" select a.QUOTE_NUM,a.CURCY_CD as currency, a.TARGET_OU_ID as ACCOUNT_ROW_ID, b.NAME as ACCOUNT_NAME, IsNull(c.ATTRIB_05,'') as ERPID, "))
            .AppendLine(String.Format(" IsNull((select cast(sum(z.QTY_REQ*z.NET_PRI) as numeric(18,2)) from S_QUOTE_ITEM z where z.SD_ID=a.ROW_ID),0) as QUOTE_SUM, "))
            .AppendLine(String.Format(" a.NAME, IsNull(a.STATUS_DT,'') as QUOTE_STATUS,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.FST_NAME from S_CONTACT z where z.ROW_ID=a.CON_PER_ID),'') as First_Name,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.LAST_NAME from S_CONTACT z where z.ROW_ID=a.CON_PER_ID),'') as Last_Name,   "))
            .AppendLine(String.Format(" IsNull((select top 1 z.EMAIL_ADDR from S_CONTACT z where z.ROW_ID=a.CON_PER_ID),'') as Contact_Email,  "))
            .AppendLine("  ISNULL((SELECT top 1 T.ATTRIB_04 FROM S_DOC_QUOTE_X T where T.ROW_ID=a.ROW_ID),'') as SalesName,  ")
            .AppendLine(String.Format(" a.CURCY_CD as Currency,  "))
            .AppendLine(String.Format(" IsNull(a.EFF_START_DT,GetDate()) as Effective_Date,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.NAME from S_OPTY z where z.ROW_ID=a.OPTY_ID),'') as OPTY_NAME, a.OPTY_ID,  "))
            .AppendLine(String.Format(" IsNull((select top 1 z.CURR_STG_ID from S_OPTY z where z.ROW_ID=a.OPTY_ID),'') as Sales_Stage_ID,  "))
            .AppendLine("   IsNull((select TOP 1 NAME  from   S_STG  WHERE ROW_ID = (select top 1 z.CURR_STG_ID from S_OPTY z where z.ROW_ID=a.OPTY_ID)),'') as Sales_Stage_NAME,  ")
            .AppendLine(String.Format(" IsNull((select top 1 z.EMAIL_ADDR from S_CONTACT z where z.ROW_ID in (select z2.PR_EMP_ID from S_POSTN z2 where z2.ROW_ID=a.SALES_REP_POSTN_ID)),'') as Sales_Rep,  "))
            .AppendLine(String.Format(" a.CREATED, IsNull(a.DESC_TEXT,'') as QUOTE_DESC,IsNull(a.DUE_DT,GetDate()) as DUE_DT,a.EFF_END_DT, a.ACTIVE_FLG, "))
            .AppendLine(String.Format(" a.CREATED_BY, a.SALES_REP_POSTN_ID as OWNER_ID "))
            .AppendLine(String.Format(" from S_DOC_QUOTE a inner join S_ORG_EXT b on a.TARGET_OU_ID=b.ROW_ID  inner join S_ORG_EXT_X c on b.ROW_ID=c.ROW_ID	"))
            .AppendLine(String.Format(" where a.ROW_ID='{0}' ", QuoteId))
        End With
        Dim qmDt As DataTable = Nothing
        For i As Integer = 1 To 3
            Try
                qmDt = tbOPBase.dbGetDataTable("CRM", sb.ToString())
                Exit For
            Catch ex As System.Data.SqlClient.SqlException
                Threading.Thread.Sleep(500)
            End Try
        Next
        Return qmDt
    End Function
    Public Shared Function CopySiebelQuote2eQuotation(ByVal RowID As String, ByRef QuoteID As String) As Boolean
        Dim NewQuoteID As String = Pivot.NewObjDoc.NewUID()
        Dim NewQuoteNo As String = Business.GetNoByPrefix(Pivot.CurrentProfile) 'ex:AUSQ003054 
        ' Dim quoteno As String = TBquoteid.Text.Trim
        Dim QMdt = GetQuoteHeader(RowID)

        Dim QM As IBUS.iDocHeaderLine = Pivot.NewLineHeader
        Dim QML As New List(Of IBUS.iDocHeaderLine)

        With QMdt.Rows(0)
            QM.Key = NewQuoteID
            QM.CustomId = .Item("NAME") 'QUOTE_DESC
            QM.AccRowId = .Item("ACCOUNT_ROW_ID")
            QM.AccErpId = IIf(Business.is_Valid_Company_Id(.Item("ERPID")), .Item("ERPID"), "")
            QM.AccName = .Item("ACCOUNT_NAME")
            'QM.AccOfficeName = .Item("office")
            QM.currency = Business.Correctcurrecy(.Item("currency"))
            'QM.salesEmail = .Item("salesEmail")
            'QM.salesRowId = .Item("salesRowId")
            'QM.directPhone = .Item("directPhone")
            'QM.attentionRowId = .Item("attentionRowId")
            'QM.attentionEmail = .Item("attentionEmail")
            'QM.bankInfo = .Item("bankInfo")
            QM.DocDate = Now 'ParseSAPDate(.Item("AUDAT"))
            QM.deliveryDate = Now.AddDays(3).Date 'ParseSAPDate(.Item("vdatu"))
            QM.expiredDate = Today.AddDays(COMM.MasterFixer.getExpDaysByReg(Pivot.CurrentProfile.CurrDocReg)).ToShortDateString()
            'QM.shipTerm = .Item("shipTerm")
            QM.paymentTerm = "PPDW"
            If Not String.IsNullOrEmpty(QM.AccErpId) Then
                Dim PTV As String = "", PTN As String = ""
                Business.getPayMentTrem(QM.AccErpId.Trim, PTV, PTN, Pivot.CurrentProfile.getCurrOrg)
                If Not String.IsNullOrEmpty(PTV) Then
                    QM.paymentTerm = PTV
                End If
            End If
            'QM.freight = .Item("Freight")
            'QM.insurance = 0
            'If .Item("insurance") IsNot Nothing AndAlso Decimal.TryParse(.Item("insurance"), 0) Then
            '    QM.insurance = Decimal.Parse(.Item("insurance"))
            'End If
            'QM.specialCharge = 0
            'If .Item("specialCharge") IsNot Nothing AndAlso Decimal.TryParse(.Item("specialCharge"), 0) Then
            '    QM.specialCharge = Decimal.Parse(.Item("specialCharge"))
            'End If
            'QM.tax = 0
            'If .Item("Tax") IsNot Nothing AndAlso Not IsDBNull(.Item("Tax")) AndAlso Decimal.TryParse(.Item("Tax"), 0) Then
            '    QM.tax = Decimal.Parse(.Item("Tax"))
            'End If
            QM.quoteNote = "" ' .Item("quoteNote")
            ' QM.relatedInfo = .Item("relatedInfo")
            QM.CreatedBy = Pivot.CurrentProfile.UserId
            QM.CreatedDate = Now
            '  QM.preparedBy = .Item("preparedBy")
            QM.qStatus = CInt(COMM.Fixer.eDocStatus.QDRAFT)
            QM.isShowListPrice = 0
            QM.isShowDiscount = 0
            QM.isShowDueDate = 0
            QM.isLumpSumOnly = 0
            QM.isRepeatedOrder = 0
            Dim RBU As String = Util.GetRBUforQuote(.Item("ACCOUNT_ROW_ID"), .Item("ERPID"))
            QM.AccOfficeName = RBU
            If String.IsNullOrEmpty(QM.AccOfficeName) Then
                QM.AccOfficeName = "ATW"
            End If
            ' QM.AccGroupName = .Item("ogroup")
            QM.delDateFlag = 0
            QM.org = Pivot.CurrentProfile.getCurrOrg '.Item("Org")
            QM.siebelRBU = RBU
            'If IsDBNull(.Item("DIST_CHAN")) OrElse String.IsNullOrEmpty(.Item("DIST_CHAN")) Then
            '    QM.DIST_CHAN = 30
            'Else
            '    QM.DIST_CHAN = .Item("DIST_CHAN")
            'End If
            'QM.DIVISION = .Item("DIVISION")
            'QM.AccGroupCode = .Item("SALESGROUP")
            'QM.AccOfficeCode = .Item("SALESOFFICE")
            'QM.SalesDistrict = .Item("DISTRICT")
            ' QM.PO_NO = .Item("PO_NO") ' vbkd.Rows(0).Item("bstkd")
            '  QM.CARE_ON = .Item("CARE_ON")
            QM.OriginalQuoteID = RowID '.Item("quoteid")
            QM.isExempt = 0 ' IIf(Relics.SAPDAL.isTaxExempt(.Item("COMPANYID")), 1, 0)
            'If .Item("isExempt") IsNot Nothing AndAlso Integer.TryParse(.Item("isExempt"), 0) Then
            '    QM.isExempt = Integer.Parse(.Item("isExempt"))
            'End If

            'QM.Inco1 = .Item("INCO1")
            'QM.Inco2 = .Item("INCO2")
            'QM.quoteNo = quoteno
            QM.tax = 0.05
            QM.Revision_Number = 1
            QM.Active = True
            QM.DocReg = 96
            If Pivot.CurrentProfile.CurrDocReg.ToString.StartsWith("EU", StringComparison.CurrentCultureIgnoreCase) Then
                QM.DocReg = 31
            End If
            QM.DOCSTATUS = 0
        End With
        QML.Add(QM)
        Pivot.NewObjDocHeader.AddByAssignedUID(NewQuoteID, QM, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
        Dim QDdt As DataTable = GetQuoteLines(RowID) ' GetDetailFromeStore(quoteno)
        '''''''''''''''''''''''''''''''''''
        Dim Arry As New ArrayList
        For Each dr As DataRow In QDdt.Rows
            Arry.Add(dr.Item("PART_NO"))
        Next
        Dim pns() As String = Arry.ToArray(GetType(String))
        If pns.Length > 0 Then
            'Ming20150304开始批量添加料
            Dim _QuoteDetailDT As DataTable = tbOPBase.dbGetDataTable("EQ", "Select * from QuotationDetail Where QuoteID='-1'")
            Dim dt1 As DataTable = tbOPBase.dbGetDataTable("MY", String.Format("Select PART_NO,EXTENDED_DESC From SAP_PRODUCT_EXT_DESC Where PART_NO in ('{0}')", String.Join("','", pns)))
            Dim _row As DataRow = Nothing, _ParentLineNumber = 0, _ItemType = 0, _ProDesc As String = String.Empty, _IsUpdateEW = False
            Dim EWitem As QuoteItem = Nothing, EWint As Integer = 0
            Dim _LineNo As Integer = 1
            If _ParentLineNumber = 0 Then
                Dim _Quotelist As List(Of QuoteItem) = MyQuoteX.GetQuoteLooseItems(NewQuoteID)
                If _Quotelist IsNot Nothing AndAlso _Quotelist.Count > 0 Then
                    _LineNo = _Quotelist.LastOrDefault().line_No + 1
                End If
            Else
                Dim QITEM As QuoteItem = MyQuoteX.GetQuoteItem(NewQuoteID, _ParentLineNumber)
                If QITEM IsNot Nothing AndAlso QITEM.ChildListX.Count > 0 Then
                    _LineNo = QITEM.ChildListX.LastOrDefault().line_No + 1
                    Dim _Quotelist As List(Of QuoteItem) = QITEM.ChildListX
                    EWitem = _Quotelist.FirstOrDefault(Function(p) p.partNo.StartsWith("AGS-EW"))
                    If EWitem IsNot Nothing Then
                        _IsUpdateEW = True
                        _LineNo = EWitem.line_No
                        EWint = QITEM.ewFlag
                        'QITEM.ewFlag = 0
                        'For Each item In _Quotelist
                        '    item.ewFlag = 0
                        '    If item.IsEWpartnoX Then
                        '        MyQuoteX.DeleteQuoteItem(UID, item.line_No)
                        '        _LineNo = item.line_No
                        '        Exit For
                        '    End If
                        'Next
                    End If
                Else
                    _LineNo = CInt(_ParentLineNumber) + 1
                End If

            End If
            Dim _IsATWUser As Boolean = Role.IsTWAonlineSales
            Dim _ListPrice As Decimal, _UnitPrice As Decimal, ITP As Decimal, isHaveErpID = False
            If QM.AccErpId IsNot Nothing AndAlso Not String.IsNullOrEmpty(QM.AccErpId) Then isHaveErpID = True
            Dim _Order As Order = Nothing

            Dim plant As String = Business.getPlantByOrgID(QM.org)
            If isHaveErpID Then
                'Ming 20150305 批量获取price
                Dim PNlist As New List(Of PNInfo)
                For Each pn In pns
                    ' If Not pn.ToUpper.EndsWith("BTOS", StringComparison.InvariantCultureIgnoreCase) Then
                    PNlist.Add(New PNInfo(pn, 1, plant))
                    ' End If
                Next
                _Order = MyQuoteX.getMultiPrice(NewQuoteID, PNlist)
            End If
            Dim k As Integer = 0
            For Each pn In pns
                _row = _QuoteDetailDT.NewRow()
                _row.Item("PartNo") = pn
                _row.Item("quoteId") = NewQuoteID

                If pn.EndsWith("BTO", StringComparison.InvariantCultureIgnoreCase) Then
                    _LineNo = ((_LineNo + 100) \ 100) * 100
                End If
                _row.Item("Line_No") = _LineNo
                _ListPrice = 0 : _UnitPrice = 0 : ITP = 0
                If isHaveErpID AndAlso _Order IsNot Nothing Then
                    Dim _part As Part = _Order.LineItems.FirstOrDefault(Function(p) p.PartNumber = pn)
                    If _part IsNot Nothing Then
                        _ListPrice = _part.ListPrice
                        _UnitPrice = _part.UnitPrice
                    End If
                End If
                If Not isHaveErpID OrElse _ListPrice = 0 OrElse _UnitPrice = 0 Then
                    Business.GetPriceV2(pn, NewQuoteID, Pivot.CurrentProfile.getCurrOrg, _ListPrice, _UnitPrice)
                End If
                Business.GetITP(NewQuoteID, pn, ITP, _UnitPrice)
                _row.Item("ListPrice") = _ListPrice
                _row.Item("UnitPrice") = _UnitPrice
                _row.Item("newUnitPrice") = _UnitPrice
                _ParentLineNumber = 0 : Dim itemtype As Integer = 0
                If _LineNo >= 100 Then
                    _ParentLineNumber = (_LineNo \ 100) * 100
                    If _LineNo Mod 100 = 0 Then
                        _ParentLineNumber = 0
                        itemtype = 1
                    End If
                End If

                _row.Item("HigherLevel") = _ParentLineNumber
                _row.Item("RECFIGID") = ""
                _row.Item("ItemType") = itemtype
                _row.Item("QTY") = QDdt.Rows(k)("QTY_REQ")
                'Frank 20150213 ATW的報價單，Virtual Part No.預設帶出Part No.
                If _IsATWUser Then _row.Item("VirtualPartNo") = _row.Item("PartNo")

                _row.Item("Description") = "" : _row.Item("DisplayLineNo") = ""
                _row.Item("DisplayUnitPrice") = "" : _row.Item("DisplayQty") = ""


                If dt1 IsNot Nothing AndAlso dt1.Rows.Count > 0 Then
                    Dim drcurr() As DataRow = dt1.Select(String.Format("PART_NO ='{0}'", pn))
                    If drcurr.Length >= 1 AndAlso drcurr(0).Item("EXTENDED_DESC") IsNot DBNull.Value Then
                        _row.Item("Description") = drcurr(0).Item("EXTENDED_DESC").ToString
                    End If
                    'Else
                    '    _row.Item("Description") = ""
                End If

                _row.Item("Itp") = ITP
                _row.Item("newItp") = ITP
                _row.Item("DeliveryPlant") = plant
                _row.Item("Category") = ""
                _row.Item("rohs") = 0
                _row.Item("satisfyFlag") = 0
                _row.Item("canBeConfirmed") = 1
                _row.Item("custMaterial") = ""
                _row.Item("inventory") = 0
                _row.Item("modelNo") = ""
                _row.Item("ewFlag") = EWint
                _row.Item("reqDate") = Now
                _row.Item("dueDate") = Now
                _row.Item("DisplayQty") = QDdt.Rows(k)("QTY_REQ").ToString()
                _LineNo += 1
                Dim dr() As DataRow = QDdt.Select(String.Format("PART_NO='{0}' ", pn))
                If dr.Length > 0 Then
                    If Not IsDBNull(dr(0).Item("DISCOUNT_PRICE")) AndAlso Not String.IsNullOrEmpty(dr(0).Item("DISCOUNT_PRICE")) Then
                        If Decimal.TryParse(dr(0).Item("DISCOUNT_PRICE"), 0) Then
                            If Decimal.Parse(dr(0).Item("DISCOUNT_PRICE")) > 0 Then
                                _row.Item("DisplayUnitPrice") = Decimal.Parse(dr(0).Item("DISCOUNT_PRICE"))
                            End If
                        End If
                    End If
                    'If dr(0).Item("QTY_REQ") IsNot Nothing AndAlso Not String.IsNullOrEmpty(dr(0).Item("QTY_REQ")) Then
                    If Not IsDBNull(dr(0).Item("QTY_REQ")) AndAlso Not String.IsNullOrEmpty(dr(0).Item("QTY_REQ")) Then
                        _row.Item("DisplayQty") = dr(0).Item("QTY_REQ")
                    End If
                    'If dr(0).Item("ItemMark") IsNot Nothing AndAlso Not String.IsNullOrEmpty(dr(0).Item("ItemMark")) Then
                    If Not IsDBNull(dr(0).Item("ItemMark")) AndAlso Not String.IsNullOrEmpty(dr(0).Item("ItemMark")) Then
                        _row.Item("DisplayLineNo") = dr(0).Item("ItemMark")
                    End If

                End If
                k = k + 1
                _QuoteDetailDT.Rows.Add(_row)
            Next

            Dim bkdetail As New System.Data.SqlClient.SqlBulkCopy(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
            bkdetail.DestinationTableName = "QuotationDetail"
            bkdetail.WriteToServer(_QuoteDetailDT)
            If _IsUpdateEW Then
                If EWitem IsNot Nothing Then
                    EWitem.line_No = _LineNo + 1
                    MyUtil.Current.CurrentDataContext.SubmitChanges()
                End If
                MyQuoteX.ReSetLineNo(NewQuoteID)
            End If
            'Dim myOptyQuote As New EQDSTableAdapters.optyQuoteTableAdapter
            'myOptyQuote.DeleteOptyByQuoteID(NewQuoteID)

            'If QMdt.Rows(0).Item("OPTY_ID") IsNot Nothing AndAlso Not String.IsNullOrEmpty(QMdt.Rows(0).Item("OPTY_ID")) Then
            If Not IsDBNull(QMdt.Rows(0).Item("OPTY_ID")) AndAlso Not String.IsNullOrEmpty(QMdt.Rows(0).Item("OPTY_ID")) Then
                Dim OptyQ As IBUS.iOptyQuote = Pivot.NewObjOptyQuote
                Dim optyQuote1 As IBUS.iOptyQuoteLine = Pivot.NewLineOptyQuote
                optyQuote1.optyId = QMdt.Rows(0).Item("OPTY_ID")
                optyQuote1.optyName = QMdt.Rows(0).Item("OPTY_NAME")
                optyQuote1.quoteId = NewQuoteID
                optyQuote1.optyStage = QMdt.Rows(0).Item("Sales_Stage_NAME")
                OptyQ.DeleteByQuoteID(NewQuoteID) : OptyQ.Add(optyQuote1)
            End If

            'If QMdt.Rows(0).Item("ERPID") IsNot Nothing AndAlso Not String.IsNullOrEmpty(QMdt.Rows(0).Item("ERPID")) Then
            If Not IsDBNull(QMdt.Rows(0).Item("ERPID")) AndAlso Not String.IsNullOrEmpty(QMdt.Rows(0).Item("ERPID")) Then

                Dim aptSiebAccount As New SiebelDSTableAdapters.SIEBEL_ACCOUNTTableAdapter
                Dim dtAccount As SiebelDS.SIEBEL_ACCOUNTDataTable = aptSiebAccount.GetAccountByRowId(QMdt.Rows(0).Item("ACCOUNT_ROW_ID"))
                If dtAccount.Count > 0 Then
                    Dim AccountRow As SiebelDS.SIEBEL_ACCOUNTRow = dtAccount(0)
                    'RBU = AccountRow.RBU : siebelRBU = RBU
                    Dim Attention As String = ""
                    Dim City As String = AccountRow.CITY : Dim Country As String = AccountRow.COUNTRY : Dim State As String = AccountRow.STATE
                    Dim Street As String = AccountRow.ADDRESS : Dim Tel As String = AccountRow.PHONE_NUM : Dim Zipcode As String = AccountRow.ZIPCODE
                    Dim Fax As String = AccountRow.FAX_NUM : Dim soldtoname As String = AccountRow.ACCOUNT_NAME
                    ''''
                    Dim SapCustAddrDt As SAPDAL.SalesOrder.PartnerAddressesDataTable = SAPDAL.SAPDAL.GetSAPPartnerAddressesTableByKunnr(QMdt.Rows(0).Item("ERPID"), True)

                    If SapCustAddrDt.Count > 0 Then
                        Dim SapCustAddrRow As SAPDAL.SalesOrder.PartnerAddressesRow = SapCustAddrDt(0)
                        'Dim strDefaultStreet As String = "", strDefaultCity As String = "", strDefaultZip As String = "", strDefaultState As String = "", strDefaultCountry As String = ""
                        'Dim dtPartner As Relics.SalesOrder.SAP_BAPIPARNRDataTable = SAPTools.GetSAPPartnerTableByKunnr(hQuoteErpId)
                        'If dtPartner.Count > 0 Then
                        '    strDefaultStreet = dtPartner(0).STREET : strDefaultCity = dtPartner(0).CITY : strDefaultState = dtPartner(0)._REGION
                        '    strDefaultZip = dtPartner(0).POSTL_CODE : strDefaultCountry = dtPartner(0).COUNTRY
                        'End If
                        Attention = SapCustAddrRow.C_O_Name : City = SapCustAddrRow.City : Country = SapCustAddrRow.Country
                        State = SapCustAddrRow.Region_str : Street = SapCustAddrRow.Street : Tel = SapCustAddrRow.Tel1_Numbr
                        Zipcode = SapCustAddrRow.Postl_Cod1
                        Fax = SapCustAddrRow.Fax_Number
                    End If
                    'Dim SapCustAddrRow As Relics.SalesOrder.PartnerAddressesRow = SapCustAddrDt(0)


                    Dim apt As New EQDSTableAdapters.EQPARTNERTableAdapter
                    apt.DeleteQuotePartnersByQuoteId(NewQuoteID)

                    apt.InsertPartner(NewQuoteID, "", QMdt.Rows(0).Item("ERPID").Trim.ToUpper, soldtoname, _
                                      Street + " " + City + " " + State + " " + Zipcode + " " + Country, _
                                      "SOLDTO", Attention, Tel, Tel, Zipcode, Country, City, Street, State, "", "", Fax)
                    apt.InsertPartner(NewQuoteID, "", QMdt.Rows(0).Item("ERPID").Trim.ToUpper, soldtoname, _
                                     Street + " " + City + " " + State + " " + Zipcode + " " + Country, _
                                     "S", Attention, Tel, Tel, Zipcode, Country, City, Street, State, "", "", Fax)
                    apt.InsertPartner(NewQuoteID, "", QMdt.Rows(0).Item("ERPID").Trim.ToUpper, soldtoname, _
                                     Street + " " + City + " " + State + " " + Zipcode + " " + Country, _
                                     "B", Attention, Tel, Tel, Zipcode, Country, City, Street, State, "", "", Fax)
                    'apt.InsertPartner(NewQuoteID, "", Me.txtBillID.Text.Trim.ToUpper, txtBillName.Text, _
                    '                  txtBillToStreet.Text + " " + txtBillToCity.Text + " " + txtBillToState.Text + " " + txtBillToZip.Text + " " + txtBillToCountry.Text, _
                    '                  "B", strBillToAttention, strBillToTel, strBillToMobile, txtBillToZip.Text, txtBillToCountry.Text, txtBillToCity.Text, txtBillToStreet.Text, txtBillToState.Text, "", txtBillToStreet2.Text, HFbilltofax.Value)

                End If

            End If

            Business.ADDQuotationUpdatelog(NewQuoteID, "Added partno " & String.Join(",", pns))
        End If
        QuoteID = NewQuoteID

        'Exit Function
        Return True

        ' '''''''''''''''''''''''''''''''''''
        'Dim i As Integer = 1, ParentLineNo As Integer = 0, IsParentItem As Boolean = False
        'For Each r As DataRow In QDdt.Rows
        '    Dim QDdr As New QuoteItem
        '    IsParentItem = False
        '    With r
        '        QDdr.quoteId = NewQuoteID
        '        QDdr.line_No = .Item("line_No")
        '        QDdr.HigherLevel = 0 'Integer.Parse(.Item("HigherLevel"))
        '        QDdr.partNo = .Item("PARTNO")
        '        If IsNumeric(.Item("otype")) AndAlso Integer.TryParse(.Item("otype"), 0) Then
        '            Select Case Integer.Parse(.Item("otype"))
        '                Case 0 : QDdr.line_No = i : QDdr.HigherLevel = 0
        '                Case -1
        '                    QDdr.line_No = MyQuoteX.getBtosParentLineNo(NewQuoteID)
        '                    ParentLineNo = QDdr.line_No
        '                    QDdr.HigherLevel = 0
        '                    IsParentItem = True
        '                    If Not IsDBNull(.Item("BTONO")) AndAlso Not String.IsNullOrEmpty(.Item("BTONO")) Then
        '                        QDdr.partNo = .Item("BTONO")
        '                    End If
        '                Case 1 : QDdr.line_No = ParentLineNo + i : QDdr.HigherLevel = ParentLineNo
        '            End Select
        '        End If
        '        QDdr.description = .Item("description")
        '        QDdr.qty = 1
        '        If IsNumeric(.Item("QTY")) Then
        '            QDdr.qty = Integer.Parse(.Item("QTY"))
        '        End If
        '        QDdr.listPrice = 0
        '        If Decimal.TryParse(.Item("listPrice"), 0) AndAlso Not IsParentItem Then
        '            QDdr.listPrice = Decimal.Parse(.Item("listPrice"))
        '        End If
        '        QDdr.unitPrice = 0
        '        If Decimal.TryParse(.Item("unitPrice"), 0) AndAlso Not IsParentItem Then
        '            QDdr.unitPrice = Decimal.Parse(.Item("unitPrice"))
        '        End If
        '        'QDdr.newUnitPrice = .Item("newUnitPrice")
        '        QDdr.newUnitPrice = 0
        '        If Decimal.TryParse(.Item("newUnitPrice"), 0) AndAlso Not IsParentItem Then
        '            QDdr.newUnitPrice = Decimal.Parse(.Item("newUnitPrice"))
        '        End If
        '        QDdr.itp = 0
        '        QDdr.newItp = 0
        '        QDdr.deliveryPlant = .Item("deliveryPlant")
        '        QDdr.category = ""
        '        QDdr.classABC = ""
        '        QDdr.rohs = 0
        '        QDdr.ewFlag = 0
        '        QDdr.reqDate = Now
        '        QDdr.dueDate = Now
        '        QDdr.satisfyFlag = 0
        '        QDdr.canBeConfirmed = 0
        '        QDdr.custMaterial = ""
        '        QDdr.inventory = 0
        '        QDdr.ItemType = 0
        '        If IsNumeric(.Item("otype")) AndAlso Integer.Parse(.Item("otype")) = -1 Then
        '            QDdr.ItemType = 1
        '        End If
        '        QDdr.modelNo = .Item("modelNo")
        '        QDdr.sprNo = ""
        '        ' QDdr.HigherLevel = Integer.Parse(.Item("HigherLevel"))
        '    End With
        '    'QD.Add(QDdr)
        '    MyUtil.Current.CurrentDataContext.QuoteItems.InsertOnSubmit(QDdr)
        '    MyUtil.Current.CurrentDataContext.SubmitChanges()
        '    i = i + 1
        'Next


        ''gv2.DataSource = QD
        ''gv2.DataBind()
        'Dim QP As New EQDS.EQPARTNERDataTable
        'If QM.AccErpId IsNot Nothing AndAlso Not String.IsNullOrEmpty(QM.AccErpId) AndAlso Business.is_Valid_Company_Id(QM.AccErpId) Then
        '    Dim dt As DataTable = tbOPBase.dbExecuteScalar("CRM", SiebelTools.GET_Account_info_By_ERPID(QM.AccErpId))
        '    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
        '        QM.AccRowId = dt.Rows(0).Item("ROW_ID")
        '    End If
        '    Dim SapCustAddrDt As Relics.SalesOrder.PartnerAddressesDataTable = Relics.SAPDAL.GetSAPPartnerAddressesTableByKunnr(QM.AccErpId, True)
        '    If SapCustAddrDt.Count > 0 Then
        '        Dim QPdr As EQDS.EQPARTNERRow = QP.NewEQPARTNERRow()
        '        QPdr.QUOTEID = NewQuoteID
        '        QPdr.ERPID = QM.AccErpId
        '        Dim SapCustAddrRow As Relics.SalesOrder.PartnerAddressesRow = SapCustAddrDt(0)
        '        QPdr.NAME = SapCustAddrRow.Name
        '        QPdr.ADDRESS = ""
        '        QPdr.ATTENTION = SapCustAddrRow.C_O_Name
        '        QPdr.TEL = SapCustAddrRow.Tel1_Numbr
        '        QPdr.MOBILE = "" 'DT.Rows(0).Item("ADRNR")
        '        QPdr.ZIPCODE = SapCustAddrRow.Postl_Cod1
        '        QPdr.COUNTRY = SapCustAddrRow.Country
        '        QPdr.CITY = SapCustAddrRow.City
        '        QPdr.STREET = SapCustAddrRow.Street
        '        QPdr.STATE = SapCustAddrRow.Region_str
        '        QPdr.DISTRICT = ""
        '        QPdr.STREET2 = ""
        '        For j As Integer = 1 To 3
        '            Select Case j
        '                Case 1
        '                    QPdr.TYPE = "SOLDTO"
        '                    QP.Rows.Add(QPdr)
        '                Case 2
        '                    Dim QPdr2 As EQDS.EQPARTNERRow = QP.NewEQPARTNERRow()
        '                    QPdr2.ItemArray = QPdr.ItemArray
        '                    QPdr2.TYPE = "S"
        '                    QP.Rows.Add(QPdr2)
        '                Case 3
        '                    Dim QPdr3 As EQDS.EQPARTNERRow = QP.NewEQPARTNERRow()
        '                    QPdr3.ItemArray = QPdr.ItemArray
        '                    QPdr3.TYPE = "B"
        '                    QP.Rows.Add(QPdr3)
        '            End Select
        '        Next
        '    End If
        'End If

        'tbOPBase.adddblog("delete from quotationMaster where quoteid='" & NewQuoteID & "'")
        'Dim QDA As New EQDSTableAdapters.QuotationDetailTableAdapter
        ''QDA.DeleteQuoteDetailById(NewQuoteID)
        'tbOPBase.adddblog("delete from quotationDetail where quoteid='" & NewQuoteID & "'")
        'Dim QPA As New EQDSTableAdapters.EQPARTNERTableAdapter
        'QPA.DeleteQuotePartnersByQuoteId(NewQuoteID)
        'tbOPBase.adddblog("DELETE FROM EQPARTNER WHERE (QUOTEID = '" & NewQuoteID & "')")
        'Dim conn As New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        'conn.Open()
        'Dim bk As New SqlClient.SqlBulkCopy(conn)
        'bk.DestinationTableName = "EQPARTNER" : bk.WriteToServer(QP)
        'Pivot.NewObjDocHeader.AddByAssignedUID(NewQuoteID, QM, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)
        '' bk.DestinationTableName = "QuotationDetail" : bk.WriteToServer(QD)
        ''  Dim strErr As String = ""
        '' Dim addItemType As EnumSetting.ItemType = COMM.Fixer.eItemType.Others, intParentLineNo As Integer = 0, strCategoryName As String = ""
        ''For Each dr As EQDS.QuotationDetailRow In QD.Rows
        ''    strCategoryName = Business.GetCategoryName(dr.partNo, QM.org)
        ''    If Integer.Parse(dr.line_No) > 100 Then
        ''        addItemType = COMM.Fixer.eItemType.Others : intParentLineNo = 100
        ''    End If
        ''    If Integer.Parse(dr.line_No) = 100 Then
        ''        addItemType = COMM.Fixer.eItemType.Parent : intParentLineNo = 0
        ''    End If
        ''    'If Not Business.ADD2QUOTE_V2(NewQuoteId, IsNumericItem_Shrink(dr.partNo), dr.qty, 0, addItemType, strCategoryName, 1, 1, dr.reqDate, intParentLineNo, dr.line_No, strErr) Then
        ''    If Not Business.ADD2QUOTE_V2_1(NewQuoteID, IsNumericItem_Shrink(dr.partNo), dr.qty, 0, addItemType, strCategoryName, 1, 1, dr.reqDate, intParentLineNo, dr.deliveryPlant, dr.line_No, strErr, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ) Then
        ''        Response.Write(dr.partNo + strErr) : Exit Sub
        ''    End If
        ''Next
        'If conn.State <> ConnectionState.Closed Then conn.Close()
        'Business.RefreshQuotationInventory(NewQuoteID)
        ''  Response.Redirect("~/quote/QuotationMaster.aspx?UID=" & NewQuoteID)

        'Return False
    End Function
    Shared Function GetQuoteLinesSql(ByVal QuoteId As String) As String
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" SELECT a.LN_NUM, b.NAME AS PART_NO, b.DESC_TEXT, IsNull(cast(IsNull(a.BASE_UNIT_PRI,a.UNIT_PRI) as numeric(18,2)),0) as START_PRICE,  "))
            'Nada20140102 for Show's request changed Qty to Req Qty ,Price to Net Price
            '.AppendLine(String.Format(" IsNull(cast(IsNull(a.NET_PRI,a.BASE_UNIT_PRI) as numeric(18,2)),0) as DISCOUNT_PRICE, "))
            .AppendLine(String.Format(" (case when ISNUMERIC(c.ATTRIB_15) = 1 then CAST(c.ATTRIB_15 as numeric(18,2)) else 0 end) as DISCOUNT_PRICE, "))
            '/Nada20140102
            .AppendLine(String.Format(" case  CONVERT(int, IsNull(a.BASE_UNIT_PRI ,0)) when 0 then CONVERT(varchar(10),0.0)+'%' else "))
            .AppendLine(String.Format(" cast(IsNull(cast((a.BASE_UNIT_PRI-IsNull(a.UNIT_PRI,a.BASE_UNIT_PRI))/a.BASE_UNIT_PRI*100 as numeric(18,2)),0.0) as varchar(10))+'%' end as DISC, "))
            'Nada20140102 for Show's request changed Qty to Req Qty ,Price to Net Price
            '.AppendLine(String.Format(" cast(a.QTY_REQ as int) as QTY_REQ, cast(a.EXTD_QTY as int) as EXTD_QTY, "))
            .AppendLine(String.Format(" (case when ISNUMERIC(c.ATTRIB_04)=1 THEN CAST(c.ATTRIB_04 as int) ELSE 1 END) as QTY_REQ, cast(a.EXTD_QTY as int) as EXTD_QTY, "))
            '/Nada20140102
            .AppendLine(String.Format(" IsNull(c.ATTRIB_03,'') as Product_Rpt,IsNull(c.ATTRIB_05,'') as ItemMark, IsNull(c.ATTRIB_47,'') as Description_Rpt,convert(varchar(14),a.QUOTE_ITM_EXCH_DT,111) as duedate "))
            .AppendLine(String.Format(" FROM S_QUOTE_ITEM AS a INNER JOIN S_PROD_INT AS b ON a.PROD_ID = b.ROW_ID inner join S_QUOTE_ITEM_X c on a.ROW_ID=c.ROW_ID "))
            .AppendLine(String.Format(" WHERE a.SD_ID = '{0}' ORDER BY a.LN_NUM", QuoteId))
        End With
        Return sb.ToString()
    End Function
    Shared Function GetQuoteLines(ByVal QuoteId As String) As DataTable
        Dim qiDt As DataTable = Nothing
        For i As Integer = 1 To 3
            Try
                qiDt = tbOPBase.dbGetDataTable("CRM", GetQuoteLinesSql(QuoteId))
                Exit For
            Catch ex As System.Data.SqlClient.SqlException
                Threading.Thread.Sleep(500)
            End Try
        Next
        If qiDt.Rows.Count > 0 Then
            For Each dr As DataRow In qiDt.Rows
                If String.Equals(dr.Item("PART_NO").ToString.Trim, "T") Then
                    dr.Item("PART_NO") = "PTRADE-BTO"
                End If
            Next
            qiDt.AcceptChanges()
        End If
        Return qiDt
    End Function
End Class
