Public Class KRAOnlineQuoteTemplate
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
    Dim dtQuoteDetail As List(Of QuoteItem) = Nothing
    Dim _CurrentParentItemQty As Integer = 0
    Dim _CurrentPage As Integer = 1
    'Dim _IsListPriceOnly As Boolean = False
    Dim _AKRQuotingPriceMethod As Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod = Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod.ListAndNegoPrice

    Public Sub LoadData()


        Dim dtM As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(_QuoteId, COMM.Fixer.eDocType.EQ)
        Me._QuoteNo = dtM.quoteNo
        Me._MasterRef = dtM

        Dim _QM As Quote_Master = MyQuoteX.GetQuoteMaster(Me.QuoteId)
        [Enum].TryParse(Of Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod)(dtM.isShowListPrice, _AKRQuotingPriceMethod)

        If Not IsNothing(dtM) Then

            _currencySign = Util.getCurrSign(dtM.currency)

            If _currencySign.Equals("KRW") Then
                _currencySign = "₩"
            End If

            _TaxRate = dtM.tax
            Me.dtQuoteDetail = MyQuoteX.GetQuoteList(_QuoteId)


            Select Case _AKRQuotingPriceMethod
                Case Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod.ListAndNegoPrice
                Case Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod.ListPriceOnly
                    For Each _lineitem As QuoteItem In Me.dtQuoteDetail
                        _lineitem.newUnitPrice = _lineitem.listPrice
                    Next
                    Me.trNegoPriceTotal.Visible = False
                Case Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod.NegoPriceOnly
                    Me.trListPriceTotal.Visible = False
            End Select



            Dim LoostItems As List(Of QuoteItem) = Me.dtQuoteDetail.Where(Function(p) p.line_No < 100).ToList
            Me.dtQuoteDetail.RemoveAll(Function(p) p.line_No < 100)
            Me.dtQuoteDetail.AddRange(LoostItems)



            Me.gv1.Attributes("style") = "border-collapse:separate"
            Me.gv1.DataSource = Me.dtQuoteDetail
            Me.gv1.DataBind()

            FillQuoteMasterInfo(dtM)
        End If
    End Sub

    Protected Sub FillQuoteMasterInfo(ByRef QuoteMasterRow As IBUS.iDocHeaderLine)

        'Nadia Kim and Iris 2015-11-11:報價單Account Name呈現SAP Account Name
        If String.IsNullOrEmpty(QuoteMasterRow.AccErpId) Then
            Me.litAccountName.Text = QuoteMasterRow.AccName
        Else
            Dim _dtAccountFromSAP As DataTable = SAPDAL.SAPDAL.GetCompanyDataFromLocal(QuoteMasterRow.AccErpId, QuoteMasterRow.org)
            If _dtAccountFromSAP IsNot Nothing AndAlso _dtAccountFromSAP.Rows.Count > 0 Then
                Me.litAccountName.Text = _dtAccountFromSAP.Rows(0).Item("COMPANY_NAME")
            End If
        End If

        Dim _dt As DataTable = Nothing

        _dt = SiebelTools.GET_Account_Info_By_ID(QuoteMasterRow.AccRowId)

        If Not String.IsNullOrEmpty(QuoteMasterRow.attentionRowId) Then
            _dt = SiebelTools.GET_Contact_Info_by_RowID(QuoteMasterRow.attentionRowId)
            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then

                'Jessica.Lee 20151218: Pls add “님” the end of ATTN name, like ATTN: Jessica님, pls see the second picture for your reference
                Me.litAccountContactPerson.Text = _dt.Rows(0).Item("LastName") & _dt.Rows(0).Item("FirstName") & _dt.Rows(0).Item("JobTitle") & " 님"

                'Jessica.Lee 20151218:Remove “+82” at the Tel no.
                'Dim _accountcontacttel As String = _dt.Rows(0).Item("WorkPhone").ToString.Split(Chr(10))(0)
                'If Not String.IsNullOrEmpty(_accountcontacttel) Then
                '    If _accountcontacttel.StartsWith("+82") Then _accountcontacttel = _accountcontacttel.Replace("+82", "")
                '    'ICC 2016/1/15 Format KR work phone number.
                '    _accountcontacttel = Advantech.Myadvantech.Business.UserRoleBusinessLogic.GetCountryTelephoneFormat(_accountcontacttel, Advantech.Myadvantech.DataAccess.AOnlineRegion.AKR)
                'End If
                'ICC 2016/1/26 Use new function to set work phone number format.
                Dim _accountcontacttel As String = Advantech.Myadvantech.Business.UserRoleBusinessLogic.GetCountryTelephoneFormat(_dt.Rows(0).Item("WorkPhone").ToString, Advantech.Myadvantech.DataAccess.AOnlineRegion.AKR)

                'Dim _accountCellPhone As String = _dt.Rows(0).Item("CellPhone").ToString.Split(Chr(10))(0)
                'If Not String.IsNullOrEmpty(_accountCellPhone) Then
                '    If _accountCellPhone.StartsWith("+82") Then _accountCellPhone = _accountCellPhone.Replace("+82", "0")
                '    'ICC 2016/1/15 Format KR cell phone number.
                '    _accountCellPhone = Advantech.Myadvantech.Business.UserRoleBusinessLogic.GetCountryCellphoneFormat(_accountCellPhone, Advantech.Myadvantech.DataAccess.AOnlineRegion.AKR)
                'End If
                'ICC 2016/1/26 Use new function to set cell phone number format.
                Dim _accountCellPhone As String = Advantech.Myadvantech.Business.UserRoleBusinessLogic.GetCountryTelephoneFormat(_dt.Rows(0).Item("CellPhone").ToString, Advantech.Myadvantech.DataAccess.AOnlineRegion.AKR)

                'ICC 2016/1/15 Display work phone / cell phone
                If Not String.IsNullOrEmpty(_accountcontacttel) AndAlso Not String.IsNullOrEmpty(_accountCellPhone) Then
                    Me.LitAccountContactTEL.Text = String.Format("{0} / {1}", _accountcontacttel, _accountCellPhone) 'Both work phone and cell phone have value
                ElseIf Not String.IsNullOrEmpty(_accountcontacttel) Then
                    Me.LitAccountContactTEL.Text = _accountcontacttel 'Only work phone has value
                ElseIf Not String.IsNullOrEmpty(_accountCellPhone) Then
                    Me.LitAccountContactTEL.Text = _accountCellPhone 'Only cell phone has value
                Else
                    Me.LitAccountContactTEL.Text = String.Empty 'Neither work phone nor cell phone is empty 
                End If

                Me.LitAccountContactEmail.Text = _dt.Rows(0).Item("email_address").ToString.Split(Chr(10))(0)

            End If
        End If

        Me.LitQuoteDate.Text = QuoteMasterRow.ReqDate.ToString("yyyy/MM/dd")
        Me.litQuoteNo.Text = QuoteMasterRow.quoteNo

        Me.LitQuoteVersion.Text = QuoteMasterRow.Revision_Number

        'Nadia
        'We show it 10 days on eQuotion.. but... it's only for we have argument with customer
        'Normally, AKR price is very stable... Make sure we don't change Terms in Quotation document
        'Me.LitExpiredDate.Text = COMM.Fixer.eQuoteExpDur.KR
        Me.LitExpiredDate.Text = 10

        'Frank 20170707
        'Dim _dtsaleinfo As DataTable = SAPDAL.SAPDAL.GetAKRSalesInfo(QuoteMasterRow.CreatedBy)
        Dim _AKRSales As String = QuoteMasterRow.salesEmail
        If String.IsNullOrEmpty(_AKRSales) Then
            _AKRSales = QuoteMasterRow.CreatedBy
        End If
        Dim _dtsaleinfo As DataTable = SAPDAL.SAPDAL.GetAKRSalesInfo(_AKRSales)

        Dim _SalesTeamName As String = String.Empty, _SalesWorkPhone As String = String.Empty, _SalesMobilePhone As String = String.Empty
        Dim _SalesPositionName As String = String.Empty, _SalesName As String = String.Empty

        'Me.LitSalesRepresentative.Text = SiebelTools.GetLocalNameByeMail(QuoteMasterRow.CreatedBy)
        'If String.IsNullOrEmpty(LitSalesRepresentative.Text.Trim) Then
        '    Me.LitSalesRepresentative.Text = SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(Nothing, QuoteMasterRow.CreatedBy)
        'End If

        '_SalesName = SiebelTools.GetLocalNameByeMail(QuoteMasterRow.CreatedBy)
        'If String.IsNullOrEmpty(_SalesName) Then
        '    _SalesName = SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(Nothing, QuoteMasterRow.CreatedBy)
        'End If
        _SalesName = SiebelTools.GetLocalNameByeMail(_AKRSales)
        If String.IsNullOrEmpty(_SalesName) Then
            _SalesName = SAPDAL.SAPDAL.GetSalesRepresentativeByEmployeeID(Nothing, _AKRSales)
        End If

        If Not String.IsNullOrEmpty(_SalesName) Then
            _SalesName = " " & _SalesName
        End If

        If _dtsaleinfo IsNot Nothing AndAlso _dtsaleinfo.Rows.Count > 0 Then
            'Get Team Name of AKR sales
            If _dtsaleinfo.Rows(0).Item("Team_Name") IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(_dtsaleinfo.Rows(0).Item("Team_Name").ToString) Then
                _SalesTeamName = _dtsaleinfo.Rows(0).Item("Team_Name").ToString & "팀"
            End If
            'Get Position of AKR sales
            If _dtsaleinfo.Rows(0).Item("Position") IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(_dtsaleinfo.Rows(0).Item("Position").ToString) Then
                _SalesPositionName = " " & _dtsaleinfo.Rows(0).Item("Position").ToString
            End If
            'Get work phone of AKR sales
            If _dtsaleinfo.Rows(0).Item("Office_Tel") IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(_dtsaleinfo.Rows(0).Item("Office_Tel").ToString) Then
                _SalesWorkPhone = "직통번호: " & _dtsaleinfo.Rows(0).Item("Office_Tel").ToString
            End If
            'Get cell phone
            If _dtsaleinfo.Rows(0).Item("Mobile_Phone") IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(_dtsaleinfo.Rows(0).Item("Mobile_Phone").ToString) Then
                _SalesMobilePhone = _dtsaleinfo.Rows(0).Item("Mobile_Phone").ToString
            End If
        End If
        'Display sales team, name and position
        Me.LitSalesRepresentative.Text = _SalesTeamName & _SalesName & _SalesPositionName

        'Display work phone
        Me.LitSalesRepresentativeTEL.Text = _SalesWorkPhone
        'If Not String.IsNullOrEmpty(_SalesMobilePhone) Then
        '    Me.LitSalesRepresentativeTEL.Text = _SalesWorkPhone + ", " + _SalesMobilePhone
        'End If

        'Dim _dtSiebelContact As DataTable = SiebelTools.GetContactInfoWithSIEBEL_POSITION(QuoteMasterRow.CreatedBy)
        'Dim _dtSiebelContact As DataTable = SiebelTools.GetContactInfoWithSIEBEL_POSITION(_AKRSales)
        'If _dtSiebelContact IsNot Nothing AndAlso _dtSiebelContact.Rows.Count > 0 Then
        '    Dim row As DataRow = _dtSiebelContact.Rows(0)
        '    If Not String.IsNullOrEmpty(row.Item("EMAIL_ADDRESS").ToString) Then
        '        Me.LitSalesRepresentativeEmail.Text = row.Item("EMAIL_ADDRESS").ToString
        '    End If
        'End If
        Me.LitSalesRepresentativeEmail.Text = _AKRSales
        'Nadia 20180110
        'On the quotation Sales information should come as below. 
        '1st row : Team&Name&Position / Office Number (if have)
        '2nd row : e-mail, mobile phone 
        If Not String.IsNullOrEmpty(_SalesMobilePhone) Then
            Me.LitSalesRepresentativeCellPhone.Text &= ", " + _SalesMobilePhone
        End If


        'Payment Term
        Dim _PaymentTermDisplayName As String = "TBD"
        'Frank 20170731 update by Nadia
        Select Case QuoteMasterRow.paymentTerm
            Case "COD"
                _PaymentTermDisplayName = "선입금"
            Case "PPD"
                _PaymentTermDisplayName = "주문시 선입금"
            Case "I014"
                _PaymentTermDisplayName = "납품후 14일 이내"
            Case "I030"
                _PaymentTermDisplayName = "납품후 30일 이내"
            Case "MO15"
                _PaymentTermDisplayName = "익월말 15일"
            Case "MO30"
                _PaymentTermDisplayName = "익월말 현금"
            Case "MO45"
                _PaymentTermDisplayName = "익월말 45일"
            Case "MO60"
                _PaymentTermDisplayName = "익익월말 현금"
            Case "MO75"
                _PaymentTermDisplayName = "월말 기준 75일"
            Case "MO90"
                _PaymentTermDisplayName = "귀사 결제 조건"
            Case Else
                _PaymentTermDisplayName = SAPTools.GetPaymentMethodNameByValue(QuoteMasterRow.paymentTerm)
                If _PaymentTermDisplayName = "0" Then _PaymentTermDisplayName = "TBD"
        End Select

        Me.LitPaymentTerm.Text = _PaymentTermDisplayName

        Dim ListPriceTotal As Decimal = MyQuoteX.GetTotalListPrice(_QuoteId)
        Me.lbListPriceTotal.Text = _currencySign + " " + ListPriceTotal.ToString("N0")

        Dim decSubTotal As Decimal = MyQuoteX.GetTotalPrice(_QuoteId)
        Me.lbSubTotal1.Text = _currencySign + " " + decSubTotal.ToString("N0")

        Me.lbTaxRate.Text = FormatNumber(QuoteMasterRow.tax * 100, 2) & "%"
        Dim _tax As Double = Math.Round(decSubTotal * _TaxRate, MidpointRounding.AwayFromZero)
        Me.tax.Text = _currencySign + " " + _tax.ToString("N0")


        Me.lbTotal.Text = _currencySign + " " + (decSubTotal + _tax).ToString("N0")

        Dim _noteDA As New EQDSTableAdapters.QuotationNoteTableAdapter()
        Dim _dtNotes As DataTable = _noteDA.GetNoteTextBYQuoteId(UID)
        For Each _row As DataRow In _dtNotes.Rows
            Select Case _row.Item("notetype").ToString.ToUpper
                Case "ORDERNOTE"
                    Me._lbExternalNote = _row.Item("notetext").ToString.Replace(Chr(10), "<BR />")
                    Me.ltNotes.Text = _row.Item("notetext").ToString.Replace(Chr(10), "<BR />").ToString
            End Select
        Next

    End Sub

    Dim DTList As IBUS.iCartList = Nothing, SystemCount As Integer = 0
    Protected Sub gv1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim _QuoteItem As QuoteItem = CType(e.Row.DataItem, QuoteItem)

            Dim UnitPrice As Double = _QuoteItem.newUnitPrice
            Dim Qty As Integer = _QuoteItem.qty
            Dim SubTotal As Decimal = 0.0
            SubTotal = FormatNumber(Qty * UnitPrice, 0)

            'Frank 20170706: added model page url to Advantech standard part number
            Dim _partno As String = e.Row.Cells(2).Text
            Dim _PisPart As Advantech.Myadvantech.DataAccess.Part = Advantech.Myadvantech.Business.PartBusinessLogic.GetPartWithCompleteModelInformation(_partno)
            If _PisPart.Models.Count > 0 Then
                Dim _url As String = _PisPart.Models(0).GetModelURL(Advantech.Myadvantech.DataAccess.AOnlineRegion.AKR)
                e.Row.Cells(2).Text = "<a href='" & _url & "'>" & e.Row.Cells(2).Text & "</a>"
            End If

            CType(e.Row.FindControl("lbUnitPriceSign"), Label).Text = _currencySign
            CType(e.Row.FindControl("lbUnitPrice"), Label).Text = FormatNumber(UnitPrice, 0)

            Select Case _QuoteItem.ItemTypeX
                Case QuoteItemType.Part
                    CType(e.Row.FindControl("lbSubTotalSign"), Label).Text = _currencySign
                    CType(e.Row.FindControl("lbSubTotal"), Label).Text = FormatNumber(SubTotal, 0)

                    e.Row.Cells(0).Text = _QuoteItem.line_No + SystemCount
                Case QuoteItemType.BtosParent

                    e.Row.Cells(0).Text = _QuoteItem.line_No / 100
                    SystemCount = SystemCount + 1

                    Dim totalamont As Decimal = 0

                    Dim DTL As IBUS.iCartList = DTList.Merge2NewCartList(DTList.Where(Function(X) X.parentLineNo.Value = _QuoteItem.line_No))

                    'Frank 20170822
                    'totalamont = DTL.getTotalAmount()
                    If Me._AKRQuotingPriceMethod = Advantech.Myadvantech.DataAccess.AKRQuotingPriceMethod.ListPriceOnly Then
                        totalamont = DTL.getTotalListAmount()
                    Else
                        totalamont = DTL.getTotalAmount()
                    End If


                    CType(e.Row.FindControl("lbSubTotal"), Label).Text = FormatNumber(totalamont, 0)
                    CType(e.Row.FindControl("lbUnitPrice"), Label).Text = FormatNumber(totalamont / _QuoteItem.qty, 0)


                    CType(e.Row.FindControl("lbGVQty"), Label).CssClass = "QDtext3"
                    CType(e.Row.FindControl("lbSubTotalSign"), Label).Text = _currencySign
                    e.Row.CssClass = "QDtext3"
                Case QuoteItemType.BtosPart
                    e.Row.Cells(0).Text = ""
                    CType(e.Row.FindControl("lbUnitPriceSign"), Label).Text = ""
                    CType(e.Row.FindControl("lbUnitPrice"), Label).Text = ""
            End Select

        End If

    End Sub
    Private Sub gv1_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv1.DataBinding
        DTList = Pivot.FactCart.getCartByAppArea(COMM.Fixer.eCartAppArea.EQ, UID, Pivot.CurrentProfile.getCurrOrg).GetListAll(COMM.Fixer.eDocType.EQ)
    End Sub
End Class