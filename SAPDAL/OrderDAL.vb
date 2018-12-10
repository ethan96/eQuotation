Public Class OrderDAL
    ''' <summary>
    ''' 'MasterReqdate  已经是页面计算过的工作日
    ''' </summary>
    Public Shared Function GetRequireDate(ByVal dt As DataTable, ByVal OrgID As String, ByVal IsHaveBtos As Boolean, _
                                                                         ByVal MasterReqdate As DateTime, ByVal IsPartial As Boolean) As DataTable
        dt.Columns.Add("reqdateX", GetType(DateTime))
        Dim LocalTime As Date = GetLocalTime(OrgID.ToString.Substring(0, 2))
        Dim _ReqDate As Date = LocalTime, _BtosLastDay As DateTime = DateTime.MinValue, _isSBCBtoOrder As Boolean = False
        For Each r As DataRow In dt.Rows
            If r.Item("reqdate") IsNot Nothing AndAlso Date.TryParse(r.Item("reqdate"), Now) Then
                If CDate(r.Item("reqdate")) > _BtosLastDay Then
                    _BtosLastDay = CDate(r.Item("reqdate"))
                End If
            End If
            If r.Item("Partno") IsNot Nothing Then
                If String.Equals(r.Item("Partno"), "SBC-BTO", StringComparison.CurrentCultureIgnoreCase) Then
                    _isSBCBtoOrder = True
                End If
            End If
        Next
        Dim C As String = GetCalendarbyOrg(Left(OrgID, 2))
        Dim Loadingdays As Integer = 0
        If IsHaveBtos Then
            Select Case OrgID
                Case "US01"
                    Loadingdays = 5
                Case "EU10"
                    Loadingdays = 7
            End Select
        End If
        If _isSBCBtoOrder Then Loadingdays = 1
        For Each r As DataRow In dt.Rows
            _ReqDate = CDate(r.Item("reqdate"))
            If _ReqDate < LocalTime Then _ReqDate = LocalTime

            If IsHaveBtos Then
                If IsPartial = False Then
                    If _ReqDate < _BtosLastDay Then
                        Dim _ReqDateTemp = CDate(_BtosLastDay).ToString("yyyy-MM-dd")
                        SAPDAL.Get_Next_WorkingDate_ByCode(_ReqDateTemp, Loadingdays.ToString, C)
                        _ReqDate = CDate(_ReqDateTemp)
                    End If
                    If _ReqDate < MasterReqdate Then
                        _ReqDate = MasterReqdate
                    End If
                Else
                    Dim _ReqDateTemp = CDate(_ReqDate).ToString("yyyy-MM-dd")
                    SAPDAL.Get_Next_WorkingDate_ByCode(_ReqDateTemp, Loadingdays.ToString, C)
                    _ReqDate = CDate(_ReqDateTemp)
                End If
            Else
                If IsPartial = False Then
                    If _ReqDate < MasterReqdate Then
                        _ReqDate = MasterReqdate
                    End If
                Else
                    Dim _ReqDateTemp = CDate(_ReqDate).ToString("yyyy-MM-dd")
                    SAPDAL.Get_Next_WorkingDate_ByCode(_ReqDateTemp, Loadingdays.ToString, C)
                    _ReqDate = CDate(_ReqDateTemp)
                End If
            End If
            r.Item("reqdateX") = _ReqDate
        Next
        dt.AcceptChanges()
        Return dt
    End Function
    Public Shared Function GetCalendarbyOrg(ByVal org As String) As String
        Dim plant As String = org & "H1"
        Dim str As String = String.Format("select LAND1 from saprdp.t001w where WERKS='{0}' and mandt='168' and rownum=1", plant)
        Dim CID As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", str)
        If Not IsNothing(CID) AndAlso CID.ToString <> "" Then
            Return CID.ToString
        End If
        Return "TW"
    End Function
    Public Shared Function GetLocalTime(ByVal org As String) As DateTime
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
End Class
