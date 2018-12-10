Imports System.Web.Services
Imports System.Web.Script.Serialization

Public Class Opportunity_Management
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            '取得登入者可以看到哪些Siebel RBU
            Dim RBUArrayList As ArrayList = Pivot.CurrentProfile.VisibleRBU
            Dim strRBU As String = String.Empty
            If RBUArrayList.Count > 0 Then
                For i As Integer = 0 To RBUArrayList.Count - 1
                    If i = 0 Then
                        strRBU = RBUArrayList.Item(i).ToString
                    Else
                        strRBU += "," + RBUArrayList.Item(i).ToString
                    End If
                Next
                HF_RBU.Value = strRBU
            End If
        End If
    End Sub

    <System.Web.Services.WebMethod()> _
  <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function GetData(ByVal ipage As String, ByVal irows As String, ByVal _ProjectName As String, ByVal _AccountName As String, ByVal _AccountStatus As String, ByVal _ERPID As String, ByVal _RBU As String, ByVal _Stage As String) As String
        Dim JsonStr = String.Empty
        Dim dt As New DataTable
        Dim db As New CRMDBDataContext
        Dim serializer As New JavaScriptSerializer
        Dim _rows As New List(Of Dictionary(Of String, Object))
        Dim _row As Dictionary(Of String, Object) = Nothing
        Dim result As New Dictionary(Of String, Object)
        Dim startRow As Integer = 0, endRow As Integer = 0
        Dim rowtotal As Integer = 0
        Dim arrRBU As String()
        Dim arrStage As String() = {"1-VXVAIE", "1-VXVAID", "1-3HQKE9", "1-3HQKGR"}
        db.CommandTimeout = 30

        'RBU為空白就直接回傳空的
        If _RBU = "" Then
            result.Add("total", 0)
            _row = New Dictionary(Of String, Object)
            _row.Add("action", "RBU is Empty")
            _rows.Add(_row)
            result.Add("rows", _rows)

            JsonStr = serializer.Serialize(result)
            Return JsonStr
        Else
            arrRBU = _RBU.Split(",")
        End If

        Try


        Dim _result
        If _ProjectName = "" And _AccountName = "" And _AccountStatus = "" And _ERPID = "" Then
            If _Stage = 0 Then 'Open 抓取除了0%和100%
                    _result = (From v In db.S_ORG_EXTs
                           From x In db.S_ORG_EXT_Xes.Where(Function(x) CBool(x.ROW_ID = v.ROW_ID)).DefaultIfEmpty()
                           From o In db.S_OPTies.Where(Function(o) CBool(v.ROW_ID = o.PR_DEPT_OU_ID)).DefaultIfEmpty()
                           From s In db.S_STGs.Where(Function(s) CBool(o.CURR_STG_ID = s.ROW_ID))
                           Where (arrRBU.Contains(v.S_BU.NAME) And Not arrStage.Contains(o.CURR_STG_ID))
                           Order By v.NAME, o.CREATED Descending
                            Select CURCY = o.CURCY_CD, RBU = v.S_BU.NAME, ProjectName = o.NAME, AccRowID = v.ROW_ID, AccountName = v.NAME, AccountStatus = v.CUST_STAT_CD, ERPID = x.ATTRIB_05, AMT = o.SUM_REVN_AMT, SalesStageID = o.CURR_STG_ID, SalesStage = s.NAME, DESC = o.DESC_TEXT, PRIMARY = o.S_POSTN.S_USER.LOGIN, CREATED = o.CREATED, CREATEDBY = o.S_USER.LOGIN, RowID = o.ROW_ID).Take(1000).ToList
            Else 'All 抓取全部
                    _result = (From v In db.S_ORG_EXTs
                             From x In db.S_ORG_EXT_Xes.Where(Function(x) CBool(x.ROW_ID = v.ROW_ID)).DefaultIfEmpty()
                             From o In db.S_OPTies.Where(Function(o) CBool(v.ROW_ID = o.PR_DEPT_OU_ID)).DefaultIfEmpty()
                             From s In db.S_STGs.Where(Function(s) CBool(o.CURR_STG_ID = s.ROW_ID))
                             Where (arrRBU.Contains(v.S_BU.NAME))
                             Order By v.NAME, o.CREATED Descending
                              Select CURCY = o.CURCY_CD, RBU = v.S_BU.NAME, ProjectName = o.NAME, AccRowID = v.ROW_ID, AccountName = v.NAME, AccountStatus = v.CUST_STAT_CD, ERPID = x.ATTRIB_05, AMT = o.SUM_REVN_AMT, SalesStageID = o.CURR_STG_ID, SalesStage = s.NAME, DESC = o.DESC_TEXT, PRIMARY = o.S_POSTN.S_USER.LOGIN, CREATED = o.CREATED, CREATEDBY = o.S_USER.LOGIN, RowID = o.ROW_ID).Take(1000).ToList
            End If
        Else
            If _Stage = 0 Then 'Open 抓取除了0%和100%
                    _result = (From v In db.S_ORG_EXTs
                              From x In db.S_ORG_EXT_Xes.Where(Function(x) CBool(x.ROW_ID = v.ROW_ID)).DefaultIfEmpty()
                              From o In db.S_OPTies.Where(Function(o) CBool(v.ROW_ID = o.PR_DEPT_OU_ID)).DefaultIfEmpty()
                              From s In db.S_STGs.Where(Function(s) CBool(o.CURR_STG_ID = s.ROW_ID))
                              Where (arrRBU.Contains(v.S_BU.NAME) And o.NAME.ToLower.Contains(_ProjectName.ToLower) And v.NAME.ToLower.Contains(_AccountName.ToLower) And v.CUST_STAT_CD.ToLower.Contains(_AccountStatus.ToLower) And If(x.ATTRIB_05 Is Nothing, "", x.ATTRIB_05.ToLower).Contains(_ERPID.ToLower) And Not arrStage.Contains(o.CURR_STG_ID))
                              Order By v.NAME, o.CREATED Descending
                             Select CURCY = o.CURCY_CD, RBU = v.S_BU.NAME, ProjectName = o.NAME, AccRowID = v.ROW_ID, AccountName = v.NAME, AccountStatus = v.CUST_STAT_CD, ERPID = x.ATTRIB_05, AMT = o.SUM_REVN_AMT, SalesStageID = o.CURR_STG_ID, SalesStage = s.NAME, DESC = o.DESC_TEXT, PRIMARY = o.S_POSTN.S_USER.LOGIN, CREATED = o.CREATED, CREATEDBY = o.S_USER.LOGIN, RowID = o.ROW_ID).ToList
            Else 'All 抓取全部
                    _result = (From v In db.S_ORG_EXTs
                               From x In db.S_ORG_EXT_Xes.Where(Function(x) CBool(x.ROW_ID = v.ROW_ID)).DefaultIfEmpty()
                               From o In db.S_OPTies.Where(Function(o) CBool(v.ROW_ID = o.PR_DEPT_OU_ID)).DefaultIfEmpty()
                               From s In db.S_STGs.Where(Function(s) CBool(o.CURR_STG_ID = s.ROW_ID))
                               Where (arrRBU.Contains(v.S_BU.NAME) And o.NAME.ToLower.Contains(_ProjectName.ToLower) And v.NAME.ToLower.Contains(_AccountName.ToLower) And v.CUST_STAT_CD.ToLower.Contains(_AccountStatus.ToLower) And If(x.ATTRIB_05 Is Nothing, "", x.ATTRIB_05.ToLower).Contains(_ERPID.ToLower))
                               Order By v.NAME, o.CREATED Descending
                              Select CURCY = o.CURCY_CD, RBU = v.S_BU.NAME, ProjectName = o.NAME, AccRowID = v.ROW_ID, AccountName = v.NAME, AccountStatus = v.CUST_STAT_CD, ERPID = x.ATTRIB_05, AMT = o.SUM_REVN_AMT, SalesStageID = o.CURR_STG_ID, SalesStage = s.NAME, DESC = o.DESC_TEXT, PRIMARY = o.S_POSTN.S_USER.LOGIN, CREATED = o.CREATED, CREATEDBY = o.S_USER.LOGIN, RowID = o.ROW_ID).ToList
            End If
        End If
            dt.Columns.Add("CURCY", GetType(String))
            dt.Columns.Add("RBU", GetType(String))
            dt.Columns.Add("ERPID", GetType(String))
            dt.Columns.Add("AccRowID", GetType(String))
            dt.Columns.Add("AccountName", GetType(String))
            dt.Columns.Add("ProjectName", GetType(String))
            dt.Columns.Add("AccountStatus", GetType(String))
            dt.Columns.Add("AMT", GetType(String))
            'dt.Columns.Add("SalesStageID", GetType(String))
            dt.Columns.Add("SalesStage", GetType(String))
            dt.Columns.Add("DESC", GetType(String))
            dt.Columns.Add("PRIMARY", GetType(String))
            dt.Columns.Add("CREATED", GetType(String))
            dt.Columns.Add("CREATEDBY", GetType(String))
            dt.Columns.Add("RowID", GetType(String))

            Dim _datarow As DataRow
            For Each item In _result
                _datarow = dt.NewRow
                _datarow.Item("CURCY") = Util.FormatCurrency(item.CURCY)
                _datarow.Item("RBU") = item.RBU
                _datarow.Item("ERPID") = item.ERPID
                _datarow.Item("AccRowID") = item.AccRowID
                _datarow.Item("AccountName") = item.AccountName
                _datarow.Item("ProjectName") = item.ProjectName
                _datarow.Item("AccountStatus") = item.AccountStatus
                _datarow.Item("AMT") = Math.Round(item.AMT, 2)
                '_datarow.Item("SalesStageID") = item.SalesStageID
                _datarow.Item("SalesStage") = item.SalesStage
                _datarow.Item("DESC") = item.DESC
                _datarow.Item("PRIMARY") = item.PRIMARY
                _datarow.Item("CREATED") = item.CREATED
                _datarow.Item("CREATEDBY") = item.CREATEDBY
                _datarow.Item("RowID") = item.RowID
                dt.Rows.Add(_datarow)
            Next

        If dt.Rows.Count <> 0 Then

            '最後一頁的判斷
            rowtotal = ipage * irows
            If (dt.Rows.Count - rowtotal) > 0 Then
                endRow = rowtotal '非最後一頁就為每頁筆數*第幾頁
                startRow = endRow - irows
            Else
                endRow = dt.Rows.Count '最後一頁就為原總筆數
                startRow = (ipage - 1) * irows '最後一頁的起始筆數= (第幾頁-1)* 每頁筆數
            End If

            '總筆數
            result.Add("total", dt.Rows.Count)

            For i As Integer = startRow To endRow - 1
                Dim dr As DataRow = dt.Rows(i)
                _row = New Dictionary(Of String, Object)
                For Each col As DataColumn In dt.Columns
                    _row.Add(col.ColumnName.Trim(), dr(col))
                Next
                _rows.Add(_row)
            Next

            result.Add("rows", _rows)

        Else
            result.Add("total", dt.Rows.Count)
            _row = New Dictionary(Of String, Object)
            _row.Add("action", "No Records Found")
            _rows.Add(_row)
            result.Add("rows", _rows)
        End If

        JsonStr = serializer.Serialize(result)
            Return JsonStr

        Catch ex As Exception

            '查詢失敗就回傳Error
            result.Add("total", 0)
            _row = New Dictionary(Of String, Object)
            _row.Add("action", ex.ToString)
            _rows.Add(_row)
            result.Add("rows", _rows)

            JsonStr = serializer.Serialize(result)
            Return JsonStr
        End Try
    End Function

    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function getERPIDData(_ERPID As String, _RBU As String) As String
        Dim serializer As New JavaScriptSerializer
        Dim JsonStr = String.Empty
        Dim result As New List(Of KeyValue)
        Dim db As New CRMDBDataContext
        Dim _result = Nothing
        Dim arrRBU As String()

        'RBU為空白就直接回傳空的
        If _RBU = "" Then
            result.Add(New KeyValue("", ""))

            JsonStr = serializer.Serialize(result)
            Return JsonStr
        Else
            arrRBU = _RBU.Split(",")
        End If

        If _ERPID <> "" Then
            _result = (From x In db.S_ORG_EXT_Xes
                       From v In db.S_ORG_EXTs.Where(Function(v) CBool(x.ROW_ID = v.ROW_ID)).DefaultIfEmpty()
                       Where (arrRBU.Contains(v.S_BU.NAME) And x.ATTRIB_05.ToLower.Contains(_ERPID.ToLower))
                       Select ERPID = x.ATTRIB_05).Distinct().Take(20).ToList
        Else
            _result = (From x In db.S_ORG_EXT_Xes
                       From v In db.S_ORG_EXTs.Where(Function(v) CBool(x.ROW_ID = v.ROW_ID)).DefaultIfEmpty()
                       Where (arrRBU.Contains(v.S_BU.NAME))
                       Select ERPID = x.ATTRIB_05).Distinct().Take(20).ToList
        End If

        For Each Item In _result
            If (Item = Nothing) Then
                result.Add(New KeyValue("", ""))
            Else
                result.Add(New KeyValue(Item.ToString(), Item.ToString()))
            End If
        Next

        JsonStr = serializer.Serialize(result)
        Return JsonStr
    End Function

    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function getAccountNamData(_AccountName As String, _RBU As String) As String
        Dim serializer As New JavaScriptSerializer
        Dim JsonStr = String.Empty
        Dim result As New List(Of KeyValue)
        Dim db As New CRMDBDataContext
        Dim _result = Nothing
        Dim arrRBU As String()

        'RBU為空白就直接回傳空的
        If _RBU = "" Then
            result.Add(New KeyValue("", ""))

            JsonStr = serializer.Serialize(result)
            Return JsonStr
        Else
            arrRBU = _RBU.Split(",")
        End If

        If _AccountName <> "" Then
            _result = (From v In db.S_ORG_EXTs
                      Where (arrRBU.Contains(v.S_BU.NAME) And v.NAME.ToLower.Contains(_AccountName.ToLower))
                   Select AccountName = v.NAME).Distinct().Take(20).ToList
        Else
            _result = (From v In db.S_ORG_EXTs
                       Where (arrRBU.Contains(v.S_BU.NAME))
                   Select AccountName = v.NAME).Distinct().Take(20).ToList
        End If

        For Each Item In _result
            If (Item = Nothing) Then
                result.Add(New KeyValue("", ""))
            Else
                result.Add(New KeyValue(Item.ToString(), Item.ToString()))
            End If
        Next

        JsonStr = serializer.Serialize(result)
        Return JsonStr
    End Function

    '  <System.Web.Services.WebMethod()> _
    '<System.Web.Script.Services.ScriptMethod()> _
    '  Public Shared Function getProjectNamData(_ProjectName As String) As String
    '      Dim serializer As New JavaScriptSerializer
    '      Dim JsonStr = String.Empty
    '      Dim result As New List(Of KeyValue)
    '      Dim db As New CRMDBDataContext
    '      Dim _result = Nothing

    '      If _ProjectName <> "" Then
    '          _result = (From o In db.S_OPTies
    '                    Where (o.NAME.ToLower.Contains(_ProjectName.ToLower))
    '                 Select ProjectName = o.NAME).Distinct().Take(20).ToList
    '      Else
    '          _result = (From o In db.S_OPTies
    '                 Select ProjectName = o.NAME).Distinct().Take(20).ToList
    '      End If

    '      For Each Item In _result
    '          If (Item = Nothing) Then
    '              result.Add(New KeyValue("", ""))
    '          Else
    '              result.Add(New KeyValue(Item.ToString(), Item.ToString()))
    '          End If
    '      Next

    '      JsonStr = serializer.Serialize(result)
    '      Return JsonStr
    '  End Function

    <System.Web.Services.WebMethod()> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function getAccountStatusData() As String
        Dim serializer As New JavaScriptSerializer
        Dim JsonStr = String.Empty
        Dim result As New List(Of KeyValue)
        Dim db As New CRMDBDataContext
        Dim _result = Nothing

        _result = (From v In db.S_ORG_EXTs
                   Select AccountStatus = v.CUST_STAT_CD).Distinct().ToList
        For Each Item In _result
            If (Item = Nothing) Then
                result.Add(New KeyValue("", ""))
            Else
                result.Add(New KeyValue(Item.ToString(), Item.ToString()))
            End If
        Next

        JsonStr = serializer.Serialize(result)
        Return JsonStr
    End Function

    <System.Web.Services.WebMethod()> _
   <System.Web.Script.Services.ScriptMethod()> _
    Public Shared Function UpdateOOP(RowID As String, AMT As String, OOP As String, DESC As String) As String
        Dim serializer As New JavaScriptSerializer
        Dim JsonStr = String.Empty
        'Dim bkOOP As New backOOP("", "", "")
        Dim result As String = String.Empty
        Dim siebelWS As New SiebelWS.Siebel_WS

        Dim amount As Decimal = 0.0
        Decimal.TryParse(AMT, amount)

        If (RowID <> "") Then
            'siebelWS.UpdateOpportunityStatusAmtCloseDateProb(RowID, "", DESC, AMT, Nothing, OOP)
            'siebelWS.UpdateOpportunityStage(RowID, OOP, DESC, AMT, Nothing)

            'ICC 2017/2/24 Change to new Siebel web service to update opportunity
            Dim r As Boolean = Advantech.Myadvantech.DataAccess.SiebelDAL.UpdateOptyStageV2(RowID, OOP, amount, DESC, Nothing, result)
            If r = True Then result = "Updated"
        End If

        'result = "Updated"

        JsonStr = serializer.Serialize(result)
        Return JsonStr
    End Function

    'Class backOOP
    '    Public ErrorMessage As String : Public SentOOP As String : Public SentAMT As String
    '    Public Sub New(ByVal err As String, OOP As String, AMT As String)
    '        ErrorMessage = err : SentOOP = OOP : SentAMT = AMT
    '    End Sub
    'End Class

    Class KeyValue
        Public Key As String : Public Value As String
        Public Sub New(ByVal skey As String, svalue As String)
            Key = skey : Value = svalue
        End Sub
    End Class

    '<System.Web.Services.WebMethod()> _
    '<System.Web.Script.Services.ScriptMethod()> _
    'Public Shared Function GetDetailData(ByVal detailID As String) As String
    '    Dim JsonStrDetail As String = String.Empty

    '    Dim dt As New DataTable
    '    Dim db As New CRMDBDataContext
    '    Dim serializer As New JavaScriptSerializer
    '    Dim _rows As New List(Of Dictionary(Of String, Object))
    '    Dim _row As Dictionary(Of String, Object) = Nothing
    '    Dim result As New Dictionary(Of String, Object)
    '    Dim startRow As Integer = 0, endRow As Integer = 0
    '    Dim rowtotal As Integer = 0

    '    Dim _result
    '    If detailID = "" Then
    '        _result = (From v In db.S_OPTies
    '                  From p In db.S_POSTNs.Where(Function(p) CBool(v.PR_POSTN_ID = p.ROW_ID)).DefaultIfEmpty()
    '                  From x In db.S_USERs.Where(Function(x) CBool(p.PR_EMP_ID = x.ROW_ID)).DefaultIfEmpty()
    '        Select ID = v.PR_DEPT_OU_ID, Name = v.NAME, AMT = v.SUM_REVN_AMT, OOP = v.SUM_WIN_PROB, OWNER = x.LOGIN).Take(100).ToList
    '    Else
    '        _result = (From v In db.S_OPTies Where v.PR_DEPT_OU_ID = detailID
    '          From p In db.S_POSTNs.Where(Function(p) CBool(v.PR_POSTN_ID = p.ROW_ID)).DefaultIfEmpty()
    '                  From x In db.S_USERs.Where(Function(x) CBool(p.PR_EMP_ID = x.ROW_ID)).DefaultIfEmpty()
    '        Select ID = v.PR_DEPT_OU_ID, Name = v.NAME, AMT = v.SUM_REVN_AMT, OOP = v.SUM_WIN_PROB, OWNER = x.LOGIN).Take(100).ToList
    '    End If

    '    dt.Columns.Add("ID", GetType(String))
    '    dt.Columns.Add("Name", GetType(String))
    '    dt.Columns.Add("AMT", GetType(String))
    '    dt.Columns.Add("OOP", GetType(String))
    '    dt.Columns.Add("OWNER", GetType(String))

    '    Dim _datarow As DataRow
    '    For Each item In _result
    '        _datarow = dt.NewRow

    '        _datarow.Item("ID") = item.ID
    '        _datarow.Item("Name") = item.Name
    '        _datarow.Item("AMT") = CStr(CInt(item.AMT))
    '        _datarow.Item("OOP") = CStr(CInt(item.OOP)) & "%"
    '        _datarow.Item("OWNER") = item.OWNER

    '        dt.Rows.Add(_datarow)
    '    Next

    '    If dt.Rows.Count <> 0 Then

    '        '總筆數
    '        result.Add("total", dt.Rows.Count)

    '        For i As Integer = 0 To dt.Rows.Count - 1
    '            Dim dr As DataRow = dt.Rows(i)
    '            _row = New Dictionary(Of String, Object)
    '            For Each col As DataColumn In dt.Columns
    '                _row.Add(col.ColumnName.Trim(), dr(col))
    '            Next
    '            _rows.Add(_row)
    '        Next

    '        result.Add("rows", _rows)

    '    Else
    '        result.Add("total", dt.Rows.Count)
    '        _row = New Dictionary(Of String, Object)
    '        _row.Add("ID", "No Records Found")
    '        _rows.Add(_row)
    '        result.Add("rows", _rows)
    '    End If

    '    JsonStrDetail = serializer.Serialize(result)
    '    Return JsonStrDetail
    'End Function
End Class