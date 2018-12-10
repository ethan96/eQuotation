Public Interface iCartLine

End Interface
Public Interface iProd

End Interface
Public Class tbSource
    Sub New(ByVal AppArea As CartFixer.eAppArea)
        Select Case AppArea
            Case CartFixer.eAppArea.EQ
                Me._conn = "EQ"
                Me._tbName = "QuotationDetail"
            Case CartFixer.eAppArea.Cart
                Me._conn = "MY"
                Me._tbName = "Cart_Detail"
            Case CartFixer.eAppArea.Order
                Me._conn = "MY"
                Me._tbName = "Order_Detail"
        End Select
    End Sub
    Public _conn As String
    Public _tbName As String
    Public Property conn As String
        Get
            Return _conn
        End Get
        Set(ByVal value As String)
            _conn = value
        End Set
    End Property
    Public Property tbName As String
        Get
            Return _tbName
        End Get
        Set(ByVal value As String)
            _tbName = value
        End Set
    End Property
End Class
<AttributeUsage(AttributeTargets.Field, AllowMultiple:=False)> _
Public NotInheritable Class EnumDescriptionAttribute : Inherits Attribute
    Dim _description As String
    Public ReadOnly Property description As String
        Get
            Return _description
        End Get
    End Property

    Public Sub New(ByVal pdescription As String)
        MyBase.New()
        Me._description = pdescription
    End Sub
End Class
Public Class EnumHelper
    Public Shared Function getDescription(ByVal value As [Enum]) As String
        If IsNothing(value) Then
            Throw New ArgumentNullException("value")
        End If
        Dim description As String = value.ToString
        Dim fieldInfo As System.Reflection.FieldInfo = value.GetType().GetField(description)
        Dim ats() As EnumDescriptionAttribute = CType(fieldInfo.GetCustomAttributes(GetType(EnumDescriptionAttribute), False), EnumDescriptionAttribute())
        If Not IsNothing(ats) AndAlso ats.Length > 0 Then
            description = ats(0).description
        End If
        Return description
    End Function
End Class


Public Class CartFixer
    Public Enum eUndef
        X = -999
    End Enum
    Public Const StartLine As Integer = 0
    Public Const LineLimiter As Integer = eCartLineStep.Parent / eCartLineStep.Others

    Public Enum eErrCode
        <EnumDescription("HigherLevel cannot be found.")> _
        HigherLevelCannotBeFound = 2 ^ 0
        <EnumDescription("HigherLevel line must under startLine")> _
        HigherLevelMustUnderStartLine = 2 ^ 1
        <EnumDescription("Line no can not map to higherLevel.")> _
        LineNoCannotMapToHigherLevel = 2 ^ 2
        <EnumDescription("Update Line No faild,New line no can not map to higherLevel, items will exceed arrange of parent.")> _
        UpdateLineNoNewLineNoCannotMapToHigherLevel = 2 ^ 3
        <EnumDescription("Get new line no failed, New line no can not map to higherLevel.")> _
        NewLineCannotMapToHigherLevel = 2 ^ 4
        <EnumDescription("Get new line no failed, New line no can not map to itemType.")> _
        NewLineCannotMapToItemType = 2 ^ 5
        <EnumDescription("Incorrect Item Type.")> _
        ItemTypeIncorrect = 2 ^ 6
        <EnumDescription("Invalid PartNo.")> _
        InvalidPartNo = 2 ^ 7
        <EnumDescription("Only AGSEW item can be set Item Type AGS.")> _
        AGSEWItemTypeIncorrect = 2 ^ 8
        <EnumDescription("Field name definition duplicated.")> _
        FieldNameDefinitionDuplicated = 2 ^ 9
    End Enum
    Private Enum eCartLineStep
        Parent = 100
        Others = 1
    End Enum
    Public Enum eItemType
        Parent = 1
        Others = 0
    End Enum
    Public Enum eInsertOrRemove
        Insert = 0
        Remove = 1
    End Enum
    Public Enum eSpecialFlg
        Normal = 0
        isANA = 1
        isAJP = 2
    End Enum
    Public Const AGSEWPrefix As String = "AGS-EW"
    Private Shared Function isParent(ByVal LineNo As Integer) As Boolean
        If LineNo > StartLine AndAlso ((LineNo - StartLine) Mod (LineLimiter * eCartLineStep.Others)) = 0 Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function isValidParentLineNo(ByVal parentLineNo As Integer) As Boolean
        If parentLineNo = CartFixer.StartLine Then
            Return True
        End If
        If CartFixer.isParent(parentLineNo) Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function isParentUndStartLine(ByVal ItemType As eItemType, ByVal HigherLevel As Integer) As Boolean
        If ItemType = eItemType.Parent AndAlso HigherLevel = StartLine Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function isItemTypeIncorrect(ByVal ItemType As eItemType, ByVal LineNo As Integer) As Boolean
        If isValidParentLineNo(LineNo) Then
            If ItemType <> eItemType.Parent Then
                Return True
            End If
        Else
            If ItemType <> eItemType.Others Then
                Return True
            End If
        End If

        Return False
    End Function
    Public Shared Function isValidItemType(ByVal type As eItemType) As Boolean
        Dim t As Type = GetType(CartFixer.eItemType)
        For Each r As Integer In [Enum].GetValues(t)
            If r = type Then
                Return True
            End If
        Next
        Return False
    End Function


    Public Shared Function getParentLineNoFromLineNo(ByVal LineNo As Integer) As Integer
        If (LineNo - StartLine) Mod (LineLimiter * eCartLineStep.Others) = 0 Then
            Return StartLine
        End If
        Return (LineNo \ (LineLimiter * eCartLineStep.Others)) * (LineLimiter * eCartLineStep.Others)
    End Function
    Public Shared Function getStepByParent(ByVal ParentLineNo As Integer, ByVal ItemType As eItemType) As Integer
        If ParentLineNo = StartLine Then
            If ItemType = eItemType.Parent Then
                Return eCartLineStep.Parent
            End If
        End If
        Return eCartLineStep.Others
    End Function
    Public Shared Function getStartByParent(ByVal ParentLineNo As Integer) As Integer
        If ParentLineNo = StartLine Then
            Return StartLine
        End If
        Return ParentLineNo
    End Function
    Public Enum eProdType As Integer
        ItemStandard = 0
        ItemPTD = 1
        ItemAGSEW = 2
        ItemBTO = 3
        Dummy = 4
        HardDrive = 5
    End Enum
    Public Enum eAppArea As Integer
        EQ = 0
        Cart = 1
        Order = 2
    End Enum
End Class


Partial Public MustInherit Class oCart(Of T As oCartLine)


    Private Function getNewLineNo(ByVal parentLineNo As Integer, ByVal ItemType As CartFixer.eItemType, ByRef expCode As CartFixer.eErrCode) As Integer
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
                expCode = expCode Or CartFixer.eErrCode.HigherLevelCannotBeFound
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
            expCode = expCode Or CartFixer.eErrCode.NewLineCannotMapToItemType
            Return CartFixer.eUndef.X
        End If
        If CartFixer.getParentLineNoFromLineNo(ret) <> parentLineNo Then
            expCode = expCode Or CartFixer.eErrCode.NewLineCannotMapToHigherLevel
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
    Private Function AddNew(ByVal item As T) As Integer
        If item.errCode > 0 Then
            Me._errCode = Me._errCode Or item.errCode
            Return CartFixer.eUndef.X
        End If
        If item.parentLineNo.Value <> CartFixer.StartLine And (Not (CartFixer.isValidParentLineNo(item.parentLineNo.Value) AndAlso isLineNoExists(item.parentLineNo.Value))) Then
            Me._errCode = Me._errCode Or CartFixer.eErrCode.HigherLevelCannotBeFound
            Return CartFixer.eUndef.X
        End If
        Dim expCode As Integer = 0
        Dim NLineNo = getNewLineNo(item.parentLineNo.Value, item.itemType.Value, expCode)

        If expCode <> 0 Then
            Me._errCode = Me._errCode Or expCode
            Return CartFixer.eUndef.X
        End If
        item.lineNo.Value = NLineNo
        Me.dbInsert(item)
        Return NLineNo
    End Function

    Private Sub InsertB(ByVal item As T)
        If item.errCode > 0 Then
            Me._errCode = Me._errCode Or item.errCode
            Exit Sub
        End If
        If item.parentLineNo.Value <> CartFixer.StartLine And (Not (CartFixer.isValidParentLineNo(item.parentLineNo.Value) AndAlso isLineNoExists(item.parentLineNo.Value))) Then
            Me._errCode = Me._errCode Or CartFixer.eErrCode.HigherLevelCannotBeFound
            Exit Sub
        End If
        If CartFixer.isItemTypeIncorrect(item.itemType.Value, item.lineNo.Value) Then
            Me._errCode = Me._errCode Or CartFixer.eErrCode.ItemTypeIncorrect
            Exit Sub
        End If
        If CartFixer.getParentLineNoFromLineNo(item.lineNo.Value) <> item.parentLineNo.Value Then
            Me._errCode = Me._errCode Or CartFixer.eErrCode.LineNoCannotMapToHigherLevel
            Exit Sub
        End If
        Dim expCode As Integer = 0
        updateLineInsertOrRemove(item.lineNo.Value, item.parentLineNo.Value, item.itemType.Value, CartFixer.eInsertOrRemove.Insert, expCode)
        If expCode <> 0 Then
            Me._errCode = Me._errCode Or expCode
            Exit Sub
        End If
        Me.dbInsert(item)
    End Sub
    Private Sub updateLineInsertOrRemove(ByVal lineNo As Integer, ByVal parentLineNo As Integer, ByVal ItemType As CartFixer.eItemType, ByVal InsertOrRemove As CartFixer.eInsertOrRemove, ByRef expCode As CartFixer.eErrCode)
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
                expCode = expCode Or CartFixer.eErrCode.UpdateLineNoNewLineNoCannotMapToHigherLevel
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
        r.Close()
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
            Dim expCode As CartFixer.eErrCode = 0
            updateLineInsertOrRemove(LineNo, o(0).parentLineNo.Value, o(0).itemType.Value, CartFixer.eInsertOrRemove.Remove, expCode)
            If expCode <> 0 Then
                Me._errCode = Me._errCode Or expCode
            End If
        End If

    End Sub
    Private Sub RemoveAtB(ByVal LineNo As SortedSet(Of Integer))
        Dim c1 As Integer = 0, c2 As Integer = 0, c3 As Integer = 0
        For Each i As Integer In LineNo
            Dim pl As Integer = CartFixer.getParentLineNoFromLineNo(i)
            Dim itemTpye As CartFixer.eItemType = CartFixer.eItemType.Others
            If CartFixer.isValidParentLineNo(i) Then itemTpye = CartFixer.eItemType.Parent
            Dim newi As Integer = i
            If itemTpye = CartFixer.eItemType.Parent Then
                newi = i - c1 * CartFixer.getStepByParent(pl, itemTpye)
            ElseIf itemTpye = CartFixer.eItemType.Parent Then
                If pl = CartFixer.StartLine Then
                    newi = i - c2 * CartFixer.getStepByParent(pl, itemTpye)
                Else
                    newi = i - c3 * CartFixer.getStepByParent(pl, itemTpye)
                End If
            End If
            Dim ewn As Integer = 0
            ewn = RemoveEWFirstBeforeRemoveItem(newi)
            RemoveAt(newi)
            If pl > CartFixer.StartLine Then
                updateEW(pl)
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

    Public Function UpdateByLine(ByVal setStr As String, ByVal LineNo As Integer) As Integer
        Dim str As String = String.Format("update " & Me.tbSource.tbName & " set " & setStr & " where {0}=@key and {1}=@lineNo", Me.cartStruc.key.Name, Me.cartStruc.lineNo.Name)
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key), _
                                          New SqlClient.SqlParameter("@LineNo", LineNo)}
        Dim n As Integer = sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
        Return n
    End Function
    Public Function UpdateByStr(ByVal setStr As String, ByVal whereStr As String) As Integer
        Dim str As String = String.Format("update " & Me.tbSource.tbName & " set {0} where {1}", setStr, whereStr)
        Dim n As Integer = sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, Nothing)
        Return n
    End Function
    Public Function RemoveEWFirstBeforeRemoveItem(ByVal lineNo As Integer) As Integer
        Dim brothersN As Integer = 0
        Dim oew As oCartList = dbReadLine(lineNo, isParent:=True, isEW:=True)
        If Not IsNothing(oew) AndAlso oew.Count > 0 Then
            Me.RemoveAt(oew(0).lineNo.Value)
        End If
        Dim oew1 As oCartList = dbReadLine(lineNo, isEW:=True)
        If Not IsNothing(oew1) AndAlso oew1.Count > 0 Then
            Me.RemoveAt(oew1(0).lineNo.Value)
            brothersN = brothersN + 1
        End If
        Return brothersN
    End Function

    Public Sub updateEW(ByVal lineNo As Integer)
        Dim op As oCartList = Me.dbReadLine(lineNo)
        If Not IsNothing(op) AndAlso op.Count > 0 AndAlso Not (op(0).partNo.ToString.Contains(CartFixer.AGSEWPrefix)) Then
            If CartFixer.isValidParentLineNo(lineNo) Then
                Dim oew As oCartList = dbReadLine(lineNo, isParent:=True, isEW:=True)
                If Not IsNothing(oew) AndAlso oew.Count > 0 Then

                    If op(0).ewFlag.Value = 0 Then
                        Me.RemoveAt(oew(0).lineNo.Value)
                    Else
                        Me.RemoveAt(oew(0).lineNo.Value)
                        Dim dt As New DataTable
                        dt = Me.dbReadLineDT(lineNo, True)
                        Dim ewPrice As Decimal = 0
                        If _specialFlag = CartFixer.eSpecialFlg.isANA Then
                            ewPrice = oCartList.getWarrantableTotalAmount(dt) / op(0).Qty.Value * cartBiz.getRateByEWItem(cartBiz.getEWItemByMonth(op(0).ewFlag.Value), op(0).divPlant.Value)
                        Else
                            ewPrice = oCartList.getTotalAmount(dt) / op(0).Qty.Value * cartBiz.getRateByEWItem(cartBiz.getEWItemByMonth(op(0).ewFlag.Value), op(0).divPlant.Value)
                        End If
                        If ewPrice <> 0 Then
                            Me.Add(getEWItem(New EW(lineNo, 0, cartBiz.getEWItemByMonth(op(0).ewFlag.Value), Me.org, op(0).divPlant.Value, ewPrice, op(0).Qty.Value)))
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
                        Dim dt As New DataTable
                        dt = Me.dbReadLineDT(lineNo, True)
                        Dim ewPrice As Decimal = 0
                        If _specialFlag = CartFixer.eSpecialFlg.isANA Then
                            ewPrice = oCartList.getWarrantableTotalAmount(dt) / op(0).Qty.Value * cartBiz.getRateByEWItem(cartBiz.getEWItemByMonth(op(0).ewFlag.Value), op(0).divPlant.Value)
                        Else
                            ewPrice = oCartList.getTotalAmount(dt) / op(0).Qty.Value * cartBiz.getRateByEWItem(cartBiz.getEWItemByMonth(op(0).ewFlag.Value), op(0).divPlant.Value)
                        End If
                        If ewPrice <> 0 Then
                            Me.Add(getEWItem(New EW(lineNo, 0, cartBiz.getEWItemByMonth(op(0).ewFlag.Value), Me.org, op(0).divPlant.Value, ewPrice, op(0).Qty.Value)))
                        Else
                            Dim str As String = String.Format("update " & Me.tbSource.tbName & " set {2} where {0}=@key and ({1}=@lineNo or {3}=@lineNo)", Me.cartStruc.key.Name, Me.cartStruc.lineNo.Name, Me.cartStruc.ewFlag.Name & "=0", Me.cartStruc.parentLineNo.Name)
                            Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key), _
                                                              New SqlClient.SqlParameter("@LineNo", lineNo)}
                            Dim n As Integer = sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
                        End If
                    End If
                End If
            Else
                If _specialFlag = CartFixer.eSpecialFlg.isANA Then
                    If cartBiz.isWarrantable(op(0).partNo.Value) Then
                        Dim oew As oCartList = dbReadLine(lineNo, isEW:=True)
                        If Not IsNothing(oew) AndAlso oew.Count > 0 Then
                            If op(0).ewFlag.Value = 0 Then
                                Me.RemoveAt(oew(0).lineNo.Value)
                            Else
                                Me.RemoveAt(oew(0).lineNo.Value)

                                Dim ewPrice As Decimal = op(0).newunitPrice.Value * cartBiz.getRateByEWItem(cartBiz.getEWItemByMonth(op(0).ewFlag.Value), op(0).divPlant.Value)
                                If ewPrice > 0 Then
                                    Me.Insert(getEWItem(New EW(CartFixer.StartLine, op(0).lineNo.Value + CartFixer.getStepByParent(op(0).parentLineNo.Value, op(0).itemType.Value), _
                                                            cartBiz.getEWItemByMonth(op(0).ewFlag.Value), Me.org, op(0).divPlant.Value, ewPrice, op(0).Qty.Value)))
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
                                Dim ewPrice As Decimal = op(0).newunitPrice.Value * cartBiz.getRateByEWItem(cartBiz.getEWItemByMonth(op(0).ewFlag.Value), op(0).divPlant.Value)
                                If ewPrice > 0 Then
                                    Dim EW As New EW(CartFixer.StartLine, op(0).lineNo.Value + CartFixer.getStepByParent(op(0).parentLineNo.Value, op(0).itemType.Value), _
                                                            cartBiz.getEWItemByMonth(op(0).ewFlag.Value), Me.org, op(0).divPlant.Value, ewPrice, op(0).Qty.Value)
                                    Me.Insert(getEWItem(EW))
                                Else
                                    Dim str As String = String.Format("update " & Me.tbSource.tbName & " set {2} where {0}=@key and ({1}=@lineNo or {3}=@lineNo)", Me.cartStruc.key.Name, Me, cartStruc.lineNo.Name, Me.cartStruc.ewFlag.Name & "=0", Me.cartStruc.parentLineNo.Name)
                                    Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@key", Me.key), _
                                                                      New SqlClient.SqlParameter("@LineNo", lineNo)}
                                    Dim n As Integer = sqlhelper.ExecuteNonQuery(Me.tbSource.conn, CommandType.Text, str, p)
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub
End Class
'Public Delegate Function dReturnEwItem(ByVal ewItem As EW) As oCartLine
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
Public Class Field
    Sub New(ByVal Name As String, ByVal defaultValue As Object  ,Optional ByVal Value As Object = Nothing)
        Me._Name = Name
        Me._defaultValue = defaultValue
        If IsNothing(Value) Then
            Me.Value = defaultValue
        Else
            Me.Value = _Value
        End If
    End Sub
    Private _Name As String = ""
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property
    Private _Value As Object = Nothing
    Public Property Value As Object
        Get
            Return _Value
        End Get
        Set(ByVal value As Object)
            _Value = value
        End Set
    End Property
    Private _defaultValue As Object = Nothing
    Public ReadOnly Property defaultValue As Object
        Get
            Return _defaultValue
        End Get
    End Property
End Class
Public MustInherit Class oCartLine
    Implements iCartLine
    Sub New(ByVal key As String, ByVal lineNo As Integer, ByVal parentLineNo As Integer,
            ByVal partNo As String, ByVal partDesc As String, ByVal listPrice As Decimal, ByVal newUnitPrice As Decimal, ByVal newCost As Decimal, _
            ByVal Qty As Integer, ByVal EWFlag As Integer, ByVal divPlant As String, Optional ByVal reqDate As DateTime = Nothing, Optional ByVal ItemType As CartFixer.eItemType = CartFixer.eItemType.Others, Optional ByVal category As String = "")
        setFieldName()
        If partNo <> "" Then
            Me._partNo.Value = partNo
            Me._partDesc.Value = partDesc
            Me._divPlant.Value = divPlant
        Else
            Me._errCode = Me._errCode Or CartFixer.eErrCode.InvalidPartNo
            Exit Sub
        End If


        If Not CartFixer.isValidItemType(ItemType) Then
            Me._errCode = Me._errCode Or CartFixer.eErrCode.ItemTypeIncorrect
            Exit Sub
        End If
        If ItemType = CartFixer.eItemType.Parent And (Not CartFixer.isParentUndStartLine(ItemType, parentLineNo)) Then
            Me._errCode = Me._errCode Or CartFixer.eErrCode.HigherLevelMustUnderStartLine
            Exit Sub
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

    End Sub
    Sub New(ByVal key As String, ByVal lineNo As Integer, ByVal parentLineNo As Integer,
            ByVal p As oProd, ByVal newUnitPrice As Decimal, ByVal listPrice As Decimal, ByVal newCost As Decimal, _
            ByVal Qty As Integer, ByVal EWFlag As Integer, Optional ByVal reqDate As DateTime = Nothing, Optional ByVal ItemType As CartFixer.eItemType = CartFixer.eItemType.Others, Optional ByVal category As String = "")

        '================
        setFieldName()
        If IsNothing(p) OrElse (ItemType <> CartFixer.eItemType.Parent AndAlso p.prodValid = False) Then
            Me._errCode = Me._errCode Or CartFixer.eErrCode.InvalidPartNo
            Exit Sub
        Else
            Me._partNo.Value = p.partNo
            Me._partDesc.Value = p.partDesc
            Me._divPlant.Value = p.divPlant

        End If
        If Not CartFixer.isValidItemType(ItemType) Then
            Me._errCode = Me._errCode Or CartFixer.eErrCode.ItemTypeIncorrect
            Exit Sub
        End If
        If ItemType = CartFixer.eItemType.Parent And (Not CartFixer.isParentUndStartLine(ItemType, parentLineNo)) Then
            Me._errCode = Me._errCode Or CartFixer.eErrCode.HigherLevelMustUnderStartLine
            Exit Sub
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
    Public ReadOnly Property isValidLine As Boolean
        Get
            Return _isValidLine
        End Get
    End Property
    Private _errCode As Integer = 0
    Public ReadOnly Property errCode As Integer
        Get
            Return errCode
        End Get
    End Property
    Protected MustOverride Sub setFieldName()

    Protected _key As New Field("", "")
    Public Property key As Field
        Get
            Return _key
        End Get
        Set(ByVal value As Field)
            _key.Value = value.Value
        End Set
    End Property
    Protected _parentLineNo As New Field("", CartFixer.StartLine)
    Public Property parentLineNo As Field
        Get
            Return _parentLineNo
        End Get
        Set(ByVal value As Field)
            _parentLineNo.Value = value.Value
        End Set
    End Property
    Protected _lineNo As New Field("", CartFixer.StartLine)
    Public Property lineNo As Field
        Get
            Return _lineNo
        End Get
        Set(ByVal value As Field)
            _lineNo.Value = value.Value
        End Set
    End Property
    Protected _partNo As New Field("", "")
    Public Property partNo As Field
        Get
            Return _partNo
        End Get
        Set(ByVal value As Field)
            _partNo.Value = value.Value
        End Set
    End Property
    Protected _partDesc As New Field("", "")
    Public Property partDesc As Field
        Get
            Return _partDesc
        End Get
        Set(ByVal value As Field)
            _partDesc.Value = value.Value
        End Set
    End Property
    Protected _listPrice As New Field("", 0)
    Public Property listPrice As Field
        Get
            Return _listPrice
        End Get
        Set(ByVal value As Field)
            _listPrice.Value = value.Value
        End Set
    End Property
    Protected _unitPrice As New Field("", 0)
    Public Property unitPrice As Field
        Get
            Return _unitPrice
        End Get
        Set(ByVal value As Field)
            _unitPrice.Value = value.Value
        End Set
    End Property
    Protected _newUnitPrice As New Field("", 0)
    Public Property newunitPrice As Field
        Get
            Return _newUnitPrice
        End Get
        Set(ByVal value As Field)
            _newUnitPrice.Value = value.Value
        End Set
    End Property
    Protected _cost As New Field("", 0)
    Public Property cost As Field
        Get
            Return _cost
        End Get
        Set(ByVal value As Field)
            _cost.Value = value.Value
        End Set
    End Property
    Protected _newCost As New Field("", 0)
    Public Property newCost As Field
        Get
            Return _newCost
        End Get
        Set(ByVal value As Field)
            _newCost.Value = value.Value
        End Set
    End Property
    Protected _qty As New Field("", 0)
    Public Property Qty As Field
        Get
            Return _qty
        End Get
        Set(ByVal value As Field)
            _qty.Value = value.Value
        End Set
    End Property
    Protected _itemType As New Field("", CInt(CartFixer.eItemType.Others))
    Public Property itemType As Field
        Get
            Return _itemType
        End Get
        Set(ByVal value As Field)
            _itemType.Value = value.Value
        End Set
    End Property
    Protected _ewFlag As New Field("", 0)
    Public Property ewFlag As Field
        Get
            Return _ewFlag
        End Get
        Set(ByVal value As Field)
            _ewFlag.Value = value.Value
        End Set
    End Property
    Protected _inventory As New Field("", 0)
    Public Property inventory As Field
        Get
            Return _inventory
        End Get
        Set(ByVal value As Field)
            _inventory.Value = value.Value
        End Set
    End Property
    Protected _reqDate As New Field("", Now.Date.ToShortDateString)
    Public Property reqDate As Field
        Get
            Return _reqDate
        End Get
        Set(ByVal value As Field)
            _reqDate.Value = value.Value
        End Set
    End Property
    Protected _dueDate As New Field("", Now.Date.ToShortDateString)
    Public Property dueDate As Field
        Get
            Return _dueDate
        End Get
        Set(ByVal value As Field)
            _dueDate.Value = value.Value
        End Set
    End Property
    Protected _canBeConfirmed As New Field("", 0)
    Public Property canBeConfirmed As Field
        Get
            Return _canBeConfirmed
        End Get
        Set(ByVal value As Field)
            _canBeConfirmed.Value = value.Value
        End Set
    End Property
    Protected _category As New Field("", "")
    Public Property category As Field
        Get
            Return _category
        End Get
        Set(ByVal value As Field)
            _category.Value = value.Value
        End Set
    End Property
    Protected _divPlant As New Field("", "")
    Public Property divPlant As Field
        Get
            Return _divPlant
        End Get
        Set(ByVal value As Field)
            _divPlant.Value = value.Value
        End Set
    End Property
End Class
Public MustInherit Class oProd
    Implements iProd
    Sub New(ByVal partNo As String, ByVal org As String, Optional ByVal divPlant As String = "")
        Dim r As DataRow = getProductStatusLine(partNo, org)
        Me._partNo = partNo
        Me._org = org
        Me._divPlant = divPlant
        If Not IsNothing(r) Then
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
        End If
    End Sub
 
    Private _prodValid As Boolean = False
    Public ReadOnly Property prodValid As Boolean
        Get
            Return _prodValid
        End Get
    End Property
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
                            " SAP_PRODUCT_STATUS_ORDERABLE AS b ON a.PART_NO = b.PART_NO LEFT OUTER JOIN " & _
                            " SAP_PRODUCT_ABC AS c ON a.PART_NO = c.PART_NO AND b.DLV_PLANT = c.PLANT LEFT OUTER JOIN " & _
                            " SAP_PRODUCT_EXT_DESC AS d ON a.PART_NO = d.PART_NO " & _
        " WHERE (a.PART_NO = @PN) AND (b.SALES_ORG = @SALESORG)", partNo, org), p)
        If Not IsNothing(dt) AndAlso dt.Rows.Count > 0 Then
            Return dt.Rows(0)
        End If
        Return Nothing
    End Function
    Private _partNo As String = ""
    Public Property partNo As String
        Get
            Return _partNo
        End Get
        Set(ByVal value As String)
            _partNo = value
        End Set
    End Property
    Private _org As String = ""
    Public Property org As String
        Get
            Return _org
        End Get
        Set(ByVal value As String)
            _org = value
        End Set
    End Property
    Private _divPlant As String = ""
    Public Property divPlant As String
        Get
            Return _divPlant
        End Get
        Set(ByVal value As String)
            _divPlant = value
        End Set
    End Property
    Private _partDesc As String = ""
    Public Property partDesc As String
        Get
            Return _partDesc
        End Get
        Set(ByVal value As String)
            _partDesc = value
        End Set
    End Property
    Private _partModel As String = ""
    Public Property partModel As String
        Get
            Return _partModel
        End Get
        Set(ByVal value As String)
            _partModel = value
        End Set
    End Property
    Private _type As Integer = CartFixer.eProdType.ItemStandard
    Public Property type As Integer
        Get
            Return _type
        End Get
        Set(ByVal value As Integer)
            _type = value
        End Set
    End Property
    Private _productLine As String = ""
    Public Property productLine As String
        Get
            Return _productLine
        End Get
        Set(ByVal value As String)
            _productLine = value
        End Set
    End Property
    Private _ROHS As Integer = 0
    Public Property ROHS As Integer
        Get
            Return _ROHS
        End Get
        Set(ByVal value As Integer)
            _ROHS = value
        End Set
    End Property
    Private _ABC As String = ""
    Public Property ABC As String
        Get
            Return _ABC
        End Get
        Set(ByVal value As String)
            _ABC = value
        End Set
    End Property
End Class
Public Class oCartList
    Inherits List(Of oCartLine)
    Shared Function getWarrantableTotalAmount(ByVal quotationDetail As DataTable) As Decimal
        Dim rlt As Decimal = 0
        If quotationDetail IsNot Nothing AndAlso quotationDetail.Rows.Count > 0 Then
            For Each r As DataRow In quotationDetail.Rows
                If Not CartFixer.isValidParentLineNo(r.Item("Line_No")) AndAlso cartBiz.isWarrantable(r.Item("PartNo")) Then
                    rlt = rlt + (r.Item("newunitprice") * r.Item("qty"))
                End If
            Next
        End If
        Return rlt
    End Function
    Shared Function getTotalAmount(ByVal quotationDetail As DataTable) As Decimal
        Dim rlt As Decimal = 0
        If quotationDetail IsNot Nothing AndAlso quotationDetail.Rows.Count > 0 Then
            For Each r As DataRow In quotationDetail.Rows
                If IsNumeric(r.Item("otype")) AndAlso (Not r.Item("otype") = 1) Then
                    rlt = rlt + (r.Item("newunitprice") * r.Item("qty"))
                End If
            Next
        End If
        Return rlt
    End Function
End Class
Public Class dbac
    Shared Function getProductByLine(ByVal PN As String) As DataTable
        Dim str As String = " SELECT PART_NO, MODEL_NO, MATERIAL_GROUP, DIVISION, PRODUCT_HIERARCHY, PRODUCT_GROUP, PRODUCT_DIVISION, PRODUCT_LINE, " & _
        " GENITEMCATGRP, PRODUCT_DESC, ROHS_FLAG, EGROUP, STATUS, EDIVISION, NET_WEIGHT, VOLUME, WEIGHT_UNIT, GROSS_WEIGHT, " & _
        " VOLUME_UNIT, PRODUCT_TYPE, LAST_UPD_DATE, CREATE_DATE, SIZE_DIMENSIONS, GIP_CODE " & _
        " FROM SAP_PRODUCT " & _
        " WHERE (PART_NO = @PN) "
        Dim p() As SqlClient.SqlParameter = {New SqlClient.SqlParameter("@PN", PN)}
        Dim DT As New DataTable
        DT = sqlhelper.getDT("MY", CommandType.Text, str, p)
        Return DT
    End Function
End Class

Public Class cartBiz
    Shared Function isWarrantable(ByVal PN As String) As [Boolean]
        If String.IsNullOrEmpty(PN) = False AndAlso PN.EndsWith("-BTO", StringComparison.InvariantCultureIgnoreCase) Then
            Return True
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
    Shared Function isSoftware(ByVal PN As String) As [Boolean]
        Dim DT As New DataTable
        DT = dbac.getProductByLine(PN)

        If DT.Rows.Count = 0 Then Return True

        If PN.StartsWith("96SW") OrElse PN.StartsWith("98MQ") OrElse PN.StartsWith("968Q") Then
            Return True
        ElseIf DT.Rows(0).Item("PRODUCT_LINE") IsNot Nothing AndAlso (DT.Rows(0).Item("PRODUCT_LINE") = "EPCS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "EDOS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WCOM" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WAUT" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "DAAS") Then
            Return True
        Else
            Return False
        End If
    End Function
    Shared Function getRateByEWItem(ByVal itemNo As String, ByVal Plant As String) As Double

        If Plant.Length = 4 AndAlso Left(Plant, 2) = "US" Then
            Select Case itemNo.ToUpper.Trim()
                Case "AGS-EW-03"
                    Return 0.03
                Case "AGS-EW-06"
                    Return 0.05
                Case "AGS-EW-09"
                    Return 0.05
                Case "AGS-EW-12"
                    Return 0.1
                Case "AGS-EW-15"
                    Return 0.07
                Case "AGS-EW-21"
                    Return 0.08
                Case "AGS-EW-24"
                    Return 0.15
                Case "AGS-EW-36"
                    Return 0.25
                Case "AGS-EW-AD"
                    Return 0.05
                Case "AGS-EW/DOA-03"
                    Return 0.01
            End Select
        Else
            Select Case itemNo.ToUpper.Trim()
                Case "AGS-EW-03"
                    Return 0.02
                Case "AGS-EW-06"
                    Return 0.035
                Case "AGS-EW-09"
                    Return 0.05
                Case "AGS-EW-12"
                    Return 0.06
                Case "AGS-EW-15"
                    Return 0.07
                Case "AGS-EW-21"
                    Return 0.08
                Case "AGS-EW-24"
                    Return 0.1
                Case "AGS-EW-36"
                    Return 0.15
                Case "AGS-EW-AD"
                    Return 0.05
                Case "AGS-EW/DOA-03"
                    Return 0.01
            End Select
        End If

        Return 0
    End Function
    Shared Function getEWItemByMonth(ByVal month As Integer) As String
        If IsNumeric(month) AndAlso month > 0 And month.ToString.Length < 3 Or month = 999 Then
            If month = 99 Then
                Return "AGS-EW-AD"
            End If
            If month = 999 Then
                Return "AGS-EW/DOA-03"
            End If
            Return "AGS-EW-" & month.ToString("00")
        End If
        Return ""
    End Function
End Class
Partial Public MustInherit Class oCart(Of T As oCartLine)
    Public Sub New(ByVal pTbSource As tbSource, ByVal key As String, ByVal org As String, ByVal pCartStruc As T)
        _count = 0
        Me._tbSource = pTbSource
        Me._key = key
        Me._org = org
        Me._cartStruc = pCartStruc
    End Sub
    Private _errCode As Integer = 0
    Public ReadOnly Property errCode As Integer
        Get
            Return _errCode
        End Get
    End Property
    Private _cartStruc As T
    Public ReadOnly Property cartStruc As T
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
    Public ReadOnly Property key As String
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
    Private _specialFlag As CartFixer.eSpecialFlg = CartFixer.eSpecialFlg.Normal
    Public Property specialFlag As CartFixer.eSpecialFlg
        Get
            Return _specialFlag
        End Get
        Set(ByVal value As CartFixer.eSpecialFlg)
            _specialFlag = value
        End Set
    End Property

    Private _count As Integer = 0
    Public ReadOnly Property Count As Integer
        Get
            Count = getCartCount()
            Return Count
        End Get
    End Property
    Private Function getPropertyDic(ByVal o As Object, ByRef expCode As Integer) As SortedDictionary(Of String, Field)
        Dim ret As New SortedDictionary(Of String, Field)
        Dim t As Type = o.GetType
        For Each r As System.Reflection.PropertyInfo In t.GetProperties()
            If r.PropertyType = GetType(Field) Then
                Dim otemp As Field = CType(r.GetValue(o, Nothing), Field)
                If Not IsNothing(otemp) AndAlso otemp.Name <> "" Then
                    If Not ret.ContainsKey(otemp.Name) Then
                        ret.Add(otemp.Name, otemp)
                    Else
                        expCode = expCode Or CartFixer.eErrCode.FieldNameDefinitionDuplicated
                        Return Nothing
                    End If
                End If
            End If
        Next
        Return ret
    End Function
    Private Sub dbInsert(ByVal item As T)
        Dim expCode As Integer = 0
        Dim fieldDic As SortedDictionary(Of String, Field) = getPropertyDic(item, expCode)
        If expCode <> 0 Then
            Me._errCode = Me._errCode Or errCode
            Exit Sub
        End If
        If Not IsNothing(fieldDic) AndAlso fieldDic.Count > 0 Then
            Dim p() As SqlClient.SqlParameter
            Dim strPat As String = ""
            Dim strVal As String = ""
            Dim n As Integer = 0
            For Each r As KeyValuePair(Of String, Field) In fieldDic
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
        Dim item As T = Me.cartStruc
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
        Dim fieldDic As SortedDictionary(Of String, Field) = getPropertyDic(item, expCode)
        If expCode <> 0 Then
            Me._errCode = Me._errCode Or errCode
            Exit Function
        End If
        Dim sqlstr As String = ""
        Dim pat As String = ""
        If Not IsNothing(fieldDic) AndAlso fieldDic.Count > 0 Then
            For Each r As KeyValuePair(Of String, Field) In fieldDic
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
        Return dt
    End Function
    Protected MustOverride Function getItemByDataRow(ByVal r As DataRow) As oCartLine
    Protected MustOverride Function getEWItem(ByVal ewItem As EW) As T
    Protected MustOverride Function getNewLineList() As oCartList
    Public Function GetListAll() As oCartList
        Return Me.dbReadLine(CartFixer.StartLine)
    End Function
    Public Function GetListByParent(ByVal ParentLineNo As Integer) As oCartList
        Return Me.dbReadLine(ParentLineNo, True)
    End Function
    Public Function GetListDTByParent(ByVal ParentLineNo As Integer) As DataTable
        Return Me.dbReadLineDT(ParentLineNo, True)
    End Function
    Public Function GetListAllDT() As DataTable
        Return Me.dbReadLineDT(CartFixer.StartLine)
    End Function
    Public Function Add(ByVal item As T) As Integer
        Return AddNew(item)
    End Function
    Public Sub Insert(ByVal item As T)
        InsertB(item)
    End Sub
    Default Public ReadOnly Property Item(ByVal LineNo As Integer) As T
        Get
            Dim o As oCartList = dbReadLine(LineNo)
            If Not IsNothing(o) AndAlso o.Count > 0 Then
                Return o(0)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public Sub Clear()
        ClearB()
    End Sub
    Public Sub RemoveAt(ByVal index As Integer)
        RemoveAtB(index)
    End Sub
    Public Sub RemoveAt(ByVal index As SortedSet(Of Integer))
        RemoveAtB(index)
    End Sub
 
End Class

Public Class errMsg
    Shared Function getErrMsg(ByVal errCode As CartFixer.eErrCode) As String
        Dim em As String = ""
        Dim t As Type = GetType(CartFixer.eErrCode)
        For Each s As String In [Enum].GetNames(t)
            If ([Enum].Format(t, [Enum].Parse(t, s), "d") And errCode) = [Enum].Format(t, [Enum].Parse(t, s), "d") Then
                em = em & "<br/>" & EnumHelper.getDescription(CType([Enum].Format(t, [Enum].Parse(t, s), "d"), CartFixer.eErrCode))
            End If
        Next
        Return em
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
Public Class prodF
    Public Shared Function getProdByPartNo(ByVal partNo As String, ByVal org As String, Optional ByVal divPlant As String = "") As oProd
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
End Class
