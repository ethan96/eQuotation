Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Configuration
Public Class tbBase
    Sub New(ByVal tbName As String)
        Me._tbSource = New tbSource("EQ", tbName)
    End Sub
    Sub New(ByVal conn As String, ByVal tbName As String)
        Me._tbSource = New tbSource(conn, tbName)
    End Sub
    Private _tbSource As tbSource
    Public Property tbSource As tbSource
        Get
            Return _tbSource
        End Get
        Set(ByVal value As tbSource)
            _tbSource = value
        End Set
    End Property

    Public Function GetDTbySelectStr(ByVal selectStr As String) As DataTable
        Dim dt As DataTable = dbUtil.dbGetDataTable(_tbSource.conn, selectStr)
        Return dt
    End Function
    Public Function GetScalar(ByVal whereStr As String, ByVal orderStr As String, Optional ByVal Columnname As String = "*") As Object
        Dim str As String = ""
        If whereStr <> "" Then
            whereStr = "where " & whereStr
        End If
        If orderStr <> "" Then
            orderStr = "order by " & orderStr
        End If
        str = String.Format("select {3} from {0} {1} {2}", _tbSource.tbName, whereStr, orderStr, Columnname)
        Return dbUtil.dbExecuteScalar(_tbSource.conn, str)
    End Function
    Public Function GetDT(ByVal whereStr As String, ByVal orderStr As String, Optional ByVal Columnname As String = "*") As DataTable
        Dim str As String = ""
        If whereStr <> "" Then
            whereStr = "where " & whereStr
        End If
        If orderStr <> "" Then
            orderStr = "order by " & orderStr
        End If
        str = String.Format("select {3} from {0} {1} {2}", _tbSource.tbName, whereStr, orderStr, Columnname)
        Dim dt As DataTable = dbUtil.dbGetDataTable(_tbSource.conn, str)
        Return dt
    End Function

    Public Function Delete(ByVal whereStr As String) As Integer
        Dim str As String = String.Format("delete from {1} where {0}", whereStr, _tbSource.tbName)
        If whereStr = "" Then
            str = String.Format("delete from {0} ", _tbSource.tbName)
        End If
        dbUtil.dbExecuteNoQuery(_tbSource.conn, str)
        Return 1
    End Function

    Public Function Update(ByVal whereStr As String, ByVal setStr As String) As Integer
        Dim str As String = String.Format("update {2} set {0} where {1}", setStr, whereStr, _tbSource.tbName)
        Return dbUtil.dbExecuteNoQuery(_tbSource.conn, str)
    End Function

    Public Function IsExists(ByVal whereStr As String) As Integer
        Dim dt As DataTable = dbUtil.dbGetDataTable(_tbSource.conn, String.Format("select top 1 * from {1} where {0}", whereStr, _tbSource.tbName))
        If dt.Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function

    Public Function SqlBulkCopy(ByVal dt As DataTable) As Integer
        Dim bk As New System.Data.SqlClient.SqlBulkCopy(ConfigurationManager.ConnectionStrings(_tbSource.conn).ConnectionString)
        bk.DestinationTableName = _tbSource.tbName
        bk.WriteToServer(dt)
        Return 1
    End Function

    Public Overloads Function Add() As Integer
        Return 1
    End Function
End Class
