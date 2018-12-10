Public Class CNAOnlineQuoteTemplate
    Inherits System.Web.UI.UserControl

    Public Property QuoteId As String
        Get
            Return _QuoteId
        End Get
        Set(ByVal value As String)
            _QuoteId = value
        End Set
    End Property
    Private _UID As String = ""
    Public ReadOnly Property UID As String
        Get
            Return QuoteId
        End Get
    End Property
    Private _MasterRef As IBUS.iDocHeaderLine = Nothing
    Public ReadOnly Property MasterRef As IBUS.iDocHeaderLine
        Get
            If IsNothing(_MasterRef) Then
                _MasterRef = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
            End If
            Return _MasterRef
        End Get
    End Property

    Public Property currencySign As String
        Get
            Return _currencySign
        End Get
        Set(ByVal value As String)
            _currencySign = value
        End Set
    End Property

    Public Property IsInternalUser As Boolean
        Get
            Return _IsInternalUserMode
        End Get
        Set(ByVal value As Boolean)
            _IsInternalUserMode = value
        End Set
    End Property

    Public _QuoteNo As String = String.Empty

    Public _currencySign As String = "", _QuoteId As String = "", _IsInternalUserMode As Boolean = True, _IsBTosQuotation As Boolean = False
    Public _lbExternalNote As String = ""
    Dim _TaxRate As Decimal = CDec(0.0)
    'Dim DTList As IBUS.iCartList = Nothing
    Dim dtQuoteDetail As List(Of QuoteItem) = Nothing
    Dim _CurrentParentItemQty As Integer = 0
    Dim _CurrentPage As Integer = 1


    Public Sub LoadData()


        Dim dtM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteId, COMM.Fixer.eDocType.EQ)
        Me._QuoteNo = dtM.quoteNo
        Me._MasterRef = dtM
        'Dim dtDetail As IBUS.iCartList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, _QuoteId, dtM.org).GetListAll(COMM.Fixer.eDocType.EQ)


        If Not IsNothing(dtM) Then

            _currencySign = Util.getCurrSign(dtM.currency)

            If _currencySign.Equals("$") Then
                _currencySign = "US" & _currencySign
            End If

            _TaxRate = dtM.tax
            Me.dtQuoteDetail = MyQuoteX.GetQuoteList(_QuoteId)
            Me.gv1.Attributes("style") = "border-collapse:separate"
            Me.gv1.DataSource = Me.dtQuoteDetail
            Me.gv1.DataBind()

            '    Dim QM As IBUS.iDocHeader = New DOCH.DocHeader
            '    Me._IsV2_0Quot = QM.IsV2_0Quotation(_QuoteId)

            '    _currencySign = Util.getCurrSign(dtM.currency) : _IsLumpSumOnly = dtM.isLumpSumOnly
            FillQuoteMasterInfo(dtM)
        End If
    End Sub

    Protected Sub FillQuoteMasterInfo(ByRef QuoteMasterRow As IBUS.iDocHeaderLine)

        Me.litAccountName.Text = QuoteMasterRow.AccName

        'Dim apt As New EQDSTableAdapters.EQPARTNERTableAdapter
        'Dim SoldToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "SOLDTO")
        'Dim ShipToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "S")
        'Dim BillToTable As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "B")
        'If SoldToTable.Count > 0 Then
        '    Dim SoldToRow As EQDS.EQPARTNERRow = SoldToTable(0)
        '    Me.litAccountContactAddress.Text = SoldToRow.STREET
        '    Me.LitAccountContactTEL.Text = SoldToRow.TEL
        '    Me.LitAccountContactFAX.Text = SoldToRow.FAX
        '    Me.litAccountContactPerson.Text = SoldToRow.ATTENTION
        '    Me.LitAccountERPID.Text = SoldToRow.ERPID.TrimStart("T")
        'End If

        Dim _dt As DataTable = Nothing

        _dt = SiebelTools.GET_Account_Info_By_ID(QuoteMasterRow.AccRowId)
        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
            Me.litAccountContactAddress.Text = _dt.Rows(0).Item("Address")
        End If

        If Not String.IsNullOrEmpty(QuoteMasterRow.attentionRowId) Then
            _dt = SiebelTools.GET_Contact_Info_by_RowID(QuoteMasterRow.attentionRowId)
            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then

                Me.LitAccountContactTEL.Text = _dt.Rows(0).Item("WorkPhone").ToString.Split(Chr(10))(0)
                Me.LitAccountContactFAX.Text = _dt.Rows(0).Item("FaxNumber").ToString.Split(Chr(10))(0)
                Me.litAccountContactPerson.Text = _dt.Rows(0).Item("LastName") & _dt.Rows(0).Item("FirstName")

            End If
        End If


        Me.LitAccountERPID.Text = QuoteMasterRow.AccErpId.TrimStart("T")

        Me.LitQuoteDate.Text = QuoteMasterRow.ReqDate
        'ACL AOnline team:Quote PDF的報價單號改成顯示quote Desc.
        'Me.litQuoteNo.Text = QuoteMasterRow.quoteNo
        Me.litQuoteNo.Text = QuoteMasterRow.CustomId

        Me.LitQuoteVersion.Text = QuoteMasterRow.Revision_Number
        Me.LitExpiredDate.Text = QuoteMasterRow.expiredDate


        'Dim Employee1 As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "E")
        'If Employee1.Count > 0 Then
        '    Dim Employee1Row As EQDS.EQPARTNERRow = Employee1(0)
        '    Me.LitSalesRepresentative.Text = SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(Employee1Row.ERPID, QuoteMasterRow.CreatedBy)
        'End If

        'Evon,Tang:quotation內Sales employee為串進SAP內的sales employee的欄位這是對的,
        '但報價單內顯示的”負責業務”請帶出Created by欄位的人
        'Me.LitSalesRepresentative.Text = SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(Nothing, QuoteMasterRow.CreatedBy)
        'Ming add 20140228取得負責業務的中文名稱，
        Me.LitSalesRepresentative.Text = SiebelTools.GetLocalNameByeMail(QuoteMasterRow.CreatedBy)
        If String.IsNullOrEmpty(LitSalesRepresentative.Text.Trim) Then
            Me.LitSalesRepresentative.Text = SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(Nothing, QuoteMasterRow.CreatedBy)
        End If
        'Dim SiebConDAL As New SiebelDSTableAdapters.SIEBEL_CONTACTTableAdapter
        'Dim SiebConDt As SiebelDS.SIEBEL_CONTACTDataTable = SiebConDAL.GetTopOneContactByEmail(QuoteMasterRow.CreatedBy)


        Dim _dtSiebelContact As DataTable = SiebelTools.GetContactInfoWithSIEBEL_POSITION(QuoteMasterRow.CreatedBy)

        'If SiebConDt.Rows.Count > 0 Then
        If _dtSiebelContact IsNot Nothing AndAlso _dtSiebelContact.Rows.Count > 0 Then
            'Dim row As SiebelDS.SIEBEL_CONTACTRow = SiebConDt.Rows(0)
            'If Not String.IsNullOrEmpty(row.WorkPhone) Then
            '    Me.LitSalesRepresentativeTEL.Text = row.WorkPhone.Split(vbLf)(0)
            'End If
            'If Not String.IsNullOrEmpty(row.FaxNumber) Then
            '    Me.LitSalesRepresentativeFAX.Text = row.FaxNumber.Split(vbLf)(0)
            'End If
            Dim row As DataRow = _dtSiebelContact.Rows(0)
            If Not String.IsNullOrEmpty(row.Item("WorkPhone").ToString) Then
                Me.LitSalesRepresentativeTEL.Text = row.Item("WorkPhone").ToString.Split(vbLf)(0)
            End If
            If Not String.IsNullOrEmpty(row.Item("FaxNumber").ToString) Then
                Me.LitSalesRepresentativeFAX.Text = row.Item("FaxNumber").ToString.Split(vbLf)(0)
            End If
        End If


        ''Extended Warrenty
        'Dim _MaxEWItem As QuoteItem = MyQuoteX.GetMaxEWQuoteItem(_QuoteId)
        'If _MaxEWItem IsNot Nothing Then
        '    Dim _EWMonths As Integer = _MaxEWItem.ewFlag
        '    Dim _StandardWarrantyYears As Integer = 2, _ExtendedWarrantyMonths As Integer = 0
        '    If _EWMonths > 0 Then
        '        _StandardWarrantyYears = _StandardWarrantyYears + Int(_EWMonths / 12)
        '        _ExtendedWarrantyMonths = _EWMonths Mod 12
        '    End If
        '    Me.LitExtendWarranty.Text = _StandardWarrantyYears & " Years"
        '    If _ExtendedWarrantyMonths > 0 Then Me.LitExtendWarranty.Text &= _ExtendedWarrantyMonths & " Months"
        'End If


        'Payment Term
        Dim _PaymentTermDisplayName As String = SAPTools.GetPaymentMethodNameByValue(QuoteMasterRow.paymentTerm)
        If _PaymentTermDisplayName = "0" Then _PaymentTermDisplayName = "TBD"
        'Me.LitPaymentTerm.Text = _PaymentTermDisplayName & "，" & Me.LitPaymentTerm.Text

        'Dim decSubTotal As Decimal = MyQuoteX.GetTotalPrice(_QuoteId)
        Dim decSubTotal As Integer = MyQuoteX.GetATWTotalAmount(_QuoteId)
        Me.lbSubTotal1.Text = _currencySign + decSubTotal.ToString("N0")

        Me.lbTaxRate.Text = FormatNumber(QuoteMasterRow.tax * 100, 2) & "%"
        Dim _tax As Double = Math.Round(decSubTotal * _TaxRate, MidpointRounding.AwayFromZero)
        Me.tax.Text = _currencySign + _tax.ToString("N0")


        'Me.lbTotal.Text = _currencySign + (decSubTotal + QuoteMasterRow.freight + QuoteMasterRow.tax).ToString("n2")
        Me.lbTotal.Text = _currencySign + (decSubTotal + _tax).ToString("N0")

        'Me._lbExternalNote = QuoteMasterRow.quoteNote
        Dim _noteDA As New EQDSTableAdapters.QuotationNoteTableAdapter()
        Dim _dtNotes As DataTable = _noteDA.GetNoteTextBYQuoteId(UID)
        For Each _row As DataRow In _dtNotes.Rows
            Select Case _row.Item("notetype").ToString.ToUpper
                Case "ORDERNOTE"
                    Me._lbExternalNote = _row.Item("notetext").ToString.Replace(Chr(10), "<BR />")
            End Select
        Next


        Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(QuoteId)
        If _ME IsNot Nothing Then
            lbSalesEngineerName.Text = _ME.Engineer
            'lbSalesEngineerTEL.Text = _ME.Engineer_Telephone
            LitExtendWarranty.Text = _ME.Warranty
        End If
        'Frank 2015/02/10
        '報價單的業務工程師如果沒輸入則預設帶出create報價單的sales
        If String.IsNullOrEmpty(lbSalesEngineerName.Text) Then
            lbSalesEngineerName.Text = Me.LitSalesRepresentative.Text
            'lbSalesEngineerTEL.Text = Me.LitSalesRepresentativeTEL.Text
        End If

    End Sub

    Protected Sub gv1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim _QuoteItem As QuoteItem = CType(e.Row.DataItem, QuoteItem)

            'If Not String.IsNullOrEmpty(_QuoteItem.VirtualPartNo) Then
            '    e.Row.Cells(1).Text = _QuoteItem.VirtualPartNo
            'End If
            'Ming 20100505 註解掉這段程式，讓PDF自行換頁
            'Dim _PageBreak As Integer = 0
            'Select Case _CurrentPage
            '    Case 1
            '        _PageBreak = 13
            '    Case Else
            '        _PageBreak = 13 + ((_CurrentPage - 1) * 20)
            'End Select

            'If e.Row.RowIndex Mod _PageBreak = 0 AndAlso e.Row.RowIndex <> 0 Then
            '    e.Row.Attributes("style") = "page-break-after:always;"
            '    _CurrentPage += 1
            'End If

            Dim _DisplayUnitPrice As Double = 0
            Dim _IsValidDisplayUnitPrice As Boolean = False
            _IsValidDisplayUnitPrice = Double.TryParse(_QuoteItem.DisplayUnitPrice, _DisplayUnitPrice)
            Dim _DisplayQty As Integer = 0
            Dim _IsValidDisplayQty As Boolean = False
            _IsValidDisplayQty = Integer.TryParse(_QuoteItem.DisplayQty, _DisplayQty)

            'Frank 20150603, Supporting new line displaying
            Dim _PartDescription As String = CType(e.Row.FindControl("lbdescription"), Label).Text
            If Not String.IsNullOrEmpty(_PartDescription) Then
                CType(e.Row.FindControl("lbdescription"), Label).Text = _PartDescription.Replace(Chr(10), "<BR />")
            End If

            If Not String.IsNullOrEmpty(_QuoteItem.DisplayUnitPrice) AndAlso _IsValidDisplayUnitPrice Then
                CType(e.Row.FindControl("lbUnitPriceSign"), Label).Text = _currencySign
                CType(e.Row.FindControl("lbUnitPrice"), Label).Text = FormatNumber(_DisplayUnitPrice, 0)
            End If

            Select Case _QuoteItem.ItemTypeX
                Case QuoteItemType.Part
                    'If Not String.IsNullOrEmpty(_QuoteItem.DisplayUnitPrice) AndAlso Not String.IsNullOrEmpty(_QuoteItem.DisplayQty) Then
                    If _IsValidDisplayUnitPrice And _IsValidDisplayQty Then
                        CType(e.Row.FindControl("lbSubTotal"), Label).Text = FormatNumber(_DisplayUnitPrice * _DisplayQty, 0)
                        CType(e.Row.FindControl("lbSubTotalSign"), Label).Text = _currencySign
                    End If
                Case QuoteItemType.BtosParent
                    If Not String.IsNullOrEmpty(_QuoteItem.DisplayQty) Then
                        CType(e.Row.FindControl("lbGVQty"), Label).Text &= "套"
                        CType(e.Row.FindControl("lbGVQty"), Label).CssClass = "QDtext3"
                        'If Not String.IsNullOrEmpty(_QuoteItem.DisplayUnitPrice) Then
                        If _IsValidDisplayUnitPrice Then
                            CType(e.Row.FindControl("lbSubTotal"), Label).Text = FormatNumber(_DisplayUnitPrice * _DisplayQty, 0)
                            CType(e.Row.FindControl("lbSubTotalSign"), Label).Text = _currencySign
                        End If
                    End If
                    e.Row.CssClass = "QDtext3"
                Case QuoteItemType.BtosPart
            End Select


            'If _QuoteItem.ItemTypeX = QuoteItemType.BtosParent Then

            '    'Frank 2015/02/09
            '    '產生給客戶看的報價單內的組裝單要可以顯示對parent item輸入的價格
            '    'parent item沒輸入價格就顯示child items的加總價格
            '    'Dim TAmt As Decimal = _QuoteItem.ChildSubUnitPriceX
            '    Dim TAmt As Decimal = 0
            '    If _QuoteItem.newUnitPrice > 0 Then
            '        TAmt = _QuoteItem.newUnitPrice * _QuoteItem.qty
            '    Else
            '        TAmt = _QuoteItem.ChildSubUnitPriceX
            '    End If


            '    Me._CurrentParentItemQty = _QuoteItem.qty
            '    CType(e.Row.FindControl("lbUnitPrice"), Label).Text = FormatNumber(TAmt / _QuoteItem.qty, 2)
            '    CType(e.Row.FindControl("lbSubTotal"), Label).Text = FormatNumber(TAmt, 2)

            '    e.Row.CssClass = "QDtext3"

            'Else

            '    Dim _pri_format As EnumSetting.USPrintOutFormat = _MasterRef.PRINTOUT_FORMAT
            '    Select Case _pri_format
            '        Case EnumSetting.USPrintOutFormat.MAIN_ITEM_ONLY
            '            If _QuoteItem.line_No > 100 Then
            '                e.Row.Visible = False
            '            End If
            '        Case EnumSetting.USPrintOutFormat.SUB_ITEM_WITH_SUB_ITEM_PRICE
            '        Case EnumSetting.USPrintOutFormat.SUB_ITEM_WITHPARTNO_WITHOUT_SUB_ITEM_PRICE
            '            If _QuoteItem.line_No > 100 Then
            '                CType(e.Row.FindControl("lbUnitPrice"), Label).Text = ""
            '                CType(e.Row.FindControl("lbUnitPriceSign"), Label).Text = ""
            '                CType(e.Row.FindControl("lbSubTotal"), Label).Text = ""
            '                CType(e.Row.FindControl("lbSubTotalSign"), Label).Text = ""
            '            End If
            '    End Select

            '    '2015/02/09 ATW request
            '    '報價單內組裝單的子階料號的數量需顯示為一台系統包含的數量(例如組裝兩台，但記憶體一台只有一條，則記憶體數量只顯示為1)
            '    If _QuoteItem.ItemTypeX = QuoteItemType.BtosPart Then
            '        CType(e.Row.FindControl("lbGVQty"), Label).Text = _QuoteItem.qty / Me._CurrentParentItemQty
            '    End If



            'End If


        End If



    End Sub
End Class