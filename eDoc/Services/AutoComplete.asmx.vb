Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class AutoComplete1
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

    <WebMethod()>
    Public Function GetPartNo(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim dt As DataTable = Nothing
        'If HttpContext.Current.Session Is Nothing Then
        '    Return Nothing
        'End If
        'contextKey = "EU10"
        prefixText = Replace(Replace(Trim(prefixText), "'", "''"), "*", "%")
        dt = Business.getProduct(contextKey, prefixText, "")
        If dt.Rows.Count > 0 Then
            Dim n As Integer = count
            If dt.Rows.Count < n Then
                n = dt.Rows.Count
            End If
            Dim str(n - 1) As String
            For i As Integer = 0 To n - 1
                str(i) = dt.Rows(i).Item(0)
            Next
            Return str
        End If
        Return Nothing
    End Function
    <WebMethod()>
    Public Function GetBTOList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim dt As DataTable = Nothing
        'If HttpContext.Current.Session Is Nothing Then
        '    Return Nothing
        'End If
        'contextKey = "EU10"
        prefixText = Replace(Replace(Trim(prefixText), "'", "''"), "*", "%")
        dt = Business.getProductBTO(contextKey, prefixText, "")
        If dt.Rows.Count > 0 Then
            Dim n As Integer = count
            If dt.Rows.Count < n Then
                n = dt.Rows.Count
            End If
            Dim str(n - 1) As String
            For i As Integer = 0 To n - 1
                str(i) = dt.Rows(i).Item(0)
            Next
            Return str
        End If
        Return Nothing
    End Function


    <WebMethod()>
    Public Function getContact(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim dt As DataTable = Nothing
        'If HttpContext.Current.Session Is Nothing Then
        '    Return Nothing
        'End If
        'contextKey = "EU10"
        prefixText = Replace(Replace(Trim(prefixText), "'", "''"), "*", "%")
        dt = Business.getContact(prefixText)
        If dt.Rows.Count > 0 Then
            Dim n As Integer = count
            If dt.Rows.Count < n Then
                n = dt.Rows.Count
            End If
            Dim str(n - 1) As String
            For i As Integer = 0 To n - 1
                str(i) = dt.Rows(i).Item(0)
            Next
            Return str
        End If
        Return Nothing
    End Function

    <WebMethod()>
    Public Function getEmployee(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim dt As DataTable = Nothing
        prefixText = Replace(Replace(Trim(prefixText), "'", "''"), "*", "%")
        dt = Business.getEmployeeList(prefixText)
        If dt.Rows.Count > 0 Then
            Dim n As Integer = count
            If dt.Rows.Count < n Then
                n = dt.Rows.Count
            End If
            Dim str(n - 1) As String
            For i As Integer = 0 To n - 1
                str(i) = dt.Rows(i).Item(0)
            Next
            Return str
        End If
        Return Nothing
    End Function

    <WebMethod(EnableSession:=True)>
    <Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetTokenInputSalesEmployee()
        Dim keyword = String.Empty
        If Not String.IsNullOrEmpty(Context.Request("q")) Then keyword = Context.Request("q").ToString().Trim()

        Dim orgid = String.Empty
        If Not String.IsNullOrEmpty(Context.Request("ORGID")) Then orgid = Context.Request("ORGID").ToString().Trim()

        keyword = Replace(Replace(Trim(keyword), "'", "''"), "*", "%")
        Dim sql As New StringBuilder
        sql.AppendFormat(" select top 20 SALES_CODE, FULL_NAME from SAP_EMPLOYEE where (SALES_CODE like '{0}%' or FULL_NAME like N'{0}%') AND PERS_AREA = '{1}' ", keyword, orgid)

        Dim dt As DataTable = Advantech.Myadvantech.DataAccess.SqlProvider.dbGetDataTable("MY", sql.ToString())
        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
            Dim list As New List(Of PNInfo)
            For i As Integer = 0 To dt.Rows.Count - 1
                list.Add(New PNInfo(dt.Rows(i).Item("SALES_CODE").ToString, dt.Rows(i).Item("FULL_NAME").ToString))
            Next
            HttpContext.Current.Response.Clear() : HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(list))
        End If
        HttpContext.Current.Response.End()
    End Function

    <WebMethod(EnableSession:=True)>
    <Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)>
    Public Sub GetDefaultEmployee(ByVal QuoteID As String)
        Dim EmployeeList As List(Of Advantech.Myadvantech.DataAccess.EQPARTNER) = Advantech.Myadvantech.DataAccess.eQuotationDAL.GetQuotePartnerByQuoteID(QuoteID).Where(Function(x) x.TYPE = "E").ToList

        Dim pninfo As PNInfo = New PNInfo(String.Empty, String.Empty)

        If EmployeeList.Count > 0 Then
            Dim employeename As Object = tbOPBase.dbExecuteScalar("MY", "select top 1 FULL_NAME from SAP_EMPLOYEE where SALES_CODE = '" + EmployeeList.FirstOrDefault.ERPID + "'")
            If Not employeename Is Nothing AndAlso Not String.IsNullOrEmpty(employeename.ToString.Trim) Then
                pninfo.id = EmployeeList.FirstOrDefault.ERPID
                pninfo.name = employeename
            End If
        End If

        System.Web.HttpContext.Current.Response.Clear()
        System.Web.HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(pninfo))
    End Sub

    <WebMethod(EnableSession:=True)>
    <Web.Script.Services.ScriptMethod(ResponseFormat:=Script.Services.ResponseFormat.Json)>
    Public Function GetTokenInput207Parts()
        Dim keyword = String.Empty
        If Not String.IsNullOrEmpty(Context.Request("q")) Then keyword = Context.Request("q").ToString().Trim()

        Dim orgid = String.Empty
        If Not String.IsNullOrEmpty(Context.Request("ORGID")) Then orgid = Context.Request("ORGID").ToString().Trim()

        keyword = Replace(Replace(Trim(keyword), "'", "''"), "*", "%")
        Dim sql As New StringBuilder
        sql.AppendFormat(" select a.PART_NO, a.PRODUCT_DESC from SAP_PRODUCT a inner join SAP_PRODUCT_STATUS_ORDERABLE b on a.PART_NO = b.PART_NO where a.PART_NO like '%{0}%' and a.MATERIAL_GROUP = '207' and b.SALES_ORG = '{1}' ", keyword, orgid)

        Dim dt As DataTable = Advantech.Myadvantech.DataAccess.SqlProvider.dbGetDataTable("MY", sql.ToString())
        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
            Dim list As New List(Of PNInfo)
            For i As Integer = 0 To dt.Rows.Count - 1
                list.Add(New PNInfo(dt.Rows(i).Item("PRODUCT_DESC").ToString, dt.Rows(i).Item("PART_NO").ToString))
            Next
            HttpContext.Current.Response.Clear() : HttpContext.Current.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(list))
        End If
        HttpContext.Current.Response.End()
    End Function

    Public Class PNInfo
        Public Property id As String : Public Property name As String
        Public Sub New(ByVal k As String, ByVal v As String)
            Me.id = k : Me.name = v
        End Sub
    End Class
End Class