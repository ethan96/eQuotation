Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Configuration
Public Class quotationMaster : Inherits tbBase
    Sub New(ByRef oType As COMM.Fixer.eDocType)
        MyBase.New("EQ", "quotationMaster")
        If oType = COMM.Fixer.eDocType.ORDER Then
            Me.tbSource.tbName = "OrderMaster"
        End If
    End Sub
    Public Overloads Function Add(ByVal quoteId As String, ByVal customId As String, ByVal quoteToRowId As String, ByVal quoteToErpId As String, ByVal quoteToName As String, _
                                    ByVal office As String, ByVal currency As String, ByVal salesEmail As String, ByVal salesRowId As String, ByVal directPhone As String, _
                                    ByVal attentionRowId As String, ByVal attentionEmail As String, ByVal bankInfo As String, ByVal quoteDate As Date, ByVal deliveryDate As Date, _
                                    ByVal expiredDate As Date, ByVal shipTerm As String, ByVal paymentTerm As String, ByVal freight As Decimal, ByVal insurance As Decimal, _
                                    ByVal specialCharge As Decimal, ByVal tax As Decimal, ByVal quoteNote As String, ByVal relatedInfo As String, ByVal createdBy As String, _
                                    ByVal createdDate As Date, ByVal preparedBy As String, ByVal qStatus As String, ByVal isShowListPrice As Integer, ByVal isShowDiscount As Integer, _
                                    ByVal isShowDueDate As Integer, ByVal isLumpSumOnly As Integer, ByVal isRepeatedOrder As Integer, ByVal ogroup As String, ByVal delDateFlag As Integer, _
                                    ByVal org As String, ByVal siebelRBU As String, ByVal DIST_CHAN As String, ByVal DIVISION As String, ByVal SALESGROUP As String, _
                                    ByVal SALESOFFICE As String, ByVal PO_NO As String, ByVal CARE_ON As String, ByVal isExempt As Integer, ByVal Inco1 As String, _
                                    ByVal Inco2 As String, ByVal SalesDistrict As String, ByVal PrintF As COMM.Fixer.USPrintOutFormat, ByVal OriginalQuoteID As String, ByVal DocReg As COMM.Fixer.eDocReg, _
                                    ByVal DocType As COMM.Fixer.eDocType, ByVal ReqDate As Date, ByVal PartialF As Integer, ByVal IS_EARLYSHIP As Integer, ByVal DocRealType As String, _
                                    ByVal LastUpdated As Date, ByVal LastUpdatedBy As String, ByVal PODate As Date, ByVal KEYPERSON As String, ByVal EMPLOYEEID As String, _
                                    ByVal ISVIRPARTONLY As COMM.Fixer.eIsVirNoOnly, ByVal TAXDEPCITY As String, ByVal TAXDSTCITY As String, ByVal TRIANGULARINDICATOR As String, ByVal TAXCLASS As String, _
                                    ByVal SHIPCUSTPONO As String, ByVal quoteNo As String, ByVal Revision_Number As Integer, ByVal Active As Boolean) As Integer
        Dim str As String = String.Format("insert into {0} " & _
                                        "(quoteId,customId,quoteToRowId,quoteToErpId,quoteToName, " & _
                                        " office,currency,salesEmail,salesRowId,directPhone, " & _
                                        " attentionRowId,attentionEmail,bankInfo,quoteDate,deliveryDate, " & _
                                        " expiredDate,shipTerm,paymentTerm,freight,insurance, " & _
                                        " specialCharge,tax,quoteNote,relatedInfo,createdBy, " & _
                                        " createdDate,preparedBy,DOCSTATUS,isShowListPrice,isShowDiscount, " & _
                                        " isShowDueDate,isLumpSumOnly,isRepeatedOrder,ogroup,DelDateFlag," & _
                                        " org,siebelRBU,DIST_CHAN,DIVISION,SALESGROUP, " & _
                                        " SALESOFFICE,PO_NO,CARE_ON,isExempt,INCO1," & _
                                        " INCO2,DISTRICT,PRINTOUT_FORMAT,OriginalQuoteID,DocReg, " & _
                                        " DocType,ReqDate,Partial,IS_EARLYSHIP,DocRealType, " & _
                                        " LastUpdatedDate,LastUpdatedBy,PODate,KEYPERSON,EMPLOYEEID, " & _
                                        " isvirpartOnly,TAXDEPCITY ,TAXDSTCITY ,TRIANGULARINDICATOR ,TAXCLASS , " & _
                                        " SHIPCUSTPONO ,quoteNo ,Revision_Number ,Active ) " & _
                                        " Values " & _
                                        " (@quoteId,@customId,@quoteToRowId,@quoteToErpId,@quoteToName,@office, " & _
                                        " @currency,@salesEmail,@salesRowId,@directPhone, " & _
                                        " @attentionRowId,@attentionEmail,@bankInfo,@quoteDate,@deliveryDate, " & _
                                        " @expiredDate,@shipTerm,@paymentTerm,@freight,@insurance, " & _
                                        " @specialCharge,@tax,@quoteNote,@relatedInfo,@createdBy, " & _
                                        " @createdDate,@preparedBy,@DOCSTATUS,@isShowListPrice,@isShowDiscount, " & _
                                        " @isShowDueDate,@isLumpSumOnly,@isRepeatedOrder,@ogroup,@DelDateFlag, " & _
                                        " @org,@siebelRBU,@DIST_CHAN,@DIVISION,@SALESGROUP, " & _
                                        " @SALESOFFICE,@PO_NO,@CARE_ON,@isExempt,@INCO1, " & _
                                        " @INCO2,@DISTRICT,@PRINTOUT_FORMAT,@OriginalQuoteID,@DocReg, " & _
                                        " @DocType,@ReqDate,@Partial,@IS_EARLYSHIP,@DocRealType, " & _
                                        " @LastUpdatedDate,@LastUpdatedBy,@PODate,@KEYPERSON,@EMPLOYEEID, " & _
                                        " @isvirpartOnly,@TAXDEPCITY ,@TAXDSTCITY ,@TRIANGULARINDICATOR ,@TAXCLASS , " & _
                                        " @SHIPCUSTPONO ,@quoteNo ,@Revision_Number ,@Active ) " _
                                        , _
                                        Me.tbSource.tbName)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@quoteId", quoteId),
                                            New SqlClient.SqlParameter("@customId", customId),
                                            New SqlClient.SqlParameter("@quoteToRowId", quoteToRowId),
                                            New SqlClient.SqlParameter("@quoteToErpId", quoteToErpId),
                                            New SqlClient.SqlParameter("@quoteToName", quoteToName),
                                            New SqlClient.SqlParameter("@office", office),
                                            New SqlClient.SqlParameter("@currency", currency),
                                            New SqlClient.SqlParameter("@salesEmail", salesEmail),
                                            New SqlClient.SqlParameter("@salesRowId", salesRowId),
                                            New SqlClient.SqlParameter("@directPhone", directPhone),
                                            New SqlClient.SqlParameter("@attentionRowId", attentionRowId),
                                            New SqlClient.SqlParameter("@attentionEmail", attentionEmail),
                                            New SqlClient.SqlParameter("@bankInfo", bankInfo),
                                            New SqlClient.SqlParameter("@quoteDate", quoteDate),
                                            New SqlClient.SqlParameter("@deliveryDate", deliveryDate),
                                            New SqlClient.SqlParameter("@expiredDate", expiredDate),
                                            New SqlClient.SqlParameter("@shipTerm", shipTerm),
                                            New SqlClient.SqlParameter("@paymentTerm", paymentTerm),
                                            New SqlClient.SqlParameter("@freight", freight),
                                            New SqlClient.SqlParameter("@insurance", insurance),
                                            New SqlClient.SqlParameter("@specialCharge", specialCharge),
                                            New SqlClient.SqlParameter("@tax", tax),
                                            New SqlClient.SqlParameter("@quoteNote", quoteNote),
                                            New SqlClient.SqlParameter("@relatedInfo", relatedInfo),
                                            New SqlClient.SqlParameter("@createdBy", createdBy),
                                            New SqlClient.SqlParameter("@createdDate", createdDate),
                                            New SqlClient.SqlParameter("@preparedBy", preparedBy),
                                            New SqlClient.SqlParameter("@DOCSTATUS", qStatus),
                                            New SqlClient.SqlParameter("@isShowListPrice", isShowListPrice),
                                            New SqlClient.SqlParameter("@isShowDiscount", isShowDiscount),
                                            New SqlClient.SqlParameter("@isShowDueDate", isShowDueDate),
                                            New SqlClient.SqlParameter("@isLumpSumOnly", isLumpSumOnly),
                                            New SqlClient.SqlParameter("@isRepeatedOrder", isRepeatedOrder),
                                            New SqlClient.SqlParameter("@ogroup", ogroup),
                                            New SqlClient.SqlParameter("@DelDateFlag", delDateFlag),
                                            New SqlClient.SqlParameter("@org", org),
                                            New SqlClient.SqlParameter("@siebelRBU", siebelRBU),
                                            New SqlClient.SqlParameter("@DIST_CHAN", DIST_CHAN),
                                            New SqlClient.SqlParameter("@DIVISION", DIVISION),
                                            New SqlClient.SqlParameter("@SALESGROUP", SALESGROUP),
                                            New SqlClient.SqlParameter("@SALESOFFICE", SALESOFFICE),
                                            New SqlClient.SqlParameter("@DISTRICT", SalesDistrict),
                                            New SqlClient.SqlParameter("@PO_NO", PO_NO),
                                            New SqlClient.SqlParameter("@CARE_ON", CARE_ON),
                                            New SqlClient.SqlParameter("@isExempt", isExempt),
                                            New SqlClient.SqlParameter("@INCO1", Inco1),
                                            New SqlClient.SqlParameter("@INCO2", Inco2),
                                            New SqlClient.SqlParameter("@PRINTOUT_FORMAT", PrintF),
                                            New SqlClient.SqlParameter("@OriginalQuoteID", OriginalQuoteID),
                                            New SqlClient.SqlParameter("@DocReg", DocReg),
                                            New SqlClient.SqlParameter("@DocType", DocType),
                                            New SqlClient.SqlParameter("@ReqDate", ReqDate),
                                            New SqlClient.SqlParameter("@Partial", PartialF),
                                            New SqlClient.SqlParameter("@IS_EARLYSHIP", IS_EARLYSHIP),
                                            New SqlClient.SqlParameter("@DocRealType", DocRealType),
                                            New SqlClient.SqlParameter("@LastUpdatedDate", LastUpdated),
                                            New SqlClient.SqlParameter("@LastUpdatedBy", LastUpdatedBy),
                                            New SqlClient.SqlParameter("@PODate", PODate),
                                            New SqlClient.SqlParameter("@KEYPERSON", KEYPERSON),
                                            New SqlClient.SqlParameter("@EMPLOYEEID", EMPLOYEEID),
                                            New SqlClient.SqlParameter("@isvirpartOnly", ISVIRPARTONLY),
                                            New SqlClient.SqlParameter("@TAXDEPCITY", TAXDEPCITY),
                                            New SqlClient.SqlParameter("@TAXDSTCITY", TAXDSTCITY),
                                            New SqlClient.SqlParameter("@TRIANGULARINDICATOR", TRIANGULARINDICATOR),
                                            New SqlClient.SqlParameter("@TAXCLASS", TAXCLASS),
                                            New SqlClient.SqlParameter("@SHIPCUSTPONO", SHIPCUSTPONO),
                                            New SqlClient.SqlParameter("@quoteNo", quoteNo),
                                            New SqlClient.SqlParameter("@Revision_Number", Revision_Number),
                                            New SqlClient.SqlParameter("@Active", Active)}
        sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
        Return 1
    End Function

    Public Function SetAllQuoteInactiveByQuoteNo(ByVal QuoteNo As String)
        Dim str As String = String.Format("Update {0} set active=0 where quoteno=@quoteNo", Me.tbSource.tbName)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@quoteNo", QuoteNo)}
        Return sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
    End Function


    ''' <summary>
    ''' Set a quote to active status
    ''' </summary>
    ''' <param name="quoteNo"></param>
    ''' <param name="revision_number"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function SetQuoteActive(ByVal quoteNo As String, ByVal revision_number As Integer)
        'Frank 2013/07/02
        Dim str As String = String.Format("Update {0} set active=1 where quoteno=@quoteNo and revision_number=@revision_number", Me.tbSource.tbName)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@quoteNo", quoteNo),
                                            New SqlClient.SqlParameter("@Revision_Number", revision_number)}
        Return sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
    End Function

    'Nada Added for get revision list
    Public Function getRevisionAr(ByVal QuoteNo As String) As ArrayList
        Dim str As String = String.Format("Select distinct revision_number from {0} where QuoteNo=@quoteNo", Me.tbSource.tbName)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@quoteNo", QuoteNo)}
        Dim r As SqlClient.SqlDataReader = sqlhelper.ExecuteReader(Me.tbSource.conn, CommandType.Text, str, p)
        Dim Ar As New ArrayList
        While r.Read
            Ar.Add(r.Item("revision_number"))
        End While
        r.Close()
        If Ar.Count > 0 Then
            Return Ar
        End If
        Ar = Nothing
        Return Ar
    End Function
    Public Function getActiveRevisionNo(ByVal QuoteNo As String) As String
        Dim str As String = String.Format("select top 1 revision_number from {0} where QuoteNo=@quoteNo and Active=1", Me.tbSource.tbName)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@quoteNo", QuoteNo)}
        Dim o As Object = sqlhelper.ExecuteScalar(Me.tbSource.conn, CommandType.Text, str, p)
        If Not IsNothing(o) AndAlso o.ToString <> "" Then
            Return o
        End If
        Return ""
    End Function
End Class



Public Class QuotationExtension : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "QuotationExtension")
    End Sub
    Public Overloads Function Add(ByVal QuoteID As String, ByVal EmailGreeting As String, ByVal SpecialTandC As String _
                                  , ByVal SignatureRowID As String, ByVal Warranty As String, ByVal LastUpdatedBy As String) As Integer

        Dim delstr As String = String.Format("delete from  {0} where QuoteID='{1}'", Me.tbSource.tbName, QuoteID)

        Dim str As String = String.Format("insert into {0} (QuoteID,EmailGreeting,SpecialTandC,SignatureRowID,Warranty,LastUpdatedBy,LastUpdated)" & _
                                          "values (@QuoteID,@EmailGreeting,@SpecialTandC,@SignatureRowID,@Warranty,@LastUpdatedBy,getdate())", _
                                        Me.tbSource.tbName, _
                                          QuoteID, _
                                          EmailGreeting, _
                                          SpecialTandC, _
                                          SignatureRowID, _
                                          LastUpdatedBy)

        Dim paras() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@QuoteID", QuoteID),
                                               New SqlClient.SqlParameter("@EmailGreeting", EmailGreeting),
                                               New SqlClient.SqlParameter("@SpecialTandC", SpecialTandC),
                                               New SqlClient.SqlParameter("@SignatureRowID", SignatureRowID),
                                            New SqlClient.SqlParameter("@Warranty", Warranty),
                                               New SqlClient.SqlParameter("@LastUpdatedBy", LastUpdatedBy)}

        Return sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, delstr & ";" & str, paras)

    End Function

End Class

Public Class quotationDetail : Inherits tbBase
    'Sub New()
    '    MyBase.New("EQ", "quotationDetail")
    'End Sub
    Sub New(ByRef oType As COMM.Fixer.eDocType)
        MyBase.New("EQ", "quotationDetail")
        If oType = COMM.Fixer.eDocType.ORDER Then
            Me.tbSource.tbName = "OrderDetail"
        End If
    End Sub
End Class


Public Class SAP_DIMCompany : Inherits tbBase
    Sub New()
        MyBase.New("MY", "SAP_DIMCompany")
    End Sub
End Class
Public Class SAP_Product : Inherits tbBase
    Sub New()
        MyBase.New("MY", "SAP_Product")
    End Sub
End Class

Public Class SAP_Product_ABC : Inherits tbBase
    Sub New()
        MyBase.New("MY", "SAP_Product_ABC")
    End Sub

End Class

Public Class SIEBEL_PAYTERMS : Inherits tbBase
    Sub New()
        MyBase.New("MY", "SIEBEL_PAYTERMS")
    End Sub


End Class
Public Class SIEBEL_SHIPTERMS : Inherits tbBase
      Sub New()
        MyBase.New("MY", "SIEBEL_SHIPTERMS")
    End Sub

End Class

'Frank 2012/07/16 replaced by EQDS.xsd
'Public Class optyQuote : Inherits tbBase
'    Sub New(ByVal conn As String, ByVal tb As String)
'        Me.tbSource.conn = conn
'        Me.tbSource.tbName = tb
'    End Sub
'    Public Overloads Function Add(ByVal optyId As String, ByVal optyName As String, ByVal quoteId As String, ByVal optyStage As String) As Integer
'        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',N'{3}',N'{4}')", _
'                                          Me.tbSource.tbName, _
'                                          optyId, _
'                                          optyName, _
'                                          quoteId, _
'                                          optyStage)

'        dbUtil.dbExecuteNoQuery(Me.tbSource.conn, str)
'        Return 1
'    End Function
'End Class


Public Class BigText : Inherits tbBase
       Sub New()
        MyBase.New("EQ", "BigText")
    End Sub
    Public Overloads Function Add(ByVal quoteId As String, ByVal content As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}')", _
                                          Me.tbSource.tbName, _
                                          quoteId, _
                                          content)
        dbUtil.dbExecuteNoQuery(Me.tbSource.conn, str)
        Return 1
    End Function
End Class
Public Class quoteSiebelQuote : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "quoteSiebelQuote")
    End Sub
    Public Overloads Function Add(ByVal quoteId As String, ByVal SiebelQuoteId As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}')", _
                                          Me.tbSource.tbName, _
                                          quoteId, _
                                          SiebelQuoteId)
        dbUtil.dbExecuteNoQuery(Me.tbSource.conn, str)
        Return 1
    End Function
End Class
Public Class userLog : Inherits tbBase
   Sub New()
        MyBase.New("EQ", "userLog")
    End Sub
    Public Overloads Function Add(ByVal UID As String, ByVal userId As String, ByVal URL As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',N'{3}',getDate())", _
                                          Me.tbSource.tbName, _
                                          UID, _
                                          userId, _
                                          URL)
        dbUtil.dbExecuteNoQuery(Me.tbSource.conn, str)
        Return 1
    End Function
End Class
Public Class BankInfo : Inherits tbBase
     Sub New()
        MyBase.New("EQ", "BankInfo")
    End Sub
    Public Overloads Function Add(ByVal RBU As String, ByVal INFO As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}')", _
                                          Me.tbSource.tbName, _
                                          RBU, _
                                          INFO)
        dbUtil.dbExecuteNoQuery(Me.tbSource.conn, str)
        Return 1
    End Function
End Class
Public Class SALESEMAIL123 : Inherits tbBase
     Sub New()
        MyBase.New("EQ", "SALESEMAIL123")
    End Sub
    Public Overloads Function Add(ByVal QUOTEID As String, ByVal SEQ As String, ByVal SALESEMAIL As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',N'{3}')", _
                                          Me.tbSource.tbName, _
                                          QUOTEID, _
                                          SEQ, _
                                          SALESEMAIL)
        dbUtil.dbExecuteNoQuery(Me.tbSource.conn, str)
        Return 1
    End Function
End Class
Public Class EQPARTNER : Inherits tbBase
    Sub New(ByRef oType As COMM.Fixer.eDocType)
        MyBase.New("EQ", "EQPARTNER")
        If oType = COMM.Fixer.eDocType.ORDER Then
            Me.tbSource.tbName = "OrderPARTNER"
        End If
    End Sub

    Public Overloads Function Add(ByVal QUOTEID As String, _
                                  ByVal ROWID As String, _
                                  ByVal ERPID As String, _
                                  ByVal NAME As String, _
                                  ByVal ADDRESS As String, _
                                  ByVal TYPE As String, _
                                  ByVal ATTENTION As String, _
                                  ByVal TEL As String, _
                                  ByVal MOBILE As String, _
                                  ByVal ZIPCODE As String, _
                                  ByVal COUNTRY As String, _
                                  ByVal CITY As String, _
                                  ByVal STREET As String, _
                                  ByVal STATE As String, _
                                  ByVal DISTRICT As String, _
                                  ByVal STREET2 As String) As Integer
        Dim str As String = String.Format("insert into {0} (QUOTEID,ROWID,ERPID,NAME,ADDRESS,TYPE,ATTENTION,TEL,MOBILE,ZIPCODE,COUNTRY,CITY,STREET,STATE,DISTRICT,STREET2) " & _
                                        " values (@QUOTEID,@ROWID,@ERPID,@NAME,@ADDRESS,@TYPE,@ATTENTION,@TEL,@MOBILE,@ZIPCODE,@COUNTRY,@CITY,@STREET,@STATE,@DISTRICT,@STREET2) " _
                                        , _
                                        Me.tbSource.tbName)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@quoteId", QUOTEID),
                                            New SqlClient.SqlParameter("@ROWID", ROWID),
                                            New SqlClient.SqlParameter("@ERPID", ERPID),
                                            New SqlClient.SqlParameter("@NAME", NAME),
                                            New SqlClient.SqlParameter("@ADDRESS", ADDRESS),
                                            New SqlClient.SqlParameter("@TYPE", TYPE),
                                            New SqlClient.SqlParameter("@ATTENTION", ATTENTION),
                                            New SqlClient.SqlParameter("@TEL", TEL),
                                            New SqlClient.SqlParameter("@MOBILE", ""),
                                            New SqlClient.SqlParameter("@ZIPCODE", ZIPCODE),
                                            New SqlClient.SqlParameter("@COUNTRY", COUNTRY),
                                            New SqlClient.SqlParameter("@CITY", CITY),
                                            New SqlClient.SqlParameter("@STREET", STREET),
                                            New SqlClient.SqlParameter("@STATE", STATE),
                                            New SqlClient.SqlParameter("@DISTRICT", DISTRICT),
                                            New SqlClient.SqlParameter("@STREET2", STREET2)}
        sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)

        Return 1
    End Function
End Class
Public Class QuotationNote : Inherits tbBase
    Sub New(ByRef oType As COMM.Fixer.eDocType)
        MyBase.New("EQ", "QuotationNote")
        If oType = COMM.Fixer.eDocType.ORDER Then
            Me.tbSource.tbName = "OrderNote"
        End If
    End Sub
End Class
Public Class loginLog : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "loginLog")
    End Sub
    Public Overloads Function Add(ByVal tempID As String, ByVal userId As String, ByVal PW As String, ByVal IPADDR As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',getDate(),N'{3}',N'{4}')", _
                                          Me.tbSource.tbName, _
                                          tempID, _
                                          userId, _
                                          PW, _
                                          IPADDR)
        dbUtil.dbExecuteNoQuery(Me.tbSource.conn, str)
        Return 1
    End Function
End Class
Public Class rbuAddress : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "rbuAddress")
    End Sub
End Class
Public Class PRODUCT_ITP : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "PRODUCT_ITP")
    End Sub
End Class
Public Class Freight : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "Freight")
    End Sub
    Public Overloads Function Add(ByVal order_id As String, ByVal ftype As String, ByVal fvalue As String)
        Dim str As String = String.Format("insert into {0} (order_id,ftype,fvalue) values (@order_id,@ftype,@fvalue)", Me.tbSource.tbName)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@order_id", order_id),
                                             New SqlClient.SqlParameter("@ftype", ftype),
                                             New SqlClient.SqlParameter("@fvalue", fvalue)}

        sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
        Return 1
    End Function

End Class
Public Class doctext : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "doctext")
    End Sub
    Public Overloads Function Add(ByVal DocId As String, ByVal TXT As String, ByVal oType As String)
        Dim str As String = String.Format("insert into {0} (DocId,TXT,oType) values (@DocId,@TXT,@oType)", Me.tbSource.tbName)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@DocId", DocId),
                            New SqlClient.SqlParameter("@TXT", TXT),
                            New SqlClient.SqlParameter("@oType", oType)}
        sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
        Return 1
    End Function
End Class
Public Class ORDER_PROC_STATUS2 : Inherits tbBase
    Sub New()
        MyBase.New("MY", "ORDER_PROC_STATUS2")
    End Sub
    Public Overloads Function Add(ByVal ORDER_NO As String, ByVal LINE_SEQ As String, ByVal NUMBER As String, ByVal MESSAGE As String, ByVal CREATED_DATE As Date, _
       ByVal STATUS As Integer, ByVal TYPE As String)
        Dim str As String = String.Format("insert into {0} (ORDER_NO,LINE_SEQ,NUMBER,MESSAGE,CREATED_DATE) values (@ORDER_NO,@LINE_SEQ,@NUMBER,@MESSAGE,@CREATED_DATE)", Me.tbSource.tbName)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@ORDER_NO", ORDER_NO),
                            New SqlClient.SqlParameter("@LINE_SEQ", LINE_SEQ),
                            New SqlClient.SqlParameter("@NUMBER", NUMBER),
                                             New SqlClient.SqlParameter("@MESSAGE", MESSAGE),
                                             New SqlClient.SqlParameter("@CREATED_DATE", CREATED_DATE)}
        sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
        Return 1
    End Function
End Class
Public Class credit : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "credit")
    End Sub
    Public Overloads Function Add(ByVal OrderID As String, ByVal HOLDER As String, ByVal EXPIRED As Date, ByVal TYPE As String, ByVal NUMBER As String, ByVal VERIFICATION_VALUE As String)
        Dim str As String = String.Format("insert into {0} (OrderID,HOLDER,EXPIRED,TYPE,NUMBER,VERIFICATION_VALUE) values (@OrderID,@HOLDER,@EXPIRED,@TYPE,@NUMBER,@VERIFICATION_VALUE)", Me.tbSource.tbName)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@OrderID", OrderID),
                                                New SqlClient.SqlParameter("@HOLDER", HOLDER),
                                                New SqlClient.SqlParameter("@EXPIRED", EXPIRED),
                                                New SqlClient.SqlParameter("@TYPE", TYPE),
                                                New SqlClient.SqlParameter("@NUMBER", NUMBER),
                                                New SqlClient.SqlParameter("@VERIFICATION_VALUE", VERIFICATION_VALUE)}
        sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
        Return 1
    End Function
End Class
Public Class optyQuote : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "optyQuote")
    End Sub
    Public Overloads Function Add(ByVal optyId As String,
    ByVal optyName As String,
    ByVal quoteId As String,
    ByVal optyStage As String,
    ByVal Opty_Owner_Email As String)
        Dim str As String = String.Format("insert into {0} (optyId,optyName,quoteId,optyStage,Opty_Owner_Email) values (@optyId,@optyName,@quoteId,@optyStage,@Opty_Owner_Email)", Me.tbSource.tbName)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@optyId", optyId),
                                                New SqlClient.SqlParameter("@optyName", optyName),
                                                New SqlClient.SqlParameter("@quoteId", quoteId),
                                                New SqlClient.SqlParameter("@optyStage", optyStage),
                                                New SqlClient.SqlParameter("@Opty_Owner_Email", Opty_Owner_Email)}
        sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
        Return 1
    End Function

End Class
Public Class ITP_first : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "ITP_first")
    End Sub
    Public Overloads Function Add(ByVal Org As String, ByVal PartNo As String, ByVal ITP As String, ByVal CURR As String, ByVal CompanyId As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}','{3}',N'{4}',N'{5}')", _
                                          Me.tbSource.tbName, _
                                          Org, _
                                          PartNo, _
                                          ITP, _
                                          CURR, _
                                          CompanyId)
        dbUtil.dbExecuteNoQuery(Me.tbSource.conn, str)
        Return 1
    End Function
End Class
Public Class ManagerSalesMap : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "ManagerSalesMap")
    End Sub
    Public Overloads Function Add(ByVal Manager As String, ByVal Sales As String, ByVal User As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',getdate(),N'{3}')", _
                                          Me.tbSource.tbName, _
                                          Manager, _
                                          Sales, _
                                          User)
        dbUtil.dbExecuteNoQuery(Me.tbSource.conn, str)
        Return 1
    End Function
End Class
Public Class quotation_approval : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "quotation_approval")
    End Sub
    Public Overloads Function Add(ByVal quoteId As String, ByVal approver As String, ByVal gpLevel As Decimal, _
                                  ByVal approveLevel As Integer, ByVal _status As Integer, _
                                  ByVal createdDate As DateTime, ByVal MapproveYes As String, _
                                  ByVal MapproveNo As String, ByVal _type As String) As Integer
        Dim str As String = String.Format("insert into {0} values (N'{1}',N'{2}',N'{3}',N'{4}',N'{5}','{6}',N'{7}',N'{8}',N'{9}')", _
                                          Me.tbSource.tbName, _
                                          quoteId, _
                                          approver, _
                                          gpLevel, _
                                          approveLevel, _
                                          _status, _
                                          createdDate, _
                                          MapproveYes, _
                                          MapproveNo, _
                                          _type)
        dbUtil.dbExecuteNoQuery(Me.tbSource.conn, str)
        Return 1
    End Function
End Class

Public Class QUOTEPOFILEMAPPING : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "QUOTEPOFILEMAPPING")
    End Sub
    Public Overloads Function Add(ByVal QuoteId As String, ByVal DocData As Byte()) As Integer
        Dim str As String = String.Format("insert into {0} (QuoteId,PoDoc) values (@QuoteId,@DocData)", Me.tbSource.tbName)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@QuoteId", QuoteId),
                                                New SqlClient.SqlParameter("@DocData", DocData)}
        sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
        Return 1
    End Function
End Class


Public Class Signature : Inherits tbBase
    Sub New()
        MyBase.New("EQ", "Signature")
    End Sub
    Public Overloads Function Add(ByVal SID As String, ByVal UserID As String, ByVal SignatureData As Byte() _
                                  , ByVal Active As Boolean, ByVal FileName As String) As Integer

        If String.IsNullOrEmpty(SID) OrElse SID = "" Then
            SID = System.Guid.NewGuid().ToString().Replace("-", "")
        End If

        Dim str As String = String.Format("update  Signature set Active =0 where UserID ='{2}';insert into {0} (SID,UserID,SignatureData,Active,FileName,LastUpdated) values (N'{1}',N'{2}',@SignatureData,@Active,N'{3}',getdate())", _
                                          Me.tbSource.tbName, _
                                          SID, _
                                          UserID, _
                                          FileName)

        Dim paras() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@SignatureData", SignatureData),
                                               New SqlClient.SqlParameter("@Active", Active)}

        Return sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, paras)

    End Function
End Class


