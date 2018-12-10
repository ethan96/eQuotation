Public Class CartLineC
    Inherits oCartLine

    Sub New(ByVal key As String, ByVal lineNo As Integer, ByVal parentLineNo As Integer,
           ByVal partNo As String, ByVal partDesc As String, ByVal listPrice As Decimal, ByVal newUnitPrice As Decimal, ByVal newCost As Decimal, _
           ByVal Qty As Integer, ByVal EWFlag As Integer, ByVal divPlant As String, Optional ByVal reqDate As DateTime = Nothing, Optional ByVal ItemType As CartFixer.eItemType = CartFixer.eItemType.Others, Optional ByVal category As String = "")
        MyBase.New(key, lineNo, parentLineNo, partNo, partDesc, listPrice, newUnitPrice, newCost, Qty, EWFlag, divPlant, reqDate, ItemType, category)
    End Sub
    Sub New(ByVal key As String, ByVal lineNo As Integer, ByVal parentLineNo As Integer,
            ByVal p As oProd, ByVal newUnitPrice As Decimal, ByVal listPrice As Decimal, ByVal newCost As Decimal, _
            ByVal Qty As Integer, ByVal EWFlag As Integer, Optional ByVal reqDate As DateTime = Nothing, Optional ByVal ItemType As CartFixer.eItemType = CartFixer.eItemType.Others, Optional ByVal category As String = "")
        MyBase.New(key, lineNo, parentLineNo, p, newUnitPrice, listPrice, newCost, Qty, EWFlag, reqDate, ItemType, category)
    End Sub
    Sub New()
        MyBase.New()
    End Sub

    Private _Rohs As New COMM.Field("Rohs", 0)
    Public Property ROHS As COMM.Field
        Get
            Return _Rohs
        End Get
        Set(ByVal value As COMM.Field)
            _Rohs.Value = value.Value
        End Set
    End Property

    Private _ClassABC As New COMM.Field("Class", "")
    Public Property ClassABC As COMM.Field
        Get
            Return _ClassABC
        End Get
        Set(ByVal value As COMM.Field)
            _ClassABC.Value = value.Value
        End Set
    End Property

    Protected Overrides Sub setFieldName()
        Me._key = New COMM.Field("CART_id", Me._key.Value)
        Me._canBeConfirmed = New COMM.Field("canBeConfirmed", Me._canBeConfirmed.Value)
        Me._category = New COMM.Field("category", Me._category.Value)
        Me._cost = New COMM.Field("itp", Me._cost.Value)
        Me._divPlant = New COMM.Field("delivery_Plant", Me._divPlant.Value)
        Me._dueDate = New COMM.Field("due_Date", Me.dueDate.Value)
        Me._ewFlag = New COMM.Field("ew_Flag", Me._ewFlag.Value)
        Me._inventory = New COMM.Field("inventory", Me._inventory.Value)
        Me._itemType = New COMM.Field("otype", Me._itemType.Value)
        Me._lineNo = New COMM.Field("Line_No", Me._lineNo.Value)
        Me._listPrice = New COMM.Field("list_Price", Me._listPrice.Value)
        Me._newCost = New COMM.Field("ITP", Me._newCost.Value)
        Me._newUnitPrice = New COMM.Field("Unit_Price", Me.newunitPrice.Value)
        Me._parentLineNo = New COMM.Field("higherLevel", Me._parentLineNo.Value)
        Me._partDesc = New COMM.Field("description", Me._partDesc.Value)
        Me._partNo = New COMM.Field("Part_No", Me._partNo.Value)
        Me._qty = New COMM.Field("Qty", Me._qty.Value)
        Me._reqDate = New COMM.Field("req_Date", Me._reqDate.Value)
        Me._unitPrice = New COMM.Field("ounit_Price", Me._unitPrice.Value)
    End Sub
End Class
Public Class CartListC
    Inherits oCartList
End Class
Public Class CartC
    Inherits oCart

    Public Sub New(ByVal pTbSource As tbSource, ByVal key As String, ByVal org As String, ByVal pCartStruc As IBUS.iCartLine)
        MyBase.New(pTbSource, key, org, pCartStruc)
    End Sub
    Protected Overrides Function getEWItem(ByVal ewItem As EW) As IBUS.iCartLine
        Dim pF As IBUS.iProdF = New prodF
        Return New CartLineC(Me.key, ewItem.lineNo, ewItem.PlineNo, pF.getProdByPartNo(ewItem.partNo, ewItem.org, ewItem.divPlant), ewItem.ewPrice, ewItem.ewPrice, ewItem.ewPrice, ewItem.qty, 0)
    End Function
    Protected Overrides Function getItemByDataRow(ByVal r As System.Data.DataRow) As oCartLine
        Dim o As New CartLineC()
        Dim f As oCartLine = New CartLineC(r.Item(o.key.Name), r.Item(o.lineNo.Name), r.Item(o.parentLineNo.Name), r.Item(o.partNo.Name), r.Item(o.partDesc.Name), _
                              r.Item(o.listPrice.Name), r.Item(o.newunitPrice.Name), _
                                r.Item(o.newCost.Name), r.Item(o.Qty.Name), r.Item(o.ewFlag.Name), r.Item(o.divPlant.Name), r.Item(o.reqDate.Name), r.Item(o.itemType.Name), r.Item(o.category.Name))
        If f.isValidLine = False Then
            Return Nothing
        End If
        Return f
    End Function
    Public Function Add2Cart(ByVal quoteId As String, ByVal partNo As String, ByVal QTY As Integer, ByVal ewFlag As Integer, _
                               ByVal HigherLevel As Integer, ByVal itemType As CartFixer.eItemType, ByVal category As String, ByVal isSyncPrice As Integer, _
                               ByVal isSyncATP As Integer, ByVal ReqDate As Date, ByVal ParentLineNo As Integer, ByVal plantID As String, _
                               ByRef LineNo As Integer, ByRef strErrorMessage As String) As Boolean


        Return True
    End Function


    Protected Overrides Function getNewLineList() As iCartList
        Return New CartListC()
    End Function
End Class


Public Class CartLineO
    Inherits oCartLine

    Sub New(ByVal key As String, ByVal lineNo As Integer, ByVal parentLineNo As Integer,
           ByVal partNo As String, ByVal partDesc As String, ByVal listPrice As Decimal, ByVal newUnitPrice As Decimal, ByVal newCost As Decimal, _
           ByVal Qty As Integer, ByVal EWFlag As Integer, ByVal divPlant As String, Optional ByVal reqDate As DateTime = Nothing, Optional ByVal ItemType As CartFixer.eItemType = CartFixer.eItemType.Others, Optional ByVal category As String = "")
        MyBase.New(key, lineNo, parentLineNo, partNo, partDesc, listPrice, newUnitPrice, newCost, Qty, EWFlag, divPlant, reqDate, ItemType, category)
    End Sub
    Sub New(ByVal key As String, ByVal lineNo As Integer, ByVal parentLineNo As Integer,
            ByVal p As oProd, ByVal newUnitPrice As Decimal, ByVal listPrice As Decimal, ByVal newCost As Decimal, _
            ByVal Qty As Integer, ByVal EWFlag As Integer, Optional ByVal reqDate As DateTime = Nothing, Optional ByVal ItemType As CartFixer.eItemType = CartFixer.eItemType.Others, Optional ByVal category As String = "")
        MyBase.New(key, lineNo, parentLineNo, p, newUnitPrice, listPrice, newCost, Qty, EWFlag, reqDate, ItemType, category)
    End Sub
    Sub New()
        MyBase.New()
    End Sub

    Private _Rohs As New COMM.Field("Rohs_Flag", 0)
    Public Property ROHS As COMM.Field
        Get
            Return _Rohs
        End Get
        Set(ByVal value As COMM.Field)
            _Rohs.Value = value.Value
        End Set
    End Property

    Protected Overrides Sub setFieldName()
        'Me._key = New COMM.Field("Order_id", Me._key.Value)
        Me._key = New COMM.Field("quoteid", Me._key.Value)
        Me._canBeConfirmed = New COMM.Field("NOATPFLAG", Me._canBeConfirmed.Value)
        Me._category = New COMM.Field("cate", Me._category.Value)
        Me._cost = New COMM.Field("itp", Me._cost.Value)
        Me._divPlant = New COMM.Field("deliveryPlant", Me._divPlant.Value)
        Me._dueDate = New COMM.Field("due_Date", Me.dueDate.Value)
        Me._ewFlag = New COMM.Field("exwarranty_Flag", Me._ewFlag.Value)
        Me._inventory = New COMM.Field("Order_id", Me._inventory.Value)
        'Me._itemType = New COMM.Field("ORDER_LINE_TYPE", Me._itemType.Value)
        Me._itemType = New COMM.Field("itemType", Me._itemType.Value)
        Me._lineNo = New COMM.Field("Line_No", Me._lineNo.Value)
        Me._listPrice = New COMM.Field("list_Price", Me._listPrice.Value)
        Me._newCost = New COMM.Field("ITP", Me._newCost.Value)
        Me._newUnitPrice = New COMM.Field("Unit_Price", Me.newunitPrice.Value)
        Me._parentLineNo = New COMM.Field("higherLevel", Me._parentLineNo.Value)
        Me._partDesc = New COMM.Field("description", Me._partDesc.Value)
        Me._partNo = New COMM.Field("Part_No", Me._partNo.Value)
        Me._qty = New COMM.Field("Qty", Me._qty.Value)
        Me._reqDate = New COMM.Field("required_Date", Me._reqDate.Value)
        Me._unitPrice = New COMM.Field("unit_Price", Me._unitPrice.Value)
    End Sub
End Class
Public Class CartListO
    Inherits oCartList
End Class
Public Class CartO
    Inherits oCart

    Public Sub New(ByVal pTbSource As tbSource, ByVal key As String, ByVal org As String, ByVal pCartStruc As IBUS.iCartLine)
        MyBase.New(pTbSource, key, org, pCartStruc)
    End Sub

    Protected Overrides Function getEWItem(ByVal ewItem As EW) As IBUS.iCartLine
        Dim pF As IBUS.iProdF = New prodF
        Return New CartLineO(Me.key, ewItem.lineNo, ewItem.PlineNo, pF.getProdByPartNo(ewItem.partNo, ewItem.org, ewItem.divPlant), ewItem.ewPrice, ewItem.ewPrice, ewItem.ewPrice, ewItem.qty, 0)
    End Function
    Protected Overrides Function getItemByDataRow(ByVal r As System.Data.DataRow) As oCartLine
        Dim o As New CartLineO()
        Dim f As oCartLine = New CartLineO(r.Item(o.key.Name), r.Item(o.lineNo.Name), r.Item(o.parentLineNo.Name), r.Item(o.partNo.Name), r.Item(o.partDesc.Name), _
                              r.Item(o.listPrice.Name), r.Item(o.newunitPrice.Name), _
                                r.Item(o.newCost.Name), r.Item(o.Qty.Name), r.Item(o.ewFlag.Name), r.Item(o.divPlant.Name), r.Item(o.reqDate.Name), r.Item(o.itemType.Name), r.Item(o.category.Name))
        If f.isValidLine = False Then
            Return Nothing
        End If
        Return f
    End Function
    Public Function Add2Cart(ByVal quoteId As String, ByVal partNo As String, ByVal QTY As Integer, ByVal ewFlag As Integer, _
                               ByVal HigherLevel As Integer, ByVal itemType As CartFixer.eItemType, ByVal category As String, ByVal isSyncPrice As Integer, _
                               ByVal isSyncATP As Integer, ByVal ReqDate As Date, ByVal ParentLineNo As Integer, ByVal plantID As String, _
                               ByRef LineNo As Integer, ByRef strErrorMessage As String) As Boolean


        Return True
    End Function


    Protected Overrides Function getNewLineList() As iCartList
        Return New CartListO()
    End Function
End Class