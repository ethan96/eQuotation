Public Class PATC : Implements IBUS.iPatch

    Public Function IsEUStockingCompany(ByVal companyId As String) As Boolean Implements IBUS.iPatch.IsEUStockingCompany
        Dim dt As DataTable = dbUtil.dbGetDataTable("MYLOCAL", String.Format("SELECT top 1 COMPANY_ID FROM ADMIN_PREFERENTIAL_PRODS where COMPANY_ID ='{0}'", companyId))
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Public Function IsEUStockingProgram(ByVal PartNO As String, ByVal QTY As Integer, ByVal CompanyId As String) As Boolean Implements IBUS.iPatch.IsEUStockingProgram
        Dim SQL As String = String.Format("select count(*) from ADMIN_PREFERENTIAL_PRODS where COMPANY_ID='{0}' and  PART_NO ='{1}' and {2}>=MIN_ORDER_QTY", _
                                      CompanyId.ToString, PartNO, QTY)
        Dim obj As Object = dbUtil.dbExecuteScalar("MYLOCAL", SQL)
        If obj IsNot Nothing AndAlso CInt(obj) > 0 Then Return True
        Return False
    End Function

    Public Function isHasBto(ByVal CartList As IBUS.iCartList) As Boolean Implements IBUS.iPatch.isHasBto
        If Not IsNothing(CartList) Then
            For Each r As iCartLine In CartList
                If r.itemType.Value = COMM.Fixer.eItemType.Parent Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Public Function isODMCart(ByVal CartList As IBUS.iCartList) As Boolean Implements IBUS.iPatch.isODMCart
        If Not IsNothing(CartList) Then
            For Each r As iCartLine In CartList
                If r.partNo.Value = "ODM-CPCI1109-BTO" Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Public Function isSBCBtoOrder(ByVal CartList As IBUS.iCartList, ByVal ERPID As String) As Boolean Implements IBUS.iPatch.isSBCBtoOrder
        If Not IsNothing(CartList) Then
            For Each r As iCartLine In CartList
                If r.partNo.Value = "SBC-BTO" Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Public Function ReplaceCartBTO(ByVal PartNo As String, ByVal Org As String) As String Implements IBUS.iPatch.ReplaceCartBTO

        If PartNo.StartsWith("EZ-") Then PartNo = PartNo.Substring(3, PartNo.Length - 3)
        Dim vnumber As Object = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", PartNo.ToString, Org.ToString.ToUpper.Substring(0, 2)))
        If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
            Return vnumber
        End If
        vnumber = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", PartNo.ToString.Trim + "-BTO", Org.ToString.ToUpper.Substring(0, 2)))
        If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
            Return vnumber
        End If
        If PartNo.Trim.EndsWith("-BTO") Then
            Dim Temp_PN = PartNo.Substring(0, PartNo.Length - 4)
            vnumber = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", Temp_PN, Org.ToString.ToUpper.Substring(0, 2)))
            If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
                Return vnumber
            End If
        Else
            Return PartNo
        End If
        Return PartNo
    End Function
    Public Function GetCustomerOffice(ByVal connectionName As String, ByVal sql As String) As String Implements IBUS.iPatch.GetCustomerOffice
        Dim R As Object = dbUtil.dbExecuteScalar(connectionName, sql)
        If R IsNot Nothing Then
            Return R.ToString()
        End If
        Return String.Empty
    End Function
End Class
Public Class DueDate : Implements IBUS.iDueDate
    Private _errCode As COMM.Msg.eErrCode = Msg.eErrCode.OK
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property
End Class

Public Class OptyQuoteLine : Implements IBUS.iOptyQuoteLine
    Private _Opty_Owner_Email As String = ""
    Public Property Opty_Owner_Email As String Implements IBUS.iOptyQuoteLine.Opty_Owner_Email
        Get
            Return _Opty_Owner_Email
        End Get
        Set(ByVal value As String)
            _Opty_Owner_Email = value
        End Set
    End Property
    Private _optyId As String = ""
    Public Property optyId As String Implements IBUS.iOptyQuoteLine.optyId
        Get
            Return _optyId
        End Get
        Set(ByVal value As String)
            _optyId = value
        End Set
    End Property
    Private _optyName As String = ""
    Public Property optyName As String Implements IBUS.iOptyQuoteLine.optyName
        Get
            Return _optyName
        End Get
        Set(ByVal value As String)
            _optyName = value
        End Set
    End Property
    Private _optyStage As String = ""
    Public Property optyStage As String Implements IBUS.iOptyQuoteLine.optyStage
        Get
            Return _optyStage
        End Get
        Set(ByVal value As String)
            _optyStage = value
        End Set
    End Property
    Private _quoteId As String = ""
    Public Property quoteId As String Implements IBUS.iOptyQuoteLine.quoteId
        Get
            Return _quoteId
        End Get
        Set(ByVal value As String)
            _quoteId = value
        End Set
    End Property
End Class

Public Class OptyQuoteC : Implements IBUS.iOptyQuote

    Private _errCode As COMM.Msg.eErrCode = Msg.eErrCode.OK
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property

    Public Sub DeleteByOptyID(ByVal OptyId As String) Implements IBUS.iOptyQuote.DeleteByOptyID
        Dim O As New optyQuote
        O.Delete(String.Format("optyId='{0}'", OptyId))
    End Sub

    Public Sub DeleteByQuoteID(ByVal QuoteId As String) Implements IBUS.iOptyQuote.DeleteByQuoteID
        Dim O As New optyQuote
        O.Delete(String.Format("quoteId='{0}'", QuoteId))
    End Sub

    Public Function GetListByOptyID(ByVal OptyID As String) As System.Collections.Generic.List(Of IBUS.iOptyQuoteLine) Implements IBUS.iOptyQuote.GetListByOptyID
        Dim O As New optyQuote
        Dim DT As DataTable = O.GetDT(String.Format("optyId='{0}'", OptyID), "")
        Return DT2LIST(DT)
    End Function
    Private Function DT2LIST(ByVal DT As DataTable) As List(Of IBUS.iOptyQuoteLine)
        If Not IsNothing(DT) AndAlso DT.Rows.Count > 0 Then
            Dim pl As New List(Of IBUS.iOptyQuoteLine)
            For Each r As DataRow In DT.Rows
                Dim p As New OptyQuoteLine
                If Not IsDBNull(r.Item("optyId")) Then
                    p.optyId = r.Item("optyId")
                End If
                If Not IsDBNull(r.Item("quoteId")) Then
                    p.quoteId = r.Item("quoteId")
                End If
                If Not IsDBNull(r.Item("optyName")) Then
                    p.optyName = r.Item("optyName")
                End If
                If Not IsDBNull(r.Item("optyStage")) Then
                    p.optyStage = r.Item("optyStage")
                End If
                If Not IsDBNull(r.Item("Opty_Owner_Email")) Then
                    p.Opty_Owner_Email = r.Item("Opty_Owner_Email")
                End If
                pl.Add(p)
            Next
            Return pl
        End If
        Return Nothing
    End Function
    Public Function GetListByQuoteID(ByVal QuoteId As String) As System.Collections.Generic.List(Of IBUS.iOptyQuoteLine) Implements IBUS.iOptyQuote.GetListByQuoteID
        Dim O As New optyQuote
        Dim DT As DataTable = O.GetDT(String.Format("quoteId='{0}'", QuoteId), "")
        Return DT2LIST(DT)
    End Function

    Public Sub Add(ByVal OptyQuote As IBUS.iOptyQuoteLine) Implements IBUS.iOptyQuote.Add
        Dim O As New optyQuote
        O.Add(OptyQuote.optyId, OptyQuote.optyName, OptyQuote.quoteId, OptyQuote.optyStage, OptyQuote.Opty_Owner_Email)
    End Sub
End Class
