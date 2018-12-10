Imports System.Configuration
Imports MSXML2
Imports ADODB
Imports System.Web
Imports System.IO
Imports SAP.Connector
Imports System.Reflection
Imports System.Globalization
Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Caching
Imports System.Text

Public Class UserRole

    Enum DocType
        Order
        Quote
    End Enum

    Public Enum AccountStatus
        EZ
        CP
        GA
        KA
        DMS
        FC
    End Enum

    Public Shared Function GetAccountStatusEnum(ByVal Siebel_Account_Status As String)

        Dim _accstatusid As String = getAccStatus(Siebel_Account_Status)
        Dim _accenum As UserRole.AccountStatus = AccountStatus.GA
        If Not [Enum].TryParse(Of UserRole.AccountStatus)(_accstatusid, _accenum) Then
            _accenum = AccountStatus.GA
        End If

        Return _accenum
    End Function

    ''' <summary>
    ''' Get internal user's mail group
    ''' </summary>
    ''' <param name="user_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMailGroupByInternalUser(ByVal user_email As String) As ArrayList
        'Dim str1 As String = String.Format(" select distinct C.Name " & _
        '                        " from ADVANTECH_ADDRESSBOOK a left join ADVANTECH_ADDRESSBOOK_ALIAS b on a.ID=b.ID inner join ADVANTECH_ADDRESSBOOK_GROUP c on a.ID=c.ID   " & _
        '                        " where b.Email = N'{0}' or a.PrimarySmtpAddress=N'{0}' ", user_email)

        Dim str1 As String = String.Format(" select distinct C.Name " & _
                                " from AD_MEMBER a left join AD_MEMBER_ALIAS b on a.PrimarySmtpAddress=b.EMAIL Inner join AD_MEMBER_GROUP c on a.PrimarySmtpAddress=c.EMAIL " & _
                                " where b.ALIAS_EMAIL = N'{0}' or a.PrimarySmtpAddress=N'{0}' ", user_email)


        Dim dt As DataTable = dbUtil.dbGetDataTable("MY", str1)
        Dim Ar As New ArrayList
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            If dt.Rows.Count > 0 Then
                For Each r As DataRow In dt.Rows
                    Ar.Add(r.Item("Name").ToString.ToUpper)
                Next
            End If
        End If
        Return Ar
    End Function

    ''' <summary>
    ''' Based on user's mail goup and customer's account status to get AUS's order/quotation prefix number
    ''' </summary>
    ''' <param name="Ar"></param>
    ''' <param name="account_status"></param>
    ''' <param name="_docType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAUSDocNumberPrefixStringByGroup(ByVal Ar As ArrayList, ByVal account_status As AccountStatus, ByVal _docType As DocType) As String

        If Ar Is Nothing OrElse Ar.Count = 0 Then
            Select Case _docType
                Case DocType.Order
                    Return "BT"
                Case DocType.Quote
                    Return "AUSQ"
            End Select
        End If

        Dim preFix As New StringBuilder

        '_IsAUSAOlineSales means if the user belong to Aonline.usa or Aonline.usa.iag
        Dim _IsAUSAOlineSales As Boolean = IsAUSAOlineSales(Ar)

        '_IsSalesIagUsa means if the user belong to Sales.Iag.Usa
        Dim _IsSalesIagUsa As Boolean = IsInGroupSalesIagUsa(Ar)

        If _IsAUSAOlineSales And _IsSalesIagUsa Then
            'Frank
            If account_status = AccountStatus.CP OrElse account_status = AccountStatus.KA Then
                preFix.Append("AAC")
            Else
                preFix.Append("AUS")
            End If

        ElseIf _IsAUSAOlineSales And Not _IsSalesIagUsa Then
            preFix.Append("AUS")
        ElseIf Not _IsAUSAOlineSales And _IsSalesIagUsa Then
            preFix.Append("AAC")
        Else
            Select Case _docType
                Case DocType.Order
                    Return "BT"
                Case DocType.Quote
                    Return "AUSQ"
            End Select
        End If
        Select Case _docType
            Case DocType.Order
                preFix.Append("O")
            Case DocType.Quote
                preFix.Append("Q")
        End Select

        Return preFix.ToString

    End Function

    ''' <summary>
    ''' Return true if user is AUS Aonline sales(including Aonline.USA and Aonline.USA.IAG)
    ''' </summary>
    ''' <param name="GA"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsAUSAOlineSales(ByVal GA As ArrayList) As Boolean
        If IsInGroupAonlineUsa(GA) OrElse IsInGroupAonlineUsaIag(GA) Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Return true if user belongs to mail group Aonline.USA or SALES.AISA.USA
    ''' </summary>
    ''' <param name="GA"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsInGroupAonlineUsa(ByVal GA As ArrayList) As Boolean
        If GA.Contains(("Aonline.USA").ToUpper) OrElse _
            GA.Contains("SALES.AISA.USA") Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Return true if user belongs to mail group Aonline.USA.IAG
    ''' </summary>
    ''' <param name="GA"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsInGroupAonlineUsaIag(ByVal GA As ArrayList) As Boolean
        If GA.Contains(("Aonline.USA.IAG").ToUpper) Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Return true if user belongs to mail group SALES.IAG.USA(AAC CP or KA sales)
    ''' </summary>
    ''' <param name="GA"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsInGroupSalesIagUsa(ByVal GA As ArrayList) As Boolean
        If GA.Contains("SALES.IAG.USA") Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function getAccStatus(ByVal Status As String) As String
        Select Case Status
            Case "01-Platinum Channel Partner", "01-Premier Channel Partner", "02-Gold Channel Partner", "03-Certified Channel Partner"
                Return "CP"
            Case "03-Premier Key Account", "04-Premier Key Account", "06G-Golden Key Account(ACN)", "06-Key Account"
                Return "KA"
            Case "05-D&Ms PKA"
                Return "DMS"
            Case "06P-Potential Key Account", "07-General Account", "08-General Account(List Price)", "12-Leads", "11-Prospect", "10-Sales Contact", "11-Sales Contact"
                Return "GA"
            Case Else
                Return "GA"
        End Select
        Return "GA"
    End Function

End Class

Public Class GetNextWorkingDate
    Public Shared Function Get_Next_WorkingDate_ByCode(ByRef iATPDate As DateTime, ByVal Loading_Days As String, ByVal code As String) As Integer
        code = UCase(code)
        Dim factory_date_Number As Decimal
        Dim provider1 As New CultureInfo("fr-FR", True)
        Dim time1 As DateTime = iATPDate
        Dim iATPDateStr As String = time1.ToString("yyyyMMdd")
        Try
            Dim factory_date_Number1 As Decimal = factory_date_Number
            Dim iATPDateStr1 As String = iATPDateStr
            Date2FactDate("+", code, factory_date_Number1, "", iATPDateStr1)
            FactDate2Date(code, (factory_date_Number1 + Loading_Days), iATPDateStr1)
            Dim time2 As DateTime = DateTime.ParseExact(iATPDateStr, "yyyyMMdd", provider1)
            iATPDate = time2
        Catch ex As Exception
            Return -1
            Exit Function
        End Try
        Return 1
    End Function
    Shared Sub Date2FactDate(ByVal CorrectOption As String, ByVal RegionCode As String, ByRef FactDate As Decimal, ByRef WorkingDayIndicator As String, ByRef WorkingDateStr As String)
        Dim sDate As DateTime = Now
        Dim dt As DataTable = GetCalSetDTAndStart(RegionCode, sDate)

        Dim WorkingDate As DateTime = DateTime.ParseExact(WorkingDateStr, "yyyyMMdd", Nothing)
        If WorkingDate < sDate Then
            WorkingDate = sDate
        End If
        FactDate = 0
        Dim tempDate As DateTime = sDate
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            For Each r As DataRow In dt.Rows
                Dim DateRow As Char() = r.Item("dataCount").ToString.ToCharArray
                If Not IsNothing(DateRow) AndAlso DateRow.Length > 0 Then
                    For Each c As Char In DateRow
                        tempDate = DateAdd(DateInterval.Day, 1, tempDate)
                        If c = "1" Then
                            FactDate += 1
                            If tempDate >= WorkingDate Then
                                WorkingDateStr = tempDate.ToString("yyyyMMdd")
                                Exit Sub
                            End If
                        End If
                    Next
                End If
            Next
        End If
    End Sub
    Shared Sub FactDate2Date(ByVal RegionCode As String, ByVal FactDate As Decimal, ByRef WorkingDateStr As String)
        Dim sDate As DateTime = Now
        Dim dt As DataTable = GetCalSetDTAndStart(RegionCode, sDate)
        Dim tempDate As DateTime = sDate
        Dim tempFdate As Decimal = 0
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            For Each r As DataRow In dt.Rows
                Dim DateRow As Char() = r.Item("dataCount").ToString.ToCharArray
                If Not IsNothing(DateRow) AndAlso DateRow.Length > 0 Then
                    For Each c As Char In DateRow
                        tempDate = DateAdd(DateInterval.Day, 1, tempDate)
                        If c = "1" Then
                            tempFdate += 1
                            If tempFdate >= FactDate Then
                                WorkingDateStr = tempDate.ToString("yyyyMMdd")
                                Exit Sub
                            End If
                        End If
                    Next
                End If
            Next
        End If
    End Sub
    Shared Function GetCalSetDTAndStart(ByVal RegionCode As String, ByRef StartDate As DateTime) As DataTable
        Dim DT As DataTable = dbUtil.dbGetDataTable("MY", String.Format(" select Jahr, (MON01 + MON02 + MON03 + MON04 + MON05 + MON06 + MON07 + " & _
                                                                        " MON08 + MON09 + MON10 + MON11 + MON12 ) AS dataCount from dbo.ShippingCalendar_new " & _
                                                                        " where IDENT='{0}' order by jahr", RegionCode))
        StartDate = Now
        If Not IsNothing(DT) AndAlso DT.Rows.Count > 0 Then
            StartDate = DateAdd(DateInterval.Day, -1, CDate(DT(0).Item("Jahr") & "-01-01"))
        End If
        Return DT
    End Function
End Class
Public Class CommonLogic

    Public Shared Function isAllowedChangePrice(ByVal Part_No As String, ByVal orgid As String) As Boolean
        If String.IsNullOrEmpty(Part_No) Then Return True
        If String.IsNullOrEmpty(orgid) Then Return True
        If orgid.Length < 2 Then Return True
        Dim dicpns As Dictionary(Of String, Boolean) = HttpContext.Current.Cache("isAllowedChangePrice")
        If dicpns Is Nothing Then
            dicpns = New Dictionary(Of String, Boolean)
            HttpContext.Current.Cache.Add("isAllowedChangePrice", dicpns, Nothing, Now.AddHours(8), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If
        If dicpns.ContainsKey(Part_No) Then
            Return dicpns.Item(Part_No)
        End If
        Select Case orgid.ToUpper.Substring(0, 2)
            Case "EU"

                'Dear Tc,
                'Yes, please apply it to all ZSRV material group, this will solve the issue.
                'Thank you.
                'Best Regards,
                'Özdal Turp
                Dim _Count = dbUtil.dbExecuteScalar("MY", "Select count(PART_NO) from SAP_PRODUCT where PART_NO='" & Part_No & "' and MATERIAL_GROUP='ZSRV'")
                If _Count > 0 Then
                    If Not dicpns.ContainsKey(Part_No) Then
                        dicpns.Add(Part_No, False)
                    End If
                    Return False
                End If
                'If Part_No.StartsWith("IMG-", StringComparison.CurrentCultureIgnoreCase) _
                '    OrElse Part_No.StartsWith("IMG ", StringComparison.CurrentCultureIgnoreCase) _
                '    OrElse Part_No.StartsWith("CTOS-", StringComparison.CurrentCultureIgnoreCase) Then
                '    If Not dicpns.ContainsKey(Part_No) Then
                '        dicpns.Add(Part_No, False)
                '    End If
                '    Return False
                'End If
            Case Else
                Return True
        End Select
        Return True
    End Function

    Public Shared Function isAllowedAddEW(ByVal Part_No As String, ByVal ProdType As String, ByVal orgid As String) As Boolean
        If String.IsNullOrEmpty(Part_No) Then Return True
        If String.IsNullOrEmpty(orgid) Then Return True
        If orgid.Length < 2 Then Return True
        Dim dicpns As Dictionary(Of String, Boolean) = HttpContext.Current.Cache("isAllowedAddEW")
        If dicpns Is Nothing Then
            dicpns = New Dictionary(Of String, Boolean)
            HttpContext.Current.Cache.Add("isAllowedAddEW", dicpns, Nothing, Now.AddHours(8), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)
        End If
        If dicpns.ContainsKey(Part_No) Then
            Return dicpns.Item(Part_No)
        End If
        If String.IsNullOrEmpty(ProdType) Then
            Dim Ptype As Object = dbUtil.dbExecuteScalar("MY", String.Format("select  top 1 isnull(PRODUCT_TYPE,'')  as PRODUCT_TYPE  from  SAP_PRODUCT where PART_NO ='{0}'", Part_No))
            If Ptype IsNot Nothing Then
                ProdType = Ptype.ToString
            End If
        End If
        Select Case orgid.ToUpper.Substring(0, 2)
            Case "EU"
                If Part_No.StartsWith("IPS-", StringComparison.CurrentCultureIgnoreCase) OrElse syncSingleProduct.IsPTD(Part_No, ProdType) Then
                    If Not dicpns.ContainsKey(Part_No) Then
                        dicpns.Add(Part_No, False)
                    End If
                    Return False
                End If
            Case Else
                Return True
        End Select
        Return True
    End Function



    Public Shared Function isMEDC(ByVal Part_No As String) As Boolean
        Dim str As String = String.Format(" select COUNT(part_no) from SAP_PRODUCT where PART_NO='{0}' and PRODUCT_LINE in " & _
                                            "('DLGR'," & _
                                            "'POCI'," & _
                                            "'ITPA'," & _
                                            "'POCA'," & _
                                            "'IPCV'," & _
                                            "'POCN'," & _
                                            "'POCS'," & _
                                            "'HITW')", Part_No)
        Dim O As Object = dbUtil.dbExecuteScalar("MY", str)
        If Integer.TryParse(O, 0) AndAlso CInt(O) > 0 Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function NoStandardSensitiveITP(ByVal partNo As String) As Boolean
        If partNo.ToUpper.ToString.StartsWith("CTOS-") Then
            Return True
        End If
        If partNo.ToUpper.ToString.StartsWith("C-CU-") Then
            Return True
        End If
        If partNo.ToUpper.ToString.StartsWith("AGS-") Then
            Return True
        End If
        If partNo.ToUpper.ToString.StartsWith("IMG-") Then
            Return True
        End If

        Return False
    End Function

    Shared Function getEWItemByMonth(ByVal month As Integer) As String
        If IsNumeric(month) AndAlso month > 0 And month.ToString.Length < 3 Or month = 999 Then
            If month = 99 Then
                Return "AGS-EW-AD"
            End If
            If month = 999 Then
                Return "AGS-EW/DOA-03"
            End If
            Return "AGS-EW-" & month.ToString("00")
        End If
        Return ""
    End Function

    Shared Function getRateByEWItem(ByVal itemNo As String, ByVal Plant As String) As Double

        If Plant.Length = 4 AndAlso Plant.StartsWith("US", StringComparison.OrdinalIgnoreCase) Then
            Select Case itemNo.ToUpper.Trim()
                Case "AGS-EW-03"
                    Return 0.03
                Case "AGS-EW-06"
                    Return 0.05
                Case "AGS-EW-12"
                    Return 0.1
                Case "AGS-EW-24"
                    Return 0.15
                Case "AGS-EW-36"
                    Return 0.25
            End Select
        ElseIf Plant.Length = 4 AndAlso Plant.StartsWith("JP", StringComparison.OrdinalIgnoreCase) Then
            Select Case itemNo.ToUpper.Trim()
                Case "AGS-EW-03"
                    Return 0.03
                Case "AGS-EW-06"
                    Return 0.055
                Case "AGS-EW-12"
                    Return 0.1
                Case "AGS-EW-24"
                    Return 0.18
                Case "AGS-EW-36"
                    Return 0.25
            End Select
        Else
            Select Case itemNo.ToUpper.Trim()
                Case "AGS-EW-03"
                    Return 0.0125
                Case "AGS-EW-06"
                    Return 0.025
                Case "AGS-EW-09"
                    Return 0.05
                Case "AGS-EW-12"
                    Return 0.06
                Case "AGS-EW-15"
                    Return 0.07
                Case "AGS-EW-21"
                    Return 0.08
                Case "AGS-EW-24"
                    Return 0.1
                Case "AGS-EW-36"
                    Return 0.15
                Case "AGS-EW-AD"
                    Return 0.05
                Case "AGS-EW/DOA-03"
                    Return 0.01
            End Select
        End If

        Return 0
    End Function
    <Obsolete("Replaced by isWarrantableV2")>
    Shared Function isWarrantable(ByVal PN As String) As [Boolean]
        'Frank 2012/10/16: If it is BTOS main item, then just return true to let Quotation.ewFlag can be updated by extended warranty months
        If String.IsNullOrEmpty(PN) = False AndAlso PN.EndsWith("-BTO", StringComparison.InvariantCultureIgnoreCase) Then
            Return True
        End If

        Dim DT As DataTable = getSAPProductLine(PN)
        If DT.Rows.Count = 0 Then Return True
        If isSoftware(PN) Then
            Return False
        ElseIf PN.StartsWith("AGS", StringComparison.OrdinalIgnoreCase) OrElse PN.StartsWith("OPTION", StringComparison.OrdinalIgnoreCase) Then
            'assembly or special option
            Return False

        ElseIf DT.Rows(0).Item("PRODUCT_LINE") IsNot Nothing AndAlso DT.Rows(0).Item("PRODUCT_LINE") = "ASS#" Then
            'assembly charges
            Return False
        Else
            Return True
        End If
    End Function
    <Obsolete("Replaced by isSoftwareV2")>
    Shared Function isSoftware(ByVal PN As String) As [Boolean]

        Dim DT As New DataTable
        DT = getSAPProductLine(PN)
        If DT.Rows.Count = 0 Then Return True

        If PN.StartsWith("96SW") OrElse PN.StartsWith("98MQ") OrElse PN.StartsWith("968Q") Then
            Return True
        ElseIf DT.Rows(0).Item("PRODUCT_LINE") IsNot Nothing AndAlso (DT.Rows(0).Item("PRODUCT_LINE") = "EPCS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "EDOS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WCOM" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WAUT" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "DAAS") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function isWarrantableV2(ByVal PN As String, ByVal OrgID As String) As Boolean
        'Frank 20140605: Only AEU's extended warranty fee need to be calculated with all parts
        If String.Equals(OrgID, "EU10", StringComparison.CurrentCultureIgnoreCase) Then
            Return True
        End If

        Dim DT As New DataTable
        DT = dbac.getProductByLine(PN)
        If DT.Rows.Count = 0 Then Return True

        'Ryan 20170425 Special logic for AJP
        If String.Equals(OrgID, "JP01", StringComparison.CurrentCultureIgnoreCase) Then
            'Ryan 20170425 
            If isSoftwareV2(PN, DT) Then
                Return False
            ElseIf PN.StartsWith("AGS") OrElse PN.StartsWith("XAJP") Then
                Return False
            Else
                Return True
            End If
        End If

        If isSoftwareV2(PN, DT) Then
            Return False
        ElseIf PN.StartsWith("AGS") OrElse PN.StartsWith("OPTION") Then
            Return False
        ElseIf DT.Rows(0).Item("PRODUCT_LINE") IsNot Nothing AndAlso DT.Rows(0).Item("PRODUCT_LINE") = "ASS#" Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared Function isSoftwareV2(ByVal PN As String, ByRef DT As DataTable) As Boolean
        'Dim DT As New DataTable
        'DT = dbac.getProductByLine(PN)
        'If DT.Rows.Count = 0 Then Return True
        If PN.StartsWith("96SW") OrElse PN.StartsWith("98MQ") OrElse PN.StartsWith("968Q") OrElse PN.StartsWith("968T") Then
            Return True
        ElseIf DT.Rows(0).Item("PRODUCT_LINE") IsNot Nothing AndAlso (DT.Rows(0).Item("PRODUCT_LINE") = "EPCS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "EDOS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WCOM" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WAUT" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "DAAS") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function isWarrantableV3(ByVal PN As String, ByVal OrgID As String) As Boolean

        If String.Equals(OrgID, "EU10", StringComparison.CurrentCultureIgnoreCase) Then
            Return True
        ElseIf String.Equals(OrgID, "JP01", StringComparison.CurrentCultureIgnoreCase) Then
            If isSoftwareV3(PN) Then
                Return False
            ElseIf PN.StartsWith("XAJP") Then
                Return False
            Else
                Return True
            End If
        Else
            If isSoftwareV3(PN) Then
                Return False
            Else
                Return True
            End If
        End If

    End Function

    Public Shared Function isSoftwareV3(ByVal PN As String) As Boolean
        Dim dt As DataTable = dbUtil.dbGetDataTable("MY", String.Format("select * from SAP_PRODUCT where PART_NO = '{0}' ", PN))
        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then

            If dt.Rows(0).Item("MATERIAL_GROUP") IsNot Nothing AndAlso
                       (dt.Rows(0).Item("MATERIAL_GROUP") = "ZSRV" OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "968MS" OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "96SW" OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "98" OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "ZHD0" OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "ZSPC" OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "ZINC" OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "206" OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "207" OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "968MS/SW" OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "968OT" OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "968WA" OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "98DP") OrElse
                        dt.Rows(0).Item("MATERIAL_GROUP") = "20" Then
                Return True
            ElseIf dt.Rows(0).Item("PRODUCT_TYPE") = "ZCON" Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function

    Shared Function getSAPProductLine(ByVal PN As String) As DataTable
        Dim DT As New DataTable
        Dim STR As String = String.Format(" SELECT     PART_NO, MODEL_NO, MATERIAL_GROUP, DIVISION, PRODUCT_HIERARCHY, PRODUCT_GROUP, PRODUCT_DIVISION, PRODUCT_LINE, " & _
       " GENITEMCATGRP, PRODUCT_DESC, ROHS_FLAG, EGROUP, STATUS, EDIVISION, NET_WEIGHT, VOLUME, WEIGHT_UNIT, GROSS_WEIGHT, " & _
       " VOLUME_UNIT, PRODUCT_TYPE, LAST_UPD_DATE, CREATE_DATE, SIZE_DIMENSIONS, GIP_CODE " & _
       " FROM SAP_PRODUCT " & _
       " WHERE     (PART_NO = '{0}')", PN)
        DT = dbUtil.dbGetDataTable("MY", STR)
        Return DT
    End Function

    Shared Function getListPrice(ByVal partno As String, ByVal org As String, ByVal CURR As String) As Decimal
        Dim ListPrice As Decimal = 0, objLp As Object = Nothing
        'Dim cmd As New SqlClient.SqlCommand( _
        '    "select TOP 1 LIST_PRICE from PRODUCT_LIST_PRICE where ORG=@ORG and CURRENCY=@CUR and PART_NO = @PN and LIST_PRICE>=0 order by LIST_PRICE desc", _
        '    New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("EQ").ConnectionString))
        'cmd.Parameters.AddWithValue("ORG", org) : cmd.Parameters.AddWithValue("CUR", CURR) : cmd.Parameters.AddWithValue("PN", partno)
        'cmd.Connection.Open()
        'objLp = cmd.ExecuteScalar()
        'cmd.Connection.Close()

        If objLp IsNot Nothing Then
            Return CType(objLp, Decimal)
        Else
            Dim spd As New SAPDAL
            If org = "US01" Then
                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, "UEPP5001", LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If org = "JP01" Then
                Dim LpDt As DataTable = Nothing
                If CURR = "JPY" Then
                    spd.getSAPPriceByTable(partno, org, "JJCBOM", LpDt)
                ElseIf CURR = "USD" Then
                    spd.getSAPPriceByTable(partno, org, "UUAASC", LpDt)
                End If
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
            If String.Equals(org, "EU10", StringComparison.CurrentCultureIgnoreCase) Then
                Dim dummyERPID As String = "EDDEVI07"
                If String.Equals(CURR, "GBP", StringComparison.CurrentCultureIgnoreCase) Then
                    dummyERPID = "EKGBEC02"
                ElseIf String.Equals(CURR, "USD", StringComparison.CurrentCultureIgnoreCase) Then
                    dummyERPID = "EFESAL01"
                End If
                Dim LpDt As DataTable = Nothing
                spd.getSAPPriceByTable(partno, org, dummyERPID, LpDt)
                If LpDt IsNot Nothing AndAlso LpDt.Rows.Count > 0 Then
                    Return LpDt.Rows(0).Item("Kzwi1")
                End If
            End If
        End If
        Return 0
    End Function
    Shared Function isANAPnBelowGP(ByVal PN As String, ByVal unitPrice As Decimal, ByRef gpPrice As Decimal, ByRef errmsg As String) As Boolean
        If Not SAPDAL.isNeedANAGPControl(PN) Then
            Return False
        End If
        Dim ws As New SAPDAL
        gpPrice = ws.getPriceByProdLine(PN, ws.getProdPricingGrp(PN), errmsg)

        If gpPrice > unitPrice Then
            Dim listprice As Decimal = 0
            listprice = getListPrice(PN, "US01", "USD")
            If listprice < gpPrice Then
                gpPrice = listprice
            End If
        End If

        If gpPrice > 0 AndAlso unitPrice < gpPrice Then
            Return True
        End If
        'PPerc = getANAGPPercByPN(PN, div)
        'Dim Pcost As Decimal = getCostForANAPn(PN, Plant)
        'If (unitPrice - Pcost) < (PPerc * Pcost) Then
        '    Return True
        'Else
        '    Return False
        'End If
        Return False
    End Function

    Public Shared Function GetBackOrder(ByVal company_id As String, ByVal org_id As String, ByVal part_no As String, ByVal so_no As String, ByVal po_no As String, ByVal order_date_from As String, ByVal order_date_to As String, ByVal user_id As String) As DataSet
        Dim boDt As New DataTable
        Call GetBackOrderAB(boDt, company_id, org_id, part_no, so_no, po_no, order_date_from, order_date_to, user_id)
        Call GetBackOrderC(boDt, company_id, org_id, part_no, so_no, po_no, order_date_from, order_date_to, user_id)
        If boDt.Rows.Count > 0 Then boDt.DefaultView.Sort = "ORDERDATE desc"
        Dim ds As New DataSet
        ds.Tables.Add(boDt)

        Return ds
    End Function

    Private Shared Sub GetBackOrderAB(ByRef boDt As DataTable, ByVal company_id As String, ByVal org_id As String, ByVal part_no As String, ByVal so_no As String, ByVal po_no As String, ByVal order_date_from As String, ByVal order_date_to As String, ByVal user_id As String)
        Try
            Dim kunnr As String = UCase(company_id), vkorg As String = UCase(org_id)
            If kunnr = "" Or vkorg = "" Then Exit Sub

            Dim matnr As String = HttpContext.Current.Server.HtmlEncode(part_no.Trim().ToUpper())
            Dim vbeln As String = HttpContext.Current.Server.HtmlEncode(so_no.Trim().ToUpper())
            Dim bstnk As String = HttpContext.Current.Server.HtmlEncode(po_no.Trim().ToUpper())
            Dim FromDate As String = DateAdd(DateInterval.Month, -3, Now).ToString("yyyyMMdd")
            Dim ToDate As String = Now.ToString("yyyyMMdd")
            Dim tmpFrom As Date = Date.MinValue, tmpTo As Date = Date.MaxValue
            If Date.TryParseExact(order_date_from, "yyyy/MM/dd", New Globalization.CultureInfo("fr-FR"), Globalization.DateTimeStyles.None, tmpFrom) Then
                FromDate = tmpFrom.ToString("yyyyMMdd")
            End If
            If Date.TryParseExact(order_date_to, "yyyy/MM/dd", New Globalization.CultureInfo("fr-FR"), Globalization.DateTimeStyles.None, tmpTo) Then
                ToDate = tmpTo.ToString("yyyyMMdd")
            End If
            Dim sb As New System.Text.StringBuilder
            With sb
                .AppendFormat(" select VBAK.VBELN AS OrderNo, VBAK.BSTNK AS PONO, VBAK.KUNNR as BILLTOID, ")
                .AppendFormat(" (select kunnr from saprdp.vbpa where vbpa.vbeln=vbak.vbeln and vbpa.parvw='WE' and rownum=1) AS SHIPTOID, ")
                .AppendFormat(" VBAK.AUDAT AS ORDERDATE, VBAK.WAERK AS CURRENCY, cast(VBAP.POSNR as integer) AS ORDERLINE, ")
                .AppendFormat(" VBAP.MATNR AS ProductId, VBAP.KWMENG AS SchdLineConfirmQty, ")
                .AppendFormat(" VBEP.BMENG AS SchdLineOpenQty, VBAP.NETPR AS UNITPRICE, ")
                .AppendFormat(" VBAP.NETWR AS TOTALPRICE, VBUP.LFSTA AS DOC_STATUS, VBEP.EDATU AS DUEDATE, VBEP.EDATU AS OriginalDD, VBAP.ZZ_GUARA AS ExWarranty, ")
                .AppendFormat(" nvl((select cast(LFIMG as integer) from SAPRDP.LIPS where LIPS.VGBEL=VBAK.VBELN and LIPS.VGPOS=VBAP.POSNR and rownum=1),0) as SchedLineShipedQty, ")
                .AppendFormat(" cast(VBEP.ETENR as integer) as SchdLineNo, ")
                .AppendFormat(" nvl((select SUM(LFIMG) from SAPRDP.LIPS where LIPS.VGBEL=VBAK.VBELN and LIPS.VGPOS=VBAP.POSNR),0) as DLV_QTY ")
                .AppendFormat(" FROM SAPRDP.VBAK INNER JOIN SAPRDP.VBAP ON VBAK.VBELN = VBAP.VBELN INNER JOIN ")
                .AppendFormat(" SAPRDP.VBEP ON VBAP.VBELN = VBEP.VBELN AND VBAP.POSNR = VBEP.POSNR INNER JOIN ")
                .AppendFormat(" SAPRDP.VBUP ON VBAP.VBELN = VBUP.VBELN AND VBAP.POSNR = VBUP.POSNR ")
                .AppendFormat(" WHERE (VBAK.MANDT = '168') AND (VBAP.MANDT = '168') AND (VBEP.MANDT = '168') AND (VBUP.MANDT = '168')  AND ")
                .AppendFormat(" (VBAK.VKORG like '{0}%') AND ", Left(vkorg, 2))
                If kunnr <> "EKGBEC01" Then
                    .AppendFormat(" (VBAK.KUNNR = '{0}') AND ", kunnr)
                Else
                    If LCase(user_id) = "freya.huggard@ecauk.com" Then
                        .AppendFormat(" (VBAK.KUNNR in ('EKGBEC01','EKGBEC02','EKGBEC03','EKGBEC04')) AND ")
                    Else
                        .AppendFormat(" (VBAK.KUNNR = '{0}') AND ", kunnr)
                    End If
                End If

                .AppendFormat(" (VBAK.AUDAT between '{0}' and '{1}') and VBUP.LFSTA IN ('A','B') ", FromDate, ToDate)
                If matnr <> "" Then .AppendFormat(" and VBAP.MATNR like '%{0}%' ", matnr)
                If vbeln <> "" Then .AppendFormat(" and VBAK.VBELN like '%{0}%' ", vbeln)
                If bstnk <> "" Then .AppendFormat(" and VBAK.BSTNK like '%{0}%' ", bstnk)
                .AppendFormat(" and VBAP.ABGRU = ' ' ")
                .AppendFormat(" ORDER BY ORDERLINE asc, DUEDATE desc")
            End With

            'Response.Write(sb.ToString)
            Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())
            'Response.Write(sb.ToString())
            Dim BRs() As DataRow = dt.Select("DOC_STATUS='B'", "OrderNo ASC, ORDERLINE ASC, DUEDATE ASC")

            If BRs.Length > 0 Then
                Dim curSO As String = "", curLine As String = "", curQty As Decimal = 0
                For Each sch As DataRow In BRs
                    If sch.Item("OrderNo").ToString() <> curSO Or sch.Item("ORDERLINE").ToString() <> curLine Then
                        curSO = sch.Item("OrderNo").ToString() : curLine = sch.Item("ORDERLINE")
                        curQty = DirectCast(sch.Item("DLV_QTY"), Decimal)
                    End If
                    If CDbl(sch.Item("SchdLineOpenQty")) > curQty Then
                        sch.Item("SchdLineOpenQty") = sch.Item("SchdLineOpenQty") - curQty
                        curQty = 0
                    Else
                        curQty = curQty - CDbl(sch.Item("SchdLineOpenQty"))
                        sch.Delete()
                    End If
                Next
            End If
            dt.AcceptChanges()
            BRs = dt.Select("DOC_STATUS='A' and SchedLineShipedQty=0 and SchdLineNo=1")

            For Each sch As DataRow In BRs
                If dt.Select(String.Format("OrderNo='{0}' and ORDERLINE={1} and SchdLineNo>1", sch.Item("OrderNo"), sch.Item("ORDERLINE"))).Length > 0 Then
                    sch.Delete()
                End If
            Next
            dt.AcceptChanges()
            'If LCase(user_id) = "ming.zhao@advantech.com.cn" Then
            '    boDt.Merge(dt)
            '    Exit Sub
            'End If
            BRs = dt.Select("DOC_STATUS='A' and SchedLineShipedQty=0 and SchdLineNo>1 and SchdLineOpenQty=0")
            For Each sch As DataRow In BRs
                'If dt.Select(String.Format("OrderNo='{0}' and ORDERLINE={1} and SchdLineOpenQty=0", sch.Item("OrderNo"), sch.Item("ORDERLINE"))).Length > 0 Then
                sch.Delete()
                'End If
            Next
            dt.AcceptChanges()

            BRs = dt.Select("SchdLineConfirmQty=0")
            For Each sch As DataRow In BRs
                sch.Delete()
            Next
            dt.AcceptChanges()

            If org_id = "EU10" Then
                BRs = dt.Select("ORDERLINE >= 100", "OrderNo asc, ORDERLINE desc")
                If BRs.Length > 0 Then
                    Dim btoUnitSum As Double = 0, btoAllSum As Double = 0, btoOrderLine As Integer = 0
                    For Each sch As DataRow In BRs
                        If CInt(sch.Item("ORDERLINE")) <> btoOrderLine Then
                            btoOrderLine = CInt(sch.Item("ORDERLINE"))
                            '2016/11/24 ICC For AEU customers, we restricted them only can add one BTOS in each shopping cart.
                            Dim line As Integer = CInt(sch.Item("ORDERLINE"))
                            If line Mod 100 > 0 Then
                                btoUnitSum += sch.Item("UNITPRICE") : btoAllSum += sch.Item("TOTALPRICE")
                                sch.Delete()
                            Else
                                sch.Item("UNITPRICE") = btoUnitSum : sch.Item("TOTALPRICE") = btoAllSum
                                btoUnitSum = 0 : btoAllSum = 0
                            End If
                        Else
                            sch.Delete()
                        End If
                    Next
                End If
                dt.AcceptChanges()
            End If

            boDt.Merge(dt)
            'Nada 20140124 remove first sch line
            If Not IsNothing(boDt) AndAlso boDt.Rows.Count > 0 Then
                Dim isnextfirstline As Boolean = True
                Dim iscurrentfirstline As Boolean = False
                Dim ishassucc As Boolean = False
                Dim iszeroq As Boolean = False
                For i As Integer = boDt.Rows.Count - 1 To 1 Step -1
                    iscurrentfirstline = isnextfirstline
                    If boDt.Rows(i).Item("SchdLineOpenQty") = 0 Then
                        iszeroq = True
                    Else
                        iszeroq = False
                    End If
                    If boDt.Rows(i - 1).Item("OrderNo") = boDt.Rows(i).Item("OrderNo") AndAlso boDt.Rows(i - 1).Item("OrderLine") = boDt.Rows(i).Item("OrderLine") Then
                        ishassucc = True
                    Else
                        ishassucc = False
                        isnextfirstline = True
                    End If
                    If (Not iscurrentfirstline AndAlso iszeroq) OrElse (iscurrentfirstline AndAlso iszeroq AndAlso ishassucc) Then
                        boDt.Rows.Remove(boDt.Rows(i))
                    End If
                Next
            End If
            boDt.AcceptChanges()
            '/Nada 20140124
        Catch ex As Exception
            'Response.Write(ex.ToString())
        End Try
    End Sub
    Public Shared Sub GetBackOrderC(ByRef boDt As DataTable, ByVal company_id As String, ByVal org_id As String, ByVal part_no As String, ByVal so_no As String, ByVal po_no As String, ByVal order_date_from As String, ByVal order_date_to As String, ByVal user_id As String)
        Try
            Dim kunnr As String = UCase(company_id), vkorg As String = UCase(org_id)
            If kunnr = "" Or vkorg = "" Then Exit Sub

            Dim matnr As String = HttpContext.Current.Server.HtmlEncode(part_no.Trim().ToUpper())
            Dim vbeln As String = HttpContext.Current.Server.HtmlEncode(so_no.Trim().ToUpper())
            Dim bstnk As String = HttpContext.Current.Server.HtmlEncode(po_no.Trim().ToUpper())
            Dim FromDate As String = DateAdd(DateInterval.Month, -3, Now).ToString("yyyyMMdd")
            Dim ToDate As String = Now.ToString("yyyyMMdd")
            Dim tmpFrom As Date = Date.MinValue, tmpTo As Date = Date.MaxValue
            If Date.TryParseExact(order_date_from, "yyyy/MM/dd", New Globalization.CultureInfo("fr-FR"), Globalization.DateTimeStyles.None, tmpFrom) Then
                FromDate = tmpFrom.ToString("yyyyMMdd")
            End If
            If Date.TryParseExact(order_date_to, "yyyy/MM/dd", New Globalization.CultureInfo("fr-FR"), Globalization.DateTimeStyles.None, tmpTo) Then
                ToDate = tmpTo.ToString("yyyyMMdd")
            End If
            Dim sb As New System.Text.StringBuilder
            With sb
                .AppendFormat(" select VBAK.VBELN AS OrderNo, VBAK.BSTNK AS PONO, VBAK.KUNNR as BILLTOID, ")
                .AppendFormat(" (select kunnr from saprdp.vbpa where vbpa.vbeln=vbak.vbeln and vbpa.parvw='WE' and rownum=1) AS SHIPTOID, ")
                .AppendFormat(" VBAK.AUDAT AS ORDERDATE, VBAK.WAERK AS CURRENCY, cast(VBAP.POSNR as integer) AS ORDERLINE, ")
                .AppendFormat(" VBAP.MATNR AS ProductId, VBAP.KWMENG AS SchdLineConfirmQty, ")
                .AppendFormat(" VBEP.BMENG AS SchdLineOpenQty, VBAP.NETPR AS UNITPRICE, ")
                .AppendFormat(" VBAP.NETWR AS TOTALPRICE, VBUP.LFSTA AS DOC_STATUS, VBEP.EDATU AS DUEDATE, VBEP.EDATU AS OriginalDD, VBAP.ZZ_GUARA AS ExWarranty, ")
                .AppendFormat(" (select cast(LFIMG as integer) from SAPRDP.LIPS where LIPS.VGBEL=VBAK.VBELN and LIPS.VGPOS=VBAP.POSNR and rownum=1) as SchedLineShipedQty, ")
                .AppendFormat(" cast(VBEP.ETENR as integer) as SchdLineNo, ")
                .AppendFormat(" nvl((select count(*) as n from SAPRDP.VBRP where VBRP.AUBEL = VBAK.VBELN and VBRP.AUPOS=VBAP.POSNR),0) as DLV_QTY ")
                .AppendFormat(" FROM SAPRDP.VBAK INNER JOIN SAPRDP.VBAP ON VBAK.VBELN = VBAP.VBELN INNER JOIN ")
                .AppendFormat(" SAPRDP.VBEP ON VBAP.VBELN = VBEP.VBELN AND VBAP.POSNR = VBEP.POSNR INNER JOIN ")
                .AppendFormat(" SAPRDP.VBUP ON VBAP.VBELN = VBUP.VBELN AND VBAP.POSNR = VBUP.POSNR ")
                .AppendFormat(" WHERE (VBAK.MANDT = '168') AND (VBAP.MANDT = '168') AND (VBEP.MANDT = '168') AND (VBUP.MANDT = '168')  AND ")
                .AppendFormat(" (VBAK.VKORG like '{0}%') AND ", Left(vkorg, 2))
                If kunnr <> "EKGBEC01" Then
                    .AppendFormat(" (VBAK.KUNNR = '{0}') AND ", kunnr)
                Else
                    If LCase(user_id) = "freya.huggard@ecauk.com" Then
                        .AppendFormat(" (VBAK.KUNNR in ('EKGBEC01','EKGBEC02','EKGBEC03','EKGBEC04')) AND ")
                    Else
                        .AppendFormat(" (VBAK.KUNNR = '{0}') AND ", kunnr)
                    End If
                End If
                .AppendFormat(" (VBAK.AUDAT between '{0}' and '{1}') and VBUP.LFSTA ='C' ", FromDate, ToDate)
                If matnr <> "" Then .AppendFormat(" and VBAP.MATNR like '%{0}%' ", matnr)
                If vbeln <> "" Then .AppendFormat(" and VBAK.VBELN like '%{0}%' ", vbeln)
                If bstnk <> "" Then .AppendFormat(" and VBAK.BSTNK like '%{0}%' ", bstnk)
                .AppendFormat(" and VBAP.ABGRU = ' ' ")
                .AppendFormat(" ORDER BY ORDERLINE asc, DUEDATE desc")
            End With
            'Response.Write(sb.ToString())
            'If Session("user_id") = "rudy.wang@advantech.com.tw" Then Response.Write(sb.ToString)
            Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString())

            For Each r As DataRow In dt.Rows
                If CInt(r.Item("DLV_QTY")) > 0 Then
                    r.Delete()
                End If
            Next
            dt.AcceptChanges()

            Dim BRs() As DataRow
            If org_id = "EU10" Then
                BRs = dt.Select("ORDERLINE >= 100", "OrderNo asc, ORDERLINE desc")
                If BRs.Length > 0 Then
                    Dim btoUnitSum As Double = 0, btoAllSum As Double = 0, btoOrderLine As Integer = 0
                    For Each sch As DataRow In BRs
                        If CInt(sch.Item("ORDERLINE")) <> btoOrderLine Then
                            btoOrderLine = CInt(sch.Item("ORDERLINE"))
                            If CInt(sch.Item("ORDERLINE")) > 100 Then
                                btoUnitSum += sch.Item("UNITPRICE") : btoAllSum += sch.Item("TOTALPRICE")
                                sch.Delete()
                            Else
                                sch.Item("UNITPRICE") = btoUnitSum : sch.Item("TOTALPRICE") = btoAllSum
                                btoUnitSum = 0 : btoAllSum = 0
                            End If
                        Else
                            sch.Delete()
                        End If
                    Next
                End If
                dt.AcceptChanges()
            End If

            BRs = dt.Select("SchdLineConfirmQty=0")
            For Each sch As DataRow In BRs
                sch.Delete()
            Next
            dt.AcceptChanges()

            'Dim part_no As String = ""
            'For Each row As DataRow In dt.Rows
            '    If row.Item("ProductId") <> part_no Then
            '        part_no = row.Item("ProductId")
            '    Else
            '        row.Delete()
            '    End If
            'Next
            'dt.AcceptChanges()

            boDt.Merge(dt)
            'Nada 20140124 remove first sch line
            If Not IsNothing(boDt) AndAlso boDt.Rows.Count > 0 Then
                Dim isnextfirstline As Boolean = True
                Dim iscurrentfirstline As Boolean = False
                Dim ishassucc As Boolean = False
                Dim iszeroq As Boolean = False
                For i As Integer = boDt.Rows.Count - 1 To 1 Step -1
                    iscurrentfirstline = isnextfirstline
                    If boDt.Rows(i).Item("SchdLineOpenQty") = 0 Then
                        iszeroq = True
                    Else
                        iszeroq = False
                    End If
                    If boDt.Rows(i - 1).Item("OrderNo") = boDt.Rows(i).Item("OrderNo") AndAlso boDt.Rows(i - 1).Item("OrderLine") = boDt.Rows(i).Item("OrderLine") Then
                        ishassucc = True
                    Else
                        ishassucc = False
                        isnextfirstline = True
                    End If
                    If (Not iscurrentfirstline AndAlso iszeroq) OrElse (iscurrentfirstline AndAlso iszeroq AndAlso ishassucc) Then
                        boDt.Rows.Remove(boDt.Rows(i))
                    End If
                Next
            End If
            boDt.AcceptChanges()
            '/Nada 20140124
        Catch ex As Exception
            'Response.Write(ex.ToString())
        End Try
    End Sub
    Public Shared Function GetMultiATP_New(ByVal strXML As String, ByRef strResult As String, ByRef strRemark As String) As Integer
        strRemark = ""
        strResult = ""

        'read ADOXML to dataset
        Dim reader1 As New StringReader(strXML)
        Dim set1 As New DataSet
        set1.ReadXml(reader1)
        Dim zssD_08Table1_New As New GetMultiATP.ZSSD_08Table
        Dim zssD_04Table1_New As New GetMultiATP.ZSSD_04Table
        Dim proxy1 As New GetMultiATP.GetMultiATP
        Try

            'Set Connection information
            'Me.destination1 = New Destination
            'Me.destination1.AppServerHost = "172.20.1.176"
            'Me.destination1.Client = "168"
            'Me.destination1.Language = ""
            'Me.destination1.Password = "ebizaeu"
            'Me.destination1.SystemNumber = "5"
            'Me.destination1.Username = "b2baeu"

            proxy1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD").ToString)
            proxy1.Connection.Open()

            'retrive data from ADOXML string
            Dim table1 As DataTable = set1.Tables.Item("row")
            Dim num1 As Integer


            'set RFC parameter
            For num1 = 0 To table1.Rows.Count - 1
                Dim zssd_1 As New GetMultiATP.ZSSD_08
                zssd_1.Werks = table1.Rows.Item(num1).Item("WERK").ToString
                zssd_1.Matnr = table1.Rows.Item(num1).Item("MATNR").ToString
                zssd_1.Req_Qty = Decimal.Parse(table1.Rows.Item(num1).Item("REQ_QTY").ToString)
                zssd_1.Req_Date = table1.Rows.Item(num1).Item("REQ_Date").ToString
                zssd_1.Unit = table1.Rows.Item(num1).Item("UNI").ToString
                zssD_08Table1_New.Add(zssd_1)
            Next num1

            proxy1.Z_Sd_So_Atpinquiry(CDec(35), zssD_08Table1_New, zssD_04Table1_New)

            proxy1.Connection.Close()

            'datetime formate transform
            Dim info1 As New CultureInfo("en-US")
            Dim num2 As Integer
            For num2 = 0 To zssD_04Table1_New.Count - 1
                Dim provider1 As New CultureInfo("fr-FR", True)
                Dim time1 As DateTime = DateTime.ParseExact(zssD_04Table1_New.Item(num2).Due_Date, "yyyyMMdd", provider1)
                '  Me.zssD_04Table1.Item(num2).Due_Date = time1.ToString("yyyy-MM-ddTHH:mm:ss")
                zssD_04Table1_New.Item(num2).Due_Date = time1.ToString("yyyy-MM-dd")

            Next num2

            'field name mapping
            Dim table2 As DataTable = ATPTableFieldMapping_New(zssD_04Table1_New)

            'transform to ADOXML
            strResult = DataTableToADOXML(table2)

        Catch exception1 As Exception
            '            Dim table1 As DataTable = Me.ATPTableFieldMapping(Me.zssD_04Table1)
            '           strResult = Me.DataTableToADOXML(table1)

            proxy1.Connection.Close()
            strRemark = exception1.ToString
            Return -1
        End Try

        Return 0

    End Function

    Public Shared Function GetMultiATP_Newbytable(ByVal partIn As DataTable, ByRef partOut As DataTable, ByRef strRemark As String) As Integer
        strRemark = ""
        'strResult = ""

        'read ADOXML to dataset
        'Dim reader1 As New StringReader(strXML)
        'Dim set1 As New DataSet
        'set1.ReadXml(reader1)
        Dim zssD_08Table1_New As New GetMultiATP.ZSSD_08Table
        Dim zssD_04Table1_New As New GetMultiATP.ZSSD_04Table
        Dim proxy1 As New GetMultiATP.GetMultiATP
        Try

            'Set Connection information
            'Me.destination1 = New Destination
            'Me.destination1.AppServerHost = "172.20.1.176"
            'Me.destination1.Client = "168"
            'Me.destination1.Language = ""
            'Me.destination1.Password = "ebizaeu"
            'Me.destination1.SystemNumber = "5"
            'Me.destination1.Username = "b2baeu"

            proxy1.Connection = New SAPConnection(ConfigurationManager.AppSettings("SAP_PRD").ToString)
            proxy1.Connection.Open()

            'retrive data from ADOXML string
            Dim table1 As DataTable = partIn
            Dim num1 As Integer


            'set RFC parameter
            For num1 = 0 To table1.Rows.Count - 1
                Dim zssd_1 As New GetMultiATP.ZSSD_08
                zssd_1.Werks = table1.Rows.Item(num1).Item("WERK").ToString
                zssd_1.Matnr = table1.Rows.Item(num1).Item("MATNR").ToString
                zssd_1.Req_Qty = Decimal.Parse(table1.Rows.Item(num1).Item("REQ_QTY").ToString)
                zssd_1.Req_Date = CDate(table1.Rows.Item(num1).Item("REQ_Date")).ToString("yyyy-MM-dd")
                zssd_1.Unit = table1.Rows.Item(num1).Item("UNI").ToString
                zssD_08Table1_New.Add(zssd_1)
            Next num1

            proxy1.Z_Sd_So_Atpinquiry(CDec(35), zssD_08Table1_New, zssD_04Table1_New)

            proxy1.Connection.Close()

            'datetime formate transform
            Dim info1 As New CultureInfo("en-US")
            Dim num2 As Integer
            For num2 = 0 To zssD_04Table1_New.Count - 1
                Dim provider1 As New CultureInfo("fr-FR", True)

                If Date.TryParseExact(zssD_04Table1_New.Item(num2).Due_Date, "yyyyMMdd", provider1, DateTimeStyles.None, Now) Then
                    Dim time1 As DateTime = DateTime.ParseExact(zssD_04Table1_New.Item(num2).Due_Date, "yyyyMMdd", provider1)
                    '  Me.zssD_04Table1.Item(num2).Due_Date = time1.ToString("yyyy-MM-ddTHH:mm:ss")
                    zssD_04Table1_New.Item(num2).Due_Date = time1.ToString("yyyy-MM-dd")
                Else
                    zssD_04Table1_New.Item(num2).Due_Date = Now.ToString("yyyy-MM-dd")
                End If



            Next num2

            'field name mapping
            Dim table2 As DataTable = ATPTableFieldMapping_New(zssD_04Table1_New)

            'transform to ADOXML
            partOut = table2

        Catch exception1 As Exception
            '            Dim table1 As DataTable = Me.ATPTableFieldMapping(Me.zssD_04Table1)
            '           strResult = Me.DataTableToADOXML(table1)

            proxy1.Connection.Close()
            strRemark = exception1.ToString
            Return -1
        End Try

        Return 0

    End Function

    Public Shared Function ATPTableFieldMapping_New(ByVal zssd_dt As GetMultiATP.ZSSD_04Table) As DataTable
        Dim table1 As DataTable = zssd_dt.ToADODataTable
        table1.Columns.Item("Vkorg").ColumnName = "entity"
        table1.Columns.Item("Matnr").ColumnName = "part"
        table1.Columns.Item("Werks").ColumnName = "site"
        table1.Columns.Item("Flag").ColumnName = "flag"
        ' table1.Columns.Add("date", Type.GetType("System.DateTime"))
        table1.Columns.Item("Due_Date").ColumnName = "date"
        table1.Columns.Item("Type").ColumnName = "type"
        table1.Columns.Item("Qty_Atb").ColumnName = "qty_atb"
        table1.Columns.Item("Av_Qty_Plt").ColumnName = "qty_atp"
        table1.Columns.Item("Menge").ColumnName = "qty_req"
        table1.Columns.Add("flag_scm", Type.GetType("System.Int32"))
        table1.Columns.Item("flag_scm").DefaultValue = 0
        table1.Columns.Add("due_date", Type.GetType("System.DateTime"))
        table1.Columns.Item("due_date").DefaultValue = "1999/12/31 12:00:00"
        table1.Columns.Add("due_date_scm", Type.GetType("System.DateTime"))
        table1.Columns.Item("due_date_scm").DefaultValue = "1999/12/31 12:00:00"
        table1.Columns.Add("atp_date_scm", Type.GetType("System.DateTime"))
        table1.Columns.Item("atp_date_scm").DefaultValue = "1999/12/31 12:00:00"
        Return table1
    End Function
    Public Shared Function DataTableToADOXML(ByVal dt As DataTable) As String
        Dim class1 As New DOMDocumentClass

        Dim class2 As New RecordsetClass

        Dim num1 As Integer
        For num1 = 0 To dt.Columns.Count - 1
            Select Case dt.Columns.Item(num1).DataType.ToString
                Case "System.Int16"
                    class2.Fields.Append(dt.Columns.Item(num1).ColumnName, DataTypeEnum.adSmallInt, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
                    GoTo Label_02E5
                Case "System.SByte"
                    class2.Fields.Append(dt.Columns.Item(num1).ColumnName, DataTypeEnum.adTinyInt, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
                    GoTo Label_02E5
                Case "System.Int32"
                    class2.Fields.Append(dt.Columns.Item(num1).ColumnName, DataTypeEnum.adInteger, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
                    GoTo Label_02E5
                Case "System.Int64"
                    class2.Fields.Append(dt.Columns.Item(num1).ColumnName, DataTypeEnum.adBigInt, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
                    GoTo Label_02E5
                Case "System.Single"
                    class2.Fields.Append(dt.Columns.Item(num1).ColumnName, DataTypeEnum.adSingle, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
                    GoTo Label_02E5
                Case "System.Double"
                    class2.Fields.Append(dt.Columns.Item(num1).ColumnName, DataTypeEnum.adDouble, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
                    GoTo Label_02E5
                Case "System.Decimal"
                    'class2.Fields.Append(dt.Columns.Item(num1).ColumnName, DataTypeEnum.adDecimal, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
                    class2.Fields.Append(dt.Columns.Item(num1).ColumnName, DataTypeEnum.adCurrency, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
                    GoTo Label_02E5
                Case "System.DateTime"
                    class2.Fields.Append(dt.Columns.Item(num1).ColumnName, DataTypeEnum.adDate, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
                    GoTo Label_02E5
                Case "System.Object"
                    class2.Fields.Append(dt.Columns.Item(num1).ColumnName, DataTypeEnum.adVariant, 0, FieldAttributeEnum.adFldUnspecified, Missing.Value)
                    GoTo Label_02E5
                Case "System.String"
                    class2.Fields.Append(dt.Columns.Item(num1).ColumnName, DataTypeEnum.adVarChar, 255, FieldAttributeEnum.adFldUnspecified, Missing.Value)
                    GoTo Label_02E5
                Case Else
                    GoTo Label_02E5
            End Select

Label_02E5:
        Next num1
        class2.CursorLocation = CursorLocationEnum.adUseClient
        class2.Open(Missing.Value, Missing.Value, CursorTypeEnum.adOpenUnspecified, LockTypeEnum.adLockUnspecified, -1)
        Dim num2 As Integer
        For num2 = 0 To dt.Rows.Count - 1
            class2.AddNew(Missing.Value, Missing.Value)
            Dim num3 As Integer
            For num3 = 0 To dt.Columns.Count - 1
                Try

                    If (dt.Rows.Item(num2).Item(dt.Columns.Item(num3).ColumnName) Is DBNull.Value) Then
                        class2.Fields.Item(dt.Columns.Item(num3).ColumnName).Value = dt.Columns.Item(num3).DefaultValue
                    Else
                        class2.Fields.Item(dt.Columns.Item(num3).ColumnName).Value = dt.Rows.Item(num2).Item(dt.Columns.Item(num3).ColumnName)
                    End If

                Catch ex As Exception

                    'class2.Fields.Item(dt.Columns.Item(num3).ColumnName).Value = ""

                End Try

            Next num3
        Next num2
        class2.Save(class1, PersistFormatEnum.adPersistXML)
        Return class1.xml
    End Function
End Class


<Serializable()> _
Public Class DimCompanySet
    Private _company As List(Of SAP_DIMCOMPANY) = Nothing
    Public Property Company As List(Of SAP_DIMCOMPANY)
        Get
            Return _company
        End Get
        Set(ByVal value As List(Of SAP_DIMCOMPANY))
            _company = value
        End Set
    End Property
    Private _partner As List(Of SAP_COMPANY_PARTNER) = Nothing
    Public Property Partner As List(Of SAP_COMPANY_PARTNER)
        Get
            Return _partner
        End Get
        Set(ByVal value As List(Of SAP_COMPANY_PARTNER))
            _partner = value
        End Set
    End Property
    Private _contact As List(Of SAP_COMPANY_CONTACT) = Nothing
    Public Property Contact As List(Of SAP_COMPANY_CONTACT)
        Get
            Return _contact
        End Get
        Set(ByVal value As List(Of SAP_COMPANY_CONTACT))
            _contact = value
        End Set
    End Property
    Private _salesdef As List(Of SAP_COMPANY_SALES_DEF) = Nothing
    Public Property Salesdef As List(Of SAP_COMPANY_SALES_DEF)
        Get
            Return _salesdef
        End Get
        Set(ByVal value As List(Of SAP_COMPANY_SALES_DEF))
            _salesdef = value
        End Set
    End Property
    Private _employee As List(Of SAP_COMPANY_EMPLOYEE) = Nothing
    Public Property Employee As List(Of SAP_COMPANY_EMPLOYEE)
        Get
            Return _employee
        End Get
        Set(ByVal value As List(Of SAP_COMPANY_EMPLOYEE))
            _employee = value
        End Set
    End Property
End Class
Public Class syncSingleCompany
    Private Shared Function getSAPCompany(ByVal comStr As ArrayList, ByVal conn As Oracle.DataAccess.Client.OracleConnection) As List(Of SAP_DIMCOMPANY)
        Dim comCond As String = ""
        If Not IsNothing(comStr) AndAlso comStr.Count > 0 Then
            comCond = " and knvv.kunnr in ('" & String.Join("','", comStr.ToArray) & "')"
        End If
        Dim validOrg As String = ConfigurationManager.AppSettings("InvalidOrg")
        validOrg = validOrg.Replace(",'TW02'", "") 'ICC TW02 also has to be synced 2014/11/28
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" select kna1.kunnr as Company_Id, "))
            .AppendLine(String.Format(" 	   knvv.vkorg as org_id, "))
            .AppendLine(String.Format("     (select MIN(knvp.kunnr) from saprdp.knvp where knvp.kunn2 = kna1.kunnr and knvp.vkorg=knvv.vkorg AND knvp.parvw='WE') as ParentCompanyId, "))
            .AppendLine(String.Format(" 		kna1.name1 || kna1.name2 as Company_Name, "))
            .AppendLine(String.Format(" 		adrc.street || adrc.str_suppl3 || adrc.location as Address, "))
            .AppendLine(String.Format(" 		kna1.telfx as fax_no, "))
            .AppendLine(String.Format(" 		kna1.telf1 as tel_no, "))
            .AppendLine(String.Format(" 		kna1.ktokd as company_type, "))
            .AppendLine(String.Format(" 		kna1.kdkg1 || kna1.kdkg2 || kna1.kdkg3 || kna1.kdkg4 as price_class,  "))
            .AppendLine(String.Format("     '' as ptrade_price_class, "))
            .AppendLine(String.Format(" 		knvv.waers as Currency, "))
            .AppendLine(String.Format(" 		adrc.country as Country,  "))
            .AppendLine(String.Format("     '' as region, "))
            .AppendLine(String.Format(" 		adrc.post_code1 as Zip_Code, "))
            .AppendLine(String.Format(" 		adrc.city1 as City, "))
            .AppendLine(String.Format(" 		adrc.name_co as Attention, "))
            .AppendLine(String.Format(" 		'0' as Credit_Limit, "))
            .AppendLine(String.Format(" 		knvv.zterm as Credit_Term, "))
            .AppendLine(String.Format(" 		knvv.inco1 || '  ' || knvv.inco2 as Ship_Via, "))
            .AppendLine(String.Format(" 		kna1.knurl as Url,  "))
            .AppendLine(String.Format("     '' as LAST_UPDATED,  "))
            .AppendLine(String.Format("     '' as UPDATED_BY,  "))
            .AppendLine(String.Format(" 		kna1.erdat as CREATED_DATE, "))
            .AppendLine(String.Format(" 		kna1.ernam as Created_By, "))
            .AppendLine(String.Format(" 		knvv.kdgrp as Company_Price_Type,	 "))
            .AppendLine(String.Format("     '' as SALES_USERID,	 "))
            .AppendLine(String.Format(" 		knvv.vsbed as SHIP_CONDITION, "))
            .AppendLine(String.Format(" 		kna1.KATR4 as attribute4, "))
            .AppendLine(String.Format(" 		KNVV.VKBUR as SalesOffice, "))
            .AppendLine(String.Format("     KNVV.VKGRP as SalesGroup, "))

            .AppendLine(String.Format("     (select c.vlibb from saprdp.knb1 c where c.mandt='168' and c.kunnr=kna1.kunnr and rownum=1 and c.vlibb>0),null,0.0,(select c.vlibb from saprdp.knb1 c where c.mandt='168' and c.kunnr=kna1.kunnr and rownum=1 and c.vlibb>0) as amt_insured, "))
            .AppendLine(String.Format("     (select c.klimk from saprdp.knkk c where c.mandt='168' and c.kunnr=kna1.kunnr and rownum=1 and c.klimk>0),null,0.0,(select c.klimk from saprdp.knkk c where c.mandt='168' and c.kunnr=kna1.kunnr and rownum=1 and c.klimk>0) as credit_limit, "))
            .AppendLine(String.Format("     (select z.smtp_addr from saprdp.adr6 z where z.addrnumber=kna1.adrnr and z.client='168' and rownum=1),null,'',(select z.smtp_addr from saprdp.adr6 z where z.addrnumber=kna1.adrnr and z.client='168' and rownum=1) as contact_email, "))
            .AppendLine(String.Format("     kna1.loevm as deletion_flag, "))
            .AppendLine(String.Format("     kna1.KATR9 as CUST_IND, "))
            .AppendLine(String.Format("     (select z.ddtext from saprdp.dd07t z where z.domname='ZZ_KATR9' and z.ddlanguage='E' and z.domvalue_l=kna1.KATR9 and rownum=1) as VM, "))
            .AppendLine(String.Format("     knvv.KONDA as price_grp, "))
            .AppendLine(String.Format("     knvv.PLTYP as price_list, "))
            .AppendLine(String.Format("     knvv.inco1,knvv.inco2 ,knvv.zterm , "))
            .AppendLine(String.Format("    (select VTEXT from saprdp.tvzbt where tvzbt.zterm=knvv.zterm and rownum=1) as paymentTermName, "))

            .AppendLine(String.Format(" (select c.landx from saprdp.t005t c where c.land1=kna1.land1 and c.spras='E' and rownum=1) as country_name,"))
            .AppendLine(String.Format(" (select KNVI.TAXKD from saprdp.KNVI where KNVI.kunnr=kna1.kunnr and KNVI.ALAND = 'NL' and KNVI.TATYP = 'MWST' and KNVI.mandt='168' and rownum=1) as TAX_CLASS,kna1.REGIO as RegCode "))
            .AppendLine(String.Format(" from saprdp.knvv inner join saprdp.kna1 on knvv.kunnr=kna1.kunnr  "))
            .AppendLine(String.Format(" 	inner join saprdp.adrc on kna1.adrnr=adrc.addrnumber and kna1.land1=adrc.country and adrc.NATION=' ' and adrc.client='168' "))
            .AppendLine(String.Format(" where knvv.mandt='168'  and kna1.loevm = ' ' and knvv.vkorg not in {1} {0} ", comCond, validOrg))
            .AppendLine(String.Format(" and kna1.ktokd in ('Z001','Z002','Z003') "))
        End With

        Dim da As New Oracle.DataAccess.Client.OracleDataAdapter(sb.ToString, conn)
        Dim DT As New DataTable
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            Dim CompL As New List(Of SAP_DIMCOMPANY)
            For Each Row As DataRow In DT.Rows
                Dim Comp As New SAP_DIMCOMPANY
                Comp.UNIQUE_ID = ""
                If Not IsDBNull(Row.Item("COMPANY_ID")) Then
                    Comp.COMPANY_ID = Row.Item("COMPANY_ID")
                End If
                If Not IsDBNull(Row.Item("ORG_ID")) Then
                    Comp.ORG_ID = Row.Item("ORG_ID")
                End If
                If Not IsDBNull(Row.Item("PARENTCOMPANYID")) Then
                    Comp.PARENTCOMPANYID = Row.Item("PARENTCOMPANYID")
                End If
                If Not IsDBNull(Row.Item("COMPANY_NAME")) Then
                    Comp.COMPANY_NAME = Row.Item("COMPANY_NAME")
                End If
                If Not IsDBNull(Row.Item("ADDRESS")) Then
                    Comp.ADDRESS = Row.Item("ADDRESS")
                End If
                If Not IsDBNull(Row.Item("FAX_NO")) Then
                    Comp.FAX_NO = Row.Item("FAX_NO")
                End If
                If Not IsDBNull(Row.Item("TEL_NO")) Then
                    Comp.TEL_NO = Row.Item("TEL_NO")
                End If
                If Not IsDBNull(Row.Item("COMPANY_TYPE")) Then
                    Comp.COMPANY_TYPE = Row.Item("COMPANY_TYPE")
                End If
                If Not IsDBNull(Row.Item("PRICE_CLASS")) Then
                    Comp.PRICE_CLASS = Row.Item("PRICE_CLASS")
                End If
                If Not IsDBNull(Row.Item("CURRENCY")) Then
                    Comp.CURRENCY = Row.Item("CURRENCY")
                End If
                If Not IsDBNull(Row.Item("COUNTRY")) Then
                    Comp.COUNTRY = Row.Item("COUNTRY")
                End If
                If Not IsDBNull(Row.Item("ZIP_CODE")) Then
                    Comp.ZIP_CODE = Row.Item("ZIP_CODE")
                End If
                If Not IsDBNull(Row.Item("CITY")) Then
                    Comp.CITY = Row.Item("CITY")
                End If
                If Not IsDBNull(Row.Item("ATTENTION")) Then
                    Comp.ATTENTION = Row.Item("ATTENTION")
                End If
                If Not IsDBNull(Row.Item("CREDIT_TERM")) Then
                    Comp.CREDIT_TERM = Row.Item("CREDIT_TERM")
                End If
                If Not IsDBNull(Row.Item("SHIP_VIA")) Then
                    Comp.SHIP_VIA = Row.Item("SHIP_VIA")
                End If
                If Not IsDBNull(Row.Item("URL")) Then
                    Comp.URL = Row.Item("URL")
                End If
                If Not IsDBNull(Row.Item("CREATED_DATE")) Then
                    Comp.CREATEDDATE = Row.Item("CREATED_DATE")
                End If
                If Not IsDBNull(Row.Item("CREATED_BY")) Then
                    Comp.CREATED_BY = Row.Item("CREATED_BY")
                End If
                If Not IsDBNull(Row.Item("COMPANY_PRICE_TYPE")) Then
                    Comp.COMPANY_PRICE_TYPE = Row.Item("COMPANY_PRICE_TYPE")
                End If
                If Not IsDBNull(Row.Item("SHIP_CONDITION")) Then
                    Comp.SHIPCONDITION = Row.Item("SHIP_CONDITION")
                End If
                If Not IsDBNull(Row.Item("ATTRIBUTE4")) Then
                    Comp.ATTRIBUTE4 = Row.Item("ATTRIBUTE4")
                End If
                If Not IsDBNull(Row.Item("SALESOFFICE")) Then
                    Comp.SALESOFFICE = Row.Item("SALESOFFICE")
                End If
                If Not IsDBNull(Row.Item("SALESGROUP")) Then
                    Comp.SALESGROUP = Row.Item("SALESGROUP")
                End If
                If Not IsDBNull(Row.Item("amt_insured")) Then
                    If Decimal.TryParse(Row.Item("amt_insured"), 0) Then
                        Comp.AMT_INSURED = CDec(Row.Item("amt_insured"))
                    Else
                        Comp.AMT_INSURED = 0
                    End If
                End If
                If Not IsDBNull(Row.Item("credit_limit")) Then
                    If Decimal.TryParse(Row.Item("credit_limit"), 0) Then
                        Comp.CREDIT_LIMIT = CDec(Row.Item("credit_limit"))
                    Else
                        Comp.CREDIT_LIMIT = 0
                    End If
                End If
                If Not IsDBNull(Row.Item("contact_email")) Then
                    Comp.CONTACT_EMAIL = Row.Item("contact_email")
                End If
                If Not IsDBNull(Row.Item("paymentTermName")) Then
                    Comp.PAYMENT_TERM_NAME = Row.Item("paymentTermName")
                End If
                If Not IsDBNull(Row.Item("deletion_flag")) Then
                    Comp.DELETION_FLAG = Row.Item("deletion_flag")
                End If
                If Not IsDBNull(Row.Item("CUST_IND")) Then
                    Comp.CUST_IND = Row.Item("CUST_IND")
                End If
                If Not IsDBNull(Row.Item("VM")) Then
                    Comp.VM = Row.Item("VM")
                End If
                If Not IsDBNull(Row.Item("price_grp")) Then
                    Comp.PRICE_GRP = Row.Item("price_grp")
                End If
                If Not IsDBNull(Row.Item("price_list")) Then
                    Comp.PRICE_LIST = Row.Item("price_list")
                End If
                If Not IsDBNull(Row.Item("inco1")) Then
                    Comp.INCO1 = Row.Item("inco1")
                End If
                If Not IsDBNull(Row.Item("inco2")) Then
                    Comp.INCO2 = Row.Item("inco2")
                End If
                If Not IsDBNull(Row.Item("zterm")) Then
                    Comp.PAYMENT_TERM_CODE = Row.Item("zterm")
                End If
                If Not IsDBNull(Row.Item("COUNTRY_NAME")) Then
                    Comp.COUNTRY_NAME = Row.Item("COUNTRY_NAME")
                End If
                If Not IsDBNull(Row.Item("RegCode")) Then
                    Comp.REGION_CODE = Row.Item("RegCode")
                End If
                CompL.Add(Comp)
            Next
            Return CompL
        End If
        Return Nothing
    End Function
    Public Shared Function getSAPPartner(ByVal comStr As ArrayList, ByVal conn As Oracle.DataAccess.Client.OracleConnection) As List(Of SAP_COMPANY_PARTNER)
        Dim comCond As String = ""
        If Not IsNothing(comStr) AndAlso comStr.Count > 0 Then
            comCond = " and b.KUNNR in ('" & String.Join("','", comStr.ToArray) & "')"
        End If
        Dim sb As New System.Text.StringBuilder
        With sb
            sb.AppendLine(" select b.kunnr as COMPANY_ID, b.vkorg as ORG_ID, b.vtweg as DIST_CHANN,  ")
            sb.AppendLine(" b.spart as DIVISION, b.parvw as PARTNER_FUNCTION, b.kunn2 as PARENT_COMPANY_ID,  ")
            sb.AppendLine(" b.lifnr as VENDOR_CREDITOR, b.pernr as SALES_CODE, b.parnr as PARTNER_NUMBER, b.KNREF, b.DEFPA ")
            sb.AppendLine(" from saprdp.kna1 a inner join saprdp.knvp b on a.kunnr=b.kunnr ")
            sb.AppendLine(" where a.mandt='168' and b.mandt='168' ")
            sb.AppendLine(" and b.vkorg not in " + ConfigurationManager.AppSettings("InvalidOrg") + comCond)
        End With

        Dim da As New Oracle.DataAccess.Client.OracleDataAdapter(sb.ToString, conn)
        Dim DT As New DataTable
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            Dim CompL As New List(Of SAP_COMPANY_PARTNER)
            For Each Row As DataRow In DT.Rows
                Dim Comp As New SAP_COMPANY_PARTNER
                If Not IsDBNull(Row.Item("COMPANY_ID")) Then
                    Comp.COMPANY_ID = Row.Item("COMPANY_ID")
                End If
                If Not IsDBNull(Row.Item("ORG_ID")) Then
                    Comp.ORG_ID = Row.Item("ORG_ID")
                End If
                If Not IsDBNull(Row.Item("DIST_CHANN")) Then
                    Comp.DIST_CHANN = Row.Item("DIST_CHANN")
                End If
                If Not IsDBNull(Row.Item("DIVISION")) Then
                    Comp.DIVISION = Row.Item("DIVISION")
                End If
                If Not IsDBNull(Row.Item("PARTNER_FUNCTION")) Then
                    Comp.PARTNER_FUNCTION = Row.Item("PARTNER_FUNCTION")
                End If
                If Not IsDBNull(Row.Item("PARENT_COMPANY_ID")) Then
                    Comp.PARENT_COMPANY_ID = Row.Item("PARENT_COMPANY_ID")
                End If
                If Not IsDBNull(Row.Item("VENDOR_CREDITOR")) Then
                    Comp.VENDOR_CREDITOR = Row.Item("VENDOR_CREDITOR")
                End If
                If Not IsDBNull(Row.Item("SALES_CODE")) Then
                    Comp.SALES_CODE = Row.Item("SALES_CODE")
                End If
                If Not IsDBNull(Row.Item("PARTNER_NUMBER")) Then
                    Comp.PARTNER_NUMBER = Row.Item("PARTNER_NUMBER")
                End If
                If Not IsDBNull(Row.Item("KNREF")) Then
                    Comp.KNREF = Row.Item("KNREF")
                End If
                If Not IsDBNull(Row.Item("DEFPA")) Then
                    Comp.DEFPA = Row.Item("DEFPA")
                End If
                CompL.Add(Comp)
            Next
            Return CompL
        End If
        Return Nothing
    End Function
    Private Shared Function getSAPContact(ByVal comStr As ArrayList, ByVal conn As Oracle.DataAccess.Client.OracleConnection) As List(Of SAP_COMPANY_CONTACT)
        Dim comCond As String = ""
        If Not IsNothing(comStr) AndAlso comStr.Count > 0 Then
            comCond = " A.KUNNR in ('" & String.Join("','", comStr.ToArray) & "')"
        End If
        Dim sb As New System.Text.StringBuilder
        With sb
            .AppendLine(String.Format(" SELECT A.KUNNR AS COMPANY_ID, B.SMTP_ADDR AS CONTACT_EMAIL, B.FLGDEFAULT, B.CONSNUMBER,  "))
            .AppendLine(String.Format(" B.HOME_FLAG, B.SMTP_SRCH, B.PERSNUMBER, C.TEL_NUMBER, C.TELNR_LONG, C.TELNR_CALL "))
            .AppendLine(String.Format(" FROM SAPRDP.KNA1 A INNER JOIN SAPRDP.ADR6 B ON A.ADRNR=B.ADDRNUMBER  "))
            .AppendLine(String.Format(" LEFT JOIN SAPRDP.ADR2 C ON B.ADDRNUMBER=C.ADDRNUMBER AND B.PERSNUMBER=C.PERSNUMBER "))
            .AppendLine(String.Format(" where {0} ", comCond))
        End With

        Dim da As New Oracle.DataAccess.Client.OracleDataAdapter(sb.ToString, conn)
        Dim DT As New DataTable
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            Dim CompL As New List(Of SAP_COMPANY_CONTACT)
            For Each Row As DataRow In DT.Rows
                Dim Comp As New SAP_COMPANY_CONTACT
                If Not IsDBNull(Row.Item("COMPANY_ID")) Then
                    Comp.COMPANY_ID = Row.Item("COMPANY_ID")
                End If
                If Not IsDBNull(Row.Item("CONTACT_EMAIL")) Then
                    Comp.CONTACT_EMAIL = Row.Item("CONTACT_EMAIL")
                End If
                If Not IsDBNull(Row.Item("FLGDEFAULT")) Then
                    Comp.FLGDEFAULT = Row.Item("FLGDEFAULT")
                End If
                If Not IsDBNull(Row.Item("CONSNUMBER")) Then
                    Comp.CONSNUMBER = Row.Item("CONSNUMBER")
                End If
                If Not IsDBNull(Row.Item("HOME_FLAG")) Then
                    Comp.HOME_FLAG = Row.Item("HOME_FLAG")
                End If
                If Not IsDBNull(Row.Item("SMTP_SRCH")) Then
                    Comp.SMTP_SRCH = Row.Item("SMTP_SRCH")
                End If
                If Not IsDBNull(Row.Item("PERSNUMBER")) Then
                    Comp.PERSNUMBER = Row.Item("PERSNUMBER")
                End If
                If Not IsDBNull(Row.Item("TEL_NUMBER")) Then
                    Comp.TEL_NUMBER = Row.Item("TEL_NUMBER")
                End If
                If Not IsDBNull(Row.Item("TELNR_LONG")) Then
                    Comp.TELNR_LONG = Row.Item("TELNR_LONG")
                End If
                If Not IsDBNull(Row.Item("TELNR_CALL")) Then
                    Comp.TELNR_CALL = Row.Item("TELNR_CALL")
                End If
                CompL.Add(Comp)
            Next
            Return CompL
        End If
        Return Nothing
    End Function
    Private Shared Function getSAPSalesdef(ByVal comStr As ArrayList, ByVal conn As Oracle.DataAccess.Client.OracleConnection) As List(Of SAP_COMPANY_SALES_DEF)

        Dim comCond As String = ""
        If Not IsNothing(comStr) AndAlso comStr.Count > 0 Then
            comCond = " and knvv.kunnr in ('" & String.Join("','", comStr.ToArray) & "')"
        End If
        Dim sb As New System.Text.StringBuilder
        With sb
            sb.AppendLine(" SELECT KNVV.KUNNR AS COMPANY_ID	, ")
            sb.AppendLine("  KNVV.VKORG	AS	SALES_ORG	, ")
            sb.AppendLine("  KNVV.VTWEG  AS  DISTR_CHAN , ")
            sb.AppendLine("  KNVV.SPART  AS  DIVISION , ")
            sb.AppendLine("  KNVV.BZIRK	AS	SALES_DISTRICT	, ")
            sb.AppendLine("  KNVV.VKBUR	AS	SALES_OFFICE	, ")
            sb.AppendLine("  KNVV.VKGRP	AS	SALES_GROUP	, ")
            sb.AppendLine("  KNVV.KDGRP	AS	CUST_GROUP	, ")
            sb.AppendLine("  KNVV.KLABC	AS	ABC_CLASS	, ")
            sb.AppendLine("  KNVV.WAERS	AS	CURRENCY	, ")
            sb.AppendLine("  KNVV.BEGRU	AS	AUTH_GROUP	, ")
            sb.AppendLine("  KNVV.EIKTO	AS	ACCT_AT_CUST	, ")
            sb.AppendLine("  KNVV.KURST	AS	EXCH_RATE_TYPE	, ")
            sb.AppendLine("  KNVV.KONDA	AS	PRICE_GROUP	, ")
            sb.AppendLine("  KNVV.KALKS	AS	CUST_PRIC_PROC	, ")
            sb.AppendLine("  KNVV.PLTYP	AS	PRICE_LIST	, ")
            sb.AppendFormat("  KNVV.VERSG AS CUST_STATS_GRP	FROM SAPRDP.KNVV where knvv.vkorg not in {1} {0}", comCond, ConfigurationManager.AppSettings("InvalidOrg"))
        End With

        Dim da As New Oracle.DataAccess.Client.OracleDataAdapter(sb.ToString, conn)
        Dim DT As New DataTable
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            Dim CompL As New List(Of SAP_COMPANY_SALES_DEF)
            For Each Row As DataRow In DT.Rows
                Dim Comp As New SAP_COMPANY_SALES_DEF
                If Not IsDBNull(Row.Item("COMPANY_ID")) Then
                    Comp.COMPANY_ID = Row.Item("COMPANY_ID")
                End If
                If Not IsDBNull(Row.Item("SALES_ORG")) Then
                    Comp.SALES_ORG = Row.Item("SALES_ORG")
                End If
                If Not IsDBNull(Row.Item("DISTR_CHAN")) Then
                    Comp.DISTR_CHAN = Row.Item("DISTR_CHAN")
                End If
                If Not IsDBNull(Row.Item("DIVISION")) Then
                    Comp.DIVISION = Row.Item("DIVISION")
                End If
                If Not IsDBNull(Row.Item("SALES_DISTRICT")) Then
                    Comp.SALES_DISTRICT = Row.Item("SALES_DISTRICT")
                End If
                If Not IsDBNull(Row.Item("SALES_OFFICE")) Then
                    Comp.SALES_OFFICE = Row.Item("SALES_OFFICE")
                End If
                If Not IsDBNull(Row.Item("SALES_GROUP")) Then
                    Comp.SALES_GROUP = Row.Item("SALES_GROUP")
                End If
                If Not IsDBNull(Row.Item("CUST_GROUP")) Then
                    Comp.CUST_GROUP = Row.Item("CUST_GROUP")
                End If
                If Not IsDBNull(Row.Item("ABC_CLASS")) Then
                    Comp.ABC_CLASS = Row.Item("ABC_CLASS")
                End If
                If Not IsDBNull(Row.Item("CURRENCY")) Then
                    Comp.CURRENCY = Row.Item("CURRENCY")
                End If
                If Not IsDBNull(Row.Item("AUTH_GROUP")) Then
                    Comp.AUTH_GROUP = Row.Item("AUTH_GROUP")
                End If
                If Not IsDBNull(Row.Item("ACCT_AT_CUST")) Then
                    Comp.ACCT_AT_CUST = Row.Item("ACCT_AT_CUST")
                End If
                If Not IsDBNull(Row.Item("EXCH_RATE_TYPE")) Then
                    Comp.EXCH_RATE_TYPE = Row.Item("EXCH_RATE_TYPE")
                End If
                If Not IsDBNull(Row.Item("PRICE_GROUP")) Then
                    Comp.PRICE_GROUP = Row.Item("PRICE_GROUP")
                End If
                If Not IsDBNull(Row.Item("CUST_PRIC_PROC")) Then
                    Comp.CUST_PRIC_PROC = Row.Item("CUST_PRIC_PROC")
                End If
                If Not IsDBNull(Row.Item("PRICE_LIST")) Then
                    Comp.PRICE_LIST = Row.Item("PRICE_LIST")
                End If
                If Not IsDBNull(Row.Item("CUST_STATS_GRP")) Then
                    Comp.CUST_STATS_GRP = Row.Item("CUST_STATS_GRP")
                End If
                CompL.Add(Comp)
            Next
            Return CompL
        End If
        Return Nothing
    End Function
    Private Shared Function getSAPEmployee(ByVal comStr As ArrayList, ByVal conn As Oracle.DataAccess.Client.OracleConnection) As List(Of SAP_COMPANY_EMPLOYEE)
        Dim comCond As String = ""
        If Not IsNothing(comStr) AndAlso comStr.Count > 0 Then
            comCond = " and knvp.kunnr in ('" & String.Join("','", comStr.ToArray) & "')"
        End If
        Dim sb As New System.Text.StringBuilder
        With sb
            sb.AppendLine(" select kunnr as company_id, kunn2 as sub_company_id, vkorg as sales_org, parvw as partner_function, ")
            sb.AppendFormat(" pernr as sales_code from saprdp.knvp where mandt='168' and pernr<>'00000000' {0} and knvp.vkorg not in {1} ", comCond, ConfigurationManager.AppSettings("InvalidOrg"))
        End With

        Dim da As New Oracle.DataAccess.Client.OracleDataAdapter(sb.ToString, conn)
        Dim DT As New DataTable
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            Dim CompL As New List(Of SAP_COMPANY_EMPLOYEE)
            For Each Row As DataRow In DT.Rows
                Dim Comp As New SAP_COMPANY_EMPLOYEE
                If Not IsDBNull(Row.Item("COMPANY_ID")) Then
                    Comp.COMPANY_ID = Row.Item("COMPANY_ID")
                End If
                If Not IsDBNull(Row.Item("sub_company_id")) Then
                    Comp.SUB_COMPANY_ID = Row.Item("sub_company_id")
                End If
                If Not IsDBNull(Row.Item("sales_org")) Then
                    Comp.SALES_ORG = Row.Item("sales_org")
                End If
                If Not IsDBNull(Row.Item("partner_function")) Then
                    Comp.PARTNER_FUNCTION = Row.Item("partner_function")
                End If
                If Not IsDBNull(Row.Item("sales_code")) Then
                    Comp.SALES_CODE = Row.Item("sales_code")
                End If
                CompL.Add(Comp)
            Next
            Return CompL
        End If
        Return Nothing
    End Function
    Public Shared Function getDimSAPComSet(ByVal comStr As ArrayList, ByVal dbDest As String) As DimCompanySet
        Dim Conn As New Oracle.DataAccess.Client.OracleConnection(ConfigurationManager.ConnectionStrings(dbDest).ConnectionString)
        Try
            Dim dimCom As DimCompanySet = Nothing
            Dim company As List(Of SAP_DIMCOMPANY) = getSAPCompany(comStr, Conn)
            Dim partner As List(Of SAP_COMPANY_PARTNER) = getSAPPartner(comStr, Conn)
            Dim contact As List(Of SAP_COMPANY_CONTACT) = getSAPContact(comStr, Conn)
            Dim salesdef As List(Of SAP_COMPANY_SALES_DEF) = getSAPSalesdef(comStr, Conn)
            Dim employee As List(Of SAP_COMPANY_EMPLOYEE) = getSAPEmployee(comStr, Conn)

            If Not IsNothing(company) AndAlso company.Count > 0 Then
                dimCom = New DimCompanySet
                dimCom.Company = company
                If Not IsNothing(partner) AndAlso partner.Count > 0 Then
                    dimCom.Partner = partner
                    If Not IsNothing(contact) AndAlso contact.Count > 0 Then
                        dimCom.Contact = contact
                    End If
                    If Not IsNothing(salesdef) AndAlso salesdef.Count > 0 Then
                        dimCom.Salesdef = salesdef
                    End If
                    If Not IsNothing(employee) AndAlso employee.Count > 0 Then
                        dimCom.Employee = employee
                    End If
                End If
            End If
            Return dimCom
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(Conn) Then
                Conn.Close()
            End If
        End Try
    End Function

    Private Shared Sub Clear(ByVal comStr As ArrayList, ByVal Context As L2SCompanyDataContext, Optional ByVal isSubmit As Boolean = True)
        If Not IsNothing(comStr) AndAlso comStr.Count > 0 Then
            Dim coL As List(Of SAP_DIMCOMPANY)
            Dim coPartnerL As List(Of SAP_COMPANY_PARTNER)
            Dim coContactL As List(Of SAP_COMPANY_CONTACT)
            Dim coEmployeeL As List(Of SAP_COMPANY_EMPLOYEE)
            Dim coSalesDefL As List(Of SAP_COMPANY_SALES_DEF)
            Dim dcoL = From X In Context.SAP_DIMCOMPANies _
                     Where comStr.ToArray.Contains(X.COMPANY_ID) _
                     Select X

            Dim dcoPartnerL = From X In Context.SAP_COMPANY_PARTNERs _
                      Where comStr.ToArray.Contains(X.COMPANY_ID) _
                      Select X

            Dim dcoContactL = From X In Context.SAP_COMPANY_CONTACTs _
                        Where comStr.ToArray.Contains(X.COMPANY_ID) _
                        Select X

            Dim dcoEmployeeL = From X In Context.SAP_COMPANY_EMPLOYEEs _
                           Where comStr.ToArray.Contains(X.COMPANY_ID) _
                           Select X

            Dim dcoSalesDefL = From X In Context.SAP_COMPANY_SALES_DEFs _
                        Where comStr.ToArray.Contains(X.COMPANY_ID) _
                        Select X

            coL = dcoL.ToList()
            coPartnerL = dcoPartnerL.ToList()
            coContactL = dcoContactL.ToList()
            coEmployeeL = dcoEmployeeL.ToList()
            coSalesDefL = dcoSalesDefL.ToList()
            If Not IsNothing(coL) AndAlso coL.Count > 0 Then
                Context.SAP_DIMCOMPANies.DeleteAllOnSubmit(coL)
            End If
            If Not IsNothing(coPartnerL) AndAlso coPartnerL.Count > 0 Then
                Context.SAP_COMPANY_PARTNERs.DeleteAllOnSubmit(coPartnerL)
            End If
            If Not IsNothing(coContactL) AndAlso coContactL.Count > 0 Then
                Context.SAP_COMPANY_CONTACTs.DeleteAllOnSubmit(coContactL)
            End If
            If Not IsNothing(coEmployeeL) AndAlso coEmployeeL.Count > 0 Then
                Context.SAP_COMPANY_EMPLOYEEs.DeleteAllOnSubmit(coEmployeeL)
            End If
            If Not IsNothing(coSalesDefL) AndAlso coSalesDefL.Count > 0 Then
                Context.SAP_COMPANY_SALES_DEFs.DeleteAllOnSubmit(coSalesDefL)
            End If
            If isSubmit = True Then
                Context.SubmitChanges()
            End If
        End If
    End Sub
    Public Shared Function syncSapCompanyContacts()
        Try
            Dim sb As New Text.StringBuilder
            With sb
                .AppendLine(String.Format(" SELECT A.KUNNR AS COMPANY_ID, B.SMTP_ADDR AS CONTACT_EMAIL, B.FLGDEFAULT, B.CONSNUMBER,  "))
                .AppendLine(String.Format(" B.HOME_FLAG, B.SMTP_SRCH, B.PERSNUMBER, C.TEL_NUMBER, C.TELNR_LONG, C.TELNR_CALL "))
                .AppendLine(String.Format(" FROM SAPRDP.KNA1 A INNER JOIN SAPRDP.ADR6 B ON A.ADRNR=B.ADDRNUMBER  "))
                .AppendLine(String.Format(" LEFT JOIN SAPRDP.ADR2 C ON B.ADDRNUMBER=C.ADDRNUMBER AND B.PERSNUMBER=C.PERSNUMBER "))
            End With
            Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                dbUtil.dbExecuteNoQuery("MY", " DELETE FROM dbo.SAP_COMPANY_CONTACTS ")
                Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
                    conn.Open()
                    Dim trans As SqlTransaction = conn.BeginTransaction()
                    Using bulkCopy As New SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, trans)
                        bulkCopy.DestinationTableName = "dbo.SAP_COMPANY_CONTACTS"
                        bulkCopy.WriteToServer(dt)
                    End Using
                    trans.Commit()
                End Using
            End If
        Catch ex As Exception
            Throw New Exception("syncSapCompanyContacts error: " + ex.Message)
        End Try
    End Function
    Public Shared Function syncSapCompanyLov()
        Try
            Dim sb As New Text.StringBuilder
            With sb
                .AppendLine(String.Format(" select distinct b.vkorg as org_id, c.vtweg as dist_chann, c.SPART as division, c.VKBUR as SalesOffice, c.VKGRP as SalesGroup, "))
                .AppendLine(String.Format(" b.vsbed as ShipTerm, (select VTEXT from saprdp.tvsbt where tvsbt.vsbed=b.vsbed and rownum=1 and tvsbt.spras='E') as shipTermName, "))
                .AppendLine(String.Format(" b.zterm as PaymentTerm, (select VTEXT from saprdp.tvzbt where tvzbt.zterm=b.zterm and rownum=1 and tvzbt.spras='E') as paymentTermName "))
                .AppendLine(String.Format(" from saprdp.knvv b inner join saprdp.vbak c on b.vkorg=c.vkorg and b.kunnr=c.kunnr where b.mandt='168' order by b.vkorg "))
            End With
            Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                dbUtil.dbExecuteNoQuery("MY", " DELETE FROM SAP_COMPANY_LOV ")
                Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
                    conn.Open()
                    Dim trans As SqlTransaction = conn.BeginTransaction()
                    Using bulkCopy As New SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, trans)
                        bulkCopy.DestinationTableName = "dbo.SAP_COMPANY_LOV"
                        bulkCopy.WriteToServer(dt)
                    End Using
                    trans.Commit()
                End Using
            End If
        Catch ex As Exception
            Throw New Exception("syncSapCompanyLov error: " + ex.Message)
        End Try
    End Function
    Public Shared Function syncSAPCompnayEXT()
        Try
            Dim sb As New Text.StringBuilder
            With sb
                .AppendLine("select KUNNR as COMPANY_ID, 'Buying Group' as TYPE, EKVBD as value  from  saprdp.KNB1 where mandt='168'")
            End With
            Dim dt As DataTable = OraDbUtil.dbGetDataTable("SAP_PRD", sb.ToString)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                dbUtil.dbExecuteNoQuery("MY", " DELETE FROM SAP_DIMCOMPANY_EXT ")
                Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
                    conn.Open()
                    Dim trans As SqlTransaction = conn.BeginTransaction()
                    Using bulkCopy As New SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, trans)
                        bulkCopy.DestinationTableName = "dbo.SAP_DIMCOMPANY_EXT"
                        bulkCopy.WriteToServer(dt)
                    End Using
                    trans.Commit()
                End Using
            End If
        Catch ex As Exception
            Throw New Exception("syncSAPCompnayEXT error: " + ex.Message)
        End Try
    End Function
    Public Shared Function syncSingleSAPCustomer(ByVal CompanyIdStr As ArrayList, ByVal isTest As Boolean, ByRef errMsg As String, Optional ByVal FailedMsg As ArrayList = Nothing) As DimCompanySet
        Dim dest As String = "SAP_PRD"
        If isTest Then
            dest = "SAP_Test"
        End If
        If IsNothing(CompanyIdStr) OrElse CompanyIdStr.Count <= 0 Then
            errMsg = "Invalid company id list." : Return New DimCompanySet
        Else
            For i As Integer = 0 To CompanyIdStr.Count - 1
                CompanyIdStr(i) = CompanyIdStr(i).ToString.ToUpper
            Next
        End If
        Dim C As DimCompanySet = Nothing
        C = getDimSAPComSet(CompanyIdStr, dest)
        If IsNothing(C) OrElse IsNothing(C.Company) Then
            errMsg = "Get all company master data failed." : Return New DimCompanySet
        End If

        For Each co As String In CompanyIdStr
            Dim comL As List(Of SAP_DIMCOMPANY) = C.Company.Where(Function(x As SAP_DIMCOMPANY) x.COMPANY_ID = co).ToList
            If IsNothing(comL) OrElse comL.Count <= 0 Then
                If Not IsNothing(FailedMsg) Then
                    FailedMsg.Add(String.Format("'{0}' sync failed,cannot get company master data correctly.", co))
                End If
                If Not IsNothing(C.Company) Then
                    C.Company.RemoveAll(Function(x As SAP_DIMCOMPANY) x.COMPANY_ID = co)
                End If
                If Not IsNothing(C.Partner) Then
                    C.Partner.RemoveAll(Function(x As SAP_COMPANY_PARTNER) x.COMPANY_ID = co)
                End If
                If Not IsNothing(C.Contact) Then
                    C.Contact.RemoveAll(Function(x As SAP_COMPANY_CONTACT) x.COMPANY_ID = co)
                End If
                If Not IsNothing(C.Employee) Then
                    C.Employee.RemoveAll(Function(x As SAP_COMPANY_EMPLOYEE) x.COMPANY_ID = co)
                End If
                If Not IsNothing(C.Salesdef) Then
                    C.Salesdef.RemoveAll(Function(x As SAP_COMPANY_SALES_DEF) x.COMPANY_ID = co)
                End If
            End If
        Next
        Using Context As New L2SCompanyDataContext
            If Not IsNothing(C.Company) AndAlso C.Company.Count > 0 Then
                Context.SAP_DIMCOMPANies.InsertAllOnSubmit(C.Company)

                If Not IsNothing(C.Partner) AndAlso C.Partner.Count > 0 Then
                    Context.SAP_COMPANY_PARTNERs.InsertAllOnSubmit(C.Partner)
                    If Not IsNothing(C.Contact) AndAlso C.Contact.Count > 0 Then
                        Context.SAP_COMPANY_CONTACTs.InsertAllOnSubmit(C.Contact)
                    End If
                    If Not IsNothing(C.Employee) AndAlso C.Employee.Count > 0 Then
                        Context.SAP_COMPANY_EMPLOYEEs.InsertAllOnSubmit(C.Employee)
                    End If
                    If Not IsNothing(C.Salesdef) AndAlso C.Salesdef.Count > 0 Then
                        Context.SAP_COMPANY_SALES_DEFs.InsertAllOnSubmit(C.Salesdef)
                    End If
                End If
                Clear(CompanyIdStr, Context, False)
                Context.SubmitChanges()
            End If
        End Using
        Return C
    End Function

End Class

Public Class Company
    Public Shared Function getCompanyPatner(ByVal CompanyId As String, ByVal OrgId As String) As List(Of SAP_COMPANY_PARTNER)
        Dim ar As New ArrayList : ar.Add(CompanyId)
        Dim Conn As New Oracle.DataAccess.Client.OracleConnection(ConfigurationManager.ConnectionStrings("SAP_PRD").ConnectionString)
        Try
            Dim partner As List(Of SAP_COMPANY_PARTNER) = syncSingleCompany.getSAPPartner(ar, Conn)
            If Not IsNothing(partner) AndAlso partner.Count > 0 Then
                partner = partner.Where(Function(X As SAP_COMPANY_PARTNER) X.COMPANY_ID = CompanyId AndAlso X.ORG_ID = OrgId).ToList
            End If
            Return partner
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(Conn) Then
                Conn.Close()
            End If
        End Try
    End Function
    Public Shared Function getPartnerByType(ByVal CompanyId As String, ByVal OrgId As String, ByVal Type As Setting.PartnerType) As List(Of SAP_COMPANY_PARTNER)
        Dim l As List(Of SAP_COMPANY_PARTNER) = getCompanyPatner(CompanyId, OrgId)
        If Not IsNothing(l) AndAlso l.Count > 0 Then
            l = l.Where(Function(x As SAP_COMPANY_PARTNER) x.PARTNER_FUNCTION = Type)
        End If
    End Function

End Class

<Serializable()> _
Public Class DimProductSet
    Private _prod As List(Of SAP_PRODUCT) = Nothing
    Public Property Product As List(Of SAP_PRODUCT)
        Get
            Return _prod
        End Get
        Set(ByVal value As List(Of SAP_PRODUCT))
            _prod = value
        End Set
    End Property
    Private _prodOrg As List(Of SAP_PRODUCT_ORG) = Nothing
    Public Property Product_Org As List(Of SAP_PRODUCT_ORG)
        Get
            Return _prodOrg
        End Get
        Set(ByVal value As List(Of SAP_PRODUCT_ORG))
            _prodOrg = value
        End Set
    End Property
    Private _prodStatus As List(Of SAP_PRODUCT_STATUS) = Nothing
    Public Property Product_Status As List(Of SAP_PRODUCT_STATUS)
        Get
            Return _prodStatus
        End Get
        Set(ByVal value As List(Of SAP_PRODUCT_STATUS))
            _prodStatus = value
        End Set
    End Property
    Private _prodABC As List(Of SAP_PRODUCT_ABC) = Nothing
    Public Property Product_ABC As List(Of SAP_PRODUCT_ABC)
        Get
            Return _prodABC
        End Get
        Set(ByVal value As List(Of SAP_PRODUCT_ABC))
            _prodABC = value
        End Set
    End Property
    Private _prodOrderable As List(Of SAP_PRODUCT_STATUS_ORDERABLE) = Nothing
    Public Property Product_Orderable As List(Of SAP_PRODUCT_STATUS_ORDERABLE)
        Get
            Return _prodOrderable
        End Get
        Set(ByVal value As List(Of SAP_PRODUCT_STATUS_ORDERABLE))
            _prodOrderable = value
        End Set
    End Property
End Class
Public Class syncSingleProduct
    Private Shared Function getSAPRecPN(ByVal pnStr As ArrayList, ByVal conn As Oracle.DataAccess.Client.OracleConnection) As List(Of SAP_PRODUCT)
        Dim partCond As String = ""
        If Not IsNothing(pnStr) AndAlso pnStr.Count > 0 Then
            partCond = " and matnr in ('" & String.Join("','", pnStr.ToArray) & "')"
        End If
        Dim str As String = String.Format("select distinct a.matnr as part_no, " & _
                                    "a.bismt as model_no, " & _
                                    "a.MATKL as material_group, " & _
                                    "a.SPART as division, " & _
                                    "a.PRDHA as product_hierarchy, " & _
                                    "a.PRDHA as product_group, " & _
                                    "a.PRDHA as product_division, " &
                                    "a.PRDHA as product_line, " & _
                                    "a.MTPOS_MARA as GenItemCatGrp, " & _
                                    "(select MAKTX from saprdp.makt b where b.matnr=a.matnr and rownum=1 and b.spras='E') as product_desc," & _
                                    "a.ZEIFO as rohs_flag, " & _
                                    "(select vmsta from saprdp.mvke where mvke.matnr=a.matnr and mvke.vkorg='TW01' and rownum=1) as status," & _
                                    "'' as EGROUP, " & _
                                    "'' as EDIVISION, " & _
                                    "a.NTGEW as NET_WEIGHT, " & _
                                    "a.BRGEW as GROSS_WEIGHT, " & _
                                    "a.GEWEI as WEIGHT_UNIT, " & _
                                    "a.VOLUM as VOLUME, " & _
                                    "a.VOLEH as VOLUME_UNIT, " & _
                                    "a.ERSDA as CREATE_DATE, " & _
                                    "a.LAEDA as LAST_UPD_DATE, " & _
                                    "to_char(a.mtart) as product_type, " & _
                                    "a.blatt as GIP_CODE," & _
                                    "a.GROES as SIZE_DIMENSIONS, " & _
                                    "a.NORMT as SOURCE_LOCATION, " & _
                                    "a.ZZPRODT_FAMILY as PRODUCT_FAMILY, " & _
                                    "a.ZZPLM_STAT as PLM_STATUS " & _
                                    "from saprdp.mara a where mandt='168' and a.lvorm <> 'X' {0}", partCond)
        Dim da As New Oracle.DataAccess.Client.OracleDataAdapter(str, conn)
        Dim DT As New DataTable
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            Dim LP As New List(Of SAP_PRODUCT)
            For Each R As DataRow In DT.Rows
                Dim P As New SAP_PRODUCT
                If Not IsDBNull(R.Item("Part_no")) Then
                    P.PART_NO = R.Item("Part_no").ToString.TrimStart("0")
                End If
                If Not IsDBNull(R.Item("model_no")) Then
                    P.MODEL_NO = R.Item("model_no").ToString
                End If
                If Not IsDBNull(R.Item("material_group")) Then
                    P.MATERIAL_GROUP = R.Item("material_group").ToString
                End If
                If Not IsDBNull(R.Item("division")) Then
                    P.DIVISION = R.Item("division").ToString
                End If
                If Not IsDBNull(R.Item("product_hierarchy")) Then
                    P.PRODUCT_HIERARCHY = R.Item("product_hierarchy").ToString
                End If
                If Not IsDBNull(R.Item("product_group")) Then
                    P.PRODUCT_GROUP = R.Item("product_group").ToString
                End If
                If Not IsDBNull(R.Item("product_division")) Then
                    P.PRODUCT_DIVISION = R.Item("product_division").ToString
                End If
                If Not IsDBNull(R.Item("product_line")) Then
                    P.PRODUCT_LINE = R.Item("product_line").ToString
                End If
                If Not IsDBNull(R.Item("GenItemCatGrp")) Then
                    P.GENITEMCATGRP = R.Item("GenItemCatGrp").ToString
                End If
                If Not IsDBNull(R.Item("product_desc")) Then
                    P.PRODUCT_DESC = R.Item("product_desc").ToString
                End If
                If Not IsDBNull(R.Item("rohs_flag")) Then
                    P.ROHS_FLAG = R.Item("rohs_flag").ToString
                End If
                If Not IsDBNull(R.Item("status")) Then
                    P.STATUS = R.Item("status").ToString
                End If
                If Not IsDBNull(R.Item("EGROUP")) Then
                    P.EGROUP = R.Item("EGROUP").ToString
                End If
                If Not IsDBNull(R.Item("EDIVISION")) Then
                    P.EDIVISION = R.Item("EDIVISION").ToString
                End If
                If Not IsDBNull(R.Item("NET_WEIGHT")) Then
                    P.NET_WEIGHT = R.Item("NET_WEIGHT").ToString
                End If
                If Not IsDBNull(R.Item("GROSS_WEIGHT")) Then
                    P.GROSS_WEIGHT = R.Item("GROSS_WEIGHT").ToString
                End If
                If Not IsDBNull(R.Item("WEIGHT_UNIT")) Then
                    P.WEIGHT_UNIT = R.Item("WEIGHT_UNIT").ToString
                End If
                If Not IsDBNull(R.Item("VOLUME")) Then
                    P.VOLUME = R.Item("VOLUME").ToString
                End If
                If Not IsDBNull(R.Item("VOLUME_UNIT")) Then
                    P.VOLUME_UNIT = R.Item("VOLUME_UNIT").ToString
                End If
                If Not IsDBNull(R.Item("CREATE_DATE")) Then
                    P.CREATE_DATE = R.Item("CREATE_DATE").ToString
                End If
                If Not IsDBNull(R.Item("LAST_UPD_DATE")) Then
                    P.LAST_UPD_DATE = R.Item("LAST_UPD_DATE").ToString
                End If
                If Not IsDBNull(R.Item("product_type")) Then
                    P.PRODUCT_TYPE = R.Item("product_type").ToString
                End If
                If Not IsDBNull(R.Item("GIP_CODE")) Then
                    P.GIP_CODE = R.Item("GIP_CODE").ToString
                End If
                If Not IsDBNull(R.Item("SIZE_DIMENSIONS")) Then
                    P.SIZE_DIMENSIONS = R.Item("SIZE_DIMENSIONS").ToString
                End If
                If Not IsDBNull(R.Item("SOURCE_LOCATION")) Then
                    P.SOURCE_LOCATION = R.Item("SOURCE_LOCATION")
                End If
                If Not IsDBNull(R.Item("product_hierarchy")) Then
                    Dim ps() As String = Split(R.Item("product_hierarchy"), "-")
                    If ps.Length >= 3 Then
                        P.PRODUCT_GROUP = ps(0) : P.PRODUCT_DIVISION = ps(1)
                        If ps.Length = 3 Then
                            P.PRODUCT_LINE = ps(2)
                        Else
                            P.PRODUCT_LINE = ps(2) + ps(3)
                        End If
                    End If
                End If
                If Not IsDBNull(R.Item("PRODUCT_FAMILY")) Then
                    P.PRODUCT_FAMILY = R.Item("PRODUCT_FAMILY").ToString.Trim
                End If
                If Not IsDBNull(R.Item("PLM_STATUS")) Then
                    P.PLM_STATUS = R.Item("PLM_STATUS").ToString.Trim
                End If
                LP.Add(P)
            Next
            Return LP
        End If
        Return Nothing
    End Function
    Private Shared Function getSAPRecPNOrg(ByVal pnStr As ArrayList, ByVal Org As String, ByVal conn As Oracle.DataAccess.Client.OracleConnection) As List(Of SAP_PRODUCT_ORG)
        Dim partCond As String = ""
        If Not IsNothing(pnStr) AndAlso pnStr.Count > 0 Then
            partCond = " and mara.matnr in ('" & String.Join("','", pnStr.ToArray) & "')"
        End If
        Dim ORGCondition As String = ""
        If Org.ToUpper <> "ALL" Then
            ORGCondition = "and mvke.vkorg like '" & Org & "%'"
        End If
        Dim str As String = String.Format("SELECT DISTINCT " & _
                                            " to_char(mara.matnr) as part_no, " & _
                                            " mvke.vkorg as org_id, " & _
                                            " mvke.VTWEG as dist_channel, " & _
                                            " to_char(mvke.vmsta) as status, " & _
                                            " to_char(mvke.PRAT5) as B2BOnline, " & _
                                            " mvke.DWERK as DeliveryPlant, " & _
                                            " mvke.kondm as PricingGroup, mara.laeda as LAST_UPD_DATE " & _
                                            " FROM saprdp.mara INNER JOIN saprdp.mvke ON mara.matnr = mvke.matnr " & _
                                            " WHERE mara.lvorm <> 'X' and mara.mandt='168' and mvke.mandt='168' and mara.mtart LIKE 'Z%' {0} {1}", partCond, ORGCondition) 'ICC 2015/2/26 Exclude X data in lvorm
        Dim da As New Oracle.DataAccess.Client.OracleDataAdapter(str, conn)
        Dim DT As New DataTable
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            Dim LP As New List(Of SAP_PRODUCT_ORG)
            For Each R As DataRow In DT.Rows
                Dim P As New SAP_PRODUCT_ORG
                If Not IsDBNull(R.Item("Part_no")) Then
                    P.PART_NO = R.Item("Part_no").ToString.TrimStart("0")
                End If
                If Not IsDBNull(R.Item("org_id")) Then
                    P.ORG_ID = R.Item("org_id").ToString
                End If
                If Not IsDBNull(R.Item("dist_channel")) Then
                    P.DIST_CHANNEL = R.Item("dist_channel").ToString
                End If
                If Not IsDBNull(R.Item("status")) Then
                    P.STATUS = R.Item("status").ToString
                End If
                If Not IsDBNull(R.Item("B2BOnline")) Then
                    P.B2BONLINE = R.Item("B2BOnline").ToString
                End If
                If Not IsDBNull(R.Item("DeliveryPlant")) Then
                    P.DELIVERYPLANT = R.Item("DeliveryPlant").ToString
                End If
                If Not IsDBNull(R.Item("PricingGroup")) Then
                    P.PRICINGGROUP = R.Item("PricingGroup").ToString
                End If
                If Not IsDBNull(R.Item("LAST_UPD_DATE")) Then
                    P.LAST_UPD_DATE = R.Item("LAST_UPD_DATE").ToString
                End If
                LP.Add(P)
            Next
            Return LP
        End If
        Return Nothing
    End Function
    Private Shared Function getSAPRecPNStatus(ByVal pnStr As ArrayList, ByVal Org As String, ByVal conn As Oracle.DataAccess.Client.OracleConnection) As List(Of SAP_PRODUCT_STATUS)
        Dim partCond As String = ""
        If Not IsNothing(pnStr) AndAlso pnStr.Count > 0 Then
            partCond = " and matnr in ('" & String.Join("','", pnStr.ToArray) & "')"
        End If
        Dim ORGCondition As String = ""
        If Org.ToUpper <> "ALL" Then
            ORGCondition = "and mvke.vkorg like '" & Org & "%'"
        End If
        Dim str As String = String.Format("select matnr as part_no, vkorg as sales_org, vtweg as dist_channel, vmsta as product_status, " & _
                                           " AUMNG as min_order_qty, LFMNG as min_dlv_qty, EFMNG as min_bto_qty, DWERK as dlv_plant, " & _
                                           " KONDM as material_pricing_grp, vmstd as valid_date, to_char(mvke.mtpos) as item_category_group " & _
                                           " from saprdp.MVKE " & _
                                           " where mandt='168' {0} {1}", partCond, ORGCondition)
        Dim da As New Oracle.DataAccess.Client.OracleDataAdapter(str, conn)
        Dim DT As New DataTable
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            Dim LP As New List(Of SAP_PRODUCT_STATUS)
            For Each R As DataRow In DT.Rows
                Dim P As New SAP_PRODUCT_STATUS
                If Not IsDBNull(R.Item("Part_no")) Then
                    P.PART_NO = R.Item("Part_no").ToString.TrimStart("0")
                End If
                If Not IsDBNull(R.Item("sales_org")) Then
                    P.SALES_ORG = R.Item("sales_org").ToString
                End If
                If Not IsDBNull(R.Item("dist_channel")) Then
                    P.DIST_CHANNEL = R.Item("dist_channel").ToString
                End If
                If Not IsDBNull(R.Item("product_status")) Then
                    P.PRODUCT_STATUS = R.Item("product_status").ToString
                End If
                If Not IsDBNull(R.Item("min_order_qty")) Then
                    P.MIN_ORDER_QTY = R.Item("min_order_qty").ToString
                End If
                If Not IsDBNull(R.Item("min_dlv_qty")) Then
                    P.MIN_DLV_QTY = R.Item("min_dlv_qty").ToString
                End If
                If Not IsDBNull(R.Item("min_bto_qty")) Then
                    P.MIN_BTO_QTY = R.Item("min_bto_qty").ToString
                End If
                If Not IsDBNull(R.Item("dlv_plant")) Then
                    P.DLV_PLANT = R.Item("dlv_plant").ToString
                End If
                If Not IsDBNull(R.Item("material_pricing_grp")) Then
                    P.MATERIAL_PRICING_GRP = R.Item("material_pricing_grp").ToString
                End If
                If Not IsDBNull(R.Item("valid_date")) Then
                    P.VALID_DATE = R.Item("valid_date").ToString
                End If
                If Not IsDBNull(R.Item("item_category_group")) Then
                    P.ITEM_CATEGORY_GROUP = R.Item("item_category_group").ToString
                End If
                LP.Add(P)
            Next
            Return LP
        End If
        Return Nothing
    End Function
    Private Shared Function getSAPRecPNABC(ByVal pnStr As ArrayList, ByVal Org As String, ByVal conn As Oracle.DataAccess.Client.OracleConnection) As List(Of SAP_PRODUCT_ABC)
        Dim partCond As String = ""
        If Not IsNothing(pnStr) AndAlso pnStr.Count > 0 Then
            partCond = " and matnr in ('" & String.Join("','", pnStr.ToArray) & "')"
        End If
        Dim ORGCondition As String = ""
        If Org.ToUpper <> "ALL" Then
            ORGCondition = "and marc.werks like '" & Org.Substring(0, 2) & "%'"
        End If
        Dim STR As String = String.Format("select matnr as PART_NO, werks as PLANT, maabc as ABC_INDICATOR,marc.PLIFZ as PlannedDelTime, " & _
                                            "marc.WEBAZ as GrProcessingTime," & _
                                             "marc.DZEIT as InHouseProduction, marc.PRCTR as ProfitCenter, marc.STEUC as Ctrl_Code," & _
                                            "marc.EISBE as safety_stock, marc.EISLO as min_safety_stock, marc.LGRAD as service_level, marc.BSTMI," & _
                                            "(SELECT TMFGT.BEZEI FROM saprdp.TMFGT where SPRAS='E' and MFRGR=MARC.MFRGR and rownum=1) AS FREIGHT_METHOD,MARC.HERKL " & _
                                            "from saprdp.marc where mandt='168' {0} {1}", partCond, ORGCondition)
        Dim da As New Oracle.DataAccess.Client.OracleDataAdapter(STR, conn)
        Dim DT As New DataTable
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            Dim LP As New List(Of SAP_PRODUCT_ABC)
            For Each R As DataRow In DT.Rows
                Dim P As New SAP_PRODUCT_ABC
                If Not IsDBNull(R.Item("InHouseProduction")) AndAlso Integer.TryParse(R.Item("InHouseProduction"), 0) Then
                    P.IN_HOUSE_PRODUCTION = Integer.Parse(R.Item("InHouseProduction"))
                End If
                If Not IsDBNull(R.Item("ProfitCenter")) Then
                    P.ProfitCenter = R.Item("ProfitCenter").ToString
                End If
                If Not IsDBNull(R.Item("Ctrl_Code")) Then
                    P.Ctrl_Code = R.Item("Ctrl_Code").ToString
                End If
                If Not IsDBNull(R.Item("safety_stock")) AndAlso Decimal.TryParse(R.Item("safety_stock"), 0) Then
                    P.safety_stock = Decimal.Parse(R.Item("safety_stock"))
                End If
                If Not IsDBNull(R.Item("min_safety_stock")) AndAlso Decimal.TryParse(R.Item("min_safety_stock"), 0) Then
                    P.min_safety_stock = Decimal.Parse(R.Item("min_safety_stock"))
                End If
                If Not IsDBNull(R.Item("service_level")) AndAlso Decimal.TryParse(R.Item("service_level"), 0) Then
                    P.service_level = Decimal.Parse(R.Item("service_level"))
                End If
                If Not IsDBNull(R.Item("BSTMI")) AndAlso Decimal.TryParse(R.Item("BSTMI"), 0) Then
                    P.MIN_LOT_SIZE = Decimal.Parse(R.Item("BSTMI"))
                End If


                If Not IsDBNull(R.Item("Part_no")) Then
                    P.PART_NO = R.Item("Part_no").ToString.TrimStart("0")
                End If
                If Not IsDBNull(R.Item("PLANT")) Then
                    P.PLANT = R.Item("PLANT").ToString
                End If
                If Not IsDBNull(R.Item("ABC_INDICATOR")) Then
                    P.ABC_INDICATOR = R.Item("ABC_INDICATOR").ToString
                End If
                If Not IsDBNull(R.Item("PlannedDelTime")) Then
                    P.PLANNED_DEL_TIME = R.Item("PlannedDelTime").ToString
                End If
                If Not IsDBNull(R.Item("GrProcessingTime")) Then
                    P.GP_PROCESSING_TIME = R.Item("GrProcessingTime").ToString
                End If
                If Not IsDBNull(R.Item("FREIGHT_METHOD")) Then
                    P.FREIGHT_METHOD = R.Item("FREIGHT_METHOD")
                End If
                If Not IsDBNull(R.Item("HERKL")) Then
                    P.COUNTRY_ORIGIN = R.Item("HERKL")
                End If
                LP.Add(P)
            Next
            Return LP
        End If
        Return Nothing
    End Function
    Public Shared Function getDimSAPProdSet(ByVal pnStr As ArrayList, ByVal Org As String, ByVal dbDest As String) As DimProductSet
        Dim Conn As New Oracle.DataAccess.Client.OracleConnection(ConfigurationManager.ConnectionStrings(dbDest).ConnectionString)
        Try
            Dim dimProd As DimProductSet = Nothing
            Dim pnL As List(Of SAP_PRODUCT) = getSAPRecPN(pnStr, Conn)
            Dim pnOrgL As List(Of SAP_PRODUCT_ORG) = getSAPRecPNOrg(pnStr, Org, Conn)
            Dim pnStatusL As List(Of SAP_PRODUCT_STATUS) = getSAPRecPNStatus(pnStr, Org, Conn)
            Dim pnABCL As List(Of SAP_PRODUCT_ABC) = getSAPRecPNABC(pnStr, Org, Conn)
            Dim PnOrderableL As List(Of SAP_PRODUCT_STATUS_ORDERABLE) = Nothing

            If Not IsNothing(pnL) AndAlso pnL.Count > 0 Then
                dimProd = New DimProductSet
                dimProd.Product = pnL
                If Not IsNothing(pnOrgL) AndAlso pnOrgL.Count > 0 Then
                    dimProd.Product_Org = pnOrgL
                End If
                If Not IsNothing(pnStatusL) AndAlso pnStatusL.Count > 0 Then
                    dimProd.Product_Status = pnStatusL
                End If
                If Not IsNothing(pnABCL) AndAlso pnABCL.Count > 0 Then
                    dimProd.Product_ABC = pnABCL
                End If
                If Not IsNothing(PnOrderableL) AndAlso PnOrderableL.Count > 0 Then
                    dimProd.Product_Orderable = PnOrderableL
                End If
            End If
            Return dimProd
        Catch ex As Exception
            Throw ex
        Finally
            If Not IsNothing(Conn) Then
                Conn.Close()
            End If
        End Try
    End Function

    Private Shared Sub Clear(ByVal pnStr As ArrayList, ByVal Org As String, ByVal Context As L2SProductDataContext, Optional ByVal isSubmit As Boolean = True)
        If Not IsNothing(pnStr) AndAlso pnStr.Count > 0 Then
            For i As Integer = 0 To pnStr.Count - 1
                pnStr.Item(i) = pnStr.Item(i).ToString.TrimStart("0").ToUpper
            Next
            Dim PnL As List(Of SAP_PRODUCT) = (From X In Context.SAP_PRODUCTs _
                     Where pnStr.ToArray.Contains(X.PART_NO) _
                     Select X).ToList
            Dim PnOrderableL As List(Of SAP_PRODUCT_STATUS_ORDERABLE) = (From X In Context.SAP_PRODUCT_STATUS_ORDERABLEs _
                      Where pnStr.ToArray.Contains(X.PART_NO) _
                      Select X).ToList
            Dim PnOrgL As List(Of SAP_PRODUCT_ORG) = (From X In Context.SAP_PRODUCT_ORGs _
                        Where pnStr.ToArray.Contains(X.PART_NO) _
                        Select X).ToList
            Dim PnStatusL As List(Of SAP_PRODUCT_STATUS) = (From X In Context.SAP_PRODUCT_STATUS _
                           Where pnStr.ToArray.Contains(X.PART_NO) _
                           Select X).ToList
            Dim PnABCL As List(Of SAP_PRODUCT_ABC) = (From X In Context.SAP_PRODUCT_ABCs _
                        Where pnStr.ToArray.Contains(X.PART_NO) _
                        Select X).ToList
            If Org.ToUpper <> "ALL" Then
                PnOrderableL = PnOrderableL.Where(Function(x As SAP_PRODUCT_STATUS_ORDERABLE) x.SALES_ORG.StartsWith(Org)).ToList
                PnOrgL = PnOrgL.Where(Function(x As SAP_PRODUCT_ORG) x.ORG_ID.StartsWith(Org)).ToList
                PnStatusL = PnStatusL.Where(Function(x As SAP_PRODUCT_STATUS) x.SALES_ORG.StartsWith(Org)).ToList
                PnABCL = PnABCL.Where(Function(x As SAP_PRODUCT_ABC) x.PLANT.StartsWith(Org)).ToList
            End If
            If Not IsNothing(PnL) AndAlso PnL.Count > 0 Then
                Context.SAP_PRODUCTs.DeleteAllOnSubmit(PnL)
            End If
            If Not IsNothing(PnOrderableL) AndAlso PnOrderableL.Count > 0 Then
                Context.SAP_PRODUCT_STATUS_ORDERABLEs.DeleteAllOnSubmit(PnOrderableL)
            End If
            If Not IsNothing(PnOrgL) AndAlso PnOrgL.Count > 0 Then
                Context.SAP_PRODUCT_ORGs.DeleteAllOnSubmit(PnOrgL)
            End If
            If Not IsNothing(PnStatusL) AndAlso PnStatusL.Count > 0 Then
                Context.SAP_PRODUCT_STATUS.DeleteAllOnSubmit(PnStatusL)
            End If
            If Not IsNothing(PnABCL) AndAlso PnABCL.Count > 0 Then
                Context.SAP_PRODUCT_ABCs.DeleteAllOnSubmit(PnABCL)
            End If
            If isSubmit = True Then
                Context.SubmitChanges()
            End If
        End If
    End Sub
    Public Shared Function syncSAPProduct(ByVal PNStr As ArrayList, ByVal OrgPrefix As String, ByVal isTest As Boolean, ByRef errMsg As String, Optional ByVal isSyncPIS As Boolean = False, Optional ByRef FailedMsg As ArrayList = Nothing, Optional ByVal isBatch As Boolean = False) As DimProductSet
        Dim dest As String = "SAP_PRD"
        If isTest Then
            dest = "SAP_Test"
        End If
        If Not IsNothing(PNStr) AndAlso PNStr.Count > 0 Then
            For i As Integer = 0 To PNStr.Count - 1
                If ULong.TryParse(PNStr(i), 0) Then
                    PNStr.Item(i) = CDbl(PNStr.Item(i)).ToString("000000000000000000")
                End If
                PNStr.Item(i) = PNStr.Item(i).ToString.ToUpper
            Next
        Else
            errMsg = "Invalid part no list." : Return New DimProductSet
        End If

        Dim P As DimProductSet = Nothing
        P = getDimSAPProdSet(PNStr, OrgPrefix, dest)
        If IsNothing(P) OrElse IsNothing(P.Product) Then
            errMsg = "Get all products master data failed." : Return New DimProductSet
        End If



        For Each pn As String In PNStr
            Dim pnL As List(Of SAP_PRODUCT) = P.Product.Where(Function(x As SAP_PRODUCT) x.PART_NO = pn).ToList
            If IsNothing(pnL) Then
                If Not IsNothing(FailedMsg) Then
                    FailedMsg.Add(String.Format("'{0}' sync failed,cannot get product master data.", pn))
                End If
                If Not IsNothing(P.Product_ABC) Then
                    P.Product_ABC.RemoveAll(Function(x As SAP_PRODUCT_ABC) x.PART_NO = pn)

                End If
                If Not IsNothing(P.Product_Org) Then
                    P.Product_Org.RemoveAll(Function(x As SAP_PRODUCT_ORG) x.PART_NO = pn)

                End If
                If Not IsNothing(P.Product_Status) Then
                    P.Product_Status.RemoveAll(Function(x As SAP_PRODUCT_STATUS) x.PART_NO = pn)

                End If
            End If
        Next

        If Not IsNothing(P.Product_Status) Then
            P.Product_Orderable = New List(Of SAP_PRODUCT_STATUS_ORDERABLE)
            Dim p1 As New GET_MATERIAL_ATP.GET_MATERIAL_ATP
            p1.Connection = New SAP.Connector.SAPConnection(ConfigurationManager.AppSettings(dest))
            p1.Connection.Open()
            For Each x As SAP_PRODUCT_STATUS In P.Product_Status
                Dim PO As New SAP_PRODUCT_STATUS_ORDERABLE
                PO.PART_NO = x.PART_NO
                PO.SALES_ORG = x.SALES_ORG
                PO.DIST_CHANNEL = x.DIST_CHANNEL
                PO.PRODUCT_STATUS = x.PRODUCT_STATUS
                PO.MIN_ORDER_QTY = x.MIN_ORDER_QTY
                PO.MIN_DLV_QTY = x.MIN_DLV_QTY
                PO.MIN_BTO_QTY = x.MIN_BTO_QTY
                PO.DLV_PLANT = x.DLV_PLANT
                PO.MATERIAL_PRICING_GRP = x.MATERIAL_PRICING_GRP
                PO.VALID_DATE = x.VALID_DATE
                PO.ITEM_CATEGORY_GROUP = x.ITEM_CATEGORY_GROUP
                If PO.PRODUCT_STATUS.ToUpper = "O" Or PO.PRODUCT_STATUS.ToUpper = "A" Or PO.PRODUCT_STATUS.ToUpper = "N" Or PO.PRODUCT_STATUS.ToUpper = "H" Or PO.PRODUCT_STATUS.Trim = "" Or PO.PRODUCT_STATUS.ToUpper = "M1" Or PO.PRODUCT_STATUS.ToUpper = "S2" Or PO.PRODUCT_STATUS.ToUpper = "T" Or PO.PRODUCT_STATUS.ToUpper = "V" Then
                    If PO.PRODUCT_STATUS = "O" Then
                        Dim intATP As Integer = GetATP(PO.PART_NO, PO.DLV_PLANT, p1)
                        If intATP > 0 Then
                            If Not PO.SALES_ORG.ToUpper.StartsWith("EU") AndAlso IsPTD(PO.PART_NO, P.Product.Where(Function(t As SAP_PRODUCT) t.PART_NO = x.PART_NO).FirstOrDefault.PRODUCT_TYPE) Then
                            Else
                                P.Product_Orderable.Add(PO)
                            End If
                        End If
                    ElseIf PO.PRODUCT_STATUS.Trim = "" Then
                        If PO.SALES_ORG.ToUpper.StartsWith("US") Then
                            P.Product_Orderable.Add(PO)
                        End If
                    Else
                        P.Product_Orderable.Add(PO)
                    End If
                End If
            Next
            If Not IsNothing(p1) AndAlso Not IsNothing(p1.Connection) Then
                p1.Connection.Close() : p1 = Nothing
            End If
        End If

        Using Context As New L2SProductDataContext
            If Not IsNothing(P.Product) AndAlso P.Product.Count > 0 Then
                Context.SAP_PRODUCTs.InsertAllOnSubmit(P.Product)

                If Not IsNothing(P.Product_Org) AndAlso P.Product_Org.Count > 0 Then
                    Context.SAP_PRODUCT_ORGs.InsertAllOnSubmit(P.Product_Org)
                End If
                If Not IsNothing(P.Product_Status) AndAlso P.Product_Status.Count > 0 Then
                    Context.SAP_PRODUCT_STATUS.InsertAllOnSubmit(P.Product_Status)
                    If Not IsNothing(P.Product_Orderable) AndAlso P.Product_Orderable.Count > 0 Then
                        Context.SAP_PRODUCT_STATUS_ORDERABLEs.InsertAllOnSubmit(P.Product_Orderable)
                    End If
                End If
                If Not IsNothing(P.Product_ABC) AndAlso P.Product_ABC.Count > 0 Then
                    Context.SAP_PRODUCT_ABCs.InsertAllOnSubmit(P.Product_ABC)
                End If
                Clear(PNStr, OrgPrefix, Context, False)
                Context.SubmitChanges()
            End If
        End Using
        '----------------------------
        If isSyncPIS Then
            Dim currentPartNo As New StringBuilder() 'ICC 2015/2/26 Add sb to record part no
            Try
                SyncPIS(OrgPrefix, P, currentPartNo)
            Catch ex As Exception
                errMsg &= String.Format("Sync PIS error! Part No : {0}, Error Message : {1}", currentPartNo.ToString, ex.ToString) 'ICC 2015/2/26 Record error message in detail
            End Try
        End If
        If isBatch = False Then
            SyncExtDesc(PNStr.Item(0).ToString.TrimStart("0"))
        End If
        Return P
    End Function
    Private Shared Function SyncExtDesc(ByVal partNo As String)
        Dim str As String = String.Format(" update SAP_PRODUCT " &
       " set SAP_PRODUCT.PRODUCT_DESC = b.extended_desc " &
       " from SAP_PRODUCT a inner join SAP_PRODUCT_EXT_DESC b on a.part_no=b.PART_NO where a.part_no='{0}'; " &
       " update cbom_catalog_category " &
       " set cbom_catalog_category.CATEGORY_DESC = b.extended_desc " &
       " from cbom_catalog_category a inner join SAP_PRODUCT_EXT_DESC b on a.CATEGORY_ID=b.PART_NO " &
       " where a.ORG<>'EU' and a.CATEGORY_ID='{0}'", partNo.ToUpper)

        dbUtil.dbExecuteNoQuery("MY", str)
    End Function
    Private Shared Function SyncPIS(ByVal OrgPrefix As String, ByVal P As DimProductSet, ByVal currentPartNo As StringBuilder)
        Dim arr As New ArrayList
        For Each x As SAP_PRODUCT In P.Product
            'arr.Add(x.PART_NO.Trim("0").ToString)
            arr.Add(x.PART_NO.TrimStart("0").ToString)
        Next
        Dim partCond As String = ""
        If Not IsNothing(arr) AndAlso arr.Count > 0 Then
            partCond = " part_no in ('" & String.Join("','", arr.ToArray) & "')"
        End If
        If Not IsNothing(P) AndAlso
            Not IsNothing(P.Product) AndAlso P.Product.Count > 0 AndAlso
            Not IsNothing(P.Product_Org) AndAlso P.Product_Org.Count > 0 AndAlso
            Not IsNothing(P.Product_Status) AndAlso P.Product_Status.Count > 0 AndAlso
            Not IsNothing(P.Product_ABC) AndAlso P.Product_ABC.Count > 0 Then

            Using conn As SqlClient.SqlConnection = New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("PIS").ConnectionString)
                Dim STR As String = ""
                If OrgPrefix.ToUpper <> "ALL" Then
                    STR = String.Format("delete from PRODUCT_LOGISTICS_NEW where {0} and Org_Id like '{1}%'", partCond, OrgPrefix)
                Else
                    STR = String.Format("delete from PRODUCT_LOGISTICS_NEW where {0}", partCond)
                End If
                Dim CMD As New SqlClient.SqlCommand(STR, conn)
                CMD.CommandTimeout = 1800
                conn.Open()
                CMD.ExecuteNonQuery()
                For Each r As SAP_PRODUCT_ORG In P.Product_Org
                    currentPartNo.Clear()
                    currentPartNo.Append(r.PART_NO) 'ICC 2015/2/26 Save current part no
                    Dim pno As SAP_PRODUCT = P.Product.Where(Function(x As SAP_PRODUCT) x.PART_NO = r.PART_NO).FirstOrDefault
                    Dim rstatus As SAP_PRODUCT_STATUS = P.Product_Status.Where(Function(x) x.PART_NO = r.PART_NO AndAlso x.SALES_ORG = r.ORG_ID).FirstOrDefault()
                    Dim rABC As SAP_PRODUCT_ABC = P.Product_ABC.Where(Function(x) x.PART_NO = r.PART_NO AndAlso x.PLANT = r.DELIVERYPLANT).FirstOrDefault()

                    'ICC 20170721 Check data to prevent null reference error
                    If pno Is Nothing Then
                        currentPartNo.Append(" does not have SAP_PRODUCT data")
                        Continue For
                    End If
                    If rstatus Is Nothing Then
                        currentPartNo.Append(" does not have SAP_PRODUCT_STATUS data")
                        Continue For
                    End If
                    If rABC Is Nothing Then
                        currentPartNo.Append(" does not have SAP_PRODUCT_ABC data")
                        Continue For
                    End If

                    If Not IsNothing(rstatus) AndAlso Not IsNothing(rABC) Then
                        Dim tlud As Date = Now
                        Dim lud As String = Now.ToString("yyyyMMdd")
                        If Date.TryParseExact("20140210", "yyyyMMdd", Nothing, System.Globalization.DateTimeStyles.None, tlud) Then
                            lud = tlud.ToString("yyyyMMdd")
                        End If

                        Dim tcd As Date = Now
                        Dim cd As String = Now.ToString("yyyyMMdd")
                        If Date.TryParseExact("20140210", "yyyyMMdd", Nothing, System.Globalization.DateTimeStyles.None, tcd) Then
                            cd = tcd.ToString("yyyyMMdd")
                        End If

                        Dim desc As String = String.Empty
                        If Not String.IsNullOrEmpty(pno.PRODUCT_DESC) Then desc = pno.PRODUCT_DESC.Replace("'", "''")
                        Dim pdStatus As String = String.Empty
                        If Not String.IsNullOrEmpty(rstatus.PRODUCT_STATUS) Then pdStatus = rstatus.PRODUCT_STATUS.Replace("'", "''")
                        Dim orgID As String = String.Empty
                        If Not String.IsNullOrEmpty(r.ORG_ID) Then orgID = r.ORG_ID.Replace("'", "''")
                        Dim modelNo As String = String.Empty
                        If Not String.IsNullOrEmpty(pno.MODEL_NO) Then modelNo = pno.MODEL_NO.Replace("'", "''")
                        Dim productLine As String = String.Empty
                        If Not String.IsNullOrEmpty(pno.PRODUCT_LINE) Then productLine = pno.PRODUCT_LINE.Replace("'", "''")
                        Dim productGroup As String = String.Empty
                        If Not String.IsNullOrEmpty(pno.PRODUCT_GROUP) Then productGroup = pno.PRODUCT_GROUP.Replace("'", "''")
                        Dim productType As String = String.Empty
                        If Not String.IsNullOrEmpty(pno.PRODUCT_TYPE) Then productType = pno.PRODUCT_TYPE.Replace("'", "''")
                        Dim deliveryPlant As String = String.Empty
                        If Not String.IsNullOrEmpty(r.DELIVERYPLANT) Then deliveryPlant = r.DELIVERYPLANT.Replace("'", "''")
                        Dim abcIndicator As String = String.Empty
                        If Not String.IsNullOrEmpty(rABC.ABC_INDICATOR) Then abcIndicator = rABC.ABC_INDICATOR.Replace("'", "''")
                        Dim productDivi As String = String.Empty
                        If Not String.IsNullOrEmpty(pno.PRODUCT_DIVISION) Then productDivi = pno.PRODUCT_DIVISION.Replace("'", "''")
                        Dim sizeDim As String = String.Empty
                        If Not String.IsNullOrEmpty(pno.SIZE_DIMENSIONS) Then sizeDim = pno.SIZE_DIMENSIONS.Replace("'", "''")

                        CMD.CommandText &= String.Format("insert into PRODUCT_LOGISTICS_NEW ( " &
                                           " PRODUCT_ID, " &
                                           " PART_NO, " &
                                           " PRODUCT_DESC, " &
                                           " STATUS, " &
                                           " ORG_ID, " &
                                           " VERSION, " &
                                           " UOM, " &
                                           " MODEL_NO, " &
                                           " CERTIFICATE, " &
                                           " PRODUCT_LINE, " &
                                           " PRODUCT_GROUP, " &
                                           " TUMBNAIL_IMAGE_ID, " &
                                           " IMAGE_ID, " &
                                           " PRODUCT_TYPE, " &
                                           " ONLINE_PUBLISH, " &
                                           " EXTENTED_DESC, " &
                                           " NEW_PRODUCT_DATE, " &
                                           " SHIP_WEIGHT, " &
                                           " NET_WEIGHT, " &
                                           " PRODUCT_SITE, " &
                                           " LAST_UPDATED, " &
                                           " LAST_UPDATED_BY, " &
                                           " CREATED_DATE, " &
                                           " CREATED_BY, " &
                                           " Controller_Code, " &
                                           " Cost, " &
                                           " Currency, " &
                                           " RoHS_Status, " &
                                           " ABC_Ind, " &
                                           " Region_Dim_Weight, " &
                                           " Product_Division, " &
                                           " Dimension) VALUES (N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}',N'{9}',N'{10}',N'{11}',N'{12}',N'{13}',N'{14}',N'{15}',N'{16}'," &
                                           " N'{17}',N'{18}',N'{19}',N'{20}',N'{21}',N'{22}',N'{23}',N'{24}',N'{25}',N'{26}',N'{27}',N'{28}',N'{29}',N'{30}',N'{31}'); ",
                                           "", r.PART_NO.ToUpper.TrimStart("0").Replace("'", "''"), desc, pdStatus, orgID, "-1", "PC", modelNo, "", productLine, productGroup,
                                           "", "", productType, "", "", "", pno.GROSS_WEIGHT, pno.NET_WEIGHT, deliveryPlant, lud, "",
                                           cd, "", "", 0, "", IIf((Integer.TryParse(pno.ROHS_FLAG.Trim, 0) AndAlso pno.ROHS_FLAG = "1"), "Y", "N"), abcIndicator, 0, productDivi, sizeDim)

                    End If
                Next
                CMD.ExecuteNonQuery()
            End Using
        End If
    End Function

    Public Shared Function SyncSAPProudctWarranty() As String
        Dim sb As New StringBuilder()
        sb.Append("SELECT DISTINCT A.PART_NO, B.SALES_ORG from  dbo.SAP_PRODUCT A ")
        sb.Append("INNER JOIN SAP_PRODUCT_STATUS_ORDERABLE B ON A.PART_NO=B.PART_NO ")
        sb.AppendFormat("AND  B.PRODUCT_STATUS IN {0}", ConfigurationManager.AppSettings("CanOrderProdStatus"))
        Dim dt As DataTable = dbUtil.dbGetDataTable("MY", sb.ToString())

        Dim resultDt As DataTable = New DataTable()
        Dim col1 As New DataColumn("PART_NO", GetType(System.String))
        Dim col2 As New DataColumn("SALES_ORG", GetType(System.String))
        Dim col3 As New DataColumn("WARRANTY_MONTH", GetType(System.Int32))
        col3.DefaultValue = 0

        resultDt.Columns.Add(col1)
        resultDt.Columns.Add(col2)
        resultDt.Columns.Add(col3)

        Dim errMsg As List(Of String) = New List(Of String)()

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            For Each dr As DataRow In dt.Rows
                Dim pn As String = dr(0).ToString().Trim().ToUpper()
                Dim sappn As String = Global_Inc.Format2SAPItem(pn)
                Dim org = dr(1).ToString().Trim().ToUpper()

                If Not String.IsNullOrEmpty(sappn) AndAlso Not String.IsNullOrEmpty(org) Then
                    Dim warranty As Object = GetProductWarranty(sappn, org)
                    Dim warrantymonth As Integer = 0
                    If Not warranty Is Nothing AndAlso Integer.TryParse(warranty, warrantymonth) = True Then
                        Dim resultDr As DataRow = resultDt.NewRow()
                        resultDr(0) = pn
                        resultDr(1) = org
                        resultDr(2) = warrantymonth
                        resultDt.Rows.Add(resultDr)
                    Else
                        errMsg.Add(String.Format("PartNO: {0}, Org: {1}, warranty month: {2} has some problems.", pn, org, warrantymonth))
                    End If
                End If
            Next
        End If
        If resultDt.Rows.Count > 0 Then
            Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("MY").ConnectionString)
            Try
                dbUtil.dbExecuteNoQuery("MY", "TRUNCATE TABLE SAP_PRODUCT_WARRANTY")

                conn.Open()
                Dim bk As New SqlBulkCopy(conn)
                bk.DestinationTableName = "SAP_PRODUCT_WARRANTY"
                bk.ColumnMappings.Add("PART_NO", "PART_NO")
                bk.ColumnMappings.Add("SALES_ORG", "SALES_ORG")
                bk.ColumnMappings.Add("WARRANTY_MONTH", "WARRANTY_MONTH")
                bk.WriteToServer(resultDt)
            Catch ex As Exception
                errMsg.Add(ex.ToString())
            Finally
                If conn.State <> ConnectionState.Closed Then conn.Close()
                conn.Dispose()
            End Try
        End If
        'ICC 2018/5/25 Also sync eStore warranty
        Dim eStore As String = SyncSAPProductWarrantyToeStore()
        If Not String.IsNullOrEmpty(eStore) Then errMsg.Add(eStore)
        Return String.Join(",", errMsg)
    End Function

    Public Shared Function SyncSAPProductWarrantyToeStore() As String
        Try
            dbUtil.dbExecuteNoQuery("MY", " TRUNCATE TABLE SAP_PRODUCT_WARRANTY_eStore; INSERT INTO SAP_PRODUCT_WARRANTY_eStore SELECT PART_NO,WARRANTY_MONTH FROM SAP_PRODUCT_WARRANTY group by PART_NO,WARRANTY_MONTH")
        Catch ex As Exception
            Return "Truncate SAP_PRODUCT_WARRANTY_eStore error, " + ex.Message
        End Try
        Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("eStore").ConnectionString)
        Try
            Dim dt As DataTable = dbUtil.dbGetDataTable("MY", "select * from SAP_PRODUCT_WARRANTY_eStore")
            If dt.Rows.Count > 0 Then
                dbUtil.dbExecuteNoQuery("eStore", "TRUNCATE TABLE SAP_PRODUCT_WARRANTY")
                conn.Open()
                Dim bk As New SqlBulkCopy(conn)
                bk.DestinationTableName = "SAP_PRODUCT_WARRANTY"
                bk.WriteToServer(dt)
            End If
        Catch ex As Exception
            Return "Sync eStore warranty error, " + ex.Message
        Finally
            If conn.State <> ConnectionState.Closed Then conn.Close()
            conn.Dispose()
        End Try
        Return String.Empty
    End Function

    Public Shared Function GetProductWarranty(ByVal partno As String, ByVal org As String) As Object
        Dim sb As New StringBuilder()
        sb.Append("select to_char(zzwarr_m) as warranty_month from saprdp.mara INNER JOIN saprdp.mvke ON mara.matnr = mvke.matnr ")
        sb.AppendFormat("where mara.mandt='168' and mara.matnr='{0}' and mvke.vkorg = '{1}' ", partno, org)
        Try
            Return OraDbUtil.dbExecuteScalar("SAP_PRD", sb.ToString())
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Shared Function GetATP(ByVal PartNo As String, ByVal Plant As String, ByRef p1 As GET_MATERIAL_ATP.GET_MATERIAL_ATP) As Integer
        Dim Inventory As Integer = 0
        PartNo = SAPDAL.Format2SAPItem(Trim(UCase(PartNo)))
        Dim culQty As Integer = 0
        Dim retTb As New GET_MATERIAL_ATP.BAPIWMDVSTable, atpTb As New GET_MATERIAL_ATP.BAPIWMDVETable, rOfretTb As New GET_MATERIAL_ATP.BAPIWMDVS
        rOfretTb.Req_Qty = 9999 : rOfretTb.Req_Date = Now.ToString("yyyyMMdd")
        retTb.Add(rOfretTb)
        p1.Bapi_Material_Availability("", "A", "", New Short, "", "", "", PartNo, UCase(Plant), "", "", "", "", "PC", "", Inventory, "", "", _
                                      New GET_MATERIAL_ATP.BAPIRETURN, atpTb, retTb)
        Inventory = 0
        Dim atpDt As DataTable = atpTb.ToADODataTable()
        For Each r As DataRow In atpDt.Rows
            Inventory += CType(r.Item("com_qty"), Integer)
        Next
        Return Inventory
    End Function

    Protected Friend Shared Function IsPTD(ByVal PartNo As String, ByVal ProdType As String) As Boolean
        Return False
        Dim f As Boolean = False
        If ProdType = "ZPER" OrElse _
            ((ProdType = "ZFIN" Or ProdType = "ZOEM") AndAlso (PartNo.StartsWith("BT") Or PartNo.StartsWith("DSD") Or PartNo.StartsWith("ES") Or PartNo.StartsWith("EWM") Or PartNo.StartsWith("GPS") Or PartNo.StartsWith("SQF") Or PartNo.StartsWith("WIFI") Or PartNo.StartsWith("PMM") Or PartNo.StartsWith("Y"))) _
            OrElse (ProdType = "ZRAW" And PartNo.StartsWith("206Q")) _
            OrElse (ProdType = "ZSEM" AndAlso PartNo.StartsWith("968Q")) Then
            f = True
        End If
        Return f
    End Function
End Class
Public Class Setting
    Public Enum PartnerType
        WE
        RE
        EM
    End Enum
End Class

