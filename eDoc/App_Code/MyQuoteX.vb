Imports Advantech.Myadvantech.DataAccess

'Imports EDOC
Public Class MyQuoteX
    Public Shared Function GetQuoteMaster(ByVal QuoteID As String) As Quote_Master
        If Not String.IsNullOrEmpty(QuoteID) Then
            Dim _QuoteMaster As Quote_Master = MyUtil.Current.CurrentDataContext.Quote_Masters.Where(Function(p) p.quoteId = QuoteID).FirstOrDefault()
            If _QuoteMaster IsNot Nothing Then Return _QuoteMaster
        End If
        Return Nothing
    End Function
    Public Shared Function GetQuoteMasterByQuoteNo(ByVal QuoteNo As String) As Quote_Master
        If Not String.IsNullOrEmpty(QuoteNo) Then
            Dim _QuoteMaster As Quote_Master = MyUtil.Current.CurrentDataContext.Quote_Masters.Where(Function(p) p.quoteNo = QuoteNo).FirstOrDefault()
            If _QuoteMaster IsNot Nothing Then Return _QuoteMaster
        End If
        Return Nothing
    End Function
    Public Shared Function GetQuoteList(ByVal QuoteID As String) As List(Of QuoteItem)
        If Not String.IsNullOrEmpty(QuoteID) Then
            Dim _Quotelist As List(Of QuoteItem) = MyUtil.Current.CurrentDataContext.QuoteItems.Where(Function(p) p.quoteId = QuoteID).OrderBy(Function(p) p.line_No).ToList()
            If _Quotelist IsNot Nothing Then Return _Quotelist
        End If
        Return Nothing
    End Function
    Public Shared Function GetATWTotalAmount(ByVal QuoteID As String) As Integer
        Dim looseItemList As List(Of QuoteItem) = GetQuoteLooseItems(QuoteID)
        Dim btosParentList As List(Of QuoteItem) = GetQuoteBTOSParentItems(QuoteID)
        Dim total As Decimal = 0

        Dim _DisplayUnitPrice As Double = 0
        Dim _DisplayQty As Integer = 0

        If Not looseItemList Is Nothing AndAlso looseItemList.Any() Then
            For Each item As QuoteItem In looseItemList
                _DisplayUnitPrice = 0
                _DisplayQty = 0
                Double.TryParse(item.DisplayUnitPrice, _DisplayUnitPrice)
                Integer.TryParse(item.DisplayQty, _DisplayQty)

                total = total + _DisplayUnitPrice * _DisplayQty
            Next
        End If

        If Not btosParentList Is Nothing AndAlso btosParentList.Any() Then
            For Each item As QuoteItem In btosParentList

                _DisplayUnitPrice = 0
                _DisplayQty = 0
                Double.TryParse(item.DisplayUnitPrice, _DisplayUnitPrice)
                Integer.TryParse(item.DisplayQty, _DisplayQty)

                total = total + _DisplayUnitPrice * _DisplayQty
                'If item.newUnitPrice > 0 Then
                '    total = total + item.newUnitPrice * item.qty
                'Else
                '    total = total + item.UnitPriceX * item.qty
                'End If
            Next
        End If
        Return total
    End Function
    'ICC 2016/4/12 Add amount V2 for USD dollar
    Public Shared Function GetATWTotalAmountV2(ByVal QuoteID As String) As Decimal
        Dim looseItemList As List(Of QuoteItem) = GetQuoteLooseItems(QuoteID)
        Dim btosParentList As List(Of QuoteItem) = GetQuoteBTOSParentItems(QuoteID)
        Dim total As Decimal = 0

        Dim _DisplayUnitPrice As Double = 0
        Dim _DisplayQty As Integer = 0

        If Not looseItemList Is Nothing AndAlso looseItemList.Any() Then
            For Each item As QuoteItem In looseItemList
                _DisplayUnitPrice = 0
                _DisplayQty = 0
                Double.TryParse(item.DisplayUnitPrice, _DisplayUnitPrice)
                Integer.TryParse(item.DisplayQty, _DisplayQty)

                total = total + _DisplayUnitPrice * _DisplayQty
            Next
        End If

        If Not btosParentList Is Nothing AndAlso btosParentList.Any() Then
            For Each item As QuoteItem In btosParentList

                _DisplayUnitPrice = 0
                _DisplayQty = 0
                Double.TryParse(item.DisplayUnitPrice, _DisplayUnitPrice)
                Integer.TryParse(item.DisplayQty, _DisplayQty)

                total = total + _DisplayUnitPrice * _DisplayQty
                'If item.newUnitPrice > 0 Then
                '    total = total + item.newUnitPrice * item.qty
                'Else
                '    total = total + item.UnitPriceX * item.qty
                'End If
            Next
        End If
        Return total
    End Function
    Public Shared Function GetQuoteLooseItems(ByVal QuoteID As String) As List(Of QuoteItem)
        If Not String.IsNullOrEmpty(QuoteID) Then

            Dim _Quotelist As List(Of QuoteItem) = GetQuoteList(QuoteID)
            Dim _returnval As New List(Of QuoteItem)

            If _Quotelist IsNot Nothing Then
                For Each _item As QuoteItem In _Quotelist
                    If _item.ItemTypeX = QuoteItemType.Part Then
                        _returnval.Add(_item)
                    End If
                Next
            End If
            Return _returnval
        End If
        Return Nothing
    End Function


    Public Shared Function GetQuoteBTOSParentItems(ByVal QuoteID As String) As List(Of QuoteItem)
        If Not String.IsNullOrEmpty(QuoteID) Then

            If Not IsHaveBtos(QuoteID) Then
                Return Nothing
            End If

            Dim _Quotelist As List(Of QuoteItem) = GetQuoteList(QuoteID)
            Dim _returnval As New List(Of QuoteItem)

            If _Quotelist IsNot Nothing Then
                For Each _item As QuoteItem In _Quotelist
                    If _item.ItemTypeX = QuoteItemType.BtosParent Then
                        _returnval.Add(_item)
                    End If
                Next
            End If
            Return _returnval
        End If
        Return Nothing
    End Function


    Public Shared Function IsHaveBtos(ByVal quoteid As String) As Boolean
        Dim _QuoteItem As QuoteItem = GetQuoteList(quoteid).Where(Function(p) p.line_No >= 100).FirstOrDefault()
        If _QuoteItem IsNot Nothing Then
            Return True
        End If
        Return False
    End Function
    Public Shared Function GetQuoteItem(ByVal QuoteID As String, ByVal LineNO As Object) As QuoteItem
        If Not String.IsNullOrEmpty(LineNO.ToString) Then
            Dim _QuoteItem As QuoteItem = GetQuoteList(QuoteID).SingleOrDefault(Function(p) p.line_No = Integer.Parse(LineNO) AndAlso p.quoteId = QuoteID)
            If _QuoteItem IsNot Nothing Then Return _QuoteItem
        End If
        Return Nothing
    End Function
    Public Shared Function GetMaxEWQuoteItem(ByVal QuoteID As String) As QuoteItem
        Dim _QuoteItem As QuoteItem = GetQuoteList(QuoteID).Where(Function(p) p.quoteId = QuoteID And p.ewFlag > 0).OrderBy(Function(p) p.ewFlag).LastOrDefault
        If _QuoteItem IsNot Nothing Then Return _QuoteItem
        Return Nothing
    End Function
    Public Shared Function DeleteQuoteItem(ByVal Quoteid As String, ByVal LineNO As Object) As Boolean
        If Not String.IsNullOrEmpty(LineNO.ToString) Then
            Try
                Dim _CurrentLing2Sql = MyUtil.Current.CurrentDataContext
                Dim _QuoteItem As QuoteItem = _CurrentLing2Sql.QuoteItems.SingleOrDefault(Function(p) p.line_No = LineNO.ToString AndAlso p.quoteId = Quoteid)
                If _QuoteItem IsNot Nothing Then
                    If _QuoteItem.ItemTypeX = QuoteItemType.BtosParent Then
                        Dim _Quotelist As List(Of QuoteItem) = _CurrentLing2Sql.QuoteItems.Where(Function(p) (p.line_No = _QuoteItem.line_No OrElse p.HigherLevel = _QuoteItem.line_No) AndAlso p.quoteId = Quoteid).ToList()
                        _CurrentLing2Sql.QuoteItems.DeleteAllOnSubmit(_Quotelist)
                    Else
                        _CurrentLing2Sql.QuoteItems.DeleteOnSubmit(_QuoteItem)
                    End If
                    _CurrentLing2Sql.SubmitChanges()
                    If _QuoteItem.ItemTypeX = QuoteItemType.Part Then
                        Dim EW_item As QuoteItem = GetQuoteItem(_QuoteItem.quoteId, _QuoteItem.line_No + 1)
                        If EW_item IsNot Nothing AndAlso EW_item.IsEWpartnoX Then
                            _CurrentLing2Sql.QuoteItems.DeleteOnSubmit(EW_item)
                            _CurrentLing2Sql.SubmitChanges()
                        End If
                    End If
                End If
                Return True
            Catch ex As Exception
                Return False
            End Try
        End If
        Return False
    End Function
    Public Shared Function ReSetLineNo(ByVal QuoteID As String) As Integer
        Dim _Quotelist As List(Of QuoteItem) = GetQuoteList(QuoteID)
        Dim _Quotelistdanpin As List(Of QuoteItem) = _Quotelist.Where(Function(p) p.ItemTypeX = QuoteItemType.Part AndAlso p.quoteId = QuoteID).OrderBy(Function(p) p.line_No).ToList()
        Dim i As Integer = 1
        For Each _Quoteitem As QuoteItem In _Quotelistdanpin
            _Quoteitem.line_No = i
            i = i + 1
        Next
        MyUtil.Current.CurrentDataContext.SubmitChanges()

        Dim _Quote_Master As Quote_Master = GetQuoteMaster(QuoteID)
        Dim _QuotelistBtosParentitems As List(Of QuoteItem) = _Quotelist.Where(Function(p) p.ItemTypeX = QuoteItemType.BtosParent AndAlso p.quoteId = QuoteID).OrderBy(Function(p) p.line_No).ToList()

        For Each _Quoteitem As QuoteItem In _QuotelistBtosParentitems
            i = _Quoteitem.line_No + 1
            Dim lineno As Integer = _Quoteitem.line_No
            Dim _ChildList As List(Of QuoteItem) = _Quoteitem.ChildListX ' _Quotelist.Where(Function(p) p.HigherLevel = lineno AndAlso p.quoteId = QuoteID).OrderBy(Function(p) p.line_No).ToList()
            For Each _Quoteine As QuoteItem In _ChildList
                _Quoteine.line_No = i
                i = i + 1
                If _Quoteine.IsEWpartnoX Then
                    '_Quoteine.listPrice = (_Quoteine.EW_RateX * _Quoteitem.ChildSubUnitPriceX) / _Quoteine.qty
                    _Quoteine.listPrice = Decimal.Round(CType((_Quoteine.EW_RateX * _Quoteitem.ChildWarrantAbleSubUnitPriceX) / _Quoteine.qty, Decimal), 2)
                    If _Quote_Master.currency.Equals("TWD", StringComparison.InvariantCultureIgnoreCase) Then
                        _Quoteine.listPrice = Math.Ceiling(_Quoteine.listPrice.GetValueOrDefault(0))
                    End If
                    _Quoteine.unitPrice = _Quoteine.listPrice
                    _Quoteine.newUnitPrice = _Quoteine.listPrice
                    _Quoteine.itp = _Quoteine.listPrice
                    _Quoteine.newItp = _Quoteine.listPrice
                End If
            Next
        Next
        MyUtil.Current.CurrentDataContext.SubmitChanges()
        Return 1
    End Function
    Public Shared Function getBtosParentLineNo(ByVal quoteid As String) As Integer
        Dim ParentLineNo As Integer = 0
        Do While True
            ParentLineNo = ParentLineNo + 100
            If CInt(
             tbOPBase.dbExecuteScalar("EQ", String.Format("select count(Line_No) as counts from {0} where quoteid='{1}' and Line_No={2}", "QuotationDetail", quoteid, ParentLineNo))
              ) = 0 Then
                Exit Do
            End If
        Loop
        Return ParentLineNo
    End Function
    Public Shared Function getMaxParentLineNo(ByVal quoteid As String) As Integer
        'Return getBtosParentLineNo(quoteid)
        Dim _MaxParentLineNo = tbOPBase.dbExecuteScalar("EQ", String.Format("select max(HigherLevel) from {0} where quoteid='{1}'", "QuotationDetail", quoteid))
        If _MaxParentLineNo Is DBNull.Value Then Return 0
        Return _MaxParentLineNo
    End Function

    Public Shared Function getMaxLooseItemLineNo(ByVal quoteid As String) As Integer
        'Return getBtosParentLineNo(quoteid)
        Dim _MaxParentLineNo = tbOPBase.dbExecuteScalar("EQ", String.Format("select max(Line_No) from {0} where line_No<100 and quoteid='{1}'", "QuotationDetail", quoteid))
        If _MaxParentLineNo Is DBNull.Value Then Return 0
        Return _MaxParentLineNo
    End Function



    Public Shared Function UpOrDownLineNo(ByVal quoteid As String, ByVal lineno As Integer, ByVal willdo As String) As Boolean
        If Not String.IsNullOrEmpty(quoteid) Then
            Dim _quotelist As List(Of QuoteItem) = GetQuoteList(quoteid)
            If _quotelist.Count > 0 Then
                Select Case willdo
                    Case "up"
                        Dim _up2quoteitem As QuoteItem = _quotelist.SingleOrDefault(Function(p) p.line_No = lineno - 2 AndAlso p.quoteId = quoteid)
                        Dim _up1quoteitem As QuoteItem = _quotelist.SingleOrDefault(Function(p) p.line_No = lineno - 1 AndAlso p.quoteId = quoteid)
                        Dim _currentquoteitem As QuoteItem = _quotelist.SingleOrDefault(Function(p) p.line_No = lineno AndAlso p.quoteId = quoteid)
                        Dim _dn1quotetem As QuoteItem = _quotelist.SingleOrDefault(Function(p) p.line_No = lineno + 1 AndAlso p.quoteId = quoteid)
                        If _currentquoteitem IsNot Nothing AndAlso _up1quoteitem IsNot Nothing AndAlso _currentquoteitem.ItemTypeX = QuoteItemType.Part Then
                            If _currentquoteitem.ewFlag > 0 Then
                                If _up1quoteitem IsNot Nothing AndAlso Not _up1quoteitem.IsEWpartnoX Then
                                    _currentquoteitem.line_No = lineno - 1
                                    _up1quoteitem.line_No = lineno + 1
                                    _dn1quotetem.line_No = lineno
                                End If
                                If _up1quoteitem IsNot Nothing AndAlso _up1quoteitem.IsEWpartnoX Then
                                    _currentquoteitem.line_No = lineno - 2
                                    _dn1quotetem.line_No = lineno - 1
                                    _up2quoteitem.line_No = lineno
                                    _up1quoteitem.line_No = lineno + 1
                                End If
                            End If
                            If _currentquoteitem.ewFlag = 0 Then
                                If _up1quoteitem IsNot Nothing AndAlso Not _up1quoteitem.IsEWpartnoX Then
                                    _currentquoteitem.line_No = lineno - 1
                                    _up1quoteitem.line_No = lineno
                                End If
                                If _up1quoteitem IsNot Nothing AndAlso _up1quoteitem.IsEWpartnoX Then
                                    _currentquoteitem.line_No = lineno - 2
                                    _up2quoteitem.line_No = lineno - 1
                                    _up1quoteitem.line_No = lineno
                                End If
                            End If
                        End If
                        If _currentquoteitem IsNot Nothing AndAlso _currentquoteitem.ItemTypeX = QuoteItemType.BtosPart Then
                            If _up1quoteitem IsNot Nothing Then
                                If _up1quoteitem.ItemTypeX <> QuoteItemType.BtosParent Then
                                    _currentquoteitem.line_No = lineno - 1
                                    _up1quoteitem.line_No = lineno
                                End If
                            End If
                        End If

                    Case "down"

                        Dim _currentquoteitem As QuoteItem = _quotelist.SingleOrDefault(Function(p) p.line_No = lineno AndAlso p.quoteId = quoteid)
                        Dim _dn1quotetem As QuoteItem = _quotelist.SingleOrDefault(Function(p) p.line_No = lineno + 1 AndAlso p.quoteId = quoteid)
                        Dim _dn2quoteitem As QuoteItem = _quotelist.SingleOrDefault(Function(p) p.line_No = lineno + 2 AndAlso p.quoteId = quoteid)
                        Dim _dn3quoteitem As QuoteItem = _quotelist.SingleOrDefault(Function(p) p.line_No = lineno + 3 AndAlso p.quoteId = quoteid)
                        If _currentquoteitem IsNot Nothing AndAlso _currentquoteitem.ItemTypeX = QuoteItemType.Part Then
                            If _currentquoteitem.ewFlag > 0 Then
                                If _dn2quoteitem IsNot Nothing AndAlso _dn2quoteitem.ewFlag = 0 Then
                                    _dn2quoteitem.line_No = lineno
                                    _currentquoteitem.line_No = lineno + 1
                                    _dn1quotetem.line_No = lineno + 2
                                End If
                                If _dn2quoteitem IsNot Nothing AndAlso _dn2quoteitem.ewFlag > 0 Then
                                    _dn2quoteitem.line_No = lineno
                                    _dn3quoteitem.line_No = lineno + 1
                                    _currentquoteitem.line_No = lineno + 2
                                    _dn1quotetem.line_No = lineno + 3
                                End If
                            End If
                            If _currentquoteitem.ewFlag = 0 AndAlso _dn1quotetem IsNot Nothing Then
                                If _dn1quotetem.ewFlag = 0 Then
                                    _dn1quotetem.line_No = lineno
                                    _currentquoteitem.line_No = lineno + 1
                                End If
                                If _dn1quotetem.ewFlag > 0 Then
                                    _dn1quotetem.line_No = lineno
                                    _dn2quoteitem.line_No = lineno + 1
                                    _currentquoteitem.line_No = lineno + 2
                                End If
                            End If

                        End If
                        If _currentquoteitem IsNot Nothing AndAlso _currentquoteitem.ItemTypeX = QuoteItemType.BtosPart Then
                            If _dn1quotetem IsNot Nothing AndAlso Not _dn1quotetem.IsEWpartnoX Then
                                _currentquoteitem.line_No = lineno + 1
                                _dn1quotetem.line_No = lineno
                            End If
                        End If
                End Select
                MyUtil.Current.CurrentDataContext.SubmitChanges()
            End If
        End If
        Return False
    End Function

    Public Shared Function GetTotalListPrice(ByVal QuoteID As String) As Decimal
        Dim _Quotelist As List(Of QuoteItem) = MyQuoteX.GetQuoteList(QuoteID)
        If _Quotelist.Count > 0 Then
            Return _Quotelist.Sum(Function(p) p.listPrice * (p.qty))
        End If
        Return 0
    End Function

    Public Shared Function GetTotalPrice(ByVal QuoteID As String) As Decimal
        Dim _Quotelist As List(Of QuoteItem) = MyQuoteX.GetQuoteList(QuoteID)
        If _Quotelist.Count > 0 Then
            Return _Quotelist.Sum(Function(p) p.newUnitPrice * (p.qty))
        End If
        Return 0
    End Function
    Public Shared Sub LogQuoteMasterExtension(ByVal Master_Extension As Quote_Master_Extension)
        If Master_Extension IsNot Nothing Then
            Dim _ME As Quote_Master_Extension = MyUtil.Current.CurrentDataContext.Quote_Master_Extensions.Where(Function(p) p.QuoteID = Master_Extension.QuoteID).FirstOrDefault()
            If _ME IsNot Nothing Then
                _ME.QuoteID = Master_Extension.QuoteID
                _ME.EmailGreeting = Master_Extension.EmailGreeting
                _ME.SpecialTandC = Master_Extension.SpecialTandC
                _ME.SignatureRowID = Master_Extension.SignatureRowID
                _ME.ApprovalFlowType = Master_Extension.ApprovalFlowType
                _ME.LastUpdatedBy = Pivot.CurrentProfile.UserId
                _ME.LastUpdated = Now
                _ME.Engineer = Master_Extension.Engineer
                _ME.Engineer_Telephone = Master_Extension.Engineer_Telephone
                _ME.Warranty = Master_Extension.Warranty
                _ME.ABRQuoteType = Master_Extension.ABRQuoteType
                _ME.JPCustomerOffice = Master_Extension.JPCustomerOffice

                If Not Master_Extension.CopyPurpose Is Nothing AndAlso Master_Extension.CopyPurpose.HasValue Then
                    _ME.CopyPurpose = Master_Extension.CopyPurpose
                Else
                    _ME.CopyPurpose = 0
                End If

                If _ME.IsShowTotal IsNot Nothing AndAlso _ME.IsShowTotal = True Then
                    _ME.IsShowTotal = 1
                Else
                    _ME.IsShowTotal = 0
                End If
            Else
                Master_Extension.IsShowTotal = 0
                MyUtil.Current.CurrentDataContext.Quote_Master_Extensions.InsertOnSubmit(Master_Extension)
            End If
            MyUtil.Current.CurrentDataContext.SubmitChanges()
        End If
    End Sub
    Public Shared Function GetMasterExtension(ByVal QuoteID As String) As Quote_Master_Extension
        If Not String.IsNullOrEmpty(QuoteID) Then
            Dim _ME As Quote_Master_Extension = MyUtil.Current.CurrentDataContext.Quote_Master_Extensions.Where(Function(p) p.QuoteID = QuoteID).FirstOrDefault()
            If _ME IsNot Nothing Then
                Return _ME
            End If
        End If
        Return Nothing
    End Function
    'Public Shared Sub RefreshBTOSMainitemAvailableDate(ByVal _QuoteId As String)
    '    'Dim myQD As New EQDSTableAdapters.QuotationDetailTableAdapter
    '    'myQD.UpdateBTOSMainItemDueDate(_QuoteId)

    '    'update qd1 set qd1.dueDate=isnull((select max(duedate) as dueDate from QuotationDetail where 
    '    'quoteid=@quoteid and HigherLevel=qd1.line_No),qd1.dueDate)
    '    'from [QuotationDetail] as qd1
    '    'where qd1.quoteId=@quoteid
    '    'and qd1.HigherLevel=0

    '    Dim _QuoteMaster As Quote_Master = MyQuoteX.GetQuoteMaster(_QuoteId)

    '    Dim _QuoteDetailDT As List(Of QuoteItem) = MyQuoteX.GetQuoteList(_QuoteId)
    '    For Each _QuoteItem As QuoteItem In _QuoteDetailDT
    '        If _QuoteItem.line_No Mod 100 = 0 Then

    '        End If
    '    Next

    'End Sub
    Public Shared Sub RefreshPartInventory(ByVal _QuoteId As String, ByVal _lineNumber As Integer)

        'Frank 這邊呼叫Role.IsAonlineUsa會影響到歐洲簽核程式，所以改用quoteno判斷
        Dim _QuoteMaster As Quote_Master = MyQuoteX.GetQuoteMaster(_QuoteId)

        'Jay 20150812
        'Sorry for asking you making another change.  Please go ahead to roll back this change at cart item
        ', but keep the new enhancement at providing ATP visibility of both US and part’s supplying plant inventory.
        'Cathee 20150812
        'Hi Frank
        'This is probably not a good idea.  We should show available date based on inventory in USH1 only.  If we show available date based on other plants we will have a lot of miscommunication with the customer and wrong information will be given. 
        ''If Role.IsAonlineUsa Or Role.IsAonlineUsaIag Then
        'If _QuoteMaster.quoteNo.StartsWith("AUSQ", StringComparison.InvariantCultureIgnoreCase) Then
        '    Advantech.Myadvantech.Business.QuoteBusinessLogic.RefreshPartInventoryOfUSAOnline(_QuoteId, _lineNumber)
        '    'MyUtil.Current.CurrentDataContext = Nothing
        '    Exit Sub
        'End If

        'Frank 2012/09/18
        '1.component
        ' 1.1 if available qty can not be found，then available date need to be changed to 12/31/9999；set available qty=0
        ' 1.2 if available qty is not enough for request qty, then available date need to be changed to 12/31/9999；and set available qty as real stock
        '2.BTOS 100 line：No need to query available qty，but available date need to be updated by lastest available date(due date) of sub components；and set available qty as request qty
        '3.If part's pref string is AGS-：No need to query available qty and set available date as today, set available qty as request qty

        '!!!!!!!!Please do not use session in this function!!!!!!!!!!!!!


        'Dim _QuoteDetailDT As List(Of QuoteItem) = MyQuoteX.GetQuoteList(_QuoteId)
        Dim _QuoteDetailDT As List(Of QuoteItem) = Nothing
        If _lineNumber > 0 Then
            '_QuoteDetailDT = _QuoteDetailDT.Where(Function(p) p.line_No = _lineNumber)
            _QuoteDetailDT = New List(Of QuoteItem)
            _QuoteDetailDT.Add(MyQuoteX.GetQuoteItem(_QuoteId, _lineNumber))
        Else
            _QuoteDetailDT = MyQuoteX.GetQuoteList(_QuoteId)
        End If

        Dim prod_input As New SAPDAL.SAPDALDS.ProductInDataTable, _sapdal As New SAPDAL.SAPDAL, _errormsg As String = String.Empty
        Dim _deliveryPlant As String = "USH1", inventory_out As New SAPDAL.SAPDALDS.QueryInventory_OutputDataTable

        'Reading partno and require qty, fill into product in table
        For Each _QuoteItem As QuoteItem In _QuoteDetailDT
            If String.IsNullOrEmpty(_QuoteItem.partNo) Then Continue For
            If COMM.CartFixer.isValidParentLineNo(_QuoteItem.line_No) Then Continue For
            prod_input.AddProductInRow(_QuoteItem.partNo, _QuoteItem.qty, _QuoteItem.deliveryPlant)
        Next
        'Get real time inventory
        _sapdal.QueryInventory_V2(prod_input, _deliveryPlant, Now, inventory_out, _errormsg)
        Dim _MatchInventoryRow() As SAPDAL.SAPDALDS.QueryInventory_OutputRow = Nothing, _STOCK_DATE As String = String.Empty, _STOCK_DATE_NEXTHOLIDAY As Date = Nothing, _code As String = "TW"
        Dim _StockTotalValue As Integer = 0

        Dim _codebyorgid As String = "TW"
        If Not String.IsNullOrEmpty(_QuoteMaster.org) AndAlso _QuoteMaster.org.Length > 2 Then
            _codebyorgid = SAPDAL.SAPDAL.GetCalendarIDbyOrg(_QuoteMaster.org.Substring(0, 2))
        End If

        For Each _QuoteItem As QuoteItem In _QuoteDetailDT

            If String.IsNullOrEmpty(_QuoteItem.partNo) OrElse _QuoteItem.ItemTypeX = QuoteItemType.BtosParent Then Continue For

            If _QuoteItem.dueDate Is Nothing OrElse Date.TryParse(_QuoteItem.dueDate, Now) = False Then _QuoteItem.dueDate = Now.Date

            'If part number start with AGS- , then just updating due date as today
            'If _QuoteItem.partNo.StartsWith("AGS-", StringComparison.InvariantCultureIgnoreCase) Then
            'Frank 20150904:Check service part by new rule, using GenItemCatGroup=DIEN in SAP
            If Advantech.Myadvantech.Business.PartBusinessLogic.IsServicePart(_QuoteItem.partNo, _QuoteMaster.org) Then
                _QuoteItem.dueDate = Now.Date
                Continue For
            End If

            'If part's inventory cannot be found.
            If inventory_out.Rows.Count = 0 Then
                If (_QuoteMaster.DocReg And COMM.Fixer.eDocReg.AUS) = _QuoteMaster.DocReg Then
                    _QuoteItem.inventory = 0
                    _QuoteItem.dueDate = CDate("9999/12/31")
                    Continue For
                Else
                    _QuoteItem.inventory = 0
                    _QuoteItem.dueDate = Now.Date.AddDays(SAPTools.getLeadTime(_QuoteItem.partNo, _QuoteItem.deliveryPlant))
                    Continue For
                End If
            End If

            _MatchInventoryRow = inventory_out.Select("PART_NO='" & _QuoteItem.partNo & "' and PLANT='" & _QuoteItem.deliveryPlant & "'")

            If _MatchInventoryRow.Length > 0 Then
                'According to the quoteid and line no, update its inventory and duedate
                'Ryan 20160304 Set .Equals(CDate(_MatchInventoryRow(_MatchInventoryRow.Count - 1), Original is _MatchInventoryRow(0)
                If Not _STOCK_DATE.Equals(CDate(_MatchInventoryRow(_MatchInventoryRow.Count - 1).STOCK_DATE).ToString("yyyy-MM-dd"), StringComparison.InvariantCultureIgnoreCase) Then
                    _STOCK_DATE = CDate(_MatchInventoryRow(_MatchInventoryRow.Count - 1).STOCK_DATE).ToString("yyyy-MM-dd")
                    _STOCK_DATE_NEXTHOLIDAY = _STOCK_DATE

                    'Frank 2015/02/24
                    If Not String.IsNullOrEmpty(_QuoteItem.deliveryPlant) Then
                        _code = SAPDAL.SAPDAL.GetCalendarIDbyPlant(_QuoteItem.deliveryPlant)
                    Else
                        _code = _codebyorgid
                    End If
                    SAPDAL.SAPDAL.Get_Next_WorkingDate_ByCode(_STOCK_DATE_NEXTHOLIDAY, 0, _code)
                End If

                'Calculate total available qty
                _StockTotalValue = 0
                _StockTotalValue = inventory_out.Compute("Sum(STOCK)", "PART_NO = '" & _QuoteItem.partNo & "'")

                If _StockTotalValue >= _QuoteItem.qty Then
                    _QuoteItem.inventory = _StockTotalValue
                    _QuoteItem.dueDate = _STOCK_DATE_NEXTHOLIDAY
                Else
                    If (_QuoteMaster.DocReg And COMM.Fixer.eDocReg.AUS) = _QuoteMaster.DocReg Then
                        _QuoteItem.inventory = _StockTotalValue
                        _QuoteItem.dueDate = CDate("9999/12/31")
                    Else
                        _QuoteItem.inventory = _StockTotalValue
                        _QuoteItem.dueDate = Now.Date.AddDays(SAPTools.getLeadTime(_QuoteItem.partNo, _QuoteItem.deliveryPlant))
                        Continue For
                    End If

                End If
            Else
                If (_QuoteMaster.DocReg And COMM.Fixer.eDocReg.AUS) = _QuoteMaster.DocReg Then
                    _QuoteItem.inventory = 0
                    _QuoteItem.dueDate = CDate("9999/12/31")
                Else
                    _QuoteItem.inventory = 0
                    _QuoteItem.dueDate = Now.Date.AddDays(SAPTools.getLeadTime(_QuoteItem.partNo, _QuoteItem.deliveryPlant))
                    Continue For
                End If
            End If
        Next
        ' RefreshBTOSMainitemAvailableDate
        If MyQuoteX.IsHaveBtos(_QuoteId) Then
            Dim childlist As List(Of QuoteItem) = Nothing
            For Each _QuoteItem As QuoteItem In _QuoteDetailDT
                childlist = Nothing
                If _QuoteItem.ItemTypeX = QuoteItemType.BtosParent Then
                    childlist = _QuoteItem.ChildListX
                    If childlist IsNot Nothing AndAlso childlist.Count > 0 AndAlso Not _QuoteMaster.org = "US01" Then
                        _QuoteItem.dueDate = childlist.Max(Function(p) p.dueDate)
                    End If
                End If
            Next
        End If
        MyUtil.Current.CurrentDataContext.SubmitChanges()
    End Sub
    'Public Shared Sub LogMasterExtension(ByVal QM_Extension As Quote_Master_Extension)
    '    Dim _QMExtension As Quote_Master_Extension = GetMasterExtension(QM_Extension.QuoteID)
    '    If _QMExtension IsNot Nothing Then
    '        _QMExtension.QuoteID = QM_Extension.QuoteID
    '        _QMExtension.SignatureRowID = QM_Extension.SignatureRowID
    '        _QMExtension.IsShowTotal = QM_Extension.IsShowTotal
    '        _QMExtension.LastUpdated = Now
    '        _QMExtension.SpecialTandC = QM_Extension.SpecialTandC
    '        _QMExtension.EmailGreeting = QM_Extension.EmailGreeting
    '        _QMExtension.Engineer = QM_Extension.Engineer
    '        _QMExtension.Engineer_Telephone = QM_Extension.Engineer_Telephone
    '        _QMExtension.LastUpdatedBy = Pivot.CurrentProfile.UserId
    '    Else
    '        MyUtil.Current.CurrentDataContext.Quote_Master_Extensions.InsertOnSubmit(QM_Extension)
    '    End If
    '    MyUtil.Current.CurrentDataContext.SubmitChanges()
    'End Sub

    Public Shared Sub ExportQuoteToExcel(ByVal QuoteID As String)
        Dim _IQueryable = From p As QuoteItem In MyUtil.Current.CurrentDataContext.QuoteItems Where p.quoteId = QuoteID Order By p.line_No
                  Select New With {
                      .Line_No = p.line_No,
                      .PartNo = p.partNo,
                      .QTY = p.qty,
                      .ListPrice = p.ListPriceX,
                      .UnitPrice = p.newUnitPrice,
                      .ExtendedPrice = p.newUnitPrice * p.qty,
                      .Discount = CalculateDiscount(p.ListPriceX, p.newUnitPrice),
                      .Description = p.description,
                      .DeliveryPlant = p.deliveryPlant,
                      .HigherLevel = p.HigherLevel,
                      .ReqDate = p.reqDate,
                      .DueDate = p.dueDate}
        Dim _list = _IQueryable.ToList()
        Util.List2ExcelDownload(_list, "quoteDetail")
    End Sub

    Protected Shared Function CalculateDiscount(ByVal _ListPrice As Decimal, ByVal _UnitPrice As Decimal) As String
        Dim Discount As Decimal = 0
        If _ListPrice = 0 AndAlso _UnitPrice = 0 Then Return "0%"
        If _ListPrice < _UnitPrice Then Return "0%"

        Discount = ((_ListPrice - _UnitPrice) / _ListPrice) * 100

        Return Discount.ToString("F") & "%"
    End Function

    Public Shared Function GetSoNoByQuoteID(ByVal QuoteID As String, ByRef SoNo As String) As Boolean
        Dim dt As DataTable = tbOPBase.dbGetDataTable("EQ", String.Format("select top 1 SO_NO,PO_NO,convert(varchar, ORDER_DATE, 111) as ORDER_DATE from QUOTE_TO_ORDER_LOG z where z.QUOTEID='{0}' and SO_NO is not null and SO_NO <> '' order by z.ORDER_DATE desc ", QuoteID))
        If dt.Rows.Count = 1 Then
            If Not IsNothing(dt.Rows(0).Item("SO_NO")) Then
                SoNo = dt.Rows(0).Item("SO_NO")
            End If
            Return True
        End If
        Return False
    End Function
    Public Shared Function getMultiPrice(ByVal QuoteID As String, ByVal PartNOs As List(Of PNInfo)) As Order

        Dim _QMaster As QuotationMaster = eQuotationContext.Current.QuotationMaster.Find(QuoteID)

        'eQuotationContext.Current.QuotationExtension

        Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(QuoteID)

        If _QMaster Is Nothing Then Return Nothing
        Dim _order As New Order()
        _order.Currency = _QMaster.currency
        _order.DistChannel = _QMaster.DIST_CHAN
        _order.Division = _QMaster.DIVISION
        _order.OrgID = _QMaster.org

        If _ME IsNot Nothing Then
            If Not String.IsNullOrEmpty(_ME.ABRQuoteType) Then
                Dim _ordertype As SAPOrderType = SAPOrderType.ZOR
                If [Enum].TryParse(Of SAPOrderType)(_ME.ABRQuoteType, _ordertype) Then
                    _order.OrderType = _ordertype
                End If
            End If
        End If


        Dim i As Integer = 0
        For Each pn In PartNOs
            _order.AddLooseItem(pn.partno, pn.qty)
            _order.LineItems(i).PlantID = pn.plant
            i = i + 1
        Next
        Dim apt As New EQDSTableAdapters.EQPARTNERTableAdapter
        Dim dtPartners As EQDS.EQPARTNERDataTable = apt.GetPartnersByQuoteId(QuoteID)
        For Each PartnerRow As EQDS.EQPARTNERRow In dtPartners
            Select Case PartnerRow.TYPE
                Case "S"
                    _order.SetOrderPartnet(New OrderPartner(PartnerRow.ERPID, _QMaster.org, OrderPartnerType.ShipTo))
                Case "B"
                    _order.SetOrderPartnet(New OrderPartner(PartnerRow.ERPID, _QMaster.org, OrderPartnerType.BillTo))
                Case "SOLDTO"
                    _order.SetOrderPartnet(New OrderPartner(PartnerRow.ERPID, _QMaster.org, OrderPartnerType.SoldTo))

            End Select
        Next
        If dtPartners.Rows.Count = 0 Then
            _order.SetOrderPartnet(New OrderPartner(_QMaster.quoteToErpId, _QMaster.org, OrderPartnerType.ShipTo))
            _order.SetOrderPartnet(New OrderPartner(_QMaster.quoteToErpId, _QMaster.org, OrderPartnerType.BillTo))
            _order.SetOrderPartnet(New OrderPartner(_QMaster.quoteToErpId, _QMaster.org, OrderPartnerType.SoldTo))
        End If
        Dim _errmsg As String = String.Empty
        Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(_order, _errmsg)
        Return _order
    End Function

End Class
Public Class PNInfo
    Public Property partno As String : Public Property qty As Integer : Public Property plant As String
    Public Sub New(ByVal p As String, ByVal q As Integer, ByVal pl As String)
        Me.partno = p : Me.qty = q : Me.plant = pl
    End Sub
End Class
Partial Public Class QuoteItem

    Private _minimumprice As Decimal = 0

    Property MinimumPrice As Decimal
        Get
            Return _minimumprice
        End Get
        Set(ByVal value As Decimal)
            _minimumprice = value
        End Set
    End Property

    Public ReadOnly Property IsBelowMinimumPrice As Boolean
        Get
            If UnitPriceX < _minimumprice AndAlso _minimumprice > 0 AndAlso unitPrice > 0 Then
                Return True
            End If
            Return False
        End Get
    End Property


    Public ReadOnly Property ItemTypeX As QuoteItemType
        Get
            If IsNumeric(Me.ItemType) Then
                'If [Enum].IsDefined(GetType(QuoteItemType), Me.oType) Then
                '    Return CType([Enum].ToObject(GetType(QuoteItemType), Me.oType), QuoteItemType)
                'End If
                If Me.ItemType = 1 Then Return QuoteItemType.BtosParent
                If Me.ItemType = 0 Then
                    If IsNumeric(Me.HigherLevel) AndAlso Me.HigherLevel > 0 Then Return QuoteItemType.BtosPart
                Else
                    Return QuoteItemType.Part
                End If
            End If
            Return QuoteItemType.Part
        End Get
    End Property
    Public ReadOnly Property IsEWpartnoX As Boolean
        Get
            If Me.partNo.StartsWith("AGS-EW", StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            End If
            Return False
        End Get
    End Property
    Public ReadOnly Property EW_RateX As Decimal
        Get
            If Me.IsEWpartnoX Then

                'Ryan 20170306 Get current profile for AUS to select EWrate
                Dim DocReg As COMM.Fixer.eDocReg = Pivot.CurrentProfile.CurrDocReg
                Dim Salesgroup As String = String.Empty
                If (DocReg And COMM.Fixer.eDocReg.AUS) = DocReg Then
                    If DocReg = COMM.Fixer.eDocReg.AENC OrElse DocReg = COMM.Fixer.eDocReg.AAC Then
                        Salesgroup = "KA/CP"
                    Else
                        Salesgroup = "AOnline"
                    End If
                End If

                Dim _sql As New StringBuilder
                _sql.AppendFormat("select top 1 EW_Rate  from  dbo.ExtendedWarrantyPartNo_V2 where EW_PartNO='{0}' and  Plant ='{1}'", Me.partNo, Me.deliveryPlant)
                If Not String.IsNullOrEmpty(Salesgroup) Then _sql.AppendFormat(" and SalesGroup = '{0}' ", Salesgroup)
                _sql.AppendLine(" order by SeqNO ")

                Dim dt As DataTable = tbOPBase.dbGetDataTable("MY", _sql.ToString)
                If dt.Rows.Count > 0 Then
                    Dim EW_Rate As Decimal = 0
                    If Not IsDBNull(dt.Rows(0).Item("EW_Rate")) AndAlso Decimal.TryParse(dt.Rows(0).Item("EW_Rate"), 0) Then
                        EW_Rate = Decimal.Parse(dt.Rows(0).Item("EW_Rate"))
                    End If
                    Return EW_Rate
                End If
            End If
            Return 0
        End Get
    End Property
    Public ReadOnly Property ChildListX As List(Of QuoteItem)
        Get
            If Me.ItemTypeX = QuoteItemType.BtosParent Then
                Dim _Quotelist As List(Of QuoteItem) = MyQuoteX.GetQuoteList(Me.quoteId)
                If _Quotelist.Count > 0 Then
                    Dim _ChildList As List(Of QuoteItem) = _Quotelist.Where(Function(p) p.HigherLevel = Me.line_No AndAlso p.quoteId = Me.quoteId).OrderBy(Function(p) p.line_No).ToList()
                    If _ChildList.Count > 0 Then
                        Return _ChildList
                    End If
                End If
            End If
            Return New List(Of QuoteItem)
        End Get
    End Property
    Public ReadOnly Property ListPriceX As Decimal
        Get
            If Me.ItemTypeX = QuoteItemType.BtosParent Then
                Return Me.ChildSubListPriceX
            Else
                Return Me.listPrice
            End If
            Return 0
        End Get
    End Property
    Public ReadOnly Property UnitPriceX As Decimal
        Get
            If Me.ItemTypeX = QuoteItemType.BtosParent Then
                Return Me.ChildSubUnitPriceX / Me.qty
            Else
                Return Me.newUnitPrice
            End If
            Return 0
        End Get
    End Property
    Public ReadOnly Property UnitPriceWithWarrantX As Decimal
        Get
            If Me.ItemTypeX = QuoteItemType.BtosParent Then
                Return Me.ChildSubUnitPriceWithWarrantX / Me.qty
            Else
                Return Me.newUnitPrice
            End If
            Return 0
        End Get
    End Property
    Public ReadOnly Property SubTotalX As Decimal
        Get
            If Me.ItemTypeX = QuoteItemType.BtosParent Then
                Return Me.ChildSubUnitPriceX
            Else
                Return Me.newUnitPrice * Me.qty
            End If
            Return 0
        End Get
    End Property
    Public ReadOnly Property SubTotalWithWarrantX As Decimal
        Get
            If Me.ItemTypeX = QuoteItemType.BtosParent Then
                Return Me.ChildSubUnitPriceWithWarrantX
            Else
                Return Me.newUnitPrice * Me.qty
            End If
            Return 0
        End Get
    End Property

    Public ReadOnly Property ChildSubListPriceX As Decimal
        Get
            If Me.ItemTypeX = QuoteItemType.BtosParent Then
                If Me.ChildListX.Count > 0 Then
                    Dim _SubListPrice As Decimal = 0
                    _SubListPrice = Me.ChildListX.Sum(Function(p) p.listPrice)
                    Return _SubListPrice
                End If
            End If
            Return 0
        End Get
    End Property
    Public ReadOnly Property ChildSubUnitPriceX As Decimal
        Get
            If Me.ItemTypeX = QuoteItemType.BtosParent Then
                Dim _CurrentLing2Sql As Ling2SqlDataContext = New Ling2SqlDataContext()
                Dim _ChildList As List(Of QuoteItem) = _CurrentLing2Sql.QuoteItems.Where(Function(p) p.HigherLevel = Me.line_No AndAlso p.quoteId = Me.quoteId).OrderBy(Function(p) p.line_No).ToList()
                If _ChildList.Count > 0 Then
                    'Me.List_Price = _cartlistBtosChild.Sum(Function(p) p.List_Price)
                    'Return _ChildList.Sum(Function(p) p.newUnitPrice * (p.qty / Me.qty))
                    ' Return _ChildList.Sum(Function(p) p.newUnitPrice * p.qty)
                    Dim _SubNewUnitPrice As Decimal = 0
                    For Each _quoteline As QuoteItem In _ChildList
                        If Not _quoteline.IsEWpartnoX Then
                            _SubNewUnitPrice += _quoteline.newUnitPrice * _quoteline.qty
                        End If
                    Next
                    Return _SubNewUnitPrice
                End If
            End If
            Return 0
        End Get
    End Property
    Public ReadOnly Property ChildSubUnitPriceWithWarrantX As Decimal
        Get
            If Me.ItemTypeX = QuoteItemType.BtosParent Then
                Dim _CurrentLing2Sql As Ling2SqlDataContext = New Ling2SqlDataContext()
                Dim _ChildLists As List(Of QuoteItem) = _CurrentLing2Sql.QuoteItems.Where(Function(p) p.HigherLevel = Me.line_No AndAlso p.quoteId = Me.quoteId).OrderBy(Function(p) p.line_No).ToList()
                If _ChildLists.Count > 0 Then
                    Dim _SubNewUnitPrice As Decimal = 0
                    For Each _quoteline As QuoteItem In _ChildLists
                        _SubNewUnitPrice += _quoteline.newUnitPrice * _quoteline.qty
                    Next
                    Return _SubNewUnitPrice
                End If
            End If
            Return 0
        End Get
    End Property
    Public ReadOnly Property ChildWarrantAbleSubUnitPriceX As Decimal
        Get
            If Me.ItemTypeX <> QuoteItemType.BtosParent Then Return 0
            If String.Equals("EU10", MyUtil.Current.CurrentOrgID) Then
                Return Me.ChildSubUnitPriceX
            End If
            Dim _ChildList As List(Of QuoteItem) = MyUtil.Current.CurrentDataContext.QuoteItems.Where(Function(p) p.HigherLevel = Me.line_No AndAlso p.quoteId = Me.quoteId).OrderBy(Function(p) p.line_No).ToList()
            If _ChildList.Count > 0 Then
                Dim _SubNewUnitPrice As Decimal = 0, PN As String = String.Empty
                Dim DT As DataTable = Nothing
                For Each _quoteitem As QuoteItem In _ChildList

                    If _quoteitem.IsEWpartnoX Then Continue For

                    PN = _quoteitem.partNo
                    'Ryan 20170622 Comment below old EW logic out
                    'If PN.StartsWith("AGS") OrElse PN.StartsWith("OPTION") OrElse PN.StartsWith("96SW") OrElse PN.StartsWith("98MQ") OrElse PN.StartsWith("968Q") Then
                    '    Continue For
                    'End If
                    'DT = tbOPBase.dbGetDataTable("MY", String.Format("select top 1  PRODUCT_LINE  from SAP_PRODUCT where PART_NO='{0}'", PN))
                    'If DT.Rows.Count > 0 AndAlso DT.Rows(0).Item("PRODUCT_LINE") IsNot Nothing AndAlso (DT.Rows(0).Item("PRODUCT_LINE") = "EPCS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "EDOS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WCOM" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "WAUT" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "DAAS" OrElse DT.Rows(0).Item("PRODUCT_LINE") = "ASS#") Then
                    '    Continue For
                    'End If

                    If String.Equals("JP01", MyUtil.Current.CurrentOrgID) Then
                        If SAPDAL.CommonLogic.isSoftwareV3(PN) Then
                            Continue For
                        ElseIf PN.ToUpper.StartsWith("XAJP") Then
                            Continue For
                        End If
                    Else
                        If SAPDAL.CommonLogic.isSoftwareV3(PN) Then
                            Continue For
                        End If
                    End If

                    _SubNewUnitPrice += _quoteitem.newUnitPrice * _quoteitem.qty
                Next
                Return _SubNewUnitPrice
            End If
            Return 0
        End Get
    End Property
End Class
Public Enum QuoteItemType
    BtosParent = -1
    Part = 0
    BtosPart = 1
End Enum
