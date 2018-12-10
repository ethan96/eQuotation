Public Class CartLineQ
    Inherits oCartLine

    Sub New(ByVal key As String, ByVal lineNo As Integer, ByVal parentLineNo As Integer,
           ByVal partNo As String, ByVal partDesc As String, ByVal listPrice As Decimal, ByVal newUnitPrice As Decimal, ByVal newCost As Decimal, _
           ByVal Qty As Integer, ByVal EWFlag As Integer, ByVal divPlant As String, ByVal reqDate As DateTime, ByVal ItemType As CartFixer.eItemType, ByVal category As String, ByVal VirtualPartNo As String, ByVal RecyclingFee As Decimal)
        MyBase.New(key, lineNo, parentLineNo, partNo, partDesc, listPrice, newUnitPrice, newCost, Qty, EWFlag, divPlant, reqDate, ItemType, category, VirtualPartNo, RecyclingFee)
    End Sub
    Sub New(ByVal key As String, ByVal lineNo As Integer, ByVal parentLineNo As Integer,
            ByVal p As oProd, ByVal newUnitPrice As Decimal, ByVal listPrice As Decimal, ByVal newCost As Decimal, _
            ByVal Qty As Integer, ByVal EWFlag As Integer, ByVal reqDate As DateTime, ByVal ItemType As CartFixer.eItemType, ByVal category As String)
        MyBase.New(key, lineNo, parentLineNo, p, newUnitPrice, listPrice, newCost, Qty, EWFlag, reqDate, ItemType, category)
    End Sub
    Sub New()
        MyBase.New()
    End Sub
    Protected Overrides Sub setFieldName()
        Me._key = New COMM.Field("quoteid", Me._key.Value)
        Me._canBeConfirmed = New COMM.Field("canBeConfirmed", Me._canBeConfirmed.Value)
        Me._category = New COMM.Field("category", Me._category.Value)
        Me._cost = New COMM.Field("itp", Me._cost.Value)
        Me._divPlant = New COMM.Field("deliveryPlant", Me._divPlant.Value)
        Me._dueDate = New COMM.Field("dueDate", Me.dueDate.Value)
        Me._ewFlag = New COMM.Field("ewFlag", Me._ewFlag.Value)
        Me._inventory = New COMM.Field("inventory", Me._inventory.Value)
        Me._itemType = New COMM.Field("itemType", Me._itemType.Value)
        Me._lineNo = New COMM.Field("Line_No", Me._lineNo.Value)
        Me._listPrice = New COMM.Field("listPrice", Me._listPrice.Value)
        Me._newCost = New COMM.Field("newITP", Me._newCost.Value)
        Me._newUnitPrice = New COMM.Field("newUnitPrice", Me.newunitPrice.Value)
        Me._parentLineNo = New COMM.Field("higherLevel", Me._parentLineNo.Value)
        Me._partDesc = New COMM.Field("description", Me._partDesc.Value)
        Me._partNo = New COMM.Field("PartNo", Me._partNo.Value)
        Me._qty = New COMM.Field("Qty", Me._qty.Value)
        Me._reqDate = New COMM.Field("reqDate", Me._reqDate.Value)
        Me._unitPrice = New COMM.Field("unitPrice", Me._unitPrice.Value)
        Me._CustMaterial = New COMM.Field("custMaterial", Me._CustMaterial.Value)
        Me._RHOS = New COMM.Field("rohs", Me._RHOS.Value)
        Me._satisfyflag = New COMM.Field("satisfyFlag", Me._satisfyflag.Value)
        Me._DMFFlag = New COMM.Field("DMF_Flag", Me._DMFFlag.Value)
        Me._VirtualPartNo = New COMM.Field("VirtualPartNo", Me._VirtualPartNo.Value)
        Me._RecyclingFee = New COMM.Field("RecyclingFee", Me._RecyclingFee.Value)
        Me._ShipPoint = New COMM.Field("ShipPoint", Me._ShipPoint.Value)
        Me._StorageLoc = New COMM.Field("StorageLoc", Me._StorageLoc.Value)
        Me._DELIVERYGROUP = New COMM.Field("DELIVERYGROUP", Me._DELIVERYGROUP.Value)
        Me._RECFIGID = New COMM.Field("RECFIGID", Me._RECFIGID.Value)
    End Sub
End Class
Public Class CartListQ
    Inherits oCartList
End Class
Public Class CartQ
    Inherits oCart

    Public Sub New(ByVal pTbSource As tbSource, ByVal key As String, ByVal org As String, ByVal pCartStruc As IBUS.iCartLine)
        MyBase.New(pTbSource, key, org, pCartStruc)
    End Sub

    Protected Overrides Function getEWItem(ByVal ewItem As EW) As IBUS.iCartLine
        Dim pF As IBUS.iProdF = New prodF
        Return New CartLineQ(Me.key, ewItem.lineNo, ewItem.PlineNo, pF.getProdByPartNo(ewItem.partNo, ewItem.org, ewItem.divPlant), ewItem.ewPrice, ewItem.ewPrice, ewItem.ewPrice, ewItem.qty, 0, Now.Date.ToShortDateString, Fixer.eItemType.Others, "")
    End Function
    Protected Overrides Function getItemByDataRow(ByVal r As System.Data.DataRow) As oCartLine
        Dim o As New CartLineQ()
        Dim f As oCartLine = New CartLineQ(r.Item(o.key.Name), r.Item(o.lineNo.Name), r.Item(o.parentLineNo.Name), r.Item(o.partNo.Name), r.Item(o.partDesc.Name), _
                              r.Item(o.listPrice.Name), r.Item(o.newunitPrice.Name), _
                                r.Item(o.newCost.Name), r.Item(o.Qty.Name), r.Item(o.ewFlag.Name), r.Item(o.divPlant.Name), r.Item(o.reqDate.Name), r.Item(o.itemType.Name), r.Item(o.category.Name), r.Item(o.VirtualPartNo.Name), r.Item(o.RecyclingFee.Name))

        f.CustMaterial.Value = r.Item(o.CustMaterial.Name)
        f.RHOS.Value = r.Item(o.RHOS.Name)
        o.satisfyflag.Value = r.Item(o.satisfyflag.Name)
        o.DMFFlag.Value = r.Item(o.DMFFlag.Name)
        o.VirtualPartNo.Value = r.Item(o.VirtualPartNo.Name)
        o.RecyclingFee.Value = r.Item(o.RecyclingFee.Name)
        o.ShipPoint.Value = r.Item(o.ShipPoint.Name)
        o.StorageLoc.Value = r.Item(o.StorageLoc.Name)
        o.DELIVERYGROUP.Value = r.Item(o.DELIVERYGROUP.Name)
        o.RECFIGID.Value = r.Item(o.RECFIGID.Name)
        If f.isValidLine = False Then
            Return Nothing
        End If
        Return f
    End Function
    Protected Overrides Function getNewLineList() As iCartList
        Return New CartListQ()
    End Function
End Class