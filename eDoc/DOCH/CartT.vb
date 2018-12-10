Public Class CartF : Implements iCartF
    Private _errCode As COMM.Msg.eErrCode = COMM.Msg.eErrCode.OK
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get

    End Property

    Public Function getCartByAppArea(ByVal AppArea As COMM.Fixer.eCartAppArea, ByVal Key As String, ByVal Org As String) As IBUS.iCart(Of IBUS.iCartLine) Implements IBUS.iCartF.getCartByAppArea
        'If AppArea = COMM.Fixer.eCartAppArea.Cart Then
        '    Return New CartC(New tbSource(AppArea), Key, Org, New CartLineC())
        'End If
        If AppArea = COMM.Fixer.eCartAppArea.EQ Then
            Return New CartQ(New tbSource(AppArea), Key, Org, New CartLineQ())
        End If
        If AppArea = COMM.Fixer.eCartAppArea.Order Then
            Return New CartO(New tbSource(AppArea), Key, Org, New CartLineO())
        End If
        Return Nothing
    End Function
End Class

Partial Public MustInherit Class oCart

    Private Function getNewLineNo(ByVal parentLineNo As Integer, ByVal ItemType As CartFixer.eItemType, ByRef expCode As COMM.Msg.eErrCode) As Integer
        Dim stp As Integer = CartFixer.getStepByParent(parentLineNo, ItemType)
        Dim stt As Integer = CartFixer.getStartByParent(parentLineNo)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key), _
                                             New SqlClient.SqlParameter("@parenLineNo", parentLineNo), _
                                             New SqlClient.SqlParameter("@itemType", ItemType)}
        Dim str As String = String.Format("select {0} from {1} where {3}=@key and {2}=@parenLineNo and {4}=@itemType order by {0} asc", Me.cartStruc.lineNo.Name, Me.tbSource.tbName, Me.cartStruc.parentLineNo.Name, Me.cartStruc.key.Name, Me.cartStruc.itemType.Name)
        Dim r As SqlClient.SqlDataReader = sqlhelper.ExecuteReader(Me.tbSource.conn, CommandType.Text, str, p)
        Dim f As Integer = 0
        Dim rd As Integer = 0
        Dim nline As Integer = stt
        While r.Read
            nline = nline + stp
            rd = 1
            Dim t As Integer = r.Item(Me.cartStruc.lineNo.Name)
            If nline <> t Then
                f = 1
                Exit While
            End If
        End While
        r.Close()
        Dim ret As Integer = nline
        If rd = 0 Then
            If Not CartFixer.isValidParentLineNo(parentLineNo) Then
                expCode = expCode Or COMM.Msg.eErrCode.HigherLevelCannotBeFound
                Return CartFixer.eUndef.X
            End If
            ret = parentLineNo + stp
        End If
        If f <> 1 Then
            ret = nline + stp
        End If
        While isLineNoExists(ret)
            ret = ret + stp
        End While
        If CartFixer.isItemTypeIncorrect(ItemType, ret) Then
            expCode = expCode Or COMM.Msg.eErrCode.NewLineCannotMapToItemType
            Return CartFixer.eUndef.X
        End If
        If CartFixer.getParentLineNoFromLineNo(ret) <> parentLineNo Then
            expCode = expCode Or COMM.Msg.eErrCode.NewLineCannotMapToHigherLevel
            Return CartFixer.eUndef.X
        End If
        Return ret
    End Function


    Private Function isLineNoExists(ByVal lineNo As Integer) As Boolean

        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key), _
                                             New SqlClient.SqlParameter("@lineNo", lineNo)}
        Dim n As New Object
        n = sqlhelper.ExecuteScalar(Me.tbSource.conn, CommandType.Text, String.Format("select count({0}) from {1} where {0}=@key and {2}=@lineNo", Me.cartStruc.key.Name, Me.tbSource.tbName, Me.cartStruc.lineNo.Name), p)
        If IsNumeric(n) AndAlso n > 0 Then
            Return True
        End If
        Return False
    End Function
    Private Function AddNew(ByVal item As IBUS.iCartLine, ByVal oType As COMM.Fixer.eDocType) As Integer

        If item.parentLineNo.Value <> CartFixer.StartLine And (Not (CartFixer.isValidParentLineNo(item.parentLineNo.Value) AndAlso isLineNoExists(item.parentLineNo.Value))) Then
            Me._errCode = Me._errCode Or COMM.Msg.eErrCode.HigherLevelCannotBeFound
            Return CartFixer.eUndef.X
        End If
        Dim expCode As Integer = 0
        Dim NLineNo = getNewLineNo(item.parentLineNo.Value, item.itemType.Value, expCode)

        If expCode <> 0 Then
            Me._errCode = Me._errCode Or expCode
            Return CartFixer.eUndef.X
        End If
        item.lineNo.Value = NLineNo
        Me.dbInsert(item, oType)
        Return NLineNo
    End Function

    Private Sub InsertB(ByVal item As IBUS.iCartLine)

        If item.parentLineNo.Value <> CartFixer.StartLine And (Not (CartFixer.isValidParentLineNo(item.parentLineNo.Value) AndAlso isLineNoExists(item.parentLineNo.Value))) Then
            Me._errCode = Me._errCode Or COMM.Msg.eErrCode.HigherLevelCannotBeFound
            Exit Sub
        End If
        If CartFixer.isItemTypeIncorrect(item.itemType.Value, item.lineNo.Value) Then
            Me._errCode = Me._errCode Or COMM.Msg.eErrCode.ItemTypeIncorrect
            Exit Sub
        End If
        If CartFixer.getParentLineNoFromLineNo(item.lineNo.Value) <> item.parentLineNo.Value Then
            Me._errCode = Me._errCode Or COMM.Msg.eErrCode.LineNoCannotMapToHigherLevel
            Exit Sub
        End If
        Dim expCode As Integer = 0
        updateLineInsertOrRemove(item.lineNo.Value, item.parentLineNo.Value, item.itemType.Value, CartFixer.eInsertOrRemove.Insert, expCode)
        If expCode <> 0 Then
            Me._errCode = Me._errCode Or expCode
            Exit Sub
        End If
        Me.dbInsert(item, Fixer.eDocType.EQ)
    End Sub
    Private Sub updateLineInsertOrRemove(ByVal lineNo As Integer, ByVal parentLineNo As Integer, ByVal ItemType As CartFixer.eItemType, ByVal InsertOrRemove As CartFixer.eInsertOrRemove, ByRef expCode As COMM.Msg.eErrCode)
        Dim s As Integer = CartFixer.getStepByParent(parentLineNo, ItemType)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key), _
                                             New SqlClient.SqlParameter("@lineNo", lineNo), _
                                          New SqlClient.SqlParameter("@parenLineNo", parentLineNo), _
                                            New SqlClient.SqlParameter("@itemType", ItemType)}
        Dim tempStr1 As String = ">="
        If InsertOrRemove = CartFixer.eInsertOrRemove.Remove Then
            tempStr1 = ">"
            s = -s
        End If
        Dim str As String = String.Format("select {0} from {1} where {3}=@key and {2}=@parenLineNo and {5}=@itemType and {0}{4}@lineNo order by {0} asc", Me.cartStruc.lineNo.Name, Me.tbSource.tbName, Me.cartStruc.parentLineNo.Name, Me.cartStruc.key.Name, tempStr1, Me.cartStruc.itemType.Name)
        Dim r As SqlClient.SqlDataReader = sqlhelper.ExecuteReader(Me.tbSource.conn, CommandType.Text, str, p)

        Dim upStr() As String = Nothing
        Dim i As Integer = 0
        While r.Read
            Dim temp As Integer = r.Item(Me.cartStruc.lineNo.Name)
            If CartFixer.getParentLineNoFromLineNo(temp + s) <> parentLineNo Then
                expCode = expCode Or COMM.Msg.eErrCode.UpdateLineNoNewLineNoCannotMapToHigherLevel
                Exit Sub
            End If
            ReDim Preserve upStr(i + 1)

            If CartFixer.isValidParentLineNo(temp) Then
                upStr(i) = String.Format("update {0} set {1}={1} + {3} where {2}={5} and {4}=@key;update {0} set {2}={2} + {3} where {2}={5} and {4}=@key;", Me.tbSource.tbName, _
                                         Me.cartStruc.lineNo.Name, Me.cartStruc.parentLineNo.Name, s, Me.cartStruc.key.Name, temp)
            End If
            upStr(i) &= String.Format("update {0} set {1}={1}+ {3} where {1}={4} and {2}=@key;", Me.tbSource.tbName, Me.cartStruc.lineNo.Name, Me.cartStruc.key.Name, _
                                    s, temp)
            i = i + 1
        End While
        r.Close()
        If Not IsNothing(upStr) AndAlso upStr.Length > 0 Then
            Dim sqlstr As String = ""
            Dim j As Integer = upStr.Length
            Dim q As Integer = 1
            Dim stp As Integer = -1
            If InsertOrRemove = CartFixer.eInsertOrRemove.Remove Then
                j = 1
                q = upStr.Length
                stp = 1
            End If
            For N As Integer = j To q Step stp
                sqlstr &= upStr(N - 1)
            Next
            If sqlstr <> "" Then
                Dim p1() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key)}
                sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, sqlstr, p1)
            End If
        End If
    End Sub

    Private Sub ClearB()
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key)}
        Dim str As String = String.Format("delete from {0} where {1}=@key", Me.tbSource.tbName, Me.cartStruc.key.Name)
        sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
    End Sub

    Private Sub RemoveAtB(ByVal LineNo As Integer)
        Dim o As oCartList = Me.dbReadLine(LineNo)
        If Not IsNothing(o) AndAlso o.Count > 0 Then
            Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key), _
                                            New SqlClient.SqlParameter("@LineNo", LineNo)}
            Dim str As String = String.Format("delete from {0} where {1}=@key and {2}=@LineNo", Me.tbSource.tbName, Me.cartStruc.key.Name, Me.cartStruc.parentLineNo.Name)
            Dim str1 As String = String.Format("delete from {0} where {1}=@key and {2}=@LineNo", Me.tbSource.tbName, Me.cartStruc.key.Name, Me.cartStruc.lineNo.Name)
            sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str & ";" & str1, p)
            Dim expCode As COMM.Msg.eErrCode = 0
            updateLineInsertOrRemove(LineNo, o(0).parentLineNo.Value, o(0).itemType.Value, CartFixer.eInsertOrRemove.Remove, expCode)
            If expCode <> 0 Then
                Me._errCode = Me._errCode Or expCode
            End If
        End If
    End Sub
    Private Sub RemoveAtBWithEW(ByVal LineNo As SortedSet(Of Integer), ByVal oType As COMM.Fixer.eDocType)
        Dim c1 As Integer = 0, c2 As Integer = 0, c3 As Integer = 0
        Dim LineNoR As IEnumerable(Of Integer) = LineNo.Reverse
        For Each i As Integer In LineNoR
            Dim pl As Integer = CartFixer.getParentLineNoFromLineNo(i)
            Dim itemTpye As CartFixer.eItemType = CartFixer.eItemType.Others
            If CartFixer.isValidParentLineNo(i) Then itemTpye = CartFixer.eItemType.Parent
            Dim newi As Integer = i
            If itemTpye = CartFixer.eItemType.Parent Then
                newi = i - c1 * CartFixer.getStepByParent(pl, itemTpye)
            Else
                If pl = CartFixer.StartLine Then
                    newi = i - c2 * CartFixer.getStepByParent(pl, itemTpye)
                Else
                    newi = i - c3 * CartFixer.getStepByParent(pl, itemTpye)
                End If
            End If
            Dim ewn As Integer = 0
            ewn = RemoveEWFirstBeforeRemoveItem(newi)
            RemoveAtB(newi)
            If pl > CartFixer.StartLine Then
                updateEW(pl, COMM.Fixer.eDocReg.DefaultReg, oType)
            End If
            If itemTpye = CartFixer.eItemType.Parent Then
                c1 += 1
            Else
                If pl = CartFixer.StartLine Then
                    c2 += 1
                    c2 += ewn
                Else
                    c3 += 1
                End If
            End If
        Next
    End Sub

    Private Function getCartCount() As Integer
        Dim n As New Object
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key)}
        n = sqlhelper.ExecuteScalar(Me.tbSource.conn, CommandType.Text, String.Format("select count({0}) from {1} where {0}=@key", Me.cartStruc.key.Name, Me.tbSource.tbName), p)
        If IsNumeric(n) Then
            Return n
        End If
        Return 0
    End Function


    Private Function RemoveEWFirstBeforeRemoveItem(ByVal lineNo As Integer) As Integer
        Dim brothersN As Integer = 0
        Dim oew As oCartList = dbReadLine(lineNo, isParent:=True, isEW:=True)
        If Not IsNothing(oew) AndAlso oew.Count > 0 Then
            Me.RemoveAtB(oew(0).lineNo.Value)
            Return brothersN
        End If
        Dim oew1 As oCartList = dbReadLine(lineNo, isEW:=True)
        If Not IsNothing(oew1) AndAlso oew1.Count > 0 Then
            Me.RemoveAtB(oew1(0).lineNo.Value)
            brothersN = brothersN + 1
        End If
        Return brothersN
    End Function

    Public Function isPartialAble() As Boolean Implements IBUS.iCart(Of IBUS.iCartLine).isPartialAble
        Return True
    End Function

    Public Function CopyPaste(ByVal NewCart As iCart(Of iCartLine), ByVal oType As COMM.Fixer.eDocType) As iCartList Implements IBUS.iCart(Of IBUS.iCartLine).CopyPaste
        If NewCart.Key <> Me.key Then
            Dim dth As oCartList = Me.GetListAll(Fixer.eDocType.EQ)
            If Not IsNothing(dth) Then
                NewCart.Clear(oType)
                Dim o As oCartList = getNewLineList()

                For Each r As iCartLine In dth
                    r.key.Value = NewCart.Key
                    NewCart.Add(r, oType)
                    o.Add(r)
                Next
                Return o
            End If
        End If
        Return Nothing
    End Function

    Public ReadOnly Property Item1 As IBUS.iCartLine Implements IBUS.iCart(Of IBUS.iCartLine).Item1
        Get
            Return Me.cartStruc
        End Get
    End Property


End Class



Public MustInherit Class oCartLine
    Implements iCartLine


    Sub New(ByVal key As String, ByVal lineNo As Integer, ByVal parentLineNo As Integer,
            ByVal partNo As String, ByVal partDesc As String, ByVal listPrice As Decimal, ByVal newUnitPrice As Decimal, ByVal newCost As Decimal, _
            ByVal Qty As Integer, ByVal EWFlag As Integer, ByVal divPlant As String, Optional ByVal reqDate As DateTime = Nothing, Optional ByVal ItemType As CartFixer.eItemType = CartFixer.eItemType.Others, Optional ByVal category As String = "", Optional ByVal VirtualPartNo As String = "", Optional RecyclingFee As Decimal = 0)
        setFieldName()
        If partNo <> "" Then
            Me._partNo.Value = partNo
            Me._partDesc.Value = partDesc
            Me._divPlant.Value = divPlant
        Else
            Throw New Exception(COMM.EnumHelper.getDescription(COMM.Msg.eErrCode.InvalidPartNo) & ":" & partNo)
        End If
        If Not CartFixer.isValidItemType(ItemType) Then
            Throw New Exception(COMM.EnumHelper.getDescription(COMM.Msg.eErrCode.ItemTypeIncorrect) & ":" & ItemType.ToString)
        End If
        If ItemType = CartFixer.eItemType.Parent And (Not CartFixer.isParentUndStartLine(ItemType, parentLineNo)) Then
            Throw New Exception(COMM.EnumHelper.getDescription(COMM.Msg.eErrCode.HigherLevelMustUnderStartLine) & ":" & parentLineNo.ToString)
        End If
        '================
        Me._key.Value = key
        Me._parentLineNo.Value = parentLineNo
        Me._lineNo.Value = lineNo

        If listPrice > 0 Then
            Me._listPrice.Value = listPrice
        End If

        If newUnitPrice > 0 Then
            Me._unitPrice.Value = newUnitPrice
            Me._newUnitPrice.Value = newUnitPrice
        End If

        If newCost > 0 Then
            Me._cost.Value = newCost
            Me._newCost.Value = newCost
        End If
        If Qty > 0 Then
            Me._qty.Value = Qty
        Else
            Me._qty.Value = 1
        End If
        Me._inventory.Value = 0
        Me._canBeConfirmed.Value = True
        Dim tempReqD As Date
        If Date.TryParse(reqDate, tempReqD) Then
            Me._reqDate.Value = tempReqD
        End If
        Me._itemType.Value = ItemType
        Me._dueDate.Value = Now.Date.ToShortDateString
        Me._ewFlag.Value = EWFlag
        Me._category.Value = category
        Me._isValidLine = True
        If Not String.IsNullOrEmpty(VirtualPartNo) Then
            Me._VirtualPartNo.Value = VirtualPartNo
        End If
        If Not String.IsNullOrEmpty(RecyclingFee) Then
            Me._RecyclingFee.Value = RecyclingFee
        End If
    End Sub
    Sub New(ByVal key As String, ByVal lineNo As Integer, ByVal parentLineNo As Integer,
            ByVal p As oProd, ByVal newUnitPrice As Decimal, ByVal listPrice As Decimal, ByVal newCost As Decimal, _
            ByVal Qty As Integer, ByVal EWFlag As Integer, Optional ByVal reqDate As DateTime = Nothing, Optional ByVal ItemType As CartFixer.eItemType = CartFixer.eItemType.Others, Optional ByVal category As String = "")

        '================
        setFieldName()
        If IsNothing(p) OrElse (ItemType <> CartFixer.eItemType.Parent AndAlso p.prodValid = False) Then
            Throw New Exception(COMM.EnumHelper.getDescription(COMM.Msg.eErrCode.InvalidPartNo) & ":" & partNo.ToString)
        Else
            Me._partNo.Value = p.partNo
            Me._partDesc.Value = p.partDesc
            Me._divPlant.Value = p.divPlant
        End If
        If Not CartFixer.isValidItemType(ItemType) Then
            Throw New Exception(COMM.EnumHelper.getDescription(COMM.Msg.eErrCode.ItemTypeIncorrect) & ":" & ItemType.ToString)

        End If
        If ItemType = CartFixer.eItemType.Parent And (Not CartFixer.isParentUndStartLine(ItemType, parentLineNo)) Then
            Throw New Exception(COMM.EnumHelper.getDescription(COMM.Msg.eErrCode.HigherLevelMustUnderStartLine) & ":" & parentLineNo.ToString)
        End If
        '================
        Me._key.Value = key
        Me._parentLineNo.Value = parentLineNo
        Me._lineNo.Value = lineNo

        If listPrice > 0 Then
            Me._listPrice.Value = listPrice
        End If

        If newUnitPrice > 0 Then
            Me._unitPrice.Value = newUnitPrice
            Me._newUnitPrice.Value = newUnitPrice
        End If

        If newCost > 0 Then
            Me._cost.Value = newCost
            Me._newCost.Value = newCost
        End If
        If Qty > 0 Then
            Me._qty.Value = Qty
        Else
            Me._qty.Value = 1
        End If
        Me._inventory.Value = 0
        Me._canBeConfirmed.Value = True
        Dim tempReqDate As Date
        If Date.TryParse(reqDate, tempReqDate) Then
            Me._reqDate.Value = tempReqDate
        End If
        Me._itemType.Value = ItemType
        Me._dueDate.Value = Now.Date.ToShortDateString
        Me._ewFlag.Value = EWFlag
        Me._category.Value = category
        Me._isValidLine = True
    End Sub
    Sub New()
        setFieldName()
    End Sub

    Private _isValidLine As Boolean = False
    Public ReadOnly Property isValidLine As Boolean Implements IBUS.iCartLine.isValidLine
        Get
            Return _isValidLine
        End Get
    End Property

    Protected MustOverride Sub setFieldName()

    Protected _key As New COMM.Field("", "")
    Public Property key As COMM.Field Implements iCartLine.key
        Get
            Return _key
        End Get
        Set(ByVal value As COMM.Field)
            _key.Value = value.Value
        End Set
    End Property
    Protected _parentLineNo As New COMM.Field("", CartFixer.StartLine)
    Public Property parentLineNo As COMM.Field Implements iCartLine.parentLineNo
        Get
            Return _parentLineNo
        End Get
        Set(ByVal value As COMM.Field)
            _parentLineNo.Value = value.Value
        End Set
    End Property
    Protected _lineNo As New COMM.Field("", CartFixer.StartLine)
    Public Property lineNo As COMM.Field Implements iCartLine.lineNo
        Get
            Return _lineNo
        End Get
        Set(ByVal value As COMM.Field)
            _lineNo.Value = value.Value
        End Set
    End Property
    Protected _partNo As New COMM.Field("", "")
    Public Property partNo As COMM.Field Implements iCartLine.partNo
        Get
            Return _partNo
        End Get
        Set(ByVal value As COMM.Field)
            _partNo.Value = value.Value
        End Set
    End Property
    Protected _partDesc As New COMM.Field("", "")
    Public Property partDesc As COMM.Field Implements iCartLine.partDesc
        Get
            Return _partDesc
        End Get
        Set(ByVal value As COMM.Field)
            _partDesc.Value = value.Value
        End Set
    End Property
    Protected _listPrice As New COMM.Field("", 0)
    Public Property listPrice As COMM.Field Implements iCartLine.listPrice
        Get
            Return _listPrice
        End Get
        Set(ByVal value As COMM.Field)
            _listPrice.Value = value.Value
        End Set
    End Property
    Protected _unitPrice As New COMM.Field("", 0)
    Public Property unitPrice As COMM.Field Implements iCartLine.unitPrice
        Get
            Return _unitPrice
        End Get
        Set(ByVal value As COMM.Field)
            _unitPrice.Value = value.Value
        End Set
    End Property
    Protected _newUnitPrice As New COMM.Field("", 0)
    Public Property newunitPrice As COMM.Field Implements iCartLine.newunitPrice
        Get
            Return _newUnitPrice
        End Get
        Set(ByVal value As COMM.Field)
            _newUnitPrice.Value = value.Value
        End Set
    End Property
    Protected _cost As New COMM.Field("", 0)
    Public Property cost As COMM.Field Implements iCartLine.cost
        Get
            Return _cost
        End Get
        Set(ByVal value As COMM.Field)
            _cost.Value = value.Value
        End Set
    End Property
    Protected _newCost As New COMM.Field("", 0)
    Public Property newCost As COMM.Field Implements iCartLine.newCost
        Get
            Return _newCost
        End Get
        Set(ByVal value As COMM.Field)
            _newCost.Value = value.Value
        End Set
    End Property
    Protected _qty As New COMM.Field("", 0)
    Public Property Qty As COMM.Field Implements iCartLine.Qty
        Get
            Return _qty
        End Get
        Set(ByVal value As COMM.Field)
            _qty.Value = value.Value
        End Set
    End Property
    Protected _itemType As New COMM.Field("", CInt(CartFixer.eItemType.Others))
    Public Property itemType As COMM.Field Implements iCartLine.itemType
        Get
            Return _itemType
        End Get
        Set(ByVal value As COMM.Field)
            _itemType.Value = value.Value
        End Set
    End Property
    Protected _ewFlag As New COMM.Field("", 0)
    Public Property ewFlag As COMM.Field Implements iCartLine.ewFlag
        Get
            Return _ewFlag
        End Get
        Set(ByVal value As COMM.Field)
            _ewFlag.Value = value.Value
        End Set
    End Property
    Protected _inventory As New COMM.Field("", 0)
    Public Property inventory As COMM.Field Implements iCartLine.inventory
        Get
            Return _inventory
        End Get
        Set(ByVal value As COMM.Field)
            _inventory.Value = value.Value
        End Set
    End Property
    Protected _reqDate As New COMM.Field("", Now.Date.ToShortDateString)
    Public Property reqDate As COMM.Field Implements iCartLine.reqDate
        Get
            Return _reqDate
        End Get
        Set(ByVal value As COMM.Field)
            _reqDate.Value = value.Value
        End Set
    End Property
    Protected _dueDate As New COMM.Field("", Now.Date.ToShortDateString)
    Public Property dueDate As COMM.Field Implements iCartLine.dueDate
        Get
            Return _dueDate
        End Get
        Set(ByVal value As COMM.Field)
            _dueDate.Value = value.Value
        End Set
    End Property
    Protected _canBeConfirmed As New COMM.Field("", 0)
    Public Property canBeConfirmed As COMM.Field Implements iCartLine.canBeConfirmed
        Get
            Return _canBeConfirmed
        End Get
        Set(ByVal value As COMM.Field)
            _canBeConfirmed.Value = value.Value
        End Set
    End Property
    Protected _category As New COMM.Field("", "")
    Public Property category As COMM.Field Implements iCartLine.category
        Get
            Return _category
        End Get
        Set(ByVal value As COMM.Field)
            _category.Value = value.Value
        End Set
    End Property
    Protected _divPlant As New COMM.Field("", "")
    Public Property divPlant As COMM.Field Implements iCartLine.divPlant
        Get
            Return _divPlant
        End Get
        Set(ByVal value As COMM.Field)
            _divPlant.Value = value.Value
        End Set
    End Property
    Protected _CustMaterial As New COMM.Field("", "")
    Public Property CustMaterial As COMM.Field Implements IBUS.iCartLine.CustMaterial
        Get
            Return _CustMaterial
        End Get
        Set(ByVal value As COMM.Field)
            _CustMaterial = value
        End Set
    End Property
    Protected _RHOS As New COMM.Field("", 0)
    Public Property RHOS As COMM.Field Implements IBUS.iCartLine.RHOS
        Get
            Return _RHOS
        End Get
        Set(ByVal value As COMM.Field)
            _RHOS = value
        End Set
    End Property
    Protected _satisfyflag As New COMM.Field("", 0)
    Public Property satisfyflag As COMM.Field Implements IBUS.iCartLine.satisfyflag
        Get
            Return _satisfyflag
        End Get
        Set(ByVal value As COMM.Field)
            _satisfyflag = value
        End Set
    End Property
    Protected _DMFFlag As New COMM.Field("", "")
    Public Property DMFFlag As COMM.Field Implements IBUS.iCartLine.DMFFlag
        Get
            Return _DMFFlag
        End Get
        Set(ByVal value As COMM.Field)
            _DMFFlag = value
        End Set
    End Property
    Protected _VirtualPartNo As New COMM.Field("", "")
    Public Property VirtualPartNo As COMM.Field Implements IBUS.iCartLine.VirtualPartNo
        Get
            Return _VirtualPartNo
        End Get
        Set(ByVal value As COMM.Field)
            _VirtualPartNo = value
        End Set
    End Property
    Protected _RecyclingFee As New COMM.Field("", 0)
    Public Property RecyclingFee As COMM.Field Implements IBUS.iCartLine.RecyclingFee
        Get
            Return _RecyclingFee
        End Get
        Set(ByVal value As COMM.Field)
            _RecyclingFee = value
        End Set
    End Property
    Protected _DELIVERYGROUP As New Field("", "")
    Public Property DELIVERYGROUP As COMM.Field Implements IBUS.iCartLine.DELIVERYGROUP
        Get
            Return _DELIVERYGROUP
        End Get
        Set(ByVal value As COMM.Field)
            _DELIVERYGROUP = value
        End Set
    End Property
    Protected _ShipPoint As New Field("", "")
    Public Property ShipPoint As COMM.Field Implements IBUS.iCartLine.ShipPoint
        Get
            Return _ShipPoint
        End Get
        Set(ByVal value As COMM.Field)
            _ShipPoint = value
        End Set
    End Property
    Protected _StorageLoc As New Field("", "")
    Public Property StorageLoc As COMM.Field Implements IBUS.iCartLine.StorageLoc
        Get
            Return _StorageLoc
        End Get
        Set(ByVal value As COMM.Field)
            _StorageLoc = value
        End Set
    End Property
    Protected _RECFIGID As New Field("", "")
    Public Property RECFIGID As COMM.Field Implements IBUS.iCartLine.RECFIGID
        Get
            Return _RECFIGID
        End Get
        Set(ByVal value As COMM.Field)
            _RECFIGID = value
        End Set
    End Property
End Class

Partial Public Class oCartList : Implements IBUS.iCartList

    Private _errCode As COMM.Msg.eErrCode
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property
    Private _contents() As iCartLine = Nothing
    Private _count As Integer = 0
    Public Sub New()
        Clear()
    End Sub
    Public ReadOnly Property Count As Integer Implements IBUS.iCartList.Count
        Get
            Return _count
        End Get
    End Property


    Public Sub Add(ByVal item As IBUS.iCartLine) Implements IBUS.iCartList.Add
        ReDim Preserve _contents(_count)
        _contents(_count) = item
        _count += 1
    End Sub

    Public Sub Clear() Implements IBUS.iCartList.Clear
        _contents = Nothing
        _count = 0
    End Sub

    Public Function Contains(ByVal LineNo As Integer) As Boolean Implements IBUS.iCartList.Contains
        Dim inList As Boolean = False
        Dim i As Integer
        For i = 0 To _count - 1
            If _contents(i).lineNo.Value = LineNo Then
                inList = True
                Exit For
            End If
        Next i
        Return inList
    End Function

    Public Function IndexOf(ByVal LineNo As Integer) As Integer Implements IBUS.iCartList.IndexOf
        Dim i As Integer
        For i = 0 To _count - 1
            If _contents(i).lineNo.Value = LineNo Then
                Return i
            End If
        Next i
        Return -1
    End Function

    Public Property Item(ByVal LineNo As Integer) As iCartLine Implements IBUS.iCartList.Item
        Get
            Return _contents(LineNo)
        End Get
        Set(ByVal value As iCartLine)
            _contents(LineNo) = value
        End Set
    End Property
    <Obsolete("This method is obsoleted, use MyQuoteX<DeleteQuoteItem> instead.")> _
    Public Sub RemoveAt(ByVal LineNo As Integer) Implements IBUS.iCartList.RemoveAt
        Dim Ind As String = IndexOf(LineNo)
        If Ind <> -1 Then
            For i As Integer = Ind To (_count - 1)
                _contents(i) = _contents(i + 1)
            Next
            _count -= 1
        End If
    End Sub

    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of IBUS.iCartLine) Implements System.Collections.Generic.IEnumerable(Of IBUS.iCartLine).GetEnumerator
        Return New CartLineEnum(_contents)
    End Function

    Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return CType(Me._contents, IEnumerable).GetEnumerator()
    End Function

    Public Function lineNoOf(ByVal index As Integer) As Integer Implements IBUS.iCartList.lineNoOf
        Dim L As Integer = -1
        If Not IsNothing(_contents(index)) Then
            L = _contents(index).lineNo.Value
        End If
        Return L
    End Function

  
End Class
Partial Public Class oCartList
    Function getWarrantableTotalAmount(ByVal DocReg As COMM.Fixer.eDocReg) As Decimal Implements IBUS.iCartList.getWarrantableTotalAmount
        Dim rlt As Decimal = 0
        For Each r As IBUS.iCartLine In Me
            If r.itemType.Value = COMM.Fixer.eItemType.Others AndAlso cartBiz.isWarrantableV2(r.partNo.Value, DocReg) Then
                rlt = rlt + (r.newunitPrice.Value * r.Qty.Value)
            End If
        Next
        Return rlt
    End Function
    Function getTotalAmount() As Decimal Implements IBUS.iCartList.getTotalAmount
        Dim rlt As Decimal = 0
        For Each r As IBUS.iCartLine In Me
            If r.itemType.Value = COMM.Fixer.eItemType.Others Then
                rlt = rlt + (r.newunitPrice.Value * r.Qty.Value)
            End If
        Next
        Return rlt
    End Function
    Function getTotalRecyclingFee() As Decimal Implements IBUS.iCartList.getTotalRecyclingFee
        Dim rlt As Decimal = 0
        For Each r As IBUS.iCartLine In Me
            If r.itemType.Value = COMM.Fixer.eItemType.Others Then
                rlt = rlt + (r.RecyclingFee.Value * r.Qty.Value)
            End If
        Next
        Return rlt
    End Function
    Function getListPriceTotalAmount() As Decimal Implements IBUS.iCartList.getListPriceTotalAmount
        Dim rlt As Decimal = 0
        For Each r As IBUS.iCartLine In Me
            If r.itemType.Value = COMM.Fixer.eItemType.Others Then
                rlt = rlt + (r.listPrice.Value * r.Qty.Value)
            End If
        Next
        Return rlt
    End Function

    Public Function GetTaxableAmount(ByVal ShipTo As String) As Decimal Implements IBUS.iCartList.GetTaxableAmount
        Dim amount As Decimal = 0
        For Each r As IBUS.iCartLine In Me
            If r.itemType.Value = COMM.Fixer.eItemType.Others AndAlso cartBiz.isTaxable(r.partNo.Value, ShipTo) Then
                amount += r.newunitPrice.Value * r.Qty.Value
            End If
        Next
        Return amount
    End Function

    Public Function getTotalListAmount() As Decimal Implements IBUS.iCartList.getTotalListAmount
        Dim rlt As Decimal = 0
        For Each r As IBUS.iCartLine In Me
            If r.itemType.Value = COMM.Fixer.eItemType.Others Then
                rlt = rlt + (r.listPrice.Value * r.Qty.Value)
            End If
        Next
        Return rlt
    End Function

    Public Function Merge2NewCartList(ByVal MyEnumerable As System.Collections.Generic.IEnumerable(Of IBUS.iCartLine)) As IBUS.iCartList Implements IBUS.iCartList.Merge2NewCartList
        If Not IsNothing(MyEnumerable) Then
            Dim o As New oCartList
            For Each r As IBUS.iCartLine In MyEnumerable
                o.Add(r)
            Next
            Return o
        End If
        Return Nothing
    End Function

    Public Function ItemValidate(ByVal Type As COMM.Fixer.eCartItemValidateType, ByVal R As iRole) As IBUS.iCartList Implements IBUS.iCartList.ItemValidate
        If Type = Fixer.eCartItemValidateType.IsOrderable Then
            Dim o As New oCartList, Instr As String = String.Empty
            For Each item As IBUS.iCartLine In Me
                Instr += "'" + item.partNo.Value + "',"
            Next
            Instr = Instr.TrimEnd(New Char() {","})
            Dim dt As DataTable = dbUtil.dbGetDataTable("MY", String.Format("SELECT part_no ,org_id  from dbo.SAP_PRODUCT_ORG where org_id='{0}' AND part_no in ({1})", R.getCurrOrg, Instr))
            For Each item As IBUS.iCartLine In Me
                If dt.Select(String.Format("part_no='{0}'", item.partNo.Value.ToString.Trim)).Length = 0 Then
                    item.partDesc.Value = String.Format("{0} is invalid for {1} ", item.partNo.Value, R.getCurrOrg)
                    o.Add(item)
                End If
            Next
            Return o
        End If
        Return Nothing
    End Function
End Class

Public Class CartLineEnum
    Implements IEnumerator(Of iCartLine)

    Public _objList() As iCartLine
    Private position As Integer = -1

    Public Sub New(ByVal list() As iCartLine)
        _objList = list
    End Sub

    Public ReadOnly Property Current As IBUS.iCartLine Implements System.Collections.Generic.IEnumerator(Of IBUS.iCartLine).Current
        Get
            Try
                Return _objList(position)
            Catch ex As IndexOutOfRangeException
                Throw New InvalidOperationException()
            End Try
        End Get
    End Property

    Public ReadOnly Property Current1 As Object Implements System.Collections.IEnumerator.Current
        Get
            Return Me.Current
        End Get
    End Property

    Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
        Dim _objListCount As Integer = 0
        If Not IsNothing(_objList) AndAlso _objList.Length > 0 Then
            _objListCount = _objList.Length
        End If
        position = position + 1
        'Return (position < _objList.Length)
        Return (position < _objListCount)
    End Function

    Public Sub Reset() Implements System.Collections.IEnumerator.Reset
        position = -1
    End Sub

    Private disposedValue As Boolean
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If Not IsNothing(_objList) Then
                    _objList = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


End Class

Public Class cartBiz
    Shared Function getCountryCodebyShipTo(ByVal ShipToId As String) As String
        Dim str As String = String.Format("SELECT LAND1 FROM SAPRDP.KNA1 WHERE KUNNR='{0}' and rownum=1", ShipToId.ToUpper)
        Dim CID As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", str)
        If Not IsNothing(CID) AndAlso CID.ToString <> "" Then
            Return CID.ToString
        End If
        Return "TW"
    End Function
    Public Shared Function isTaxable(ByVal PartNo As String, ByVal ShipToId As String) As Boolean

        Dim ConCode As String = getCountryCodebyShipTo(ShipToId)
        Dim str As String = String.Format("select count(Part_No) FROM SAP_PRODUCT_TAXEXEMPT where Part_NO='{0}' AND Country_Code='{1}'", PartNo.ToUpper, ConCode.ToUpper)
        Dim O As New Object
        O = dbUtil.dbExecuteScalar("MY", str)
        If IsNumeric(O) AndAlso CInt(O) > 0 Then
            Return False
        End If
        Return True
    End Function
    Shared Function isWarrantable(ByVal PN As String, ByVal docReg As COMM.Fixer.eDocReg) As [Boolean]
        If (docReg And Fixer.eDocReg.AUS) <> docReg Then
            If docReg = Fixer.eDocReg.AJP Then
                If isSoftware(PN) Then
                    Return False
                ElseIf PN.StartsWith("AGS") OrElse PN.StartsWith("XAJP") Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return True
            End If
        End If
        Dim DT As New DataTable
        DT = dbac.getProductByLine(PN)
        If DT.Rows.Count = 0 Then Return True
        If isSoftware(PN) Then
            Return False
        ElseIf PN.StartsWith("AGS") OrElse PN.StartsWith("OPTION") Then
            Return False
        ElseIf DT.Rows(0).Item("PRODUCT_LINE") IsNot Nothing AndAlso DT.Rows(0).Item("PRODUCT_LINE") = "ASS#" Then
            Return False
        Else
            Return True
        End If
    End Function

    Shared Function isWarrantableV2(ByVal PN As String, ByVal docReg As COMM.Fixer.eDocReg) As [Boolean]

        If docReg AndAlso docReg = Fixer.eDocReg.AJP Then
            If SAPDAL.CommonLogic.isSoftwareV3(PN) Then
                Return False
            ElseIf PN.ToUpper.StartsWith("XAJP") Then
                Return False
            Else
                Return True
            End If
        ElseIf docReg AndAlso docReg = Fixer.eDocReg.AEU Then
            Return True
        Else
            If SAPDAL.CommonLogic.isSoftwareV3(PN) Then
                Return False
            Else
                Return True
            End If
        End If

    End Function

    Shared Function isSoftware(ByVal PN As String) As [Boolean]
        Dim DT As New DataTable
        DT = dbac.getProductByLine(PN)
        If DT.Rows.Count = 0 Then Return True
        If PN.StartsWith("96SW") OrElse PN.StartsWith("98MQ") OrElse PN.StartsWith("968Q") OrElse PN.StartsWith("968T") Then
            Return True
        ElseIf DT.Rows(0).Item("PRODUCT_LINE") IsNot Nothing AndAlso (DT.Rows(0).Item("PRODUCT_LINE") = "EPCS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "EDOS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WCOM" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WAUT" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "DAAS") Then
            Return True
        Else
            Return False
        End If
    End Function

   
End Class
Partial Public MustInherit Class oCart : Implements IBUS.iCart(Of IBUS.iCartLine)
    Public Sub New(ByVal pTbSource As tbSource, ByVal key As String, ByVal org As String, ByVal pCartStruc As IBUS.iCartLine)
        _count = 0
        Me._tbSource = pTbSource
        Me._key = key
        Me._org = org
        Me._cartStruc = pCartStruc
    End Sub
    Private _errCode As COMM.Msg.eErrCode = COMM.Msg.eErrCode.OK
    Public ReadOnly Property errCode As COMM.Msg.eErrCode Implements IBUS.iBase.errCode
        Get
            Return _errCode
        End Get
    End Property
    Private _cartStruc As IBUS.iCartLine
    Public ReadOnly Property cartStruc As IBUS.iCartLine
        Get
            Return _cartStruc
        End Get
    End Property
    Private _tbSource As tbSource
    Public ReadOnly Property tbSource As tbSource
        Get
            Return _tbSource
        End Get
    End Property
    Private _key As String = ""
    Public ReadOnly Property key As String Implements iCart(Of IBUS.iCartLine).Key
        Get
            Return _key
        End Get
    End Property
    Private _org As String = ""
    Public ReadOnly Property org As String
        Get
            Return _org
        End Get
    End Property

    Private _count As Integer = 0
    Public ReadOnly Property Count As Integer
        Get
            Count = getCartCount()
            Return Count
        End Get
    End Property

    Private Function getPropertyDic(ByVal o As Object, ByRef expCode As Integer) As SortedDictionary(Of String, COMM.Field)
        Dim ret As New SortedDictionary(Of String, COMM.Field)
        Dim t As Type = o.GetType
        For Each r As System.Reflection.PropertyInfo In t.GetProperties()
            If r.PropertyType = GetType(COMM.Field) Then
                Dim otemp As COMM.Field = CType(r.GetValue(o, Nothing), COMM.Field)
                If Not IsNothing(otemp) AndAlso otemp.Name <> "" Then
                    If Not ret.ContainsKey(otemp.Name) Then
                        ret.Add(otemp.Name, otemp)
                    Else
                        expCode = expCode Or COMM.Msg.eErrCode.FieldNameDefinitionDuplicated
                        Return Nothing
                    End If
                End If
            End If
        Next
        Return ret
    End Function
    Private Sub dbInsert(ByVal item As IBUS.iCartLine, ByVal oType As COMM.Fixer.eDocType)
        If oType = Fixer.eDocType.ORDER Then
            Me.tbSource.tbName = "OrderDetail"
        End If
        Dim expCode As Integer = 0
        Dim fieldDic As SortedDictionary(Of String, COMM.Field) = getPropertyDic(item, expCode)
        If expCode <> 0 Then
            Me._errCode = Me._errCode Or errCode
            Exit Sub
        End If
        If Not IsNothing(fieldDic) AndAlso fieldDic.Count > 0 Then
            Dim p() As SqlClient.SqlParameter = Nothing
            Dim strPat As String = ""
            Dim strVal As String = ""
            Dim n As Integer = 0
            For Each r As KeyValuePair(Of String, COMM.Field) In fieldDic
                ReDim Preserve p(n)
                Dim paraName As String = "@" & r.Key
                p(n) = New SqlClient.SqlParameter(paraName, r.Value.Value)
                strPat &= r.Key & ","
                strVal &= paraName & ","
                n += 1
            Next
            strPat = strPat.Trim(",")
            strVal = strVal.Trim(",")
            Dim str As String = String.Format("insert into {0} ({1}) values ({2})", Me.tbSource.tbName, strPat, strVal)
            sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
        End If
    End Sub
    Private Function getListStr(ByVal LineNo As Integer, Optional ByVal isParent As Boolean = False, Optional ByVal isEW As Boolean = False) As String
        Dim item As IBUS.iCartLine = Me.cartStruc
        Dim str1 As String = String.Format(" and {0}=@{0} ", Me.cartStruc.lineNo.Name)
        If LineNo = CartFixer.StartLine Then
            str1 = ""
        End If

        If isParent Then
            str1 = String.Format(" and {0}=@{1} ", Me.cartStruc.parentLineNo.Name, Me.cartStruc.lineNo.Name)
            If isEW Then
                str1 = str1 & String.Format(" and {0} like '{1}%' ", Me.cartStruc.partNo.Name, CartFixer.AGSEWPrefix)
            End If
        Else
            If isEW Then
                Dim stp As Integer = CartFixer.getStepByParent(CartFixer.StartLine, CartFixer.eItemType.Others)
                str1 = String.Format(" and {0}=@{0} +{1}  and {2} like '{3}%' ", Me.cartStruc.lineNo.Name, stp, Me.cartStruc.partNo.Name, CartFixer.AGSEWPrefix)
            End If
        End If
        Dim expCode As Integer = 0
        Dim fieldDic As SortedDictionary(Of String, COMM.Field) = getPropertyDic(item, expCode)
        If expCode <> 0 Then
            Me._errCode = Me._errCode Or errCode
            Return ""
        End If
        Dim sqlstr As String = ""
        Dim pat As String = ""
        If Not IsNothing(fieldDic) AndAlso fieldDic.Count > 0 Then
            For Each r As KeyValuePair(Of String, COMM.Field) In fieldDic
                Dim A As String = r.Value.defaultValue.ToString
                A = "'" & A & "'"
                pat &= String.Format("ISNULL({0},{1}) AS {0},", r.Key, A)
            Next
            sqlstr &= String.Format("select {4} from {0} where {2}=@{2} {1} order by {3} ", Me.tbSource.tbName, str1, Me.cartStruc.key.Name, Me.cartStruc.lineNo.Name, pat.Trim(","))
        End If
        Return sqlstr
    End Function

    Private Function dbReadLine(ByVal lineNo As Integer, Optional ByVal isParent As Boolean = False, Optional ByVal isEW As Boolean = False) As oCartList

        Dim oList As oCartList = getNewLineList()
        Dim dt As DataTable = dbReadLineDT(lineNo, isParent, isEW)
        If dt.Rows.Count > 0 Then
            For Each r As DataRow In dt.Rows

                Dim o As oCartLine = getItemByDataRow(r)
                oList.Add(o)
            Next
            Return oList
        End If
        Return Nothing
    End Function

    Private Function dbReadLineDT(ByVal lineNo As Integer, Optional ByVal isParent As Boolean = False, Optional ByVal isEW As Boolean = False) As DataTable
        Dim dt As DataTable
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@" & Me.cartStruc.key.Name, Me.key), _
                                             New SqlClient.SqlParameter("@" & Me.cartStruc.lineNo.Name, lineNo)}
        dt = sqlhelper.getDT(Me.tbSource.conn, CommandType.Text, getListStr(lineNo, isParent, isEW), p)

        'Frank 2013/08/22: Upgrade V2.0 quotation detail data to V2.5
        Dim QM As IBUS.iDocHeader = New DOCH.DocHeader
        Dim _IsV2_0Quot As Boolean = QM.IsV2_0Quotation(Me.key)
        If _IsV2_0Quot AndAlso dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            For Each _row As DataRow In dt.Rows
                If _row.Item("line_No") = 100 AndAlso _row.Item("ItemType") = 0 Then
                    _row.Item("ItemType") = 1 : Exit For
                End If
            Next
        End If

        Return dt
    End Function
    Protected MustOverride Function getItemByDataRow(ByVal r As DataRow) As oCartLine
    Protected MustOverride Function getEWItem(ByVal ewItem As EW) As IBUS.iCartLine
    Protected MustOverride Function getNewLineList() As IBUS.iCartList
    Public Function GetListAll(ByVal oType As COMM.Fixer.eDocType) As iCartList Implements iCart(Of IBUS.iCartLine).GetListAll
        If oType = COMM.Fixer.eDocType.ORDER Then
            Me.tbSource.tbName = "OrderDetail"
        End If
        Return Me.dbReadLine(CartFixer.StartLine)
    End Function
    Public Function UpdateByLine(ByVal setStr As String, ByVal LineNo As Integer, ByVal oType As COMM.Fixer.eDocType) As Integer Implements iCart(Of IBUS.iCartLine).UpdateByLine
        If oType = Fixer.eDocType.ORDER Then Me.tbSource.tbName = "OrderDetail"
        Dim str As String = String.Format("update " & Me.tbSource.tbName & " set " & setStr & " where {0}=@key and {1}=@lineNo", Me.cartStruc.key.Name, Me.cartStruc.lineNo.Name)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key), _
                                          New SqlClient.SqlParameter("@LineNo", LineNo)}
        Dim n As Integer = sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
        Return n
    End Function
    Public Function UpdateByStr(ByVal setStr As String, ByVal whereStr As String, ByVal oType As COMM.Fixer.eDocType) As Integer Implements iCart(Of IBUS.iCartLine).UpdateByStr
        Dim str As String = String.Format("update " & Me.tbSource.tbName & " set {0} where {1}", setStr, whereStr)
        Dim n As Integer = sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, Nothing)
        Return n
    End Function
    Public Sub updateEW(ByVal lineNo As SortedSet(Of Integer), ByVal DocReg As COMM.Fixer.eDocReg, ByVal oType As COMM.Fixer.eDocType, Optional Currency As String = "") Implements IBUS.iCart(Of IBUS.iCartLine).updateEW
        Dim c1 As Integer = 0
        For Each i As Integer In lineNo
            Dim pl As Integer = CartFixer.getParentLineNoFromLineNo(i)
            Dim itemTpye As CartFixer.eItemType = CartFixer.eItemType.Others
            If CartFixer.isValidParentLineNo(i) Then itemTpye = CartFixer.eItemType.Parent
            If pl = CartFixer.StartLine Then
                Dim newi As Integer = i
                newi = i - c1 * CartFixer.getStepByParent(pl, itemTpye)
                Dim ewn As Integer = 0
                ewn = updateEW(newi, DocReg, oType, Currency)
                c1 += ewn
            End If
        Next
    End Sub
    Public Function updateEW(ByVal lineNo As Integer, ByVal DocReg As COMM.Fixer.eDocReg, ByVal oType As COMM.Fixer.eDocType, Optional Currency As String = "") As Integer
        Dim ewn As Integer = 0
        Dim op As oCartList = Me.dbReadLine(lineNo)
        If Not IsNothing(op) AndAlso op.Count > 0 AndAlso Not (op(0).partNo.ToString.Contains(CartFixer.AGSEWPrefix)) Then
            If CartFixer.isValidParentLineNo(lineNo) Then
                Dim oew As oCartList = dbReadLine(lineNo, isParent:=True, isEW:=True)
                If Not IsNothing(oew) AndAlso oew.Count > 0 Then
                    If op(0).ewFlag.Value = 0 Then
                        Me.RemoveAtB(oew(0).lineNo.Value)
                    Else
                        Me.RemoveAtB(oew(0).lineNo.Value)
                        Dim dt As oCartList = Nothing
                        dt = Me.dbReadLine(lineNo, True)
                        Dim ewPrice As Decimal = 0
                        Dim EWU As IBUS.iEWUtil = New EWUtil

                        ewPrice = dt.getWarrantableTotalAmount(DocReg) / op(0).Qty.Value * EWU.getRateByEWItem(EWU.getEWItemByMonth(op(0).ewFlag.Value, DocReg), DocReg)

                        If ewPrice <> 0 Then
                            ewPrice = Decimal.Round(ewPrice, 2)
                            'Frank 2014/03/18 TWD need to be rounded up only
                            If Currency.Equals("TWD", StringComparison.InvariantCultureIgnoreCase) Then ewPrice = Math.Ceiling(ewPrice)

                            Me.Add(getEWItem(New EW(lineNo, 0, EWU.getEWItemByMonth(op(0).ewFlag.Value, DocReg), Me.org, op(0).divPlant.Value, ewPrice, op(0).Qty.Value)), oType)
                        Else
                            Dim str As String = String.Format("update " & Me.tbSource.tbName & " set {2} where {0}=@key and ({1}=@lineNo or {3}=@lineNo)", Me.cartStruc.key.Name, Me.cartStruc.lineNo.Name, Me.cartStruc.ewFlag.Name & "=0", Me.cartStruc.parentLineNo.Name)
                            Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key), _
                                                              New SqlClient.SqlParameter("@LineNo", lineNo)}
                            Dim n As Integer = sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
                        End If
                    End If
                Else
                    If op(0).ewFlag.Value = 0 Then
                    Else
                        Dim dt As oCartList = Nothing
                        dt = Me.dbReadLine(lineNo, True)
                        Dim ewPrice As Decimal = 0
                        Dim EWU As IBUS.iEWUtil = New EWUtil
                        If Not IsNothing(dt) AndAlso dt.Count > 0 Then
                            ewPrice = dt.getWarrantableTotalAmount(DocReg) / op(0).Qty.Value * EWU.getRateByEWItem(EWU.getEWItemByMonth(op(0).ewFlag.Value, DocReg), DocReg)
                        End If

                        If ewPrice <> 0 Then
                            ewPrice = Decimal.Round(ewPrice, 2)
                            'Frank 2014/03/18 TWD need to be rounded up only
                            If Currency.Equals("TWD", StringComparison.InvariantCultureIgnoreCase) Then ewPrice = Math.Ceiling(ewPrice)

                            Me.Add(getEWItem(New EW(lineNo, 0, EWU.getEWItemByMonth(op(0).ewFlag.Value, DocReg), Me.org, op(0).divPlant.Value, ewPrice, op(0).Qty.Value)), oType)
                        Else
                            Dim str As String = String.Format("update " & Me.tbSource.tbName & " set {2} where {0}=@key and ({1}=@lineNo or {3}=@lineNo)", Me.cartStruc.key.Name, Me.cartStruc.lineNo.Name, Me.cartStruc.ewFlag.Name & "=0", Me.cartStruc.parentLineNo.Name)
                            Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key), _
                                                              New SqlClient.SqlParameter("@LineNo", lineNo)}
                            Dim n As Integer = sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
                        End If
                    End If
                End If
            Else
                If cartBiz.isWarrantable(op(0).partNo.Value, DocReg) Then
                    Dim oew As oCartList = dbReadLine(lineNo, isEW:=True)
                    If Not IsNothing(oew) AndAlso oew.Count > 0 Then
                        If op(0).ewFlag.Value = 0 Then
                            Me.RemoveAtB(oew(0).lineNo.Value)
                            ewn = -1
                        Else
                            Me.RemoveAtB(oew(0).lineNo.Value)
                            Dim EWU As IBUS.iEWUtil = New EWUtil

                            Dim ewPrice As Decimal = op(0).newunitPrice.Value * EWU.getRateByEWItem(EWU.getEWItemByMonth(op(0).ewFlag.Value, DocReg), DocReg)

                            If ewPrice > 0 Then
                                ewPrice = Decimal.Round(ewPrice, 2)
                                'Frank 2014/03/18 TWD need to be rounded up only
                                If Currency.Equals("TWD", StringComparison.InvariantCultureIgnoreCase) Then ewPrice = Math.Ceiling(ewPrice)

                                Me.Insert(getEWItem(New EW(CartFixer.StartLine, op(0).lineNo.Value + CartFixer.getStepByParent(op(0).parentLineNo.Value, op(0).itemType.Value), _
                                                        EWU.getEWItemByMonth(op(0).ewFlag.Value, DocReg), Me.org, op(0).divPlant.Value, ewPrice, op(0).Qty.Value)))
                            Else
                                Dim str As String = String.Format("update " & Me.tbSource.tbName & " set {2} where {0}=@key and ({1}=@lineNo or {3}=@lineNo)", Me.cartStruc.key.Name, Me.cartStruc.lineNo.Name, Me.cartStruc.ewFlag.Name & "=0", Me.cartStruc.parentLineNo.Name)
                                Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key), _
                                                                  New SqlClient.SqlParameter("@LineNo", lineNo)}
                                Dim n As Integer = sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
                            End If
                        End If
                    Else
                        If op(0).ewFlag.Value = 0 Then
                        Else
                            Dim EWU As IBUS.iEWUtil = New EWUtil
                            Dim ewPrice As Decimal = op(0).newunitPrice.Value * EWU.getRateByEWItem(EWU.getEWItemByMonth(op(0).ewFlag.Value, DocReg), DocReg)
                            If ewPrice > 0 Then
                                ewPrice = Decimal.Round(ewPrice, 2)
                                'Frank 2014/03/18 TWD need to be rounded up only
                                If Currency.Equals("TWD", StringComparison.InvariantCultureIgnoreCase) Then ewPrice = Math.Ceiling(ewPrice)

                                Dim EW As New EW(CartFixer.StartLine, op(0).lineNo.Value + CartFixer.getStepByParent(op(0).parentLineNo.Value, op(0).itemType.Value), _
                                                        EWU.getEWItemByMonth(op(0).ewFlag.Value, DocReg), Me.org, op(0).divPlant.Value, ewPrice, op(0).Qty.Value)
                                Me.Insert(getEWItem(EW))
                                ewn = 1
                            Else
                                Dim str As String = String.Format("update " & Me.tbSource.tbName & " set {2} where {0}=@key and ({1}=@lineNo or {3}=@lineNo)", Me.cartStruc.key.Name, Me.cartStruc.lineNo.Name, Me.cartStruc.ewFlag.Name & "=0", Me.cartStruc.parentLineNo.Name)
                                Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key), _
                                                                  New SqlClient.SqlParameter("@LineNo", lineNo)}
                                Dim n As Integer = sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
                            End If
                        End If
                    End If
                End If
            End If
        End If
        Return ewn
    End Function



    Public Function Add(ByVal item As IBUS.iCartLine, ByVal oType As COMM.Fixer.eDocType) As Integer Implements IBUS.iCart(Of IBUS.iCartLine).Add

        Return AddNew(item, oType)
    End Function
    Public Function GetListByParent(ByVal ParentLineNo As Integer, ByVal oType As COMM.Fixer.eDocType) As iCartList Implements iCart(Of IBUS.iCartLine).GetListByParent
        Return Me.dbReadLine(ParentLineNo, True)
    End Function
    Public Function GetListDTByParent(ByVal ParentLineNo As Integer) As DataTable
        Return Me.dbReadLineDT(ParentLineNo, True)
    End Function
    Public Function GetListAllDT() As DataTable
        Return Me.dbReadLineDT(CartFixer.StartLine)
    End Function

    Public Sub Insert(ByVal item As IBUS.iCartLine)
        InsertB(item)
    End Sub
    Default Public ReadOnly Property Item(ByVal LineNo As Integer) As IBUS.iCartLine Implements IBUS.iCart(Of IBUS.iCartLine).Item
        Get
            Dim o As oCartList = dbReadLine(LineNo)
            If Not IsNothing(o) AndAlso o.Count > 0 Then
                Return o(0)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public Sub Clear(ByVal oType As COMM.Fixer.eDocType) Implements IBUS.iCart(Of IBUS.iCartLine).Clear
        ClearB()
    End Sub
    'Public Sub RemoveAt(ByVal index As Integer) Implements IBUS.iCart(Of IBUS.iCartLine).RemoveAt
    '    RemoveAtB(index)
    'End Sub
    Public Sub RemoveAt(ByVal index As SortedSet(Of Integer), ByVal oType As COMM.Fixer.eDocType) Implements IBUS.iCart(Of IBUS.iCartLine).RemoveAt
        RemoveAtBWithEW(index, oType)
    End Sub

End Class




