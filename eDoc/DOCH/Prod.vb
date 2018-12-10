Public Class EWType : Implements iEWTypeLine


    Private _NameItem As String = ""
    Public Property NameItem As String Implements iEWTypeLine.NameItem
        Get
            Return _NameItem
        End Get
        Set(ByVal value As String)
            _NameItem = value
        End Set
    End Property
    Private _Month As Integer = 0
    Public Property Month As Integer Implements iEWTypeLine.Month
        Get
            Return _Month
        End Get
        Set(ByVal value As Integer)
            _Month = value
        End Set
    End Property
    Private _Type As Integer = 0
    Public Property Type As Integer Implements iEWTypeLine.Type
        Get
            Return _Type
        End Get
        Set(ByVal value As Integer)
            _Type = value
        End Set
    End Property
    Private _Rate As Decimal = 0
    Public Property Rate As Decimal Implements IBUS.iEWTypeLine.Rate
        Get
            Return _Rate
        End Get
        Set(ByVal value As Decimal)
            _Rate = value
        End Set
    End Property
End Class
Public Class EWUtil : Implements iEWUtil

    Public Function getRealMonthByMonth(ByVal Month As Integer, ByVal DocReg As COMM.Fixer.eDocReg) As Integer Implements iEWUtil.getRealMonthByMonth
        Select Case Month
            Case 999
                Return 3
            Case 99
                Return 36
        End Select
        Return Month
    End Function
    Public Function getMonthByEWItem(ByVal itemNo As String, ByVal DocReg As COMM.Fixer.eDocReg) As Integer Implements IBUS.iEWUtil.getMonthByEWItem
        Dim L As List(Of iEWTypeLine) = getListByReg(DocReg, 0)
        Dim A As iEWTypeLine = L.Where(Function(X) X.NameItem = itemNo).FirstOrDefault
        If Not IsNothing(A) Then
            Return A.Month
        End If
        Return 0
    End Function
    Public Function getRateByEWItem(ByVal itemNo As String, ByVal DocReg As COMM.Fixer.eDocReg) As Decimal Implements iEWUtil.getRateByEWItem
        Dim L As List(Of iEWTypeLine) = getListByReg(DocReg, 0)
        Dim A As iEWTypeLine = L.Where(Function(X) X.NameItem = itemNo).FirstOrDefault
        If Not IsNothing(A) Then
            Return A.Rate
        End If
        Return 0
    End Function
    Public Function getEWItemByMonth(ByVal month As Integer, ByVal DocReg As COMM.Fixer.eDocReg) As String Implements iEWUtil.getEWItemByMonth
        Dim L As List(Of iEWTypeLine) = getListByReg(DocReg, 0)
        Dim A As iEWTypeLine = L.Where(Function(X) X.Month = month).FirstOrDefault
        If Not IsNothing(A) Then
            Return A.NameItem
        End If
        Return ""
    End Function
    Public Function getListByReg(ByVal DocReg As COMM.Fixer.eDocReg, ByVal Type As Integer) As List(Of iEWTypeLine) Implements iEWUtil.getListByReg

        ' Notice: If any EW rate needs to be changed here, please also remember to update ExtendedWarrantyPartNo_V2 table!!!

        If (DocReg And Fixer.eDocReg.AEU) = DocReg Then
            Dim O As New List(Of iEWTypeLine)
            Dim ew As iEWTypeLine = New EWType
            ew.NameItem = "AGS-EW-03" : ew.Type = 0 : ew.Month = 3 : ew.Rate = 0.02
            O.Add(ew)
            Dim ew1 As iEWTypeLine = New EWType
            ew1.NameItem = "AGS-EW-06" : ew1.Type = 0 : ew1.Month = 6 : ew1.Rate = 0.035
            O.Add(ew1)
            Dim ew2 As iEWTypeLine = New EWType
            ew2.NameItem = "AGS-EW-09" : ew2.Type = 0 : ew2.Month = 9 : ew2.Rate = 0.05
            O.Add(ew2)
            Dim ew3 As iEWTypeLine = New EWType
            ew3.NameItem = "AGS-EW-12" : ew3.Type = 0 : ew3.Month = 12 : ew3.Rate = 0.06
            O.Add(ew3)
            Dim ew4 As iEWTypeLine = New EWType
            ew4.NameItem = "AGS-EW-15" : ew4.Type = 0 : ew4.Month = 15 : ew4.Rate = 0.07
            O.Add(ew4)
            Dim ew5 As iEWTypeLine = New EWType
            ew5.NameItem = "AGS-EW-21" : ew5.Type = 0 : ew5.Month = 21 : ew5.Rate = 0.09
            O.Add(ew5)
            Dim ew6 As iEWTypeLine = New EWType
            ew6.NameItem = "AGS-EW-24" : ew6.Type = 0 : ew6.Month = 24 : ew6.Rate = 0.1
            O.Add(ew6)
            Dim ew7 As iEWTypeLine = New EWType
            ew7.NameItem = "AGS-EW-36" : ew7.Type = 0 : ew7.Month = 36 : ew7.Rate = 0.15
            O.Add(ew7)
            If Type = 0 Then
                Dim ew8 As iEWTypeLine = New EWType
                ew8.NameItem = "AGS-EW-AD" : ew8.Type = 1 : ew8.Month = 99 : ew8.Rate = 0.05
                O.Add(ew8)
                Dim ew9 As iEWTypeLine = New EWType
                ew9.NameItem = "AGS-EW/DOA-03" : ew9.Type = 1 : ew9.Month = 999 : ew9.Rate = 0.01
                O.Add(ew9)
            End If
            Return O
        End If

        If (DocReg And Fixer.eDocReg.AUS) = DocReg Then
            Dim O As New List(Of iEWTypeLine)
            If DocReg = Fixer.eDocReg.AENC OrElse DocReg = Fixer.eDocReg.AAC Then
                Dim ew As iEWTypeLine = New EWType
                ew.NameItem = "AGS-EW-03" : ew.Type = 0 : ew.Month = 3 : ew.Rate = 0.03
                O.Add(ew)
                Dim ew1 As iEWTypeLine = New EWType
                ew1.NameItem = "AGS-EW-06" : ew1.Type = 0 : ew1.Month = 6 : ew1.Rate = 0.05
                O.Add(ew1)
                Dim ew3 As iEWTypeLine = New EWType
                ew3.NameItem = "AGS-EW-12" : ew3.Type = 0 : ew3.Month = 12 : ew3.Rate = 0.1
                O.Add(ew3)
                Dim ew6 As iEWTypeLine = New EWType
                ew6.NameItem = "AGS-EW-24" : ew6.Type = 0 : ew6.Month = 24 : ew6.Rate = 0.15
                O.Add(ew6)
                Dim ew7 As iEWTypeLine = New EWType
                ew7.NameItem = "AGS-EW-36" : ew7.Type = 0 : ew7.Month = 36 : ew7.Rate = 0.25
                O.Add(ew7)
            Else
                Dim ew As iEWTypeLine = New EWType
                ew.NameItem = "AGS-EW-03" : ew.Type = 0 : ew.Month = 3 : ew.Rate = 0.03
                O.Add(ew)
                Dim ew1 As iEWTypeLine = New EWType
                ew1.NameItem = "AGS-EW-06" : ew1.Type = 0 : ew1.Month = 6 : ew1.Rate = 0.05
                O.Add(ew1)
                Dim ew3 As iEWTypeLine = New EWType
                ew3.NameItem = "AGS-EW-12" : ew3.Type = 0 : ew3.Month = 12 : ew3.Rate = 0.1
                O.Add(ew3)
                Dim ew6 As iEWTypeLine = New EWType
                ew6.NameItem = "AGS-EW-24" : ew6.Type = 0 : ew6.Month = 24 : ew6.Rate = 0.15
                O.Add(ew6)
                Dim ew7 As iEWTypeLine = New EWType
                ew7.NameItem = "AGS-EW-36" : ew7.Type = 0 : ew7.Month = 36 : ew7.Rate = 0.25
                O.Add(ew7)
            End If
            Return O
        End If

        If (DocReg And Fixer.eDocReg.ATW) = DocReg Then
            Dim O As New List(Of iEWTypeLine)
            Dim ew As iEWTypeLine = New EWType
            ew.NameItem = "AGS-EW-03" : ew.Type = 0 : ew.Month = 3 : ew.Rate = 0.0125
            O.Add(ew)
            Dim ew1 As iEWTypeLine = New EWType
            ew1.NameItem = "AGS-EW-06" : ew1.Type = 0 : ew1.Month = 6 : ew1.Rate = 0.025
            O.Add(ew1)
            Dim ew3 As iEWTypeLine = New EWType
            ew3.NameItem = "AGS-EW-12" : ew3.Type = 0 : ew3.Month = 12 : ew3.Rate = 0.05
            O.Add(ew3)
            Dim ew6 As iEWTypeLine = New EWType
            ew6.NameItem = "AGS-EW-24" : ew6.Type = 0 : ew6.Month = 24 : ew6.Rate = 0.08
            O.Add(ew6)
            Dim ew7 As iEWTypeLine = New EWType
            ew7.NameItem = "AGS-EW-36" : ew7.Type = 0 : ew7.Month = 36 : ew7.Rate = 0.12
            O.Add(ew7)
            Return O
        End If

        'Frank added for Intercon sales team
        If (DocReg And Fixer.eDocReg.HQDC) = DocReg Then
            Dim O As New List(Of iEWTypeLine)
            Dim ew As iEWTypeLine = New EWType
            ew.NameItem = "AGS-EW-03" : ew.Type = 0 : ew.Month = 3 : ew.Rate = 0.0125
            O.Add(ew)
            Dim ew1 As iEWTypeLine = New EWType
            ew1.NameItem = "AGS-EW-06" : ew1.Type = 0 : ew1.Month = 6 : ew1.Rate = 0.025
            O.Add(ew1)
            Dim ew3 As iEWTypeLine = New EWType
            ew3.NameItem = "AGS-EW-12" : ew3.Type = 0 : ew3.Month = 12 : ew3.Rate = 0.05
            O.Add(ew3)
            Dim ew6 As iEWTypeLine = New EWType
            ew6.NameItem = "AGS-EW-24" : ew6.Type = 0 : ew6.Month = 24 : ew6.Rate = 0.08
            O.Add(ew6)
            Dim ew7 As iEWTypeLine = New EWType
            ew7.NameItem = "AGS-EW-36" : ew7.Type = 0 : ew7.Month = 36 : ew7.Rate = 0.12
            O.Add(ew7)
            Return O
        End If

        If (DocReg And Fixer.eDocReg.AKR) = DocReg Then
            Dim O As New List(Of iEWTypeLine)
            Dim ew As iEWTypeLine = New EWType
            ew.NameItem = "AGS-EW-03" : ew.Type = 0 : ew.Month = 3 : ew.Rate = 0.015
            O.Add(ew)
            Dim ew1 As iEWTypeLine = New EWType
            ew1.NameItem = "AGS-EW-06" : ew1.Type = 0 : ew1.Month = 6 : ew1.Rate = 0.03
            O.Add(ew1)
            Dim ew3 As iEWTypeLine = New EWType
            ew3.NameItem = "AGS-EW-12" : ew3.Type = 0 : ew3.Month = 12 : ew3.Rate = 0.06
            O.Add(ew3)
            Dim ew6 As iEWTypeLine = New EWType
            ew6.NameItem = "AGS-EW-24" : ew6.Type = 0 : ew6.Month = 24 : ew6.Rate = 0.1
            O.Add(ew6)
            Dim ew7 As iEWTypeLine = New EWType
            ew7.NameItem = "AGS-EW-36" : ew7.Type = 0 : ew7.Month = 36 : ew7.Rate = 0.15
            O.Add(ew7)
            Return O
        End If

        If (DocReg And Fixer.eDocReg.AJP) = DocReg Then
            Dim O As New List(Of iEWTypeLine)
            Dim ew As iEWTypeLine = New EWType
            ew.NameItem = "AGS-EW-03" : ew.Type = 0 : ew.Month = 3 : ew.Rate = 0.03
            O.Add(ew)
            Dim ew1 As iEWTypeLine = New EWType
            ew1.NameItem = "AGS-EW-05" : ew1.Type = 0 : ew1.Month = 5 : ew1.Rate = 0.055
            O.Add(ew1)
            Dim ew3 As iEWTypeLine = New EWType
            ew3.NameItem = "AGS-EW-12" : ew3.Type = 0 : ew3.Month = 12 : ew3.Rate = 0.1
            O.Add(ew3)
            Dim ew6 As iEWTypeLine = New EWType
            ew6.NameItem = "AGS-EW-24" : ew6.Type = 0 : ew6.Month = 24 : ew6.Rate = 0.18
            O.Add(ew6)
            Dim ew7 As iEWTypeLine = New EWType
            ew7.NameItem = "AGS-EW-36" : ew7.Type = 0 : ew7.Month = 36 : ew7.Rate = 0.25
            O.Add(ew7)
            Return O
        End If

        Dim Dl As New List(Of iEWTypeLine)
        Dim Dew As iEWTypeLine = New EWType
        Dew.NameItem = "AGS-EW-03" : Dew.Type = 0 : Dew.Month = 3 : Dew.Rate = 0.03
        Dl.Add(Dew)
        Return Dl
    End Function


End Class


Public Class EW
    Sub New(ByVal pLineNo As Integer, ByVal lineNo As Integer, ByVal partNo As String, ByVal org As String, ByVal divPlant As String, ByVal ewPrice As Decimal, _
            ByVal qty As Integer)
        _PlineNo = pLineNo
        _lineNo = lineNo
        _partNo = partNo
        _org = org
        _divPlant = divPlant
        _ewPrice = ewPrice
        _qty = qty
    End Sub
    Private _lineNo As Integer
    Public Property lineNo As Integer
        Get
            Return _lineNo
        End Get
        Set(ByVal value As Integer)
            _lineNo = value
        End Set
    End Property
    Private _partNo As String
    Public Property partNo As String
        Get
            Return _partNo
        End Get
        Set(ByVal value As String)
            _partNo = value
        End Set
    End Property

    Private _org As String
    Public Property org As String
        Get
            Return _org
        End Get
        Set(ByVal value As String)
            _org = value
        End Set
    End Property

    Private _divPlant As String
    Public Property divPlant As String
        Get
            Return _divPlant
        End Get
        Set(ByVal value As String)
            _divPlant = value
        End Set
    End Property
    Private _ewPrice As Decimal
    Public Property ewPrice As String
        Get
            Return _ewPrice
        End Get
        Set(ByVal value As String)
            _ewPrice = value
        End Set
    End Property
    Private _qty As Integer
    Public Property qty As Integer
        Get
            Return _qty
        End Get
        Set(ByVal value As Integer)
            _qty = value
        End Set
    End Property
    Private _PlineNo As Integer
    Public Property PlineNo As Integer
        Get
            Return _PlineNo
        End Get
        Set(ByVal value As Integer)
            _PlineNo = value
        End Set
    End Property
End Class
Public MustInherit Class oProd
    Implements iProd
    Sub New(ByVal partNo As String, ByVal org As String, Optional ByVal divPlant As String = "")
        partNo = replaceCartBTO(partNo, org)
        Dim r As DataRow = getProductStatusLine(partNo, org)
        If Not IsNothing(r) Then
            Me._partNo = partNo
            Me._org = org
            Me._divPlant = divPlant
            Me._type = CartFixer.eProdType.ItemStandard
            If Not IsDBNull(r.Item("PRODUCT_DESC")) Then
                Me._partDesc = r.Item("PRODUCT_DESC")
            Else
                Me._partDesc = ""
            End If
            If Not IsDBNull(r.Item("MODEL_NO")) Then
                Me._partModel = r.Item("MODEL_NO")
            Else
                Me._partModel = ""
            End If

            If Not IsDBNull(r.Item("PRODUCT_LINE")) Then
                Me._productLine = r.Item("PRODUCT_LINE")
            Else
                Me._productLine = ""
            End If

            If Not IsDBNull(r.Item("DLV_PLANT")) Then
                Me._divPlant = r.Item("DLV_PLANT")
            Else
                Me._divPlant = ""
            End If

            If Not IsDBNull(r.Item("ABC_INDICATOR")) Then
                Me._ABC = r.Item("ABC_INDICATOR")
            Else
                Me._ABC = ""
            End If
            If divPlant <> "" Then
                Me._divPlant = divPlant
            End If
            If Not IsDBNull(r.Item("ROHS_FLAG")) AndAlso IsNumeric(r.Item("ROHS_FLAG")) Then
                Me._ROHS = CInt(r.Item("ROHS_FLAG"))
            Else
                Me._ROHS = 0
            End If

            If Not IsDBNull(r.Item("EXTENDED_DESC")) Then Me.partDesc = r.Item("EXTENDED_DESC")
            Me._prodValid = True
        Else
            Throw New Exception(COMM.EnumHelper.getDescription(COMM.Msg.eErrCode.InvalidPartNo) & ":" & partNo)
        End If
    End Sub

    Private _prodValid As Boolean = False
    Public ReadOnly Property prodValid As Boolean
        Get
            Return _prodValid
        End Get
    End Property
    Private Function replaceCartBTO(ByVal PN As String, ByVal Org As String) As String
        '\chang org from JP01 to US01 ,  Test custom Btos for mary.huang
        ' If String.Equals(Org, "JP01", StringComparison.CurrentCultureIgnoreCase) Then Org = "US01"
        '/ end
        If PN.StartsWith("EZ-") Then PN = PN.Substring(3, PN.Length - 3)
        Dim vnumber As Object = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", PN.ToString, Org.ToUpper.Substring(0, 2)))
        If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
            Return vnumber
        End If

        '=============Ryan 20171116 Comment below out=============
        'vnumber = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", PN.ToString.Trim + "-BTO", Org.ToUpper.Substring(0, 2)))
        'If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
        '    Return vnumber
        'End If
        '==============End Ryan 20171116 Comment out==============

        If PN.Trim.EndsWith("-BTO") Then
            Dim Temp_PN = PN.Substring(0, PN.Length - 4)
            vnumber = dbUtil.dbExecuteScalar("MY", String.Format("select top 1 vnumber from EZ_CBOM_MAPPING where number='{0}' and ORG ='{1}'  order by ismanual  desc ", Temp_PN, Org.ToUpper.Substring(0, 2)))
            If Not IsNothing(vnumber) AndAlso vnumber.ToString <> "" Then
                Return vnumber
            End If
        Else
            Return PN
        End If
        Return PN
    End Function
    Private Function getProductStatusLine(ByVal partNo As String, ByVal org As String) As DataRow
        Dim dt As New DataTable
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@PN", partNo), _
                                             New SqlClient.SqlParameter("@SALESORG", org)}
        dt = sqlhelper.getDT("MY", CommandType.Text, String.Format("SELECT a.PART_NO, a.MODEL_NO, a.MATERIAL_GROUP, a.PRODUCT_GROUP, a.PRODUCT_DIVISION, a.PRODUCT_LINE," & _
        " a.GENITEMCATGRP, a.PRODUCT_DESC, a.ROHS_FLAG, a.EGROUP, a.EDIVISION, a.NET_WEIGHT," & _
        " a.GROSS_WEIGHT, a.WEIGHT_UNIT, a.VOLUME, a.VOLUME_UNIT, a.CREATE_DATE, a.LAST_UPD_DATE," & _
        " a.PRODUCT_TYPE, a.GIP_CODE, a.SIZE_DIMENSIONS, b.SALES_ORG, b.PRODUCT_STATUS, b.MIN_ORDER_QTY," & _
        " b.DLV_PLANT, b.MATERIAL_PRICING_GRP, b.VALID_DATE, b.ITEM_CATEGORY_GROUP, c.PLANT, isnull(c.ABC_INDICATOR,'') as ABC_INDICATOR, " & _
        " d.EXTENDED_DESC " & _
        " FROM SAP_PRODUCT AS a INNER JOIN  " & _
                            " SAP_PRODUCT_STATUS AS b ON a.PART_NO = b.PART_NO LEFT OUTER JOIN " & _
                            " SAP_PRODUCT_ABC AS c ON a.PART_NO = c.PART_NO AND b.DLV_PLANT = c.PLANT LEFT OUTER JOIN " & _
                            " SAP_PRODUCT_EXT_DESC AS d ON a.PART_NO = d.PART_NO " & _
        " WHERE (a.PART_NO = @PN) AND (b.SALES_ORG = @SALESORG)", partNo, org), p)
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            Return dt.Rows(0)
        End If
        Return Nothing
    End Function
    Private _partNo As String = ""
    Public Property partNo As String Implements iProd.partNo
        Get
            Return _partNo
        End Get
        Set(ByVal value As String)
            _partNo = value
        End Set
    End Property
    Private _org As String = ""
    Public Property org As String Implements iProd.org
        Get
            Return _org
        End Get
        Set(ByVal value As String)
            _org = value
        End Set
    End Property
    Private _divPlant As String = ""
    Public Property divPlant As String Implements iProd.divPlant
        Get
            Return _divPlant
        End Get
        Set(ByVal value As String)
            _divPlant = value
        End Set
    End Property
    Private _partDesc As String = ""
    Public Property partDesc As String Implements iProd.partDesc
        Get
            Return _partDesc
        End Get
        Set(ByVal value As String)
            _partDesc = value
        End Set
    End Property
    Private _partModel As String = ""
    Public Property partModel As String Implements iProd.partModel
        Get
            Return _partModel
        End Get
        Set(ByVal value As String)
            _partModel = value
        End Set
    End Property
    Private _type As CartFixer.eProdType = CartFixer.eProdType.ItemStandard
    Public Property type As CartFixer.eProdType Implements IBUS.iProd.type
        Get
            Return _type
        End Get
        Set(ByVal value As CartFixer.eProdType)
            _type = value
        End Set
    End Property
    Private _productLine As String = ""
    Public Property productLine As String Implements iProd.productLine
        Get
            Return _productLine
        End Get
        Set(ByVal value As String)
            _productLine = value
        End Set
    End Property
    Private _ROHS As Integer = 0
    Public Property ROHS As Integer Implements iProd.ROHS
        Get
            Return _ROHS
        End Get
        Set(ByVal value As Integer)
            _ROHS = value
        End Set
    End Property
    Private _ABC As String = ""
    Public Property ABC As String Implements iProd.ABC
        Get
            Return _ABC
        End Get
        Set(ByVal value As String)
            _ABC = value
        End Set
    End Property

    Private _errCode As COMM.Msg.eErrCode = COMM.Msg.eErrCode.OK
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property

    Private Function getCountryCodebyShipTo(ByVal ShipToId As String) As String
        Return cartBiz.getCountryCodebyShipTo(ShipToId)
    End Function
    Function isTaxable(ByVal PartNo As String, ByVal ShipToId As String) As Boolean Implements iProd.isTaxable
        Return cartBiz.isTaxable(PartNo, ShipToId)
    End Function
    Function isWarrantable(ByVal PN As String, ByVal docReg As COMM.Fixer.eDocReg) As Boolean Implements iProd.isWarrantable
        Return cartBiz.isWarrantable(PN, docReg)
    End Function
    Function isSoftware(ByVal PN As String) As Boolean Implements iProd.isSoftware
        Return cartBiz.isSoftware(PN)
    End Function
End Class
Public Class ProdStandard
    Inherits oProd
    Sub New(ByVal partNo As String, ByVal org As String, Optional ByVal divPlant As String = "")
        MyBase.New(partNo, org, divPlant)
        Me.type = CartFixer.eProdType.ItemStandard
    End Sub
End Class
Public Class ProdAGS
    Inherits oProd
    Sub New(ByVal partNo As String, ByVal org As String, Optional ByVal divPlant As String = "")
        MyBase.New(partNo, org, divPlant)
        Me.type = CartFixer.eProdType.ItemAGSEW
    End Sub
End Class
Public Class ProdBTO
    Inherits oProd
    Sub New(ByVal partNo As String, ByVal org As String, Optional ByVal divPlant As String = "")
        MyBase.New(partNo, org, divPlant)
        Me.type = CartFixer.eProdType.ItemBTO
    End Sub
End Class
Public Class ProdPTD
    Inherits oProd
    Sub New(ByVal partNo As String, ByVal org As String, Optional ByVal divPlant As String = "")
        MyBase.New(partNo, org, divPlant)
        Me.type = CartFixer.eProdType.ItemPTD
    End Sub
End Class
Public Class ProdDummy
    Inherits oProd
    Sub New(ByVal partNo As String, ByVal org As String, Optional ByVal divPlant As String = "")
        MyBase.New(partNo, org, divPlant)
        Me.type = CartFixer.eProdType.Dummy
    End Sub
End Class
Public Class ProdHardDrive
    Inherits oProd
    Sub New(ByVal partNo As String, ByVal org As String, Optional ByVal divPlant As String = "")
        MyBase.New(partNo, org, divPlant)
        Me.type = CartFixer.eProdType.HardDrive
    End Sub
End Class
Public Class prodF : Implements iProdF

    Public Function getProdByPartNo(ByVal partNo As String, ByVal org As String, Optional ByVal divPlant As String = "") As IBUS.iProd Implements IBUS.iProdF.getProdByPartNo
        If partNo.ToUpper.Contains("AGS-") Then
            Return New ProdAGS(partNo, org, divPlant)
        End If
        If partNo.ToUpper.Contains("-BTO") Then
            Return New ProdBTO(partNo, org, divPlant)
        End If
        If partNo.ToUpper.Contains("NO NEED") Then
            Return New ProdDummy(partNo, org, divPlant)
        End If
        If partNo.ToUpper.Contains("Build In") Then
            Return New ProdDummy(partNo, org, divPlant)
        End If
        If partNo.ToUpper.StartsWith("96HD") Or partNo.ToUpper.StartsWith("96MD") Then
            Return New ProdHardDrive(partNo, org, divPlant)
        End If
        Return New ProdStandard(partNo, org, divPlant)
    End Function
    Private _errCode As COMM.Msg.eErrCode = COMM.Msg.eErrCode.OK
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property
End Class