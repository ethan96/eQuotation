Imports Advantech.Myadvantech.DataAccess

Public Class QuotationMaster1
    Inherits PageBase
    Dim oCustomId As String = "", oCreatedBy As String = "", oQuoteDate As DateTime = Now, oPrepareBy As String = "", oQuoteToName As String = "", hQuoteErpId As String = ""
    Dim hQuoteToAccountRowId As String = "", oOffice As String = "", oCurrency As String = "", oSalesEmail As String = "", hSalesRowId As String = "", oDirectPhone As String = ""
    Dim oAttention As String = "", hAttentionRowId As String = "", oBankAccount As String = "", oDeliveryDate As Date = Now.Date, oCreatedDate = Now.Date
    'Dim oExpiredDate As Date = CDate(Now.AddMonths(1).ToShortDateString), oShipTerms As String = "", oPaymentTerms As String = ""
    Dim oExpiredDate As Date = CDate(Now.ToShortDateString), oShipTerms As String = "", oPaymentTerms As String = ""
    Dim oFreight As String = "TBD", oInsurance As Decimal = CDec(0.0), oSpecialCharge As Decimal = CDec(0.0), oTax As Decimal = CDec(0.0), oQuoteNote As String = ""
    Dim oRelatedInfo As String = "", oIsRepeatedOrder As Integer = 0, oGroup As String = "", oDelDateFlag As Integer = 0, oOrg As String = "", siebelRBU As String = ""
    Dim oSalesOfficeCode As String = "", oSalesGroupCode As String = ""
    Dim oQuoteNo As String = "", oRevision_Number As Integer = 0, oActive As Boolean = True
    Dim oEmailGreeting As String = "", oSpecialTandC As String = "", oSignatureRowID As String = "", oWarranty As String = ""
    Dim oABRQuoteType As String = ""
    Dim oCopyPurpose As Integer = 0
    Dim oIsShowListPrice As Integer = 0
    Public _IsJPAonlineUser As Boolean = False, _IsTWAonlineUser As Boolean = False, _IsCNAonlineUser As Boolean = False
    Public _IsATWKAUser As Boolean = False
    Public _IsUSAUser As Boolean = False, _IsEUUser As Boolean = False, _IsEUAonlineUser As Boolean = False
    Public _IsUSAonline As Boolean = False, _IsUSAAC As Boolean = False, _IsKRAonlineUser As Boolean = False
    Public _IsHQDCUser As Boolean = False, _IsUSAENC As Boolean = False, _IsUSAOnlineEP As Boolean = False
    Public _IsABRUser As Boolean = False, _IsAAUUser As Boolean = False, _IsCAPSUser As Boolean = False, isBrandNewQuote As Boolean = False
    Function GetValFromForm() As Integer
        Dim errStr As String = ""

        If String.IsNullOrEmpty(Me.txtCustomId.Text.Trim.ToString) _
            AndAlso Not (_IsUSAUser OrElse _IsHQDCUser OrElse _IsABRUser OrElse _IsAAUUser OrElse _IsCAPSUser) Then

            errStr &= " Please input Quote Description. " : Me.txtCustomId.Focus()
            'txtCustomId.Text = getUID()
        End If
        If String.IsNullOrEmpty(Me.hfAccountRowId.Value.Trim.ToString) AndAlso Not String.IsNullOrEmpty(txtQuoteToName.Text.Trim) Then
            errStr &= " Quote-to customer does not exist in Siebel, please re-pick quote-to customer. " : Me.txtQuoteToName.Focus()
        End If
        If String.IsNullOrEmpty(Me.hfAccountRowId.Value.Trim.ToString) AndAlso String.IsNullOrEmpty(txtQuoteToName.Text.Trim) Then
            errStr &= " Please pick a quote-to customer first. " : Me.txtQuoteToName.Focus()
        End If
        If Not String.IsNullOrEmpty(errStr.Trim) Then
            Util.showMessage(errStr) : Return 0
        End If
        If Date.TryParse(Me.txtDeliveryDate.Text, Now) = False Then
            errStr &= " Delivery Date Format Error! " : Me.txtDeliveryDate.Focus()
        End If

        If Date.TryParse(Me.txtExpiredDate.Text, Now) = False Then
            errStr &= " Expired Date Format Error! " : Me.txtExpiredDate.Focus()
        End If
        If String.IsNullOrEmpty(Me.drpOrg.SelectedValue) Then
            errStr &= " Sales Org is not maintained correct." : Me.txtExpiredDate.Focus()
        End If
        If errStr <> "" Then
            Util.showMessage(errStr) : Return 0
        End If

        'ICC 2015/10/16 For ANA AOnline sales, check sales employee should not be null.
        If _IsUSAonline OrElse _IsEUUser Then
            If String.IsNullOrEmpty(drpSE.SelectedValue) Then
                Util.showMessage("Please select sales employee!") : Return 0
            End If
        End If

        If _IsUSAAC Then
            Dim _po As String = Me.txtRefPONO.Text
            Dim _org As String = Me.drpOrg.SelectedValue
            Dim erpid As String = Me.hfErpId.Text
            If Not String.IsNullOrEmpty(_po) AndAlso Not String.IsNullOrEmpty(_org) AndAlso Not String.IsNullOrEmpty(erpid) Then
                If Advantech.Myadvantech.Business.OrderBusinessLogic.IsPONumberExisting(_org, erpid, _po) Then
                    Util.showMessage("The PO# '" + _po + "' already exists!") : Return 0
                End If
            End If
        End If


        oCustomId = Me.txtCustomId.Text.Trim() : oCreatedBy = Me.txtCreatedBy.Text.Trim() : oQuoteDate = Me.txtQuoteDate.Text.Trim()
        oPrepareBy = Me.txtPreparedBy.Text.Trim() : oQuoteToName = Me.txtQuoteToName.Text.Trim() : hQuoteErpId = Me.hfErpId.Text.Trim()
        'hQuoteToAccountRowId = Me.hfAccountRowId.Value.Trim() : oOffice = Me.txtOffice.Text.Trim() : oCurrency = Me.drpCurrency.SelectedValue.Trim()
        hQuoteToAccountRowId = Me.hfAccountRowId.Value.Trim() : oOffice = Me.drpSalesOffice.SelectedItem.Text.Trim : oCurrency = Me.drpCurrency.SelectedValue.Trim()
        oSalesEmail = Me.txtSalesEmail.Text.Trim() : hSalesRowId = Me.hfSalesRowId.Value.Trim() : oDirectPhone = Me.txtDirectPhone.Text.Trim()
        oAttention = Me.txtAttention.Text.Trim() : hAttentionRowId = Me.hfAttentionRowId.Value.Trim() : oBankAccount = Me.txtBankInfo.Text.Trim()
        oDeliveryDate = Me.txtDeliveryDate.Text.Trim() : oExpiredDate = Me.txtExpiredDate.Text.Trim() : oShipTerms = Me.dlShipTerm.SelectedValue.Trim()
        oPaymentTerms = Me.dlPaymentTerm.SelectedValue.Trim() : oFreight = IIf(Me.txtFreight.Text.Trim() = "TBD", 0, Me.txtFreight.Text.Trim())
        oInsurance = Me.txtInsurance.Text.Trim() : oSpecialCharge = Me.txtSpecialCharge.Text.Trim()
        'Frank 2013/04/02 if tax value is empty, then put 0.05 to tax column
        oTax = IIf(Me.txtTax.Text.Trim() = "", 0.05, Decimal.Parse(Me.txtTax.Text.Trim()) / 100) : oQuoteNote = Me.txtQuoteNote.Text.Trim()
        oRelatedInfo = Me.txtRelatedInfo.Text.Trim() : oIsRepeatedOrder = IIf(Me.cbxIsRepeatedOrder.Checked, 1, 0)
        oQuoteNo = Me.hfQuoteNo.Value : oRevision_Number = Me.DDLRevisionNumber.SelectedValue : oActive = Me.cbxIsActive.Checked
        oCreatedDate = Me.hfCreatedDate.Value

        'Frank 2013/07/30
        oEmailGreeting = Me.txtEmailGreeting.Content.Trim : oSpecialTandC = Me.txtSpecialTandC.Text.Trim : oSignatureRowID = Me.HFSignatureID.Value
        oWarranty = Me.txtWarranty.Text.Trim
        oABRQuoteType = Me.ddlABRQuoteType.SelectedValue
        If Me.drpGroup.SelectedItem IsNot Nothing Then
            oGroup = Me.drpGroup.SelectedItem.Text.Trim()
        Else
            oGroup = ""
        End If

        If _IsKRAonlineUser Then
            oIsShowListPrice = Me.HFAKRQuoteListPriceOnly.Value
        End If

        oDelDateFlag = IIf(Me.cbxDelDateFlag.Checked, 1, 0) : oOrg = Me.drpOrg.SelectedValue.Trim() : siebelRBU = Me.h_office.Value.Trim()

        If oOffice.Equals("Select...", StringComparison.InvariantCultureIgnoreCase) Then oOffice = ""
        If oGroup.Equals("Select...", StringComparison.InvariantCultureIgnoreCase) Then oGroup = ""


        Return 1
    End Function

    Function SetValToForm(ByVal _CustomId As String, ByVal _CreatedBy As String, ByVal _QuoteDate As String, ByVal _PreparedBy As String, ByVal _QuoteToName As String,
                            ByVal _QuoteToRowId As String, ByVal _QuoteToErpId As String, ByVal _Office As String, ByVal _Currency As String,
                            ByVal _SalesEmail As String, ByVal _SalesRowId As String, ByVal _DirectPhone As String, ByVal _Attention As String,
                            ByVal _AttentionRowId As String, ByVal _BankAccount As String, ByVal _DeliveryDate As String, ByVal _ExpiredDate As String,
                            ByVal _ShipTerms As String, ByVal _PaymentTerms As String, ByVal _Freight As String, ByVal _Insurance As String,
                            ByVal _SpecialCharge As String, ByVal _Tax As String, ByVal _QuoteNote As String, ByVal _RelatedInfo As String,
                            ByVal _IsRepeatedOrder As Integer, ByVal _oGroup As String, ByVal _DelDateFlag As Integer, ByVal _Org As String,
                            ByVal _SiebelRBU As String, ByVal _SalesOfficeCode As String, ByVal _SalesGroupCode As String,
                            ByVal _QuoteNo As String, ByVal _Revision_Number As Integer, ByVal _Active As Boolean,
                            ByVal _EmailGreeting As String, ByVal _SpecialTandC As String, ByVal _SignatureRowID As String,
                            ByVal _Warranty As String, ByVal _ABRQuoteType As String, _CreatedDate As String) As Integer
        Me.txtCustomId.Text = _CustomId : Me.txtCreatedBy.Text = _CreatedBy : Me.txtQuoteDate.Text = _QuoteDate
        Me.txtPreparedBy.Text = _PreparedBy : Me.txtQuoteToName.Text = _QuoteToName : Me.hfAccountRowId.Value = _QuoteToRowId
        'Me.hfErpId.Text = _QuoteToErpId : Me.txtOffice.Text = _Office : Me.drpCurrency.SelectedValue = _Currency
        Me.hfErpId.Text = _QuoteToErpId : Me.drpCurrency.SelectedValue = _Currency
        Me.txtSalesEmail.Text = _SalesEmail : Me.hfSalesRowId.Value = _SalesRowId : Me.txtDirectPhone.Text = _DirectPhone
        Me.txtAttention.Text = _Attention : Me.hfAttentionRowId.Value = _AttentionRowId : Me.txtBankInfo.Text = _BankAccount
        Me.txtDeliveryDate.Text = _DeliveryDate : Me.txtExpiredDate.Text = _ExpiredDate : Me.dlShipTerm.SelectedValue = _ShipTerms


        Me.dlPaymentTerm.SelectedValue = _PaymentTerms
        'Frank 20140702
        If _IsEUUser AndAlso String.IsNullOrEmpty(_QuoteToErpId) Then

            'If String.IsNullOrEmpty(Me.hfErpId.Text.Trim) Then
            '    dlPaymentTerm.ClearSelection()
            '    Dim PPD As ListItem = dlPaymentTerm.Items.FindByValue("PPD")
            '    If PPD IsNot Nothing Then
            '        PPD.Selected = True
            '    End If
            'End If

            Me.dlPaymentTerm.SelectedValue = "PPD"
        End If

        If _IsTWAonlineUser OrElse _IsCNAonlineUser OrElse _IsKRAonlineUser _
            OrElse _IsHQDCUser OrElse _IsABRUser OrElse _IsAAUUser Then

            Dim _dt As DataTable = SiebelTools.GET_Contact_Info_by_RowID(_AttentionRowId)
            If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
                Me.txtAttentionName.Text = _dt.Rows(0).Item("LastName") & _dt.Rows(0).Item("FirstName")
            End If
        End If

        If _IsABRUser AndAlso Not String.IsNullOrEmpty(_ABRQuoteType) Then
            Me.ddlABRQuoteType.SelectedValue = _ABRQuoteType
        End If

        Me.txtFreight.Text = IIf(_Freight = "0.00" Or _Freight = "0", "TBD", _Freight) : Me.txtInsurance.Text = _Insurance
        Me.txtSpecialCharge.Text = _SpecialCharge : Me.txtTax.Text = _Tax : Me.txtQuoteNote.Text = _QuoteNote
        Me.txtRelatedInfo.Text = _RelatedInfo : Me.cbxIsRepeatedOrder.Checked = IIf(_IsRepeatedOrder = 1, True, False)
        Me.cbxDelDateFlag.Checked = IIf(_DelDateFlag = 1, True, False) : Me.drpOrg.SelectedValue = _Org : Me.h_office.Value = _SiebelRBU
        Me.hfCreatedDate.Value = _CreatedDate

        'Frank 2014/01/20 If account has ERPID, then currency cannot be changed
        'If Not String.IsNullOrEmpty(_QuoteToErpId) Then Me.drpCurrency.Enabled = False
        PutDLCurrency(_Currency, _QuoteToErpId)
        'Frank 2013/07/30

        Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
        If _ME IsNot Nothing Then
            Me.txtEmailGreeting.Content = _ME.EmailGreeting
            Me.txtSpecialTandC.Text = _ME.SpecialTandC
            Me.HFSignatureID.Value = _ME.SignatureRowID
            If _IsUSAENC Then
                Me.ddlAENCInsideSales.SelectedValue = _ME.Engineer
            Else
                TBEngineer.Text = _ME.Engineer
            End If

            TBEngineerTEL.Text = _ME.Engineer_Telephone
            Me.txtWarranty.Text = _ME.Warranty
            Dim qs As ListItem = ddlQuoteSource.Items.FindByValue(_ME.QuoteSource)
            If qs IsNot Nothing Then
                ddlQuoteSource.ClearSelection()
                qs.Selected = True
            End If

            If Not String.IsNullOrEmpty(_ME.ABRQuoteType) Then
                Me.ddlABRQuoteType.SelectedValue = _ME.ABRQuoteType
            End If

        End If
        Me.ShowSignature(Me.HFSignatureID.Value)
        'Frank 2013/04/02
        If Me._IsJPAonlineUser Then
            Me.LoadSalesOfficeByOrgID(_Org)
            'If Me.drpSalesOffice.SelectedIndex < 0 Then Me.drpSalesOffice.SelectedIndex = 0
        Else
            Me.LoadOnlyOneSalesOffice(_Office)
        End If

        'Frank 2013/07/02:Set QuoteNo, Revision Number and Active to user control
        Me.hfQuoteNo.Value = _QuoteNo : Me.cbxIsActive.Checked = _Active
        Me.ButtonMakeItActive.Visible = Not _Active

        'Frank 2013/07/02:Set QuoteNo, Revision Number and Active to user control
        Me.LoadRevisionNumberList(_QuoteNo) : Me.DDLRevisionNumber.SelectedValue = _Revision_Number

        If _IsKRAonlineUser Then
            Me.HFAKRQuoteListPriceOnly.Value = oIsShowListPrice
        End If

        'Me.drpSalesOffice.SelectedValue = _Office
        'Frank 2013/04/03:Office group options must be loaded when office selected value is being changed for AJP user.
        If _IsJPAonlineUser Then
            'Me.LoadSalesGroupByOfficeCodeAndOrgId(Me.drpSalesOffice.SelectedValue, _Org)

            Dim vListItem As ListItem = Me.drpSalesOffice.Items.FindByValue(_SalesOfficeCode)
            If Not Me.drpSalesOffice.Items.Contains(vListItem) Then
                Me.drpSalesOffice.SelectedIndex = 0
            Else
                Me.drpSalesOffice.SelectedValue = _SalesOfficeCode
            End If
            Me.LoadSalesGroupByOfficeCodeAndOrgId(Me.drpSalesOffice.SelectedValue, Me.drpOrg.SelectedValue)
            Me.drpGroup.SelectedValue = _SalesGroupCode
        End If

        Return 1
    End Function

    'Protected Sub SetSalesOffice_GroupDefaultSelected(ByVal _SalesOfficeCode As String, ByVal _SalesGroupCode As String)
    '    'Frank 2013/04/09: Set default selected value for both the SalesOffice and SalesGroup drop down list

    '    Dim vListItem As ListItem = Me.drpSalesOffice.Items.FindByValue(_SalesOfficeCode)
    '    If Not Me.drpSalesOffice.Items.Contains(vListItem) Then
    '        Me.drpSalesOffice.SelectedIndex = 0
    '    Else
    '        Me.drpSalesOffice.SelectedValue = _SalesOfficeCode
    '    End If
    '    Me.LoadSalesGroupByOfficeCodeAndOrgId(Me.drpSalesOffice.SelectedValue, Me.drpOrg.SelectedValue)
    '    Me.drpGroup.SelectedValue = _SalesGroupCode
    'End Sub


    Protected Sub Loadsetting(ByVal rbu As String)

        If _IsUSAUser Then
            Me.lbGroup.Visible = False : Me.drpGroup.Visible = False : Me.txtSalesEmail.ReadOnly = False : Me.txtSalesEmail.BackColor = Drawing.Color.White
            'ElseIf rbu = "ATW" Then
            'Me.lbPreparedBy.Visible = True : Me.txtPreparedBy.Visible = True
        End If

        If _IsHQDCUser OrElse _IsKRAonlineUser OrElse _IsTWAonlineUser Then
            Me.txtSalesEmail.ReadOnly = False : Me.txtSalesEmail.BackColor = Drawing.Color.White
        End If



    End Sub
    'work point       
    Protected Sub initInterFace(ByVal UID As String)

        'Ming 20150511 判断DocReg是否是NuLL, 如果是先update成当前Pivot.CurrentProfile.CurrDocReg
        Dim QM As Quote_Master = MyQuoteX.GetQuoteMaster(UID)
        If QM IsNot Nothing AndAlso (IsNothing(QM.DocReg) OrElse IsDBNull(QM.DocReg)) Then
            QM.DocReg = Pivot.CurrentProfile.CurrDocReg
            MyUtil.Current.CurrentDataContext.SubmitChanges()
        End If

        Dim QuoteDt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByDocID(UID, COMM.Fixer.eDocType.EQ)
        If Not IsNothing(QuoteDt) Then
            With QuoteDt
                oCustomId = .CustomId : oCreatedBy = .CreatedBy : oQuoteDate = .DocDate : oPrepareBy = .preparedBy : oQuoteToName = .AccName : hQuoteToAccountRowId = .AccRowId
                hQuoteErpId = .AccErpId : oOffice = .AccOfficeName : oCurrency = .currency : oSalesEmail = .salesEmail : hSalesRowId = .salesRowId : oDirectPhone = .directPhone
                oAttention = .attentionEmail : hAttentionRowId = .attentionRowId : oBankAccount = .bankInfo : oDeliveryDate = .deliveryDate : oExpiredDate = .expiredDate
                oShipTerms = .shipTerm : oPaymentTerms = .paymentTerm : oFreight = .freight : oInsurance = .insurance : oSpecialCharge = .specialCharge : oTax = FormatNumber(.tax * 100, 2)

                oQuoteNote = .quoteNote : oRelatedInfo = .relatedInfo : oIsRepeatedOrder = .isRepeatedOrder : oGroup = .AccGroupName : oDelDateFlag = .delDateFlag
                oOrg = .org : siebelRBU = .siebelRBU 'Me.cbxExempt.SelectedValue = .isExempt

                oCreatedDate = .CreatedDate

                oIsShowListPrice = .isShowListPrice

                'Hello Jay, 
                'I need your help to add EXW (Ex-Work) as an option in the incoterm field in eQuotation.
                'Starting now, we would need this incoterm whenever we are drop shipping to the customer.
                'We have a good amount of orders where the customer would rather drop ship than to wait for our ocean shipment
                'Denise.Kwong 2015/03/31
                If _IsUSAUser OrElse _IsHQDCUser Then
                    Me.DDLInco1.SelectedValue = .Inco1
                End If

                txtInco2.Text = .Inco2 : HFOriginalQuoteID.Value = .OriginalQuoteID
                oSalesOfficeCode = .AccOfficeCode : oSalesGroupCode = .AccGroupCode
                oQuoteNo = .quoteNo : oRevision_Number = .Revision_Number : oActive = .Active
                If .isExempt = 1 Then
                    cbxIsTaxExempt.Checked = True
                Else
                    cbxIsTaxExempt.Checked = False
                End If

                'Frank 2014/01/20: If quote is in draft stage, then drop down list for selecting expired date need to be shown up
                If _IsEUUser AndAlso QuoteDt.DOCSTATUS = COMM.Fixer.eDocStatus.QDRAFT Then
                    Me.DDLExpiredDays.Visible = True
                End If

            End With

            ''Frank 2013/04/02
            'If Me._IsJPAonlineUser Then
            '    Me.LoadSalesOfficeByOrgID()
            'Else
            '    Me.LoadOnlyOneSalesOffice()
            'End If

            'Frank 2013/07/30
            'Dim QuoteExtensionline As IBUS.iDOCHeaderExtensionLine = Pivot.NewObjDocHeaderExtension.GetQuoteExtension(UID)  '.GetyDocID(UID, COMM.Fixer.eDocType.EQ)
            'If Not IsNothing(QuoteExtensionline) Then
            '    With QuoteExtensionline
            '        oEmailGreeting = .EmailGreeting
            '        oSpecialTandC = .SpecialTandC
            '        oSignatureRowID = .SignatureRowID
            '    End With
            'End If

            SetValToForm(oCustomId, oCreatedBy, oQuoteDate, oPrepareBy, oQuoteToName, hQuoteToAccountRowId, hQuoteErpId, oOffice, oCurrency, oSalesEmail, hSalesRowId,
                         oDirectPhone, oAttention, hAttentionRowId, oBankAccount, oDeliveryDate, oExpiredDate, oShipTerms, oPaymentTerms, oFreight, oInsurance,
                         oSpecialCharge, oTax, oQuoteNote, oRelatedInfo, oIsRepeatedOrder, oGroup, oDelDateFlag, oOrg, siebelRBU, oSalesOfficeCode, oSalesGroupCode,
                         oQuoteNo, oRevision_Number, oActive, oEmailGreeting, oSpecialTandC, oSignatureRowID, oWarranty, oABRQuoteType, oCreatedDate)

            'txtCustomId.Text = UID
            'move create item before set value
            getSalesEmployee()
            INITOPTY(Request("UID")) : INITSALESEMAIL(Request("UID")) : INITSHIPBILL(Request("UID"))
            ' txtCustomId.Text = Request("UID")
            'set sales employee of the quotation

            txtRefPONO.Text = QuoteDt.PO_NO : txtCareOn.Text = QuoteDt.CARE_ON
            'txtSalesDistrict.Text = QuoteDt.SalesDistrict : txtSalesGroup.Text = QuoteDt.AccGroupName : txtSalesOffice.Text = QuoteDt.AccOfficeCode
            txtSalesDistrict.Text = QuoteDt.SalesDistrict : txtSalesGroup.Text = QuoteDt.AccGroupCode : txtSalesOffice.Text = QuoteDt.AccOfficeCode
            txtSalesDivision.Text = QuoteDt.DIVISION
            If dlDistChann.Items.FindByValue(QuoteDt.DIST_CHAN) IsNot Nothing Then dlDistChann.SelectedValue = QuoteDt.DIST_CHAN

            'If Me._IsJPAonlineUser Then Me.SetSalesOffice_GroupDefaultSelected(QuoteDt(0).SALESOFFICE, QuoteDt(0).SALESGROUP)

            'Ryan 20170217 Set EM block visibility
            EndCustomerSettings(oCustomId)

        End If

    End Sub

    Protected Sub initInterFaceDefault()

        oCreatedBy = Pivot.CurrentProfile.UserId : oQuoteDate = Now : oCustomId = ""
        oDeliveryDate = oQuoteDate.AddDays(3).Date

        'Frank 2013/04/02
        If Me._IsTWAonlineUser Then oTax = 5
        'Frank 2016/03/16
        If Me._IsJPAonlineUser Then oTax = 8
        'Frank 2015/08/25
        If Me._IsCNAonlineUser Then oTax = 17
        'Frank 2015/10/08
        If Me._IsKRAonlineUser Then oTax = 10
        'Frank 2015/10/27 Ning confirmed the default tax rate is 0%
        If Me._IsHQDCUser Then oTax = 0

        If _IsEUUser Then
            Dim usl As IBUS.iUserSignature = Pivot.NewObjUserSignature
            oSignatureRowID = usl.GetDefaultSignature(Pivot.CurrentProfile.UserId)
            usl = Nothing
        End If

        'Esther 20160603
        If _IsUSAENC Then
            Me.DDLInco1.SelectedValue = "EXW"
        End If


        SetValToForm(oCustomId, oCreatedBy, oQuoteDate, oPrepareBy, oQuoteToName, hQuoteToAccountRowId, hQuoteErpId, oOffice, oCurrency, oSalesEmail, hSalesRowId,
                     oDirectPhone, oAttention, hAttentionRowId, oBankAccount, oDeliveryDate, oExpiredDate, oShipTerms, oPaymentTerms, oFreight, oInsurance,
                     oSpecialCharge, oTax, oQuoteNote, oRelatedInfo, oIsRepeatedOrder, oGroup, oDelDateFlag, oOrg, siebelRBU, oSalesOfficeCode, oSalesGroupCode,
                     oQuoteNo, oRevision_Number, oActive, oEmailGreeting, oSpecialTandC, oSignatureRowID, oWarranty, oABRQuoteType, oCreatedDate)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''Frank 2013/04/02
        '_IsEUUser = Role.IsEUSales()
        '_IsUSAonlineUser = Role.IsUsaUser()
        '_IsTWAonlineUser = Role.IsTWAonlineSales()
        '_IsJPAonlineUser = Role.IsJPAonlineSales()


        If _IsEUUser Then
            ' Ryan 20161227 Get Copy purpose from database
            ' MasterExtension.CopyPurpose 0 means normal (non-EU)
            ' 1 means Create new quotation/opty, required to create new opty
            ' 2 means Revise quotation, required to pick existing quotation 
            Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
            If _ME IsNot Nothing Then
                If _ME.CopyPurpose.HasValue Then
                    oCopyPurpose = _ME.CopyPurpose

                    '===Ryan 20171005 Comment out AEU copy quotation opty logic.===
                    'If oCopyPurpose = 1 Then
                    '    ibtn_PickOpty.Visible = False
                    'ElseIf oCopyPurpose = 2 Then
                    '    ButtonNewOpty.Visible = False
                    'End If
                    '===End 20171005 Comment out===
                End If
            End If
        End If


        If Not IsPostBack Then

            'Frank 2013/10/18:Hide po file upload function in production site because we still waiting for John's API
            If _IsUSAUser AndAlso COMM.Util.IsTesting() Then
                Me.lbUploadPOFile.Visible = True : Me.updPO.Visible = True : Me.btnUploadPO.Visible = True
            Else
                Me.lbUploadPOFile.Visible = False : Me.updPO.Visible = False : Me.btnUploadPO.Visible = False
            End If

            'Frank 2013/07/31 Get default expired date by region
            oExpiredDate = Today.AddDays(COMM.MasterFixer.getExpDaysByReg(Pivot.CurrentProfile.CurrDocReg)).ToShortDateString

            'Frank 2013/07/29
            If _IsEUUser Then
                dlPaymentTerm.Enabled = False
                trEmployee.Visible = True
                If IsNothing(Request("UID")) OrElse String.IsNullOrEmpty(Request("UID")) Then
                    Me.DDLExpiredDays.Visible = True
                    If _IsEUAonlineUser Then
                        Dim _item As ListItem = DDLExpiredDays.Items.FindByValue("90")
                        If _item IsNot Nothing Then
                            DDLExpiredDays.ClearSelection()
                            _item.Selected = True
                        End If
                    End If
                    ShowSignature("")
                End If
            End If

            If _IsUSAUser Then
                lbPreparedBy.Visible = False : txtPreparedBy.Visible = False : trBankInfo.Visible = False : trDeliveryDate.Visible = False
                trFreightInsuSPTax.Visible = False : trQuoteNote.Visible = False : trLowGPReason.Visible = False : trShipTerm.Visible = False

                trDSGSO.Visible = True : tbOfficeGroup.Visible = False : txtQuoteDesc.Visible = False : trRefPO.Visible = True : trCareOn.Visible = False
                trSalesContactNum.Visible = False : trIncoterm.Visible = True : Me.trExempt.Visible = True
                trCurrencyRow.Visible = False
                If Not IsNothing(Request("UID")) AndAlso Not String.IsNullOrEmpty(Request("UID")) Then trReVisionControl.Visible = True
                dlDistChann_SelectedIndexChanged(Nothing, Nothing)
                Me.trSignature.Visible = False : trEmailGreeting.Visible = False : trSpecialTandC.Visible = False

                'Me.btnConfirm.OnClientClick = "return fnOpenNormalDialog();"
                'Me.btnConfirm.OnClientClick = "return confirm('Proceed?');"
            End If

            'Frank 2016-03-17
            'If _IsUSAENC Then
            If _IsUSAENC OrElse _IsCAPSUser Then
                trCustomerContact.Visible = False
                tableopty.Visible = False
                txtQuoteDesc.Visible = False
                divEngineer.Visible = True

                Label_Description.Visible = False : SP_star1.Visible = False
                txtCustomId.Visible = False ': Label_EngineerTel.Text = "Lead Time:"
                'Esther: leave this blank as default
                txtWarranty.Text = ""
                LabelEngineer.Visible = False : TBEngineer.Visible = False
                trInsideSales.Visible = True

                'Ryan 20170426 Get Inside Sales List data from AD table
                Dim str_IS As String = " select a.Name as NAME, a.PrimarySmtpAddress as EMAIL from AD_MEMBER a inner join AD_MEMBER_GROUP b " +
                                       " on a.PrimarySmtpAddress = b.EMAIL where b.GROUP_NAME = 'IS.AENC.USA' order by a.Name "
                Dim dt_IS As DataTable = tbOPBase.dbGetDataTable("MY", str_IS)
                If Not dt_IS Is Nothing AndAlso dt_IS.Rows.Count > 0 Then
                    For Each d As DataRow In dt_IS.Rows
                        ddlAENCInsideSales.Items.Add(New ListItem(d("NAME").ToString, d("EMAIL").ToString))
                    Next
                End If

            End If

            If _IsCAPSUser Then
                trSignature.Visible = False
                trQuoteNote.Visible = False
            End If

            'Nathan 20160504:please enable the Quote Description field. It does not need to be mandatory
            If _IsUSAAC Then
                txtQuoteDesc.Visible = True
                SP_star1.Visible = False
                divEngineer.Visible = False
            End If

            '因这部分会影响到GPcontrol，所以不能在eQuotation管控
            'If trDSGSO.Visible = False Then
            '    dlDistChann.ClearSelection()
            '    dlDistChann.SelectedIndex = 0
            'End If
            If _IsTWAonlineUser OrElse _IsCNAonlineUser Then
                trQuoteNote.Visible = False : trBankInfo.Visible = False
                trEmployee.Visible = False : trSignature.Visible = False
                txtAttentionName.Visible = True : txtAttention.Visible = False
                divEngineer.Visible = True
                If Not IsNothing(Request("UID")) AndAlso Not String.IsNullOrEmpty(Request("UID")) Then trReVisionControl.Visible = True
                Label_Description.Text = "Customer Quote No."
            End If

            If _IsKRAonlineUser Then
                tbOfficeGroup.Visible = False : trDeliveryDate.Visible = True
                trShipTerm.Visible = False : trFreightInsuSPTax.Visible = False
                trQuoteNote.Visible = False : trBankInfo.Visible = False
                trSalesContactNum.Visible = False : trSignature.Visible = False
                txtAttentionName.Visible = True : txtAttention.Visible = False
                trLowGPReason.Visible = False : trEmailGreeting.Visible = False
                cbxDelDateFlag.Visible = False
                trSpecialTandC.Visible = False
                If Not IsNothing(Request("UID")) AndAlso Not String.IsNullOrEmpty(Request("UID")) Then trReVisionControl.Visible = True
            End If

            If _IsHQDCUser Then
                tbOfficeGroup.Visible = False
                divEngineer.Visible = False : txtQuoteDesc.Visible = False
                trQuoteNote.Visible = False : trBankInfo.Visible = False
                trEmployee.Visible = False : trSignature.Visible = False
                txtAttentionName.Visible = True : txtAttention.Visible = False
                trLowGPReason.Visible = False : trEmailGreeting.Visible = False
                trSpecialTandC.Visible = False
                trIncoterm.Visible = True
                If Not IsNothing(Request("UID")) AndAlso Not String.IsNullOrEmpty(Request("UID")) Then trReVisionControl.Visible = True
                Me.DDLInco1.Items.Clear()
                Me.DDLInco1.Items.Add(New ListItem("Cost and freight", "CFR"))
                Me.DDLInco1.Items.Add(New ListItem("Cost, insureance & freight", "CIF"))
                Me.DDLInco1.Items.Add(New ListItem("Ex Wrks Taipei, Taiwan", "EW3"))
                Me.DDLInco1.Items.Add(New ListItem("FOB Advantech Taiwan", "FB5"))
                Me.DDLInco1.SelectedIndex = 3
            End If

            If _IsABRUser Then
                tbOfficeGroup.Visible = False
                divEngineer.Visible = False : txtQuoteDesc.Visible = False
                trQuoteNote.Visible = False : trBankInfo.Visible = False
                trEmployee.Visible = True : trSignature.Visible = False
                txtAttentionName.Visible = True : txtAttention.Visible = False
                trLowGPReason.Visible = False : trEmailGreeting.Visible = False
                trSpecialTandC.Visible = False : trABRQuoteType.Visible = True
                trFreightInsuSPTax.Visible = False : trIncoterm.Visible = True
                If Not IsNothing(Request("UID")) AndAlso Not String.IsNullOrEmpty(Request("UID")) Then trReVisionControl.Visible = True

                Me.DDLInco1.Items.Clear()
                Me.DDLInco1.Items.Add(New ListItem("Cost, insureance & freight", "CIF"))
                Me.DDLInco1.Items.Add(New ListItem("Free on board", "FOB"))
                Me.DDLInco1.SelectedIndex = 0

            End If
            If _IsAAUUser Then
                tbOfficeGroup.Visible = False
                divEngineer.Visible = False : txtQuoteDesc.Visible = False
                trQuoteNote.Visible = False : trBankInfo.Visible = False
                trEmployee.Visible = False : trSignature.Visible = False
                txtAttentionName.Visible = True : txtAttention.Visible = False
                trLowGPReason.Visible = False : trEmailGreeting.Visible = False
                trSpecialTandC.Visible = False
                If Not IsNothing(Request("UID")) AndAlso Not String.IsNullOrEmpty(Request("UID")) Then trReVisionControl.Visible = True
            End If


            'Lynette 20150811 for AAC can you change the default distribution channel to be “10”? Right now it is “30”
            'Chris 20151218 Please use KR01 / 10 / 00 for AKR's order/quote
            'Esther 20160412 Distribution Channel:  Default to 10
            'TC&Joyce 20170905 為了讓TWO的訂單也能在SAP內被copy, 請將distribution channel從30改為10
            If _IsUSAAC OrElse _IsKRAonlineUser OrElse _IsTWAonlineUser OrElse _IsUSAENC Then
                Me.dlDistChann.SelectedIndex = 1
                'ElseIf _IsUSAENC Then
                '    'Esther 20160412 Distribution Channel:  Default to 10
                '    Me.dlDistChann.SelectedIndex = 1
            End If

            If Role.IsFranchiser() Then
                trLowGPReason.Visible = False
                'trEmployee.Visible = False
            End If

            'Ming add 20150408 //ICC Move payment term before initial form 2015/4/22
            getPayMentTerm()

            If Not IsNothing(Request("UID")) AndAlso Business.isEidtableQuote(Request("UID")) Then
                initInterFace(Request("UID"))
            Else
                'ICC 2017/01/20 Use this para to seperate brand new quote
                Me.isBrandNewQuote = True
                initInterFaceDefault() : CType(Me.ascxPickAccount.FindControl("hType"), HiddenField).Value = "ALL"
                Me.UPPickAccount.Update() : Me.MPPickAccount.Show()
            End If
            If Role.IsInternalUser() Then
                txtShipToStreet.ReadOnly = False : txtShipToStreet2.ReadOnly = False
            End If

            'Frank 2013/04/02
            'If Role.IsJPAonlineSales() Then
            If _IsJPAonlineUser Then
                tableopty.Visible = False
                txtExpiredDate.ReadOnly = False : CalExt1.Enabled = True : Me.trBankInfo.Visible = False
                Me.LabelFreight.Visible = False : Me.txtFreight.Visible = False : Me.trSpecialChargeInsurance.Visible = False
                Me.LabelAlternativeSalesEmail.Visible = False : Me.txtSalesEmail1.Visible = False
                Me.drpSalesOffice.Enabled = True
                trSalesContactNum.Visible = False
                Me.trSignature.Visible = False : Me.trSpecialTandC.Visible = False : Me.trEmailGreeting.Visible = False
                txtTax.Enabled = False : ibtnPickAttention.Visible = False
                cpeDemo.Collapsed = False : cpeDemo.ClientState = False.ToString().ToLower()
                tdShipto.Visible = False

                If Pivot.CurrentProfile.UserId.Equals("yc.liu@advantech.com", StringComparison.InvariantCultureIgnoreCase) Then
                    Me.txtSalesEmail.ReadOnly = False : Me.txtSalesEmail.BackColor = Drawing.Color.White
                End If

                'Ryan 20170218 AJP users will use auto complete textbox to select employee
                Me.trEmployee.Visible = False : Me.trAJPEmployee.Visible = True
            End If

            'Ryan 20161019 Set Expired date dropdownlist visility to TW KA users.
            If _IsATWKAUser Then
                Me.DDLExpiredDays.Visible = True
                Dim _item As ListItem = DDLExpiredDays.Items.FindByValue("30")
                If _item IsNot Nothing Then
                    DDLExpiredDays.ClearSelection()
                    _item.Selected = True
                End If
            End If

        End If
    End Sub

    ''' <summary>
    ''' Load revision number items to drop down list
    ''' </summary>
    ''' <param name="quoteno"></param>
    ''' <remarks></remarks>
    Sub LoadRevisionNumberList(ByVal quoteno As String)
        Me.DDLRevisionNumber.Items.Clear()
        Dim _dt As DataTable = Business.getRevisionNumberList(quoteno)
        Dim _item As ListItem = Nothing, _ItemDisplayName As String = String.Empty
        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
            For Each _row As DataRow In _dt.Rows

                'If _row.Item("active") Then
                '    _ItemDisplayName = _row.Item("revision_number") & " (Active)"
                'Else
                _ItemDisplayName = _row.Item("revision_number")
                ' End If

                _item = New ListItem(_ItemDisplayName, _row.Item("revision_number"))

                Me.DDLRevisionNumber.Items.Add(_item)

                If _row.Item("active") Then
                    Me.DDLRevisionNumber.SelectedValue = _row.Item("revision_number")
                End If
            Next
        Else
            _item = New ListItem(1, 1)
            Me.DDLRevisionNumber.Items.Add(_item)
            Me.DDLRevisionNumber.SelectedValue = 1
        End If
    End Sub


    Sub LoadSalesOfficeByOrgID(ByVal orgid As String)

        'Dim orgid As String = "JP01"
        'If _IsJPAonlineUser Then orgid = "JP01"

        Me.drpSalesOffice.Items.Clear()
        Dim _dt As DataTable = Business.getSalesOfficeList(orgid)
        Me.drpSalesOffice.DataValueField = "SALES_OFFICE_CODE" : Me.drpSalesOffice.DataTextField = "SALES_OFFICE"
        Dim _newrow As DataRow = _dt.NewRow
        _newrow.Item("SALES_OFFICE") = "Select..."
        _dt.Rows.InsertAt(_newrow, 0)

        Me.drpSalesOffice.DataSource = _dt
        Me.drpSalesOffice.DataBind()
    End Sub

    Sub LoadOnlyOneSalesOffice(ByVal _office As String)
        Me.drpSalesOffice.Items.Clear()
        Me.drpSalesOffice.Items.Add(_office)
        Me.drpSalesOffice.SelectedIndex = 0
    End Sub


    Sub LoadSalesGroupByOfficeCodeAndOrgId(ByVal SalesOffice As String, ByVal orgid As String)

        Me.drpGroup.SelectedValue = Nothing
        Me.drpGroup.Items.Clear()

        If String.IsNullOrEmpty(SalesOffice) Then Exit Sub
        Dim _dt As DataTable = Business.getSalesGroupListByOffice(orgid, Me.drpSalesOffice.SelectedValue)
        Dim _newrow As DataRow = _dt.NewRow
        _newrow.Item("SALES_GROUP") = "Select..."
        _dt.Rows.InsertAt(_newrow, 0)

        Me.drpGroup.DataValueField = "SALES_GROUP_CODE" : Me.drpGroup.DataTextField = "SALES_GROUP"
        Me.drpGroup.DataSource = _dt
        Me.drpGroup.DataBind()

    End Sub

    Sub INITOPTY(ByVal UID As String)
        Dim myOptyQuote As New EQDSTableAdapters.optyQuoteTableAdapter, DT As EQDS.optyQuoteDataTable = myOptyQuote.GetOptyQuoteByOuoteID(UID)
        If DT.Rows.Count > 0 Then
            Me.txtOptyName.Text = DT(0).optyName : Me.txtOptyRowID.Text = DT(0).optyId
            If DT(0).optyId.Equals("NEW ID", StringComparison.InvariantCultureIgnoreCase) Then
                Me.cbx_NewOpty.Checked = True
            End If
        End If
    End Sub

    Sub INITSALESEMAIL(ByVal UID As String)
        Dim S123 As New SALESEMAIL123("EQ", "SALESEMAIL123")
        Dim DT As New DataTable
        DT = S123.GetDT(String.Format("QUOTEID='{0}'", UID), "SEQ")
        If DT.Rows.Count > 0 Then
            Me.txtSalesEmail1.Text = DT.Rows(0).Item("SALESEMAIL")

        End If
    End Sub

    Sub INITSHIPBILL(ByVal UID As String)
        Dim apt As New EQDSTableAdapters.EQPARTNERTableAdapter
        Dim dtPartners As EQDS.EQPARTNERDataTable = apt.GetPartnersByQuoteId(UID)
        For Each PartnerRow As EQDS.EQPARTNERRow In dtPartners
            Select Case PartnerRow.TYPE
                Case "S"
                    Me.txtShipToERPID.Text = PartnerRow.ERPID : Me.txtShipToName.Text = PartnerRow.NAME
                    Me.txtShipToStreet.Text = PartnerRow.STREET : Me.txtShipToCity.Text = PartnerRow.CITY
                    Me.txtShipToZip.Text = PartnerRow.ZIPCODE : Me.txtShipToState.Text = PartnerRow.STATE
                    Me.txtShipToCountry.Text = PartnerRow.COUNTRY : Me.txtShipToStreet2.Text = PartnerRow.STREET2
                    Me.txtShipToAttention.Text = PartnerRow.ATTENTION : Me.txtShipToTel.Text = PartnerRow.TEL
                Case "B"
                    Me.txtBillID.Text = PartnerRow.ERPID : Me.txtBillName.Text = PartnerRow.NAME
                    Me.txtBillToStreet.Text = PartnerRow.STREET : Me.txtBillToCity.Text = PartnerRow.CITY
                    Me.txtBillToZip.Text = PartnerRow.ZIPCODE : Me.txtBillToState.Text = PartnerRow.STATE
                    Me.txtBillToCountry.Text = PartnerRow.COUNTRY : Me.txtBillToStreet2.Text = PartnerRow.STREET2
                    Me.txtBillToAttention.Text = PartnerRow.ATTENTION : Me.txtBillToTel.Text = PartnerRow.TEL
                Case "EM"
                    Me.txtEMERPID.Text = PartnerRow.ERPID : Me.txtEMName.Text = PartnerRow.NAME
                    Me.txtEMStreet.Text = PartnerRow.STREET : Me.txtEMCity.Text = PartnerRow.CITY
                    Me.txtEMZip.Text = PartnerRow.ZIPCODE : Me.txtEMState.Text = PartnerRow.STATE
                    Me.txtEMCountry.Text = PartnerRow.COUNTRY : Me.txtEMStreet2.Text = PartnerRow.STREET2
                    Me.txtEMAttention.Text = PartnerRow.ATTENTION : Me.txtEMTel.Text = PartnerRow.TEL
                Case "SOLDTO"
                    lbSoldToStreet.Text = PartnerRow.STREET : lbSoldToCity.Text = PartnerRow.CITY
                    lbSoldToZipcode.Text = PartnerRow.ZIPCODE : lbSoldToState.Text = PartnerRow.STATE
                    lbSoldToCountry.Text = PartnerRow.COUNTRY
                    lbSoldToAttention.Text = PartnerRow.ATTENTION : lbSoldToTel.Text = PartnerRow.TEL
                Case "E"
                    Me.drpSE.SelectedIndex = -1 : Me.drpSE.SelectedValue = PartnerRow.ERPID
                Case "E2"
                    Me.drpSE2.SelectedIndex = -1 : Me.drpSE2.SelectedValue = PartnerRow.ERPID
                Case "E3"
                    Me.drpSE3.SelectedIndex = -1 : Me.drpSE3.SelectedValue = PartnerRow.ERPID
            End Select
        Next
    End Sub
    Protected Function getUID() As String
        If Not IsNothing(Request("UID")) AndAlso Business.isEidtableQuote(Request("UID")) Then
            Return Request("UID")
        End If

        'Frank 2013/07/01
        Dim O As IBUS.iDoc = Pivot.NewObjDoc
        Return O.NewUID
        'Return Business.GetNoByPrefix("GQ")
        'Return (New Doc).NewUID
        'Return Business.GetNoByPrefix(Pivot.CurrentProfile)
    End Function

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        If Me.txtOptyRowID.Text <> "" And Me.txtOptyName.Text = "" Then
            Util.showMessage("Opty name cannot be empty") : Exit Sub
        End If

        If _IsJPAonlineUser Then
            If String.IsNullOrEmpty(Me.txtEMERPID.Text.Trim) Then
                Util.showMessage("Please select an end customer first.")
                cpeDemo.Collapsed = False
                cpeDemo.ClientState = False.ToString().ToLower()
                Exit Sub
            End If

            'Ryan 20170817 Check if sales employee is seleceted
            If String.IsNullOrEmpty(Me.txtAJPSalesEmployee.Text.Trim) Then
                Util.showMessage("Please select a sales employee first.")
                cpeDemo.Collapsed = False
                cpeDemo.ClientState = False.ToString().ToLower()
                Exit Sub
            End If
        End If

        If GetValFromForm() = 1 Then
            goNext()
        End If
    End Sub

    Private Function IsNeedingChangeQuoteNumber()

    End Function

    Protected Sub goNext()

        'Frank 2013/07/01:New UID format will be c783e68dd74441a689ba55dd3e4521
        'AUSQ014230 will be saved in quoteNo column

        Dim UIDd As String = getUID()
        'Ming 20150610 取得之前的ShipTo
        Dim ShipTo As String = Business.GetShipToByQuoteID(UIDd)
        If Not logSalesEmail123(UIDd) Then
            Exit Sub
        End If
        If Not logShipToBillTo(UIDd) Then
            Exit Sub
        End If

        '===Ryan 20171005 Comment out AEU copy quotation opty logic.===        
        'Ryan 20161230 If is EU copied quotation and copypurpose = 1, not allowed to leave opty blank.
        'If _IsEUUser AndAlso String.IsNullOrEmpty(Me.txtOptyRowID.Text.Trim.Replace("'", "''")) AndAlso oCopyPurpose = 1 Then
        '    Util.showMessage("Please create a new opportunity for copied quotation, .")
        '    Exit Sub
        'End If
        '===End 20171005 Comment out===

        Dim fradd As Boolean = False
        Dim _IsChangedAccount As Boolean = isChangedAccount(UIDd)
        Dim _IsChangedCurrency As Boolean = isChangedCurrency(UIDd)
        Dim _IsChangedShipTo As Boolean = IIf(String.Equals(txtShipToERPID.Text.Trim, ShipTo, StringComparison.CurrentCultureIgnoreCase), False, True)
        Dim _IsChangedABRQuoteType As Boolean = False
        If _IsABRUser Then _IsChangedABRQuoteType = isChangedABRQuoteType(UIDd)

        If _IsChangedAccount OrElse _IsChangedCurrency OrElse _IsChangedShipTo OrElse _IsChangedABRQuoteType Then
            fradd = True
        End If
        'If isNeedReAdd(UIDd) Then
        '    fradd = True
        'End If

        Dim strDistChann As String = "", strDivision As String = "", strSalesGrp As String = "", strSalesOffice As String = "", strDistrict As String = ""
        If dlDistChann.SelectedIndex > 0 Then
            strDistChann = dlDistChann.SelectedValue : strDivision = txtSalesDivision.Text : strSalesGrp = txtSalesGroup.Text : strSalesOffice = txtSalesOffice.Text : strDistrict = txtSalesDistrict.Text
            If Pivot.CurrentProfile.CurrDocReg = COMM.Fixer.eDocReg.HQDC OrElse Pivot.CurrentProfile.CurrDocReg = COMM.Fixer.eDocReg.HQDC_EC OrElse Pivot.CurrentProfile.CurrDocReg = COMM.Fixer.eDocReg.HQDC_IA OrElse Pivot.CurrentProfile.CurrDocReg = COMM.Fixer.eDocReg.HQDC_IS OrElse Pivot.CurrentProfile.CurrDocReg = COMM.Fixer.eDocReg.AJP Then
                strDistChann = 10
            End If
        End If
        'If String.IsNullOrEmpty(oCustomId) Then oCustomId = UIDd
        If _IsUSAAC Then
            'please enable the Quote Description field. It does not need to be mandatory
        Else
            If String.IsNullOrEmpty(oCustomId) Then oCustomId = UIDd
        End If




        Dim strIncoterms As String = String.Empty

        'If _IsUSAUser Then strIncoterms = txtInco1.Text
        If _IsUSAUser OrElse _IsHQDCUser OrElse _IsABRUser Then strIncoterms = DDLInco1.SelectedValue

        If Me._IsJPAonlineUser Then
            If Me.drpSalesOffice.SelectedItem IsNot Nothing Then strSalesOffice = Me.drpSalesOffice.SelectedItem.Value
            If Me.drpGroup.SelectedItem IsNot Nothing Then strSalesGrp = Me.drpGroup.SelectedItem.Value
        End If
        If String.IsNullOrEmpty(siebelRBU.Trim) Then
            siebelRBU = Util.GetRBUforQuote(hQuoteToAccountRowId, hQuoteErpId)
        End If
        Dim M As IBUS.iDocHeaderLine = Pivot.NewLineHeader
        M.Key = UIDd
        M.CustomId = oCustomId
        M.AccRowId = hQuoteToAccountRowId
        M.AccErpId = hQuoteErpId
        M.AccName = oQuoteToName
        M.AccOfficeName = oOffice
        M.currency = oCurrency
        M.salesEmail = oSalesEmail
        M.salesRowId = hSalesRowId
        M.directPhone = oDirectPhone
        M.attentionRowId = hAttentionRowId
        M.attentionEmail = oAttention
        M.bankInfo = oBankAccount
        M.DocDate = oQuoteDate
        M.deliveryDate = oDeliveryDate
        M.expiredDate = oExpiredDate
        M.shipTerm = oShipTerms
        M.paymentTerm = oPaymentTerms
        M.freight = oFreight
        M.insurance = oInsurance
        M.specialCharge = oSpecialCharge
        M.tax = oTax
        M.quoteNote = oQuoteNote
        M.relatedInfo = oRelatedInfo
        M.preparedBy = oPrepareBy
        M.isRepeatedOrder = oIsRepeatedOrder
        M.AccGroupName = oGroup
        M.delDateFlag = oDelDateFlag
        M.org = oOrg
        M.siebelRBU = siebelRBU
        M.DIST_CHAN = strDistChann
        M.DIVISION = strDivision
        M.AccGroupCode = strSalesGrp
        M.AccOfficeCode = strSalesOffice
        M.PO_NO = txtRefPONO.Text
        M.CARE_ON = txtCareOn.Text
        M.isExempt = IIf(cbxIsTaxExempt.Checked, 1, 0)
        M.Inco1 = strIncoterms
        M.Inco2 = txtInco2.Text
        M.SalesDistrict = strDistrict
        M.OriginalQuoteID = HFOriginalQuoteID.Value
        M.DocReg = Pivot.CurrentProfile.CurrDocReg
        M.CreatedDate = oCreatedDate

        'Frank 20170822 I reuse quotationmaster.IsShowListPrice to note that if AKR team only quote the list price to their accounts
        If _IsKRAonlineUser Then
            M.isShowListPrice = oIsShowListPrice
        End If

        'Ryan 20170220 Add for Dora Wu case, save dora wu's mailgroup to KeyPerson field for temporary use.
        If Pivot.CurrentProfile.UserId.Equals("dora.wu@advantech.com.tw", StringComparison.InvariantCultureIgnoreCase) Then
            Dim groupforJoe As String = System.Web.HttpContext.Current.Session("Joe").ToString
            If Not String.IsNullOrEmpty(groupforJoe) Then
                If groupforJoe.Equals("Joebtn_1", StringComparison.InvariantCultureIgnoreCase) Then
                    M.KEYPERSON = "Sales.ATW.AOL-ATC(IIoT)".ToUpper()
                ElseIf groupforJoe.Equals("Joebtn_2", StringComparison.InvariantCultureIgnoreCase) Then
                    M.KEYPERSON = "Sales.ATW.AOL-EC".ToUpper()
                End If
            End If
        End If


        'Frank 2014/12/03
        If _IsChangedAccount AndAlso _IsUSAUser Then
            If Role.IsUSAACSales AndAlso (Role.IsAonlineUsa OrElse Role.IsAonlineUsaIag) Then

                Dim SiebelAccountApt As New SiebelDSTableAdapters.SIEBEL_ACCOUNTTableAdapter
                'Get old account's account status
                Dim acc As SiebelDS.SIEBEL_ACCOUNTDataTable = SiebelAccountApt.GetAccountByRowId(MasterRef.AccRowId)
                Dim _accenum As SAPDAL.UserRole.AccountStatus = SAPDAL.UserRole.AccountStatus.GA
                If acc.Rows.Count > 0 Then
                    Dim _accrow As SiebelDS.SIEBEL_ACCOUNTRow = acc.Rows(0)
                    _accenum = SAPDAL.UserRole.GetAccountStatusEnum(_accrow.ACCOUNT_STATUS)
                End If

                'Get new account's account status
                Dim newacc As SiebelDS.SIEBEL_ACCOUNTDataTable = SiebelAccountApt.GetAccountByRowId(hQuoteToAccountRowId)
                Dim _newaccenum As SAPDAL.UserRole.AccountStatus = SAPDAL.UserRole.AccountStatus.GA
                If newacc.Rows.Count > 0 Then
                    Dim _newaccrow As SiebelDS.SIEBEL_ACCOUNTRow = newacc.Rows(0)
                    _newaccenum = SAPDAL.UserRole.GetAccountStatusEnum(_newaccrow.ACCOUNT_STATUS)
                End If

                Select Case _accenum
                    Case SAPDAL.UserRole.AccountStatus.CP, SAPDAL.UserRole.AccountStatus.KA
                        If _newaccenum <> SAPDAL.UserRole.AccountStatus.CP AndAlso _newaccenum <> SAPDAL.UserRole.AccountStatus.KA Then
                            oQuoteNo = ""
                        End If
                    Case Else
                        If _newaccenum = SAPDAL.UserRole.AccountStatus.CP OrElse _newaccenum = SAPDAL.UserRole.AccountStatus.KA Then
                            oQuoteNo = ""
                        End If
                End Select
            End If

        End If
        'Frank 2013/07/01
        M.quoteNo = oQuoteNo

        M.Revision_Number = oRevision_Number
        M.Active = oActive
        Dim MO As IBUS.iDocHeader = Pivot.NewObjDocHeader()
        MO.AddByAssignedUID(UIDd, M, Pivot.CurrentProfile, COMM.Fixer.eDocType.EQ)

        'Dim HELine As IBUS.iDOCHeaderExtensionLine = Pivot.NewLineHeaderExtension
        Dim _Master_Extension As New Quote_Master_Extension
        _Master_Extension.QuoteID = UIDd
        _Master_Extension.EmailGreeting = oEmailGreeting
        _Master_Extension.SpecialTandC = oSpecialTandC
        _Master_Extension.SignatureRowID = oSignatureRowID
        _Master_Extension.LastUpdatedBy = Pivot.CurrentProfile.UserId
        _Master_Extension.LastUpdated = Now

        '_Master_Extension.Engineer = TBEngineer.Text.Trim
        If _IsUSAENC Then
            _Master_Extension.Engineer = ddlAENCInsideSales.SelectedValue
        Else
            _Master_Extension.Engineer = TBEngineer.Text.Trim
        End If

        _Master_Extension.Engineer_Telephone = TBEngineerTEL.Text.Trim
        _Master_Extension.QuoteSource = ddlQuoteSource.SelectedValue
        _Master_Extension.Warranty = oWarranty
        _Master_Extension.ApprovalFlowType = eQApprovalFlowType.Normal
        _Master_Extension.ABRQuoteType = oABRQuoteType


        'Ryan 20161228 Initial copy purpose 
        _Master_Extension.CopyPurpose = oCopyPurpose

        'Ryan 20170313 AJPOffice settings
        If _IsJPAonlineUser Then
            Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
            If Not _ME Is Nothing AndAlso Not String.IsNullOrEmpty(_ME.JPCustomerOffice) Then
                _Master_Extension.JPCustomerOffice = _ME.JPCustomerOffice
            Else
                _Master_Extension.JPCustomerOffice = 1
            End If

            'Ryan 20170531 If is AJP users and acoount changed
            If _IsChangedAccount Then
                tbOPBase.dbExecuteNoQuery("EQ", String.Format(" delete FROM QuotationExtensionAJP WHERE QUOTEID = '{0}'", UID))
            End If
        End If

        MyQuoteX.LogQuoteMasterExtension(_Master_Extension)
        'Dim HExtenObj As IBUS.iDOCHeaderExtension = Pivot.NewObjDocHeaderExtension
        'HExtenObj.SetQuoteExtension(HELine)


        'If Role.IsUsaUser() Then MO.ChangeDocStatus(UIDd, CInt(COMM.Fixer.eDocStatus.OFINISH))

        If fradd = True Then
            readd(UIDd)
        End If
        Dim optyId As String = Me.txtOptyRowID.Text.Trim.Replace("'", "''"), optyName As String = Me.txtOptyName.Text.Trim
        Dim optyStage As String = Me.DDLOptyStage.SelectedValue
        If Not String.IsNullOrEmpty(optyId) Then
            'Nada revised.....................................................
            Dim myOptyQuote As New EQDSTableAdapters.optyQuoteTableAdapter
            myOptyQuote.DeleteOptyByQuoteID(UIDd)
            Dim OptyQ As IBUS.iOptyQuote = Pivot.NewObjOptyQuote
            Dim optyQuote1 As IBUS.iOptyQuoteLine = Pivot.NewLineOptyQuote
            optyQuote1.optyId = optyId
            optyQuote1.optyName = optyName
            optyQuote1.quoteId = UIDd
            optyQuote1.optyStage = optyStage
            OptyQ.DeleteByQuoteID(UIDd) : OptyQ.Add(optyQuote1)
            '/Nada revised........................................................
        End If
        'myOptyQuote.InsertOptyQuote(optyId, optyName, UID, optyStage)
        Response.Redirect(String.Format("~/Quote/QuotationDetail.aspx?VIEW=0&UID=" & UIDd))
    End Sub

    Sub loadTaxSetting(ByVal CompanyId As String)
        If Me.trExempt.Visible = True Then
            'Me.cbxIsTaxExempt.Checked = IIf(Relics.SAPDAL.isTaxExempt(CompanyId), 1, 0)
            Me.cbxIsTaxExempt.Checked = IIf(SAPDAL.SAPDAL.isTaxExempt(CompanyId), 1, 0)
        End If
    End Sub

    'Function isNeedReAdd(ByVal UID As String) As Boolean
    '    If Not IsNothing(MasterRef) Then

    '        If MasterRef.AccRowId <> hQuoteToAccountRowId Then Return True

    '        'Frank 2014/01/20: If user changes currency, products need to bo readded to quotedetail by new currency
    '        If MasterRef.currency <> oCurrency Then Return True

    '    End If
    '    Return False
    'End Function

    ''' <summary>
    ''' Return true if the ABR quote type has been changed
    ''' </summary>
    ''' <param name="UID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function isChangedABRQuoteType(ByVal UID As String) As Boolean

        Dim _ME As Quote_Master_Extension = MyQuoteX.GetMasterExtension(UID)
        If _ME IsNot Nothing Then
            'Frank 2014/01/20: If ABR user changes quote type, products need to be readded to quotedetail by new quote type
            If _ME.ABRQuoteType <> oABRQuoteType Then Return True
        End If
        Return False
    End Function


    ''' <summary>
    ''' Return true if the currency has been changed
    ''' </summary>
    ''' <param name="UID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function isChangedCurrency(ByVal UID As String) As Boolean
        If Not IsNothing(MasterRef) Then
            'Frank 2014/01/20: If user changes currency, products need to be readded to quotedetail by new currency
            If MasterRef.currency <> oCurrency Then Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Return true if the quote-to account has been changed
    ''' </summary>
    ''' <param name="UID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function isChangedAccount(ByVal UID As String) As Boolean
        If Not IsNothing(MasterRef) Then
            If MasterRef.AccRowId <> hQuoteToAccountRowId Then Return True
        End If
        Return False
    End Function

    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        Me.txtEMERPID.Text = String.Empty : Me.txtEMName.Text = String.Empty
        Me.txtEMStreet.Text = String.Empty : Me.txtEMCity.Text = String.Empty
        Me.txtEMZip.Text = String.Empty : Me.txtEMState.Text = String.Empty
        Me.txtEMCountry.Text = String.Empty : Me.txtEMStreet2.Text = String.Empty
        Me.txtEMAttention.Text = String.Empty : Me.txtEMTel.Text = String.Empty
    End Sub

    Sub readd(ByVal UID As String)

        Dim myQD As New quotationDetail("EQ", "quotationDetail")
        Dim dt As New DataTable
        dt = myQD.GetDT(String.Format("quoteid='{0}'", UID), "line_no")
        If dt.Rows.Count > 0 Then
            'myQD.Delete(String.Format("quoteid='{0}'", UID))
            'Frank 2013/11/25: readd is important function for US AOnline sales. Only reload listprice and unitprice by new company id
            Dim unitPrice As Decimal = 0, listPrice As Decimal = 0, RecyclingFee As Decimal = 0
            Dim shipTo As String = hQuoteErpId
            If Not String.IsNullOrEmpty(txtShipToERPID.Text.Trim) Then
                shipTo = txtShipToERPID.Text.Trim
            End If
            Dim Qitems As List(Of QuoteItem) = MyQuoteX.GetQuoteList(UID)
            For Each r As DataRow In dt.Rows
                RecyclingFee = 0
                If r.Item("itemType") <> COMM.Fixer.eItemType.Parent Then
                    Dim dtPrice As New DataTable
                    Dim _ABRQuoteType As String = ""
                    If _IsABRUser Then
                        _ABRQuoteType = Me.ddlABRQuoteType.SelectedValue
                    End If
                    Business.GetPriceBiz(hQuoteErpId, shipTo, MasterRef.org, oCurrency, _ABRQuoteType, MasterRef.siebelRBU, r.Item("partNo"), MasterRef.CreatedBy, dtPrice, Pivot.CurrentProfile.CurrDocReg)
                    If Not IsNothing(dtPrice) AndAlso dtPrice.Rows.Count > 0 Then
                        unitPrice = dtPrice.Rows(0).Item("Netwr")
                        listPrice = dtPrice.Rows(0).Item("Kzwi1")
                        Dim itp As Decimal = 0
                        If (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AEU) = Pivot.CurrentProfile.CurrDocReg Then
                            itp = SAPDAL.SAPDAL.getItp(MasterRef.org, r.Item("partNo"), oCurrency, hQuoteErpId, SAPDAL.SAPDAL.itpType.EU)
                        ElseIf _IsUSAOnlineEP Then
                            'Ryan 20160802 USAonline users itp calculation
                            Business.isANAPnBelowGPV2(UID, r.Item("partNo"), unitPrice, itp, "")
                        ElseIf (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AJP) = Pivot.CurrentProfile.CurrDocReg Then
                            'Ryan 20171102 AJP users itp calculation
                            itp = SAPDAL.SAPDAL.getItp(MasterRef.org, r.Item("partNo"), oCurrency, hQuoteErpId, SAPDAL.SAPDAL.itpType.JP)
                        ElseIf (Pivot.CurrentProfile.CurrDocReg And COMM.Fixer.eDocReg.AKR) = Pivot.CurrentProfile.CurrDocReg Then
                            'Ryan 20171102 AKR users itp calculation
                            itp = SAPDAL.SAPDAL.getItp(MasterRef.org, r.Item("partNo"), oCurrency, hQuoteErpId, SAPDAL.SAPDAL.itpType.KR)
                        End If
                        '20150610 Ming get Recycling Fee
                        If String.Equals(MasterRef.org, "US01") Then
                            Dim _RecycleFee As Decimal = 0
                            If Not IsDBNull(dtPrice.Rows(0).Item("RECYCLE_FEE")) AndAlso Decimal.TryParse(dtPrice.Rows(0).Item("RECYCLE_FEE").ToString(), _RecycleFee) Then
                                RecyclingFee = _RecycleFee
                            End If
                        End If

                        'myQD.Update(String.Format("quoteid='{0}' and line_No={1}", UID, r.Item("line_no")), "listPrice=" & listPrice)
                        'myQD.Update(String.Format("quoteid='{0}' and line_No={1}", UID, r.Item("line_no")), "unitPrice=" & unitPrice)
                        'myQD.Update(String.Format("quoteid='{0}' and line_No={1}", UID, r.Item("line_no")), "newUnitPrice=" & unitPrice)
                        Dim _item As QuoteItem = Qitems.Find(Function(p) p.id = r.Item("id"))
                        If _item IsNot Nothing Then
                            _item.listPrice = listPrice
                            If RecyclingFee > 0 Then
                                _item.listPrice = listPrice - RecyclingFee
                                _item.unitPrice = unitPrice - RecyclingFee
                                _item.newUnitPrice = unitPrice - RecyclingFee
                                _item.RecyclingFee = RecyclingFee
                            Else
                                _item.newUnitPrice = unitPrice
                                _item.unitPrice = unitPrice
                                _item.RecyclingFee = 0
                            End If
                            _item.itp = itp
                            _item.newItp = itp
                        End If
                    End If
                End If
            Next

            If _IsABRUser Then
                Dim ABRDiscount As Decimal = 0
                Dim abrdiscountdt As DataTable = tbOPBase.dbGetDataTable("EQ", "select * from QuotationExtensionABR where quoteId = '" + UID + "'")
                If abrdiscountdt IsNot Nothing AndAlso abrdiscountdt.Rows.Count > 0 Then
                    ABRDiscount = Convert.ToDecimal(abrdiscountdt.Rows(0).Item("discount").ToString)
                End If
                Advantech.Myadvantech.Business.QuoteBusinessLogic.UpdateSystemPriceForABRQuote(UID, ABRDiscount)
            End If

            If Qitems.Count > 0 Then
                MyUtil.Current.CurrentDataContext.SubmitChanges()
            End If
        End If
    End Sub
    Protected Function logSalesEmail123(ByVal UID As String) As Boolean

        If _IsUSAUser Then

        ElseIf _IsKRAonlineUser Then

        ElseIf _IsJPAonlineUser Then
            If Not String.IsNullOrEmpty(Me.txtSalesEmail.Text) AndAlso Not Util.isEmail(Util.ReplaceSQLChar(Me.txtSalesEmail.Text.Trim)) Then
                Util.showMessage("Prepared For field is not in valid EMail format.")
                Me.txtSalesEmail.Focus()
                Return False
            Else
                hSalesRowId = SiebelTools.GET_ContactRowID_by_Email(oSalesEmail)
                Me.hfSalesRowId.Value = hSalesRowId
            End If
            If (Not Business.isValidSalesEmail(Util.ReplaceSQLChar(Me.txtSalesEmail1.Text.Trim))) Then
                Util.showMessage("Sales Email 1 is invalid.")
                Return False
            End If
        Else
            If (Not Business.isValidSalesEmail(Util.ReplaceSQLChar(Me.txtSalesEmail.Text.Trim))) Then
                Util.showMessage("Primary Sales Email is not defined in Siebel system.")
                Return False
            Else
                hSalesRowId = SiebelTools.GET_ContactRowID_by_Email(oSalesEmail)
                Me.hfSalesRowId.Value = hSalesRowId
            End If
            If (Not Business.isValidSalesEmail(Util.ReplaceSQLChar(Me.txtSalesEmail1.Text.Trim))) Then
                Util.showMessage("Sales Email 1 is invalid.")
                Return False
            End If
        End If
        Dim S123 As New SALESEMAIL123("EQ", "SALESEMAIL123")
        S123.Delete(String.Format("QUOTEID='{0}'", UID))
        S123.Add(UID, "1", Util.ReplaceSQLChar(Me.txtSalesEmail1.Text.Trim))
        Return True
    End Function

    Protected Function logShipToBillTo(ByVal UID As String) As Boolean
        Dim apt As New EQDSTableAdapters.EQPARTNERTableAdapter
        apt.DeleteQuotePartnersByQuoteId(UID)
        tbOPBase.adddblog("DELETE FROM EQPARTNER WHERE (QUOTEID = '" & UID & "')")
        'Dim strSoldToName As String = "", strSoldToAttention As String = "", strSoldToTel As String = "", strSoldToAddr As String = "", strSoldToMobile As String = ""
        Dim strSoldToAttention As String = "", strShipToAttention As String = "", strShipToTel As String = "", strShipToMobile = "", strBillToAttention As String = "", strBillToTel As String = "", strBillToMobile As String = "", strEMAttention As String = "", strEMTel As String = "", strEMMobile As String = ""
        '20120718 TC:If quote-to account has valid ERPID then get sold-to info from SAP, otherwise get from Siebel
        'If Not String.IsNullOrEmpty(Me.hfErpId.Text) Then
        '    SAPTools.GetSAPPartnerProfile(Me.hfErpId.Text, EnumSetting.PartnerTypes.SoldTo, strSoldToName, strSoldToAddr, strSoldToAttention, strSoldToTel, strSoldToMobile)
        'Else
        '    Dim aptAccount As New SiebelDSTableAdapters.SIEBEL_ACCOUNTTableAdapter
        '    Dim dtAccount As SiebelDS.SIEBEL_ACCOUNTDataTable = aptAccount.GetAccountByRowId(hQuoteRowId)
        '    If dtAccount.Count = 1 Then
        '        Dim rowAccount As SiebelDS.SIEBEL_ACCOUNTRow = dtAccount(0)
        '        With rowAccount
        '            strSoldToName = .ACCOUNT_NAME : strSoldToAddr = .ADDRESS + "<br />" + .CITY + ", " + .STATE + " " + .ZIPCODE + " " + .COUNTRY : strSoldToTel = .PHONE_NUM
        '        End With
        '    End If
        'End If
        If Util.isEmail(txtAttention.Text.Trim) Then
            Dim SiebConDAL As New SiebelDSTableAdapters.SIEBEL_CONTACTTableAdapter
            Dim SiebConDt As SiebelDS.SIEBEL_CONTACTDataTable = SiebConDAL.GetTopOneContactByEmail(txtAttention.Text.Trim)
            If SiebConDt.Rows.Count > 0 Then
                Dim row As SiebelDS.SIEBEL_CONTACTRow = SiebConDt.Rows(0)
                If row.FirstName + row.LastName <> "" Then
                    strSoldToAttention = row.FirstName + " " + row.LastName
                Else
                    strSoldToAttention = Util.GetNameVonEmail(txtAttention.Text.Trim)
                End If
            Else
                strSoldToAttention = Util.GetNameVonEmail(txtAttention.Text.Trim)
            End If
        Else
            strSoldToAttention = txtAttention.Text
        End If
        If strSoldToAttention = "" Then strSoldToAttention = lbSoldToAttention.Text
        If Not String.IsNullOrEmpty(Me.txtShipToERPID.Text) Then
            strShipToAttention = Me.txtShipToAttention.Text.Trim
            strShipToTel = Me.txtShipToTel.Text.Trim
            strShipToMobile = ""
            'SAPTools.GetSAPPartnerProfile(Me.txtShipToERPID.Text, EnumSetting.PartnerTypes.ShipTo, "", "", strShipToAttention, strShipToTel, strShipToMobile)
        End If
        If Not String.IsNullOrEmpty(Me.txtBillID.Text) Then
            strBillToAttention = Me.txtBillToAttention.Text.Trim
            strBillToTel = Me.txtBillToTel.Text.Trim
            strBillToMobile = ""
            'SAPTools.GetSAPPartnerProfile(Me.txtBillID.Text, EnumSetting.PartnerTypes.BillTo, "", "", strBillToAttention, strBillToTel, strBillToMobile)
        End If
        If Not String.IsNullOrEmpty(Me.txtEMERPID.Text) Then
            strEMAttention = Me.txtEMAttention.Text.Trim
            strEMTel = Me.txtEMTel.Text.Trim
            strEMMobile = ""
        End If

        strShipToAttention = txtShipToAttention.Text
        strShipToTel = txtShipToTel.Text
        strBillToAttention = txtBillToAttention.Text
        strBillToTel = txtBillToTel.Text
        strEMAttention = txtEMAttention.Text
        strEMTel = txtEMTel.Text

        apt.InsertPartner(UID, "", Me.hfErpId.Text.Trim.ToUpper, txtQuoteToName.Text, lbSoldToStreet.Text + " " + lbSoldToCity.Text + " " + lbSoldToState.Text + " " + lbSoldToZipcode.Text + " " + lbSoldToCountry.Text, "SOLDTO", strSoldToAttention, lbSoldToTel.Text, "", lbSoldToZipcode.Text, lbSoldToCountry.Text, lbSoldToCity.Text, lbSoldToStreet.Text, lbSoldToState.Text, "", "", HFsoldtofax.Value.Trim)
        apt.InsertPartner(UID, "", Me.txtShipToERPID.Text.Trim.ToUpper, txtShipToName.Text,
                          txtShipToStreet.Text + " " + txtShipToCity.Text + " " + txtShipToState.Text + " " + txtShipToZip.Text + " " + txtShipToCountry.Text,
                          "S", strShipToAttention, strShipToTel, strShipToMobile, txtShipToZip.Text, txtShipToCountry.Text, txtShipToCity.Text, txtShipToStreet.Text, txtShipToState.Text, "", txtShipToStreet2.Text, HFshiptofax.Value)
        apt.InsertPartner(UID, "", Me.txtBillID.Text.Trim.ToUpper, txtBillName.Text,
                          txtBillToStreet.Text + " " + txtBillToCity.Text + " " + txtBillToState.Text + " " + txtBillToZip.Text + " " + txtBillToCountry.Text,
                          "B", strBillToAttention, strBillToTel, strBillToMobile, txtBillToZip.Text, txtBillToCountry.Text, txtBillToCity.Text, txtBillToStreet.Text, txtBillToState.Text, "", txtBillToStreet2.Text, HFbilltofax.Value)

        'Ryan 20170213 Add end customer insertion
        apt.InsertPartner(UID, "", Me.txtEMERPID.Text.Trim.ToUpper, txtEMName.Text,
                          txtEMStreet.Text + " " + txtEMCity.Text + " " + txtEMState.Text + " " + txtEMZip.Text + " " + txtEMCountry.Text,
                          "EM", strEMAttention, strEMTel, strEMMobile, txtEMZip.Text, txtEMCountry.Text, txtEMCity.Text, txtEMStreet.Text, txtEMState.Text, "", txtEMStreet2.Text, HFEMfax.Value)

        'Ryan 20170215 If is AJP users, needs to insert Partner from tokeninput textbox
        If _IsJPAonlineUser Then
            apt.InsertPartner(UID, "", txtAJPSalesEmployee.Text, "", "", "E", "", "", "", "", "", "", "", "", "", "", "")
        Else
            '20120726 TC: Add sales employee code into EQPARTNER
            apt.InsertPartner(UID, "", drpSE.SelectedValue, "", "", "E", "", "", "", "", "", "", "", "", "", "", "")
        End If

        If drpSE2.SelectedIndex > 0 Then
            apt.InsertPartner(UID, "", drpSE2.SelectedValue, "", "", "E2", "", "", "", "", "", "", "", "", "", "", "")
        End If
        If drpSE3.SelectedIndex > 0 Then
            apt.InsertPartner(UID, "", drpSE3.SelectedValue, "", "", "E3", "", "", "", "", "", "", "", "", "", "", "")
        End If
        Return True
    End Function
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'Frank 2013/04/02
        _IsEUUser = Role.IsEUSales()
        If _IsEUUser Then
            _IsEUAonlineUser = Pivot.CurrentProfile.IsAOnLineSales()
        End If
        _IsUSAUser = Role.IsUsaUser()
        _IsUSAOnlineEP = Role.IsAonlineUsa()
        _IsUSAonline = Role.IsAonlineUsa() OrElse Role.IsAonlineUsaIag() OrElse Role.IsAonlineUsaISystem()
        _IsUSAENC = Role.IsUsaAenc()
        _IsCAPSUser = Role.IsCAPS()

        '_IsUSAAC = Role.IsUSAACSales()
        If Role.IsUSAACSales() AndAlso Pivot.CurrentProfile.SalesOfficeCode.Contains("2100") Then
            _IsUSAAC = True
        End If

        _IsTWAonlineUser = Role.IsTWAonlineSales()
        _IsCNAonlineUser = Role.IsCNAonlineSales()
        _IsJPAonlineUser = Role.IsJPAonlineSales()
        _IsKRAonlineUser = Role.IsKRAonlineSales()
        _IsHQDCUser = Role.IsHQDCSales()
        _IsABRUser = Role.IsABRSales()
        _IsAAUUser = Role.IsAAUSales()

        'Ryan 20161019 Add ATW KA user validation
        If _IsTWAonlineUser Then
            Dim MG As New PROF.MailGroup
            Dim GroupBelTo As ArrayList = MG.getMailGroupArray(Pivot.CurrentProfile.UserId)
            If Role.IsATWKASales(GroupBelTo) AndAlso Not Role.IsATWAonlineSales(GroupBelTo) Then
                _IsATWKAUser = True
            End If
        End If


        If Not IsPostBack Then
            If Not IsNothing(MasterRef) Then
                If MasterRef.DocType <> COMM.Fixer.eDocType.EQ Then
                    Util.showMessage("Only Quotation is editable.", "back")
                End If
            End If
            'Ryan 2015/12/15 Correct the executing order to avoid Org-setting error.
            getOrg() : getShipTerm()
        End If
    End Sub
    Protected Sub getOrg()
        If Not IsNothing(Request("UID")) AndAlso Request("UID") <> "" Then
            If Not IsNothing(MasterRef) Then
                drpOrg.Items.Clear()
                drpOrg.Items.Add(New ListItem(MasterRef.org, MasterRef.org))
            End If
        Else
            Dim c As String = Me.hfErpId.Text
            If c <> "" Then
                Dim str As String = String.Format("select DISTINCT ORG_id from SAP_DIMCOMPANY where COMPANY_ID='{0}' and org_id not in " + ConfigurationManager.AppSettings("InvalidOrg"), c)

                'Ryan 20180710 Fix for Intercon TW20
                'Frank 2015/11/16
                If _IsTWAonlineUser Or _IsATWKAUser Or _IsHQDCUser Then
                    str = String.Format("select DISTINCT ORG_id from SAP_DIMCOMPANY where COMPANY_ID='{0}' and org_id like 'TW%'", c)
                End If
                Dim dt As New DataTable
                dt = tbOPBase.dbGetDataTable("B2B", str)
                If dt.Rows.Count > 0 Then
                    drpOrg.Items.Clear()
                    For Each X As DataRow In dt.Rows
                        drpOrg.Items.Add(New ListItem(X.Item("org_id"), X.Item("org_id")))
                    Next
                End If
            Else
                'Frank 2013/04/03:Sales Office user control was changed to drop down list
                'Dim office As String = Me.txtOffice.Text.Trim
                Dim office As String = Me.drpSalesOffice.SelectedValue
                If String.IsNullOrEmpty(office) AndAlso Me._IsJPAonlineUser Then
                    office = "AJP"
                End If
                If office <> "" Then
                    Dim org As String = Pivot.CurrentProfile.getCurrOrg
                    drpOrg.Items.Clear()
                    drpOrg.Items.Add(org)
                End If
            End If
        End If

        'Ryan 20161116 Hard code AJP org to JP01 and disable drpOrg
        If Me._IsJPAonlineUser Then
            For Each X As ListItem In drpOrg.Items
                If X.Value = "JP01" Then
                    X.Selected = True
                End If
            Next
            drpOrg.Enabled = False
        Else
            For Each X As ListItem In drpOrg.Items
                If X.Value = "EU10" Then
                    X.Selected = True
                End If
            Next
        End If

        'Ryan 20180710 Take TW20 first if exists.
        If _IsTWAonlineUser OrElse _IsHQDCUser Then
            If drpOrg.Items.FindByValue("TW20") IsNot Nothing Then
                Dim sapcnt As Object = OraDbUtil.dbExecuteScalar("SAP_PRD", String.Format("select COUNT(*) as count from SAPRDP.KNVV WHERE KUNNR = '{0}' AND VKORG = 'TW20' AND AUFSD = ' '", Me.hfErpId.Text.ToUpper))
                If sapcnt IsNot Nothing AndAlso Integer.Parse(sapcnt) > 0 Then
                    'Only allow to change to TW20
                    For x = drpOrg.Items.Count - 1 To 0 Step -1
                        If Not drpOrg.Items(x).Value.Equals("TW20", StringComparison.OrdinalIgnoreCase) Then
                            drpOrg.Items.RemoveAt(x)
                        End If
                    Next
                Else
                    'Remove TW20 if blocked
                    For x = drpOrg.Items.Count - 1 To 0 Step -1
                        If drpOrg.Items(x).Value.Equals("TW20", StringComparison.OrdinalIgnoreCase) Then
                            drpOrg.Items.RemoveAt(x)
                        End If
                    Next
                End If
            Else
                For x = drpOrg.Items.Count - 1 To 0 Step -1
                    If drpOrg.Items(x).Value.StartsWith("TW", StringComparison.OrdinalIgnoreCase) AndAlso Not drpOrg.Items(x).Value.Equals("TW01", StringComparison.OrdinalIgnoreCase) Then
                        drpOrg.Items.RemoveAt(x)
                    End If
                Next
            End If
        End If

        If drpOrg.Items.Count = 0 Then
            drpOrg.Items.Add(Pivot.CurrentProfile.getCurrOrg)
        End If


        'setExpiredDate(drpOrg.SelectedValue)
    End Sub
    'Sub setExpiredDate(ByVal Org As String)
    '    'Frank 2013/07/29: Requirement from EU Emil, Open the door to change the expire date for certain customers
    '    'Dim days As Integer = Business.getExipDay(Org)
    '    Dim days As Integer = 45
    '    If _IsEUUser Then
    '        days = Me.DDLExpiredDays.SelectedValue
    '    Else
    '        days = Business.getExipDay(Org)
    '    End If

    '    Me.txtExpiredDate.Text = Now.AddDays(days).ToShortDateString
    'End Sub
    Protected Sub getPayMentTerm()
        'Dim strERPID As String = Me.hfErpId.Text

        Dim strERPID As String = Me.txtBillID.Text

        'Ryan 20171116 For EU10 take SoldtoID as default per Sigrid's request.
        If _IsEUUser Then
            strERPID = Me.hfErpId.Text
        End If

        If String.IsNullOrEmpty(strERPID) Then
            strERPID = Me.hfErpId.Text
        End If

        'If Not IsNothing(Request("UID")) AndAlso Request("UID") <> "" Then


        '    If Not IsNothing(MasterRef) Then
        '        strERPID = MasterRef.AccErpId
        '    End If
        'End If
        dlPaymentTerm.Items.Clear()
        Dim dtPt As DataTable = Business.getSapPayTermsFORANA(Pivot.CurrentProfile.getCurrOrg)
        If dtPt.Rows.Count > 0 Then
            For Each X As DataRow In dtPt.Rows
                If X.Item("name") Is DBNull.Value Or X.Item("value") Is DBNull.Value Then
                    X.Item("name") = ""
                End If
                dlPaymentTerm.Items.Add(New ListItem(X.Item("name"), X.Item("value")))
            Next

            'If String.IsNullOrEmpty(Me.hfErpId.Text.Trim) = False Then
            If String.IsNullOrEmpty(strERPID) = False Then
                Dim PTV As String = "", PTN As String = ""
                'Business.getPayMentTrem(Me.hfErpId.Text.Trim, PTV, PTN)
                Business.getPayMentTrem(strERPID, PTV, PTN, Me.drpOrg.SelectedValue)
                If PTV <> "" Then
                    For Each X As ListItem In Me.dlPaymentTerm.Items
                        If X.Value = PTV Then
                            X.Selected = True
                        End If
                    Next
                End If
            Else
                'Frank 2012/08/17:If pick account no have erp id, then default value of paymentTerm is CODC(credit card)
                'dlPaymentTerm.SelectedIndex = 0
                For Each l As ListItem In dlPaymentTerm.Items
                    If l.Value = "CODC" Then
                        l.Selected = True
                        Exit For
                    End If
                Next
                'dlPaymentTerm.SelectedValue = "CODC"
            End If
            'Ming add 2014-6-26
            ' 1.  eQuotation with SAP ERP ID:   Payment term always default/Equal  from SAP customer master settings, not allowed to change those payment terms in eQuotation
            ' 2. eQuotation with Siebel account:  Always PPD.  (Payment term possible after payment credit assessment)
            If _IsEUUser Or _IsKRAonlineUser Then

                'Ryan 20170803 AKR payment terms are hard coded to Korean Value per Nadia's request
                If _IsKRAonlineUser Then
                    dlPaymentTerm.Items.Clear()
                    dlPaymentTerm.Items.Add(New ListItem("납품후 14일 이내", "I014"))
                    dlPaymentTerm.Items.Add(New ListItem("납품후 30일 이내", "I030"))
                    dlPaymentTerm.Items.Add(New ListItem("익월말 15일", "M015"))
                    dlPaymentTerm.Items.Add(New ListItem("익월말 현금", "M030"))
                    dlPaymentTerm.Items.Add(New ListItem("익월말 45일", "M045"))
                    dlPaymentTerm.Items.Add(New ListItem("익익월말 현금", "M060"))
                    dlPaymentTerm.Items.Add(New ListItem("귀사 결제 조건", "M090"))
                    dlPaymentTerm.Items.Add(New ListItem("주문시 선입금", "PPD"))
                End If

                If String.IsNullOrEmpty(strERPID) Then
                    dlPaymentTerm.ClearSelection()
                    Dim PPD As ListItem = dlPaymentTerm.Items.FindByValue("PPD")
                    If PPD IsNot Nothing Then
                        PPD.Selected = True
                    End If
                End If
            End If
        End If
    End Sub
    Protected Sub getShipTerm()
        If drpOrg.SelectedValue = "EU10" Then
            dlShipTerm.Items.Clear() : dlShipTerm.Items.Add(New ListItem("EX Works", "EX Works"))
        Else
            dlShipTerm.Items.Clear()
            'Frank 20170220 Get ship term list by org id
            'Dim dtSt As DataTable = Business.getSapShipTermsFORANA("US01")
            Dim dtSt As DataTable = Business.getSapShipTermsByOrg(Pivot.CurrentProfile.getCurrOrg)
            If dtSt.Rows.Count > 0 Then
                dlShipTerm.Items.Add(New ListItem("TBD", 0))
                For Each X As DataRow In dtSt.Rows
                    If X.Item("name") Is DBNull.Value Or X.Item("value") Is DBNull.Value Then
                        X.Item("name") = ""
                    End If
                    dlShipTerm.Items.Add(New ListItem(X.Item("name"), X.Item("value")))
                Next
                If Not String.IsNullOrEmpty(Me.hfErpId.Text.Trim()) Then
                    Dim STV As String = "", STN As String = ""
                    Business.getShipTrem(Me.hfErpId.Text.Trim, STV, STN)
                    If STV <> "" Then

                        For Each X As ListItem In Me.dlShipTerm.Items
                            If X.Value = STV Then
                                X.Selected = True
                            End If
                        Next
                    End If
                Else
                    dlShipTerm.SelectedIndex = 0
                End If
            End If

        End If
    End Sub


    Protected Sub getSalesEmployee()
        Dim _orgid As String = Me.drpOrg.Text

        Dim blFoundCurrentSales As Boolean = False, strCurrentSalesEmail As String = Pivot.CurrentProfile.UserId
        Dim defaultSalesCode As String = Business.getDefaultSalesEmployee(_orgid, Me.hfErpId.Text)
        If Not IsNothing(Request("UID")) AndAlso Request("UID") <> "" Then

            If Not IsNothing(MasterRef) Then
                _orgid = MasterRef.org
                'please set strCurrentSalesEmail of the quote
                'strCurrentSalesEmail = DT.Rows(0).Item("")
            End If
        End If

        drpSE.Items.Clear() : drpSE2.Items.Clear() : drpSE3.Items.Clear()
        'Frank 2012/09/14:The getSalesEmployeeList functin does not join the SAP_COMPANY_EMPLOYEE.
        Dim dtSE As DataTable = Business.getSalesEmployeeList(_orgid, Me.hfErpId.Text)

        If dtSE.Rows.Count > 0 Then
            drpSE.Items.Add(New ListItem("Select...", ""))
            For Each X As DataRow In dtSE.Rows
                If X.Item("SALES_CODE") Is DBNull.Value Or X.Item("FULL_NAME") Is DBNull.Value Then
                    X.Item("FULL_NAME") = ""
                End If
                Dim li As New ListItem(X.Item("FULL_NAME") & " (" & X.Item("SALES_CODE") & ")", X.Item("SALES_CODE"))
                drpSE.Items.Add(li)
                '20120716 TC: Only when user is US employee then select current user as default sales employee
                'If blFoundCurrentSales = False AndAlso String.Equals(X.Item("EMAIL"), strCurrentSalesEmail, StringComparison.CurrentCultureIgnoreCase) _
                '    AndAlso strCurrentSalesEmail.EndsWith("@advantech.com", StringComparison.CurrentCultureIgnoreCase) Then
                '    li.Selected = True : blFoundCurrentSales = True
                'End If

                'If blFoundCurrentSales = False AndAlso String.Equals(X.Item("EMAIL"), strCurrentSalesEmail, StringComparison.CurrentCultureIgnoreCase) _
                '    AndAlso Role.IsUsaUser() Then
                '    li.Selected = True : blFoundCurrentSales = True
                'End If
                '20130716 Nada revised for Jay's request ,to auto select default sales employee
                If blFoundCurrentSales = False AndAlso String.Equals(X.Item("SALES_CODE"), defaultSalesCode, StringComparison.OrdinalIgnoreCase) Then
                    li.Selected = True : blFoundCurrentSales = True
                End If
            Next
            For Each r As ListItem In drpSE.Items
                drpSE2.Items.Add(New ListItem(r.Text, r.Value))
                drpSE3.Items.Add(New ListItem(r.Text, r.Value))
            Next
        End If
    End Sub

    Protected Sub ibtnPickAccount_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPickAccount.Click
        CType(Me.ascxPickAccount.FindControl("hType"), HiddenField).Value = "ALL"
        'ICC 2017/01/20 Set focus to account name
        Me.ascxPickAccount.TxtAccountName.Focus()
        Me.UPPickAccount.Update() : Me.MPPickAccount.Show()
    End Sub
    Protected Sub PutDLCurrency(ByVal currency As String, ByVal QuoteToErpId As String)
        If (_IsTWAonlineUser Or _IsCNAonlineUser) AndAlso Not String.IsNullOrEmpty(QuoteToErpId) Then
            drpCurrency.Items.Clear()
            If Not String.IsNullOrEmpty(currency) Then
                drpCurrency.Items.Add(New ListItem(currency))
            End If
            If drpCurrency.Items.FindByValue("TWD") Is Nothing Then drpCurrency.Items.Add(New ListItem("TWD"))
            If drpCurrency.Items.FindByValue("USD") Is Nothing Then drpCurrency.Items.Add(New ListItem("USD"))
            If Not String.IsNullOrEmpty(currency) Then
                drpCurrency.ClearSelection() : drpCurrency.Items.FindByValue(currency).Selected = True
            End If
            Me.drpCurrency.Enabled = True
        ElseIf _IsKRAonlineUser AndAlso Not String.IsNullOrEmpty(QuoteToErpId) Then
            drpCurrency.Items.Clear()
            If Not String.IsNullOrEmpty(currency) Then
                drpCurrency.Items.Add(New ListItem(currency))
            End If
            If drpCurrency.Items.FindByValue("KRW") Is Nothing Then drpCurrency.Items.Add(New ListItem("KRW"))
            If drpCurrency.Items.FindByValue("USD") Is Nothing Then drpCurrency.Items.Add(New ListItem("USD"))
            If Not String.IsNullOrEmpty(currency) Then
                drpCurrency.ClearSelection() : drpCurrency.Items.FindByValue(currency).Selected = True
            End If
            Me.drpCurrency.Enabled = True
        Else
            Me.drpCurrency.Enabled = False
        End If
        If String.IsNullOrEmpty(QuoteToErpId) Then
            Me.drpCurrency.Enabled = True
        End If
    End Sub
    Public Sub PickAccountEnd(ByVal str As Object)
        'btnConfirm.Enabled = True
        gvERPIDCreditBlockMsg.DataSource = Nothing : gvERPIDCreditBlockMsg.DataBind() : trCreditBlockMsg.Visible = False
        txtInco2.Text = ""

        hQuoteToAccountRowId = str(0).ToString : hQuoteErpId = IIf(Business.is_Valid_Company_Id(str(1).ToString), str(1).ToString, "") : oQuoteToName = str(2).ToString
        If Not String.IsNullOrEmpty(str(1).ToString.Trim) AndAlso Not String.Equals(hQuoteErpId, str(1).ToString.Trim, StringComparison.InvariantCultureIgnoreCase) Then
            Dim errMag As String = String.Format("This account’s ERPID ""{0}"" is invalid either because it does not exist in SAP or it is not a sold-to account", str(1).ToString)
            Util.AjaxShowLoading(Me.UPForm, String.Empty, errMag, 3)
            Util.SendEmail("myadvantech@advantech.com", "myadvantech@advantech.com", "", "", "eQuotation Error Erpid by " + Pivot.CurrentProfile.UserId.ToString, "", str(1).ToString + vbNewLine + hQuoteErpId, "")
        End If
        If _IsUSAUser Then
            oSalesEmail = Pivot.CurrentProfile.UserId
        Else
            oSalesEmail = SiebelTools.getPriSalesEmailByAccountROWID(hQuoteToAccountRowId)
        End If

        'Frank 20140214
        Dim _SalesRealEmail As String = SiebelTools.TransferOpportunityOwnerEmail(oSalesEmail)
        hSalesRowId = SiebelTools.GET_ContactRowID_by_Email(_SalesRealEmail)
        'hSalesRowId = SiebelTools.GET_ContactRowID_by_Email(oSalesEmail)

        'Ryan 20160830 If erpid's org is TWXX but not TW01, set is as blank and go to NO-ERPID case.
        Dim tw_str As String = "select distinct ORG_ID from SAP_DIMCOMPANY where COMPANY_ID = '" + hQuoteErpId + "'"
        Dim tw_dt As DataTable = tbOPBase.dbGetDataTable("B2B", tw_str)
        If Not tw_dt Is Nothing AndAlso tw_dt.Rows.Count > 0 Then
            If tw_dt.Rows.Count = 1 AndAlso tw_dt.Rows(0).Item("ORG_ID").ToString.StartsWith("TW") AndAlso Not tw_dt.Rows(0).Item("ORG_ID").ToString.Equals("TW01") Then
                hQuoteErpId = ""
            End If
        End If


        Dim SalesInfoDT As DataTable = SiebelTools.GET_Contact_Info_by_RowID(hSalesRowId)
        If Not IsNothing(SalesInfoDT) AndAlso SalesInfoDT.Rows.Count > 0 Then
            oDirectPhone = Business.FormatTel(SalesInfoDT.Rows(0).Item("WorkPhone").ToString())
        End If

        Dim RBU As String = "", curr As String = ""

        Dim aptSiebAccount As New SiebelDSTableAdapters.SIEBEL_ACCOUNTTableAdapter
        Dim dtAccount As SiebelDS.SIEBEL_ACCOUNTDataTable = aptSiebAccount.GetAccountByRowId(hQuoteToAccountRowId)
        If dtAccount.Count > 0 Then
            Dim AccountRow As SiebelDS.SIEBEL_ACCOUNTRow = dtAccount(0)
            RBU = AccountRow.RBU : siebelRBU = RBU
            lbSoldToAttention.Text = ""
            lbSoldToCity.Text = AccountRow.CITY : lbSoldToCountry.Text = AccountRow.COUNTRY : lbSoldToState.Text = AccountRow.STATE
            lbSoldToStreet.Text = AccountRow.ADDRESS : lbSoldToTel.Text = AccountRow.PHONE_NUM : lbSoldToZipcode.Text = AccountRow.ZIPCODE
            HFsoldtofax.Value = AccountRow.FAX_NUM
        End If
        'Dim dt As DataTable = SiebelTools.GET_Account_Info_By_ID(hQuoteToAccountRowId)
        'If dt.Rows.Count > 0 Then
        '    RBU = dt.Rows(0).Item("RBU") : siebelRBU = RBU
        'End If
        Dim _SalesOfficeCode As String = String.Empty, _SalesGroupCode As String = String.Empty
        If hQuoteErpId <> "" Then

            'Ryan 20160908 Get Currency by company_id and org_id
            curr = Business.GET_Currency_By_Company_ID(hQuoteErpId, drpOrg.SelectedValue)
            If _IsHQDCUser Then
                'curr = Business.GET_Currency_By_InterconCompany_ID(hQuoteErpId, drpOrg.SelectedValue)

                Dim _Engineer As String = Business.getDefaultEmployeeEMail(hQuoteErpId)
                Me.TBEngineer.Text = _Engineer
            Else
                'curr = Business.GET_Curr_By_Company_ID(hQuoteErpId)
            End If



            PutDLCurrency(curr, hQuoteErpId)
            Dim strGETRBU As String = String.Format(
                    " select isnull(SalesOfficeName,'') as RBU,isnull(org_id,'') AS org, isnull(inco1,'') as inco1, isnull(inco2,'') as inco2 " +
                    " from SAP_DIMCOMPANY where COMPANY_ID='{0}'", hQuoteErpId)
            Dim dt1 As New DataTable
            dt1 = tbOPBase.dbGetDataTable("B2B", strGETRBU)
            If dt1.Rows.Count > 0 Then
                RBU = dt1.Rows(0).Item("RBU")
                'setShipBillByERPID(hQuoteErpId, dt1.Rows(0).Item("org"))
                '20120713 TC: Get sold-to's address as default bill-to/ship-to
                'Dim SapCustDt As DataTable = SAPTools.SearchAllSAPCompanySoldBillShipTo(hQuoteErpId, dt1.Rows(0).Item("org"), "", "", "", txtSalesDivision.Text, "", "", "")

                'Dim SapCustAddrDt As Relics.SalesOrder.PartnerAddressesDataTable = Relics.SAPDAL.GetSAPPartnerAddressesTableByKunnr(hQuoteErpId, True)
                Dim SapCustAddrDt As SAPDAL.SalesOrder.PartnerAddressesDataTable = SAPDAL.SAPDAL.GetSAPPartnerAddressesTableByKunnr(hQuoteErpId, True)

                If SapCustAddrDt.Count > 0 Then
                    'Dim SapCustAddrRow As Relics.SalesOrder.PartnerAddressesRow = SapCustAddrDt(0)
                    Dim SapCustAddrRow As SAPDAL.SalesOrder.PartnerAddressesRow = SapCustAddrDt(0)
                    'Dim strDefaultStreet As String = "", strDefaultCity As String = "", strDefaultZip As String = "", strDefaultState As String = "", strDefaultCountry As String = ""
                    'Dim dtPartner As Relics.SalesOrder.SAP_BAPIPARNRDataTable = SAPTools.GetSAPPartnerTableByKunnr(hQuoteErpId)
                    'If dtPartner.Count > 0 Then
                    '    strDefaultStreet = dtPartner(0).STREET : strDefaultCity = dtPartner(0).CITY : strDefaultState = dtPartner(0)._REGION
                    '    strDefaultZip = dtPartner(0).POSTL_CODE : strDefaultCountry = dtPartner(0).COUNTRY
                    'End If
                    lbSoldToAttention.Text = SapCustAddrRow.C_O_Name : lbSoldToCity.Text = SapCustAddrRow.City : lbSoldToCountry.Text = SapCustAddrRow.Country
                    lbSoldToState.Text = SapCustAddrRow.Region_str : lbSoldToStreet.Text = SapCustAddrRow.Street : lbSoldToTel.Text = SapCustAddrRow.Tel1_Numbr : lbSoldToZipcode.Text = SapCustAddrRow.Postl_Cod1
                    HFsoldtofax.Value = SapCustAddrRow.Fax_Number
                    'Dim FstSApCustRow As DataRow = Nothing
                    'For Each custRow As DataRow In SapCustDt.Rows
                    '    If String.Equals(custRow.Item("company_id"), hQuoteErpId, StringComparison.CurrentCultureIgnoreCase) _
                    '        AndAlso Not String.IsNullOrEmpty(custRow.Item("SalesOffice")) AndAlso Not String.IsNullOrEmpty(custRow.Item("SalesGroup")) Then
                    '        FstSApCustRow = custRow : Exit For
                    '    End If
                    'Next
                    'If FstSApCustRow Is Nothing Then
                    '    For Each custRow As DataRow In SapCustDt.Rows
                    '        If String.Equals(custRow.Item("company_id"), hQuoteErpId, StringComparison.CurrentCultureIgnoreCase) Then
                    '            FstSApCustRow = custRow : Exit For
                    '        End If
                    '    Next
                    'End If

                    'Frank get Default ship-to party
                    Dim _strsql As New StringBuilder
                    _strsql.AppendLine(" Select top 1 PARENT_COMPANY_ID as ShipToERPID ")
                    _strsql.AppendLine(" From MyAdvantechGlobal.dbo.SAP_COMPANY_PARTNERS ")
                    _strsql.AppendLine(" Where COMPANY_ID='" & hQuoteErpId & "' and ORG_ID='EU10' and PARTNER_FUNCTION='WE' and DEFPA='X' ")
                    Dim sid As Object = tbOPBase.dbExecuteScalar("B2B", _strsql.ToString)
                    Dim shiptoERPID As String = hQuoteErpId
                    Dim _HavaDefault As Boolean = False
                    If Not sid Is Nothing AndAlso Not String.IsNullOrEmpty(sid.ToString.Trim) Then
                        _HavaDefault = True : shiptoERPID = sid.ToString.Trim
                    End If

                    If _HavaDefault Then
                        Dim SapCustAddrDt_Shipto As SAPDAL.SalesOrder.PartnerAddressesDataTable = SAPDAL.SAPDAL.GetSAPPartnerAddressesTableByKunnr(shiptoERPID, True)
                        Dim SapCustAddrRow_Shipto As SAPDAL.SalesOrder.PartnerAddressesRow = Nothing
                        If SapCustAddrDt_Shipto IsNot Nothing AndAlso SapCustAddrDt_Shipto.Rows.Count > 0 Then
                            SapCustAddrRow_Shipto = SapCustAddrDt_Shipto(0)
                        Else
                            SapCustAddrRow_Shipto = SapCustAddrRow : shiptoERPID = hQuoteErpId
                        End If
                        Me.txtShipToERPID.Text = shiptoERPID : Me.txtShipToName.Text = SapCustAddrRow_Shipto.Name
                        Me.txtShipToStreet.Text = SapCustAddrRow_Shipto.Street : Me.txtShipToCity.Text = SapCustAddrRow_Shipto.City
                        Me.txtShipToState.Text = SapCustAddrRow_Shipto.Region_str : Me.txtShipToStreet2.Text = SapCustAddrRow_Shipto.Str_Suppl3
                        Me.txtShipToZip.Text = SapCustAddrRow_Shipto.Postl_Cod1 : Me.txtShipToCountry.Text = SapCustAddrRow_Shipto.Country
                        Me.txtShipToAttention.Text = SapCustAddrRow_Shipto.C_O_Name : Me.txtShipToTel.Text = SapCustAddrRow_Shipto.Tel1_Numbr
                        HFshiptofax.Value = SapCustAddrRow_Shipto.Fax_Number
                    Else
                        'If FstSApCustRow Is Nothing Then FstSApCustRow = SapCustDt.Rows(0)
                        Me.txtShipToERPID.Text = hQuoteErpId : Me.txtShipToName.Text = SapCustAddrRow.Name
                        Me.txtShipToStreet.Text = SapCustAddrRow.Street : Me.txtShipToCity.Text = SapCustAddrRow.City
                        Me.txtShipToState.Text = SapCustAddrRow.Region_str : Me.txtShipToStreet2.Text = SapCustAddrRow.Str_Suppl3
                        Me.txtShipToZip.Text = SapCustAddrRow.Postl_Cod1 : Me.txtShipToCountry.Text = SapCustAddrRow.Country
                        Me.txtShipToAttention.Text = SapCustAddrRow.C_O_Name : Me.txtShipToTel.Text = SapCustAddrRow.Tel1_Numbr
                        HFshiptofax.Value = SapCustAddrRow.Fax_Number
                    End If




                    Me.txtBillID.Text = hQuoteErpId
                    'Dim billto As String = SAPDAL.SAPDAL.GetBillToNotSoldTo(hQuoteErpId)
                    Dim billto As String = SAPDAL.SAPDAL.GetBillToNotSoldTo(hQuoteErpId, drpOrg.SelectedValue)
                    If Not String.IsNullOrEmpty(billto) AndAlso billto <> hQuoteErpId Then
                        'Dim SapCustAddrDt2 As Relics.SalesOrder.PartnerAddressesDataTable = Relics.SAPDAL.GetSAPPartnerAddressesTableByKunnr(billto, True)
                        Dim SapCustAddrDt2 As SAPDAL.SalesOrder.PartnerAddressesDataTable = SAPDAL.SAPDAL.GetSAPPartnerAddressesTableByKunnr(billto, True)
                        If SapCustAddrDt2.Count > 0 Then
                            SapCustAddrRow = SapCustAddrDt2(0)
                            Me.txtBillID.Text = billto
                        End If
                    Else
                        Me.txtBillID.Text = hQuoteErpId
                    End If
                    Me.txtBillName.Text = SapCustAddrRow.Name
                    Me.txtBillToStreet.Text = SapCustAddrRow.Street : Me.txtBillToCity.Text = SapCustAddrRow.City : Me.txtBillToState.Text = SapCustAddrRow.Region_str
                    Me.txtBillToStreet2.Text = SapCustAddrRow.Str_Suppl3
                    Me.txtBillToZip.Text = SapCustAddrRow.Postl_Cod1 : Me.txtBillToCountry.Text = SapCustAddrRow.Country
                    Me.txtBillToAttention.Text = SapCustAddrRow.C_O_Name : Me.txtBillToTel.Text = SapCustAddrRow.Tel1_Numbr
                    HFbilltofax.Value = SapCustAddrRow.Fax_Number

                    Me.txtEMERPID.Text = String.Empty
                    If EndCustomerSettings(hQuoteErpId) Then
                        Dim EndCustomerID As String = String.Empty
                        Advantech.Myadvantech.Business.OrderBusinessLogic.HasSAPEndCustomer(hQuoteErpId, EndCustomerID)
                        If Not String.IsNullOrEmpty(EndCustomerID) AndAlso EndCustomerID <> hQuoteErpId Then
                            Dim SapCustAddrDtEM As SAPDAL.SalesOrder.PartnerAddressesDataTable = SAPDAL.SAPDAL.GetSAPPartnerAddressesTableByKunnr(EndCustomerID, True)
                            If SapCustAddrDtEM.Count > 0 Then
                                SapCustAddrRow = SapCustAddrDtEM(0)
                                Me.txtEMERPID.Text = EndCustomerID
                                Me.txtEMName.Text = SapCustAddrRow.Name
                                Me.txtEMStreet.Text = SapCustAddrRow.Street
                                Me.txtEMCity.Text = SapCustAddrRow.City
                                Me.txtEMState.Text = SapCustAddrRow.Region_str
                                Me.txtEMStreet2.Text = SapCustAddrRow.Str_Suppl3
                                Me.txtEMZip.Text = SapCustAddrRow.Postl_Cod1
                                Me.txtEMCountry.Text = SapCustAddrRow.Country
                                Me.txtEMAttention.Text = SapCustAddrRow.C_O_Name
                                Me.txtEMTel.Text = SapCustAddrRow.Tel1_Numbr
                                HFEMfax.Value = SapCustAddrRow.Fax_Number
                            End If
                        End If
                    End If

                    Dim dtGrpOfficeDivision As DataTable = SAPTools.GetSalesGrpOfficeDivisionDistrictByKunnr(hQuoteErpId)
                    If dtGrpOfficeDivision.Rows.Count > 0 Then
                        txtSalesDivision.Text = dtGrpOfficeDivision.Rows(0).Item("division").ToString.Trim
                        txtSalesGroup.Text = dtGrpOfficeDivision.Rows(0).Item("SalesGroup").ToString.Trim
                        'Frank 20160608
                        If _IsUSAENC AndAlso String.IsNullOrEmpty(txtSalesGroup.Text) Then
                            txtSalesGroup.Text = "230"
                        End If
                        txtSalesOffice.Text = dtGrpOfficeDivision.Rows(0).Item("SalesOffice").ToString.Trim
                        txtSalesDistrict.Text = dtGrpOfficeDivision.Rows(0).Item("District").ToString.Trim
                        _SalesOfficeCode = dtGrpOfficeDivision.Rows(0).Item("SalesOffice").ToString.Trim
                        _SalesGroupCode = dtGrpOfficeDivision.Rows(0).Item("SalesGroup").ToString.Trim
                    End If

                    txtInco2.Text = dt1.Rows(0).Item("inco2").ToString()
                    Dim creditCheckDt As DataTable = Nothing
                    If SAPTools.IsCustomerCreditBlock(drpOrg.SelectedValue, hQuoteErpId, dlDistChann.SelectedValue, txtSalesDivision.Text, creditCheckDt) = False Then
                        gvERPIDCreditBlockMsg.DataSource = creditCheckDt : gvERPIDCreditBlockMsg.DataBind() : trCreditBlockMsg.Visible = True
                        '20120727 TC: hide credit block message first and wait Cathee's confirmation
                        trCreditBlockMsg.Visible = False
                    End If
                End If
            End If
            'Me.txtShipAddresSiebel.Text = "" : Me.txtBillAddresSiebel.Text = ""
        Else
            curr = SiebelTools.GET_Curr_By_AccoutRow_id(hQuoteToAccountRowId, RBU)
            Me.txtShipToStreet.Text = "" : Me.txtShipToERPID.Text = "" : Me.txtShipToName.Text = "" : txtShipToAttention.Text = "" : txtShipToCity.Text = "" : txtShipToCountry.Text = "" : txtShipToState.Text = "" : txtShipToZip.Text = "" : txtShipToTel.Text = ""
            Me.txtBillToStreet.Text = "" : Me.txtBillID.Text = "" : Me.txtBillName.Text = "" : txtBillToAttention.Text = "" : txtBillToCity.Text = "" : txtBillToCountry.Text = "" : txtBillToState.Text = "" : txtBillToZip.Text = "" : txtBillToTel.Text = ""
            Me.txtEMStreet.Text = "" : Me.txtEMERPID.Text = "" : Me.txtEMName.Text = "" : txtEMAttention.Text = "" : txtEMCity.Text = "" : txtEMCountry.Text = "" : txtEMState.Text = "" : txtEMZip.Text = "" : txtEMTel.Text = ""

            If _IsUSAUser Then
                curr = "USD"
            ElseIf Role.IsFranchiser() Then
                curr = "USD"
            ElseIf _IsKRAonlineUser Then
                curr = "KRW"
            End If
        End If

        If siebelRBU = "" Then siebelRBU = "ANADMF"
        'Ryan 20161116 Set Siebel RBU to specific value for JP
        If Me._IsJPAonlineUser Then siebelRBU = "AJP"

        'Frank 2013/04/02
        Dim _SalesOffice As String = RBU
        If Me._IsJPAonlineUser Then _SalesOffice = Me.drpSalesOffice.SelectedValue


        SetValToForm(Me.txtCustomId.Text, Me.txtCreatedBy.Text, Me.txtQuoteDate.Text, Me.txtPreparedBy.Text,
                     oQuoteToName, hQuoteToAccountRowId, hQuoteErpId, _SalesOffice,
                     curr, oSalesEmail, hSalesRowId, oDirectPhone,
                     Me.txtAttention.Text, Me.hfAttentionRowId.Value, Me.txtBankInfo.Text, Me.txtDeliveryDate.Text, Me.txtExpiredDate.Text,
                     Me.dlShipTerm.SelectedValue, Me.dlPaymentTerm.SelectedValue, Me.txtFreight.Text, Me.txtInsurance.Text, Me.txtSpecialCharge.Text,
                     Me.txtTax.Text, Me.txtQuoteNote.Text, Me.txtRelatedInfo.Text, IIf(Me.cbxIsRepeatedOrder.Checked, 1, 0), Me.drpGroup.SelectedValue,
                     IIf(Me.cbxDelDateFlag.Checked, 1, 0), Me.drpOrg.SelectedValue, siebelRBU, _SalesOfficeCode, _SalesGroupCode,
                     Me.hfQuoteNo.Value, Me.DDLRevisionNumber.SelectedValue, Me.cbxIsActive.Checked,
                     Me.txtEmailGreeting.Content, Me.txtSpecialTandC.Text, Me.HFSignatureID.Value, Me.txtWarranty.Text, Me.ddlABRQuoteType.SelectedValue, Me.hfCreatedDate.Value)

        getOrg() : fillBlankOrAccount(Me.h_office.Value) : getSalesEmployee() : getShipTerm() : getPayMentTerm() : loadTaxSetting(Me.hfErpId.Text.Trim.ToUpper) : Me.UPForm.Update() : Me.MPPickAccount.Hide()


        'Frank 20150803 Determine sales office and group code of US AOnline's quote by
        ' 1. sales employee
        ' 2. prepare for
        ' 3. creator
        If _IsUSAonline Then
            Dim _USSalesOfficeCode As String = String.Empty
            Dim _USSalesGroupCode As String = String.Empty

            setOfficeAndGroupCode(_USSalesOfficeCode, _USSalesGroupCode)

            Me.txtSalesOffice.Text = _USSalesOfficeCode
            Me.txtSalesGroup.Text = _USSalesGroupCode
        End If

        'Ryan 20170822 Check customer credit info and show fancybox if neccesary.
        If _IsJPAonlineUser Then
            Me.ascxCustomerCreditInfo.RequestID = hQuoteErpId
            Me.ascxCustomerCreditInfo.showCreditInfo(hQuoteErpId)
            If Me.ascxCustomerCreditInfo.isBalanceExpired = True Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(Page), "Script", "ShowFancyBox();", True)
            End If
        End If


    End Sub

    Protected Sub setOfficeAndGroupCode(ByRef SalesOfficeCode As String, ByRef SalesGroupCode As String)

        Dim _salescode As String = Me.drpSE.SelectedValue
        If Not String.IsNullOrEmpty(_salescode) Then
            Dim dt As DataTable = tbOPBase.dbGetDataTable("B2B", "Select SALES_CODE,isnull(SALESOFFICE,'') as SALESOFFICE, isnull(SALESGROUP,'') as SALESGROUP From SAP_EMPLOYEE where SALES_CODE='" & _salescode & "'")
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                SalesOfficeCode = dt.Rows(0).Item("SALESOFFICE")
                SalesGroupCode = dt.Rows(0).Item("SALESGROUP")
                Exit Sub
            End If
        End If

        If Not String.IsNullOrEmpty(Me.txtSalesEmail.Text) Then
            Dim dt As DataTable = tbOPBase.dbGetDataTable("B2B", "Select SALES_CODE,isnull(SALESOFFICE,'') as SALESOFFICE, isnull(SALESGROUP,'') as SALESGROUP From SAP_EMPLOYEE where EMAIL='" & Me.txtSalesEmail.Text.Replace("'", "''") & "'")
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                SalesOfficeCode = dt.Rows(0).Item("SALESOFFICE")
                SalesGroupCode = dt.Rows(0).Item("SALESGROUP")
                Exit Sub
            End If
        End If

        If Not String.IsNullOrEmpty(Me.txtCreatedBy.Text) Then
            Dim dt As DataTable = tbOPBase.dbGetDataTable("B2B", "Select SALES_CODE,isnull(SALESOFFICE,'') as SALESOFFICE, isnull(SALESGROUP,'') as SALESGROUP From SAP_EMPLOYEE where EMAIL='" & Me.txtCreatedBy.Text.Replace("'", "''") & "'")
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                SalesOfficeCode = dt.Rows(0).Item("SALESOFFICE")
                SalesGroupCode = dt.Rows(0).Item("SALESGROUP")
                Exit Sub
            End If
        End If

    End Sub

    Public Sub PickAttentionEnd(ByVal str As Object)
        hAttentionRowId = str(0).ToString
        oAttention = str(1).ToString
        'Frank 2013/04/03:Sales Office user control was changed to drop down list
        SetValToForm(Me.txtCustomId.Text, Me.txtCreatedBy.Text, Me.txtQuoteDate.Text, Me.txtPreparedBy.Text,
                     Me.txtQuoteToName.Text, Me.hfAccountRowId.Value, Me.hfErpId.Text, Me.drpSalesOffice.SelectedValue,
                     Me.drpCurrency.SelectedValue, Me.txtSalesEmail.Text, Me.hfSalesRowId.Value, Me.txtDirectPhone.Text,
                     oAttention, hAttentionRowId, Me.txtBankInfo.Text, Me.txtDeliveryDate.Text, Me.txtExpiredDate.Text,
                     Me.dlShipTerm.SelectedValue, Me.dlPaymentTerm.SelectedValue, Me.txtFreight.Text, Me.txtInsurance.Text,
                     Me.txtSpecialCharge.Text, Me.txtTax.Text, Me.txtQuoteNote.Text, Me.txtRelatedInfo.Text, IIf(Me.cbxIsRepeatedOrder.Checked, 1, 0),
                     Me.drpGroup.SelectedValue, IIf(Me.cbxDelDateFlag.Checked, 1, 0), Me.drpOrg.SelectedValue, Me.h_office.Value _
                     , Me.drpSalesOffice.SelectedValue, Me.drpGroup.SelectedValue, Me.hfQuoteNo.Value, Me.DDLRevisionNumber.SelectedValue, Me.cbxIsActive.Checked _
                     , Me.txtEmailGreeting.Content, Me.txtSpecialTandC.Text, Me.HFSignatureID.Value, Me.txtWarranty.Text, Me.ddlABRQuoteType.SelectedValue, Me.hfCreatedDate.Value)
        Me.UPForm.Update()
        Me.MPPickAttention.Hide()
    End Sub

    Protected Sub ibtnPickAttention_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CType(Me.ascxPickAttention.FindControl("txtID"), TextBox).Text = Me.hfAccountRowId.Value
        Me.ascxPickAttention.ShowData(Me.hfAccountRowId.Value, "") : Me.UPPickAttention.Update() : Me.MPPickAttention.Show()
    End Sub


    Sub fillBlankOrAccount(ByVal RBU As String)
        oBankAccount = Business.getBankInfoByRBU(RBU)
        'Frank 2013/04/03:Sales Office user control was changed to drop down list
        SetValToForm(Me.txtCustomId.Text, Me.txtCreatedBy.Text, Me.txtQuoteDate.Text, Me.txtPreparedBy.Text,
             Me.txtQuoteToName.Text, Me.hfAccountRowId.Value, Me.hfErpId.Text, Me.drpSalesOffice.SelectedValue,
             Me.drpCurrency.SelectedValue, Me.txtSalesEmail.Text, Me.hfSalesRowId.Value, Me.txtDirectPhone.Text,
             Me.txtAttention.Text, Me.hfAttentionRowId.Value, oBankAccount, Me.txtDeliveryDate.Text, Me.txtExpiredDate.Text,
             Me.dlShipTerm.SelectedValue, Me.dlPaymentTerm.SelectedValue, Me.txtFreight.Text, Me.txtInsurance.Text,
             Me.txtSpecialCharge.Text, Me.txtTax.Text, Me.txtQuoteNote.Text, oRelatedInfo, IIf(Me.cbxIsRepeatedOrder.Checked, 1, 0),
             Me.drpGroup.SelectedValue, IIf(Me.cbxDelDateFlag.Checked, 1, 0), Me.drpOrg.SelectedValue, Me.h_office.Value _
             , Me.drpSalesOffice.SelectedValue, Me.drpGroup.SelectedValue, Me.hfQuoteNo.Value, Me.DDLRevisionNumber.SelectedValue, Me.cbxIsActive.Checked,
             Me.txtEmailGreeting.Content, Me.txtSpecialTandC.Text, Me.HFSignatureID.Value, Me.txtWarranty.Text, Me.ddlABRQuoteType.SelectedValue, Me.hfCreatedDate.Value)
    End Sub

    Protected Sub UPForm_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        If SiebelTools.IsCanRepeatOrder(hQuoteToAccountRowId) Then
            Me.trRepeatedOrder.Visible = True
        Else
            Me.trRepeatedOrder.Visible = False
        End If
    End Sub

    Private Sub pupupMPOPTY(ByVal _tabid As Integer)
        Dim _accrowid As String = Me.hfAccountRowId.Value
        CType(Me.ascxPickopty1.FindControl("h_rowid"), HiddenField).Value = _accrowid
        'Ming20141128如果已關聯了一個opty，當再次按下pick按鈕時，Project Name不用帶入textbox，然後查出此account 所有的opty。不然怕使用者誤以為只有這一個opty可以用
        CType(ascxPickopty1.FindControl("HFoptyID"), HiddenField).Value = txtOptyRowID.Text
        Me.ascxPickopty1.ShowData(_accrowid, "") : Me.ascxPickopty1.SetTabSelectedIndex(_tabid)
        'CType(ascxPickopty1.FindControl("TabContainer1").FindControl("tbPickOpportunity").FindControl("txtSearchOptyName"), TextBox).Text = txtOptyName.Text

        '===Ryan 20171005 Comment out AEU copy quotation opty logic.===
        'Ryan 20161228 Set opty ascx tabs visibilty according to copy purpose
        'If _IsEUUser AndAlso oCopyPurpose <> 0 Then
        '    If oCopyPurpose = 1 Then
        '        'oCopyPurpose = 1 means user wants to create new quotation from copy, needs to create new opty, hide pick tab in ascx
        '        Me.ascxPickopty1.DisableTabsbyIndex(0)
        '    ElseIf oCopyPurpose = 2 Then
        '        'oCopyPurpose = 2 means user wants to revise existing quotation from copy, needs to pick existing opty, hide new opty tab 
        '        Me.ascxPickopty1.DisableTabsbyIndex(1)
        '    Else
        '        'oCopyPurpose = 0 means new quotation, no need to do anything
        '    End If
        'End If
        '===End 20171005 Comment out===

        Me.UPPickOpty.Update() : Me.MPPickOpty.Show()
    End Sub

    Protected Sub ibtn_PickOpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_PickOpty.Click
        pupupMPOPTY(0)
    End Sub

    Protected Sub ButtonNewOpty_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        pupupMPOPTY(1)
    End Sub



    Public Sub PickOptyEnd(ByVal optyid As Object, ByVal optyName As Object, ByVal optyStage As Object)
        Dim Id As String = optyid, Name As String = optyName, Stage As String = optyStage

        Me.txtOptyRowID.Text = Id : Me.txtOptyName.Text = Name : Me.UP_Opty.Update() : Me.MPPickOpty.Hide()
        If Not String.IsNullOrEmpty(optyStage) Then
            Me.DDLOptyStage.SelectedValue = optyStage
        End If
        'Me.LabelOptyRowID.Text = Id : Me.LabelOptyName.Text = Name : Me.LabelOptyStage.Text = Stage
        'Me.UP_Opty.Update() : Me.MP1.Hide()
    End Sub


    'Protected Sub ibtn_PickOpty_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    CType(Me.ascxPickOpty.FindControl("h_rowid"), HiddenField).Value = Me.hfAccountRowId.Value
    '    Me.ascxPickOpty.ShowData(Me.hfAccountRowId.Value) : Me.UPPickOpty.Update() : Me.MPPickOpty.Show()
    'End Sub

    'Public Sub PickOptyEnd(ByVal str As Object)
    '    Dim Id As String = str(0).ToString, Name As String = str(1).ToString
    '    Me.txtOptyRowID.Text = Id : Me.txtOptyName.Text = Name : Me.UP_Opty.Update() : Me.MPPickOpty.Hide()
    'End Sub

    Protected Sub cbx_NewOpty_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If CType(sender, CheckBox).Checked Then
            Me.txtOptyRowID.Text = "new ID"
        Else
            Me.txtOptyRowID.Text = ""
        End If
        Me.txtOptyName.Text = "" : Me.UP_Opty.Update()
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Loadsetting(Me.h_office.Value)
        If String.Equals(txtOptyRowID.Text.Trim, "new ID") Then
            Me.txtOptyName.BackColor = Drawing.ColorTranslator.FromHtml("#ebebe4") : Me.cbx_NewOpty.Checked = True
            Me.LabelOptyName.Visible = True : Me.txtOptyName.Visible = True
            Me.LabelOptyStage.Visible = True : Me.DDLOptyStage.Visible = True
        Else
            Me.txtOptyName.BackColor = Drawing.ColorTranslator.FromHtml("#ebebe4") : Me.cbx_NewOpty.Checked = False
            Me.LabelOptyName.Visible = True : Me.txtOptyName.Visible = True
            Me.LabelOptyStage.Visible = False : Me.DDLOptyStage.Visible = False
        End If


        If Me.hfErpId.Text <> "" Or _IsUSAUser Then
            Me.tbSBSAP.Visible = True 'Me.tbSBSiebel.Visible = False
        Else
            Me.tbSBSAP.Visible = False 'Me.tbSBSiebel.Visible = True
        End If

        If Me.dlDistChann.SelectedValue <> "" Or (Not _IsUSAUser) Then
            Me.trSB.Visible = True
        Else
            Me.trSB.Visible = False
        End If
    End Sub


    Public Sub PickSHIPEnd(ByVal str As Object)
        'Me.txtShipToERPID.Text = str(0).ToString : Me.txtShipToName.Text = str(1).ToString
        'Me.txtShipToStreet.Text = str(2).ToString
        Dim strPickedErpId As String = str(0).ToString
        'Dim PtnrDt As Relics.SalesOrder.SAP_BAPIPARNRDataTable = SAPTools.GetSAPPartnerTableByKunnr(strPickedErpId)
        'Dim PtnrAddrDt As Relics.SalesOrder.PartnerAddressesDataTable = Relics.SAPDAL.GetSAPPartnerAddressesTableByKunnr(strPickedErpId)
        Dim PtnrAddrDt As SAPDAL.SalesOrder.PartnerAddressesDataTable = SAPDAL.SAPDAL.GetSAPPartnerAddressesTableByKunnr(strPickedErpId)
        If PtnrAddrDt.Count > 0 Then
            With PtnrAddrDt(0)
                Me.txtShipToERPID.Text = strPickedErpId : Me.txtShipToName.Text = .Name : Me.txtShipToStreet.Text = .Street : Me.txtShipToCity.Text = .City
                Me.txtShipToState.Text = .Region_str : Me.txtShipToTel.Text = .Tel1_Numbr : Me.txtShipToZip.Text = .Postl_Cod1 : Me.txtShipToAttention.Text = .C_O_Name
                Me.txtShipToStreet2.Text = .Str_Suppl3
                Me.txtShipToCountry.Text = .Country
            End With
            loadTaxSetting(strPickedErpId)
        End If
        Me.UPForm.Update()
    End Sub
    Public Sub PickBILLEnd(ByVal str As Object)
        'Me.txtBillID.Text = str(0).ToString : Me.txtBillName.Text = str(1).ToString
        'Me.txtBillToStreet.Text = str(2).ToString
        Dim strPickedErpId As String = str(0).ToString
        'Dim PtnrAddrDt As Relics.SalesOrder.PartnerAddressesDataTable = Relics.SAPDAL.GetSAPPartnerAddressesTableByKunnr(strPickedErpId)
        Dim PtnrAddrDt As SAPDAL.SalesOrder.PartnerAddressesDataTable = SAPDAL.SAPDAL.GetSAPPartnerAddressesTableByKunnr(strPickedErpId)
        If PtnrAddrDt.Count > 0 Then
            With PtnrAddrDt(0)
                Me.txtBillID.Text = strPickedErpId : Me.txtBillName.Text = .Name : Me.txtBillToStreet.Text = .Street : Me.txtBillToCity.Text = .City
                Me.txtBillToState.Text = .Region_str : Me.txtBillToTel.Text = .Tel1_Numbr : Me.txtBillToZip.Text = .Postl_Cod1 : Me.txtBillToAttention.Text = .C_O_Name
            End With
        End If
        Me.UPForm.Update()
    End Sub

    Public Sub PickEMEnd(ByVal str As Object)
        Dim strPickedErpId As String = str(0).ToString
        Dim PtnrAddrDt As SAPDAL.SalesOrder.PartnerAddressesDataTable = SAPDAL.SAPDAL.GetSAPPartnerAddressesTableByKunnr(strPickedErpId)
        If PtnrAddrDt.Count > 0 Then
            With PtnrAddrDt(0)
                Me.txtEMERPID.Text = strPickedErpId : Me.txtEMName.Text = .Name : Me.txtEMStreet.Text = .Street : Me.txtEMCity.Text = .City
                Me.txtEMState.Text = .Region_str : Me.txtEMTel.Text = .Tel1_Numbr : Me.txtEMZip.Text = .Postl_Cod1 : Me.txtEMAttention.Text = .C_O_Name
            End With
        End If
        Me.UPForm.Update()
    End Sub
    Protected Sub ibtnPickShipTo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPickShipTo.Click
        CType(Me.PickSB.FindControl("hType"), HiddenField).Value = "SHIP"
        CType(Me.PickSB.FindControl("hPERPID"), HiddenField).Value = Me.hfErpId.Text
        CType(Me.PickSB.FindControl("txtERPID"), TextBox).Text = Me.hfErpId.Text
        CType(Me.PickSB.FindControl("hORG"), HiddenField).Value = Me.drpOrg.SelectedValue
        CType(Me.PickSB.FindControl("hdDistChannel"), HiddenField).Value = dlDistChann.SelectedValue
        CType(Me.PickSB.FindControl("hdDivision"), HiddenField).Value = Trim(txtSalesDivision.Text)
        CType(Me.PickSB.FindControl("hdSalesGroup"), HiddenField).Value = Trim(txtSalesGroup.Text)
        CType(Me.PickSB.FindControl("hdSalesOffice"), HiddenField).Value = Trim(txtSalesOffice.Text)
        CType(Me.PickSB.FindControl("hIsAll"), HiddenField).Value = "Y"
        Me.PickSB.PreSettings()
        Me.PickSB.ShowData()
        Me.UPSB.Update() : Me.MPSB.Show()
    End Sub

    Protected Sub ibtnPickBill_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CType(Me.PickSB.FindControl("hType"), HiddenField).Value = "BILL"
        CType(Me.PickSB.FindControl("hPERPID"), HiddenField).Value = Me.hfErpId.Text
        CType(Me.PickSB.FindControl("txtERPID"), TextBox).Text = Me.hfErpId.Text
        CType(Me.PickSB.FindControl("hORG"), HiddenField).Value = Me.drpOrg.SelectedValue
        CType(Me.PickSB.FindControl("hdDistChannel"), HiddenField).Value = dlDistChann.SelectedValue
        CType(Me.PickSB.FindControl("hdDivision"), HiddenField).Value = Trim(txtSalesDivision.Text)
        CType(Me.PickSB.FindControl("hdSalesGroup"), HiddenField).Value = Trim(txtSalesGroup.Text)
        CType(Me.PickSB.FindControl("hdSalesOffice"), HiddenField).Value = Trim(txtSalesOffice.Text)
        CType(Me.PickSB.FindControl("hIsAll"), HiddenField).Value = "Y"
        Me.PickSB.PreSettings()
        Me.PickSB.ShowData()
        Me.UPSB.Update() : Me.MPSB.Show()
    End Sub

    Protected Sub ibtnPickEM_Click(sender As Object, e As ImageClickEventArgs)
        CType(Me.PickSB.FindControl("hType"), HiddenField).Value = "EM"
        CType(Me.PickSB.FindControl("hPERPID"), HiddenField).Value = Me.hfErpId.Text
        CType(Me.PickSB.FindControl("txtERPID"), TextBox).Text = Me.hfErpId.Text
        CType(Me.PickSB.FindControl("hORG"), HiddenField).Value = Me.drpOrg.SelectedValue
        CType(Me.PickSB.FindControl("hdDistChannel"), HiddenField).Value = dlDistChann.SelectedValue
        CType(Me.PickSB.FindControl("hdDivision"), HiddenField).Value = Trim(txtSalesDivision.Text)
        CType(Me.PickSB.FindControl("hdSalesGroup"), HiddenField).Value = Trim(txtSalesGroup.Text)
        CType(Me.PickSB.FindControl("hdSalesOffice"), HiddenField).Value = Trim(txtSalesOffice.Text)
        CType(Me.PickSB.FindControl("hIsAll"), HiddenField).Value = "Y"
        Me.PickSB.PreSettings()
        Me.PickSB.ShowData()
        Me.UPSB.Update() : Me.MPSB.Show()
    End Sub

    Protected Sub dlDistChann_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If dlDistChann.SelectedIndex > 0 Then
            tbDivSalesGrpOffice.Visible = True
        Else
            tbDivSalesGrpOffice.Visible = False
        End If
    End Sub

    Protected Sub cbNewBillTo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        txtBillName.ReadOnly = Not cbNewBillTo.Checked
        txtBillToStreet.ReadOnly = Not cbNewBillTo.Checked
        txtBillToStreet2.ReadOnly = Not cbNewBillTo.Checked
        txtBillToCity.ReadOnly = Not cbNewBillTo.Checked
        txtBillToState.ReadOnly = Not cbNewBillTo.Checked
        txtBillToZip.ReadOnly = Not cbNewBillTo.Checked
        txtBillToCountry.ReadOnly = Not cbNewBillTo.Checked
    End Sub

    Protected Sub cbNewShipTo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        txtShipToName.ReadOnly = Not cbNewShipTo.Checked
        txtShipToStreet.ReadOnly = Not cbNewShipTo.Checked
        txtShipToStreet2.ReadOnly = Not cbNewShipTo.Checked
        txtShipToCity.ReadOnly = Not cbNewShipTo.Checked
        txtShipToState.ReadOnly = Not cbNewShipTo.Checked
        txtShipToZip.ReadOnly = Not cbNewShipTo.Checked
        txtShipToCountry.ReadOnly = Not cbNewShipTo.Checked
    End Sub

    Protected Sub drpSalesOffice_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim _org As String = ""
        If _IsJPAonlineUser Then _org = "JP01"
        Me.LoadSalesGroupByOfficeCodeAndOrgId(Me.drpSalesOffice.SelectedValue, Me.drpOrg.SelectedValue)
    End Sub

    Protected Sub ButtonCreateNewRevision_Click(sender As Object, e As EventArgs) Handles ButtonCreateNewRevision.Click

        If Not IsNothing(Request("UID")) AndAlso Business.isEidtableQuote(Request("UID")) Then

            Dim NewQuoteID As String = String.Empty, ErrorStr As String = String.Empty
            Dim QuoteID As String = Request("UID")
            Dim retint As Integer = Business.CreateNewRevisionQuotation(QuoteID, Pivot.CurrentProfile.UserId, NewQuoteID, False, ErrorStr)
            If retint = 0 Then
                '    LabCopymessage.Text = ErrorStr
                '    BTCopyNext.Visible = False
                '    MPCopy.Show()
                '    Exit Sub
            Else

                If _IsABRUser Then
                    Dim ABRDiscount As Decimal = 0
                    Dim abrdiscountdt As DataTable = tbOPBase.dbGetDataTable("EQ", "select * from QuotationExtensionABR where quoteId = '" + NewQuoteID + "'")
                    If abrdiscountdt IsNot Nothing AndAlso abrdiscountdt.Rows.Count > 0 Then
                        ABRDiscount = Convert.ToDecimal(abrdiscountdt.Rows(0).Item("discount").ToString)
                    End If
                    Advantech.Myadvantech.Business.QuoteBusinessLogic.UpdateSystemPriceForABRQuote(NewQuoteID, ABRDiscount)
                End If

                Response.Redirect(String.Format("~/Quote/QuotationMaster.aspx?UID={0}", NewQuoteID))

                '    BTCopyNext.CommandArgument = NewQuoteID
                '    If String.IsNullOrEmpty(ErrorStr.Trim) Then
                '        BTCopyNext_Click(Me.BTCopyNext, Nothing)
                '    Else
                '        LabCopymessage.Text = ErrorStr
                '        BTCopyNext.Visible = True
                '        MPCopy.Show()
                '    End If
            End If


        End If




    End Sub

    Protected Sub DDLRevisionNumber_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DDLRevisionNumber.SelectedIndexChanged

        Dim _SelectedRevisionNumber As Integer = Me.DDLRevisionNumber.SelectedValue

        Dim QuoteDt As IBUS.iDocHeaderLine = Pivot.NewObjDocHeader.GetByQuoteNoandRevisionNumber(Me.hfQuoteNo.Value, _SelectedRevisionNumber, COMM.Fixer.eDocType.EQ)

        If QuoteDt IsNot Nothing Then
            Response.Redirect(String.Format("~/Quote/QuotationMaster.aspx?UID={0}", QuoteDt.Key))
        End If

    End Sub

    Protected Sub ButtonMakeItActive_Click1(sender As Object, e As EventArgs) Handles ButtonMakeItActive.Click
        Dim dth As IBUS.iDocHeader = Pivot.NewObjDocHeader
        dth.SetRevisionQuoteToActive(Request("UID"), COMM.Fixer.eDocType.EQ)
        Response.Redirect(String.Format("~/Quote/QuotationMaster.aspx?UID={0}", Request("UID")))
    End Sub

    Protected Sub ImgButtonPickSignature_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImgButtonPickSignature.Click
        pupupMPSignature()
    End Sub

    Private Sub pupupMPSignature()
        'CType(Me.ascxPickopty1.FindControl("h_rowid"), HiddenField).Value = _accrowid
        Me.ascxPickSignature1.ShowData()
        Me.UPPickSignature.Update() : Me.MPPickSignature.Show()
        'CType(ascxPickopty1.FindControl("TabContainer1").FindControl("tbPickOpportunity").FindControl("txtSearchOptyName"), TextBox).Text = txtOptyName.Text
        'Me.UPPickOpty.Update() : Me.MPPickOpty.Show()
    End Sub

    Public Sub PickSignatureEnd(ByVal SID As Object)

        Me.HFSignatureID.Value = "" : oSignatureRowID = ""
        If SID Is Nothing Then Exit Sub
        If SID.ToString Is String.Empty Then Exit Sub

        oSignatureRowID = SID.ToString
        Me.HFSignatureID.Value = oSignatureRowID

        Me.ShowSignature(oSignatureRowID)

        'If Not String.IsNullOrEmpty(oSignatureRowID) Then
        '    Me.imgSignature.ImageUrl = "~\Ascx\DisplayImageHandler.ashx?Type=signature&ImageID=" & ID
        '    Me.imgSignature.Visible = True
        'Else
        '    Me.imgSignature.Visible = False
        'End If
        'Me.UPForm.Update() : Me.MPPickSignature.Hide()
    End Sub

    Private Sub ShowSignature(ByVal SID As String)
        If Not String.IsNullOrEmpty(SID) Then
            Me.imgSignature.ImageUrl = "~\Ascx\DisplayImageHandler.ashx?Type=signature&ImageID=" & SID
            Me.imgSignature.Visible = True
            Me.ImgButtonRemoveSignature.Visible = True
        Else
            Dim sidobj As Object = tbOPBase.dbExecuteScalar("EQ", String.Format("select  top 1 SID  from  Signature where UserID ='{0}' AND Active =1  order by  LastUpdated DESC ", Pivot.CurrentProfile.UserId))
            If sidobj IsNot Nothing Then
                Me.imgSignature.ImageUrl = "~\Ascx\DisplayImageHandler.ashx?Type=signature&ImageID=" & sidobj.ToString.Trim
                Me.imgSignature.Visible = True
                Me.ImgButtonRemoveSignature.Visible = True
            Else
                Me.imgSignature.Visible = False
                Me.ImgButtonRemoveSignature.Visible = False
            End If


        End If
        Me.UPForm.Update() : Me.MPPickSignature.Hide()

    End Sub

    Protected Sub ImgButtonRemoveSignature_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ImgButtonRemoveSignature.Click
        Me.HFSignatureID.Value = "" : oSignatureRowID = ""
        Me.ShowSignature(oSignatureRowID)
    End Sub

    Private Sub btnUploadPO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadPO.Click
        If Me.updPO.FileBytes.Length > 0 Then
            SavePODoc(UID, Me.updPO.FileBytes)
        Else
            Util.showMessage("File to be uploaded is empty.")
        End If
    End Sub
    Sub SavePODoc(ByVal QuoteId As String, ByVal DocData As Byte())
        Dim PODOC As New DOCH.PODoc
        Dim PODOCLine As New DOCH.PODocLine
        PODOCLine.QuoteId = QuoteId
        PODOCLine.DocData = DocData
        PODOC.Add(PODOCLine)
    End Sub

    Public Function EndCustomerSettings(ByVal ERPID As String) As Boolean
        'Ryan 20170217 Move all end customer blocks logic to here.
        If _IsJPAonlineUser OrElse _IsEUUser OrElse _IsTWAonlineUser OrElse _IsHQDCUser Then
            'Ryan 20170216 JP01 will show EM ascx anyway, others will need to check if ERPID has maintained EM or not.
            If _IsJPAonlineUser Then
                tdEM.Visible = True
                Return True
            Else
                Dim HasSAPEM As Boolean = Advantech.Myadvantech.Business.OrderBusinessLogic.HasSAPEndCustomer(ERPID, "")
                If HasSAPEM Then
                    tdEM.Visible = True
                    Return True
                End If
            End If
        End If

        Return False
    End Function
End Class