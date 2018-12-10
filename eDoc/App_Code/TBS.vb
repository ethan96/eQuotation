Imports Microsoft.VisualBasic
Imports System.Data

Public Class TempInfo : Inherits tbBase

    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub

    Public Overloads Function Add(ByVal UId As String, ByVal message As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}')", _
                                          Me.tb, _
                                          UId, _
                                          message)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class

Public Class rRole : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub

    Public Overloads Function Add(ByVal UId As String, ByVal RoleName As String, ByVal RoleValue As Decimal) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',N'{3}')", _
                                          Me.tb, _
                                          UId, _
                                          RoleName, _
                                          RoleValue)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class

Public Class User : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub

    Public Overloads Function Add(ByVal UId As String, ByVal UserName As String, ByVal RoleID As String, ByVal Password As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',N'{3}',N'{4}')", _
                                          Me.tb, _
                                          UId, _
                                          UserName, _
                                          RoleID, _
                                          Password)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class

Public Class RoleManagement : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub

    Public Overloads Function Add(ByVal URL As String, ByVal Name As String, ByVal Value As Decimal, ByVal seq As Integer, ByVal _CLASS As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',N'{3}',N'{4}',N'{5}')", _
                                          Me.tb, _
                                           URL, _
                                           Name, _
                                           Value, _
                                           seq, _
                                           _CLASS)
        tbOPBase.dbExecuteNoQuery(Me.conn, Str)
        Return 1
    End Function
End Class

Public Class quotationDetail : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
    Public Overloads Function Add(ByVal quoteId As String, _
                                        ByVal lineNo As Integer, _
                                        ByVal partNo As String, _
                                        ByVal description As String, _
                                        ByVal qty As Integer, _
                                        ByVal listPrice As Decimal, _
                                        ByVal unitPrice As Decimal, _
                                        ByVal newUnitPrice As Decimal, _
                                        ByVal itp As Decimal, _
                                        ByVal newItp As Decimal, _
                                        ByVal deliveryPlant As String, _
                                        ByVal category As String, _
                                        ByVal classABC As String, _
                                        ByVal rohs As Integer, _
                                        ByVal ewFlag As Integer, _
                                        ByVal reqDate As Date, _
                                        ByVal dueDate As Date, _
                                        ByVal satisfyFlag As Integer, _
                                        ByVal canBeConfirmed As Integer, _
                                        ByVal custMaterial As String, _
                                        ByVal inventory As Integer, _
                                        ByVal oType As Integer, _
                                        ByVal modelNo As String, _
                                        ByVal SPRNO As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}',N'{9}',N'{10}',N'{11}',N'{12}',N'{13}',N'{14}',N'{15}','{16}','{17}',N'{18}',N'{19}',N'{20}',N'{21}',N'{22}',N'{23}',N'{24}')", _
                                          Me.tb, _
                                          quoteId, _
                                          lineNo, _
                                          partNo, _
                                          description, _
                                          qty, _
                                          listPrice, _
                                          unitPrice, _
                                          newUnitPrice, _
                                          itp, _
                                          newItp, _
                                          deliveryPlant, _
                                          category, _
                                          classABC, _
                                          rohs, _
                                          ewFlag, _
                                          reqDate.ToShortDateString, _
                                          dueDate.ToShortDateString, _
                                          satisfyFlag, _
                                          canBeConfirmed, _
                                          custMaterial, _
                                          inventory, _
                                          oType, _
                                          modelNo, _
                                          SPRNO)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
    Public Function Copy(ByVal orgQuoteId As String, ByVal newQuoteId As String) As Integer
        Dim myQuoteDetail As New quotationDetail("EQ", "quotationDetail")
        myQuoteDetail.Delete(String.Format("quoteId='{0}'", newQuoteId))
        Dim dth As DataTable = myQuoteDetail.GetDT(String.Format("quoteId='{0}'", orgQuoteId), "line_No")
        If dth.Rows.Count > 0 Then
            For Each r As DataRow In dth.Rows
                r.Item("quoteId") = newQuoteId
            Next
            Dim bk As New System.Data.SqlClient.SqlBulkCopy(ConfigurationManager.ConnectionStrings(myQuoteDetail.conn).ConnectionString)
            bk.DestinationTableName = myQuoteDetail.tb
            bk.WriteToServer(dth)
        End If
        Return 1
    End Function
    Public Function isBtoOrder(ByVal quoteId As String) As Integer
        Dim dt As DataTable = Me.GetDT(String.Format("quoteId='{0}' and itemType='" & COMM.Fixer.eItemType.Parent & "'", quoteId), "")
        If dt.Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function
    Public Function isBtoOrder(ByVal quotationDetail As DataTable) As Integer
        Dim dt() As DataRow = quotationDetail.Select(String.Format("itemType='" & COMM.Fixer.eItemType.Parent & "'"))
        If dt.Length > 0 Then
            Return 1
        End If
        Return 0
    End Function
  
    Public Function getMaxDueDate(ByVal quoteId As String) As Date
        Dim o As Object = tbOPBase.dbExecuteScalar(Me.conn, String.Format("select max(dueDate) from {0} where  quoteId='{1}'", Me.tb, quoteId))
        If IsDate(o) Then
            Return CDate(o)
        End If
        Return Now
    End Function
    Public Function getMaxReqDate(ByVal quoteId As String) As Date
        Dim o As Object = tbOPBase.dbExecuteScalar(Me.conn, String.Format("select max(reqdate) from {0} where quoteId='{1}'", Me.tb, quoteId))
        If IsDate(o) Then
            Return CDate(o)
        End If
        Return Now
    End Function
    Public Function getTotalAmount(ByVal quoteId As String) As Decimal
        Dim o As Object = tbOPBase.dbExecuteScalar(Me.conn, String.Format("select sum(qty * newunitprice) from {0} where quoteId='{1}' and itemType<>'" & COMM.Fixer.eItemType.Parent & "'", Me.tb, quoteId))
        If IsNumeric(o) Then
            Return CDec(o)
        End If
        Return 0
    End Function
    Public Function getTotalPrice(ByVal quoteId As String) As Decimal
        Dim o As Object = tbOPBase.dbExecuteScalar(Me.conn, String.Format("select sum(newunitprice) from {0} where quoteId='{1}' and itemType<>'" & COMM.Fixer.eItemType.Parent & "'", Me.tb, quoteId))
        If IsNumeric(o) Then
            Return CDec(o)
        End If
        Return 0
    End Function
    Public Function getTotalListPrice(ByVal quoteId As String) As Decimal
        Dim o As Object = tbOPBase.dbExecuteScalar(Me.conn, String.Format("select sum(listprice) from {0} where quoteId='{1}' and itemType<>'" & COMM.Fixer.eItemType.Parent & "'", Me.tb, quoteId))
        If IsNumeric(o) Then
            Return CDec(o)
        End If
        Return 0
    End Function
    Public Function getTotalListAmount(ByVal quoteId As String) As Decimal
        Dim o As Object = tbOPBase.dbExecuteScalar(Me.conn, String.Format("select sum(listprice*qty) from {0} where quoteId='{1}' and itemType<>'" & COMM.Fixer.eItemType.Parent & "'", Me.tb, quoteId))
        If IsNumeric(o) Then
            Return CDec(o)
        End If
        Return 0
    End Function
    Public Function getTotalAmount_PTD(ByVal quoteId As String) As Decimal
        Dim o As Object = tbOPBase.dbExecuteScalar(Me.conn, String.Format("select sum(qty * newunitprice) from {0} where quoteId='{1}' and itemType<>'" & COMM.Fixer.eItemType.Parent & "' and partNo like '96%'", Me.tb, quoteId))
        If IsNumeric(o) Then
            Return CDec(o)
        End If
        Return 0
    End Function
    Public Function getTotalAmount_AGS(ByVal quoteId As String) As Decimal
        Dim o As Object = tbOPBase.dbExecuteScalar(Me.conn, String.Format("select sum(qty * newunitprice) from {0} where quoteId='{1}' and itemType<>'" & COMM.Fixer.eItemType.Parent & "' and (PARTNO LIKE 'CTOS-%' or partno like 'C-CU-%' or PARTNO LIKE 'AGS-%' or PARTNO LIKE 'IMG-%')", Me.tb, quoteId))
        If IsNumeric(o) Then
            Return CDec(o)
        End If
        Return 0
    End Function

    Public Function getTotalITP(ByVal quoteId As String) As Decimal
        Dim o As Object = tbOPBase.dbExecuteScalar(Me.conn, String.Format("select sum(qty * newItp) from {0} where quoteId='{1}' and itemType<>'" & COMM.Fixer.eItemType.Parent & "'", Me.tb, quoteId))
        If IsNumeric(o) Then
            Return CDec(o)
        End If
        Return 0
    End Function
    Public Function getTotalITP_PTD(ByVal quoteId As String) As Decimal
        Dim o As Object = tbOPBase.dbExecuteScalar(Me.conn, String.Format("select sum(qty * newItp) from {0} where quoteId='{1}' and itemType<>'" & COMM.Fixer.eItemType.Parent & "' and partNo like '96%'", Me.tb, quoteId))
        If IsNumeric(o) Then
            Return CDec(o)
        End If
        Return 0
    End Function
    Public Function getTotalITP_AGS(ByVal quoteId As String) As Decimal
        Dim o As Object = tbOPBase.dbExecuteScalar(Me.conn, String.Format("select sum(qty * newItp) from {0} where quoteId='{1}' and itemType<>'" & COMM.Fixer.eItemType.Parent & "' and (PARTNO LIKE 'CTOS-%' or partno like 'C-CU-%' or PARTNO LIKE 'AGS-%' or PARTNO LIKE 'IMG-%')", Me.tb, quoteId))
        If IsNumeric(o) Then
            Return CDec(o)
        End If
        Return 0
    End Function

  
    'Public Function getTotalPrice_EW(ByVal quoteId As String) As Decimal
    '    Dim DT As DataTable = Me.GetDT(String.Format("quoteId='{0}' and ewFlag>0", quoteId), "")
    '    'If DT.Rows.Count > 0 Then
    '    '    Dim am As Decimal = 0
    '    '    For Each r As DataRow In DT.Rows
    '    '        Dim price As Decimal = r.Item("newunitPrice")
    '    '        Dim month As Integer = r.Item("ewFlag")
    '    '        am += price * Business.getRateByEWItem(Business.getEWItemByMonth(month), r.Item("deliveryPlant"))
    '    '    Next
    '    '    Return am
    '    'End If
    '    Return Business.getTotalPrice_EW(DT)
    'End Function
    Public Function reSetLineNoAfterDel(ByVal quoteId As String, ByVal line_no As Integer) As Integer
        Me.Update(String.Format("quoteId='{0}' and line_no>'{1}'", quoteId, line_no), String.Format("line_no=line_no-1"))
        Return 1
    End Function
    Public Function reSetLineNoBeforeInsert(ByVal quoteId As String, ByVal line_no As Integer) As Integer
        Me.Update(String.Format("quoteId='{0}' and line_no>='{1}'", quoteId, line_no), String.Format("line_no=line_no+1"))
        Return 1
    End Function
    Public Function isItemWithEW(ByVal quoteId As String, ByVal line_no As Integer) As Integer
        Return Me.IsExists(String.Format("quoteId='{0}' and line_no='{1}' and ewFlag>0", quoteId, line_no))
    End Function
    Public Function exChangeLineNo(ByVal quoteId As String, ByVal line_no1 As Integer, ByVal line_no2 As Integer) As Integer
        Return 1
    End Function
    Public Function getMaxLineCount(ByVal quoteId As String) As Integer
        Dim o As Object = tbOPBase.dbExecuteScalar(Me.conn, String.Format("select max(line_No) from {0} where quoteId='{1}'", Me.tb, quoteId))
        If IsNumeric(o) Then
            Return CInt(o)
        End If
        Return 0
    End Function
    Public Function isBtoParentItem(ByVal quoteId As String, ByVal line_no As Integer) As Integer
        If line_no < 100 Then
            Return 0
        End If
        If line_no Mod 100 = 0 Then
            Return 1
        End If
        Dim dt As DataTable = Me.GetDT(String.Format("quoteId='{0}' and line_no='{1}' and itemType='" & COMM.Fixer.eItemType.Parent & "'", quoteId, line_no), "")
        If dt.Rows.Count = 1 Then
            Return 1
        End If
        Return 0
    End Function
    Public Function isBtoParentItemV2(ByVal line_no As Integer) As Boolean
        If line_no Mod 100 = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function isBtoChildItem(ByVal quoteId As String, ByVal line_no As Integer) As Integer
        If line_no > 100 AndAlso line_no Mod 100 > 0 Then
            Return 1
        End If
        Dim dt As DataTable = Me.GetDT(String.Format("quoteId='{0}' and line_no='{1}' and itemType='" & COMM.Fixer.eItemType.Parent & "'", quoteId, line_no), "")
        If dt.Rows.Count = 1 Then
            Return 1
        End If
        Return 0
    End Function
    Public Function isStandItem(ByVal quoteId As String, ByVal line_no As Integer) As Integer
        Dim dt As DataTable = Me.GetDT(String.Format("quoteId='{0}' and line_no='{1}' and itemType='0'", quoteId, line_no), "")
        If dt.Rows.Count = 1 Then
            Return 1
        End If
        Return 0
    End Function

End Class
'Public Class SAP_Company : Inherits tbBase
'    Sub New(ByVal conn As String, ByVal tb As String)
'        Me.conn = conn
'        Me.tb = tb
'    End Sub
'End Class
Public Class SAPProduct : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
End Class

Public Class SAPProduct_ABC : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub

End Class

Public Class Cust_Material : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub

End Class

Public Class SIEBEL_PAYTERMS : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub

End Class
Public Class SIEBEL_SHIPTERMS : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub

End Class

'Frank 2012/07/16 replaced by EQDS.xsd
'Public Class optyQuote : Inherits tbBase
'    Sub New(ByVal conn As String, ByVal tb As String)
'        Me.conn = conn
'        Me.tb = tb
'    End Sub
'    Public Overloads Function Add(ByVal optyId As String, ByVal optyName As String, ByVal quoteId As String, ByVal optyStage As String) As Integer
'        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',N'{3}',N'{4}')", _
'                                          Me.tb, _
'                                          optyId, _
'                                          optyName, _
'                                          quoteId, _
'                                          optyStage)

'        tbOPBase.dbExecuteNoQuery(Me.conn, str)
'        Return 1
'    End Function
'End Class

Public Class quoteCart : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
    Public Overloads Function Add(ByVal quoteId As String, ByVal cartId As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}')", _
                                          Me.tb, _
                                          quoteId, _
                                          cartId)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class
Public Class BigText : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
    Public Overloads Function Add(ByVal quoteId As String, ByVal content As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}')", _
                                          Me.tb, _
                                          quoteId, _
                                          content)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class
Public Class quoteSiebelQuote : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
    Public Overloads Function Add(ByVal quoteId As String, ByVal SiebelQuoteId As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}')", _
                                          Me.tb, _
                                          quoteId, _
                                          SiebelQuoteId)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class
Public Class userLog : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
    Public Overloads Function Add(ByVal UID As String, ByVal userId As String, ByVal URL As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',N'{3}',getDate())", _
                                          Me.tb, _
                                          UID, _
                                          userId, _
                                          URL)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class
Public Class BankInfo : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
    Public Overloads Function Add(ByVal RBU As String, ByVal INFO As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}')", _
                                          Me.tb, _
                                          RBU, _
                                          INFO)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class
Public Class SALESEMAIL123 : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
    Public Overloads Function Add(ByVal QUOTEID As String, ByVal SEQ As String, ByVal SALESEMAIL As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',N'{3}')", _
                                          Me.tb, _
                                          QUOTEID, _
                                          SEQ, _
                                          SALESEMAIL)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class
Public Class EQPARTNER : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
    Public Overloads Function Add(ByVal QUOTEID As String, ByVal ROWID As String, ByVal ERPID As String, _
                                  ByVal NAME As String, ByVal ADDRESS As String, ByVal TYPE As String) As Integer
        Dim apt As New EQDSTableAdapters.EQPARTNERTableAdapter
        apt.InsertPartner(QUOTEID, ROWID, ERPID, NAME, ADDRESS, TYPE, "", "", "", "", "", "", "", "", "", "", "")
        Return 1
    End Function
End Class
Public Class loginLog : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
    Public Overloads Function Add(ByVal tempID As String, ByVal userId As String, ByVal PW As String, ByVal IPADDR As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',getDate(),N'{3}',N'{4}')", _
                                          Me.tb, _
                                          tempID, _
                                          userId, _
                                          PW, _
                                          IPADDR)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class
Public Class rbuAddress : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
End Class
Public Class PRODUCT_ITP : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
End Class
Public Class ITP_first : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
    Public Overloads Function Add(ByVal Org As String, ByVal PartNo As String, ByVal ITP As String, ByVal CURR As String, ByVal CompanyId As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}','{3}',N'{4}',N'{5}')", _
                                          Me.tb, _
                                          Org, _
                                          PartNo, _
                                          ITP, _
                                          CURR, _
                                          CompanyId)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class
Public Class ManagerSalesMap : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
    Public Overloads Function Add(ByVal Manager As String, ByVal Sales As String, ByVal User As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',getdate(),N'{3}')", _
                                          Me.tb, _
                                          Manager, _
                                          Sales, _
                                          User)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class
Public Class quotation_approval : Inherits tbBase
    Sub New(ByVal conn As String, ByVal tb As String)
        Me.conn = conn
        Me.tb = tb
    End Sub
    Public Overloads Function Add(ByVal quoteId As String, ByVal approver As String, ByVal gpLevel As Decimal, _
                                  ByVal approveLevel As Integer, ByVal _status As Integer, _
                                  ByVal createdDate As DateTime, ByVal MapproveYes As String, _
                                  ByVal MapproveNo As String, ByVal _type As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',N'{3}',N'{4}',N'{5}','{6}',N'{7}',N'{8}',N'{9}')", _
                                          Me.tb, _
                                          quoteId, _
                                          approver, _
                                          gpLevel, _
                                          approveLevel, _
                                          _status, _
                                          createdDate, _
                                          MapproveYes, _
                                          MapproveNo, _
                                          _type)
        tbOPBase.dbExecuteNoQuery(Me.conn, str)
        Return 1
    End Function
End Class

Public Enum LineType
    StandardItem = 0
    BtoParetItem = -1
    ChildItem = 1
End Enum

Public Structure struct_GP_Detail
    Dim lineNo As Integer
    Dim PartNo As String
    Dim Price As Decimal
    Dim Itp As Decimal
    Dim QTY As Integer
End Structure

Public Structure struct_Quote_Detail
    Dim quoteId As String
    Dim lineNo As Integer
    Dim partNo As String
    Dim description As String
    Dim qty As Integer
    Dim listPrice As Decimal
    Dim unitPrice As Decimal
    Dim newUnitPrice As Decimal
    Dim itp As Decimal
    Dim newItp As Decimal
    Dim deliveryPlant As String
    Dim category As String
    Dim classABC As String
    Dim rohs As Integer
    Dim ewFlag As Integer
    Dim reqDate As Date
    Dim dueDate As Date
    Dim satisfyFlag As Integer
    Dim canBeConfirmed As Integer
    Dim custMaterial As String
    Dim inventory As Integer
    Dim oType As Integer
    Dim modelNo As String
    Dim SPRNO As String
End Structure