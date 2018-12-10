Public Class TWAOnlineQuoteTemplate
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


    Public Property IsEnglishVersion As Boolean
        Get
            Return _IsEnglishVersion
        End Get
        Set(ByVal value As Boolean)
            _IsEnglishVersion = value
        End Set
    End Property


    Public _QuoteNo As String = String.Empty

    Public _currencySign As String = "", _QuoteId As String = "", _IsInternalUserMode As Boolean = True, _IsBTosQuotation As Boolean = False, _IsEnglishVersion = False
    Public _lbExternalNote As String = ""
    Dim _TaxRate As Decimal = CDec(0.0)
    'Dim DTList As IBUS.iCartList = Nothing
    Dim dtQuoteDetail As List(Of QuoteItem) = Nothing
    Dim _CurrentParentItemQty As Integer = 0
    Dim _CurrentPage As Integer = 1
    Public _columnnamewith As String = "90"
    Public _columnvalueith As String = "540"
    Public Sub LoadData()

        'QuoteHeaderLine.PRINTOUT_FORMAT


        Dim dtM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteId, COMM.Fixer.eDocType.EQ)
        Me._QuoteNo = dtM.quoteNo
        Me._MasterRef = dtM

        'Frank 20171214, convert quotation from Chinese to English
        If dtM.PRINTOUT_FORMAT = COMM.Fixer.USPrintOutFormat.ATW_Quote_PDF_Englisn _
            OrElse dtM.PRINTOUT_FORMAT = COMM.Fixer.USPrintOutFormat.ATW_Quote_Word_Englisn Then

            Me._IsEnglishVersion = True

        End If

        'End If

        'Dim dtDetail As IBUS.iCartList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, _QuoteId, dtM.org).GetListAll(COMM.Fixer.eDocType.EQ)

        'Ryan 20170320 Show Advantech Company Info according to user is Aonline IA or else.
        Dim MG As New PROF.MailGroup
        Dim GroupBelTo As ArrayList = MG.getMailGroupArray(Pivot.CurrentProfile.UserId)
        If GroupBelTo.Contains(("CallCenter.IA.ACL").ToUpper) Then
            'tdAdvantechInfo1.Visible = False
            'tdAdvantechInfo2.Visible = True

            Me.Label_HQOfficeTel.Text = "2794-7305"
        End If

        If Not IsNothing(dtM) Then

            _currencySign = Util.getCurrSign(dtM.currency)

            If _currencySign.Equals("$") Then
                _currencySign = "US" & _currencySign
            End If

            If dtM.org.ToUpper.Equals("TW20") Then
                tdAdvantechHeader1.Visible = False
                tdAdvantechHeader2.Visible = True
                tdAdvantechInfo1.Visible = False
                tdAdvantechInfo2.Visible = True
                If _IsEnglishVersion Then
                    td2EnglishVersion.Visible = True
                Else
                    td2ChineseVersion.Visible = True
                End If
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

        'ICC 2016/4/13 For customers do not have ERP ID, we change to get uni number for insread.
        If String.IsNullOrEmpty(QuoteMasterRow.AccErpId) AndAlso Not String.IsNullOrEmpty(QuoteMasterRow.AccRowId) Then
            Dim uniNumber As Object = tbOPBase.dbExecuteScalar("CRM", String.Format("select top 1 case when ATTRIB_04 IS NULL then ISNULL(ATTRIB_05,'') else ATTRIB_04 end as [UN] from S_ORG_EXT_X where ROW_ID = '{0}' ", QuoteMasterRow.AccRowId))
            If Not uniNumber Is Nothing Then QuoteMasterRow.AccErpId = uniNumber.ToString
        End If

        Me.LitAccountERPID.Text = QuoteMasterRow.AccErpId.TrimStart("T")

        Me.LitQuoteDate.Text = QuoteMasterRow.ReqDate
        'ACL AOnline team:Quote PDF的報價單號改成顯示quote Desc.
        'Me.litQuoteNo.Text = QuoteMasterRow.quoteNo
        Me.litQuoteNo.Text = QuoteMasterRow.CustomId

        Me.LitQuoteVersion.Text = QuoteMasterRow.Revision_Number
        Me.LitExpiredDate.Text = QuoteMasterRow.expiredDate

        If QuoteMasterRow.org.ToUpper.Equals("TW20") Then
            If QuoteMasterRow.currency.Equals("USD", StringComparison.InvariantCultureIgnoreCase) Then
                Me.lt_BackAccount.Text = "5831680204 (USD)"
            Else
                Me.lt_BackAccount.Text = "5831680018 (TWD)"
            End If
        Else
            'Evon 20170220 if currency is USD, then change the bank account
            If QuoteMasterRow.currency.Equals("USD", StringComparison.InvariantCultureIgnoreCase) Then
                Me.lt_BackAccount.Text = "5028523205"
            End If
        End If


        'Evon 20170220 if ship term is not Standard, then replace 送至貴處 by the QuoteMasterRow.shipTerm
        '01=Standard
        If Not QuoteMasterRow.shipTerm.Equals("01", StringComparison.InvariantCultureIgnoreCase) Then

            Dim _shiptermname As String = Business.getSapShipTermNameByOrg(Pivot.CurrentProfile.getCurrOrg, QuoteMasterRow.shipTerm)

            If Not String.IsNullOrEmpty(_shiptermname) Then
                Me.ltShipment.Text = _shiptermname
            End If
        End If

        'Dim Employee1 As EQDS.EQPARTNERDataTable = apt.GetPartnerByQIDAndType(_QuoteId, "E")
        'If Employee1.Count > 0 Then
        '    Dim Employee1Row As EQDS.EQPARTNERRow = Employee1(0)
        '    Me.LitSalesRepresentative.Text = SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(Employee1Row.ERPID, QuoteMasterRow.CreatedBy)
        'End If

        'Evon,Tang:quotation內Sales employee為串進SAP內的sales employee的欄位這是對的,
        '但報價單內顯示的”負責業務”請帶出Created by欄位的人
        'Me.LitSalesRepresentative.Text = SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(Nothing, QuoteMasterRow.CreatedBy)
        'Ming add 20140228取得負責業務的中文名稱，

        'Frank 20171213 支援英文版的報價單顯示業務英文名字
        'Me.LitSalesRepresentative.Text = SiebelTools.GetLocalNameByeMail(QuoteMasterRow.CreatedBy)
        If _IsEnglishVersion Then
            Me.LitSalesRepresentative.Text = SiebelTools.GetEnglishNameByeMail(QuoteMasterRow.CreatedBy)
        Else
            Me.LitSalesRepresentative.Text = SiebelTools.GetLocalNameByeMail(QuoteMasterRow.CreatedBy)
        End If

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
        'Frank 20180131 :Jenny.Lin:display sales email on the quotation header
        Me.LitSalesRepresentativeEmail.Text = "<A Href='mailto:" & QuoteMasterRow.CreatedBy & "'>" & QuoteMasterRow.CreatedBy & "</A>"


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
        Me.LitPaymentTerm.Text = _PaymentTermDisplayName

        'ICC 2016/4/12 For ATW use USD currency, please remain decimal point numbers for two digit
        If _currencySign.EndsWith("$") Then
            Dim decSubTotalDecimal As Decimal = MyQuoteX.GetATWTotalAmountV2(_QuoteId)
            Me.lbSubTotal1.Text = _currencySign + decSubTotalDecimal.ToString("N2")

            Me.lbTaxRate.Text = FormatNumber(QuoteMasterRow.tax * 100, 2) & "%"
            'Ryan 20170316 Remove decimal.round to USD tax amount 
            Dim _tax As Double = decSubTotalDecimal * _TaxRate
            Me.tax.Text = _currencySign + _tax.ToString("N2")

            Me.lbTotal.Text = _currencySign + (decSubTotalDecimal + _tax).ToString("N2")
        Else
            Dim decSubTotal As Integer = MyQuoteX.GetATWTotalAmount(_QuoteId)
            Me.lbSubTotal1.Text = _currencySign + decSubTotal.ToString("N0") 'ICC Change to parameter

            Me.lbTaxRate.Text = FormatNumber(QuoteMasterRow.tax * 100, 2) & "%"
            Dim _tax As Double = Math.Round(decSubTotal * _TaxRate, MidpointRounding.AwayFromZero)
            Me.tax.Text = _currencySign + _tax.ToString("N0")


            'Me.lbTotal.Text = _currencySign + (decSubTotal + QuoteMasterRow.freight + QuoteMasterRow.tax).ToString("n2")
            Me.lbTotal.Text = _currencySign + (decSubTotal + _tax).ToString("N0")
        End If

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
            lbSalesEngineerTEL.Text = _ME.Engineer_Telephone
            LitExtendWarranty.Text = _ME.Warranty
        End If
        'Frank 2015/02/10
        '報價單的業務工程師如果沒輸入則預設帶出create報價單的sales
        If String.IsNullOrEmpty(lbSalesEngineerName.Text) Then
            lbSalesEngineerName.Text = Me.LitSalesRepresentative.Text
            lbSalesEngineerTEL.Text = Me.LitSalesRepresentativeTEL.Text
        End If

        If _IsEnglishVersion Then
            Me._columnnamewith = "150"
            Me._columnvalueith = "500"
            'Me.Label_AdvantechName.Text = "Advantech Co., Ltd."
            Me.lbQuotationTitle1.Text = "Quotation"
            Me.lbQuotationTitle2.Text = "Quotation"

            Me.Label_AdvantechHQAddress.Text = "Head Office: No. 1, Alley 20, Lane 26, Rueiguang Road, Neihu District, Taipei 11491, Taiwan, (R.O.C.)"
            Me.Label_AdvantechTaichungAddress.Text = "Taichung Office: Rm. 5, 6F., No.633, Sec. 2, Taiwan Blvd., Xitun Dist., Taichung City 407, Taiwan (R.O.C.)"
            Me.Label_AdvantechKaohsiungAddress.Text = "Kaohsiung Office: 1F.-A1, No.502-2, Jiuru 1st Rd., Sanmin Dist., Kaohsiung City 807, Taiwan (R.O.C.)"

            Me.LableColumnName_AccountAddress.Text = "Address"
            Me.LableColumnName_AccountCompanyID.Text = "CompanyID"
            Me.LableColumnName_AccountContactPerson.Text = "Contact Person"
            Me.LableColumnName_AccountFAX.Text = "Fax"
            Me.LableColumnName_AccountTEL.Text = "Tel"
            Me.LableColumnName_AccountName.Text = "Account Name"
            Me.LableColumnName_QuoteDate.Text = "Date"
            Me.LableColumnName_QuoteExpiredDate.Text = "Expiration Date"
            Me.LableColumnName_QuoteNumber.Text = "Quote No."
            Me.LableColumnName_QuoteRevNumber.Text = "REV#"
            Me.LableColumnName_SalesRepresentative.Text = "Sales Rep."
            Me.LableColumnName_SalesRepresentativeFAX.Text = "Fax"
            Me.LableColumnName_SalesRepresentativeTEL.Text = "Tel"
            Me.LableColumnName_SalesRepresentativeEmail.Text = "EMail"

            Me.LabelTitle_ShippingDate.Text = "Shipping Date"
            Me.LitReqDate.Text = "Please confirm the shipping date with sales representative before placing the order"
            Me.LabelTitle_ShippingCondition.Text = "Shipping Method"
            Me.ltShipment.Text = ""
            Me.LabelTitle_Warranty.Text = "Warranty"
            Me.LabelTitle_PaymentTerm.Text = "Payment Method"

            Me.LabelTitle_PaymentTermText1.Text = ""
            Me.LabelTitle_PaymentTermText2.Text = ""
            Me.lt_BackAccount.Text = ""


            Me.Label_Note.Text = "Note"
            Me.Label_QuoteAmoutWithoutTax.Text = "Sub Total"
            Me.Label_Tax.Text = "Tax"
            Me.Label_QuoteAmoutWithTax.Text = "Total"

            Me.LabelTitle_SaleEngineer.Text = "Sales Engineer"
            Me.LabelTitle_SaleEngineerTEL.Text = "Tel"
            Me.LabelTitle_ePlatformTollFreeNumber.Text = "ePlatform Toll Free Number"

            Dim _TandC As New StringBuilder

            _TandC.AppendLine("<font style='font-size: 12px;'>Personal Property Secured Transactions Act<br/>")
            _TandC.AppendLine("Article 28<br/>")
            _TandC.AppendLine("If, before the transfer of ownership of the subject property to the buyer, the buyer has any of the following doings, to the detriment of the rights and interests of the seller, the seller may retrieve and take possession of the subject property:<br/>")
            _TandC.AppendLine("1. Fails to pay the price of the property as stipulated.<br/>")
            _TandC.AppendLine("2. Fails to fulfill the specific conditions as stipulated.<br/>")
            _TandC.AppendLine("3. Sells, pledges, or otherwise disposes of the subject property.<br/>")
            _TandC.AppendLine("If, when the seller retrieves and takes possession of the subject property of the preceding paragraph, there is significant depreciation in the value of the subject property, the seller may claim damages from the buyer.")
            _TandC.AppendLine("</font>")
            Me.DIV_question1.InnerHtml = _TandC.ToString
            Me.TermsAndConditions.InnerHtml = ""



            'Me.gv1.Columns(0).HeaderText = "Item No."
            'Me.gv1.Columns(1).HeaderText = "Part No."
            'Me.gv1.Columns(2).HeaderText = "Part No."
            'Me.gv1.Columns(3).HeaderText = "Description"
            'Me.gv1.Columns(4).HeaderText = "Quantity"
            'Me.gv1.Columns(5).HeaderText = "Unit Price"
            'Me.gv1.Columns(6).HeaderText = "Amount"

        End If


    End Sub

    Protected Sub gv1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.Header And _IsEnglishVersion Then
            e.Row.Cells(0).Text = "Item No."
            e.Row.Cells(1).Text = "Part No."
            e.Row.Cells(2).Text = "Part No."
            e.Row.Cells(3).Text = "Description"
            e.Row.Cells(4).Text = "Quantity"
            e.Row.Cells(5).Text = "Unit Price"
            e.Row.Cells(6).Text = "Amount"
        End If

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

            'ICC 2016/4/12 For ATW use USD currency, please remain decimal point numbers for two digit
            Dim point As Integer = 0
            If _currencySign.EndsWith("$") Then point = 2

            If Not String.IsNullOrEmpty(_QuoteItem.DisplayUnitPrice) AndAlso _IsValidDisplayUnitPrice Then
                CType(e.Row.FindControl("lbUnitPriceSign"), Label).Text = _currencySign
                CType(e.Row.FindControl("lbUnitPrice"), Label).Text = FormatNumber(_DisplayUnitPrice, point)
            End If

            Select Case _QuoteItem.ItemTypeX
                Case QuoteItemType.Part
                    'If Not String.IsNullOrEmpty(_QuoteItem.DisplayUnitPrice) AndAlso Not String.IsNullOrEmpty(_QuoteItem.DisplayQty) Then
                    If _IsValidDisplayUnitPrice And _IsValidDisplayQty Then
                        CType(e.Row.FindControl("lbSubTotal"), Label).Text = FormatNumber(_DisplayUnitPrice * _DisplayQty, point)
                        CType(e.Row.FindControl("lbSubTotalSign"), Label).Text = _currencySign
                    End If
                Case QuoteItemType.BtosParent
                    If Not String.IsNullOrEmpty(_QuoteItem.DisplayQty) Then
                        If _IsEnglishVersion Then
                            CType(e.Row.FindControl("lbGVQty"), Label).Text &= "Set"
                        Else
                            CType(e.Row.FindControl("lbGVQty"), Label).Text &= "套"
                        End If
                        CType(e.Row.FindControl("lbGVQty"), Label).CssClass = "QDtext3"
                        'If Not String.IsNullOrEmpty(_QuoteItem.DisplayUnitPrice) Then
                        If _IsValidDisplayUnitPrice Then
                            CType(e.Row.FindControl("lbSubTotal"), Label).Text = FormatNumber(_DisplayUnitPrice * _DisplayQty, point)
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

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    'End Sub

End Class